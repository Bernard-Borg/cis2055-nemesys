window.onload = function () {
    //Creates map bounds
    let topLeftCorner = L.latLng(35.90377946948467, 14.480948443757697);
    let bottomRightCorner = L.latLng(35.9009070634084, 14.485854021054182);
    let bounds = L.latLngBounds(topLeftCorner, bottomRightCorner);

    let map = L.map("map", {
        center: [35.902470, 14.483849],
        zoom: 18,
        zoomControl: false,
        maxBounds: bounds,
        maxBoundsViscosity: 1.0
    });

    map.setMinZoom(18);

    L.tileLayer(
        'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png'
    ).addTo(map);

    let marker = null;
    let latitude = $("#latitude");
    let longitude = $("#longitude");

    map.on("click", function (event) {
        if (marker != null) {
            map.removeLayer(marker);   
        }

        marker = L.marker(event.latlng, { draggable: false }).addTo(map);
        
        latitude.val(event.latlng.lat);
        longitude.val(event.latlng.lng);
    });
}