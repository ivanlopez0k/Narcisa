using Microsoft.AspNetCore.Mvc;
using Narcisa.Repositorio.Repositorios;
using Narcisa.Shared.DTO;
using System.Threading.Tasks;

namespace Narcisa.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public AuthController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UsuarioDTO>> Register([FromBody] RegisterDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password) ||
                string.IsNullOrWhiteSpace(dto.Nombre))
            {
                return BadRequest("Nombre, email y contraseña son obligatorios.");
            }

            var existente = await _usuarioRepositorio.ObtenerPorEmail(dto.Email);
            if (existente != null)
            {
                return BadRequest("Ya existe un usuario registrado con ese email.");
            }

            var usuario = await _usuarioRepositorio.Registrar(dto);

            return Ok(new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.email,
                Rol = usuario.rol
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDTO>> Login([FromBody] LoginDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Email y contraseña son obligatorios.");
            }

            var usuario = await _usuarioRepositorio.ValidarCredenciales(dto.Email, dto.Password);
            if (usuario == null)
            {
                return Unauthorized("Email o contraseña incorrectos.");
            }

            return Ok(new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.email,
                Rol = usuario.rol
            });
        }
    }
}
