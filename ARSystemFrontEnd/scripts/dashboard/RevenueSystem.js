Data = {};
var columnNameData = new Array();
columnNameData = [];

var columnValue = new Array();

jQuery(document).ready(function () {
    $('.menu-toggler').click();


    Control.LoadCompany();
    Control.LoadCompanyINV();
    Control.LoadOperatorRevSys();
    Control.LoadRegionRevSys();
    Control.LoadKategoryRevenueRevSys();
    Control.loadDateRevSys();
    Form.Init();
    Form.search();

    $("#btReset").unbind().click(function () {
        Form.reset();
        Form.bind();
    });

    $("#lkExportDetail").unbind().click(function () {
        Form.ExportDetail();
    });


    $("#btSearch").unbind().click(function () {
        Form.bind();
    });


    $('#tblDetail').on('click', '.btInvoiceCount', function (e) {
        var table = $('#tblDetail').DataTable();
        var tr = $(this).closest('tr');
        var data = table.row(tr).data();
        console.log(data);
        var countInv = data.InvoiceCount;

        var dateInv = new Date(data.StartDueDate);
        var year = pad(dateInv.getFullYear());
        var month = pad(dateInv.getMonth() + 1);
        dateInv = year + month;

        var sonumb = $("#lblRevSysDetailSonumb").text().toString().trim()
        if (countInv > 0) {
            window.open("DetailInvoice" + "?sonumb=" + sonumb + "&invoiceDate=" + dateInv, "PopupWindow", "toolbar=no,menubar=no,resizable=no");
        }
    });

    $("#tblSummaryData").on("click", ".btDetail", function (e) {
        var table = $("#tblSummaryData").DataTable();
        var data = table.row($(this).parents("tr")).data();
        if (data != null) {
            Data.Selected = data;

            $('#tblDetail').dataTable().fnClearTable();
            var table = $('#tblSummaryData').DataTable();
            var tr = $(this).closest('tr');
            var data = table.row(tr).data();

            console.log(data);

            var sonumb = data.sonumb;
            var asatDate = new Date(data.asatdate);
            var year = pad(asatDate.getFullYear());
            var month = pad(asatDate.getMonth() + 1);
            asatDate = year + month;
            var kategoryR = data.KategoriR;

            window.open("AccrueRevenueDetail" + "?sonumb=" + sonumb + "&KategoryRevenue=" + kategoryR + "&Periode=" + asatDate, "toolbar=no,menubar=no,resizable=no");
        }
    });


    $('#tblSummaryData .filters th').each(function () {
        //$('#tblSummaryData tfoot th').each(function () {
        var title = $('#tblSummaryData thead tr th').eq($(this).index()).text();
        title = title.replace(/ /g, "_");
        title = title.replace("(", "");
        title = title.replace(")", "");
        if (title != "") {
            columnNameData.push(title);
            $(this).html('<input type="text" style="Font-size:8px; width:100%;" onkeyup="lookup(this);" id=' + title + '  placeholder="Search ' + title.replace('_', ' ') + '" />');
        }
    });
});

function pad(numb) {
    return (numb < 10 ? '0' : '') + numb;
}


