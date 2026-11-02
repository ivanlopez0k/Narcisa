using System;
using System.Collections.Generic;

namespace Narcisa.Shared.DTO
{
    public class ProyectoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ImagenUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int UsuarioId { get; set; }
        public List<ProyectoItemDTO> Items { get; set; } = new();
    }
}
