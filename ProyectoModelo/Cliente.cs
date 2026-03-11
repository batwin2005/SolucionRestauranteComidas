// ProyectoModelo/Cliente.cs
using System;

namespace ProyectoModelo
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Identificacion { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}