using API_Aggregation.Models;
using Microsoft.OpenApi.Services;
using Newtonsoft.Json;
using RestSharp;
using static API_Aggregation.Models.Spotify.SpotifySearch;
using System.Text;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace API_Aggregation.Services
{
    public class SpotifyService : ISpotifyService
    {
        private static OAuthSettings _oauthSettings;
        private readonly HttpClient _httpClient;
        private static Token _cachedToken;
        private readonly ILogger<FoodService> _logger;
        private const string TokenUrl = "https://accounts.spotify.com/api/token";
        private const string SearchUrl = "https://api.spotify.com/v1/search";
        private static readonly TimeSpan TokenExpiryBuffer = TimeSpan.FromMinutes(1); // Buffer time to avoid using an expired token

        public SpotifyService(IOptions<OAuthSettings> oauthSettings, HttpClient httpClient, ILogger<FoodService> logger)
        {
            _oauthSettings = oauthSettings.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<SpotifyResult> GetSpotifyList()
        {
            try
            {
                // Ensure we have a valid token
                var token = await GetTokenAsync();

                // Search for artists or songs using the token
                var result = await SearchArtistOrSongAsync("Taylor", token);
                return result ?? GetFallbackSpotifyResult(); // Return fallback result if search fails
            }
            catch (Exception ex)
            {
                // Log the error and return a fallback result
                _logger.LogError($"Error fetching Spotify list: {ex.Message}");
                return GetFallbackSpotifyResult(); // Return a fallback result in case of error
            }
        }

        private async Task<Token> GetTokenAsync()
        {
            // Use cached token if available and not expired
            if (_cachedToken != null && !IsTokenExpired(_cachedToken))
            {
                return _cachedToken;
            }

            try
            {
                string clientID = _oauthSettings.ClientId;
                string clientSecret = _oauthSettings.ClientSecret;

                string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientID}:{clientSecret}"));

                var request = new HttpRequestMessage(HttpMethod.Post, TokenUrl)
                {
                    Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                })
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode(); // Throws if the response status code is not success

                var responseBody = await response.Content.ReadAsStringAsync();
                _cachedToken = JsonConvert.DeserializeObject<Token>(responseBody);

                // Set the token's expiration time
                _cachedToken.IssuedAt = DateTime.UtcNow;
                return _cachedToken;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error fetching Spotify token: {ex.Message}");
                throw; // Rethrow or handle based on your application's needs
            }
        }

        private async Task<SpotifyResult> SearchArtistOrSongAsync(string searchWord, Token token)
        {
            try
            {
                var request = new RestRequest($"{SearchUrl}?q={searchWord}&type=artist", Method.Get);
                request.AddHeader("Authorization", $"Bearer {token.access_token}");

                var client = new RestClient();
                var response = await client.ExecuteAsync<SpotifyResult>(request);

                if (response.IsSuccessful)
                {
                    return JsonConvert.DeserializeObject<SpotifyResult>(response.Content);
                }
                else
                {
                    _logger.LogWarning($"Spotify search failed: StatusCode {response.StatusCode}");
                    return GetFallbackSpotifyResult(); // Return fallback result if search fails
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error searching Spotify: {ex.Message}");
                return GetFallbackSpotifyResult(); // Return fallback result in case of error
            }
        }

        private bool IsTokenExpired(Token token)
        {
            if (token.IssuedAt == default)
            {
                return true; // If we don't have an issued time, consider it expired
            }

            var expirationTime = token.IssuedAt.AddSeconds(token.expires_in);
            return DateTime.UtcNow >= expirationTime.Subtract(TokenExpiryBuffer);
        }

        private SpotifyResult GetFallbackSpotifyResult()
        {
            // Return a fallback result if Spotify data cannot be fetched
            return new SpotifyResult
            {
                artists = new Artists
                {
                    items = new List<Item>
                {
                    new Item
                    {
                        name = "Fallback Artist",
                        popularity = 0,
                        images = new List<ImageSP> { new ImageSP { url = "https://via.placeholder.com/150" } },
                        id = "fallback",
                        followers = new Followers { total = 0 }
                    }
                }
                }
            };
        }
    }



}
