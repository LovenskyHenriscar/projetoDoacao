using Microsoft.AspNetCore.Http;
using System;

namespace Projeto_de_Doacao.Dtos
{
    public class DoacaoCreateDto
    {
        public long UsuarioId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Condicao { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public DateTime? Validade { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public DateTime? DataDoacao { get; set; }

        // Novos campos do doador
        public string DoadorNome { get; set; } = string.Empty;
        public string DoadorTelefone { get; set; } = string.Empty;

        // Endereço para entrega, opcional se RetiradaEmCasa for true
        public string? Endereco { get; set; }

        // Foto opcional
        public IFormFile? Foto { get; set; }

        // Retirada em casa?
        public bool RetiradaEmCasa { get; set; }

        // Campos de endereço de retirada, opcionais
        public string? RuaRetirada { get; set; }
        public string? NumeroRetirada { get; set; }
        public string? BairroRetirada { get; set; }
        public string? CidadeRetirada { get; set; }
        public string? CepRetirada { get; set; }
    }
}
