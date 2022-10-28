Data = {};
Data.RowSelectedCollection = [];
Data.SelectedPayment = {};

param = {};
var chkID = '';
var trxInvoiceMatchingARID = '';
var companyCode = '';
var selectedPayment;

jQuery(document).ready(function () {
    Control.Init();
    Control.SetParams();
    Table.Init();
    Table.ReportInvoice();
    Table.ReportCollection();
});

var Control = {
    Init: function () {
        Control.BindingCompany();
        Control.BindingDocumentPayment();
        $("#slStatus").select2({ placeholder: "Select Status", width: null });

        PKPPurpose.Set.UserCompanyCode();
        if (PKPPurpose.Temp.UserCompanyCode == Constants.CompanyCode.PKP) {
            PKPPurpose.Filter.PKPOnly();
        }

        $('#tbSummaryCollection').on('change', 'tbody tr .checkboxes', function () {
            var id = $(this).parent().attr('id');
            var table = $("#tbSummaryCollection").DataTable();

            var DataRows = {};
            var Row = table.row($(this).parents('tr')).data();

            if (this.checked) {
                DataRows.trxMatchingARID = Row.trxMatchingARID;
                DataRows.DocheaderText = Row.DocumentHeaderText;
                DataRows.Entrydate = Row.InsertDate;
                DataRows.Entrytime = Row.InsertTime;
                DataRows.CompanycodeInvoice = Row.CompanyCodeInvoice;
                DataRows.CustomerNumber = Row.Customer;
                DataRows.CompanycodePayment = Row.CompanyCodePayment;
                DataRows.RekeningKoranid = Row.RekeningKoranid
                DataRows.DocumentPayment = Row.Documentpayment;
                DataRows.Tanggaluangmasuk = Row.Tanggaluangmasuk;
                DataRows.Currency = Row.Currency;
                DataRows.TotalPayment = Row.Totalpayment;
                DataRows.NilaiInvoice = Row.NilaiInvoice;
                DataRows.PaidAmount = Row.PaidAmount;
                DataRows.PphAmount = Row.PPHAmount;
                DataRows.Rounding = Row.Rounding;
                DataRows.Wapu = Row.WAPU;
                DataRows.Rtgs = Row.RTGS;
                DataRows.Penalty = Row.Penalty;
                DataRows.PpnExpired = Row.PPNExpired;
                DataRows.Status = Row.PaymentType;
                DataRows.IsOtherRevision = Row.IsOtherRevision;
                Data.RowSelectedCollection.push(DataRows);
            } else {
                var index = Data.RowSelectedCollection.findIndex(function (data) {
                    return data.RowNumber == Row.RowNumber;
                });
                Data.RowSelectedCollection.splice(index, 1);
            }
        });

        $("#btGenerateSAP").unbind().click(function () {
            Process.GenerateToSAP();
        });

        $("#btSearch").unbind().click(function () {
            Control.SetParams();
            Table.ReportInvoice();
            Table.ReportCollection();
        });

        $("#btReset").unbind().click(function () {
            $('#slCompany').val(null).trigger('change');
            $('#slDocumentPayment').val(null).trigger('change');
            $('#tbInvNo').val('');
            $('#tbStartPaid').val('');
            $('#tbEndPaid').val('')
        });

        $(".btSearchHeader").unbind().click(function () {
            Table.DocumentPayment();
        });

        $("#btSave").unbind().click(function () {
            Process.EditOthers();
        });
    },

    BindingCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
        })
       .done(function (data, textStatus, jqXHR) {
           $("#slCompany").html("<option></option>")

           if (Common.CheckError.List(data)) {
               $("#slCompany").html("<option></option>");
               $.each(data, function (i, item) {
                   $("#slCompany").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
               })
           }

           $("#slCompany").select2({ placeholder: "Select Company", width: null });

       })
       .fail(function (jqXHR, textStatus, errorThrown) {
           Common.Alert.Error(errorThrown);
       });
    },

    BindingDocumentPayment: function () {
        $.ajax({
            url: "/api/ApprovalMonitoringAR/DocumentPayment",
            type: "GET",
            async: false
        })
       .done(function (data, textStatus, jqXHR) {
           $("#slDocumentPayment").html("<option></option>")

           if (Common.CheckError.List(data)) {
               $("#slDocumentPayment").html("<option></option>");
               $.each(data, function (i, item) {
                   $("#slDocumentPayment").append("<option value='" + item.Documentpayment + "'>" + item.Documentpayment + "</option>");
               })
           }

           $("#slDocumentPayment").select2({ placeholder: "Select Document Payment", width: null });

       })
       .fail(function (jqXHR, textStatus, errorThrown) {
           Common.Alert.Error(errorThrown);
       });
    },

    SetParams: function () {
        param = {
            vCompanyID: $('#slCompany').val(),
            vInvoiceNo: $('#tbInvNo').val(),
            vStartPaid: $('#tbStartPaid').val(),
            vEndPaid: $('#tbEndPaid').val(),
            vDocumentPayment: $('#slDocumentPayment').val(),
            vStatus: $('#slStatus').val()
        }
    },
}

