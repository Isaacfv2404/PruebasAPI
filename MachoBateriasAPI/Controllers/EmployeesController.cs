using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MachoBateriasAPI.Data;
using MachoBateriasAPI.Models;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MachoBateriasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly MachoBateriasAPIContext _context;

        public EmployeesController(MachoBateriasAPIContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
            var activeEmployee = await _context.Employee
                                                  .Where(e => e.activo)
                                                  .ToListAsync();

            if (activeEmployee == null || activeEmployee.Count == 0)
            {
                return NotFound();
            }

            return activeEmployee;
        }

        [HttpGet("verificarEmpleado/{identification}")]
        public IActionResult VerificarEmpleado(string identification)
        {
            var existeCedula = _context.Employee.Any(e => e.identification == identification);

            return Ok(new { existeCedula = existeCedula });
        }
        [HttpGet("verificarEmail/{email}")]
        public IActionResult VerificarEmail(string email)
        {
            var existeEmail = _context.Employee.Any(e => e.email == email);

            return Ok(new { existeEmail = existeEmail });
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
          if (_context.Employee == null)
          {
              return NotFound();
          }
            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        [HttpPut("delete/{id}")]
        public async Task<IActionResult> PutEmployeeDelete(int id, bool activo)
        {
            try
            {
                // Busca el vehículo por ID
                var employee = await _context.Employee.FindAsync(id);

                if (employee == null)
                {
                    return NotFound(); // El vehículo no fue encontrado
                }

                // Actualiza el valor de "activo"
                employee.activo = activo;

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

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.id)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
          if (_context.Employee == null)
          {
              return Problem("Entity set 'MachoBateriasAPIContext.Employee'  is null.");
          }
            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (_context.Employee == null)
            {
                return NotFound();
            }
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return (_context.Employee?.Any(e => e.id == id)).GetValueOrDefault();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var employee = await _context.Employee.FirstOrDefaultAsync(e => e.email == loginModel.Email);

            if (employee == null || !VerifyPassword(employee.password, loginModel.Password))
            {
                return Unauthorized(new { message = "Credenciales inválidas" });
            }

            // Crear un token JWT
            var token = CreateJwtToken(employee);

            return Ok(new { token = token });
        }

        private bool VerifyPassword(string hashedPassword, string inputPassword)
        {
          
                // Comprobar si la sal es nula
                if (string.IsNullOrEmpty(hashedPassword) || hashedPassword != inputPassword)
                {
                    Console.WriteLine($"Incorrect password: {hashedPassword}");
                    Console.WriteLine("La sal es nula o no es válida.");
                    return false;
                }
            return true;
        }

        private string CreateJwtToken(Employee employee)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("tu-clave-secreta"); // Reemplaza esto con tu clave secreta
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, employee.id.ToString()),
            new Claim(ClaimTypes.Email, employee.email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        

    }
}
