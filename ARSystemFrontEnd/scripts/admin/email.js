Data = {};

jQuery(document).ready(function () {
    Form.Init();
    Table.Init();
    Table.Search();
 
    //panel Summary
    $("#formSearch").submit(function (e) {
        Table.Search();
        e.preventDefault();
    });

    $("#btSearch").unbind().click(function () {
        Table.Search();
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
    });

    //panel transaction
    $("#formTransaction").submit(function (e) {
        Email.Put();
        e.preventDefault();
    });

    $(".btCancel").unbind().click(function () {
        Form.Cancel();
    });
});

var Form = {
    Init: function () {
        $("#pnlTransaction").hide();
        $("#formTransaction").parsley();
    },
    Edit: function () {
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $("#formTransaction").parsley().reset()
        console.log(Data.Selected);
        $("#chStatus").bootstrapSwitch("state", Data.Selected.IsActive);
        $("#tbEmailName").val(Data.Selected.EmailName);
        $("#tbSubject").val(Data.Selected.Subject);
        $("#snBody").summernote("code", Data.Selected.Body);
        
        Table.TableEmailVariable();
    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
        $(".panelSearchBegin").fadeIn();
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        Table.Search();
    }
}

var Table = {
    Init: function () {
        //Table Summary
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();

        var tblSummaryData = $('#tblSummaryData').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $("#tblSummaryData tbody").on("click", "button.btEdit", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                Form.Edit();
            }
        });

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });

        //Table Email Variable
        var tblEmailVariable = $("#tblEmailVariable").DataTable({
            "language": {
                "emptyTable": "No data available in table",
            },
            "bLengthChange": false,
            "paging": false,
            "scrollY": "300px",
            "scrollCollapse": true,
            "data": [],
            "columns": [
                { data: "EmailVariable" },
                { data: "EmailVariableDesc" }
            ],
            "columnDefs": [
                { "className": "dt-center", "targets": "_all" }
            ]
        });

        $(window).resize(function () {
            $("#tblEmailVariable").DataTable().columns.adjust().draw();
        });

        $("#mdlEmailVariable").on("shown.bs.modal", function () {
            $("#tblEmailVariable").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();

        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/email/grid",
                "type": "POST",
                "datatype": "json",
                "data": {
                    strEmailName: $("#tbSearchEmailName").val(),
                    intIsActive: ($("#rbSearchIsActiveYes").is(":checked") ? 1 : ($("#rbSearchIsActiveNo").is(":checked") ? 0 : -1))
                },
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "order": [[1, 'asc']],
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        if ($("#hdAllowEdit").val())
                            strReturn += "<button type='button' title='Edit' class='btn btn-xs green btEdit'><i class='fa fa-edit'></i></button>";
                        return strReturn;
                    }
                },
                { data: "EmailName" },
                {
                    mRender: function (data, type, full) {
                        return full.IsActive ? "Yes" : "No";
                    }
                },
            ],
            "columnDefs": [
                { "targets": [0, 2], "orderable": false },
                { "targets": [2], "className": "dt-center" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop();
            }
        });
    },
    Reset: function () {
        $("#tbSearchSource").val("");
    },
    TableEmailVariable: function () {
        var data = Data.Selected.EmailVariables;
        var table = $("#tblEmailVariable").DataTable();
        table.clear().rows.add(data).draw();
    },
    Export: function () {

        var strEmailName = $("#tbSearchEmailName").val();
        var intIsActive = ($("#rbSearchIsActiveYes").is(":checked") ? 1 : ($("#rbSearchIsActiveNo").is(":checked") ? 0 : -1))

        window.location.href = "/Admin/Email/Export?strEmailName=" + strEmailName + "&intIsActive=" + intIsActive;
    },
}

var Email = {
    Put: function () {
        var params = {
            EmailName: $("#tbEmailName").val(),
            Subject: $("#tbSubject").val(),
            Body: $('#snBody').summernote('code')
        }

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/email/" + Data.Selected.mstEmailID,
            type: "PUT",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Email has been edited!")
                Form.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    }
}