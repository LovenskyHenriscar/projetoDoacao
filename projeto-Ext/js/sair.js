// js/sair.js
function logout() {
  // Limpa dados de sessão/localStorage
  sessionStorage.clear();
  localStorage.removeItem('authToken'); // ajuste conforme sua lógica

  // Redireciona para a tela de login
  window.location.href = 'login.html';
}
