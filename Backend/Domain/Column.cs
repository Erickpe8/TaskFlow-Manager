namespace TaskFlow.Api.Domain
{
    public class Column
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Order { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
