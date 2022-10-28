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
var fsRejectHeader = "";
var fsRejectDetail = "";
var fsRecipient = "";
var fsCC = "";
var fsSiteName = "";
var fsAccountNumber = "";
var fsAccountName = "";
var fsBankName = "";
var fsSiteIDOpr = "";
var fsDescription = "";
var fsVoucherNumber = "";
var fsPICA = "";
var fsRejectRemarks = "";
var fsYearPeriod = "";
var fsRegion = "";

jQuery(document).ready(function () {
    Data.RowSelectedReceive = [];
    Data.RowSelectedConfirm = [];
    Form.Init();
    Table.Init();
    TableReceive.Init();
    TableReject.Init();
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
        Table.Reset();
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
            $("#tbDetailRounding").val("0.00");
        }
        else {
            $('.NeedRounding').hide();
            $("#tbDetailAmountInvoice").val(Common.Format.CommaSeparation(Data.Selected.InvoiceAmount.toString()));
        }

    });

    $("#tbDetailRounding").unbind().on("blur", function () {
        Helper.Calculate();
    });

    $("#btUpdateAmount").unbind().click(function () {
        Process.UpdateRentalAmount();
    });


    $('#tabBAPS').unbind().on('tabsactivate', function (event, ui) {
        $('#rowFilterConfirm').hide();
        Form.ResetParameters();
        var newIndex = ui.newTab.index();
        if (newIndex == 0) {
            Data.RowSelectedReceive = [];
            TableReceive.Search();
            $('#btReceive').show();
            $('#btConfirm').hide();
            $('#btReject').show();
        }
        else if (newIndex == 1) {
            $('#rowFilterConfirm').show();
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

    $(".btnSearchData").unbind().click(function () {
        TableReceive.Search();
    });

    $(".btnSearchConfirm").unbind().click(function () {
        Table.Search();
    });

    $(".btnSearchReject").unbind().click(function () {
        TableReject.Search();
    });

    Form.LoadParam();
});

var Form = {
    Init: function () {
        $("#formReject").parsley();

        if (!$("#hdAllowProcess").val()) {
            $("#btConfirm").hide();
            $("#btReject").hide();
            $("#btReceive").hide();
        }

        $("#slSearchConfirmType").select2({ placeholder: "Confirm Type", width: null });
        $('#rowFilterConfirm').hide();
        Control.BindingSelectCompany();
        Control.BindingSelectOperator();
        Control.BindingSelectRegion();
        Control.BindSelectYearAll($('.SearchYearFilter'));
        Control.BindingSelectRejectHdr();
        Control.BindingSelectRejectDtl();
        Table.Reset();
        $('#tabBAPS').tabs();
        $('#btReceive').show();
        $('#btConfirm').hide();
        $("#spRemarks").hide();

    },

    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
    },
    DoneConfirm: function () {
        $("#pnlSummary").fadeIn();
        $('#mdlConfirmBAPS').modal('toggle');
        Table.Search();
        //Table.Reset();
        window.location.href = "/InvoiceTransaction/TrxCreateInvoiceTower";
    },
    DoneReject: function () {
        $("#pnlSummary").fadeIn();
        $('#mdlRejectBAPS').modal('toggle');
        if ($("#tabBAPS").tabs('option', 'active') == 0) {
            TableReceive.Search();
            //Table.Reset();
        }
        else {
            Table.Search();
            //Table.Reset();
        }
        var chat = $.connection.notificationHub;
        chat.server.getNotif();
        chat.server.getTask();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $('#mdlDetail').modal('toggle');
        Table.Search();
        //Table.Reset();
    },
    ConfirmButton: function () {
        if ($("#slSearchConfirmType").val() == "INV") {
            $("#btConfirm").hide();
            $("#btReject").hide();
        }
        else {
            $("#btConfirm").show();
            $("#btReject").show();
        }
    },
    ResetParameters: function () {
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
        fsRejectHeader = "";
        fsRejectDetail = "";
        fsRecipient = "";
        fsCC = "";
        fsSiteName = "";
        fsAccountNumber = "";
        fsAccountName = "";
        fsBankName = "";
        fsSiteIDOpr = "";
        fsDescription = "";
        fsVoucherNumber = "";
        fsPICA = "";
        fsRejectRemarks = "";
        fsYearPeriod = "";
    },
    LoadParam: function () {
        var TabSelect = Helper.GetUrlParameterValue("Tab");
        var Bind = Helper.GetUrlParameterValue("Bind");
        var SONumberParam = Helper.GetUrlParameterValue("SoNumber");

        if (TabSelect != null)
            $('.nav-tabs a[href="#' + TabSelect + '"]').trigger('click');

        if (SONumberParam != null)
            fsSONumber = SONumberParam;

        if (Bind == "Reject") {
            $("#schSONumberReject").val(fsSONumber);
            TableReject.Search();
        }
        else if (Bind == "Confirm") {
            $("#schSONumberConfirm").val(fsSONumber);
            Table.Search();
        }
            
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
        fsCompanyId = $("#slSearchCompanyName").val() == null ? $("#schCompanyIDConfirm").val() : $("#slSearchCompanyName").val();
        fsOperator = $("#slSearchOperator").val() == null ? $("#schCustomerIDConfirm").val() : $("#slSearchOperator").val();
        fsSiteName = $("#schSiteNameConfirm").val() == null ? "" : $("#schSiteNameConfirm").val();
        fsAccountNumber = $("#schAccountNumberConfirm").val() == null ? "" : $("#schAccountNumberConfirm").val();
        fsAccountName = $("#schAccountNameConfirm").val() == null ? "" : $("#schAccountNameConfirm").val();
        fsPONumber = $("#schExpenseNumberConfirm").val();
        fsBankName = $("#schBankNameConfirm").val();
        fsSONumber = $("#schSONumberConfirm").val();
        fsSiteIdOld = $("#schSiteIDConfirm").val();
        fsSiteIDOpr = $("#schSiteIDOprConfirm").val() == null ? "" : $("#schSiteIDOprConfirm").val();
        fsVoucherNumber = $("#schVoucherNumberConfirm").val();
        fsDescription = $("#schDescriptionConfirm").val();
        fsStartPeriod = $("#slSearchYearFrom").val();
        fsEndPeriod = $("#slSearchYearTo").val();
        fsRegion = Helper.GetSelectedTextDD($("#slSearchRegion"), fsRegion);
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strSiteName: fsSiteName,
            strAccountNumber: fsAccountNumber,
            strAccountName: fsAccountName,
            strBankName: fsBankName,
            strPONumber: fsPONumber,
            strSiteIDOpr: fsSiteIDOpr,
            strSONumber: fsSONumber,
            strVoucherNumber: fsVoucherNumber,
            strSiteIdOld: fsSiteIdOld,
            isReceive: $("#slSearchConfirmType").val() == "INV" ? 2 : 0,
            strDescription: fsDescription,
            strRegion: fsRegion,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "bSortCellsTop": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/Electricity/grid",
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
                        var disabledchks = $("#slSearchConfirmType").val() == "INV" ? " disabled" : "";
                        var strReturn = "";
                        {
                            if (Helper.IsElementExistsInArray(full.trxBapsDataId, Data.RowSelectedConfirm)) {
                                //$("#Row" + full.trxBapsDataId).addClass("active");
                                strReturn += "<label id='" + full.trxBapsDataId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' checked class='checkboxes' " + disabledchks + "/><span></span></label>";
                            }
                            else {
                                strReturn += "<label id='" + full.trxBapsDataId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' class='checkboxes' "+ disabledchks +"/><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                },
                {
                    data: "ReceiveDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
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
                { data: "PoNumber" },
                { data: "PPHType" },
                {
                    data: "IsLossPPN", mRender: function (data, type, full) {
                        return full.IsLossPPN ? "Loss" : "Claim";
                    }
                },
                {
                    data: "TRANSDATE", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "BankAccountNumber" },
                { data: "BankAccountName" },
                { data: "BankName" },
                { data: "ReferenceNumber" },
                { data: "AmountExpense", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "VOUCHER" },
                { data: "ExpenseAdvance" },
                { data: "Currency" },
                { data: "InvoiceAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                {
                    data: "InvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvoiceNumber" },
                { data: "ExpDescription" }
                
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
            //"fixedColumns": {
            //    leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            //},
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
            "initComplete": function () {
                var disabledchk = $("#slSearchConfirmType").val() == "INV" ? " disabled" : "";
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                    '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tblSummaryData .checkboxes" '+ disabledchk +'/>' +
                    '<span></span> ' +
                    '</label>';
                var th = $("th.select-all-confirm").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-confirm").unbind().on("change", ".group-checkable", function (e) {
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
                    if (checked)
                        Data.RowSelectedConfirm = Helper.GetListId(0);
                    else
                        Data.RowSelectedConfirm = [];
                });
            }
        });

        if ($("#slSearchConfirmType").val() != "INV") {
            tblSummaryData.column(23).visible(false);
            tblSummaryData.column(24).visible(false);
        }
    },
    Reset: function () {
        var dt = new Date();
        var currentyear = dt.getFullYear();
        $("#slSearchCompanyName").val("").trigger('change');
        $("#slSearchOperator").val("").trigger('change');
        $("#slSearchRegion").val("").trigger('change');
        $("#slSearchConfirmType").val("RTI").trigger('change');
        $("#slSearchYearFrom").val(currentyear).trigger('change');
        $("#slSearchYearTo").val(currentyear).trigger('change');
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
        $('#tbDetailSiteId').val(Data.Selected.SiteIdOld);
        $('#tbDetailRegion').val(Data.Selected.Regional);
        $('#tbDetailSiteIdOpr').val(Data.Selected.SiteIdOpr);
        $('#tbDetailCompanyInvoice').val(Data.Selected.CompanyInvoice);
        $('#tbDetailBapsPeriod').val(Data.Selected.BapsPeriod);
        $('#tbDetailYear').val(new Date(Data.Selected.StartDateInvoice).getFullYear());
        $('#tbDetailSiteName').val(Data.Selected.SiteName);
        $('#tbDetailOprInvoice').val(Data.Selected.Operator);
        $('#tbDetailProvince').val("");

        $('#tbDetailExpenseNumber').val(Data.Selected.PoNumber);
        $('#tbDetailPPHType').val(Data.Selected.PPHType.toString());
        $('#tbDetailTransferDate').val(Common.Format.ConvertJSONDateTime(Data.Selected.TRANSDATE));
        $('#tbDetailAccountNumber').val(Data.Selected.BankAccountNumber);
        $('#tbDetailAccountName').val(Data.Selected.BankAccountName);
        $('#tbDetailBankName').val(Data.Selected.BankName);
        $('#tbDetailPaymentReference').val(Data.Selected.ReferenceNumber);
        $('#tbDetailExpenseAmount').val(Common.Format.CommaSeparation(Data.Selected.AmountExpense));
        $('#tbDetailVoucher').val(Data.Selected.VOUCHER);
        $("#tbDetailAmount").val(Common.Format.CommaSeparation(Data.Selected.InvoiceAmount));
        $('#tbDetailCurrency').val(Data.Selected.Currency); 
        $('#tbDetailDocumentNumber').val(Data.Selected.ExpenseAdvance);
        $('#tbDetailDescription').val(Data.Selected.ExpDescription);
        $("#formDetailData :input").prop("disabled", true);
        Data.SelectedInvoiceAmount = Data.Selected.InvoiceAmount;
        $("#btUpdateAmount").hide();
    },
    Export: function () {
        var strCompanyId = fsCompanyId;
        var strOperator = fsOperator;
        var strStatusBAPS = fsStatusBAPS;
        var strPeriodInvoice = "";
        var strInvoiceType = fsInvoiceType;
        var strCurrency = fsCurrency;
        var strPONumber = fsPONumber;
        var strBAPSNumber = fsBAPSNumber;
        var strSONumber = fsSONumber;
        var strBapsType = fsBapsType;
        var strSiteIdOld = fsSiteIdOld;
        var strStartPeriod = fsStartPeriod;
        var strEndPeriod = fsEndPeriod;

        window.location.href = "/InvoiceTransaction/ExportTrxElectricity?strCompanyId=" + strCompanyId + "&strOperator=" + strOperator
            + "&strStatus=" + strStatusBAPS + "&strPeriodInvoice=" + strPeriodInvoice + "&strAccountName=" + fsAccountName
            + "&strCurrency=" + strCurrency + "&strSiteIdOld=" + strSiteIdOld + "&isReceive=0" + "&strPONumber=" + strPONumber
            + "&strAccountNumber=" + fsAccountNumber + "&strSONumber=" + strSONumber + "&strBankName=" + fsBankName + "&strSiteIDOpr=" + fsSiteIDOpr
            + "&strSiteName=" + fsSiteName + "&strVoucherNumber=" + fsVoucherNumber + "&strDescription=" + fsDescription
            + "&strRejectRemarks=" + fsRejectRemarks + "&strPICA=" + fsPICA + "&strYearPeriod=" + fsYearPeriod + "&isReject=0"
            + "&strStartPeriod=" + strStartPeriod + "&strEndPeriod=" + strEndPeriod + "&strRegion=" + fsRegion;
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
        fsCompanyId = $("#slSearchCompanyName").val() == null ? $("#schCompanyID").val() : $("#slSearchCompanyName").val();
        fsOperator = $("#slSearchOperator").val() == null ? $("#schCustomerID").val() : $("#slSearchOperator").val();
        fsSiteName = $("#schSiteName").val() == null ? "" : $("#schSiteName").val();
        fsAccountNumber = $("#schAccountNumber").val() == null ? "" : $("#schAccountNumber").val();
        fsAccountName = $("#schAccountName").val() == null ? "" : $("#schAccountName").val();
        fsPONumber = $("#schExpenseNumber").val();
        fsBankName = $("#schBankName").val();
        fsSONumber = $("#schSONumber").val();
        fsSiteIdOld = $("#schSiteID").val();
        fsSiteIDOpr = $("#schSiteIDOpr").val() == null ? "" : $("#schSiteIDOpr").val();
        fsVoucherNumber = $("#schVoucherNumber").val();
        fsDescription = $("#schDescription").val();
        fsStartPeriod = $("#slSearchYearFrom").val();
        fsEndPeriod = $("#slSearchYearTo").val();
        fsRegion = Helper.GetSelectedTextDD($("#slSearchRegion"), fsRegion);
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strSiteName: fsSiteName,
            strAccountNumber: fsAccountNumber,
            strAccountName: fsAccountName,
            strBankName: fsBankName,
            strPONumber: fsPONumber,
            strSiteIDOpr: fsSiteIDOpr,
            strSONumber: fsSONumber,
            strVoucherNumber: fsVoucherNumber,
            strSiteIdOld: fsSiteIdOld,
            isReceive: 1,
            strDescription: fsDescription,
            strRegion: fsRegion,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod
        };
        var tblSummaryData = $("#tblSummaryDataReceive").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "bSortCellsTop": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/Electricity/grid",
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
                            if (Helper.IsElementExistsInArray(full.trxBapsDataId, Data.RowSelectedReceive)) {
                                //$("#Row" + full.trxBapsDataId).addClass("active");
                                strReturn += "<label id='" + full.trxBapsDataId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxBapsDataId + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
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
                        return "<a class='btDetail' style='vertical-align:middle;text-align:center;'>" + data + "</a>";
                    }
                },
                { data: "SiteIdOpr" },
                { data: "SiteIdOld" },
                { data: "SiteName" },
                { data: "CompanyInvoice" },
                { data: "Operator" },
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
                { data: "PoNumber" },
                { data: "PPHType" },
                {
                    data: "IsLossPPN", mRender: function (data, type, full) {
                        return full.IsLossPPN ? "Loss" : "Claim";
                    }
                },
                {
                    data: "TRANSDATE", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "BankAccountNumber" },
                { data: "BankAccountName" },
                { data: "BankName" },
                { data: "ReferenceNumber" },
                { data: "AmountExpense", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "VOUCHER" },
                { data: "ExpenseAdvance" },
                { data: "Currency" },
                { data: "InvoiceAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ExpDescription" }
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
            //"fixedColumns": {
            //    leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            //},
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
                    if (checked)
                        Data.RowSelectedReceive = Helper.GetListId(1);
                    else
                        Data.RowSelectedReceive = [];
                });
            }
        });
    },
    ShowDetail: function () {
        $('#tbDetailSONumber').val(Data.Selected.SONumber);
        $('#tbDetailSiteId').val(Data.Selected.SiteIdOld);
        $('#tbDetailRegion').val(Data.Selected.Regional);
        $('#tbDetailSiteIdOpr').val(Data.Selected.SiteIdOpr);
        $('#tbDetailCompanyInvoice').val(Data.Selected.CompanyInvoice);
        $('#tbDetailBapsPeriod').val(Data.Selected.BapsPeriod);
        $('#tbDetailYear').val(new Date(Data.Selected.StartDateInvoice).getFullYear());
        $('#tbDetailSiteName').val(Data.Selected.SiteName);
        $('#tbDetailOprInvoice').val(Data.Selected.Operator);
        $('#tbDetailProvince').val("");

        $('#tbDetailExpenseNumber').val(Data.Selected.PoNumber);
        $('#tbDetailPPHType').val(Data.Selected.PPHType.toString());
        $('#tbDetailTransferDate').val(Common.Format.ConvertJSONDateTime(Data.Selected.TRANSDATE));
        $('#tbDetailAccountNumber').val(Data.Selected.BankAccountNumber);
        $('#tbDetailAccountName').val(Data.Selected.BankAccountName);
        $('#tbDetailBankName').val(Data.Selected.BankName);
        $('#tbDetailPaymentReference').val(Data.Selected.ReferenceNumber);
        $('#tbDetailExpenseAmount').val(Common.Format.CommaSeparation(Data.Selected.AmountExpense));
        $('#tbDetailVoucher').val(Data.Selected.VOUCHER);
        $("#tbDetailAmount").val(Common.Format.CommaSeparation(Data.Selected.InvoiceAmount));
        $('#tbDetailCurrency').val(Data.Selected.Currency);
        $('#tbDetailDocumentNumber').val(Data.Selected.ExpenseAdvance);
        $('#tbDetailDescription').val(Data.Selected.ExpDescription);
        $("#formDetailData :input").prop("disabled", true);
        $("#btUpdateAmount").hide();
        Data.SelectedInvoiceAmount = Data.Selected.InvoiceAmount;
    },
    Export: function () {
        var strCompanyId = fsCompanyId;
        var strOperator = fsOperator;
        var strStatusBAPS = fsStatusBAPS;
        var strPeriodInvoice = "";
        var strAccountName = fsAccountName;
        var strCurrency = fsCurrency;
        var strPONumber = fsPONumber;
        var strBAPSNumber = fsBAPSNumber;
        var strSONumber = fsSONumber;
        var strBapsType = fsBapsType;
        var strSiteIdOld = fsSiteIdOld;
        var strStartPeriod = fsStartPeriod;
        var strEndPeriod = fsEndPeriod;

        window.location.href = "/InvoiceTransaction/ExportTrxElectricity?strCompanyId=" + strCompanyId + "&strOperator=" + strOperator
            + "&strStatus=" + strStatusBAPS + "&strPeriodInvoice=" + strPeriodInvoice + "&strAccountName=" + fsAccountName
            + "&strCurrency=" + strCurrency + "&strSiteIdOld=" + strSiteIdOld + "&isReceive=1" + "&strPONumber=" + strPONumber
            + "&strAccountNumber=" + fsAccountNumber + "&strSONumber=" + strSONumber + "&strBankName=" + fsBankName + "&strSiteIDOpr=" + fsSiteIDOpr
            + "&strSiteName=" + fsSiteName + "&strVoucherNumber=" + fsVoucherNumber + "&strDescription=" + fsDescription
            + "&strRejectRemarks=" + fsRejectRemarks + "&strPICA=" + fsPICA + "&strYearPeriod=" + fsYearPeriod + "&isReject=0"
            + "&strStartPeriod=" + strStartPeriod + "&strEndPeriod=" + strEndPeriod + "&strRegion=" + fsRegion;
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
        fsCompanyId = $("#slSearchCompanyName").val() == null ? $("#schCompanyIDReject").val() : $("#slSearchCompanyName").val();
        fsOperator = $("#slSearchOperator").val() == null ? $("#schCustomerIDReject").val() : $("#slSearchOperator").val();
        fsSiteName = $("#schSiteNameReject").val() == null ? "" : $("#schSiteNameReject").val();
        fsPICA = $("#schPICA").val() == null ? "" : $("#schPICA").val();
        fsRejectRemarks = $("#schRejectRemarks").val() == null ? "" : $("#schRejectRemarks").val();
        fsPONumber = $("#schExpenseNumberReject").val();
        fsPeriodInvoice = $("#schBapsPeriod").val();
        fsSONumber = $("#schSONumberReject").val();
        fsSiteIdOld = $("#schSiteIDReject").val();
        fsSiteIDOpr = $("#schSiteIDOprReject").val() == null ? "" : $("#schSiteIDOprReject").val();
        fsYearPeriod = $("#schYearPeriod").val();
        fsStatusBAPS = $("#schStatusReject").val();
        fsStartPeriod = $("#slSearchYearFrom").val();
        fsEndPeriod = $("#slSearchYearTo").val();
        fsRegion = Helper.GetSelectedTextDD($("#slSearchRegion"), fsRegion);
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strSiteName: fsSiteName,
            strPICA: fsPICA,
            strRejectRemarks: fsRejectRemarks,
            strPeriodInvoice: fsPeriodInvoice,
            strPONumber: fsPONumber,
            strSiteIDOpr: fsSiteIDOpr,
            strSONumber: fsSONumber,
            strYearPeriod: fsYearPeriod,
            strSiteIdOld: fsSiteIdOld,
            strStatus: fsStatusBAPS,
            isReject: 1,
            strRegion: fsRegion,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod
        };
        var tblSummaryData = $("#tblSummaryDataReject").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "bSortCellsTop": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/Electricity/gridReject",
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
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
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
                { data: "BapsPeriod" },
                {
                    data: "StartDateInvoice", render: function (data) {
                        var dateInv = new Date(data);
                        return dateInv.getFullYear();
                    }
                },
                { data: "PoNumber" },
                { data: "InvoiceAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "PICAStatus" }
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [10], "orderable": false }],
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
            //"fixedColumns": {
            //    leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            //}
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

        window.location.href = "/InvoiceTransaction/ExportRejectTrxElectricity?strCompanyId=" + strCompanyId + "&strOperator=" + strOperator
            + "&strStatus=" + strStatusBAPS + "&strPeriodInvoice=" + strPeriodInvoice + "&strAccountName=" + fsAccountName
            + "&strCurrency=" + strCurrency + "&strSiteIdOld=" + strSiteIdOld + "&isReceive=0" + "&strPONumber=" + strPONumber
            + "&strAccountNumber=" + fsAccountNumber + "&strSONumber=" + strSONumber + "&strBankName=" + fsBankName + "&strSiteIDOpr=" + fsSiteIDOpr
            + "&strSiteName=" + fsSiteName + "&strVoucherNumber=" + fsVoucherNumber + "&strDescription=" + fsDescription
            + "&strRejectRemarks=" + fsRejectRemarks + "&strPICA=" + fsPICA + "&strYearPeriod=" + fsYearPeriod + "&isReject=1"
            + "&strStartPeriod=" + strStartPeriod + "&strEndPeriod=" + strEndPeriod + "&strRegion=" + fsRegion;
    }
}

