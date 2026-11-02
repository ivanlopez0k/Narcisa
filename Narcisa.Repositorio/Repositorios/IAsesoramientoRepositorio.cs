using Narcisa.BD.Datos.Entity;
using Narcisa.Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Narcisa.Repositorio.Repositorios
{
    public interface IAsesoramientoRepositorio : IRepositorio<Asesoramiento>
    {
        Task<List<AsesoramientoDTO>> ObtenerPorUsuario(int usuarioId);
        Task<List<AsesoramientoDTO>> ObtenerTodos();
        Task<AsesoramientoDTO?> ObtenerPorId(int id);
        Task<AsesoramientoDTO> Crear(CrearAsesoramientoDTO dto, int usuarioId);
        Task<bool> ActualizarEstado(int id, string nuevoEstado);
    }
}
