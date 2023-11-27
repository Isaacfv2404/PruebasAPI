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
    public class SaleProductsController : Controller
    {
        private readonly MachoBateriasAPIContext _context;

        public SaleProductsController(MachoBateriasAPIContext context)
        {
            _context = context;
        }

        // GET: SaleProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleProduct>>> GetSaleProduct()
        {
            if (_context.SaleProduct == null)
            {
                return NotFound();
            }
            return await _context.SaleProduct.ToListAsync();
        }
        // GET: api/Sale/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleProduct>> GetSaleProduct(int id)
        {
            if (_context.SaleProduct == null)
            {
                return NotFound();
            }
            var salep = await _context.SaleProduct.FindAsync(id);

            if (salep == null)
            {
                return NotFound();
            }

            return salep;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSaleProduct(int id, SaleProduct salep)
        {
            if (id != salep.id)
            {
                return BadRequest();
            }

            _context.Entry(salep).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalePExists(id))
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

        // POST: api/Sale

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SaleProduct>> PostSaleProduct(SaleProduct salep)
        {
            if (_context.SaleProduct == null)
            {
                return Problem("Entity set 'MachoBateriasAPIContext.Product'  is null.");
            }
            _context.SaleProduct.Add(salep);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSaleProduct", new { id = salep.id }, salep);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSaleP(int id)
        {
            if (_context.SaleProduct == null)
            {
                return NotFound();
            }
            var salep = await _context.SaleProduct.FindAsync(id);
            if (salep == null)
            {
                return NotFound();
            }

            _context.SaleProduct.Remove(salep);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalePExists(int id)
        {
            return (_context.SaleProduct?.Any(e => e.id == id)).GetValueOrDefault();
        }

        //////////////////consultas
        [HttpGet("{id}/products")]
        public async Task<ActionResult<List<Product>>> GetProductsForSale(int id)
        {
            Console.WriteLine("IDDD HOLAAA");
            try
            {
                // Llama al método del servicio que obtiene los productos para la venta específica
                var productsForSale = await _context.GetProductsForSaleAsync(id);

                if (productsForSale == null || productsForSale.Count == 0)
                {
                    return NotFound("No se encontraron productos para la venta especificada.");
                }

                return Ok(productsForSale);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
