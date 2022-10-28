Data = {};
UploadedData = [];

jQuery(document).ready(function () {
    Form.Init();
    $("#btDownloadTemplate").unbind().click(function () {
        var cat = $('input[name=rbcategory]:checked').val();
        if (cat == undefined || cat == "") {
            Common.Alert.Warning("Please Check Category.");
        }
        else {
          helper.Download(cat);
        }

    });

    $("#btUpload").unbind().click(function () {
        var cat = $('input[name=rbcategory]:checked').val();

        if (cat == undefined || cat == "") {
            Common.Alert.Warning("Please Check Category.");
        }
        else {
            Form.UploadExcel();
        }
    });

    $("#btReset").unbind().click(function () {

    });

    $("#btSubmit").unbind().click(function () {
        var valCategory = $('input[name=rbcategory]:checked').val();
        Process.AddData(valCategory);
    });

    $('input[type="radio"]').each(function () {
        $(this).removeAttr('checked');
        $('input[type="radio"]').prop('checked', false);
        $('#dvAll').hide();
        $('#dvExecution').hide();

    })

    $('input[name=rbcategory]').on('change', function () {
        var cat = $('input[name=rbcategory]:checked').val();
        if (cat == undefined || cat == "") {
            $('#dvAll').hide();
        }
        else if (cat == "Accrue") {
            $('#dvAll').show();
            $('#dvtblAccrue').show();
            $('#dvtblOver').hide();
        }
        else {
            $('#dvAll').show();
            $('#dvtblOver').show();
            $('#dvtblAccrue').hide();


        }
        $('#dvExecution').hide();
    });

});

var Process = {
    AddData: function (valCategory) {
        var l = Ladda.create(document.querySelector("#btSubmit"))

        var params;
        if (valCategory == "Accrue") {
            params = {
                year: UploadedData[0]["year"].toString(),
                month: UploadedData[0]["month"].toString(),
                Category: valCategory
            }
        }
        else {
            params = {
                Category: valCategory
            }
        }


        $.ajax({
            type: "POST",
            url: "/api/RevenueSystem/SaveRevSysUploadPerDept",
            data: params,
            datatype: "json",
            cache: false,
            success: function (data) {
                if (valCategory == "Accrue")
                    Common.Alert.Success("Data Accrue has been submitted");
                else if (valCategory == "Overblast")
                    Common.Alert.Success("Data Overblast has been submitted");
                else
                    Common.Alert.Success("Data Overquota has been submitted");

                $('#tblAccrue tbody').remove();
                $('#tblOver tbody').remove();

                $(".panelSearchZero").hide();
                $(".panelSearchResult").hide();

                $('#dvExecution').hide();
            },
            error: function (xhr) {
                if (valCategory == "Accrue")
                    Common.Alert.Success("Insert Data Accrue Failed.");
                else if (valCategory == "Overblast")
                    Common.Alert.Success("Data Overblast Failed");
                else
                    Common.Alert.Success("Data Overquota Failed");

                $('#dvExecution').hide();
            }
        });
    },
    DeleteDataCategory: function (valCategory) {

        $('#tblAccrue tbody').remove();
        $('#tblOver tbody').remove();

        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();

        $('#dvExecution').hide();
    }
}


