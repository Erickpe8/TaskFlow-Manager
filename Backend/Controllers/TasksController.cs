using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Interfaces;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: api/tasks
        [HttpGet]
        [Authorize(Policy = "Tasks.Read")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll()
        {
            var tasks = await _taskService.GetAllAsync();
            return Ok(tasks);
        }

        // GET: api/tasks/5
        [HttpGet("{id:int}")]
        [Authorize(Policy = "Tasks.Read")]
        public async Task<ActionResult<TaskDto>> GetById(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        // POST: api/tasks
        [HttpPost]
        [Authorize(Policy = "Tasks.create")]
        public async Task<ActionResult<TaskDto>> Create([FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _taskService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created
            );
        }

        // PUT: api/tasks/5
        [HttpPut("{id:int}")]
        [Authorize(Policy = "Tasks.update")]
        public async Task<ActionResult<TaskDto>> Update(int id, [FromBody] UpdateTaskDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _taskService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        // DELETE: api/tasks/5
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Tasks.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _taskService.DeleteAsync(id);
            if (!ok) return NotFound();

            return NoContent();
        }

        // PUT: api/tasks/5/move  → para drag & drop
        [HttpPut("{id:int}/move")]
        [Authorize(Policy = "Tasks.update")]
        public async Task<IActionResult> Move(int id, [FromBody] MoveTaskDto dto)
        {
            var ok = await _taskService.MoveAsync(id, dto);
            if (!ok) return NotFound();

            return NoContent();
        }
    }
}
