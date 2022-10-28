Data = {};
UploadedData = [];
jQuery(document).ready(function () {
    //document.getElementById("idAmount").onblur = function () {

    //    var currencyFormat = document.getElementById('idAmount');

    //    if (Math.floor(this.value)) {
    //        currencyFormat.value = currency(this.value);
    //    }
    //    else {
    //        this.value = "0";
    //    }


    //};

    Form.Init();

    Form.search();

    $("#btCreate").unbind().click(function () {
        Form.Reset();
        Form.Create();
        $('#inSonumbTr').prop('readonly', false);

    });
    $("#btDownloadFile").unbind().click(function () {
        helper.Download();
    });

    $("#btUploadTr").unbind().click(function () {
        Transactions.UploadExcel();
    });

    $("#btReset").unbind().click(function () {
        location.reload();
    });

    $("#btUpload").unbind().click(function () {
        Form.Upload();

    });

    $("#btSearch").unbind().click(function () {
        Form.search();
    });


    $(".btCancel").unbind().click(function () {
        Form.Cancel();
    });

    $("#btSaveUpload").unbind().click(function () {
        Transactions.PostBulky();




    });


    $("#btSubmit").on('click', function () {
        $("#form").submit();
    });

    $("#form").on('submit', function (e) {
        if ($("#form").parsley().validate()) {
            if ($("#panelTransactionTitle").text() == "Create") {
                var stDate = new Date($("#slPeriodeStart").val())
                var edDate = new Date($("#slPeriodeEnd").val())

                if (stDate > edDate) {
                    Common.Alert.Warning("End Date must be greater or equal to Start Date !!!");
                }
                else {
                    var param = {
                        Sonumb: $("#inSonumbTr").val()
                    }

                    $.ajax({
                        type: "GET",
                        url: "/api/RevenueSystem/CheckAdjustmentCustomSonumbCount",
                        data: param,
                        success: function (data) {
                            if (data == 1) {
                                Transactions.Post();
                            }
                            else {
                                Common.Alert.Warning("Sonumb not exist !!!");
                            }
                        }
                    });
                }
            }
            else {
                var id = $("#hdID").val()
                Transactions.Put(id);
            }
        }

        e.preventDefault();
    });

    $('#tblData').on('click', '.btEdit', function (e) {
        var table = $('#tblData').DataTable();
        var tr = $(this).closest('tr');
        var data = table.row(tr).data();
        Form.SetData(data);

    });


    $('#tblData').on('click', '.btDelete', function (e) {
        var table = $('#tblData').DataTable();
        var tr = $(this).closest('tr');
        var data = table.row(tr).data();
        $("#hdIDDelete").val(data.ID);
        $('#mdlDelete').modal('show');
        //Transactions.Delete();

    });

    $("#btnDeleteTr").unbind().click(function () {
        $('#mdlDelete').modal('hide');
        Transactions.Delete();
    });


    //$("#btSubmit").unbind().click(function () {
    //    Form.Create();
    //});
});

function pad(str, max) {
    str = str.toString();
    return str.length < max ? pad("0" + str, max) : str;
}

function MonthYear(date) {
    var date = new Date(date);
    var year = date.getFullYear(date);
    var month = pad(date.getMonth() + 1, 2);
    return year.toString() + '-' + month.toString();
}


