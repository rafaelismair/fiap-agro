using System.Text;
using AgroSolutions.Analysis.API.BackgroundServices;
using AgroSolutions.Analysis.Application.Rules;
using AgroSolutions.Analysis.Application.Services;
using AgroSolutions.Analysis.Domain.Interfaces;
using AgroSolutions.Analysis.Infrastructure.Repositories;
using AgroSolutions.Ingestion.Domain.Interfaces;
using AgroSolutions.Ingestion.Infrastructure.Repositories;
using AgroSolutions.MessageBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Prometheus;
using AgroSolutions.Analysis.API.Internal;
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

// repos
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<ISensorReadingRepository, SensorReadingRepository>();

// rules + engine
builder.Services.AddScoped<IAlertRule, DroughtAlertRule>();
builder.Services.AddScoped<IAlertRule, PestRiskAlertRule>();
builder.Services.AddScoped<AlertEngine>();

// bus + consumer
builder.Services.AddSingleton<IMessageBus>(new RabbitMqMessageBus(builder.Configuration["RabbitMQ:Host"] ?? "rabbitmq"));
builder.Services.AddHostedService<SensorDataConsumer>();

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
