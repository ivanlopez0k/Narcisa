using System;
using System.Collections.Generic;
using System.Text;

namespace Narcisa.BD.Datos.Entity
{
    public class Usuario : EntityBase
    {
        public string Nombre { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string rol {  get; set; }
        public DateTime createdAt { get; set; }

    }
}
