using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionAndStockERP.Interfaces;

namespace ProductionAndStockERP.Controllers
{
    [ApiController]
    [Authorize(Roles ="Admin")]
    [Route("api/activitylog")]
    public class ActivityLogController : ControllerBase
    {
        private readonly IActivityLogsService _activityLogsService;
        public ActivityLogController(IActivityLogsService activityLogsService)
        {
            _activityLogsService = activityLogsService;
        }
        [HttpGet("getallactivitiylogs")]
        public async Task<IActionResult> GetAllActivitiyLogs()
        {
            var result =await _activityLogsService.GetAllLogsAsync();
            return Ok(result);
        }

        [HttpGet("getlogsbyuserid/{id}")]
        public async Task<IActionResult> GetLogsByUserId(int id)
        {
            var result = await _activityLogsService.GetLogsByUserIdAsync(id);
            return Ok(result);
        }

        [HttpGet("getlogbyid/{id}")]
        public async Task<IActionResult> GetLogById(int id)
        {
            var result = await _activityLogsService.GetLogByIdAsync(id);
            return Ok();
        }
    }
}
