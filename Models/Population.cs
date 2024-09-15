namespace API_Aggregation.Models
{
    public class Population
    {
        public class PopulationData
        {
            public string Nation { get; set; }
            public int Population { get; set; }
            public string Year { get; set; }
        }

        public class PopulationResult
        {
            public string Nation { get; set; }
            public int Population { get; set; }
            public string Year { get; set; }
        }
        public class PopulationApiResponse
        {
            public List<PopulationData> Data { get; set; }
        }

    }
}
