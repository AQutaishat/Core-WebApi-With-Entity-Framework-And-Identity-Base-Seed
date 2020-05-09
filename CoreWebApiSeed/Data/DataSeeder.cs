using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApiWithEntityFrameworkAndIdentityBaseSeed.Data
{
    public class DataSeeder
    {
        private static readonly string[] Summaries = new[]
{
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ApplicationDbContext _db;
        public DataSeeder(ApplicationDbContext db)
        {
            this._db = db;
        }
        public void SeedData()
        {
            var ExistingRecords = this._db.WeatherForecasts.Any();
            if(ExistingRecords)
            {
                return;
            }

            var rng = new Random();
            var forcastList = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToList();

            foreach (var item in forcastList)
            {
                this._db.WeatherForecasts.Add(item);
            }
            this._db.SaveChanges();
        }
    }
}
