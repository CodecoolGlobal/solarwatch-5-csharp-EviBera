using SolarWatch5.Models;

namespace SolarWatch5.Services.Repository
{
    public interface ISolarDataRepository
    {
        Task<IEnumerable<SunsetSunriseData>> GetAllByCityIdAsync(int cityId);
        Task<SunsetSunriseData> AddSolarDataAsync(SunsetSunriseData data);
        Task<SunsetSunriseData> GetByDateAndCityAsync(DateOnly date, int cityId);
    }
}
