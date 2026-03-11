namespace ProyectoModelo
{
    public class DetalleFactura
    {
        public int IdDetalle { get; set; }
        public int NroFactura { get; set; }
        public string Plato { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal SubTotal => Cantidad * ValorUnitario;
    }
}