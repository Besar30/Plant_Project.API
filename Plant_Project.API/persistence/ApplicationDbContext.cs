using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Plant_Project.API.Entities;
using System.Reflection;

namespace Plant_Project.API.persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
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
