namespace Plant_Project.API.Entities;

public class Payment
{
	public int Id { get; set; }
	public string UserId { get; set; } = string.Empty; // Foreign key from Identity
	public ApplicationUser? User { get; set; }
	public int OrderId { get; set; }// Link to order
	public Order? Order { get; set; }

	[Precision(18, 2)]
	public decimal Amount { get; set; }
	public string Currency { get; set; } = "USD";
	public string PaymentIntentId { get; set; } = string.Empty; // Stripe PaymentIntent ID
	public string Status { get; set; } = "Pending"; // "Succeeded", "Failed", "Pending"
	public string? FailureReason { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime? UpdatedAt { get; set; }
}

