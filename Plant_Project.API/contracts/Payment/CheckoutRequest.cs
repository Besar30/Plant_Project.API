namespace Plant_Project.API.contracts.Payment;

public record CheckoutRequest(
	string UserId,
	string PaymentMethod,
	PaymentCardDetails? CardDetails
	);
