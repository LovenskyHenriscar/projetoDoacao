window.addEventListener('DOMContentLoaded', loadHistorico);

async function loadHistorico() {
  const tabelaBody      = document.querySelector('#tabelaDoacoes tbody');
  const alertPlaceholder = document.getElementById('alertPlaceholder');
  const apiBase         = 'https://localhost:7048/api/doacao';
  const usuarioId       = localStorage.getItem('usuarioLogadoId');

  if (!usuarioId) {
    return showAlert('Você precisa fazer login para ver seu histórico.', 'warning');
  }

  try {
    const res = await fetch(`${apiBase}/historico/${usuarioId}`);
    if (!res.ok) throw new Error(`HTTP ${res.status}`);
    const doacoes = await res.json();
    if (doacoes.length === 0) {
      return showAlert('Você ainda não cadastrou nenhuma doação.', 'info');
    }
    tabelaBody.innerHTML = '';
    doacoes.forEach(d => popularLinha(d, tabelaBody));
  } catch (err) {
    console.error(err);
    showAlert('Erro ao carregar histórico: ' + err.message, 'danger');
  }

  function popularLinha(d, tbody) {
    const tr = document.createElement('tr');
    const tdNome = document.createElement('td');
    tdNome.textContent = d.nome ?? '-';

    const tdQuantidade = document.createElement('td');
    tdQuantidade.textContent = d.quantidade;

    const tdData = document.createElement('td');
    tdData.textContent = d.dataDoacao
      ? new Date(d.dataDoacao).toLocaleDateString()
      : '-';

    const tdAcoes = document.createElement('td');
    tdAcoes.classList.add('text-center');
    tdAcoes.innerHTML = `
      <button class="btn btn-sm btn-success me-2 btn-doado" title="Marcar como doado">
        <i class="bi bi-check2-circle"></i>
      </button>
      <button class="btn btn-sm btn-outline-danger btn-excluir" title="Excluir">
        <i class="bi bi-trash"></i>
      </button>
    `;

    tr.append(tdNome, tdQuantidade, tdData, tdAcoes);

    if (d.quantidade === 0) {
      tr.classList.add('text-decoration-line-through', 'table-secondary');
      tr.querySelector('.btn-doado').disabled = true;
    }

    tbody.appendChild(tr);

    const id           = d.idDoacao ?? d.id;
    const btnDoado     = tr.querySelector('.btn-doado');
    const btnExcluir   = tr.querySelector('.btn-excluir');
    const rotaMarcar   = `${apiBase}/disponiveis/marcar-doado/${id}`;
    const rotaExcluir  = `${apiBase}/${id}`;

    btnDoado.addEventListener('click', async () => {
      if (!confirm('Marcar 1 unidade como doada?')) return;
      try {
        const r = await fetch(rotaMarcar, {
          method: 'PATCH',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ quantidade: 1 })
        });
        if (r.status === 404) {
          return showAlert('Rota de marcação não encontrada (404).', 'warning');
        }
        if (r.status === 405) {
          return showAlert('Método não permitido (405).', 'warning');
        }
        if (!r.ok) throw new Error(`HTTP ${r.status}`);

        let current = parseInt(tdQuantidade.textContent, 10);
        const updated = Math.max(current - 1, 0);
        tdQuantidade.textContent = updated;

        if (updated === 0) {
          tr.classList.add('text-decoration-line-through', 'table-secondary');
          btnDoado.disabled = true;
        }
        showAlert('Doação marcada: 1 unidade decrementada.', 'success');
      } catch (e) {
        console.error(e);
        showAlert('Erro ao marcar como doado: ' + e.message, 'danger');
      }
    });

    btnExcluir.addEventListener('click', async () => {
      if (!confirm('Deseja excluir esta doação?')) return;
      try {
        const r = await fetch(rotaExcluir, { method: 'DELETE' });
        if (r.status === 404) {
          return showAlert('Doação não encontrada (404).', 'warning');
        }
        if (r.status === 405) {
          return showAlert('Método não permitido na exclusão (405).', 'warning');
        }
        if (!r.ok) throw new Error(`HTTP ${r.status}`);
        tr.remove();
        showAlert('Doação excluída com sucesso.', 'success');
      } catch (e) {
        console.error(e);
        showAlert('Erro ao excluir: ' + e.message, 'danger');
      }
    });
  }

  function showAlert(msg, type) {
    alertPlaceholder.innerHTML = `
      <div class="alert alert-${type}" role="alert">
        ${msg}
      </div>
    `;
  }
}
