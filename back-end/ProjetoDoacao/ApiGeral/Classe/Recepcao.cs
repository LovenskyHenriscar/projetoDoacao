using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace ApiGeral.Classe
{
    [Table("Recepcao")]
    public class Recepcao
    {
        private Int64 idRecepcao;
        private string nome;
        private DateTime dataSolicitacao;
        private decimal quantidade;
        private Usuario usuario;
        private List<ItensRecepcao> recepcaos;

        public long IdRecepcao { get => idRecepcao; set => idRecepcao = value; }
        public string Nome { get => nome; set => nome = value; }
        public DateTime DataSolicitacao { get => dataSolicitacao; set => dataSolicitacao = value; }
        public decimal Quantidade { get => quantidade; set => quantidade = value; }
        public Usuario Usuario { get => usuario; set => usuario = value; }
        public List<ItensRecepcao> Recepcaos { get => recepcaos; set => recepcaos = value; }
    }
}
