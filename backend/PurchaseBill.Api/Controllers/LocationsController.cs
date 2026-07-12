using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PurchaseBill.Api.DTOs;
using PurchaseBill.Api.Interfaces;
using PurchaseBill.Api.Models;

namespace PurchaseBill.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationsController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    /// <summary>Returns the saved Location_Details rows, used to populate the Batch dropdown.</summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<LocationDto>>>> GetAll()
    {
        var locations = await _locationService.GetAllAsync();
        return Ok(ApiResponse<List<LocationDto>>.Ok(locations));
    }
}
