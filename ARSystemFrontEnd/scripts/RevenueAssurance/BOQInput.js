Data = {};

//filter search 
var fsID = "";
var fsCurrentActivity = "";
var fsOperatorId = "";
var fsCompanyId = "";
var fsYear = "";
var fsArea = "";
var fsRegional = "";
var fsTotalTenant = "";
var fsTotalAmount = "";
var fsBOQNumber = "";
var fsIdSignatory = "";
var fsFilepath = "";
var fsFilename = "";
var fsPrint = "";
var fsBatch = "";
var fsSignatureType = "";
var fsCustomerType = "";
var fsSoNumber = "";
var fsSummary = "";
var fsMsg = "";

jQuery(document).ready(function () {

    $("#slSONumber").select2({
        tags: true,
        multiple: true,
        createTag: function (params) {
            return {
                id: params.term,
                text: params.term,
                newOption: true
            }
        },
        templateResult: function (data) {
            var $result = $("<span></span>");

            $result.text(data.text);

            if (data.newOption) {
                $result.append(" <em>(new)</em>");
            }

            return $result;
        }
    });

    $("#slBOQNumberProcess").select2({
        width: "250px",
        tags: true,
        multiple: true,
        createTag: function (params) {
            return {
                id: params.term,
                text: params.term,
                newOption: true
            }
        },
        templateResult: function (data) {
            var $result = $("<span></span>");

            $result.text(data.text);

            if (data.newOption) {
                $result.append(" <em>(new)</em>");
            }

            return $result;
        }
    });

    $("#slBOQNumberDone").select2({
        width: "250px",
        tags: true,
        multiple: true,
        createTag: function (params) {
            return {
                id: params.term,
                text: params.term,
                newOption: true
            }
        },
        templateResult: function (data) {
            var $result = $("<span></span>");

            $result.text(data.text);

            if (data.newOption) {
                $result.append(" <em>(new)</em>");
            }

            return $result;
        }
    });
    $('#txtTotalAmount').val('0');
    Form.Init();
    Table.Init();
    TableProcess.Init();
    TableProcessDetail.Init();
    TableDone.Init();
    TableDoneDetail.Init();

    $(".panelSearchResult").fadeIn();

    $('#tabBOQ').unbind().on('tabsactivate', function (event, ui) {
        var newIndex = ui.newTab.index();
        if (newIndex == 0) {
            Data.RowSelected = [];
            Data.RowSelectedSite = [];
            if (Data.RowSelected.length == 0) {
                $(".panelSiteData").fadeOut();
            } else {
                $(".panelSiteData").fadeIn();
            }
            $("#btAddSite").show();
            $("#btReturn").hide();

        }
        else if (newIndex == 1) {
            Data.RowSelectedProcess = [];
            Data.RowSelectedProcessDetail = [];
            TableProcess.Search();
            $(".panelSiteData").fadeOut();
            $("#btAddSite").hide();
            $("#btReturn").show();
        }
        else {
            $(".panelSiteData").fadeOut();
            $("#btAddSite").hide();
            $("#btReturn").hide();
        }
    });

    $("#slSearchArea").change(function () {
        fsArea = $("#slSearchArea").val();
        Control.BindingSelectRegional(fsArea);
    });

    $("#slSearchCompanyName").change(function () {
        Form.Cancel();
    });

    $("#btSearch").unbind().click(function () {

        var ValidationSearch = Helper.ValidationSearch();
        if (ValidationSearch != "") {
            Common.Alert.Warning(ValidationSearch);
        }
        else {
            Data.RowSelectedSite = [];
            Table.Search();
        }

    });

    $("#btSearchProcess").unbind().click(function () {
        TableProcess.Search();
    });

    $("#btSearchDone").unbind().click(function () {

        var ValidationSearch = Helper.ValidationSearchDone();
        if (ValidationSearch != "") {
            Common.Alert.Warning(ValidationSearch);
        }
        else {
            TableDone.Search();
        }
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
        var tblSummary = $('#tblSummaryData').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });
    });

    $("#btResetProcess").unbind().click(function () {
        TableProcess.Reset();
    });

    $("#btResetDone").unbind().click(function () {
        TableDone.Reset();
    });

    $("#btDismissApprove").unbind().click(function () {
        Table.Reset();
    });

    $("#btAddSite").unbind().click(function () {
        Process.AddToList();
    });

    $("#btAddBOQ").unbind().click(function () {
        if ($("#slSearchCompanyName").val() == "" | ($("#slSearchCompanyName").val() == "00")) {
            Common.Alert.Warning("Company must be choosed!");
        }
        else {
            Process.InsertBOQData();
        }
    });

    $("#btCancel").unbind().click(function () {
        Form.Cancel();
        Table.Search();
    });

    $("#btReturn").unbind().click(function () {
        $('#mdlBackToBOQInput').modal('toggle');
    });

    $("#btYesConfirmBack").unbind().click(function () {
        $('#mdlBackToBOQInput').modal('hide');
        if (Data.RowSelectedProcess.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            Form.BackToBOQInput();
        }
    });

    $('#tblSummaryData').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        var amt = $(this).parent().attr('name');
        var txt = $('#txtTotalAmount').val();
        txt = txt.replace(/,/g, "");
        var CurrentAmt = parseFloat(txt);

        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            Data.RowSelected.push(parseInt(id));
            CurrentAmt = CurrentAmt + parseFloat(amt);
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row" + id).removeClass("active");
            Helper.RemoveElementFromArray(Data.RowSelected, parseInt(id));
            CurrentAmt = CurrentAmt - parseFloat(amt);
        }

        $('#txtTotalAmount').val(Common.Format.CommaSeparation(CurrentAmt));

    });

    $('#tblProcess').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            Data.RowSelectedProcess.push(parseInt(id));
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row" + id).removeClass("active");
            Helper.RemoveElementFromArray(Data.RowSelectedProcess, parseInt(id));
        }
    });

    $('#tblProcess').find('.group-checkable').change(function () {
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
        if (checked)
            Data.RowSelectedProcess = Helper.GetListProcessId(1);
        else
            Data.RowSelectedProcess = [];
    });

    $("#tblProcess tbody").on("click", "a.btDetail", function (e) {
        e.preventDefault();
        var table = $("#tblProcess").DataTable();
        var data = table.row($(this).parents("tr")).data();
        if (data != null) {
            var tblProcessDetail = $('#tblProcessDetail').dataTable({
                "filter": false,
                "destroy": true,
                "data": []
            });
            Data.Selected = data;
            fsID = data.ID;
            fsBOQNumber = data.BOQNumber;
            TableProcess.BOQProcessDetail(fsID);
            $('#mdlDetail').modal('toggle');
            $('#lblBOQNumber').text("Detail Data :" + data.BOQNumber);
        }
    });


    $("#tblProcess tbody").on("click", "a.btSignatory", function (e) {
        e.preventDefault();
        var table = $("#tblProcess").DataTable();
        var data = table.row($(this).parents("tr")).data();
        Data.Selected = data;
        fsID = data.ID;
        fsBOQNumber = data.BOQNumber;
        fsPrint = data.PrintID;
        fsBatch = data.BatchID;
        $("#txtPrintID").val(data.PrintID);
        $("#slBatch").val(data.BatchID);
        if (data.Signatory != "Available") {
            $("#btApply").show();
            $("#btApplyAll").show();
            $("#btEdit").hide();
        } else {
            $("#btApply").hide();
            $("#btApplyAll").hide();
            $("#btEdit").show();
            TableDone.BOQDoneSignatory(fsID);
        }
        $('#mdlSignatory').modal('toggle');

        $('#lblBOQNumberSignatory').text("Signatory :" + data.BOQNumber);

    });

    $("#tblProcess tbody").on("click", "a.btDocument", function (e) {
        e.preventDefault();
        var table = $("#tblProcess").DataTable();
        var data = table.row($(this).parents("tr")).data();
        Data.Selected = data;
        fsID = data.ID;
        fsBOQNumber = data.BOQNumber;
        window.location.href = "/RevenueAssurance/ReportBOQ/Print?ID=" + fsID + " &BOQNumber=" + fsBOQNumber;
    });


    $("#tblProcess tbody").on("click", "a.btApprove", function (e) {
        e.preventDefault();
        $("#slStatus").val("").trigger('change');
        $("#txtRemarksApprove").val("").trigger('change');
        Data.RowSelectedProcess = [];
        var table = $("#tblProcess").DataTable();
        var data = table.row($(this).parents("tr")).data();
        if (data != null) {
            var tblProcessApproveReject = $('#tblProcessApproveReject').dataTable({
                "filter": false,
                "destroy": true,
                "data": []
            });
            Data.Selected = data;
            fsID = data.ID;
            fsCurrentActivity = data.mstRAActivityID;
            $('#mdlApprove').modal('toggle');
            $('#lblBOQNumberApprove').text("Detail Data :" + data.BOQNumber);
            $("#txtCompanyApprove").val(data.Company);
            $("#txtAreaApprove").val(data.Area);
            $("#txtTotalRenewalTenantApprove").val((data.TotalTenant));
            $("#txtTotalAmountApprove").val((Helper.ConvertToRupiah(data.TotalAmount)));
            Control.BindingSelectStatus(fsCurrentActivity);
        }
    });

    $("#tblDone tbody").on("click", "a.btDocument", function (e) {
        e.preventDefault();
        var table = $("#tblDone").DataTable();
        var data = table.row($(this).parents("tr")).data();
        Data.Selected = data;
        fsID = data.ID;
        fsBOQNumber = data.BOQNumber;
        window.location.href = "/RevenueAssurance/ReportBOQ/Print?ID=" + fsID + " &BOQNumber=" + fsBOQNumber;
    });

    $("#tblDone tbody").on("click", "a.btDocumentSigned", function (e) {
        $("#lblError").html("");
        e.preventDefault();
        var table = $("#tblDone").DataTable();
        var data = table.row($(this).parents("tr")).data();
        Data.Selected = data;
        fsID = data.ID;
        fsBOQNumber = data.BOQNumber;
        $('#mdlUpload').modal('toggle');
        $('#lblBOQNumberUpload').text("Detail Data :" + data.BOQNumber);

    });

    $("#tblDone tbody").on("click", "a.btDownloadDocSign", function (e) {
        e.preventDefault();
        var table = $("#tblDone").DataTable();
        var data = table.row($(this).parents("tr")).data();
        Data.Selected = data;
        fsFilepath = data.FilePath;
        var FileNames = fsFilepath.split('\\');
        fsFilename = FileNames[3].toString();
        fsContentType = data.ContentType

        window.location.href = "/RevenueAssurance/DownloadDocument?Filepath=" + fsFilepath + "&fileName=" + fsFilename + "&ContentType=" + fsContentType;
    });

    $("#tblDone tbody").on("click", "a.btDetailDone", function (e) {
        e.preventDefault();
        var table = $("#tblDone").DataTable();
        var data = table.row($(this).parents("tr")).data();
        if (data != null) {
            var tblDoneDetail = $('#tblDoneDetail').dataTable({
                "filter": false,
                "destroy": true,
                "data": []
            });
            Data.Selected = data;
            fsID = data.ID;
            TableDone.BOQDoneDetail(fsID);
            $('#mdlDetailDone').modal('toggle');
            $('#lblBOQNumberDetailDone').text("Detail Data :" + data.BOQNumber);
        }
    });

    $("#formApprove").submit(function (e) {
        e.preventDefault();
        Process.ApproveRejectBOQData();
        $('#mdlApprove').modal('hide');
    });

    $("#btUndoDelete").unbind().click(function () {
        TableProcess.BOQApproveReject(fsID);
        Process.SetDataBOQAmount(fsID);
    });

    $("#btUpload").unbind().click(function (e) {
        fileInput = document.getElementById("file")
        if (fileInput.files.length != 0) {
            TableDone.UploadBOQDoc(fsID);
            $('#mdlUpload').modal('hide');
            TableProcess.Search();
        }
        else {
            $("#lblError").html("File must be uploaded!");
        }
    });

    $("#btApply").unbind().click(function (e) {
        e.preventDefault();

        var ValidationApply = Helper.ValidationApply();
        if (ValidationApply != "") {
            Common.Alert.Warning(ValidationApply);
            $('#mdlSignatory').modal('show');
        }
        else {
            var RowSignatory = Process.SetSignatory(fsID);
            TableDone.ApplyBOQSignatory(RowSignatory);

            $('#mdlSignatory').modal('hide');
            TableProcess.Search();
        }
    });

    $("#btApplyAll").unbind().click(function (e) {
        e.preventDefault();
        var ValidationApply = Helper.ValidationApply();
        if (ValidationApply != "") {
            Common.Alert.Warning(ValidationApply);
        }
        else {
            var RowSignatory = Process.SetSignatory(null);
            TableDone.ApplyAllBOQSignatory(RowSignatory);

            $('#mdlSignatory').modal('hide');
            TableProcess.Search();
        }
    });

    $("#btEdit").unbind().click(function () {
        var ValidationApply = Helper.ValidationApply();
        if (ValidationApply != "") {
            Common.Alert.Warning(ValidationApply);
        }
        else {
            var RowSignatory = Process.SetSignatory(fsID);
            TableDone.EditBOQSignatoryOperator(RowSignatory);

            $('#mdlSignatory').modal('hide');
            TableProcess.Search();
        }
    });

    $("#file").change(function () {
        fileInput = document.getElementById("file")
        if (!Helper.FileValidation(fileInput)) {
            $("#lblError").html(fsMsg);
            $("#file").val("")
        }
    })
});

