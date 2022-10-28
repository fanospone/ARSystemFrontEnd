Data = {};

jQuery(document).ready(function () {
    Form.Init();
    Table.LoadData();
});

var Form = {
    Init: function () {

        Table.Init();

        if (!$("#hdAllowAdd").val())
            $(".btnAddNewData").hide();


        $("#sCategoryBuilding").val('');
        
        $("#btnAddNewData").unbind().click(function () {
            Form.Done();
            $("#mdlCategoryBuilding").modal('show');
            $("#formCategoryBuilding").parsley().reset();
        });
        $("#formCategoryBuilding").parsley();
        $("#btnTrxSave").unbind().click(function (e) {
            e.preventDefault();
            /* Your Input must be in <form> </form>  */
            reqValue = $("#formCategoryBuilding").parsley().validate();
            if ($("#formCategoryBuilding").parsley().validate()) {
                Form.SaveCategoryBuilding();
                // Add Your function In Here
            }
        })

        $("#btnTrxCancel").unbind().click(function () {
            Form.Done();
        });

        $("#btSearch").unbind().click(function () {
            Table.LoadData();
        });

        $("#btReset").unbind().click(function () {
            $("#sCategoryBuilding").val("");
        });

        Control.BindParameter("PPH", "#iPPH");
        Control.BindParameter("PPN", "#iPPN");
        
        Control.FormatCurrency("#iPricePerArea");
        $("#IsActive").bootstrapSwitch("state", true);

        $("input[name=iBuildingType]").unbind().change(function () {
            var categ = $("input[name=iBuildingType]:checked").val();
            if (categ == "1") {
                $("#lbPrice").text('Price / Area');
                $("#iPricePerArea").prop('required', true);
            } else {
                $("#lbPrice").text('Price / Month');
                $("#iPricePerArea").prop('required', true);
            }
        })
    },

    Done: function () {
        $("#idTrx").val('');
        $("#iCategoryBuilding").val('');
        $("#iPricePerArea").val('');
        $("#iPPH").val('').trigger('change');
        $("#iPPN").val('').trigger('change');

    },

    SaveCategoryBuilding: function () {
        var isAvtive = true;
        if ($("#iIsActive").bootstrapSwitch("state") == false)
            isAvtive = false;

        var categ = $("input[name=iBuildingType]:checked").val();
        if (categ == "1" && $("#iPricePerArea").val() == "0.00"){
            $("#lbPriceMessage").text("Price Per Area must be > 0 !");
        }
        else {
            $("#lbPriceMessage").text("");
            Data.ID = $("#idTrx").val();
            Data.CategoryBuilding = $("#iCategoryBuilding").val();
            Data.PPHID = $("#iPPH").val();
            Data.PPNID = $("#iPPN").val();
            Data.PricePerArea = $("#iPricePerArea").val();
            Data.IsActive = isAvtive;
            Data.BuildingType = $("input[name=iBuildingType]:checked").val();

            var params = {
                model: Data
            }

            var l = Ladda.create(document.querySelector("#btnTrxSave"))
            $.ajax({
                url: "/api/CategoryBuilding/Save",
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
                    $("#mdlCategoryBuilding").modal("hide");
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
        }
    },

    DeleteCategoryBuilding: function (ID) {
        Data.ID = ID;
        var params = {
            model: Data
        }
        $.ajax({
            url: "/api/CategoryBuilding/Delete",
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

        var tblCategoryBuilding = $('#tblCategoryBuilding').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });


    },

    LoadData: function () {
        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();
        var params = { strCategoryBuilding: $("#sCategoryBuilding").val() };

        var tblCategoryBuilding = $("#tblCategoryBuilding").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/CategoryBuilding/GetList",
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
                        if ($("#hdAllowEdit").val())
                            strReturn += "<i class='fa fa-edit btn btn-xs green link-btEdit'></i>";

                        return strReturn;
                    }
                },
                { data: "CategoryBuilding" },
                { data: "PPHType" },
                { data: "PPNType" },
                  {
                      data: "PricePerArea", render: function (data) {
                          return Common.Format.CommaSeparation(data);
                      }
                  },
                {
                    mRender: function (data, type, full) {
                        return full.IsActive ? "Yes" : "No";
                    }
                },
                 { data: "ID" },
                 { data: "PPHID" },
                { data: "PPNID" },
                {
                    mRender: function (data, type, full) {
                        var BuildingType = "Building";
                        if (full.BuildingType == 0)
                            BuildingType = "Non Building";

                        return BuildingType;
                    }
                },
                { data: "BuildingType" }
            ],
            "columnDefs": [
                { "targets": [0], "orderable": false },
                { "targets": [1], "class": "text-center" },
                { "targets": [7], "visible": false },
                { "targets": [8], "visible": false },
                { "targets": [9], "visible": false },
                { "targets": [11], "visible": false },
                { "targets": [5], "className": "text-right" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblCategoryBuilding.data())) {
                    $(".panelSearchResult").fadeIn();
                }
                l.stop();
            }
        });

        $("#tblCategoryBuilding tbody").unbind();
        $("#tblCategoryBuilding tbody").on("click", ".link-btEdit", function (e) {
            var table = $("#tblCategoryBuilding").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $("#formCategoryBuilding").parsley().reset();
            $("#idTrx").val(row.ID);
            $("#iCategoryBuilding").val(row.CategoryBuilding);
            $("#iPricePerArea").val(Common.Format.CommaSeparation(row.PricePerArea));
            $("#iPPH").val(row.PPHID).trigger('change');
            $("#iPPN").val(row.PPNID).trigger('change');
            $("#iIsActive").bootstrapSwitch("state", row.IsActive);
            if (row.BuildingType == "1") {
                $("input[name=iBuildingType][value='1'").prop('checked', true)
                $("input[name=iBuildingType][value='0'").prop('checked', false)
            } else {
                $("input[name=iBuildingType][value='1']").prop('checked', false)
                $("input[name=iBuildingType][value='0']").prop('checked', true)
            }
            
            $("#mdlCategoryBuilding").modal("show");
        });
    },

    Export: function () {

        var  strCategoryBuilding =$("#sCategoryBuilding").val() 
        window.location.href = "/Admin/CategoryBuilding/Export?strCategoryBuilding=" + strCategoryBuilding;
    },
}

var Control = {
    BindParameter: function (paramType, id) {
        //var params ={strParamterType : paramType}
        $.ajax({
            url: "/api/MstDataSource/Parameter",
            type: "GET",
            data: { strParamterType: paramType },
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.Value) + "'>" + item.Text + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select " + paramType, width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    FormatCurrency: function (selectedID) {
    $(selectedID).unbind().on('blur', function () {
        var value = $(selectedID).val();
        if (value != "") {
            $(selectedID).val(Common.Format.CommaSeparation(value));
        } else {
            $(selectedID).val("0.00");
        }
    });
},

}
