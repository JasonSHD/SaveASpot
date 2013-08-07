q("mapTab", function (arg) {
	console.log("map tab load.");

	var application = new MvcCompositeObject().
		add(new PhasesMvcObject()).
		add(new SpotsMvcObject());

	if ($("#isCustomer").val().toUpperCase() == "TRUE") {
		application.add(new CustomerSpotHandlers());
	}

	application.execute("phases");


	function MvcCompositeObject() {
		var result = { _mvcObjectCollection: [], _sharedViewData: {} };

		result.add = function (mvcObject) {
			result._mvcObjectCollection.push(mvcObject);

			return result;
		};

		function buildControllerContext(mvcObject) {
			var context = {};

			for (var contextIndex in mvcObject._controllers.context) {
				context[contextIndex] = mvcObject._controllers.context[contextIndex];
			}

			context.view = function (name, model) {
				var viewContext = mvcObject._views.context || {};
				Object.defineProperty(viewContext, "shared", { configurable: false, writable: false, value: result._sharedViewData });
				viewContext.view = context.view;
				mvcObject._views[name].call(viewContext, model);

				return context;
			};

			context.model = function (name, modelArg, callback) {
				var modelContext = mvcObject._models.context;
				mvcObject._models[name].call(modelContext, modelArg, callback);

				return context;
			};

			context.execute = function (action, excArg) {
				return result.execute(action, excArg);
			};

			return context;
		}

		result.execute = function (action, excArg) {
			var targetMvcObject = undefined;

			for (var objectIndex in result._mvcObjectCollection) {
				var object = result._mvcObjectCollection[objectIndex];

				if (object._controllers[action] != undefined) {
					targetMvcObject = object;
					break;
				}
			}

			if (targetMvcObject == undefined) return result;

			var controllerContext = buildControllerContext(targetMvcObject);
			targetMvcObject._controllers[action].call(controllerContext, excArg);

			return result;
		};

		result.destroy = function () {
			for (var objectIndex in result._mvcObjectCollection) {
				var object = result._mvcObjectCollection[objectIndex];
				if (typeof object["destroy"] == "function") {
					object.destroy();
				}
			}

			return result;
		};

		return result;
	}

	function MvcObject() {
		var result = {
			_controllers: {},
			_views: {},
			_models: {},
			_destroy: [],
			destory: function () {
				for (var destroyHandlerIndex in this._destroy) {
					var destroyHandler = this._destroy[destroyHandlerIndex];

					if (typeof destroyHandler == "function") {
						destroyHandler.call(this);
					}
				}
			}
		};

		return result;
	}

	function PhasesMvcObject() {
		var result = new MvcObject();

		result._controllers.context = {
			gmapKey: $("[data-gmap-api-key]").data("gmap-api-key")
		};
		result._controllers.phases = function () {
			var context = this;

			context.view("gmap", context.gmapKey);
			context.model("phases", function (phases) {
				context.view("phasesMenu", {
					phases: phases,
					onSelect: function (phaseArg) {
						context.execute("spots", phaseArg);
					}
				});
			});
		};

		result._views.context = {
			navMenu: $("#dashboard-menu"),
			destroy: function (handler) {
				result._destroy.push(handler);
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
		result._views.phases = function () {
		};
		result._views.gmap = function (key) {
			var context = this;
			q.controls.gmap(key, function () {
				var mapOptions = {
					zoom: 8,
					center: new google.maps.LatLng(0, 0),
					mapTypeId: google.maps.MapTypeId.ROADMAP
				};
				context.shared.gmap = new google.maps.Map(context.gmapContext.mapCanvas, mapOptions);
			});
		};
		result._views.phasesMenu = function (argPhases) {
			var context = this;
			var navMenuContext = context.navMenuContext = context.navMenuContext || {};
			navMenuContext.onSelect = argPhases.onSelect;

			if (navMenuContext.selectHandler == undefined) {
				navMenuContext.selectHandler = function () {
					if (navMenuContext.onSelect == undefined) {
						return;
					}
					navMenuContext.onSelect({ identity: this.getAttribute("data-identity") });
				};

				context.navMenu.on("click", "[data-identity]", navMenuContext.selectHandler);

				context.destroy(function () {
					context.navMenu.off("click", "[data-identity]", navMenuContext.selectHandler);
					navMenuContext.selectHandler = undefined;

					console.log("navMenu view destroyed");
				});
			}
			context.navMenu.html("");

			$(argPhases.phases).each(function () {
				context.view("addPhaseElement", { icon: "icon-map-marker", text: this.name, identity: this.identity });
			});
		};
		result._views.addPhaseElement = function (element) {
			this.navMenu.append($("<li/>").attr("data-identity", element.identity).append($("<a/>").attr("href", "javascript:void(0)").append($("<i/>").addClass(element.icon)).append($("<span/>").text(element.text))));
		};

		result._models.context = {
			phasesUrl: q.pageConfig.phasesUrl
		};
		result._models.phases = function (callback) {
			q.ajax({ url: this.phasesUrl + "?isJson=true", type: "GET" }).done(function (result) {
				callback(result);
			});
		};

		return result;
	}

	function SpotsMvcObject() {
		var result = new MvcObject();

		result._controllers.spots = function (spotsArg) {
			var context = this;
			context.model("spots", spotsArg.identity, function (spotsResult) {
				context.view("spots", {
					spots: spotsResult,
					onSelect: function (spot) {
						context.execute("onSpotSelect", { spot: spot });
					}
				});
			});
		};

		result._views.context = {
			existsPolygons: []
		};
		result._views.spots = function (spotsArg) {
			var color = {
				available: "#00FF00",
				selected: "#FFFF00",
				unavailable: "#FF0000"
			};
			var spots = spotsArg.spots;

			this.view("clearPolygons");

			var bounds = new google.maps.LatLngBounds();
			for (var elementIndex in spots) {
				var element = spots[elementIndex];
				var elementPolygonCoords = [];

				for (var pointIndex in element.points) {
					var point = element.points[pointIndex];

					elementPolygonCoords.push(new google.maps.LatLng(point.lng, point.lat));
				}

				var elementPolygon = new google.maps.Polygon({
					paths: elementPolygonCoords,
					strokeColor: element.isAvailable ? color.available : color.unavailable,
					strokeOpacity: 0.8,
					strokeWeight: 2,
					fillColor: element.isAvailable ? color.available : color.unavailable,
					fillOpacity: 0.35
				});

				this.view("addSpotOnMap", { polygon: elementPolygon, spot: element, bounds: bounds, onSelect: spotsArg.onSelect });
			}

			this.shared.gmap.fitBounds(bounds);
		};
		result._views.addSpotOnMap = function (spotArg) {
			var polygon = spotArg.polygon;
			var spot = spotArg.spot;
			var bounds = spotArg.bounds;

			polygon.setMap(this.shared.gmap);
			polygon.getPath().forEach(function (pointArg) {
				bounds.extend(pointArg);
			});
			this.existsPolygons.push(polygon);
			google.maps.event.addListener(polygon, 'click', function () {
				spotArg.onSelect(spot);
			});
		};
		result._views.clearPolygons = function () {
			q.each(this.existsPolygons, function () {
				this.setMap(undefined);
			});
		};

		result._models.context = {
			spotsUrl: q.pageConfig.spotsUrl
		};
		result._models.spots = function (identity, callback) {
			q.ajax({ url: this.spotsUrl + "?identity=" + identity }).done(function (spotsResult) {
				callback(spotsResult);
			});
		};

		return result;
	}

	function CustomerSpotHandlers() {
		var result = new MvcObject();

		result._controllers.onSpotSelect = function (spotArg) {
			alert("try to bron spot:" + spotArg.spot.identity);
		};

		return result;
	}

	arg = arg || {};

	arg.unload = function () {
		application.destroy();
		console.log("map tab unload.");
	};
});