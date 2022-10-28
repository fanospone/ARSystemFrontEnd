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

jQuery(document).ready(function () {
    Form.Init();

    $("#btSearch").unbind().click(function () {
        Table.Summary();
    });


    $("#btBack").unbind().click(function () {
        $('#pnlDetail').fadeOut();
        $('.panelSearchResult').fadeIn(1000);

    });

    $("#btReset").unbind().click(function () {
        Form.Reset();
    });

    $("#slSearchSection").on('change', function () {
        Control.BindingSelectSow($('#slSearchSOW'), $("#slSearchSection").val());
        Control.BindingSelectProduct($('#slSearchProduct'), $("#slSearchSection").val(), "");
    });
    $("#slSearchSOW").on('change', function () {
        Control.BindingSelectProduct($('#slSearchProduct'), $("#slSearchSection").val(), $("#slSearchSOW").val());
    });

    $('input[name=rdStatus]').on('change', function () {
        _Type = $(this).val();
    });
});
var Form = {
    Init: function () {
        $('.panelSearchResult').hide();

        Control.BindingSelectSection($('#slSearchSection'));
        Control.BindingSelectSow($('#slSearchSOW'), "");
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

        $.ajax({
            url: "/api/DashboardTSEL/GetTSELOutstandingSummary",
            type: "GET",
            data: {
                type: _Type,
                STIPDate: _StipDate,
                RFIDate: _RfiDate,
                SectionID: _SectionID,
                SOWID: _SowID,
                ProductID: _ProductID,
                STIPID: _StipID,
                RegionalID: _RegionalID,
                CompanyID: _CompanyID

            }
        })
            .done(function (data, textStatus, jqXHR) {
                $('#pnlDetail').fadeOut();
                $('.panelSearchResult').fadeIn(1000);
                $("#tblHeadSummary").html("");
                $("#tblHeadSummary").append(data.tblHead);
                //l.stop();
                var columns = [];
                var idx = 0;

                $.each(data.customcolumn, function (i, item) {

                    if (idx > 0)
                        columns[idx] = { data: item, className: "text-right", render: function (data, type, full) { return "<a class='btDetail " + item + "'>" + data.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "</a>"; } };
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
                                Table.Export('tblSummary', 'OutstandingSummary', 'OutstandingSummary');
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
                            //set class custom for parameter detail
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

                $("#tblSummary tbody").on("click", "a.btDetail", function (e) {
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
                        Table.DetailOutstanding();
                    }
                });

                $("#tblSummary tfoot").on("click", ".btDetailTotal", function (e) {
                    e.preventDefault();
                    var data = e.currentTarget.attributes.class.nodeValue.split(" ")[1];
                    _paramRow = "";
                    if (data != null) {
                        if (data != "GrandTotal") {
                            _paramCol = data;
                        } else {
                            _paramCol = "";
                        }
                        Table.DetailOutstanding();
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

    DetailOutstanding: function () {
        $('.panelSearchResult').fadeOut();
        $('#pnlDetail').fadeIn(1000);
        //var l = Ladda.create(document.querySelector("#btSearch"))
        //l.start();

        var params = {
            type: _Type,
            STIPDate: _StipDate,
            RFIDate: _RfiDate,
            SectionID: _SectionID,
            SOWID: _SowID,
            ProductID: _ProductID,
            STIPID: _StipID,
            RegionalID: _RegionalID,
            CompanyID: _CompanyID,
            paramRow: _paramRow,
            paramColumn: _paramCol
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
                "url": "/api/DashboardTSEL/GetDetailSiteOutstanding",
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
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                //l.stop();
                App.unblockUI('.panelSearchResult');
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
        var STIPDate= _StipDate;
        var RFIDate= _RfiDate;
        var SectionID= _SectionID;
        var SOWID= _SowID;
        var ProductID= _ProductID;
        var STIPID= _StipID;
        var RegionalID= _RegionalID;
        var CompanyID= _CompanyID;
        var paramRow= _paramRow;
        var paramColumn= _paramCol

        window.location.href = "/DashboardTSEL/ExportDetailSiteOutstanding?strtype=" + type + "&strSTIPDate=" + STIPDate + "&strRFIDate=" + RFIDate
            + "&strSectionID=" + SectionID + "&strSOWID=" + SOWID + "&strProductID=" + ProductID + "&strSTIPID=" + STIPID
            + "&strRegionalID=" + RegionalID + "&strCompanyID=" + CompanyID + "&strparamRow=" + paramRow + "&strparamColumn=" + paramColumn;
    }
}

var Control = {
    BindingSelectSection: function (id) {
        $.ajax({
            url: "/api/DashboardTSEL/Section",
            type: "GET"
        })
            .done(function (data, textstatus, jqxhr) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.SectionProductId + "'>" + item.SectionName + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Section", width: null });
            })
            .fail(function (jqxhr, textstatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectSow: function (id, section) {
        $.ajax({
            url: "/api/DashboardTSEL/SOWbyParamList",
            type: "GET",
            data: { sectionID: section }
        })
            .done(function (data, textstatus, jqxhr) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.Value + "'>" + item.Type + "</option>");
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
            url: "/api/DashboardTSEL/CompanyList",
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
            url: "/api/DashboardTSEL/STIPCategory",
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
            url: "/api/DashboardTSEL/ProductList",
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