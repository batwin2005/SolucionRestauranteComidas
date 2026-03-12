using Microsoft.AspNetCore.Mvc;
using ProyectoWebApp.Models;

namespace ProyectoWebApp.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturaController : ControllerBase
    {
        [HttpPost]
        public IActionResult Create([FromBody] FacturaModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            // Persistir la factura aquí (ejemplo: servicio o base de datos)
            return CreatedAtAction(nameof(Get), new { id = 1 }, model);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var factura = new FacturaModel { Id = id, ClienteNombre = "Cliente", Total = 100m };
            return Ok(factura);
        }
    }
}