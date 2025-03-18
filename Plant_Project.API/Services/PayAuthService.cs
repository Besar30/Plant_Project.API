using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using Plant_Project.API.contracts.Payment;

namespace Plant_Project.API.Services;

public class PayAuthService
{
	private readonly string _apiLoginId;
	private readonly string _transactionKey;
	private readonly bool _sandbox;

	public PayAuthService(IConfiguration configuration)
	{
		_apiLoginId = configuration["AuthorizeNet:ApiLoginId"]!;
		_transactionKey = configuration["AuthorizeNet:TransactionKey"]!;
		_sandbox = bool.Parse(configuration["AuthorizeNet:Sandbox"]!);
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

		var request = new createTransactionRequest { merchantAuthentication = merchantAuth, transactionRequest = transactionRequest };
		var controller = new createTransactionController(request);
		controller.Execute();

		var response = controller.GetApiResponse();

		if (response != null && response.transactionResponse != null && response.transactionResponse.responseCode == "1")
		{
			return new PaymentResult(true, response.transactionResponse.transId);
		}

		return new PaymentResult(false, response?.transactionResponse?.errors?[0].errorText ?? "Transaction failed");
	}
}
