using HotelBackend.Models;
using HotelBackend.Repositories;
using HotelBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using FluentValidation;
using HotelBackend.DTOs;
using HotelBackend.Validators;
using System.Text;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
    throw new InvalidOperationException("Chiave JWT non trovata nella configurazione.");

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();


builder.Services.AddScoped<IValidator<RegisterDTO>, RegisterDTOValidator>();
builder.Services.AddScoped<IValidator<LoginDTO>, LoginDTOValidator>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value!.Errors.Count > 0)
            .ToDictionary(
                e => e.Key,
                e => e.Value!.Errors.Select(err => err.ErrorMessage).ToArray()
            );

        return new BadRequestObjectResult(new { errors });
    };
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICameraRepository, CameraRepository>();
builder.Services.AddScoped<ICameraService, CameraService>();
builder.Services.AddScoped<IPrenotazioneRepository, PrenotazioneRepository>();
builder.Services.AddScoped<IPrenotazioneService, PrenotazioneService>();
builder.Services.AddScoped<IServizioExtraRepository, ServizioExtraRepository>();
builder.Services.AddScoped<IServizioExtraService, ServizioExtraService>();
builder.Services.AddHostedService<AggiornaDisponibilitaCamereService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelBackend API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Inserisci il token JWT come: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var dbContext = services.GetRequiredService<ApplicationDbContext>();

  
    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    if (!await roleManager.RoleExistsAsync("User"))
        await roleManager.CreateAsync(new IdentityRole("User"));

   
    var adminEmail = "admin@hotel.com";
    var admin = await userManager.FindByEmailAsync(adminEmail);
    if (admin == null)
    {
        admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail
        };
        await userManager.CreateAsync(admin, "Admin123!");
    }

    var roles = await userManager.GetRolesAsync(admin);
    if (!roles.Contains("Admin"))
        await userManager.AddToRoleAsync(admin, "Admin");

    
    var userEmail = "user@hotel.com";
    var user = await userManager.FindByEmailAsync(userEmail);
    if (user == null)
    {
        user = new ApplicationUser
        {
            UserName = userEmail,
            Email = userEmail,
            Nome = "Mario",
            Cognome = "Rossi",
            CodiceFiscale = "RSSMRA80A01H501Z",
            DataDiNascita = new DateTime(1980, 1, 1),
        };
        await userManager.CreateAsync(user, "User123!");
    }

    var userRoles = await userManager.GetRolesAsync(user);
    if (!userRoles.Contains("User"))
        await userManager.AddToRoleAsync(user, "User");

   
    if (!await dbContext.ServiziExtra.AnyAsync())
    {
        dbContext.ServiziExtra.AddRange(
            new ServizioExtra { Nome = "Colazione inclusa", Prezzo = 10m, Disponibile = true },
            new ServizioExtra { Nome = "Spa accesso giornaliero", Prezzo = 25m, Disponibile = true },
            new ServizioExtra { Nome = "Parcheggio custodito", Prezzo = 8m, Disponibile = true }
        );
        await dbContext.SaveChangesAsync();
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
