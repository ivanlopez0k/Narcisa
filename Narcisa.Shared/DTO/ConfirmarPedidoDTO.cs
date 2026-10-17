using System.ComponentModel.DataAnnotations;

namespace Narcisa.Shared.DTO
{
    public class ConfirmarPedidoDTO
    {
        [Required(ErrorMessage = "La dirección de entrega es obligatoria")]
        [MinLength(5, ErrorMessage = "Ingrese una dirección válida (calle, número, localidad)")]
        [MaxLength(250, ErrorMessage = "La dirección no puede superar los 250 caracteres")]
        public string Direccion { get; set; } = "";
    }
}
