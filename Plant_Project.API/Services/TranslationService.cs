//namespace Plant_Project.API.Services;

//public class TranslationService(HttpClient httpClient)
//{
//	private readonly HttpClient _httpClient = httpClient;

//	public async Task<string> TranslateToArabicAsync(string text)
//	{
//		var requestData = new
//		{
//			q = text,
//			source = "en",
//			target = "ar",
//			format = "text"
//		};

//		var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

//		var response = await _httpClient.PostAsync("https://libretranslate.de/translate", content);

//		if (!response.IsSuccessStatusCode)
//			throw new Exception("Translation failed.");

//		var json = await response.Content.ReadAsStringAsync();
//		using var doc = JsonDocument.Parse(json);
//		return doc.RootElement.GetProperty("translatedText").GetString()!;
//	}
//}

