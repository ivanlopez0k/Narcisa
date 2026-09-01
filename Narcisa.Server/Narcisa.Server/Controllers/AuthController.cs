using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Narcisa.BD.Datos;
using Narcisa.BD.Datos.Entity;
using Narcisa.Server.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Narcisa.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.email) ||
                string.IsNullOrWhiteSpace(dto.password) ||
                string.IsNullOrWhiteSpace(dto.Nombre))
            {
                return BadRequest("Nombre, email y contraseña son obligatorios.");
            }

            bool existe = await _context.Usuario
                .AnyAsync(u => u.email == dto.email);

            if (existe)
            {
                return BadRequest("Ya existe un usuario con ese email.");
            }

            Usuario usuario = new Usuario
            {
                Nombre = dto.Nombre,
                email = dto.email,
                password = dto.password,
                rol = string.IsNullOrWhiteSpace(dto.rol) ? "Cliente" : dto.rol,
                createdAt = DateTime.UtcNow
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                usuario.Id,
                usuario.Nombre,
                usuario.email,
                usuario.rol
            });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(u =>
                    u.email == dto.email &&
                    u.password == dto.password);

            if (usuario == null)
            {
                return Unauthorized("Email o contraseña incorrectos.");
            }

            return Ok(new
            {
                usuario.Id,
                usuario.Nombre,
                usuario.email,
                usuario.rol
            });
        }
    }
}
