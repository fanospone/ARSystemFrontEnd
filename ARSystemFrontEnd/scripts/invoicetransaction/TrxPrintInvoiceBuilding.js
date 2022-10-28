Data = {};
Data.ARDeptHead = "";

var fsCompanyName = "";
var fsInvoiceTypeId = "";
var fsStartPeriod = "";
var fsEndPeriod = "";
var fsInvoiceStatusId = -1;
var fsInvNo = "";

jQuery(document).ready(function () {
    Data.RowSelected = [];
    Data.RowSelectedSite = [];
    Data.RowSelectedDeptHead = [];
    Data.RowSelectedDeptHeadReprint = [];
    Form.Init();
    Control.GetCurrentUserRole();
    Control.BindingSelectInvoiceType();
    Control.BindingPicaCategory();
    Control.BindingSelectInvoiceStatus();
    Control.BindingSelectPICAType();
    Table.Reset();
    $(".panelSearchZero").hide();

    //panel Summary
    $("#btSearch").unbind().click(function () {
        if (Data.Role == "DEPT HEAD")
            if ($("#tabApproval").tabs('option', 'active') == 0)
                TableDepthead.Search();
            else
                TableDeptheadReprint.Search();

        else
            Table.Search();
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
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
                $('#mdlCNInvoice').modal('show');
            }
        }
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

    $("#btMerge").unbind().click(function () {
        Process.ShowMerge();
    });

    $("#formTransaction").submit(function (e) {
        Process.PostingMergeInvoices();
        e.preventDefault();
    });

    $("#formReprint").submit(function (e) {
        var tempHeaderId = [];
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(item.HeaderId);
        });
        var HeaderId = tempHeaderId.join('|');
        var strDocType = $('input[name=DocType]:checked').val();
        var strPicaReprintID = $("#slPicaCategory").val();
        if (strDocType == 1) //INVOICE
        {
            if (Data.PrintCount < 2)
                window.location.href = "/InvoiceTransaction/TrxPrintInvoiceBuilding/Print?HeaderId=" + HeaderId + "&PicaReprintID=" + strPicaReprintID;
            setTimeout(function () {
                Process.Print();
            }, 5000);
            e.preventDefault();
        }
    });

    $("#formReject").submit(function (e) {
        Process.CNInvoice();
        e.preventDefault();
    });

    $('#tblSummaryData').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        var temp = new Object();
        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $('.Row_' + id).addClass("active");
            temp.HeaderId = parseInt(id);
            if (!Helper.IsElementExistsInArray(parseInt(id), Data.RowSelected))
                Data.RowSelected.push(temp);
        } else {
            $(this).parents('tr').removeClass("active");
            $('.Row_' + id).removeClass("active");
            Data.RowSelected = Helper.RemoveObjectByIdFromArray(Data.RowSelected, parseInt(id));
        }
    });

    $('#tblSummaryDataDeptHead').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        var temp = new Object();
        if (this.checked) {
            $(this).parents('tr').addClass("active");
            temp.HeaderId = id;
            if (!Helper.IsElementExistsInArray(id, Data.RowSelectedDeptHead))
                Data.RowSelectedDeptHead.push(temp);
        } else {
            $(this).parents('tr').removeClass("active");
            Data.RowSelectedDeptHead = Helper.RemoveObjectByIdFromArray(Data.RowSelectedDeptHead, parseInt(id));
        }
    });

    $('#tblSummaryDataDeptHeadReprint').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        var temp = new Object();
        if (this.checked) {
            $(this).parents('tr').addClass("active");
            temp.HeaderId = id;
            if (!Helper.IsElementExistsInArray(id, Data.RowSelectedDeptHeadReprint))
                Data.RowSelectedDeptHeadReprint.push(temp);
        } else {
            $(this).parents('tr').removeClass("active");
            Data.RowSelectedDeptHeadReprint = Helper.RemoveObjectByIdFromArray(Data.RowSelectedDeptHeadReprint, parseInt(id));
        }
    });

    $("#btApproveHead").unbind().on("click", function () {
        var isFormValid = $("#formReprintHead").parsley().validate();
        if (isFormValid == true) {
            var validationResult = Helper.ValidateUser("Head");
            $("#lblHeadError").text(validationResult.Result);
            if (validationResult.Result == "") {
                var tempHeaderId = [];
                $.each(Data.RowSelected, function (index, item) {
                    tempHeaderId.push(item.HeaderId);
                });
                var HeaderId = tempHeaderId.join('|');
                window.location.href = "/InvoiceTransaction/TrxPrintInvoiceBuilding/Print?HeaderId=" + HeaderId;
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
            TableDeptheadReprint.Search();
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
});

var Form = {
    Init: function () {
        if (!$("#hdAllowProcess").val()) {
            $("#btAdd").hide();
            $("#btPrintInvoice").hide();
        }
        

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
        TableDeptheadReprint.Init();
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

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsCompanyName = $("#tbSearchCompanyName").val();
        fsInvoiceTypeId = $("#slSearchTermInvoice").val() == null || $("#slSearchTermInvoice").val() == "" ? "" : $("#slSearchTermInvoice").val();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        fsInvoiceStatusId = $("#slSearchInvoiceStatus").val() == null || $("#slSearchInvoiceStatus").val() == "" ? -1 : $("#slSearchInvoiceStatus").val();
        fsInvNo = $("#tbInvNo").val();

        var params = {
            CompanyName: fsCompanyName,
            InvoiceTypeId: fsInvoiceTypeId,
            StartPeriod: fsStartPeriod,
            EndPeriod: fsEndPeriod,
            InvoiceStatusId: fsInvoiceStatusId,
            InvNo: fsInvNo
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/PrintInvoiceBuilding/grid",
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
                        if (full.mstInvoiceStatusId == 27) {
                            strReturn += "<label style='display:none' id='" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxInvoiceHeaderID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                        }
                        else {
                            if (Helper.IsElementExistsInArray(parseInt(full.trxInvoiceHeaderID), Data.RowSelected)) {
                                if (Helper.IsElementExistsInArray(parseInt(full.trxInvoiceHeaderID), Data.RowSelectedSite)) {
                                    strReturn += "<label style='display:none' id='" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxInvoiceHeaderID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                                }
                                else {
                                    $(".Row_" + full.trxInvoiceHeaderID).addClass("active");
                                    strReturn += "<label id='" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxInvoiceHeaderID + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                }
                            }
                            else {
                                strReturn += "<label id='" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxInvoiceHeaderID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }
                        }
                        return strReturn;
                    }
                },
                { data: "InvNo" },
                { data: "PrintStatus" },
                { data: "ChecklistStatus" },
                { data: "InvStatus" },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "Term" },
                { data: "CompanyTBG" },
                {
                    data: "Company", render: function (val, type, full) {
                        return full.CompanyType + " " + val;
                    }
                },
                {
                    data: "InvSumADPP", render: function (val, type, full) {
                        if (full.Discount > 0)
                            return Common.Format.CommaSeparation(val + full.Discount);
                        else
                            return Common.Format.CommaSeparation(val);
                    }
                },
                { data: "Discount", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvTotalAPPN", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvTotalPenalty", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Currency" },
                {
                    data: "StartPeriod", render: function (val, type, full) {
                        return Common.Format.ConvertJSONDateTime(val);
                    }
                },
                {
                    data: "EndPeriod", render: function (val, type, full) {
                        return Common.Format.ConvertJSONDateTime(val);
                    }
                },
                {
                    data: "InvPaidStatus",
                    mRender: function (data, type, full) {
                        return full.InvPaidStatus != "0" ? "Paid" : "Unpaid";
                    }
                },
                { data: "PrintUsers" }
            ],
            "columnDefs": [{
                "targets": [0, 2],
                "orderable": false
            }],
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

                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        if (!Helper.IsElementExistsInArray(parseInt(item.HeaderId), Data.RowSelectedSite)) {
                            $(".Row_" + item.HeaderId).addClass("active");
                            $("#Row_" + item.HeaderId).addClass("active");
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
                            temp.HeaderId = parseInt(item);
                            if (!Helper.IsElementExistsInArray(temp.HeaderId, Data.RowSelected))
                                tempArr.push(temp);
                        });
                        Data.RowSelected = Data.RowSelected.concat(tempArr);
                    }
                    else {
                        $.each(allIDs, function (index, item) {
                            Data.RowSelected = Helper.RemoveObjectByIdFromArray(Data.RowSelected, parseInt(item));
                        });
                    }
                });
            }
        });

    },
    Reset: function () {
        fsCompanyName = "";
        fsInvNo = "";
        fsInvoiceTypeId = "";
        fsStartPeriod = "";
        fsEndPeriod = "";
        fsInvoiceStatusId = -1;

        $("#tbSearchCompanyName").val("");
        $("#tbInvNo").val("");
        $("#slSearchTermInvoice").val("").trigger('change');
        $("#tbStartPeriod").val("");
        $("#tbEndPeriod").val("");
        $("#slSearchInvoiceStatus").val("").trigger("change");
    },
    Export: function () {

        //var startPeriod = Common.Format.ConvertJSONDateTime(fsStartPeriod);
        //var endPeriod = Common.Format.ConvertJSONDateTime(fsEndPeriod);

        var href = "/InvoiceTransaction/TrxPrintInvoiceBuilding/Export?companyName=" + fsCompanyName + "&invoiceTypeId=" + fsInvoiceTypeId + "&invoiceStatusId=" + fsInvoiceStatusId + "&invNo=" + fsInvNo;

        href += "&startPeriod=" + fsStartPeriod;
        href += "&endPeriod=" + fsEndPeriod;

        window.location.href = href;
    },
    Print: function () {
        if (Data.RowSelected.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            var tempHeaderId = [];
            $.each(Data.RowSelected, function (index, item) {
                tempHeaderId.push(parseInt(item.HeaderId));
            });
            var HeaderId = tempHeaderId.join('|');
            var strDocType = $('input[name=DocType]:checked').val();
            var strPicaReprintID = $("#slPicaCategory").val();

            if (strDocType == 0) //DETAIL
                window.location.href = "/InvoiceTransaction/TrxPrintInvoiceBuildingDetailCalculation/Export?HeaderId=" + HeaderId;
            else if (strDocType == 1) //INVOICE
            {
                var ValidatePrint = Helper.ValidatePrint();
                if (ValidatePrint.Result != "") {
                    //if (ValidatePrint.Result == "3") {
                    //    $('#mdlReprint3').modal('show');
                    //}
                    //else
                    Common.Alert.Warning(ValidatePrint.Result);
                }
                else {
                    if (ValidatePrint.isReprint) {
                        if (ValidatePrint.ApprovalStatus == ApprovalStatus.Approved) {
                            window.location.href = "/InvoiceTransaction/TrxPrintInvoiceBuilding/Print?HeaderId=" + HeaderId + "&PicaReprintID=" + strPicaReprintID;
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
                        window.location.href = "/InvoiceTransaction/TrxPrintInvoiceBuilding/Print?HeaderId=" + HeaderId + "&PicaReprintID=" + strPicaReprintID;;
                        setTimeout(function () {
                            Process.Print();
                        }, 5000);
                    }
                }
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
        $.each(RowSelected, function (index, item) {
            tempHeaderId.push(parseInt(item.HeaderId));
        });

        var params = {
            HeaderId: tempHeaderId
        }
        var l = Ladda.create(document.querySelector("#btAdd"));
        $.ajax({
            url: "/api/PrintInvoiceBuilding/InvoicelistGrid",
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
                        strReturn += "<button type='button' title='Delete' class='btn btn-xs red btDeleteSite' id='btDeleteSite" + full.trxInvoiceHeaderID + "'><i class='fa fa-trash'></i></button>";
                        return strReturn;
                    }
                },
                { data: "InvNo" },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "Term" },
                { data: "CompanyTBG" },
                {
                    data: "Company", render: function (val, type, full) {
                        return full.CompanyType + " " + val;
                    }
                },
                {
                    data: "InvSumADPP", render: function (val, type, full) {
                        if (full.Discount > 0)
                            return Common.Format.CommaSeparation(val + full.Discount);
                        else
                            return Common.Format.CommaSeparation(val);
                    }
                },
                { data: "Discount", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvTotalAPPN", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvTotalPenalty", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Currency" },
                {
                    data: "StartPeriod", render: function (val, type, full) {
                        return Common.Format.ConvertJSONDateTime(val);
                    }
                },
                {
                    data: "EndPeriod", render: function (val, type, full) {
                        return Common.Format.ConvertJSONDateTime(val);
                    }
                },
                {
                    data: "InvPaidStatus",
                    mRender: function (data, type, full) {
                        return full.InvPaidStatus != "0" ? "Paid" : "Unpaid";
                    }
                },
                { data: "ChecklistStatus" },
                { data: "PrintUsers" }
            ],
            "columnDefs": [{
                "targets": [0],
                "orderable": false
            }],
            "order": []
        });

        $("#tblSiteData tbody").unbind().on("click", "button.btDeleteSite", function (e) {
            var table = $("#tblSiteData").DataTable();
            var buttonId = $(this).attr("id");
            var idComponents = buttonId.split('btDeleteSite');
            var id = parseInt(idComponents[1]);

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
            Data.RowSelected = Helper.RemoveObjectByIdFromArray(Data.RowSelected, id);
            Data.RowSelectedSite = Helper.RemoveObjectByIdFromArray(Data.RowSelectedSite, id);
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
        var oTable = $('#tblSiteData').DataTable();
        oTable.rows('').every(function (rowIdx, tableLoop, rowLoop) {
            ajaxData.push(this.data());
            DiscountAmount += this.data().Discount;
            InvoiceAmount += (this.data().InvSumADPP);
            PPNAmount += this.data().InvTotalAPPN;
            PenaltyAmount += this.data().InvTotalPenalty;
        });
        if (ajaxData.length > 1) {
            Control.BindingSelectSignature();

            $("#tbTotal").val(Common.Format.CommaSeparation(InvoiceAmount + DiscountAmount));
            $("#tbDiscount").val(Common.Format.CommaSeparation(DiscountAmount));
            $("#tbPPN").val(Common.Format.CommaSeparation(PPNAmount));
            $("#tbPenalty").val(Common.Format.CommaSeparation(PenaltyAmount));
            $("#tbCurrency").val(ajaxData[0].Currency);
            $("#tbInvoiceDate").datepicker({ format: "dd-M-yyyy" });
            $("#tbInvoiceDate").datepicker('setDate', new Date());
            $("#tbDescription").val("");
            $("#slSignature").val(Data.ARDeptHead).trigger("change");
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

        fsCompanyName = $("#tbSearchCompanyName").val();
        fsInvoiceTypeId = $("#slSearchTermInvoice").val() == null || $("#slSearchTermInvoice").val() == "" ? "" : $("#slSearchTermInvoice").val();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        fsInvoiceStatusId = $("#slSearchInvoiceStatus").val() == null || $("#slSearchInvoiceStatus").val() == "" ? -1 : $("#slSearchInvoiceStatus").val();
        fsInvNo = $("#tbInvNo").val();

        var params = {
            CompanyName: fsCompanyName,
            InvoiceTypeId: fsInvoiceTypeId,
            StartPeriod: fsStartPeriod,
            EndPeriod: fsEndPeriod,
            InvoiceStatusId: fsInvoiceStatusId,
            InvNo: fsInvNo
        };

        var tblSummaryDataDeptHead = $("#tblSummaryDataDeptHead").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/PrintInvoiceBuilding/grid",
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
                         if (full.mstInvoiceStatusId != 27) {
                             strReturn += "<label style='display:none' id='" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxInvoiceHeaderID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                         }
                         else {
                             if (Helper.IsElementExistsInArray(parseInt(full.trxInvoiceHeaderID), Data.RowSelectedDeptHead)) {
                                 if (Helper.IsElementExistsInArray(parseInt(full.trxInvoiceHeaderID), Data.RowSelectedSite)) {
                                     strReturn += "<label style='display:none' id='" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxInvoiceHeaderID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                                 }
                                 else {
                                     $(".Row_" + full.trxInvoiceHeaderID).addClass("active");
                                     strReturn += "<label id='" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxInvoiceHeaderID + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                 }
                             }
                             else {
                                 strReturn += "<label id='" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxInvoiceHeaderID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
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
                { data: "Term" },
                { data: "CompanyTBG" },
                {
                    data: "Company", render: function (val, type, full) {
                        return full.CompanyType + " " + val;
                    }
                },
                {
                    data: "InvSumADPP",
                    render: function (val, type, full) {
                        if (full.Discount > 0)
                            return Common.Format.CommaSeparation(val + full.Discount);
                        else
                            return Common.Format.CommaSeparation(val);
                    }
                },
                { data: "Discount" },
                { data: "InvTotalAPPN", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvTotalPenalty", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Currency" },
                {
                    data: "StartPeriod", render: function (val, type, full) {
                        return Common.Format.ConvertJSONDateTime(val);
                    }
                },
                {
                    data: "EndPeriod", render: function (val, type, full) {
                        return Common.Format.ConvertJSONDateTime(val);
                    }
                },
                {
                    data: "InvPaidStatus",
                    mRender: function (data, type, full) {
                        return full.InvPaidStatus != "0" ? "Paid" : "Unpaid";
                    }
                },
                { data: "ChecklistStatus" },
                { data: "PrintUsers" }
            ],
            "order": [],
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
                        $("#Row" + item.HeaderId).addClass("active");
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
                    '<input type="checkbox" name="SelectAllDeptHead" class="group-checkable" data-set="#tblSummaryDataDeptHead .checkboxes" />' +
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
                            temp.HeaderId = parseInt(item);
                            if (!Helper.IsElementExistsInArray(temp.HeaderId, Data.RowSelectedDeptHead))
                                tempArr.push(temp);
                        });
                        Data.RowSelectedDeptHead = Data.RowSelectedDeptHead.concat(tempArr);
                    }
                    else {
                        $.each(allIDs, function (index, item) {
                            Data.RowSelectedDeptHead = Helper.RemoveObjectByIdFromArray(Data.RowSelectedDeptHead, parseInt(item));
                        });
                    }
                });
            }

        });
    }
}

