Data = {};

/* Helper Functions */

var fsOperatorId = "";
var fsCompanyId = "";
var fsInvoiceTypeId = "";
var fsInvNo = "";
var fsUserCompanyCode = "";

jQuery(document).ready(function () {
    // Modification Or Added By Ibnu Setiawan 04. September 2020
    // Add Company Code By User Login
    fsUserCompanyCode = $('#hdUserCompanyCode').val();
    // End Modification Or Added By Ibnu Setiawan 04. September 2020
    Helper.SetRole();
    Form.Init();
    Table.Init();

    if (fsUserCompanyCode.trim() == "PKP") {
        $("#slSearchCompany").val(fsUserCompanyCode).trigger("change");
    }

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
        if (fsUserCompanyCode.trim() == "PKP") {
            $("#slSearchCompany").val(fsUserCompanyCode).trigger("change");
        }
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
        if ($("#formCNInvoiceHead").parsley().validate())
            InvoiceTower.CNDeptHead();
        e.preventDefault();
        
    });

    $("#btApproveSPV").unbind().on("click", function () {
        if ($("#formCNInvoiceSPV").parsley().validate())
            InvoiceTower.CNSPV();
        e.preventDefault();
    });

    $("#btRejectHead").unbind().on("click", function () {
        InvoiceTower.RejectCN("Dept");
    });

    $("#btRejectSPV").unbind().on("click", function () {
        InvoiceTower.RejectCN("Section");
    });

    $("#slPICAType").unbind().on("change", function () {
        var PICATypeId = $(this).val();
        if (PICATypeId != null && PICATypeId != "") {
                Control.BindingPICADetail(PICATypeId);
        } else {
            Control.BindingPICADetail(0);
        }
    });
   
});

var Form = {
    Init: function () {
        $("#pnlTransaction").hide();
        $("#formTransaction").parsley();

        if (!$("#hdAllowProcess").val()) {
            $("#btSubmit").hide();
        }

        Table.Reset();
        // Memindahkan Binding Dropdown yang sebelum nya ada di akhir Page Load ke Form Init Karena kebutuhan Searching Data Awal
        Control.BindingSelectInvoiceType();
        Control.BindingSelectOperator();
        Control.BindingSelectCompany();
        Control.BindingPICAType();
        // Modification Or Added By Ibnu Setiawan 09. September 2020
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
        $("#panelTransactionTitle").text("CN Invoice Tower");
        $("#formTransaction").parsley().reset()

        $("#tbInvoiceNo").val(Data.Selected.InvNo);
        $("#tbInvoiceDate").val(Common.Format.ConvertJSONDateTime(Data.Selected.InvPrintDate));
        $("#tbCNApprovalDate").val(CNApprovalDate);
        $("#tbAmount").val(Common.Format.CommaSeparation(Data.Selected.InvSumADPP));
        $("#tbAmountPPN").val(Common.Format.CommaSeparation(Data.Selected.InvTotalAPPN));
        $("#tbCompanyTBG").val(Data.Selected.CompanyTBG);
        $("#tbOperator").val(Data.Selected.OperatorDesc);
        $("#tbPICAType").val(Data.Selected.PICAType);
        $("#tbPICAMajor").val(Data.Selected.PICAMajor);
        $("#tbPICADetail").val(Data.Selected.PICADetail);
        $("#taRemark").val(Data.Selected.Remark);
        $(".tbPICATypeCollection").val(Data.Selected.PICAType);
        $(".tbPICADetailCollection").val(Data.Selected.PICADetail);
        $(".tbPICATypeActual").val(Data.Selected.PicaTypeSec);
        $(".tbPICADetailActual").val(Data.Selected.PicaDetailSec);

        $("#slPICAType").val("").trigger("change");
            $("#slPICADetail").val("").trigger("change");
        
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
            $("#formCNInvoiceHead").parsley();
        } else if (Data.Selected.mstInvoiceStatusId == InvoiceStatus.StateWaitingSPVApproval) {
            $("#tabSPV").show();
            $("#tabHead").hide();
            $("#btApproveHead").attr("disabled", "disabled");
            $("#btApproveSPV").removeAttr("disabled");
            $("#btRejectHead").attr("disabled", "disabled");
            $("#btRejectSPV").removeAttr("disabled");
            $("#formCNInvoiceSPV").parsley();
        } else {
            $("#tabSPV").hide();
            $("#tabHead").show();
            $("#btApproveHead").attr("disabled", "disabled");
            $("#btApproveSPV").attr("disabled", "disabld");
            $("#btRejectHead").attr("disabled", "disabled");
            $("#btRejectSPV").attr("disabled", "disabld");
        }
        $("#mdlCNInvoice").modal("show");
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
                "url": "/api/ApprovalCNInvoiceTower/grid",
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
                { data: "Source" },
                { data: "InvTemp" },
                { data: "PICADetail" },
                { data: "Remark" },
                { data: "PicaDetailSec" },
                { data: "InvCompanyId" },
                { data: "InvOperatorID" },
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
        var href = "/InvoiceTransaction/TrxApprovalCNInvoiceTower/Export?companyId=" + fsCompanyId + "&invoiceTypeId=" + fsInvoiceTypeId + "&operatorId=" + fsOperatorId + "&invNo=" + fsInvNo;

        window.location.href = href;
    },
    Print: function (trxInvoiceHeaderID, mstInvoiceCategoryId, isApproved) {
        window.location.href = "/InvoiceTransaction/TrxPrintCNInvoiceTower/Print?trxCNInvoiceHeaderID=" + trxInvoiceHeaderID + "&mstInvoiceCategoryId="+ mstInvoiceCategoryId +"&isApproved=" + isApproved;
    }
}

