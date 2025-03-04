namespace Plant_Project.API.contracts.Payment;

public record CheckoutRequest(
	string UserId,
	string PaymentMethod,
	string Address,
	PaymentCardDetails? CardDetails
	);
