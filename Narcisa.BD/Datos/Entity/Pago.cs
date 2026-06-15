using System;
using System.Collections.Generic;
using System.Text;

namespace Narcisa.BD.Datos.Entity
{
    public class Pago : EntityBase
    {
        public string Metodo { get; set; }
        public string Estado { get; set; }
        public string Referencia { get; set; }
        public DateTime Fecha { get; set; }

        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }
    }
}
