using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CancelingRequestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{delayMilliseconds:int}")]
        public async Task<IEnumerable<WeatherForecast>> Get(CancellationToken cancellationToken, int delayMilliseconds)
        {
            _logger.LogInformation("Request started.");
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            await Task.Delay(delayMilliseconds, cancellationToken);
            sw.Stop();
            _logger.LogInformation($"Canceled after {sw.ElapsedMilliseconds}ms.");
            cancellationToken.ThrowIfCancellationRequested();

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
