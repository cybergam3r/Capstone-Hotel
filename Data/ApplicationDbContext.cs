using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HotelBackend.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Camera> Camere { get; set; }
    public DbSet<Prenotazione> Prenotazioni { get; set; }
    public DbSet<ServizioExtra> ServiziExtra { get; set; }
    public DbSet<PrenotazioneServizio> PrenotazioniServizi { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Camera>()
            .Property(c => c.PrezzoNotte)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Prenotazione>()
            .Property(p => p.Totale)
            .HasPrecision(10, 2);

        modelBuilder.Entity<ServizioExtra>()
            .Property(s => s.Prezzo)
            .HasPrecision(10, 2);

       
        modelBuilder.Entity<PrenotazioneServizio>()
            .HasKey(ps => new { ps.PrenotazioneId, ps.ServizioExtraId });
    }
}
