namespace Plant_Project.API.Entities;

public class Order
{
	public int Id { get; set; } // Primary Key

	public string? UserId { get; set; } // Foreign Key from AspNetUsers
	public ApplicationUser? User { get; set; }

	[Precision(18, 2)]
	public decimal TotalAmount { get; set; } // Total cost of the order

	public DateTime OrderDate { get; set; } = DateTime.UtcNow; // Order timestamp

	public string? Address { get; set; }
	public string PaymentMethod { get; set; } = string.Empty; // Example: "Credit Card", "PayPal"

	public string PaymentStatus { get; set; } = "Pending"; // Possible values: Pending, Paid, Failed

	public string? TransactionId { get; set; } // Payment Gateway Transaction ID

	public ICollection<OrderItem>? OrderItems { get; set; } // List of plants in the order
	public ICollection<Payment>? Payments { get; set; }
}

