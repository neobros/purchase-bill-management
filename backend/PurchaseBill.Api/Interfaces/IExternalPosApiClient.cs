using PurchaseBill.Api.Models;

namespace PurchaseBill.Api.Interfaces;

public interface IExternalPosApiClient
{
    Task<PosApiResponse> GetLoginDataAsync(string email, string password);
}
