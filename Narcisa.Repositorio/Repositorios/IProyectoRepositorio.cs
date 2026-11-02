using Narcisa.BD.Datos.Entity;
using Narcisa.Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Narcisa.Repositorio.Repositorios
{
    public interface IProyectoRepositorio : IRepositorio<Proyecto>
    {
        Task<List<ProyectoDTO>> ObtenerPorUsuario(int usuarioId);
        Task<ProyectoDTO?> ObtenerDetalle(int id, int usuarioId);
        Task<ProyectoDTO> GuardarProyecto(GuardarProyectoDTO dto, int usuarioId);
        Task<bool> EliminarProyecto(int id, int usuarioId);
    }
}
