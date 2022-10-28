Data = {};

var fsCustomerName = '';
var fsInvoiceTypeId = '';
var fsInvCompanyId = '';
var fsInvNo = '';
var fsReceiptDateFrom = '';
var fsReceiptDateTo = '';

/* Helper Functions */

jQuery(document).ready(function () {
    Table.Init();
    Table.Search();

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

    $("#btDownload").unbind().on("click", function (e) {
        Helper.Download(Data.Selected.FilePath, Data.Selected.InvReceiptFile, Data.Selected.ContentType);
    });

    //Control.BindingSelectInvoiceType();
    Control.BindingSelectCompany();
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

        fsInvOperatorId = ($("#slSearchOperator").val() == null) ? "" : $("#slSearchOperator").val();
        fsInvoiceTypeId = ($("#slSearchTerm").val() == null) ? "" : $("#slSearchTerm").val();
        fsInvCompanyId = ($("#slSearchCompany").val() == null) ? "" : $("#slSearchCompany").val();
        fsInvNo = $("#tbInvNo").val();
        fsReceiptDateFrom = ($("#tbSearchReceiptDateFrom").val() == "") ? null : $("#tbSearchReceiptDateFrom").val();
        fsReceiptDateTo = ($("#tbSearchReceiptDateTo").val() == "") ? null : $("#tbSearchReceiptDateTo").val();

        var params = {
            invOperatorId: fsInvOperatorId,
            invoiceTypeId: fsInvoiceTypeId,
            invCompanyId: fsInvCompanyId,
            invNo: fsInvNo,
            receiptDateFrom: fsReceiptDateFrom,
            receiptDateTo: fsReceiptDateTo
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/ARProcessInvoiceBuilding/gridHistory",
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
                { data: "Company" },
                { data: "CustomerName" },
                {
                    data: "ReceiptDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "Remark" },
                {
                    data: "ARProcessPenalty", className: "text-right", render: function (data, type, full) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    render: function (data, type, full) {
                        if (full.InvReceiptFile != null && full.InvReceiptFile != '')
                            return '<button type="button" class="btn btn-xs btn-primary btn-download" id="btDownload_' + full.trxInvoiceHeaderID + '"><i class="fa fa-download"></i></button>';
                        return '';
                    }
                },
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "CreatedBy" }
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
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': [6]
            }],
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 1 /* Set the 2 most left columns as fixed columns */
            },
            "order": []
        });

        $("#tblSummaryData tbody").unbind();

        $("#tblSummaryData tbody").on("click", "button.btn-download", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var row = table.row($(this).parents('tr')).data();
            Helper.Download(row.FilePath, row.InvReceiptFile, row.ContentType);
        });
    },
    Reset: function () {
        fsCustomerName = '';
        fsInvoiceTypeId = '';
        fsInvCompanyId = '';
        fsInvNo = '';
        fsReceiptDateFrom = '';
        fsReceiptDateTo = '';
        $("#slSearchCompany").val("").trigger("change");
        $("#slSearchTerm").val("").trigger("change");
        $("#tbSearchCustomerName").val("");
        $("#tbInvNo").val("");
        $("#tbSearchReceiptDateFrom").val("");
        $("#tbSearchReceiptDateTo").val("");
    },
    Export: function () {
        var href = "/InvoiceTransaction/TrxARProcessInvoiceBuildingHistory/Export?customerName=" + fsCustomerName + "&invoiceTypeId=" + fsInvoiceTypeId
            + "&invCompanyId=" + fsInvCompanyId + "&invNo=" + fsInvNo;

        if (fsReceiptDateFrom != null)
            href += "&receiptDateFrom=" + fsReceiptDateFrom;

        if (fsReceiptDateTo != null)
            href += "&receiptDateTo=" + fsReceiptDateTo;

        window.location.href = href;
    }
}

var Control = {
    BindingSelectInvoiceType: function () {
        $.ajax({
            url: "/api/MstDataSource/InvoiceType",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $("#slSearchTerm").html("<option value='0'>All</option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $("#slSearchTerm").append("<option value='" + item.mstInvoiceTypeId + "'>" + item.Description + "</option>");
                    })
                }

                $("#slSearchTerm").select2({ placeholder: "Select Term Invoice", width: null });

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchCompany").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchCompany").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
            }

            $("#slSearchCompany").select2({ placeholder: "Select Company", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
}

var Helper = {
    InitCurrencyInput: function (selector) {
        $(selector).unbind().on("blur", function () {
            var value = $(selector).val();
            if (value != "") {
                $(selector).val(Common.Format.CommaSeparation(value));
            } else {
                $(selector).val("0.00");
            }
        })
    },
    Download: function (filePath, invReceiptFile, contentType) {
        var docPath = $("#hfDocPath").val();
        var path = docPath + filePath;
        window.location.href = "/Admin/Download?path=" + path + "&fileName=" + invReceiptFile + "&contentType=" + contentType;
    },
    Print: function () {
        window.location.href = $("#hfURL").val();
    }
}