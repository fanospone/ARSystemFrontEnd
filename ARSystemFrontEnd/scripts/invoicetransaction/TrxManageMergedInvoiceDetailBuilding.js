Data = {};

var fsCompanyId = "";
var fsCustomerName = "";
var fsInvNo = "";

/* Helper Functions */

jQuery(document).ready(function () {
    Data.RowSelected = [];
    Control.BindingSelectCompany();
    Table.Init();
    Table.Search();

    $("#formReprint").parsley();

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

        $("#tblSummaryData tbody").on("click", "button.btEdit", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
            }
        });

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsCustomerName = $("#tbSearchCustomerName").val();
        fsInvNo = $("#tbInvNo").val();
        var params = {
            companyId: fsCompanyId,
            customerName: fsCustomerName,
            InvNo: fsInvNo
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/ManageMergedInvoiceDetailBuilding/grid",
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
                { data: "InvParentNo" },
                { data: "Company" },
                { data: "CustomerName" },
                {
                    data: "InvSumADPP", className: "text-right", render: function (data, type, full) {
                        return Common.Format.CommaSeparation(data + full.Discount);
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
                { data: "Currency" }
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

                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        $("#Row_" + item.trxInvoiceHeaderID).addClass("active");
                    }
                }
            },
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': []
            }],
            "order": [],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (aData.InvIsParent == 1) {
                    $('td', nRow).css('background-color', '#9ACD32');
                } else {
                    $('td', nRow).css('background-color', '#DEB887');
                }
                l.stop();
            },
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            }
        });
    },
    Reset: function () {
        fsInvNo = "";
        fsCustomerName = "";
        fsCompanyId = "";
        $("#tbInvNo").val("");
        $("#tbSearchCustomerName").val("");
        $("#slSearchCompany").val("").trigger("change");
    },
    Export: function () {
        window.location.href = "/InvoiceTransaction/TrxManageMergedInvoiceDetailBuilding/Export?customerName=" + fsCustomerName + "&companyId=" + fsCompanyId + "&invNo=" + fsInvNo;
    }
}

var Control = {
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
    }
}

var Helper = {
    RemoveObjectByIdFromArray: function (data, id) {
        var data = $.grep(data, function (e) {
            if (e.trxInvoiceHeaderID == id) {
                return false;
            } else {
                return true;
            }
        });

        return data;
    },
    DoesElementExistInArray: function (trxInvoiceHeaderID, arr) {
        var isExist = false;
        for (var i = 0 ; i < arr.length ; i++) {
            if (arr[i].trxInvoiceHeaderID == trxInvoiceHeaderID) {
                isExist = true;
                break;
            }
        }
        return isExist;
    },
    ClearRowSelected: function () {
        Data.RowSelected = [];
    }
}