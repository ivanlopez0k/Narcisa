using System;
using System.Collections.Generic;

namespace Narcisa.Shared.DTO
{
    public class PedidoDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }
        public string? UsuarioEmail { get; set; }
        public string Estado { get; set; } = "";
        public string Direccion { get; set; } = "";
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public PagoDTO? Pago { get; set; }
        public List<ItemPedidoDTO> Items { get; set; } = new List<ItemPedidoDTO>();
    }
}
