using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace ApiGeral.Classe
{
    [Table("Produto")]
    public class Produto
    {
        private Int64 idProduto;
        private string nome;
        private decimal quantidade;
        private decimal peso;
        private List<ItemProd> itemProd;
        private Categoria categorias;

        public long IdProduto { get => idProduto; set => idProduto = value; }
        public string Nome { get => nome; set => nome = value; }
        public decimal Quantidade { get => quantidade; set => quantidade = value; }
        public decimal Peso { get => peso; set => peso = value; }
        public List<ItemProd> ItemProd { get => itemProd; set => itemProd = value; }
        public Categoria Categorias { get => categorias; set => categorias = value; }
    }
}
