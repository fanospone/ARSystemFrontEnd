Data = {};

var fsCompanyId = "";
var fsOperatorId = "";
var fsInvoiceTypeId = "";
var fsStartPeriod = "";
var fsEndPeriod = "";
var fsInvoiceStatusId = -1;
var fsInvNo = "";
var fsInvoiceManual = "";
var fsUserCompanyCode = "";

jQuery(document).ready(function () {
    // Modification Or Added By Ibnu Setiawan 04. September 2020
    // Add Company Code By User Login
    fsUserCompanyCode = $('#hdUserCompanyCode').val();
    // End Modification Or Added By Ibnu Setiawan 04. September 2020
    Data.RowSelected = [];
    Data.RowSelectedSite = [];
    Data.RowSelectedDeptHead = [];
    Data.RowSelectedDeptHeadReprint = [];
    Form.Init();
    //Control.GetCurrentUserRole();
    Control.BindingSelectCompany();
    Control.BindingSelectOperator();
    Control.BindingSelectInvoiceType();
    Control.BindingPicaCategory();
    Control.BindingSelectInvoiceStatus();
    Control.BindingSelectSignature();
    Table.Reset();

    if (fsUserCompanyCode.trim() == "PKP") {
        $("#slSearchCompanyName").val(fsUserCompanyCode).trigger("change");
    }

    // Di pindahkan karena ada Binding Grid yang mendahului Initialize Dropdown
    Control.GetCurrentUserRole(); // Modification Or Added By Ibnu Setiawan 09. September 2020 

    //panel Summary
    $("#btSearch").unbind().click(function () {
        if (Data.Role == "DEPT HEAD")
            if ($("#tabApproval").tabs('option', 'active') == 0)
                TableDepthead.Search();
            else
                TableDeptHeadReprint.Search();

        else
            Table.Search();
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
        if (fsUserCompanyCode.trim() == "PKP") {
            $("#slSearchCompanyName").val(fsUserCompanyCode).trigger("change");
        }
    });

    $("#btCancel").unbind().click(function () {
        Form.Cancel();
        Table.Search();
    });
    $("#btCancelMerge").unbind().click(function () {
        Form.CancelMerge();
        //Table.Reset();
    });

    $("#btAdd").unbind().click(function () {
        Process.AddToSiteList();
    });

    $("#btPrintInvoice").unbind().click(function () {
        Table.Print();
    });
    $("#btCNInvoice").unbind().click(function () {
        if (Data.RowSelected.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            var ValidateCN = Helper.ValidateCN();
            if (ValidateCN.Result != "")
                Common.Alert.Warning(ValidateCN.Result);
            else {
                $("#tbRemarksCancel").val("");
                //$('#mdlCNInvoice').modal('show'); 
                Common.PanelCNARData.Reset();
                $('.mdlCNARData').modal('show');
            }
        }
    });
    $("#btMerge").unbind().click(function () {
        Process.ShowMerge();
    });
    $("#formTransaction").submit(function (e) {
        Process.PostingMergeInvoices();
        e.preventDefault();
    });

    $("#formReprint").submit(function (e) {
        var l = Ladda.create(document.querySelector("#btYesPrint"))
        l.start();
        var tempHeaderId = [];
        var tempCategoryId = [];
        var tempOprId = "";
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(parseInt(item.HeaderId));
            tempCategoryId.push(parseInt(item.CategoryId));
            tempOprId = item.CustomerId;
        });
        var HeaderId = tempHeaderId.join('|');
        var CategoryId = tempCategoryId.join('|');
        var PicaReprintID = $("#slPicaCategory").val() == null || $("#slPicaCategory").val() == null ? 0 : $("#slPicaCategory").val();
        var strDocType = $('input[name=DocType]:checked').val();
        if (strDocType == 1) //INVOICE
        {
           
            if (Data.PrintCount < 2) {
                window.location.href = "/InvoiceTransaction/TrxPrintInvoiceTower/Print?HeaderId=" + HeaderId + "&CategoryId=" + CategoryId + "&PicaReprintID="+PicaReprintID;
            }
            setTimeout(function () {
                Process.Print();
            }, 5000);

            e.preventDefault();
        } else if (strDocType == 2 && tempOprId.toUpperCase() == "XL") {

            window.location.href = "/InvoiceTransaction/TrxPrintInvoiceDynamic/Print?HeaderId=" + HeaderId + "&CustomerID=" + tempOprId.toUpperCase();

            setTimeout(function () {
                Process.Print();
            }, 5000);
            e.preventDefault();
        }
        l.stop();
    });

    $("#formCNARData").submit(function (e) {
        Process.CNInvoice();
        e.preventDefault();

    });
    $("#btApproveCancel").unbind().click(function () {
        if (Data.RowSelectedDeptHead.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else
            Process.ApproveCNInvoice();
    });
    $("#btRejectCancel").unbind().click(function () {
        if (Data.RowSelectedDeptHead.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else
            Process.RejectCNInvoice();
    });

    $('#tblSummaryData').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        var CategoryId = id.split('-')[0];
        var HeaderId = id.split('-')[1];
        var CustomerId = id.split('-')[2];
        var temp = new Object();
        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row_" + CategoryId + "-" + HeaderId + "-" + CustomerId).addClass("active");
            temp.CategoryId = parseInt(CategoryId);
            temp.HeaderId = parseInt(HeaderId);
            temp.CustomerId = CustomerId;
            Data.RowSelected.push(temp);
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row_" + CategoryId + "-" + HeaderId + "-" + CustomerId).removeClass("active");
            Data.RowSelected = Helper.RemoveObjectByIdFromArray(Data.RowSelected, parseInt(HeaderId), parseInt(CategoryId), CustomerId);
        }

    });

    $('#tblSummaryDataDeptHead').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        var CategoryId = id.split('-')[0];
        var HeaderId = id.split('-')[1];
        var temp = new Object();
        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row_" + CategoryId + "-" + HeaderId).addClass("active");
            temp.CategoryId = parseInt(CategoryId);
            temp.HeaderId = parseInt(HeaderId);
            Data.RowSelectedDeptHead.push(temp);
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row_" + CategoryId + "-" + HeaderId).removeClass("active");
            Data.RowSelectedDeptHead = Helper.RemoveObjectByIdFromArray(Data.RowSelectedDeptHead, parseInt(HeaderId), parseInt(CategoryId));
        }

    });
    $('#tblSummaryDataDeptHeadReprint').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        var CategoryId = id.split('-')[0];
        var HeaderId = id.split('-')[1];
        var temp = new Object();
        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row_" + CategoryId + "-" + HeaderId).addClass("active");
            temp.CategoryId = parseInt(CategoryId);
            temp.HeaderId = parseInt(HeaderId);
            Data.RowSelectedDeptHeadReprint.push(temp);
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row_" + CategoryId + "-" + HeaderId).removeClass("active");
            Data.RowSelectedDeptHeadReprint = Helper.RemoveObjectByIdFromArray(Data.RowSelectedDeptHeadReprint, parseInt(HeaderId), parseInt(CategoryId));
        }

    });

    $("#btApproveHead").unbind().on("click", function () {
        var isFormValid = $("#formReprintHead").parsley().validate();
        if (isFormValid == true) {
            var validationResult = Helper.ValidateUser("Head");
            $("#lblHeadError").text(validationResult.Result);
            if (validationResult.Result == "") {
                var tempHeaderId = [];
                var tempCategoryId = [];
                var tempOprId = "";
                $.each(Data.RowSelected, function (index, item) {
                    tempHeaderId.push(parseInt(item.HeaderId));
                    tempCategoryId.push(parseInt(item.CategoryId));
                    tempOprId = item.CustomerId;
                });
                var HeaderId = tempHeaderId.join('|');
                var CategoryId = tempCategoryId.join('|');
                if (tempOprId.toUpperCase() == "XL")
                    window.location.href = "/InvoiceTransaction/TrxPrintInvoiceDynamic/Print?HeaderId=" + HeaderId + "&CustomerID=" + tempOprId.toUpperCase();
                else
                    window.location.href = "/InvoiceTransaction/TrxPrintInvoiceTower/Print?HeaderId=" + HeaderId + "&CategoryId=" + CategoryId;

                setTimeout(function () {
                    Process.Print();
                }, 5000);
                e.preventDefault();
            }
        }
    });
    $('#tabApproval').unbind().on('tabsactivate', function (event, ui) {
        var newIndex = ui.newTab.index();
        if (newIndex == 0) {
            Data.RowSelectedDeptHead = [];
            TableDepthead.Search();
            $('#btApproveCancel').show();
            $('#btRejectCancel').show();
            $('#btApproveReprint').hide();
            $('#btRejectReprint').hide();
        }
        else {
            Data.RowSelectedDeptHeadReprint = [];
            TableDeptHeadReprint.Search();
            $('#btApproveCancel').hide();
            $('#btRejectCancel').hide();
            $('#btApproveReprint').show();
            $('#btRejectReprint').show();
        }
    });
    $("#btApproveReprint").unbind().click(function () {
        if (Data.RowSelectedDeptHeadReprint.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else
            Process.ApprovalReprint(ApprovalStatus.Approved);
    });
    $("#btRejectReprint").unbind().click(function () {
        if (Data.RowSelectedDeptHeadReprint.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else
            Process.ApprovalReprint(ApprovalStatus.Rejected);
    });
    $('input[type=radio][name=DocType]').change(function () {
        var a = $('input[name=DocType]:checked').val();
        if (a == "2") {
            $("#slSearchOperator").val('XL').trigger('change');
            $("#slSearchOperator").prop('disabled', true);
        } else {
            $("#slSearchOperator").val('').trigger('change');
            $("#slSearchOperator").prop('disabled', false);
        }
    });
});

