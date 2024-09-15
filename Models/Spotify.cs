namespace API_Aggregation.Models
{
    public class Spotify
    {
        public class SpotifyArtist
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Image { get; set; }
            public string Followers { get; set; }
            public string Popularity { get; set; }
        }
        public class SpotifySearch
        {
            public class ExternalUrls
            {
                public string spotify { get; set; }
            }

            public class Followers
            {
                public int total { get; set; }
            }

            public class ImageSP
            {
                public string url { get; set; }
            }

            public class Item
            {
                public Followers followers { get; set; }
                public string id { get; set; }
                public List<ImageSP> images { get; set; }
                public string name { get; set; }
                public int popularity { get; set; }
            }

            public class Artists
            {
                public List<Item> items { get; set; }
            }

            public class SpotifyResult
            {
                public Artists artists { get; set; }
            }
            public class SearchRequest
            {
                public string SearchWord { get; set; }
                public Token Token { get; set; }
            }
        }
    }
}
