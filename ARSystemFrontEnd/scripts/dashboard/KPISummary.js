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

    $("#btBackSoNumber").unbind().click(function (e) {
        $(".panelSearchResult").fadeIn();
        $("#rowGraph").fadeIn();
        $("#pnlDetailSoNumber").hide();

        if ($("#slMonth").val() != null && $("#slMonth").val() != "0") {
            $(".weekly").fadeIn();
            $(".monthly").hide();
        } else {
            $(".weekly").hide();
            $(".monthly").fadeIn();
        }
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
        var selector = "#tblMonthlyData";
        if ($("#slMonth").val() == null || $("#slMonth").val() == "0") {
            columns = [
                { data: "No" },
                { data: "KPIDesc" },
                { data: "TargetKPI" },
                {
                    data: "LeadTimeJan", render: function (val, type, full) {
                        return Helper.RenderLink(full, 1, 0, val);
                    }
                },
                {
                    data: "LeadTimeFeb", render: function (val, type, full) {
                        return Helper.RenderLink(full, 2, 0, val);
                    }
                },
                {
                    data: "LeadTimeMar", render: function (val, type, full) {
                        return Helper.RenderLink(full, 3, 0, val);
                    }
                },
                {
                    data: "LeadTimeApr", render: function (val, type, full) {
                        return Helper.RenderLink(full, 4, 0, val);
                    }
                },
                {
                    data: "LeadTimeMay", render: function (val, type, full) {
                        return Helper.RenderLink(full, 5, 0, val);
                    }
                },
                {
                    data: "LeadTimeJun", render: function (val, type, full) {
                        return Helper.RenderLink(full, 6, 0, val);
                    }
                },
                {
                    data: "LeadTimeJul", render: function (val, type, full) {
                        return Helper.RenderLink(full, 7, 0, val);
                    }
                },
                {
                    data: "LeadTimeAug", render: function (val, type, full) {
                        return Helper.RenderLink(full, 8, 0, val);
                    }
                },
                {
                    data: "LeadTimeSep", render: function (val, type, full) {
                        return Helper.RenderLink(full, 9, 0, val);
                    }
                },
                {
                    data: "LeadTimeOct", render: function (val, type, full) {
                        return Helper.RenderLink(full, 10, 0, val);
                    }
                },
                {
                    data: "LeadTimeNov", render: function (val, type, full) {
                        return Helper.RenderLink(full, 11, 0, val);
                    }
                },
                {
                    data: "LeadTimeDec", render: function (val, type, full) {
                        return Helper.RenderLink(full, 12, 0, val);
                    }
                },
                { data: "BobotKPI" },
            ]
            selector = "#tblMonthlyData";
            Table.Draw(selector, columns);
            $(".monthly").fadeIn();
            $(".weekly").hide();
        } else {
            columns = [
                { data: "No" },
                { data: "KPIDesc" },
                { data: "TargetKPI" },
                {
                    data: "LeadTimeWeek1", render: function (val, type, full) {
                        return Helper.RenderLink(full, $("#slMonth").val(), 1, val);
                    }
                },
                {
                    data: "LeadTimeWeek2", render: function (val, type, full) {
                        return Helper.RenderLink(full, $("#slMonth").val(), 2, val);
                    }
                },
                {
                    data: "LeadTimeWeek3", render: function (val, type, full) {
                        return Helper.RenderLink(full, $("#slMonth").val(), 3, val);
                    }
                },
                {
                    data: "LeadTimeWeek4", render: function (val, type, full) {
                        return Helper.RenderLink(full, $("#slMonth").val(), 4, val);
                    }
                },
                {
                    data: "LeadTimeWeek5", render: function (val, type, full) {
                        return Helper.RenderLink(full, $("#slMonth").val(), 5, val);
                    }
                },
                {
                    data: "LeadTimeWeek6", render: function (val, type, full) {
                        return Helper.RenderLink(full, $("#slMonth").val(), 6, val);
                    }
                },
                { data: "BobotKPI" },
            ];
            selector = "#tblWeeklyData";
            Table.Draw(selector, columns);
            $(".weekly").fadeIn();
            $(".monthly").hide();
        }

        $("#pnlDetailSoNumber").hide();
    },
    Reset: function () {
        $("#slYear").val($('#slYear option:first-child').val()).trigger("change");
        $("#slMonth").val("0").trigger("change");
    },
    Export: function () {
        var year = $("#slYear").val();
        var month = $("#slMonth").val();

        window.location.href = "/Dashboard/KPISummary/Export?month=" + month + "&year=" + year;
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
                "url": "/api/Dashboard/KPISummary",
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

        $(selector + " tbody").unbind().on("click", "a.btDetail", function (e) {
            var table = $(selector).DataTable();
            var params = {
                Year: $(this).attr('year'),
                Month: $(this).attr('month'),
                Week: $(this).attr('week')
            };

            TableDetail.Search(params);
            $(".panelSearchResult").hide();
            $("#pnlDetailSoNumber").fadeIn();
            $("#rowGraph").hide();
            e.preventDefault();
        });
    }
}

