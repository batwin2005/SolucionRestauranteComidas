using ProyectoData;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews()
#if DEBUG
            .AddRazorRuntimeCompilation();
#else
    ;
#endif

        builder.Services.AddHttpClient("Api", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5071/");
        });

        builder.Services.AddScoped<ClienteData>();
        builder.Services.AddScoped<MeseroData>();
        builder.Services.AddScoped<PlatoData>();
        builder.Services.AddScoped<FacturaData>();
        builder.Services.AddScoped<ReportesData>();

        // Si necesitas CORS para que la WebApp consuma la API
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                policy => policy.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader());
        });

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        });

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Factura}/{action=Index}/{id?}");

        app.Run();
    }
}

#if DEBUG

#else
#endif
