Data = {};
Data.ARDeptHead = "";

var fsCompanyName = "";
var fsInvoiceTypeId = "";
var fsInvoiceStatusId = "";
var fsInvNo = "";

/* Helper Functions */

jQuery(document).ready(function () {
    Helper.GetCurrentUserRole();
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

    //panel transaction
    $("#formTransaction").submit(function (e) {
        Process.Posting();
        e.preventDefault();
    });

    $("#btCancelInvoiceTemp").unbind().click(function () {
        $('#mdlCancelInvoice').modal('show');
    });

    $("#btApproveCancel").unbind().click(function () {
        $('#mdlCancelInvoice').modal('show');
    });

    $("#btApprove").unbind().click(function () {
        Process.ApproveCancelInvoice();
    });

    $("#btReject").unbind().click(function () {
        Process.RejectCancelInvoiceTemp();
    });

    $(".btCancel").unbind().click(function () {
        Form.Cancel();
    });

    $("#formReject").submit(function (e) {
        Process.CancelInvoiceTemp();
        e.preventDefault();
    });

    $("#btBack").unbind().click(function () {
        $("#pnlSummary").fadeIn();
        $("#pnlDetailResult").hide();
    });
});

var Form = {
    Init: function () {
        $("#pnlTransaction").hide();
        $("#formTransaction").parsley();
        $("#formReject").parsley();

        if (!$("#hdAllowProcess").val()) {
            $(".btSelect").hide();
        }

        Control.BindingSelectInvoiceType();
        Control.LoadSignatureUser();
        Control.BindingSelectInvoiceStatus();

        //Helper.InitCurrencyInput("#tbDiscount");
        //Helper.InitCurrencyInput("#tbPenalty");

        $("#taDescription").val("");
        Table.Reset();
    },
    SelectedData: function () {
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Posting Building Invoice");
        $("#formTransaction").parsley().reset()

        $("#rbCompany" + Data.Selected.CompanyType).attr("checked", "checked");
        $("#tbCompanyName").val(Data.Selected.CompanyName);
        $("#tbCompanyTBG").val(Data.Selected.CompanyTBG);
        $("#hfTrxInvoiceHeaderID").val(Data.Selected.trxInvoiceHeaderID);
        $("#hfTrxInvoiceBuildingDetailID").val(Data.Selected.trxInvoiceBuildingDetailID);
        $("#tbTermPeriod").val(Data.Selected.TermPeriod);
        $("#tbArea").val(Data.Selected.Area);
        //$("#tbPricePerArea").val(Helper.FormatCurrency(Data.Selected.MeterPrice));
        $("#tbPricePerArea").val(Helper.FormatCurrency(Data.Selected.PricePerArea));
        $("#tbPricePerMonth").val(Helper.FormatCurrency(Data.Selected.MonthlyPrice));
        $("#tbTotalPrice").val(Helper.FormatCurrency(Data.Selected.InvSumADPP + Data.Selected.Discount));
        $("#tbDiscount").val(Helper.FormatCurrency(Data.Selected.Discount));
        $("#tbDPP").val(Helper.FormatCurrency(Data.Selected.InvSumADPP));
        $("#tbPenalty").val(Helper.FormatCurrency(Data.Selected.InvTotalPenalty));
        $("#tbPPN").val(Helper.FormatCurrency(Data.Selected.InvTotalAPPN));
        $("#tbTotalAmount").val(Helper.FormatCurrency(Data.Selected.InvTotalAmount));
        $("#tbStartPeriod").val(Common.Format.ConvertJSONDateTime(Data.Selected.StartPeriod));
        $("#tbEndPeriod").val(Common.Format.ConvertJSONDateTime(Data.Selected.EndPeriod));

        //$("#tbInvoiceDate").val(Common.Format.ConvertJSONDateTime(Data.Selected.InvPrintDate));

        $("#taDescription").val(Data.Selected.InvSubject);
        $("#slSignature").val(Data.Selected.InvFARSignature).trigger("change");

        if (Data.Selected.mstInvoiceStatusId != InvoiceStatus.StateCreated && Data.Selected.mstInvoiceStatusId != InvoiceStatus.StateRejectedCNPrintInvoice) {
            $("#btCancelInvoiceTemp").attr("disabled", "disabled");
            $("#btSubmit").attr("disabled", "disabled");
            $("#tbInvoiceDate").attr("disabled", "disabled");
            $("#taDescription").attr("disabled", "disabled");
            $("#slSignature").attr("disabled", "disabled");
        } else {
            $("#btCancelInvoiceTemp").removeAttr("disabled");
            $("#btSubmit").removeAttr("disabled");
            $("#tbInvoiceDate").removeAttr("disabled");
            $("#taDescription").removeAttr("disabled");
            $("#slSignature").removeAttr("disabled");
            $("#tbInvoiceDate").datepicker({ format: "dd-M-yyyy" });
            $("#tbInvoiceDate").datepicker('setDate', new Date());
            $("#taDescription").val("");
            $("#slSignature").val(Data.ARDeptHead).trigger("change");

            // Generate Default Description
            var defaultDescription = "Pembayaran Sewa Gedung " + Data.Selected.CompanyName + " periode " + Common.Format.ConvertIndonesiaDateTime(Data.Selected.StartPeriod) + " - " + Common.Format.ConvertIndonesiaDateTime(Data.Selected.EndPeriod);
            $("#taDescription").val(defaultDescription);
        }
        /*edit by mtr*/
        if (Data.Selected.BuildingType != 1) {
            $(".tbTermPeriod").hide();
            $(".tbAreaMetric").hide();
            $(".tbPricePerArea").hide();
            $("#tbPricePerMonth").val(Helper.FormatCurrency(Data.Selected.PricePerArea));
        } else {
            $(".tbTermPeriod").show();
            $(".tbAreaMetric").show();
            $(".tbPricePerArea").show();
        }
        /*edit by mtr*/
        Control.SetControlByPosition();

    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransaction").hide();
        $(".panelSearchBegin").fadeIn();
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        $('#mdlCancelInvoice').modal('hide');
        $("#tbRemarksCancel").val("");
        Table.Search();
    },
    Clear: function () {
        $("#tbArea").val("");
        $("#tbTermPeriod").val("");
        $("#tbTotalPrice").val("");
        $("#tbPricePerArea").val("");
        $("#tbPricePerMonth").val("");
        $("#tbPPN").val("");
        $("#tbTotalAmount").val("");
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

        fsCompanyName = $("#tbSearchCompanyName").val();
        fsInvoiceTypeId = $("#slSearchTerm").val() == null ? "" : $("#slSearchTerm").val();
        fsInvoiceStatusId = $("#slSearchInvoiceStatus").val() == null || $("#slSearchInvoiceStatus").val() == "" ? -1 : $("#slSearchInvoiceStatus").val();
        fsInvNo = $("#tbInvNo").val();

        var params = {
            companyName: fsCompanyName,
            invoiceTypeId: fsInvoiceTypeId,
            invoiceStatusId: fsInvoiceStatusId,
            invNo: fsInvNo
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/PostingInvoiceBuilding/grid",
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
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<button type='button' title='Select' class='btn blue btn-xs btSelect'><i class='fa fa-mouse-pointer'></i></button>";

                        return strReturn;
                    }
                },
                { data: "InvTemp" },
                { data: "CompanyType" },
                { data: "CompanyName" },
                { data: "Area", className: "text-right" },
                //{
                //    data: "MeterPrice", className: "text-right", render: function (data) {
                //        return Helper.FormatCurrency(data);
                //    }
                //},
                 {
                     data: "PricePerArea", className: "text-right", render: function (data) {
                         return Helper.FormatCurrency(data);
                     }
                 },
                {
                    data: "StartPeriod", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndPeriod", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "TermPeriod" },
                {
                    data: "InvSumADPP", className: "text-right", render: function (data, type, full) {
                        return Helper.FormatCurrency(data + full.Discount);
                    }
                },
                {
                    data: "Discount", className: "text-right", render: function (data) {
                        return Helper.FormatCurrency(data);
                    }
                },
                {
                    data: "InvTotalAPPN", className: "text-right", render: function (data) {
                        return Helper.FormatCurrency(data);
                    }
                },
                {
                    data: "InvTotalPenalty", className: "text-right", render: function (data) {
                        return Helper.FormatCurrency(data);
                    }
                },
                { data: "BuildingType" },
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
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': [0],
            }, {
                
                'aTargets': [13],
                'visible': false,
            }],
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (Data.Role == "DEPT HEAD") {
                    if (aData.mstInvoiceStatusId == 5) {
                        $('td', nRow).css('background-color', '#FF9999');
                    }
                }
                l.stop();
            },
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
            "order": []
        });

        // Initialize events (for button clicks, etc.)
        $("#tblSummaryData tbody").unbind().on("click", "button.btSelect", function (e) {
            var table = $("#tblSummaryData").DataTable();
            Data.Selected = {};
            Data.Selected = table.row($(this).parents('tr')).data();
            Form.SelectedData();
        });
    },
    Reset: function () {
        fsCompanyName = "";
        fsInvoiceTypeId = "";
        fsInvoiceStatusId = "";
        fsInvNo = "";
        $("#tbInvNo").val("");
        $("#tbSearchCompanyName").val("");
        $("#slSearchTerm").val("").trigger("change");
        $("#slSearchInvoiceStatus").val("").trigger("change");
    },
    Export: function () {
        window.location.href = "/InvoiceTransaction/TrxCreateInvoiceBuilding/Export?companyName=" + fsCompanyName + "&invoiceTypeId=" + fsInvoiceTypeId + "&invoiceStatusId=" + fsInvoiceStatusId + "&invNo=" + fsInvNo;
    }
}

