Data = {};

/* Helper Functions */

var fsOperatorId = "";
var fsCompanyId = "";
var fsInvoiceTypeId = "";
var fsInvNo = "";

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

    $("#formCNInvoiceHead .btCancel").unbind().on("click", function (e) {
        FormCNInvoice.Cancel();
    });

    $("#formCNInvoiceSPV .btCancel").unbind().on("click", function (e) {
        FormCNInvoice.Cancel();
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
        var isFormValid = $("#formCNInvoiceHead").parsley().validate();
        if (isFormValid == true) {
            var validationResult = Helper.ValidateUser("Head");
            $("#lblHeadError").text(validationResult.Result);
            if (validationResult.Result == "") {
                InvoiceTower.CNDeptHead();
            }
        }
    });

    $("#btApproveSPV").unbind().on("click", function () {
        var isFormValid = $("#formCNInvoiceSPV").parsley().validate();
        if (isFormValid == true) {
            var validationResult = Helper.ValidateUser("SPV");
            $("#lblSPVError").text(validationResult.Result);
            if (validationResult.Result == "") {
                InvoiceTower.CNSPV();
            }
        }
    });

    Control.BindingSelectInvoiceType();
    Control.BindingSelectOperator();
    Control.BindingSelectCompany();
});

var Form = {
    Init: function () {
        $("#pnlTransaction").hide();
        $("#formTransaction").parsley();

        if (!$("#hdAllowProcess").val() || (Data.Role != Role.ARData)) {
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
        $("#tbCNRequestDate").val(emptyString);
        $("#tbAmount").val(zero);
        $("#tbAmountPPN").val(zero);
        $("#tbCompanyTBG").val(emptyString);
        $("#tbCompanyType").val(emptyString);
        $("#tbPICAType").val(emptyString);
        $("#tbPICAMajor").val(emptyString);
        $("#tbPICADetail").val(emptyString);
        $("#taRemark").val(emptyString);
    },
    SelectedData: function () {
        Data.Mode = "CN Invoice";

        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("CN Invoice Tower");
        $("#formTransaction").parsley().reset()

        $("#tbInvoiceNo").val(Data.Selected.InvNo);
        $("#tbInvoiceDate").val(Common.Format.ConvertJSONDateTime(Data.Selected.InvPrintDate));
        $("#tbCNRequestDate").val(Common.Format.ConvertJSONDateTime(Data.Selected.CNRequestDate));
        $("#tbAmount").val(Common.Format.CommaSeparation(Data.Selected.InvSumADPP));
        $("#tbAmountPPN").val(Common.Format.CommaSeparation(Data.Selected.InvTotalAPPN));
        $("#tbCompanyTBG").val(Data.Selected.CompanyTBG);
        $("#tbOperator").val(Data.Selected.OperatorDesc);
        $("#tbPICAType").val(Data.Selected.PICAType);
        $("#tbPICAMajor").val(Data.Selected.PICAMajor);
        $("#tbPICADetail").val(Data.Selected.PICADetail);
        $("#taRemark").val(Data.Selected.Remark);
    },
}

var FormCNInvoice = {
    Init: function () {
        $("#formCNInvoiceSPV").parsley();
        $("#formCNInvoiceHead").parsley();

        if (Data.Selected.mstInvoiceStatusId == InvoiceStatus.StateWaitingDeptHeadApproval) {
            $("#tbUserIDSPV").attr("disabled", "disabled");
            $("#tbPasswordSPV").attr("disabled", "disabled");
            $("#tbUserIDHead").removeAttr("disabled");
            $("#tbPasswordHead").removeAttr("disabled");
            $("#btApproveHead").removeAttr("disabled");
            $("#btApproveSPV").attr("disabled", "disabled");
        } else if (Data.Selected.mstInvoiceStatusId == InvoiceStatus.StateWaitingSPVApproval) {
            $("#tbUserIDSPV").removeAttr("disabled");
            $("#tbPasswordSPV").removeAttr("disabled");
            $("#tbUserIDHead").attr("disabled", "disabled");
            $("#tbPasswordHead").attr("disabled", "disabled");
            $("#btApproveHead").attr("disabled", "disabled");
            $("#btApproveSPV").removeAttr("disabled");
        } else {
            $("#tbUserIDSPV").attr("disabled", "disabled");
            $("#tbPasswordSPV").attr("disabled", "disabled");
            $("#tbUserIDHead").attr("disabled", "disabled");
            $("#tbPasswordHead").attr("disabled", "disabled");
            $("#btApproveHead").attr("disabled", "disabled");
            $("#btApproveSPV").attr("disabled", "disabld");
        }

        if (!$("#hdAllowProcess").val() || (Data.Role != Role.ARData)) {
            $("#btApproveSPV").hide();
            $("#btApproveHead").hide();
            $("#tbUserIDSPV").attr("disabled", "disabled");
            $("#tbPasswordSPV").attr("disabled", "disabled");
            $("#tbUserIDHead").attr("disabled", "disabled");
            $("#tbPasswordHead").attr("disabled", "disabled");
        }

        $("#tbUserIDSPV").val("");
        $("#tbPasswordSPV").val("");
        $("#lblSPVError").text("");
        $("#tbUserIDHead").val("");
        $("#tbPasswordHead").val("");
        $("#lblHeadError").text("");

        if (Data.Selected.SPVApproval != undefined && Data.Selected.SPVApproval != null && Data.Selected.SPVApproval.length > 0) {
            TableApprovalSPV.Search(Data.Selected.SPVApproval);
            $("#divApprovalSPVHistory").show();
        }
        else
            $("#divApprovalSPVHistory").hide();

        if (Data.Selected.DeptHeadApproval != undefined && Data.Selected.DeptHeadApproval != null && Data.Selected.DeptHeadApproval.length > 0) {
            TableApprovalHead.Search(Data.Selected.DeptHeadApproval);
            $("#divApprovalHeadHistory").show();
        }
        else
            $("#divApprovalHeadHistory").hide();

        $("#mdlCNInvoice").modal("show");
        Helper.OpenTab("tabSPV");

    },
    Cancel: function () {
        console.log("Modal Closed");
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

        fsOperatorId = ($("#slSearchOperator").val() == null) ? "" : $("#slSearchOperator").val();
        fsInvoiceTypeId = ($("#slSearchTerm").val() == null) ? "" : $("#slSearchTerm").val();
        fsCompanyId = ($("#slSearchCompany").val() == null) ? "" : $("#slSearchCompany").val();
        fsInvNo = $("#tbInvNo").val();

        var params = {
            companyId: fsCompanyId,
            operatorId: fsOperatorId,
            invoiceTypeId: fsInvoiceTypeId,
            invNo: fsInvNo
        };

        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/CNInvoiceTower/grid",
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
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<button type='button' title='Re-Print Note' class='btn blue btn-xs btPrint'>Re-Print Note</button>";

                        return strReturn;
                    }
                },
                { data: "InvNo" },
                { data: "InvTemp" },
                { data: "CompanyTBG" },
                { data: "OperatorDesc" },
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
            Table.Print(Data.Selected.trxInvoiceHeaderID, Data.Selected.mstInvoiceCategoryId, isApproved);
        });
    },
    Reset: function () {
        fsCompanyId = '';
        fsInvoiceTypeId = '';
        fsOperatorId = '';
        fsInvNo = '';
        $("#slSearchCompany").val("").trigger("change");
        $("#slSearchTerm").val("").trigger("change");
        $("#slSearchOperator").val("").trigger("change");
        $("#tbInvNo").val("");
    },
    Export: function () {
        var href = "/InvoiceTransaction/TrxCNInvoiceTower/Export?companyId=" + fsCompanyId + "&invoiceTypeId=" + fsInvoiceTypeId + "&operatorId=" + fsOperatorId + "&invNo=" + fsInvNo;

        window.location.href = href;
    },
    Print: function (trxInvoiceHeaderID, mstInvoiceCategoryId, isApproved) {
        window.location.href = "/InvoiceTransaction/TrxPrintCNInvoiceTower/Print?trxCNInvoiceHeaderID=" + trxInvoiceHeaderID + "&mstInvoiceCategoryId="+ mstInvoiceCategoryId +"&isApproved=" + isApproved;
    }
}

