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
    public class ProductosController : ControllerBase
    {
        private readonly IProductoRepositorio _repositorio;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductosController(IProductoRepositorio repositorio, IWebHostEnvironment webHostEnvironment)
        {
            _repositorio = repositorio;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductoDTO>>> GetProductos()
        {
            var productos = await _repositorio.ObtenerCatalogoActivo();
            return Ok(productos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductoDTO>> GetProducto(int id)
        {
            var producto = await _repositorio.ObtenerDetalle(id);
            if (producto == null)
            {
                return NotFound("El producto no existe o no está disponible.");
            }

            return Ok(producto);
        }

        [HttpGet("admin")]
        public async Task<ActionResult<List<ProductoDTO>>> GetProductosAdmin()
        {
            var productos = await _repositorio.ObtenerTodosAdmin();
            return Ok(productos);
        }

        [HttpPost]
        public async Task<ActionResult<ProductoDTO>> CrearProducto([FromBody] ProductoDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var guardado = await _repositorio.GuardarProducto(dto);
            return Ok(guardado);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductoDTO>> ActualizarProducto(int id, [FromBody] ProductoDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dto.Id = id;
            var guardado = await _repositorio.GuardarProducto(dto);
            return Ok(guardado);
        }

        [HttpPut("{id:int}/activo")]
        public async Task<ActionResult<bool>> CambiarEstadoActivo(int id, [FromQuery] bool activo)
        {
            var modificado = await _repositorio.CambiarEstadoActivo(id, activo);
            if (!modificado)
            {
                return NotFound("Producto no encontrado.");
            }

            return Ok(true);
        }

        [HttpPost("upload-imagen")]
        public async Task<IActionResult> UploadImagen(IFormFile file)
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

            var webRoot = _webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsFolder = Path.Combine(webRoot, "uploads", "productos");
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

            var fileUrl = $"/uploads/productos/{fileName}";
            return Ok(new { url = fileUrl });
        }
    }
}
