using System.ComponentModel.DataAnnotations;
namespace ToDoList.Domain.Enum
{
    public enum TaskPriority
    {
        [Display(Name = "Простая")]
        Easy = 1,
        [Display(Name = "Средняя")]
        Medium = 2,
        [Display(Name = "Сложная")]
        Hard = 3
    }
}
