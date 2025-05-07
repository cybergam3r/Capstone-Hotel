namespace HotelBackend.Models;

public class ServizioExtra
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Prezzo { get; set; }
    public bool Disponibile { get; set; } = true;

    public List<PrenotazioneServizio> Prenotazioni { get; set; } = new();
}
