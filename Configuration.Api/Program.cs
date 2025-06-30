using Configuration.Application.Common.Interface;
using Configuration.Application.Common.Mappings;
using Configuration.Application.ConfigurationItems.Commands;
using Configuration.Application.ConfigurationItems.Queries;
using Configuration.Infrastructure;
using Configuration.Infrastructure.Messaging;
using Configuration.Library;
using FluentValidation;
using Configuration.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetConfigurationItemsQueryHandler).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddPersistence(connectionString);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
builder.Services.AddSingleton(new ConfigurationReader(
        applicationName: "SERVICE-A",
        connectionString: connectionString!,
        refreshTimerIntervalInMs: 30000)
);

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddValidatorsFromAssemblyContaining
    <CreateConfigurationItemCommandValidator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
    
    const int maxRetries = 3;
    int retries = 0;
    while (true)
    {
        try
        {
            ConfigurationDbInitializer.Initialize(context);
            break;
        }
        catch (Exception ex)
        {
            retries++;
            Console.WriteLine($"❌ MSSQL bağlantı hatası (deneme {retries}): {ex.Message}");

            if (retries >= maxRetries)
            {
                Console.WriteLine("❌ Maksimum tekrar sayısına ulaşıldı. Devam edilemiyor.");
                break; 
            }

            await Task.Delay(3000);
        }
    }
}


app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();