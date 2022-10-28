Data = {};

var fsCompanyId = "";
var fsOperatorId = "";
var fsInvoiceTypeId = "";
var fsInvoiceStatusId = "";
var fsInvNo = "";
var fsInvoiceManual = "";
var fsUserCompanyCode = "";
var fsUserCompanyName = "";

jQuery(document).ready(function () {
    // Modification Or Added By Ibnu Setiawan 04. September 2020
    // Add Company Code By User Login
    fsUserCompanyCode = $('#hdUserCompanyCode').val();
    // End Modification Or Added By Ibnu Setiawan 04. September 2020
    
    Form.Init();
    Table.Init();
    // Modification Or Added By Ibnu Setiawan 07. September 2020
    if (fsUserCompanyCode.trim() == "PKP") {
        $("#slSearchCompanyName").val(fsUserCompanyCode).trigger("change");
    }

    // Di pindahkan karena ada Binding Grid yang mendahului Initialize Dropdown
    Control.GetCurrentUserRole(); // Modification Or Added By Ibnu Setiawan 09. September 2020 

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
        if (fsUserCompanyCode.trim() == "PKP") {
            $("#slSearchCompanyName").val(fsUserCompanyCode).trigger("change");
        }
    });
    //panel transaction
    $("#formTransaction").submit(function (e) {
        Process.Posting();
        e.preventDefault();
    });

    $("#btCancelInvoiceTemp").unbind().click(function () {
        if (Data.Selected.mstInvoiceCategoryId == 6)
            Common.Alert.Warning("Non-Revenue Invoice, Unable to Cancel Invoice!");
        else {
            if (Data.Selected.mstInvoiceStatusId != 5) //waiting for approval
                Common.PanelCNARData.Reset();
            $('#mdlCancelInvoice').modal('show');
        }
    });
    $("#btApproveCancel").unbind().click(function () {
        $('#mdlCancelInvoice').modal('show');
    });

    $(".btCancel").unbind().click(function () {
        Form.Cancel();
        //Table.Reset();
    });

    //$("#formReject").submit(function (e) {
    //    Process.CancelInvoiceTemp();
    //    e.preventDefault();
    //});
    $("#btRequest").unbind().click(function () {
        Process.CancelInvoiceTemp();
    });
    $("#btApprove").unbind().click(function () {
        Process.ApproveCancelInvoice();
    });
    $("#btReject").unbind().click(function () {
        Process.RejectCancelInvoice();
    });   
});

