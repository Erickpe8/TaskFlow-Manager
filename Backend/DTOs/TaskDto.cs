namespace TaskFlow.Api.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int ColumnId { get; set; }
        public string ColumnName { get; set; } = ""; 
        public int? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int Order { get; set; }
    }

    public class CreateTaskDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int ColumnId { get; set; }
        public int? Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class UpdateTaskDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int ColumnId { get; set; }
        public int? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int Order { get; set; }
    }

    // Para el drag & drop
    public class MoveTaskDto
    {
        public int ColumnId { get; set; }
        public int NewOrder { get; set; }
    }
}
