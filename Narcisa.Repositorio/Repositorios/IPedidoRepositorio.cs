using Narcisa.BD.Datos.Entity;
using Narcisa.Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Narcisa.Repositorio.Repositorios
{
    public interface IPedidoRepositorio : IRepositorio<Pedido>
    {
        Task<PedidoDTO?> ObtenerCarritoActivo(int usuarioId);
        Task<PedidoDTO?> ObtenerResumenPedido(int pedidoId);
        Task<PedidoDTO> CrearOObtenerCarrito(int usuarioId);
        Task<(bool Exito, string? Error, PedidoDTO? Pedido)> AgregarItem(int pedidoId, int productoId, int cantidad);
        Task<(bool Exito, string? Error, PedidoDTO? Pedido)> ActualizarCantidadItem(int pedidoId, int productoId, int nuevaCantidad);
        Task<(bool Exito, string? Error, PedidoDTO? Pedido)> EliminarItem(int pedidoId, int productoId);
        Task<(bool Exito, string? Error, PedidoDTO? Pedido)> ActualizarDireccion(int pedidoId, string direccion);
        Task<(bool Exito, string? Error, PedidoDTO? Pedido)> ConfirmarPedido(int pedidoId);
        Task<List<PedidoDTO>> ObtenerHistorialUsuario(int usuarioId);
        Task<List<PedidoDTO>> ObtenerTodosPedidos();
        Task<bool> ActualizarEstadoPedido(int pedidoId, string nuevoEstado);
    }
}
