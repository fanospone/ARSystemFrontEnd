Data = {};
Data.RowSelectedRaw = [];
RowSelected = {};
TempData = [];
AllDataId = [];
CheckAllDataFlag = false;

fsSONumber = null;
fsAccrueStatusID = null;
fsCustomerID = null;
fsCompanyID = null;
fsSOW = null;
fsRegionalID = null;
fsSiteName = null;

jQuery(document).ready(function () {
    Form.Init();

    $("#btReset").unbind().click(function () {
        Table.Reset();
    });
    $("#btCancel").unbind().click(function () {
        $(".panelSearchResult").hide();
        $(".panelSearchZero").hide();
        $(".panelSiteData").hide();
    });
    $("#btCancelAdd").unbind().click(function () {
        Form.CancelAdd();
    });
    $("#btSearch").unbind().click(function () {
        $(".panelSiteData").hide();
        AllDataId = [];
        CheckAllDataFlag = false;
        TempData = [];
        
        Table.Search();

    });
    $("#btAdd").unbind().click(function () {
        if (TempData.length > 0) {
            $(".panelSiteData").show();
            if (CheckAllDataFlag) {
                AllDataId = Helper.GetListId();
                Table.AddListSearchAll();
            }
            else
                Table.AddListSearch();
        }
        else
            Common.Alert.Warning("Please Select One or More Data")

    });
    $("#btSubmit").unbind().click(function () {
        if (TempData.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            Action.Submit();
        }

    });


    $("#tblSummaryData").on('change', 'tbody tr .checkboxes', function () {
        var idTemp = $(this).parent().attr('id');
        var table = $("#tblSummaryData").DataTable();
        var idComponents = idTemp.split('cb_');
        var id = idComponents[1];
        var DataRows = {};
        var Row = table.row($(this).parents('tr')).data();

        if (this.checked) {
            if (Control.IsEmptyData(id)) {
                DataRows.SONumber = Row.SONumber;
                DataRows.SiteID = Row.SiteID;
                DataRows.SiteName = Row.SiteName;
                DataRows.CustomerSiteID = Row.CustomerSiteID;
                DataRows.StatusMasterList = Row.StatusMasterList;
                DataRows.CompanyID = Row.CompanyID;
                DataRows.CompanyInvoiceID = Row.CompanyInvoiceID;
                DataRows.CustomerName = Row.CustomerName;
                DataRows.RFIDate = Row.RFIDate;
                DataRows.SldDate = Row.SldDate;
                DataRows.BAPSDate = Row.BAPSDate;
                DataRows.StartBapsDate = Row.StartBapsDate;
                DataRows.EndBapsDate = Row.EndBapsDate;
                DataRows.TenantType = Row.TenantType;
                DataRows.BaseLeasePrice = Row.BaseLeasePrice;
                DataRows.ServicePrice = Row.ServicePrice;
                DataRows.AmountTotal = Row.AmountTotal;
                DataRows.Currency = Row.Currency;
                DataRows.Accrue = Row.Accrue;
                DataRows.Unearned = Row.Unearned;
                DataRows.AccrueUnearned = Row.AccrueUnearned;
                DataRows.MioAccrue = Row.MioAccrue;
                DataRows.Month = Row.Month;
                DataRows.StartDateAccrue = Row.StartDateAccrue;
                DataRows.EndDateAccrue = Row.EndDateAccrue;

                DataRows.D = Row.D;
                DataRows.OD = Row.OD;
                DataRows.ODCATEGORY = Row.ODCATEGORY;
                DataRows.RegionName = Row.RegionName;
                DataRows.RegionID = Row.RegionID;
                DataRows.Type = Row.Type;
                DataRows.SOW = Row.SOW;

                DataRows.StatusAccrue = Row.StatusAccrue;
                DataRows.Remarks = Row.Remarks;
                DataRows.TargetUser = Row.TargetUser;
                DataRows.RootCause = Row.RootCause;
                DataRows.PICADetail = Row.PICADetail;
                DataRows.IDTemp = Row.IDTemp;
                RowSelected = DataRows;
                TempData.push(RowSelected);
            }
        } else {
            var index = TempData.findIndex(function (data) {
                return data.IDTemp == Row.IDTemp;
            });
            TempData.splice(index, 1);
        }
        Table.AddListSearch();

    });



});

