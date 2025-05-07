namespace HotelBackend.DTOs;

public class PrenotazioneDTO
{
    public int Id { get; set; }
    public int CameraId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public decimal Totale { get; set; }

    public CameraDTO? Camera { get; set; }

 
    public List<ServizioExtraDTO>? Servizi { get; set; }

   
    public string? UserEmail { get; set; }
}