var timer;
function lookup(arg) {
    var id = arg.getAttribute('id');
    var value = arg.value;
    clearTimeout(timer);
    columnValue = [];
    timer = setTimeout(function (event) {
        if (columnNameData != null) {
            for (var i = 0; i < columnNameData.length; i++) {
                var dt = columnNameData[i].toString().trim();

                if (dt != undefined && dt != "") {
                    dt = "#" + dt;
                    columnValue.push($(dt).val().toString().trim());
                }
            }
        }

        columns =
            [
                    {
                        orderable: false, mRender: function (data, type, full) {
                            var strReturn = "";
                            strReturn += "<button type='button' title='Detail' class='btn btn-xs green btDetail' ><i class='fa fa-edit'></i></button>";
                            return strReturn;
                        }
                    },
                    { data: "asatdate" },
                    { data: "sonumb" },
                    { data: "KategoriR" },
                    { data: "SiteID" },
                    { data: "SiteName" },
                    { data: "RegionalName" },
                    { data: "Province" },
                    { data: "UserNumber" },
                    { data: "OperatorId" },
                    { data: "status" },
                    { data: "DismantleDate" },
                    { data: "statusAccrue" },
                    { data: "company" },
                    { data: "CompanyInv" },
                    { data: "RFIDate" },
                    { data: "SLDDate" },
                    { data: "BAPSDate" },
                    { data: "RentalStart" },
                    { data: "RentalEnd" },
                    { data: "TenantType" },
                    { data: "TenancyWeight" },
                    { data: "TechType" },
                    { data: "RFCurr" },
                    { data: "MFCurr" },
                    { data: "RentalAmount", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "ServiceFeeAmount", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "Serviceandinflation", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "Overblast", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "Overdaya", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "NormalRev", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "RentalAmountInv", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "OMAmountInv", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "InflationInv", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "OverblastInv", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "OverdayaInv", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "PenaltyInv", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "DiscountPaidInv", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "AmountInv", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "NetRevenue", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "AdjAccrue", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "adjDismantle", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "AdjUpdateHarga", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "AdjOther", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "SharingRevenue", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "AdjProRate", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "TotalAdjustment", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "BalanceAccured", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "BalanceDI", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "TotalAmountAmortSite", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                    { data: "SonumbReloc" }
            ]

        if (columnValue.length > 0)
            Form.drawSearch("#tblSummaryData", columns, columnNameData, columnValue);
        else
            Form.draw("#tblSummaryData", columns);

    }, 500);
}


var Form = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();

        var tblSummaryData = $('#tblSummaryData').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        var comAsset = $("#slCompanyAsset").val().toString().trim()
        var comInv = $("#slCompanyInvoice").val().toString().trim()
        var operatorid = $("#slOperator").val().toString().trim()
        var region = $("#slRegion").val().toString().trim()
        var KategoryRevenue = $("#slKategoryRevenue").val().toString().trim()
        var MonthYear = $("#slMonthYear").val().toString().trim()

        Control.setAmountHeader(comAsset, comInv, operatorid, region, KategoryRevenue, MonthYear);
    },
    ExportDetail: function () {
        var comSonumb = $("#lblRevSysDetailSonumb").text().toString().trim();
        var comKategoryRevenue = $("#lblRevSysDetailKategoryRevenue").text().toString().trim();
        var comRentalStart = $("#lblRevSysDetailRentalStart").text().toString().trim();
        var comTotalAmortsiteBAPS = $("#lblRevSysDetailTotalAmortsiteBAPS").text().toString().trim();
        var comSiteID = $("#lblRevSysDetailSiteID").text().toString().trim();
        var comRentalEnd = $("#lblRevSysDetailRentalEnd").text().toString().trim();
        var comTotalAccrue = $("#lblRevSysDetailTotalAccrue").text().toString().trim();
        var comSiteName = $("#lblRevSysDetailSiteName").text().toString().trim();
        var comRFIDate = $("#lblRevSysDetailRFIDate").text().toString().trim();
        var comAdjusmentSLD = $("#lblRevSysDetailAdjusmentSLD").text().toString().trim();
        var comOperator = $("#lblRevSysDetailOperator").text().toString().trim();
        var comSLDDate = $("#lblRevSysDetailSLDDate").text().toString().trim();
        var comTotalInvoice = $("#lblRevSysDetailTotalInvoice").text().toString().trim();
        var comTenantType = $("#lblRevSysDetailTenantType").text().toString().trim();
        var comBAPSDate = $("#lblRevSysDetailBAPSDate").text().toString().trim();
        var comTotalBalance = $("#lblRevSysDetailTotalBalance").text().toString().trim();
        var comCompanyAsset = $("#lblRevSysDetailCompanyAsset").text().toString().trim();
        var comRegional = $("#lblRevSysDetailRegional").text().toString().trim();
        var comPeriode = $("#lblRevSysDetailPeriode").text().toString().trim();

        window.location.href = "/RevenueSystem/RevSysDetail/ExportDetail?comSonumb=" + comSonumb + "&comKategoryRevenue=" + comKategoryRevenue + "&comRentalStart=" + comRentalStart + "&comTotalAmortsiteBAPS=" + comTotalAmortsiteBAPS + "&comSiteID=" + comSiteID + "&comRentalEnd=" + comRentalEnd + "&comTotalAccrue=" + comTotalAccrue + "&comSiteName=" + comSiteName + "&comRFIDate=" + comRFIDate + "&comAdjusmentSLD=" + comAdjusmentSLD + "&comOperator=" + comOperator + "&comSLDDate=" + comSLDDate + "&comTotalInvoice=" + comTotalInvoice + "&comTenantType=" + comTenantType + "&comBAPSDate=" + comBAPSDate + "&comTotalBalance=" + comTotalBalance + "&comCompanyAsset=" + comCompanyAsset + "&comRegional=" + comRegional + "&comPeriode=" + comPeriode;
    },
    Export: function (param) {
        var comAsset = param.comAsset;
        var comInv = param.comInv;
        var OperatorID = param.OperatorID;
        var region = param.region;
        var KategoryRevenue = param.KategoryRevenue;
        var MonthYear = param.MonthYear;
        var endIdx = param.MonthYear;

        window.location.href = "/RevenueSystem/RevSysHeader/Export?comAsset=" + comAsset + "&comInv=" + comInv + "&OperatorID=" + OperatorID + "&region=" + region + "&KategoryRevenue=" + KategoryRevenue + "&MonthYear=" + MonthYear;
    },


    bind: function () {
        Form.search();

        var comAsset = $("#slCompanyAsset").val().toString().trim()
        var comInv = $("#slCompanyInvoice").val().toString().trim()
        var operatorid = $("#slOperator").val().toString().trim()
        var region = $("#slRegion").val().toString().trim()
        var KategoryRevenue = $("#slKategoryRevenue").val().toString().trim()
        var MonthYear = $("#slMonthYear").val().toString().trim()
        Control.setAmountHeader(comAsset, comInv, operatorid, region, KategoryRevenue, MonthYear);
    },
    reset: function () {
        $("#slCompanyAsset").val($('#slCompanyAsset option:first-child').val()).trigger("change");
        $("#slCompanyInvoice").val($('#slCompanyInvoice option:first-child').val()).trigger("change");
        $("#slRegion").val($('#slRegion option:first-child').val()).trigger("change");
        $("#slOperator").val($('#slOperator option:first-child').val()).trigger("change");
        $("#slKategoryRevenue").val($('#slKategoryRevenue option:first-child').val()).trigger("change");
        Control.loadDateRevSys();
    },

    setModalDetail: function (data) {
        $('#mdlDetail').modal('toggle');
        $('#lblRevSysDetailSonumb').text(data.sonumb).toString().trim();
        $('#lblRevSysDetailSiteID').text(data.SiteID).toString().trim();
        $('#lblRevSysDetailSiteName').text(data.SiteName).toString().trim();
        $('#lblRevSysDetailOperator').text(data.OperatorId).toString().trim();
        $('#lblRevSysDetailTenantType').text(data.TenantType).toString().trim();
        $('#lblRevSysDetailCompanyAsset').text(data.company).toString().trim();
        $('#lblRevSysDetailPeriode').text(Common.Format.ConvertJSONDateTime(data.asatdate)).toString().trim();

        $('#lblRevSysDetailRentalStart').text(data.RentalStart).toString().trim();
        $('#lblRevSysDetailRentalEnd').text(data.RentalEnd).toString().trim();
        $('#lblRevSysDetailRFIDate').text(data.RFIDate).toString().trim();
        $('#lblRevSysDetailSLDDate').text(data.SLDDate).toString().trim();
        $('#lblRevSysDetailBAPSDate').text(data.BAPSDate).toString().trim();
        $('#lblRevSysDetailRegional').text(data.RegionalName).toString().trim();

        $('#lblRevSysDetailTotalAmortsiteBAPS').text(Common.Format.CommaSeparation(data.TotalAmountAmortSite)).toString().trim();
        $('#lblRevSysDetailTotalAccrue').text("").toString().trim();
        $('#lblRevSysDetailAdjusmentSLD').text(Common.Format.CommaSeparation(data.AdjAccrue)).toString().trim();
        $('#lblRevSysDetailTotalInvoice').text(Common.Format.CommaSeparation(data.AmountInv)).toString().trim();
        if (data.BalanceAccured > 0) {
            $('#lblRevSysDetailTotalBalance').text(Common.Format.CommaSeparation(data.BalanceAccured)).toString().trim();
        }
        else {
            $('#lblRevSysDetailTotalBalance').text(Common.Format.CommaSeparation(data.BalanceDI)).toString().trim();
        }
        $('#lblRevSysDetailKategoryRevenue').text(data.KategoriR).toString().trim();
        var getPeriode = helper.convertDate($("#slMonthYear").val().toString().trim());
        $('#lblPeriodeMaxAsatDate').text("Periode : " + getPeriode).toString().trim();
    },

    searchDetail: function (data) {
        var columnsDetail = [];
        var selector = "#tblDetail";

        columnsDetail = [
                   { data: "DescInv", sClass: "Chidden" },
                   { data: "StartDueDateSLD", sClass: "param-StartPeriod-SLD" },
                   { data: "EndDueDateSLD", className: "text-center" },
                   { data: "AmountSLD", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                   { data: "StartDueDate", sClass: "param-StartPeriod-BAPS" },
                   { data: "EndDueDate", className: "text-center" },
                   { data: "InvoiceCount", sClass: "tooltip-card-name", render: function (val, type, full) { return helper.RenderLink(val) } },
                   { data: "AmountTotalInvoice", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                   { data: "AmountAmortSite", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                   { data: "AmountAccrue", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                   { data: "AmountInflasi", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                   { data: "AmountInvoiced", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                   { data: "InvoicedInflasi", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                   { data: "AdjUpdateHarga", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                   { data: "AdjSLD", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                   { data: "AdjDismantle", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                   { data: "AdjCleansing", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                   { data: "Balance", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') }
        ]
        Form.drawDetail(selector, columnsDetail, data);
    },
    footercallback: function () {
        var api = $("#tblDetail").dataTable().api(), data;
        //var api = apsi.api();

        var intVal = function (i) {
            return typeof i === 'string' ?
                i.replace(/[\$,]/g, '') * 1 :
                typeof i === 'number' ?
                i : 0;
        };


        total = api
            .column(3)
            .data()
            .reduce(function (a, b) {
                return intVal(a) + intVal(b);
            }, 0);


        // Update footer
        $(api.column(3).footer()).html(
          total
        );

        //api.columns('.sum', { page: 'current' }).every(function () {
        //    var sum = this
        //      .data()
        //      .reduce(function (a, b) {
        //          return intVal(a) + intVal(b);
        //      }, 0);

        //    this.footer().innerHTML = sum;
        //});

    },
    drawDetail: function (selector, columnsDetail, data) {

        var param = {
            sonumb: data.sonumb,
            KategoryRevenue: data.KategoriR
        }

        var tblDetail = $("#tblDetail").DataTable({
            //dom: 'Bfrtip',
            "deferRender": true,
            "bInfo": false,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "infoFiltered": ""
            },
            "ajax": {
                "url": "/Api/RevenueSystem/GetRevSysDetail",
                "type": "POST",
                "datatype": "json",
                "data": param
            },
            //buttons: [
            //    { extend: 'excelHtml5', text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Excel' }
            //],
            "filter": false,
            "lengthMenu": false,
            "ordering": false,
            "lengthChange": false,
            "paging": false,
            "columns": columnsDetail,
            "aoColumnDefs": [{
                "targets": [0],
                "visible": false
            }],
            "fnDrawCallback": function (oSettings) {
                $('#tblDetail tbody tr ').each(function () {
                    var invNo, invTemp, invDate, startPeriod, endPeriode, AmountInv;
                    var dtA = this;
                    var nTds = $(this).find('td .btInvoiceCount');
                    var invoiceCount = parseInt(nTds[0].innerHTML);

                    if (invoiceCount >= 1) {
                        var PERIODE;
                        var paramPeriodeSLD = $(this).find('td.param-StartPeriod-SLD');
                        paramPeriodeSLD = paramPeriodeSLD[0].innerHTML;
                        var paramPeriodeBAPS = $(this).find('td.param-StartPeriod-BAPS');
                        paramPeriodeBAPS = paramPeriodeBAPS[0].innerHTML;

                        if (paramPeriodeSLD != "") {
                            PERIODE = paramPeriodeSLD;
                        }
                        else {
                            PERIODE = paramPeriodeBAPS;
                        }

                        var paramDescInv = {
                            sonumb: data.sonumb,
                            StartPeriod: PERIODE
                        }
                        var tooltip;
                        $.ajax({
                            url: "/api/RevenueSystem/GetRevSysDetailDescInv",
                            type: "POST",
                            async: false,
                            data: paramDescInv
                        })
                        .done(function (data, textStatus, jqXHR) {
                            if (Common.CheckError.List(data)) {
                                $.each(data, function (i, item) {
                                    if (item != null) {
                                        var length = data.length - 1;
                                        tooltip = "";
                                        for (var i = 0; i <= length; i++) {
                                            if (i < length) {
                                                tooltip = "No Invoice :" + data[i].InvoiceNo + " \n"
                                                tooltip = tooltip + "No Invoice Temp : " + data[i].InvoiceTemp + " \n"
                                                tooltip = tooltip + "Invoice Date : " + data[i].InvoiceDate + " \n"
                                                tooltip = tooltip + "Start Periode : " + data[i].StartPeriod + " \n"
                                                tooltip = tooltip + "End Periode : " + data[i].EndPeriod + " \n"
                                                tooltip = tooltip + "Total Amount Invoice : " + helper.convertToRupiah(data[i].AmountTotalInvoice)
                                                tooltip = tooltip + " \n"
                                                tooltip = tooltip + "==================================================================" + " \n"
                                            }
                                            else {
                                                tooltip = tooltip + " \n"
                                                tooltip = tooltip + "No Invoice :" + data[i].InvoiceNo + " \n"
                                                tooltip = tooltip + "No Invoice Temp : " + data[i].InvoiceTemp + " \n"
                                                tooltip = tooltip + "Invoice Date : " + data[i].InvoiceDate + " \n"
                                                tooltip = tooltip + "Start Periode : " + data[i].StartPeriod + " \n"
                                                tooltip = tooltip + "End Periode : " + data[i].EndPeriod + " \n"
                                                tooltip = tooltip + "Total Amount Invoice : " + helper.convertToRupiah(data[i].AmountTotalInvoice)

                                            }
                                        }

                                    }
                                });
                            }
                        });

                        nTds[0].setAttribute('title', tooltip);
                    }
                });
            },
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;
                var colNumber = [3, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17];

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
                }
            },
            //"sDom": '&lt;"top"flp&gt;&lt;"clear"&gt;"bottom"flp&gt;&lt;"clear"&gt;',
            "destroy": true,
        });
    },
    search: function () {
        var columns = [];
        var selector = "#tblheaderRevenueSystem";
        columns = [
                      {
                          orderable: false,
                          mRender: function (data, type, full) {
                              var ID = "ID";
                              var strReturn = "";
                              strReturn += "<button type='button' title='Detail' class='btn btn-xs green btDetail' ><i class='fa fa-edit'></i></button>";
                              return strReturn;
                          }
                      },
                      { data: "asatdate" },
                      { data: "sonumb" },
                      { data: "KategoriR" },
                      { data: "SiteID" },
                      { data: "SiteName" },
                      { data: "RegionalName" },
                      { data: "Province" },
                      { data: "UserNumber" },
                      { data: "OperatorId" },
                      { data: "status" },
                      { data: "DismantleDate" },
                      { data: "statusAccrue" },
                      { data: "company" },
                      { data: "CompanyInv" },
                      { data: "RFIDate" },
                      { data: "SLDDate" },
                      { data: "BAPSDate" },
                      { data: "RentalStart" },
                      { data: "RentalEnd" },
                      { data: "TenantType" },
                      { data: "TenancyWeight" },
                      { data: "TechType" },
                      { data: "RFCurr" },
                      { data: "MFCurr" },
                      { data: "RentalAmount", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "ServiceFeeAmount", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "Serviceandinflation", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "Overblast", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "Overdaya", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "NormalRev", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "RentalAmountInv", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "OMAmountInv", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "InflationInv", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "OverblastInv", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "OverdayaInv", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "PenaltyInv", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "DiscountPaidInv", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "AmountInv", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "NetRevenue", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "AdjAccrue", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "adjDismantle", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "AdjUpdateHarga", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "AdjOther", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "SharingRevenue", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "AdjProRate", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "TotalAdjustment", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "BalanceAccured", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "BalanceDI", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "TotalAmountAmortSite", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "SonumbReloc" }
        ]
        selector = "#tblheaderRevenueSystem";
        Form.draw(selector, columns);

    },
    drawSearch: function (selector, columns, columnNameData, columnValue) {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var paramsearch = {
            columnValue: columnValue,
            columns: columns,
            MonthYear: $("#slMonthYear").val().toString().trim(),
            KategoryRevenue: $("#slKategoryRevenue").val().toString().trim(),
            comAsset: $("#slCompanyAsset").val().toString().trim(),
            comInv: $("#slCompanyInvoice").val().toString().trim(),
            OperatorID: $("#slOperator").val().toString().trim(),
            region: $("#slRegion").val().toString().trim()
        }


        var tblSummaryData = $("#tblSummaryData").DataTable({
            dom: 'Bfrtip',
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,

            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/Api/RevenueSystem/GetRevSysSearchDataHeader",
                "type": "POST",
                "datatype": "json",
                "data": paramsearch,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Form.Export(param);
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }

            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "columns": columns,
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

            },
            "order": [],
            "destroy": true
        });


    },
    draw: function (selector, columns) {

        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var param = {
            comAsset: $("#slCompanyAsset").val().toString().trim(),
            comInv: $("#slCompanyInvoice").val().toString().trim(),
            OperatorID: $("#slOperator").val().toString().trim(),
            region: $("#slRegion").val().toString().trim(),
            KategoryRevenue: $("#slKategoryRevenue").val().toString().trim(),
            MonthYear: $("#slMonthYear").val().toString().trim()
        }

        var tblSummaryData = $("#tblSummaryData").DataTable({
            dom: 'Bfrtip',
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,

            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/Api/RevenueSystem/GetRevSysDataHeader",
                "type": "POST",
                "datatype": "json",
                "data": param,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Form.Export(param)
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }

            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "columns": columns,
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

            },
            "order": [],
            //"createdRow": function (row, data, index) {
            //    $('td', this).removeClass('hover');
            //},
            "destroy": true
        });

        $('.collapse').click();
    }
}

var Control = {
    LoadCompany: function () {
        $.ajax({
            url: "/api/RevenueSystem/GetRevSysCompany",
            type: "POST",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null) {

                        $("#slCompanyAsset").append("<option value='" + item.ValueId + "'>" + item.ValueDesc + "</option>");
                    }
                });
            }
            $("#slCompanyAsset").select2({ width: null });
        });
    },
    LoadCompanyINV: function () {
        $.ajax({
            url: "/api/RevenueSystem/GetCompanyInv",
            type: "POST",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null) {
                        $("#slCompanyInvoice").append("<option value='" + item.ValueId + "'>" + item.ValueDesc + "</option>");
                    }
                });
            }
            $("#slCompanyInvoice").select2({ width: null });
        });
    },
    LoadOperatorRevSys: function () {
        $.ajax({
            url: "/api/RevenueSystem/GetOperatorRevSys",
            type: "POST",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null) {
                        $("#slOperator").append("<option value='" + item.ValueId + "'>" + item.ValueDesc + "</option>");
                    }
                });
            }
            $("#slOperator").select2({ width: null });
        });
    },
    LoadRegionRevSys: function () {
        $.ajax({
            url: "/api/RevenueSystem/GetRegionRevSys",
            type: "POST",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null) {
                        $("#slRegion").append("<option value='" + item.ValueId + "'>" + item.ValueDesc + "</option>");
                    }
                });
            }
            $("#slRegion").select2({ width: null });
        });
    },
    LoadKategoryRevenueRevSys: function () {
        $.ajax({
            url: "/api/RevenueSystem/GetKategoryRevenueRevSys",
            type: "POST",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null) {
                        $("#slKategoryRevenue").append("<option value='" + item.ValueId + "'>" + item.ValueDesc + "</option>");
                    }
                });
            }
            $("#slKategoryRevenue").select2({ width: null });
        });
    },
    loadDateRevSys: function () {
        $.ajax({
            url: "/api/RevenueSystem/getMaxAsatDate",
            type: "POST",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null) {
                        $("#slMonthYear").attr('value', item.YearMAX.toString() + '-' + item.MonthMAX.toString());
                    }
                });
            }
        });
    },
    setAmountHeader: function (comAsset, comInv, operatorid, region, KategoryRevenue, MonthYear) {
        var params = {
            comAsset, comInv, operatorid, region, KategoryRevenue, MonthYear
        };
        $.ajax({
            url: "/api/RevenueSystem/GetRevSysAmountHeader",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $("#inormalRev").val(helper.convertToRupiah(data[0].NormalRevenue));
                $("#inetRev").val((helper.convertToRupiah(data[0].NetRevenue)));
                $("#iBalanceAccrueRev").val((helper.convertToRupiah(data[0].BalanceAccured)));
                $("#iBalanceDIRev").val((helper.convertToRupiah(data[0].BalanceDI)));
                $("#itotalAdjustmentRev").val((helper.convertToRupiah(data[0].TotalAdjustment)));
                $("#itotalsonumbRev").val(helper.convertToRupiah(data[0].sonumb));
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });

    }
}


