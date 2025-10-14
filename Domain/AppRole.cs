using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppRole : IdentityRole<Guid>
    {
        public ICollection<UserRole> UserRoles { get; set; } //veza prema klasi Korisnik
    }
}