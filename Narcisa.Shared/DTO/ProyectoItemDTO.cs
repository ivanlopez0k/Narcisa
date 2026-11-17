namespace Narcisa.Shared.DTO
{
    public class ProyectoItemDTO
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public decimal ProductoPrecio { get; set; }
        public string? ImagenPngUrl { get; set; }
        public string Posicion { get; set; } = string.Empty;
    }
}