var Form = {
    Init: function () {
        $("#pnlTransaction").hide();
        $("#formTransaction").parsley(); 
        $("#formReject").parsley();
        Control.BindingSelectCompany();
        Control.BindingSelectOperator();
        Control.BindingSelectInvoiceType(); 
        Control.BindingSelectSignature();
        Control.BindingSelectInvoiceStatus();
        Control.BindingSelectPICAType();
        Control.BindingSelectPICADetail();
        Table.Reset();
        $("#slReturnTo").select2({ placeholder: "Select Return To", width: null });
    },
    SelectedData: function () {
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Posting Invoice Tower");
        $("#formTransaction").parsley().reset()

        $("#tbInvoiceTemp").val(Data.Selected.InvTemp);
        $("#tbCompany").val(Data.Selected.CompanyName);
        $("#slTermInvoice").val(Data.Selected.InvoiceTypeId).trigger('change');
        $("#tbAmount").val(Common.Format.CommaSeparation(Data.Selected.Amount));
        $("#tbDPP").val(Common.Format.CommaSeparation(Data.Selected.AmountADPP));
        $("#tbPPN").val(Common.Format.CommaSeparation(Data.Selected.InvTotalAPPN));
        $('#tbDiscount').val(Common.Format.CommaSeparation(Data.Selected.Discount));
        $("#tbPenalty").val(Common.Format.CommaSeparation(Data.Selected.InvTotalPenalty));
        $("#tbDescription").val("");
        Control.BindingSelectOperatorRegional();
        Control.SetSubjectTextBox();

        Control.SetControlByPosition();
        $("#slSignature").val(Data.ARDeptHead).trigger("change");
        $("#tbInvoiceDate").datepicker({ format: "dd-M-yyyy" });
        $("#tbInvoiceDate").datepicker('setDate', new Date());
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

        $("#tblSummaryData tbody").on("click", "button.btSelect", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                Form.SelectedData();
            }
        });

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsCompanyId = $("#slSearchCompanyName").val();
        fsOperatorId = $("#slSearchOperator").val();
        fsInvoiceTypeId = $("#slSearchTermInvoice").val();
        fsInvoiceStatusId = $("#slSearchInvoiceStatus").val() == null || $("#slSearchInvoiceStatus").val() == "" ? -1 : $("#slSearchInvoiceStatus").val();
        fsInvNo = $("#tbInvNo").val();
        fsInvoiceManual = $('input[name=rbInvoiceManual]:checked').val();

        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperatorId,
            strInvoiceType: fsInvoiceTypeId,
            intmstInvoiceStatusId: fsInvoiceStatusId,
            invNo: fsInvNo,
            invoiceManual: fsInvoiceManual
        }; 
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/PostingInvoiceTower/grid",
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
            "lengthMenu": [[10, 25, 50, -1], ['10', '25', '50', 'All']],
            "destroy": true,
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        if ($("#hdAllowProcess").val())
                            strReturn += "<button type='button' title='Select' class='btn blue btSelect btn-xs'><i class='fa fa-mouse-pointer'></i></button>";
                        return strReturn;
                    }
                },
                { data: "InvTemp" },
                {
                    data: "InvDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "CreateInvoice", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "TermPeriod" },
                { data: "Company" },
                { data: "CompanyInvoice" },
                { data: "Operator" },
                { data: "OperatorInvoice" },
                { data: "Amount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Discount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvTotalAPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvTotalPenalty", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Currency" },
                { data: "InvStatus" },
                {
                    data: "InvoiceManual", mRender: function (data, type, full) {
                        return full.InvoiceManual ? "Yes" : "No";
                    }
                },
                {
                data: "InvoiceNonRevenue", mRender: function (data, type, full) {
                    return full.InvoiceNonRevenue ? "Yes" : "No";
                }
        }
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
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (Data.Role == "DEPT HEAD") {
                    if (aData.mstInvoiceStatusId == 5) {
                        $('td', nRow).css('background-color', '#FF9999');
                    }
                }
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
            "order": []
        });
    },
    Reset: function () {
        fsCompanyId = "";
        fsOperatorId = "";
        fsInvoiceTypeId = "";
        fsInvoiceStatusId = "";
        fsInvNo = "";
        $("#slSearchOperator").val("").trigger('change');
        $('#slSearchTermInvoice').val("").trigger('change');
        $("#slSearchCompanyName").val("").trigger('change');
        $("#slSearchInvoiceStatus").val("").trigger('change');
        $("#tbInvNo").val("");
        $("#rbAll").prop('checked', true);

    },
    Export: function () {
        window.location.href = "/InvoiceTransaction/TrxPostingInvoiceTower/Export?strCompanyId=" + fsCompanyId + "&strOperator=" + fsOperatorId
            + "&strInvoiceType=" + fsInvoiceTypeId + "&intmstInvoiceStatusId=" + fsInvoiceStatusId + "&invNo=" + fsInvNo + "&invoiceManual=" + fsInvoiceManual;
    }
}

