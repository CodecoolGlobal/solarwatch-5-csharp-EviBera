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

        public async Task AddCityAsync(City city)
        {
            _dbContext.Cities.Add(city);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> CityExistsAsync(string cityName)
        {
            return await _dbContext.Cities
            .AnyAsync(c => c.CityName == cityName);
        }

        public async Task<City> GetCityAsync(string cityName, DateOnly day)
        {
            // Check if the city exists in the database
            var city = await _dbContext.Cities
                .Include(c => c.SunsetSunriseDataList)
                .Where(c => c.CityName == cityName)
                .FirstOrDefaultAsync();

            return city;
        }
    }
}
