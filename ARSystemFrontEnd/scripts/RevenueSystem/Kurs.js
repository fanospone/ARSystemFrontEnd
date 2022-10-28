$(function () {
    Control.Init();
});

var Control = {
    Init: function () {
        $('#addNewKurs').on('click', function () {
            Control.InitOpenModal();
            Temp.KursStartDate = null
            Temp.KursEndDate = null
            Temp.ID = 0;
        });

     
        $('.btnSearch').on('click', function () {
            Table.KursIDR.Search();
        });
        //$('#btnAddSonumbInflasi').on('click', function () {
        //    $('#mdlAddSonumb').modal('show');
        //});
        Table.KursIDR.Search();
        Widget.Button.btnSave();
        Widget.Button.btnReset();
    },
    InitOpenModal: function(){
        var htmlscript = $('script#formCreateEditKurs').html();
        $('#createEditKurs').html(htmlscript);

        Control.InitCreateEditKurs();
        $('#mdlCreateEditKurs').modal('show');
    },
    InitCreateEditKurs: function () {
        $('#txtStartDate').datepicker({
            todayHighlight: true,
            format: 'dd-M-yyyy',
            autoclose: true,
            orientation: 'bottom left',
        });
        $('#txtEndDate').datepicker({
            todayHighlight: true,
            format: 'dd-M-yyyy',
            autoclose: true,
            orientation: 'bottom left',
        });
        $('#tbxSearchStartPeriode').datepicker({
            todayHighlight: true,
            format: 'dd-M-yyyy',
            autoclose: true,
            orientation: 'bottom left',
        });
        $('#tbxSearchEndPeriode').datepicker({
            todayHighlight: true,
            format: 'dd-M-yyyy',
            autoclose: true,
            orientation: 'bottom left',
        });
        $("#fileAttach").on("change", function () {
            var fileName = $(this).val().split("\\").pop();
            $("#txtAttachment").text(fileName);
        });
        $('#txtKursIDR').change(function () {
            var val = this.value;
            $("#txtKursIDR").val(Common.Format.FormatCurrency(val))
        })

        Widget.Button.btnSave();
        Widget.Button.btnReset();
    }
}

