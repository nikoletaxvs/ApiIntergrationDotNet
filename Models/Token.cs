namespace API_Aggregation.Models
{
    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public DateTime IssuedAt { get; set; } // Added to track when the token was issued
    }

}
