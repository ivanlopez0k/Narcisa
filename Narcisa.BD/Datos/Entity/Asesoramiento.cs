using System;
using System.Collections.Generic;
using System.Text;

namespace Narcisa.BD.Datos.Entity
{
    public class Asesoramiento : EntityBase
    {
        public string Descripcion { get; set; }
        public string Disponibilidad { get; set; }
        public string Estado { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
