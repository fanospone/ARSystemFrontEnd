Data = {};

//filter search 
var fsStartPeriod = "";
var fsEndPeriod = "";
var fsYearPosting = "";
var fsMonthPosting = "";
var fsWeekPosting = "";
var fsInvNo = "";
var fCompany = "";


jQuery(document).ready(function () {
    $(".ByInvoiceControl").hide(); //won't be used

    Data.RowSelected = [];
    Data.RowSelectedSite = [];
    Form.Init();

    //panel Summary
    $("#formSearch").submit(function (e) {
        if ($('input[name=rbViewBy]:checked').val() == 0)//View By Invoice
        {
            Table.Search();
            e.preventDefault();
        }
        else {
            TableSoNumber.Search();
            e.preventDefault();
        }
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
    });
    $("#btExcelToAX").unbind().click(function () {
        if (Data.RowSelected.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            Process.AddToSiteList();
            $("#pnlExcelToAXList").show();
            $("#pnlSummary").hide();
        }
    });
    $("#btDownload").unbind().click(function () {
        TableSite.DownloadExcelToAX();
        //update isAX 1 dan download excel
    });
    $("#btBack").unbind().click(function () {
        $("#pnlExcelToAXList").hide();
        $("#pnlSummary").show();
    });
    $('input[type=radio][name=rbViewBy]').change(function () {
        if ($('input[name=rbViewBy]:checked').val() == 0)//View By Invoice
        {
            //Table.Search();
            $(".BySoNumber").hide();
            $(".ByInvoice").show();
        }
        else { //View By SO Number
            //TableSoNumber.Search();
            $(".BySoNumber").show();
            $(".ByInvoice").hide();
        }
    });
    //$('#tblSummaryData').find('.group-checkable').change(function () {
    //    var set = jQuery(this).attr("data-set");
    //    var checked = jQuery(this).is(":checked");
    //    jQuery(set).each(function () {
    //        if (checked) {
    //            $(this).prop("checked", true);
    //            $(this).parents('tr').addClass('active');
    //            $(this).trigger("change");
    //        } else {
    //            $(this).prop("checked", false);
    //            $(this).parents('tr').removeClass("active");
    //            $(this).trigger("change");
    //        }
    //    });
    //});
    $('#tblSummaryData').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        var CategoryId = id.split('-')[0];
        var HeaderId = id.split('-')[1];
        var isCNInvoice = id.split('-')[2];
        var temp = new Object();
        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $('.Row_' + id).addClass("active");
            temp.CategoryId = parseInt(CategoryId);
            temp.HeaderId = parseInt(HeaderId);
            temp.isCNInvoice = parseInt(isCNInvoice);
            Data.RowSelected.push(temp);
        } else {
            $(this).parents('tr').removeClass("active");
            $('.Row_' + id).removeClass("active");
            Data.RowSelected = Helper.RemoveObjectByIdFromArray(Data.RowSelected, parseInt(HeaderId), parseInt(CategoryId), parseInt(isCNInvoice));
        }
    });
});

