Data = {};

var fsInvOperatorId = '';
var fsInvoiceTypeId = '';
var fsInvCompanyId = '';
var fsInvNo = '';
var fsIsReceipt = '';

/* Helper Functions */

jQuery(document).ready(function () {
    Form.Init();
    FormInvoiceDetail.Init();
    FormPICA.Init();

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

    $(".btCancel").unbind().click(function () {
        Form.Cancel();
    });

    $(".btCancelPICA").unbind().click(function () {
        FormPICA.Cancel();
    });

    $(".btCancelInvoiceDetail").unbind().click(function () {
        FormInvoiceDetail.Cancel();
    });

    // Initialize Datepicker
    $("body").delegate(".datepicker", "focusin", function () {
        $(this).datepicker({
            format: "dd-M-yyyy"
        });
    });

    $(".fileinput").fileinput();

    $("#slPICAType").unbind().on("change", function () {
        var PICATypeId = $(this).val();
        if (PICATypeId != null && PICATypeId != "") {
            if (Data.Selected != null)
                Control.BindingPICADetail(PICATypeId, Data.Selected.mstPICADetailID);
            else
                Control.BindingPICADetail(PICATypeId);
        } else {
            Control.BindingPICADetail(0);
        }
    });

    $("#btSubmit").unbind().on("click", function (e) {
        if ($("#formTransaction").parsley().validate())
            InvoiceTower.SaveARReceipt();
        e.preventDefault();
    });

    $("#btSubmitPICA").unbind().on("click", function (e) {
        if ($("#formPICA").parsley().validate())
            InvoiceTower.SavePICAARCollection();
        e.preventDefault();
    });

    $("#btDownload").unbind().on("click", function (e) {
        Helper.Download(Data.Selected.FilePath, Data.Selected.InvReceiptFile, Data.Selected.ContentType);
    });

    Control.BindingSelectInvoiceType();
    Control.BindingSelectOperator();
    Control.BindingInternalPIC();
    Control.BindingPICAType();
    Control.BindingPICAMajor();

    Helper.InitCurrencyInput("#tbPenaltyAmount");
});

var Form = {
    Init: function () {
        $("#pnlTransaction").hide();
        $("#formTransaction").parsley();
        Data.Mode = "AR Receipt";

        if (!$("#hdAllowProcess").val()) {
            $("#btSubmit").hide();
        }
        $("#divReceiptFile").hide();

        Table.Reset();
    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
        $("#divReceiptFile").hide();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
        $(".panelSearchBegin").fadeIn();
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        Table.Search();
    },
    Clear: function () {
        var emptyString = "";
        var zero = "0.00";
        $("#tbInvoiceNo").val(emptyString);
        $("#tbCompanyName").val(emptyString);
        $("#tbCustomerName").val(emptyString);
        $("#tbReceiptDate").val(emptyString);
        $("#slInternalPIC").val(emptyString).trigger("change");
        $("#taRemark").val(emptyString);
        $("#tbPenaltyAmount").val(zero);
    },
    SelectedData: function () {
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("AR Receipt");
        $("#formTransaction").parsley().reset();
        $(".fileinput").fileinput("clear");

        $("#tbInvoiceNo").val(Data.Selected.InvNo);
        $("#tbCompanyName").val(Data.Selected.Company);
        $("#tbCustomerName").val(Data.Selected.CustomerName);
        $("#tbReceiptDate").val(Common.Format.ConvertJSONDateTime(Data.Selected.InvReceiptDate));
        $("#tbPenaltyAmount").val(Common.Format.CommaSeparation(Data.Selected.ARProcessPenalty));

        if (Data.Selected.InvInternalPIC == null || Data.Selected.InvInternalPIC == "") {
            $("#slInternalPIC").val(Data.Selected.ChecklistUser).trigger("change");
        } else {
            $("#slInternalPIC").val(Data.Selected.InvInternalPIC).trigger("change");
        }

        $("#taRemark").val(Data.Selected.ARProcessRemark);

        if (Data.Selected.InvReceiptFile != null && Data.Selected.InvReceiptFile != "") {
            $("#divReceiptFile").show();
            $("#btDownload").html('<i class="fa fa-download"></i> ' + Data.Selected.InvReceiptFile);
            $("#fuReceipt").removeAttr("required");
        } else {
            $("#divReceiptFile").hide();
            $("#btDownload").html('<i class="fa fa-download"></i>');
            $("#fuReceipt").attr("required", "required");
        }
    },
}

