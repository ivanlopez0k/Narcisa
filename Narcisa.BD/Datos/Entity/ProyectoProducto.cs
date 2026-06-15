using System;
using System.Collections.Generic;
using System.Text;

namespace Narcisa.BD.Datos.Entity
{
    public class ProyectoProducto : EntityBase
    {
        public int ProyectoId { get; set; }
        public Proyecto Proyecto { get; set; }
        
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        public string Posicion { get; set; }

    }
}
