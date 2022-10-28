Data = {};

/* Filter Search */
var fsCompanyId = "";
var fsOperatorId = "";
var fsInvoiceTypeId = "";
var fsPostingDateFrom = "";
var fsPostingDateTo = "";
var fsStatus = "";
var fsInvNo = "";

/* Helper Functions */

jQuery(document).ready(function () {
    Data.Role = $("#hdUserRole").val();
    Form.Init();
    Table.Init();


    PKPPurpose.Set.UserCompanyCode();
    if (PKPPurpose.Temp.UserCompanyCode == Constants.CompanyCode.PKP) {
        PKPPurpose.Filter.PKPOnly();
    }
    Table.Search();
    Data.RowSelected = [];

    // Initialize Datepicker
    $("body").delegate(".datepicker", "focusin", function () {
        $(this).datepicker({
            format: "dd-M-yyyy"
        });
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
        if (PKPPurpose.Temp.UserCompanyCode == Constants.CompanyCode.PKP) {
            PKPPurpose.Filter.PKPOnly();
        }
    });

    $("#btSubmit").unbind().on("click", function (e) {
        if (Data.Mode == "Checklist") {
            if ($("#formTransaction").parsley().validate())
                InvoiceTower.Checklist();
        }
        e.preventDefault();
    });

    $(".btCancel").unbind().click(function () {
        Form.Cancel();
    });

    $("#btBack").unbind().click(function () {
        $("#pnlSummary").fadeIn();
        $("#pnlDetailResult").hide();
    });

    // Initialize Datepicker
    $("body").delegate(".datepicker", "focusin", function () {
        $(this).datepicker({
            format: "dd-M-yyyy"
        });
    });

    // Add the selected document when the checkboxes are checked
    $('#tblChecklistDocument').find('.group-checkable').change(function () {
        var set = jQuery(this).attr("data-set");
        var checked = jQuery(this).is(":checked");
        jQuery(set).each(function () {
            if (checked) {
                $(this).prop("checked", true);
                $(this).parents('tr').addClass('active');
                $(this).trigger("change");
            } else {
                $(this).prop("checked", false);
                $(this).parents('tr').removeClass("active");
                $(this).trigger("change");
            }
        });
    });

    $('#tblChecklistDocument').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        if (this.checked) {
            $(this).parents('tr').addClass("active");
        } else {
            $(this).parents('tr').removeClass("active");
        }
    });

    $("#btCancelTaxInvoiceNo").unbind().on('click', function () {
        $("#mdlEditTaxInvoiceNo");
    });

    $("#btSaveTaxInvoiceNo").unbind().on("click", function (e) {
        if ($("#formEditInvoiceNo").parsley().validate())
            InvoiceTower.UpdateTaxInvoiceNo();
        e.preventDefault();
    });

    $("#slStatus").unbind().on("change", function () {
        var status = $(this).val();
        $('#tblChecklistDocument tbody tr .collection-checkboxes').each(function () {
            var cb = $(this);
            if (status == "Approved")
                cb.prop("checked", true);
            //else
            //    cb.prop("checked", false);
            cb.trigger("change");
        });
    });

    $("#pnlChecklistHistory").hide();
    $("#pnlApproveChecklist").hide();

    $("#btCNInvoice").unbind().click(function () {
        if (Data.Selected.mstInvoiceCategoryId == 6)
            Common.Alert.Warning("Non-Revenue Invoice, Unable to CN Invoice!");
        else {
            if (Data.Selected.mstInvoiceStatusId != 58) //waiting for approval 
                Common.PanelCNARData.Reset();
            $('.mdlCNARData').modal('show');
        }
    });
    $("#btApproveCN").unbind().click(function () {
        InvoiceTower.ApproveCNInvoice();
    });
    $("#btRejectCN").unbind().click(function () {
        InvoiceTower.RejectCNInvoice();
    });
    $("#btYesCNARData").unbind().click(function () {
        InvoiceTower.CNInvoice();
    });

});

