namespace ToDoList.DAL.Interfaces
{
    public interface IRepository<T>
    {
        T GetById(int id);
        IQueryable<T> GetAll();
        void Delete(T entity);
        void DeleteById(int id);
        void Update(T entity);
        void Add(T entity);
        Task<T> GetByIdAsync(int id);
        Task DeleteAsync(T entity);
        Task DeleteByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task AddAsync(T entity);
    }
}
