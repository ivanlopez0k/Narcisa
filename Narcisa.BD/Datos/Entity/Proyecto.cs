using System;
using System.Collections.Generic;
using System.Text;

namespace Narcisa.BD.Datos.Entity
{
    public class Proyecto : EntityBase
    {
        public string Nombre { get; set; }
        public string ImagenUrl { get; set; }
        public DateTime createdAt { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

    }
}
