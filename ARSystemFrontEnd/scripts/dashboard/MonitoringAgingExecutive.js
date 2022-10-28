params = {};
_category = "";
PKP = "";

jQuery(document).ready(function () {
    Control.Init();
    Table.Init();
    Control.SetParams();
    Table.Search();
});

var Control = {
    Init: function () {
        Control.BindingCompay();
        Control.BindingOpertor();
        Control.BindingAmountType();
        Control.BindingPeriod();

        $("#txtCaptionSubject").text("ALL OPERATOR TBG - ALL");
        $("#thRangeOD1").text("OD 31 - 60 Days");
        $("#thRangeOD2").text("OD 61 - 90 Days");
        $("#thRangeOD3").text("OD > 90 Days");
        PKP = "PKP";
        //Action
        $("#btSearch").unbind().click(function () {
            Control.SetParams();
            Table.Search();

            _category = $('input[name=rbCategory]:checked').val();

            if (_category == 1) 
            {
                $("#txtCaptionSubject").text("ALL OPERATOR TBG - ALL");
                $("#thRangeOD1").text("OD 31 - 60 Days");
                $("#thRangeOD2").text("OD 61 - 90 Days");
                $("#thRangeOD3").text("OD > 90 Days");
            }
            else if (_category == 2)
            {
                $("#txtCaptionSubject").text("ALL OPERATOR TBG - 30 Days");
                $("#thRangeOD1").text("OD > 30 Days");
            }          
            else 
            {
                $("#txtCaptionSubject").text("ALL OPERATOR TBG - 60 Days");
                $("#thRangeOD2").text("OD > 60 Days");
            }         

        });

        $("#btReset").unbind().click(function () {
            Control.Reset();
        });

        $('#chExcludePKP').on("switchChange.bootstrapSwitch", function (event, state) {
            if ($("#chExcludePKP").bootstrapSwitch("state") == false)
                PKP = "";
            else
                PKP = "PKP";

            Control.SetParams();
            Table.Search();
        });
    },

    BindingCompay: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $("#slCompany").html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slCompany").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
            }
            $("#slCompany").select2({ placeholder: "Select Company", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    
    BindingOpertor: function () {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $("#slOperator").html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slOperator").append("<option value='" + item.OperatorId + "'>" + item.Operator + "</option>");
                })
            }
            $("#slOperator").select2({ placeholder: "Select Operator", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingAmountType: function () {
        $.ajax({
            url: "/api/Dashboard/GetAmountType",
            type: "POST",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null) {
                        $("#slAmountType").append("<option value='" + item.ValueId + "'>" + item.ValueDesc + "</option>");
                    }
                });
            }
            $("#slAmountType").select2({ width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    
    BindingPeriod: function () {
        var date = new Date();
        var today = new Date(date.getFullYear(), date.getMonth(), date.getDate());
        $("#txtPeriod").datepicker({
            endDate: '+0d',
            format: "dd-M-yyyy"
        });
        $("#txtPeriod").datepicker("setDate", today);
    },

    SetParams: function () {
        params = {
            vCompanyID: $("#slCompany").val(),
            vOperatorID: $("#slOperator").val(),
            vAmountType: $("#slAmountType").val(),
            vPeriod: $("#txtPeriod").val(),
            vInvoiceType: $('input[name=rbSummary]:checked').val(),
            vCategory: $('input[name=rbCategory]:checked').val(),
            vPKP: PKP
        }
    },

    Reset: function () {
        $("#slCompany").val(null).trigger("change");
        $("#slOperator").val(null).trigger("change");
        $("#rbAllRent").prop("checked", true);
        $("#rbAll").prop("checked", true);
        Control.BindingAmountType();
        Control.LoadPeriod();
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

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        $(".summary").fadeIn();

        var tblSummaryData = $('#tblSummaryData').DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/Dashboard/MonitoringAgingExecutive/summary",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
                {
                    text: 'Detail', titleAttr: 'Export to Excel', className: 'btn blue btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".blue"));
                        l.start();
                        Table.ExportDetail();
                        l.stop();
                    }
                },
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                //{
                //    extend: 'excelHtml5',
                //    filename: 'MonitoringAgingExecutiveSummary',
                //    text: '<i class="fa fa-file-excel-o"></i>',
                //    titleAttr: 'Export to Excel',
                //    className: 'btn yellow btn-outline',
                //    exportOptions: {
                //        format: {
                //            header: function (data, columnIdx) {
                //                return data.replace(/&gt;/g, 'Diatas');
                //            }
                //        }
                //    },
                //    rows: ':visible'
                //},
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
            "lengthMenu": [[-1], ['All']],
            "destroy": true,
            "columns": [
                {
                    data: "OperatorID", render: function (val, type, full) {
                        return "<b>" + val + "</b>";
                    }
                },
                {
                    data: "AmountCurrent", render: function (val, type, full) {
                        if (full.OperatorID == "% GRAND TOTAL") {
                            return val.format(3) + " %";
                        } else {
                            return val.format(0);
                        }
                    }
                },
                {
                    data: "AmountOD_30", render: function (val, type, full) {
                        if (full.OperatorID == "% GRAND TOTAL") {
                            return val.format(3) + " %";
                        } else {
                            return val.format(0);
                        }
                    }
                },
                {
                    data: "AmountOD_60", render: function (val, type, full) {
                        if (full.OperatorID == "% GRAND TOTAL") {
                            return val.format(3) + " %";
                        } else {
                            return val.format(0);
                        }
                    }
                },
                {
                    data: "AmountOD_90", render: function (val, type, full) {
                        if (full.OperatorID == "% GRAND TOTAL") {
                            return val.format(3) + " %";
                        } else {
                            return val.format(0);
                        }
                    }
                },
                {
                    data: "TotalOS", render: function (val, type, full) {
                        if (full.OperatorID == "% GRAND TOTAL") {
                            return val.format(0) + " %";
                        } else {
                            return val.format(0);
                        }
                    }
                },
                {
                    data: "PercetageODPerOpt", render: function (val, type, full) {
                        if (full.OperatorID == "GRAND TOTAL" || full.OperatorID == "% GRAND TOTAL") {
                            return "-";
                        } else {
                            if (full.PercetageODPerOpt == 100 || full.PercetageODPerOpt == 0)
                                return val.format(0) + " %";
                            else
                                return val.format(3) + " %";
                        }
                    }
                },
                {
                    data: "PercentageODAllOpt", render: function (val, type, full) {
                        if (full.OperatorID == "% GRAND TOTAL") {
                            return "-";
                        }
                        else {
                            if (full.PercentageODAllOpt == 100 || full.PercentageODAllOpt == 0)
                            {
                                return val.format(0) + " %";
                            }
                            else if (full.PercentageODAllOpt >= 99)
                            {
                                return "100 %";
                            }
                            else
                            {
                                return val.format(3) + " %";
                            }
                        }
                    }
                }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fixedColumns": {
                leftColumns: 1
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                }
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (aData.OperatorID == "GRAND TOTAL") {
                    $('td', nRow).css('background-color', '#e8e8e8');
                    //$('td', nRow).css('font-size', '100%');
                    $('td', nRow).css('text-align', 'right');
                    $('td', nRow).css('font-weight', 'bold');
                    $('tr', nRow).css('color', 'black');
                    $('center', nRow).css('color', 'black');
                    $(nRow).find('td:eq(6)').css('text-align', 'center');
                    $(nRow).find('td:eq(7)').css('text-align', 'center');
                }
                if (aData.OperatorID == "% GRAND TOTAL") {
                    $('td', nRow).css('background-color', '#e8e8e8');
                    //$('td', nRow).css('font-size', '100%');
                    $('td', nRow).css('text-align', 'right');
                    $('td', nRow).css('font-weight', 'bold');
                    $('tr', nRow).css('color', 'black');
                    $('center', nRow).css('color', 'black');
                    $(nRow).find('td:eq(1)').css('background-color', '#87CEFA');
                    $(nRow).find('td:eq(2)').css('background-color', '#87CEFA');
                    $(nRow).find('td:eq(3)').css('background-color', '#87CEFA');
                    $(nRow).find('td:eq(4)').css('background-color', '#87CEFA');
                    $(nRow).find('td:eq(6)').css('text-align', 'center');
                    $(nRow).find('td:eq(7)').css('text-align', 'center');
                }
            },
            "columnDefs": [
                    { "targets": [1, 2, 3, 4, 5], "className": "dt-right" },
                    { "targets": [6, 7], "className": "dt-center" },
                    { "targets": [0], "width" : "20%", "className": "dt-left" }
            ],
            "orderin": false,
            "order": []
        });

        var category = $('input[name=rbCategory]:checked').val();

        if (category == 2) {
            tblSummaryData.columns([3, 4]).visible(false).columns.adjust();
        }

        if (category == 3) {
            tblSummaryData.columns([2, 4]).visible(false).columns.adjust();
        }
    },

    ExportDetail: function () {
        Control.SetParams();
        window.location.href = "/Dashboard/MonitoringAgingExecutive/Detail/Export?" + $.param(params);
    },

    Export: function () {
        Control.SetParams();
        window.location.href = "/Dashboard/MonitoringAgingExecutive/Export?" + $.param(params);
    }
}

Number.prototype.format = function (n) {
    //currenct format: IDR Format : 2.020.213,00: param n -> belakang comma
    var re = '\\d(?=(\\d{' + (3) + '})+' + (n > 0 ? '\\D' : '$') + ')',
        num = this.toFixed(Math.max(0, ~~n));

    return (',' ? num.replace('.', ',') : num).replace(new RegExp(re, 'g'), '$&' + ('.' || ','));
};