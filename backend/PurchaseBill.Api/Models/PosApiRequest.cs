using System.Text.Json.Serialization;

namespace PurchaseBill.Api.Models;

/// <summary>
/// Wire format required by the external POS Invoke API (see assignment spec).
/// Field names must match the external contract exactly, hence the JSON attributes.
/// </summary>
public class PosApiRequest
{
    [JsonPropertyName("API_Action")]
    public string ApiAction { get; set; } = "GetLoginData";

    [JsonPropertyName("Device_Id")]
    public string DeviceId { get; set; } = "D001";

    [JsonPropertyName("Sync_Time")]
    public string SyncTime { get; set; } = string.Empty;

    [JsonPropertyName("Company_Code")]
    public string CompanyCode { get; set; } = string.Empty;

    [JsonPropertyName("API_Body")]
    public PosApiRequestBody ApiBody { get; set; } = new();
}

public class PosApiRequestBody
{
    [JsonPropertyName("Username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("Pw")]
    public string Pw { get; set; } = string.Empty;
}