var Process = {
    Posting: function () {
        var params = {
            trxInvoiceHeaderID: Data.Selected.trxInvoiceHeaderID,
            invTemp: Data.Selected.InvTemp,
            invNo: Data.Selected.InvNo,
            subject: $("#taDescription").val(),
            signature: $("#slSignature").val(),
            invoiceDate: $("#tbInvoiceDate").val()
        }

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/PostingInvoiceBuilding",
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
                Common.Alert.Success("Invoice " + data.InvTemp + " has been posted successfully!");
                //Table.Reset();
                Form.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })

    },
    CancelInvoiceTemp: function () {
        var l = Ladda.create(document.querySelector("#btCancelInvoiceTemp"))
        var params = {
            trxInvoiceHeaderID: Data.Selected.trxInvoiceHeaderID,
            invTemp: Data.Selected.InvTemp,
            invNo: Data.Selected.InvNo,
            subject: $("#taDescription").val(),
            signature: $("#slSignature").val(),
            invoiceDate: $("#tbInvoiceDate").val(),
            remark: $("#tbRemarksCancel").val()
        }

        $.ajax({
            url: "/api/PostingInvoiceBuilding/Cancel",
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
                Common.Alert.Success("Invoice Rejected Successfully.")
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Form.Done();
        })
    },
    ApproveCancelInvoice: function () {
        var l = Ladda.create(document.querySelector("#btApprove"))
        var params = {
            trxInvoiceHeaderID: Data.Selected.trxInvoiceHeaderID,
            remark: $("#tbRemarksCancel").val()
        }

        $.ajax({
            url: "/api/PostingInvoiceBuilding/ApproveCancel",
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
                Common.Alert.Success("Invoice Rejected Successfully.");
                Notification.GetList();
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Form.Done();
        })

    },
    RejectCancelInvoiceTemp: function () {
        var l = Ladda.create(document.querySelector("#btReject"))
        var params = {
            trxInvoiceHeaderID: Data.Selected.trxInvoiceHeaderID
        }

        $.ajax({
            url: "/api/PostingInvoiceBuilding/RejectCancel",
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
                Common.Alert.Success("Cancel Invoice Rejected Successfully.")
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Form.Done();
        })
    }
}

