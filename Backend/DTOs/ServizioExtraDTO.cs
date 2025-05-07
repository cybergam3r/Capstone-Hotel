namespace HotelBackend.DTOs;

public class ServizioExtraDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Prezzo { get; set; }

  
    public bool Disponibile { get; set; }
}