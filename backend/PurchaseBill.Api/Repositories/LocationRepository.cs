using Microsoft.EntityFrameworkCore;
using PurchaseBill.Api.Data;
using PurchaseBill.Api.Entities;
using PurchaseBill.Api.Interfaces;

namespace PurchaseBill.Api.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly AppDbContext _context;

    public LocationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<LocationDetail>> GetAllAsync() =>
        await _context.LocationDetails.AsNoTracking().OrderBy(l => l.LocationName).ToListAsync();

    public async Task<LocationDetail?> GetByCodeAsync(string locationCode) =>
        await _context.LocationDetails.FirstOrDefaultAsync(l => l.LocationCode == locationCode);

    public async Task UpsertRangeAsync(IEnumerable<LocationDetail> locations)
    {
        foreach (var location in locations)
        {
            var existing = await GetByCodeAsync(location.LocationCode);
            if (existing is null)
            {
                _context.LocationDetails.Add(location);
            }
            else
            {
                existing.LocationName = location.LocationName;
                existing.UpdatedAt = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync();
    }
}
