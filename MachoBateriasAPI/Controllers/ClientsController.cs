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
    public class ClientsController : ControllerBase
    {
        private readonly MachoBateriasAPIContext _context;

        public ClientsController(MachoBateriasAPIContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClient()
        {
          var activeClients = await _context.Client
                .Where(c => c.activo)
                .ToListAsync();
            if(activeClients == null || activeClients.Count == 0)
            {
                return NotFound();
            }
            return activeClients;
        }

        [HttpGet("verificarCliente/{identification}")]
        public IActionResult VerificarEmpleado(string identification)
        {
            var existeCedula = _context.Client.Any(c => c.identification == identification);

            return Ok(new { existeCedula = existeCedula });
        }
        [HttpGet("verificarEmail/{email}")]
        public IActionResult VerificarEmail(string email)
        {
            var existeEmail = _context.Client.Any(c => c.email == email);

            return Ok(new { existeEmail = existeEmail });
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
          if (_context.Client == null)
          {
              return NotFound();
          }
            var client = await _context.Client.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // PUT: api/Clients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (id != client.id)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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
        public async Task<IActionResult> PutClientDelete(int id, bool activo)
        {
            try
            {
                // Busca el vehículo por ID
                var client = await _context.Client.FindAsync(id);

                if (client == null)
                {
                    return NotFound(); // El vehículo no fue encontrado
                }

                // Actualiza el valor de "activo"
                client.activo = activo;

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


        // POST: api/Clients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
          if (_context.Client == null)
          {
              return Problem("Entity set 'MachoBateriasAPIContext.Client'  is null.");
          }
            _context.Client.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.id }, client);
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            if (_context.Client == null)
            {
                return NotFound();
            }
            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Client.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(int id)
        {
            return (_context.Client?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
