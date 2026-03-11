using Microsoft.EntityFrameworkCore;
using ProyectoData;
using ProyectoServices;
using ProyectoServices.Implementaciones;
using ProyectoWebAPI.Data;

namespace ProyectoWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configuración de EF Core con SQL Server
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));

            // Registrar acceso a datos y servicios
            builder.Services.AddTransient<ClienteData>();
            builder.Services.AddTransient<FacturaData>();
            builder.Services.AddTransient<ReportesData>();
            builder.Services.AddTransient<PlatoData>();
            builder.Services.AddTransient<MeseroData>();

            builder.Services.AddScoped<IMeseroService, MeseroService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}