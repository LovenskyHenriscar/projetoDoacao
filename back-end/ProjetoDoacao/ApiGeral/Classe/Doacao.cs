using ApiGeral.Classe;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace Projeto_de_Doacao.Models
{
    [Table("Doacao")]
    public class Doacao
    {
        [Key]
        public long IdDoacao { get; set; }

        private long usuarioId;
        private string tipo = string.Empty;
        private string nome = string.Empty;
        private string condicao = string.Empty;
        private string descricao = string.Empty;
        private int quantidade;
        private DateTime? validade;
        private string motivo = string.Empty;
        private DateTime? dataDoacao;
        // Novo campo para data da última atualização (decremento de quantidade)
        private DateTime? dataAtualizacao;
        private string endereco = string.Empty;
        private bool retiradaEmCasa;
        private string ruaRetirada = string.Empty;
        private string numeroRetirada = string.Empty;
        private string bairroRetirada = string.Empty;
        private string cidadeRetirada = string.Empty;
        private string cepRetirada = string.Empty;
        private double? latitude;
        private double? longitude;
        private string fotoUrl = string.Empty;
        private string status = "disponivel";

        // Novos campos para o doador
        private string doadorNome = string.Empty;
        private string doadorTelefone = string.Empty;

        private Doador doador = new Doador();
        private List<ItemProd> itemProds = new List<ItemProd>();

        public long UsuarioId
        {
            get => usuarioId;
            set => usuarioId = value;
        }

        public string Tipo
        {
            get => tipo;
            set => tipo = value ?? string.Empty;
        }

        public string Nome
        {
            get => nome;
            set => nome = value ?? string.Empty;
        }

        public string Condicao
        {
            get => condicao;
            set => condicao = value ?? string.Empty;
        }

        public string Descricao
        {
            get => descricao;
            set => descricao = value ?? string.Empty;
        }

        public int Quantidade
        {
            get => quantidade;
            set => quantidade = value;
        }

        public DateTime? Validade
        {
            get => validade;
            set => validade = value;
        }

        public string Motivo
        {
            get => motivo;
            set => motivo = value ?? string.Empty;
        }

        public DateTime? DataDoacao
        {
            get => dataDoacao;
            set => dataDoacao = value;
        }

        // Propriedade para data da última atualização
        public DateTime? DataAtualizacao
        {
            get => dataAtualizacao;
            set => dataAtualizacao = value;
        }

        public string Endereco
        {
            get => endereco;
            set => endereco = value ?? string.Empty;
        }

        public bool RetiradaEmCasa
        {
            get => retiradaEmCasa;
            set => retiradaEmCasa = value;
        }

        public string RuaRetirada
        {
            get => ruaRetirada;
            set => ruaRetirada = value ?? string.Empty;
        }

        public string NumeroRetirada
        {
            get => numeroRetirada;
            set => numeroRetirada = value ?? string.Empty;
        }

        public string BairroRetirada
        {
            get => bairroRetirada;
            set => bairroRetirada = value ?? string.Empty;
        }

        public string CidadeRetirada
        {
            get => cidadeRetirada;
            set => cidadeRetirada = value ?? string.Empty;
        }

        public string CepRetirada
        {
            get => cepRetirada;
            set => cepRetirada = value ?? string.Empty;
        }

        public double? Latitude
        {
            get => latitude;
            set => latitude = value;
        }

        public double? Longitude
        {
            get => longitude;
            set => longitude = value;
        }

        public string FotoUrl
        {
            get => fotoUrl;
            set => fotoUrl = value ?? string.Empty;
        }

        public string Status
        {
            get => status;
            set => status = value ?? "disponivel";
        }

        // Propriedade do novo campo DoadorNome
        public string DoadorNome
        {
            get => doadorNome;
            set => doadorNome = value ?? string.Empty;
        }

        // Propriedade do novo campo DoadorTelefone
        public string DoadorTelefone
        {
            get => doadorTelefone;
            set => doadorTelefone = value ?? string.Empty;
        }

        [Computed]
        public Doador Doador
        {
            get => doador;
            set => doador = value ?? new Doador();
        }

        [Computed]
        public List<ItemProd> ItemProds
        {
            get => itemProds;
            set => itemProds = value ?? new List<ItemProd>();
        }
    }
}
