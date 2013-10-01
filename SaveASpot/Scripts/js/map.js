$(function () {
    SaveASpot.Phase.Initialize();
    SaveASpot.Spot.Initialize();
    SaveASpot.SponsorSpot.Initialize();
    SaveASpot.Map.Initialize();
});

var SaveASpot = SaveASpot || {};

SaveASpot.Map = (function ($) {
    var my = {};

    my.map;
    my.geocoder;
    my.infoWindow;
    my.layers = [];
    my.Loader = null;

    my.SpotColors = { Available: "#00FF00", Selected: "#FFFF00", Unavailable: "#FF0000" };
    my.baseUrl = "/API/Map/";
    my.CartUrl = "/API/Cart/";

    my.Initialize = function () {
        // initialize map
        var mapOptions = {
            center: new google.maps.LatLng(40.58822748562923, -111.58563494682312),
            zoom: 17,
            mapTypeId: google.maps.MapTypeId.SATELLITE
        };
        my.map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);

        // initialize directions
        my.infoWindow = new google.maps.InfoWindow();

        // map loader
        my.Loader = $("#map_loading");
    };

    my.SetLoaderPercentage = function (percent) {
        my.Loader.find(".bar").css("width", percent + "%");
    };

    my.setPhase = function (id) {
        // clear layers
        my.clearOverlays();

        // find the phase
        var phase = SaveASpot.Phase.FindPhaseByID(id);

        // get settings
        //my.loadPhase(phase, true, my.showArrays);
    };

    my.setAllPhase = function () {
        // clear layers
        my.clearOverlays();

        for (var i = 0; i < SaveASpot.Phase.Phases.length; i++) {
            // get settings
            my.loadPhase(SaveASpot.Phase.Phases[i], SaveASpot.Phase.Phases[i].Active, my.loadPhaseSpots);
        }
    };

    my.setZoom = function () {
        my.map.fitBounds(my.getBounds());
    };

    my.loadPhaseSpots = function () {
        console.log("load spots");
        SaveASpot.Map.setPhase(this.phase);
        SaveASpot.SponsorSpot.LoadSpots(this.phase);
    };

    my.loadPhase = function (phase, selectable, clickEvent) {
        var color = my.pickPhaseColor(phase)
        my.processPolygonsLayers(phase, color, selectable, clickEvent);
        my.map.fitBounds(my.getBounds());
    };

    my.pickPhaseColor = function (phase) {
        if (phase.Complete) { return my.PhaseColors.Complete; }
        else if (phase.Active) { return my.PhaseColors.Active; }
        return my.PhaseColors.Default;
    };

    my.PhaseColors = { Active: "#5BA0A3", Complete: "#444444", Default: "#aaaaaa" };

    my.clearOverlays = function () {
        for (var i = 0; i < my.layers.length; i++) {
            my.layers[i].setMap(null);
        }
        my.layers = [];
    };

    my.processPolygonsLayers = function (phase, color, selectable, clickEvent) {
        if (selectable == null) { selectable = false; }
        if (phase != null) {
            for (var i = 0; i < phase.Parcels.length; i++) {
                var parcel = phase.Parcels[i];
                var coords = parcel.ParcelShape;
                //var name = parcel.ParcelName;
                //var phaseName = phase.PhaseName;

                var phaseCoords = new Array();

                for (var j = 0; j < coords.length; j++) {
                    phaseCoords.push(new google.maps.LatLng(coords[j].Longitude, coords[j].Latitude));
                }

                var spot = new google.maps.Polygon({
                    paths: phaseCoords,
                    strokeColor: color,
                    strokeOpacity: 0.50,
                    strokeWeight: 1,
                    fillColor: color,
                    fillOpacity: 0.45
                });
                spot.selected = false;
                spot.phase = phase.ID;

                spot.setMap(my.map);
                my.layers.push(spot);

                // add a listener for the click event
                if (selectable) {
                    google.maps.event.addListener(spot, 'click', clickEvent);
                }
            }
        }
    };

    my.processSpot = function (spot, color, selectable, clickEvent) {
        if (selectable == null) { selectable = false; }
        if (spot != null) {
            var coords = spot.SpotShape;
            var phaseCoords = new Array();

            for (var j = 0; j < coords.length; j++) {
                phaseCoords.push(new google.maps.LatLng(coords[j].Longitude, coords[j].Latitude));
            }

            var layer = new google.maps.Polygon({
                paths: phaseCoords,
                strokeColor: color,
                strokeOpacity: 0.50,
                strokeWeight: 1,
                fillColor: color,
                fillOpacity: 0.45
            });
            layer.selected = false;
            layer.SpotID = spot.SpotIDString;

            layer.setMap(my.map);
            my.layers.push(layer);

            // add a listener for the click event
            if (selectable) {
                google.maps.event.addListener(layer, 'click', clickEvent);
            }
        }
    };

    /** @this {google.maps.Polygon} */
    my.showArrays = function (event) {

        var color = my.selectedColor;
        if (this.selected) {
            color = my.availableColor;
        }
        this.selected = !this.selected;
        this.setOptions({
            fillColor: color,
            strokeColor: color
        });

        // Since this Polygon only has one path, we can call getPath()
        // to return the MVCArray of LatLngs
        /*var vertices = this.getPath();

        var contentString = '<b>Phase Polygons</b><br>';
        contentString += 'Clicked Location: <br>' + event.latLng.lat() + ',' + event.latLng.lng() + '<br>';

        // Iterate over the vertices.
        for (var i =0; i < vertices.getLength(); i++) {
        var xy = vertices.getAt(i);
        contentString += '<br>' + 'Coordinate: ' + i + '<br>' + xy.lat() +',' + xy.lng();
        }

        // Replace our Info Window's content and position
        infoWindow.setContent(contentString);
        infoWindow.setPosition(event.latLng);

        infoWindow.open(map);*/
    };

    my.showSpots = function (event) {
        var file = this.file.replace('.json', '_Grid.json');
        my.map.fitBounds(getPhaseBounds(this));
        my.setGrid(file);
    };

    my.getBounds = function () {
        var bounds = new google.maps.LatLngBounds();

        for (var i = 0; i < my.layers.length; i++) {
            my.layers[i].getPath().forEach(function (element, index) { bounds.extend(element); });
        }

        return bounds;
    };

    my.getPhaseBounds = function (layer) {
        var bounds = new google.maps.LatLngBounds();

        layer.getPath().forEach(function (element, index) { bounds.extend(element); });
        return bounds;
    };

    my.addToCart = function (id, phaseID) {
        var data = { id: id, phaseID: phaseID };
        $.post(my.CartUrl + "Add", data, function (result) {
            if (result.success) {
                //console.log("hooray beer added.");
            }
            else {
                //console.log("boo beer, not added.");
            }
        }, "json");
    };

    my.removeFromCart = function (id) {
        var data = { id: id };
        $.post(my.CartUrl + "Remove", data, function (result) {
            if (result.success) {
                //console.log("hooray beer removed.");
            }
            else {
                //console.log("boo beer, not removed.");
            }
        }, "json");
    };

    return my;
} (jQuery));