using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;




namespace ApiDoc.Classe
{
    public class Doacao
    {
        public long IdDoacao { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public string Condicao { get; set; }

        public Doador Doador { get; set; }
        public List<ItemProd> ItemProds { get; set; }

    }
}
