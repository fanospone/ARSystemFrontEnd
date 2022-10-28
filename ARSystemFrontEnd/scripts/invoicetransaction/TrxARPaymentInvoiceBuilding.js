Data = {};

var fsCustomerName = '';
var fsInvoiceTypeId = '';
var fsInvCompanyId = '';
var fsInvNo = '';

/* Helper Functions */

jQuery(document).ready(function () {
    Form.Init();
    FormInvoiceDetail.Init();

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

    $("#btCancel").unbind().click(function () {
        Form.Cancel();
    });

    $(".btCancelInvoiceDetail").unbind().click(function () {
        FormInvoiceDetail.Cancel();
    });

    $("#btSubmit").unbind().on("click", function (e) {
        var isValidPaidDate = $("#tbPaymentDate").parsley().validate();
        var isValidPaymentBank = $("#slPaymentBank").parsley().validate();
        if (isValidPaidDate == true && isValidPaymentBank == true) {
            if (Data.PaidStatus == "") {
                Common.Alert.Warning("There is no Amount to be Save")
            }
            else {
                $('#spPaidStatus').text(Data.PaidStatus);
                $('#mdlConfirmSave').modal('show');
            }
        } else {
            Helper.ScrollToTop();
        }
        e.preventDefault();
    });

    $("#btYesConfirm").unbind().click(function () {
        InvoiceBuilding.SavePayment();
    });

    $("#lnkDownload").unbind().on("click", function (e) {
        Helper.Download(Data.Selected.FilePath, Data.Selected.InvReceiptFile, Data.Selected.ContentType);
        e.preventDefault();
    });

    Control.BindingSelectInvoiceType();
    Control.BindingSelectCompany();

    Helper.InitCurrencyInput("#tbPaidAmount");
    Helper.InitCurrencyInput("#tbAmountRND");
    Helper.InitCurrencyInput("#tbAmountDR");
    Helper.InitCurrencyInput("#tbAmountRTGS");
    Helper.InitCurrencyInput("#tbAmountPenalty");

    $('#rdPPNExpired').on("switchChange.bootstrapSwitch", function (event, state) {
        Helper.Calculate();
    });

    $('#rdPPH').on("switchChange.bootstrapSwitch", function (event, state) {
        Helper.Calculate();
    });

    // Set datepicker to start at today - 7 days and end at today + 0 day
    $("body").delegate(".datepicker", "focusin", function () {
        $(this).datepicker({
            startDate: '-6d',
            endDate: '+0d',
            format: "dd-M-yyyy"
        });
    });

    $('input[type=radio][name=rdPPHType]').change(function () {
        Helper.Calculate();
    });

    $("#btYesBackToARProcess").unbind().click(function (e) {
        var isRemarkValid = $("#tbRemarks").parsley().validate();
        if (isRemarkValid)
            InvoiceBuilding.BackToARProcess(Data.Selected.trxInvoiceHeaderID);
        e.preventDefault();
    });
});

