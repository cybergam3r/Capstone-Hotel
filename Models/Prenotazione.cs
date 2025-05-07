using System.ComponentModel.DataAnnotations;

namespace HotelBackend.Models;

public class Prenotazione
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public int CameraId { get; set; }
    public Camera Camera { get; set; } = null!;

    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }

    public decimal Totale { get; set; }

   
    public ICollection<PrenotazioneServizio> ServiziPrenotati { get; set; } = new List<PrenotazioneServizio>();
}