using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Workshop05.Models;

namespace Workshop05.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Advertisement> Advertisements { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}