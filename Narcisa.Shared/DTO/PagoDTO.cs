using System;

namespace Narcisa.Shared.DTO
{
    public class PagoDTO
    {
        public int Id { get; set; }
        public string Metodo { get; set; } = "";
        public string Estado { get; set; } = "";
        public string Referencia { get; set; } = "";
        public DateTime Fecha { get; set; }
        public int PedidoId { get; set; }
        public decimal Total { get; set; }
    }
}
