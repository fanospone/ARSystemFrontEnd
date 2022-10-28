jQuery(document).ready(function () {
    trxDashboardInputTarget.Init();
    Control.BindSelectYearAll();
    Control.BindingSelectCompany();
    Control.BindingSelectOperator();

    $('#btSearch').click(function(e)
    {
        e.preventDefault();
        trxDashboardInputTarget.FormTargetDetail.hide()
        trxDashboardInputTarget.FormTargetSummary.show();
        trxDashboardInputTarget.Search();
    });
    $('#btReset').click(function (e) {
        e.preventDefault();
        trxDashboardInputTarget.Reset()
    });
    

});

var trxDashboardInputTarget = {
    Init: function () {
        var tblSummaryData = $('#tbltrxdashboard').dataTable({
            "filter": false,
            "destroy": true,
            "data": [],
            "ordering": false
        });
        trxDashboardInputTargetDetail.Init()

        $("#tbltrxdashboard tbody").unbind().on("click", ".lnkDetail", function (e) {
            let selector = $(this);
            let year = $(this).data('year');
            let month = $(this).data('month');
            let departmentCode = $(this).data('department');
            


            trxDashboardInputTarget.FormTargetSummary.hide();
            trxDashboardInputTarget.FormTargetDetail.show();
            trxDashboardInputTargetDetail.Search(departmentCode, year, month);
            e.preventDefault();
        });
        $("#pnlDetail #btBack").click(function () {
            trxDashboardInputTarget.FormTargetDetail.hide();
            trxDashboardInputTarget.FormTargetSummary.show();
            trxDashboardInputTargetDetail.Init()
        });

        $(window).resize(function () {
            $("#tbltrxARInputForecastVsActual").DataTable().columns.adjust().draw();
        });

        // unremark if you call funtion
        //Table.trxARInputForecastVsActual.Search();
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        App.blockUI({
            target: "#gridPnltrxDashboard", boxed: true
        });

        params = {
            Year: $('#slSearchPeriod').val(),
            CompanyInvoiceID: $('#slSearchCompany').val(),
            CustomerID: $('#slSearchCustomer').val(),
        };
      

        var tblSummaryData = $("#tbltrxdashboard").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/ApiDashboardInputTarget/GetListDashboard",
                "type": "POST",
                "datatype": "json",
                "data": params,
                "dataSrc": function (json) {
                    var tempData = [];
                    var tempDataTotal = {
                        DepartmentName: 'RA Division', DepartmentCode: 'Total', Year: json.data[0].Year, Jan_AmountIDR: 0, Jan_OptimistIDR: 0, Jan_MostLikelyIDR: 0, Jan_PessimistIDR: 0, Feb_AmountIDR: 0, Feb_OptimistIDR: 0, Feb_MostLikelyIDR: 0, Feb_PessimistIDR: 0,
                        Mar_AmountIDR: 0, Mar_OptimistIDR: 0, Mar_MostLikelyIDR: 0, Mar_PessimistIDR: 0,Apr_AmountIDR: 0, Apr_OptimistIDR: 0, Apr_MostLikelyIDR: 0, Apr_PessimistIDR: 0,
                        May_AmountIDR: 0, May_OptimistIDR: 0, May_MostLikelyIDR: 0, May_PessimistIDR: 0,Jun_AmountIDR: 0, Jun_OptimistIDR: 0, Jun_MostLikelyIDR: 0, Jun_PessimistIDR: 0,
                        Jul_AmountIDR: 0, Jul_OptimistIDR: 0, Jul_MostLikelyIDR: 0, Jul_PessimistIDR: 0,Aug_AmountIDR: 0, Aug_OptimistIDR: 0, Aug_MostLikelyIDR: 0, Aug_PessimistIDR: 0,
                        Sep_AmountIDR: 0, Sep_OptimistIDR: 0, Sep_MostLikelyIDR: 0, Sep_PessimistIDR: 0,Oct_AmountIDR: 0, Oct_OptimistIDR: 0, Oct_MostLikelyIDR: 0, Oct_PessimistIDR: 0,
                        Nov_AmountIDR: 0, Nov_OptimistIDR: 0, Nov_MostLikelyIDR: 0, Nov_PessimistIDR: 0,Dec_AmountIDR: 0, Dec_OptimistIDR: 0, Dec_MostLikelyIDR: 0, Dec_PessimistIDR: 0
                    };
                    for (var i = 0; i < (json.data.length) ; i++) {
                        tempDataTotal.Jan_AmountIDR += parseInt(json.data[i].Jan_AmountIDR) || 0;
                        tempDataTotal.Jan_OptimistIDR += parseInt(json.data[i].Jan_OptimistIDR) || 0;
                        tempDataTotal.Jan_MostLikelyIDR += parseInt(json.data[i].Jan_MostLikelyIDR) || 0;
                        tempDataTotal.Jan_PessimistIDR += parseInt(json.data[i].Jan_PessimistIDR) || 0;

                        tempDataTotal.Feb_AmountIDR += parseInt(json.data[i].Feb_AmountIDR) || 0
                        tempDataTotal.Feb_OptimistIDR += parseInt(json.data[i].Feb_OptimistIDR) || 0
                        tempDataTotal.Feb_MostLikelyIDR += parseInt(json.data[i].Feb_MostLikelyIDR) || 0
                        tempDataTotal.Feb_PessimistIDR += parseInt(json.data[i].Feb_PessimistIDR) || 0

                        tempDataTotal.Mar_AmountIDR += parseInt(json.data[i].Mar_AmountIDR) || 0
                        tempDataTotal.Mar_OptimistIDR += parseInt(json.data[i].Mar_OptimistIDR) || 0
                        tempDataTotal.Mar_MostLikelyIDR += parseInt(json.data[i].Mar_MostLikelyIDR) || 0
                        tempDataTotal.Mar_PessimistIDR += parseInt(json.data[i].Mar_PessimistIDR) || 0

                        tempDataTotal.Apr_AmountIDR += parseInt(json.data[i].Apr_AmountIDR) || 0
                        tempDataTotal.Apr_OptimistIDR += parseInt(json.data[i].Apr_OptimistIDR) || 0
                        tempDataTotal.Apr_MostLikelyIDR += parseInt(json.data[i].Apr_MostLikelyIDR) || 0
                        tempDataTotal.Apr_PessimistIDR += parseInt(json.data[i].Apr_PessimistIDR) || 0

                        tempDataTotal.May_AmountIDR += parseInt(json.data[i].May_AmountIDR) || 0
                        tempDataTotal.May_OptimistIDR += parseInt(json.data[i].May_OptimistIDR) || 0
                        tempDataTotal.May_MostLikelyIDR += parseInt(json.data[i].May_MostLikelyIDR) || 0
                        tempDataTotal.May_PessimistIDR += parseInt(json.data[i].May_PessimistIDR) || 0

                        tempDataTotal.Jun_AmountIDR += parseInt(json.data[i].Jun_AmountIDR) || 0
                        tempDataTotal.Jun_OptimistIDR += parseInt(json.data[i].Jun_OptimistIDR) || 0
                        tempDataTotal.Jun_MostLikelyIDR += parseInt(json.data[i].Jun_MostLikelyIDR) || 0
                        tempDataTotal.Jun_PessimistIDR += parseInt(json.data[i].Jun_PessimistIDR) || 0

                        tempDataTotal.Jul_AmountIDR += parseInt(json.data[i].Jul_AmountIDR) || 0
                        tempDataTotal.Jul_OptimistIDR += parseInt(json.data[i].Jul_OptimistIDR) || 0
                        tempDataTotal.Jul_MostLikelyIDR += parseInt(json.data[i].Jul_MostLikelyIDR) || 0
                        tempDataTotal.Jul_PessimistIDR += parseInt(json.data[i].Jul_PessimistIDR) || 0

                        tempDataTotal.Aug_AmountIDR += parseInt(json.data[i].Aug_AmountIDR) || 0
                        tempDataTotal.Aug_OptimistIDR += parseInt(json.data[i].Aug_OptimistIDR) || 0
                        tempDataTotal.Aug_MostLikelyIDR += parseInt(json.data[i].Aug_MostLikelyIDR) || 0
                        tempDataTotal.Aug_PessimistIDR += parseInt(json.data[i].Aug_PessimistIDR) || 0

                        tempDataTotal.Sep_AmountIDR += parseInt(json.data[i].Sep_AmountIDR) || 0
                        tempDataTotal.Sep_OptimistIDR += parseInt(json.data[i].Sep_OptimistIDR) || 0
                        tempDataTotal.Sep_MostLikelyIDR += parseInt(json.data[i].Sep_MostLikelyIDR) || 0
                        tempDataTotal.Sep_PessimistIDR += parseInt(json.data[i].Sep_PessimistIDR) || 0

                        tempDataTotal.Oct_AmountIDR += parseInt(json.data[i].Oct_AmountIDR) || 0
                        tempDataTotal.Oct_OptimistIDR += parseInt(json.data[i].Oct_OptimistIDR) || 0
                        tempDataTotal.Oct_MostLikelyIDR += parseInt(json.data[i].Oct_MostLikelyIDR) || 0
                        tempDataTotal.Oct_PessimistIDR += parseInt(json.data[i].Oct_PessimistIDR) || 0

                        tempDataTotal.Nov_AmountIDR += parseInt(json.data[i].Nov_AmountIDR) || 0
                        tempDataTotal.Nov_OptimistIDR += parseInt(json.data[i].Nov_OptimistIDR) || 0
                        tempDataTotal.Nov_MostLikelyIDR += parseInt(json.data[i].Nov_MostLikelyIDR) || 0
                        tempDataTotal.Nov_PessimistIDR += parseInt(json.data[i].Nov_PessimistIDR) || 0

                        tempDataTotal.Dec_AmountIDR += parseInt(json.data[i].Dec_AmountIDR) || 0
                        tempDataTotal.Dec_OptimistIDR += parseInt(json.data[i].Dec_OptimistIDR) || 0
                        tempDataTotal.Dec_MostLikelyIDR += parseInt(json.data[i].Dec_MostLikelyIDR) || 0
                        tempDataTotal.Dec_PessimistIDR += parseInt(json.data[i].Dec_PessimistIDR) || 0


                        tempData.push({
                            DepartmentName: json.data[i].DepartmentName,
                            DepartmentCode: json.data[i].DepartmentCode,
                            Year: json.data[i].Year,
                            Jan_AmountIDR: parseInt(json.data[i].Jan_AmountIDR) || 0,
                            Jan_OptimistIDR: parseInt(json.data[i].Jan_OptimistIDR) || 0,
                            Jan_MostLikelyIDR: parseInt(json.data[i].Jan_MostLikelyIDR) || 0,
                            Jan_PessimistIDR: parseInt(json.data[i].Jan_PessimistIDR) || 0,

                            Feb_AmountIDR: parseInt(json.data[i].Feb_AmountIDR) || 0,
                            Feb_OptimistIDR: parseInt(json.data[i].Feb_OptimistIDR) || 0,
                            Feb_MostLikelyIDR: parseInt(json.data[i].Feb_MostLikelyIDR) || 0,
                            Feb_PessimistIDR: parseInt(json.data[i].Feb_PessimistIDR) || 0,

                            Mar_AmountIDR: parseInt(json.data[i].Mar_AmountIDR) || 0,
                            Mar_OptimistIDR: parseInt(json.data[i].Mar_OptimistIDR) || 0,
                            Mar_MostLikelyIDR: parseInt(json.data[i].Mar_MostLikelyIDR) || 0,
                            Mar_PessimistIDR: parseInt(json.data[i].Mar_PessimistIDR) || 0,

                            Apr_AmountIDR: parseInt(json.data[i].Apr_AmountIDR) || 0,
                            Apr_OptimistIDR: parseInt(json.data[i].Apr_OptimistIDR) || 0,
                            Apr_MostLikelyIDR: parseInt(json.data[i].Apr_MostLikelyIDR) || 0,
                            Apr_PessimistIDR: parseInt(json.data[i].Apr_PessimistIDR) || 0,

                            May_AmountIDR: parseInt(json.data[i].May_AmountIDR) || 0,
                            May_OptimistIDR: parseInt(json.data[i].May_OptimistIDR) || 0,
                            May_MostLikelyIDR: parseInt(json.data[i].May_MostLikelyIDR) || 0,
                            May_PessimistIDR: parseInt(json.data[i].May_PessimistIDR) || 0,

                            Jun_AmountIDR: parseInt(json.data[i].Jun_AmountIDR) || 0,
                            Jun_OptimistIDR: parseInt(json.data[i].Jun_OptimistIDR) || 0,
                            Jun_MostLikelyIDR: parseInt(json.data[i].Jun_MostLikelyIDR) || 0,
                            Jun_PessimistIDR: parseInt(json.data[i].Jun_PessimistIDR) || 0,

                            Jul_AmountIDR: parseInt(json.data[i].Jul_AmountIDR) || 0,
                            Jul_OptimistIDR: parseInt(json.data[i].Jul_OptimistIDR) || 0,
                            Jul_MostLikelyIDR: parseInt(json.data[i].Jul_MostLikelyIDR) || 0,
                            Jul_PessimistIDR: parseInt(json.data[i].Jul_PessimistIDR) || 0,

                            Aug_AmountIDR: parseInt(json.data[i].Aug_AmountIDR) || 0,
                            Aug_OptimistIDR: parseInt(json.data[i].Aug_OptimistIDR) || 0,
                            Aug_MostLikelyIDR: parseInt(json.data[i].Aug_MostLikelyIDR) || 0,
                            Aug_PessimistIDR: parseInt(json.data[i].Aug_PessimistIDR) || 0,

                            Sep_AmountIDR: parseInt(json.data[i].Sep_AmountIDR) || 0,
                            Sep_OptimistIDR: parseInt(json.data[i].Sep_OptimistIDR) || 0,
                            Sep_MostLikelyIDR: parseInt(json.data[i].Sep_MostLikelyIDR) || 0,
                            Sep_PessimistIDR: parseInt(json.data[i].Sep_PessimistIDR) || 0,

                            Oct_AmountIDR: parseInt(json.data[i].Oct_AmountIDR) || 0,
                            Oct_OptimistIDR: parseInt(json.data[i].Oct_OptimistIDR) || 0,
                            Oct_MostLikelyIDR: parseInt(json.data[i].Oct_MostLikelyIDR) || 0,
                            Oct_PessimistIDR: parseInt(json.data[i].Oct_PessimistIDR) || 0,

                            Nov_AmountIDR: parseInt(json.data[i].Nov_AmountIDR) || 0,
                            Nov_OptimistIDR: parseInt(json.data[i].Nov_OptimistIDR) || 0,
                            Nov_MostLikelyIDR: parseInt(json.data[i].Nov_MostLikelyIDR) || 0,
                            Nov_PessimistIDR: parseInt(json.data[i].Nov_PessimistIDR) || 0,

                            Dec_AmountIDR: parseInt(json.data[i].Dec_AmountIDR) || 0,
                            Dec_OptimistIDR: parseInt(json.data[i].Dec_OptimistIDR) || 0,
                            Dec_MostLikelyIDR: parseInt(json.data[i].Dec_MostLikelyIDR) || 0,
                            Dec_PessimistIDR: parseInt(json.data[i].Dec_PessimistIDR) || 0,
                        });
                    }
                    tempData.push(tempDataTotal);
                    return tempData;
                }
            },
            buttons: [

             { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
             {
                 text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                     var l = Ladda.create(document.querySelector(".yellow"));
                     l.start();
                     trxDashboardInputTarget.Export(params);
                     l.stop();
                 }
             },
             { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu":false,
            "destroy": true,
            "paging" : false,
            "columns": [

            { data: 'DepartmentName' },
            {
                data: 'Jan_AmountIDR', render: function (data, type, row, meta) {
                    var value = Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                    return '<a class="lnkDetail" data-year="' + row.Year + '" data-month="1", data-department="' + row.DepartmentCode + '">' + value + '</a>'
                }
            }, {
                data: 'Jan_OptimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Jan_MostLikelyIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Jan_PessimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            },

            {
                data: 'Feb_AmountIDR', render: function (data, type, row, meta) {
                    var value = Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                    return '<a class="lnkDetail" data-year="' + row.Year + '" data-month="2", data-department="' + row.DepartmentCode + '">' + value + '</a>'
                }
            }, {
                data: 'Feb_OptimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Feb_MostLikelyIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Feb_PessimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            },

            {
                data: 'Mar_AmountIDR', render: function (data, type, row, meta) {
                    var value = Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                    return '<a class="lnkDetail" data-year="' + row.Year + '" data-month="3", data-department="' + row.DepartmentCode + '">' + value + '</a>'
                }
            }, {
                data: 'Mar_OptimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Mar_MostLikelyIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Mar_PessimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            },

            {
                data: 'Apr_AmountIDR', render: function (data, type, row, meta) {
                    var value = Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                    return '<a class="lnkDetail" data-year="' + row.Year + '" data-month="4", data-department="' + row.DepartmentCode + '">' + value + '</a>'
                }
            }, {
                data: 'Apr_OptimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Apr_MostLikelyIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Apr_PessimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            },

            {
                data: 'May_AmountIDR', render: function (data, type, row, meta) {
                    var value = Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                    return '<a class="lnkDetail" data-year="' + row.Year + '" data-month="5", data-department="' + row.DepartmentCode + '">' + value + '</a>'
                }
            }, {
                data: 'May_OptimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'May_MostLikelyIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'May_PessimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            },

            {
                data: 'Jun_AmountIDR', render: function (data, type, row, meta) {
                    var value = Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                    return '<a class="lnkDetail" data-year="' + row.Year + '" data-month="6", data-department="' + row.DepartmentCode + '">' + value + '</a>'
                }
            }, {
                data: 'Jun_OptimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Jun_MostLikelyIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Jun_PessimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            },
            {
                data: 'Jul_AmountIDR', render: function (data, type, row, meta) {
                    var value = Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                    return '<a class="lnkDetail" data-year="' + row.Year + '" data-month="7", data-department="' + row.DepartmentCode + '">' + value + '</a>'
                }
            }, {
                data: 'Jul_OptimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Jul_MostLikelyIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Jul_PessimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            },

            {
                data: 'Aug_AmountIDR', render: function (data, type, row, meta) {
                    var value = Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                    return '<a class="lnkDetail" data-year="' + row.Year + '" data-month="8", data-department="' + row.DepartmentCode + '">' + value + '</a>'
                }
            }, {
                data: 'Aug_OptimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Aug_MostLikelyIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Aug_PessimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            },
            {
                data: 'Sep_AmountIDR', render: function (data, type, row, meta) {
                    var value = Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                    return '<a class="lnkDetail" data-year="' + row.Year + '" data-month="9", data-department="' + row.DepartmentCode + '">' + value + '</a>'
                }
            }, {
                data: 'Sep_OptimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Sep_MostLikelyIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Sep_PessimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            },
            {
                data: 'Oct_AmountIDR', render: function (data, type, row, meta) {
                    var value = Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                    return '<a class="lnkDetail" data-year="' + row.Year + '" data-month="10", data-department="' + row.DepartmentCode + '">' + value + '</a>'
                }
            }, {
                data: 'Oct_OptimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Oct_MostLikelyIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Oct_PessimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            },
            {
                data: 'Nov_AmountIDR', render: function (data, type, row, meta) {
                    var value = Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                    return '<a class="lnkDetail" data-year="' + row.Year + '" data-month="11", data-department="' + row.DepartmentCode + '">' + value + '</a>'
                }
            }, {
                data: 'Nov_OptimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Nov_MostLikelyIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Nov_PessimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            },
            {
                data: 'Dec_AmountIDR', render: function (data, type, row, meta) {
                    var value = Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                    return '<a class="lnkDetail" data-year="' + row.Year + '" data-month="12", data-department="' + row.DepartmentCode + '">' + value + '</a>'
                }
            }, {
                data: 'Dec_OptimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Dec_MostLikelyIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            }, {
                data: 'Dec_PessimistIDR', render: function (data, type, row, meta) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data / 1000000 :data).toFixed(0));
                }
            },
            ],
  
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "fnDrawCallback": function () {
               
                $('span.syeargrid').html(params.Year)
                l.stop();
                App.unblockUI('#gridPnltrxDashboard');
               
            },
            "createdRow": function (row, data, index) {
                if (data.DepartmentName == 'RA Division') {
                    $('td', row).addClass('totalRow')
                }
                $('td', row).addClass('text-center')

            },
            "order": []
        });
    },
    Export: function () {

        varYear = $('#fyear').val();
        varQuarter = $('#fquarter').val();

        window.location.href = "/Dashboard/DashboardInputTargetHeader/Export?CompanyInvoiceID=" + params.CompanyInvoiceID + "&CustomerID=" + params.CustomerID +
            "&DepartmentCode=" + params.DepartmentCode + "&Month=" + params.Month + "&Year=" + params.Year
    },
    Reset: function () {
        Control.BindSelectYearAll();
        Control.BindingSelectCompany();
        Control.BindingSelectOperator();
        trxDashboardInputTarget.FormTargetDetail.hide();
        trxDashboardInputTarget.FormTargetSummary.show();
        trxDashboardInputTarget.Search();
    },
    InitModal: function () {
        $("#btCloseFcactual").unbind().click(function (e) {
            e.preventDefault();
            $('#mdlUpdateForecastActual').modal('hide');
        });
        $("#btSubmitFcactual").unbind().click(function (e) {
            e.preventDefault();
            ModalUploadInflasi.SubmitUpload();
        });
        $("#mdlUpdateForecastActual").modal({ backdrop: 'static', keyboard: false }).show();
    },
    ResetModal: function () { //Modal.Reset
        var srt = $("script#mdlUpdateForecastActualHtml").html();
        $("#mdlUpdateForecastContainer").html(srt);
    },
    submitUpdatePica: function (ele) {
        var textarea = $(ele).parent().find('textarea').val();
        var operator = $(ele).parent().find('.pica-operator').val();
        alert(textarea);
        var l = Ladda.create(document.querySelector(".btnSavePica"))
        l.start();
        App.blockUI({
            target: "#gridPnltrxARInputForecastVsActual", boxed: true
        });
        var url = "/api/ApiInputForecastCashIn/submitForecastVsActual";
        $.ajax({
            url: url,
            type: "post",
            dataType: "json",
            data: {
                PiCa: textarea,
                OperatorID: operator,
                UpdateType: 'Update Pica'
            },
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            l.stop(); App.unblockUI('gridPnltrxARInputForecastVsActual');
            if (typeof data.Message != "undefined") {
            } else {
                Common.Alert.Success("Data has been saved!")
                $('.popover-edit-pica-grid').popover('hide')
                trxARInputForecastVsActual.Search();
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            if (textStatus == "error" && errorThrown == "") {
                Common.Alert.Warning("Error.")
                l.stop(); App.unblockUI('gridPnltrxARInputForecastVsActual');
            } else {
                Common.Alert.Error(errorThrown)
                l.stop(); App.unblockUI('#gridPnltrxARInputForecastVsActual');
            }
        });
    },

    FormTargetSummary: {
        show: function () {
            $("#pnlSummary").show();
        },
        hide: function () {
            $("#pnlSummary").hide();
        }
    },
    FormTargetDetail: {
        show: function () {
            $("#pnlDetail").show();
      

        },
        hide: function () {
            $("#pnlDetail").hide();
        }
    },
}

var trxDashboardInputTargetDetail = {
    Init: function () {
        var tblSummaryData = $('#tbltrxdashboardDetail').dataTable({
            "filter": false,
            "destroy": true,
            "data": [],
            "ordering": false
        });
    },
    Search: function (departmentCode, year, month) {
        var dept = (departmentCode == 'Total') ? 'RA Division' : departmentCode;
        $('#detail-title').html(' - ' + dept + ' (' + year + '-' + month + ')')

        App.blockUI({
            target: "#pnlDetail", boxed: true
        });

        params = {
            Year: year,
            Month: month,
            DepartmentCode: departmentCode,
            CompanyInvoiceID: $('#slSearchCompany').val(),
            CustomerID: $('#slSearchCustomer').val(),
        };


        var tblSummaryData = $("#tbltrxdashboardDetail").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/ApiDashboardInputTarget/GetListDetail",
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
                       trxDashboardInputTargetDetail.Export(params );
                       l.stop();
                   }
               },
               { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[25, 50], ['25', '50']],
            "destroy": true,
            "columns": [

            { data: 'SONumber' },
            { data: 'SiteID' },
            { data: 'SiteName' },
            { data: 'CustomerSiteID' },
            { data: 'CustomerSiteName' },
            { data: 'CustomerID' },
            { data: 'CompanyInvoiceID' },
            { data: 'RegionalName' },
            {
                data: 'StartInvoiceDate', render: function (data) {
                    return (data != null) ? moment(data).format('DD MMM YYYY') : '';
                }
            },
            {
                data: 'EndInvoiceDate', render: function (data) {
                    return (data != null) ? moment(data).format('DD MMM YYYY') : '';
                }
            },
            {    
                data: 'AmountIDR', render: function (data) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data  : 0));
                }
            },
            {    
                data: 'AmountUSD', render: function (data) {
                    return Common.Format.CommaSeparationOnly((parseInt(data) ? data : 0));
                }
            },
            { data: 'DepartmentName' },

            ],

            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "fnDrawCallback": function () {

                App.unblockUI('#pnlDetail');
            },

            "order": []
        });
    },
    Export: function (params) {
        window.location.href = "/Dashboard/DashboardInputTarget/Export?CompanyInvoiceID=" + params.CompanyInvoiceID + "&CustomerID=" + params.CustomerID +
            "&DepartmentCode=" + params.DepartmentCode + "&Month=" + params.Month + "&Year=" + params.Year
    },
    Reset: function () {
        Filter.ddlfYear('#fyear');
        Filter.ddlfQuarter('#fquarter');
        trxARInputForecastVsActual.Init();
    },

}

var Control = {

    BindSelectYearAll: function () {
        var dt = new Date();
        var currentyear = dt.getFullYear();
        var elements = $("#slSearchPeriod");
        elements.html("");
        for (var i = -10; i <= 10; i++) {
            if (currentyear == (currentyear + i))
                elements.append("<option value='" + (currentyear + i) + "' selected >" + (currentyear + i) + "</option>");
            else
                elements.append("<option value='" + (currentyear + i) + "'>" + (currentyear + i) + "</option>");

        }

        elements.select2({ placeholder: "Select Year", width: null });
    },
    BindingSelectCompany: function () {
        var ids = "#slSearchCompany";
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $(ids).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(ids).append("<option value='" + $.trim(item.CompanyId) + "'>" + $.trim(item.CompanyId) + ' - ' + item.Company + "</option>");
                })
            }
            $(ids).select2({ placeholder: "Select Company Name", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectOperator: function () {
        var ids = "#slSearchCustomer";
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $(ids).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(ids).append("<option value='" + $.trim(item.OperatorId) + "'>" + $.trim(item.OperatorId) + ' - ' + item.Operator + "</option>");
                })
            }
            $(ids).select2({ placeholder: "Select Operator", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
}

jQuery.fn.extend({
    serializeObject: function () {
        var formdata = $(this).serializeArray();
        var data = {};
        $(formdata).each(function (index, obj) {
            if (obj.name == 'ActualM1' || obj.name == 'ActualM2' || obj.name == 'ActualM3' ||
                obj.name == 'FCFinanceM1' || obj.name == 'FCFinanceM2' || obj.name == 'FCFinanceM3' ||
                obj.name == 'FCRevenueAssuranceM1' || obj.name == 'FCRevenueAssuranceM2' || obj.name == 'FCRevenueAssuranceM3' ||
                obj.name == 'FCMarketingM1' || obj.name == 'FCMarketingM2' || obj.name == 'FCMarketingM3' ||
                obj.name == 'VarianceM1' || obj.name == 'VarianceM2' || obj.name == 'VarianceM3') {
                data[obj.name] = obj.value.replace(/,/g, "");
            } else {
                data[obj.name] = obj.value;
            }
        });
        return data;
    }
});