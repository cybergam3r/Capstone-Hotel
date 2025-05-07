using HotelBackend.DTOs;
using HotelBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CamereController : ControllerBase
{
    private readonly ICameraService _service;
    private readonly IPrenotazioneService _prenotazioneService;

    public CamereController(ICameraService service, IPrenotazioneService prenotazioneService)
    {
        _service = service;
        _prenotazioneService = prenotazioneService;
    }
    [HttpGet("disponibili")]
    public async Task<IActionResult> GetCamereDisponibili()
    {
        var camere = await _prenotazioneService.GetCamereDisponibiliAsync();
        return Ok(camere);
    }

    [HttpGet("disponibili-per-data")]
    public async Task<IActionResult> GetCamereDisponibiliPerData([FromQuery] DateTime data)
    {
        var camere = await _prenotazioneService.GetCamereDisponibiliPerDataAsync(data);
        return Ok(camere);
    }

    [HttpGet("disponibili-per-intervallo")]
    public async Task<IActionResult> GetCamereDisponibiliPerIntervallo([FromQuery] DateTime dataInizio, [FromQuery] DateTime dataFine)
    {
        if (dataInizio >= dataFine)
        {
            return BadRequest("La data di inizio deve essere precedente alla data di fine.");
        }

        var camere = await _prenotazioneService.GetCamereDisponibiliPerIntervalloAsync(dataInizio, dataFine);
        return Ok(camere);
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var camere = await _service.GetAllAsync();
        return Ok(camere);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var camera = await _service.GetByIdAsync(id);
        if (camera == null) return NotFound();
        return Ok(camera);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CameraDTO dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, CameraDTO dto)
    {
        if (id != dto.Id) return BadRequest();

        var updated = await _service.UpdateAsync(dto);
        if (updated == null) return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
