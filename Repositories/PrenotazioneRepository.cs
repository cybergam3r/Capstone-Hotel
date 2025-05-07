using HotelBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBackend.Repositories
{
    public class PrenotazioneRepository : IPrenotazioneRepository
    {
        private readonly ApplicationDbContext _context;

        public PrenotazioneRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Prenotazione>> GetAllByUserAsync(string userId)
        {
            return await _context.Prenotazioni
                .Where(p => p.UserId == userId)
                .Include(p => p.Camera) 
                .ToListAsync();
        }

        public async Task<Prenotazione?> GetByIdAsync(int id)
        {
            return await _context.Prenotazioni
                .Include(p => p.Camera) 
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Prenotazione> CreateAsync(Prenotazione prenotazione)
        {
            _context.Prenotazioni.Add(prenotazione);
            await _context.SaveChangesAsync();
            return prenotazione;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var p = await _context.Prenotazioni
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (p == null) return false;

            _context.Prenotazioni.Remove(p);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}