using System.Net.Http.Json;
using PurchaseBill.Api.Interfaces;
using PurchaseBill.Api.Models;

namespace PurchaseBill.Api.Services;

public class ExternalPosApiClient : IExternalPosApiClient
{
    private readonly HttpClient _httpClient;

    public ExternalPosApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PosApiResponse> GetLoginDataAsync(string email, string password)
    {
        var request = new PosApiRequest
        {
            ApiAction = "GetLoginData",
            DeviceId = "D001",
            SyncTime = string.Empty,
            CompanyCode = email,
            ApiBody = new PosApiRequestBody
            {
                Username = email,
                Pw = password
            }
        };

        using var httpResponse = await _httpClient.PostAsJsonAsync(string.Empty, request);
        httpResponse.EnsureSuccessStatusCode();

        var result = await httpResponse.Content.ReadFromJsonAsync<PosApiResponse>();
        return result ?? new PosApiResponse();
    }
}
