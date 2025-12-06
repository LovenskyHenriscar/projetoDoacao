using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;


namespace ApiDoc.Classe
{
    [Table("Login")]
    public class Login
    {
        [Key]
        public string IdLogin { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

    }
}
