using HotelBackend.DTOs;
using HotelBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PrenotazioniController : ControllerBase
{
    private readonly IPrenotazioneService _service;

    public PrenotazioniController(IPrenotazioneService service)
    {
        _service = service;
    }

    private string GetUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserId();
        var prenotazioni = await _service.GetAllAsync(userId);
        return Ok(prenotazioni);
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllAdmin()
    {
        var prenotazioni = await _service.GetAllForAdminAsync();
        return Ok(prenotazioni);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var userId = GetUserId();
        var p = await _service.GetByIdAsync(id, userId);
        if (p == null) return NotFound();
        return Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PrenotazioneDTO dto)
    {
        var userId = GetUserId();
        var created = await _service.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        var deleted = await _service.DeleteAsync(id, userId);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, PrenotazioneDTO dto)
    {
        try
        {
            var updatedPrenotazione = await _service.UpdateAsync(id, dto); 
            if (updatedPrenotazione == null)
                return NotFound();

            return Ok(updatedPrenotazione);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Errore interno del server.", details = ex.Message });
        }
    }


}