var Form = {
    Init: function () {
        $("#pnlPayment").hide();
        $("#formInvoiceInformation").parsley();
        Data.Mode = "AR Receipt";

        if (!$("#hdAllowProcess").val()) {
            $("#btSubmit").hide();
        }

        Table.Reset();
    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlPayment").hide();
        $(".panelDetailData").hide();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $(".panelSearchBegin").fadeIn();
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        $("#pnlPayment").hide();
        //Table.Reset();
        Table.Search();
    },
    Clear: function () {
        var emptyString = "";
        var zero = "0.00";
        $("#tbInvoiceNo").val(emptyString);
        $("#tbCompanyName").val(emptyString);
        $("#tbCustomerName").val(emptyString);
        $("#tbReceiptDate").val(emptyString);
        $("#tbPICInternal").val(emptyString);
    },
    SelectedData: function () {
        $("#pnlSummary").hide();
        $("#pnlPayment").fadeIn();
        $(".pnlPayment").show();
        $("#pnlPaymentTitle").text("AR Receipt");
        $("#formInvoiceInformation").parsley().reset();

        $("#tbInvoiceNo").val(Data.Selected.InvNo);
        $("#tbCompanyName").val(Data.Selected.Company);
        $("#tbCustomerName").val(Data.Selected.CustomerName);
        $("#tbReceiptDate").val(Common.Format.ConvertJSONDateTime(Data.Selected.InvReceiptDate));
        $("#tbPICInternal").val(Data.Selected.InvInternalPIC);

        Control.BindingSelectPaymentBank(Data.Selected.InvCompanyId, Data.Selected.Currency);
        TableHistory.Init(Data.Selected.History);

        Data.trxInvoiceHeaderID = Data.Selected.trxInvoiceHeaderID;
        Data.mstInvoiceCategoryId = Data.Selected.mstInvoiceCategoryId;
        Data.PaidStatus = "";
        Data.Validate = Helper.ValidateIncludedAmount(Data.Selected.trxInvoiceHeaderID);
        Data.PPHValue = $("#hdPPHValue").val();
        Data.PPFValue = $("#hdPPFValue").val();
        Data.IsPPHFinal = Data.Selected.isPPHFinal;
        if (Data.IsPPHFinal) {
            $("#rbPPF").attr("checked", "checked");
            $("#rbPPH").removeAttr("checked");
        }
        else {
            $("#rbPPH").attr("checked", "checked");
            $("#rbPPF").removeAttr("checked");
        }

        //Check if PPH has been Paid before or not
        if (Data.Selected.PPHIndex > 0 || Data.Selected.PPFIndex > 0) {
            var text = Data.Selected.PPHIndex > 0 ? Data.Selected.PPHIndex : Data.Selected.PPFIndex;
            $("#lblPPHIndex").text(text);
            $("#spPPHIndex").show();
        }
        else
            $("#spPPHIndex").hide();

        //Check if PPE has been Paid before or not
        if (Data.Selected.PPEIndex > 0) {
            $("#lblPPEIndex").text(Data.Selected.PPEIndex);
            $("#spPPEIndex").show();
        }
        else
            $("#spPPEIndex").hide();

        var AmountDPP = Data.Selected.InvSumADPP == null ? 0 : Data.Selected.InvSumADPP;
        var AmountPPN = Data.Selected.InvTotalAPPN == null ? 0 : Data.Selected.InvTotalAPPN;
        var AmountPAT = Data.Selected.InvTotalPenalty == null ? 0 : Data.Selected.InvTotalPenalty;
        //var AmountPPH = Data.Selected.InvTotalAPPH == null ? 0 : Data.Selected.InvTotalAPPH;
        var AmountPPH = 0;
        var AmountPPE = 0;
        var AmountPPF = 0;
        if (!Data.Validate.isShowPPH || !Data.Validate.isShowPPF) {
            $("#rdPPH").bootstrapSwitch('state', false);
            $("#rdPPH").bootstrapSwitch('disabled', true);
        }
        else {
            $("#rdPPH").bootstrapSwitch('state', true);
            $("#rdPPH").bootstrapSwitch('disabled', false);
        }
        if ($("#rdPPH").bootstrapSwitch("state") == false) {
            AmountPPH = 0;
            AmountPPF = 0;
        }
        else {
            //check what radio button is checked
            if ($("input[name=rdPPHType]:checked").val() == 0)//PPH
            {
                AmountPPH = AmountDPP * Data.PPHValue;
                $('#tbAmountPPH').val(Common.Format.CommaSeparation(AmountPPH.toString()));
                $('#tbAmountPPF').val(Common.Format.CommaSeparation(AmountPPF.toString()));
                $(".tbPPH").show();
                $(".tbPPF").hide();
            }
            else //PPF
            {
                AmountPPF = AmountDPP * Data.PPFValue;
                $('#tbAmountPPH').val(Common.Format.CommaSeparation(AmountPPH.toString()));
                $('#tbAmountPPF').val(Common.Format.CommaSeparation(AmountPPF.toString()));
                $(".tbPPH").hide();
                $(".tbPPF").show();
            }
        }

        if (!Data.Validate.isShowPPE) {
            $("#rdPPNExpired").bootstrapSwitch('state', false);
            $("#rdPPNExpired").bootstrapSwitch('disabled', true);
        }
        else {
            $("#rdPPNExpired").bootstrapSwitch('disabled', false);
        }

        if ($("#rdPPNExpired").bootstrapSwitch("state") == false) {
            AmountPPE = 0;
        }
        else {
            AmountPPE = AmountPPN;
        }

        if (!Data.Validate.isShowPAT)
            AmountPAT = 0;

        var AmountDeal = AmountDPP + AmountPPN - AmountPPH - AmountPAT - AmountPPF;
        //var AmountDeal = AmountDPP + AmountPPN - (AmountDPP * Data.PPHValue);
        var AmountPartialPaid = Data.Selected.PartialPaid == null ? 0 : Data.Selected.PartialPaid;

        $("#tbInvoiceNo").val(Data.Selected.InvNo);
        $("#tbCompany").val(Data.Selected.Company);
        $("#tbOperator").val(Data.Selected.InvOperatorID);
        $("#tbPICInternal").val(Data.Selected.InvInternalPIC);
        $("#tbReceiptDate").val(Common.Format.ConvertJSONDateTime(Data.Selected.InvReceiptDate));

        $('input[name="rdCurrency"][value="' + Data.Selected.Currency + '"]').prop('checked', true);
        $('input[name="rdCurrency"]').attr('disabled', true);
        $('#tbAmountDPP').val(Common.Format.CommaSeparation(AmountDPP.toString()));
        $('#tbAmountPPN').val(Common.Format.CommaSeparation(AmountPPN.toString()));
        $('#tbAmountPAT').val(Common.Format.CommaSeparation(AmountPAT.toString()));
        $('#tbAmountDeal').val(Common.Format.CommaSeparation(AmountDeal.toString()));
        $('#tbPartialPaid').val(Common.Format.CommaSeparation(AmountPartialPaid.toString()));
        $('#tbBalanceInvoice').val(Common.Format.CommaSeparation((AmountDeal - AmountPartialPaid).toString()));
        $('#tbTotalMonitoring').val(Common.Format.CommaSeparation((AmountDeal - AmountPartialPaid).toString()));
        $('#tbBalance').val(Common.Format.CommaSeparation((AmountDeal - AmountPartialPaid).toString()));
        $('#tbAmountPPNExpired').val(Common.Format.CommaSeparation(AmountPPE.toString()));
        $('#tbPaidAmount').val(Common.Format.CommaSeparation("0.00"));
        $('#tbAmountRND').val(Common.Format.CommaSeparation("0.00"));
        $('#tbAmountDR').val(Common.Format.CommaSeparation("0.00"));
        $('#tbAmountRTGS').val(Common.Format.CommaSeparation("0.00"));
        $('#tbAmountPenalty').val(Common.Format.CommaSeparation("0.00"));
        $('#tbPaymentDate').val("");
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

        fsCustomerName = ($("#tbSearchCustomerName").val() == null) ? "" : $("#tbSearchCustomerName").val();
        fsInvoiceTypeId = ($("#slSearchTerm").val() == null) ? "" : $("#slSearchTerm").val();
        fsInvCompanyId = ($("#slSearchCompany").val() == null) ? "" : $("#slSearchCompany").val();
        fsInvNo = $("#tbInvNo").val();

        var params = {
            customerName: fsCustomerName,
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
                "url": "/api/ARPaymentInvoiceBuilding/grid",
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
                        strReturn += "<button type='button' title='Detail' class='btn blue btn-xs btDetail'><i class='fa fa-mouse-pointer'></i></button>";
                        strReturn += "<button type='button' title='Back To AR Process' class='btn red btn-xs btBackToARProcess pull-right'>To AR Process</button>";
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
                    data: "InvReceiptDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "AgingDays" },
                { data: "InvInternalPIC" },
                { data: "PaidStatus" },
                {
                    data: "InvAmountLossPPN", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "InvTotalPenalty", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "InvTotalAmount", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                { data: "Currency" },
                {
                    data: "ChecklistDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "PPHType" }
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
            "order": [],
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            }
        });

        $("#tblSummaryData tbody").unbind();

        $("#tblSummaryData tbody").on("click", "button.btDetail", function (e) {
            var table = $("#tblSummaryData").DataTable();
            Data.Selected = {};
            Data.Selected = table.row($(this).parents('tr')).data();

            Form.SelectedData();
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

        $("#tblSummaryData tbody").on("click", "button.btBackToARProcess", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var selectedRow = table.row($(this).parents('tr')).data();
            Data.Selected = {};
            Data.Selected.trxInvoiceHeaderID = selectedRow.trxInvoiceHeaderID;
            $("#tbRemarks").val("");
            $('#mdlBackToARProcess').modal('show');
        });
    },
    Reset: function () {
        fsCustomerName = "";
        fsInvoiceTypeId = "";
        fsInvCompanyId = "";
        fsInvNo = "";
        $("#tbSearchCustomerName").val("");
        $("#slSearchTerm").val("").trigger("change");
        $("#slSearchCompany").val("").trigger("change");
        $("#tbInvNo").val("");
    },
    Export: function () {
        var href = "/InvoiceTransaction/TrxARPaymentInvoiceBuilding/Export?customerName=" + fsCustomerName + "&invoiceTypeId=" + fsInvoiceTypeId + "&invCompanyId=" + fsInvCompanyId + "&invNo=" + fsInvNo;

        window.location.href = href;
    }
}

var TableHistory = {
    Init: function (data) {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        var rowNumber = 1;
        var rowCount = data.length;
        var tblHistory = $("#tblHistory").DataTable({
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table",
            },
            "data": data,
            buttons: [],
            "filter": false,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        if (rowNumber == rowCount)
                            return 'Total';
                        return rowNumber++;
                    }
                },
                {
                    data: "PaymentDate"
                },
                {
                    data: "DBT", className: "text-right", render: function (data, type, full) {
                        if (data == null)
                            return '';
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "PAM", className: "text-right", render: function (data, type, full) {
                        if (data == null)
                            return '';
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "PNT", className: "text-right", render: function (data, type, full) {
                        if (data == null)
                            return '';
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "PPE", className: "text-right", render: function (data, type, full) {
                        if (data == null)
                            return '';
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "PPH", className: "text-right", render: function (data, type, full) {
                        if (data == null)
                            return '';
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "RTG", className: "text-right", render: function (data, type, full) {
                        if (data == null)
                            return '';
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "RND", className: "text-right", render: function (data, type, full) {
                        if (data == null)
                            return '';
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "PAT", className: "text-right", render: function (data, type, full) {
                        if (data == null)
                            return '';
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "PPF", className: "text-right", render: function (data, type, full) {
                        if (data == null)
                            return '';
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "Total", className: "text-right", render: function (data, type, full) {
                        if (data == null)
                            return '';
                        return Common.Format.CommaSeparation(data);
                    }
                }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                l.stop();
            },
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
            }],
            "order": []
        });
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

        var no = 1;
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
                {
                    render: function (data) {
                        return no++;
                    }
                },
                { data: "CompanyName" },
                { data: "TermPeriod" },
                {
                    data: "Area", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "MeterPrice", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "MonthlyPrice", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                { data: "Currency" },
                {
                    data: "StartPeriod", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndPeriod", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
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

var InvoiceBuilding = {
    SavePayment: function () {
        var l = Ladda.create(document.querySelector("#btYesConfirm"))

        var AmountPPNExpired = parseFloat($("#tbAmountPPNExpired").val().replace(/,/g, ""));
        var AmountPAM = parseFloat($("#tbPaidAmount").val().replace(/,/g, ""));
        var AmountDBT = parseFloat($("#tbAmountDR").val().replace(/,/g, ""));
        var AmountPNT = parseFloat($("#tbAmountPenalty").val().replace(/,/g, ""));
        var AmountRND = parseFloat($("#tbAmountRND").val().replace(/,/g, ""));
        var AmountRTG = parseFloat($("#tbAmountRTGS").val().replace(/,/g, ""));
        var AmountPartialPaid = parseFloat($("#tbPartialPaid").val().replace(/,/g, ""));
        var AmountDPP = parseFloat($("#tbAmountDPP").val().replace(/,/g, ""));
        var AmountPAT = parseFloat($("#tbAmountPAT").val().replace(/,/g, ""));
        var AmountPPH = AmountDPP * Data.PPHValue;
        if ($("#rdPPH").bootstrapSwitch("state") == false) {
            AmountPPH = 0;
            AmountPPF = 0;
        }
        else {
            //check what radio button is checked
            if ($("input[type=radio][name=rdPPHType]:checked").val() == 0)//PPH
            {
                AmountPPH = AmountDPP * Data.PPHValue;
                AmountPPF = 0;
            }
            else //PPF
            {
                AmountPPF = AmountDPP * Data.PPFValue;
                AmountPPH = 0;
            }
        }
        if (AmountPPH == null || AmountPPH == undefined || AmountPPH == "")
            AmountPPH = 0;

        if (AmountPPF == null || AmountPPF == undefined || AmountPPF == "")
            AmountPPF = 0;

        if (AmountPPNExpired == null || AmountPPNExpired == undefined || AmountPPNExpired == "")
            AmountPPNExpired = 0;

        if (AmountPAM == null || AmountPAM == undefined || AmountPAM == "")
            AmountPAM = 0;

        if (AmountDBT == null || AmountDBT == undefined || AmountDBT == "")
            AmountDBT = 0;

        if (AmountPNT == null || AmountPNT == undefined || AmountPNT == "")
            AmountPNT = 0;

        if (AmountRND == null || AmountRND == undefined || AmountRND == "")
            AmountRND = 0;

        if (AmountRTG == null || AmountRTG == undefined || AmountRTG == "")
            AmountRTG = 0;

        if (AmountPartialPaid == null || AmountPartialPaid == undefined || AmountPartialPaid == "")
            AmountPartialPaid = 0;

        if (AmountDPP == null || AmountDPP == undefined || AmountDPP == "")
            AmountDPP = 0;

        if (AmountPAT == null || AmountPAT == undefined || AmountPAT == "")
            AmountPAT = 0;

        var AmountTotalPaid = AmountPAM + AmountRND + AmountDBT + AmountRTG + AmountPNT + AmountPPH + AmountPartialPaid + AmountPPF;
        //var AmountTotalPaid = AmountPAM + AmountRND + AmountDBT + AmountRTG + AmountPNT + AmountPPNExpired + AmountPPH + AmountPartialPaid + AmountPPF;
        var params = {
            trxInvoiceHeaderId: Data.trxInvoiceHeaderID,
            mstInvoiceCategoryId: Data.mstInvoiceCategoryId,
            InvPaidDate: $("#tbPaymentDate").val(),
            mstPaymentId: $("#slPaymentBank").val(),
            InvPaidStatus: Data.PaidStatus,
            PAM: AmountPAM,
            RND: AmountRND,
            DBT: AmountDBT,
            RTGS: AmountRTG,
            PNT: AmountPNT,
            PPE: AmountPPNExpired,
            PPH: AmountPPH,
            PAT: AmountPAT,
            ARTotalPaid: AmountTotalPaid,
            PPF: AmountPPF,
            IsPPHFinal: ($("input[type=radio][name=rdPPHType]:checked").val() == 1)
        }

        $.ajax({
            url: "/api/ARPaymentInvoiceBuilding/SavePayment",
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
                Common.Alert.Success("Data Success Paid With Status:" + Data.PaidStatus)
            }
            l.stop();

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Form.Done();
            $('#mdlConfirmSave').modal('toggle');
        })
    },
    BackToARProcess: function (HeaderId) {
        var l = Ladda.create(document.querySelector("#btYesBackToARProcess"))
        var params = {
            trxInvoiceHeaderId: HeaderId,
            strRemarks: $("#tbRemarks").val()
        }

        $.ajax({
            url: "/api/ARPaymentInvoiceBuilding/CNInvoiceToARProcess",
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
                $('#mdlBackToARProcess').modal('hide');
                Common.Alert.Success("Data Success Back To AR Process!")
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
            $("#slSearchTerm").html("<option></option>");

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
    BindingSelectPaymentBank: function (CompanyId, Currency) {
        var params = {
            CompanyId: CompanyId,
            Currency: Currency
        };
        $.ajax({
            url: "/api/MstDataSource/PaymentBank",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slPaymentBank").html("<option></option>")

            var mstPaymentBankId;
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {

                    $("#slPaymentBank").append("<option value='" + item.mstPaymentBankId + "'>" + item.BankGroupId + " - " + item.AccountNum + "</option>");
                    mstPaymentBankId = item.mstPaymentBankId;
                })
            }

            $("#slPaymentBank").val(mstPaymentBankId).trigger("change");
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    }
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
            Helper.Calculate();
        })
    },
    Download: function (filePath, invReceiptFile, contentType) {
        var docPath = $("#hdDocPath").val();
        var path = docPath + filePath;
        window.location.href = "/Admin/Download?path=" + path + "&fileName=" + invReceiptFile + "&contentType=" + contentType;
    },
    Calculate: function () {
        if ($("#rdPPNExpired").bootstrapSwitch("state") == false) {
            $('.disabledCtrl').hide();
        }
        else
            $('.disabledCtrl').show();

        var AmountPPH = parseFloat($("#tbAmountPPH").val().replace(/,/g, ""));
        var AmountPPF = parseFloat($("#tbAmountPPF").val().replace(/,/g, ""));
        var AmountPPN = parseFloat($("#tbAmountPPN").val().replace(/,/g, ""));
        var AmountDPP = parseFloat($("#tbAmountDPP").val().replace(/,/g, ""));
        var AmountPartial = parseFloat($("#tbPartialPaid").val().replace(/,/g, ""));
        var AmountPAT = parseFloat($("#tbAmountPAT").val().replace(/,/g, ""));

        var AmountPAM = parseFloat($("#tbPaidAmount").val().replace(/,/g, ""));
        var AmountDBT = parseFloat($("#tbAmountDR").val().replace(/,/g, ""));
        var AmountPNT = parseFloat($("#tbAmountPenalty").val().replace(/,/g, ""));
        var AmountRND = parseFloat($("#tbAmountRND").val().replace(/,/g, ""));
        var AmountRTG = parseFloat($("#tbAmountRTGS").val().replace(/,/g, ""));

        var AmountPPNExpired = 0;
        if (AmountPPH == null || AmountPPH == undefined || AmountPPH == "")
            AmountPPH = 0;
        if (AmountPPF == null || AmountPPF == undefined || AmountPPF == "")
            AmountPPF = 0;
        if (AmountPPN == null || AmountPPN == undefined || AmountPPN == "")
            AmountPPN = 0;
        if (AmountDPP == null || AmountDPP == undefined || AmountDPP == "")
            AmountDPP = 0;
        if (AmountPartial == null || AmountPartial == undefined || AmountPartial == "")
            AmountPartial = 0;
        if (AmountPAT == null || AmountPAT == undefined || AmountPAT == "")
            AmountPAT = 0;

        if (AmountPAM == null || AmountPAM == undefined || AmountPAM == "")
            AmountPAM = 0;
        if (AmountDBT == null || AmountDBT == undefined || AmountDBT == "")
            AmountDBT = 0;
        if (AmountPNT == null || AmountPNT == undefined || AmountPNT == "")
            AmountPNT = 0;
        if (AmountRND == null || AmountRND == undefined || AmountRND == "")
            AmountRND = 0;
        if (AmountRTG == null || AmountRTG == undefined || AmountRTG == "")
            AmountRTG = 0;

        if ($("#rdPPH").bootstrapSwitch("state") == false) {
            $(".divPPHType").hide();
            AmountPPH = 0;
            AmountPPF = 0;
        }
        else {
            $(".divPPHType").show();
            //check what radio button is checked
            if ($("input[type=radio][name=rdPPHType]:checked").val() == 0)//PPH
            {
                AmountPPH = AmountDPP * Data.PPHValue;
                AmountPPF = 0;
                $('#tbAmountPPH').val(Common.Format.CommaSeparation(AmountPPH.toString()));
                $('#tbAmountPPF').val(Common.Format.CommaSeparation(AmountPPF.toString()));
                $(".tbPPH").show();
                $(".tbPPF").hide();
            }
            else //PPF
            {
                AmountPPF = AmountDPP * Data.PPFValue;
                AmountPPH = 0;
                $('#tbAmountPPH').val(Common.Format.CommaSeparation(AmountPPH.toString()));
                $('#tbAmountPPF').val(Common.Format.CommaSeparation(AmountPPF.toString()));
                $(".tbPPH").hide();
                $(".tbPPF").show();
            }
        }
        if ($("#rdPPNExpired").bootstrapSwitch("state") == false) {
            AmountPPNExpired = 0;
        }
        else {
            AmountPPNExpired = AmountDPP * 0.1;//AmountPPN;
        }

        var AmountDeal = AmountDPP + AmountPPN - AmountPPH - AmountPAT - AmountPPF;
        //var AmountDeal = AmountDPP + AmountPPN - (AmountDPP * Data.PPHValue);
        var AmountBalanceInvoice = parseFloat(AmountDeal - AmountPartial).toFixed(2);
        var AmountBalance = parseFloat(AmountBalanceInvoice - AmountPAM - AmountDBT - AmountPNT - AmountRND - AmountRTG).toFixed(2);
        //var AmountBalance = parseFloat(AmountBalanceInvoice - AmountPPNExpired - AmountPAM - AmountDBT - AmountPNT - AmountRND - AmountRTG).toFixed(2);

        //var AmountBalance = AmountBalanceInvoice - AmountPPNExpired - AmountPAM - AmountDBT - AmountPNT - AmountRND - AmountRTG;
        $("#tbAmountPPNExpired").val(Common.Format.CommaSeparation(AmountPPNExpired.toString()));
        $("#tbAmountDeal").val(Common.Format.CommaSeparation(AmountDeal.toString()));
        $("#tbBalanceInvoice").val(Common.Format.CommaSeparation(AmountBalanceInvoice.toString()));
        $("#tbTotalMonitoring").val(Common.Format.CommaSeparation(AmountBalanceInvoice.toString()));
        $("#tbBalance").val(Common.Format.CommaSeparation(AmountBalance.toString()));
        if (AmountBalance == AmountBalanceInvoice) {
            $("#tbPaymentType").val("");
        }
        else if (AmountBalance == 0) {
            $("#tbPaymentType").val("FULL");
            Data.PaidStatus = "FULL";
        }
        else if (AmountBalance > 0) {
            $("#tbPaymentType").val("PARTIAL");
            Data.PaidStatus = "PARTIAL";
        }
        else if (AmountBalance < 0) {
            $("#tbPaymentType").val("OVERPAID");
            Data.PaidStatus = "OVERPAID";
        }
    },
    ValidateIncludedAmount: function (HeaderId, CategoryId) {
        var Result = {};
        var params = {
            trxInvoiceHeaderID: HeaderId
        }
        $.ajax({
            url: "/api/ARPaymentInvoiceBuilding/GetValidateIncludedAmount",
            type: "GET",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: params,
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                Result = data;
            }
        })
        return Result;
    },
    Download: function (filePath, invReceiptFile, contentType) {
        var docPath = $("#hdDocPath").val();
        var path = docPath + filePath;
        window.location.href = "/Admin/Download?path=" + path + "&fileName=" + invReceiptFile + "&contentType=" + contentType;
    },
    ScrollToTop: function () {
        $("html, body").animate({ scrollTop: 0 }, "slow");
        return false;
    }
}