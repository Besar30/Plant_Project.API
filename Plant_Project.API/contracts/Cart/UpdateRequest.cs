namespace Plant_Project.API.contracts.Cart;

public record UpdateRequest(
	
	int CartId,
	int Quantity
	
	);