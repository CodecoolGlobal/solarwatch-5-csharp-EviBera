using SolarWatch5.Models;

namespace SolarWatch5.Services.Repository
{
    public interface ICityRepository
    {
        IEnumerable<City> GetAll();
        City? GetByName(string name);

        void Add(City city);
        
    }
}
