using Microsoft.AspNetCore.Mvc;
namespace CurrencyConverterApi.Services.Contracts;

[ApiController]
[Route("api/[controller]")]
public class CurrencyConverterController : ControllerBase
{
    private readonly IExchangeRateService _exchangeRateService;

   public CurrencyConverterController(IExchangeRateService exchangeRateService)
    {
       _exchangeRateService = exchangeRateService;
    }

    [HttpGet("convert")]
    public async Task<IActionResult> ConvertCurrency([FromQuery] string from, [FromQuery] string to, [FromQuery] decimal amount)
    {
        if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
        {
            return BadRequest(new { error = "Invalid input parameters." });
        }
        if (amount < 0)
        {
            return BadRequest(new { error = "Amount must be greater than zero." });
        }
        var result = await _exchangeRateService.GetConvertCurrencyAsync(from, to, amount);

        return Ok(new {result});
    }
}