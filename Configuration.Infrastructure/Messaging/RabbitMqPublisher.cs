using System.Text;
using System.Text.Json;
using Configuration.Application.Common;
using Configuration.Application.Common.Interface;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Configuration.Infrastructure.Messaging;

public class RabbitMqPublisher : IMessagePublisher
{
    private readonly ConnectionFactory _factory;
    private readonly string _exchangeName;
    
    public RabbitMqPublisher(IConfiguration config)
    {
        _factory = new ConnectionFactory
        {
            HostName = config["RabbitMq:HostName"],
            Port = int.Parse(config["RabbitMq:Port"] ?? "5672"),
            UserName = config["RabbitMq:UserName"],
            Password = config["RabbitMq:Password"]
        };

        _exchangeName = config["RabbitMq:Exchange"] ?? "config_exchange";
    }

    public void Publish<T>(T message)
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        channel.BasicPublish(exchange: _exchangeName, routingKey: "", body: body);
    }
}
