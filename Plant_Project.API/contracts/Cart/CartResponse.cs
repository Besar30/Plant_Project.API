namespace Plant_Project.API.contracts.Cart;

public record CartResponse
(
	 int Id,
	 int PlantId,
	 string? PlantName,
	 int Quantity,
	 decimal Price,
	 decimal TotalPrice,
	 string ImagePath

);
