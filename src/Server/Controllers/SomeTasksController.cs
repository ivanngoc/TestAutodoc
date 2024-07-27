using DAL.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SomeTasksController : ControllerBase
    {
        private readonly IDb _context;

        public SomeTasksController(IDb context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SomeTask>>> GetTasks()
        {
            return await _context.Tasks.Include(x => x.Files).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SomeTask>> GetSomeTask(uint id)
        {
            var someTask = await _context.Tasks.FindAsync(id);

            if (someTask == null)
            {
                return NotFound();
            }

            return Ok(someTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSomeTask(uint id, SomeTask someTask)
        {
            if (id != someTask.SomeTaskId)
            {
                return BadRequest();
            }

            _context.AsContext().Entry(someTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SomeTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        /// <summary>
        /// Создать задачу с данными о файлах
        /// </summary>
        /// <param name="someTask"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<SomeTask>> PostSomeTask([FromBody] SomeTask someTask)
        {
            if (someTask.SomeTaskId != default) throw new FormatException("Must be 0. Explicit intention to create");
            _context.Tasks.Add(someTask);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetSomeTask", new { id = someTask.SomeTaskId }, someTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSomeTask(uint id)
        {
            var someTask = await _context.Tasks.FindAsync(id);
            if (someTask == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(someTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SomeTaskExists(uint id)
        {
            return _context.Tasks.Any(e => e.SomeTaskId == id);
        }
    }
}
