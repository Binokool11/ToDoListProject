using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Enum;
using ToDoList.Domain.Filters.Task;
using ToDoList.Domain.Response.Interface;
using ToDoList.Domain.ViewModels;
using ToDoList.Services.Interfaces;
using ToDoList.Services.Services;

namespace ToDoList.Controllers
{
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private readonly ITaskService _taskService;

        public TaskController(ILogger<TaskController> logger, ITaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        public IActionResult Index()
        {
            return View();
        }
                
        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskViewModel model)
        {
            if (model != null)
            {
                _logger.LogInformation($"Описание задачи: {model.Description}; Имя:{model.Name}; Приоритет:{model.Priority}");
                var response = await _taskService.CreateAsync(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return Ok(new { description = response.Description });
                }
            }
            return BadRequest(new { description = "Модель была пуста" });
        }

        public async Task<IActionResult> TaskHandler(TaskFilters taskFilter)
        {

            var priority = HttpContext.Request.Headers["Priority"].ToString();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();

            taskFilter.Priority = (TaskPriority)int.Parse(priority);
            taskFilter.Skip = start != null ? int.Parse(start) : 0;
            taskFilter.PageSize = length != null ? int.Parse(length) : 0;



            var response = await _taskService.GetTasksAsync(taskFilter);
            var data = response.Data as IBaseResponse<IEnumerable<TaskViewModel>>;
            if (data != null && data.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Json(new { recordsFiltered = response.LengthDataBase, recordsTotal = response.LengthDataBase, data = data.Data });
            }
            return RedirectToAction("Index");    
        }

        [HttpPost]
        public async Task<IActionResult> EndTask(int id)
        {
            var response = await _taskService.CompliteTaskAsync(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok(new { description = "Задача выполнена!"});
            }
            return BadRequest(new { description = "Задача не найдена или произошла ошибка на сервере"});
        }

        [HttpGet]
        public async Task<IActionResult> GetComplitedTasks()
        {
            var response = await _taskService.GetComplitedTasksAsync();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok(new { data = response.Data });
            }
            return BadRequest(new { description = "Данные не найдены или произошла ошибка на сервере"});
        }

        [HttpGet]
        public async Task<IActionResult> FinishDay()
        {
            var response = await _taskService.FinishTasks();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var bytes = new CsvBaseService<IEnumerable<TaskViewModel>>().UploadFile(response.Data);
                return File(bytes,"text/csv",$"Статистика за {DateTime.Now.ToLongDateString()}.csv");
            }
            return BadRequest(new {description = response.Description});
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var response = await _taskService.DeleteTaskAsync(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok(new { description = response.Description});
            }
            return BadRequest(new { description = response.Description });
        }
    }
}