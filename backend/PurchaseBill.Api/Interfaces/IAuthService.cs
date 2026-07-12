using PurchaseBill.Api.DTOs;

namespace PurchaseBill.Api.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}
