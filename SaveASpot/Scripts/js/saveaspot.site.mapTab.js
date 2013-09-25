q("mapTab", function (arg) {

	var gmapKey = $("[data-gmap-api-key]").data("gmap-api-key");
	var gmapCanvas = document.getElementById("map-canvas");
	var gmap;
	q.controls.gmap(gmapKey, function () {
		var mapOptions = {
			zoom: 17,
			center: new window.google.maps.LatLng(40.58822748562923, -111.58563494682312),
			mapTypeId: window.google.maps.MapTypeId.ROADMAP
		};
		gmap = new google.maps.Map(gmapCanvas, mapOptions);
	});

	var controlsForDestroy = [];

	//spots control
	controlsForDestroy.push((function (options) {
		var settings = $.extend(options, {
			spotsUrl: q.pageConfig.spotsUrl,
			spotsForSquareUrl: q.pageConfig.spotsForSquareUrl,
			spotColor: "#00FF00",
			colors: {
				available: "#00FF00",
				selected: "#FFFF00",
				unavailable: "#FF0000"
			},
			goToCenter: $("[data-gocenter]"),
			messageContainer: $("[data-gmap-message-container]")
		});

		var result = {};

		var spotDescriptions = [];

		var parcelsDescriptions = [];

		var spotSelectedHandler = function () {
			q.events().fire(this.val + "SpotSelected", { spot: this.spot });
		};

		var displaySpot = function (spotDesc) {
			var colors = settings.colors;

			var polygon = new window.google.maps.Polygon({
				paths: spotDesc.paths,
				strokeColor: colors[spotDesc.val],
				strokeOpacity: 0.8,
				strokeWeight: 2,
				fillColor: colors[spotDesc.val],
				fillOpacity: 0.35
			});
			polygon.setMap(gmap);
			spotDesc.polygon = polygon;


			window.google.maps.event.addListener(polygon, 'click', function () {
				spotSelectedHandler.call(spotDesc);
			});
		};

		var displayParcel = function (parcelDesc) {
			var polygon = new window.google.maps.Polygon({
				paths: parcelDesc.paths,
				strokeColor: settings.spotColor,
				strokeOpacity: 0.8,
				strokeWeight: 2,
				fillColor: settings.spotColor,
				fillOpacity: 0.35
			});
			polygon.setMap(gmap);
			parcelDesc.polygon = polygon;
		};

		var goToCenterHandler = function () {
			var center = settings.center;
			gmap.setCenter(new window.google.maps.LatLng(center.lat, center.lng));
		};

		var clearMap = function () {
			for (var spotDescIndex in spotDescriptions) {
				var oldSpotDesc = spotDescriptions[spotDescIndex];
				oldSpotDesc.polygon.setMap(null);
			}

			spotDescriptions = [];

			for (var parcelDescIndex in parcelsDescriptions) {
				var oldParcelDesc = parcelsDescriptions[parcelDescIndex];
				oldParcelDesc.polygon.setMap(null);
			}

			parcelsDescriptions = [];
		};

		var spotsResultStrategies = {
			"Phase": function (spotsResult) {
				settings.messageContainer.html("");
				initializeParcels(spotsResult.parcels, settings.isDisplayBounds);
				settings.isDisplayBounds = false;
				q.controls.alert(settings.messageContainer, spotsResult.message, "error").show();

				if (settings.isNavigateToCenter) {
					goToCenterHandler();
				}
				settings.isNavigateToCenter = false;
			},
			"NotFound": function (spotsResult) {
				settings.messageContainer.html("");
				q.controls.alert(settings.messageContainer, spotsResult.message, "error").show();
				clearMap();

				if (settings.isNavigateToCenter) {
					goToCenterHandler();
				}
				settings.isNavigateToCenter = false;
			},
			"All": function (spotsResult) {
				settings.messageContainer.html("");
				initializeSpots(spotsResult.spots, settings.isDisplayBounds);
				settings.isDisplayBounds = false;
				window.google.maps.event.clearListeners(gmap, "idle");
				settings.isNavigateToCenter = false;
			},
			"Part": function (spotsResult) {
				settings.messageContainer.html("");
				q.controls.alert(settings.messageContainer, spotsResult.message, "info").show();
				initializeSpots(spotsResult.spots);
				settings.isNavigateToCenter = false;
			},
			"Last": function (spotsResult) {
				settings.messageContainer.html("");
				q.controls.alert(settings.messageContainer, spotsResult.message, "error").show();
				clearMap();
				settings.isNavigateToCenter = false;
			}
		};

		var refreshData = function (phaseId) {
			var mapBound = gmap.getBounds();
			var southWest = mapBound.getSouthWest();
			var northEast = mapBound.getNorthEast();
			var data = {
				phaseIdentity: phaseId,
				"topRight.Latitude": northEast.lat(),
				"topRight.Longitude": northEast.lng(),
				"bottomLeft.Latitude": southWest.lat(),
				"bottomLeft.Longitude": southWest.lng(),
			};
			q.ajax({ type: "GET", data: data, url: settings.spotsForSquareUrl }).done(function (spotsResult) {
				settings.center = spotsResult.center;
				spotsResultStrategies[spotsResult.status](spotsResult);
			});
		};

		var initializeParcels = function (parcels, isSetBound) {
			clearMap();

			var bounds = new window.google.maps.LatLngBounds();

			for (var parcelIndex in parcels) {
				var parcel = parcels[parcelIndex];

				var elementPolygonCoords = [];

				for (var pointIndex in parcel.points) {
					var point = parcel.points[pointIndex];
					elementPolygonCoords.push(new window.google.maps.LatLng(point.lng, point.lat));
				}

				var parcelDesc = {
					parcel: parcel,
					paths: elementPolygonCoords
				};

				parcelsDescriptions.push(parcelDesc);
				displayParcel(parcelDesc);

				parcelDesc.polygon.getPath().forEach(function (pointArg) {
					bounds.extend(pointArg);
				});

				if (isSetBound) {
					gmap.fitBounds(bounds);
				}
			}
		};

		var initializeSpots = function (spots, isSetBounds) {
			clearMap();

			var bounds = new window.google.maps.LatLngBounds();

			for (var spotIndex in spots) {
				var spot = spots[spotIndex];

				var elementPolygonCoords = [];

				for (var pointIndex in spot.points) {
					var point = spot.points[pointIndex];

					elementPolygonCoords.push(new window.google.maps.LatLng(point.lng, point.lat));
				}

				var spotDesc = {
					spot: spot,
					val: spot.isAvailable ? "available" : "unavailable",
					paths: elementPolygonCoords
				};

				spotDescriptions.push(spotDesc);
				displaySpot(spotDesc);

				spotDesc.polygon.getPath().forEach(function (pointArg) {
					bounds.extend(pointArg);
				});
			}
			if (isSetBounds) {
				gmap.fitBounds(bounds);
			}

			q.events().fire("phaseRenderCompleted", {});
		};

		var onPhaseChangedHandler = function (phaseArg) {
			settings.isDisplayBounds = true;
			settings.isNavigateToCenter = true;
			window.google.maps.event.clearListeners(gmap, "idle");
			var idleHandler = function () {
				refreshData(phaseArg.arg.phaseId);
			};
			window.google.maps.event.addListener(gmap, "idle", idleHandler);
			idleHandler();
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
			window.google.maps.event.clearListeners(gmap, "idle");

			console.log("destory spots control.");
		};

		return result;
	})());

	//phases control
	controlsForDestroy.push((function (options, $) {
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

	//numeric control
	controlsForDestroy.push((function (options, $) {
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
	})({}, jQuery));

	if (q.pageConfig.controlsUserConfiguration == "customer") {
		//select handlers
		controlsForDestroy.push((function () {
			var result = {};

			var availableSpotSelectedHandler = function (args) {
				q.cart.currentCart().add(args.arg.spot.identity);
			};
			var selectedSpotSelectedHandler = function (args) {
				q.cart.currentCart().remove(args.arg.spot.identity);
			};

			q.events().bind("availableSpotSelected", availableSpotSelectedHandler);
			q.events().bind("selectedSpotSelected", selectedSpotSelectedHandler);

			result.destroy = function () {
				q.events().unbind("availableSpotSelected", availableSpotSelectedHandler);
				q.events().unbind("selectedSpotSelected", selectedSpotSelectedHandler);
			};

			return result;
		})({}, jQuery));

		//map cart sync
		controlsForDestroy.push((function () {
			var result = {};

			var fireUpdateSpotsState = function () {
				var cart = q.cart.currentCart().cart();
				for (var elementIndex in cart.elements) {
					var element = cart.elements[elementIndex];
					q.events().fire("updateSpotState", { identity: element.identity, val: "selected" });
				}

				q.events().fire("updateSpotsCount", { count: cart.elements.length });
			};

			var removeElementFromCartHandler = function (removeArg) {
				var identity = removeArg.arg.identity;
				var val = "available";
				q.events().fire("updateSpotState", { identity: identity, val: val });
			};

			q.events().bind("global_cart_update", fireUpdateSpotsState);
			q.events().bind("global_cart_removeElement", removeElementFromCartHandler);
			q.events().bind("phaseRenderCompleted", fireUpdateSpotsState);

			result.destroy = function () {
				q.events().unbind("global_cart_update", fireUpdateSpotsState);
				q.events().unbind("phaseRenderCompleted", fireUpdateSpotsState);
				q.events().unbind("global_cart_removeElement", removeElementFromCartHandler);
			};

			return result;
		})({}, jQuery));

		//checkout/map/thanks switcher
		controlsForDestroy.push((function (options, $) {
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
					q.events().fire("changeTab_leave");
					switchToTab("map");
				},
				checkout: function () {
					q.events().fire("changeTab_leave");
					switchToTab("checkout");
					q.events().fire("changeTab_checkout", { element: settings.checkoutTab });
				},
				thanks: function () {
					q.events().fire("changeTab_leave");
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
		})({}, jQuery));

		controlsForDestroy.push((function () {
			var result = { _data: {} };

			var checkoutTabSwitchedHandler = function () {
				var arguments = {};
				q.runReadyHandlers({ filter: "mapTab_checkout", args: arguments });
				result._data.destroyCheckout = arguments.destory;
			};

			var changeTabLeaveHandler = function () {
				if (typeof result._data.destroyCheckout == "function") {
					result._data.destroyCheckout();
				}
			};

			q.events().bind("changeTab_checkout", checkoutTabSwitchedHandler);
			q.events().bind("changeTab_leave", changeTabLeaveHandler);

			result.destroy = function () {
				q.events().unbind("changeTab_checkout", checkoutTabSwitchedHandler);
				changeTabLeaveHandler();
			};

			return result;
		})({}, jQuery));

		//sponsors
		controlsForDestroy.push((function (options, $) {
			var settings = $.extend({
				sponsorDetailsUrl: q.pageConfig.sponsorDetailsUrl,
				spondorDetailsPanel: $(".sponsor-details")
			}, options);
			var result = {};
			var sponsorsCache = {};

			var displaySponsor = function (sponsor) {
				settings.spondorDetailsPanel.show();

				settings.spondorDetailsPanel.find(".company-name").text(sponsor.companyName);
			};

			var hideSponsorDetails = function () {
				settings.spondorDetailsPanel.hide();
			};

			var unavailableSpotSelectedHandler = function (args) {
				var sponsorIdentity = args.arg.spot.identity;
				if (sponsorsCache[sponsorIdentity] != undefined) {
					displaySponsor(sponsorsCache[sponsorIdentity]);
				} else {
					q.ajax({ type: "GET", data: { spotIdentity: sponsorIdentity }, url: settings.sponsorDetailsUrl }).done(function (sponsorResult) {
						if (sponsorResult.isExists) {
							sponsorsCache[sponsorIdentity] = sponsorResult.sponsor;
							displaySponsor(sponsorResult.sponsor);
						} else {
							hideSponsorDetails();
						}
					});
				}
			};
			q.events().bind("unavailableSpotSelected", unavailableSpotSelectedHandler);
			q.events().bind("availableSpotSelected", hideSponsorDetails);
			q.events().bind("selectedSpotSelected", hideSponsorDetails);

			result.destroy = function () {
				q.events().unbind("unavailableSpotSelected", unavailableSpotSelectedHandler);
				q.events().unbind("availableSpotSelected", hideSponsorDetails);
				q.events().unbind("selectedSpotSelected", hideSponsorDetails);
			};

			return result;
		})({}, jQuery));
	} else if (q.pageConfig.controlsUserConfiguration == "admin") {
		controlsForDestroy.push((function (options, $) {
			var settings = $.extend({
				panel: $("#sponsorsSpotSelectPanel"),
				booking: $("[data-booking]"),
				sponsors: $("#sponsors"),
				bookingUrl: q.pageConfig.bookingSpots
			}, options);
			var result = {};
			var selectedSpots = {};

			var authenticationChanged = function () {
				var user = q.security.currentUser().user();

				if (user.isAdmin) {
					settings.panel.show();
				} else {
					settings.panel.hide();
				}
			};
			var fireUpdateSpotsCount = function () {
				var count = Object.keys(selectedSpots).length;
				//for (var index in selectedSpots) {
				//	count++;
				//}

				q.events().fire("updateSpotsCount", { count: count });
			};
			var availableSpotSelectedHandler = function (args) {
				selectedSpots[args.arg.spot.identity] = args.arg.spot;
				q.events().fire("updateSpotState", { identity: args.arg.spot.identity, val: "selected" });
				fireUpdateSpotsCount();
			};
			var selectedSpotSelectedHandler = function (args) {
				delete selectedSpots[args.arg.spot.identity];
				q.events().fire("updateSpotState", { identity: args.arg.spot.identity, val: "available" });
				fireUpdateSpotsCount();
			};

			q.events().bind("availableSpotSelected", availableSpotSelectedHandler);
			q.events().bind("selectedSpotSelected", selectedSpotSelectedHandler);
			q.events().bind("global_security_authenticated", authenticationChanged);

			var bookingHandler = function () {
				var data = {};
				var arrayIndex = 0;
				for (var identityIndex in selectedSpots) {
					data["identities[" + arrayIndex + "]"] = identityIndex;
					arrayIndex++;
				}
				data.sponsorIdentity = settings.sponsors.find("option:selected").val();

				q.ajax({ type: "POST", url: settings.bookingUrl, data: data }).done(function (bookingResult) {
					var bookedSpots = bookingResult.bookedSpots;

					for (var spotIdex in bookedSpots) {
						var bookedSpot = bookedSpots[spotIdex];

						delete selectedSpots[bookedSpot];
						q.events().fire("updateSpotState", { identity: bookedSpot, val: "unavailable" });
					}
				});
			};
			settings.booking.bind("click", bookingHandler);

			result.destroy = function () {
				q.events().unbind("global_security_authenticated", authenticationChanged);
				q.events().unbind("availableSpotSelected", availableSpotSelectedHandler);
				q.events().unbind("selectedSpotSelected", selectedSpotSelectedHandler);
				settings.booking.unbind("click", bookingHandler);
			};

			return result;
		})({}, jQuery));
	}

	arg.unload = function () {
		for (var controlIndex in controlsForDestroy) {
			var control = controlsForDestroy[controlIndex];
			control.destroy();
		}
	};
});

q("mapTab_checkout", function (arg) {
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

		result.start = function () {
			settings.content.html("");
			var sortControls = function (left, right) {
				return left.controlInfo.show > right.controlInfo.show;
			};

			runControlsAction(0, checkoutControls.sort(sortControls), "show");
		};

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
			settings.orderButton.unbind("click", startOrder);

			for (var controlIndex in checkoutControls) {
				var control = checkoutControls[controlIndex];
				control.destroy();
			}
		};

		return result;
	})({}, jQuery);

	//checkout phases control
	checkoutControl.add((function (options, $) {
		var settings = $.extend(options, {
			spotsFromCartUrl: q.pageConfig.spotsFromCartUrl,
			removeSpotFromCartUrl: q.pageConfig.removeSpotFromCartUrl,
			checkoutUrl: q.pageConfig.checkoutUrl
		});
		var result = {};

		var updateCartHandler = function () {
			var $generalTemplateContainer = $("[data-template='spotInCart']");
			var templateContainerSelector = $generalTemplateContainer.data("template-selector");
			var template = $generalTemplateContainer.find(templateContainerSelector).html();

			var cart = q.cart.currentCart().cart();
			var tempalateContent = "";
			for (var elementIndex in cart.elements) {
				var element = cart.elements[elementIndex];
				tempalateContent += template.
					replace("${identity}", element.identity).
					replace("${spotIdenx}", elementIndex).
					replace("${phaseName}", element.phase.name).
					replace("${spotPrice}", element.phase.spotPrice);
			}

			var $templateContent = $("[data-template-content='spotInCart']");
			$templateContent.html(tempalateContent);

			$("[data-cart-price]").text(cart.price);

			deleteActionSelector().bind("click", deleteSpotHandler);
			deleteSelectedActionSelector().bind("click", deleteSelectedActionHandler);
		};

		var deleteSpotHandler = function () {
			var $row = $(this).parents("[data-identity]");
			var identity = $row.data("identity");

			q.cart.currentCart().remove(identity);
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

				updateCartHandler();

				q.events().bind("global_cart_update", updateCartHandler);

				showArg.complete();
			});
		};

		result.process = function (processArg) {
			var data = { spotsForCheckout: [] };

			$("[data-element='cart']").find("[data-identity]").each(function (index) {
				var spotIdentity = this.getAttribute("data-identity");
				data["spotsForCheckout[" + index + "]"] = spotIdentity;
			});

			q.ajax({ url: settings.checkoutUrl, type: "POST", data: data, dataType: "json" }).done(function (checkoutResult) {
				if (checkoutResult.isSuccess) {
					processArg.complete();
					q.cart.currentCart().cart(checkoutResult.cart);
					q.events().fire("changeTab", { tab: "thanks" });
					q.events().unbind("global_cart_update", updateCartHandler);
				} else {
					processArg.break();
				}
			});
		};

		result.destroy = function () {
			deleteActionSelector().unbind("click", deleteSpotHandler);
			deleteSelectedActionSelector().unbind("click", deleteSelectedActionHandler);
			q.events().unbind("global_cart_update", updateCartHandler);
		};

		return result;
	})({}, jQuery), { show: 10, order: 100 });

	//checkout card control
	checkoutControl.add((function (options, $) {
		var settings = $.extend(options, {
			cardInfoUrl: q.pageConfig.cardInfoUrl,
			getStripePublicKeyUrl: q.pageConfig.getStripePublicKeyUrl,
			createPaymentInformationUrl: q.pageConfig.createPaymentInformationUrl
		});
		var result = {};

		var changeAuthenticationHandler = function () {
			var fullUser = q.security.currentUser().full();

			if (fullUser.user.isCustomer && fullUser.isPaymentInfoAdded) {
				$("[data-user-payment-info='payment-panel']").hide();
				$("[data-user-payment-info='payment-info-panel']").show();
			} else {
				$("[data-user-payment-info='payment-panel']").show();
				$("[data-user-payment-info='payment-info-panel']").hide();
			}
		};

		result.show = function (showArg) {

			q.ajax({ url: settings.cardInfoUrl, type: "GET" }).done(function (cardInfoResult) {
				$(showArg.container).append(cardInfoResult);
				changeAuthenticationHandler();
				q.events().bind("global_security_authenticated", changeAuthenticationHandler);
				showArg.complete();
			});
		};

		result.process = function (processArg) {
			var fullUser = q.security.currentUser().full();

			if (fullUser.isPaymentInfoAdded == true) {
				q.events().unbind("global_security_authenticated", changeAuthenticationHandler);
				processArg.complete();
			} else {
				q.ajax({ url: settings.getStripePublicKeyUrl, type: "GET" }).done(function (publicKeyResult) {

					window.Stripe.setPublishableKey(publicKeyResult.key);
					var $form = $('#payment-form');

					window.Stripe.createToken($form, function (status, response) {
						var $f = $form;
						if (response.error) {
							$f.find('.payment-errors').text(response.error.message);
							processArg.break();
						} else {
							var token = response.id;
							$.ajax({ url: settings.createPaymentInformationUrl, type: "POST", data: { token: token } }).done(function () {
								q.events().unbind("global_security_authenticated", changeAuthenticationHandler);
								processArg.complete();
							});
						}
					});
				});
			}
		};

		result.destroy = function () {
			q.events().unbind("global_security_authenticated", changeAuthenticationHandler);
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

		var changeAuthenticationHandler = function () {
			var user = q.security.currentUser().user();

			if (user.isCustomer) {
				$("[data-user-info='authentication-panel']").hide();
				$("[data-user-info='user-info-panel']").show().find("[data-user='email']").text(user.email);
			} else {
				$("[data-user-info='authentication-panel']").show();
				$("[data-user-info='user-info-panel']").hide().find("[data-user='email']").text("");
			}
		};

		result.show = function (showArg) {
			q.ajax({ url: settings.userInfoUrl, type: "GET" }).done(function (userInfoResult) {
				$(showArg.container).append(userInfoResult);

				changeAuthenticationHandler();
				logonMethodSwitcher().bind("click", logonMethodSwitcherHandler);

				q.events().bind("global_security_authenticated", changeAuthenticationHandler);

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
						q.events().unbind("global_security_authenticated", changeAuthenticationHandler);
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
						q.events().unbind("global_security_authenticated", changeAuthenticationHandler);
						processArg.complete();
					},
					failed: function () {
						processArg.break();
					}
				}).registrate(registrationMethod);
			} else {
				processArg.complete();
				q.events().unbind("global_security_authenticated", changeAuthenticationHandler);
			}
		};

		result.destroy = function () {
			logonMethodSwitcher().unbind("click", logonMethodSwitcherHandler);
			q.events().unbind("global_security_authenticated", changeAuthenticationHandler);
		};

		return result;
	})({}, jQuery), { show: 30, order: 10 });

	checkoutControl.start();


	arg.unload = function () {
		checkoutControl.destroy();
	};
});