var Table = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();

        var tbSummaryInvoice = $('#tbSummaryInvoice').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tbSummaryInvoice").DataTable().columns.adjust().draw();
        });

        $("#tbSummaryInvoice tbody").on("click", "button.btEdit", function (e) {
            var table = $("#tbSummaryInvoice").DataTable();
            var data = table.row($(this).parents("tr")).data();

            if (data != null) {
                Data.Selected = data;
                $("#tbInvoiceNumber").val(Data.Selected.InvoiceNumber);
                $("#tbPaidDate").val(Common.Format.ConvertJSONDateTime(Data.Selected.InvPaidDate));
                $("#tbPaidAmount").val(Common.Format.CommaSeparation(Data.Selected.PaidAmount));
                $("#tbKeterangan").val(Data.Selected.Keterangan);
                $("#tbCompany").val(Data.Selected.CompanyCodeInvoice);
                $("#tbOperator").val(Data.Selected.Customer);
                $("#tbDocumentPayment").val(Data.Selected.DocumentPayment);
                trxInvoiceMatchingARID = Data.Selected.ID;
                companyCode = Data.Selected.CompanyCodeInvoice;
            }

            Table.DocumentPaymentLog();
            $('#mdlEditDocPayment').modal('show');
        });
    },

    ReportInvoice: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var tbSummaryInvoice = $("#tbSummaryInvoice").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/ApprovalMonitoringAR/invoiceGrid",
                "type": "POST",
                "datatype": "json",
                "data": param
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.ExportInvoice();
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
                        if (full.DocumentPayment == 'OTHERS') {
                            strReturn += "<button type='button' title='Edit' class='btn blue btn-xs btEdit'><i class='fa fa-edit'></i></button>";
                        }
                        return strReturn;
                    }
                },
                { data: "InvoiceNumber" },
                {
                    data: "InvPaidDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "CompanyCodeInvoice" },
                { data: "Customer" },
                { data: "PaidAmount", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "DocumentPayment" },
                { data: "Keterangan" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tbSummaryInvoice.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").show();
                }
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            "columnDefs": [
                    { "targets": [1, 7], "className": "dt-left" },
                    { "targets": [5], "className": "dt-right" },
                    { "targets": [0, 2, 3, 4, 6], "className": "dt-center" }
            ],
            "order": []
        });
    },

    ReportCollection: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var tbSummaryCollection = $("#tbSummaryCollection").DataTable({
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/ApprovalMonitoringAR/collectionGrid",
                "type": "POST",
                "datatype": "json",
                "data": param
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.ExportCollection();
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
                        if (full.Status == "POSTED" || full.Status == "SUCCESS")
                            strReturn += "<label style='display:none;' id='" + full.RowNumber + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.RowNumber + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                        else
                            strReturn += "<label id='" + full.RowNumber + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.RowNumber + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                        return strReturn;
                    }
                },
                { data: "DocumentHeaderText" },
                { data: "InsertDate" },
                { data: "InsertTime" },
                { data: "CompanyCodeInvoice" },
                { data: "Customer" },
                { data: "CompanyCodePayment" },
                { data: "RekeningKoranid" },
                { data: "Documentpayment" },
                { data: "Tanggaluangmasuk" },
                { data: "Currency" },
                { data: "Totalpayment", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "NilaiInvoice", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "PaidAmount", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "PPHAmount", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Rounding", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "WAPU", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "RTGS", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Penalty", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "PPNExpired" },
                { data: "PaymentType" },
                { data: "Status" },
                { data: "ResponseMessage" },
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        if (full.IsOtherRevision == 1)
                            strReturn += "X";
                        else
                            strReturn += "";
                        return strReturn;
                    }
                },
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                //if (Common.CheckError.List(tbSummaryCollection.data())) {
                //    $(".panelSearchBegin").hide();
                //    $(".panelSearchResult").show();
                //}
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            "columnDefs": [
                    { "targets": [11, 12, 13, 14, 15, 16, 17, 18], "className": "dt-right" },
                    { "targets": [22], "className": "dt-left" },
                    { "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 19, 20, 21, 23], "className": "dt-center" }
            ],
            "order": [],
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                                    '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tbSummaryCollection .checkboxes" />' +
                                    '<span></span> ' +
                                '</label>';
                var th = $("th.select-all-raw").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-raw").unbind().on("change", ".group-checkable", function (e) {
                    //Data.RowSelected = [];
                    var set = $(".checkboxes");
                    var checked = jQuery(this).is(":checked");
                    jQuery(set).each(function () {
                        label = $(this).parent();
                        var id = label.attr("id");
                        if (label.attr("style") != "display:none") {
                            if (checked) {
                                $(this).prop("checked", true);
                                $(this).parents('tr').addClass('active');
                                $(this).trigger("change");

                                $(".Row" + id).addClass("active");
                                $("." + id).prop("checked", true);
                                $("." + id).trigger("change");

                                //$("." + id).hide();
                            } else {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");
                                $(this).trigger("change");

                                $(".Row" + id).removeClass("active");
                                $("." + id).prop("checked", false);
                                $("." + id).trigger("change");

                                $("." + id).show()
                            }
                        }
                    });
                });
            }
        });
    },

    DocumentPaymentLog: function () {
        var params = {
            vDocumentPayment: $("#vDocumentPayment").val(),
            vCompanyCode: companyCode,
            vTglUangMasuk: $("#vTglUangMasuk").val()
        };

        var tbl = $("#tbDocPaymentSAP").DataTable({
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/ApprovalMonitoringAR/docPaymentLog",
                "type": "POST",
                "data": params
            },
            buttons: [
               { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
               {
                   text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                       var l = Ladda.create(document.querySelector(".yellow"));
                       l.start();
                       Table.ExportDocPayment()
                       l.stop();
                   }
               },
               { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[5, 10], ['5', '10']],
            "destroy": true,
            "columns": [
                {
                    mRender: function (data, type, full) {
                        var tgluangmasuk = full.Tanggaluangmasuk.replace(/\./g, "");
                        var idStg = full.Rekeningkoranid + full.Documentpayment + full.Companycode + tgluangmasuk;

                        return "<label id='" + idStg + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input id='cb_" + idStg + "' type='checkbox' class='checkboxes1'/><span></span></label>";
                    }
                },
                { data: "DocumentPayment" },
                { data: "Companycode" },
                { data: "Tanggaluangmasuk" },
                { data: "TotalPayment", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Currency" },
                { data: "Namabank" },
                { data: "Nomorrekening" },
                { data: "Keterangan" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                if (Data.SelectedPayment.length > 0) {
                    $(".checkboxes1").prop("disabled", true);
                    $(chkID).prop("disabled", false);
                }
            },
            "columnDefs": [
                    { "targets": [0, 1, 2, 4, 6], "className": "dt-center" },
                    { "targets": [3], "className": "dt-right" },
                    { "targets": [5, 7], "className": "dt-left" }
            ],
            "order": []
        });

        $('#tbDocPaymentSAP').on('change', 'tbody tr .checkboxes1', function () {
            var id = $(this).parent().attr('id');
            var table = $("#tbDocPaymentSAP").DataTable();
            var DataRows = {};
            var Row = table.row($(this).parents('tr')).data();

            chkID = '#cb_' + id;

            $(".checkboxes1").prop("disabled", true);
            if (this.checked) {
                Data.SelectedPayment = Row
                selectedPayment = 1;
                $(chkID).prop("disabled", false);
                $(this).parents('tr').css("background-color", "yellow");
            } else {
                Data.SelectedPayment = [];
                selectedPayment = 0;
                Table.DocumentPaymentSAP();
            }
        });
    },

    ExportDocPayment: function () {
        var href = "/InvoiceTransaction/stgDocumentPayment/Export";
        window.location.href = href;
    },

    ExportInvoice: function () {
        var href = "/InvoiceTransaction/ReportInvoice/Export?" + $.param(param);
        window.location.href = href;
    },

    ExportCollection: function () {
        var href = "/InvoiceTransaction/ReportCollection/Export?" + $.param(param);
        window.location.href = href;
    }
}

var Process = {
    GenerateToSAP: function () {
        var l = Ladda.create(document.querySelector("#btGenerateSAP"));
        if (Data.RowSelectedCollection.length > 0) {
            dataList = [];
            for (var i = 0; i < Data.RowSelectedCollection.length; i++) {
                var DataRows = {}
                DataRows.trxMatchingARID = Data.RowSelectedCollection[i].trxMatchingARID;
                DataRows.DocheaderText = Data.RowSelectedCollection[i].DocheaderText;
                DataRows.Entrydate = Data.RowSelectedCollection[i].Entrydate;
                DataRows.Entrytime = Data.RowSelectedCollection[i].Entrytime;
                DataRows.CompanycodeInvoice = Data.RowSelectedCollection[i].CompanycodeInvoice;
                DataRows.CustomerNumber = Data.RowSelectedCollection[i].CustomerNumber;
                DataRows.CompanycodePayment = Data.RowSelectedCollection[i].CompanycodePayment;
                DataRows.RekeningKoranid = Data.RowSelectedCollection[i].RekeningKoranid;
                DataRows.DocumentPayment = Data.RowSelectedCollection[i].DocumentPayment;
                DataRows.Tanggaluangmasuk = Data.RowSelectedCollection[i].Tanggaluangmasuk;
                DataRows.Currency = Data.RowSelectedCollection[i].Currency;
                DataRows.TotalPayment = Data.RowSelectedCollection[i].TotalPayment;
                DataRows.NilaiInvoice = Data.RowSelectedCollection[i].NilaiInvoice;
                DataRows.PaidAmount = Data.RowSelectedCollection[i].PaidAmount;
                DataRows.PphAmount = Data.RowSelectedCollection[i].PphAmount;
                DataRows.Rounding = Data.RowSelectedCollection[i].Rounding;
                DataRows.Wapu = Data.RowSelectedCollection[i].Wapu;
                DataRows.Rtgs = Data.RowSelectedCollection[i].Rtgs;
                DataRows.Penalty = Data.RowSelectedCollection[i].Penalty;
                DataRows.PpnExpired = Data.RowSelectedCollection[i].PpnExpired;
                DataRows.Status = Data.RowSelectedCollection[i].Status;
                DataRows.FlagOthers = Data.RowSelectedCollection[i].IsOtherRevision;
                dataList.push(DataRows);
            }

            var params = {
                ListMatchingARCollection: dataList
            }

            $.ajax({
                url: "/api/ApprovalMonitoringAR/insertToStg",
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
                    Common.Alert.Success("Data has been posted to SAP Staging Integration");
                    Control.SetParams();
                    Table.ReportInvoice();
                    Table.ReportCollection();
                }
                l.stop();
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l.stop();
            })
            .always(function (jqXHR, textStatus) {
                Data.RowSelectedCollection = [];
            })
        } else {
            Common.Alert.Warning("There is no data to Generate SAP.");
        }
        
    },

    EditOthers: function () {
        if (selectedPayment > 0) {
            var l = Ladda.create(document.querySelector("#btSave"));

            var params = {
                trxInvoiceMatchingARID: trxInvoiceMatchingARID,
                DocumentPaymentLog: Data.SelectedPayment
            }

            $.ajax({
                url: "/api/ApprovalMonitoringAR/updateDocPaymentOther",
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
                    Common.Alert.Success("Data Success Edit");
                    $('#mdlEditDocPayment').modal('hide');
                    Table.ReportInvoice();
                    Table.ReportCollection();
                }
                l.stop();
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l.stop();
            })
        } else {
            Common.Alert.Warning("There is no Document Payment to be save.");
        }

    }
}

var Constants = {
    CompanyCode: {
        PKP: "PKP"
    }
}

var PKPPurpose = {
    Filter: {
        PKPOnly: function () {
            $('#slCompany').val(Constants.CompanyCode.PKP).trigger('change');
        },
    },
    Set: {
        UserCompanyCode: function () {
            PKPPurpose.Temp.UserCompanyCode = $('#hdUserCompanyCode').val();
        }
    },
    Temp: {
        UserCompanyCode: ""
    }
}