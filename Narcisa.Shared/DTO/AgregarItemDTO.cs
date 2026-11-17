using System.ComponentModel.DataAnnotations;

namespace Narcisa.Shared.DTO
{
    public class AgregarItemDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe especificar un producto válido")]
        public int ProductoId { get; set; }

        [Range(1, 100, ErrorMessage = "La cantidad debe ser de al menos 1 unidad")]
        public int Cantidad { get; set; } = 1;
    }
}