var Form = {
    Init: function () {
        Control.BindingYear();
        Control.BindingMonth();
        Control.BindingWeek();
        Table.Reset();
        // Initialize Datepicker
        var date = new Date();
        $("body").delegate(".date-picker", "focusin", function () {
            $(this).datepicker({
                autoclose: true,
                format: "dd-M-yyyy",     
            })

        });
        var date = new Date();
        Table.Init();
        TableSite.Init();
        TableSoNumber.Init();
        $("#pnlExcelToAXList").hide();
        $(".panelSearchResult").hide();
        $(".BySoNumber").hide();
        $('#tbStartPeriod').val(Common.Format.ConvertJSONDateTime(date));
        $('#tbEndPeriod').val(Common.Format.ConvertJSONDateTime(date));

    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlExcelToAXList").fadeOut();
        Table.Search();
        //Table.Reset();
    },
    ClearRowSelected: function () {
        Data.RowSelected = [];
        Data.RowSelectedSite = [];
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
        fsYearPosting = $("#slSearchYear").val() == null || $("#slSearchYear").val() == "" ? 0 : $("#slSearchYear").val();
        fsMonthPosting = $("#slSearchMonth").val() == null || $("#slSearchMonth").val() == "" ? 0 : $("#slSearchMonth").val();
        fsWeekPosting = $("#slSearchWeek").val() == null || $("#slSearchWeek").val() == "" ? 0 : $("#slSearchWeek").val();
        fsInvNo = $("#tbSearchInvNo").val();
        fCompany = $("#chCompanyName").bootstrapSwitch("state")

        var params = {
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            intYearPosting: fsYearPosting,
            intMonthPosting: fsMonthPosting,
            intWeekPosting: fsWeekPosting,
            invNo: fsInvNo,
            strCompanyCode: fCompany == 1 ? "TBG" : "PKP",
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/ReportInvoiceTower/grid",
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
                        Table.Export(params)
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
                        if (Helper.IsElementExistsInArray(full.trxInvoiceHeaderID, full.mstInvoiceCategoryId, full.isCNInvoice, Data.RowSelected)) {
                            //$("#Row_" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID).addClass("active");
                            strReturn += "<label id='" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "-" + full.isCNInvoice + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                        }
                        else {
                            strReturn += "<label id='" + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "-" + full.isCNInvoice + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.mstInvoiceCategoryId + "-" + full.trxInvoiceHeaderID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                        }
                        return strReturn;
                    }
                },
                { data: "InvNo" },
                { data: "InvSubject" },
                { data: "InvSumADPP", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvCompanyId" },
                { data: "CompanyIdAx" },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvTemp" },
                { data: "AccountType" },
                { data: "InvoiceCategory" },
                { data: "ElectricityCategory" },
                { data: "InvOperatorID" },
                { data: "Credit" },
                { data: "Currency" },
                { data: "Xrate" },
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
                { data: "PostingProfile" },
                { data: "OffSetAccount" },
                { data: "TaxGroup" },
                { data: "TaxItemGroup" },
                { data: "TaxInvoiceNo" },
                {
                    data: "FPJDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "PostingDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "OperatorRegion" },
                { data: "Address" },
                { data: "Status" }
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

                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        $("#Row_" + item.CategoryId + "-" + item.HeaderId + "-" + item.isCNInvoice).addClass("active");
                        $(".Row_" + item.CategoryId + "-" + item.HeaderId + "-" + item.isCNInvoice).addClass("active");
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
                    if (checked) {
                        var allIDs = Helper.GetListId();
                        var temp;
                        var tempArr = [];
                        $.each(allIDs, function (index, item) {
                            temp = new Object();
                            temp.HeaderId = parseInt(item.HeaderId);
                            temp.CategoryId = parseInt(item.CategoryId);
                            temp.isCNInvoice = parseInt(item.isCNInvoice);
                            if (!Helper.IsElementExistsInArray(parseInt(item.HeaderId), parseInt(item.CategoryId), parseInt(item.isCNInvoice), Data.RowSelected))
                                tempArr.push(temp);
                        });
                        Data.RowSelected = Data.RowSelected.concat(tempArr);
                    }
                    else {
                        $.each(Helper.GetListId(), function (index, item) {
                            Data.RowSelected = Helper.RemoveObjectByIdFromArray(Data.RowSelected, item.HeaderId, item.CategoryId, item.isCNInvoice);
                        });
                    }
                });
            }
        });

    },
    Reset: function () {
        fsYearPosting = 0;
        fsMonthPosting = 0;
        fsWeekPosting = 0;
        fsStartPeriod = "";
        fsEndPeriod = "";
        fsInvNo = "";

        $("#slSearchYear").val("").trigger('change');
        $("#slSearchMonth").val("").trigger('change');
        $("#slSearchWeek").val("").trigger('change');
        $("#tbStartPeriod").val("");
        $("#tbEndPeriod").val("");
        $("#tbSearchInvNo").val("");
    },
    Export: function (p) {

        window.location.href = "/InvoiceTransaction/TrxReportInvoiceTower/Export?intYearPosting=" + fsYearPosting + "&intMonthPosting=" + fsMonthPosting
           + "&intWeekPosting=" + fsWeekPosting + "&strStartPeriod=" + fsStartPeriod + "&strEndPeriod=" + fsEndPeriod + "&invNo=" + fsInvNo + "&strCompanyCode=" + p.strCompanyCode;
    }
}

