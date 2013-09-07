q("mapTab", function (arg) {

	var gmapKey = $("[data-gmap-api-key]").data("gmap-api-key");
	var gmapCanvas = document.getElementById("map-canvas");
	var gmap;
	q.controls.gmap(gmapKey, function () {
		var mapOptions = {
			zoom: 8,
			// ReSharper disable UseOfImplicitGlobalInFunctionScope
			center: new google.maps.LatLng(0, 0),
			mapTypeId: google.maps.MapTypeId.ROADMAP
		};
		gmap = new google.maps.Map(gmapCanvas, mapOptions);
		// ReSharper restore UseOfImplicitGlobalInFunctionScope
	});

	var spotsControl = (function (options) {
		var settings = $.extend(options, {
			spotsUrl: q.pageConfig.spotsUrl,
			colors: {
				available: "#00FF00",
				selected: "#FFFF00",
				unavailable: "#FF0000"
			}
		});

		var result = {};

		var spotDescriptions = [];

		var spotSelectedHandler = function () {
			q.events().fire(this.val + "SpotSelected", { spot: this.spot });
		};

		var displaySpot = function (spotDesc) {
			var colors = settings.colors;
			// ReSharper disable UseOfImplicitGlobalInFunctionScope
			var polygon = new google.maps.Polygon({
				// ReSharper restore UseOfImplicitGlobalInFunctionScope
				paths: spotDesc.paths,
				strokeColor: colors[spotDesc.val],
				strokeOpacity: 0.8,
				strokeWeight: 2,
				fillColor: colors[spotDesc.val],
				fillOpacity: 0.35
			});
			polygon.setMap(gmap);
			spotDesc.polygon = polygon;

			// ReSharper disable UseOfImplicitGlobalInFunctionScope
			google.maps.event.addListener(polygon, 'click', function () {
				// ReSharper restore UseOfImplicitGlobalInFunctionScope
				spotSelectedHandler.call(spotDesc);
			});
		};

		var initializeSpots = function (spots) {

			spotDescriptions = [];

			// ReSharper disable UseOfImplicitGlobalInFunctionScope
			var bounds = new google.maps.LatLngBounds();
			// ReSharper restore UseOfImplicitGlobalInFunctionScope

			for (var spotIndex in spots) {
				var spot = spots[spotIndex];

				var elementPolygonCoords = [];

				for (var pointIndex in spot.points) {
					var point = spot.points[pointIndex];

					// ReSharper disable UseOfImplicitGlobalInFunctionScope
					elementPolygonCoords.push(new google.maps.LatLng(point.lng, point.lat));
					// ReSharper restore UseOfImplicitGlobalInFunctionScope
				}

				var spotDesc = {
					spot: spot,
					val: spot.isAvailable ? "available" : "unavailable",
					paths: elementPolygonCoords
				};

				if (!spot.isAvailable) {
					var bp = "";
				}

				spotDescriptions.push(spotDesc);
				displaySpot(spotDesc);

				spotDesc.polygon.getPath().forEach(function (pointArg) {
					bounds.extend(pointArg);
				});
			}

			gmap.fitBounds(bounds);

			q.events().fire("phaseRenderCompleted", {});
		};

		var onPhaseChangedHandler = function (phaseArg) {
			q.ajax({ url: settings.spotsUrl + "?identity=" + phaseArg.arg.phaseId, type: "GET" }).done(function (spotResults) {

				initializeSpots(spotResults);
			});
		};

		q.events().bind("phaseChanged", onPhaseChangedHandler);

		var onUpdateSpotStateHandler = function (stateArg) {
			var args = stateArg.arg;

			var identity = args.identity;
			var val = args.val;
			var selectedCount = 0;

			for (var spotDescIndex in spotDescriptions) {
				var spotDesc = spotDescriptions[spotDescIndex];

				if (spotDesc.spot.identity == identity && spotDesc.val != val) {
					spotDesc.val = val;
					spotDesc.polygon.setMap(null);
					displaySpot(spotDesc);
				}

				if (spotDesc.val == "selected") {
					selectedCount++;
				}
			}

			q.events().fire("updateSpotsCount", { count: selectedCount });
		};

		q.events().bind("updateSpotState", onUpdateSpotStateHandler);

		var onSelectFirstAvailableSpot = function () {
			for (var spotDescIndex in spotDescriptions) {
				var spotDesc = spotDescriptions[spotDescIndex];

				if (spotDesc.val == "available") {
					spotSelectedHandler.call(spotDesc);

					return;
				}
			}
		};

		q.events().bind("selectFirstAvailableSpot", onSelectFirstAvailableSpot);

		var onRemoveFirstSpot = function () {
			for (var spotDescIndex in spotDescriptions) {
				var spotDesc = spotDescriptions[spotDescIndex];

				if (spotDesc.val == "selected") {
					spotSelectedHandler.call(spotDesc);

					return;
				}
			}
		};

		q.events().bind("removeFirstSpot", onRemoveFirstSpot);

		result.destroy = function () {
			q.events().unbind("phaseChanged", onPhaseChangedHandler);
			q.events().unbind("updateSpotState", onUpdateSpotStateHandler);
			q.events().unbind("selectFirstAvailableSpot", onSelectFirstAvailableSpot);
			q.events().unbind("removeFirstSpot", onRemoveFirstSpot);
			log.write("destory spots control.");
		};

		return result;
	})();

	var phaseControl = (function (options, $) {
		var settings = $.extend(options, {});
		var result = {};

		var $dashboardMenu = $("#dashboard-menu");
		$dashboardMenu.on("click", "[data-identity]", function () {
			var phaseId = this.getAttribute("data-identity");
			q.events().fire("changeTab", { tab: "map" });
			q.events().fire("phaseChanging", { phaseId: phaseId });
			$dashboardMenu.find("[data-identity]").removeClass("phase-active");
			$(this).addClass("phase-active");
			q.events().fire("phaseChanged", { phaseId: phaseId });
		});

		result.destroy = function () {
		};

		return result;
	})({}, jQuery);

	var customerControl = (function (options, $) {
		var settings = $.extend(options, {
			addSpotToCartUrl: q.pageConfig.addSpotToCartUrl,
			removeSpotFromCartUrl: q.pageConfig.removeSpotFromCartUrl
		});

		var result = {};

		var availableSpotSelectedHandler = function (args) {
			q.ajax({ url: settings.addSpotToCartUrl, type: "POST", data: { spotIdentity: args.arg.spot.identity } }).done(function (addResult) {
				if (addResult.isSuccess) {
					q.events().fire("updateCart", { cart: addResult.cart });
				} else {
					alert(addResult.message);
				}
			});
		};
		var selectedSpotSelectedHandler = function (args) {
			q.ajax({ url: settings.removeSpotFromCartUrl, type: "POST", data: { spotIdentity: args.arg.spot.identity } }).done(function (removeResult) {
				if (removeResult.isSuccess) {
					q.events().fire("updateCart", { cart: removeResult.cart });
					var identity = args.arg.spot.identity;
					var val = "available";
					q.events().fire("updateSpotState", { identity: identity, val: val });
				} else {
					alert(removeResult.message);
				}
			});
		};

		q.events().bind("availableSpotSelected", availableSpotSelectedHandler);
		q.events().bind("selectedSpotSelected", selectedSpotSelectedHandler);

		result.destroy = function () {
			q.events().unbind("availableSpotSelected", availableSpotSelectedHandler);
			q.events().unbind("selectedSpotSelected", selectedSpotSelectedHandler);
		};

		return result;
	})({}, jQuery);

	var cartControl = (function (options, $) {
		var settings = $.extend(options, {
			cartElement: $("[data-cart]"),
			cartInitializeElement: $("[data-cart-initialize]"),
			cart: { elements: [] }
		});
		var result = {};

		settings.cartElement.show();

		var fireUpdateSpotsState = function () {
			for (var elemntIndex in settings.cart.elements) {
				var element = settings.cart.elements[elemntIndex];

				q.events().fire("updateSpotState", { identity: element, val: "selected" });
			}
		};

		var updateCartHandler = function (args) {
			settings.cart = args.arg.cart;
			$(".count").text(settings.cart.elements.length);
			fireUpdateSpotsState();
		};

		q.events().bind("updateCart", updateCartHandler);
		q.events().bind("phaseRenderCompleted", fireUpdateSpotsState);

		result.destroy = function () {
			q.events().unbind("updateCart", updateCartHandler);
			q.events().unbind("phaseRenderCompleted", fireUpdateSpotsState);
		};

		if (settings.cartInitializeElement.length > 0) {
			var initializeFunction = settings.cartInitializeElement.data("cart-initialize");
			var cart = window[initializeFunction]();
			q.events().fire("updateCart", { cart: cart });
		}

		return result;
	})({}, jQuery);

	var customerTabsControl = (function (options, $) {
		var settings = $.extend(options, {
			mapTab: $("[data-tabelement='map']"),
			checkoutTab: $("[data-tabelement='checkout']"),
			checkoutSelector: $("[data-checkout]")
		});
		var result = {};

		var tabs = {
			map: function () {
				settings.mapTab.show();
				settings.checkoutTab.hide();
			},
			checkout: function () {
				settings.mapTab.hide();
				settings.checkoutTab.show();
				q.events().fire("changeTab_checkout", { element: settings.checkoutTab });
			}
		};

		var onTabChange = function (args) {
			var tab = args.arg.tab;
			tabs[tab]();
		};

		settings.checkoutSelector.click(function () {
			q.events().fire("changeTab", { tab: "checkout" });
		});

		q.events().bind("changeTab", onTabChange);

		result.destroy = function () {
			q.events().unbind("changeTab", onTabChange);
		};

		return result;
	})({}, jQuery);

	var checkoutControl = (function (options, $) {
		var settings = $.extend(options, {
			viewUrl: q.pageConfig.checkoutViewUrl
		});
		var result = {};

		var changeTabCheckoutHandler = function (args) {
			q.ajax({ url: settings.viewUrl, type: "GET" }).done(function (viewResult) {
				var element = args.arg.element;
				$(element).html(viewResult);
			});
		};

		q.events().bind("changeTab_checkout", changeTabCheckoutHandler);

		result.destroy = function () {
			q.events().unbind("changeTab_checkout", changeTabCheckoutHandler);
		};

		return result;
	})({}, jQuery);

	var numericControl = (function (options, $) {
		var settings = $.extend(options, {
			control: $("[data-numericcontrol]"),
			upControl: $("[data-numericcontrol] [data-up]"),
			downControl: $("[data-numericcontrol] [data-down]"),
			countControl: $("[data-numericcontrol] [data-count]")
		});
		var result = {};

		var updateSpotsCount = function (args) {
			var count = args.arg.count;
			settings.countControl.val(count);
		};

		var phaseChange = function () {
			settings.control.show();
		};

		q.events().bind("phaseChanged", phaseChange);

		settings.upControl.click(function () {
			q.events().fire("selectFirstAvailableSpot");
		});

		settings.downControl.click(function () {
			q.events().fire("removeFirstSpot");
		});

		q.events().bind("updateSpotsCount", updateSpotsCount);

		result.destroy = function () {
			q.events().unbind("updateSpotsCount", updateSpotsCount);
			q.events().unbind("phaseChanged", phaseChange);
		};

		return result;
	})({}, jQuery);

	var adminNumericControl = (function (options, $) {
	})({}, jQuery);

	arg.destroy = function () {
		spotsControl.destroy();
		phaseControl.destroy();
		customerControl.destroy();
		cartControl.destroy();
		customerTabsControl.destroy();
		checkoutControl.destroy();
		numericControl.destroy();
	};
});

