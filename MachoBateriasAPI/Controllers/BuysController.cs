using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MachoBateriasAPI.Data;
using MachoBateriasAPI.Models;

namespace MachoBateriasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuysController : Controller
    {
        private readonly MachoBateriasAPIContext _context;

        public BuysController(MachoBateriasAPIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Buys>>> GetClient()
        {
            if (_context.Buys == null)
            {
                return NotFound();
            }
            return await _context.Buys.ToListAsync();
        }


        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Buys>> GetBuys(int id)
        {
            if (_context.Buys == null)
            {
                return NotFound();
            }
            var buys = await _context.Buys.FindAsync(id);

            if (buys == null)
            {
                return NotFound();
            }

            return buys;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuys(int id, Buys buys)
        {
            if (id != buys.id)
            {
                return BadRequest();
            }

            _context.Entry(buys).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuysExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Buys>> Postbuys(Buys buys)
        {
            if (_context.Buys == null)
            {
                return Problem("Entity set 'MachoBateriasAPIContext.Buys'  is null.");
            }
            _context.Buys.Add(buys);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBuys", new { id = buys.id }, buys);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuys(int id)
        {
            if (_context.Buys == null)
            {
                return NotFound();
            }
            var buys = await _context.Buys.FindAsync(id);
            if (buys == null)
            {
                return NotFound();
            }

            _context.Buys.Remove(buys);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BuysExists(int id)
        {
            return (_context.Buys?.Any(e => e.id == id)).GetValueOrDefault();
        }

    }
}
