namespace Plant_Project.API.contracts.Plants
{
    public record PlantsResponse(
        int Id,
        string Name,
        decimal Price,
        string Description,
        string How_To_Plant,
        int Quantity,
        string ImageUrl,
        bool Is_Available,
        string CategoryName
        )
    {
    };
 
}
