using HotelBackend.DTOs;
using HotelBackend.Models;
using HotelBackend.Repositories;

namespace HotelBackend.Services;

public class CameraService : ICameraService
{
    private readonly ICameraRepository _repository;

    public CameraService(ICameraRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CameraDTO>> GetAllAsync()
    {
        var camere = await _repository.GetAllAsync();
        return camere.Select(c => new CameraDTO
        {
            Id = c.Id,
            Numero = c.Numero,
            Tipo = c.Tipo,
            PrezzoNotte = c.PrezzoNotte,
            Disponibile = c.Disponibile
        });
    }

    public async Task<CameraDTO?> GetByIdAsync(int id)
    {
        var c = await _repository.GetByIdAsync(id);
        if (c == null) return null;

        return new CameraDTO
        {
            Id = c.Id,
            Numero = c.Numero,
            Tipo = c.Tipo,
            PrezzoNotte = c.PrezzoNotte,
            Disponibile = c.Disponibile
        };
    }

    public async Task<CameraDTO> CreateAsync(CameraDTO dto)
    {
        var camera = new Camera
        {
            Numero = dto.Numero,
            Tipo = dto.Tipo,
            PrezzoNotte = dto.PrezzoNotte,
            Disponibile = dto.Disponibile
        };

        var created = await _repository.CreateAsync(camera);

        dto.Id = created.Id;
        return dto;
    }

    public async Task<CameraDTO?> UpdateAsync(CameraDTO dto)
    {
        var camera = new Camera
        {
            Id = dto.Id,
            Numero = dto.Numero,
            Tipo = dto.Tipo,
            PrezzoNotte = dto.PrezzoNotte,
            Disponibile = dto.Disponibile
        };

        var updated = await _repository.UpdateAsync(camera);
        return updated == null ? null : dto;
    }

    public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
}