var Form = {
    Init: function () {
        if (!$("#hdAllowProcess").val()) {
            $("#btAdd").hide();
            $("#btPrintInvoice").hide();
        }

        $(".panelSearchZero").hide();
        $(".panelSiteData").hide();
        $("#panelSiteDataMerge").hide();
        $("#formTransaction").parsley();
        $("#formReprint").parsley();
        $("#formReject").parsley();
        $("#formReprintHead").parsley();
        //used for validating checkbox
        Data.RowSelected = [];
        //used for validating checkbox table site
        Data.RowSelectedSite = [];
        TableSite.Init();
        TableSiteMerge.Init();

        $("body").delegate(".date-picker", "focusin", function () {
            $(this).datepicker({
                format: "dd-M-yyyy"
            });
        });
        $('#tabApproval').tabs();
        TableDepthead.Init();
        TableDeptHeadReprint.Init();
        $('#btApproveReprint').hide();
        $('#btRejectReprint').hide();
    },

    Cancel: function () {
        $("#panelSiteDataMerge").fadeOut();
        $("#pnlSummary").fadeIn();
        Form.ClearRowSelected();
        $(".panelSiteData").fadeOut();

    },
    CancelMerge: function () {
        $("#panelSiteDataMerge").fadeOut();
        $("#pnlSummary").fadeIn();
        $(".panelSiteData").fadeIn();

    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        Table.Search();
        //Table.Reset();
        Notification.GetList();
    },
    ClearRowSelected: function () {
        Data.RowSelected = [];
        Data.RowSelectedSite = [];
        Data.RowSelectedDeptHead = [];
        Data.RowSelectedDeptHeadReprint = [];
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

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsCompanyId = $("#slSearchCompanyName").val();
        fsOperatorId = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsInvoiceTypeId = $("#slSearchTermInvoice").val() == null || $("#slSearchTermInvoice").val() == "" ? "" : $("#slSearchTermInvoice").val();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        fsInvoiceStatusId = $("#slSearchInvoiceStatus").val() == null || $("#slSearchInvoiceStatus").val() == "" ? -1 : $("#slSearchInvoiceStatus").val();
        fsInvNo = $("#tbInvNo").val();
        fsInvoiceManual = $('input[name=rbInvoiceManual]:checked').val();

        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperatorId,
            strInvoiceType: fsInvoiceTypeId,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            intmstInvoiceStatusId: fsInvoiceStatusId,
            InvNo: fsInvNo,
            invoiceManual: fsInvoiceManual
        };

        var tblSummaryData = $("#tblSummaryData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/PrintInvoiceTower/grid",
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
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        if (full.mstInvoiceStatusId == 27)
                        { strReturn += "<label style='display:none' id='" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "-" + full.InvOperatorID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "'><input type='checkbox' class='checkboxes' /><span></span></label>"; }
                        else
                        {
                            if (Helper.IsElementExistsInArray(full.trxInvoiceHeaderID, full.mstInvoiceCategoryId, Data.RowSelected)) {
                                if (Helper.IsElementExistsInArray(full.trxInvoiceHeaderID, full.mstInvoiceCategoryId, Data.RowSelectedSite)) {
                                    strReturn += "<label style='display:none' id='" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "-" + full.InvOperatorID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "-" + full.InvOperatorID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                                }
                                else {
                                    $("#Row_" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID).addClass("active");
                                    strReturn += "<label id='" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "-" + full.InvOperatorID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "-" + full.InvOperatorID + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                }
                            }
                            else {
                                strReturn += "<label id='" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "-" + full.InvOperatorID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "-" + full.InvOperatorID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }
                        }
                        return strReturn;
                    }
                },
                { data: "InvNo" },
                { data: "PrintStatus" },
                { data: "PrintStatusCover" },
                { data: "ChecklistStatus" },
                { data: "InvStatus" },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvTypeDesc" },
                { data: "InvCompanyId" },
                { data: "InvOperatorID" },
                {
                    data: "InvSumADPP", className: "text-right",
                    render: function (val, type, full) {
                        if (full.InvTotalDiscount > 0)
                            return Common.Format.CommaSeparation(val + full.InvTotalDiscount);
                        else
                            return Common.Format.CommaSeparation(val);
                    }
                },
                { data: "InvTotalDiscount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvTotalAPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvTotalPenalty", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Currency" },
                {
                    mRender: function (data, type, full) {
                        return full.InvPaidStatus == "1" ? "Paid" : "Unpaid";
                    }
                },
                { data: "CNStatus" },
                { data: "InvoiceCategory" },
                { data: "PPHType" },
                { data: "PostingProfile" },
                { data: "PrintUsers" },
                {
                    data: "InvoiceManual", mRender: function (data, type, full) {
                        return full.InvoiceManual ? "Yes" : "No";
                    }
                },
                {
                    data: "InvoiceNonRevenue", mRender: function (data, type, full) {
                        return full.InvoiceNonRevenue ? "Yes" : "No";
                    }
                }
            ],
            "columnDefs": [{ "targets": [0, 2], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    if (this.fnSettings().fnRecordsTotal() > 0) {
                        $(".panelSearchBegin").hide();
                        $(".panelSearchResult").fadeIn();
                    }
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        if (!Helper.IsElementExistsInArray(parseInt(item.HeaderId), parseInt(item.CategoryId), Data.RowSelectedSite)) {
                            $("#Row_" + item.CategoryId + "-" + item.HeaderId).addClass("active");
                            $(".Row_" + item.CategoryId + "-" + item.HeaderId).addClass("active");
                        }
                    }
                }
            },
            "order": [],
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

                                $(".Row_" + id).addClass("active");
                                $("." + id).prop("checked", true);
                                $("." + id).trigger("change");
                            } else {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");
                                $(this).trigger("change");

                                $(".Row_" + id).removeClass("active");
                                $("." + id).prop("checked", false);
                                $("." + id).trigger("change");
                            }
                        }
                    });
                    var allIDs = Helper.GetListId();
                    if (checked) {
                        var tempArr = [];
                        var temp = new Object();
                        $.each(allIDs, function (index, item) {
                            temp = new Object();
                            temp.HeaderId = parseInt(item.HeaderId);
                            temp.CategoryId = parseInt(item.CategoryId);
                            if (!Helper.IsElementExistsInArray(temp.HeaderId, temp.CategoryId, Data.RowSelected))
                                tempArr.push(temp);
                        });
                        Data.RowSelected = Data.RowSelected.concat(tempArr);
                    }
                    else {
                        $.each(allIDs, function (index, item) {
                            Data.RowSelected = Helper.RemoveObjectByIdFromArray(Data.RowSelected, parseInt(item.HeaderId), parseInt(item.CategoryId));
                        });
                    }
                });
            }
        });

    },
    Reset: function () {
        fsInvNo = "";
        fsCompanyId = "";
        fsOperatorId = "";
        fsStartPeriod = "";
        fsEndPeriod = "";
        fsInvoiceStatusId = -1;


        $("#tbInvNo").val("");
        $("#slSearchCompanyName").val("").trigger('change');
        $("#slSearchOperator").val("").trigger('change');
        $("#slSearchTermInvoice").val("").trigger('change');
        $("#tbStartPeriod").val("");
        $("#tbEndPeriod").val("");
        $("#slSearchInvoiceStatus").val("").trigger('change');
        $("#rbAll").prop('checked', true);

    },
    Export: function () {
        window.location.href = "/InvoiceTransaction/TrxPrintInvoiceTower/Export?strCompanyId=" + fsCompanyId + "&strOperator=" + fsOperatorId
           + "&strInvoiceType=" + fsInvoiceTypeId + "&strStartPeriod=" + fsStartPeriod + "&strEndPeriod=" + fsEndPeriod + "&intmstInvoiceStatusId=" + fsInvoiceStatusId + "&invNo=" + fsInvNo + "&invoiceManual=" + fsInvoiceManual
    },
    Print: function () {
        if (Data.RowSelected.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            var tempHeaderId = [];
            var tempCategoryId = [];
            var tempOprId = "";
            $.each(Data.RowSelected, function (index, item) {
                tempHeaderId.push(item.HeaderId);
                tempCategoryId.push(item.CategoryId);
                tempOprId = item.CustomerId;
            });
            var HeaderId = tempHeaderId.join('|');
            var CategoryId = tempCategoryId.join('|');
            var PicaReprintID = $("#slPicaCategory").val() == null || $("#slPicaCategory").val() == null ? 0 : $("#slPicaCategory").val();
            var strDocType = $('input[name=DocType]:checked').val();

            if (strDocType == 0) //DETAIL
                window.location.href = "/InvoiceTransaction/TrxPrintInvoiceTowerDetailCalculation/Export?HeaderId=" + HeaderId + "&CategoryId=" + CategoryId;
            else if (strDocType == 1) //INVOICE & INVOICE WITH COVER
            {

                var ValidatePrint = Helper.ValidatePrint();
                if (ValidatePrint.Result != "") {
                    //if (ValidatePrint.Result == "3")
                    //{
                    //$('#mdlReprint3').modal('show');
                    //}
                    //else
                    Common.Alert.Warning(ValidatePrint.Result);
                }
                else {

                    if (ValidatePrint.isReprint) {
                        if (ValidatePrint.ApprovalStatus == ApprovalStatus.Approved) {
                            //if (strDocType == 2 && tempOprId.toUpperCase() == "XL")
                            //    window.location.href = "/InvoiceTransaction/TrxPrintInvoiceDynamic/Print?HeaderId=" + HeaderId + "&CustomerID=" + tempOprId.toUpperCase();
                            //else
                            window.location.href = "/InvoiceTransaction/TrxPrintInvoiceTower/Print?HeaderId=" + HeaderId + "&CategoryId=" + CategoryId + "&PicaReprintID=" + PicaReprintID;

                            setTimeout(function () {
                                Process.Print();
                            }, 5000);
                        }
                        else if (ValidatePrint.ApprovalStatus == ApprovalStatus.Waiting) {
                            Common.Alert.Warning("The Invoice(s) is Still Waiting for Approval")
                        }
                        else {
                            $("#slPicaCategory").val("").trigger('change');
                            $("#tbRemarks").val("");
                            $('#mdlReprint').modal('show');
                        }
                        Data.PrintCount = ValidatePrint.PrintCount;
                    }
                    else {
                        //if (strDocType == 2 && tempOprId.toUpperCase() == "XL")
                        //    window.location.href = "/InvoiceTransaction/TrxPrintInvoiceDynamic/Print?HeaderId=" + HeaderId + "&CustomerID=" + tempOprId.toUpperCase();
                        //else
                        window.location.href = "/InvoiceTransaction/TrxPrintInvoiceTower/Print?HeaderId=" + HeaderId + "&CategoryId=" + CategoryId + "&PicaReprintID=" + PicaReprintID;

                        setTimeout(function () {
                            Process.Print();
                        }, 5000);
                    }
                }
            } else if (strDocType == 2 && tempOprId.toUpperCase() == "XL") {
                window.location.href = "/InvoiceTransaction/TrxPrintInvoiceDynamic/Print?HeaderId=" + HeaderId + "&CustomerID=" + tempOprId.toUpperCase();
                setTimeout(function () {
                    Process.Print();
                }, 5000);
            }
        }
    }
}

