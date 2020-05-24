using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BauGrid.Test.Data
{
	public class WeatherForecastService
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		public async Task<List<WeatherForecast>> GetForecastAsync(DateTime startDate, int items)
		{
			List<WeatherForecast> forecasts = new List<WeatherForecast>();
			Random rnd = new Random();

				await Task.Delay(1);
				for (int index = 0; index < items; index++)
					forecasts.Add(new WeatherForecast 
											{
												Date = startDate.AddDays(rnd.Next(-10, 10)),
												TemperatureC = rnd.Next(-20, 55),
												Summary = Summaries[rnd.Next(Summaries.Length)],
												IsReal = rnd.Next(5) > 2
											}
								  );
				return forecasts;
		}
	}
}
