using HotelBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBackend.Repositories;

public class ServizioExtraRepository : IServizioExtraRepository
{
    private readonly ApplicationDbContext _context;

    public ServizioExtraRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ServizioExtra>> GetAllAsync()
        => await _context.ServiziExtra.ToListAsync();

    public async Task<ServizioExtra?> GetByIdAsync(int id)
        => await _context.ServiziExtra.FindAsync(id);

    public async Task<ServizioExtra> CreateAsync(ServizioExtra servizio)
    {
        _context.ServiziExtra.Add(servizio);
        await _context.SaveChangesAsync();
        return servizio;
    }

    public async Task<ServizioExtra?> UpdateAsync(ServizioExtra servizio)
    {
        var existing = await _context.ServiziExtra.FindAsync(servizio.Id);
        if (existing == null) return null;

        existing.Nome = servizio.Nome;
        existing.Prezzo = servizio.Prezzo;
        existing.Disponibile = servizio.Disponibile;
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var servizio = await _context.ServiziExtra.FindAsync(id);
        if (servizio == null) return false;

        _context.ServiziExtra.Remove(servizio);
        await _context.SaveChangesAsync();
        return true;
    }
}