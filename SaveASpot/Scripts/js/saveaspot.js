(function ($) {
	var oldQ = window.q;

	window.q.runReadyHandlers = function (filter, runEmpty) {
		var readyHandlers = oldQ.readyCollection();
		var config = undefined;
		var args = {};

		if (typeof filter == "object") {
			config = filter;
			filter = config.filter;
			runEmpty = config.runEmpty;
			args = config.args || {};
		}

		for (var readyHandlerIndex in readyHandlers) {
			var readyHandler = readyHandlers[readyHandlerIndex];

			if (readyHandler.filter == filter || (readyHandler.filter === undefined && runEmpty)) {
				readyHandler.handler(args);
			}
		}
	};

	$(function () {
		var filter = $("#qFilter").val();

		q.runReadyHandlers(filter, true);
	});
})(jQuery);

(function (namespace) {
	namespace.serialize = function (form) {
		var $elements = $(form).find("input");
		var result = {};

		$elements.each(function () {
			result[this.getAttribute("name")] = this.value;
		});

		return result;
	};
})(q);

(function (namespace, $) {
	namespace.each = function (collection, handler) {
		$(collection).each(handler);
	}
})(q, jQuery);

(function (namespace, $) {
	namespace.ajax = function (arg, ajaxIndicator) {
		var result = {};
		var handlers = {};

		if (ajaxIndicator != undefined)
			ajaxIndicator.show();

		$.ajax(arg).done(function (successResult) {
			(handlers.done || function () {
			})(successResult);
		}).fail(function (failResult) {
			(handlers.fail || function () {
			})(failResult);
		}).always(function () {
			if (ajaxIndicator != undefined)
				ajaxIndicator.hide();

			(handlers.always || function () {
			})();
		});

		result.done = function (handler) {
			handlers.done = handler;

			return result;
		};

		result.fail = function (handler) {
			handler.fail = handler;

			return result;
		};

		return result;
	};
})(q, jQuery);

(function (namespace, $) {
	namespace.addScript = function (url, ready) {
		if ($("script[src='" + url + "']").length > 0) {
			ready();

			return;
		}

		var el = document.createElement("script");
		var loaded = false;
		el.onload = el.onreadystatechange = function () {
			if ((el.readyState && el.readyState !== "complete" && el.readyState !== "loaded") || loaded) {
				return false;
			}

			el.onload = el.onreadystatechange = null;
			loaded = true;
			ready();
		};

		el.async = true;
		el.src = url;
		var lastScript = $("script[src]").last()[0];
		lastScript.parentNode.insertBefore(el, lastScript.nextSibling);
	};
})(q, jQuery);

(function (namespace) {
	namespace._data = namespace._data || {};
	namespace.events = function () {
		if (namespace._data.events != undefined) {
			return namespace._data.events;
		}

		var result = { _data: {} };

		result.fire = function (event, arg) {
			$(document).trigger({ type: event, arg: arg });

			return result;
		};

		result.bind = function (event, handler) {
			$(document).bind(event, handler);

			return result;
		};

		result.unbind = function (event, handler) {
			$(document).unbind(event, handler);

			return result;
		};

		return namespace._data.events = result;
	};
})(q);

(function (namespace) {
	namespace.setConfig = function (name, value) {
		var names = name.split(".");
		var currentConfig = q;
		for (var nameElementIndex in names) {
			var nameElement = names[nameElementIndex];
			if (nameElement == "q") continue;

			var memberDescription = Object.getOwnPropertyDescriptor(currentConfig, nameElement);
			var propertyValue = {};
			if (memberDescription != undefined && memberDescription.writable) {
				propertyValue = currentConfig[nameElement];
				delete currentConfig[nameElement];
			}

			if (nameElementIndex == (names.length - 1)) {
				propertyValue = value;
			}

			Object.defineProperty(currentConfig, nameElement, { writable: false, configurable: false, value: propertyValue, enumerable: true });
			currentConfig = currentConfig[nameElement];
		}

		return namespace;
	};
})(q);

