Data = {};

//filter search 
var fsBAPSNumber = "";
var fsBAPSSignDate = "";
var fsCompanyInv = "";
var fsOperator = "";
var fsPONumber = "";
var fsRemarks = "";
var fsRemarksApproval = "";
var fsAttachment = "";
var fsAmountBAPS = "";
var fstotalTenant = 0;
var fstotalAmount = 0;
var fsAmount = 0;
var fsFileName = "";
var fsExtension = "";
var fsFilePath = "";
var fsFile = "";
var fsID = "";
var fsIDDetail = "";
var fsSoNumber = "";
var fsSiteIDOpr = "";
var fsMLANumber = "";
var fsBaseLeasePrice = "";
var fsDeductionPrice = "";
var fsSiteIDName = "";
var fsRFIOprDate = "";
var fsServicePrice = "";
var fsTotalPrice = "";
var fsMsg = "";

jQuery(document).ready(function () {
    Form.Init();
    Table.Init();
    TableProcess.Init();
    TableDone.Init();

    $(".panelSearchResult").fadeIn();

    $('#tabBAPS').unbind().on('tabsactivate', function (event, ui) {
        var newIndex = ui.newTab.index();
        if (newIndex == 0) {
            Data.RowSelected = [];
            Data.RowSelectedSite = [];
            if (Data.RowSelected.length == 0) {
                $(".panelSiteData").fadeOut();
            } else {
                $(".panelSiteData").fadeIn();
            }
            $(".panelDoneDetail").fadeOut();
            $(".panelDoneHeader").fadeIn();
            $("#btAssignSite").show();
            $("#btResetAssign").show();
            $("#btReturn").hide();
        }
            //else if (newIndex == 1) {
            //    Data.RowSelectedProcess = [];
            //    Data.RowSelectedProcessDetail = [];
            //    TableProcess.Search();
            //    $(".panelSiteData").fadeOut();
            //    $("#btAssignSite").hide();
            //    $("#btResetAssign").hide();
            //    $("#btReturn").hide();
            //}
        else {
            Data.RowSelectedDone = [];
            Data.RowSelectedDoneDetail = [];
            $(".panelSiteData").fadeOut();
            $(".panelDoneDetail").fadeOut();
            $(".panelDoneHeader").fadeIn();
            $("#btAssignSite").hide();
            $("#btResetAssign").hide();
            $("#btReturn").show();
        }
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

    $("#file").change(function () {
        $("#lblError").html("");
        fileInput = document.getElementById("file")
        if (!Helper.FileValidation(fileInput)) {
            $("#lblError").html(fsMsg);
            $("#file").val("")
        }
    })
});

var Table = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        $(".PORef").hide();

        var tblSummary = $('#tblSummaryData').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });

        $("#slSearchPO").select2({
            tags: true,
            multiple: true,
            width: "270px",
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

        $("#btSearchAssign").unbind().click(function () {
            Table.Search();
        });

        $("#btResetAssign").unbind().click(function () {
            Form.Cancel();
            Table.Reset();
        });

        $("#slSearchOperator").change(function () {
            fsOperator = $("#slSearchOperator").val();
            if ($.trim(fsOperator) == "TSEL" || $.trim(fsOperator) == "ISAT") {
                $(".PORef").show();
            }
            else {
                $(".PORef").hide();
            }
            Control.BindingSelectPOReference();
        });

        $("#slSearchPO").change(function () {
            Table.Search();
        });

        $("#btAssignSite").unbind().click(function () {
            var fileInput = document.getElementById("file");

            if (fileInput.files.length != 0) {
                fsFileName = fileInput.files[0].name;
                fsFile = fileInput.files[0];

                fsExtension = fsFileName.split('.').pop().toUpperCase();

                if (fsExtension != "XLS" && fsExtension != "XLSX" && fsExtension != "DOC" && fsExtension != "DOCX" && fsExtension != "PDF") {
                    Common.Alert.Warning("Please upload an Excel or PDF or DOC File.");
                }
                else if ((fsFile.size / 1024) > 2048) {
                    Common.Alert.Warning("Upload File Can`t bigger then 2048 bytes (2mb).");
                }
                else {
                    Process.AddToAssignList();
                }
            } else {
                var operator = $('#slSearchOperator option:selected').val();
                console.log('OPERATOR:', operator);
                console.log(operator);
                if (operator.trim() == 'MITEL' || operator.trim() == 'PAB' || operator.trim() == 'TELKOM') {
                    Process.AddToAssignList();
                }
                else {
                    Common.Alert.Warning("Please Upload an File!")
                }
            }
        });

        $("#btProcessBAPS").unbind().click(function () {
            $('#mdlToProcess').modal('toggle');
        });

        $("#btYesConfirm").unbind().click(function () {
            Process.SaveBAPSBulky();
            $('#mdlToProcess').modal('hide');
        });

        $("#btCancel").unbind().click(function () {
            Form.Cancel();
            Table.Search();
        });

    },

    Search: function () {

        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsCustomerId = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsPONumber = $("#slSearchPO").val() == null ? "" : $("#slSearchPO").val();

        var params = {
            CompanyId: fsCompanyId,
            CustomerId: fsCustomerId,
            PONumber: fsPONumber,
        };
        var tblSummary = $("#tblSummaryData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/BAPSBulky/grid",
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
                                    strReturn += "<label style='display:none;' id='" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.ID + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                } else {
                                    strReturn += "<label id='" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.ID + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                }
                            }
                            else {
                                strReturn += "<label id='" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.ID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "Term" },
                {
                    data: "BAPSStartDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "BAPSEndDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "SiteName" },
                { data: "CustomerID" },
                { data: "Product" },
                { data: "CompanyName" },
                { data: "RegionName" },
                { data: "BAPSType" },
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AdditionalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "DeductionAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "PenaltySlaAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AmountIDR", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') }
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {

                if (Common.CheckError.List(tblSummary.data())) {
                    $(".panelSearchResult").fadeIn();
                }
                App.unblockUI('.panelSearchResult');

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
            "scrollY": 600,
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
        $("#lblError").html("");

        $(':input', this).val('');
        $(".fileinput").fileinput("clear");

        $("#txtBAPSNumber").val("");
        $("#tbBAPSSignDate").val("");
        $("#txtAreaApprove").val("");
        $("#slSearchCompany").val("").trigger('change');
        $("#slSearchOperator").val("").trigger('change');
        $("#txtRemarks").val("");
        $("#txtTenantBAPS").val("");
        $("#txtAmountBAPS").val("");

        var tblSummary = $('#tblSummaryData').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });
    },

    Export: function () {

        fsOperatorId = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsPONumber = $("#slSearchPO").val() == null ? "" : $("#slSearchPO").val();

        var strOperator = fsOperatorId;
        var strCompanyId = fsCompanyId;
        var strPONumber = fsPONumber;

        window.location.href = "/RevenueAssurance/trxBAPSInput/Export?strOperator=" + strOperator + "&strCompanyId=" + strCompanyId + "&strPONumber=" + strPONumber;
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

    AddSite: function () {
        //Get Data that in RowSelected
        var ajaxData = TableSite.GetSelectedSiteData(Data.RowSelected);

        var tblSiteData = $("#tblSiteData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "data": ajaxData,
            "filter": false,
            "destroy": true,
            "lengthMenu": [[25, 50], ['25', '50']],
            "columns": [
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<button type='button' title='Delete' class='btn btn-xs red btDeleteSite' id='btDeleteSite" + full.ID + "'><i class='fa fa-trash'></i></button>";
                        return strReturn;
                    }
                },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "Term" },
                {
                    data: "BAPSStartDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "BAPSEndDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "SiteName" },
                { data: "CustomerID" },
                { data: "Product" },
                { data: "CompanyName" },
                { data: "RegionName" },
                { data: "BAPSType" },
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AdditionalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "DeductionAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "PenaltySlaAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AmountIDR", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') }
                //{ data: "StatusPO" }
                //{ data: "BAPSValidation" }

            ],
            "buttons": [],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;
                //var colNumber = [16];

                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                        i : 0;
                };

                //var numformat = $.fn.dataTable.render.number(',', '.', 2, '').display;
                fstotalTenant = 0;
                fstotalAmount = 0;
                var oTableSite = $('#tblSiteData').DataTable();
                var Percent = $('input[name=rbPercent]:checked').val();

                oTableSite.rows('').every(function (rowIdx, tableLoop, rowLoop) {
                    fstotalTenant += 1
                    fstotalAmount += this.data().AmountIDR;
                });

                $(api.column(17).footer()).html(Helper.numformat(fstotalAmount));
                $("#txtTenantBAPS").val(fstotalTenant);
                $("#txtAmountBAPS").val(Helper.numformat(fstotalAmount));
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
        var l = Ladda.create(document.querySelector("#btAssignSite"));
        $.ajax({
            //url: "/api/BAPSBulky/SitelistGrid",
            //type: "POST",
            //dataType: "json",
            //async: false,
            //contentType: "application/json",
            //data: JSON.stringify(params),
            //cache: false,
            //beforeSend: function (xhr) {
            //    l.start();
            //}
            url: "/api/BAPSBulky/SitelistGrid",
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

    GetSelectedDocument: function (listID) {
        var AjaxData = [];
        var params = {
            ListId: listID
        };
        var l = Ladda.create(document.querySelector("#btSaveBAPS"));
        $.ajax({
            url: "/api/BAPSBulky/DoclistGrid",
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

        $(window).resize(function () {
            $("#tblProcessDetail").DataTable().columns.adjust().draw();
        });

        $("#tblProcess tbody").on("click", "a.btProcessDetail", function (e) {
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
                TableProcess.BAPSProcessDetail(fsID);
                $('#mdlDetail').modal('toggle');
                $('#lblBAPSNumberDetail').text("Detail Data :" + data.BAPSNumber);
            }
        });

        $("#tblProcess tbody").on("click", "a.btApprove", function (e) {
            e.preventDefault();
            $("#slStatus").val("").trigger('change');
            $("#txtRemarksApprove").val("");
            Data.RowSelectedProcess = [];
            var table = $("#tblProcess").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                fsID = data.ID;
                fsCurrentActivity = data.mstRAActivityID;
                $('#mdlApprove').modal('toggle');
                $('#lblBAPSNumberApprove').text("BAPS Number :" + data.BAPSNumber);
                Control.BindingSelectStatus(fsCurrentActivity);
            }
        });

        $("#formApprove").submit(function (e) {
            e.preventDefault();
            Process.ApproveRejectBAPSData();
            $('#mdlApprove').modal('hide');
        });

        $("#tblProcess tbody").on("click", "a.btDocumentProcess", function (e) {
            e.preventDefault();
            var table = $("#tblProcess").DataTable();
            var data = table.row($(this).parents("tr")).data();
            Data.Selected = data;
            fsFilepath = data.FilePath;
            var FileNames = fsFilepath.split('\\');
            fsFilename = FileNames[3].toString();
            fsContentType = data.ContentType

            window.location.href = "/RevenueAssurance/DownloadDocument?Filepath=" + fsFilepath + "&fileName=" + fsFilename + "&ContentType=" + fsContentType;
        });

    },

    Search: function () {

        var tblProcess = $("#tblProcess").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/BAPSBulky/gridProcess",
                "type": "POST",
                "datatype": "json",
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
            "filter": false,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    data: "DocumentSigned", mRender: function (data, type, full) {
                        return "<center><a href='' class='btDocumentProcess'><i class='fa fa-download'></i></a></center>";
                    }
                },
                { data: "BAPSNumber" },
                {
                    data: "TotalTenant", mRender: function (data, type, full) {
                        return "<a href='' class='btProcessDetail'>" + data + "</a>";
                    }
                },
                { data: "TotalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Remarks" },
                {
                    data: "BAPSSignDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "CreatedBy" },
                {
                    data: "Status", mRender: function (data, type, full) {
                        if ($("#hdAllowApproval").val() && full.Status != "Approved") {
                            return "<a href='#' class='btApprove'>" + full.Status + "</a>";
                        }
                        else {
                            return full.Status;
                        }

                    }
                },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            //"fnPreDrawCallback": function () {
            //    App.blockUI({ target: ".panelSearchResult", boxed: true });
            //},
            "fnDrawCallback": function () {

                if (Common.CheckError.List(tblProcess.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
            },
            "order": [1, "asc"],
            //"scrollY": 300,
            //"scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            //"scrollCollapse": true,
            //"fixedColumns": {
            //    leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            //},
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },

        });

    },

    BAPSProcessDetail: function (fsID) {
        var params = {
            ID: fsID
        };

        var tblProcessDetail = $("#tblProcessDetail").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/BAPSBulky/gridProcessDetail",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            "buttons": [],
            "filter": false,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            "columns": [
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "Term" },
                {
                    data: "bapsStartDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "bapsEndDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "CustomerID" },
                { data: "Product" },
                { data: "companyName", className: "width:100px" },
                { data: "RegionName" },
                { data: "BapsType" },
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AdditionalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "DeductionAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "PenaltySlaAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AmountIDR", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') }
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            //"order": [1, "asc"],
            //"scrollY": 300,
            //"scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            //"scrollCollapse": true,
            //"fixedColumns": {
            //    leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            //},
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },

        });

        $(window).resize(function () {
            $("#tblProcessDetail").DataTable().columns.adjust().draw();
        });
    },

    Reset: function () {
        $('#txtSoNumber').val("");
        $('#txtTerm').val("");
        $('#txtSiteIDOpr').val("");
        $('#txtMLANumber').val("");
        $('#txtBaseLeasePrice').val("");
        $('#txtDeductionPrice').val("");
        $('#txtStipSiro').val("");
        $('#txtBapsType').val("");
        $('#txtSiteIDName').val("");
        $('#txtRFIOprDate').val("").trigger();
        $('#txtServicePrice').val("");
        $('#txtTotalPrice').val("");
    }
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

        var tblDoneDetail = $('#tblDoneDetail').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblDone").DataTable().columns.adjust().draw();
        });

        $(window).resize(function () {
            $("#tblDoneDetail").DataTable().columns.adjust().draw();
        });

        $("#btSearchDone").unbind().click(function () {
            TableDone.Search();
        });

        $("#btResetDone").unbind().click(function () {
            TableDone.Reset();
            var tblDone = $('#tblDone').dataTable({
                "filter": false,
                "destroy": true,
                "data": []
            });
        });

        $("#slBAPSNumberDone").select2({
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

        Helper.InitCurrencyInput("#txtAmountBAPS");
        Helper.InitCurrencyInput("#txtBaseLeasePrice");
        Helper.InitCurrencyInput("#txtServicePrice");
        Helper.InitCurrencyInput("#txtDeductionPrice");
        Helper.InitCurrencyInput("#txtTotalPrice");

        $('#txtBaseLeasePrice').on('input', function (e) {
            Control.BindProRateAmount();
        });

        $('#txtServicePrice').on('input', function (e) {
            Control.BindProRateAmount();
        });

        $('#txtDeductionPrice').on('input', function (e) {
            Control.BindProRateAmount();
        });

        $('#tblDone').on('change', 'tbody tr .checkboxes', function () {
            var id = $(this).parent().attr('id');
            if (this.checked) {
                $(this).parents('tr').addClass("active");
                $(".Row" + id).addClass("active");
                Data.RowSelectedDone.push(parseInt(id));
            } else {
                $(this).parents('tr').removeClass("active");
                $(".Row" + id).removeClass("active");
                Helper.RemoveElementFromArray(Data.RowSelectedDone, parseInt(id));
            }
        });

        $('#tblDone').find('.group-checkable').change(function () {
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
                Data.RowSelectedDone = Helper.GetListDoneId(1);
            else
                Data.RowSelectedDone = [];
        });

        $("#tblDone tbody").on("click", "a.btDocumentDone", function (e) {
            e.preventDefault();
            var table = $("#tblDone").DataTable();
            var data = table.row($(this).parents("tr")).data();
            Data.Selected = data;
            fsFilepath = data.FilePath;
            fsFilename = data.FileName;
            fsContentType = data.ContentType

            window.location.href = "/RevenueAssurance/DownloadDocument?Filepath=" + fsFilepath + "&fileName=" + fsFilename + "&ContentType=" + fsContentType;
        });

        $("#tblDone tbody").on("click", "a.btDetailDone", function (e) {
            e.preventDefault();
            $("#btReturn").hide();
            $("#slStatus").val("").trigger('change');
            $("#txtRemarksApprove").val("");
            var table = $("#tblDone").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                var tblProcessDetail = $('#tblDoneDetail').dataTable({
                    "filter": false,
                    "destroy": true,
                    "data": []
                });
                Data.Selected = data;
                fsID = data.ID;
                fsBOQNumber = data.BAPSNumber;
                $(".panelDoneHeader").hide();
                $(".panelDoneDetail").show();
                $('#txtBAPSNumberHeader').val(data.BAPSNumber);
                $('#txtTotalTenantHeader').val(data.TotalTenant);
                $('#txtTotalAmountHeader').val(Helper.numformat(data.TotalAmount));
                TableDone.BAPSDoneDetail(fsID);
            }
        });

        $("#tblDone tbody").on("click", "a.btApproveDone", function (e) {
            e.preventDefault();
            Data.RowSelectedDone = [];
            var table = $("#tblDone").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                fsID = data.ID;
                $('#mdlToApproveDone').modal('toggle');
            }
        });

        $("#tblDoneDetail tbody").on("click", "button.btDeleteBapsDetailDone", function (e) {
            $('#mdlToDelete').modal('show');
            var table = $("#tblDoneDetail").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                fsID = data.ID;
                fsIDDetail = data.trxReconcileID;
            }
        });

        $("#tblDoneDetail tbody").on("click", "button.btEditBapsDetailDone", function (e) {
            e.preventDefault();
            var table = $("#tblDoneDetail").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                fsID = data.ID;
                fsIDDetail = data.trxReconcileID;
                $('#txtSoNumber').val(data.SONumber);
                $('#txtTerm').val(data.Term);
                $('#txtInvoiceStartDate').val(Common.Format.ConvertJSONDateTime(data.StartInvoiceDate));
                $('#txtSiteIDOpr').val(data.SiteID);
                $('#txtMLANumber').val(data.Mla);
                $('#txtBaseLeasePrice').val(Helper.numformat(data.BaseLeasePrice));
                $('#txtDeductionPrice').val(Helper.numformat(data.DeductionAmount));
                $('#txtStipSiro').val(data.StipSiro);
                $('#txtCustomerID').val(data.CustomerID);
                $('#txtInvoiceEndDate').val(Common.Format.ConvertJSONDateTime(data.EndInvoiceDate));
                $('#txtSiteIDName').val(data.SiteName);
                $('#txtRFIOprDate').val(Common.Format.ConvertJSONDateTime(data.RFIDate));
                $('#txtServicePrice').val(Helper.numformat(data.ServicePrice));
                $('#txtTotalPrice').val(Helper.numformat(data.AmountIDR));
                $('#mdlUpdate').modal('toggle');
            }
        });

        $("#btReturn").unbind().click(function () {
            if (Data.RowSelectedDone.length == 0)
                Common.Alert.Warning("Please Select One or More Data")
            else {
                $('#mdlBackToBAPSInput').modal('toggle');
            }
        });

        $("#btYesConfirmBack").unbind().click(function () {
            Process.BackToBAPSInput();
        });

        $("#btEdit").unbind().click(function () {
            TableDone.UpdateDetailBAPS(fsID, fsIDDetail)
            $('#mdlUpdate').modal('hide');
        });

        $("#btYesConfirmDelete").unbind().click(function () {
            TableDone.DeleteDetailBAPS(fsID, fsIDDetail)
            $('#mdlToDelete').modal('hide');
        });

        $("#btYesConfirmApprove").unbind().click(function () {
            Process.ApproveBAPSData(fsID)
            $('#mdlToApproveDone').modal('hide');
        });

        $("#btBack").unbind().click(function () {
            $(".panelDoneHeader").show();
            $(".panelDoneDetail").hide();
            $("#btReturn").show();
            TableDone.Search();
        });

    },

    Search: function () {

        var l = Ladda.create(document.querySelector("#btSearchDone"))
        l.start();

        fsCompanyId = $("#slSearchCompanyNameDone").val() == null ? "" : $("#slSearchCompanyNameDone").val();
        fsBAPSNumber = $("#slBAPSNumberDone").val() == null ? "" : $("#slBAPSNumberDone").val();


        var params = {
            CompanyID: fsCompanyId,
            ListBAPSNumber: fsBAPSNumber
        };

        var tblDone = $("#tblDone").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/BAPSBulky/gridDone",
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
            "lengthMenu": [[25, 50], ['25', '50']],
            "destroy": true,
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Helper.IsElementExistsInArray(parseInt(full.ID), Data.RowSelectedDone)) {
                                strReturn += "<label id='" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.ID + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                            }
                            else {
                                if (full.mstRAActivityID != RAActivity.BAPS_RETURN) {
                                    strReturn += "<label id='" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.ID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                                }
                            }

                            return strReturn;

                        }
                    }
                },
                {
                    data: "BAPSNumber", mRender: function (data, type, full) {
                        return "<a href='' class='btDetailDone'>" + data + "</a>";
                    }
                },
                {
                    data: "DocumentSigned", mRender: function (data, type, full) {
                        if (full.FilePath == '' || full.FilePath == undefined || full.FilePath == null) {
                            //console.log('hello')
                            return "";
                        }
                        else {
                            return "<center><a href='' class='btDocumentDone'><i class='fa fa-download'></i></a></center>";
                        }

                    }
                },
                { data: "TotalTenant" },
                { data: "TotalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Remarks" },
                //{ data: "RemarksApproval" },
                {
                    data: "BAPSSignDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "CreatedBy" },
                //{
                //    data: "ApprovedDate", render: function (data) {
                //        return Common.Format.ConvertJSONDateTime(data);
                //    }
                //},
                //{ data: "ApprovedBy" },
                {
                    data: "Status", mRender: function (data, type, full) {
                        if (full.Status == "Return from Finance") {
                            return "<a href='' class='btApproveDone'>" + full.Status + "</a>";
                        }
                        else {
                            return full.Status;
                        }

                    }
                },


            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {

                if (Common.CheckError.List(tblDone.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }

                l.stop(); App.unblockUI('.panelSearchResult');

            },
            "order": [1, "asc"],
            "scrollY": 600,
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            //"fixedColumns": {
            //    leftColumns: 1 /* Set the 2 most left columns as fixed columns */
            //},
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },

        });


    },

    BAPSDoneDetail: function (fsID) {
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
                "url": "/api/BAPSBulky/gridDoneDetail",
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
                            TableDone.ExportDetail()
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
                        if (full.mstRAactivityID == RAActivity.BAPS_RETURN) {
                            strReturn += "<button type='button' title='Delete' class='btn btn-xs red btDeleteBapsDetailDone' id='btDeleteBapsDetailDone" + full.ID + "'><i class='fa fa-trash'></i></button>";
                        }
                        return strReturn;
                    }
                },
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        if (full.mstRAactivityID == RAActivity.BAPS_RETURN) {
                            var strReturn = "<button type='button' title='Edit' class='btn btn-xs green btEditBapsDetailDone' id='btEditBapsDetailDone" + full.ID + "'><i class='fa fa-edit'></i></button>";
                        }
                        return strReturn;
                    }
                },
                { data: "Status" },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "Term" },
                {
                    data: "bapsStartDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "bapsEndDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "CustomerID" },
                { data: "Product" },
                { data: "companyName", className: "width:100px" },
                { data: "RegionName" },
                { data: "BapsType" },
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AdditionalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "DeductionAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "PenaltySlaAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AmountIDR", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            //"footerCallback": function (row, data, start, end, display) {
            //    var api = this.api(), data;
            //    var colNumber = [5];


            //    var intVal = function (i) {
            //        return typeof i === 'string' ?
            //            i.replace(/[\$,]/g, '') * 1 :
            //            typeof i === 'number' ?
            //            i : 0;
            //    };

            //    //var numformat = $.fn.dataTable.render.number(',', '.', 2, '').display;

            //    for (i = 0; i < colNumber.length; i++) {

            //        var colNo = colNumber[i];

            //        AmountIDR = api
            //                .column(colNo, { page: 'current' })
            //                .data()
            //                .reduce(function (a, b) {
            //                    return intVal(a) + intVal(b);
            //                }, 0);
            //        $(api.column(colNo).footer()).html(Helper.numformat(AmountIDR));

            //        $("#txtTotalAmountApprove").val(Helper.numformat(AmountIDR));
            //    }
            //},
            "order": [2, "desc"],
            //"scrollY": 300,
            //"scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            //"scrollCollapse": true,
            //"fixedColumns": {
            //    leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            //},
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },

        });

        $(window).resize(function () {
            $("#tblProcessDetail").DataTable().columns.adjust().draw();
        });
    },

    UpdateDetailBAPS: function (fsID, fsIDDetail) {

        Data.SiteRow = [];
        var l = Ladda.create(document.querySelector("#btEdit"))
        var oTable = $('#tblProcessDetail').DataTable();

        oTable.rows('').every(function (rowIdx, tableLoop, rowLoop) {
            Data.SiteRow.push(this.data().ID);
        });

        fsSoNumber = $("#txtSoNumber").val() == "" ? 0 : $("#txtSoNumber").val();
        fsSiteIDOpr = $("#txtSiteIDOpr").val() == "" ? 0 : $("#txtSiteIDOpr").val();
        fsMLANumber = $("#txtMLANumber").val() == "" ? 0 : $("#txtMLANumber").val();
        fsBaseLeasePrice = $("#txtBaseLeasePrice").val() == "" ? 0 : $("#txtBaseLeasePrice").val();
        fsDeductionPrice = $("#txtDeductionPrice").val() == "" ? 0 : $("#txtDeductionPrice").val();
        fsSiteIDName = $("#txtSiteIDName").val() == "" ? 0 : $("#txtSiteIDName").val();
        fsRFIOprDate = $("#txtRFIOprDate").val() == "" ? 0 : $("#txtRFIOprDate").val();
        fsServicePrice = $("#txtServicePrice").val() == "" ? 0 : $("#txtServicePrice").val();
        fsTotalPrice = $("#txtTotalPrice").val() == "" ? 0 : $("#txtTotalPrice").val();

        var params = {
            ID: fsID,
            trxReconcileID: fsIDDetail,
            SoNumber: fsSoNumber,
            CustomerSiteID: fsSiteIDOpr,
            CustomerSiteName: fsSiteIDName,
            CustomerMLANumber: fsMLANumber,
            BaseLeasePrice: fsBaseLeasePrice,
            DeductionAmount: fsDeductionPrice,
            ServicePrice: fsServicePrice,
            AmountIDR: fsTotalPrice,
            RFIOprDate: fsRFIOprDate,
        };

        $.ajax({
            url: "/api/BAPSBulky/UpdateDetailBAPS",
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
                $('#txtTotalTenantHeader').val(data.TotalTenant);
                $('#txtTotalAmountHeader').val(Helper.numformat(data.TotalAmount));
                TableDone.BAPSDoneDetail(fsID);
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })

    },

    DeleteDetailBAPS: function (fsID, fsIDDetail) {

        Data.SiteRow = [];
        var l = Ladda.create(document.querySelector("#btYesConfirmDelete"))
        var oTable = $('#tblDoneDetail').DataTable();

        oTable.rows('').every(function (rowIdx, tableLoop, rowLoop) {
            Data.SiteRow.push(this.data().ID);
        });

        var params = {
            ID: fsID,
            trxReconcileID: fsIDDetail
        };

        $.ajax({
            url: "/api/BAPSBulky/DeleteDetailBAPS",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            async: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                $('#txtTotalTenantHeader').val(data.TotalTenant);
                $('#txtTotalAmountHeader').val(Helper.numformat(data.TotalAmount));
                TableDone.BAPSDoneDetail(fsID);

            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })

    },

    Reset: function () {

        $("#slSearchCompanyNameDone").val("").trigger('change');
        $("#slBAPSNumberDone").val("").trigger('change');

        fsCompanyId = "";
        fsBAPSNumber = "";

    },

    Export: function () {

        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsBAPSNumber = $("#slSearchPO").val() == null ? "" : $("#slSearchPO").val();

        var strCompanyId = fsCompanyId;
        var strBAPSNumber = fsBAPSNumber;

        window.location.href = "/RevenueAssurance/trxBAPSDone/Export?strCompanyId=" + strCompanyId + "&strBAPSNumber=" + strBAPSNumber;
    },

    ExportDetail: function () {

        window.location.href = "/RevenueAssurance/trxBAPSDoneDetail/Export?strID=" + fsID;
    }
}

