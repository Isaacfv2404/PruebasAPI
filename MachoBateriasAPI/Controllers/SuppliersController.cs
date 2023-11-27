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
    public class SuppliersController:ControllerBase
    {


        private readonly MachoBateriasAPIContext _context;

        public SuppliersController(MachoBateriasAPIContext context)
        {
            _context = context;
        }

        // GET: api/Supplier
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSupplier()
        {
            if (_context.Supplier == null)
            {
                return NotFound();
            }
            return await _context.Supplier.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSu(int id)
        {
            if (_context.Supplier == null)
            {
                return NotFound();
            }
            var sup = await _context.Supplier.FindAsync(id);

            if (sup == null)
            {
                return NotFound();
            }

            return sup;
        }
    }
}
