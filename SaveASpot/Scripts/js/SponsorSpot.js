var SaveASpot = SaveASpot || {};

SaveASpot.SponsorSpot = (function ($) {
    var my = {};

    my.baseUrl = "/API/SponsorSpot/";
    my.Spots = [];
    my.SelectedSpots = [];
    my.Total = 0;
    my.Loaded = 0;
    my.Take = 400;
    my.SelectedPhaseID = null;
    my.Loader = null;

    my.Initialize = function () {
    };

    my.LoadSpots = function (phaseID) {
        // clear out the old spots
        my.Spots = [];
        my.Loaded = 0;
        my.SelectedPhaseID = phaseID;

        // reset loader
        SaveASpot.Map.SetLoaderPercentage(1);
        SaveASpot.Map.Loader.show();

        // get the count of the spots
        $.post(my.baseUrl + "CountByPhase", { id: phaseID }, function (result) {
            if (result.success) {
                my.Total = result.count;
                my.RecursiveLoad();
            }
            else {
                alert("there was an error loading the spots.");
            }
        }, "json");
    };

    my.RecursiveLoad = function () {
        if (my.Loaded < my.Total) {
            var data = {
                id: my.SelectedPhaseID,
                index: my.Loaded,
                take: my.Take
            };

            $.post(my.baseUrl + "GetByPhase", data, function (result) {
                if (result.success) {
                    my.Loaded += my.Take;

                    var percent = Math.ceil((my.Loaded / my.Total) * 100);
                    SaveASpot.Map.SetLoaderPercentage(percent);

                    for (var i = 0; i < result.results.length; i++) {
                        var color = SaveASpot.Map.SpotColors.Available;
                        var selectable = true;

                        if (result.results[i].Taken && result.results[i].SponsorIDString == $("#id").val()) {
                            color = SaveASpot.Map.SpotColors.Selected;
                        } else if (result.results[i].Taken /*&& result.results[i].sponsorID == sponsorID*/) {
                            // if spot's sponsorID is this sponsor, color not red (yellow?)
                            color = SaveASpot.Map.SpotColors.Unavailable;
                            selectable = false;
                        }

                        my.Spots.push(result.results[i]);
                        SaveASpot.Map.processSpot(result.results[i], color, selectable, my.ToggleSelect);
                    }

                    // recursive call to load more sequentially
                    my.RecursiveLoad();
                }
                else {
                    alert("there was an error loading spots.");
                }
            });
        }
        else {
            SaveASpot.Map.Loader.hide();
            SaveASpot.Map.setZoom();
        }
    };

    my.ToggleSelect = function () {
        var color = SaveASpot.Map.SpotColors.Selected;
        if (this.selected) {
            SaveASpot.Map.removeFromCart(this.SpotID, my.SelectedPhaseID);
            color = SaveASpot.Map.SpotColors.Available;
        }
        else {
            SaveASpot.Map.addToCart(this.SpotID, my.SelectedPhaseID);
            var spot = my.FindSpotByID(this.SpotID);
            my.SelectedSpots.push(spot);
        }

        $(".notification-count").html(my.SelectedSpots.length);
        my.RenderList();

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

    my.RenderList = function () {
        // clear the menu
        $("#cart").html("");

        // load the templates
        var template = $("#cartItem").html();

        // load the phase items
        for (var i = 0; i < my.SelectedSpots.length; i++) {
            var data = { Index: i + 1, Price: "N/A", ID: my.SelectedSpots[i].SpotIDString };
            var output = Mustache.render(template, data);
            $("#cart").append(output);
        }
    };

    my.FindSpotByID = function (id) {
        for (var i = 0; i < my.Spots.length; i++) {
            if (my.Spots[i].SpotIDString == id) {
                return my.Spots[i];
            }
        }

        return null;
    };

    my.RemoveFromSelected = function (id) {
        for (var i = 0; i < my.SelectedSpots.length; i++) {
            if (my.SelectedSpots[i].SpotIDString == id) {
                my.SelectedSpots.splice(i, 1);
                return true;
            }
        }

        return false;
    };

    my.Select = function () {
        /// <summary>
        /// select a blog page from the site.
        /// </summary>
        var data = { id: $(this).attr("id") };

        if (data.id != $("#PageID").val()) {

            // get the pageEditor
            $.post(baseURL + 'Details', data, my.setData, "json");
        }
    };

    return my;
} (jQuery));