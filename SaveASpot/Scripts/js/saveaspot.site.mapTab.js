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

	var controlsForDestroy = [];

	//spots control
	controlsForDestroy.push((function (options) {
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
			q.ajax({ url: settings.spotsUrl + "/" + phaseArg.arg.phaseId, type: "GET" }).done(function (spotResults) {

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
	})());

	//phases control
	controlsForDestroy.push((function (options, $) {
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
	})({}, jQuery));

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
			tabs: {
				map: $("[data-tabelement='map']"),
				checkout: $("[data-tabelement='checkout']"),
				thanks: $("[data-tabelement='thanks']")
			},
			checkoutSelector: $("[data-checkout]")
		});
		var result = {};

		var switchToTab = function (tabName) {
			for (var tabIndex in settings.tabs) {
				var tab = settings.tabs[tabIndex];
				tab.hide();
			}

			settings.tabs[tabName].show();
		};

		var tabs = {
			map: function () {
				switchToTab("map");
			},
			checkout: function () {
				switchToTab("checkout");
				q.events().fire("changeTab_checkout", { element: settings.checkoutTab });
			},
			thanks: function () {
				switchToTab("thanks");
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

	//main checkout control for checkout process
	var checkoutControl = (function (options, $) {
		var settings = $.extend(options, {
			element: $("[data-tabelement='checkout']"),
			orderButton: $("[data-order]"),
			content: $("[data-tabelement='checkout'] [data-checkout-content]")
		});
		var result = {};
		var checkoutControls = [];

		var runControlsAction = function (controlNumber, controls, action) {
			settings.orderButton.attr("disabled", "disabled");
			if (controls.length > controlNumber) {
				var control = controls[controlNumber].control;

				var actionValue = control[action];
				actionValue.call(control, {
					complete: function () {
						runControlsAction(controlNumber + 1, controls, action);
					},
					break: function () {
						settings.orderButton.removeAttr("disabled");
					},
					container: settings.content
				});
			} else if (controls.length == controlNumber) {
				settings.orderButton.removeAttr("disabled");
			}
		};

		var changeTabCheckoutHandler = function () {
			settings.content.html("");
			var sortControls = function (left, right) {
				return left.controlInfo.show > right.controlInfo.show;
			};

			runControlsAction(0, checkoutControls.sort(sortControls), "show");
		};

		q.events().bind("changeTab_checkout", changeTabCheckoutHandler);

		var startOrder = function () {
			var sortControls = function (left, right) {
				return left.controlInfo.order > right.controlInfo.order;
			};

			runControlsAction(0, checkoutControls.sort(sortControls), "process");
		};

		settings.orderButton.bind("click", startOrder);

		result.add = function (control, controlInfo) {
			checkoutControls.push({ control: control, controlInfo: controlInfo });

			return result;
		};

		result.destroy = function () {
			q.events().unbind("changeTab_checkout", changeTabCheckoutHandler);
			settings.orderButton.unbind("click", startOrder);

			for (var controlIndex in checkoutControls) {
				var control = checkoutControls[controlIndex];
				control.destroy();
			}
		};

		return result;
	})({}, jQuery);
	controlsForDestroy.push(checkoutControl);

	//checkout phases control
	checkoutControl.add((function (options, $) {
		var settings = $.extend(options, {
			spotsFromCartUrl: q.pageConfig.spotsFromCartUrl,
			removeSpotFromCartUrl: q.pageConfig.removeSpotFromCartUrl,
			checkoutUrl: q.pageConfig.checkoutUrl
		});
		var result = {};

		var deleteSpotHandler = function () {
			var $row = $(this).parents("[data-identity]");
			var identity = $row.data("identity");

			q.ajax({ url: settings.removeSpotFromCartUrl, type: "POST", data: { spotIdentity: identity } }).done(function (removeResult) {
				if (removeResult.isSuccess) {
					q.events().fire("updateCart", { cart: removeResult.cart });
					var val = "available";
					q.events().fire("updateSpotState", { identity: identity, val: val });
					$row.remove();
				} else {
					alert(removeResult.message);
				}
			});
		};
		var deleteActionSelector = function () { return $("[data-element='cart'] [data-action='delete']"); };

		var deleteSelectedActionHandler = function () {
			$("input:checked").each(function () {
				deleteSpotHandler.call(this);
			});
		};
		var deleteSelectedActionSelector = function () { return $("[data-element='cart'] [data-action='delete-selected']"); };

		result.show = function (showArg) {
			q.ajax({ url: settings.spotsFromCartUrl, type: "GET" }).done(function (phasesResult) {
				$(showArg.container).append(phasesResult);

				deleteActionSelector().bind("click", deleteSpotHandler);
				deleteSelectedActionSelector().bind("click", deleteSelectedActionHandler);

				showArg.complete();
			});
		};

		result.process = function (processArg) {
			var data = { spotsForCheckout: [] };

			$("[data-element='cart']").find("[data-identity]").each(function (index) {
				var spotIdentity = this.getAttribute("data-identity");
				//data.spotsForCheckout.push(spotIdentity);
				data["spotsForCheckout[" + index + "]"] = spotIdentity;
			});

			q.ajax({ url: settings.checkoutUrl, type: "POST", data: data, dataType: "json" }).done(function (checkoutResult) {
				if (checkoutResult.isSuccess) {
					processArg.complete();
					q.events().fire("changeTab", { tab: "thanks" });
				} else {
					processArg.break();
				}
			});
		};

		result.destroy = function () {
			deleteActionSelector().unbind("click", deleteSpotHandler);
			deleteSelectedActionSelector().unbind("click", deleteSelectedActionHandler);
		};

		return result;
	})({}, jQuery), { show: 10, order: 100 });

	//checkout card control
	checkoutControl.add((function (options, $) {
		var result = {};

		result.show = function (showArg) {
			showArg.complete();
		};

		result.process = function (processArg) {
			processArg.complete(); //or processArg.break()
		};

		result.destroy = function () {
		};

		return result;
	})({}, jQuery), { show: 20, order: 90 });

	//checkout user control
	checkoutControl.add((function (options, $) {
		var settings = $.extend(options, {
			userInfoUrl: q.pageConfig.userInfoUrl,
			customerAuthenticateUrl: q.pageConfig.customerAuthenticateUrl
		});
		var result = {};

		var logonMethodSwitcher = function () {
			return $("[data-customer-switcher] li");
		};
		var logonMethodSwitcherHandler = function () {
			var methods = {
				logon: function () {
					$("[data-customer-switcher] li[data-method='logon'][data-userinfo='logon']").addClass("active");
					$("[data-userinfo='logon']").show();
					$("[data-customer-switcher] li[data-method='registration']").removeClass("active");
					$("[data-userinfo='registration']").hide();
				},
				registration: function () {
					$("[data-customer-switcher] li[data-method='logon'], [data-userinfo='logon']").removeClass("active");
					$("[data-userinfo='logon']").hide();
					$("[data-customer-switcher] li[data-method='registration']").addClass("active");
					$("[data-userinfo='registration']").show();
				}
			};

			var method = this.getAttribute("data-method");
			methods[method]();
		};

		result.show = function (showArg) {
			q.ajax({ url: settings.userInfoUrl, type: "GET" }).done(function (userInfoResult) {
				$(showArg.container).append(userInfoResult);

				logonMethodSwitcher().bind("click", logonMethodSwitcherHandler);

				showArg.complete();
			});
		};

		result.process = function (processArg) {
			var logonMethod = $("[data-userinfo='logon']:visible");
			var registrationMethod = $("[data-userinfo='registration']:visible");
			if (logonMethod.length > 0) {
				q.controls.userAuthentication({
					authenticate: function (logonResult) {
						q.security.currentUser().authenticate(logonResult.user);
						logonMethod.html("Your was authenticated with email: " + logonResult.user.email);
						processArg.complete();
					},
					failed: function () {
						processArg.break();
					}
				}).authenticate(logonMethod);
			} else if (registrationMethod.length > 0) {
				q.controls.userRegistration({
					authenticate: function (logonResult) {
						q.security.currentUser().authenticate(logonResult.user);
						registrationMethod.html("Your was authenticated with email: " + logonResult.user.email);
						processArg.complete();
					},
					failed: function () {
						processArg.break();
					}
				}).registrate(registrationMethod);
			} else {
				processArg.complete();
			}
		};

		result.destroy = function () {
			logonMethodSwitcher().unbind("click", logonMethodSwitcherHandler);
		};

		return result;
	})({}, jQuery), { show: 30, order: 10 });

	var adminNumericControl = (function (options, $) {
	})({}, jQuery);

	arg.destroy = function () {
		for (var controlIndex in controlsForDestroy) {
			var control = controlsForDestroy[controlIndex];
			control.destroy();
		}

		customerControl.destroy();
		cartControl.destroy();
		customerTabsControl.destroy();
		checkoutControl.destroy();
		numericControl.destroy();
	};
});