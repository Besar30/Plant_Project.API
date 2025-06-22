using System.Net.Http.Headers;
using System.Text.Json;
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
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<Result<YourMappedResult>> DetectPlantAsync(IFormFile file, CancellationToken cancellationToken = default)
		{
			if (file == null || file.Length == 0)
				return Result.Failure<YourMappedResult>(new Error("NO_FILE", "No file uploaded."));

			using var content = new MultipartFormDataContent();
			using var fileStream = file.OpenReadStream();
			var streamContent = new StreamContent(fileStream);
			streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
			content.Add(streamContent, "img", file.FileName);

			try
			{
				var response = await _httpClient.PostAsync("https://planet-disease-arabic.onrender.com/api/predict", content, cancellationToken);

				var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
				_logger.LogInformation("AI Raw Response: {RawContent}", responseContent);

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogError("AI model error: {StatusCode} - {Error}", response.StatusCode, responseContent);
					return Result.Failure<YourMappedResult>(new Error("AI_MODEL_ERROR", "AI service failed to process the image."));
				}

				var plantDetectionResponse = JsonSerializer.Deserialize<PlantDetectionResponse>(
					responseContent,
					new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

				if (plantDetectionResponse == null || plantDetectionResponse.Prediction == null)
				{
					_logger.LogWarning("AI returned no prediction.");
					var fallbackResult = new YourMappedResult
					{
						PlantName = "Unknown Plant",
						HasDisease = false,
						Disease = "No plant detected.",
						Solution = "Please upload a clearer image.",
						Accuracy = 0
					};
					return Result.Success(fallbackResult);
				}

				var mappedResult = _mapper.Map(plantDetectionResponse);
				return Result.Success(mappedResult);
			}
			catch (JsonException jsonEx)
			{
				_logger.LogError(jsonEx, "JSON deserialization error.");
				return Result.Failure<YourMappedResult>(new Error("INVALID_AI_RESPONSE", "Received unexpected response from AI."));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "General error detecting plant disease.");
				return Result.Failure<YourMappedResult>(new Error("SERVER_ERROR", "An error occurred while processing the image."));
			}
		}
	}
}
