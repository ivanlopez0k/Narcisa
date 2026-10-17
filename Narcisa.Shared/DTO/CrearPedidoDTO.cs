using System.ComponentModel.DataAnnotations;

namespace Narcisa.Shared.DTO
{
    public class CrearPedidoDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de usuario es obligatorio")]
        public int UsuarioId { get; set; }
    }
}
