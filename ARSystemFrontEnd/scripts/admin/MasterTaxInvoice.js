Data = {};
UploadedData = [];

/* Helper Functions */

jQuery(document).ready(function () {
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

    //panel transaction Header
    $("#formMaster").submit(function (e) {
        if (Data.Mode == "Edit")
            TaxInvoice.Put();
        else if (Data.Mode == "Create")
            TaxInvoice.Post();
        e.preventDefault();
    });

    $("#btCreate").unbind().click(function () {
        Form.Create();
    });

    $(".btCancel").unbind().click(function () {
        Form.Cancel();
        Table.Reset();
    });

    $("#btDownload").on("click", function () {
        Form.DownloadTemplate();
    });

    $("#btUpload").on("click", function () {
        Form.UploadExcel();
    });

    $("#btBack").unbind().click(function () {
        $("#pnlSummary").fadeIn();
        $("#pnlDetailResult").hide();
    });

    $("#btSubmitTaxInvoices").on("click", function () {
        TaxInvoice.Post();
    });

    $(".fileinput").fileinput();

    $("#btSubmitTaxInvoices").unbind().on("click", function (e) {
        if ($("#formUploadedTaxInvoice").parsley().validate())
            TaxInvoice.Post();
        e.preventDefault();
    });
});

var Form = {
    Init: function () {
        $("#pnlFormMaster").hide();
        $("#formMaster").parsley();

        if (!$("#hdAllowAdd").val()) {
            $("#btCreate").hide();
        }

        Table.Reset();
    },
    Create: function () {
        Data.Mode = "Create";

        $("#divUploadField").show();
        $("#divUploadButtons").show();
        $("#divInvoiceNumber").hide();
        $("#divTaxInvoiceNumber").hide();
        $("#divTaxInvoiceDate").hide();
        $("#divFormButtons").hide();

        $("#pnlSummary").hide();
        $("#pnlFormMaster").fadeIn();
        $(".panelFormMaster").show();
        $("#panelFormMasterTitle").text("Upload Tax Invoice List");
        $("#formMaster").parsley().reset()

        $(".fileinput").fileinput("clear");
    },
    Edit: function () {
        Data.Mode = "Edit";
        $("#pnlSummary").hide();
        $("#pnlFormMaster").fadeIn();
        $(".panelFormMaster").show();
        $("#panelFormMasterTitle").text("Edit Tax Invoice");
        $("#formMaster").parsley().reset();

        $("#divUploadField").hide();
        $("#divUploadButtons").hide();
        $("#divInvoiceNumber").show();
        $("#divTaxInvoiceNumber").show();
        $("#divTaxInvoiceDate").show();
        $("#divFormButtons").show();

        $("#tbInvoiceNumber").val(Data.Selected.InvNo);
        $("#tbTaxInvoiceNumber").val(Data.Selected.TaxInvoiceNo);
        $("#tbTaxInvoiceDate").val(Data.Selected.TaxInvoiceDate);
    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlFormMaster").hide();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlFormMaster").hide();

        Table.Search();
        $(".panelSearchResult").fadeIn();
    },
    DownloadTemplate: function () {
        window.location.href = "/Admin/TaxInvoice/DownloadExcel";
    },
    UploadExcel: function () {
        var formData = new FormData(); //FormData object  
        var fileInput = document.getElementById("fuTaxInvoice");
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

                var l = Ladda.create(document.querySelector("#btUpload"));
                $.ajax({
                    url: '/Admin/TaxInvoice/ImportExcel',
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
                            $('#mdlUploadedTaxInvoice').modal('show');
                            $("#mdlUploadedTaxInvoice tbody").html("");
                            $("#ulErrors").html("");
                            UploadedTaxInvoice.DrawTable(data);
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
            invNo: $("#tbSearchInvNo").val(),
            taxInvoiceNo: $("#tbSearchTaxInvoiceNo").val()
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/MstTaxInvoice/grid",
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
                //{
                //    orderable: false,
                //    mRender: function (data, type, full) {
                //        var strReturn = "";
                //        //strReturn += "<button type='button' title='Detail' class='btn green btDetail'><i class='fa fa-search-plus'></i></button>";
                //        if ($("#hdAllowEdit").val())
                //            strReturn += "<button type='button' title='Edit' class='btn btn-xs green btEdit'><i class='fa fa-edit'></i></button>";
                //        return strReturn;
                //    },
                //    width: "10%"
                //},
                { data: "InvNo" },
                { data: "TaxInvoiceNo" },
                {
                    data: "TaxInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
            ],
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
            "order": [[0, "asc"]]
        });
    },
    Reset: function () {
        $("#tbSearchInvNo").val("");
        $("#tbSearchTaxInvoiceNo").val("");
    },
    Export: function () {
        var invNo = $("#tbSearchInvNo").val();
        var taxInvoiceNo = $("#tbSearchTaxInvoiceNo").val();

        window.location.href = "/Admin/TaxInvoice/Export?invNo=" + invNo + "&taxInvoiceNo=" + taxInvoiceNo;
    }
}

