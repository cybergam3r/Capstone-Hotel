namespace HotelBackend.DTOs
{
    public class UpdateProfiloDTO
    {
        public string? Nome { get; set; }
        public string? Cognome { get; set; }
        public string? CodiceFiscale { get; set; }
        public DateTime? DataDiNascita { get; set; }
        public string? NumeroDiTelefono { get; set; } 
    }
}
