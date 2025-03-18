using System.Text.RegularExpressions;

namespace Plant_Project.API.contracts.Payment;

public static class CheckoutValidator
{
	public static bool IsValidCardNumber(string cardNumber)
	{
		cardNumber = cardNumber.Replace(" ", ""); // Remove spaces

		if (!Regex.IsMatch(cardNumber, "^[0-9]{13,19}$"))
			return false; // Invalid length

		return LuhnCheck(cardNumber);
	}

	private static bool LuhnCheck(string cardNumber)
	{
		int sum = 0;
		bool alternate = false;

		for (int i = cardNumber.Length - 1; i >= 0; i--)
		{
			int digit = cardNumber[i] - '0';

			if (alternate)
			{
				digit *= 2;
				if (digit > 9) digit -= 9;
			}

			sum += digit;
			alternate = !alternate;
		}

		return (sum % 10 == 0);
	}

	public static bool IsValidExpiryDate(string expiryDate)
	{
		if (!Regex.IsMatch(expiryDate, "^(0[1-9]|1[0-2])/(\\d{2}|\\d{4})$"))
			return false; // Invalid format MM/YY or MM/YYYY

		string[] parts = expiryDate.Split('/');
		int month = int.Parse(parts[0]);
		int year = parts[1].Length == 2 ? 2000 + int.Parse(parts[1]) : int.Parse(parts[1]);

		DateTime expiry = new DateTime(year, month, DateTime.DaysInMonth(year, month));
		return expiry > DateTime.UtcNow;
	}

	public static bool IsValidCVV(string cvv, string cardType)
	{
		return cardType switch
		{
			"Amex" => Regex.IsMatch(cvv, "^[0-9]{4}$"),
			"Visa" or "MasterCard" or "Discover" => Regex.IsMatch(cvv, "^[0-9]{3}$"),
			_ => false
		};
	}
}

