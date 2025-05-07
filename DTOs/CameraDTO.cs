namespace HotelBackend.DTOs;

public class CameraDTO
{
    public int Id { get; set; }
    public string Numero { get; set; }
    public string Tipo { get; set; }
    public decimal PrezzoNotte { get; set; }
    public bool Disponibile { get; set; }
}