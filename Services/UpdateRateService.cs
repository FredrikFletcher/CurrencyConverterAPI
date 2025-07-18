using System;
using System.Threading;
using System.Threading.Tasks;
using CurrencyConverterApi.Services.Contracts;
namespace CurrencyConverterApi.Services;

public class UpdateRatesService
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly TimeSpan _updateInterval = TimeSpan.FromHours(1); // Update interval for exchange rates

    public UpdateRatesService(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService; // Inject the exchange rate service
        var timer = new Timer(UpdateRatesAsync, null, TimeSpan.Zero, _updateInterval); // Initialize the timer to update rates periodically
    }


    public void UpdateRatesAsync(object? state) // Timer callback method to update rates periodically, not supported with async
    {
        Console.WriteLine($"[API] Scheduled update of exchange rates at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
        _ = Task.Run(async () => // Run the update in a separate task to avoid blocking the timer
        {

            try
            {
                await _exchangeRateService.GetRateAsync(); // Fetch and update exchange rates
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating exchange rates: " + ex.Message);
            }


        });

    }
}