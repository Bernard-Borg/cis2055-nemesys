﻿window.onload = function () {
    let latitude = $("#latitude");
    let longitude = $("#longitude");

    //Hides the input labels when JS is enabled
    $("#latitude-label").hide();
    $("#longitude-label").hide();

    //Sets the type of the input fields to hidden when JS is enabled
    latitude.attr("type", "hidden");
    longitude.attr("type", "hidden");

    //Creates map bounds
    let topLeftCorner = L.latLng(35.90377946948467, 14.480948443757697);
    let bottomRightCorner = L.latLng(35.9009070634084, 14.485854021054182);
    let bounds = L.latLngBounds(topLeftCorner, bottomRightCorner);

    //Sets the height of the map div when JS is enabled
    $("#map").css("height", "400px");

    let map = L.map("map", {
        center: [35.902470, 14.483849],
        zoom: 18,
        zoomControl: false,
        maxBounds: bounds,
        maxBoundsViscosity: 1.0
    });

    map.setMinZoom(18);

    //Map does not show up without the tile layer
    L.tileLayer(
        'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png'
    ).addTo(map);

    let marker = null;
    
    map.on("click", function (event) {
        //Disallows multiple markers
        if (marker != null) {
            map.removeLayer(marker);   
        }

        //Creates the marker at the location where the user clicked
        marker = L.marker(event.latlng, { draggable: false }).addTo(map);

        //Sets the input values to be sent to server
        latitude.val(event.latlng.lat);
        longitude.val(event.latlng.lng);
    });
}