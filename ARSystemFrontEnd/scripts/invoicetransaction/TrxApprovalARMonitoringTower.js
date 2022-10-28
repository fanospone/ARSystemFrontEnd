
Data = {};

//filter search 
var fsStartPeriod = "";
var fsEndPeriod = "";
var fsCompanyId = "";
var fsStatusGenerate = "";
var fsInvNo = "";

jQuery(document).ready(function () {
    Data.RowSelected = [];
    Data.RowSelectedCollection = [];
    Form.Init();

    PKPPurpose.Set.UserCompanyCode();
    if (PKPPurpose.Temp.UserCompanyCode == Constants.CompanyCode.PKP) {
        PKPPurpose.Filter.PKPOnly();
    }
    Table.Search();

    //panel Summary
    $("#formSearch").submit(function (e) {
        if ($('input[name=rbViewBy]:checked').val() == 0)//View By Invoice
        {
            Table.Search();
            e.preventDefault();
        }
        else {
            TableCollection.Search();
            e.preventDefault();
        }
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
        if (PKPPurpose.Temp.UserCompanyCode == Constants.CompanyCode.PKP) {
            PKPPurpose.Filter.PKPOnly();
        }
    });

    $("#btGenerateToCollection").unbind().click(function () {
        if (Data.RowSelected.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            Process.GenerateToCollection();
        }
    });

    $("#btGenerateToAX").unbind().click(function () {
        if (Data.RowSelectedCollection.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            Process.GenerateToAX();
        }
    });

    $("#btGenerateToExcel").unbind().click(function () {
        if (Data.RowSelectedCollection.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            Process.GenerateToExcel();
        }
    });

    $('input[type=radio][name=rbViewBy]').change(function () {
        if ($('input[name=rbViewBy]:checked').val() == 0)//View By Invoice
        {
            Table.Search();
            $(".ByCollection").hide();
            $(".ByInvoice").show();
            Data.RowSelected = [];

        }
        else { //View By Collection
            TableCollection.Search();
            $(".ByCollection").show();
            $(".ByInvoice").hide();
            Data.RowSelectedCollection = [];
        }
        //Determine the reference CheckBox in Header row.
        var chkAll = $(".group-checkable");
        //By default set to Unchecked.
        chkAll.prop("checked", false);
    });

    $('#tblSummaryData').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        var CategoryId = id.split('-')[0];
        var HeaderId = id.split('-')[1];
        var BatchNumber = id.split('-')[2];
        var temp = new Object();
        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row_" + id).addClass("active");
            temp.CategoryId = parseInt(CategoryId);
            temp.HeaderId = parseInt(HeaderId);
            temp.BatchNumber = parseInt(BatchNumber);
            if(!Helper.IsElementExistsInArray(temp.HeaderId, temp.CategoryId, temp.BatchNumber, Data.RowSelected))
                Data.RowSelected.push(temp);
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row_" + id).removeClass("active");
            if (Helper.IsElementExistsInArray(parseInt(HeaderId), parseInt(CategoryId), parseInt(BatchNumber), Data.RowSelected))
                Data.RowSelected = Helper.RemoveObjectByIdFromArray(Data.RowSelected, parseInt(HeaderId), parseInt(CategoryId), parseInt(BatchNumber));
        }
        Helper.CalculateTotalAmountPaid();
    });

    $('#tblSummaryDataCollection').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        var CategoryId = id.split('-')[0];
        var HeaderId = id.split('-')[1];
        var BatchNumber = id.split('-')[2];
        var mstPaymentCodeId = id.split('-')[3];
        var temp = new Object();
        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row_" + id).addClass("active");
            temp.CategoryId = parseInt(CategoryId);
            temp.HeaderId = parseInt(HeaderId);
            temp.BatchNumber = parseInt(BatchNumber);
            temp.mstPaymentCodeId = parseInt(mstPaymentCodeId);

            if (!Helper.IsElementExistsInArrayCollection(temp.HeaderId, temp.CategoryId, temp.BatchNumber, temp.mstPaymentCodeId, Data.RowSelectedCollection))
                Data.RowSelectedCollection.push(temp);

            // Check all rows when a voucher in a batch is checked
            $(".Row_"+ id +" .checkboxes").each(function (e) {
                var checkBox = $(this);
                checkBox.prop("checked", true);
            });
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row_" + id).removeClass("active");
            if (Helper.IsElementExistsInArrayCollection(parseInt(HeaderId), parseInt(CategoryId), parseInt(BatchNumber), parseInt(mstPaymentCodeId), Data.RowSelectedCollection))
                Data.RowSelectedCollection = Helper.RemoveObjectByIdFromArrayCollection(Data.RowSelectedCollection, parseInt(HeaderId), parseInt(CategoryId), parseInt(BatchNumber), parseInt(mstPaymentCodeId));

            // Uncheck all rows when a voucher in a batch is unchecked
            //$(".Row_" + id + " .checkboxes").each(function (e) {
            //    var checkBox = $(this);
            //    checkBox.prop("checked", false);
            //});
        }
    });

    $("#btYesReOpen").unbind().click(function (e) {
        var isRemarkValid = $("#tbRemarks").parsley().validate();
        if(isRemarkValid)
            Process.ReOpen(Data.Selected.trxInvoiceHeaderID, Data.Selected.mstInvoiceCategoryId);
        e.preventDefault();
    });

    $("#btYesReOpenToPayment").unbind().click(function (e) {
        var isRemarkValid = $("#tbRemarks").parsley().validate();
        if (isRemarkValid)
            Process.ReOpenToPayment(Data.Selected.trxInvoiceHeaderID, Data.Selected.mstInvoiceCategoryId);
        e.preventDefault();
    });
});

