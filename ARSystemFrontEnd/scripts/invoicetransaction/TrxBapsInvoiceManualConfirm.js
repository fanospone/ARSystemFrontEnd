Data = {};

//filter search 
var fsCompanyId = "";
var fsOperator = "";
var fsStatusBAPS = "";
var fsPeriodInvoice = "";
var fsInvoiceType = "";
var fsCurrency = "";
var fsPONumber = "";
var fsBAPSNumber = "";
var fsSONumber = "";
var fsBapsType = "";
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
        if ($("#tabBAPS").tabs('option', 'active') == 0)
            TableReceive.Search();
        else if ($("#tabBAPS").tabs('option', 'active') == 1)
            Table.Search();
        else
            TableReject.Search();

    });

    $("#btReset").unbind().click(function () {
        Form.Reset();
        if (fsUserCompanyCode.trim() == "PKP") {
            $("#slSearchCompanyName").val(fsUserCompanyCode).trigger("change");
        }
    });

    $(".btCancel").unbind().click(function () {
        Form.Cancel();
        Table.Reset();
    });

    $("#btConfirm").unbind().click(function () {
        if (Data.RowSelectedConfirm.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else
            $('#mdlConfirmBAPS').modal('show');

    });

    $("#btReject").unbind().click(function () {
        if ($("#tabBAPS").tabs('option', 'active') == 0) {
            if (Data.RowSelectedReceive.length == 0)
                Common.Alert.Warning("Please Select One or More Data");
            else {
                Table.ResetMdlReject();
                $('#mdlRejectBAPS').modal('show');
            }
        }
        else {
            if (Data.RowSelectedConfirm.length == 0)
                Common.Alert.Warning("Please Select One or More Data");
            else {
                Table.ResetMdlReject();
                $('#mdlRejectBAPS').modal('show');
            }
        }
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

    $('#tblSummaryData').find('.group-checkable').change(function () {
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
        //if (checked)
        //    Data.RowSelectedConfirm = Helper.GetListId(0);
        //else
        //    Data.RowSelectedConfirm = [];
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

    $('#tblSummaryDataReceive').find('.group-checkable').change(function () {
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
        //if(checked)
        //    Data.RowSelectedReceive = Helper.GetListId(1);
        //else
        //    Data.RowSelectedReceive = [];
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
            $("#tbDetailRounding").val("0.00");
        }
        else {
            $('.NeedRounding').hide();
            $("#tbDetailAmountInvoice").val(Common.Format.CommaSeparation(Data.Selected.InvoiceAmount.toString()));//Note
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
            $('#btReject').show();
            $('#btConfirm').hide();

        }
        else if (newIndex == 1) {
            Data.RowSelectedConfirm = [];
            Table.Search();
            $('#btReceive').hide();
            $('#btConfirm').show();
            $('#btReject').show();
        }
        else {
            TableReject.Search();
            $('#btReceive').hide();
            $('#btConfirm').hide();
            $('#btReject').hide();
        }
    });

    $("#btReceive").unbind().click(function () {
        Process.Receive();
    });
});

var Form = {
    Init: function () {
        $("#formReject").parsley();

        if (!$("#hdAllowProcess").val()) {
            $("#btConfirm").hide();
            $("#btReject").hide();
            $("#btReceive").hide();
        }
        $("body").delegate(".date-picker", "focusin", function () {
            $(this).datepicker({
                format: "dd-M-yyyy"
            });
        });
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
        $('#btReceive').show();
        $('#btReject').show();
        $('#btConfirm').hide();
        $("#spRemarks").hide();
        //$("body").delegate(".date-picker", "focusin", function () {
        //    $(this).datepicker({
        //        format: "dd-M-yyyy"
        //    });
        //});
    },
    Reset: function () {
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
        fsCompanyId = "";
        fsOperator = "";
        fsStatusBAPS = "";
        fsPeriodInvoice = "";
        fsInvoiceType = "";
        fsCurrency = "";
        fsPONumber = "";
        fsBAPSNumber = "";
        fsSONumber = "";
        fsBapsType = "";
        fsSiteIdOld = "";
        fsStartPeriod = "";
        fsEndPeriod = "";
    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
    },
    DoneConfirm: function () {
        $("#pnlSummary").fadeIn();
        $('#mdlConfirmBAPS').modal('toggle');
        Table.Search();
        Table.Reset();

    },
    DoneReject: function () {
        $("#pnlSummary").fadeIn();
        $('#mdlRejectBAPS').modal('toggle');
        if ($("#tabBAPS").tabs('option', 'active') == 0) {
            TableReceive.Search();
            Table.Reset();
        }
        else {
            Table.Search();
            Table.Reset();
        }

    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $('#mdlDetail').modal('toggle');
        Table.Search();
        Table.Reset();
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
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsPeriodInvoice = $("#slSearchInvoiceType").val() == null ? "" : $("#slSearchInvoiceType").val();
        fsInvoiceType = $("#slSearchTermInvoice").val() == null ? "" : $("#slSearchTermInvoice").val();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
        fsPONumber = $("#tbSearchPONumber").val();
        fsBAPSNumber = $("#tbSearchBAPSNumber").val();
        fsSONumber = $("#tbSearchSONumber").val();
        fsSiteIdOld = $("#tbSearchSiteIdOld").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strStatusBAPS: fsStatusBAPS,
            strPeriodInvoice: fsPeriodInvoice,
            strInvoiceType: fsInvoiceType,
            strCurrency: fsCurrency,
            strPONumber: fsPONumber,
            strBAPSNumber: fsBAPSNumber,
            strSONumber: fsSONumber,
            strBapsType: fsBapsType,
            strSiteIdOld: fsSiteIdOld,
            isReceive: 1,
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
                "url": "/api/TrxInvoiceManual/gridBAPSReceive",
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
                            if (Helper.IsElementExistsInArray(full.trxInvoiceManualTempID, Data.RowSelectedReceive)) {
                                //$("#Row" + full.trxBapsDataId).addClass("active");
                                strReturn += "<label id='" + full.trxInvoiceManualTempID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxInvoiceManualTempID + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                            }
                            else {
                                strReturn += "<label id='" + full.trxInvoiceManualTempID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxInvoiceManualTempID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
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
                { data: "SiteIDOpr" },
                 { data: "SiteID" },
                { data: "SiteNameOpr" },
                { data: "CompanyID" },
                 { data: "OperatorName" },
                { data: "InitialPONumber" },
                {
                    data: "InvoiceStartDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "InvoiceEndDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                 { data: "BapsNO" },
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "OMPrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                 { data: "InvoiceAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                 {
                     data: "StartLeaseDate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                {
                    data: "EndLeaseDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                 { data: "AmountPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                  { data: "AmountLossPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                 { data: "CompanyID" },
                  { data: "OperatorName" },
                  { data: "InvoiceType" },
                   { data: "InvoiceTerm" },
                   { data: "BapsType" },
                   { data: "PowerType" },
                    { data: "PriceCurrency" },
                    { data: "PPHType" },
                     {
                         data: "LossPNN", mRender: function (data, type, full) {
                             return full.LossPNN ? "Loss" : "Claim";
                         }
                     }
                //     ,
                //{ data: "StipSiro", className: "text-right" },
                //{ data: "BapsPeriod" },
                //{ data: "MLANumber" },
                //{ data: "SiteName" }

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
                    //if (checked)
                    //    Data.RowSelectedReceive = Helper.GetListId(1);
                    //else
                    //    Data.RowSelectedReceive = [];
                });
            }
        });
    },
    ShowDetail: function () {
        $('#tbDetailSONumber').val(Data.Selected.SONumber);
        $('#tbDetailSiteId').val(Data.Selected.SiteID);
        $('#tbDetailMasterPrice').val("");
        $('#tbDetailSiteIdOpr').val(Data.Selected.SiteIDOpr);
        $('#tbDetailCompanyInvoice').val(Data.Selected.CompanyID);//Note
        $('#tbDetailStartDate').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartLeaseDate)); //Note
        $('#tbDetailStartDateRec').val(Common.Format.ConvertJSONDateTime(Data.Selected.InvoiceStartDate));
        $('#tbDetailSiteName').val(Data.Selected.SiteName);
        $('#tbDetailOprInvoice').val(Data.Selected.OperatorName);
        $('#tbDetailEndDate').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndLeaseDate));//Note
        $('#tbDetailEndDateRec').val(Common.Format.ConvertJSONDateTime(Data.Selected.InvoiceEndDate));

        $('#tbDetailTermInvoice').val(Data.Selected.InvoiceTerm);
        $('#tbDetailAmountRental').val(Common.Format.CommaSeparation(Data.Selected.BaseLeasePrice.toString())); //Note
        $('#tbDetailAmountService').val(Common.Format.CommaSeparation(Data.Selected.OMPrice.toString())); //Note
        $('#tbDetailAmountInvoice').val(Common.Format.CommaSeparation(Data.Selected.InvoiceAmount.toString())); //Note
        $('#tbDetailAmountPenalty').val(Common.Format.CommaSeparation('0')); //Note
        $('#tbDetailAmountOverdaya').val(Common.Format.CommaSeparation('0')); //Note
        $('#tbDetailAmountOverblast').val(Common.Format.CommaSeparation('0')); //Note
        $('#tbDetailAmountInflasi').val(Common.Format.CommaSeparation("0"));
        $('#tbDetailAmountDiscount').val(Common.Format.CommaSeparation("0"));
        $("#tbDetailRounding").val(Common.Format.CommaSeparation("0"));
        $('#tbDetailCurrency').val(Data.Selected.PriceCurrency);
        $("#formDetailData :input").prop("disabled", true);
        $(".divRounding").attr("style", "display:none");
        $("#btUpdateAmount").hide();
        Data.SelectedInvoiceAmount = Data.Selected.OMPrice;//Note
    },
    Export: function () {
        var strCompanyId = fsCompanyId;
        var strOperator = fsOperator;
        var strStatusBAPS = fsStatusBAPS;
        var strPeriodInvoice = fsPeriodInvoice;
        var strInvoiceType = fsInvoiceType;
        var strCurrency = fsCurrency;
        var strPONumber = fsPONumber;
        var strBAPSNumber = fsBAPSNumber;
        var strSONumber = fsSONumber;
        var strBapsType = fsBapsType;
        var strSiteIdOld = fsSiteIdOld;
        var strStartPeriod = fsStartPeriod;
        var strEndPeriod = fsEndPeriod;

        window.location.href = "/InvoiceTransaction/TrxBapsReceiveInvoiceManual/Export?strCompanyId=" + strCompanyId + "&strOperator=" + strOperator
            + "&strStatusBAPS=" + strStatusBAPS + "&strPeriodInvoice=" + strPeriodInvoice + "&strInvoiceType=" + strInvoiceType + "&strCurrency=" + strCurrency
        + "&strPONumber=" + strPONumber + "&strBAPSNumber=" + strBAPSNumber + "&strSONumber=" + strSONumber + "&strBapsType=" + strBapsType + "&strSiteIdOld=" + strSiteIdOld + "&isReceive=1"
        + "&strStartPeriod=" + strStartPeriod + "&strEndPeriod=" + strEndPeriod;
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
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsPeriodInvoice = $("#slSearchInvoiceType").val() == null ? "" : $("#slSearchInvoiceType").val();
        fsInvoiceType = $("#slSearchTermInvoice").val() == null ? "" : $("#slSearchTermInvoice").val();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
        fsPONumber = $("#tbSearchPONumber").val();
        fsBAPSNumber = $("#tbSearchBAPSNumber").val();
        fsSONumber = $("#tbSearchSONumber").val();
        fsSiteIdOld = $("#tbSearchSiteIdOld").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strStatusBAPS: fsStatusBAPS,
            strPeriodInvoice: fsPeriodInvoice,
            strInvoiceType: fsInvoiceType,
            strCurrency: fsCurrency,
            strPONumber: fsPONumber,
            strBAPSNumber: fsBAPSNumber,
            strSONumber: fsSONumber,
            strBapsType: fsBapsType,
            strSiteIdOld: fsSiteIdOld,
            isReceive: 0,
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
                "url": "/api/TrxInvoiceManual/gridBAPSConfirm",
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
                            if (Helper.IsElementExistsInArray(full.trxInvoiceManualID, Data.RowSelectedConfirm)) {
                                //$("#Row" + full.trxBapsDataId).addClass("active");
                                strReturn += "<label id='" + full.trxInvoiceManualID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                            }
                            else {
                                strReturn += "<label id='" + full.trxInvoiceManualID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' class='checkboxes' /><span></span></label>";
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
                { data: "SiteIDOpr" },
                 { data: "SiteID" },
                { data: "SiteNameOpr" },
                { data: "CompanyID" },
                 { data: "OperatorName" },
                { data: "InitialPONumber" },
                {
                    data: "InvoiceStartDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "InvoiceEndDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                 { data: "BapsNO" },
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "OMPrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                 { data: "InvoiceAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                 {
                     data: "StartLeaseDate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                {
                    data: "EndLeaseDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                 { data: "AmountPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                  { data: "AmountLossPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                 { data: "CompanyID" },
                  { data: "OperatorName" },
                  { data: "InvoiceType" },
                   { data: "InvoiceTerm" },
                   { data: "BapsType" },
                   { data: "PowerType" },
                    { data: "PriceCurrency" },
                    { data: "PPHType" },
                     {
                         data: "LossPNN", mRender: function (data, type, full) {
                             return full.LossPNN ? "Loss" : "Claim";
                         }
                     }
                //     ,
                //{ data: "StipSiro", className: "text-right" },
                //{ data: "BapsPeriod" },
                //{ data: "MLANumber" },
                //{ data: "SiteName" }
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
                    //if (checked)
                    //    Data.RowSelectedConfirm = Helper.GetListId(0);
                    //else
                    //    Data.RowSelectedConfirm = [];
                });
            }
        });
    },
    Reset: function () {
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
        fsCompanyId = "";
        fsOperator = "";
        fsStatusBAPS = "";
        fsPeriodInvoice = "";
        fsInvoiceType = "";
        fsCurrency = "";
        fsPONumber = "";
        fsBAPSNumber = "";
        fsSONumber = "";
        fsBapsType = "";
        fsSiteIdOld = "";
        fsStartPeriod = "";
        fsEndPeriod = "";
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
        $('#tbDetailSiteId').val(Data.Selected.SiteID);
        $('#tbDetailMasterPrice').val("");
        $('#tbDetailSiteIdOpr').val(Data.Selected.SiteIDOpr);
        $('#tbDetailCompanyInvoice').val(Data.Selected.CompanyID);//Note
        $('#tbDetailStartDate').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartLeaseDate)); //Note
        $('#tbDetailStartDateRec').val(Common.Format.ConvertJSONDateTime(Data.Selected.InvoiceStartDate));
        $('#tbDetailSiteName').val(Data.Selected.SiteName);
        $('#tbDetailOprInvoice').val(Data.Selected.OperatorName);
        $('#tbDetailEndDate').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndLeaseDate));//Note
        $('#tbDetailEndDateRec').val(Common.Format.ConvertJSONDateTime(Data.Selected.InvoiceEndDate));

        $('#tbDetailTermInvoice').val(Data.Selected.InvoiceTerm);
        $('#tbDetailAmountRental').val(Common.Format.CommaSeparation(Data.Selected.BaseLeasePrice.toString())); //Note
        $('#tbDetailAmountService').val(Common.Format.CommaSeparation(Data.Selected.OMPrice.toString())); //Note
       
        $('#tbDetailAmountInvoice').val(Common.Format.CommaSeparation(Data.Selected.InvoiceAmount.toString())); //Note
        $('#tbDetailAmountPenalty').val(Common.Format.CommaSeparation('0')); //Note
        $('#tbDetailAmountOverdaya').val(Common.Format.CommaSeparation('0')); //Note
        $('#tbDetailAmountOverblast').val(Common.Format.CommaSeparation('0')); //Note
        $('#tbDetailAmountInflasi').val(Common.Format.CommaSeparation("0"));
        $('#tbDetailAmountDiscount').val(Common.Format.CommaSeparation("0"));
        $("#tbDetailRounding").val(Common.Format.CommaSeparation("0"));
        $('#tbDetailCurrency').val(Data.Selected.PriceCurrency);

        $("#formDetailData :input").prop("disabled", true);
        $("#chRounding").bootstrapSwitch("state", false);
        Data.SelectedInvoiceAmount = Data.Selected.InvoiceAmount;//Note
        $(".divRounding").removeAttr("style");
        $("#btUpdateAmount").hide();
    },
    Export: function () {
        var strCompanyId = fsCompanyId;
        var strOperator = fsOperator;
        var strStatusBAPS = fsStatusBAPS;
        var strPeriodInvoice = fsPeriodInvoice;
        var strInvoiceType = fsInvoiceType;
        var strCurrency = fsCurrency;
        var strPONumber = fsPONumber;
        var strBAPSNumber = fsBAPSNumber;
        var strSONumber = fsSONumber;
        var strBapsType = fsBapsType;
        var strSiteIdOld = fsSiteIdOld;
        var strStartPeriod = fsStartPeriod;
        var strEndPeriod = fsEndPeriod;

        window.location.href = "/InvoiceTransaction/TrxBapsConfirmInvoiceManual/Export?strCompanyId=" + strCompanyId + "&strOperator=" + strOperator
            + "&strStatusBAPS=" + strStatusBAPS + "&strPeriodInvoice=" + strPeriodInvoice + "&strInvoiceType=" + strInvoiceType + "&strCurrency=" + strCurrency
        + "&strPONumber=" + strPONumber + "&strBAPSNumber=" + strBAPSNumber + "&strSONumber=" + strSONumber + "&strBapsType=" + strBapsType + "&strSiteIdOld=" + strSiteIdOld + "&isReceive=0"
        + "&strStartPeriod=" + strStartPeriod + "&strEndPeriod=" + strEndPeriod;
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
            $("#tblSummaryDataReject").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsPeriodInvoice = $("#slSearchInvoiceType").val() == null ? "" : $("#slSearchInvoiceType").val();
        fsInvoiceType = $("#slSearchTermInvoice").val() == null ? "" : $("#slSearchTermInvoice").val();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
        fsPONumber = $("#tbSearchPONumber").val();
        fsBAPSNumber = $("#tbSearchBAPSNumber").val();
        fsSONumber = $("#tbSearchSONumber").val();
        fsSiteIdOld = $("#tbSearchSiteIdOld").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strStatusBAPS: fsStatusBAPS,
            strPeriodInvoice: fsPeriodInvoice,
            strInvoiceType: fsInvoiceType,
            strCurrency: fsCurrency,
            strPONumber: fsPONumber,
            strBAPSNumber: fsBAPSNumber,
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
                "url": "/api/TrxInvoiceManual/gridBAPSReject",
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
                        TableReject.Export();
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
               { data: "SiteIDOpr" },
                 { data: "SiteID" },
                { data: "SiteNameOpr" },
                { data: "CompanyID" },
                 { data: "OperatorName" },
                { data: "InitialPONumber" },
                {
                    data: "InvoiceStartDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "InvoiceEndDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                 { data: "BapsNO" },
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "OMPrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                 { data: "InvoiceAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                 {
                     data: "StartLeaseDate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                {
                    data: "EndLeaseDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                 { data: "AmountPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                  { data: "AmountLossPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                 { data: "CompanyID" },
                  { data: "OperatorName" },
                  { data: "InvoiceType" },
                   { data: "InvoiceTerm" },
                   { data: "BapsType" },
                   { data: "PowerType" },
                    { data: "PriceCurrency" },
                    { data: "PPHType" },
                     {
                         data: "LossPNN", mRender: function (data, type, full) {
                             return full.LossPNN ? "Loss" : "Claim";
                         }
                     }
                //     ,
                //{ data: "StipSiro", className: "text-right" },
                //{ data: "BapsPeriod" },
                //{ data: "MLANumber" },
                //{ data: "SiteName" }
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
            "order": [],
            "scrollY": 300, /* Enable vertical scroll to allow fixed columns */
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            }
        });
    },
    Export: function () {
        var strCompanyId = fsCompanyId;
        var strOperator = fsOperator;
        var strStatusBAPS = fsStatusBAPS;
        var strPeriodInvoice = fsPeriodInvoice;
        var strInvoiceType = fsInvoiceType;
        var strCurrency = fsCurrency;
        var strPONumber = fsPONumber;
        var strBAPSNumber = fsBAPSNumber;
        var strSONumber = fsSONumber;
        var strBapsType = fsBapsType;
        var strSiteIdOld = fsSiteIdOld;
        var strStartPeriod = fsStartPeriod;
        var strEndPeriod = fsEndPeriod;

        window.location.href = "/InvoiceTransaction/TrxBapsRejectInvoiceManual/Export?strCompanyId=" + strCompanyId + "&strOperator=" + strOperator
            + "&strStatusBAPS=" + strStatusBAPS + "&strPeriodInvoice=" + strPeriodInvoice + "&strInvoiceType=" + strInvoiceType + "&strCurrency=" + strCurrency
        + "&strPONumber=" + strPONumber + "&strBAPSNumber=" + strBAPSNumber + "&strSONumber=" + strSONumber + "&strBapsType=" + strBapsType + "&strSiteIdOld=" + strSiteIdOld
        + "&strStartPeriod=" + strStartPeriod + "&strEndPeriod=" + strEndPeriod;
    }
}

