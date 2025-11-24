using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Interfaces;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskDto>> GetAllAsync()
        {
            var tasks = await _context.TaskItems
                .AsNoTracking()
                .OrderBy(t => t.ColumnId)
                .ThenBy(t => t.Order)
                .ToListAsync();

            return tasks.Select(MapToDto).ToList();
        }

        public async Task<TaskDto?> GetByIdAsync(int id)
        {
            var task = await _context.TaskItems
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            return task == null ? null : MapToDto(task);
        }

        public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
        {
            // Calcular el último orden en la columna
            var lastOrder = await _context.TaskItems
                .Where(t => t.ColumnId == dto.ColumnId)
                .MaxAsync(t => (int?)t.Order) ?? 0;

            var entity = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                ColumnId = dto.ColumnId,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                Order = lastOrder + 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.TaskItems.Add(entity);
            await _context.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task<TaskDto?> UpdateAsync(int id, UpdateTaskDto dto)
        {
            var task = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id);
            if (task == null) return null;

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.ColumnId = dto.ColumnId;
            task.Priority = dto.Priority;
            task.DueDate = dto.DueDate;
            task.Order = dto.Order;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToDto(task);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id);
            if (task == null) return false;

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

    public async Task<bool> MoveAsync(int id, MoveTaskDto dto)
    {
        var task = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id);
        if (task == null) return false;

        // Cambiar columna
        task.ColumnId = dto.ColumnId;

        // Cambiar orden
        task.Order = dto.NewOrder;

        // Actualizar fecha
        task.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

        // ------------ Mapeos privados ------------

        private static TaskDto MapToDto(TaskItem entity)
        {
            return new TaskDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                ColumnId = entity.ColumnId,
                Priority = entity.Priority,
                DueDate = entity.DueDate,
                Order = entity.Order
            };
        }
    }
}
