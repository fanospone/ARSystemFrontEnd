Data = {};
var fsCustomerId = "";
var fsCompanyId = "";
var fsProductType = "";
var fsGroupBy = "";
var fsBapsType = "";
var fsBapsTypeId = "";
var fsSoNumb = "";
var fsAction = "";
var fsUserId = "";
var fsPassword = "";
var fsRemark = "";
var fsCustomerID = "";
var fsSoNumber = "";
var fsSiteID = "";
var fsSiteIDCust = "";
var fsSiteNameCust
var fsIDTransaction = "";
var fsApprIDCompany = "";
var fsApprIDCustomer1 = "";
var fsApprIDCustomer2 = "";
var fsApprIDCustomer3 = "";
var fsStipCategory = "";
var NextActivity = "";
var Activitys = "";
DataSelected = {};
BapsTypeSoNumber = [];
CurrentTab = 0;

jQuery(document).ready(function () {

    Form.Init();
    FormEdit.Init();
    Control.BindYear();
    // Table.Init();
    //Table.GridBaspValidation();
    //$('#pwr').hide();
    // ==================== Buttton Seacrh =============================//
    $("#btSearch").unbind().click(function () {
        $('.panelBapsValidation').css('visibility', 'visible');
        $(".panelBapsValidation").fadeIn(1500);
        if ($("#tabStartBaps").tabs('option', 'active') == 0) {
            Table.GridBaspValidation();
        } else if ($("#tabStartBaps").tabs('option', 'active') == 1) {
            Table.GridBapsData();
        }
        else {
            Table.GridWaitingPo();
        }
    });
    // ==================== End Buttton Seacrh =============================//
    $("#btReset").unbind().click(function () {
        $("#slSearchCustomerID").val(" ").trigger('change');
        $("#slSearchSiteID").val("").trigger('change');
        $("#slSearchSoNumber").val("").trigger('change');
        $("#slSearchCompanyID").val("").trigger('change');
        $("#slSearchProductID").val("").trigger('change');
        $("#slBulkyNumber").val("").trigger('change');
    });

    $("#btnSaveValidation").unbind().click(function () {
        var oprID = $("#OperatorIdHD").val();
        Form.SubmitValidation(oprID);
    });

    $("#btnCancelValidation").unbind().click(function () {
        $(".panelValidationForm").fadeOut();
        $(".panelTab").fadeIn(1000);
        $(".filter").fadeIn();
        $(".panelDataValidationPrint").fadeIn(1000);
    });

    $("#BapsType").change(function () {
        WhereClause = " sonumb = '" + DataSelected.SoNumber + "' and site_id_old = '" + DataSelected.SiteID + "' and stip_siro = " + $("#StipSiro").val() + " and status_renewal = 1 and po_type = '" + $(this).find("option:selected").text() + "' "

        if ($(this).find("option:selected").text() == "POWER")
            $('#pwr').show();
        else
            $('#pwr').hide();


        $.ajax({
            url: "/api/MstDataSource/PONewBaps",
            type: "GET",
            data: { WhereClause: WhereClause }
        })
            .done(function (data, textStatus, jqXHR) {
                if (data.PONumber != null) {
                    $('#PoNumberVal').val(data.PONumber);
                    $('#PoDateVal').val(Common.Format.ConvertJSONDateTime(data.PoDate));
                    var amount = Common.Format.CommaSeparation(data.PoAmount);
                    $('#TotalPoAmountVal').val(Common.Format.CommaSeparation(data.PoAmount));
                    $('#InitialPoAmountVal').val(Common.Format.CommaSeparation(data.PoAmount));
                    $('#BaseLeasePriceVal').val(Common.Format.CommaSeparation(data.PriceAmount));
                    $('#ServicePriceVal').val(Common.Format.CommaSeparation(data.OmPrice));
                }
                //console.log(data);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    });

    $('#tabStartBaps').unbind().on('tabsactivate', function (event, ui) {
        var newIndex = ui.newTab.index();
        CurrentTab = newIndex;
        $('.BaukYear').hide();
        $('.caption-subject').text('Data');
        if (newIndex == 0) {
            // Control.BindingSelectSoNumberPint('validation');
            Control.TabActive(newIndex)
            Table.GridBaspValidation();
            $(".panelPicaInput").fadeOut(100);
            $(".panelDataValidationPrint").fadeOut(100);
            $(".panelDataValidationBulkyPrint").fadeOut(100);
            // $(".panelBapsValidation").fadeIn(1500);
            $('.BaukYear').show();
            $('.caption-subject').text('Data of Year');

        } else if (newIndex == 1) {
            Control.TabActive(newIndex)
            Table.GridBapsData();
            $(".panelPicaInput").fadeOut(100);
            $(".panelDataValidationPrint").fadeOut(100);
            $(".panelDataValidationBulkyPrint").fadeOut(100);
            //  $(".panelBapsValidation").fadeIn(1500);
        }
        else {
            //Control.TabActive(newIndex)
            Table.GridWaitingPo();
            $(".panelPicaInput").fadeOut(100);
            $(".panelDataValidationPrint").fadeOut(100);
            $(".panelDataValidationBulkyPrint").fadeOut(100);
            //  $(".panelBapsValidation").fadeIn(1500);
        }
        $('.panelBapsValidation').css('visibility', 'visible');
        $(".panelBapsValidation").fadeIn(1500);
    });

    // ==================== End Buttton Action =============================//
    $("#btnProcessData").unbind().click(function () {
        if ($('#BapsType').val() == null || $('#BapsType').val() == "") {
            Common.Alert.Warning("Please Select BAPS Type");
            return;
        }

        if ($("#formDetailData").parsley().validate()) {
            Control.BindingNextStep();
            $('#mdlConfirmNextProcess').modal('show');
        }
    });

    $("#btnOK").unbind().click(function () {
        var l = Ladda.create(document.querySelector("#btnOK"))
        l.start();
        NextActivity = $("#slNextStep").val();
        if (NextActivity != null && NextActivity != "") {
            //Form.BapsValidationSubmit();
            Form.ApproveNextStep();
            l.stop();
        } else {
            l.stop();
            $('#mdlConfirmNextProcess').modal('hide');
            Common.Alert.Warning("Please Select Next Activity");
        }

    });

    $("#btnBapsValidationSave").unbind().click(function () {
        var l = Ladda.create(document.querySelector("#btnBapsValidationSave"))
        l.start();
        if ($('#BapsType').val() == null || $('#BapsType').val() == "") {
            Common.Alert.Warning("Please Select BAPS Type"); l.stop();
            return;
        }

        if ($('#CompanyInvoice').val() == null || $('#CompanyInvoice').val() == "") {
            Common.Alert.Warning("Please Select Company Invoice !"); l.stop();
            return;
        }

        if ($('#CustomerInvoice').val() == null || $('#CustomerInvoice').val() == "") {
            Common.Alert.Warning("Please Select Customer Invoice !"); l.stop();
            return;
        }

        if (Data.TextboxEdit.IsActive) {
            Common.Alert.Warning("Please Save Distance Change !"); l.stop();
            return;
        }

        if ($("#formDetailData").parsley().validate()) {
            Form.BapsValidationSubmit(); l.stop();
        }

    });

    $("#btnBapsValidationCancel").unbind().click(function () {
        $("#partialBapsValidationForm").hide();
        Control.ButtonCancelForm();
    });

    $('.UploadInfo').hide();
    $("#StipSiro").change(function () {
        var WhereClause = "SoNumber = '" + DataSelected.SoNumber + "' AND StipSiro = ISNULL(" + $("#StipSiro").val() + ",0) ";
        Control.BindingBapsTypeSoNumber(WhereClause);
    });

    $('#BapsDateRow').hide();
    $('#BapsDateBlank').show();
    $(".DocumentBaukList").hide();
    $('#btnViewDocumentBauk').unbind().click(function () {
        $(".DocumentBaukList").show();
    });
    $('#BaukYear').change(function () {
        $('#btSearch').trigger("click");
    })
    if ($('#DefaultFilter').val()) {

        $('#btSearch').trigger("click");
    }

    $('#chkProrateFormulation').on('switchChange.bootstrapSwitch', function (event, state) {
        if (event.target.checked) {
            DataSelected.ProrateFormulation = 1;
        } else {
            DataSelected.ProrateFormulation = 0;
        }
        Control.BindProRateAmount()
    });
    
    $('#chkInstallationFee').on('switchChange.bootstrapSwitch', function (event, state) {
        if (event.target.checked) {
            DataSelected.InstallationFee = 1;
            $("#BapsDateBlank").hide();
            $("#divInstallationAmount").fadeIn();
        } else {
            DataSelected.InstallationFee = 0;
            $("#divInstallationAmount").hide();
            $("#BapsDateBlank").fadeIn();
            $("#tbxInstallationAmount").val("0");
        }
        Control.BindProRateAmount()
    });
});

var Control = {

    BindingSelectCompany: function () {
        var id = "#slSearchCompanyID";
        var id2 = "#CompanyInvoice";
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + $.trim(item.CompanyId) + "'>" + item.Company + "</option>");
                        $(id2).append("<option value='" + $.trim(item.CompanyId) + "'>" + item.Company + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Company Name", width: null });
                $(id2).select2({ placeholder: "Select Company Name", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingSelectOperator: function () {
        var id = "#slSearchCustomerID";
        var id2 = "#CustomerInvoice";
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option value=' ' selected>All</option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + $.trim(item.OperatorId) + "'>" + item.Operator + "</option>");
                        $(id2).append("<option value='" + $.trim(item.OperatorId) + "'>" + item.Operator + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Operator", width: null });
                $(id2).select2({ placeholder: "Select Operator", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingSelectProductType: function () {
        var id = "#slSearchProductID";
        $.ajax({
            url: "/api/MstDataSource/TenantType",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + $.trim(item.Value) + "'>" + item.Text + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Product", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingSelectGroupBy: function () {
        $("#slGroupBy").html("<option></option>")
        $("#slGroupBy").append("<option value='NEW' selected>New</option>");
        $("#slGroupBy").append("<option value='REPEAT'>Repeat Order</option>");
        $("#slGroupBy").select2({ placeholder: "Select Group", width: null });
    },

    BindingSelectInvoiceType: function () {
        //$("#InvoiceType").html("<option></option>")
        //$("#InvoiceType").append("<option value='TRIWULAN'>TRIWULAN</option>");
        //$("#InvoiceType").append("<option value='MONTHLY'>MONTHLY</option>");
        //$("#InvoiceType").append("<option value='YEARLY'>YEARLY</option>");
        //$("#InvoiceType").select2({ placeholder: "Select Invoice Type", width: null });

        var id = "#InvoiceType";
        $.ajax({
            url: "/api/mstDataSource/InvoiceType",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + parseInt($.trim(item.mstInvoiceTypeId)) + "'>" + item.Description + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Invoice Type", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingSelectCurrency: function (FieldId) {
        FieldId = FieldId;
        $(FieldId).html("<option></option>")
        $(FieldId).append("<option value='IDR'>IDR</option>");
        $(FieldId).append("<option value='USD'>USD</option>");
        $(FieldId).val('IDR').trigger('change');
        $(FieldId).select2({ placeholder: "Select", width: null });
    },

    BindingSelectRegional: function () {
        $.ajax({
            url: "/api/MstDataSource/Regional",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $("#OprCoresRegion").html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $("#OprCoresRegion").append("<option value='" + $.trim(item.RegionalId) + "'>" + item.Regional + "</option>");
                    })
                }
                $("#OprCoresRegion").select2({ placeholder: "Select Regional", width: null });

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingSelectBapsType: function () {
        var id = "#BapsType";
        $.ajax({
            url: "/api/mstDataSource/BapsType",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        if (Activitys != "New") {
                            $(id).append("<option value='" + $.trim(item.mstBapsTypeId) + "'>" + item.BapsType + "</option>");
                        }
                        else {
                            if (Helper.IsElementExistsInArray(parseFloat($.trim(item.mstBapsTypeId)), BapsTypeSoNumber)) {
                                $(id).append("<option disabled='disabled' value='" + $.trim(item.mstBapsTypeId) + "'>" + item.BapsType + "</option>");
                            }
                            else {
                                $(id).append("<option value='" + $.trim(item.mstBapsTypeId) + "'>" + item.BapsType + "</option>");
                            }
                        }
                    })
                    //$(id).val(5).trigger('change');
                }

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingSelectPowerType: function () {
        var id = "#PowerType";
        $.ajax({
            url: "/api/mstDataSource/PowerType",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + parseInt($.trim(item.KodeType)) + "'>" + item.PowerType + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Power Type", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingDatePicker: function () {
        $(".datepicker").datepicker({
            format: "dd M yyyy"
        });
    },

    BindProRateAmount: function () {

        var CustomerID = $('#CustomerInvoice').val();
        var InvoiceStartDate = $('#StartEffectiveDateVal').val();
        var InvoiceEndDate = $('#EndEffectiveDateVal').val();
        var InvoiceAmount = $('#BaseLeasePriceVal').val().replace(/,/g, "");
        var ServiceAmount = $('#ServicePriceVal').val().replace(/,/g, "") == "" ? "0" : $('#ServicePriceVal').val().replace(/,/g, "");
        var DeductionAmount = "0";
        var InstallationAmount = $('#tbxInstallationAmount').val().replace(/,/g, "");
        var DropFODistance = $('#tbxBapsValidateDropFODistance').val().replace(/,/g, "");
        var StipCategorySelected = $('#StipCategory').text().trim();

        if (StipCategorySelected == "STIP 3") {
            var result = (parseFloat(InvoiceAmount) + parseFloat(ServiceAmount)).toString();
            $("#TotalAmount").val(Common.Format.CommaSeparation(result));
        }
        else if (StipCategorySelected == "STIP 10" && !DataSelected.ProrateFormulation) {
            var result = (parseFloat(InvoiceAmount) + parseFloat(ServiceAmount) + parseFloat(InstallationAmount)).toString();
            $("#TotalAmount").val(Common.Format.CommaSeparation(result));
        }
        else {
            var params = { CustomerID: CustomerID, StartInvoiceDate: InvoiceStartDate, EndInvoiceDate: InvoiceEndDate, InvoiceAmount: InvoiceAmount, ServiceAmount: ServiceAmount, DropFODistance: DropFODistance, ProductID: DataSelected.ProductID }

            $.ajax({
                url: "/api/ReconcileData/GetProRateAmount",
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false,
            })
            .done(function (data, textStatus, jqXHR) {
                var result = (parseFloat(data.data) - parseFloat(DeductionAmount)).toString();

                if (StipCategorySelected == "STIP 10" && DataSelected.ProrateFormulation) {
                    result = (parseFloat(data.data) - parseFloat(DeductionAmount) + parseFloat(InstallationAmount)).toString();
                }

                $("#TotalAmount").val(Common.Format.CommaSeparation(result));
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
        }

    },

    AlertWarning: function (sa_message) {
        swal({
            title: "Warning",
            text: sa_message,
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, Continue!",
            closeOnConfirm: false
        }, function () {

            // remove these events;
            window.onkeydown = null;
            window.onfocus = null;
        });
    },

    DateDiff: function (year1, year2, idResult) {
        year1 = new Date(year1);
        year2 = new Date(year2);
        var millisBetween = year1.getFullYear() - year2.getFullYear();
        $("#" + idResult).val(millisBetween);
    },

    ButtonCancelForm: function () {
        //Control.BindingSelectBulkNumber();
        $("#panelHeaderInfo").hide();
        $(".panelValidationForm").hide();
        $(".panelTab").fadeIn(2000);
        $(".filter").fadeIn(2000);
        $('#BapsType').val(null).trigger('change');
        $('#PowerType').val(null).trigger('change');
        $('#CompanyInvoice').val(null).trigger('change');
        $('#CustomerInvoice').val(null).trigger('change');
        $('#TotalAmount').val('');
        $('#RemarksVal').val('');
        $('#PoNumberVal').val('');
        $('#InitialPoAmountVal').val('');
        $('#TotalPoAmountVal').val('');
        $('#PoDateVal').val('');
        $('#MLAnumberVal').val('');
        $('#MLAdateVal').val('');
        $('#BaukNumberVal').val('');
        $('#BaukDateVal').val('');
        $('#StartLeasePeriodVal').val('');
        $('#EndLeasePeriodVal').val('');
        $('#EndEffectiveDateVal').val('');
        $('#BaseLeasePriceVal').val('');
        $('#ServicePriceVal').val('');
        $('#StartFreeRent').val('');
        $('#EndFreeRent').val('');
        $('#EndFreeRentRow').hide();
        $('#FreeRentRow').hide();
        $('#divProrateFormulation').hide();
        $('#divInstallationFee').hide();
        $('#divBapsValidateDropFODistance').hide();
        $("#BapsDateBlank").fadeIn();
        $("#chkProrateFormulation").bootstrapSwitch("state", false);
        $("#chkInstallationFee").bootstrapSwitch("state", false);
    },

    AlertDeleteRowTable: function (rowSelected) {
        swal({
            title: "Warning",
            text: "Are You Sure Delete This Row ?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, Continue!",
            closeOnConfirm: true
        }, function () {

            window.onkeydown = null;
            window.onfocus = null;
            $(rowSelected).closest('tr').remove();
        });
    },

    TabActive: function (index) {
        if (index == 2) {
            $(".fGroupBy").hide();
            $(".fProductType").hide();
            $(".fBapsType").hide();
            $(".fSoNumber").hide();
            $(".fsSiteID").hide();
            $(".fCustomer").hide();
            $(".fBuklyNumber").show();
        } else {
            $(".fBuklyNumber").hide();
            $(".fGroupBy").show();
            $(".fProductType").show();
            $(".fBapsType").show();
            $(".fSoNumber").show();
            $(".fsSiteID").show();
            $(".fCustomer").show();
        }
    },

    ApprValidate: function (id) {
        var value = $(id).val();
        if (value == "") {
            Common.Alert.Warning("Approval is missing !.");
            return false;
        } else {
            return true
        }
    },

    ReplaceDecimal: function (text) {
        var inputnumber = text.value.replace(',', '');
        text.value = Common.Format.FormatCurrency(inputnumber);
        Control.BindProRateAmount();
    },

    CommaSeparationOnly: function (text) {
        var inputnumber = text.value.replace(',', '');
        text.value = Common.Format.CommaSeparationOnly(inputnumber)
        Control.BindProRateAmount();
    },

    BindingNextStep: function () {

        var id = "#slNextStep";
        $.ajax({
            url: "/api/MstDataSource/NextActivity",
            type: "GET",
            data: { CurrentActivity: 14, CustomerID: $("#CustomerID").text() }
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Next Step", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingBapsTypeSoNumber: function (SoNumber) {
        BapsTypeSoNumber = [];
        $.ajax({
            url: "/api/MstDataSource/ListBapsTypeSoNumber",
            type: "GET",
            data: { SoNumber: SoNumber }
        })
            .done(function (data, textStatus, jqXHR) {

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        BapsTypeSoNumber.push(parseFloat(item.Value));
                    })
                }

                if (Activitys == "New") {
                    $('#BapsType').html('');
                    Control.BindingSelectBapsType();
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingSIROSoNumber: function (SoNumber, DataS) {
        var id = "#StipSiro";
        $.ajax({
            url: "/api/MstDataSource/GetSiroSoNumber",
            type: "GET",
            data: { SoNumber: SoNumber }
        })
            .done(function (data, textStatus, jqXHR) {

                $(id).html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        if (DataS != null && DataS == item.Value.toString())
                            $(id).append("<option value='" + item.Value + "' selected>" + item.Text + "</option>");
                        else
                            $(id).append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                    })
                }
                //$(id).select2({ placeholder: "Select SIRO", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    InvoiceTypeAddMonth: function () {
        if ($('#StartLeasePeriodVal').val() != null && $('#StartLeasePeriodVal').val() != "") {
            $.ajax({
                url: "/api/BAPSValidation/GetEndDate",
                type: "GET",
                data: { date: $('#StartLeasePeriodVal').val(), invoice: $('#InvoiceType').val() }
            })
                .done(function (data, textStatus, jqXHR) {
                    $('#EndEffectiveDateVal').val(Common.Format.ConvertJSONDateTime(data));
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    Common.Alert.Error(errorThrown);
                });
        }

        //tanggal = $('#StartLeasePeriodVal').val();
        //DateStart = new Date(tanggal);
        //var day = DateStart.getDate();
        //var month = DateStart.getMonth()-1;
        //var year = DateStart.getFullYear();
        //someDate = new Date();
        //numberOfDaysToAdd = -1;

        //Invoice = $('#InvoiceType').val();
        //if (Invoice == 1) {
        //    month = month + 2;
        //    someDate = new Date(year, month, day);
        //    someDate.setDate(someDate.getDate() + numberOfDaysToAdd);
        //}
        //else if (Invoice == 2) {
        //    month = month + 4;
        //    someDate = new Date(year, month, day);
        //    someDate.setDate(someDate.getDate() + numberOfDaysToAdd);
        //}
        //else {
        //    month = month + 13;
        //    someDate = new Date(year, month, day);
        //    someDate.setDate(someDate.getDate() + numberOfDaysToAdd);
        //}

        //result = someDate.toDateString(); //(month + 1).toString() + "/" + day + "/" + year;
        //$('#EndEffectiveDateVal').val(Common.Format.ConvertJSONDateTime(result));
        ////DateStart = DateStart.setMonth(DateStart.getMonth() + 3);
        ////$('#StartLeasePeriodVal').val(DateStart.toString("dd/MM/yyyy"));
    },

    BindSplitAmount: function (typeSplit) {
        if (document.getElementById("SplitBill").checked) {

            if (typeSplit != 1) {
                if ($("#AmountSplit").val() == null || $("#AmountSplit").val() == "" || $("#AmountSplit").val() == "0" || $("#AmountSplit").val() == "0.00") {
                    Common.Alert.Warning("Please Complete First Row Data of Split !");
                    return;
                }
            }

            var CustomerID = $('#CustomerInvoice').val();
            var InvoiceStartDate = (typeSplit == 1) ? $('#StartSplit').val() : $('#StartSplit2').val();
            var InvoiceEndDate = (typeSplit == 1) ? $('#EndSplit').val() : $('#EndSplit2').val();
            var InvoiceAmount = $('#BaseLeasePriceVal').val().replace(/,/g, "");
            var ServiceAmount = $('#ServicePriceVal').val().replace(/,/g, "");
            var DeductionAmount = "0";
            var a = Date.parse(InvoiceStartDate);

            if (Date.parse(InvoiceStartDate) > Date.parse(InvoiceEndDate)) {
                Common.Alert.Warning("Start Date Can`t bigger than End Date !");
                return;
            }

            if (typeSplit != 1) {
                if (Date.parse(InvoiceStartDate) <= Date.parse($('#EndSplit').val())) {
                    Common.Alert.Warning("Start Date Row 2 Must be Bigger than End Date Row 1 !");
                    return;
                }

                if (Date.parse(InvoiceEndDate) <= Date.parse($('#EndSplit').val())) {
                    Common.Alert.Warning("End Date Row 2 Must be Bigger than End Date Row 1 !");
                    return;
                }
            }

            if (Date.parse(InvoiceStartDate) > Date.parse($('#EndEffectiveDateVal').val())) {
                Common.Alert.Warning("Invalid Range Date !");
                return;
            }

            if (Date.parse(InvoiceStartDate) < Date.parse($('#StartEffectiveDateVal').val())) {
                Common.Alert.Warning("Invalid Range Date !");
                return;
            }

            if (Date.parse(InvoiceEndDate) > Date.parse($('#EndEffectiveDateVal').val())) {
                Common.Alert.Warning("Invalid Range Date !");
                return;
            }

            var params = { CustomerID: CustomerID, StartInvoiceDate: InvoiceStartDate, EndInvoiceDate: InvoiceEndDate, InvoiceAmount: InvoiceAmount, ServiceAmount: ServiceAmount }

            $.ajax({
                url: "/api/ReconcileData/GetProRateAmount",
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false,
            })
                .done(function (data, textStatus, jqXHR) {
                    var result = (parseFloat(data.data) - parseFloat(DeductionAmount)).toString();
                    if (typeSplit == 1) {
                        $("#AmountSplit").val(Common.Format.CommaSeparation(result));
                    }
                    else {
                        $("#AmountSplit2").val(Common.Format.CommaSeparation(result));
                    }

                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    Common.Alert.Error(errorThrown);
                });
        }

    },

    ValidateSplit: function (element, typeSplit) {
        if (document.getElementById("SplitBill").checked) {
            var InvoiceStartDate = element.value;
            var InvoiceEndDate = (typeSplit == 1) ? $('#EndSplit').val() : $('#EndSplit2').val();

            if (Date.parse(InvoiceStartDate) < Date.parse($('#StartEffectiveDateVal').val())) {
                Common.Alert.Warning("Invalid Range Date !");
                return;
            }

            if (typeSplit != 1) {
                if (Date.parse(InvoiceStartDate) <= Date.parse($('#EndSplit').val())) {
                    Common.Alert.Warning("Start Date Row 2 Must be Bigger than End Date Row 1 !");
                    return;
                }
            }

            if (Date.parse(InvoiceStartDate) > Date.parse($('#EndEffectiveDateVal').val())) {
                Common.Alert.Warning("Start Date Can`t Bigger Than End EffDate !");
                return;
            }

            if (InvoiceEndDate) {
                if (InvoiceEndDate != "" && Date.parse(InvoiceStartDate) > Date.parse(InvoiceEndDate)) {
                    Common.Alert.Warning("Start Date Can`t bigger than End Date !");
                    return;
                }

                if (InvoiceEndDate != "" && Date.parse(InvoiceStartDate) < Date.parse(InvoiceEndDate)) {
                    Control.BindSplitAmount(typeSplit);
                }
            }
        }
    },
    BindYear: function () {
        var date = new Date()
        var yearNow = date.getFullYear();
        var yearOld = yearNow - 10;
        $('#BaukYear').html('');
        $('#BaukYear').append("<option value='0'> ALL </option>")
        for (var i = yearNow; i > yearOld; i--) {
            $('#BaukYear').append("<option value='" + i + "'>" + i + "</option>")
        }

        if (!$('#DefaultFilter').val()) {
            $('#BaukYear').val(yearNow).trigger('change');
        }
    },
   
    GetDefaultPrice: function (SoNumber) {

        $.ajax({
            url: "/api/BAPSValidation/GetDefaultPrice",
            type: "GET",
            async: false,
            data: { SoNumber: SoNumber },
        }).done(function (data, textStatus, jqXHR) {
            $('#BaseLeasePriceVal').val(Common.Format.CommaSeparation(data.First5Year));
            $('#ServicePriceVal').val(Common.Format.CommaSeparation(data.OMPrice));
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
}

var Form = {

    Init: function () {
        Control.BindingSelectCompany();
        Control.BindingSelectOperator();
        Control.BindingSelectProductType();
        Control.BindingSelectGroupBy();
        Control.BindingSelectBapsType();
        Control.BindingDatePicker();
        Control.BindingSelectPowerType();
        Control.BindingSelectInvoiceType();
        //untuk validasi checkbox
        Data.RowSelected = [];
        Data.RowFormValidation = [];
        Data.RowSelectedSite = [];
        $(".panelBapsDone").hide();
        $(".panelValidationForm").hide();
        $("#panelHeaderInfo").hide();
        $('#tabStartBaps').tabs();
        /// Forms ///
        $("#partialBapsValidationForm").hide();

    },

    SetHeaderInfo: function (SiteID, SoNumber, CustomerID, CompanyID, CompanyName, CustomerSiteID, CustomerSiteName, BapsType) {
        $("#SiteIdTbg").text($.trim(SiteID));
        $("#SoNumber").text($.trim(SoNumber));
        $("#CustomerID").text($.trim(CustomerID));
        $("#CompanyID").text($.trim(CompanyID));
        $("#CompanyName").text($.trim(CompanyName));
        $("#CustomerSiteID").val($.trim(CustomerSiteID));
        //$("#BapsType").val($.trim(BapsType));
        $("#CustomerSiteName").val($.trim(CustomerSiteName));
    },

    BapsValidationForm: function (SoNumber, CustomerID, SIRO, CompanyID, MLANumber, MLADate, BaukNumber, BaukDate, POAmount, PoDate, CustomerInvoice) {

        Control.BindingSelectCurrency('#ServiceCryVal');
        Control.BindingSelectCurrency('#BaseLeaseCryVal');
        $(".panelTab").fadeOut();
        $(".filter").fadeOut();
        $('.panelValidationForm').css('visibility', 'visible');
        $(".panelValidationForm").fadeIn(1800);
        $("#partialBapsValidationForm").fadeIn(1800);
        $("#panelHeaderInfo").fadeIn(2000);
        $(".partialBapsValidationForm").fadeIn(2000);
        // =========================================================================================//
        $("#SoNumberVal").val($.trim(SoNumber));
        $("#CustomerIDVal").val($.trim(CustomerID));
        $("#SiroVal").val($.trim(SIRO));
        $("#CompanyInvoiceIDVal").val($.trim(CompanyID));
        $("#BapsTypeIDVal").val($("#slBapsTypeID").val());
        $("#MLAnumberVal").val(MLANumber);
        $("#MLAdateVal").val(Common.Format.ConvertJSONDateTime(MLADate));
        $("#BaukNumberVal").val($.trim(BaukNumber));
        $("#BaukDateVal").val(Common.Format.ConvertJSONDateTime(BaukDate));
        if (Activitys != "New") {
            $("#InitialPoAmountVal").val($.trim(POAmount));
            $("#PoDateVal").val(Common.Format.ConvertJSONDateTime(PoDate));
            $("#TotalPoAmountVal").val($.trim(POAmount));
            // =========================================================================================//

            // =========================================================================================//
            var params = { strSoNumber: $.trim(SoNumber), strCustomerId: CustomerInvoice, strStipSiro: $.trim(SIRO) };
            $.ajax({
                url: "/api/BAPSValidation/getBapsValidation",
                type: "POST",
                datatype: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                if (data != null) {
                    if (Common.CheckError.Object(data)) {
                        if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                            $("#IDTrxBapsVal").val($.trim(data.ID));
                            $("#PoNumberVal").val($.trim(data.InitialPONumber));
                            $("#StartLeasePeriodVal").val(Common.Format.ConvertJSONDateTime(data.StartBapsDate));
                            $("#EndLeasePeriodVal").val(Common.Format.ConvertJSONDateTime(data.EndBapsDate));
                            $("#InvoiceTypeIDVal").val(data.CompanyInvoiceId);
                            $("#StartEffectiveDateVal").val(Common.Format.ConvertJSONDateTime(data.StartEffectiveDate == null ? data.StartBapsDate : data.StartEffectiveDate));
                            $("#EndEffectiveDateVal").val(Common.Format.ConvertJSONDateTime(data.EndEffectiveDate));
                            $("#BaseLeasePriceVal").val($.trim(Common.Format.CommaSeparation(data.BaseLeasePrice)));
                            $("#ServicePriceVal").val($.trim(Common.Format.CommaSeparation(data.ServicePrice)));
                            $("#RemarksVal").val($.trim(data.Remark));
                            Control.BindProRateAmount();
                        } else {
                            Common.Alert.Warning(data.ErrorMessage);
                        }
                    }
                } else {
                    $("#IDTrxBapsVal").val('');
                    $("#StartLeasePeriodVal").val('');
                    $("#EndLeasePeriodVal").val('');
                    $("#InvoiceTypeIDVal").val('');
                    $("#StartEffectiveDateVal").val('');
                    $("#EndEffectiveDateVal").val('');
                    $("#BaseLeasePriceVal").val('');
                    $("#ServicePriceVal").val('');
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
            });
        }
    },

    BapsValidationSubmit: function () {
        var data = new Object();
        data.ID = $.trim($("#IDTrxBapsVal").val());
        data.SoNumber = $.trim($("#SoNumberVal").val());
        data.InitialPONumber = $.trim($("#PoNumberVal").val());
        data.InitialPODate = $.trim($("#PoDateVal").val());
        data.StipSiro = $.trim($("#StipSiro").val());
        data.mstBapsTypeID = $.trim($("#BapsType").val());
        data.PowerTypeID = $.trim($("#PowerType").val());
        data.mstCustomerInvoiceID = $.trim($("#InvoiceType").val());
        data.CompanyInvoiceId = $.trim($("#CompanyInvoice").val());
        data.CustomerId = $.trim($("#CustomerInvoice").val());
        data.StartEffectiveDate = $.trim($("#StartEffectiveDateVal").val());
        data.EndEffectiveDate = $.trim($("#EndEffectiveDateVal").val());
        data.BaseLeasePrice = $.trim($("#BaseLeasePriceVal").val());
        data.ServicePrice = $.trim($("#ServicePriceVal").val());
        data.BaseLeaseCurrency = $.trim($("#BaseLeaseCryVal").val());
        data.ServiceCurrency = $.trim($("#ServiceCryVal").val());
        data.StartBapsDate = $.trim($("#StartLeasePeriodVal").val());
        data.EndBapsDate = $.trim($("#EndLeasePeriodVal").val());
        data.Remark = $.trim($("#RemarksVal").val());

        data.CustomerSiteID = $.trim($("#CustomerSiteID").val());
        data.CustomerSiteName = $.trim($("#CustomerSiteName").val());
        data.BapsDate = $.trim($("#BapsDate").val());

        data.ProrateFormulation = DataSelected.ProrateFormulation;
        data.InstallationAmount = parseFloat($('#tbxInstallationAmount').val().replace(/,/g, ""));

        var FreeRent = new Object();
        if ($('#StartFreeRent').val()) {
            FreeRent.StartDate = $('#StartFreeRent').val();
            FreeRent.EndDate = $('#EndFreeRent').val();
            FreeRent.FreeRent = 1;
            FreeRent.SONumber = $.trim($("#SoNumberVal").val());
            FreeRent.StipSiro = $.trim($("#StipSiro").val());
            FreeRent.BapsType = $.trim($("#BapsType").val());
            FreeRent.PowerType = $.trim($("#PowerType").val());
        }
        else {
            FreeRent.FreeRent = 0;
        }

        var params = {
            strProductId: DataSelected.ProductID,
            bapsValidation: data,
            FreeRent: FreeRent
        };
        var l = Ladda.create(document.querySelector("#btnBapsValidationSave"));
        $.ajax({
            url: "/api/BAPSValidation/submitBapsValidation",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    Common.Alert.Success("Data Success Saved!");
                    Control.ButtonCancelForm();
                    Table.GridBaspValidation();
                } else {
                    Common.Alert.Warning(data.ErrorMessage);
                }
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        });

    },

    ClearRowSelected: function () {
        Data.RowSelected = [];
        Data.RowSelectedSite = [];
    },

    GetForm: function (SoNumber, CustomerID, SiteID, CompanyID, CompanyName, CustomerSiteID, BapsType, CustomerSiteName, ProductID, StipCategory, StipSiro) {
        var params = { strSoNumber: SoNumber, strSiteID: SiteID, strCustomerId: CustomerID, strProductId: ProductID, strStipCategory: StipCategory, strStipSiro: StipSiro };
        $.ajax({
            url: "/api/BAPSValidation/getFormValidationPrint",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (data != '') {
                Form.SetHeaderInfo(SiteID, SoNumber, CustomerID, CompanyID, CompanyName, CustomerSiteID, CustomerSiteName, BapsType)
                $(".panelTab").fadeOut();
                $(".filter").fadeOut();
                $('.panelValidationForm').css('visibility', 'visible');
                $(".panelValidationForm").fadeIn(1800);
                $("#panelHeaderInfo").fadeIn(2000);
                jQuery.globalEval(data);
            } else {
                Common.Alert.Warning("Form Not Found !");
            }
        });

    },

    ApproveNextStep: function () {

        var params = {
            SoNumber: DataSelected.SoNumber,
            mstRAActivityID: $('#slNextStep').val(),
            PowerType: $.trim($("#PowerType").val()),
            SIRO: DataSelected.SIRO,
            BapsType: DataSelected.PoType
        };
        $.ajax({
            url: "/api/BAPSValidation/ApproveBAPSValidation",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
        })
            .done(function (data, textStatus, jqXHR) {
                if (Common.CheckError.Object(data)) {
                    if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                        Form.BapsValidationSubmit();
                        Table.GridBapsData();
                        $('#mdlConfirmNextProcess').modal('hide');
                    } else {
                        $('#mdlConfirmNextProcess').modal('hide');
                        Common.Alert.Warning(data.ErrorMessage);

                    }
                }

            }).fail(function (jqXHR, textStatus, errorThrown) {
                $('#mdlConfirmNextProcess').modal('hide');
                Common.Alert.Error(errorThrown)

            });
    },

    EffDate: function (dt) {
        $('#StartEffectiveDateVal').val(Common.Format.ConvertJSONDateTime(dt.value));
    },

}

var Table = {

    Init: function (id) {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        $(id).dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });


        $(window).resize(function () {
            $("#tblBapsValidation").DataTable().columns.adjust().draw();
        });
    },

    GridBaspValidation: function () {
        fsCustomerId = $("#slSearchCustomerID").val() == ' ' ? "" : $("#slSearchCustomerID").val();

        if (!$('#DefaultFilter').val() && fsCustomerId == '') {
            Common.Alert.Warning("Please select Customer!");
            return false;
        }

        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();


        fsCompanyId = $("#slSearchCompanyID").val() == null ? "" : $("#slSearchCompanyID").val();
        fsProductID = $("#slSearchProductID").val() == null ? "" : $("#slSearchProductID").val();
        fsSiteID = $("#slSearchSiteID").val() == null ? "" : $("#slSearchSiteID").val();
        fsSoNumb = $("#slSearchSoNumber").val() == null ? "" : $("#slSearchSoNumber").val();

        var params = {
            strCustomerId: fsCustomerId,
            strCompanyId: fsCompanyId,
            strProductId: fsProductID,
            strSoNumber: fsSoNumb,
            strSiteID: fsSiteID,
            strAction: '14',
            strTenantTypeID: 'validation',
            strBaukDoneYear: $('#BaukYear').val()
        };
        Table.Init("#tblBapsValidation");
        var tblProcess = $("#tblBapsValidation").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/BAPSValidation/gridSonumbList",
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
            "lengthMenu": [[25, 50, 100, 200], ['25', '50', '100', '200']],
            "destroy": true,
            "columns": [
                { data: "RowIndex" },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<i class='fa fa-edit btn btn-xs green link-TrxBaspValidation'></i>"
                        return strReturn;
                    }
                },
                { data: "SoNumber" },
                { data: "CustomerID" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CompanyID" },
                { data: "CompanyName" },
                { data: "SIRO" },
                { data: "STIPNumber" },
                { data: "StipCode" },
                { data: "Product" },
                {
                    data: "MLADate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "MLANumber" },
                { data: "BaukNumber" },
                {
                    data: "BaukDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "PONumber" },
                {
                    data: "POInputDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "PoDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblProcess.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        $("#Row" + item).addClass("active");
                    }
                }
            },
            "order": [],
            "scrollY": 300, /* Enable vertical scroll to allow fixed columns */
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
        });
        $("#tblBapsValidation tbody").unbind();
        $("#tblBapsValidation tbody").on("click", ".link-TrxBaspValidation", function (e) {
            Activitys = "New";
            $(".DocumentBaukList").hide();
            $('#formDetailData').parsley().reset();
            var table = $("#tblBapsValidation").DataTable();
            var row = table.row($(this).parents('tr')).data();
            DataSelected = row;
            $('#StipCategory').text(row.StipCode);
            $('#StipSiro').val(row.SIRO);
            $('#StipSiro').removeAttr('disabled');

            var WhereClause = "SoNumber = '" + row.SoNumber + "' AND StipSiro = ISNULL(" + row.SIRO + ",0) ";

            Control.BindingSIROSoNumber("SoNumber = '" + row.SoNumber + "'");

            Control.BindingBapsTypeSoNumber(WhereClause);

            Form.SetHeaderInfo(row.SiteID, row.SoNumber, row.CustomerID, row.CompanyID, row.CompanyName, row.CustomerSiteID, row.CustomerSiteName, fsBapsType)

            $('#SiteName').text(row.SiteName);

            Form.BapsValidationForm(row.SoNumber, row.CustomerID, row.SIRO, row.CompanyID, row.MLANumber, row.MLADate, row.BaukNumber, row.BaukDate, row.PoAmount, row.PoDate, "");
            $('#pwr').hide();
            $('#CustomerInvoice').val(row.CustomerID).trigger('change');
            $('#CompanyInvoice').val(row.CompanyID).trigger('change');
            $('#InvoiceType').val(row.InvoiceType).trigger('change');
            $('#StartEffectiveDateVal').val('');
            $('#btnProcessData').hide();
            $('#btnBapsValidationSave').show();
            DataSelected.ID = 0;
            $("#IDTrxBapsVal").val("0");
            $("#BapsType").removeAttr("disabled");
            if (row.CustomerID != "TSEL" && row.CustomerID != "ISAT") {
                $("#BaseLeasePriceVal").removeAttr("disabled");
                $("#ServicePriceVal").removeAttr("disabled");
            }

            $('#EndFreeRentRow').show();
            $('#FreeRentRow').show();

            $("#tbxInstallationAmount").val("0");

            if (row.StipID == 10) {
                $('#divProrateFormulation').show();
                $('#divInstallationFee').show();

                if (row.ProrateFormulation == 1) {
                    $("#chkProrateFormulation").bootstrapSwitch("state", true);
                } else {
                    $("#chkProrateFormulation").bootstrapSwitch("state", false);
                }
                
                if (row.InstallationAmount > 0) {
                    $('#tbxInstallationAmount').val(Common.Format.CommaSeparation(row.InstallationAmount))
                    $("#chkInstallationFee").bootstrapSwitch("state", true);
                } else {
                    $("#chkInstallationFee").bootstrapSwitch("state", false);
                }
            } else if (row.StipID == 1 && row.ProductID == 45) {
                $('#tbxBapsValidateDropFODistance').val(Common.Format.CommaSeparationOnly(row.DropFODistance))
                $('#divBapsValidateDropFODistance').show();
                $("#BapsDateBlank").hide();
            }


            Table.GridDocumentSupport(row.CompanyID, row.SiteID);
            Table.GridChecdocList(row.SoNumber, row.SiteID, row.CustomerID);
            Control.GetDefaultPrice(row.SoNumber);
            //$('#BaseLeasePriceVal').val(Common.Format.CommaSeparation(row.BaseLeasePrice));
            //$('#ServicePriceVal').val(Common.Format.CommaSeparation(row.OmPrice));
        });
    },

    GridBapsData: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsCustomerId = $("#slSearchCustomerID").val() == " " ? "" : $("#slSearchCustomerID").val();
        fsCompanyId = $("#slSearchCompanyID").val() == null ? "" : $("#slSearchCompanyID").val();
        fsProductID = $("#slSearchProductID").val() == null ? "" : $("#slSearchProductID").val();
        fsSiteID = $("#slSearchSiteID").val() == null ? "" : $("#slSearchSiteID").val();
        fsSoNumb = $("#slSearchSoNumber").val() == null ? "" : $("#slSearchSoNumber").val();

        var params = {
            strCustomerId: fsCustomerId,
            strCompanyId: fsCompanyId,
            strProductId: fsProductID,
            strSoNumber: fsSoNumb,
            strSiteID: fsSiteID,
            strAction: '14',
            strTenantTypeID: 'baps'
        };
        Table.Init("#tblBapsData");
        var tblProcess = $("#tblBapsData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/BAPSValidation/gridSonumbList",
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
            "lengthMenu": [[25, 50, 100, 200], ['25', '50', '100', '200']],
            "destroy": true,
            "columns": [
                { data: "RowIndex" },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<i class='fa fa-edit btn btn-xs green link-TrxBaspValidation'></i>"
                        return strReturn;
                    }
                },
                { data: "SoNumber" },
                { data: "TowerTypeID" },
                { data: "CustomerID" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CompanyID" },
                { data: "CompanyName" },
                { data: "SIRO" },
                { data: "STIPNumber" },
                { data: "StipCode" },
                { data: "Product" },
                {
                    data: "MLADate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "MLANumber" },
                { data: "BaukNumber" },
                {
                    data: "BaukDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "PONumber" },
                {
                    data: "POInputDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "PoDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "PoAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblProcess.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        $("#Row" + item).addClass("active");
                    }
                }
            },
            "order": [],
            "scrollY": 300, /* Enable vertical scroll to allow fixed columns */
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
        });
        $("#tblBapsData tbody").unbind();
        $("#tblBapsData tbody").on("click", ".link-TrxBaspValidation", function (e) {
            Activitys = "Update";
            $('#BapsType').html('');
            Control.BindingSelectBapsType();

            $('#formDetailData').parsley().reset();
            var table = $("#tblBapsData").DataTable();
            var row = table.row($(this).parents('tr')).data();
            DataSelected = row;
            Control.BindingSIROSoNumber("SoNumber = '" + row.SoNumber + "'", row.SIRO);
            //$('#StipSiro').val(row.SIRO.toString());
            $('#StipCategory').text(row.StipCode);

            Form.SetHeaderInfo(row.SiteID, row.SoNumber, row.CustomerID, row.CompanyID, row.CompanyName, row.CustomerSiteID, row.CustomerSiteName, fsBapsType)

            $('#SiteName').text(row.SiteName);
            $('#BapsType').val(row.PoType).trigger('change');
            $('#InvoiceType').val(row.InvoiceType).trigger('change');
            $('#BapsDate').val(row.BapsDate);
            $('#PowerType').val(row.PowerTypeID).trigger('change');

            $('#CustomerInvoice').val(row.CustomerInvoice).trigger('change');
            $('#CompanyInvoice').val(row.CompanyInvoice).trigger('change');

            Form.BapsValidationForm(row.SoNumber, row.CustomerID, row.SIRO, row.CompanyID, row.MLANumber, row.MLADate, row.BaukNumber, row.BaukDate, row.PoAmount, row.PoDate, row.CustomerInvoice);
            $('#btnProcessData').show();
            $('#btnBapsValidationSave').hide();
            $("#BapsType").attr("disabled", "disabled").off('click');
            $("#InvoiceType").attr("disabled", "disabled").off('click');
            if (row.CustomerID != "TSEL" && row.CustomerID != "ISAT") {
                $("#BaseLeasePriceVal").removeAttr("disabled");
                $("#ServicePriceVal").removeAttr("disabled");
            }
            //else {
            //    $("#BaseLeasePriceVal").attr("disabled", "disabled");
            //    $("#ServicePriceVal").attr("disabled", "disabled");
            //}

            $("#tbxInstallationAmount").val("0");

            if (row.StipID == 10) {
                $('#divProrateFormulation').show();
                $('#divInstallationFee').show();

                if (row.ProrateFormulation == 1) {
                    $("#chkProrateFormulation").bootstrapSwitch("state", true);
                } else {
                    $("#chkProrateFormulation").bootstrapSwitch("state", false);
                }

                if (row.InstallationAmount > 0) {
                    $('#tbxInstallationAmount').val(Common.Format.CommaSeparation(row.InstallationAmount))
                    $("#chkInstallationFee").bootstrapSwitch("state", true);
                } else {
                    $("#chkInstallationFee").bootstrapSwitch("state", false);
                }
            } else if (row.StipID == 1 && row.ProductID == 45) {
                $('#tbxBapsValidateDropFODistance').val(Common.Format.CommaSeparationOnly(row.DropFODistance))
                $('#divBapsValidateDropFODistance').show();
                $("#BapsDateBlank").hide();
            }

            $('#StipSiro').attr('disabled', 'disabled');
            Table.GridDocumentSupport(row.CompanyID, row.SiteID);
            Table.GridChecdocList(row.SoNumber, row.SiteID, row.CustomerID);
        });
       
    },

    GridWaitingPo: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsCustomerId = $("#slSearchCustomerID").val() == " " ? "" : $("#slSearchCustomerID").val();
        fsCompanyId = $("#slSearchCompanyID").val() == null ? "" : $("#slSearchCompanyID").val();
        fsProductID = $("#slSearchProductID").val() == null ? "" : $("#slSearchProductID").val();
        fsSiteID = $("#slSearchSiteID").val() == null ? "" : $("#slSearchSiteID").val();
        fsSoNumb = $("#slSearchSoNumber").val() == null ? "" : $("#slSearchSoNumber").val();

        var params = {
            strCustomerId: fsCustomerId,
            strCompanyId: fsCompanyId,
            strProductId: fsProductID,
            strSoNumber: fsSoNumb,
            strSiteID: fsSiteID,
            strAction: '14',
            strTenantTypeID: 'validation'
        };
        Table.Init("#tblWaitingPo");
        var tblProcess = $("#tblWaitingPo").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/BAPSValidation/gridWaitingInputPoList",
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
            "lengthMenu": [[25, 50, 100, 200], ['25', '50', '100', '200']],
            "destroy": true,
            "columns": [
                { data: "RowIndex" },
                //{
                //    orderable: false,
                //    mRender: function (data, type, full) {
                //        var strReturn = "";
                //        strReturn += "<i class='fa fa-edit btn btn-xs green link-TrxWaitingPo'></i>"
                //        return strReturn;
                //    }
                //},
                { data: "SoNumber" },
                { data: "TowerTypeID" },
                { data: "CustomerID" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CompanyID" },
                { data: "CompanyName" },
                { data: "SIRO" },
                { data: "StipCode" },
                { data: "Product" },
                { data: "PONumber" },
                { data: "PoAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                {
                    data: "PoDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblProcess.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        $("#Row" + item).addClass("active");
                    }
                }
            },
            "order": [],
            "scrollY": 300, /* Enable vertical scroll to allow fixed columns */
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
        });
    },

    Export: function () {
        fsCustomerId = $("#slSearchCustomerID").val() == " " ? "" : $("#slSearchCustomerID").val();
        fsCompanyId = $("#slSearchCompanyID").val() == null ? "" : $("#slSearchCompanyID").val();
        fsProductID = $("#slSearchProductID").val() == null ? "" : $("#slSearchProductID").val();
        fsSiteID = $("#slSearchSiteID").val() == null ? "" : $("#slSearchSiteID").val();
        fsSoNumb = $("#slSearchSoNumber").val() == null ? "" : $("#slSearchSoNumber").val();
        mstRAActivityID = $('#slSearchStatus').val();
        strTenantTypeID = 'validation';

        if (CurrentTab == 1) {
            strTenantTypeID = 'baps';
            window.location.href = "/RevenueAssurance/NewBaps/Export?strCustomerID=" + fsCustomerId + "&strCompanyId=" + fsCompanyId + "&strProductId=" + fsProductID
                + "&strSoNumber=" + fsSoNumb + "&strSiteID=" + fsSiteID + "&strTenantType=" + strTenantTypeID + "&mstRAActivityID=" + 14;
        } else if (CurrentTab == 2) {
            window.location.href = "/RevenueAssurance/BAPSWaitingPo/Export?strCustomerID=" + fsCustomerId + "&strCompanyId=" + fsCompanyId + "&strProductId=" + fsProductID
                + "&strSoNumber=" + fsSoNumb + "&strSiteID=" + fsSiteID + "&strTenantType=" + strTenantTypeID + "&mstRAActivityID=" + 14;
        }
        else {
            window.location.href = "/RevenueAssurance/BAPSValidation/Export?strCustomerID=" + fsCustomerId + "&strCompanyId=" + fsCompanyId + "&strProductId=" + fsProductID
                + "&strSoNumber=" + fsSoNumb + "&strSiteID=" + fsSiteID + "&strTenantType=" + strTenantTypeID + "&mstRAActivityID=" + 14 + "&strBaukDoneYear=" + $('#BaukYear').val();
        }

    },

    GridDocumentSupport: function (CompanyID, SiteID) {
        var params = { companyId: CompanyID, siteId: SiteID };
        var idTbl = "#AddDocumentSupport";
        Table.Init(idTbl);
        $(idTbl).DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "paging": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/CheckingDoc/getDocumentSupport",
                "type": "GET",
                "data": params,
            },
            "filter": false,
            "destroy": true,
            "columns": [
                { data: "No" },
                {
                    data: "DocumentName",
                    mRender: function (data) {
                        return "<a href='#' class='btn-Download'>" + data + "</a>"
                    }
                },
            ],
            "columnDefs": [[{ "targets": [0], "className": "text-center" }]],

        });
        $(idTbl + " tbody").unbind();
        $(idTbl + " tbody").on("click", ".btn-Download", function (e) {
            var table = $(idTbl).DataTable();
            var row = table.row($(this).parents('tr')).data();
            Helper.DownloadDocSupport(row.DocumentName, CompanyID, SiteID);
        });
    },

    GridChecdocList: function (SoNumber, SiteID, CustomerID) {
        var params = {
            strSoNumber: SoNumber,
            strSiteId: SiteID,
            strCustomerId: CustomerID
        };
        var idTbl = "#DocumentList";
        Table.Init(idTbl);
        $(idTbl).DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "paging": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/CheckingDoc/getCheckDocList",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            "filter": false,
            "destroy": true,
            "columns": [
                { data: "RowIndex" }
                , {
                    data: "DocName",
                    mRender: function (data, type, full) {
                        return "<a href='#' class='btn-Download' id='btDownload_" + full.RowIndex + "'>" + full.DocName + "</a>"
                    }
                }
            ],
            "columnDefs": [[{ "targets": [0], "className": "text-center" }]],
            
        });
        $(idTbl+" tbody").unbind();
        $(idTbl+" tbody").on("click", ".btn-Download", function (e) {
            var table = $(idTbl).DataTable();
            var row = table.row($(this).parents('tr')).data();
            Helper.DownloadBAUKDocument(row.FileName, row.LinkFile, row.IsLegacy);
        });
    },
}

var Helper = {
    IsElementExistsInArray: function (value, arr) {
        var isExist = false;
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == value) {
                isExist = true;
                break;
            }
        }
        return isExist;
    },
    DownloadBAUKDocument: function (fileName, path, IsLegacy) {
        var contentType = "application/pdf";
        window.location.href = "/RevenueAssurance/DownloadFileProject?FilePath=" + path + "&FileName=" + fileName + "&ContentType=" + contentType + "&IsLegacy=" + IsLegacy;
    },
    DownloadDocSupport: function (fileName, CompanyId, SiteId) {
        window.location.href = "/RevenueAssurance/DownloadDocumentSuppoert?fileName=" + fileName + "&companyId=" + CompanyId + "&siteId=" + SiteId;

    },
}

var FormEdit = {
    Init: function () {
        Data.TextboxEdit = {
            IsActive: false,
            DropFODistanceBackup: 0
        }

        if ($("#hdnAllowEdit").val()) {
            $(".btnBapsValidateEdit").removeAttr("disabled");
        }

        // Parsley
        $("#formDetailData").parsley();

        FormEdit.EventEdit();
        FormEdit.EventUndo();
        FormEdit.EventSave();
    },

    EventEdit: function () {
        $(".btnBapsValidateEdit").on("click", function () {
            var input = $("#" + this.id).parent().parent().find("input");

            if (input[0] != null) {
                FormEdit.InputBackup(input[0].id, input.val());
                input.removeAttr("disabled");
            }

            $("#" + this.id).hide();
            $("#" + this.id).parent().find(".btnBapsValidateUndo").fadeIn();
            $("#" + this.id).parent().find(".btnBapsValidateSave").fadeIn();

            Data.TextboxEdit.IsActive = true;
        });
    },

    EventUndo: function () {
        $(".btnBapsValidateUndo").on("click", function () {
            var input = $("#" + this.id).parent().parent().find("input");

            if (input[0] != null) {
                FormEdit.InputRestore(input[0].id);
                input.attr("disabled", "disabled");
            }

            $("#" + this.id).hide();
            $("#" + this.id).parent().find(".btnBapsValidateSave").hide();
            $("#" + this.id).parent().find(".btnBapsValidateEdit").fadeIn();

            Data.TextboxEdit.IsActive = false;
        });
    },

    EventSave: function () {
        $("#formDetailData").submit(function (e) {
            if (Data.TextboxEdit.IsActive) {
                var DropFODistance = $("#tbxBapsValidateDropFODistance").val().replace(/,/g, "");
                FormEdit.InputSave("DropFODistance", DropFODistance);
                $("#tbxBapsValidateDropFODistance").attr("disabled", "disabled");

                $("#btnBapsValidateDropFODistanceEdit").fadeIn();
                $("#btnBapsValidateDropFODistanceUndo").hide();
                $("#btnBapsValidateDropFODistanceSave").hide();
            }

            e.preventDefault();
        });
    },

    InputSave: function (fieldName, fieldValue) {
        var params = {
            strSONumber: $.trim($("#SoNumberVal").val()),
            strFieldName: fieldName,
            strFieldValue: fieldValue
        };

        App.blockUI({ target: ".divBapsValidateGroupBlock", animate: !0 });
        $.ajax({
            url: "/api/BAPSValidation/UpdateBAPSValidation",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Update Success");
            }
            else {
                if (fieldName == "DropFODistance") {
                    FormEdit.InputRestore("tbxBapsValidateDropFODistance");
                }
            }

            App.unblockUI(".divBapsValidateGroupBlock");
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);

            if (fieldName == "DropFODistance") {
                FormEdit.InputRestore("tbxBapsValidateDropFODistance");
            }
            
            App.unblockUI(".divBapsValidateGroupBlock");
        })

        Data.TextboxEdit.IsActive = false;
    },

    InputBackup: function (inputID, inputVal) {
        if (inputID == "tbxBapsValidateDropFODistance")
            Data.TextboxEdit.DropFODistanceBackup = inputVal;
    },

    InputRestore: function (inputID) {
        if (inputID == "tbxBapsValidateDropFODistance") {
            $("#" + inputID).val(Data.TextboxEdit.DropFODistanceBackup);
            $("#formDetailData").parsley().reset();
            Control.BindProRateAmount();
        }
    },
}
