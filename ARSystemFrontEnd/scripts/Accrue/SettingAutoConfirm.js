Data = {};

jQuery(document).ready(function () {
    Form.Init();
    $("#btReset").unbind().click(function () {
        Table.Reset();
    });

    $("#btCancel").unbind().click(function () {
        Form.Cancel();
    });
    $("#btCancelAdd").unbind().click(function () {
        Form.CancelAdd();
    });
    $("#btSearch").unbind().click(function () {
        Table.Search();
    });   
    $("#btCreate").unbind().click(function () {
        Form.Create();
    });
    $("#tbSearchPeriode").change(function () {
        if ($("#tbSearchPeriode").val() != "")
            Control.BindingSelectWeekSearch();
    });
    $("#tbPeriode").change(function () {
        if ($("#tbPeriode").val() != "")
            Control.BindingSelectWeek();
    });
    //$("#btSubmit").unbind().click(function () {
    //    Action.Save();
    //});
    $("#btSubmit").on('click', function () {
        var result = "";
        if ($("#tbPeriode").val() == null || $("#tbPeriode").val() == "") {
            result += " Month Period ,";
        }
        if ($("#slPeriodeWeek").val() == null || $("#slPeriodeWeek").val() == "") {
            result += " Week ,";
        }
        if ($("#slStatus").val() == null || $("#slStatus").val() == "") {
            result += " Activity,";
        }
        if ($("#tbAutoConfirmDate").val() == null || $("#tbAutoConfirmDate").val() == "") {
            result += " Auto Confirm Date ,";
        }
        if (result == "") {
            Action.Save();
        }
        else {
            Common.Alert.Warning(result + " is Mandatory");
        }
    });
    $("#form").on('submit', function (e) {
        
        //if ($("#form").parsley().validate()) {
        //    Action.Save();
        //}
        //e.preventDefault();
    });
    $("#btYesConfirm").unbind().click(function () {
        Action.Delete();
    });
});

var Form = {
    Init: function () {
        $("#pnlTransaction").hide();
        $(".panelSearchZero").hide();
        $("#form").parsley();

        if (!$("#hdAllowProcess").val()) {
            $("#btCreate").hide();
        }
        $("body").delegate(".datepicker", "focusin", function () {
            $(this).datepicker({
                format: "M-yyyy",
                viewMode: "months",
                minViewMode: "months"
            });
        });
        $("body").delegate(".datepickers", "focusin", function () {
            $(this).datetimepicker({
                dateFormat: "dd-mm-yy",
                startDate: new Date(),
                timeFormat: "HH:mm"
            });
        });
        
        $("#slSearchPeriodeWeek").select2({ placeholder: "Week", width: null });
        $("#slPeriodeWeek").select2({ placeholder: "Week", width: null });
        Control.BindingSelectStatus();
        
        Table.Init();
        Table.Search();
    },
    Create: function () {
        Data.Mode = "Create";
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Setting Auto Confirm");
        $("#form").parsley().reset()
        $("#tbPeriode").val("");
        $("#tbAutoConfirmDate").val("");
        $("#slPeriodeWeek").val("").trigger('change');
        $("#slStatus").val("").trigger('change');
        Control.BindingSelectMonthGetDate();
        Control.BindingSelectWeekGetDate();
        Control.GetWeekNowSelected();
    },
    
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
        $(".panelSearchBegin").fadeIn();
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        
        Table.Search();
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

        $("#tblSummaryData tbody").unbind().on("click", "button.btDelete", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                $('#mdlConfirm').modal('show');
                //Form.Delete();
            }
        });

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        param = {};        
        param.AccrueStatusID = $('#slSearchStatus').val();
        var params = {
            vwmstAccrueSettingAutoConfirm: param,
            date: $('#tbSearchPeriode').val(),
            week: $('#slSearchPeriodeWeek').val()
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/SettingAutoConfirm/grid",
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
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        
                        if ($("#hdAllowProcess").val())
                            strReturn += "<button type='button' title='Delete' class='btn btn-xs red btDelete'><i class='fa fa-trash'></i></button>";
                        return strReturn;
                    }
                },
                { data: "PeriodText" },
                { data: "AccrueStatus" },
                //{ data: "AutoConfirmDate" },
                {
                    data: "AutoConfirmDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data) + ' ' + ((parseInt(new Date(data).getHours()) < 10) ? "0" + new Date(data).getHours() : new Date(data).getHours()) + ':' + ((parseInt(new Date(data).getMinutes()) < 10) ? "0" + new Date(data).getMinutes() : new Date(data).getMinutes());
                    }
                },
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
                l.stop();
                App.unblockUI('.panelSearchResult');
            },

        });
    },
    Reset: function () {

        $("#slSearchPeriodeWeek").val("").trigger('change'); 
        $("#tbSearchPeriode").val("");
        $("#slSearchStatus").val("").trigger('change');
    },
    Export: function () {
        var AccrueStatusID = $('#slSearchStatus').val();
        var date = $('#tbSearchPeriode').val();
        var week = $('#slSearchPeriodeWeek').val();

        window.location.href = "/Accrue/SettingAutoConfirm/Export?Date=" + date + "&Week=" + week + "&AccrueStatusID=" + AccrueStatusID;
    }
}

var Action = {
    Save: function () {
        param = {};
        param.AccrueStatusID = $('#slStatus').val();

        var params = {
            vwmstAccrueSettingAutoConfirm: param,
            date: $('#tbPeriode').val(),
            week: $('#slPeriodeWeek').val(),
            autoConfirmDate: $('#tbAutoConfirmDate').val()
        };

        var l = Ladda.create(document.querySelector("#btSubmit"));
        $.ajax({
            url: "/api/SettingAutoConfirm/Save",
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
                Common.Alert.Success("Data has been saved!");
                Table.Reset();
                Form.Done();
            }
            else {
                Common.Alert.Error(data.ErrorMessage);
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    },

    Delete: function () {
        
        param = {};
        param.ID = Data.Selected.ID;
        var params = {
            vwmstAccrueSettingAutoConfirm: param            
        };
        var l = Ladda.create(document.querySelector("#btSubmit"));
        $.ajax({
            url: "/api/SettingAutoConfirm/Delete",
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
                Common.Alert.Success("Data has been deleted!");
                Table.Reset();
                Form.Done();
            }
            l.stop()
        })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l.stop()
            })
    }
}
var Control = {
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
            $('#tbPeriode').val(data);
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
    BindingSelectStatus: function () {

        $.ajax({
            url: "/api/ListDataAccrue/statusAccrue/list",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchStatus").html("<option></option>")
            $("#slStatus").html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item.ID == "3" || item.ID == "5") {
                        $("#slSearchStatus").append("<option value='" + item.ID + "'>" + item.AccrueStatus + "</option>");
                        $("#slStatus").append("<option value='" + item.ID + "'>" + item.AccrueStatus + "</option>");
                    }
                   
                })
            }

            $("#slSearchStatus").select2({ placeholder: "Select Activity", width: null });
            $("#slStatus").select2({ placeholder: "Select Activity", width: null });

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
            $('#tbPeriode').val(data);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectWeekGetDate: function () {

        $.ajax({
            url: "/api/ListDataAccrue/weekOfMonth/listGetDate",
            type: "GET",
            async: false
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
            $('#slPeriodeWeek').val(data).trigger('change');;
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });

    }
}