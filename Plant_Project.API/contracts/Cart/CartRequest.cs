namespace Plant_Project.API.contracts.Cart;

public record CartRequest(
	string UserId,
	int PlantId,
	string PlantName,
	int Quantity
	
);