var TableDeptheadReprint = {
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

        fsCompanyName = $("#tbSearchCompanyName").val();
        fsInvoiceTypeId = $("#slSearchTermInvoice").val() == null || $("#slSearchTermInvoice").val() == "" ? "" : $("#slSearchTermInvoice").val();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        fsInvoiceStatusId = $("#slSearchInvoiceStatus").val() == null || $("#slSearchInvoiceStatus").val() == "" ? -1 : $("#slSearchInvoiceStatus").val();
        fsInvNo = $("#tbInvNo").val();

        var params = {
            CompanyName: fsCompanyName,
            InvoiceTypeId: fsInvoiceTypeId,
            StartPeriod: fsStartPeriod,
            EndPeriod: fsEndPeriod,
            InvoiceStatusId: fsInvoiceStatusId,
            InvNo: fsInvNo
        };

        var tblSummaryDataDeptHeadReprint = $("#tblSummaryDataDeptHeadReprint").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/PrintInvoiceBuilding/gridReprint",
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
                        TableDeptheadReprint.Export()
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
                         if (Helper.IsElementExistsInArray(parseInt(full.trxInvoiceHeaderID), Data.RowSelectedDeptHeadReprint)) {
                             // $(".Row_" + full.trxInvoiceHeaderID).addClass("active");
                             strReturn += "<label id='" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxInvoiceHeaderID + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                         }
                         else {
                             strReturn += "<label id='" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxInvoiceHeaderID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
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
                { data: "Term" },
                { data: "InvCompanyId" },
                { data: "Company" },
                { data: "Currency" },
                { data: "RequestCounter" },
                { data: "ApproveCounter" },
                { data: "RejectCounter" },
                { data: "PrintUsers" }
            ],
            "order": [],
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
                        $("#Row" + item.HeaderId).addClass("active");
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
                            temp.HeaderId = parseInt(item);
                            if (!Helper.IsElementExistsInArray(temp.HeaderId, Data.RowSelectedDeptHeadReprint))
                                tempArr.push(temp);
                        });
                        Data.RowSelectedDeptHeadReprint = Data.RowSelectedDeptHeadReprint.concat(tempArr);
                    }
                    else {
                        $.each(allIDs, function (index, item) {
                            Data.RowSelectedDeptHeadReprint = Helper.RemoveObjectByIdFromArray(Data.RowSelectedDeptHeadReprint, parseInt(item));
                        });
                    }
                });
            }

        });
    },
    Export: function () {

        //var startPeriod = Common.Format.ConvertJSONDateTime(fsStartPeriod);
        //var endPeriod = Common.Format.ConvertJSONDateTime(fsEndPeriod);

        var href = "/InvoiceTransaction/TrxRePrintInvoiceBuilding/Export?companyName=" + fsCompanyName + "&invoiceTypeId=" + fsInvoiceTypeId + "&invoiceStatusId=" + fsInvoiceStatusId + "&invNo=" + fsInvNo;

        href += "&startPeriod=" + fsStartPeriod;
        

        //if (endPeriod != null && endPeriod != "") {
        href += "&endPeriod=" + fsEndPeriod;
        //}

        window.location.href = href;
    }
}

