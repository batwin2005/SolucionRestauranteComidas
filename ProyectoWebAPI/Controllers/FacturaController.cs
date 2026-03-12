using Microsoft.AspNetCore.Mvc;
using ProyectoData;
using ProyectoModelo;

namespace ProyectoWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturaController : ControllerBase
    {
        private readonly FacturaData _data;
        public FacturaController(FacturaData data) => _data = data;

        [HttpPost]
        public IActionResult Post([FromBody] FacturaCreateDto dto)
        {
            if (dto == null || dto.Lineas == null || dto.Lineas.Count == 0)
                return BadRequest("Factura o detalles inválidos.");

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

            var id = _data.CreateFactura(factura);
            return CreatedAtAction(nameof(GetById), new { id }, new { NroFactura = id });
        }

        [HttpGet]
        public IActionResult Get([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        {
            var list = _data.GetFacturas(desde, hasta);
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            return NotFound();
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
}