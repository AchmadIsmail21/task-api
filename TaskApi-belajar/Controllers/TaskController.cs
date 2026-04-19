using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskApi_belajar.DTOs;
using TaskApi_belajar.Services.Interface;

namespace TaskApi_belajar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _service;
        public TaskController(ITaskService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetTasksAsync();
            return Ok(new
            {
                status = HttpStatusCode.OK,
                message = result
            });
        }
        [HttpPost]
        public async Task<IActionResult> Post(CreateTaskDTO dto)
        {
            var result = await _service.CreateTaskAsync(dto);
            return Ok(new
            {
                status = HttpStatusCode.OK,
                message = result
            });
        }
    }
}
