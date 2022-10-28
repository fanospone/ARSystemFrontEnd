Data = {};

jQuery(document).ready(function () {
    Control.LoadYear();
    Control.LoadMonth();
    Control.LoadCurrency();
    Table.Init();
    Table.Search();

    //panel Summary
    $("#formSearch").submit(function (e) {
        Table.Search();
        e.preventDefault();
    });

    $("#btSearch").unbind().click(function () {
        Table.Search();
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
    });

    $("#btBackSoNumber").unbind().click(function (e) {
        $(".panelSearchResult").fadeIn();
        $("#pnlDetailSoNumber").hide();
        $("#rowGraph").fadeIn();

        if ($("#slMonth").val() != null && $("#slMonth").val() != "0") {
            $(".weekly").fadeIn();
            $(".monthly").hide();
        } else {
            $(".weekly").hide();
            $(".monthly").fadeIn();
        }
    });
});

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
        var columns = [];
        var tableSelector = "#tblMonthlyData";
        var currency = $("#slCurrency").val() == null ? "IDR" : $("#slCurrency").val();
        if(currency == "USD")
            $(".caption-subject").html("Search Result (in " + currency + ")");
        else
            $(".caption-subject").html("Search Result (in Million of " + currency + ")");
        if ($("#slMonth").val() == null || $("#slMonth").val() == "0") {
            columns = [
                { data: "Category" },
                {
                    data: "JanCurr", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 1, 0, "Curr", currency);
                    }
                },
                {
                    data: "JanOD", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 1, 0, "OD", currency);
                    }
                },
                {
                    data: "FebCurr", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 2, 0, "Curr", currency);
                    }
                },
                {
                    data: "FebOD", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 2, 0, "OD", currency);
                    }
                },
                {
                    data: "MarCurr", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 3, 0, "Curr", currency);
                    }
                },
                {
                    data: "MarOD", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 3, 0, "OD", currency);
                    }
                },
                {
                    data: "AprCurr", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 4, 0, "Curr", currency);
                    }
                },
                {
                    data: "AprOD", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 4, 0, "OD", currency);
                    }
                },
                {
                    data: "MayCurr", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 5, 0, "Curr", currency);
                    }
                },
                {
                    data: "MayOD", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 5, 0, "OD", currency);
                    }
                },
                {
                    data: "JunCurr", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 6, 0, "Curr", currency);
                    }
                },
                {
                    data: "JunOD", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 6, 0, "OD", currency);
                    }
                },
                {
                    data: "JulCurr", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 7, 0, "Curr", currency);
                    }
                },
                {
                    data: "JulOD", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 7, 0, "OD", currency);
                    }
                },
                {
                    data: "AugCurr", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 8, 0, "Curr", currency);
                    }
                },
                {
                    data: "AugOD", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 8, 0, "OD", currency);
                    }
                },
                {
                    data: "SepCurr", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 9, 0, "Curr", currency);
                    }
                },
                {
                    data: "SepOD", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 9, 0, "OD", currency);
                    }
                },
                {
                    data: "OctCurr", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 10, 0, "Curr", currency);
                    }
                },
                {
                    data: "OctOD", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 10, 0, "OD", currency);
                    }
                },
                {
                    data: "NovCurr", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 11, 0, "Curr", currency);
                    }
                },
                {
                    data: "NovOD", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 11, 0, "OD", currency);
                    }
                },
                {
                    data: "DecCurr", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 12, 0, "Curr", currency);
                    }
                },
                {
                    data: "DecOD", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 12, 0, "OD", currency);
                    }
                },
            ]
            selector = "#tblMonthlyData";
            $(".monthly").fadeIn();
            $(".weekly").hide();
        } else {
            $("#thMonthName").html(Helper.GetMonthName($("#slMonth").val()));
            columns = [
                { data: "Category" },
                {
                    data: "Week1Curr", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, $("#slMonth").val(), 1, "Curr", currency);
                    }
                },
                {
                    data: "Week1OD", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, $("#slMonth").val(), 1, "OD", currency);
                    }
                },
                {
                    data: "Week2Curr", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, $("#slMonth").val(), 2, "Curr", currency);
                    }
                },
                {
                    data: "Week2OD", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, $("#slMonth").val(), 2, "OD", currency);
                    }
                },
                {
                    data: "Week3Curr", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, $("#slMonth").val(), 3, "Curr", currency);
                    }
                },
                {
                    data: "Week3OD", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, $("#slMonth").val(), 3, "OD", currency);
                    }
                },
                {
                    data: "Week4Curr", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, $("#slMonth").val(), 4, "Curr", currency);
                    }
                },
                {
                    data: "Week4OD", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, $("#slMonth").val(), 4, "OD", currency);
                    }
                },
                {
                    data: "Week5Curr", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, $("#slMonth").val(), 5, "Curr", currency);
                    }
                },
                {
                    data: "Week5OD", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, $("#slMonth").val(), 5, "OD", currency);
                    }
                },
                {
                    data: "Week6Curr", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, $("#slMonth").val(), 6, "Curr", currency);
                    }
                },
                {
                    data: "Week6OD", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, $("#slMonth").val(), 6, "OD", currency);
                    }
                },
            ];
            selector = "#tblWeeklyData";
            $(".weekly").fadeIn();
            $(".monthly").hide();
        }

        Table.Draw(selector, columns);
        $("#pnlDetailSoNumber").hide();

        $(selector + " tbody").unbind().on("click", "a.btDetail", function (e) {
            var table = $(selector).DataTable();
            var params = {
                Year: $(this).attr('year'),
                Month: $(this).attr('month'),
                Week: $(this).attr('week'),
                LeadTime: $(this).attr('leadTime'),
                Currency: $(this).attr('currency')
            };

            TableDetail.Search(params);
            $(".panelSearchResult").hide();
            $("#pnlDetailSoNumber").fadeIn();
            e.preventDefault();
        });

        Graph.Init();
    },
    Reset: function () {
        $("#slYear").val("").trigger("change");
        $("#slMonth").val("0").trigger("change");
    },
    Export: function () {
        var year = $("#slYear").val();
        var month = $("#slMonth").val();

        window.location.href = "/Dashboard/InvoiceTowerSummary/Export?month=" + month + "&year=" + year;
    },
    Draw: function (selector, columns) {

        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var params = {
            Year: $("#slYear").val(),
            Month: $("#slMonth").val(),
            Currency: $("#slCurrency").val()
        };

        var tblSummaryData = $(selector).DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/Dashboard/InvoiceTowerSummary",
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
            "lengthMenu": [[-1], ['All']],
            "destroy": true,
            "columns": columns,
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    //$(".panelSearchResult").fadeIn();
                }
                Helper.MergeColumns(selector);
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            'ordering': false,
            'order': []
        });
    }
}

