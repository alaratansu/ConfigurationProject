using System.Text;
using System.Text.Json;
using Configuration.Application.Messaging;
using Configuration.Domain.Entities;
using Configuration.Infrastructure.Messaging;
using Configuration.Infrastructure.Services;
using Configuration.Persistence;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Configuration.Library;

public class ConfigurationReader
{
    private readonly string applicationName;
    private readonly string connectionString;
    private readonly int refreshTimerIntervalInMs;
    private readonly string redisKey;
    
    private IRedisCacheService? RedisCacheService => RedisCacheAccessor.Instance;
    private readonly Dictionary<string, ConfigurationItem> configCache = new();
    private readonly System.Timers.Timer refreshTimer;

    public ConfigurationReader(
        string applicationName,
        string connectionString,
        int refreshTimerIntervalInMs)
    {
        this.applicationName = applicationName;
        this.connectionString = connectionString;
        this.refreshTimerIntervalInMs = refreshTimerIntervalInMs;
        redisKey = $"config:{applicationName}";

        LoadConfigurations();
        refreshTimer = new System.Timers.Timer(refreshTimerIntervalInMs);
        refreshTimer.Elapsed += (_, _) => LoadConfigurations();
        refreshTimer.Start();
        
        StartListeningForChanges();
    }

    public T GetValue<T>(string key)
    {
        if (configCache.TryGetValue(key, out var item) && item.IsActive)
        {
            return (T)Convert.ChangeType(item.Value, typeof(T));
        }

        throw new KeyNotFoundException($"'{key}' konfigürasyonu bulunamadı.");
    }

    private void LoadConfigurations()
    {
        try
        {
            var options = new DbContextOptionsBuilder<ConfigurationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using var context = new ConfigurationDbContext(options);

            var configs = context.ConfigurationItems
                .Where(c => c.IsActive && c.ApplicationName == applicationName)
                .ToList();

            lock (configCache)
            {
                configCache.Clear();
                foreach (var item in configs)
                    configCache[item.Name] = item;
            }

            // Redis'e yaz
            RedisCacheService?.SetAsync(redisKey, configCache).Wait();
        }
        catch
        {
            // SQL erişilemezse Redis'ten oku
            var fallback = RedisCacheService?.GetAsync<Dictionary<string, ConfigurationItem>>(redisKey).Result;
            if (fallback is not null)
            {
                lock (configCache)
                {
                    configCache.Clear();
                    foreach (var kv in fallback)
                        configCache[kv.Key] = kv.Value;
                }
            }
        }
    }
    
    private void StartListeningForChanges()
    {
        Task.Run(() =>
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare("config_exchange", ExchangeType.Fanout);

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName, exchange: "config_exchange", routingKey: "");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (_, ea) =>
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                var evt = JsonSerializer.Deserialize<ConfigurationChangedEvent>(body);
                if (evt?.Item.ApplicationName == applicationName)
                {
                    LoadConfigurations();
                }
            };

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        });
    }

}
