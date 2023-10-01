using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoList.DAL.Interfaces;
using ToDoList.Domain.Entity;

namespace ToDoList.DAL.Repositories
{
    public class TaskRepository : IRepository<TaskEntity>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TaskRepository> _logger;
        public TaskRepository(ApplicationDbContext context, ILogger<TaskRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public void Add(TaskEntity entity)
        {
            if (entity != null && !_context.Tasks.Contains(entity))
            {
                _context.Tasks.Add(entity);
                _context.SaveChanges();
                _logger.LogInformation($"Добавлены новые данные в базу данных; Name = {entity.Name}");
            }
            else throw new ArgumentNullException("Произошла ошибка доступа к базе данных");
        }

        public async Task AddAsync(TaskEntity entity)
        {
            if (entity != null && !_context.Tasks.Contains(entity))
            {
                await _context.Tasks.AddAsync(entity);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Добавлены новые данные в базу данных; Name = {entity.Name}");
            }
            else throw new ArgumentNullException("Произошла ошибка доступа к базе данных");
        }

        public void Delete(TaskEntity entity)
        {
            if (entity != null && _context.Tasks.Contains(entity))
            {
                _context.Tasks.Remove(entity);
                _context.SaveChanges();
                _logger.LogInformation($"Удалены данные из базы данных; Name = {entity.Name}");
            }
            else
            {
                _logger.LogWarning("Произошла ошибка. Данные из базы данных не удалены");
                throw new ArgumentNullException("Произошла ошибка доступа к базе данных");
            }
        }

        public async Task DeleteAsync(TaskEntity entity)
        {
            if (entity != null && _context.Tasks.Contains(entity))
            {
                _context.Tasks.Remove(entity);
                await _context.SaveChangesAsync();
            }
            else throw new ArgumentNullException("Произошла ошибка доступа к базе данных");
        }

        public IQueryable<TaskEntity> GetAll()
        {
            return _context.Tasks;
        }

        public async Task<TaskEntity> GetByIdAsync(int id)
        {
            var currentTask = await _context.Tasks.FirstOrDefaultAsync(task => task.Id == id);
            if (currentTask != null)
            {
                _logger.LogInformation("Извлечение данных из базы данных прошло успешно");
                return currentTask;
            }
            else throw new ArgumentNullException($"Извлечение данных прошло не успешно. Не найден объект в базе данных с id = {id}");
        }

        public TaskEntity GetById(int id)
        {
            var currentTask = _context.Tasks.FirstOrDefault(task => task.Id == id);
            if (currentTask != null)
            {
                _logger.LogInformation("Извлечение данных из базы данных прошло успешно");
                return currentTask;
            }
            else throw new ArgumentNullException($"Извлечение данных прошло не успешно. Не найден объект в базе данных с id = {id}");
        }

        public void Update(TaskEntity entity)
        {
            if (entity != null && _context.Tasks.Contains(entity))
            {
                _context.Tasks.Update(entity);
                _context.SaveChanges();
            }
            else throw new ArgumentNullException("Произошла ошибка доступа к базе данных");
        }

        public async Task UpdateAsync(TaskEntity entity)
        {
            if (entity != null && _context.Tasks.Contains(entity))
            {
                _context.Tasks.Update(entity);
                await _context.SaveChangesAsync();
            }
            else throw new ArgumentNullException("Произошла ошибка доступа к базе данных");
        }

        public void DeleteById(int id)
        {
            var task = _context.Tasks.FirstOrDefault(task => task.Id == id);
            if (task != null)
            {
                _context.Remove(task);
                _context.SaveChanges();
            }
            else throw new ArgumentNullException("Произошла ошибка доступа к базе данных");
        }

        public async Task DeleteByIdAsync(int id)
        {
            var task = _context.Tasks.FirstOrDefault(task => task.Id == id);
            if (task != null)
            {
                _context.Remove(task);
                await _context.SaveChangesAsync();
            }
            else throw new ArgumentNullException("Произошла ошибка доступа к базе данных");
        }
    }
}