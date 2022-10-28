Data = {};

jQuery(document).ready(function () {
    //Control.LoadYear();
    //Control.LoadMonth();
    Table.Init();
    Table.Search();

    //Panel Add/Edit Master Kurs
    $("#formKurs").submit(function (e) {
        e.preventDefault();
    });

    $("#btSave").unbind().click(function () {
        //    Table.Search();
        Table.Save();
    });

    $("#btCancel").unbind().click(function () {
        Table.Reset();
    });

    //Format Date
    $('.date-picker').datepicker({
        format: 'dd MM yyyy',
        autoclose: true
    });

    //Kurs Input Formatter
    $('input.number').keyup(function (event) {

        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return Helper.FormatNumber(value);
        });
    });


});

var Table = {
    Init: function () {
        var tblSummaryData = $('#tblMasterKurs').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblMasterKurs").DataTable().columns.adjust().draw();
        });

        $('#tblMasterKurs tbody').on('click', 'tr', function () {
            var kursDate = $(this).find('td').eq(0).text();
            var kursTengahBI = $(this).find('td').eq(1).text();
            var kursPajak = $(this).find('td').eq(2).text();

            $("#txtKursDate").val(kursDate).trigger("change");
            $("#txtKursTengahBI").val(kursTengahBI).trigger("change");
            $("#txtKursPajak").val(kursPajak).trigger("change");

            $("html, body").animate({
                scrollTop: 0
            }, 600);
        });
    },
    Search: function () {
        var columns = [];
        columns = [
                {
                    data: "KursDate", render: function (val, type, full) {
                        return "<b>" + $.datepicker.formatDate('dd MM yy', new Date(val)); + "</b>";
                    }
                },
                {
                    data: "KursTengahBI", render: function (val, type, full) {
                        return Helper.FormatNumber(val.toString());
                    }
                },
                {
                    data: "KursPajak", render: function (val, type, full) {
                        return Helper.FormatNumber(val.toString());
                    }
                }
            ]
        selector = "#tblMasterKurs";
        Table.Draw(selector, columns);
    },
    Reset: function () {
        $("#txtKursDate").val("").trigger("change");
        $('.date-picker').data('datepicker').setDate(null);
        $("#txtKursTengahBI").val("").trigger("change");
        $("#txtKursPajak").val("").trigger("change");
    },
    Export: function () {
        window.location.href = "/Admin/Kurs/Export";
    },
    Draw: function (selector, columns) {

        var l = Ladda.create(document.querySelector("#btSave"))
        l.start();

        var data = {};

        var tblSummaryData = $(selector).DataTable({
            "proccessing": true,
            "serverSide": true,
            "select": {
                        "style": "single"
            },
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/Kurs",
                "type": "POST",
                "datatype": "json",
                "data": data
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' } },
                {
                    text: 'Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: 'Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50, 100, -1], ['10', '25', '50', '100', 'All']],
            "destroy": true,
            "columns": columns,
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                }
                l.stop();
            },
            'ordering': false,
            'order': []
        });
    },
    Save: function () {
        var params = {
            kursDate: $.datepicker.formatDate('yy/mm/dd', new Date($("#txtKursDate").val())),
            kursTengahBI: $("#txtKursTengahBI").val().replace(",",""),
            kursPajak: $("#txtKursPajak").val().replace(",", "")
        }

        if (!Date.parse(params.kursDate)) {
            Common.Alert.Warning("Kurs Date Not Valid");
            return;
        }else if (!params.kursTengahBI) {
            Common.Alert.Warning("Kurs Tengah BI Not Valid");
            return;
        } else if (!params.kursPajak) {
            Common.Alert.Warning("Kurs Pajak Not Valid");
            return;
        }

        $.ajax({
            url: "/api/Kurs/Save",
            type: "POST",
            data: params,
            dataType: "json"
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                Common.Alert.Success("Master Kurs Saved!");
                Table.Search();
                Table.Reset();
            }
        });
    },
}

var Helper = {
    FormatNumber: function (value) {
        // format number
        return value
            .replace(/\D/g, "")
            .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
        ;
    }
}