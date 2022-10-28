Data = {};

//filter search 
var fsStartPeriod = "";
var fsEndPeriod = "";
var fsYearPosting = "";
var fsMonthPosting = "";
var fsWeekPosting = "";
var fsInvNo = "";

jQuery(document).ready(function () {

    $(".ByInvoiceControl").hide(); //won't be used
    Form.Init();
    //Table.Search();
    //panel Summary
    $("#formSearch").submit(function (e) {
        if ($('input[name=rbViewBy]:checked').val() == 0)//View By Invoice
        {
            Table.Search();
            e.preventDefault();
        }
        else {
            TableSoNumber.Search();
            e.preventDefault();
        }
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
    });
    $('input[type=radio][name=rbViewBy]').change(function () {
        if ($('input[name=rbViewBy]:checked').val() == 0)//View By Invoice
        {
            Table.Search();
            $(".BySoNumber").hide();
            $(".ByInvoice").show();
        }
        else { //View By SO Number
            TableSoNumber.Search();
            $(".BySoNumber").show();
            $(".ByInvoice").hide();
        }
    });

});

var Form = {
    Init: function () {
        Control.BindingYear();
        Control.BindingMonth();
        Control.BindingWeek();
        Table.Reset();
        // Initialize Datepicker
        $("body").delegate(".date-picker", "focusin", function () {
            $(this).datepicker({
                format: "dd-M-yyyy"
            });
        });
        Table.Init();
        TableSoNumber.Init();
        $(".panelSearchResult").hide();
        $(".BySoNumber").hide();

        var date = new Date();
        $('#tbStartPeriod').val(Common.Format.ConvertJSONDateTime(date));
        $('#tbEndPeriod').val(Common.Format.ConvertJSONDateTime(date));

    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        Table.Search();
    }
}

var Table = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val()
        fsYearPosting = $("#slSearchYear").val() == null || $("#slSearchYear").val() == "" ? 0 : $("#slSearchYear").val();
        fsMonthPosting = $("#slSearchMonth").val() == null || $("#slSearchMonth").val() == "" ? 0 : $("#slSearchMonth").val();
        fsWeekPosting = $("#slSearchWeek").val() == null || $("#slSearchWeek").val() == "" ? 0 : $("#slSearchWeek").val();
        fsInvNo = $("#tbSearchInvNo").val();

        var params = {
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            intYearPosting: fsYearPosting,
            intMonthPosting: fsMonthPosting,
            intWeekPosting: fsWeekPosting,
            invNo: fsInvNo
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/ReportInvoiceTower/gridCompile",
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
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [

                { data: "InvNo" },
                { data: "InvSubject" },
                { data: "InvSumADPP", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvCompanyId" },
                { data: "CompanyIdAx" },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvTemp" },
                { data: "AccountType" },
                { data: "InvoiceCategory" },
                { data: "ElectricityCategory" },
                { data: "InvOperatorID" },
                { data: "Credit" },
                { data: "Currency" },
                { data: "Xrate" },
                { data: "DocNumber" },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "DueDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "PostingProfile" },
                { data: "OffSetAccount" },
                { data: "TaxGroup" },
                { data: "TaxItemGroup" },
                { data: "TaxInvoiceNo" },
                {
                    data: "FPJDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "PostingDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "OperatorRegion" },
                { data: "Address" }
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');
            },
            "order": [],
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            }
        });

    },
    Reset: function () {
        fsYearPosting = 0;
        fsMonthPosting = 0;
        fsWeekPosting = 0;
        fsStartPeriod = "";
        fsEndPeriod = "";
        fsInvNo = "";

        $("#slSearchYear").val("").trigger('change');
        $("#slSearchMonth").val("").trigger('change');
        $("#slSearchWeek").val("").trigger('change');
        $("#tbStartPeriod").val("");
        $("#tbEndPeriod").val("");
        $("#tbSearchInvNo").val("");
    },
    Export: function () {
        window.location.href = "/InvoiceTransaction/TrxReportInvoiceCompile/Export?intYearPosting=" + fsYearPosting + "&intMonthPosting=" + fsMonthPosting
           + "&intWeekPosting=" + fsWeekPosting + "&strStartPeriod=" + fsStartPeriod + "&strEndPeriod=" + fsEndPeriod + "&invNo=" + fsInvNo;
    }
}

var TableSoNumber = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        $(window).resize(function () {
            $("#tblSummaryDataSoNumber").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        fsInvNo = $("#tbSearchInvNo").val();
        var params = {
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            invNo: fsInvNo
        };
        var tblSummaryData = $("#tblSummaryDataSoNumber").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/ReportInvoiceTower/gridCompileBySONumber",
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
                        TableSoNumber.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [

                { data: "SONumber" },
                { data: "SiteIdOld" },
                { data: "SiteName" },
                { data: "InvNo" },
                { data: "InvSubject" },
                {
                    data: "StartDatePeriod", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndDatePeriod", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvSumADPP", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvCompanyId" },
                { data: "CompanyIdAx" },
                { data: "InvTemp" },
                { data: "Description" },
                { data: "InvoiceCategory" },
                { data: "InvOperatorID" },
                { data: "Credit" },
                { data: "Currency" },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    } //nanti ganti SLD dari table M_RFI
                },
                {
                    data: "BapsReceiveDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    } 
                },
                {
                    data: "BapsDone", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "DueDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "PostingProfile" },
                { data: "OffSetAccount" },
                { data: "TaxGroup" },
                { data: "TaxItemGroup" },
                { data: "TaxInvoiceNo" },
                {
                    data: "FPJDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "BapsConfirmDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    } //posting date or invoice date? existing invoice date
                },
                {
                    data: "InvFirstPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "InvReceiptDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    } 
                },
                { data: "LeadTimeVerificator" }, //lead time baps confirm ?
                { data: "LeadTimeVerificator" }, 
                { data: "LeadTimeInputer" },
                { data: "LeadTimeFinishing" },//LeadTimeFinishing
                { data: "LeadTimeARData" } //LeadTimeARData
            ],
            "columnDefs": [{ "targets": [0, 1], "orderable": true }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    if (this.fnSettings().fnRecordsTotal() > 0) {
                        $(".panelSearchBegin").hide();
                        $(".panelSearchResult").fadeIn();
                    }
                }
                l.stop(); App.unblockUI('.panelSearchResult');
            },
            "order": [],
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
        });

    },
    Export: function () {
        var strStartPeriod = fsStartPeriod;
        var strEndPeriod = fsEndPeriod;

        window.location.href = "/InvoiceTransaction/TrxReportInvoiceCompileSONumber/Export?strStartPeriod=" + strStartPeriod + "&strEndPeriod=" + strEndPeriod + "&invNo=" + fsInvNo;
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
