Data = {};
var directorateCode;
var GroupBy;
var isGroupBy;
var boolGroupBy;
var accrueTypeSearch, activitySearch, departmentcodeSearch, detailCaseSearch;
Data.RowSelectedDocumentChecklist = [];

jQuery(document).ready(function () {
    DropDown.Init();
    DropDown.BindingDatePicker();
    Table.Init();

    $("#slsDirectorate").on("change", function () {
        directorateCode = $(this).val();
        if (directorateCode == 'ALL'){
            directorateCode = null;
        } else {
            directorateCode = directorateCode;
        }
        console.log("directorate: ", directorateCode);
    });
    $('#StartCreatedDate').datepicker();
    $('#EndCreatedDate').datepicker();

    $("#btResetRequest").unbind().click(function () {
        Table.Reset();
    });

    $("#btSearchRequest").unbind().click(function () {

        if ($("#rdoDepartment").is(":checked")) {
            GroupBy = "Department";
            isGroupBy = 1;
            boolGroupBy = true;

        } else if ($("#rdoDetailCase").is(":checked")) {
            GroupBy = "DetailCase";
            isGroupBy = 2;
            boolGroupBy = false;
        }
        $('.pnlDashboardHeader').hide();
        $('.pnlDashboardHeaderDetailCase').hide();
        $('.pnlDetail').hide();
        $('.pnlDetailCase').hide();

        console.log("GroupBy: ", GroupBy);

        Data.Chart();
        Data.ChartFinish();
    });

    $('#tblDashboardHeader input').keypress(function (e) {
        if (e.which == 13) {
            //console.log("Masuk gak ya? : ", accrueTypeSearch, activitySearch, departmentcodeSearch, detailCaseSearch);
            Table.SearchData(accrueTypeSearch, activitySearch, departmentcodeSearch, detailCaseSearch);
        }
    });

    $('#tblDashboardHeaderDetailCase input').keypress(function (e) {
        if (e.which == 13) {
            Table.SearchData(accrueTypeSearch, activitySearch, departmentcodeSearch, detailCaseSearch);
        }
    });

    $("#btExportToExcel").unbind().click(function () {
        Table.ExportAllDetail();
    });
    $("#btExportToExcelByDate").unbind().click(function () {
        if ($('#StartCreatedDate').val() > $('#EndCreatedDate')) {
            Common.Alert.Error("tanggal awal lebih besar dari tanggal akhir");
        }
        else {
            Table.ExportAllDetailByDate();
        }
    });
    Highcharts.Chart({
        lang: {
            thousandsSep: ','
        },
        colors: Highcharts.map(Highcharts.getOptions().colors, function (color) {
            return {
                radialGradient: {
                    cx: 0.5,
                    cy: 0.3,
                    r: 0.7
                },
                stops: [
                    [0, color],
                    [1, Highcharts.color(color).brighten(-0.3).get('rgb')] // darken
                ]
            };
        })
    });
});

