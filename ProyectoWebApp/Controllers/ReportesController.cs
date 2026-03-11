using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace ProyectoWebApp.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IHttpClientFactory _httpFactory;
        public ReportesController(IHttpClientFactory httpFactory) => _httpFactory = httpFactory;

        // Totales por mesero en periodo
        public IActionResult TotalesMesero() => View();

        [HttpPost]
        public async Task<IActionResult> TotalesMesero(DateTime desde, DateTime hasta)
        {
            var client = _httpFactory.CreateClient("Api");
            var resp = await client.GetFromJsonAsync<IEnumerable<object>>($"api/reportes/totales-mesero?desde={desde:O}&hasta={hasta:O}");
            return View("TotalesMesero", resp);
        }

        // Clientes con consumo >= valor
        public IActionResult ClientesPorConsumo() => View();

        [HttpPost]
        public async Task<IActionResult> ClientesPorConsumo(decimal valor, DateTime desde, DateTime hasta)
        {
            var client = _httpFactory.CreateClient("Api");
            var resp = await client.GetFromJsonAsync<IEnumerable<object>>($"api/reportes/clientes-por-consumo?valor={valor}&desde={desde:O}&hasta={hasta:O}");
            return View("ClientesPorConsumo", resp);
        }

        // Producto más vendido en mes
        public IActionResult ProductoTop() => View();

        [HttpPost]
        public async Task<IActionResult> ProductoTop(int year, int month)
        {
            var client = _httpFactory.CreateClient("Api");
            var resp = await client.GetFromJsonAsync<object>($"api/reportes/producto-mas-vendido?year={year}&month={month}");
            return View("ProductoTop", resp);
        }
    }
}