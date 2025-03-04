namespace Plant_Project.API.contracts.Payment;

public record PaymentCardDetails(
	string CardNumber,
	string ExpirationDate,
	string CVV
	);
