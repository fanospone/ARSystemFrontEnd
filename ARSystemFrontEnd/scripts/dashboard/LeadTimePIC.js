Data = {};

jQuery(document).ready(function () {
    Control.LoadYear();
    Control.LoadMonth();
    Table.Init();
    Table.Search();

    $("#pnlDetailLeadTime").hide();

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

    $("#btBackLeadTime").unbind().click(function (e) {
        $(".panelSearchResult").fadeIn();
        $("#pnlDetailLeadTime").hide();
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
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var params = {
            Year: $("#slYear").val(),
            Month: $("#slMonth").val()
        };

        if ($("#slMonth").val() == 0 || $("#slMonth").val() == null) {
            records = 12;
            $("#thMonthName").html("Month");
        } else {
            $("#thMonthName").html("Week");
        }

        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/Dashboard/LeadTimePIC",
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
            "columns": [
                { data: "Param3" },
                {
                    data: "VerificatorCurr",
                    render: function (val, type, full) {
                        return Helper.RenderLink(full, 'Verificator', 'Curr', val);
                    }
                },
                {
                    data: "VerificatorOD",
                    render: function (val, type, full) {
                        return Helper.RenderLink(full, 'Verificator', 'OD', val);
                    }
                },
                {
                    data: "VerificatorAVG",
                    render: function (val, type, full) {
                        return Helper.FormatDuration(val);
                    }
                },
                {
                    data: "InputerCurr",
                    render: function (val, type, full) {
                        return Helper.RenderLink(full, 'Inputer', 'Curr', val);
                    }
                },
                {
                    data: "InputerOD",
                    render: function (val, type, full) {
                        return Helper.RenderLink(full, 'Inputer', 'OD', val);
                    }
                },
                {
                    data: "InputerAVG",
                    render: function (val, type, full) {
                        return Helper.FormatDuration(val);
                    }
                },
                {
                    data: "FinishingCurr",
                    render: function (val, type, full) {
                        return Helper.RenderLink(full, 'Finishing', 'Curr', val);
                    }
                },
                {
                    data: "FinishingOD",
                    render: function (val, type, full) {
                        return Helper.RenderLink(full, 'Finishing', 'OD', val);
                    }
                },
                {
                    data: "FinishingAVG",
                    render: function (val, type, full) {
                        return Helper.FormatDuration(val);
                    }
                },
                {
                    data: "ARDataCurr",
                    render: function (val, type, full) {
                        return Helper.RenderLink(full, 'ARData', 'Curr', val);
                    }
                },
                {
                    data: "ARDataOD",
                    render: function (val, type, full) {
                        return Helper.RenderLink(full, 'ARData', 'OD', val);
                    }
                },
                {
                    data: "ARDataAVG",
                    render: function (val, type, full) {
                        return Helper.FormatDuration(val);
                    }
                },
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]
            }],
            'order': []
        });
        $(".panelSearchResult").fadeIn();
        $("#pnlDetailLeadTime").hide();

        // Initialize events (for button clicks, etc.)
        $("#tblSummaryData tbody").unbind().on("click", "a.btDetail", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var params = {
                Year: $(this).attr('year'),
                Month: $(this).attr('month'),
                Week: $(this).attr('week'),
                PIC: $(this).attr('pic'),
                LeadTime: $(this).attr('leadTime')
            };

            TableDetail.Search(params);
            $(".panelSearchResult").hide();
            $("#pnlDetailLeadTime").fadeIn();
            e.preventDefault();
        });
    },
    Reset: function () {
        $("#slYear").val($('#slYear option:first-child').val()).trigger("change");
        $("#slMonth").val("0").trigger("change");
    },
    Export: function () {
        var year = $("#slYear").val();
        var month = $("#slMonth").val();

        window.location.href = "/Dashboard/LeadTimePIC/Export?month=" + month + "&year=" + year;
    }
}

var TableDetail = {
    Search: function (params) {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        $.ajax({
            url: "/api/Dashboard/Detail/LeadTime",
            type: "POST",
            dataType: "JSON",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            var tblSummaryData = $("#tblDetailLeadTime").DataTable({
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
                            TableDetail.Export()
                            l.stop();
                        }
                    },
                    { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
                ],
                "filter": false,
                "lengthMenu": [[5, 10, 25], ['5', '10', '25']],
                "destroy": true,
                "columns": [
                    { data: "Company" },
                    { data: "Operator" },
                    { data: "SoNumber" },
                    { data: "CollectionPeriod" },
                    { data: "InvoiceNumber" },
                    { data: "Activity" },
                    {
                        data: "LogActivity", render: function (val, type, full) {
                            return Helper.FormatLogDate(val);
                        }
                    },
                    { data: "LogUser" },
                ],
                "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                "fnDrawCallback": function () {
                    //if (Common.CheckError.List(tblSummaryData.data())) {
                    //    $(".panelSearchBegin").hide();
                    //    $(".panelSearchResult").hide();
                    //    $("#pnlDetailLeadTime").fadeIn();
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
        var pic = params.PIC;
        var leadTime = params.LeadTime;

        window.location.href = "/Dashboard/DetailLeadTime/Export?year=" + year + "&month=" + month + "&week=" + week + "&pic=" + pic + "&leadTime=" + leadTime;
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
    FormatLogDate: function (val) {
        var timeOnly = val.substring(11);
        return Common.Format.ConvertJSONDateTime(val) + ' ' + timeOnly;
    },
    RenderLinkLeadTimeMonthly: function (data, pic, leadTime, text) {
        return "<a class='btDetail' year='"+ data.Param1 +"' month='"+ data.Param2 +"' week='0' pic='"+ pic +"' leadTime='"+ leadTime +"'>"+ Helper.FormatPercentage(text) +"</a>";
    },
    RenderLinkLeadTimeWeekly: function (data, pic, leadTime, text) {
        return "<a class='btDetail' year='" + $("#slYear").val() + "' month='" + $("#slMonth").val() + "' week='"+ data.Param3.split('-')[1] +"' pic='" + pic + "' leadTime='" + leadTime + "'>" + Helper.FormatPercentage(text) + "</a>";
    },
    RenderLink: function (data, pic, leadTime, text) {
        if ($("#thMonthName").text() == "Week")
            return Helper.RenderLinkLeadTimeWeekly(data, pic, leadTime, text);
        else
            return Helper.RenderLinkLeadTimeMonthly(data, pic, leadTime, text);
    }
}