var Table = {
    Init: function () {
        $('.pnlDashboardHeader').hide();
        $('.pnlDashboardHeaderDetailCase').hide();
        
    },

    SearchData: function (accrueType, activity, departmentcode, detailCase) {
        var RequestNumber;
        $('#pnlDetail').hide();
        $('#pnlDetailCase').hide();
        if (isGroupBy == 1) {
            $('.pnlDashboardHeader').show();
            var id = "#tblDashboardHeader";
            RequestNumber = $("#sRequestNumber").val().replace(/'/g, "''");
        } else {
            $('.pnlDashboardHeaderDetailCase').show();
            var id = "#tblDashboardHeaderDetailCase";
            RequestNumber = $("#sRequestNumberDetailCase").val().replace(/'/g, "''")
        }
        
        //Table.Init(id);
        //Table.Init("#tblDashboardDetail");
        //var l = Ladda.create(document.querySelector("#btSearchRequest"));
        //l.start();

        paramHeader = {
            RequestTypeID: $("#slsRequestTypeID").val(),
            RequestNumber: RequestNumber,//$("#sRequestNumber").val().replace(/'/g, "''"), //$("#sRequestNumber").val(),
            CreatedDate: $("#StartCreatedDate").val(),
            CreatedDate2: $("#EndCreatedDate").val(),
            DepartName: departmentcode,//$("#sDirectorat").val(),
            Activity: activity,//$("#slsActivityID").val(),
            SubmissionDateFrom: $("#StartCreatedDate").val(),
            SubmissionDateTo: $("#EndCreatedDate").val(),
            AccrueType: accrueType,
            DetailCase: detailCase,
            DirectorateCode: directorateCode
            //DirectorateCode: directorateCode
        };


        
        console.log("paramHeader: ", paramHeader);
        $(id + " tbody").hide();
        $(id).DataTable({
            "deferRender": false,
            "proccessing": false,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/DashboardStopAccrue/dashboardHeader",
                "type": "POST",
                "datatype": "json",
                "data": paramHeader,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.ExportHeader()
                        l.stop();
                    }
                },

            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "filter": false,
            "order": [[0, 'asc']],
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    data: "RowIndex", render: function (data) {
                        
                        if (data == "Total")
                            return "<b> </b>";
                        else
                            return data;
                    }
                }, {
                    render: function (x, y, data) {
                        if (data.RowIndex != "Total" && data.Activity.toLowerCase() == "finish")
                            return "<button  type='button ' class='btn  btn-xs green  downloadDoc' data-style='zoom-in'><i class='fa fa-download' title='Download'></i>&nbsp;Download</button>";
                        else
                            return "";
                    }
                },
                {
                    data: "RequestNumber", render: function (data) {
                        console.log("data: ", data);
                        return "<a class='linkDetail' title='Detail Request'>" + data + "</i>";
                    }
                },
                {
                    data: "DepartName"
                },
                
                { data: "Activity" },
                {
                    data: "SoNumberCount", render: function (a, b, c) {
                        if (c.RowIndex == "Total")
                            return "<b> </b>";
                        else
                            return c.SoNumberCount;
                    }
                },
                {
                    data: "SumRevenue", render: function (a, b, c) {
                        if (c.SumRevenue == null)
                            return "";
                        else {
                            if (c.RowIndex == "Total") {
                                return "<b> </b>";
                            } else {
                                return Common.Format.CommaSeparation(c.SumRevenue);
                            }
                        }

                    }
                },
                {
                    data: "SumCapex", render: function (a, b, c) {
                        if (c.SumCapex == null)
                            return "";
                        else {
                            if (c.RowIndex == "Total") {
                                return "<b> </b>";
                            } else {
                                return Common.Format.CommaSeparation(c.SumCapex);
                            }
                        }

                    }
                },
                {
                    data: "CraetedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "LastModifiedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "ID" },
                { data: "RequestStatus" },
                { data: "FileName" },
                { data: "AppHeaderID" },
                { data: "AccrueType" },
                { data: "IsReHold" },

            ],
            "columnDefs": [{ "targets": [10, 11, 12, 13, 14, 15], "visible": false }, { "targets": [2, 3, 4, 5, 8, 9], "class": "text-center" }, { "targets": [7, 6], "class": "text-right" }],
            "fnDrawCallback": function (row, data, start, end, display) {
                //l.stop();
                $(id + " tbody").fadeIn(1500);

               
                    var api = this.api(), data;
                    var colNumber = [5, 6, 7];


                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };


                    var colNo = colNumber[0];

                    amount = api
                        .column(colNo, { page: 'all' })
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);
                    $(api.column(colNo).footer()).html(amount);


                    var colNo = colNumber[1];

                    amount = api
                        .column(colNo, { page: 'all' })
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);
                    $(api.column(colNo).footer()).html(Common.Format.CommaSeparation(amount));

                    var colNo = colNumber[2];

                    amount = api
                        .column(colNo, { page: 'all' })
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);
                    $(api.column(colNo).footer()).html(Common.Format.CommaSeparation(amount));
                
            },

        });
        $(id + " tbody").unbind().on("click", ".linkDetail", function (e) {
            var table = $(id).DataTable();
            var row = table.row($(this).parents('tr')).data();
            //$("#pnlHeader").hide();
            //$("#pnlDetail").fadeIn(2000);
            //$("#pnlDetail").css('visibility', 'visible');
            console.log("rows: ", row);
            fsIsReHold = row.IsReHold;
            fsRequestNumber = row.RequestNumber;
            HeaderID = row.ID;
            fsActivity = row.Activity;


            //Table.LoadDetail();
            Table.LoadDetail(row.DepartName, row.SoNumberCount);
        });

        $(id + " tbody").on("click", ".downloadDoc", function (e) {
            var table = $(id).DataTable();
            var row = table.row($(this).parents('tr')).data();

            window.location.href = "/StopAccrue/downloadDocSubmit?fileName=" + row.FileName + "&activity=" + row.Activity + "&appHeaderID=" + row.AppHeaderID + "&accrueType=" + row.AccrueType;

        });
    },

    LoadDetail: function (departName, soNumberCount) {
        requestDetail = [];
        $(".timeline").html('');
        var params = {
            HeaderID: HeaderID,
            DepartName: departName,
            DeptOrDetailCase: isGroupBy,
            SoNumberCount: soNumberCount,
            RequestNumber: fsRequestNumber
        };

        console.log("HeaderID: ", params);
        $.ajax({
            url: "/api/DashboardStopAccrue/dashboardDetail",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
        }).done(function (data, textStatus, jqXHR) {
            requestDetail = data;
            Table.DetailTable();
            //Table.DetailTable();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
        fndraw
    },

    DetailTable: function () {
        if (isGroupBy == 1) {
            $('#pnlDetail').show();
            var id = "#tblDashboardDetail";
        } else {
            $('#pnlDetailCase').show();
            var id = "#tblDashboardDetailCase";
        }
        
        //Table.Init(id);
        $(id + " tbody").hide();
        $(id).DataTable({
            "serverSide": false,
            "filter": true,
            "destroy": true,
            "async": false,
            "data": requestDetail,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },

                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline btnExportDetail', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.ExportDetail();
                        l.stop();
                    }
                },

            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "columns": [
                {
                    data: "RowIndex", render: function (data) {
                        console.log("RowIndex: ", data);
                        if (data == "Total")
                            return "<b>" + data + "</b>";
                        else
                            return data;
                    }
                },
                {
                    render: function (x, y, data) {
                        if (data.RowIndex != "Total" && fsActivity.toLowerCase() == "finish")
                            return "<button  type='button ' class='btn  btn-xs green  downloadDoc' data-style='zoom-in'><i class='fa fa-download' title='Download'></i>&nbsp;Download</button>";
                        else
                            return "";
                    }
                },
                { data: "RequestNumber" },
                { data: "AccrueType" },
                { data: "Company" },
                { data: "SONumber" },
                { data: "SiteName" },
                { data: "Customer" },
                { data: "Product" },
                {
                    data: "RFIDone", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "RevenueAmount", render: function (a, b, c) {
                        if (c.RevenueAmount == null)
                            return "";
                        else {
                            if (c.RowIndex == "Total") {
                                return "<b>" + Common.Format.CommaSeparation(c.RevenueAmount) + "</b>";
                            } else {
                                return Common.Format.CommaSeparation(c.RevenueAmount);
                            }
                        }

                    }
                },
                {
                    data: "CapexAmount", render: function (a, b, c) {
                        if (c.CapexAmount == null)
                            return "";
                        else {
                            if (c.RowIndex == "Total") {
                                return "<b>" + Common.Format.CommaSeparation(c.CapexAmount) + "</b>";
                            } else {
                                return Common.Format.CommaSeparation(c.CapexAmount);
                            }
                        }
                    }
                },
                { data: "CategoryCase" },
                { data: "DetailCase" },
                {
                    data: "CompensationAmount", render: function (a, b, c) {
                        if (c.CompensationAmount == null)
                            return "";
                        else {
                            if (c.RowIndex == "Total") {
                                return "<b>" + Common.Format.CommaSeparation(c.CompensationAmount) + "</b>";
                            } else {
                                return Common.Format.CommaSeparation(c.CompensationAmount);
                            }
                        }
                    }
                },
                {
                    mRender: function (a, b, c) {
                        if (c.IsHold) {
                            return "Hold";
                        }
                        else {
                            return "Active";
                        }
                    },
                },
                {
                    data: "DepartName"
                },
                {
                    data: "StartEffectiveDate", render: function (a, b, c) {
                        if (c.IsHold)
                            return Common.Format.ConvertJSONDateTime(c.StartEffectiveDate);
                        else
                            return "";
                    }
                },
                {
                    data: "EndEffectiveDate", render: function (a, b, c) {
                        if (c.IsHold)
                            return Common.Format.ConvertJSONDateTime(c.EndEffectiveDate);
                        else
                            return "";
                    }
                },

                {
                    data: "Initiator"
                },
                {
                    data: "FileName"
                },
            ],
            "columnDefs": [{ "targets": [20], "visible": false }, { "targets": [9, 10, 13], "class": "text-right" }, { "targets": [1, 2, 3, 4, 5, 6, 7, 8, 11, 12, 14, 16, 17, 18], "class": "text-center" }],
            "fnDrawCallback": function () {
                $(id + " tbody").fadeIn(1000);
            },
            //"bInfo": false,
            "order": false,
            //"scrollY": 300,
            //"scrollX": true,
            //"scrollCollapse": true,
            //"fixedColumns": {
            //    leftColumns: 3 /* Set the 2 most left columns as fixed columns */
            //},

            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;
                //var colNumber = [9, 10, 13];
                var colNumber = [10, 11];

                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                        i : 0;
                };


                var colNo = colNumber[0];

                amount = api
                    .column(colNo, { page: 'all' })
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);
                $(api.column(colNo).footer()).html(Common.Format.CommaSeparation(amount));


                var colNo = colNumber[1];

                amount = api
                    .column(colNo, { page: 'all' })
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);
                $(api.column(colNo).footer()).html(Common.Format.CommaSeparation(amount));

                //var colNo = colNumber[2];

                //amount = api
                //    .column(colNo, { page: 'all' })
                //    .data()
                //    .reduce(function (a, b) {
                //        return intVal(a) + intVal(b);
                //    }, 0);
                //$(api.column(colNo).footer()).html(Common.Format.CommaSeparation(amount));
            }
        });
        var Colums = $(id).DataTable();
        if (fsIsReHold)
            Colums.column(14).visible(true);
        else
            Colums.column(14).visible(false);

        $(id + " tbody").on("click", ".downloadDoc", function (e) {
            var table = $(id).DataTable();
            var row = table.row($(this).parents('tr')).data();

            window.location.href = "/StopAccrue/downloadFile?fileName=" + row.FileName;

        });
    },

    ExportDetail: function () {
        window.location.href = "/StopAccrue/exportDashboardStopAccrueDetail?HeaderID=" + HeaderID + "&RequestNumber=" + fsRequestNumber + "&IsReHold=" + fsIsReHold;
    },

    ExportAllDetail: function () {
        window.location.href = "/StopAccrue/exportAllDashboardDetail?SubmissionFrom=" + $('#StartCreatedDate').val() + "&SubmissionTo=" + $('#EndCreatedDate').val() + "&DirectorateCode=" + directorateCode + "&GroupBy=" + GroupBy;
    },
    ExportAllDetailByDate: function () {
        window.location.href = "/StopAccrue/exportAllDashboardDetailByDate?SubmissionFrom=" + $('#StartCreatedDate').val() + "&SubmissionTo=" + $('#EndCreatedDate').val() + "&DirectorateCode=" + directorateCode + "&GroupBy=" + GroupBy;
    },

    ExportHeader: function () {
        window.location.href = "/StopAccrue/exportDashboardStopAccrueHeader?" + $.param(paramHeader);
    },

    Reset: function () {
        $("#slsDirectorate").val("").trigger('change');
        DropDown.BindingDatePicker();
    }
}