var TableDetail = {
    Search: function (params) {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        $.ajax({
            url: "/api/Dashboard/Detail/SoNumber",
            type: "POST",
            dataType: "JSON",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            var tblSummaryData = $("#tblDetailSoNumber").DataTable({
                "proccessing": true,
                "serverSide": false,
                "language": {
                    "emptyTable": "No data available in table",
                },
                data: data.data,
                buttons: [
                    { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                    {
                        text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                            var l = Ladda.create(document.querySelector(".yellow"));
                            l.start();
                            TableDetail.Export(params)
                            l.stop();
                        }
                    },
                    { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
                ],
                "filter": false,
                "lengthMenu": [[5, 10, 25], ['5', '10', '25']],
                "destroy": true,
                "columns": [
                    { data: "SoNumber" },
                    { data: "Company" },
                    { data: "Operator" },
                    { data: "TenantType" },
                    { data: "Start" },
                    { data: "End" },
                    { data: "EGF" },
                    {
                        data: "RevPerMonth", render: function (val, type, full) {
                            return Helper.GetShortNumberFormat(val);
                        }
                    },
                    {
                        data: "AmountInv", render: function (val, type, full) {
                            return Helper.GetShortNumberFormat(val);
                        }
                    }
                ],
                "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                "fnDrawCallback": function () {
                    //if (Common.CheckError.List(tblSummaryData.data())) {
                    //    $(".panelSearchBegin").hide();
                    //    $(".panelSearchResult").hide();
                    //    $("#pnlDetailSoNumber").fadeIn();
                    //}
                    l.stop();
                },
                'aoColumnDefs': [{
                    'bSortable': false,
                    'aTargets': [0, 1, 2, 3, 4, 5, 6, 7]
                }],
                'order': []
            });
        });

        $("#rowGraph").hide();
    },
    Export: function (params) {
        var year = params.Year;
        var month = params.Month;
        var week = params.Week;
        var leadTime = params.LeadTime;

        window.location.href = "/Dashboard/DetailSoNumber/Export?month=" + month + "&year=" + year + "&week=" + week + "&leadTime=" + leadTime;
    }
}

