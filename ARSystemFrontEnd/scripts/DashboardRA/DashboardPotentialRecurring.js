var _StipDate = "";
var _RfiDate = "";
var _SectionID = "";
var _SowID = "";

var _ProductID = "";
var _StipID = "";
var _RegionalID = "";
var _CompanyID = "";
var _paramCol = "";
var _paramRow = "";
var _Type = "GroupBySection";
var _Year = new Date().getFullYear().toString();

var _TypeMonth = "";
var _Params = {}

jQuery(document).ready(function () {
    Form.Init();
    $(document).keypress(function (e) {
        if (e.which == 13) {
            Table.Summary();
        }
    });

    $("#btSearch").unbind().click(function () {
        Table.Summary();
    });


    $("#btBack").unbind().click(function () {
        $('#pnlDetail').fadeOut();
        $('.panelSearchResult').fadeOut();
        $('.panelSearchResultMonth').fadeIn(1000);

    });
    $("#btBackFromMonth").unbind().click(function () {
        $('#pnlDetail').fadeOut();
        $('.panelSearchResultMonth').fadeOut();
        $('.panelSearchResult').fadeIn(1000);

    });


    $("#btReset").unbind().click(function () {
        Form.Reset();
    });

    $("#btSearchDetail").unbind().click(function () {
        $('.panelSearchResultWeek').fadeOut();
        $('.pnlDetail').fadeOut();
        $('.panelSearchResultMonth').fadeOut();
        $('.panelSearchChart').fadeOut();
        $('#pnlDetail').fadeIn(1000);

        Table.Details(_Params.Type, _Params.RFIDateMonth, _Params.RFIDateWeek, _Params.CustomerID, _Params.Step);

    });

    $("#btResetDetail").unbind().click(function () {
        $("#slSearchDetailSonum").val('');
        $("#slSearchDetailSiteID").val('');
        $("#slSearchDetailSiteName").val('');
        Table.Details(_Params.Type, _Params.RFIDateMonth, _Params.RFIDateWeek, _Params.CustomerID, _Params.Step);
    });

    $('input[name=rdStatus]').on('change', function () {
        _Type = $(this).val();
    });
    $('input[name=rdStatusMonth]').on('change', function () {
        _TypeMonth = $(this).val();
        Table.Summary();
    });
});
var Form = {
    Init: function () {
        $('.panelSearchResult').hide();
        $('.panelSearchResultMonth').hide();


        Control.BindingSelectSection($('#slSearchSection'));

        Control.BindingSelectSow($('#slSearchSOW'));
        Control.BindingSelectRegional($('#slSearchRegional'));
        Control.BindingSelectCompany($('#slSearchCompany'));
        Control.BindingSelectSTIP($('#slSearchSTIPCategory'));
        Control.BindingSelectProduct($('#slSearchProduct'), "", "");

        //var tblSummary = $('#tblSummary').DataTable({
        //    "filter": false,
        //    "destroy": true,
        //    "searchable": true,
        //    "data": []
        //});

        //var tblDetail = $('#tblDetail').DataTable({
        //    "filter": false,
        //    "destroy": true,
        //    "searchable": true,
        //    "data": []
        //});

        $("body").delegate(".datepicker", "focusin", function () {
            $(this).datepicker({
                format: "yyyy",
                viewMode: "years",
                minViewMode: "years",
                orientation: "bottom",
            });
        });



    },

    Reset: function () {
        $("#slSearchSection").val(null).trigger("change");
        $("#slSearchSOW").val(null).trigger("change");
        $("#slSearchRegional").val(null).trigger("change");
        $("#slSearchCompany").val(null).trigger("change");
        $("#slSearchSTIPCategory").val(null).trigger("change");
        $("#slSearchProduct").val(null).trigger("change");
        $("#tbSearchSTIPDate").val("");
        $("#tbSearchRFIDate").val("");
    }
}

