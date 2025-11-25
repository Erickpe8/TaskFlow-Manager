namespace TaskFlow.Api.Domain
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public int ColumnId { get; set; }
        public Column Column { get; set; } = null!;

        public int? Priority { get; set; }
        public DateTime? DueDate { get; set; }

        // Para ordenar dentro de la columna (Kanban)
        public int Order { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
