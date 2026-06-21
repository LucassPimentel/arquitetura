using Microsoft.EntityFrameworkCore;
using Social.Application.Commands;
using Social.Infrastructure.Persistence;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register infrastructure (DbContext for SQL Server) from Social.Infrastructure
// This requires the Social.API project to reference Social.Infrastructure.
builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    Assembly.GetExecutingAssembly(),
    typeof(CreatePostCommand).Assembly));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SocialDbContext>();
    var maxRetries = 30;
    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            db.Database.Migrate();
            break;
        }
        catch (Exception)
        {
            if (i == maxRetries - 1) throw;
            System.Threading.Thread.Sleep(2000);
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
