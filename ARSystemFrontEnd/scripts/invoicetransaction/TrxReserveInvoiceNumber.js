Data = {};

var fsOperator = '';
var fsYear = '';
var fsInvCompanyId = '';
var fsReservedNo = '';
var fsStartPeriod = "";
var fsEndPeriod = "";

/* Helper Functions */

jQuery(document).ready(function () {
    Form.Init();
    Table.Init();
    Table.Search();

    //panel Summary
    $("#formSearch").submit(function (e) {
        
        if ($("#tabReserve").tabs('option', 'active') == 0)
            Table.Search();
        else
            TableReplace.Search();
        e.preventDefault();
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
    });

    $(".btCancel").unbind().click(function () {
        Form.Cancel();
    });
    $(".btCancelReplace").unbind().click(function () {
        Form.CancelReplace();
    });

    $("#btBack").unbind().click(function () {
        $("#pnlSummary").fadeIn();
        $(".panelDetailData").hide();
    });
    $("#btBackReplace").unbind().click(function () {
        $("#pnlSummary").fadeIn();
        $(".panelDetailDataReplace").hide();
    });


    $("#btAdd").unbind().click(function (e) {
        Form.RequestNumber();
    });
    $("#formReserve").submit(function (e) {
        Process.ReserveNumber();
        e.preventDefault();
    });
    $("#formTransaction").submit(function (e) {
        Control.GetInvoiceProperties();
        e.preventDefault();
    });
    

    $('#tabReserve').unbind().on('tabsactivate', function (event, ui) {
        var newIndex = ui.newTab.index();
        if (newIndex == 0) {
            Table.Search();
            $(".ReplaceCtrl").hide();
            $(".ReserveCtrl").show();
        }
        else {
            TableReplace.Search();
            $(".ReplaceCtrl").show();
            $(".ReserveCtrl").hide();
        }
    });
    $("#btSaveReplace").unbind().click(function () {
        Process.ReplaceNumber();
    });
    $("#btRelease").unbind().click(function () {
        Process.ReleaseNumber();
    });
});

var Form = {
    Init: function () {
        Control.BindingSelectCompany("#slSearchCompanyName");
        Control.BindingSelectOperator("#slSearchOperator");
        Control.BindingYear();
        Control.BindingSelectCompany("#slRequestCompany");
        Control.BindingSelectOperator("#slRequestOperator");
        $(".panelDetailData").hide();
        $(".panelDetailDataReplace").hide();
        $("#pnlTransaction").hide();
        $("#form").parsley();
        if (!$("#hdAllowProcess").val()) {
            $("#btSave").hide();
        }
        Table.Reset();
        $('#tabReserve').tabs();
        $(".ReplaceCtrl").hide();
        $(".ReserveCtrl").show();
        $("#formTransaction").parsley();
        $("body").delegate(".date-picker", "focusin", function () {
            $(this).datepicker({
                format: "dd-M-yyyy"

            })

        });

    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $(".panelDetailData").hide(); 
        $("#pnlTransaction").hide(); 
        Table.Search();
    },
    CancelReplace: function () {
        $("#pnlSummary").fadeIn();
        $(".panelDetailDataReplace").hide();
        TableReplace.Search();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $(".panelDetailDataReplace").hide();
        $("#pnlTransaction").hide();
        $(".panelSearchBegin").fadeIn();
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        if ($("#tabReserve").tabs('option', 'active') == 0) {
            Table.Search();
        }
        else {
            TableReplace.Search();
        }
    },
    SelectedData: function () {
        $("#pnlSummary").hide();
        Data.RowSelected = Data.Selected.Detail;

        var tblDetailData = $("#tblDetailData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "data": Data.RowSelected,
            "filter": false,
            "destroy": true,
            "columns": [
                { data: "CompanyId" },
                { data: "OperatorId" },
                { data: "InvNo" }
            ],

        });
        $(".panelDetailData").fadeIn();
    },
    SelectedDataReplace: function () {
        $("#pnlSummary").hide();
        $("#formTransaction").parsley().reset();
        $("#tbReservedInvoiceNumber").val(Data.SelectedReplace.InvNo);
        Control.BindingPostedInvoiceNumber();

        $(".panelDetailDataReplace").fadeIn();
        $(".divInvoiceNumberProperties").hide();
        $("#btSaveReplace").prop('disabled', true);
        Helper.ScrollToTop();
    },
    RequestNumber: function () {
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Reserve Number");
        $("#form").parsley().reset();

        $("#slRequestCompany").val("").trigger('change');
        $("#slRequestOperator").val("").trigger('change');
        $("#tbRequestAmount").val("");
        $("#tbRemarks").val("");
    }
}

