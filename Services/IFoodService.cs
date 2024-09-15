using API_Aggregation.Controllers;
using static API_Aggregation.Models.Food;

namespace API_Aggregation.Services
{
    public interface IFoodService
    {
        Task<List<FoodResult>> GetFoodDataAsync();
    }
}
