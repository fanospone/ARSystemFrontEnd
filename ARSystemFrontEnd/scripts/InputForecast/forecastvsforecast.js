jQuery(document).ready(function () {
    trxARInputForecastVsForecast.Init();
    $('#btnSendApprovalFcVsFc').on('click', function (e) {
        e.preventDefault();
        trxARInputForecastVsForecast.SendToApprovalClick();
    });
    $('#btnSendApprovalFormFcVsFc').on('click', function (e) {
        e.preventDefault();
        trxARInputForecastVsForecast.SubmitApprovalForm();

    });
});
trxARInputForecastVsForecast = {
        Init: function () {
            var tblSummaryData = $('#tbltrxARInputForecastVsForecast').dataTable({
                "filter": false,
                "destroy": true,
                "data": []
            });

            $("#tbltrxARInputForecastVsActual tbody").on("click", ".btDetail", function (e) {
                var tblSummaryData = $("#tbltrxARInputForecastVsForecast").DataTable();
                var tr = $(this).closest('tr');
                var row = tblSummaryData.row(tr);
                var dt = row.data();
                // you get All data row table in (dt)
            });

            $(window).resize(function () {
                $("#tbltrxARInputForecastVsForecast").DataTable().columns.adjust().draw();
            });

            // unremark if you call funtion
            //Table.trxARInputForecastVsActual.Search();
        },
        Search: function () {
            var l = Ladda.create(document.querySelector("#btSearch"))
            l.start();
            App.blockUI({
                target: "#gridPnltrxARInputForecastVsForecast", boxed: true
            });

            params = {

                Year: $('#fyear').val(),
                Quarter: $('#fquarter').val(),

            };
            var currentQuarter = $('#fquarter').val();
            var monthsInQuarter = (currentQuarter == 1) ? ['JAN', 'FEB', 'MAR'] : (currentQuarter == 2) ? ['APR', 'MAY', 'JUN'] :
                    (currentQuarter == 3) ? ['JUL', 'AUG', 'SEP'] : (currentQuarter == 4) ? ['OCT', 'NOV', 'DEC'] : ['', '', ''];
            var today = new Date();
            var todayQuarter = Math.floor(((today.getMonth()) + 2) / 3);
            var numberOfMonthWithinQuarter = 0;
            var startMonth = currentQuarter * 3 - 2;
            for (var i = 0; i < 3; i++)
            {
                if (today.getMonth() == (startMonth + i))
                {
                    numberOfMonthWithinQuarter = i + 1;
                }
            }
            console.log(numberOfMonthWithinQuarter)
            var tblSummaryData = $("#tbltrxARInputForecastVsForecast").DataTable({
                "proccessing": true,
                "serverSide": true,
                "language": {
                    "emptyTable": "No data available in table",
                },
                "ajax": {
                    "url": "/api/ApiInputForecastCashIn/loadForecastVsForecast",
                    "type": "POST",
                    "datatype": "json",
                    "data": params,
                },
              
                "filter": false,
                "lengthMenu": [[25, 50], ['25', '50']],
                "destroy": true,
                "columns": [
                    {
                        orderable: false,
                        mRender: function (data, type, full) {
                            var strReturn = "";
                            if (isFcFcApproval == '' || isFcFcApproval == 'True')
                                {
                                strReturn += "<button type='button' title='Edit' class='btn btn-xs green btEdit' onClick='trxARInputForecastVsForecast.UpdateForecastClick(\"" + full.OperatorID + "\", \"" + full.Year + "\", \"" + full.Quarter + "\")'><i class='fa fa-edit'></i></button>";
                                return strReturn;
                                }else{
                                return ''
                                }
                        }
                    },
                { data: 'OperatorID' },
                {
                    data: 'TotalForecastM1Idr', render: function (data) {
                        return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                    }
                },
                {
                    data: 'TotalForecastM1Usd', render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: 'TotalForecastM2Idr', render: function (data) {
                        return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                    }
                },
                {
                    data: 'TotalForecastM2Usd', render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: 'TotalForecastM3Idr', render: function (data) {
                        return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                    }
                },
                {
                    data: 'TotalForecastM3Usd', render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: 'TotalLastPeriodIdr', render: function (data) {
                        return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                    }, className: "bg-color-total"
                },
                {
                    data: 'TotalLastPeriodUsd', render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }, className: "bg-color-total"
                },


                {
                    data: 'TotalActualM1Idr', render: function (data) {
                        return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                    }
                },
                 {
                     data: 'TotalActualM1Usd', render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                {
                    data: 'TotalActualM2Idr', render: function (data) {
                        return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                    }
                },
                 {
                     data: 'TotalActualM2Usd', render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                {
                    data: 'TotalActualM3Idr', render: function (data) {
                        return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                    }
                },
               
                {
                    data: 'TotalActualM3Usd', render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: 'TotalCurrentPeriodIdr', render: function (data) {
                        return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                    }, className: "bg-color-total"
                },
                {
                    data: 'TotalCurrentPeriodUsd', render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }, className: "bg-color-total"
                },
                {
                    data: 'VarianceIdr', render: function (data) {
                        return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                    }
                },
                {
                    data: 'VarianceUsd', render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },

                {
                    data: 'Remarks', render: function (data, type, full) {
                        return data != null ? "<div onClick='trxARInputForecastVsForecast.UpdateForecastClick(\"" + full.OperatorID + "\", \"" + full.Year + "\", \"" + full.Quarter + "\", \"ViewPica\")'><span class='picasText'>View...</span></div>" : "";
                }},
                ],
                "columnDefs": [

               { 'targets': [1], 'width': '100%', 'className': 'dt-left', 'visible': true },
               { 'targets': [2], 'width': '100%', 'className': 'dt-left', 'visible': true },
               { 'targets': [3], 'width': '100%', 'className': 'dt-left', 'visible': true },
               { 'targets': [4], 'width': '100%', 'className': 'dt-left', 'visible': true },
               { 'targets': [5], 'width': '100%', 'className': 'dt-left', 'visible': true },

                ],
                "fnPreDrawCallback": function () {
                    App.blockUI({ target: ".gridPnltrxARInputForecastVsForecast", boxed: true });
                },
                "fnDrawCallback": function () {
                    if (Common.CheckError.List(tblSummaryData.data())) {
                        $(".panelSearchBegin").hide();
                        $(".panelSearchResult").show();
                    }
                    $("#gridPnltrxARInputForecastVsForecast .sGridTotalQuarter").html("Total Q" + currentQuarter + " " + params.Year);
                    $("#gridPnltrxARInputForecastVsForecast .sGridVarianceQuarter").html("Variance Q" + currentQuarter + " " + params.Year);

                    $("#gridPnltrxARInputForecastVsForecast .sGridMonth1").html(monthsInQuarter[0]);
                    $("#gridPnltrxARInputForecastVsForecast .sGridMonth2").html(monthsInQuarter[1]);
                    $("#gridPnltrxARInputForecastVsForecast .sGridMonth3").html(monthsInQuarter[2]);
                    var comb = settingVariable.yearOfInputPica + settingVariable.quarterOfInputPica + settingVariable.numberOfMonthInputPica;
                    $("#gridPnltrxARInputForecastVsForecast .sGridTypeLPeriodM1").html((comb <= params.Year + params.Quarter + 1) ? "Forecast" : "Actual");
                    $("#gridPnltrxARInputForecastVsForecast .sGridTypeLPeriodM2").html((comb <= params.Year + params.Quarter + 2) ? "Forecast" : "Actual");
                    $("#gridPnltrxARInputForecastVsForecast .sGridTypeLPeriodM3").html((comb <= params.Year + params.Quarter + 3) ? "Forecast" : "Actual");

                    $("#gridPnltrxARInputForecastVsForecast .sGridTypeCPeriodM1").html((comb < params.Year + params.Quarter + 1) ? "Forecast" : "Actual");
                    $("#gridPnltrxARInputForecastVsForecast .sGridTypeCPeriodM2").html((comb < params.Year + params.Quarter + 2) ? "Forecast" : "Actual");
                    $("#gridPnltrxARInputForecastVsForecast .sGridTypeCPeriodM3").html((comb < params.Year + params.Quarter + 3) ? "Forecast" : "Actual");

                    l.stop();
                    App.unblockUI('#gridPnltrxARInputForecastVsForecast');
                },
                "createdRow": function (row, data, dataIndex) {
                    if (data.VarianceIdr < 0) {
                        $(row).find('td:nth-child(19)').attr('style', 'background-color : #ea6157 !important; color: #fff;')
                    }
                    if (data.VarianceUsd < 0) {
                        $(row).find('td:nth-child(20)').attr('style', 'background-color : #ea6157 !important; color: #fff;')
                    }

                },
                "order": []
            });
        },
        Export: function () {

            varYear = $('#fyear').val();
            varQuarter = $('#fquarter').val();

            window.location.href = "/Controller/ActionName/Export?FCMarketing=" + FCMarketing + "&FCRevenueAssurance=" + FCRevenueAssurance + "&FCFinance=" + FCFinance + "&Actual=" + Actual + "&PiCa=" + PiCa
        },
        Reset: function () {
            Filter.ddlfYear('#fyear');
            Filter.ddlfQuarter('#fquarter');
            trxARInputForecastVsForecast.Init();
        },
        InitModal: function () {
            $("#btCloseFcactual").unbind().click(function (e) {
                e.preventDefault();
                $('#mdlUpdateForecastActual').modal('hide');
            });
            $("#btSubmitFcactual").unbind().click(function (e) {
                e.preventDefault();
                ModalUploadInflasi.SubmitUpload();
            });
            $("#mdlUpdateForecastActual").modal({ backdrop: 'static', keyboard: false }).show();
        },
        ResetModal: function () { //Modal.Reset
            var srt = $("script#mdlUpdateForecastActualHtml").html();
            $("#mdlUpdateForecastContainer").html(srt);
        },
        UpdateForecastClick: function (OperatorID, Year, Quarter, callfrom) {
            App.blockUI({
                target: "#gridPnltrxARInputForecastVsForecast", boxed: true
            });
            var url = "/InputForecastCashIn/UpdateForecastVsForecast";
            $.ajax({
                url: url,
                type: "post",
                data: { OperatorID: OperatorID, Year: Year, Quarter: Quarter },
                cache: false,
            }).done(function (data, textStatus, jqXHR) {
                $("#mdlUpdateForecastContainer").html(data);
                $('#mdlUpdateForecastForecast').modal('show');
                if (callfrom == "ViewPica") {
                    $('mdlUpdateForecastForecast .form-control').attr('readonly', true)
                    $('#btSubmitFormForecastForecast').css('display', 'none')
                } else {
                    $('#btSubmitFormForecastForecast').on('click', function (e) {
                        e.preventDefault();
                        trxARInputForecastVsForecast.SubmitModal();
                    });
                }

                $('.btCancelFormForecastForecast').on('click', function (e) {
                    e.preventDefault();
                    $('#mdlUpdateForecastForecast').modal('hide');
                });

                App.unblockUI('#gridPnltrxARInputForecastVsForecast');
            }).fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                App.unblockUI('#gridPnltrxARInputForecastVsForecast');
            })
        },
        SubmitModal: function () {
            var l = Ladda.create(document.querySelector("#btSubmitFormForecastForecast"))
            l.start();
            App.blockUI({
                target: "#mdlUpdateForecastForecast .modal-content", boxed: true
            });
 
            var url = "/api/ApiInputForecastCashIn/submitForecastVsForecast";
            $.ajax({
                url: url,
                type: "post",
                data: $("#formForecastVsForecast").serializeObject(),
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                l.stop(); App.unblockUI('#mdlUpdateForecastForecast .modal-content');
                if (typeof data.Message != "undefined") {
                } else {
                    Common.Alert.Success("Data has been saved!")
                    $('#mdlUpdateForecastForecast').modal('hide');
                    trxARInputForecastVsForecast.Search();
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                if (textStatus == "error" && errorThrown == "") {
                    Common.Alert.Warning("Cannot upload file, please try to reselect file again, it's may cause the file has been updated at external resource.")
                    l.stop(); App.unblockUI('#mdlUpdateForecastForecast .modal-content');
                } else {
                    Common.Alert.Error(errorThrown)
                    l.stop(); App.unblockUI('#mdlUpdateForecastForecast .modal-content');
                }
            });
        },
        SubmitApprovalForm: function () {
            var l = Ladda.create(document.querySelector("#btnSendApprovalFormFcVsFc"))
            l.start();
            App.blockUI({
                target: "#gridPnltrxARInputForecastVsForecast", boxed: true
            });
            var isValidatePica = trxARInputForecastVsForecast.CheckPiCaValidation();
            if (isValidatePica.length > 0) {
                l.stop(); App.unblockUI('#gridPnltrxARInputForecastVsForecast');
                return false;
            }

            var url = "/api/ApiInputForecastCashIn/submitApprovalForecastVsForecast";
            $.ajax({
                url: url,
                type: "post",
                dataType: "json",
                data: {
                    ApprovalAction: $('#approvalActionForecastForecast').val(),
                    ApprovalRemarks: $('#approvalRemarksForecastForecast').val(),
                    ApprovalType: $('#approvalTypeForecastForecast').val(),
                    ProcessOID: $('#ProcessOIDFcVsFc').val()
                },
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                l.stop(); App.unblockUI('#gridPnltrxARInputForecastVsForecast ');
                if (typeof data.Message != "undefined") {
                } else {
                    Common.Alert.Success("Data has been saved!")
                    $('#btnSendApprovalFormFcVsFc').attr('disabled', true)
                    trxARInputForecastVsForecast.Search();
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                if (textStatus == "error" && errorThrown == "") {
                    Common.Alert.Warning("Cannot upload file, please try to reselect file again, it's may cause the file has been updated at external resource.")
                    l.stop(); App.unblockUI('#gridPnltrxARInputForecastVsForecast');
                } else {
                    Common.Alert.Error(errorThrown)
                    l.stop(); App.unblockUI('#gridPnltrxARInputForecastVsForecast ');
                }
            });
        },
        SendToApprovalClick: function () {
            var l = Ladda.create(document.querySelector("#btnSendApprovalFcVsFc"))
            l.start();
            App.blockUI({
                target: "#gridPnltrxARInputForecastVsForecast", boxed: true
            });
            var url = "/api/ApiInputForecastCashIn/sendApprovalRequestForecastVsForecast";
            $.ajax({
                url: url,
                type: "post",
                dataType: "json",

                cache: false
            }).done(function (data, textStatus, jqXHR) {
                l.stop(); App.unblockUI('#gridPnltrxARInputForecastVsForecast ');
                if (typeof data.Message != "undefined") {
                } else {
                    Common.Alert.Success("Data has been send to approval!")
                    $('#btnSendApprovalFcVsFc').attr('disabled', true)
                    trxARInputForecastVsForecast.Search();
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                if (textStatus == "error" && errorThrown == "") {
                    Common.Alert.Warning("Cannot upload file, please try to reselect file again, it's may cause the file has been updated at external resource.")
                    l.stop(); App.unblockUI('#gridPnltrxARInputForecastVsForecast');
                } else {
                    Common.Alert.Error(errorThrown)
                    l.stop(); App.unblockUI('#gridPnltrxARInputForecastVsForecast');
                }
            });
        },
        CheckPiCaValidation: function () {
            var url = "/api/ApiInputForecastCashIn/checkPiCaValidationForecastVsForecast";
            var res = '';
            $.ajax({
                url: url,
                type: "post",
                dataType: "json",
                async: false,
                cache: false,
                data: {
                    Year: settingVariable.yearOfInputPica,
                    Quarter: settingVariable.quarterOfInputPica,
                }
            }).done(function (data, textStatus, jqXHR) {
                if (data.length > 0) {
                    var message = "";
                    $.each(data, function (index, value) {
                        message += "\n" + value.OperatorID;
                    })
                    Common.Alert.Warning("Cannot Submit Forecast vs Actual, Please fill PiCa first at Operator : " + message);
                    res = message;
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                if (textStatus == "error" && errorThrown == "") {
                    Common.Alert.Warning("Error, Please Contact IT Helpdesk.")
                } else {
                    Common.Alert.Error(errorThrown)
                }
                res = "Error, Please Contact IT Helpdesk";
            });
            return res;
        },

        
}
//jQuery.fn.extend({
//    serializeObject: function () {
//        var formdata = $(this).serializeArray();
//        var data = {};
//        $(formdata).each(function (index, obj) {
//            data[obj.name] = obj.value;
//        });
//        return data;
//    }
//});