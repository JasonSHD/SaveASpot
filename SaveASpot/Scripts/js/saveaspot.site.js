q(function () {
	q.validation.dynamicValidator();
});

q("mapTab", function (arg) {
	console.log("map tab load.");

	//window.mapCallback = function () {
	//	readyRun();
	//};


	//q.addScript($("[data-gmap-api-url]").attr("data-gmap-api-url") + "&callback=mapCallback", function () {
	//});

	//function readyRun() {
	//	var mapOptions = {
	//		zoom: 8,
	//		center: new google.maps.LatLng(-34.397, 150.644),
	//		mapTypeId: google.maps.MapTypeId.ROADMAP
	//	};
	//	var map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);
	//}

	q.controls.gmap($("[data-gmap-api-key]").attr("data-gmap-api-key"), function () {
		var mapOptions = {
			zoom: 8,
			center: new google.maps.LatLng(-34.397, 150.644),
			mapTypeId: google.maps.MapTypeId.ROADMAP
		};
		var map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);
	});

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

	$('form#parcels .new-uploader-link, form#spots .new-uploader-link').click(function () {
		var $form = $(this).parents("form");
		$("<div>").append($("<input type='file'>").attr("name", $form.attr("data-arg-name")).attr("width", "30px").attr("height", "10px")).append("<br/><br/>").insertBefore(this);
	});

	$("#phases-and-parcels-uploader-table [data-upload-button]").click(function () {
		var $form = $(this).parents("form");
		$form.find(".alert").remove();
		var uploadType = this.getAttribute("data-upload-button");
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
				var alert = q.controls.alert($form, "All files uploaded, you can see result on <a data-link='' href='javascript:void(0)'>page</a> result.", "success").show();
				alert.content().find("[data-link]").click(function () {
					var urlAfterUpload = q.pageConfig[uploadType + "AfterUploadUrl"];
					q.controls.ajaxForm.fireUpdate({
						arg: {},
						url: urlAfterUpload,
						method: "GET",
						alias: "phasesTab",
						ajaxForm: phasePageTabAttributeValue
					});
				});
			}

			$form.find("input[type='file']").each(function () {
				if (this.files.length == 0) {
					$(this).parent().remove();
					return true;
				}

				var fileName = this.files[0].name;
				var uploadError = false;
				$(result.files).each(function () {
					return !(uploadError = (this == fileName));
				});

				if (!uploadError) {
					$(this).parent().remove();
				} else {
					q.controls.alert($form, "Next file is not uploaded: " + fileName, "error").show();
				}

				return true;
			});

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

	var $searchPanel = $("#searchPhasesAndSpotsMenu");
	$searchPanel.show();
	var $searchInput = $searchPanel.find("input").val("");

	$("[data-parcel-delete-identity]").click(function () {
		var name = this.getAttribute("data-parcel-delete-name");
		if (confirm("Are you sure that remove parcel with name '" + name + "'?") == true) {
			var parcelIdentity = this.getAttribute("data-parcel-delete-identity");
			var removeArg = { identity: parcelIdentity };
			q.controls.selector(removeArg, $searchInput.val());
			q.controls.ajaxForm.fireUpdate({
				arg: removeArg,
				url: q.pageConfig.removeParcelUrl,
				method: "POST",
				alias: "parcelsTab",
				ajaxForm: phasePageTabAttributeValue
			});
		}
	});

	arg = arg || {};

	arg.unload = function () {
		$searchPanel.hide();
		console.log("parcels group unload");
	};
});

q("spotsTab", function (arg) {
	console.log("spots group load");

	var $searchPanel = $("#searchPhasesAndSpotsMenu");
	$searchPanel.show();
	var $searchInput = $searchPanel.find("input").val("");

	$("[data-spot-delete-identity]").click(function () {
		var name = this.getAttribute("data-spot-delete-name");
		if (confirm("Are you sure that remove spot with name '" + name + "'?") == true) {
			var parcelIdentity = this.getAttribute("data-spot-delete-identity");
			var removeArg = { identity: parcelIdentity };
			q.controls.selector(removeArg, $searchInput.val());
			q.controls.ajaxForm.fireUpdate({
				arg: removeArg,
				url: q.pageConfig.removeSpotUrl,
				method: "POST",
				alias: "spotsTab",
				ajaxForm: phasePageTabAttributeValue
			});
		}
	});

	arg = arg || {};

	arg.unload = function () {
		$searchPanel.hide();
		console.log("spots group unload");
	};
});