using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager
{
    [ApiController]
    [Route("api/[controller]s")]
    public class TaskController : ControllerBase
    {
        private static List<TaskResponseDto> _task = new ();
        
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<List<TaskResponseDto>> GetAllTask() => Ok(_task); //Used Lambda Expression

        [HttpGet("{id:int}")] 
        public IActionResult GetTaskById(int id)
        {
            try
            {
                var task = _task.FirstOrDefault(t => t.Id == id);
                if (task == null) return NotFound();
                return Ok(task);
            }
            catch (Exception error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public IActionResult CreateTask([FromBody] TaskRequestDto taskRequest)
        {
            try
            {
                if (taskRequest == null!) return BadRequest();

                var createTask = new TaskResponseDto()
                {
                    Id = _task.Count + 1,
                    Title = taskRequest.Title,
                    IsCompleted = taskRequest.IsCompleted
                };

                _task.Add(createTask);

                var response = new TaskRequestDto()
                {
                    Title = createTask.Title,
                    IsCompleted = createTask.IsCompleted
                };

                return Created(nameof(GetTaskById), response);
            }
            catch (NetworkInformationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        public IActionResult UpdateTask(int id, [FromBody] UpdateTaskDto taskUpdate)
        {
            try
            {
                var updateTask = _task.FirstOrDefault(t => t.Id == id);
                if (updateTask == null) return NotFound();

                updateTask.Id = id;
                updateTask.Title = taskUpdate.Title;
                updateTask.IsCompleted = taskUpdate?.IsCompleted ?? updateTask.IsCompleted;
                
                return NoContent();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
            
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        public IActionResult DeleteTask(int id)
        {
            try
            {
                var deleteBook = _task.FirstOrDefault(t => t.Id == id);
                if (deleteBook == null) return NotFound();

                _task.Remove(deleteBook);
                return NoContent();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("search")]
        public ActionResult<List<TaskResponseDto>> SearchByParam([FromQuery] bool? isCompleted)
        {
            try
            {
                var filteredTask = _task.Where(e => e.IsCompleted.Equals(isCompleted)).ToList();
                if (!filteredTask.Any()) return NotFound();

                return Ok(filteredTask);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}

