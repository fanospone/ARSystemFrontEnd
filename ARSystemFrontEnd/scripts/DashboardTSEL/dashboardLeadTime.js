var _YearBill = "";
var _MonthBill = "";
var _MonthBillName = "";
var _BapsType = "";
var _PowerType = "";
var _MonthSelected = "";
var _Targets = "";
var _NameOfMonth = "";
var _SOWName = "";
var _SectionName = "";

jQuery(document).ready(function () {
    Form.Init();
    

    $("#btSearch").unbind().click(function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        _YearBill = $('#slSearchYear').val();
        Bind.ChartLeadTime("TOWER");
        Bind.ChartLeadTime("NON-TOWER");
        Bind.ChartAchievementLeadTime("TOWER");
        Bind.ChartAchievementLeadTime("NON-TOWER");
        l.stop();
        //Bind.ChartAchivement();
        $('#pnlDetail').fadeOut();
        $('.panelSearchChart').fadeIn(1000);
    });

    $("#btBack").unbind().click(function () {
        $('#pnlDetail').fadeOut();
        $('.panelSearchChart').fadeIn(1000);
        _MonthBill = "";
        _BapsType = "";
        _MonthBillName = "";
        _Targets = "";
        _PowerType = "";
        _SectionName = "";
        _SOWName = "";
    });

    $("#btReset").unbind().click(function () {
        var dt = new Date();
        var currentyear = dt.getFullYear();
        $('#slSearchYear').val(currentyear).trigger("change");
    });

    var tblRaw = $('#tblRaw').DataTable({
        "filter": false,
        "destroy": true,
        "searchable": true,
        "data": []
    });

    $("#btSearch").trigger("click");
});

var Form = {
    Init: function () {
        $('.panelSearchChart').hide();
        Control.BindSelectYear($('#slSearchYear'));
        _MonthBill = "";
        _BapsType = "";
        _MonthBillName = "";
        _Targets = "";
        _PowerType = "";
    }
}

