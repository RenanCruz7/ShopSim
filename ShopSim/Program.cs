using Microsoft.EntityFrameworkCore;
using ShopSim.Data;
using ShopSim.Profiles;
using ShopSim.Services;
using Pomelo.EntityFrameworkCore.MySql;

var builder = WebApplication.CreateBuilder(args);

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(ProductProfile));

// Injeção de dependência do serviço de produtos
builder.Services.AddScoped<IProductService, ProductService>();

// Configuração do DbContext com SQL Server
var connectionString = builder.Configuration.GetConnectionString("DriverConnection");
builder.Services.AddDbContext<ShopSimContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();