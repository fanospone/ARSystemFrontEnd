var year = 0;
var customerid = "";
var vmonth;
var date = new Date();
var globalmonth = "";
var fsgroupby = "";

jQuery(document).ready(function () {
    $(".SummaryHeader").hide();
    $(".SummaryDetail").hide();
    $(".SummaryStatus").hide();

    Helper.BindingSelectOperator();
    Helper.BindYear();


    var months = ['JAN', 'FEB', 'MAR', 'APR', 'MEI', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];
    var now = new Date();
    var thisMonth = months[now.getMonth()];
    globalmonth = thisMonth;

    fsgroupby = $("#hdUserRole").val();

    Data.DataChart();
    Data.DataChartByGroup();
    Table.Init('#overdueRTITable');

    Head.Init(thisMonth);

    $('#sslYear').on('change', function () {
        Data.DataChart();
        $(".SummaryDetail").hide();
        $("#numbernearlyoverdue").text("Loading..");
        $("#numberoverdue").text("Loading..");
        $("#numberrti").text("Loading..");
        Head.Init(globalmonth);
    });

    $('#slSearchCustomerID').on('change', function () {
        Data.DataChart();
        $(".SummaryDetail").hide();
        $("#numbernearlyoverdue").text("Loading..");
        $("#numberoverdue").text("Loading..");
        $("#numberrti").text("Loading..");
        Head.Init(globalmonth);
    });
    $('#btnExport').on('click', function () {
        Table.Export("All", 0);
    });

    if (fsgroupby == "NON TSEL") {
        $("#btnGroupNew2").hide();
        $("#btnGroupTsel2").hide();
        $("#btnGroupNonTsel2").show();
    } else if (fsgroupby == "TSEL") {
        $("#btnGroupNew2").hide();
        $("#btnGroupTsel2").show();
        $("#btnGroupNonTsel2").hide();
    } else if (fsgroupby == "NEW PRODUCT") {
        $("#btnGroupNew2").show();
        $("#btnGroupTsel2").hide();
        $("#btnGroupNonTsel2").hide();
    }

    //$('#btnGroupNew2').on('click', function () {
    //    fsgroupby = "NEW PRODUCT";
    //    Data.DataChartByGroup();
    //    $("#btnGroupNew2").attr("disabled", true);
    //    $("#btnGroupTsel2").removeAttr("disabled");
    //    $("#btnGroupNonTsel2").removeAttr("disabled");
    //});

    //$('#btnGroupTsel2').on('click', function () {
    //    fsgroupby = "TSEL";
    //    Data.DataChartByGroup();
    //    $("#btnGroupTsel2").attr("disabled", true);
    //    $("#btnGroupNew2").removeAttr("disabled");
    //    $("#btnGroupNonTsel2").removeAttr("disabled");
    //});

    //$('#btnGroupNonTsel2').on('click', function () {
    //    fsgroupby = "NON TSEL";
    //    Data.DataChartByGroup();
    //    $("#btnGroupNonTsel2").attr("disabled", true);
    //    $("#btnGroupTsel2").removeAttr("disabled");
    //    $("#btnGroupNew2").removeAttr("disabled");
    //});


    //$("#btSearch").unbind().click(function () {
    //    Data.DataChart();
    //    $(".SummaryDetail").hide();
    //});
    $("#btnNearly").unbind().click(function () {
       
        
        if ($("#spbtnNearly").text() == "Select Nearly Overdue"){
            $("#spbtnNearly").text("Select Overdue");
            type = "nearly";
        }   
        else {
            $("#spbtnNearly").text("Select Nearly Overdue");
            type = "overdue";
        }
        Table.LoadData(vmonth, type);
            
        //$("#btnNearly").text("Select Overdue");
    });



});

