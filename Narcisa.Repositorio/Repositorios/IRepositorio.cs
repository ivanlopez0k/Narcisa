using System.Collections.Generic;
using System.Threading.Tasks;

namespace Narcisa.Repositorio.Repositorios
{
    public interface IRepositorio<E> where E : class
    {
        Task<bool> Delete(int id);
        Task<bool> Existe(E entity);
        Task<E> Insert(E entity);
        Task<List<E>> Select();
        Task<E?> SelectById(int id);
        Task<bool> Update(E entity);
    }
}
