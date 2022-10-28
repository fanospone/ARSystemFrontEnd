Data = {};

jQuery(document).ready(function () {
    $('.menu-toggler').click();

    var sonumb = getUrlVars()["sonumb"];
    var InvoiceDate = getUrlVars()["invoiceDate"];

    var param = {
        sonumb: sonumb,
        InvoiceDate: InvoiceDate
    };

    Form.Bind(param);
});

var Form = {
    Bind: function (param) {
        var columns = [];
        var selector = "#tblSummaryData";

        columns = [
            { data: "Sonumb" },
            { data: "InvoiceDate" },
            { data: "StartPeriod" },
            { data: "EndPeriod" },
            { data: "InvoiceNo" },
            { data: "InvoiceTemp" },
            { data: "AmountTotalInvoice", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') }
        ];

        Form.draw(selector, columns, param);
    },

    draw: function (selector, columns,param) {
        var tblDetail = $("#tblSummaryData").DataTable({
            dom: 'Bfrtip',
            "bInfo": false,
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "infoFiltered": ""
            },
            "ajax": {
                "url": "/Api/RevenueSystem/GetRevSysDetailInvoice",
                "type": "POST",
                "datatype": "json",
                "data": param
            },
            buttons: [
                 { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' }
                 //{
                 //    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                 //        var l = Ladda.create(document.querySelector(".yellow"));
                 //    }
                 //},
                 //{ extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "bFilter": false,
            "bPaginate": false,
            "bLengthChange": false,
            "columns": columns,
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblDetail.data())) {
                }
                App.unblockUI('.panelSearchResult');

            },
            "order": [],
            "destroy": true
        })
    },
}

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