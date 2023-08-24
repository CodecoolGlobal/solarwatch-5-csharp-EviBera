﻿using Microsoft.AspNetCore.Mvc;
using SolarWatch5.Models;
using SolarWatch5.Services;
using System.ComponentModel.DataAnnotations;

namespace SolarWatch5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ILogger<CityController> _logger;
        private readonly ICityService _cityService;


        public CityController(ILogger<CityController> logger, ICityService cityService)
        {
            _logger = logger;
            _cityService = cityService;
        }

        [HttpGet("GetAsync")]
        public async Task<ActionResult<City>> GetAsync(string cityName, [Required] DateOnly day)
        {
            try
            {
                var city = await _cityService.GetCityAsync(cityName, day);
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
