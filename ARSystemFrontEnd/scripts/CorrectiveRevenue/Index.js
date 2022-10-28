var isDeleteAll = false;
jQuery(document).ready(function () {
    Data = {}
    RowSelected = {};
    TempData = [];
    DataBind.Month();
    DataBind.Year();
    DataBind.GetPeriod();
    DataBind.DataReadyProcess();
    $('.pnlErrorLog').hide();
    $('#btnGenerateData').unbind().click(function () {
        $('.pnlErrorLog').css('visibility', 'hidden');
        $('.pnlErrorLog').hide();
        Form.UploadData();
    });
    $('#btnDownloadTemplate').unbind().click(function () {
        Control.DownloadTemplate();
    });
    $('#btnSaveData').unbind().click(function () {
        $('.pnlErrorLog').css('visibility', 'hidden');
        $('.pnlErrorLog').hide();
        var table = $("#tblGenerateData").DataTable().data();
        if (table.length > 0) {
            swal({
                title: "Warning",
                text: "Are you sure ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, Continue!",
                closeOnConfirm: true,
                cancelButtonText: "No",
            }, function (e) {
                if (e) {
                    Form.ProcessData();
                }

            });
        } else {
            Common.Alert.Warning('Please upload data first!');
        }
    });
    $('#btnDeleteData').unbind().click(function () {
        $('.pnlErrorLog').css('visibility', 'hidden');
        $('.pnlErrorLog').hide();
        if (TempData.length > 0) {
            swal({
                title: "Warning",
                text: "Are you sure ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, Continue!",
                closeOnConfirm: true,
                cancelButtonText: "No",
            }, function (e) {
                if (e) {
                    Form.Delete();
                }

            });
        } else {
            Common.Alert.Warning('Please select data first!.');
        }
    });
    $("#cbALLData").unbind().change(function () {
        var table = $("#tblGenerateData").DataTable().data();
        if ($('#cbALLData input[type=checkbox]').prop('checked')) {
            isDeleteAll = true;
            $.each(table, function (i, item) {

                if (Control.IsEmptyData(item.Id)) {
                    var DataRows = {};
                    DataRows.Id = item.Id;
                    TempData.push(DataRows);
                }
            });
            $(".checkboxes").prop('checked', true);
        } else {
            isDeleteAll = false;
            $(".checkboxes").prop('checked', false);
            $.each(table, function (i, item) {
                Control.DeleteRow(item.Id);
            });
        }
        //console.log(TempData);

    });
});
var Form = {
    UploadData: function () {
        var fileInput = document.getElementById("FileUpload");

        if (fileInput.files[0] != undefined && fileInput.files[0].type == 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet') {

            var formData = new FormData();
            formData.append("Month", $('#Month').val());
            formData.append("Year", $('#Year').val());
            formData.append("File", fileInput.files[0]);
            var l = Ladda.create(document.querySelector("#btnGenerateData"))
            $.ajax({
                url: "/api/CorrectiveRevenue/UploadData",
                type: "POST",
                dataType: "json",
                contentType: false,
                data: formData,
                processData: false,
                beforeSend: function (xhr) {
                    l.start();
                }
            }).done(function (data, textStatus, jqXHR) {
                TempData = [];
                isDeleteAll = false;
                $('#FileUpload').val('');
                l.stop();
                if (Common.CheckError.List(data)) {
                    Common.Alert.Success("Data has been generated.");
                    DataBind.DataReadyProcess();
                    $('.pnlErrorLog').css('visibility', 'hidden');
                    $('.pnlErrorLog').hide();
                } else {
                    DataBind.LogError(data);
                    $('.pnlErrorLog').css('visibility', 'visible');
                    $('.pnlErrorLog').show();
                }
                // console.log(data);

            })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    Common.Alert.Error(errorThrown);
                    l.stop()
                });
        } else {
            Common.Alert.Warning('Please Select File Excel!');
        }
    },
    ProcessData: function () {

        var l = Ladda.create(document.querySelector("#btnSaveData"))
        $.ajax({
            url: "/api/CorrectiveRevenue/ProcessData",
            type: "GET",
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {

            setTimeout(function () {
                if (Common.CheckError.Object(data)) {
                    Common.Alert.Success('Data has been successfull generated.');
                    TempData = [];
                    DataBind.InitTable();
                    DataBind.DataReadyProcess();
                }
                l.stop();
            }, 2000);
        })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
                l.stop()
            });

    },
    Delete: function () {

        var l = Ladda.create(document.querySelector("#btnDeleteData"))
        $.ajax({
            url: "/api/CorrectiveRevenue/Delete",
            type: "POST",
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ RowID: TempData, AllDelete: isDeleteAll }),
            processData: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {

            setTimeout(function () {
                if (Common.CheckError.Object(data)) {
                    Common.Alert.Success('Data has been deleted.');
                    DataBind.InitTable();
                    DataBind.DataReadyProcess();
                    TempData = [];
                }
                l.stop();
            }, 2000);


        })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
                l.stop()
            });

    },
    ClearFormUpload: function () {

    },
}
var DataBind = {
    Month: function () {
        var id = "#Month";
        var Month = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']
        var monthValue = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

        var thisMonthInt = new Date().getMonth() + 1;
        var thisMonth = monthValue[thisMonthInt];
        for (var i = 0; i < monthValue.length; i++) {
            if (i == thisMonthInt) {
                $(id).append('<option value="' + monthValue[i] + '" selected>  ' + Month[i] + '  </option>');
            }
            else {
                $(id).append('<option value="' + monthValue[i] + '">  ' + Month[i] + '  </option>');
            }
        }


        $(id).select2({ placeholder: Month[thisMonth], width: null });
        $(id).val(thisMonth);
    },
    Year: function () {
        var start_year = new Date().getFullYear();
        var id = "#Year";
        var yearNow = new Date();


        for (var i = start_year - 10; i < start_year; i++) {
            $(id).append('<option value="' + i + '">  ' + i + '  </option>');
        }

        for (var i = start_year; i < start_year + 20; i++) {
            $(id).append('<option value="' + i + '">  ' + i + '  </option>');
        }

        $(id).select2({ placeholder: "Select Year", width: null });
        $(id).val(yearNow.getFullYear()).trigger('change');
    },
    InitTable: function (id) {
        $(id).dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $(id).DataTable().columns.adjust().draw();
        });
    },
    DataReadyProcess: function () {
        var idTbl = '#tblGenerateData';

        $(idTbl).DataTable({
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/CorrectiveRevenue/GetDataGenerated",
                "type": "Get",
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        DataBind.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    mRender: function (data, type, full) {
                        return "<label id='" + full.Id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input id='cb_" + full.Id + "' type='checkbox' class='checkboxes' disable/><span></span></label>";
                    }

                },
                { data: "RowIndex" },
                { data: "SoNumber" },
                { data: "SiteId" },
                { data: "SiteName" },
                { data: "TotalAdjusment", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "RemarkAdjusment" },
                { data: "AdjPPSNormalRevenue", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AdjPPSNetRevenue", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "NormalRevenue", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "NetRevenue", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "BalanceAccrue", render: $.fn.dataTable.render.number(',', '.', 2, '') },
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {

                App.unblockUI('.panelSearchResult');
                $('#cbALLData input[type=checkbox]').prop('checked', false);
                Control.SelectAllData();
            },
            'ordering': false,
            'order': false,
            "columnDefs": [{ "targets": [0], "class": "text-center" }],
        });

        $(idTbl).on('change', 'tbody tr .checkboxes', function () {
            var id = $(this).parent().attr('id');
            if (this.checked) {
                Control.SelectData(id);
            } else {
                Control.DeleteRow(id);
            }

        });
    },
    Export: function () {
        window.location.href = '/CorrectiveRevenue/ExportDataGenerated';
    },
    GetPeriod: function () {
        $.ajax({
            url: "/api/CorrectiveRevenue/GetPeriod",
            type: "GET",
            async: false

        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                $('#Month').val(data.MonthPeriod).trigger('change');
                $('#Year').val(data.YearPeriod).trigger('change');
            }
        });
    },
    GetListId: function () {
        //for CheckAll Pages
        var result = [];
        $.ajax({
            url: "/api/CorrectiveRevenue/GetListId",
            type: "Get",
            cache: false,
            async: false,
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                result = data;
            }
        })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
                result = [];
            });
        return result;
    },
    LogError: function (dataLogs) {
        $('#tblErrorLogs').DataTable({
            "proccessing": true,
            "serverSide": false,
            "data": dataLogs,
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [

                { data: "ErrorMessage" },

            ],
            'ordering': false,
            'order': false,
            "columnDefs": [{ "targets": [0], "class": "text-left" }],
        });

    }
}
var Control = {
    DownloadTemplate: function () {
        window.location.href = '/CorrectiveRevenue/DownloadExcelTemplate';
    },
    IsElementExistsInArray: function (id, arr) {
        var isExist = false;
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].Id == id) {
                isExist = true;
                break;
            }
        }
        return isExist;
    },

    DeleteRow: function (rowID) {
        $("#cb_" + rowID).prop('checked', false);
        var index = TempData.findIndex(function (data) {
            return data.Id == rowID
        });
        TempData.splice(index, 1);

        //console.log(TempData);
    },
    SelectData: function (rowID) {

        if (Control.IsEmptyData(rowID)) {
            $("#cb_" + rowID).prop('checked', true);
            var DataRows = {};
            DataRows.Id = rowID;
            TempData.push(DataRows);
            // console.log(TempData);
        }
    },
    SelectAllData: function () {
        if (TempData.length > 0) {
            $.each(TempData, function (i, item) {
                $("#cb_" + item.Id).prop('checked', true);
            })
        }

    },
    IsEmptyData: function (rowId) {
        var result = true;
        if (TempData.length > 0) {
            for (var i = 0; i < TempData.length; i++) {
                if (TempData[i].Id == rowId) {
                    result = false;
                    break;
                }
            }
        }
        return result;
    },
}