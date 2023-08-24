namespace SolarWatch5.Models
{
    public class SunsetSunriseData
    {
        public DateOnly Date { get; set; }
        public TimeOnly Sunrise { get; set; }
        public TimeOnly Sunset { get; set; }
    }
}
