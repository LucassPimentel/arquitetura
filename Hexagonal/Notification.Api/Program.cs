using Notification.Application.Common;
using Notification.Application.Factories;
using Notification.Application.Factories.Interfaces;
using Notification.Application.UseCases;
using Notification.Blazor.Services;
using Notification.Domain.Ports.In;
using Notification.Infrastructure.DependecyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Registrar infraestrutura
builder.Services.AddInfrastructure();

builder.Services.AddScoped<ChannelService>();
builder.Services.AddScoped<ISendNotificationUseCase, SendNotificationUseCase>();

// Abstract Factory: uma fábrica concreta por canal (cada uma produz a família de produtos)
builder.Services.AddScoped<IChannelNotificationFactory, EmailNotificationFactory>();
builder.Services.AddScoped<IChannelNotificationFactory, SmsNotificationFactory>();
builder.Services.AddScoped<IChannelNotificationFactory, WhatsAppNotificationFactory>();

// Resolver genérico "por canal" (compartilhado por gateways e fábricas)
builder.Services.AddScoped<IChannelResolver, ChannelResolver>();

// Resolver/fachada que seleciona a fábrica adequada pelo canal
builder.Services.AddScoped<INotificationFactory, NotificationFactory>();

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

app.MapRazorComponents<Notification.Blazor.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
