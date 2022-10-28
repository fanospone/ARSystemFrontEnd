jQuery(document).ready(function () {
    $("#formPasswordExpired").submit(function (e) {
        Form.Submit();
        e.preventDefault();
    });

    $("#btLogout").unbind().click(function () {
        window.location.href = '/Login/Logout';
    });

    $("#tbOldPassword").focus();
});

var Form = {
    Submit: function () {
        var error = false;
        var newPassword = $("#tbNewPassword").val();

        if ($("#tbOldPassword").val() == "" || $("#tbNewPassword").val() == "" || $("#tbConfirmNewPassword").val() == "") {
            $("#ErrorMessage").html(" Old Password, New Password, and Confirm New Password are required! ")
            $("#ErrorHolder").show();
            error = true;
        }
        else if ($("#tbNewPassword").val().length < 8) {
            $("#ErrorMessage").html(" New Password is too short. It should have 8 characters or more! ")
            $("#ErrorHolder").show();
            error = true;
        }
        else if (!newPassword.match(/^.*(?=.*[A-Z])(?=.*[a-z])(?=.*\d).*$/)) {
            $("#ErrorMessage").html(" New Password require Uppercase, Lowercase, and Number! ")
            $("#ErrorHolder").show();
            error = true;
        }
        else if ($("#tbNewPassword").val() != $("#tbConfirmNewPassword").val()) {
            $("#ErrorMessage").html(" New Password and Confirm New Password do not match! ")
            $("#ErrorHolder").show();
            error = true;
        }

        if (!error) {
            var params = {
                OldPassword: $("#tbOldPassword").val(),
                NewPassword: $("#tbNewPassword").val()
            }
            
            $.ajax({
                url: "/api/user/password",
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                if (data.ErrorType > 0) {
                    $("#ErrorMessage").html(data.ErrorMessage)
                    $("#ErrorHolder").show();
                } else {
                    window.location.href = '/Login/Logout';
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $("#ErrorMessage").html(" Error on System. Please Call IT Help Desk! ")
                $("#ErrorHolder").show();
            })
        }
    }
}

