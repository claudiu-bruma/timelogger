using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Timelogger.Core.DTOs;
using Timelogger.Core.Interfaces;

namespace Timelogger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimelogsController : ControllerBase
    {
        private readonly ITimeLogService _timeLogService; 

        public TimelogsController(ITimeLogService timeLogService)
        {
            _timeLogService = timeLogService;
        } 

        [HttpGet]
        public async Task<IActionResult> Get(int projectId, CancellationToken cancellationToken)
        {
            var timelogs =await _timeLogService.GetLogsByProjectId(projectId, cancellationToken);
            return Ok(timelogs);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddTimeLogDto timeLog, CancellationToken cancellationToken)
        {
            if (timeLog is null)
            {
                return BadRequest();
            }
            
            await _timeLogService.AddTimeLog(timeLog, cancellationToken);
            return Ok();
        }


    }
}
