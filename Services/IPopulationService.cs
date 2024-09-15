using API_Aggregation.Controllers;
using static API_Aggregation.Models.Population;
namespace API_Aggregation.Services
{
    public interface IPopulationService
    {
        Task<List<PopulationResult>> GetPopulationDataAsync();
    }
}