var TableApprovalSPV = {
    Search: function (data) {
        var tblApprovalSPVHistory = $("#tblApprovalSPVHistory").DataTable({
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table",
            },
            data: data,
            buttons: [],
            "filter": false,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    data: "CreatedDate", render: function (data, type, full) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "CreatedBy" },
                { data: "AppStatus" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': []
            }],
            "order": [[0, "asc"]]
        });
    }
}

var TableApprovalHead = {
    Search: function (data) {
        var tblApprovalHeadHistory = $("#tblApprovalHeadHistory").DataTable({
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table",
            },
            data: data,
            buttons: [],
            "filter": false,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    data: "CreatedDate", render: function (data, type, full) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "CreatedBy" },
                { data: "AppStatus" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': []
            }],
            "order": [[0, "asc"]]
        });
    }
}

var InvoiceTower = {
    CNSPV: function () {
        var params = {
            trxInvoiceHeaderID: Data.Selected.trxInvoiceHeaderID,
            mstInvoiceCategoryId: Data.Selected.mstInvoiceCategoryId,
            userID: $("#tbUserIDSPV").val()
        };

        var l = Ladda.create(document.querySelector("#btApproveSPV"))
        $.ajax({
            url: "/api/CNInvoiceTower/CNSPV",
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
                Common.Alert.Success("CN Invoice " + Data.Selected.InvNo + " has been approved!");
                Table.Reset();
                FormCNInvoice.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
            $("#mdlCNInvoice").modal("hide");
        })
    },
    CNDeptHead: function () {
        var params = {
            trxInvoiceHeaderID: Data.Selected.trxInvoiceHeaderID,
            mstInvoiceCategoryId: Data.Selected.mstInvoiceCategoryId,
            userID: $("#tbUserIDHead").val()
        };

        var l = Ladda.create(document.querySelector("#btApproveHead"))
        $.ajax({
            url: "/api/CNInvoiceTower/CNDeptHead",
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
                Table.Reset();
                FormCNInvoice.Done();
            } else {
                FormCNInvoice.Cancel();
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            FormCNInvoice.Cancel();
            Common.Alert.Error(errorThrown);
            l.stop();
        });
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
        else if (role == "Head")
            tempRole = "DEPT HEAD";

        var params = {
            UserID: $("#tbUserID" + role).val(),
            Password: $("#tbPassword" + role).val(),
            Role: tempRole
        };

        $.ajax({
            url: "/api/CNInvoiceBuilding/ValidateUser",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            async: false,
            cache: false
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
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