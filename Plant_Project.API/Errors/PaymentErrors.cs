namespace Plant_Project.API.Errors;

public class PaymentErrors
{
	public static readonly Error PaymentFailer =
		new("User.PaymentFailer","Fail transaction method", StatusCodes.Status400BadRequest);

	public static readonly Error MissingCardDetails =
		
		new("User.MissingCardDetails", "Missing Card Details", StatusCodes.Status400BadRequest);
	public static readonly Error PaymentMethodRequaird =
		new("User.PaymentMethodRequaird", "Payment Method Requaird", StatusCodes.Status400BadRequest);

}
