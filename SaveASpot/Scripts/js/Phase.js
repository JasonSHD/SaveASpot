var SaveASpot = SaveASpot || {};

SaveASpot.Phase = (function ($) {
    var my = {};

    my.baseUrl = "/API/Phase/";
    my.Phases = [];

    my.Initialize = function () {
        my.LoadPhases();
    };

    my.LoadPhases = function () {
        $.post(my.baseUrl + "All", null, function (result) {
            if (result.success) {
                my.Phases = result.results;
                my.RenderList();
                SaveASpot.Map.setAllPhase();
            }
            else {
                alert("there was an error loading phases.");
            }
        }, "json");
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
            SaveASpot.SponsorSpot.LoadSpots(phase);

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

    my.FindPhaseByID = function (id) {
        for (var i = 0; i < my.Phases.length; i++) {
            if (my.Phases[i].ID == id) {
                return my.Phases[i];
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