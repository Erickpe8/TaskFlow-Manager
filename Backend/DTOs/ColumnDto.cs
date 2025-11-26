namespace TaskFlow.Api.DTOs
{
    public class ColumnDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Order { get; set; }
        public List<TaskDto> Tasks { get; set; } = new();
    }

    public class CreateColumnDto
    {
        public string Name { get; set; } = null!;
        public int Order { get; set; }
    }

    public class UpdateColumnDto
    {
        public string Name { get; set; } = null!;
        public int Order { get; set; }
    }
}
