Data = {};

/* Helper Functions */

var fsCompanyId = "";
var fsCustomerName = "";
var fsInvNo = "";

jQuery(document).ready(function () {
    Data.RowSelected = [];
    Data.RowSelectedDeptHeadReprint = [];
    Table.Init();
    Control.BindingSelectCompany();
    Control.BindingPicaCategory();
    Control.GetCurrentUserRole();

    $("#formReprint").parsley();
    $("#formReprintHead").parsley();


    //panel Summary
    $("#btSearch").unbind().click(function () {
        if (Data.Role == "DEPT HEAD")
            TableDeptheadReprint.Search();

        else
            Table.Search();
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
    });

    $('#tblSummaryData').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        var temp = new Object();
        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row_" + id).addClass("active");
            temp.trxInvoiceHeaderID = parseInt(id);
            Data.RowSelected.push(temp);
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row_" + id).removeClass("active");
            Data.RowSelected = Helper.RemoveObjectByIdFromArray(Data.RowSelected, parseInt(id));
        }
    });

    $("#btPrintInvoice").unbind().click(function () {
        Table.Print();
    });

    $("#btCancelMergeInvoice").unbind().click(function () {
        if (Data.RowSelected.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            var validateCancelMerge = Helper.ValidateCancelMerge();
            if (validateCancelMerge.Result == "")
                MergedInvoiceBuilding.CancelMerge();
            else
                Common.Alert.Warning(validateCancelMerge.Result);
        }
    });

    $("#formReprint").submit(function (e) {
        e.preventDefault();
        var tempHeaderId = [];
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(item.trxInvoiceHeaderID);
        });
        var HeaderId = tempHeaderId.join('|');
        if (Data.PrintCount < 2)
            window.location.href = "/InvoiceTransaction/TrxPrintMergedInvoiceBuilding/Print?HeaderId=" + HeaderId;
        setTimeout(function () {
            MergedInvoiceBuilding.Print();
        }, 5000);
        
    });
    $("#btApproveHead").unbind().on("click", function () {
        var isFormValid = $("#formReprintHead").parsley().validate();
        if (isFormValid == true) {
            var validationResult = Helper.ValidateUser("Head");
            $("#lblHeadError").text(validationResult.Result);
            if (validationResult.Result == "") {
                var tempHeaderId = [];
                $.each(Data.RowSelected, function (index, item) {
                    tempHeaderId.push(item.trxInvoiceHeaderID);
                });
                var HeaderId = tempHeaderId.join('|');
                window.location.href = "/InvoiceTransaction/TrxPrintMergedInvoiceBuilding/Print?HeaderId=" + HeaderId;
                setTimeout(function () {
                    MergedInvoiceBuilding.Print();
                }, 5000);
                e.preventDefault();
            }
        }
    });
    $('#tblSummaryDataDeptHeadReprint').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        var temp = new Object();
        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row_" + id).addClass("active");
            temp.trxInvoiceHeaderID = parseInt(id);
            Data.RowSelectedDeptHeadReprint.push(temp);
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row_" + id).removeClass("active");
            Data.RowSelectedDeptHeadReprint = Helper.RemoveObjectByIdFromArray(Data.RowSelectedDeptHeadReprint, parseInt(id));
        }

    });
    $("#btApproveReprint").unbind().click(function () {
        if (Data.RowSelectedDeptHeadReprint.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else
            MergedInvoiceBuilding.ApprovalReprint(ApprovalStatus.Approved);
    });
    $("#btRejectReprint").unbind().click(function () {
        if (Data.RowSelectedDeptHeadReprint.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else
            MergedInvoiceBuilding.ApprovalReprint(ApprovalStatus.Rejected);
    });
});