var Form = {
    Init: function () {

        $('#tabBAPS').tabs();
        $("#btReturn").hide();

        //Bind dropdown list
        Control.BindingSelectCompany();
        Control.BindingSelectOperator();
        //Control.BindingSelectPOReference();
        Table.Reset();
        Form.Cancel();

        $(".panelSiteData").hide();
        $(".panelSiteDetail").hide();

        // validasi checkbox
        Data.RowSelected = [];

        //untuk validasi table site
        Data.RowSelectedSite = [];

        //validasi checkbox ListDocument
        Data.RowSelectedDoc = [];
    },

    UploadBAPS: function (fsID) {

        var formData = new FormData();
        formData.append("ID", fsID);
        formData.append('BAPSDoc', fsFile);

        var l = Ladda.create(document.querySelector("#btProcessBAPS"));
        $.ajax({
            url: '/api/BAPSBulky/UploadFile',
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
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        });

        //console.log(UploadFile);
        return UploadFile.strFilePath;
    },

    ClearRowSelected: function () {
        Data.RowSelected = [];
        Data.RowSelectedSite = [];
    },

    Cancel: function () {
        fstotalTenant = "";
        fstotalTenant = "";
        Data.RowSelected = [];
        $('input:checkbox').removeAttr('checked');
        $(".panelSiteData").hide();


    },

}

