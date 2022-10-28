Data = {};

var fsCompanyId = "";
var fsOperator = "";
var fsBapsType = "";
var fsPeriodInvoice = "";
var fsInvoiceType = "";
var fsRegional = "";
var fsSONumber = "";
var fsPONumber = "";
var fsBAPSNumber = "";
var fsSiteIdOld = "";
var fsStartPeriod = "";
var fsEndPeriod = "";
var fsInvoiceStatusId = -1;

Data.RowSelected = [];
Data.RowSelectedSite = [];
Data.RowSelectedDeptHead = [];
Data.RowSelectedCount = 0;
var fsUserCompanyCode = "";

jQuery(document).ready(function () {
    // Modification Or Added By Ibnu Setiawan 04. September 2020
    // Add Company Code By User Login
    fsUserCompanyCode = $('#hdUserCompanyCode').val();
    // End Modification Or Added By Ibnu Setiawan 04. September 2020
    
    $("#slSearchInvoiceType").select2({ placeholder: "Select Invoice Type", width: null });

    Control.BindingSelectCompany();
    Control.BindingSelectOperator();
    Control.BindingSelectInvoiceType();
    Control.BindingSelectBapsType();
    Control.BindingSelectRegional();
    Control.BindingSelectInvoiceStatus();
    Form.Init();
    Table.Reset();
    if (fsUserCompanyCode.trim() == "PKP") {
        $("#slSearchCompanyName").val(fsUserCompanyCode).trigger("change");
    }

    // Di pindahkan karena ada Binding Grid yang mendahului Initialize Dropdown
    Control.GetCurrentUserRole(); // Modification Or Added By Ibnu Setiawan 09. September 2020 

    //panel Summary
    $("#btSearch").unbind().click(function () {
        if (Data.Role == "DEPT HEAD")
            TableDepthead.Search();
        else
            Table.Search();
        //Control.SetPPHAndPercentage();
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
        if (fsUserCompanyCode.trim() == "PKP") {
            $("#slSearchCompanyName").val(fsUserCompanyCode).trigger("change");
        }
    });

    $("#btCancel").unbind().click(function () {
        $(".panelSiteData").fadeOut();
        Data.RowSelected = [];
        Data.RowSelectedDeptHead = [];
        Data.RowSelectedSite = [];
        Form.Init();
        Table.Search();
    });

    $(".panelSearchZero").hide();

    $("#btAddSite").unbind().click(function () {
        Process.AddToSiteList();
        Helper.ScrollToTop();
    });

    $("#btAddSiteDeptHead").unbind().click(function () {
        Process.AddToSiteListDeptHead();
        Helper.ScrollToTop();

    });

    $("#btCreateInvoice").unbind().click(function () {
        var validate = Helper.ValidateStatus();
        if (validate.Result == "")
            Process.CreateInvoice();
        else
            Common.Alert.Warning(validate.Result);
    });

    $("#btReturn").unbind().click(function () {
        Data.CheckedRow = [];
        var oTable = $('#tblSummaryData').DataTable();
        var TotalItem = Helper.GetListId().length > 0 ? Helper.GetListId().length : 0;
        var ExcludedItem = Helper.GetExcludedID().length > 0 ? Helper.GetExcludedID().length : 0;
        var ProcessedItem = TotalItem - ExcludedItem;
        $("#lblTotalItemReject").text(Common.Format.CommaSeparationOnly(TotalItem));
        $("#lblProcessedItemReject").text(Common.Format.CommaSeparationOnly(ProcessedItem));
        $("#lblExcludedItemReject").text(Common.Format.CommaSeparationOnly(ExcludedItem));
        $('#mdlCancelInvoice').modal('show');
    });

    $("#formReject").submit(function (e) {
        Process.ReturnInvoice();
        e.preventDefault();
    });

    $("#btApproveCancel").unbind().click(function () {
        Process.ApproveCancelInvoice();
    });

    $('#tblSummaryData').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            Data.RowSelected.push(parseInt(id));
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row" + id).removeClass("active");
            Helper.RemoveElementFromArray(Data.RowSelected, parseInt(id));
        }
    });

    $('#tblSummaryDataDeptHead').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            Data.RowSelectedDeptHead.push(parseInt(id));
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row" + id).removeClass("active");
            Helper.RemoveElementFromArray(Data.RowSelectedDeptHead, parseInt(id));
        }
    });

    // Handle click on table cells with checkboxes
    //$('#tblSummaryData').on('click', 'tbody td, thead th:first-child', function (e) {
    //    $(this).parent().find('input[type="checkbox"]').trigger('click');
    //});

    $('#chPPN').on("switchChange.bootstrapSwitch", function (event, state) {
        Process.Calculate();
    });

    $('#chPPH').on("switchChange.bootstrapSwitch", function (event, state) {
        Process.Calculate();
    });

    $('#chRounding').on("switchChange.bootstrapSwitch", function (event, state) {
        Process.Calculate();
    });

    $('input[type=radio][name=rbPercent]').change(function () {
        Process.Calculate();
    });

    ///use GR
    $("#chGR").bootstrapSwitch("state", false);

    $("#btCalculate").unbind().click(function () {
        var validate = Helper.ValidateStatus();
        if (validate.Result == "")
        {
            Control.SetPPHAndPercentage();
            Control.GetSumADPP();
            // Process.Calculate();
            $("#btCreateInvoice").show();
        }
        else
            Common.Alert.Warning(validate.Result);
        
    });
});

