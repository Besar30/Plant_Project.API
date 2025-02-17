using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Plant_Project.API.Migrations;

namespace Plant_Project.API.persistence.EntityConfigaration
{
    public class PlantConfigration : IEntityTypeConfiguration<Plant>
    {
        public void Configure(EntityTypeBuilder<Plant> builder)
        {
            builder.Property(p => p.Is_Avilable)
                   .HasDefaultValue(true);

            builder.Property(p => p.Name)
                  .IsRequired() 
                  .HasMaxLength(100); 

            builder.Property(p => p.Description)
                  .HasMaxLength(2000);

            builder.Property(p => p.How_To_Plant)
                  .HasMaxLength(2000); 

            
            builder.Property(p => p.ImagePath)
                  .HasMaxLength(2000);
        }
    }
}
