using Narcisa.BD.Datos.Entity;
using Narcisa.Shared.DTO;
using System.Threading.Tasks;

namespace Narcisa.Repositorio.Repositorios
{
    public interface IPagoRepositorio : IRepositorio<Pago>
    {
        Task<PagoDTO?> ObtenerPorPedido(int pedidoId);
        Task<(bool Exito, string? Error, PagoDTO? Pago)> ProcesarPagoSimulado(ProcesarPagoDTO dto);
    }
}
