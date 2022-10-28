Data = {};

jQuery(document).ready(function () {
    Form.Init();
    Table.Init();
    Table.LoadData();
});

var Form = {
    Init: function () {
        $("#btnAddNewData").unbind().click(function () {
            Data.Mode = "Add"
            $('.attrData').hide();
            Form.Done();
            $("#btnTrx").html("Save");
            $("#formTransaction").parsley().reset()
            $("#MdDetail").modal("show");
        });
              
        //if (!$("#hdAllowAdd").val())
        //    $(".btnAddNewData").hide();        
        //$("#btnTrxSave").unbind().click(function () {
        //    Form.Save();
        //});
        //$("#btnTrxCancel").unbind().click(function () {
        //    Form.Done();
        //});       
        //$("#btReset").unbind().click(function () {
        //    $("#sSONumber").val("");
        //}); 
    },

    Done: function () {
        $("#tb1").val('automatically...');
        $("#tb2").val('');
        $("#tb3").val('');
        $("#tb2").prop('disabled', false);
        $("#tb4").val('automatically...');
        $("#tb5").val('automatically...');
        $("#tb6").val('automatically...');
        $("#tb7").val('automatically...');
         
    },
    Action: function () {
        if (Data.Mode == "Add") {
            Form.Save();
        }
        else {
            Form.Update();
        }
    },
    Save: function () {
       
        if ($("#formTransaction").parsley().validate())
        {
            var params = {
                ParamName: $("#tb2").val(), ParamValue: $("#tb3").val(), CreatedBy: $("#hdUsrFullName").val()

            }

            var l = Ladda.create(document.querySelector("#btnTrx"))
            $.ajax({
                url: "/api/MasterARRevSysParameter/save",
                type: "post",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false,
                beforeSend: function (xhr) {
                    l.start();
                }
            }).done(function (data, textStatus, jqXHR) {
                //if (Common.CheckError.Object(data)) {
                    
                //}
                $("#MdDetail").modal("hide");
                Form.Done();
                Common.Alert.Success("Data has been saved!")
                Table.LoadData();
                l.stop()
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l.stop()
            })
        }
       
    },
    Update: function () {
        if ($("#formTransaction").parsley().validate())
        {
            var params = {
                ID:$("#tb1").val(),
                   ParamName: $("#tb2").val(),
                   ParamValue: $("#tb3").val(),
                   CreatedDate: $("#tb4").val(),
                   CreatedBy: $("#tb5").val(),
                   UpdatedDate: $("#tb6").val(),
                   UpdatedBy: $("#hdUsrFullName").val()
                ///ParamValue: $("#tb3").val(), UpdatedBy: $("#hdUsrFullName").val()
            }

            var l = Ladda.create(document.querySelector("#btnTrx"))
            $.ajax({
                url: "/api/MasterARRevSysParameter/update",
                type: "post",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false,
                beforeSend: function (xhr) {
                    l.start();
                }
            }).done(function (data, textStatus, jqXHR) {
                $("#MdDetail").modal("hide");
                Form.Done();
                Common.Alert.Success("Data has been updated!")
                Table.LoadData();
                l.stop()
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l.stop()
            })
        }
       
    },
    //Delete: function (ID) {
    //    Data.ID = ID;
    //    var params = {
    //        model: Data
    //    }
    //    $.ajax({
    //        url: "/api/MasterARRevSysParameter/Delete",
    //        type: "post",
    //        dataType: "json",
    //        contentType: "application/json",
    //        data: JSON.stringify(params),
    //        cache: false,
    //    }).done(function (data, textStatus, jqXHR) {
    //        if (Common.CheckError.Object(data)) {
    //            Common.Alert.Success("Data has been deleted!")
    //            Table.LoadData();
    //        }
    //    })
    //    .fail(function (jqXHR, textStatus, errorThrown) {
    //        Common.Alert.Error(errorThrown)
    //    })
    //}
     ModalCancel: function (ID) {
        Form.Done();
        $("#formTransaction").parsley().reset()
        $("#MdDetail").modal("hide");
    }
    ,
     GenerateRASystem: function (flag) {
        
        swal({
            title: "Confirmation",
            text: "Are you sure?",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn blue",
            confirmButtonText: "Yes",
            closeOnConfirm: false
        },
        function () {            
            var params = {
                UpdatedBy: $("#hdUsrFullName").val(), Flag:flag
            }
            var url = "startjob";            
            $.ajax({
                url: "/api/MasterARRevSysParameter/startjob",
                type: "post",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false,
            }).done(function (data, textStatus, jqXHR) {
                var message= "Data has been generated!"
                if (flag == "stop")
                {
                    message = "Job has been stopped!"
                }
                Common.Alert.Success(message)
                Table.LoadData();
            })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        })
        });
    }

     
}

var Table = {
    Init: function () {
        //Table Summary
        //$(".panelSearchResult").hide();

        var tbList = $('#tblListData').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $("#tblListData tbody").unbind();

        $("#tblListData tbody").on("click", ".link-btEdit", function (e) {
            var table = $("#tblListData").DataTable();
            var row = table.row($(this).parents('tr')).data();
            Data.Mode = "Edit"
            $("#btnTrx").html("Update");
            $('.attrData').show();
            $("#formTransaction").parsley().reset()
            $("#tb2").prop('disabled', true);
            $("#tb1").val(row.ID);
            $("#tb2").val(row.ParamName);
            $("#tb3").val(row.ParamValue);
            $("#tb4").val(Common.Format.ConvertJSONDateTime(row.CreatedDate));
            $("#tb5").val(row.CreatedBy);
            $("#tb6").val(Common.Format.ConvertJSONDateTime(row.UpdatedDate));
            $("#tb7").val(row.UpdatedBy);
            $("#MdDetail").modal("show");
        });


        

    },

    LoadData: function () {
        //var l = Ladda.create(document.querySelector("#btnAddNewData"));
        //l.start();

        var params = { ID: 1 };

        var tbList = $("#tblListData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/MasterARRevSysParameter/GetList",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                       // l.start();
                        Table.Export()
                       // l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "order": [[1, 'asc']],
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
               // { data: "RowIndex" },
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        //if ($("#hdAllowEdit").val())
                        strReturn += "<i class='fa fa-edit btn btn-xs green link-btEdit'></i>";

                        return strReturn;
                    }
                },
                { data: "ParamName" },
                { data: "ParamValue" },                
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }, orderable: true
                },
                { data: "CreatedBy" },                
                {
                    data: "UpdatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }, orderable: true
                },
                { data: "UpdatedBy" },
            ],
            "columnDefs": [
                { "targets": [0], "orderable": false }
                //{ "targets": [1], "class": "text-center" }
                //{ "targets": [4], "visible": false }
            ],
            //"dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            //"fnDrawCallback": function () {
            //    if (Common.CheckError.List(tbList.data())) {
            //        //$(".panelSearchResult").fadeIn();
            //    }
            //    //l.stop();
            //}
        });       
    },

    Export: function () {
        alert('X');
        //var strSONumber = $("#").val()
        //window.location.href = "/Admin/MasterARRevSysParameter/Export?str=" + str;
    },
}

 

