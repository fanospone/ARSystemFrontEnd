var year = 0;
var exportMonth = 0;
let customerids = ['ALL'];
var customerid = "";
var groupby = "";
var groupby2 = "NON TSEL";
var defaultOpt = "XL";
var isLoadDataGridDetail = [];
var monthsName = ["JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"];

jQuery(document).ready(function () {


    $(".SummaryHeader").hide();
    $(".SummaryDetail").hide();
    $(".SummaryStatus").hide();

    $("#totalAchieve").text("Loading..");
    $("#totalTarget").text("Loading..");
    $("#totalNotAchieve").text("Loading..");

    groupby = $("#hdUserRole").val();
    Helper.BindingSelectOperator();
    Helper.BindYear();
    Helper.BindMonth();

    Data.DataChart();
    // Data.DataChartGroup(groupby2);
    Data.DataChartTotal(groupby);
    Data.DataChartGroup(groupby);

    Table.Init('#overdueRTITable');

    $('#btnExport').on('click', function () {
        Table.Export("All", "All");
    });
    $('#backToHeader').on('click', function () {
        $("#SummaryDetail").hide();
        $("#chartdivaverage").fadeIn();
    });
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

    yearNow = $("#sslYear").val();


    $('#sslYear').on('change', function () {
        //$(".SummaryDetail").hide();
        $("#totalAchieve").text("Loading..");
        $("#totalTarget").text("Loading..");
        $("#totalNotAchieve").text("Loading..");
        Data.DataChart();
        // Data.DataChartGroup(groupby2);
        Data.DataChartTotal(groupby);
        Data.DataChartGroup(groupby);
    });

    $('#slSearchCustomerID').on('change', function (ev) {
        //$(".SummaryDetail").hide();

        var diff = $('#slSearchCustomerID').val().filter(e => customerids.indexOf(e) == -1);
        //kalau nambah baru ALL -> hapus smua customer sisakan ALL
        //kalau nambah tapi ada ALL -> hapus ALL
        if (diff.length > 0) {
            if (diff.includes('ALL')) {
                $('#slSearchCustomerID').val('ALL');
                customerids = $('#slSearchCustomerID').val();
                $('#slSearchCustomerID').trigger('change');
            } else if ($('#slSearchCustomerID').val().includes('ALL')) {
                $('#slSearchCustomerID').val(diff)
                customerids = $('#slSearchCustomerID').val();
                $('#slSearchCustomerID').trigger('change');
            }
        }
       

        customerids = $('#slSearchCustomerID').val();
        $("#totalAchieve").text("Loading..");
        $("#totalTarget").text("Loading..");
        $("#totalNotAchieve").text("Loading..");
        Data.DataChart();
        Data.DataChartTotal(groupby);
        Data.DataChartGroup(groupby);

    });

    $("#total").val("0");

    $('#sslMonth').on('change', function () {
        //$(".SummaryDetail").hide();
        //Data.DataChartGroup(groupby2);
        Data.DataChartTotal(groupby);
        Data.DataChartGroup(groupby);
    });

    $('input[name="optionsDeptRadio"]').on('change', function () {
        //$(".SummaryDetail").hide();
        $("#totalAchieve").text("Loading..");
        $("#totalTarget").text("Loading..");
        $("#totalNotAchieve").text("Loading..");
        Data.DataChart();
        Data.DataChartTotal(groupby);
        Data.DataChartGroup(groupby);

    });
 
});

function fillTarget() {
    var months = [];
    for (i = 1; i <= 12; i++) {
        if ($("#month" + i).val()) {
            months.push($("#month" + i).val());
        }
        else {
            months.push(0);
        }
    }
    var operator = $('#slSearchCustomerID2').val();
    var yearfill = $('#sslYear2').val();
    var params = {
        isFilled: true,
        CustomerID: operator,
        year: yearfill,
        JAN: months[0],
        FEB: months[1],
        MAR: months[2],
        APR: months[3],
        MAY: months[4],
        JUN: months[5],
        JUL: months[6],
        AUG: months[7],
        SEP: months[8],
        OCT: months[9],
        NOV: months[10],
        DEC: months[11],
    }

    $.ajax({
        url: "/api/FulfilmentRTI/FillTarget",
        type: "POST",
        datatype: "json",
        contentType: "application/json",
        data: JSON.stringify(params),
        cache: false
    }).done(function (data, textStatus, jqXHR) {
        location.reload();
    }).fail(function (jqXHR, textStatus, errorThrown) {
        Common.Alert.Error(errorThrown)
    });
}

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
    $("#" + id).toggleClass('fullscreen');
}

