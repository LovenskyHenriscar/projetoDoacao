using Dapper;
using Dapper.Contrib.Extensions;
using Projeto_de_Doacao.Models;
using ProjetoDoacao.Persistencia;
using System.Collections.Generic;
using System.Linq;

namespace Projeto_de_Doacao.Repository
{
    public class DoacaoRepository
    {
        private readonly ConexaoDB _conexaoDB;

        public DoacaoRepository(ConexaoDB conexaoDB)
        {
            _conexaoDB = conexaoDB;
        }

        // Insere uma nova doação e retorna o ID gerado
        public long Inserir(Doacao doacao)
        {
            using var conn = _conexaoDB.Conexao();
            return conn.Insert(doacao);
        }

        // Atualiza os dados de uma doação existente
        public bool Atualizar(Doacao doacao)
        {
            using var conn = _conexaoDB.Conexao();
            return conn.Update(doacao);
        }

        // Exclui uma doação pelo ID
        public bool Excluir(long id)
        {
            using var conn = _conexaoDB.Conexao();
            var doacao = conn.Get<Doacao>(id);
            if (doacao == null) return false;
            return conn.Delete(doacao);
        }

        // Obtém uma doação pelo ID
        public Doacao ObterPorId(long id)
        {
            using var conn = _conexaoDB.Conexao();
            const string sql = @"
                SELECT * FROM Doacao
                WHERE IdDoacao = @Id";
            return conn.QueryFirstOrDefault<Doacao>(sql, new { Id = id });
        }

        // Lista todas as doações
        public List<Doacao> Listar()
        {
            using var conn = _conexaoDB.Conexao();
            return conn.GetAll<Doacao>().ToList();
        }

        // Lista apenas as doações de um usuário específico
        public List<Doacao> ListarPorUsuario(long usuarioId)
        {
            using var conn = _conexaoDB.Conexao();
            const string sql = @"SELECT * FROM Doacao WHERE UsuarioId = @UsuarioId";
            return conn.Query<Doacao>(sql, new { UsuarioId = usuarioId }).ToList();
        }

        // Retorna somente as doações disponíveis (status 'disponivel' e quantidade > 0)
        public List<Doacao> ListarDisponiveis()
        {
            using var conn = _conexaoDB.Conexao();
            const string sql = @"SELECT * FROM Doacao WHERE Status = 'disponivel' AND Quantidade > 0";
            return conn.Query<Doacao>(sql).ToList();
        }
    }
}
