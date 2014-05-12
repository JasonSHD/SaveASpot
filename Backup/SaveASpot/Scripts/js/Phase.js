var SaveASpot = SaveASpot || {};

SaveASpot.Phase = (function ($) {
    var my = {};

    my.baseUrl = "/API/Phase/";
    my.Phases = [];
    my.Sponsors = [];

    my.Initialize = function () {
        my.LoadPhases();
        my.LoadSponsors();

        $("#dashboard-sponsors").on("click", "li", function () {
            SaveASpot.Map.clearOverlays();
            SaveASpot.Map.setSponsors($(this).attr("id"));
            SaveASpot.Spot.GetBySponsor($(this).attr("id"));
        });
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

    my.LoadSponsors = function () {
        $.post(my.baseUrl + "Sponsors", null, function (result) {
            if (result.success) {
                my.Sponsors = result.results;

                var sponsorList = $("#dashboard-sponsors");
                var dropdownList = $(".sponsor-list");

                sponsorList.html("");
                dropdownList.html("<option value=''>No Preference</option>");

                var sponsorTemplate = $("#sponsorItem").html();
                for (var i = 0; i < my.Sponsors.length; i++) {
                    my.Sponsors[i].SponsorIDToString = my.Sponsors[i].SponsorID;
                    sponsorList.append(Mustache.render(sponsorTemplate, my.Sponsors[i]));
                    dropdownList.append("<option value='" + my.Sponsors[i].SponsorID + "'>" + my.Sponsors[i].Name + "</option>");
                }
            }
            else {
                alert("there was an error loading sponsors.");
            }
        }, "json").fail(function (e) {
            console.log(e);
        });
    };

    my.RenderList = function () {
        // clear the menu
        var dashboard = $("#dashboard-menu").html("");

        // load the templates
        var all = $("#phaseAllItem").html();

        // load the all item
        $("#dashboard-menu").append(all);

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

    my.DisplaySponsors = function (phase) {
        var data = { id: phase };
        $.post(my.baseUrl + 'GetSponsorsByPhase', data, function (results) {
            if (results.success) {
                var sponsorList = $("#dashboard-sponsors");
                sponsorList.html("");
                var sponsorTemplate = $("#sponsorItem").html();
                for (var i = 0; i < results.results.length; i++) {
                    sponsorList.append(Mustache.render(sponsorTemplate, results.results[i]));
                }
            }
        }, 'json');
    };

    return my;
} (jQuery));