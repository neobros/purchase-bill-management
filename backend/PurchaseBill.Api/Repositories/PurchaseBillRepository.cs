using Microsoft.EntityFrameworkCore;
using PurchaseBill.Api.Data;
using PurchaseBill.Api.Entities;
using PurchaseBill.Api.Interfaces;

namespace PurchaseBill.Api.Repositories;

public class PurchaseBillRepository : IPurchaseBillRepository
{
    private readonly AppDbContext _context;

    public PurchaseBillRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PurchaseBillHeader>> GetAllAsync() =>
        await _context.PurchaseBills
            .Include(b => b.Items)
            .AsNoTracking()
            .OrderByDescending(b => b.BillDate)
            .ToListAsync();

    public async Task<PurchaseBillHeader?> GetByIdAsync(int id) =>
        await _context.PurchaseBills
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.Id == id);

    public async Task<PurchaseBillHeader> AddAsync(PurchaseBillHeader bill)
    {
        _context.PurchaseBills.Add(bill);
        await _context.SaveChangesAsync();
        return bill;
    }

    public async Task UpdateAsync(PurchaseBillHeader bill)
    {
        bill.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PurchaseBillHeader bill)
    {
        _context.PurchaseBills.Remove(bill);
        await _context.SaveChangesAsync();
    }
}
