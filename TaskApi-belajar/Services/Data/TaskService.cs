using StackExchange.Redis;
using System.Text.Json;
using TaskApi_belajar.DTOs;
using TaskApi_belajar.Models;
using TaskApi_belajar.Repositories.Interface;
using TaskApi_belajar.Services.Interface;

namespace TaskApi_belajar.Services.Data
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly IDatabase _cache;
        public TaskService(ITaskRepository repository, IConnectionMultiplexer redis)
        {
            _repository = repository;
            _cache = redis.GetDatabase();
        }

        public async Task<TaskResponseDTO> CreateTaskAsync(CreateTaskDTO dto)
        {
            var task = new TaskItem { 
                Title = dto.Title,
                IsCompleted = false
            };

            var result = await _repository.CreateAsync(task);
            //Invalidate cache
            try
            {
                await _cache.KeyDeleteAsync("tasks");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis DELETE error: {ex.Message}");
            }

            return new TaskResponseDTO
            {
                Id = result.Id,
                Title = result.Title,
                IsCompleted = result.IsCompleted
            };

        }

        public async Task<List<TaskResponseDTO>> GetTasksAsync()
        {
            var cacheKey = "tasks";

            try
            {
                var cachedData = await _cache.StringGetAsync(cacheKey);

                if (!cachedData.IsNullOrEmpty)
                {
                    return JsonSerializer.Deserialize<List<TaskResponseDTO>>(cachedData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis GET error: {ex.Message}");
                // 🔥 lanjut ke DB
            }

            // fallback ke database
            var tasks = await _repository.GetAllAsync();

            var result = tasks.Select(t => new TaskResponseDTO
            {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted
            }).ToList();

            // 🔥 simpan ke cache (optional, jangan bikin error kalau gagal)
            try
            {
                await _cache.StringSetAsync(
                    cacheKey,
                    JsonSerializer.Serialize(result),
                    TimeSpan.FromMinutes(5) // TTL
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis SET error: {ex.Message}");
            }

            return result;
        }
    }
}
