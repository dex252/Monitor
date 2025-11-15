using Microsoft.AspNetCore.Mvc;
using Monitor.Repositories.Inretfaces;

namespace Monitor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly IHealthDbRepository _healthDbRepository;
        public HealthCheckController(IHealthDbRepository healthDbRepository)
        {
            _healthDbRepository = healthDbRepository;
        }

        [HttpGet("to-postgres")]
        public async Task<IActionResult> ToPostgres()
        {
            var result = await _healthDbRepository.CheckConnection();
            if (!result)
            {
                return BadRequest();
            }
            
            return Ok(result);
        }
    }
}