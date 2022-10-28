var _YearBill = "";
var _SecName = "";
var _SowName = "";
var _IsOverdue = "";

jQuery(document).ready(function () {
    Form.Init();
    Highcharts.setOptions({
        lang: {
            thousandsSep: ','
        }
    });
    $("#btSearch").unbind().click(function () {
        _YearBill = $('#slSearchYear').val();
        //_MonthSelected = $('#slSearchMonth').val();

        Bind.Chart();
    });

    $("#divTotal").unbind().click(function () {
        _YearBill = $('#slSearchYear').val();
        _SecName = "";
        _SowName = "";
        Bind.Table();
    });

    $("#divTower").unbind().click(function () {
        _YearBill = $('#slSearchYear').val();
        _SecName = "TOWER";
        _SowName = "";
        Bind.Table();
    });

    $("#divNonTower").unbind().click(function () {
        _YearBill = $('#slSearchYear').val();
        _SecName = "NON-TOWER";
        _SowName = "";
        Bind.Table();
    });

    $("#btBack").unbind().click(function () {
        $('#pnlDetail').fadeOut();
        $('.panelSearchChart').fadeIn(1000);

    });

    $("#btReset").unbind().click(function () {
        var dt = new Date();
        var currentyear = dt.getFullYear();
        $('#slSearchYear').val(currentyear).trigger('change');

    });

    var tblRaw = $('#tblRaw').DataTable({
        "filter": false,
        "destroy": true,
        "searchable": true,
        "data": []
    });
});
var Form = {
    Init: function () {
        $('.panelSearchChart').hide();
        var dt = new Date();
      
        Control.BindSelectYear($('#slSearchYear'));
        _YearBill = dt.getFullYear();
       
        Bind.Chart();

    }
}

var Bind = {
    Chart: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        $.ajax({
            url: "/api/DashboardTSEL/GetDataOverdue",
            type: "GET",
            data: { YearBill: $('#slSearchYear').val() }
        })
            .done(function (data, textStatus, jqXHR) {
                $('#pnlDetail').fadeOut();
                $('.panelSearchChart').fadeIn(1000);
                $('#PercentSummary').html(data.PercentageTotal[0].Percentage.toFixed(2));
                $('#divCountSiteTotal').html("<b>Tenant : " + data.PercentageTotal[0].CountSite + "</b>");
                $('#PercentTower').html(data.PercentageTower[0].Percentage.toFixed(2));
                $('#divCountSiteTower').html("<b>Tenant : " + data.PercentageTower[0].CountSite + "</b>");
                $('#PercentNonTower').html(data.PercentageNonTower[0].Percentage.toFixed(2));
                $('#divCountSiteNonTower').html("<b>Tenant : " + data.PercentageNonTower[0].CountSite + "</b>");

                Control.Pie("Tower", data.ChartTower, 1, "TOWER", 0);
                Control.Pie("NonTower", data.ChartNonTower, 1, "NON-TOWER", 0);

                Control.Pie("vsTower", data.ChartVersusTower, 1, "TOWER", 1);
                Control.Pie("vsNonTower", data.ChartVersusNonTower, 1, "NON-TOWER", 1);
                l.stop();
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
                l.stop();
            });
    },

    Table: function () {
        $('.panelSearchChart').fadeOut();
        $('#pnlDetail').fadeIn(1000);
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var params = {
            YearBill: _YearBill,
            SecName: _SecName,
            SOWName: _SowName,
            IsOverdue: _IsOverdue
        }

        var tblRaw = $("#tblRaw").DataTable({
            "columnDefs": [
                { "searchable": true, "targets": 0 }
            ],
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/DashboardTSEL/GetDetailSiteOverdue",
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
                        Table.Export("Achievement");
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[25, 50, 100, 200, 7000], ['25', '50', '100', '200', '7000']],
            "destroy": true,
            "columns": [
                { data: "RowIndex" },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CustomerID" },
                { data: "RegionalName" },
                { data: "ProvinceName" },

                { data: "ResidenceName" },
                { data: "PoNumber" },
                { data: "MLANumber" },
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
                { data: "BapsType" },
                { data: "PowerTypeCode" },
                { data: "CustomerInvoice" },
                { data: "CompanyInvoice" },
                { data: "Company" },
                { data: "StipSiro" },
                { data: "Currency" },

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
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "DeductionAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "TotalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                {
                    data: "RTIDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "DateConfirm", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
            ],
            "columnDefs": [{ "targets": [0], "orderable": true }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                l.stop(); App.unblockUI('.panelSearchResult');
            },
            "order": [],
            "scrollY": 500, /* Enable vertical scroll to allow fixed columns */
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            //"fixedColumns": {
            //    leftColumns: 3 /* Set the 2 most left columns as fixed columns */
            //},
        });
    },
}


var Control = {
    BindSelectMonth: function (elements) {
        var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

        elements.html("");

        for (var i = 0; i < monthNames.length; i++) {
            elements.append("<option value='" + (i + 1) + "'>" + monthNames[i] + "</option>");
        }

        elements.select2({ placeholder: "Select Month", width: null });
    },
    BindSelectYear: function (elements) {
        var dt = new Date();
        var currentyear = dt.getFullYear();
        elements.html("");

        for (var i = -5; i <= 5; i++) {
            if (currentyear == (currentyear + i))
                elements.append("<option value='" + (currentyear + i) + "' selected >" + (currentyear + i) + "</option>");
            else
                elements.append("<option value='" + (currentyear + i) + "'>" + (currentyear + i) + "</option>");

        }

        elements.select2({ placeholder: "Select Year", width: null });
    },
    Pie: function (divId, data, isStatus, SecNameParam, isVersus) {
        var resData = [];
        var count = 0;
        for (i = 0; i < data.length; i++) {
            var obj = new Object();
            obj.name = data[i].Type;
            obj.y = data[i].Value;
            var tipe = data[i].Type.toLowerCase();
            if (divId == "vsTower" || divId == "vsNonTower") {
                if (tipe == "overdue")
                    obj.color = "#FF6666";
                else
                    obj.color = "#3399FF";
            }



            count = count + 1;
            var jsonString = JSON.stringify(obj);
            resData.push(obj);
        }
        $('#' + divId).highcharts({
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: {
                text: ''
            },
            tooltip: {
                pointFormat: '<b>{point.percentage:.2f}% ({point.y:,.0f})</b>'
            },
            accessibility: {
                point: {
                    valueSuffix: '%'
                }
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        distance: 5,
                        format: '<b>{point.name}</b>: {point.percentage:.2f}%  ({point.y:,.0f}) '
                    },
                    point: {
                        events: {
                            click: function (e) {
                                _SecName = SecNameParam;
                                if (isVersus == 0) {
                                    _SowName = e.point.name;
                                    _IsOverdue = "";
                                }
                                else {
                                    _SowName = "";
                                    _IsOverdue = e.point.name;
                                }

                                Bind.Table();

                            }
                        }
                    },
                    showInLegend: true
                }
            },
            credits: {
                enabled: false
            },
            series: [{
                name: '',
                innerSize: isStatus == 1 ? '0%' : '0%',
                data: resData
            }]

        });


    },
}

var Table = {
    Export: function (TypeTrx) {

        window.location.href = "/DashboardTSEL/ExportDetailSiteOverdue?strYearBill=" + _YearBill + "&strSectionName=" + _SecName + "&strSowName=" + _SowName
            + "&strIsOverdue=" + _IsOverdue ;


    }
}