var FormPICA = {
    Init: function () {
        $("#pnlPICA").hide();
        $("#formPICA").parsley();
        Data.Mode = "PICA AR Collection";

        if (!$("#hdAllowProcess").val()) {
            $("#btSubmitPICA").hide();
        }

        $("#slPICAType").removeAttr("disabled");
        $("#slPICAMajor").removeAttr("disabled");
        $("#slPICADetail").removeAttr("disabled");
        $("#taRemarkPICA").removeAttr("disabled");
        $("#btSubmitPICA").show();
        $("#btPrint").hide();
    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlPICA").hide();
    },
    Done: function () {
        $("#slPICAType").attr("disabled", "disabled");
        $("#slPICAMajor").attr("disabled", "disabled");
        $("#slPICADetail").attr("disabled", "disabled");
        $("#taRemarkPICA").attr("disabled", "disabled");
        $("#btPrint").show();
        $("#btSubmitPICA").hide();
        Table.Search();
    },
    Clear: function () {
        var emptyString = "";
        var zero = "0.00";
        $("#slPICAType").val(emptyString).trigger("change");
        $("#slPICAMajor").val(emptyString).trigger("change");
        $("#slPICADetail").val(emptyString).trigger("change");
        $("#taRemarkPICA").val(emptyString);
    },
    SelectedData: function () {
        $("#pnlSummary").hide();
        $("#pnlPICA").fadeIn();
        $(".panelPICA").show();
        $("#panelPICATitle").text("PICA AR Collection");
        $("#formPICA").parsley().reset();

        $("#slPICAType").removeAttr("disabled");
        $("#slPICAMajor").removeAttr("disabled");
        $("#slPICADetail").removeAttr("disabled");
        $("#taRemarkPICA").removeAttr("disabled");
        $("#btSubmitPICA").show();
        $("#btPrint").hide();

        if (Data.Selected.InvReceiptFile != null && Data.Selected.InvReceiptFile && Data.Selected.InvReceiptFile != undefined) {
            $("#btSubmitPICA").removeAttr("disabled");
        } else {
            $("#btSubmitPICA").attr("disabled", "disabled");
        }

        $("#tbPICAInvoiceNo").val(Data.Selected.InvNo);
        if (Data.Selected != null) {
            $("#slPICAType").val(Data.Selected.mstPICATypeID).trigger("change");
            $("#slPICAMajor").val(Data.Selected.mstPICAMajorID).trigger("change");
            $("#slPICADetail").val(Data.Selected.mstPICADetailID).trigger("change");
            $("#taRemarkPICA").val(Data.Selected.Remark);
        } else {
            $("#slPICAType").val("").trigger("change");
            $("#slPICAMajor").val("").trigger("change");
            $("#slPICADetail").val("").trigger("change");
            $("#taRemarkPICA").val("");
        }
    },
}

