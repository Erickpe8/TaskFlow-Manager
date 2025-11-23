using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Interfaces;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services
{
    public class ColumnService : IColumnService
    {
        private readonly AppDbContext _context;

        public ColumnService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ColumnDto>> GetAllWithTasksAsync()
        {
            var columns = await _context.Columns
                .Include(c => c.Tasks)
                .AsNoTracking()
                .OrderBy(c => c.Order)
                .ToListAsync();

            return columns.Select(MapToDtoWithTasks).ToList();
        }

        public async Task<ColumnDto?> GetByIdAsync(int id)
        {
            var column = await _context.Columns
                .Include(c => c.Tasks)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (column == null) return null;

            return MapToDtoWithTasks(column);
        }

        public async Task<ColumnDto> CreateAsync(CreateColumnDto dto)
        {
            var entity = new Column
            {
                Name = dto.Name,
                Order = dto.Order
            };

            _context.Columns.Add(entity);
            await _context.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task<ColumnDto?> UpdateAsync(int id, UpdateColumnDto dto)
        {
            var column = await _context.Columns.FirstOrDefaultAsync(c => c.Id == id);
            if (column == null) return null;

            column.Name = dto.Name;
            column.Order = dto.Order;

            await _context.SaveChangesAsync();

            return MapToDto(column);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var column = await _context.Columns.FirstOrDefaultAsync(c => c.Id == id);
            if (column == null) return false;

            _context.Columns.Remove(column);
            await _context.SaveChangesAsync();

            return true;
        }

        // ------------ Mapeos privados ------------

        private static ColumnDto MapToDto(Column entity)
        {
            return new ColumnDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Order = entity.Order,
                Tasks = new List<TaskDto>()
            };
        }

        private static ColumnDto MapToDtoWithTasks(Column entity)
        {
            return new ColumnDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Order = entity.Order,
                Tasks = entity.Tasks
                    .OrderBy(t => t.Order)
                    .Select(t => new TaskDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        ColumnId = t.ColumnId,
                        Priority = t.Priority,
                        DueDate = t.DueDate,
                        Order = t.Order
                    })
                    .ToList()
            };
        }
    }
}