var Form = {
    Init: function () {
        $("#formReject").parsley();
        if (!$("#hdAllowProcess").val()) {
            $("#btAddSite").hide();
            $("#btReturn").hide();
        }

        

        $(".panelSiteData").hide();

        TableSite.Init();
        Helper.InitCurrencyInput("#tbDiscount");
        Helper.InitCurrencyInput("#tbPenalty");
        //Control.SetPPHAndPercentage();
        //Control.GetSumADPP();
        $("#btCreateInvoice").hide();

        if (Data.Role == "DEPT HEAD")
            $("#staffPanel").hide();
    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        //Control.SetPPHAndPercentage();
    },
    ClearRowSelected: function () {
        Data.RowSelected = [];
        Data.RowSelectedSite = [];
        Data.RowSelectedDeptHead = [];
        $("#btCreateInvoice").hide();
    }
}

var Table = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsPeriodInvoice = $("#slSearchInvoiceType").val() == null ? "" : $("#slSearchInvoiceType").val();
        fsInvoiceType = $("#slSearchTermInvoice").val() == null ? "" : $("#slSearchTermInvoice").val();
        fsRegional = $("#slSearchRegional").val() == null ? "" : $("#slSearchRegional").val();
        fsSONumber = $("#tbSearchSONumber").val() == null ? "" : $("#tbSearchSONumber").val();
        fsPONumber = $("#tbSearchPONumber").val() == null ? "" : $("#tbSearchPONumber").val();
        fsBAPSNumber = $("#tbSearchBAPSNumber").val() == null ? "" : $("#tbSearchBAPSNumber").val();
        fsSiteIdOld = $("#tbSearchSiteIdOld").val() == null ? "" : $("#tbSearchSiteIdOld").val();
        fsInvoiceStatusId = $("#slSearchInvoiceStatus").val() == null || $("#slSearchInvoiceStatus").val() == "" ? -1 : $("#slSearchInvoiceStatus").val();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strBapsType: fsBapsType,
            strPeriodInvoice: fsPeriodInvoice,
            strInvoiceType: fsInvoiceType,
            strRegional: fsRegional,
            strSONumber: fsSONumber,
            strPONumber: fsPONumber,
            strBAPSNumber: fsBAPSNumber,
            strSiteIdOld: fsSiteIdOld,
            intmstInvoiceStatusId: fsInvoiceStatusId,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod
        };
        //Control.GetSumADPP();

        var tblSummaryData = $("#tblSummaryData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/CreateSuperInvoiceTower/grid",
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
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        if (full.mstInvoiceStatusId == 5)
                        { strReturn += "<label style='display:none' id='" + full.trxArDetailId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' class='checkboxes' /><span></span></label>"; }
                        else
                        {
                            if (Helper.IsElementExistsInArray(parseInt(full.trxArDetailId), Data.RowSelected)) {
                                if (Helper.IsElementExistsInArray(parseInt(full.trxArDetailId), Helper.GetExcludedID())) {
                                    //$("#Row" + full.trxArDetailId).addClass("active");
                                    strReturn += "<label style='display:none' id='" + full.trxArDetailId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxArDetailId + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                                }
                                else {
                                    strReturn += "<label id='" + full.trxArDetailId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxArDetailId + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                }
                            }
                            else {
                                strReturn += "<label id='" + full.trxArDetailId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxArDetailId + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }
                        }
                        return strReturn;
                    }
                },
                { data: "SONumber" },
                { data: "SiteIdOpr" },
                { data: "SiteIdOld" },
                { data: "SiteName" },
                { data: "CompanyInvoice" },
                { data: "Operator" },
                { data: "PoNumber" },
                {
                    data: "StartDateInvoice", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndDateInvoice", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "BapsNo" },
                { data: "AmountRental", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AmountService", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvoiceAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                {
                    data: "StartDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "AmountPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AmountLossPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "CompanyId" },
                { data: "OperatorAsset" },
                { data: "PeriodInvoice" },
                { data: "InvoiceTypeDesc" },
                { data: "BapsType" },
                { data: "PowerType" },
                { data: "Currency" },
                { data: "InvStatus" },
                { data: "ReturnRemarks" },
                //{
                //    data: "IsPartial", mRender: function (data, type, full) {
                //        return full.IsPartial ? "Yes" : "No";
                //    }
                //},
                {
                    data: "IsLossPPN", mRender: function (data, type, full) {
                        return full.IsLossPPN ? "Yes" : "No";
                    }
                },
                { data: "PPHType" }
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "order": [1, "asc"],
            "scrollY": 300,
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        if(!Helper.IsElementExistsInArray(parseInt(item), Helper.GetExcludedID()))
                            $("#Row" + item).addClass("active");
                    }
                }
            },
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                                    '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tblSummaryData .checkboxes" />' +
                                    '<span></span> '+
                                '</label>';
                var th = $("th.select-all").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all").unbind().on("change", ".group-checkable", function (e) {
                    var set = $(".panelSearchResult .DTFC_LeftBodyWrapper .DTFC_Cloned .checkboxes");
                    var checked = jQuery(this).is(":checked");
                    jQuery(set).each(function () {
                        label = $(this).parent();
                        var id = label.attr("id");
                        if (label.attr("style") != "display:none") {
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
                    if (checked)
                        Data.RowSelected = Data.RowSelected.concat(Helper.GetListId());
                    else {
                        $.each(Helper.GetListId(), function (index, item) {
                            Helper.RemoveElementFromArray(Data.RowSelected, parseInt(item));
                        });
                    }
                });
            }
        });
    },
    Reset: function () {
        fsCompanyId = "";
        fsOperator = "";
        fsBapsType = "";
        fsPeriodInvoice = "";
        fsInvoiceType = "";
        fsRegional = "";
        fsSONumber = "";
        fsPONumber = "";
        fsBapsNumber = "";
        fsSiteIdOld = "";
        fsInvoiceStatusId = -1;
        fsStartPeriod = "";
        fsEndPeriod = "";

        $("#slSearchCompanyName").val("").trigger('change');
        $("#slSearchOperator").val("").trigger('change');
        $("#slSearchBapsType").val("").trigger('change');
        $("#slSearchTermInvoice").val("").trigger('change');
        $("#slSearchInvoiceType").val("").trigger('change');
        $("#slSearchRegional").val("").trigger('change');
        $("#tbSearchSONumber").val("");
        $("#tbSearchSiteIdOld").val("");
        $("#tbSearchPONumber").val("");
        $("#tbSearchBAPSNumber").val("");
        $("#slSearchInvoiceStatus").val("").trigger('change');
        $("#tbStartPeriod").val("");
        $("#tbEndPeriod").val("");
    },
    Export: function () {
        window.location.href = "/InvoiceTransaction/TrxCreateInvoiceTower/Export?strCompanyId=" + fsCompanyId + "&strOperator=" + fsOperator
            + "&strBapsType=" + fsBapsType + "&strPeriodInvoice=" + fsPeriodInvoice + "&strInvoiceType=" + fsInvoiceType + "&strRegional=" + fsRegional
            + "&strSONumber=" + fsSONumber + "&intmstInvoiceStatusId=" + fsInvoiceStatusId + "&strBAPSNumber=" + fsBapsNumber + "&strPONumber=" + fsPONumber + "&strSiteIdOld=" + fsSiteIdOld
        + "&strStartPeriod=" + fsStartPeriod + "&strEndPeriod=" + fsEndPeriod;
    }
}

