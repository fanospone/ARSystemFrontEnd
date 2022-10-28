Data = {};

//filter search 
var fsStartPeriod = "";
var fsEndPeriod = "";
var fsCompanyId = "";
var fsOperator = "";
var fsCNStatus = "";
var fsInvNo = "";
var fsInvoiceTypeId = "";

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
        $("#slSearchCNStatus").select2({ placeholder: "Select CN Status", width: null });
        Control.BindingSelectInvoiceType();

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
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsCNStatus = $("#slSearchCNStatus").val() == null ? "" : $("#slSearchCNStatus").val();
        fsInvNo = $("#tbInvNo").val();
        fsInvoiceTypeId = $("#slSearchTermInvoice").val() == null ? "" : $("#slSearchTermInvoice").val();

        var params = {
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            InvNo: fsInvNo,
            CNStatus: fsCNStatus,
            InvoiceTypeId: fsInvoiceTypeId
        };

        var tblSummaryData = $("#tblSummaryData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/HistoryCNInvoiceARCollection/grid",
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
                        Table.Export();
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
                        strReturn += "<button type='button' title='Print' class='btn blue btn-xs btPrint'>Print</button>";

                        return strReturn;
                    }
                },
                { data: "InvNo" },
                { data: "InvParentNo" },                
                { data: "InvTemp" },
                { data: "InvSumADPP", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvTotalAPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "RequestedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "RequestedBy" },
                {
                    data: "ApprovedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "ApprovedBy" },
                { data: "Description" },
                { data: "InvCompanyId" },
                { data: "InvOperatorID" },
                { data: "isCNApproved" },
                { data: "PicaDetailRequestor" },
                { data: "Remark" }
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
                "leftColumns": 2
            },
            "scrollX": true,
            "scrollCollapse": true
        });

        $("#tblSummaryData tbody").on("click", "button.btPrint", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var selectedRow = table.row($(this).parents('tr')).data();
            var isApproved = selectedRow.isCNApproved == "APPROVED" ? "1" : "0";
            window.location.href = "/InvoiceTransaction/TrxPrintCNInvoiceTower/Print?trxCNInvoiceHeaderID=" + selectedRow.trxInvoiceHeaderID
                + "&mstInvoiceCategoryId=" + selectedRow.mstInvoiceCategoryId + "&isApproved=" + isApproved;
        });

    },
    Reset: function () {
        fsStartPeriod = "";
        fsEndPeriod = "";
        fsCompanyId = "";
        fsOperator = "";
        fsCNStatus = "";
        fsInvNo = "";
        fsInvoiceTypeId = "";

        $("#tbStartPeriod").val("");
        $("#tbEndPeriod").val("");
        $("#slSearchCompanyName").val("").trigger('change');
        $("#slSearchOperator").val("").trigger('change');
        $("#slSearchCNStatus").val("").trigger('change');
        $("#slSearchTermInvoice").val("").trigger('change');
        $("#tbInvNo").val("");
    },
    Export: function () {
        window.location.href = "/InvoiceTransaction/TrxHistoryCNInvoiceARCollectionReport/Export?strStartPeriod=" + fsStartPeriod + "&strEndPeriod=" + fsEndPeriod
           + "&strCompanyId=" + fsCompanyId + "&strOperator=" + fsOperator
           + "&strCNStatus=" + fsCNStatus + "&InvNo=" + fsInvNo + "&InvoiceTypeId=" + fsInvoiceTypeId;
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
    },
    BindingSelectInvoiceType: function () {
        $.ajax({
            url: "/api/MstDataSource/InvoiceType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchTermInvoice").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchTermInvoice").append("<option value='" + item.mstInvoiceTypeId + "'>" + item.Description + "</option>");
                })
            }

            $("#slSearchTermInvoice").select2({ placeholder: "Select Term Invoice", width: null });

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