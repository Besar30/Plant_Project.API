using Azure.Core;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Plant_Project.API.contracts.Plants;
using Plant_Project.API.Entities;

namespace Plant_Project.API.Services
{
    public class plantServices (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : IplantServices
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<List<PlantsResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result= await _context.plants.Include(p=>p.Category).ToListAsync(cancellationToken);

            var response = result.Select(p => new PlantsResponse(
                   p.Id,
                  p.Name,
                  p.Price,
                  p.Description,
                  p.How_To_Plant,
                  p.Quantity,
                  p.ImagePath,
                  p.Is_Avilable,
                  p.Category != null ? p.Category.Name : "Unknown" // تجنب null
                )).ToList();
            return Result.Success(response);
        }


        public async Task<Result> AddPlantAsync(PlantsRequest request, CancellationToken cancellationToken)
        {
            var CategoryIsExist = await _context.categories.FindAsync(request.CategoryId, cancellationToken);
            if (CategoryIsExist == null) {

                return Result.Failure(CategoryError.CategoryNotFound);
            }
            string imagePath = await SaveImageAsync(request.ImagePath);
            var absUri = $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{imagePath}";

            var plant = new Plant
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImagePath = absUri,
                CategoryId = request.CategoryId,
                How_To_Plant = request.How_To_Plant,
                Quantity = request.Quantity

            };
            await _context.plants.AddAsync(plant);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        
        public async Task<Result> UpdatePlantAsync(int Id,PlantsRequest request, CancellationToken cancellationToken = default)
        {
            var Plant = await _context.plants.Include(x=>x.Category).Where(x => x.Id == Id).FirstOrDefaultAsync(cancellationToken);

            if (Plant == null)
                return Result.Failure(PlantErrors.PlantNotFound);
            var CategoryIsExist = await _context.categories.FindAsync(request.CategoryId, cancellationToken);
            if (CategoryIsExist == null)
            {

                return Result.Failure(CategoryError.CategoryNotFound);
            }
            // تحديث الصورة إذا تم رفع صورة جديدة
            if (request.ImagePath != null)
            {
                // إذا كانت الصورة الجديدة مختلفة عن القديمة
                if (!Plant.ImagePath.Equals(request.ImagePath.FileName, StringComparison.OrdinalIgnoreCase))
                {
                    // مسح الصورة القديمة
                    DeleteImage(Plant.ImagePath);

                    // حفظ الصورة الجديدة
                    string newImagePath = await SaveImageAsync(request.ImagePath);
                    Plant.ImagePath = newImagePath;
                }
            }
            Plant.Name = request.Name;
            Plant.Description = request.Description;
            Plant.Price = request.Price;
            Plant.How_To_Plant = request.How_To_Plant;
            Plant.Quantity = request.Quantity;
            Plant.CategoryId = request.CategoryId;
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        public async Task<Result<PlantsResponse>> GetByIdAsync(int Id, CancellationToken cancellationToken = default)
        {
            var plant= await _context.plants.Where(x=>x.Id==Id).FirstOrDefaultAsync(cancellationToken);
            if(plant == null)
                return Result.Failure<PlantsResponse>(PlantErrors.PlantNotFound);
            
            var category= await _context.categories.Where(x=>x.Id==plant.CategoryId).FirstOrDefaultAsync(cancellationToken);
            
            
            var plantResponse = new PlantsResponse(
                   plant.Id,
                   plant.Name,
                    plant.Price,
                   plant.Description,
                     plant.How_To_Plant,
               plant.Quantity,
               plant.ImagePath,
               plant.Is_Avilable,
               category!.Name
       );

            return Result.Success(plantResponse);
        }
        public async Task<Result<PlantsResponse>> GetByNameAsync(string Name, CancellationToken cancellationToken = default)
        {
            var plant= await _context.plants.Where(x=>x.Name==Name).FirstOrDefaultAsync(cancellationToken);
            if (plant == null)
                return Result.Failure<PlantsResponse>(PlantErrors.PlantNotFound);

            var category = await _context.categories.Where(x => x.Id == plant.CategoryId).FirstOrDefaultAsync(cancellationToken);


            var plantResponse = new PlantsResponse(
                   plant.Id,
                   plant.Name,
                    plant.Price,
                   plant.Description,
                     plant.How_To_Plant,
               plant.Quantity,
               plant.ImagePath,
               plant.Is_Avilable,
               category!.Name
       );

            return Result.Success(plantResponse);
        }
        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var plant = await _context.plants.Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
            if (plant == null)
                return Result.Failure(PlantErrors.PlantNotFound);
             DeleteImage(plant.ImagePath);
             _context.plants.Remove(plant);
            await _context.SaveChangesAsync(cancellationToken);
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
        private void DeleteImage(string imagePath)
        {
             var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
               File.Delete(fullPath);
            }
        }
    }
}