var Form = {
    Init: function () {
        $(".panelSearchResult").hide();
        $(".panelSearchZero").hide();
        $(".panelSiteData").hide();
        if (!$("#hdAllowProcess").val()) {
            $("#btAdd").hide();
            $("#btSubmit").hide();
        }
        Control.BindingSelectCompany();
        Control.BindingSelectCustomer();
        Control.BindingSelectRegion();
        Control.BindingSelectStatus();
        Control.BindingSelectDept();
        $('#cbALLData input[type=checkbox]').prop('checked', false);
        $("body").delegate(".datepicker", "focusin", function () {
            $(this).datepicker({
                format: "dd MM yyyy",

            });
        });
    },

    CancelAdd: function () {
        $(".panelSiteData").hide();
    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $('#mdlDetail').modal('toggle');
        Table.Search();
        //Table.Reset();
    }
}

var Action = {
    Submit: function () {
        if (TempData.length > 0) {
            var requestData = [];
            var paramAll = "";
            param = {};
            
            if (CheckAllDataFlag) {
                //requestData = AllDataId;
                paramAll = "Not Yet";
                param.SONumber = fsSONumber;
                param.SiteName = fsSiteName;
                param.CustomerID = fsCustomerID;
                param.CompanyID = fsCompanyID;
                param.RegionID = fsRegionalID;
                param.SOW = fsSOW;
            }

            else {
                for (var i = 0; i < TempData.length; i++) {
                    requestData.push(TempData[i].IDTemp);
                }
            }
            var params = {
                ListID: requestData,
                paramAllData: paramAll,
                vwAccrueList: param
            };
            var l = Ladda.create(document.querySelector("#btSubmit"))
            $.ajax({
                url: "/api/ListDataAccrue/SubmitDataAccrue",
                type: "post",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false,
                beforeSend: function (xhr) {
                    l.start();
                }
            }).done(function (data, textStatus, jqXHR) {
                if (Common.CheckError.List(data)) {
                    if (data.ErrorType != 0) {
                        Common.Alert.Warning(data.ErrorMessage);
                    }
                    else {
                        Common.Alert.Success("Data has been saved!")
                        l.stop();
                        TempData = [];
                        RowSelected = {};
                        $(".panelSiteData").hide();
                        $(".panelSearchResult").hide();
                    //Table.Search();
                    }
                   
                }
                else {
                    Common.Alert.Error(data.ErrorMessage);
                }


            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l.stop()
            })
        } else {
            Common.Alert.Warning("Request details cannot be empty");
        }
    },
}
var Table = {
    Init: function (idTable) {

        var tblInit = $(idTable).dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $(idTable).DataTable().columns.adjust().draw();
        });

    },
    Search: function () {
        Table.Init("#tblSummaryData");
        TempData = [];
        AllDataId = [];
        CheckAllDataFlag = false;
        fsSONumber = $('#tbSearchSONumber').val();
        fsCustomerID = $('#slSearchCustomer').val();
        fsCompanyID = $('#slSearchCompany').val();
        fsSOW = $('#slSearchDept').val();
        fsRegionalID = $('#slSearchRegional').val();
        fsSiteName = $('#tbSearchSiteName').val();

        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        param = {};
        param.SONumber = fsSONumber;
        param.SiteName = fsSiteName;       
        param.CustomerID = fsCustomerID;
        param.CompanyID = fsCompanyID;
        param.RegionID = fsRegionalID;
        param.SOW = fsSOW;
         //param.StatusAccrue = $('#slSearchStatus option:selected').html();
        //param.EndDatePeriod = $('#tbSearchEndPeriode').val();
        var params = {
            vwAccrueList: param
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            //"deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/ListDataAccrue/grid",
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
                        var strResult = "";
                        if (full.StatusAccrue == "Not Yet")
                            if (CheckAllDataFlag)
                                strResult += "<label id='cb_" + full.IDTemp + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input id='" + full.IDTemp + "' type='checkbox' checked class='checkboxes'/><span></span></label>";
                            else
                                strResult += "<label id='cb_" + full.IDTemp + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input id='" + full.IDTemp + "' type='checkbox' class='checkboxes'/><span></span></label>";
                        else
                            strResult += "<label id='" + full.IDTemp + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline' style='display:none'><input id='cb_" + full.IDTemp + "' type='checkbox' class='checkboxes' style='display:none'/><span></span></label>";

                        return strResult;
                    }
                },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "StatusMasterList" },

                { data: "CompanyID" },
                { data: "CompanyInvoiceID" },
                { data: "CustomerName" },

                {
                    data: "RFIDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "SldDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "BAPSDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                 {
                     data: "StartBapsDate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                  {
                      data: "EndBapsDate", render: function (data) {
                          return Common.Format.ConvertJSONDateTime(data);
                      }
                  },
                   { data: "TenantType" },
                   { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "AmountTotal", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "Currency" },
                    { data: "Accrue" },
                    { data: "Unearned" },
                    { data: "AccrueUnearned" },
                     { data: "MioAccrue" },
                      { data: "Month" },
                   {
                       data: "StartDateAccrue", render: function (data) {
                           return Common.Format.ConvertJSONDateTime(data);
                       }
                   },
                    {
                        data: "EndDateAccrue", render: function (data) {
                            return Common.Format.ConvertJSONDateTime(data);
                        }
                    },
                    { data: "D" },
                    { data: "OD" },
                    { data: "ODCATEGORY" },
            { data: "RegionName" },
                { data: "Type" },
                 { data: "SOW" },
                { data: "StatusAccrue" },

                { data: "Remarks" },
                { data: "TargetUser" },
                { data: "RootCause" },
                { data: "PICADetail" }


            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop();
                if (CheckAllDataFlag) {
                    var item;
                    $('#cbALLData input[type=checkbox]').prop('checked', true);

                }
                else {
                    $('#cbALLData input[type=checkbox]').prop('checked', false);
                    Control.SelectData();
                }
            },
            "order": [],
            "scrollY": 300, /* Enable vertical scroll to allow fixed columns */
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label id="cbALLData" class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
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
                                if (Control.IsEmptyData(id)) {
                                    $(this).prop("checked", true);
                                    $(this).parents('tr').addClass('active');
                                    $(this).trigger("change");

                                    $(".Row" + id).addClass("active");
                                    $("." + id).prop("checked", true);
                                    $("." + id).trigger("change");
                                }
                            } else {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");
                                $(this).trigger("change");

                                $(".Row" + id).removeClass("active");
                                $("." + id).prop("checked", false);
                                $("." + id).trigger("change");

                                Control.DeleteRequestDetail(id);
                            }
                        }
                    });
                    if (checked) {
                        //AllDataId = Helper.GetListId();
                        CheckAllDataFlag = true;
                    }
                    else {
                        CheckAllDataFlag = false;
                        AllDataId = [];
                    }
                });
            }
        });


    },

    SearchClientBackUp: function () {
        Table.Init("#tblSummaryData");
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        param = {};
        param.SONumber = $('#tbSearchSONumber').val();
        param.SiteName = $('#tbSearchSiteName').val();
        param.StatusAccrue = "Not Yet";
        param.EndDatePeriod = $('#tbSearchEndPeriode').val();
        param.CustomerID = $('#slSearchCustomer').val();
        param.CompanyID = $('#slSearchCompany').val();
        param.RegionID = $('#slSearchRegional').val();
        param.SOW = $('#slSearchDept').val();
        var params = {
            vwAccrueList: param
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "filter": false,
            "destroy": true,
            "async": false,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/ListDataAccrue/grid",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },

            "columns": [{
                orderable: false,
                mRender: function (data, type, full) {
                    var strResult = "";
                    if (full.StatusAccrue == "Not Yet")
                        if (CheckAllDataFlag)
                            strResult += "<label id='cb_" + full.IDTemp + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input id='" + full.IDTemp + "' type='checkbox' checked class='checkboxes'/><span></span></label>";
                        else
                            strResult += "<label id='cb_" + full.IDTemp + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input id='" + full.IDTemp + "' type='checkbox' class='checkboxes'/><span></span></label>";
                    else
                        strResult += "<label id='" + full.IDTemp + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline' style='display:none'><input id='cb_" + full.IDTemp + "' type='checkbox' class='checkboxes' style='display:none'/><span></span></label>";

                    return strResult;
                }
            },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "StatusMasterList" },

                { data: "CompanyID" },
                { data: "CompanyInvoiceID" },
                { data: "CustomerName" },

                {
                    data: "RFIDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "SldDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "BAPSDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                 {
                     data: "StartBapsDate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                  {
                      data: "EndBapsDate", render: function (data) {
                          return Common.Format.ConvertJSONDateTime(data);
                      }
                  },
                   { data: "TenantType" },
                   { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "AmountTotal", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "Currency" },
                    { data: "Accrue" },
                    { data: "Unearned" },
                    { data: "AccrueUnearned" },
                     { data: "MioAccrue" },
                      { data: "Month" },
                   {
                       data: "StartDateAccrue", render: function (data) {
                           return Common.Format.ConvertJSONDateTime(data);
                       }
                   },
                    {
                        data: "EndDateAccrue", render: function (data) {
                            return Common.Format.ConvertJSONDateTime(data);
                        }
                    },
                    { data: "D" },
                    { data: "OD" },
                    { data: "ODCATEGORY" },
            { data: "RegionName" },
                { data: "Type" },
                 { data: "SOW" },
                { data: "StatusAccrue" },
                { data: "Remarks" },
                { data: "TargetUser" },
                { data: "RootCause" },
                { data: "PICADetail" }
            ],

            "columnDefs": [{ "targets": [0], "orderable": false }],
            "fnDrawCallback": function () {
                $(".panelSearchBegin").hide();
                $(".panelSearchResult").fadeIn();
                l.stop();
                if (CheckAllDataFlag) {
                    var item;
                    $('#cbALLData input[type=checkbox]').prop('checked', true);

                }
                else {
                    $('#cbALLData input[type=checkbox]').prop('checked', false);
                    Control.SelectData();
                }
            },
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
            "scrollY": 400, /* Enable vertical scroll to allow fixed columns */
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label id="cbALLData" class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
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
                                if (Control.IsEmptyData(id)) {
                                    $(this).prop("checked", true);
                                    $(this).parents('tr').addClass('active');
                                    $(this).trigger("change");

                                    $(".Row" + id).addClass("active");
                                    $("." + id).prop("checked", true);
                                    $("." + id).trigger("change");
                                }
                            } else {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");
                                $(this).trigger("change");

                                $(".Row" + id).removeClass("active");
                                $("." + id).prop("checked", false);
                                $("." + id).trigger("change");

                                Control.DeleteRequestDetail(id);
                            }
                        }
                    });
                    if (checked) {
                        AllDataId = Helper.GetListId();
                        CheckAllDataFlag = true;
                    }
                    else {
                        CheckAllDataFlag = false;
                        AllDataId = [];
                    }
                });
            }
        });
        $("#tblSummaryData").unbind();


    },
    AddListSearchAll: function () {
        Table.Init("#tblAddData");
        var l = Ladda.create(document.querySelector("#btAdd"))
        l.start();
        //AllDataId = Helper.GetListId();
        param = {};
        param.SONumber = fsSONumber;
        param.SiteName = fsSiteName; 
        param.StatusAccrue = "Not Yet";
        //param.EndDatePeriod = $('#tbSearchEndPeriode').val();
        param.CustomerID = fsCustomerID;
        param.CompanyID = fsCompanyID;
        param.RegionID = fsRegionalID;
        param.SOW = fsSOW;
        var params = {
            vwAccrueList: param
        };
        var tblSummaryData = $("#tblAddData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "filter": false,
            "destroy": true,
            "async": false,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/ListDataAccrue/grid",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },

            "columns": [{
                orderable: false,
                mRender: function (data, type, full) {
                    return "<i class='fa fa-remove btn btn-xs red deleteRow'></i>";
                }
            },
           { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "StatusMasterList" },

                { data: "CompanyID" },
                { data: "CompanyInvoiceID" },
                { data: "CustomerName" },
                {
                    data: "RFIDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "SldDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "BAPSDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                 {
                     data: "StartBapsDate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                  {
                      data: "EndBapsDate", render: function (data) {
                          return Common.Format.ConvertJSONDateTime(data);
                      }
                  },
                   { data: "TenantType" },
                   { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "AmountTotal", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "Currency" },
                    { data: "Accrue" },
                    { data: "Unearned" },
                    { data: "AccrueUnearned" },
                     { data: "MioAccrue" },
                      { data: "Month" },
                   {
                       data: "StartDateAccrue", render: function (data) {
                           return Common.Format.ConvertJSONDateTime(data);
                       }
                   },
                    {
                        data: "EndDateAccrue", render: function (data) {
                            return Common.Format.ConvertJSONDateTime(data);
                        }
                    },
                    { data: "D" },
                    { data: "OD" },
                    { data: "ODCATEGORY" },
            { data: "RegionName" },
                { data: "Type" },
                 { data: "SOW" },
                { data: "StatusAccrue" },
                { data: "Remarks" },
                { data: "TargetUser" },
                { data: "RootCause" },
                { data: "PICADetail" }
            ],

            "columnDefs": [{ "targets": [0], "orderable": false }],
            "fnDrawCallback": function () {
                l.stop();
            },
        });
        $("#tblAddData").unbind();
        $("#tblAddData").on("click", ".deleteRow", function (e) {
            var table = $("#tblAddData").DataTable();
            var row = table.row($(this).parents('tr')).data();
            table.row($(this).parents('tr')).remove().draw(false);
            Control.DeleteRequestDetailAll(row.IDTemp);
        });

    },
    AddListSearch: function () {
        Table.Init("#tblAddData");
        $("#tblAddData").DataTable({
            "serverSide": false,
            "filter": false,
            "destroy": true,
            "async": false,
            "data": TempData,
            "columns": [{
                orderable: false,
                mRender: function (data, type, full) {
                    return "<i class='fa fa-remove btn btn-xs red deleteRow'></i>";
                }
            },
            { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "StatusMasterList" },

                { data: "CompanyID" },
                { data: "CompanyInvoiceID" },
                { data: "CustomerName" },
                {
                    data: "RFIDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "SldDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "BAPSDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                 {
                     data: "StartBapsDate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                  {
                      data: "EndBapsDate", render: function (data) {
                          return Common.Format.ConvertJSONDateTime(data);
                      }
                  },
                   { data: "TenantType" },
                   { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "AmountTotal", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "Currency" },
                    { data: "Accrue" },
                    { data: "Unearned" },
                    { data: "AccrueUnearned" },
                     { data: "MioAccrue" },
                      { data: "Month" },
                   {
                       data: "StartDateAccrue", render: function (data) {
                           return Common.Format.ConvertJSONDateTime(data);
                       }
                   },
                    {
                        data: "EndDateAccrue", render: function (data) {
                            return Common.Format.ConvertJSONDateTime(data);
                        }
                    },
                    { data: "D" },
                    { data: "OD" },
                    { data: "ODCATEGORY" },
            { data: "RegionName" },
                { data: "Type" },
                 { data: "SOW" },
                { data: "StatusAccrue" },
                { data: "Remarks" },
                { data: "TargetUser" },
                { data: "RootCause" },
                { data: "PICADetail" }
            ],

            "columnDefs": [{ "targets": [0], "orderable": false }],
        });

        $("#tblAddData").unbind();
        $("#tblAddData").on("click", ".deleteRow", function (e) {
            var table = $("#tblAddData").DataTable();
            var row = table.row($(this).parents('tr')).data();
            table.row($(this).parents('tr')).remove().draw(false);
            Control.DeleteRequestDetail(row.IDTemp);
        });
    },
    Export: function () {
        var SONumber = fsSONumber;
        var SiteName = fsSiteName;
        var StatusAccrue = $('#slSearchStatus option:selected').html();
        var EndDatePeriod = $('#tbSearchEndPeriode').val();
        var CustomerID = fsCustomerID;
        var CompanyID = fsCompanyID;
        var RegionID = fsRegionalID;
        var SOW = fsSOW;
        window.location.href = "/Accrue/ListDataAccrue/Export?SONumber=" + SONumber + "&SiteName=" + SiteName
            + "&EndDatePeriod=" + EndDatePeriod + "&CustomerID=" + CustomerID + "&CompanyID=" + CompanyID
        + "&RegionID=" + RegionID + "&StatusAccrue=" + StatusAccrue + "&SOW=" + SOW;
    },
    Reset: function () {
        $("#slSearchCustomer").val("").trigger('change');
        $("#slSearchCompany").val("").trigger('change');
        $("#slSearchRegional").val("").trigger('change');
        $("#slSearchDept").val("").trigger('change');
        $("#slSearchStatus").val("").trigger('change');
        $("#tbSearchPeriode").val("");
        $("#tbSearchSONumber").val("");
        $("#tbSearchSiteName").val("");
    },

}

