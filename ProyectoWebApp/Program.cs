using ProyectoData;

var builder = WebApplication.CreateBuilder(args);

// HttpClient con nombre "Api" usando configuración
builder.Services.AddHttpClient("Api", client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];
    client.BaseAddress = new Uri(baseUrl);
});


//builder.Services.AddSingleton<ClienteData>();
builder.Services.AddTransient<MeseroData>();
builder.Services.AddTransient<PlatoData>();
builder.Services.AddTransient<FacturaData>();
builder.Services.AddTransient<ReportesData>();
builder.Services.AddTransient<ClienteData>(); // si ya existe

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();