q(function () {
	q.validation.dynamicValidator();
});

q("mapTab", function () {
	console.log("for mapTab");
});

q("customerTab", function () {
	console.log("for customerTab");
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