var Form = {
    Init: function () {
        $("#pnlTransaction").hide();
        $("#formTransaction").parsley();

        if (!$("#hdAllowProcess").val()) {
            $("#btSubmit").hide();
        }

        Control.BindingSelectCompany();
        Control.BindingSelectOperator();
        Control.BindingSelectInvoiceType();
        $("#slSearchStatus").select2({ placeholder: "Select Status", width: null });

        Table.Reset();
        if (Data.Role == Role.ARCollection) {
            $("#slSearchStatus").val(VerificationStatus.Pending).trigger("change");
        }
    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
        $("#pnlChecklistHistory").hide();
        $("#pnlApproveChecklist").hide();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
        $(".panelSearchBegin").fadeIn();
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        $("#thReceived").hide();
        $("#thRemark").hide();
        $("#pnlChecklistHistory").hide();
        $("#pnlApproveChecklist").hide();
        Table.Search();
    },
    Clear: function () {
        var emptyString = "";
        var zero = "0.00";
        $("#tbArea").val(zero);
        $("#tbTermPeriod").val(emptyString);
        $("#tbTotalPrice").val(zero);
        $("#tbPricePerArea").val(zero);
        $("#tbPricePerMonth").val(zero);
        $("#tbPPN").val(zero);
        $("#tbTotalAmount").val(zero);
        $("#slOperator").val(emptyString).trigger("change");
        $("#slStatus").val("Approved").trigger("change");
        $("#taRemark").val(emptyString);
    },
    SelectedData: function () {
        $(".group-checkable").removeAttr("checked");
        Data.Mode = "Checklist";
        Data.RowSelected = Data.Selected.DocInvoiceDetail;

        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Checklist Tower Invoice");
        $("#formTransaction").parsley().reset()

        $("#tbInvoiceNo").val(Data.Selected.InvNo);
        $("#tbInvoiceDate").val(Common.Format.ConvertJSONDateTime(Data.Selected.InvPrintDate));
        $("#tbCompanyName").val(Data.Selected.Company);
        $("#tbTermPeriod").val(Data.Selected.Term);
        $("#tbOperator").val(Data.Selected.Operator);
        $("#tbAmount").val(Data.Selected.Currency + ' ' + Common.Format.CommaSeparation(Data.Selected.InvTotalAmount));
        $("#slStatus").val("").trigger("change");
        $("#slOperator").val("").trigger("change");
        $("#taRemark").val(""); 
        $("#tbPICARemark").val(Data.Selected.PicaRemark);

        $("#tbTaxNumber").val(Data.Selected.TaxInvoiceNo);
        $("#tbDPP").val(Data.Selected.Currency + ' ' + Common.Format.CommaSeparation(Data.Selected.InvSumADPP));
        $("#tbPPN").val(Data.Selected.Currency + ' ' + Common.Format.CommaSeparation(Data.Selected.InvTotalAPPN));
        if (Data.Selected.InvTotalPenalty > 0) {
            $("#tbPenalty").val(Data.Selected.Currency + ' ' + Common.Format.CommaSeparation(Data.Selected.InvTotalPenalty));
            $("#divPenalty").show();
        }
        else
            $("#divPenalty").hide();

        if (Data.Selected.DocInvoiceDetail.length == Helper.GetNumberOfCheckedDocument(Data.Selected.DocInvoiceDetail)) {
            $("#checkAll").prop("checked", true);
        } else {
            $("#checkAll").prop("checked", false);
        }

        TableDocument.Init(Data.Selected.DocInvoiceDetail);
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

        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsOperatorId = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsInvoiceTypeId = $("#slSearchTerm").val() == null ? "" : $("#slSearchTerm").val();
        fsPostingDateFrom = Helper.ReverseDateToSQLFormat($("#tbSearchPostingDateFrom").val());
        fsPostingDateTo = Helper.ReverseDateToSQLFormat($("#tbSearchPostingDateTo").val());
        fsStatus = $("#slSearchStatus").val() == "0" ? "" : $("#slSearchStatus").val();
        fsInvNo = $("#tbInvNo").val();

        var params = {
            CompanyId: fsCompanyId,
            OperatorId: fsOperatorId,
            InvoiceTypeId: fsInvoiceTypeId,
            PostingDateFrom: fsPostingDateFrom,
            PostingDateTo: fsPostingDateTo,
            Status: fsStatus,
            InvNo: fsInvNo
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/ChecklistInvoiceTower/grid",
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
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvTemp" },
                { data: "PostedBy" },
                {
                    data: "PostedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "Term" },
                {
                    data: "VerificationStatus", render: function (data) {
                        if (data == "P")
                            return "Waiting for AR Collection Verification";
                        else if (data == "A")
                            return "Received by AR Collection ";
                        else if (data == "R")
                            return "Rejected by AR Collection";
                        else if (data == "N")
                            return "Printed by AR Data";
                        else if (data == "W")
                            return "Waiting Dept Head Approval";
                        else
                            return "";
                    }
                },
                { data: "Company" },
                { data: "Operator" },
                {
                    data: "InvSumADPP", className: "text-right", render: function (data, type, full) {
                        return Helper.FormatCurrency(data + full.Discount);
                    }
                },
                {
                    data: "Discount", className: "text-right", render: function (data) {
                        return Helper.FormatCurrency(data);
                    }
                },
                {
                    data: "InvTotalAPPN", className: "text-right", render: function (data) {
                        return Helper.FormatCurrency(data);
                    }
                },
                {
                    data: "InvTotalPenalty", className: "text-right", render: function (data) {
                        return Helper.FormatCurrency(data);
                    }
                },
                { data: "Currency" },
                //{
                //    orderable: false,
                //    mRender: function (data, type, full) {
                //        var strReturn = "";
                //        strReturn += "<button type='button' title='Edit Tax Invoice Number' class='btn blue btn-xs btEdit'><i class='fa fa-edit'></i></button>";

                //        return strReturn;
                //    }
                //},
                { data: "TaxInvoiceNo" },
                { data: "Remark" },
                {
                    data: "ChecklistARData", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "VerificationDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "InvoiceNonRevenue", mRender: function (data, type, full) {
                        return full.InvoiceNonRevenue ? "Yes" : "No";
                    }
                }
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
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
            "order": []
        });

        // Initialize events (for button clicks, etc.)
        $("#tblSummaryData tbody").unbind();

        $("#tblSummaryData tbody").on("click", "button.btSelect", function (e) {
            var table = $("#tblSummaryData").DataTable();
            Data.Selected = {};
            Data.Selected = table.row($(this).parents('tr')).data();
            var validationResult = Helper.ValidateChecklist(Data.Selected.trxInvoiceHeaderID, Data.Selected.mstInvoiceCategoryId);
            if (validationResult.Result != "") {
                Common.Alert.Warning(validationResult.Result);
            } else {
                Form.SelectedData();
            }
        });

        $("#tblSummaryData tbody").on("click", "button.btEdit", function (e) {
            var table = $("#tblSummaryData").DataTable();
            $('#mdlEditTaxInvoiceNo').modal('show');
            Data.Selected = {};
            Data.Selected = table.row($(this).parents('tr')).data();
            $("#tbMdlInvoiceNo").val(Data.Selected.InvNo);
            $("#tbMdlTaxInvoiceNo").val(Data.Selected.TaxInvoiceNo);
        });
        tblSummaryData.column(15).visible(Data.Role == Role.ARData);
    },
    Reset: function () {
        fsInvNo = "";
        fsCompanyId = "";
        fsOperatorId = "";
        fsPostingDateFrom = "";
        fsPostingDateTo = "";
        fsStatus = "";
        fsInvoiceTypeId = "";
        $("#tbInvNo").val("");
        $("#slSearchCompany").val("").trigger("change");
        $("#slSearchOperator").val("").trigger("change");
        $("#tbSearchPostingDateFrom").val("");
        $("#tbSearchPostingDateTo").val("");
        $("#slSearchTerm").val("").trigger("change");
        $("#slSearchStatus").val("-1").trigger("change");
    },
    Export: function () {
        var href = "/InvoiceTransaction/TrxChecklistInvoiceTower/Export?companyId=" + fsCompanyId + "&invoiceTypeId=" + fsInvoiceTypeId + "&status=" + fsStatus + "&operatorId=" + fsOperatorId + "&invNo=" + fsInvNo;

        if (fsPostingDateFrom != "")
            href = href + "&postingDateFrom=" + fsPostingDateFrom;

        if (fsPostingDateTo != "")
            href = href + "&postingDateTo=" + fsPostingDateTo;

        window.location.href = href;
    }
}

