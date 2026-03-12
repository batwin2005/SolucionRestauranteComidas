using Microsoft.AspNetCore.Mvc;
using ProyectoData;
using ProyectoModelo;

namespace ProyectoWebApp.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturaController : ControllerBase
    {
        private readonly FacturaData _facturaData;

        public FacturaController(FacturaData facturaData)
        {
            _facturaData = facturaData;
        }

        [HttpPost]
        public IActionResult Create(FacturaCreateDto dto)
        {
            try
            {
                var factura = new Factura
                {
                    IdCliente = dto.ClienteId,
                    IdMesero = dto.MeseroId,
                    IdMesa = null,
                    Fecha = dto.Fecha,
                    Detalles = dto.Lineas.Select(l => new DetalleFactura
                    {
                        Plato = l.PlatoId,
                        Cantidad = l.Cantidad,
                        ValorUnitario = l.Precio
                    }).ToList()
                };

                var id = _facturaData.CreateFactura(factura);
                return Ok(new { nroFactura = id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear factura: {ex.Message}");
            }
        }
    }

    public class FacturaCreateDto
    {
        public int ClienteId { get; set; }
        public int MeseroId { get; set; }
        public DateTime Fecha { get; set; }
        public List<FacturaLineaDto> Lineas { get; set; } = new();
    }

    public class FacturaLineaDto
    {
        public int PlatoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}