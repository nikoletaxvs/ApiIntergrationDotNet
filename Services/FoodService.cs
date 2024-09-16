using API_Aggregation.Controllers;
using static API_Aggregation.Models.Food;


namespace API_Aggregation.Services
{
    public class FoodService : IFoodService
    {
    
        private readonly HttpClient _httpClient;
        private readonly ILogger<FoodService> _logger;
        private const string FoodApiUrl = "https://world.openfoodfacts.org/api/v0/search?search_terms=food&fields=code,name,ingredients_text,nutrition_grades_tags,image_url&sort_by=popularity&order_by=desc";

        public FoodService(HttpClient httpClient, ILogger<FoodService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<FoodResult>> GetFoodDataAsync()
        {
            try
            {
                // Fetch food data from API
                var foodResponse = await _httpClient.GetAsync(FoodApiUrl);

                if (!foodResponse.IsSuccessStatusCode)
                {
                    // Log detailed error information
                    _logger.LogError($"Failed to fetch food data from {FoodApiUrl}: StatusCode {foodResponse.StatusCode}");
                    // Optionally, return fallback data
                    return GetFallbackFoodResults();
                }

                var foodContent = await foodResponse.Content.ReadAsStringAsync();
                _logger.LogInformation($"Food data response: {foodContent}");

                var foodData = await foodResponse.Content.ReadFromJsonAsync<FoodApiResponse>();

                var limitedFoodResults = foodData?.Products?.Take(5).Select(p => new FoodResult
                {
                    Code = p.Code,
                    Image_Url = p.Image_Url,
                    ingredients_text = p.ingredients_text,
                    nutrition_grades_tags = p.nutrition_grades_tags,
                }).ToList() ?? new List<FoodResult>();

                return limitedFoodResults;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error fetching food data from {FoodApiUrl}: {ex.Message}");
                // Optionally, return fallback data
                return GetFallbackFoodResults();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error fetching food data from {FoodApiUrl}: {ex.Message}");
                // Optionally, return fallback data
                return GetFallbackFoodResults();
            }
        }

        private List<FoodResult> GetFallbackFoodResults()
        {
            // Provide a set of default data or an empty list
            return new List<FoodResult>
            {
                new FoodResult
                {
                    Code = "Fallback",
                    Image_Url = "https://via.placeholder.com/150",
                    ingredients_text = "No data available",
                    nutrition_grades_tags = new List<string> { "unknown" }
                }
            };
        }
    }
}
