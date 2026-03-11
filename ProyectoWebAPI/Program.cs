using Fluent.Infrastructure.FluentModel;
using ProyectoData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("CadenaSQL"));



builder.Services.AddTransient<ClienteData>();
builder.Services.AddTransient<FacturaData>();
builder.Services.AddTransient<ReportesData>();
builder.Services.AddTransient<ClienteData>(); 
builder.Services.AddTransient<PlatoData>();

//builder.Services.AddScoped<IMeseroService, MeseroService>();
//builder.Services.AddScoped<IPlatoService, PlatoService>();

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