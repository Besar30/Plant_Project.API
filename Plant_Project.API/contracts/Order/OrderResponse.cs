namespace Plant_Project.API.contracts.Order;

public record OrderResponse(
	
	string UserId,
	int OrderId,
	string PlantName,
	string Address,
	string PaymentMethod,
	decimal TotalAmount,
	DateTime OrderDate,
	string PaymentStatus
	);
