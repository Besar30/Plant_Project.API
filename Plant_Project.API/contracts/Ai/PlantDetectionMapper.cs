namespace Plant_Project.API.contracts.Ai
{
	public class PlantDetectionMapper
	{
		public YourMappedResult Map(PlantDetectionResponse response)
		{
			if (response == null)
				throw new ArgumentNullException(nameof(response), "Plant detection response cannot be null.");

			// If no name returned from AI
			if (string.IsNullOrWhiteSpace(response.Name))
			{
				return new YourMappedResult
				{
					PlantName = "Unknown Plant",
					HasDisease = false,
					Disease = "No plant detected in the image.",
					Solution = "Try using a clearer image with a visible plant.",
					Accuracy = response.Accuracy ?? 0f
				};
			}

			// If no cause => No disease
			if (string.IsNullOrWhiteSpace(response.Cause))
			{
				return new YourMappedResult
				{
					PlantName = response.Name,
					HasDisease = false,
					Disease = "No disease detected.",
					Solution = "No solution available.",
					Accuracy = response.Accuracy ?? 0f
				};
			}

			// Disease detected
			return new YourMappedResult
			{
				PlantName = response.Name,
				HasDisease = true,
				Disease = response.Cause,
				Solution = response.Cure ?? "No treatment available.",
				Accuracy = response.Accuracy ?? 0f
			};
		}
	}
}
