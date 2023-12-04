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
    public class VehiclesController : ControllerBase
    {
        private readonly MachoBateriasAPIContext _context;

        public VehiclesController(MachoBateriasAPIContext context)
        {
            _context = context;
        }

        // GET: api/Vehicles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetActiveVehicles()
        {
            var activeVehicles = await _context.Vehicle
                                               .Where(v => v.activo)
                                               .ToListAsync();

            if (activeVehicles == null || activeVehicles.Count == 0)
            {
                return NotFound();
            }

            return activeVehicles;
        }


        // GET: api/Vehicles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetVehicle(int id)
        {
          if (_context.Vehicle == null)
          {
              return NotFound();
          }
            var vehicle = await _context.Vehicle.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return vehicle;
        }

        // PUT: api/Vehicles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle(int id, Vehicle vehicle)
        {
            if (id != vehicle.id)
            {
                return BadRequest();
            }

            _context.Entry(vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
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

        [HttpPut("delete/{id}")]
        public async Task<IActionResult> PutVehicleDelete(int id, bool activo)
        {
            try
            {
                // Busca el vehículo por ID
                var vehicle = await _context.Vehicle.FindAsync(id);

                if (vehicle == null)
                {
                    return NotFound(); // El vehículo no fue encontrado
                }

                // Actualiza el valor de "activo"
                vehicle.activo = activo;

                // Guarda los cambios en la base de datos
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Registra la excepción para diagnóstico
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Error interno del servidor");
            }
        }


        // POST: api/Vehicles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Vehicle>> PostVehicle(Vehicle vehicle)
        {
          if (_context.Vehicle == null)
          {
              return Problem("Entity set 'MachoBateriasAPIContext.Vehicle'  is null.");
          }
            _context.Vehicle.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicle", new { id = vehicle.id }, vehicle);
        }



        [HttpGet("verificarPlaca/{plate}")]
        public IActionResult VerificarPlaca(string plate)
        {
            var existePlaca = _context.Vehicle.Any(v => v.plate == plate);

            return Ok(new { ExistePlaca = existePlaca });
        }

        // DELETE: api/Vehicles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            if (_context.Vehicle == null)
            {
                return NotFound();
            }
            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehicleExists(int id)
        {
            return (_context.Vehicle?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
