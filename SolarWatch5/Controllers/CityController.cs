using Microsoft.AspNetCore.Mvc;
using SolarWatch5.Models;
using SolarWatch5.Services;
using SolarWatch5.Services.Repository;
using System.ComponentModel.DataAnnotations;

namespace SolarWatch5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ILogger<CityController> _logger;
        private readonly ICityService _cityService;
        private readonly IJsonProcessor _jsonProcessor;
        private readonly ICityRepository _cityRepository;


        public CityController(ILogger<CityController> logger, ICityService cityService, IJsonProcessor jsonProcessor, 
            ICityRepository cityRepository)
        {
            _logger = logger;
            _cityService = cityService;
            _jsonProcessor = jsonProcessor;
            _cityRepository = cityRepository;
        }

        [HttpGet("GetAsync")]
        public async Task<ActionResult<City>> GetAsync(string cityName, [Required] DateOnly day)
        {
            var city = _cityRepository.GetByName(cityName);

            if (city == null)
            {
                var newCityData = _cityService.GetCoordinatesAsync(cityName);
                city = _jsonProcessor.ProcessJsonCityData(await newCityData);

                if (city == null)
                {
                    return NotFound($"City {cityName} not found");
                }

                _cityRepository.Add(city);

            }

            try
            {

                var solarDataString = await _cityService.GetSolarDataAsync(day, city);

                if (solarDataString == null)
                {
                    // Handle the case where solarDataString is null, possibly an error in the service.
                    return NotFound($"Solar data not available for {cityName} on {day}");
                }

                var solarData = _jsonProcessor.ProcessJsonSolarData(solarDataString, day);

                if (city.SunsetSunriseDataList == null)
                {
                    city.SunsetSunriseDataList = new List<SunsetSunriseData>(); 
                }

                city.SunsetSunriseDataList.Add(solarData);

                return Ok(city);

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting solar data, {ex.Message}");
                return NotFound($"Error getting solar data, {ex.Message}");
            }
        }

    }
}
