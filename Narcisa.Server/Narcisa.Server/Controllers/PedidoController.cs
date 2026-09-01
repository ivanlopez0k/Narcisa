using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Narcisa.BD.Datos;
using Narcisa.BD.Datos.Entity;
using Narcisa.Server.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Narcisa.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private const string EstadoCarrito = "Carrito";
        private const string EstadoPendientePago = "Pendiente de pago";

        private readonly AppDbContext _context;

        public PedidoController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost("crear")]
        public async Task<ActionResult<PedidoDTO>> CrearCarrito(CrearPedidoDTO dto)
        {
            var usuario = await _context.Usuario.FindAsync(dto.UsuarioId);
            if (usuario == null)
            {
                return BadRequest("El usuario no existe. El cliente debe estar autenticado.");
            }

            var pedido = new Pedido
            {
                UsuarioId = usuario.Id,
                Estado = EstadoCarrito,
                Direccion = "",
                Total = 0,
                CreatedAt = DateTime.UtcNow
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return Ok(new PedidoDTO
            {
                Id = pedido.Id,
                UsuarioId = pedido.UsuarioId,
                Estado = pedido.Estado,
                Total = pedido.Total,
                Items = new List<ItemPedidoDTO>()
            });
        }

        [HttpPost("{id}/agregar")]
        public async Task<ActionResult<PedidoDTO>> AgregarItem(int id, AgregarItemDTO dto)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound("El pedido no existe.");
            }

            if (pedido.Estado != EstadoCarrito)
            {
                return BadRequest("El pedido ya fue confirmado y no puede modificarse.");
            }

            if (dto.Cantidad <= 0)
            {
                return BadRequest("La cantidad debe ser mayor a cero.");
            }

            var producto = await _context.Productos.FindAsync(dto.ProductoId);
            if (producto == null || !producto.Activo)
            {
                return BadRequest("El producto no existe o no está disponible.");
            }


            var existeItem = await _context.DetallesPedidos
                .FirstOrDefaultAsync(dp => dp.PedidoId == pedido.Id && dp.ProductoId == dto.ProductoId);

            int cantidadPedida = (existeItem?.Cantidad ?? 0) + dto.Cantidad;
            if (cantidadPedida > producto.Stock)
            {
                return BadRequest($"No hay suficiente stock. Disponible: {producto.Stock}.");
            }

            if (existeItem != null)
            {
                existeItem.Cantidad = cantidadPedida;
            }
            else
            {
                _context.DetallesPedidos.Add(new DetallePedido
                {
                    PedidoId = pedido.Id,
                    ProductoId = producto.Id,
                    Cantidad = dto.Cantidad,
                    Precio = producto.Precio
                });
            }

            await RecalcularTotal(pedido);
            await _context.SaveChangesAsync();

            return Ok(await ObtenerResumen(pedido.Id));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoDTO>> VerResumen(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound("El pedido no existe.");
            }

            return Ok(await ObtenerResumen(pedido.Id));
        }


        [HttpPut("{id}/direccion")]
        public async Task<ActionResult<PedidoDTO>> IngresarDireccion(int id, ConfirmarPedidoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Direccion))
            {
                return BadRequest("Debe ingresar una dirección de entrega válida.");
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound("El pedido no existe.");
            }

            if (pedido.Estado != EstadoCarrito)
            {
                return BadRequest("El pedido ya fue confirmado y no puede modificarse.");
            }

            pedido.Direccion = dto.Direccion;
            await _context.SaveChangesAsync();

            return Ok(await ObtenerResumen(pedido.Id));
        }


        [HttpPost("{id}/confirmar")]
        public async Task<ActionResult<PedidoDTO>> ConfirmarPedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound("El pedido no existe.");
            }

            bool tieneItems = await _context.DetallesPedidos
                .AnyAsync(dp => dp.PedidoId == pedido.Id);
            if (!tieneItems)
            {
                return BadRequest("El carrito está vacío. Agregue productos antes de confirmar.");
            }

            if (string.IsNullOrWhiteSpace(pedido.Direccion))
            {
                return BadRequest("Faltan los datos de entrega. Ingrese la dirección antes de confirmar.");
            }

            pedido.Estado = EstadoPendientePago;
            await RecalcularTotal(pedido);
            await _context.SaveChangesAsync();

            return Ok(await ObtenerResumen(pedido.Id));
        }

        private async Task RecalcularTotal(Pedido pedido)
        {
            pedido.Total = await _context.DetallesPedidos
                .Where(dp => dp.PedidoId == pedido.Id)
                .SumAsync(dp => dp.Cantidad * dp.Precio);
        }

        private async Task<PedidoDTO> ObtenerResumen(int pedidoId)
        {
            var pedido = await _context.Pedidos.FindAsync(pedidoId);

            var items = await _context.DetallesPedidos
                .Include(dp => dp.Producto)
                .Where(dp => dp.PedidoId == pedidoId)
                .Select(dp => new ItemPedidoDTO
                {
                    ProductoId = dp.ProductoId,
                    ProductoNombre = dp.Producto.Nombre,
                    Cantidad = dp.Cantidad,
                    PrecioUnitario = dp.Precio,
                    Subtotal = dp.Cantidad * dp.Precio
                })
                .ToListAsync();

            await RecalcularTotal(pedido);

            return new PedidoDTO
            {
                Id = pedido.Id,
                UsuarioId = pedido.UsuarioId,
                Estado = pedido.Estado,
                Direccion = pedido.Direccion,
                Total = pedido.Total,
                Items = items
            };
        }
    }
}
