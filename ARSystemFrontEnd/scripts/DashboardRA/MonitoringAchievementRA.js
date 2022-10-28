var year = (new Date()).getFullYear();
var month = 0;
var fsgroupby = "NON TSEL";
var userGroup="";

jQuery(document).ready(function () {

    Helper.BindMonth();
    Helper.BindYear();

    Data.DataChartLT();
    Data.DataChartOD();
    Data.DataChartRTI();
    Data.DataChartAll();

    userGroup = $("#hdUserRole").val();
    if (userGroup == "All") {
        fsgroupby = "NON TSEL";
        $("#btnGroupNonTsel").attr("disabled", true);
    }

    else if (userGroup == "TSEL") {
        fsgroupby = "TSEL";
        $("#btnGroupTsel").attr("disabled", true);
        $("#btnGroupNonTsel").removeClass("btn-info");
        $("#btnGroupNonTsel").addClass("btn-danger");
        $("#btnGroupNonTsel").attr("disabled", true);
        $("#btnGroupNew").removeClass("btn-info");
        $("#btnGroupNew").addClass("btn-danger");
        $("#btnGroupNew").attr("disabled", true);
        $("#btnGroupNew").hide();
        $("#btnGroupNonTsel").hide();
    }
    else if (userGroup == "NON TSEL") {
        fsgroupby = "NON TSEL";
        $("#btnGroupNonTsel").attr("disabled", true);
        $("#btnGroupTsel").removeClass("btn-info");
        $("#btnGroupTsel").addClass("btn-danger");
        $("#btnGroupTsel").attr("disabled", true);
        $("#btnGroupNew").removeClass("btn-info");
        $("#btnGroupNew").addClass("btn-danger");
        $("#btnGroupNew").attr("disabled", true);
        $("#btnGroupTsel").hide();
        $("#btnGroupNew").hide();
    }
    else if (userGroup == "NEW PRODUCT") {
        fsgroupby = "NEW PRODUCT";
        $("#btnGroupNew").attr("disabled", true);
        $("#btnGroupNonTsel").removeClass("btn-info");
        $("#btnGroupNonTsel").addClass("btn-danger");
        $("#btnGroupNonTsel").attr("disabled", true);
        $("#btnGroupTsel").removeClass("btn-info");
        $("#btnGroupTsel").addClass("btn-danger");
        $("#btnGroupTsel").attr("disabled", true);
        $("#btnGroupTsel").hide();
        $("#btnGroupNonTsel").hide();
    
    }

    $('#sslYear').on('change', function () {
        Data.DataChartLT();
        Data.DataChartOD();
        Data.DataChartRTI();
        Data.DataChartAll();
    });

    $('#btnGroupNew').on('click', function () {
        fsgroupby = "NEW PRODUCT";
        Data.DataChartLT();
        Data.DataChartOD();
        Data.DataChartRTI();
        Data.DataChartAll();
        $("#btnGroupNew").attr("disabled", true);
        $("#btnGroupTsel").removeAttr("disabled");
        $("#btnGroupNonTsel").removeAttr("disabled");
    });

    $('#btnGroupTsel').on('click', function () {
        fsgroupby = "TSEL";
        Data.DataChartLT();
        Data.DataChartOD();
        Data.DataChartRTI();
        Data.DataChartAll();
        $("#btnGroupTsel").attr("disabled", true);
        $("#btnGroupNew").removeAttr("disabled");
        $("#btnGroupNonTsel").removeAttr("disabled");
    });

    $('#btnGroupNonTsel').on('click', function () {
        fsgroupby = "NON TSEL";
        Data.DataChartLT();
        Data.DataChartOD();
        Data.DataChartRTI();
        Data.DataChartAll();
        $("#btnGroupNonTsel").attr("disabled", true);
        $("#btnGroupTsel").removeAttr("disabled");
        $("#btnGroupNew").removeAttr("disabled");
    });


    $('#sslYear').on('change', function () {
        Data.DataChartRTI();
        Data.DataChartOD();
        Data.DataChartLT();
        Data.DataChartAll();
    });

    $('#sslMonth').on('change', function () {
        Data.DataChartRTI();
        Data.DataChartAll();
    });

});

