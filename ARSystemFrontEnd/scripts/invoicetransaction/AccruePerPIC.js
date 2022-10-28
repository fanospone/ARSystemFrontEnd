Data = {};
jQuery(document).ready(function () {

    Form.Init();
    //var getPeriode = helper.convertDate($("#slMonthYear").val().toString().trim());
    $("#btSearch").unbind().click(function () {

        var dateParam = $("#slMonthYear").val().toString().trim();

        if (dateParam != undefined && dateParam != "")
            Form.bind();
        else
            Common.Alert.Error("Please Fill Data");
    });
    $("#btReset").unbind().click(function () {
        $(".panelSearchBegin").fadeIn();
        $(".panelSearchResult").hide()
    });
});

var Form = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();

        var tblSummaryData = $('#tblSummaryData').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

    },
    bind: function () {
        Form.search();
    },

    search: function () {
        var columns = [];
        var selector = "#tblSummaryData";

        columns = [
            { data: "ID" },
            { data: "PIC" },
            { data: "ACCOUNTING" },
            { data: "FINANCE" },
            { data: "FLEXIUPDATE" },
            { data: "LEGAL" },
            { data: "PDIMARKETING" },
            { data: "RAREC" },
            { data: "ASSET" },
            { data: "MARKETING" },
            { data: "PDI" },
            { data: "PROJECT" },
            { data: "RANEW" },
            { data: "XLMINICME" },
            { data: "TOTAL" }
        ];

        Form.draw(selector, columns);
    },

    draw: function (selector, columns) {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var years, months, dateParam;

        dateParam = $("#slMonthYear").val().toString().trim();
        var years = dateParam.substring(0, 4);
        var months = parseInt(dateParam.slice(-2));

        var param = {
            year: years,
            month: months,
            week: $("#slWeek").val().toString().trim()
        };

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
                "url": "/Api/RevenueSystem/GetRevSysAccruePerPIC",
                "type": "POST",
                "datatype": "json",
                "data": param
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
            "bFilter": false,
            "bLengthChange": false,
            "columns": columns,
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblDetail.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop();
                App.unblockUI('.panelSearchResult');

            },
            "order": [],
            "destroy": true
        })
    },

    Export: function (param) {
        window.location.href = "/RevenueSystem/RevSysAccruePerPIC/Export?year=" + param.year + "&month=" + param.month + "&week=" + param.week;
    }
}

var helper = {
    convertDate: function (dateParam) {
        var year = dateParam.substring(0, 4);
        var month = parseInt(dateParam.slice(-2));
        var months = ['January', 'February', 'Maret', 'April', 'May', 'Juni', 'Juli', 'Agustus', 'September', 'Oktober', 'November', 'Desember'];

        var mm = months[month - 1];

        return mm + ' ' + year;
    }
};