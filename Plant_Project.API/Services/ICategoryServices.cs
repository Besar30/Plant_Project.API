using Plant_Project.API.Abstraction;
using Plant_Project.API.contracts.Categorys;
using Plant_Project.API.contracts.Common;
using Plant_Project.API.contracts.Plants;

namespace Plant_Project.API.Services
{
    public interface ICategoryServices
    {
        public Task<Result<PaginatedList<CategoryResponse>>> GetAllCategoriesAsync(RequestFilters Filters, CancellationToken cancellationToken);
        public Task<Result<CategoryResponse>> GetCategoryByIdAsync(int Id, CancellationToken cancellation);
        public Task<Result> AddCategoryAsync(CategoryRequest request,CancellationToken cancellationToken);
        public Task<Result> UpdateCategoryAsync(int categoryId,CategoryRequest request,CancellationToken cancellationToken);
        public Task<Result> DeleteCategoryAsync(int categoryId, CancellationToken cancellationToken);
        public Task<Result<PaginatedList<PlantsResponse>>> GetAllPlantByCategoryName(string categoryName,RequestFilters filters,CancellationToken cancellationToken);
        
    }
}
