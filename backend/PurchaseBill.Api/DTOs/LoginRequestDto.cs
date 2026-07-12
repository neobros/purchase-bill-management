using System.ComponentModel.DataAnnotations;

namespace PurchaseBill.Api.DTOs;

public class LoginRequestDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(1, ErrorMessage = "Password cannot be empty.")]
    public string Password { get; set; } = string.Empty;
}
