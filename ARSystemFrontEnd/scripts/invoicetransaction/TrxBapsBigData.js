Data = {};

var fsCompanyId = "";
var fsOperatorId = "";
var fsBapsStatus = "";
var fsInvoicePeriod = "";
var fsInvoiceTypeId = "";
var fsCurrency = "";
var fsPONumber = "";
var fsBapsNumber = "";
var fsBapsType = "";
var fsIsReceive = "";
var fsSONumber = "";
var fsSiteIdOld = "";
var fsStartPeriod = "";
var fsEndPeriod = "";
var fsUserCompanyCode = "";

jQuery(document).ready(function () {
    // Modification Or Added By Ibnu Setiawan 04. September 2020
    // Add Company Code By User Login
    fsUserCompanyCode = $('#hdUserCompanyCode').val();
    // End Modification Or Added By Ibnu Setiawan 04. September 2020
    Data.RowSelectedReceive = [];
    Data.RowSelectedConfirm = [];
    Data.RowSelectedSite = [];
    Data.Mode = Mode.Receive;
    Data.ExcludedIDs = [];

    Form.Init();
    Table.Init();
    TableReceive.Init();

    TableReject.Init();
    if (fsUserCompanyCode.trim() == "PKP") {
        $("#slSearchCompanyName").val(fsUserCompanyCode).trigger("change");
    }
    TableReceive.Search();

    //panel Summary
    $("#formSearch").submit(function (e) {
        Table.Search();
        e.preventDefault();
    });

    $("#btSearch").unbind().click(function () {
        if ($("#tabBAPS").tabs('option', 'active') == 0) {
            TableReceive.Search();
        }
        else if ($("#tabBAPS").tabs('option', 'active') == 1)
            Table.Search();
        else
            TableReject.Search();

        //Data.RowSelectedReceive = [];
        //Data.RowSelectedConfirm = [];
        //Data.RowSelectedSite = [];
        //$(".panelSiteData").hide();
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
        if (fsUserCompanyCode.trim() == "PKP") {
            $("#slSearchCompanyName").val(fsUserCompanyCode).trigger("change");
        }
    });

    $(".btCancel").unbind().click(function () {
        Form.Cancel();
        Table.Reset();
    });

    $("#btConfirm").unbind().click(function () {
        //if (Data.RowSelectedConfirm.length == 0)
        //    Common.Alert.Warning("Please Select One or More Data")
        //else
        var TotalItem = Helper.GetListId().length > 0 ? Helper.GetListId().length : 0;
        var ExcludedItem = Helper.GetExcludedID().length > 0 ? Helper.GetExcludedID().length : 0;
        var ProcessedItem = TotalItem - ExcludedItem;
        $("#lblTotalItem").text(Common.Format.CommaSeparationOnly(TotalItem));
        $("#lblProcessedItem").text(Common.Format.CommaSeparationOnly(ProcessedItem));
        $("#lblExcludedItem").text(Common.Format.CommaSeparationOnly(ExcludedItem));
        $('#mdlConfirmBAPS').modal('show');
    });

    $("#btReject").unbind().click(function () {
        var TotalItem = Helper.GetListId().length > 0 ? Helper.GetListId().length : 0;
        var ExcludedItem = Helper.GetExcludedID().length > 0 ? Helper.GetExcludedID().length : 0;
        var ProcessedItem = TotalItem - ExcludedItem;
        $("#lblTotalItemReject").text(Common.Format.CommaSeparationOnly(TotalItem));
        $("#lblProcessedItemReject").text(Common.Format.CommaSeparationOnly(ProcessedItem));
        $("#lblExcludedItemReject").text(Common.Format.CommaSeparationOnly(ExcludedItem));
        Table.ResetMdlReject();
        $('#mdlRejectBAPS').modal('show');
    });

    $("#btYesConfirm").unbind().click(function () {
        Process.Confirm();
    });

    $("#formReject").submit(function (e) {
        if ($("#tbRemarks").val().includes("'")) {
            $("#spRemarks").text(" Please Avoid Using Apostrophe (') Character");
            $("#spRemarks").show();
        }
        else {
            Process.Reject();
            $("#spRemarks").hide();
        }
        e.preventDefault();
    });

    $("#btNoReject").unbind().click(function () {
        Table.ResetMdlReject();
    });

    $('#tblSummaryData').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            Data.RowSelectedConfirm.push(parseInt(id));
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row" + id).removeClass("active");
            Helper.RemoveElementFromArray(Data.RowSelectedConfirm, parseInt(id));
        }
    });

    $('#tblSummaryDataReceive').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            Data.RowSelectedReceive.push(parseInt(id));
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row" + id).removeClass("active");
            Helper.RemoveElementFromArray(Data.RowSelectedReceive, parseInt(id));
        }
    });

    $('#chRounding').on("switchChange.bootstrapSwitch", function (event, state) {
        if (state == true) {
            $('.NeedRounding').show();
            $("#tbDetailRounding").prop("disabled", false);
        }
        else {
            $('.NeedRounding').hide();
        }
    });

    $("#tbDetailRounding").unbind().on("blur", function () {
        Helper.Calculate();
    });

    $("#btUpdateAmount").unbind().click(function () {
        Process.UpdateRentalAmount();
    });

    $('#tabBAPS').unbind().on('tabsactivate', function (event, ui) {
        var newIndex = ui.newTab.index();
        if (newIndex == 0) {
            Data.RowSelectedReceive = [];
            TableReceive.Search();
            $('#btReceive').show();
            $('#btConfirm').hide();
            $('#btReject').show();
            Data.Mode = Mode.Receive;
        }
        else if (newIndex == 1) {
            Data.RowSelectedConfirm = [];
            Table.Search();
            $('#btReceive').hide();
            $('#btConfirm').show();
            $('#btReject').show();
            Data.Mode = Mode.Confirm;
        }
        else {
            TableReject.Search();
            $('#btReceive').hide();
            $('#btConfirm').hide();
            $('#btExclude').hide();
            $('#btReject').hide();
        }
        Data.RowSelectedSite = [];
        TableSite.Init();
        $(".panelSiteData").hide();
    });

    $("#btReceive").unbind().click(function () {
        Process.Receive();
    });

    $("#btExclude").unbind().click(function () {
        if ($("#tabBAPS").tabs('option', 'active') == 0) {
            Process.ExcludeReceive();
        } else {
            Process.ExcludeConfirm();
        }
        Helper.ScrollToTop();
    });


    $("#btYesReceive").unbind().click(function () {
        Process.Receive();
    });

    $("#btReceive").unbind().click(function () {
        var TotalItem = Helper.GetListId().length > 0 ? Helper.GetListId().length : 0;
        var ExcludedItem = Helper.GetExcludedID().length > 0 ? Helper.GetExcludedID().length : 0;
        var ProcessedItem = TotalItem - ExcludedItem;
        $("#lblTotalItemReceive").text(Common.Format.CommaSeparationOnly(TotalItem));
        $("#lblProcessedItemReceive").text(Common.Format.CommaSeparationOnly(ProcessedItem));
        $("#lblExcludedItemReceive").text(Common.Format.CommaSeparationOnly(ExcludedItem));
        $('#mdlReceiveBAPS').modal('show');
    });

});

