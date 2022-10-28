Data = {};

var sonumbDetail, KategoryRevenueDetail;

jQuery(document).ready(function () {
    $('.menu-toggler').click();
    $('.collapse').click();
    var sonumb = getUrlVars()["sonumb"];
    var KategoryRevenue = getUrlVars()["KategoryRevenue"];
    var Periode = getUrlVars()["Periode"];

    var month = Periode.slice(-2)
    var year = Periode.substring(0, 4);

    var param = {
        month: month,
        year: year,
        sonumb: sonumb,
        KategoryRevenue: KategoryRevenue
    };

    sonumbDetail = sonumb;
    KategoryRevenueDetail = KategoryRevenue;

    Control.LoadInformation(param);

    Form.searchDetail(Data);

    $("#lkExportDetail").unbind().click(function () {
        Form.ExportDetail();
    });

    $('#tblDetail').on('click', '.btInvoiceCount', function (e) {
        var table = $('#tblDetail').DataTable();
        var tr = $(this).closest('tr');
        var data = table.row(tr).data();
        console.log(data);
        var countInv = data.InvoiceCount;
        var dateInv;
        if (data.StartDueDate != "")
            dateInv = data.StartDueDate;
        else
            dateInv = data.StartDueDateSLD;

        dateInv = new Date(dateInv);
        var year = pad(dateInv.getFullYear());
        var month = pad(dateInv.getMonth() + 1);
        dateInv = year + month;

        var sonumb = $("#lblRevSysDetailSonumb").text().toString().trim()
        if (countInv > 0) {
            window.open("DetailInvoice" + "?sonumb=" + sonumb + "&invoiceDate=" + dateInv, "PopupWindow", "toolbar=no,menubar=no,resizable=no");
        }
    });


});

var Form = {
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
    Detail: function (data) {
        $('#lblRevSysDetailSonumb').text(data[0].sonumb).toString().trim();
        $('#lblRevSysDetailSiteID').text(data[0].SiteID).toString().trim();
        $('#lblRevSysDetailSiteName').text(data[0].SiteName).toString().trim();
        $('#lblRevSysDetailOperator').text(data[0].OperatorId).toString().trim();
        $('#lblRevSysDetailTenantType').text(data[0].TenantType).toString().trim();
        $('#lblRevSysDetailCompanyAsset').text(data[0].company).toString().trim();
        $('#lblRevSysDetailPeriode').text(Common.Format.ConvertJSONDateTime(data[0].asatdate)).toString().trim();

        $('#lblRevSysDetailRentalStart').text(data[0].RentalStart).toString().trim();
        $('#lblRevSysDetailRentalEnd').text(data[0].RentalEnd).toString().trim();
        $('#lblRevSysDetailRFIDate').text(data[0].RFIDate).toString().trim();
        $('#lblRevSysDetailSLDDate').text(data[0].SLDDate).toString().trim();
        $('#lblRevSysDetailBAPSDate').text(data[0].BAPSDate).toString().trim();
        $('#lblRevSysDetailRegional').text(data[0].RegionalName).toString().trim();

        $('#lblRevSysDetailTotalAmortsiteBAPS').text(Common.Format.CommaSeparation(data[0].TotalAmountAmortSite)).toString().trim();
        $('#lblRevSysDetailTotalAccrue').text("").toString().trim();
        $('#lblRevSysDetailAdjusmentSLD').text(Common.Format.CommaSeparation(data[0].AdjAccrue)).toString().trim();
        $('#lblRevSysDetailTotalInvoice').text(Common.Format.CommaSeparation(data[0].AmountInv)).toString().trim();
        if (data.BalanceAccured > 0) {
            $('#lblRevSysDetailTotalBalance').text(Common.Format.CommaSeparation(data[0].BalanceAccured)).toString().trim();
        }
        else {
            $('#lblRevSysDetailTotalBalance').text(Common.Format.CommaSeparation(data[0].BalanceDI)).toString().trim();
        }
        $('#lblRevSysDetailKategoryRevenue').text(data[0].KategoriR).toString().trim();
        //var getPeriode = helper.convertDate($("#slMonthYear").val().toString().trim());
        //$('#lblPeriodeMaxAsatDate').text("Periode : " + getPeriode).toString().trim();



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

        var param = {
            sonumb: sonumbDetail,
            KategoryRevenue: KategoryRevenueDetail
        }

        Form.drawDetail(selector, columnsDetail, data, param);
    },
    drawDetail: function (selector, columnsDetail, data, param) {



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
            //"autoWidth": true,
            //"scrollY": 450,
            //"scrollX": true,
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
                            sonumb: sonumbDetail,
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
    }

}

var Control = {
    LoadInformation: function (param) {
        $.ajax({
            url: "/api/RevenueSystem/GetRevSysDetailSonumb",
            type: "POST",
            datatype: "json",
            data: param
        })
        .done(function (data, textStatus, jqXHR) {
            Form.Detail(data);
        });
    }
};

var helper = {
    RenderLink: function (val) {
        return "<a class='btInvoiceCount'>" + val + "</a>";

    },
    convertToRupiah: function convertToRupiah(angka) {
        var rupiah = '';
        var angkarev = angka.toString().split('').reverse().join('');
        for (var i = 0; i < angkarev.length; i++) if (i % 3 == 0) rupiah += angkarev.substr(i, 3) + '.';
        return rupiah.split('', rupiah.length - 1).reverse().join('');
    }
};


function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

function pad(numb) {
    return (numb < 10 ? '0' : '') + numb;
}
