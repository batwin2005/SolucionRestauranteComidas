using Microsoft.AspNetCore.Mvc;
using ProyectoData;

namespace ProyectoWebAPI.Controllers
{
    [ApiController]
    [Route("api/reportes")]
    public class ReporteController : ControllerBase
    {
        private readonly ReportesData _repo;
        public ReporteController(ReportesData repo) => _repo = repo;

        [HttpGet("ventas-por-mesero")]
        public IActionResult VentasPorMesero([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
            => Ok(_repo.VentasPorMesero(desde, hasta));

        [HttpGet("clientes-por-consumo")]
        public IActionResult ClientesPorConsumo([FromQuery] decimal minimo, [FromQuery] DateTime desde, [FromQuery] DateTime hasta)
            => Ok(_repo.ClientesPorConsumo(minimo, desde, hasta));

        [HttpGet("producto-mas-vendido")]
        public IActionResult ProductoMasVendido([FromQuery] int anio, [FromQuery] int mes)
            => Ok(_repo.ProductoMasVendido(anio, mes));
    }
}