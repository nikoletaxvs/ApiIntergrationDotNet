using API_Aggregation.Controllers;
using static API_Aggregation.Models.Population;
namespace API_Aggregation.Services
{
    public class PopulationService : IPopulationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PopulationService> _logger;
        private const string PopulationApiUrl = "https://datausa.io/api/data?drilldowns=Nation&measures=Population";

        public PopulationService(HttpClient httpClient, ILogger<PopulationService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<PopulationResult>> GetPopulationDataAsync()
        {
            try
            {
                // Fetch population data from API
                var populationResponse = await _httpClient.GetAsync(PopulationApiUrl);
                if (!populationResponse.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to fetch population data: {populationResponse.StatusCode}");
                    throw new HttpRequestException($"Failed to fetch population data. StatusCode: {populationResponse.StatusCode}");
                }

                var populationContent = await populationResponse.Content.ReadAsStringAsync();
                _logger.LogInformation($"Population data response: {populationContent}");

                var populationData = await populationResponse.Content.ReadFromJsonAsync<PopulationApiResponse>();
                var limitedPopulationResults = populationData?.Data?.Take(5).Select(p => new PopulationResult
                {
                    Nation = p.Nation,
                    Population = p.Population,
                    Year = p.Year
                }).ToList() ?? new List<PopulationResult>();

                return limitedPopulationResults;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error fetching population data: {ex.Message}");
                throw; // You may throw the error or return a default value based on your use case
            }
        }
    }

}
