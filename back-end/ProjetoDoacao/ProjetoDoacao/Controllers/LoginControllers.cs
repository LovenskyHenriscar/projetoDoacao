using Microsoft.AspNetCore.Mvc;
using ProjetoDoacao.Repositorio;
using ApiGeral.Classe;
using BCrypt.Net;
using Microsoft.AspNetCore.Cors;


namespace ProjetoDoacao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class LoginController : ControllerBase
    {
        private readonly LoginRepositorio _loginRepositorio;

        public LoginController(LoginRepositorio loginRepositorio)
        {
            _loginRepositorio = loginRepositorio;
        }

        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Senha { get; set; } = string.Empty;
        }
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha))
                return BadRequest(new { mensagem = "Email e senha são obrigatórios." });

            var usuario = _loginRepositorio.BuscarUsuarioPorEmail(login.Email);
            if (usuario == null)
                return Unauthorized(new { mensagem = "Usuário não encontrado." });

            bool senhaValida = BCrypt.Net.BCrypt.Verify(login.Senha, usuario.Senha);
            if (!senhaValida)
                return Unauthorized(new { mensagem = "Senha incorreta." });

            // Resposta com os dados do usuário direto no corpo principal
            return Ok(new
            {
                id = usuario.IdUsuario,
                nome = usuario.Nome,
                email = usuario.Email,
                tipoPessoa = usuario.TipoPessoa,
                mensagem = "Login realizado com sucesso!"
            });
        }

    }
}
