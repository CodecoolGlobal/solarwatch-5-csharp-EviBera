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
        private readonly ISolarDataRepository _sunsetSunriseDataRepository;


        public CityController(ILogger<CityController> logger, ICityService cityService, IJsonProcessor jsonProcessor, 
            ICityRepository cityRepository, ISolarDataRepository sunsetSunriseDataRepository)
        {
            _logger = logger;
            _cityService = cityService;
            _jsonProcessor = jsonProcessor;
            _cityRepository = cityRepository;
            _sunsetSunriseDataRepository = sunsetSunriseDataRepository;
        }

        [HttpGet("GetAsync")]
        public async Task<ActionResult<CityWithSolarData>> GetAsync(string cityName, [Required] DateOnly day)
        {
            try
            {
                var city = await _cityRepository.GetByNameAsync(cityName);

                if (city == null)
                {
                    var newCityData = _cityService.GetCoordinatesAsync(cityName);
                    city = _jsonProcessor.ProcessJsonCityData(await newCityData);

                    if (city == null)
                    {
                        return NotFound($"City {cityName} not found");
                    }

                    _cityRepository.AddAsync(city);

                }

                var solarData = await _sunsetSunriseDataRepository.GetByDateAndCityAsync(day, city.Id);

                if (solarData == null)
                {
                    var solarDataString = await _cityService.GetSolarDataAsync(day, city);

                    if (solarDataString == null)
                    {
                        // Handle the case where solarDataString is null, possibly an error in the service.
                        return NotFound($"Solar data not available for {cityName} on {day}");
                    }

                    solarData = _jsonProcessor.ProcessJsonSolarData(solarDataString, day);
                    solarData.CityId = city.Id;
                    _sunsetSunriseDataRepository.AddSolarDataAsync(solarData);
                }


                if (city.SunsetSunriseDataList == null)
                {
                    city.SunsetSunriseDataList = new List<SunsetSunriseData>(); 
                }

                city.SunsetSunriseDataList.Add(solarData);

                var combinedData = new CityWithSolarData()
                {
                    City = city,
                    SolarData = solarData
                };

                return Ok(combinedData);

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting solar data, {ex.Message}");
                return NotFound($"Error getting solar data, {ex.Message}");
            }
        }

    }
}
