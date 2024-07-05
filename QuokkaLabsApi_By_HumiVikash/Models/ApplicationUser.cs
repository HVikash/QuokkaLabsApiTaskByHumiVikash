using Microsoft.AspNetCore.Identity;

namespace QuokkaLabsApi_By_HumiVikash.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string address { get; set; }
    }
}
