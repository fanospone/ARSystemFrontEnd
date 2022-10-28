TempData = [];
RowSelected = {};
flagValid = true;
Data = {};
CountData = 0;
week = "";

//global parameters
//params = {};

jQuery(document).ready(function () {
    Form.Init();
    Table.Init();

    $("#tbSearchPeriode2").change(function () {
        if ($("#tbSearchPeriode2").val() != "")
            Control.BindingSelectWeek2();
    });

    $("#tbSearchPeriode").change(function () {
        if ($("#tbSearchPeriode").val() != "")
            Control.BindingSelectWeek();
    });

    $("#btnDownloadTemplate").unbind().click(function () {
        Form.DownloadTemplate();
    });

    $("#btValidate").unbind().click(function () {
        Form.Validate();
    });

    $("#btCancelAdd").unbind().click(function () {
        $('#mdlConfirmCancel').modal('show');
        
    });

    $("#btSubmit").unbind().click(function () {
        if (flagValid == true)
        {
            Action.SubmitUpload();
        } else {
            Common.Alert.Warning("Data Not Valid!");
        }
       
    });

    $("#btRemove").unbind().click(function () {
        Action.RemoveHistory(TempData);
    });

    $("#btSearch").unbind().click(function () {
        CheckAllDataFlag = false;
        Table.Search();
    });

    $(".btnSearchHeader").unbind().click(function () {
        Table.DetailTable();
    });

    $(".btnSearchHistory").unbind().click(function () {
        Table.Search();
    });

    $("#btReset").unbind().click(function () {
        Control.ClearFormDetail();
        $("#SONumberSearch").val('');
        $("#SiteIDSearch").val('');
        $("#RemarksSearch").val('');
    });

    $("#btResetHistory").unbind().click(function () {
        $("#schSONumber").val('');
        $("#schSiteID").val('');
    });

    $("#tblHistoryDataUpload").on('change', 'tbody tr .checkboxes', function () {
        var ID = $(this).parent().attr('id');
        var table = $("#tblHistoryDataUpload").DataTable();
        var idComponents = ID.split('cb_');
        var id = idComponents[1];
        var DataRows = {};
        //var Row = table.row($(this).parents('tr')).data();

        if (this.checked) {
            DataRows.ID = id;
            RowSelected = DataRows;
            TempData.push(RowSelected);
        } else {
            var index = TempData.findIndex(function (data) {
                return data.ID == DataRows.ID;
            });
            TempData.splice(index, 1);
        }
    });

    $("#btYesConfirm").unbind().click(function () {
        Action.Delete();
    });

    $("#btYesConfirmCancel").unbind().click(function () {
        Action.Cancel();
    });

    $(".progress").progressbar({ value: 0 });

    $('#lblDataValid').text(CountData);
    $('#lblDataNotValid').text(CountData);

});

