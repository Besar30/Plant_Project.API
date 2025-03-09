namespace Plant_Project.API.contracts.Authentication;

public record ComfirmEamilRequest(
	   string UserId,
	   string Code
	   );