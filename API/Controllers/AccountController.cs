using System.Security.Claims;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using BCrypt.Net;
using Persistence;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly MongoDbContext _context;

    public AccountController(TokenService tokenService, MongoDbContext context)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _context.Users.Find(u => u.Email == loginDto.Email).FirstOrDefaultAsync();
        if (user == null) return BadRequest($"Unet je netacan email: {loginDto.Email}");

        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            return BadRequest("Uneta sifra nije tacna");

        await SetRefreshToken(user);
        return await CreateUserObject(user);
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // MongoDB nema session, logout samo brise cookie
        Response.Cookies.Delete("refreshToken");
        return Ok("Logout successful");
    }

    [Authorize]
    [HttpGet("access-denied")]
    public IActionResult AccessDenied()
    {
        return Unauthorized("Access denied");
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await _context.Users.Find(u => u.Username == registerDto.Username).AnyAsync())
            return ValidationProblem(new ValidationProblemDetails { Title = "Username taken" });

        if (await _context.Users.Find(u => u.Email == registerDto.Email).AnyAsync())
            return ValidationProblem(new ValidationProblemDetails { Title = "Email taken" });

        var user = new User
        {
            Ime = registerDto.Ime,
            Prezime = registerDto.Prezime,
            Email = registerDto.Email,
            Username = registerDto.Username,
            Telefon = registerDto.Telefon,
            DatumRodjenja = registerDto.DatumRodjenja,
            Role = registerDto.Role,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
        };

        await _context.Users.InsertOneAsync(user);
        return Ok("Registration successful");
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        var user = await _context.Users.Find(u => u.Username == username).FirstOrDefaultAsync();
        if (user == null) return Unauthorized();

        await SetRefreshToken(user);
        return await CreateUserObject(user);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();
            return BadRequest(new { message = "ModelState nije validan", errors });
        }

        var username = User.FindFirstValue(ClaimTypes.Name);
        var user = await _context.Users.Find(u => u.Username == username).FirstOrDefaultAsync();
        if (user == null) return Unauthorized();

        if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, user.PasswordHash))
            return BadRequest("Stara sifra nije tacna");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
        var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
        await _context.Users.ReplaceOneAsync(filter, user);

        return Ok("Sifra je uspesno promenjena.");
    }

    [Authorize]
    [HttpPost("refreshToken")]
    public async Task<ActionResult<UserDto>> RefreshToken()
    {
        var refreshTokenValue = Request.Cookies["refreshToken"];
        var username = User.FindFirstValue(ClaimTypes.Name);
        var user = await _context.Users.Find(u => u.Username == username).FirstOrDefaultAsync();

        if (user == null) return Unauthorized();

        var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshTokenValue);
        if (oldToken == null || !oldToken.IsActive) return Unauthorized();

        return await CreateUserObject(user);
    }

    private async Task<UserDto> CreateUserObject(User user)
    {
        return new UserDto
        {
            Ime = user.Ime,
            Prezime = user.Prezime,
            Token = await _tokenService.CreateToken(user),
            UserName = user.Username
        };
    }

    private async Task SetRefreshToken(User user)
    {
        var refreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshTokens.Add(refreshToken);

        var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
        await _context.Users.ReplaceOneAsync(filter, user);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
    }
}
