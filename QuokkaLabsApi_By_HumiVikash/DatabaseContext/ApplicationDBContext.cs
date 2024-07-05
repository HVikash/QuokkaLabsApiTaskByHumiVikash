using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuokkaLabsApi_By_HumiVikash.Models;

namespace QuokkaLabsApi_By_HumiVikash.DatabaseContext
{
    public class ApplicationDBContext :IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options) 
        {
            
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Articles> Articles { get; set; }
    }
}
