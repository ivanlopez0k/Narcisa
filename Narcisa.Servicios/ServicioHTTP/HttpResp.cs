using System;
using System.Net.Http;

namespace Narcisa.Servicios.ServicioHTTP
{
    public class HttpResp<TResp>
    {
        public TResp? Respuesta { get; set; }
        public bool Error { get; set; }
        public HttpResponseMessage? Response { get; set; }

        private string? _mensaje;
        public string? Mensaje
        {
            get => !string.IsNullOrEmpty(_mensaje) ? _mensaje : ObtenerError();
            set => _mensaje = value;
        }

        public HttpResp(TResp? dato, bool error, HttpResponseMessage? response, string? mensaje)
        {
            Respuesta = dato;
            Error = error;
            Response = response;
            _mensaje = mensaje;
        }

        public string ObtenerError()
        {
            if (!string.IsNullOrEmpty(_mensaje))
            {
                return _mensaje;
            }

            if (Response == null)
            {
                return "No se obtuvo respuesta del servidor.";
            }

            if (Response.IsSuccessStatusCode)
            {
                return string.Empty;
            }

            return Response.StatusCode switch
            {
                System.Net.HttpStatusCode.BadRequest => "Solicitud incorrecta o datos inválidos.",
                System.Net.HttpStatusCode.Unauthorized => "No autorizado.",
                System.Net.HttpStatusCode.Forbidden => "Acceso denegado.",
                System.Net.HttpStatusCode.NotFound => "Recurso no encontrado.",
                System.Net.HttpStatusCode.InternalServerError => "Error interno del servidor.",
                _ => $"Error en la llamada HTTP: {Response.StatusCode}"
            };
        }
    }
}
