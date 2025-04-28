namespace Plant_Project.API.contracts.Ai;


	public class PlantDetectionMapper
	{
		public YourMappedResult Map(PlantDetectionResponse response)
		{
			if (response == null || response.Prediction == null)
				throw new ArgumentNullException(nameof(response), "Plant detection response cannot be null or empty.");

			var prediction = response.Prediction;

			// Handle cases where the AI model detects something irrelevant (like background without leaves)
			if (string.IsNullOrEmpty(prediction.Name) || prediction.Name.Contains("Background_without_leaves"))
			{
				return new YourMappedResult
				{
					PlantName = "Unknown Plant",  // Default to "Unknown Plant" when no valid plant is detected
					HasDisease = false,           // Assume no disease if no valid plant detected
					Disease = "No plant detected in the image.",
					Solution = "Try using a clearer image with a visible plant."
				};
			}

			// Handle cases where the disease or plant name is not detected
			if (string.IsNullOrEmpty(prediction.Cause))
			{
				return new YourMappedResult
				{
					PlantName = prediction.Name ?? "Unknown Plant",
					HasDisease = false,
					Disease = "No disease detected.",
					Solution = "No solution available."
				};
			}

			return new YourMappedResult
			{
				PlantName = prediction.Name ?? "Unknown Plant",
				HasDisease = !string.IsNullOrEmpty(prediction.Cause),  // If there's no cause, we assume no disease
				Disease = prediction.Cause ?? "No disease detected.",
				Solution = prediction.Cure ?? "No solution available."
			};
		}
	}


