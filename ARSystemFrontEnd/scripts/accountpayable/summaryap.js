var ProductCurrent = "";

$(document).ready(function () {
    $(".panelSearchResultTenant").hide();
    $(".panelSearchResultTenantDetail").hide();
    $(".panelSearchResultSummary").hide();
    Table.Grouping();
    Table.Init();

    $("#btBackSummary").unbind().click(function () {
        $(".panelSearchResultTenant").hide();
        $(".panelSearchResultTenantDetail").hide();
        $(".panelSearchResult").hide();
        $(".panelSearchResultSummary").fadeIn(1000);
        Form.ClearInformation();
    });

    $("#btBackTenant").unbind().click(function () {
        Form.ClearInformation();
        $(".panelSearchResult").hide();
        $(".panelSearchResultTenantDetail").hide();
        $(".panelSearchResultTenant").fadeIn(1000);
    });
});

var Form = {
    HideSummary: function(){
        Form.ClearInformation();
        $(".panelSearchResultTenantDetail").hide();
        $(".panelSearchResultSummary").hide();
        $(".panelSearchResultTenant").hide();
        $(".panelSearchResult").fadeIn(1000);
    },
    ShowSummary: function (strProduct) {
        Table.Summary(strProduct);
        ProductCurrent = strProduct;
        $(".panelSearchResultTenantDetail").hide();
        $(".panelSearchResult").hide();
        $(".panelSearchResultTenant").hide();
        $(".panelSearchResultSummary").fadeIn(1000);
    },
    ShowTenant: function (strProduct,strQty) {
        Table.BindingTenant(strProduct, strQty);
        ProductCurrent = strProduct;
        $(".panelSearchResultTenantDetail").hide();
        $(".panelSearchResult").hide();
        $(".panelSearchResultSummary").hide();
        $(".panelSearchResultTenant").fadeIn(1000);
    },
    ShowTenantDetail: function (data) {
        $("#txtSONumber").val(data.SONumber);
        $("#txtSiteID").val(data.SiteID);
        $("#txtSiteName").val(data.SiteName);
        $("#txtProduct").val(data.Product);
        $("#txtCustomerSiteID").val(data.CustomerSiteID);
        $("#txtCustomerSiteName").val(data.CustomerSiteName);
        $("#txtRegional").val(data.RegionalName);
        $("#txtStipCategory").val(data.StipCategory);

        Table.BindingTenantDetail(data.SONumber);
        $(".panelSearchResultTenant").hide();
        $(".panelSearchResult").hide();
        $(".panelSearchResultTenantDetail").fadeIn(1000);
    },
    ClearInformation: function () {
        $("#txtSONumber").val("");
        $("#txtSiteID").val("");
        $("#txtSiteName").val("");
        $("#txtProduct").val("");
        $("#txtCustomerSiteID").val("");
        $("#txtCustomerSiteName").val("");
        $("#txtRegional").val("");
        $("#txtStipCategory").val("");
    }
}

