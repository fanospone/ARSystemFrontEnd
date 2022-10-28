var Common = {
    //Check Error
    CheckError: {
        Object: function (data) {
            if (data.ErrorType == 0) {
                return true;
            } else if (data.ErrorType == 1) {
                Common.Alert.Warning(data.ErrorMessage);
                return false;
            } else if (data.ErrorType == 2) {
                Common.Alert.Error(data.ErrorMessage);
                return false;
            } else if (data.ErrorType == 3) {
                Common.Alert.Info(data.ErrorMessage);
                return false;
            }
        },
        List: function (data) {
            if (data.length > 0) {
                if (data[0].ErrorType == 0) {
                    return true;
                } else if (data[0].ErrorType == 1) {
                    Common.Alert.Warning(data[0].ErrorMessage);
                    return false;
                } else if (data[0].ErrorType == 2) {
                    Common.Alert.Error(data[0].ErrorMessage);
                    return false;
                }
            }
            return true
        }
    },
    //Alert
    Alert: {
        Warning: function (sa_message) {
            swal({
                title: "",
                text: sa_message,
                type: "warning",
                allowOutsideClick: true,
                confirmButtonClass: "btn-warning"
            });
        },
        Error: function (sa_message) {
            swal({
                title: "Error on System",
                text: sa_message,
                type: "error",
                allowOutsideClick: true,
                confirmButtonClass: "btn-error"
            });
        },
        Info: function (sa_message) {
            swal({
                title: "",
                text: sa_message,
                type: "info",
                allowOutsideClick: true,
                confirmButtonClass: "btn-info"
            });
        },
        Success: function (sa_message) {
            swal({
                title: "Success",
                text: sa_message,
                type: "success",
                allowOutsideClick: true,
                confirmButtonClass: "btn-success"
            });
        },
        SuccessThenRoute: function (sa_message, sa_url) {
            swal({
                title: "Success",
                text: sa_message,
                type: "success",
                allowOutsideClick: true,
                confirmButtonClass: "btn-success"
            }).then(function () {
                window.location.href = sa_url;
            });
        },
        Successhtml: function (sa_message) {
            swal({
                title: "Success",
                text: sa_message,
                type: "success",
                allowOutsideClick: true,
                confirmButtonClass: "btn-success",
                html: true

            });
        },
        WarningThenRunFunction: function (sa_message, myfunction) {
            swal({
                title: "Warning",
                text: sa_message,
                type: "warning",
                allowOutsideClick: false,
                confirmButtonClass: "btn-warning"
            }, function (isConfirm) {
                if (isConfirm) {
                    myfunction();
                }
            });
        },
        SuccessThenRunFunction: function (sa_message, myfunction) {
            swal({
                title: "Success",
                text: sa_message,
                type: "success",
                allowOutsideClick: false,
                confirmButtonClass: "btn-success"
            }, function (isConfirm) {
                if (isConfirm) {
                    myfunction();
                }
            });
        },
    },
    Format: {
        //Convert DateTime JSON to Date
        ConvertJSONDateTime: function (value) {
            if (value === null) return "";
            var dt = new Date(value);
            var month = "";
            switch (dt.getMonth() + 1) {
                case 1:
                    month = "Jan";
                    break;
                case 2:
                    month = "Feb";
                    break;
                case 3:
                    month = "Mar";
                    break;
                case 4:
                    month = "Apr";
                    break;
                case 5:
                    month = "May";
                    break;
                case 6:
                    month = "Jun";
                    break;
                case 7:
                    month = "Jul";
                    break;
                case 8:
                    month = "Aug";
                    break;
                case 9:
                    month = "Sep";
                    break;
                case 10:
                    month = "Oct";
                    break;
                case 11:
                    month = "Nov";
                    break;
                case 12:
                    month = "Dec";
                    break;
            }
            return ((parseInt(dt.getDate()) < 10) ? "0" + dt.getDate() : dt.getDate()) + "-" + month + "-" + dt.getFullYear();
        },
        //Number with Comma Separation
        CommaSeparation: function (yourNumber) {
            //Seperates the components of the number
            // Set the parameter to string, to enable replace functionality (replace won't work on float / int)
            var temp = yourNumber + "";
            var value = parseFloat(temp.replace(/,/g, ""));
            if (value != "" && !isNaN(value)) {
                var n = parseFloat(value).toFixed(2).toString().split(".");
                //Comma-fies the first part
                n[0] = n[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                //Combines the two sections
                return n.join(".");
            } else {
                return "0.00";
            }
        },
        //Terbilang (Indonesian)
        Terbilang: function (number, currency) {
            var numberSplit = parseFloat(number).toFixed(2).toString().split('.');
            var a = numberSplit[0];
            var b = numberSplit[1];
            if (b == undefined)
                b = 0;
            if (currency == undefined)
                currency = 'IDR'

            if (currency == 'IDR')
                currency = 'Rupiah';
            else
                currency = 'Dolar';
            return Common.Format.TerbilangBeforeComma(a) + " " + Common.Format.TerbilangAfterComma(b) + '' + currency;
        },
        //Terbilang Before Comma  (Indonesian)
        TerbilangBeforeComma: function (a) {
            var bilangan = ['', 'Satu', 'Dua', 'Tiga', 'Empat', 'Lima', 'Enam', 'Tujuh', 'Delapan', 'Sembilan', 'Sepuluh', 'Sebelas'];

            // 1 - 11
            if (a < 12) {
                var kalimat = bilangan[a];
            }
                // 12 - 19
            else if (a < 20) {
                var kalimat = bilangan[a - 10] + ' Belas';
            }
                // 20 - 99
            else if (a < 100) {
                var utama = a / 10;
                var depan = parseInt(String(utama).substr(0, 1));
                var belakang = a % 10;
                var kalimat = bilangan[depan] + ' Puluh ' + bilangan[belakang];
            }
                // 100 - 199
            else if (a < 200) {
                var kalimat = 'Seratus ' + Common.Format.TerbilangBeforeComma(a - 100);
            }
                // 200 - 999
            else if (a < 1000) {
                var utama = a / 100;
                var depan = parseInt(String(utama).substr(0, 1));
                var belakang = a % 100;
                var kalimat = bilangan[depan] + ' Ratus ' + Common.Format.TerbilangBeforeComma(belakang);
            }
                // 1,000 - 1,999
            else if (a < 2000) {
                var kalimat = 'Seribu ' + Common.Format.TerbilangBeforeComma(a - 1000);
            }
                // 2,000 - 9,999
            else if (a < 10000) {
                var utama = a / 1000;
                var depan = parseInt(String(utama).substr(0, 1));
                var belakang = a % 1000;
                var kalimat = bilangan[depan] + ' Ribu ' + Common.Format.TerbilangBeforeComma(belakang);
            }
                // 10,000 - 99,999
            else if (a < 100000) {
                var utama = a / 100;
                var depan = parseInt(String(utama).substr(0, 2));
                var belakang = a % 1000;
                var kalimat = Common.Format.TerbilangBeforeComma(depan) + ' Ribu ' + Common.Format.TerbilangBeforeComma(belakang);
            }
                // 100,000 - 999,999
            else if (a < 1000000) {
                var utama = a / 1000;
                var depan = parseInt(String(utama).substr(0, 3));
                var belakang = a % 1000;
                var kalimat = Common.Format.TerbilangBeforeComma(depan) + ' Ribu ' + Common.Format.TerbilangBeforeComma(belakang);
            }
                // 1,000,000 - 	99,999,999
            else if (a < 100000000) {
                var utama = a / 1000000;
                var depan = parseInt(String(utama).substr(0, 4));
                var belakang = a % 1000000;
                var kalimat = Common.Format.TerbilangBeforeComma(depan) + ' Juta ' + Common.Format.TerbilangBeforeComma(belakang);
            }
            else if (a < 1000000000) {
                var utama = a / 1000000;
                var depan = parseInt(String(utama).substr(0, 4));
                var belakang = a % 1000000;
                var kalimat = Common.Format.TerbilangBeforeComma(depan) + ' Juta ' + Common.Format.TerbilangBeforeComma(belakang);
            }
            else if (a < 10000000000) {
                var utama = a / 1000000000;
                var depan = parseInt(String(utama).substr(0, 1));
                var belakang = a % 1000000000;
                var kalimat = Common.Format.TerbilangBeforeComma(depan) + ' Milyar ' + Common.Format.TerbilangBeforeComma(belakang);
            }
            else if (a < 100000000000) {
                var utama = a / 1000000000;
                var depan = parseInt(String(utama).substr(0, 2));
                var belakang = a % 1000000000;
                var kalimat = Common.Format.TerbilangBeforeComma(depan) + ' Milyar ' + Common.Format.TerbilangBeforeComma(belakang);
            }
            else if (a < 1000000000000) {
                var utama = a / 1000000000;
                var depan = parseInt(String(utama).substr(0, 3));
                var belakang = a % 1000000000;
                var kalimat = Common.Format.TerbilangBeforeComma(depan) + ' Milyar ' + Common.Format.TerbilangBeforeComma(belakang);
            }
            else if (a < 10000000000000) {
                var utama = a / 10000000000;
                var depan = parseInt(String(utama).substr(0, 1));
                var belakang = a % 10000000000;
                var kalimat = Common.Format.TerbilangBeforeComma(depan) + ' Triliun ' + Common.Format.TerbilangBeforeComma(belakang);
            }
            else if (a < 100000000000000) {
                var utama = a / 1000000000000;
                var depan = parseInt(String(utama).substr(0, 2));
                var belakang = a % 1000000000000;
                var kalimat = Common.Format.TerbilangBeforeComma(depan) + ' Triliun ' + Common.Format.TerbilangBeforeComma(belakang);
            }

            else if (a < 1000000000000000) {
                var utama = a / 1000000000000;
                var depan = parseInt(String(utama).substr(0, 3));
                var belakang = a % 1000000000000;
                var kalimat = Common.Format.TerbilangBeforeComma(depan) + ' Triliun ' + Common.Format.TerbilangBeforeComma(belakang);
            }

            else if (a < 10000000000000000) {
                var utama = a / 1000000000000000;
                var depan = parseInt(String(utama).substr(0, 1));
                var belakang = a % 1000000000000000;
                var kalimat = Common.Format.TerbilangBeforeComma(depan) + ' Kuadriliun ' + Common.Format.TerbilangBeforeComma(belakang);
            }

            var pisah = kalimat.split(' ');
            var full = [];
            for (var i = 0; i < pisah.length; i++) {
                if (pisah[i] != "") { full.push(pisah[i]); }
            }
            return full.join(' ');
        },
        //Terbilang After Comma (Indonesian)
        TerbilangAfterComma: function (b) {
            var fullKoma = [];
            //var bilangankoma = ['Nol', 'Satu', 'Dua', 'Tiga', 'Empat', 'Lima', 'Enam', 'Tujuh', 'Delapan', 'Sembilan', 'Sepuluh', 'Sebelas'];
            //fullKoma.push("Koma");
            //// 1 - 11
            //if (b < 12) {
            //    fullKoma.push(bilangankoma[b]);
            //}
            //    // 12 - 19
            //else if (b < 20) {
            //    var kalimat = bilangan[b - 10] + ' Belas';
            //}
            //    // > 19
            //else {
            //    for (var i = 0; i < b.length; i++) {
            //        fullKoma.push(bilangankoma[b[i]]);
            //    }

            //}
            if (b == 0)
                return "";
            else {
                var bilanganKoma = ['', 'Satu', 'Dua', 'Tiga', 'Empat', 'Lima', 'Enam', 'Tujuh', 'Delapan', 'Sembilan', 'Sepuluh', 'Sebelas'];

                // 1 - 11
                if (b < 12) {
                    var kalimat = bilanganKoma[b];
                    fullKoma.push(kalimat);
                }
                    // 12 - 19
                else if (b < 20) {
                    var kalimat = bilanganKoma[b - 10] + ' Belas';
                    fullKoma.push(kalimat);

                }
                    // 20 - 99
                else if (b < 100) {
                    var utama = b / 10;
                    var depan = parseInt(String(utama).substr(0, 1));
                    var belakang = b % 10;
                    var kalimat = bilanganKoma[depan] + ' Puluh ' + bilanganKoma[belakang];
                    fullKoma.push(kalimat);

                }
                //fullKoma.push("Sen");

                return fullKoma.join(' ');
            }
        },
        FormatCurrency: function (value) {
            var value = parseFloat(value.replace(/,/g, ""));
            if (value != "" && !isNaN(value)) {
                return Common.Format.CommaSeparation(value);
            } else {
                return "0.00";
            }
        },
        //Number Comma Only without decimal format
        CommaSeparationOnly: function (yourNumber) {
            //Seperates the components of the number
            // Set the parameter to string, to enable replace functionality (replace won't work on float / int)
            var temp = yourNumber + "";
            var value = parseFloat(temp.replace(/,/g, ""));
            if (value != "" && !isNaN(value)) {
                var n = 0;
                //Comma-fies the first part
                n = value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                //Combines the two sections
                return n;
            } else {
                return "0";
            }
        },
        //Convert DateTime JSON to Date
        ConvertIndonesiaDateTime: function (value) {
            if (value === null) return "";
            var dt = new Date(value);
            var month = "";
            switch (dt.getMonth() + 1) {
                case 1:
                    month = "Januari";
                    break;
                case 2:
                    month = "Februari";
                    break;
                case 3:
                    month = "Maret";
                    break;
                case 4:
                    month = "April";
                    break;
                case 5:
                    month = "Mei";
                    break;
                case 6:
                    month = "Juni";
                    break;
                case 7:
                    month = "Juli";
                    break;
                case 8:
                    month = "Agustus";
                    break;
                case 9:
                    month = "September";
                    break;
                case 10:
                    month = "Oktober";
                    break;
                case 11:
                    month = "November";
                    break;
                case 12:
                    month = "Desember";
                    break;
            }
            return ((parseInt(dt.getDate()) < 10) ? "0" + dt.getDate() : dt.getDate()) + " " + month + " " + dt.getFullYear();
        },
    },
    PanelCNARData:
    {
        Reset: function () {
            $("#slPicaTypeARData").val("").trigger('change');
            $("#slPicaDetailARData").val("").trigger('change');
            $("#tbRemarksARData").val("");
        }
    },
    FileValidation: function (id) {
        var app = app || {};

        // Utils
        (function ($, app) {
            'use strict';
            app.utils = {};
            app.utils.formDataSuppoerted = (function () {
                return !!('FormData' in window);
            }());
        }(jQuery, app));
        //Parsley validators
        (function ($, app) {
            'use strict';
            window.Parsley.addValidator('filemaxmegabytes', {
                requirementType: 'string',
                validateString: function (value, requirement, parsleyInstance) {

                    if (!app.utils.formDataSuppoerted) {
                        return true;
                    }
                    var file = parsleyInstance.$element[0].files;
                    var maxBytes = requirement * 1048576;
                    if (file.length == 0) {
                        return true;
                    }
                    return file.length === 1 && file[0].size <= maxBytes;
                }
            }).addValidator('filemimetypes', {
                requirementType: 'string',
                validateString: function (value, requirement, parsleyInstance) {

                    if (!app.utils.formDataSuppoerted) {
                        return true;
                    }
                    var file = parsleyInstance.$element[0].files;

                    if (file.length == 0) {
                        return true;
                    }
                    var allowedMimeTypes = requirement.replace(/\s/g, "").split(',');
                    return allowedMimeTypes.indexOf(file[0].type) !== -1;
                },
            });

        }(jQuery, app));
        // Parsley Init
        (function ($, app) {
            'use strict';
            $("#" + id).parsley();
            return true;
        }(jQuery, app));
    },
    Function: {
        IsNumberKey: function (evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        },
    },
    
    Helper: {
        getUrlParameter: function (sParam) {
            var sPageURL = window.location.search.substring(1),
                sURLVariables = sPageURL.split('&'),
                sParameterName,
                i;

            for (i = 0; i < sURLVariables.length; i++) {
                sParameterName = sURLVariables[i].split('=');

                if (sParameterName[0] === sParam) {
                    return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
                }
            }
        }
    }
}

var Notification = {
    GetList: function () {

        //var html = "";
        //var count = 0;
        //count = 0;
        //html = "<li class='external'><a><span class='bold'>0 pending</span> notifications</a></li>";
        //$("#lblNotificationCount").text(count);
        //$("#dropdownMenu").html(html);

        var l = Ladda.create(document.querySelector("#btNotification"));
        $.ajax({
            url: "/api/Notification/GetList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            cache: false
        }).done(function (data) {
            var html = "";
            var count = 0;
            if (data.length > 0) {
                count = data.length;
                $.each(data, function (index, item) {
                    html += "<li class='external'><a href='" + item.TaskUrl + "'><span class='bold'>" + item.TaskCount + " pending</span> " + item.TaskName + "</a></li>";
                });
            } else {
                count = 0;
                html = "<li class='external'><a><span class='bold'>0 pending</span> notifications</a></li>";
            }
            $("#lblNotificationCount").text(count);
            $("#dropdownMenu").html(html);
        }).fail(function (data) {
            $("#lblNotificationCount").text("0");
            var html = "<li class='external'><a><span class='bold'>0 pending</span> notifications</a></li>";
            $("#dropdownMenu").html(html);
            Common.Alert.Error(data.ErrorMessage);
        })
        .always(function () {
            l.stop();
        });
    }
}

jQuery(document).ready(function () {
    $("body").delegate(".date-picker", "focusin", function () {
        $(this).datepicker({
            format: "dd-M-yyyy"
        });
    });
});

/* Custom JS for jQuery DataTables Fixed Columns Row Hover Background Color */
if (window.location.href.toLowerCase().indexOf("dashboard") == -1) { // Disable hover class on Dashboard Pages
    if (window.location.href.toLowerCase().indexOf("summaryrejection")) {

    } else {
        $(document).on({
            mouseenter: function () {
                trIndex = $(this).index() + 1;
                $("table.dataTable").each(function (index) {
                    $(this).find("tr:eq(" + trIndex + ")").addClass("hover")
                });
            },
            mouseleave: function () {
                trIndex = $(this).index() + 1;
                $("table.dataTable").each(function (index) {
                    $(this).find("tr:eq(" + trIndex + ")").removeClass("hover")
                });
            }
        }, ".dataTables_wrapper tr");
    }
}
/* End of Custom JS for jQuery DataTables Fixed Columns Row Hover Background Color */

var MappingOperatorRoundUp = ['HUAWEI']

var RAActivity = {

    HOLD: 0,
    RECONCILE_NOT_YET : 1,
    RECONCILE_PROCESSED : 2,
    RECONCILE_DONE : 3,
    BOQ_PROCESSED : 4,
    BOQ_DONE : 5,
    BOQ_REJECT : 6,
    PO_INPUT : 7,
    PO_DONE : 8,
    BAPS_INPUT : 9,
    BAPS_DONE : 10,
    BAPS_RETURN : 11,
    RTI : 12

}