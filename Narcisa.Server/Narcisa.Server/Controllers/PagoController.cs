using Microsoft.AspNetCore.Mvc;
using Narcisa.Repositorio.Repositorios;
using Narcisa.Shared.DTO;
using System.Threading.Tasks;

namespace Narcisa.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly IPagoRepositorio _pagoRepositorio;

        public PagoController(IPagoRepositorio pagoRepositorio)
        {
            _pagoRepositorio = pagoRepositorio;
        }

        [HttpPost("procesar")]
        public async Task<ActionResult<PagoDTO>> Procesar([FromBody] ProcesarPagoDTO dto)
        {
            var resultado = await _pagoRepositorio.ProcesarPagoSimulado(dto);
            if (!resultado.Exito)
            {
                return BadRequest(resultado.Error);
            }

            return Ok(resultado.Pago);
        }

        [HttpGet("pedido/{pedidoId:int}")]
        public async Task<ActionResult<PagoDTO>> ObtenerPorPedido(int pedidoId)
        {
            var pago = await _pagoRepositorio.ObtenerPorPedido(pedidoId);
            if (pago == null)
            {
                return NotFound("No se encontró el comprobante de pago para este pedido.");
            }

            return Ok(pago);
        }
    }
}
