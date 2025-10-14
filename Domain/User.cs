using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class User : IdentityUser<Guid>
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Telefon { get; set; }
        public DateTime DatumRodjenja { get; set; }
         public ICollection<UserRole> UserRoles { get; set; } = [];
         public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}