var FormInvoiceDetail = {
    Init: function () {
        $("#pnlInvoiceDetail").hide();

        Table.Reset();
    },
    SelectedData: function (data) {
        $("#pnlSummary").hide();
        $("#pnlInvoiceDetail").fadeIn();
        $("#pnlDetailTitle").html("Invoice Detail");

        TableInvoiceDetail.Search(data);
    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlInvoiceDetail").hide();
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
        fsIsReceipt = $('input[name=rbReceipt]:checked').val();

        var params = {
            invOperatorId: fsInvOperatorId,
            invoiceTypeId: fsInvoiceTypeId,
            invCompanyId: fsInvCompanyId,
            invNo: fsInvNo,
            StatusReceipt : fsIsReceipt
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/ARProcessInvoiceTower/grid",
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

                        //if(full.IsLog == 0)
                            strReturn += "<button type='button' title='Detail' class='btn blue btn-xs btDetail'><i class='fa fa-mouse-pointer'></i></button>";

                        return strReturn;
                    }
                },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";

                        //if (full.IsLog == 0)
                            strReturn += "<button type='button' title='PICA' class='btn blue btn-xs btPICA'>PICA</button>";

                        return strReturn;
                    }
                },
                {
                    data: "InvNo", render: function (data, type, full) {
                        return "<a class='btInvoiceDetail'>" + data + "</a>";
                    }
                },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvTemp" },
                { data: "Term" },
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
                { data: "Currency" },
                { data: "StatusReceipt" },
                { data: "InvoiceStatus" },
                { data: "InvReceiptFile" },
                {
                    render: function (data, type, full) {
                        if (full.InvReceiptFile != null && full.InvReceiptFile != '')
                            return '<button type="button" class="btn btn-xs btn-primary btn-download" id="btDownload_' + full.trxInvoiceHeaderID + '"><i class="fa fa-download"></i></button>';
                        return '';
                    }
                },
                {
                    data: "ChecklistDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "PPHType" },
                {
                    data: "InvAmountLossPPN", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
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
                'aTargets': [0, 1, 16]
            }],
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 3 /* Set the 2 most left columns as fixed columns */
            },
            "order": []
        });

        $("#tblSummaryData tbody").unbind();

        $("#tblSummaryData tbody").on("click", "button.btDetail", function (e) {
            var table = $("#tblSummaryData").DataTable();
            Data.Selected = {};
            Data.Selected = table.row($(this).parents('tr')).data();

            Form.SelectedData();
        });

        $("#tblSummaryData tbody").on("click", "button.btPICA", function (e) {
            var table = $("#tblSummaryData").DataTable();
            Data.Selected = {};
            var selectedRow = table.row($(this).parents('tr')).data();

            Data.Selected.trxInvoiceHeaderID = selectedRow.trxInvoiceHeaderID;
            Data.Selected.InvNo = selectedRow.InvNo;
            Data.Selected.InvReceiptFile = selectedRow.InvReceiptFile;
            Data.Selected.mstInvoiceCategoryId = selectedRow.mstInvoiceCategoryId;
            if (selectedRow.PICAAR != null) {
                Data.Selected.trxPICAARID = selectedRow.PICAAR.trxPICAARID;
                Data.Selected.mstPICATypeID = selectedRow.PICAAR.mstPICATypeID;
                Data.Selected.mstPICAMajorID = selectedRow.PICAAR.mstPICAMajorID;
                Data.Selected.mstPICADetailID = selectedRow.PICAAR.mstPICADetailID;
                Data.Selected.Remark = selectedRow.PICAAR.Remark;
            }

            FormPICA.SelectedData();
        });

        $("#tblSummaryData tbody").on("click", "a.btInvoiceDetail", function (e) {
            var table = $("#tblSummaryData").DataTable();
            Data.Selected = {};
            Data.Selected = table.row($(this).parents('tr')).data().Detail;
            FormInvoiceDetail.SelectedData(Data.Selected);
        });

        $("#tblSummaryData tbody").on("click", "button.btn-download", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var row = table.row($(this).parents('tr')).data();
            Helper.Download(row.FilePath, row.InvReceiptFile, row.ContentType);
        });
    },
    Reset: function () {
        fsInvOperatorId = '';
        fsInvoiceTypeId = '';
        fsInvCompanyId = '';
        fsInvNo = '';
        fsIsReceipt = '';
        $("#slSearchCompany").val("").trigger("change");
        $("#slSearchTerm").val("").trigger("change");
        $("#slSearchOperator").val("").trigger("change");
        $("#tbInvNo").val("");
        $("#rbAll").prop('checked', true);
    },
    Export: function () {
        var href = "/InvoiceTransaction/TrxARProcessInvoiceTower/Export?invOperatorId=" + fsInvOperatorId + "&invoiceTypeId=" + fsInvoiceTypeId + "&invCompanyId=" + fsInvCompanyId + "&invNo=" + fsInvNo + "&StatusReceipt=" + fsIsReceipt;

        window.location.href = href;
    }
}