var TableSite = {
    Init: function () {
        var tblSummaryData = $('#tblSiteData').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblSiteData").DataTable().columns.adjust().draw();
        });
    },
    GetSelectedSiteData: function (RowSelected) {
        var AjaxData = [];
        var tempHeaderId = [];
        var tempCategoryId = [];
        $.each(RowSelected, function (index, item) {
            tempHeaderId.push(item.HeaderId);
            tempCategoryId.push(item.CategoryId);
        });

        var params = {
            HeaderId: tempHeaderId,
            CategoryId: tempCategoryId
        }
        var l = Ladda.create(document.querySelector("#btAdd"));
        $.ajax({
            url: "/api/PrintInvoiceTower/InvoicelistGrid",
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
            Common.Alert.Error(errorThrown)
            l.stop();
        });
        return AjaxData;
    },
    AddSite: function () {
        //Get Data that in RowSelected
        var ajaxData = TableSite.GetSelectedSiteData(Data.RowSelected);

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
                        strReturn += "<button type='button' title='Delete' class='btn btn-xs red btDeleteSite' id='btDeleteSite" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "'><i class='fa fa-trash'></i></button>";
                        return strReturn;
                    }
                },
                { data: "InvNo" },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvTypeDesc" },
                { data: "InvCompanyId" },
                { data: "InvOperatorID" },
                {
                    data: "InvSumADPP", className: "text-right",
                    render: function (val, type, full) {
                        if (full.InvTotalDiscount > 0)
                            return Common.Format.CommaSeparation(val + full.InvTotalDiscount);
                        else
                            return Common.Format.CommaSeparation(val);
                    }
                },
                { data: "InvTotalDiscount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvTotalAPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvTotalPenalty", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Currency" },
                {
                    mRender: function (data, type, full) {
                        return full.InvPaidStatus == "1" ? "Paid" : "Unpaid";
                    }
                },
                { data: "CNStatus" },
                { data: "PPHType" },
                { data: "PostingProfile" },
                { data: "PrintUsers" },
                {
                    data: "InvoiceManual", mRender: function (data, type, full) {
                        return full.InvoiceManual ? "Yes" : "No";
                    }
                }

            ],
        });

        $("#tblSiteData tbody").unbind().on("click", "button.btDeleteSite", function (e) {
            var table = $("#tblSiteData").DataTable();
            var buttonId = $(this).attr("id");
            var idComponents = buttonId.split('btDeleteSite');
            var id = idComponents[1];
            //Row with ID in btDeleteSite back to tblSummary with normal state checkbox
            $("#Row_" + id).removeClass('active');
            $("#" + id + " input[type=checkbox]").removeAttr("checked");
            $("#" + id).show();

            /* Uncheck the checkbox in cloned table */
            $('.Row_' + id).removeClass('active');
            $('.' + id + ' input[type=checkbox]').removeAttr("checked");
            $('.' + id).show();

            table.row($(this).parents('tr')).remove().draw();
            //Delete id in Array for rendering when change page
            Data.RowSelected = Helper.RemoveObjectByIdFromArray(Data.RowSelected, id.split("-")[1], id.split("-")[0]);
            Data.RowSelectedSite = Helper.RemoveObjectByIdFromArray(Data.RowSelectedSite, id.split("-")[1], id.split("-")[0]);
            if (Data.RowSelected.length == 0)
                $(".panelSiteData").fadeOut();
        });

        $(window).resize(function () {
            $("#tblSiteData").DataTable().columns.adjust().draw();
        });
    }
}