var Process = {
    Posting: function () {
        var l = Ladda.create(document.querySelector("#btPosting"))
        var params = {
            strInvoiceDate: $("#tbInvoiceDate").val(),
            strSubject: $("#tbDescription").val(),
            strOperatorRegionId: $("#slOperatorRegional").val(),
            strSignature: $("#slSignature").val(),
            DataCreatedInvoice: Data.Selected,
            strAdditionalNote: $("#tbAdditionalNote").val(),
        }

        $.ajax({
            url: "/api/PostingInvoiceTower/PostingInvoice",
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
                Common.Alert.Success("Invoice " + data.InvNo + " has been posted successfully!");

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
    CancelInvoiceTemp: function () {
        var l = Ladda.create(document.querySelector("#btRequest"))
        var params = {
            DataCreatedInvoice: Data.Selected,
            strRemarksCancel: $("#tbRemarksCancel").val(),
            mstPICATypeID: $("#slPicaType").val(),
            mstPICADetailID: $("#slPicaDetail").val()
        }
        $.ajax({
            url: "/api/PostingInvoiceTower/CancelInvoice",
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
                Common.Alert.Success("Data Success Canceled, Please Wait for Approval..")
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
            DataCreatedInvoice: Data.Selected
        }


        $.ajax({
            url: "/api/PostingInvoiceTower/ApproveCancelInvoice",
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
                Common.Alert.Success("Data Successfully Approved!");
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
    RejectCancelInvoice: function () {
        var l = Ladda.create(document.querySelector("#btReject"))
        var params = {
            DataCreatedInvoice: Data.Selected
        }


        $.ajax({
            url: "/api/PostingInvoiceTower/RejectCancelInvoice",
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
                Common.Alert.Success("Data Successfully Rejected!");
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

    }
}

var Control = {
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchCompanyName").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchCompanyName").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                    if (fsUserCompanyCode == "PKP") {
                        fsUserCompanyName = item.Company;
                    }
                })
            }

            $("#slSearchCompanyName").select2({ placeholder: "Select Company Name", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectOperator: function () {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchOperator").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchOperator").append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                })
            }

            $("#slSearchOperator").select2({ placeholder: "Select Operator", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectInvoiceType: function () {
        $.ajax({
            url: "/api/MstDataSource/InvoiceType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchTermInvoice").html("<option></option>")
            $("#slTermInvoice").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchTermInvoice").append("<option value='" + item.mstInvoiceTypeId + "'>" + item.Description + "</option>");
                    $("#slTermInvoice").append("<option value='" + item.mstInvoiceTypeId + "'>" + item.Description + "</option>");
                })
            }

            $("#slSearchTermInvoice").select2({ placeholder: "Select Term Invoice", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectSignature: function () {
        $.ajax({
            url: "/api/MstDataSource/SignatureUser",
            type: "GET"
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

            $("#slSignature").select2({ placeholder: "Select PIC Signature", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectOperatorRegional: function () {
        var params = {
            OperatorId: Data.Selected.Operator
        };
        $.ajax({
            url: "/api/MstDataSource/OperatorRegional",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slOperatorRegional").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slOperatorRegional").append("<option value='" + item.OprRegionId + "' title='"+item.Address1+" "+item.Address2+" "+item.Address3+"'>" + item.OperatorRegion + "</option>");
                })
            }

            $("#slOperatorRegional").select2({ placeholder: "Select Operator Regional", width: null });

           

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    SetSubjectTextBox: function () {
        var params = {
            trxInvoiceHeaderId: Data.Selected.trxInvoiceHeaderID,
            mstInvoiceCategoryId: Data.Selected.mstInvoiceCategoryId
        };
        $.ajax({
            url: "/api/PostingInvoiceTower/GetSubject",
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $("#tbDescription").val(data.Result);
            } 

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
                $("#btPosting").hide();
                $("#btRequest").hide();
                $("#tbRemarksCancel").val(Data.Selected.InvRemarksPosting);
                $("#slPicaType").val(Data.Selected.mstPICATypeID).trigger('change');
                $("#slPicaDetail").val(Data.Selected.mstPICADetailID).trigger('change');
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
                    $("#slPicaType").val(Data.Selected.mstPICATypeID).trigger('change');
                    $("#slPicaDetail").val(Data.Selected.mstPICADetailID).trigger('change');
                    $("#btCancelInvoiceTemp").show();
                    $("#btPosting").hide();
                    $("#btApproveCancel").hide();
                    $("#btRequest").hide();
                    $("#btApprove").hide();
                    $("#btReject").hide();
                }
                else {
                    $(".disabledCtrl").prop('disabled', false);
                    $("#btCancelInvoiceTemp").show();
                    $("#btPosting").show();
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
    BindingSelectInvoiceStatus: function () {
        $("#slSearchInvoiceStatus").html("<option></option>")
        if (Data.Role == "DEPT HEAD") {
            $("#slSearchInvoiceStatus").append("<option value='12'>CANCEL APPROVED</option>");
        }
        else {
            $("#slSearchInvoiceStatus").append("<option value='2'>INVOICE CREATED</option>");
            $("#slSearchInvoiceStatus").append("<option value='19'>INVOICE REMAINING CREATED</option>");
            $("#slSearchInvoiceStatus").append("<option value='26'>CN FROM PRINT INVOICE</option>");
        }
        $("#slSearchInvoiceStatus").append("<option value='5'>WAITING FOR APPROVAL</option>");
        $("#slSearchInvoiceStatus").select2({ placeholder: "Select Invoice Status", width: null });

    },
    BindingSelectPICAType: function () {
        $.ajax({
            url: "/api/MstDataSource/PICATypeARData",
            type: "GET",
            async: false,

        })
        .done(function (data, textStatus, jqXHR) {
            $("#slPicaType").html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slPicaType").append("<option value='" + item.mstPICATypeID + "'>" + item.Description + "</option>");
                })
            }

            $("#slPicaType").select2({ placeholder: "Select PICA Type", width: null }).on("change", function (e) {
                Control.BindingSelectPICADetail();
            });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectPICADetail: function () {
        var params = {
            HdrId: $("#slPicaType").val() == null || $("#slPicaType").val() == "" ? 0 : $("#slPicaType").val()
        };
        $.ajax({
            url: "/api/BapsData/RejectDtl",
            type: "GET",
            async: false,
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slPicaDetail").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slPicaDetail").append("<option value='" + item.mstPICADetailID + "'>" + item.Description + "</option>");
                })
            }

            $("#slPicaDetail").select2({ placeholder: "Select PICA Detail", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    }
}

var HCISPosition = {
    ARDeptHead: "Account Receivable Database Department Head"
}