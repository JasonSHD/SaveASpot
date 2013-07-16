q(function () {
	q.validation.dynamicValidator();
});

q("mapTab", function () {
	console.log("for mapTab");
});

q("customerTab", function () {
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

q("phasesTab", function () {
	console.log("for phasesTab");
});

q("adminTabController", function () {
	q.controls.ajaxTab($("#tabContent"));
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