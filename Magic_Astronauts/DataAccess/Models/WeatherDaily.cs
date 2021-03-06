using System.ComponentModel.DataAnnotations;

namespace Magic_Astronauts.DataAccess.Models
{
    public class WeatherDaily
    {
        [Required]
        public int Id { get; set; }
        // SelectDate shows average temp for a chosen day
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime SelectDate { get; set; }
        public double AverageHumidity { get; set; }
        public double AverageTemperature { get; set; }
        public string Location { get; set; }
    }
}
