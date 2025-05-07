using Microsoft.AspNetCore.Identity;

namespace HotelBackend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Nome { get; set; } 
        public string? Cognome { get; set; } 
        public string? CodiceFiscale { get; set; } 
        public DateTime? DataDiNascita { get; set; } 
        

        public string? NumeroDiTelefono { get; set; } 

    }
}
