var year = 0;
var customerid = "";
var groupby = "NON TSEL";
var groupby2 = "NON TSEL";
var defaultOpt = "XL";

jQuery(document).ready(function () {
    $(".SummaryHeader").hide();
    $(".SummaryDetail").hide();
    $(".SummaryStatus").hide();

    Helper.BindingSelectOperator();
    Helper.BindYear();

    Data.DataChart();
    Table.Init('#overdueRTITable');

   

    $('#sslYear').on('change', function () {
        Data.DataChart();
        $(".SummaryDetail").hide();
    });

    $('#btnExport').on('click', function () {
        Table.Export("All", "All");
    });

    Data.DataChartAvg();
    $("#btnGroupNonTsel").attr("disabled", true);

    groupby = $("#hdUserRole").val();
    if (groupby == "NON TSEL") {
        $("#btnGroupNew").hide();
        $("#btnGroupTsel").hide();
        $("#btnGroupNonTsel").show();
        $("#btnGroupNew2").hide();
        $("#btnGroupTsel2").hide();
        $("#btnGroupNonTsel2").show();
    } else if (groupby == "TSEL") {
        $("#btnGroupNew").hide();
        $("#btnGroupTsel").show();
        $("#btnGroupNonTsel").hide();
        $("#btnGroupNew2").hide();
        $("#btnGroupTsel2").show();
        $("#btnGroupNonTsel2").hide();
    } else if (groupby == "NEW PRODUCT") {
        $("#btnGroupNew").show();
        $("#btnGroupTsel").hide();
        $("#btnGroupNonTsel").hide();
        $("#btnGroupNew2").show();
        $("#btnGroupTsel2").hide();
        $("#btnGroupNonTsel2").hide();
    }

    //$('#btnGroupNew').on('click', function () {
    //    groupby = "NEW PRODUCT";
    //    Data.DataChartAvg();
    //    $("#btnGroupNew").attr("disabled", true);
    //    $("#btnGroupTsel").removeAttr("disabled");
    //    $("#btnGroupNonTsel").removeAttr("disabled");
    //});

    //$('#btnGroupTsel').on('click', function () {
    //    groupby = "TSEL";
    //    Data.DataChartAvg();
    //    $("#btnGroupTsel").attr("disabled", true);
    //    $("#btnGroupNew").removeAttr("disabled");
    //    $("#btnGroupNonTsel").removeAttr("disabled");
    //});

    //$('#btnGroupNonTsel').on('click', function () {
    //    groupby = "NON TSEL";
    //    Data.DataChartAvg();
    //    $("#btnGroupNonTsel").attr("disabled", true);
    //    $("#btnGroupTsel").removeAttr("disabled");
    //    $("#btnGroupNew").removeAttr("disabled");
    //});

    //$('#btnGroupNew2').on('click', function () {
    //    groupby2 = "NEW PRODUCT";
    //    Data.DataChart();
    //    $("#btnGroupNew2").attr("disabled", true);
    //    $("#btnGroupTsel2").removeAttr("disabled");
    //    $("#btnGroupNonTsel2").removeAttr("disabled");
    //});

    //$('#btnGroupTsel2').on('click', function () {
    //    groupby2 = "TSEL";
    //    Data.DataChart();
    //    $("#btnGroupTsel2").attr("disabled", true);
    //    $("#btnGroupNew2").removeAttr("disabled");
    //    $("#btnGroupNonTsel2").removeAttr("disabled");
    //});

    //$('#btnGroupNonTsel2').on('click', function () {
    //    groupby2 = "NON TSEL";
    //    Data.DataChart();
    //    $("#btnGroupNonTsel2").attr("disabled", true);
    //    $("#btnGroupTsel2").removeAttr("disabled");
    //    $("#btnGroupNew2").removeAttr("disabled");
    //});

    yearNow = $("#sslYear").val();
    Data.summaryChartStatus(defaultOpt, yearNow);
    Data.Header(yearNow);

    $('#sslYear').on('change', function () {
        $(".SummaryDetail").hide();
        Data.DataChart();
        Data.DataChartAvg();
        Data.summaryChartStatus(defaultOpt, $("#sslYear").val());
        Data.Header($("#sslYear").val());
    });

    $('#slSearchCustomerID').on('change', function () {
        $(".SummaryDetail").hide();
        defaultOpt = $("#slSearchCustomerID").val();
        Data.summaryChartStatus(defaultOpt, yearNow);
    });


});

