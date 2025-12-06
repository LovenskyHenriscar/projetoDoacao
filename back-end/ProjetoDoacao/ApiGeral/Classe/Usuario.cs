using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace ApiGeral.Classe
{

    [Table("Usuario")]
    public class Usuario
    {
        private Int64 idUsuario;
        private string nome;
        private string email;
        private string senha;
        private string telefone;
        private string endereco;
        private string cpfCnpj;
        private string tipoPessoa;
        private Doador doador;
        private List<Recepcao> recepcaos;

        public long IdUsuario { get => idUsuario; set => idUsuario = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Email { get => email; set => email = value; }
        public string Senha { get => senha; set => senha = value; }
        public string Telefone { get => telefone; set => telefone = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public string CpfCnpj { get => cpfCnpj; set => cpfCnpj = value; }
        public Doador Doador { get => doador; set => doador = value; }
        public List<Recepcao> Recepcaos { get => recepcaos; set => recepcaos = value; }
        public string TipoPessoa { get => tipoPessoa; set => tipoPessoa = value; }
    }
}
