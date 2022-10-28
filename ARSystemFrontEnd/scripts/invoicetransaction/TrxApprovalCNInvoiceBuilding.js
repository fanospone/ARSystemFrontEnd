Data = {};

/* Helper Functions */

var fsInvCompanyId = '';
var fsCustomerName = '';
var fsInvoiceTypeId = '';
var fsInvNo = '';

jQuery(document).ready(function () {
    Helper.SetRole();
    Form.Init();
    Table.Init();
    Table.Search();
    Data.RowSelected = [];

    $("#mdlCNInvoice").draggable({
        handle: ".modal-header"
    });

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

    $("#btSubmit").unbind().on("click", function (e) {
        FormCNInvoice.Init();
        e.preventDefault();
    });


    $("#formTransaction .btCancel").unbind().click(function () {
        Form.Cancel();
    });

    $("#btBack").unbind().click(function () {
        $("#pnlSummary").fadeIn();
        $("#pnlDetailResult").hide();
    });

    $("#slSearchOperator").select2();

    $("#btApproveHead").unbind().on("click", function () {
        InvoiceBuilding.CNDeptHead();
    });

    $("#btApproveSPV").unbind().on("click", function () {
        InvoiceBuilding.CNSPV();
    });
    $("#btRejectHead").unbind().on("click", function () {
        InvoiceBuilding.RejectCN("Dept");
    });

    $("#btRejectSPV").unbind().on("click", function () {
        InvoiceBuilding.RejectCN("Section");
    });
    Control.BindingSelectInvoiceType();
    Control.BindingSelectCompany();
});

var Form = {
    Init: function () {
        $("#pnlTransaction").hide();
        $("#formTransaction").parsley();

        //if (!$("#hdAllowProcess").val() || (Data.Role != Role.ARData)) {
        //    $("#btSubmit").hide();
        //} remarks by ens & KTE

        if (!$("#hdAllowProcess").val() ) {
            $("#btSubmit").hide();
        }


        Table.Reset();
    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
    },
    Clear: function () {
        var emptyString = "";
        var zero = "0.00";

        $("#tbInvoiceNo").val(emptyString);
        $("#tbInvoiceDate").val(emptyString);
        $("#tbCNApprovalDate").val(emptyString);
        $("#tbAmount").val(zero);
        $("#tbAmountPPN").val(zero);
        $("#tbCompanyTBG").val(emptyString);
        $("#tbCompanyType").val(emptyString);
        $("#tbCompanyName").val(emptyString);
        $("#tbPICAType").val(emptyString);
        $("#tbPICAMajor").val(emptyString);
        $("#tbPICADetail").val(emptyString);
        $("#taRemark").val(emptyString);
    },
    SelectedData: function () {
        Data.Mode = "CN Invoice";
        var CNApprovalDate = (Data.Selected.CNApprovalDate != null) ? Common.Format.ConvertJSONDateTime(Data.Selected.CNApprovalDate) : "";
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("CN Invoice Building");
        $("#formTransaction").parsley().reset()

        $("#tbInvoiceNo").val(Data.Selected.InvNo);
        $("#tbInvoiceDate").val(Common.Format.ConvertJSONDateTime(Data.Selected.InvPrintDate));
        $("#tbCNApprovalDate").val(CNApprovalDate);
        $("#tbAmount").val(Common.Format.CommaSeparation(Data.Selected.InvSumADPP));
        $("#tbAmountPPN").val(Common.Format.CommaSeparation(Data.Selected.InvTotalAPPN));
        $("#tbCompanyTBG").val(Data.Selected.CompanyTBG);
        $("#tbCompanyType").val(Data.Selected.CompanyType);
        $("#tbCompanyName").val(Data.Selected.Company);
        $("#tbPICAType").val(Data.Selected.PICAType);
        $("#tbPICAMajor").val(Data.Selected.PICAMajor);
        $("#tbPICADetail").val(Data.Selected.PICADetail);
        $("#taRemark").val(Data.Selected.Remark);
    },
}

