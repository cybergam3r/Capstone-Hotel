using HotelBackend.DTOs;

namespace HotelBackend.Services;

public interface IServizioExtraService
{
    Task<IEnumerable<ServizioExtraDTO>> GetAllAsync();
    Task<ServizioExtraDTO?> GetByIdAsync(int id);
    Task<ServizioExtraDTO> CreateAsync(ServizioExtraDTO dto);
    Task<ServizioExtraDTO?> UpdateAsync(ServizioExtraDTO dto);
    Task<bool> DeleteAsync(int id);
}