var TableInvoiceDetail = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblInvoiceDetail = $('#tblInvoiceDetail').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblInvoiceDetail").DataTable().columns.adjust().draw();
        });
    },
    Search: function (data) {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var tblInvoiceDetail = $("#tblInvoiceDetail").DataTable({
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table",
            },
            data: data,
            "filter": false,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            buttons: [],
            "columns": [
                { data: "SONumber" },
                { data: "SiteIdOld" },
                { data: "SiteName" },
                { data: "BapsPeriod" },
                {
                    data: "AmountRental", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "AmountService", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "DiscountRental", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "DiscountService", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "Total", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': []
            }],
            "order": [[0, "asc"]]
        });

        l.stop();
    }
}

var InvoiceTower = {
    SavePICAARCollection: function () {
        var params = {
            trxPICAARID: Data.Selected.trxPICAARID,
            trxInvoiceHeaderID: Data.Selected.trxInvoiceHeaderID,
            mstPICATypeID: $("#slPICAType").val(),
            mstPICAMajorID: $("#slPICAMajor").val(),
            mstPICADetailID: $("#slPICADetail").val(),
            Remark: $("#taRemarkPICA").val(),
            NeedCN: true,
            mstInvoiceCategoryId: Data.Selected.mstInvoiceCategoryId
        };

        var l = Ladda.create(document.querySelector("#btSubmitPICA"))
        $.ajax({
            url: "/api/ARProcessInvoiceTower/PICA",
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
                Common.Alert.Success("PICA AR Collection for Invoice " + Data.Selected.InvNo + " has been submitted!");
                //Table.Reset();
                var id = data.trxInvoiceHeaderID;
                if (Data.Selected.mstInvoiceCategoryId != 1)
                    id = data.trxInvoiceHeaderRemainingAmountID;

                $("#btPrint").attr("href", "/InvoiceTransaction/TrxPrintCNInvoiceTower/Print?trxCNInvoiceHeaderID=" + id + "&mstInvoiceCategoryId=" + Data.Selected.mstInvoiceCategoryId);

                FormPICA.Done();
            }
            l.stop()
        })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l.stop()
            })
    },
    SaveARReceipt: function () {
        var arProcessPenalty = parseFloat($("#tbPenaltyAmount").val().replace(/,/g, ""));
        var formData = new FormData();
        formData.append("ARProcessRemark", $("#taRemark").val());
        formData.append("InvReceiptDate", $("#tbReceiptDate").val());
        formData.append("InvInternalPIC", $("#slInternalPIC").val());
        formData.append("trxInvoiceHeaderID", Data.Selected.trxInvoiceHeaderID);
        formData.append("mstInvoiceCategoryId", Data.Selected.mstInvoiceCategoryId);
        formData.append("ARProcessPenalty", arProcessPenalty);

        var fileInput = document.getElementById("fuReceipt");
        var file = null;
        if (fileInput.files != undefined) {
            file = fileInput.files[0];
        }

        if (file != null && file != undefined) {
            formData.append("InvReceiptFile", file);
        }

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/ARProcessInvoiceTower/Receipt",
            type: "POST",
            dataType: "json",
            contentType: false,
            data: formData,
            processData: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("AR Receipt for Invoice: " + Data.Selected.InvNo + " has been submitted!");
                //Table.Reset();
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
    BindingInternalPIC: function () {
        $.ajax({
            url: "/api/MstDataSource/InternalPIC",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $("#slInternalPIC").html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $("#slInternalPIC").append("<option value='" + item.UserID + "'>" + item.FullName + "</option>");
                    })
                }

                $("#slInternalPIC").select2({ placeholder: "Select Internal PIC", width: null });

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
    BindingPICAMajor: function (selectedValue) {
        $.ajax({
            url: "/api/MstDataSource/PICAMajor",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slPICAMajor").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slPICAMajor").append("<option value='" + item.mstPICAMajorID + "'>" + item.Description + "</option>");
                })
            }

            $("#slPICAMajor").select2({ placeholder: "Select PICA Major", width: null });

            if (selectedValue != undefined) {
                $("#slPICAMajor").val(selectedValue).trigger("change");
            }
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