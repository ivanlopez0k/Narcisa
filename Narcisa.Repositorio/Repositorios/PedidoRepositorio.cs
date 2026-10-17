using Microsoft.EntityFrameworkCore;
using Narcisa.BD.Datos;
using Narcisa.BD.Datos.Entity;
using Narcisa.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Narcisa.Repositorio.Repositorios
{
    public class PedidoRepositorio : Repositorio<Pedido>, IPedidoRepositorio
    {
        private const string EstadoCarrito = "Carrito";
        private const string EstadoPendientePago = "Pendiente de pago";

        public PedidoRepositorio(AppDbContext context) : base(context)
        {
        }

        public async Task<PedidoDTO?> ObtenerCarritoActivo(int usuarioId)
        {
            var pedido = await context.Pedidos
                .Where(p => p.UsuarioId == usuarioId && p.Estado == EstadoCarrito)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync();

            if (pedido == null)
            {
                return null;
            }

            return await ObtenerResumenPedido(pedido.Id);
        }

        public async Task<PedidoDTO?> ObtenerResumenPedido(int pedidoId)
        {
            var pedido = await context.Pedidos.FindAsync(pedidoId);
            if (pedido == null)
            {
                return null;
            }

            var items = await context.DetallesPedidos
                .Include(dp => dp.Producto)
                .Where(dp => dp.PedidoId == pedidoId)
                .Select(dp => new ItemPedidoDTO
                {
                    ProductoId = dp.ProductoId,
                    ProductoNombre = dp.Producto.Nombre,
                    ImagenUrl = dp.Producto.ImagenUrl,
                    Cantidad = dp.Cantidad,
                    PrecioUnitario = dp.Precio,
                    Subtotal = dp.Cantidad * dp.Precio
                })
                .ToListAsync();

            pedido.Total = items.Sum(i => i.Subtotal);

            return new PedidoDTO
            {
                Id = pedido.Id,
                UsuarioId = pedido.UsuarioId,
                Estado = pedido.Estado,
                Direccion = pedido.Direccion ?? "",
                Total = pedido.Total,
                CreatedAt = pedido.CreatedAt,
                Items = items
            };
        }

        public async Task<PedidoDTO> CrearOObtenerCarrito(int usuarioId)
        {
            var existente = await ObtenerCarritoActivo(usuarioId);
            if (existente != null)
            {
                return existente;
            }

            var pedido = new Pedido
            {
                UsuarioId = usuarioId,
                Estado = EstadoCarrito,
                Direccion = "",
                Total = 0,
                CreatedAt = DateTime.UtcNow
            };

            context.Pedidos.Add(pedido);
            await context.SaveChangesAsync();

            return new PedidoDTO
            {
                Id = pedido.Id,
                UsuarioId = pedido.UsuarioId,
                Estado = pedido.Estado,
                Direccion = "",
                Total = 0,
                CreatedAt = pedido.CreatedAt,
                Items = new List<ItemPedidoDTO>()
            };
        }

        public async Task<(bool Exito, string? Error, PedidoDTO? Pedido)> AgregarItem(int pedidoId, int productoId, int cantidad)
        {
            if (cantidad <= 0)
            {
                return (false, "La cantidad debe ser mayor a cero.", null);
            }

            var pedido = await context.Pedidos.FindAsync(pedidoId);
            if (pedido == null)
            {
                return (false, "El pedido no existe.", null);
            }

            if (pedido.Estado != EstadoCarrito)
            {
                return (false, "El pedido ya fue confirmado y no puede modificarse.", null);
            }

            var producto = await context.Productos.FindAsync(productoId);
            if (producto == null || !producto.Activo)
            {
                return (false, "El producto no existe o no está disponible.", null);
            }

            var detalle = await context.DetallesPedidos
                .FirstOrDefaultAsync(dp => dp.PedidoId == pedidoId && dp.ProductoId == productoId);

            int cantidadFinal = (detalle?.Cantidad ?? 0) + cantidad;
            if (cantidadFinal > producto.Stock)
            {
                return (false, $"Stock insuficiente. Disponible: {producto.Stock}.", null);
            }

            if (detalle != null)
            {
                detalle.Cantidad = cantidadFinal;
                detalle.Precio = producto.Precio;
            }
            else
            {
                context.DetallesPedidos.Add(new DetallePedido
                {
                    PedidoId = pedido.Id,
                    ProductoId = producto.Id,
                    Cantidad = cantidad,
                    Precio = producto.Precio
                });
            }

            await context.SaveChangesAsync();
            await RecalcularTotal(pedido);
            await context.SaveChangesAsync();

            var resumen = await ObtenerResumenPedido(pedido.Id);
            return (true, null, resumen);
        }

        public async Task<(bool Exito, string? Error, PedidoDTO? Pedido)> ActualizarCantidadItem(int pedidoId, int productoId, int nuevaCantidad)
        {
            var pedido = await context.Pedidos.FindAsync(pedidoId);
            if (pedido == null)
            {
                return (false, "El pedido no existe.", null);
            }

            if (pedido.Estado != EstadoCarrito)
            {
                return (false, "El pedido ya fue confirmado y no puede modificarse.", null);
            }

            var detalle = await context.DetallesPedidos
                .FirstOrDefaultAsync(dp => dp.PedidoId == pedidoId && dp.ProductoId == productoId);

            if (detalle == null)
            {
                return (false, "El ítem no está en el carrito.", null);
            }

            if (nuevaCantidad <= 0)
            {
                context.DetallesPedidos.Remove(detalle);
            }
            else
            {
                var producto = await context.Productos.FindAsync(productoId);
                if (producto == null || !producto.Activo)
                {
                    return (false, "El producto no está disponible.", null);
                }

                if (nuevaCantidad > producto.Stock)
                {
                    return (false, $"Stock insuficiente. Disponible: {producto.Stock}.", null);
                }

                detalle.Cantidad = nuevaCantidad;
            }

            await context.SaveChangesAsync();
            await RecalcularTotal(pedido);
            await context.SaveChangesAsync();

            var resumen = await ObtenerResumenPedido(pedido.Id);
            return (true, null, resumen);
        }

        public async Task<(bool Exito, string? Error, PedidoDTO? Pedido)> EliminarItem(int pedidoId, int productoId)
        {
            var pedido = await context.Pedidos.FindAsync(pedidoId);
            if (pedido == null)
            {
                return (false, "El pedido no existe.", null);
            }

            if (pedido.Estado != EstadoCarrito)
            {
                return (false, "El pedido ya fue confirmado y no puede modificarse.", null);
            }

            var detalle = await context.DetallesPedidos
                .FirstOrDefaultAsync(dp => dp.PedidoId == pedidoId && dp.ProductoId == productoId);

            if (detalle != null)
            {
                context.DetallesPedidos.Remove(detalle);
                await context.SaveChangesAsync();
                await RecalcularTotal(pedido);
                await context.SaveChangesAsync();
            }

            var resumen = await ObtenerResumenPedido(pedido.Id);
            return (true, null, resumen);
        }

        public async Task<(bool Exito, string? Error, PedidoDTO? Pedido)> ActualizarDireccion(int pedidoId, string direccion)
        {
            if (string.IsNullOrWhiteSpace(direccion))
            {
                return (false, "Debe ingresar una dirección de entrega válida.", null);
            }

            var pedido = await context.Pedidos.FindAsync(pedidoId);
            if (pedido == null)
            {
                return (false, "El pedido no existe.", null);
            }

            if (pedido.Estado != EstadoCarrito)
            {
                return (false, "El pedido ya fue confirmado y no puede modificarse.", null);
            }

            pedido.Direccion = direccion.Trim();
            await context.SaveChangesAsync();

            var resumen = await ObtenerResumenPedido(pedido.Id);
            return (true, null, resumen);
        }

        public async Task<(bool Exito, string? Error, PedidoDTO? Pedido)> ConfirmarPedido(int pedidoId)
        {
            var pedido = await context.Pedidos.FindAsync(pedidoId);
            if (pedido == null)
            {
                return (false, "El pedido no existe.", null);
            }

            if (pedido.Estado != EstadoCarrito)
            {
                return (false, "El pedido ya no está en estado de carrito.", null);
            }

            var detalles = await context.DetallesPedidos
                .Include(dp => dp.Producto)
                .Where(dp => dp.PedidoId == pedidoId)
                .ToListAsync();

            if (!detalles.Any())
            {
                return (false, "El carrito está vacío. Agregue productos antes de confirmar.", null);
            }

            if (string.IsNullOrWhiteSpace(pedido.Direccion))
            {
                return (false, "Faltan los datos de entrega. Ingrese la dirección antes de confirmar.", null);
            }

            // Validar y descontar stock
            foreach (var item in detalles)
            {
                if (item.Cantidad > item.Producto.Stock)
                {
                    return (false, $"Stock insuficiente para '{item.Producto.Nombre}'. Disponible: {item.Producto.Stock}.", null);
                }
                item.Producto.Stock -= item.Cantidad;
            }

            pedido.Estado = EstadoPendientePago;
            await RecalcularTotal(pedido);
            await context.SaveChangesAsync();

            var resumen = await ObtenerResumenPedido(pedido.Id);
            return (true, null, resumen);
        }

        private async Task RecalcularTotal(Pedido pedido)
        {
            pedido.Total = await context.DetallesPedidos
                .Where(dp => dp.PedidoId == pedido.Id)
                .SumAsync(dp => dp.Cantidad * dp.Precio);
        }
    }
}
