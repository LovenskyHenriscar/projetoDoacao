using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace ApiGeral.Classe
{

    [Table("Login")]
    public class Login
    {

        private string idLogin;
        private string email;
        private string senha;

        public string IdLogin { get => idLogin; set => idLogin = value; }
        public string Email { get => email; set => email = value; }
        public string Senha { get => senha; set => senha = value; }
    }
}
