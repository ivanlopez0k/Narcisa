using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Narcisa.Repositorio.Repositorios;
using Narcisa.Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Narcisa.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsesoramientoController : ControllerBase
    {
        private readonly IAsesoramientoRepositorio _asesoramientoRepositorio;

        public AsesoramientoController(IAsesoramientoRepositorio asesoramientoRepositorio)
        {
            _asesoramientoRepositorio = asesoramientoRepositorio;
        }

        [HttpGet("usuario/{usuarioId:int}")]
        public async Task<ActionResult<List<AsesoramientoDTO>>> ObtenerPorUsuario(int usuarioId)
        {
            var asesoramientos = await _asesoramientoRepositorio.ObtenerPorUsuario(usuarioId);
            return Ok(asesoramientos);
        }

        [HttpGet]
        public async Task<ActionResult<List<AsesoramientoDTO>>> ObtenerTodos()
        {
            var asesoramientos = await _asesoramientoRepositorio.ObtenerTodos();
            return Ok(asesoramientos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AsesoramientoDTO>> ObtenerPorId(int id)
        {
            var asesoramiento = await _asesoramientoRepositorio.ObtenerPorId(id);
            if (asesoramiento == null)
            {
                return NotFound("La solicitud de asesoramiento no fue encontrada.");
            }

            return Ok(asesoramiento);
        }

        [HttpPost]
        public async Task<ActionResult<AsesoramientoDTO>> Crear(
            [FromBody] CrearAsesoramientoDTO dto,
            [FromQuery] int? usuarioId = null)
        {
            var resolvedUsuarioId = usuarioId ?? dto.UsuarioId;
            if (resolvedUsuarioId <= 0)
            {
                return BadRequest("El identificador de usuario es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(dto.Descripcion))
            {
                return BadRequest("La descripción de lo que buscás es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(dto.Disponibilidad))
            {
                return BadRequest("La disponibilidad horaria es obligatoria.");
            }

            var asesoramiento = await _asesoramientoRepositorio.Crear(dto, resolvedUsuarioId);
            return Ok(asesoramiento);
        }

        [HttpPut("{id:int}/estado")]
        public async Task<ActionResult<bool>> ActualizarEstado(
            int id,
            [FromQuery] string? nuevoEstado = null,
            [FromQuery] string? estado = null,
            [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ActualizarEstadoAsesoramientoDTO? dto = null)
        {
            var estadoFinal = !string.IsNullOrWhiteSpace(nuevoEstado)
                ? nuevoEstado
                : !string.IsNullOrWhiteSpace(estado)
                    ? estado
                    : dto?.Estado;

            if (string.IsNullOrWhiteSpace(estadoFinal))
            {
                return BadRequest("El nuevo estado es obligatorio.");
            }

            var actualizado = await _asesoramientoRepositorio.ActualizarEstado(id, estadoFinal.Trim());
            if (!actualizado)
            {
                return NotFound("No se encontró la solicitud de asesoramiento.");
            }

            return Ok(true);
        }
    }
}
