using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Plant_Project.API.persistence.EntityConfigaration
{
    public class UserRoleConfigration : IEntityTypeConfiguration<IdentityUserRole<string>>

    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            //builder.HasData(
            //     new IdentityUserRole<string>
            //     {
            //         UserId=DefaultUsers.AdminId,
            //         RoleId=DefaultRoles.AdminRoleId
            //     }
            //    );
        }
    }
}
