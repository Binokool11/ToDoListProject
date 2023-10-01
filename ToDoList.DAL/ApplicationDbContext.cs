using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Entity;

namespace ToDoList.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<TaskEntity> Tasks { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }   
    }
}
