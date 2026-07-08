using EWallet.Infrastructure.Context;
using EWallet.Infrastructure.Messaging.Settings;
using EWallet.Worker.Consumers;
using EWallet.Infrastructure.DependencyInjection;
using EWallet.Worker.Projections;
using Microsoft.EntityFrameworkCore;
using EWallet.Worker.Projections.Interfaces;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    var connectionString = context.Configuration.GetConnectionString("DefaultConnection")
                           ?? "Server=db,1433;Database=EWalletDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;";

    services.AddDbContext<EWalletDbContext>(options => options.UseSqlServer(connectionString));

    var rabbitSettings = new RabbitMqSettings();
    context.Configuration.GetSection("RabbitMq").Bind(rabbitSettings);

    services.AddSingleton(rabbitSettings);
    services.AddSingleton<RabbitMqConnection>();

    // MongoDB (read model)
    services.AddMongoDb(options => context.Configuration.GetSection("Mongo").Bind(options));

    // Projeções de eventos para o read model
    services.AddSingleton<IEventProjection, AccountCreatedProjection>();
    services.AddSingleton<IEventProjection, MoneyDepositedProjection>();
    services.AddSingleton<IEventProjection, MoneyTransferredProjection>();
    services.AddSingleton<IEventProjection, MoneyReceivedProjection>();
    services.AddSingleton<IEventProjection, MoneyRefundedProjection>();
    services.AddSingleton<IEventProjection, MoneyRefundReceivedProjection>();
    services.AddSingleton<IEventProjection, AccountBlockedProjection>();

    // Background service - consome eventos e hidrata a view materializada
    services.AddHostedService<ReadModelConsumer>();
});

var host = builder.Build();
host.Run();
