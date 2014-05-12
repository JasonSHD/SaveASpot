$(function () {
    SaveASpot.Phase.Initialize();
    //SaveASpot.Spot.Initialize();
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
    my.phaseID = null;
    my.SponsorMarker = null;

    my.SpotColors = { Available: "#00FF00", Selected: "#FFFF00", Unavailable: "#FF0000" };
    my.baseUrl = "/API/Map/";
    my.CartUrl = "/API/Cart/";

    my.Initialize = function () {
        // initialize map
        var mapOptions = {
            center: new google.maps.LatLng(40.58822748562923, -111.58563494682312),
            zoom: 17,
            mapTypeId: google.maps.MapTypeId.TERRAIN
        };
        my.map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);

        if (!SaveASpot.IsSponsor) {
            google.maps.event.addListener(my.map, 'dragend', my.changeMapBounds);
            google.maps.event.addListener(my.map, 'zoom_changed', my.changeMapBounds);
        }

        // initialize directions
        my.infoWindow = new google.maps.InfoWindow();

        // map loader
        my.Loader = $("#map_loading");

        $(".project-btn").click(my.addRandomToCart);
    };

    my.SetLoaderPercentage = function (percent) {
        my.Loader.find(".bar").css("width", percent + "%");
    };

    my.setPhase = function (id) {
        // clear layers
        my.clearOverlays();

        // find the phase
        var phase = SaveASpot.Phase.FindPhaseByID(id);
        my.phaseID = id;

        // get settings
        my.loadPhase(phase, true, null);
    };

    my.setAllPhase = function () {
        // clear layers
        my.clearOverlays();

        for (var i = 0; i < SaveASpot.Phase.Phases.length; i++) {
            // get settings
            my.loadPhase(SaveASpot.Phase.Phases[i], SaveASpot.Phase.Phases[i].Active, my.loadPhaseSpots);
        }
    };

    my.setSponsors = function (sponsorID) {
        for (var i = 0; i < SaveASpot.Phase.Sponsors.length; i++) {
            var sponsor = SaveASpot.Phase.Sponsors[i];

            if (sponsor.SponsorID == sponsorID) {
                var coord = new google.maps.LatLng(sponsor.Center.Longitude, sponsor.Center.Latitude);
                var marker = new google.maps.Marker({
                    position: coord,
                    icon: {
                        url: sponsor.ImageUrl,
                        size: new google.maps.Size(150, 75),
                        origin: new google.maps.Point(0, 0),
                        anchor: new google.maps.Point(0, 0),
                        scaledSize: new google.maps.Size(150, 75)
                    },
                    title: sponsor.Name
                });

                if (my.SponsorMarker != null) { my.SponsorMarker.setMap(null); }

                marker.setMap(my.map);
                my.SponsorMarker = marker;
                my.layers.push(marker);
                break;
            }
        }
    };

    my.setZoom = function () {
        my.map.fitBounds(my.getBounds());
    };

    my.loadPhaseSpots = function () {
        my.clearOverlays();
        SaveASpot.Map.setPhase(this.phase);
        SaveASpot.Phase.DisplaySponsors(this.phase);
        if (SaveASpot.IsSponsor) {
            SaveASpot.SponsorSpot.LoadSpots(this.phase);
        }
        else {
            //SaveASpot.Spot.LoadSpots(this.phase);
        }
        /*for (var i = 0; i < SaveASpot.Phase.Phases.length; i++) {
        // get settings
        my.loadPhase(SaveASpot.Phase.Phases[i], SaveASpot.Phase.Phases[i].Active, my.loadPhaseSpots);
        }*/
    };

    my.loadPhase = function (phase, selectable, clickEvent) {
        var color = my.pickPhaseColor(phase)
        var layer = my.processPolygonsLayers(phase, color, selectable, clickEvent);

        // set active marker
        if (phase.Active) {
            var bounds = my.getPhaseBounds(layer);
            my.map.fitBounds(bounds);
            var center = bounds.getCenter();

            var marker = new google.maps.Marker({
                position: center,
                icon: "/Content/img/active.png",
                title: "Active"
            });

            marker.setMap(my.map);
            my.layers.push(marker);
        }
    };

    my.pickPhaseColor = function (phase) {
        if (phase.Complete) { return my.PhaseColors.Complete; }
        else if (phase.Active) { return my.PhaseColors.Active; }
        return my.PhaseColors.Default;
    };

    my.PhaseColors = { Active: "#60D4F7", Complete: "#444444", Default: "#aaaaaa" };

    my.clearOverlays = function () {
        for (var i = 0; i < my.layers.length; i++) {
            my.layers[i].setMap(null);
        }
        my.layers = [];
    };

    my.changeMapBounds = function () {
        var bounds = my.map.getBounds();

        var northEast = bounds.getNorthEast();
        var southWest = bounds.getSouthWest();

        if (northEast && southWest && my.phaseID) {

            // SaveASpot.Spot.LoadSpots(my.phaseID, northEast, southWest);
        }
    };

    my.processPolygonsLayers = function (phase, color, selectable, clickEvent) {
        var spot = null;
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

                spot = new google.maps.Polygon({
                    paths: phaseCoords,
                    strokeColor: color,
                    strokeOpacity: 1,
                    strokeWeight: 2,
                    fillColor: color,
                    fillOpacity: 0.75,
                    zIndex: 1
                });
                spot.selected = false;
                spot.phase = phase.ID;

                spot.setMap(my.map);
                my.layers.push(spot);

                // add a listener for the click event
                if (selectable && clickEvent) {
                    google.maps.event.addListener(spot, 'click', clickEvent);
                }
            }
        }

        return spot;
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
                fillOpacity: 0.45,
                zIndex: 2
            });
            layer.selected = false;
            layer.SpotID = spot.SpotIDString;

            layer.setMap(my.map);
            my.layers.push(layer);

            // add a listener for the click event
            if (selectable && clickEvent) {
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
    };

    my.showSpots = function (event) {
        var file = this.file.replace('.json', '_Grid.json');
        my.map.fitBounds(getPhaseBounds(this));
        my.setGrid(file);
    };

    my.getBounds = function () {
        var bounds = new google.maps.LatLngBounds();

        for (var i = 0; i < my.layers.length; i++) {
            if (my.layers[i].getPath != null) {
                my.layers[i].getPath().forEach(function (element, index) { bounds.extend(element); });
            }
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

    my.addRandomToCart = function () {
        var data = {
            sponsorID: $(".sponsor-list").val(),
            qty: $(".qty").val() 
        };
        $.post(my.CartUrl + "AddItems", data, function (result) {
            if (result.success) {
                $(".count").html(result.quantity);
            }
            else {
                alert("There was an issue adding spots to the cart.");
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