var Form = {

    Init: function () {
        $("#btReturn").hide();

        $("#slSearchYear").val("").trigger('change');
        $("#slSearchYear").select2({ placeholder: "Select Year", width: null });

        Control.BindingSelectCompany();
        Control.BindingSelectArea();
        Control.BindingSelectOperatorSign();
        Control.BindingSelectCompanySign();

        $('#tabBOQ').tabs();

        $(".panelSiteData").hide();

        $(".panelSiteDataDetail").hide();
        //untuk validasi checkbox
        Data.RowSelected = [];

        Data.RowSelectedSite = [];

        Data.RowSelectedProcess = [];

        Data.RowSelectedProcessDetail = [];

        Data.RowSelectedDone = [];
    },

    ClearRowSelected: function () {
        Data.RowSelected = [];
        Data.RowSelectedSite = [];
        Data.RowSelectedProcess = [];
    },

    Cancel: function () {
        $("#txtTotalRenewalTenant").val("").trigger('change');
        $("#txtTotalAmount").val("0").trigger('change');

        Data.RowSelected = [];
        $('input:checkbox').removeAttr('checked');
        $(".panelSiteData").hide();
    },

    BackToBOQInput: function () {
        var l = Ladda.create(document.querySelector("#btYesConfirmBack"))

        var params = {
            detailIDs: Data.RowSelectedProcess
        }

        $.ajax({
            url: "/api/BOQData/BacktoBOQInput",
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
                Common.Alert.Success("Data Success Back to BOQ Input")
            }
            l.stop();

        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        }).always(function (jqXHR, textStatus) {
            TableProcess.Search();
            Form.ClearRowSelected();
        })
    },

    DoneProcess: function () {
        $("#pnlSummary").fadeIn();
        $('#mdlBackToBOQInput').modal('toggle');
        TableRaw.Search();
    },
}

