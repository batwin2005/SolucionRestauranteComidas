using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;

namespace ProyectoWebApp.Controllers
{
    public class FacturaController : Controller
    {
        private readonly IHttpClientFactory _httpFactory;

        public FacturaController(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        // Ejemplo de uso
        public async Task<IActionResult> Create()
        {
            var client = _httpFactory.CreateClient("Api");
            var clientes = await client.GetFromJsonAsync<IEnumerable<object>>("api/cliente");
            // ...
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] object payload)
        {
            var client = _httpFactory.CreateClient("Api");
            var resp = await client.PostAsJsonAsync("api/factura", payload);
            // ...
            return RedirectToAction("Index");
        }
    }
}