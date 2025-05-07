using HotelBackend.Models;

namespace HotelBackend.Repositories;

public interface IServizioExtraRepository
{
    Task<IEnumerable<ServizioExtra>> GetAllAsync();
    Task<ServizioExtra?> GetByIdAsync(int id);
    Task<ServizioExtra> CreateAsync(ServizioExtra servizio);
    Task<ServizioExtra?> UpdateAsync(ServizioExtra servizio);
    Task<bool> DeleteAsync(int id);
}