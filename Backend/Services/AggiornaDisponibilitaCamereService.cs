using Microsoft.Extensions.Hosting;

namespace HotelBackend.Services;

public class AggiornaDisponibilitaCamereService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public AggiornaDisponibilitaCamereService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var prenotazioneService = scope.ServiceProvider.GetRequiredService<IPrenotazioneService>();

                
                await prenotazioneService.AggiornaDisponibilitaCamereAsync();
            }

            
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}
