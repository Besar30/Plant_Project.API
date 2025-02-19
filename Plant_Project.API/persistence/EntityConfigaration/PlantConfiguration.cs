
namespace Plant_Project.API.persistence.EntityConfigaration;

public class PlantConfiguration : IEntityTypeConfiguration<Plant>
{
	public void Configure(EntityTypeBuilder<Plant> builder)
	{
		 builder.HasOne(p => p.Category)
				.WithMany(c => c.Plants)
				.HasForeignKey(p => p.CategoryId)
				.IsRequired();

	}
}