var Table = {
    Init: function () {
        var tblSummaryTenant = $('#tblSummaryTenant').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        var tblSummaryTenantCost = $('#tblSummaryTenantCost').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        var tblSummaryTenantRevenue = $('#tblSummaryTenantRevenue').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $("#tblSummaryTenant tbody").on("click", "a.btDetailTenant", function (e) {
            e.preventDefault();
            var table = $("#tblSummaryTenant").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Form.ShowTenantDetail(data);
            }
        });
    },
    Grouping: function () {
        $.ajax({
            url: "/api/SummaryAP/GetGroup",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            cache: false,
        })
        .done(function (data, textStatus, jqXHR) {
            

            $('#tblHead').DataTable({
                data: JSON.parse(data.dataSummary),
                buttons: [
                    { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                    //{
                    //    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                    //        var l = Ladda.create(document.querySelector(".yellow"));
                    //        l.start();
                    //        Table.Export('tblHead', 'SummaryGroup', 'GroupSummaryAP');
                    //        l.stop();
                    //    }
                    //},
                    {
                        extend: 'excelHtml5',
                        footer: true,
                        filename: 'GroupSummaryAP',
                        text: '<i class="fa fa-file-excel-o"></i>',
                        titleAttr: 'Export to Excel',
                        className: 'btn yellow btn-outline',
                        rows: ':visible'
                    },
                    { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
                ],
                "paging": false,
                "columns": [
                    { data: "Product" },
                    { data: "Qty", className: "text-center", mRender: function (data, type, full) { return "<a class='btDetail'>" + data + "</a>"; } },
                    { data: "TotalCost", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "TotalRevenue", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                ],
                searching: false,
                "order": [],
                "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;
                    var colNumber = [1,2,3];


                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };

                    var numformat = $.fn.dataTable.render.number(',', '.', 2, '').display;

                    for (i = 0; i < colNumber.length; i++) {

                        var colNo = colNumber[i];

                        if (i > 0) {
                            TotalAmount = api
                                .column(colNo, { page: 'all' })
                                .data()
                                .reduce(function (a, b) {
                                    return intVal(a) + intVal(b);
                                }, 0);
                            $(api.column(colNo).footer()).html(numformat(TotalAmount));
                        }
                        else {
                            TotalAmount = api
                                .column(colNo, { page: 'all' })
                                .data()
                                .reduce(function (a, b) {
                                    return intVal(a) + intVal(b);
                                }, 0);
                            $(api.column(colNo).footer()).html(TotalAmount);
                        }
                    }
                },
                "destroy": true
            });

            $("#tblHead tbody").on("click", "a.btDetail", function (e) {
                e.preventDefault();
                var table = $("#tblHead").DataTable();
                var data = table.row($(this).parents("tr")).data();
                if (data != null) {
                    //console.log(data);
                    Form.ShowSummary(data.Product);
                }
            });
            //console.log(JSON.parse(data.dataSummary));
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    Summary: function (ProductSite) {
        App.blockUI({ target: ".portlet-body", animate: !0 });
        
        var params = { ProductSite: ProductSite }

        $.ajax({
            url: "/api/SummaryAP/GetData",
            type: "GET",
            data: { ProductSite: ProductSite }
        })
        .done(function (data, textStatus, jqXHR) {
            $("#tblHeadSummary").html("");
            $("#tblHeadSummary").append(data.tblHead);

            var columns = [];
            var idx = 0;

            $.each(data.customcolumn, function (i, item) {

                if (idx > 1)
                    columns[idx] = { data: item.data, className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') };
                else if (idx == 1)
                    columns[idx] = { data: item.data, className: item.className, mRender: function (data, type, full) { return "<a class='btDetail'>" + data + "</a>"; } };
                else
                    columns[idx] = { data: item.data, className: "text-left" };

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
                            Table.Export('tblSummary','Summary','SummaryAP');
                            l.stop();
                        }
                    },
                    { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
                ],
                "lengthMenu": [[-1], ['All']],
                "columns": columns,
                searching: false,
                "order": [],
                "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                "destroy": true
            });

            $("#tblSummary tbody").on("click", "a.btDetail", function (e) {
                e.preventDefault();
                var table = $("#tblSummary").DataTable();
                var data = table.row($(this).parents("tr")).data();
                if (data != null) {
                    //console.log(data);
                    Form.ShowTenant(data.Product,data.Qty);
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
    BindingTenant: function (strProduct, strQty) {
        var params = {
            strProduct: strProduct,
            strQty : strQty
        }

        var tblRaw = $("#tblSummaryTenant").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/SummaryAP/GetTenantData",
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
                        Table.ExportTenant(strProduct);
                        l.stop();
                    }
                },
            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50, 100, 200], ['5', '10', '25', '50', '100', '200']],
            "destroy": true,
            "columns": [
                { data: "RowIndex" },
                { data: "SONumber", mRender: function (data, type, full) { return "<a class='btDetailTenant'>" + data + "</a>"; } },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "RegionalName" },
                { data: "StipCategory" },

                { data: "Product" },
                { data: "TotalCost", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "TotalRevenue", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResultTenant", boxed: true });
            },
            "fnDrawCallback": function () {
                App.unblockUI('.panelSearchResultTenant');

            },
            "order": [],
            "orderCellsTop": true,
            "scrollY": 300, /* Enable vertical scroll to allow fixed columns */
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "initComplete": function () {
                
            }
        });


        //App.blockUI({ target: ".portlet-body", animate: !0 });

        //$.ajax({
        //    url: "/api/SummaryAP/GetTenantData",
        //    type: "GET",
        //    data: { Product: strProduct },
        //})
        //.done(function (data, textStatus, jqXHR) {
            

        //    //console.log(data);
        //    //var tblProcessDetail = $("#tblSummaryTenant").DataTable({
        //    //    searching:true,
        //    //    data: data,
        //    //    buttons: [
        //    //        { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
        //    //        {
        //    //            text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
        //    //                var l = Ladda.create(document.querySelector(".yellow"));
        //    //                l.start();
        //    //                Table.ExportTenant(strProduct);
        //    //                l.stop();
        //    //            }
        //    //        },
        //    //        { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
        //    //    ],
        //    //    "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
        //    //    "scrollY": "300px",
        //    //    "scrollCollapse": true,
        //    //    "paging": false,
        //    //    "columns": [
        //    //        { data: "RowIndex" },
        //    //        { data: "SONumber", mRender: function (data, type, full) { return "<a class='btDetailTenant'>" + data + "</a>"; } },
        //    //        { data: "SiteID" },
        //    //        { data: "SiteName" },
        //    //        { data: "RegionalName" },
        //    //        { data: "StipCategory" },

        //    //        { data: "Product" },
        //    //        { data: "TotalCost", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
        //    //        { data: "TotalRevenue", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
        //    //    ],
        //    //    "order": [],
        //    //    "destroy": true,
        //    //    //"footerCallback": function (row, data, start, end, display) {
        //    //    //    var api = this.api(), data;
        //    //    //    var colNumber = [7,8];


        //    //    //    var intVal = function (i) {
        //    //    //        return typeof i === 'string' ?
        //    //    //            i.replace(/[\$,]/g, '') * 1 :
        //    //    //            typeof i === 'number' ?
        //    //    //            i : 0;
        //    //    //    };

        //    //    //    var numformat = $.fn.dataTable.render.number(',', '.', 2, '').display;

        //    //    //    for (i = 0; i < colNumber.length; i++) {

        //    //    //        var colNo = colNumber[i];

        //    //    //        TotalAmount = api
        //    //    //                .column(colNo, { page: 'all' })
        //    //    //                .data()
        //    //    //                .reduce(function (a, b) {
        //    //    //                    return intVal(a) + intVal(b);
        //    //    //                }, 0);
        //    //    //        $(api.column(colNo).footer()).html(numformat(TotalAmount));

        //    //    //    }
        //    //    //},
        //    //});

        //    //$(window).resize(function () {
        //    //    $("#tblSummaryTenant").DataTable().columns.adjust().draw();
        //    //});
        //    App.unblockUI(".portlet-body");
        //})
        //.fail(function (jqXHR, textStatus, errorThrown) {
        //    App.unblockUI(".portlet-body");
        //    Common.Alert.Error(errorThrown);
        //});
    },
    BindingTenantDetail: function (strSONumber) {
        $.ajax({
            url: "/api/SummaryAP/GetTenantCostData",
            type: "GET",
            data: { SONumber: strSONumber },
        })
        .done(function (data, textStatus, jqXHR) {
            var tblSummaryTenantCost = $("#tblSummaryTenantCost").DataTable({
                searching: true,
                data: data,
                buttons: [
                    { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                    {
                        text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                            var l = Ladda.create(document.querySelector(".yellow"));
                            l.start();
                            Table.ExportTenantCost(strSONumber);
                            l.stop();
                        }
                    },
                    { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
                ],
                "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                "scrollY": "300px",
                "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
                "scrollCollapse": true,
                "fixedColumns": {
                    leftColumns: 3 /* Set the 2 most left columns as fixed columns */
                },
                "paging": false,
                "columns": [
                    { data: "RowIndex" },
                    { data: "SourceData" },
                    { data: "DocumentNumber" },
                    { data: "Termin" },
                    { data: "VOUCHER" },
                    { data: "TRANSDATE" },
                    { data: "InvoiceNumber" },
                    { data: "Amount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "Description" },
                ],
                "order": [],
                "destroy": true,
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;
                    var colNumber = [7];


                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };

                    var numformat = $.fn.dataTable.render.number(',', '.', 2, '').display;

                    for (i = 0; i < colNumber.length; i++) {

                        var colNo = colNumber[i];

                        TotalAmount = api
                                .column(colNo, { page: 'all' })
                                .data()
                                .reduce(function (a, b) {
                                    return intVal(a) + intVal(b);
                                }, 0);
                        $(api.column(colNo).footer()).html(numformat(TotalAmount));

                    }
                },
            });

            //var dat = tblSummaryTenantCost.data();
            var scrollPos = tblSummaryTenantCost.scroller().pixelsToRow($('.dataTables_scrollBody').scrollTop());
            //console.log(scrollPos);
            //tblSummaryTenantCost.clear().rows.add(dat).draw();

            $(window).resize(function () {
                $("#tblSummaryTenantCost").DataTable().columns.adjust().draw();
            });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });

        $.ajax({
            url: "/api/SummaryAP/GetTenantRevenueData",
            type: "GET",
            data: { SONumber: strSONumber },
        })
        .done(function (data, textStatus, jqXHR) {
            var tblSummaryTenantRevenue = $("#tblSummaryTenantRevenue").DataTable({
                searching: true,
                data: data,
                buttons: [
                    { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                    {
                        text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                            var l = Ladda.create(document.querySelector(".yellow"));
                            l.start();
                            Table.ExportTenantRevenue(strSONumber);
                            l.stop();
                        }
                    },
                    { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
                ],
                "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                "scrollY":        "300px",
                "scrollCollapse": true,
                "paging":         false,
                "columns": [
                    { data: "RowIndex" },
                    { data: "TypePayment" },
                    { data: "CustomerID" },
                    { data: "InvoiceNumber" },
                    { data: "Period" },
                    { data: "Amount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                ],
                "order": [],
                "destroy": true,
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;
                    var colNumber = [5];


                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };

                    var numformat = $.fn.dataTable.render.number(',', '.', 2, '').display;

                    for (i = 0; i < colNumber.length; i++) {

                        var colNo = colNumber[i];

                        TotalAmount = api
                                .column(colNo, { page: 'all' })
                                .data()
                                .reduce(function (a, b) {
                                    return intVal(a) + intVal(b);
                                }, 0);
                        $(api.column(colNo).footer()).html(numformat(TotalAmount));

                    }
                },
            });

           // var dat = tblSummaryTenantRevenue.data();
            var scrollPos = tblSummaryTenantRevenue.scroller().pixelsToRow($('.dataTables_scrollBody').scrollTop());
            //console.log(scrollPos);
            //tblSummaryTenantRevenue.clear().rows.add(dat).draw();

            $(window).resize(function () {
                $("#tblSummaryTenantRevenue").DataTable().columns.adjust().draw();
            });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    ExportTenantRevenue: function (strSONumber) {
        window.location.href = "/AccountPayable/ExportTenantRevenue?strSONumber=" + strSONumber;
    },
    ExportTenantCost: function (strSONumber) {
        window.location.href = "/AccountPayable/ExportTenantCost?strSONumber=" + strSONumber;
    },
    ExportTenant: function (strProduct) {
        window.location.href = "/AccountPayable/ExportTenant?strProduct=" + strProduct;
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
    }
}