using Narcisa.BD.Datos.Entity;
using Narcisa.Shared.DTO;
using System.Threading.Tasks;

namespace Narcisa.Repositorio.Repositorios
{
    public interface IUsuarioRepositorio : IRepositorio<Usuario>
    {
        Task<Usuario?> ObtenerPorEmail(string email);
        Task<Usuario?> ValidarCredenciales(string email, string password);
        Task<Usuario> Registrar(RegisterDTO dto);
    }
}
