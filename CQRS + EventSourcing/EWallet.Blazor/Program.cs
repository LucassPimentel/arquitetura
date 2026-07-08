using EWallet.Blazor.Components;
using EWallet.Infrastructure.Context;
using EWallet.Infrastructure.DependencyInjection;
using EWallet.Infrastructure.Persistance.Event_Store;
using EWallet.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<EWalletDbContext>(options => options.UseSqlServer(connectionString));

// Event Store - persistência síncrona dos eventos
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();

// MongoDB (read model)
builder.Services.AddMongoDb(options => builder.Configuration.GetSection("Mongo").Bind(options));

// MediatR - registra handlers do assembly Application
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(EWallet.Application.Commands.CreateAccountCommand).Assembly));

// RabbitMQ - publisher (consumer roda no Worker Service separado)
builder.Services.AddRabbitMqPublisher(settings =>
    builder.Configuration.GetSection("RabbitMq").Bind(settings));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
