namespace HotelBackend.DTOs;

public class RegisterDTO
{
    public required string Email { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty;
}