var Control = {
    BindingSelectInvoiceType: function () {
        $.ajax({
            url: "/api/MstDataSource/InvoiceType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchTerm").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchTerm").append("<option value='" + item.mstInvoiceTypeId + "'>" + item.Description + "</option>");
                })
            }

            $("#slSearchTerm").select2({ placeholder: "Select Term Invoice", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    LoadSignatureUser: function () {
        $.ajax({
            url: "/api/MstDataSource/SignatureUser",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                $("#slSignature").html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $("#slSignature").append("<option value='" + item.UserID + "'>" + item.FullName + "</option>");
                        if (item.HCISPosition == HCISPosition.ARDeptHead)
                            Data.ARDeptHead = item.UserID;
                    })
                }

                $("#slSignature").select2({ placeholder: "Select Signature", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    SetControlByPosition: function () {
        $.ajax({
            url: "/api/user/GetPosition",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                if (data.Result == "DEPT HEAD") {

                    $(".disabledCtrl").prop('disabled', true);
                    $("#btCancelInvoiceTemp").hide();
                    $("#btSubmit").hide();
                    $("#btRequest").hide();
                    $("#tbRemarksCancel").val(Data.Selected.InvRemarksPosting);
                    if (Data.Selected.mstInvoiceStatusId == 5) {
                        $("#btApproveCancel").show();
                        $("#btApprove").show();
                        $("#btReject").show();
                    }
                    else {
                        $("#btApproveCancel").show();
                        $("#btApprove").hide();
                        $("#btReject").hide();
                    }
                }
                else {
                    if (Data.Selected.mstInvoiceStatusId == 5) {
                        $(".disabledCtrl").prop('disabled', true);
                        $("#tbRemarksCancel").val(Data.Selected.InvRemarksPosting);
                        $("#btCancelInvoiceTemp").show();
                        $("#btSubmit").hide();
                        $("#btApproveCancel").hide();
                        $("#btRequest").hide();
                        $("#btApprove").hide();
                        $("#btReject").hide();
                    }
                    else {
                        $(".disabledCtrl").prop('disabled', false);
                        $("#btCancelInvoiceTemp").show();
                        $("#btSubmit").show();
                        $("#btApproveCancel").hide();
                        $("#btRequest").show();
                        $("#btApprove").hide();
                        $("#btReject").hide();
                    }
                }
            }

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });

    },
    BindingSelectInvoiceStatus: function () {
        $("#slSearchInvoiceStatus").html("<option></option>")
        if (Data.Role == "DEPT HEAD") {
            $("#slSearchInvoiceStatus").append("<option value='12'>CANCEL APPROVED</option>");
        }
        else {
            $("#slSearchInvoiceStatus").append("<option value='2'>INVOICE CREATED</option>");
            $("#slSearchInvoiceStatus").append("<option value='26'>CN FROM PRINT INVOICE</option>");
        }
        $("#slSearchInvoiceStatus").append("<option value='5'>WAITING FOR APPROVAL</option>");
        $("#slSearchInvoiceStatus").select2({ placeholder: "Select Invoice Status", width: null });

    },
}

var Helper = {
    CalculateAmount: function () {
        var ppn = parseFloat($("#tbPPN").val().replace(/,/g, ""));
        var discount = parseFloat($("#tbDiscount").val().replace(/,/g, ""));
        var penalty = parseFloat($("#tbPenalty").val().replace(/,/g, ""));
        var totalPrice = parseFloat($("#tbTotalPrice").val().replace(/,/g, ""));

        var totalBeforePPN = totalPrice - discount + penalty;
        var ppn = totalBeforePPN * 0.1;
        var total = totalBeforePPN + ppn;
        $("#tbTotalAmount").val(Helper.FormatCurrency(total));
        $("#tbPPN").val(Helper.FormatCurrency(ppn));
        return total;
    },
    FormatCurrency: function (value) {
        if (isNaN(value))
            return "0.00";
        else
            return Common.Format.CommaSeparation(value);
    },
    GetCurrentUserRole: function () {
        $.ajax({
            url: "/api/user/GetPosition",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                Data.Role = data.Result;
            }
        });
    },
    InitCurrencyInput: function (selector) {
        $(selector).unbind().on("blur", function () {
            var value = $(selector).val();
            if (value != "") {
                $(selector).val(Helper.FormatCurrency(value));
            } else {
                $(selector).val("0.00");
            }
            Helper.CalculateAmount();
        })
    }
}

var InvoiceStatus = {
    NotProcessed: 0,
    StateBAPSConfirm: 1,
    StateCreated: 2,
    StatePosted: 3,
    StatePrinted: 4,
    StateWaitingReject: 5,
    Create: 6,
    Posting: 7,
    Print: 8,
    Reject: 9,
    Approve: 10,
    CancelInvoice: 11,
    StateRejectApproved: 12,
    StateRejectedCNPrintInvoice: 26
}

var HCISPosition = {
    ARDeptHead: "Account Receivable Database Department Head"
}