var FormCNInvoice = {
    Init: function () {
        if (Data.Selected.mstInvoiceStatusId == InvoiceStatus.StateWaitingDeptHeadApproval) {
            $("#tabSPV").hide();
            $("#tabHead").show();
            $("#btApproveHead").removeAttr("disabled");
            $("#btApproveSPV").attr("disabled", "disabled");
            $("#btRejectHead").removeAttr("disabled");
            $("#btRejectSPV").attr("disabled", "disabled");
        } else if (Data.Selected.mstInvoiceStatusId == InvoiceStatus.StateWaitingSPVApproval) {
            $("#tabSPV").show();
            $("#tabHead").hide();
            $("#btApproveHead").attr("disabled", "disabled");
            $("#btApproveSPV").removeAttr("disabled");
            $("#btRejectHead").attr("disabled", "disabled");
            $("#btRejectSPV").removeAttr("disabled");
        } else {
            $("#tabSPV").hide();
            $("#tabHead").show();
            $("#btApproveHead").attr("disabled", "disabled");
            $("#btApproveSPV").attr("disabled", "disabld");
            $("#btRejectHead").attr("disabled", "disabled");
            $("#btRejectSPV").attr("disabled", "disabld");
        }

        //if (!$("#hdAllowProcess").val() || (Data.Role != Role.ARData)) {
        //    $("#btApproveSPV").hide();
        //    $("#btApproveHead").hide();
        //} remarks by ens & KTE
		if (!$("#hdAllowProcess").val() ) {
            $("#btApproveSPV").hide();
            $("#btApproveHead").hide();
        }
        $("#mdlCNInvoice").modal("show");
    },
    Cancel: function () {
        $("#mdlCNInvoice").modal("hide");
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
        $("#mdlCNInvoice").modal("hide");
        $(".panelSearchBegin").fadeIn();
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        Table.Search();
    },
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

        fsCustomerName = $("#tbSearchCompany").val();
        fsInvoiceTypeId = $("#slSearchTerm").val();
        fsInvCompanyId = $("#slSearchCompany").val();
        fsInvNo = $("#tbInvNo").val();

        var params = {
            companyName: fsCustomerName,
            invoiceTypeId: fsInvoiceTypeId,
            invCompanyId: fsInvCompanyId,
            invNo: fsInvNo
        };

        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/ApprovalCNInvoiceBuilding/grid",
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
                        strReturn += "<button type='button' title='Select' class='btn blue btn-xs btSelect'><i class='fa fa-mouse-pointer'></i></button>";

                        return strReturn;
                    }
                },
                { data: "InvNo" },
                { data: "InvTemp" },
                { data: "Company" },
                { data: "CompanyType" },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "PostedBy" },
                {
                    data: "PostingDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "Term" },
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
                { data: "TaxInvoiceNo" },
                { data: "ApprovalStatus" }
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
                'aTargets': [0]
            }],
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 3 /* Set the 2 most left columns as fixed columns */
            },
            "order": []
        });

        $("#tblSummaryData tbody").unbind();

        $("#tblSummaryData tbody").on("click", "button.btSelect", function (e) {
            var table = $("#tblSummaryData").DataTable();
            Data.Selected = {};
            Data.Selected = table.row($(this).parents('tr')).data();
            Form.SelectedData();
        });

        $("#tblSummaryData tbody").on("click", "button.btPrint", function (e) {
            var table = $("#tblSummaryData").DataTable();
            Data.Selected = {};
            Data.Selected = table.row($(this).parents('tr')).data();
            var isApproved = (Data.Selected.ApprovalStatus == "APPROVED") ? 1 : 0;
            Table.Print(Data.Selected.trxInvoiceHeaderID, isApproved);
        });
    },
    Reset: function () {
        fsCustomerName = '';
        fsInvCompanyId = '';
        fsInvoiceTypeId = '';
        fsInvNo = '';
        $("#tbSearchCompany").val("");
        $("#tbInvNo").val("");
        $("#slSearchTerm").val("").trigger("change");
        $("#slSearchCompany").val("").trigger("change");
    },
    Export: function () {
        var href = "/InvoiceTransaction/TrxApprovalCNInvoiceBuilding/Export?companyName=" + fsCustomerName + "&invoiceTypeId=" + fsInvoiceTypeId + "&invCompanyId=" + fsInvCompanyId + "&invNo=" + fsInvNo;

        window.location.href = href;
    },
    Print: function (trxInvoiceHeaderID, isApproved) {
        window.location.href = "/InvoiceTransaction/TrxPrintCNInvoiceBuilding/Print?trxCNInvoiceHeaderID=" + trxInvoiceHeaderID + "&isApproved=" + isApproved;
    }
}


