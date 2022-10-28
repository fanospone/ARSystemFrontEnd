Data = {};
dataMaterial = {};

var fsCompanyId = "";
var fsSoNumb = "";
var fsCustomerID = "";
var fsIDTransaction = "";
var fsApprIDCompany = "";
var fsApprIDCustomer1 = "";
var fsApprIDCustomer2 = "";
var fsApprIDCustomer3 = "";
var fsActLabel = "";
var fsLandStatus = "";
var fsTowerType = "";

// == BAPS == //
var vIDBaps = "";
var vSoNumber = "";
var vStipSiro = "";
var vSiteID = "";
var vSpkNumber = "";
var vCustomerID = "";
var vStipCategory = "";
var vProductID = "";
var vSpkDate = null;
var vSdaDate = null;
var vAtpDate = null;
var vStartPeriodDate = null;
var vEndPeriodDate = null;
var vStartAddLeaseDate = null;
var vStartAddLeaseDate2 = null;
var vEndAddLeaseDate = null;
var vEndAddLeaseDate2 = null;
var vPriceLease = "";
var vAddPriceLease = "";
var vAddPriceLease2 = "";
var vProposalNumber = "";
var vProposalDate = null;
var vIssuingDate = null;
var vBapsNumber = "";
var vContractNumber = "";
var vPoNumber = "";
var vPoDate = null;
var vLandStatusID = "";
var vSurfaceArea1 = "";
var vSurfaceArea2 = "";
var vEffectiveDate = null;
var vPricePerMonth = "";
var vFrequency = "";
var vNumberOfAntenna = "";
var vProjectDef = "";
var vBuildingHeight = "";
var vLandSize = "";
var vPowerSize = "";
var vOtherPrice = "";
var vOpexPrice = "";
var vInstallPermitDate = null;
var vBaufDate = null;
var vBapsDate = null;
var vSiteType = "";
var vServiceType = "";
var vTowerType = "";
//== Height SPACE ==//
var vIDHeightSpace = "";
var vMaterialName = "";
var vMaterialType = "";
var vQuantity = "";
var vHeight = "";
var vDimension = "";
var vModel = "";
var vGroundSpace = "";
var vAzim = "";
var vValue = "";
var vCableSize = "";
var vCableQuantity = "";
var vManufacture = "";
// == BAPS Bulky == //
var vIDBapsBulky = "";
var vCompanyID = "";
var vJobDesc = "";
var vContractDate = null;
var vNoted = "";
var vDescription = "";
var vGrRecievedDate = null;
var vPerformGR = "";
var vTicketNumber = "";
var vCategory = "";
var vCompanyInfo = "";
var vHeightSpace = "";
var vActivityID = "";
var vMlaDate = null;
var vBulkyNumber = null;
jQuery(document).ready(function () {
    Form.Init();
    Table.GridValidationPrint();
    // ==================== Buttton Seacrh =============================//
    $("#btSearch").unbind().click(function () {
        if ($("#tabStartBaps").tabs('option', 'active') == 0) {
            Table.GridValidationPrint();
        }
        else if ($("#tabStartBaps").tabs('option', 'active') == 1) {
            Table.GriValidationBulkyPrint();
        } else if ($("#tabStartBaps").tabs('option', 'active') == 3) {
            Table.GridViewBaukDocument();
        }
    });
    // ==================== End Buttton Seacrh =============================//
    $("#btReset").unbind().click(function () {
        $("#slSearchCustomerID").val("").trigger('change');
        $("#slSearchSiteID").val("").trigger('change');
        $("#slSearchSoNumber").val("").trigger('change');
        $("#slSearchCompanyID").val("").trigger('change');
        $("#slSearchProductID").val("").trigger('change');
        $("#slBulkyNumber").val("").trigger('change');
        $("#slStipCategory").val("").trigger('change');
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
            Table.GridValidationPrint();
            Control.TabActive(newIndex)
            $(".panelPicaInput").fadeOut(100);
            $(".panelDataValidationBulkyPrint").fadeOut(100);
            $(".panelBapsValidation").fadeOut(100);
            $(".panelBaukDocument").fadeOut(100);
            $(".panelDataValidationPrint").fadeIn(1500);
        } else if (newIndex == 1) {
            Table.GriValidationBulkyPrint();
            Control.TabActive(newIndex)
            $(".panelPicaInput").fadeOut(100);
            $(".panelDataValidationPrint").fadeOut(100);
            $(".panelBapsValidation").fadeOut(100);
            $(".panelBaukDocument").fadeOut(100);
            $(".panelDataValidationBulkyPrint").fadeIn(1500);
        }
        else if (newIndex == 2) {
            Table.Init("#tblPicaList");
            Control.TabActive(newIndex)
            $(".panelDataValidationPrint").fadeOut(100);
            $(".panelDataValidationBulkyPrint").fadeOut(100);
            $(".panelBapsValidation").fadeOut(100);
            $(".panelBaukDocument").fadeOut(100);
            $(".panelPicaInput").fadeIn(1500);
        }
        else if (newIndex == 3) {

            Control.TabActive(newIndex)
            $(".panelDataValidationPrint").fadeOut(100);
            $(".panelDataValidationBulkyPrint").fadeOut(100);
            $(".panelBapsValidation").fadeOut(100);
            $(".panelPicaInput").fadeOut(1500);
            $(".panelBaukDocument").fadeIn(1500);
            Table.GridViewBaukDocument();
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
        $("#BulkyIDIsatBulky").val('');
        $('#BulkNumberIsatBulky').val('');

        Control.ButtonResetBulky("1");
        $(".panelValidationForm").fadeIn(1800);
        $("#partialBulkyForm").fadeIn(2000);

        $('#SearchSiteIDBulky').empty();
        $('#SearchSoNumberBulky').empty();
        Control.BindingSelectProductTypeBulky();
        $("#slSearchStipCategoryBulky").change(function () {
            Control.BindingSelectProductTypeBulky();
        });
        $('.btnSubmitTrx').prop('disabled', false);
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
            if (CustomerID == "XL" && StipCategory == "NON-ADDITIONAL") {
                Form.XlBulkyForm(SoNumber, CustomerID, CompanyID, SiteID, $('#BulkID').val());
            }
            else if (CustomerID == "XL" && StipCategory == "ADDITIONAL") {
                Form.XLAddForm(SoNumber, CustomerID, CompanyID, StipCategory, SiteID, $('#IDTrxXLAdd').val(), $("#slSearchProductIDBulky").val());
            } else if (CustomerID == "ISAT") {
                Form.ISATBulkyForm(SoNumber, CustomerID, CompanyID, StipCategory, SiteID, $('#BulkyIDIsatBulky').val(), $("#slSearchProductIDBulky").val());
            }
        }
    });

    $('#btnCloseDocBauk').unbind().click(function () {
        $(".panelTab").fadeIn(1800);
        $(".filter").fadeIn(1800);
        $("#panelHeaderInfo").hide();
        $(".panelValidationForm").hide();
        $("#partialBAUKDocument").hide();
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
                $(id).html("<option></option>");
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

    BindingSelectProductTypeBulky: function () {
        var id2 = "#slSearchProductIDBulky";
        var category = $("#slSearchStipCategoryBulky").val() == null ? "" : $("#slSearchStipCategoryBulky").val();

        var params = { strCategory: category };
        $.ajax({
            url: "/api/StartBaps/getProductType",
            type: "POST",
            data: params
        })
            .done(function (data, textStatus, jqXHR) {
                $(id2).html("<option></option>");
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id2).append("<option value='" + $.trim(item.Value) + "'>" + item.Text + "</option>");
                    })
                }
                $(id2).select2({ placeholder: "Select Product", width: null });
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

    BindingSelectApprovalCompany: function (selectID) {
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
        var params = {
            strCustomerIdId: operatorId, strBapsType: bapsType, strSoNumber: soNumber, strProductType: productType
        };
        $.ajax({
            url: "/api/StartBaps/fillForm",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        })
            .done(function (data, textStatus, jqXHR) {
                var htmlString = Data.HtmlString;
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
        //Control.BindingSelectBulkNumber();
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

    PrintPDF: function (SoNumber, CustomerID, BulkID, PrintType, StipCategory, ProductType) {
        window.location.href = "/RANewBaps/printPDF?SoNumber=" + SoNumber + "&BulkID=" + BulkID + "&CustomerID=" + CustomerID + "&PrintType=" + PrintType + "&StipCategory=" + StipCategory + "&ProductType=" + ProductType;
    },

    ButtonResetBulky: function (value) {
        $("#btResetBulky").unbind().click(function () {
            if (value == "1") {
                $("#slSearchCompanyNameBulky").val("").trigger('change');
                $("#slSearchCustomerNameBulky").val("").trigger('change');
                $("#slStipCategoryBulky").val("").trigger('change');


            }
            $("#SearchSoNumberBulky").val("");//.trigger('change');
            $("#SearchSiteIDBulky").val("");//.trigger('change');
            $("#slStipCategoryBulky").val("").trigger('change');
            $("#slSearchProductIDBulky").val("").trigger('change');
        });
    },

    BindingStipCategory: function () {
        $("#slSearchStipCategoryBulky").html("<option></option>")
        $("#slSearchStipCategoryBulky").append("<option value='ADDITIONAL' selected>Additional</option>");
        $("#slSearchStipCategoryBulky").append("<option value='NON-ADDITIONAL'>Non Additional</option>");
        $("#slSearchStipCategoryBulky").select2({ placeholder: "Select Group", width: null });
    },

    BindingSelectSiteIDCust: function () {
        var selectId = "#SearchSiteIDBulky";
        var params = { strCustomerId: $("#slSearchCustomerNameBulky").val() }
        $.ajax({
            url: "/api/StartBaps/getSiteIDCust",
            type: "POST",
            async: false,
            data: params
        })
            .done(function (data, textStatus, jqXHR) {
                $(selectId).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(selectId).append("<option value='" + $.trim(item.CustomerSiteID) + "'>" + item.SiteName + "</option>");
                    })
                }
                $(selectId).select2({ placeholder: "Select Site ID", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingSelectSoNumbAdd: function () {
        var selectId = "#SearchSoNumberBulky";
        var params = { strCustomerId: $("#slSearchCustomerNameBulky").val() }
        $.ajax({
            url: "/api/StartBaps/getSoNumbAddlist",
            type: "POST",
            async: false,
            data: params
        })
            .done(function (data, textStatus, jqXHR) {
                $(selectId).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(selectId).append("<option value='" + $.trim(item.Sonumber) + "'>" + item.SiteName + "</option>");
                    })
                }
                $(selectId).select2({ placeholder: "Select So Number", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingSelectSoNumbXLStip3: function () {
        var selectId = "#SearchSoNumberBulky";
        var selectId2 = "#SearchSiteIDBulky";
        var params = { strCustomerId: $("#slSearchCustomerNameBulky").val() }
        $.ajax({
            url: "/api/StartBaps/getSoNumbXLStip3List",
            type: "POST",
            async: false,
            data: params
        })
            .done(function (data, textStatus, jqXHR) {
                $(selectId).html("<option></option>")
                $(selectId2).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(selectId).append("<option value='" + $.trim(item.SoNumber) + "'>" + item.SoNumber + ' - ' + item.SiteNameCustomer + "</option>");
                        $(selectId2).append("<option value='" + $.trim(item.SiteID) + "'>" + item.SiteID + ' - ' + item.SiteNameCustomer + "</option>");
                    })
                }
                $(selectId).select2({ placeholder: "Select Sonumb", width: null });
                $(selectId2).select2({ placeholder: "Select Site ID", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });

    },

    BindingSelectBulkNumber: function () {
        var selectId = "#slBulkyNumber";
        var params = { strBulkNumber: "", strCompanyId: "" };
        $.ajax({
            url: "/api/StartBaps/gridValidationBulkyPrintList",
            type: "POST",
            async: false,
            data: params
        })
            .done(function (data, textStatus, jqXHR) {
                $(selectId).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(selectId).append("<option value='" + $.trim(item.BulkNumber) + "'>" + item.BulkNumber + ' - ' + item.CustomerID + "</option>");
                    })
                }
                $(selectId).select2({ placeholder: "Select So Number", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingSelectSoNumberPint: function (action) {
        var selectId = "#slSearchSoNumber";
        var selectId2 = "#slSearchSiteID";
        var params = { strCompanyId: "", strCustomerId: "", strCustomerId: "", strProductId: "", strSoNumber: "", strSiteID: "", strTenantTypeID: "", strAction: action, strDataType: "nonBulky", strBapsType: "" };
        $.ajax({
            url: "/api/StartBaps/gridSonumbListDDL",
            type: "POST",
            async: false,
            data: params
        })
            .done(function (data, textStatus, jqXHR) {
                $(selectId).html("<option></option>");
                $(selectId2).html("<option></option>");
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(selectId).append("<option value='" + $.trim(item.SoNumber) + "'>" + item.SoNumber + ' - ' + item.SiteName + "</option>");
                        $(selectId2).append("<option value='" + $.trim(item.SiteID) + "'>" + item.SiteID + ' - ' + item.SiteName + "</option>");
                    })
                }
                $(selectId).select2({ placeholder: "Select So Number", width: null });
                $(selectId2).select2({ placeholder: "Select Site ID", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BingSelectStipCategory: function () {
        var id = "#slStipCategory";
        var id2 = "#slStipCategoryBulky";
        $.ajax({
            url: "/api/StartBaps/getListStipCategory",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>");
                $(id2).html("<option></option>");
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + $.trim(item.STIPID) + "'>" + item.STIPCode + "</option>");
                        $(id2).append("<option value='" + $.trim(item.STIPID) + "'>" + item.STIPCode + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select STIP", width: null });
                $(id2).select2({ placeholder: "Select STIP", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BingSelectNextActivity: function (currActID) {
        //var params = { CurrentActivity : currActID};
        //var id = "#slStatusAppr";
        //$.ajax({
        //    url: "/api/MstDataSource/NextActivity",
        //    type: "GET",
        //    async: false,
        //    data : params
        //})
        //.done(function (data, textStatus, jqXHR) {
        //    $(id).html("<option></option>");
        //    if (Common.CheckError.List(data)) {
        //        $.each(data, function (i, item) {
        //            $(id).append("<option value='" + $.trim(item.Value) + "'>" + item.Text + "</option>");
        //        })
        //    }
        //    $(id).select2({ placeholder: "Select Status", width: null });
        //})
        //.fail(function (jqXHR, textStatus, errorThrown) {
        //    Common.Alert.Error(errorThrown);
        //});
        $("#slStatusAppr").html("<option></option>")
        $("#slStatusAppr").append("<option value='Approve' selected>Approve</option>");
        $("#slStatusAppr").append("<option value='Reject'>Reject</option>");
        $("#slStatusAppr").select2({ placeholder: "Select Group", width: null });

        $("#slStatusApprBulky").html("<option></option>")
        $("#slStatusApprBulky").append("<option value='Approve' selected>Approve</option>");
        $("#slStatusApprBulky").append("<option value='Reject'>Reject</option>");
        $("#slStatusApprBulky").select2({ placeholder: "Select Group", width: null });
    },

    TabActive: function (index) {
        if (index == 1) {
            $(".fGroupBy").hide();
            $(".fProductType").hide();
            $(".fBapsType").hide();
            $(".fSoNumber").hide();
            $(".fsSiteID").hide();
            $(".fStip").hide();
            $(".fBuklyNumber").show();
        } else {
            $(".fBuklyNumber").hide();
            $(".fGroupBy").show();
            $(".fProductType").show();
            $(".fBapsType").show();
            $(".fSoNumber").show();
            $(".fsSiteID").show();
            $(".fCustomer").show();
            $(".fStip").show();
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

    FormatCurrency: function (selectedID) {
        $(selectedID).unbind().on('blur', function () {
            var value = $(selectedID).val();
            if (value != "") {
                $(selectedID).val(Common.Format.CommaSeparation(value));
            } else {
                $(selectedID).val("0.00");
            }
        });
    },

    CheckCountRowTb: function (idTb) {
        var result = false
        var countTblXL = $(idTb).DataTable();
        var i = countTblXL.data().count();
        if (i > 0) {
            result = true;
        }

        return result;
    },

    ClearValues: function () {
        // == BAPS == //
        vIDBaps = "";
        vSoNumber = "";
        vStipSiro = "";
        vSiteID = "";
        vSpkNumber = "";
        vCustomerID = "";
        vStipCategory = "";
        vProductID = "";
        vSpkDate = null;
        vSdaDate = null;
        vAtpDate = null;
        vStartPeriodDate = null;
        vEndPeriodDate = null;
        vStartAddLeaseDate = null;
        vStartAddLeaseDate2 = null;
        vEndAddLeaseDate = null;
        vEndAddLeaseDate2 = null;
        vPriceLease = "";
        vAddPriceLease = "";
        vAddPriceLease2 = "";
        vProposalNumber = "";
        vProposalDate = null;
        vIssuingDate = null;
        vBapsNumber = "";
        vContractNumber = "";
        vPoNumber = "";
        vPoDate = null;
        vLandStatusID = "";
        vSurfaceArea1 = "";
        vSurfaceArea2 = "";
        vEffectiveDate = null;
        vPricePerMonth = "";
        vFrequency = "";
        vNumberOfAntenna = "";
        vProjectDef = "";
        vBuildingHeight = "";
        vLandSize = "";
        vPowerSize = "";
        vOtherPrice = "";
        vOpexPrice = "";
        vInstallPermitDate = null;
        vBaufDate = null;
        vBapsDate = null;
        //== Height SPACE ==//
        vIDHeightSpace = "";
        vMaterialName = "";
        vMaterialType = "";
        vQuantity = "";
        vHeight = "";
        vDimension = "";
        vModel = "";
        vGroundSpace = "";
        vAzim = "";
        vValue = "";
        vCableSize = "";
        vCableQuantity = "";
        vManufacture = "";

        // == BAPS Bulky == //
        vIDBapsBulky = "";
        vCompanyID = "";
        vJobDesc = "";
        vContractDate = null;
        vNoted = "";
        vDescription = "";
        vGrRecievedDate = null;
        vPerformGR = "";
        vTicketNumber = "";
        vCategory = "";
        vCompanyInfo = "";
        vHeightSpace = "";
        vActivityID = "";

    },
    BindBAUKDocument: function (SoNumber, SiteID, CustomerID) {
        var params = {
            strSoNumber: SoNumber,
            strSiteId: SiteID,
            strCustomerId: CustomerID
        };

        $.ajax({
            url: "/api/CheckingDoc/getCheckDocList",
            type: "POST",
            datatype: "JSON",
            data: params
        })
            .done(function (data, textStatus, jqXHR) {
                $('#DocumentList').html('');
                if (data.data.length > 0) {
                    var htmlElemetns = "<ul>";
                    var no = 0;
                    $.each(data.data, function (i, item) {
                        htmlElemetns += '<li class="mt-list-item">' +
                            ' <div class="list-icon-container">' +
                            '<i class="icon-check"></i> &nbsp;' + (no += 1) +
                            '</div>' +
                            '<div class="list-datetime"> </div>' +
                            '<div class="list-item-content">' +
                            '<h3 class="uppercase">' +
                            '<a href="javascript:Helper.DownloadBAUKDocument(\'' + item.FileName + '\',\'' + item.LinkFile + '\',' + item.IsLegacy + ');">' + item.AliasName + '</a>' +
                            '</h3>' +
                            '</div>' +
                            '</li >';

                    });
                    htmlElemetns += "</ul>";
                    $('#DocumentList').append(htmlElemetns);
                }

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingDocumentSupport: function (CompanyId, SiteId) {

        $.ajax({
            url: "/api/CheckingDoc/getDocumentSupport",
            type: "GET",
            async: false,
            data: { companyId: CompanyId, siteId: SiteId },
        })
            .done(function (data, textStatus, jqXHR) {
                console.log(data);
                $('#DocumenSupporttList').html('');
                if (data != null && data.length > 0) {
                    var htmlElemetns = "<ul>";
                    $.each(data, function (i, item) {
                        htmlElemetns += '<li class="mt-list-item">' +
                            ' <div class="list-icon-container">' +
                            '<i class="icon-check"></i> &nbsp;' + item.No +
                            '</div>' +
                            '<div class="list-datetime"> </div>' +
                            '<div class="list-item-content">' +
                            '<h3 class="uppercase">' +
                            '<a href="javascript:Helper.DownloadDocSupport(\'' + item.DocumentName + '\',\'' + CompanyId + '\',\'' + SiteId + '\');">' + item.DocumentName + '</a>' +
                            '</h3>' +
                            '</div>' +
                            '</li >';

                    });
                    htmlElemetns += "</ul>";

                    $('#DocumenSupporttList').append(htmlElemetns);
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
}

var Form = {

    Init: function () {
        $("#panelHeaderInfo").hide();
        $(".panelDataValidationBulkyPrint").hide();
        $(".panelValidationForm").hide();
        $(".panelPicaInput").hide();

        $('#tabStartBaps').tabs();
        $("#partialBapsValidationForm").hide();
        $("#partialHcptForm").hide();
        $("#partialSfIBSForm").hide();
        $("#partialBulkyForm").hide();
        $("#panelPrintViewPDF").hide();
        $("#partialSfAddForm").hide();
        $("#partialXLAddForm").hide();
        $("#partialXlSiteAccessForm").hide();
        $("#partialSmartSiroForm").hide();
        $("#partialiSatForm").hide();
        $("#partialXLSiroForm").hide();
        $("#partialXLRelocForm").hide();
        $("#partialXLNewForm").hide();
        $("#partialSTIForm").hide();
        $("#partialINUXForm").hide();
        $("#partialSFNewForm").hide();
        $("#partialSFSIROForm").hide();
        $("#partialBAUKDocument").hide();
        $(".fBuklyNumber").hide();
        Control.BindingSelectCompany();
        Control.BindingSelectOperator();
        Control.BindingStipCategory();
        Control.BindingSelectProductType();
        Control.BindingSelectGroupBy();
        Control.BindingDatePicker();
        Control.BingSelectStipCategory();
        //untuk validasi checkbox
        Data.RowSelected = [];
        Data.RowSelectedSite = [];
    },

    GetForm: function (SoNumber, CustomerID, SiteID, CompanyID, CompanyName, CustomerSiteID, CustomerSiteName, ProductType, StipCategory, StipSiro, RegionID) {
        var params = { strSoNumber: SoNumber, strSiteID: SiteID, strCustomerId: CustomerID, strProductId: ProductType, strStipCategory: StipCategory, strStipSiro: StipSiro, strRegionId: RegionID };
        $.ajax({
            url: "/api/StartBaps/getFormValidationPrint",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (data != '') {
                Form.SetHeaderInfo(SiteID, SoNumber, CustomerID, CompanyID, CompanyName, CustomerSiteID, CustomerSiteName)
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
    },

    SetHeaderInfo: function (SiteID, SoNumber, CustomerID, CompanyID, CompanyName, CustomerSiteID, CustomerSiteName, BapsType) {
        $("#SiteIdTbg").text($.trim(SiteID));
        $("#SoNumber").text($.trim(SoNumber));
        $("#CustomerID").text($.trim(CustomerID));
        $("#CompanyID").text($.trim(CompanyID));
        $("#CompanyName").text($.trim(CompanyName));
        $("#CustomerSiteID").text($.trim(CustomerSiteID));
        $("#CustomerSiteName").text($.trim(CustomerSiteName));
    },

    // Xl Bulky / non additional //
    XlBulkyForm: function (SoNumber, CustomerID, CompanyID, SiteID, BulkID) {
        Control.BindingSelectApprovalCustomer('#slApprCustomerXLBulky', CustomerID, '0');
        Control.BindingSelectApprovalCompany('#slApprCompanyXLBulky');
        $("#PartialXLAddBulky").hide();
        $("#PartialiSatBulky").hide();
        Table.GridSonumbXLBulky(CustomerID, CompanyID, SoNumber, SiteID);
        Table.GridDetailsXLBulky(BulkID);
        $("#PartialXLBulky").show();
        $("#btnXLBuklySave").unbind().click(function () {

            if (Control.CheckCountRowTb("#tblXLBulky") == false)
                Common.Alert.Warning("Details is not null");
            else
                Form.XLBulkySubmit(CustomerID, CompanyID, SoNumber, SiteID);

        });
        $("#btnXLBuklyCancel").unbind().click(function () {
            $(".panelValidationForm").fadeOut();
            $("#partialXLBulkyForm").fadeOut();
            $(".panelTab").fadeIn(2000);
            $(".filter").fadeIn(2000);
            Table.GriValidationBulkyPrint();
        });
        $("#btnXLBulkyListSave").unbind().click(function () {
            Form.XLBulkySaveDetail(CustomerID, CompanyID, SoNumber, SiteID);
        });
        $("#partialBulkyForm").fadeIn(1500);
        $("#PartialXLBulky").fadeIn(1000);
    },

    XLBulkySaveDetail: function (CustomerID, CompanyID, SoNumber, SiteID) {
        var bulkID;
        var l = Ladda.create(document.querySelector("#btnXLBulkyListSave"));
        Data.SoNumber = $("#SoNumberXL").val();
        Data.SiteID = $("#SiteIDXL").val();
        Data.SiteIDCustomer = $("#SiteIDCustomerXL").val();
        Data.SiteNameCustomer = $("#SiteNameCustomnerXL").val();
        Data.ID = $("#IDTrxXL").val();
        Data.BulkyID = $("#BulkID").val();
        Data.ProjectType = $("#ProjectType").val();
        Data.EWO = $("#EWO").val();
        Data.PO = $("#PO").val();
        Data.RemarkFOP = $("#RemarkFOP").val();
        Data.Material = $("#Material").val();
        Data.MaterialDescription = $("#MaterialDesc").val();
        Data.Qty = $("#Qty").val();
        Data.Price = $("#Price").val();
        Data.PoValue = $("#PoValue").val();
        Data.Currency = $("#Currency").val();
        Data.RemarkMML = $("#RemarkMML").val();
        Data.Item = $("#Item").val();
        Data.PoItem = $("#PoItem").val();
        Data.Ess = $("#Ess").val();
        Data.PoDate = $("#PoDate").val();
        Data.AtpDate = $("#AtpDate").val();
        Data.PicFop = $("#PicFop").val();
        Data.SiteStatus = $("#SiteStatus").val();
        Data.CustomerID = $("#slSearchCustomerNameBulky").val();
        Data.CompanyID = $("#slSearchCompanyNameBulky").val();
        Data.StipSiro = $("#StipSiroXL").val();
        var params = {
            trxRABapsPrintXLBulky: Data
        };
        $.ajax({
            url: "/api/StartBaps/saveDetailXLBulky",
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
                    Table.GridDetailsXLBulky(data.BulkyID);
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

    XlBulkyDetailDelete: function (ID, bulkID) {
        var params = { strIDTrx: ID };
        $.ajax({
            url: "/api/StartBaps/deleteDetailXLBulky",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            Table.GridSonumbXLBulky();
            Table.GridDetailsXLBulky(bulkID);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
    },

    XLBulkyGetDetail: function (BulkID) {
        var params = { strBulkID: BulkID };
        $.ajax({
            url: "/api/StartBaps/getDetailXLBulky",
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

    XLBulkySubmit: function (CustomerID, CompanyID, SoNumber, SiteID) {
        vIDBapsBulky = $("#BulkID").val();
        vContractNumber = $("#BapsNumber").val();
        vDescription = $("#Description").val();
        vGrRecievedDate = $("#GrRecievedDate").val();
        vPerformGR = $("#PerformGR").val();
        vTicketNumber = $("#TicketNumber").val();
        vCustomerID = $("#slSearchCustomerNameBulky").val();
        vCompanyID = $("#slSearchCompanyNameBulky").val();
        vCategory = $("#slSearchStipCategoryBulky").val();
        fsApprIDCompany = $("#slApprCompanyXLBulky").val();
        fsApprIDCustomer1 = $("#slApprCustomerXLBulky").val();
        DataAccess.SubmitBAPSBulky();
    },

    // Smartfren additional //
    SfAddForm: function (SoNumber, SiteIDTBG, CustomerID, ProductID, StipCategory, StipSiro, RegionID) {
        Control.BindingSelectApprovalCustomer('#slApprCustomerSfAdd', CustomerID, RegionID);
        Control.BindingSelectApprovalCompany('#slApprCompanySfAdd');
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
                            $("#slApprCompanySfAdd").val(data.ApprIDCompany).trigger('change');
                            $("#slApprCustomerFsAdd").val(data.ApprIDCustomer1).trigger('change');
                            $("#SiteIDSfAdd").val(data.SiteID);
                        } else {
                            Common.Alert.Warning(data.ErrorMessage);
                        }
                    }
                }
            }
            else {
                $("#IDTrxSfAdd").val("");
                $("#SiteIDSfAdd").val("");
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
        fsApprIDCompany = $("#slApprCompanySfAdd").val();
        fsApprIDCustomer1 = $("#slApprCustomerSfAdd").val();

        Data.ID = fsIDTransaction;
        Data.StartPeriodDate = fsStartDateLease;
        Data.EndPeriodDate = fsEndDateLease;
        Data.SoNumber = SoNumber;
        Data.SiteID = SiteIDTBG;
        Data.CustomerID = CustomerID;
        Data.StipCategory = StipCategory;
        Data.ApprIDCompany = fsApprIDCompany;
        Data.ApprIDCustomer1 = fsApprIDCustomer1;
        Data.BapsNumber = fsBapsNumber;
        Data.ProposalNumber = fsProposalNumber;
        Data.StartAddLeaseDate = fsStartAddLeaseDate;
        Data.StartAddLeaseDate2 = fsStartAddLeaseDate2;
        Data.EndAddLeaseDate = fsEndAddLeaseDate;
        Data.EndAddLeaseDate2 = fsEndAddLeaseDate2;
        Data.AddPriceLease = fsAddLeasePrice;
        Data.AddPriceLease2 = fsAddLeasePrice2;
        Data.StipSiro = StipSiro;


        var params = {
            validation: Data
        };
        var l = Ladda.create(document.querySelector("#btnSfIBSSave"));
        $.ajax({
            url: "/api/StartBaps/submitBapsPrint",
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

        Data.ID = fsIDTrx;
        Data.SoNumber = fsSoNumb;
        Data.MaterialType = fsAntennaType;
        Data.Quantity = fsQty;
        Data.Dimension = fsDimension;
        Data.Model = fsModel;
        Data.Height = fsHeight;
        Data.StipSiro = fsStipSiro;

        var params = { equipment: Data }
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

    // Xl additional //
    XLAddForm: function (SoNumber, CustomerID, CompanyID, StipCategory, SiteID, BulkID, ProducID) {
        Control.BindingSelectApprovalCustomer('#slApprCustomerXLAdd1', CustomerID, '0');
        Control.BindingSelectApprovalCustomer('#slApprCustomerXLAdd2', CustomerID, '0');
        Control.BindingSelectApprovalCompany('#slApprCompanyXLAdd');
        $("#PartialiSatBulky").hide();
        $("#PartialXLBulky").hide();
        Table.GridSonumbXLAdd(SoNumber, CustomerID, SiteID, ProducID);
        Table.GridDetailsXLAdd(BulkID, SoNumber, CustomerID, SiteID);
        $('#SearchSoNumberBulky').val(SoNumber);
        $("#btnXLAddSave").unbind().click(function () {

            if (Control.CheckCountRowTb("#tblDetailXLAdd") == false)
                Common.Alert.Warning("Additional detail is not null");
            else
                Form.XLAddSubmit(SoNumber, CustomerID, CompanyID, StipCategory);
        });
        $("#btnXLAddCancel").unbind().click(function () {
            Control.ButtonCancelForm();
            $("#partialXLAddForm").fadeOut();
        });
        $("#PartialXLAddBulky").fadeIn(1500);
        Control.FormatCurrency('#PriceLeaseXLAdd');
    },

    XlAddDetailSave: function (SoNumber, CustomerID, SiteID, ProductID) {
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

        Data.ID = $('#IDTrxDetailXLAdd').val();
        Data.BulkyID = $('#IDTrxXLAdd').val();
        Data.SoNumberAdd = $('#SoNumberXLAdd').val();
        Data.StipSiro = $('#StipSiroXLAdd').val();
        Data.AntennaCount = jumAntenna;
        Data.Height = height;
        Data.AntennaDiameter = diameterMW;
        Data.SurfaceArea = luasPermukaan;
        Data.SurfaceAreaModern = luasPermukaanModern;
        Data.Foundation = foundation;
        Data.FoundationByContract = foundationContract;
        Data.AvailableSpace = availableSpace;
        Data.Price = FoundationPrice;
        Data.AddPrice = addPrice;
        Data.SiteID = $("#SiteIDSfAdd").val();

        var params = { trxRABapsPrintXLAdd: Data, strCustomerId: $('#slSearchCustomerNameBulky').val(), strCompanyId: $('#slSearchCompanyNameBulky').val() }
        var l = Ladda.create(document.querySelector("#btnXLAddDetailSave"));
        $.ajax({
            url: "/api/StartBaps/saveDetailXlAdditional",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    $('#IDTrxXLAdd').val(data.BulkyID);
                    Table.GridSonumbXLAdd(null, CustomerID, SiteID, ProductID);
                    Table.GridDetailsXLAdd(data.BulkyID, SoNumber, CustomerID, SiteID)
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

    XLAddDelete: function (ID, BulkID, SoNumber, CustomerID, SiteID) {
        var params = { strIDTrx: ID };
        $.ajax({
            url: "/api/StartBaps/deleteDetailXlAdditional",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (data == true) {
                Common.Alert.Success("Data Success Deleted!");
                Table.GridSonumbXLAdd(null, CustomerID, SiteID)
                Table.GridDetailsXLAdd(BulkID, SoNumber, CustomerID, SiteID);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
    },

    XLAddSubmit: function (SoNumber, CustomerID, CompanyID, StipCategory) {
        vIDBapsBulky = $("#IDTrxXLAdd").val();
        vStartPeriodDate = $("#StartLeaseDateXLAdd").val();
        vEndPeriodDate = $("#EndLeaseDateXLAdd").val();
        vPriceLease = $("#PriceLeaseXLAdd").val();
        vCompanyID = $("#slSearchCompanyNameBulky").val(); // CompanyID;
        vCustomerID = CustomerID;
        vCategory = StipCategory;
        vCompanyInfo = $("#CompanyInfoXLAdd").val();
        vHeightSpace = $("#HeightSpaceXLAdd").val();
        vContractDate = $('#BakDateXLAdd').val();
        vPoDate = $("#StartLeaseDateXLAdd2").val();
        fsApprIDCompany = $("#slApprCompanyXLAdd").val();
        fsApprIDCustomer1 = $("#slApprCustomerXLAdd1").val();
        fsApprIDCustomer2 = $("#slApprCustomerXLAdd2").val();
        DataAccess.SubmitBAPSBulky();
    },

    // Xl Site Access Non Bulky //
    XLSiteAccessForm: function (SoNumber, SiteID, CustomerID, ProductType, StipCategory, StipSiro, RegionID) {
        Control.BindingSelectApprovalCompany("#slApprCompanyXLSA");
        Control.BindingSelectApprovalCustomer("#slApprCustomerXLSA", CustomerID, RegionID);
        Control.BindingSelectApprovalCustomer("#slApprCustomerXLSA2", CustomerID, RegionID);
        DataAccess.GetBAPSData(SoNumber, SiteID, StipSiro);
        $("#IDTrxXLSA").val(vIDBaps);
        $("#BAKdateXLSA").val(Common.Format.ConvertJSONDateTime(vContractDate));
        $("#LeasePriceXLSA").val(Common.Format.CommaSeparation(vPriceLease));
        $("#StartLeaseDate1").val(Common.Format.ConvertJSONDateTime(vStartPeriodDate));
        $("#StartLeaseDate2").val(Common.Format.ConvertJSONDateTime(vStartAddLeaseDate));
        $("#EndLeaseDate").val(Common.Format.ConvertJSONDateTime(vEndPeriodDate));
        $("#AddPriceXLSA").val(Common.Format.CommaSeparation(vAddPriceLease));
        $('#ProjectDefXLSA').val(vProjectDef);
        $("#slApprCompanyXLSA").val(fsApprIDCompany).trigger("change");
        $("#slApprCustomerXLSA").val(fsApprIDCustomer1).trigger("change");
        $("#slApprCustomerXLSA2").val(fsApprIDCustomer2).trigger("change");
        $("#btnXLSASubmit").unbind().click(function () {
            Form.XLSiteAccessSubmit(SoNumber, CustomerID, StipCategory, SiteID, StipSiro);
        });
        $("#btnXLSACancel").unbind().click(function () {
            Control.ButtonCancelForm();
            $("#partialXlSiteAccessForm").fadeOut();
        });
        Control.FormatCurrency('#LeasePriceXLSA');
        Control.FormatCurrency('#AddPriceXLSA');
        $("#partialXlSiteAccessForm").fadeIn(1500);
    },

    XLSiteAccessSubmit: function (SoNumber, CustomerID, StipCategory, SiteID, StipSiro, RegionID) {
        vCustomerID = CustomerID;
        vStipCategory = StipCategory;
        vSoNumber = SoNumber;
        vSiteID = SiteID;
        vStipSiro = StipSiro;
        vIDBaps = $("#IDTrxXLSA").val();
        vStartPeriodDate = $("#StartLeaseDate1").val();
        vStartAddLeaseDate = $("#StartLeaseDate2").val();
        vEndPeriodDate = $("#EndLeaseDate").val();
        vPriceLease = $("#LeasePriceXLSA").val();
        vAddPriceLease = $('#AddPriceXLSA').val();
        vContractDate = $('#BAKdateXLSA').val();
        vProjectDef = $('#ProjectDefXLSA').val();
        fsApprIDCompany = $("#slApprCompanyXLSA").val();
        fsApprIDCustomer1 = $("#slApprCustomerXLSA").val();
        fsApprIDCustomer2 = $("#slApprCustomerXLSA2").val();
        var result = "";
        result = DataAccess.SubmitBAPS();
        if (result == "") {
            $("#partialXlSiteAccessForm").fadeOut();
        }
    },

    // Smart Siro Non Bulky //
    SmartSiroForm: function (SoNumber, SiteID, CustomerID, ProductType, StipCategory, StipSiro, RegionID) {
        Control.BindingSelectApprovalCompany("#slApprCompanySmartSiro");
        Control.BindingSelectApprovalCustomer("#slApprCustomerSmartSiro", CustomerID, RegionID);
        DataAccess.GetBAPSData(SoNumber, SiteID, StipSiro);
        $("#IDTrxSmartSiro").val(vIDBaps);
        $("#MLANumberSmartSiro").val(vContractNumber);
        $("#MLAdateSmartSiro").val(Common.Format.ConvertJSONDateTime(vContractDate));
        $("#BapsNumberSmartSiro").val(vBapsNumber);
        $("#BapsDateSmartSiro").val(Common.Format.ConvertJSONDateTime(vBapsDate));
        $("#StartLeaseDateSmartSiro").val(Common.Format.ConvertJSONDateTime(vStartPeriodDate));
        $("#EndLeaseDateSmartSiro").val(Common.Format.ConvertJSONDateTime(vEndPeriodDate));
        $("#LeasePriceSmartSiro").val(Common.Format.CommaSeparation(vPriceLease));
        $("#AddPriceSmartSiro").val(Common.Format.CommaSeparation(vAddPriceLease));
        $('#SiteNameSmartSiro').val(vProjectDef);
        $("#slApprCompanySmartSiro").val(fsApprIDCompany).trigger("change");
        $("#slApprCustomerSmartSiro").val(fsApprIDCustomer1).trigger("change");
        $("#btnSmartSiroSubmit").unbind().click(function () {
            Form.SmartSiroSubmit(SoNumber, CustomerID, StipCategory, SiteID, StipSiro);
        });
        $("#btnSmartSiroCancel").unbind().click(function () {
            Control.ButtonCancelForm();
            $("#partialSmartSiroForm").fadeOut();
        });
        Control.FormatCurrency('#LeasePriceSmartSiro');
        Control.FormatCurrency('#AddPriceSmartSiro');
        $("#partialSmartSiroForm").fadeIn(1500);
    },

    SmartSiroSubmit: function (SoNumber, CustomerID, StipCategory, SiteIDTBG, StipSiro) {
        // ==============================================================================//        
        vCustomerID = CustomerID;
        vStipCategory = StipCategory;
        vSoNumber = SoNumber;
        vSiteID = SiteIDTBG;
        vStipSiro = StipSiro;
        vIDBaps = $("#IDTrxSmartSiro").val();
        vStartPeriodDate = $("#StartLeaseDateSmartSiro").val();
        vEndPeriodDate = $("#EndLeaseDateSmartSiro").val();
        vPriceLease = $("#LeasePriceSmartSiro").val();
        vAddPriceLease = $('#AddPriceSmartSiro').val();
        vBapsDate = $('#BapsDateSmartSiro').val();
        vContractDate = $("#MLAdateSmartSiro").val();
        vContractNumber = $("#MLANumberSmartSiro").val();
        vProjectDef = $('#SiteNameSmartSiro').val();
        vBapsNumber = $("#BapsNumberSmartSiro").val();
        fsApprIDCompany = $("#slApprCompanySmartSiro").val();
        fsApprIDCustomer1 = $("#slApprCustomerSmartSiro").val();
        var result = "";
        result = DataAccess.SubmitBAPS();
        if (result == "") {
            $("#partialSmartSiroForm").fadeOut();
        }
    },

    // ISAT Bulky //
    ISATForm: function (SoNumber, SiteID, CustomerID, ProductType, StipCategory, StipSiro, RegionID) {
        Control.BindingSelectApprovalCompany("#slApprCompanyIsat");
        Control.BindingSelectApprovalCustomer("#slApprCustomerIsat", CustomerID, RegionID);
        Control.BindingSelectApprovalCustomer("#slApprCustomerIsat2", CustomerID, RegionID);
        DataAccess.GetBAPSData(SoNumber, SiteID, StipSiro);
        $("#IDTrxIsat").val(vIDBaps);
        $("#JobDescIsat").val(vProjectDef);
        $("#ContractNumberIsat").val(vContractNumber);
        $("#PoNumberIsat").val(vPoNumber);
        $("#PoDateIsat").val(Common.Format.ConvertJSONDateTime(vPoDate));
        $("#BaufDateIsat").val(Common.Format.ConvertJSONDateTime(vSpkDate));
        $("#CheckDateIsat").val(Common.Format.ConvertJSONDateTime(vSdaDate));
        $("#slApprCompanyIsat").val(fsApprIDCompany).trigger("change");
        $("#slApprCustomerIsat").val(fsApprIDCustomer1).trigger("change");
        $("#slApprCustomerIsat2").val(fsApprIDCustomer2).trigger("change");
        Form.ISATDetailForm(SoNumber, StipSiro);
        $("#btnIsatSubmit").unbind().click(function () {
            Form.ISATSubmit(SoNumber, CustomerID, StipCategory, SiteID, StipSiro);
        });
        $("#btnIsatCancel").unbind().click(function () {
            Control.ButtonCancelForm();
            $("#partialiSatForm").fadeOut();
        });
        $("#partialiSatForm").fadeIn(1500);
    },

    ISATDetailForm: function (SoNumber, StipSiro) {
        var params = { strSoNumber: SoNumber, strStipSiro: StipSiro };
        $.ajax({
            url: "/api/StartBaps/getBapsMaterials",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (data != null) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    $("#MaterialIsat").val(data.Material);
                    $("#IDTrxDtlIsat").val(data.ID);
                    $("#SoNumberIsat").val(data.SoNumber);
                    $("#JobDescriptionIsat").val(data.JobDesc);
                    $("#UnitNetPriceIsat").val(Common.Format.CommaSeparation(data.UnitNetPrice));
                    $("#ResultIsat").val(data.Result);
                    $("#RemarksIsat").val(data.Description);
                    $("#DiscountIsat").val(data.Discount);
                    $("#SpkQtyIsat").val(data.SpkQty);
                    $("#SpkUnpIsat").val(Common.Format.CommaSeparation(data.SpkUNP));
                    $("#BeforeQtyIsat").val(data.RealisationBeforeQty);
                    $("#BeforeUnpIsat").val(Common.Format.CommaSeparation(data.RealisationBeforeUNP));
                    $("#CurrentQtyIsat").val(data.RealisationCurrentQty);
                    $("#CurrentUnpIsat").val(Common.Format.CommaSeparation(data.RealisationCurrentUNP));
                    $("#RemainingQtyIsat").val(data.TheRestQty);
                    $("#RemainingUnpIsat").val(Common.Format.CommaSeparation(data.TheRestUNP));
                    $("#BaufNumberIsat").val(data.BaufNumber);
                    $("#RfsPoIsat").val(Common.Format.ConvertJSONDateTime(data.RfsPoDate));
                    $("#CheckDateIsat").val(Common.Format.ConvertJSONDateTime(data.CheckedDate));
                }
            }
            else {
                $("#MaterialIsat").val('');
                $("#IDTrxDtlIsat").val('');
                $("#ContractNumberIsat").val('');
                $("#JobDescriptionIsat").val('');
                $("#UnitNetPriceIsat").val('');
                $("#ResultIsat").val('');
                $("#RemarksIsat").val('');
                $("#DiscountIsat").val('');
                $("#SpkQtyIsat").val('');
                $("#SpkUnpIsat").val('');
                $("#BeforeQtyIsat").val('');
                $("#BeforeUnpIsat").val('');
                $("#CurrentQtyIsat").val('');
                $("#CurrentUnpIsat").val('');
                $("#CurrentQtyIsat").val('');
                $("#RemainingQtyIsat").val('');
                $("#RemainingUnpIsat").val('');
                $("#RfsPoIsat").val('');
                $("#CheckDateIsat").val('');
                $("#BaufNumberIsat").val('');
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
        Control.FormatCurrency('#UnitNetPriceIsat');
        Control.FormatCurrency('#SpkUnpIsat');
        Control.FormatCurrency('#BeforeUnpIsat');
        Control.FormatCurrency('#CurrentUnpIsat');
        Control.FormatCurrency('#RemainingUnpIsat');
    },

    ISATSubmit: function (SoNumber, CustomerID, StipCategory, SiteID, StipSiro) {

        //** header **//
        vCustomerID = CustomerID;
        vStipCategory = StipCategory;
        vSoNumber = SoNumber;
        vSiteID = SiteID;
        vIDBaps = $("#IDTrxIsat").val();
        vPoDate = $("#PoDateIsat").val();
        vContractNumber = $("#ContractNumberIsat").val();
        vPoNumber = $("#PoNumberIsat").val();
        vStipSiro = StipSiro;
        vProjectDef = $("#JobDescIsat").val();
        vSpkDate = $("#BaufDateIsat").val();
        fsApprIDCompany = $("#slApprCompanyIsat").val();
        fsApprIDCustomer1 = $("#slApprCustomerIsat").val();
        fsApprIDCustomer2 = $("#slApprCustomerIsat2").val();
        //** detail **//
        dataMaterial.ID = $('#IDTrxDtlIsat').val();
        dataMaterial.SoNumber = SoNumber;
        dataMaterial.JobDesc = $('#JobDescriptionIsat').val();
        dataMaterial.Material = $('#MaterialIsat').val();
        dataMaterial.UnitNetPrice = $('#UnitNetPriceIsat').val();
        dataMaterial.SpkQty = $('#SpkQtyIsat').val();
        dataMaterial.SpkUNP = $('#SpkUnpIsat').val();
        dataMaterial.RealisationBeforeQty = $('#BeforeQtyIsat').val();
        dataMaterial.RealisationBeforeUNP = $('#BeforeUnpIsat').val();
        dataMaterial.RealisationCurrentQty = $('#CurrentQtyIsat').val();
        dataMaterial.RealisationCurrentUNP = $('#CurrentUnpIsat').val();
        dataMaterial.TheRestQty = $('#RemainingQtyIsat').val();
        dataMaterial.TheRestUNP = $('#RemainingUnpIsat').val();
        dataMaterial.Result = $('#ResultIsat').val();
        dataMaterial.Description = $('#RemarksIsat').val();
        dataMaterial.PhysicalProgress = $('#ProgressIsat').val();
        dataMaterial.Discount = $('#DiscountIsat').val();
        dataMaterial.RfsPoDate = $("#RfsPoIsat").val();
        dataMaterial.CheckedDate = $("#CheckDateIsat").val();
        dataMaterial.BaufNumber = $("#BaufNumberIsat").val();
        dataMaterial.SIRO = StipSiro;

        var result = "";
        result = DataAccess.SubmitBAPS();
        if (result == "") {
            $("#partialiSatForm").fadeOut();
        }
    },

    ISATBulkyForm: function (SoNumber, CustomerID, CompanyID, StipCategory, SiteID, BulkID, ProductID) {
        Control.BindingSelectApprovalCustomer('#slApprCustomerIsatBulky', CustomerID, '0');
        Control.BindingSelectApprovalCustomer('#slApprCustomerIsatBulky2', CustomerID, '0');
        Control.BindingSelectApprovalCompany('#slApprCompanyIsatBulky');
        $("#PartialXLAddBulky").hide();
        $("#PartialXLBulky").hide();
        Table.GridSonumbIsatBulky(SoNumber, CustomerID, SiteID, ProductID);
        Table.GridDetailsIsatBulky(BulkID, SoNumber, CustomerID, SiteID);
        $('#SearchSoNumberBulky').val(SoNumber);
        $("#btnIsatBulkySave").unbind().click(function () {

            if (Control.CheckCountRowTb("#tblDetailiSatBulky") == false)
                Common.Alert.Warning("Details is not null");
            else
                Form.ISATBulkySubmit(SoNumber, CustomerID, CompanyID, StipCategory);
        });
        $("#btnIsatBulkyCancel").unbind().click(function () {
            Control.ButtonCancelForm();
            $("#PartialiSatBulky").fadeOut();
        });
        $('#btnIsatDetailSave').unbind().click(function () {
            Form.ISATBulkyDetailSave(CustomerID, SiteID, ProductID);
        });
        $("#PartialiSatBulky").fadeIn(1500);
    },

    ISATBulkyDetailSave: function (CustomerID, SiteID, ProductID) {
        var l = Ladda.create(document.querySelector("#btnIsatDetailSave"));
        Data.ID = $('#IDTrxIsatBulky').val();
        Data.BulkyID = $('#BulkyIDIsatBulky').val();
        Data.SoNumber = $('#SoNumberIsatBulky').val();
        Data.JobDesc = $('#JobDescDetailIsatBulky').val();
        Data.Material = $('#MaterialsIsatBulky').val();
        Data.UnitNetPrice = $('#UnitNetPriceIsatBulky').val();
        Data.SpkQty = $('#QTYspkIsatBulky').val();
        Data.SpkUNP = $('#UNPspkIsatBulky').val();
        Data.RealisationBeforeQty = $('#QTYbeforeIsatBulky').val();
        Data.RealisationBeforeUNP = $('#UNPbeforeIsatBulky').val();
        Data.RealisationCurrentQty = $('#QTYcurIsatBulky').val();
        Data.RealisationCurrentUNP = $('#UNPcurIsatBulky').val();
        Data.TheRestQty = $('#QTYremainingIsatBulky').val();
        Data.TheRestUNP = $('#UNPremainingIsatBulky').val();
        Data.Result = $('#ResultIsatBulky').val();
        Data.Description = $('#RemarksIsatBulky').val();
        Data.PhysicalProgress = $('#ProgressIsatBulky').val();
        Data.Discount = $('#DiscountIsatBulky').val();
        Data.RfsPoDate = $('#RFSPOdateIsatBulky').val();
        Data.SiteNameOpr = $('#SiteNameCustIsatBulky').val();
        Data.SiteIdOpr = $('#SiteIDCustIsatBulky').val();
        Data.BaufNumber = $('#BaufNumberIsatBulky').val();
        Data.CheckedDate = $('#CheckedDateIsatBulky').val();
        Data.SIRO = $('#StipSiroIsatBulky').val();

        var params = { trxRABapsMaterials: Data, strCustomerId: $('#slSearchCustomerNameBulky').val(), strCompanyId: $('#slSearchCompanyNameBulky').val() };
        $.ajax({
            url: "/api/StartBaps/saveBapsMaterials",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    Table.GridSonumbIsatBulky(null, CustomerID, SiteID, ProductID);
                    Table.GridDetailsIsatBulky(data.BulkyID, "", CustomerID, SiteID);
                    $('#BulkyIDIsatBulky').val(data.BulkyID)
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

    ISATBulkyDetailDelete: function (ID, BulkID, SoNumber, CustomerID, SiteID) {
        Data.ID = ID;
        var params = { trxRABapsMaterials: Data };
        $.ajax({
            url: "/api/StartBaps/deleteBapsMaterials",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (data == true) {
                Common.Alert.Success("Data Success Deleted!");
                Table.GridSonumbIsatBulky(null, CustomerID, SiteID)
                Table.GridDetailsIsatBulky(BulkID, "", CustomerID, SiteID);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
    },

    ISATBulkySubmit: function (SoNumber, CustomerID, CompanyID, StipCategory) {
        vIDBapsBulky = $("#BulkyIDIsatBulky").val();
        vJobDesc = $("#JobDescIsatBulky").val();
        vApprIDCompany = $("#slApprCompanyIsatBulky").val();
        vApprIDCustomer1 = $("#slApprCustomerIsatBulky").val();
        vApprIDCustomer2 = $("#slApprCustomerIsatBulky2").val();
        vCustomerID = $("#slSearchCustomerNameBulky").val();
        vCompanyID = $("#slSearchCompanyNameBulky").val();
        vCategory = $('#slSearchStipCategoryBulky').val();
        vContractNumber = $('#ContractNumberIsatBulky').val();
        vPoDate = $('#PoDateIsatBulky').val();
        vPoNumber = $('#POnumberIsatBulky').val();
        fsApprIDCompany = $("#slApprCompanyIsatBulky").val();
        fsApprIDCustomer1 = $("#slApprCustomerIsatBulky").val();
        fsApprIDCustomer2 = $("#slApprCustomerIsatBulky2").val();
        DataAccess.SubmitBAPSBulky();
    },
    // ISAT Bulky //

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

        $('#ProjectDefXLAdd').val('');
        // ===========================================//
        $("#XlAddAntennaRRU").hide();
        $("#XlAddAntennaRF").hide();
        $("#XlAddLand").hide();
        $("#XlAddAntennaMW").hide();
    },

    HCPTForm: function (SoNumber, SiteID, CustomerID, ProductType, StipCategory, StipSiro) {
        Control.BindingSelectApprovalCompany("#slApprCompanyHCPT");
        DataAccess.GetBAPSData(SoNumber, SiteID, StipSiro);
        $("#IDTrxHCPT").val(vIDBaps);
        $("#BaseLeaseFeeHCPT").val(Common.Format.CommaSeparation(vPricePerMonth));
        $("#IssuingDateHCPT").val(Common.Format.ConvertJSONDateTime(vIssuingDate));
        $("#EffectiveDateHCPT").val(Common.Format.ConvertJSONDateTime(vEffectiveDate));
        $("#slApprCompanyHCPT").val(fsApprIDCompany).trigger("change");
        $('#btnAddMoreDtlHCPT').unbind().click(function () {
            $('#IDTrxDtlHCPT').val('');
            $('#TypeHCPT').val('');
            $('#QTYHCPT').val('');
            $('#HeightHCPT').val('');
            $('#DimensionHCPT').val('');
            $('#ModelHCPT').val('');
            $('#AzimHCPT').val('');
            $('#ValueHCPT').val('');
            $('#CableSizeHCPT').val('');
            $('#CableQtyHCPT').val('');
            $('#ManufactureHCPT').val('');
            $('#mdlDetailHCPT').modal('show');

            $('#btnDtlHCPTSave').unbind().click(function () {
                Form.HCPTDetailSave(SoNumber, StipSiro);
            });
        });
        Table.GeridDetailHCPT(SoNumber, StipSiro);
        $('#btnHcptSave').unbind().click(function () {
            if (Control.CheckCountRowTb('#tblHeightSpaceHCPT') == true) {
                Form.HCPTSubmit(SoNumber, CustomerID, StipCategory, SiteID, StipSiro);
            } else {
                Common.Alert.Warning("Details is not null");
            }

        });
        $('#btnHcptCancel').unbind().click(function () {
            Control.ButtonCancelForm();
            $("#partialHcptForm").fadeOut();
        });
        Control.FormatCurrency('#BaseLeaseFeeHCPT');
        $('#partialHcptForm').fadeIn(1000);
    },

    HCPTDetailSave: function (SoNumber, StipSiro) {
        vIDHeightSpace = $('#IDTrxDtlHCPT').val();
        vSoNumber = SoNumber;
        vStipSiro = StipSiro;
        vMaterialType = $('#TypeHCPT').val();
        vQuantity = $('#QTYHCPT').val();
        vHeight = $('#HeightHCPT').val();
        vDimension = $('#DimensionHCPT').val();
        vModel = $('#ModelHCPT').val();
        vAzim = $('#AzimHCPT').val();
        vValue = $('#ValueHCPT').val();
        vCableSize = $('#CableSizeHCPT').val();
        vCableQuantity = $('#CableQtyHCPT').val();
        vManufacture = $('#ManufactureHCPT').val();
        var result = "";
        result = DataAccess.SaveHeightSpace(SoNumber, StipSiro);
        if (result == "") {
            Table.GeridDetailHCPT(SoNumber, StipSiro);
            $('#mdlDetailHCPT').modal('hide');
        }
    },

    HCPTDetailDelete: function (SoNumber, StipSiro, ID) {
        var result = "";
        result = DataAccess.DeleteHeightSpace(ID);
        if (result == "") {
            Table.GeridDetailHCPT(SoNumber, StipSiro);
        }
    },

    HCPTSubmit: function (SoNumber, CustomerID, StipCategory, SiteID, StipSiro) {
        vCustomerID = CustomerID;
        vStipCategory = StipCategory;
        vSoNumber = SoNumber;
        vSiteID = SiteID;
        vStipSiro = StipSiro;
        vIDBaps = $('#IDTrxHCPT').val();
        vIssuingDate = $('#IssuingDateHCPT').val();
        vEffectiveDate = $('#EffectiveDateHCPT').val();
        vPricePerMonth = $("#BaseLeaseFeeHCPT").val();
        fsApprIDCompany = $("#slApprCompanyHCPT").val();
        var result = "";
        result = DataAccess.SubmitBAPS();
        if (result == "") {
            $("#partialHcptForm").hide();
        }
    },

    XLSiroForm: function (SoNumber, SiteID, CustomerID, ProductType, StipCategory, StipSiro, RegionID) {
        Control.BindingSelectApprovalCompany("#slApprCompanyXLSiro");
        Control.BindingSelectApprovalCustomer("#slApprCustomerXLSiro", CustomerID, RegionID);
        Control.BindingSelectApprovalCustomer("#slApprCustomerXLSiro2", CustomerID, RegionID);
        DataAccess.GetBAPSData(SoNumber, SiteID, StipSiro);
        $("#IDTrxXLSiro").val(vIDBaps);
        $("#LeasePricePerMonthXLSiro").val(Common.Format.CommaSeparation(vPricePerMonth));
        $("#BaseLeasePriceXLSiro").val(Common.Format.CommaSeparation(vPriceLease));
        $("#AddPriceXLSiro").val(Common.Format.CommaSeparation(vAddPriceLease));
        $("#SppsdateXLSiro").val(Common.Format.ConvertJSONDateTime(vSpkDate));
        $("#StartLeaseDateXLSiro").val(Common.Format.ConvertJSONDateTime(vStartPeriodDate));
        $("#EndLeaseDateXLSiro").val(Common.Format.ConvertJSONDateTime(vEndPeriodDate));
        $("#slApprCompanyXLSiro").val(fsApprIDCompany).trigger("change");
        $("#slApprCustomerXLSiro").val(fsApprIDCustomer1).trigger("change");
        $("#slApprCustomerXLSiro2").val(fsApprIDCustomer2).trigger("change");
        $("#btnXLSiroSubmit").unbind().click(function () {
            Form.XLSiroSubmit(SoNumber, CustomerID, StipCategory, SiteID, StipSiro);
        });
        $("#btnXLSiroCancel").unbind().click(function () {
            Control.ButtonCancelForm();
            $("#partialXLSiroForm").fadeOut();
        });
        Control.FormatCurrency('#LeasePricePerMonthXLSiro');
        $("#partialXLSiroForm").fadeIn(1500);
    },

    XLSiroSubmit: function (SoNumber, CustomerID, StipCategory, SiteID, StipSiro) {
        vCustomerID = CustomerID;
        vStipCategory = StipCategory;
        vSoNumber = SoNumber;
        vSiteID = SiteID;
        vStipSiro = StipSiro;
        vIDBaps = $("#IDTrxXLSiro").val();
        vPricePerMonth = $("#LeasePricePerMonthXLSiro").val();
        vStartPeriodDate = $("#StartLeaseDateXLSiro").val();
        vEndPeriodDate = $("#EndLeaseDateXLSiro").val();
        vPriceLease = $("#BaseLeasePriceXLSiro").val();
        vAddPriceLease = $("#AddPriceXLSiro").val();
        vSpkDate = $("#SppsdateXLSiro").val();
        fsApprIDCompany = $("#slApprCompanyXLSiro").val();
        fsApprIDCustomer1 = $("#slApprCustomerXLSiro").val();
        fsApprIDCustomer2 = $("#slApprCustomerXLSiro2").val();
        var result = "";
        result = DataAccess.SubmitBAPS();
        if (result == "") {
            $("#partialXLSiroForm").hide();
        }
    },

    XLRelocForm: function (SoNumber, SiteID, CustomerID, ProductType, StipCategory, StipSiro, RegionID) {
        Control.BindingSelectApprovalCompany("#slApprCompanyXLReloc");
        Control.BindingSelectApprovalCustomer("#slApprCustomerXLReloc", CustomerID, RegionID);
        Control.BindingSelectApprovalCustomer("#slApprCustomerXLReloc2", CustomerID, RegionID);
        DataAccess.GetBAPSData(SoNumber, SiteID, StipSiro);
        $("#IDTrxXLReloc").val(vIDBaps);
        $("#SpkDateXLReloc").val(Common.Format.ConvertJSONDateTime(vSpkDate));
        $("#StartLeaseDateXLReloc").val(Common.Format.ConvertJSONDateTime(vStartPeriodDate));
        $("#EndLeaseDateXLReloc").val(Common.Format.ConvertJSONDateTime(vEndPeriodDate));
        $("#PairingSiteXlReloc").val(vProjectDef);
        $("#MaxHeightXlReloc").val(vNumberOfAntenna);
        $("#HeightSpaceXlReloc").val(vNoted);
        $("#PriceLeaseXlReloc").val(Common.Format.CommaSeparation(vPriceLease));
        $("#slApprCompanyXLReloc").val(fsApprIDCompany).trigger("change");
        $("#slApprCustomerXLReloc").val(fsApprIDCustomer1).trigger("change");
        $("#slApprCustomerXLReloc2").val(fsApprIDCustomer2).trigger("change");
        $("#btnXLRelocSubmit").unbind().click(function () {
            Form.XLRelocSubmit(SoNumber, CustomerID, StipCategory, SiteID, StipSiro);
        });
        $("#btnXLRelocCancel").unbind().click(function () {
            Control.ButtonCancelForm();
            $("#partialXLRelocForm").fadeOut();
        });
        Control.FormatCurrency('#PriceLeaseXlReloc');
        $("#partialXLRelocForm").fadeIn(1500);
    },

    XLRelocSubmit: function (SoNumber, CustomerID, StipCategory, SiteID, StipSiro) {
        vCustomerID = CustomerID;
        vStipCategory = StipCategory;
        vSoNumber = SoNumber;
        vSiteID = SiteID;
        vStipSiro = StipSiro;
        vIDBaps = $("#IDTrxXLReloc").val();
        vStartPeriodDate = $("#StartLeaseDateXLReloc").val();
        vEndPeriodDate = $("#EndLeaseDateXLReloc").val();
        vSpkDate = $("#SpkDateXLReloc").val();
        vProjectDef = $("#PairingSiteXlReloc").val();
        vNumberOfAntenna = $("#MaxHeightXlReloc").val();
        vNoted = $("#HeightSpaceXlReloc").val();
        vPriceLease = $("#PriceLeaseXlReloc").val();
        fsApprIDCompany = $("#slApprCompanyXLReloc").val();
        fsApprIDCustomer1 = $("#slApprCustomerXLReloc").val();
        fsApprIDCustomer2 = $("#slApprCustomerXLReloc2").val();
        var result = "";
        result = DataAccess.SubmitBAPS();
        if (result == "") {
            $("#partialXLRelocForm").hide();
        }
    },

    XLNewForm: function (SoNumber, SiteID, CustomerID, ProductType, StipCategory, StipSiro, RegionID) {
        Control.BindingSelectApprovalCompany("#slApprCompanyXLNew");
        Control.BindingSelectApprovalCustomer("#slApprCustomerXLNew", CustomerID, RegionID);
        Control.BindingSelectApprovalCustomer("#slApprCustomerXLNew2", CustomerID, RegionID);
        DataAccess.GetBAPSData(SoNumber, SiteID, StipSiro);
        $("#IDTrxXLNew").val(vIDBaps);
        $("#BaseLeasePriceXLNew").val(Common.Format.CommaSeparation(vPricePerMonth));
        $("#AddBaseLeasePriceXLNew").val(Common.Format.CommaSeparation(vAddPriceLease));
        $("#MaxHeightXlNew").val(vNumberOfAntenna);
        $("#HeightSpaceXlNew").val(vNoted);
        $("#SppsdateXLNew").val(Common.Format.ConvertJSONDateTime(vSpkDate));
        $("#StartLeaseDateXLNew").val(Common.Format.ConvertJSONDateTime(vStartPeriodDate));
        $("#EndLeaseDateXLNew").val(Common.Format.ConvertJSONDateTime(vEndPeriodDate));
        $("#slApprCompanyXLNew").val(fsApprIDCompany).trigger("change");
        $("#slApprCustomerXLNew").val(fsApprIDCustomer1).trigger("change");
        $("#slApprCustomerXLNew2").val(fsApprIDCustomer2).trigger("change");
        $("#btnXLNewSubmit").unbind().click(function () {
            Form.XLNewSubmit(SoNumber, CustomerID, StipCategory, SiteID, StipSiro);
        });
        $("#btnXLNewCancel").unbind().click(function () {
            Control.ButtonCancelForm();
            $("#partialXLNewForm").fadeOut();
        });
        Control.FormatCurrency('#BaseLeasePriceXLNew');
        Control.FormatCurrency('#AddBaseLeasePriceXLNew');
        $("#partialXLNewForm").fadeIn(1500);
    },

    XLNewSubmit: function (SoNumber, CustomerID, StipCategory, SiteID, StipSiro) {
        vCustomerID = CustomerID;
        vStipCategory = StipCategory;
        vSoNumber = SoNumber;
        vSiteID = SiteID;
        vStipSiro = StipSiro;
        vIDBaps = $("#IDTrxXLNew").val();
        vStartPeriodDate = $("#StartLeaseDateXLNew").val();
        vEndPeriodDate = $("#EndLeaseDateXLNew").val();
        vSpkDate = $("#SppsdateXLNew").val();
        vPricePerMonth = $("#BaseLeasePriceXLNew").val();
        vAddPriceLease = $("#AddBaseLeasePriceXLNew").val();
        vNumberOfAntenna = $("#MaxHeightXlNew").val();
        vNoted = $("#HeightSpaceXlNew").val();
        fsApprIDCompany = $("#slApprCompanyXLNew").val();
        fsApprIDCustomer1 = $("#slApprCustomerXLNew").val();
        fsApprIDCustomer2 = $("#slApprCustomerXLNew2").val();
        var result = "";
        result = DataAccess.SubmitBAPS();
        if (result == "") {
            $("#partialXLNewForm").hide();
        }
    },

    STIForm: function (SoNumber, SiteID, CustomerID, ProductType, StipCategory, StipSiro, RegionID) {
        Control.BindingSelectApprovalCompany("#slApprCompanySTI");
        Control.BindingSelectApprovalCustomer("#slApprCustomerSTI", CustomerID, RegionID);
        DataAccess.GetBAPSData(SoNumber, SiteID, StipSiro);
        $("#IDTrxSTI").val(vIDBaps);
        $("#StartLeaseDateSTI").val(Common.Format.ConvertJSONDateTime(vStartPeriodDate));
        $("#EndLeaseDateSTI").val(Common.Format.ConvertJSONDateTime(vEndPeriodDate));
        $("#BuldingHeightSTI").val(vBuildingHeight);
        $("#LandSizeSTI").val(vLandSize);
        $("#PowerSizeSTI").val(vPowerSize);
        $("#OpexPriceSTI").val(Common.Format.CommaSeparation(vOpexPrice));
        $("#AddPriceSTI").val(Common.Format.CommaSeparation(vAddPriceLease));
        $("#OtherPriceSTI").val(Common.Format.CommaSeparation(vOtherPrice));
        $("#LeasePriceSTI").val(Common.Format.CommaSeparation(vPriceLease));
        $("#BaufDateSTI").val(Common.Format.ConvertJSONDateTime(vBaufDate));
        $("#InstallPermitDateSTI").val(Common.Format.ConvertJSONDateTime(vInstallPermitDate));
        Table.GeridDetailSTI(SoNumber, StipSiro);
        $("#slApprCompanySTI").val(fsApprIDCompany).trigger("change");
        $("#slApprCustomerSTI").val(fsApprIDCustomer1).trigger("change");
        $('#btnAddMoreDtlSTI').unbind().click(function () {
            $('#IDTrxDtlSTI').val('');
            $('#TypeSTI').val('');
            $('#QTYSTI').val('');
            $('#HeightSTI').val('');
            $('#DimensionSTI').val('');
            $('#ModelSTI').val('');
            $('#AzimSTI').val('');
            $('#ValueSTI').val('');
            $('#CableSizeSTI').val('');
            $('#CableQtySTI').val('');
            $('#ManufactureSTI').val('');
            $('#mdlDetailSTI').modal('show');
            $('#btnDtlSTISave').unbind().click(function () {
                Form.STIDetailSave(SoNumber, StipSiro);
            });
        });
        $('#btnSTISave').unbind().click(function () {
            Form.STISubmit(SoNumber, CustomerID, StipCategory, SiteID, StipSiro);
        });
        $('#btnSTICancel').unbind().click(function () {
            Control.ButtonCancelForm();
            $("#partialSTIForm").fadeOut();
        });
        Control.FormatCurrency('#OpexPriceSTI');
        Control.FormatCurrency('#AddPriceSTI');
        Control.FormatCurrency('#OtherPriceSTI');
        Control.FormatCurrency('#LeasePriceSTI');
        $('#partialSTIForm').fadeIn(1000);

    },

    STIDetailSave: function (SoNumber, StipSiro) {
        vSoNumber = SoNumber;
        vStipSiro = StipSiro;
        vIDHeightSpace = $('#IDTrxDtlSTI').val();
        vMaterialType = $('#TypeSTI').val();
        vQuantity = $('#QTYSTI').val();
        vHeight = $('#HeightSTI').val();
        vDimension = $('#DimensionSTI').val();
        vModel = $('#ModelSTI').val();
        vAzim = $('#AzimSTI').val();
        vValue = $('#ValueSTI').val();
        vCableSize = $('#CableSizeSTI').val();
        vCableQuantity = $('#CableQtySTI').val();
        vManufacture = $('#ManufactureSTI').val();
        var result = "";
        result = DataAccess.SaveHeightSpace(SoNumber, StipSiro);
        if (result == "") {
            Table.GeridDetailSTI(SoNumber, StipSiro);
            $('#mdlDetailSTI').modal('hide');
        }
    },

    STIDetailDelete: function (SoNumber, StipSiro, ID) {
        var result = "";
        result = DataAccess.DeleteHeightSpace(ID);
        if (result == "") {
            Table.GeridDetailSTI(SoNumber, StipSiro);
        }
    },

    STISubmit: function (SoNumber, CustomerID, StipCategory, SiteID, StipSiro) {
        // ==============================================================================//   
        vCustomerID = CustomerID;
        vStipCategory = StipCategory;
        vSoNumber = SoNumber;
        vSiteID = SiteID;
        vStipSiro = StipSiro;
        // ==============================================================================//   
        vID = $('#IDTrxSTI').val();
        vBuildingHeight = $("#BuldingHeightSTI").val();
        vLandSize = $("#LandSizeSTI").val();
        vPowerSize = $("#PowerSizeSTI").val();
        vOpexPrice = $("#OpexPriceSTI").val();
        vAddPriceLease = $("#AddPriceSTI").val();
        vOtherPrice = $("#OtherPriceSTI").val();
        vPriceLease = $("#LeasePriceSTI").val();
        vBaufDate = $("#BaufDateSTI").val();
        vInstallPermitDate = $("#InstallPermitDateSTI").val();
        vStartPeriodDate = $('#StartLeaseDateSTI').val();
        vEndPeriodDate = $('#EndLeaseDateSTI').val();
        fsApprIDCompany = $("#slApprCompanySTI").val();
        fsApprIDCustomer1 = $("#slApprCustomerSTI").val();
        var result = "";
        result = DataAccess.SubmitBAPS();
        if (result == "") {
            $("#partialSTIForm").fadeOut();
        }
    },

    INUXForm: function (SoNumber, SiteID, CustomerID, ProductType, StipCategory, StipSiro, RegionID) {
        Control.BindingSelectApprovalCompany("#slApprCompanyINUX");
        Control.BindingSelectApprovalCustomer("#slApprCustomerINUX", CustomerID, RegionID);
        Control.BindingSelectApprovalCustomer("#slApprCustomerINUX2", CustomerID, RegionID);
        DataAccess.GetBAPSData(SoNumber, SiteID, StipSiro);
        $("#IDTrxINUX").val(vIDBaps);
        $("#BuildingHeightINUX").val(vBuildingHeight);
        $("#LeasePriceINUX").val(Common.Format.CommaSeparation(vPriceLease))
        $("#AddPriceINUX").val(Common.Format.CommaSeparation(vAddPriceLease))
        $("#OpexPriceINUX").val(Common.Format.CommaSeparation(vOpexPrice));
        $("#OtherPriceINUX").val(Common.Format.CommaSeparation(vOtherPrice));
        $("#StartLeaseDateINUX").val(Common.Format.ConvertJSONDateTime(vStartPeriodDate));
        $("#EndLeaseDateINUX").val(Common.Format.ConvertJSONDateTime(vEndPeriodDate));
        Table.GeridDetailINUX(SoNumber, StipSiro);
        $("#slApprCompanyINUX").val(fsApprIDCompany).trigger("change");
        $("#slApprCustomerINUX").val(fsApprIDCustomer1).trigger("change");
        $("#slApprCustomerINUX2").val(fsApprIDCustomer2).trigger("change");
        $('#btnAddMoreDtlINUX').unbind().click(function () {
            $('#IDTrxDtlINUX').val('');
            $('#TypeINUX').val('');
            $('#QTYINUX').val('');
            $('#HeightINUX').val('');
            $('#DimensionINUX').val('');
            $('#ModelINUX').val('');
            $('#AzimINUX').val('');
            $('#ValueINUX').val('');
            $('#CableSizeINUX').val('');
            $('#CableQtyINUX').val('');
            $('#ManufactureINUX').val('');
            $('#mdlDetailINUX').modal('show');
            $('#btnDtlINUXSave').unbind().click(function () {
                Form.INUXDetailSave(SoNumber, StipSiro);
            });
        });
        $('#btnINUXSave').unbind().click(function () {
            if (Control.CheckCountRowTb('#tblHeightSpaceINUX') == true) {
                Form.INUXSubmit(SoNumber, CustomerID, StipCategory, SiteID, StipSiro);
            } else {
                Common.Alert.Warning("Details is not null");
            }

        });
        $('#btnINUXCancel').unbind().click(function () {
            Control.ButtonCancelForm();
            $("#partialINUXForm").fadeOut();
        });
        Control.FormatCurrency('#AddPriceINUX');
        Control.FormatCurrency('#OtherPriceINUX');
        Control.FormatCurrency('#OpexPriceINUX');
        Control.FormatCurrency('#LeasePriceINUX');
        $('#partialINUXForm').fadeIn(1000);
    },

    INUXDetailSave: function (SoNumber, StipSiro) {
        vSoNumber = SoNumber;
        vStipSiro = StipSiro;
        vIDHeightSpace = $('#IDTrxDtlINUX').val();
        vMaterialType = $('#TypeINUX').val();
        vQuantity = $('#QTYINUX').val();
        vHeight = $('#HeightINUX').val();
        vDimension = $('#DimensionINUX').val();
        vModel = $('#ModelINUX').val();
        vAzim = $('#AzimINUX').val();
        vValue = $('#ValueINUX').val();
        vCableSize = $('#CableSizeINUX').val();
        vCableQuantity = $('#CableQtyINUX').val();
        vManufacture = $('#ManufactureINUX').val();
        var result = "";
        result = DataAccess.SaveHeightSpace(SoNumber, StipSiro);
        if (result == "") {
            Table.GeridDetailINUX(SoNumber, StipSiro);
            $('#mdlDetailINUX').modal('hide');
        }
    },

    INUXDetailDelete: function (SoNumber, StipSiro, ID) {
        var result = "";
        result = DataAccess.DeleteHeightSpace(ID);
        if (result == "") {
            Table.GeridDetailINUX(SoNumber, StipSiro);
        }
    },

    INUXSubmit: function (SoNumber, CustomerID, StipCategory, SiteID, StipSiro) {
        vCustomerID = CustomerID;
        vStipCategory = StipCategory;
        vSoNumber = SoNumber;
        vSiteID = SiteID;
        vStipSiro = StipSiro;
        vIDBaps = $('#IDTrxINUX').val();
        vBuildingHeight = $('#BuildingHeightINUX').val();
        vOtherPrice = $('#OtherPriceINUX').val();
        vOpexPrice = $('#OpexPriceINUX').val();
        vAddPriceLease = $('#AddPriceINUX').val();
        vPriceLease = $('#LeasePriceINUX').val();
        vStartPeriodDate = $('#StartLeaseDateINUX').val();
        vEndPeriodDate = $('#EndLeaseDateINUX').val();
        fsApprIDCompany = $("#slApprCompanyINUX").val();
        fsApprIDCustomer1 = $("#slApprCustomerINUX").val();
        fsApprIDCustomer2 = $("#slApprCustomerINUX2").val();
        var result = "";
        result = DataAccess.SubmitBAPS();
        if (result == "") {
            $("#partialINUXForm").fadeOut();
        }
    },

    SFNewForm: function (SoNumber, SiteID, CustomerID, ProductType, StipCategory, StipSiro, RegionID) {
        // == get data == //
        Control.BindingSelectApprovalCompany("#slApprCompanySFNew");
        Control.BindingSelectApprovalCustomer("#slApprCustomerSFNew", CustomerID, RegionID);
        DataAccess.GetBAPSData(SoNumber, SiteID, StipSiro);
        $("#IDTrxSFNew").val(vIDBaps);
        $("#RFEDateSFNew").val(Common.Format.ConvertJSONDateTime(vSpkDate));
        $("#LaterInterestNumbSFNew").val(vContractNumber);
        $("#LaterInterestDateSFNew").val(Common.Format.ConvertJSONDateTime(vContractDate));
        $("#StartLeaseDateSFNew").val(Common.Format.ConvertJSONDateTime(vStartPeriodDate));
        $("#EndLeaseDateSFNew").val(Common.Format.ConvertJSONDateTime(vEndPeriodDate));
        $("#LeasePriceSFNew").val(Common.Format.CommaSeparation(vPriceLease));
        $("#LandSizeSFNew").val(vLandSize);
        $("#BapsNumberSFNew").val(vBapsNumber);
        $("#MLADateSFNew").val(Common.Format.ConvertJSONDateTime(vMlaDate));
        $("#slApprCompanySFNew").val(fsApprIDCompany).trigger("change");
        $("#slApprCustomerSFNew").val(fsApprIDCustomer1).trigger("change");
        $("#btnSFNewSave").unbind().click(function () {
            Form.SFNewSubmit(SoNumber, CustomerID, StipCategory, SiteID, StipSiro);
        });
        $("#btnSFNewCancel").unbind().click(function () {
            Control.ButtonCancelForm();
            $("#partialSFNewForm").fadeOut();
        });
        $("#partialSFNewForm").fadeIn(1500);
        Control.FormatCurrency('#LeasePriceSFNew');
    },

    SFNewSubmit: function (SoNumber, CustomerID, StipCategory, SiteID, StipSiro) {
        // ==============================================================================//        
        vSoNumber = SoNumber;
        vCustomerID = CustomerID;
        vStipCategory = StipCategory;
        vSiteID = SiteID;
        vStipSiro = StipSiro;
        //========================================================================//
        vIDBaps = $("#IDTrxSFNew").val();
        vStartPeriodDate = $("#StartLeaseDateSFNew").val();
        vEndPeriodDate = $("#EndLeaseDateSFNew").val();
        vPriceLease = $("#LeasePriceSFNew").val();
        vSpkDate = $("#RFEDateSFNew").val();
        vContractNumber = $("#LaterInterestNumbSFNew").val();
        vContractDate = $("#LaterInterestDateSFNew").val();
        vLandSize = $("#LandSizeSFNew").val();
        vMlaDate = $("#MLADateSFNew").val();
        vBapsNumber = $("#BapsNumberSFNew").val();
        fsApprIDCompany = $("#slApprCompanySFNew").val();
        fsApprIDCustomer1 = $("#slApprCustomerSFNew").val();
        fsLandStatus = $("#slLandStatusSFNew").val();
        vSiteType = $("#SiteTypeSFNew").val();
        fsTowerType = $("#slTowerTypeSFNew").val();
        vServiceType = $("#ServiceTypeSFNew").val();
        vTowerType = $("#TowerTypeSFNew").val();
        //========================================================================//
        result = DataAccess.SubmitBAPS();
        if (result == "") {
            $("#partialSFNewForm").fadeOut();
        }
    },

    SFSIROForm: function (SoNumber, SiteID, CustomerID, ProductType, StipCategory, StipSiro, RegionID) {
        // == get data == //
        Control.BindingSelectApprovalCompany("#slApprCompanySFSiro");
        Control.BindingSelectApprovalCustomer("#slApprCustomerSFSiro", CustomerID, RegionID);
        DataAccess.GetBAPSData(SoNumber, SiteID, StipSiro);
        //== set data ==//
        $("#IDTrxSFSIRO").val(vIDBaps);
        $("#MLADateSFSIRO").val(Common.Format.ConvertJSONDateTime(vMlaDate));
        $("#MLANumberSFSIRO").val(vContractNumber);
        $("#BapsDateSFSIRO").val(Common.Format.ConvertJSONDateTime(vBapsDate));
        $("#BapsNumberSFSIRO").val(vBapsNumber);
        $("#StartLeaseDateSFSIRO").val(Common.Format.ConvertJSONDateTime(vStartPeriodDate));
        $("#EndLeaseDateSFSIRO").val(Common.Format.ConvertJSONDateTime(vEndPeriodDate));
        $("#LeasePriceSFSIRO").val(Common.Format.CommaSeparation(vPriceLease));
        $("#AddPriceSFSIRO").val(Common.Format.CommaSeparation(vAddPriceLease));
        $("#slApprCompanySFSiro").val(fsApprIDCompany).trigger("change");
        $("#slApprCustomerSFSiro").val(fsApprIDCustomer1).trigger("change");
        $("#btnSFSIROSave").unbind().click(function () {
            Form.SFSIROSubmit(SoNumber, CustomerID, StipCategory, SiteID, StipSiro);
        });
        $("#btnSFSIROCancel").unbind().click(function () {
            Control.ButtonCancelForm();
            $("#partialSFSIROForm").fadeOut();
        });
        Control.FormatCurrency('#LeasePriceSFSIRO');
        Control.FormatCurrency('#AddPriceSFSIRO');
        $("#partialSFSIROForm").fadeIn(1500);
    },

    SFSIROSubmit: function (SoNumber, CustomerID, StipCategory, SiteID, StipSiro) {
        // ==============================================================================//        
        vSoNumber = SoNumber;
        vCustomerID = CustomerID;
        vStipCategory = StipCategory;
        vSiteID = SiteID;
        vStipSiro = StipSiro;
        //========================================================================//
        vIDBaps = $("#IDTrxSFSIRO").val();
        vMlaDate = $("#MLADateSFSIRO").val();
        vContractNumber = $("#MLANumberSFSIRO").val();
        vBapsDate = $("#BapsDateSFSIRO").val();
        vBapsNumber = $("#BapsNumberSFSIRO").val();
        vStartPeriodDate = $("#StartLeaseDateSFSIRO").val();
        vEndPeriodDate = $("#EndLeaseDateSFSIRO").val();
        vPriceLease = $("#LeasePriceSFSIRO").val();
        vAddPriceLease = $("#AddPriceSFSIRO").val();
        fsApprIDCompany = $("#slApprCompanySFSiro").val();
        fsApprIDCustomer1 = $("#slApprCustomerSFSiro").val();
        //========================================================================//
        result = DataAccess.SubmitBAPS();
        if (result == "") {
            $("#partialSFSIROForm").fadeOut();
        }
    },

    ApprovalSubmit: function (Sonumber, siro, actID, statusAppr) {
        var params = { strSoNumber: Sonumber, strStipSiro: siro, strActID: actID, strStatusAppr: statusAppr };
        $.ajax({
            url: "/api/StartBaps/submitApproval",
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

                } else {
                    Common.Alert.Warning(data.ErrorMessage);
                }
            }

        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)

        });

    },

    ApprovalSubmitBulky: function (actID, statusAppr, customerID, category, bulkyID) {
        var params = { strActID: actID, strStatusAppr: statusAppr, strCustomerId: customerID, strCategory: category, strBulkID: bulkyID };
        $.ajax({
            url: "/api/StartBaps/submitApprovalBulky",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    Table.GriValidationBulkyPrint();
                    Common.Alert.Success("Data Success Saved!");

                } else {
                    Common.Alert.Warning(data.ErrorMessage);
                }
            }

        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)

        });

    },
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

    GridValidationPrint: function () {
        Table.Init("#tblValidationListPrint");
        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();
        var params = {
            strCustomerId: $("#slSearchCustomerID").val() == null ? "" : $("#slSearchCustomerID").val(),
            strCompanyId: $("#slSearchCompanyID").val() == null ? "" : $("#slSearchCompanyID").val(),
            strProductId: $("#slSearchProductID").val() == null ? "" : $("#slSearchProductID").val(),
            strSoNumber: $("#slSearchSoNumber").val() == null ? "" : $("#slSearchSoNumber").val(),
            strSiteID: $("#slSearchSiteID").val() == null ? "" : $("#slSearchSiteID").val(),
            strStipID: $("#slStipCategory").val() == null ? "" : $("#slStipCategory").val(),
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
                "url": "/api/StartBaps/getListSoNumber",
                "type": "POST",
                "data": params,
                "cache": false,
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
                        if (full.Label == "NEWBAPS_PRINT" && full.BapsPrintInput == false) {
                            strReturn += "<i class='fa fa-edit btn btn-xs green link-TrxDataValidation' title='Input Data'></i>";
                            strReturn += "<i class='fa fa-print btn btn-xs blue' title='Print' disabled='disabled'></i>";
                            strReturn += "<i class='fa fa-share btn btn-xs blue' disabled='disabled'></i>";
                            strReturn += "<i class='fa fa-step-forward btn btn-xs red link-SkipPrint' title='Skip Print'></i>";
                        }
                        else if ((full.Label == "NEWBAPS_PRINT") && full.BapsPrintInput == true) {
                            strReturn += "<i class='fa fa-edit btn btn-xs green link-TrxDataValidation' title='Input Data'></i>";
                            strReturn += "<i class='fa fa-print btn btn-xs blue link-PrintPDF' title='Print'></i>";
                            strReturn += "<i class='fa fa-share btn btn-xs blue link-SubmitToCust' title='Submit To Customer'></i>";
                            strReturn += "<i class='fa fa-step-forward btn btn-xs red' title='Skip Print' disabled='disabled'></i>";
                        }
                        else if (full.Label == "NEWBAPS_SUBMITOPR" && full.BapsPrintInput == true) {
                            strReturn += "<i class='fa fa-edit btn btn-xs green link-TrxDataValidation' title='Input Data'></i>";
                            strReturn += "<i class='fa fa-print btn btn-xs blue' title='Print' disabled='disabled'></i>";
                            strReturn += "<i class='fa fa-share btn btn-xs blue link-Approval'></i>";
                            strReturn += "<i class='fa fa-step-forward btn btn-xs red' title='Skip Print' disabled='disabled'></i>";
                        }
                        else if ((full.Label == "NEWBAPS_INPUT" || full.Label == "NEWBAPS_DONE") && full.BapsPrintInput == true) {
                            strReturn += "<i class='fa fa-edit btn btn-xs green link-TrxDataValidation' title='Input Data'></i>";
                            strReturn += "<i class='fa fa-print btn btn-xs blue' title='Print' disabled='disabled'></i>";
                            strReturn += "<i class='fa fa-share btn btn-xs blue' disabled='disabled'></i>";
                            strReturn += "<i class='fa fa-step-forward btn btn-xs red' title='Skip Print' disabled='disabled'></i>";
                        } else {
                            strReturn += "<i class='fa fa-edit btn btn-xs green' title='Input Data' disabled='disabled'></i>";
                            // strReturn += "<i class='fa fa-edit btn btn-xs green link-TrxDataValidation' title='Input Data'></i>";
                            strReturn += "<i class='fa fa-print btn btn-xs blue' title='Print' disabled='disabled'></i>";
                            strReturn += "<i class='fa fa-share btn btn-xs blue' disabled='disabled'></i>";
                            strReturn += "<i class='fa fa-step-forward btn btn-xs red' title='Skip Print' disabled='disabled'></i>";
                        }
                        return strReturn;
                    }
                },
                { data: "SoNumber" },
                { data: "ActivityName" },
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
                { data: "CompanyID" },
                { data: "ActivityID" },
                { data: "RegionID" },
                { data: "RegionName" },
                { data: "StartPeriodDate" },
                { data: "EndPeriodDate" },
                { data: "EffectiveBapsDate" },
                { data: "BaseLeasePrice" },
                { data: "AddLeasePrice" },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [1, 10, 13, 14], "class": "text-center" }, { "targets": [15, 16, 17, 19, 20, 21, 22, 23], "visible": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblList.data())) {
                    $(".panelSearchResult").fadeIn(1000);
                }
                l.stop(); App.unblockUI('.panelSearchResult');
                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0; i < Data.RowSelected.length; i++) {
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
            Control.ClearValues();
            vStartPeriodDate = row.StartPeriodDate;
            vEndPeriodDate = row.EndPeriodDate;
            vEffectiveDate = row.EffectiveBapsDate;
            vAddPriceLease = row.AddLeasePrice;
            vPriceLease = row.BaseLeasePrice;
            Form.GetForm($.trim(row.SoNumber), $.trim(row.CustomerID), $.trim(row.SiteID), $.trim(row.CompanyID), $.trim(row.CompanyName), $.trim(row.CustomerSiteID), $.trim(row.CustomerSiteName), $.trim(row.Product), $.trim(row.StipCode), $.trim(row.SIRO), $.trim(row.RegionID));
            $('.btnSubmitTrx').prop('disabled', false);
            if (row.Label != "NEWBAPS_PRINT") {
                $('.btnSubmitTrx').prop('disabled', true);
            }
            fsActLabel = row.Label;

        });
        $("#tblValidationListPrint tbody").on("click", ".link-PrintPDF", function (e) {
            var table = $("#tblValidationListPrint").DataTable();
            var row = table.row($(this).parents('tr')).data();
            var stipCategory = $.trim(row.SIRO) > 0 ? "SIRO" : $.trim(row.StipCode);
            Control.PrintPDF($.trim(row.SoNumber), $.trim(row.CustomerID), '00', 'header', stipCategory, $.trim(row.Product));
        });
        $("#tblValidationListPrint tbody").on("click", ".link-SubmitToCust", function (e) {
            var table = $("#tblValidationListPrint").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $('#mdlSubmitToCust').modal('show');
            $('#btnSubmitCustOK').unbind().click(function () {
                Form.ApprovalSubmit(row.SoNumber, row.SIRO, row.ActivityID, "submit");
                $('#mdlSubmitToCust').modal('hide');
            });
            $('#btnSubmitCustCancel').unbind().click(function () {
                $('#mdlSubmitToCust').modal('hide');
            });
        });
        $("#tblValidationListPrint tbody").on("click", ".link-Approval", function (e) {
            var table = $("#tblValidationListPrint").DataTable();
            var row = table.row($(this).parents('tr')).data();
            Control.BingSelectNextActivity(row.ActivityID);
            $('#mdlConfirmNextProcess').modal('show');
            $('#btnSubmitApprOk').unbind().click(function () {
                //Form.ApprovalSubmit($('#SoNumberAppr2').val(), $('#SIROAppr2').val(), $('#ActivityID2').val(), $('#slStatusAppr').text());
                Form.ApprovalSubmit(row.SoNumber, row.SIRO, row.ActivityID, $('#slStatusAppr').val());
                $('#mdlConfirmNextProcess').modal('hide');
            });
            $('#btnSubmitApprCancel').unbind().click(function () {
                $('#mdlConfirmNextProcess').modal('hide');
            });
        });
        $("#tblValidationListPrint tbody").on("click", ".link-SkipPrint", function (e) {
            var table = $("#tblValidationListPrint").DataTable();
            var row = table.row($(this).parents('tr')).data();
            DataAccess.SkipPrintBAPS(row.SoNumber, row.SIRO);
        });
    },

    GriValidationBulkyPrint: function () {
        Table.Init("#tblValidationListBulkyPrint");
        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();
        fsCompanyId = $("#slSearchCompanyID").val() == null ? "" : $("#slSearchCompanyID").val();
        var fsBulkNumber = $("#slBulkyNumber").val() == null ? "" : $("#slBulkyNumber").val();
        fsCustomerID = $("#slSearchCustomerID").val() == null ? "" : $("#slSearchCustomerID").val();
        var params = {
            strCompanyId: fsCompanyId,
            strBulkNumber: fsBulkNumber,
            strCustomerId: fsCustomerID
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
                "url": "/api/StartBaps/getListBapsPrintBulky",
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

                        strReturn += "<i class='fa fa-edit btn btn-xs green link-TrxDataValidationBulky' data-toggle='modal' title='edit'></i> &nbsp;&nbsp;";

                        if (full.Label == "NEWBAPS_PRINT") {
                            strReturn += "<i class='fa fa-print btn btn-xs blue link-PrintPDFHeader'  data-toggle='modal' title='Print Header'></i>&nbsp;&nbsp;";
                            if ((full.Category == "NON-ADDITIONAL" && full.CustomerID == "XL") || full.CustomerID == "ISAT") {
                                strReturn += "<i class='fa fa-print btn btn-xs yellow link-PrintPDFDetails'  data-toggle='modal' title='Print Detail'></i>&nbsp;&nbsp;";
                            } else {
                                strReturn += "<i class='fa fa-print btn btn-xs yellow' disabled='disabled'></i>&nbsp;&nbsp;";
                            }
                            strReturn += "<i class='fa fa-share btn btn-xs blue link-SubmitToCustBulky' title='Submit To Customer'></i>";
                        } else {
                            strReturn += "<i class='fa fa-print btn btn-xs blue' disabled='disabled' title='Print Header'></i>&nbsp;&nbsp;";
                            if ((full.Category == "NON-ADDITIONAL" && full.CustomerID == "XL") || full.CustomerID == "ISAT") {
                                strReturn += "<i class='fa fa-print btn btn-xs yellow' disabled='disabled' title='Print Detail'></i>&nbsp;&nbsp;";
                            }
                            else {
                                strReturn += "<i class='fa fa-print btn btn-xs yellow' disabled='disabled'></i>&nbsp;&nbsp;";
                            }
                        }
                        if (full.Label == "NEWBAPS_SUBMITOPR") {
                            strReturn += "<i class='fa fa-share btn btn-xs blue link-ApprovalBulky'></i>";
                        }
                        if (full.Label == "NEWBAPS_INPUT") {
                            strReturn += "<i class='fa fa-share btn btn-xs blue' disabled='disabled'></i>";
                        }
                        return strReturn;
                    }
                },
                { data: "BulkNumber" },
                { data: "ActivityName" },
                { data: "Category" },
                { data: "CompanyID" },
                { data: "CustomerID" },
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "Noted" },
                { data: "ID" },
                { data: "ApprIDCompany" },
                { data: "ApprIDCustomer1" },
                { data: "Description" },
                { data: "GrRecievedDate" },
                { data: "PerformGR" },
                { data: "TicketNumber" },
                { data: "PriceLease" },
                { data: "StartPeriodDate" },
                { data: "EndPeriodDate" },
                { data: "PoDate" },
                { data: "ContractDate" },
                { data: "HeightSpace" },
                { data: "CompanyInfo" },
                { data: "ActivityID" },
                { data: "Label" },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [9], "visible": false }, { "targets": [10], "visible": false }, { "targets": [1], "class": "text-center" }
                , { "targets": [11], "visible": false }, { "targets": [12], "visible": false }, { "targets": [13], "visible": false }, { "targets": [14], "visible": false },
            { "targets": [15], "visible": false }, { "targets": [16], "visible": false }, { "targets": [17], "visible": false }, { "targets": [18], "visible": false },
            { "targets": [19], "visible": false }, { "targets": [20], "visible": false }, { "targets": [21], "visible": false }, { "targets": [22], "visible": false }, { "targets": [23], "visible": false }, { "targets": [24], "visible": false }],

            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblList.data())) {
                    $(".panelSearchResult").fadeIn(1000);
                }
                l.stop(); App.unblockUI('.panelSearchResult');
                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        if (!Helper.IsElementExistsInArray(parseInt(item), Data.RowSelectedSite))
                            $("#Row" + item).addClass("active");
                    }
                }
            },
            "scrollY": 400,
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
        $("#tblValidationListBulkyPrint tbody").unbind();
        $("#tblValidationListBulkyPrint tbody").on("click", ".link-TrxDataValidationBulky", function (e) {
            var table = $("#tblValidationListBulkyPrint").DataTable();
            var row = table.row($(this).parents('tr')).data();
            Control.ClearValues();
            vBulkyNumber = row.BulkNumber;
            $(".panelTab").hide();
            $(".filter").hide();
            $("#partialBapsValidationForm").hide();
            $("#slSearchCompanyNameBulky").val($.trim(row.CompanyID)).trigger('change');
            $("#slSearchCustomerNameBulky").val($.trim(row.CustomerID)).trigger('change');
            $("#slSearchCompanyNameBulky").prop("disabled", true);
            $("#slSearchCustomerNameBulky").prop("disabled", true);
            $("#slSearchStipCategoryBulky").prop("disabled", true);
            $('.btnSubmitTrx').prop('disabled', false);
            fsActLabel = row.Label;
            if (fsActLabel != "NEWBAPS_PRINT")
                $('.btnSubmitTrx').prop('disabled', true);
            if (row.CustomerID == 'XL' && row.Category == "NON-ADDITIONAL") {
                Form.XlBulkyForm('', row.CustomerID, row.CompanyID, '', row.ID);
                $("#BulkID").val($.trim(row.ID));
                $("#BapsNumber").val($.trim(row.ContractNumber));
                $("#Description").val($.trim(row.Description));
                $("#GrRecievedDate").val(Common.Format.ConvertJSONDateTime(row.GrRecievedDate));
                $("#PerformGR").val($.trim(row.PerformGR));
                $("#TicketNumber").val($.trim(row.TicketNumber));
                $("#slApprCompanyXLBulky").val(row.ApprIDCompany).trigger('change');
                $("#slApprCustomerXLBulky").val(row.ApprIDCustomer1).trigger('change');
                $("#slSearchStipCategoryBulky").val(row.Category).trigger('change');

            } else if (row.CustomerID == 'XL' && row.Category == "ADDITIONAL") {
                Form.XLAddForm(null, row.CustomerID, row.CompanyID, row.Category, '', row.ID);
                $('#IDTrxXLAdd').val(row.ID);
                $('#PriceLeaseXLAdd').val(Common.Format.CommaSeparation(row.PriceLease));
                $('#StartLeaseDateXLAdd').val(Common.Format.ConvertJSONDateTime(row.StartPeriodDate));
                $('#EndLeaseDateXLAdd').val(Common.Format.ConvertJSONDateTime(row.EndPeriodDate));
                $('#StartLeaseDateXLAdd2').val(Common.Format.ConvertJSONDateTime(row.PoDate));
                $('#BakDateXLAdd').val(Common.Format.ConvertJSONDateTime(row.ContractDate));
                $('#HeightSpaceXLAdd').val(row.HeightSpace);
                $('#CompanyInfoXLAdd').val(row.CompanyInfo);
                $("#slApprCustomerXLAdd1").val(row.ApprIDCustomer1).trigger('change');
                $("#slApprCustomerXLAdd2").val(row.ApprIDCustomer2).trigger('change');
                $("#slApprCompanyXLAdd").val(row.ApprIDCompany).trigger('change');
                $("#slSearchStipCategoryBulky").val(row.Category).trigger('change');
                $('#BulkNumberXLSA').val(row.BulkNumber);
            } else if (row.CustomerID == "ISAT") {
                $('#BulkyIDIsatBulky').val(row.ID);
                $('#JobDescIsatBulky').val(row.JobDesc);
                $('#ContractNumberIsatBulky').val(row.ContractNumber);
                $('#POnumberIsatBulky').val(row.PoNumber);
                $('#PoDateIsatBulky').val(Common.Format.ConvertJSONDateTime(row.PoDate));
                Form.ISATBulkyForm(null, row.CustomerID, row.CompanyID, row.Category, '', row.ID, '');
                $("#slApprCustomerIsatBulky").val(row.ApprIDCustomer1).trigger('change');
                $("#slApprCustomerIsatBulky2").val(row.ApprIDCustomer2).trigger('change');
                $("#slApprCompanyIsatBulky").val(row.ApprIDCompany).trigger('change');
                $("#slSearchStipCategoryBulky").val(row.Category).trigger('change');
                $("#slSearchStipCategoryBulky").prop("disabled", false);
            }

            else {
                $("#partialBulkyForm").fadeIn(1500);
            }
            $(".panelValidationForm").fadeIn(1800);
            $("#partialBulkyForm").fadeIn(2000);
            Control.ButtonResetBulky("0");
        });
        $("#tblValidationListBulkyPrint tbody").on("click", ".link-PrintPDFHeader", function (e) {
            var table = $("#tblValidationListBulkyPrint").DataTable();
            var row = table.row($(this).parents('tr')).data();
            Control.PrintPDF("", $.trim(row.CustomerID), $.trim(row.ID), 'header', $.trim(row.Category), "");
        });
        $("#tblValidationListBulkyPrint tbody").on("click", ".link-PrintPDFDetails", function (e) {
            var table = $("#tblValidationListBulkyPrint").DataTable();
            var row = table.row($(this).parents('tr')).data();
            Control.PrintPDF("", $.trim(row.CustomerID), $.trim(row.ID), 'details', $.trim(row.Category), "");
        });
        $("#tblValidationListBulkyPrint tbody").on("click", ".link-SubmitToCustBulky", function (e) {
            var table = $("#tblValidationListBulkyPrint").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $('#mdlSubmitToCustBulky').modal('show');
            $('#btnSubmitCustOKBulky').unbind().click(function () {
                Form.ApprovalSubmitBulky(row.ActivityID, "submit", row.CustomerID, row.Category, row.ID);
                $('#mdlSubmitToCustBulky').modal('hide');
            });
            $('#btnSubmitCustCancelBulky').unbind().click(function () {
                $('#mdlSubmitToCustBulky').modal('hide');
            });
        });
        $("#tblValidationListBulkyPrint tbody").on("click", ".link-ApprovalBulky", function (e) {
            var table = $("#tblValidationListBulkyPrint").DataTable();
            var row = table.row($(this).parents('tr')).data();
            Control.BingSelectNextActivity(row.ActivityID);
            $('#mdlConfirmNextProcessBulky').modal('show');
            $('#btnSubmitApprOkBulky').unbind().click(function () {
                Form.ApprovalSubmitBulky(row.ActivityID, $('#slStatusApprBulky').val(), row.CustomerID, row.Category, row.ID);
                $('#mdlConfirmNextProcessBulky').modal('hide');
            });
            $('#btnSubmitApprCancelBulky').unbind().click(function () {
                $('#mdlConfirmNextProcessBulky').modal('hide');
            });
        });
        $("#slSearchStipCategoryBulky").change(function () {
            Control.BindingSelectProductTypeBulky();
        });
    },

    GridSonumbXLBulky: function (CustomerID, CompanyID, SoNumber, SiteID) {
        Table.Init("#tblSonumbListXLBulky");
        var l = Ladda.create(document.querySelector("#btSearchBulky"));
        l.start();
        var params = {
            strCustomerId: CustomerID,
            strSoNumber: SoNumber,
            strSiteID: SiteID,
            strDataType: "Bulky",
            strCategory: $('#slSearchStipCategoryBulky').val() == null ? "" : $("#slSearchStipCategoryBulky").val(),
            strStipID: $('#slStipCategoryBulky').val() == null ? "" : $("#slStipCategoryBulky").val(),
            strProductId: $('#slSearchProductIDBulky').val() == null ? "" : $("#slSearchProductIDBulky").val()
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
                "url": "/api/StartBaps/getListSoNumber",
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
                        // strReturn += "<i class='fa fa-plus btn btn-xs green link-TrxDataValidationPrintBulky'  data-target='#mdlXLBulkySave' data-toggle='modal'></i>";
                        if (full.Label == "NEWBAPS_PRINT")
                            strReturn += "<i class='fa fa-plus btn btn-xs green link-TrxDataValidationPrintBulky' data-target='#mdlXLBulkySave' data-toggle='modal'></i>";
                        else
                            strReturn += "<i class='fa fa-plus btn btn-xs green' disabled='disabled'></i>";
                        return strReturn;
                    }
                },
                { data: "SoNumber" },
                { data: "ActivityName" },
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
                },
                { data: "Product" },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [4], "visible": false }, { "targets": [7], "visible": false }, { "targets": [8], "visible": false }, { "targets": [12], "visible": false }, { "targets": [1], "class": "text-center" }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblList.data())) {
                    $(".panelSearchResult").fadeIn(1000);
                }
                l.stop(); App.unblockUI('.panelSearchResult');
                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0; i < Data.RowSelected.length; i++) {
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
        Control.FormatCurrency('#Price');
    },

    GridDetailsXLBulky: function (bulkID) {
        Table.Init("#tblXLBulky");
        var buttonDelete = "";
        if (fsActLabel == "NEWBAPS_PRINT")
            buttonDelete = "<i class='fa fa-trash btn btn-xs red link-TrxDeleteListXLBulky' data-toggle='modal'></i>";
        else
            buttonDelete = "<i class='fa fa-trash btn btn-xs red' disabled='disabled'></i>";

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
                "url": "/api/StartBaps/getListDetailXLBulky",
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
                        strReturn += buttonDelete;

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
            { "targets": [15], "visible": false },
            { "targets": [16], "visible": false },
            { "targets": [17], "visible": false },
            { "targets": [18], "visible": false },
            { "targets": [19], "visible": false },
            { "targets": [20], "visible": false },
            { "targets": [21], "visible": false },
            { "targets": [22], "visible": false },
            { "targets": [23], "visible": false },
            { "targets": [1], "className": "text-center" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                //$('td').css('font-size', '12px');
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
            $("#Price").val(Common.Format.CommaSeparation(row.Price));
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
                Form.XlBulkyDetailDelete($.trim(row.ID), $.trim(bulkID));
            });
        });
    },

    GeridDetailHCPT: function (SoNumber, SIRO) {
        Table.Init('#tblHeightSpaceHCPT');
        var buttonDelete = "";
        if (fsActLabel == "NEWBAPS_PRINT")
            buttonDelete = "<i class='fa fa-trash btn btn-xs red link-tblHeightSpaceHcptDelete' data-toggle='modal'></i>";
        else
            buttonDelete = "<i class='fa fa-trash btn btn-xs red' disabled='disabled'></i>";

        var params = { strSoNumber: SoNumber, strStipSiro: SIRO };
        var tblList = $("#tblHeightSpaceHCPT").DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/StartBaps/getListHeightSpace",
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
                        strReturn += "<i class='fa fa-pencil btn btn-xs green link-tblHeightSpaceHcptEdit' data-toggle='modal'></i> &nbsp;";
                        strReturn += buttonDelete;
                        return strReturn;
                    }
                },
                { data: "Model" },
                { data: "MaterialType" },
                { data: "Quantity" },
                { data: "Manufacture" },
                { data: "Height" },
                { data: "Dimension" },
                { data: "Azim" },
                { data: "CableQuantity" },
                { data: "CableSize" },
                //{ data: "Value" },
                { data: "ID" }

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [11], "visible": false }, { "targets": [1], "class": "text-center" }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                //$('td').css('font-size', '12px');
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
        $("#tblHeightSpaceHCPT tbody").unbind();
        $("#tblHeightSpaceHCPT tbody").on("click", ".link-tblHeightSpaceHcptDelete", function (e) {
            var table = $("#tblHeightSpaceHCPT").DataTable();
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
                Form.HCPTDetailDelete(SoNumber, SIRO, row.ID)
            });
        });
        $("#tblHeightSpaceHCPT tbody").on("click", ".link-tblHeightSpaceHcptEdit", function (e) {
            var table = $("#tblHeightSpaceHCPT").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $("#IDTrxDtlHCPT").val($.trim(row.ID));
            $("#StipSiroHCPT").val($.trim(SIRO));
            $("#SoNumberHCPT").val($.trim(SoNumber));
            $("#TypeHCPT").val($.trim(row.MaterialType));
            $("#QTYHCPT").val($.trim(row.Quantity));
            $("#DimensionHCPT").val($.trim(row.Dimension));
            $("#ModelHCPT").val($.trim(row.Model));
            $("#HeightHCPT").val($.trim(row.Height));
            $("#AzimHCPT").val($.trim(row.Azim));
            $("#CableQtyHCPT").val($.trim(row.CableQuantity));
            $("#CableSizeHCPT").val($.trim(row.CableSize));
            $("#ValueHCPT").val($.trim(row.Value));
            $("#ManufactureHCPT").val($.trim(row.Manufacture));
            $('#mdlDetailHCPT').modal('show');
        });
        $('#btnDtlHCPTSave').unbind().click(function () {
            Form.HCPTDetailSave(SoNumber, SIRO);
        });

    },

    GridEquipment: function (SoNumber) {
        var buttonDelete = "";
        if (fsActLabel == "NEWBAPS_PRINT")
            buttonDelete = "<i class='fa fa-trash btn btn-xs red link-TrxDeletetListEquipmentSfAdd' data-toggle='modal'></i>";
        else
            buttonDelete = "<i class='fa fa-trash btn btn-xs red' disabled='disabled'></i>";

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
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [7], "visible": false }, { "targets": [1], "className": "text-center" }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                //  //$('td').css('font-size', '12px');
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

    GridSonumbXLAdd: function (SoNumber, CustomerID, SiteID, ProductID) {
        var idTb = "#tblSonumbAddListXLAdd";
        Table.Init(idTb);
        var l = Ladda.create(document.querySelector("#btSearchBulky"));
        l.start();
        var params = {
            strCustomerId: CustomerID,
            strSoNumber: SoNumber,
            strSiteID: SiteID,
            strDataType: "Bulky",
            strCategory: $('#slSearchStipCategoryBulky').val() == null ? "" : $("#slSearchStipCategoryBulky").val(),
            strStipID: $('#slStipCategoryBulky').val() == null ? "" : $("#slStipCategoryBulky").val(),
            strProductId: $('#slSearchProductIDBulky').val() == null ? "" : $("#slSearchProductIDBulky").val(),
        };
        var tblList = $(idTb).DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/StartBaps/getListSoNumber",
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
                        //strReturn += "<i class='fa fa-plus btn btn-xs green link-TrxSombListXLAdd'></i>";
                        if (full.Label == "NEWBAPS_PRINT")
                            strReturn += "<i class='fa fa-plus btn btn-xs green link-TrxSombListXLAdd'></i>";
                        else
                            strReturn += "<i class='fa fa-plus btn btn-xs green' disabled='disabled'></i>";
                        return strReturn;
                    }
                },
                { data: "SoNumber" },
                { data: "ActivityName" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "Product" },
                { data: "SIRO" },
                { data: "StipCode" },
                { data: "RegionName" }
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [1], "className": "text-center" }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblList.data())) {
                    $(".panelSearchResult").fadeIn(1000);
                }
                l.stop(); App.unblockUI('.panelSearchResult');
                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0; i < Data.RowSelected.length; i++) {
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
        $(idTb + " tbody").unbind();
        $(idTb + " tbody").on("click", ".link-TrxSombListXLAdd", function (e) {
            var table = $(idTb).DataTable();
            var row = table.row($(this).parents('tr')).data();
            $("#StipSiroXLAdd").val(row.SIRO);
            $("#SoNumberXLAdd").val(row.SoNumber);
            $("#StipCategoryXLAdd").val(row.StipCode);
            $('#SiteIDSfAdd').val(row.SiteID);
            Form.ClearFormXlAddDetail();

            if ($.trim(row.Product).toUpperCase() == "ADDITIONAL LAND") {
                $("#mdlXLAddSave").modal('show');
                $("#XlAddLand").show();
                Control.FormatCurrency('#FoundationPrice');
            }
            else if ($.trim(row.Product).toUpperCase() == "ADDITIONAL RRU") {
                $("#mdlXLAddSave").modal('show');
                $('#JenisAntennaRRUXLAdd').val(row.Product);
                $("#XlAddAntennaRRU").show();
                Control.FormatCurrency('#RRUPrice');
            }
            else if ($.trim(row.Product).toUpperCase() == "ADDITIONAL ANTENNA RF") {
                $("#mdlXLAddSave").modal('show');
                $('#JenisAntennaRFXLAdd').val(row.Product);
                $("#XlAddAntennaRF").show();
                Control.FormatCurrency('#RFPrice');
            }
            else if ($.trim(row.Product).toUpperCase() == "ADDITIONAL ANTENNA MW") {
                $("#mdlXLAddSave").modal('show');
                $('#JenisAntennaMWXLAdd').val(row.Product);
                $("#XlAddAntennaMW").show();
                Control.FormatCurrency('#MWPrice');
            }
            else {
                $("#mdlXLAddSave").modal('hide');
                alert("Template Not Found !");
            }
            $("#btnXLAddDetailSave").unbind().click(function () {
                Form.XlAddDetailSave(SoNumber, 'XL', SiteID, ProductID);
            });
            $("#btnXLAddDetailCancel").unbind().click(function () {

            });

        });
    },

    GridDetailsXLAdd: function (BulkID, SoNumber, CustomerID, SiteID) {
        var buttonDelete = "";
        if (fsActLabel == "NEWBAPS_PRINT")
            buttonDelete = "<i class='fa fa-trash btn btn-xs red link-TrxDeleteListXLAdd' data-toggle='modal'></i>";
        else
            buttonDelete = "<i class='fa fa-trash btn btn-xs red' disabled='disabled'></i>";
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
                "url": "/api/StartBaps/getListDetailXlAdditional",
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
                        strReturn += buttonDelete;
                        return strReturn;
                    }
                },
                { data: "SoNumberAdd" },
                { data: "Product" },
                { data: "AntennaCount" },
                { data: "Height" },
                { data: "AntennaDiameter" },
                { data: "SurfaceArea" },
                { data: "SurfaceAreaModern" },
                { data: "Foundation" },
                { data: "FoundationByContract" },
                { data: "AvailableSpace" },
                {
                    data: "Price", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                { data: "AddPrice" },
                { data: "ID" },
                { data: "BulkyID" }

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [13], "visible": true }, { "targets": [14], "visible": false }, { "targets": [15], "visible": false }, { "targets": [12], "className": "text-right" }, { "targets": [1], "className": "text-center" }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                //$('td').css('font-size', '12px');
                $('.link-TrxEditListXLAdd').prop('disabled', true);
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
        $("#tblDetailXLAdd tbody").unbind();
        $("#tblDetailXLAdd tbody").on("click", ".link-TrxEditListXLAdd", function (e) {
            var table = $("#tblDetailXLAdd").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $('#IDTrxDetailXLAdd').val(row.ID);
            Form.ClearFormXlAddDetail();
            if ($.trim(row.Product).toUpperCase() == "ADDITIONAL LAND") {
                $('#FoundationXlAdd').val(row.Foundation);
                $('#FoundationByContractXLAdd').val(row.FoundationByContract);
                $('#AvailableSpaceXLAdd').val(row.AvailableSpace);
                $('#FoundationPrice').val(Common.Format.CommaSeparation(Data.Price));
                $("#XlAddLand").show();
                Control.FormatCurrency('#FoundationPrice');
            }

            else if ($.trim(row.Product).toUpperCase() == "ADDITIONAL RRU") {
                $('#JenisAntennaRRUXLAdd').val(row.Product);
                $('#JumlahAntennaRRUXlAdd').val(row.AntennaCount);
                $('#LuasPermukaanRRUXlAdd').val(row.SurfaceArea);
                $('#LuasPermukaanModernRRUXLAdd').val(row.SurfaceAreaModern);
                $('#KetinggianRRUXLAdd').val(row.Height);
                $('#RRUPrice').val(Common.Format.CommaSeparation(row.Price));
                $('#AdditionalPrice').val(row.AddPrice);
                $("#XlAddAntennaRRU").show();
                Control.FormatCurrency('#RRUPrice');
            }

            else if ($.trim(row.Product).toUpperCase() == "ADDITIONAL ANTENNA RF") {
                $('#JenisAntennaRFXLAdd').val(row.Product);
                $('#JumlahAntennaRFXlAdd').val(row.AntennaCount);
                $('#KetinggianRFXLAdd').val(row.Height);
                $('#RFPrice').val(Common.Format.CommaSeparation(row.Price));
                $("#XlAddAntennaRF").show();
                Control.FormatCurrency('#RFPrice');
            }

            else if ($.trim(row.Product).toUpperCase() == "ADDITIONAL ANTENNA MW") {
                $('#JenisAntennaMWXLAdd').val(row.Product);
                $('#JumlahAntennaMWXlAdd').val(row.AntennaCount);
                $('#DiameterMWXLAdd').val(row.AntennaDiameter);
                $('#KetinggianMWXLAdd').val(row.Height);
                $('#MWPrice').val(Common.Format.CommaSeparation(row.Price));
                $("#XlAddAntennaMW").show();
                Control.FormatCurrency('#MWPrice');
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

    GridSonumbIsatBulky: function (SoNumber, CustomerID, SiteID, ProductID) {

        Table.Init("#tblSonumbListIsatBulky");
        var params = {
            strCustomerId: CustomerID,
            strSoNumber: SoNumber,
            strSiteID: SiteID,
            strDataType: "Bulky",
            strCategory: $('#slSearchStipCategoryBulky').val() == null ? "" : $("#slSearchStipCategoryBulky").val(),
            strStipID: $('#slStipCategoryBulky').val() == null ? "" : $("#slStipCategoryBulky").val(),
            strProductId: $('#slSearchProductIDBulky').val() == null ? "" : $("#slSearchProductIDBulky").val(),
        };

        var tblList = $("#tblSonumbListIsatBulky").DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/StartBaps/getListSoNumber",
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
                        // strReturn += "<i class='fa fa-plus btn btn-xs green link-TrxSombListIsatBulky'></i>";
                        if (full.Label == "NEWBAPS_PRINT")
                            strReturn += "<i class='fa fa-plus btn btn-xs green link-TrxSombListIsatBulky'></i>";
                        else
                            strReturn += "<i class='fa fa-plus btn btn-xs green' disabled='disabled'></i>";
                        return strReturn;
                    }
                },
                { data: "SoNumber" },
                { data: "ActivityName" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "Product" },
                { data: "CompanyName" },
                { data: "SIRO" },
                { data: "StipCode" },
                {
                    data: "MLADate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "CustomerID" },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [6], "visible": false }, { "targets": [7], "visible": false }, { "targets": [12], "visible": false }, { "targets": [13], "visible": false }, { "targets": [1], "class": "text-center" }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                //$('td').css('font-size', '12px');
            },
            "order": [1, "asc"],
            "scrollY": 300,
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": { leftColumns: 2 }, /* Set the 2 most left columns as fixed columns */
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
        });
        $("#tblSonumbListIsatBulky tbody").unbind();
        $("#tblSonumbListIsatBulky tbody").on("click", ".link-TrxSombListIsatBulky", function (e) {
            var table = $("#tblSonumbListIsatBulky").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $('#StipSiroIsatBulky').val(row.SIRO);
            $('#SoNumberIsatBulky').val(row.SoNumber);
            $('#SiteIDCustIsatBulky').val(row.CustomerSiteID);
            $('#SiteNameCustIsatBulky').val(row.CustomerSiteName);
            $('#mdlisatBulkySave').modal('show');
            Control.FormatCurrency('#UnitNetPriceIsatBulky');
            Control.FormatCurrency('#UNPspkIsatBulky');
            Control.FormatCurrency('#UNPbeforeIsatBulky');
            Control.FormatCurrency('#UNPcurIsatBulky');
            Control.FormatCurrency('#UNPremainingIsatBulky');
        });
    },

    GridDetailsIsatBulky: function (BulkID, SoNumber, CustomerID, SiteID) {
        var buttonDelete = "";
        if (fsActLabel == "NEWBAPS_PRINT")
            buttonDelete = "<i class='fa fa-trash btn btn-xs red  link-TrxDeleteListIsatBulky' data-toggle='modal'></i>";
        else
            buttonDelete = "<i class='fa fa-trash btn btn-xs red' disabled='disabled'></i>";

        Table.Init('#tblDetailiSatBulky');
        var params = { strSoNumber: SoNumber, strBulkID: BulkID };
        var tblList = $("#tblDetailiSatBulky").DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/StartBaps/getListBapsMaterials",
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
                        strReturn += "<i class='fa fa-pencil btn btn-xs green  link-TrxEditListIsatBulky' data-toggle='modal'></i> &nbsp;";
                        strReturn += buttonDelete;
                        return strReturn;
                    }
                },
                { data: "SoNumber" },
                { data: "BaufNumber" },
                { data: "JobDesc" },
                {
                    data: "RfsPoDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "CheckedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "Material" },
                { data: "UnitNetPrice" },
                { data: "SpkQty" },
                { data: "SpkUNP" },
                { data: "RealisationBeforeQty" },
                { data: "RealisationBeforeUNP" },
                { data: "RealisationCurrentQty" },
                { data: "RealisationCurrentUNP" },
                { data: "TheRestQty" },
                { data: "TheRestUNP" },
                { data: "Result" },
                { data: "Description" },
                { data: "PhysicalProgress" },
                { data: "Discount" },
                { data: "ID" },
                { data: "BulkyID" },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [21], "visible": false }, { "targets": [22], "visible": false }, { "targets": [1], "class": "text-center" }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                //$('td').css('font-size', '12px');
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
        $("#tblDetailiSatBulky tbody").unbind();
        $("#tblDetailiSatBulky tbody").on("click", ".link-TrxEditListIsatBulky", function (e) {
            var table = $("#tblDetailiSatBulky").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $('#IDTrxIsatBulky').val(row.ID);
            $('#SoNumberIsatBulky').val(row.SoNumber);
            $('#JobDescDetailIsatBulky').val(row.JobDesc);
            $('#MaterialsIsatBulky').val(row.Material);
            $('#UnitNetPriceIsatBulky').val(Common.Format.CommaSeparation(row.UnitNetPrice));
            $('#QTYspkIsatBulky').val(row.SpkQty);
            $('#UNPspkIsatBulky').val(Common.Format.CommaSeparation(row.SpkUNP));
            $('#QTYbeforeIsatBulky').val(row.RealisationBeforeQty);
            $('#UNPbeforeIsatBulky').val(Common.Format.CommaSeparation(row.RealisationBeforeUNP));
            $('#QTYcurIsatBulky').val(row.RealisationCurrentQty);
            $('#UNPcurIsatBulky').val(Common.Format.CommaSeparation(row.RealisationCurrentUNP));
            $('#QTYremainingIsatBulky').val(row.TheRestQty);
            $('#UNPremainingIsatBulky').val(Common.Format.CommaSeparation(row.TheRestUNP));
            $('#ResultIsatBulky').val(row.Result);
            $('#RemarksIsatBulky').val(row.Description);
            $('#ProgressIsatBulky').val(row.PhysicalProgress);
            $('#DiscountIsatBulky').val(row.Discount);
            $('#RFSPOdateIsatBulky').val(Common.Format.ConvertJSONDateTime(row.RfsPoDate));
            $('#SiteNameCustIsatBulky').val(row.SiteNameOpr);
            $('#SiteIDCustIsatBulky').val(row.SiteIdOpr);
            $('#BaufNumberIsatBulky').val(row.BaufNumber);
            $('#CheckedDateIsatBulky').val(Common.Format.ConvertJSONDateTime(row.CheckedDate));
            $('#StipSiroIsatBulky').val(row.SIRO);
            $('#mdlisatBulkySave').modal('show');
            Control.FormatCurrency('#UnitNetPriceIsatBulky');
            Control.FormatCurrency('#UNPspkIsatBulky');
            Control.FormatCurrency('#UNPbeforeIsatBulky');
            Control.FormatCurrency('#UNPcurIsatBulky');
            Control.FormatCurrency('#UNPremainingIsatBulky');
        });

        $("#tblDetailiSatBulky tbody").on("click", ".link-TrxDeleteListIsatBulky", function (e) {
            var table = $("#tblDetailiSatBulky").DataTable();
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
                Form.ISATBulkyDetailDelete($.trim(row.ID), $.trim(row.BulkyID), $.trim(row.SoNumber), CustomerID, SiteID);
            });
        });
    },

    GeridDetailSTI: function (SoNumber, SIRO) {
        var buttonDelete = "";
        if (fsActLabel == "NEWBAPS_PRINT")
            buttonDelete = "<i class='fa fa-trash btn btn-xs red link-tblHeightSpaceSTIDelete' data-toggle='modal'></i>";
        else
            buttonDelete = "<i class='fa fa-trash btn btn-xs red' disabled='disabled'></i>";
        var params = { strSoNumber: SoNumber, strStipSiro: SIRO };
        var tblList = $("#tblHeightSpaceSTI").DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/StartBaps/getListHeightSpace",
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
                        strReturn += "<i class='fa fa-pencil btn btn-xs green link-tblHeightSpaceSTIEdit' data-toggle='modal'></i> &nbsp;";
                        strReturn += buttonDelete;
                        return strReturn;
                    }
                },
                { data: "MaterialType" },
                { data: "Quantity" },
                { data: "Manufacture" },
                { data: "Model" },
                { data: "Height" },
                { data: "Dimension" },
                { data: "Azim" },
                { data: "CableQuantity" },
                { data: "CableSize" },
                { data: "Value" },
                { data: "ID" }

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [12], "visible": false }, { "targets": [1], "class": "text-center" }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblList.data())) {
                    $(".panelSearchResult").fadeIn(1000);
                }
                App.unblockUI('.panelSearchResult');
                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0; i < Data.RowSelected.length; i++) {
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
        $("#tblHeightSpaceSTI tbody").unbind();
        $("#tblHeightSpaceSTI tbody").on("click", ".link-tblHeightSpaceSTIDelete", function (e) {
            var table = $("#tblHeightSpaceSTI").DataTable();
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
                Form.STIDetailDelete(SoNumber, SIRO, row.ID)
            });
        });
        $("#tblHeightSpaceSTI tbody").on("click", ".link-tblHeightSpaceSTIEdit", function (e) {
            var table = $("#tblHeightSpaceSTI").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $("#IDTrxDtlSTI").val($.trim(row.ID));
            $("#StipSiroSTI").val($.trim(SIRO));
            $("#SoNumberSTI").val($.trim(SoNumber));
            $("#TypeSTI").val($.trim(row.MaterialType));
            $("#QTYSTI").val($.trim(row.Quantity));
            $("#DimensionSTI").val($.trim(row.Dimension));
            $("#ModelSTI").val($.trim(row.Model));
            $("#HeightSTI").val($.trim(row.Height));
            $("#AzimSTI").val($.trim(row.Azim));
            $("#CableQtySTI").val($.trim(row.CableQuantity));
            $("#CableSizeSTI").val($.trim(row.CableSize));
            $("#ValueSTI").val($.trim(row.Value));
            $("#ManufactureSTI").val($.trim(row.Manufacture));
            $('#mdlDetailSTI').modal('show');
        });
        $('#btnDtlSTISave').unbind().click(function () {
            Form.STIDetailSave(SoNumber, SIRO);
        });
    },

    GeridDetailINUX: function (SoNumber, SIRO) {
        var buttonDelete = "";
        if (fsActLabel == "NEWBAPS_PRINT")
            buttonDelete = "<i class='fa fa-trash btn btn-xs red link-tblHeightSpaceINUXDelete' data-toggle='modal'></i>";
        else
            buttonDelete = "<i class='fa fa-trash btn btn-xs red' disabled='disabled' data-toggle='modal'></i>";

        var params = { strSoNumber: SoNumber, strINUXpSiro: SIRO };
        var tblList = $("#tblHeightSpaceINUX").DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/StartBaps/getListHeightSpace",
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
                        strReturn += "<i class='fa fa-pencil btn btn-xs green link-tblHeightSpaceINUXEdit' data-toggle='modal'></i> &nbsp;";
                        strReturn += buttonDelete;
                        return strReturn;
                    }
                },
                { data: "MaterialType" },
                { data: "Quantity" },
                { data: "Manufacture" },
                { data: "Model" },
                { data: "Height" },
                { data: "Dimension" },
                { data: "Azim" },
                { data: "CableQuantity" },
                { data: "CableSize" },
                { data: "Value" },
                { data: "ID" }

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [12], "visible": false }, { "targets": [1], "class": "text-center" }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                //$('td').css('font-size', '12px');
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
        $("#tblHeightSpaceINUX tbody").unbind();
        $("#tblHeightSpaceINUX tbody").on("click", ".link-tblHeightSpaceINUXDelete", function (e) {
            var table = $("#tblHeightSpaceINUX").DataTable();
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
                Form.INUXDetailDelete(SoNumber, SIRO, row.ID)
            });
        });
        $("#tblHeightSpaceINUX tbody").on("click", ".link-tblHeightSpaceINUXEdit", function (e) {
            var table = $("#tblHeightSpaceINUX").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $("#IDTrxDtlINUX").val($.trim(row.ID));
            $("#INUXpSiroINUX").val($.trim(SIRO));
            $("#SoNumberINUX").val($.trim(SoNumber));
            $("#TypeINUX").val($.trim(row.MaterialType));
            $("#QTYINUX").val($.trim(row.Quantity));
            $("#DimensionINUX").val($.trim(row.Dimension));
            $("#ModelINUX").val($.trim(row.Model));
            $("#HeightINUX").val($.trim(row.Height));
            $("#AzimINUX").val($.trim(row.Azim));
            $("#CableQtyINUX").val($.trim(row.CableQuantity));
            $("#CableSizeINUX").val($.trim(row.CableSize));
            $("#ValueINUX").val($.trim(row.Value));
            $("#ManufactureINUX").val($.trim(row.Manufacture));
            $('#mdlDetailINUX').modal('show');
        });
        $('#btnDtlINUXSave').unbind().click(function () {
            Form.INUXDetailSave(SoNumber, SIRO);
        });
    },

    GridViewBaukDocument: function () {
        var idTBL = "#tblBaukDocument";
        Table.Init(idTBL);
        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();
        var params = {
            strCustomerId: $("#slSearchCustomerID").val() == null ? "" : $("#slSearchCustomerID").val(),
            strCompanyId: $("#slSearchCompanyID").val() == null ? "" : $("#slSearchCompanyID").val(),
            strProductId: $("#slSearchProductID").val() == null ? "" : $("#slSearchProductID").val(),
            strSoNumber: $("#slSearchSoNumber").val() == null ? "" : $("#slSearchSoNumber").val(),
            strSiteID: $("#slSearchSiteID").val() == null ? "" : $("#slSearchSiteID").val(),
            strStipID: $("#slStipCategory").val() == null ? "" : $("#slStipCategory").val(),
            strDataType: "NonBulky"
        };
        var tblList = $(idTBL).DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/StartBaps/getListSoNumber",
                "type": "POST",
                "data": params,
                "cache": false,
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

                        return "<i class='fa fa-eye btn btn-xs blue viewDoc' title='View Doc'></i>";
                    }
                },
                { data: "SoNumber" },
                { data: "ActivityName" },
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
                { data: "RegionName" },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [1, 10, 13, 14], "class": "text-center" }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblList.data())) {
                    $(".panelSearchResult").fadeIn(1000);
                }
                l.stop(); App.unblockUI('.panelSearchResult');
                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0; i < Data.RowSelected.length; i++) {
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
        $(idTBL + " tbody").unbind();
        $(idTBL + " tbody").on("click", ".viewDoc", function (e) {
            var table = $(idTBL).DataTable();
            var row = table.row($(this).parents('tr')).data();
            Form.SetHeaderInfo(row.SiteID, row.SoNumber, row.CustomerID, row.CompanyID, row.CompanyName, row.CustomerSiteID, row.CustomerSiteName)
            Control.BindBAUKDocument(row.SoNumber, row.SiteID, row.CustomerID);
            Control.BindingDocumentSupport(row.CompanyID, row.SiteID);
            $(".panelTab").fadeOut();
            $(".filter").fadeOut();
            $("#panelHeaderInfo").fadeIn(1800);
            $(".panelValidationForm").fadeIn(1800);
            $("#partialBAUKDocument").fadeIn(1800);

        });

    },

}

