Data = {};
TempData = [];
var AllDataId = [];

TempChkDataNonTsel = [];
var AllDataNonTselId = [];

TempChkDataBaps = [];
var AllDataNewBapsId = [];

TempChkDataProd = [];
var AllDataProdId = [];

var AllDataHsIds = [];

//filter Search 
var fsCompanyInvoice = "";
var fsCustomerInvoice = "";
var fsSection = "";
var fsSOW = "";
var fsBillingYear = "";
var fsRegional = "";
var fsProvince = "";
var fsTenantType = "";
var strUserToken = "";
var schBapsType = "";
var schPowerType = "";
var schReconcileType = "";
var fsReconcileType = "";
var fsPowerType = "";

var chckReconcile = "";
//Summary

jQuery(document).ready(function () {
    //set active tab while page loaded
    $("#tabDasboard ul li:first").addClass('active');
    Form.Init();
    Data.RowSelected = [];
    Data.RowSelectedConfirm = [];
    Table.Init();
    TableNonTsel.Init();
    TableNewBaps.Init();
    TableNewProduct.Init();
    TableHistory.Init();
    TempData = [];
    TempChkDataNonTsel = [];
    TempChkDataBaps = [];
    TempChkDataProd = [];

    CheckAllDataFlag = false;
    $('#formSearchOne').parsley();
    $('#formSearchHS').parsley();


    //search
    $("#formSearch").submit(function (e) {
        Table.Search();
        e.preventDefault();
    });

    $("#formSearchTwo").submit(function (e) {
        TableHistory.SearchHistory();
        e.preventDefault();
    });

    $("#formHistory").submit(function (e) {
        Table.Search();
        e.preventDefault();
    });

    //SEARCH CLICK, FILTER FORM REC. TSEL
    $("#btnSearch").unbind().click(function (e) {
        e.preventDefault();
        if ($("#formSearchOne").parsley().validate()) {
            AllDataId = [];
            Data.RowSelected = [];
            $("#pnlHistory").hide();


            schBapsType = $('#slFilterBapsType').val();
            schPowerType = $('#slFilterPowerType').val();

            if ($("#tabDasboard").tabs('option', 'active') == 0) {
                if ($("#formSearch").parsley().validate()) {
                    Table.Search();
                }
            }
            else {
                TableHistory.SearchHistory();
            }
        }
    });

    //SEARCH CLICK, FILTER FORM REC. NON TSEL ------------
    $("#btnSearchNonTsel").unbind().click(function (e) {
        e.preventDefault();
        if ($("#formSearchNonTsel").parsley().validate()) {
            $("#pnlHistory").hide();

            schBapsType = $('#slFilterBapsTypeNonTsel').val();
            schPowerType = $('#slFilterPowerTypeNonTsel').val();

            if ($("#tabDasboard li.active").attr('aria-controls') == 'tabRecNonTsel') {
                if ($("#formSearchNonTsel").parsley().validate()) {
                    TempChkDataNonTsel = (AllDataNonTselId || []).concat();
                    TableNonTsel.Search();
                }
            }
            else {
                TableHistory.SearchHistory();
            }
        }
    });

    //SEARCH CLICK, FILTER FORM REC. BAPS ------------
    $("#btnSearchBaps").unbind().click(function (e) {
        e.preventDefault();
        if ($("#formSearchOneBaps").parsley().validate()) {
            $("#pnlHistoryNewBaps").hide();


            schBapsType = $('#slFilterBapsTypeBaps').val();
            schPowerType = $('#slFilterPowerTypeBaps').val();

            if ($("#tabDasboard li.active").attr('aria-controls') == 'tabRecBaps') {
                if ($("#formSearchOneBaps").parsley().validate()) {
                    TempChkDataBaps = (AllDataNewBapsId || []).concat();
                    TableNewBaps.Search();
                }
            }
            else {
                TableHistory.SearchHistory();
            }
        }
    });

    //SEARCH CLICK, FILTER FORM REC. NEW PROD ------------
    $("#btnSearchProd").unbind().click(function (e) {
        e.preventDefault();
        if ($("#formSearchOneProd").parsley().validate()) {
            $("#pnlHistoryProd").hide();


            schBapsType = $('#slFilterBapsTypeProd').val();
            schPowerType = $('#slFilterPowerTypeProd').val();

            if ($("#tabDasboard li.active").attr('aria-controls') == 'tabRecNewProduct') {
                if ($("#formSearchOneProd").parsley().validate()) {
                    TempChkDataProd = (AllDataProdId || []).concat();
                    TableNewProduct.Search();
                }
            }
            else {
                TableHistory.SearchHistory();
            }
        }
    });


    $("#btSearchHS").unbind().click(function (e) {
        e.preventDefault();
        if ($("#formSearchHS").parsley().validate()) {

            $("#pnlHistory").hide();
            TableHistory.SearchHistory();
        }
    });





    $("#btReset").unbind().click(function () {
        Table.Reset();
        $("#schSONumber").val('');
        $("#schSiteID").val('');
        $("#schSiteName").val('');
        $("#schCustomerSiteID").val('');
        $('#schCustomerSiteName').val('');
        $('#schCustomerID').val('');
        $('#schRegionName').val('');
        $('#schYearBill').val('');

    });

    $("#btResetHS").unbind().click(function () {
        TableHistory.Reset();
    });

    $("#btAddSite").unbind().click(function () {
        //var l = Ladda.create(document.querySelector("#btSearch"))
        $('#slReconcileType').val($('#slFilterBapsType').val()).trigger("change");
        $('#slTypeTower').val($('#slFilterPowerType').val()).trigger("change");
        $('#slFilterBapsType').prop("disabled", false);
        $('#slFilterPowerType').prop("disabled", false);


        if (TempData.length > 0) {
            AllDataId = (TempData || []).concat();
            $("#pnlHistory").show();
            Table.Search();
            Table.AddListSearch();
            // Control.DeleteRequestDetailAll(row.MstBapsId);
        }
        else
            //Control.DeleteRequestDetailAll(row.MstBapsId);
            Common.Alert.Warning("Please Select One or More Data")
        //}

    });

    $("#btnSubmit").unbind().click(function () {
        Action.Submit();
    });

    $(".btnSearchHistory").unbind().click(function () {
        Table.Search();
    });

    $(".btnSearchHistoryNonTsel").unbind().click(function () {
        TableNonTsel.Search();
    });
    $(".btnSearchHistoryBaps").unbind().click(function () {
        TableNewBaps.Search();
    });
    $(".btnSearchHistoryProd").unbind().click(function () {
        TableNewProduct.Search();
    });

    $("#btnCancelBaps").unbind().click(function () {
        $('#pnlHistoryNewBaps').hide();
        TempChkDataBaps = [];
        AllDataNewBapsId = [];
        TableNewBaps.Search();

    });
    $("#btnCancelNonTsel").unbind().click(function () {
        $('#pnlHistoryNonTsel').hide();
        TempChkDataNonTsel = [];
        AllDataNonTselId = [];
        TableNonTsel.Search();
    });
    $("#btnCancelProd").unbind().click(function () {
        $('#pnlHistoryProd').hide();
        TempChkDataProd = [];
        AllDataProdId = [];
        Data.RowSelected = [];
        TableNewProduct.Search();
    });

    $("#tblHistoryAddSite").on('change', 'tbody tr .checkboxes', function () {
        var ID = $(this).parent().attr('id');
        var table = $("#tblHistoryAddSite").DataTable();

    });

    $("#tblSummaryData").on('change', 'tbody tr .checkboxes', function () {
        var MstBapsId = $(this).parent().attr('id');
        var table = $("#tblSummaryData").DataTable();
        var idComponents = MstBapsId
        var id = idComponents[1];
        var DataRows = {};
        var Row = table.row($(this).parents('tr')).data();

        if (this.checked) {
            if (!(AllDataId.some(m => m.MstBapsId == Row.MstBapsId && m.StartInvoiceDate == Row.StartInvoiceDate))) {
                DataRows.KeySetting = Row.KeySetting;
                DataRows.SONumber = Row.SONumber;
                DataRows.SiteID = Row.SiteID;
                DataRows.SiteName = Row.SiteName;
                DataRows.CustomerSiteID = Row.CustomerSiteID;
                DataRows.CustomerSiteName = Row.CustomerSiteName;
                DataRows.CustomerID = Row.CustomerID;
                DataRows.RegionName = Row.RegionName;
                //DataRows.YearBill = Row.YearBill;
                DataRows.StartInvoiceDate = Row.StartInvoiceDate;
                DataRows.EndInvoiceDate = Row.EndInvoiceDate;
                DataRows.AmountIDR = Row.AmountIDR;


                DataRows.MstBapsId = Row.MstBapsId;
                RowSelected = DataRows;
                TempData.push(RowSelected);

                Data.RowSelected.push(Row.MstBapsId);
            }
        } else {
            var index = TempData.findIndex(function (data) {
                return data.MstBapsId == Row.MstBapsId;
            });
            TempData.splice(index, 1);
        }
        //Table.AddListSearch();

    });

    $('#tabDasboard').unbind().on('tabsactivate', function (event, ui) {
        var newIndex = ui.newTab.index();

        schBapsType = $('#slFilterBapsType').val();
        schPowerType = $('#slFilterPowerType').val();

        if (newIndex == 0) {
            // Modification Or Added By Ibnu Setiawan 29. January 2020 Change Method and Structure
            //Table.Search();
            Control.BindSelectYearAll($('#slBillingYear'));
            //Table.Search();
        }
        else {
            // Modification Or Added By Ibnu Setiawan 29. January 2020
            //TableHistory.SearchHistory();
            Control.BindSelectYearAll($("#slYearTargetHs"));
            Control.BindSelectYearAll($("#slBillingYear"));
        }
    });

    $("#slSection").change(function () {
        Control.BindingSelectSow($("#slSow"), $('#slSection').val());
    });

    $("#slSectionNonTsel").change(function () {
        Control.BindingSelectSow($("#slSowNonTsel"), $('#slSectionNonTsel').val());
    });
    $("#slSectionBaps").change(function () {
        Control.BindingSelectSow($("#slSowBaps"), $('#slSectionBaps').val());
    });
    $("#slSectionProd").change(function () {
        Control.BindingSelectSow($("#slSowProd"), $('#slSectionProd').val());
    });
    $("#btnCancel").unbind().click(function () {
        $('#panelSearchResult').hide();
        $("#pnlHistory").hide();
        TempData = [];
        AllDataId = [];
        Data.RowSelected = [];
        Table.Search();
    });
    //EVENT CHECKBOX CLICKED
    $("#tblSummaryDataProd").on('change', 'tbody tr .checkboxes', function () {
        var MstBapsId = $(this).parent().attr('id');
        var table = $("#tblSummaryDataProd").DataTable();
        var idComponents = MstBapsId
        var id = idComponents[1];
        var DataRows = {};
        var Row = table.row($(this).parents('tr')).data();

        if (this.checked) {
            if (Control.IsEmptyData(id)) {
                DataRows.KeySetting = Row.KeySetting;
                DataRows.SONumber = Row.SONumber;
                DataRows.SiteID = Row.SiteID;
                DataRows.SiteName = Row.SiteName;
                DataRows.CustomerSiteID = Row.CustomerSiteID;
                DataRows.CustomerSiteName = Row.CustomerSiteName;
                DataRows.CustomerID = Row.CustomerID;
                DataRows.RegionName = Row.RegionName;
                DataRows.CompanyInvoiceId = Row.CompanyInvoiceId;
                //DataRows.YearBill = Row.YearBill;
                DataRows.StartInvoiceDate = Row.StartInvoiceDate;
                DataRows.EndInvoiceDate = Row.EndInvoiceDate;
                DataRows.AmountIDR = Row.AmountIDR;

                DataRows.MstBapsId = Row.MstBapsId;
                RowSelected = DataRows;
                TempChkDataProd.push(RowSelected);

                Data.RowSelected.push(Row.MstBapsId);
            }
        } else {
            var index = TempChkDataProd.findIndex(function (data) {
                return data.MstBapsId == Row.MstBapsId;
            });
            TempChkDataProd.splice(index, 1);
        }
        //TableNewProduct.AddListSearch();

    });
    //EVENT CHECKBOX CLICKED
    $("#tblSummaryDataNonTsel").on('change', 'tbody tr .checkboxes', function () {
        var MstBapsId = $(this).parent().attr('id');
        var table = $("#tblSummaryDataNonTsel").DataTable();
        var idComponents = MstBapsId
        var id = idComponents[1];
        var DataRows = {};
        var Row = table.row($(this).parents('tr')).data();

        if (this.checked) {
            if (Control.IsEmptyData(id)) {
                DataRows.KeySetting = Row.KeySetting;
                DataRows.SONumber = Row.SONumber;
                DataRows.SiteID = Row.SiteID;
                DataRows.SiteName = Row.SiteName;
                DataRows.CustomerSiteID = Row.CustomerSiteID;
                DataRows.CustomerSiteName = Row.CustomerSiteName;
                DataRows.CustomerID = Row.CustomerID;
                DataRows.RegionName = Row.RegionName;
                //DataRows.YearBill = Row.YearBill;
                DataRows.StartInvoiceDate = Row.StartInvoiceDate;
                DataRows.EndInvoiceDate = Row.EndInvoiceDate;
                DataRows.AmountIDR = Row.AmountIDR;


                DataRows.MstBapsId = Row.MstBapsId;
                RowSelected = DataRows;
                TempChkDataNonTsel.push(RowSelected);

                Data.RowSelected.push(Row.MstBapsId);
            }
        } else {
            var index = TempChkDataNonTsel.findIndex(function (data) {
                return data.MstBapsId == Row.MstBapsId && data.StartInvoiceDate == Row.StartInvoiceDate;
            });
            TempChkDataNonTsel.splice(index, 1);
        }
        //TableNonTsel.AddListSearch();

    });
    //EVENT CHECKBOX CLICKED
    $("#tblSummaryDataBaps").on('change', 'tbody tr .checkboxes', function () {
        var MstBapsId = $(this).parent().attr('id');
        var table = $("#tblSummaryDataBaps").DataTable();
        var idComponents = MstBapsId
        var id = idComponents[1];
        var DataRows = {};
        var Row = table.row($(this).parents('tr')).data();

        if (this.checked) {
            if (Control.IsEmptyData(id)) {
                DataRows.KeySetting = Row.KeySetting;
                DataRows.SONumber = Row.SONumber;
                DataRows.SiteID = Row.SiteID;
                DataRows.SiteName = Row.SiteName;
                DataRows.CustomerSiteID = Row.CustomerSiteID;
                DataRows.CustomerSiteName = Row.CustomerSiteName;
                DataRows.CustomerID = Row.CustomerID;
                DataRows.RegionName = Row.RegionName;
                DataRows.CompanyInvoiceId = Row.CompanyInvoiceId;

                //DataRows.YearBill = Row.YearBill;
                DataRows.StartInvoiceDate = Row.StartInvoiceDate;
                DataRows.EndInvoiceDate = Row.EndInvoiceDate;
                DataRows.AmountIDR = Row.AmountIDR;


                DataRows.MstBapsId = Row.MstBapsId;
                RowSelected = DataRows;
                TempChkDataBaps.push(RowSelected);

                Data.RowSelected.push(Row.MstBapsId);
            }
        } else {
            var index = TempChkDataBaps.findIndex(function (data) {
                return data.MstBapsId == Row.MstBapsId && data.StartInvoiceDate == Row.StartInvoiceDate;
            });
            if (index != null) {
                TempChkDataBaps.splice(index, 1);
            }
        }
        //TableNewBaps.AddListSearch();

    });

    //click checkbox untuk history target
    $("#tblHistoryAddSite").on('change', 'tbody tr .checkboxes', function () {
        var targetId = $(this).parent().attr('id');
 

        if (this.checked) {
            if (Control.IsEmptyData(targetId)) {
                AllDataHsIds.push(targetId);
            }
        } else {
            var index = AllDataHsIds.indexOf(targetId)
            if (index != null) {
                AllDataHsIds.splice(index, 1);
            }
        }
    });
});