var TableSiteMerge = {
    Init: function () {
        var tblSummaryData = $('#tblSiteDataMerge').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblSiteDataMerge").DataTable().columns.adjust().draw();
        });
    },
    AddList: function () {
        //Get Data that in RowSelected
        var ajaxData = [];
        var InvoiceAmount = 0;
        var DiscountAmount = 0;
        var PPNAmount = 0;
        var PenaltyAmount = 0;
        var Operator = "";
        var oTable = $('#tblSiteData').DataTable();
        var ListInvoiceHdrId = [];
        oTable.rows('').every(function (rowIdx, tableLoop, rowLoop) {
            ajaxData.push(this.data());
            DiscountAmount += this.data().InvTotalDiscount;
            InvoiceAmount += (this.data().InvSumADPP);
            PPNAmount += this.data().InvTotalAPPN;
            PenaltyAmount += this.data().InvTotalPenalty;
            Operator = this.data().InvOperatorID;
            ListInvoiceHdrId.push(this.data().trxInvoiceHeaderID);
        });
        if (ajaxData.length > 1) {
            $("#tbTotal").val(Common.Format.CommaSeparation(InvoiceAmount + DiscountAmount));
            $("#tbDiscount").val(Common.Format.CommaSeparation(DiscountAmount));
            $("#tbPPN").val(Common.Format.CommaSeparation(PPNAmount));
            $("#tbPenalty").val(Common.Format.CommaSeparation(PenaltyAmount));
            $("#tbCurrency").val(ajaxData[0].Currency);
            $("#tbInvoiceDate").val("");
            Control.SetSubjectTextBox(ListInvoiceHdrId);

            Control.BindingSelectOperatorRegional(Operator);
            $("#slSignature").val(Data.ARDeptHead).trigger("change");
            $("#tbInvoiceDate").datepicker({ format: "dd-M-yyyy" });
            $("#tbInvoiceDate").datepicker('setDate', new Date());
            //Control.SetSubjectTextBox();
            var tblSiteData = $("#tblSiteDataMerge").DataTable({
                "deferRender": true,
                "proccessing": true,
                "data": ajaxData,
                "filter": false,
                "destroy": true,
                "columns": [
                   { data: "InvNo" },
                    { data: "InvTotalAmount", render: $.fn.dataTable.render.number(',', '.', 2, '') }
                ],

            });

            $(window).resize(function () {
                $("#tblSiteDataMerge").DataTable().columns.adjust().draw();
            });
            $("#panelSiteDataMerge").fadeIn();
            $("#pnlSummary").fadeOut();
        }
        else
            Common.Alert.Warning("Invoice Must be More Than One");
    }
}