var Form = {
    Init: function () {
        $("#formReject").parsley();

        $(".panelSiteData").hide();

        if (!$("#hdAllowProcess").val()) {
            $("#btConfirm").hide();
            //$("#btReject").hide();
            $("#btReceive").hide();
        }

        $("#slSearchInvoiceType").select2({ placeholder: "Select Invoice Type", width: null });
        $("#slSearchCurrency").select2({ placeholder: "Select Currency", width: null });

        Control.BindingSelectCompany();
        Control.BindingSelectOperator();
        Control.BindingSelectInvoiceType();
        Control.BindingSelectRejectHdr();
        Control.BindingSelectRejectDtl();
        //Control.BindingSelectDepartment();
        Control.BindingSelectBapsType();
        Table.Reset();
        $('#tabBAPS').tabs();
        $("#btConfirm").hide();
        $("#spRemarks").hide();
        //$("body").delegate(".date-picker", "focusin", function () {
        //    $(this).datepicker({
        //        format: "dd-M-yyyy"
        //    });
        //});
    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
    },
    DoneConfirm: function () {
        $("#pnlSummary").fadeIn();
        $('#mdlConfirmBAPS').modal('toggle');
        $(".panelSiteData").fadeOut();
        Table.Search();
        $('#btReceive').hide();
        $('#btConfirm').show();
    },
    DoneReject: function () {
        $("#pnlSummary").fadeIn();
        $('#mdlRejectBAPS').modal('toggle');
        if ($("#tabBAPS").tabs('option', 'active') == 0) {
            TableReceive.Search();
            $('#btReceive').show();
            $('#btConfirm').hide();
        }
        else {
            Table.Search();
            $('#btReceive').hide();
            $('#btConfirm').show();
        }
        //Table.Reset();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $('#mdlDetail').modal('toggle');
        Table.Search();
        // Table.Reset();
        $('#btReceive').show();
        $('#btConfirm').hide();
    },
    DoneReceive: function () {

        $(".panelSiteData").fadeOut();
        //Form.Init();
        TableReceive.Search();
        $('#mdlReceiveBAPS').modal('toggle');
    }
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

        $("#tblSummaryData tbody").on("click", "a.btDetail", function (e) {
            e.preventDefault();
            var table = $("#tblSummaryData").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                $('#mdlDetail').modal('toggle');
                Table.ShowDetail();
            }
        });

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsOperatorId = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsInvoicePeriod = $("#slSearchInvoiceType").val() == null ? "" : $("#slSearchInvoiceType").val();
        fsInvoiceTypeId = $("#slSearchTermInvoice").val() == null ? "" : $("#slSearchTermInvoice").val();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
        fsPONumber = $("#tbSearchPONumber").val();
        fsBapsNumber = $("#tbSearchBAPSNumber").val();
        fsSONumber = $("#tbSearchSONumber").val();
        fsSiteIdOld = $("#tbSearchSiteIdOld").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsIsReceive = 0;
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();

        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperatorId,
            strStatusBAPS: fsBapsStatus,
            strPeriodInvoice: fsInvoicePeriod,
            strInvoiceType: fsInvoiceTypeId,
            strCurrency: fsCurrency,
            strPONumber: fsPONumber,
            strBAPSNumber: fsBapsNumber,
            strSONumber: fsSONumber,
            strBapsType: fsBapsType,
            strSiteIdOld: fsSiteIdOld,
            isReceive: 18,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/BapsData/grid",
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
                        {
                            if (Helper.IsElementExistsInArray(parseInt(full.trxBapsDataId), Data.RowSelectedConfirm)) {
                                if (Helper.IsElementExistsInArray(parseInt(full.trxBapsDataId), Data.RowSelectedSite)) {
                                    strReturn += "<label style='display:none;' id='" + full.trxBapsDataId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxBapsDataId + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                } else {
                                    strReturn += "<label id='" + full.trxBapsDataId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxBapsDataId + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                }
                            }
                            else {
                                strReturn += "<label id='" + full.trxBapsDataId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxBapsDataId + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                },
                {
                    data: "SONumber", mRender: function (data, type, full) {
                        return "<a href='' class='btDetail'>" + data + "</a>";
                    }
                },
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
                { data: "PPHType" },
                {
                    data: "IsLossPPN", mRender: function (data, type, full) {
                        return full.IsLossPPN ? "Loss" : "Claim";
                    }
                }
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelectedConfirm.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelectedConfirm.length; i++) {
                        item = Data.RowSelectedConfirm[i];
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
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                                    '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tblSummaryData .checkboxes" />' +
                                    '<span></span> ' +
                                '</label>';
                var th = $("th.select-all-confirm").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-confirm").unbind().on("change", ".group-checkable", function (e) {
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
                        Data.RowSelectedConfirm = Data.RowSelectedConfirm.concat(Helper.GetListId());
                    else {
                        $.each(Helper.GetListId(), function (index, item) {
                            Helper.RemoveElementFromArray(Data.RowSelectedConfirm, parseInt(item));
                        });
                    }
                });
            }
        });
    },
    Reset: function () {
        fsCompanyId = "";
        fsOperator = "";
        fsBapsStatus = "";
        fsInvoicePeriod = "";
        fsInvoiceTypeId = "";
        fsCurrency = "";
        fsPONumber = "";
        fsBapsNumber = "";
        fsBapsType = "";
        fsIsReceive = (Data.Mode == Mode.Receive) ? 1 : 0;
        fsSONumber = "";
        fsSiteIdOld = "";
        fsStartPeriod = "";
        fsEndPeriod = "";

        $("#slSearchCompanyName").val("").trigger('change');
        $("#slSearchOperator").val("").trigger('change');
        $("#slSearchTermInvoice").val("").trigger('change');
        $("#slSearchInvoiceType").val("").trigger('change');
        $("#slSearchCurrency").val("").trigger('change');
        $("#tbSearchPONumber").val("");
        $("#tbSearchBAPSNumber").val("");
        $("#tbSearchSONumber").val("");
        $("#tbSearchSiteIdOld").val("");
        $("#slSearchBapsType").val("").trigger('change');
        $("#tbStartPeriod").val("");
        $("#tbEndPeriod").val("");
    },
    ResetMdlReject: function () {
        $("#slHeaderDescription").val("").trigger('change');
        $("#slCategory").val("").trigger('change');
        //$("#slPIC").val("").trigger('change');
        $("#tbRemarks").val("");
        $("#tbEmailRecipient").val("");
        $("#tbEmailCC").val("");

    },
    ShowDetail: function () {
        $('#tbDetailSONumber').val(Data.Selected.SONumber);
        $('#tbDetailSiteId').val(Data.Selected.SiteIdOld);
        $('#tbDetailMasterPrice').val("");
        $('#tbDetailSiteIdOpr').val(Data.Selected.SiteIdOpr);
        $('#tbDetailCompanyInvoice').val(Data.Selected.CompanyInvoice);
        $('#tbDetailStartDate').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartDate));
        $('#tbDetailStartDateRec').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartDateInvoice));
        $('#tbDetailSiteName').val(Data.Selected.SiteName);
        $('#tbDetailOprInvoice').val(Data.Selected.Operator);
        $('#tbDetailEndDate').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndDate));
        $('#tbDetailEndDateRec').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndDateInvoice));

        $('#tbDetailTermInvoice').val(Data.Selected.InvoiceTypeDesc);
        $('#tbDetailAmountRental').val(Common.Format.CommaSeparation(Data.Selected.AmountRental.toString()));
        $('#tbDetailAmountService').val(Common.Format.CommaSeparation(Data.Selected.AmountService.toString()));
        $('#tbDetailAmountInvoice').val(Common.Format.CommaSeparation(Data.Selected.InvoiceAmount.toString()));
        $('#tbDetailAmountPenalty').val(Common.Format.CommaSeparation(Data.Selected.AmountPenaltyPeriod.toString()));
        $('#tbDetailAmountOverdaya').val(Common.Format.CommaSeparation(Data.Selected.AmountOverdaya.toString()));
        $('#tbDetailAmountOverblast').val(Common.Format.CommaSeparation(Data.Selected.AmountOverblast.toString()));
        $('#tbDetailAmountInflasi').val(Common.Format.CommaSeparation("0"));
        $('#tbDetailAmountDiscount').val(Common.Format.CommaSeparation("0"));
        $("#tbDetailRounding").val(Common.Format.CommaSeparation("0"));
        $('#tbDetailCurrency').val(Data.Selected.Currency);
        $("#formDetailData :input").prop("disabled", true);
        $("#chRounding").bootstrapSwitch("state", false);
        Data.SelectedInvoiceAmount = Data.Selected.InvoiceAmount;
        $(".divRounding").removeAttr("style");
        $("#btUpdateAmount").hide();
    },
    Export: function () {
        window.location.href = "/InvoiceTransaction/TrxBapsConfirm/Export?strCompanyId=" + fsCompanyId + "&strOperator=" + fsOperatorId
            + "&strStatusBAPS=" + fsBapsStatus + "&strPeriodInvoice=" + fsInvoicePeriod + "&strInvoiceType=" + fsInvoiceTypeId + "&strCurrency=" + fsCurrency
        + "&strPONumber=" + fsPONumber + "&strBAPSNumber=" + fsBapsNumber + "&strSONumber=" + fsSONumber + "&strBapsType=" + fsBapsType + "&strSiteIdOld=" + fsSiteIdOld + "&isReceive=18"
        + "&strStartPeriod=" + fsStartPeriod + "&strEndPeriod=" + fsEndPeriod;
    }
}

