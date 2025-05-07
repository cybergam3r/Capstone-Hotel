
using HotelBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBackend.Repositories;

public class CameraRepository : ICameraRepository
{
    private readonly ApplicationDbContext _context;

    public CameraRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Camera>> GetAllAsync()
        => await _context.Camere.ToListAsync();

    public async Task<Camera?> GetByIdAsync(int id)
        => await _context.Camere.FindAsync(id);

    public async Task<Camera> CreateAsync(Camera camera)
    {
        _context.Camere.Add(camera);
        await _context.SaveChangesAsync();
        return camera;
    }

    public async Task<Camera?> UpdateAsync(Camera camera)
    {
        var existing = await _context.Camere.FindAsync(camera.Id);
        if (existing == null) return null;

        existing.Numero = camera.Numero;
        existing.Tipo = camera.Tipo;
        existing.PrezzoNotte = camera.PrezzoNotte;
        existing.Disponibile = camera.Disponibile;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var camera = await _context.Camere.FindAsync(id);
        if (camera == null) return false;

        _context.Camere.Remove(camera);
        await _context.SaveChangesAsync();
        return true;
    }
}