var TableDocument = {
    Init: function (data) {
        Helper.GetCurrentUserRole(data);
    }
}

var InvoiceTower = {
    Checklist: function () {
        var params = {
            trxInvoiceHeaderID: Data.Selected.trxInvoiceHeaderID,
            mstInvoiceCategoryId: Data.Selected.mstInvoiceCategoryId,
            TaxInvoiceNo: $("#tbTaxInvoice").val(),
            DocInvoiceDetail: Helper.GetAllChecklistDocument()
        };

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/ChecklistInvoiceTower",
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
                Common.Alert.Success("Checklist for Invoice " + Data.Selected.InvNo + " has been submitted!");
                //Table.Reset();
                Form.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    },

    ChecklistCollection: function () {
        var params = {
            trxInvoiceHeaderID: Data.Selected.trxInvoiceHeaderID,
            DocInvoiceDetail: Helper.GetAllChecklistDocument(),
            FormVerificationStatus: $("#slStatus").val(),
            FormRemark: $("#taRemark").val(),
            mstInvoiceCategoryId: Data.Selected.mstInvoiceCategoryId
        };

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/ChecklistInvoiceTower/Collection",
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
                Common.Alert.Success("Checklist for Invoice " + Data.Selected.InvNo + " has been " + $("#slStatus").val() + "!");
                //Table.Reset();
                Form.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    },

    UpdateTaxInvoiceNo: function () {
        var params = {
            trxInvoiceHeaderID: Data.Selected.trxInvoiceHeaderID,
            taxInvoiceNo: $("#tbMdlTaxInvoiceNo").val(),
            mstInvoiceCategoryId: Data.Selected.mstInvoiceCategoryId
        };

        var l = Ladda.create(document.querySelector("#btSaveTaxInvoiceNo"))
        $.ajax({
            url: "/api/ChecklistInvoiceTower",
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
                Common.Alert.Success("Tax Invoice Number has been updated!");
                //Table.Reset();
                Form.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })

        $("#mdlEditTaxInvoiceNo").modal("hide");
    },

    CNInvoice: function () {
        var l = Ladda.create(document.querySelector("#btYesCNARData"))
        var params = {
            dataChecklist: Data.Selected,
            strRemarksCancel: $("#tbRemarksARData").val(),
            mstPICATypeID: $("#slPicaTypeARData").val(),
            mstPICADetailID: $("#slPicaDetailARData").val()
        }
        $.ajax({
            url: "/api/ChecklistInvoiceTower/CNInvoice",
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
                Common.Alert.Success("Data Success Canceled, Please Wait for Approval..")
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            $('.mdlCNARData').modal('hide');
            Form.Done();
        })

    },
    ApproveCNInvoice: function () {
        var l = Ladda.create(document.querySelector("#btApproveCN"))
        var params = {
            dataChecklist: Data.Selected
        }


        $.ajax({
            url: "/api/ChecklistInvoiceTower/ApproveCNInvoice",
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
                Common.Alert.Success("Data Successfully Approved!");
                Notification.GetList();
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Form.Done();
        })

    },
    RejectCNInvoice: function () {
        var l = Ladda.create(document.querySelector("#btRejectCN"))
        var params = {
            dataChecklist: Data.Selected
        }


        $.ajax({
            url: "/api/ChecklistInvoiceTower/RejectCNInvoice",
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
                Common.Alert.Success("Data Successfully Rejected!");
                Notification.GetList();
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Form.Done();
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
            type: "GET",
            async:false
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
}

var Helper = {
    FormatCurrency: function (value) {
        if (isNaN(value))
            return "0.00";
        else
            return parseFloat(value, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
    },
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
    RemoveElementFromArray: function (arr) {
        var what, a = arguments, L = a.length, ax;
        while (L > 1 && arr.length) {
            what = a[--L];
            while ((ax = arr.indexOf(what)) !== -1) {
                arr.splice(ax, 1);
            }
        }
        return arr;
    },
    IsElementExistsInArray: function (value, arr) {
        var isExist = false;
        for (var i = 0 ; i < arr.length ; i++) {
            if (arr[i] == value) {
                isExist = true;
                break;
            }
        }
        return isExist;
    },
    RemoveObjectByIdFromArray: function (data, id) {
        var data = $.grep(data, function (e) {
            return e.trxDocInvoiceDetailID != id;
        });
        return data;
    },
    UpdateObjectInArray: function (arr, object) {
        var arr = $.grep(arr, function (e) {
            if (e.trxDocInvoiceDetailID == object.trxDocInvoiceDetailID) {
                e.IsChecked = object.IsChecked;
            }
            return true;
        });
        return arr;
    },
    GetNumberOfCheckedDocument: function (arr) {
        var n = 0;
        $.each(arr, function (index, item) {
            if (item.IsChecked)
                n++;
        });
        return n;
    },
    GetAllChecklistDocument: function () {
        var temp = [];
        var checked = false;
        var id = 0;
        var obj = new Object();
        var isReceived = false;
        var remark = "";
        jQuery("#tblChecklistDocument .checkboxes").each(function () {
            checked = $(this).prop("checked");
            id = $(this).parent().attr("id");
            obj = new Object();
            obj.IsChecked = checked;
            obj.trxDocInvoiceDetailID = id;
            if ($("#cb_" + id) != undefined) {
                isReceived = $("#cb_" + id).is(":checked");
                remark = $("#tb_" + id).val();
                obj.IsReceived = isReceived;
                obj.Remark = remark;
            }
            temp.push(obj);
        });
        return temp;
    },
    ValidateChecklist: function (trxInvoiceHeaderID, mstInvoiceCategoryId) {
        var Result = {};

        var params = {
            trxInvoiceHeaderID: trxInvoiceHeaderID,
            mstInvoiceCategoryId: mstInvoiceCategoryId
        }

        var l = Ladda.create(document.querySelector("#btSearch"));
        $.ajax({
            url: "/api/ChecklistInvoiceTower/GetValidateResultChecklist",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                Result = data;
            }
        })
        return Result;
    },
    GetCurrentUserRole: function (docs) {
        Helper.SetControlByRole(Data.Role, docs);
    },
    SetControlByRole: function (role, docs) {
        Helper.LoadDocuments(role, docs);
        Helper.SetFormControl(role);
    },
    LoadDocuments: function (role, docs) {
        var columns = [];
        if (role == Role.ARCollection || ((role == Role.ARData && Data.Selected.VerificationStatus != VerificationStatus.Pending) && (role == Role.ARData && Data.Selected.VerificationStatus != null))) {
            $("#thReceived").show();
            $("#thRemark").show();
            $("#divChecklistDocument").attr("class", "col-md-9");

            var readonly = "";
            var disabled = "";
            if ((role == Role.ARData) || (role == Role.ARCollection && Data.Selected.VerificationStatus != VerificationStatus.Pending)) {
                readonly = "readonly";
                disabled = "disabled";
            } else if (role == Role.ARCollection && Data.Selected.VerificationStatus == VerificationStatus.Pending) {
                $("#pnlApproveChecklist").show();
            }

            columns = [
                { data: "DocType" },
                { data: "DocName" },
                {
                    mRender: function (data, type, full) {
                        var checked = "";
                        if (full.IsChecked) {
                            checked = "checked";
                        }
                        return "<label id='" + full.trxDocInvoiceDetailID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input name='cbSelect' multiple " + checked + " type='checkbox' class='checkboxes' /><span></span></label>";
                    }
                },
                {
                    mRender: function (data, type, full) {
                        var checked = "";
                        if (full.IsReceived) {
                            checked = "checked";
                        }
                        return "<label id='collection_" + full.trxDocInvoiceDetailID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input name='cbReceive' " + disabled + " multiple " + checked + " id='cb_" + full.trxDocInvoiceDetailID + "' type='checkbox' class='collection-checkboxes' /><span></span></label>";
                    }
                },
                {
                    render: function (data, type, full) {
                        return "<input type='text' id='tb_" + full.trxDocInvoiceDetailID + "' class='form-control collection-textboxes' " + readonly + " value='" + (full.Remark == null ? "" : full.Remark) + "' />";
                    }
                },
            ];
        } else {
            $("#thReceived").hide();
            $("#thRemark").hide();

            $("#pnlApproveChecklist").hide();
            $("#divChecklistDocument").attr("class", "col-md-6");
            columns = [
                { data: "DocType" },
                { data: "DocName" },
                {
                    mRender: function (data, type, full) {
                        var checked = "";
                        if (full.IsChecked) {
                            checked = "checked";
                        }
                        return "<label id='" + full.trxDocInvoiceDetailID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input name='cbSelect' multiple " + checked + " type='checkbox' class='checkboxes' /><span></span></label>";
                    }
                },
                { mRender: function () { return ""; } },
                { mRender: function () { return ""; } }
            ];
        }

        var tblChecklistDocument = $("#tblChecklistDocument").DataTable({
            "language": {
                "emptyTable": "No data available in table",
            },
            "data": docs,
            "filter": false,
            "lengthMenu": [[-1], ['All']],
            "lengthChange": false,
            "destroy": true,
            "columns": columns,
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "buttons": [],
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': [2]
            }],
            "order": [[0, "asc"]],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                var isChecked = false;
                if (role == Role.ARCollection)
                    isChecked = aData.isReceived;
                else
                    isChecked = aData.isChecked;

                if (isChecked) {
                    $(nRow).addClass('active');
                } else {
                    $(nRow).removeClass('active');
                }
            }
        });

        if (role == Role.ARCollection && Data.Selected.VerificationStatus == VerificationStatus.Pending) {
            $("#tblChecklistDocument tbody tr .collection-textboxes").each(function () {
                $(this).attr("required", "required");
            });
        }

        $('#tblChecklistDocument').on('change', 'tbody tr .collection-checkboxes', function () {
            var id = $(this).parent().attr('id').substr(("collection_").length);
            if (this.checked) {
                $("#tb_" + id).removeAttr("required");
                $(this).parents('tr').addClass("active");
            } else {
                $("#tb_" + id).prop("required", "required");
                $(this).parents('tr').removeClass("active");
            }
        });

        if ((role == Role.ARData && Data.Selected.VerificationStatus == null) || role == Role.ARData && Data.Selected.VerificationStatus == VerificationStatus.Pending || (role != Role.ARData && role != Role.ARCollection)) {
            tblChecklistDocument.column(3).visible(false);
            tblChecklistDocument.column(4).visible(false);
        } else {
            tblChecklistDocument.column(3).visible(true);
            tblChecklistDocument.column(4).visible(true);
        }
    },
    SetFormControl: function (role) {
        if (role == "DEPT HEAD") {
            $("#pnlChecklistHistory").hide();
            $("#checkAll").attr("disabled", "disabled");
            $(".checkboxes").each(function () {
                $(this).attr("disabled", "disabled");
            });
            if (Data.Selected.VerificationStatus == VerificationStatus.Waiting) {
                $("#btSubmit").hide();
                $("#btCNInvoice").hide();
                $("#slOperator").attr("disabled", "disabled");
                $("#slStatus").attr("disabled", "disabled");
                $("#taReport").attr("disabled", "disabled");
                $(".collection-checkboxes").each(function () {
                    $(this).attr("disabled", "disabled");
                });
                $("#btApproveCN").show();
                $("#btRejectCN").show();
                $(".divPICARemark").show();
                $(".divChecklistDocumentCtrl").hide();                
            }
            else {
                $("#btSubmit").hide();
                $("#btCNInvoice").hide();
                $("#slOperator").attr("disabled", "disabled");
                $("#slStatus").attr("disabled", "disabled");
                $("#taReport").attr("disabled", "disabled");
                $(".collection-checkboxes").each(function () {
                    $(this).attr("disabled", "disabled");
                });
                $("#btApproveCN").hide();
                $("#btRejectCN").hide();
                $(".divPICARemark").hide();
                $(".divChecklistDocumentCtrl").show();
            }
        }
        else if (role != Role.ARCollection) {
            $("#pnlChecklistHistory").hide();
            if (role == Role.ARData && Data.Selected.VerificationStatus == VerificationStatus.Rejected) {
                $(".collection-checkboxes").each(function () {
                    $(this).attr("disabled", "disabled");
                });

                $(".collection-textboxes").each(function () {
                    $(this).attr("readonly", "readonly");
                });
            }

            if (Data.Selected.VerificationStatus == VerificationStatus.Pending) {
                $("#btSubmit").hide();
                $("#btCNInvoice").hide();
                $("#checkAll").attr("disabled", "disabled");
                $(".checkboxes").each(function () {
                    $(this).attr("disabled", "disabled");
                });
                $("#btApproveCN").hide();
                $("#btRejectCN").hide();

            } else if (Data.Selected.VerificationStatus == VerificationStatus.Approved) {
                $("#btSubmit").hide();
                $("#btCNInvoice").hide();
                $("#checkAll").attr("disabled", "disabled");
                $(".checkboxes").each(function () {
                    $(this).attr("disabled", "disabled");
                });
                $("#btApproveCN").hide();
                $("#btRejectCN").hide();
            } else if (Data.Selected.VerificationStatus == VerificationStatus.New) {
                $("#btSubmit").show();
                $("#btCNInvoice").show();
                $("#checkAll").removeAttr("disabled");
                $(".checkboxes").each(function () {
                    $(this).removeAttr("disabled");
                });
                $("#btApproveCN").hide();
                $("#btRejectCN").hide();
            }
            else {
                $("#btSubmit").show();
                $("#btCNInvoice").hide();
                $("#checkAll").removeAttr("disabled");
                $(".checkboxes").each(function () {
                    $(this).removeAttr("disabled");
                });
                $("#btApproveCN").hide();
                $("#btRejectCN").hide();
            }
            $("#slStatus").removeAttr("required");
            $(".divPICARemark").hide();
            $(".divChecklistDocumentCtrl").show();
        } else {
            $("#pnlChecklistHistory").show();
            $("#checkAll").attr("disabled", "disabled");
            $(".checkboxes").each(function () {
                $(this).attr("disabled", "disabled");
            });

            TableChecklistHistory.Init(Data.Selected.ChecklistHistory);
            $("#slStatus").select2({ placeholder: "Select Status", width: null });

            if (Data.Selected.VerificationStatus == VerificationStatus.Pending) {
                $("#btSubmit").show();
                $("#btCNInvoice").hide();
                $("#slOperator").removeAttr("disabled");
                $("#slStatus").removeAttr("disabled");
                $("#taReport").removeAttr("disabled");
                $(".collection-checkboxes").each(function () {
                    $(this).removeAttr("disabled");
                });
                $("#btApproveCN").hide();
                $("#btRejectCN").hide();
            }
            else if (Data.Selected.VerificationStatus == VerificationStatus.Waiting) {
                $("#btSubmit").hide();
                $("#btCNInvoice").hide();
                $("#slOperator").attr("disabled", "disabled");
                $("#slStatus").attr("disabled", "disabled");
                $("#taReport").attr("disabled", "disabled");
                $(".collection-checkboxes").each(function () {
                    $(this).attr("disabled", "disabled");
                });
                $("#btApproveCN").show();
                $("#btRejectCN").show();
            }
            else {
                $("#btSubmit").hide();
                $("#btCNInvoice").hide();
                $("#slOperator").attr("disabled", "disabled");
                $("#slStatus").attr("disabled", "disabled");
                $("#taReport").attr("disabled", "disabled");
                $(".collection-checkboxes").each(function () {
                    $(this).attr("disabled", "disabled");
                });
                $("#btApproveCN").hide();
                $("#btRejectCN").hide();
            }
            $("#slStatus").attr("required", "required");
            $(".divPICARemark").hide();
            $(".divChecklistDocumentCtrl").show();
        }

        $("#btSubmit").unbind().on("click", function (e) {
            if (role != Role.ARCollection) {
                if ($("#formTransaction").parsley().validate())
                    InvoiceTower.Checklist();
            } else {
                if ($("#formTransaction").parsley().validate())
                    InvoiceTower.ChecklistCollection();
            }
            e.preventDefault();
        });
    }
}

var TableChecklistHistory = {
    Init: function (data) {
        var no = 1;
        var tblChecklistHistory = $("#tblChecklistHistory").DataTable({
            "language": {
                "emptyTable": "No data available in table",
            },
            "data": data,
            "filter": false,
            "lengthMenu": [[-1], ['All']],
            "lengthChange": false,
            "destroy": true,
            "columns": [
                { render: function (data, type, full) { return no++; } },
                { data: "VerificationStatus" },
                { data: "ReprintRemarks" },
                { data: "CreatedBy" },
                {
                    data: "CreatedDate", render: function (data, type, full) {
                        var normalDate = Common.Format.ConvertJSONDateTime(data.substr(0, 10));
                        var time = (data.substr(11, 8));
                        return normalDate + " " + time;
                    }
                },
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "buttons": [],
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': []
            }],
            "order": [[0, "asc"]]
        });
    }
}

Role = {
    ARCollection: "AR COLLECTION",
    ARData: "AR DATA"
}

VerificationStatus = {
    Pending: "P",
    Approved: "A",
    Rejected: "R",
    New: "N",
    Waiting: "W"
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