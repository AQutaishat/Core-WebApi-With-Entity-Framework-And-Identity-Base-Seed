using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApiWithEntityFrameworkAndIdentityBaseSeed.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreWebApiWithEntityFrameworkAndIdentityBaseSeed.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ApplicationDbContext _Db;


        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ApplicationDbContext Db)
        {
            _logger = logger;
            this._Db = Db;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var Query = this._Db.WeatherForecasts.ToList();
            return Query.ToList();
        }
    }
}
