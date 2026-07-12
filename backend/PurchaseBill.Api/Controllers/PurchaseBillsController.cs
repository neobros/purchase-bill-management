using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PurchaseBill.Api.DTOs;
using PurchaseBill.Api.Interfaces;
using PurchaseBill.Api.Models;

namespace PurchaseBill.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PurchaseBillsController : ControllerBase
{
    private readonly IPurchaseBillService _purchaseBillService;

    public PurchaseBillsController(IPurchaseBillService purchaseBillService)
    {
        _purchaseBillService = purchaseBillService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<PurchaseBillResponseDto>>>> GetAll()
    {
        var bills = await _purchaseBillService.GetAllAsync();
        return Ok(ApiResponse<List<PurchaseBillResponseDto>>.Ok(bills));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<PurchaseBillResponseDto>>> GetById(int id)
    {
        var bill = await _purchaseBillService.GetByIdAsync(id);
        if (bill is null)
        {
            return NotFound(ApiResponse<PurchaseBillResponseDto>.Fail($"Purchase bill {id} was not found."));
        }

        return Ok(ApiResponse<PurchaseBillResponseDto>.Ok(bill));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PurchaseBillResponseDto>>> Create(PurchaseBillCreateDto dto)
    {
        var created = await _purchaseBillService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id },
            ApiResponse<PurchaseBillResponseDto>.Ok(created, "Purchase bill created."));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<PurchaseBillResponseDto>>> Update(int id, PurchaseBillUpdateDto dto)
    {
        var updated = await _purchaseBillService.UpdateAsync(id, dto);
        if (updated is null)
        {
            return NotFound(ApiResponse<PurchaseBillResponseDto>.Fail($"Purchase bill {id} was not found."));
        }

        return Ok(ApiResponse<PurchaseBillResponseDto>.Ok(updated, "Purchase bill updated."));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var deleted = await _purchaseBillService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(ApiResponse<object>.Fail($"Purchase bill {id} was not found."));
        }

        return Ok(ApiResponse<object>.Ok(new { }, "Purchase bill deleted."));
    }
}
