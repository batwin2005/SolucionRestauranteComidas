using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register named HttpClient "Api" with the base URL of the API
builder.Services.AddHttpClient("Api", client =>
{
    // Set the URL to your API (e.g. https://localhost:7054/ or http://localhost:5256/)
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7054/");
});

// (optional) Cors if accessing API from another origin
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Enable CORS if your API and web are on different origins
app.UseCors();

app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Factura}/{action=Index}/{id?}");

app.Run();
