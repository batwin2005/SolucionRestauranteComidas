using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ProyectoWebApp.Models;
using System;

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

        public IActionResult Index()
        {
            return View(); // busca Views/Factura/Index.cshtml
        }

        // GET Create: construye y devuelve el ViewModel esperado por la vista
        public async Task<IActionResult> Create()
        {
            var vm = new FacturaCreateViewModel
            {
                Fecha = DateTime.Now,
                Clientes = Enumerable.Empty<object>(),
                Meseros = Enumerable.Empty<object>(),
                Platos = Enumerable.Empty<object>()
            };

            try
            {
                var client = _httpFactory.CreateClient("Api");
                vm.Clientes = await client.GetFromJsonAsync<IEnumerable<object>>("api/cliente") ?? Enumerable.Empty<object>();
                vm.Meseros = await client.GetFromJsonAsync<IEnumerable<object>>("api/mesero") ?? Enumerable.Empty<object>();
                vm.Platos = await client.GetFromJsonAsync<IEnumerable<object>>("api/plato") ?? Enumerable.Empty<object>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "No se pudo conectar con la API para poblar el formulario de creación de factura.");
                TempData["ErrorMessage"] = "No se pudo conectar con el servicio de API. Comprueba que el proyecto de API esté en ejecución y la URL en 'ApiBaseUrl'.";
            }

            return View(vm);
        }   

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FacturaCreateViewModel payload)
        {
            if (payload == null)
            {
                TempData["ErrorMessage"] = "Datos de factura inválidos.";
                return BadRequest();
            }

            try
            {
                var client = _httpFactory.CreateClient("Api");

                // Construir el objeto que espera la API (ajusta nombres si tu API difiere)
                var apiPayload = new
                {
                    clienteId = payload.ClienteId,
                    meseroId = payload.MeseroId,
                    fecha = payload.Fecha,
                    lineas = payload.Lineas?.Select(l => new { platoId = l.PlatoId, cantidad = l.Cantidad }).ToArray()
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