var Data = {
    DataChart: function () {

        year = $("#sslYear").val();
        customerid = $("#slSearchCustomerID").val();

        $.ajax({
            url: "/api/OverdueRTI/DataChart",
            type: "GET",
            data: { year: year, customerid: customerid, groupBy: fsgroupby },
            success: function (data) {
                Data.summaryChart(data.dataChart);
                //Data.summaryOverdueOperators(data.vwSumData[1].Operators);
                //Data.summaryNearlyOverdueOperators(data.vwSumData[2].Operators);

                Data.summaryOverdueOperators(data.vwSumData);
                Data.summaryNearlyOverdueOperators(data.vwSumData);

                $(".yearplusinfo").text("   " + year + " FOR " + customerid + " - In Bilion - Click on chart label for details");
                $(".yearoperator").text("   " + year + " FOR " + customerid);

                $(".SummaryHeader").show();
                $(".SummaryStatus").show();
            },
            error: function (xhr) {
                alert("error");
            }

        });


    },

    DataChartByGroup: function () {

        customerid = "ALL";

        $.ajax({
            url: "/api/OverdueRTI/DataChartByGroup",
            type: "GET",
            data: { year: year, customerid: customerid, groupby: fsgroupby },
            success: function (data) {
                for (i = 0; i < data.length; i++) {
                    data[i]["Limit"] = "99";
                }
                $(".yearbilion").text("   " + year + " FOR " + fsgroupby + " - In Bilion");
                Data.summaryAll(data);
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
            chart.data = data;
            var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
            categoryAxis.dataFields.category = "Month";
            categoryAxis.renderer.grid.template.location = 0;
            categoryAxis.renderer.minGridDistance = 30;

            var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
            valueAxis.title.fontWeight = 800;

            // Create series
            var series = chart.series.push(new am4charts.ColumnSeries());
            series.dataFields.valueY = "OverDue";
            series.dataFields.categoryX = "Month";
            series.clustered = false;
            series.tooltipText = "Overdue in {categoryX} : [bold]{valueY}[/]";
            series.tooltip.label.interactionsEnabled = true;
            series.tooltip.keepTargetHover = true;
            series.stroke = am4core.color("#cc0000");
            series.columns.template.fill = am4core.color("#cc0000");

            var series2 = chart.series.push(new am4charts.ColumnSeries());
            series2.dataFields.valueY = "RTI";
            series2.dataFields.categoryX = "Month";
            series2.clustered = false;
            series2.columns.template.width = am4core.percent(50);
            //series2.tooltipText = "RTI in {categoryX} : [bold]{valueY}[/]";



            series2.tooltip.label.interactionsEnabled = true;
            series2.tooltip.keepTargetHover = true;
            series2.tooltipText = "RTI in {categoryX} : [bold]{valueY}[/]";

            series2.tooltip.label.events.on(
              "hit",
              ev => {
                  var item = ev.target.dataItem.component.tooltipDataItem.dataContext;
                  //alert("line clicked on: " + item.Month);
                  $("#numbernearlyoverdue").text("Loading..");
                  $("#numberoverdue").text("Loading..");
                  $("#numberrti").text("Loading..");
                  Head.Init(item.Month);
                  GetDetails(item.Month, 'RTI');
                  globalmonth = item.Month;
              },
              this
            );
            series.tooltip.label.events.on(
              "hit",
              ev => {
                  var item = ev.target.dataItem.component.tooltipDataItem.dataContext;
                  $("#numbernearlyoverdue").text("Loading..");
                  $("#numberoverdue").text("Loading..");
                  $("#numberrti").text("Loading..");
                  //alert("line clicked on: " + item.Month);
                  Head.Init(item.Month);
                  GetDetails(item.Month, 'OverDue');
                  globalmonth = item.Month;
              },
              this
            );

            chart.cursor = new am4charts.XYCursor();
            chart.cursor.lineX.disabled = true;
            chart.cursor.lineY.disabled = true;


        }); // end am4core.ready()


    },

    summaryAll: function (data) {
        am4core.ready(function() {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end

            var chart = am4core.create("chartdivsummary", am4charts.XYChart);

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

    summaryOverdue: function (totalRTI, totalOverdue) {
        am4core.ready(function () {


            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end

            // Create chart instance
            var chart = am4core.create("chartdivoverdue", am4charts.PieChart);

            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());
            pieSeries.dataFields.value = "amount";
            pieSeries.dataFields.category = "status";

            // Let's cut a hole in our Pie chart the size of 30% the radius
            chart.innerRadius = am4core.percent(30);

            // Put a thick white border around each Slice
            pieSeries.slices.template.stroke = am4core.color("#fff");
            pieSeries.slices.template.strokeWidth = 2;
            pieSeries.slices.template.strokeOpacity = 1;
            pieSeries.slices.template
              // change the cursor on hover to make it apparent the object can be interacted with
              .cursorOverStyle = [
                {
                    "property": "cursor",
                    "value": "pointer"
                }
              ];

            pieSeries.alignLabels = false;
            pieSeries.labels.template.bent = true;
            pieSeries.labels.template.radius = 3;
            pieSeries.labels.template.padding(0, 0, 0, 0);

            pieSeries.ticks.template.disabled = true;

            pieSeries.colors.list = [
                am4core.color("#845EC2"),
                am4core.color("#D65DB1"),
                am4core.color("#FF6F91"),
                am4core.color("#FF9671"),
                am4core.color("#FFC75F"),
                am4core.color("#F9F871")
            ];
            pieSeries.slices.template.propertyFields.fill = "color";
            pieSeries.slices.template.propertyFields.stroke = "color";

            // Create a base filter effect (as if it's not there) for the hover to return to
            var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
            shadow.opacity = 0;

            // Create hover state
            var hoverState = pieSeries.slices.template.states.getKey("hover"); // normally we have to create the hover state, in this case it already exists

            // Slightly shift the shadow and make it more prominent on hover
            var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
            hoverShadow.opacity = 0.7;
            hoverShadow.blur = 5;

            // Add a legend
            //chart.legend = new am4charts.Legend();

            chart.data = [{
                "status": "RTI",
                "amount": totalRTI
            }, {
                "status": "Overdue",
                "amount": totalOverdue
            }, ]
        }); // end am4core.ready()

        
    },

    summaryNearlyOverdue: function (totalRTI, totalNearlyOverdue) {
        am4core.ready(function () {


            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end

            // Create chart instance
            var chart = am4core.create("chartdivnearlyoverdue", am4charts.PieChart);

            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());
            pieSeries.dataFields.value = "amount";
            pieSeries.dataFields.category = "status";

            // Let's cut a hole in our Pie chart the size of 30% the radius
            chart.innerRadius = am4core.percent(30);

            // Put a thick white border around each Slice
            pieSeries.slices.template.stroke = am4core.color("#fff");
            pieSeries.slices.template.strokeWidth = 2;
            pieSeries.slices.template.strokeOpacity = 1;
            pieSeries.slices.template
              // change the cursor on hover to make it apparent the object can be interacted with
              .cursorOverStyle = [
                {
                    "property": "cursor",
                    "value": "pointer"
                }
              ];

            pieSeries.alignLabels = false;
            pieSeries.labels.template.bent = true;
            pieSeries.labels.template.radius = 3;
            pieSeries.labels.template.padding(0, 0, 0, 0);

            pieSeries.ticks.template.disabled = true;

            // Create a base filter effect (as if it's not there) for the hover to return to
            var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
            shadow.opacity = 0;

            // Create hover state
            var hoverState = pieSeries.slices.template.states.getKey("hover"); // normally we have to create the hover state, in this case it already exists

            // Slightly shift the shadow and make it more prominent on hover
            var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
            hoverShadow.opacity = 0.7;
            hoverShadow.blur = 5;

            // Add a legend
            //chart.legend = new am4charts.Legend();

            chart.data = [{
                "status": "RTI",
                "amount": totalRTI
            }, {
                "status": "Nearly Overdue",
                "amount": totalNearlyOverdue
            }, ]
        }); // end am4core.ready()

    },

    summaryRTIOperators: function (data) {
        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end

            // Create chart instance
            var chart = am4core.create("chartdivrtioperators", am4charts.PieChart);

            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());
            pieSeries.dataFields.value = "Amount";
            pieSeries.dataFields.category = "CustomerID";

            // Let's cut a hole in our Pie chart the size of 30% the radius
            chart.innerRadius = am4core.percent(30);

            // Put a thick white border around each Slice
            pieSeries.slices.template.stroke = am4core.color("#fff");
            pieSeries.slices.template.strokeWidth = 2;
            pieSeries.slices.template.strokeOpacity = 1;
            pieSeries.slices.template
              // change the cursor on hover to make it apparent the object can be interacted with
              .cursorOverStyle = [
                {
                    "property": "cursor",
                    "value": "pointer"
                }
              ];

            pieSeries.ticks.template.disabled = true;
            pieSeries.alignLabels = false;
            pieSeries.labels.template.text = "{category} : {value.percent.formatNumber('#.0')}%";
            pieSeries.labels.template.radius = am4core.percent(-40);
            pieSeries.labels.template.fill = am4core.color("black");

            pieSeries.labels.template.adapter.add("text", function (text, target) {
                if (target.dataItem && (target.dataItem.values.value.percent < 5)) {
                    return "";
                }
                return text;
            });

            pieSeries.labels.template.adapter.add("radius", function (radius, target) {
                if (target.dataItem && (target.dataItem.values.value.percent < 10)) {
                    return 0;
                }
                return radius;
            });

            pieSeries.labels.template.adapter.add("fill", function (color, target) {
                if (target.dataItem && (target.dataItem.values.value.percent < 10)) {
                    return am4core.color("#000");
                }
                return color;
            });

            pieSeries.colors.list = [
                am4core.color("#845EC2"),
                am4core.color("#D65DB1"),
                am4core.color("#FF6F91"),
                am4core.color("#FF9671"),
                am4core.color("#FFC75F"),
                am4core.color("#F9F871")
            ];
            pieSeries.slices.template.propertyFields.fill = "color";
            pieSeries.slices.template.propertyFields.stroke = "color";

            // Create a base filter effect (as if it's not there) for the hover to return to
            var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
            shadow.opacity = 0;

            // Create hover state
            var hoverState = pieSeries.slices.template.states.getKey("hover"); // normally we have to create the hover state, in this case it already exists

            // Slightly shift the shadow and make it more prominent on hover
            var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
            hoverShadow.opacity = 0.7;
            hoverShadow.blur = 5;

            // Add a legend

            var resData = [];
            for (i = 0; i < data.length; i++) {
                var obj = new Object();
                obj.CustomerID = data[i].CustomerID;
                obj.Amount = data[i].Amount;
                var jsonString = JSON.stringify(obj);
                resData.push(obj);
            }

            chart.data = resData;
        }); // end am4core.ready()

    },

    summaryOverdueOperators: function (data) {
        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end

            // Create chart instance
            var chart = am4core.create("chartdivoverdueoperators", am4charts.PieChart);

            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());
            //pieSeries.dataFields.value = "Amount";
            //pieSeries.dataFields.category = "CustomerID";

            pieSeries.dataFields.value = "Amount";
            pieSeries.dataFields.category = "CustomerID";

            // Let's cut a hole in our Pie chart the size of 30% the radius
            chart.innerRadius = am4core.percent(30);

            // Put a thick white border around each Slice
            pieSeries.slices.template.stroke = am4core.color("#fff");
            pieSeries.slices.template.strokeWidth = 2;
            pieSeries.slices.template.strokeOpacity = 1;
            pieSeries.slices.template
              // change the cursor on hover to make it apparent the object can be interacted with
              .cursorOverStyle = [
                {
                    "property": "cursor",
                    "value": "pointer"
                }
              ];

            pieSeries.ticks.template.disabled = true;
            pieSeries.alignLabels = false;
            pieSeries.labels.template.text = "{category} : {value.percent.formatNumber('#.0')}%";
            pieSeries.labels.template.radius = am4core.percent(-40);
            pieSeries.labels.template.fill = am4core.color("black");

            pieSeries.labels.template.adapter.add("text", function (text, target) {
                if (target.dataItem && (target.dataItem.values.value.percent < 5)) {
                    return "";
                }
                return text;
            });

            pieSeries.labels.template.adapter.add("radius", function (radius, target) {
                if (target.dataItem && (target.dataItem.values.value.percent < 10)) {
                    return 0;
                }
                return radius;
            });

            pieSeries.labels.template.adapter.add("fill", function (color, target) {
                if (target.dataItem && (target.dataItem.values.value.percent < 10)) {
                    return am4core.color("#000");
                }
                return color;
            });

            pieSeries.colors.list = [
                am4core.color("#845EC2"),
                am4core.color("#D65DB1"),
                am4core.color("#FF6F91"),
                am4core.color("#FF9671"),
                am4core.color("#FFC75F"),
                am4core.color("#F9F871")
            ];
            pieSeries.slices.template.propertyFields.fill = "color";
            pieSeries.slices.template.propertyFields.stroke = "color";

            // Create a base filter effect (as if it's not there) for the hover to return to
            var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
            shadow.opacity = 0;

            // Create hover state
            var hoverState = pieSeries.slices.template.states.getKey("hover"); // normally we have to create the hover state, in this case it already exists

            // Slightly shift the shadow and make it more prominent on hover
            var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
            hoverShadow.opacity = 0.7;
            hoverShadow.blur = 5;

            // Add a legend

            var resData = [];
            //for (i = 0; i < data.length; i++) {
            //    var obj = new Object();
            //    obj.CustomerID= data[i].CustomerID;
            //    obj.Amount = data[i].Amount;
            //    var jsonString = JSON.stringify(obj);
            //    resData.push(obj);
            //}

            for (i = 0; i < data.length; i++) {
                var obj = new Object();
                obj.CustomerID = data[i].CustomerID;
                obj.Amount = data[i].OverDue;
                var jsonString = JSON.stringify(obj);
                resData.push(obj);
            }

            chart.data = resData;
        }); // end am4core.ready()
        
    },

    summaryNearlyOverdueOperators: function (data) {
        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end

            // Create chart instance
            var chart = am4core.create("chartdivnearlyoverdueoperators", am4charts.PieChart);

            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());
            //pieSeries.dataFields.value = "Amount";
            //pieSeries.dataFields.category = "CustomerID";

            pieSeries.dataFields.value = "Amount";
            pieSeries.dataFields.category = "CustomerID";

            // Let's cut a hole in our Pie chart the size of 30% the radius
            chart.innerRadius = am4core.percent(30);

            // Put a thick white border around each Slice
            pieSeries.slices.template.stroke = am4core.color("#fff");
            pieSeries.slices.template.strokeWidth = 2;
            pieSeries.slices.template.strokeOpacity = 1;
            pieSeries.slices.template
              // change the cursor on hover to make it apparent the object can be interacted with
              .cursorOverStyle = [
                {
                    "property": "cursor",
                    "value": "pointer"
                }
              ];

            pieSeries.ticks.template.disabled = true;
            pieSeries.alignLabels = false;
            pieSeries.labels.template.text = "{category} : {value.percent.formatNumber('#.0')}%";
            pieSeries.labels.template.radius = am4core.percent(-40);
            pieSeries.labels.template.fill = am4core.color("black");

            pieSeries.labels.template.adapter.add("text", function (text, target) {
                if (target.dataItem && (target.dataItem.values.value.percent < 5)) {
                    return "";
                }
                return text;
            });

            pieSeries.labels.template.adapter.add("radius", function (radius, target) {
                if (target.dataItem && (target.dataItem.values.value.percent < 10)) {
                    return 0;
                }
                return radius;
            });

            pieSeries.labels.template.adapter.add("fill", function (color, target) {
                if (target.dataItem && (target.dataItem.values.value.percent < 10)) {
                    return am4core.color("#000");
                }
                return color;
            });

            pieSeries.colors.list = [
                am4core.color("#845EC2"),
                am4core.color("#6f27f5"),
                am4core.color("#3527f5"),
                am4core.color("#7f77f7"),
                am4core.color("#a39dfc"),
                am4core.color("#c2befa")
            ];
            pieSeries.slices.template.propertyFields.fill = "color";
            pieSeries.slices.template.propertyFields.stroke = "color";

            // Create a base filter effect (as if it's not there) for the hover to return to
            var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
            shadow.opacity = 0;

            // Create hover state
            var hoverState = pieSeries.slices.template.states.getKey("hover"); // normally we have to create the hover state, in this case it already exists

            // Slightly shift the shadow and make it more prominent on hover
            var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
            hoverShadow.opacity = 0.7;
            hoverShadow.blur = 5;

            // Add a legend

            var resData = [];
            //for (i = 0; i < data.length; i++) {
            //    var obj = new Object();
            //    obj.CustomerID = data[i].CustomerID;
            //    obj.Amount = data[i].Amount;
            //    var jsonString = JSON.stringify(obj);
            //    resData.push(obj);
            //}

            for (i = 0; i < data.length; i++) {
                var obj = new Object();
                obj.CustomerID = data[i].CustomerID;
                obj.Amount = data[i].NearlyOverDue;
                var jsonString = JSON.stringify(obj);
                resData.push(obj);
            }

            chart.data = resData;
        }); // end am4core.ready()

    },

    
};

