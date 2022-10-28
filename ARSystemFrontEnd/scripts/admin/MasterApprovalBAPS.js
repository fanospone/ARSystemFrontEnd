Data = {};

jQuery(document).ready(function () {
    Form.Init();
    Table.Init();   
    Table.LoadData();

});

var Form = {
    Init: function () {
        
        $("#pnlSummary").show();
        $("#pnlFormInput").hide();
        
        Control.BindCustomer("#slSearchCustomerID");
        Control.BindRegion("#slSearcRegionID");
        Control.BindCustomer("#iCustomerID");
        Control.BindRegion("#iRegionID");

        $("#btnAddNewData").unbind().click(function () {
            Form.Done();
            $("#mdlApprovalBAPS").modal('show');
        });
        $("#formApprovalBAPS").parsley();
        $("#btnTrxSave").unbind().click(function (e) {
            e.preventDefault();
            /* Your Input must be in <form> </form>  */
            reqValue = $("#formApprovalBAPS").parsley().validate();
            if ($("#formApprovalBAPS").parsley().validate()) {
                Form.SaveApprovalBAPS();
                // Add Your function In Here
            }
        })

        $("#btnTrxCancel").unbind().click(function () {
            Form.Done();
        });

        $("#btSearch").unbind().click(function () {
            Table.Init();
            Table.LoadData();
        });

        $("#btReset").unbind().click(function () {
            $("#slSearchCustomerID").val("").trigger('change');
            $("#slSearcRegionID").val("").trigger('change');
            $("#tbSearcApprName").val("");
            $("#tbSearcApprJob").val("");
        });

        $("#btnAddNewData").unbind().click(function () {
            
            $('#formApprovalBAPS').parsley().reset();
            //$("#mdlApprovalBAPS").modal("show");
            $("#pnlSummary").fadeOut(500);
            $("#pnlFormInput").fadeIn(1000);
        });
    },

    Done: function () {
        $("#idTrx").val('');
        $("#iPosition").val('');
        $("#iApprovalName").val('');
        $("#iCustomerID").val('').trigger('change');
        $("#iRegionID").val('').trigger('change');
        $("#pnlFormInput").fadeOut(500);
        $("#pnlSummary").fadeIn(1000);
    },

    SaveApprovalBAPS: function () {
        var isAvtive = true;
        if ($("#iIsActive").bootstrapSwitch("state") == false)
            isAvtive = false;

        Data.ApprovalID = $("#idTrx").val();
        Data.ApprovalName = $("#iApprovalName").val();
        Data.Position = $("#iPosition").val();
        Data.RegionID = $("#iRegionID").val();
        Data.OperatorID = $("#iCustomerID").val();
        Data.IsActive = isAvtive;

        var params = {
            ApprBaps: Data
        }

        var l = Ladda.create(document.querySelector("#btnTrxSave"))
        $.ajax({
            url: "/api/ApprovalBAPS/submit",
            type: "post",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                $("#mdlApprovalBAPS").modal("hide");
                Form.Done();
                Common.Alert.Success("Data has been saved!")
                Table.LoadData();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    },

    DeleteApprovalBAPS: function (ID) {
        Data.ApprovalID = ID;
        var params = {
            ApprBaps: Data
        }
        $.ajax({
            url: "/api/ApprovalBAPS/submit",
            type: "post",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Data has been deleted!")
                Table.LoadData();
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        })
    },
}

var Table = {
    Init: function () {
        //Table Summary
        $(".panelSearchResult").hide();

        var tblApprovalBAPS = $('#tblApprovalBAPS').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });


    },

    LoadData: function () {
        var CustomerID = $("#slSearchCustomerID").val();
        var ApprName = $("#tbSearcApprName").val();
        var RegionID = $("#slSearcRegionID").val();
        var Position = $("#tbSearcApprJob").val();
        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();
        var params = { strCustomerID: CustomerID, strRegionID: RegionID, strApprovalName: ApprName, strPosition: Position };

        var tblApprovalBAPS = $("#tblApprovalBAPS").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/ApprovalBAPS/getList",
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
            "order": [[1, 'asc']],
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                { data: "RowIndex" },
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<i class='fa fa-edit btn btn-xs green link-btEdit'></i>";
                        return strReturn;
                    }
                },
                { data: "RegionName" },
                { data: "OperatorID" },
                { data: "ApprovalName" },
                { data: "Position" },
                {
                    mRender: function (data, type, full) {
                        return full.IsActive ? "Yes" : "No";
                    }
                },
                { data: "ApprovalID" },
                { data: "RegionID" },
            ],
            "columnDefs": [
                { "targets": [0], "orderable": false },
                { "targets": [1], "class": "text-center" },
                { "targets": [7], "visible": false },
                { "targets": [8], "visible": false },
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblApprovalBAPS.data())) {
                    $(".panelSearchResult").fadeIn();
                }
                l.stop();
            }
        });

        $("#tblApprovalBAPS tbody").unbind();
        $("#tblApprovalBAPS tbody").on("click", ".link-btEdit", function (e) {
            
            var table = $("#tblApprovalBAPS").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $('#formApprovalBAPS').parsley().reset();
            $("#idTrx").val(row.ApprovalID);
            $("#iApprovalName").val(row.ApprovalName);
            $("#iPosition").val(row.Position);
            $("#iRegionID").val($.trim(row.RegionID)).trigger("change");
            $("#iCustomerID").val($.trim(row.OperatorID)).trigger("change");
            $("#iIsActive").bootstrapSwitch("state", row.IsActive);
            $("#pnlSummary").fadeOut(500);
            $("#pnlFormInput").fadeIn(1000);
        });
    },

    Export: function () {

        
        window.location.href = "/Admin/ApprovalBAPS/Export";
    },
}

var Control = {
    BindRegion: function (id) {
        $.ajax({
            url: "/api/MstDataSource/Region",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>");
            
            if (Common.CheckError.List(data)) {
                $(id).append("<option value='0'>ALL</option>");
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.RegionID) + "'>" + item.RegionName + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Region", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindCustomer: function (id) {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.OperatorId) + "'>" + item.Operator + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Customer", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
}
