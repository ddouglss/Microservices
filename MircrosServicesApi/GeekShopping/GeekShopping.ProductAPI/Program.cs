using AutoMapper;
using GeekShopping.ProductAPI.Config;
using GeekShopping.ProductAPI.Model.Context;
using GeekShopping.ProductAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GeekShopping Product API",
        Version = "v1"
    });
});

// Adicionar suporte a Controllers
builder.Services.AddControllers();

// Configura��o do banco de dados
var connection = builder.Configuration.GetConnectionString("MySQLConnection")
                 ?? throw new InvalidOperationException("A string de conex�o 'MySQLConnection' n�o foi encontrada.");

builder.Services.AddDbContext<MySQLContext>(options =>
    options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

// Configura��o do AutoMapper
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configura��o dos reposit�rios
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configura��o do pipeline de requisi��es
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeekShopping Product API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers(); // Garante que os Controllers sejam mapeados corretamente

app.Run();
