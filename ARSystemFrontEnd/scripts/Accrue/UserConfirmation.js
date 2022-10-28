Data = {};
RowSelected = {};
PeriodeSearch = "";
WeekSearch = "";

fsSONumber = null;
fsAccrueStatusID = null;
fsCustomerID = null;
fsCompanyID = null;
fsSOW = null;
fsdate = null;
fsweek = null;
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
        TempData = [];
        AllDataId = [];
        CheckAllDataFlag = false;
        if ($('#tbSearchPeriode').val() == "" || $('#slSearchPeriodeWeek').val() == null || $('#slSearchPeriodeWeek').val() == "")
            Common.Alert.Warning("Please Select Periode And Week to Show Data")
        else
            Table.Search();
    });

    $("#btConfirm").unbind().click(function () {
        if (TempData.length > 0) {
            Flag = "Confirm";
            $(".panelSiteData").show();
            $("#lblDetailAdd").html('List Add Confirm Accrue');
            Control.BindingSelectRootCause();
            if (CheckAllDataFlag)
                Table.AddListSearchAll();
            else
                Table.AddListSearch();
            $("#divConfirm").show();
            $("#divMove").hide();
        }
        else
            Common.Alert.Warning("Please Select One or More Data")

    });
    $("#btMove").unbind().click(function () {
        if (TempData.length > 0) {
            Flag = "Move";
            if (CheckAllDataFlag) {
                if (!TypeFlagAll) {
                    Common.Alert.Warning("Please Select Same Type of SOW");
                    return false;
                }
                else
                    Table.AddListSearchAll();
            }
            else {
                for (var i = 0; i < TempData.length; i++) {
                    if (TempData[0].Type != TempData[i].Type) {
                        Common.Alert.Warning("Please Select Same Type of SOW");
                        return false;
                    }
                }
                Table.AddListSearch();
            }
            
            $(".panelSiteData").show();
            $("#lblDetailAdd").html('List Add Move Accrue');
            $("#divConfirm").hide();
            $("#divMove").show();
            Control.BindingSelectDepartment();
            

        }
        else
            Common.Alert.Warning("Please Select One or More Data")

    });
    $("#btSubmit").unbind().click(function () {
        if (TempData.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            if (Flag != "") {
                if (Flag == "Confirm")
                    Action.CheckValidationConfirm();
                else if (Flag == "Move")
                    Action.CheckValidationMove();

            }

        }

    });
    $("#tbSearchPeriode").change(function () {
        if ($("#tbSearchPeriode").val() != "")
            Control.BindingSelectWeekSearch();
    });
    $("#tbPeriode").change(function () {
        if ($("#tbPeriode").val() != "")
            Control.BindingSelectWeek();
    });

    $("#slRootcause").change(function () {
        if ($("#slRootcause").val() != "" || $("#slRootcause").val() != null)
            Control.BindingSelectPICA();
    });

    $("#slPica").change(function () {
        if ($("#slPica").val() != "" || $("#slPica").val() != null)
            Control.BindingSelectPICADetail();
    });

    $("#tblSummaryData").on('change', 'tbody tr .checkboxes', function () {
        var trxDataAccrueID = $(this).parent().attr('id');
        var table = $("#tblSummaryData").DataTable();
        var idComponents = trxDataAccrueID.split('cb_');
        var id = idComponents[1];
        var DataRows = {};
        var Row = table.row($(this).parents('tr')).data();

        if (this.checked) {
            if (Control.IsEmptyData(id)) {
                DataRows.SONumber = Row.SONumber;
                DataRows.SiteID = Row.SiteID;
                DataRows.SiteName = Row.SiteName;
                DataRows.SiteIDOpr = Row.SiteIDOpr;
                DataRows.SiteNameOpr = Row.SiteNameOpr;
                DataRows.Company = Row.Company;
                DataRows.CustomerName = Row.CustomerName;
                DataRows.RegionName = Row.RegionName;
                DataRows.EndDatePeriod = Row.EndDatePeriod;

                DataRows.CompanyID = Row.CompanyID;
                DataRows.TenantType = Row.TenantType;
                DataRows.CompanyInvID = Row.CompanyInvID;
                DataRows.RFIDate = Row.RFIDate;
                DataRows.SldDate = Row.SldDate;
                DataRows.BAPSDate = Row.BAPSDate;
                DataRows.StatusMasterList = Row.StatusMasterList;
                DataRows.MioAccrue = Row.MioAccrue;
                DataRows.Month = Row.Month;
                DataRows.D = Row.D;
                DataRows.OD = Row.OD;
                DataRows.ODCategory = Row.ODCategory;
                DataRows.Currency = Row.Currency;
                DataRows.SOW = Row.SOW;


                DataRows.StartDateBAPS = Row.StartDateBAPS;
                DataRows.EndDateBAPS = Row.EndDateBAPS;
                DataRows.StartDateAccrue = Row.StartDateAccrue;
                DataRows.EndDateAccrue = Row.EndDateAccrue;
                DataRows.TypeSOW = Row.TypeSOW;
                DataRows.Type = Row.Type;
                DataRows.StatusAccrue = Row.StatusAccrue;
                DataRows.BaseLeasePrice = Row.BaseLeasePrice;
                DataRows.ServicePrice = Row.ServicePrice;
                DataRows.TotalAmount = Row.TotalAmount;
                DataRows.Remarks = Row.Remarks;
                DataRows.Week = Row.Week;
                DataRows.trxDataAccrueID = Row.trxDataAccrueID;
                RowSelected = DataRows;
                TempData.push(RowSelected);
            }
        } else {
            var index = TempData.findIndex(function (data) {
                return data.trxDataAccrueID == Row.trxDataAccrueID;
            });
            TempData.splice(index, 1);
            $("#slRootcause").val("").trigger('change');
            $("#slPica").val("").trigger('change');
            $("#slPeriodeWeek").val("").trigger('change');
            $("#slPicaDetail").val("").trigger('change');
            $("#tbPeriode").val("");
            $("#tbRemarksConfirm").val("");
            $("#tbRemarksMove").val("");
            $('#fuConfirm').val('');
            $('#fuMove').val('');
            $(".fileinput-filename").text(''); 
            //$(".fileinput-exists").hide();
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
            $("#btMove").hide();
            $("#btConfirm").hide();
            $("#btCancel").hide();
            $("#btSubmit").hide();
        }
        Control.BindingSelectCompany();
        Control.BindingSelectCustomer();
        Control.BindingSelectDept();
        Control.BindingSelectStatus();
        Control.BindingSelectMonthGetDate();
        Control.BindingSelectWeekSearchGetDate();
        //$("#slSearchPeriodeWeek").select2({ placeholder: "Week", width: null });
        $("#slPeriodeWeek").select2({ placeholder: "Week", width: null });
        $('#cbALLData input[type=checkbox]').prop('checked', false);
        $("body").delegate(".datepicker", "focusin", function () {
            $(this).datepicker({
                format: "M-yyyy",
                viewMode: "months",
                minViewMode: "months"
            });
        });
       
        Helper.GetWeekNowSelected();
        Table.Search();
    },

    CancelAdd: function () {
        $(".panelSiteData").hide();
        $("#divConfirm").hide();
        $("#slRootcause").val("").trigger('change');
        $("#slPica").val("").trigger('change');
        $("#slPeriodeWeek").val("").trigger('change');
        $("#slPicaDetail").val("").trigger('change');
        $("#tbPeriode").val("");
        $("#tbRemarksConfirm").val("");
        $("#tbRemarksMove").val("");
        $('#fuConfirm').val('');
        $('#fuMove').val('');
        $(".fileinput-filename").text('');
        
    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $('#mdlDetail').modal('toggle');
        //Table.Search();

    }
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

        $("#tblSummaryData tbody").on("click", ".btEdit", function (e) {
            e.preventDefault();
            var table = $(idTable).DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Edit.Selected = data;
                Table.ShowDetail();
                $('#mdlDetail').modal('toggle');

            }
        });


    },
    Search: function () {
        Table.Init("#tblSummaryData");
        TempData = [];
        AllDataId = [];
        CheckAllDataFlag = false;

        fsSONumber = $('#tbSearchSONumber').val();
        fsAccrueStatusID = $('#slSearchStatus').val();
        fsCustomerID = $('#slSearchCustomer').val();
        fsCompanyID = $('#slSearchCompany').val();
        fsSOW = $('#slSearchDept').val();
        fsdate = $('#tbSearchPeriode').val();
        fsweek = $('#slSearchPeriodeWeek').val();

        PeriodeSearch = $('#tbSearchPeriode').val();
        WeekSearch = $('#slSearchPeriodeWeek').val();

        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        
        param = {};
        param.SONumber = fsSONumber;
        param.AccrueStatusID = fsAccrueStatusID;
        param.CustomerID = fsCustomerID;
        param.CompanyID = fsCompanyID;
        param.SOW = fsSOW;
        var params = {
            vwtrxDataAccrue: param,
            date: fsdate,
            week: fsweek
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/UserConfirmation/grid",
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
                        if (full.StatusAccrue.toLowerCase().includes("user") == true)
                            if (CheckAllDataFlag)
                                strResult += "<label id='cb_" + full.trxDataAccrueID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input id='" + full.trxDataAccrueID + "' type='checkbox' checked class='checkboxes'/><span></span></label>";
                            else
                                strResult += "<label id='cb_" + full.trxDataAccrueID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input id='" + full.trxDataAccrueID + "' type='checkbox' class='checkboxes'/><span></span></label>";
                        else
                            strResult += "<label id='" + full.trxDataAccrueID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline' style='display:none'><input id='cb_" + full.trxDataAccrueID + "' type='checkbox' class='checkboxes' style='display:none'/><span></span></label>";

                        return strResult;
                    }
                },

                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "SiteIDOpr" },
                { data: "StatusMasterList" },

                { data: "CompanyID" },
               { data: "CompanyInvID" },
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
                     data: "StartDateBAPS", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                  {
                      data: "EndDateBAPS", render: function (data) {
                          return Common.Format.ConvertJSONDateTime(data);
                      }
                  },
                   { data: "TenantType" },
                   { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "TotalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "Currency" },
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
                    { data: "ODCategory" },
            { data: "RegionName" },
                { data: "Type" },
                 { data: "SOW" },
                { data: "StatusAccrue" },
                { data: "Remarks" },
                {
                    render: function (data, type, full) {
                        if (full.FileNameConfirm != null && full.FileNameConfirm != '')
                            return '<button type="button" class="btn btn-xs btn-primary btn-downloadConfirm" id="btDownload_' + full.trxDataAccrueID + '"><i class="fa fa-download"></i></button>';
                        return '';
                    }
                },
                {
                    render: function (data, type, full) {
                        if (full.FileNameMove != null && full.FileNameMove != '')
                            return '<button type="button" class="btn btn-xs btn-primary btn-downloadMove" id="btDownload_' + full.trxDataAccrueID + '"><i class="fa fa-download"></i></button>';
                        return '';
                    }
                },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSiteData").hide();
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
                        AllDataId = Helper.GetListAllId();
                        Helper.ValidateAllType();
                        CheckAllDataFlag = true;
                    }
                    else {
                        CheckAllDataFlag = false;
                        AllDataId = [];
                    }
                });
            }
        });

        $("#tblSummaryData tbody").on("click", "button.btn-downloadConfirm", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var row = table.row($(this).parents('tr')).data();
            Helper.Download(row.FilePathConfirm, row.FileNameConfirm, row.ContentTypeConfirm);
        });
        $("#tblSummaryData tbody").on("click", "button.btn-downloadMove", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var row = table.row($(this).parents('tr')).data();
            Helper.Download(row.FilePathMove, row.FileNameMove, row.ContentTypeMove);
        });


    },

    AddListSearchAll: function () {
        Table.Init("#tblAddData");
        var l = Ladda.create(document.querySelector("#btSearch"))
        //l.start();
        param = {};
        param.SONumber = fsSONumber;
        param.AccrueStatusID = fsAccrueStatusID;
        param.CustomerID = fsCustomerID;
        param.CompanyID = fsCompanyID;
        param.SOW = fsSOW;
        var params = {
            vwtrxDataAccrue: param,
            date: fsdate,
            week: fsweek
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
                "url": "/api/UserConfirmation/grid",
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
               { data: "SiteIDOpr" },
               { data: "StatusMasterList" },

               { data: "CompanyID" },
               { data: "CompanyInvID" },
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
                     data: "StartDateBAPS", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                 {
                     data: "EndDateBAPS", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                  { data: "TenantType" },
                  { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                   { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                   { data: "TotalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                   { data: "Currency" },
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
                   { data: "ODCategory" },
                   { data: "RegionName" },
               { data: "Type" },
                { data: "SOW" },
               { data: "StatusAccrue" },
               { data: "Remarks" }
            ],

            "columnDefs": [{ "targets": [0], "orderable": false }],
        });
        $("#tblAddData").unbind();
        $("#tblAddData").on("click", ".deleteRow", function (e) {
            var table = $("#tblAddData").DataTable();
            var row = table.row($(this).parents('tr')).data();
            table.row($(this).parents('tr')).remove().draw(false);
            Control.DeleteRequestDetailAll(row.trxDataAccrueID);
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
                { data: "SiteIDOpr" },
                { data: "StatusMasterList" },

                { data: "CompanyID" },
               { data: "CompanyInvID" },
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
                     data: "StartDateBAPS", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                  {
                      data: "EndDateBAPS", render: function (data) {
                          return Common.Format.ConvertJSONDateTime(data);
                      }
                  },
                   { data: "TenantType" },
                   { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "TotalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "Currency" },
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
                    { data: "ODCategory" },
            { data: "RegionName" },
                { data: "Type" },
                 { data: "SOW" },
                { data: "StatusAccrue" },
                { data: "Remarks" }
            ],

            "columnDefs": [{ "targets": [0], "orderable": false }],
        });

        $("#tblAddData").unbind();
        $("#tblAddData").on("click", ".deleteRow", function (e) {
            var table = $("#tblAddData").DataTable();
            var row = table.row($(this).parents('tr')).data();
            table.row($(this).parents('tr')).remove().draw(false);
            Control.DeleteRequestDetail(row.trxDataAccrueID);
        });
    },
    Export: function () {
        var SONumber = fsSONumber;
        var Week = fsweek == null ? "" : fsweek;
        var monthDate = fsdate;
        var AccrueStatusID = fsAccrueStatusID;
        var CustomerID = fsCustomerID;
        var CompanyID = fsCompanyID;
        var SOW = fsSOW;

        window.location.href = "/Accrue/UserConfirm/Export?SONumber=" + SONumber + "&AccrueStatusID=" + AccrueStatusID
            + "&monthDate=" + monthDate + "&CustomerID=" + CustomerID + "&CompanyID=" + CompanyID + "&Week=" + Week
        + "&SOW=" + SOW;
    },

    Reset: function () {
        $("#slSearchCustomer").val("").trigger('change');
        $("#slSearchCompany").val("").trigger('change');
        $("#slSearchDept").val("").trigger('change');
        $("#slSearchStatus").val("").trigger('change');
        $("#slDepartment").val("").trigger('change');
        $("#slSearchPeriodeWeek").val("").trigger('change');
        $("#tbSearchPeriode").val("");
        $("#tbSearchSONumber").val("");
        $("#tbRemarks").val("");
        $("#tbSearchSiteName").val("");
    },

}

var Action = {
    Confirm: function () {
        if (TempData.length > 0) {
            var requestData = [];
            var formData = new FormData();
            var fileInput = document.getElementById("fuConfirm");
            var file = null;
            if (fileInput.files != undefined) {
                file = fileInput.files[0];
            }
            if (file != null && file != undefined) {
                formData.append("FileConfirm", file);
            }

            if (CheckAllDataFlag)
                requestData = AllDataId;
            else {
                for (var i = 0; i < TempData.length; i++) {
                    requestData.push(TempData[i].trxDataAccrueID);
                }

            }
            formData.append("targetDate", $("#tbPeriode").val());
            formData.append("weekTargetUser", $("#slPeriodeWeek").val());
            formData.append("remarks", $("#tbRemarksConfirm").val());
            formData.append("ListID", requestData);

            formData.append("rootCauseID", $("#slRootcause").val());
            formData.append("picaID", $("#slPica").val());
            formData.append("picaDetailID", $("#slPicaDetail").val());

            var l = Ladda.create(document.querySelector("#btSubmit"))
            $.ajax({
                url: "/api/UserConfirmation/ConfirmUser",
                type: "POST",
                dataType: "json",
                contentType: false,
                data: formData,
                processData: false,
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
                        Form.CancelAdd();
                    //Table.Search();
                    }
                    
                }
                

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l.stop()
            })
        } else {
            Common.Alert.Warning("Request cannot be empty");
        }
    },

    Move: function () {

        if (TempData.length > 0) {
            var requestData = [];
            var formData = new FormData();
            var fileInput = document.getElementById("fuMove");
            var file = null;
            if (fileInput.files != undefined) {
                file = fileInput.files[0];
            }
            if (file != null && file != undefined) {
                formData.append("FileMove", file);
            }

            if (CheckAllDataFlag)
                requestData = AllDataId;
            else {
                for (var i = 0; i < TempData.length; i++) {
                    requestData.push(TempData[i].trxDataAccrueID);
                }

            }
            formData.append("department", $("#slDepartment").val());
            formData.append("remarks", $("#tbRemarksMove").val());
            formData.append("ListID", requestData);
            var params = {
                ListID: requestData,
                department: $("#slDepartment").val(),
                File: file
            };
            var l = Ladda.create(document.querySelector("#btSubmit"))
            $.ajax({
                url: "/api/UserConfirmation/Move",
                type: "POST",
                dataType: "json",
                contentType: false,
                data: formData,
                processData: false,
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
                        Form.CancelAdd();
                    }
                    
                }
                

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l.stop()
            })
        } else {
            Common.Alert.Warning("Request cannot be empty");
        }
    },
    CheckValidationMove: function () {

        var result = "";
        var fileInput = document.getElementById("fuMove");

        var file = null;
        if ($("#slDepartment").val() == null || $("#slDepartment").val() == "") {
            result += " Department,";
        }
        if (fileInput.files == undefined || fileInput.files.length == 0) {
            result += " File Upload,";
        }
        if ($("#tbRemarksMove").val() == null || $("#tbRemarksMove").val() == "") {
            result += " Remarks";
        }

        if (result == "") {
            var fsExtension = fileInput.files[0].name.split('.').pop().toUpperCase();
            if ((fileInput.files[0].size / 1024) > 2048)
                Common.Alert.Warning("Upload File Can`t Bigger Than 2048 bytes (2mb)!");
            else if (fsExtension != "XLS" && fsExtension != "XLSX" && fsExtension != "PDF")
                Common.Alert.Warning("Please Upload an Excel (.xls or .xlsx) or PDF File!");
            else
                Action.Move();
        }
        else
            Common.Alert.Warning(result + " is Mandatory");
    },
    CheckValidationConfirm: function () {

        var result = "";
        var fileInput = document.getElementById("fuConfirm");

        var file = null;
        if ($("#tbPeriode").val() == null || $("#tbPeriode").val() == "") {
            result += " Period Target,";
        }
        if ($("#slPeriodeWeek").val() == null || $("#slPeriodeWeek").val() == "") {
            result += " Week Target,";
        }
        if ($("#slRootcause").val() == null || $("#slRootcause").val() == "") {
            result += " Rootcause,";
        }
        if ($("#slPica").val() == null || $("#slPica").val() == "") {
            result += " Pica,";
        }
        if ($("#slPicaDetail").val() == null || $("#slPicaDetail").val() == "") {
            result += " Pica Detail,";
        }
        if (fileInput.files == undefined || fileInput.files.length == 0) {
            result += " File Upload,";
        }
        if ($("#tbRemarksConfirm").val() == null || $("#tbRemarksConfirm").val() == "") {
            result += " Remarks";
        }

        if (result == "") {
            var fsExtension = fileInput.files[0].name.split('.').pop().toUpperCase();
            if ((fileInput.files[0].size / 1024) > 2048)
                Common.Alert.Warning("Upload File Can`t Bigger Than 2048 bytes (2mb)!");
            else if (fsExtension != "XLS" && fsExtension != "XLSX" && fsExtension != "PDF")
                Common.Alert.Warning("Please Upload an Excel (.xls or .xlsx) or PDF File!");
            else
                Action.Confirm();
        }
        else
            Common.Alert.Warning(result + " is Mandatory");
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
                    $("#slSearchCompany").append("<option value='" + item.CompanyID + "'>" + item.CompanyID + " - " + item.Company + "</option>");
                })
            }

            $("#slSearchCompany").select2({ placeholder: "Select Company", width: null });

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
    BindingSelectStatus: function () {

        $.ajax({
            url: "/api/ListDataAccrue/statusAccrue/list",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchStatus").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchStatus").append("<option value='" + item.ID + "'>" + item.AccrueStatus + "</option>");
                })
            }

            $("#slSearchStatus").select2({ placeholder: "Select Status", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectRootCause: function () {

        $.ajax({
            url: "/api/ListDataAccrue/rootCause/list",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slRootcause").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slRootcause").append("<option value='" + item.ID + "'>" + item.RootCause + "</option>");
                })
            }

            $("#slRootcause").select2({ placeholder: "Select RootCause", width: null });
            $("#slPica").select2({ placeholder: "Select PICA", width: null });
            $("#slPicaDetail").select2({ placeholder: "Select PICA Detail", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectPICA: function () {
        var params = {
            rootCauseID: $("#slRootcause").val()
        };
        $.ajax({
            url: "/api/ListDataAccrue/picaAccrue/list",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slPica").html("<option></option>")

            if (Common.CheckError.List(data)) {

                $.each(data, function (i, item) {
                    $("#slPica").append("<option value='" + item.ID + "'>" + item.PICA + "</option>");
                })


            }

            $("#slPica").select2({ placeholder: "Select PICA", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectPICADetail: function () {
        var params = {
            picaID: $("#slPica").val()
        };
        $.ajax({
            url: "/api/ListDataAccrue/picaDetailAccrue/list",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slPicaDetail").html("<option></option>")

            if (Common.CheckError.List(data)) {

                $.each(data, function (i, item) {
                    $("#slPicaDetail").append("<option value='" + item.ID + "'>" + item.PICADetail + "</option>");
                })


            }

            $("#slPicaDetail").select2({ placeholder: "Select PICA Detail", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectDepartment: function () {
        //var params = {
        //    typeSOW: TempData[0].Type
        //};
        $.ajax({
            url: "/api/ListDataAccrue/departmentAccrue/list",
            type: "GET"//,
            //data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slDepartment").html("<option></option>")

            if (Common.CheckError.List(data)) {

                $.each(data, function (i, item) {
                    $("#slDepartment").append("<option value='" + item.DepartmentName + "'>" + item.DepartmentName + "</option>");
                })
            }
            $("#slDepartment").select2({ placeholder: "Select Department", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectWeekSearch: function () {
        var params = {
            date: $('#tbSearchPeriode').val()
        };
        $.ajax({
            url: "/api/ListDataAccrue/weekOfMonth/list",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchPeriodeWeek").html("<option></option>")

            if (Common.CheckError.List(data)) {

                $.each(data, function (i, item) {
                    $("#slSearchPeriodeWeek").append("<option value='" + item.ID + "'>" + item.Week + "</option>");
                })

            }

            $("#slSearchPeriodeWeek").select2({ placeholder: "Week", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectWeekSearchGetDate: function () {
       
        $.ajax({
            url: "/api/ListDataAccrue/weekOfMonth/listGetDate",
            type: "GET",
            async:false
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchPeriodeWeek").html("<option></option>")

            if (Common.CheckError.List(data)) {

                $.each(data, function (i, item) {
                    $("#slSearchPeriodeWeek").append("<option value='" + item.ID + "'>" + item.Week + "</option>");
                })

            }

            $("#slSearchPeriodeWeek").select2({ placeholder: "Week", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectMonthGetDate: function () {
        $.ajax({
            url: "/api/ListDataAccrue/Month/SetMonthGetDate",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $('#tbSearchPeriode').val(data);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectWeek: function () {
        var params = {
            date: $('#tbPeriode').val()
        };
        $.ajax({
            url: "/api/ListDataAccrue/weekOfMonth/list",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slPeriodeWeek").html("<option></option>")

            if (Common.CheckError.List(data)) {

                $.each(data, function (i, item) {
                    $("#slPeriodeWeek").append("<option value='" + item.ID + "'>" + item.Week + "</option>");
                })

            }

            $("#slPeriodeWeek").select2({ placeholder: "Week", width: null });

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

    DeleteRequestDetail: function (rowID) {
        $("#cb_" + rowID + " input[type=checkbox]").prop('checked', false);
        var index = TempData.findIndex(function (data) {
            return data.trxDataAccrueID == rowID
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
        var index2 = TempData.findIndex(function (data) {
            return data.trxDataAccrueID == rowID
        });
        AllDataId.splice(index, 1);
        TempData.splice(index2, 1);
        if (AllDataId.length == 0)
            $(".panelSiteData").hide();
    },
    SelectData: function () {
        if (TempData.length > 0) {
            $.each(TempData, function (i, item) {
                $("#cb_" + item.trxDataAccrueID + " input[type=checkbox]").prop('checked', true);
            })
        }
    },

    IsEmptyData: function (ID) {
        var result = true;
        if (TempData.length > 0) {
            for (var i = 0 ; i < TempData.length ; i++) {
                if (TempData[i].trxDataAccrueID == ID) {
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
    GetListAllId: function () {
        //for CheckAll Pages
        param = {};
        param.SONumber = $('#tbSearchSONumber').val();
        param.AccrueStatusID = $('#slSearchStatus').val();
        param.CustomerID = $('#slSearchCustomer').val();
        param.CompanyID = $('#slSearchCompany').val();
        param.SOW = $('#slSearchDept').val();
        var params = {
            vwtrxDataAccrue: param,
            date: $('#tbSearchPeriode').val(),
            week: $('#slSearchPeriodeWeek').val()
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/UserConfirmation/GettrxDataAccrueListId",
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
    ValidateAllType: function () {
        //for CheckAll Pages     
        var params = {
            ListID: AllDataId
        };
        var result = false;
        $.ajax({
            url: "/api/FinanceConfirmation/ValidateAllType",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            //if (Common.CheckError.Object(data)) {
            if (data.ErrorType == 0)
                TypeFlagAll = true;
            //}
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
        return result;
    },
    Download: function (filePath, FileName, contentType) {
        var docPath = $("#hdDocPath").val();
        var path = docPath + filePath;
        window.location.href = "/Admin/Download?path=" + path + "&fileName=" + FileName + "&contentType=" + contentType;
    },

    GetWeekNowSelected: function () {
        //for CheckAll Pages     
        var params = {            
        };
        $.ajax({
            url: "/api/UserConfirmation/GetWeekNowSelected",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            $('#slSearchPeriodeWeek').val(data).trigger('change');;
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });

    }
}

