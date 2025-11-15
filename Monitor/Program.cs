using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Monitor.Factories;
using Monitor.Models.Settings;
using Monitor.Repositories;
using Monitor.Repositories.Inretfaces;
using Monitor.Services;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);
builder.Services.AddSingleton(appSettings);

builder.Services.AddControllers();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddHealthChecks();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPostgresConnection, PostgresConnection>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILogsRepository, LogsRepository>();
builder.Services.AddScoped<IHealthDbRepository, HealthPostgresRepository>();

builder.Services.AddHostedService<BackgroundGenerateLogService>();

var app = builder.Build();

app.MapHealthChecks("/health");

app.UseHttpMetrics();
app.MapMetrics();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseStaticFiles();
app.UseRouting();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
