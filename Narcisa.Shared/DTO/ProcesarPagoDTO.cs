using System.ComponentModel.DataAnnotations;

namespace Narcisa.Shared.DTO
{
    public class ProcesarPagoDTO
    {
        public int PedidoId { get; set; }

        [Required(ErrorMessage = "El método de pago es obligatorio")]
        public string Metodo { get; set; } = "Tarjeta de Crédito";

        [Required(ErrorMessage = "El número de tarjeta es obligatorio")]
        public string NumeroTarjeta { get; set; } = "";

        [Required(ErrorMessage = "El nombre del titular es obligatorio")]
        public string NombreTitular { get; set; } = "";

        [Required(ErrorMessage = "El vencimiento es obligatorio")]
        public string Vencimiento { get; set; } = "";

        [Required(ErrorMessage = "El código CVV es obligatorio")]
        public string Cvv { get; set; } = "";
    }
}