function loadTable(param) {
    Table.LoadDataByStatus(param, $("#sslYear").val(), $("#slSearchCustomerID").val());
}

function reload(id) {
    var container = document.getElementById(id);
    var content = container.innerHTML;
    container.innerHTML = content;
    
}

function remove(id) {
    var container = document.getElementById(id);
    var content = "";
    container.innerHTML = content;
    
}

function fullscreen(id) {
    $("#"+id).toggleClass('fullscreen');
}

var Data = {
    DataChart: function () {

        year = $("#sslYear").val();

        $.ajax({
            url: "/api/LeadTimeRTI/DataChartByOperators",
            type: "GET",
            data: { year: year, groupby: groupby2 },
            success: function (data) {
                Data.summaryChart(data);
                $(".yearplusinfo").text("   " + year +  " - In Bilion - Click on chart label for details");
            },
            error: function (xhr) {
                alert("error");
            }

        });

    },

    DataChartAvg: function () {

        year = $("#sslYear").val();

        $.ajax({
            url: "/api/LeadTimeRTI/DataChartByAverage",
            type: "GET",
            data: { year: year, groupby: groupby },
            success: function (data) {
                
                Data.summaryChartAverage(data);
                $(".yearbilion").text("   " + year + " - In Bilion");
            },
            error: function (xhr) {
                alert("error");
            }

        });


    },


    summaryChart: function (data) {

        am4core.ready(function () {
            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end
            // Create chart instance
            var chart = am4core.create("chartdiv", am4charts.XYChart);

            var dataRes = [];
            var setOpt = new Set();

            var map = new Map();
            for (i = 0; i < data.length; i++) {
                var res = new Map();
                map.set(data[i].Category, res);
            }

            for (i = 0; i < data.length; i++) {

                var res = map.get(data[i].Category);
                var cust = data[i].CustomerID;
                res.set(cust.replace(/\s/g, ''), data[i].CountSite);
                setOpt.add(cust.replace(/\s/g, ''))
            }

            for ([key, value]of map) {
                dataRes.push(mapToObj(key, value));
            }

            function mapToObj(key, map) {
                const obj = {}
                obj["Category"] = key;
                for ([k, v]of map)
                    obj[k] = v
                return JSON.parse(JSON.stringify(obj));
            }

            chart.data = dataRes;

            chart.legend = new am4charts.Legend();
            chart.legend.position = "bottom";

            // Create axes
            var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
            categoryAxis.dataFields.category = "Category";
            categoryAxis.renderer.grid.template.opacity = 0;

            var valueAxis = chart.xAxes.push(new am4charts.ValueAxis());
            valueAxis.min = 0;
            valueAxis.renderer.grid.template.opacity = 0.5;
            valueAxis.renderer.ticks.template.strokeOpacity = 0.5;
            valueAxis.renderer.ticks.template.stroke = am4core.color("#495C43");
            valueAxis.renderer.ticks.template.length = 10;
            valueAxis.renderer.line.strokeOpacity = 0.5;
            valueAxis.renderer.baseGrid.disabled = false;
            valueAxis.renderer.minGridDistance = 50;

            // Create series
            function createSeries(field, name) {
                var series = chart.series.push(new am4charts.ColumnSeries());
                series.dataFields.valueX = field;
                series.dataFields.categoryY = "Category";
                series.stacked = true;
                series.name = name;
                series.clustered = false;
                series.tooltipText = "LT for {name} : [bold]{valueX} Sites[/]";
                series.tooltip.interactionsEnabled = true;
                series.tooltip.label.interactionsEnabled = true;
                series.tooltip.keepTargetHover = true;

                var labelBullet = series.bullets.push(new am4charts.LabelBullet());
                labelBullet.locationX = 0.5;
                labelBullet.label.text = "{valueX}";
                labelBullet.label.fill = am4core.color("#fff");


                series.tooltip.label.events.on(
                    "hit",
                    ev => {
                        var item = ev.target.dataItem.component.tooltipDataItem.dataContext;

                        //Head.Init(item.Month);
                        year = $("#sslYear").val();
                        Table.LoadData(item.Category, year, series.name);
                    },
                    this
                 );
                series.events.on(
                   "hit",
                    ev => {
                        var item = ev.target.dataItem.component.tooltipDataItem.dataContext;

                        //Head.Init(item.Month);
                        year = $("#sslYear").val();
                        Table.LoadData(item.Category, year, series.name);
                    },
                    this
                  );
            }

            setOpt.forEach(function (value) {
                createSeries(value, value);
            });
            //createSeries("lamerica", "Latin America");
            //createSeries("meast", "Middle East");
            //createSeries("africa", "Africa");

            chart.cursor = new am4charts.XYCursor();
            chart.cursor.lineX.disabled = true;
            chart.cursor.lineY.disabled = true;


        }); // end am4core.ready()


    },

    Header: function (year) {
        $.ajax({
            url: "/api/LeadTimeRTI/DataChartByOperators",
            type: "GET",
            data: { year: year, groupby : "TSEL" },
            success: function (data) {

                var newCount = 0;

                for (i = 0; i < data.length; i++) {
                    newCount += data[i].CountSite;
                }

                $("#numbertsel").text(newCount);
            },
            error: function (xhr) {

            }

        });
        $.ajax({
            url: "/api/LeadTimeRTI/DataChartByOperators",
            type: "GET",
            data: { year: year, groupby: "NON TSEL" },
            success: function (data) {

                var newCount = 0;

                for (i = 0; i < data.length; i++) {
                    newCount += data[i].CountSite;
                }

                $("#numbernontsel").text(newCount);
            },
            error: function (xhr) {

            }

        });
        $.ajax({
            url: "/api/LeadTimeRTI/DataChartByOperators",
            type: "GET",
            data: { year: year, groupby: "NEW PRODUCT" },
            success: function (data) {

                var newCount = 0;

                for (i = 0; i < data.length; i++) {
                    newCount += data[i].CountSite;
                }

                $("#numbernew").text(newCount);
            },
            error: function (xhr) {

            }

        });

        

    },

    summaryChartAverage: function (data) {

        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end

            // Create chart instance
            var chart = am4core.create("chartdivaverage", am4charts.XYChart);



            // Data for both series

            var dataRes = [];
            var sum = 0;
            var counter = 0;

            for (i = 0; i < data.length; i++) {

                sum += data[i].Everage;
                counter += 1;

            }

            var average = sum / counter;

            for (i = 0; i < data.length; i++) {

                var obj = data[i];
                obj["Average"] = average;
                obj["Target"] = 17;
                dataRes.push(obj);
            }

            chart.data = dataRes;

            /* Create axes */
            var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
            categoryAxis.dataFields.category = "CustomerID";
            categoryAxis.renderer.minGridDistance = 30;

            /* Create value axis */
            var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

            /* Create series */
            var columnSeries = chart.series.push(new am4charts.ColumnSeries());
            columnSeries.name = "Average";
            columnSeries.dataFields.valueY = "Everage";
            columnSeries.dataFields.categoryX = "CustomerID";

            columnSeries.columns.template.tooltipText = "[#fff font-size: 15px]{name} for {categoryX}:\n[/][#fff font-size: 20px]{valueY}[/] [#fff]{additional}[/]"
            columnSeries.columns.template.propertyFields.fillOpacity = "fillOpacity";
            columnSeries.columns.template.propertyFields.stroke = "stroke";
            columnSeries.columns.template.propertyFields.strokeWidth = "strokeWidth";
            columnSeries.columns.template.propertyFields.strokeDasharray = "columnDash";
            columnSeries.tooltip.label.textAlign = "middle";

            var lineSeries = chart.series.push(new am4charts.LineSeries());
            lineSeries.name = "Average";
            lineSeries.dataFields.valueY = "Average";
            lineSeries.dataFields.categoryX = "CustomerID";

            lineSeries.stroke = am4core.color("#fdd400");
            lineSeries.strokeWidth = 3;
            lineSeries.propertyFields.strokeDasharray = "lineDash";
            lineSeries.tooltip.label.textAlign = "middle";

            var lineSeries2 = chart.series.push(new am4charts.LineSeries());
            lineSeries2.name = "Target";
            lineSeries2.dataFields.valueY = "Target";
            lineSeries2.dataFields.categoryX = "CustomerID";

            lineSeries2.stroke = am4core.color("#cc0000");
            lineSeries2.strokeWidth = 3;
            lineSeries2.propertyFields.strokeDasharray = "lineDash";
            lineSeries2.tooltip.label.textAlign = "middle";

            var bullet = lineSeries.bullets.push(new am4charts.Bullet());
            bullet.fill = am4core.color("#fdd400"); // tooltips grab fill from parent by default
            bullet.tooltipText = "[#fff font-size: 15px]{name} for ALL:\n[/][#fff font-size: 20px]{valueY}[/] [#fff]{additional}[/]"
            var circle = bullet.createChild(am4core.Circle);
            circle.radius = 4;
            circle.fill = am4core.color("#fff");
            circle.strokeWidth = 3;

            var bullet = lineSeries2.bullets.push(new am4charts.Bullet());
            bullet.fill = am4core.color("#cc0000"); // tooltips grab fill from parent by default
            bullet.tooltipText = "[#fff font-size: 15px]LT Target:\n[/][#fff font-size: 20px]17[/] [#fff]{additional}[/]"
            var circle = bullet.createChild(am4core.Circle);
            circle.radius = 4;
            circle.fill = am4core.color("#fff");
            circle.strokeWidth = 3;



        }); // end am4core.ready()
    },


    summaryChartStatus: function (customerid, year) {

        $.ajax({
            url: "/api/LeadTimeRTI/DataChartByStatus",
            type: "GET",
            data: { customerid: customerid, year: year },
            success: function (data) {

                var html = '';
                var colorHtml = ["indigo", "purple", "deep-purple", "light-blue", "blue", "light-blue", "green", "light-green", "cyan", "teal", "yellow", "grey", "blue"];

                var sum = 0;
                for (i = 0; i < data.length; i++) {
                    sum += data[i].CountSite;
                }

                for (i = 0; i < data.length; i++) {
                    var iconHtml = "event_note";
                    if (data[i].CurrentStatus.toLowerCase().includes("done")) {
                        iconHtml = "event_available";
                    }
                    else if (data[i].CurrentStatus.toLowerCase().includes("waiting")) {
                        iconHtml = "access_time";
                    }
                    else if (data[i].CurrentStatus.toLowerCase().includes("processed")) {
                        iconHtml = "event";
                    }
                    else if (data[i].CurrentStatus.toLowerCase().includes("reject")) {
                        iconHtml = "event_busy";
                        colorHtml[i] = "red";
                    }
                    else if (data[i].CurrentStatus.toLowerCase().includes("return")) {
                        iconHtml = "report_problem";
                        colorHtml[i] = "red";
                    }
                    else if (data[i].CurrentStatus.toLowerCase().includes("input")) {
                        iconHtml = "input";
                    }
                    var dataStatus = data[i].CurrentStatus;
                    //var pattern = '^' + dataStatus.replace(' ', '\\s');
                    //var rexp = new RegExp(pattern, '');
                    var status = data[i].CurrentStatus.replace(/\s/g, '');
                    var countSite = data[i].CountSite;
                    var percentage = ((data[i].CountSite) * 100 / sum).toFixed(2);

                    var divHtml = '<div class="col-md-3"><div class="info-box hover-expand-effect clickable" onClick="loadTable(' + "'"+dataStatus+"'" + ')"><div class="icon bg-' + colorHtml[i] + '"><i class="material-icons">' + iconHtml + '</i></div><div class="content"><div class="text">' + dataStatus + '</div><div class="number">' + countSite + '<span>&nbsp</span><span style="font-size:14px">(' + percentage + '%)</span></div></div></div></div>'
                 //   var divHtml = "<div class='col-md-3'><div class='info-box hover-expand-effect' onClick='loadTable('"+dataStatus+"')' value='" + dataStatus + "'><div class='icon bg-'" + colorHtml[i] + "'><i class='material-icons'>'" + iconHtml + "'</i></div><div class='content'><div class='text'>'" + dataStatus + "'</div><div class='number'>'" + countSite + "'<span>&nbsp</span><span style='font-size:14px'>('" + percentage + "'%)</span></div></div></div></div>"
                    html += divHtml;
                }
                
                $("#leadTimeByStatus").html(html);
               

            },
            error: function (xhr) {
                alert("error");
            }

        });

    },

};