var Form = {
    Init: function () {
        Control.BindingSelectMonthGetDate();

        Control.BindingSelectWeekSearchGetDate();
        Control.BindingSelectStatus();
        //Form.SetParams();
        $("#slSearchPeriodeWeek").select2({ placeholder: "Week", width: null });
        $("body").delegate(".datepicker", "focusin", function () {
            $(this).datepicker({
                format: "M-yyyy",
                viewMode: "months",
                minViewMode: "months"
            });
        });

        $('#iFileUpload').on('change', function () {
            var input = document.getElementById('iFileUpload');
            var infoArea = document.getElementById('file-upload-filename');
            var fileName = input.files[0].name;
            infoArea.textContent = fileName;
            $("#spanFile").text("");
        });

        $('#cbALLData input[type=checkbox]').prop('checked', false);

        Helper.GetWeekNowSelected();
        Action.CancelRefresh();
        Table.DetailTable();
        Table.Search();

    },

    //SetParams: function () {
    //    params = {
    //        MonthYear: $('#tbSearchPeriode2').val(),
    //        Week: $('#slSearchPeriodeWeek2').val(),
    //        AccrueStatusID: $('#slSearchStatus').val(),
    //        SONumber: $("#schSONumber").val(),
    //        SiteID: $("#schSiteID").val(),
    //    };
    //},

    DownloadTemplate: function () {
        window.location.href = "/Accrue/downloadTemplateUpdateAccrue";
    },

    Validate: function () {
        var l = Ladda.create(document.querySelector("#btValidate"));
        l.start();
        //var intervalID = setInterval(UpdateProgress, 250);
        var formData = new FormData(); //FormData object  
        var fileInput = document.getElementById("iFileUpload");
        if (fileInput.files[0] != undefined && fileInput.files[0] != null) {
            var fileName = fileInput.files[0].name;
            var extension = fileName.split('.').pop().toUpperCase();
            if (extension != "XLS" && extension != "XLSX") {
                Common.Alert.Warning("Please upload an Excel File.");
            }
            else {
                formData.append("File", fileInput.files[0]);
                formData.append("Month", $("#tbSearchPeriode").val());
                formData.append("Week", $("#slSearchPeriodeWeek").val());
                formData.append("IsActive", 1);

                $.ajax({
                    url: '/api/UploadAccrue/ValidateDataAccrue',
                    type: 'POST',
                    data: formData,
                    async: false,
                    beforeSend: function (xhr) {
                        l.start();
                    },
                    cache: false,
                    contentType: false,
                    processData: false, // Not to process data  
                }).done(function (data, textStatus, jqXHR) {
                    if (data != "") {
                        Common.Alert.Warning(data);
                    } else {
                        //$(".progress").progressbar("value", 100);
                        Common.Alert.Success("Upload data success!");
                        Table.DetailTable();
                        Helper.DataValid();
                        Helper.DataNotValid();
                        Control.ClearFormDetail();
                        //clearInterval(intervalID);
                    }
                    l.stop();
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    Common.Alert.Error(errorThrown)
                    l.stop()
                });
            }
        } else {
            Common.Alert.Warning("Please upload an Excel File.");
            l.stop();
        }

        l.stop();
    },

    //UpdateProgress: function () {
    //    var value = $(".progress").progressbar("option", "value");
    //    if (value < 100) {
    //        $(".progress").progressbar("value", value + 1);
    //    }
    //}

}

