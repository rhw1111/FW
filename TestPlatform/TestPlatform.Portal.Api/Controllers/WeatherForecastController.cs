using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MSLibrary.Logger;
using FW.TestPlatform.Main;

namespace FW.TestPlatform.Portal.Api.Controllers
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

        
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpHead]
        public async Task Do()
        {
            await Task.CompletedTask;
        }

        [HttpPost]
        public async Task Post()
        {
            var apiKey=HttpContext.Request.Headers["Sm-Apikey"][0];
            var signature = HttpContext.Request.Headers["Sm-Signature"][0];
            LoggerHelper.LogError(LoggerCategoryNames.TestPlatform_Portal_Api, $"Sm-Apikey:{apiKey},Sm-Signature:{signature}");
            await Task.CompletedTask;
        }
    }
}
