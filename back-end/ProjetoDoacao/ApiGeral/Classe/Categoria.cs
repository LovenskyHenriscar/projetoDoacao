using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace ApiGeral.Classe
{
    public class Categoria
    {
        private Int64 idCategoria;
        private string nome;
        private decimal quantidade;
        private decimal peso;
        private List<Produto> produtos;

        public long IdCategoria { get => idCategoria; set => idCategoria = value; }
        public string Nome { get => nome; set => nome = value; }
        public decimal Quantidade { get => quantidade; set => quantidade = value; }
        public decimal Peso { get => peso; set => peso = value; }
        public List<Produto> Produtos { get => produtos; set => produtos = value; }
    }

}