var Process = {
    Confirm: function () {
        Data.ConfirmBAPS = [];
        var l = Ladda.create(document.querySelector("#btYesConfirm"))
        var params = {
            ListId: Data.RowSelectedConfirm
        }
        $.ajax({
            url: "/api/TrxInvoiceManual/ConfirmBAPS",
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
                if (data.ErrorMessage == "" || data.ErrorMessage == null) {
                    Common.Alert.Success("Data Success Confirmed!");
                    Data.RowSelectedConfirm = [];
                } else {
                    Common.Alert.Warning(data.ErrorMessage);
                }

            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Form.DoneConfirm();
        })

    },
    Reject: function () {
        Data.RejectBAPS = [];
        var l = Ladda.create(document.querySelector("#btYesReject"))
        var indexTab = $("#tabBAPS").tabs('option', 'active');
        var params = {
            ListId: (indexTab == 0) ? Data.RowSelectedReceive : Data.RowSelectedConfirm,
            Remarks: $("#tbRemarks").val(),
            MstRejectDtlId: $("#slCategory").val(),
            isReceive: (indexTab == 0) ? 1 : 0
        }

        $.ajax({
            url: "/api/TrxInvoiceManual/RejectBAPS",
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
                if (data.ErrorMessage == "" || data.ErrorMessage == null) {
                    Common.Alert.Success("Data Success Rejected!");
                    Data.RowSelectedReceive = [];
                    Data.RowSelectedConfirm = [];
                } else {
                    Common.Alert.Warning(data.ErrorMessage);
                }

            }
            l.stop();

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Form.DoneReject();
        })
    },
    UpdateRentalAmount: function () {
        var RoundingAmount = parseFloat($("#tbDetailRounding").val().replace(/,/g, ""));

        if (RoundingAmount == null || RoundingAmount == undefined || RoundingAmount == "")
            RoundingAmount = 0;

        var l = Ladda.create(document.querySelector("#btUpdateAmount"))
        $.ajax({
            url: "/api/TrxInvoiceManual/" + Data.Selected.trxInvoiceManualID + "/" + RoundingAmount,
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
    Receive: function () {
        var l = Ladda.create(document.querySelector("#btReceive"))
        if (Data.RowSelectedReceive.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            var params = {
                ListId: Data.RowSelectedReceive
            }
            $.ajax({
                url: "/api/TrxInvoiceManual/ReceiveBAPS",
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
                    if (data.ErrorMessage == "" || data.ErrorMessage == null) {
                        Common.Alert.Success("Data Success Received!")
                        Data.RowSelectedReceive = [];
                    } else {
                        Common.Alert.Warning(data.ErrorMessage);
                    }

                }
                l.stop();

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l.stop();
            })
            .always(function (jqXHR, textStatus) {
                $(".panelSiteData").fadeOut();
                TableReceive.Search();

            })
        }
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
                    $("#slSearchTermInvoice").append("<option value='" + item.Description + "'>" + item.Description + "</option>");
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
        var InvoiceAmount = Data.SelectedInvoiceAmount;
        var AmountRounding = parseFloat($("#tbDetailRounding").val().toString().replace(/,/g, ""));
        var TotalAmount = 0;
        if (!isNaN(AmountRounding))
            TotalAmount = InvoiceAmount + AmountRounding;
        else
            TotalAmount = InvoiceAmount;
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
    GetListId: function (isReceive) {
        //for CheckAll Pages
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strStatusBAPS: fsStatusBAPS,
            strPeriodInvoice: fsPeriodInvoice,
            strInvoiceType: fsInvoiceType,
            strCurrency: fsCurrency,
            strPONumber: fsPONumber,
            strBAPSNumber: fsBAPSNumber,
            strSONumber: fsSONumber,
            strBapsType: fsBapsType,
            strSiteIdOld: fsSiteIdOld,
            isReceive: isReceive,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/BapsData/GetListId",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                AjaxData = data;
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
        return AjaxData;
    }
}