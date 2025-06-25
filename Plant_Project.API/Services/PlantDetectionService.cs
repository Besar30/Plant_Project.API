using System.Net.Http.Headers;
using System.Text.Json;
using Plant_Project.API.Abstraction;
using Plant_Project.API.contracts.Ai;

namespace Plant_Project.API.Services
{
	public class PlantDetectionService : IPlantDetectionService
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<PlantDetectionService> _logger;
		private readonly PlantDetectionMapper _mapper;

		public PlantDetectionService(HttpClient httpClient, ILogger<PlantDetectionService> logger, PlantDetectionMapper mapper)
		{
			_httpClient = httpClient;
			_logger = logger;
			_mapper = mapper;
		}

		public async Task<Result<YourMappedResult>> DetectPlantAsync(IFormFile file, CancellationToken cancellationToken = default)
		{
			if (file == null || file.Length == 0)
				return Result.Failure<YourMappedResult>(new Error("NoFile", "No file uploaded."));

			using var content = new MultipartFormDataContent();
			using var fileStream = file.OpenReadStream();
			var streamContent = new StreamContent(fileStream);
			streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
			content.Add(streamContent, "image", file.FileName); // Corrected key name

			try
			{
				var response = await _httpClient.PostAsync("https://planet-disease-arabic.onrender.com/predict", content, cancellationToken);

				if (!response.IsSuccessStatusCode)
				{
					var errorDetails = await response.Content.ReadAsStringAsync(cancellationToken);
					_logger.LogError($"AI model error: {response.StatusCode} - {errorDetails}");
					return Result.Failure<YourMappedResult>(new Error("AIError", "Failed to analyze the image."));
				}

				var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
				var plantDetectionResponse = JsonSerializer.Deserialize<PlantDetectionResponse>(
					responseContent,
					new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

				if (plantDetectionResponse == null || string.IsNullOrWhiteSpace(plantDetectionResponse.Name))
				{
					return Result.Failure<YourMappedResult>(new Error("Empty", "No prediction found."));
				}

				var mappedResult = _mapper.Map(plantDetectionResponse);
				return Result.Success(mappedResult);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error detecting plant disease.");
				return Result.Failure<YourMappedResult>(new Error("Exception", "An error occurred while processing the image."));
			}
		}
	}
}
