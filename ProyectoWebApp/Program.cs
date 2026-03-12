var builder = WebApplication.CreateBuilder(args);

// HttpClient con nombre "Api" usando configuración
builder.Services.AddHttpClient("Api", client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();