using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Monitor.Services;

namespace Monitor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
         private readonly IServiceProvider _serviceProvider;

        public HomeController(IServiceProvider serviceProvider)
        {
             _serviceProvider = serviceProvider;
        }

        [HttpGet("status")]
        public IActionResult Status()
        {
            var backgroundService = GetBackgroundService();
            var isSuccess = backgroundService?.Status();

            return Ok(isSuccess);
        }

        [HttpGet("start")]
        public async Task<IActionResult> StartService()
        {
            var backgroundService = GetBackgroundService();
            var isSuccess = backgroundService?.Start();

            return Ok(isSuccess);
        }

        [HttpGet("stop")]
        public IActionResult StopService()
        {
            var backgroundService = GetBackgroundService();
            var isSuccess = backgroundService?.Stop();

            return Ok(isSuccess);
        }

         private BackgroundGenerateLogService? GetBackgroundService()
        {
            var hostedServices = _serviceProvider.GetServices<IHostedService>();
            var backgroundService = hostedServices.OfType<BackgroundGenerateLogService>().FirstOrDefault();
            
            return backgroundService;
        }
    }
}