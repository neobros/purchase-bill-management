using PurchaseBill.Api.DTOs;

namespace PurchaseBill.Api.Interfaces;

public interface IPurchaseBillService
{
    Task<List<PurchaseBillResponseDto>> GetAllAsync();
    Task<PurchaseBillResponseDto?> GetByIdAsync(int id);
    Task<PurchaseBillResponseDto> CreateAsync(PurchaseBillCreateDto dto);
    Task<PurchaseBillResponseDto?> UpdateAsync(int id, PurchaseBillUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