var Form = {
    Init: function () {
        strUserToken = $('#hdUserToken').val();
        Control.BindingSelectCompany();
        Control.BindingSelectRegional();
        Control.BindingSelectProvince();
        Control.BindingSelectTenant();
        Control.BindingSelectCustomer();
        Control.BindingSelectSection();
        Control.BindingSelectSow("#slSow, #slSowNonTsel, #slSowBaps, #slSowProd", 0);
        Control.BindingSelectDepartment();
        Control.BindingSelectSiro("#slFilterStipSiroBaps");
        Control.BindingSelectMonthYear("#slFilterEndDateBaps, #slFilterStartDateBaps");
        $('#slFilterStartDateBaps').datepicker().on("changeDate", function () {
            var startVal = $('#slFilterStartDateBaps').val();
            $('#slFilterEndDateBaps').data('datepicker').setStartDate(startVal);
        });
        $('#slFilterEndDateBaps').datepicker().on("changeDate", function () {
            var endVal = $('#slFilterEndDateBaps').val();
            $('#slFilterStartDateBaps').data('datepicker').setEndDate(endVal);
        });
        Control.BindingSelectMonthYear("#slFilterStartDateProd, #slFilterEndDateProd");
        $('#slFilterStartDateProd').datepicker().on("changeDate", function () {
            var startVal = $('#slFilterStartDateProd').val();
            $('#slFilterEndDateProd').data('datepicker').setStartDate(startVal);
        });
        $('#slFilterEndDateProd').datepicker().on("changeDate", function () {
            var endVal = $('#slFilterEndDateProd').val();
            $('#slFilterStartDateProd').data('datepicker').setEndDate(endVal);
        });
        Control.BindingSelectMonthYear("#slFilterStartDateTsel, #slFilterEndDateTsel");
        $('#slFilterStartDateTsel').datepicker().on("changeDate", function () {
            var startVal = $('#slFilterStartDateTsel').val();
            $('#slFilterEndDateTsel').data('datepicker').setStartDate(startVal);
        });
        $('#slFilterEndDateTsel').datepicker().on("changeDate", function () {
            var endVal = $('#slFilterEndDateTsel').val();
            $('#slFilterStartDateTsel').data('datepicker').setEndDate(endVal);
        });
        Control.BindingSelectMonthYear("#slFilterStartDateNonTsel, #slFilterEndDateNonTsel");
        $('#slFilterStartDateNonTsel').datepicker().on("changeDate", function () {
            var startVal = $('#slFilterStartDateNonTsel').val();
            $('#slFilterEndDateNonTsel').data('datepicker').setStartDate(startVal);
        });
        $('#slFilterEndDateNonTsel').datepicker().on("changeDate", function () {
            var endVal = $('#slFilterEndDateNonTsel').val();
            $('#slFilterStartDateNonTsel').data('datepicker').setEndDate(endVal);
        });
        $("#slSONumberMultiple").select2({
            tags: true,
            multiple: true,
            width: '100%',
            createTag: function (params) {
                return {
                    id: params.term,
                    text: params.term,
                    newOption: true
                }
            },
            templateResult: function (data) {
                var $result = $("<span></span>");

                $result.text(data.text);

                if (data.newOption) {
                    $result.append(" <em>(add)</em>");
                }

                return $result;
            }
        });
        //join as 1 selector, to make just single request to the server
        Control.BindingSelectYearTarget($('#slYearTarget, #slYearTargetNonTsel, #slYearTargetBaps, #slYearTargetProd'));
        Control.BindingSelectMonthTarget($('#slMonthTarget, #slMonthTargetNonTsel, #slMonthTargetBaps, #slMonthTargetProd, #slMonthBaps'));
        // 10 before and after
        Control.BindSelectYearAll($('#slYearBaps, #slYearProd, #slYearTargetHs'))
        Control.BindSelectMonth($('#slMonthProd, #slMonthBaps, #slMonthTargetHs'))

        var selectorsReconcileType = '#slReconcileType, #slReconcileTypeNonTsel, #slReconcileTypeBaps, #slReconcileTypeProd, ' +
                                     '#slFilterBapsType, #slFilterBapsTypeNonTsel, #slFilterBapsTypeBaps, #slFilterBapsTypeProd';
        Control.BindingSelectReconcileType(selectorsReconcileType);

        Control.BindingSelectReconType($('#slReconType'));

        var selectorsPwrType = '#slPWRType, #slPWRTypeNonTsel, #slPWRTypeBaps, #slPWRTypeProd';
        Control.BindingFilterPwrType(selectorsPwrType);

        Control.BindingSelectTenantInNewProduct('#slTenantTypeProd', '#slCustomerInvoiceProd');





        $('#slFilterBapsType').change(function () {
            //alert($('#slReconcileType').val());

            if ($('#slFilterBapsType').val() == '5') {
                $('#slFilterPowerType').prop("disabled", true);
                $('#slFilterPowerType').val("").trigger('change');
                $('#slFilterPowerType').prop("required", false);
            } else {
                $('#slFilterPowerType').prop("disabled", false);
                $('#slFilterPowerType').prop("required", true);
            }

        });


        $('#slFilterBapsTypeNonTsel').change(function () {
            //alert($('#slReconcileType').val());

            if ($('#slFilterBapsTypeNonTsel').val() == '5') {
                $('#slFilterPowerTypeNonTsel').prop("disabled", true);
                $('#slFilterPowerTypeNonTsel').val("").trigger('change');
                $('#slFilterPowerTypeNonTsel').prop("required", false);
            } else {
                $('#slFilterPowerTypeNonTsel').prop("disabled", false);
                $('#slFilterPowerTypeNonTsel').prop("required", true);
            }

        });
        $('#slFilterBapsTypeBaps').change(function () {
            //alert($('#slReconcileType').val());

            if ($('#slFilterBapsTypeBaps').val() == '5') {
                $('#slFilterPowerTypeBaps').prop("disabled", true);
                $('#slFilterPowerTypeBaps').val("").trigger('change');
                $('#slFilterPowerTypeBaps').prop("required", false);
            } else {
                $('#slFilterPowerTypeBaps').prop("disabled", false);
                $('#slFilterPowerTypeBaps').prop("required", true);
            }

        });
        $('#slFilterBapsTypeProd').change(function () {
            //alert($('#slReconcileType').val());

            if ($('#slFilterBapsTypeProd').val() == '5') {
                $('#slFilterPowerTypeProd').prop("disabled", true);
                $('#slFilterPowerTypeProd').val("").trigger('change');
                $('#slFilterPowerTypeProd').prop("required", false);
            } else {
                $('#slFilterPowerTypeProd').prop("disabled", false);
                $('#slFilterPowerTypeProd').prop("required", true);
            }

        });
        $('#slReconType').change(function () {
            //alert($('#slReconcileType').val());

            if ($('#slReconType').val() == '5') {
                $('#slPWRType').prop("disabled", true);
                $('#slPWRType').val("").trigger('change');
                $('#slPWRType').prop("required", false);
            } else {
                $('#slPWRType').prop("disabled", false);
                $('#slFilterPowerType').prop("required", true);
            }

        });


        Control.BindingPnlHistoryPowerType('#slTypeTower');
        Control.BindingPnlHistoryPowerType('#slTypeTowerNonTsel');
        Control.BindingPnlHistoryPowerType('#slTypeTowerBaps');
        Control.BindingPnlHistoryPowerType('#slTypeTowerProd');
        Control.BindingFilterPowerType();
        $('#tabDasboard').tabs();

    },

}