var Form = {
    Init: function () {
        Control.BindingSelectCompany();
        $("#slSearchStatus").select2({ placeholder: "Select Status Generate", width: null });
        Table.Reset();

        Table.Init();
        TableCollection.Init();
        $(".panelSearchResult").hide();
        $(".ByCollection").hide();
        $("#formReopen").parsley();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        if ($('input[name=rbViewBy]:checked').val() == 0)//View By Invoice
        {
            Table.Search();
            e.preventDefault();
        }
        else {
            TableCollection.Search();
            e.preventDefault();
        }
        //Table.Reset();

    },
    ClearRowSelected: function () {
        Data.RowSelected = [];
        Data.RowSelectedCollection = [];
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
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsInvNo = $("#tbInvNo").val();

        var params = {
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            strCompanyId: fsCompanyId,
            invNo: fsInvNo
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/ApprovalARMonitoringTower/grid",
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
                        if (Helper.IsElementExistsInArray(parseInt(full.trxInvoiceHeaderID), parseInt(full.mstInvoiceCategoryId), parseInt(full.BatchNumber), Data.RowSelected)) {
                            $("#Row_" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "-" + full.BatchNumber).addClass("active");
                            strReturn += "<label id='" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "-" + full.BatchNumber + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                        }
                        else {
                            strReturn += "<label id='" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "-" + full.BatchNumber + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' class='checkboxes' /><span></span></label>";
                        }
                        return strReturn;
                    }
                },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<button type='button' title='ReOpen' class='btn blue btn-xs btReOpen'>Re Open</button>";

                        return strReturn;
                    }
                },
                { data: "InvNo" },
                { data: "InvTemp" },
                {
                    data: "PaymentDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvCompanyId" },
                { data: "InvOperatorID" },
                {
                    data: "PAM", className: "text-right", render: function (data, type, full) {
                        return Common.Format.CommaSeparation(data);
                    }
                }
            ],
            "columnDefs": [{ "targets": [0, 1], "orderable": false }],
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
                        $("#Row_" + item.CategoryId + "-" + item.HeaderId + "-" + item.BatchNumber).addClass("active");
                        $(".Row_" + item.CategoryId + "-" + item.HeaderId + "-" + item.BatchNumber).addClass("active");
                    }
                }
            },
            "order": [],
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": {
                "leftColumns": 3
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
                    var allIDs = Helper.GetListId(false);
                    if (checked) {
                        var tempArr = [];
                        var temp = new Object();
                        $.each(allIDs, function (index, item) {
                            temp = new Object();
                            temp.HeaderId = parseInt(item.HeaderId);
                            temp.CategoryId = parseInt(item.CategoryId);
                            temp.BatchNumber = parseInt(item.BatchNumber);
                            if (!Helper.IsElementExistsInArray(temp.HeaderId, temp.CategoryId, temp.BatchNumber, Data.RowSelected))
                                tempArr.push(temp);
                        });
                        Data.RowSelected = Data.RowSelected.concat(tempArr);
                    }
                    else {
                        $.each(allIDs, function (index, item) {
                            Data.RowSelected = Helper.RemoveObjectByIdFromArray(Data.RowSelected, parseInt(item.HeaderId), parseInt(item.CategoryId), parseInt(item.BatchNumber));
                        });
                    }
                    Helper.CalculateTotalAmountPaid();
                });
            }
        });

        $("#tblSummaryData tbody").on("click", "button.btReOpen", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var selectedRow = table.row($(this).parents('tr')).data();
            Data.Selected = {};
            Data.Selected.trxInvoiceHeaderID = selectedRow.trxInvoiceHeaderID;
            Data.Selected.mstInvoiceCategoryId = selectedRow.mstInvoiceCategoryId;
            Data.Selected.FilePath = selectedRow.FilePath;
            $("#tbRemarks").val("");
            $('#mdlReOpen').modal('show');
        });
    },
    Reset: function () {
        fsCompanyId = "";
        fsEndPeriod = "";
        fsInvNo = "";
        fsStartPeriod = "";
        fsStatusGenerate = "";
        $("#slSearchCompanyName").val("").trigger('change');
        $("#slSearchStatus").val("").trigger('change');
        $("#tbStartPeriod").val("");
        $("#tbEndPeriod").val("");
        $("#tbInvNo").val("");
    },
    Export: function () {
        var strCompanyId = fsCompanyId;
        var strStartPeriod = fsStartPeriod;
        var strEndPeriod = fsEndPeriod;
        window.location.href = "/InvoiceTransaction/TrxApprovalARMonitoringTowerInvoice/Export?strStartPeriod=" + strStartPeriod + "&strEndPeriod=" + strEndPeriod
                            + "&strCompanyId=" + strCompanyId + "&invNo=" + fsInvNo;
    }
}

