using Projeto_de_Doacao.Repository;
using ProjetoDoacao.Persistencia;
using ProjetoDoacao.Repositorio;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1) CORS (aceita qualquer origem, cabeçalho e método)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 2) Controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3) DI dos repositórios e DB
builder.Services.AddScoped<LoginRepositorio>();
builder.Services.AddScoped<UsuarioRepositorio>();
builder.Services.AddScoped<DoacaoRepository>();
builder.Services.AddScoped<ConexaoDB>();

var app = builder.Build();

// 4) Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 4.1) Redireciona para HTTPS
app.UseHttpsRedirection();

// 4.2) Serve arquivos estáticos em wwwroot (incluindo /uploads)
app.UseStaticFiles();

// 4.3) Roteamento
app.UseRouting();

// 4.4) CORS
app.UseCors("AllowAll");

// 4.5) Autorização (se tiver autenticação)
app.UseAuthorization();

// 4.6) Mapeia seus controllers [ApiController]
app.MapControllers();

app.Run();
