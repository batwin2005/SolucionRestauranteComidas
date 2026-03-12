using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProyectoWebApp.Models;

namespace ProyectoWebApp.Controllers
{
    public class FacturaController : Controller
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly ILogger<FacturaController> _logger;

        public FacturaController(IHttpClientFactory httpFactory, ILogger<FacturaController> logger)
        {
            _httpFactory = httpFactory;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var client = _httpFactory.CreateClient("Api");

                // Rango por defecto: últimos 30 días
                var desde = DateTime.Today.AddDays(-30);
                var hasta = DateTime.Today.AddDays(1);

                var url = $"api/factura?desde={Uri.EscapeDataString(desde.ToString("o"))}&hasta={Uri.EscapeDataString(hasta.ToString("o"))}";

                var list = await client.GetFromJsonAsync<IEnumerable<JsonElement>>(url)
                           ?? Enumerable.Empty<JsonElement>();

                return View(list);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "No se pudo obtener la lista de facturas desde la API.");
                TempData["Error"] = "No se pudo conectar con la API para obtener las facturas.";
                return View(Enumerable.Empty<JsonElement>());
            }
        }

        // GET Create
        public async Task<IActionResult> Create()
        {
            var vm = new FacturaCreateViewModel
            {
                Fecha = DateTime.Now,
                Clientes = Enumerable.Empty<Cliente>(),
                Meseros = Enumerable.Empty<Mesero>(),
                Platos = Enumerable.Empty<Plato>()
            };

            try
            {
                var client = _httpFactory.CreateClient("Api");

                vm.Clientes = await client.GetFromJsonAsync<IEnumerable<Cliente>>("api/cliente") ?? Enumerable.Empty<Cliente>();
                vm.Meseros = await client.GetFromJsonAsync<IEnumerable<Mesero>>("api/mesero") ?? Enumerable.Empty<Mesero>();
                vm.Platos = await client.GetFromJsonAsync<IEnumerable<Plato>>("api/plato") ?? Enumerable.Empty<Plato>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "No se pudo conectar con la API para poblar el formulario de creación de factura.");
                TempData["ErrorMessage"] = "No se pudo conectar con el servicio de API. Comprueba que el proyecto de API esté en ejecución y la URL en 'ApiSettings:BaseUrl'.";
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FacturaCreateViewModel payload)
        {
            try
            {
                var client = _httpFactory.CreateClient("Api");

                var apiPayload = new
                {
                    clienteId = payload.ClienteId,
                    meseroId = payload.MeseroId,
                    mesaId = payload.MesaId,  // <-- añadido
                    fecha = payload.Fecha,
                    lineas = payload.Lineas?.Select(l => new { platoId = l.PlatoId, cantidad = l.Cantidad, precio = l.Precio }).ToArray() ?? Array.Empty<object>()
                };

                var resp = await client.PostAsJsonAsync("api/factura", apiPayload);

                if (resp.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                _logger.LogWarning("API devolvió código {StatusCode} al crear factura.", resp.StatusCode);
                TempData["ErrorMessage"] = "Error al crear la factura en el servidor API.";
                return StatusCode((int)resp.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al enviar la factura a la API.");
                TempData["ErrorMessage"] = "No se pudo conectar con el servicio de API al intentar guardar la factura.";
                return StatusCode(500);
            }
        }
    }
}