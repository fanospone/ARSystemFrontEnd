$(function () {
    Page.Init();
    TableSalesSystem.Init();
    TableBAPS.Init();
    TableInvoice.Init();
});

var Page = {
    Init: function () {
        Control.Init();
        Page.Load();
    },
    Load: function () {
        Service.GetTrxInvoiceInformationBySonumb(ViewState.SONumber, function (t) {
            console.log(t);
            $('#' + Control.ID.txtSONumber).val(t.trxISP.SONumber);
            $('#' + Control.ID.txtSiteID).val(t.trxISP.SiteID);
            $('#' + Control.ID.txtSiteName).val(t.trxISP.SiteName);
            $('#' + Control.ID.txtSiteIDOpr).val(t.trxISP.CustomerSiteID);
            $('#' + Control.ID.txtSiteNameOpr).val(t.trxISP.CustomerSiteName);
            $('#' + Control.ID.txtCompany).val(t.trxISP.Company);
            $('#' + Control.ID.txtCustomer).val(t.trxISP.CompanyID); 
            $('#' + Control.ID.txtDate).val(t.trxISP.RFIDate);
            $('#' + Control.ID.txtStatus).val(t.trxISP.Status);
        })
    }
}

var Control = {
    Init: function () {
        //$('#' + Control.ID.txtDate).datepicker({ format: "dd-M-yyyy" });
        $('#' + Control.ID.btnBack).on('click', function () {
            window.history.back();
        });
    },
    ID: {
        txtSONumber: "txtSONumber",
        txtSiteID: "txtSiteID",
        txtSiteName: "txtSiteName",
        txtSiteIDOpr: "txtSiteIDOpr",
        txtSiteNameOpr: "txtSiteNameOpr",
        txtCompany: "txtCompany",
        txtCustomer: "txtCustomer",
        txtDate: "txtDate",
        txtStatus:"txtStatus",
        btnBack: "btnBack",
        
    }
}

