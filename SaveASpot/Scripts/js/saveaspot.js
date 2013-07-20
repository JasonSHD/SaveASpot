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

		var $updateButtons = $("[data-ajaxform='" + formAlias + "']");

		$updateButtons.bind("click", onButtonClick);

		result.update = function (contextElement) {
			var $contextElement = $(contextElement);
			var url = $contextElement.attr("data-ajaxform-url");

			$.ajax({
				type: "GET", url: url, beforeSend: function (jqXHR) {
					jqXHR.setRequestHeader(formAlias, "true");
				}
			}).done(function (content) {
				var readyHandlerAlias = $contextElement.attr("data-ajaxform-alias");
				var $ajaxForm = $("[data-ajaxform-container-" + readyHandlerAlias + "]");
				if ($ajaxForm.length == 0) {
					$ajaxForm = $("[data-ajaxform-container-" + formAlias + "]");
				}
				result._data.currentEvents.unload();

				$ajaxForm.html(content);

				var readyContext = { alias: readyHandlerAlias };
				q.runReadyHandlers({ filter: readyHandlerAlias, args: readyContext });
				result._data.currentEvents.unload = readyContext.unload || function () { };
			});

			return result;
		};

		result.emulateUpdate = function (alias) {
			result._data.currentEvents.unload();
			var readyContext = { alias: alias };
			q.runReadyHandlers({ filter: alias, args: readyContext });
			result._data.currentEvents.unload = readyContext.unload || function () { };
		};

		result.destroy = function () {
			$updateButtons.unbind("click", onButtonClick);
			$updateButtons = undefined;
		};

		return result;
	};

	namespace.ajaxForm.unloadEvent = ".unload";
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
			(result._handlers.onOkPublic || function () {
			})();
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
	namespace.login = function (loginButton, loginUrl) {
		var $loginButton = $(loginButton);
		var result = { _data: { loginUrl: loginUrl, loginButton: $loginButton } };
		var modal = q.controls.modal();
		var validator = q.validation.validator(modal.body());

		result.login = function (handler) {
			result._data.loginHandler = handler;
			return result;
		};

		$loginButton.click(function () {
			$.ajax({ url: loginUrl, type: "GET" }).done(function (dialogContent) {
				modal.title("Login").body(dialogContent).ok("Login", function () {
					if (validator.validate()) {
						var data = q.serialize(modal.body());
						var logonUrl = modal.body().find("[data-logon-url]").attr("data-logon-url");

						$.ajax({ type: "POST", url: logonUrl, data: data }).done(function (logonResult) {
							if (logonResult.status == false && logonResult.message != undefined) {
								modal.body().find("[data-error-message]").show().find("[data-error-message-content]").text(logonResult.message);
								return;
							}

							modal.hide();
							(result._data.loginHandler || function () {
							})(logonResult);
						});
					}
				}).show();
			});
		});

		result.hide = function () {
			$loginButton.hide();

			return result;
		};

		result.show = function () {
			$loginButton.show();

			return result;
		};

		return result;
	};

	namespace.userInfo = function (loginfoButton, userinfoPanel, logoutUrl) {
		var result = { _data: {} };
		var $loginfoButton = result._data.loginfoButton = $(loginfoButton);
		var $userinfoPanel = result._data.userinfoPanel = $(userinfoPanel);
		var $logoutButton = $userinfoPanel.find(".logout");

		$logoutButton.click(function () {
			$.ajax({ url: logoutUrl, type: "POST" }).done(function () {
				(result._data.logoutHandler || function () {
				})();
			});
		});

		result.logout = function (handler) {
			result._data.logoutHandler = handler;

			return result;
		};

		result.show = function (user) {
			$loginfoButton.show();
			$userinfoPanel.find("[data-username]").text(user.name);
			$userinfoPanel.find("[data-email]").text(user.email);

			return result;
		};

		result.hide = function () {
			$loginfoButton.hide();

			return result;
		};

		return result;
	};
})(q.controls, q, jQuery);

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
})(q.security, q.events);