var DataAccess = {

    SubmitBAPS: function () {
        var result = "";
        Data.ID = vIDBaps;
        Data.SoNumber = vSoNumber;
        Data.StipSiro = vStipSiro;
        Data.SiteID = vSiteID;
        Data.SpkNumber = vSpkNumber;
        Data.CustomerID = vCustomerID;
        Data.StipCategory = vStipCategory;
        Data.ProductID = vProductID;
        Data.SpkDate = vSpkDate;
        Data.SdaDate = vSpkDate;
        Data.AtpDate = vAtpDate;
        Data.StartPeriodDate = vStartPeriodDate;
        Data.EndPeriodDate = vEndPeriodDate;
        Data.StartAddLeaseDate = vStartAddLeaseDate;
        Data.StartAddLeaseDate2 = vStartAddLeaseDate2;
        Data.EndAddLeaseDate = vEndAddLeaseDate;
        Data.EndAddLeaseDate2 = vEndAddLeaseDate2;
        Data.PriceLease = vPriceLease;
        Data.AddPriceLease = vAddPriceLease;
        Data.AddPriceLease2 = vAddPriceLease2;
        Data.ProposalNumber = vProposalNumber;
        Data.ProposalDate = vProposalDate;
        Data.IssuingDate = vIssuingDate;
        Data.BapsNumber = vBapsNumber;
        Data.ContractNumber = vContractNumber;
        Data.PoNumber = vPoNumber;
        Data.PoDate = vPoDate;
        Data.SurfaceArea1 = vSurfaceArea1;
        Data.SurfaceArea2 = vSurfaceArea2;
        Data.EffectiveDate = vEffectiveDate;
        Data.PricePerMonth = vPricePerMonth;
        Data.Frequency = vFrequency;
        Data.NumberOfAntenna = vNumberOfAntenna;
        Data.ProjectDef = vProjectDef;
        Data.BuildingHeight = vBuildingHeight;
        Data.LandSize = vLandSize;
        Data.PowerSize = vPowerSize;
        Data.OtherPrice = vOtherPrice;
        Data.OpexPrice = vOpexPrice;
        Data.InstallPermitDate = vInstallPermitDate;
        Data.BaufDate = vBaufDate;
        Data.BapsDate = vBapsDate;
        Data.MlaDate = vMlaDate;
        Data.ContractDate = vContractDate;
        Data.Noted = vNoted;
        Data.ApprIDCompany = fsApprIDCompany;
        Data.ApprIDCustomer1 = fsApprIDCustomer1;
        Data.ApprIDCustomer2 = fsApprIDCustomer2;
        Data.ApprIDCustomer3 = fsApprIDCustomer3;
        Data.LandStatus = fsLandStatus;
        Data.SiteType = vSiteType;
        Data.TowerType = fsTowerType;
        Data.ServiceType = vServiceType;
        Data.InitialTowerType = vTowerType;

        var params = { trxRABapsValidation: Data };
        var action = "submitBapsPrint";
        if (vCustomerID == "ISAT") {
            action = "submitIsat";
            params = { trxRABapsValidation: Data, trxRABapsMaterials: dataMaterial };
        }
        $.ajax({
            url: "/api/StartBaps/" + action,
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    Table.GridValidationPrint();
                    Control.ButtonCancelForm();
                    Common.Alert.Success("Data Success Saved!");
                    result = "";
                } else {
                    Common.Alert.Warning(data.ErrorMessage);
                    result = data.ErrorMessage;
                }
            }

        }).fail(function (jqXHR, textStatus, errorThrown) {
            result = errorThrown;
        });

        return result;
    },

    GetBAPSData: function (SoNumber, SiteID, StipSiro) {
        var result = "";
        Data.SoNumber = SoNumber;
        Data.SiteID = SiteID;
        Data.StipSiro = StipSiro;
        var params = { trxRABapsValidation: Data };

        $.ajax({
            url: "/api/StartBaps/getBapsPrint",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            async: false
        }).done(function (data, textStatus, jqXHR) {
            if (data != null) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    vIDBaps = data.ID;
                    vSoNumber = data.SoNumber;
                    vStipSiro = data.StipSiro;
                    vSiteID = data.SiteID;
                    vSpkNumber = data.SpkNumber;
                    vCustomerID = data.CustomerID;
                    vStipCategory = data.StipCategory;
                    vProductID = data.ProductID;
                    vSpkDate = data.SpkDate;
                    vSdaDate = data.SdaDate;
                    vAtpDate = data.AtpDate;
                    vStartPeriodDate = data.StartPeriodDate;
                    vEndPeriodDate = data.EndPeriodDate;
                    vStartAddLeaseDate = data.StartAddLeaseDate;
                    vStartAddLeaseDate2 = data.StartAddLeaseDate2;
                    vEndAddLeaseDate = data.EndAddLeaseDate;
                    vEndAddLeaseDate2 = data.EndAddLeaseDate2;
                    vPriceLease = data.PriceLease;
                    vAddPriceLease = data.AddPriceLease;
                    vAddPriceLease2 = data.AddPriceLease2;
                    vProposalNumber = data.ProposalNumber;
                    vProposalDate = data.ProposalDate;
                    vIssuingDate = data.IssuingDate;
                    vBapsNumber = data.BapsNumber;
                    vContractNumber = data.ContractNumber;
                    vPoNumber = data.PoNumber;
                    vPoDate = data.PoDate;
                    vSurfaceArea1 = data.SurfaceArea1;
                    vSurfaceArea2 = data.SurfaceArea2;
                    vEffectiveDate = data.EffectiveDate;
                    vPricePerMonth = data.PricePerMonth;
                    vFrequency = data.Frequency;
                    vNumberOfAntenna = data.NumberOfAntenna;
                    vProjectDef = data.ProjectDef;
                    vBuildingHeight = data.BuildingHeight;
                    vLandSize = data.LandSize;
                    vPowerSize = data.PowerSize;
                    vOtherPrice = data.OtherPrice;
                    vOpexPrice = data.OpexPrice;
                    vInstallPermitDate = data.InstallPermitDate;
                    vBaufDate = data.BaufDate;
                    vBapsDate = data.BapsDate;
                    vMlaDate = data.MlaDate;
                    vContractDate = data.ContractDate;
                    vNoted = data.Noted;
                    fsApprIDCompany = data.ApprIDCompany;
                    fsApprIDCustomer1 = data.ApprIDCustomer1;
                    fsApprIDCustomer2 = data.ApprIDCustomer2;
                    fsApprIDCustomer3 = data.ApprIDCustomer3;
                } else {
                    Common.Alert.Warning(data.ErrorMessage);
                }
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
    },

    SaveHeightSpace: function (SoNumber, StipSiro) {
        var result = "";
        Data.ID = vIDHeightSpace;
        Data.SoNumber = SoNumber;
        Data.StipSiro = StipSiro;
        Data.MaterialType = vMaterialType;
        Data.Quantity = vQuantity;
        Data.Height = vHeight;
        Data.Dimension = vDimension;
        Data.Model = vModel;
        Data.Azim = vAzim;
        Data.Value = vValue;
        Data.CableSize = vCableSize;
        Data.CableQuantity = vCableQuantity;
        Data.Manufacture = vManufacture;

        var params = { trxRABapsHeightSpace: Data };
        $.ajax({
            url: "/api/StartBaps/saveHeightSpace",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            async: false
        }).done(function (data, textStatus, jqXHR) {
            if (data != null) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    Common.Alert.Success("Data Success Saved!");
                    result = "";
                } else {
                    Common.Alert.Warning(data.ErrorMessage);
                    result = data.ErrorMessage;
                }
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            result = errorThrown;
        });

        return result;
    },

    DeleteHeightSpace: function (ID) {
        var result = "";

        var params = { strIDTrx: ID };
        $.ajax({
            url: "/api/StartBaps/deleteHeightSpace",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (data == true) {
                result = "";
            } else {
                result = false;
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });

        return result;
    },

    SubmitBAPSBulky: function () {
        Data.ID = vIDBapsBulky;
        Data.CompanyID = vCompanyID;
        Data.CustomerID = vCustomerID;
        Data.JobDesc = vJobDesc;
        Data.ContractNumber = vContractNumber;
        Data.ContractDate = vContractDate;
        Data.Description = vDescription;
        Data.GrRecievedDate = vGrRecievedDate;
        Data.PerformGR = vPerformGR;
        Data.TicketNumber = vTicketNumber;
        Data.Category = vCategory;
        Data.PriceLease = vPriceLease;
        Data.StartPeriodDate = vStartPeriodDate;
        Data.EndPeriodDate = vEndPeriodDate;
        Data.CompanyInfo = vCompanyInfo;
        Data.HeightSpace = vHeightSpace;
        Data.ActivityID = vActivityID;
        Data.Noted = vNoted;
        Data.PoDate = vPoDate;
        Data.PoNumber = vPoNumber;
        Data.ApprIDCompany = fsApprIDCompany;
        Data.ApprIDCustomer1 = fsApprIDCustomer1;
        Data.ApprIDCustomer2 = fsApprIDCustomer2;
        Data.ApprIDCustomer3 = fsApprIDCustomer3;
        var params = {
            trxRABapsBulkyValidation: Data
        };
        $.ajax({
            url: "/api/StartBaps/submitBapsPrintBulky",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    vBulkyNumber = data.BulkNumber == null ? vBulkyNumber : data.BulkNumber;
                    Common.Alert.Success("Data " + data.BulkNumber + " Success Submited! ");
                    Table.GriValidationBulkyPrint();
                    Control.ButtonCancelForm();
                } else {
                    Common.Alert.Warning(data.ErrorMessage);
                }
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
    },

    SkipPrintBAPS: function (SoNumber, SIRO) {
        if (!confirm("Skip BAPS Print?"))
            return;

        let Data = {
            strSoNumber: SoNumber,
            strStipSiro: SIRO
        };
        $.ajax({
            url: "/api/StartBaps/skipBAPSPrint",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(Data),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    Common.Alert.Success("Skip BAPS Print for SO Number " + data.SoNumber + " is succeeded! ");
                    Table.GridValidationPrint();
                    Control.ButtonCancelForm();
                } else {
                    Common.Alert.Warning(data.ErrorMessage);
                }
            } else if (data.GenericError != undefined) {
                Common.Alert.Error(data.GenericError)
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
    },
}