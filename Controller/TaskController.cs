using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using program.Data;
using TaskManager.Model;

namespace TaskManager
{
    [ApiController]
    [Route("api/[controller]s")]
    public class TaskController : ControllerBase
    {
        private readonly PgDbContext _context; // List<TaskResponseDto> explicitly states that _task is a list.

        public TaskController(PgDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllTask() => Ok(await _context.Tasks.ToListAsync()); //Used Lambda Expression
        // public ActionResult<List<TaskResponseDto>> GetAllTask() => Ok(_task); //Used Lambda Expression

        [HttpGet("{id:int}")] 
        public async Task<ActionResult<TaskModel>> GetTaskById([FromRoute] int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
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
        public async Task<IActionResult> CreateTask([FromBody] TaskRequestDto taskRequest)
        {
            try
            {
                if (await _context.Tasks.AnyAsync(e => e.Title == taskRequest.Title))
                {
                    return BadRequest("Task already exists.");
                }

                var task = new TaskModel
                {
                    Title = taskRequest.Title,
                    IsCompleted = taskRequest.IsCompleted
                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                return StatusCode(201, task);
            }
            catch (Exception error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpPut("{id:Range(1, 19)}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateTask([FromRoute] int id, [FromBody] UpdateTaskDto taskUpdate)
        {
            try
            {
                var updateTask = await _context.Tasks.FindAsync(id);
                if (updateTask == null) return NotFound();
                
                updateTask.Title = taskUpdate.Title;
                updateTask.IsCompleted = taskUpdate?.IsCompleted ?? updateTask.IsCompleted;

                _context.Tasks.Update(updateTask);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
            
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteTask([FromRoute] int id)
        {
            try
            {
                var deleteBook = await _context.Tasks.FindAsync(id);
                if (deleteBook == null) return NotFound();
                
                _context.Tasks.Remove(deleteBook);
                await _context.SaveChangesAsync();
                
                return NoContent();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
        
        // ActionResult<List<TaskResponseDto>>

        [HttpGet("search")]
        public async Task<IActionResult> SearchByParam([FromQuery] bool? isCompleted, [FromQuery] string? title)
        {
            try
            {
               
                var query = _context.Tasks.AsQueryable();
                var sql = query.ToQueryString();
                Console.WriteLine(sql);
                
                // if (!bool.TryParse(title, out var res))
                // {
                //     query = query.Where(i => i.IsCompleted.Equals(isCompleted));
                // }
                
                if (isCompleted.HasValue)
                {
                    query = query.Where(i => i.IsCompleted.Equals(isCompleted.Value));
                }
                
                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(p => p.Title.ToLower().Contains(title.ToLower()));
                }
                
                var tasks = await query.ToListAsync();
                return Ok(tasks);
                
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}

