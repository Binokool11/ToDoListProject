using System.ComponentModel.DataAnnotations;
namespace ToDoList.Domain.Enum
{
    public enum FilterPriority
    {
        [Display(Name = "Все")]
        All = 0,
        [Display(Name = "Простая")]
        Easy = 1,
        [Display(Name = "Средняя")]
        Medium = 2,
        [Display(Name = "Сложная")]
        Hard = 3,
        
    }
}
