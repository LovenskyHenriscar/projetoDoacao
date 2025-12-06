using ApiGeral.Classe;
using Microsoft.AspNetCore.Mvc;
using Projeto_de_Doacao.Repository;
using System.Collections.Generic;

namespace Projeto_de_Doacao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemProdController : ControllerBase
    {
        private readonly ItemProdRepository _repo;

        public ItemProdController(ItemProdRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public IActionResult Cadastrar([FromBody] ItemProd item)
        {
            if (item == null || item.Quantidade <= 0)
                return BadRequest("Dados inválidos ou quantidade nula.");

            var id = _repo.Inserir(item);
            return CreatedAtAction(nameof(Obter), new { id }, new
            {
                id,
                mensagem = "Item cadastrado com sucesso!"
            });
        }

        [HttpGet("{id}")]
        public IActionResult Obter(long id)
        {
            var item = _repo.Obter(id);
            if (item == null)
                return NotFound("Item não encontrado.");

            return Ok(item);
        }

        [HttpGet]
        public IActionResult Listar()
        {
            var lista = _repo.Listar();
            return Ok(lista);
        }

        // REMOVIDO: [HttpGet("doacao/{doacaoId}")] e o método ListarPorDoacao

        [HttpPut]
        public IActionResult Atualizar([FromBody] ItemProd item)
        {
            if (item == null || item.IdItemProd == 0)
                return BadRequest("Item inválido.");

            var sucesso = _repo.Alterar(item);
            if (!sucesso)
                return NotFound("Erro ao atualizar o item.");

            return Ok("Item atualizado com sucesso!");
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(long id)
        {
            var sucesso = _repo.Excluir(id);
            if (!sucesso)
                return NotFound("Erro ao excluir o item.");

            return Ok("Item excluído com sucesso!");
        }
    }
}