var Table = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblSummary = $('#tblSummaryData').dataTable({
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

        fsOperatorId = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsYear = $("#slSearchYear").val() == null ? "" : $("#slSearchYear").val();
        fsArea = $("#slSearchArea").val() == null ? "" : $("#slSearchArea").val();
        fsRegional = $("#slSearchRegional").val() == null ? "" : $("#slSearchRegional").val();
        fsSoNumber = $("#slSONumber").val() == null ? "" : $("#slSONumber").val();

        var params = {
            strOperator: fsOperatorId,
            strCompanyId: fsCompanyId,
            strYear: fsYear,
            strArea: fsArea,
            strRegional: fsRegional,
            strSoNumber: fsSoNumber,
        };
        var tblSummary = $("#tblSummaryData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/BOQData/grid",
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
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Helper.IsElementExistsInArray(parseInt(full.ID), Data.RowSelected)) {
                                if (Helper.IsElementExistsInArray(parseInt(full.ID), Data.RowSelectedSite)) {
                                    strReturn += "<label name='" + full.TotalAmount + "' style='display:none;' id='" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.ID + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                } else {
                                    strReturn += "<label name='" + full.TotalAmount + "' id='" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.ID + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                }
                            }
                            else {
                                strReturn += "<label name='" + full.TotalAmount + "' id='" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.ID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                },
                { data: "SoNumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "Operator" },
                { data: "SiteIDOperator" },
                { data: "SiteNameOperator" },
                { data: "CompanyInv" },
                { data: "CompanyAsset" },
                { data: "Area" },
                { data: "Region" },
                { data: "Province" },
                { data: "Residence" },
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
                { data: "Term" },
                { data: "InitPONumber" },
                { data: "MLANumber" },
                { data: "InvoiceType" },
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
                { data: "AmountBilled", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AmountDeduction", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "TotalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {

                if (Common.CheckError.List(tblSummary.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
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
                var th = $("th.select-all-raw").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-raw").unbind().on("change", ".group-checkable", function (e) {
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

        $("#slSearchCompanyName").val("").trigger('change');
        $("#slSearchYear").val("").trigger('change');
        $("#slSearchArea").val("").trigger('change');
        $("#slSearchRegional").val("").trigger('change');
        $("#slSONumber").val("").trigger('change');

        $("#slOperatorSign").val("").trigger('change');
        $("#slOperatorPICNameSign").val("").trigger('change');
        $("#slOperatorPositionSign").val("").trigger('change');
        $("#slCompanySign").val("").trigger('change');
        $("#slCompanyPICNameSign").val("").trigger('change');
        $("#slCompanyPositionSign").val("").trigger('change');

        $("#txtPrintID").val("");
        $("#slBatch").val("");

        $("#txtTotalRenewalTenant").val("");
        $("#txtTotalAmount").val("0");

        fsID = "";
        fsCurrentActivity = "";
        fsOperatorId = "";
        fsCompanyId = "";
        fsYear = "";
        fsArea = "";
        fsRegional = "";
        fsTotalTenant = "";
        fsTotalAmount = "";
        fsBOQNumber = "";
        fsIdSignatory = "";
        fsFilepath = "";
        fsFilename = "";
        fsPrint = "";
        fsBatch = "";
        fsSignatureType = "";
        fsCustomerType = "";
        fsSoNumber = "";

    },

    Export: function () {

        fsOperatorId = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsYear = $("#slSearchYear").val() == null ? "" : $("#slSearchYear").val();
        fsArea = $("#slSearchArea").val() == null ? "" : $("#slSearchArea").val();
        fsRegional = $("#slSearchRegional").val() == null ? "" : $("#slSearchRegional").val();
        fsSoNumber = $("#slSONumber").val() == null ? "" : $("#slSONumber").val();

        var strOperator = fsOperatorId;
        var strCompanyId = fsCompanyId;
        var strYear = fsYear;
        var strArea = fsArea;
        var strRegional = fsRegional;
        var strSONumber = fsSoNumber;

        window.location.href = "/RevenueAssurance/trxBOQInput/Export?strOperator=" + strOperator + "&strCompanyId=" + strCompanyId
            + "&strYear=" + strYear + "&strArea=" + strArea + "&strRegional=" + strRegional + "&strSONumber=" + strSONumber;
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

    AddSite: function (ajaxData) {
        //Get Data that in RowSelected
        var ajaxData = TableSite.GetSelectedSiteData(Data.RowSelected);

        var tblSiteData = $("#tblSiteData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "data": ajaxData,
            "filter": false,
            "destroy": true,

            "columns": [
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<button type='button' title='Delete' class='btn btn-xs red btDeleteSite' id='btDeleteSite" + full.ID + "'><i class='fa fa-trash'></i></button>";
                        return strReturn;
                    }
                },
                { data: "SoNumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "Operator" },
                { data: "SiteIDOperator" },
                { data: "SiteNameOperator" },
                { data: "CompanyInv" },
                { data: "CompanyAsset" },
                { data: "Area" },
                { data: "Region" },
                { data: "Province" },
                { data: "Residence" },
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
                { data: "Term" },
                { data: "InitPONumber" },
                { data: "MLANumber" },
                { data: "InvoiceType" },
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
                { data: "AmountBilled", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AmountDeduction", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "TotalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
            ],
            "buttons": [],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;
                //var colNumber = [23];


                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                        i : 0;
                };

                var numformat = $.fn.dataTable.render.number(',', '.', 2, '').display;

                fsTotalTenant = 0;
                fsTotalAmount = 0;
                var oTableSite = $('#tblSiteData').DataTable();
                var Percent = $('input[name=rbPercent]:checked').val();

                oTableSite.rows('').every(function (rowIdx, tableLoop, rowLoop) {
                    fsTotalTenant += 1;
                    fsTotalAmount += this.data().TotalAmount;
                });

                $(api.column(25).footer()).html(numformat(fsTotalAmount));

                $("#txtTotalRenewalTenant").val(fsTotalTenant);
                $("#txtTotalAmount").val(numformat(fsTotalAmount));
            },

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
            Helper.RemoveElementFromArray(Data.RowSelected, parseInt(id));
            Helper.RemoveElementFromArray(Data.RowSelectedSite, parseInt(id));
            //Process.Calculate();
            if (Data.RowSelected.length == 0)
                $(".panelSiteData").fadeOut();
        });

        $(window).resize(function () {
            $("#tblSiteData").DataTable().columns.adjust().draw();
        });
    },

    GetSelectedSiteData: function (listID) {
        var AjaxData = [];
        var params = {
            ListId: listID
        };
        var l = Ladda.create(document.querySelector("#btAddSite"));
        $.ajax({
            url: "/api/BOQData/SitelistGrid",
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

}

var TableProcess = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblProcess = $('#tblProcess').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblProcess").DataTable().columns.adjust().draw();
        });
    },

    Search: function () {

        fsCompanyId = $("#slSearchCompanyNameProcess").val() == null ? "" : $("#slSearchCompanyNameProcess").val();
        fsBOQNumber = $("#slBOQNumberProcess").val() == null ? "" : $("#slBOQNumberProcess").val();

        var params = {
            strCompanyId: fsCompanyId,
            listBOQNumber: fsBOQNumber,
        };


        var tblProcess = $("#tblProcess").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "filter": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/BOQData/gridProcess",
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
                        TableProcess.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Helper.IsElementExistsInArray(parseInt(full.ID), Data.RowSelectedProcess)) {

                                strReturn += "<label id='" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.ID + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";

                            }
                            else {
                                strReturn += "<label id='" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.ID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                },
                { data: "BOQNumber" },
                { data: "Area" },
                { data: "Company" },
                {
                    data: "TotalTenant", mRender: function (data, type, full) {
                        return "<a href='' class='btDetail'>" + data + "</a>";
                    }
                },
                { data: "TotalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "CreatedBy" },
                {
                    data: "Signatory", mRender: function (data, type, full) {
                        if ($("#hdAllowProcess").val()) {
                            return "<a href='' class='btSignatory'>" + full.Signatory + "</a>";
                        }
                        else {
                            return full.Signatory;
                        }
                    }
                },
                {
                    data: "Document", mRender: function (data, type, full) {
                        return "<center><a href='' class='btDocument'><i class='fa fa-download'></i></a></center>";
                    }
                },
                {
                    data: "Status", mRender: function (data, type, full) {
                        if ($("#hdAllowApproval").val() && full.Signatory == "Available") {
                            return "<a href='' class='btApprove'>" + full.Status + "</a>";
                        }
                        else {
                            return full.Status;
                        }

                    }
                },

            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "scrollY": 300,
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "destroy": true

        });
    },

    BOQProcessDetail: function (fsID) {
        var params = {
            ID: fsID
        };

        var tblProcessDetail = $("#tblProcessDetail").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "async": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/BOQData/gridProcessDetail",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },

            "columns": [
                { data: "SoNumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "Company" },
                { data: "CompanyInvoice" },
                { data: "Area" },
                { data: "Regional" },
                { data: "Province" },
                { data: "Residence" },
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
                { data: "Term" },
                { data: "InitPONumber" },
                { data: "MLANumber" },
                { data: "InvoiceType" },
                {
                    data: "StartInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvoiceAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ServiceAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AmountTagih", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
            ],
            "buttons": [],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "scrollY": 300,
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            //"scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 1 /* Set the 2 most left columns as fixed columns */
            },
            "destroy": true
        });

        $(window).resize(function () {
            $("#tblProcessDetail").DataTable().columns.adjust().draw();
        });
    },

    BOQApproveReject: function () {
        //Get Data that in RowSelected
        var params = {
            ID: fsID
        };

        var tblProcessApproveReject = $("#tblProcessApproveReject").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/BOQData/gridProcessDetail",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },

            "columns": [
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<button type='button' title='Delete' class='btn btn-xs red btDeleteApprove' id='btDeleteApprove" + full.ID + "'><i class='fa fa-trash'></i></button>";
                        return strReturn;
                    }
                },
                { data: "SoNumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "Company" },
                { data: "CompanyInvoice" },
                { data: "Area" },
                { data: "Regional" },
                { data: "Province" },
                { data: "Residence" },
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
                { data: "Term" },
                { data: "InitPONumber" },
                { data: "MLANumber" },
                { data: "InvoiceType" },
                {
                    data: "StartInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvoiceAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ServiceAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AmountTagih", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
            ],
            "buttons": [],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "destroy": true,
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;
                var colNumber = [22];


                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                        i : 0;
                };

                var numformat = $.fn.dataTable.render.number(',', '.', 2, '').display;

                for (i = 0; i < colNumber.length; i++) {

                    var colNo = colNumber[i];

                    AmountTagih = api
                            .column(colNo, { page: 'current' })
                            .data()
                            .reduce(function (a, b) {
                                return intVal(a) + intVal(b);
                            }, 0);
                    $(api.column(colNo).footer()).html(numformat(AmountTagih));

                    $("#txtTotalAmountApprove").val(numformat(AmountTagih));
                }

            },

        });

        $("#tblProcessApproveReject tbody").unbind().on("click", "button.btDeleteApprove", function (e) {
            var myTable = $('#tblProcessApproveReject').DataTable();
            myTable.row($(this).parents('tr')).remove().draw();

            var rowCount = $('#tblProcessApproveReject tr').length;
            fsTotalTenant = rowCount - 1;
            fsTotalAmount = AmountTagih;

            if (fsTotalAmount == 0) {
                fsTotalTenant = 0
            }

            $("#txtTotalRenewalTenantApprove").val(fsTotalTenant);
            $("#txtTotalAmountApprove").val(Helper.ConvertToRupiah(fsTotalAmount));

        });

        $(window).resize(function () {
            $("#tblProcessApproveReject").DataTable().columns.adjust().draw();
        });
    },

    Reset: function () {
        $("#slSearchCompanyNameProcess").val("").trigger('change');
        $("#slBOQNumberProcess").val("").trigger('change');

        fsCompanyId = "";
        fsBOQNumber = "";

        var tblDone = $('#tblProcess').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });
    },

    Export: function () {

        fsCompanyId = $("#slSearchCompanyNameProcess").val() == null ? "" : $("#slSearchCompanyNameProcess").val();
        fsBOQNumber = $("#slBOQNumberProcess").val() == null ? "" : $("#slBOQNumberProcess").val();

        var strCompanyId = fsCompanyId;
        var strBOQNumber = fsBOQNumber;

        window.location.href = "/RevenueAssurance/trxBOQProcess/Export?strCompanyId=" + strCompanyId + "&strBOQNumber=" + strBOQNumber;
    }
}

