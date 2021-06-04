window.addEventListener('load', function () {
    //Creates map bounds
    let topLeftCorner = L.latLng(35.90377946948467, 14.480948443757697);
    let bottomRightCorner = L.latLng(35.9009070634084, 14.485854021054182);
    let bounds = L.latLngBounds(topLeftCorner, bottomRightCorner);

    let map = L.map("map", {
        center: L.latLng(lat, long),
        zoom: 14,
        zoomControl: false,
        dragging: false,
        keyboard: false,
        maxBoundsViscosity: 1.0
    });

    map.setMinZoom(18);

    L.tileLayer(
        'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png'
    ).addTo(map);

    marker = L.marker([lat, long], { draggable: false }).addTo(map);
});