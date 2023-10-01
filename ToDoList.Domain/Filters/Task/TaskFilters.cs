using ToDoList.Domain.Enum;

namespace ToDoList.Domain.Filters.Task
{
    public class TaskFilters : PagingFilter
    {
        public TaskPriority Priority { get; set; }
    }
}
