Data = {};
Edit = {};
Data.RowSelectedRaw = [];
RowSelected = {};
TempData = [];
AllDataId = [];
CheckAllDataFlag = false;
TypeFlagAll = false;
Flag = "";
TotalAmount = 0;
PeriodeSearch = "";
WeekSearch = "";
FlagConfirm = "";
fsdate = null;
fsweek = null;

jQuery(document).ready(function () {
    Form.Init();
    
    $("#btReset").unbind().click(function () {
        Table.Reset();
    });
    $("#btCancel").unbind().click(function () {
        $(".panelSearchResult").hide();
        $(".panelSearchZero").hide();
        $(".panelSiteData").hide();
    });
    $("#btCancelAdd").unbind().click(function () {
        Form.CancelAdd();
    });

    $("#btReConfirm").unbind().click(function () {
        FlagConfirm = "ReConfirm";
        $("#lblDetailAdd").html('Re Confirm');
        $('#mdlConfirm').modal('show');
        //Action.ReConfirm();
    });

    $("#btExportExcel").unbind().click(function () {
        if ($("#tabAccrue").tabs('option', 'active') == 0)
            Table.ExportConfirmedUser();
        else
            Table.Export();
    });
    $("#btFinalConfirm").unbind().click(function () {
        FlagConfirm = "Final";
        $("#lblDetailAdd").html('Final Confirm');
        $('#mdlConfirm').modal('show');
        
    });
    $("#btYesConfirm").unbind().click(function () {
        if(FlagConfirm == "Final")
            Action.FinalConfirm();
        else
            Action.ReConfirm();
    });
    $("#btSearch").unbind().click(function () {
        TempData = [];
        AllDataId = [];
        CheckAllDataFlag = false;
        
        if ($('#tbSearchPeriode').val() == "" || $('#slSearchPeriodeWeek').val() == null || $('#slSearchPeriodeWeek').val() == "")
            Common.Alert.Warning("Please Select Periode And Week to Show Data")
        else
        {
            //Helper.CheckIsReConfirm();
            //Table.Search();
            if ($("#tabAccrue").tabs('option', 'active') == 0)
                Table.ConfirmedUserSearch();
            else
                Table.Search();
        }
            
    });
    $('#tabAccrue').unbind().on('tabsactivate', function (event, ui) {
        var newIndex = ui.newTab.index();
        if (newIndex == 0) {            
            Table.ConfirmedUserSearch();
            $("#btReConfirm").show();
            $("#btFinalConfirm").show();

        }        
        else {
            Table.Search();
            $("#btReConfirm").hide();
            $("#btFinalConfirm").hide();
        }
    });

    
    $("#tbSearchPeriode").change(function () {
        if ($("#tbSearchPeriode").val() != "")
            Control.BindingSelectWeek();
    });

    

});
var Form = {
    Init: function () {
        $(".panelSearchResult").hide();
        $(".panelSearchZero").hide();
        $(".panelSiteData").hide();
        if (!$("#hdAllowProcess").val()) {
            $("#btReConfirm").hide();
            $("#btFinalConfirm").hide();            
        }
        $('#tabAccrue').tabs();
        Control.BindingSelectMonthGetDate();
        Control.BindingSelectWeekSearchGetDate();
        $("#slSearchPeriodeWeek").select2({ placeholder: "Week", width: null });
        $('#cbALLData input[type=checkbox]').prop('checked', false);
        $("body").delegate(".datepicker", "focusin", function () {
            $(this).datepicker({
                format: "M-yyyy",
                viewMode: "months",
                minViewMode: "months"
            });
        });
        Helper.GetWeekNowSelected();
        Helper.CheckIsReConfirm();
        //Table.Search();
        Table.ConfirmedUserSearch();
    },

    CancelAdd: function () {
        $(".panelSiteData").hide();
        $("#divRemarks").hide();
    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $('#mdlDetail').modal('toggle');
        //Table.Search();

    }
}
var Table = {
    Init: function (idTable) {

        var tblInit = $(idTable).dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $(idTable).DataTable().columns.adjust().draw();
        });

    },
    Search: function () {
        //Table.Init("#tblSummaryData");
        TempData = [];
        AllDataId = [];
        CheckAllDataFlag = false;
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        param = {};
        fsdate = $('#tbSearchPeriode').val();
        fsweek = $('#slSearchPeriodeWeek').val();
        var params = {

            date: fsdate,
            week: fsweek
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/FinalConfirmation/grid",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
                //{ extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                //{
                //    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                //        var l = Ladda.create(document.querySelector(".yellow"));
                //        l.start();
                //        Table.Export()
                //        l.stop();
                //    }
                //},
                //{ extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "bLengthChange": false,
            "destroy": true,
            "columns": [
                { data: "SOW" },
                { data: "NEWCURRNoOfSite", className: "text-right" },
                { data: "NEWCURRTotalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "NEWODNoOfSite", className: "text-right" },
                { data: "NEWODTotalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "RECCURRNoOfSite", className: "text-right" },
                { data: "RECCURRTotalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "RECODNoOfSite", className: "text-right" },
                { data: "RECODTotalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "TotalNoOfSite", className: "text-right" },
                { data: "TotalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Percen", className: "text-right" }
            ],
            "columnDefs": [{}],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }

                l.stop();
            },
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api();
                nb_cols = api.columns().nodes().length;
                var j = 1;

                while (j < nb_cols) {
                    var pageTotal = api
                .column(j, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return Number(a) + Number(b);
                }, 0);
                    // Update footer
                    if (j == 2 || j == 4 || j == 6 || j == 8 || j == 10 || j == 11 )
                        $(api.column(j).footer()).html("<label>" + Control.numberWithCommas(pageTotal.toFixed(2)) + "</label>");
                    else
                        $(api.column(j).footer()).html("<label>" + pageTotal.toString() + "</label>");
                    j++;
                }
            },
            "order": [],
            //"scrollY": 300, /* Enable vertical scroll to allow fixed columns */
            //"scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,


        });


    },
    ConfirmedUserSearch: function () {
        fsdate = $('#tbSearchPeriode').val();
        fsweek = $('#slSearchPeriodeWeek').val();
        var params = {
            date: fsdate,
            week: fsweek
        };
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        $.ajax({
            url: "/api/FinalConfirmation/gridUserConfirmed",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            var a = data;
            //$('#tblSummaryDataUser').dataTable().fnDestroy();;
            $(".panelSearchBegin").hide();
            $(".panelSearchResult").fadeIn();
            $("#tblSummaryDataUser thead").empty();
            $("#tblSummaryDataUser tbody").empty();
            $("#tblSummaryDataUser thead").append(data.data.thead);
            $("#tblSummaryDataUser tbody").append(data.data.tbody);
            if (!$("#hdAllowProcess").val()) {
                $("#btReConfirm").hide();
                $("#btFinalConfirm").hide();
            }
            else {
                $("#btReConfirm").show();
                $("#btFinalConfirm").show();
            }
            //$("#tblSummaryDataUser").dataTable();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
        l.stop();
    },
    Export: function () {
        var date = fsdate;
        var week = fsweek;

        window.location.href = "/Accrue/FinalConfirmSummary/Export?Date=" + date + "&Week=" + week;
    },
    ExportConfirmedUser: function () {
        var date = fsdate;
        var week = fsweek;

        window.location.href = "/Accrue/FinalConfirmedUserSummary/Export?Date=" + date + "&Week=" + week;
    },
    Reset: function () {
        $("#slSearchCustomer").val("").trigger('change');
        $("#slSearchCompany").val("").trigger('change');
        $("#slSearchRegional").val("").trigger('change');
        $("#slSearchStatus").val("").trigger('change');
        $("#slDepartment").val("").trigger('change');
        $("#slSearchPeriodeWeek").val("").trigger('change');
        $("#tbSearchPeriode").val("");
        $("#tbSearchSONumber").val("");
        $("#tbRemarks").val("");
        $("#tbSearchSiteName").val("");
    },

}

var Action = {
    FinalConfirm: function () {

        var params = {

            date: fsdate,
            week: fsweek
        };
        var l = Ladda.create(document.querySelector("#btSearch"))
        var l2 = Ladda.create(document.querySelector("#btFinalConfirm"))
        var l3 = Ladda.create(document.querySelector("#btYesConfirm"))
        $.ajax({
            url: "/api/FinalConfirmation/FinalConfirm",
            type: "post",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l3.start();
                l2.start();
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                if (data.ErrorType != 0) {
                    l2.stop();
                    l.stop();
                    $('#mdlConfirm').modal('toggle');
                    Common.Alert.Warning(data.ErrorMessage);
                }
                    
                else {
                    Common.Alert.Success("Data has been saved!")
                    l2.stop();
                    l.stop();
                    $('#mdlConfirm').modal('toggle');
                    $(".panelSearchResult").hide();
                    //Table.Search();
                }
                
            }
            else {
                Common.Alert.Error(data.ErrorMessage);
            }
            l3.stop()
            l2.stop();
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l3.stop()
            l2.stop();
            l.stop();
        })

    },

    ReConfirm: function () {
        var params = {
            date: fsdate,
            week: fsweek
        };
        var l = Ladda.create(document.querySelector("#btSearch"))
        var l2 = Ladda.create(document.querySelector("#btReConfirm"))
        var l3 = Ladda.create(document.querySelector("#btYesConfirm"))
        $.ajax({
            url: "/api/FinalConfirmation/ReConfirm",
            type: "post",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l3.start();
                l2.start();
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                if (data.ErrorType != 0){
                    $('#mdlConfirm').modal('toggle');
                    Common.Alert.Warning(data.ErrorMessage);
                }                    
                else {
                    Common.Alert.Success("Data has been saved!")
                    l3.stop()
                    l2.stop();
                    l.stop();
                    $('#mdlConfirm').modal('toggle');
                    $(".panelSearchResult").hide();
                    //Table.Search();
                }
                
            }
            else {
                Common.Alert.Error(data.ErrorMessage);
            }

            l3.stop()
            l2.stop();
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l3.stop()
            l2.stop();
            l.stop();
        })

    },
}
var Control = {
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/ListDataAccrue/company/list",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchCompany").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchCompany").append("<option value='" + item.CompanyID + "'>" + item.Company + "</option>");
                })
            }

            $("#slSearchCompany").select2({ placeholder: "Select Company", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectCustomer: function () {
        var params = {
            bolIsTelco: null
        };
        $.ajax({
            url: "/api/ListDataAccrue/customer/list",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchCustomer").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchCustomer").append("<option value='" + item.CustomerID + "'>" + item.CustomerName + "</option>");
                })
            }

            $("#slSearchCustomer").select2({ placeholder: "Select Customer", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectStatus: function () {

        $.ajax({
            url: "/api/ListDataAccrue/statusAccrue/list",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchStatus").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchStatus").append("<option value='" + item.ID + "'>" + item.AccrueStatus + "</option>");
                })
            }

            $("#slSearchStatus").select2({ placeholder: "Select Status", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectDepartment: function () {
        var params = {
            typeSOW: TempData[0].Type
        };
        $.ajax({
            url: "/api/ListDataAccrue/departmentAccrue/list",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slDepartment").html("<option></option>")

            if (Common.CheckError.List(data)) {

                $.each(data, function (i, item) {
                    $("#slDepartment").append("<option value='" + item.SOW + "'>" + item.SOW + "</option>");
                })


            }

            $("#slDepartment").select2({ placeholder: "Select Department", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectWeek: function () {
        var params = {
            date: $('#tbSearchPeriode').val()
        };
        $.ajax({
            url: "/api/ListDataAccrue/weekOfMonth/list",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchPeriodeWeek").html("<option></option>")

            if (Common.CheckError.List(data)) {

                $.each(data, function (i, item) {
                    $("#slSearchPeriodeWeek").append("<option value='" + item.ID + "'>" + item.Week + "</option>");
                })

            }

            $("#slSearchPeriodeWeek").select2({ placeholder: "Week", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectWeekSearchGetDate: function () {

        $.ajax({
            url: "/api/ListDataAccrue/weekOfMonth/listGetDate",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchPeriodeWeek").html("<option></option>")

            if (Common.CheckError.List(data)) {

                $.each(data, function (i, item) {
                    $("#slSearchPeriodeWeek").append("<option value='" + item.ID + "'>" + item.Week + "</option>");
                })

            }

            $("#slSearchPeriodeWeek").select2({ placeholder: "Week", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectMonthGetDate: function () {
        $.ajax({
            url: "/api/ListDataAccrue/Month/SetMonthGetDate",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $('#tbSearchPeriode').val(data);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectRegion: function () {
        $.ajax({
            url: "/api/ListDataAccrue/regional/list",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchRegional").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchRegional").append("<option value='" + item.RegionID + "'>" + item.RegionName + "</option>");
                })
            }

            $("#slSearchRegional").select2({ placeholder: "Select Regional", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    Calculate: function (input) {
        //var result = 0;
        var params = {
            vwtrxDataAccrue: input
        };
        $.ajax({
            url: "/api/FinanceConfirmation/Calculate",
            type: "POST",
            async: false,
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            Edit.Selected.TotalAmount = data;
            //return result;
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    DeleteRequestDetail: function (rowID) {
        $("#cb_" + rowID + " input[type=checkbox]").prop('checked', false);
        var index = TempData.findIndex(function (data) {
            return data.trxDataAccrueID == rowID
        });
        TempData.splice(index, 1);
        if (TempData.length == 0)
            $(".panelSiteData").hide();
    },
    DeleteRequestDetailAll: function (rowID) {
        $("#cb_" + rowID + " input[type=checkbox]").prop('checked', false);
        var index = AllDataId.findIndex(function (data) {
            return data == rowID
        });
        AllDataId.splice(index, 1);
        if (AllDataId.length == 0)
            $(".panelSiteData").hide();
    },
    SelectData: function () {
        if (TempData.length > 0) {
            $.each(TempData, function (i, item) {
                $("#cb_" + item.trxDataAccrueID + " input[type=checkbox]").prop('checked', true);
            })
        }
    },

    IsEmptyData: function (ID) {
        var result = true;
        if (TempData.length > 0) {
            for (var i = 0 ; i < TempData.length ; i++) {
                if (TempData[i].trxDataAccrueID == ID) {
                    result = false;
                    break;
                }
            }
        }
        return result;
    },

    numberWithCommas: function (x) {
        //return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        var parts = x.toString().split(".");
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        return parts.join(".");
    },
}

var Helper = {

    RemoveElementFromArray: function (arr) {
        var what, a = arguments, L = a.length, ax;
        while (L > 1 && arr.length) {
            what = a[--L];
            while ((ax = arr.indexOf(what)) !== -1) {
                arr.splice(ax, 1);
            }
        }
        return arr;
    },
    IsElementExistsInArray: function (value, arr) {
        var isExist = false;
        for (var i = 0 ; i < arr.length ; i++) {
            if (arr[i] == value) {
                isExist = true;
                break;
            }
        }
        return isExist;
    },
   
    CheckIsReConfirm: function () {
        //for CheckAll Pages     
        var params = {

            date: fsdate,
            week: fsweek
        };
        $.ajax({
            url: "/api/FinalConfirmation/CheckIsReConfirm",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            
            if (data != null) {
                if (data.IsReConfirm != "0")
                    $("#btReConfirm").hide();
                else {
                    $("#btReConfirm").show();
                    $("#btFinalConfirm").show();
                }
            }
            else {
                $("#btReConfirm").hide();
                $("#btFinalConfirm").hide();
            }
            
            
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
        
    },
    GetWeekNowSelected: function () {
        //for CheckAll Pages     
        var params = {
        };
        $.ajax({
            url: "/api/UserConfirmation/GetWeekNowSelected",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            $('#slSearchPeriodeWeek').val(data).trigger('change');;
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });

    }

}