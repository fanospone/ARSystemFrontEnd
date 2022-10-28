Data = {};

jQuery(document).ready(function () {
    Form.Init();

    $("#btSearch").unbind().click(function () {
        Table.Search();
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
    });
});

var Form = {
    Init: function () {
        Control.BindingSelectCompany();
        Control.BindingSelectInvoiceType();
        Control.BindingSelectOperator();
        Control.BindingSelectBapsType();
        Table.Init();
    }
}

var Control = {
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchCompanyName").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchCompanyName").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
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
                })
            }

            $("#slSearchOperator").select2({ placeholder: "Select Operator", width: null });

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
    },
    BindingSelectBapsType: function () {
        $.ajax({
            url: "/api/MstDataSource/BapsType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchBapsType").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchBapsType").append("<option value='" + item.BapsType + "'>" + item.BapsType + "</option>");
                })
            }

            $("#slSearchBapsType").select2({ placeholder: "Select BAPS Type", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
}

var Table = {
    Init: function () {
        var tblRaw = $('#tblRaw').DataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblRaw").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {

        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsInvoiceType = $("#slSearchTermInvoice").val() == null ? "" : $("#slSearchTermInvoice").val();
        fsPONumber = $("#tbSearchPONumber").val();
        fsBAPSNumber = $("#tbSearchBAPSNumber").val();
        fsSONumber = $("#tbSearchSONumber").val();
        fsSiteIdOld = $("#tbSearchSiteIdOld").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        fsTransactionType = $("#slSearchTransactionType").val() == null ? "" : $("#slSearchTransactionType").val();

        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strInvoiceType: fsInvoiceType,
            strPONumber: fsPONumber,
            strBAPSNumber: fsBAPSNumber,
            strSONumber: fsSONumber,
            strBapsType: fsBapsType,
            strSiteIdOld: fsSiteIdOld,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            strCreatedBy: fsTransactionType
        };

        var tblRaw = $("#tblRaw").DataTable({
            "scrollY": 300,
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
            "scroller": true,
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/Dashboard/ReconcileHistory",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            "lengthMenu": [[-1], ['All']],
            "columns": [
                { data: "SoNumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CompanyInvoice" },
                { data: "CustomerInvoice" },

                { data: "Term" },
                {
                    data: "StartInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InflationAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AdditionalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "DeductionAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "PenaltySlaAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "BOQNumber" },
                { data: "PONumber" },
                { data: "BAPSNumber" },
                { data: "ActivityName" },
            ],
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
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "destroy": true,
            "fnDrawCallback": function (oSettings, json) {

            },
            "footerCallback": function (row, data, start, end, display) {

            },
        });

        $(window).resize(function () {
            $("#tblRaw").DataTable().columns.adjust().draw();
        });
    },
    Export: function () {
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsInvoiceType = $("#slSearchTermInvoice").val() == null ? "" : $("#slSearchTermInvoice").val();
        fsPONumber = $("#tbSearchPONumber").val();
        fsBAPSNumber = $("#tbSearchBAPSNumber").val();
        fsSONumber = $("#tbSearchSONumber").val();
        fsSiteIdOld = $("#tbSearchSiteIdOld").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        fsTransactionType = $("#slSearchTransactionType").val() == null ? "" : $("#slSearchTransactionType").val();

        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strInvoiceType: fsInvoiceType,
            strPONumber: fsPONumber,
            strBAPSNumber: fsBAPSNumber,
            strSONumber: fsSONumber,
            strBapsType: fsBapsType,
            strSiteIdOld: fsSiteIdOld,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            strCreatedBy: fsTransactionType
        };

        window.location.href = "/Dashboard/ReconcileHistory/Export?strCompanyId=" + fsCompanyId + "&strOperator=" + fsOperator
            + "&strInvoiceType=" + fsInvoiceType + "&strPONumber=" + fsPONumber + "&strBAPSNumber=" + fsBAPSNumber + "&strSONumber=" + fsSONumber
        + "&strBapsType=" + fsBapsType + "&strSiteIdOld=" + fsSiteIdOld + "&strStartPeriod=" + fsStartPeriod + "&strEndPeriod=" + fsEndPeriod + "&strCreatedBy=" + fsTransactionType;
    },
    Reset: function () {
        $("#slSearchCompanyName").val("").trigger('change');
        $("#slSearchOperator").val("").trigger('change');
        $("#slSearchTermInvoice").val("").trigger('change');
        $("#slSearchCurrency").val("").trigger('change');
        $("#tbSearchPONumber").val("");
        $("#tbSearchBAPSNumber").val("");
        $("#tbSearchSONumber").val("");
        $("#tbSearchSiteIdOld").val("");
        $("#slSearchBapsType").val("").trigger('change');
        $("#tbStartPeriod").val("");
        $("#tbEndPeriod").val("");
        fsCompanyId = "";
        fsOperator = "";
        fsStatusBAPS = "";
        fsPeriodInvoice = "";
        fsInvoiceType = "";
        fsCurrency = "";
        fsPONumber = "";
        fsBAPSNumber = "";
        fsSONumber = "";
        fsBapsType = "";
        fsSiteIdOld = "";
        fsStartPeriod = "";
        fsEndPeriod = "";
    }
}