var TableReceive = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblSummaryDataReceive = $('#tblSummaryDataReceive').DataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });
        $("#tblSummaryDataReceive tbody").on("click", "a.btDetail", function (e) {
            e.preventDefault();
            var table = $("#tblSummaryDataReceive").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                $('#mdlDetail').modal('toggle');
                TableReceive.ShowDetail();
            }
        });
        $(window).resize(function () {
            $("#tblSummaryDataReceive").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsOperatorId = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsInvoicePeriod = $("#slSearchInvoiceType").val() == null ? "" : $("#slSearchInvoiceType").val();
        fsInvoiceTypeId = $("#slSearchTermInvoice").val() == null ? "" : $("#slSearchTermInvoice").val();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
        fsPONumber = $("#tbSearchPONumber").val();
        fsBapsNumber = $("#tbSearchBAPSNumber").val();
        fsSONumber = $("#tbSearchSONumber").val();
        fsSiteIdOld = $("#tbSearchSiteIdOld").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsIsReceive = 1;
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();

        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperatorId,
            strStatusBAPS: fsBapsStatus,
            strPeriodInvoice: fsInvoicePeriod,
            strInvoiceType: fsInvoiceTypeId,
            strCurrency: fsCurrency,
            strPONumber: fsPONumber,
            strBAPSNumber: fsBapsNumber,
            strSONumber: fsSONumber,
            strBapsType: fsBapsType,
            strSiteIdOld: fsSiteIdOld,
            isReceive: fsIsReceive,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod
        };
        var tblSummaryData = $("#tblSummaryDataReceive").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/BapsData/grid",
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
                        TableReceive.Export()
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
                            if (Helper.IsElementExistsInArray(parseInt(full.trxBapsDataId), Data.RowSelectedReceive)) {
                                if (Helper.IsElementExistsInArray(parseInt(full.trxBapsDataId), Data.RowSelectedSite)) {
                                    strReturn += "<label style='display:none;' id='" + full.trxBapsDataId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxBapsDataId + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                } else {
                                    //$("#Row" + full.trxBapsDataId).addClass("active");
                                    strReturn += "<label id='" + full.trxBapsDataId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxBapsDataId + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                }
                            }
                            else {
                                strReturn += "<label id='" + full.trxBapsDataId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxBapsDataId + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                },
                {
                    data: "SONumber", mRender: function (data, type, full) {
                        return "<a class='btDetail'>" + data + "</a>";
                    }
                },
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
                { data: "PPHType" },
                {
                    data: "IsLossPPN", mRender: function (data, type, full) {
                        return full.IsLossPPN ? "Loss" : "Claim";
                    }
                }
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelectedReceive.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelectedReceive.length; i++) {
                        item = Data.RowSelectedReceive[i];
                        if (!Helper.IsElementExistsInArray(parseInt(item), Data.RowSelectedSite))
                            $("#Row" + item).addClass("active");
                    }
                }
            },
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (aData.IsLossPPN == 1) {
                    if (aData.AmountPPN != aData.AmountLossPPN) {
                        $('td', nRow).css('background-color', '#FF9999');
                    }
                }
                l.stop();
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
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                                    '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tblSummaryDataReceive .checkboxes" />' +
                                    '<span></span> ' +
                                '</label>';
                var th = $("th.select-all-receive").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-receive").unbind().on("change", ".group-checkable", function (e) {
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
                        Data.RowSelectedReceive = Data.RowSelectedReceive.concat(Helper.GetListId());
                    else {
                        $.each(Helper.GetListId(), function (index, item) {
                            Helper.RemoveElementFromArray(Data.RowSelectedReceive, parseInt(item));
                        });
                    }
                });
            }
        });
    },
    ShowDetail: function () {
        $('#tbDetailSONumber').val(Data.Selected.SONumber);
        $('#tbDetailSiteId').val(Data.Selected.SiteIdOld);
        $('#tbDetailMasterPrice').val("");
        $('#tbDetailSiteIdOpr').val(Data.Selected.SiteIdOpr);
        $('#tbDetailCompanyInvoice').val(Data.Selected.CompanyInvoice);
        $('#tbDetailStartDate').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartDate));
        $('#tbDetailStartDateRec').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartDateInvoice));
        $('#tbDetailSiteName').val(Data.Selected.SiteName);
        $('#tbDetailOprInvoice').val(Data.Selected.Operator);
        $('#tbDetailEndDate').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndDate));
        $('#tbDetailEndDateRec').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndDateInvoice));

        $('#tbDetailTermInvoice').val(Data.Selected.InvoiceTypeDesc);
        $('#tbDetailAmountRental').val(Common.Format.CommaSeparation(Data.Selected.AmountRental.toString()));
        $('#tbDetailAmountService').val(Common.Format.CommaSeparation(Data.Selected.AmountService.toString()));
        $('#tbDetailAmountInvoice').val(Common.Format.CommaSeparation(Data.Selected.InvoiceAmount.toString()));
        $('#tbDetailAmountPenalty').val(Common.Format.CommaSeparation(Data.Selected.AmountPenaltyPeriod.toString()));
        $('#tbDetailAmountOverdaya').val(Common.Format.CommaSeparation(Data.Selected.AmountOverdaya.toString()));
        $('#tbDetailAmountOverblast').val(Common.Format.CommaSeparation(Data.Selected.AmountOverblast.toString()));
        $('#tbDetailAmountInflasi').val(Common.Format.CommaSeparation("0"));
        $('#tbDetailAmountDiscount').val(Common.Format.CommaSeparation("0"));
        $("#tbDetailRounding").val(Common.Format.CommaSeparation("0"));
        $('#tbDetailCurrency').val(Data.Selected.Currency);
        $("#formDetailData :input").prop("disabled", true);
        $(".divRounding").attr("style", "display:none");
        $("#btUpdateAmount").hide();
        Data.SelectedInvoiceAmount = Data.Selected.InvoiceAmount;
    },
    Export: function () {
        window.location.href = "/InvoiceTransaction/TrxBapsConfirm/Export?strCompanyId=" + fsCompanyId + "&strOperator=" + fsOperatorId
            + "&strStatusBAPS=" + fsBapsStatus + "&strPeriodInvoice=" + fsInvoicePeriod + "&strInvoiceType=" + fsInvoiceTypeId + "&strCurrency=" + fsCurrency
        + "&strPONumber=" + fsPONumber + "&strBAPSNumber=" + fsBapsNumber + "&strSONumber=" + fsSONumber + "&strBapsType=" + fsBapsType + "&strSiteIdOld=" + fsSiteIdOld + "&isReceive=1"
        + "&strStartPeriod=" + fsStartPeriod + "&strEndPeriod=" + fsEndPeriod;
    }
}
var TableReject = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblSummaryDataReject = $('#tblSummaryDataReject').DataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });
        $(window).resize(function () {
            $("#tblSummaryDataReceive").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsOperatorId = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsInvoicePeriod = $("#slSearchInvoiceType").val() == null ? "" : $("#slSearchInvoiceType").val();
        fsInvoiceTypeId = $("#slSearchTermInvoice").val() == null ? "" : $("#slSearchTermInvoice").val();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
        fsPONumber = $("#tbSearchPONumber").val();
        fsBapsNumber = $("#tbSearchBAPSNumber").val();
        fsSONumber = $("#tbSearchSONumber").val();
        fsSiteIdOld = $("#tbSearchSiteIdOld").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();

        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperatorId,
            strStatusBAPS: fsBapsStatus,
            strPeriodInvoice: fsInvoicePeriod,
            strInvoiceType: fsInvoiceTypeId,
            strCurrency: fsCurrency,
            strPONumber: fsPONumber,
            strBAPSNumber: fsBapsNumber,
            strSONumber: fsSONumber,
            strBapsType: fsBapsType,
            strSiteIdOld: fsSiteIdOld,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod
        };
        var tblSummaryData = $("#tblSummaryDataReject").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/BapsData/gridReject",
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
                        TableReject.Export()
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
                    data: "SONumber"
                },
                {
                    data: "Description"
                },
                {
                    data: "RejectRemarks"
                },
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
                { data: "PPHType" },
                {
                    data: "IsLossPPN", mRender: function (data, type, full) {
                        return full.IsLossPPN ? "Loss" : "Claim";
                    }
                }
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');


            },
            "order": [1, "asc"],
            "scrollY": 300,
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            }
        });
    },
    Export: function () {
        window.location.href = "/InvoiceTransaction/TrxBapsReject/Export?strCompanyId=" + fsCompanyId + "&strOperator=" + fsOperatorId
            + "&strStatusBAPS=" + fsBapsStatus + "&strPeriodInvoice=" + fsInvoicePeriod + "&strInvoiceType=" + fsInvoiceTypeId + "&strCurrency=" + fsCurrency
        + "&strPONumber=" + fsPONumber + "&strBAPSNumber=" + fsBapsNumber + "&strSONumber=" + fsSONumber + "&strBapsType=" + fsBapsType + "&strSiteIdOld=" + fsSiteIdOld
        + "&strStartPeriod=" + fsStartPeriod + "&strEndPeriod=" + fsEndPeriod;
    }
}