var TableSoNumber = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        $(window).resize(function () {
            $("#tblSummaryDataSoNumber").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        fsInvNo = $("#tbSearchInvNo").val();
        fCompany = $("#chCompanyName").bootstrapSwitch("state")
        var params = {
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            invNo: fsInvNo,
            strCompanyCode: fCompany == 1 ? "TBG" : "PKP",

        };
        var tblSummaryData = $("#tblSummaryDataSoNumber").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/ReportInvoiceTower/gridBySONumber",
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
                        TableSoNumber.Export(params)
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [

                { data: "SONumber" },
                { data: "SiteIdOld" },
                { data: "SiteName" },
                { data: "InvNo" },
                { data: "InvSubject" },
                {
                    data: "StartDatePeriod", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndDatePeriod", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvSumADPP", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvCompanyId" },
                { data: "CompanyIdAx" },
                { data: "InvTemp" },
                { data: "Description" },
                { data: "InvoiceCategory" },
                { data: "InvOperatorID" },
                { data: "Credit" },
                { data: "Currency" },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    } //nanti ganti SLD dari table M_RFI
                },
                {
                    data: "BapsReceiveDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "BapsDone", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
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
                { data: "PostingProfile" },
                { data: "OffSetAccount" },
                { data: "TaxGroup" },
                { data: "TaxItemGroup" },
                { data: "TaxInvoiceNo" },
                {
                    data: "FPJDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "BapsConfirmDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "confirm_user_name" },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    } //posting date or invoice date? existing invoice date
                },
                { data: "posting_inv_user_name" },
                {
                    data: "InvFirstPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "print_inv_user_name" },
                {
                    data: "InvReceiptDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "receipt_user_name" },
                { data: "LeadTimeVerificator" }, //lead time baps confirm ?
                { data: "LeadTimeVerificator" },
                { data: "LeadTimeInputer" },
                { data: "LeadTimeFinishing" },//LeadTimeFinishing
                { data: "LeadTimeARData" } //LeadTimeARData
            ],
            "columnDefs": [{ "targets": [0, 1], "orderable": true }],
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
            },
            "order": [],
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
        });

    },
    Export: function (p) {
        var strStartPeriod = fsStartPeriod;
        var strEndPeriod = fsEndPeriod;

        window.location.href = "/InvoiceTransaction/TrxReportInvoiceTowerSONumber/Export?strStartPeriod=" + strStartPeriod + "&strEndPeriod=" + strEndPeriod + "&invNo=" + fsInvNo + "&strCompanyCode=" + p.strCompanyCode;
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
        var tempisCNInvoice = [];
        $.each(RowSelected, function (index, item) {
            tempHeaderId.push(item.HeaderId);
            tempCategoryId.push(item.CategoryId);
            tempisCNInvoice.push(item.isCNInvoice);
        });

        var params = {
            HeaderId: tempHeaderId,
            CategoryId: tempCategoryId,
            isCNInvoice: tempisCNInvoice
        }
        var l = Ladda.create(document.querySelector("#btExcelToAX"));
        $.ajax({
            url: "/api/ReportInvoiceTower/InvoicelistGrid",
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
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvTemp" },
                { data: "AccountType" },
                { data: "InvOperatorID" },
                { data: "InvSubject" },
                { data: "InvSumADPP" },
                { data: "Credit" },
                { data: "Currency" },
                { data: "Xrate" },
                { data: "DocNumber" },
                { data: "SONumber" },
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
                { data: "TaxItemGroup" },
                { data: "TaxInvoiceNo" },
                {
                    data: "FPJDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                }

            ],

        });


        $(window).resize(function () {
            $("#tblSiteData").DataTable().columns.adjust().draw();
        });
    },
    DownloadExcelToAX: function () {
        var tempHeaderId = [];
        var tempCategoryId = [];
        var tempisCNInvoice = [];
        $.each(Data.RowSelected, function (index, item) {
            tempHeaderId.push(item.HeaderId);
            tempCategoryId.push(item.CategoryId);
            tempisCNInvoice.push(item.isCNInvoice);
        });
        var HeaderId = tempHeaderId.join('|');
        var CategoryId = tempCategoryId.join('|');
        var isCNInvoice = tempisCNInvoice.join('|');

        window.location.href = "/InvoiceTransaction/TrxReportInvoiceTowerToAX/Export?HeaderId=" + HeaderId + "&CategoryId=" + CategoryId + "&isCNInvoice=" + isCNInvoice;
        Process.Download(tempHeaderId, tempCategoryId, tempisCNInvoice);

    }
}

