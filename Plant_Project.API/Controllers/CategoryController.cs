using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plant_Project.API.Abstraction;
using Plant_Project.API.contracts.Categorys;
using Plant_Project.API.Services;

namespace Plant_Project.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController(ICategoryServices categoryServices) : ControllerBase
    {
        private readonly ICategoryServices _categoryServices = categoryServices;
        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await _categoryServices.GetAllCategoriesAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet("Id")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            var result = await _categoryServices.GetCategoryByIdAsync(Id, cancellationToken);

            return result.IsSuccess ?
                Ok(result.Value) : result.ToProblem(StatusCodes.Status404NotFound);
        }
        [HttpPost("")]
        public async Task<IActionResult> AddAsync(CategoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _categoryServices.AddCategoryAsync(request, cancellationToken);
            return result.IsSuccess ?
                Ok() : result.ToProblem(StatusCodes.Status400BadRequest);
        }
        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int categoryId, [FromBody] CategoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _categoryServices.UpdateCategoryAsync(categoryId, request, cancellationToken);
            return result.IsSuccess ?
                Ok() : result.ToProblem(StatusCodes.Status400BadRequest);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int Id,CancellationToken cancellationToken)
        {
            var result= await _categoryServices.DeleteCategoryAsync(Id,cancellationToken);
            return result.IsSuccess ?
                Ok() : result.ToProblem(StatusCodes.Status404NotFound);
        }
    }
}
