using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace ApiGeral.Classe
{
    [Table("ItemProd")]
    public class ItemProd
    {
        private Int64 idItemProd;
        private int quantidade;
        private long produtoId;
        private long? itensRecepcaoId;
        private Produto produto;
        private ItensRecepcao itensRecepcao;

        [Key]
        public long IdItemProd { get => idItemProd; set => idItemProd = value; }
        public int Quantidade { get => quantidade; set => quantidade = value; }
        public long ProdutoId { get => produtoId; set => produtoId = value; }
        public long? ItensRecepcaoId { get => itensRecepcaoId; set => itensRecepcaoId = value; }

        [Write(false)]
        public Produto Produto { get => produto; set => produto = value; }

        [Write(false)]
        public ItensRecepcao ItensRecepcao { get => itensRecepcao; set => itensRecepcao = value; }

    }
}
