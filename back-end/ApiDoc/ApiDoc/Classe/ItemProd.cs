using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace ApiDoc.Classe
{
    public class ItemProd
    {

        [Key]
        public long IdItemProd { get; set; }
        public string Quantidade { get; set; }
        public TpStatuts Status { get; set; }

        public Doacao Doacao { get; set; }
        public Produto Produto { get; set; }
        public ItensRecepcao ItensRecepcao { get; set; }
    }
}
