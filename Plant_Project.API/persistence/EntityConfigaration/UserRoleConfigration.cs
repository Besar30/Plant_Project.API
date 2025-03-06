using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Plant_Project.API.Abstraction.Consts;

namespace Plant_Project.API.persistence.EntityConfigaration
{
    public class UserRoleConfigration : IEntityTypeConfiguration<IdentityUserRole<string>>

    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                 new IdentityUserRole<string>
                 {
                     UserId = DefaultUsers.AdminId,
                     RoleId = DefaultRoles.AdminRoleId
                 }
                );
        }
    }
}
