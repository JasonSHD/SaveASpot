var SaveASpot = SaveASpot || {};

SaveASpot.Spot = (function ($) {
    var my = {};

    my.baseUrl = "/API/Spot/";
    my.Spots = [];
    my.SelectedSpots = [];
    my.Total = 0;
    my.Loaded = 0;
    my.Take = 400;
    my.SelectedPhaseID = null;
    my.Loader = null;
    my.Data = null;

    my.Initialize = function () {
    };

    my.LoadSpots = function (phaseID, northEast, southWest) {
        // clear out the old spots
        my.Spots = [];
        my.Loaded = 0;
        my.SelectedPhaseID = phaseID;

        // reset loader
        SaveASpot.Map.SetLoaderPercentage(1);
        SaveASpot.Map.Loader.show();

        my.Data = {
            id: phaseID,
            neLat: northEast.lat(),
            neLng: northEast.lng(),
            swLat: southWest.lat(),
            swLng: southWest.lng()
        };

        // get the count of the spots
        $.post(my.baseUrl + "GetByPhaseAndBounds", my.Data, function (result) {
            if (result.success) {
                my.Total = result.count;

                if (my.Total < 1000) {
                    // clear the old
                    SaveASpot.Map.clearOverlays();
                    $("#spotCount").hide();

                    // load the new
                    my.RecursiveLoad();
                }
                else {
                    var output = "Found Spots:<br /><strong>" + my.Total + "</strong><br />Zoom in to see spots available.";
                    $("#spotCount").html(output);
                    $("#spotCount").show();
                    SaveASpot.Map.Loader.hide();
                    //var phase = SaveASpot.Phase.FindPhaseByID(my.SelectedPhaseID);
                    //SaveASpot.Map.loadPhase(phase, true, null);
                }
            }
            else {
                alert("there was an error loading the spots.");
            }
        }, "json");
    };

    my.GetBySponsor = function (sponsorID) {
        SaveASpot.Map.Loader.hide();

        $.post(my.baseUrl + "GetBySponsor", { id: sponsorID }, function (result) {
            if (result.success) {
                for (var i = 0; i < result.results.length; i++) {
                    my.Spots.push(result.results[i]);
                    SaveASpot.Map.processSpot(result.results[i], "#0000ff", true, my.ToggleSelect);
                }
            }
            else {
                alert("there was an error loading spots.");
            }
        });
    };

    my.RecursiveLoad = function () {
        if (my.Loaded < my.Total) {
            var data = my.Data;
            data.index = my.Loaded;
            data.take = my.Take;

            $.post(my.baseUrl + "GetByPhase", data, function (result) {
                if (result.success) {
                    my.Loaded += my.Take;

                    var percent = Math.ceil((my.Loaded / my.Total) * 100);
                    SaveASpot.Map.SetLoaderPercentage(percent);

                    for (var i = 0; i < result.results.length; i++) {
                        my.Spots.push(result.results[i]);
                        SaveASpot.Map.processSpot(result.results[i], "#0000ff", true, my.ToggleSelect);
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
        }
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

    my.ToggleSelect = function () {
        /*var color = SaveASpot.Map.SpotColors.Selected;
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
        });*/
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

    return my;
} (jQuery));