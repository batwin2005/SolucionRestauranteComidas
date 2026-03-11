using Microsoft.AspNetCore.Mvc;
using ProyectoData;
using ProyectoModelo;
using System;

namespace ProyectoWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatoController : ControllerBase
    {
        private readonly PlatoData _platoData;

        public PlatoController(PlatoData platoData)
        {
            _platoData = platoData;
        }

        // GET api/plato
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var list = _platoData.GetAll();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/plato/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var item = _platoData.GetById(id);
                if (item == null) return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/plato
        [HttpPost]
        public IActionResult Create([FromBody] Plato plato)
        {
            if (plato == null) return BadRequest("Cuerpo de petición inválido.");
            try
            {
                var id = _platoData.Create(plato);
                return CreatedAtAction(nameof(GetById), new { id }, new { IdPlato = id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/plato/{id}
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] Plato plato)
        {
            if (plato == null || plato.IdPlato != id) return BadRequest("Id inválido o cuerpo de petición inválido.");
            try
            {
                var ok = _platoData.Update(plato);
                return ok ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/plato/{id}
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var ok = _platoData.Delete(id);
                return ok ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}