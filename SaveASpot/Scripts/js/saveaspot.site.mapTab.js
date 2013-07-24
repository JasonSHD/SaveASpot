﻿q("mapTab", function (arg) {
	console.log("map tab load.");

	var mvc = { _controllers: {}, _views: {}, _models: {} };

	mvc._controllers.context = {};
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
		(mvc._controllers[name] || function () {
		})(argExec);
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
		alert(controllerArg.identity);
	};
	mvc._controllers.spots = function () {
	};
	
	mvc._views.context = {//view context
		navMenu: $("#dashboard-menu"),
		mapCanvas: document.getElementById("map-canvas"),
		destroy: function (handler) {
			mvc._destroy.handlers.push(handler);
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
			var map = new google.maps.Map(context.mapCanvas, mapOptions);
		});
	};

	mvc._models.context = {
		phasesUrl: q.pageConfig.phasesUrl,
		parcelsUrl: q.pageConfig.parcelsUrl
	};

	mvc._models.phases = function (callback) {
		q.ajax({ url: this.context.phasesUrl + "?isJson=true", type: "GET" }).done(function (result) {
			callback(result);
		});
	};
	mvc._models.parcels = function (phaseIdentity) {
	};
	mvc._models.spots = function (parcelsIdentity) {
	};

	mvc._controllers.phases();
	mvc._destroy = { handlers: [] };
	mvc.destroy = function () {
		for (var destoryIndex in mvc._destroy.handlers) {
			var destroyHandler = mvc._destroy.handlers[destoryIndex];
			destroyHandler();
		}
	};


	arg = arg || {};

	arg.unload = function () {
		mvc.destroy();
		console.log("map tab unload.");
	};
});