var Control = {
    LoadYear: function () {
        $.ajax({
            url: "/api/Dashboard/GetYear",
            type: "POST"
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null) {
                        $("#slYear").append("<option value='" + item.ValueId + "'>" + item.ValueDesc + "</option>");
                    }
                });
            }
            $("#slYear").select2({ width: null });
        });
    },
    LoadMonth: function () {
        $.ajax({
            url: "/api/Dashboard/GetMonth",
            type: "POST"
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null) {
                        $("#slMonth").append("<option value='" + item.ValueId + "'>" + item.ValueDesc + "</option>");
                    }
                });
            }
            $("#slMonth").select2({ width: null });
        });
    },
    LoadCurrency: function () {
        var currencies = [{
            ValueId: "IDR",
            ValueDesc: "IDR"
        }, {
            ValueId: "USD",
            ValueDesc: "USD"
        }];
        console.log(currencies);
        $.each(currencies, function (i, item) {
            $("#slCurrency").append("<option value='"+ item.ValueId +"'>"+ item.ValueDesc +"</option>");
        });
        $("#slCurrency").select2({ width: null });
    }
}

var Helper = {
    FormatPercentage: function (val) {
        var percentage = Common.Format.CommaSeparation(val + "");
        return percentage + " %";
    },
    FormatDuration: function (val) {
        var duration = parseFloat(val + "").toFixed(1);
        return duration + " days";
    },
    RenderLink: function (val, full, month, week, leadTime, currency) {
        var year = $("#slYear").val();

        if (year == null) {
            $("#slYear").val($('#slYear option:first-child').val()).trigger('change');
            year = $("#slYear").val();
        }

        if (full.Category != "LT%" && full.Category != "LT Days") {
            if (full.Category == "Grand Total" || full.Category == "Amount")
                return "<a class='btDetail' year='" + year + "' month='" + month + "' week='" + week + "' currency='"+ currency +"'>" + Helper.GetShortNumberFormat(val) + "</a>";
            return "<a class='btDetail' year='" + year + "' month='" + month + "' week='" + week + "' currency='" + currency + "' leadTime='" + leadTime + "'>" + val + "</a>";
        } else {
            if (full.Category == "LT Days") {
                return val + " Day";
            } else {
                return val;
            }
        }
    },
    GetMonthName: function (val) {
        var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
        return months[parseInt(val) - 1];
    },
    MergeColumns: function (tableSelector) {
        var rowNumber = [2, 6]; // Index of the datatable row body, starting from 1
        var colStart = 2; // 
        var colEnd = 25;

        $.each(rowNumber, function (index, item) {
            for (var i = colStart ; i <= colEnd ; i++) {
                if(i%2 == 0)
                    $(tableSelector + " tr:nth-child(" + item + ") td:nth-child(" + i + ")").attr('colspan', '2').addClass('text-center');
                else
                    $(tableSelector + " tr:nth-child(" + item + ") td:nth-child(" + i + ")").attr('style', 'display:none;');
            }
        });
    },
    GetShortNumberFormat(val) {
        var divider = 1000000.00;
        var currency = $("#slCurrency").val() == null ? "IDR" : $("#slCurrency").val();
        if (currency == "USD")
            divider = 1.00;
        val = parseFloat(val.toString().replace(/,/g, '')) / divider;
        return Common.Format.CommaSeparation(val);
    }
}

