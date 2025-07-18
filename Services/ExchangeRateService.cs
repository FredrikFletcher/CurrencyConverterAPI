using System.Text.Json;
using System.Threading;
using CurrencyConverterApi.Services.Contracts;
namespace CurrencyConverterApi.Services.Contracts;

public class ExchangeRateService : IExchangeRateService
{
    private readonly HttpClient _httpClient; // HttpClient for making API requests

    private Dictionary<string, decimal> _savedRates = new(); // saved rates from the API
    private DateTime _lastUpdated = DateTime.MinValue; // Timestamp of the last update


    public ExchangeRateService(HttpClient httpClient) // Constructor injection for HttpClient
    {
        _httpClient = httpClient;

    }



    public async Task<decimal> GetConvertCurrencyAsync(string from, string to, decimal amount)
    {
        if (!_savedRates.ContainsKey(from) || !_savedRates.ContainsKey(to)) // Check if the currencies are in the saved rates
        {
            throw new Exception($"Conversion rate for {from} or {to} not found.");
        }
        if (_savedRates.Count == 0)
        {
                await GetRateAsync(); // Fetch rates if cache is empty or expired
        }


       
        
        if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to) || amount <= 0) //check for valid input parameters
        {
            throw new ArgumentException("Invalid input parameters.");
        }

        if (from == to) // If the source and target currencies are the same, return the amount as is
        {
            return amount;
        }
        else if (from == "EUR")
        {
            return amount * _savedRates[to]; // Convert from EUR to the target currency
        }
        else if (to == "EUR")
        {
            return amount / _savedRates[from]; // Convert from the source currency to EUR
        }
        else
        {
            // Convert from source currency to EUR, then from EUR to target currency
            decimal amountInEur = amount / _savedRates[from];
            return amountInEur * _savedRates[to];
        }



    }

    public async Task GetRateAsync()
    {
        try
        {
            Console.WriteLine($"[API] Fetching exchange rates at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");

            var response = await _httpClient.GetAsync($"https://api.frankfurter.app/latest?base=EUR"); // Fetch rates from the API
            response.EnsureSuccessStatusCode(); // Ensure the request was successful
            var content = await response.Content.ReadAsStringAsync(); // Read the response content as a string
            var conversionResponse = JsonSerializer.Deserialize<ConversionResponse>(content); // Deserialize the JSON response into a ConversionResponse object

            if (conversionResponse == null || conversionResponse.Rates == null)
            {
                throw new Exception("Conversion rate not found.");
            }

            _savedRates = conversionResponse.Rates; // Update the saved rates with the new rates from the API
            _savedRates["EUR"] = 1m; // Ensure EUR is always in the dictionary with a rate of 1
            _lastUpdated = DateTime.UtcNow; // Update the last updated timestamp
            Console.WriteLine($"[CACHE] Exchange rates updated at {_lastUpdated:yyyy-MM-dd HH:mm:ss}");       
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching exchange rates: " + ex.Message);
        }

    }

}



    



