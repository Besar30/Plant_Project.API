namespace Plant_Project.API.contracts.Plants;

public record PlantsRequest(
		string Name,
		decimal Price,
		string Description,
		string How_To_Plant,
		int Quantity,
		string ImageUrl,
		bool Is_Available,
		int CategoryId
	);
