namespace API_Aggregation.Models
{
    public class Cat
    {
        public class CatBreedApiResponse
        {
            public string Name { get; set; }
            public string Origin { get; set; }
            public string Temperament { get; set; }
            public string Life_Span { get; set; }
            public string Description { get; set; }
        }

        public class CatBreedResult
        {
            public string Name { get; set; }
            public string Origin { get; set; }
            public string Temperament { get; set; }
            public string LifeSpan { get; set; }
            public string Description { get; set; }
        }
    }
}