var Table = {
    Init: function () {

        $("#tblSummaryDataUpload tbody").unbind().on("click", "button.btDelete", function (e) {
            var table = $("#tblSummaryDataUpload").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                $('#mdlConfirm').modal('show');
            }
        });


    },

    DetailTable: function () {
        Helper.CheckRemarks();
        var SONumber = $("#SONumberSearch").val();
        var SiteID = $("#SiteIDSearch").val();
        var Remarks = $("#RemarksSearch").val();

        param = {
            SONumber: SONumber,
            SiteID: SiteID,
            Remarks: Remarks
        };

        var tblSummaryData = $("#tblSummaryDataUpload").DataTable({
            "deferRender": false,
            "proccessing": false,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/UploadAccrue/list",
                "type": "POST",
                "datatype": "json",
                "data": param,
            },
            buttons: [
               { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
               {
                   text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                       var l = Ladda.create(document.querySelector(".yellow"));
                       l.start();
                       Table.ExportDetailTable()
                       l.stop();
                   }
               },
               { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            "order": false,
            "columns": [{
                        orderable: false,
                        mRender: function (data, type, full) {
                            //var button = "<a type='button' class='btn green btn-xs btnDelete center' value='" + full.ID +
                            //    "' OnClick='Form.DeleteDataUpload(" + full.ID + ")'><i class='fa fa-trash'></i></a>";
                            var button = "<button type='button' title='Delete' class='btn btn-xs green btDelete'><i class='fa fa-trash'></i></button>";
                            return button;
                        }
                    },
                   { data: "SONumber" },
                   { data: "SiteID" },
                   { data: "SiteName" },
                   { data: "SiteIDOpr" },
                   { data: "SiteNameOpr" },
                   { data: "CompanyID" },
                   { data: "CompanyInvID" },
                   { data: "CustomerID" },
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
                   { data: "RentalStart" },
                   { data: "RentalEndNew" },
                   { data: "TenantType" },
                   { data: "Type" },
                   { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                   { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                   { data: "BaseOnMasterListData", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                   { data: "BaseOnRevenueListingNew", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
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
                   { data: "TotalAccrue", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                   { data: "MonthYear" },
                   { data: "Year" },
                   { data: "Week" },
                   { data: "Remarks" }
            ],

            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnRowCallback": function (nRow, adata) {
                if (adata.Remarks !== null) {
                    $('td', nRow).css('background-color', 'Red');
                }
            }
        });

    },

    Search: function () {
        CheckAllDataFlag = false;
        AllDataId = [];

        var params = {
            MonthYear: $('#tbSearchPeriode2').val(),
            Week: $('#slSearchPeriodeWeek2').val(),
            AccrueStatusID: $('#slSearchStatus').val(),
            SONumber: $("#schSONumber").val(),
            SiteID: $("#schSiteID").val(),
        };

        var tblSummaryData = $("#tblHistoryDataUpload").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/UploadAccrue/listhistory",
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
            "order": [],
            "columns": [{
                        orderable: false,
                        mRender: function (data, type, full) {
                            var strResult = "";
                            if (full.AccrueStatusID == 1) {
                                if (CheckAllDataFlag)
                                    strResult += "<label id='cb_" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input id='" + full.ID + "' type='checkbox' checked class='checkboxes'/><span></span></label>";
                                else
                                    strResult = "<label id='cb_" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input id='" + full.ID + "' type='checkbox' class='checkboxes'/><span></span></label>";
                            } else {
                                strResult = "<label id='cb_" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline' style='display:none'><input id='" + full.ID + "' type='checkbox' class='checkboxes'/><span></span></label>";
                            }                         
                          
                            return strResult;
                        }
                    },
                   { data: "SONumber" },
                   { data: "SiteID" },
                   { data: "SiteName" },
                   { data: "SiteIDOpr" },
                   { data: "SiteNameOpr" },
                   { data: "CompanyID" },
                   { data: "CompanyInvID" },
                   { data: "CustomerID" },
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
                   { data: "RentalStart" },
                   { data: "RentalEndNew" },
                   { data: "TenantType" },
               { data: "Type" },
                   { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                   { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                   { data: "BaseOnMasterListData", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                   { data: "BaseOnRevenueListingNew", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
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
                   { data: "TotalAccrue", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
               { data: "Month" },
               { data: "Year" },
               { data: "Week" },
               { data: "AccrueStatus" },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                if (AllDataId.length > 0) {
                    var item;
                    for (var i = 0; i < AllDataId.length; i++) {
                        item = AllDataId[i];
                        $("#Row" + item).addClass("active");
                    }
                }

            },
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label id="cbALLData" class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                    '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tblHistoryDataUpload .checkboxes" />' +
                    '<span></span> ' +
                    '</label>';
                var th = $("th.select-all-raw").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-raw").unbind().on("change", ".group-checkable", function (e) {
                    var set = $(".panelSearchResult .checkboxes");
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
    },

    Export: function () {
        var MonthYear = $('#tbSearchPeriode2').val();
        var Week = $('#slSearchPeriodeWeek2').val() == null ? "" : $('#slSearchPeriodeWeek2').val();
        var AccrueStatusID = $('#slSearchStatus').val();

        window.location.href = "/Accrue/HistoryUploadAccrue/Export?MonthYear=" + MonthYear + "&Week=" + Week
            + "&AccrueStatusID=" + AccrueStatusID;
    },

    ExportDetailTable: function () {
        var SONumber = $("#SONumberSearch").val();
        var SiteID = $("#SiteIDSearch").val();
        var Remarks = $("#RemarksSearch").val();

        window.location.href = "/Accrue/UploadAccrue/ExportDetailTable?SONumber=" + SONumber + "&SiteID=" + SiteID
            + "&Remarks=" + Remarks;
    },
}

var Action = {
    SubmitUpload: function () {
        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/UploadAccrue/SubmitUploadDataAccrue",
            type: "post",
            dataType: "json",
            contentType: "application/json",
            //data: JSON.stringify(data),
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
                    Table.DetailTable();
                    Table.Search();
                    Helper.DataValid();
                    Helper.DataNotValid();
                }
            }
            else {
                Common.Alert.Error(data.ErrorMessage);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    },

    Delete: function () 
    {
        var params = {
            ID: Data.Selected.ID
        };
        var l = Ladda.create(document.querySelector("#btYesConfirm"));
        $.ajax({
            url: "/api/UploadAccrue/Delete",
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
                $('#mdlConfirm').modal('toggle');
                Common.Alert.Success("Data Has Been Deleted.");
                $(".confirm.btn-success").unbind().click(function () {
                    //location.reload();
                    Table.DetailTable();
                    
                });
            }
            l.stop()
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        });
    },

    Cancel: function () {
        $.ajax({
            url: "/api/UploadAccrue/CancelUpload",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            //data: JSON.stringify(params),
            cache: false,
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                $('#mdlConfirmCancel').modal('hide');
                Common.Alert.Success("Data has been deleted.");
                $(".confirm.btn-success").unbind().click(function () {
                    location.reload();
                });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
    },

    CancelRefresh: function () {
        $.ajax({
            url: "/api/UploadAccrue/CancelUpload",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            //data: JSON.stringify(params),
            cache: false,
        }).done(function (data, textStatus, jqXHR) {

        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
    },

    RemoveHistory: function (tempData) {
        if (CheckAllDataFlag == true) {

            var params = {
                MonthYear: $('#tbSearchPeriode2').val(),
                Week: $('#slSearchPeriodeWeek2').val(),
                AccrueStatusID: $('#slSearchStatus').val(),
                SONumber: $("#schSONumber").val(),
                SiteID: $("#schSiteID").val(),
            };

            $.ajax({
                url: "/api/UploadAccrue/DeleteHistoryByParams",
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false,
            }).done(function (data, textStatus, jqXHR) {
                if (Common.CheckError.Object(data)) {
                    Common.Alert.Success("Data has been deleted.");
                    $(".confirm.btn-success").unbind().click(function () {
                        //location.reload();
                        Table.Search();
                    });
                }
                //l.stop();
            }).fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
            });
        } else {
            if (TempData.length > 0) {
                var requestData = [];
                for (var i = 0; i < TempData.length; i++) {
                    requestData.push(TempData[i].ID);
                }
                var params = {
                    ListID: requestData
                };
                $.ajax({
                    url: "/api/UploadAccrue/DeleteHistory",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json",
                    data: JSON.stringify(params),
                    cache: false,
                }).done(function (data, textStatus, jqXHR) {
                    if (Common.CheckError.Object(data)) {
                        Common.Alert.Success("Data has been deleted.");
                        $(".confirm.btn-success").unbind().click(function () {
                            //location.reload();
                            Table.Search();
                        });
                    }
                    //l.stop();
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    Common.Alert.Error(errorThrown)
                });
            } else {
                Common.Alert.Warning("Request cannot be empty");
            }
        }
    
    }
}

var Control = {
    ClearFormDetail: function () {
        document.getElementById('iFileUpload').value = "";
        document.getElementById("file-upload-filename").innerText = "";
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
    BindingSelectWeek2: function () {
        var params = {
            date: $('#tbSearchPeriode2').val()
        };
        $.ajax({
            url: "/api/ListDataAccrue/weekOfMonth/list",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchPeriodeWeek2").html("<option></option>")

            if (Common.CheckError.List(data)) {

                $.each(data, function (i, item) {
                    $("#slSearchPeriodeWeek2").append("<option value='" + item.ID + "'>" + item.Week + "</option>");
                })

            }

            $("#slSearchPeriodeWeek2").select2({ placeholder: "Week", width: null });

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

            $("#slSearchPeriodeWeek2").html("<option></option>")

            if (Common.CheckError.List(data)) {

                $.each(data, function (i, item) {
                    $("#slSearchPeriodeWeek2").append("<option value='" + item.ID + "'>" + item.Week + "</option>");
                })

            }

            $("#slSearchPeriodeWeek2").select2({ placeholder: "Week", width: null });

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
            $('#tbSearchPeriode2').val(data);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectStatus: function () {

        $.ajax({
            url: "/api/UploadAccrue/statusAccrue/list",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            //$("#slSearchStatus").html("<option></option>")

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
}

var Helper = {
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
            $('#slSearchPeriodeWeek').val(data).trigger('change');
            $('#slSearchPeriodeWeek2').val(data).trigger('change');
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });

    },

    CheckRemarks: function()
    {
        var params = {
        };
        $.ajax({
            url: "/api/UploadAccrue/checkremarks",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            flagValid = data;
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
    },

    DataValid: function()
    {
        $.ajax({
            url: "/api/UploadAccrue/CountDataValid",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $('#lblDataValid').text(data);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
    },

    DataNotValid: function () {
        $.ajax({
            url: "/api/UploadAccrue/CountDataNotValid",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $('#lblDataNotValid').text(data);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
    },

    GetListId: function () {
        //for CheckAll Pages
        var SONumber = $("#schSONumber").val();
        var SiteID = $("#schSiteID").val();

        var params = {
            MonthYear: $('#tbSearchPeriode2').val(),
            Week: $('#slSearchPeriodeWeek2').val(),
            AccrueStatusID: $('#slSearchStatus').val(),
            SONumber: SONumber,
            SiteID: SiteID,
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/UploadAccrue/GetListId",
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

    LoadProgressBar: function()
    {
        var progressbar = $(".progress");
        var progressLabel = $("#myBar");
        progressbar.show();  
        $(".progress").progressbar({
            //value: false,  
            change: function () {  
                progressLabel.text(  
                 progressbar.progressbar("value") + "%");  // Showing the progress increment value in progress bar  
            },  
            complete: function () {  
                progressLabel.text("Loading Completed!");   
                progressbar.progressbar("value", 0);  //Reinitialize the progress bar value 0  
                progressLabel.text("");   
                progressbar.hide(); //Hiding the progress bar
                Table.DetailTable();
            }  
        });  
        function progress() {  
            var val = progressbar.progressbar("value") || 0;  
            progressbar.progressbar("value", val + 1);  
            if (val < 99) {  
                setTimeout(progress, 25);
                var elem = document.getElementById("myBar");
                var width = val;
                elem.style.width = width + "%";
                elem.innerHTML = width + "%";

                //var elem = document.getElementById("myBar");
                //var width = 0;
                //var id = setInterval(frame, 10);
                //function frame() {
                //    if (width >= 100) {
                //        clearInterval(id);
                //        //i = 0;
                //    } else {
                //        width++;
                //        elem.style.width = width + "%";
                //        elem.innerHTML = width + "%";
                //    }
                //}
            }

            //var i = 0;
            //if (i == 0) {
            //    i = 1;
            //    var elem = document.getElementById("myBar");
            //    var width = 10;
            //    var id = setInterval(frame, 10);
            //    function frame() {
            //        if (width >= 100) {
            //            clearInterval(id);
            //            i = 0;
            //        } else {
            //            width++;
            //            elem.style.width = width + "%";
            //            elem.innerHTML = width + "%";
            //        }
            //    }
            //}
        }  
        setTimeout(progress, 100);

    }
}