var Form = {
    Init: function () {
        $("#pnlTransaction").hide();
        $("#pnlUpload").hide();
        $("#form").parsley();

        //if (!$("#hdAllowAdd").val()) {
        //    $("#btCreate").hide();
        //}

    },
    drawUpload: function () {
        var oTable = $("#tblDtUpload").DataTable({
            "data": UploadedData,
            //"pageLength": 10,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            //"columnDefs": [{
            //    'orderable': false,
            //    'targets': [0, 1, 2]
            //}, {
            //    "searchable": true,
            //    "targets": [0, 1, 2]
            //}],
            "destroy": true,
            "columns": [
                { data: "Sonumb" },
                { data: "StartDate" },
                { data: "EndDate" },
                { data: "Amount", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                { data: "Remarks" },
                { data: "Type" }
            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                var param = {
                    Sonumb: aData.Sonumb
                }

                $.ajax({
                    type: "GET",
                    url: "/api/RevenueSystem/CheckAdjustmentCustomSonumbCount",
                    data: param,
                    success: function (data) {
                        if (data == 0) {
                            $('td', nRow).attr('style', 'background-color: #FF9999 !important;');
                            $('td', nRow).attr('class', 'error');
                        }

                    }
                });

            },
            "order": []
        });
    },

    search: function () {

        var selector = "#tblData";

        columns = [
                       {
                           orderable: false,
                           mRender: function (data, type, full) {
                               var strReturn = "";
                               var IDd = full.ID;
                               strReturn += "<button type='button' title='Update' class='btn btn-xs green btEdit' id='btEdit' name:" + IDd + " ><i class='fa fa-edit'></i></button>";
                               strReturn += "<button type='button' title='Delete' class='btn btn-xs green btDelete' id='btDelete'name:" + IDd + " ><i class='fa fa-trash'></i></button>";
                               return strReturn;
                           }
                       },
                      { data: "Sonumb" },
                      {
                          data: "StartDate", render: function (data) {
                              return Common.Format.ConvertJSONDateTime(data);
                          }
                      },
                      {
                          data: "EndDate", render: function (data) {
                              return Common.Format.ConvertJSONDateTime(data);
                          }
                      },
                      { data: "Amount", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                      { data: "Remarks" },
                      { data: "Type" }
        ]
        Form.draw(selector, columns);
    },

    draw: function (selector, columns) {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var param = {
            Sonumb: $("#inSonumb").val() == null ? "" : $("#inSonumb").val().toString().trim(),
            Type: $("#slTypeFilter").val() == null ? "" : $("#slTypeFilter").val().toString().trim()
        }

        var tblData = $("#tblData").DataTable({
            dom: 'Bfrtip',
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,

            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/Api/RevenueSystem/GetAdjustmentCustomToList",
                "type": "POST",
                "datatype": "json",
                "data": param,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                //{
                //    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                //        var l = Ladda.create(document.querySelector(".yellow"));
                //        l.start();
                //        //Form.Export(param)
                //        l.stop();
                //    }
                //},
                //{ extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }

            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "columns": columns,
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchZero").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

            },
            "order": [],
            //"createdRow": function (row, data, index) {
            //    $('td', this).removeClass('hover');
            //},
            "destroy": true
        });


    },

    Create: function () {
        Data.Mode = "Create";
        $("#pnlSummary").hide();
        $("#pnlUpload").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Create");
        $("#form").parsley().reset()
    },
    Upload: function () {
        Data.Mode = "Upload";
        $("#pnlSummary").hide();
        $("#pnlUpload").fadeIn();
        $("#pnlTransaction").hide();
        $(".panelupload").show();
        $("#panelupload").text("Upload");
        $("#formUpload").parsley().reset()
        $("#dvAll").hide();


    },

    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
        $("#pnlUpload").hide();
    },
    Done: function () {
        Form.Reset();
        $("#pnlSummary").fadeIn();
        $("#pnlUpload").hide();
        $("#pnlTransaction").hide();
        $(".panelSearchBegin").fadeIn();
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        Form.search();
    },
    Reset: function () {
        document.getElementById("form").reset();
    },
    SetData: function (data) {
        $("#pnlSummary").hide();
        $("#pnlUpload").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Update");

        $("#inSonumbTr").val(data.Sonumb);



        var stDate = MonthYear(data.StartDate)
        var endDate = MonthYear(data.EndDate)
        var sdsds = data.Type;



        $("#slPeriodeStart").val(stDate);
        $("#slPeriodeEnd").val(endDate);
        $("#idAmount").val(data.Amount);
        $("#inRemarks").val(data.Remarks);
        $("#slType").val(sdsds);
        $("#hdID").val(data.ID);

        $('#inSonumbTr').prop('readonly', true);

    }
};

