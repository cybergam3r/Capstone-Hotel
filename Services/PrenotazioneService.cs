using HotelBackend.DTOs;
using HotelBackend.Models;
using HotelBackend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelBackend.Services;

public class PrenotazioneService : IPrenotazioneService
{
    private readonly IPrenotazioneRepository _repository;
    private readonly ApplicationDbContext _context;

    public PrenotazioneService(IPrenotazioneRepository repository, ApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<IEnumerable<PrenotazioneDTO>> GetAllAsync(string userId)
    {
        var prenotazioni = await _repository.GetAllByUserAsync(userId);

        return prenotazioni.Select(p => new PrenotazioneDTO
        {
            Id = p.Id,
            CameraId = p.CameraId,
            CheckIn = p.CheckIn,
            CheckOut = p.CheckOut,
            Totale = p.Totale,
            Camera = new CameraDTO
            {
                Id = p.Camera.Id,
                Numero = p.Camera.Numero
            },
            Servizi = p.ServiziPrenotati?.Select(sp => new ServizioExtraDTO
            {
                Id = sp.ServizioExtra.Id,
                Nome = sp.ServizioExtra.Nome,
                Prezzo = sp.ServizioExtra.Prezzo,
                Disponibile = sp.ServizioExtra.Disponibile
            }).ToList() ?? new List<ServizioExtraDTO>()
        });
    }

    public async Task<IEnumerable<PrenotazioneDTO>> GetAllForAdminAsync()
    {
        var prenotazioni = await _context.Prenotazioni
            .Include(p => p.Camera)
            .Include(p => p.User)
            .Include(p => p.ServiziPrenotati)
                .ThenInclude(sp => sp.ServizioExtra)
            .OrderByDescending(p => p.CheckIn)
            .ToListAsync();

        return prenotazioni.Select(p => new PrenotazioneDTO
        {
            Id = p.Id,
            CameraId = p.CameraId,
            CheckIn = p.CheckIn,
            CheckOut = p.CheckOut,
            Totale = p.Totale,
            Camera = new CameraDTO
            {
                Id = p.Camera.Id,
                Numero = p.Camera.Numero
            },
            UserEmail = p.User.Email,
            Servizi = p.ServiziPrenotati?.Select(sp => new ServizioExtraDTO
            {
                Id = sp.ServizioExtra.Id,
                Nome = sp.ServizioExtra.Nome,
                Prezzo = sp.ServizioExtra.Prezzo,
                Disponibile = sp.ServizioExtra.Disponibile
            }).ToList() ?? new List<ServizioExtraDTO>()
        });
    }

    public async Task<PrenotazioneDTO?> GetByIdAsync(int id, string userId)
    {
        var p = await _context.Prenotazioni
            .Include(p => p.Camera)
            .Include(p => p.ServiziPrenotati)
                .ThenInclude(sp => sp.ServizioExtra)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (p == null || p.UserId != userId) return null;

        return new PrenotazioneDTO
        {
            Id = p.Id,
            CameraId = p.CameraId,
            CheckIn = p.CheckIn,
            CheckOut = p.CheckOut,
            Totale = p.Totale,
            Camera = new CameraDTO
            {
                Id = p.Camera.Id,
                Numero = p.Camera.Numero
            },
            Servizi = p.ServiziPrenotati?.Select(sp => new ServizioExtraDTO
            {
                Id = sp.ServizioExtra.Id,
                Nome = sp.ServizioExtra.Nome,
                Prezzo = sp.ServizioExtra.Prezzo,
                Disponibile = sp.ServizioExtra.Disponibile
            }).ToList() ?? new List<ServizioExtraDTO>()
        };
    }

    public async Task<PrenotazioneDTO> CreateAsync(PrenotazioneDTO dto, string userId)
    {
        var camera = await _context.Camere.FindAsync(dto.CameraId);
        if (camera == null)
            throw new InvalidOperationException("Camera non trovata.");


        var sovrapposizione = await _context.Prenotazioni.AnyAsync(p =>
            p.CameraId == dto.CameraId &&
            p.CheckIn < dto.CheckOut && 
            p.CheckOut > dto.CheckIn   
        );

        if (sovrapposizione)
            throw new InvalidOperationException("La camera è già prenotata per le date selezionate.");

        var giorni = (dto.CheckOut.Date - dto.CheckIn.Date).Days;
        if (giorni <= 0)
            throw new InvalidOperationException("Intervallo date non valido.");

        decimal totale = giorni * camera.PrezzoNotte;

        var servizi = new List<PrenotazioneServizio>();
        if (dto.Servizi != null && dto.Servizi.Any())
        {
            foreach (var servizioDto in dto.Servizi)
            {
                var servizio = await _context.ServiziExtra.FindAsync(servizioDto.Id);
                if (servizio != null && servizio.Disponibile)
                {
                    totale += servizio.Prezzo;

                    servizi.Add(new PrenotazioneServizio
                    {
                        ServizioExtraId = servizio.Id
                    });
                }
            }
        }

        var prenotazione = new Prenotazione
        {
            CameraId = dto.CameraId,
            CheckIn = dto.CheckIn,
            CheckOut = dto.CheckOut,
            Totale = totale,
            UserId = userId,
            ServiziPrenotati = servizi
        };

        var created = await _repository.CreateAsync(prenotazione);

        await _context.SaveChangesAsync();

        return new PrenotazioneDTO
        {
            Id = created.Id,
            CameraId = created.CameraId,
            CheckIn = created.CheckIn,
            CheckOut = created.CheckOut,
            Totale = created.Totale,
            Camera = new CameraDTO
            {
                Id = camera.Id,
                Numero = camera.Numero
            },
            Servizi = dto.Servizi
        };
    }


    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var prenotazione = await _context.Prenotazioni
            .Include(p => p.ServiziPrenotati)
            .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

        if (prenotazione == null) return false;

        var camera = await _context.Camere.FindAsync(prenotazione.CameraId);
        if (camera != null)
            camera.Disponibile = true;

        _context.Prenotazioni.Remove(prenotazione);
        await _context.SaveChangesAsync();

        return true;
    }