var TableDepthead = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();

        $(window).resize(function () {
            $("#tblSummaryDataDeptHead").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsPeriodInvoice = $("#slSearchInvoiceType").val() == null ? "" : $("#slSearchInvoiceType").val();
        fsInvoiceType = $("#slSearchTermInvoice").val() == null ? "" : $("#slSearchTermInvoice").val();
        fsRegional = $("#slSearchRegional").val() == null ? "" : $("#slSearchRegional").val();
        fsSONumber = $("#tbSearchSONumber").val() == null ? "" : $("#tbSearchSONumber").val();
        fsPONumber = $("#tbSearchPONumber").val() == null ? "" : $("#tbSearchPONumber").val();
        fsBAPSNumber = $("#tbSearchBAPSNumber").val() == null ? "" : $("#tbSearchBAPSNumber").val();
        fsSiteIdOld = $("#tbSearchSiteIdOld").val() == null ? "" : $("#tbSearchSiteIdOld").val();
        fsInvoiceStatusId = $("#slSearchInvoiceStatus").val() == null || $("#slSearchInvoiceStatus").val() == "" ? -1 : $("#slSearchInvoiceStatus").val();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strBapsType: fsBapsType,
            strPeriodInvoice: fsPeriodInvoice,
            strInvoiceType: fsInvoiceType,
            strRegional: fsRegional,
            strSONumber: fsSONumber,
            strPONumber: fsPONumber,
            strBAPSNumber: fsBAPSNumber,
            strSiteIdOld: fsSiteIdOld,
            intmstInvoiceStatusId: fsInvoiceStatusId,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod
        };

        var tblSummaryDataDeptHead = $("#tblSummaryDataDeptHead").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/CreateSuperInvoiceTower/grid",
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
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [

                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        var strReturn = "";
                        if (full.mstInvoiceStatusId != 5)
                        { strReturn += "<label style='display:none' id='" + full.trxArDetailId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxArDetailId + "'><input type='checkbox' class='checkboxes' /><span></span></label>"; }
                        else
                        {
                            if (Helper.IsElementExistsInArray(parseInt(full.trxArDetailId), Data.RowSelectedDeptHead)) {
                                if (Helper.IsElementExistsInArray(parseInt(full.trxArDetailId), Helper.GetExcludedID())) {
                                    //$("#Row" + full.trxArDetailId).addClass("active");
                                    strReturn += "<label style='display:none' id='" + full.trxArDetailId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxArDetailId + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                                }
                                else {
                                    strReturn += "<label id='" + full.trxArDetailId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxArDetailId + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                }
                            }
                            else {
                                strReturn += "<label id='" + full.trxArDetailId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxArDetailId + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }
                        }
                        return strReturn;
                    }
                },
                { data: "SONumber" },
                { data: "SiteIdOpr" },
                { data: "SiteIdOld" },
                { data: "SiteName" },
                { data: "CompanyInvoice" },
                { data: "Operator" },
                { data: "PoNumber" },
                {
                    data: "StartDateInvoice", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndDateInvoice", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "BapsNo" },
                { data: "AmountRental", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AmountService", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvoiceAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvStatus" },
                { data: "ReturnRemarks" },
                {
                    data: "StartDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "AmountPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AmountLossPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "CompanyId" },
                { data: "OperatorAsset" },
                { data: "PeriodInvoice" },
                { data: "InvoiceTypeDesc" },
                { data: "BapsType" },
                { data: "PowerType" },
                { data: "Currency" },
                //{
                //    data: "IsPartial", mRender: function (data, type, full) {
                //        return full.IsPartial ? "Yes" : "No";
                //    }
                //},
                {
                    data: "IsLossPPN", mRender: function (data, type, full) {
                        return full.IsLossPPN ? "Yes" : "No";
                    }
                },
                { data: "PPHType" }
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryDataDeptHead.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelectedDeptHead.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelectedDeptHead.length; i++) {
                        item = Data.RowSelectedDeptHead[i];
                        $("#Row" + item).addClass("active");
                    }
                }
            },
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (Data.Role == "DEPT HEAD") {
                    if (aData.mstInvoiceStatusId == 5) {
                        $('td', nRow).css('background-color', '#FF9999');
                    }
                }
                l.stop();
            },
            //"order": [2, "asc"],
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
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                                    '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tblSummaryData .checkboxes" />' +
                                    '<span></span> ' +
                                '</label>';
                var th = $("th.select-all-dept-head").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-dept-head").unbind().on("change", ".group-checkable", function (e) {
                    var set = $(".panelSearchResult .DTFC_LeftBodyWrapper .DTFC_Cloned .checkboxes");
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
                    if (checked)
                        Data.RowSelectedDeptHead = Helper.GetListId();
                    else
                        Data.RowSelectedDeptHead = [];
                });
            }
        });
    }
}

