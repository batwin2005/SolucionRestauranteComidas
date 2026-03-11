namespace ProyectoModelo
{
    public class Venta
    {
        public int Id { get; set; }
        public int PlatoId { get; set; }
        public int FacturaId { get; set; }

        public Plato? Plato { get; set; }
        public Factura Factura { get; set; }
    }
}