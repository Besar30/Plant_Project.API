

namespace Plant_Project.API.Errors
{
    public class PlantsErrors
    {
	
		public static readonly Error DisabledUser =
			 new("User.DisabledUser", "Disabled user, please contact your administrator", StatusCodes.Status401Unauthorized);

		public static readonly Error DuplicatedPlantTitle =
			new("User.DuplicatedPlantTitle", "Plant already exist", StatusCodes.Status409Conflict);

		public static readonly Error InvalidRefreshToken =
			new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);

		public static readonly Error PlantNotFound =
			new("User.PlantNotFound", "Plant is not found", StatusCodes.Status404NotFound);

		public static readonly Error InvalidRoles =
			new("Role.InvalidRoles", "Invalid roles", StatusCodes.Status400BadRequest);
	}
}