var Table = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblSummaryData = $('#tblSummaryData').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $("#tblSummaryData tbody").on("click", "button.btEdit", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
            }
        });

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val(),
        fsCustomerName = $("#tbSearchCustomerName").val(),
        fsInvNo = $("#tbInvNo").val()

        var params = {
            companyId: fsCompanyId,
            customerName: fsCustomerName,
            InvNo: fsInvNo
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/ManageMergedInvoiceOnlyBuilding/grid",
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
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        if (Helper.DoesElementExistInArray(parseInt(full.trxInvoiceHeaderID), Data.RowSelected)) {
                            $("#Row_" + full.trxInvoiceHeaderID).addClass("active");
                            strReturn += "<label id='" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxInvoiceHeaderID + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                        }
                        else {
                            strReturn += "<label id='" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxInvoiceHeaderID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                        }
                        return strReturn;
                    }
                },
                { data: "InvNo" },
                { data: "PrintStatus" },
                { data: "Company" },
                { data: "CustomerName" },
                {
                    data: "InvSumADPP", className: "text-right", render: function (data, type, full) {
                        return Common.Format.CommaSeparation(data + full.Discount);
                    }
                },
                {
                    data: "Discount", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "InvTotalAPPN", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "InvTotalPenalty", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                { data: "Currency" },
                { data: "PrintUsers" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").show();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        $("#Row_" + item.trxInvoiceHeaderID).addClass("active");
                    }
                }
            },
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': [0, 2]
            }],
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
                            temp.trxInvoiceHeaderID = parseInt(item);
                            if (!Helper.DoesElementExistInArray(temp.trxInvoiceHeaderID, Data.RowSelected))
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
        fsCustomerName = "";
        fsInvNo = "";
        fsCompanyId = "";
        $("#tbSearchCustomerName").val("");
        $("#tbInvNo").val("");
        $("#slSearchCompany").val("").trigger("change");
    },
    Export: function () {
        window.location.href = "/InvoiceTransaction/TrxManageMergedInvoiceOnlyBuilding/Export?customerName=" + fsCustomerName + "&companyId=" + fsCompanyId + "&invNo=" + fsInvNo;
    },
    Print: function () {
        if (Data.RowSelected.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            var tempHeaderId = [];
            $.each(Data.RowSelected, function (index, item) {
                tempHeaderId.push(item.trxInvoiceHeaderID);
            });
            var listHeaderId = tempHeaderId.join('|');
            var strDocType = $('input[name=DocType]:checked').val();

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
                        window.location.href = "/InvoiceTransaction/TrxPrintMergedInvoiceBuilding/Print?HeaderId=" + listHeaderId;
                        setTimeout(function () {
                            MergedInvoiceBuilding.Print();
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
                    window.location.href = "/InvoiceTransaction/TrxPrintMergedInvoiceBuilding/Print?HeaderId=" + listHeaderId;
                    setTimeout(function () {
                        MergedInvoiceBuilding.Print();
                    }, 5000);
                }
            }
        }
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

        $("#tblSummaryDataDeptHeadReprint tbody").on("click", "button.btEdit", function (e) {
            var table = $("#tblSummaryDataDeptHeadReprint").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
            }
        });

        $(window).resize(function () {
            $("#tblSummaryDataDeptHeadReprint").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val(),
        fsCustomerName = $("#tbSearchCustomerName").val(),
        fsInvNo = $("#tbInvNo").val()

        var params = {
            companyId: fsCompanyId,
            customerName: fsCustomerName,
            InvNo: fsInvNo
        };
        var tblSummaryDataDeptHeadReprint = $("#tblSummaryDataDeptHeadReprint").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/ManageMergedInvoiceOnlyBuilding/gridReprint",
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
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        if (Helper.DoesElementExistInArray(full.trxInvoiceHeaderID, Data.RowSelectedDeptHeadReprint)) {
                            $("#Row_" + full.trxInvoiceHeaderID).addClass("active");
                            strReturn += "<label id='" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                        }
                        else {
                            strReturn += "<label id='" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' class='checkboxes' /><span></span></label>";
                        }
                        return strReturn;
                    }
                },
                { data: "InvNo" },
                { data: "FullName" },
                { data: "PICAReprint" },
                { data: "PICARemarks" },
                { data: "Company" },
                { data: "CustomerName" },
                {
                    data: "InvSumADPP", className: "text-right", render: function (data, type, full) {
                        return Common.Format.CommaSeparation(data + full.Discount);
                    }
                },
                {
                    data: "Discount", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "InvTotalAPPN", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "InvTotalPenalty", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                { data: "Currency" },
                { data: "RequestCounter" },
                { data: "ApproveCounter" },
                { data: "RejectCounter" },
                { data: "PrintUsers" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryDataDeptHeadReprint.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").show();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelectedDeptHeadReprint.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelectedDeptHeadReprint.length; i++) {
                        item = Data.RowSelectedDeptHeadReprint[i];
                        $("#Row_" + item.trxInvoiceHeaderID).addClass("active");
                    }
                }
            },
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': [0, 2]
            }],
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
                    '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tblSummaryDataDeptHeadReprint .checkboxes" />' +
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
                            temp.trxInvoiceHeaderID = parseInt(item);
                            if (!Helper.DoesElementExistInArray(temp.trxInvoiceHeaderID, Data.RowSelectedDeptHeadReprint))
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
    Reset: function () {
        fsOperatorId = "";
        fsCompanyId = "";
        fsInvNo = "";
        $("#tbInvNo").val("");
        $("#slSearchOperator").val("").trigger("change");
        $("#slSearchCompany").val("").trigger("change");
    },
    Export: function () {
        window.location.href = "/InvoiceTransaction/TrxManageMergedRePrintInvoiceBuilding/Export?customerName=" + fsCustomerName + "&companyId=" + fsCompanyId + "&invNo=" + fsInvNo;
    }
}
var MergedInvoiceBuilding = {
    CancelMerge: function () {
        var tempId = [];
        $.each(Data.RowSelected, function (index, item) {
            tempId.push(item.trxInvoiceHeaderID);
        });

        var params = {
            listHeaderId: tempId
        }

        var l = Ladda.create(document.querySelector("#btCancelMergeInvoice"))
        $.ajax({
            url: "/api/ManageMergedInvoiceOnlyBuilding/CancelMerge",
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
                Common.Alert.Success("Merge Invoice has been cancelled!");
                //Table.Reset();
                Table.Search();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    },
    Print: function () {
        var tempHeaderId = [];
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(item.trxInvoiceHeaderID);
        });

        var params = {
            listHeaderId: tempHeaderId,
            PICAReprintID: $("#slPicaCategory").val(),
            ReprintRemark: $("#tbRemarks").val()
        }
        var l = Ladda.create(document.querySelector("#btPrintInvoice"));
        $.ajax({
            url: "/api/ManageMergedInvoiceOnlyBuilding/PrintInvoice",
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
            Helper.ClearRowSelected();
            Table.Search();
            $("#checkParent").attr("checked", false);
        });

    },
    ApprovalReprint: function (Status) {
        var tempHeaderId = [];
        $.each(Data.RowSelectedDeptHeadReprint, function (index, item) {
            tempHeaderId.push(item.trxInvoiceHeaderID);
        });

        var params = {
            listHeaderId: tempHeaderId,
            ApprovalStatus: Status
        }
        var l = Ladda.create(document.querySelector("#btPrintInvoice"));
        $.ajax({
            url: "/api/ManageMergedInvoiceOnlyBuilding/ApprovalInvoice",
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
            Helper.ClearRowSelected();
            TableDeptheadReprint.Search();
        });
    }
}

