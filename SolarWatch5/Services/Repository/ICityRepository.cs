using SolarWatch5.Models;

namespace SolarWatch5.Services.Repository
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetAllAsync();
        Task<City?> GetByNameAsync(string name);

        Task<City> AddAsync(City city);
        
    }
}
