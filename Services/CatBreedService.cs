using static API_Aggregation.Models.Cat;

namespace API_Aggregation.Services
{
    public class CatBreedService : ICatService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CatBreedService> _logger;
        private const string CatApiUrl = "https://api.thecatapi.com/v1/breeds";

        public CatBreedService(HttpClient httpClient, ILogger<CatBreedService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<CatBreedResult>> GetCatBreedsAsync()
        {
            try
            {
                // Fetch cat breeds data from The Cat API
                var catBreedResponse = await _httpClient.GetAsync(CatApiUrl);
                if (!catBreedResponse.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to fetch cat breed data: {catBreedResponse.StatusCode}");
                    throw new HttpRequestException($"Failed to fetch cat breed data. StatusCode: {catBreedResponse.StatusCode}");
                }

                var catBreedContent = await catBreedResponse.Content.ReadAsStringAsync();
                _logger.LogInformation($"Cat breed data response: {catBreedContent}");

                var catBreedData = await catBreedResponse.Content.ReadFromJsonAsync<List<CatBreedApiResponse>>();

                // Map the API response to a simpler model
                var limitedCatBreedResults = catBreedData?.Take(5).Select(c => new CatBreedResult
                {
                    Name = c.Name,
                    Origin = c.Origin,
                    Temperament = c.Temperament,
                    LifeSpan = c.Life_Span,
                    Description = c.Description
                }).ToList() ?? new List<CatBreedResult>();

                return limitedCatBreedResults;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error fetching cat breed data: {ex.Message}");
                throw; // You can decide to return default data or rethrow the exception depending on your use case
            }
        }
    }
}