var DropDown = {
    Init: function(){
        DropDown.GetDirectorate();
        //DropDown.SubmissionDate();
    },
    GetDirectorate: function () {
        $.ajax({
            url: "/api/DashboardStopAccrue/GetDirectorate",
            type: "GET",
            async: false,
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slsDirectorate").html("<option>ALL</option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    
                    $("#slsDirectorate").append("<option value='" + item.Code + "'>" + item.Description + "</option>");

                })
            }
            $("#slsDirectorate").select2({ placeholder: "Directorate", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingDatePicker: function () {
        var d = new Date();
        var dateNow = new Date(d.getDate(), d.getMonth(), d.getFullYear());

        var todaydate = new Date();
        var day = todaydate.getDate();
        var month = todaydate.getMonth() + 1;
        var year = todaydate.getFullYear();
        var datestring = year + "-" + month + "-" + day; //day + "-" + month + "-" + year;
        var datestring2 = todaydate.getFullYear() + "-" + "1" + "-" + day //day + "-" + "1" + "-" + todaydate.getFullYear();

        var datestring = year + "-" + month + "-" + day;
        var datestring2 = todaydate.getFullYear() + "-" + "1" + "-" + day;
        console.log("date: ", datestring);
        $("#StartCreatedDate").val(datestring2);
        $("#EndCreatedDate").val(datestring);

        $('#StartCreatedDate').datepicker({
            format: "yyyy-mm-dd",
            autoclose: true,
            defaultDate: new Date()
        });

        $('#EndCreatedDate').datepicker({
            format: "yyyy-mm-dd",
            autoclose: true,
            defaultDate: new Date()
        });

    },
}

var Data = {
    Chart: function () {
        $.ajax({
            url: "/api/DashboardStopAccrue/CountData",
            type: "POST",
            data: {
                SubmissionDateFrom: $("#StartCreatedDate").val(),
                SubmissionDateTo: $("#EndCreatedDate").val(),
                DirectorateCode: directorateCode,
                DepartName: GroupBy,
                Activity: "ongoing"
            }
        }).done(function (data, textStatus, jqXHR) {
            console.log("dataChart: ", data);
            $('.panelSearchChart').fadeIn(1000);

            Chart.PieStopAccrueOnGoing("StopAccrueOnGoing", data.OngoingStop, "STOP", "ongoing");
            Chart.PieHoldAccrueOnGoing("HoldAccrueOnGoing", data.data, "HOLD", "ongoing");
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        })

    },

    ChartFinish: function () {
        // var l = Ladda.create(document.querySelector())
        $.ajax({
            url: "/api/DashboardStopAccrue/CountDataFinish",
            type: "POST",
            data: {
                SubmissionDateFrom: $("#StartCreatedDate").val(),
                SubmissionDateTo: $("#EndCreatedDate").val(),
                DirectorateCode: directorateCode,
                DepartName: GroupBy,
                Activity: "finish"
            }
        }).done(function (data, textStatus, jqXHR) {
            $('.panelSearchChart').fadeIn(1000);

            Chart.PieStopAccrueFinish("StopAccrueFinish", data.OngoingStop, "STOP", "finish");
            Chart.PieHoldAccrueFinish("HoldAccrueFinish", data.data, "HOLD", "finish");
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        })

    },
}

var Chart = {
    PieStopAccrueOnGoing: function (divId, data, accrueType, status) {
        console.log("charttt: ", divId, data, status)
        var resData = [];
        var colors = [];
        var count = 0;
        
        for (i = 0; i < data.length; i++) {
            var obj = new Object();

            if (GroupBy == "Department") {
                obj.name = data[i].DepartName;
                obj.y = data[i].CountData;
            } else {
                obj.name = data[i].DetailCase;
                obj.y = data[i].CountData;
            }
                        
            colors.push(data[i].Color);
            count = count + 1;
            var jsonString = JSON.stringify(obj);

            resData.push(obj);
            
        }

        if (data.length == 0) {
            $('#' + divId).hide();
            $('#StopAccrueOnGoingStatus').show();
        } else {
            $('#' + divId).show();
            $('#StopAccrueOnGoingStatus').hide();
            
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
                    //pointFormat: '<b> ({point.y:,.0f})</b>'
                    pointFormat: '<b>{point.y:.1f}%</b>'
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
                            format: '<b>{point.name}</b>: ({point.y:.0f}%) ' //'<b>{point.name}</b>: ({point.y:,.0f}) '
                        },
                        point: {
                            events: {
                                click: function (e) {
                                    DepartName = e.point.name;
                                    $('.pnlDetail').hide();
                                    console.log(accrueType + "-" + status + "-" + DepartName + "-" + GroupBy);
                                    if (GroupBy == "Department") {
                                        Table.SearchData(accrueType, status, DepartName, "1");
                                        detailCaseSearch = "1";
                                    } else {
                                        Table.SearchData(accrueType, status, DepartName, "2");
                                        detailCaseSearch = "2";
                                    }

                                    accrueTypeSearch = accrueType;
                                    activitySearch = status;
                                    departmentcodeSearch = DepartName;

                                }
                            }
                        },
                        showInLegend: true,
                        colors: colors
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: '',
                    //  innerSize: isStatus == 1 ? '0%' : '0%',
                    data: resData
                }]

            });
        }
       
    },

    PieHoldAccrueOnGoing: function (divId, data, accrueType, status) {
        console.log("charttt: ", divId, data, status)
        var resData = [];
        var colors = [];
        var count = 0;

        for (i = 0; i < data.length; i++) {
            var obj = new Object();

            if (GroupBy == "Department") {
                obj.name = data[i].DepartName;
                obj.y = data[i].CountData;
            } else {
                obj.name = data[i].DetailCase;
                obj.y = data[i].CountData;
            }



            colors.push(data[i].Color);
            count = count + 1;
            var jsonString = JSON.stringify(obj);

            resData.push(obj);

        }

        if (data.length == 0) {
            $('#' + divId).hide();
            $('#HoldAccrueOnGoingStatus').show();
        } else {
            $('#' + divId).show();
            $('#HoldAccrueOnGoingStatus').hide();
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
                    //pointFormat: '<b> ({point.y:,.0f})</b>'
                    pointFormat: '<b>{point.y:.1f}%</b>'
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
                            format: '<b>{point.name}</b>: ({point.y:.0f}%) '
                        },
                        point: {
                            events: {
                                click: function (e) {
                                    DepartName = e.point.name;
                                    $('.pnlDetail').hide();
                                    console.log(accrueType + "-" + status + "-" + DepartName + "-" + GroupBy);
                                    if (GroupBy == "Department") {
                                        Table.SearchData(accrueType, status, DepartName, "1");
                                        detailCaseSearch = "1";
                                    } else {
                                        Table.SearchData(accrueType, status, DepartName, "2");
                                        detailCaseSearch = "2";
                                    }

                                    accrueTypeSearch = accrueType;
                                    activitySearch = status;
                                    departmentcodeSearch = DepartName;

                                }
                            }
                        },
                        showInLegend: true,
                        colors: colors
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: '',
                    //  innerSize: isStatus == 1 ? '0%' : '0%',
                    data: resData
                }]

            });
        }



    },

    PieStopAccrueFinish: function (divId, data, accrueType, status) {
        console.log("charttt: ", divId, data, status)
        var resData = [];
        var colors = [];
        var count = 0;

        for (i = 0; i < data.length; i++) {
            var obj = new Object();

            if (GroupBy == "Department") {
                obj.name = data[i].DepartName;
                obj.y = data[i].CountData;
            } else {
                obj.name = data[i].DetailCase;
                obj.y = data[i].CountData;
            }



            colors.push(data[i].Color);
            count = count + 1;
            var jsonString = JSON.stringify(obj);

            resData.push(obj);

        }

        if (data.length == 0) {
            $('#' + divId).hide();
            $('#StopAccrueFinishtatus').show();
        } else {
            $('#' + divId).show();
            $('#StopAccrueFinishtatus').hide();
            //$('#HoldAccrueFinishtatus').hide();
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
                    //pointFormat: '<b> ({point.y:,.0f})</b>'
                    pointFormat: '<b>{point.y:.1f}%</b>'
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
                            format: '<b>{point.name}</b>: ({point.y:.0f}%) '
                        },
                        point: {
                            events: {
                                click: function (e) {
                                    DepartName = e.point.name;
                                    $('.pnlDetail').hide();
                                    console.log(accrueType + "-" + status + "-" + DepartName + "-" + GroupBy);
                                    if (GroupBy == "Department") {
                                        Table.SearchData(accrueType, status, DepartName, "1");
                                        detailCaseSearch = "1";
                                    } else {
                                        Table.SearchData(accrueType, status, DepartName, "2");
                                        detailCaseSearch = "2";
                                    }
                                    accrueTypeSearch = accrueType;
                                    activitySearch = status;
                                    departmentcodeSearch = DepartName;
                                }
                            }
                        },
                        showInLegend: true,
                        colors: colors
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: '',
                    //  innerSize: isStatus == 1 ? '0%' : '0%',
                    data: resData
                }]

            });
        }



    },

    PieHoldAccrueFinish: function (divId, data, accrueType, status) {
        console.log("charttt: ", divId, data, status)
        var resData = [];
        var colors = [];
        var count = 0;

        for (i = 0; i < data.length; i++) {
            var obj = new Object();

            if (GroupBy == "Department") {
                obj.name = data[i].DepartName;
                obj.y = data[i].CountData;
            } else {
                obj.name = data[i].DetailCase;
                obj.y = data[i].CountData;
            }



            colors.push(data[i].Color);
            count = count + 1;
            var jsonString = JSON.stringify(obj);

            resData.push(obj);

        }

        if (data.length == 0) {
            $('#' + divId).hide();
            $('#HoldAccrueFinishtatus').show();
        } else {
            $('#' + divId).show();
            $('#HoldAccrueFinishtatus').hide();
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
                    //pointFormat: '<b> ({point.y:,.0f})</b>'
                    pointFormat: '<b>{point.y:.1f}%</b>'
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
                            format: '<b>{point.name}</b>: ({point.y:.0f}%) '
                        },
                        point: {
                            events: {
                                click: function (e) {
                                    DepartName = e.point.name;
                                    $('.pnlDetail').hide();
                                    console.log(accrueType + "-" + status + "-" + DepartName + "-" + GroupBy);
                                    if (GroupBy == "Department") {
                                        Table.SearchData(accrueType, status, DepartName, "1");
                                        detailCaseSearch ="1";
                                    } else {
                                        Table.SearchData(accrueType, status, DepartName, "2");
                                        detailCaseSearch = "2";
                                    }
                                    accrueTypeSearch = accrueType;
                                    activitySearch = status;
                                    departmentcodeSearch = DepartName;
                                }
                            }
                        },
                        showInLegend: true,
                        colors: colors
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: '',
                    //  innerSize: isStatus == 1 ? '0%' : '0%',
                    data: resData
                }]

            });
        }



    }

}
