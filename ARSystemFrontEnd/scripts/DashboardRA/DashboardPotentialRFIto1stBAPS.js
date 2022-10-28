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
var _Type = "ForecastinGroupByCountForWeek";
var _TypeMonth = "ForecastingGroupByMonth";
var _Year = new Date().getFullYear().toString();

var _TotalSumSTIP = 0;
var _TotalSumTP = 0;
var _TotalSumBAPS = 0;

var _Params = {}



jQuery(document).ready(function () {
    Form.Init();
    $(document).keypress(function (e) {
        if (e.which == 13) {
            Table.Summary();
        }
    });
   
    
    

    $("#btSearch").unbind().click(function () {
        if ($("#slSearchSTIP").val() != '' && $("#slSearchSTIP").val() != null && $("#slSearchYear").val() != '' && $("#slSearchYear").val() != null)
        {
            $('#pnlDetail').fadeOut();
            $('.panelSearchResultWeek').fadeOut();
            $('.pnlDetail').fadeOut();
            $('.panelSearchResultMonth').fadeIn(1000);
            $('.panelSearchChart').fadeIn(1000);

            Table.Summary();
            _TotalSumSTIP = 0;
            _TotalSumTP = 0;
            _TotalSumBAPS = 0;
            Control.BindingSummary();
        }
        else {
            Common.Alert.Warning("Please Select STIP Category And Year !");
            return false;
        }

       
    });

    $("#btSearchDetail").unbind().click(function () {
        $('.panelSearchResultWeek').fadeOut();
        $('.pnlDetail').fadeOut();
        $('.panelSearchResultMonth').fadeOut();
        $('.panelSearchChart').fadeOut();
        $('#pnlDetail').fadeIn(1000);
   
        Table.Details(_ParamExport.Type, _ParamExport.RFIDateMonth, _ParamExport.RFIDateWeek, _ParamExport.CustomerID, _ParamExport.Step);

    });

    $("#btResetDetail").unbind().click(function () {
        $("#slSearchDetailSonum").val('');
        $("#slSearchDetailSiteID").val('');
        $("#slSearchDetailSiteName").val('');
        Table.Details(_ParamExport.Type, _ParamExport.RFIDateMonth, _ParamExport.RFIDateWeek, _ParamExport.CustomerID, _ParamExport.Step);
    });

     
    $("#btBackFromWeek").unbind().click(function () {

        $('.pnlDetail').fadeOut();
        $('.panelSearchResultWeek').fadeOut();
        $('.panelSearchResultMonth').fadeIn(1000);

    });
    $("#btBackDetail").unbind().click(function () {
        $("#slSearchDetailSonum").val('');
        $("#slSearchDetailSiteID").val('');
        $("#slSearchDetailSiteName").val('');
        if (_ParamExport.RFIDateWeek != null) {
            $('.pnlDetail').fadeOut();
            $('.panelSearchResultMonth').fadeOut();
            $('.panelSearchResultWeek').fadeIn(1000);
            $('.panelSearchChart').fadeIn(1000);
        }
        else
        {

            $('.pnlDetail').fadeOut();
            $('.panelSearchResultWeek').fadeOut();
            $('.panelSearchResultMonth').fadeIn(1000);
            $('.panelSearchChart').fadeIn(1000);

            
        }

    });
    
    $("#btReset").unbind().click(function () {
        Form.Reset();
    });
 

    $('input[name=rdStatusWeek]').on('change', function () {
        _Type = $(this).val();
        Table.SummaryWeek(_paramCol, _paramRow);
    });

    $('input[name=rdStatusMonth]').on('change', function () {
        _TypeMonth = $(this).val();
        Table.Summary();
    });

    //$('input[id=schDetailSonum]').on('change', function (e) {
    ////alert('UHUY!');
    ////var table = $('#tblDetail').DataTable({
    ////    initComplete: function () {
    ////        // Apply the search
    ////        this.api().columns().every(function () {
    ////            var that = this;

    ////            $('schDetailSonum', this.val()).on('keyup change clear', function () {
    ////                if (that.search() !== this.value) {
    ////                    that
    ////                        .search(this.value)
    ////                        .draw();
    ////                }
    ////            });
    ////        });
    ////    }
    ////});
    ////$('#tblDetail tfoot th').each(function () {
    ////    var title = $(this).text();
    ////    $(this).html('<input type="text" placeholder="Search ' + title + '" />');
    ////});
    //    //$('#schDetailSonum input').unbind();
    //    //$('#schDetailSonum input').bind('keyup', function (e) {
    //    //    var that = $('#tblDetail').DataTable();
    //    //    if (that.search() !== this.value) that.search(this.value);
    //    //    // if serverside draw() only on enter
    //    //    if (e.keyCode == 13) that.draw();
    //    //});
    //    var that = $('#tblDetail').DataTable();
    //    if (that.search() !== this.value) that.search(this.value);
    //    // if serverside draw() only on enter
    //    if (e.keyCode == 13) that.draw();
    //});

});