var Control = {
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/ListDataAccrue/company/list",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchCompany").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchCompany").append("<option value='" + item.CompanyID + "'>" + item.CompanyID +" - "+item.Company + "</option>");
                })
            }

            $("#slSearchCompany").select2({ placeholder: "Select Company", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectDept: function () {
        $.ajax({
            url: "/api/ListDataAccrue/departmentAccrue/list",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchDept").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchDept").append("<option value='" + item.DepartmentName + "'>" + item.DepartmentName + "</option>");
                })
            }

            $("#slSearchDept").select2({ placeholder: "Select Department", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectStatus: function () {

        $.ajax({
            url: "/api/ListDataAccrue/statusListDataAccrue/list",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchStatus").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchStatus").append("<option value='" + item.AccrueStatus + "'>" + item.AccrueStatus + "</option>");
                })
            }

            $("#slSearchStatus").select2({ placeholder: "Select Status", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectCustomer: function () {
        var params = {
            bolIsTelco: null
        };
        $.ajax({
            url: "/api/ListDataAccrue/customer/list",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchCustomer").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchCustomer").append("<option value='" + item.CustomerID + "'>" + item.CustomerName + "</option>");
                })
            }

            $("#slSearchCustomer").select2({ placeholder: "Select Customer", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectRegion: function () {
        $.ajax({
            url: "/api/ListDataAccrue/regional/list",
            type: "GET"
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

    DeleteRequestDetail: function (rowID) {
        $("#cb_" + rowID + " input[type=checkbox]").prop('checked', false);
        //$("#" + rowID + " input[type=checkbox]").removeAttr("checked");
        //$("#" + rowID).show();
        var index = TempData.findIndex(function (data) {
            return data.IDTemp == rowID
        });
        TempData.splice(index, 1);
        if (TempData.length == 0)
            $(".panelSiteData").hide();
    },
    DeleteRequestDetailAll: function (rowID) {
        $("#cb_" + rowID + " input[type=checkbox]").prop('checked', false);
        var index = AllDataId.findIndex(function (data) {
            return data == rowID
        });
        AllDataId.splice(index, 1);
        if (AllDataId.length == 0)
            $(".panelSiteData").hide();
    },
    SelectData: function () {
        if (TempData.length > 0) {
            $.each(TempData, function (i, item) {
                $("#cb_" + item.IDTemp + " input[type=checkbox]").prop('checked', true);
            })
        }
    },

    IsEmptyData: function (ID) {
        var result = true;
        if (TempData.length > 0) {
            for (var i = 0 ; i < TempData.length ; i++) {
                if (TempData[i].IDTemp == ID) {
                    result = false;
                    break;
                }
            }
        }
        return result;
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
    DeleteArrayByName: function (array, param) {
        var index = array.indexOf(param);
        if (index > -1) {
            array.splice(index, 1);
        }
        return array;
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
    GetListId: function () {
        //for CheckAll Pages
        param = {};
        param.SONumber = $('#tbSearchSONumber').val();
        param.SiteName = $('#tbSearchSiteName').val();
        param.StatusAccrue = $('#slSearchStatus option:selected').html();
        param.EndDatePeriod = $('#tbSearchEndPeriode').val();
        param.CustomerID = $('#slSearchCustomer').val();
        param.CompanyID = $('#slSearchCompany').val();
        param.RegionID = $('#slSearchRegional').val();
        var params = {
            vwAccrueList: param
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/ListDataAccrue/GetDataAccrueListId",
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