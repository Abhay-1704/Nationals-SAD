using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Session3.Models;

namespace Session3.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class TaskController : Controller
    {
        Session3FinalContext db = new Session3FinalContext();

        [HttpGet]
        public ActionResult<List<object>> Gettasks()
        {
            var tasks = db.Pmtasks
    .Include(x => x.Asset)
    .Include(x => x.Task)
    .Include(x => x.PmscheduleType)
        .ThenInclude(x => x.PmscheduleModels)
    .Select(x => new
    {
        Id = x.Id,
        AssetName = x.Asset.AssetName,
        AssetSN = x.Asset.AssetSn,
        Task = x.Task.Name,
        Type = x.PmscheduleType.Name,
        Model = x.PmscheduleType.PmscheduleModels
                    .Select(m => m.Name)
                    .FirstOrDefault(),
        Date = x.ScheduleDate,
        Mileage = x.ScheduleKilometer,
        Done = x.TaskDone
    }).ToList();
            if (!tasks.Any()){
                return NotFound("No Tasks Yet");
            }
            return Ok(tasks);
        }

        [HttpPut("{id}/done")]
        public ActionResult ToggleTaskDone(long id, [FromBody] ToggleDoneRequest request)
        {
            try
            {
                var pmTask = db.Pmtasks.Where(x => x.Id == id).FirstOrDefault();

                if (pmTask == null)
                {
                    return NotFound("Task not Found");
                }

                pmTask.TaskDone = request.TaskDone;
                db.SaveChanges();

                return Ok(new { Id = id, TaskDone = pmTask.TaskDone });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("assets")]
        public ActionResult<List<object>> GetAssetsDropdown()
        {
            var assets = db.Assets.Select(x => new
            {
                Id = x.Id,
                AssetName = x.AssetName
            }).ToList();

            if (!assets.Any())
            {
                return NotFound("No assets found");
            }

            return Ok(assets);
        }

        [HttpGet("tasks")]
        public ActionResult<List<object>> GetTasksDropdown()
        {
            var tasks = db.Tasks.Select(x => new
            {
                Id = x.Id,
                TaskName = x.Name
            }).ToList();

            if (!tasks.Any())
            {
                return NotFound("No tasks to show");
            }

            return Ok(tasks);
        }

        public class ToggleDoneRequest
        {
            public bool TaskDone { get; set; }
        }
    }
}
