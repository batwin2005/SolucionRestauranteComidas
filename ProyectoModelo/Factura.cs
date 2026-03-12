namespace ProyectoModelo
{
    public class Factura
    {
        public int NroFactura { get; set; }
        public int IdCliente { get; set; }
        public int IdMesero { get; set; }
        public int? IdMesa { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public List<DetalleFactura>? Detalles { get; set; }
    }

    public class DetalleFactura
    {
        public int Plato { get; set; }          // Id del plato
        public int Cantidad { get; set; }
        public decimal ValorUnitario { get; set; }
    }
}