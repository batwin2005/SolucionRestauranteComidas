using Microsoft.AspNetCore.Mvc;
using ProyectoData;
using ProyectoModelo;

namespace ProyectoWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeseroController : ControllerBase
    {
        private readonly MeseroData _meseroData;

        public MeseroController(MeseroData meseroData)
        {
            _meseroData = meseroData;
        }

        // GET: api/mesero
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var meseros = _meseroData.GetAll();
                return Ok(meseros);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // GET: api/mesero/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var mesero = _meseroData.GetById(id);
                if (mesero == null) return NotFound();
                return Ok(mesero);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // POST: api/mesero
        [HttpPost]
        public IActionResult Create(Mesero mesero)
        {
            try
            {
                var id = _meseroData.Create(mesero);
                mesero.IdMesero = id;
                return CreatedAtAction(nameof(GetById), new { id }, mesero);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // PUT: api/mesero/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, Mesero mesero)
        {
            try
            {
                mesero.IdMesero = id;
                var ok = _meseroData.Update(mesero);
                if (!ok) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // DELETE: api/mesero/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var ok = _meseroData.Delete(id);
                if (!ok) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}