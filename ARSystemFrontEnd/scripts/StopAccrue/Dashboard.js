var requestDetail = [];
var paramHeader;
var HeaderID;
var fsIsReHold = false;
var fsRequestNumber
var fsActivity;
jQuery(document).ready(function () {
    Control.BindAccrueType();
    Control.BindingDatePicker();
    Control.BindAccrueActivity();
    userLoginRole = $("#userLoginRoleLabel").val();
    userLoginID = $("#userLogin").val();
    //$("#pnlHeader").hide();
    //$("#pnlHeader").css('visibility', 'visible');
    //$("#pnlHeader").fadeIn(2000);
    Table.Init("#tblDashboardHeader");
    Table.Init("#tblDashboardDetail");
    Table.LoadHeader();

    $("#btResetRequest").unbind().click(function () {
        $("#StartCreatedDate").val("");
        $("#EndCreatedDate").val("");
        $("#sRequestNumber").val("");
        $("#sDirectorat").val("");
        $("#slsRequestTypeID").val("").trigger("change");
        $("#slsActivityID").val("").trigger("change");
    });

    $("#btSearchRequest").unbind().click(function () {
        Table.LoadHeader();
    });

    $(".btnSearchHeader").unbind().click(function () {
        Table.LoadHeader();
    });
});

var Control = {
    BindAccrueType: function () {
        var selectId2 = "#slsRequestTypeID";
        $.ajax({
            url: "/api/StopAccrue/accrueType",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                $(selectId2).html("<option></option>");
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(selectId2).append("<option value='" + $.trim(item.ID) + "'>" + item.AccrueType + " Accrue </option>");
                    })
                }
                $(selectId2).select2({ placeholder: "Select Type Submission", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingDatePicker: function () {
        var d = new Date();
        var dateNow = new Date(d.getDate(), d.getMonth(), d.getFullYear());
        $(".datepicker").datepicker({
            format: "dd M yyyy",
            autoclose: true
        });
    },
    BindAccrueActivity: function () {
        var selectId = "#slsActivityID";
        $(selectId).html("<option></option>");
        $(selectId).append("<option value='finish' selected>Finish</option>");
        $(selectId).append("<option value='ongoing'>On going</option>");
        $(selectId).select2({ placeholder: "Select Activity", width: null });

        //$.ajax({
        //    url: "/api/StopAccrue/accrueActivity",
        //    type: "GET",
        //    async: false,

        //})
        //    .done(function (data, textStatus, jqXHR) {
        //        $(selectId).html("<option></option>");
        //        if (Common.CheckError.List(data)) {
        //            $.each(data, function (i, item) {
        //                $(selectId).append("<option value='" + $.trim(item.ActivityID) + "'>" + item.Activity + "</option>");
        //            })
        //        }
        //        $(selectId).select2({ placeholder: "Select Activity", width: null });
        //    })
        //    .fail(function (jqXHR, textStatus, errorThrown) {
        //        Common.Alert.Error(errorThrown);
        //    });
    },

}
var Table = {
    Init: function (id) {
        var tbl = $(id).dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $(id).DataTable().columns.adjust().draw();
        });
    },

    LoadHeader: function () {
        var id = "#tblDashboardHeader";
        Table.Init(id);
        Table.Init("#tblDashboardDetail");
        var l = Ladda.create(document.querySelector("#btSearchRequest"));
        l.start();
        paramHeader = {
            RequestTypeID: $("#slsRequestTypeID").val(),
            RequestNumber: $("#sRequestNumber").val(),
            CreatedDate: $("#StartCreatedDate").val(),
            CreatedDate2: $("#EndCreatedDate").val(),
            DepartName: $("#sDirectorat").val(),
            Activity: $("#slsActivityID").val()
        };

        $(id + " tbody").hide();
        $(id).DataTable({
            "deferRender": false,
            "proccessing": false,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/StopAccrue/dashboardHeader",
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
                            return "<b>" + data + "</b>";
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
                        return "<a class='linkDetail' title='Detail Request'>" + data + "</i>";
                    }
                },
                { data: "DepartName" },
                { data: "Activity" },
                {
                    data: "SoNumberCount", render: function (a, b, c) {
                        if (c.RowIndex == "Total")
                            return "<b>" + c.SoNumberCount + "</b>";
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
                                return "<b>" + Common.Format.CommaSeparation(c.SumRevenue) + "</b>";
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
                                return "<b>" + Common.Format.CommaSeparation(c.SumCapex) + "</b>";
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
            "columnDefs": [{ "targets": [10, 11, 12, 13, 14, 15], "visible": false }, { "targets": [2,3, 4, 5, 8, 9], "class": "text-center" }, { "targets": [7, 6], "class": "text-right" }],
            "fnDrawCallback": function () {
                l.stop();
                $(id + " tbody").fadeIn(1500);

            },

        });
        $(id + " tbody").unbind().on("click", ".linkDetail", function (e) {
            var table = $(id).DataTable();
            var row = table.row($(this).parents('tr')).data();
            //$("#pnlHeader").hide();
            //$("#pnlDetail").fadeIn(2000);
            //$("#pnlDetail").css('visibility', 'visible');
            fsIsReHold = row.IsReHold;
            fsRequestNumber = row.RequestNumber;
            HeaderID = row.ID;
            fsActivity = row.Activity;
            Table.LoadDetail();
        });

        $(id + " tbody").on("click", ".downloadDoc", function (e) {
            var table = $(id).DataTable();
            var row = table.row($(this).parents('tr')).data();

            window.location.href = "/StopAccrue/downloadDocSubmit?fileName=" + row.FileName + "&activity=" + row.Activity + "&appHeaderID=" + row.AppHeaderID + "&accrueType=" + row.AccrueType;

        });
    },

    LoadDetail: function () {
        requestDetail = [];
        $(".timeline").html('');
        var params = {
            HeaderID: HeaderID,
        };


        $.ajax({
            url: "/api/StopAccrue/dashboardDetail",
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
    },

    DetailTable: function () {
        var id = "#tblDashboardDetail";
        Table.Init(id);
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
                    data: "StartEffectiveDate", render: function (a,b,c) {
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
            "columnDefs": [{ "targets": [20], "visible": false }, { "targets": [9, 10, 13], "class": "text-right" }, { "targets": [1,2,3,4,5,6,7,8,11,12,14,16,17,18], "class": "text-center" }],
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
                var colNumber = [9, 10, 13];


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

    ExportHeader: function () {
        window.location.href = "/StopAccrue/exportDashboardHeader?" + $.param(paramHeader);
    },
    ExportDetail: function () {
        window.location.href = "/StopAccrue/exportDashboardDetail?HeaderID=" + HeaderID + "&RequestNumber=" + fsRequestNumber + "&IsReHold=" + fsIsReHold;
    }
}