var TableDepthead = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblSummaryDataDeptHead = $('#tblSummaryDataDeptHead').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblSummaryDataDeptHead").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsCompanyId = $("#slSearchCompanyName").val();
        fsOperatorId = $("#slSearchOperator").val();
        fsInvoiceTypeId = $("#slSearchTermInvoice").val() == null || $("#slSearchTermInvoice").val() == "" ? "" : $("#slSearchTermInvoice").val();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        fsInvoiceStatusId = $("#slSearchInvoiceStatus").val() == null || $("#slSearchInvoiceStatus").val() == "" ? -1 : $("#slSearchInvoiceStatus").val();
        fsInvNo = $("#tbInvNo").val();
        fsInvoiceManual = $('input[name=rbInvoiceManual]:checked').val();

        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperatorId,
            strInvoiceType: fsInvoiceTypeId,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            intmstInvoiceStatusId: fsInvoiceStatusId,
            InvNo: fsInvNo,
            invoiceManual: fsInvoiceManual
        };

        var tblSummaryDataDeptHead = $("#tblSummaryDataDeptHead").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/PrintInvoiceTower/grid",
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
                        if (full.mstInvoiceStatusId != 27)
                        { strReturn += "<label style='display:none' id='" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "'><input type='checkbox' class='checkboxes' /><span></span></label>"; }
                        else
                        {
                            if (Helper.IsElementExistsInArray(parseInt(full.trxInvoiceHeaderID), parseInt(full.mstInvoiceCategoryId), Data.RowSelectedDeptHead)) {
                                //$("#Row_" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID).addClass("active");
                                strReturn += "<label id='" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                            }
                            else {
                                strReturn += "<label id='" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }
                        }
                        return strReturn;
                    }
                },
                { data: "InvNo" },
                { data: "InvStatus" },
                { data: "InvRemarksPrint" },
                { data: "PrintStatus" },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvTypeDesc" },
                { data: "InvCompanyId" },
                { data: "InvOperatorID" },
                {
                    data: "InvSumADPP",
                    render: function (val, type, full) {
                        if (full.InvTotalDiscount > 0)
                            return Common.Format.CommaSeparation(val + full.InvTotalDiscount);
                        else
                            return Common.Format.CommaSeparation(val);
                    }
                },
                { data: "InvTotalDiscount", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvTotalAPPN", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvTotalPenalty", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Currency" },
                {
                    mRender: function (data, type, full) {
                        return full.InvPaidStatus == "1" ? "Paid" : "Unpaid";
                    }
                },
                { data: "ChecklistStatus" },
                { data: "InvoiceCategory" },
                { data: "PPHType" },
                { data: "PostingProfile" },
                { data: "PrintUsers" },
                {
                    data: "InvoiceManual", mRender: function (data, type, full) {
                        return full.InvoiceManual ? "Yes" : "No";
                    }
                },
                {
                    data: "InvoiceNonRevenue", mRender: function (data, type, full) {
                        return full.InvoiceNonRevenue ? "Yes" : "No";
                    }
                }
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
                        $("#Row_" + item.CategoryId + "-" + item.HeaderId).addClass("active");
                    }
                }
            },
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (Data.Role == "DEPT HEAD") {
                    if (aData.mstInvoiceStatusId == 27) {
                        $('td', nRow).css('background-color', '#FF9999');
                    }
                }
                l.stop();
            },
            "order": [],
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
                    '<input type="checkbox" name="SelectAllDeptHead" class="group-checkable" data-set="#tblSummaryData .checkboxes" />' +
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
                            if (checked) {
                                $(this).prop("checked", true);
                                $(this).parents('tr').addClass('active');
                                $(this).trigger("change");

                                $(".Row_" + id).addClass("active");
                                $("." + id).prop("checked", true);
                                $("." + id).trigger("change");
                            } else {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");
                                $(this).trigger("change");

                                $(".Row_" + id).removeClass("active");
                                $("." + id).prop("checked", false);
                                $("." + id).trigger("change");
                            }
                        }
                    });
                    var allIDs = Helper.GetListId();
                    if (checked) {
                        var tempArr = [];
                        var temp = new Object();
                        $.each(allIDs, function (index, item) {
                            temp = new Object();
                            temp.HeaderId = parseInt(item.HeaderId);
                            temp.CategoryId = parseInt(item.CategoryId);
                            if (!Helper.IsElementExistsInArray(temp.HeaderId, temp.CategoryId, Data.RowSelectedDeptHead))
                                tempArr.push(temp);
                        });
                        Data.RowSelectedDeptHead = Data.RowSelectedDeptHead.concat(tempArr);
                    }
                    else {
                        $.each(allIDs, function (index, item) {
                            Data.RowSelectedDeptHead = Helper.RemoveObjectByIdFromArray(Data.RowSelectedDeptHead, parseInt(item.HeaderId), parseInt(item.CategoryId));
                        });
                    }
                });
            }
        });
    }
}

