using AutoMapper;
using GeekShopping.ProductAPI.Config;
using GeekShopping.ProductAPI.Model.Context;
using GeekShopping.ProductAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Swagger/OpenAPI
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

// Configuração do banco de dados
var connection = builder.Configuration.GetConnectionString("MySQLConnection")
                 ?? throw new InvalidOperationException("A string de conexão 'MySQLConnection' não foi encontrada.");

builder.Services.AddDbContext<MySQLContext>(options =>
    options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

// Configuração do AutoMapper
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configuração dos repositórios
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configuração do pipeline de requisições
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
