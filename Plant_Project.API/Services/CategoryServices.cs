using Azure;
using Plant_Project.API.Abstraction;
using Plant_Project.API.contracts.Categorys;
using Plant_Project.API.contracts.Common;
using Plant_Project.API.contracts.Plants;
namespace Plant_Project.API.Services
{
    public class CategoryServices(ApplicationDbContext Context,IHttpContextAccessor httpContextAccessor,IcacheService icacheService,ILogger<CategoryServices> logger) : ICategoryServices
    {
        private readonly ApplicationDbContext _Context = Context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IcacheService _icacheService = icacheService;
        private readonly ILogger<CategoryServices> _logger = logger;
        private const string _cachePerfix = "availableCategory";
        public async Task<Result<List<CategoryResponse>>> GetAllCategoriesAsync(RequestFilters Filters, CancellationToken cancellationToken)
        {
            var result = await _Context.categories
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var categories = result
                .Select(x => new CategoryResponse(
                    x.Id,
                    x.Name,
                    x.Description,
                    $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{x.ImagePath}"
                ))
                .ToList();

            return Result.Success(categories);
        }

        public async Task<Result<CategoryResponse>> GetCategoryByIdAsync(int Id, CancellationToken cancellation)
        {
            var cacheKey = $"{_cachePerfix}-{Id}";
            var category = await _icacheService.GetAsync<CategoryResponse>(cacheKey, cancellation);
            
            if (category is not null)
            {
                _logger.LogInformation("get by cache");
               return Result.Success(category);
            }
            _logger.LogInformation("get by database");

            var result = await _Context.categories.Where(x => x.Id == Id).FirstOrDefaultAsync(cancellation);

                if (result == null)
                {
                    return Result.Failure<CategoryResponse>(CategoryError.CategoryNotFound);
                }
                category = new CategoryResponse(
                    result.Id,
                    result.Name,
                    result.Description,
                    $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{result.ImagePath}"
                    );
                await _icacheService.SetAsync(cacheKey,category, cancellation);
            return Result.Success(category);
        }

        public async Task<Result> AddCategoryAsync(CategoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _Context.categories.AnyAsync(x=>x.Name==request.Name,cancellationToken);
            if (result)
                return Result.Failure(CategoryError.CategoryDublicated);

            string imagePath = await SaveImageAsync(request.ImagePath);
            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                ImagePath= imagePath
            };
            await _Context.AddAsync(category, cancellationToken);
            await _Context.SaveChangesAsync(cancellationToken);
           
            return Result.Success();
        }

        public async Task<Result> UpdateCategoryAsync(int categoryId, CategoryRequest request, CancellationToken cancellationToken)
        {
            var category = await _Context.categories
                .Where(x => x.Id == categoryId)
                .FirstOrDefaultAsync(cancellationToken);

            if (category == null)
            {
                return Result.Failure(CategoryError.CategoryNotFound);
            }

            var isDuplicate = await _Context.categories
                .AnyAsync(x => x.Name == request.Name && x.Id != categoryId, cancellationToken);

            if (isDuplicate)
                return Result.Failure(CategoryError.CategoryDublicated);

            // ✅ الصح إننا نفحص الـ request.ImagePath مش القديمة
            if (request.ImagePath != null)
            {
                string imagePath = await SaveImageAsync(request.ImagePath);
                category.ImagePath = imagePath;
            }

            category.Description = request.Description;
            category.Name = request.Name;

            await _Context.SaveChangesAsync(cancellationToken);

            var cacheKey = $"{_cachePerfix}-{categoryId}";
            await _icacheService.RemoveAsync(cacheKey, cancellationToken);

            return Result.Success();
        }

        public async Task<Result<PaginatedList<PlantsResponse>>> GetAllPlantByCategoryName(string categoryName,RequestFilters filters, CancellationToken cancellationToken)
        {
            var category = await _Context.categories.Where(x => x.Name == categoryName).FirstOrDefaultAsync(cancellationToken);
            if(category == null)
            {
                return Result.Failure<PaginatedList<PlantsResponse>>(CategoryError.CategoryNotFound);
            }
            var Query =  _Context.plants
          .Where(x => x.CategoryId == category.Id)
          .Select(x => new PlantsResponse(
              x.Id,
              x.Name,
              x.Price,
              x.Description,
              x.How_To_Plant,
              x.Quantity,
      $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{x.ImagePath}",
              x.Is_Avilable,
              categoryName
          ));
          var plants=await PaginatedList< PlantsResponse >.CreateAsync(Query,filters.PageNumber,filters.PageSize,cancellationToken);
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
            var cacheKey = $"{_cachePerfix}_all";
            await _icacheService.RemoveAsync(cacheKey, cancellationToken);
            cacheKey = $"{cacheKey}-{categoryId}";
            await _icacheService.RemoveAsync(cacheKey, cancellationToken);
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