var Table = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblSummaryData = $('#tblSummaryData').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsOperator = ($("#slSearchOperator").val() == null) ? "" : $("#slSearchOperator").val();
        fsInvCompanyId = ($("#slSearchCompanyName").val() == null) ? "" : $("#slSearchCompanyName").val();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        var params = {
            OperatorId: fsOperator,
            CompanyId: fsInvCompanyId,
            StartDateRequest: fsStartPeriod,
            EndDateRequest: fsEndPeriod,
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/ReserveNumber/grid",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            "columns": [

                {
                    data: "trxReserveInvoiceNumberHeaderId", mRender: function (data, type, full) {
                        return "<a class='btSelect'>" + data + "</a>";
                    }
                },
                { data: "CompanyId" },
                { data: "OperatorId" },
                { data: "AmountReserve" },
                { data: "CreatedBy" },
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").show();
                }
                l.stop();
            },
            "order": [],
            "scrollCollapse": true
        });

        $("#tblSummaryData tbody").unbind();


        $("#tblSummaryData tbody").on("click", "a.btSelect", function (e) {
            var table = $("#tblSummaryData").DataTable();
            Data.Selected = {};
            Data.Selected = table.row($(this).parents('tr')).data();
            Form.SelectedData();
            e.preventDefault();
        });

    },
    Reset: function () {
        fsOperator = '';
        fsYear = '';
        fsInvCompanyId = '';
        fsReservedNo = '';

        fsStartPeriod = "";
        fsEndPeriod = "";

        $("#slSearchCompanyName").val("").trigger("change");
        $("#slSearchOperator").val("").trigger("change");
        $("#tbInvNo").val("");
        $("#slSearchYear").val("").trigger("change");
    },
    Export: function () {
        var href = "/InvoiceTransaction/TrxReserveInvoiceNumberToExport/Export?OperatorId=" + fsOperator + "&CompanyId=" + fsInvCompanyId
            + "&StartDateRequest=" + fsStartPeriod + "&EndDateRequest=" + fsEndPeriod;

        window.location.href = href;
    }
}

var TableReplace = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblSummaryDataReplace = $('#tblSummaryDataReplace').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });


        $(window).resize(function () {
            $("#tblSummaryDataReplace").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsOperator = ($("#slSearchOperator").val() == null) ? "" : $("#slSearchOperator").val();
        fsInvCompanyId = ($("#slSearchCompanyName").val() == null) ? "" : $("#slSearchCompanyName").val();
        fsYear = ($("#slSearchYear").val() == null) ? "" : $("#slSearchYear").val();
        fsReservedNo = $("#tbInvNo").val();
        var params = {
            OperatorId: fsOperator,
            CompanyId: fsInvCompanyId,
            ReserveNo: fsReservedNo,
            Year: fsYear
        };
        var tblSummaryDataReplace = $("#tblSummaryDataReplace").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/ReserveNumber/gridReplace",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        TableReplace.Export();
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            "columns": [

                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        if ($("#hdAllowProcess").val())
                            strReturn += "<button type='button' title='Select' class='btn blue btSelectReplace btn-xs'><i class='fa fa-mouse-pointer'></i></button>";
                        return strReturn;
                    }
                },
                //{
                //    data: "trxReserveInvoiceNumberDetailId", mRender: function (data, type, full) {
                //        return "<a class='btSelectReplace'>" + data + "</a>";
                //    }
                //},
                { data: "InvNo" },
                { data: "CompanyId" },
                { data: "OperatorId" },
                { data: "Year" },
                { data: "PairedNo" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryDataReplace.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").show();
                }
                l.stop();
            },
            "order": [],
            "scrollCollapse": true
        });

        $("#tblSummaryDataReplace tbody").unbind();


        $("#tblSummaryDataReplace tbody").on("click", "button.btSelectReplace", function (e) {
            var table = $("#tblSummaryDataReplace").DataTable();
            Data.SelectedReplace = {};
            Data.SelectedReplace = table.row($(this).parents('tr')).data();
            Form.SelectedDataReplace();
            e.preventDefault();
        });

    },
    Export: function () {
        var href = "/InvoiceTransaction/TrxReplaceInvoiceNumberToExport/Export?OperatorId=" + fsOperator + "&CompanyId=" + fsInvCompanyId + "&ReservedNo=" + fsReservedNo + "&Year=" + fsYear;

        window.location.href = href;
    }
}


