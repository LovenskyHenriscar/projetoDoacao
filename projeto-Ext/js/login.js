// login.js

document.getElementById("formLogin").addEventListener("submit", async function (e) {
  e.preventDefault();

  const email = document.getElementById("loginEmail").value.trim();
  const senha = document.getElementById("loginSenha").value.trim();

  if (!email || !senha) {
    alert("Preencha todos os campos.");
    return;
  }

  try {
    // 1) Faz a requisição de login
    const response = await fetch("https://localhost:7048/api/login", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email, senha }),
    });

    // 2) Log do status HTTP
    console.log("👉 Status de resposta:", response.status, response.statusText);

    // 3) Log do corpo bruto (antes de JSON.parse)
    const textoBruto = await response.clone().text();
    console.log("👉 Body bruto   :", textoBruto);

    // 4) Tenta converter para JSON e logar
    let data;
    try {
      data = await response.json();
      console.log("👉 Body JSON    :", data);
    } catch (e) {
      console.error("❌ Não foi possível converter para JSON:", e);
      alert("Resposta inválida do servidor.");
      return;
    }

    // 5) Valida se o status não é OK
    if (!response.ok) {
      alert("Erro: " + (data.mensagem || "Erro desconhecido"));
      return;
    }

    // 6) Verifica campo de ID (ajuste para usar 'id' conforme seu backend retorna)
    if (!data.id) {
      alert("Resposta do servidor não contém o ID do usuário.");
      return;
    }

    // 7) Armazena o ID do usuário logado
    localStorage.setItem("usuarioLogadoId", data.id);

    alert(data.mensagem || "Login realizado com sucesso!");
    window.location.href = "doar.html";

  } catch (err) {
    console.error("Falha no login:", err);
    alert("Erro ao conectar com o servidor: " + err.message);
  }
});
