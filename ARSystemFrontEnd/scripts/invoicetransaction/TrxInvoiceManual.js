Data = {};
UploadedData = [];
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

jQuery(document).ready(function () {
    Form.Init();
    Table.Init();
    Table.Search();
    TableReject.Init();

    //panel Summary
    $("#formSearch").submit(function (e) {
        Table.Search();
        e.preventDefault();
    });

    //$("#btSearch").unbind().click(function () {
    //    Table.Search();
    //});
    $("#btSearch").unbind().click(function () {
        if ($("#tabBAPS").tabs('option', 'active') == 0)
            Table.Search();
        else
            TableReject.Search();

    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
    });

    //panel transaction Header
    $("#formMaster").submit(function (e) {
        if (Data.Mode == "Edit")
            TaxInvoice.Put();
        else if (Data.Mode == "Create")
            TaxInvoice.Post();
        e.preventDefault();
    });

    $("#btCreate").unbind().click(function () {
        Form.Create();
    });

    $(".btCancel").unbind().click(function () {
        Form.Cancel();
        Table.Reset();
    });

    $("#btDownload").on("click", function () {
        Form.DownloadTemplate();
    });

    $("#btUpload").on("click", function () {
        Form.UploadExcel();
    });

    $("#btBack").unbind().click(function () {
        $("#pnlSummary").fadeIn();
        $("#pnlDetailResult").hide();
    });

    $("#btYes").unbind().click(function () {
        Form.ClearData();
    });

    $(".fileinput").fileinput();

    $("#btClearData").unbind().click(function () {
        var oTable = $('#tblSummaryData').DataTable();

        if (Data.RowSelected.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            $('#mdlConfirm').modal('show');
            //if (confirm("Are you sure?"))
            //    Form.ClearData();
        }

    });
    $("#btSubmitTaxInvoices").unbind().on("click", function (e) {
        if ($("#formUploadedTaxInvoice").parsley().validate())
            TaxInvoice.Post();
        e.preventDefault();
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

    $('#tabBAPS').unbind().on('tabsactivate', function (event, ui) {
        var newIndex = ui.newTab.index();
        if (newIndex == 0) {
            Data.RowSelected = [];
            Table.Search();
            if (!$("#hdAllowDelete").val()) {
                $("#btClearData").hide();
            }
            else
                $("#btClearData").show();
        }
        else {
            TableReject.Search();
            $('#btClearData').hide();

        }
    });
});

var Form = {
    Init: function () {
        $("#pnlFormMaster").hide();
        $("#formMaster").parsley();
        $("body").delegate(".date-picker", "focusin", function () {
            $(this).datepicker({
                format: "dd-M-yyyy"
            });
        });
        Data.RowSelected = [];
        if (!$("#hdAllowAdd").val()) {
            $("#btCreate").hide();
        }
        if (!$("#hdAllowDelete").val()) {
            $("#btClearData").hide();
        }
        Table.Reset();
        $('#tabBAPS').tabs();
    },
    Create: function () {
        Data.Mode = "Create";
        window.scrollTo(0, 0);
        $("#divUploadField").show();
        $("#divUploadButtons").show();
        $("#divInvoiceNumber").hide();
        $("#divTaxInvoiceNumber").hide();
        $("#divTaxInvoiceDate").hide();
        $("#divFormButtons").hide();

        $("#pnlSummary").hide();
        $("#pnlFormMaster").fadeIn();
        $(".panelFormMaster").show();
        $("#panelFormMasterTitle").text("Upload Invoice Manual");
        $("#formMaster").parsley().reset()

        $(".fileinput").fileinput("clear");
    },
    Edit: function () {
        Data.Mode = "Edit";
        $("#pnlSummary").hide();
        $("#pnlFormMaster").fadeIn();
        $(".panelFormMaster").show();
        $("#panelFormMasterTitle").text("Edit Tax Invoice");
        $("#formMaster").parsley().reset();

        $("#divUploadField").hide();
        $("#divUploadButtons").hide();
        $("#divInvoiceNumber").show();
        $("#divTaxInvoiceNumber").show();
        $("#divTaxInvoiceDate").show();
        $("#divFormButtons").show();

        $("#tbInvoiceNumber").val(Data.Selected.InvNo);
        $("#tbTaxInvoiceNumber").val(Data.Selected.TaxInvoiceNo);
        $("#tbTaxInvoiceDate").val(Data.Selected.TaxInvoiceDate);
    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlFormMaster").hide();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlFormMaster").hide();

        Table.Search();
        $(".panelSearchResult").fadeIn();
    },
    ClearRowSelected: function () {
        Data.RowSelected = [];

    },
    DownloadTemplate: function () {
        window.location.href = "/InvoiceTransaction/TrxInvoiceManual/DownloadExcel";
    },
    UploadExcel: function () {
        var formData = new FormData(); //FormData object  
        var fileInput = document.getElementById("fuInvoiceManual");
        if (fileInput.files[0] != undefined && fileInput.files[0] != null) {
            var fileName = fileInput.files[0].name;
            var extension = fileName.split('.').pop().toUpperCase();
            if (extension != "XLS" && extension != "XLSX") {
                Common.Alert.Warning("Please upload an Excel File.");
            }
            else {
                for (i = 0; i < fileInput.files.length; i++) {
                    formData.append(fileInput.files[i].name, fileInput.files[i]);
                }

                var l = Ladda.create(document.querySelector("#btUpload"));
                $.ajax({
                    url: '/InvoiceTransaction/TrxUploadInvoiceManual/ImportExcel',
                    type: 'POST',
                    data: formData,
                    async: false,
                    beforeSend: function (xhr) {
                        l.start();
                    },
                    cache: false,
                    contentType: false,
                    processData: false
                }).done(function (data, textStatus, jqXHR) {
                    //var a = data.split("|")[0];
                    if (Common.CheckError.List(data)) {
                        if (data.ErrorMessage == "" || data.ErrorMessage == null) {
                            $('#mdlUploadedTaxInvoice').modal('hide');
                            Common.Alert.Success("Upload Invoice Manual Success")

                        }
                        else {
                            $(".fileinput").fileinput("clear");
                            Common.Alert.Warning(data.ErrorMessage);

                        }

                    }
                    Table.Reset();
                    Form.ClearRowSelected();
                    Form.Done();
                    l.stop()
                    //if (data == "S") {


                    //}
                    //else if (data == "Exception") {
                    //    $(".fileinput").fileinput("clear");
                    //    Common.Alert.Warning("The uploaded file is invalid. Please download the latest template by clicking the Download Template button.");
                    //}
                    //else {
                    //    $(".fileinput").fileinput("clear");
                    //    Common.Alert.Warning("The uploaded file is invalid, field " + data + " is Mandatory");
                    //}
                    //l.stop();
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    Common.Alert.Error(errorThrown)
                    l.stop()
                });
            }
        } else {
            Common.Alert.Warning("Please upload an Excel File.");
        }
    },
    ClearData: function () {
        Data.CheckedRow = [];

        var temp = new Object();
        $.each(Data.RowSelected, function (index, item) {
            temp = new Object();
            temp.trxInvoiceManualTempID = item;
            Data.CheckedRow.push(temp);
        });

        var l = Ladda.create(document.querySelector("#btClearData"));
        var params = {
            ListTrxInvoiceManual: Data.CheckedRow
        }

        $.ajax({
            url: "/api/TrxInvoiceManual/ClearData",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (data == true) {
                $('#mdlConfirm').modal('hide');
                Common.Alert.Success("Clear data Success")
                Form.ClearRowSelected();
                Form.Done();
            }
            l.stop();

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })

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

        $("#tblSummaryData tbody").on("click", "button.btEdit", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                Form.Edit();
            }
        });

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var params = {
            strSONumber: $("#tbSearchSONumber").val(),
            strStartPeriod: $("#tbStartPeriod").val(),
            strEndPeriod: $("#tbEndPeriod").val()
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/TrxInvoiceManual/grid",
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


                        if (Helper.IsElementExistsInArray(parseInt(full.trxInvoiceManualTempID), Data.RowSelected)) {
                            $("#Row" + full.trxInvoiceManualTempID).addClass("active");
                            strReturn += "<label id='" + full.trxInvoiceManualTempID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxInvoiceManualTempID + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                        }
                        else {
                            strReturn += "<label id='" + full.trxInvoiceManualTempID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.trxInvoiceManualTempID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                        }

                        return strReturn;
                    }
                },
                { data: "SONumber" },
                { data: "SiteIDOpr" },
                { data: "SiteNameOpr" },
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
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "OMPrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvoiceAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "StipSiro", className: "text-right" },

                { data: "CompanyID" },
                { data: "PriceCurrency" },

                 {
                     data: "LossPNN", mRender: function (data, type, full) {
                         return full.LossPNN ? "Yes" : "No";
                     }
                 },
                { data: "AmountPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AmountLossPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "BapsNO" },
                { data: "BapsType" },
                { data: "BapsPeriod" },
                { data: "OperatorName" },
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
                { data: "InvoiceTerm" },
                { data: "MLANumber" }

            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }

                l.stop();
                App.unblockUI('.panelSearchResult');
                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        $("#Row" + item).addClass("active");
                    }
                }
            },
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (aData.Status == 1) {
                    $('td', nRow).css('background-color', '#FF9999');

                }
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            //"order": [[0, "asc"]]
            "order": [],
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
        $("#tbSearchSONumber").val("");
        $("#tbStartPeriod").val("");
        $("#tbEndPeriod").val("");
    },
    Export: function () {
        var soNumber = $("#tbSearchSONumber").val();
        var dateFrom = $("#tbStartPeriod").val();
        var dateTo = $("#tbEndPeriod").val();

        window.location.href = "/InvoiceTransaction/TrxUploadInvoiceManual/Export?SONumber=" + soNumber + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo;
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
            ListId: Data.RowSelected
        }
        var l = Ladda.create(document.querySelector("#btAddSite"));
        $.ajax({
            url: "/api/CreateInvoiceTower/GetValidateResult",
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
            SONumber: $("#tbSearchSONumber").val(),
            dateFrom: $("#tbStartPeriod").val(),
            dateTo: $("#tbEndPeriod").val()

        };
        var AjaxData = [];
        $.ajax({
            url: "/api/TrxInvoiceManual/GetListId",
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


