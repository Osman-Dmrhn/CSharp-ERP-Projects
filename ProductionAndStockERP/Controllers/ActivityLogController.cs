// Dosya: Controllers/ActivityLogController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;

namespace ProductionAndStockERP.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/activitylog")]
    public class ActivityLogController : ControllerBase
    {
        private readonly IActivityLogsService _activityLogsService;

        public ActivityLogController(IActivityLogsService activityLogsService)
        {
            _activityLogsService = activityLogsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActivitiyLogs([FromQuery] LogFilterParameters filters)
        {
            var result = await _activityLogsService.GetAllLogsAsync(filters);
            if (result.Success)
            {
                Response.AddPaginationHeader(result.Data.CurrentPage, result.Data.PageSize, result.Data.TotalCount, result.Data.TotalPages);
                return Ok(result.Data.Items);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLogById(int id)
        {
            var result = await _activityLogsService.GetLogByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}