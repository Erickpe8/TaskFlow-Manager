using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.Domain;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Interfaces;

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
                .Include(t => t.Column)              // 🔥 NECESARIO
                .AsNoTracking()
                .OrderBy(t => t.Column.Order)
                .ThenBy(t => t.Order)
                .ToListAsync();

            return tasks.Select(MapToDto).ToList();
        }

        public async Task<TaskDto?> GetByIdAsync(int id)
        {
            var task = await _context.TaskItems
                .Include(t => t.Column)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            return task == null ? null : MapToDto(task);
        }

        public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
        {
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

            await _context.Entry(entity).Reference(t => t.Column).LoadAsync();

            return MapToDto(entity);
        }

        public async Task<TaskDto?> UpdateAsync(int id, UpdateTaskDto dto)
        {
            var task = await _context.TaskItems
                .Include(t => t.Column)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null) return null;

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.ColumnId = dto.ColumnId;
            task.Priority = dto.Priority;
            task.DueDate = dto.DueDate;
            task.Order = dto.Order;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            await _context.Entry(task).Reference(t => t.Column).LoadAsync();

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

            task.ColumnId = dto.ColumnId;
            task.Order = dto.NewOrder;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        private static TaskDto MapToDto(TaskItem entity)
        {
            return new TaskDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                ColumnId = entity.ColumnId,
                ColumnName = entity.Column?.Name ?? "", // 🔥 ESTA LÍNEA ARREGLA EL LISTADO
                Priority = entity.Priority,
                DueDate = entity.DueDate,
                Order = entity.Order
            };
        }

        // Services/TaskService.cs - Agrega este método
        public async Task<PaginatedResult<TaskDto>> GetFilteredAsync(TaskFilterDto filter)
        {
            var query = _context.TaskItems
                .Include(t => t.Column)
                .AsNoTracking()
                .AsQueryable();

            // Búsqueda
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.ToLower();
                query = query.Where(t =>
                    t.Title.ToLower().Contains(search) ||
                    (t.Description != null && t.Description.ToLower().Contains(search))
                );
            }

            // Filtro por columna
            if (filter.ColumnId.HasValue)
            {
                query = query.Where(t => t.ColumnId == filter.ColumnId.Value);
            }

            // Filtro por prioridad
            if (filter.Priority.HasValue)
            {
                query = query.Where(t => t.Priority == filter.Priority.Value);
            }

            // Ordenamiento
            query = filter.SortBy?.ToLower() switch
            {
                "priority" => filter.SortDesc
                    ? query.OrderByDescending(t => t.Priority)
                    : query.OrderBy(t => t.Priority),
                "duedate" => filter.SortDesc
                    ? query.OrderByDescending(t => t.DueDate)
                    : query.OrderBy(t => t.DueDate),
                "order" => filter.SortDesc
                    ? query.OrderByDescending(t => t.Order)
                    : query.OrderBy(t => t.Order),
                "column" => filter.SortDesc
                    ? query.OrderByDescending(t => t.Column.Name)
                    : query.OrderBy(t => t.Column.Name),
                _ => filter.SortDesc
                    ? query.OrderByDescending(t => t.Title)
                    : query.OrderBy(t => t.Title)
            };

            // Total antes de paginar
            var totalCount = await query.CountAsync();

            // Paginación
            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PaginatedResult<TaskDto>
            {
                Items = items.Select(MapToDto).ToList(),
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }
    }
}
