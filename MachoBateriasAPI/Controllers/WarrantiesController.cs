using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MachoBateriasAPI.Data;
using MachoBateriasAPI.Models;

namespace MachoBateriasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarrantiesController : ControllerBase
    {
        private readonly MachoBateriasAPIContext _context;

        public WarrantiesController(MachoBateriasAPIContext context)
        {
            _context = context;
        }

        // GET: api/Warranties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Warranty>>> GetWarranty()
        {
          if (_context.Warranty == null)
          {
              return NotFound();
          }
            return await _context.Warranty.Include(w => w.product).Include(w => w.vehicle).ToListAsync();
        }

        // GET: api/Warranties/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Warranty>> GetWarranty(int id)
        {
          if (_context.Warranty == null)
          {
              return NotFound();
          }
            var warranty = await _context.Warranty.FindAsync(id);

            if (warranty == null)
            {
                return NotFound();
            }

            return warranty;
        }

        // PUT: api/Warranties/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWarranty(int id, Warranty warranty)
        {
            if (id != warranty.id)
            {
                return BadRequest();
            }

            _context.Entry(warranty).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WarrantyExists(id))
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

        // POST: api/Warranties
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Warranty>> PostWarranty(Warranty warranty)
        {
          if (_context.Warranty == null)
          {
              return Problem("Entity set 'MachoBateriasAPIContext.Warranty'  is null.");
          }
            _context.Warranty.Add(warranty);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWarranty", new { id = warranty.id }, warranty);
        }

        // DELETE: api/Warranties/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarranty(int id)
        {
            if (_context.Warranty == null)
            {
                return NotFound();
            }
            var warranty = await _context.Warranty.FindAsync(id);
            if (warranty == null)
            {
                return NotFound();
            }

            _context.Warranty.Remove(warranty);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WarrantyExists(int id)
        {
            return (_context.Warranty?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