var helper = {
    RenderLink: function (val, full, status) {
        var table = $('#tblDetail').DataTable();

        if (full.Operator == "TOTAL" && table.data().count() == 1) {
            return val;
        } else {

            return "<a class='btDetail' companyId='" + CompanyId + "' endDate='" + EndDate + "' operatorId='" + OperatorId + "' invoiceType='" + InvoiceType + "' amountType='" + AmountType + "' status='" + status + "'>" + val + "</a>";
        }
    },
    RenderLink: function (val) {
        return "<a class='btInvoiceCount'>" + val + "</a>";

    },
    convertDate: function (dateParam) {
        var year = dateParam.substring(0, 4);
        var month = parseInt(dateParam.slice(-2));
        var months = ['January', 'February', 'Maret', 'April', 'May', 'Juni', 'Juli', 'Agustus', 'September', 'Oktober', 'November', 'Desember'];

        var mm = months[month - 1];

        return mm + ' ' + year;
    },
    getDataTb: function (dtA) {
        var sdsd = dtA.$('td');
        var nNodes = dtA.fnGetData();

        return nNodes;
    },
    convertToRupiah: function convertToRupiah(angka) {
        var rupiah = '';
        var angkarev = angka.toString().split('').reverse().join('');
        for (var i = 0; i < angkarev.length; i++) if (i % 3 == 0) rupiah += angkarev.substr(i, 3) + '.';
        return rupiah.split('', rupiah.length - 1).reverse().join('');
    }
}