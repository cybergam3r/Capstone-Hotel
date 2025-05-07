using HotelBackend.DTOs;
using HotelBackend.Models;
using HotelBackend.Repositories;

namespace HotelBackend.Services;

public class ServizioExtraService : IServizioExtraService
{
    private readonly IServizioExtraRepository _repository;

    public ServizioExtraService(IServizioExtraRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ServizioExtraDTO>> GetAllAsync()
    {
        var servizi = await _repository.GetAllAsync();
        return servizi.Select(s => new ServizioExtraDTO
        {
            Id = s.Id,
            Nome = s.Nome,
            Prezzo = s.Prezzo,
            Disponibile = s.Disponibile
        });
    }

    public async Task<ServizioExtraDTO?> GetByIdAsync(int id)
    {
        var s = await _repository.GetByIdAsync(id);
        if (s == null) return null;

        return new ServizioExtraDTO
        {
            Id = s.Id,
            Nome = s.Nome,
            Prezzo = s.Prezzo,
            Disponibile = s.Disponibile
        };
    }

    public async Task<ServizioExtraDTO> CreateAsync(ServizioExtraDTO dto)
    {
        var servizio = new ServizioExtra
        {
            Nome = dto.Nome,
            Prezzo = dto.Prezzo,
            Disponibile = dto.Disponibile
        };

        var created = await _repository.CreateAsync(servizio);
        dto.Id = created.Id;
        return dto;
    }

    public async Task<ServizioExtraDTO?> UpdateAsync(ServizioExtraDTO dto)
    {
        var servizio = new ServizioExtra
        {
            Id = dto.Id,
            Nome = dto.Nome,
            Prezzo = dto.Prezzo,
            Disponibile = dto.Disponibile
        };

        var updated = await _repository.UpdateAsync(servizio);
        return updated == null ? null : dto;
    }

    public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
}