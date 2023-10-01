using ToDoList.Domain.Entity;
using ToDoList.Domain.Filters;
using ToDoList.Domain.Filters.Task;
using ToDoList.Domain.Response;
using ToDoList.Domain.Response.Interface;
using ToDoList.Domain.ViewModels;

namespace ToDoList.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IBaseResponse<TaskEntity>> CreateAsync(CreateTaskViewModel model);
        Task<IBaseResponse<IEnumerable<TaskViewModel>>> FinishTasks();
        Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetComplitedTasksAsync();
        public Task<IBaseResponse<TaskEntity>> CompliteTaskAsync(int id);
        Task<DataTableResponse> GetTasksAsync(TaskFilters filter);
        Task<IBaseResponse<TaskEntity>> DeleteTaskAsync(int id);
    }
}
