
using GapBi.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração dos serviços da aplicação
builder.Services.AddControllers();

// Configuração do OpenAPI
builder.Services.AddOpenApi();

// Configuração da conexão com o SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ConexaoBancoDados")
    )
);

// Configuração do CORS para permitir acesso do React
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirReact", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "http://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configuração do ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Redirecionamento para HTTPS
app.UseHttpsRedirection();

// Habilita o CORS
app.UseCors("PermitirReact");

// Mapeamento dos Controllers
app.MapControllers();

// Inicialização da aplicação
app.Run();
