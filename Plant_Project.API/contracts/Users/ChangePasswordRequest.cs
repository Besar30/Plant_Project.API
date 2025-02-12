namespace Plant_Project.API.contracts.Users;

public record ChangePasswordRequest(
	string CurrentPassword,
	string NewPassword
);