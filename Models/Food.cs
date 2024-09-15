namespace API_Aggregation.Models
{
    public class Food
    {
        public class FoodResult
        {
            public string Code { get; set; }
            public string Image_Url { get; set; }
            public string ingredients_text { get; set; }
            public List<string> nutrition_grades_tags { get; set; }
        }

        public class FoodApiResponse
        {
            public List<Product> Products { get; set; }
        }

        public class Product
        {
            public string Code { get; set; }
            public string Image_Url { get; set; }
            public string ingredients_text { get; set; }
            public List<string> nutrition_grades_tags { get; set; }

        }
    }
}