    public async Task AggiornaDisponibilitaCamereAsync()
    {
        var oggi = DateTime.Today;

    
        var camere = await _context.Camere.Include(c => c.Prenotazioni).ToListAsync();

        foreach (var camera in camere)
        {
            
            var haPrenotazioniAttive = camera.Prenotazioni.Any(p => p.CheckOut >= oggi);

           
            camera.Disponibile = !haPrenotazioniAttive;
        }

        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<CameraDTO>> GetCamereDisponibiliAsync()
    {
    
        await AggiornaDisponibilitaCamereAsync();

       
        var camere = await _context.Camere
            .Where(c => c.Disponibile) 
            .ToListAsync();

     
        return camere.Select(c => new CameraDTO
        {
            Id = c.Id,
            Numero = c.Numero,
            Tipo = c.Tipo,
            PrezzoNotte = c.PrezzoNotte,
            Disponibile = c.Disponibile
        });
    }
    public async Task<IEnumerable<CameraDTO>> GetCamereDisponibiliPerDataAsync(DateTime data)
    {
     
        var camere = await _context.Camere
            .Include(c => c.Prenotazioni) 
            .ToListAsync();

      
        var camereDisponibili = camere.Where(c =>
            !c.Prenotazioni.Any(p => p.CheckIn <= data && p.CheckOut >= data) 
        );

       
        return camereDisponibili.Select(c => new CameraDTO
        {
            Id = c.Id,
            Numero = c.Numero,
            Tipo = c.Tipo,
            PrezzoNotte = c.PrezzoNotte,
            Disponibile = true 
        });
    }
    public async Task<IEnumerable<CameraDTO>> GetCamereDisponibiliPerIntervalloAsync(DateTime dataInizio, DateTime dataFine)
    {
        
        var camere = await _context.Camere
            .Include(c => c.Prenotazioni) 
            .ToListAsync();

        
        var camereDisponibili = camere.Where(c =>
            !c.Prenotazioni.Any(p =>
                (p.CheckIn < dataFine && p.CheckOut > dataInizio) 
            )
        );

        
        return camereDisponibili.Select(c => new CameraDTO
        {
            Id = c.Id,
            Numero = c.Numero,
            Tipo = c.Tipo,
            PrezzoNotte = c.PrezzoNotte,
            Disponibile = true 
        });
    }
    public async Task<PrenotazioneDTO?> UpdateAsync(int id, PrenotazioneDTO dto)
    {
        
        var prenotazione = await _context.Prenotazioni
            .Include(p => p.ServiziPrenotati)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (prenotazione == null)
            throw new InvalidOperationException("Prenotazione non trovata.");

       
        if (prenotazione.CameraId != dto.CameraId)
        {
            var sovrapposizione = await _context.Prenotazioni.AnyAsync(p =>
                p.CameraId == dto.CameraId &&
                p.CheckIn < dto.CheckOut &&
                p.CheckOut > dto.CheckIn &&
                p.Id != id 
            );

            if (sovrapposizione)
                throw new InvalidOperationException("La nuova camera è già prenotata per le date selezionate.");
        }

        
        prenotazione.CameraId = dto.CameraId;
        prenotazione.CheckIn = dto.CheckIn;
        prenotazione.CheckOut = dto.CheckOut;

        var giorni = (dto.CheckOut.Date - dto.CheckIn.Date).Days;
        if (giorni <= 0)
            throw new InvalidOperationException("Intervallo date non valido.");

        decimal totale = giorni * (await _context.Camere.FindAsync(dto.CameraId))!.PrezzoNotte;

     
        prenotazione.ServiziPrenotati.Clear();
        if (dto.Servizi != null && dto.Servizi.Any())
        {
            foreach (var servizioDto in dto.Servizi)
            {
                var servizio = await _context.ServiziExtra.FindAsync(servizioDto.Id);
                if (servizio != null && servizio.Disponibile)
                {
                    totale += servizio.Prezzo;

                    prenotazione.ServiziPrenotati.Add(new PrenotazioneServizio
                    {
                        ServizioExtraId = servizio.Id
                    });
                }
            }
        }

        prenotazione.Totale = totale;


        await _context.SaveChangesAsync();

     
        return new PrenotazioneDTO
        {
            Id = prenotazione.Id,
            CameraId = prenotazione.CameraId,
            CheckIn = prenotazione.CheckIn,
            CheckOut = prenotazione.CheckOut,
            Totale = prenotazione.Totale,
            Camera = new CameraDTO
            {
                Id = prenotazione.Camera.Id,
                Numero = prenotazione.Camera.Numero,
                Tipo = prenotazione.Camera.Tipo,
                PrezzoNotte = prenotazione.Camera.PrezzoNotte,
                Disponibile = prenotazione.Camera.Disponibile
            },
            Servizi = prenotazione.ServiziPrenotati.Select(sp => new ServizioExtraDTO
            {
                Id = sp.ServizioExtra.Id,
                Nome = sp.ServizioExtra.Nome,
                Prezzo = sp.ServizioExtra.Prezzo,
                Disponibile = sp.ServizioExtra.Disponibile
            }).ToList()
        };
    }


}
