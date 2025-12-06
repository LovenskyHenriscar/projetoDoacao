// disponiveis.js

window.addEventListener('DOMContentLoaded', () => {
  const lista            = document.getElementById('listaDoacoes');
  const alertPlaceholder = document.getElementById('alertPlaceholder');
  const apiBaseUrl       = 'https://localhost:7048';

  function showAlert(msg, type) {
    alertPlaceholder.innerHTML = `
      <div class="alert alert-${type} mt-3" role="alert">
        ${msg}
      </div>
    `;
  }

  // fallback de geocoding gratuito
  async function geocodeAddress(address) {
    const url = `https://nominatim.openstreetmap.org/search?format=json&limit=1&q=${encodeURIComponent(address)}`;
    try {
      const resp = await fetch(url, { headers: { 'Accept-Language': 'pt-BR' } });
      const results = await resp.json();
      if (results && results.length) {
        return { lat: parseFloat(results[0].lat), lng: parseFloat(results[0].lon) };
      }
    } catch (e) {
      console.error('Erro no geocoding:', e);
    }
    return null;
  }

  (async () => {
    try {
      const res = await fetch(`${apiBaseUrl}/api/doacao/disponiveis`);
      if (!res.ok) throw new Error(`${res.status} ${res.statusText}`);
      const doacoes = await res.json();

      if (!Array.isArray(doacoes) || doacoes.length === 0) {
        showAlert('Nenhum item disponível no momento.', 'info');
        return;
      }

      doacoes.forEach(item => {
        const id   = item.idDoacao || item.IdDoacao || item.id || item.Id;
        const nome = item.Nome      || item.nome     || '-';
        const tipo = item.Tipo      || item.tipo     || '-';
        const cond = item.Condicao  || item.condicao || '-';
        const qnt  = item.Quantidade != null
                     ? item.Quantidade
                     : (item.quantidade != null ? item.quantidade : '-');
        const desc = item.Descricao || item.descricao || '-';
        const data = item.DataDoacao
                     ? new Date(item.DataDoacao).toLocaleDateString()
                     : (item.dataDoacao
                        ? new Date(item.dataDoacao).toLocaleDateString()
                        : '');
        const foto = item.FotoUrl   || item.fotoUrl   || null;

        let fotoUrlFull = null;
        if (foto) {
          if (foto.startsWith('/'))        fotoUrlFull = apiBaseUrl + foto;
          else if (foto.startsWith('http')) fotoUrlFull = foto;
          else                               fotoUrlFull = `${apiBaseUrl}/uploads/${foto}`;
        }

        const card = document.createElement('div');
        card.className    = 'col-md-4 mb-4';
        card.style.cursor = 'pointer';
        card.dataset.id   = id;
        card.innerHTML    = `
          <div class="card shadow-sm h-100">
            ${fotoUrlFull ? `
              <img src="${fotoUrlFull}" class="card-img-top" alt="Foto de ${nome}" style="object-fit: cover; height: 180px;" />
            ` : ''}
            <div class="card-body">
              <h5 class="card-title">${nome}</h5>
              <p class="card-text">
                <strong>Tipo:</strong> ${tipo}<br/>
                <strong>Condição:</strong> ${cond}<br/>
                <strong>Quantidade:</strong> ${qnt}<br/>
                <strong>Descrição:</strong> ${desc}
              </p>
              ${data ? `<p><strong>Data:</strong> ${data}</p>` : ''}
            </div>
          </div>
        `;

        card.addEventListener('click', async () => {
          // abre modal
          const modalEl = document.getElementById('detailModal');
          new bootstrap.Modal(modalEl).show();

          try {
            const detailRes = await fetch(`${apiBaseUrl}/api/doacao/${id}`);
            if (!detailRes.ok) throw new Error('Erro ao carregar detalhes');
            const full = await detailRes.json();

            // preenche doador + WhatsApp
            const nomeDoador = full.DoadorNome     || full.doadorNome     || '';
            const telRaw     = full.DoadorTelefone || full.doadorTelefone || '';
            const digits     = telRaw.replace(/\D/g, '');
            const phone      = digits.startsWith('55') ? digits : '55' + digits;
            const waLink     = `https://api.whatsapp.com/send?phone=${phone}`;
            document.getElementById('modalDoadorNome').textContent = nomeDoador;
            document.getElementById('modalDoadorTelefone').innerHTML = telRaw
              ? `<a href="${waLink}" target="_blank" rel="noopener" style="display:inline-flex; align-items:center; text-decoration:none; color:inherit;"><img src="https://upload.wikimedia.org/wikipedia/commons/6/6b/WhatsApp.svg" alt="WhatsApp" style="width:20px; height:20px; margin-right:6px;"/>${telRaw}</a>`
              : '';

            // preenche retirada
            const retirarEmCasa = full.RetiradaEmCasa === true || full.retiradaEmCasa === true;
            document.getElementById('modalRetiradaInfo').textContent = retirarEmCasa
              ? 'Retirar na casa do doador'
              : 'Doador entregará pessoalmente';

            const modalEnderecoDiv = document.getElementById('modalEndereco');
            const mapDiv = document.getElementById('map');
            if (retirarEmCasa) {
              const rua    = full.RuaRetirada    || full.ruaRetirada    || '';
              const num    = full.NumeroRetirada || full.numeroRetirada || '';
              const bairro = full.BairroRetirada || full.bairroRetirada || '';
              const cidade = full.CidadeRetirada || full.cidadeRetirada || '';
              const cep    = full.CepRetirada    || full.cepRetirada    || '';
              const enderecoTexto = `${rua}, ${num} – ${bairro}, ${cidade}, CEP ${cep}`;

              // link para Google Maps
              const gmapLink = `https://www.google.com/maps/search/?api=1&query=${encodeURIComponent(enderecoTexto)}`;
              document.getElementById('modalEnderecoTexto').innerHTML = `<a href="${gmapLink}" target="_blank" rel="noopener">${enderecoTexto}</a>`;
              modalEnderecoDiv.style.display = 'block';

              // embed do mapa diretamente
              mapDiv.innerHTML = `
                <iframe
                  width="100%"
                  height="300"
                  style="border:0"
                  loading="lazy"
                  allowfullscreen
                  src="https://maps.google.com/maps?q=${encodeURIComponent(enderecoTexto)}&output=embed">
                </iframe>
              `;
              mapDiv.style.display = 'block';

            } else {
              modalEnderecoDiv.style.display = 'none';
              mapDiv.style.display = 'none';
            }

          } catch (err) {
            console.error(err);
            alert('Erro: ' + err.message);
          }
        });

        lista.appendChild(card);
      });

    } catch (err) {
      showAlert('Erro ao buscar itens: ' + err.message, 'danger');
      console.error(err);
    }
  })();
});
