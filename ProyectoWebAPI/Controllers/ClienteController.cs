// ProyectoWebAPI/Controllers/ClienteController.cs
using Microsoft.AspNetCore.Mvc;
using ProyectoData;
using ProyectoModelo;

namespace ProyectoWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteData _clienteData;

        public ClienteController(ClienteData clienteData)
        {
            _clienteData = clienteData;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _clienteData.GetAll();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var c = _clienteData.GetById(id);
            if (c == null) return NotFound();
            return Ok(c);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Cliente cliente)
        {
            var id = _clienteData.Create(cliente);
            cliente.IdCliente = id;
            return CreatedAtAction(nameof(Get), new { id }, cliente);
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] Cliente cliente)
        {
            if (cliente == null || cliente.IdCliente != id) return BadRequest();
            var ok = _clienteData.Update(cliente);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var ok = _clienteData.Delete(id);
            return ok ? NoContent() : NotFound();
        }

    }
}