var TableSite = {
    Init: function () {
        $(window).resize(function () {
            $("#tblSiteData").DataTable().columns.adjust().draw();
        });
    },
    GetSelectedSiteData: function (listID) {
        var AjaxData = [];
        var excludedIDs = Helper.GetExcludedID();

        $.each(listID, function (index, item) {
            excludedIDs.push(parseInt(item));
        });

        var params = {
            ListId: excludedIDs
        };
        var l = Ladda.create(document.querySelector("#btAddSite"));
        $.ajax({
            url: "/api/CreateInvoiceTower/SitelistGrid",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                AjaxData = data;
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            alert("fail");
            Common.Alert.Error(errorThrown)
            l.stop();
        });
        return AjaxData;
    },
    AddSite: function () {
        //Get Data that in RowSelected
        var ajaxData;
        if (Data.Role == "DEPT HEAD")
            ajaxData = TableSite.GetSelectedSiteData(Data.RowSelectedDeptHead);
        else
            ajaxData = TableSite.GetSelectedSiteData(Data.RowSelected);

        var tblSiteData = $("#tblSiteData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "data": ajaxData,
            "filter": false,
            "destroy": true,
            "columns": [
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<button type='button' title='Delete' class='btn btn-xs red btDeleteSite' id='btDeleteSite" + full.trxArDetailId + "'><i class='fa fa-trash'></i></button>";
                        return strReturn;
                    }
                },
                { data: "SONumber" },
                { data: "SiteIdOpr" },
                { data: "SiteIdOld" },
                { data: "SiteName" },
                { data: "CompanyInvoice" },
                { data: "Operator" },
                { data: "PoNumber" },
                {
                    data: "StartDateInvoice", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndDateInvoice", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "BapsNo" },
                { data: "AmountRental", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AmountService", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvoiceAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                {
                    data: "StartDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "AmountPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AmountLossPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "CompanyId" },
                { data: "OperatorAsset" },
                { data: "PeriodInvoice" },
                { data: "InvoiceTypeDesc" },
                { data: "BapsType" },
                { data: "PowerType" },
                { data: "Currency" },
                //{
                //    data: "IsPartial", mRender: function (data, type, full) {
                //        return full.IsPartial ? "Yes" : "No";
                //    }
                //},
                {
                    data: "IsLossPPN", mRender: function (data, type, full) {
                        return full.IsLossPPN ? "Yes" : "No";
                    }
                },
                { data: "PPHType" }
            ],
            "order": [1, "asc"],
            "columnDefs": [{ "targets": [0], "orderable": false }]
        });

        $("#tblSiteData tbody").unbind().on("click", "button.btDeleteSite", function (e) {
            var table = $("#tblSiteData").DataTable();
            var buttonId = $(this).attr("id");
            var idComponents = buttonId.split('btDeleteSite');
            var id = idComponents[1];

            //Row with ID in btDeleteSite back to tblSummary with normal state checkbox
            $("#Row" + id).removeClass('active');
            $("#" + id + " input[type=checkbox]").removeAttr("checked");
            $("#" + id).show();

            /* Uncheck the checkbox in cloned table */
            $('.Row' + id).removeClass('active');
            $('.' + id + ' input[type=checkbox]').removeAttr("checked");
            $('.' + id).show();

            table.row($(this).parents('tr')).remove().draw();
            //Delete id in Array for rendering when change page
            if(Data.Role == "DEPT HEAD")
                Helper.RemoveElementFromArray(Data.RowSelectedDeptHead, parseInt(id));
            else
                Helper.RemoveElementFromArray(Data.RowSelected, parseInt(id));
            Helper.RemoveElementFromArray(Data.RowSelectedSite, parseInt(id));
            //Control.GetSumADPP();
            if (Data.RowSelectedSite.length == 0)
                $(".panelSiteData").fadeOut();
        });
    },
    Validate: function (dtExistingRow, checkedRow) {
        if (dtExistingRow.CompanyInvoice.trim() != checkedRow.CompanyInvoice.trim() || dtExistingRow.Operator.trim() != checkedRow.Operator.trim()
            || dtExistingRow.InvoiceTypeDesc.trim() != checkedRow.InvoiceTypeDesc.trim() || dtExistingRow.Currency.trim() != checkedRow.Currency.trim()) {
            return true;
        }
        return false;
    }
}

