using SolarWatch5.Models;

namespace SolarWatch5.Services.Repository
{
    public interface ICityRepository
    {
        Task<City> GetCityAsync(string cityName, DateOnly day);
        Task<bool> CityExistsAsync(string cityName);
        Task AddCityAsync(City city);
    }
}
