using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace ApiDoc.Classe
{
    [Table("Usuario")]
    public class Usuario
    {
      
            [Key]
            public long IdUsuario { get; set; }
            public string Nome { get; set; }
            public string Email { get; set; }
            public string Senha { get; set; }
            public string Telefone { get; set; }
            public string Endereco { get; set; }
            public string CpfCnpj { get; set; }
            public string TipoPessoa { get; set; }

            public Doador Doador { get; set; }
            public List<Recepcao> Recepcaos { get; set; }
        
    }
}