var Control = {
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async:false
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchCompanyName").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchCompanyName").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
            }

            $("#slSearchCompanyName").select2({ placeholder: "Select Company Name", width: null });

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
    BindingSelectInvoiceType: function () {
        $.ajax({
            url: "/api/MstDataSource/InvoiceType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchTermInvoice").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchTermInvoice").append("<option value='" + item.mstInvoiceTypeId + "'>" + item.Description + "</option>");
                })
            }

            $("#slSearchTermInvoice").select2({ placeholder: "Select Term Invoice", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectBapsType: function () {
        $.ajax({
            url: "/api/MstDataSource/BapsType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchBapsType").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchBapsType").append("<option value='" + item.BapsType + "'>" + item.BapsType + "</option>");
                })
            }

            $("#slSearchBapsType").select2({ placeholder: "Select BAPS Type", width: null });

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
            $("#slSearchRegional").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchRegional").append("<option value='" + item.Regional + "'>" + item.Regional + "</option>");
                })
            }

            $("#slSearchRegional").select2({ placeholder: "Select Regional", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectInvoiceStatus: function () {
        $("#slSearchInvoiceStatus").html("<option></option>")
        if (Data.Role == "DEPT HEAD")
            $("#slSearchInvoiceStatus").append("<option value='12'>CANCEL APPROVED</option>");
        else
            $("#slSearchInvoiceStatus").append("<option value='0'>BAPS CONFIRM</option>");
        $("#slSearchInvoiceStatus").append("<option value='5'>WAITING FOR APPROVAL</option>");
        $("#slSearchInvoiceStatus").select2({ placeholder: "Select Invoice Status", width: null });

    },
    SetControlByPosition: function () {
        $.ajax({
            url: "/api/user/GetPosition",
            type: "GET"

        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                if (data.Result == "DEPT HEAD") {

                    $(".disabledCtrl").prop('disabled', true);
                    $("#btCancelInvoiceTemp").hide();
                    $("#btPosting").hide();
                    $("#btRequest").hide();
                    $("#tbRemarksCancel").val(Data.Selected.InvRemarksPosting);
                    if (Data.Selected.mstInvoiceStatusId == 5) {
                        $("#btApproveCancel").show();
                        $("#btApprove").show();
                    }
                    else {
                        $("#btApproveCancel").show();
                        $("#btApprove").hide();
                    }
                }
                else {
                    if (Data.Selected.mstInvoiceStatusId == 5) {
                        $(".disabledCtrl").prop('disabled', true);
                        $("#tbRemarksCancel").val(Data.Selected.InvRemarksPosting);
                        $("#btCancelInvoiceTemp").show();
                        $("#btPosting").hide();
                        $("#btApproveCancel").hide();
                        $("#btRequest").hide();
                        $("#btApprove").hide();
                    }
                    else {
                        $(".disabledCtrl").prop('disabled', false);
                        $("#btCancelInvoiceTemp").show();
                        $("#btPosting").show();
                        $("#btApproveCancel").hide();
                        $("#btRequest").show();
                        $("#btApprove").hide();
                    }
                }
            }

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });

    },
    GetCurrentUserRole: function () {
        Role = "";
        $.ajax({
            url: "/api/user/GetPosition",
            type: "GET",
            async: false

        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                Data.Role = data.Result;
                if (data.Result == "DEPT HEAD") {
                    $('.Staff').hide();
                    $('.DeptHead').show();
                }
                else {
                    $('.Staff').show();
                    $('.DeptHead').hide();
                }
                if (Data.Role == "DEPT HEAD")
                    TableDepthead.Search();
                else
                    Table.Search();
            }
        });
    },
    GetSumADPP: function () {
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strBapsType: fsBapsType,
            strPeriodInvoice: fsPeriodInvoice,
            strInvoiceType: fsInvoiceType,
            strRegional: fsRegional,
            strSONumber: fsSONumber,
            strBAPSNumber: fsBAPSNumber,
            strPONumber: fsPONumber,
            strSiteIdOld: fsSiteIdOld,
            intmstInvoiceStatusId: fsInvoiceStatusId,
            excludedIDs: Helper.GetExcludedID(),
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod
        };

        $.ajax({
            url: "/api/CreateSuperInvoiceTower/GetSumADPP",
            type: "POST",
            cache: false,
            async: true,
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params)
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $("#tbTotal").val(Common.Format.CommaSeparation(data.InvSumADPP));
                Process.Calculate();
            }
        });
    },
    SetPPHAndPercentage: function () {
        var validationResult = Helper.ValidateStatus();
        if (validationResult.isShowPPH) {
            $("#divPPH").show();
            $("#chPPH").bootstrapSwitch("state", true);
        } else {
            $("#divPPH").hide();
            //$("#chPPH").bootstrapSwitch("state", false);
        }
        if (validationResult.isCreate15) {
            $("#divPercent").show();
            $("#rbPercent100").attr('checked', true);
        }
        else {
            $("#divPercent").hide();
        }
        if (validationResult.isLossPPN) {
            $("#chPPN").bootstrapSwitch("state", true);
            $("#chPPN").bootstrapSwitch('disabled', true);
        }
        else {
            $("#chPPN").bootstrapSwitch("state", true);
            $("#chPPN").bootstrapSwitch('disabled', false);
        }
        //Checking Operator for Initiate Round Checkbox
        if (Helper.IsElementExistsInArray(validationResult.Operator.trim().toUpperCase(), MappingOperatorRoundUp)) {
            $("#chRounding").bootstrapSwitch("state", true);
        }
        else {
            $("#chRounding").bootstrapSwitch("state", false);
        }
    }
}

