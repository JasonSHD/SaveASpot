var SaveASpot = SaveASpot || {};

SaveASpot.Spot = (function ($) {
    var my = {};

    my.baseUrl = "/API/Spot/";
    my.Spots = [];
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
                        my.Spots.push(result.results[i]);
                        SaveASpot.Map.processSpot(result.results[i], "#0000ff", true, null);
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

    my.FindSpotByID = function (id) {
        for (var i = 0; i < my.Spots.length; i++) {
            if (my.Spots[i].ID == id) {
                return my.Spots[i];
            }
        }

        return null;
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