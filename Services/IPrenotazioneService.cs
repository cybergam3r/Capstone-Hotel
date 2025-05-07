using HotelBackend.DTOs;

namespace HotelBackend.Services;

public interface IPrenotazioneService
{
    Task<IEnumerable<PrenotazioneDTO>> GetAllAsync(string userId);
    Task<PrenotazioneDTO?> GetByIdAsync(int id, string userId);
    Task<PrenotazioneDTO> CreateAsync(PrenotazioneDTO dto, string userId);
    Task<bool> DeleteAsync(int id, string userId);

    Task<IEnumerable<PrenotazioneDTO>> GetAllForAdminAsync();
    Task AggiornaDisponibilitaCamereAsync();
    Task<IEnumerable<CameraDTO>> GetCamereDisponibiliAsync();
    Task<IEnumerable<CameraDTO>> GetCamereDisponibiliPerDataAsync(DateTime data);
    Task<IEnumerable<CameraDTO>> GetCamereDisponibiliPerIntervalloAsync(DateTime dataInizio, DateTime dataFine);

    Task<PrenotazioneDTO?> UpdateAsync(int id, PrenotazioneDTO dto);




}