var Control = {
    BindingYear: function () {
        $.ajax({
            url: "/api/ReportInvoiceTower/GetYear",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchYear").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchYear").append("<option value='" + item.ValueId + "'>" + item.ValueDesc + "</option>");
                })
            }

            $("#slSearchYear").select2({ placeholder: "Year", width: null }).on("change", function (e) {
                Control.BindingWeek();
            });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingMonth: function () {
        $.ajax({
            url: "/api/ReportInvoiceTower/GetMonth",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchMonth").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchMonth").append("<option value='" + item.ValueId + "'>" + item.ValueDesc + "</option>");
                })
            }

            $("#slSearchMonth").select2({ placeholder: "Month", width: null }).on("change", function (e) {
                Control.BindingWeek();
            });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingWeek: function () {
        var params = {
            intYearPosting: $("#slSearchYear").val() == null || $("#slSearchYear").val() == "" ? 0 : $("#slSearchYear").val(),
            intMonthPosting: $("#slSearchMonth").val() == null || $("#slSearchMonth").val() == "" ? 0 : $("#slSearchMonth").val()

        };
        $.ajax({
            url: "/api/ReportInvoiceTower/GetWeek",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchWeek").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchWeek").append("<option value='" + item.ValueId + "'>" + item.ValueDesc + "</option>");
                })
            }

            $("#slSearchWeek").select2({ placeholder: "Week", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    }
}

var Process = {
    AddToSiteList: function () {

        Data.isDifferent = false;
        var l = Ladda.create(document.querySelector("#btExcelToAX"))
        var oTable = $('#tblSummaryData').DataTable();
        var oTableSite = $('#tblSiteData').DataTable();
        TableSite.AddSite();
        $("#pnlExcelToAXList").fadeIn();
        /* oTable.rows('.active').every(function (rowIdx, tableLoop, rowLoop) {
             var HeaderId = this.data().trxInvoiceHeaderID;
             var CategoryId = this.data().mstInvoiceCategoryId;
             var checkBoxId = CategoryId + "-" + HeaderId;
             $("#Row_" + checkBoxId).removeClass('active');
             $("#" + checkBoxId).hide();
         });*/
        //insert Data.RowSelectedSite for rendering checkbox
        Data.RowSelectedSite = Data.RowSelected;

    },
    Download: function (tempHeaderId, tempCategoryId, tempisCNInvoice) {

        var params = {
            HeaderId: tempHeaderId,
            CategoryId: tempCategoryId,
            isCNInvoice: tempisCNInvoice
        }
        var l = Ladda.create(document.querySelector("#btDownload"));
        $.ajax({
            url: "/api/ReportInvoiceTower/DownloadExcelToAXReportInvoice",
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
            Form.ClearRowSelected();
            Form.Done();
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
    IsElementExistsInArray: function (HeaderId, CategoryId, isCNInvoice, arr) {
        var isExist = false;
        for (var i = 0 ; i < arr.length ; i++) {
            if (arr[i].HeaderId == HeaderId && arr[i].CategoryId == CategoryId && arr[i].isCNInvoice == isCNInvoice) {
                isExist = true;
                break;
            }
        }
        return isExist;
    },
    RemoveObjectByIdFromArray: function (data, id, CategoryId, isCNInvoice) {
        var data = $.grep(data, function (e) {
            if (e.HeaderId == id) {
                if (e.CategoryId == CategoryId)
                    if (e.isCNInvoice == isCNInvoice)
                        return false;
                    else
                        return true;
                else
                    return true;
            } else {
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
    GetListId: function () {
        //for CheckAll Pages
        var params = {
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            intYearPosting: fsYearPosting,
            intMonthPosting: fsMonthPosting,
            intWeekPosting: fsWeekPosting,
            invNo: fsInvNo
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/ReportInvoiceTower/GetListIdByInvoice",
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