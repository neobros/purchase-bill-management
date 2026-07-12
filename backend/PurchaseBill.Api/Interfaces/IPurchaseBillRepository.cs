using PurchaseBill.Api.Entities;

namespace PurchaseBill.Api.Interfaces;

public interface IPurchaseBillRepository
{
    Task<List<PurchaseBillHeader>> GetAllAsync();
    Task<PurchaseBillHeader?> GetByIdAsync(int id);
    Task<PurchaseBillHeader> AddAsync(PurchaseBillHeader bill);
    Task UpdateAsync(PurchaseBillHeader bill);
    Task DeleteAsync(PurchaseBillHeader bill);
}