var Form = {
    Init: function () {
        $('.panelSearchResult').hide();
        $('.panelSearchResultMonth').hide();

        Control.BindingSelectYear($('#slSearchYear'));
        Control.BindingSelectSTIP($('#slSearchSTIP'));
       
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
        $("#slSearchYear").val(null).trigger("change");
        $("#slSearchSTIP").val(null).trigger("change");
    }
}

var Table = {
    Summary: function () {

            App.blockUI({ target: ".portlet-body", animate: !0 });
            $.ajax({
                url: "/api/DashboardPotentialRFITo1stBAPS/GetSummaryMonth",
                // (string Type, string STIP, string year, string month, string desc)
                type: "GET",
                data: {
                    Type: _TypeMonth,
                    STIP: $("#slSearchSTIP").val(),
                    year: $("#slSearchYear").val(),
                    month: null,
                    desc: null

                }
            })
                .done(function (data, textStatus, jqXHR) {
                    

                    $("#tblHeadSummaryMonth").html("");
                    $("#tblHeadSummaryMonth").append(data.tblHead);
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

                    $('#tblSummaryMonth').DataTable({
                        data: JSON.parse(data.dataSummary),
                        buttons: [
                            { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                            {
                                text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                                    var l = Ladda.create(document.querySelector(".yellow"));
                                    l.start();
                                    var filename = 'Potential RFI ' + $("#slSearchSTIP").val() + ' ' + $("#slSearchYear").val() +  ' (Months)';
                                    Table.ExportSummary('tblSummaryMonth', 'Potential RFI', filename, '');
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
                                    $(api.column(j).footer()).html("<a class=" + "'btSummaryMonthDetail " + param + "'>" + pageTotal.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "</a>");
                                    //param++;
                                }
                             
                                j++;
                            }
                        },
                        "order": [],
                        "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

                        "destroy": true
                    });

                    $("#tblSummaryMonth tbody").on("click", "a.btSummaryMonth", function (e) {
                        //$("#tblSummary tbody").on("click", "a.btDetail", function (e) {                
                        e.preventDefault();
                        var col = e.currentTarget.attributes.class.nodeValue.split(" ")[1];
                        var table = $("#tblSummaryMonth").DataTable();
                        var data = table.row($(this).parents("tr")).data();
                        _paramRow = data.Description;
                        if (data != null) {
                            if (col == "13") {
                                _paramCol = "";
                               
                                
                                Table.Details(null, null, null, _paramRow, "Total");
                            } else {
                                _paramCol = col;
                                Table.SummaryWeek(_paramCol, _paramRow);
                               
                            }
                            
                        }
                    });

                    $("#tblSummaryMonth tfoot").on("click", ".btSummaryMonthDetail", function (e) {
                        e.preventDefault();
                        var data = e.currentTarget.attributes.class.nodeValue.split(" ")[1];
                        _paramRow = "";
                        if (data != null) {
                            if (data != "GrandTotal") {
                                _paramCol = data;
                            } else {
                                _paramCol = null;
                            }
                            Table.Details(null, _paramCol, null, null, "FooterTotal");
                        }
                    });
                    App.unblockUI(".portlet-body");
                 
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    App.unblockUI(".portlet-body");
                    Common.Alert.Error(errorThrown);
                });

       
    },   
    SummaryWeek: function (pmonth, pdesc) {
        $('.pnlDetail').fadeOut();
        $('.panelSearchResult').fadeOut();
        $('.panelSearchResultMonth').fadeOut();
        $('.panelSearchResultWeek').fadeIn(1000);
        var params = {
            Type: _Type,
            STIP: $("#slSearchSTIP").val(),
            year: $("#slSearchYear").val(),
            month: pmonth,
            desc: pdesc
             
        }
        var tblRaw = $("#tblSummaryWeeks").DataTable({
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
                "url": "/api/DashboardPotentialRFITo1stBAPS/GetSummaryWeek",
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
                        var filename = 'Potential RFI ' + $("#slSearchSTIP").val() + ' ' +$("#slSearchYear").val() +  ' (Weeks)';
                        Table.ExportSummary('tblSummaryWeeks', 'Potential RFI', filename, 'week');
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[25, 50, 100, 200, 7000], ['25', '50', '100', '200', '7000']],
            "destroy": true,
            "columns": [
                { data: "EstimasiRTI", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 0, '') },
               
                {
                    'mRender': function (data, type, row, meta) {
                        var id = row.BAUKSUBMIT;
                        return "<a class='btSummaryWeeksBAUKSUBMIT " + id + "'>" +id+ "</a>";
                    }
                },
                {
                    'mRender': function (data, type, row, meta) {
                        var id = meta.settings.fnFormatNumber(row.BAUKREVIEW);
                        return "<a class='btSummaryWeeksBAUKREVIEW " + id + "'>" + id + "</a>";
                    }
                },
               
                {
                    'mRender': function (data, type, row, meta) {
                        var id = meta.settings.fnFormatNumber(row.BAUKVALIDATION);
                        return "<a class='btSummaryWeeksBAUKVALIDATION " + id + "'>" + id + "</a>";
                    }
                },
                
                {
                    'mRender': function (data, type, row, meta) {
                        var id = meta.settings.fnFormatNumber(row.BAUKDONE);
                        return "<a class='btSummaryWeeksBAUKDONE " + id + "'>" + id + "</a>";
                    }
                },
               
                {
                    'mRender': function (data, type, row, meta) {
                        var id = meta.settings.fnFormatNumber(row.BAPSPRODUCTION);
                        return "<a class='btSummaryWeeksBAPSPRODUCTION " + id + "'>" + id + "</a>";
                    }
                },
                
                {
                    'mRender': function (data, type, row, meta) {
                        var id = meta.settings.fnFormatNumber(row.RTI);
                        return "<a class='btSummaryWeeksRTI " + id + "'>" + id + "</a>";
                    }
                },
                {
                    'mRender': function (data, type, row, meta) {
                        var id = meta.settings.fnFormatNumber(row.TOTAL);
                        return "<a class='btSummaryWeeksTotal " + id + "'>" + id + "</a>";
                    }
                },
               
            ],
            "columnDefs": [{ "targets": [0], "orderable": true }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".portlet-Main", boxed: true });
            },
            "fnDrawCallback": function () {                
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
        var j = pmonth;
        var mth = "";
        if (j == 1)
            mth = "JAN";
        else if (j == 2)
            mth = "FEB";
        else if (j == 3)
            mth = "MAR";
        else if (j == 4)
            mth = "APR";
        else if (j == 5)
            mth = "MAY";
        else if (j == 6)
            mth = "JUN";
        else if (j == 7)
            mth = "JUL";
        else if (j == 8)
            mth = "AUG";
        else if (j == 9)
            mth = "SEP";
        else if (j == 10)
            mth = "OCT";
        else if (j == 11)
            mth = "NOV";
        else if (j == 12)
            mth = "DEC";


        var header = 'FORECASTING 1st BILLING - ' + $("#slSearchYear").val() + ' - ' + mth
        $("#thTitleOfSumWeeks").text(header);


        $("#tblSummaryWeeks tbody").on("click", "a.btSummaryWeeksBAUKSUBMIT", function (e) {              
            e.preventDefault();
            var col = e.currentTarget.attributes.class.nodeValue.split(" ")[1];
            var table = $("#tblSummaryWeeks").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                if (col != "TOTAL") {
                    _paramCol = col;                                  
                } else {
                    _paramCol = "";
                }
                Table.Details(null, _paramCol, data.EstimasiRTI, _paramRow, "BAUK SUBMIT");
            }
        });
         
        $("#tblSummaryWeeks tbody").on("click", "a.btSummaryWeeksBAUKREVIEW", function (e) {
                       
            e.preventDefault();
            var col = e.currentTarget.attributes.class.nodeValue.split(" ")[1];
            var table = $("#tblSummaryWeeks").DataTable();
            var data = table.row($(this).parents("tr")).data();
          
            if (data != null) {
               
                Table.Details(null, _paramCol, data.EstimasiRTI, _paramRow, "BAUK REVIEW");
            }
        });
        $("#tblSummaryWeeks tbody").on("click", "a.btSummaryWeeksBAUKVALIDATION", function (e) {
                   
            e.preventDefault();
            var col = e.currentTarget.attributes.class.nodeValue.split(" ")[1];
            var table = $("#tblSummaryWeeks").DataTable();
            var data = table.row($(this).parents("tr")).data();
           
            if (data != null) {
               
                Table.Details(null, _paramCol, data.EstimasiRTI, _paramRow, "BAUK VALIDATION");
            }
        });
        $("#tblSummaryWeeks tbody").on("click", "a.btSummaryWeeksBAUKDONE", function (e) {
                       
            e.preventDefault();
            var col = e.currentTarget.attributes.class.nodeValue.split(" ")[1];
            var table = $("#tblSummaryWeeks").DataTable();
            var data = table.row($(this).parents("tr")).data();
           
            if (data != null) {
               
                Table.Details(null, _paramCol, data.EstimasiRTI, _paramRow, "BAUK DONE");
            }
        });
        $("#tblSummaryWeeks tbody").on("click", "a.btSummaryWeeksBAPSPRODUCTION", function (e) {
                    
            e.preventDefault();
            var col = e.currentTarget.attributes.class.nodeValue.split(" ")[1];
            var table = $("#tblSummaryWeeks").DataTable();
            var data = table.row($(this).parents("tr")).data();
          
            if (data != null) {
               
                Table.Details(null, _paramCol, data.EstimasiRTI, _paramRow, "BAPS PRODUCTION");
            }
        });
        $("#tblSummaryWeeks tbody").on("click", "a.btSummaryWeeksRTI", function (e) {
                        
            e.preventDefault();
            var col = e.currentTarget.attributes.class.nodeValue.split(" ")[1];
            var table = $("#tblSummaryWeeks").DataTable();
            var data = table.row($(this).parents("tr")).data();
           
            if (data != null) {
                
                Table.Details(null, _paramCol, data.EstimasiRTI, _paramRow, "RTI");
            }
        });

        $("#tblSummaryWeeks tbody").on("click", "a.btSummaryWeeksTotal", function (e) {
                      
            e.preventDefault();
            var col = e.currentTarget.attributes.class.nodeValue.split(" ")[1];
            var table = $("#tblSummaryWeeks").DataTable();
            var data = table.row($(this).parents("tr")).data();
           
            if (data != null) {
              
                Table.Details(null, _paramCol, data.EstimasiRTI, _paramRow, "WeekTotal");
            }
        });



    },
    Details: function (ptype, RFIDateMonth, RFIDateWeek, opt, step) {
        App.blockUI({ target: ".portlet-Main", animate: !0 });
        $('.panelSearchChart').fadeOut();
        $('.panelSearchResultMonth').fadeOut();
        $('.panelSearchResultWeek').fadeOut();
       
        $('.pnlDetail').fadeIn(1000);
        //var l = Ladda.create(document.querySelector("#btSearch"))
        //l.start();

        var params = {
            Type: ptype,
            STIPCategory: $("#slSearchSTIP").val(),
            RFIDateYear: $("#slSearchYear").val(),
            RFIDateMonth: RFIDateMonth,
            RFIDateWeek: RFIDateWeek,
            CustomerID: opt,
            Step: step,
            SoNumber: $("#slSearchDetailSonum").val(),
            SiteID: $("#slSearchDetailSiteID").val(),
            SiteName: $("#slSearchDetailSiteName").val()
        }
        _ParamExport = {
            Type: ptype,
            STIPCategory: $("#slSearchSTIP").val(),
            RFIDateYear: $("#slSearchYear").val(),
            RFIDateMonth: RFIDateMonth,
            RFIDateWeek: RFIDateWeek,
            CustomerID: opt,
            Step: step
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
                "url": "/api/DashboardPotentialRFITo1stBAPS/GetDetail",
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
                    "data": "ID",
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
                { data: "po_number" },
                { data: "MLANumber" },
                {
                    data: "StartLeaseDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndLeaseDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "BapsType" },
                { data: "CustomerInvoice" },
                { data: "CompanyInvoiceName" },
                { data: "CompanyID" },
                { data: "Currency" },

                    
                {
                    data: "InvoiceStartDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "InvoiceEndDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },

                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "DeductionAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AmountTotal" }                               
                
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
    ExportSummary: function (table, name, filename, type) {
        var uri = 'data:application/vnd.ms-excel;base64,'
            , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><meta charset="utf-8"/><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
            , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
            , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
        var ctx = {}
        if (type == "week") {
            if (!table.nodeType) {
                var head = document.getElementById("tblHeadSummaryWeeks")
                table = document.getElementById(table)
                ctx = { worksheet: name || 'Worksheet', table: head.innerHTML + table.innerHTML }
            }

        }
        else {
            if (!table.nodeType) table = document.getElementById(table)
            ctx = { worksheet: name || 'Worksheet', table: table.innerHTML }
        }           
        document.getElementById("dlink").href = uri + base64(format(template, ctx));
        document.getElementById("dlink").download = filename;
        document.getElementById("dlink").click();
    },
    ExportDetail: function () {
        //ptype, RFIDateMonth, RFIDateWeek, opt, step
        //Type: ptype,
        //STIPCategory: $("#slSearchSTIP").val(),
        //RFIDateYear: $("#slSearchYear").val(),
        //RFIDateMonth: RFIDateMonth,
        //RFIDateWeek: RFIDateWeek,
        //CustomerID: opt,
        //Step: step
        var Type = _ParamExport.Type;
        var STIPCategory = _ParamExport.STIPCategory;
        var RFIDateYear = _ParamExport.RFIDateYear;
        var RFIDateMonth = _ParamExport.RFIDateMonth;
        var week = _ParamExport.RFIDateWeek;
        var step = _ParamExport.Step;
        var CustomerID = _ParamExport.CustomerID;

        var SoNumber= $("#slSearchDetailSonum").val();
        var SiteID= $("#slSearchDetailSiteID").val();
        var SiteName= $("#slSearchDetailSiteName").val();

        window.location.href = "/DashboardRA/ExportDetailPotentialRFI?strType=" + Type + "&strSTIPCategory=" + STIPCategory + "&strRFIDateYear=" + RFIDateYear
            + "&RFIDateMonth=" + RFIDateMonth + "&CustomerID=" + CustomerID + "&Week=" + week + "&Step=" + step + "&SoNumber=" + SoNumber + "&SiteID=" + SiteID + "&SiteName=" + SiteName
        
    },
    
}

var Control = {
    BindingSummary: function () {
        Control.ResetBindingSummary();
            $.ajax({
                url: "/api/DashboardPotentialRFITo1stBAPS/GetSummary",                 
                type: "GET",
                data: {
                    Type: "Summary",
                    STIP: $("#slSearchSTIP").val(),
                    year: $("#slSearchYear").val(),
                    month: null,
                    desc: null

                }
            })
                .done(function (data, textStatus, jqXHR) {      
                $.each(data.dataSummary, function (i, item) {

                    if (item.GroupName == "INPUT-STIP")
                    {
                        if (item.DataGroupDescription == "PO Process")
                        {
                            $("#ValueA1").text(item.Total);
                        }
                        

                        if (item.DataGroupDescription == "PO Done")
                        {
                            $("#ValueA2").text(item.Total);
                        }
                        

                        _TotalSumSTIP += item.Total
                        $("#ValueA").text(_TotalSumSTIP)
                    }
                    else if (item.GroupName == "TAHAPAN-PROJECT")
                    {
                        if (item.DataGroupDescription == "SITAC")
                        {
                            $("#ValueB1").text(item.Total);
                        }
                        
                        

                        if (item.DataGroupDescription == "CME")
                        {
                            $("#ValueB2").text(item.Total);
                        }
                        

                        if (item.DataGroupDescription == "RFI")
                        {
                            $("#ValueB3").text(item.Total);
                        }
                       

                        if (item.DataGroupDescription == "BAUK SUBMIT")
                        {
                            $("#ValueB4").text(item.Total);
                        }
                        

                        _TotalSumTP += item.Total
                        $("#ValueB").text(_TotalSumTP)
                    }
                    else if (item.GroupName == "BAPS-BILLING")
                    {
                        if (item.DataGroupDescription == "BAUK REVIEW")
                        {
                            $("#ValueC1").text(item.Total);
                        }
                        

                        if (item.DataGroupDescription == "BAUK DONE")
                        {
                            $("#ValueC2").text(item.Total);
                        }
                        

                        if (item.DataGroupDescription == "BAPS PRODUCTION")
                        {
                            $("#ValueC3").text(item.Total);
                        }
                       
                        if (item.DataGroupDescription == "BAPS VALIDATION")
                        {
                            $("#ValueC4").text(item.Total);
                        }
                        

                        if (item.DataGroupDescription == "RTI")
                        {
                            $("#ValueC5").text(item.Total);
                        }
                        


                        if (item.DataGroupDescription == "CASH IN First BAPS BILLING")
                        {
                            $("#ValueC6").text(item.Total);
                        }
                        

                        _TotalSumBAPS += item.Total
                        $("#ValueC").text(_TotalSumBAPS)
                    }

            });               
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            //App.unblockUI(".portlet-body");
            Common.Alert.Error(errorThrown);
        });
    },
    ResetBindingSummary: function () {
        _TotalSumSTIP = 0;
        _TotalSumTP = 0;
        _TotalSumBAPS = 0;

        $("#ValueA1").text(0);
        $("#ValueA2").text(0);
        $("#ValueB1").text(0);
        $("#ValueB2").text(0);
        $("#ValueB3").text(0);
        $("#ValueB4").text(0);
        $("#ValueC1").text(0);
        $("#ValueC2").text(0);
        $("#ValueC3").text(0);
        $("#ValueC4").text(0);
        $("#ValueC5").text(0);
        $("#ValueC6").text(0);

    },
    BindingSelectYear: function (id) {
        $.ajax({
            url: "/api/DashboardPotentialRFITo1stBAPS/Year",
            type: "GET"
        })
            .done(function (data, textstatus, jqxhr) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.Description + "'>" + item.Description + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Section", width: null });
            })
            .fail(function (jqxhr, textstatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectSTIP: function (id) {
        $.ajax({
            url: "/api/DashboardPotentialRFITo1stBAPS/STIP",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.Description + "'>" + item.Description + "</option>");
                    })
                }

                $(id).select2({ placeholder: "Select STIP", width: null });

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    }
}