var TableSalesSystem = {
    Init: function () {
        var tblSalesSystemInformation = $("#tblSalesSystemInformation").DataTable({
            "language": {
                "emptyTable": "No data available in table",
            },
            "filter": true,
            "paging": false,
            "order": [[2, 'desc']],
            "destroy": true,
            "data": [],
            "columns": [
                         { data: "STIPSiro" },
                         { data: "STIPNumber" },
                         { data: "STIPCode" },
                          {
                              data: 'StartLeasePeriod',
                              render: function (data) {
                                  return Common.Format.ConvertJSONDateTime(data);
                              }
                          },
                            {
                                data: 'EndLeasePeriod',
                                render: function (data) {
                                    return Common.Format.ConvertJSONDateTime(data);
                                }
                            },
                         { data: "OMPrice" },
                         { data: "PriceAmount" },
            ],
            "columnDefs": [
               { "targets": [0], "width": "5%", "className": "dt-center", "orderable": false },
               { "targets": [1, 2, 3, 4, 5], "width": "17%", "className": "dt-center" },
               { "targets": [6], "width": "10%", "className": "dt-center" }
            ],
        });
        TableSalesSystem.Search();
        $("#tabDetails").tabs();
    },
    Search: function () {
        var params = {
            SONumber: $("#hdSONumber").val()
        };

       // App.blockUI({ target: "#tbKitchenProcess", animate: !0 });
        $.ajax({
            url: "/api/ISPInvoiceInformation/GetSalesSystemInformation/",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            App.unblockUI("#tblSalesSystemInformation");
            if (Common.CheckError.List(data)) {
                var table = $("#tblSalesSystemInformation").DataTable();
                table.clear().rows.add(data).draw();
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(jqXHR.responseJSON.Message);;
            App.unblockUI("#tblSalesSystemInformation");
        });
    }
}

var TableBAPS = {
    Init: function () {
        var tblBAPSNewInformation = $("#tblBAPSNewInformation").DataTable({
            "language": {
                "emptyTable": " No data available in table",
            },
            "filter": true,
            "paging": false,
            "order": [[2, 'desc']],
            "destroy": true,
            "data": [],
            "columns": [
                 { data: "BAPSNumber"},
                 {
                     data: 'BapsDate',
                     render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                 {
                     data: 'EffectiveBapsDate',
                     render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                  {
                      data: 'StartEffectiveDate',
                      render: function (data) {
                          return Common.Format.ConvertJSONDateTime(data);
                      }
                  },
                  {
                      data: 'EndEffectiveDate',
                      render: function (data) {
                          return Common.Format.ConvertJSONDateTime(data);
                      }
                  },
                { data: "BaseLeasePrice" },
                { data: "ServicePrice" },
                { data: "StipSiro" },
            ],
            "columnDefs": [
                 { "targets": [0], "width": "5%", "className": "dt-center", "orderable": false },
                 { "targets": [1, 2, 3, 4, 5 ], "width": "17%", "className": "dt-center" },
                 { "targets": [6], "width": "10%", "className": "dt-center" }
            ],
        })
        TableBAPS.Search();
    },
    Search: function () {
        var params = {
            SONumber: $("#hdSONumber").val()
        };

        // App.blockUI({ target: "#tbKitchenProcess", animate: !0 });
        $.ajax({
            url: "/api/ISPInvoiceInformation/GetBAPSNewInformation/",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            App.unblockUI("#tblBAPSNewInformation");
            if (Common.CheckError.List(data)) {
                var table = $("#tblBAPSNewInformation").DataTable();
                table.clear().rows.add(data).draw();
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(jqXHR.responseJSON.Message);;
            App.unblockUI("#tblBAPSNewInformation");
        });
    }
   
}

var TableInvoice = {
    Init: function () {
        var tblInvoiceInformation = $("#tblInvoiceInformation").DataTable({
            "deferRender": true,
            "processing": true,
            "language": {
                "emptyTable": " No data available in table",
            },
            "filter": false,
            "paging": false,
            "order": [[4, 'asc']],
            "destroy": true,
            "filter" : true,
            "data": [],
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                     {
                         extend: 'excelHtml5',
                         filename: 'InvoiceInformationDetail',
                         text: '<i class="fa fa-file-excel-o"></i>',
                         titleAttr: 'Export to Excel',
                         className: 'btn yellow btn-outline',
                         exportOptions: {
                         },
                         rows: ':visible'
                     },

                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "columns": [
                { data: "SONumber" },
                { data: "CustomerSiteID" },
                { data:"CustomerSiteName"},
                { data: "TenantType" },
                { data: "RFIDateOpr"},
                { data: "StipSiro"},
                 {
                     data: 'StartInvoice',
                     render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                 {
                     data: 'EndInvoice',
                     render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                { data: "RAACtivityName" },
                { data: "PoNumber" },
                { data: "DocPO" },
                { data: "BapsNo" },
                 {
                     data: 'BAPSConfirmDate',
                     render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                { data: "DocBAPS" },
                { data: "BasicPrice" },
                { data: "ServicePrice" },
                { data: "AmountInvoice" },
                { data: "NoInvoice" },
                 {
                     data: 'CreateDateInvoice',
                     render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                  {
                      data: 'PostingDate',
                      render: function (data) {
                          return Common.Format.ConvertJSONDateTime(data);
                      }
                  },
                { data: "PaidStatus" },
                {
                    data: 'PaidDate',
                    render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
            ],
            "order":[[6, "asc"]],
            "columnDefs": [
                 { "targets": [0], "width": "5%", "className": "dt-center", "orderable": false },
                 { "targets": [1, 2, 3, 4, 5], "width": "17%", "className": "dt-center" },
                 { "targets": [6], "width": "10%", "className": "dt-center" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
        })
        TableInvoice.Search();
    },
    Search: function () {
        var params = {
            SONumber: $("#hdSONumber").val()
        };
        $.ajax({
            url: "/api/ISPInvoiceInformation/GetInvoiceInformationDetail/",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            App.unblockUI("#tblInvoiceInformation");
            if (Common.CheckError.List(data)) {
                var table = $("#tblInvoiceInformation").DataTable();
                table.clear().rows.add(data).draw();
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(jqXHR.responseJSON.Message);;
            App.unblockUI("#tblInvoiceInformation");
        });
    },
    Export: function (table, name, filename) {
        var uri = 'data:application/vnd.ms-excel;base64,'
            , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><meta charset="utf-8"/><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
             , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
        , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }

        if (!table.nodeType) table = $('#' + table).html()
                             .replace(/height: 0px;/g, "");;
        var ctx = { worksheet: name || 'Worksheet', table: table }

        document.getElementById("dlink").href = uri + base64(format(template, ctx));
        document.getElementById("dlink").download = filename;
        document.getElementById("dlink").click();
    },
}


const Url = {
    APIs: {
        GetTrxInvoiceInformationBySonumb: "/api/ISPInvoiceInformation/TrxISPInvoiceInformationBySonumb/"
    },
    Export: {
        ISPInvoiceInformationDetail: "ExporInvoiceInformationDetailList"
}
}

var Service = {
    GetTrxInvoiceInformationBySonumb: function (_soNumber, callback) {
        $.ajax({
            url:Url.APIs.GetTrxInvoiceInformationBySonumb + _soNumber,
            type: "GET",
            cache: false,
            async: false,
            beforeSend: function () {
                App.blockUI({ target: "#filterPnltrxInvoiceInformation", animate: !0 });
            }
        }).done(function (data, textStatus, jqXHR) {
            callback(data);
            App.unblockUI('#filterPnltrxInvoiceInformation');
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    }
}