var Control = {
    BindingSelectCompany: function (selector) {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(selector).html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(selector).append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
            }

            $(selector).select2({ placeholder: "Select Company Name", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectOperator: function (selector) {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(selector).html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(selector).append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                })
            }

            $(selector).select2({ placeholder: "Select Operator", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingYear: function () {
        $.ajax({
            url: "/api/ReportInvoiceTower/GetYear",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchYear").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchYear").append("<option value='" + item.ValueId + "'>" + item.ValueDesc + "</option>");
                })
            }

            $("#slSearchYear").select2({ placeholder: "Select Year", width: null }).on("change", function (e) {
                Control.BindingWeek();
            });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingPostedInvoiceNumber: function () {
        $.ajax({
            url: "/api/ReserveNumber/GetInvoiceNumber",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slInvoNo").html("<option></option>")

            if (Common.CheckError.List(data.ListPostedInvoiceNumber)) {
                $.each(data.ListPostedInvoiceNumber, function (i, item) {
                    $("#slInvoNo").append("<option value='" + item + "'>" + item + "</option>");
                })
            }
            $("#slInvoNo").select2({ placeholder: "Select Invoice Number", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    GetInvoiceProperties: function () {
        var params = {
            InvNo: $("#slInvoNo").val()
        };
        $.ajax({
            url: "/api/ReserveNumber/GetInvoiceNumberProperties",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $("#tbInvoiceTemp").val(data.InvTemp);
                $("#tbCompany").val(data.InvCompanyId);
                $("#tbOperator").val(data.InvOperatorId);
                $("#tbPrintDate").val(Common.Format.ConvertJSONDateTime(data.InvPrintDate));
                $("#tbDPP").val(Common.Format.CommaSeparation(data.SumADPP));
                $("#tbPPN").val(Common.Format.CommaSeparation(data.InvTotalAPPN));
                $("#tbPPH").val(Common.Format.CommaSeparation(data.InvTotalAPPH));
                $(".divInvoiceNumberProperties").show();
                $("#btSaveReplace").prop('disabled', false);
                
            }

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    }
}

var Helper = {
    FormatCurrency: function (value) {
        if (isNaN(value))
            return "0.00";
        else
            return parseFloat(value, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
    },
    ScrollToTop: function () {
        $("html, body").animate({ scrollTop: theOffset.top - 30 }, "slow");
        return false;
    }
}

var Process = {
    ReserveNumber: function () {
        var l = Ladda.create(document.querySelector("#btReserve"))

        var params = {
            OperatorId: $("#slRequestOperator").val(),
            CompanyId: $("#slRequestCompany").val(),
            Remarks: $("#tbRemarks").val(),
            AmountReserve: $("#tbRequestAmount").val()

        }

        $.ajax({
            url: "/api/ReserveNumber/Reserve",
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
                Common.Alert.Success("Invoice Number Requested")
            }
            l.stop();

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Form.Done();
        })
    },
    ReplaceNumber: function () {
        var l = Ladda.create(document.querySelector("#btSaveReplace"))

        var params = {
            DataReservedInvoiceNumber: Data.SelectedReplace,
            InvNoHeader: $("#slInvoNo").val()

        }

        $.ajax({
            url: "/api/ReserveNumber/ReplaceInvoiceNumber",
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
                Common.Alert.Success("Invoice Number Replaced")
            }
            l.stop();

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Form.Done();
        })
    },
    ReleaseNumber: function () {
        var l = Ladda.create(document.querySelector("#btRelease"))

        var params = {
            InvNo: $("#tbReservedInvoiceNumber").val()
        };
        //$.ajax({
        //    url: "/api/ReserveNumber/GetInvoiceNumberProperties",
        //    type: "GET",
        //    data: params
        //})
        $.ajax({
            url: "/api/ReserveNumber/ReleaseNumber",
            type: "GET",
            data: params
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                if (data.isValid)
                {
                    Common.Alert.Success(data.Result)
                    Form.Done();
                }
                else
                { Common.Alert.Warning(data.Result) }

            }
            l.stop();

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
        })
    },
}