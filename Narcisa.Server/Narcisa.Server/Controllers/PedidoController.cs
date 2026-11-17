using Microsoft.AspNetCore.Mvc;
using Narcisa.Repositorio.Repositorios;
using Narcisa.Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Narcisa.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;

        public PedidoController(IPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio;
        }

        [HttpGet("usuario/{usuarioId:int}")]
        public async Task<ActionResult<PedidoDTO?>> ObtenerCarrito(int usuarioId)
        {
            var carrito = await _pedidoRepositorio.ObtenerCarritoActivo(usuarioId);
            return Ok(carrito);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PedidoDTO>> VerResumen(int id)
        {
            var pedido = await _pedidoRepositorio.ObtenerResumenPedido(id);
            if (pedido == null)
            {
                return NotFound("El pedido no existe.");
            }

            return Ok(pedido);
        }

        [HttpPost("crear")]
        public async Task<ActionResult<PedidoDTO>> CrearCarrito([FromBody] CrearPedidoDTO dto)
        {
            if (dto.UsuarioId <= 0)
            {
                return BadRequest("El identificador de usuario es obligatorio.");
            }

            var pedido = await _pedidoRepositorio.CrearOObtenerCarrito(dto.UsuarioId);
            return Ok(pedido);
        }

        [HttpPost("{id:int}/agregar")]
        public async Task<ActionResult<PedidoDTO>> AgregarItem(int id, [FromBody] AgregarItemDTO dto)
        {
            var resultado = await _pedidoRepositorio.AgregarItem(id, dto.ProductoId, dto.Cantidad);
            if (!resultado.Exito)
            {
                return BadRequest(resultado.Error);
            }

            return Ok(resultado.Pedido);
        }

        [HttpPut("{id:int}/item/{productoId:int}")]
        public async Task<ActionResult<PedidoDTO>> ActualizarCantidad(int id, int productoId, [FromQuery] int cantidad)
        {
            var resultado = await _pedidoRepositorio.ActualizarCantidadItem(id, productoId, cantidad);
            if (!resultado.Exito)
            {
                return BadRequest(resultado.Error);
            }

            return Ok(resultado.Pedido);
        }

        [HttpDelete("{id:int}/item/{productoId:int}")]
        public async Task<ActionResult<PedidoDTO>> EliminarItem(int id, int productoId)
        {
            var resultado = await _pedidoRepositorio.EliminarItem(id, productoId);
            if (!resultado.Exito)
            {
                return BadRequest(resultado.Error);
            }

            return Ok(resultado.Pedido);
        }

        [HttpPut("{id:int}/direccion")]
        public async Task<ActionResult<PedidoDTO>> IngresarDireccion(int id, [FromBody] ConfirmarPedidoDTO dto)
        {
            var resultado = await _pedidoRepositorio.ActualizarDireccion(id, dto.Direccion);
            if (!resultado.Exito)
            {
                return BadRequest(resultado.Error);
            }

            return Ok(resultado.Pedido);
        }

        [HttpPost("{id:int}/confirmar")]
        public async Task<ActionResult<PedidoDTO>> ConfirmarPedido(int id)
        {
            var resultado = await _pedidoRepositorio.ConfirmarPedido(id);
            if (!resultado.Exito)
            {
                return BadRequest(resultado.Error);
            }

            return Ok(resultado.Pedido);
        }

        [HttpGet("usuario/{usuarioId:int}/historial")]
        public async Task<ActionResult<List<PedidoDTO>>> ObtenerHistorialUsuario(int usuarioId)
        {
            var historial = await _pedidoRepositorio.ObtenerHistorialUsuario(usuarioId);
            return Ok(historial);
        }

        [HttpGet("admin/todos")]
        public async Task<ActionResult<List<PedidoDTO>>> ObtenerTodosPedidos()
        {
            var pedidos = await _pedidoRepositorio.ObtenerTodosPedidos();
            return Ok(pedidos);
        }

        [HttpPut("{id:int}/estado")]
        public async Task<ActionResult<bool>> ActualizarEstadoPedido(int id, [FromQuery] string? nuevoEstado = null, [FromQuery] string? estado = null)
        {
            var estadoFinal = !string.IsNullOrWhiteSpace(nuevoEstado) ? nuevoEstado : estado;
            if (string.IsNullOrWhiteSpace(estadoFinal))
            {
                return BadRequest("El nuevo estado es obligatorio.");
            }

            var actualizado = await _pedidoRepositorio.ActualizarEstadoPedido(id, estadoFinal.Trim());
            if (!actualizado)
            {
                return NotFound("No se encontró el pedido.");
            }

            return Ok(true);
        }
    }
}
