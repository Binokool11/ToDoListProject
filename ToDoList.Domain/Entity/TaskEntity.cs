using ToDoList.Domain.Enum;

namespace ToDoList.Domain.Entity
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsCompleted { get; set; } = false;
        public DateTime DateTime { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
