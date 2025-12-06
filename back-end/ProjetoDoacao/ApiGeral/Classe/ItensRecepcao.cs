using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace ApiGeral.Classe
{
    public class ItensRecepcao
    {
        private Int64 idItensRecepcao;
        private decimal quantidade;
        private Recepcao Recepcao;
        private ItemProd ItemProd;

        public long IdItensRecepcao { get => idItensRecepcao; set => idItensRecepcao = value; }
        public decimal Quantidade { get => quantidade; set => quantidade = value; }
        public Recepcao Recepcao1 { get => Recepcao; set => Recepcao = value; }
        public ItemProd ItemProd1 { get => ItemProd; set => ItemProd = value; }
    }
}
