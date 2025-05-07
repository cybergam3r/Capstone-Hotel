using HotelBackend.Models;

namespace HotelBackend.Repositories;

public interface ICameraRepository
{
    Task<IEnumerable<Camera>> GetAllAsync();
    Task<Camera?> GetByIdAsync(int id);
    Task<Camera> CreateAsync(Camera camera);
    Task<Camera?> UpdateAsync(Camera camera);
    Task<bool> DeleteAsync(int id);
}
