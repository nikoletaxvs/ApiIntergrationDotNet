using static API_Aggregation.Models.Food;
using static API_Aggregation.Models.Population;
using static API_Aggregation.Models.Cat;
using Microsoft.OpenApi.Services;
using static API_Aggregation.Models.Spotify.SpotifySearch;

namespace API_Aggregation.Models
{
    public class AggregatedData
    {
        public class AggregatedResult
        {
            public List<PopulationResult> Population { get; set; }
            public List<FoodResult> Food { get; set; }
            public List<CatBreedResult> Cat { get; set; }
            public SpotifyResult Spotify { get; set; }
        }
    }
}