var Process = {
    AddToAssignList: function () {
        Data.isDifferent = false;
        //Data.CheckedRow = [];
        var l = Ladda.create(document.querySelector("#btAssignSite"))
        var oTable = $('#tblSummaryData').DataTable();
        var oTableSite = $('#tblSiteData').DataTable();

        if (Data.RowSelected.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {

            var ValidationField = Helper.ValidationField();

            if (ValidationField != "") {
                Common.Alert.Warning(ValidationField);
            } else {

                TableSite.AddSite();

                $(".panelSiteData").fadeIn();
                //oTable.rows('.active').every(function (rowIdx, tableLoop, rowLoop) {
                //    var checkBoxId = this.data().ID;
                //    $("#Row" + checkBoxId).removeClass('active');
                //    $("#" + checkBoxId).hide();
                //});

                //// Hide the checkboxes in the cloned table
                //$.each(Data.RowSelected, function (index, item) {
                //    $(".Row" + item).removeClass('active');
                //    $("." + item).hide();
                //});

                ////insert Data.RowSelectedSite for rendering checkbox
                //$.each(Data.RowSelected, function (index, item) {
                //    if (Data.RowSelectedSite.indexOf(parseInt(item)) == -1)
                //        Data.RowSelectedSite.push(parseInt(item));
                //});
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

    ProcessBAPS: function () {
        var l = Ladda.create(document.querySelector("#btProcessBAPS"))
        l.start();

        fsCustomerId = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();

        var params = {
            CustomerId: fsCustomerId
        };
        var tblBAPSDoc = $("#tblBAPSDoc").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "async": true,
                "url": "/api/BAPSBulky/BAPSDocument",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            //buttons: [
            //    { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
            //    {
            //        text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
            //            var l = Ladda.create(document.querySelector(".yellow"));
            //            l.start();
            //            Table.Export()
            //            l.stop();
            //        }
            //    },
            //    { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            //],
            "filter": false,
            "destroy": true,
            "columns": [
                { data: "CustomerId" },
                { data: "DocumentName" },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Helper.IsElementExistsInArray(parseInt(full.DocId), Data.RowSelectedDoc)) {
                                strReturn += "<label id='" + full.DocId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.DocId + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                            }
                            else {
                                strReturn += "<label id='" + full.DocId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.DocId + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                }

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
        });
        l.stop();
    },

    SaveBAPSBulky: function () {

        Data.RowSelectedSite = [];

        //var ListDoc = TableSite.GetSelectedDocument(Data.RowSelectedDoc);

        var l = Ladda.create(document.querySelector("#btProcessBAPS"))
        var oTable = $('#tblSiteData').DataTable();

        oTable.rows('').every(function (rowIdx, tableLoop, rowLoop) {
            Data.RowSelectedSite.push(this.data());
        });

        var params = {
            BAPSNumber: fsBAPSNumber,
            CompanyID: fsCompanyInv,
            CustomerID: fsCustomerId,
            TotalTenant: fstotalTenant,
            TotalAmount: fstotalAmount,
            BAPSSignDate: fsBAPSSignDate,
            Remarks: fsRemarks,
            ListTrxBAPS: Data.RowSelectedSite
            //ListDoc: ListDoc,
        };

        $.ajax({
            url: "/api/BAPSBulky/SaveBAPSBulky",
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

                Form.UploadBAPS(data.ID);
                Common.Alert.Successhtml("Data Success Save With BAPS Number :<b> " + data.BAPSNumber + "</b>")
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
        })
        return fsBAPSNumber
    },

    ApproveRejectBAPSData: function () {

        var l = Ladda.create(document.querySelector("#btSave"))

        fsStatusBOQ = $("#slStatus").val() == "" ? 0 : $("#slStatus").val();
        fsRemarksApproval = $("#txtRemarksApprove").val() == "" ? 0 : $("#txtRemarksApprove").val();

        var params = {
            ID: fsID,
            RemarksApproval: fsRemarksApproval,
            mstRAActivityID: fsStatusBOQ
        };

        $.ajax({
            url: "/api/BAPSBulky/ApproveRejectBAPSData",
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
                if (fsStatusBOQ == RAActivity.BAPS_DONE) {
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

    BackToBAPSInput: function () {
        var l = Ladda.create(document.querySelector("#btYesConfirmBack"))

        var params = {
            detailIDs: Data.RowSelectedDone
        }

        $.ajax({
            url: "/api/BAPSBulky/BacktoBAPSInput",
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
                Common.Alert.Success("Data Success Back to BAPS Input")
                $('#mdlBackToBAPSInput').modal('hide');
            }
            l.stop();

        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        }).always(function (jqXHR, textStatus) {
            TableDone.Search();
            Form.ClearRowSelected();
        })
    },

    ApproveBAPSData: function () {

        var l = Ladda.create(document.querySelector("#btYesConfirmApprove"))

        var params = {
            ID: fsID
        };

        $.ajax({
            url: "/api/BAPSBulky/ApproveBAPSData",
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
                Common.Alert.Success("Data Success Approve")
                TableDone.Search();
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
    },

}

var Control = {
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/TBGSys_Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchCompany").html("<option></option>")
            $("#slSearchCompanyNameDone").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchCompany").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                    $("#slSearchCompanyNameDone").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
            }

            $("#slSearchCompany").select2({ placeholder: "Select Company Name", width: null });
            $("#slSearchCompanyNameDone").select2({ placeholder: "Select Company Name", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectOperator: function () {
        $.ajax({
            url: "/api/BAPSBulky/Operator",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchOperator").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchOperator").append("<option value='" + item.OperatorId + "'>" + item.Operator + "</option>");
                })
            }

            $("#slSearchOperator").select2({ placeholder: "Select Operator Name", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectPOReference: function () {

        fsCompany = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();

        var params = {
            strCompany: fsCompany,
            strOperator: fsOperator
        };

        $.ajax({
            url: "/api/BAPSBulky/PORef",
            type: "GET",
            datatype: "json",
            data: params,
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchPO").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchPO").append("<option value='" + item.DataValueField + "'>" + item.DataTextField + "</option>");
                })
            }

            $("#slSearchPO").select2({ placeholder: "Select PO Reference", width: null });

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
            url: "/api/BAPSBulky/Status",
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

    BindProRateAmount: function () {
        var CustomerID = $('#txtCustomerID').val();
        var InvoiceStartDate = $('#txtInvoiceStartDate').val();
        var InvoiceEndDate = $('#txtInvoiceEndDate').val();
        var InvoiceAmount = $('#txtBaseLeasePrice').val().replace(/,/g, "");
        var ServiceAmount = $('#txtServicePrice').val().replace(/,/g, "");
        var DeductionAmount = $('#txtDeductionPrice').val().replace(/,/g, "");

        var params = { CustomerID: CustomerID, StartInvoiceDate: InvoiceStartDate, EndInvoiceDate: InvoiceEndDate, InvoiceAmount: InvoiceAmount, ServiceAmount: ServiceAmount }

        $.ajax({
            url: "/api/ReconcileData/GetProRateAmount",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
        })
        .done(function (data, textStatus, jqXHR) {
            var result = (parseFloat(data.data) - parseFloat(DeductionAmount)).toString();
            $("#txtTotalPrice").val(Common.Format.CommaSeparation(result));
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
        for (var i = 0; i < angkarev.length; i++) if (i % 3 == 0) rupiah += angkarev.substr(i, 3) + '.';
        return rupiah.split('', rupiah.length - 1).reverse().join('');
    },

    numformat: function numformat(angka) {
        var numformat = $.fn.dataTable.render.number(',', '.', 2, '').display;
        return numformat(angka)
    },

    ValidationField: function () {

        fsBAPSNumber = $("#txtBAPSNumber").val() == null ? "" : $("#txtBAPSNumber").val();
        fsCompanyInv = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsCustomerId = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        slSearchPO = $("#slSearchPO").val() == null ? "" : $("#slSearchPO").val();
        fsBAPSSignDate = $("#tbBAPSSignDate").val() == null ? "" : $("#tbBAPSSignDate").val();
        fsRemarks = $("#txtRemarks").val() == null ? "" : $("#txtRemarks").val();
        fsAmountBAPS = $("#txtAmountBAPS").val() == null ? "" : $("#txtAmountBAPS").val();

        var msg = '';

        if (fsBAPSNumber == "") {
            msg = msg + "BAPS Number must be filled! \n"
        }
        if (fsCompanyInv == "") {
            msg = msg + "Company invoice must be choosed! \n"
        }
        if (fsCustomerId == "") {
            msg = msg + "Operator Invoice must be choosed! \n"
        }
        if ($.trim(fsOperator) == "TSEL") {
            if (slSearchPO == "") {
                msg = msg + "PO Number must be choosed! \n"
            }
        }
        if (fsBAPSSignDate == "") {
            msg = msg + "BAPS Sign Date must be selected! \n"
        }

        if ($.trim(fsOperator) == "MITEL" || $.trim(fsOperator) == "TELKOM" || $.trim(fsOperator) == "PAB") {
       
        }
        else {
            if ($("#file").val() == "") {
                msg = msg + "Attachment must be filled! \n"
            }
        }
       
        return msg;
    },

    formatDate: function () {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();
        hour = d.getHours();
        minute = d.getMinutes();
        second = d.getSeconds();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [year, month, day].join('/') + " " + hour + ":" + minute + ":" + second;
    },

    GetListId: function () {
        //for CheckAll Pages
        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsCustomerId = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsPONumber = $("#slSearchPO").val() == null ? "" : $("#slSearchPO").val();

        var params = {
            CompanyId: fsCompanyId,
            CustomerId: fsCustomerId,
            PONumber: fsPONumber,
        };

        var AjaxData = [];
        $.ajax({
            url: "/api/BAPSBulky/GetListId",
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

        var AjaxData = [];
        $.ajax({
            url: "/api/BAPSBulky/GetListProcessId",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
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

    GetListDoneId: function () {

        //for CheckAll Pages
        fsCompanyId = $("#slSearchCompanyNameDone").val() == null ? "" : $("#slSearchCompanyNameDone").val();
        fsBAPSNumber = $("#slBAPSNumberDone").val() == null ? "" : $("#slBAPSNumberDone").val();

        var params = {
            CompanyID: fsCompanyId,
            ListBAPSNumber: fsBAPSNumber,
        };

        var AjaxData = [];
        $.ajax({
            url: "/api/BAPSBulky/GetListDoneId",
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

    InitCurrencyInput: function (selector) {
        $(selector).unbind().on("blur", function () {
            var value = $(selector).val();
            fsAmount = value;
            if (value != "") {
                $(selector).val(Common.Format.CommaSeparation(value));
            } else {
                $(selector).val("0.00");
            }
        })
    },

    FileValidation: function (fileInput) {
        fsMsg = "";
        if (fileInput.files.length != 0) {
            fsFileName = fileInput.files[0].name;
            fsFile = fileInput.files[0];
            fsExtension = fsFileName.split('.').pop().toUpperCase();
            if ((fsFile.size / 1024) > 2048) {
                //Common.Alert.Warning("Upload File Can`t bigger then 2048 bytes (2mb)..!");
                fsMsg = fsMsg + "Upload File Can`t bigger then 2048 bytes (2mb)..!  <br/>";
                return false;
            }
            if (fsExtension != "XLS" && fsExtension != "XLSX" && fsExtension != "PDF") {
                //Common.Alert.Warning("Please upload an Excel or PDF File..!");
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
