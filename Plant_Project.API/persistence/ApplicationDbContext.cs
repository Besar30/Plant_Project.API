using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace Plant_Project.API.persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole,string>
    {
        public DbSet<Plant> plants { get; set; }
        public DbSet<Category> categories { get; set; }
         public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        { 

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
