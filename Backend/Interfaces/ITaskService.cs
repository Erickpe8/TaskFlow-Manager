using TaskFlow.Api.DTOs;

namespace TaskFlow.Api.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetAllAsync();
        Task<TaskDto?> GetByIdAsync(int id);
        Task<TaskDto> CreateAsync(CreateTaskDto dto);
        Task<TaskDto?> UpdateAsync(int id, UpdateTaskDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> MoveAsync(int id, MoveTaskDto dto);
    }
}
