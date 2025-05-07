using HotelBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ServiziExtraController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ServiziExtraController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous] 
    public async Task<IActionResult> GetAll()
    {
        var servizi = await _context.ServiziExtra.ToListAsync();
        return Ok(servizi);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ServizioExtra servizio)
    {
        _context.ServiziExtra.Add(servizio);
        await _context.SaveChangesAsync();
        return Ok(servizio);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ServizioExtra servizio)
    {
        if (id != servizio.Id) return BadRequest();
        _context.Entry(servizio).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return Ok(servizio);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var servizio = await _context.ServiziExtra.FindAsync(id);
        if (servizio == null) return NotFound();
        _context.ServiziExtra.Remove(servizio);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