var TableDetail = {
    Search: function (params) {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        $.ajax({
            url: "/api/Dashboard/Detail/SoNumber",
            type: "POST",
            dataType: "JSON",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            var tblSummaryData = $("#tblDetailSoNumber").DataTable({
                "proccessing": true,
                "serverSide": false,
                "language": {
                    "emptyTable": "No data available in table",
                },
                data: data.data,
                buttons: [
                    { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                    {
                        text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                            var l = Ladda.create(document.querySelector(".yellow"));
                            l.start();
                            TableDetail.Export(params)
                            l.stop();
                        }
                    },
                    { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
                ],
                "filter": false,
                "lengthMenu": [[5, 10, 25], ['5', '10', '25']],
                "destroy": true,
                "columns": [
                    { data: "SoNumber" },
                    { data: "Company" },
                    { data: "Operator" },
                    { data: "TenantType" },
                    { data: "Start" },
                    { data: "End" },
                    { data: "EGF" },
                    {
                        data: "RevPerMonth", render: function (val, type, full) {
                            return Helper.GetShortNumberFormat(val);
                        }
                    },
                    {
                        data: "AmountInv", render: function (val, type, full) {
                            return Helper.GetShortNumberFormat(val);
                        }
                    }
                ],
                "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                "fnDrawCallback": function () {
                    //if (Common.CheckError.List(tblSummaryData.data())) {
                    //    $(".panelSearchBegin").hide();
                    //    $(".panelSearchResult").hide();
                    //    $("#pnlDetailSoNumber").fadeIn();
                    //}
                    l.stop();
                },
                'aoColumnDefs': [{
                    'bSortable': false,
                    'aTargets': []
                }],
                'order': []
            });
        });
    },
    Export: function (params) {
        var year = params.Year;
        var month = params.Month;
        var week = params.Week;

        window.location.href = "/Dashboard/DetailSoNumber/Export?year=" + year + "&month=" + month + "&week=" + week;
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
    FormatPercentage: function (val) {
        var percentage = Common.Format.CommaSeparation(val + "");
        return percentage + " %";
    },
    FormatDuration: function (val) {

        if (isNaN(parseInt(val)))
            val = "0.0";

        var duration = parseFloat(val + "").toFixed(1);
        return duration + " days";
    },
    RenderLink: function (obj, month, week, text) {
        var year = $("#slYear").val();

        if (year == null) {
            $("#slYear").val($('#slYear option:first-child').val()).trigger('change');
            year = $("#slYear").val();
        }

        if (obj.KPIDesc == "Lead Time Invoice")
            return "<a class='btDetail' year='" + year + "' month='" + month + "' week='" + week + "'>" + Helper.FormatDuration(text) + "</a>";
        else
            return "";
    },
    GetShortNumberFormat(val) {
        var divider = 1000000.00;
        val = parseFloat(val.toString().replace(/,/g, '')) / divider;
        return Common.Format.CommaSeparation(val);
    }
}