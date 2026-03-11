using Microsoft.AspNetCore.Mvc;
using ProyectoData;
using ProyectoModelo;
using System;

namespace ProyectoWebAPI.Controllers
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

        // GET api/mesero
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var list = _meseroData.GetAll();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/mesero/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var item = _meseroData.GetById(id);
                if (item == null) return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/mesero
        [HttpPost]
        public IActionResult Create([FromBody] Mesero mesero)
        {
            if (mesero == null) return BadRequest("Cuerpo de petición inválido.");
            try
            {
                var id = _meseroData.Create(mesero);
                return CreatedAtAction(nameof(GetById), new { id }, new { IdMesero = id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/mesero/{id}
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] Mesero mesero)
        {
            if (mesero == null || mesero.IdMesero != id) return BadRequest("Id inválido o cuerpo de petición inválido.");
            try
            {
                var ok = _meseroData.Update(mesero);
                return ok ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/mesero/{id}
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var ok = _meseroData.Delete(id);
                return ok ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}