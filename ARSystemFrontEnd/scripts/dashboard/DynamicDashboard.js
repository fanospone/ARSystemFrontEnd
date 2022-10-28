Data = {};
DataSource = {};
var fsPivotCols = [];
var fsPivotRows = [];
var fsHiddenCols = [];
var fsAggregator = "Count";
var fsRenderer = "Table";
var fsAggregatorVals = "";
var fsConfig = "";
var functionsConfig = "";
var fsDataSourceID;
var fsFiltering;
var fsParamFilter = ["companyid", "customerid", "regionid", "productid", "stipid", "month", "year"];
var fsBtnReset = 0;

// testing branching
jQuery(document).ready(function () {
    BindingData.SelectDataSource();
    BindingData.SelectCompany();
    BindingData.SelectCustomer();
    BindingData.SelectProductType();
    BindingData.SelectRegional();
    BindingData.SelectStipCategory();
    BindingData.BindMonth();
    BindingData.BindYear();
    BindingData.BindingDatePicker();

    
    Control.ListView();
    Button.Init();
    Control.HideSelect();
    $('#panelSummary').show();
    $('#panelDashboardDetail').hide();
    $('.dashboadData').hide();

});
var Control = {
    SaveTemplate: function () {
        var l = Ladda.create(document.querySelector("#btSaveTemplate"));
        l.start();

        var config = $("#output").data("pivotUIOptions");
        var config_copy = JSON.parse(JSON.stringify(config));
        delete config_copy["aggregators"];
        delete config_copy["renderers"];
        fsConfig = JSON.stringify(config_copy);
        DataSave = {};
        DataSave.ID = $('#tbTemplateID').val();
        DataSave.TemplateName = $('#tbTemplateName').val();
        DataSave.TemplateDesc = $('#tbTemplateDesc').val();
        DataSave.JSONConfig = fsConfig;
        DataSave.RendererName = $('#tbRendererName').val();
        DataSave.AggregatorName = $('#tbAggregatorName').val();
        DataSave.AggregatorVals = $('#tbAggregatorVal').val();
        DataSave.PivotColumn = $('#tbPivotCols').val();
        DataSave.PivotRow = $('#tbPivotRows').val();
        DataSave.DataSourceID = $('#sslDataSourceID2').val();

        DataSave.Filtering = Helper.GetValueFilter(fsFiltering);
        var param = { dashboardTemplate: DataSave };
        $.ajax({
            url: "/api/Dashboard/SaveDashboardTemplate",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            cache: false,
            data: JSON.stringify(param),

        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Data has been saved!");
                $('#mdlSaveTemplate').modal('hide');
                l.stop();
                Control.ListView();
                $('#panelSummary').show();
                $('#panelDashboardDetail').hide();
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        });
    },

    ExportPDF: function () {

        var all = $(".pvtRendererArea").map(function () {
            return this.innerHTML;
        }).get();
        var html = all.join();
        var pageSize = $('#slPageSize').val();
        var pageOrientation = $('#slOrientation').val();
        var params = { htmlElements: html };
        $.ajax({
            url: "/api/Dashboard/DashboardToPDF",
            type: "POST",
            datatype: "json",
            data: params,
            success: function (x) {
                window.location.href = "/Dashboard/DashboardToPDF?pageSize=" + pageSize + "&pageOrientation=" + pageOrientation;
            }
        });
    },
    Download: function () {
        window.location.href = "/dashboard/DashboardToExcel?DataSourceID=" + fsDataSourceID + "";
    },
    LoadFormDashboard: function (id) {
        $('.dashboadData').hide();
        $('#panelSummary').fadeOut(600);
        $('#panelDashboardDetail').fadeIn(800);

        fsPivotCols = [];
        fsPivotRows = [];
        fsConfig = "";
        fsAggregator = "Count";
        fsRenderer = "Table";
        fsAggregatorVals = "";
        $('#tbPivotCols').val('');
        $('#tbPivotRows').val('');
        $('#tbRendererName').val('');
        $('#tbAggregatorName').val('');
        $('#tbAggregatorVal').val('');
        $('#tbTemplateName').val('');
        $('#tbTemplateDesc').val('');
        $('#tbDataSourceID').val('');
        $('#tbTemplateID').val('');
        $('#spDashboardName').text('Search Result');

        if (id != null && id != "") {
            $('#sslDataSourceID2').val(id).trigger('change');
            $('#sslDataSourceID2').attr("disabled", true);
            Control.HideSelect();
            $.each(DataSource, function (i, item) {
                if (item.ID == $("#sslDataSourceID2").val()) {
                    fsFiltering = item.ParamFilter;
                    if (fsFiltering != null && fsFiltering != "") {
                        $('.formFilter').html(GenerateFilter(fsFiltering));
                        //var filtering = fsFiltering.split(",");
                        //for (var i = 0; i < filtering.length; i++) {
                        //    $("#" + filtering[i].toLowerCase()).val('').trigger('change');
                        //    $("." + filtering[i].toLowerCase()).show();
                        //}
                        filtering = JSON.parse(fsFiltering);
                        $.each(filtering, function (key, value) {
                            $("#" + key.toLowerCase()).val('').trigger('change');
                            $("." + key.toLowerCase()).show();
                        });
                    }

                    fsHiddenCols = [];
                    if (item.ShowColumn != null && item.ShowColumn != "") {
                        fsHiddenCols = item.ShowColumn.split(',');
                        fsHiddenCols.push("Row Index");
                    }
                }
            });
        }
        else {
            $('#sslDataSourceID2').val('').trigger('change');
            $('#sslDataSourceID2').attr("disabled", false);
            $('.formFilter').html('');
        }
        BindingData.BindingDatePicker();
    },
    LoadTemplate: function (data) {
        dataID = [];
        Data = data.data;
        if (Common.CheckError.List(data.data)) {
            $.each(data.data, function (i, item) {
                dataID.push(item.DataSource);
            });
        };

        var dataTemplate = [];
        dataTemplate = dataID.filter(Control.onlyUnique);
        var htmlElements = "";
        htmlElements += "<div class='row'>";
        var count = 0;
        var idTemp = 0;
        for (var a = 0; a < dataTemplate.length; a++) {
            if (count == 3) {
                htmlElements += "</div>";
                htmlElements += "<div class='row'>";
                count = 0;
            }
            idTemp++;
            var total = 0;
            var idDashboad;
            $.each(data.data, function (ii, item) {

                if (item.DataSource == dataTemplate[a]) {
                    total++;
                    idDashboad = item.DataSourceID;
                }
            });
            htmlElements += "<div class='col-lg-4'>" +
                 "<div class='portlet light portlet-fit bordered'>" +
                     "<div class='mt-element-list'>" +
                         "<div class='mt-list-container list-todo'>" +
                             "<div class='list-todo-line red'></div>" +
                             "<ul>" +
                                 "<li class='mt-list-item'>" +
                                     "<div class='list-todo-icon bg-white font-blue-steel'>" +
                                         "<i class='fa fa-database'></i>" +
                                     "</div>" +
                                     "<div class='list-todo-item blue-steel'>" +
                                         "<a class='list-toggle-container font-white collapsed' data-toggle='collapse' href='#task-" + idTemp + "' aria-expanded='false'>" +
                                             "<div class='list-toggle done uppercase'>" +
                                                 "<div class='list-toggle-title bold'>" + dataTemplate[a] + "</div>" +
                                                 "<div class='badge badge-default pull-right bold'>" + total + "</div>" +
                                             "</div>" +
                                           "</a>" +
                                         "<div class='task-list panel-collapse collapse' id='task-" + idTemp + "' aria-expanded='false' style=''>" +
                                             "<ul>";

            $.each(data.data, function (ii, item) {
                if (item.DataSource == dataTemplate[a]) {
                    htmlElements += "<li class='task-list-item done'>" +
                                                    "<div class='task-icon'>" +
                                                        "<a href='javascript:LoadDashboard(" + item.ID + ")''>" +
                                                           " <i class='fa fa-table'></i>" +
                                                        "</a>" +
                                                   " </div>" +
                                                   " <div class='task-status'>" +
                                                        "<a class='pending' href='javascript:;'>" +
                                                           " <i class='fa fa-close'></i>" +
                                                       " </a>" +
                                                  "  </div>" +
                                                   " <div class='task-content'>" +
                                                      "  <h4 class='uppercase bold'>" +
                                                           " <a href='javascript:LoadDashboard(" + item.ID + ")'; >" + item.TemplateName + "</a>" +
                                                       " </h4>" +
                                                      "  <p>" + item.TemplateDesc + "" +
                                                   " </div>" +
                                               " </li>";
                }

            });
            htmlElements += "</ul>" +
                                            "<div class='task-footer bg-grey'>" +
                                                "<div class='row'>" +
                                                   " <div class='col-xs-6'>" +
                                                    "</div>" +
                                                   " <div class='col-xs-6'>" +
                                                        "<a class='task-add' href='javascript:LoadDashbordForm(" + idDashboad + ")';>" +
                                                           " <i class='fa fa-plus'></i>" +
                                                      "  </a>" +
                                                   " </div>" +
                                              "  </div>" +
                                           " </div>" +
                                       " </div>" +
                                  "  </div>" +
                              "  </li>" +
                          "  </ul>" +
                       " </div>" +
                   " </div>" +
              "  </div>" +
          "  </div>";
            count++;
        }
        htmlElements += "</div>";

        $("#dashboardTemlpate").html(htmlElements);
        BindingData.BindingDatePicker();

    },
    ListView: function () {
        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();
        var params = {
            TemplateName: $('#stbTemplateName').val(),
            DataSourceID: $('#sslDataSourceID').val()
        };

        $.ajax({
            url: "/Api/Dashboard/GetDashboardTemplate",
            type: "POST",
            datatype: "json",
            async: false,
            data: params,
        })
     .done(function (data, textStatus, jqXHR) {
         Control.LoadTemplate(data);
         l.stop();
     })
     .fail(function (jqXHR, textStatus, errorThrown) {
         Common.Alert.Error(errorThrown);
         l.stop();
     });

    },
    onlyUnique: function (value, index, self) {
        return self.indexOf(value) === index;
    },
    HideSelect: function () {
        $('.companyid').hide();
        $('.customerid').hide();
        $('.productid').hide();
        $('.stipid').hide();
        $('.regionid').hide();
        $('.month').hide();
        $('.year').hide();
    },

}

