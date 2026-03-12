namespace ProyectoWebApp.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
    }

    public class Mesero
    {
        public int MeseroId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
    }

    public class Plato
    {
        public int PlatoId { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
    }

    public class FacturaCreateViewModels
    {
        public DateTime Fecha { get; set; }
        public IEnumerable<Cliente> Clientes { get; set; }
        public IEnumerable<Mesero> Meseros { get; set; }
        public IEnumerable<Plato> Platos { get; set; }

        // campos adicionales para la factura
        public int ClienteId { get; set; }
        public int MeseroId { get; set; }
        public List<LineaFactura> Lineas { get; set; }
    }

    public class LineaFactura
    {
        public int PlatoId { get; set; }
        public int Cantidad { get; set; }
    }

    // DTO usado por el controlador API (Create/Get)
    public class FacturaModel
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public int MeseroId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public List<LineaFactura> Lineas { get; set; }
    }
}