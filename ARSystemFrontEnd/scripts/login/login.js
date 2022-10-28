jQuery(document).ready(function () {
    $("#FormLogin").submit(function (e) {
        Form.Submit();
        e.preventDefault();
    });

    $('#tbPassword').keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            $('#btLogin').click();
            return false;
        }
    });

    $("#tbUserID").focus();

    Form.init();
    Control.init();

    var id = $("#paramUserID").val();
    var password = $("#paramUserPassword").val();
    var link = $("#urlLink").val();
    var auth = $("#isAuthenticated").val();

    if (auth && link != "" || auth && link != "undefined") {
        window.location.href = link;
    }
    else if (id && password && link != "" || id && password && link != "undefined") {
        link = link == '/Dashboard' ? '/Dashboard/Auth' : link
        LoginFromOld(id, password, link)
    }
});

function LoginFromOld(id, password, link) {
    var error = false;
    if (id == "" || password == "") {
        $("#ErrorMessage").html(" Enter any User ID and Password. ")
        $("#ErrorHolder").show();
        error = true;
    }

    if (!error) {
        var params = {
            UserID: id,
            Password: password
        }

        $.ajax({
            url: '/Login/Login',
            type: 'POST',
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (data.ErrorType > 0) {
                if (data.ErrorMessage === 'User ID has been used on another computer!') {
                    $("#ErrorMessage").html('User ID is used on another Computer, <a id="forceLogin" onclick="Form.ForceLogin.init()">Claim Login?</a>');
                    $("#ErrorHolder").show();
                }
                else {
                    $("#ErrorMessage").html(data.ErrorMessage);
                    $("#ErrorHolder").show();
                }
            } else {
                if (data.IsPasswordExpired)
                    window.location.href = '/Login/PasswordExpired';
                else
                    window.location.href = link;
            }
        })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $("#ErrorMessage").html(" Error on System. Please Call IT Help Desk! ")
                $("#ErrorHolder").show();
            })
    }
}

