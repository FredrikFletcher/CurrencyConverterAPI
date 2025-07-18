using System.Text.Json.Serialization;

public class ConversionResponse
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
    [JsonPropertyName("base")]
    public string? Base { get; set; }
    [JsonPropertyName("date")]
    public string? Date { get; set; }
    [JsonPropertyName("rates")]
    public Dictionary<string, decimal>? Rates { get; set; }
}

// Class represents the respnse from frankfurter.app API
// It contains the amount converted, the base currency, the date of conversion,
// and a dictionary of rates where the key is the target currency and the value is the converted