var map, directionService, directionRenderer, geocoder, infoWindow;

$(function () {
    // initialize map
    var mapOptions = {
        center: new google.maps.LatLng(40.58822748562923,-111.58563494682312),
        zoom: 17,
        mapTypeId: google.maps.MapTypeId.SATELLITE
    };
    map = new google.maps.Map(document.getElementById("map"),
      mapOptions);

    // initialize directions
    directionService = new google.maps.DirectionsService();
    directionRenderer = new google.maps.DirectionsRenderer();
    directionRenderer.setMap(map);
    geocoder = new google.maps.Geocoder();
    infoWindow = new google.maps.InfoWindow();


    // compute the distance
    google.maps.event.addListener(directionRenderer, 'directions_changed', function () {
        computeTotalDistance(directionRenderer.directions);
    });

    // get settings
    $.post('data/Phase1.json', function(result) {
      processPolygonsLayers(result, '#FF0000');
    }, 'json');

    // get settings
    $.post('data/Phase1_Grid.json', function(result) {        
      processPolygonsLayers(result, '#00FF00');
    }, 'json');

});

function processPolygonsLayers(result, color) {
      if(result != null) {
        for(var i = 0; i < result.features.length; i++) {
          var feature = result.features[i];
          var coords = feature.geometry.coordinates[0];
          var name = feature.properties.Name;
          var phase = feature.properties.Phase;

          var phaseCoords = new Array();

          for(var j = 0; j < coords.length; j++) {
            phaseCoords.push(new google.maps.LatLng(coords[j][1],coords[j][0]));
          }

          var spot = new google.maps.Polygon({
            paths: phaseCoords,
            strokeColor: color,
            strokeOpacity: 0.5,
            strokeWeight: 1,
            fillColor: color,
            fillOpacity: 0.35
          });

          spot.setMap(map);

          // add a listener for the click event
          google.maps.event.addListener(spot, 'click', showArrays);
        }
      }
    }

      
/** @this {google.maps.Polygon} */
function showArrays(event) {

  // Since this Polygon only has one path, we can call getPath()
  // to return the MVCArray of LatLngs
  var vertices = this.getPath();

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

  infoWindow.open(map);
}

function showLocation() {
    var start = document.getElementById("address1").value;
    var end = document.getElementById("address2").value;
    var request = {
        origin: start,
        destination: end,
        travelMode: google.maps.TravelMode.DRIVING
    };
    directionService.route(request, function (result, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            directionRenderer.setDirections(result);
        }
    });
}

function computeTotalDistance(result) {
    var total = 0;
    var myroute = result.routes[0];
    for (i = 0; i < myroute.legs.length; i++) {
        total += myroute.legs[i].distance.value;
    }
    var drivingDistanceMiles = total / 1609.344;
    drivingDistanceMiles = Math.round(drivingDistanceMiles * 100) / 100;
    var drivingrate1 = Math.round((drivingDistanceMiles * fleetSettings.EstimateMileageCost * fleetSettings.EstimateMileageTicks) + fleetSettings.EstimatePkupCost);
    var drivingrate2 = Math.round(drivingrate1 * (1 + fleetSettings.EstimateUpperPercentage));

    $("#estimate").html('<strong>Driving Distance: </strong>' + drivingDistanceMiles + ' miles <br/><strong>Rate estimate: </strong> $' + drivingrate1 + ' to $' + drivingrate2 + '<br/>');
    $("#estimate").show();
}

function codeAddress() {
    geocoder.geocode({ 'address': fleetSettings.MapZipCode }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            map.setCenter(results[0].geometry.location);
        } else {
            alert("Unable to center the map because: " + status);
        }
    });
}

function getUserAddress() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
        function (position) {

            // Did we get the position correctly?
            // alert (position.coords.latitude);

            // To see everything available in the position.coords array:
            // for (key in position.coords) {alert(key)}

            mapServiceProvider(position.coords.latitude, position.coords.longitude);

        },
        // next function is the error callback
        function (error) {
            switch (error.code) {
                case error.TIMEOUT:
                    alert('Timeout');
                    break;
                case error.POSITION_UNAVAILABLE:
                    alert('Position unavailable');
                    break;
                case error.PERMISSION_DENIED:
                    alert('Permission denied');
                    break;
                case error.UNKNOWN_ERROR:
                    alert('Unknown error');
                    break;
            }
        }
        );
    }
}