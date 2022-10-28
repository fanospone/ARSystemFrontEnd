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

    $("#btBackBuilding").unbind().click(function (e) {
        $(".panelSearchResult").fadeIn();
        $("#pnlDetailBuilding").hide();

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
        var tableSelector = "#tblMonthlyData";
        if ($("#slMonth").val() == null || $("#slMonth").val() == "0") {
            columns = [
                { data: "Category" },
                {
                    data: "JanCurr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 1, 0, "Curr");
                    }
                },
                {
                    data: "JanOD", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 1, 0, "OD");
                    }
                },
                {
                    data: "FebCurr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 2, 0, "Curr");
                    }
                },
                {
                    data: "FebOD", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 2, 0, "OD");
                    }
                },
                {
                    data: "MarCurr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 3, 0, "Curr");
                    }
                },
                {
                    data: "MarOD", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 3, 0, "OD");
                    }
                },
                {
                    data: "AprCurr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 4, 0, "Curr");
                    }
                },
                {
                    data: "AprOD", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 4, 0, "OD");
                    }
                },
                {
                    data: "MayCurr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 5, 0, "Curr");
                    }
                },
                {
                    data: "MayOD", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 5, 0, "OD");
                    }
                },
                {
                    data: "JunCurr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 6, 0, "Curr");
                    }
                },
                {
                    data: "JunOD", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 6, 0, "OD");
                    }
                },
                {
                    data: "JulCurr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 7, 0, "Curr");
                    }
                },
                {
                    data: "JulOD", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 7, 0, "OD");
                    }
                },
                {
                    data: "AugCurr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 8, 0, "Curr");
                    }
                },
                {
                    data: "AugOD", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 8, 0, "OD");
                    }
                },
                {
                    data: "SepCurr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 9, 0, "Curr");
                    }
                },
                {
                    data: "SepOD", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 9, 0, "OD");
                    }
                },
                {
                    data: "OctCurr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 10, 0, "Curr");
                    }
                },
                {
                    data: "OctOD", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 10, 0, "OD");
                    }
                },
                {
                    data: "NovCurr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 11, 0, "Curr");
                    }
                },
                {
                    data: "NovOD", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 11, 0, "OD");
                    }
                },
                {
                    data: "DecCurr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 12, 0, "Curr");
                    }
                },
                {
                    data: "DecOD", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, 12, 0, "OD");
                    }
                },
            ]
            selector = "#tblMonthlyData";
            $(".monthly").fadeIn();
            $(".weekly").hide();
        } else {
            $("#thMonthName").html(Helper.GetMonthName($("#slMonth").val()));
            var month = $("#slMonth").val();
            columns = [
                { data: "Category" },
                {
                    data: "Week1Curr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, month, 1, "Curr");
                    }
                },
                {
                    data: "Week1OD", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, month, 1, "OD");
                    }
                },
                {
                    data: "Week2Curr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, month, 2, "Curr");
                    }
                },
                {
                    data: "Week2OD", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, month, 2, "OD");
                    }
                },
                {
                    data: "Week3Curr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, month, 3, "Curr");
                    }
                },
                {
                    data: "Week3OD", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, month, 3, "OD");
                    }
                },
                {
                    data: "Week4Curr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, month, 4, "Curr");
                    }
                },
                {
                    data: "Week4OD", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, month, 4, "OD");
                    }
                },
                {
                    data: "Week5Curr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, month, 5, "Curr");
                    }
                },
                {
                    data: "Week5OD", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, month, 5, "OD");
                    }
                },
                {
                    data: "Week6Curr", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, month, 6, "Curr");
                    }
                },
                {
                    data: "Week6OD", className: "text-right", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, month, 6, "OD");
                    }
                },
            ];
            selector = "#tblWeeklyData";
            $(".weekly").fadeIn();
            $(".monthly").hide();
        }
        Table.Draw(selector, columns);
        $("#pnlDetailBuilding").hide();

        $(selector + " tbody").unbind().on("click", "a.btDetail", function (e) {
            var table = $(selector).DataTable();
            var params = {
                Year: $(this).attr('year'),
                Month: $(this).attr('month'),
                Week: $(this).attr('week'),
                LeadTime: $(this).attr('leadTime')
            };

            TableDetail.Search(params);
            $(".panelSearchResult").hide();
            $("#pnlDetailSoNumber").fadeIn();
            e.preventDefault();
        });
    },
    Reset: function () {
        $("#slYear").val("").trigger("change");
        $("#slMonth").val("0").trigger("change");
    },
    Export: function () {
        var year = $("#slYear").val();
        var month = $("#slMonth").val();

        window.location.href = "/Dashboard/InvoiceBuildingSummary/Export?month=" + month + "&year=" + year;
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
                "url": "/api/Dashboard/InvoiceBuildingSummary",
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
                Helper.MergeColumns(selector);
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            'ordering': false,
            'order': []
        });
    }
}

