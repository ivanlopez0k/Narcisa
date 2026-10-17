using System.ComponentModel.DataAnnotations;

namespace Narcisa.Shared.DTO
{
    public class ProductoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [MaxLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres")]
        public string Nombre { get; set; } = "";

        [MaxLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres")]
        public string Descripcion { get; set; } = "";

        [Range(0.01, 10000000, ErrorMessage = "El precio debe ser mayor a cero")]
        public decimal Precio { get; set; }

        [Range(0, 10000, ErrorMessage = "El stock debe ser cero o positivo")]
        public int Stock { get; set; }

        public string ImagenUrl { get; set; } = "";
        public bool Activo { get; set; } = true;
    }
}
