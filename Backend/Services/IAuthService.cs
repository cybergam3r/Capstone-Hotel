using HotelBackend.DTOs;

namespace HotelBackend.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDTO dto);
    Task<AuthResponseDTO?> LoginAsync(LoginDTO dto);
}
