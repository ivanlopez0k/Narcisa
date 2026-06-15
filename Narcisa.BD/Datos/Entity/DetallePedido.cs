using System;
using System.Collections.Generic;
using System.Text;

namespace Narcisa.BD.Datos.Entity
{
    public class DetallePedido : EntityBase 
    {
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }

        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
    }
}
