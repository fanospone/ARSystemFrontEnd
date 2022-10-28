Data = {};

jQuery(document).ready(function () {
    Control.LoadYear();
    Control.LoadMonth();
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
        if ($("#slMonth").val() == null || $("#slMonth").val() == "0") {
            columns = [
                {
                    data: "Category", render: function (val, type, full) {
                        return "<b>"+ val +"</b>";
                    }
                },
                {
                    data: "AmountJan", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full);
                    }
                },
                {
                    data: "AmountFeb", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full);
                    }
                },
                {
                    data: "AmountMar", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full);
                    }
                },
                {
                    data: "AmountApr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full);
                    }
                },
                {
                    data: "AmountMay", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full);
                    }
                },
                {
                    data: "AmountJun", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full);
                    }
                },
                {
                    data: "AmountJul", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full);
                    }
                },
                {
                    data: "AmountAug", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full);
                    }
                },
                {
                    data: "AmountSep", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full);
                    }
                },
                {
                    data: "AmountOct", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full);
                    }
                },
                {
                    data: "AmountNov", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full);
                    }
                },
                {
                    data: "AmountDec", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full);
                    }
                }
            ]
            selector = "#tblMonthlyData";
            Table.Draw(selector, columns);
            $(".monthly").fadeIn();
            $(".weekly").hide();
            if ($("#slYear").val() == null) {
                $('#slYear').val($('#slYear option:first-child').val()).trigger('change');
            }
            $("#thYear").html($("#slYear").val());
        } else {
            $("#thMonthName").html(Helper.GetMonthName($("#slMonth").val()) + " " + $("#slYear").val());
            columns = [
                {
                    data: "Category", render: function (val, type, full) {
                        return "<b>" + val + "</b>";
                    }
                },
                {
                    data: "AmountWeek1", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full);
                    }
                },
                {
                    data: "AmountWeek2", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full);
                    }
                },
                {
                    data: "AmountWeek3", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full);
                    }
                },
                {
                    data: "AmountWeek4", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full);
                    }
                },
                {
                    data: "AmountWeek5", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full);
                    }
                },
                {
                    data: "AmountWeek6", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full);
                    }
                }
            ];
            selector = "#tblWeeklyData";
            Table.Draw(selector, columns);
            $(".weekly").fadeIn();
            $(".monthly").hide();
        }
        Graph.Init();
    },
    Reset: function () {
        $("#slYear").val("").trigger("change");
        $("#slMonth").val("0").trigger("change");
    },
    Export: function () {
        var year = $("#slYear").val();
        var month = $("#slMonth").val();

        window.location.href = "/Dashboard/ARRatio/Export?month=" + month + "&year=" + year;
    },
    Draw: function (selector, columns) {

        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var params = {
            Year: $("#slYear").val(),
            Month: $("#slMonth").val()
        };

        var tblSummaryData = $(selector).DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/Dashboard/ARRatio",
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
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            'ordering': false,
            'order': []
        });
    },
    ThousandFormatter: function (value) {
        var parts = value.toString().split(".");
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");

        if (parseFloat(value).toFixed(2) % 1 == 0)
            return parts[0];

        return parts.join(".");
        //return parseFloat(value).toLocaleString('en');
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
            $("#thYear").html($("#slYear").val());
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
    }
}

var Helper = {
    RenderLink: function (val, full) {
        /*if (full.Category != "LT%" && full.Category != "LT Days") {
            return "<a class='btDetail'>" + val + "</a>";
        } else {
            if (full.Category == "LT Days") {
                return val + " Day";
            } else {
                return val;
            }
        }*/
        var divider = 1000000;
        if (val == null)
            return "0";

        if (full.Category == "TRADE RECEIVABLE" || full.Category == "ACCRUE REVENUE" || full.Category == "UNEARNED INCOME" || full.Category == "MONTHLY REVENUE")
            return Common.Format.CommaSeparation(parseFloat(val.replace(/,/g, "")) / divider);
        else
            return Common.Format.CommaSeparation(val);
        //return Common.Format.CommaSeparation(val);
    },
    GetMonthName: function (val) {
        var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
        return months[parseInt(val) - 1];
    }
}

var Graph = {
    Init: function () {
        var params = {
            Year: $("#slYear").val(),
            Month: $("#slMonth").val()
        }

        $.ajax({
            url: "/api/Dashboard/GraphARRatio",
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
                            "ratio": data.RatioJan,
                            "target": data.TargetJan
                        },
                        {
                            "month": "Feb",
                            "ratio": data.RatioFeb,
                            "target": data.TargetFeb
                        },
                        {
                            "month": "Mar",
                            "ratio": data.RatioMar,
                            "target": data.TargetMar
                        },
                        {
                            "month": "Apr",
                            "ratio": data.RatioApr,
                            "target": data.TargetApr
                        },
                        {
                            "month": "May",
                            "ratio": data.RatioMay,
                            "target": data.TargetMay
                        },
                        {
                            "month": "Jun",
                            "ratio": data.RatioJun,
                            "target": data.TargetJun
                        },
                        {
                            "month": "Jul",
                            "ratio": data.RatioJul,
                            "target": data.TargetJul
                        },
                        {
                            "month": "Aug",
                            "ratio": data.RatioAug,
                            "target": data.TargetAug
                        },
                        {
                            "month": "Sep",
                            "ratio": data.RatioSep,
                            "target": data.TargetSep
                        },
                        {
                            "month": "Oct",
                            "ratio": data.RatioOct,
                            "target": data.TargetOct
                        },
                        {
                            "month": "Nov",
                            "ratio": data.RatioNov,
                            "target": data.TargetNov
                        },
                        {
                            "month": "Dec",
                            "ratio": data.RatioDec,
                            "target": data.TargetDec
                        },
                    ];
                } else {
                    GraphDataProvider = [
                        {
                            "month": "Week 1",
                            "ratio": data.RatioWeek1,
                            "target": data.TargetWeek1
                        },
                        {
                            "month": "Week 2",
                            "ratio": data.RatioWeek2,
                            "target": data.TargetWeek2
                        },
                        {
                            "month": "Week 3",
                            "ratio": data.RatioWeek3,
                            "target": data.TargetWeek3
                        },
                        {
                            "month": "Week 4",
                            "ratio": data.RatioWeek4,
                            "target": data.TargetWeek4
                        },
                        {
                            "month": "Week 5",
                            "ratio": data.RatioWeek5,
                            "target": data.TargetWeek5
                        },
                        {
                            "month": "Week 6",
                            "ratio": data.RatioWeek6,
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
                "axisColor": "#0250f7",//"#FF6600",
                "axisThickness": 2,
                "axisAlpha": 1,
                "position": "left"
            }, {
                "id": "v2",
                "axisColor": "#57d63e",//"#FCD202",
                "axisThickness": 2,
                "axisAlpha": 1,
                "position": "right"
            }],
            "graphs": [{
                "valueAxis": "v1",
                "lineColor": "#0250f7",//"#FF6600",
                "bullet": "round",
                "bulletBorderThickness": 1,
                "hideBulletsCount": 30,
                "title": "Ratio",
                "valueField": "ratio",
                "fillAlphas": 0
            }, {
                "valueAxis": "v2",
                "lineColor": "#57d63e",//"#FCD202",
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