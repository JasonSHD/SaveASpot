q(function () {
	q.validation.dynamicValidator();
});

q("mapTab", function (arg) {
	console.log("map tab load.");

	arg = arg || {};

	arg.unload = function () {
		console.log("map tab unload.");
	};
});

q("customersTab", function (arg) {
	console.log("customers tab load.");

	arg = arg || {};

	arg.unload = function () {
		console.log("customers tab unload");
	};

	var modal = q.controls.modal();

	$("#createCustomer").click(function () {
		$.ajax({ url: q.pageConfig.createCustomerView, type: "GET" }).done(function (createCustomerView) {
			modal.
				title("Create customer").
				body(createCustomerView).
				ok("Create", function () {
					var validator = q.validation.validator(modal.body());
					if (validator.validate()) {

						$.ajax({ url: q.pageConfig.createCustomerView, type: "POST", data: q.serialize(modal.body()) }).done(function (result) {
						});
					}
				}).
				show();
		});
	});
});

q("parcelsAndSpotsTab", function (arg) {
	console.log("parcels & spots tab load.");

	var ajaxForm = q.controls.ajaxForm("PhasePageTabAttribute");
	var currectAlias = $("#PhasePageTabAttribute").val();

	if (currectAlias != undefined && currectAlias != "") {
		ajaxForm.emulateUpdate(currectAlias);
	}
	
	arg = arg || {};

	arg.unload = function () {
		console.log("parcels & spots tab unload");

		ajaxForm.destroy();
	};
});

q("adminTabController", function () {
	var ajaxForm = q.controls.ajaxForm("MainMenuTabAttribute");
	var currectAlias = $("#MainMenuTabAttribute").val();

	if (currectAlias != undefined && currectAlias != "") {
		ajaxForm.emulateUpdate(currectAlias);
	}
});

q("homePage", function () {
	var loginControl = q.controls.
		login($("#loginButton"), q.pageConfig.loginUrl).
		login(function (user) {
			q.security.user().login(user);
		});

	var userInfo = q.controls.
		userInfo($("#userInfoButton"), $("[data-userinfo-popover]"), q.pageConfig.logoutUrl).
		logout(function () {
			q.security.user().logout();
		});

	q.security.user().login(function (user) {
		loginControl.hide();
		userInfo.show(user.arg);
	}).logout(function () {
		loginControl.show();
		userInfo.hide();
	});
});

q("uploadPhasesAndParcelsTab", function (arg) {
	console.log("upload phases group load.");
	arg = arg || {};

	arg.unload = function () {
		console.log("upload phases group unload");
	};
});

q("phasesTab", function (arg) {
	console.log("phases group load");

	arg = arg || {};

	arg.unload = function () {
		console.log("phases group unload.");
	};
});

q("parcelsTab", function (arg) {
	console.log("parcels group load");

	arg = arg || {};

	arg.unload = function () {
		console.log("parcels group unload");
	};
});

q("spotsTab", function (arg) {
	console.log("spots group load");

	arg = arg || {};

	arg.unload = function () {
		console.log("spots group unload");
	};
});