q("mapTab_", function (arg) {
	console.log("map tab load.");

	var application = new MvcCompositeObject().
		add(new PhasesMvcObject()).
		add(new SpotsMvcObject());

	if ($("#showCustomerBookingPanel").val().toUpperCase() == "TRUE") {
		application.add(new CustomerSpotsPartialMvcObject());
	} else {
		application.add(new SponsorSpotsPartialMvcObject());
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
					// ReSharper disable UseOfImplicitGlobalInFunctionScope
					center: new google.maps.LatLng(0, 0),
					mapTypeId: google.maps.MapTypeId.ROADMAP
				};
				context.shared.gmap = new google.maps.Map(context.gmapContext.mapCanvas, mapOptions);
				// ReSharper restore UseOfImplicitGlobalInFunctionScope
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
			q.ajax({ url: this.phasesUrl + "?isJson=true", type: "GET" }).done(function (phasesResult) {
				callback(phasesResult);
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
		result._controllers.onSpotSelect = function (argSelect) {
			var context = this;
			var spotDesc = argSelect;
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
				context.execute("selectUnavailableElement", { spotDesc: argSelect });
				return;
			}

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
		result._controllers.updateSpotState = function (spotArg) {
			this.view("changeSpotState", spotArg);
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

			// ReSharper disable UseOfImplicitGlobalInFunctionScope
			var bounds = new google.maps.LatLngBounds();
			// ReSharper restore UseOfImplicitGlobalInFunctionScope
			this.existsPolygons = this.shared.existsPolygons || [];

			for (var elementIndex in spots) {
				var spot = spots[elementIndex];
				var elementPolygonCoords = [];

				for (var pointIndex in spot.points) {
					var point = spot.points[pointIndex];

					// ReSharper disable UseOfImplicitGlobalInFunctionScope
					elementPolygonCoords.push(new google.maps.LatLng(point.lng, point.lat));
					// ReSharper restore UseOfImplicitGlobalInFunctionScope
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
			// ReSharper disable UseOfImplicitGlobalInFunctionScope
			var polygon = new google.maps.Polygon({
				// ReSharper restore UseOfImplicitGlobalInFunctionScope
				paths: spotArg.paths,
				strokeColor: colors[spotDesc.val],
				strokeOpacity: 0.8,
				strokeWeight: 2,
				fillColor: colors[spotDesc.val],
				fillOpacity: 0.35
			});
			polygon.setMap(this.shared.gmap);
			spotDesc.polygon = polygon;

			// ReSharper disable UseOfImplicitGlobalInFunctionScope
			google.maps.event.addListener(polygon, 'click', function () {
				// ReSharper restore UseOfImplicitGlobalInFunctionScope
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
				for (var spotIdentity in bookedSpots.spots) {
					var spot = bookedSpots.spots[spotIdentity];
					spot.spotDesc.spot.isAvailable = false;
					spot.spotDesc.val = "unavailable";
					context.execute("updateSpotState", spot);
				}

				q.events().fire("updateCart", { elements: bookedSpots.cart.elements });

				context.execute("updateSelectedSpotsCount");
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
			spot.spotDesc.val = "available";
			spot.spotDesc.spot.selected = false;

			this.model("removeSpot", spot);
			this.execute("updateSelectedSpotsCount");
		};
		result._controllers.onSpotSelected = function (spot) {
			this.view("hideSponsorDetails");
			if (spot.spotDesc.val == "selected") {
				this.model("addSpot", spot);
			} else {
				this.execute("removeSpot", spot);
			}
			this.execute("updateSelectedSpotsCount");
		};
		result._controllers.updateSelectedSpotsCount = function () {
			var context = this;
			this.model("selectedSpotCount", function (count) {
				context.view("updateSelectedSpotsCount", count);
			});
		};
		result._controllers.selectUnavailableElement = function (argElement) {
			var spotDesc = argElement.spotDesc;
			var context = this;

			context.view("hideSponsorDetails");

			if (spotDesc.spot.sponsorId == "") {
				this.view("confirm", {
					message: "Realy remove spot?",
					onOk: function () {
						context.execute("removeBooking", { spotDesc: spotDesc });
					}
				});
			} else {
				this.model("sponsorDetails", { sponsorIdentity: spotDesc.spot.sponsorId }, function (sponsorDetails) {
					context.view("showSponsorDetails", sponsorDetails);
				});
			}
		};
		result._controllers.removeBooking = function (spotArg) {
			var context = this;
			this.model("removeBooking", spotArg, function (bookedSpots) {
				for (var spotIdentity in bookedSpots.spots) {
					var spot = bookedSpots.spots[spotIdentity];
					spot.spotDesc.spot.isAvailable = true;
					spot.spotDesc.val = "available";
					context.execute("updateSpotState", spot);
				}

				q.events().fire("updateCart", { elements: bookedSpots.cart.elements });
			});
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
		result._views.updateSelectedSpotsCount = function (model) {
			this.$panel.find("input").val(model);
		};
		result._views.confirm = function (confirmArg) {
			if (confirm(confirmArg.message)) {
				confirmArg.onOk();
			}
		};
		result._views.showSponsorDetails = function (sponsorDetails) {
			var $sponsorDetails = $(".sponsor-details").show();

			$sponsorDetails.find(".company-name").text(sponsorDetails.companyName);
			$sponsorDetails.find(".sponsor-sentence").text(sponsorDetails.sentence);
			$sponsorDetails.find(".sponsor-url").text(sponsorDetails.url);
			$sponsorDetails.find(".sponsor-logo").attr("src", sponsorDetails.logo);
		};
		result._views.hideSponsorDetails = function () {
			$(".sponsor-details").hide();
		};

		result._models.context = {
			selectedSpots: {},
			sponsorsDetails: {},
			bookingUrl: q.pageConfig.bookingForCustomerUrl,
			unbookingUrl: q.pageConfig.unbookingForCustomerUrl,
			sponsorDetailsUrl: q.pageConfig.sponsorDetailsUrl
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
				bookingResult.spots = context.selectedSpots;
				context.selectedSpots = {};

				callback(bookingResult);
			});
		};
		result._models.getFirst = function (callback) {
			for (var spotIndex in this.selectedSpots) {
				callback(this.selectedSpots[spotIndex]);
				return;
			}
		};
		result._models.selectedSpotCount = function (callback) {
			var count = 0;
			for (var index in this.selectedSpots) {
				count++;
			}

			callback(count);
		};
		result._models.removeBooking = function (spotDesc, callback) {
			q.ajax({ url: this.unbookingUrl, data: { bookedSpotIdentity: spotDesc.spotDesc.spot.identity }, type: "POST" }).done(function (booingResult) {
				booingResult.spots = [];
				if (spotDesc.spotDesc.spot.identity == booingResult.identities[0]) {
					booingResult.spots.push(spotDesc);
				}
				callback(booingResult);
			});
		};
		result._models.sponsorDetails = function (sponsorArg, callback) {
			if (this.sponsorsDetails[sponsorArg.sponsorIdentity] != undefined) {
				callback(this.sponsorsDetails[sponsorArg.sponsorIdentity]);
				return;
			}

			var context = this;
			q.ajax({ url: this.sponsorDetailsUrl, type: "GET", data: { sponsorIdentity: sponsorArg.sponsorIdentity } }).done(function (sponsorDetails) {
				context.sponsorsDetails[sponsorArg.sponsorIdentity] = sponsorDetails;
				callback(sponsorDetails);
			});
		};

		return result;
	}

	function SponsorSpotsPartialMvcObject() {
		var result = new MvcObject();

		result._controllers.initialize = function () {
			var context = this;
			this.view("showPanel", {
				onBook: function (bookingArg) {
					context.execute("booking", bookingArg);
				},
				onUp: function () {
					context.execute("selectUp");
				},
				onDown: function () {
					context.execute("selectDown");
				}
				//onCheckOut:function(phaseId, spotPrice) {
				//	q.ajax({ url: q.pageConfig.checkOut, type: "POST", data: { phaseId: phaseId, spotPrice: spotPrice } }).done(function (result) {
				//	});
				//}
			});
		};
		result._controllers.onSpotSelected = function (spot) {
			if (spot.spotDesc.val == "selected") {
				this.model("addSpot", spot);
			} else {
				this.model("removeSpot", spot);
			}

			this.execute("updateSelectedSpotsCount");
		};
		result._controllers.booking = function (bookingArg) {
			var context = this;
			this.model("bookingSpots", bookingArg, function (bookedSpots) {
				for (var spotIdentity in bookedSpots.spots) {
					var spot = bookedSpots.spots[spotIdentity];
					spot.spotDesc.spot.isAvailable = false;
					spot.spotDesc.val = "unavailable";
					context.execute("updateSpotState", spot);
				}

				context.execute("updateSelectedSpotsCount");
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
		result._controllers.updateSelectedSpotsCount = function () {
			var context = this;
			this.model("selectedSpotCount", function (count) {
				context.view("updateSelectedSpotsCount", count);
			});
		};

		result._views.context = {
			$panel: $("#sponsorsSpotSelectPanel")
		};
		result._views.showPanel = function (panelArg) {
			this.$panel.show();
			this.$panel.find("button[data-booking]").click(function () {
				panelArg.onBook({ sponsorIdentity: $("#sponsors").val() });
			});
			this.$panel.find("button[data-up]").click(function () {
				panelArg.onUp();
			});
			this.$panel.find("button[data-down]").click(function () {
				panelArg.onDown();
			});
			//this.$panel.find("button[data-checkout]").click(function () {

			//	var spotPrice = $("input#spot-price").val();
			//	var phaseId = $("li[data-identity]").attr("data-identity");

			//	panelArg.onCheckOut(phaseId,spotPrice);
			//});
		};
		result._views.updateSelectedSpotsCount = function (model) {
			this.$panel.find("input").val(model);
		};

		result._models.context = {
			selectedSpots: {},
			bookingUrl: q.pageConfig.bookingForSponsorsUrl
		};
		result._models.addSpot = function (spotArg) {
			this.selectedSpots[spotArg.spotDesc.spot.identity] = spotArg;
		};
		result._models.removeSpot = function (spotArg) {
			delete this.selectedSpots[spotArg.spotDesc.spot.identity];
		};
		result._models.getFirst = function (callback) {
			for (var spotIndex in this.selectedSpots) {
				callback(this.selectedSpots[spotIndex]);
				return;
			}
		};
		result._models.selectedSpotCount = function (callback) {
			var count = 0;
			for (var index in this.selectedSpots) {
				count++;
			}

			callback(count);
		};
		result._models.bookingSpots = function (bookingArg, callback) {
			var data = { sponsorIdentity: bookingArg.sponsorIdentity };
			var index = 0;
			for (var spotIdentity in this.selectedSpots) {
				data["identities[" + index + "]"] = spotIdentity;
				index++;
			}
			var context = this;
			q.ajax({ type: "POST", url: this.bookingUrl, data: data }).done(function (bookingResult) {
				bookingResult.spots = context.selectedSpots;
				context.selectedSpots = {};

				callback(bookingResult);
			});
		};

		return result;
	}

	arg = arg || {};

	arg.unload = function () {
		application.destroy();
		console.log("map tab unload.");
	};
});