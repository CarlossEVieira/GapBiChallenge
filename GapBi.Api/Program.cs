
using GapBi.Api.Data;
using Microsoft.EntityFrameworkCore;
using GapBi.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração dos serviços da aplicação
builder.Services.AddControllers();

// Configuração do HttpClient para realizar requisições a APIs externas
builder.Services.AddHttpClient();

// Registra o serviço responsável por consultar a API DummyJSON
builder.Services.AddScoped<DummyJsonService>();

// Registra o serviço responsável pela sincronização dos dados
builder.Services.AddScoped<SincronizacaoService>();

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