var Process = {
    Confirm: function () {
        Data.ConfirmBAPS = [];
        var l = Ladda.create(document.querySelector("#btYesConfirm"))
        var remarks = "";
        var params = {
            ListId: Data.RowSelectedConfirm,
            Remarks: remarks
        }
        $.ajax({
            url: "/api/Electricity/ConfirmBAPS",
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
                    Process.SendTaskToDo(0);
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

        fsRejectDetail = Helper.GetSelectedTextDD($("#slCategory"), fsRejectDetail);
        fsRecipient = $("#tbEmailRecipient").val();
        fsCC = $("#tbEmailCC").val();

        var params = {
            ListId: (indexTab == 0) ? Data.RowSelectedReceive : Data.RowSelectedConfirm,
            Remarks: $("#tbRemarks").val(),
            MstRejectDtlId: $("#slCategory").val(),
            RejectType: fsRejectDetail,
            Recipient: fsRecipient,
            CC: fsCC
        }

        console.log(params);

        $.ajax({
            url: "/api/Electricity/RejectMail",
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
    Receive: function () {
        var l = Ladda.create(document.querySelector("#btReceive"))
        if (Data.RowSelectedReceive.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            var params = {
                ListId: Data.RowSelectedReceive
            }
            $.ajax({
                url: "/api/Electricity/ReceiveBAPS",
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
                        Process.SendTaskToDo(1);
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
    },
    SendTaskToDo: function (isReceive) {
        var params = {
            ListId: isReceive == 1 ? Data.RowSelectedReceive : Data.RowSelectedConfirm,
            isReceive: isReceive
        }

        $.ajax({
            url: "/api/Electricity/SendTask",
            type: "POST",
            data: params,
            cache: false,
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                if (data.ErrorMessage == "" || data.ErrorMessage == null) {
                    isReceive = 0;
                } else {
                    Common.Alert.Warning(data.ErrorMessage);
                }
            }

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        })
        .always(function (jqXHR, textStatus) {
            var chat = $.connection.notificationHub;
            chat.server.getNotif();
            chat.server.getTask();
        })
    }
}

var Control = {
    BindingSelectRejectHdr: function () {
        $.ajax({
            url: "/api/Electricity/RejectHdr",
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
    },
    BindSelectYearAll: function (elements) {
        var dt = new Date();
        var currentyear = dt.getFullYear();
        elements.html("");

        for (var i = -10; i <= 10; i++) {
            if (currentyear == (currentyear + i))
                elements.append("<option value='" + (currentyear + i) + "' selected >" + (currentyear + i) + "</option>");
            else
                elements.append("<option value='" + (currentyear + i) + "'>" + (currentyear + i) + "</option>");

        }

        elements.select2({ placeholder: "Select Year", width: null });
    },
    BindingSelectRegion: function () {
        $.ajax({
            url: "/api/MstDataSource/Regional",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchRegion").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchRegion").append("<option value='" + item.RegionalId + "'>" + item.RegionalName + "</option>");
                })
            }

            $("#slSearchRegion").select2({ placeholder: "Select Regional Name", width: null });

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
            url: "/api/Electricity/GetListId",
            type: "POST",
            async: false,
            data: params,
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
    },
    GetSelectedTextDD: function (elements, output) {
        if (elements.val() == null || elements.val() == "") {
            output = "";
        }
        else {
            var selections = (JSON.stringify(elements.select2('data'))).toString();

            var slt = JSON.parse(selections);
            output = slt[0].text;
        }

        return output;
    },
    GetUrlParameterValue: function (name,url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, '\\$&');
        var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    }
}