var Control = {
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchCompany").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchCompany").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
            }

            $("#slSearchCompany").select2({ placeholder: "Select Company", width: null });

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
                    TableDeptheadReprint.Search();
                else
                    Table.Search();
            }
        });
    }
}

var Helper = {
    RemoveObjectByIdFromArray: function (data, id) {
        var data = $.grep(data, function (e) {
            if (e.trxInvoiceHeaderID == id) {
                return false;
            } else {
                return true;
            }
        });

        return data;
    },
    DoesElementExistInArray: function (trxInvoiceHeaderID, arr) {
        var isExist = false;
        for (var i = 0 ; i < arr.length ; i++) {
            if (arr[i].trxInvoiceHeaderID == trxInvoiceHeaderID) {
                isExist = true;
                break;
            }
        }
        return isExist;
    },
    ValidatePrint: function () {
        var Result = {};

        var tempHeaderId = [];
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(item.trxInvoiceHeaderID);
        });

        var params = {
            listHeaderId: tempHeaderId
        }

        var l = Ladda.create(document.querySelector("#btPrintInvoice"));
        $.ajax({
            url: "/api/ManageMergedInvoiceOnlyBuilding/GetValidateResultPrint",
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
    ValidateCancelMerge: function () {
        var Result = {};

        var tempHeaderId = [];
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(item.trxInvoiceHeaderID);
        });

        var params = {
            listHeaderId: tempHeaderId
        }

        var l = Ladda.create(document.querySelector("#btPrintInvoice"));
        $.ajax({
            url: "/api/ManageMergedInvoiceOnlyBuilding/GetValidateResultCancelMerge",
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
    ClearRowSelected: function () {
        Data.RowSelected = [];
        Data.RowSelectedDeptHeadReprint = [];
    },
    GetListId: function () {
        //for CheckAll Pages
        var params = {
            companyId: fsCompanyId,
            customerName: fsCustomerName,
            InvNo: fsInvNo
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/ManageMergedInvoiceOnlyBuilding/GetListId",
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
            companyId: fsCompanyId,
            customerName: fsCustomerName,
            InvNo: fsInvNo
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/ManageMergedInvoiceOnlyBuilding/GetReprintListId",
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

var ApprovalStatus = {
    Approved: "A",
    Rejected: "R",
    Waiting: "W"
}