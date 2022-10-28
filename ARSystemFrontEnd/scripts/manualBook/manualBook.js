$(document).ready(function () {
    Table.Init();
});

var Table = {
    Init: function () {
        var tblSummary = $('#tblManualBook').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(".btnSearch").unbind().click(function () {
            Table.Search();
        });

        $('#tblSummary input').keypress(function (e) {
            if (e.which == 13) { Table.Search(); }
        });

        $("#tblManualBook tbody").on("click", "button.btnDownload", function (e) {
            var table = $("#tblManualBook").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                window.open("/ManualBook/Download/Document/" + data.ProjectID);
            }
        });

        Table.Search();
    },
    Search: function () {
        App.blockUI({ target: "#tblManualBook", animate: !0 });

        var tblSummary = $("#tblManualBook").DataTable({
            "orderCellsTop": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/manualBook/view",
                "type": "POST",
                "datatype": "json",
                "data": {
                    strProjectName: $("#tbxSearchProjectName").val().replace(/'/g, "''"),
                    strProjectDescription: $("#tbxSearchProjectDescription").val().replace(/'/g, "''"),
                },
            },
            buttons: [
                { extend: 'colvis', className: 'btn dark btn-outline', text: 'Columns' }
            ],
            "filter": false,
            "order": [[1, 'asc']],
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    mRender: function (data, type, full) {
                        return "<button type='button' title='Download' class='btn btn-xs green btnDownload'><i class='fa fa-download'></i></button>";
                    }
                },
                { data: "ProjectName" },
                { data: "ProjectDescription" },
            ],
            "columnDefs": [
                { "targets": [0], "width": "5%", "className": "dt-center", "orderable": false },
                { "targets": [1], "width": "10%", "className": "dt-center" },
                { "targets": [2], "width": "15%", "className": "dt-center" },
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                Common.CheckError.List(tblSummary.data());
                App.unblockUI("#tblManualBook");
            }
        });
    }
}

