using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;


namespace ApiDoc.Classe
{
    [Table("Recepcao")]
    public class Recepcao
    {
        [Key]
        public long IdRecepcao { get; set; }
        public string Nome { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public decimal Quantidade { get; set; }

        public Usuario Usuario { get; set; }
        public List<ItensRecepcao> Recepcaos { get; set; }
    }
}
