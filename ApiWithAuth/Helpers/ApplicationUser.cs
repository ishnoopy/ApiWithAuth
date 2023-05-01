using Microsoft.AspNetCore.Identity;

namespace ApiWithAuth.Helpers
{
    public class ApplicationUser: IdentityUser
    {
        public string Role { get; set; }
    }
}
