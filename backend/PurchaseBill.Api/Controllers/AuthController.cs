using Microsoft.AspNetCore.Mvc;
using PurchaseBill.Api.DTOs;
using PurchaseBill.Api.Interfaces;
using PurchaseBill.Api.Models;

namespace PurchaseBill.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>Authenticates against the external POS API and saves the returned locations.</summary>
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login(LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);
        return Ok(ApiResponse<LoginResponseDto>.Ok(result, "Login successful."));
    }
}
