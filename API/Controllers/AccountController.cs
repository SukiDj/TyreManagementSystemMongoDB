using System.Security.Claims;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly TokenService _tokenService;
    private readonly DataContext _context;

    private readonly SignInManager<User> _signInManager;// dodato za promenu sifre

    public AccountController(UserManager<User> userManager, TokenService tokenService, DataContext context, SignInManager<User> signInManager)
    {
        _context = context;
        _tokenService = tokenService;
        _userManager = userManager;

        _signInManager = signInManager;// dodato za promenu sifre
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.Email == loginDto.Email);

        if (user == null) return BadRequest($"Unet je netacan email: {loginDto.Email}");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);//aspnet ovde proverava sifru da l se poklapa za nas

        if (result.Succeeded)
        {
            await SetRefreshToken(user);
            var userObject = await CreateUserObject(user);
            return userObject;
        }

        return BadRequest("Uneta sifra nije tacna");
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
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
        if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
        {
            ModelState.AddModelError("username", "Username taken");
            return ValidationProblem();
        }

        if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
        {
            ModelState.AddModelError("email", "Email taken");
            return ValidationProblem();
        }

        User user;
        switch (registerDto.Role)
        {
            case "ProductionOperator":
                user = new ProductionOperator
                {
                    Ime = registerDto.Ime,
                    Prezime = registerDto.Prezime,
                    Email = registerDto.Email,
                    UserName = registerDto.Username,
                    Telefon = registerDto.Telefon,
                    DatumRodjenja = registerDto.DatumRodjenja
                };
                break;

            case "QualitySupervisor":
                user = new QualitySupervisor
                {
                    Ime = registerDto.Ime,
                    Prezime = registerDto.Prezime,
                    Email = registerDto.Email,
                    UserName = registerDto.Username,
                    Telefon = registerDto.Telefon,
                    DatumRodjenja = registerDto.DatumRodjenja
                };
                break;

            case "BusinessUnitLeader":
                user = new BusinessUnitLeader
                {
                    Ime = registerDto.Ime,
                    Prezime = registerDto.Prezime,
                    Email = registerDto.Email,
                    UserName = registerDto.Username,
                    Telefon = registerDto.Telefon,
                    DatumRodjenja = registerDto.DatumRodjenja
                };
                break;

            default:
                user = new User
                {
                    Ime = registerDto.Ime,
                    Prezime = registerDto.Prezime,
                    Email = registerDto.Email,
                    UserName = registerDto.Username,
                    Telefon = registerDto.Telefon,
                    DatumRodjenja = registerDto.DatumRodjenja
                };
                break;
        }

        var result = await _userManager.CreateAsync(user, registerDto.Password);//da sacuvamo usera u bazu

        if (!result.Succeeded) return BadRequest("Problem with registration");

        result = await _userManager.AddToRoleAsync(user, registerDto.Role);
        
        if (!result.Succeeded) return BadRequest("Problem with adding role to user");

        return Ok("Registration succesfull");
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));
        if(user != null){
            await SetRefreshToken(user);// NOVO
        }
        var userObject = await CreateUserObject(user);
        return userObject;
    }

    private async Task<UserDto> CreateUserObject(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        return new UserDto
        {
            Ime = user.Ime,
            Prezime = user.Prezime,
            Token = await _tokenService.CreateToken(user),
            UserName = user.UserName
        };
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

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        if (result.Succeeded)
        {
            await _signInManager.RefreshSignInAsync(user);
            return Ok("Sifra je uspesno promenjena.");
        }

        var errorDescriptions = result.Errors.Select(error => error.Description);
        return BadRequest("Greska pri promeni sifre: " + string.Join("; ", errorDescriptions));
    }

    [Authorize]
    [HttpPost("refreshToken")]
    public async Task<ActionResult<UserDto>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var user = await _userManager.Users
            .Include(r => r.RefreshTokens)
            .FirstOrDefaultAsync(x => x.UserName == User.FindFirstValue(ClaimTypes.Name));

        if (user == null) return Unauthorized();

        var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

        if (oldToken != null && !oldToken.IsActive) return Unauthorized();

        var userObject = await CreateUserObject(user);
        return userObject;
    }

    private async Task SetRefreshToken(User user)
    {
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshTokens.Add(refreshToken);
        await _userManager.UpdateAsync(user);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
    }
}