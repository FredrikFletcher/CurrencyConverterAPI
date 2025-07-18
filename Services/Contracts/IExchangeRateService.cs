using CurrencyConverterApi.Services;
using CurrencyConverterApi.Services.Contracts;
namespace CurrencyConverterApi;
using System.Threading.Tasks;
public interface IExchangeRateService
{
    Task<decimal> GetConvertCurrencyAsync(string from, string to, decimal amount); // Method to convert currency from one to another
    Task GetRateAsync(); // Method to fetch exchange rates from the API and update the cache
  

}