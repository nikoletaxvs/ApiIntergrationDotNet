using API_Aggregation.Models;
using Microsoft.OpenApi.Services;
using Newtonsoft.Json;
using RestSharp;
using static API_Aggregation.Models.Spotify.SpotifySearch;
using System.Text;
using Microsoft.Extensions.Options;

namespace API_Aggregation.Services
{
    public class SpotifyService :ISpotifyService
    {
        private static OAuthSettings _oauthSettings;
        public SpotifyService(IOptions<OAuthSettings> oauthSettings)
        {
            _oauthSettings = oauthSettings.Value;
        }
        public async Task<SpotifyResult> GetSpotifyList()
        {
            Token t = await SearchHelper.GetTokenAsync();
            // Pass the searchWord and token to your method
            SpotifyResult result = SearchHelper.SearchArtistOrSong("Taylor", t);
            return result;
        }

        public static class SearchHelper
        {
            public static Token token { get; set; }

            public static async Task<Token> GetTokenAsync()
            {
               

                string clientID = _oauthSettings.ClientId;
                string clientSecret = _oauthSettings.ClientSecret;

                string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(clientID + ":" + clientSecret));

                List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>
                 {
                     new KeyValuePair<string, string>("grant_type", "client_credentials")
                 };

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {auth}");
                HttpContent content = new FormUrlEncodedContent(args);

                HttpResponseMessage resp = await client.PostAsync("https://accounts.spotify.com/api/token", content);
                string msg = await resp.Content.ReadAsStringAsync();

                token = JsonConvert.DeserializeObject<Token>(msg);
                return token;
            }

            public static SpotifyResult SearchArtistOrSong(string searchWord, Token token)
            {
                var client = new RestClient("https://api.spotify.com/v1/search");
                client.AddDefaultHeader("Authorization", $"Bearer {token.access_token}");
                var request = new RestRequest($"?q={searchWord}&type=artist", Method.Get);
                var response = client.Execute<SpotifyResult>(request);


                if (response.IsSuccessful)
                {
                    var result = JsonConvert.DeserializeObject<SpotifyResult>(response.Content);
                    return result;
                }
                else
                {
                    return null;
                }

            }


        }
    }
}
