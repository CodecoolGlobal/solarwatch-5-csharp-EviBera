using Microsoft.EntityFrameworkCore;
using SolarWatch5.Data;
using SolarWatch5.Models;

namespace SolarWatch5.Services.Repository
{
    public class CityRepository : ICityRepository
    {

        private readonly SolarWatchContext _dbContext;

        public CityRepository(SolarWatchContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(City city)
        {
            _dbContext.Add<City>(city);
            _dbContext.SaveChanges();
        }

        public IEnumerable<City> GetAll()
        {
            return _dbContext.Cities.ToList();
        }

        public async Task<City?> GetByNameAsync(string name)
        {
            return await _dbContext.Cities.FirstOrDefaultAsync(c => c.CityName  == name);
        }

    }
}
