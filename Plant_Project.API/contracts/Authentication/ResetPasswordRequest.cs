namespace Plant_Project.API.Contracts.Authentication;

public record ResetPasswordRequest(
    string Email,
    string NewPassword,
	string ConfirmPassword,
	string Token
);