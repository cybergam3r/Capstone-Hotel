using HotelBackend.Models;

namespace HotelBackend.Repositories
{
    public interface IPrenotazioneRepository
    {
        Task<IEnumerable<Prenotazione>> GetAllByUserAsync(string userId);
        Task<Prenotazione?> GetByIdAsync(int id);
        Task<Prenotazione> CreateAsync(Prenotazione prenotazione);
        Task<bool> DeleteAsync(int id, string userId);
    }
}