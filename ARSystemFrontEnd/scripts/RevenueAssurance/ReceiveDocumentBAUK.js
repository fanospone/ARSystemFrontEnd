Data = {};
AjaxData = [];
param = {};
jQuery(document).ready(function () {
    Form.Init();
    Table.Init();
    Table.ReceiveDocumentBAUK();
    Control.SetParam();
    $("#btReset").unbind().click(function () {
        Form.Reset();
    });

    $("#btSearch").unbind().click(function () {
        Table.ReceiveDocumentBAUK();
    });

    $(".btSearchHeader").unbind().click(function () {
        Table.ReceiveDocumentBAUK();
    });

    $('#tbSummary').on('change', 'tbody tr .checknext', function () {
        var id = $(this).parent().attr('id');
        var table = $("#tbSummary").DataTable();

        var DataRows = {};
        var Row = table.row($(this).parents('tr')).data();

        if (this.checked) {
            DataRows.DeptSubmitBAUK = Row.DeptSubmitBAUK;
            DataRows.BaukSubmitBySystem = Row.BaukSubmitBySystem;
            DataRows.SONumber = Row.SONumber;
            DataRows.SiteID = Row.SiteID;
            DataRows.SiteName = Row.SiteName;
            DataRows.Product = Row.Product;
            DataRows.CustomerName = Row.CustomerName;
            Data.RowSelectedCheckNext.push(DataRows);

            $(this).closest('tr').find('.checknotcomplete').prop('disabled', true);
            $(this).closest('tr').find('.checkcomplete').prop('disabled', true);
        } else {
            var index = Data.RowSelectedCheckNext.findIndex(function (data) {
                return data.SONumber == Row.SONumber;
            });
            Data.RowSelectedCheckNext.splice(index, 1);

            $(this).closest('tr').find('.checknotcomplete').prop('disabled', false);
            $(this).closest('tr').find('.checkcomplete').prop('disabled', false);
        }
    });

    $('#tbSummary').on('change', 'tbody tr .checknotcomplete', function () {
        var id = $(this).parent().attr('id');
        var table = $("#tbSummary").DataTable();

        var DataRows = {};
        var Row = table.row($(this).parents('tr')).data();
        if (this.checked) {
            DataRows.DeptSubmitBAUK = Row.DeptSubmitBAUK;
            DataRows.BaukSubmitBySystem = Row.BaukSubmitBySystem;
            DataRows.SONumber = Row.SONumber;
            DataRows.SiteID = Row.SiteID;
            DataRows.SiteName = Row.SiteName;
            DataRows.Product = Row.Product;
            DataRows.CustomerName = Row.CustomerName;
            Data.RowSelectedNotComplete.push(DataRows);

            $(this).closest('tr').find('.checknext').prop('disabled', true);
            $(this).closest('tr').find('.checkcomplete').prop('disabled', true);
        } else {
            var index = Data.RowSelectedNotComplete.findIndex(function (data) {
                return data.SONumber == Row.SONumber;
            });
            Data.RowSelectedNotComplete.splice(index, 1);

            $(this).closest('tr').find('.checknext').prop('disabled', false);
            $(this).closest('tr').find('.checkcomplete').prop('disabled', false);
        }
    });

    $('#tbSummary').on('change', 'tbody tr .checkcomplete', function () {
        var table = $("#tbSummary").DataTable();

        var DataRows = {};
        var Row = table.row($(this).parents('tr')).data();
        if (this.checked) {
            DataRows.DeptSubmitBAUK = Row.DeptSubmitBAUK;
            DataRows.BaukSubmitBySystem = Row.BaukSubmitBySystem;
            DataRows.SONumber = Row.SONumber;
            DataRows.SiteID = Row.SiteID;
            DataRows.SiteName = Row.SiteName;
            DataRows.Product = Row.Product;
            DataRows.CustomerName = Row.CustomerName;
            Data.RowSelectedComplete.push(DataRows);

            $(this).closest('tr').find('.checknext').prop('disabled', true);
            $(this).closest('tr').find('.checknotcomplete').prop('disabled', true);
        } else {
            var index = Data.RowSelectedComplete.findIndex(function (data) {
                return data.SONumber == Row.SONumber;
            });
            Data.RowSelectedComplete.splice(index, 1);

            $(this).closest('tr').find('.checknext').prop('disabled', false);
            $(this).closest('tr').find('.checknotcomplete').prop('disabled', false);
        }
    });

    $("#btSave").unbind().click(function () {
        Process.AddToList();    
    });

    $("#btCancel").unbind().click(function () {
        var chkAll = $(".group-checkable");
        //By default set to Unchecked.
        chkAll.prop("checked", false);

        var set = $(".panelSearchResult .DTFC_LeftBodyWrapper .DTFC_Cloned .checknext");
        jQuery(set).each(function () {
            label = $(this).parent();
            var id = label.attr("id");
            if (label.attr("style") != "display:none") {
                $(this).prop("checked", false);
                $(this).parents('tr').removeClass("active");
                $(this).trigger("change");

                $(".Row" + id).removeClass("active");
                $("." + id).prop("checked", false);
                $("." + id).trigger("change");               
            }
        });

        var set1 = $(".panelSearchResult .DTFC_LeftBodyWrapper .DTFC_Cloned .checknotcomplete");
        jQuery(set1).each(function () {
            label = $(this).parent();
            var id = label.attr("id");
            if (label.attr("style") != "display:none") {
                $(this).prop("checked", false);
                $(this).parents('tr').removeClass("active");
                $(this).trigger("change");

                $(".Row" + id).removeClass("active");
                $("." + id).prop("checked", false);
                $("." + id).trigger("change");
            }
        });

        var set2 = $(".panelSearchResult .DTFC_LeftBodyWrapper .DTFC_Cloned .checkcomplete");
        jQuery(set2).each(function () {
            label = $(this).parent();
            var id = label.attr("id");
            if (label.attr("style") != "display:none") {
                $(this).prop("checked", false);
                $(this).parents('tr').removeClass("active");
                $(this).trigger("change");

                $(".Row" + id).removeClass("active");
                $("." + id).prop("checked", false);
                $("." + id).trigger("change");
            }
        });

        Form.ClearRowSelected();
    });

    $("#btYesSubmit").unbind().click(function () {
        Process.SaveReceiveDocBulky();
        $('#mdlDetailCheckList').modal('hide');
        Table.Init();
        Table.ReceiveDocumentBAUK();
    });

    $("#btDismiss").unbind().click(function () {
        $('#mdlDetailCheckList').modal('hide');
        Form.ClearRowSelected();
    });

    //$("#slSearchStatus").change(function () {

        
        
    //});
});

