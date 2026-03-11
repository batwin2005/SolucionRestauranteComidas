using Microsoft.EntityFrameworkCore;
using ProyectoModelo; // Ajusta según el namespace de tus entidades

namespace ProyectoWebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets para cada entidad del sistema
        public DbSet<Mesero> Meseros { get; set; }
        public DbSet<Plato> Platos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Venta> Ventas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Mesero>(entity =>
            {
                entity.HasKey(m => m.IdMesero);
                entity.Property(m => m.Nombres)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<Plato>(entity =>
            {
                entity.HasKey(p => p.IdPlato);
                entity.Property(p => p.Nombre)
                      .IsRequired()
                      .HasMaxLength(150);
                entity.Property(p => p.Precio)
                      .HasPrecision(18, 2);
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(c => c.IdCliente);
                entity.Property(c => c.Nombres)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<Factura>(entity =>
            {
                entity.HasKey(f => f.NroFactura);   // Aquí usas la propiedad real
                entity.HasOne<Cliente>()            // Relación con Cliente
                      .WithMany()
                      .HasForeignKey(f => f.IdCliente);
            });

            modelBuilder.Entity<Venta>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.HasOne(v => v.Plato)
                      .WithMany()
                      .HasForeignKey(v => v.PlatoId);
                entity.HasOne(v => v.Factura)
                      .WithMany()
                      .HasForeignKey(v => v.FacturaId);
            });
        }
    }
}