var Head = {
    Init: function (month){
        var paramsRti = {
            Month: month,
            Year: year,
            DataType: "RTI",
            CustomerID: customerid,
        };
        Head.RTISites(paramsRti);
        var paramsOverdue = {
            Month: month,
            Year: year,
            DataType: "OverDue",
            CustomerID: customerid,
        };
        Head.OverdueSites(paramsOverdue);
        var paramsNearly = {
            Month: month,
            Year: year,
            DataType: "nearly",
            CustomerID: customerid,
        };
        Head.NearlyOverdueSites(paramsNearly);
    },

    RTISites: function (params) {
        $.ajax({
            url: "/api/OverdueRTI/DataDetail",
            "type": "POST",
            "datatype": "json",
            "data": params,
            "cache": false,
            success: function (data) {
                $("#numberrti").text(data.recordsTotal);
            },
            error: function (xhr) {
            }

        });
    },
    OverdueSites: function (params) {
    $.ajax({
        url: "/api/OverdueRTI/DataDetail",
        "type": "POST",
        "datatype": "json",
        "data": params,
        "cache": false,
        success: function (data) {
            $("#numberoverdue").text(data.recordsTotal);
        },
        error: function (xhr) {
        }

    });
    },
    NearlyOverdueSites: function (params) {
        $.ajax({
            url: "/api/OverdueRTI/DataDetail",
            "type": "POST",
            "datatype": "json",
            "data": params,
            "cache": false,
            success: function (data) {
                $("#numbernearlyoverdue").text(data.recordsTotal);
            },
            error: function (xhr) {
            }

        });
    }


}

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

    LoadData: function (month, type) {
        vmonth = month;
        var idTb = "#overdueRTITable";
        Table.Init(idTb);
        //var l = Ladda.create(document.querySelector("#btSearchBulky"));
        //l.start();
        if (type.toLowerCase() == "rti") {
            $("#lblSumDetail").text("( RTI )");
            $("#btnNearly").hide();
        }
        else if (type.toLowerCase() == "nearly") {
            $("#lblSumDetail").text("( Nearly Overdue )");
            $("#btnNearly").show();
        }
        else {
            $("#lblSumDetail").text("(Overdue )");
            var date = new Date();
            if (month == MonthName(date.getMonth()) || month == MonthName(date.getMonth() - 1)) {
                $("#btnNearly").show();
                $("#lblSumDetail").text("( Nearly Overdue & Overdue )");
            } else {
                $("#btnNearly").hide();
            }
        }

        var params = {
            Month: month,
            Year: year,
            DataType: type,
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
                "url": "/api/OverdueRTI/DataDetail",
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
                        Table.Export(type, vmonth);
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
                //{ data: "AmountRental" },
                //{ data: "AmountService" },
                { data: "Currency" },
                { data: "CurrentStatus" },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [1], "className": "text-center" }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            //"fnPreDrawCallback": function () {
            //    App.blockUI({ target: ".panelSearchResult", boxed: true });
            //},
            //"fnDrawCallback": function () {
            //    if (Common.CheckError.List(tblList.data())) {
            //        $(".panelSearchResult").fadeIn(1000);
            //    }
            //    l.stop(); App.unblockUI('.panelSearchResult');
            //    if (Data.RowSelected.length > 0) {
            //        var item;
            //        for (var i = 0 ; i < Data.RowSelected.length; i++) {
            //            item = Data.RowSelected[i];
            //            if (!Helper.IsElementExistsInArray(parseInt(item), Data.RowSelectedSite))
            //                $("#Row" + item).addClass("active");
            //        }
            //    }
            //},
            //"order": [1, "asc"],
            //"scrollY": 300,
            //"scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            //"scrollCollapse": true,
            //"fixedColumns": {
            //    leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            //},
            //"createdRow": function (row, data, index) {
            //    /* Add Unique CSS Class to row as identifier in the cloned table */
            //    var id = $(row).attr("id");
            //    $(row).addClass(id);
            //},
        });
    },

    Export: function (dataType, month) {
        var year = $("#sslYear").val();
        var customerid = $("#slSearchCustomerID").val();
        window.location.href = "/DashboardRA/OverdueRTI/Export?DataType=" + dataType + "&Month=" + month + "&Year=" + year + "&Customer=" + customerid + "";
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
            $(id).html("<option value='All' selected> All</option>")
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

function GetDetails(month, type) {
    Table.LoadData(month, type);


    $(".SummaryDetail").show();
}

function MonthName(dt) {
    mlist = ["JAN", "FEB", "MAR", "APR", "MEI", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"];
    return mlist[dt];
};

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
