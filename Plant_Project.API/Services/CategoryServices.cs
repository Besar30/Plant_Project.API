using Mapster;
using Plant_Project.API.Abstraction;
using Plant_Project.API.contracts.Categorys;
using Plant_Project.API.contracts.Plants;
using Plant_Project.API.Errors;
namespace Plant_Project.API.Services
{
    public class CategoryServices(ApplicationDbContext Context,IHttpContextAccessor httpContextAccessor) : ICategoryServices
    {
        private readonly ApplicationDbContext _Context = Context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<List<CategoryResponse>>> GetAllCategoriesAsync(CancellationToken cancellationToken)
        {
           var result= await _Context.categories.AsNoTracking().ToListAsync(cancellationToken);

            var categoryRespons = result.Select(x => new CategoryResponse(
                x.Id,
                x.Name,
                x.Description,
                x.ImagePath
                )).ToList();

           return Result.Success(categoryRespons);
        }
        public async Task<Result<CategoryResponse>> GetCategoryByIdAsync(int Id, CancellationToken cancellation)
        {
            var result = await _Context.categories.Where(x => x.Id == Id).FirstOrDefaultAsync(cancellation);

            if (result == null) { 
              return Result.Failure<CategoryResponse>(CategoryError.CategoryNotFound);
            }
            //var category=result.Adapt<CategoryResponse>();
            var category = new CategoryResponse(
                result.Id,
                result.Name,
                result.Description,
                result.ImagePath
                );
            return Result.Success(category);
        }

        public async Task<Result> AddCategoryAsync(CategoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _Context.categories.AnyAsync(x=>x.Name==request.Name,cancellationToken);
            if (result)
                return Result.Failure(CategoryError.CategoryDublicated);

            string imagePath = await SaveImageAsync(request.ImagePath);
            var absUri = $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{imagePath}";


            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                ImagePath= absUri
            };
            await _Context.AddAsync(category, cancellationToken);
            await _Context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> UpdateCategoryAsync(int categoryId,CategoryRequest request, CancellationToken cancellationToken)
        {
            var category = await _Context.categories.Where(x => x.Id == categoryId).FirstOrDefaultAsync(cancellationToken);
            if (category == null) {
                return Result.Failure(CategoryError.CategoryNotFound);
            }
            var result = await _Context.categories.AnyAsync(x => x.Name == request.Name && x.Id!=categoryId, cancellationToken);
            if (result)
                return Result.Failure(CategoryError.CategoryDublicated);
            string imagePath = await SaveImageAsync(request.ImagePath);
            var absUri = $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{imagePath}";

            category.Description = request.Description;
            category.Name = request.Name;
            category.ImagePath = absUri;
            await _Context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        public async Task<Result<List<PlantsResponse>>> GetAllPlantByCategoryName(string categoryName, CancellationToken cancellationToken)
        {
            var category = await _Context.categories.Where(x => x.Name == categoryName).FirstOrDefaultAsync(cancellationToken);
            if(category == null)
            {
                return Result.Failure<List<PlantsResponse>>(CategoryError.CategoryNotFound);
            }
            var plants = await _Context.plants
          .Where(x => x.CategoryId == category.Id)
          .Select(x => new PlantsResponse(
              x.Id,
              x.Name,
              x.Price,
              x.Description,
              x.How_To_Plant,
              x.Quantity,
              x.ImagePath,
              x.Is_Avilable,
              categoryName
          ))
          .ToListAsync(cancellationToken);
            return Result.Success(plants);
        }
        public async Task<Result> DeleteCategoryAsync(int categoryId, CancellationToken cancellationToken)
        {
            var result = await _Context.categories.Where(x => x.Id == categoryId).FirstOrDefaultAsync(cancellationToken);
            if (result == null) { 
              return Result.Failure(CategoryError.CategoryNotFound);
            }
            _Context.categories.Remove(result);
            await _Context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }


        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return "/images/" + uniqueFileName;
        }
    }
}