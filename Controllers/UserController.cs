using HotelBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")] 
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }


    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = _userManager.Users.ToList().Select(u => new
        {
            u.Id,
            u.Email,
            Roles = _userManager.GetRolesAsync(u).Result
        });

        return Ok(users);
    }

   
    [HttpPut("{id}/role")]
    public async Task<IActionResult> UpdateUserRole(string id, [FromBody] string newRole)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, newRole);

        return Ok("Ruolo aggiornato");
    }

  
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        await _userManager.DeleteAsync(user);
        return Ok("Utente eliminato");
    }
}