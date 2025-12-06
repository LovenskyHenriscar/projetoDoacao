document.getElementById("formCadastro").addEventListener("submit", async function (e) {
    e.preventDefault();

    const nome = document.getElementById("cadNome").value.trim();
    const email = document.getElementById("cadEmail").value.trim();
    const senha = document.getElementById("cadSenha").value.trim();
    const confirmarSenha = document.getElementById("cadConfirmarSenha").value.trim();
    const telefone = document.getElementById("cadTelefone").value.trim();
    const endereco = document.getElementById("cadEndereco").value.trim();
    const cpfCnpj = document.getElementById("cadCpfCnpj").value.trim();
    const tipoPessoa = document.getElementById("cadTipoPessoa").value;

    // Verifica se as senhas coincidem
    if (senha !== confirmarSenha) {
        alert("As senhas não coincidem!");
        return;
    }

    // Validação de CPF ou CNPJ
    if (!validarCpfCnpj(cpfCnpj)) {
        alert("CPF ou CNPJ inválido!");
        return;
    }

    const usuario = {
        nome,
        email,
        senha,
        telefone,
        endereco,
        cpfCnpj,
        tipoPessoa
    };

    try {
        const response = await fetch("https://localhost:7048/api/usuario/cadastrar", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(usuario),
        });

        if (!response.ok) {
            const error = await response.json();
            alert("Erro: " + error.mensagem);
            return;
        }

        alert("Cadastro realizado com sucesso!");
        window.location.href = "login.html";
    } catch (err) {
        alert("Erro ao conectar com o servidor.");
    }
});