var Form = {
    Init: function () {
        $('#tbStartSubmitDate').datepicker();
        $('#tbEndSubmitDate').datepicker();

        Control.BindingDatePicker();
        Control.BindingSelectCompany();
        Control.BindingSelectOperator();
        Control.BindingSelectProductType();
        Control.BindingSelectStip();

        // checkbox check next
        Data.RowSelectedCheckNext = [];
        // checkbox not complete
        Data.RowSelectedNotComplete = [];
        // checkbox complete
        Data.RowSelectedComplete = [];
    },
    Reset: function () {
        $("#slSearchCompanyID").val("").trigger('change');
        $("#slSearchCustomerID").val("").trigger('change');
        $("#slSearchProductID").val("").trigger('change');
        $("#slStipCategory").val("").trigger('change');

        $('#tbStartSubmitDate').val("");
        $('#tbEndSubmitDate').val("");

        Control.BindingDatePicker();
    },
    ClearRowSelected: function () {
        Data.RowSelectedCheckNext = [];
        Data.RowSelectedNotComplete = [];
        Data.RowSelectedComplete = [];
    }
}

var Table = {
    Init: function () {
        var tblSummary = $('#tbSummary').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tbSummary").DataTable().columns.adjust().draw();
        });
    },
    ReceiveDocumentBAUK: function () {
        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();
        var params = {
            vSONumber: $('#sSONumber').val(),
            vSiteID: $('#sSiteID').val(),
            vSiteName: $('#sSiteName').val(),
            vStartSubmit: $("#tbStartSubmitDate").val() == "" ? null : $("#tbStartSubmitDate").val(),
            vEndSubmit: $("#tbEndSubmitDate").val() == "" ? null : $("#tbEndSubmitDate").val(),
            vCustomerID: $('#slSearchCustomerID').val(),
            vCompanyID: $('#slSearchCompanyID').val(),
            vStatusDoc: $('#slSearchStatus').val(),
            vProductID: $('#slSearchProductID').val(),
            vStip: $('#slStipCategory').val(),
        };
        var tblProcess = $("#tbSummary").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/ReceiveDoc/summary",
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
                        Table.ExportReceiveDocumentBAUK()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 100], ['10', '25', '100']],
            "destroy": true,
            "columns": [
                { className: "text-center", data: "RowIndex" },
                {
                    orderable: false, className: "text-center",
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Helper.IsElementExistsInArray(parseInt(full.SONumber), Data.RowSelectedComplete)) {
                                strReturn += "<label id='" + full.SONumber + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.SONumber + "'><input type='checkbox' checked class='checkcomplete' /><span></span></label>";
                            }
                            else {
                                strReturn += "<label id='" + full.SONumber + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.SONumber + "'><input type='checkbox' class='checkcomplete' /><span></span></label>";
                            }
                            return strReturn;
                        }
                    }
                },
                {
                    orderable: false, className: "text-center",
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Helper.IsElementExistsInArray(parseInt(full.SONumber), Data.RowSelectedNotComplete)) {
                                strReturn += "<label id='" + full.SONumber + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.SONumber + "'><input type='checkbox' checked class='checknotcomplete' /><span></span></label>";
                            }
                            else {
                                strReturn += "<label id='" + full.SONumber + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.SONumber + "'><input type='checkbox' class='checknotcomplete' /><span></span></label>";
                            }
                            return strReturn;
                        }
                    }
                },
                {
                    orderable: false, className: "text-center",
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Helper.IsElementExistsInArray(parseInt(full.SONumber), Data.RowSelectedCheckNext)) {
                                strReturn += "<label id='" + full.SONumber + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.SONumber + "'><input type='checkbox' checked class='checknext' /><span></span></label>";
                            }
                            else {
                                strReturn += "<label id='" + full.SONumber + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.SONumber + "'><input type='checkbox' class='checknext' /><span></span></label>";
                            }
                            return strReturn;
                        }
                    }
                },
                { data: "DeptSubmitBAUK" },
                {
                    data: "BaukSubmitBySystem", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "CustomerID" },
                { data: "CustomerName" },
                { data: "Product" },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "StatusDoc" },
                {
                    mRender: function (data, type, full) {
                        return "<a href='' class='btHistory' data-toggle='modal' data-target='#mdlHistory'>History</a>";
                    }
                }
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblProcess.data())) {
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelectedCheckNext.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelectedCheckNext.length; i++) {
                        item = Data.RowSelectedCheckNext[i];
                        $("#Row" + item).addClass("active");
                    }
                }

                if (Data.RowSelectedNotComplete.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelectedNotComplete.length; i++) {
                        item = Data.RowSelectedNotComplete[i];
                        $("#Row" + item).addClass("active");
                    }
                }

                if (Data.RowSelectedComplete.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelectedComplete.length; i++) {
                        item = Data.RowSelectedComplete[i];
                        $("#Row" + item).addClass("active");
                    }
                }

                if ($('#slSearchStatus').val() == 'Complete') {
                    var chkAll = $(".group-checkable");
                    chkAll.prop("disabled", true);

                    var chkNext = $(".checknext");
                    chkNext.prop("disabled", true);

                    var chkNotComplete = $(".checknotcomplete");
                    chkNotComplete.prop("disabled", true);

                    var chkComplete = $(".checkcomplete");
                    chkComplete.prop("disabled", true);
                } else {
                    var chkAll = $(".group-checkable");
                    chkAll.prop("disabled", false);

                    var chkNext = $(".checknext");
                    chkNext.prop("disabled", false);

                    var chkNotComplete = $(".checknotcomplete");
                    chkNotComplete.prop("disabled", false);

                    var chkComplete = $(".checkcomplete");
                    chkComplete.prop("disabled", false);
                }
            },
            "order": [],
            "scrollY": 300, /* Enable vertical scroll to allow fixed columns */
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 4 /* Set the 2 most left columns as fixed columns */
            },
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                    '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tbSummary .checknext" />' +
                    '<span></span> ' +
                    '</label> CheckAll';
                var th = $("th.select-all").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all").unbind().on("change", ".group-checkable", function (e) {
                    var set = $(".panelSearchResult .DTFC_LeftBodyWrapper .DTFC_Cloned .checknext");
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
                });

                var uncheckbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                    '<input type="checkbox" name="UncheckAll" class="group-checkable" />' +
                    '<span></span> ' +
                    '</label> UncheckAll';
                var th = $("th.uncheck-all").html(uncheckbox);

                /* Bind Event to Uncheck All Checkbox in the Cloned Table */
                $("th.uncheck-all").unbind().on("change", ".group-checkable", function (e) {
                    var set1 = $(".panelSearchResult .DTFC_LeftBodyWrapper .DTFC_Cloned .checknext");
                    var checked1 = jQuery(this).is(":checked");
                    jQuery(set1).each(function () {
                        label = $(this).parent();
                        var id = label.attr("id");
                        if (label.attr("style") != "display:none") {
                            if (checked1) {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");
                                $(this).trigger("change");

                                $(".Row" + id).removeClass("active");
                                $("." + id).prop("checked", false);
                                $("." + id).trigger("change");
                            } 
                        }
                    });

                    var set2 = $(".panelSearchResult .DTFC_LeftBodyWrapper .DTFC_Cloned .checkcomplete");
                    var checked2 = jQuery(this).is(":checked");
                    jQuery(set2).each(function () {
                        label = $(this).parent();
                        var id = label.attr("id");
                        if (label.attr("style") != "display:none") {
                            if (checked2) {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");
                                $(this).trigger("change");

                                $(".Row" + id).removeClass("active");
                                $("." + id).prop("checked", false);
                                $("." + id).trigger("change");
                            }
                        }
                    });

                    var set3 = $(".panelSearchResult .DTFC_LeftBodyWrapper .DTFC_Cloned .checknotcomplete");
                    var checked3 = jQuery(this).is(":checked");
                    jQuery(set3).each(function () {
                        label = $(this).parent();
                        var id = label.attr("id");
                        if (label.attr("style") != "display:none") {
                            if (checked3) {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");
                                $(this).trigger("change");

                                $(".Row" + id).removeClass("active");
                                $("." + id).prop("checked", false);
                                $("." + id).trigger("change");
                            }
                        }
                    });
                });
            }
        });

        $("#tbSummary tbody").on("click", "a.btHistory", function (e) {
            var table = $("#tbSummary").DataTable();
            Data.Selected = {};
            Data.Selected = table.row($(this).parents('tr')).data();

            var params = {
                vSONumber: Data.Selected.SONumber
            };

            Table.History(params);       
        });

    },
    History: function (params) {
        $.ajax({
            url: "/api/ReceiveDoc/history",
            type: "POST",
            datatype: "json",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            var tblHistory = $("#tbHistory").DataTable({
                "deferRender": true,
                "proccessing": true,
                "serverSide": false,
                "language": {
                    "emptyTable": "No data available in table",
                },
                "data": data,
                buttons: [],
                "filter": false,
                "destroy": true,
                "columns": [
                    { className: "text-center", data: "RowIndex" },
                    { data: "SONumber" },
                    { data: "StatusDocument" },
                    { data: "Remarks" },
                    { data: "PICReceive" },
                    {
                        data: "ReceiveDate", render: function (data) {
                            return Common.Format.ConvertJSONDateTime(data);
                        }
                    },
                ],
                "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                'scrollCollapse': true,
                'order': [],
                //"scrollY": 300,
                //"scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            });
        })
        .fail(function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
        });

        $(window).resize(function () {
            $("#tbHistory").DataTable().columns.adjust().draw();
        });
    },
    CheckListDetail: function () {
        var tbProcessDetail = $("#tbProcessDetail").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "data": AjaxData,
            "filter": false,
            "destroy": true,
            "columns": [
                {
                    "data": "id",
                    render: function (data, type, row, meta) {
                        return meta.row + meta.settings._iDisplayStart + 1;
                    }
                },
                {
                    data: "ActionConfirmation",
                    mRender: function (data, type, full) {
                        var color = 'black';
                        if (full.ActionConfirmation == "Complete") {
                            color = 'green';
                        }
                        if (full.ActionConfirmation == "Not Complete") {
                            color = 'red';
                        }
                        if (full.ActionConfirmation == "Check Next") {
                            color = 'blue';
                        }
                        return '<span style="color:' + color + '">' + data + '</span>';
                    }
                },
                {
                    orderable: false, mRender: function (data, type, full) {
                        var strReturn = "";
                        if (full.ActionConfirmation == "Not Complete") {
                            strReturn += "<div class='row'>";
                            strReturn += "<div class='col-md-12'><input class='form-control' placeholder='Remarks' id='txtRemark" + full.SONumber + "'></div>";
                        }
                        strReturn += "</div>";
                        return strReturn;
                    }
                },
                {
                    data: "BaukSubmitBySystem", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerName" },
            ],
            "buttons": [],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "scrollY": 300,
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true
        });

        //RowNumber No.
        //tbProcessDetail.on('draw.dt', function () {
        //    var PageInfo = $('#tbProcessDetail').DataTable().page.info();
        //    tbProcessDetail.column(0, { page: 'current' }).nodes().each(function (cell, i) {
        //        cell.innerHTML = i + 1 + PageInfo.start;
        //    });
        //});

        $(window).resize(function () {
            $("#tbProcessDetail").DataTable().columns.adjust().draw();
        });
    },
    GetSelectedCheckComplete: function (list) {
        var params = {
            strSONumber: list,
            vAction: "Complete"
        };
        var l = Ladda.create(document.querySelector("#btSave"));
        $.ajax({
            url: "/api/ReceiveDoc/CheckList",
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
                $.each(data, function (index, item) {
                    AjaxData.push(item);
                });
                console.log(data);
                console.log(AjaxData);
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
    ExportReceiveDocumentBAUK: function () {
        window.location.href = "/RevenueAssurance/ReceiveDocumentBAUK/Export?" + $.param(param);
    }
}

var Process = {
    AddToList: function () {
        if (Data.RowSelectedCheckNext.length == 0 && Data.RowSelectedNotComplete.length == 0 && Data.RowSelectedComplete.length == 0) {
            Common.Alert.Warning("Please Select One or More Data")
        } else {
            var countCheckNext = Data.RowSelectedCheckNext.length;
            $('#lblNextCheckList').text(countCheckNext);

            var countNotComplete = Data.RowSelectedNotComplete.length;
            $('#lblNotCompleteCheckList').text(countNotComplete);

            var countComplete = Data.RowSelectedComplete.length;
            $('#lblCompleteCheckList').text(countComplete);

            var countTotal = countCheckNext + countNotComplete + countComplete;
            $('#lblTotalCheckList').text(countTotal);

            Control.SelectDataCheckNext(Data.RowSelectedCheckNext);
            Control.SelectDataNotComplete(Data.RowSelectedNotComplete);
            Control.SelectDataComplete(Data.RowSelectedComplete);

            var tbProcessDetail = $('#tbProcessDetail').dataTable({
                "filter": false,
                "destroy": true,
                "data": []
            });

            $('#mdlDetailCheckList').modal('toggle');
            Table.CheckListDetail();
            Control.BindingPIC();
            Control.BindingGetDate();
        }
    },
    SaveReceiveDocBulky: function () {        
        var l = Ladda.create(document.querySelector("#btSearch"));
        ReceiveDocument = [];
        for (var i = 0; i < AjaxData.length; i++) {
            DataRows = {};
            DataRows.BaukSubmitBySystem = AjaxData[i].BaukSubmitBySystem;
            DataRows.SONumber = AjaxData[i].SONumber;
            DataRows.SiteID = AjaxData[i].SiteID;
            DataRows.SiteName = AjaxData[i].SiteName;
            DataRows.CustomerName = AjaxData[i].CustomerName;
            DataRows.Remarks = $('#txtRemark' + AjaxData[i].SONumber).val();
            DataRows.PICReceive = $('#slPICReceiveDocument').val();
            DataRows.ReceiveDate = $('#tbGetDate').val();
            DataRows.StatusDocument = AjaxData[i].ActionConfirmation;
            ReceiveDocument.push(DataRows);
        }
        //console.log(Data.RowSelected);        
        var params = {
            ListTrxReceive: ReceiveDocument
        };

        $.ajax({
            url: "/api/ReceiveDoc/SaveReceiveDocBulky",
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
                Common.Alert.Successhtml("Data Success Save");
                //Table.Init();
                //Table.ReceiveDocumentBAUK();
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            //$(".panelSiteData").fadeOut();
            Form.Init();
        })
    }
}

var Control = {
    BindingDatePicker: function () {
        $(".datepicker").datepicker({
            format: "dd M yyyy"
        });
    },
    BindingSelectCompany: function () {
        var id = "#slSearchCompanyID"
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Company Name", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectOperator: function () {
        var id = "#slSearchCustomerID";
        $.ajax({
            url: "/api/ReceiveDoc/CustomerReceiveBAUK",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.OperatorId + "'>" + item.Operator + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Operator", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectProductType: function () {
        var id = "#slSearchProductID";
        $.ajax({
            url: "/api/MstDataSource/TenantType",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Product", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });

    },
    BindingSelectStip: function () {
        var id = "#slStipCategory";
        $.ajax({
            url: "/api/StartBaps/getListStipCategory",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>");
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + $.trim(item.STIPID) + "'>" + item.STIPCode + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select STIP", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingPIC: function () {
        $.ajax({
            url: "/api/ReceiveDoc/PICReceiveDoc",
            type: "GET"
        })
          .done(function (data, textStatus, jqXHR) {
              $("#slPICReceiveDocument").html("<option></option>")

              if (Common.CheckError.List(data)) {
                  $.each(data, function (i, item) {
                      $("#slPICReceiveDocument").append("<option value='" + item.UserID + "'>" + item.FullName + "</option>");
                  })
              }

              $("#slPICReceiveDocument").select2({ placeholder: "Select PIC Serah Terima", width: null });

          })
          .fail(function (jqXHR, textStatus, errorThrown) {
              Common.Alert.Error(errorThrown);
          });
    },
    BindingGetDate: function () {
        $('#tbGetDate').datepicker({ dateFormat: 'dd M yyyy' });
        $('#tbGetDate').datepicker('setDate', new Date());
        //$('#tbGetDate').prop('disabled', true);
    },
    SelectDataCheckNext: function (list){
        for (var i = 0; i < list.length; i++) {
            var DataRows = {}
            DataRows.ActionConfirmation = "Check Next";
            DataRows.DeptSubmitBAUK = list[i].DeptSubmitBAUK;
            DataRows.BaukSubmitBySystem = list[i].BaukSubmitBySystem;
            DataRows.SONumber = list[i].SONumber;
            DataRows.SiteID = list[i].SiteID;
            DataRows.SiteName = list[i].SiteName;
            DataRows.Product = list[i].Product;
            DataRows.CustomerName = list[i].CustomerName;
            AjaxData.push(DataRows);
        }
        Data.RowSelectedCheckNext = {}
    },
    SelectDataNotComplete: function (list){
        for (var i = 0; i < list.length; i++) {
            var DataRows = {}
            DataRows.ActionConfirmation = "Not Complete";
            DataRows.DeptSubmitBAUK = list[i].DeptSubmitBAUK;
            DataRows.BaukSubmitBySystem = list[i].BaukSubmitBySystem;
            DataRows.SONumber = list[i].SONumber;
            DataRows.SiteID = list[i].SiteID;
            DataRows.SiteName = list[i].SiteName;
            DataRows.Product = list[i].Product;
            DataRows.CustomerName = list[i].CustomerName;
            AjaxData.push(DataRows);
        }
        Data.RowSelectedNotComplete = {}
    },
    SelectDataComplete: function (list) {
        for (var i = 0; i < list.length; i++) {
            var DataRows = {}
            DataRows.ActionConfirmation = "Complete";
            DataRows.DeptSubmitBAUK = list[i].DeptSubmitBAUK;
            DataRows.BaukSubmitBySystem = list[i].BaukSubmitBySystem;
            DataRows.SONumber = list[i].SONumber;
            DataRows.SiteID = list[i].SiteID;
            DataRows.SiteName = list[i].SiteName;
            DataRows.Product = list[i].Product;
            DataRows.CustomerName = list[i].CustomerName;
            AjaxData.push(DataRows);
        }
        Data.RowSelectedComplete = {}
    },
    SetParam: function () {
        var vSONumber = $('#sSONumber').val();
        var vSiteID = $('#sSiteID').val();
        var vSiteName = $('#sSiteName').val();
        var vStartSubmit = $("#tbStartSubmitDate").val() == "" ? null : $("#tbStartSubmitDate").val();
        var vEndSubmit = $("#tbEndSubmitDate").val() == "" ? null : $("#tbEndSubmitDate").val();
        var vCustomerID = $('#slSearchCustomerID').val();
        var vCompanyID = $('#slSearchCompanyID').val();
        var vStatusDoc = $('#slSearchStatus').val();
        var vProductID = $('#slSearchProductID').val();
        var vStip = $('#slStipCategory').val();

        param = {
            vSONumber: vSONumber,
            vSiteID: vSiteID,
            vSiteName: vSiteName,
            vStartSubmit: vStartSubmit,
            vEndSubmit: vEndSubmit,
            vCustomerID: vCustomerID,
            vCompanyID: vCompanyID,
            vStatusDoc: vStatusDoc,
            vProductID: vProductID,
            vStip: vStip,
        };
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
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == value) {
                isExist = true;
                break;
            }
        }
        return isExist;
    },
}
