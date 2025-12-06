using ApiGeral.Classe;
using Microsoft.Data.Sqlite;
using ProjetoDoacao.Persistencia;
using System;
using System.Collections.Generic;

namespace ProjetoDoacao.Repositorio
{
    public class UsuarioRepositorio
    {
        private readonly ConexaoDB _conexaoDB;

        public UsuarioRepositorio(ConexaoDB conexaoDB)
        {
            _conexaoDB = conexaoDB;
        }

      
        public string Inserir(Usuario usuario)
        {
            SqliteConnection? conexao = null;

            try
            {
                conexao = _conexaoDB.Conexao();
                using var comando = conexao.CreateCommand();

                comando.CommandText = @"INSERT INTO Usuario (Nome, Email, Senha, Telefone, Endereco, CpfCnpj, TipoPessoa)
                                        VALUES (@Nome, @Email, @Senha, @Telefone, @Endereco, @CpfCnpj, @TipoPessoa);";

                comando.Parameters.AddWithValue("@Nome", usuario.Nome);
                comando.Parameters.AddWithValue("@Email", usuario.Email);
                comando.Parameters.AddWithValue("@Senha", usuario.Senha);
                comando.Parameters.AddWithValue("@Telefone", usuario.Telefone ?? "");
                comando.Parameters.AddWithValue("@Endereco", usuario.Endereco ?? "");
                comando.Parameters.AddWithValue("@CpfCnpj", usuario.CpfCnpj ?? "");
                comando.Parameters.AddWithValue("@TipoPessoa", usuario.TipoPessoa ?? "");

                comando.ExecuteNonQuery();
                return "Usuário inserido com sucesso!";
            }
            catch (Exception ex)
            {
                return $"Erro: {ex.Message}";
            }
            finally
            {
                conexao?.Close();
            }
        }

        public List<Usuario> Listar()
        {
            var lista = new List<Usuario>();

            try
            {
                using var conexao = _conexaoDB.Conexao();
                using var comando = conexao.CreateCommand();
                comando.CommandText = "SELECT * FROM Usuario";

                using var leitor = comando.ExecuteReader();
                while (leitor.Read())
                {
                    lista.Add(new Usuario
                    {
                        IdUsuario = leitor.GetInt64(0),
                        Nome = leitor.GetString(1),
                        Email = leitor.GetString(2),
                        Senha = leitor.GetString(3),
                        Telefone = leitor.IsDBNull(4) ? "" : leitor.GetString(4),
                        Endereco = leitor.IsDBNull(5) ? "" : leitor.GetString(5),
                        CpfCnpj = leitor.IsDBNull(6) ? "" : leitor.GetString(6),
                        TipoPessoa = leitor.IsDBNull(7) ? "" : leitor.GetString(7)
                    });
                }
            }
            catch
            {
                // Log de erro
            }

            return lista;
        }

        public Usuario? BuscarPorId(long id)
        {
            try
            {
                using var conexao = _conexaoDB.Conexao();
                using var comando = conexao.CreateCommand();
                comando.CommandText = "SELECT * FROM Usuario WHERE Id = @Id";
                comando.Parameters.AddWithValue("@Id", id);

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
                throw new Exception("Erro ao buscar usuário: " + ex.Message);
            }

            return null;
        }

        public Usuario? BuscarPorEmail(string email)
        {
            try
            {
                using var conexao = _conexaoDB.Conexao();
                using var comando = conexao.CreateCommand();
                comando.CommandText = "SELECT * FROM Usuario WHERE Email = @Email";
                comando.Parameters.AddWithValue("@Email", email);

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
                throw new Exception("Erro ao buscar usuário por email: " + ex.Message);
            }

            return null;
        }

        public string Atualizar(Usuario usuario)
        {
            try
            {
                using var conexao = _conexaoDB.Conexao();
                using var comando = conexao.CreateCommand();

                comando.CommandText = @"UPDATE Usuario SET Nome = @Nome, Email = @Email, Senha = @Senha, 
                                        Telefone = @Telefone, Endereco = @Endereco, CpfCnpj = @CpfCnpj, TipoPessoa = @TipoPessoa
                                        WHERE Id = @Id";

                comando.Parameters.AddWithValue("@Nome", usuario.Nome);
                comando.Parameters.AddWithValue("@Email", usuario.Email);
                comando.Parameters.AddWithValue("@Senha", usuario.Senha);
                comando.Parameters.AddWithValue("@Telefone", usuario.Telefone ?? "");
                comando.Parameters.AddWithValue("@Endereco", usuario.Endereco ?? "");
                comando.Parameters.AddWithValue("@CpfCnpj", usuario.CpfCnpj ?? "");
                comando.Parameters.AddWithValue("@TipoPessoa", usuario.TipoPessoa?? "");
                comando.Parameters.AddWithValue("@Id", usuario.IdUsuario);

                int linhasAfetadas = comando.ExecuteNonQuery();
                return linhasAfetadas > 0 ? "Usuário atualizado com sucesso!" : "Usuário não encontrado.";
            }
            catch (Exception ex)
            {
                return $"Erro: {ex.Message}";
            }
        }

        public string Excluir(long id)
        {
            try
            {
                using var conexao = _conexaoDB.Conexao();
                using var comando = conexao.CreateCommand();
                comando.CommandText = "DELETE FROM Usuario WHERE Id = @Id";
                comando.Parameters.AddWithValue("@Id", id);

                int linhasAfetadas = comando.ExecuteNonQuery();
                return linhasAfetadas > 0 ? "Usuário excluído com sucesso!" : "Usuário não encontrado.";
            }
            catch (Exception ex)
            {
                return $"Erro: {ex.Message}";
            }
        }
    }
}