q.controls = q.controls || {};
(function (namespace, $) {
	namespace.ajaxForm = function (formAlias) {
		var result = { _data: { alias: formAlias, currentEvents: { unload: function () { } } } };

		var onButtonClick = function () {
			result.update(this);
		};

		var tryUpdateEventHandler = function (arg) {
			result.updateForm(arg.arg);
		};

		var $updateButtons = $("[data-ajaxform='" + formAlias + "']");

		$updateButtons.bind("click", onButtonClick);

		q.events().bind(namespace.ajaxForm.tryUpdate + "." + formAlias, tryUpdateEventHandler);

		result.update = function (contextElement) {
			var $contextElement = $(contextElement);
			var url = $contextElement.attr("data-ajaxform-url");
			var arg = { type: "GET", url: url, arg: {}, alias: $contextElement.attr("data-ajaxform-alias") };

			return result.updateForm(arg);
		};

		result.updateForm = function (arg) {
			var url = arg.url;

			$.ajax({
				type: arg.method || "GET", url: url, data: arg.arg, beforeSend: function (jqXHR) {
					jqXHR.setRequestHeader(formAlias, "true");
				}
			}).done(function (content) {
				var readyHandlerAlias = arg.alias;
				var $ajaxForm = $("[data-ajaxform-container-" + readyHandlerAlias + "]");
				if ($ajaxForm.length == 0) {
					$ajaxForm = $("[data-ajaxform-container-" + formAlias + "]");
				}
				result._data.currentEvents.unload();

				$ajaxForm.html(content);

				var readyContext = {
					alias: readyHandlerAlias,
					update: function () {
						result.update($("[data-ajaxform-alias='" + readyHandlerAlias + "']"));
					}
				};
				q.runReadyHandlers({ filter: readyHandlerAlias, args: readyContext });
				result._data.currentEvents.unload = readyContext.unload || function () { };
			});

			return result;
		};

		result.emulateUpdate = function (alias) {
			result._data.currentEvents.unload();
			var readyContext = {
				alias: alias,
				update: function () {
					result.update($("[data-ajaxform-alias='" + alias + "']"));
				}
			};
			q.runReadyHandlers({ filter: alias, args: readyContext });
			result._data.currentEvents.unload = readyContext.unload || function () { };
		};

		result.destroy = function () {

			result._data.currentEvents.unload();
			q.events().unbind(namespace.ajaxForm.tryUpdate + "." + formAlias, tryUpdateEventHandler);
			$updateButtons.unbind("click", onButtonClick);
			$updateButtons = undefined;
		};

		return result;
	};

	namespace.ajaxForm.fireUpdate = function (arg) {
		q.events().fire(namespace.ajaxForm.tryUpdate + "." + arg.ajaxForm, arg);
	};

	namespace.ajaxForm.unloadEvent = ".unload";
	namespace.ajaxForm.tryUpdate = "ajaxFormTryUpdateEvent";
})(q.controls, jQuery);

(function (namespace) {
	namespace.selector = function (arg, searchValue) {
		arg.Search = searchValue;

		arg.PageNumber = 1;
		arg.ElementsPerPage = 10;

		return arg;
	};
})(q.controls);

(function (namespace, $) {
	namespace.ajaxIndicator = function (container, url) {
		var result = { _data: { url: url || "/Content/img/select2/spinner.gif" } };
		var $img = $("<img/>").attr("src", result._data.url);

		result.show = function () {
			$(container).append($img);

			return result;
		};

		result.hide = function () {
			$img.hide();
		};

		return result;
	};
})(q.controls, jQuery);

(function (namespace, $) {
	namespace.modal = function () {
		var result = { _data: {} };
		var $dialog = result._data.$dialog = $(".modal");
		var $okButton = $dialog.find("[data-model-ok]");
		var $modalBody = $dialog.find(".modal-body");

		result.title = function (title) {
			$dialog.find("[data-header]").text(title);

			return result;
		};

		result.body = function (body) {
			if (body === undefined) {
				return $modalBody;
			}

			$modalBody.html(body);
			return result;
		};

		result._handlers = {};
		result._handlers.onOk = function () {
			if (result._handlers.onOkPublic != undefined) {
				result._handlers.onOkPublic.call(result);
			}
		};

		result.show = function () {
			$dialog.modal("show");
			$okButton.bind("click", result._handlers.onOk);

			return result;
		};

		result.hide = function () {
			$dialog.modal("hide");
			$okButton.unbind("click", result._handlers.onOk);

			return result;
		};

		result.ok = function (text, handler) {
			$okButton.text(text);
			result._handlers.onOkPublic = handler;

			return result;
		};

		return result;
	};
})(q.controls, jQuery);

