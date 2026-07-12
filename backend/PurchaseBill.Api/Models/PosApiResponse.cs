using System.Text.Json.Serialization;

namespace PurchaseBill.Api.Models;

/// <summary>
/// Raw response shape returned by the external POS Invoke API. Verified against
/// the live GetLoginData response: the envelope carries Status_Code/Message, and
/// the actual login payload (including User_Locations) is nested one level down
/// inside Response_Body[0], not at the top level.
/// </summary>
public class PosApiResponse
{
    [JsonPropertyName("Status_Code")]
    public int StatusCode { get; set; }

    [JsonPropertyName("Message")]
    public string? Message { get; set; }

    [JsonPropertyName("Response_Body")]
    public List<PosLoginData>? ResponseBody { get; set; }
}

public class PosLoginData
{
    [JsonPropertyName("User_Code")]
    public string? UserCode { get; set; }

    [JsonPropertyName("User_Display_Name")]
    public string? UserDisplayName { get; set; }

    [JsonPropertyName("Email")]
    public string? Email { get; set; }

    [JsonPropertyName("Company_Code")]
    public string? CompanyCode { get; set; }

    [JsonPropertyName("User_Locations")]
    public List<PosUserLocation>? UserLocations { get; set; }
}

public class PosUserLocation
{
    [JsonPropertyName("Location_Code")]
    public string LocationCode { get; set; } = string.Empty;

    [JsonPropertyName("Location_Name")]
    public string LocationName { get; set; } = string.Empty;
}