var InvoiceBuilding = {
    CNSPV: function () {
        var params = {
            trxInvoiceHeaderID: Data.Selected.trxInvoiceHeaderID
        };

        var l = Ladda.create(document.querySelector("#btApproveSPV"))
        $.ajax({
            url: "/api/ApprovalCNInvoiceBuilding/ApprovalCNSPV",
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
                Common.Alert.Success("CN Invoice "+ Data.Selected.InvNo +" has been approved!");
                //Table.Reset();
                FormCNInvoice.Done();
            } else {
                FormCNInvoice.Cancel();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $("#mdlCNInvoice").modal("hide");
            Common.Alert.Error(errorThrown);
            l.stop();
        })
    },
    CNDeptHead: function () {
        var params = {
            trxInvoiceHeaderID: Data.Selected.trxInvoiceHeaderID
        };

        var l = Ladda.create(document.querySelector("#btApproveHead"))
        $.ajax({
            url: "/api/ApprovalCNInvoiceBuilding/ApprovalCNDeptHead",
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
                Common.Alert.Success("CN for Invoice " + Data.Selected.InvNo + " has been approved!");
                //Table.Reset();
                FormCNInvoice.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $("#mdlCNInvoice").modal("hide");
            Common.Alert.Error(errorThrown);
            l.stop();
        })
    },
    RejectCN: function (RejectRole) {
        var params = {
            trxInvoiceHeaderID: Data.Selected.trxInvoiceHeaderID,
            RejectRole: RejectRole
        };

        var l;
        if (RejectRole == "Dept")
            l = Ladda.create(document.querySelector("#btApproveHead"))
        else
            l = Ladda.create(document.querySelector("#btApproveSPV"))
        $.ajax({
            url: "/api/ApprovalCNInvoiceBuilding/RejectCN",
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
                Common.Alert.Success("CN Invoice " + Data.Selected.InvNo + " has been rejected!");
                //Table.Reset();
                FormCNInvoice.Done();
            } else {
                FormCNInvoice.Cancel();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $("#mdlCNInvoice").modal("hide");
            Common.Alert.Error(errorThrown);
            l.stop();
        })
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
    }
}

var Helper = {
    ReverseDateToSQLFormat: function (dateValue) {
        if (dateValue != "") {
            var dateComponents = dateValue.split("-");
            var dateValue = dateComponents[0];
            var monthValue = dateComponents[1];
            var yearValue = dateComponents[2];
            var allMonths = "JanFebMarAprMayJunJulAugSepOctNovDec";

            var monthNumberValue = allMonths.indexOf(monthValue) / 3 + 1;

            return yearValue + "-" + monthNumberValue + "-" + dateValue;
        }
        return "";
    },
    OpenTab: function (tabName) {
        $(".nav-tabs a[href='#" + tabName + "']").tab("show");
    },
    SetRole: function () {
        Data.Role = $("#hdRole").val();
    },
    ValidateUser: function (role) {
        var result = {};
        var tempRole = "";
        if (role == "SPV")
            tempRole = "SUPERVISOR";
        else if(role == "Head")
            tempRole = "DEPT HEAD";

        var params = {
            UserID: $("#tbUserID" + role).val(),
            Password: $("#tbPassword" + role).val(),
            Role: tempRole
        };

        $.ajax({
            url: "/api/ApprovalCNInvoiceBuilding/ValidateUser",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            async: false,
            cache: false
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                console.log(data);
                result = data;
            }
        });
        return result;
    }
}

var Role = {
    ARCollection: "AR COLLECTION",
    ARData: "AR DATA"
}

var InvoiceStatus = {
    StateWaitingDeptHeadApproval: 40,
    StateWaitingSPVApproval: 39
}