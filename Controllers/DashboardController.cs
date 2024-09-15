using Microsoft.AspNetCore.Mvc;
using static API_Aggregation.Models.AggregatedData;
using API_Aggregation.Services;


namespace API_Aggregation.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IPopulationService _populationService;
        private readonly IFoodService _foodService;
        private readonly ICatService _catService;
        private readonly ISpotifyService _spotifyService;
        private readonly ILogger<DashboardController> _logger;
        public DashboardController(IPopulationService populationService, IFoodService foodService, ICatService catService,ISpotifyService spotifyService, ILogger<DashboardController> logger)
        {
            _populationService = populationService;
            _foodService = foodService;
            _catService = catService;
            _spotifyService = spotifyService;
            _logger = logger;
        }

        [HttpGet("data")]
        public async Task<IActionResult> GetAggregatedData()
        {
            try
            {
                // Fetch population data using the service
                var populationResults = await _populationService.GetPopulationDataAsync();

                // Fetch food data using the service
                var foodResults = await _foodService.GetFoodDataAsync();

                //Fetch cat breed data using the service
                var catResults = await _catService.GetCatBreedsAsync();

                //Fetch spotify data
                var spotifyResults = await _spotifyService.GetSpotifyList();

                // Create the aggregated result
                var aggregatedResult = new AggregatedResult
                {
                    Population = populationResults,
                    Food = foodResults,
                    Cat = catResults,
                    Spotify = spotifyResults
                };

                return Ok(aggregatedResult);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error: {ex.Message}");
                return StatusCode(500, $"Request error: {ex.Message}");
            }
        }

    }
}