var TableSite = {
    Init: function () {
        var tableSite = $("#tblSiteData").DataTable();
        tableSite.clear();
    },
    AddSite: function (data) {
        //Get Data that in RowSelected
        Data.ExcludedIDs = Helper.GetExcludedID();

        $.each(data, function (index, item) {
            Data.ExcludedIDs.push(parseInt(item));
        });

        var AjaxData = [];
        var params = {
            excludedIDs: Data.ExcludedIDs
        };

        var l = Ladda.create(document.querySelector("#btExclude"));
        $.ajax({
            url: "/api/BapsBigData/SitelistGrid",
            type: "POST",
            dataType: "json",
            async: true,
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                var tblSiteData = $("#tblSiteData").DataTable({
                    "deferRender": true,
                    "proccessing": true,
                    "data": data,
                    "filter": false,
                    "destroy": true,
                    "columns": [
                        {
                            mRender: function (data, type, full) {
                                var strReturn = "";
                                strReturn += "<button type='button' title='Delete' class='btn btn-xs red btDeleteSite' id='btDeleteSite" + full.trxBapsDataId + "'><i class='fa fa-trash'></i></button>";
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
                        //    mRender: function (data, type, full) {
                        //        return full.IsPartial ? "Yes" : "No";
                        //    }
                        //},
                        {
                            mRender: function (data, type, full) {
                                return full.IsLossPPN ? "Yes" : "No";
                            }
                        },
                        { data: "PPHType" },
                {
                    data: "IsLossPPN", mRender: function (data, type, full) {
                        return full.IsLossPPN ? "Loss" : "Claim";
                    }
                }
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
                    if (Data.Mode == Mode.Receive) {
                        Helper.RemoveElementFromArray(Data.RowSelectedReceive, parseInt(id));
                        if (Data.RowSelectedReceive.length == 0)
                            $(".panelSiteData").fadeOut();
                    }
                    else {
                        Helper.RemoveElementFromArray(Data.RowSelectedConfirm, parseInt(id));
                        if (Data.RowSelectedConfirm.length == 0)
                            $(".panelSiteData").fadeOut();
                    }

                });

                $(window).resize(function () {
                    $("#tblSiteData").DataTable().columns.adjust().draw();
                });
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        });
    }
}

var Process = {
    Receive: function () {
        var l = Ladda.create(document.querySelector("#btReceive"))
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperatorId,
            strStatusBaps: fsBapsStatus,
            strPeriodInvoice: fsInvoicePeriod,
            strInvoiceType: fsInvoiceTypeId,
            strCurrency: fsCurrency,
            strPONumber: fsPONumber,
            strBapsNumber: fsBapsNumber,
            strSONumber: fsSONumber,
            strBapsType: fsBapsType,
            strSiteIdOld: fsSiteIdOld,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            isReceive: fsIsReceive,
            excludedIDs: Helper.GetExcludedID()
        }

        $.ajax({
            url: "/api/BapsBigData/Receive",
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
                if (data.ErrorMessage == "") {
                    Common.Alert.Success("Data Success Received. Please wait a few seconds before all of the data are fully received.")
                    Data.RowSelectedReceive = [];
                    Form.DoneReceive();
                } else {
                    Common.Alert.Warning(data.ErrorMessage);
                }
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
            $(".panelSiteData").fadeOut();
            Form.Init();
            TableReceive.Search();
            //Table.Reset();
        })
        .always(function (jqXHR, textStatus) {
        });
    },
    Confirm: function () {
        Data.ConfirmBAPS = [];
        var l = Ladda.create(document.querySelector("#btYesConfirm"))
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperatorId,
            strStatusBaps: fsBapsStatus,
            strPeriodInvoice: fsInvoicePeriod,
            strInvoiceType: fsInvoiceTypeId,
            strCurrency: fsCurrency,
            strPONumber: fsPONumber,
            strBapsNumber: fsBapsNumber,
            strSONumber: fsSONumber,
            strBapsType: fsBapsType,
            strSiteIdOld: fsSiteIdOld,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            isReceive: fsIsReceive,
            excludedIDs: Helper.GetExcludedID()
        }

        $.ajax({
            url: "/api/BapsBigData/Confirm",
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
                if (data.ErrorMessage == "") {
                    Common.Alert.Success("Data Confirmed Successfully. Please wait a few seconds before all of the data are fully confirmed.");
                    Data.RowSelectedConfirm = [];
                    Form.DoneConfirm();
                } else {
                    Common.Alert.Warning(data.ErrorMessage);
                }
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
            Form.DoneConfirm();
        });
    },
    Reject: function () {
        Data.RejectBAPS = [];
        var l = Ladda.create(document.querySelector("#btYesReject"))
        var indexTab = $("#tabBAPS").tabs('option', 'active');

        var listIDs = Helper.GetListId();
        var excludedIDs = Helper.GetExcludedID();
        var includedIDs = removeItem(listIDs, excludedIDs);

        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperatorId,
            strStatusBaps: fsBapsStatus,
            strPeriodInvoice: fsInvoicePeriod,
            strInvoiceType: fsInvoiceTypeId,
            strCurrency: fsCurrency,
            strPONumber: fsPONumber,
            strBapsNumber: fsBapsNumber,
            strSONumber: fsSONumber,
            strBapsType: fsBapsType,
            strSiteIdOld: fsSiteIdOld,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            isReceive: fsIsReceive,
            excludedIDs: excludedIDs,
            includedIDs: includedIDs,
            Remarks: $("#tbRemarks").val(),
            MstRejectDtlId: $("#slCategory").val()
        }

        $.ajax({
            url: "/api/BapsBigData/Reject",
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
                console.log(data);
                if (data.Result == "") {
                    Common.Alert.Success("Data Rejected Successfully. Please wait a few seconds before all of the data are fully rejected.");
                    Data.RowSelectedReceive = [];
                    Data.RowSelectedConfirm = [];
                    $(".panelSiteData").fadeOut();

                    Form.DoneReject();
                } else {
                    $('#mdlRejectBAPS').modal('toggle');
                    Table.ResetMdlReject();
                    Common.Alert.Warning(data.ErrorMessage);
                }
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
            $(".panelSiteData").fadeOut();
            Form.Init();
            Form.DoneReject();
        });
    },
    UpdateRentalAmount: function () {
        var RoundingAmount = parseFloat($("#tbDetailRounding").val().replace(/,/g, ""));

        if (RoundingAmount == null || RoundingAmount == undefined || RoundingAmount == "")
            RoundingAmount = 0;

        var l = Ladda.create(document.querySelector("#btUpdateAmount"))
        $.ajax({
            url: "/api/BapsData/" + Data.Selected.trxBapsDataId + "/" + RoundingAmount,
            type: "PUT",
            dataType: "json",
            contentType: "application/json",
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Amount Invoice has been edited!")
                Form.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    },
    ExcludeReceive: function () {
        var l = Ladda.create(document.querySelector("#btExclude"))
        var oTable = $('#tblSummaryDataReceive').DataTable();
        var oTableSite = $('#tblSiteData').DataTable();
        if (Data.RowSelectedReceive.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            TableSite.AddSite(Data.RowSelectedReceive);
            $(".panelSiteData").fadeIn();
            oTable.rows('.active').every(function (rowIdx, tableLoop, rowLoop) {
                var checkBoxId = this.data().trxBapsDataId;
                $("#Row" + checkBoxId).removeClass('active');
                $("#" + checkBoxId).hide();
            });

            $.each(Data.RowSelectedReceive, function (index, item) {
                $(".Row" + item).removeClass('active');
                $("." + item).hide();
            });

            $.each(Data.RowSelectedReceive, function (index, item) {
                if (Data.RowSelectedSite.indexOf(parseInt(item)) == -1)
                    Data.RowSelectedSite.push(parseInt(item));
            });
            //Data.RowSelectedSite = Data.RowSelectedReceive;
        }
        l.stop();
    },
    ExcludeConfirm: function () {
        var l = Ladda.create(document.querySelector("#btExclude"))
        var oTable = $('#tblSummaryData').DataTable();
        var oTableSite = $('#tblSiteData').DataTable();
        if (Data.RowSelectedConfirm.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            TableSite.AddSite(Data.RowSelectedConfirm);
            $(".panelSiteData").fadeIn();
            oTable.rows('.active').every(function (rowIdx, tableLoop, rowLoop) {
                var checkBoxId = this.data().trxBapsDataId;
                $("#Row" + checkBoxId).removeClass('active');
                $("#" + checkBoxId).hide();
            });
            $.each(Data.RowSelectedConfirm, function (index, item) {
                $(".Row" + item).removeClass('active');
                $("." + item).hide();
            });

            $.each(Data.RowSelectedConfirm, function (index, item) {
                if (Data.RowSelectedSite.indexOf(item) == -1)
                    Data.RowSelectedSite.push(parseInt(item));
            });
            //Data.RowSelectedSite = Data.RowSelectedConfirm;
        }
        l.stop();
    }
}

var Control = {
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
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
    BindingSelectRejectHdr: function () {
        $.ajax({
            url: "/api/BapsData/RejectHdr",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slHeaderDescription").html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slHeaderDescription").append("<option value='" + item.mstPICATypeID + "'>" + item.Description + "</option>");
                })
            }

            $("#slHeaderDescription").select2({ placeholder: "Select Reject Description Type", width: null }).on("change", function (e) {
                Control.BindingSelectRejectDtl();
                Control.SetEmailIntoTextArea();
            });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectRejectDtl: function () {
        var params = {
            HdrId: $("#slHeaderDescription").val() == null || $("#slHeaderDescription").val() == "" ? 0 : $("#slHeaderDescription").val()
        };
        $.ajax({
            url: "/api/BapsData/RejectDtl",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slCategory").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slCategory").append("<option value='" + item.mstPICADetailID + "'>" + item.Description + "</option>");
                })
            }

            $("#slCategory").select2({ placeholder: "Select Category", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    //BindingSelectDepartment: function () {
    //    $.ajax({
    //        url: "/api/MstDataSource/Department",
    //        type: "GET"
    //    })
    //    .done(function (data, textStatus, jqXHR) {
    //        $("#slPIC").html("<option></option>")

    //        if (Common.CheckError.List(data)) {
    //            $.each(data, function (i, item) {
    //                $("#slPIC").append("<option value='" + item.DepartmentCode + "_" + item.DepartmentName + "'>" + item.DepartmentCode + " - " + item.DepartmentName + "</option>");
    //            })
    //        }

    //        $("#slPIC").select2({ placeholder: "Select PIC Department", width: null });

    //    })
    //    .fail(function (jqXHR, textStatus, errorThrown) {
    //        Common.Alert.Error(errorThrown);
    //    });
    //},
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
    SetEmailIntoTextArea: function () {
        var params = {
            HdrId: $("#slHeaderDescription").val() == null || $("#slHeaderDescription").val() == "" ? 0 : $("#slHeaderDescription").val()
        };
        $.ajax({
            url: "/api/BapsData/PICAEmails",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $("#tbEmailRecipient").val(data.Recipient);
                $("#tbEmailCC").val(data.CC);
            }


        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    }

}

var Helper = {
    Calculate: function () {
        var InvoiceAmount = parseFloat($("#tbDetailAmountInvoice").val().replace(/,/g, ""));
        var AmountRounding = parseFloat($("#tbDetailRounding").val().toString().replace(/,/g, ""));
        var TotalAmount = 0;
        if (!isNaN(AmountRounding))
            TotalAmount = InvoiceAmount + AmountRounding;
        else
            TotalAmount = Data.SelectedInvoiceAmount;
        $("#tbDetailAmountInvoice").val(Common.Format.CommaSeparation(TotalAmount.toString()));
        $("#tbDetailRounding").val(Common.Format.CommaSeparation(AmountRounding.toString()));
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
    GetListId: function () {
        //for CheckAll Pages
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperatorId,
            strStatusBaps: fsBapsStatus,
            strPeriodInvoice: fsInvoicePeriod,
            strInvoiceType: fsInvoiceTypeId,
            strCurrency: fsCurrency,
            strPONumber: fsPONumber,
            strBapsNumber: fsBapsNumber,
            strSONumber: fsSONumber,
            strBapsType: fsBapsType,
            strSiteIdOld: fsSiteIdOld,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            isReceive: fsIsReceive,
            excludedIDs: Helper.GetExcludedID()
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/BapsBigData/GetListId",
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
            var id = this.data().trxBapsDataId;
            excludedIDs.push(parseInt(id));
        });
        return excludedIDs;
    },
    ScrollToTop: function () {
        $("html, body").animate({ scrollTop: theOffset.top - 50 }, "slow");
        return false;
    },
}

var Mode = {
    Receive: "Receive",
    Confirm: "Confirm"
}

function removeItem(arrBase, arrValue) {
    var j = 0;
    for (j = 0; j < arrValue.length; j++) {
        var i = 0;
        while (i < arrBase.length) {
            if (arrBase[i] === arrValue[j]) {
                arrBase.splice(i, 1);
            } else {
                ++i;
            }
        }
    }
    return arrBase;
}