using Microsoft.AspNetCore.Mvc;
using ProyectoWebApp.Models;
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

        public async Task<IActionResult> Index()
        {
            // listado de facturas (si lo deseas)
            var client = _httpFactory.CreateClient("Api");
            var resp = await client.GetFromJsonAsync<IEnumerable<object>>("api/factura");
            return View(resp);
        }

        public async Task<IActionResult> Create()
        {
            var client = _httpFactory.CreateClient("Api");

            var vm = new FacturaCreateViewModel
            {
                Clientes = await client.GetFromJsonAsync<IEnumerable<object>>("api/cliente"),
                Meseros = await client.GetFromJsonAsync<IEnumerable<object>>("api/mesero"),
                Platos = await client.GetFromJsonAsync<IEnumerable<object>>("api/plato")
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FacturaCreateViewModel factura)
        {
            var client = _httpFactory.CreateClient("Api");

            // Construir payload según tu API de facturas. Ejemplo genérico:
            var payload = new
            {
                clienteId = factura.ClienteId,
                meseroId = factura.MeseroId,
                fecha = factura.Fecha,
                lineas = factura.Lineas.Select(l => new { platoId = l.PlatoId, cantidad = l.Cantidad })
            };

            var resp = await client.PostAsJsonAsync("api/factura", payload);

            if (resp.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // si falla, recargar selects y mostrar error
            factura.Clientes = await client.GetFromJsonAsync<IEnumerable<object>>("api/cliente");
            factura.Meseros = await client.GetFromJsonAsync<IEnumerable<object>>("api/mesero");
            factura.Platos = await client.GetFromJsonAsync<IEnumerable<object>>("api/plato");
            ModelState.AddModelError("", "Error al crear la factura");
            return View(factura);
        }

        // Reportes: ejemplo de acción para Totales por mesero
        public IActionResult Reportes() => View();
    }
}