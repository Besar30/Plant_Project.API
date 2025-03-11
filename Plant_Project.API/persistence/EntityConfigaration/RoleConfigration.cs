using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Plant_Project.API.Abstraction.Consts;

namespace Plant_Project.API.persistence.EntityConfigaration
{
    public class RoleConfigration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            // بيانات افتراضية
            builder.HasData([
                new ApplicationRole
                {
                    Id = DefaultRoles.AdminRoleId,
                    Name = DefaultRoles.Admin,
                    NormalizedName = DefaultRoles.Admin.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp
                },
                new ApplicationRole
                {
                    Id = DefaultRoles.MemberRoleId,
                    Name = DefaultRoles.Member,
                    NormalizedName = DefaultRoles.Member.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.MemberRoleConcurrencyStamp,
                    IsDefault = true // تعيين دور الافتراضي للمستخدم العادي
                }
           ] );
        }
    }
}
