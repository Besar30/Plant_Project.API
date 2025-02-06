namespace Plant_Project.API.Contracts.Users;

public record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword
);