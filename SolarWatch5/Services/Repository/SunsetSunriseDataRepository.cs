﻿using Microsoft.EntityFrameworkCore;
using SolarWatch5.Data;
using SolarWatch5.Models;
using System.Reflection.Metadata.Ecma335;

namespace SolarWatch5.Services.Repository
{
    public class SunsetSunriseDataRepository : ISolarDataRepository
    {
        private readonly SolarWatchContext _dbContext;

        public SunsetSunriseDataRepository(SolarWatchContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SunsetSunriseData> AddSolarDataAsync(SunsetSunriseData data)
        {
            await _dbContext.SunsetSunriseDataCollection.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return data;
        }

        public async Task<IEnumerable<SunsetSunriseData>> GetAllByCityIdAsync(int cityId)
        {
            var solarDataByCityId = await _dbContext
                .SunsetSunriseDataCollection.Where(SunsetSunriseData => SunsetSunriseData.CityId == cityId).ToListAsync();
            return solarDataByCityId;
        }

        public async Task<SunsetSunriseData> GetByDateAndCityAsync(DateOnly date, int cityId)
        {
            var solarData = await _dbContext.SunsetSunriseDataCollection
                .FirstOrDefaultAsync(ssd => ssd.CityId == cityId && ssd.Date == date.ToDateTime(TimeOnly.Parse("10:00 PM")));
            return solarData;
        }
    }
}
