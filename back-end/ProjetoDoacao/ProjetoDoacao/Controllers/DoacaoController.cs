using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Projeto_de_Doacao.Dtos;
using Projeto_de_Doacao.Models;
using Projeto_de_Doacao.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Projeto_de_Doacao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class DoacaoController : ControllerBase
    {
        private readonly DoacaoRepository _repo;
        private readonly IWebHostEnvironment _env;
        private static readonly HttpClient _geoClient = new HttpClient()
        { DefaultRequestHeaders = { { "User-Agent", "DoaFacilApp" } } };

        public DoacaoController(DoacaoRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromForm] DoacaoCreateDto dto)
        {
            try
            {
                string webRoot = _env.WebRootPath;
                if (string.IsNullOrEmpty(webRoot))
                    webRoot = Path.Combine(_env.ContentRootPath, "wwwroot");

                var uploadsRoot = Path.Combine(webRoot, "uploads");
                Directory.CreateDirectory(uploadsRoot);

                string fotoUrl = string.Empty;
                if (dto.Foto != null && dto.Foto.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Foto.FileName)}";
                    var filePath = Path.Combine(uploadsRoot, fileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await dto.Foto.CopyToAsync(stream);
                    fotoUrl = $"/uploads/{fileName}";
                }

                var doacao = new Doacao
                {
                    UsuarioId = dto.UsuarioId,
                    Tipo = dto.Tipo,
                    Nome = dto.Nome,
                    Condicao = dto.Condicao,
                    Descricao = dto.Descricao,
                    Quantidade = dto.Quantidade,
                    Validade = dto.Validade,
                    Motivo = dto.Motivo,
                    DataDoacao = dto.DataDoacao,
                    Endereco = dto.Endereco,
                    RetiradaEmCasa = dto.RetiradaEmCasa,
                    RuaRetirada = dto.RuaRetirada,
                    NumeroRetirada = dto.NumeroRetirada,
                    BairroRetirada = dto.BairroRetirada,
                    CidadeRetirada = dto.CidadeRetirada,
                    CepRetirada = dto.CepRetirada,
                    FotoUrl = fotoUrl,
                    Status = "disponivel",
                    DoadorNome = dto.DoadorNome,
                    DoadorTelefone = dto.DoadorTelefone
                };
                /*
                if (doacao.RetiradaEmCasa)
                {
                    var address = $"{doacao.RuaRetirada}, {doacao.NumeroRetirada} – {doacao.BairroRetirada}, {doacao.CidadeRetirada}, Brasil";
                    var geo = await GeocodeAsync(address);
                    if (geo.HasValue)
                    {
                        doacao.Latitude = geo.Value.lat;
                        doacao.Longitude = geo.Value.lng;
                    }
                }
                */

                _repo.Inserir(doacao);
                return CreatedAtAction(nameof(Obter), new { id = doacao.IdDoacao }, doacao);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ListarTodas()
            => Ok(_repo.Listar());

        [HttpGet("historico/{usuarioId:long}")]
        public IActionResult HistoricoPorUsuario(long usuarioId)
            => Ok(_repo.ListarPorUsuario(usuarioId));

        [HttpGet("disponiveis")]
        public IActionResult ListarDisponiveis()
            => Ok(_repo.ListarDisponiveis());

        [HttpGet("{id:long}")]
        public IActionResult Obter(long id)
        {
            var doacao = _repo.ObterPorId(id);
            if (doacao == null)
                return NotFound();
            return Ok(doacao);
        }

        [HttpPatch("disponiveis/marcar-doado/{id:long}")]
        public IActionResult MarcarComoDoado(long id, [FromBody] QuantidadeDto dto)
        {
            var doacao = _repo.ObterPorId(id);
            if (doacao == null)
                return NotFound(new { mensagem = "Doação não encontrada." });

            if (dto == null || dto.Quantidade < 1)
                return BadRequest(new { mensagem = "Quantidade inválida." });

            if (doacao.Quantidade < dto.Quantidade)
                return BadRequest(new { mensagem = "Quantidade a doar excede o disponível." });

            doacao.Quantidade -= dto.Quantidade;
            doacao.DataAtualizacao = DateTime.UtcNow;

            _repo.Atualizar(doacao);
            return Ok(doacao);
        }

        [HttpDelete("{id:long}")]
        public IActionResult Excluir(long id)
        {
            var sucesso = _repo.Excluir(id);
            if (!sucesso)
                return NotFound(new { mensagem = "Doação não encontrada." });

            return NoContent();
        }

        private async Task<(double lat, double lng)?> GeocodeAsync(string address)
        {
            var url = "https://nominatim.openstreetmap.org/search?format=json&limit=1&q="
                    + Uri.EscapeDataString(address);
            var resp = await _geoClient.GetStringAsync(url);
            var results = JsonSerializer.Deserialize<List<NominatimResult>>(resp);
            if (results != null && results.Count > 0
             && double.TryParse(results[0].lat, NumberStyles.Any, CultureInfo.InvariantCulture, out var lat)
             && double.TryParse(results[0].lon, NumberStyles.Any, CultureInfo.InvariantCulture, out var lng))
            {
                return (lat, lng);
            }
            return null;
        }

        private class NominatimResult
        {
            public string lat { get; set; } = string.Empty;
            public string lon { get; set; } = string.Empty;
        }
    }

    public class QuantidadeDto
    {
        public int Quantidade { get; set; }
    }
}
