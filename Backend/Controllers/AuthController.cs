using HotelBackend.DTOs;
using HotelBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        var success = await _authService.RegisterAsync(dto);
        if (!success) return BadRequest("Errore nella registrazione.");
        return Ok("Utente registrato.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (result == null) return Unauthorized("Credenziali non valide.");
        return Ok(result);
    }
}