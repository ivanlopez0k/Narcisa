using Narcisa.Servicios.ServicioHTTP;
using Narcisa.Shared.DTO;
using System;
using System.Threading.Tasks;

namespace Narcisa.Servicios
{
    public class AuthStateService
    {
        private readonly IHttpService _http;
        private readonly CarritoStateService _carritoState;

        public UsuarioDTO? UsuarioActual { get; private set; }
        public bool EstaAutenticado => UsuarioActual != null;

        public event Action? OnChange;

        public AuthStateService(IHttpService http, CarritoStateService carritoState)
        {
            _http = http;
            _carritoState = carritoState;
        }

        public async Task<(bool Exito, string? Error)> LoginAsync(LoginDTO dto)
        {
            try
            {
                var resp = await _http.PostAsync<LoginDTO, UsuarioDTO>("api/auth/login", dto);
                if (resp.Error || resp.Respuesta == null)
                {
                    return (false, resp.Mensaje ?? "Credenciales inválidas o error en el servidor.");
                }

                UsuarioActual = resp.Respuesta;
                _carritoState.UsuarioIdActual = UsuarioActual.Id;
                await _carritoState.InicializarAsync();
                NotifyStateChanged();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Error al iniciar sesión: {ex.Message}");
            }
        }

        public async Task<(bool Exito, string? Error)> RegisterAsync(RegisterDTO dto)
        {
            try
            {
                var resp = await _http.PostAsync<RegisterDTO, UsuarioDTO>("api/auth/register", dto);
                if (resp.Error || resp.Respuesta == null)
                {
                    return (false, resp.Mensaje ?? "Error al registrar la cuenta.");
                }

                UsuarioActual = resp.Respuesta;
                _carritoState.UsuarioIdActual = UsuarioActual.Id;
                await _carritoState.InicializarAsync();
                NotifyStateChanged();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Error al registrar usuario: {ex.Message}");
            }
        }

        public void Logout()
        {
            UsuarioActual = null;
            _carritoState.UsuarioIdActual = 1;
            _carritoState.LimpiarCarritoLocal();
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
