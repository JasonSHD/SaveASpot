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
                        if (result.results[i].Taken) {
                            color = SaveASpot.Map.SpotColors.Unavailable;
                        }

                        my.Spots.push(result.results[i]);
                        SaveASpot.Map.processSpot(result.results[i], color, true, my.ToggleSelect);
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
            console.log("remove:" + my.RemoveFromSelected(this.SpotID));
            color = SaveASpot.Map.SpotColors.Available;
        }
        else {
            console.log("add: " + this.SpotID);

            var spot = my.FindSpotByID(this.SpotID);
            my.SelectedSpots.push(spot);
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

    my.RenderList = function () {
        // clear the menu
        $("#dashboard-menu").html("");

        // load the templates
        var template = $("#phaseItem").html();
        var all = $("#phaseAllItem").html();

        // load the all item
        $("#dashboard-menu").append(all);

        // load the phase items
        for (var i = 0; i < my.Phases.length; i++) {
            var output = Mustache.render(template, my.Phases[i]);
            $("#dashboard-menu").append(output);
        }

        // bind events
        $(".phase").click(function () {
            var phase = $(this).attr("href").replace("#", "");
            SaveASpot.Map.setPhase(phase);

            var parent = $(this).parent().parent();
            parent.find("li").removeClass("active");
            $(this).parent().addClass("active");
            return false;
        });

        $(".phase-all").click(function () {
            SaveASpot.Map.setAllPhase();
            return false;
        });
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