(function (namespace, q, $) {
	namespace.currentCustomer = function (options) {
		options = options || {};
		options.loginUrl = q.pageConfig.loginCustomerUrl;

		var result = {};
		var currentUserControl = namespace.currentUser();

		result.authenticate = function () {
			currentUserControl.authenticate(function (logonResult) {
			});
		};

		return result;
	};

	namespace.currentAdmin = function (options) {
		var result = {};
		var currentUser = namespace.currentUser(options);

		var userChangedHandler = function () {
			var user = q.security.currentUser().user();

			if (!user.isAdmin) {
				currentUser.authenticate(function () {
					location.reload();
				});
			}
		};

		userChangedHandler();

		q.events().bind("global_security_authenticated", userChangedHandler);

		result.destroy = function () {
			q.events().unbind("global_security_authenticated", userChangedHandler);
			currentUser.destroy();
		};

		return result;
	};

	namespace.currentUser = function (options) {
		var settings = $.extend(options, {
			loginItem: $("[data-login-item]"),
			userInfoItem: $("[data-userInfo-item]"),
			loginUrl: q.pageConfig.loginUrl,
			logoutUrl: q.pageConfig.logoutUrl
		});
		var result = { _data: { settings: settings } };

		var modal = q.controls.modal();

		var displayUserInfo = function (user) {
			settings.loginItem.hide();
			settings.userInfoItem.show();

			settings.userInfoItem.find("[data-username]").text(user.name);
			settings.userInfoItem.find("[data-email]").text(user.email);
		};

		var logoutUser = function () {
			settings.loginItem.show();
			settings.userInfoItem.hide();
		};

		var authenticationHandler = function () {
			var user = q.security.currentUser().user();

			if (user.isAnonym) {
				logoutUser();
			} else {
				displayUserInfo(user);
			}
		};

		authenticationHandler();

		q.events().bind("global_security_authenticated", authenticationHandler);

		settings.loginItem.click(function () {
			result.authenticate();
		});

		settings.userInfoItem.find("[data-logoff]").click(function () {
			q.ajax({ type: "POST", url: settings.logoutUrl }).done(function (logoutResult) {
				q.security.currentUser().authenticate(logoutResult);
				logoutUser();
			});
		});

		result.authenticate = function (callback) {
			q.ajax({ url: settings.loginUrl, type: "GET" }).done(function (dialogContext) {
				modal.
					title("Login").
					body(dialogContext).ok("Login", function () {
						namespace.userAuthentication({
							authenticate: function (logonResult) {
								modal.hide();
								q.security.currentUser().authenticate(logonResult.user);

								if (typeof callback == "function")
									callback(logonResult);
							}
						}).
							authenticate(modal.body());
					}).
					show();
			});
		};

		result.destroy = function () {
			q.events().unbind("global_security_authenticated", authenticationHandler);
		};

		return result;
	};

	namespace.userAuthentication = function (options) {
		var settings = $.extend({
			loginUrl: q.pageConfig.logonUrl,
			authenticate: function (logonResult) {
				q.security.currentUser().authenticate(logonResult.user);
			},
			failed: function () { }
		}, options);
		var result = {};

		result.authenticate = function (container) {
			var data = q.serialize(container);

			q.ajax({ type: "POST", url: settings.loginUrl, data: data }).done(function (logonResult) {
				if (logonResult.status == false) {
					$(container).find("[data-error-message]").show().find("[data-error-message-context]").text(logonResult.message);
					settings.failed();
					return;
				}

				settings.authenticate(logonResult);
			});
		};

		return result;
	};

	namespace.userRegistration = function (options) {
		var settings = $.extend({
			registrationUrl: q.pageConfig.registrationUrl,
			authenticate: function (logonResult) {
				q.security.currentUser().authenticate(logonResult.user);
			},
			failed: function () { }
		}, options);
		var result = {};

		result.registrate = function (container) {
			var data = q.serialize(container);

			q.ajax({ type: "POST", url: settings.registrationUrl, data: data }).done(function (logonResult) {
				if (logonResult.status == false) {
					$(container).find("[data-error-message]").show().find("[data-error-message-context]").text(logonResult.message);
					settings.failed();
					return;
				}

				settings.authenticate(logonResult);
			});
		};

		return result;
	};
})(q.controls, q, jQuery);

(function (namespace, $) {
	namespace.alert = function (container, html, type) {
		var result = {};
		var $container = $(container);
		var $content = $("<div>").addClass("alert alert-" + type).html(html);

		result.show = function () {
			$container.prepend($content.prepend($("<button type='button'>").attr("data-dismiss", "alert").addClass("close").text("x")))

			return result;
		};

		result.content = function () {
			return $content;
		};

		return result;
	};
})(q.controls, jQuery);

