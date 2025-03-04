namespace Plant_Project.API.persistence.EntityConfigaration;

public class PaymentConfiguration:IEntityTypeConfiguration<Payment>
{
	public void Configure(EntityTypeBuilder<Payment> builder)
	{
		builder
		.HasOne(p => p.Order)
		.WithMany(o => o.Payments) 
		.HasForeignKey(p => p.OrderId)
		.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(p => p.User)
			.WithMany(o=>o.Payments) // Adjust if the user has multiple payments
			.HasForeignKey(p => p.UserId)
			.OnDelete(DeleteBehavior.Restrict);
	}

}
