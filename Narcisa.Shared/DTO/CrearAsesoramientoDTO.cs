using System.ComponentModel.DataAnnotations;

namespace Narcisa.Shared.DTO
{
    public class CrearAsesoramientoDTO
    {
        [Required(ErrorMessage = "La descripción de lo que buscás es obligatoria")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La disponibilidad horaria es obligatoria")]
        public string Disponibilidad { get; set; } = string.Empty;

        public string TipoEspacio { get; set; } = "Living / Sala de Estar";

        public int UsuarioId { get; set; }
    }
}