var TableDetail = {
    Search: function (params) {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        
        $.ajax({
            url: "/api/Dashboard/Detail/Building",
            type: "POST",
            datatype: "json",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            var tblSummaryData = $("#tblDetailBuilding").DataTable({
                "proccessing": true,
                "serverSide": false,
                "language": {
                    "emptyTable": "No data available in table",
                },
                "data": data.data,
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
                    { data: "InvNo" },
                    { data: "Company" },
                    { data: "CompanyType" },
                    { data: "TermPeriod" },
                    {
                        data: "Area", className: "text-right", render: function (val, type, full) {
                            return Common.Format.CommaSeparation(val);
                        }
                    },
                    {
                        data: "PricePerMeter", className: "text-right", render: function (val, type, full) {
                            return Helper.GetShortNumberFormat(val);
                        }
                    },
                    {
                        data: "PricePerMonth", className: "text-right", render: function (val, type, full) {
                            return Helper.GetShortNumberFormat(val);
                        }
                    },
                    {
                        data: "TotalPrice", className: "text-right", render: function (val, type, full) {
                            return Helper.GetShortNumberFormat(val);
                        }
                    },
                    {
                        data: "Discount", className: "text-right", render: function (val, type, full) {
                            return Helper.GetShortNumberFormat(val);
                        }
                    },
                    {
                        data: "PPN", className: "text-right", render: function (val, type, full) {
                            return Helper.GetShortNumberFormat(val);
                        }
                    },
                    {
                        data: "Penalty", className: "text-right", render: function (val, type, full) {
                            return Helper.GetShortNumberFormat(val);
                        }
                    },
                    {
                        data: "TotalAmount", className: "text-right", render: function (val, type, full) {
                            return Helper.GetShortNumberFormat(val);
                        }
                    }
                ],
                "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                "fnDrawCallback": function () {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").hide();
                    $("#pnlDetailBuilding").fadeIn();
                    l.stop();
                },
                'aoColumnDefs': [{
                    'bSortable': false,
                    'aTargets': []
                }],
                'order': []
            });
        }).fail(function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            console.log(err.Message);
        });;
    },
    Export: function (params) {
        var year = params.Year;
        var month = params.Month;
        var week = params.Week;
        var leadTime = params.LeadTime;

        window.location.href = "/Dashboard/DetailBuilding/Export?month=" + month + "&year=" + year + "&week=" + week + "&leadTime=" + leadTime;
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
        var duration = parseFloat(val + "").toFixed(1);
        return duration + " days";
    },
    RenderLink: function (val, full, month, week, leadTime) {
        var year = $("#slYear").val();

        if (year == null) {
            $("#slYear").val($('#slYear option:first-child').val()).trigger('change');
            year = $("#slYear").val();
        }

        var format = "";
        var divider = 1000000.00;
        if (full.Category != "Number Of Inv.") {
            val = parseFloat(val.replace(/,/g, '')) / divider;
            format = Common.Format.CommaSeparation(val);
        }

        if (full.Category == "Grand Total")
            return "<a class='btDetail' year='" + year + "' month='" + month + "' week='" + week + "'>" + format + "</a>";

        if (full.Category == "Number Of Inv.")
            format = val;

        return "<a class='btDetail' year='" + year + "' month='" + month + "' week='" + week + "' leadTime='" + leadTime + "'>" + format + "</a>";
    },
    GetMonthName: function (val) {
        var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
        return months[parseInt(val) - 1];
    },
    MergeColumns: function (tableSelector) {
        var rowNumber = [3]; // Index of the datatable row body, starting from 1
        var colStart = 2; // 
        var colEnd = 25;

        $.each(rowNumber, function (index, item) {
            for (var i = colStart ; i <= colEnd ; i++) {
                if (i % 2 == 0)
                    $(tableSelector + " tr:nth-child(" + item + ") td:nth-child(" + i + ")").attr('colspan', '2').addClass('text-center');
                else
                    $(tableSelector + " tr:nth-child(" + item + ") td:nth-child(" + i + ")").attr('style', 'display:none;');
            }
        });
    },
    GetShortNumberFormat(val) {
        var divider = 1000000.00;
        val = parseFloat(val.toString().replace(/,/g, '')) / divider;
        return Common.Format.CommaSeparation(val);
    }
}