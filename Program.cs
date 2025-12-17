using Microsoft.Data.SqlClient;
using P1700.Api.Repositories;
using P1700.Api.Services;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

var cs = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(cs))
    throw new Exception("No se encontró ConnectionStrings:DefaultConnection en appsettings.json");

builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(cs));
//Repositories
builder.Services.AddControllers();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<CatalogosRepository>();
builder.Services.AddScoped<EmpleadosRepository>();

//Services
builder.Services.AddScoped<EmpleadosService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CatalogosService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();
