using System;
using System.Collections.Generic;

namespace ProyectoWebApp.Models
{
    public class FacturaCreateViewModel
    {
        public int ClienteId { get; set; }
        public int MeseroId { get; set; }
        public int? MesaId { get; set; }   // <-- añadido
        public DateTime Fecha { get; set; }

        public IEnumerable<Cliente>? Clientes { get; set; }
        public IEnumerable<Mesero>? Meseros { get; set; }
        public IEnumerable<Plato>? Platos { get; set; }

        public List<FacturaLineaViewModel> Lineas { get; set; } = new List<FacturaLineaViewModel>();
    }

    public class FacturaLineaViewModel
    {
        public int PlatoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}