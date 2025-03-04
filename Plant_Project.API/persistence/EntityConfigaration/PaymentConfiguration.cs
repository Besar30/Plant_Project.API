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

	}

}
