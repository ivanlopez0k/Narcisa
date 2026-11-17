using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Narcisa.Repositorio.Repositorios;
using Narcisa.Shared.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Narcisa.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProyectosController : ControllerBase
    {
        private readonly IProyectoRepositorio _proyectoRepositorio;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProyectosController(IProyectoRepositorio proyectoRepositorio, IWebHostEnvironment webHostEnvironment)
        {
            _proyectoRepositorio = proyectoRepositorio;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("usuario/{usuarioId:int}")]
        public async Task<ActionResult<List<ProyectoDTO>>> ObtenerProyectos(int usuarioId)
        {
            var proyectos = await _proyectoRepositorio.ObtenerPorUsuario(usuarioId);
            return Ok(proyectos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProyectoDTO>> ObtenerProyecto(int id, [FromQuery] int usuarioId)
        {
            var proyecto = await _proyectoRepositorio.ObtenerDetalle(id, usuarioId);
            if (proyecto == null)
            {
                return NotFound("El proyecto no existe.");
            }
            return Ok(proyecto);
        }

        [HttpPost]
        public async Task<ActionResult<ProyectoDTO>> GuardarProyecto([FromBody] GuardarProyectoDTO dto, [FromQuery] int usuarioId)
        {
            if (usuarioId <= 0)
            {
                return BadRequest("El identificador de usuario es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(dto.Nombre))
            {
                return BadRequest("El nombre del proyecto es obligatorio.");
            }

            var proyecto = await _proyectoRepositorio.GuardarProyecto(dto, usuarioId);
            return Ok(proyecto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarProyecto(int id, [FromQuery] int usuarioId)
        {
            var eliminado = await _proyectoRepositorio.EliminarProyecto(id, usuarioId);
            if (!eliminado)
            {
                return NotFound("El proyecto no existe o no pertenece al usuario.");
            }
            return Ok();
        }

        [HttpPost("upload-ambiente")]
        public async Task<IActionResult> UploadAmbiente(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No se proporcionó ningún archivo.");
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest("Formato de imagen no soportado. Formatos válidos: JPG, PNG, WEBP.");
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "ambientes");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"/uploads/ambientes/{fileName}";
            return Ok(new { url = fileUrl });
        }
    }
}
