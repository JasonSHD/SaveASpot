var $wizard = null;

$(function () {
    $("#certificate").click(function () {
        console.log('clicked');
        $('.certificate').toggle();
    });

    $wizard = $('#fuelux-wizard'),
                $btnPrev = $('.wizard-actions .btn-prev'),
                $btnNext = $('.wizard-actions .btn-next'),
                $btnFinish = $(".wizard-actions .btn-finish");

    $wizard.wizard().on('finished', function (e) {
        // wizard complete code
    }).on("changed", function (e) {
        var step = $wizard.wizard("selectedItem");
        // reset states
        $btnNext.removeAttr("disabled");
        $btnPrev.removeAttr("disabled");
        $btnNext.show();
        $btnFinish.hide();

        if (step.step === 1) {
            $btnPrev.attr("disabled", "disabled");
        } else if (step.step === 4) {
            $btnNext.hide();
            $btnFinish.show();
        }
    });

    $btnPrev.on('click', function () {
        $wizard.wizard('previous');
    });
    $btnNext.on('click', function () {
        var step = $wizard.wizard("selectedItem");

        if (step.step === 2) {
            checkPayment();
        }
        else if (step.step === 3) {
            validateUser();
        }
        else {
            $wizard.wizard('next');
        }
    });

    $(".remove-selected").click(function () {
        var output = "";

        $(".remove-item").each(function (index, value) {
            var checked = $(this).is(':checked');
            var id = $(this).attr("data-id");

            if (checked) {
                $.post("/API/Cart/Remove/", { id: id }, function (result) {
                    if (result.success) {
                        $("#" + result.id).remove();
                    }
                });
            }
        });
    });

    $(".delete-spots").click(function (e) {
        var id = $(this).attr("href");

        $.post("/API/Cart/Remove/", { id: id }, function (result) {
            if (result.success) {
                $("#" + result.id).remove();
            }
        });

        e.preventDefault();
        return false;
    });

    $(".btn-finish").click(submitCart);
});


function stripeResponseHandler(status, response) {
    if (response.error) {
        // show the errors on the form
        setAlert(response.error.message, "alert-success", "alert-error");
        $('.submit-button').removeAttr("disabled");
    } else {
        // token contains id, last4, and card type
        var token = response['id'];
        $("#CustomerKey").val(token);
        $wizard.wizard('next');
    }
}

function setAlert(message, removeClass, addClass) {
    $(".payment-message").text(message).removeClass(removeClass).addClass(addClass).show();
}

function setUserAlert(message, removeClass, addClass) {
    $(".user-message").text(message).removeClass(removeClass).addClass(addClass).show();
}

function validateCreditCardInfo(data) {

    var validCard = Stripe.validateCardNumber(data.number);
    var validDate = Stripe.validateExpiry(data.exp_month, data.exp_year);
    var validCVC = Stripe.validateCVC(data.cvc);
    var errors = [];

    if (!validCard) { errors.push("The card number is invalid."); }
    if (!validDate) { errors.push("The expiration date is invalid."); }
    if (!validCVC) { errors.push("The CVC isn't a valid verification code."); }

    var result = {
        valid: validCard && validDate && validCVC,
        errors: errors
    };
    return result;
}

function checkPayment() {
    var data = {
        number: $('.card-number').val(),
        cvc: $('.card-cvc').val(),
        exp_month: $('.card-expiry-month').val(),
        exp_year: $('.card-expiry-year').val(),
        name: $('.card-name').val(),
        address_zip: $(".card-zip").val()
    };

    var valid = validateCreditCardInfo(data);

    if (valid.valid) {
        Stripe.createToken(data, stripeResponseHandler);
    }
    else {
        setAlert(valid.errors[0], "alert-success", "alert-error");
    }
    // prevent the form from submitting with the default action
    return false;
}

function validateUser() {
    var data = {
        userName: $("#UserName").val(),
        email: $("#Email").val(),
        password: $("#Password").val(),
        confirmPassword: $("#ConfirmPassword").val()
    };
    var errors = [];

    if (data.userName == null || data.userName == "") { errors.push("The username is invalid."); }
    if (data.email == null || data.email == "") { errors.push("The email is invalid."); }
    if (data.password == null || data.password == "") { errors.push("The password is invalid."); }
    if (data.confirmPassword == null || data.confirmPassword == "") { errors.push("The confirm password is invalid."); }
    if (data.password != data.confirmPassword) { errors.push("The password doesn't match the confirm password is invalid."); }

    var result = {
        valid: errors.length == 0,
        errors: errors
    };

    if (!result.valid) {
        setUserAlert(result.errors[0], "alert-success", "alert-error");
    }
    else {
        $wizard.wizard("next");
    }
}

function submitCart() {
    $.post("/Checkout", data, function (result) {
    });
}
