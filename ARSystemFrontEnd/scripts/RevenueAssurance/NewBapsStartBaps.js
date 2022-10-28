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

jQuery(document).ready(function () {

    Form.Init();
    Table.GridBaspValidation();
    // ==================== Buttton Seacrh =============================//
    $("#btSearch").unbind().click(function () {
        if ($("#tabStartBaps").tabs('option', 'active') == 0) {
            Table.GridBaspValidation();
        }
        else if ($("#tabStartBaps").tabs('option', 'active') == 1) {
            Table.GridValidationPrint();
        } else if ($("#tabStartBaps").tabs('option', 'active') == 2) {
            Table.GriValidationListBulkyPrint();
        } else if ($("#tabStartBaps").tabs('option', 'active') == 3) {
            Table.Init("#tblPicaList");
            Table.GridPica();
        }

    });
    // ==================== End Buttton Seacrh =============================//
    $("#btReset").unbind().click(function () {
        $("#slSearchCustomerID").val("").trigger('change');
        $("#slSearchSiteID").val("").trigger('change');
        $("#slSearchSoNumber").val("").trigger('change');
        $("#slSearchCompanyID").val("").trigger('change');
        $("#slSearchProductID").val("").trigger('change');
        $("#slBulkyNumber").val("");
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

    $('#tabStartBaps').unbind().on('tabsactivate', function (event, ui) {
        var newIndex = ui.newTab.index();
        if (newIndex == 0) {
            Control.TabActive(newIndex)
            Table.GridBaspValidation();
            $(".panelPicaInput").fadeOut(100);
            $(".panelDataValidationPrint").fadeOut(100);
            $(".panelDataValidationBulkyPrint").fadeOut(100);
            $(".panelBapsValidation").fadeIn(1500);

        }
        if (newIndex == 1) {

            Table.GridValidationPrint();
            Control.TabActive(newIndex)
            $(".panelPicaInput").fadeOut(100);
            $(".panelDataValidationBulkyPrint").fadeOut(100);
            $(".panelBapsValidation").fadeOut(100);
            $(".panelDataValidationPrint").fadeIn(1500);
        } else if (newIndex == 2) {

            Table.GriValidationListBulkyPrint();
            Control.TabActive(newIndex)
            $(".panelPicaInput").fadeOut(100);
            $(".panelDataValidationPrint").fadeOut(100);
            $(".panelBapsValidation").fadeOut(100);
            $(".panelDataValidationBulkyPrint").fadeIn(1500);
        }
        else if (newIndex == 3) {
            Table.Init("#tblPicaList");
            //Table.GridPica();
            Control.TabActive(newIndex)
            $(".panelDataValidationPrint").fadeOut(100);
            $(".panelDataValidationBulkyPrint").fadeOut(100);
            $(".panelBapsValidation").fadeOut(100);
            $(".panelPicaInput").fadeIn(1500);
        }
    });

    $("#btnAddBulky").unbind().click(function () {
        $(".panelTab").hide();
        $(".filter").hide();
        $("#PartialiSatBulky").hide();
        $("#PartialXLBulky").hide();
        $("#PartialXLAddBulky").hide();
        $("#btResetBulky").attr('disabled', false);
        $("#slSearchCompanyNameBulky").prop("disabled", false);
        $("#slSearchCustomerNameBulky").prop("disabled", false);
        $("#slSearchStipCategoryBulky").prop("disabled", false);
        $("#slSearchCompanyNameBulky").val('').trigger('change');
        $("#slSearchCustomerNameBulky").val('').trigger('change');
        $("#SearchSoNumberBulky").val("");
        $("#SearchSiteIDBulky").val("");
        // ======================= XL STIP 3 ===================//
        $("#BulkID").val("");
        $("#BapsNumber").val("");
        $("#Remarks").val("");
        $("#PerformGR").val("");
        $("#TicketNumber").val("");
        $("#Description").val("");
        $("#GrRecievedDate").val("");
        // ======================= End XL STIP 3 ===================//        
        // ======================= XL STIP 2 ===================//
        $("#IDTrxXLAdd").val("");
        $("#SoNumberXLAdd").val("");
        $("#StipCategoryXLAdd").val("");
        $("#CompanyXLAdd").val("");
        $("#PriceLeaseXLAdd").val("");
        $("#StartLeaseDateXLAdd").val("");
        $("#StartLeaseDateXLAdd2").val("");
        $("#EndLeaseDateXLAdd").val("");
        $("#IDTrxDetailXLAdd").val("");
        $("#StipSiroXLAdd").val("");
        $("#SoNumberXLAdd").val("");
        $("#HeightSpaceXLAdd").val("");
        $("#CompanyInfoXLAdd").val("");
        $("#BakDateXLAdd").val("");        
        // ======================= End XL STIP 2 ===================//        
        Control.ButtonResetBulky("1");
        $(".panelValidationForm").fadeIn(1800);
        $("#partialBulkyForm").fadeIn(2000);


    });

    $("#btCancelBulky").unbind().click(function () {
        Control.ButtonCancelForm();
    });

    $("#btSearchBulky").unbind().click(function () {
        var msg = '';
        if ($("#slSearchCompanyNameBulky").val() == null || $("#slSearchCompanyNameBulky").val() == '') {
            msg = msg + "Company must be choosed! \n"
        }
        if ($("#slSearchCustomerNameBulky").val() == null || $("#slSearchCustomerNameBulky").val() == '') {
            msg = msg + "Customer must be choosed! \n"
        }
        if ($("#slSearchStipCategoryBulky").val() == null || $("#slSearchStipCategoryBulky").val() == '') {
            msg = msg + "STIP Category must be choosed! \n"
        }
        if ($("#slSearchStipCategoryBulky").val() == "STIP 2" && $("#SearchSoNumberBulky").val() == '') {
            msg = msg + "SO Number must be fill! \n"
        }
        if (msg != '') {
            Common.Alert.Warning(msg);
        } else {
            $("#PartialXLBulky").hide();
            $("#PartialiSatBulky").hide();
            $("#PartialXLAddBulky").hide();
            var SoNumber = $.trim($("#SearchSoNumberBulky").val());
            var CustomerID = $.trim($("#slSearchCustomerNameBulky").val());
            var CompanyID = $.trim($("#slSearchCompanyNameBulky").val());
            var StipCategory = $.trim($("#slSearchStipCategoryBulky").val());
            var SiteID = $.trim($("#SearchSiteIDBulky").val());
            Control.ButtonResetBulky("0");
            if (CustomerID == "XL" && StipCategory == "STIP 3") {
                Control.BindingSelectApprovalCustomer('#slApprCustomerXLBulky', CustomerID, '');
                Control.BndingSelectApprovalCompany('#slApprCompanyXLBulky');
                Form.XlBulkyForm(SoNumber, CustomerID, CompanyID, SiteID, $('#BulkID').val());

            }
            else if (CustomerID == "XL" && StipCategory == "STIP 2") {
                Control.BindingSelectApprovalCustomer('#slApprCustomerXLAdd1', CustomerID, '');
                Control.BindingSelectApprovalCustomer('#slApprCustomerXLAdd2', CustomerID, '');
                Control.BndingSelectApprovalCompany('#slApprCompanyXLAdd');
                Form.XLAddForm(SoNumber, CustomerID, CompanyID, StipCategory, SiteID, '0');
            }
        }
    });
    // ==================== End Buttton Action =============================//

});

var Control = {

    BindingSelectCompany: function () {
        var id = "#slSearchCompanyID";
        var id2 = "#slSearchCompanyNameBulky";
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
        var id2 = "#slSearchCustomerNameBulky";
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
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
        $("#InvoiceType").html("<option></option>")
        $("#InvoiceType").append("<option value='TRIWULAN'>TRIWULAN</option>");
        $("#InvoiceType").append("<option value='MONTHLY'>MONTHLY</option>");
        $("#InvoiceType").append("<option value='YEARLY'>YEARLY</option>");
        $("#InvoiceType").select2({ placeholder: "Select Invoice Type", width: null });
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
        var id = "#slBapsTypeID";
        $.ajax({
            url: "/api/StartBaps/getBapsTypeList",
            type: "GET",
            async: false
        })
       .done(function (data, textStatus, jqXHR) {
           $(id).html("<option></option>")
           if (Common.CheckError.List(data)) {
               $.each(data, function (i, item) {
                   $(id).append("<option value='" + $.trim(item.mstBapsTypeId) + "'>" + item.BapsType + "</option>");
               })
               $(id).val(5).trigger('change');
           }
           $(id).select2({ placeholder: "Select Baps Type", width: null });
       })
       .fail(function (jqXHR, textStatus, errorThrown) {
           Common.Alert.Error(errorThrown);
       });
    },

    BindingSelectApprovalCustomer: function (selectID, operatorId, regionId) {
        var selectId = selectID;
        var params = { strCustomerId: operatorId, strRegionId: regionId };
        $.ajax({
            url: "/api/StartBaps/getApprovalCustomer",
            type: "POST",
            data: params,
            async: false

        })
       .done(function (data, textStatus, jqXHR) {
           $(selectId).html("<option></option>")
           if (Common.CheckError.List(data)) {
               $.each(data, function (i, item) {

                   $(selectId).append("<option value='" + $.trim(item.ApprovalID) + "'>" + item.ApprovalName + "</option>");
               })
           }
           $(selectId).select2({ placeholder: "Select Aprroval", width: null });
       })
       .fail(function (jqXHR, textStatus, errorThrown) {
           Common.Alert.Error(errorThrown);
       });
    },

    BndingSelectApprovalCompany: function (selectID) {
        var selectId = selectID;
        $.ajax({
            url: "/api/StartBaps/getApprovalCompany",
            type: "POST",
            async: false
        })
       .done(function (data, textStatus, jqXHR) {
           $(selectId).html("<option></option>")
           if (Common.CheckError.List(data)) {
               $.each(data, function (i, item) {
                   $(selectId).append("<option value='" + $.trim(item.UserID) + "'>" + item.ApprovalName + "</option>");
               })
           }
           $(selectId).select2({ placeholder: "Select Aprroval", width: null });
       })
       .fail(function (jqXHR, textStatus, errorThrown) {
           Common.Alert.Error(errorThrown);
       });
    },

    FillFormValidation: function (operatorId) {
        var bapsType = $('#slsBapsType option:selected').val();
        var productType = $('#slProductType option:selected').val();
        var soNumber = $('#SoNumber').val();
        var stipSiro = $('#slGroupBy option:selected').val();

        var params = { strCustomerIdId: operatorId, strBapsType: bapsType, strStipSiro: stipSiro, strSoNumber: soNumber, strProductType: productType };
        $.ajax({
            url: "/api/StartBaps/fillForm",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        })
        .done(function (data, textStatus, jqXHR) {
            var htmlString = data.HtmlString;
            jQuery.globalEval(htmlString);
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
        $("#panelHeaderInfo").hide();
        $(".panelValidationForm").hide();
        $(".panelTab").fadeIn(2000);
        $(".filter").fadeIn(2000);
    },

    ClearSfIBSForm: function () {
        $("#SPKNumberSfIBS").val("");
        $("#SPKDateSfIBS").val("");
        $("#StartDateLeaseSfIBS").val("");
        $("#EndDateLeaseSfIBS").val("");
        $("#ATPDateSfIBS").val("");
        $("#SDADateSfIBS").val("");
        $("#RentalPeriodSfIBS").val("");
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

    PrintPDF: function (SoNumber, CustomerID, BulkID, PrintType, StipCategory) {

        window.location.href = "/RANewBaps/PrintBAPS?SoNumber=" + SoNumber + "&CustomerID=" + CustomerID + "&BulkID=" + BulkID + "&PrintType=" + PrintType + "&StipCategory=" + StipCategory + "";
    },

    ButtonResetBulky: function (value) {
        $("#btResetBulky").unbind().click(function () {
            if (value == "1") {
                $("#slSearchCompanyNameBulky").val("").trigger('change');
                $("#slSearchCustomerNameBulky").val("").trigger('change');
            }
            $("#SearchSoNumberBulky").val("");
            $("#SearchSiteIDBulky").val("");
        });
    },

    BindingStipCategory: function () {
        $("#slSearchStipCategoryBulky").html("<option></option>")
        $("#slSearchStipCategoryBulky").append("<option value='STIP 2' selected>STIP 2</option>");
        $("#slSearchStipCategoryBulky").append("<option value='STIP 3'>STIP 3</option>");
        $("#slSearchStipCategoryBulky").select2({ placeholder: "Select Group", width: null });

    },

    //PrintPDF: function (CustomerID, BulkID, PrintType, StipCategory) {

    //    window.location.href = "/RANewBaps/PrintBAPS?CustomerID=" + CustomerID + "&BulkID=" + BulkID + "&PrintType=" + PrintType + "&StipCategory=" + StipCategory + "";
    //},

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
    }
}

var Form = {

    Init: function () {
        Control.BindingSelectCompany();
        Control.BindingSelectOperator();
        Control.BindingSelectProductType();
        Control.BindingSelectGroupBy();
        Control.BindingSelectBapsType();
        Control.BindingDatePicker();
        Control.BindingStipCategory();
        //untuk validasi checkbox
        Data.RowSelected = [];
        Data.RowFormValidation = [];
        // Data.RowSelectedProcess = [];
        //untuk validasi table site
        Data.RowSelectedSite = [];
        // Data.RowSignatory = []
        $(".panelPicaInput").hide();
        $(".panelDataValidationPrint").hide();
        $(".panelDataValidationBulkyPrint").hide();
        $(".panelBapsDone").hide();
        $(".panelValidationForm").hide();
        $("#partialApproval").hide();
        $("#panelHeaderInfo").hide();
        $("#partialXLBulkyForm").hide();
        $('#tabStartBaps').tabs();
        /// Forms ///
        $("#partialBapsValidationForm").hide();
        $("#partialHcptForm").hide();
        $("#partialSfIBSForm").hide();
        $("#partialBulkyForm").hide();
        $("#panelPrintViewPDF").hide();
        $("#partialSfAddForm").hide();
        $("#partialXLAddForm").hide();
        $(".fBuklyNumber").hide();

    },

    SetHeaderInfo: function (SiteID, SoNumber, CustomerID, CompanyID, CompanyName, CustomerSiteID, CustomerSiteName, BapsType) {
        $("#SiteIdTbg").text($.trim(SiteID));
        $("#SoNumber").text($.trim(SoNumber));
        $("#CustomerID").text($.trim(CustomerID));
        $("#CompanyID").text($.trim(CompanyID));
        $("#CompanyName").text($.trim(CompanyName));
        $("#CustomerSiteID").text($.trim(CustomerSiteID));
        $("#BapsType").text($.trim(BapsType));
        $("#CustomerSiteName").text($.trim(CustomerSiteName));
    },

    BapsValidationForm: function (SoNumber, CustomerID, SIRO, CompanyID, MLANumber, MLADate, BaukNumber, BaukDate, POAmount, PoDate) {

        Control.BindingSelectCurrency('#ServiceCryVal');
        Control.BindingSelectCurrency('#BaseLeaseCryVal');
        $(".panelTab").fadeOut();
        $(".filter").fadeOut();
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
        $("#InitialPoAmountVal").val($.trim(POAmount));
        $("#PoDateVal").val(Common.Format.ConvertJSONDateTime(PoDate));
        $("#TotalPoAmountVal").val($.trim(POAmount));
        // =========================================================================================//
        $("#btnBapsValidationSave").unbind().click(function () {
            Form.BapsValidationSubmit();
        });
        $("#btnBapsValidationCancel").unbind().click(function () {
            $("#partialBapsValidationForm").hide();
            Control.ButtonCancelForm();
        });
        // =========================================================================================//
        var params = { strSoNumber: $.trim(SoNumber), strCustomerId: $.trim(CustomerID), strStipSiro: $.trim(SIRO) };
        $.ajax({
            url: "/api/StartBaps/getBapsValidation",
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
                        $("#StartEffectiveDateVal").val(Common.Format.ConvertJSONDateTime(data.StartEffectiveDate));
                        $("#EndEffectiveDateVal").val(Common.Format.ConvertJSONDateTime(data.EndEffectiveDate));
                        $("#BaseLeasePriceVal").val($.trim(data.BaseLeasePrice));
                        $("#ServicePriceVal").val($.trim(data.ServicePrice));
                        $("#RemarksVal").val($.trim(data.Remark));
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
    },

    BapsValidationSubmit: function () {
        var data = new Object();
        data.ID = $.trim($("#IDTrxBapsVal").val());
        data.SoNumber = $.trim($("#SoNumberVal").val());
        data.InitialPONumber = $.trim($("#PoNumberVal").val());
        data.InitialPODate = $.trim($("#PoDateVal").val());
        data.StipSiro = $.trim($("#SiroVal").val());
        data.mstBapsTypeID = $.trim($("#BapsTypeIDVal").val());
        data.PowerTypeID = $.trim($("#PowerTypeID").val());
        data.mstCustomerInvoiceID = $.trim($("#InvoiceTypeIDVal").val());
        data.CompanyInvoiceId = $.trim($("#CompanyInvoiceIDVal").val());
        data.CustomerId = $.trim($("#CustomerIDVal").val());
        data.StartEffectiveDate = $.trim($("#StartEffectiveDateVal").val());
        data.EndEffectiveDate = $.trim($("#EndEffectiveDateVal").val());
        data.BaseLeasePrice = $.trim($("#BaseLeasePriceVal").val());
        data.ServicePrice = $.trim($("#ServicePriceVal").val());
        data.BaseLeaseCurrency = $.trim($("#BaseLeaseCryVal").val());
        data.ServiceCurrency = $.trim($("#ServiceCryVal").val());
        data.StartBapsDate = $.trim($("#StartLeasePeriodVal").val());
        data.EndBapsDate = $.trim($("#EndLeasePeriodVal").val());
        data.Remark = $.trim($("#RemarksVal").val());
        var params = {
            bapsValidation: data
        };
        var l = Ladda.create(document.querySelector("#btnBapsValidationSave"));
        $.ajax({
            url: "/api/StartBaps/submitBapsValidation",
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

    XlBulkyForm: function (SoNumber, CustomerID, CompanyID, SiteID, BulkID) {

        $("#PartialXLAddBulky").hide();
        $("#PartialiSatBulky").hide();



        Table.GridSonumbXLBulky(CustomerID, CompanyID, SoNumber, SiteID);
        Table.GridXLBulky(BulkID);
        $("#PartialXLBulky").show();
        $("#btnXLBuklySave").unbind().click(function () {
            var result = false
            var countTblXL = $("#tblXLBulky").DataTable();
            var i = countTblXL.data().count();
            if (i > 0)
                result = true
            if (result == false) {
                Common.Alert.Warning("Detail List Is Nothing!.");
            }
            else {
                result = Control.ApprValidate("#slApprCustomerXLBulky");
                if (result == true)
                    result = Control.ApprValidate("#slApprCompanyXLBulky");
                if (result == true)
                    Form.SubmitXLBulky(CustomerID, CompanyID, SoNumber, SiteID);
            }
        });
        $("#btnXLBuklyCancel").unbind().click(function () {
            $(".panelValidationForm").fadeOut();
            $("#partialXLBulkyForm").fadeOut();
            $(".panelTab").fadeIn(2000);
            $(".filter").fadeIn(2000);
            Table.GriValidationListBulkyPrint();
        });
        $("#btnXLBulkyListSave").unbind().click(function () {
            Form.AddDetailXLBulky(CustomerID, CompanyID, SoNumber, SiteID);
        });
        $("#partialBulkyForm").fadeIn(1500);
        $("#PartialXLBulky").fadeIn(1000);


    },

    AddDetailXLBulky: function (CustomerID, CompanyID, SoNumber, SiteID) {
        var bulkID;
        var l = Ladda.create(document.querySelector("#btnXLBulkyListSave"));
        data = new Object();
        data.SoNumber = $("#SoNumberXL").val();
        data.SiteID = $("#SiteIDXL").val();
        data.SiteIDCustomer = $("#SiteIDCustomerXL").val();
        data.SiteNameCustomer = $("#SiteNameCustomnerXL").val();
        data.ID = $("#IDTrxXL").val();
        data.BulkyID = $("#BulkID").val();
        data.ProjectType = $("#ProjectType").val();
        data.EWO = $("#EWO").val();
        data.PO = $("#PO").val();
        data.RemarkFOP = $("#RemarkFOP").val();
        data.Material = $("#Material").val();
        data.MaterialDescription = $("#MaterialDesc").val();
        data.Qty = $("#Qty").val();
        data.Price = $("#Price").val();
        data.PoValue = $("#PoValue").val();
        data.Currency = $("#Currency").val();
        data.RemarkMML = $("#RemarkMML").val();
        data.Item = $("#Item").val();
        data.PoItem = $("#PoItem").val();
        data.Ess = $("#Ess").val();
        data.PoDate = $("#PoDate").val();
        data.AtpDate = $("#AtpDate").val();
        data.PicFop = $("#PicFop").val();
        data.SiteStatus = $("#SiteStatus").val();
        data.CustomerID = $("#CustomerIDXLBulk").val();
        data.CompanyID = $("#CompanyIDXLBulk").val();
        data.StipSiro = $("#StipSiroXL").val();
        var params = {
            XlBulky: data
        };

        $.ajax({
            url: "/api/StartBaps/addDetailXLBulky",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {

            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    $("#BulkID").val(data.BulkyID);
                    $("#mdlXLBulkySave").modal('hide');
                    Table.GridSonumbXLBulky(CustomerID, CompanyID, SoNumber, SiteID);
                    Table.GridXLBulky(data.BulkyID);
                    Common.Alert.Success("Data Success Confirmed!");
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

    SubmitXLBulky: function (CustomerID, CompanyID, SoNumber, SiteID) {
        var l = Ladda.create(document.querySelector("#btnXLBuklySave"));
        // ====================== declare ===================================//
        var fsStipCategory = $("#slSearchStipCategoryBulky").val();
        var fsBulkID = $("#BulkID").val();
        var fsBapsNumber = $("#BapsNumber").val();
        var fsRemarks = $("#Remarks").val();
        var fsApprIDCompany = $("#slApprCompanyXLBulky").val();
        var fsDescription = $("#Description").val();
        var fsApprIDCustomer1 = $("#slApprCustomerXLBulky").val();
        var fsGrRecievedDate = $("#GrRecievedDate").val();
        var fsPerformGR = $("#PerformGR").val();
        var fsTicketNumber = $("#TicketNumber").val();
        // ====================== end declare ===================================//
        var data = new Object();
        data.ID = fsBulkID;
        data.BulkNumber = fsBapsNumber;
        data.Noted = fsRemarks;
        data.ApprIDCompany = fsApprIDCompany;
        data.Description = fsDescription;
        data.ApprIDCustomer1 = fsApprIDCustomer1;
        data.GrRecievedDate = fsGrRecievedDate;
        data.PerformGR = fsPerformGR;
        data.TicketNumber = fsTicketNumber;
        data.CompanyID = CompanyID;
        data.CustomerID = CustomerID;
        data.StipCategory = fsStipCategory;
        var params = {
            validateBulky: data
        };
        $.ajax({
            url: "/api/StartBaps/submitXlBulky",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    Common.Alert.Success("Data Success Confirmed!");
                    $(".panelValidationForm").hide();
                    $("#partialXLBulkyForm").hide();
                    $(".panelTab").fadeIn(2500);
                    $(".filter").fadeIn(2500);
                    Table.GriValidationListBulkyPrint();
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

    GetDataXLBulkyDetail: function (BulkID) {
        var params = { strBulkID: BulkID };
        $.ajax({
            url: "/api/StartBaps/getDataXLBulkyDetail",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: params,
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    $("#BulkID").val(data.ID);
                    $("#BapsNumber").val(data.BulkNumber);
                    $("#Remarks").val(data.Noted);
                } else {
                    Common.Alert.Warning(data.ErrorMessage);
                }
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
    },

    DeleteDataXlBulkyDetail: function (ID, bulkID) {

        var params = { strBulkID: ID };

        $.ajax({
            url: "/api/StartBaps/deleteDetailXLBulky",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false

        }).done(function (data, textStatus, jqXHR) {

            Table.GridSonumbXLBulky();
            Table.GridXLBulky(bulkID);

        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)

        });

    },

    ClearRowSelected: function () {
        Data.RowSelected = [];
        Data.RowSelectedSite = [];
    },

    GetForm: function (SoNumber, CustomerID, SiteID, CompanyID, CompanyName, CustomerSiteID, BapsType, CustomerSiteName, ProductID, StipCategory, StipSiro) {
        var params = { strSoNumber: SoNumber, strSiteID: SiteID, strCustomerId: CustomerID, strProductId: ProductID, strStipCategory: StipCategory, strStipSiro: StipSiro };
        $.ajax({
            url: "/api/StartBaps/getFormValidationPrint",
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
                $("#partialBulkyForm").hide();
                $(".panelValidationForm").fadeIn(1800);
                $("#panelHeaderInfo").fadeIn(2000);
                jQuery.globalEval(data);
            } else {
                Common.Alert.Warning("Form Not Found !");
            }
        });

        // jQuery.globalEval("Form.SfAddForm('" + SoNumber + "','" + SiteIDTBG + "','" + CustomerID + "','" + StipCategory + "')");
        // jQuery.globalEval("Form.SfIBSForm('" + SoNumber + "','" + SiteIDTBG + "','" + CustomerID + "','" + StipCategory + "')");
    },

    //MainEquipmentRow: function () {
    //    var row = '<tr>';
    //    row = row + '<td><button type="button" class="btn red deleteRow" ><i class="glyphicon glyphicon-trash"></i></button></td>';
    //    row = row + '<td><input type="text" class="form-control model" /></td>';
    //    row = row + '<td><input type="text" class="form-control type" /></td>';
    //    row = row + '<td><input type="text" class="form-control qty" /></td>';
    //    row = row + '<td><input type="text" class="form-control height" /></td>';
    //    row = row + '<td><input type="text" class="form-control dimension" /></td>';
    //    row = row + '<td><input type="text" class="form-control azim" /></td>';
    //    row = row + '<td><input type="text" class="form-control qtyCable" /></td>';
    //    row = row + '<td><input type="text" class="form-control sizeCable" /></td>';
    //    row = row + '<td><input type="text" class="form-control value" /></td>';
    //    row = row + '<td><input type="text" class="form-control manuf" /></td>';
    //    row = row + '</tr>';
    //    return row;
    //},

    //HcptForm: function () {
    //    $("#tblMainEquipment tbody").append(Form.MainEquipmentRow);
    //    $("#partialHcptForm").fadeIn();
    //    $("#btnAddEquipment").unbind().click(function () {
    //        $("#tblMainEquipment tbody").append(Form.MainEquipmentRow);
    //    });
    //    $("#tblMainEquipment thead th").css("text-align", "center");
    //    $("#tblMainEquipment").on('click', '.deleteRow', function () {
    //        Control.AlertDeleteRowTable(this);
    //    });
    //    $("#btnHcptCancel").unbind().click(function () {
    //        Control.ButtonCancelForm();
    //        $("#partialHcptForm").fadeOut();
    //        $("#tblMainEquipment").dataTable({ "destroy": true, "data": [] });
    //    });
    //},

    SfIBSForm: function (SoNumber, SiteIDTBG, CustomerID, ProductID, StipCategory) {
        Control.BindingSelectApprovalCustomer('#slApprCustomerFsIBS', CustomerID, '');
        Control.BndingSelectApprovalCompany('#slApprCompanyFsIBS');

        var params = { strSoNumber: SoNumber, strSiteID: SiteIDTBG };
        $.ajax({
            url: "/api/StartBaps/getValidationData",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (data != null) {
                if (Common.CheckError.Object(data)) {
                    if (data != null) {
                        if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                            $("#IDTrxSfIBS").val(data.ID);
                            $("#SPKNumberSfIBS").val(data.SpkNumber);
                            $("#SPKDateSfIBS").val(Common.Format.ConvertJSONDateTime(data.SpkDate));
                            $("#StartDateLeaseSfIBS").val(Common.Format.ConvertJSONDateTime(data.StartPeriodDate));
                            $("#EndDateLeaseSfIBS").val(Common.Format.ConvertJSONDateTime(data.EndPeriodDate));
                            $("#ATPDateSfIBS").val(Common.Format.ConvertJSONDateTime(data.AtpDate));
                            $("#SDADateSfIBS").val(Common.Format.ConvertJSONDateTime(data.SdaDate));
                            $("#NumberOfAntennaSfIBS").val(data.NumberOfAntenna);;
                            $("#FrequencySfIBS").val(data.Frequency);;
                            $("#LeasePriceSfIBS").val(data.PricePerMonth);;
                            $("#slApprCompanyFsIBS").val(data.ApprIDCompany).trigger('change');
                            $("#slApprCustomerFsIBS").val(data.ApprIDCustomer1).trigger('change');
                            Control.DateDiff($("#EndDateLeaseSfIBS").val(), $("#StartDateLeaseSfIBS").val(), "RentalPeriodSfIBS");
                        } else {
                            Common.Alert.Warning(data.ErrorMessage);
                        }
                    }
                }
            }

            $("#partialSfIBSForm").fadeIn();
            $("#StartDateLeaseSfIBS").change(function () {
                Control.DateDiff($("#EndDateLeaseSfIBS").val(), $("#StartDateLeaseSfIBS").val(), "RentalPeriodSfIBS");
            });
            $("#EndDateLeaseSfIBS").change(function () {
                Control.DateDiff($("#EndDateLeaseSfIBS").val(), $("#StartDateLeaseSfIBS").val(), "RentalPeriodSfIBS");
            });
            $("#btnSfIBSSave").unbind().click(function () {
                Form.SfIBSSubmit(SoNumber, SiteIDTBG, CustomerID, ProductID, StipCategory);
            });
            $("#btnSfIBSCancel").unbind().click(function () {
                Control.ButtonCancelForm();
                Control.ClearSfIBSForm();
                $("#partialSfIBSForm").fadeOut();
            });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });

    },

    SfIBSSubmit: function (SoNumber, SiteID, CustomerID, ProductID, StipCategory) {

        var fsSPKNumber = $("#SPKNumberSfIBS").val();
        var fsSPKDate = $("#SPKDateSfIBS").val();
        var fsStartDateLease = $("#StartDateLeaseSfIBS").val();
        var fsEndDateLease = $("#EndDateLeaseSfIBS").val();
        var fsATPDate = $("#ATPDateSfIBS").val();
        var fsSDADate = $("#SDADateSfIBS").val();
        var fsNumberOfAntenna = $("#NumberOfAntennaSfIBS").val();
        var fsFrequency = $("#FrequencySfIBS").val();
        var fsPricePerMonth = $("#LeasePriceSfIBS").val();
        var fsIDtrx = $("#IDTrxSfIBS").val();

        fsApprIDCompany = $("#slApprCompanyFsIBS").val();
        fsApprIDCustomer1 = $("#slApprCustomerFsIBS").val();

        var data = new Object();
        data.ID = fsIDtrx;
        data.SpkNumber = fsSPKNumber;
        data.SpkDate = fsSPKDate;
        data.StartPeriodDate = fsStartDateLease;
        data.EndPeriodDate = fsEndDateLease;
        data.AtpDate = fsATPDate;
        data.SdaDate = fsSDADate;
        data.SoNumber = SoNumber;
        data.SiteID = SiteID;
        data.CustomerID = CustomerID;
        data.StipCategory = StipCategory;
        data.ApprIDCompany = fsApprIDCompany;
        data.ApprIDCustomer1 = fsApprIDCustomer1;
        data.ProductID = ProductID;
        data.PricePerMonth = fsPricePerMonth;
        data.Frequency = fsFrequency;
        data.NumberOfAntenna = fsNumberOfAntenna;

        var params = {
            validation: data
        };
        var l = Ladda.create(document.querySelector("#btnSfIBSSave"));
        $.ajax({
            url: "/api/StartBaps/submitValidation",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null) && (data.ValidateResult == "" || data.ValidateResult == null)) {
                    Common.Alert.Success("Data Success Saved!");
                    Control.ButtonCancelForm();
                    Control.ClearSfIBSForm();
                    $("#partialSfIBSForm").fadeOut();
                } else {
                    Common.Alert.Warning(data.ErrorMessage + " : " + data.ValidateResult);
                }
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        });
    },

    SfAddForm: function (SoNumber, SiteIDTBG, CustomerID, ProductID, StipCategory, StipSiro) {

        Control.BindingSelectApprovalCustomer('#slApprCustomerFsAdd', CustomerID, '');
        Control.BndingSelectApprovalCompany('#slApprCompanyFsAdd');
        $("#SoNumberSfAdd").val(SoNumber);
        $("#StipSiroSfAdd").val(StipSiro);
        $("#partialSfAddForm").fadeIn();
        $("#btnSfAddSave").unbind().click(function () {
            Form.SfAddSbumit(SoNumber, SiteIDTBG, CustomerID, ProductID, StipCategory, StipSiro);
        });
        $("#btnSfAddCancel").unbind().click(function () {
            Control.ButtonCancelForm();
            $("#partialSfAddForm").fadeOut();
        });
        $("#btnAddEquipmentSfAdd").unbind().click(function () {
            $("#IdEquipSfAdd").val("");
            $("#AntennaTypeSfAdd").val("");
            $("#QtySfAdd").val("");
            $("#DimensionSfAdd").val("");
            $("#ModelSfAdd").val("");
            $("#HeightSfAdd").val("");
            $("#mdlSfAddEquipmentSave").modal('show');
        });

        $("#btnEquipmentFsAddSave").unbind().click(function () {
            Form.SfAddEquipment();
        });
        $("#btnEquipmentFsAddCancel").unbind().click(function () {

        });

        Table.GridEquipment(SoNumber);

        var params = { strSoNumber: SoNumber, strSiteID: SiteIDTBG, strStipSiro: StipSiro };
        $.ajax({
            url: "/api/StartBaps/getValidationData",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (data != null) {
                if (Common.CheckError.Object(data)) {
                    if (data != null) {
                        if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                            $("#IDTrxSfAdd").val(data.ID);
                            $("#BapsNumberSfAdd").val(data.BapsNumber);
                            $("#ProposalNumberSfAdd").val(data.ProposalNumber);
                            $("#StartAdditionalLeaseDateSfAdd").val(Common.Format.ConvertJSONDateTime(data.StartAddLeaseDate));
                            $("#StartAdditionalLeaseDateSfAdd2").val(Common.Format.ConvertJSONDateTime(data.StartAddLeaseDate2));
                            $("#EndAdditionalLeaseDateSfAdd").val(Common.Format.ConvertJSONDateTime(data.EndAddLeaseDate));
                            $("#EndAdditionalLeaseDateSfAdd2").val(Common.Format.ConvertJSONDateTime(data.EndAddLeaseDate2));
                            $("#AdditionalLeaseSfAdd").val(data.AddPriceLease);
                            $("#AdditionalLeaseSfAdd2").val(data.AddPriceLease2);
                            $("#StartDateLeaseSfAdd").val(Common.Format.ConvertJSONDateTime(data.StartPeriodDate));
                            $("#EndDateLeaseSfAdd").val(Common.Format.ConvertJSONDateTime(data.EndPeriodDate));
                            $("#slApprCompanyFsAdd").val(data.ApprIDCompany).trigger('change');
                            $("#slApprCustomerFsAdd").val(data.ApprIDCustomer1).trigger('change');
                        } else {
                            Common.Alert.Warning(data.ErrorMessage);
                        }
                    }
                }
            }
            else {
                $("#IDTrxSfAdd").val("");
                $("#BapsNumberSfAdd").val("");
                $("#ProposalNumberSfAdd").val("");
                $("#StartAdditionalLeaseDateSfAdd").val("");
                $("#StartAdditionalLeaseDateSfAdd2").val("");
                $("#EndAdditionalLeaseDateSfAdd").val("");
                $("#EndAdditionalLeaseDateSfAdd2").val("");
                $("#AdditionalLeaseSfAdd").val("");
                $("#AdditionalLeaseSfAdd2").val("");
                $("#StartDateLeaseSfAdd").val("");
                $("#EndDateLeaseSfAdd").val("");
                $("#slApprCompanyFsAdd").val("");
                $("#slApprCustomerFsAdd").val("");
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });


    },

    SfAddSbumit: function (SoNumber, SiteIDTBG, CustomerID, ProductID, StipCategory, StipSiro) {

        var fsStartDateLease = $("#StartDateLeaseSfAdd").val();
        var fsEndDateLease = $("#EndDateLeaseSfAdd").val();
        var fsProposalNumber = $("#ProposalNumberSfAdd").val();
        var fsBapsNumber = $("#BapsNumberSfAdd").val();
        var fsStartAddLeaseDate = $("#StartAdditionalLeaseDateSfAdd").val();
        var fsEndAddLeaseDate = $("#EndAdditionalLeaseDateSfAdd").val();
        var fsStartAddLeaseDate2 = $("#StartAdditionalLeaseDateSfAdd2").val();
        var fsEndAddLeaseDate2 = $("#EndAdditionalLeaseDateSfAdd2").val();
        var fsAddLeasePrice = $("#AdditionalLeaseSfAdd").val();
        var fsAddLeasePrice2 = $("#AdditionalLeaseSfAdd2").val();
        fsIDTransaction = $("#IDTrxSfAdd").val();
        fsApprIDCompany = $("#slApprCompanyFsAdd").val();
        fsApprIDCustomer1 = $("#slApprCustomerFsAdd").val();

        var data = new Object();
        data.ID = fsIDTransaction;
        data.StartPeriodDate = fsStartDateLease;
        data.EndPeriodDate = fsEndDateLease;
        data.SoNumber = SoNumber;
        data.SiteID = SiteIDTBG;
        data.CustomerID = CustomerID;
        data.StipCategory = StipCategory;
        data.ApprIDCompany = fsApprIDCompany;
        data.ApprIDCustomer1 = fsApprIDCustomer1;
        data.BapsNumber = fsBapsNumber;
        data.ProposalNumber = fsProposalNumber;
        data.StartAddLeaseDate = fsStartAddLeaseDate;
        data.StartAddLeaseDate2 = fsStartAddLeaseDate2;
        data.EndAddLeaseDate = fsEndAddLeaseDate;
        data.EndAddLeaseDate2 = fsEndAddLeaseDate2;
        data.AddPriceLease = fsAddLeasePrice;
        data.AddPriceLease2 = fsAddLeasePrice2;
        data.StipSiro = StipSiro;

        var params = {
            validation: data
        };
        var l = Ladda.create(document.querySelector("#btnSfIBSSave"));
        $.ajax({
            url: "/api/StartBaps/submitValidation",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    Table.GridValidationPrint();
                    Common.Alert.Success("Data Success Saved!");
                    $("#partialSfAddForm").hide();
                    Control.ButtonCancelForm();
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

    SfAddEquipment: function () {
        var l = Ladda.create(document.querySelector("#btnEquipmentFsAddSave"));
        fsSoNumb = $("#SoNumberSfAdd").val();
        var fsStipSiro = $("#StipSiroSfAdd").val();
        var fsIDTrx = $("#IdEquipSfAdd").val();

        var fsAntennaType = $("#AntennaTypeSfAdd").val();
        var fsQty = $("#QtySfAdd").val();
        var fsDimension = $("#DimensionSfAdd").val();
        var fsModel = $("#ModelSfAdd").val();
        var fsHeight = $("#HeightSfAdd").val();
        var data = new Object();

        data.ID = fsIDTrx;
        data.SoNumber = fsSoNumb;
        data.MaterialType = fsAntennaType;
        data.Quantity = fsQty;
        data.Dimension = fsDimension;
        data.Model = fsModel;
        data.Height = fsHeight;
        data.StipSiro = fsStipSiro;

        var params = { equipment: data }
        $.ajax({
            url: "/api/StartBaps/addEquipment",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    Common.Alert.Success("Data Success Saved!");
                    $("#mdlSfAddEquipmentSave").modal('hide');
                    Table.GridEquipment(fsSoNumb);

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

    SfAddDeleteEquipment: function (ID, SoNumber) {
        var params = { strIDTrx: ID };
        $.ajax({
            url: "/api/StartBaps/deleteEquipment",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (data == true) {
                Common.Alert.Success("Data Success Deleted!");
                Table.GridEquipment(SoNumber);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
    },

    XLAddForm: function (SoNumber, CustomerID, CompanyID, StipCategory, SiteID, BulkID) {
        $("#PartialiSatBulky").hide();
        $("#PartialXLBulky").hide();
        Table.GridSonumbXLAdd(SoNumber, CustomerID, SiteID);
        Table.GridTrxXLAdd(BulkID, SoNumber, CustomerID, SiteID);
        $('#SearchSoNumberBulky').val(SoNumber);
        $("#btnXLAddSave").unbind().click(function () {
            Form.XLAddSubmit(SoNumber, CustomerID, CompanyID, StipCategory);
        });
        $("#btnXLAddCancel").unbind().click(function () {
            Control.ButtonCancelForm();
            $("#partialXLAddForm").fadeOut();
        });
        $("#PartialXLAddBulky").fadeIn(1500);
    },

    XlAddDetailSave: function (SoNumber, CustomerID, SiteID) {
        var foundation = $('#FoundationXlAdd').val();
        var foundationContract = $('#FoundationByContractXLAdd').val();
        var availableSpace = $('#AvailableSpaceXLAdd').val();
        var jenisAntenna = $('#JenisAntennaRRUXLAdd').val();
        var jumAntenna = $('#JumlahAntennaRRUXlAdd').val();
        var luasPermukaan = $('#LuasPermukaanRRUXlAdd').val();
        var luasPermukaanModern = $('#LuasPermukaanModernRRUXLAdd').val();
        var height = $('#KetinggianRRUXLAdd').val();
        var diameterMW = $('#DiameterMWXLAdd').val();
        var FoundationPrice = $('#FoundationPrice').val();
        var addPrice = $('#AdditionalPrice').val();
        jenisAntenna = jenisAntenna == "" ? $('#KetinggianRFXLAdd').val() : jenisAntenna;
        jenisAntenna = jenisAntenna == "" ? $('#KetinggianMWXLAdd').val() : jenisAntenna;
        jumAntenna = jumAntenna == "" ? $('#JumlahAntennaRFXlAdd').val() : jumAntenna;
        jumAntenna = jumAntenna == "" ? $('#JumlahAntennaMWXlAdd').val() : jumAntenna;
        height = height == "" ? $('#KetinggianRFXLAdd').val() : height;
        height = height == "" ? $('#KetinggianMWXLAdd').val() : height;
        FoundationPrice = FoundationPrice == "" ? $('#RRUPrice').val() : FoundationPrice;
        FoundationPrice = FoundationPrice == "" ? $('#RFPrice').val() : FoundationPrice;
        FoundationPrice = FoundationPrice == "" ? $('#MWPrice').val() : FoundationPrice;
        data = new Object();
        data.ID = $('#IDTrxDetailXLAdd').val();
        data.BulkyID = $('#IDTrxXLAdd').val();
        data.SoNumberAdd = $('#SoNumberXLAdd').val();
        data.StipSiro = $('#StipSiroXLAdd').val();
        data.AntennaCount = jumAntenna;
        data.Height = height;
        data.AntennaDiameter = diameterMW;
        data.SurfaceArea = luasPermukaan;
        data.SurfaceAreaModern = luasPermukaanModern;
        data.Foundation = foundation;
        data.FoundationByContract = foundationContract;
        data.AvailableSpace = availableSpace;
        data.Price = FoundationPrice;
        data.AddPrice = addPrice;
        var params = { xlAdditional: data, strCustomerId: $('#slSearchCustomerNameBulky').val(), strCompanyId: $('#slSearchCompanyNameBulky').val(), strSoNumber: $('#SearchSoNumberBulky').val() }
        var l = Ladda.create(document.querySelector("#btnXLAddDetailSave"));
        $.ajax({
            url: "/api/StartBaps/submitDetailXLAdd",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    $('#IDTrxXLAdd').val(data.BulkyID);
                    Table.GridSonumbXLAdd(SoNumber, CustomerID, SiteID);
                    Table.GridTrxXLAdd(data.BulkyID, SoNumber, CustomerID, SiteID)
                    Common.Alert.Success("Data Success Saved!");
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

    XLAddSubmit: function (SoNumber, CustomerID, CompanyID, StipCategory) {
        var fsIDTransaction = $("#IDTrxXLAdd").val();
        var fsPriceLease = $("#PriceLeaseXLAdd").val();
        var fsStartDateLease = $("#StartLeaseDateXLAdd").val();
        var fsStartDateLease2 = $("#StartLeaseDateXLAdd2").val();
        var fsEndDateLease = $("#EndLeaseDateXLAdd").val();
        var fsCompanyInfo = $("#CompanyInfoXLAdd").val();
        var fsHeightSpace = $("#HeightSpaceXLAdd").val();
        var fsBAKdate = $('#BakDateXLAdd').val();
        var data = new Object();
        data.ID = fsIDTransaction;
        data.StartPeriodDate = fsStartDateLease;
        data.EndPeriodDate = fsEndDateLease;
        data.PriceLease = fsPriceLease;
        data.ApprIDCompany = $("#slApprCompanyXLAdd").val();
        data.ApprIDCustomer1 = $("#slApprCustomerXLAdd1").val();
        data.ApprIDCustomer2 = $("#slApprCustomerXLAdd2").val();
        data.BulkNumber = SoNumber;
        data.CompanyID = $("#slSearchCompanyNameBulky").val(); // CompanyID;
        data.CustomerID = CustomerID;
        data.StipCategory = StipCategory;
        data.CompanyInfo = fsCompanyInfo;
        data.HeightSpace = fsHeightSpace;
        data.ContractDate = fsBAKdate;
        data.PoDate = fsStartDateLease2;
        var params = {
            validateBulky: data
        };
        var l = Ladda.create(document.querySelector("#btnXLAddSave"));
        $.ajax({
            url: "/api/StartBaps/submitXlBulky",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    Table.GriValidationListBulkyPrint();                    
                    Common.Alert.Success("Data Success Saved!");
                    $("#partialXLAddForm").hide();
                    Control.ButtonCancelForm();
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

    XLAddDelete: function (ID, BulkID, SoNumber, CustomerID, SiteID) {
        var params = { strIDTrx: ID };
        $.ajax({
            url: "/api/StartBaps/deleteTrxXlAddList",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (data == true) {
                Common.Alert.Success("Data Success Deleted!");
                Table.GridSonumbXLAdd(SoNumber, CustomerID, SiteID)
                Table.GridTrxXLAdd(BulkID, SoNumber, CustomerID, SiteID);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
    },

    ClearFormXlAddDetail: function () {

            $('#FoundationXlAdd').val('');
            $('#FoundationByContractXLAdd').val('');
            $('#AvailableSpaceXLAdd').val('');
            $('#FoundationPrice').val('');
            $('#JenisAntennaRRUXLAdd').val('');
            $('#JumlahAntennaRRUXlAdd').val('');
            $('#LuasPermukaanRRUXlAdd').val('');
            $('#LuasPermukaanModernRRUXLAdd').val('');
            $('#KetinggianRRUXLAdd').val('');
            $('#JenisAntennaRFXLAdd').val('');
            $('#JumlahAntennaRFXlAdd').val('');
            $('#KetinggianRFXLAdd').val('');
            $('#RFPrice').val('');
            $('#RRUPrice').val('');
            $('#MWPrice').val('');
            $('FoundationPrice').val();
            $('#JenisAntennaMWXLAdd').val('');
            $('#JumlahAntennaMWXlAdd').val('');
            $('#DiameterMWXLAdd').val('');
            $('#KetinggianMWXLAdd').val('');
        // ===========================================//
            $("#XlAddAntennaRRU").hide();
            $("#XlAddAntennaRF").hide();
            $("#XlAddLand").hide();
            $("#XlAddAntennaMW").hide();
    }
        //GetFormValidation: function (operatorId, soNumber) {
        //    $(".panelValidationForm").fadeIn();
        //    $(".panelTab").fadeOut();
        //    $(".filter").fadeOut();
        //    var projectInfoHTml = "<h5><b># Project Information</b></h5>   <input type='hidden' id='OperatorIdHD' /> <input type='hidden' id='SoNumber' />";
        //    var siteInfoHTml = "<h5><b># Site Information</b></h5>";
        //    var params = "";
        //    params = { strFormName: "PROJECT_INFO", strCustomerIdId: operatorId };
        //    $.ajax({
        //        url: "/api/StartBaps/formValidation",
        //        type: "POST",
        //        datatype: "json",
        //        contentType: "application/json",
        //        data: JSON.stringify(params),
        //        cache: false
        //    })
        //   .done(function (data, textStatus, jqXHR) {
        //       if (Common.CheckError.List(data)) {
        //           var count = 0;
        //           $.each(data, function (i, item) {
        //               if (item.FieldNameId == "OperatorApproval1")
        //                   count = count + 1;
        //               if (item.FieldNameId == "OperatorApproval2")
        //                   count = count + 1;
        //               if (item.FormName == "PROJECT_INFO") {
        //                   if (item.FieldType == 'dropdownlist') {
        //                       projectInfoHTml = projectInfoHTml + "<div class='form-group'>" +
        //                                         "<label class='col-md-4 control-label'>" + item.FieldName + "</label>" +
        //                                         "<div class='col-md-6'>" +
        //                                             "<select id='" + item.FieldNameId + "' class='form-control select2'></select>" +
        //                                         "</div>" +
        //                                     "</div>";
        //                   } else if (item.FieldType == 'text') {
        //                       if (item.FieldNameId == "BaseLeasePrice" || item.FieldNameId == "ServicePrice") {
        //                           projectInfoHTml = projectInfoHTml + "<div class='form-group'>" +
        //                                           "<label class='col-md-4 control-label'>" + item.FieldName + "</label>" +
        //                                           "<div class='col-md-2'>" +
        //                                              "<select id='" + item.FieldNameId + "Currency' class='form-control select2'></select>" +
        //                                           "</div>" +
        //                                           "<div class='col-md-4'>" +
        //                                               "<input type='number' id='" + item.FieldNameId + "' class='form-control'  " + item.ReadOnly + " />" +
        //                                           "</div>" +
        //                                       "</div>";
        //                       }
        //                       else {
        //                           projectInfoHTml = projectInfoHTml + "<div class='form-group'>" +
        //                                             "<label class='col-md-4 control-label'>" + item.FieldName + "</label>" +
        //                                             "<div class='col-md-6'>" +
        //                                                 "<input type='text' id='" + item.FieldNameId + "' class='form-control' " + item.ReadOnly + " />" +
        //                                             "</div>" +
        //                                         "</div>";
        //                       }
        //                   }
        //                   else if (item.FieldType == 'datepicker') {
        //                       projectInfoHTml = projectInfoHTml + "<div class='form-group'>" +
        //                                         "<label class='col-md-4 control-label'>" + item.FieldName + "</label>" +
        //                                         "<div class='col-md-6'>" +
        //                                             //"<input type='text' id='" + item.FieldNameId + "'   class='form-control datepicker' " + item.ReadOnly + " />" +
        //                                             "<input type='text' id='" + item.FieldNameId + "'   class='form-control datepicker' readonly />" +
        //                                         "</div>" +
        //                                     "</div>";
        //                   }
        //               } else {
        //                   if (item.FieldNameId == "VerticalArea" || item.FieldNameId == "MountainHeightArea") {
        //                       siteInfoHTml = siteInfoHTml + "<div class='form-group'>" +
        //                                     "<label class='col-md-3 control-label'>" + item.FieldName + "</label>" +
        //                                     "<div class='col-md-3'>" +
        //                                        "<input type='text' id='" + item.FieldNameId + "' class='form-control' " + item.ReadOnly + " /> " +
        //                                     "</div> " +
        //                                     "<div class='col-md-3'>" +
        //                                         "<input type='text' id='" + item.FieldNameId + "2' class='form-control' " + item.ReadOnly + " />" +
        //                                     "</div>" +
        //                                 "</div>";
        //                   }
        //                   if (item.FieldType == 'textarea') {
        //                       siteInfoHTml = siteInfoHTml + "<div class='form-group'>" +
        //                                      "<label class='col-md-3 control-label'>" + item.FieldName + "</label>" +
        //                                      "<div class='col-md-6'>" +
        //                                          "<textarea  id='" + item.FieldNameId + "' class='form-control' rows='3' " + item.ReadOnly + " /></textarea>" +
        //                                      "</div>" +
        //                                  "</div>";
        //                   }
        //                   else {
        //                       siteInfoHTml = siteInfoHTml + "<div class='form-group'>" +
        //                                       "<label class='col-md-3 control-label'>" + item.FieldName + "</label>" +
        //                                       "<div class='col-md-6'>" +
        //                                           "<input type='text' id='" + item.FieldNameId + "' class='form-control' " + item.ReadOnly + " />" +
        //                                       "</div>" +
        //                                   "</div>";
        //                   }
        //               }
        //           })
        //           $("#projectInfoForm").html("");
        //           $("#siteInfoForm").html("");
        //           $("#projectInfoForm").html(projectInfoHTml);
        //           $("#siteInfoForm").html(siteInfoHTml);
        //           $("#OperatorIdHD").val(operatorId);
        //           $("#SoNumber").val(soNumber);
        //           $("#BapsType").val($("#slsBapsType").val());
        //           Control.BindingSelectInvoiceType();
        //           Control.BindingDatePicker();
        //           Control.BindingSelectRegional();
        //           Control.BindingSelectCurrency("BaseLeasePrice");
        //           Control.BindingSelectCurrency("ServicePrice");
        //           $("#OprCoresRegion").unbind().change(function () {
        //               if (count > 0 && count < 2) {
        //                   Control.BindingSelectApprovalOpr("#OperatorApproval1", operatorId, $("#OprCoresRegion option:selected").val())
        //                   $("#OperatorApprName1").val("");
        //                   $("#OperatorApproval1").unbind().change(function () {
        //                       $("#OperatorApprName1").val($("#OperatorApproval1  option:selected").val());
        //                   });
        //               } else if (count > 1) {
        //                   Control.BindingSelectApprovalOpr("#OperatorApproval1", operatorId, $("#OprCoresRegion option:selected").val())
        //                   $("#OperatorApprName1").val("");
        //                   $("#OperatorApprName2").val("");
        //                   $("#OperatorApproval1").unbind().change(function () {
        //                       $("#OperatorApprName1").val($("#OperatorApproval1  option:selected").val());
        //                   });
        //                   Control.BindingSelectApprovalOpr("#OperatorApproval2", operatorId, $("#OprCoresRegion option:selected").val())
        //                   $("#OperatorApproval2").unbind().change(function () {
        //                       $("#OperatorApprName2").val($("#OperatorApproval2  option:selected").val());
        //                   });
        //               }
        //           });
        //           $("#InvoiceType").change(function () {
        //               Control.TotalLeasePriceCalculate($("#InvoiceType option:selected").val());
        //           });
        //           Control.FillFormValidation(operatorId);
        //       }
        //   })
        //   .fail(function (jqXHR, textStatus, errorThrown) {
        //       Common.Alert.Error(errorThrown);
        //   });
        //},

        //SubmitValidation: function (operatorId) {
        //    var params = "";
        //    params = { strFormName: "PROJECT_INFO", strCustomerIdId: operatorId };
        //    $.ajax({
        //        url: "/api/StartBaps/formValidation",
        //        type: "POST",
        //        datatype: "json",
        //        contentType: "application/json",
        //        data: JSON.stringify(params),
        //        cache: false
        //    })
        //  .done(function (data, textStatus, jqXHR) {
        //      if (Common.CheckError.List(data)) {
        //          var parameters = [];
        //          var value = [];
        //          var param;
        //          $.each(data, function (i, item) {
        //              if (item.FormName == "PROJECT_INFO") {
        //                  parameters.push(item.FieldNameId);
        //                  value.push($("#" + item.FieldNameId).val());
        //              }
        //          })
        //          param = { strParameterName: parameters, strValue: value }
        //          $.ajax({
        //              url: "/api/StartBaps/submitValidation",
        //              type: "POST",
        //              datatype: "json",
        //              contentType: "application/json",
        //              data: JSON.stringify(param),
        //              cache: false
        //          })
        //           .done(function (data, textStatus, jqXHR) {
        //               if (Common.CheckError.Object(data)) {
        //                   if ((data.ErrorMessage == "" || data.ErrorMessage == null) && (data.ValidateResult == "" || data.ValidateResult == null)) {
        //                       Common.Alert.Success("Data Success Confirmed!");
        //                       Form.DoneSubmited();
        //                   } else {
        //                       Common.Alert.Warning(data.ErrorMessage + " : " + data.ValidateResult);
        //                   }
        //               }
        //               l.stop();
        //           })
        //           .fail(function (jqXHR, textStatus, errorThrown) {
        //               Common.Alert.Error(errorThrown)
        //               l.stop();
        //           });
        //      }
        //  })
        //  .fail(function (jqXHR, textStatus, errorThrown) {
        //      Common.Alert.Error(errorThrown);
        //  });
        //},

        //DoneSubmited: function () {
        //    $(".panelValidationForm").fadeOut(100);
        //    $(".panelTab").fadeIn(100);
        //    $(".filter").fadeIn(100);
        //},

        //ConfirmProcess: function (status,soNumber) {
        //    var ddlCustomers = "";       
        //    if (status == "START_BAPS") {
        //        var customers = [
        //            { valueID: 0, descp: "Select" },
        //            { valueID: 1, descp: "Start BAPS" },
        //            { valueID: 2, descp: "PICA Input" },
        //            { valueID: 3, descp: "Data Validation" },
        //            { valueID: 4, descp: "BAPS(BAK) Done" }
        //        ];        
        //    } else {
        //        var customers = [
        //            { valueID: 0, descp: "Select" },
        //            { valueID: 2, descp: "PICA Input" },
        //            { valueID: 3, descp: "Data Validation" },
        //            { valueID: 4, descp: "BAPS(BAK) Done" }
        //        ];
        //    }
        //    ddlCustomers = $("#slNextAction");
        //    $(customers).each(function () {
        //        var option = $("<option />");
        //        //Set Customer Name in Text part.
        //        option.html(this.descp);
        //        //Set Customer CustomerId in Value part.
        //        option.val(this.valueID);
        //        //Add the Option element to DropDownList.
        //        ddlCustomers.append(option);
        //    });
        //}
}

var Table = {

    Init: function (idTable) {
        var tblSummary = $(idTable).dataTable({
            "filter": false,
            "destroy": true,
            "data": [],
            "proccessing": true,
            "language": {
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },


        });
        $(window).resize(function () {
            $(idTable).DataTable().columns.adjust().draw();
        });
    },

    GridBaspValidation: function () {
        Table.Init("#tblBapsValidation");
        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();
        fsCustomerId = $("#slSearchCustomerID").val() == null ? "" : $("#slSearchCustomerID").val();
        fsCompanyId = $("#slSearchCompanyID").val() == null ? "" : $("#slSearchCompanyID").val();
        fsProductID = $("#slSearchProductID").val() == null ? "" : $("#slSearchProductID").val();
        fsSiteID = $("#slSearchSiteID").val() == null ? "" : $("#slSearchSiteID").val();
        fsSoNumb = $("#slSearchSoNumber").val() == null ? "" : $("#slSearchSoNumber").val();
        fsGroupBy = $("#slGroupBy").val() == null ? "" : $("#slGroupBy").val();
        fsBapsType = $("#slBapsTypeID option:selected").text() == null ? "" : $("#slBapsTypeID  option:selected").text();
        var params = {
            strCustomerId: fsCustomerId,
            strCompanyId: fsCompanyId,
            strProductId: fsProductID,
            strGroupBy: fsGroupBy,
            strSoNumber: fsSoNumb,
            strSiteID: fsSiteID,
            strBapsType: fsBapsType,
            strAction: 'validation'
        };
        var tblList = $("#tblBapsValidation").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/StartBaps/gridSonumbList",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
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
                { data: "SIRO" },
                { data: "STIPNumber" },
                { data: "Product" },
                 {
                     data: "MLADate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                 { data: "MLANumber" },
                 { data: "BaukNumber" },
                 { data: "BaukDate" },
                 { data: "PoAmount" },
                 { data: "PoDate" },
                 { data: "CompanyName" }

            ],
            "columnDefs": [{ "targets": [0], "orderable": true }, { "targets": [18], "visible": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                l.stop();
                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        if (!Helper.IsElementExistsInArray(parseInt(item), Data.RowSelectedSite))
                            $("#Row" + item).addClass("active");
                    }
                }
            },
            "order": [1, "asc"],
            "scrollY": 300,
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
            var table = $("#tblBapsValidation").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $("#partialHcptForm").hide();
            $("#partialSfIBSForm").hide();
            $("#partialBulkyForm").hide();
            $("#partialSfAddForm").hide();
            Form.SetHeaderInfo(row.SiteID, row.SoNumber, row.CustomerID, row.CompanyID, row.CompanyName, row.CustomerSiteID, row.CustomerSiteName, fsBapsType)
            Form.BapsValidationForm(row.SoNumber, row.CustomerID, row.SIRO, row.CompanyID, row.MLANumber, row.MLADate, row.BaukNumber, row.BaukDate, row.PoAmount, row.PoDate);
        });
    },

    GridValidationPrint: function () {
        Table.Init("#tblValidationListPrint");
        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();
        fsCustomerId = $("#slSearchCustomerID").val() == null ? "" : $("#slSearchCustomerID").val();
        fsCompanyId = $("#slSearchCompanyID").val() == null ? "" : $("#slSearchCompanyID").val();
        fsProductID = $("#slSearchProductID").val() == null ? "" : $("#slSearchProductID").val();
        fsSiteID = $("#slSearchSiteID").val() == null ? "" : $("#slSearchSiteID").val();
        fsSoNumb = $("#slSearchSoNumber").val() == null ? "" : $("#slSearchSoNumber").val();
        fsGroupBy = $("#slGroupBy").val() == null ? "" : $("#slGroupBy").val();
        fsBapsType = $("#slBapsTypeID option:selected").text() == null ? "" : $("#slBapsTypeID  option:selected").text();
        var params = {
            strCustomerId: fsCustomerId,
            strCompanyId: fsCompanyId,
            strProductId: fsProductID,
            strGroupBy: fsGroupBy,
            strSoNumber: fsSoNumb,
            strSiteID: fsSiteID,
            strBapsType: fsBapsType,
            strAction: 'print',
            strDataType: "NonBulky"
        };
        var tblList = $("#tblValidationListPrint").DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },

            "ajax": {
                "url": "/api/StartBaps/gridSonumbList",// + idTable + "",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
               //{ extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
               //{
               //    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
               //        var l = Ladda.create(document.querySelector(".yellow"));
               //        l.start();
               //        Table.Export()
               //        l.stop();
               //    }
               //},
               //{ extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                { data: "RowIndex" },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<i class='fa fa-edit btn btn-xs green link-TrxDataValidation' data-toggle='modal'></i>";
                        if (full.BapsPrintInput == true) {
                            strReturn += "<i class='fa fa-print btn btn-xs blue link-PrintPDF'  data-toggle='modal'></i>";
                        }

                        return strReturn;
                    }
                },
                { data: "SoNumber" },
                { data: "CustomerID" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CompanyName" },
                { data: "SIRO" },
                { data: "STIPNumber" },
                { data: "Product" },
                { data: "StipCode" },
                 {
                     data: "MLADate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                 { data: "CompanyID" }

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [14], "visible": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                l.stop();
                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        if (!Helper.IsElementExistsInArray(parseInt(item), Data.RowSelectedSite))
                            $("#Row" + item).addClass("active");
                    }
                }
            },
            "order": [1, "asc"],
            "scrollY": 300,
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
        $("#tblValidationListPrint tbody").unbind();
        $("#tblValidationListPrint tbody").on("click", ".link-TrxDataValidation", function (e) {
            var table = $("#tblValidationListPrint").DataTable();
            var row = table.row($(this).parents('tr')).data();
            Form.GetForm($.trim(row.SoNumber), $.trim(row.CustomerID), $.trim(row.SiteID), $.trim(row.CompanyID), $.trim(row.CompanyName), $.trim(row.CustomerSiteID), $.trim(fsBapsType), $.trim(row.CustomerSiteName), $.trim(row.Product), $.trim(row.StipCode), $.trim(row.SIRO));
        });
        $("#tblValidationListPrint tbody").on("click", ".link-PrintPDF", function (e) {
            var table = $("#tblValidationListPrint").DataTable();
            var row = table.row($(this).parents('tr')).data();
            Control.PrintPDF($.trim(row.SoNumber), $.trim(row.CustomerID), '00', 'header', $.trim(row.StipCode));
        });
    },

    GriValidationListBulkyPrint: function () {
        Table.Init("#tblValidationListBulkyPrint");
        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();
        fsCompanyId = $("#slSearchCompanyID").val() == null ? "" : $("#slSearchCompanyID").val();
        var fsBulkNumber = $("#slBulkyNumber").val() == null ? "" : $("#slBulkyNumber").val();
        var params = {
            strCompanyId: fsCompanyId,
            strBulkNumber: fsBulkNumber
        };
        var tblList = $("#tblValidationListBulkyPrint").DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/StartBaps/gridValidationBulkyPrint",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
               //{ extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
               //{
               //    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
               //        var l = Ladda.create(document.querySelector(".yellow"));
               //        l.start();
               //        Table.Export()
               //        l.stop();
               //    }
               //},
               //{ extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                { data: "RowIndex" },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<i class='fa fa-edit btn btn-xs green link-TrxDataValidationBulky' data-toggle='modal' title='edit'></i> &nbsp;&nbsp;";
                        strReturn += "<i class='fa fa-print btn btn-xs blue link-PrintPDFHeader'  data-toggle='modal' title='Print Header'></i>&nbsp;&nbsp;";
                        if (full.StipCategory != "STIP 2" && full.CustomerID=="XL") {
                            strReturn += "<i class='fa fa-print btn btn-xs yellow link-PrintPDFDetails'  data-toggle='modal' title='Print Detail'></i>";
                        }
                        
                        return strReturn;
                    }
                },
                { data: "BulkNumber" },
                { data: "CompanyID" },
                { data: "CustomerID" },
                { data: "Noted" },
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "ID" },
                { data: "ApprIDCompany" },
                { data: "ApprIDCustomer1" },
                { data: "Description" },
                { data: "GrRecievedDate" },
                { data: "PerformGR" },
                { data: "TicketNumber" },
                { data: "StipCategory" },
                { data: "PriceLease" },
                { data: "StartPeriodDate" },
                { data: "EndPeriodDate" },
                { data: "PoDate" },
                { data: "ContractDate" },
                { data: "HeightSpace" },
                { data: "CompanyInfo" },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [7], "visible": false }, { "targets": [8], "visible": false }, { "targets": [9], "visible": false }
                            , { "targets": [10], "visible": false }, { "targets": [11], "visible": false }, { "targets": [12], "visible": false }, { "targets": [13], "visible": false },
                            { "targets": [14], "visible": false }, { "targets": [15], "visible": false }, { "targets": [16], "visible": false }, { "targets": [17], "visible": false },
                            { "targets": [18], "visible": false }, { "targets": [19], "visible": false }, { "targets": [20], "visible": false }, { "targets": [21], "visible": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                l.stop();
                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        if (!Helper.IsElementExistsInArray(parseInt(item), Data.RowSelectedSite))
                            $("#Row" + item).addClass("active");
                    }
                }
            },
            "order": [1, "asc"],
            "scrollY": 300,
            "scrollCollapse": true,
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
        });

        $("#tblValidationListBulkyPrint tbody").unbind();
        $("#tblValidationListBulkyPrint tbody").on("click", ".link-TrxDataValidationBulky", function (e) {
            var table = $("#tblValidationListBulkyPrint").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $(".panelTab").hide();
            $(".filter").hide();
            $("#partialBapsValidationForm").hide();
            $("#partialHcptForm").hide();
            $("#partialSfIBSForm").hide();
            $("#partialSfAddForm").hide();
            $("#slSearchCompanyNameBulky").val($.trim(row.CompanyID)).trigger('change');
            $("#slSearchCustomerNameBulky").val($.trim(row.CustomerID)).trigger('change');
            $("#slSearchCompanyNameBulky").prop("disabled", true);
            $("#slSearchCustomerNameBulky").prop("disabled", true);
            $("#slSearchStipCategoryBulky").prop("disabled", true);

            if (row.CustomerID == 'XL' && row.StipCategory == "STIP 3") {
                Control.BindingSelectApprovalCustomer('#slApprCustomerXLBulky', row.CustomerID, '');
                Control.BndingSelectApprovalCompany('#slApprCompanyXLBulky');
                $("#BulkID").val($.trim(row.ID));
                $("#BapsNumber").val($.trim(row.BulkNumber));
                $("#Description").val($.trim(row.Description));
                $("#GrRecievedDate").val(Common.Format.ConvertJSONDateTime(row.GrRecievedDate));
                $("#PerformGR").val($.trim(row.PerformGR));
                $("#TicketNumber").val($.trim(row.TicketNumber));
                $("#slApprCompanyXLBulky").val(row.ApprIDCompany).trigger('change');
                $("#slApprCustomerXLBulky").val(row.ApprIDCustomer1).trigger('change');
                $("#slSearchStipCategoryBulky").val(row.StipCategory).trigger('change');
                Form.XlBulkyForm('', row.CustomerID, row.CompanyID, '', row.ID);

            } else if (row.CustomerID == 'XL' && row.StipCategory == "STIP 2") {
                Control.BindingSelectApprovalCustomer('#slApprCustomerXLAdd1', row.CustomerID, '');
                Control.BindingSelectApprovalCustomer('#slApprCustomerXLAdd2', row.CustomerID, '');
                Control.BndingSelectApprovalCompany('#slApprCompanyXLAdd');
                $('#IDTrxXLAdd').val(row.ID);
                $('#PriceLeaseXLAdd').val(row.PriceLease);
                $('#StartLeaseDateXLAdd').val(Common.Format.ConvertJSONDateTime(row.StartPeriodDate));
                $('#EndLeaseDateXLAdd').val(Common.Format.ConvertJSONDateTime(row.EndPeriodDate));
                $('#StartLeaseDateXLAdd2').val(Common.Format.ConvertJSONDateTime(row.PoDate));
                $('#BakDateXLAdd').val(Common.Format.ConvertJSONDateTime(row.ContractDate));
                $('#HeightSpaceXLAdd').val(row.HeightSpace);
                $('#CompanyInfoXLAdd').val(row.CompanyInfo);
                Form.XLAddForm(row.BulkNumber, row.CustomerID, row.CompanyID, row.StipCategory, '', row.ID);
                $("#slApprCustomerXLAdd1").val(row.ApprIDCustomer1).trigger('change');
                $("#slApprCustomerXLAdd2").val(row.ApprIDCustomer2).trigger('change');
                $("#slApprCompanyXLAdd").val(row.ApprIDCompany).trigger('change');
                $("#slSearchStipCategoryBulky").val(row.StipCategory).trigger('change');
            } else {
                $("#partialBulkyForm").fadeIn(1500);
            }
            $(".panelValidationForm").fadeIn(1800);
            $("#partialBulkyForm").fadeIn(2000);
            Control.ButtonResetBulky("0");
        });

        $("#tblValidationListBulkyPrint tbody").on("click", ".link-PrintPDFHeader", function (e) {
            var table = $("#tblValidationListBulkyPrint").DataTable();
            var row = table.row($(this).parents('tr')).data();
          
            Control.PrintPDF("00", $.trim(row.CustomerID), $.trim(row.ID), "header", $.trim(row.StipCategory));
        });

        $("#tblValidationListBulkyPrint tbody").on("click", ".link-PrintPDFDetails", function (e) {
            var table = $("#tblValidationListBulkyPrint").DataTable();
            var row = table.row($(this).parents('tr')).data();
            if (row.CustomerID == 'XL')
                fsStipCategory = "STIP 3";
            Control.PrintPDF("00", $.trim(row.CustomerID), $.trim(row.ID), "details", $.trim(fsStipCategory));
        });

        $("#btResetBulky").attr('disabled', 'disabled');
    },

    GridSonumbXLBulky: function (CustomerID, CompanyID, SoNumber, SiteID) {
        Table.Init("#tblSonumbListXLBulky");
        var l = Ladda.create(document.querySelector("#btSearchBulky"));
        l.start();
        var params = {
            strCustomerId: CustomerID,
            strCompanyId: CompanyID,
            strSoNumber: SoNumber,
            strSiteID: SiteID,
            strAction: 'print',
            strDataType: "Bulky",
        };
        var tblList = $("#tblSonumbListXLBulky").DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },

            "ajax": {
                "url": "/api/StartBaps/gridSonumbList",// + idTable + "",
                "type": "POST",
                "datatype": "json",
                "data": params,
                "cache": false
            },
            buttons: [],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                { data: "RowIndex" },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<i class='fa fa-plus btn btn-xs green link-TrxDataValidationPrintBulky'  data-target='#mdlXLBulkySave' data-toggle='modal'></i>";
                        return strReturn;
                    }
                },
                { data: "SoNumber" },
                { data: "CustomerID" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CompanyName" },
                { data: "SIRO" },
                { data: "StipCode" },
                 {
                     data: "MLADate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 }
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                l.stop();
                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        if (!Helper.IsElementExistsInArray(parseInt(item), Data.RowSelectedSite))
                            $("#Row" + item).addClass("active");
                    }
                }
            },
            "order": [1, "asc"],
            "scrollY": 300,
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
        $("#tblSonumbListXLBulky tbody").unbind();
        $("#tblSonumbListXLBulky tbody").on("click", ".link-TrxDataValidationPrintBulky", function (e) {
            var table = $("#tblSonumbListXLBulky").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $("#IDTrxXL").val("");
            $("#SoNumberXL").val($.trim(row.SoNumber));
            $("#StipSiroXL").val($.trim(row.SIRO));
            $("#SiteIDXL").val($.trim(row.SiteID));
            $("#SiteIDCustomerXL").val($.trim(row.CustomerSiteID));
            $("#SiteNameCustomnerXL").val($.trim(row.CustomerSiteName));
        });
    },

    GridXLBulky: function (bulkID) {
        Table.Init("#tblXLBulky");
        var params = { strBulkID: $.trim(bulkID) };
        var tblList = $("#tblXLBulky").DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },

            "ajax": {
                "url": "/api/StartBaps/getDetailXLBulkyList",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [

                { data: "RowIndex" },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";

                        strReturn += "<i class='fa fa-pencil btn btn-xs green link-TrxEditListXLBulky' data-toggle='modal'></i> &nbsp;";
                        strReturn += "<i class='fa fa-trash btn btn-xs red link-TrxDeleteListXLBulky' data-toggle='modal'></i>";

                        return strReturn;
                    }
                },
                { data: "SoNumber" },
                { data: "SiteIDCustomer" },
                { data: "SiteNameCustomer" },
                { data: "ProjectType" },
                { data: "Ewo" },
                { data: "Po" },
                { data: "PoItem" },
                { data: "Qty" },
                { data: "Price" },
                { data: "PoValue" },
                { data: "Ess" },
                {
                    data: "AtpDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "PoDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },


                { data: "ID" },
                { data: "RemarkFop" },
                { data: "Material" },
                { data: "MaterialDescription" },
                { data: "Currency" },
                { data: "RemarkMML" },
                { data: "PicFop" },
                { data: "SiteID" },

                { data: "SiteStatus" },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false },
                            { "targets": [13], "visible": false },
                            { "targets": [14], "visible": false },
                            { "targets": [15], "visible": false }
                            , { "targets": [16], "visible": false },
                            { "targets": [17], "visible": false },
                            { "targets": [18], "visible": false },
                            { "targets": [19], "visible": false }
                            , { "targets": [20], "visible": false },
                            { "targets": [21], "visible": false },
                            { "targets": [22], "visible": false },
                            { "targets": [23], "visible": false }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            // "fnPreDrawCallback": function () { l.stop(); },
            //"fnPreDrawCallback": function () {
            //    App.blockUI({ target: ".panelSearchResult", boxed: true });
            //},
            //"fnDrawCallback": function () {

            //    if (Common.CheckError.List(tblList.data())) {
            //        $(".panelSearchBegin").hide();
            //        $(".panelSearchResult").fadeIn();
            //    }
            //    App.unblockUI('.panelSearchResult');

            //    if (Data.RowSelected.length > 0) {
            //        var item;
            //        for (var i = 0 ; i < Data.RowSelected.length; i++) {
            //            item = Data.RowSelected[i];
            //            if (!Helper.IsElementExistsInArray(parseInt(item), Data.RowSelectedSite))
            //                $("#Row" + item).addClass("active");
            //        }
            //    }
            //},
            "order": [1, "asc"],
            "scrollY": 300,
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
        $("#tblXLBulky tbody").unbind();
        $("#tblXLBulky tbody").on("click", ".link-TrxEditListXLBulky", function (e) {
            var table = $("#tblXLBulky").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $("#IDTrxXL").val($.trim(row.ID));
            $("#SoNumberXL").val($.trim(row.SoNumber));
            $("#SiteIDXL").val($.trim(row.SiteID));
            $("#SiteIDCustomerXL").val($.trim(row.SiteIDCustomer));
            $("#SiteNameCustomnerXL").val($.trim(row.SiteNameCustomer));
            $("#ProjectType").val($.trim(row.ProjectType));
            $("#EWO").val($.trim(row.Ewo));
            $("#PO").val($.trim(row.Po));
            $("#RemarkFOP").val($.trim(row.RemarkFop));
            $("#Material").val($.trim(row.Material));
            $("#MaterialDesc").val($.trim(row.MaterialDescription));
            $("#Qty").val($.trim(row.Qty));
            $("#Price").val($.trim(row.Price));
            $("#PoValue").val($.trim(row.PoValue));
            $("#Currency").val($.trim(row.Currency));
            $("#RemarkMML").val($.trim(row.RemarkMML));
            $("#Item").val($.trim(row.Item));
            $("#Ess").val($.trim(row.Ess));
            $("#PoItem").val($.trim(row.PoItem));
            $("#AtpDate").val(Common.Format.ConvertJSONDateTime(row.AtpDate));
            $("#PoDate").val(Common.Format.ConvertJSONDateTime(row.PoDate));
            $("#PicFop").val($.trim(row.PicFop));
            $("#SiteStatus").val($.trim(row.SiteStatus));
            $("#mdlXLBulkySave").modal('show');
            Control.BindingDatePicker();
        });

        $("#tblXLBulky tbody").on("click", ".link-TrxDeleteListXLBulky", function (e) {
            var table = $("#tblXLBulky").DataTable();
            var row = table.row($(this).parents('tr')).data();
            swal({
                title: "Warning",
                text: "Are You Sure Delete This Row ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, Continue!",
                closeOnConfirm: true
            }, function () {
                Form.DeleteDataXlBulkyDetail($.trim(row.ID), $.trim(bulkID));
            });
        });
    },

    GridEquipment: function (SoNumber) {
        var params = { strSoNumber: SoNumber };
        var tblList = $("#tblEquipmentListSfAdd").DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },

            "ajax": {
                "url": "/api/StartBaps/getDataEquipment",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                { data: "RowIndex" },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<i class='fa fa-pencil btn btn-xs green link-TrxEditListEquipmentSfAdd' data-toggle='modal'></i> &nbsp;";
                        strReturn += "<i class='fa fa-trash btn btn-xs red link-TrxDeletetListEquipmentSfAdd' data-toggle='modal'></i>";
                        return strReturn;
                    }
                },
                { data: "MaterialType" },
                { data: "Quantity" },
                { data: "Dimension" },
                { data: "Model" },
                { data: "Height" },
                { data: "ID" }
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [7], "visible": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            //"fnPreDrawCallback": function () {
            //    App.blockUI({ target: ".panelSearchResult", boxed: true });
            //},
            //"fnDrawCallback": function () {

            //    if (Common.CheckError.List(tblList.data())) {
            //        $(".panelSearchBegin").hide();
            //        $(".panelSearchResult").fadeIn();
            //    }
            //    App.unblockUI('.panelSearchResult');

            //    if (Data.RowSelected.length > 0) {
            //        var item;
            //        for (var i = 0 ; i < Data.RowSelected.length; i++) {
            //            item = Data.RowSelected[i];
            //            if (!Helper.IsElementExistsInArray(parseInt(item), Data.RowSelectedSite))
            //                $("#Row" + item).addClass("active");
            //        }
            //    }
            //},
            "order": [1, "asc"],
            "scrollY": 300,
            "scrollCollapse": true,
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },

        });
        $("#tblEquipmentListSfAdd tbody").unbind();
        $("#tblEquipmentListSfAdd tbody").on("click", ".link-TrxDeletetListEquipmentSfAdd", function (e) {
            var table = $("#tblEquipmentListSfAdd").DataTable();
            var row = table.row($(this).parents('tr')).data();
            swal({
                title: "Warning",
                text: "Are You Sure Delete This Row ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, Continue!",
                closeOnConfirm: true
            }, function () {
                Form.SfAddDeleteEquipment($.trim(row.ID), $.trim(SoNumber));
            });
        });
        $("#tblEquipmentListSfAdd tbody").on("click", ".link-TrxEditListEquipmentSfAdd", function (e) {
            var table = $("#tblEquipmentListSfAdd").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $("#IdEquipSfAdd").val($.trim(row.ID));
            $("#AntennaTypeSfAdd").val($.trim(row.MaterialType));
            $("#QtySfAdd").val($.trim(row.Quantity));
            $("#DimensionSfAdd").val($.trim(row.Dimension));
            $("#ModelSfAdd").val($.trim(row.Model));
            $("#HeightSfAdd").val($.trim(row.Height));
            $("#mdlSfAddEquipmentSave").modal('show');
        });

    },

    GridSonumbXLAdd: function (SoNumber, CustomerID, SiteID) {
        Table.Init("#tblSonumbAddListXLAdd");
        var l = Ladda.create(document.querySelector("#btSearchBulky"));
        l.start();
        var params = { strSoNumber: SoNumber, strCustomerId: CustomerID, strSiteID: SiteID };
        var tblList = $("#tblSonumbAddListXLAdd").DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },

            "ajax": {
                "url": "/api/StartBaps/getXlAddList",
                "type": "POST",
                "datatype": "json",
                "data": params,
                "cache": false
            },
            buttons: [],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                 { data: "RowIndex" },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<i class='fa fa-plus btn btn-xs green link-TrxSombListXLAdd' data-toggle='modal'></i>";
                        return strReturn;
                    }
                },
                { data: "SoNumberAdd" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "ProductType" },
                { data: "StipSiro" },
                { data: "StipCategory" }

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                l.stop();
                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        if (!Helper.IsElementExistsInArray(parseInt(item), Data.RowSelectedSite))
                            $("#Row" + item).addClass("active");
                    }
                }
            },
            "order": [1, "asc"],
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
        });
        $("#tblSonumbAddListXLAdd tbody").unbind();
        $("#tblSonumbAddListXLAdd tbody").on("click", ".link-TrxSombListXLAdd", function (e) {
            var table = $("#tblSonumbAddListXLAdd").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $("#StipSiroXLAdd").val(row.StipSiro);
            $("#SoNumberXLAdd").val(row.SoNumberAdd);
            Form.ClearFormXlAddDetail();
            if ($.trim(row.ProductType) == "ADDITIONAL LAND") {
                
                $("#XlAddLand").show();
            }
            else if ($.trim(row.ProductType) == "ADDITIONAL RRU") {
                $('#JenisAntennaRRUXLAdd').val(row.ProductType);
                $("#XlAddAntennaRRU").show();
            }
            else if ($.trim(row.ProductType) == "ADDITIONAL ANTENA RF") {
                $('#JenisAntennaRFXLAdd').val(row.ProductType);
                $("#XlAddAntennaRF").show();
            }
            else if ($.trim(row.ProductType) == "ADDITIONAL ANTENA MW") {
                $('#JenisAntennaMWXLAdd').val(row.ProductType);
                $("#XlAddAntennaMW").show();
            }
            $("#btnXLAddDetailSave").unbind().click(function () {
                Form.XlAddDetailSave(SoNumber, CustomerID, SiteID);
            });
            $("#btnXLAddDetailCancel").unbind().click(function () {

            });
            $("#mdlXLAddSave").modal('show');
        });
    },

    GridTrxXLAdd: function (BulkID, SoNumber, CustomerID, SiteID) {
        Table.Init('#tblDetailXLAdd');
        var params = { strBulkID: BulkID };
        var tblList = $("#tblDetailXLAdd").DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/StartBaps/getTrxXlAddList",
                "type": "POST",
                "datatype": "json",
                "data": params,
                "cache": false
            },
            buttons: [],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                 { data: "RowIndex" },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<i class='fa fa-pencil btn btn-xs green link-TrxEditListXLAdd' data-toggle='modal'></i> &nbsp;";
                        strReturn += "<i class='fa fa-trash btn btn-xs red link-TrxDeleteListXLAdd' data-toggle='modal'></i>";
                        return strReturn;
                    }
                },
                { data: "SoNumberAdd" },
                { data: "ProductType" },
                { data: "AntennaCount" },
                { data: "Height" },
                { data: "AntennaDiameter" },
                { data: "SurfaceArea" },
                { data: "SurfaceAreaModern" },
                { data: "Foundation" },
                { data: "FoundationByContract" },
                { data: "AvailableSpace" },
                { data: "Price" },
                { data: "AddPrice" },
                { data: "ID" },
                { data: "BulkyID" },
                
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [13], "visible": true }, { "targets": [14], "visible": false }, { "targets": [15], "visible": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [1, "asc"],
            "scrollY": 300,
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
        $("#tblDetailXLAdd tbody").unbind();
        $("#tblDetailXLAdd tbody").on("click", ".link-TrxEditListXLAdd", function (e) {
            var table = $("#tblDetailXLAdd").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $('#IDTrxDetailXLAdd').val(row.ID);
            Form.ClearFormXlAddDetail();
            if ($.trim(row.ProductType) == "ADDITIONAL LAND") {
                $('#FoundationXlAdd').val(row.Foundation);
                $('#FoundationByContractXLAdd').val(row.FoundationByContract);
                $('#AvailableSpaceXLAdd').val(row.AvailableSpace);
                $('#FoundationPrice').val(row.Price);                
                $("#XlAddLand").show();
            }

            else if ($.trim(row.ProductType) == "ADDITIONAL RRU") {
                $('#JenisAntennaRRUXLAdd').val(row.ProductType);
                $('#JumlahAntennaRRUXlAdd').val(row.AntennaCount);
                $('#LuasPermukaanRRUXlAdd').val(row.SurfaceArea);
                $('#LuasPermukaanModernRRUXLAdd').val(row.SurfaceAreaModern);
                $('#KetinggianRRUXLAdd').val(row.Height);
                $('#RRUPrice').val(row.Price);
                $('#AdditionalPrice').val(row.AddPrice);
                $("#XlAddAntennaRRU").show();
            }
                
            else if ($.trim(row.ProductType) == "ADDITIONAL ANTENA RF") {
                $('#JenisAntennaRFXLAdd').val(row.ProductType);
                $('#JumlahAntennaRFXlAdd').val(row.AntennaCount);                
                $('#KetinggianRFXLAdd').val(row.Height);
                $('#RFPrice').val(row.Price);
                $("#XlAddAntennaRF").show();
            }
                
            else if ($.trim(row.ProductType) == "ADDITIONAL ANTENA MW") {   
                $('#JenisAntennaMWXLAdd').val(row.ProductType);
                $('#JumlahAntennaMWXlAdd').val(row.AntennaCount);                
                $('#DiameterMWXLAdd').val(row.AntennaDiameter);
                $('#KetinggianMWXLAdd').val(row.Height);
                $('#MWPrice').val(row.Price);
                $("#XlAddAntennaMW").show();
            }
            $("#btnXLAddDetailSave").unbind().click(function () {
                Form.XlAddDetailSave(SoNumber, CustomerID, SiteID);
            });
            $("#btnXLAddDetailCancel").unbind().click(function () {

            });
            $("#mdlXLAddSave").modal('show');
                
        });

        $("#tblDetailXLAdd tbody").on("click", ".link-TrxDeleteListXLAdd", function (e) {
            var table = $("#tblDetailXLAdd").DataTable();
            var row = table.row($(this).parents('tr')).data();
            swal({
                title: "Warning",
                text: "Are You Sure Delete This Row ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, Continue!",
                closeOnConfirm: true
            }, function () {
                Form.XLAddDelete($.trim(row.ID), $.trim(row.BulkyID), SoNumber, CustomerID, SiteID);
            });
        });

    },

    //GridPica: function () {
    //    var l = Ladda.create(document.querySelector("#btSearch"));
    //    l.start();
    //    fsCustomerId = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
    //    fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
    //    fsProductType = $("#slProductType").val() == null ? "" : $("#slProductType").val();
    //    fsGroupBy = $("#slGroupBy").val() == null ? "" : $("#slGroupBy").val();
    //    var params = {
    //        strCustomerId: fsCustomerId,
    //        strCompanyId: fsCompanyId,
    //        strProductId: fsProductType,
    //        strGroupBy: fsGroupBy
    //    };
    //    var tblList = $("#tblPicaList").DataTable({
    //        "deferRender": true,
    //        "proccessing": true,
    //        "serverSide": true,
    //        "language": {
    //            "emptyTable": "No data available in table"
    //        },
    //        "ajax": {
    //            "url": "/api/StartBaps/gridPica",// + idTable + "",
    //            "type": "POST",
    //            "datatype": "json",
    //            "data": params,
    //        },
    //        buttons: [
    //            { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
    //            {
    //                text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
    //                    var l = Ladda.create(document.querySelector(".yellow"));
    //                    l.start();
    //                    Table.Export()
    //                    l.stop();
    //                }
    //            },
    //            { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
    //        ],
    //        "filter": false,
    //        "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
    //        "destroy": true,
    //        "columns": [
    //               { data: "RowIndex" },
    //            {
    //                orderable: false,
    //                mRender: function (data, type, full) {
    //                    var strReturn = "";
    //                    strReturn += "<i class='fa fa-edit btn btn-xs green link-TrxCheckDoc' data-toggle='modal' data-target='' class='link'></i>"
    //                    strReturn += "<i class='fa fa-mouse-pointer btn btn-xs blue'  data-toggle='modal' data-target=''></i>"
    //                    return strReturn;
    //                }
    //            },
    //            { data: "SoNumber" },
    //            { data: "CustomerID" },
    //            { data: "CustomerSiteID" },
    //            { data: "CustomerSiteName" },
    //            { data: "Company" },
    //            {
    //                data: "BaukDone", render: function (data) {
    //                    return Common.Format.ConvertJSONDateTime(data);
    //                }
    //            },
    //            {
    //                data: "RFITargetDate", render: function (data) {
    //                    return Common.Format.ConvertJSONDateTime(data);
    //                }
    //            },
    //        ],
    //        "columnDefs": [{ "targets": [0], "orderable": false }],
    //        "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
    //        "fnPreDrawCallback": function () {
    //            App.blockUI({ target: ".panelPicaInput", boxed: true });
    //        },
    //        "fnDrawCallback": function () {
    //            if (Common.CheckError.List(tblList.data())) {
    //                $(".panelPicaInput").fadeIn();
    //            }
    //            l.stop(); App.unblockUI('.panelPicaInput');
    //            if (Data.RowSelected.length > 0) {
    //                var item;
    //                for (var i = 0 ; i < Data.RowSelected.length; i++) {
    //                    item = Data.RowSelected[i];
    //                    if (!Helper.IsElementExistsInArray(parseInt(item), Data.RowSelectedSite))
    //                        $("#Row" + item).addClass("active");
    //                }
    //            }
    //        },
    //        "order": [1, "asc"],
    //        "scrollY": 300,
    //        "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
    //        "scrollCollapse": true,
    //        "fixedColumns": {
    //            leftColumns: 1 /* Set the 2 most left columns as fixed columns */
    //        },
    //        "createdRow": function (row, data, index) {
    //            /* Add Unique CSS Class to row as identifier in the cloned table */
    //            var id = $(row).attr("id");
    //            $(row).addClass(id);
    //        },
    //    });    
    //}
}