var InvoiceTower = {
    CNSPV: function () {
        var params = {
            trxInvoiceHeaderID: Data.Selected.trxInvoiceHeaderID,
            mstInvoiceCategoryId: Data.Selected.mstInvoiceCategoryId,
            mstPICATypeIDSection : $("#slPICAType").val(),
            mstPICADetailIDSection: $("#slPICADetail").val()
        };

        var l = Ladda.create(document.querySelector("#btApproveSPV"))
        $.ajax({
            url: "/api/ApprovalCNInvoiceTower/ApprovalCNSPV",
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
               // Table.Reset();
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
            vSource: Data.Selected.Source
        };

        var l = Ladda.create(document.querySelector("#btApproveHead"))
        $.ajax({
            url: "/api/ApprovalCNInvoiceTower/ApprovalCNDeptHead",
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
            } //else {
            //    FormCNInvoice.Cancel();
            //}
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            //FormCNInvoice.Cancel();
            //FormCNInvoice.Done();
            Common.Alert.Error(errorThrown);
            l.stop();
        });
    },
    RejectCN: function (RejectRole) {
        var params = {
            trxInvoiceHeaderID: Data.Selected.trxInvoiceHeaderID,
            mstInvoiceCategoryId: Data.Selected.mstInvoiceCategoryId,
            RejectRole : RejectRole
        };
        var l;
        if (RejectRole == "Dept")
            l = Ladda.create(document.querySelector("#btApproveHead"))
        else
            l = Ladda.create(document.querySelector("#btApproveSPV"))
        $.ajax({
            url: "/api/ApprovalCNInvoiceTower/RejectCN",
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
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
            $("#mdlCNInvoice").modal("hide");
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
            type: "GET",
            async: false
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
    BindingPICAType: function () {
        $.ajax({
            url: "/api/MstDataSource/PICAType",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $("#slPICAType").html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $("#slPICAType").append("<option value='" + item.mstPICATypeID + "'>" + item.Description + "</option>");
                    })
                }

                $("#slPICAType").select2({ placeholder: "Select PICA Type", width: null });

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingPICADetail: function (PICATypeID, selectedValue) {
        $.ajax({
            url: "/api/MstDataSource/PICADetail?PICATypeId=" + PICATypeID,
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $("#slPICADetail").html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $("#slPICADetail").append("<option value='" + item.mstPICADetailID + "'>" + item.Description + "</option>");
                    })
                }

                $("#slPICADetail").select2({ placeholder: "Select PICA Detail", width: null });

                if (selectedValue != undefined) {
                    $("#slPICADetail").val(selectedValue).trigger("change");
                }
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
    SetRole: function () {
        Data.Role = $("#hdRole").val();
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