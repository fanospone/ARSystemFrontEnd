jQuery(document).ready(function () {
    Form.Init();

    $("#formTransaction").submit(function (e) {
        User.Post();
        e.preventDefault();
    });
});

var Form = {
    Init: function () {
        User.Get();
        $("#formTransaction").parsley();
    }
}

var User = {
    Get: function () {
        $.ajax({
            url: "/api/user/single",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                $("#tbUserID").val(data.UserID);
                $("#tbFullName").val(data.FullName);
                $("#tbHCISPosition").val(data.HCISPosition);
                $("#tbPhone1").val(data.Phone1);
                $("#tbPhone2").val(data.Phone2);
                $("#tbEmail").val(data.Email);
                if (data.IsActive)
                    $("#tbIsActive").val("Yes")
                else
                    $("#tbIsActive").val("No")
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    Post: function () {
        var params = {
            OldPassword: $("#tbOldPassword").val(),
            NewPassword: $("#tbNewPassword").val()
        }

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/user/password",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Password has been changed!")
                $("#tbOldPassword").val("");
                $("#tbNewPassword").val("");
                $("#tbConfirmNewPassword").val("");
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    }
}