using System.Text;
using AgroSolutions.Ingestion.Application.Interfaces;
using AgroSolutions.Ingestion.Application.Services;
using AgroSolutions.Ingestion.Domain.Interfaces;
using AgroSolutions.Ingestion.Infrastructure.Repositories;
using AgroSolutions.MessageBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Prometheus;
using AgroSolutions.Ingestion.API.Internal;
using Microsoft.Extensions.Caching.Memory;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<PropertiesInternalClient>(c =>
{
    c.BaseAddress = new Uri("http://properties-service");
});


var mongoConn = builder.Configuration["MongoDB:ConnectionString"] ?? "mongodb://mongodb:27017";
var mongo = new MongoClient(mongoConn).GetDatabase("agro_sensors");

builder.Services.AddSingleton<IMongoDatabase>(mongo);
builder.Services.AddScoped<ISensorReadingRepository, SensorReadingRepository>();

builder.Services.AddSingleton<IMessageBus>(new RabbitMqMessageBus(builder.Configuration["RabbitMQ:Host"] ?? "rabbitmq"));
builder.Services.AddScoped<IIngestionService, IngestionService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpMetrics();
app.MapMetrics();

app.MapControllers();
app.MapHealthChecks("/health");
app.Run();
