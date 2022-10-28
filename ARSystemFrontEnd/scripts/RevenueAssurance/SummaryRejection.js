
jQuery(document).ready(function () {
    Control.Init();
    Table.Init();
    Control.SetParams();
    Table.Search();
});

var Control = {
    Init: function () {
        Control.BindingCompany();
        Control.BindingOperator();
        Control.BindingBeginPeriod('tbStartRTIPeriod');
        Control.BindingPeriod('tbEndRTIPeriod');

        $("#tbStartRTIPeriod").on("change", function () {
            Control.SetParams();
            Table.Search();
        });

        $("#tbEndRTIPeriod").on("change", function () {
            Control.SetParams();
            Table.Search();
        });

        $("#btSearch").unbind().click(function () {
            Control.SetParams();
            Table.Search();
        });

        $("#btBack").unbind().click(function (e) {
            $(".summary").fadeIn();
            $(".detail").hide();
            $(".pnlBySonumb").hide();
            $(".pnlByBaps").hide();
            $(".pnlByInvoice").hide();
        });

        $("#btReset").unbind().click(function (e) {
            $("#slCompany").val(null).trigger("change");
            $("#slOperator").val(null).trigger("change");
            $("#tbStartRTIPeriod").val(null);
            $("#tbEndRTIPeriod").val(null);
        });
    },

    BindingCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $("#slCompany").html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slCompany").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
            }
            $("#slCompany").select2({ placeholder: "Select Company", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingOperator: function () {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $("#slOperator").html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slOperator").append("<option value='" + item.OperatorId + "'>" + item.Operator + "</option>");
                })
            }
            $("#slOperator").select2({ placeholder: "Select Customer", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingPeriod: function (e) {
        var date = new Date();
        var today = new Date(date.getFullYear(), date.getMonth(), date.getDate());
        $("#" + e).datepicker({
            endDate: '+0d',
            format: "dd-M-yyyy",
            orientation: "bottom"
        });
        $('#' + e).datepicker("setDate", today);
    },

    BindingBeginPeriod: function (element) {
        var date = new Date();
        var begin = new Date(date.getFullYear(), "01" - 1, "01");
        $('#' + element).datepicker({
            format: "dd-M-yyyy",
            orientation: "bottom"
        });
        $('#' + element).datepicker("setDate", begin);
    },

    SetParams: function () {
        params = {
            vCompanyID: $("#slCompany").val(),
            vOperatorID: $("#slOperator").val(),
            vRTIPeriodStart: $("#tbStartRTIPeriod").val(),
            vRTIPeriodEnd: $("#tbEndRTIPeriod").val(),
            vGroupBy: $('input[name=rbView]:checked').val()
        }
    },

    HeaderNameSummary: function () {
        var vGroup = $('input[name=rbView]:checked').val();
        if ( vGroup == 1) {
            $('#countFinCol').text("# Of Sonumb Reject");
            $('#amountFinCol').text("Amount Of Sonumb Reject");
            $('#repFinCol').text("# Of Sonumb Repetitive Reject");
            $('#countOprCol').text("# Of Sonumb Reject");
            $('#amountOprCol').text("Amount Of Sonumb Reject");
            $('#repOprCol').text("# Of Sonumb Repetitive Reject");
        } else if (vGroup == 2) {
            $('#countFinCol').text("# Of Baps Reject");
            $('#amountFinCol').text("Amount Of Baps Reject");
            $('#repFinCol').text("# Of Baps Repetitive Reject");
            $('#countOprCol').text("# Of Baps Reject");
            $('#amountOprCol').text("Amount Of Baps Reject");
            $('#repOprCol').text("# Of Baps Repetitive Reject");
        } else {
            $('#countFinCol').text("# Of Invoice Reject");
            $('#amountFinCol').text("Amount Of Invoice Reject");
            $('#repFinCol').text("# Of Invoice Repetitive Reject");
            $('#countOprCol').text("# Of Invoice Reject");
            $('#amountOprCol').text("Amount Of Invoice Reject");
            $('#repOprCol').text("# Of Invoice Repetitive Reject");
        }
          
    }
}

var Table = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();

        var tblSummary = $('#tblSummary').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblSummary").DataTable().columns.adjust().draw();
        });

        $("#tblSummary tbody").unbind().on("click", "a.btDetail", function () {

            var params = {
                vCompanyID: $("#slCompany").val(),
                vOperatorID: $("#slOperator").val(),
                vRTIPeriodStart: $("#tbStartRTIPeriod").val(),
                vRTIPeriodEnd: $("#tbEndRTIPeriod").val(),
                vGroupBy: $('input[name=rbView]:checked').val(),
                vType: $(this).attr("Type"),
                vCol: $(this).attr("Col"),
                vDepartmentCode: $(this).attr("DepartmentCode")
            };

            Table.Detail(params);
            $(".summary").hide();
            $(".detail").fadeIn();           
        });

        $("#tblSummary tfoot").unbind().on("click", "a.btDetail", function () {

            var params = {
                vCompanyID: $("#slCompany").val(),
                vOperatorID: $("#slOperator").val(),
                vRTIPeriodStart: $("#tbStartRTIPeriod").val(),
                vRTIPeriodEnd: $("#tbEndRTIPeriod").val(),
                vGroupBy: $('input[name=rbView]:checked').val(),
                vType: $(this).attr("Type"),
                vCol: $(this).attr("Col"),
                vDepartmentCode: $(this).attr("DepartmentCode")
            };

            Table.Detail(params);
            $(".summary").hide();
            $(".detail").fadeIn();
            //e.preventDefault();
        });
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        $(".summary").fadeIn();
        $(".detail").hide();
        Control.HeaderNameSummary();

        var tblSummary = $('#tblSummary').DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/SummaryRejection/summary",
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
                        Table.Export(params);
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[-1], ['All']],
            "destroy": true,
            "columns": [
                {
                    data: "DepartmentName"
                },
                {
                    data: "CountRejectFin", className: "text-center", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, "1");
                    }
                },
                {
                    data: "AmountFin", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '')
                },
                {
                    data: "RepatitiveFin", className: "text-center", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, "3");
                    }
                },
                {
                    data: "CountRejectOpr", className: "text-center", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, "4");
                    }
                },
                {
                    data: "AmountOpr", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '')
                },
                {
                    data: "RepatitiveOpr", className: "text-center", render: function (val, type, full) {
                        return Helper.RenderLink(val, full, "6");
                    }
                }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: "#tblSummary", animate: true });
            },
            "fixedColumns": {
                leftColumns: 1
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummary.data())) {
                    $(".panelSearchBegin").hide();
                }
                l.stop();
                App.unblockUI('#tblSummary');
            },
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;
                var colNumber = [1, 2, 3, 4, 5, 6];

                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                        i : 0;
                };

                var numformat = $.fn.dataTable.render.number('.', ',', 0, '').display;

                for (i = 0; i < colNumber.length; i++) {
                    var colNo = colNumber[i];
                    var total2 = api
                            .column(colNo, { page: 'current' })
                            .data()
                            .reduce(function (a, b) {
                                return intVal(a) + intVal(b);
                            }, 0);
                    $(api.column(colNo).footer()).html(numformat(total2));

                    if (colNo == 1 || colNo == 3 || colNo == 4 || colNo == 6)
                        $(api.column(colNo).footer()).html("<a class='btDetail' DepartmentCode ='TOTAL' Col='" + colNo + "'>" + numformat(total2) + "</a>");
                    else
                        $(api.column(colNo).footer()).html(numformat(total2));
                }
            },
            'ordering': false,
            'order': []
        });
    },

    Export: function (params) {
        window.location.href = "/RevenueAssurance/SummaryRejection/Summary/Export?" + $.param(params);
    },

    Detail: function (params) {
        $.ajax({
            url: "/api/SummaryRejection/detail",
            type: "POST",
            datatype: "json",
            data: params,
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            //var vGroup = $('input[name=rbView]:checked').val();
            //$(".pnlBySonumb").hide();
            //$(".pnlByBaps").hide();
            //$(".pnlByInvoice").hide();
            //if (vGroup == 1) {
            //    $(".pnlBySonumb").show();
            //    Table.DetailBySonumb(data.data, params);
            //} else if (vGroup == 2) {
            //    $(".pnlByBaps").show();
            //    Table.DetailByBaps(data.data, params);
            //} else {
            //    $(".pnlByInvoice").show();
            //    Table.DetailByInvoice(data.data, params);
            //}
            Table.DetailBySonumb(data.data, params);

        })
        .fail(function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
        });
    },

    DetailBySonumb: function (data, params) {
        var tblDetailBySonumb = $("#tblDetailBySonumb").DataTable({
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table",
            },
            "data": data,
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.DetailExport(params);
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50, -1], ['10', '25', '50', 'All']],
            "destroy": true,
            "columns": [
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CustomerInvoice" },
                { data: "CompanyInvoice" },
                { data: "RegionName" },
                { data: "BapsNo" },
                { data: "BapsType" },
                { data: "InvoiceNumber" },
                {
                    data: "StartDateInvoice", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndDateInvoice", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "AmountInvoice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '')
                },
                { data: "Currency" },
                { data: "DepartmentName" },
                { data: "RejectYear" },
                { data: "RejectMonth" },
                {
                    data: "RejectDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "RTIDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "BapsConfirmDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "PicaType" },
                { data: "PicaMajor" },
                { data: "PicaDetail" },
                { data: "Remarks" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: "#tblDetailBySonumb", animate: true });
            },
            "fnDrawCallback": function () {
                App.unblockUI('#tblDetailBySonumb');
            },
            "columnDefs": [
                { "targets": [0, 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 16, 17, 18, 19, 20, 21, 22], "className": "dt-center" },
                { "targets": [13], "className": "dt-right" },
                { "targets": [2, 15, 23, 24], "className": "dt-left" }
            ],
            'ordering': false,
            'order': []
        });

        var vGroup = $('input[name=rbView]:checked').val();
        if (vGroup == 2) {
            tblDetailBySonumb.column(0).visible(false);
            tblDetailBySonumb.column(1).visible(false);
            tblDetailBySonumb.column(2).visible(false);
            tblDetailBySonumb.column(3).visible(false);
            tblDetailBySonumb.column(4).visible(false);
            tblDetailBySonumb.column(5).visible(false);
            tblDetailBySonumb.column(6).visible(false);
        } else if (vGroup == 3) {
            tblDetailBySonumb.column(0).visible(false);
            tblDetailBySonumb.column(1).visible(false);
            tblDetailBySonumb.column(2).visible(false);
            tblDetailBySonumb.column(3).visible(false);
            tblDetailBySonumb.column(4).visible(false);
            tblDetailBySonumb.column(5).visible(false);
            tblDetailBySonumb.column(6).visible(false);
            tblDetailBySonumb.column(8).visible(false);
            tblDetailBySonumb.column(9).visible(false);
        }
        else {
            tblDetailBySonumb.column(0).visible(true);
            tblDetailBySonumb.column(1).visible(true);
            tblDetailBySonumb.column(2).visible(true);
            tblDetailBySonumb.column(3).visible(true);
            tblDetailBySonumb.column(4).visible(true);
            tblDetailBySonumb.column(5).visible(true);
            tblDetailBySonumb.column(6).visible(true);
            tblDetailBySonumb.column(8).visible(false);
            tblDetailBySonumb.column(9).visible(false);
        }
    },

    DetailExport: function (params) {
        window.location.href = "/RevenueAssurance/SummaryRejection/Detail/Export?" + $.param(params);
    }
}

var Helper = {
    RenderLink: function (val, full, col) {
        return "<a class='btDetail' DepartmentCode ='" + full.DepartmentCode + "' Col='" + col + "'>" + val + "</a>";
    }
}