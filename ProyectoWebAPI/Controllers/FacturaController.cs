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
        public IActionResult Post([FromBody] Factura factura)
        {
            if (factura == null || factura.Detalles == null || factura.Detalles.Count == 0)
                return BadRequest("Factura o detalles inválidos.");

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
    }
}