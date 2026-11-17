using Microsoft.EntityFrameworkCore;
using Narcisa.BD.Datos;
using Narcisa.BD.Datos.Entity;
using Narcisa.Shared.DTO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Narcisa.Repositorio.Repositorios
{
    public class PagoRepositorio : Repositorio<Pago>, IPagoRepositorio
    {
        public PagoRepositorio(AppDbContext context) : base(context)
        {
        }

        public async Task<PagoDTO?> ObtenerPorPedido(int pedidoId)
        {
            var pago = await context.Pagos
                .Include(p => p.Pedido)
                .OrderByDescending(p => p.Fecha)
                .FirstOrDefaultAsync(p => p.PedidoId == pedidoId);

            if (pago == null)
            {
                return null;
            }

            return new PagoDTO
            {
                Id = pago.Id,
                Metodo = pago.Metodo,
                Estado = pago.Estado,
                Referencia = pago.Referencia,
                Fecha = pago.Fecha,
                PedidoId = pago.PedidoId,
                Total = pago.Pedido?.Total ?? 0
            };
        }

        public async Task<(bool Exito, string? Error, PagoDTO? Pago)> ProcesarPagoSimulado(ProcesarPagoDTO dto)
        {
            var pedido = await context.Pedidos
                .Include(p => p.Detalles)
                .FirstOrDefaultAsync(p => p.Id == dto.PedidoId);

            if (pedido == null)
            {
                return (false, "El pedido no existe.", null);
            }

            if (pedido.Estado == "Pagado")
            {
                return (false, "El pedido ya se encuentra pagado.", null);
            }

            var pago = new Pago
            {
                Metodo = dto.Metodo,
                Estado = "Aprobado",
                Referencia = $"MP-SIM-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                Fecha = DateTime.UtcNow,
                PedidoId = dto.PedidoId
            };

            context.Pagos.Add(pago);
            pedido.Estado = "Pagado";
            await context.SaveChangesAsync();

            var pagoDto = new PagoDTO
            {
                Id = pago.Id,
                Metodo = pago.Metodo,
                Estado = pago.Estado,
                Referencia = pago.Referencia,
                Fecha = pago.Fecha,
                PedidoId = pago.PedidoId,
                Total = pedido.Total
            };

            return (true, null, pagoDto);
        }
    }
}
