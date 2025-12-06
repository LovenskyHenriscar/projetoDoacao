using ApiGeral.Classe;
using ProjetoDoacao.Persistencia;
using Microsoft.Data.Sqlite;
using System;

namespace ProjetoDoacao.Repositorio
{
    public class LoginRepositorio
    {
        private readonly ConexaoDB _conexaoDB;

        public LoginRepositorio(ConexaoDB conexaoDB)
        {
            _conexaoDB = conexaoDB;
        }

        


        public Usuario? BuscarUsuarioPorEmail(string email)
        {
            try
            {
                using var conexao = _conexaoDB.Conexao();
                using var comando = conexao.CreateCommand();
                comando.CommandText = "SELECT * FROM Usuario WHERE LOWER(Email) = LOWER(@Email)";
                comando.Parameters.AddWithValue("@Email", email.Trim());


                using var leitor = comando.ExecuteReader();
                if (leitor.Read())
                {
                    return new Usuario
                    {
                        IdUsuario = leitor.GetInt64(0),
                        Nome = leitor.GetString(1),
                        Email = leitor.GetString(2),
                        Senha = leitor.GetString(3), 
                        Telefone = leitor.IsDBNull(4) ? "" : leitor.GetString(4),
                        Endereco = leitor.IsDBNull(5) ? "" : leitor.GetString(5),
                        CpfCnpj = leitor.IsDBNull(6) ? "" : leitor.GetString(6),
                        TipoPessoa = leitor.IsDBNull(7) ? "" : leitor.GetString(7)
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar usuário para login: " + ex.Message);
            }

            return null;
        }
    }
}