var Form = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        //var tblSummaryAccrue= $('#tblAccrue').dataTable({
        //    "filter": false,
        //    "destroy": true,
        //    "data": []
        //});

        //var tblSummaryOver = $('#tblOver').dataTable({
        //    "filter": false,
        //    "destroy": true,
        //    "data": []
        //});

        $('input[type="radio"]').each(function () {
            $(this).removeAttr('checked');
            $('input[type="radio"]').prop('checked', false);
            $('#dvAll').hide();
        })

        $('#tblAccrue tbody').remove();
        $('#tblOver tbody').remove();
    },
    bind: function (valCategory) {
        var selector = "";
        var columns;
        var Category = valCategory;
        if (valCategory == "Accrue") {
            $('#tblOver tbody').remove();
            columns = [
                         { data: "Division", className: "text-center" },
                         { data: "Sonumb", className: "text-center" },
                         { data: "SiteID", className: "text-center" },
                         { data: "SiteName", className: "text-center" },
                         { data: "Operatorid", className: "text-center" },
                         { data: "Status", className: "text-center" },
                         { data: "CompanyID", className: "text-center" },
                         { data: "SldDate", className: "text-center" },
                         { data: "Accrue", className: "text-right", render: $.fn.dataTable.render.number('.', ',', 0, '') },
                         { data: "PICA", className: "text-center" },
                         { data: "PicaDetail", className: "text-center" },
                         { data: "PicaSuggest", className: "text-center" },
                         { data: "Remaks", className: "text-center" }
            ];
            selector = "tblAccrue";
        }
        else {
            $('#tblAccrue tbody').remove();
            columns = [
                         { data: "Sonumb", className: "text-center" },
                         { data: "type", className: "text-center" },
                         { data: "SiteID", className: "text-center" },
                         { data: "SiteName", className: "text-center" },
                         { data: "Operatorid", className: "text-center" },
                         { data: "StartDate", className: "text-center" },
                         { data: "EndDate", className: "text-center" },
                         { data: "Status", className: "text-center" },
                         { data: "CompanyID", className: "text-center" },
                         { data: "currency", className: "text-center" },
                         { data: "amount", className: "text-center" },
                         { data: "PeriodeStartDate", className: "text-center" },
                         { data: "PeriodeEndDate", className: "text-center" },
                         { data: "Remaks", className: "text-center" }
            ];
            selector = "tblOver";
        }

        Form.draw(selector, columns, Category);
    },
    draw: function (selector, columns, Category) {
        var l = Ladda.create(document.querySelector("#btUpload"))
        l.start();

        var params;
        var grid;
        if (Category == "Accrue") {
            params = {
                Category: Category.toString().trim(),
                year: UploadedData[0]["year"].toString(),
                month: UploadedData[0]["month"].toString(),
                week: UploadedData[0]["week"].toString()
            }
            grid = "#tblAccrue";
        }
        else {
            params = {
                Category: Category.toString().trim()
            }
            grid = "#tblOver";
        }




        var tblData = $(grid).DataTable({
            dom: 'Bfrtip',
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/Api/RevenueSystem/GetRevSysDataAccruePerDept",
                "type": "POST",
                "datatype": "json",
                "data": params
            },
            buttons: [
               { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
               {
                   text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                       var l = Ladda.create(document.querySelector(".yellow"));
                       l.start();
                       //Form.Export(param)
                       l.stop();
                   }
               },
               { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }

            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "columns": columns,
            //"columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblData.data())) {
                    $(".panelSearchResult").fadeIn();


                }
                l.stop(); App.unblockUI('.panelSearchResult');
            },
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (aData.Remaks != '') {
                    $('td', nRow).css('background-color', '#FF9999');
                    $('#dvExecution').hide();
                }
                else {
                    $('#dvExecution').show();
                }
                l.stop();
            },
            "order": [],
            "destroy": true
        })
    },
    UploadExcel: function () {
        var formData = new FormData(); //FormData object  
        var fileInput = document.getElementById("fileToUpload1");
        var valCategory = $('input[name=rbcategory]:checked').val();
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

                formData.append("Category", valCategory);

                //$.ajax({
                //    type: "get",
                //    url: "/Admin/RevSys/setCategory",
                //    data: valCategory,
                //    datatype: "json",
                //    cache: false
                //});


                var l = Ladda.create(document.querySelector("#btUpload"));
                $.ajax({
                    url: '/RevenueSystem/RevSys/UploadExcel',
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
                    if (data !== "Exception") {
                        if (data.length <= 0) {
                            $(".fileinput").fileinput("clear");
                            Common.Alert.Warning("The uploaded file is invalid or empty. Please use the available template, and fill the file correctly with valid product data.");
                        } else {
                            $(".fileinput").fileinput("clear");
                            UploadedData = data;
                            Form.bind(valCategory);

                        }
                    } else {
                        $(".fileinput").fileinput("clear");
                        Common.Alert.Warning("The uploaded file is invalid. Please download the latest template by clicking the Download Template button.");
                    }
                    l.stop();
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    Common.Alert.Error(errorThrown)
                    l.stop()
                });

            }
        } else {
            Common.Alert.Warning("Please upload an Excel File.");
        }
    }
}

var helper = {
    Download: function (category) {
        var fileDocName;
        if (category == "Accrue")
            fileDocName = "UploadAccruePerDept.xlsx";
        else
            fileDocName = "Template_Overquota_or_Overblast.xlsx";


        window.location.href = "/RevenueSystem/DownloadPageAccruePerDept?fileName=" + fileDocName;
    }
}