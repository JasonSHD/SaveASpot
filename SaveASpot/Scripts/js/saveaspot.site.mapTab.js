﻿q("mapTab", function (arg) {
	console.log("map tab load.");

	var mvc = { _controllers: {}, _views: {}, _models: {} };

	mvc._controllers.context = {};//controller context
	Object.defineProperty(mvc._controllers.context, "gmapKey", {
		configurable: false,
		get: function () {
			return $("[data-gmap-api-key]").attr("data-gmap-api-key");
		},
		set: function () { }
	});
	Object.defineProperty(mvc._controllers.context, "models", {
		configurable: false,
		get: function () {
			return mvc._models;
		},
		set: function () { }
	});
	Object.defineProperty(mvc._controllers.context, "views", {
		configurable: false,
		get: function () {
			return mvc._views;
		},
		set: function () { }
	});

	mvc._controllers.context.execute = function (name, argExec) {
		mvc._controllers[name](argExec);
	};

	mvc._controllers.phases = function () {
		var context = this.context;

		context.views.gmap(context.gmapKey);
		context.views.ajaxIndicator("show");
		this.context.models.phases(function (phases) {
			context.views.ajaxIndicator("hide");
			context.views.navMenu(phases, function (phaseArg) {
				context.execute("parcels", phaseArg);
			});
		});
	};
	mvc._controllers.parcels = function (controllerArg) {
		var context = this.context;
		context.models.parcels(controllerArg.identity, function (result) {
			context.views.navMenu(result, function (parcelArg) {
				context.execute("spots", parcelArg);
			});
			context.views.parcels(result);
		});
	};
	mvc._controllers.spots = function (controllerArg) {
		var context = this.context;
		context.models.spots(controllerArg.identity, function (result) {
			$(result).each(function (index) {
				this.name = "Spot " + (index + 1);
			});
			context.views.navMenu(result, function (spotArg) {
			});
			context.views.spots(result);
		});
	};

	mvc._views.context = {//view context
		navMenu: $("#dashboard-menu"),
		destroy: function (handler) {
			mvc._destroy.handlers.push(handler);
		},
		gmapContext: {
			existsPolygons: [],
			mapCanvas: document.getElementById("map-canvas"),
			clearPolygons: function () {
				var existsPolygons = this.existsPolygons;

				for (var polIndex in existsPolygons) {
					var pol = existsPolygons[polIndex];
					pol.setMap(undefined);
				}

				this.existsPolygons = [];
			}
		}
	};

	mvc._views.ajaxIndicator = function (state) {
		var indicatorContext = this.context.ajaxIndicator = this.context.ajaxIndicator || {
			hide: function () {
				this.control.hide();
			},
			show: function () {
				this.control.show();
			}
		};
		indicatorContext.indicator = indicatorContext.indicator || {};
		if (indicatorContext.indicator.control == undefined) {
			indicatorContext.indicator.control = q.controls.ajaxIndicator(this.context.navMenu);
		}

		(indicatorContext.indicator[state] || function () {
		})();

	};
	mvc._views.navMenu = function (elements, onSelect) {
		var context = this.context;
		var navMenuContext = this.context.navMenuContext = this.context.navMenuContext || {};

		if (navMenuContext.selectHandler == undefined) {
			navMenuContext.selectHandler = function () {
				navMenuContext.onSelect({ identity: this.getAttribute("data-identity") });
			};

			this.context.navMenu.on("click", "[data-identity]", navMenuContext.selectHandler);

			this.context.destroy(function () {
				context.navMenu.off("click", "[data-identity]", navMenuContext.selectHandler);
				navMenuContext.selectHandler = undefined;

				console.log("navMenu view destroyed");
			});
		}
		navMenuContext.onSelect = onSelect;
		context.navMenu.html("");

		$(elements).each(function () {
			context.navMenu.append(
				$("<li/>").attr("data-identity", this.identity).append(
					$("<a/>").attr("href", "javascript:void(0)").append(
						$("<i/>").addClass("icon-map-marker")).append(
							$("<span/>").text(this.name))
				));
		});
	};
	mvc._views.gmap = function (key) {
		var context = this.context;
		q.controls.gmap(key, function () {
			var mapOptions = {
				zoom: 8,
				center: new google.maps.LatLng(-34.397, 150.644),
				mapTypeId: google.maps.MapTypeId.ROADMAP
			};
			context.gmapContext.gmap = new google.maps.Map(context.gmapContext.mapCanvas, mapOptions);
		});
	};
	mvc._views.parcels = function (parcels, onSelect) {
		this.gmapPolygons(parcels, { onSelect: onSelect, color: "#FF0000" });
	};
	mvc._views.spots = function (spots, onSelect) {
		this.gmapPolygons(spots, { onSelect: onSelect, color: "#00FF00" });
	};
	mvc._views.gmapPolygons = function (elements, args) {
		this.context.gmapContext.clearPolygons();

		var isFirst = false;
		var center = undefined;
		for (var elementIndex in elements) {
			var element = elements[elementIndex];
			var elementPolygonCoords = [];

			for (var pointIndex in element.points) {
				var point = element.points[pointIndex];

				if (!isFirst) {
					center = new google.maps.LatLng(point.lng, point.lat);
					isFirst = true;
				}

				elementPolygonCoords.push(new google.maps.LatLng(point.lng, point.lat));
			}

			var color = args.color;
			var elementPolygon = new google.maps.Polygon({
				paths: elementPolygonCoords,
				strokeColor: color,
				strokeOpacity: 0.8,
				strokeWeight: 2,
				fillColor: color,
				fillOpacity: 0.35
			});

			elementPolygon.setMap(this.context.gmapContext.gmap);
			this.context.gmapContext.existsPolygons.push(elementPolygon);
		}

		this.context.gmapContext.gmap.setCenter(center);
	};

	mvc._models.context = {//model context
		phasesUrl: q.pageConfig.phasesUrl,
		parcelsUrl: q.pageConfig.parcelsUrl,
		spotsUrl: q.pageConfig.spotsUrl
	};

	mvc._models.phases = function (callback) {
		q.ajax({ url: this.context.phasesUrl + "?isJson=true", type: "GET" }).done(function (result) {
			callback(result);
		});
	};
	mvc._models.parcels = function (phaseIdentity, callback) {
		q.ajax({ url: this.context.parcelsUrl + "?identity=" + phaseIdentity }).done(function (result) {
			callback(result);
		});
	};
	mvc._models.spots = function (parcelsIdentity, callback) {
		q.ajax({ url: this.context.spotsUrl + "?identity=" + parcelsIdentity }).done(function (result) {
			callback(result);
		});
	};

	mvc._destroy = { handlers: [] };
	mvc.destroy = function () {
		for (var destoryIndex in mvc._destroy.handlers) {
			var destroyHandler = mvc._destroy.handlers[destoryIndex];
			destroyHandler();
		}
	};

	mvc._controllers.phases();

	arg = arg || {};

	arg.unload = function () {
		mvc.destroy();
		console.log("map tab unload.");
	};
});