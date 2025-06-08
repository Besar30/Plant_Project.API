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

		// Endpoint to Detect Plant using an image file
		public async Task<YourMappedResult> DetectPlantAsync(IFormFile file, CancellationToken cancellationToken = default)
		{
			if (file == null || file.Length == 0)
				throw new ArgumentException("No file uploaded.", nameof(file));

			using var content = new MultipartFormDataContent();
			using var fileStream = file.OpenReadStream();
			var streamContent = new StreamContent(fileStream);

			streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

			content.Add(streamContent, "img", file.FileName);

			try
			{
				var response = await _httpClient.PostAsync("https://plant-disease-v2.onrender.com/api/predict", content, cancellationToken);

				if (!response.IsSuccessStatusCode)
				{
					var errorDetails = await response.Content.ReadAsStringAsync(cancellationToken);
					_logger.LogError($"Error from AI model: {response.StatusCode} - {errorDetails}");
					throw new Exception($"AI model error: {response.StatusCode} - {errorDetails}");
				}

				var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
				var plantDetectionResponse = await JsonSerializer.DeserializeAsync<PlantDetectionResponse>(
					responseStream,
					new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
					cancellationToken);

				if (plantDetectionResponse?.Prediction == null || string.IsNullOrEmpty(plantDetectionResponse.Prediction.Name))
				{
					throw new Exception("Unable to detect the plant or disease. Please try another image.");
				}

				var result = _mapper.Map(plantDetectionResponse);

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error detecting plant disease.");
				throw new Exception("Error processing the image. Please ensure the image is clear and try again.", ex);
			}
		}
	}
}
