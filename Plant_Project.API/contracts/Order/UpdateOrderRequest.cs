namespace Plant_Project.API.contracts.Order;

public record UpdateOrderRequest(
	int OrderId,
	string Address,
	string PaymentMethod
	);
