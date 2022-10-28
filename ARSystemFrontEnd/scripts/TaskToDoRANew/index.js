
jQuery(document).ready(function () {
    Data.BindDataHeader();
    $('#DetailTable').hide();
});

var Data = {
    BindDataHeader: function () {
        $.ajax({
            url: "/api/TaskToDoRANew/getToDoList",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                Data.LoadTable2(data);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {

            });
    },

    BindDataDetail: function (toDoName) {
        $('#DetailTable').css('visibility', 'visible');
        $('#DetailTable').show();
        $('#ToDoname').text(toDoName);
        $.ajax({
            url: "/api/TaskToDoRANew/getToDoListDetail",
            type: "GET",
            data: { ToDoName: toDoName },
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                Data.LoadTable(data);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {

            });
    },
    LoadTable: function (data) {
        var idTbl = $("#TableDetailSummary");

        idTbl.DataTable({
            "serverSide": false,
            "filter": false,
            "destroy": true,
            "async": false,
            "data": data,
            buttons: [

                { extend: 'copy', className: 'btn white btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn white btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".white"));
                        l.start();
                        Data.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn white btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "columns": [
                { data: "Id" },
                { data: "CustomerId" },
                {
                    data: "Stip1",

                },
                { data: "Stip267" },
                { data: "Others" },
            ],
            "order": [[0, "asc"]],
            "paging": false,
            "columnDefs": [{ 'targets': [0], 'visible': false }, { 'targets': [1, 2], 'className': 'text-center' }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;
                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                            i : 0;
                };

                // Total over all pages
                for (var i = 2; i <= 4; i++) {
                    total = api
                        .column(i)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    // Total over this page
                    pageTotal = api
                        .column(i, { page: 'current' })
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);
                    $(api.column(i).footer()).html(total >= 1000 ? Common.Format.CommaSeparationOnly(total) : total);
                }


            },
            "scrollY": false,
            "scrollX": false,
            "bInfo": false
        });

    },
    LoadTable2: function (data) {
        var idTbl = $("#TableSummary");

        idTbl.DataTable({
            "serverSide": false,
            "filter": false,
            "destroy": true,
            "async": false,
            "data": data,
            "columns": [
                {
                    data: "TaskToDoName", mRender: function (i, j, item) {
                        var str = item.TaskToDoName;
                        if (item.TaskToDoName == 'Receive Hardcopy BAUK')
                            str += ' (XL Stip 2, SMART and HCPT)';

                        return '<a href="' + item.UrlPage + '" target="_blank">' + str + '</a>';
                    }
                },
                {
                    data: "CountData", mRender: function (i, j, item) {
                        if (item.TaskToDoName == 'Create BAPS') {
                            return Common.Format.CommaSeparationOnly(item.CountData) + ' &nbsp;<i class="fa fa-file btn btn-xs green linkExport" title="export data"></i>&nbsp;&nbsp;<i class="fa fa-eye btn btn-xs blue linkDetail" title="view detail" ></i>';
                        } else {
                            return Common.Format.CommaSeparationOnly(item.CountData) + ' &nbsp;<i class="fa fa-eye btn btn-xs blue linkDetail" title="view detail"></i>';
                        }


                    }
                },
            ],
            "order": false,
            "paging": false,
            "columnDefs": [{ 'targets': [1], 'className': 'text-right' }],
            "scrollY": false,
            "scrollX": false,
            "bInfo": false
        });
        $('#TableSummary tbody').unbind();
        $('#TableSummary tbody').on("click", ".linkDetail", function (e) {
            var table = $('#TableSummary').DataTable();
            var row = table.row($(this).parents('tr')).data();
            Data.BindDataDetail(row.TaskToDoName);
        });
        $('#TableSummary tbody').on("click", ".linkExport", function (e) {
            window.location.href = "/TaskToDoRANew/ExportCreateBaps";
        });

    },
    Export: function () {
        window.location.href = '/TaskToDoRANew/ExportData?ToDoName=' + $('#ToDoname').text();
    }
}