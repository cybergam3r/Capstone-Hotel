using HotelBackend.DTOs;
using HotelBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelBackend.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _config;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
        _roleManager = roleManager;
    }

    public async Task<bool> RegisterAsync(RegisterDTO dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded) return false;

        const string defaultRole = "User";

        if (!await _roleManager.RoleExistsAsync(defaultRole))
        {
            await _roleManager.CreateAsync(new IdentityRole(defaultRole));
        }

        await _userManager.AddToRoleAsync(user, defaultRole);

        return true;
    }

    public async Task<AuthResponseDTO?> LoginAsync(LoginDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return null;

        var roles = await _userManager.GetRolesAsync(user);
        if (roles == null || roles.Count == 0)
            return null;

        var role = roles.First(); 

        
        Console.WriteLine($"[Login] {user.Email} ha il ruolo: {role}");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(4),
            signingCredentials: creds
        );

        return new AuthResponseDTO
        {
            Email = user.Email!,
            Role = role,
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };
    }
}
