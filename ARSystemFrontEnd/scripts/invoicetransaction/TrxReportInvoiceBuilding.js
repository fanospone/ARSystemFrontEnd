Data = {};

var fsInvoiceDateFrom = "";
var fsInvoiceDateTo = "";
var fsInvoiceYear = "";
var fsInvoiceMonth = "";
var fsInvoiceWeek = "";
var fsInvNo = "";

/* Helper Functions */

jQuery(document).ready(function () {

    $(".ByInvoiceControl").hide(); //won't be used
    Table.Init();
    Table.Search();

    Control.BindingYear();
    Control.BindingMonth();
    Control.BindingWeek();

    //panel Summary
    $("#formSearch").submit(function (e) {
        Table.Search();
        e.preventDefault();
    });

    $("#btSearch").unbind().click(function () {
        Table.Search();
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
    });

    // Initialize Datepicker
    $("body").delegate(".datepicker", "focusin", function () {
        $(this).datepicker({
            format: "dd-M-yyyy"
        });
    });

    var date = new Date();
    $('#tbSearchInvoiceDateFrom').val(Common.Format.ConvertJSONDateTime(date));
    $('#tbSearchInvoiceDateTo').val(Common.Format.ConvertJSONDateTime(date));
});

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

        var minDate = "1900-01-01";
        var maxDate = "9999-12-31";

        var params = {
            invPrintDateFrom: ($("#tbSearchInvoiceDateFrom").val() == null || $("#tbSearchInvoiceDateFrom").val() == "") ? minDate : Helper.ReverseDateToSQLFormat($("#tbSearchInvoiceDateFrom").val()),
            invPrintDateTo: ($("#tbSearchInvoiceDateTo").val() == null || $("#tbSearchInvoiceDateTo").val() == "") ? maxDate : Helper.ReverseDateToSQLFormat($("#tbSearchInvoiceDateTo").val()),
            year: $("#slSearchYear").val() == null ? 0 : $("#slSearchYear").val(),
            month: $("#slSearchMonth").val() == null ? 0 : $("#slSearchMonth").val(),
            week: $("#slSearchWeek").val() == null ? 0 : $("#slSearchWeek").val(),
            InvNo: $("#tbSearchInvNo").val()
        };

        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/ReportInvoiceBuilding/grid",
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
                { data: "InvNo" },
                { data: "InvSubject" },
                {
                    data: "InvPrintDate", render: function (data, type, full) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "PostingDate", render: function (data, type, full) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "Company" },
                { data: "CustomerName" },
                {
                    data: "Area", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "MeterPrice", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                { data: "TermPeriod" },
                { data: "Currency" },
                {
                    data: "InvSumADPP", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "Discount", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "InvTotalAPPN", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "InvTotalPenalty", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").show();
                }
                l.stop(); App.unblockUI('.panelSearchResult');
            },
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': []
            }],
            "order": [],
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 1 /* Set the 2 most left columns as fixed columns */
            },
        });

        fsInvoiceDateFrom = ($("#tbSearchInvoiceDateFrom").val() == null || $("#tbSearchInvoiceDateFrom").val() == "") ? minDate : Helper.ReverseDateToSQLFormat($("#tbSearchInvoiceDateFrom").val());
        fsInvoiceDateTo = ($("#tbSearchInvoiceDateTo").val() == null || $("#tbSearchInvoiceDateTo").val() == "") ? maxDate : Helper.ReverseDateToSQLFormat($("#tbSearchInvoiceDateTo").val());
        fsInvoiceYear = $("#slSearchYear").val() == null ? 0 : $("#slSearchYear").val();
        fsInvoiceMonth = $("#slSearchMonth").val() == null ? 0 : $("#slSearchMonth").val();
        fsInvoiceWeek = $("#slSearchWeek").val() == null ? 0 : $("#slSearchWeek").val();
        fsInvNo = $("#tbSearchInvNo").val();
    },
    Reset: function () {
        fsInvoiceDateFrom = "";
        fsInvoiceDateTo = "";
        fsInvoiceYear = "";
        fsInvoiceMonth = "";
        fsInvoiceWeek = "";
        fsInvNo = "";
        $("#tbSearchInvNo").val("");
        $("#tbSearchInvoiceDateFrom").val("");
        $("#tbSearchInvoiceDateTo").val("");
        $("#slSearchYear").val("").trigger("change");
        $("#slSearchMonth").val("").trigger("change");
        $("#slSearchWeek").val("").trigger("change");
    },
    Export: function () {
        window.location.href = "/InvoiceTransaction/TrxReportInvoiceBuilding/Export?invPrintDateFrom=" + fsInvoiceDateFrom + "&invPrintDateTo=" + fsInvoiceDateTo + "&year=" + fsInvoiceYear + "&month=" + fsInvoiceMonth + "&week=" + fsInvoiceWeek + "&invNo=" + fsInvNo;
    }
}

var Control = {
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

            $("#slSearchYear").select2({ placeholder: "Year", width: null }).on("change", function (e) {
                Control.BindingWeek();
            });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingMonth: function () {
        $.ajax({
            url: "/api/ReportInvoiceTower/GetMonth",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchMonth").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchMonth").append("<option value='" + item.ValueId + "'>" + item.ValueDesc + "</option>");
                })
            }

            $("#slSearchMonth").select2({ placeholder: "Month", width: null }).on("change", function (e) {
                Control.BindingWeek();
            });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingWeek: function () {
        var params = {
            intYearPosting: $("#slSearchYear").val() == null || $("#slSearchYear").val() == "" ? 0 : $("#slSearchYear").val(),
            intMonthPosting: $("#slSearchMonth").val() == null || $("#slSearchMonth").val() == "" ? 0 : $("#slSearchMonth").val()

        };
        $.ajax({
            url: "/api/ReportInvoiceTower/GetWeek",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchWeek").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchWeek").append("<option value='" + item.ValueId + "'>" + item.ValueDesc + "</option>");
                })
            }

            $("#slSearchWeek").select2({ placeholder: "Week", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    }
}

var Helper = {
    ReverseDateToSQLFormat: function (dateValue) {
        var dateComponents = dateValue.split("-");
        var dateValue = dateComponents[0];
        var monthValue = dateComponents[1];
        var yearValue = dateComponents[2];
        var allMonths = "JanFebMarAprMayJunJulAugSepOctNovDec";

        var monthNumberValue = allMonths.indexOf(monthValue) / 3 + 1;

        return monthNumberValue + "/" + dateValue + "/" + yearValue;
    }
}