using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Monitor.Models.DTOs.Requests;
using Monitor.Models.Entities.Postgres;
using Monitor.Repositories.Inretfaces;

namespace Monitor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        readonly ILogsRepository _logsRepository;
        public LogsController(ILogsRepository logsRepository)
        {
            _logsRepository = logsRepository;
        }

        [HttpPost("add")]
        [Consumes("application/json")]
        public async Task<IActionResult> AddLog(CreateLogRequest logDto)
        {
            var log = new Log(logDto);

            var id = await _logsRepository.AddLogAsync(log);
            return Ok(id);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetLog(decimal id)
        {
            var log = await _logsRepository.GetLogAsync(id);
            return Ok(log);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetLogs()
        {
            var logs = await _logsRepository.GetAllLogsAsync();
            return Ok(logs);
        }
    }
}