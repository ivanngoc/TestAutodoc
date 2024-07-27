using DAL.Contracts;
using DAL.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repo = DAL.Contracts.IRepository12<DAL.EFCore.Meta, uint>;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachController : ControllerBase
    {
        private readonly Repo _context;

        public AttachController(Repo repo)
        {
            _context = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Meta>>> GetMetas()
        {
            return Ok(await _context.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Meta>> GetMeta(uint id)
        {
            var meta = await _context.GetSingleAsync(id);

            if (meta == null)
            {
                return NotFound();
            }

            return Ok(meta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeta(uint id, Meta meta)
        {
            if (id != meta.MetaId)
            {
                return BadRequest();
            }
            try
            {
                await _context.UpdateAsync(meta);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MetaExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Meta>> PostMeta(Meta meta)
        {
            await _context.CreateAsync(meta);
            return CreatedAtAction("GetMeta", new { id = meta.MetaId }, meta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeta(uint id)
        {
            var e = await _context.DeleteAsync(id);
            if (e is null) return NotFound();
            return NoContent();
        }

        private bool MetaExists(uint id)
        {
            return _context.Exists(id);
        }
    }
}
