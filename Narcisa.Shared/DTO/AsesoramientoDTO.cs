using System;

namespace Narcisa.Shared.DTO
{
    public class AsesoramientoDTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string Disponibilidad { get; set; } = string.Empty;
        public string Estado { get; set; } = "Nueva";
        public DateTime CreatedAt { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public string UsuarioEmail { get; set; } = string.Empty;
    }
}