var TableCollection = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        $(window).resize(function () {
            $("#tblSummaryDataCollection").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsInvNo = $("#tbInvNo").val();
        fsStatusGenerate = $("#slSearchStatus").val() == null ? "" : $("#slSearchStatus").val();
        var params = {
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            strCompanyId: fsCompanyId,
            strStatusGenerate: fsStatusGenerate,
            invNo: fsInvNo
        };
        var tblSummaryData = $("#tblSummaryDataCollection").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/ApprovalARMonitoringTower/gridByCollection",
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
                        TableCollection.Export()
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
                        if (Helper.IsElementExistsInArrayCollection(parseInt(full.trxInvoiceHeaderID), parseInt(full.mstInvoiceCategoryId), parseInt(full.BatchNumber), parseInt(full.mstPaymentCodeId), Data.RowSelectedCollection)) {
                            $("#Row_" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "-" + full.BatchNumber + "-" + full.mstPaymentCodeId).addClass("active");
                            strReturn += "<label id='" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "-" + full.BatchNumber + "-" + full.mstPaymentCodeId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "-" + full.BatchNumber + "-" + full.mstPaymentCodeId + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                        }
                        else {
                            strReturn += "<label id='" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "-" + full.BatchNumber + "-" + full.mstPaymentCodeId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "-" + full.BatchNumber + "-" + full.mstPaymentCodeId + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                        }
                        return strReturn;
                    }
                },
                { data: "CompanyIdAx" },
                { data: "CompanyId" },
                {
                    data: "TransDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "VoucherNumber" },
                { data: "AccountType" },
                { data: "AccountNum" },
                { data: "TransactionText" },
                {
                    data: "Debit", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "Credit", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                { data: "Notes" },
                { data: "Currency" },
                { data: "Xrate" },
                { data: "SONumber" },
                { data: "DocNumber" },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "DueDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvNo" },
                { data: "PostingProfile" },
                { data: "OffSetAccount" },
                { data: "TaxGroup" },
                { data: "TaxGroup" },
                { data: "TaxInvoiceNo" },
                {
                    data: "FPJDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "TaxGroup" },
                { data: "OffSetAccountType" },
                { data: "JournalId" },
                { data: "StatusGenerate" }
            ],
            "columnDefs": [{ "targets": [0, 1], "orderable": false }],
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
                
                if (Data.RowSelectedCollection.length > 0) {
                    var item;
                    for (var i = 0; i < Data.RowSelectedCollection.length; i++) {
                        item = Data.RowSelectedCollection[i];
                        $("#Row_" + item.CategoryId + "-" + item.HeaderId + "-" + item.BatchNumber + "-" + item.mstPaymentCodeId).addClass("active");
                        $(".Row_" + item.CategoryId + "-" + item.HeaderId + "-" + item.BatchNumber + "-" + item.mstPaymentCodeId).addClass("active");
                    }
                }
            },
            "order": [],
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": {
                "leftColumns": 3
            },
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                    '<input type="checkbox" name="SelectAllCollection" class="group-checkable" data-set="#tblSummaryDataCollection .checkboxes" />' +
                    '<span></span> ' +
                    '</label>';
                var th = $("th.select-all-collection").html(checkbox);
                var checked;
                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-collection").unbind().on("change", ".group-checkable", function (e) {
                    var set = $(".panelSearchResult .DTFC_LeftBodyWrapper .DTFC_Cloned .checkboxes");
                     checked = jQuery(this).is(":checked");
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
                    var allIDs = Helper.GetListId(true);
                    if (checked) {
                        var temp = new Object();
                        $.each(allIDs, function (index, item) {
                            temp = new Object();
                            temp.HeaderId = parseInt(item.HeaderId);
                            temp.CategoryId = parseInt(item.CategoryId);
                            temp.BatchNumber = parseInt(item.BatchNumber);
                            temp.mstPaymentCodeId = parseInt(item.mstPaymentCodeId);
                            if (!Helper.IsElementExistsInArrayCollection(temp.HeaderId, temp.CategoryId, temp.BatchNumber, temp.mstPaymentCodeId, Data.RowSelectedCollection))
                                Data.RowSelectedCollection.push(temp);
                        });
                    }
                    else {
                        $.each(allIDs, function (index, item) {
                            Data.RowSelectedCollection = Helper.RemoveObjectByIdFromArrayCollection(Data.RowSelectedCollection, parseInt(item.HeaderId), parseInt(item.CategoryId), parseInt(item.BatchNumber), parseInt(item.mstPaymentCodeId));
                        });
                    }
                });
            }
        });


    },
    Export: function () {
        var strCompanyId = fsCompanyId;
        var strStatusGenerate = fsStatusGenerate;
        var strStartPeriod = fsStartPeriod;
        var strEndPeriod = fsEndPeriod;
        var strInvNo = fsInvNo
        window.location.href = "/InvoiceTransaction/TrxApprovalARMonitoringTowerCollection/Export?strStartPeriod=" + strStartPeriod + "&strEndPeriod=" + strEndPeriod
                            + "&strCompanyId=" + strCompanyId + "&strStatusGenerate=" + strStatusGenerate + "&strInvNo=" + strInvNo;
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
    }
}

