using System.Reflection;
using FibonacciCalculatorA.Api;
using FibonacciCalculatorA.Api.Consumers;
using FibonacciCalculatorA.Application.Services;
using FibonacciCalculatorA.Infrastructure.ExternalServices;
using MassTransit;
using Microsoft.OpenApi.Models;
using Refit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddSingleton<Common.FibonacciSequenceNumberCalculator>();
builder.Services.AddScoped<FibonacciSequencesManager>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<FibonacciSequenceNumberCalculatedConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Durable = true;
        cfg.PurgeOnStartup = false;
        cfg.ReceiveEndpoint("fibonacci_queue", ep =>
        {
            ep.PrefetchCount = 16;
            ep.ConfigureConsumer<FibonacciSequenceNumberCalculatedConsumer>(context);
        });
    });
});

builder.Services
    .AddRefitClient<IRefitFibonacciCalculatorB>()
    .ConfigureHttpClient(
        client =>
        {
            client.BaseAddress = new Uri("http://localhost:5122/");
        });


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseRouting();
app.Run();
