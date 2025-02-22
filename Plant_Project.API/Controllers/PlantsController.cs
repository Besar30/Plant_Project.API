namespace Plant_Project.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public  class PlantsController(IPlantServices plantServices) : ControllerBase
    {
		private readonly IPlantServices _plantServices = plantServices;

		[HttpGet("")]
		[HasPermission(Permissions.GetPlant)]
		public async Task<IActionResult> GetAll(CancellationToken cancellationToken) 
        {
			return Ok(await _plantServices.GetAllAsync(cancellationToken));
		}

		[HttpGet("{id}")]
		[HasPermission(Permissions.GetPlant)]
		public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
		{
			var result = await _plantServices.GetAsync(id, cancellationToken);

			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}

		[HttpPost("")]
		[HasPermission(Permissions.AddPlant)]
		public async Task<IActionResult> Add([FromBody] PlantsRequest request,
		CancellationToken cancellationToken)
		{
			var result = await _plantServices.AddAsync(request, cancellationToken);

			return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value) : result.ToProblem();
		}
		[HttpPut("{id}")]
		[HasPermission(Permissions.UpdatePlant)]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PlantsRequest request,
		CancellationToken cancellationToken)
		{
			var result = await _plantServices.UpdateAsync(id, request, cancellationToken);

			return result.IsSuccess ? NoContent() : result.ToProblem();
		}
		[HttpPut("{id}/is_available")]
		[HasPermission(Permissions.UpdatePlant)]
		public async Task<IActionResult> ToggleAvailable([FromRoute] int id, CancellationToken cancellationToken)
		{
			var result = await _plantServices.IsAvailableAsync(id, cancellationToken);

			return result.IsSuccess ? NoContent() : result.ToProblem();
		}
	}
}
