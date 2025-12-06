const frases = [
  "Doe amor. Doe esperança.",
  "Pequenas ações, grandes mudanças.",
  "A solidariedade é o maior presente que podemos dar.",
  "Transforme o mundo com sua generosidade."
];

const motivacao = document.getElementById('motivacao');
let index = 0;

function mostrarFrase() {
  motivacao.style.opacity = 0;
  setTimeout(() => {
    motivacao.textContent = frases[index];
    motivacao.style.opacity = 1;
    index = (index + 1) % frases.length;
  }, 500);
}

mostrarFrase();
setInterval(mostrarFrase, 4000);