var TableProcessDetail = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblProcessDetail = $('#tblProcessDetail').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblProcessDetail").DataTable().columns.adjust().draw();
        });
    },
}

var TableDoneDetail = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblDoneDetail = $('#tblDoneDetail').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblDoneDetail").DataTable().columns.adjust().draw();
        });
    },
}

var TableDone = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblDone = $('#tblDone').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblDone").DataTable().columns.adjust().draw();
        });

    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearchDone"))
        l.start();

        fsCompanyId = $("#slSearchCompanyNameDone").val() == null ? "" : $("#slSearchCompanyNameDone").val();
        fsBOQNumber = $("#slBOQNumberDone").val() == null ? "" : $("#slBOQNumberDone").val();

        var params = {
            strCompanyId: fsCompanyId,
            listBOQNumber: fsBOQNumber,
        };


        var tblDone = $("#tblDone").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/BOQData/gridDone",
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
                        TableDone.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                { data: "BOQNumber" },
                { data: "Area" },
                { data: "Company" },
                {
                    data: "TotalTenant", mRender: function (data, type, full) {
                        return "<a href='' class='btDetailDone'>" + data + "</a>";
                    }
                },
                { data: "TotalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                {
                    data: "Document", mRender: function (data, type, full) {
                        return "<center><a href='' class='btDocument'><i class='fa fa-download'></i></a></center>";
                    }
                },
                {
                    data: "DocumentSigned", mRender: function (data, type, full) {
                        if ($("#hdAllowProcess").val() && full.DocumentSigned != "Available") {
                            return "<a href='' class='btDocumentSigned'>" + full.DocumentSigned + "</a>";
                        }
                        else {
                            if (full.DocumentSigned == "Available") {
                                return "<a href='' class='btDownloadDocSign'>" + full.DocumentSigned + "</a>";
                            } else {
                                return full.DocumentSigned
                            }
                        }
                    }

                },
                { data: "CreatedBy" },
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "ApprovedBy" },
                 {
                     data: "ApprovedDate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {

                if (Common.CheckError.List(tblDone.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');
            },
            "order": [1, "asc"],
            "scrollY": 300,
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },

        });
    },

    BOQDoneDetail: function (fsID) {
        //Get Data that in RowSelected
        var params = {
            ID: fsID
        };

        var tblDoneDetail = $("#tblDoneDetail").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/BOQData/gridDoneDetail",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            "columns": [
                { data: "SoNumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "Company" },
                { data: "CompanyInvoice" },
                { data: "Area" },
                { data: "Regional" },
                { data: "Province" },
                { data: "Residence" },
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
                { data: "Term" },
                { data: "InitPONumber" },
                { data: "MLANumber" },
                { data: "InvoiceType" },
                {
                    data: "StartInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvoiceAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ServiceAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AmountTagih", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
            ],
            "buttons": [],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "scrollY": 300,
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "destroy": true
        });

        $(window).resize(function () {
            $("#tblDoneDetail").DataTable().columns.adjust().draw();
        });
    },

    BOQDoneSignatory: function (fsID) {
        var params = {
            ID: fsID,
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/BOQData/GetSignatory",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $("#slOperatorSign").val(data[0].CompanyName);
                $("#slOperatorPICNameSign").val(data[0].PICName);
                $("#slOperatorPositionSign").val(data[0].Position);
                $("#slCompanySign").val(data[1].CompanyName);
                $("#slCompanyPICNameSign").val(data[1].PICName);
                $("#slCompanyPositionSign").val(data[1].Position);
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    ApplyBOQSignatory: function (ListSignatory) {
        var l = Ladda.create(document.querySelector("#btApply"))

        fsPrint = $("#txtPrintID").val() == null ? "" : $("#txtPrintID").val();
        fsBatch = $("#slBatch").val() == null ? "" : $("#slBatch").val();

        var params = {
            mstRABoqID: fsID,
            PrintID: fsPrint,
            BatchID: fsBatch,
            ListSignatory: ListSignatory,
        };

        $.ajax({
            url: "/api/BOQData/ApplySignatory",
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
    },

    ApplyAllBOQSignatory: function (ListSignatory) {
        var l = Ladda.create(document.querySelector("#btApplyAll"))

        fsPrint = $("#txtPrintID").val() == null ? "" : $("#txtPrintID").val();
        fsBatch = $("#slBatch").val() == null ? "" : $("#slBatch").val();

        var params = {
            PrintID: fsPrint,
            BatchID: fsBatch,
            ListSignatory: ListSignatory,
        };

        $.ajax({
            url: "/api/BOQData/ApplyAllSignatory",
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
    },

    EditBOQSignatoryOperator: function (ListSignatory) {
        var l = Ladda.create(document.querySelector("#btEdit"))

        fsPrint = $("#txtPrintID").val() == null ? "" : $("#txtPrintID").val();
        fsBatch = $("#slBatch").val() == null ? "" : $("#slBatch").val();

        var params = {
            mstRABoqID: fsID,
            PrintID: fsPrint,
            BatchID: fsBatch,
            ListSignatory: ListSignatory,
        };

        $.ajax({
            url: "/api/BOQData/EditSignatory",
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
            }
            l.stop();

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            $(".panelSiteData").fadeOut();
            Form.Init();
            Form.ClearRowSelected();
        })
    },

    Reset: function () {
        $("#slSearchCompanyNameDone").val("").trigger('change');
        $("#slBOQNumberDone").val("").trigger('change');

        fsCompanyId = "";
        fsBOQNumber = "";

        var tblDone = $('#tblDone').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

    },

    Export: function () {

        fsCompanyId = $("#slSearchCompanyNameDone").val() == null ? "" : $("#slSearchCompanyNameDone").val();
        fsBOQNumber = $("#slBOQNumberDone").val() == null ? "" : $("#slBOQNumberDone").val();

        var strCompanyId = fsCompanyId;
        var strBOQNumber = fsBOQNumber;

        window.location.href = "/RevenueAssurance/trxBOQDone/Export?&strCompanyId=" + strCompanyId + "&strBOQNumber=" + strBOQNumber;
    },

    UploadBOQDoc: function () {
        var formData = new FormData();
        formData.append("ID", fsID);

        var fileInput = document.getElementById("file");
        if (fileInput.files != undefined && fileInput.files != null) {

            fsFileName = fileInput.files[0].name;
            formData.append("FileName", fsFileName);

            fsFile = fileInput.files[0];

            fsExtension = fsFileName.split('.').pop().toUpperCase();

            if (fsExtension != "XLS" && fsExtension != "XLSX" && fsExtension != "DOC" && fsExtension != "DOCX" && fsExtension != "PDF") {
                Common.Alert.Warning("Please upload an Excel or PDF File.");
            }
            else {

                formData.append('BOQDoc', fsFile);

                var l = Ladda.create(document.querySelector("#btUpload"));
                $.ajax({
                    url: '/api/BOQData/UploadFile',
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
                    if (data !== "Exception") {
                        if (data.length <= 0) {
                            $(".fileinput").fileinput("clear");
                            Common.Alert.Warning("The uploaded file is invalid or empty. Please use the available template, and fill the file correctly with valid product data.");
                        } else {
                            $(".fileinput").fileinput("clear");
                            UploadFile = data;
                        }
                    } else {
                        $(".fileinput").fileinput("clear");
                        Common.Alert.Warning("The uploaded file is invalid. Please download the latest template by clicking the Download Template button.");
                    }

                    if (Common.CheckError.Object(data)) {
                        Common.Alert.Success("File Successfully Uploaded!")
                        TableDone.Search();
                    }

                    l.stop();
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    Common.Alert.Error(errorThrown)
                    l.stop()
                });

            }
        } else {
            Common.Alert.Warning("Please upload an Excel or PDF File.");
        }

        console.log(UploadFile);
        return UploadFile.filePath;
    },

}

var Process = {
    AddToList: function () {
        Data.isDifferent = false;
        var l = Ladda.create(document.querySelector("#btAddSite"))
        var oTable = $('#tblSummaryData').DataTable();
        var oTableSite = $('#tblSiteData').DataTable();

        if (Data.RowSelected.length == 0) {
            Common.Alert.Warning("Please Select One or More Data")
        }
        else {
            TableSite.AddSite();

            if (fsTotalAmount > 25000000000) {
                Common.Alert.Warning("Total Amount more than 25 M!");
            }
            else {
                $(".panelSiteData").fadeIn();
                oTable.rows('.active').every(function (rowIdx, tableLoop, rowLoop) {
                    var checkBoxId = this.data().ID;
                    $("#Row" + checkBoxId).removeClass('active');
                    $("#" + checkBoxId).hide();
                });

                // Hide the checkboxes in the cloned table
                $.each(Data.RowSelected, function (index, item) {
                    $(".Row" + item).removeClass('active');
                    $("." + item).hide();
                });

                //insert Data.RowSelectedSite for rendering checkbox
                $.each(Data.RowSelected, function (index, item) {
                    if (Data.RowSelectedSite.indexOf(parseInt(item)) == -1)
                        Data.RowSelectedSite.push(parseInt(item));
                });

            }

        }
    },

    SetDataBOQAmount: function (fsID) {
        var params = {
            ID: fsID
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/BOQData/GetDataBOQAmount",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $("#txtTotalRenewalTenantApprove").val((data[0].TotalTenant));
                $("#txtTotalAmountApprove").val((Helper.ConvertToRupiah(data[0].TotalAmount)));
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    SetDataBOQHeader: function (strBOQNumber) {
        var params = {
            strBOQNumber: fsBOQNumber
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/BOQData/SetDataBOQHeader",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $("#txtTotalRenewalTenant").val(data[0].TotalTenant);
                $("#txtTotalAmount").val(Helper.ConvertToRupiah(TotalAmount));
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    InsertBOQData: function () {
        Data.SiteRow = [];
        var l = Ladda.create(document.querySelector("#btAddBOQ"))
        var oTable = $('#tblSiteData').DataTable();

        oTable.rows('').every(function (rowIdx, tableLoop, rowLoop) {
            Data.SiteRow.push(this.data());
        });

        var params = {
            CompanyId: fsCompanyId,
            AreaId: fsArea,
            TotalTenant: fsTotalTenant,
            TotalAmount: fsTotalAmount,
            ListTrxReconcile: Data.SiteRow
        };

        $.ajax({
            url: "/api/BOQData/InsertBOQData",
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
                Common.Alert.Successhtml("Data Success Insert With BOQ No :<b>" + data.BOQNumber + "</b>")
                Form.Cancel();
            }
            l.stop();

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            $(".panelSiteData").fadeOut();
            Table.Search();
            Form.ClearRowSelected();
        })
    },

    ApproveRejectBOQData: function () {

        Data.SiteRow = [];
        var l = Ladda.create(document.querySelector("#btSave"))
        var oTable = $('#tblProcessApproveReject').DataTable();

        oTable.rows('').every(function (rowIdx, tableLoop, rowLoop) {
            Data.SiteRow.push(this.data().ID);
        });

        fsTotalTenant = $("#txtTotalRenewalTenantApprove").val() == "" ? 0 : $("#txtTotalRenewalTenantApprove").val();
        fsTotalAmount = $("#txtTotalAmountApprove").val() == "" ? 0 : $("#txtTotalAmountApprove").val();
        fsStatusBOQ = $("#slStatus").val() == "" ? 0 : $("#slStatus").val();
        fsRemarkOnApproval = $("#txtRemarksApprove").val() == "" ? 0 : $("#txtRemarksApprove").val();

        var params = {
            ID: fsID,
            TotalTenant: fsTotalTenant,
            TotalAmount: fsTotalAmount,
            mstRAActivityID: fsStatusBOQ,
            Remarks: fsRemarkOnApproval,
            detailIDs: Data.SiteRow,
        };

        $.ajax({
            url: "/api/BOQData/ApproveRejectBOQData",
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
                if (fsStatusBOQ == RAActivity.BOQ_DONE) {
                    Common.Alert.Success("Data Success Approve")
                } else {
                    Common.Alert.Success("Data Success Reject")
                }
                TableProcess.Search();
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
    },

    SetSignatory: function (fsID) {

        var value1 = {}; value2 = {}; value3 = {}; value4 = {};
        Data.RowSignatory = [];

        //Mandatory Signatory
        value1.ID = null;
        value1.mstRABoqID = fsID;
        value1.StatusEmployee = "Operator";
        value1.NoSigner = 1;
        value1.CompanyName = $("#slOperatorSign").val() == "" ? null : $("#slOperatorSign").val();
        value1.PICName = $("#slOperatorPICNameSign").val() == "" ? null : $("#slOperatorPICNameSign").val();
        value1.Position = $("#slOperatorPositionSign").val() == "" ? null : $("#slOperatorPositionSign").val();
        value1.CreatedBy = null;
        value1.CreatedDate = null;
        value1.UpdatedBy = null;
        value1.UpdatedDate = null;
        Data.RowSignatory.push(value1);

        value2.ID = null;
        value2.mstRABoqID = fsID;
        value2.StatusEmployee = "Vendor";
        value2.NoSigner = 1;
        value2.CompanyName = $("#slCompanySign").val() == "" ? null : $("#slCompanySign").val();
        value2.PICName = $("#slCompanyPICNameSign").val() == "" ? null : $("#slCompanyPICNameSign").val();
        value2.Position = $("#slCompanyPositionSign").val() == "" ? null : $("#slCompanyPositionSign").val();
        value2.CreatedBy = null;
        value2.CreatedDate = null;
        value2.UpdatedBy = null;
        value2.UpdatedDate = null;
        Data.RowSignatory.push(value2);

        return Data.RowSignatory;

    },

}

var Control = {
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/TBGSys_Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchCompanyName").html("<option></option>")
            $("#slSearchCompanyNameProcess").html("<option></option>")
            $("#slSearchCompanyNameDone").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchCompanyName").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                    $("#slSearchCompanyNameProcess").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                    $("#slSearchCompanyNameDone").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
            }

            $("#slSearchCompanyName").select2({ placeholder: "Select Company Name", width: null });
            $("#slSearchCompanyNameProcess").select2({ placeholder: "Select Company Name", width: null });
            $("#slSearchCompanyNameDone").select2({ placeholder: "Select Company Name", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectRegional: function (fsArea) {
        var params = {
            strArea: fsArea
        };
        $.ajax({
            url: "/api/BOQData/Region",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchRegional").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchRegional").append("<option value='" + item.RegionID + "'>" + item.RegionName + "</option>");
                })
            }

            $("#slSearchRegional").select2({ placeholder: "Select Regional", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectArea: function () {
        $.ajax({
            url: "/api/BOQData/AreaOM",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchArea").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchArea").append("<option value='" + item.AreaID + "'>" + item.AreaName + "</option>");
                })
            }

            $("#slSearchArea").select2({ placeholder: "Select Area", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectStatus: function (fsCurrentActivity) {
        var params = {
            CurrentActivity: fsCurrentActivity
        };
        $.ajax({
            url: "/api/BOQData/Status",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slStatus").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slStatus").append("<option value='" + item.ValueId + "'>" + item.ValueDesc + "</option>");
                })
            }

            $("#slStatus").select2({ placeholder: "Select Status", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectOperatorSign: function () {

        $.ajax({
            url: "/api/BOQData/OperatorSign",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slOperatorSign").html("<option></option>")
            $("#slOperatorPICNameSign").html("<option></option>")
            $("#slOperatorPositionSign").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slOperatorSign").append("<option value='" + item.CustomerName + "'>" + item.CustomerName + "</option>");
                    $("#slOperatorPICNameSign").append("<option value='" + item.PICName + "'>" + item.PICName + " </option>");
                    $("#slOperatorPositionSign").append("<option value='" + item.Jabatan + "'>" + item.Jabatan + "</option>");
                })
            }

            $("#slOperatorSign").select({ placeholder: "Select Operator", width: null });
            $("#slOperatorPICNameSign").select({ placeholder: "Select PIC", width: null });
            $("#slOperatorPositionSign").select({ placeholder: "Select Position", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectCompanySign: function () {

        $.ajax({
            url: "/api/BOQData/CompanySign",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slCompanySign").html("<option></option>")
            $("#slCompanyPICNameSign").html("<option></option>")
            $("#slCompanyPositionSign").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slCompanySign").append("<option value='" + item.CustomerName + "'>" + item.CustomerName + "</option>");
                    $("#slCompanyPICNameSign").append("<option value='" + item.PICName + "'>" + item.PICName + " </option>");
                    $("#slCompanyPositionSign").append("<option value='" + item.Jabatan + "'>" + item.Jabatan + "</option>");
                })
            }

            $("#slCompanySign").select({ placeholder: "Select Company", width: null });
            $("#slCompanyPICNameSign").select({ placeholder: "Select PIC", width: null });
            $("#slCompanyPositionSign").select({ placeholder: "Select Position", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

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

    ScrollToTop: function () {
        $("html, body").animate({ scrollTop: theOffset.top - 50 }, "slow");
        return false;
    },

    ConvertToRupiah: function convertToRupiah(angka) {
        var rupiah = '';
        var angkarev = angka.toString().split('').reverse().join('');
        for (var i = 0; i < angkarev.length; i++) if (i % 3 == 0) rupiah += angkarev.substr(i, 3) + ',';
        return rupiah.split('', rupiah.length - 1).reverse().join('');
    },

    GetListId: function () {
        //for CheckAll Pages
        var params = {
            strOperator: fsOperatorId,
            strCompanyId: fsCompanyId,
            strYear: fsYear,
            strArea: fsArea,
            strRegional: fsRegional,
            strSoNumber: fsSoNumber,
        };

        var AjaxData = [];
        $.ajax({
            url: "/api/BOQData/GetListId",
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

    GetListProcessId: function () {

        fsCompanyId = $("#slSearchCompanyNameProcess").val() == null ? "" : $("#slSearchCompanyNameProcess").val();
        fsBOQNumber = $("#slBOQNumberProcess").val() == null ? "" : $("#slBOQNumberProcess").val();

        var params = {
            strCompanyId: fsCompanyId,
            strBOQNumber: fsBOQNumber,
        };

        var AjaxData = [];
        $.ajax({
            url: "/api/BOQData/GetListProcessId",
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

    ValidationSearch: function () {

        var msg = '';

        if ($("#slSearchCompanyName").val() == "") {
            msg = msg + "Company must be choosed! \n"
        }
        return msg;
    },

    ValidationSearchDone: function () {

        var msg = '';

        if ($("#slSearchCompanyNameDone").val() == "") {
            msg = msg + "Company must be choosed! \n"
        }
        return msg;
    },

    //Init Currency input on blur
    InitCurrencyInput: function (selector) {
        $(selector).unbind().on("blur", function () {
            var value = $(selector).val();
            if (value != "") {
                $(selector).val(Common.Format.CommaSeparation(value));
            } else {
                $(selector).val("0.00");
            }
        })
    },

    GetDetailID: function () {
        var oTableSite = $('#tblSiteData').DataTable();
        var GetDetailIDs = [];
        oTableSite.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var id = this.data().ID;
            GetDetailIDs.push(parseInt(id));
        });
        return GetDetailIDs;
    },

    ValidationApply: function () {

        var msg = '';

        if ($("#slOperatorSign").val() == "") {
            msg = msg + "Operator must be filled! \n"
        }
        if ($("#slOperatorPICNameSign").val() == "") {
            msg = msg + "Operator PIC Name must be filled! \n"
        }
        if ($("#slOperatorPositionSign").val() == "") {
            msg = msg + "Operator Position must be filled! \n"
        }
        if ($("#slCompanySign").val() == "") {
            msg = msg + "Company must be filled! \n"
        }
        if ($("#slCompanyPICNameSign").val() == "") {
            msg = msg + "Company PIC Name must be filled! \n"
        }
        if ($("#slCompanyPositionSign").val() == "") {
            msg = msg + "Company Position must be filled! \n"
        }

        return msg;

    },

    FileValidation: function (fileInput) {
        if (fileInput.files.length != 0) {
            fsFileName = fileInput.files[0].name;
            fsFile = fileInput.files[0];
            fsExtension = fsFileName.split('.').pop().toUpperCase();
            if ((fsFile.size / 1024) > 2048) {
                fsMsg = fsMsg + "Upload File Can`t bigger then 2048 bytes (2mb)..!  <br/>";
                return false;
            }
            if (fsExtension != "XLS" && fsExtension != "XLSX" && fsExtension != "PDF") {
                fsMsg = fsMsg + "Please upload an Excel or PDF File..!  <br/>";
                return false;
            }
            else {
                return true
            }
            errors = false;
        }
        else {
            return false
        }

    },
}


