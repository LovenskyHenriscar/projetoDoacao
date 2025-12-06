using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;


namespace ApiDoc.Classe
{
    [Table("Produto")]
    public class Produto
    {
        [Key]
        public long IdProduto { get; set; }
        public string Nome { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Peso { get; set; }

        public List<ItemProd> itemProds{ get; set; }
        public Categoria Categoria { get; set; }
    }
}
