using Magic_Astronauts.DataAccess;
using Magic_Astronauts.DataAccess.Models;

namespace Magic_Astronauts.Core
{
    public class DailyCalc : WeatherDaily
    {
        //PopulateAvgTempAndHumidity populates WeatherDailies-table in DB.
        public static void PopulateAvgTempAndHumidity(WeatherDbContext context)
        {
            string Place = "Ute";
            bool running = true;
            List<DailyCalc> AvgTempAsList = new List<DailyCalc>();
            while (running)
            {
                DateTime StartDate = new DateTime(2016, 10, 01);
                while (StartDate.Month != 12)
                {
                    // Query to get data for temp & humidity.
                    int counterTemp = 0;
                    int counterHumi = 0;
                    List<double> counterListHum = new List<double>();
                    List<double> counterListTemp = new List<double>();
                    var CSV = context.CsvModels.ToList();
                    var dateSelect = (from m in CSV
                                      where m.Date.DayOfYear == StartDate.DayOfYear
                                      select m).ToList();
                    var placeSelect = (from e in dateSelect
                                       where e.Location == Place
                                       select e).ToList();
                    var queryTemp = (from k in placeSelect
                                     select k.Temp).ToList();
                    var queryHumid = (from k in placeSelect
                                      select k.Humidity).ToList();

                    // Calculation for getting average temp & humidity using Math.Round();
                    if (queryTemp.Count() != 0 || queryHumid.Count() != 0)
                    {
                        foreach (var item in queryHumid)
                        {
                            counterHumi++;
                            counterListHum.Add(counterHumi);
                        }
                        double total = queryHumid.Sum();
                        double avgHum = Math.Round((total / counterListHum.Count()), 2);
                        foreach (var item in queryTemp)
                        {
                            counterTemp++;
                            counterListTemp.Add(counterTemp);
                        }
                        double total2 = queryTemp.Sum();
                        double avgTemp = Math.Round((total2 / counterListTemp.Count()), 2);

                        // Send back the values of our calculations.
                        DailyCalc avgTemps = new DailyCalc
                        {
                            Location = Place,
                            AverageTemperature = avgTemp,
                            AverageHumidity = avgHum,
                            SelectDate = StartDate
                        };
                        AvgTempAsList.Add(avgTemps);
                    }
                    StartDate = StartDate.AddDays(1);
                }
                if (Place == "Inne")
                {
                    running = false;
                }
                Place = "Inne";
            }
            context.WeatherDailies.BulkInsert(AvgTempAsList);
            context.SaveChanges();
        }
    }
}
