using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Interfaces;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColumnsController : ControllerBase
    {
        private readonly IColumnService _columnService;

        public ColumnsController(IColumnService columnService)
        {
            _columnService = columnService;
        }

        // GET: api/columns
        // Devuelve las columnas con sus tareas → ideal para el Kanban
        [HttpGet]
        [Authorize(Policy = "Columns.read")]
        public async Task<ActionResult<IEnumerable<ColumnDto>>> GetAll()
        {
            var columns = await _columnService.GetAllWithTasksAsync();
            return Ok(columns);
        }

        // GET: api/columns/3
        [HttpGet("{id:int}")]
        [Authorize(Policy = "Columns.read")]
        public async Task<ActionResult<ColumnDto>> GetById(int id)
        {
            var column = await _columnService.GetByIdAsync(id);
            if (column == null) return NotFound();

            return Ok(column);
        }

        // POST: api/columns
        [HttpPost]
        [Authorize(Policy = "Columns.create")]
        public async Task<ActionResult<ColumnDto>> Create([FromBody] CreateColumnDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _columnService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created
            );
        }

        // PUT: api/columns/3
        [HttpPut("{id:int}")]
        [Authorize(Policy = "Columns.update")]
        public async Task<ActionResult<ColumnDto>> Update(int id, [FromBody] UpdateColumnDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _columnService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        // DELETE: api/columns/3
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Columns.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _columnService.DeleteAsync(id);
            if (!ok) return NotFound();

            return NoContent();
        }
    }
}