(function (namespace) {
	namespace.gmap = function (key, readyHandler) {
		var result = {};

		if (window.google != undefined) {
			readyHandler();
			return result;
		}

		var gmapGlobalCallbackName = "_____________gmapCallback_____________";
		window[gmapGlobalCallbackName] = function () {
			readyHandler();
			delete window[gmapGlobalCallbackName];
		};

		var gmapScriptUrl = "https://maps.googleapis.com/maps/api/js?key=" + key + "&sensor=true&callback=" + gmapGlobalCallbackName;
		q.addScript(gmapScriptUrl, function () {
		});

		return result;
	};
})(q.controls);

(function (namespace, $) {
	namespace._data = namespace._data || {};

	namespace.cart = function (options) {
		var settings = $.extend({
			cartElement: $("[data-cart]"),
			countSelector: $(".count")
		}, options);
		var result = { _data: { control: settings.element } };
		settings.cartElement.show();

		var updateCartControl = function () {
			var cart = q.cart.currentCart().cart();
			settings.countSelector.text(cart.elements.length);
			var elementsContainer = settings.cartElement.find(".body [data-items-container]");
			elementsContainer.html("");

			for (var cartElementIndex in cart.elements) {
				var cartElement = cart.elements[cartElementIndex];
				elementsContainer.append(
					$("<a/>").attr("href", "javascript:void(0)").addClass("item").append(
						$("<i/>").addClass("icon-map-marker")
					).append(
						"Spot " + cartElementIndex
					).append(
						$("<span/>").addClass("time").append(
							$("<i/>").addClass("icon-dollar")
						).append(
							cartElement.phase.spotPrice
						)
					)
				);

				if (cartElementIndex > 2) {
					break;
				}
			}

			settings.cartElement.find("[data-cart-amount]").text(cart.price);
		};

		updateCartControl();

		q.events().bind("global_cart_update", updateCartControl);

		result.destroy = function () {
			q.events().unbind("global_cart_update", updateCartControl);
		};

		return result;
	};
})(q.controls, jQuery);

q.validation = q.validation || {};
(function (namespace, $) {
	var extendConfig = function (config) {
		return $.extend(config, { invalidClass: "field-validation-error", validClass: "field-validation-valid" });
	};

	namespace.validator = function (form, config) {
		var result = { _data: { config: extendConfig(config) } };
		var $form = result._data.form = $(form);

		result.validate = function () {
			var isValid = true;

			var validators = [];
			$form.find("input").each(function () {
				validators.push(namespace.elementValidator(this));
			});

			$(validators).each(function () {
				isValid = this.validate(result._data.config) && isValid;
			});

			return isValid;
		};

		return result;
	};

	namespace.elementValidator = function (element) {
		var result = { _data: {} };
		var $element = result._data.element = $(element);
		var factory = namespace.attrValidator.factory();

		result.validate = function (options) {
			var validators = factory.parseElement($element);
			var isValid = true;
			$(validators).each(function () {
				isValid = this.validate();

				return isValid;
			});

			if (isValid) {
				$element.removeClass(options.invalidClass).addClass(options.validClass);
			} else {
				$element.removeClass(options.validClass).addClass(options.invalidClass);
			}

			return isValid;
		};

		return result;
	};

	namespace.attrValidator = namespace.attrValidator || {};

	namespace.dynamicValidator = function (form, config) {
		var result = { _data: { config: extendConfig(config) } };

		var $buttons = $("form button[data-submit='true']");
		$buttons.click(function () {
			var $form = $(this).parents("form");
			var validator = q.validation.validator($form, result._data.config);
			if (validator.validate()) {
				$form.submit();
			}
		});

		return result;
	};

	namespace.attrValidator.required = function (element, message) {
		var result = { _data: {}, message: function () { return message; } };
		var $element = result._data.element = $(element);

		result.validate = function () {
			var value = $element.val();
			return value != undefined && value != "";
		};

		return result;
	};

	namespace.attrValidator.lengthValidator = function (element, options) {
		var result = { _data: { options: options } };
		var $element = result._data.element = $(element);

		result.validate = function () {
			var value = $element.val();

			return result._data.options.min < value.length && value.length < result._data.options.max;
		}

		return result;
	}

	namespace.attrValidator.equalTo = function (element, message) {
		var result = { _data: {}, message: function () { return message; } };
		var $element = result._data.element = $(element);
		var $elementToEqual = result._data.elementToEqual = $($element.attr("data-val-equalTo-selector"));

		result.validate = function () {
			return $element.val() == $elementToEqual.val();
		};

		return result;
	};

	namespace.attrValidator.factory = function () {
		var result = {};

		var mappings = [];

		mappings.push({
			attr: "data-val-required",
			factory: function (element) {
				return namespace.attrValidator.required(element, $(element).attr("data-val-required"));
			},
		});

		mappings.push({
			attr: "data-val-length",
			factory: function (element) {
				var $element = $(element);
				return namespace.attrValidator.lengthValidator(element, { message: $element.attr("data-val-length"), min: $element.attr("data-val-length-min"), max: $element.attr("data-val-length-max") });
			},
		});

		mappings.push({
			attr: "data-val-equalto",
			factory: function (element) {
				return namespace.attrValidator.equalTo(element, $(element).attr("data-val-equalto"));
			}
		});

		result.parseElement = function (element) {
			var validators = [];

			$(mappings).each(function () {
				if (element[0].hasAttribute(this.attr)) {
					validators.push(this.factory(element));
				}
			});

			return validators;
		};

		return result;
	};
})(q.validation, jQuery);

