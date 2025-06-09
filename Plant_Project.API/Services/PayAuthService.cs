using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using Plant_Project.API.contracts.Payment;

namespace Plant_Project.API.Services;

public class PayAuthService
{
	private readonly string _apiLoginId;
	private readonly string _transactionKey;
	private readonly bool _sandbox;

	public PayAuthService(IConfiguration configuration)
	{
		_apiLoginId = configuration["AuthorizeNet:ApiLoginId"]
			?? throw new ArgumentNullException("AuthorizeNet:ApiLoginId", "Missing Authorize.Net API Login ID in configuration.");

		_transactionKey = configuration["AuthorizeNet:TransactionKey"]
			?? throw new ArgumentNullException("AuthorizeNet:TransactionKey", "Missing Authorize.Net Transaction Key in configuration.");

		var sandboxValue = configuration["AuthorizeNet:Sandbox"]
			?? throw new ArgumentNullException("AuthorizeNet:Sandbox", "Missing Authorize.Net Sandbox flag in configuration.");

		if (!bool.TryParse(sandboxValue, out _sandbox))
			throw new FormatException("AuthorizeNet:Sandbox must be either 'true' or 'false' as string.");

		ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = _sandbox
			? AuthorizeNet.Environment.SANDBOX
			: AuthorizeNet.Environment.PRODUCTION;
	}

	public async Task<PaymentResult> ProcessPaymentAsync(decimal amount, string cardNumber, string expirationDate, string cvv)
	{
		var merchantAuth = new merchantAuthenticationType
		{
			name = _apiLoginId,
			Item = _transactionKey,
			ItemElementName = ItemChoiceType.transactionKey
		};

		var creditCard = new creditCardType
		{
			cardNumber = cardNumber,
			expirationDate = expirationDate,
			cardCode = cvv
		};

		var paymentType = new paymentType { Item = creditCard };

		var transactionRequest = new transactionRequestType
		{
			transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),
			amount = amount,
			payment = paymentType
		};

		var request = new createTransactionRequest
		{
			merchantAuthentication = merchantAuth,
			transactionRequest = transactionRequest
		};

		var controller = new createTransactionController(request);
		controller.Execute();

		var response = controller.GetApiResponse();

		if (response != null && response.transactionResponse != null)
		{
			if (response.transactionResponse.responseCode == "1")
			{
				return new PaymentResult(true, response.transactionResponse.transId);
			}
			else if (response.transactionResponse.errors != null && response.transactionResponse.errors.Length > 0)
			{
				var error = response.transactionResponse.errors[0];
				return new PaymentResult(false, $"{error.errorCode}: {error.errorText}");
			}
		}

		var message = response?.messages?.message?.FirstOrDefault()?.text ?? "Transaction failed for unknown reasons.";
		return new PaymentResult(false, message);
	}
}