var Process = {
    GenerateToCollection: function () {
        var tempHeaderId = [];
        var tempCategoryId = [];
        var tempBatchNumber = [];
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(item.HeaderId);
            tempCategoryId.push(item.CategoryId);
            tempBatchNumber.push(item.BatchNumber);
        });

        var params = {
            ListHeaderId: tempHeaderId,
            ListCategoryId: tempCategoryId,
            ListBatchNumber: tempBatchNumber
        }
        var l = Ladda.create(document.querySelector("#btGenerateToCollection"));
        $.ajax({
            url: "/api/ApprovalARMonitoringTower/GenerateToCollection",
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
                Common.Alert.Success("Data Success Generate To Collection!")
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Form.ClearRowSelected();
            Table.Search();
        });
    },
    GenerateToAX: function () {
        var tempHeaderId = [];
        var tempCategoryId = [];
        var tempBatchNumber = [];
        var tempPaymentCodeId = [];
        $.each(Data.RowSelectedCollection, function (index, item) {
            tempHeaderId.push(item.HeaderId);
            tempCategoryId.push(item.CategoryId);
            tempBatchNumber.push(item.BatchNumber);
            tempPaymentCodeId.push(item.mstPaymentCodeId);
        });

        var params = {
            ListHeaderId: tempHeaderId,
            ListCategoryId: tempCategoryId,
            ListBatchNumber: tempBatchNumber,
            ListPaymentCodeId: tempPaymentCodeId
        }
        var l = Ladda.create(document.querySelector("#btGenerateToAX"));
        $.ajax({
            url: "/api/ApprovalARMonitoringTower/GenerateToAX",
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
                Common.Alert.Success("Data Success Generate To AX!")
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Form.ClearRowSelected();
            TableCollection.Search();
        });

    },
    ReOpen: function (HeaderId, CategoryId) {
        var l = Ladda.create(document.querySelector("#btYesReOpen"))
        var params = {
            HeaderId: HeaderId,
            CategoryId: CategoryId,
            strRemarks: $("#tbRemarks").val()
        }

        $.ajax({
            url: "/api/ApprovalARMonitoringTower/ReOpenInvoice",
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
                $('#mdlReOpen').modal('hide');
                Common.Alert.Success("Data Success Re Open!")
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Form.Done();
        })
    },
    GenerateToExcel: function () {
        var tempHeaderId = [];
        var tempCategoryId = [];
        var tempBatchNumber = [];
        var tempPaymentCodeId = [];
        $.each(Data.RowSelectedCollection, function (index, item) {
            tempHeaderId.push(item.HeaderId);
            tempCategoryId.push(item.CategoryId);
            tempBatchNumber.push(item.BatchNumber);
            tempPaymentCodeId.push(item.mstPaymentCodeId);
        });
        var HeaderId = tempHeaderId.join('|');
        var CategoryId = tempCategoryId.join('|');
        var BatchNumber = tempBatchNumber.join('|');
        var PaymentCodeId = tempPaymentCodeId.join('|');

        window.location.href = "/InvoiceTransaction/TrxApprovalARMonitoringTowerToExcel/Export?strHeaderId=" + HeaderId + "&strCategoryId=" + CategoryId
                            + "&strBatchNumber=" + BatchNumber + "&strPaymentCodeId=" + PaymentCodeId;
    },
    ReOpenToPayment: function (HeaderId, CategoryId) {
        var l = Ladda.create(document.querySelector("#btYesReOpenToPayment"))
        var params = {
            HeaderId: HeaderId,
            CategoryId: CategoryId,
            strRemarks: $("#tbRemarks").val()
        }

        $.ajax({
            url: "/api/ApprovalARMonitoringTower/ReOpenInvoiceToPayment",
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
                $('#mdlReOpen').modal('hide');
                Common.Alert.Success("Data Success Re Open!")
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Form.Done();
        })
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
    IsElementExistsInArrayCollection: function (HeaderId, CategoryId, BatchNumber, mstPaymentCodeId, arr) {
        var isExist = false;
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].HeaderId == HeaderId && arr[i].CategoryId == CategoryId && arr[i].BatchNumber == BatchNumber && arr[i].mstPaymentCodeId == mstPaymentCodeId) {
                isExist = true;
                break;
            }
        }
        return isExist;
    },
    IsElementExistsInArray: function (HeaderId, CategoryId, BatchNumber,  arr) {
        var isExist = false;
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].HeaderId == HeaderId && arr[i].CategoryId == CategoryId && arr[i].BatchNumber == BatchNumber ) {
                isExist = true;
                break;
            }
        }
        return isExist;
    },
    RemoveObjectByIdFromArrayCollection: function (data, id, CategoryId, BatchNumber, mstPaymentCodeId) {
        var data = $.grep(data, function (e) {
            if (e.HeaderId == id) {
                if (e.CategoryId == CategoryId) {
                    if (e.BatchNumber == BatchNumber)
                    {
                        if (e.mstPaymentCodeId == mstPaymentCodeId)
                            return false;
                        else
                            return true
                    }
                    else {
                        return true;
                    }
                }
                else {
                    return true;
                }
            }
            else {
                return true;
            }
        });

        return data;
    },
    RemoveObjectByIdFromArray: function (data, id, CategoryId, BatchNumber) {
        var data = $.grep(data, function (e) {
            if (e.HeaderId == id) {
                if (e.CategoryId == CategoryId) {
                    if (e.BatchNumber == BatchNumber) {
                            return false;
                    }
                    else {
                        return true;
                    }
                }
                else {
                    return true;
                }
            }
            else {
                return true;
            }
        });

        return data;
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
    CalculateTotalAmountPaid: function ()
    {
        //var total = 0.00;
        //$.each(Data.RowSelected, function (index, item) {
        //    console.log($("#amount_" + item.CategoryId + "-" + item.HeaderId + "-" + item.BatchNumber));
        //    total += parseFloat($("#amount_" + item.CategoryId + "-" + item.HeaderId + "-" + item.BatchNumber).val());
        //});
        $("#tbTotalPaidAmount").val(Common.Format.CommaSeparation(Helper.GetTotalAmount()));
    },
    GetListId: function (isCollection) {
        //for CheckAll Pages
        var params = {
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            strCompanyId: fsCompanyId,
            invNo: fsInvNo,
            isCollection: isCollection
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/ApprovalARMonitoringTower/GetListId",
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
    GetTotalAmount: function () {
        //for Get Total Amount when check data
        var tempHeaderId = [];
        var tempCategoryId = [];
        var tempBatchNumber = [];
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(item.HeaderId);
            tempCategoryId.push(item.CategoryId);
            tempBatchNumber.push(item.BatchNumber);
        });

        var params = {
            ListHeaderId: tempHeaderId,
            ListCategoryId: tempCategoryId,
            ListBatchNumber: tempBatchNumber
        }
        var TotalAmount = 0.00;
        $.ajax({
            url: "/api/ApprovalARMonitoringTower/GetTotalAmount",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            TotalAmount = data;
            console.log(data);
        })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
            });
        return TotalAmount;
    }

}

var Constants = {
    CompanyCode: {
        PKP: "PKP"
    }
}

var PKPPurpose = {
    Filter: {
        PKPOnly: function () {
            $('#slSearchCompanyName').val(Constants.CompanyCode.PKP).trigger('change');
        },
    },
    Set: {
        UserCompanyCode: function () {
            PKPPurpose.Temp.UserCompanyCode = $('#hdUserCompanyCode').val();
        }
    },
    Temp: {
        UserCompanyCode: ""
    }
}