var Action = {
    Submit: function () {
        var result = "";
        if ($('#slYearTarget').val() == "" || $('#slYearTarget').val() == null)
            result += " Year Target,";
        if ($('#slMonthTarget').val() == "" || $('#slMonthTarget').val() == null)
            result += " Month Target,";
        if ($('#slReconcileType').val() == "" || $('#slReconcileType').val() == null)
            result += " Reconcile Type,";
        //if ($('#slTypeTower').val() == "")
        //    result += " Type Tower";


        if (result == "") {
            if (TempData.length > 0) {
                var requestData = [];
                var paramAll = "";
                param = {};
                if (CheckAllDataFlag) {
                    paramAll = "Not Yet";
                    param.CompanyInvoiceId = $('#slCompanyInvoice').val();
                    param.CustomerID = $('#slCustomerInvoice').val();
                    param.SectionProductId = $('#slSection').val();
                    param.SowProductId = $('#slSow').val();
                    //param.YearBill = $('#slBillingYear').val();
                    param.RegionID = $('#slRegional').val();
                    param.ProvinceID = $('#slProvince').val();
                    param.ProductID = $('#slTenantType').val();
                    //requestData = AllDataId;
                }

                else {
                    for (var i = 0; i < TempData.length; i++) {
                        requestData.push(TempData[i].KeySetting);
                        requestData.push(TempData[i].YearBill);
                    }

                }

                var params = {
                    YearTarget: $('#slYearTarget').val(),
                    MonthBill: $('#slMonthTarget').val(),
                    BapsType: $('#slReconcileType').val(),
                    PowerType: $('#slReconcileType').val() == '5' ? "0000" : $('#slTypeTower').val(),
                    DepartmentCode: settingVariable.deptCodeTsel,
                    vwRABapsSiteList: AllDataId,
                }
                $.ajax({
                    url: "/api/DashboardTSEL/AddDataDashboardTSEL",
                    type: "post",
                    dataType: "json",
                    contentType: "application/json",
                    data: JSON.stringify(params),
                    cache: false,
                    beforeSend: function (xhr) {
                    }
                }).done(function (data, textStatus, jqXHR) {
                    if (Common.CheckError.List(data)) {
                        if (data.ErrorType != 0) {
                            Common.Alert.Warning(data.ErrorMessage);
                        } else {
                            Common.Alert.Success("Data has been saved!")
                            //$(".panelSiteData").hide();
                            $("#pnlHistory").hide();
                            $("#slYearTarget").val("").trigger('change');
                            $("#slMonthTarget").val("").trigger('change');
                            $("#slReconcileType").val("").trigger('change');
                            $("#slTypeTower").val("").trigger('change');
                            //$(".panelAddSiteHistory").hide();
                            TempData = [];
                            AllDataId = [];
                            Table.Search();

                            $('#slFilterBapsType').prop("disabled", false);
                            $('#slFilterPowerType').prop("disabled", false);
                        }

                    }
                    else {
                        Common.Alert.Error(data.ErrorMessage);
                    }
                    //  l2.stop();
                    // l.stop();
                })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        Common.Alert.Error(errorThrown)
                        l2.stop();
                        l.stop()
                    })
            } else {
                // Common.Alert.Error("Please Input Year Target, Month Target, Reconcile Type and Type Power !");
                Common.Alert.Warning("Request cannot be empty");
            }
        }

        else
            Common.Alert.Warning(result + " Is Mandatory");


    },
    GetListData: function (params) {
        $.ajax({
            url: "/api/DashboardTSEL/GetListData",
            type: "post",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                TempData = [];
                TempData = data;
                Table.AddListSearch();
                $("#pnlHistory").show();
                Table.Search();
            }
            else {
                Common.Alert.Error(data.ErrorMessage);
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        })
    }
}
//GRID TABLE TSEL
var Table = {
    Init: function () {

        //$("#tblSummaryData").hide();
        $(".panelSearchZero").hide();
        //$(".panelSearchResult").hide();
        $("#formHistory").hide();
        $("#pnlHistory").hide();

        var tblSummaryData = $('#tblSummaryData').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $("#tblsummarydata tbody").on("click", "a.btdetail", function (e) {
            e.preventdefault();
            var table = $('#tblsummarydata').DataTable();
            var data = table.row($(this).parents("tr")).data();

        });

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });

    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btnSearch"))
        l.start();

        var tblSummaryData = $("#tblSummaryData").DataTable({
            "deferRender": true,
            "processing": true,
            "serverSide": true,
            "bSortCellsTop": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/DashboardTSEL/TSELGrid",
                "type": "POST",
                "datatype": "json",
                "data": Table.GetParam(),
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
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Helper.IsGridDataExistsInTempData(full.MstBapsId, full.StartInvoiceDate, TempData)) { //Data.RowSelected)) {
                            }
                            else {
                                strReturn += "<label id='" + full.MstBapsId + "_" + full.YearBill + "_" + full.MonthBill + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.MstBapsId + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CustomerID" },
                { data: "RegionName" },
                {
                    data: "StartInvoiceDate", render: function (data, type, full) {
                        if (data != null) {
                            return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data, type, full) {
                        if (data != null) {
                            return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "AmountIDR", render: function (data, type, full) {
                        if (data != null) {
                            return (data).format(0);
                        } else {
                            return "";
                        }
                    }
                },
            ],


            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({
                    target: ".panelSearchResult", boxed: true
                });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (AllDataId.length > 0) {
                    var item;
                    for (var i = 0; i < AllDataId.length; i++) {
                        item = AllDataId[i].MstBapsId + '_' + AllDataId[i].YearBill + '_' + AllDataId[i].MonthBill;
                        $("." + item).addClass("active");
                    }
                }
            },
            "order": [],
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                $(row).addClass("." + data.MstBapsId + "_" + data.YearBill + "_" + data.MonthBill);
            },
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                    '<input type="checkbox" name="SelectAll" class="group-checkable chkAll-tsel" data-set="#tblSummaryData .checkboxes" />' +
                    '<span></span> ' +
                    '</label>';
                var th = $("th.select-all-raw").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-raw").unbind().on("change", ".group-checkable.chkAll-tsel", function (e) {
                    var set = $(".panelSearchResult .checkboxes");
                    var checked = jQuery(this).is(":checked");
                    jQuery(set).each(function () {
                        label = $(this).parent();
                        var id = label.attr("id");
                        if (label.attr("style") != "display:none") {
                            /* Replace the following code with the code to select all checkboxes in all pages */
                            if (checked) {
                                $(this).prop("checked", true);
                                $(this).parents('tr').addClass('active');

                                $("." + id).addClass("active");
                                $("." + id).prop("checked", true);
                            } else {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");

                                $("." + id).removeClass("active");
                                $("." + id).prop("checked", false);
                            }
                        }
                    });
                    if (checked) {
                        App.blockUI({
                            target: ".panelSearchResult", boxed: true
                        });
                        TempData = [];
                        TempData = Helper.GetListId();
                        App.unblockUI('.panelSearchResult');

                    }
                    else
                        TempData = (AllDataId || []).concat();
                });
            }
        });
    },
    AddListSearch: function () {
        AllDataId = (TempData || []).concat();
        $("#tblAddSite").DataTable({
            "serverSide": false,
            "filter": true,
            "destroy": true,
            "async": false,
            "data": AllDataId,
            "columns": [{
                orderable: false,
                mRender: function (data, type, full) {
                    return "<i class='fa fa-remove btn bn-xs red deleteRow'></i>";
                }
            },
            { data: "SONumber" },
            { data: "SiteID" },
            { data: "SiteName" },
            { data: "CustomerSiteID" },
            { data: "CustomerSiteName" },
            { data: "CustomerID" },
            { data: "RegionName" },
                {
                    data: "StartInvoiceDate", render: function (data, type, full) {
                        if (data != null) {
                            return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data, type, full) {
                        if (data != null) {
                            return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "AmountIDR", render: function (data, type, full) {
                        if (data != null) {
                            return (data).format(0);
                        } else {
                            return "";
                        }
                    }
                },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
        });
        $("#tblAddSite").unbind();
        $("#tblAddSite").on("click", ".deleteRow", function (e) {
            var table = $("#tblAddSite").DataTable();
            var row = table.row($(this).parents('tr')).data();
            table.row($(this).parents('tr')).remove().draw(false);
            Helper.RemoveDataInTempData(row.MstBapsId, row.StartInvoiceDate, TempData);
            Helper.RemoveDataInTempData(row.MstBapsId, row.StartInvoiceDate, AllDataId);
            Table.Search();
            if (AllDataId.length < 1) {
                $("#pnlHistoryNonTsel").hide();
                $('#slFilterBapsType').prop("disabled", false);
                $('#slFilterPowerType').prop("disabled", false);
            }
        });
    },
    Export: function () {
        var paramObject = Table.GetParam();
        var param = $.param(paramObject);

        window.location.href = "/DashboardTSEL/ListDashboardTSEL/Export?" + param;
    },
    Reset: function () {
        var dt = new Date();
        var currentyear = dt.getFullYear();
        $("#slCompanyInvoice").val("").trigger('change');
        $("#slSection").val("").trigger('change');
        $("#slSow").val("").trigger('change');
        $("#slRegional").val("").trigger('change');
        $("#slProvince").val("").trigger('change');
        $("#slTenantType").val("").trigger('change');

        $("#slFilterBapsType").val(null).trigger('change');
        $("#slFilterPowerType").val(null).trigger('change');
        $("#slFilterStartDateTsel").val(null).trigger('changeDate');
        $("#slFilterEndDateTsel").val(null).trigger('changeDate');

        TempData = [];
        AllDataId = [];
        Table.Init();

    },
    GetParam: function () {
        var SONumber = $("#schSONumber").val();
        var SiteID = $("#schSiteID").val();
        var SiteName = $("#schSiteName").val();
        var CustomerSiteID = $("#schCustomerSiteID").val();
        var CustomerSiteName = $("#schCustomerSiteName").val();
        var CustomerID = $("#schCustomerID").val();
        var RegionName = $("#schRegionName").val();
        var BapsType = $('#slFilterBapsType').val();
        var PowerType = $('#slFilterPowerType').val();

        var sDate = $("#slFilterStartDateTsel").val() != '' ? moment('1 ' + $("#slFilterStartDateTsel").val(), 'D MMM YYYY').format('YYYY/MM/DD') : '';
        var _eDate = '';
        if ($("#slFilterEndDateTsel").val() != '') {
            var eDate = moment('1 ' + $("#slFilterEndDateTsel").val(), 'D MMM YYYY').format('YYYY/MM/DD');
            var dayInMonth = moment(eDate, "YYYY/MM/DD").daysInMonth()
            _eDate = moment(eDate, 'YYYY/MM/DD').set({ 'D': dayInMonth }).format('YYYY/MM/DD');
        }

        fsCompanyInvoice = $("#slCompanyInvoice").val() == null ? "" : $("#slCompanyInvoice").val();
        fsCustomerInvoice = $("#slCustomerInvoice").val() == null ? "" : $("#slCustomerInvoice").val();
        fsSection = $("#slSection").val() == null ? "" : $("#slSection").val();
        fsSOW = $("#slSow").val() == null ? "" : $("#slSow").val();
        fsRegional = $("#slRegional").val() == null ? "" : $("#slRegional").val();
        fsProvince = $("#slProvince").val() == null ? "" : $("#slProvince").val();
        fsTenantType = $("#slTenantType").val() == null ? "" : $("#slTenantType").val();

        var params = {
            strCompanyInvoice: fsCompanyInvoice,
            strCustomerInvoice: fsCustomerInvoice,
            strSection: fsSection,
            strSOW: fsSOW,
            strRegional: fsRegional,
            strProvince: fsProvince,
            strTenantType: fsTenantType,
            isReceive: 0,
            schSONumber: SONumber,
            schSiteID: SiteID,
            schSiteName: SiteName,
            schCustomerSiteID: CustomerSiteID,
            schCustomerSiteName: CustomerSiteName,
            schCustomerID: CustomerID,
            schRegionName: RegionName,
            BapsType: BapsType,
            PowerType: PowerType,
            StartInvoiceDate: sDate,
            EndInvoiceDate: _eDate
        };
        return params;
    }
}

//GRID TABLE NON TSEL
var TableNonTsel = {
    Init: function () {
        $(".panelSearchZero").hide();
        $("#pnlHistoryNonTsel").hide();

        var tblSummaryDataNonTsel = $('#tblSummaryDataNonTsel').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $("#tblSummaryDataNonTsel tbody").on("click", "a.btdetail", function (e) {
            e.preventdefault();
            var table = $('#tblSummaryDataNonTsel').DataTable();
            var data = table.row($(this).parents("tr")).data();

        });

        $(window).resize(function () {
            $("#tblSummaryDataNonTsel").DataTable().columns.adjust().draw();
        });
        //EVENT CLICK ADD SITE
        $("#btAddSiteNonTsel").unbind().click(function () {
            //var l = Ladda.create(document.querySelector("#btSearch"))
            $('#slReconcileTypeNonTsel').val($('#slFilterBapsTypeNonTsel').val()).trigger("change");
            $('#slTypeTowerNonTsel').val($('#slFilterPowerTypeNonTsel').val()).trigger("change");
            $('#slFilterBapsTypeNonTsel').prop("disabled", false);
            $('#slFilterPowerTypeNonTsel').prop("disabled", false);
            if (TempChkDataNonTsel.length > 0) {
                AllDataNonTselId = (TempChkDataNonTsel || []).concat();
                $("#pnlHistoryNonTsel").show();
                TableNonTsel.Search();
                TableNonTsel.AddListSearch();
            }
            else
                Common.Alert.Warning("Please Select One or More Data")
        });
        $("#btResetNonTsel").unbind().click(function () {
            TableNonTsel.Reset();
        });
        //SUBMIT BUTTON EVENT
        $("#btnSubmitNonTsel").unbind().click(function () {
            TableNonTsel.Submit();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btnSearchNonTsel"))
        l.start();
        var SONumber = $("#schSONumberNonTsel").val();
        var SiteID = $("#schSiteIDNonTsel").val();
        var SiteName = $("#schSiteNameNonTsel").val();
        var CustomerSiteID = $("#schCustomerSiteIDNonTsel").val();
        var CustomerSiteName = $("#schCustomerSiteNameNonTsel").val();
        var CustomerID = $("#schCustomerIDNonTsel").val();
        var RegionName = $("#schRegionNameNonTsel").val();
        var BapsType = $('#slFilterBapsTypeNonTsel').val();
        var PowerType = $('#slFilterPowerTypeNonTsel').val();

        var sDate = $("#slFilterStartDateNonTsel").val() != '' ? moment('1 ' + $("#slFilterStartDateNonTsel").val(), 'D MMM YYYY').format('YYYY/MM/DD') : '';
        var _eDate = '';
        if ($("#slFilterEndDateNonTsel").val() != null) {
            var eDate = moment('1 ' + $("#slFilterEndDateNonTsel").val(), 'D MMM YYYY').format('YYYY/MM/DD');
            var dayInMonth = moment(eDate, "YYYY/MM/DD").daysInMonth()
            _eDate = moment(eDate, 'YYYY/MM/DD').set({ 'D': dayInMonth }).format('YYYY/MM/DD');
        }

        fsCompanyInvoice = $("#slCompanyInvoiceNonTsel").val() == null ? "" : $("#slCompanyInvoiceNonTsel").val();
        fsCustomerInvoice = $("#slCustomerInvoiceNonTsel").val() == null ? "" : $("#slCustomerInvoiceNonTsel").val();
        fsSection = $("#slSectionNonTsel").val() == null ? "" : $("#slSectionNonTsel").val();
        fsSOW = $("#slSowNonTsel").val() == null ? "" : $("#slSowNonTsel").val();
        fsBillingYear = $("#slBillingYearNonTsel").val() == null ? "" : $("#slBillingYearNonTsel").val();
        fsRegional = $("#slRegionalNonTsel").val() == null ? "" : $("#slRegionalNonTsel").val();
        fsProvince = $("#slProvinceNonTsel").val() == null ? "" : $("#slProvinceNonTsel").val();
        fsTenantType = $("#slTenantTypeNonTsel").val() == null ? "" : $("#slTenantTypeNonTsel").val();

        var params = {
            strCompanyInvoice: fsCompanyInvoice,
            strCustomerInvoice: fsCustomerInvoice,
            strSection: fsSection,
            strSOW: fsSOW,
            strBillingYear: fsBillingYear,
            strRegional: fsRegional,
            strProvince: fsProvince,
            strTenantType: fsTenantType,
            isReceive: 0,
            schSONumber: SONumber,
            schSiteID: SiteID,
            schSiteName: SiteName,
            schCustomerSiteID: CustomerSiteID,
            schCustomerSiteName: CustomerSiteName,
            schCustomerID: CustomerID,
            schRegionName: RegionName,
            //schYearBill: YearBill,
            BapsType: BapsType,
            PowerType: PowerType,
            StartInvoiceDate: sDate,
            EndInvoiceDate: _eDate
        };

        var tblSummaryDataNonTsel = $("#tblSummaryDataNonTsel").DataTable({
            "deferRender": true,
            "processing": true,
            "serverSide": true,
            "bSortCellsTop": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/ApiInputTarget/NonTselGrid",
                "type": "POST",
                "datatype": "json",
                "data": TableNonTsel.GetParam(),
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        TableNonTsel.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Helper.IsGridDataExistsInTempData(full.MstBapsId, full.StartInvoiceDate, TempChkDataNonTsel)) { //Data.RowSelected)) {
                            }
                            else {
                                strReturn += "<label id='" + full.MstBapsId + "_" + full.YearBill + "_" + full.MonthBill + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.MstBapsId + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CustomerID" },
                { data: "RegionName" },
                {
                    data: "StartInvoiceDate", render: function (data, type, full) {
                        if (data != null) {
                            return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data, type, full) {
                        if (data != null) {
                            return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "AmountIDR", render: function (data, type, full) {
                        if (data != null) {
                            return (data).format(0);
                        } else {
                            return "";
                        }
                    }
                },
            ],


            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({
                    target: ".panelSearchResult", boxed: true
                });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryDataNonTsel.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (AllDataNonTselId.length > 0) {
                    var item;
                    for (var i = 0; i < AllDataNonTselId.length; i++) {
                        item = AllDataNonTselId[i].MstBapsId + '_' + AllDataNonTselId[i].YearBill + '_' + AllDataNonTselId[i].MonthBill;
                        $("." + item).addClass("active");
                    }
                }
            },
            "order": [],
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                $(row).addClass("." + data.MstBapsId + "_" + data.YearBill + "_" + data.MonthBill);
            },
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                    '<input type="checkbox" name="SelectAll" class="group-checkable chkAll-NonTsel" data-set="#tblSummaryDataNonTsel .checkboxes" />' +
                    '<span></span> ' +
                    '</label>';
                var th = $("th.select-all-raw").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-raw").unbind().on("change", ".group-checkable.chkAll-NonTsel", function (e) {
                    var set = $(".panelSearchResult .checkboxes");
                    var checked = jQuery(this).is(":checked");
                    jQuery(set).each(function () {
                        label = $(this).parent();
                        var id = label.attr("id");
                        if (label.attr("style") != "display:none") {
                            /* Replace the following code with the code to select all checkboxes in all pages */
                            if (checked) {
                                $(this).prop("checked", true);
                                $(this).parents('tr').addClass('active');

                                $("." + id).addClass("active");
                                $("." + id).prop("checked", true);
                            } else {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");

                                $("." + id).removeClass("active");
                                $("." + id).prop("checked", false);
                            }
                        }
                    });
                    if (checked) {
                        TempChkDataNonTsel = [];
                        TempChkDataNonTsel = TableNonTsel.GetListId();
                    }
                    else
                        TempChkDataNonTsel = (AllDataNonTselId || []).concat();
                });
            }
        });
    },
    AddListSearch: function () {
        AllDataNonTselId = (TempChkDataNonTsel || []).concat();
        $("#tblAddSiteNonTsel").DataTable({
            "serverSide": false,
            "filter": true,
            "destroy": true,
            "async": false,
            "data": AllDataNonTselId,
            "columns": [{
                orderable: false,
                mRender: function (data, type, full) {
                    return "<i class='fa fa-remove btn bn-xs red deleteRow'></i>";
                }
            },
            { data: "SONumber" },
            { data: "SiteID" },
            { data: "SiteName" },
            { data: "CustomerSiteID" },
            { data: "CustomerSiteName" },
            { data: "CustomerID" },
            { data: "RegionName" },
                {
                    data: "StartInvoiceDate", render: function (data, type, full) {
                        if (data != null) {
                            return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data, type, full) {
                        if (data != null) {
                            return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "AmountIDR", render: function (data, type, full) {
                        if (data != null) {
                            return (data).format(0);
                        } else {
                            return "";
                        }
                    }
                },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
        });
        $("#tblAddSiteNonTsel").unbind();
        $("#tblAddSiteNonTsel").on("click", ".deleteRow", function (e) {
            var table = $("#tblAddSiteNonTsel").DataTable();
            var row = table.row($(this).parents('tr')).data();
            table.row($(this).parents('tr')).remove().draw(false);
            Helper.RemoveDataInTempData(row.MstBapsId, row.StartInvoiceDate, AllDataNonTselId);
            Helper.RemoveDataInTempData(row.MstBapsId, row.StartInvoiceDate, TempChkDataNonTsel);
            TableNonTsel.Search();
            if (AllDataNonTselId.length < 1) {
                $("#pnlHistoryNonTsel").hide();
                $('#slFilterBapsTypeNonTsel').prop("disabled", false);
                $('#slFilterPowerTypeNonTsel').prop("disabled", false);
            }
        });
    },
    Export: function () {
        var paramObject = TableNonTsel.GetParam();
        var param = $.param(paramObject);
        window.location.href = "/DashboardTSEL/ListDashboardNonTSEL/Export?" + param;
    },
    Reset: function () {
        var dt = new Date();
        var currentyear = dt.getFullYear();
        $("#slCompanyInvoiceNonTsel").val("").trigger('change');
        $("#slSectionNonTsel").val("").trigger('change');
        $("#slSowNonTsel").val("").trigger('change');
        $("#slRegionalNonTsel").val("").trigger('change');
        $("#slProvinceNonTsel").val("").trigger('change');
        $("#slTenantTypeNonTsel").val("").trigger('change');
        $("#schCustomerSiteIDNonTsel").val("").trigger('change');
        $("#slCustomerInvoiceNonTsel").val("").trigger('change');
        $("#schCustomerSiteNameNonTsel").val("").trigger('change');
        $("#slFilterBapsTypeNonTsel").val(null).trigger('change');
        $("#slFilterPowerTypeNonTsel").val(null).trigger('change');
        $("#slFilterStartDateNonTsel").val(null).trigger('changeDate');
        $("#slFilterEndDateNonTsel").val(null).trigger('changeDate');
        $("#tblSummaryDataNonTsel input").val("").trigger('change');
        TempChkDataNonTsel = [];
        AllDataNonTselId = [];
        TableNonTsel.Init();

    },
    Submit: function () {
        var result = "";
        if ($('#slYearTargetNonTsel').val() == "" || $('#slYearTargetNonTsel').val() == null)
            result += " Year Target,";
        if ($('#slMonthTargetNonTsel').val() == "" || $('#slMonthTargetNonTsel').val() == null)
            result += " Month Target,";
        if ($('#slReconcileTypeNonTsel').val() == "" || $('#slReconcileTypeNonTsel').val() == null)
            result += " Reconcile Type,";

        if (result == "") {
            if (TempChkDataNonTsel.length > 0) {
                var requestData = [];
                var params = {
                    YearTarget: $('#slYearTargetNonTsel').val(),
                    MonthBill: $('#slMonthTargetNonTsel').val(),
                    BapsType: $('#slReconcileTypeNonTsel').val(),
                    PowerType: $('#slReconcileTypeNonTsel').val() == '5' ? "0000" : $('#slTypeTowerNonTsel').val(),
                    DepartmentCode: settingVariable.deptCodeNonTsel,
                    vwRABapsSiteList: TempChkDataNonTsel,
                }
                $.ajax({
                    url: "/api/DashboardTSEL/AddDataDashboardTSEL",
                    type: "post",
                    dataType: "json",
                    contentType: "application/json",
                    data: JSON.stringify(params),
                    cache: false,
                    beforeSend: function (xhr) {
                    }
                }).done(function (data, textStatus, jqXHR) {
                    if (Common.CheckError.List(data)) {
                        if (data.ErrorType != 0) {
                            Common.Alert.Warning(data.ErrorMessage);
                        } else {
                            Common.Alert.Success("Data has been saved!")
                            $("#pnlHistoryNonTsel").hide();
                            $("#slYearTargetNonTsel").val("").trigger('change');
                            $("#slMonthTargetNonTsel").val("").trigger('change');
                            $("#slReconcileTypeNonTsel").val("").trigger('change');
                            $("#slTypeTowerNonTsel").val("").trigger('change');
                            TempChkDataNonTsel = [];
                            AllDataNonTselId = [];
                            TableNonTsel.Search();

                            $('#slFilterBapsTypeNonTsel').prop("disabled", false);
                            $('#slFilterPowerTypeNonTsel').prop("disabled", false);
                        }

                    }
                    else {
                        Common.Alert.Error(data.ErrorMessage);
                    }
                })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        Common.Alert.Error(errorThrown)
                        l2.stop();
                        l.stop()
                    })
            } else {
                Common.Alert.Warning("Request cannot be empty");
            }
        }

        else
            Common.Alert.Warning(result + " Is Mandatory");


    },
    GetListId: function () {
        var AjaxData = [];
        App.blockUI({
            target: ".panelSearchResult", boxed: true
        });
        try {
            var params = TableNonTsel.GetParam();
            $.ajax({
                url: "/api/ApiInputTarget/GetListNonTselData",
                type: "POST",
                dataType: "json",
                async: false,
                data: params,
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                AjaxData = data;
                App.unblockUI('.panelSearchResult');
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                App.unblockUI('.panelSearchResult');
            });
        } catch (err) {
            Common.Alert.Error(err)
            App.unblockUI('.panelSearchResult');
        }
        return AjaxData;
    },
    GetParam: function () {
        var sDate = $("#slFilterStartDateNonTsel").val() != '' ? moment('1 ' + $("#slFilterStartDateNonTsel").val(), 'D MMM YYYY').format('YYYY/MM/DD') : '';
        var _eDate = '';
        if ($("#slFilterEndDateNonTsel").val() != '') {
            var eDate = moment('1 ' + $("#slFilterEndDateNonTsel").val(), 'D MMM YYYY').format('YYYY/MM/DD');
            var dayInMonth = moment(eDate, "YYYY/MM/DD").daysInMonth()
            _eDate = moment(eDate, 'YYYY/MM/DD').set({ 'D': dayInMonth }).format('YYYY/MM/DD');
        }
        return {
            strCompanyInvoice: $("#slCompanyInvoiceNonTsel").val(),
            strCustomerInvoice: $("#slCustomerInvoiceNonTsel").val(),
            strSection: $("#slSectionNonTsel").val(),
            strSOW: $("#slSowNonTsel").val(),
            strBillingYear: fsBillingYear,
            strRegional: $("#slRegionalNonTsel").val(),
            strProvince: $("#slProvinceNonTsel").val(),
            strTenantType: $("#slTenantTypeNonTsel").val(),
            schSONumber: $("#schSONumberNonTsel").val(),
            schSiteID: $("#schSiteIDNonTsel").val(),
            schSiteName: $("#schSiteNameNonTsel").val(),
            schCustomerSiteID: $("#schCustomerSiteIDNonTsel").val(),
            schCustomerSiteName: $("#schCustomerSiteNameNonTsel").val(),
            schCustomerID: $("#schCustomerSiteNameNonTsel").val(),
            schRegionName: $("#schRegionNameNonTsel").val(),
            schYearBill: $("#schYearBill").val(),
            PowerType: $('#slFilterPowerTypeNonTsel').val(),
            BapsType: $('#slFilterBapsTypeNonTsel').val(),
            ReconcileType: $('#slFilterBapsTypeNonTsel').val(),
            StartInvoiceDate: sDate,
            EndInvoiceDate: _eDate
        }
    }
}

//GRID TABLE HISTORY
var TableHistory = {
    Init: function () {
        var tblSummaryDataHistory = $('#tblHistoryAddSite').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblHistoryAddSite").DataTable().columns.adjust().draw();
        });
    },
    SearchHistory: function () {
        fsCompanyInvoice = $("#slCompanyInvoice").val() == null ? "" : $("#slCompanyInvoice").val();
        fsCustomerInvoice = $("#slCustomerInvoice").val() == null ? "" : $("#slCustomerInvoice").val();
        fsSection = $("#slSection").val() == null ? "" : $("#slSection").val();
        fsSOW = $("#slSow").val() == null ? "" : $("#slSow").val();
        fsBillingYear = $("#slBillingYear").val() == null ? "" : $("#slBillingYear").val();
        fsRegional = $("#slRegional").val() == null ? "" : $("#slRegional").val();
        fsProvince = $("#slProvince").val() == null ? "" : $("#slProvince").val();
        fsTenantType = $("#slTenantType").val() == null ? "" : $("#slTenantType").val();
        fsMonthTargetHistory = $("#slYearTargetHs").val() == null ? "" : $("#slYearTargetHs").val();
        fsYearTargetHistory = $("#slMonthTargetHs").val() == null ? "" : $("#slMonthTargetHs").val();
        fsReconcileType = $("#slReconType").val() == null ? "" : $("#slReconType").val();
        fsPowerType = $("#slPWRType").val() == null ? "" : $("#slPWRType").val();
        fsDepartmentCode = $("#slDepartmentName").val() == null ? "" : $("#slDepartmentName").val();

        fsCompanyHistory = $("#slCompanyHs").val() == null ? "" : $("#slCompanyHs").val();
        fsCustomerHistory = $("#slCustomerHs").val() == null ? "" : $("#slCustomerHs").val();

        var l = Ladda.create(document.querySelector("#btSearchHS"))
        l.start();
        var params = {
            strCompanyInvoice: fsCompanyInvoice,
            strCustomerInvoice: fsCustomerInvoice,
            strSection: fsSection,
            strSOW: fsSOW,
            strBillingYear: fsBillingYear,
            strRegional: fsRegional,
            strProvince: fsProvince,
            strTenantType: fsTenantType,
            strYearTargetHistory: fsMonthTargetHistory,
            strMonthTargetHistory: fsYearTargetHistory,
            slReconType: fsReconcileType,
            slPWRType: fsPowerType,
            BapsType: fsReconcileType,
            PowerType: fsPowerType,
            DepartmentCode: fsDepartmentCode,

            strCompanyInvoice: fsCompanyHistory,
            strCustomerInvoice: fsCustomerHistory,


            strSONumberMultiple: $("#slSONumberMultiple").val()

        }
        var tblSummaryData = $("#tblHistoryAddSite").DataTable({
            "deferRender": false,
            "processing": false,
            "serverSide": true,
            "bSortCellsTop": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/DashboardTSEL/listhistory",
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
                        TableHistory.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    orderable : false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "";
                        if (Helper.IsGridDataExistsInTempDataSONumber(full.SONumber, TempChkDataBaps)) {
                            strReturn += "<label id='" + full.TargetID + "_" + full.DepartmentCode + "'  class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.TargetID + "_" + full.DepartmentCode + "'><input type='checkbox' class='checkboxes' checked='checked'/><span></span></label>";
                        }
                        else {
                            strReturn += "<label id='" + full.TargetID + "_" + full.DepartmentCode + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.TargetID + "_" + full.DepartmentCode + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                        }
                        strReturn += "<button type='button' title='Edit' class='btn btn-xs green btEdit' onClick='TableHistory.EditTarget(" + full.TargetID + ", \"" + full.DepartmentCode + "\")'><i class='fa fa-edit'></i></button>";

                        return strReturn;
                    }
                },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CompanyInvoiceId" },
                { data: "CustomerID" },
                { data: "RegionName" },
                { data: "TargetMonth" },
                { data: "TargetYear" },
                { data: "TargetBaps" },
                { data: "TargetPower" },

                {
                    data: "StartInvoiceDate", render: function (data, type, full) {
                        if (data != null) {
                            return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data, type, full) {
                        if (data != null) {
                            return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "AmountIDR", render: function (data, type, full) {
                        if (data != null) {
                            return (data).format(0);
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "AmountUSD", render: function (data, type, full) {
                        if (data != null) {
                            return (data).format(0);
                        } else {
                            return "";
                        }
                    }
                },
                { data: "DepartmentType" },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({
                    target: ".panelSearchResult", boxed: true
                });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }

                l.stop();
                App.unblockUI('.panelSearchResult');
                if (AllDataHsIds.length > 0) {
                    var item;
                    for (var i = 0; i < AllDataHsIds.length; i++) {
                        item = AllDataHsIds[i];
                        $("#" + item + ' .checkboxes').addClass("active");
                        $("#" + item + ' .checkboxes').prop("checked", true);
                    }
                }
            },
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline" style="left:-2px;margin-right:-4px;">' +
                    '<input type="checkbox" name="SelectAll" class="group-checkable chkAll-newHs" data-set="#tblHistoryAddSite .checkboxes" />' +
                    '<span></span> ' +
                    '</label>'+
                    '<button id="btDeleteAllHistory" class="btn btn-xs red btnDeleteAllHs" onClick="TableHistory.DeleteTarget()"><i class="fa fa-trash"></i></button>';
                var th = $("th.select-all-hs").html(checkbox);
                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-hs").unbind().on("change", ".group-checkable.chkAll-newHs", function (e) {
                    var set = $(".panelSearchResult .checkboxes");
                    var checked = jQuery(this).is(":checked");
                    jQuery(set).each(function () {
                        label = $(this).parent();
                        var id = label.attr("id");
                        if (label.attr("style") != "display:none") {
                            /* Replace the following code with the code to select all checkboxes in all pages */
                            if (checked) {
                                $(this).prop("checked", true);
                                $(this).parents('tr').addClass('active');

                                $("#" + id).addClass("active");
                                $("#" + id).prop("checked", true);
                            } else {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");

                                $("#" + id).removeClass("active");
                                $("#" + id).prop("checked", false);
                            }
                        }
                    });
                    if (checked) {
                        AllDataHsIds = [];
                        App.blockUI({
                            target: ".panelSearchResult", boxed: true
                        });
                        AllDataHsIds = TableHistory.GetListId();
                        App.unblockUI('.panelSearchResult');

                    }
                    else
                        AllDataHsIds = [];
                });
            }
        });
    },
    Reset: function () {
        $("#slMonthTargetHs").val("").trigger('change');
        $("#slReconType").val("").trigger('change');
        $("#slPWRType").val("").trigger('change');
        $("#slDepartmentName").val("").trigger('change');
        $("#slCompanyHs").val("").trigger('change');
        $("#slCustomerHs").val("").trigger('change');

    },
    EditTarget: function (targetID, department) {
        var formHtml = $("script#mdlFormInputTargetHtml").html();
        $("#mdlFormInputTargetHistoryContainer").html(formHtml);
        if (formHtml.includes("mdlFormHistoryInputTarget")) {
            $('#mdlFormHistoryInputTarget').modal('show');
            App.blockUI({
                target: "#mdlFormHistoryInputTarget .modal-dialog", boxed: true
            });
            var url = (department.includes('New BAPS')) ? "/api/ApiInputTarget/GetTargetNewBapsByID" : "/api/ApiInputTarget/GetTargetRecurringByID";
            $.ajax({
                url: url,
                type: "post",
                dataType: "json",
                data: { TargetID: targetID },
                cache: false,
            }).done(function (data, textStatus, jqXHR) {
                Control.BindingSelectYearTarget($('#TargetYear'));
                Control.BindingSelectMonthTarget($('#TargetMonth'));
                $("#StartInvoiceDate, #EndInvoiceDate").datepicker({
                    format: "dd M yyyy",
                    clearBtn: true
                });
                Object.keys(data).forEach(key => {
                    $(`#formHistoryInputTarget label[data-value-for="${key}"]`).html(data[key]);
                    if (key == "StartInvoiceDate") {
                        var val = (data[key] != null) ? moment(data[key]).format('DD MMM YYYY') : null;
                        $(`#formHistoryInputTarget input[name="${key}"]`).val(val).trigger('changeDate');
                    } else if (key == "EndInvoiceDate") {
                        var val = (data[key] != null) ? moment(data[key]).format('DD MMM YYYY') : null;
                        $(`#formHistoryInputTarget input[name="${key}"]`).val(val).trigger('changeDate');
                    } else {
                        $(`#formHistoryInputTarget input[name="${key}"]`).val(data[key]).trigger('change');
                        $(`#formHistoryInputTarget select[name="${key}"]`).val(data[key]).trigger('change');
                    }
                })
                if (!(department.includes('New BAPS'))) {
                    //disabled by mas ardi Request
                    ///$("#StartInvoiceDate, #EndInvoiceDate, #AmountIDR").prop('disabled', true);
                }
                $('#mdlFormHistoryInputTarget #formHistoryInputTarget').show();

                App.unblockUI('#mdlFormHistoryInputTarget .modal-dialog');
                $(".btCancelFormHistory").click(function (e) {
                    e.preventDefault()
                    $('#mdlFormHistoryInputTarget').modal('hide');
                })
                $("#formHistoryInputTarget").submit(function (e) {
                    e.preventDefault();
                    var url = (department.includes('New BAPS')) ? "/api/ApiInputTarget/UpdateTargetNewBaps" : "/api/ApiInputTarget/UpdateTargetRecurring";
                    var l = Ladda.create(document.querySelector("#btSubmitFormHistory"));
                    l.start();

                    $.ajax({
                        url: url,
                        type: "post",
                        dataType: "json",
                        data: $("#formHistoryInputTarget").serializeObject(),
                        cache: false,
                        beforeSend: function (xhr) {
                        }
                    }).done(function (data, textStatus, jqXHR) {
                        if (Common.CheckError.List(data)) {
                            if (data.ErrorType != 0) {
                                Common.Alert.Warning(data.ErrorMessage);
                            } else {
                                Common.Alert.Success("Data has been saved!")
                                TableHistory.SearchHistory();
                                $('#mdlFormHistoryInputTarget').modal('hide');

                            }
                        }
                        else {
                            Common.Alert.Error(data.ErrorMessage);
                        }
                        l.stop()
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        Common.Alert.Error(errorThrown)
                        l.stop()
                    })
                });
            }).fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
            })
        }
    },
    DeleteTarget: function () {
        swal({
            title: "Warning",
            text: "Are You Sure Delete " + AllDataHsIds.length + "Row(s) data ?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            showConfirmButton: true,
            confirmButtonText: 'Delete',
            cancelButtonText: 'Cancel',
            dangerMode: true,
        }, function (isConfirm) {
            if (isConfirm) {
                var l = Ladda.create(document.querySelector("#btDeleteAllHistory"));
                l.start();
                App.blockUI({
                    target: ".panelSearchResult", boxed: true
                });
                var url = "/api/ApiInputTarget/DeleteHistoryInputTarget";
                $.ajax({
                    url: url,
                    type: "post",
                    dataType: "json",
                    data: { ListStrDeleteHistory: AllDataHsIds },
                    cache: false,
                    beforeSend: function (xhr) {
                    }
                }).done(function (data, textStatus, jqXHR) {
                    if (typeof data.ErrorMessage != 'undefined' && data.ErrorMessage.length > 0) {
                        Common.Alert.Error(data.ErrorMessage);
                        App.unblockUI('.panelSearchResult');
                        AllDataHsIds = [];
                        TableHistory.SearchHistory();
                    } else {
                        Common.Alert.Success("Data has been deleted!")
                        App.unblockUI('.panelSearchResult');
                        AllDataHsIds = [];
                        TableHistory.SearchHistory();
                    }
                    l.stop()
                })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        Common.Alert.Error(errorThrown)
                        App.unblockUI('.panelSearchResult');
                        l.stop()

                    })
            }
        });
    },
    Export: function () {

        var paramObject = TableHistory.GetParam();
        var param = $.param(paramObject);
        window.location.href = "/DashboardTSEL/ListDashboardTargetHistory/Export?" + param;
    },
    GetParam: function () {
        fsCompanyInvoice = $("#slCompanyInvoice").val() == null ? "" : $("#slCompanyInvoice").val();
        fsCustomerInvoice = $("#slCustomerInvoice").val() == null ? "" : $("#slCustomerInvoice").val();
        fsSection = $("#slSection").val() == null ? "" : $("#slSection").val();
        fsSOW = $("#slSow").val() == null ? "" : $("#slSow").val();
        fsBillingYear = $("#slBillingYear").val() == null ? "" : $("#slBillingYear").val();
        fsRegional = $("#slRegional").val() == null ? "" : $("#slRegional").val();
        fsProvince = $("#slProvince").val() == null ? "" : $("#slProvince").val();
        fsTenantType = $("#slTenantType").val() == null ? "" : $("#slTenantType").val();
        fsMonthTargetHistory = $("#slYearTargetHs").val() == null ? "" : $("#slYearTargetHs").val();
        fsYearTargetHistory = $("#slMonthTargetHs").val() == null ? "" : $("#slMonthTargetHs").val();
        fsReconcileType = $("#slReconType").val() == null ? "" : $("#slReconType").val();
        fsPowerType = $("#slPWRType").val() == null ? "" : $("#slPWRType").val();
        fsDepartmentCode = $("#slDepartmentName").val() == null ? "" : $("#slDepartmentName").val();

        fsCompanyHistory = $("#slCompanyHs").val() == null ? "" : $("#slCompanyHs").val();
        fsCustomerHistory = $("#slCustomerHs").val() == null ? "" : $("#slCustomerHs").val();

        var params = {
            strCompanyInvoice: fsCompanyInvoice,
            strCustomerInvoice: fsCustomerInvoice,
            strSection: fsSection,
            strSOW: fsSOW,
            strBillingYear: fsBillingYear,
            strRegional: fsRegional,
            strProvince: fsProvince,
            strTenantType: fsTenantType,
            strYearTargetHistory: fsMonthTargetHistory,
            strMonthTargetHistory: fsYearTargetHistory,
            slReconType: fsReconcileType,
            slPWRType: fsPowerType,
            BapsType: fsReconcileType,
            PowerType: fsPowerType,
            DepartmentCode: fsDepartmentCode,

            strCompanyInvoice: fsCompanyHistory,
            strCustomerInvoice: fsCustomerHistory,


            strSONumberMultiple: $("#slSONumberMultiple").val()

        }
        return params;
    },
    GetListId: function () {
    var AjaxData = [];
    App.blockUI({
        target: ".panelSearchResult", boxed: true
    });
    try {
        var params = TableHistory.GetParam();
        $.ajax({
            url: "/api/DashboardTSEL/GetListHistoryTargetIds",
            type: "POST",
            dataType: "json",
            async: false,
            data: params,
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            AjaxData = data;
            App.unblockUI('.panelSearchResult');
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            App.unblockUI('.panelSearchResult');
        });
    } catch (err) {
        Common.Alert.Error(err)
        App.unblockUI('.panelSearchResult');
    }
    return AjaxData;
},
}

