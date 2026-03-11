namespace ProyectoWebApp.Models
{
    public class FacturaCreateViewModel
    {
        public int ClienteId { get; set; }
        public int MeseroId { get; set; }
        public DateTime Fecha { get; set; }

        // Listas para poblar los combos en la vista
        public IEnumerable<object>? Clientes { get; set; }
        public IEnumerable<object>? Meseros { get; set; }
        public IEnumerable<object>? Platos { get; set; }

        // Detalles de la factura
        public List<FacturaLineaViewModel> Lineas { get; set; } = new();
    }

    public class FacturaLineaViewModel
    {
        public int PlatoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}