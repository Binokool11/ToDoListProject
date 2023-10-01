using ToDoList.Domain.Enum;

namespace ToDoList.Domain.ViewModels
{
    public class CreateTaskViewModel
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public TaskPriority Priority { get; set; }
        public string FullDescription { get; set; } = null!;

        public bool Validate()
        {
            return !(Name == null || Description == null);
        }
    }
}
