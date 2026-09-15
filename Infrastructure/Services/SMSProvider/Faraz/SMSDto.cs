using Newtonsoft.Json;

public class SendPatternRequest
{
    [JsonProperty("code")]
    public string Code { get; set; } = null!;

    [JsonProperty("attributes")]
    public Dictionary<string, string> Attributes { get; set; } = new();

    [JsonProperty("recipient")]
    public string Recipient { get; set; } = null!;

    [JsonProperty("line_number")]
    public string LineNumber { get; set; } = null!;

    [JsonProperty("number_format")]
    public string NumberFormat { get; set; } = "english";

    [JsonProperty("schedule", NullValueHandling = NullValueHandling.Ignore)]
    public string? Schedule { get; set; }
}