//GRID TABLE NEW BAPS
var TableNewBaps = {
    Init: function () {
        $(".panelSearchZero").hide();
        $("#formHistory").hide();
        $("#pnlHistoryNewBaps").hide();

        var tblSummaryData = $('#tblSummaryDataBaps').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $("#tblSummaryDataBaps tbody").on("click", "a.btdetail", function (e) {
            e.preventdefault();
            var table = $('#tblSummaryDataBaps').DataTable();
            var data = table.row($(this).parents("tr")).data();

        });

        $(window).resize(function () {
            $("#tblSummaryDataBaps").DataTable().columns.adjust().draw();
        });
        $("#btAddSiteNewBaps").unbind().click(function () {
            //var l = Ladda.create(document.querySelector("#btSearch"))
            $('#slReconcileTypeBaps').val($('#slFilterBapsTypeBaps').val()).trigger("change");
            $('#slTypeTowerBaps').val($('#slFilterPowerTypeBaps').val()).trigger("change");
            $('#slFilterBapsTypeBaps').prop("disabled", false);
            $('#slFilterPowerTypeBaps').prop("disabled", false);
            if (TempChkDataBaps.length > 0) {
                AllDataNewBapsId = (TempChkDataBaps || []).concat();
                $("#pnlHistoryNewBaps").show();
                TableNewBaps.Search();
                TableNewBaps.AddListSearch();
            }
            else
                Common.Alert.Warning("Please Select One or More Data")

        });
        $("#btResetBaps").unbind().click(function () {
            TableNewBaps.Reset();
        });
        $("#btnSubmitBaps").unbind().click(function () {
            TableNewBaps.Submit();
        });

    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btnSearchBaps"))
        l.start();

        var SONumber = $("#schSONumberBaps").val();
        var SiteID = $("#schSiteIDBaps").val();
        var SiteName = $("#schSiteNameBaps").val();
        var CustomerSiteID = $("#schCustomerSiteIDBaps").val();
        var CustomerSiteName = $("#schCustomerSiteNameBaps").val();
        var CustomerID = $("#schCustomerIDBaps").val();
        var RegionName = $("#schRegionNameBaps").val();
        var BapsType = $('#slFilterBapsTypeBaps').val();
        var PowerType = $('#slFilterPowerTypeBaps').val();


        var Province = $("#slProvinceBaps").val();
        var sDate = $("#slFilterStartDateBaps").val() != '' ? moment('1 ' + $("#slFilterStartDateBaps").val(), 'D MMM YYYY').format('YYYY/MM/DD') : '';
        var _eDate = '';
        if ($("#slFilterEndDateBaps").val() != null) {
            var eDate = moment('1 ' + $("#slFilterEndDateBaps").val(), 'D MMM YYYY').format('YYYY/MM/DD');
            var dayInMonth = moment(eDate, "YYYY/MM/DD").daysInMonth()
            _eDate = moment(eDate, 'YYYY/MM/DD').set({ 'D': dayInMonth }).format('YYYY/MM/DD');
        }

        fsCompanyInvoice = $("#slCompanyInvoiceBaps").val() == null ? "" : $("#slCompanyInvoiceBaps").val();
        fsCustomerInvoice = $("#slCustomerInvoiceBaps").val() == null ? "" : $("#slCustomerInvoiceBaps").val();
        fsSection = $("#slSectionBaps").val() == null ? "" : $("#slSectionBaps").val();
        fsSOW = $("#slSowBaps").val() == null ? "" : $("#slSowBaps").val();
        fsBillingYear = $("#slBillingYearBaps").val() == null ? "" : $("#slBillingYearBaps").val();
        fsRegional = $("#slRegionalBaps").val() == null ? "" : $("#slRegionalBaps").val();
        fsProvince = $("#slProvinceBaps").val() == null ? "" : $("#slProvinceBaps").val();
        fsTenantType = $("#slTenantTypeBaps").val() == null ? "" : $("#slTenantTypeBaps").val();

        var params = {
            strCompanyInvoice: fsCompanyInvoice,
            strCustomerInvoice: fsCustomerInvoice,
            strSection: fsSection,
            strSOW: fsSOW,
            strRegional: fsRegional,
            strProvince: fsProvince,
            strTenantType: fsTenantType,
            isReceive: 0,
            schSONumber: SONumber,
            schSiteID: SiteID,
            schSiteName: SiteName,
            schCustomerSiteID: CustomerSiteID,
            schCustomerSiteName: CustomerSiteName,
            schCustomerID: CustomerID,
            schRegionName: RegionName,
            BapsType: BapsType,
            PowerType: PowerType,
            StartInvoiceDate: sDate,
            EndInvoiceDate: _eDate

        };

        var tblSummaryDataBaps = $("#tblSummaryDataBaps").DataTable({
            "deferRender": true,
            "processing": true,
            "serverSide": true,
            "bSortCellsTop": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/ApiInputTarget/NewBapsGrid",
                "type": "POST",
                "datatype": "json",
                "data": TableNewBaps.GetParam(),
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        TableNewBaps.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            //hide checkbox if data is checked and stored in TempChkBaps
                            if (Helper.IsGridDataExistsInTempDataSONumber(full.SONumber, TempChkDataBaps)) { //Data.RowSelected)) {
                            }
                            else {
                                strReturn += "<label id='" + full.SONumber + "_" + full.YearBill + "_" + full.MonthBill + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.MstBapsId + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CustomerID" },
                { data: "CompanyInvoiceId" },
                { data: "RegionName" },
                {
                    data: "StartInvoiceDate", render: function (data, type, full) {
                        if (data != null) {
                            return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data, type, full) {
                        if (data != null) {
                            return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "AmountIDR", render: function (data, type, full) {
                        if (data != null) {
                            return (data).format(0);
                        } else {
                            return "";
                        }
                    }
                },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({
                    target: ".panelSearchResult", boxed: true
                });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryDataBaps.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (AllDataNewBapsId.length > 0) {
                    var item;
                    for (var i = 0; i < AllDataNewBapsId.length; i++) {
                        item = AllDataNewBapsId[i].SONumber + '_' + AllDataNewBapsId[i].YearBill + '_' + AllDataNewBapsId[i].MonthBill;
                        $("." + item).addClass("active");
                    }
                }
            },
            "order": [],
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                $(row).addClass("." + data.MstBapsId + "_" + data.YearBill + "_" + data.MonthBill);
            },
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                    '<input type="checkbox" name="SelectAll" class="group-checkable chkAll-newBaps" data-set="#tblSummaryData .checkboxes" />' +
                    '<span></span> ' +
                    '</label>';
                var th = $("th.select-all-raw").html(checkbox);
                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-raw").unbind().on("change", ".group-checkable.chkAll-newBaps", function (e) {
                    var set = $(".panelSearchResult .checkboxes");
                    var checked = jQuery(this).is(":checked");
                    jQuery(set).each(function () {
                        label = $(this).parent();
                        var id = label.attr("id");
                        if (label.attr("style") != "display:none") {
                            /* Replace the following code with the code to select all checkboxes in all pages */
                            if (checked) {
                                $(this).prop("checked", true);
                                $(this).parents('tr').addClass('active');

                                $("." + id).addClass("active");
                                $("." + id).prop("checked", true);
                            } else {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");

                                $("." + id).removeClass("active");
                                $("." + id).prop("checked", false);
                            }
                        }
                    });
                    if (checked) {
                        TempChkDataBaps = [];
                        App.blockUI({
                            target: ".panelSearchResult", boxed: true
                        });
                        TempChkDataBaps = TableNewBaps.GetListId();
                        App.unblockUI('.panelSearchResult');

                    }
                    else
                        TempChkDataBaps = (AllDataNewBapsId || []).concat();
                });
            }
        });
    },
    AddListSearch: function () {
        AllDataNewBapsId = (TempChkDataBaps || []).concat();
        $("#tblAddSiteBaps").DataTable({
            "serverSide": false,
            "filter": true,
            "destroy": true,
            "async": false,
            "data": AllDataNewBapsId,
            "columns": [{
                orderable: false,
                mRender: function (data, type, full) {
                    return "<i class='fa fa-remove btn bn-xs red deleteRow'></i>";
                }
            },
            { data: "SONumber" },
            { data: "SiteID" },
            { data: "SiteName" },
            { data: "CustomerSiteID" },
            { data: "CustomerSiteName" },
            { data: "CustomerID" },
            { data: "CompanyInvoiceId" },
            { data: "RegionName" },
                {
                    data: "StartInvoiceDate", render: function (data, type, full) {
                        if (data != null) {
                            return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data, type, full) {
                        if (data != null) {
                            return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "AmountIDR", render: function (data, type, full) {
                        if (data != null) {
                            return (data).format(0);
                        } else {
                            return "";
                        }
                    }
                },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
        });
        $("#tblAddSiteBaps").unbind();
        $("#tblAddSiteBaps").on("click", ".deleteRow", function (e) {
            var table = $("#tblAddSiteBaps").DataTable();
            var row = table.row($(this).parents('tr')).data();
            table.row($(this).parents('tr')).remove().draw(false);
            Helper.RemoveDataInTempDataSONumber(row.SONumber, AllDataNewBapsId);
            Helper.RemoveDataInTempDataSONumber(row.SONumber, TempChkDataBaps);
            TableNewBaps.Search();
            if (AllDataNewBapsId.length < 1) {
                $("#pnlHistoryNewBaps").hide();
                $('#slFilterBapsTypeBaps').prop("disabled", false);
                $('#slFilterPowerTypeBaps').prop("disabled", false);
            }
        });
    },
    Export: function () {

        var paramObject = TableNewBaps.GetParam();
        var param = $.param(paramObject);

        window.location.href = "/DashboardTSEL/ListDashboardNewBaps/Export?" + param;
    },
    Reset: function () {
        var dt = new Date();
        var currentyear = dt.getFullYear();
        $("#slCompanyInvoiceBaps").val("").trigger('change');
        $("#slSectionBaps").val("").trigger('change');
        $("#slSowBaps").val("").trigger('change');
        $("#slRegionalBaps").val("").trigger('change');
        $("#slProvinceBaps").val("").trigger('change');
        $("#slTenantTypeBaps").val("").trigger('change');
        $("#slFilterStipSiroBaps").val("").trigger('change');

        $("#slFilterBapsTypeBaps").val(null).trigger('change');
        $("#slFilterPowerTypeBaps").val(null).trigger('change');
        $("#slFilterStartDateBaps").val(null).trigger('changeDate');
        $("#slFilterEndDateBaps").val(null).trigger('changeDate');
        $("#slCustomerInvoiceBaps").val("").trigger('change');
        $("#tblSummaryDataBaps input").val("").trigger('change');

        TempChkDataBaps = [];
        AllDataNewBapsId = [];
        TableNewBaps.Init();
    },
    Submit: function () {
        var result = "";
        if ($('#slYearTargetBaps').val() == "" || $('#slYearTargetBaps').val() == null)
            result += " Year Target,";
        if ($('#slMonthTargetBaps').val() == "" || $('#slMonthTargetBaps').val() == null)
            result += " Month Target,";
        if ($('#slReconcileTypeBaps').val() == "" || $('#slReconcileTypeBaps').val() == null)
            result += " Reconcile Type,";

        if (result == "") {
            if (TempChkDataBaps.length > 0) {
                var requestData = [];
                var params = {
                    YearTarget: $('#slYearTargetBaps').val(),
                    MonthBill: $('#slMonthTargetBaps').val(),
                    BapsType: $('#slReconcileTypeBaps').val(),
                    PowerType: $('#slReconcileTypeBaps').val() == '5' ? "0000" : $('#slTypeTowerBaps').val(),
                    DepartmentCode: settingVariable.deptCodeNewBaps,
                    vwRABapsSiteList: TempChkDataBaps,
                }
                $.ajax({
                    url: "/api/DashboardTSEL/AddDataTargetNewBaps",
                    type: "post",
                    dataType: "json",
                    data: params,
                    cache: false,
                    beforeSend: function (xhr) {
                    }
                }).done(function (data, textStatus, jqXHR) {
                    if (Common.CheckError.List(data)) {
                        if (data.ErrorType != 0) {
                            Common.Alert.Warning(data.ErrorMessage);
                        } else {
                            Common.Alert.Success("Data has been saved!")
                            $("#pnlHistoryNewBaps").hide();
                            $("#slYearTargetBaps").val("").trigger('change');
                            $("#slMonthTargetBaps").val("").trigger('change');
                            $("#slReconcileTypeBaps").val("").trigger('change');
                            $("#slTypeTowerBaps").val("").trigger('change');
                            TempChkDataBaps = [];
                            AllDataNewBapsId = [];
                            TableNewBaps.Search();

                            $('#slFilterBapsTypeBaps').prop("disabled", false);
                            $('#slFilterPowerTypeBaps').prop("disabled", false);
                        }

                    }
                    else {
                        Common.Alert.Error(data.ErrorMessage);
                    }
                })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        Common.Alert.Error(errorThrown)
                        l2.stop();
                        l.stop()
                    })
            } else {
                Common.Alert.Warning("Request cannot be empty");
            }
        }
        else
            Common.Alert.Warning(result + " Is Mandatory");
    },
    GetListId: function () {
        var AjaxData = [];
        App.blockUI({
            target: ".panelSearchResult", boxed: true
        });
        try {
            var params = TableNewBaps.GetParam();
            $.ajax({
                url: "/api/ApiInputTarget/GetListNewBapsData",
                type: "POST",
                dataType: "json",
                async: false,
                data: params,
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                AjaxData = data;
                App.unblockUI('.panelSearchResult');
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                App.unblockUI('.panelSearchResult');
            });
        } catch (err) {
            Common.Alert.Error(err)
            App.unblockUI('.panelSearchResult');
        }
        return AjaxData;
    },
    GetParam: function () {
        var sDate = $("#slFilterStartDateBaps").val() != '' ? moment('1 ' + $("#slFilterStartDateBaps").val(), 'D MMM YYYY').format('YYYY/MM/DD') : '';
        var _eDate = '';
        if ($("#slFilterEndDateBaps").val() != '') {
            var eDate = moment('1 ' + $("#slFilterEndDateBaps").val(), 'D MMM YYYY').format('YYYY/MM/DD');
            var dayInMonth = moment(eDate, "YYYY/MM/DD").daysInMonth()
            _eDate = moment(eDate, 'YYYY/MM/DD').set({ 'D': dayInMonth }).format('YYYY/MM/DD');
        }
        return {
            strCompanyInvoice: $("#slCompanyInvoiceBaps").val(),
            strCustomerInvoice: $("#slCustomerInvoiceBaps").val(),
            strSection: $("#slSectionBaps").val(),
            strSOW: $("#slSowBaps").val(),
            strBillingYear: fsBillingYear,
            strRegional: $("#slRegionalBaps").val(),
            strProvince: $("#slProvinceBaps").val(),
            strTenantType: $("#slTenantTypeBaps").val(),
            schSONumber: $("#schSONumberBaps").val(),
            schSiteID: $("#schSiteIDBaps").val(),
            schSiteName: $("#schSiteNameBaps").val(),
            schCustomerSiteID: $("#schCustomerSiteIDBaps").val(),
            schCustomerSiteName: $("#schCustomerSiteNameBaps").val(),
            schCustomerID: $("#schCustomerSiteNameBaps").val(),
            schRegionName: $("#schRegionNameBaps").val(),
            schYearBill: $("#schYearBill").val(),
            PowerType: $('#slFilterPowerTypeBaps').val(),
            BapsType: $('#slFilterBapsTypeBaps').val(),
            ReconcileType: $('#slFilterBapsTypeBaps').val(),
            schStipSiro: $("#slFilterStipSiroBaps").val(),
            StartInvoiceDate: sDate,
            EndInvoiceDate: _eDate
        }
    }
}

//GRID TABLE REC. NEW PRODUCT & OTHERS
var TableNewProduct = {
    Init: function () {
        $(".panelSearchZero").hide();
        $("#formHistory").hide();
        $("#pnlHistoryProd").hide();

        var tblSummaryData = $('#tblSummaryDataProd').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $("#tblSummaryDataProd tbody").on("click", "a.btdetail", function (e) {
            e.preventdefault();
            var table = $('#tblSummaryDataProd').DataTable();
            var data = table.row($(this).parents("tr")).data();

        });

        $(window).resize(function () {
            $("#tblSummaryDataProd").DataTable().columns.adjust().draw();
        });
        $("#btAddSiteProd").unbind().click(function () {
            $('#slReconcileTypeProd').val($('#slFilterBapsTypeProd').val()).trigger("change");
            $('#slTypeTowerProd').val($('#slFilterPowerTypeProd').val()).trigger("change");
            $('#slFilterBapsTypeProd').prop("disabled", false);
            $('#slFilterPowerTypeProd').prop("disabled", false);
            if (TempChkDataProd.length > 0) {
                AllDataProdId = (TempChkDataProd || []).concat();
                $("#pnlHistoryProd").show();
                TableNewProduct.Search();
                TableNewProduct.AddListSearch();
            }
            else
                Common.Alert.Warning("Please Select One or More Data")



        });
        //SUBMIT EVENT
        $("#btResetProd").unbind().click(function () {
            TableNewProduct.Reset();
        });

        $("#btnSubmitProd").unbind().click(function () {
            TableNewProduct.Submit();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btnSearchProd"))
        l.start();

        var SONumber = $("#schSONumberProd").val();
        var SiteID = $("#schSiteIDProd").val();
        var SiteName = $("#schSiteNameProd").val();
        var CustomerSiteID = $("#schCustomerSiteIDProd").val();
        var CustomerSiteName = $("#schCustomerSiteNameProd").val();
        var CustomerID = $("#schCustomerIDProd").val();
        var RegionName = $("#schRegionNameProd").val();
        var BapsType = $('#slFilterBapsTypeProd').val();
        var PowerType = $('#slFilterPowerTypeProd').val();
        var Province = $("#slProvinceProd").val();

        fsCompanyInvoice = $("#slCompanyInvoiceProd").val() == null ? "" : $("#slCompanyInvoiceProd").val();
        fsCustomerInvoice = $("#slCustomerInvoiceProd").val() == null ? "" : $("#slCustomerInvoiceProd").val();
        fsSection = $("#slSectionProd").val() == null ? "" : $("#slSectionProd").val();
        fsSOW = $("#slSowProd").val() == null ? "" : $("#slSowProd").val();
        fsRegional = $("#slRegionalProd").val() == null ? "" : $("#slRegionalProd").val();
        fsProvince = $("#slProvinceProd").val() == null ? "" : $("#slProvinceProd").val();
        fsTenantType = $("#slTenantTypeProd").val() == null ? "" : $("#slTenantTypeProd").val();
        var sDate = $("#slFilterStartDateProd").val() != '' ? moment('1 ' + $("#slFilterStartDateProd").val(), 'D MMM YYYY').format('YYYY/MM/DD') : '';
        var _eDate = '';
        if ($("#slFilterEndDateProd").val() != null) {
            var eDate = moment('1 ' + $("#slFilterEndDateProd").val(), 'D MMM YYYY').format('YYYY/MM/DD');
            var dayInMonth = moment(eDate, "YYYY/MM/DD").daysInMonth()
            _eDate = moment(eDate, 'YYYY/MM/DD').set({ 'D': dayInMonth }).format('YYYY/MM/DD');
        }
        var params = {
            strCompanyInvoice: fsCompanyInvoice,
            strCustomerInvoice: fsCustomerInvoice,
            strSection: fsSection,
            strRegional: fsRegional,
            strProvince: fsProvince,
            strTenantType: fsTenantType,
            isReceive: 0,
            schSONumber: SONumber,
            schSiteID: SiteID,
            schSiteName: SiteName,
            schCustomerSiteID: CustomerSiteID,
            schCustomerSiteName: CustomerSiteName,
            schCustomerID: CustomerID,
            schRegionName: RegionName,
            BapsType: BapsType,
            PowerType: PowerType,
            StartInvoiceDate: sDate,
            EndInvoiceDate: _eDate

        };

        var tblSummaryData = $("#tblSummaryDataProd").DataTable({
            "deferRender": true,
            "processing": true,
            "serverSide": true,
            "bSortCellsTop": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/ApiInputTarget/NewProductGrid",
                "type": "POST",
                "datatype": "json",
                "data": TableNewProduct.GetParam(),
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        TableNewProduct.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Helper.IsGridDataExistsInTempData(full.MstBapsId, full.StartInvoiceDate, TempChkDataProd)) { //Data.RowSelected)) {
                            }
                            else {
                                strReturn += "<label id='" + full.MstBapsId + "_" + full.YearBill + "_" + full.MonthBill + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.MstBapsId + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CustomerID" },
                { data: "CompanyInvoiceId" },
                { data: "RegionName" },
                {
                    data: "StartInvoiceDate", render: function (data, type, full) {
                        if (data != null) {
                            return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data, type, full) {
                        if (data != null) {
                            return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "AmountIDR", render: function (data, type, full) {
                        if (data != null) {
                            return (data).format(0);
                        } else {
                            return "";
                        }
                    }
                },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({
                    target: ".panelSearchResult", boxed: true
                });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (AllDataProdId.length > 0) {
                    var item;
                    for (var i = 0; i < AllDataProdId.length; i++) {
                        item = AllDataProdId[i].MstBapsId + '_' + AllDataProdId[i].YearBill + '_' + AllDataProdId[i].MonthBill;
                        $("." + item).addClass("active");
                    }
                }
            },
            "order": [],
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                $(row).addClass("." + data.MstBapsId + "_" + data.YearBill + "_" + data.MonthBill);
            },
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                    '<input type="checkbox" name="SelectAll" class="group-checkable chkAll-newProd" data-set="#tblSummaryData .checkboxes" />' +
                    '<span></span> ' +
                    '</label>';
                var th = $("th.select-all-raw").html(checkbox);
                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-raw").unbind().on("change", ".group-checkable.chkAll-newProd", function (e) {
                    var set = $(".panelSearchResult .checkboxes");
                    var checked = jQuery(this).is(":checked");
                    jQuery(set).each(function () {
                        label = $(this).parent();
                        var id = label.attr("id");
                        if (label.attr("style") != "display:none") {
                            /* Replace the following code with the code to select all checkboxes in all pages */
                            if (checked) {
                                $(this).prop("checked", true);
                                $(this).parents('tr').addClass('active');
                                $(this).trigger("change");

                                $(".Row" + id).addClass("active");
                                $("." + id).prop("checked", true);
                                $("." + id).trigger("change");
                            } else {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");
                                $(this).trigger("change");

                                $(".Row" + id).removeClass("active");
                                $("." + id).prop("checked", false);
                                $("." + id).trigger("change");
                            }
                        }
                    });
                    if (checked) {
                        TempChkDataProd = []
                        TempChkDataProd = TableNewProduct.GetListId();
                    }
                    else
                        TempChkDataProd = (AllDataProdId || []).concat();
                });
            }
        });
    },
    AddListSearch: function () {
        AllDataProdId = (TempChkDataProd || []).concat();

        $("#tblAddSiteProd").DataTable({
            "serverSide": false,
            "filter": true,
            "destroy": true,
            "async": false,
            "data": AllDataProdId,
            "columns": [{
                orderable: false,
                mRender: function (data, type, full) {
                    return "<i class='fa fa-remove btn bn-xs red deleteRow'></i>";
                }
            },
            { data: "SONumber" },
            { data: "SiteID" },
            { data: "SiteName" },
            { data: "CustomerSiteID" },
            { data: "CustomerSiteName" },
            { data: "CustomerID" },
                { data: "CompanyInvoiceId" },
            { data: "RegionName" },
                {
                    data: "StartInvoiceDate", render: function (data, type, full) {
                        if (data != null) {
                            return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data, type, full) {
                        if (data != null) {
                            return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: "AmountIDR", render: function (data, type, full) {
                        if (data != null) {
                            return (data).format(0);
                        } else {
                            return "";
                        }
                    }
                },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
        });
        $("#tblAddSiteProd").unbind();
        $("#tblAddSiteProd").on("click", ".deleteRow", function (e) {
            var table = $("#tblAddSiteProd").DataTable();
            var row = table.row($(this).parents('tr')).data();
            table.row($(this).parents('tr')).remove().draw(false);
            Helper.RemoveDataInTempData(row.MstBapsId, row.StartInvoiceDate, TempChkDataProd);
            Helper.RemoveDataInTempData(row.MstBapsId, row.StartInvoiceDate, AllDataProdId);
            TableNewProduct.Search();
            if (AllDataProdId.length < 1) {
                $("#pnlHistoryProd").hide();
                $('#slFilterBapsType').prop("disabled", false);
                $('#slFilterPowerType').prop("disabled", false);
            }
        });
    },
    Export: function () {
        var paramObject = TableNewProduct.GetParam();
        var param = $.param(paramObject);

        window.location.href = "/DashboardTSEL/ListDashboardProduct/Export?" + param;
    },
    Reset: function () {
        var dt = new Date();
        var currentyear = dt.getFullYear();
        $("#slCompanyInvoiceProd").val("").trigger('change');
        $("#slSectionProd").val("").trigger('change');
        $("#slSowProd").val("").trigger('change');
        $("#slRegionalProd").val("").trigger('change');
        $("#slProvinceProd").val("").trigger('change');
        $("#slTenantTypeProd").val("").trigger('change');
        $("#slFilterBapsTypeProd").val(null).trigger('change');
        $("#slFilterPowerTypeProd").val(null).trigger('change');
        $("#slFilterStartDateProd").val(null).trigger('changeDate');
        $("#slFilterEndDateProd").val(null).trigger('changeDate');
        $("#slCustomerInvoiceProd").val("").trigger('change');
        $("#tblSummaryDataProd input").val("").trigger('change');

        TempChkDataProd = [];
        AllDataProdId = [];
        TableNewProduct.Init();
    },
    Submit: function () {
        var result = "";
        if ($('#slYearTargetProd').val() == "" || $('#slYearTargetProd').val() == null)
            result += " Year Target,";
        if ($('#slMonthTargetProd').val() == "" || $('#slMonthTargetProd').val() == null)
            result += " Month Target,";
        if ($('#slReconcileTypeProd').val() == "" || $('#slReconcileTypeProd').val() == null)
            result += " Reconcile Type,";

        if (result == "") {
            if (AllDataProdId.length > 0) {
                var requestData = [];
                var params = {
                    YearTarget: $('#slYearTargetProd').val(),
                    MonthBill: $('#slMonthTargetProd').val(),
                    BapsType: $('#slReconcileTypeProd').val(),
                    PowerType: $('#slReconcileTypeProd').val() == '5' ? "0000" : $('#slTypeTowerProd').val(),
                    DepartmentCode: settingVariable.deptCodeNewProduct,
                    vwRABapsSiteList: AllDataProdId,
                }
                $.ajax({
                    url: "/api/DashboardTSEL/AddDataDashboardTSEL",
                    type: "post",
                    dataType: "json",
                    contentType: "application/json",
                    data: JSON.stringify(params),
                    cache: false,
                    beforeSend: function (xhr) {
                    }
                }).done(function (data, textStatus, jqXHR) {
                    if (Common.CheckError.List(data)) {
                        if (data.ErrorType != 0) {
                            Common.Alert.Warning(data.ErrorMessage);
                        } else {
                            Common.Alert.Success("Data has been saved!")
                            $("#pnlHistoryProd").hide();
                            $("#slYearTargetProd").val("").trigger('change');
                            $("#slMonthTargetProd").val("").trigger('change');
                            $("#slReconcileTypeProd").val("").trigger('change');
                            $("#slTypeTowerProd").val("").trigger('change');
                            AllDataProdId = [];
                            TempChkDataProd = [];
                            TableNewProduct.Search();

                            $('#slFilterBapsTypeProd').prop("disabled", false);
                            $('#slFilterPowerTypeProd').prop("disabled", false);
                        }

                    }
                    else {
                        Common.Alert.Error(data.ErrorMessage);
                    }
                })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        Common.Alert.Error(errorThrown)
                        l2.stop();
                        l.stop()
                    })
            } else {
                Common.Alert.Warning("Request cannot be empty");
            }
        }
        else
            Common.Alert.Warning(result + " Is Mandatory");
    },
    GetListId: function () {
        var AjaxData = [];
        App.blockUI({
            target: ".panelSearchResult", boxed: true
        });
        try {
            var params = TableNewProduct.GetParam();
            $.ajax({
                url: "/api/ApiInputTarget/GetListNewProductData",
                type: "POST",
                dataType: "json",
                async: false,
                data: params,
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                AjaxData = data;
                App.unblockUI('.panelSearchResult');
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                App.unblockUI('.panelSearchResult');
            });
        } catch (err) {
            Common.Alert.Error(err)
            App.unblockUI('.panelSearchResult');
        }
        return AjaxData;
    },
    GetParam: function () {
        var sDate = $("#slFilterStartDateProd").val() != '' ? moment('1 ' + $("#slFilterStartDateProd").val(), 'D MMM YYYY').format('YYYY/MM/DD') : '';
        var _eDate = '';
        if ($("#slFilterEndDateProd").val() != '') {
            var eDate = moment('1 ' + $("#slFilterEndDateProd").val(), 'D MMM YYYY').format('YYYY/MM/DD');
            var dayInMonth = moment(eDate, "YYYY/MM/DD").daysInMonth()
            _eDate = moment(eDate, 'YYYY/MM/DD').set({ 'D': dayInMonth }).format('YYYY/MM/DD');
        }
        return {
            strCompanyInvoice: $("#slCompanyInvoiceProd").val(),
            strCustomerInvoice: $("#slCustomerInvoiceProd").val(),
            strSection: $("#slSectionProd").val(),
            strSOW: $("#slSowProd").val(),
            strBillingYear: fsBillingYear,
            strRegional: $("#slRegionalProd").val(),
            strProvince: $("#slProvinceProd").val(),
            strTenantType: $("#slTenantTypeProd").val(),
            schSONumber: $("#schSONumberProd").val(),
            schSiteID: $("#schSiteIDProd").val(),
            schSiteName: $("#schSiteNameProd").val(),
            schCustomerSiteID: $("#schCustomerSiteIDProd").val(),
            schCustomerSiteName: $("#schCustomerSiteNameProd").val(),
            schCustomerID: $("#schCustomerIDProd").val(),
            schRegionName: $("#schRegionNameProd").val(),
            schYearBill: $("#schYearBill").val(),
            PowerType: $('#slFilterPowerTypeProd').val(),
            BapsType: $('#slFilterBapsTypeProd').val(),
            ReconcileType: $('#slFilterBapsTypeProd').val(),
            StartInvoiceDate: sDate,
            EndInvoiceDate: _eDate
        }
    }
}

var Control = {
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $("#slCompanyInvoice").html("<option></option>")
                $("#slCompanyInvoiceNonTsel").html("<option></option>")
                $("#slCompanyInvoiceBaps").html("<option></option>")
                $("#slCompanyInvoiceProd").html("<option></option>")
                $("#slCompanyHs").html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $("#slCompanyInvoice").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                        $("#slCompanyInvoiceNonTsel").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                        $("#slCompanyInvoiceBaps").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                        $("#slCompanyInvoiceProd").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                        $("#slCompanyHs").append("<option value='" + item.CompanyId + "'>" + item.CompanyId + " - " + item.Company + "</option>");
                    })
                }

                $("#slCompanyInvoice").select2({ placeholder: "Select Company Name", width: null });
                $("#slCompanyInvoiceNonTsel").select2({ placeholder: "Select Company Name", width: null });
                $("#slCompanyInvoiceBaps").select2({ placeholder: "Select Company Name", width: null });
                $("#slCompanyInvoiceProd").select2({ placeholder: "Select Company Name", width: null });
                $("#slCompanyHs").select2({ placeholder: "Select Company Name", width: null });

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectRegional: function () {
        $.ajax({
            url: "/api/MstDataSource/Regional",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $("#slRegional").html("<option></option>")
                $("#slRegionalNonTsel").html("<option></option>")
                $("#slRegionalBaps").html("<option></option>")
                $("#slRegionalProd").html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $("#slRegional").append("<option value='" + item.RegionalId + "'>" + item.RegionalName + "</option>");
                        $("#slRegionalNonTsel").append("<option value='" + item.RegionalId + "'>" + item.RegionalName + "</option>");
                        $("#slRegionalBaps").append("<option value='" + item.RegionalId + "'>" + item.RegionalName + "</option>");
                        $("#slRegionalProd").append("<option value='" + item.RegionalId + "'>" + item.RegionalName + "</option>");
                    })
                }

                $("#slRegional").select2({ placeholder: "Select Regional", width: null });
                $("#slRegionalNonTsel").select2({ placeholder: "Select Regional", width: null });
                $("#slRegionalBaps").select2({ placeholder: "Select Regional", width: null });
                $("#slRegionalProd").select2({ placeholder: "Select Regional", width: null });

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectProvince: function () {
        $.ajax({
            url: "/api/MstDataSource/TbgSysProvince",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $("#slProvince").html("<option></option>")
                $("#slProvinceNonTsel").html("<option></option>")
                $("#slProvinceBaps").html("<option></option>")
                $("#slProvinceProd").html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $("#slProvince").append("<option value='" + item.ProvinceID + "'>" + item.ProvinceName + "</option>");
                        $("#slProvinceNonTsel").append("<option value='" + item.ProvinceID + "'>" + item.ProvinceName + "</option>");
                        $("#slProvinceBaps").append("<option value='" + item.ProvinceID + "'>" + item.ProvinceName + "</option>");
                        $("#slProvinceProd").append("<option value='" + item.ProvinceID + "'>" + item.ProvinceName + "</option>");
                    })
                }

                $("#slProvince").select2({ placeholder: "Select Province", width: null });
                $("#slProvinceNonTsel").select2({ placeholder: "Select Province", width: null });
                $("#slProvinceBaps").select2({ placeholder: "Select Province", width: null });
                $("#slProvinceProd").select2({ placeholder: "Select Province", width: null });

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectTenant: function () {
        var id = "#slTenantType"
        $.ajax({
            url: "/api/MstDataSource/TenantType",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")
                $("#slTenantTypeNonTsel").html("<option></option>")
                $("#slTenantTypeBaps").html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                        $("#slTenantTypeNonTsel").append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                        $("#slTenantTypeBaps").append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Tenant Type", width: null });
                $("#slTenantTypeNonTsel").select2({ placeholder: "Select Tenant Type", width: null });
                $("#slTenantTypeBaps").select2({ placeholder: "Select Tenant Type", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectTenantInNewProduct: function (eleID, parentEleID) {
        //add event on select to parent dropdown (Customer Invoice dropdown)
        //$(eleID).select2({ placeholder: "Select Tenant Type", width: null });
        $.ajax({
            url: "/api/MstDataSource/TenantTypeByOperator",
            type: "GET",
            data: { operatorID: '' }
        })
                .done(function (data, textStatus, jqXHR) {
                    $(eleID).html("<option></option>")
                    if (Common.CheckError.List(data)) {
                        $.each(data, function (i, item) {
                            $(eleID).append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                        })
                    }
                    $(eleID).select2({ placeholder: "Select Tenant Type", width: null });
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    Common.Alert.Error(errorThrown);
                });
        $(parentEleID).on("select2:select", function (event) {
            $(eleID).val('');
            $.ajax({
                url: "/api/MstDataSource/TenantTypeByOperator",
                type: "GET",
                data: { operatorID: $(parentEleID).val() }
            })
            .done(function (data, textStatus, jqXHR) {
                $(eleID).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(eleID).append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                    })
                }
                $(eleID).select2({ placeholder: "Select Tenant Type", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
        })




    },

    BindingSelectMonthYear: function (ele) {
        $(ele).datepicker({
            format: "M yyyy",
            startView: "months",
            minViewMode: "months",
            clearBtn: true
        });
    },

    BindingSelectSiro: function (ele) {
        $(ele).html("<option></option>")
        $(ele).append("<option value='0'>0</option>");
        $(ele).append("<option value='1'>1</option>");
        $(ele).append("<option value='2'>2</option>");
        $(ele).append("<option value='3'>3</option>");
        $(ele).append("<option value='4'>4</option>");
        $(ele).append("<option value='5'>5</option>");
        $(ele).select2({ placeholder: "Select Siro", width: null });
    },

    BindingSelectCustomer: function () {
        var id = "#slCustomerInvoice"
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")
                $("#slCustomerInvoiceNonTsel").html("<option></option>")
                $("#slCustomerInvoiceBaps").html("<option></option>")
                $("#slCustomerInvoiceProd").html("<option></option>")
                $("#slCustomerHs").html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                        $("#slCustomerInvoiceBaps").append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                        $("#slCustomerInvoiceProd").append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                        $("#slCustomerHs").append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");

                        //TSEL value is not included in Non TSEL select option
                        if (!(item.OperatorId == 'TSEL' || item.OperatorId == 'TSEL  ')) {
                            $("#slCustomerInvoiceNonTsel").append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                        }
                    })
                }
                $(id).select2({ placeholder: "Select Customer", width: null });
                $("#slCustomerInvoiceNonTsel").select2({ placeholder: "Select Customer", width: null });
                $("#slCustomerInvoiceBaps").select2({ placeholder: "Select Customer", width: null });
                $("#slCustomerInvoiceProd").select2({ placeholder: "Select Customer", width: null });
                $("#slCustomerHs").select2({ placeholder: "Select Customer", width: null });
                $("#slCustomerInvoice").val("TSEL  ").trigger('change');
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });


    },
    //new
    BindingSelectSection: function () {
        var id = "#slSection"
        $.ajax({
            url: "/api/DashboardTSEL/Section",
            type: "GET"
        })
            .done(function (data, textstatus, jqxhr) {
                $(id).html("<option></option>")
                $("#slSectionNonTsel").html("<option></option>")
                $("#slSectionBaps").html("<option></option>")
                $("#slSectionProd").html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.SectionProductId + "'>" + item.SectionName + "</option>");
                        $("#slSectionNonTsel").append("<option value='" + item.SectionProductId + "'>" + item.SectionName + "</option>");
                        $("#slSectionBaps").append("<option value='" + item.SectionProductId + "'>" + item.SectionName + "</option>");
                        $("#slSectionProd").append("<option value='" + item.SectionProductId + "'>" + item.SectionName + "</option>");
                    });
                }
                $(id).select2({ placeholder: "Select Section", width: null });
                $("#slSectionNonTsel").select2({ placeholder: "Select Section", width: null });
                $("#slSectionBaps").select2({ placeholder: "Select Section", width: null });
                $("#slSectionProd").select2({ placeholder: "Select Section", width: null });
            })
            .fail(function (jqxhr, textstatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectSow: function (id, SectionID) {
        $.ajax({
            url: "/api/DashboardTSEL/SOW",
            type: "GET",
            data: { SectionID: SectionID }
        })
            .done(function (data, textstatus, jqxhr) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.SowProductId + "'>" + item.SowName + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select SOW", width: null });
            })
            .fail(function (jqxhr, textstatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectYear: function (elements) {
        var dt = new Date();
        var currentyear = dt.getFullYear();
        elements.html("");

        for (var i = 0; i <= 10; i++) {
            if (currentyear == (currentyear + i))
                elements.append("<option value='" + (currentyear + i) + "' selected >" + (currentyear + i) + "</option>");
            else
                elements.append("<option value='" + (currentyear + i) + "'>" + (currentyear + i) + "</option>");

        }

        elements.select2({ placeholder: "Select Year", width: null });
    },
    BindingSelectDepartment: function () {
        $.ajax({
            url: "/api/DashboardTSEL/GetDepartmentInputTarget",
            type: "GET"
        })
            .done(function (data, textstatus, jqxhr) {
                $("#slDepartmentName").html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $("#slDepartmentName").append("<option value='" + item.DepartmentCode + "'>" + item.DepartmentName + "</option>");
                    });
                }
                $("#slDepartmentName").select2({ placeholder: "Select Department", width: null });
            })
            .fail(function (jqxhr, textstatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    // selectTwo aDDSite
    BindingSelectYearTarget: function (elements) {
        var dt = new Date();
        var currentyear = dt.getFullYear();
        elements.html("");

        for (var i = 0; i <= 10; i++) {
            if (currentyear == (currentyear + i))
                elements.append("<option value='" + (currentyear + i) + "' selected >" + (currentyear + i) + "</option>");
            else
                elements.append("<option value='" + (currentyear + i) + "'>" + (currentyear + i) + "</option>");

        }

        elements.select2({ placeholder: "Select Year", width: null });
    },
    BindingSelectMonthTarget: function (elements) {
        var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

        elements.html("");
        var currentMonth = new Date().getMonth();
        for (var i = 0; i < monthNames.length; i++) {
            if (i >= currentMonth) {
                elements.append("<option value='" + (i + 1) + "'>" + monthNames[i] + "</option>");
            }
        }

        elements.select2({ placeholder: "Select Month", width: null });
    },
    BindingSelectReconcileType: function (IDS) {
        var id = IDS
        $.ajax({
            url: "/api/DashboardTSEL/BapsTypeTSEL",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.mstBapsTypeId + "'>" + item.BapsType + "</option>");
                        //chckReconcile = item.mstBapsTypeId;
                    })
                }
                $(id).select2({ placeholder: "Select Reconcile Type", width: null });

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectReconType: function () {
        var id = "#slReconType"
        $.ajax({
            url: "/api/DashboardTSEL/BapsTypeInputTargetHistory",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.mstBapsTypeId + "'>" + item.BapsType + "</option>");
                        //chckReconcile = item.mstBapsTypeId;
                    })
                }
                $(id).select2({ placeholder: "Select Reconcile Type", width: null });

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingPnlHistoryPowerType: function () {
        $.ajax({
            url: "/api/MstDataSource/PowerType",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {

                $('#slTypeTower').html("<option></option>");
                $('#slTypeTowerNonTsel').html("<option></option>");
                $('#slTypeTowerBaps').html("<option></option>");
                $('#slTypeTowerProd').html("<option></option>");
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $('#slTypeTower').append("<option value='" + item.KodeType + "'>" + item.PowerType + "</option>");
                        $('#slTypeTowerNonTsel').append("<option value='" + item.KodeType + "'>" + item.PowerType + "</option>");
                        $('#slTypeTowerBaps').append("<option value='" + item.KodeType + "'>" + item.PowerType + "</option>");
                        $('#slTypeTowerProd').append("<option value='" + item.KodeType + "'>" + item.PowerType + "</option>");
                    })
                }
                $('#slTypeTower').select2({ placeholder: "Select Type Power", width: null });
                $('#slTypeTowerNonTsel').select2({ placeholder: "Select Type Power", width: null });
                $('#slTypeTowerBaps').select2({ placeholder: "Select Type Power", width: null });
                $('#slTypeTowerProd').select2({ placeholder: "Select Type Power", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingFilterPowerType: function () {
        var id = "#slFilterPowerType"
        $.ajax({
            url: "/api/MstDataSource/PowerType",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")
                $("#slFilterPowerTypeNonTsel").html("<option></option>")
                $("#slFilterPowerTypeBaps").html("<option></option>")
                $("#slFilterPowerTypeProd").html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.KodeType + "'>" + item.PowerType + "</option>");
                        $("#slFilterPowerTypeNonTsel").append("<option value='" + item.KodeType + "'>" + item.PowerType + "</option>");
                        $("#slFilterPowerTypeBaps").append("<option value='" + item.KodeType + "'>" + item.PowerType + "</option>");
                        $("#slFilterPowerTypeProd").append("<option value='" + item.KodeType + "'>" + item.PowerType + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Type Power", width: null });
                $("#slFilterPowerTypeNonTsel").select2({ placeholder: "Select Type Power", width: null });
                $("#slFilterPowerTypeBaps").select2({ placeholder: "Select Type Power", width: null });
                $("#slFilterPowerTypeProd").select2({ placeholder: "Select Type Power", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingFilterPwrType: function () {
        var id = "#slPWRType"
        $.ajax({
            url: "/api/MstDataSource/PowerType",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.KodeType + "'>" + item.PowerType + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Type Power", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    IsEmptyData: function (ID) {
        var result = true;
        if (TempData.length > 0) {
            for (var i = 0; i < TempData.length; i++) {
                if (TempData[i].MstBapsId == ID) {
                    result = false;
                    break;
                }
            }
        }
        return result;
    },
    DeleteRequestDetail: function (rowID) {
        $(rowID + " input[type=checkbox]").prop('checked', false);
        var index = TempData.findIndex(function (data) {
            return data.MstBapsId == rowID
        });
        TempData.splice(index, 1);
        if (TempData.length == 0)
            $(".pnlHistory").hide();
    },
    DeleteRequestDetailAll: function (rowID) {
        $(rowID + " input[type=checkbox]").prop('checked', false);
        var index = AllDataId.findIndex(function (data) {
            return data == rowID
        });
        var index2 = TempData.findIndex(function (data) {
            return data.MstBapsId == rowID
        });

        AllDataId.splice(index, 1);
        TempData.splice(index2, 1);
        if (AllDataId.length == 0)
            $(".panelAddSiteHistory").hide();
        //$(".pnlHistory").hide();
        //$("#formSearchTwo").hide();
    },
    SelectData: function (ID) {
        if (TempData.length > 0) {
            $.each(TempData, function (i, item) {
                $(item.MstBapsId + " input[type=checkbox]").prop('checked', true);
            })
        }
    },
    // Modification Or Added By Ibnu Setiawan 29. January 2020 Add Method Get Year Back Date & 
    // Changed Binding Month Temporary Because Api Master Not Ready In Prod
    BindSelectYearAll: function (elements) {
        var dt = new Date();
        var currentyear = dt.getFullYear();
        elements.html("");
        elements.append("<option>All</option>");
        for (var i = -10; i <= 10; i++) {
            if (currentyear == (currentyear + i))
                elements.append("<option value='" + (currentyear + i) + "' selected >" + (currentyear + i) + "</option>");
            else
                elements.append("<option value='" + (currentyear + i) + "'>" + (currentyear + i) + "</option>");

        }

        elements.select2({ placeholder: "Select Year", width: null });
    },

    BindSelectMonth: function (elements) {
        var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        elements.html("");
        elements.append("<option>All</option>");
        var dt = new Date();
        var currentmonth = dt.getMonth();
        for (var i = 0; i < monthNames.length; i++) {
            if (currentmonth == (i))
                elements.append("<option value='" + (i + 1) + "' selected>" + monthNames[i] + "</option>");
            else
                elements.append("<option value='" + (i + 1) + "'>" + monthNames[i] + "</option>");
        }
        elements.select2({ placeholder: "Select Month", width: null });
    },
}

var Helper = {
    GetListId: function () {
        App.blockUI({
            target: ".panelSearchResult", boxed: true
        });
        var AjaxData = [];
        try {
            var SONumber = $("#schSONumber").val();
            var SiteID = $("#schSiteID").val();
            var SiteName = $("#schSiteName").val();
            var CustomerSiteID = $("#schCustomerSiteID").val();
            var CustomerSiteName = $("#schCustomerSiteName").val();
            var CustomerID = $("#schCustomerID").val();
            var RegionName = $("#schRegionName").val();
            var YearBill = $("#schYearBill").val();
            var BapsType = $('#slFilterBapsType').val();
            var PowerType = $('#slFilterPowerType').val();
            var ReconcileType = $('#slReconType').val();
            var PowerType = $('#slPWRType').val();

            var params = {
                strCompanyInvoice: fsCompanyInvoice,
                strCustomerInvoice: fsCustomerInvoice,
                strSection: fsSection,
                strSOW: fsSOW,
                strBillingYear: fsBillingYear,
                strRegional: fsRegional,
                strProvince: fsProvince,
                strTenantType: fsTenantType,
                isReceive: 0,
                schSONumber: SONumber,
                schSiteID: SiteID,
                schSiteName: SiteName,
                schCustomerSiteID: CustomerSiteID,
                schCustomerSiteName: CustomerSiteName,
                schCustomerID: CustomerID,
                schRegionName: RegionName,
                schYearBill: YearBill,
                PowerType: PowerType,
                BapsType: BapsType,
                ReconcileType: fsReconcileType,
                PowerType: fsPowerType
                // SiteID: SiteID
            };

            var sDate = $("#slFilterStartDateTsel").val() != '' ? moment('1 ' + $("#slFilterStartDateTsel").val(), 'D MMM YYYY').format('YYYY/MM/DD') : '';
            var _eDate = '';
            if ($("#slFilterEndDateTsel").val() != '') {
                var eDate = moment('1 ' + $("#slFilterEndDateTsel").val(), 'D MMM YYYY').format('YYYY/MM/DD');
                var dayInMonth = moment(eDate, "YYYY/MM/DD").daysInMonth()
                _eDate = moment(eDate, 'YYYY/MM/DD').set({ 'D': dayInMonth }).format('YYYY/MM/DD');
            }
            params.StartInvoiceDate = sDate,
            params.EndInvoiceDate = _eDate
            $.ajax({
                url: "/api/DashboardTSEL/GetListData",
                type: "POST",
                dataType: "json",
                async: false,
                data: params,
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                AjaxData = data;
                App.unblockUI('.panelSearchResult');
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                App.unblockUI('.panelSearchResult');
            });
        } catch (err) {
            Common.Alert.Error(err)
            App.unblockUI('.panelSearchResult');
        }
        return AjaxData;
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
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == value) {
                isExist = true;
                break;
            }
        }
        return isExist;
    },
    IsGridDataExistsInTempData: function (mstBapsid, startInvoiceDate, tempData) {
        for (var i = 0; i < tempData.length; i++) {
            var bapsId = tempData[i].MstBapsId;
            var startDate = tempData[i].StartInvoiceDate;
            if (bapsId == mstBapsid && startDate == startInvoiceDate)
                return true;
        }
        return false;
    },
    IsGridDataExistsInTempDataSONumber: function (soNumber, tempData) {
        for (var i = 0; i < tempData.length; i++) {
            var _soNumber = tempData[i].SONumber;
            if (_soNumber == soNumber)
                return true;
        }
        return false;
    },
    RemoveDataInTempData: function (mstBapsid, startInvoiceDate, tempData) {
        for (var i = 0; i < tempData.length; i++) {
            var bapsId = tempData[i].MstBapsId;
            var startDate = tempData[i].StartInvoiceDate;
            if (bapsId == mstBapsid && startDate == startInvoiceDate)
                tempData.splice(i, 1);
        }
    },
    RemoveDataInTempDataSONumber: function (soNumber, tempData) {
        for (var i = 0; i < tempData.length; i++) {
            var _soNumber = tempData[i].SONumber;
            if (_soNumber == soNumber)
                tempData.splice(i, 1);
        }
    }
}
Number.prototype.format = function (n) {
    //currenct format: IDR Format : 2.020.213,00: param n -> belakang comma
    var re = '\\d(?=(\\d{' + (3) + '})+' + (n > 0 ? '\\D' : '$') + ')',
        num = this.toFixed(Math.max(0, ~~n));

    return (',' ? num.replace('.', ',') : num).replace(new RegExp(re, 'g'), '$&' + ('.' || ','));
};

jQuery.fn.extend({
    serializeObject: function () {
        var formdata = $(this).serializeArray();
        var data = {};
        $(formdata).each(function (index, obj) {
            data[obj.name] = obj.value;
        });
        return data;
    }
});