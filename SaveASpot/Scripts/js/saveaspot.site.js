﻿q(function () {
	q.validation.dynamicValidator();
});

q("customersTab", function (arg) {
	console.log("customers tab load.");

	arg = arg || {};

	arg.unload = function () {
		console.log("customers tab unload");
	};

	var modal = q.controls.modal();

	$("#createCustomer").click(function () {
		q.ajax({ url: q.pageConfig.createCustomerView, type: "GET" }).done(function (createCustomerView) {
			modal.
				title("Create customer").
				body(createCustomerView).
				ok("Create", function () {
					var context = this;
					var validator = q.validation.validator(modal.body());
					if (validator.validate()) {

						$.ajax({ url: q.pageConfig.createCustomerView, type: "POST", data: q.serialize(modal.body()) }).done(function (result) {
							context.hide();
						});
					}
				}).
				show();
		});
	});
});

q("sponsorsTab", function (arg) {
	console.log("sponsors tab load.");

	arg = arg || {};

	arg.unload = function () {
		console.log("sponsors tab unload");
	};



	$("#createSponsor").click(function () {
		var modal = q.controls.modal();
		q.ajax({ url: q.pageConfig.createSponsorView, type: "GET" }).done(function (createSponsorView) {
			modal.
				title("Create sponsor").
				body(createSponsorView).
				ok("Create", function () {
					var context = this;
					var validator = q.validation.validator(modal.body());
					if (validator.validate()) {

						$.ajax({ url: q.pageConfig.createSponsorView, type: "POST", data: q.serialize(modal.body()) }).done(function (result) {
							modal.hide();
							context.hide();
							q.controls.ajaxForm.fireUpdate({
								arg: null,
								url: q.pageConfig.sponsorView,
								method: "POST",
								alias: "sponsorsTab",
								ajaxForm: "MainMenuTabAttribute"
							});
						});
					}
				}).
				show();
		});
	});

	$("[data-sponsor-edit-identity]").click(function () {
		var modal = q.controls.modal();
		var companyIdentity = this.getAttribute("data-sponsor-edit-identity");
		var companyName = $("tr[id='" + companyIdentity + "']").find('td[company-name]').text();
		var companySentence = $("tr[id='" + companyIdentity + "']").find('td[company-sentence]').text();
		var companyUrl = $("tr[id='" + companyIdentity + "']").find('td[company-url]').text();
		var companyLogo = $("tr[id='" + companyIdentity + "']").find('td[company-logo]').text();
		q.ajax({
			url: q.pageConfig.editSponsorView,
			type: "GET",
			data: {
				"SponsorViewModel.CompanyName": companyName,
				"SponsorViewModel.Sentence": companySentence,
				"SponsorViewModel.Url": companyUrl,
				"SponsorViewModel.Logo": companyLogo
			}
		}).done(function (editSponsorView) {
			modal.title("Edit sponsor").
				body(editSponsorView).ok("Save", function () {
					var itemFoUpdate = q.serialize(modal.body());
					q.ajax({
						url: q.pageConfig.editSponsorView,
						type: "POST",
						data: {
							"identity": companyIdentity,
							"SponsorViewModel.CompanyName": itemFoUpdate.CompanyName,
							"SponsorViewModel.Sentence": itemFoUpdate.Sentence,
							"SponsorViewModel.Url": itemFoUpdate.Url,
							"SponsorViewModel.Logo": itemFoUpdate.Logo
						},
					}).done(function (result) {
						modal.hide();
						q.controls.ajaxForm.fireUpdate({
							arg: null,
							url: q.pageConfig.sponsorView,
							method: "POST",
							alias: "sponsorsTab",
							ajaxForm: "MainMenuTabAttribute"
						});
					});
				}).
				show();
		});
	});

	$("[data-sponsor-delete-identity]").click(function () {
		var name = this.getAttribute("data-sponsor-delete-name");
		if (confirm("Are you sure that remove phase with name '" + name + "'?") == true) {
			var phaseIdentity = this.getAttribute("data-sponsor-delete-identity");
			var removeArg = { identity: phaseIdentity };
			q.controls.ajaxForm.fireUpdate({
				arg: removeArg,
				url: q.pageConfig.removeSponsorUrl,
				method: "POST",
				alias: "sponsorsTab",
				ajaxForm: "MainMenuTabAttribute"
			});
		}
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

	$("[data-phase-edit-identity]").click(function () {
		var modal = q.controls.modal();
		
		var phaseIdentity = this.getAttribute("data-phase-edit-identity");
		var phaseName = $("tr[phase-id='" + phaseIdentity + "']").find('td[data-phase-name]').text();
		var spotPrice = $("tr[phase-id='" + phaseIdentity + "']").find('td[data-phase-spot-price]').text();
				
		q.ajax({
			url: q.pageConfig.phaseEditUrl,
			type: "GET",
			data: {
				"PhaseViewModel.Name": phaseName,
				"PhaseViewModel.SpotPrice": spotPrice
			}
		}).done(function (editPhaseView) {
			modal.title("Edit phase").
				body(editPhaseView).ok("Save", function () {
					//var validator = q.validation.validator(modal.body());
					var itemFoUpdate = q.serialize(modal.body());
				//	if (validator.validate) {
						q.ajax({
							url: q.pageConfig.phaseEditUrl,
							type: "POST",
							data: {
								"identity": phaseIdentity,
								"PhaseViewModel.Name": itemFoUpdate.Name,
								"PhaseViewModel.SpotPrice": itemFoUpdate.SpotPrice
							},
						}).done(function (result) {
							modal.hide();
							q.controls.ajaxForm.fireUpdate({
								arg: null,
								url: q.pageConfig.phasesUrl,
								method: "POST",
								alias: "phasesTab",
								ajaxForm: phasePageTabAttributeValue
							});
						});
				//	}				
				}).
				show();
		});
		
	});

	$("[data-phase-checkout-identity]").click(function () {
		
		var name = this.getAttribute("data-phase-checkout-name");
		var phaseId = this.getAttribute("data-phase-checkout-identity");
		var spotPrice = $("tr[phase-id='" + phaseId + "']").find('td[data-phase-spot-price]').text();
		
		if (confirm("Are you sure you want to check out phase with name '" + name + "'?") == true) {
			q.ajax({ url: q.pageConfig.checkOut, type: "POST", data: { phaseId: phaseId, spotPrice: spotPrice } }).done(function (result) {
			});
		}
	});

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