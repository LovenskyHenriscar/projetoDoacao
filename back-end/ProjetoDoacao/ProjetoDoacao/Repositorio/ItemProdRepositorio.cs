using ApiGeral.Classe;
using ProjetoDoacao.Persistencia;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace Projeto_de_Doacao.Repository
{
    public class ItemProdRepository
    {
        private readonly ConexaoDB _conexaoDB;

        public ItemProdRepository(ConexaoDB conexaoDB)
        {
            _conexaoDB = conexaoDB;
        }

        public long Inserir(ItemProd item)
        {
            using var conn = _conexaoDB.Conexao();
            conn.Open();
            return conn.Insert(item);
        }

        public bool Alterar(ItemProd item)
        {
            using var conn = _conexaoDB.Conexao();
            conn.Open();
            return conn.Update(item);
        }

        public bool Excluir(long id)
        {
            using var conn = _conexaoDB.Conexao();
            conn.Open();
            var item = conn.Get<ItemProd>(id);
            if (item == null) return false;
            return conn.Delete(item);
        }

        public ItemProd Obter(long id)
        {
            using var conn = _conexaoDB.Conexao();
            conn.Open();
            return conn.Get<ItemProd>(id);
        }

        public List<ItemProd> Listar()
        {
            using var conn = _conexaoDB.Conexao();
            conn.Open();
            return conn.GetAll<ItemProd>().ToList();
        }

        // Método ListarPorDoacaoId removido pois DoacaoId foi eliminado
    }
}