var Data = {
    DataChart: function () {
        App.blockUI({
            target: "#cardchartdivmonthly .body", boxed: true
        });
        year = $("#sslYear").val();
        customerid = $("#slSearchCustomerID").val();
        departmentCode = [];
        $('input[name="optionsDeptRadio"]:checked').each(function () {
            departmentCode.push($(this).val());
        });

        $.ajax({
            url: "/api/FulfilmentRTI/DataChart",
            type: "POST",
            data: {
                customerId: customerid,
                year: year,
                departmentCode: departmentCode
            },
            success: function (data) {
                //hide chart dan grid detailnya
                if ($('#chartdivaverage').css('display') == 'none') {
                    $("#chartdivaverage").show();
                    $("#SummaryDetail").hide();
                }
                App.unblockUI("#cardchartdivmonthly .body");

                //hanya show chart setiap ada filter
                Data.summaryChart(data);
                $(".yearplusinfo").text("   " + year + " FOR " + customerid + " - In Bilion - Click on chart label for details");
                

            },
            error: function (xhr) {
                App.unblockUI("#cardchartdivmonthly .body");
            }
        });

    },

    DataChartGroup: function (groupby) {

        year = $("#sslYear").val();
        var month = $("#sslMonth").val();
        customerid = $("#slSearchCustomerID").val();
        departmentCode = [];
        $('input[name="optionsDeptRadio"]:checked').each(function () {
            departmentCode.push($(this).val());
        });

        $.ajax({
            url: "/api/FulfilmentRTI/DataChartByGroup",
            type: "POST",
            data: {
                groupby: 'Group',// year: year,
                customerId: customerid,
                year: year,
                departmentCode: departmentCode,
                month: month
            },
            success: function (data) {
                Data.summaryChartGroup(data);
                $(".yearbilion").text("   " + year + " - In Bilion");
            },
            error: function (xhr) {
            }

        });


    },

    DataChartTotal: function (groupby) {

        year = $("#sslYear").val();
        var month = $("#sslMonth").val();

        $.ajax({
            url: "/api/FulfilmentRTI/DataChartByGroup",
            type: "POST",
            data: {
                groupby: 'Group',// year: year,
                customerId: customerid,
                year: year,
                departmentCode: departmentCode,
                month: month
            },
            success: function (data) {
                var amountAchieve = 0;
                var amountNotAchieve = 0;
                for (i = 0; i < data.length; i++) {
                    amountAchieve += data[i].InvAmount;
                    amountNotAchieve += data[i].AmountTarget;
                }

                var dataRes = [
                  {
                      title: "Achieve",
                      amount: amountAchieve
                  },
                  {
                      title: "Not Achieve",
                      amount: amountNotAchieve
                  },
                ];

                Data.summaryChartTotal(dataRes);
                $(".yearbilion").text("   " + year + " - In Bilion");
            },
            error: function (xhr) {
            }

        });


    },

    summaryChart: function (data) {
        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end

            // Create chart instance
            var chart = am4core.create("chartdivaverage", am4charts.XYChart);
            

            chart.data = data;

            chart.cursor = new am4charts.XYCursor();
            chart.cursor.lineX.disabled = true;
            chart.cursor.lineY.disabled = true;

            // Create axes
            var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
            categoryAxis.dataFields.category = "MonthName";
            categoryAxis.renderer.grid.template.location = 0;
            categoryAxis.renderer.minGridDistance = 20;
            categoryAxis.renderer.cellStartLocation = 0.1;
            categoryAxis.renderer.cellEndLocation = 0.9;

            var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
            valueAxis.min = 0;
            valueAxis.title.text = "Amount (M)";

            // Create series
            function createSeries(field, name, stacked) {
                var series = chart.series.push(new am4charts.ColumnSeries());
                series.dataFields.valueY = field;
                series.dataFields.categoryX = "MonthName";
                series.name = name;
                series.columns.template.tooltipText = "{name}: [bold]{valueY}[/]";
                series.stacked = stacked;
                series.columns.template.width = am4core.percent(95);

                series.tooltip.label.events.on(
                    "hit",
                    ev => {
                        var item = ev.target.dataItem.component.tooltipDataItem.dataContext;

                        //Head.Init(item.Month);
                        year = $("#sslYear").val();
                        customerid = $("#slSearchCustomerID").val();
                        departmentCode = [];
                        $('input[name="optionsDeptRadio"]:checked').each(function () {
                            departmentCode.push($(this).val());
                        }); Table.LoadData(customerid, item.MonthName, year, departmentCode);
                    },
                    this
                 );
                series.events.on(
                    "hit",
                    ev => {
                        var item = ev.target.dataItem.component.tooltipDataItem.dataContext;

                        //Head.Init(item.Month);
                        year = $("#sslYear").val();
                        customerid = $("#slSearchCustomerID").val();
                        departmentCode = [];
                        $('input[name="optionsDeptRadio"]:checked').each(function () {
                            departmentCode.push($(this).val());
                        });
                        Table.LoadData(customerid, item.MonthName, year, departmentCode);
                    },
                    this
                );
            }

            createSeries("AmountTarget", "Target", false);
            createSeries("InvAmount", "Achievement", false);

            chart.legend = new am4charts.Legend();

        }); // end am4core.ready()

        Data.Header(data);
    },

    summaryChartGroup: function (data) {

        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end

            var chart = am4core.create("chartdiv", am4charts.XYChart);
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
            series1.columns.template.fillOpacity = 0.8;

            var bullet1 = series1.bullets.push(new am4charts.LabelBullet());
            bullet1.interactionsEnabled = false;
            bullet1.label.text = "{valueY.totalPercent.formatNumber('#.00')}%";
            bullet1.label.fill = am4core.color("#ffffff");
            bullet1.locationY = 0.5;

            var series3 = chart.series.push(new am4charts.ColumnSeries());
            series3.columns.template.width = am4core.percent(80);
            series3.columns.template.tooltipText =
              "-Target: \n{valueY} ({valueY.totalPercent.formatNumber('#.00')}%)";
            series3.name = "-Target";
            series3.dataFields.categoryX = "CustomerID";
            series3.dataFields.valueY = "AmountTarget";
            series3.dataFields.valueYShow = "totalPercent";
            series3.dataItems.template.locations.categoryX = 0.5;
            series3.stacked = true;
            series3.columns.template.fillOpacity = 0.8;
            series3.tooltip.pointerOrientation = "vertical";
            series3.stroke = am4core.color("#cc0000");
            series3.columns.template.fill = am4core.color("#cc0000");

            var bullet3 = series3.bullets.push(new am4charts.LabelBullet());
            bullet3.interactionsEnabled = false;
            bullet3.label.text = "{valueY.totalPercent.formatNumber('#.00')}%";
            bullet3.locationY = 0.5;
            bullet3.label.fill = am4core.color("#ffffff");

            chart.scrollbarX = new am4core.Scrollbar();

        }); // end am4core.ready()

    },

    summaryChartTotal: function (data) {

        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end

            var chart = am4core.create("chartdivtotal", am4charts.PieChart3D);
            chart.hiddenState.properties.opacity = 0; // this creates initial fade-in

            chart.data = data;

            chart.innerRadius = am4core.percent(10);
            chart.depth = 50;

            chart.legend = new am4charts.Legend();

            var series = chart.series.push(new am4charts.PieSeries3D());
            series.dataFields.value = "amount";
            series.dataFields.depthValue = "amount";
            series.dataFields.category = "title";
            series.slices.template.cornerRadius = 1;
            series.colors.step = 9;

            series.ticks.template.disabled = true;
            series.alignLabels = false;
            series.labels.template.text = "{value.percent.formatNumber('#.0')}%";
            series.labels.template.radius = am4core.percent(-40);
            series.labels.template.fill = am4core.color("white");

            series.labels.template.adapter.add("radius", function (radius, target) {
                if (target.dataItem && (target.dataItem.values.value.percent < 10)) {
                    return 0;
                }
                return radius;
            });

            series.labels.template.adapter.add("fill", function (color, target) {
                if (target.dataItem && (target.dataItem.values.value.percent < 10)) {
                    return am4core.color("#000");
                }
                return color;
            });
        }); // end am4core.ready()

    },

    Header: function (data) {
        var totalTarget = 0;
        var totalAchievement = 0;
        var totalNotAchieve = 0;

        for (i = 0; i < data.length; i++) {
            totalTarget += data[i].AmountTarget;
            totalAchievement += data[i].InvAmount;
        }

        totalNotAchieve = totalTarget - totalAchievement;
        if (totalNotAchieve < 0) {
            totalNotAchieve = 0;
        }

        $("#totalAchieve").text(totalAchievement.toFixed(2));
        $("#totalTarget").text(totalTarget.toFixed(2));
        $("#totalNotAchieve").text(totalNotAchieve.toFixed(2));

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

    LoadData: function (customerid, month, year, departmentCode) {
        var params = {
            Month: month,
            Year: year,
            CustomerID: customerid,
            departmentCode: departmentCode,
            Category: ''
        };
        Table.Init("#overdueRTITableAchivement");
        Table.Init("#overdueRTITableTarget");


        App.blockUI({
            target: "#cardchartdivmonthly .body", boxed: true
        });
        isLoadDataGridDetail = [];

        exportMonth = month;
        var tblListAchivement = Table.LoadDataAchivement(params);
        var tblListTarget = Table.LoadDataTarget(params);


    },
    LoadDataAchivement: function (params) {
        var idTb = "#overdueRTITableAchivement";
        
        params.Category = "Achivement";

        var tblListAchivement = $(idTb).DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/FulfilmentRTI/DataDetail",
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
                        Table.Export(exportMonth, "Achivement");
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50, 200, 99999], ['10', '25', '50', '200', 'Visible']],
            "destroy": true,
            "columns": [
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
                {
                    data: null, render: function (data) {
                        return ''; //return as null cause the achivement data is only used IDR
                    }
                },
                { data: "CurrentStatus" },

                {
                    data: "RTIDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "FinanceConfirmDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "CreateInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                }, {
                    data: "PostingInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "BillingCycle" },
                { data: "TypeBaps" }
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [1], "className": "text-center" }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                //cek index kalau kosong apped flagging kalau grid sudah terload
                if (isLoadDataGridDetail.indexOf("Achivement") === -1) {
                    isLoadDataGridDetail.push("Achivement");
                }
                //semua grid sudah terload
                if (isLoadDataGridDetail.length >= 2) {
                    App.unblockUI("#cardchartdivmonthly .body");
                    if ($('#SummaryDetail').css('display') == 'none') {
                        $("#chartdivaverage").hide();
                        $("#SummaryDetail").fadeIn();

                    }
                }
            },
        });
    },

    LoadDataTarget: function (params) {
        var idTb = "#overdueRTITableTarget";

        params.Category = "Target";

        var tblListTarget = $(idTb).DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/FulfilmentRTI/DataDetail",
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
                        Table.Export(exportMonth, "Target");
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50, 200, 99999], ['10', '25', '50', '200', 'Visible']],
            "destroy": true,
            "columns": [
                { data: "SoNumber" },
                { data: "SiteId" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CompanyId" },
                { data: "CustomerId" },
    
                { data: "MonthTarget" },
                { data: "YearTarget" },
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
                {
                    data: "InvoiceAmountIdrKursRate", render: function (data) {
                        if (data > 0) {
                            return Common.Format.CommaSeparation(data);
                        }
                        return '';
                    }
                },
                { data: "TypeBaps" },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [1], "className": "text-center" }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                //cek index kalau kosong apped flagging kalau grid sudah terload
                if (isLoadDataGridDetail.indexOf("Target") === -1) {
                    isLoadDataGridDetail.push("Target");
                }
                //semua grid sudah terload
                if (isLoadDataGridDetail.length >= 2) {
                    App.unblockUI("#cardchartdivmonthly .body");
                    if ($('#SummaryDetail').css('display') == 'none') {
                        $("#chartdivaverage").hide();
                        $("#SummaryDetail").fadeIn();

                    }
                }
            },
        });
    },


    Export: function (month, exportType) {
        var year = $("#sslYear").val();
        var customerid = $("#slSearchCustomerID").val();
        var departmentCode = [];
        $('input[name="optionsDeptRadio"]:checked').each(function () {
            departmentCode.push($(this).val());
        });
        var custIds = customerid.join(',')
        var deptCodes = departmentCode.join(',')
        var monthn = (month != null) ? monthsName.indexOf(month) + 1 : 0;

        window.location.href = "/DashboardRA/FulfilmentRTI/Export?CustomerID=" + custIds + "&Month=" + month + "&Year=" + year + "&DepartmentCode=" + deptCodes + "&ExportType=" + exportType;
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
            $(id).html("")
            if (Common.CheckError.List(data)) {
                $(id).append("<option value='ALL' selected='selected'>ALL OPERATOR</option>");
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

    CalculatePercentage: function (month) {
        var total = 0;

        for (i = 1; i <= 12; i++) {
            if ($("#month" + i).val()) {
                total += parseInt($("#month" + i).val());
            }
        }

        for (j = 1; j <= 12; j++) {
            if ($("#month" + j).val()) {
                var amount = parseInt($("#month" + j).val());
                var percentage = amount * 100 / total;
                $("#permonth" + j).val(parseFloat(percentage).toFixed(2));
            }
            else {
                $("#permonth" + j).val(0);
            }
        }

        $("#total").val(total);
    }
}


