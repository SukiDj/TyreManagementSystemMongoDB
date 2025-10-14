using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public User User { get; set; }
        public AppRole Role { get; set; }
    }
}