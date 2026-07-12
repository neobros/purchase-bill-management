using PurchaseBill.Api.DTOs;

namespace PurchaseBill.Api.Interfaces;

public interface ILocationService
{
    Task<List<LocationDto>> GetAllAsync();
}
