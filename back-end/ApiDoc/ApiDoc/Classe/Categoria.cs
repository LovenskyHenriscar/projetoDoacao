using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;




namespace ApiDoc.Classe
{
    public class Categoria
    {
        [Key]
        public long IdCategoria { get; set; }
        public string Nome { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Peso { get; set; }

        public List<Produto> Produtos { get; set; }
    }
}
