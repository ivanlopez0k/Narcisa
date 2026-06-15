using System;
using System.Collections.Generic;
using System.Text;

namespace Narcisa.BD.Datos.Entity
{
    public class Pedido : EntityBase
    { 
        public string Direccion {  get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public DateTime CreatedAt {  get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
