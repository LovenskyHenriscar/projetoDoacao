// js/doacao.js

document.addEventListener('DOMContentLoaded', () => {
  // Elementos principais
  const tipoItem = document.getElementById("tipoItem");
  const camposDinamicos = document.getElementById("camposDinamicos");
  const form = document.getElementById("doacaoForm");

  // Garante estado inicial limpo
  tipoItem.value = "";
  camposDinamicos.innerHTML = "";

  // ----- Campo de retirada em casa -----
  const divRetirada = document.createElement("div");
  divRetirada.className = "mb-3";
  const labelRetirada = document.createElement("label");
  labelRetirada.className = "form-label";
  labelRetirada.htmlFor = "retiradaEmCasa";
  labelRetirada.textContent = "Deseja retirada do item na sua casa?";
  const retiradaSelect = document.createElement("select");
  retiradaSelect.className = "form-select";
  retiradaSelect.id = "retiradaEmCasa";
  retiradaSelect.name = "RetiradaEmCasa";
  retiradaSelect.innerHTML = `
    <option value="false">Não, vou entregar</option>
    <option value="true">Sim, retirar na minha casa</option>
  `;
  divRetirada.append(labelRetirada, retiradaSelect);

  // ----- Campos extras de endereço -----
  const enderecosExtras = document.createElement("div");
  enderecosExtras.id = "enderecosExtras";
  enderecosExtras.style.display = "none";
  enderecosExtras.innerHTML = `
    <div class="mb-3">
      <label for="rua" class="form-label">Rua</label>
      <input type="text" class="form-control" id="rua" name="RuaRetirada" placeholder="Digite a rua" />
    </div>
    <div class="mb-3">
      <label for="numero" class="form-label">Número</label>
      <input type="text" class="form-control" id="numero" name="NumeroRetirada" placeholder="Digite o número" />
    </div>
    <div class="mb-3">
      <label for="bairro" class="form-label">Bairro</label>
      <input type="text" class="form-control" id="bairro" name="BairroRetirada" placeholder="Digite o bairro" />
    </div>
    <div class="mb-3">
      <label for="cidade" class="form-label">Cidade</label>
      <input type="text" class="form-control" id="cidade" name="CidadeRetirada" placeholder="Digite a cidade" />
    </div>
    <div class="mb-3">
      <label for="cep" class="form-label">CEP</label>
      <input type="text" class="form-control" id="cep" name="CepRetirada" placeholder="Digite o CEP" />
    </div>
  `;

  // Insere campos de retirada e endereço após o input de foto
  const fotoInput = document.getElementById("foto");
  fotoInput.parentNode.insertBefore(divRetirada, fotoInput.nextSibling);
  fotoInput.parentNode.insertBefore(enderecosExtras, divRetirada.nextSibling);

  // Atualiza visibilidade e obrigatoriedade dos campos de endereço
  function atualizarVisibilidadeEndereco() {
    const retirar = retiradaSelect.value === "true";
    enderecosExtras.style.display = retirar ? "block" : "none";
    ["rua", "numero", "bairro", "cidade", "cep"].forEach(id => {
      const el = document.getElementById(id);
      if (el) {
        el.required = retirar;
        if (!retirar) el.value = "";
      }
    });
  }
  retiradaSelect.addEventListener("change", atualizarVisibilidadeEndereco);
  atualizarVisibilidadeEndereco();

  // Autocomplete via CEP
  document.getElementById("cep").addEventListener("input", async function() {
    const cep = this.value.replace(/\D/g, "");
    if (cep.length === 8) {
      try {
        const resp = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
        if (!resp.ok) throw new Error("Erro na requisição do CEP");
        const data = await resp.json();
        if (data.erro) throw new Error("CEP não encontrado");
        document.getElementById("rua").value = data.logradouro || "";
        document.getElementById("bairro").value = data.bairro || "";
        document.getElementById("cidade").value = data.localidade || "";
      } catch (err) {
        alert(err.message);
      }
    }
  });

  // ----- Função que cria campos dinâmicos por categoria -----
  function mkField(labelText, type, id, placeholder = "", options = []) {
    const div = document.createElement("div");
    div.className = "mb-3";
    const lbl = document.createElement("label");
    lbl.className = "form-label";
    lbl.htmlFor = id;
    lbl.textContent = labelText;

    let input;
    if (type === "select") {
      input = document.createElement("select");
      input.className = "form-select";
      input.id = id;
      input.name = id;
      options.forEach(o => {
        const opt = document.createElement("option");
        opt.value = o.value;
        opt.textContent = o.label;
        input.appendChild(opt);
      });
    } else {
      input = document.createElement(type === "textarea" ? "textarea" : "input");
      if (type !== "textarea") input.type = type;
      input.className = "form-control";
      input.id = id;
      input.name = id;
      if (placeholder) input.placeholder = placeholder;
    }

    div.append(lbl, input);
    return div;
  }

  // ----- Monta campos de acordo com a categoria selecionada -----
  tipoItem.addEventListener("change", () => {
    const tipo = tipoItem.value;
    camposDinamicos.innerHTML = "";

    // só processa se for uma categoria válida
    if (!tipo) return;

    switch (tipo) {
      case "Alimento":
        camposDinamicos.append(
          mkField("Nome do Alimento", "text", "Nome", "Ex: Arroz, Feijão..."),
          mkField("Validade (mínimo 2 meses)", "date", "Validade"),
          mkField("Quantidade", "number", "Quantidade", "Ex: 2 (kg ou un.)")
        );
        break;
      case "Roupa":
        camposDinamicos.append(
          mkField("Tipo de Roupa", "text", "Nome", "Ex: Camiseta, Calça..."),
          mkField("Quantidade", "number", "Quantidade", "Ex: 1, 2..."),
          mkField("Condição da Roupa", "select", "Condicao", "",
            [{ value: "Novo", label: "Novo" }, { value: "Usado", label: "Usado" }]
          ),
          mkField("Data da Doação", "date", "DataDoacao")
        );
        break;
      case "Eletrônico":
        camposDinamicos.append(
          mkField("Nome do Componente", "text", "Nome", "Ex: Placa de Vídeo..."),
          mkField("Condição", "select", "Condicao", "",
            [{ value: "Novo", label: "Novo" }, { value: "Usado", label: "Usado" }]
          ),
          mkField("Motivo da Doação", "text", "Motivo", "Por que está doando?"),
          mkField("Quantidade", "number", "Quantidade", "Ex: 1, 2...")
        );
        break;
      case "Movel":
        camposDinamicos.append(
          mkField("Tipo de Móvel", "text", "Nome", "Ex: Mesa, Cadeira..."),
          mkField("Descrição do Móvel", "text", "Descricao", "Ex: Em bom estado"),
          mkField("Quantidade", "number", "Quantidade", "Ex: 1"),
          mkField("Condição", "select", "Condicao", "",
            [{ value: "Novo", label: "Novo" }, { value: "Usado", label: "Usado" }]
          )
        );
        break;
      case "Livro":
        camposDinamicos.append(
          mkField("Título do Livro", "text", "Nome", "Ex: O Pequeno Príncipe"),
          mkField("Autor do Livro", "text", "Descricao", "Ex: Antoine de Saint-Exupéry"),
          mkField("Condição", "select", "Condicao", "",
            [
              { value: "novo", label: "Novo" },
              { value: "usado_bom", label: "Usado (Bom)" },
              { value: "usado_medio", label: "Usado (Marcas)" },
              { value: "danificado", label: "Danificado" }
            ]
          ),
          mkField("Quantidade", "number", "Quantidade")
        );
        break;
      case "Brinquedo":
        camposDinamicos.append(
          mkField("Nome do Brinquedo", "text", "Nome", "Ex: Carrinho, Boneca..."),
          mkField("Condição", "select", "Condicao", "",
            [{ value: "novo", label: "Novo" }, { value: "usado", label: "Usado" }]
          ),
          mkField("Quantidade", "number", "Quantidade")
        );
        break;
      case "Higiene":
        camposDinamicos.append(
          mkField("Tipo de Produto", "text", "Nome", "Ex: Sabonete, Shampoo..."),
          mkField("Condição", "select", "Condicao", "",
            [{ value: "sim", label: "Sim, lacrado" }, { value: "nao", label: "Não, aberto" }]
          ),
          mkField("Quantidade", "number", "Quantidade"),
          mkField("Validade", "date", "Validade")
        );
        break;
      default:
        // sem campos adicionais
        break;
    }
  });

  // ----- Envio do formulário -----
  form.addEventListener("submit", async (e) => {
    e.preventDefault();

    // verifica usuário logado
    const usuarioId = localStorage.getItem("usuarioLogadoId");
    if (!usuarioId) {
      alert("Faça login antes de cadastrar uma doação.");
      return window.location.href = "login.html";
    }

    // Monta FormData incluindo DoadorNome e DoadorTelefone vindos dos inputs estáticos
    const formData = new FormData(form);
    formData.set("UsuarioId", usuarioId);

    // DEBUG: log dos campos enviados,
    for (const [chave, valor] of formData.entries()) {
      console.log(chave, valor);
    }

    try {
      const res = await fetch("https://localhost:7048/api/doacao", {
        method: "POST",
        body: formData
      });
      if (!res.ok) {
        const errJson = await res.json().catch(() => null);
        const msg = errJson?.mensagem || JSON.stringify(errJson) || res.statusText;
        throw new Error(msg);
      }
      alert("Doação cadastrada com sucesso!");
      form.reset();
      camposDinamicos.innerHTML = "";
      atualizarVisibilidadeEndereco();
    } catch (err) {
      console.error("Erro no cadastro de doação:", err);
      alert("Erro ao cadastrar: " + err.message);
    }
  });
});
