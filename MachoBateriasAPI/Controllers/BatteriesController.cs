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
    public class BatteriesController : ControllerBase
    {
        private readonly MachoBateriasAPIContext _context;

        public BatteriesController(MachoBateriasAPIContext context)
        {
            _context = context;
        }

        // GET: api/Batteries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Battery>>> GetBattery()
        {
          if (_context.Battery == null)
          {
              return NotFound();
          }
            return await _context.Battery.ToListAsync();
        }

        // GET: api/Batteries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Battery>> GetBattery(int id)
        {
          if (_context.Battery == null)
          {
              return NotFound();
          }
            var battery = await _context.Battery.FindAsync(id);

            if (battery == null)
            {
                return NotFound();
            }

            return battery;
        }

        // PUT: api/Batteries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBattery(int id, Battery battery)
        {
            if (id != battery.id)
            {
                return BadRequest();
            }

            _context.Entry(battery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BatteryExists(id))
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

        // POST: api/Batteries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Battery>> PostBattery(Battery battery)
        {
          if (_context.Battery == null)
          {
              return Problem("Entity set 'MachoBateriasAPIContext.Battery'  is null.");
          }
            _context.Battery.Add(battery);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBattery", new { id = battery.id }, battery);
        }

        // DELETE: api/Batteries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBattery(int id)
        {
            if (_context.Battery == null)
            {
                return NotFound();
            }
            var battery = await _context.Battery.FindAsync(id);
            if (battery == null)
            {
                return NotFound();
            }

            _context.Battery.Remove(battery);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BatteryExists(int id)
        {
            return (_context.Battery?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
