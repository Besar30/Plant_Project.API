namespace Plant_Project.API.Errors;

public class PaymentErrors
{
	public static readonly Error PaymentFailer =
		new("User.PaymentFailer","Fail transaction method");

	public static readonly Error MissingCardDetails =
		new("User.MissingCardDetails", "Missing Card Details");
	
	public static readonly Error PaymentMethodRequaird =
		new("User.PaymentMethodRequaird", "Payment Method Requaird");
	
	public static readonly Error InValidCardNumber =
		new("User.IsValidCardNumber", "Invalid card number");

	public static readonly Error InValidCVV =
		new("User.IsValidCVV", "Invalid cvv number");

	public static readonly Error InValidExpiryDate =
		new("User.IsValidExpiryDate", "Invalid Expiry date");





}
