Data = {};

jQuery(document).ready(function () {
    Form.Init();
    Table.LoadData();
});

var Form = {
    Init: function () {

        Table.Init();
        $("#sRESlip").val('');

        if (!$("#hdAllowAdd").val())
            $(".btnAddNewData").hide();

        $("#btnAddNewData").unbind().click(function () {
            Form.Done();
            $("#mdlRESlipUpload").modal('show');
        });

        $("#btnTrxSave").unbind().click(function () {
            Form.SaveRESlip();
        });
        $("#btnTrxCancel").unbind().click(function () {
            Form.Done();
        });

        $("#btSearch").unbind().click(function () {
            Table.LoadData();
        });

        $("#btReset").unbind().click(function () {
            $("#sSONumber").val("");
        });

        Control.BindParameter("PPH", "#iPPH");
        Control.BindParameter("PPN", "#iPPN");

        Common.FileValidation("formRESlip");
        $("#btnUpload").unbind().click(function () {
            if ($('#formRESlip').parsley().validate()) {
                Form.UploadExcel();
            }

            //Common.Function.FileValidation("formRESlip")
            //$("#btnUpload").unbind().click(function () {
            //    if ($('#formRESlip').parsley().validate()) {
            //        Form.UploadExcel();
            //    }
            //})

        })

        $("#btnDownloadTemplate").unbind().click(function () {
            Form.DownloadTemplate();
        });

    },

    Done: function () {
        $("#idTrx").val('');
        $("#iRESlip").val('');
        $("#iSONumber").val('');
        $("#FileUpload").val('');
    },

    SaveRESlip: function () {
        Data.ID = $("#idTrx").val();
        Data.RESlip = $("#iRESlip").val();
        Data.SONumber = $("#iSONumber").val();

        var params = {
            model: Data
        }

        var l = Ladda.create(document.querySelector("#btnTrxSave"))
        $.ajax({
            url: "/api/RESlip/Save",
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
                $("#mdlRESlip").modal("hide");
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

    DeleteRESlip: function (ID) {
        Data.ID = ID;
        var params = {
            model: Data
        }
        $.ajax({
            url: "/api/RESlip/Delete",
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

    DownloadTemplate: function () {
        window.location.href = "/Admin/RESlip/DownloadExcelTemplate";
    },
    UploadExcel: function () {
        var formData = new FormData(); //FormData object  
        var fileInput = document.getElementById("FileUpload");
        if (fileInput.files[0] != undefined && fileInput.files[0] != null) {
            var fileName = fileInput.files[0].name;
            var extension = fileName.split('.').pop().toUpperCase();
            if (extension != "XLS" && extension != "XLSX") {
                Common.Alert.Warning("Please upload an Excel File.");
                $("#mdlRESlipUpload").modal('show');
            }
            else {
                for (i = 0; i < fileInput.files.length; i++) {
                    formData.append(fileInput.files[i].name, fileInput.files[i]);
                }

                var l = Ladda.create(document.querySelector("#btnUpload"));
                $.ajax({
                    url: '/Admin/RESlip/ImportExcel',
                    type: 'POST',
                    data: formData,
                    async: false,
                    beforeSend: function (xhr) {
                        l.start();
                    },
                    cache: false,
                    contentType: false,
                    processData: false
                }).done(function (data, textStatus, jqXHR) {
                    if (data != "") {
                        Common.Alert.Warning(data);
                    } else {
                        Common.Alert.Success("Upload data success!");
                        Table.LoadData();
                    }
                    Form.Done();
                    l.stop();
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    Common.Alert.Error(errorThrown)
                    l.stop()
                });
            }
        } else {
            Common.Alert.Warning("Please upload an Excel File.");
            $("#mdlRESlipUpload").modal('show');
        }
    }
}

var Table = {
    Init: function () {
        //Table Summary
        $(".panelSearchResult").hide();

        var tblRESlip = $('#tblRESlip').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });


    },

    LoadData: function () {
        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();

        var params = { SONumber: $("#sSONumber").val() };

        var tblRESlip = $("#tblRESlip").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/RESlip/GetList",
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
                { data: "SONumber" },
                { data: "RESlip" },
                { data: "ID" },
            ],
            "columnDefs": [
                { "targets": [0], "orderable": false },
                { "targets": [1], "class": "text-center" },
                { "targets": [4], "visible": false }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblRESlip.data())) {
                    $(".panelSearchResult").fadeIn();
                }
                l.stop();
            }
        });

        $("#tblRESlip tbody").unbind();
        $("#tblRESlip tbody").on("click", ".link-btEdit", function (e) {
            var table = $("#tblRESlip").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $("#idTrx").val(row.ID);
            $("#iRESlip").val(row.RESlip);
            $("#iSONumber").val(row.SONumber)
            $("#mdlRESlip").modal("show");
        });
    },

    Export: function () {

        var strSONumber = $("#sSONumber").val()
        window.location.href = "/Admin/RESlip/Export?strSONumber=" + strSONumber;
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

    FileValidation: function (id) {
        var app = app || {};

        // Utils
        (function ($, app) {
            'use strict';
            app.utils = {};
            app.utils.formDataSuppoerted = (function () {
                return !!('FormData' in window);
            }());
        }(jQuery, app));
        //Parsley validators
        (function ($, app) {
            'use strict';
            window.Parsley.addValidator('filemaxmegabytes', {
                requirementType: 'string',
                validateString: function (value, requirement, parsleyInstance) {

                    if (!app.utils.formDataSuppoerted) {
                        return true;
                    }
                    var file = parsleyInstance.$element[0].files;
                    var maxBytes = requirement * 1048576;
                    if (file.length == 0) {
                        return true;
                    }
                    return file.length === 1 && file[0].size <= maxBytes;
                }
            }).addValidator('filemimetypes', {
                requirementType: 'string',
                validateString: function (value, requirement, parsleyInstance) {

                    if (!app.utils.formDataSuppoerted) {
                        return true;
                    }
                    var file = parsleyInstance.$element[0].files;

                    if (file.length == 0) {
                        return true;
                    }
                    var allowedMimeTypes = requirement.replace(/\s/g, "").split(',');
                    return allowedMimeTypes.indexOf(file[0].type) !== -1;
                },
            });

        }(jQuery, app));
        // Parsley Init
        (function ($, app) {
            'use strict';
            $("#" + id).parsley();
            return true;
        }(jQuery, app));
    },


}