var Form = {
    init: function(){
        $("#pnlLogin").show();
        $("#pnlForceLogin").hide();
    },
    Submit: function () {
        var error = false;
        if ($("#tbUserID").val() == "" || $("#tbPassword").val() == "") {
            $("#ErrorMessage").html(" Enter any User ID and Password. ")
            $("#ErrorHolder").show();
            error = true;
        }

        if (!error) {
            var params = {
                UserID: $("#tbUserID").val(),
                Password: $("#tbPassword").val()
            }
            
            $.ajax({
                url: '/Login/Login',
                type: 'POST',
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                if (data.ErrorType > 0) {
                    if (data.ErrorMessage === 'User ID has been used on another computer!') {
                        $("#ErrorMessage").html('User ID is used on another Computer, <a id="forceLogin" onclick="Form.ForceLogin.init()">Claim Login?</a>');
                        $("#ErrorHolder").show();
                    }
                    else {
                        $("#ErrorMessage").html(data.ErrorMessage);
                        $("#ErrorHolder").show();
                    }
                } else {
                    if (data.IsPasswordExpired)
                        window.location.href = '/Login/PasswordExpired';
                    else
                        window.location.href = '/Dashboard/Auth';
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $("#ErrorMessage").html(" Error on System. Please Call IT Help Desk! ")
                $("#ErrorHolder").show();
            })
        }
    },
    ForceLogin: {
        init: function () {
            $("#pnlLogin").hide();
            $("#pnlForceLogin").show();
            Form.ForceLogin.SubmitOTPRequest();
            $("#ErrorHolderForceLogin").hide();
        },
        SubmitOTPRequest: function () {
            App.blockUI({
                target: '#pnlForceLogin',
                boxed: true,
                textOnly: true,
                message: 'Loading..'
            });

            var params = {
                UserID: $("#tbUserID").val(),
            };

            $.ajax({
                url: '/Login/OTPRequestForceLogin',
                type: 'POST',
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                if (data.ErrorType > 0) {
                    $("#ErrorHolderForceLogin").removeClass("alert-success");
                    $("#ErrorHolderForceLogin").addClass("alert-danger");
                    $("#ErrorMessageForceLogin").html(" Error on request OTP, please click Resend!");
                    $("#ErrorHolderForceLogin").show();
                } else {
                    $("#ErrorHolderForceLogin").removeClass("alert-danger");
                    $("#ErrorHolderForceLogin").addClass("alert-success");
                    $("#ErrorMessageForceLogin").html(" OTP Code has been sent to your TBiG Mobile!");
                    $("#ErrorHolderForceLogin").show();
                    Countdown.init(300);
                    Control.ResendButton.startCountdown();
                }
                App.unblockUI('#pnlForceLogin');
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                $("#ErrorHolderForceLogin").removeClass("alert-success");
                $("#ErrorHolderForceLogin").addClass("alert-danger");
                $("#ErrorMessageForceLogin").html(" Error on request OTP, please click Resend!");
                $("#ErrorHolderForceLogin").show();
                App.unblockUI('#pnlForceLogin');
            });
        },
        ConfirmOtp: function () {
            var params = {
                UserID: $("#tbUserID").val(),
                Code: $("#tbOTP").val(),
            };

            return $.ajax({
                url: '/Login/OTPConfirmForceLogin',
                type: 'POST',
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false
            });
        },
        SubmitForceLogin: function () {
            App.blockUI({
                target: '#pnlForceLogin',
                boxed: true,
                textOnly: true,
                message: 'Loading..'
            });

            Form.ForceLogin.ConfirmOtp().done(function (data, textStatus, jqXHR) {
                App.unblockUI('#pnlForceLogin');
                if (data.ErrorType > 0) {
                    $("#ErrorHolderForceLogin").removeClass("alert-success");
                    $("#ErrorHolderForceLogin").addClass("alert-danger");
                    $("#ErrorMessageForceLogin").html(data.ErrorMessage);
                    $("#ErrorHolderForceLogin").show();
                } else {
                    if (data.ErrorType === 0 && data.IsConfirm === true) {
                        Form.Submit();
                    }
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                App.unblockUI('#pnlForceLogin');
                $("#ErrorHolderForceLogin").removeClass("alert-success");
                $("#ErrorHolderForceLogin").addClass("alert-danger");
                $("#ErrorMessageForceLogin").html(" Error on System. Please Call IT Help Desk! ");
                $("#ErrorHolderForceLogin").show();
            });
        }
    }
}

var Control = {
    init: function () {
        this.ResendButton.init();

        $("#btnSubmitOtp").click(function(){
            Form.ForceLogin.SubmitForceLogin();
        });
    },
    ResendButton: {
        button: function () {
            $("#btnResend").unbind().click(function () {
                Form.ForceLogin.SubmitOTPRequest();
                Control.ResendButton.startCountdown();
            });
        },
        init: function () {
            this.button();
        },
        startCountdown: function(){
            $("#lbl_resend").show();
            $("#btnResend").hide();

            var sec = 30;
            var timer = setInterval(function(){
                $("#lbl_timer_resend").text(sec);
                sec--;
                if (sec < 0) {
                    clearInterval(timer);
                    $("#lbl_resend").hide();
                    $("#btnResend").show();
                }
            }, 1000);
        }
    },
};

var Countdown = {
    TIME_LIMIT: 0,
    FULL_DASH_ARRAY: 283,
    COLOR_CODES: {
        info: {
            color: "green"
        },
        warning: {
            color: "orange",
            threshold: 10,
        },
        alert: {
            color: "red",
            threshold: 5,
        }
    },
    timePassed: 0,
    timeLeft: 0,
    timerInterval: null,
    remainingPathColor: '',
    init: function (timelimit) {
        let isStartOver = this.timeLeft > 0 ? false : true;
        this.remainingPathColor = this.COLOR_CODES.info.color;
        this.TIME_LIMIT = timelimit;
        this.timePassed = 0;
        document.getElementById("countdown").innerHTML = `
            <div class="base-timer">
              <svg class="base-timer__svg" viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg">
                <g class="base-timer__circle">
                  <circle class="base-timer__path-elapsed" cx="50" cy="50" r="45"></circle>
                  <path
                    id="base-timer-path-remaining"
                    stroke-dasharray="283"
                    class="base-timer__path-remaining ${this.remainingPathColor}"
                    d="
                      M 50, 50
                      m -45, 0
                      a 45,45 0 1,0 90,0
                      a 45,45 0 1,0 -90,0
                    "
                  ></path>
                </g>
              </svg>
              <span id="base-timer-label" class="base-timer__label">${this.formatTime(
            this.timeLeft
        )}</span>
            </div>
            `;

        if(isStartOver)
            this.startTimer();
    },
    onTimesUp: function () {
        $("#ErrorHolderForceLogin").removeClass("alert-success");
        $("#ErrorHolderForceLogin").addClass("alert-danger");
        $("#ErrorMessageForceLogin").html("OTP has expired, Please click Resend Code!");
        $("#ErrorHolderForceLogin").show();

        clearInterval(timerInterval);
    },
    startTimer: function () {
        timerInterval = setInterval(() => {
            this.timePassed = this.timePassed += 1;
            this.timeLeft = this.TIME_LIMIT - this.timePassed;
            document.getElementById("base-timer-label").innerHTML = this.formatTime(
                this.timeLeft
            );
            this.setCircleDasharray();
            this.setRemainingPathColor(this.timeLeft);

            if (this.timeLeft === 0) {
                this.onTimesUp();
            }
        }, 1000);
    },
    formatTime: function (time) {
        const minutes = Math.floor(time / 60);
        let seconds = time % 60;

        if (seconds < 10) {
            seconds = `0${seconds}`;
        }

        return `${minutes}:${seconds}`;
    },
    setRemainingPathColor: function (timeLeft) {
        const { alert, warning, info } = this.COLOR_CODES;
        if (timeLeft <= alert.threshold) {
            document
                .getElementById("base-timer-path-remaining")
                .classList.remove(warning.color);
            document
                .getElementById("base-timer-path-remaining")
                .classList.add(alert.color);
        } else if (timeLeft <= warning.threshold) {
            document
                .getElementById("base-timer-path-remaining")
                .classList.remove(info.color);
            document
                .getElementById("base-timer-path-remaining")
                .classList.add(warning.color);
        }
    },
    calculateTimeFraction: function () {
        const rawTimeFraction = this.timeLeft / this.TIME_LIMIT;
        return rawTimeFraction - (1 / this.TIME_LIMIT) * (1 - rawTimeFraction);
    },
    setCircleDasharray: function () {
        const circleDasharray = `${(
            this.calculateTimeFraction() * this.FULL_DASH_ARRAY
        ).toFixed(0)} 283`;
        document
            .getElementById("base-timer-path-remaining")
            .setAttribute("stroke-dasharray", circleDasharray);
    }
};