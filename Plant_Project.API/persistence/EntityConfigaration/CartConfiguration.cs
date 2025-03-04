namespace Plant_Project.API.persistence.EntityConfigaration;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
	public void Configure(EntityTypeBuilder<Cart> builder)
	{
		builder.HasKey(c => c.Id);

		builder.HasOne(c => c.User)
			   .WithMany(u => u.Carts)
			   .HasForeignKey(c => c.UserId)
			   .OnDelete(DeleteBehavior.Cascade); 

		builder.HasOne(c => c.Plant)
			   .WithMany(p => p.Carts)
			   .HasForeignKey(c => c.PlantId)
			   .OnDelete(DeleteBehavior.Cascade); 
	}
}

