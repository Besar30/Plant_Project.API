namespace Plant_Project.API.contracts.Payment;

public record PaymentResult(
	 bool Success,
	 string TransactionId
	);
