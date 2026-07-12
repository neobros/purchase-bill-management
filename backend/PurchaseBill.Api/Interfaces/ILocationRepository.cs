using PurchaseBill.Api.Entities;

namespace PurchaseBill.Api.Interfaces;

public interface ILocationRepository
{
    Task<List<LocationDetail>> GetAllAsync();
    Task<LocationDetail?> GetByCodeAsync(string locationCode);
    Task UpsertRangeAsync(IEnumerable<LocationDetail> locations);
}
