using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoList.DAL.Interfaces;
using ToDoList.Domain.Builders;
using ToDoList.Domain.Entity;
using ToDoList.Domain.Enum;
using ToDoList.Domain.Extensions;
using ToDoList.Domain.Extenstions;
using ToDoList.Domain.Filters.Task;
using ToDoList.Domain.Response;
using ToDoList.Domain.Response.Interface;
using ToDoList.Domain.ViewModels;
using ToDoList.Services.Interfaces;
//TODO:Попробовать реализовать новый репозиторий и реализовать инвесию зависимостей
//TODO:Когда заканчиваю день, тогда удаляются те задачи которые уже были выполнены и обновляется список выполненых задач
//TODO:Использовать bundler and minifier 
//TODO:Посмотреть как тестить код и начать писать тесты
//TODO:Разобраться с логером который будет записывать в файл
//TODO:Закинуть на гит все три связаных проекта
namespace ToDoList.Services.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<TaskEntity> _repository;
        private readonly ILogger<TaskService> _logger;
        public TaskService(IRepository<TaskEntity> repository, ILogger<TaskService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<IBaseResponse<TaskEntity>> CreateAsync(CreateTaskViewModel model)
        {
            var response = new ResponseBuilder<TaskEntity>();
            if (model != null && model.Validate())
            {
                var newEntity = _repository.GetAll().Where(task => task.DateTime.Date == DateTime.Today).FirstOrDefault(task => task.Name == model.Name);
                if (newEntity == null)
                {
                    try
                    {
                        newEntity = new TaskEntity
                        {
                            DateTime = DateTime.Now,
                            Name = model.Name,
                            Description = model.Description,
                            Priority = model.Priority
                        };
                        await _repository.AddAsync(newEntity);
                        response.SetStatusCode(StatusCode.OK).SetMessage("Данные успешно добавлены");
                        _logger.LogInformation("Данные успешно добавлены");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Произошла ошибка при обращении к базе данных;\n{ex.Message}");
                        response.SetStatusCode(StatusCode.InternalServerError).SetMessage(ex.Message);
                    }
                }
                else
                {
                    _logger.LogInformation("Данные уже существуют");
                    response.SetStatusCode(StatusCode.ExistingTask).SetMessage("Данные уже существуют");
                }
            }
            else
            {
                _logger.LogInformation("Валидация модели не прошла успешно");
                response.SetStatusCode(StatusCode.InternalServerError).SetMessage("Укажите данные задачи");
            }
            return response.GetResponse();
        }

        public async Task<DataTableResponse> GetTasksAsync(TaskFilters filter)
        {
            var response = new DataTableResponse();
            try
            {
                var tasks = await _repository.GetAll()
                    .Where(task => !task.IsCompleted)
                    .WhereIf(filter != null && filter.Priority != 0, task => task.Priority == filter.Priority)
                    .Select(task =>
                        new TaskViewModel
                        {
                            Id = task.Id,
                            Name = task.Name,
                            Description = task.Description,
                            Priority = task.Priority.GetDisplayName(),
                            DateTime = task.DateTime.ToShortDateString(),
                            IsCompleted = task.IsCompleted == true ? "Готова" : "Не готова"
                        })
                    .Skip(filter.Skip)
                    .Take(filter.PageSize)
                    .OrderBy(task => task.Name)
                    .ToListAsync();
                _logger.LogInformation("Извлечение данных прошло успешно");
                response.LengthDataBase = _repository.GetAll().Count(task => !task.IsCompleted);
                response.Data =  new BaseResponse<IEnumerable<TaskViewModel>>
                {
                    Data = tasks,
                    Description = "Извлечение прошло успешно",
                    StatusCode = StatusCode.OK
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка извлечения данных");
                response.Data = new BaseResponse<IEnumerable<TaskViewModel>>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
                return response;
            }
        }

        public async Task<IBaseResponse<TaskEntity>> CompliteTaskAsync(int id)
        {
            var response = new ResponseBuilder<TaskEntity>();
            try
            {
                var task = _repository.GetAll().FirstOrDefault(task => task.Id == id);
                if (task != null && !task.IsCompleted)
                {
                    task.IsCompleted = true;
                    await _repository.UpdateAsync(task);
                    _logger.LogInformation("Данные успешно изменены");
                    return response.SetMessage("Данные успешо изменены").SetStatusCode(StatusCode.OK).SetData(task).GetResponse();
                }
                return response.SetMessage("Задача уже завершена или такой задачи не было найдено").SetStatusCode(StatusCode.NotFoundTasks).GetResponse();
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Данные не изменены. " + ex.Message);
                return response.SetMessage("Данные не изменены").SetStatusCode(StatusCode.NotFoundTasks).GetResponse();
            }
        }

        public async Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetComplitedTasksAsync()
        {
            var response = new ResponseBuilder<IEnumerable<TaskViewModel>>();
            try
            {
                var complitedTasks = await _repository.GetAll().Where(task => task.IsCompleted).Select(task =>
                new TaskViewModel
                {
                    Id = task.Id,
                    IsCompleted = "Готова",
                    Priority = task.Priority.GetDisplayName(),
                    DateTime = DateTime.Now.ToShortDateString(),
                    Description = task.Description,
                    Name = task.Name
                }).ToListAsync();
                if (complitedTasks != null)
                {
                    _logger.LogInformation("Извлечение прошло успешно");
                    return response.SetMessage("Извлечение прошло успешно").SetStatusCode(StatusCode.OK).SetData(complitedTasks).GetResponse();
                }
                return response.SetMessage("Не найдены завершенные задачи").SetStatusCode(StatusCode.NotFoundTasks).GetResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка извлечения данных");
                return response.SetMessage(ex.Message).SetStatusCode(StatusCode.InternalServerError).GetResponse();
            }
        }

        public async Task<IBaseResponse<IEnumerable<TaskViewModel>>> FinishTasks()
        {
            var response = new ResponseBuilder<IEnumerable<TaskViewModel>>();
            try
            {
                var tasks = await _repository.GetAll()
                    .Where(task => task.DateTime.Date == DateTime.Today)
                    .Select(task => new TaskViewModel
                    {
                        Name = task.Name,
                        Id = task.Id,
                        Priority = task.Priority.ToString(),
                        Description = task.Description,
                        DateTime = task.DateTime.ToString(),
                        IsCompleted = task.IsCompleted ? "Done":"Not done"
                    })
                    .ToListAsync();
                if (tasks != null)
                {
                    _logger.LogInformation("Извлечение прошло успешно");
                    return response.SetMessage("Извлечение прошло успешно").SetStatusCode(StatusCode.OK).SetData(tasks).GetResponse();
                }
                return response.SetMessage("Не найдены завершенные задачи").SetStatusCode(StatusCode.NotFoundTasks).GetResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка извлечения данных");
                return response.SetMessage(ex.Message).SetStatusCode(StatusCode.InternalServerError).GetResponse();
            }
        }
        public async Task<IBaseResponse<TaskEntity>> DeleteTaskAsync(int id)
        {
            var response = new ResponseBuilder<TaskEntity>();
            try
            {
                await _repository.DeleteByIdAsync(id);
                _logger.LogInformation("Удаление произошло успешно");
                return response.SetMessage("Удаление произошло успешно").SetStatusCode(StatusCode.OK).GetResponse();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Удаление не прошло успешно." + ex.Message);
                return response.SetMessage("Удаление не прошло успешно." + ex.Message).SetStatusCode(StatusCode.InternalServerError).GetResponse();
            }
        }
    }
}
