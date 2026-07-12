using AutoMapper;
using PurchaseBill.Api.DTOs;
using PurchaseBill.Api.Entities;
using PurchaseBill.Api.Helpers;
using PurchaseBill.Api.Interfaces;

namespace PurchaseBill.Api.Services;

public class PurchaseBillService : IPurchaseBillService
{
    private readonly IPurchaseBillRepository _repository;
    private readonly IMapper _mapper;

    public PurchaseBillService(IPurchaseBillRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<PurchaseBillResponseDto>> GetAllAsync()
    {
        var bills = await _repository.GetAllAsync();
        return _mapper.Map<List<PurchaseBillResponseDto>>(bills);
    }

    public async Task<PurchaseBillResponseDto?> GetByIdAsync(int id)
    {
        var bill = await _repository.GetByIdAsync(id);
        return bill is null ? null : _mapper.Map<PurchaseBillResponseDto>(bill);
    }

    public async Task<PurchaseBillResponseDto> CreateAsync(PurchaseBillCreateDto dto)
    {
        var bill = new PurchaseBillHeader
        {
            BillDate = dto.BillDate,
            Items = dto.Items.Select(MapToItem).ToList()
        };

        var created = await _repository.AddAsync(bill);
        return _mapper.Map<PurchaseBillResponseDto>(created);
    }

    public async Task<PurchaseBillResponseDto?> UpdateAsync(int id, PurchaseBillUpdateDto dto)
    {
        var bill = await _repository.GetByIdAsync(id);
        if (bill is null)
        {
            return null;
        }

        bill.BillDate = dto.BillDate;
        bill.Items.Clear();
        foreach (var item in dto.Items.Select(MapToItem))
        {
            bill.Items.Add(item);
        }

        await _repository.UpdateAsync(bill);
        return _mapper.Map<PurchaseBillResponseDto>(bill);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var bill = await _repository.GetByIdAsync(id);
        if (bill is null)
        {
            return false;
        }

        await _repository.DeleteAsync(bill);
        return true;
    }

    private static PurchaseBillItem MapToItem(PurchaseBillItemDto dto)
    {
        var (totalCost, totalSelling) = CalculationHelper.CalculateLineTotals(
            dto.StandardCost, dto.StandardPrice, dto.Qty, dto.Discount);

        return new PurchaseBillItem
        {
            Item = dto.Item,
            Batch = dto.Batch,
            StandardCost = dto.StandardCost,
            StandardPrice = dto.StandardPrice,
            Margin = dto.Margin,
            Qty = dto.Qty,
            FreeQty = dto.FreeQty,
            Discount = dto.Discount,
            TotalCost = totalCost,
            TotalSelling = totalSelling
        };
    }
}
