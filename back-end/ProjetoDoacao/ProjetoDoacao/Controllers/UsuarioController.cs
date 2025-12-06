using Microsoft.AspNetCore.Mvc;
using ApiGeral.Classe;
using ProjetoDoacao.Repositorio;
using System.Linq;

namespace ProjetoDoacao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioRepositorio _usuarioRepositorio;

        public UsuarioController(UsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        [HttpPost("cadastrar")]
        public IActionResult Cadastrar([FromBody] Usuario usuario)
        {
            if (usuario == null)
                return BadRequest(new { mensagem = "Dados do usuário inválidos." });

            // Validação de CPF ou CNPJ
            if (!CpfOuCnpjValido(usuario.CpfCnpj))
                return BadRequest(new { mensagem = "CPF ou CNPJ inválido." });

            // Hashear a senha antes de salvar
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

            var resultado = _usuarioRepositorio.Inserir(usuario);

            if (resultado == "Usuário inserido com sucesso!")
                return Ok(new { mensagem = resultado });

            return BadRequest(new { mensagem = resultado });
        }

        [HttpGet("buscar/{id}")]
        public IActionResult BuscarPorId(long id)
        {
            var usuario = _usuarioRepositorio.BuscarPorId(id);

            if (usuario == null)
                return NotFound(new { mensagem = "Usuário não encontrado." });

            return Ok(usuario);
        }

        [HttpGet("listar")]
        public IActionResult ListarTodos()
        {
            var usuarios = _usuarioRepositorio.Listar();
            return Ok(usuarios);
        }

        [HttpPut("atualizar/{id}")]
        public IActionResult Atualizar(long id, [FromBody] Usuario usuario)
        {
            var usuarioExistente = _usuarioRepositorio.BuscarPorId(id);
            if (usuarioExistente == null)
                return NotFound(new { mensagem = "Usuário não encontrado." });

            usuario.IdUsuario = id;

            // Hashear a nova senha se ela foi alterada
            if (!string.IsNullOrEmpty(usuario.Senha))
            {
                usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
            }

            var resultado = _usuarioRepositorio.Atualizar(usuario);
            return Ok(new { mensagem = resultado });
        }

        [HttpDelete("excluir/{id}")]
        public IActionResult Excluir(long id)
        {
            var usuarioExistente = _usuarioRepositorio.BuscarPorId(id);
            if (usuarioExistente == null)
                return NotFound(new { mensagem = "Usuário não encontrado." });

            var resultado = _usuarioRepositorio.Excluir(id);
            return Ok(new { mensagem = resultado });
        }

        // Métodos de validação de CPF e CNPJ abaixo:

        private bool CpfOuCnpjValido(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return false;

            valor = new string(valor.Where(char.IsDigit).ToArray());

            if (valor.Length == 11)
                return ValidarCpf(valor);
            else if (valor.Length == 14)
                return ValidarCnpj(valor);
            else
                return false;
        }

        private bool ValidarCpf(string cpf)
        {
            if (cpf.Length != 11 || cpf.All(c => c == cpf[0])) return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            tempCpf += resto.ToString();

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            return cpf.EndsWith(resto.ToString());
        }

        private bool ValidarCnpj(string cnpj)
        {
            if (cnpj.Length != 14 || cnpj.All(c => c == cnpj[0])) return false;

            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            tempCnpj += resto.ToString();

            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            return cnpj.EndsWith(resto.ToString());
        }
    }
}
