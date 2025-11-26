namespace TaskFlow.Api.Interfaces;

public interface IExportService
{
    Task<byte[]> ExportTasksToExcelAsync(int userId);
    Task<byte[]> ExportTasksToPdfAsync(int userId);
}