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
        private readonly ICityRepository _cityRepository;


        public CityController(ILogger<CityController> logger, ICityService cityService, ICityRepository cityRepository)
        {
            _logger = logger;
            _cityService = cityService;
            _cityRepository = cityRepository;
        }

        [HttpGet("GetAsync")]
        public async Task<ActionResult<City>> GetAsync(string cityName, [Required] DateOnly day)
        {
            try
            {
                var city = await _cityRepository.GetCityAsync(cityName, day);

                if (city == null)
                {
                    var newCity = await _cityService.GetCityAsync(cityName, day);
                    await _cityRepository.AddCityAsync(newCity);

                    if (newCity == null)
                    {
                        return NotFound($"City '{cityName}' not found in the database.");
                    }

                    return Ok(newCity);
                }

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
