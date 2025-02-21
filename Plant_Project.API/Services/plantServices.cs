using Mapster;
using Plant_Project.API.contracts.Plants;

namespace Plant_Project.API.Services
{
    public class plantServices (ApplicationDbContext context): IplantServices
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<Result<List<PlantsResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result= await _context.plants.Include(p=>p.Category).ToListAsync(cancellationToken);

            var response = result.Select(p => new PlantsResponse(
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
            var plant = new Plant
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImagePath = imagePath,
                CategoryId = request.CategoryId,
                How_To_Plant = request.How_To_Plant,
                Quantity = request.Quantity

            };
            await _context.plants.AddAsync(plant);
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
    }
}