var Table = {
    KursIDR: {
        Init: function () {
            $('#tblSummary').dataTable({
                columns: Table.KursIDR.Columns,
            });
        },
        Columns: [
            {
                data: null, fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    //console.log(oData);
                    var _link = "";
                    _link += "<button type='button' title='Edit' class='btn btn-xs blue btnEdit' data-StartDate='" + moment(oData.StartDate).format('DD-MMM-YYYY') + "' data-EndDate='" + moment(oData.EndDate).format('DD-MMM-YYYY') + "' data-Kurs='" + Number(oData.Kurs).toFixed(2) + "' data-FileName='" + oData.FileName + "'><i class='fa fa-edit'></i>&nbsp;Edit</button>";
                    _link += "<button type='button' title='Delete' class='btn btn-xs blue btnDelete' data-StartDate='" + oData.StartDate + "' data-EndDate='" + oData.EndDate + "'><i class='fa fa-trash'></i>&nbsp;Delete</button>";
                    $(nTd).html(_link);
                },
                className: "dt-center",
                width: "20%"
            },
            {
                data: "StartDate",
                render: function (data) {
                    return moment(data).format('DD-MMM-YYYY');
                },
                width: "20%", className: "dt-center",
            },
            {
                data: "EndDate",
                render: function (data) {
                    return moment(data).format('DD-MMM-YYYY');
                },
                width: "20%", className: "dt-center",
            },
            {
                data: "Kurs", className: 'dt-right',
                render: function (data) {
                    return 'Rp.' + Number(data).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                },
                width: "20%"
            },
            {
                data: "FileName", mRender: function (data, type, full) {
                    return full.FilePath == null ? "" : "<center><button type='button' href='' class='btDownloadAttachment btn btn-xs blue' data-FileName='" + full.FileName + "' data-FilePath='" + full.FilePath + "' data-ContentType='" + full.ContentType + "'><i class='fa fa-download'></i>&nbsp;Download</button></center>";
                },
                width: "20%"
            }
        ],
        Search: function () {
            var params = {
                StartDate: $("#tbxSearchStartPeriode").val() != "undefined" ? $("#tbxSearchStartPeriode").val().replace(/'/g, "''") : null,
                EndDate: $("#tbxSearchEndPeriode").val() != "undefined" ? $("#tbxSearchEndPeriode").val().replace(/'/g, "''") : null
            }
            var tblSummary = $("#tblSummary").DataTable({
                "orderCellsTop": true,
                "proccessing": true,
                "serverSide": false,
                "language": {
                    "emptyTable": "No data available in table",
                },
                "ajax": {
                    "url": Url.API.gridKurs,
                    "type": "POST",
                    "datatype": "json",
                    "data": params,
                },
                buttons: [
                       {
                           extend: 'copy', className: 'btn red btn-outline', exportOptions: {
                               columns: ':visible'
                           }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy'
                       },
                               {
                                   text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                                       Table.KursIDR.Export();
                                   }
                               },
                       { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
                ],
                "filter": false,
                "order": [[1, 'asc']],
                "lengthMenu": [[15, 30, 50, 100], ['15', '30', '50', '100']],
                "destroy": true,
                "columns": Table.KursIDR.Columns,
                dom: "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                fnPreDrawCallback: function () {
                    App.blockUI({ target: "#panelSummary", animate: !0 });
                },
                fnDrawCallback: function (result) {//method ini di hit sebanyak jumlah row
                    if (result.json != undefined && result.json.data != undefined) {// mastiin data udah ke load semua.
                        App.unblockUI('#panelSummary');

                        $('.btnEdit').click(function () {
                            Control.InitOpenModal();

                            //console.log(this);
                            Temp.KursStartDate = this.getAttribute('data-StartDate');
                            Temp.KursEndDate = this.getAttribute('data-EndDate');
                            var FileName = this.getAttribute('data-FileName') == "null" ? "" : this.getAttribute('data-FileName');
                            $("#txtStartDate").val(this.getAttribute('data-StartDate'));
                            $("#txtEndDate").val(this.getAttribute('data-EndDate'));
                            $("#txtKursIDR").val(this.getAttribute('data-Kurs'));
                            $("#txtAttachment").text(FileName);
                        });
                        $('.btnDelete').click(function () {
                            var params = {
                                StartDate: this.getAttribute('data-StartDate'),
                                EndDate: this.getAttribute('data-EndDate'),
                                Mode: "Delete"
                            }
                            formData = new FormData();
                            formData.append("datainput", JSON.stringify(params));
                            files = $("#fileAttach").get(0).files;
                            if (files.length != 0) {
                                formData.append("attachment", files[0]);
                            }
                            if (confirm('Are you sure you want to delete?')) {
                                var n = Ladda.create(document.querySelector(".btnDelete"));
                                $.ajax({
                                    url: Url.API.Submit,
                                    type: "POST",
                                    contentType: false, // Not to set any content header  
                                    processData: false, // Not to process data 
                                    data: formData,
                                    cache: !1,
                                    beforeSend: function () {
                                        n.start()
                                    }
                                }).done(function (t) {
                                    var delayInMilliseconds = 500; //0,5 second

                                    setTimeout(function () {
                                        Common.CheckError.Object(t) && Common.Alert.Success("Kurs IDR has been deleted");
                                    }, delayInMilliseconds);

                                    Table.KursIDR.Search();

                                    n.stop()
                                }).fail(function (t, i, r) {
                                    Common.Alert.Error(r);
                                    n.stop()
                                });
                            } else {

                            }
                        });
                        $('.btDownloadAttachment').click(function () {
                            var FilePath = this.getAttribute('data-FilePath');
                            var ContentType = this.getAttribute('data-ContentType');
                            var OriginalFileName = this.getAttribute('data-FileName');
                            window.location.href = Url.Download.Attachment + "?FilePath=" + FilePath + "&ContentType=" + ContentType + "&OriginalFileName=" + OriginalFileName;
                        });
                    }
                }
            });
        },
        Export: function () {
            var StartDate = $("#tbxSearchStartPeriode").val() != "undefined" ? $("#tbxSearchStartPeriode").val().replace(/'/g, "''") : null,
            EndDate = $("#tbxSearchEndPeriode").val() != "undefined" ? $("#tbxSearchEndPeriode").val().replace(/'/g, "''") : null;
            window.location.href = Url.Export.gridKurs + "?StartDate=" + StartDate + "&EndDate=" + EndDate;
        }
    }
}

var Widget = {
    Button: {
        btnSave: function () {
            $('#btnSave').on('click', function () {
                var params = {
                    StartDate: $("#txtStartDate").val(),
                    EndDate: $("#txtEndDate").val(),
                    UpdatedStartDate: Temp.KursStartDate,
                    UpdatedEndDate: Temp.KursEndDate,
                    Kurs: $("#txtKursIDR").val(),
                    Currency: 'USD',
                    Mode: (Temp.KursStartDate == null && Temp.KursEndDate == null) ? "Save" : "Edit"
                }
                formData = new FormData();
                formData.append("datainput", JSON.stringify(params));
                files = $("#fileAttach").get(0).files;
                if (files.length != 0) {
                    formData.append("attachment", files[0]);
                }
                if (Widget.ValidateSubmit(formData)) {
                    var confirmS = (Temp.KursStartDate == null && Temp.KursEndDate == null) ? "Save" : "Edit";
                    if (confirm('Are you sure you want to ' + confirmS + '?')) {
                        var n = Ladda.create(document.querySelector("#btnSave"));
                        $.ajax({
                            url: Url.API.Submit,
                            type: "POST",
                            contentType: false, // Not to set any content header  
                            processData: false, // Not to process data 
                            data: formData,
                            cache: !1,
                            beforeSend: function () {
                                n.start()
                            }
                        }).done(function (t) {
                            var delayInMilliseconds = 500; //0,5 second

                            setTimeout(function () {
                                Common.CheckError.Object(t) && Common.Alert.Success("Kurs IDR has been " + confirmS + (confirmS == "Save" ? "d" : "ed."));
                            }, delayInMilliseconds);

                            Table.KursIDR.Search();
                            Temp.Clear();

                            $('#mdlCreateEditKurs').modal('hide');
                            n.stop()
                        }).fail(function (t, i, r) {
                            Common.Alert.Error(r);
                            n.stop()
                        });
                    } else {

                    }
                }
            });
        },
        btnReset: function () {
            $('#btnReset').on('click', function () {
                Temp.Clear();
            });
        }
    },
    ValidateSubmit: function (formData) {
        var n = 0, s = "", p = JSON.parse(formData.get("datainput")), o = 0;

        var StartDate = [];
        var CheckOverlap = [];
        var EndDate = [];
        //for (var i = 0; i < $("#tblSummary").DataTable().data().toArray().length; i++) {
        //    StartDate.push(moment($("#tblSummary").DataTable().data().toArray()[i].StartDate).format('DD-MMM-YYYY'));
        //    EndDate.push(moment($("#tblSummary").DataTable().data().toArray()[i].EndDate).format('DD-MMM-YYYY'));
        //    CheckOverlap.push({
        //        StartDate: moment($("#tblSummary").DataTable().data().toArray()[i].StartDate).format('DD-MMM-YYYY'),
        //        EndDate: moment($("#tblSummary").DataTable().data().toArray()[i].EndDate).format('DD-MMM-YYYY')
        //    });
        //}
        //for (var i = 0; i < CheckOverlap.length; i++) {
        //    if (moment(p.StartDate.trim()).isBetween(CheckOverlap[i].StartDate, CheckOverlap[i].EndDate)) {
        //        o += 1;
        //    }
        //    if (moment(p.EndDate.trim()).isBetween(CheckOverlap[i].StartDate, CheckOverlap[i].EndDate)) {
        //        o += 1;
        //    }
        //}

        var isOverlapping = Func.CheckIsDateRangeOverlaped()
        if (isOverlapping != '') {
            n += 1; s += "\n - Date Overlap.";
        }


        var file = formData.get("attachment");
        if (file != null) {
            if (Func.FileSizeToMB(file.size) > 2) {
                n += 1; s += "\n - Attachment file Can`t bigger then 2MB";
            }
        }
        if (p.StartDate.trim() == "" || p.StartDate.trim() == null) {
            n += 1; s += "\n - Start Date cannot null.";
        }
        if (p.EndDate.trim() == "" || p.EndDate.trim() == null) {
            n += 1; s += "\n - End Date cannot null.";
        }

        if (o > 0) {
            n += 1; s += "\n - Date Overlap.";
        }
        if (Date.parse(p.StartDate.trim()) > Date.parse(p.EndDate.trim())) {
            n += 1; s += "\n - Start Date can't bigger than End Date!";
        }
        if ((StartDate.indexOf(p.StartDate.trim()) >= 0 && EndDate.indexOf(p.EndDate.trim())) && (Temp.KursStartDate != p.StartDate.trim() && Temp.KursEndDate != p.EndDate.trim())) {
            n += 1; s += "\n - Start Date and End Date already exist.";
        }
        if (p.Kurs.trim() == "" || p.Kurs.trim() == null) {
            n += 1; s += "\n - Kurs IDR cannot null.";
        } else if (p.Kurs.trim() < 0) {

            n += 1; s += "\n - Kurs IDR cannot be lower than 0.";
        }
        if (n > 0) {
            Common.Alert.Warning(s);
            return !1
        }
        return !0;
    }
}

var Func = {
    FileSizeToMB: function (fileSize) {
        return fileSize / (1024 * 1024);
    },
    CheckIsDateRangeOverlaped: function () {
        var res = '';
        $.ajax({
            url: Url.API.validateDate,
            type: "POST",
            datatype: 'json',
            async: false,
            data: {
                StartDate: $('#txtStartDate').val(),
                EndDate: $('#txtEndDate').val(),
                UpdatedStartDate: Temp.KursStartDate,
                UpdatedEndDate: Temp.KursEndDate
            },
        }).done(function (t) {
            if (t.isValid == false) {
                res = '\n - Date Overlap.'
            }
        });
        return res;
    }
}

var Url = {
    API: {
        gridKurs: "/api/Inflasi/KursGridList",
        Submit: "/api/Inflasi/SubmitKurs",
        validateDate: "/api/Inflasi/IsDateRangeOverlaped"
    },
    Export: {
        gridKurs: "/Inflasi/GridKurs/Export"
    },
    Download: {
        Attachment: "/Inflasi/DownloadAttachmentIR"
    }
}

var Temp = {
    KursStartDate: null,
    KursEndDate: null,
    ID : 0,
    Clear: function () {
        $("#txtStartDate").val('');
        $("#txtEndDate").val('');
        $("#txtKursIDR").val('');
        $("#txtAttachment").text('');
        Temp.KursStartDate = null;
        Temp.KursEndDate = null;
    }
}