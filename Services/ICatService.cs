using static API_Aggregation.Models.Cat;

namespace API_Aggregation.Services
{
    public interface ICatService
    {
        Task<List<CatBreedResult>> GetCatBreedsAsync();
    }
}
