q("mapTab", function (arg) {
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
			context.views.navMenu({
				elements: phases,
				onSelect: function (phaseArg) {
					context.execute("spots", phaseArg);
				}
			});
		});
	};
	mvc._controllers.spots = function (controllerArg) {
		var context = this.context;
		context.models.spots(controllerArg.identity, function (result) {
			$(result).each(function (index) {
				this.name = "Spot " + (index + 1);
			});
			context.views.navMenu({
				elements: result,
				onSelect: function () {
				},
				onBack: function () {
					context.execute("phases", {});
				},
				showBack: true
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
	mvc._views.navMenu = function (navMenuArg) {
		var context = this.context;
		var navMenuContext = this.context.navMenuContext = this.context.navMenuContext || {};
		navMenuContext.onSelect = navMenuArg.onSelect;
		navMenuContext.onBack = navMenuArg.onBack;

		if (navMenuContext.selectHandler == undefined) {
			navMenuContext.selectHandler = function () {
				var dataIdentity = this.getAttribute("data-identity");
				if (dataIdentity == "" && navMenuContext.onBack != undefined) {
					navMenuContext.onBack(navMenuArg.onBackArg);
				} else {
					navMenuContext.onSelect({ identity: this.getAttribute("data-identity") });
				}
			};

			this.context.navMenu.on("click", "[data-identity]", navMenuContext.selectHandler);

			this.context.destroy(function () {
				context.navMenu.off("click", "[data-identity]", navMenuContext.selectHandler);
				navMenuContext.selectHandler = undefined;

				console.log("navMenu view destroyed");
			});
		}
		context.navMenu.html("");

		if (navMenuArg.showBack) {
			this.addNavElement({ icon: "icon-arrow-left", text: "Back", identity: "" });
		}

		var thisContext = this;

		$(navMenuArg.elements).each(function () {
			thisContext.addNavElement({ icon: "icon-map-marker", text: this.name, identity: this.identity });
		});
	};
	mvc._views.addNavElement = function (element) {
		this.context.navMenu.append($("<li/>").attr("data-identity", element.identity).append($("<a/>").attr("href", "javascript:void(0)").append($("<i/>").addClass(element.icon)).append($("<span/>").text(element.text))));
	};
	mvc._views.gmap = function (key) {
		var context = this.context;
		q.controls.gmap(key, function () {
			var mapOptions = {
				zoom: 8,
				center: new google.maps.LatLng(0, 0),
				mapTypeId: google.maps.MapTypeId.ROADMAP
			};
			context.gmapContext.gmap = new google.maps.Map(context.gmapContext.mapCanvas, mapOptions);
		});
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
		spotsUrl: q.pageConfig.spotsUrl
	};

	mvc._models.phases = function (callback) {
		q.ajax({ url: this.context.phasesUrl + "?isJson=true", type: "GET" }).done(function (result) {
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