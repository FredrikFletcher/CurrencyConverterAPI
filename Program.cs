
using CurrencyConverterApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CurrencyConverterApi.Services; 
using CurrencyConverterApi.Services.Contracts;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); // Register controllers for dependency injection
builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateService>(); // Register HttpClient for IExchangeRateService
builder.Services.AddSingleton<UpdateRatesService>(); // Register UpdateRatesService as a singleton to ensure it runs once per application lifetime
builder.Services.AddEndpointsApiExplorer(); // Enables endpoint discovery for API controllers
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Currency Converter API",
        Version = "v1"
    });
});

var app = builder.Build();

app.Services.GetRequiredService<UpdateRatesService>();
// Middleware
if (app.Environment.IsDevelopment()) // Only enable Swagger in development
{
    app.UseSwagger(); // Enable Swagger UI
    app.UseSwaggerUI(); // Use Swagger UI to visualize and test the API
}

using (var scope = app.Services.CreateScope())
{
    var exchangeRateService = scope.ServiceProvider.GetRequiredService<IExchangeRateService>();
}

//app.UseHttpsRedirection(); // standard but not in use for local development

app.MapControllers(); 

app.Run();
