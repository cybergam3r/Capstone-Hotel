namespace HotelBackend.Models;

public class PrenotazioneServizio
{
    public int PrenotazioneId { get; set; }
    public Prenotazione Prenotazione { get; set; } = null!;

    public int ServizioExtraId { get; set; }
    public ServizioExtra ServizioExtra { get; set; } = null!;
}