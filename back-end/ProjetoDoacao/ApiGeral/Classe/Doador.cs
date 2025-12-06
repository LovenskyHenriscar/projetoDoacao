using Dapper.Contrib.Extensions;
using Projeto_de_Doacao.Models;   // onde estão Doacao e Usuario
using System.Collections.Generic;

namespace ApiGeral.Classe
{
    [Table("Doador")]
    public class Doador
    {
        [Key]
        public long IdDoador { get; set; }

        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;

        [Computed]
        public Usuario Usuario { get; set; } = new Usuario();

        [Computed]
        public List<Doacao> Doacaolist { get; set; } = new List<Doacao>();
    }
}