var Graph = {
    Init: function () {
        var params = {
            Year: $("#slYear").val(),
            Month: $("#slMonth").val()
        }

        $.ajax({
            url: "/api/Dashboard/GraphLeadTime",
            type: "POST",
            data: params,
            dataType: "json"
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                
                var GraphDataProvider = [];

                if (params.Month == null || params.Month == "0") {
                    GraphDataProvider = [
                        {
                            "month": "Jan",
                            "leadTime": data.LeadTimeJan,
                            "target": data.TargetMonth1
                        },
                        {
                            "month": "Feb",
                            "leadTime": data.LeadTimeFeb,
                            "target": data.TargetMonth2
                        },
                        {
                            "month": "Mar",
                            "leadTime": data.LeadTimeMar,
                            "target": data.TargetMonth3
                        },
                        {
                            "month": "Apr",
                            "leadTime": data.LeadTimeApr,
                            "target": data.TargetMonth4
                        },
                        {
                            "month": "May",
                            "leadTime": data.LeadTimeMay,
                            "target": data.TargetMonth5
                        },
                        {
                            "month": "Jun",
                            "leadTime": data.LeadTimeJun,
                            "target": data.TargetMonth6
                        },
                        {
                            "month": "Jul",
                            "leadTime": data.LeadTimeJul,
                            "target": data.TargetMonth7
                        },
                        {
                            "month": "Aug",
                            "leadTime": data.LeadTimeAug,
                            "target": data.TargetMonth8
                        },
                        {
                            "month": "Sep",
                            "leadTime": data.LeadTimeSep,
                            "target": data.TargetMonth9
                        },
                        {
                            "month": "Oct",
                            "leadTime": data.LeadTimeOct,
                            "target": data.TargetMonth10
                        },
                        {
                            "month": "Nov",
                            "leadTime": data.LeadTimeNov,
                            "target": data.TargetMonth11
                        },
                        {
                            "month": "Dec",
                            "leadTime": data.LeadTimeDec,
                            "target": data.TargetMonth12
                        },
                    ];
                } else {
                    GraphDataProvider = [
                        {
                            "month": "Week 1",
                            "leadTime": data.LeadTimeWeek1,
                            "target": data.TargetWeek1
                        },
                        {
                            "month": "Week 2",
                            "leadTime": data.LeadTimeWeek2,
                            "target": data.TargetWeek2
                        },
                        {
                            "month": "Week 3",
                            "leadTime": data.LeadTimeWeek3,
                            "target": data.TargetWeek3
                        },
                        {
                            "month": "Week 4",
                            "leadTime": data.LeadTimeWeek4,
                            "target": data.TargetWeek4
                        },
                        {
                            "month": "Week 5",
                            "leadTime": data.LeadTimeWeek5,
                            "target": data.TargetWeek5
                        },
                        {
                            "month": "Week 6",
                            "leadTime": data.LeadTimeWeek6,
                            "target": data.TargetWeek6
                        }
                    ];
                }

                Graph.Render(GraphDataProvider);
            }
        });
    },
    Render: function (chartData) {
        var chart = AmCharts.makeChart("divChart", {
            "type": "serial",
            "theme": "light",
            "legend": {
                "useGraphSettings": true
            },
            "dataProvider": chartData,
            "synchronizeGrid": true,
            "valueAxes": [{
                "id": "v1",
                "axisColor": "#FF6600",
                "axisThickness": 2,
                "axisAlpha": 1,
                "position": "left"
            }, {
                "id": "v2",
                "axisColor": "#FCD202",
                "axisThickness": 2,
                "axisAlpha": 1,
                "position": "right"
            }],
            "graphs": [{
                "valueAxis": "v1",
                "lineColor": "#FF6600",
                "bullet": "round",
                "bulletBorderThickness": 1,
                "hideBulletsCount": 30,
                "title": "Lead Time",
                "valueField": "leadTime",
                "fillAlphas": 0
            }, {
                "valueAxis": "v2",
                "lineColor": "#FCD202",
                "bullet": "square",
                "bulletBorderThickness": 1,
                "hideBulletsCount": 30,
                "title": "Target",
                "valueField": "target",
                "fillAlphas": 0
            }],
            "chartScrollbar": {},
            "chartCursor": {
                "cursorPosition": "mouse"
            },
            "categoryField": "month",
            "categoryAxis": {
                "parseDates": false,
                "axisColor": "#DADADA",
                "minorGridEnabled": true
            }
        });

        $('#divChart').closest('.portlet').find('.fullscreen').click(function () {
            chart.invalidateSize();
        });
    }
}

var GraphDataProvider = []