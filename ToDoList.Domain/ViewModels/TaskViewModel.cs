using System.ComponentModel.DataAnnotations;

namespace ToDoList.Domain.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; } = null!;

        [Display(Name = "Описание")]
        public string Description { get; set; } = null!;

        [Display(Name = "Выполнение")]
        public string IsCompleted { get; set; } = null!;

        [Display(Name = "Дата создания")]
        public string DateTime { get; set; } = null!;

        [Display(Name = "Приоритет")]
        public string Priority { get; set; } = null!;
    }
}
