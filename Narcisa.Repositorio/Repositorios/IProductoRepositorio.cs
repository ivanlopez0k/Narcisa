using Narcisa.BD.Datos.Entity;
using Narcisa.Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Narcisa.Repositorio.Repositorios
{
    public interface IProductoRepositorio : IRepositorio<Producto>
    {
        Task<List<ProductoDTO>> ObtenerCatalogoActivo();
        Task<ProductoDTO?> ObtenerDetalle(int id);
        Task<List<ProductoDTO>> ObtenerTodosAdmin();
        Task<ProductoDTO> GuardarProducto(ProductoDTO dto);
        Task<bool> CambiarEstadoActivo(int id, bool activo);
    }
}