var Process = {
    AddToSiteList: function () {
        Data.CheckedRow = [];
        Data.isDifferent = false;
        Data.PPHValue = 1;
        var l = Ladda.create(document.querySelector("#btAddSite"))
        var oTable = $('#tblSummaryData').DataTable();
        var oClonedTable = $('.panelSearchResult .DTFC_LeftBodyWrapper .DTFC_Cloned');
        if (Data.RowSelected.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            var OperatorRound = "";
            Data.PPHValue = $("#hdPPHValue").val();
            Data.PPFValue = $("#hdPPFValue").val();
            Data.PPNValue = $("#hdPPNValue").val();
            TableSite.AddSite();
            //Control.GetSumADPP();
            $(".panelSiteData").fadeIn();

            var selectedRowId = [];
            oTable.rows('.active').every(function (rowIdx, tableLoop, rowLoop) {
                var checkBoxId = this.data().trxArDetailId;
                OperatorRound = this.data().Operator;
                $("#Row" + checkBoxId).removeClass('active');
                $("#" + checkBoxId).hide();
            });

            $.each(Data.RowSelected, function (index, item) {
                $(".Row" + item).removeClass('active');
                $("." + item).hide();
            });

            //insert Data.RowSelectedSite for rendering checkbox
            $.each(Data.RowSelected, function (index, item) {
                if (Data.RowSelectedSite.indexOf(parseInt(item)) == -1)
                    Data.RowSelectedSite.push(parseInt(item));
            });

            //Set CheckBox disabled
            //if (Data.isShowPPH) {
            //    $("#divPPH").show();
            //    $("#chPPH").bootstrapSwitch("state", true);
            //} else {
            //    $("#divPPH").hide();
            //    $("#chPPH").bootstrapSwitch("state", false);
            //}
            //if (Data.isCreate15) {
            //    $("#divPercent").show();
            //    $("#rbPercent100").attr('checked', true);
            //}
            //else {
            //    $("#divPercent").hide();
            //}
            //if (Data.isLossPPN) {
            //    $("#chPPN").bootstrapSwitch("state", true);
            //    $("#chPPN").bootstrapSwitch('disabled', true);
            //}
            //else {
            //    $("#chPPN").bootstrapSwitch("state", true);
            //    $("#chPPN").bootstrapSwitch('disabled', false);
            //}
            ////Checking Operator for Initiate Round Checkbox
            //if (Helper.IsElementExistsInArray(OperatorRound.trim().toUpperCase(), MappingOperatorRoundUp)) {
            //    $("#chRounding").bootstrapSwitch("state", true);
            //}
            //else {
            //    $("#chRounding").bootstrapSwitch("state", false);
            //}
        }
    },
    AddToSiteListDeptHead: function(){
        Data.CheckedRow = [];
        Data.isDifferent = false;
        Data.PPHValue = 1;
        var l = Ladda.create(document.querySelector("#btAddSite"))
        var oTable = $('#tblSummaryData').DataTable();
        var oClonedTable = $('.panelSearchResult .DTFC_LeftBodyWrapper .DTFC_Cloned');
        if (Data.RowSelectedDeptHead.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            TableSite.AddSite();
            $(".panelSiteData").fadeIn();

            var selectedRowId = [];
            oTable.rows('.active').every(function (rowIdx, tableLoop, rowLoop) {
                var checkBoxId = this.data().trxArDetailId;
                $("#Row" + checkBoxId).removeClass('active');
                $("#" + checkBoxId).hide();
            });

            $.each(Data.RowSelectedDeptHead, function (index, item) {
                $(".Row" + item).removeClass('active');
                $("." + item).hide();
            });

            //insert Data.RowSelectedSite for rendering checkbox
            $.each(Data.RowSelectedDeptHead, function (index, item) {
                if(Data.RowSelectedSite.indexOf(parseInt(item)) == -1)
                    Data.RowSelectedSite.push(parseInt(item));
            });
        }
    },
    Calculate: function () {
        var InvoiceAmount = parseFloat($("#tbTotal").val().replace(/,/g, ""));
        var DPPAmount = parseFloat($("#tbDPP").val().replace(/,/g, ""));
        var DiscountAmount = parseFloat($("#tbDiscount").val().replace(/,/g, ""));
        var PenaltyAmount = parseFloat($("#tbPenalty").val().replace(/,/g, ""));
        var TotalInvoice = 0;
        var PPNAmount = 0;
        var PPHAmount = 0;
        var oTableSite = $('#tblSiteData').DataTable();

        Data.PPHValue = $("#hdPPHValue").val();
        Data.PPFValue = $("#hdPPFValue").val();
        Data.PPNValue = $("#hdPPNValue").val();

        //var IsLossPPN = oTableSite.row(0).data().IsLossPPN;
        var Percent = $('input[name=rbPercent]:checked').val();

        DPPAmount = InvoiceAmount - DiscountAmount;
        PPNAmount = DPPAmount * Data.PPNValue;

        if (Data.isPPHFinal)
            PPHAmount = DPPAmount * Data.PPFValue;
        else
            PPHAmount = DPPAmount * Data.PPHValue;

        if ($("#chPPN").bootstrapSwitch("state") == false) {
            PPNAmount = 0;
        }//else cek lagi jika true cek islosppn jika bener PPN 0
        else {
            if (Data.isLossPPN) {
                PPNAmount = 0;
            }
        }
        if ($("#chPPH").bootstrapSwitch("state") == false) {
            PPHAmount = 0;
        }

        if (Percent == "15") {
            DPPAmount = DPPAmount * 0.85;
            PPNAmount = PPNAmount * 0.85;
            PPHAmount = PPHAmount * 0.85;
        }
        else if (Percent == "10") {
            DPPAmount = DPPAmount * 0.9;
            PPNAmount = PPNAmount * 0.9;
            PPHAmount = PPHAmount * 0.9;
        }

        if ($("#chRounding").bootstrapSwitch("state") == true) {
            PPNAmount = Math.floor(PPNAmount);
        }
        else {
            PPNAmount = Math.round(PPNAmount);
        }
        TotalInvoice = DPPAmount + PPNAmount - PenaltyAmount;

        $("#tbTotal").val(Common.Format.CommaSeparation(InvoiceAmount.toString()));
        $("#tbDPP").val(Common.Format.CommaSeparation(DPPAmount.toString())); 
        $("#tbPPN").val(Common.Format.CommaSeparation(PPNAmount.toString()));
        $("#tbPPH").val(Common.Format.CommaSeparation(PPHAmount.toString()));
        $("#tbPenalty").val(Common.Format.CommaSeparation(PenaltyAmount.toString()));
        $("#tbDiscount").val(Common.Format.CommaSeparation(DiscountAmount.toString()));
        $("#tbTotalInvoice").val(Common.Format.CommaSeparation(TotalInvoice.toString()));

        if (TotalInvoice != 0) {
            if (TotalInvoice > 0)
                $("#tbTerbilang").val(Common.Format.Terbilang(TotalInvoice));
            else
                $("#tbTerbilang").val("");
        } else
            $("#tbTerbilang").val("");
    },
    CreateInvoice: function () {
        Data.SiteRow = [];
        var l = Ladda.create(document.querySelector("#btCreateInvoice"));
        PercentVal = 0;
        if ($("#rbPercent85").is(":checked")) {
            PercentVal = $("#rbPercent85").val();
        }
        else if ($("#rbPercent90").is(":checked")) {
            PercentVal = $("#rbPercent90").val();
        }
        var params = {
            SumADPP: $("#tbDPP").val().replace(/,/g, ''),
            SumAPPN: $("#tbPPN").val().replace(/,/g, ''),
            SumAPenalty: $("#tbPenalty").val().replace(/,/g, ''),
            SumATotalInvoice: $("#tbTotalInvoice").val().replace(/,/g, ''),
            SumAPPH: $("#tbPPH").val().replace(/,/g, ''),
            IsPPN: $("#chPPN").bootstrapSwitch("state"),
            IsPPH: $("#chPPH").bootstrapSwitch("state"),
            ListTrxArDetail: Data.SiteRow,
            PercentValue: PercentVal,
            SumADiscount: $("#tbDiscount").val().replace(/,/g, ''),
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strBapsType: fsBapsType,
            strPeriodInvoice: fsPeriodInvoice,
            strInvoiceType: fsInvoiceType,
            strRegional: fsRegional,
            strSONumber: fsSONumber,
            intmstInvoiceStatusId: fsInvoiceStatusId,
            excludedIDs: Helper.GetExcludedID(),
            strPONumber: fsPONumber,
            strBAPSNumber: fsBAPSNumber,
            strSiteIdOld: fsSiteIdOld,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod
        }

        $.ajax({
            url: "/api/CreateSuperInvoiceTower/Create",
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
                Common.Alert.Success("Data Success Created With Invoice No :" + data.InvNo)
            }
            l.stop();

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            var oTableSite = $('#tblSiteData').DataTable();
            oTableSite.clear();
            $(".panelSiteData").fadeOut();
            Table.Search();
            Form.ClearRowSelected();
        })
    },
    ReturnInvoice: function () {
        Data.CheckedRow = [];
        var l = Ladda.create(document.querySelector("#btReturn"))
        var oTable = $('#tblSummaryData').DataTable();

        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strBapsType: fsBapsType,
            strPeriodInvoice: fsPeriodInvoice,
            strInvoiceType: fsInvoiceType,
            strRegional: fsRegional,
            strSONumber: fsSONumber,
            strBAPSNumber: fsBAPSNumber,
            strPONumber: fsPONumber,
            strSiteIdOld: fsSiteIdOld,
            intmstInvoiceStatusId: fsInvoiceStatusId,
            excludedIDs: Helper.GetExcludedID(),
            CancelRemarks: $('#tbRemarksCancel').val(),
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod
        }

        $.ajax({
            url: "/api/CreateSuperInvoiceTower/Cancel",
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
                Common.Alert.Success("Data Returned Successfully and Waiting for Approval.")
                Data.RowSelectedSite = [];
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            var oTableSite = $('#tblSiteData').DataTable();
            oTableSite.clear();
            $(".panelSiteData").fadeOut();
            Form.Init();
            Table.Search();
            $('#mdlCancelInvoice').modal('hide');
            $("#tbRemarksCancel").val("");
            Form.ClearRowSelected();
        });
    },
    ApproveCancelInvoice: function () {
        var l = Ladda.create(document.querySelector("#btApproveCancel"))
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strBapsType: fsBapsType,
            strPeriodInvoice: fsPeriodInvoice,
            strInvoiceType: fsInvoiceType,
            strRegional: fsRegional,
            strSONumber: fsSONumber,
            strBAPSNumber: fsBAPSNumber,
            strPONumber: fsPONumber,
            strSiteIdOld: fsSiteIdOld,
            intmstInvoiceStatusId: fsInvoiceStatusId,
            excludedIDs: Helper.GetExcludedID(),
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod
        }
        
        $.ajax({
            url: "/api/CreateSuperInvoiceTower/ApproveCancel",
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
                Common.Alert.Success("Data Approved Successfully. Please wait a few seconds before all data have been fully approved.")
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            var oTableSite = $('#tblSiteData').DataTable();
            oTableSite.clear();
            $(".panelSiteData").fadeOut();
            Form.Init();
            TableDepthead.Search();
            Form.ClearRowSelected();
        });
    }
}

