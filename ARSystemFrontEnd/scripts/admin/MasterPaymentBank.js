Data = {};

/* Helper Functions */
var fsCompanyId = "";
var fsBankGroupId = "";
var fsAccountNo = "";

jQuery(document).ready(function () {
    Form.Init();
    Table.Init();

    Control.BindingSelectCompany();
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

    $("#btSearch").unbind().click(function () {
        Table.Search();
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
        if (PKPPurpose.Temp.UserCompanyCode == Constants.CompanyCode.PKP) {
            PKPPurpose.Filter.PKPOnly();
        }
    });

    $("#btCreate").unbind().click(function () {
        Form.Create();
        PKPPurpose.Set.UserCompanyCode();
        if (PKPPurpose.Temp.UserCompanyCode == Constants.CompanyCode.PKP) {
            PKPPurpose.Filter.PKPOnly();
        }
    });

    //panel transaction Header
    $("#formTransaction").submit(function (e) {
        if (Data.Mode == "Create")
            PaymentBank.Post();
        else if (Data.Mode == "Edit")
            PaymentBank.Put();
        e.preventDefault();
    });

    $(".btCancel").unbind().click(function () {
        Form.Cancel();
        Table.Reset();
        PKPPurpose.Set.UserCompanyCode();
        if (PKPPurpose.Temp.UserCompanyCode == Constants.CompanyCode.PKP) {
            PKPPurpose.Filter.PKPOnly();
        }
    });

    $("#btBack").unbind().click(function () {
        $("#pnlSummary").fadeIn();
        $("#pnlDetailResult").hide();
    });

    
    $("#slCurrency").select2({ placeholder: "Select Company", width: null });
});

var Form = {
    Init: function () {
        $("#pnlTransaction").hide();
        $("#formTransaction").parsley();

        if (!$("#hdAllowAdd").val()) {
            $("#btCreate").hide();
        }

        Table.Reset();
    },
    Create: function () {
        Data.Mode = "Create";
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Create Payment Bank");
        $("#formTransaction").parsley().reset()

        $("#slCompany").val("").trigger("change");
        $("#tbAccountId").val("");
        $("#tbAccountName").val("");
        $("#tbAccountNum").val("");
        $("#tbBankGroupId").val("");
        $("#taAddress").val("");
        $("#slCurrency").val("IDR").trigger("change");
        $("#chIsActive").bootstrapSwitch("state", true);
    },
    Edit: function () {
        Data.Mode = "Edit";
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Edit Payment Bank");
        $("#formTransaction").parsley().reset()

        $("#slCompany").val(Data.Selected.CompanyId).trigger("change");
        $("#tbAccountId").val(Data.Selected.AccountId);
        $("#tbAccountName").val(Data.Selected.AccountName);
        $("#tbAccountNum").val(Data.Selected.AccountNum);
        $("#tbBankGroupId").val(Data.Selected.BankGroupId);
        $("#taAddress").val(Data.Selected.Address);
        $("#slCurrency").val(Data.Selected.Currency).trigger("change");
        $("#chIsActive").bootstrapSwitch("state", Data.Selected.IsActive);
    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
        $(".panelSearchBegin").fadeIn();
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        Table.Search();
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

        $("#tblSummaryData tbody").on("click", "button.btEdit", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                Form.Edit();
            }
        });

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsBankGroupId = $("#tbSearchBankGroup").val();
        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsAccountNo = $("#tbSearchAccountNo").val();

        var params = {
            bankGroupId: fsBankGroupId,
            companyId: fsCompanyId,
            accountNo: fsAccountNo
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/MstPaymentBank/grid",
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
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        //strReturn += "<button type='button' title='Detail' class='btn green btDetail'><i class='fa fa-search-plus'></i></button>";
                        if ($("#hdAllowEdit").val())
                            strReturn += "<button type='button' title='Edit' class='btn btn-xs green btEdit'><i class='fa fa-edit'></i></button>";
                        return strReturn;
                    }
                },
                { data: "CompanyId" },
                { data: "AccountId" },
                { data: "AccountName" },
                { data: "AccountNum" },
                { data: "BankGroupId" },
                { data: "Currency" },
                { data: "Address" },
                {
                    data: "IsActive", mRender: function (data, type, full) {
                        return full.IsActive ? "Yes" : "No";
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
                    $(".panelSearchResult").fadeIn();
                }
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': [0]
            }],
            "order": [[1, "asc"]],
            "fixedColumns": {
                "leftColumns": 2
            },
            "scrollX": true,
            "scrollCollapse": true
        });
    },
    Reset: function () {
        fsAccountNo = "";
        fsBankGroupId = "";
        fsCompanyId = "";
        $("#tbSearchAccountNo").val("");
        $("#slSearchCompany").val("").trigger("change");
        $("#tbSearchBankGroup").val("");
    },
    Export: function () {
        window.location.href = "/Admin/PaymentBank/Export?accountNo=" + fsAccountNo + "&companyId=" + fsCompanyId + "&bankGroupId=" + fsBankGroupId;
    }
}

var PaymentBank = {
    Post: function () {
        var params = {
            CompanyId: $("#slCompany").val(),
            AccountId: $("#tbAccountId").val(),
            AccountName: $("#tbAccountName").val(),
            AccountNum: $("#tbAccountNum").val(),
            BankGroupId: $("#tbBankGroupId").val(),
            Address: $("#taAddress").val(),
            Currency: $("#slCurrency").val(),
            IsActive: $("#chIsActive").bootstrapSwitch("state")
        }

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/MstPaymentBank",
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
                Common.Alert.Success("Payment Bank has been created!");
                Table.Reset();
                Form.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    },

    Put: function () {
        var params = {
            CompanyId: $("#slCompany").val(),
            AccountId: $("#tbAccountId").val(),
            AccountName: $("#tbAccountName").val(),
            AccountNum: $("#tbAccountNum").val(),
            BankGroupId: $("#tbBankGroupId").val(),
            Address: $("#taAddress").val(),
            Currency: $("#slCurrency").val(),
            IsActive: $("#chIsActive").bootstrapSwitch("state")
        }

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/MstPaymentBank/" + Data.Selected.mstPaymentBankId,
            type: "PUT",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Payment Bank has been edited!");
                Table.Reset();
                Form.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
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
            $("#slSearchCompany").html("<option></option>");
            $("#slCompany").html("<option></option>");

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slCompany").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                    $("#slSearchCompany").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                });
            }

            $("#slSearchCompany").select2({ placeholder: "Select Company", width: null });
            $("#slCompany").select2({ placeholder: "Select Company", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
}

var Constants = {
    CompanyCode: {
        PKP: "PKP"
    }
}

var PKPPurpose = {
    Filter: {
        PKPOnly: function () {
            $('#slSearchCompany').val(Constants.CompanyCode.PKP).trigger('change');
            $('#slCompany').val(Constants.CompanyCode.PKP).trigger('change');
        },
    },
    Set: {
        UserCompanyCode: function () {
            PKPPurpose.Temp.UserCompanyCode = $('#hdUserCompanyCode').val();
        }
    },
    Temp: {
        UserCompanyCode:""
    }
}