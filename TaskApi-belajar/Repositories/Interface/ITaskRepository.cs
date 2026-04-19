using TaskApi_belajar.Models;

namespace TaskApi_belajar.Repositories.Interface
{
    public interface ITaskRepository
    {
        Task<List<TaskItem>> GetAllAsync();
        Task<TaskItem> GetByIdAsync(int id);
        Task<TaskItem> CreateAsync(TaskItem task);
    }
}