var Control = {
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#tbSearchCompanyName").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#tbSearchCompanyName").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
            }

            $("#tbSearchCompanyName").select2({ placeholder: "Select Company Name", width: null });

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
    BindingSelectSignature: function () {
        $.ajax({
            url: "/api/MstDataSource/SignatureUser",
            type: "GET",
            async: false
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
    SetSubjectTextBox: function () {
        var params = {
            trxInvoiceHeaderId: Data.Selected.trxInvoiceHeaderID
        };
        $.ajax({
            url: "/api/PostingInvoiceBuilding/GetSubject",
            type: "GET",
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
                        $("#btRejectCancel").show();
                        $("#btApprove").show();
                    }
                    else {
                        $("#btApproveCancel").show();
                        $("#btRejectCancel").show();
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
                        $("#btRejectCancel").hide();
                        $("#btRequest").hide();
                        $("#btApprove").hide();
                    }
                    else {
                        $(".disabledCtrl").prop('disabled', false);
                        $("#btCancelInvoiceTemp").show();
                        $("#btPosting").show();
                        $("#btApproveCancel").hide();
                        $("#btRejectCancel").hide();
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
                    TableDepthead.Search();
                else
                    Table.Search();
            }
        });
    },
    BindingSelectPICAType: function () {
        $.ajax({
            url: "/api/MstDataSource/PICATypeARData",
            type: "GET",
            async: false,

        })
        .done(function (data, textStatus, jqXHR) {
            $("#slPicaTypeARData").html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slPicaTypeARData").append("<option value='" + item.mstPICATypeID + "'>" + item.Description + "</option>");
                })
            }

            $("#slPicaTypeARData").select2({ placeholder: "Select PICA Type", width: null }).on("change", function (e) {

            });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
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
                    var checkBoxId = HeaderId;
                    $("#Row_" + checkBoxId).removeClass('active');
                    $("#" + checkBoxId).hide();
                });

                // Hide the checkboxes in the cloned table
                $.each(Data.RowSelected, function (index, item) {
                    $(".Row_" + item.HeaderId).removeClass('active');
                    $("." + item.HeaderId).hide();
                });

                //insert Data.RowSelectedSite for rendering checkbox
                $.each(Data.RowSelected, function (index, item) {
                    if (!Helper.IsElementExistsInArray(item.HeaderId, Data.RowSelectedSite))
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

        var HeaderId = [];
        $.each(ajaxData, function (index, item) {
            HeaderId.push(item.trxInvoiceHeaderID);
        });

        var params = {
            SumDiscount: $("#tbDiscount").val().replace(/,/g, ''),
            SumADPP: $("#tbTotal").val().replace(/,/g, ''),
            SumAPPN: $("#tbPPN").val().replace(/,/g, ''),
            SumAPenalty: $("#tbPenalty").val().replace(/,/g, ''),
            ListInvoicePosted: ajaxData,
            InvoiceDate: $("#tbInvoiceDate").val(),
            Subject: $("#tbDescription").val(),
            Signature: $("#slSignature").val(),
            HeaderId: HeaderId
        }

        $.ajax({
            url: "/api/PrintInvoiceBuilding/PostingInvoiceMerge",
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
                Common.Alert.Success("Merge Invoice With Invoice No: " + data.InvNo + " has been submitted successfully.")
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
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(item.HeaderId);
        });

        var params = {
            HeaderId: tempHeaderId,
            PICAReprintID: $("#slPicaCategory").val(),
            ReprintRemark: $("#tbRemarks").val()
        }
        var l = Ladda.create(document.querySelector("#btPrintInvoice"));
        $.ajax({
            url: "/api/PrintInvoiceBuilding/PrintInvoice",
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
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(item.HeaderId);
        });

        var params = {
            HeaderId: tempHeaderId,
            RemarkCN: $("#tbRemarksCancel").val(),
            InvoiceStatusId: $("#slPicaTypeARData").val()
        }

        var l = Ladda.create(document.querySelector("#btRequest"));
        $.ajax({
            url: "/api/PrintInvoiceBuilding/CNInvoiceBuilding",
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
                Common.Alert.Success("Request CN Invoice has been submitted successfully.");
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            $('#mdlCNInvoice').modal('hide');
            Form.ClearRowSelected();
            Table.Search();
        });
    },
    ApproveCNInvoice: function () {
        var tempHeaderId = [];
        $.each(Data.RowSelectedDeptHead, function (index, item) {
            tempHeaderId.push(item.HeaderId);
        });

        var params = {
            HeaderId: tempHeaderId,
            RemarkCN: $("#tbRemarksCancel").val()
        }
        var l = Ladda.create(document.querySelector("#btApproveCancel"));
        $.ajax({
            url: "/api/PrintInvoiceBuilding/ApproveCNInvoiceBuilding",
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
                Common.Alert.Success("CN Invoice request has been approved.");
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
        $.each(Data.RowSelectedDeptHead, function (index, item) {
            tempHeaderId.push(item.HeaderId);
        });

        var params = {
            HeaderId: tempHeaderId
        }
        var l = Ladda.create(document.querySelector("#btRejectCancel"));
        $.ajax({
            url: "/api/PrintInvoiceBuilding/RejectCNInvoiceBuilding",
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
                Common.Alert.Success("CN Invoice request has been Rejected.");
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
        });

        var params = {
            HeaderId: tempHeaderId,
            ApprovalStatus: Status
        }
        var l = Ladda.create(document.querySelector("#btPrintInvoice"));
        $.ajax({
            url: "/api/PrintInvoiceBuilding/ApprovalInvoice",
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
            TableDeptheadReprint.Search();
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
    IsElementExistsInArray: function (HeaderId, arr) {
        var isExist = false;
        for (var i = 0 ; i < arr.length ; i++) {
            if (parseInt(arr[i].HeaderId) == parseInt(HeaderId)) {
                isExist = true;
                break;
            }
        }
        return isExist;
    },
    ValidateStatus: function () {
        var Result = {};

        var tempHeaderId = [];
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(item.HeaderId);
        });

        var params = {
            HeaderId: tempHeaderId,
        }

        var l = Ladda.create(document.querySelector("#btAdd"));
        $.ajax({
            url: "/api/PrintInvoiceBuilding/GetValidateResultMerge",
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
    RemoveObjectByIdFromArray: function (data, id) {
        var data = $.grep(data, function (e) {
            if (e.HeaderId == id) {
                return false;
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
                if (Helper.IsElementExistsInArray(parseInt(item.HeaderId), Data.RowSelectedSite)) {
                    countExists = countExists + 1;
                }
            });
        }

        if (countExists == Data.RowSelected.length) {
            Result.Result = "Please Select One or More Data";
            return Result;
        }

        var tempHeaderId = [];
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(item.HeaderId);
        });

        var params = {
            HeaderId: tempHeaderId
        }

        var l = Ladda.create(document.querySelector("#btAdd"));
        $.ajax({
            url: "/api/PrintInvoiceBuilding/GetValidateResultPrint",
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
                if (Helper.IsElementExistsInArray(parseInt(item.HeaderId), Data.RowSelectedSite)) {
                    countExists = countExists + 1;
                }
            });
        }

        if (countExists == Data.RowSelected.length) {
            Result.Result = "Please Select One or More Data";
            return Result;
        }

        var tempHeaderId = [];
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(item.HeaderId);
        });

        var params = {
            HeaderId: tempHeaderId
        }

        var l = Ladda.create(document.querySelector("#btCNInvoice"));
        $.ajax({
            url: "/api/PrintInvoiceBuilding/GetValidateResultCN",
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
    ReverseDateToSQLFormat: function (dateValue) {
        if (dateValue != "") {
            var dateComponents = dateValue.split("-");
            var dateValue = dateComponents[0];
            var monthValue = dateComponents[1];
            var yearValue = dateComponents[2];
            var allMonths = "JanFebMarAprMayJunJulAugSepOctNovDec";

            var monthNumberValue = allMonths.indexOf(monthValue) / 3 + 1;

            return yearValue + "-" + monthNumberValue + "-" + dateValue;
        }
        return "";
    },
    GetListId: function () {
        //for CheckAll Pages
        var params = {
            CompanyName: fsCompanyName,
            InvoiceTypeId: fsInvoiceTypeId,
            StartPeriod: fsStartPeriod,
            EndPeriod: fsEndPeriod,
            InvoiceStatusId: fsInvoiceStatusId,
            InvNo: fsInvNo
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/PrintInvoiceBuilding/GetListId",
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
                console.log(data);
                result = data;
            }
        });
        return result;
    },
    GetReprintListId: function () {
        //for CheckAll Pages
        var params = {
            CompanyName: fsCompanyName,
            InvoiceTypeId: fsInvoiceTypeId,
            StartPeriod: fsStartPeriod,
            EndPeriod: fsEndPeriod,
            InvoiceStatusId: fsInvoiceStatusId,
            InvNo: fsInvNo
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/PrintInvoiceBuilding/GetReprintListId",
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