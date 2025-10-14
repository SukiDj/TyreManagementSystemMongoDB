using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    // Lozinka mora da sadrzi izmedju 4 i 20 karaktera, makar jedan broj, jedno malo slovo i jedno veliko slovo
    [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,20}$", ErrorMessage = "Sifra mora da sadrzi izmedju 4 i 20 karaktera(makar jedan broj, jedno malo i jedno veliko slovo)!")]
    public string Password { get; set; }

    [Required]
    public string Ime { get; set; }
    [Required]
    public string Prezime { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Telefon { get; set; }

    [Required]
    public DateTime DatumRodjenja { get; set; }
    [Required]
    public string Role { get; set; }
    
}