var Table = {
    Summary: function () {
        App.blockUI({ target: ".portlet-body", animate: !0 });
        //var l = Ladda.create(document.querySelector("#btSearch"))
        //l.start();

        _StipDate = $("#tbSearchSTIPDate").val();
        _RfiDate = $("#tbSearchRFIDate").val();
        _SectionID = $("#slSearchSection").val();
        _SowID = $("#slSearchSOW").val();
        _StipID = $("#slSearchSTIPCategory").val();
        _ProductID = $("#slSearchProduct").val();
        _RegionalID = $("#slSearchRegional").val();
        _CompanyID = $("#slSearchCompany").val();

        var _MainType = _Type + _TypeMonth;

        $.ajax({
            url: "/api/DashboardPotentialRecurring/GetSummary",
            type: "GET",
            data: {
                type: _Type + _TypeMonth,
                STIPDate: _StipDate,
                RFIDate: _RfiDate,
                SectionID: _SectionID,
                SOWID: _SowID,
                ProductID: _ProductID,
                STIPID: _StipID,
                RegionalID: _RegionalID,
                CompanyID: _CompanyID,
                year: null,
                month: null, Customer: null

            }
        })
            .done(function (data, textStatus, jqXHR) {
                $('#pnlDetail').fadeOut();
                $('.panelSearchResultMonth').fadeOut();
                $('.panelSearchResult').fadeIn(1000);
                $("#tblHeadSummary").html("");
                $("#tblHeadSummary").append(data.tblHead);
                //l.stop();
                var columns = [];
                var idx = 0;

                $.each(data.customcolumn, function (i, item) {

                    if (idx > 0)
                        //columns[idx] = { data: item, className: "text-right", render: function (data, type, full) { return "<a class='btDetail " + item + "'>" + data.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "</a>"; } };
                        columns[idx] = { data: item, className: "text-right", render: function (data, type, full) { return "<a class='btSummaryMonth " + item + "'>" + data.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "</a>"; } };
                    //else if (idx == 1)
                    //    columns[idx] = { data: item.data, className: item.className, mRender: function (data, type, full) { return "<a class='btDetail'>" + data + "</a>"; } };
                    else
                        columns[idx] = { data: item, className: "text-left" };

                    idx += 1;
                });

                $('#tblSummary').DataTable({
                    data: JSON.parse(data.dataSummary),
                    buttons: [
                        { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                        {
                            text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                                var l = Ladda.create(document.querySelector(".yellow"));
                                l.start();
                                Table.Export('tblSummary', 'Potential Recurring (Year)', 'Potential Recurring (Year)');
                                l.stop();
                            }
                        },
                        { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
                    ],
                    "lengthMenu": [[-1], ['All']],
                    "columns": columns,
                    searching: false,
                    "footerCallback": function (row, data, start, end, display) {
                        var api = this.api();
                        nb_cols = api.columns().nodes().length;
                        var j = 1;
                        var param = "";
                        while (j < nb_cols) {
                            var pageTotal = api
                                .column(j, { page: 'current' })
                                .data()
                                .reduce(function (a, b) {
                                    return Number(a) + Number(b);
                                }, 0);
                            //set class custom for parameter Details
                            if (j == 1)
                                param = "<=" + (new Date().getFullYear() - 4).toString();
                            else if (j == 2)
                                param = (new Date().getFullYear() - 3).toString();
                            else if (j == 3)
                                param = (new Date().getFullYear() - 2).toString();
                            else if (j == 4)
                                param = (new Date().getFullYear() - 1).toString();
                            else if (j == 5)
                                param = new Date().getFullYear().toString();
                            else if (j == 6)
                                param = (new Date().getFullYear() + 1).toString();
                            else if (j == 7)
                                param = "GrandTotal";

                            // Update footer
                            if (j == 1 || j == 2 || j == 3 || j == 4 || j == 5 || j == 6 || j == 7) {
                                $(api.column(j).footer()).html("<a class=" + "'btSummaryMonth " + param + "'>" + pageTotal.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "</a>");
                                //param++;
                            }

                            j++;
                        }
                    },
                    "order": [],
                    "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

                    "destroy": true
                });

                $("#tblSummary tbody").on("click", "a.btSummaryMonth", function (e) {
                    //$("#tblSummary tbody").on("click", "a.btDetail", function (e) {                
                    e.preventDefault();
                    var col = e.currentTarget.attributes.class.nodeValue.split(" ")[1];
                    var table = $("#tblSummary").DataTable();
                    var data = table.row($(this).parents("tr")).data();
                    _paramRow = data.Description;
                    if (data != null) {
                        if (col != "GrandTotal") {
                            _paramCol = col;
                        } else {
                            _paramCol = "";
                        }
                        Table.SummaryMonth(_paramCol, data.Description);
                    }
                });

                $("#tblSummary tfoot").on("click", ".btSummaryMonth", function (e) {
                    e.preventDefault();
                    var data = e.currentTarget.attributes.class.nodeValue.split(" ")[1];
                    _paramRow = "";
                    if (data != null) {
                        if (data != "GrandTotal") {
                            _paramCol = data;
                        } else {
                            _paramCol = "";
                        }
                        Table.SummaryMonth(_paramCol, "");
                    }
                });
                App.unblockUI(".portlet-body");
                //console.log(JSON.parse(data.dataSummary));
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                App.unblockUI(".portlet-body");
                Common.Alert.Error(errorThrown);
            });
    },
    SummaryMonth: function (pyear, pdesc) {
        App.blockUI({ target: ".portlet-Main", animate: !0 });
        //var l = Ladda.create(document.querySelector("#btSearch"))
        //l.start();
        _Year = pyear;
        _StipDate = $("#tbSearchSTIPDate").val();
        _RfiDate = $("#tbSearchRFIDate").val();
        _SectionID = $("#slSearchSection").val();
        _SowID = $("#slSearchSOW").val();
        _StipID = $("#slSearchSTIPCategory").val();
        _ProductID = $("#slSearchProduct").val();
        _RegionalID = $("#slSearchRegional").val();
        _CompanyID = $("#slSearchCompany").val();

        $.ajax({
            url: "/api/DashboardPotentialRecurring/GetSummaryMonth",
            type: "GET",
            data: {
                type: _Type + 'ForMonth',
                STIPDate: _StipDate,
                RFIDate: _RfiDate,
                SectionID: _SectionID,
                SOWID: _SowID,
                ProductID: _ProductID,
                STIPID: _StipID,
                RegionalID: _RegionalID,
                CompanyID: _CompanyID,
                year: pyear,
                month: "",
                desc: pdesc, Customer: null

            }
        })
            .done(function (data, textStatus, jqXHR) {
                $('#pnlDetail').fadeOut();
                $('.panelSearchResult').fadeOut();
                $('.panelSearchResultMonth').fadeIn(1000);

                $("#tblHeadSummaryMonth").html("");
                $("#tblHeadSummaryMonth").append(data.tblHead);
                //l.stop();
                var columns = [];
                var idx = 0;

                $.each(data.customcolumn, function (i, item) {

                    if (idx > 0)
                        columns[idx] = { data: item, className: "text-right", render: function (data, type, full) { return "<a class='btDetail " + item + "'>" + data.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "</a>"; } };
                    //columns[idx] = { data: item, className: "text-right", render: function (data, type, full) { return "<a class='btSummaryMonth " + item + "'>" + data.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "</a>"; } };
                    //else if (idx == 1)
                    //    columns[idx] = { data: item.data, className: item.className, mRender: function (data, type, full) { return "<a class='btDetail'>" + data + "</a>"; } };
                    else
                        columns[idx] = { data: item, className: "text-left" };

                    idx += 1;
                });

                $('#tblSummaryMonth').DataTable({
                    data: JSON.parse(data.dataSummary),
                    buttons: [
                        { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                        {
                            text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                                var l = Ladda.create(document.querySelector(".yellow"));
                                l.start();
                                Table.Export('tblSummaryMonth', 'Potential Recurring (Month)', 'Potential Recurring (Month)');
                                l.stop();
                            }
                        },
                        { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
                    ],
                    "lengthMenu": [[-1], ['All']],
                    "columns": columns,
                    searching: false,
                    "footerCallback": function (row, data, start, end, display) {
                        var api = this.api();
                        nb_cols = api.columns().nodes().length;
                        var j = 1;
                        var param = "";
                        while (j < nb_cols) {
                            var pageTotal = api
                                .column(j, { page: 'current' })
                                .data()
                                .reduce(function (a, b) {
                                    return Number(a) + Number(b);
                                }, 0);
                            //set class custom for parameter Details
                            if (j == 1)
                                param = "JAN";
                            else if (j == 2)
                                param = "FEB";
                            else if (j == 3)
                                param = "MAR";
                            else if (j == 4)
                                param = "APR";
                            else if (j == 5)
                                param = "MAY";
                            else if (j == 6)
                                param = "JUN";
                            else if (j == 7)
                                param = "JUL";
                            else if (j == 8)
                                param = "AUG";
                            else if (j == 9)
                                param = "SEP";
                            else if (j == 10)
                                param = "OCT";
                            else if (j == 11)
                                param = "NOV";
                            else if (j == 12)
                                param = "DEC";
                            else if (j == 13)
                                param = "GrandTotal";

                            // Update footer
                            if (j == 1 || j == 2 || j == 3 || j == 4 || j == 5 || j == 6 || j == 7 || j == 8 || j == 9 || j == 10 || j == 11 || j == 12 || j == 13) {
                                $(api.column(j).footer()).html("<a class=" + "'btDetailTotal " + param + "'>" + pageTotal.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "</a>");
                                //param++;
                            }

                            j++;
                        }
                    },
                    "order": [],
                    "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

                    "destroy": true
                });

                $("#tblSummaryMonth tbody").on("click", "a.btDetail", function (e) {
                    e.preventDefault();
                    var col = e.currentTarget.attributes.class.nodeValue.split(" ")[1];
                    var table = $("#tblSummaryMonth").DataTable();
                    var data = table.row($(this).parents("tr")).data();
                    _paramRow = data.Description;
                    if (data != null) {
                        if (col != "GrandTotal") {
                            _paramCol = col;
                        } else {
                            _paramCol = "";
                        }
                        Table.Details();
                    }
                });

                $("#tblSummaryMonth tfoot").on("click", ".btDetailTotal", function (e) {
                    e.preventDefault();
                    var data = e.currentTarget.attributes.class.nodeValue.split(" ")[1];
                    var rowCount = $('#tblSummaryMonth tr').length;

                    if (rowCount = 4) {

                    }
                    else {
                        _paramRow = "";
                    }

                    if (data != null) {
                        if (data != "GrandTotal") {
                            _paramCol = data;
                        } else {
                            _paramCol = "";
                        }
                        Table.Details();
                    }
                });

                App.unblockUI(".portlet-Main");
                //console.log(JSON.parse(data.dataSummary));
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                App.unblockUI(".portlet-Main");
                Common.Alert.Error(errorThrown);
            });
    },
    Details: function () {
        App.blockUI({ target: ".portlet-Main", animate: !0 });
        $('.panelSearchResult').fadeOut();
        $('.panelSearchResultMonth').fadeOut();
        $('#pnlDetail').fadeIn(1000);
        //var l = Ladda.create(document.querySelector("#btSearch"))
        //l.start();

        var params = {
            type: _Type,
            STIPDate: _StipDate,
            RFIDate: _RfiDate,
            SecName: _SectionID,
            SOWName: _SowID,
            ProductID: _ProductID,
            STIPID: _StipID,
            RegionalID: _RegionalID,
            CompanyID: _CompanyID,
            paramRow: _paramRow,
            paramColumn: _paramCol,
            YearBill: _Year, Customer: null,
            SoNumber: $("#slSearchDetailSonum").val(),
            SiteID: $("#slSearchDetailSiteID").val(),
            SiteName: $("#slSearchDetailSiteName").val(),
        }
        _Params = {
            type: _Type,
            STIPDate: _StipDate,
            RFIDate: _RfiDate,
            SecName: _SectionID,
            SOWName: _SowID,
            ProductID: _ProductID,
            STIPID: _StipID,
            RegionalID: _RegionalID,
            CompanyID: _CompanyID,
            paramRow: _paramRow,
            paramColumn: _paramCol,
            YearBill: _Year, Customer: null
        }
        var tblRaw = $("#tblDetail").DataTable({
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
                "url": "/api/DashboardPotentialRecurring/GetDetail",
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
                        Table.ExportDetail();
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[25, 50, 100, 200, 7000], ['25', '50', '100', '200', '7000']],
            "destroy": true,
            "columns": [
                {
                    "data": "id",
                    render: function (data, type, row, meta) {
                        return meta.row + meta.settings._iDisplayStart + 1;
                    }
                },
                { data: "SoNumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CustomerID" },
                { data: "RegionName" },
                { data: "ProvinceName" },

                { data: "ResidenceName" },
                {
                    data: "RFIDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "FirstBAPSDone", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "StipCategory" },
                { data: "PONumber" },
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

            ],
            "columnDefs": [{ "targets": [0], "orderable": true }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".portlet-Main", boxed: true });
            },
            "fnDrawCallback": function () {
                //l.stop();
                App.unblockUI(".portlet-Main");
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

    Export: function (table, name, filename) {
        var uri = 'data:application/vnd.ms-excel;base64,'
            , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><meta charset="utf-8"/><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
            , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
            , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }

        if (!table.nodeType) table = document.getElementById(table)
        var ctx = { worksheet: name || 'Worksheet', table: table.innerHTML }

        document.getElementById("dlink").href = uri + base64(format(template, ctx));
        document.getElementById("dlink").download = filename;
        document.getElementById("dlink").click();
    },

    ExportDetail: function () {

        var type = _Type;
        var STIPDate = _StipDate;
        var RFIDate = _RfiDate;
        var SectionID = _SectionID;
        var SOWID = _SowID;
        var ProductID = _ProductID;
        var STIPID = _StipID;
        var RegionalID = _RegionalID;
        var CompanyID = _CompanyID;
        var paramRow = _paramRow;
        var paramColumn = _paramCol;
        var YearBill = _Year;

        var SoNumber = $("#slSearchDetailSonum").val();
        var SiteID = $("#slSearchDetailSiteID").val();
        var SiteName = $("#slSearchDetailSiteName").val();

        window.location.href = "/DashboardRA/ExportDetailPotentialRecurring?strtype=" + type + "&strSTIPDate=" + STIPDate + "&strRFIDate=" + RFIDate
            + "&strSectionID=" + SectionID + "&strSOWID=" + SOWID + "&strProductID=" + ProductID + "&strSTIPID=" + STIPID
            + "&strRegionalID=" + RegionalID + "&strCompanyID=" + CompanyID + "&strparamRow=" + paramRow + "&strparamColumn=" + paramColumn + "&strYearBill=" + YearBill
            + "&SoNumber=" + SoNumber + "&SiteID=" + SiteID + "&SiteName=" + SiteName;
    }
}

var Control = {

    BindingSelectSection: function (id) {
        $.ajax({
            url: "/api/DashboardPotentialRecurring/Section",
            type: "GET"
        })
            .done(function (data, textstatus, jqxhr) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.DepartmentCode + "'>" + item.Description + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Section", width: null });
            })
            .fail(function (jqxhr, textstatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectSow: function (id) {
        $.ajax({
            url: "/api/DashboardPotentialRecurring/SOW",
            type: "GET"
        })
            .done(function (data, textstatus, jqxhr) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.DepartmentCode + "'>" + item.Description + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select SOW", width: null });
            })
            .fail(function (jqxhr, textstatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectRegional: function (id) {
        $.ajax({
            url: "/api/MstDataSource/Regional",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.RegionalId + "'>" + item.RegionalName + "</option>");
                    })
                }

                $(id).select2({ placeholder: "Select Regional", width: null });

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectCompany: function (id) {
        $.ajax({
            url: "/api/DashboardPotentialRecurring/CompanyList",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.ValueString + "'>" + item.Type + "</option>");
                    })
                }

                $(id).select2({ placeholder: "Select Company", width: null });

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingSelectSTIP: function (id) {
        $.ajax({
            url: "/api/DashboardPotentialRecurring/STIPCategory",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.Value + "'>" + item.Type + "</option>");
                    })
                }

                $(id).select2({ placeholder: "Select STIP", width: null });

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingSelectProduct: function (id, section, sow) {
        $.ajax({
            url: "/api/DashboardPotentialRecurring/ProductList",
            type: "GET",
            data: { sectionID: section, sowID: sow }
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.Value + "'>" + item.Type + "</option>");
                    })
                }

                $(id).select2({ placeholder: "Select Product", width: null });

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
}