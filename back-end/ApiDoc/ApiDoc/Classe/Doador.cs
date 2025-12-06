using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;



namespace ApiDoc.Classe
{
    [Table("Doador")] 
    public class Doador
    {
        public long IdDoador { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }

        public Usuario Usuario { get; set; }
        public List<Doacao> DoacaoList { get; set; }
    }
}
