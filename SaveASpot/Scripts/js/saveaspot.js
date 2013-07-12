(function ($) {
	var oldQ = window.q;

	window.q.runReadyHandlers = function (filter, runEmpty) {
		var readyHandlers = oldQ.readyCollection();

		for (var readyHandlerIndex in readyHandlers) {
			var readyHandler = readyHandlers[readyHandlerIndex];

			if (readyHandler.filter == filter || (readyHandler.filter === undefined && runEmpty)) {
				readyHandler.handler();
			}
		}
	};

	$(function () {
		var filter = $("#qFilter").val();

		q.runReadyHandlers(filter, true);
	});
})(jQuery);

q.controls = q.controls || {};
(function (namespace, $) {
	namespace.ajaxTab = function ($container) {
		var result = { _data: { container: $($container) } };

		$("[data-ajaxtab]").each(function () {
			$(this).click(function () {
				result.update(this);
			});
		});

		result.update = function (contextElement) {
			var $context = $(contextElement);
			var url = $context.attr("data-ajaxtab");

			$.ajax({
				type: "GET", url: url, beforeSend: function (jqXHR) {
					jqXHR.setRequestHeader("AdminTabControlHeader", "true");
				}
			}).done(function (content) {
				var readyHandlersAlias = $context.attr("data-ajaxtab-ready");
				$container.html(content);
				q.runReadyHandlers(readyHandlersAlias);
			});

			return result;
		};

		q.runReadyHandlers($("#tabSpecificReadyAlias").val());

		return result;
	};
})(q.controls, jQuery);

(function (namespace, $) {
	namespace.modal = function () {
		var result = { _data: {} };
		var $dialog = result._data.$dialog = $(".modal");
		var $okButton = $dialog.find("[data-model-ok]");

		result.title = function (title) {
			$dialog.find("[data-header]").text(title);

			return result;
		};

		result.body = function (body) {
			$dialog.find(".modal-body").html(body);

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

		$loginButton.click(function () {
			$.ajax({ url: loginUrl, type: "GET" }).done(function (dialogContent) {
				modal.title("Login").body(dialogContent).ok("Login", function () {
					modal.hide();
				}).show();
			});
		});

		return result;
	};
})(q.controls, q, jQuery);
