namespace Plant_Project.API.persistence.EntityConfigaration;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
	public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
	{
		//Default Data
		builder.HasData(new IdentityUserRole<string>
		{
			UserId = DefaultUsers.AdminId,
			RoleId = DefaultRoles.AdminRoleId
		});
	}
}