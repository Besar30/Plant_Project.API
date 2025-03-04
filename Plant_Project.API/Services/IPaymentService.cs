namespace Plant_Project.API.Services;

public interface IPaymentService
{
	Task<Result> CheckoutAsync(CheckoutRequest request, PaymentCardDetails? cardDetails, CancellationToken cancellationToken);
}
