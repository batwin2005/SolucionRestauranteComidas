using ProyectoData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("CadenaSQL"));
builder.Services.AddSingleton<EmpleadoData>();

//builder.Services.AddSingleton<ClienteData>();
builder.Services.AddTransient<FacturaData>();
builder.Services.AddTransient<ReportesData>();
builder.Services.AddTransient<ClienteData>(); // si ya existe

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