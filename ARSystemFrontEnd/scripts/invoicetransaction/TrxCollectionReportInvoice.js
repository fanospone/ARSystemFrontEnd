Data = {};

//filter search 
var fsStartPeriod = "";
var fsEndPeriod = "";
var fsStartPaidDate = "";
var fsEndPaidDate = "";
var fsCompanyId = "";
var fsOperator = "";
var fsPaidStatus = "";
var fsInvoiceCategory = "";
var fsCustomerId = 0;
var fsInvNo = "";

jQuery(document).ready(function () {
    Form.Init();

    PKPPurpose.Set.UserCompanyCode();
    if (PKPPurpose.Temp.UserCompanyCode == Constants.CompanyCode.PKP) {
        PKPPurpose.Filter.PKPOnly();
    }
    Table.Search();
    //panel Summary
    $("#formSearch").submit(function (e) {
        Table.Search();
        e.preventDefault();
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
        if (PKPPurpose.Temp.UserCompanyCode == Constants.CompanyCode.PKP) {
            PKPPurpose.Filter.PKPOnly();
        }
    });
});

var Form = {
    Init: function () {
        Control.BindingSelectCompany();
        Control.BindingSelectOperator();
        Control.BindingSelectCustomer();
        Control.BindingSelectInvoiceCategory();
        $("#slSearchPaidStatus").select2({ placeholder: "Select Paid Status", width: null });

        Table.Reset();
        // Initialize Datepicker
        $("body").delegate(".date-picker", "focusin", function () {
            $(this).datepicker({
                format: "dd-M-yyyy"
            });
        });
        Table.Init();
        $(".panelSearchResult").hide();

    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        Table.Search();
        //Table.Reset();
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
        fsEndPeriod = $("#tbEndPeriod").val();
        fsStartPaidDate = $("#tbStartPaidDate").val();
        fsEndPaidDate = $("#tbEndPaidDate").val();
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsPaidStatus = $("#slSearchPaidStatus").val() == null? "" : $("#slSearchPaidStatus").val();
        fsInvoiceCategory = $("#slSearchInvoiceCategory").val() == null || $("#slSearchInvoiceCategory").val() == "" ? 0 : $("#slSearchInvoiceCategory").val();
        fsCustomerId = $("#slSearchCustomer").val() == null || $("#slSearchCustomer").val() == "" ? 0 : $("#slSearchCustomer").val();
        fsInvNo = $("#tbInvNo").val();

        var params = {
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            strStartPaidDate: fsStartPaidDate,
            strEndPaidDate: fsEndPaidDate,
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strPaidStatus: fsPaidStatus,
            intInvoiceCategory: fsInvoiceCategory,
            intCustomerId: fsCustomerId,
            InvNo : fsInvNo
        };

        var tblSummaryData = $("#tblSummaryData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/CollectionReportInvoiceTower/grid",
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
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvTemp" },
                {
                    data: "InvReceiptDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "InvPaidDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "Description" },
                { data: "InvCompanyId" },
                { data: "InvOperatorID" },
                { data: "CustomerName" },
                { data: "InvSumADPP", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvTotalAPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "PAT", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "PPH", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "PPF", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                {
                    data: "InvTotalAmount", className: "text-right", render: function (data, type, full) {
                        return Common.Format.CommaSeparation(full.InvTotalAmount + full.PPH + full.PPF);
                    }
                },
                { data: "PartialPaid", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvSumADPP", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "PAM", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },//Paid Amount 
                { data: "RND", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },//CR
                { data: "DBT", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },//DR
                { data: "RTG", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },//RTGS
                { data: "PNT", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },//PNT
                { data: "PPE", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },//PPN Expired
                { data: "WAPU", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },//WAPU 
                { data: "PaidStatus" },//Paid Status 
                { data: "StatusAx" },//Status 
                {
                    data: "VerificationDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "mstInvoiceCategoryId", mRender: function (data, type, full) {
                        return full.mstInvoiceCategoryId == "2" ? "Building" : "Tower";
                    }
                },
                { data: "LastPosition" }
            ],
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
            "orderBy": [],
            "fixedColumns": {
                "leftColumns": 1
            },
            "scrollX": true,
            "scrollCollapse": true
        });

    },
    Reset: function () {
        fsStartPeriod = "";
        fsEndPeriod = "";
        fsStartPaidDate = "";
        fsEndPaidDate = "";
        fsCompanyId = "";
        fsOperator = "";
        fsPaidStatus = "";
        fsInvoiceCategory = 0;
        fsCustomerId = 0;
        fsInvNo = "";

        $("#tbStartPeriod").val("");
        $("#tbEndPeriod").val("");
        $("#tbStartPaidDate").val("");
        $("#tbEndPaidDate").val("");
        $("#slSearchCompanyName").val("").trigger('change');
        $("#slSearchOperator").val("").trigger('change');
        $("#slSearchPaidStatus").val("").trigger('change'); 
        $("#slSearchInvoiceCategory").val("").trigger('change');
        $("#tbInvNo").val("");
    },
    Export: function () {
        window.location.href = "/InvoiceTransaction/TrxCollectionReport/Export?strStartPeriod=" + fsStartPeriod + "&strEndPeriod=" + fsEndPeriod
           + "&strStartPaidDate=" + fsStartPaidDate + "&strEndPaidDate=" + fsEndPaidDate + "&strCompanyId=" + fsCompanyId + "&strOperator=" + fsOperator
           + "&strPaidStatus=" + fsPaidStatus + "&intInvoiceCategory=" + fsInvoiceCategory + "&intCustomerId=" + fsCustomerId + "&InvNo=" + fsInvNo;
    }
}

var Control = {
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchCompanyName").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchCompanyName").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                });
            }

            $("#slSearchCompanyName").select2({ placeholder: "Select Company Name", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectCustomer: function () {
        $.ajax({
            url: "/api/MstDataSource/Customer",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchCustomer").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchCustomer").append("<option value='" + item.CustomerID + "'>" + item.CustomerName + "</option>");
                });
            }

            $("#slSearchCustomer").select2({ placeholder: "Select Customer", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectOperator: function () {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchOperator").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchOperator").append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                });
            }

            $("#slSearchOperator").select2({ placeholder: "Select Operator", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectInvoiceCategory: function () {
        $.ajax({
            url: "/api/MstDataSource/InvoiceCategory",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchInvoiceCategory").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchInvoiceCategory").append("<option value='" + item.mstInvoiceCategoryId + "'>" + item.Description + "</option>");
                });
            }

            $("#slSearchInvoiceCategory").select2({ placeholder: "Select Invoice Category", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    }
}


var Constants = {
    CompanyCode: {
        PKP: "PKP"
    }
}

var PKPPurpose = {
    Filter: {
        PKPOnly: function () {
            $('#slSearchCompanyName').val(Constants.CompanyCode.PKP).trigger('change');
        },
    },
    Set: {
        UserCompanyCode: function () {
            PKPPurpose.Temp.UserCompanyCode = $('#hdUserCompanyCode').val();
        }
    },
    Temp: {
        UserCompanyCode: ""
    }
}