using Microsoft.OpenApi.Services;
using static API_Aggregation.Models.Spotify.SpotifySearch;

namespace API_Aggregation.Services
{
    public interface ISpotifyService
    {
        Task<SpotifyResult> GetSpotifyList();
    }
}
