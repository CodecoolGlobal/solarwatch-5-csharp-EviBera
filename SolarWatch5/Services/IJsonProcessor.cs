using SolarWatch5.Models;

namespace SolarWatch5.Services
{
    public interface IJsonProcessor
    {
        City ProcessJsonCityData(string cityData);
        SunsetSunriseData ProcessJsonSolarData(string solarData, DateOnly when);
    }
}