var Table = {
    Init: function (idTable) {
        var tblSummary = $(idTable).dataTable({
            "filter": false,
            "destroy": true,
            "data": [],
            "proccessing": true,
            "language": {
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
        });
        $(window).resize(function () {
            $(idTable).DataTable().columns.adjust().draw();
        });
    },

    LoadData: function (leadtime, year, customerid) {
        $(".SummaryDetail").show();
        var idTb = "#overdueRTITable";
        Table.Init(idTb);


        var params = {
            LeadTime: leadtime,
            Year: year,
            CustomerID: customerid
        };
        var tblList = $(idTb).DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/LeadTimeRTI/DataDetail",
                "type": "POST",
                "datatype": "json",
                "data": params,
                "cache": false
            },
            buttons: [
                 { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.Export(leadtime, customerid);
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50, 200, 99999], ['10', '25', '50', '200', 'Visible']],
            "destroy": true,
            "columns": [
                { data: "RowIndex" },
                //{
                //    orderable: false,
                //    mRender: function (data, type, full) {

                //    }
                //},
                { data: "SoNumber" },
                { data: "SiteId" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CompanyId" },
                { data: "CustomerId" },
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
                { data: "Term" },
                { data: "Year" },
                {
                    data: "StartInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "InvoiceAmount", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                { data: "Currency" },
                { data: "CurrentStatus" },
                {
                    data: "BAPSConfirmDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "RTIDoneRADate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [1], "className": "text-center" }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
        });
    },

    LoadDataByStatus: function (status, year, customerid) {
        $(".SummaryDetail").show();
        var idTb = "#overdueRTITable";
        Table.Init(idTb);


        var params = {
            CurrentStatus: status,
            Year: year,
            CustomerID: customerid
        };
        var tblList = $(idTb).DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/LeadTimeRTI/DataDetailByStatus",
                "type": "POST",
                "datatype": "json",
                "data": params,
                "cache": false
            },
            buttons: [
                 { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.ExportByStatus(customerid, status);
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50, 200, 99999], ['10', '25', '50', '200', 'Visible']],
            "destroy": true,
            "columns": [
                { data: "RowIndex"
                },
                { data: "SoNumber" },
                { data: "SiteId" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CompanyId" },
                { data: "CustomerId" },
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
                { data: "Term" },
                { data: "Year" },
                {
                    data: "StartInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "InvoiceAmount", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                //{ data: "AmountRental" },
                //{ data: "AmountService" },
                { data: "Currency" },
                { data: "CurrentStatus" },
                 {
                     data: "BAPSConfirmDate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                {
                    data: "RTIDoneRADate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [1], "className": "text-center" }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
        });
    },

    Export: function (leadtime, customerid) {
        var year = $("#sslYear").val();
        window.location.href = "/DashboardRA/LeadTimeRTI/Export?LeadTime=" + leadtime + "&Year=" + year + "&Customer=" + customerid + "";
    },

    ExportByStatus: function (customerid, currentStatus) {
        var year = $("#sslYear").val();
        window.location.href = "/DashboardRA/LeadTimeRTI/ExportByStatus?Customer=" + customerid + "&Year=" + year + "&currentStatus=" + currentStatus + "";
    },

}

var Helper = {
    BindingSelectOperator: function () {
        var id = "#slSearchCustomerID";

        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option value='XL' selected>EXCELCOMINDO</option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.OperatorId) + "'>" + item.Operator + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Operator", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindYear: function () {
        var start_year = new Date().getFullYear();
        var id = "#sslYear";
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

}


