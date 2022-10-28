Data = {};

/* Helper Functions */

jQuery(document).ready(function () {
    Form.Init();
    Table.Init();
    Table.Search();

    // Initialize Datepicker
    $("body").delegate(".datepicker", "focusin", function () {
        $(this).datepicker({
            format: "dd-M-yyyy"
        });
    });

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

    //panel transaction Header
    $("#formTransaction").submit(function (e) {
        if (Data.Mode == "Create")
            ProductType.Post();
        else if (Data.Mode == "Edit")
            ProductType.Put();
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
        $("#formTransaction").parsley();

        if (!$("#hdAllowAdd").val()) {
            $("#btCreate").hide();
        }

        Table.Reset();
    },
    Create: function () {
        Data.Mode = "Create";
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Create Product Type");
        $("#formTransaction").parsley().reset()

        $("#tbProductType").val("");
        $("#tbProductCode").val("");
        $("#tbEffectiveDate").val("");
        $("#chRelatedToSonumb").bootstrapSwitch("state", true);
        $("#chStatus").bootstrapSwitch("state", true);
    },
    Edit: function () {
        Data.Mode = "Edit";
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Edit Product Type");
        $("#formTransaction").parsley().reset()
        
        $("#tbProductType").val(Data.Selected.ProductType);
        $("#tbProductCode").val(Data.Selected.ProductCode);
        $("#tbEffectiveDate").val(Common.Format.ConvertJSONDateTime(Data.Selected.EffectiveDate));
        $("#chRelatedToSonumb").bootstrapSwitch("state", Data.Selected.RelatedToSonumb);
        $("#chStatus").bootstrapSwitch("state", Data.Selected.IsActive);
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
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var params = {
            productType: $("#tbSearchProductType").val(),
            productCode: $("#tbSearchProductCode").val(),
            intIsActive: ($("#rbSearchStatusActive").is(":checked") ? 1 : ($("#rbSearchStatusInactive").is(":checked") ? 0 : -1))
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/MstProductType/grid",
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
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        //strReturn += "<button type='button' title='Detail' class='btn green btDetail'><i class='fa fa-search-plus'></i></button>";
                        if ($("#hdAllowEdit").val())
                            strReturn += "<button type='button' title='Edit' class='btn btn-xs green btEdit'><i class='fa fa-edit'></i></button>";
                        return strReturn;
                    }
                },
                { data: "ProductType" },
                { data: "ProductCode" },
                {
                    data: "EffectiveDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    mRender: function (data, type, full) {
                        console.log(full);
                        return full.IsActive ? "Yes" : "No";
                    }
                },
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop();
            },
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': [0]
            }],
            "order": [[1, "asc"]]
        });
    },
    Reset: function () {
        $("#tbSearchProductType").val("");
        $("#tbSearchProductCode").val("");
        $("#rbSearchStatusAll").prop("checked", true);
    },
    Export: function () {
        var productType = $("#tbSearchProductType").val();
        var productCode = $("#tbSearchProductCode").val();
        var intIsActive = ($("#rbSearchStatusActive").is(":checked") ? 1 : ($("#rbSearchStatusInactive").is(":checked") ? 0 : -1));

        window.location.href = "/Admin/ProductType/Export?productCode=" + productCode + "&productType=" + productType + "&status=" + intIsActive;
    }
}

var ProductType = {
    Post: function () {
        var params = {
            ProductCode: $("#tbProductCode").val(),
            ProductType: $("#tbProductType").val(),
            EffectiveDate: $("#tbEffectiveDate").val(),
            RelatedToSonumb: $("#chRelatedToSonumb").bootstrapSwitch("state"),
            IsActive: $("#chStatus").bootstrapSwitch("state")
        }

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/MstProductType",
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
                Common.Alert.Success("Master Product Type has been created!");
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
            ProductCode: $("#tbProductCode").val(),
            ProductType: $("#tbProductType").val(),
            EffectiveDate: $("#tbEffectiveDate").val(),
            RelatedToSonumb: $("#chRelatedToSonumb").bootstrapSwitch("state"),
            IsActive: $("#chStatus").bootstrapSwitch("state")
        }

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/MstProductType/" + Data.Selected.ProductTypeId,
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
                Common.Alert.Success("Master Product Type has been edited!");
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