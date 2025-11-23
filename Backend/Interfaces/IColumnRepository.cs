using TaskFlow.Api.DTOs;

namespace TaskFlow.Api.Interfaces
{
    public interface IColumnService
    {
        Task<List<ColumnDto>> GetAllWithTasksAsync();
        Task<ColumnDto?> GetByIdAsync(int id);
        Task<ColumnDto> CreateAsync(CreateColumnDto dto);
        Task<ColumnDto?> UpdateAsync(int id, UpdateColumnDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