var TableDeptHeadReprint = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblSummaryDataDeptHeadReprint = $('#tblSummaryDataDeptHeadReprint').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblSummaryDataDeptHeadReprint").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsCompanyId = $("#slSearchCompanyName").val();
        fsOperatorId = $("#slSearchOperator").val();
        fsInvoiceTypeId = $("#slSearchTermInvoice").val() == null || $("#slSearchTermInvoice").val() == "" ? "" : $("#slSearchTermInvoice").val();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        fsInvoiceStatusId = $("#slSearchInvoiceStatus").val() == null || $("#slSearchInvoiceStatus").val() == "" ? -1 : $("#slSearchInvoiceStatus").val();
        fsInvNo = $("#tbInvNo").val();
        fsInvoiceManual = $('input[name=rbInvoiceManual]:checked').val();


        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperatorId,
            strInvoiceType: fsInvoiceTypeId,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            intmstInvoiceStatusId: fsInvoiceStatusId,
            InvNo: fsInvNo,
            invoiceManual: fsInvoiceManual
        };

        var tblSummaryDataDeptHeadReprint = $("#tblSummaryDataDeptHeadReprint").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/PrintInvoiceTower/gridReprint",
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
                        TableDeptHeadReprint.Export()
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

                        if (Helper.IsElementExistsInArray(parseInt(full.trxInvoiceHeaderID), parseInt(full.mstInvoiceCategoryId), Data.RowSelectedDeptHeadReprint)) {
                            //$("#Row_" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID).addClass("active");
                            strReturn += "<label id='" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                        }
                        else {
                            strReturn += "<label id='" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                        }

                        return strReturn;
                    }
                },
                { data: "InvNo" },
                { data: "FullName" },
                { data: "PICAReprint" },
                { data: "PICARemarks" },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvTypeDesc" },
                { data: "InvCompanyId" },
                { data: "InvOperatorID" },
                { data: "Currency" },
                { data: "RequestCounter" },
                { data: "ApproveCounter" },
                { data: "RejectCounter" },
                { data: "PrintUsers" },
                {
                    data: "InvoiceManual", mRender: function (data, type, full) {
                        return full.InvoiceManual ? "Yes" : "No";
                    }
                }
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryDataDeptHeadReprint.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelectedDeptHeadReprint.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelectedDeptHeadReprint.length; i++) {
                        item = Data.RowSelectedDeptHeadReprint[i];
                        $("#Row_" + item.CategoryId + "-" + item.HeaderId).addClass("active");
                    }
                }
            },
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (Data.Role == "DEPT HEAD") {
                    if (aData.mstInvoiceStatusId == 27) {
                        $('td', nRow).css('background-color', '#FF9999');
                    }
                }
                l.stop();
            },
            "order": [],
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
                    '<input type="checkbox" name="SelectAllDeptHeadReprint" class="group-checkable" data-set="#tblSummaryDataDeptHeadReprint .checkboxes" />' +
                    '<span></span> ' +
                    '</label>';
                var th = $("th.select-all-dept-head-reprint").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-dept-head-reprint").unbind().on("change", ".group-checkable", function (e) {
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

                                $(".Row_" + id).addClass("active");
                                $("." + id).prop("checked", true);
                                $("." + id).trigger("change");
                            } else {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");
                                $(this).trigger("change");

                                $(".Row_" + id).removeClass("active");
                                $("." + id).prop("checked", false);
                                $("." + id).trigger("change");
                            }
                        }
                    });
                    var allIDs = Helper.GetReprintListId();
                    if (checked) {
                        var tempArr = [];
                        var temp = new Object();
                        $.each(allIDs, function (index, item) {
                            temp = new Object();
                            temp.HeaderId = parseInt(item.HeaderId);
                            temp.CategoryId = parseInt(item.CategoryId);
                            if (!Helper.IsElementExistsInArray(temp.HeaderId, temp.CategoryId, Data.RowSelectedDeptHeadReprint))
                                tempArr.push(temp);
                        });
                        Data.RowSelectedDeptHeadReprint = Data.RowSelectedDeptHeadReprint.concat(tempArr);
                    }
                    else {
                        $.each(allIDs, function (index, item) {
                            Data.RowSelectedDeptHeadReprint = Helper.RemoveObjectByIdFromArray(Data.RowSelectedDeptHeadReprint, parseInt(item.HeaderId), parseInt(item.CategoryId));
                        });
                    }
                });
            }
        });
    },
    Export: function () {
        window.location.href = "/InvoiceTransaction/TrxRePrintApprovalInvoiceTower/Export?strCompanyId=" + fsCompanyId + "&strOperator=" + fsOperatorId
           + "&strInvoiceType=" + fsInvoiceTypeId + "&strStartPeriod=" + fsStartPeriod + "&strEndPeriod=" + fsEndPeriod + "&intmstInvoiceStatusId=" + fsInvoiceStatusId + "&invNo=" + fsInvNo + "&invoiceManual=" + fsInvoiceManual;
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
                    $("#slSearchOperator").append("<option value='" + $.trim(item.OperatorId) + "'>" + item.OperatorId + "</option>");
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
    BindingSelectSignature: function () {
        $.ajax({
            url: "/api/MstDataSource/SignatureUser",
            type: "GET",

        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSignature").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSignature").append("<option value='" + item.UserID + "'>" + item.FullName + "</option>");
                    if (item.HCISPosition == HCISPosition.ARDeptHead)
                        Data.ARDeptHead = item.UserID;
                })
            }

            $("#slSignature").select2({ placeholder: "Select PIC Signature", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectOperatorRegional: function (Operator) {
        var params = {
            OperatorId: Operator
        };
        $.ajax({
            url: "/api/MstDataSource/OperatorRegional",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slOperatorRegional").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slOperatorRegional").append("<option value='" + item.OprRegionId + "'>" + item.OperatorRegion + "</option>");
                })
            }

            $("#slOperatorRegional").select2({ placeholder: "Select Operator Regional", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectInvoiceStatus: function () {
        $("#slSearchInvoiceStatus").html("<option></option>")
        if (Data.Role == "DEPT HEAD") {
            $("#slSearchInvoiceStatus").append("<option value='26'>CN APPROVED</option>");
        }
        else {
            $("#slSearchInvoiceStatus").append("<option value='3'>INVOICE POSTED</option>");
            $("#slSearchInvoiceStatus").append("<option value='4'>INVOICE PRINTED</option>");
            $("#slSearchInvoiceStatus").append("<option value='21'>WAITING AR COLLECTION</option>");
            $("#slSearchInvoiceStatus").append("<option value='22'>RECEIVED AR COLLECTION</option>");
            $("#slSearchInvoiceStatus").append("<option value='23'>REJECTED AR COLLECTION</option>");
        }
        $("#slSearchInvoiceStatus").append("<option value='27'>WAITING FOR APPROVAL CN</option>");
        $("#slSearchInvoiceStatus").select2({ placeholder: "Select Invoice Status", width: null });

    },
    SetSubjectTextBox: function (ListInvoiceHdrId) {
        var params = {
            HeaderId: ListInvoiceHdrId
        };
        $.ajax({
            url: "/api/PrintInvoiceTower/GetSubject",
            type: "POST",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $("#tbDescription").val(data.Result);
            }

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });

    },

    GetCurrentUserRole: function () {
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
                    if ($("#tabApproval").tabs('option', 'active') == 0)
                        TableDepthead.Search();
                    else
                        TableDeptHeadReprint.Search();
                else
                    Table.Search();
            }
        });
    },
    BindingPicaCategory: function () {
        $.ajax({
            url: "/api/MstDataSource/PicaReprint",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slPicaCategory").html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slPicaCategory").append("<option value='" + item.PICAReprintID + "'>" + item.PICAReprint + "</option>");
                })
            }

            $("#slPicaCategory").select2({ placeholder: "Select PICA Category", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    }
}

