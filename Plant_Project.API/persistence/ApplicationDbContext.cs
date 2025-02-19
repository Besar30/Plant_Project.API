using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;


namespace Plant_Project.API.persistence
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
			: IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
	{
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;


		public DbSet<Plant> Plants { get; set; }
		public DbSet<Category> categories { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.ConfigureWarnings(warnings =>
	   warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
		}
		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var entries = ChangeTracker.Entries<AuditableEntity>();

			foreach (var entityEntry in entries)
			{
				var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId()!;

				if (entityEntry.State == EntityState.Added)
				{
					entityEntry.Property(x => x.CreatedById).CurrentValue = currentUserId;
				}
				else if (entityEntry.State == EntityState.Modified)
				{
					entityEntry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
					entityEntry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
				}
			}

			return base.SaveChangesAsync(cancellationToken);
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(modelBuilder);
		}
	}
}
