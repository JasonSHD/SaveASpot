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

var phasePageTabAttributeValue = "PhasePageTabAttribute";
q("parcelsAndSpotsTab", function (arg) {
	console.log("parcels & spots tab load.");

	var ajaxForm = q.controls.ajaxForm(phasePageTabAttributeValue);
	var currectAlias = $("#" + phasePageTabAttributeValue).val();

	if (currectAlias != undefined && currectAlias != "") {
		ajaxForm.emulateUpdate(currectAlias);
	}

	arg = arg || {};

	arg.unload = function () {
		ajaxForm.destroy();

		console.log("parcels & spots tab unload");
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

	$('form#parcels .new-uploader-link, form#spots .new-upload-link').click(function () {
		var $lastInput = $(this).parent().find("input[type='file']:last");
		$lastInput.
			after($("<input type='file'/>").attr("name", $lastInput.attr("name")).attr("width", "30px").attr("height", "10px")).
			after("<br/><br/>");
	});

	$("#phases-and-parcels-uploader-table [data-upload-button]").click(function () {
		var $form = $(this).parents("form");
		var url = $form.attr("action");
		var formData = new FormData($form[0]);
		$.ajax({
			url: url,
			type: 'POST',
			data: formData,
			cache: false,
			contentType: false,
			processData: false
		}).done(function (result) {
			if (result.status == true) {
				q.controls.ajaxForm.fireUpdate({
					arg: {},
					url: q.pageConfig.afterUploadParcelsUrl,
					method: "GET",
					alias: "phasesTab",
					ajaxForm: phasePageTabAttributeValue
				});
			} else {
				$form.find("input[type='file']").each(function () {
					if (this.files.length == 0) {
						$(this).remove();
						return true;
					}

					var fileName = this.files[0].name;
					var uploadError = false;
					$(result.files).each(function () {
						return !(uploadError = (this == fileName));
					});

					if (!uploadError) {
						$(this).remove();
					} else {
						q.controls.alert($form, "Next file is not uploaded: " + fileName);
					}
				});
			}
		});
	});

	arg.unload = function () {
		//console.log("upload phases group unload");
	};
});

q("phasesTab", function (arg) {
	console.log("phases group load");

	var $searchPanel = $("#searchPhasesAndSpotsMenu");
	$searchPanel.show();
	var $searchInput = $searchPanel.find("input").val("");

	$("[data-phase-delete-identity]").click(function () {
		var name = this.getAttribute("data-phase-delete-name");
		if (confirm("Are you sure that remove phase with name '" + name + "'?") == true) {
			var phaseIdentity = this.getAttribute("data-phase-delete-identity");
			var removeArg = { identity: phaseIdentity };
			q.controls.selector(removeArg, $searchInput.val());
			q.controls.ajaxForm.fireUpdate({
				arg: removeArg,
				url: q.pageConfig.removePhaseUrl,
				method: "POST",
				alias: "phasesTab",
				ajaxForm: phasePageTabAttributeValue
			});
		}
	});

	arg = arg || {};

	arg.unload = function () {
		$searchPanel.hide();
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