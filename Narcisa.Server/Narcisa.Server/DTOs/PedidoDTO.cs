using System.Collections.Generic;

namespace Narcisa.Server.DTOs
{
    public class PedidoDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Estado { get; set; }
        public string Direccion { get; set; }
        public decimal Total { get; set; }
        public List<ItemPedidoDTO> Items { get; set; } = new();
    }

    public class ItemPedidoDTO
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
