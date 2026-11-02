using System.Collections.Generic;

namespace Narcisa.Shared.DTO
{
    public class GuardarProyectoDTO
    {
        public int? Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ImagenUrl { get; set; } = string.Empty;
        public List<GuardarProyectoItemDTO> Items { get; set; } = new();
    }
}