var Helper = {
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
    ValidateStatus: function () {
        var Result = {};
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strBapsType: fsBapsType,
            strPeriodInvoice: fsPeriodInvoice,
            strInvoiceType: fsInvoiceType,
            strRegional: fsRegional,
            strSONumber: fsSONumber,
            strBAPSNumber: fsBAPSNumber,
            strPONumber: fsPONumber,
            strSiteIdOld: fsSiteIdOld,
            intmstInvoiceStatusId: fsInvoiceStatusId,
            excludedIDs: Helper.GetExcludedID(),
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod
        };

        var l = Ladda.create(document.querySelector("#btAddSite"));
        $.ajax({
            url: "/api/CreateSuperInvoiceTower/Validate",
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
        Data.isShowPPH = Result.isShowPPH;
        Data.isCreate15 = Result.isCreate15;
        Data.isLossPPN = Result.isLossPPN;
        Data.isPPHFinal = Result.isPPHFinal;
        return Result;
    },
    InitCurrencyInput: function (selector) {
        $(selector).unbind().on("blur", function () {
            var value = $(selector).val();
            if (value != "") {
                $(selector).val(Common.Format.CommaSeparation(value));
            } else {
                $(selector).val("0.00");
            }
            Process.Calculate();
        })
    },
    GetListId: function () {
        //for CheckAll Pages
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strBapsType: fsBapsType,
            strPeriodInvoice: fsPeriodInvoice,
            strInvoiceType: fsInvoiceType,
            strRegional: fsRegional,
            strSONumber: fsSONumber,
            strBAPSNumber: fsBAPSNumber,
            strPONumber: fsPONumber,
            strSiteIdOld: fsSiteIdOld,
            intmstInvoiceStatusId: fsInvoiceStatusId,
            excludedIDs: Helper.GetExcludedID(),
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod
        };

        var AjaxData = [];
        $.ajax({
            url: "/api/CreateSuperInvoiceTower/GetListId",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            cache: false,
            data: JSON.stringify(params)
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                AjaxData = data;
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
        return AjaxData;
    },
    GetExcludedID: function () {
        var oTableSite = $('#tblSiteData').DataTable();
        var excludedIDs = [];
        oTableSite.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var id = this.data().trxArDetailId;
            excludedIDs.push(parseInt(id));
        });
        return excludedIDs;
    },
    ScrollToTop: function () {
        $("html, body").animate({ scrollTop: theOffset.top - 50 }, "slow");
        return false;
    }
}