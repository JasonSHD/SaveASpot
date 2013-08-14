q("mapTab", function (arg) {
	console.log("map tab load.");

	var application = new MvcCompositeObject().
		add(new PhasesMvcObject()).
		add(new SpotsMvcObject());

	if ($("#isCustomer").val().toUpperCase() == "TRUE") {
		application.add(new CustomerSpotsPartialMvcObject());
	}

	application.execute("initialize");


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
			var targetMvcObjects = [];

			for (var objectIndex in result._mvcObjectCollection) {
				var object = result._mvcObjectCollection[objectIndex];

				if (object._controllers[action] != undefined) {
					targetMvcObjects.push(object);
				}
			}

			for (var targetObjectIndex in targetMvcObjects) {
				var targetMvcObject = targetMvcObjects[targetObjectIndex];
				var controllerContext = buildControllerContext(targetMvcObject);
				targetMvcObject._controllers[action].call(controllerContext, excArg);
			}

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
		result._controllers.initialize = function () {
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
						context.execute("onSpotSelect", spot);
					}
				});
			});
		};
		result._controllers.onSpotSelect = function (arg) {
			var spotDesc = arg;
			var spot = spotDesc.spot;
			if (spot.isAvailable) {
				if (spot.selected) {
					spot.selected = false;
					spotDesc.val = "available";
				} else {
					spot.selected = true;
					spotDesc.val = "selected";
				}
			} else {
				return;
			}

			var context = this;
			var spotArg = {
				spotDesc: spotDesc
			};
			spotArg.onSelect = function (onSelectArg) {
				context.execute("onSpotSelect", onSelectArg);
			};
			this.view("changeSpotState", spotArg);

			this.execute("onSpotSelected", spotArg);
		};
		result._controllers.selectFirstAvailable = function () {
			var context = this;
			this.model("getFirstAvailable", function (spot) {
				context.view("selectSpot", {
					spot: spot,
					callback: function (spotArg) {
						context.execute("onSpotSelect", spotArg);
					}
				});
			});
		};

		result._views.context = {
			color: {
				available: "#00FF00",
				selected: "#FFFF00",
				unavailable: "#FF0000"
			}
		};
		result._views.spots = function (spotsArg) {
			var spots = spotsArg.spots;

			this.view("clearPolygons");

			var bounds = new google.maps.LatLngBounds();
			this.existsPolygons = this.shared.existsPolygons || [];

			for (var elementIndex in spots) {
				var spot = spots[elementIndex];
				var elementPolygonCoords = [];

				for (var pointIndex in spot.points) {
					var point = spot.points[pointIndex];

					elementPolygonCoords.push(new google.maps.LatLng(point.lng, point.lat));
				}

				var spotDesc = { spot: spot, val: spot.isAvailable ? "available" : "unavailable" };
				this.view("createPolygonForSpot", { paths: elementPolygonCoords, spotDesc: spotDesc, onSelect: spotsArg.onSelect });
				spotDesc.polygon.getPath().forEach(function (pointArg) {
					bounds.extend(pointArg);
				});
				this.existsPolygons.push(spotDesc);
			}

			this.shared.gmap.fitBounds(bounds);
		};
		result._views.clearPolygons = function () {
			q.each(this.shared.existsPolygons, function () {
				this.polygon.setMap(undefined);
			});

			this.shared.existsPolygons = [];
		};
		result._views.changeSpotState = function (spotArg) {
			spotArg.spotDesc.polygon.setMap(undefined);
			this.view("createPolygonForSpot", { paths: spotArg.spotDesc.polygon.getPath(), spotDesc: spotArg.spotDesc, onSelect: spotArg.onSelect });
		};
		result._views.createPolygonForSpot = function (spotArg) {
			var spotDesc = spotArg.spotDesc;
			var colors = this.color;
			var polygon = new google.maps.Polygon({
				paths: spotArg.paths,
				strokeColor: colors[spotDesc.val],
				strokeOpacity: 0.8,
				strokeWeight: 2,
				fillColor: colors[spotDesc.val],
				fillOpacity: 0.35
			});
			polygon.setMap(this.shared.gmap);
			spotDesc.polygon = polygon;

			google.maps.event.addListener(polygon, 'click', function () {
				spotArg.onSelect(spotDesc);
			});
		};
		result._views.selectSpot = function (selectArg) {
			var spots = this.existsPolygons;

			for (var polgIndex in spots) {
				var polg = spots[polgIndex];

				if (polg.spot.identity == selectArg.spot.identity) {
					selectArg.callback(polg);
					return;
				}
			}
		};

		result._models.context = {
			spotsUrl: q.pageConfig.spotsUrl
		};
		result._models.spots = function (identity, callback) {
			var context = this;
			q.ajax({ url: this.spotsUrl + "?identity=" + identity }).done(function (spotsResult) {
				context.spotsForPhase = spotsResult;
				callback(spotsResult);
			});
		};
		result._models.getFirstAvailable = function (callback) {
			for (var spotIndex in this.spotsForPhase) {
				var spot = this.spotsForPhase[spotIndex];

				if (spot.isAvailable && !spot.selected) {
					callback(spot);
					return;
				}
			}
		};

		return result;
	}

	function CustomerSpotsPartialMvcObject() {
		var result = new MvcObject();

		result._controllers.context = {
			selectedContext: { selectedSpots: 0 }
		};
		result._controllers.initialize = function () {
			var context = this;
			this.view("showPanel", {
				onBook: function () {
					context.execute("booking");
				},
				onUp: function () {
					context.execute("selectUp");
				},
				onDown: function () {
					context.execute("selectDown");
				}
			});
		};
		result._controllers.booking = function () {
			var context = this;
			this.model("bookingSpots", function (bookedSpots) {

			});
		};
		result._controllers.selectUp = function () {
			this.execute("selectFirstAvailable");
		};
		result._controllers.selectDown = function () {
			var context = this;
			this.model("getFirst", function (spot) {
				context.execute("onSpotSelect", spot.spotDesc);
			});
		};
		result._controllers.removeSpot = function (spot) {
			this.selectedContext.selectedSpots = this.selectedContext.selectedSpots - 1;

			spot.spotDesc.val = "available";
			spot.spotDesc.spot.selected = false;

			this.model("removeSpot", spot);
			this.view("updateSelectedSpots", this.selectedContext.selectedSpots);
		};
		result._controllers.onSpotSelected = function (spot) {
			if (spot.spotDesc.val == "selected") {
				this.selectedContext.selectedSpots = this.selectedContext.selectedSpots + 1;
				this.model("addSpot", spot);
			} else {
				this.execute("removeSpot", spot);
			}
			this.view("updateSelectedSpots", this.selectedContext.selectedSpots);
		};

		result._views.context = {
			$panel: $("#customerSpotSelectPanel")
		};
		result._views.showPanel = function (panelArg) {
			this.$panel.show();
			this.$panel.find("button[data-booking]").click(function () {
				panelArg.onBook();
			});
			this.$panel.find("button[data-up]").click(function () {
				panelArg.onUp();
			});
			this.$panel.find("button[data-down]").click(function () {
				panelArg.onDown();
			});
		};
		result._views.updateSelectedSpots = function (model) {
			this.$panel.find("input").val(model);
		};

		result._models.context = {
			selectedSpots: {},
			bookingUrl: q.pageConfig.bookingForCustomerUrl
		};
		result._models.addSpot = function (spotDesc) {
			this.selectedSpots[spotDesc.spotDesc.spot.identity] = spotDesc;
		};
		result._models.removeSpot = function (spotDesc) {
			delete this.selectedSpots[spotDesc.spotDesc.spot.identity];
		};
		result._models.bookingSpots = function (callback) {
			var data = {};
			var index = 0;
			for (var spotIdentity in this.selectedSpots) {
				data["identities[" + index + "]"] = spotIdentity;
				index++;
			}
			var context = this;
			q.ajax({ url: this.bookingUrl, data: data, type: "POST" }).done(function (bookingResult) {
				for (var bookingIndex in bookingResult.identities) {
					var bookedId = bookingResult.identities[bookingIndex];

					delete context.selectedSpots[bookedId];
				}

				callback(bookingResult);
			});
		};
		result._models.getFirst = function (callback) {
			for (var spotIndex in this.selectedSpots) {
				callback(this.selectedSpots[spotIndex]);
				return;
			}
		};

		return result;
	}

	arg = arg || {};

	arg.unload = function () {
		application.destroy();
		console.log("map tab unload.");
	};
});