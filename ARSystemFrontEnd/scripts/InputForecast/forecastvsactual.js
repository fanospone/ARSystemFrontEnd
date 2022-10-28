jQuery(document).ready(function () {
    trxARInputForecastVsActual.Init();
    $('#btnSendApprovalFcVsAct').on('click', function (e) {
        e.preventDefault();
        trxARInputForecastVsActual.SendToApprovalClick();
    });
    $('#btnSendApprovalFormFcVsAct').on('click', function (e) {
        e.preventDefault();
        trxARInputForecastVsActual.SubmitApprovalForm();
    });
});

var trxARInputForecastVsActual = {
        Init: function () {
            var tblSummaryData = $('#tbltrxARInputForecastVsActual').dataTable({
                "filter": false,
                "destroy": true,
                "data": [],
                "ordering": false
            });

            $("#tbltrxARInputForecastVsActual tbody").on("click", ".btDetail", function (e) {
                var tblSummaryData = $("#tbltrxARInputForecastVsActual").DataTable();
                var tr = $(this).closest('tr');
                var row = tblSummaryData.row(tr);
                var dt = row.data();
                // you get All data row table in (dt)
            });
            $("#tbltrxARInputForecastVsActual tbody").unbind().on("click", ".lnkDetail", function (e) {
                let selector = $(this);
                let year = $(this).data('year');
                let quarter = $(this).data('quarter');
                let monthInQuarter = $(this).data('monthinquarter');
                let invoiceOperatorID = $(this).data('operatorid');
                
                $('#gridPnltrxARInputForecastVsActual').hide();
                $('#panelSummaryDetail').show();
                //Control.FormSummary.hide();
                //Control.FormDetail.show();
                trxARInputForecastVsActual.SummaryActualDetailRefresh(year, quarter, monthInQuarter, invoiceOperatorID);
                e.preventDefault();
            });
            $("#btnBack").click(function () {
                $('#gridPnltrxARInputForecastVsActual').show();
                $('#panelSummaryDetail').hide();
            });

            $(window).resize(function () {
                $("#tbltrxARInputForecastVsActual").DataTable().columns.adjust().draw();
            });

            // unremark if you call funtion
            //Table.trxARInputForecastVsActual.Search();
        },
        Search: function () {
            var l = Ladda.create(document.querySelector("#btSearch"))
            l.start();
            App.blockUI({
                target: "#gridPnltrxARInputForecastVsActual", boxed: true
            });
            
            params = {
                Year: $('#fyear').val(),
                Quarter: $('#fquarter').val(),
            };
            var currentQuarter = $('#fquarter').val();
            var monthsInQuarter = (currentQuarter == 1) ? ['JAN', 'FEB', 'MAR'] : (currentQuarter == 2) ? ['APR', 'MAY', 'JUN'] :
                    (currentQuarter == 3) ? ['JUL', 'AUG', 'SEP'] : (currentQuarter == 4) ? ['OCT', 'NOV', 'DEC'] : ['', '', ''];

            var tblSummaryData = $("#tbltrxARInputForecastVsActual").DataTable({
                "proccessing": true,
                "serverSide": true,
                "language": {
                    "emptyTable": "No data available in table",
                },
                "ajax": {
                    "url": "/api/ApiInputForecastCashIn/loadForecastVsActual",
                    "type": "POST",
                    "datatype": "json",
                    "data": params,
                },
       
                "filter": false,
                "lengthMenu": [[25, 50], ['25', '50']],
                "destroy": true,
                "columns": [
 
                { data: 'OperatorID' },
                {
                    data: 'FCMarketingM1', render: function (data, type, full) {
                        if (settingVariable.numberOfMonthInputForecast == 1 && settingVariable.yearOfInputForecast == full.Year && settingVariable.quarterOfInputForecast == full.Quarter && isFcActApproval == 'True')
                            return "<div class='popover-edit-forecast-grid' onClick='trxARInputForecastVsActual.UpdateForecastClick(\"" + full.OperatorID + "\", \"" + full.Year + "\", \"" + full.Quarter + "\", \"1\", \"ForecastMarketing\")'>" + (data != null ? Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data)) : '') + "</div> ";
                        else
                            return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                    }
                }, {    
                    data: 'FCRevenueAssuranceM1', render: function (data) {
                        return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                    }
                }, {
                    data: 'FCFinanceM1', render: function (data, type, full) {
                        if (settingVariable.numberOfMonthInputForecast == 1 && settingVariable.yearOfInputForecast == full.Year && settingVariable.quarterOfInputForecast == full.Quarter && isFcActApproval == 'True')
                                return "<div class='popover-edit-forecast-grid' onClick='trxARInputForecastVsActual.UpdateForecastClick(\"" + full.OperatorID + "\", \"" + full.Year + "\", \"" + full.Quarter + "\", \"1\", \"ForecastFinance\")'>" + (data != null ? Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data)) : '') + "</div> ";
                        else
                            return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                    }
                }, {
                    data: 'ActualM1', render: function (data, type, row, meta) {
                        return '<a class="lnkDetail" data-operatorid="' + row.OperatorID + '" data-year="' + row.Year + '" data-monthInQuarter="1" data-quarter="' + row.Quarter + '">' + Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data)) + '</a>';
                    }
                }, {
                    data: 'VarianceM1', render: function (data) {
                        return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                    }
                }, {
                    data: 'PiCaM1', render: function (data, type, full) {
                        if (settingVariable.numberOfMonthInputForecast == 1 && settingVariable.yearOfInputForecast == full.Year && settingVariable.quarterOfInputForecast == full.Quarter && isFcActApproval == 'True') {
                            var textpica = (data != null) ? data.substr(0, 15) + (data.length  > 15 ? '...' : '') : '';
                            return "<div class='popover-edit-forecast-grid' onClick='trxARInputForecastVsActual.UpdateForecastClick(\"" + full.OperatorID + "\", \"" + full.Year + "\", \"" + full.Quarter + "\", \"1\", \"PiCa\")'> " + (data != null ? " <span class='picasText'>" + textpica + "</span> " : "") + " </div> ";
                        }
                       else
                            return (data != null ? "<div onClick='trxARInputForecastVsActual.UpdateForecastClick(\"" + full.OperatorID + "\", \"" + full.Year + "\", \"" + full.Quarter + "\", \"1\", \"PiCa\", \"ViewPica\")'> <span class='picasText'>View..</span> </div>" : '');

                    }
                },
             {
                 data: 'FCMarketingM2', render: function (data, type, full) {
                     if (settingVariable.numberOfMonthInputForecast == 2 && settingVariable.yearOfInputForecast == full.Year && settingVariable.quarterOfInputForecast == full.Quarter && isFcActApproval == 'True')
                         return "<div class='popover-edit-forecast-grid' onClick='trxARInputForecastVsActual.UpdateForecastClick(\"" + full.OperatorID + "\", \"" + full.Year + "\", \"" + full.Quarter + "\", \"2\", \"ForecastMarketing\")'>" + (data != null ? Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data)) : '') + "</div> ";
                     else
                         return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));

                 }
             }, {
                 data: 'FCRevenueAssuranceM2', render: function (data) {
                     return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                 }
             }, { 
                 data: 'FCFinanceM2', render:
                     function (data, type, full) {
                         if (settingVariable.numberOfMonthInputForecast == 2 && settingVariable.yearOfInputForecast == full.Year && settingVariable.quarterOfInputForecast == full.Quarter && isFcActApproval == 'True')
                             return "<div class='popover-edit-forecast-grid' onClick='trxARInputForecastVsActual.UpdateForecastClick(\"" + full.OperatorID + "\", \"" + full.Year + "\", \"" + full.Quarter + "\", \"2\", \"ForecastFinance\")'>" + (data != null ? Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data)) : '') + "</div> ";
                         else
                             return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                     }
             }, {
                 data: 'ActualM2', render: function (data, type, row, meta) {
                     return '<a class="lnkDetail" data-operatorid="' + row.OperatorID + '" data-year="' + row.Year + '" data-monthInQuarter="2" data-quarter="' + row.Quarter + '">' + Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data)) + '</a>';
                 }
             }, { 
                 data: 'VarianceM2', render: function (data) {
                     return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                 }
             }, {
                 data: 'PiCaM2', render: function (data, type, full) {
   
                     if (settingVariable.numberOfMonthInputPica == 2 && settingVariable.yearOfInputPica == full.Year && settingVariable.quarterOfInputPica == full.Quarter && isFcActApproval == 'True')
                     {
                         var textpica = (data != null) ? data.substr(0, 15) + (data.length  > 15 ? '...' : '') : '';
                         return "<div class='popover-edit-forecast-grid' onClick='trxARInputForecastVsActual.UpdateForecastClick(\"" + full.OperatorID + "\", \"" + full.Year + "\", \"" + full.Quarter + "\", \"2\", \"PiCa\")'> " + (data != null ? " <span class='picasText'>"+textpica+"</span> " : "") + " </div> ";
                     } else
                         return (data != null ? "<div onClick='trxARInputForecastVsActual.UpdateForecastClick(\"" + full.OperatorID + "\", \"" + full.Year + "\", \"" + full.Quarter + "\", \"2\", \"PiCa\", \"ViewPica\")'> <span class='picasText'>View..</span> </div>" : '');

                 }
             },
{
    data: 'FCMarketingM3', render: function (data, type, full) {
        if (settingVariable.numberOfMonthInputForecast == 3 && settingVariable.yearOfInputForecast == full.Year && settingVariable.quarterOfInputForecast == full.Quarter && isFcActApproval == 'True')
            return "<div class='popover-edit-forecast-grid' onClick='trxARInputForecastVsActual.UpdateForecastClick(\"" + full.OperatorID + "\", \"" + full.Year + "\", \"" + full.Quarter + "\", \"3\", \"ForecastMarketing\")'>" + (data != null ? Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data)) : '') + "</div> ";
        else
            return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
    }
                },
                  {
                    data: 'FCRevenueAssuranceM3', render: function (data) {
                        return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                    }
                },
                {
                    data: 'FCFinanceM3', render: function (data, type, full) {
                        if (settingVariable.numberOfMonthInputForecast == 3 && settingVariable.yearOfInputForecast == full.Year && settingVariable.quarterOfInputForecast == full.Quarter && isFcActApproval == 'True')
                            return "<div class='popover-edit-forecast-grid' onClick='trxARInputForecastVsActual.UpdateForecastClick(\"" + full.OperatorID + "\", \"" + full.Year + "\", \"" + full.Quarter + "\", \"3\", \"ForecastFinance\")'>" + (data != null ? Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data)) : '') + "</div> ";
                        else 
                            return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                    }
                },
                {
                    data: 'ActualM3', render: function (data, type, row, meta) {
                        return '<a class="lnkDetail" data-operatorid="' + row.OperatorID + '" data-year="' + row.Year + '" data-monthInQuarter="3" data-quarter="' + row.Quarter + '">' + Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data)) + '</a>';
                    }
                },{
                    data: 'VarianceM3', render: function (data) {
                        return Common.Format.CommaSeparation((parseInt(data) ? data / 1000000 : data));
                    }
                },
              {
                  data: 'PiCaM3', render: function (data, type, full) {
                    //  if (settingVariable.numberOfMonthInputPica == 3) return "<div class='popover-edit-pica-grid' data-toggle='popover'  data-operator='" + full.OperatorID + "' data-month='" + full.month + "'>" + (data != null ? data : '') + "</div>";
                      if (settingVariable.numberOfMonthInputPica == 3 && settingVariable.yearOfInputPica == full.Year && settingVariable.quarterOfInputPica == full.Quarter && isFcActApproval == 'True'){
                          var textpica = (data != null) ? data.substr(0, 15) + (data.length  > 15 ? '...' : '') : '';
                          return "<div class='popover-edit-forecast-grid' onClick='trxARInputForecastVsActual.UpdateForecastClick(\"" + full.OperatorID + "\", \"" + full.Year + "\", \"" + full.Quarter + "\", \"3\", \"PiCa\")'> " + (data != null ? " <span class='picasText'> " + textpica  + " </span> " : "") + " </div> ";
                      }
                      else 
                          return (data != null ? "<div onClick='trxARInputForecastVsActual.UpdateForecastClick(\"" + full.OperatorID + "\", \"" + full.Year + "\", \"" + full.Quarter + "\", \"3\", \"PiCa\", \"ViewPica\")'> <span class='picasText'>View..</span> </div>" : '');

                  }
              },

                ],
                "columnDefs": [

               { 'targets': [1], 'width': '100%', 'className': 'dt-left', 'visible': true },
               { 'targets': [2], 'width': '100%', 'className': 'dt-left', 'visible': true },
               { 'targets': [3], 'width': '100%', 'className': 'dt-left', 'visible': true },
               { 'targets': [4], 'width': '100%', 'className': 'dt-left', 'visible': true },
               { 'targets': [5], 'width': '100%', 'className': 'dt-left', 'visible': true },

                ],
                //"dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                "fnPreDrawCallback": function () {
                    App.blockUI({ target: ".gridPnltrxARInputForecastVsActual", boxed: true });
                },
                "fnDrawCallback": function () {
                    if (Common.CheckError.List(tblSummaryData.data())) {
                        $(".panelSearchBegin").hide();
                        $(".panelSearchResult").show();
                    }
                    $("#gridPnltrxARInputForecastVsActual .sGridQuarter").html("Q" + currentQuarter);
                    $("#gridPnltrxARInputForecastVsActual .sGridMonth1").html(monthsInQuarter[0]);
                    $("#gridPnltrxARInputForecastVsActual .sGridMonth2").html(monthsInQuarter[1]);
                    $("#gridPnltrxARInputForecastVsActual .sGridMonth3").html(monthsInQuarter[2]);
                    $('table#tbltrxARInputForecastVsActual').find("tr:nth-child(2)").find("th:nth-child(1)").removeClass('bg-color-blue')
                    $('table#tbltrxARInputForecastVsActual').find("tr:nth-child(2)").find("th:nth-child(2)").removeClass('bg-color-blue')
                    $('table#tbltrxARInputForecastVsActual').find("tr:nth-child(2)").find("th:nth-child(3)").removeClass('bg-color-blue')
                    if (settingVariable.yearOfInputPica == params.Year && settingVariable.quarterOfInputPica == params.Quarter) {
                        if (settingVariable.numberOfMonthInputPica == 1) {
                            $('table#tbltrxARInputForecastVsActual').find("tr:nth-child(2)").find("th:nth-child(1)").addClass('bg-color-blue')
                        }
                        if (settingVariable.numberOfMonthInputPica == 2) {
                            $('table#tbltrxARInputForecastVsActual').find("tr:nth-child(2)").find("th:nth-child(2)").addClass('bg-color-blue')
                        }
                        if (settingVariable.numberOfMonthInputPica == 3) {
                            $('table#tbltrxARInputForecastVsActual').find("tr:nth-child(2)").find("th:nth-child(3)").addClass('bg-color-blue')
                        }
                    }


                    $(function () {
                        $('.forecast-finance, .forecast-marketing').on('change', function () {
                            var res = Common.Format.CommaSeparation($(this).val());
                            $(this).val(res);
                        });

                        
                    })

                    l.stop();
                    App.unblockUI('#gridPnltrxARInputForecastVsActual');
                },
                "createdRow": function (row, data, dataIndex) {
                    if (data.VarianceM1 < 0) {
                        $(row).find('td:nth-child(6)').attr('style', 'background-color : #ea6157 !important; color: #fff;')
                        }
                    if (data.VarianceM2 < 0) {
                        $(row).find('td:nth-child(12)').attr('style', 'background-color : #ea6157 !important; color: #fff;')
                        }
                    if (data.VarianceM3 < 0) {
                        $(row).find('td:nth-child(18)').attr('style', 'background-color : #ea6157 !important; color: #fff;')
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
            trxARInputForecastVsActual.Init();
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
        submitUpdatePica: function(ele){
            var textarea = $(ele).parent().find('textarea').val();
            var operator = $(ele).parent().find('.pica-operator').val();
            alert(textarea);
            var l = Ladda.create(document.querySelector(".btnSavePica"))
            l.start();
            App.blockUI({
                target: "#gridPnltrxARInputForecastVsActual", boxed: true
            });
            var url = "/api/ApiInputForecastCashIn/submitForecastVsActual";
            $.ajax({
                url: url,
                type: "post",
                dataType: "json",
                data: {
                    PiCa: textarea,
                    OperatorID: operator,
                    UpdateType: 'Update Pica'
                },
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                l.stop(); App.unblockUI('gridPnltrxARInputForecastVsActual');
                if (typeof data.Message != "undefined") {
                } else {
                    Common.Alert.Success("Data has been saved!")
                    $('.popover-edit-pica-grid').popover('hide')
                    trxARInputForecastVsActual.Search();
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                if (textStatus == "error" && errorThrown == "") {
                    Common.Alert.Warning("Error.")
                    l.stop(); App.unblockUI('gridPnltrxARInputForecastVsActual');
                } else {
                    Common.Alert.Error(errorThrown)
                    l.stop(); App.unblockUI('#gridPnltrxARInputForecastVsActual');
                }
            });
        },
        submitUpdateForecast: function (ele) {
            alert('nah!')
            var fCFinance = $(ele).parent().find('input.forecast-finance').val();
            var fCMarketing = $(ele).parent().find('input.forecast-marketring').val();
            var operator = $(ele).parent().find('.pica-operator').val();
            alert(textarea);
            var l = Ladda.create(document.querySelector(".btnSavePica"))
            l.start();
            App.blockUI({
                target: "#gridPnltrxARInputForecastVsActual", boxed: true
            });
            var url = "/api/ApiInputForecastCashIn/submitForecastVsActual";
            $.ajax({
                url: url,
                type: "post",
                dataType: "json",
                data: {
                    FCFinance: fCFinance,
                    FCMarketing:fCMarketing ,
                    OperatorID: operator,
                    UpdateType: 'Update Forecast'
                },
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                l.stop(); App.unblockUI('gridPnltrxARInputForecastVsActual');
                if (typeof data.Message != "undefined") {
                } else {
                    Common.Alert.Success("Data has been saved!")
                    $('.popover-edit-pica-grid').popover('hide')
                    trxARInputForecastVsActual.Search();
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                if (textStatus == "error" && errorThrown == "") {
                    Common.Alert.Warning("Error.")
                    l.stop(); App.unblockUI('gridPnltrxARInputForecastVsActual');
                } else {
                    Common.Alert.Error(errorThrown)
                    l.stop(); App.unblockUI('#gridPnltrxARInputForecastVsActual');
                }
            });
        },

        UpdateForecastClick: function (OperatorID, Year, Quarter, Month, type, callfrom) {
                App.blockUI({
                    target: "#gridPnltrxARInputForecastVsActual", boxed: true
                });
                var url = "/InputForecastCashIn/UpdateForecastVsActual";
                $.ajax({
                    url: url,
                    type: "post",
                    data: { OperatorID:OperatorID, Year:Year, Quarter:Quarter,Month:Month,InputType:type },
                    cache: false,
                }).done(function (data, textStatus, jqXHR) {
                    $("#mdlUpdateForecastContainer").html(data);
                    $('#mdlUpdateForecastActual').modal('show');
                    if (callfrom == 'ViewPica') {
                        $('btSubmitFormForecastActual .form-control').attr('readonly', true)

                        $('#btSubmitFormForecastActual').css('display','none');
                    } else {
                        $('#btSubmitFormForecastActual').on('click', function (e) {
                            e.preventDefault();
                            trxARInputForecastVsActual.SubmitModal();
                        });
                    }
              
                    $('.btCancelFormForecastActual').on('click', function (e) {
                        e.preventDefault();
                        $('#mdlUpdateForecastActual').modal('hide');
                    });

                    App.unblockUI('#gridPnltrxARInputForecastVsActual');
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    Common.Alert.Error(errorThrown) 
                    App.unblockUI('#gridPnltrxARInputForecastVsActual');
                })
        },
        SubmitModal: function () {
            var varianceValue = 0;
            varianceValue = ($('#VarianceM1').length > 0) ? parseInt($('#VarianceM1').val().replace(/,/g, "")) :
                ($('#VarianceM2').length > 0) ? parseInt($('#VarianceM2').val().replace(/,/g, "")) :
                ($('#VarianceM3').length > 0) ? parseInt($('#VarianceM3').val().replace(/,/g, "")) : 0;
            if (varianceValue < 0) {
                var piCa = '';
                if ($('#PiCaM1').length > 0) {
                    if (varianceValue < 0)
                        $("#PiCaM1").attr('data-parsley-required', true);
                    else
                        $("#PiCaM1").attr('data-parsley-required', false);
                }
                if ($('#PiCaM2').length > 0) {
                    if (varianceValue < 0)
                        $("#PiCaM2").attr('data-parsley-required', true);
                    else
                        $("#PiCaM2").attr('data-parsley-required', false);
                }
                if ($('#PiCaM3').length > 0) {
                    if (varianceValue < 0)
                        $("#PiCaM3").attr('data-parsley-required', true);
                    else
                        $("#PiCaM3").attr('data-parsley-required', false);
                }
            }
            
            if (!($("#formForecastVsActual").parsley().validate())) {
                return false;
            } else {
                var l = Ladda.create(document.querySelector("#btSubmitFormForecastActual"))
                l.start();
                App.blockUI({
                    target: "#mdlUpdateForecastActual .modal-content", boxed: true
                });
                var url = "/api/ApiInputForecastCashIn/submitForecastVsActual";
                $.ajax({
                    url: url,
                    type: "post",
                    dataType: "json",
                    data: $("#formForecastVsActual").serializeObject(),
                    cache: false
                }).done(function (data, textStatus, jqXHR) {
                    l.stop(); App.unblockUI('#mdlUpdateForecastActual .modal-content');
                    if (typeof data.Message != "undefined") {
                    } else {
                        Common.Alert.Success("Data has been saved!")
                        $('#mdlUpdateForecastActual').modal('hide');
                        trxARInputForecastVsActual.Search();
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    if (textStatus == "error" && errorThrown == "") {
                        Common.Alert.Warning("Cannot upload file, please try to reselect file again, it's may cause the file has been updated at external resource.")
                        l.stop(); App.unblockUI('#mdlUpdateForecastActual .modal-content');
                    } else {
                        Common.Alert.Error(errorThrown)
                        l.stop(); App.unblockUI('#mdlUpdateForecastActual .modal-content');
                    }
                });
            }
        },
        SubmitApprovalForm: function () {
            var l = Ladda.create(document.querySelector("#btnSendApprovalFormFcVsAct"))
            l.start();
            App.blockUI({
                target: "#gridPnltrxARInputForecastVsActual", boxed: true
            });
            var isValidatePica = trxARInputForecastVsActual.CheckPiCaValidation();
            if (isValidatePica.length > 0) {
                l.stop(); App.unblockUI('#gridPnltrxARInputForecastVsActual');
                return false;
            }
            var url = "/api/ApiInputForecastCashIn/submitApprovalForecastVsActual";
            $.ajax({
                url: url,
                type: "post",
                dataType: "json",
                data: {
                    ApprovalAction: $('#approvalActionForecastActual').val(),
                    ApprovalRemarks: $('#approvalRemarksForecastActual').val(),
                    ApprovalType: $('#approvalTypeForecastActual').val(),
                    ProcessOID: $('#ProcessOID').val()
                },
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                l.stop(); App.unblockUI('#gridPnltrxARInputForecastVsActual');
                if (typeof data.Message != "undefined") {
                } else {
                    Common.Alert.Success("Data has been saved!")
                    $('#btnSendApprovalFormFcVsAct').attr('disabled', true)
                    isFcActApproval = 'Done';
                    trxARInputForecastVsActual.Search();
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                if (textStatus == "error" && errorThrown == "") {
                    Common.Alert.Warning("Cannot upload file, please try to reselect file again, it's may cause the file has been updated at external resource.")
                    l.stop(); App.unblockUI('#gridPnltrxARInputForecastVsActual');
                } else {
                    Common.Alert.Error(errorThrown)
                    l.stop(); App.unblockUI('#gridPnltrxARInputForecastVsActual');
                }
            });
        },
        SendToApprovalClick: function () {
            var l = Ladda.create(document.querySelector("#btnSendApprovalFcVsAct"))
            l.start();
            App.blockUI({
                target: "#gridPnltrxARInputForecastVsActual", boxed: true
            });
            var url = "/api/ApiInputForecastCashIn/sendApprovalRequestForecastVsActual";
            $.ajax({
                url: url,
                type: "post",
                dataType: "json",
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                l.stop(); App.unblockUI('#gridPnltrxARInputForecastVsActual .modal-content');
                if (typeof data.Message != "undefined") {
                } else {
                    Common.Alert.Success("Data has been send to approval!")
                    trxARInputForecastVsActual.Search();
                    $('#btnSendApprovalFcVsAct').attr('disabled', true)
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                if (textStatus == "error" && errorThrown == "") {
                    Common.Alert.Warning("Cannot upload file, please try to reselect file again, it's may cause the file has been updated at external resource.")
                    l.stop(); App.unblockUI('#gridPnltrxARInputForecastVsActual .modal-content');
                } else {
                    Common.Alert.Error(errorThrown)
                    l.stop(); App.unblockUI('#gridPnltrxARInputForecastVsActual .modal-content');
                }
            });
        },
        SummaryActualDetailRefresh: function (year, quarter, monthInQuarter, invoiceOperatorID) {
            var tbl = $("#tblSummaryDetail").DataTable({
                "orderCellsTop": true,
                "processing": true,
                "serverSide": true,
                "language": {
                    "emptyTable": "No data available in table",
                },
                "ajax": {
                    "url": "/api/ApiInputForecastCashIn/loadSummaryActualForecastVsActual",
                    "type": "POST",
                    "datatype": "json",
                    "data": {
                        
                            Year: year,
                            Quarter: quarter,
                            MonthWithinQuarter: monthInQuarter,
                        OperatorID: invoiceOperatorID
                   
                    },
                },
 
                "filter": false,
                "order": [[0, 'asc']],
                "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
                "destroy": true,
                "columns": [
                    { data: "InvoiceNo" },
                    {
                        data: "InvoicePrintDate",
                        render: function (data, type, row, meta) {
                            return Common.Format.ConvertJSONDateTime(data);
                        }
                    },
                    { data: "InvoiceOperatorID" },
                    { data: "InvoiceCompanyInvoice" },
                    {
                        data: "InvoiceTotalAmount",
                        render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    },
                    { data: "InvoiceSubject" }
                ],
                "columnDefs": [
                ],
                "fnDrawCallback": function () {
                    Common.CheckError.List(tbl.data());
                },
            });
        },
        CheckPiCaValidation: function () {
            var month = (parseInt(settingVariable.quarterOfInputPica) * 3 - 3) + parseInt(settingVariable.numberOfMonthInputPica)
            var url = "/api/ApiInputForecastCashIn/checkPiCaValidation";
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
                    Month: month,
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
                res =  "Error, Please Contact IT Helpdesk";
            });
            return res;
        },
}

jQuery.fn.extend({
    serializeObject: function () {
        var formdata = $(this).serializeArray();
        var data = {};
        $(formdata).each(function (index, obj) {
            if (obj.name == 'ActualM1' || obj.name == 'ActualM2' || obj.name == 'ActualM3' || 
                obj.name == 'FCFinanceM1' || obj.name == 'FCFinanceM2' || obj.name == 'FCFinanceM3' || 
                obj.name == 'FCRevenueAssuranceM1' || obj.name == 'FCRevenueAssuranceM2' || obj.name == 'FCRevenueAssuranceM3' || 
                obj.name == 'FCMarketingM1' || obj.name == 'FCMarketingM2' || obj.name == 'FCMarketingM3' ||
                obj.name == 'VarianceM1' || obj.name == 'VarianceM2' || obj.name == 'VarianceM3') {
                data[obj.name] = obj.value.replace(/,/g, "");
            } else {
                data[obj.name] = obj.value;
            }
        });
        return data;
    }
});