var TaxInvoice = {
    Post: function () {
        var params = [];
        var param = {}

        $.each(UploadedData, function (index, item) {
            param = new Object();
            param = {
                TrxInvoiceHeaderID: item.TrxInvoiceHeaderID,
                InvNo: item.InvNo,
                TaxInvoiceNo: item.TaxInvoiceNo,
                strTaxInvoiceDate: $('#tbTaxInvoiceDate').val(),
                trxInvoiceHeaderRemainingAmountId: item.trxInvoiceHeaderRemainingAmountId,
                trxInvoiceNonRevenueID: item.trxInvoiceNonRevenueID
            }
            params.push(param);
        });

        var l = Ladda.create(document.querySelector("#btSubmitTaxInvoices"));
        $.ajax({
            url: "/api/MstTaxInvoice/BulkCreate",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            $('#mdlUploadedTaxInvoice').modal('hide');
            Common.Alert.Success("Tax Invoice have been created successfully!");
            Table.Reset();
            Form.Done();
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $('#mdlUploadedTaxInvoice').modal('hide');
            Common.Alert.Error(errorThrown)
            l.stop()
        });
    },
    Put: function () {
        var params = {
            InvNo: $("#tbInvoiceNumber").val(),
            TrxInvoiceHeaderID: Data.Selected.TrxInvoiceHeaderID,
            TaxInvoiceNo: $("#tbTaxInvoiceNumber").val(),
            TaxInvoiceDate: $("#tbTaxInvoiceDate").val()
        }
        console.log(params);

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/MstTaxInvoice/" + Data.Selected.TaxInvoiceID,
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
                Common.Alert.Success("Tax Invoice has been updated!");
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

var UploadedTaxInvoice = {
    DrawTable: function (data) {
        var html = "";
        var errorHtml = "";
        var anyErrors = false;
        UploadedTaxInvoice.Table.Init();
        console.log(data);
        $.each(data, function (index, item) {
            errorHtml = "";
            if (item.HasError) {
                $.each(item.ErrorMessages, function (idx, message) {
                    errorHtml += "<li>" + message + "</li>";
                });
                $("#ulErrors").append(errorHtml);
                anyErrors = true;
            }
        });

        if (anyErrors)
            $("#btSubmitTaxInvoices").hide();
        else
            $("#btSubmitTaxInvoices").show();
    },
    Table: {
        Init: function () {
            var oTable = $("#tblUploadedTaxInvoice").DataTable({
                "data": UploadedData,
                "pageLength": 10,
                "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
                "columnDefs": [{
                    'orderable': false,
                    'targets': [0, 1, 2]
                }, {
                    "searchable": true,
                    "targets": [0, 1, 2]
                }],
                "destroy": true,
                "columns": [
                    { data: "InvNo" },
                    { data: "TaxInvoiceNo" },
                    //{ data: "strTaxInvoiceDate" },
                    { data: "HasError", visible: false }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    if (aData.IsError) {
                        $('td', nRow).attr('style', 'background-color: #FF9999 !important;');
                    } else {
                        $('td', nRow).removeAttr('style');
                    }
                },
                "order": []
            });
        }
    }
}