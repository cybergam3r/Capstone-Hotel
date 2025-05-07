using HotelBackend.DTOs;
using HotelBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Claims;

namespace HotelBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _environment;

    public AccountController(UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
    {
        _userManager = userManager;
        _environment = environment;
    }

    [HttpGet("profilo")]
    public async Task<IActionResult> GetProfilo()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return NotFound();

        return Ok(new
        {
            user.Nome,
            user.Cognome,
            user.CodiceFiscale,
            user.DataDiNascita,
            user.NumeroDiTelefono
        });
    }

    [HttpPut("profilo")]
    public async Task<IActionResult> UpdateProfilo([FromBody] UpdateProfiloDTO dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return NotFound();

       
        user.Nome = dto.Nome;
        user.Cognome = dto.Cognome;
        user.CodiceFiscale = dto.CodiceFiscale;
        user.DataDiNascita = dto.DataDiNascita;
        user.NumeroDiTelefono = dto.NumeroDiTelefono; 

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = "Profilo aggiornato con successo." });
    }

}
