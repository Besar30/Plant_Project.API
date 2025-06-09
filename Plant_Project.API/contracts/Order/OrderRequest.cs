namespace Plant_Project.API.contracts.Order;

public record OrderRequest(
	int? OrdrerId, 
	string UserId
	);
