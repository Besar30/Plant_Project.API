using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Plant_Project.API.persistence.EntityConfigaration
{
    public class CategoryConfigration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(100);
            builder.Property(x=>x.Description).HasMaxLength(2000);
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
