using Microsoft.Data.SqlClient;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// ... otros servicios
var app = builder.Build();

// Comprobación de conexión
try
{
    var connString = app.Configuration.GetConnectionString("CadenaSQL"); // o "DefaultConnection"
    using (var conn = new System.Data.SqlClient.SqlConnection(connString))
    {
        conn.Open();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Éxito: Conexión a RestauranteDB establecida.");
        Console.ResetColor();
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Fracaso: No se pudo conectar a RestauranteDB.");
    Console.WriteLine(ex.Message);
    Console.ResetColor();
}

app.MapControllers();
app.Run();