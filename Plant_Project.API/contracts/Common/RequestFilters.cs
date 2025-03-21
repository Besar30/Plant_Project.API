namespace Plant_Project.API.contracts.Common
{
    public record RequestFilters
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