var Transactions = {
    Post: function () {
        var a = $("#idAmount").val();
        var params = {
            Sonumb: $("#inSonumbTr").val(),
            StartDate: $("#slPeriodeStart").val(),
            EndDate: $("#slPeriodeEnd").val(),
            Amount: $("#idAmount").val(),
            Remarks: $("#inRemarks").val(),
            Type: $("#slType").val()

        }

        var l = Ladda.create(document.querySelector("#btSubmit"));
        $.ajax({
            url: "/Api/RevenueSystem/CreateAdjustmentCustom",
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

                Form.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    },
    Put: function (id) {
        var params = {
            ID: id,
            Sonumb: $("#inSonumbTr").val(),
            StartDate: $("#slPeriodeStart").val(),
            EndDate: $("#slPeriodeEnd").val(),
            Amount: $("#idAmount").val(),
            Remarks: $("#inRemarks").val(),
            Type: $("#slType").val()

        }

        var l = Ladda.create(document.querySelector("#btSubmit"));
        $.ajax({
            url: "/Api/RevenueSystem/UpdateAdjustmentCustom",
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
                Common.Alert.Success("Data has been updated!");

                Form.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    },
    Delete: function () {
        var params = {
            ID: $("#hdIDDelete").val(),
            Sonumb: "",
            StartDate: "",
            EndDate: "",
            Amount: "",
            Remarks: "",
            Type: "",
        }
        var l = Ladda.create(document.querySelector("#btnDeleteTr"));
        $.ajax({
            url: "/Api/RevenueSystem/DeleteAdjustmentCustom",
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
                Common.Alert.Success("Data has been deleted!");
                Form.search();
                //Form.Done();
            }
            l.stop()
        })
      .fail(function (jqXHR, textStatus, errorThrown) {
          Form.search();
          Common.Alert.Error(errorThrown)
          l.stop()
      })

        $('#mdlDelete').modal('hide');
    },
    UploadExcel: function () {
        var formData = new FormData(); //FormData object  
        var fileInput = document.getElementById("fileToUpload1");
        if (fileInput.files[0] != undefined && fileInput.files[0] != null) {
            var fileName = fileInput.files[0].name;
            var extension = fileName.split('.').pop().toUpperCase();
            if (extension != "XLS" && extension != "XLSX") {
                Common.Alert.Warning("Please upload an Excel File.");
            }
            else {
                for (i = 0; i < fileInput.files.length; i++) {
                    formData.append(fileInput.files[i].name, fileInput.files[i]);
                }


                var e = Ladda.create(document.querySelector("#btUploadTr"));
                $.ajax({
                    url: '/RevenueSystem/RevSys/UploadExcelAdjustmentCustom',
                    type: 'POST',
                    data: formData,
                    async: false,
                    beforeSend: function (xhr) {
                        e.start();
                    },
                    cache: false,
                    contentType: false,
                    processData: false
                }).done(function (data, textStatus, jqXHR) {
                    if (data !== "Exception") {
                        if (data.length <= 0) {
                            $(".fileinput").fileinput("clear");
                            Common.Alert.Warning("The uploaded file is invalid or empty. Please use the available template, and fill the file correctly with valid product data.");
                        } else {
                            $(".fileinput").fileinput("clear");

                            for (var i = 0, l = data.length; i < l; i++) {
                                var st = new Date(parseInt((data[i].StartDate).replace('/Date(', '')));
                                var dayst = pad((st.getDate()), 2);
                                var monthst = pad((st.getMonth() + 1), 2);
                                var yearst = st.getFullYear();

                                var ed = new Date(parseInt((data[i].EndDate).replace('/Date(', '')));
                                var dayed = pad((ed.getDate()), 2);
                                var monthed = pad((ed.getMonth() + 1), 2);
                                var yeared = ed.getFullYear();



                                data[i].StartDate = yearst + '-' + monthst + '-' + dayst;
                                data[i].EndDate = yeared + '-' + monthed + '-' + dayed;
                            }

                            //var ddd = ;
                            UploadedData = data;
                            $("#dvAll").show();
                            Form.drawUpload();
                            e.stop()
                        }
                    } else {
                        $(".fileinput").fileinput("clear");
                        Common.Alert.Warning("The uploaded file is invalid. Please download the latest template by clicking the Download Template button.");
                    }
                    e.stop();
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    Common.Alert.Error(errorThrown)
                    e.stop()
                });

            }
        } else {
            Common.Alert.Warning("Please upload an Excel File.");
        }
    },
    PostBulky: function () {
        var params = [];
        var param = {}

        $.each(UploadedData, function (index, item) {
            param = new Object();


            var paramsonumb = {
                Sonumb: item.Sonumb
            }

            $.ajax({
                type: "GET",
                url: "/api/RevenueSystem/CheckAdjustmentCustomSonumbCount",
                data: paramsonumb,
                async: false,
                success: function (data) {
                    if (data == 1) {
                        param = {
                            Sonumb: item.Sonumb,
                            StartDate: item.StartDate,
                            EndDate: item.EndDate,
                            Amount: item.Amount,
                            Remarks: item.Remarks,
                            Type: item.Type,
                        }
                        params.push(param);
                    }
                }
            });

        });

        console.log(params);

        var l = Ladda.create(document.querySelector("#btSaveUpload"));
        $.ajax({
            url: "/api/RevenueSystem/BulkAdjustmentCustomCreate",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            },
            async: false
        }).done(function (data, textStatus, jqXHR) {
            //$('#mdlUploadedTaxInvoice').modal('hide');
            $("#pnlSummary").fadeIn();
            $("#pnlUpload").hide();
            $("#pnlTransaction").hide();
            Common.Alert.Success("Data Adjustment Custom have been submited successfully!");
          
            //Table.Reset();
            //Form.Done();
            l.stop()
            Form.search();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        });

    },
};


function currency(amount, prefix) {
    var number_string = amount.replace(/[^,\d]/g, '').toString(),
        split = number_string.split(','),
        s = split[0].length % 3,
        c = split[0].substr(0, s),
        f = split[0].substr(s).match(/\d{1,3}/gi);

    if (f) {
        separator = s ? '.' : '';
        c += separator + f.join('.');
    }

    c = split[1] != undefined ? c + ',' + split[1] : c;
    return prefix == undefined ? c : (c ? 'Rp. ' + c : '');
}


var helper = {
    Download: function () {
        window.location.href = "/RevenueSystem/DownloadTemplateAdjustmentCustom?fileName=" + "TemplateAdjustmentCustom.xlsx";
    }
}

//function limitCharacter(event) {
//    key = event.which || event.keyCode;
//    if (key != 188 // Comma
//         && key != 8 // Backspace
//         && key != 17 && key != 86 & key != 67 // Ctrl c, ctrl v
//         && (key < 48 || key > 57) // Non digit
//        // Dan masih banyak lagi seperti tombol del, panah kiri dan kanan, tombol tab, dll
//        ) {
//        event.preventDefault();
//        return false;
//    }
//}