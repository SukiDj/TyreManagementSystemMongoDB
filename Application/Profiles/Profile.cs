using Domain;

namespace Application.Profiles;

public class Profile
{
    public string Username { get; set; }
    public string Ime { get; set; }
    public string Prezime { get; set; }
    public string Telefon { get; set; }
    public DateTime DatumRodjenja { get; set; }
}