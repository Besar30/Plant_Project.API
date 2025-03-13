using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plant_Project.API.Abstraction;
using Plant_Project.API.contracts.Categorys;
using Plant_Project.API.Services;

namespace Plant_Project.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
   // [Authorize]
    public class CategoryController(ICategoryServices categoryServices) : ControllerBase
    {
        private readonly ICategoryServices _categoryServices = categoryServices;
        //https://localhost:7286/Category
        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await _categoryServices.GetAllCategoriesAsync(cancellationToken);
            return Ok(result);
        }
        //https://localhost:7286/Category/Id
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            var result = await _categoryServices.GetCategoryByIdAsync(Id, cancellationToken);

            return result.IsSuccess ?
                Ok(result.Value) : result.ToProblem(StatusCodes.Status404NotFound);
        }
        [Authorize(Roles = DefaultRoles.Admin)]
        [HttpPost("")]
        public async Task<IActionResult> AddAsync([FromForm]CategoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _categoryServices.AddCategoryAsync(request, cancellationToken);
            return result.IsSuccess ?
                Ok() : result.ToProblem(StatusCodes.Status400BadRequest);
        }
        //https://localhost:7286/Category
        [Authorize(Roles = DefaultRoles.Admin)]
        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int categoryId, [FromForm] CategoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _categoryServices.UpdateCategoryAsync(categoryId, request, cancellationToken);
            return result.IsSuccess ?
                Ok() : result.ToProblem(StatusCodes.Status400BadRequest);
        }
        //https://localhost:7286/Category/3
        [Authorize(Roles = DefaultRoles.Admin)]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int Id,CancellationToken cancellationToken)
        {
            var result= await _categoryServices.DeleteCategoryAsync(Id,cancellationToken);
            return result.IsSuccess ?
                Ok() : result.ToProblem(StatusCodes.Status404NotFound);
        }
        [HttpGet("{Name}/plantByCategoryName")]
        public async Task<IActionResult> GetPlantsByCategoryName([FromRoute] string Name, CancellationToken cancellationToken)
        {
            var result = await _categoryServices.GetAllPlantByCategoryName(Name, cancellationToken);

            return result.IsSuccess ?
                Ok(result) : result.ToProblem(StatusCodes.Status404NotFound);
        }
    }
}