var Data = {

    DataChartLT: function () {
        $.ajax({
            url: "/api/MonitoringAchievementRA/DataChartByAverageLT",
            type: "GET",
            data: { year: year, groupby: fsgroupby },
            success: function (data) {
                Data.summaryLT(data);
                $(".yearbilion").text("   " + year + " FOR " + fsgroupby + " - In Days");
            },
            error: function (xhr) {
            }
        });
    },

    DataChartOD: function () {
        var customerid = "ALL";
        $.ajax({
            url: "/api/MonitoringAchievementRA/DataChartByGroupOD",
            type: "GET",
            data: { year: year, customerid: customerid, groupby: fsgroupby },
            success: function (data) {
                for (i = 0; i < data.length; i++) {
                    data[i]["Limit"] = "99";
                }
                $(".yearbilion2").text("   " + year + " FOR " + fsgroupby + " - In Bilion");
                Data.summaryOD(data);
            },
            error: function (xhr) {
            }
        });
    },

    DataChartRTI: function () {
        $.ajax({
            url: "/api/MonitoringAchievementRA/DataChartByGroupRTI",
            type: "GET",
            data: { groupby: fsgroupby, year: year, month: month },
            success: function (data) {
                Data.summaryChartRTI(data);
                $(".yearbilion3").text("   " + month + " " + year + " FOR " + fsgroupby + " - In Bilion");
            },
            error: function (xhr) {
            }

        });
    },

    DataChartAll: function () {
        $.ajax({
            url: "/api/MonitoringAchievementRA/DataChartByAverageLT",
            type: "GET",
            data: { year: year, groupby: fsgroupby },
            success: function (data) {
                var sum = 0;
                var count = 0;
                for (i = 0; i < data.length; i++) {
                    sum += data[i].Everage;
                    count++;
                }
                var average = (sum / count).toFixed(2);
                $("#ltstatus").text(average);
                $("#percentagelt").text("(Out of 17)");
            },
            error: function (xhr) {
            }
        });

        $.ajax({
            url: "/api/OverdueRTI/DataChartByGroup",
            type: "GET",
            data: { groupby: fsgroupby, year: year, customerid: "ALL" },
            success: function (data) {
                var sumpercentage = 0;
                var sumpercentagenod = 0;
                var count = 0;
                for (i = 0; i < data.length; i++) {
                    sumpercentage += (data[i].OverDue * 100 / data[i].RTI);
                    sumpercentagenod += (data[i].NearlyOverDue * 100 / data[i].RTI);
                    count++;
                }
                var average = sumpercentage / count;
                var averagenod = sumpercentagenod / count;

                $("#odstatus").text(average.toFixed(2) + "%");
                $("#nodstatus").text(averagenod.toFixed(2) + "%");

                if (average >= 1) {
                    $("#percentageod").text("(Out of Target)");
                }
                else {
                    $("#percentageod").text("(On Target)");
                }
            },
            error: function (xhr) {
            }

        });

        $.ajax({
            url: "/api/MonitoringAchievementRA/DataChartByGroupRTI",
            type: "GET",
            data: { groupby: fsgroupby, year: year, month: month },
            success: function (data) {
                var amountAchieve = 0;
                var amountNotAchieve = 0;
                for (i = 0; i < data.length; i++) {
                    amountAchieve += data[i].InvAmount;
                    amountNotAchieve += data[i].AmountTarget;
                }

                var percentage;
                if (data.length !=0) {
                    percentage =  amountAchieve / (amountAchieve + amountNotAchieve) * 100;
                } else {
                    percentage = 0;
                }
                $("#rtistatus").text(percentage.toFixed(2) + " %");
                $("#percentagerti").text("(Achieved)");
            },
            error: function (xhr) {
            }

        });




    },

    summaryLT: function (data) {

        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end

            // Create chart instance
            var chart = am4core.create("chartdivlt", am4charts.XYChart);



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

    summaryOD: function (data) {
        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end

            var chart = am4core.create("chartdivod", am4charts.XYChart);

            chart.data = data;

            //create category axis for years
            var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
            categoryAxis.dataFields.category = "CustomerID";
            categoryAxis.renderer.inversed = true;
            categoryAxis.renderer.grid.template.location = 0;
            categoryAxis.renderer.minGridDistance = 30;


            //create value axis for income and expenses
            var valueAxis = chart.xAxes.push(new am4charts.ValueAxis());
            valueAxis.renderer.opposite = true;
            valueAxis.min = 0;
            valueAxis.max = 100;
            valueAxis.strictMinMax = true;
            valueAxis.calculateTotals = true;


            //create columns
            var series = chart.series.push(new am4charts.ColumnSeries());
            series.dataFields.categoryY = "CustomerID";
            series.dataFields.valueX = "RTI";
            series.name = "RTI";
            series.stacked = true;
            series.columns.template.fillOpacity = 0.5;
            series.columns.template.strokeOpacity = 0;
            series.dataFields.valueXShow = "totalPercent";
            series.tooltipText = "RTI : {valueX.value} ({valueX.totalPercent.formatNumber('#.00')}%)";

            var series2 = chart.series.push(new am4charts.ColumnSeries());
            series2.dataFields.categoryY = "CustomerID";
            series2.dataFields.valueX = "NearlyOverDue";
            series2.name = "NearlyOverdue";
            series2.stacked = true;
            series2.columns.template.fillOpacity = 0.5;
            series2.dataFields.valueXShow = "totalPercent";
            series2.columns.template.strokeOpacity = 0;
            series2.tooltipText = "NOD : {valueX.value} ({valueX.totalPercent.formatNumber('#.00')}%)";

            var series3 = chart.series.push(new am4charts.ColumnSeries());
            series3.dataFields.categoryY = "CustomerID";
            series3.dataFields.valueX = "OverDue";
            series3.name = "Overdue";
            series3.stacked = true;
            series3.columns.template.fillOpacity = 0.5;
            series3.dataFields.valueXShow = "totalPercent";
            series3.columns.template.strokeOpacity = 0;
            series3.tooltipText = "Overdue : {valueX.value} ({valueX.totalPercent.formatNumber('#.00')}%)";
            series3.stroke = am4core.color("#cc0000");
            series3.columns.template.fill = am4core.color("#cc0000");


            ////create line
            var lineSeries = chart.series.push(new am4charts.LineSeries());
            lineSeries.dataFields.categoryY = "CustomerID";
            lineSeries.dataFields.valueX = "Limit";
            lineSeries.name = "Limit";
            lineSeries.stacked = false;
            lineSeries.strokeWidth = 3;


            //add bullets
            var circleBullet = lineSeries.bullets.push(new am4charts.CircleBullet());
            circleBullet.circle.fill = am4core.color("#fff");
            circleBullet.circle.strokeWidth = 2;

            //add chart cursor
            chart.cursor = new am4charts.XYCursor();
            chart.cursor.behavior = "zoomY";

            //add legend
            chart.legend = new am4charts.Legend();

        }); // end am4core.ready()
    },

    summaryChartRTI: function (data) {

        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end

            var chart = am4core.create("chartdivrti", am4charts.XYChart);
            chart.hiddenState.properties.opacity = 0; // this creates initial fade-in

            chart.data = data;
            chart.cursor = new am4charts.XYCursor();
            chart.cursor.lineX.disabled = true;
            chart.cursor.lineY.disabled = true;

            chart.colors.step = 2;
            chart.padding(30, 30, 10, 30);
            chart.legend = new am4charts.Legend();

            var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
            categoryAxis.dataFields.category = "CustomerID";
            categoryAxis.renderer.grid.template.location = 0;
            categoryAxis.renderer.minGridDistance = 30;

            var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
            valueAxis.min = 0;
            valueAxis.max = 100;
            valueAxis.strictMinMax = true;
            valueAxis.calculateTotals = true;
            valueAxis.renderer.minWidth = 50;


            var series1 = chart.series.push(new am4charts.ColumnSeries());
            series1.columns.template.width = am4core.percent(80);
            series1.columns.template.tooltipText =
              "Achievement: \n{valueY} ({valueY.totalPercent.formatNumber('#.00')}%)";
            series1.name = "Achievement";
            series1.dataFields.categoryX = "CustomerID";
            series1.dataFields.valueY = "InvAmount";
            series1.dataFields.valueYShow = "totalPercent";
            series1.dataItems.template.locations.categoryX = 0.5;
            series1.stacked = true;
            series1.tooltip.pointerOrientation = "vertical";
           

            var bullet1 = series1.bullets.push(new am4charts.LabelBullet());
            bullet1.interactionsEnabled = false;
            bullet1.label.text = "{valueY.totalPercent.formatNumber('#.00')}%";
            bullet1.label.fill = am4core.color("#ffffff");
            bullet1.locationY = 0.5;

            var series3 = chart.series.push(new am4charts.ColumnSeries());
            series3.columns.template.width = am4core.percent(80);
            series3.columns.template.tooltipText =
              "Target: \n{valueY} ({valueY.totalPercent.formatNumber('#.00')}%)";
            series3.name = "-Target";
            series3.dataFields.categoryX = "CustomerID";
            series3.dataFields.valueY = "AmountTarget";
            series3.dataFields.valueYShow = "totalPercent";
            series3.dataItems.template.locations.categoryX = 0.5;
            series3.stacked = true;
            series3.tooltip.pointerOrientation = "vertical";
            series3.fill = am4core.color("#cc0000");

            var bullet3 = series3.bullets.push(new am4charts.LabelBullet());
            bullet3.interactionsEnabled = false;
            bullet3.label.text = "{valueY.totalPercent.formatNumber('#.00')}%";
            bullet3.locationY = 0.5;
            bullet3.label.fill = am4core.color("#ffffff");
            
            chart.scrollbarX = new am4core.Scrollbar();

        }); // end am4core.ready()

    },

};

var Helper = {
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

    BindMonth: function () {

        var id = "#sslMonth";
        var Month = ['All', 'January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']
        var monthValue = ['All', 'JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];

        thisMonthInt = new Date().getMonth() + 1;
        var thisMonth = monthValue[thisMonthInt];
        for (var i = 0; i < monthValue.length ; i++) {
            if (i == thisMonthInt) {
                $(id).append('<option value="' + monthValue[i] + '" selected>  ' + Month[i] + '  </option>');
            }
            else {
                $(id).append('<option value="' + monthValue[i] + '">  ' + Month[i] + '  </option>');
            }
        }


        $(id).select2({ placeholder: Month[thisMonth], width: null });
        $(id).val(thisMonth);
    },
    
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
    $("#" + id).toggleClass('fullscreen');
}
