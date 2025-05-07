using HotelBackend.DTOs;

namespace HotelBackend.Services;

public interface ICameraService
{
    Task<IEnumerable<CameraDTO>> GetAllAsync();
    Task<CameraDTO?> GetByIdAsync(int id);
    Task<CameraDTO> CreateAsync(CameraDTO cameraDto);
    Task<CameraDTO?> UpdateAsync(CameraDTO cameraDto);
    Task<bool> DeleteAsync(int id);
}