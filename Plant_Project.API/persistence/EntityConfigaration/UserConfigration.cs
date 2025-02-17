using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Plant_Project.API.persistence.EntityConfigaration
{
    public class UserConfigration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.OwnsMany(x => x.RefreshTokens)
               .ToTable("RefreshTokens")
               .WithOwner()
               .HasForeignKey("UserId");
            builder.Property(x => x.FirstName).HasMaxLength(100);
            builder.Property(x=>x.LastName).HasMaxLength(100);
            
            var passwordHasher=new PasswordHasher<ApplicationUser>();

            //Default data
            //builder.HasData(
            //    new ApplicationUser
            //    {
            //        Id=DefaultUsers.AdminId,
            //        FirstName="Plant-Project",
            //        LastName="Admin",
            //        UserName=DefaultUsers.AdminEmail,
            //        NormalizedUserName=DefaultUsers.AdminEmail.ToUpper(),
            //        Email=DefaultUsers.AdminEmail,
            //        NormalizedEmail=DefaultUsers.AdminEmail.ToUpper(),
            //        SecurityStamp=DefaultUsers.AdminSecurityStamp,
            //        ConcurrencyStamp=DefaultUsers.AdminConcurrencyStamp,
            //        EmailConfirmed=false,
            //        PasswordHash=passwordHasher.HashPassword(null!,DefaultUsers.AdminPassword)
            //    });
        }
    }
}
