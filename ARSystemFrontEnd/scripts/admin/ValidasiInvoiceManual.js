Data = {};

jQuery(document).ready(function () {
    Control.BindingSelectFieldName();
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

    $("#btCreate").unbind().click(function () {
        Form.Create();
    });

    $("#btSubmit").on('click', function () {
        $("#form").submit();
    });

    $("#form").on('submit', function (e) {
        if ($("#form").parsley().validate()) {
            if (Data.Mode == "Create") {
                Mapping.Post();
            } else if (Data.Mode == "Edit") {
                Mapping.Put();
            }
        }
        e.preventDefault();
    });

    $(".btCancel").unbind().click(function () {
        Form.Cancel();
        Table.Reset();
    });

    $("#btBack").unbind().click(function () {
        $("#pnlSummary").fadeIn();
        $("#pnlDetailResult").hide();
    });
});

var Form = {
    Init: function () {
        $("#pnlTransaction").hide();
        $("#form").parsley();

        if (!$("#hdAllowAdd").val()) {
            $("#btCreate").hide();
        }


        Control.BindingSelectFieldName();
        Table.Reset();
    },
    Create: function () {
        Data.Mode = "Create";
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Create Customer");
        $("#form").parsley().reset()

        // Form Fields
        $("#chStatus").bootstrapSwitch("state", true);
        $("#chMandatory").bootstrapSwitch("state", true);

        //$("#tbOperatorReport").val("");

        $("#slFieldName").val("").trigger('change');


    },
    Edit: function () {
        Data.Mode = "Edit";
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Edit Customer");
        $("#form").parsley().reset();

        $("#chStatus").bootstrapSwitch("state", Data.Selected.isActive);
        $("#chMandatory").bootstrapSwitch("state", Data.Selected.isMandatory);
        
        $('#slFieldName').val(Data.Selected.FieldName).trigger('change');

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
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblSummaryData = $('#tblSummaryData').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $("#tblSummaryData tbody").unbind().on("click", "button.btEdit", function (e) {
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
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        //var operatorId = $("#slSearchOperator").val() == null || $("#slSearchOperator").val() == "" || $("#slSearchOperator").val() == undefined ? 0 : $("#slSearchOperator").val();
        var params = {
            FieldName: $("#slSearchFieldName").val() == null || $("#slSearchFieldName").val() == "" || $("#slSearchFieldName").val() == undefined ? "" : $("#slSearchFieldName").val(),
            isMandatory: ($("#rbSearchMandatoryYes").is(":checked") ? 1 : ($("#rbSearchMandatoryNo").is(":checked") ? 0 : -1)),
            isActive: ($("#rbSearchStatusActive").is(":checked") ? 1 : ($("#rbSearchStatusInactive").is(":checked") ? 0 : -1))
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/ValidasiInvoiceManual/grid",
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
                        Table.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        //strReturn += "<button type='button' title='Detail' class='btn green btDetail'><i class='fa fa-search-plus'></i></button>";
                        if ($("#hdAllowEdit").val())
                            strReturn += "<button type='button' title='Edit' class='btn btn-xs green btEdit'><i class='fa fa-mouse-pointer'></i></button>";
                        return strReturn;
                    }
                },
                { data: "FieldName" },
                {
                    data: "isMandatory", mRender: function (data, type, full) {
                        return full.isMandatory ? "Yes" : "No";
                    }
                },
                {
                    data: "isActive", mRender: function (data, type, full) {
                        return full.isActive ? "Active" : "InActive";
                    }
                }
            ],
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
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            
        });
    },
    Reset: function () {
        
        $("#slSearchFieldName").val("").trigger('change');
        $("#rbSearchStatusAll").prop("checked", true);
        $("#rbSearchMandatoryAll").prop("checked", true);
    },
    Export: function () {        
        var FieldName = $("#slSearchOperator").val() == null || $("#slSearchOperator").val() == "" || $("#slSearchOperator").val() == undefined ? "" : $("#slSearchOperator").val();
        var IsMandatory = $("input[name='rdMandatory']:checked").val();
        var IsActive = $("input[name='rdStatus']:checked").val();

        window.location.href = "/Admin/ValidasiInvoiceManual/Export?FieldName=" + FieldName + "&IsMandatory=" + IsMandatory + "&IsActive=" + IsActive;
    }
}

var Mapping = {
    Post: function () {

        var params = {            
            FieldName: $("#slFieldName").val() == null || $("#slFieldName").val() == "" || $("#slFieldName").val() == undefined ? 0 : $("#slFieldName").val(),
            isMandatory: $("#chMandatory").bootstrapSwitch("state"),
            isActive: $("#chStatus").bootstrapSwitch("state")
        }

        var l = Ladda.create(document.querySelector("#btSubmit"));
        $.ajax({
            url: "/api/ValidasiInvoiceManual/Create",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Data has been created!");
                Table.Reset();
                Form.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    },

    Put: function () {


        var params = {
            FieldName: $("#slFieldName").val() == null || $("#slFieldName").val() == "" || $("#slFieldName").val() == undefined ? 0 : $("#slFieldName").val(),
            isMandatory: $("#chMandatory").bootstrapSwitch("state"),
            isActive: $("#chStatus").bootstrapSwitch("state")
        }

        var l = Ladda.create(document.querySelector("#btSubmit"));
        $.ajax({
            url: "/api/ValidasiInvoiceManual/" + Data.Selected.mstValidasiManualID,
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
                Common.Alert.Success("Data has been edited!");
                Table.Reset();
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
var Control = {
    BindingSelectFieldName: function () {
        $.ajax({
            url: "/api/MstDataSource/FieldName",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchFieldName").html("<option></option>")
            $("#slFieldName").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchFieldName").append("<option value='" + item.Column_Name + "'>" + item.Column_Name + "</option>");
                    $("#slFieldName").append("<option value='" + item.Column_Name + "'>" + item.Column_Name + "</option>");
                })
            }

            $("#slSearchFieldName").select2({ placeholder: "Select FieldName", width: null });
            $("#slFieldName").select2({ placeholder: "Select FieldName", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    }
}