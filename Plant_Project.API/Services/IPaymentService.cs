using Plant_Project.API.contracts.Payment;

namespace Plant_Project.API.Services;

public interface IPaymentService
{
	Task<Result> CheckoutAsync(CheckoutRequest request, CancellationToken cancellationToken);
}
