namespace ProyectoModelo
{
    public class Mesero
    {
        public int IdMesero { get; set; }
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public int? Edad { get; set; }
        public int? Antiguedad { get; set; }
        public DateTime? FechaIngreso { get; set; }
    }
}