using Microsoft.AspNetCore.Identity;

namespace RealStats.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public bool IsManager { get; set; }
    }
}