var Bind = {
    ChartLeadTime: function (SectionName) {
        $.ajax({
            url: "/api/DashboardTSELLeadTime/GetLeadTimeData",
            type: "GET",
            data: { YearBill: $('#slSearchYear').val(), SectionName: SectionName }
        })
        .done(function (data, textStatus, jqXHR) {
            if (SectionName == "TOWER") {
                Control.BindDataLeadTimeTower(data.SowName, data.DataList);
            }
            else {
                Control.BindDataLeadTimeNonTower(data.SowName, data.DataList);
            }
            
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    ChartAchievementLeadTime: function (SectionName) {
        $.ajax({
            url: "/api/DashboardTSELLeadTime/GetLeadTimeAchievement",
            type: "GET",
            data: { YearBill: $('#slSearchYear').val(), SectionName: SectionName }
        })
        .done(function (data, textStatus, jqXHR) {
            if (SectionName == "TOWER") {
                Control.BindDataAchievementTower(data.MonthList, data.DataList);
            }
            else {
                Control.BindDataAchievementNonTower(data.MonthList, data.DataList);
            }

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    ChartAchivement: function () {
        $.ajax({
            url: "/api/DashboardTSELLeadTime/GetDataAchievement",
            type: "GET",
            data: { YearBill: $('#slSearchYear').val() }
        })
            .done(function (data, textStatus, jqXHR) {
                
                Control.BindDataAchievementTower(data.MonthList, data.YearlyData);
                Control.BindDataAchievementNonTower(data.MonthList, data.YearlyData);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    Table: function () {
        $('.panelSearchChart').fadeOut();
        $('#pnlDetail').fadeIn(1000);
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var params = {
            YearBill: _YearBill,
            MonthBill: _MonthBill,
            BapsType: _BapsType,
            MonthBillName: _MonthBillName,
            PowerType: _PowerType,
            SecName: _SectionName,
            SOWName: _SOWName
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
                "url": "/api/DashboardTSEL/GetDetailSite",
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
                {
                    data: "SONumber", mRender: function (data, type, full) {
                        return "<a class='btDetail'>" + data + "</a>";
                    }
                },


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
                //{ data: "ISNULL(fnCalculateTotalHargaReconcile(invoiceStartDate, invoiceEndDate, basePrice, omPrice, 0, SoNumber, SiteID), 0)", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '')},

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
    TableTarget: function () {
        $('.panelSearchChart').fadeOut();
        $('#pnlDetail').fadeIn(1000);
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var params = {
            YearBill: _YearBill,
            MonthBill: _MonthBill,
            BapsType: _BapsType,
            MonthBillName: _MonthBillName,
            PowerType: _PowerType
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
                "url": "/api/DashboardTSEL/GetDetailTargetSite",
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
                        Table.Export("Target");
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
                {
                    data: "SONumber", mRender: function (data, type, full) {
                        return "<a class='btDetail'>" + data + "</a>";
                    }
                },


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
                //{ data: "ISNULL(fnCalculateTotalHargaReconcile(invoiceStartDate, invoiceEndDate, basePrice, omPrice, 0, SoNumber, SiteID), 0)", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '')},

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

    }
}

var Control = {
    BindDataLeadTimeTower: function (SowName,DataList) {
        $('#LeadTimeTower').highcharts({
            title: {
                text: 'LEADTIME RTI'
            },
            xAxis: {
                categories: SowName
            },
            yAxis: {
                allowDecimals: false,
                min: 0
                , title: {
                    text: 'Lead Time'
                }
            },
            credits: {
                enabled: false
            },
            labels: {
                items: [{
                    html: '',
                    style: {
                        left: '50px',
                        top: '18px',
                        color: 'black'
                    }
                }]
            },
            plotOptions: {
                series: {
                    cursor: 'pointer',
                    point: {
                        events: {
                            click: function () {
                                _SOWName = this.category;
                                _SectionName = "TOWER";
                                if (this.series.name != "Target") {

                                    if (this.series.name.includes("AVG")) {
                                        _SOWName = "";
                                    }

                                    Bind.Table();
                                }
                            }
                        }
                    }
                }
            },
            series: DataList
        });
    },
    BindDataLeadTimeNonTower: function (SowName, DataList) {
        $('#LeadTimeNonTower').highcharts({
            title: {
                text: 'LEADTIME RTI'
            },
            xAxis: {
                categories: SowName
            },
            yAxis: {
                allowDecimals: false,
                min: 0
                , title: {
                    text: 'Lead Time'
                }
            },
            credits: {
                enabled: false
            },
            labels: {
                items: [{
                    html: '',
                    style: {
                        left: '50px',
                        top: '18px',
                        color: 'black'
                    }
                }]
            },
            plotOptions: {
                series: {
                    cursor: 'pointer',
                    point: {
                        events: {
                            click: function () {
                                _SOWName = this.category;
                                _SectionName = "NON-TOWER";
                                if (this.series.name != "Target") {

                                    if (this.series.name.includes("AVG")) {
                                        _SOWName = "";
                                    }

                                    Bind.Table();
                                }
                            }
                        }
                    }
                }
            },
            series: DataList
        });
    },
    BindDataAchievementTower: function (MonthList, DataList) {
        $('#AchievementTower').highcharts({
            title: {
                text: 'Lead Time VS Achievement'
            },
            xAxis: {
                categories: MonthList,
                crosshair: true
            },
            yAxis: [{ // Primary yAxis
                labels: {
                    format: '{value} Mil',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                title: {
                    text: 'Achievement',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                }
            }, { // Secondary yAxis
                title: {
                    text: 'AVG LeadTime',
                    style: {
                        color: "red"
                    }
                },
                labels: {
                    format: '{value} Days',
                    style: {
                        color: "red"
                    }
                },
                opposite: true
            }],
            tooltip: {
                shared: true
            },
            credits: {
                enabled: false
            },
            labels: {
                items: [{
                    html: '',
                    style: {
                        left: '50px',
                        top: '18px',
                        color: 'black'
                    }
                }]
            },
            plotOptions: {
                series: {
                    cursor: 'pointer',
                    point: {
                        events: {
                            click: function () {
                                _MonthBillName = this.category;
                                _SectionName = "TOWER";
                                Bind.Table();
                            }
                        }
                    }
                }
            },
            series: DataList
        });
    },
    BindDataAchievementNonTower: function (MonthList, DataList) {
        $('#AchievementNonTower').highcharts({
            title: {
                text: 'Lead Time VS Achievement'
            },
            xAxis: {
                categories: MonthList,
                crosshair: true
            },
            yAxis: [{ // Primary yAxis
                labels: {
                    format: '{value} Mil',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                title: {
                    text: 'Achievement',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                }
            }, { // Secondary yAxis
                title: {
                    text: 'AVG LeadTime',
                    style: {
                        color: "red"
                    }
                },
                labels: {
                    format: '{value} Days',
                    style: {
                        color: "red"
                    }
                },
                opposite: true
            }],
            //tooltip: {
            //    shared: true
            //},
            credits: {
                enabled: false
            },
            labels: {
                items: [{
                    html: '',
                    style: {
                        left: '50px',
                        top: '18px',
                        color: 'black'
                    }
                }]
            },
            plotOptions: {
                series: {
                    cursor: 'pointer',
                    point: {
                        events: {
                            click: function () {
                                _MonthBillName = this.category;
                                _SectionName = "NON-TOWER";
                                Bind.Table();
                            }
                        }
                    }
                }
            },
            series: DataList
        });
    },
    //BindDataLeadTimeNonTower: function (DataList) {
    //    $('#LeadTimeNonTower').highcharts({
    //        title: {
    //            text: 'LEADTIME RTI'
    //        },
    //        xAxis: {
    //            categories: MonthList
    //        },
    //        yAxis: {
    //            allowDecimals: false,
    //            min: 0
    //            , title: {
    //                text: 'Lead Time'
    //            }
    //        },
    //        credits: {
    //            enabled: false
    //        },
    //        labels: {
    //            items: [{
    //                html: '',
    //                style: {
    //                    left: '50px',
    //                    top: '18px',
    //                    color: 'black'
    //                }
    //            }]
    //        },
    //        plotOptions: {
    //            series: {
    //                cursor: 'pointer',
    //                point: {
    //                    events: {
    //                        click: function () {
    //                            _MonthBillName = this.category;
    //                            if (this.series.name == "Target") {
    //                                Bind.TableTarget();
    //                            }
    //                            else {
    //                                Bind.Table();
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        },
    //        series: DataList
    //    });
    //},
    //BindDataAchievementTower: function (MonthList, DataList) {
    //    $('#AchievementTower').highcharts({
    //        title: {
    //            text: 'Lead Time VS Achievement'
    //        },
    //        xAxis: {
    //            categories: MonthList
    //        },
    //        yAxis: {
    //            allowDecimals: false,
    //            min: 0
    //            , title: {
    //                text: 'Lead Time'
    //            }
    //        },
    //        credits: {
    //            enabled: false
    //        },
    //        labels: {
    //            items: [{
    //                html: '',
    //                style: {
    //                    left: '50px',
    //                    top: '18px',
    //                    color: 'black'
    //                }
    //            }]
    //        },
    //        plotOptions: {
    //            series: {
    //                cursor: 'pointer',
    //                point: {
    //                    events: {
    //                        click: function () {
    //                            _MonthBillName = this.category;
    //                            if (this.series.name == "Target") {
    //                                Bind.TableTarget();
    //                            }
    //                            else {
    //                                Bind.Table();
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        },
    //        series: DataList
    //    });
    //},
    //BindDataAchievementNonTower: function (MonthList, DataList) {
    //    $('#AchievementNonTower').highcharts({
    //        title: {
    //            text: 'Lead Time VS Achievement'
    //        },
    //        xAxis: {
    //            categories: MonthList
    //        },
    //        yAxis: {
    //            allowDecimals: false,
    //            min: 0,
    //            title: {
    //                text: 'Lead Time'
    //            }
    //        },
    //        credits: {
    //            enabled: false
    //        },
    //        labels: {
    //            items: [{
    //                html: '',
    //                style: {
    //                    left: '50px',
    //                    top: '18px',
    //                    color: 'black'
    //                }
    //            }]
    //        },
    //        plotOptions: {
    //            series: {
    //                cursor: 'pointer',
    //                point: {
    //                    events: {
    //                        click: function () {
    //                            _MonthBillName = this.category;
    //                            if (this.series.name == "Target") {
    //                                Bind.TableTarget();
    //                            }
    //                            else {
    //                                Bind.Table();
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        },
    //        series: DataList
    //    });
    //},
    BindSelectYear: function (elements) {
        var dt = new Date();
        var currentyear = dt.getFullYear();
        elements.html("");

        for (var i = -20; i <= 20; i++) {
            if (currentyear == (currentyear + i))
                elements.append("<option value='" + (currentyear + i) + "' selected >" + (currentyear + i) + "</option>");
            else
                elements.append("<option value='" + (currentyear + i) + "'>" + (currentyear + i) + "</option>");

        }

        elements.select2({ placeholder: "Select Year", width: null });
    },
    MonthIntToString: function (intMonth) {
        var monthNames = ["", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        return monthNames[intMonth];
    }
}

var Table = {
    Export: function (TypeTrx) {

        if (TypeTrx == "Target") {
            window.location.href = "/DashboardTSEL/ExportDetailTargetSite?strYearBill=" + _YearBill + "&strMonthBill=" + _MonthBill + "&strTargets=" + _Targets
                + "&strBapsType=" + _BapsType + "&strMonthBillName=" + _MonthBillName + "&strPowerType=" + _PowerType
                + "&strSecName=" + _SectionName + "&strSOWName=" + _SOWName;
        }
        else {
            window.location.href = "/DashboardTSEL/ExportDetailSite?strYearBill=" + _YearBill + "&strMonthBill=" + _MonthBill + "&strTargets=" + _Targets
                + "&strBapsType=" + _BapsType + "&strMonthBillName=" + _MonthBillName + "&strPowerType=" + _PowerType
                + "&strSecName=" + _SectionName + "&strSOWName=" + _SOWName;
        }

    }
}
