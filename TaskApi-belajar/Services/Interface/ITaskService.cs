using TaskApi_belajar.DTOs;

namespace TaskApi_belajar.Services.Interface
{
    public interface ITaskService
    {
        Task<List<TaskResponseDTO>> GetTasksAsync();
        Task<TaskResponseDTO> CreateTaskAsync(CreateTaskDTO dto);

    }
}
