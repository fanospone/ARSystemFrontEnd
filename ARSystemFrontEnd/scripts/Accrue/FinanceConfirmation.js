Data = {};
Edit = {};
Data.RowSelectedRaw = [];
RowSelected = {};
TempData = [];
AllDataId = [];
CheckAllDataFlag = false;
TypeFlagAll = false;
Flag = "";
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
    $("#btUpdate").unbind().click(function () {
        if ($("#tbDetailBaseLeasePrice").val() == "")
            Common.Alert.Warning("Please Input Base Lease Price")
        else if ($("#tbDetailServicePrice").val() == "")
            Common.Alert.Warning("Please Input Service Price")
        else
            Action.UpdateAmount();
    });

    $(".CancelEdit").unbind().click(function () {
        Table.Search();
        $('#mdlDetail').modal('hide');

    });

    $("#tbDetailBaseLeasePrice").change(function () {
        Edit.Selected.BaseLeasePrice = $("#tbDetailBaseLeasePrice").val().replace(/,/g, '');
        Control.Calculate(Edit.Selected);
        $('#tbDetailBaseLeasePrice').val(Common.Format.CommaSeparation(Edit.Selected.BaseLeasePrice.toString()));
        $('#tbDetailTotalAmount').val(Common.Format.CommaSeparation(Edit.Selected.TotalAmount.toString()));
        Edit.Selected.TotalAmount = $('#tbDetailTotalAmount').val().replace(/,/g, '');
    });

    $("#tbDetailServicePrice").change(function () {
        Edit.Selected.ServicePrice = $("#tbDetailServicePrice").val().replace(/,/g, '');
        Control.Calculate(Edit.Selected);
        $('#tbDetailServicePrice').val(Common.Format.CommaSeparation(Edit.Selected.ServicePrice.toString()))
        $('#tbDetailTotalAmount').val(Common.Format.CommaSeparation(Edit.Selected.TotalAmount.toString()));
        Edit.Selected.TotalAmount = $('#tbDetailTotalAmount').val().replace(/,/g, '');
    });
    $("#btConfirm").unbind().click(function () {
        if (TempData.length > 0) {
            Flag = "Confirm";
            $(".panelSiteData").show();
            $("#lblDetailAdd").html('List Add Confirm Accrue');
            if (CheckAllDataFlag)
                Table.AddListSearchAll();
            else
                Table.AddListSearch();
            $("#divRemarks").show();
            $("#divDepartment").hide();
        }
        else
            Common.Alert.Warning("Please Select One or More Data")

    });
    $("#btMove").unbind().click(function () {
        if (TempData.length > 0) {
            Flag = "Move";
            
            $(".panelSiteData").show();
            $("#lblDetailAdd").html('List Add Move Accrue');
            Control.BindingSelectDepartment();
            Table.AddListSearch();
            $("#divRemarks").hide();
            $("#divDepartment").show();
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
                    Action.Confirm();
                else if (Flag == "Move") {
                    if ($("#slDepartment").val() != null && $("#slDepartment").val() != "")
                        Action.Move();
                    else
                        Common.Alert.Warning("Please Select Department")
                }

            }

        }

    });
    $("#tbSearchPeriode").change(function () {
        if ($("#tbSearchPeriode").val() != "")
            Control.BindingSelectWeek();
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
                DataRows.CustomerName = Row.CustomerName;
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
        $("#slSearchPeriodeWeek").select2({ placeholder: "Week", width: null });
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
        $("#tbRemarks").val('');
        $("#divRemarks").hide();
    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
    },
    Done: function () {
        //$("#pnlSummary").fadeIn();
        //$('#mdlDetail').modal('toggle');
        //Table.Search();

    }
}
var Action = {
    Confirm: function () {
        if (TempData.length > 0) {
            var requestData = [];
            var paramAll = "";
            param = {};
            if (CheckAllDataFlag) {
                paramAll = "Not Yet";
                param.SONumber = fsSONumber;
                param.AccrueStatusID = fsAccrueStatusID;
                param.CustomerID = fsCustomerID;
                param.CompanyID = fsCompanyID;
                param.SOW = fsSOW;
                //requestData = AllDataId;
            }

            else {
                for (var i = 0; i < TempData.length; i++) {
                    requestData.push(TempData[i].trxDataAccrueID);
                }

            }

            var params = {
                ListID: requestData,
                remarks: $("#tbRemarks").val(),
                vwtrxDataAccrue: param,
                paramAllData: paramAll,
                date: fsdate,
                week: fsweek
            };
            var l = Ladda.create(document.querySelector("#btSearch"))
            var l2 = Ladda.create(document.querySelector("#btSubmit"))
            $.ajax({
                url: "/api/FinanceConfirmation/ConfirmFinance",
                type: "post",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false,
                beforeSend: function (xhr) {
                    l2.start();
                    l.start();
                }
            }).done(function (data, textStatus, jqXHR) {
                if (Common.CheckError.List(data)) {
                    if (data.ErrorType != 0) {
                        Common.Alert.Warning(data.ErrorMessage);
                    } else {
                        Common.Alert.Success("Data has been saved!")
                        l2.stop();
                        l.stop();
                        TempData = [];
                        RowSelected = {};
                        $("#tbRemarks").val("");
                        $(".panelSiteData").hide();
                        $(".panelSearchResult").hide();
                        //Table.Search();
                    }
                    
                }
                else {
                    Common.Alert.Success(data.ErrorMessage);
                }
                l2.stop();
                l.stop();
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l2.stop();
                l.stop()
            })
        } else {
            Common.Alert.Warning("Request cannot be empty");
        }
    },
    UpdateAmount: function () {

        var params = {
            vwtrxDataAccrue: Edit.Selected
        };
        var l = Ladda.create(document.querySelector("#btUpdate"))
        $.ajax({
            url: "/api/FinanceConfirmation/UpdateAmount",
            type: "post",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                $('#mdlDetail').modal('hide');
                Common.Alert.Success("Data has been saved!");
                Table.Search();
            }

            l.stop();


        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })

    },
    Move: function () {
        if (TempData.length > 0) {
            var requestData = [];
            var paramAll = "";
            param = {};
            if (CheckAllDataFlag) {
                paramAll = "Not Yet";
                param.SONumber = fsSONumber;
                param.AccrueStatusID = fsAccrueStatusID;
                param.CustomerID = fsCustomerID;
                param.CompanyID = fsCompanyID;
                param.SOW = fsSOW;
                // requestData = AllDataId;
            }
            else {
                for (var i = 0; i < TempData.length; i++) {
                    requestData.push(TempData[i].trxDataAccrueID);
                }
            }

            var params = {
                ListID: requestData,
                department: $("#slDepartment").val(),
                Type: $("#slType").val(),
                vwtrxDataAccrue: param,
                paramAllData: paramAll,
                date: fsdate,
                week: fsweek
            };
            var l = Ladda.create(document.querySelector("#btSearch"))
            var l2 = Ladda.create(document.querySelector("#btSubmit"))
            $.ajax({
                url: "/api/FinanceConfirmation/Move",
                type: "post",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false,
                beforeSend: function (xhr) {
                    l2.start();
                    l.start();

                }
            }).done(function (data, textStatus, jqXHR) {
                if (Common.CheckError.List(data)) {
                    if (data.ErrorType != 0) {
                        Common.Alert.Warning(data.ErrorMessage);
                    }
                    else {
                        Common.Alert.Success("Data has been saved!")
                        l2.stop();
                        l.stop();

                        TempData = [];
                        RowSelected = {};
                        $(".panelSiteData").hide();
                        $(".panelSearchResult").hide();
                    //Table.Search();
                    }
                    
                }
                l2.stop();
                l.stop();

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l2.stop();
                l.stop();
            })
        } else {
            Common.Alert.Warning("Request cannot be empty");
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

        $("#tblSummaryData tbody").on("click", ".btEdit", function (e) {
            e.preventDefault();
            var table = $(idTable).DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Edit.Selected = data;
                Table.ShowDetail();
                $('#mdlDetail').modal('show');

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
                "url": "/api/FinanceConfirmation/grid",
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
                        if (full.StatusAccrue.toLowerCase().includes("finance") == true) {
                            if (CheckAllDataFlag)
                                strResult += "<label id='cb_" + full.trxDataAccrueID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input id='" + full.trxDataAccrueID + "' type='checkbox' checked class='checkboxes'/><span></span></label>";

                            else
                                strResult += "<label id='cb_" + full.trxDataAccrueID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input id='" + full.trxDataAccrueID + "' type='checkbox' class='checkboxes'/><span></span></label>";

                        }
                        else
                            strResult += "<label id='" + full.trxDataAccrueID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline' style='display:none'><input id='cb_" + full.trxDataAccrueID + "' type='checkbox' class='checkboxes' style='display:none'/><span></span></label>";

                        return strResult;
                    }
                },
                 {
                     orderable: false,
                     mRender: function (data, type, full) {
                         var strReturn = "";
                         if ($("#hdAllowProcess").val()) {
                             if (full.StatusAccrue.toLowerCase().includes("finance") == true)
                                 strReturn += "<button type='button' title='Edit' class='btn btn-xs green btEdit'><i class='fa fa-edit'></i></button>";
                             else
                                 strReturn += "<button type='button' title='Edit' class='btn btn-xs green btEdit' style='display:none'><i class='fa fa-edit'></i></button>";
                         }
                         return strReturn;
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
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSiteData").hide();
                    $(".panelSearchResult").fadeIn();
                }


                if (CheckAllDataFlag) {
                    var item;
                    $('#cbALLData input[type=checkbox]').prop('checked', true);

                }
                else {
                    $('#cbALLData input[type=checkbox]').prop('checked', false);
                    Control.SelectData();
                }
                l.stop();
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
                        CheckAllDataFlag = true;
                        Helper.ValidateAllType();

                    }
                    else {
                        CheckAllDataFlag = false;
                        AllDataId = [];
                    }

                });
            }
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
                "url": "/api/FinanceConfirmation/grid",
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

        window.location.href = "/Accrue/FinanceConfirm/Export?SONumber=" + SONumber + "&AccrueStatusID=" + AccrueStatusID
            + "&monthDate=" + monthDate + "&CustomerID=" + CustomerID + "&CompanyID=" + CompanyID + "&Week=" + Week
        + "&SOW=" + SOW;
    },

    Reset: function () {
        $("#slSearchCustomer").val("").trigger('change');
        $("#slSearchCompany").val("").trigger('change');
        $("#slSearchDept").val("").trigger('change');
        $("#slSearchStatus").val("").trigger('change');
        $("#slDepartment").val("").trigger('change');
        $("#slType").val("").trigger('change');
        $("#slSearchPeriodeWeek").val("").trigger('change');
        $("#tbSearchPeriode").val("");
        $("#tbSearchSONumber").val("");
        $("#tbRemarks").val("");
        $("#tbSearchSiteName").val("");
    },
    ShowDetail: function () {
        $('#tbDetailSONumber').val(Edit.Selected.SONumber);
        $('#tbDetailSiteID').val(Edit.Selected.SiteID);
        $('#tbDetailSiteName').val(Edit.Selected.SiteName);
        $('#tbDetailSiteIDOpr').val(Edit.Selected.SiteIDOpr);
        $('#tbDetailSiteNameOpr').val(Edit.Selected.SiteNameOpr);

        $('#tbDetailStartDateBAPS').val(Common.Format.ConvertJSONDateTime(Edit.Selected.StartDateBAPS));
        $('#tbDetailEndDateBAPS').val(Common.Format.ConvertJSONDateTime(Edit.Selected.EndDateBAPS));
        $('#tbDetailStartDateAccrue').val(Common.Format.ConvertJSONDateTime(Edit.Selected.StartDateAccrue));
        $('#tbDetailEndDateAccrue').val(Common.Format.ConvertJSONDateTime(Edit.Selected.EndDateAccrue));

        $('#tbDetailBaseLeasePrice').val(Common.Format.CommaSeparation(Edit.Selected.BaseLeasePrice.toString()));
        $('#tbDetailServicePrice').val(Common.Format.CommaSeparation(Edit.Selected.ServicePrice.toString()));
        Control.Calculate(Edit.Selected);
        $('#tbDetailTotalAmount').val(Common.Format.CommaSeparation(Edit.Selected.TotalAmount.toString()));


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
            $("#slType").html("<option></option>")
            if (Common.CheckError.List(data)) {

                $.each(data, function (i, item) {
                    $("#slDepartment").append("<option value='" + item.DepartmentName + "'>" + item.DepartmentName + "</option>");
                })


            }

            $("#slDepartment").select2({ placeholder: "Select Department", width: null });
            $("#slType").append("<option value='NEW'>NEW</option>");
            $("#slType").append("<option value='REC'>REC</option>");
            $("#slType").select2({ placeholder: "Select Type", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectWeek: function () {
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
            async: false
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
    Calculate: function (input) {
        //var result = 0;
        var params = {
            vwtrxDataAccrue: input
        };
        $.ajax({
            url: "/api/FinanceConfirmation/Calculate",
            type: "POST",
            async: false,
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            Edit.Selected.TotalAmount = data;
            //return result;
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
        var AjaxData = [];
        $.ajax({
            url: "/api/FinanceConfirmation/GettrxDataAccrueListId",
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
        var requestData = [];
        var paramAll = "";
        param = {};
        if (CheckAllDataFlag) {
            paramAll = "Not Yet";
            param.SONumber = fsSONumber;
            param.AccrueStatusID = fsAccrueStatusID;
            param.CustomerID = fsCustomerID;
            param.CompanyID = fsCompanyID;
            param.SOW = fsSOW;
            //requestData = AllDataId;
        }

        else {
            for (var i = 0; i < TempData.length; i++) {
                requestData.push(TempData[i].trxDataAccrueID);
            }

        }
        //for CheckAll Pages     
        var params = {
            ListID: requestData,
            vwtrxDataAccrue: param,
            paramAllData: paramAll,
            date: fsdate,
            week: fsweek
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