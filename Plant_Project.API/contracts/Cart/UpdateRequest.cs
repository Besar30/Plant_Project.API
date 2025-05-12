namespace Plant_Project.API.contracts.Cart;

public record UpdateRequest(
    string UserId,
    int ItemId,
    int Quantity
);