var BindingData = {
    SelectDataSource: function () {
        var id = "#sslDataSourceID";
        var id2 = "#sslDataSourceID2";
        $.ajax({
            url: "/api/Dashboard/GetDataSourceDashboard",
            type: "GET",
            async: false
        })
      .done(function (data, textStatus, jqXHR) {
          $(id).html("<option></option>");
          $(id2).html("<option></option>");
          if (Common.CheckError.List(data)) {
              DataSource = data;
              $.each(data, function (i, item) {
                  $(id).append("<option value='" + $.trim(item.ID) + "'>" + item.DataSourceName + "</option>");
                  $(id2).append("<option value='" + $.trim(item.ID) + "'>" + item.DataSourceName + "</option>");
              });
          }
          $(id).select2({ placeholder: "Select Data Source ", width: null });
          $(id2).select2({ placeholder: "Select Data Source ", width: null });
      })
      .fail(function (jqXHR, textStatus, errorThrown) {
          Common.Alert.Error(errorThrown);
      });
    },

    SelectCompany: function () {
        var id = "#companyid";
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>");
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.CompanyId) + "'>" + item.Company + "</option>");
                });
            }
            $(id).select2({ placeholder: "Select Company Name", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    SelectCustomer: function () {
        var id = "#customerid";
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>");
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.OperatorId) + "'>" + item.Operator + "</option>");
                });
            }
            $(id).select2({ placeholder: "Select Customer", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    SelectProductType: function () {
        var id = "#productid";
        $.ajax({
            url: "/api/MstDataSource/TenantType",
            type: "GET"

        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>");
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.Value) + "'>" + item.Text + "</option>");
                });
            }
            $(id).select2({ placeholder: "Select Product", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    SelectRegional: function () {
        var id = "#regionid";
        $.ajax({
            url: "/api/MstDataSource/Regional",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>");
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.RegionalId) + "'>" + item.Regional + "</option>");
                });
            }
            $(id).select2({ placeholder: "Select Regional", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    SelectStipCategory: function () {
        var id = "#stipid";
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
                });
            }
            $(id).select2({ placeholder: "Select STIP", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindYear: function () {
        var start_year = new Date().getFullYear();
        var id = "#year";
        var yearNow = new Date();
        for (var i = start_year - 10; i < start_year ; i++) {
            $(id).append('<option value="' + i + '">  ' + i + '  </option>');
        }

        for (var i = start_year ; i < start_year + 20 ; i++) {
            $(id).append('<option value="' + i + '">  ' + i + '  </option>');
        }

        $(id).select2({ placeholder: "Select Year", width: null });
        $(id).val(yearNow.getFullYear()).trigger('change');
    },
    BindMonth: function () {
        var month = 1;
        var id = "#month";
        var dt = new Date();

        for (var i = 1; i <= 12 ; i++) {
            $(id).append('<option value="' + i + '">  ' + i + '  </option>');
        }

        $(id).select2({ placeholder: "Select month", width: null });
        $(id).val(dt.getMonth() + 1).trigger('change');
    },

    BindingDatePicker: function () {
        $(".datepicker").datepicker({
            format: "dd M yyyy"

        });
    },
}

var Table = {
    PivotTable: function () {
        $(".dashboadData").hide();
        $("#output").pivotUI([], {}, true);
        var l = Ladda.create(document.querySelector("#btSearchDashboard"));
        l.start();
        var paramJSON;
        if (fsFiltering != null && fsFiltering != "") {
            paramJSON = "{";
            var filtering = JSON.parse(fsFiltering);
            var i = 1;
            var count = Object.keys(filtering).length;
            $.each(filtering, function (key, value) {
                var keyVal = $("#" + key.toLowerCase()).val() == null ? "" : $("#" + key.toLowerCase()).val();
                if (i == count) {
                    paramJSON += '"' + key + '":"' + keyVal + '"';
                }
                else {
                    paramJSON += '"' + key + '":"' + keyVal + '",';
                }

                i++;
            });
            paramJSON += "}";
        }

        var params = {
            DataSourceID: fsDataSourceID,
            ParamJSON: paramJSON,
        };
        $.ajax({
            url: "/api/Dashboard/GetDashboardData",
            type: "POST",
            datatype: "json",
            data: params,
            success: function (listdata) {
                if (listdata != "null" && listdata != "[]") {
                    if (fsConfig == "") {
                        $("#output").pivotUI(JSON.parse(listdata), functionsConfig, true);
                        //$("#output").pivotUI(listdata, functionsConfig, true);
                    } else {
                        $("#output").pivotUI(JSON.parse(listdata), JSON.parse(fsConfig), true);
                        //$("#output").pivotUI(listdata, JSON.parse(fsConfig), true);
                    }
                    $("#btExportPDF").show();
                    $("#btShowMdlSaveTemplate").show();
                    $("#btnDownload").show();
                } else {
                    $("#output").html('<i style="color:red">No data display!</i>');
                    $("#btExportPDF").hide();
                    $("#btShowMdlSaveTemplate").hide();
                    $("#btnDownload").hide();
                }
                $(".dashboadData").fadeIn(300);
                l.stop();

            },
            error: function (response) {
                l.stop();
            }
        });

    },
}
var Helper = {
    SetValueForm: function (filter) {
        if (filter != null && filter != "") {
            filter = JSON.parse(filter);
            $.each(filter, function (key, value) {
                if (fsParamFilter.includes(key.toLowerCase())) {
                    $("#" + key.toLowerCase()).val(value).trigger('change');
                } else {
                    $("#" + key.toLowerCase()).val(value);

                }
            });
        }
    },
    GetValueFilter: function (data) {
        var result = "";
        if (data != null && data != "") {
            var paramJSON = "{";
            var data = JSON.parse(data);
            var i = 1;
            var count = Object.keys(data).length;
            $.each(data, function (key, value) {
                if (i == count)
                    paramJSON += '"' + key + '":"' + $("#" + key.toLowerCase()).val() + '"';
                else
                    paramJSON += '"' + key + '":"' + $("#" + key.toLowerCase()).val() + '",';
                i++;

            });
            paramJSON += "}";
            result = paramJSON;
        }
        return result
    },
    CemelToRegular: function (value) {
    var abs =    value.replace(/([A-Z])/g, ' $1').replace(/^./, function (str) {
            return str.toUpperCase();
    });
    var abs = abs.split(" ");
    var a = "";
    for (var i = 0; i < abs.length; i++) {

        if (abs[i].length == 1)
            a += abs[i];
        else
            a += " " + abs[i] + " ";
    }
    return $.trim(a);
    },
}
var Button = {
    Init: function () {

        $('#btnAddNewTemplate').unbind().click(function () {
            fsBtnReset = 1;
            Control.LoadFormDashboard("");
            Control.HideSelect();
			 $('#collapseTools').html('');
			 $('#collapseTools').append('<a href="" class="collapse"> </a>');
	
            $("#sslDataSourceID2").change(function () {
                $.each(DataSource, function (i, item) {
                    if (item.ID == $("#sslDataSourceID2").val()) {
                        fsFiltering = item.ParamFilter;
                        if (fsFiltering != null && fsFiltering != "") {
                            $('.formFilter').html(GenerateFilter(fsFiltering));
                            var filtering = JSON.parse(fsFiltering);
                            $.each(filtering, function (key, value) {
                                if (fsParamFilter.includes(key.toLowerCase())) {
                                    $("#" + key.toLowerCase()).val('').trigger('change');
                                    $("." + key.toLowerCase()).show();
                                } else {
                                    $("#" + key.toLowerCase()).val('');
                                    $("." + key.toLowerCase()).show();
                                }
                            });
                        }
                    }
                });

                BindingData.BindingDatePicker();
            });

        });

        $('#btBack').unbind().click(function () {

            $('#panelSummary').fadeIn(1000);
            $('#panelDashboardDetail').fadeOut(300);
            Control.ListView();
        });

        $("#btExportPDF").unbind().click(function () {
            $('#mdlExportPDF').modal('show');
        });

        $("#btShowMdlSaveTemplate").unbind().click(function () {
            $('#tbRendererName').val($("#output").data("pivotUIOptions").rendererName);
            $('#tbAggregatorName').val($("#output").data("pivotUIOptions").aggregatorName);
            $('#tbAggregatorVal').val($("#output").data("pivotUIOptions").vals);
            var pvtRows = $("#output").data("pivotUIOptions").rows;
            var pvtCols = $("#output").data("pivotUIOptions").cols;
            var Rows = "";
            var Cols = "";
            for (var i = 0; i < pvtRows.length; i++) {
                if (i < pvtRows.length - 1)
                    Rows += pvtRows[i] + ',';
                else
                    Rows += pvtRows[i];
            }
            for (var i = 0; i < pvtCols.length; i++) {
                if (i < pvtCols.length - 1)
                    Cols += pvtCols[i] + ',';
                else
                    Cols += pvtCols[i];
            }
            $('#tbPivotCols').val(Cols);
            $('#tbPivotRows').val(Rows);
            $('#mdlSaveTemplate').modal('show');
        });

        $("#btExport").unbind().click(function () {
            Control.ExportPDF();
            $('#mdlExportPDF').modal('hide');
        });

        $("#btSaveTemplate").unbind().click(function () {
            Control.SaveTemplate();
            $('#mdlSaveTemplate').modal('hide');
        });
        $('#btSearchDashboard').unbind().click(function () {
            if ($('#sslDataSourceID2').val() != "") {
              
                functionsConfig = {
                    renderers: $.extend(
                             $.pivotUtilities.renderers,
                             $.pivotUtilities.c3_renderers,
                             $.pivotUtilities.export_renderers
                             ),
                    rendererName: fsRenderer,
                    aggregatorName: fsAggregator,
                    vals: [fsAggregatorVals],
                   // hiddenAttributes: fsHiddenCols //["RowIndex"]
                };
                fsDataSourceID = $("#sslDataSourceID2").val();
                Table.PivotTable();
            }

        });

        $("#btSearch").unbind().click(function () {
            //Table.SummaryDashboard();
            Control.ListView();
        });

        $("#btReset").unbind().click(function () {
            $('#stbTemplateName').val('');
            $('#stbRendererName').val('');
            $('#stbAggregatorName').val('');
			$('#sslDataSourceID').val('').trigger('change');


        });
        $("#btResetDashboard").unbind().click(function () {
            if (fsBtnReset = 1)
                $('#sslDataSourceID').val('').trigger('change');

            if (fsFiltering != null && fsFiltering != "") {
                $('.formFilter').html(GenerateFilter(fsFiltering));
                var Filtering = JSON.parse(fsFiltering);
                $.each(Filtering, function (key, value) {
                    if (fsParamFilter.includes(key.toLowerCase())) {
                        $("#" + key.toLowerCase()).val('').trigger('change');
                    } else {
                        $("#" + key.toLowerCase()).val('');
                    }
                });
            }
        });

        $('#btnDownload').unbind().click(function () {
            Control.Download();
        });

        $(".btViewData").unbind().click(function () {
            alert("test");
        });

    },
}
function LoadDashboard(id) {
    $('.formFilter').html('');
    Control.HideSelect();

	$('#collapseTools').html('');
	$('#collapseTools').append('<a href="" class="expand"> </a>');
	$('#formSearchDetail').css("display","none");
    $.each(Data, function (i, item) {
        if (item.ID == id) {
            fsBtnReset = 0;
            $('#sslDataSourceID2').val(item.DataSourceID).trigger('change');
            fsConfig = item.JSONConfig;
            fsDataSourceID = item.DataSourceID;
            fsAggregator = item.AggregatorName;
            fsRenderer = item.RendererName;
            fsAggregatorVals = item.AggregatorVals;
            fsPivotCols = [];
            fsPivotRows = [];

            var cols = item.PivotColumn.split(",");
            for (var i = 0; i < cols.length; i++) {
                fsPivotCols.push(cols[i]);
            }
            var rows = item.PivotRow.split(",");
            for (var i = 0; i < rows.length; i++) {
                fsPivotRows.push(rows[i]);
            }
            $('#spDashboardName').text(item.TemplateName);
            $('#tbDataSourceID').val(item.DataSourceID);
            $('#tbTemplateID').val(item.ID);
            $('#tbTemplateName').val(item.TemplateName);
            $('#spDashboardName').text(item.TemplateName)
            $('#tbTemplateDesc').val(item.TemplateDesc);
            fsFiltering = '';
            fsFiltering = item.ParamFilter;
            if (fsFiltering != null && fsFiltering != "") {
                $('.formFilter').html(GenerateFilter(fsFiltering));
                var Filtering = JSON.parse(fsFiltering);
                $.each(Filtering, function (key, value) {
                    if (fsParamFilter.includes(key.toLowerCase())) {
                        $("." + key.toLowerCase()).show();
                    }
                });
            }
            fsHiddenCols = [];
            if (item.ShowColumn != null && item.ShowColumn != "") {
                fsHiddenCols = item.ShowColumn.split(',');
                fsHiddenCols.push("Row Index");
            }

            Helper.SetValueForm(item.Filtering);
        }
    });
    $('#panelSummary').fadeOut(600);
    $('#panelDashboardDetail').fadeIn(800);
    $('#sslDataSourceID2').attr("disabled", true);
    $("#btSearchDashboard").trigger('click');
    BindingData.BindingDatePicker();
}
function LoadDashbordForm(id) {
    var idDashoard;
    $.each(Data, function (i, item) {
        if (item.DataSourceID == id) {
            idDashoard = item.DataSourceID;
            return true;
        }
    });
    Control.LoadFormDashboard(idDashoard);
}
function GenerateFilter(data) {
    var html = data.split(",");
    var a = "";

    data = JSON.parse(data);
    $.each(data, function (key, value) {
        if (!fsParamFilter.includes(key.toLowerCase())) {
            a += " <div class='form-group " + key.toLowerCase() + "'>";
            a += "<label class='col-md-4 control-label'>" + Helper.CemelToRegular(key) + "</label>";
            a += " <div class='col-md-8'>";

            if (value.toLowerCase() == "date" || value.toLowerCase() == "datetime")
                a += "<input type='text' class='form-control datepicker' id='" + key.toLowerCase() + "' readonly />";
            else
                a += "<input type='text' class='form-control' id='" + key.toLowerCase() + "' />";

            a += "</div>";
            a += "</div>";
        }

    });
    return a;
}

