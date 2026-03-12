using System;
using System.Collections.Generic;

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
}