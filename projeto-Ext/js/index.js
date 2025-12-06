let map, marker;

function initMap(lat, lng) {
  map = L.map('map').setView([lat, lng], 15);

  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution:
      '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
  }).addTo(map);

  marker = L.marker([lat, lng], { draggable: true }).addTo(map);

  marker.on('dragend', () => {
    const pos = marker.getLatLng();
    updateEndereco(pos.lat, pos.lng);
  });
}

function updateEndereco(lat, lng) {
  fetch(`https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat=${lat}&lon=${lng}`)
    .then((res) => res.json())
    .then((data) => {
      document.getElementById('endereco').value = data.display_name || 'Endereço não encontrado';
    })
    .catch(() => {
      document.getElementById('endereco').value = 'Erro ao buscar endereço';
    });
}

function locateUser() {
  if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(
      (position) => {
        const lat = position.coords.latitude;
        const lng = position.coords.longitude;
        initMap(lat, lng);
        updateEndereco(lat, lng);
      },
      () => {
        alert('Não foi possível obter sua localização.');
        initMap(-23.55052, -46.633308); // São Paulo
        updateEndereco(-23.55052, -46.633308);
      }
    );
  } else {
    alert('Seu navegador não suporta Geolocalização.');
    initMap(-23.55052, -46.633308);
    updateEndereco(-23.55052, -46.633308);
  }
}

document.getElementById('doacaoForm').addEventListener('submit', function (e) {
  e.preventDefault();

  const nomeDoador = document.getElementById('nomeDoador').value;
  const tipoItem = document.getElementById('tipoItem').value;
  const descricao = document.getElementById('descricao').value;
  const endereco = document.getElementById('endereco').value;
  const pos = marker.getLatLng();

  const doacao = {
    nomeDoador,
    tipoItem,
    descricao,
    endereco,
    latitude: pos.lat,
    longitude: pos.lng
  };

  console.log('Dados da doação:', doacao);

  // Aqui você faz a chamada para o backend
});
locateUser();
