using SolarWatch5.Models;

namespace SolarWatch5.Services.Repository
{
    public interface ICityRepository
    {
        IEnumerable<City> GetAll();
        Task<City?> GetByNameAsync(string name);

        void Add(City city);
        
    }
}
