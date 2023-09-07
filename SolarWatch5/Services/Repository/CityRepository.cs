using Microsoft.EntityFrameworkCore;
using SolarWatch5.Data;
using SolarWatch5.Models;

namespace SolarWatch5.Services.Repository
{
    public class CityRepository : ICityRepository
    {
        
        public void Add(City city)
        {
            using var dbContext = new SolarWatchContext();
            dbContext.Add<City>(city);
            dbContext.SaveChanges();
        }

        public IEnumerable<City> GetAll()
        {
            using var dbContext = new SolarWatchContext();
            return dbContext.Cities.ToList();
        }

        public City? GetByName(string name)
        {
            using var dbContext = new SolarWatchContext();
            return dbContext.Cities.FirstOrDefault(c => c.CityName  == name);
        }

    }
}
