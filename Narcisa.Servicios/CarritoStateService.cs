using Narcisa.Servicios.ServicioHTTP;
using Narcisa.Shared.DTO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Narcisa.Servicios
{
    public class CarritoStateService
    {
        private readonly IHttpService _http;
        public PedidoDTO? CarritoActual { get; private set; }
        public int CantidadTotalItems => CarritoActual?.Items?.Sum(i => i.Cantidad) ?? 0;
        public int UsuarioIdActual { get; set; } = 1; // Usuario Demo por defecto

        public event Action? OnChange;

        public CarritoStateService(IHttpService http)
        {
            _http = http;
        }

        public async Task InicializarAsync()
        {
            try
            {
                var resp = await _http.GetAsync<PedidoDTO?>($"api/pedido/usuario/{UsuarioIdActual}");
                if (!resp.Error && resp.Respuesta != null)
                {
                    CarritoActual = resp.Respuesta;
                }
                NotifyStateChanged();
            }
            catch
            {
                // Manejo resiliente durante prerenderizado
            }
        }

        public async Task<(bool Exito, string? Error)> AgregarProductoAsync(int productoId, int cantidad = 1)
        {
            if (CarritoActual == null)
            {
                var crearResp = await _http.PostAsync<CrearPedidoDTO, PedidoDTO>("api/pedido/crear", new CrearPedidoDTO { UsuarioId = UsuarioIdActual });
                if (crearResp.Error || crearResp.Respuesta == null)
                {
                    return (false, crearResp.Mensaje ?? "Error al inicializar el pedido en base de datos.");
                }
                CarritoActual = crearResp.Respuesta;
            }

            var resp = await _http.PostAsync<AgregarItemDTO, PedidoDTO>($"api/pedido/{CarritoActual.Id}/agregar", new AgregarItemDTO
            {
                ProductoId = productoId,
                Cantidad = cantidad
            });

            if (resp.Error || resp.Respuesta == null)
            {
                return (false, resp.Mensaje ?? "No se pudo agregar el producto al carrito.");
            }

            CarritoActual = resp.Respuesta;
            NotifyStateChanged();
            return (true, null);
        }

        public async Task<(bool Exito, string? Error)> ActualizarCantidadAsync(int productoId, int nuevaCantidad)
        {
            if (CarritoActual == null)
            {
                return (false, "No hay carrito activo.");
            }

            var resp = await _http.PutAsync<object?, PedidoDTO>($"api/pedido/{CarritoActual.Id}/item/{productoId}?cantidad={nuevaCantidad}", null);
            if (resp.Error || resp.Respuesta == null)
            {
                return (false, resp.Mensaje ?? "Error al actualizar la cantidad.");
            }

            CarritoActual = resp.Respuesta;
            NotifyStateChanged();
            return (true, null);
        }

        public async Task<(bool Exito, string? Error)> EliminarItemAsync(int productoId)
        {
            if (CarritoActual == null)
            {
                return (false, "No hay carrito activo.");
            }

            var resp = await _http.DeleteAsync<PedidoDTO>($"api/pedido/{CarritoActual.Id}/item/{productoId}");
            if (resp.Error || resp.Respuesta == null)
            {
                return (false, resp.Mensaje ?? "Error al eliminar el ítem.");
            }

            CarritoActual = resp.Respuesta;
            NotifyStateChanged();
            return (true, null);
        }

        public void LimpiarCarritoLocal()
        {
            CarritoActual = null;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
