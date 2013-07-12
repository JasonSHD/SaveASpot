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
	q.controls.login($("#loginButton"), q.pageConfig.loginUrl);
});