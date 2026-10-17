using Microsoft.AspNetCore.Mvc;
using Narcisa.Repositorio.Repositorios;
using Narcisa.Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Narcisa.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoRepositorio _repositorio;

        public ProductosController(IProductoRepositorio repositorio)
        {
            _repositorio = repositorio;
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
    }
}