var Process = {
    AddToSiteList: function () {

        Data.CheckedRow = [];
        Data.isDifferent = false;
        var l = Ladda.create(document.querySelector("#btAdd"))
        var oTable = $('#tblSummaryData').DataTable();
        var oTableSite = $('#tblSiteData').DataTable();
        if (Data.RowSelected.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            var ValidateStatus = Helper.ValidateStatus();
            if (ValidateStatus.Result != "")
                Common.Alert.Warning(ValidateStatus.Result);
            else {
                TableSite.AddSite();
                $(".panelSiteData").fadeIn();
                oTable.rows('.active').every(function (rowIdx, tableLoop, rowLoop) {
                    var HeaderId = this.data().trxInvoiceHeaderID;
                    var CategoryId = this.data().mstInvoiceCategoryId;
                    var checkBoxId = CategoryId + "-" + HeaderId;
                    $("#Row_" + checkBoxId).removeClass('active');
                    $("#" + checkBoxId).hide();
                });

                // Hide the checkboxes in the cloned table
                $.each(Data.RowSelected, function (index, item) {
                    $(".Row_" + item.CategoryId + "-" + item.HeaderId).removeClass('active');
                    $("." + item.CategoryId + "-" + item.HeaderId).hide();
                });

                //insert Data.RowSelectedSite for rendering checkbox
                $.each(Data.RowSelected, function (index, item) {
                    if (!Helper.IsElementExistsInArray(parseInt(item.HeaderId), parseInt(item.CategoryId), Data.RowSelectedSite))
                        Data.RowSelectedSite.push(item);
                });
                //Data.RowSelectedSite = Data.RowSelected;
            }
        }

    },
    ShowMerge: function () {
        TableSiteMerge.AddList();
    },
    PostingMergeInvoices: function () {
        var l = Ladda.create(document.querySelector("#btPosting"))
        var ajaxData = [];
        var oTable = $('#tblSiteData').DataTable();
        oTable.rows('').every(function (rowIdx, tableLoop, rowLoop) {
            ajaxData.push(this.data());
        });
        var params = {
            SumADiscount: parseFloat($("#tbDiscount").val().replace(/,/g, '')),
            SumADPP: parseFloat($("#tbTotal").val().replace(/,/g, '')),
            SumAPPN: $("#tbPPN").val().replace(/,/g, ''),
            SumAPenalty: $("#tbPenalty").val().replace(/,/g, ''),
            ListInvoicePosted: ajaxData,
            strInvoiceDate: $("#tbInvoiceDate").val(),
            strSubject: $("#tbDescription").val(),
            strOperatorRegionId: $("#slOperatorRegional").val(),
            strSignature: $("#slSignature").val(),
            strAdditionalNote: $("#tbAdditionalNote").val(),
        }

        $.ajax({
            url: "/api/PrintInvoiceTower/PostingInvoiceMerge",
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
                Common.Alert.Success("Data Success Merge Invoice With Invoice No :" + data.InvNo)
            }
            l.stop();

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            $("#panelSiteDataMerge").fadeOut();
            $("#pnlSummary").fadeIn();
            Form.ClearRowSelected();
            $(".panelSiteData").fadeOut();
            Table.Search();
        })

    },
    Print: function () {
        var tempHeaderId = [];
        var tempCategoryId = [];
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(item.HeaderId);
            tempCategoryId.push(item.CategoryId);
        });
        /* add by mtr*/
        var strDocType = $('input[name=DocType]:checked').val();
        var fsIsCover = false;
        if (strDocType == 2)
            fsIsCover = true;
        /* add by mtr*/

        var params = {
            HeaderId: tempHeaderId,
            CategoryId: tempCategoryId,
            PICAReprintID: $("#slPicaCategory").val(),
            ReprintRemarks: $("#tbRemarks").val(),
            IsCover: fsIsCover
        }
        var l = Ladda.create(document.querySelector("#btPrintInvoice"));
        $.ajax({
            url: "/api/PrintInvoiceTower/PrintInvoice",
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
            if (Common.CheckError.Object(data)) {
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            $('#mdlReprint').modal('hide');
            $('#mdlReprint3').modal('hide');
            Form.ClearRowSelected();
            Table.Search();
        });

    },
    CNInvoice: function () {
        var tempHeaderId = [];
        var tempCategoryId = [];
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(item.HeaderId);
            tempCategoryId.push(item.CategoryId);
        });

        var params = {
            HeaderId: tempHeaderId,
            CategoryId: tempCategoryId,
            RemarksPrint: $("#tbRemarksARData").val(),
            mstPICATypeID: $("#slPicaTypeARData").val(),
            mstPICADetailID: $("#slPicaDetailARData").val()
        }
        var l = Ladda.create(document.querySelector("#btYesCNARData"));
        $.ajax({
            url: "/api/PrintInvoiceTower/CNInvoiceTower",
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
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Data has been CN and Waiting for Approval")
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            //$('#mdlCNInvoice').modal('hide'); 
            $('.mdlCNARData').modal('hide');
            Form.ClearRowSelected();
            Table.Search();
        });
    },
    ApproveCNInvoice: function () {
        var tempHeaderId = [];
        var tempCategoryId = [];
        $.each(Data.RowSelectedDeptHead, function (index, item) {
            tempHeaderId.push(item.HeaderId);
            tempCategoryId.push(item.CategoryId);
        });

        var params = {
            HeaderId: tempHeaderId,
            CategoryId: tempCategoryId,
            RemarksPrint: $("#tbRemarksCancel").val()
        }
        var l = Ladda.create(document.querySelector("#btApproveCancel"));
        $.ajax({
            url: "/api/PrintInvoiceTower/ApproveCNInvoiceTower",
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
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Data Successfully Approved!");
                Notification.GetList();
            }
            l.stop();

        })
         .fail(function (jqXHR, textStatus, errorThrown) {
             Common.Alert.Error(errorThrown)
             l.stop();
         })
         .always(function (jqXHR, textStatus) {
             Form.Init();
             TableDepthead.Search();
             Form.ClearRowSelected();
         });
    },
    RejectCNInvoice: function () {
        var tempHeaderId = [];
        var tempCategoryId = [];
        $.each(Data.RowSelectedDeptHead, function (index, item) {
            tempHeaderId.push(item.HeaderId);
            tempCategoryId.push(item.CategoryId);
        });

        var params = {
            HeaderId: tempHeaderId,
            CategoryId: tempCategoryId
        }
        var l = Ladda.create(document.querySelector("#btRejectCancel"));
        $.ajax({
            url: "/api/PrintInvoiceTower/RejectCNInvoiceTower",
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
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Data Successfully Reject!");
                Notification.GetList();
            }
            l.stop();

        })
         .fail(function (jqXHR, textStatus, errorThrown) {
             Common.Alert.Error(errorThrown)
             l.stop();
         })
         .always(function (jqXHR, textStatus) {
             Form.Init();
             TableDepthead.Search();
             Form.ClearRowSelected();
         });
    },
    ApprovalReprint: function (Status) {
        var tempHeaderId = [];
        var tempCategoryId = [];
        $.each(Data.RowSelectedDeptHeadReprint, function (index, item) {
            tempHeaderId.push(item.HeaderId);
            tempCategoryId.push(item.CategoryId);
        });

        var params = {
            HeaderId: tempHeaderId,
            CategoryId: tempCategoryId,
            ApprovalStatus: Status
        }
        var l = Ladda.create(document.querySelector("#btPrintInvoice"));
        $.ajax({
            url: "/api/PrintInvoiceTower/ApprovalInvoice",
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
            if (Common.CheckError.Object(data)) {
                if (Status == ApprovalStatus.Approved)
                    Common.Alert.Success("Data Successfully Approved and Can be Printed!");
                else if (Status == ApprovalStatus.Rejected)
                    Common.Alert.Success("Data Successfully Rejected!");
                Notification.GetList();
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Form.ClearRowSelected();
            TableDeptHeadReprint.Search();
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
    IsElementExistsInArray: function (HeaderId, CategoryId, arr) {
        var isExist = false;
        for (var i = 0 ; i < arr.length ; i++) {
            if (arr[i].HeaderId == HeaderId && arr[i].CategoryId == CategoryId) {
                isExist = true;
                break;
            }
        }
        return isExist;
    },
    ValidateStatus: function () {
        var Result = {};

        var tempHeaderId = [];
        var tempCategoryId = [];
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(item.HeaderId);
            tempCategoryId.push(item.CategoryId);
        });

        var params = {
            HeaderId: tempHeaderId,
            CategoryId: tempCategoryId
        }

        var l = Ladda.create(document.querySelector("#btAdd"));
        $.ajax({
            url: "/api/PrintInvoiceTower/GetValidateResultMerge",
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
        return Result;
    }, RemoveObjectByIdFromArray: function (data, id, CategoryId) {
        var data = $.grep(data, function (e) {
            if (e.HeaderId == id) {
                if (e.CategoryId == CategoryId)
                    return false;
                else
                    return true;
            } else {
                return true;
            }
        });

        return data;
    },
    ValidatePrint: function () {
        var Result = {};

        var countExists = 0;
        if (Data.RowSelectedSite.length > 0) {
            $.each(Data.RowSelected, function (index, item) {
                if (Helper.IsElementExistsInArray(parseInt(item.HeaderId), parseInt(item.CategoryId), Data.RowSelectedSite)) {
                    countExists = countExists + 1;
                }
            });
        }

        if (countExists == Data.RowSelected.length) {
            Result.Result = "Please Select One or More Data";
            return Result;
        }

        var tempHeaderId = [];
        var tempCategoryId = [];
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(item.HeaderId);
            tempCategoryId.push(item.CategoryId);
        });

        var params = {
            HeaderId: tempHeaderId,
            CategoryId: tempCategoryId
        }

        var l = Ladda.create(document.querySelector("#btAdd"));
        $.ajax({
            url: "/api/PrintInvoiceTower/GetValidateResultPrint",
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
        return Result;
    },
    ValidateCN: function () {
        var Result = {};

        var countExists = 0;
        if (Data.RowSelectedSite.length > 0) {
            $.each(Data.RowSelected, function (index, item) {
                if (Helper.IsElementExistsInArray(parseInt(item.HeaderId), parseInt(item.CategoryId), Data.RowSelectedSite)) {
                    countExists = countExists + 1;
                }
            });
        }

        if (countExists == Data.RowSelected.length) {
            Result.Result = "Please Select One or More Data";
            return Result;
        }

        var tempHeaderId = [];
        var tempCategoryId = [];
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(item.HeaderId);
            tempCategoryId.push(item.CategoryId);
        });

        var params = {
            HeaderId: tempHeaderId,
            CategoryId: tempCategoryId
        }

        var l = Ladda.create(document.querySelector("#btCNInvoice"));
        $.ajax({
            url: "/api/PrintInvoiceTower/GetValidateResultCN",
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
        return Result;
    },
    UpdateObjectInArray: function (arr, object) {
        var arr = $.grep(arr, function (e) {
            if (e.trxInvoiceHeaderID == object.trxInvoiceHeaderID) {
                e.IsChecked = object.IsChecked;
            }
            return true;
        });
        return arr;
    },
    GetListId: function () {
        //for CheckAll Pages
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperatorId,
            strInvoiceType: fsInvoiceTypeId,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            intmstInvoiceStatusId: fsInvoiceStatusId,
            InvNo: fsInvNo
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/PrintInvoiceTower/GetListId",
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
    },
    ValidateUser: function (role) {
        var result = {};
        var tempRole = "";
        if (role == "SPV")
            tempRole = "SUPERVISOR";
        else if (role == "Head")
            tempRole = "DEPT HEAD";

        var params = {
            UserID: $("#tbUserID" + role).val(),
            Password: $("#tbPassword" + role).val(),
            Role: tempRole
        };

        $.ajax({
            url: "/api/ApprovalCNInvoiceBuilding/ValidateUser",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            async: false,
            cache: false
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                result = data;
            }
        });
        return result;
    },
    GetReprintListId: function () {
        //for CheckAll Pages
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperatorId,
            strInvoiceType: fsInvoiceTypeId,
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            intmstInvoiceStatusId: fsInvoiceStatusId,
            InvNo: fsInvNo
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/PrintInvoiceTower/GetReprintListId",
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

var HCISPosition = {
    ARDeptHead: "Account Receivable Database Department Head"
}

var ApprovalStatus = {
    Approved: "A",
    Rejected: "R",
    Waiting: "W"
}