q.security = q.security || {};
(function (namespace, events) {
	namespace._data = namespace._data || {};

	namespace.user = function () {
		if (namespace._data.user != undefined) {
			return namespace._data.user;
		}

		var result = { _data: {} };

		result.user = function (user) {
			if (user === undefined) {
				return result._data.user || { isDefined: false };
			}

			result._data.user = user;
			return result;
		};

		result.login = function (handler) {
			var eventName = "q_security_user_login";
			if (typeof handler === "function") {
				events().bind(eventName, handler);

				return result;
			}
			result.user(handler);
			events().fire(eventName, result.user());

			return result;
		};

		result.logout = function (hahdler) {
			var eventName = "q_security_user_logout";
			if (hahdler === undefined) {
				events().fire(eventName);
				result.user({ isDefined: false });

				return result;
			}

			events().bind(eventName, hahdler);
			return result;
		};

		return namespace._data.user = result;
	};

	namespace.currentUser = function () {
		if (namespace._data.currentUser != undefined) {
			return namespace._data.currentUser;
		}

		var result = { _data: {} };

		result.authenticate = function (user) {
			if (user == undefined) {
				result._data.user = result._data.anonym;
			} else {
				result._data.user = user;
			}

			q.events().fire("global_security_authenticated", { user: result._data.user });

			return result;
		};

		result.anonym = function (user) {
			result._data.anonym = user;

			return result;
		};

		result.user = function () {
			return result._data.user;
		};

		return namespace._data.currentUser = result;
	};

	namespace.currentCustomer = function () {
		if (namespace._data.currentCustomer != undefined) {
			return namespace._data.currentCustomer;
		}

		var result = { _data: {} };

		result.authenticate = function (customer) {
			if (customer == undefined) {
				namespace.currentUser().authenticate();

				result._data.currentCustomer = {
					user: namespace.currentUser().user()
				};
			} else {
				result._data.currentCustomer = customer;
				namespace.currentUser().authenticate(customer.user);
			}

			return result;
		};

		result.customer = function () {
			return result._data.currentCustomer;
		};

		return namespace._data.currentCustomer = result;
	};
})(q.security, q.events);

q.cart = q.cart || {};
(function (namespace) {
	var currentCart = undefined;

	namespace.currentCart = function () {
		if (currentCart != undefined) {
			return currentCart;
		}

		var result = { _data: { cart: {} } };
		var settings = {
			addSpotUrl: q.pageConfig.addSpotToCartUrl,
			removeSpotUrl: q.pageConfig.removeSpotFromCartUrl
		};

		result.cart = function (cart) {
			if (cart == undefined) {
				return result._data.cart;
			}

			result._data.cart = cart;
			q.events().fire("global_cart_update", { cart: cart });

			return result;
		};

		result.remove = function (spotIdentity) {
			q.ajax({ url: settings.removeSpotUrl, type: "POST", data: { spotIdentity: spotIdentity } }).done(function (cartResult) {
				if (cartResult.isSuccess) {
					result.cart(cartResult.cart);
					q.events().fire("global_cart_removeElement", { identity: spotIdentity });
				}
			});

			return result;
		};

		result.add = function (spotIdentity) {
			q.ajax({ url: settings.addSpotUrl, type: "POST", data: { spotIdentity: spotIdentity } }).done(function (cartResult) {
				if (cartResult.isSuccess) {
					result.cart(cartResult.cart);
				}
			});

			return result;
		};

		return currentCart = result;
	};
})(q.cart);
