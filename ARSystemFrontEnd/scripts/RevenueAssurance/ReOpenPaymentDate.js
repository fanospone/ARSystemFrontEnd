Data = {};
Data.RowSelectedRaw = [];
RowSelected = {};
TempData = [];


jQuery(document).ready(function () {

    /*set dafult null forms*/
    $("#tbRemarks").val('');
    $("#tbPaidDateRev").val('');
    $("#tbsPaidDateStart").val('');
    $("#tbsPaidDateEnd").val('');
    $("#tbsCreatedDateStart").val('');
    $("#tbsCreatedDateEnd").val('');
    $("#slsCompanyID").val('').trigger('change');
    $("#slsCustomerID").val('').trigger('change');
    $("#slStatuApproval").val('').trigger('change');
    $("#tbsInvNo").val('');
    $("#tbsRequestNumber").val('');
    $("#tbPaidDateRev").datepicker({
        format: "dd M yyyy",
    });

    Control.Init();
    Control.Buttons();


});

var Form = {
    CreateRequest: function () {

        var Request = { vwData: Data };

        Request.PaymentDateRevision = $("#tbPaidDateRev").val();
        Request.Remarks = $("#tbRemarks").val();
        Request.RequestNumber = TempData.length;

        if (TempData.length > 0) {
            var requestData = [];
            for (var i = 0; i < TempData.length; i++) {
                var requestDetailList = {};
                requestDetailList.TrxInvoiceHeaderID = TempData[i].trxInvoiceHeaderID;
                requestDetailList.PaymentDate = TempData[i].PaymentDate;
                requestData.push(requestDetailList);
            }

            var params = { request: Request, requestDetailList: requestData }
            var l = Ladda.create(document.querySelector("#btSubmitRequest"))
            $.ajax({
                url: "/api/ReOpenPaymentDate/CraeteRequest",
                type: "post",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false,
                beforeSend: function (xhr) {
                    l.start();
                }
            }).done(function (data, textStatus, jqXHR) {
                Common.Alert.Success("Data has been saved!")
                l.stop();
                TempData = [];
                RowSelected = {};
                $(".RequestDetail").hide();
                Table.RequestTbl();
                Control.ResetFormRequest();
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l.stop()
            })
        } else {
            Common.Alert.Warning("Request details cannot be empty");
        }
    },
    UpdateStatus: function () {
        var l = Ladda.create(document.querySelector("#btSubmitSummary"))
        Data = {};
        Data.ID = $("#hdTrxHeaderID").val();
        Data.ApprStatusID = $("#slStatuApproval").val();
        var params = { request: Data };
        $.ajax({
            url: "/api/ReOpenPaymentDate/UpdateRequest",
            type: "post",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            Common.Alert.Success("Data has been submited!")
            l.stop();
            $("#btSummaryClose").trigger('click');
        })
      .fail(function (jqXHR, textStatus, errorThrown) {
          Common.Alert.Error(errorThrown)
          l.stop()
      })
    },
}

var Control = {
    Init: function () {
        /*binding default data */
        Control.BindingSelectCompany();
        PKPPurpose.Set.UserCompanyCode();
        if (PKPPurpose.Temp.UserCompanyCode == Constants.CompanyCode.PKP) {
            PKPPurpose.Filter.PKPOnly();
        }
        Control.BindingSelectOperator();
        Control.BindingApprStatus();
        Helper.BindingDatePicker("#tbsPaidDateStart", "#tbsPaidDateEnd");
        Helper.BindingDatePicker("#tbsCreatedDateStart", "#tbsCreatedDateEnd");

        /*set up forms*/
        $("#tabReOpenPaymentDate").hide();
        $(".RequestDetail").hide();
        $("#SummaryDetail").hide();
        $("#tabReOpenPaymentDate").tabs();
        $("#formSubmitRequest").parsley();
        $("#formSummarySubmit").parsley();
        $('#cbALLData input[type=checkbox]').prop('checked', false);
        
        if ($("#hdRequest").val()) {
            $("#tabReOpenPaymentDate").tabs("option", "active", 0);
            Table.RequestTbl();
            Control.FilterChange(0);
        } else {
            $("#tabReOpenPaymentDate").tabs("option", "active", 1);
            Table.SummaryTbl();
            $("#tabReq").hide();
            Control.FilterChange(1);
            $(".lbPaidDate").text('Paid Date Rev');
        }
    },

    Buttons: function () {
        /*set up buttons*/
        $("#tabReOpenPaymentDate").show();

        $('#tabReOpenPaymentDate').unbind().on('tabsactivate', function (event, ui) {
            var newIndex = ui.newTab.index();
            Control.FilterChange(newIndex);
            if (newIndex == 0) {
                /*show table request*/
                Table.RequestTbl();
                $(".lbPaidDate").text('Paid Date');
                if (TempData.length > 0) {
                    $(".RequestDetail").show();
                }
            } else {
                /*show table summary*/
                $("#SummaryDetail").hide();
                Table.SummaryTbl();
                $("#SummaryHeader").show();
                RowSelected = {};
                $(".lbPaidDate").text('Paid Date Rev');
                $('#cbALLData input[type=checkbox]').prop('checked', false);
            }
        });

        $("#btSearch").unbind().click(function () {

            if ($("#tabReOpenPaymentDate").tabs('option', 'active') == 0) {
                Table.RequestTbl();
            }
            else if ($("#tabReOpenPaymentDate").tabs('option', 'active') == 1) {
                Table.SummaryTbl();
            }
        });

        $("#btAddData").unbind().click(function () {
            if (TempData.length > 0) {
                $(".RequestDetail").show();
                Table.RequestDetailTbl();
                $("#formSubmitRequest").parsley().reset();
            }

        });

        $("#tblRequest").on('change', 'tbody tr .checkboxes', function () {
            var id = $(this).parent().attr('id');
            var table = $("#tblRequest").DataTable();

            var DataRows = {};
            var Row = table.row($(this).parents('tr')).data();

            if (this.checked) {
                DataRows.InvNo = Row.InvNo;
                DataRows.PaymentDate = Row.PaymentDate;
                DataRows.trxInvoiceHeaderID = Row.trxInvoiceHeaderID;
                RowSelected = DataRows;
                TempData.push(RowSelected);
            } else {
                var index = TempData.findIndex(function (data) {
                    return data.trxInvoiceHeaderID == Row.trxInvoiceHeaderID;
                });
                TempData.splice(index, 1);
            }
            Table.RequestDetailTbl();

        });

        $("#btSubmitRequest").unbind().click(function (e) {
            e.preventDefault();
            reqValue = $("#formSubmitRequest").parsley().validate();
            if ($("#formSubmitRequest").parsley().validate()) {
                Form.CreateRequest();
            }

        });

        $("#btSubmitSummary").unbind().click(function (e) {
            e.preventDefault();
            reqValue = $("#formSummarySubmit").parsley().validate();
            if ($("#formSummarySubmit").parsley().validate()) {
                Form.UpdateStatus();
            }

        });

        $("#btSummaryClose").unbind().click(function () {
            $("#SummaryDetail").hide();
            Table.SummaryTbl();
            $("#SummaryHeader").show();

        })

        $("#btResetRequest").unbind().click(function () {
            Control.ResetFormRequest();
        });

        $("#btReset").unbind().click(function () {
            $("#tbsPaidDateStart").val('');
            $("#tbsPaidDateEnd").val('');
            $("#tbsCreatedDateStart").val('');
            $("#tbsCreatedDateEnd").val('');
            $("#slsCompanyID").val('').trigger('change');
            $("#slsCustomerID").val('').trigger('change');
            $("#tbsInvNo").val('');
            $("#tbsRequestNumber").val('');
            if (PKPPurpose.Temp.UserCompanyCode == Constants.CompanyCode.PKP) {
                PKPPurpose.Filter.PKPOnly();
            }
        });

        $("#btRequestCancel").unbind().click(function () {
            $(".RequestDetail").hide()
            Control.ResetFormRequest();
            $(".checkboxes").prop('checked', false);
            TempData = [];
        });

        $("#cbALLData").unbind().change(function () {
            var table = $("#tblRequest").DataTable().data();
            if ($('#cbALLData input[type=checkbox]').prop('checked')) {

                $.each(table, function (i, item) {

                    if (Control.IsEmptyData(item.trxInvoiceHeaderID)) {
                        var DataRows = {};
                        DataRows.InvNo = item.InvNo;
                        DataRows.PaymentDate = item.PaymentDate;
                        DataRows.trxInvoiceHeaderID = item.trxInvoiceHeaderID;
                        RowSelected = DataRows;
                        TempData.push(RowSelected);
                    }
                    $(".RequestDetail").show();
                });

                $(".checkboxes").prop('checked', true);
            } else {
                $(".checkboxes").prop('checked', false);

                $.each(table, function (i, item) {
                    Control.DeleteRequestDetail(item.trxInvoiceHeaderID);
                });
            }
            Table.RequestDetailTbl();

        });
    },

    FilterChange: function (index) {
        $(".RequestDetail").hide();
        if (index == 0) {
            $(".requestForm").show();
            $(".summaryForm").hide();
            $(".btnAction").show();
        } else {
            $(".summaryForm").show();
            $(".requestForm").hide();
            $(".btnAction").hide();
        }
    },

    DeleteRequestDetail: function (rowID) {
        $("#cb_" + rowID).prop('checked', false);
        var index = TempData.findIndex(function (data) {
            return data.trxInvoiceHeaderID == rowID
        });
        TempData.splice(index, 1);
    },

    SelectData: function () {
        if (TempData.length > 0) {
            $.each(TempData, function (i, item) {
                $("#cb_" + item.trxInvoiceHeaderID).prop('checked', true);
            })
        }
    },

    IsEmptyData: function (ID) {
        var result = true;
        if (TempData.length > 0) {
            for (var i = 0 ; i < TempData.length ; i++) {
                if (TempData[i].trxInvoiceHeaderID == ID) {
                    result = false;
                    break;
                }
            }
        }
        return result;
    },

    ResetFormRequest: function () {
        $("#tbRemarks").val('');
        $("#tbPaidDateRev").val('');
    },

    BindingSelectCompany: function () {
        var id = "#slsCompanyID";
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.CompanyId) + "'>" + item.Company + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Company", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectOperator: function () {
        var id = "#slsCustomerID";
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.OperatorId) + "'>" + item.Operator + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Operator", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingApprStatus: function () {
        var id = "#slStatuApproval";
        $.ajax({
            url: "/api/ReOpenPaymentDate/ApprovalStatus",
            type: "POST",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.ID) + "'>" + item.StatusName + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Action", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
}

var Helper = {
    BindingDatePicker: function (startDate,endDate) {
        //$("#tbsPaidDateStart").datepicker({
        //    format: "dd M yyyy",
        //});

        $(startDate).datepicker({
            todayBtn: 1,
            autoclose: true,
            format: "dd M yyyy",
        }).on('changeDate', function (selected) {
            var minDate = new Date(selected.date.valueOf());
            $(endDate).datepicker('setStartDate', minDate);
        });

        $(endDate).datepicker({
            format: "dd M yyyy"
        })
            .on('changeDate', function (selected) {
                var maxDate = new Date(selected.date.valueOf());
                $(startDate).datepicker('setEndDate', maxDate);
            });

    },
    CheckObjectEmpty: function (obj) {
        for (var key in obj) {
            if (obj.hasOwnProperty(key))
                return false;
        }
        return true;
    },
    RemoveElementFromArray: function (arr) {
        var what, a = arguments, L = a.length, ax;
        while (L > 1 && arr.length) {
            what = a[--L];
            while ((ax = arr.indexOf(what)) !== -1) {
                arr.splice(ax, 1);
            }
        }
        return arr;
    },

}

var Table = {

    Init: function (idTable) {

        var tblInit = $(idTable).dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $(idTable).DataTable().columns.adjust().draw();
        });




    },

    RequestTbl: function () {

        // Table.Init("#tblRequest");

        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();

        var params = {
            PaymentDate: $("#tbsPaidDateStart").val(),
            PaymentDate2: $("#tbsPaidDateEnd").val(),

            InvCompanyId: $("#slsCompanyID").val(),
            InvOperatorID: $("#slsCustomerID").val(),
            InvNo: $("#tbsInvNo").val(),
        };

        var tbl = $("#tblRequest").DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "filter": false,
            "destroy": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/ReOpenPaymentDate/Request",
                "type": "POST",
                "cache": false,
                "async": true,
                data: params
            },
            "columns": [
               {
                   orderable: false,
                   mRender: function (data, type, full) {
                       return "<label id='" + full.trxInvoiceHeaderID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input id='cb_" + full.trxInvoiceHeaderID + "' type='checkbox' class='checkboxes' disable/><span></span></label>";
                   }
               },
               { data: "RowIndex" },
                { data: "InvNo" },
                { data: "InvTemp" },
                { data: "InvCompanyId" },
                { data: "InvOperatorID" },
                {
                    data: "PaymentDate",
                    render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "PaidAmount" },
                { data: "trxInvoiceHeaderID" },
            ],
            "columnDefs": [{ "targets": [8], "visible": false }, { "targets": [7], "class": "text-right" }, { "targets": [0, 1], "class": "text-center" }
            ],
            "scrollY": 400,

            "fnDrawCallback": function () {
                l.stop();
                $('#cbALLData input[type=checkbox]').prop('checked', false);
                Control.SelectData();
            },
        });

    },

    RequestDetailTbl: function () {
        Table.Init("#tblRequestDetail");
        $("#tblRequestDetail").DataTable({
            "serverSide": false,
            "filter": false,
            "destroy": true,
            "async": false,
            "data": TempData,
            "columns": [{
                orderable: false,
                mRender: function (data, type, full) {
                    return "<i class='fa fa-remove btn btn-xs red deleteRow'></i>";
                }
            },
            //{ data: "RowIndex" },
            { data: "InvNo" },
            {
                data: "PaymentDate",
                render: function (data) {
                    return Common.Format.ConvertJSONDateTime(data);
                }
            },
            { data: "trxInvoiceHeaderID" },
            ],

            "columnDefs": [{ "targets": [3], "visible": false }, { "targets": [0, 1], "class": "text-center" }]
        });

        $("#tblRequestDetail").unbind();
        $("#tblRequestDetail").on("click", ".deleteRow", function (e) {
            var table = $("#tblRequestDetail").DataTable();
            var row = table.row($(this).parents('tr')).data();
            table.row($(this).parents('tr')).remove().draw(false);
            Control.DeleteRequestDetail(row.trxInvoiceHeaderID);
        });
    },

    SummaryTbl: function () {
        Table.Init("#tblSummary");
        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();

        var params = {
            PaymentDate: $("#tbsPaidDateStart").val(),
            PaymentDate2: $("#tbsPaidDateEnd").val(),
            RequestNumber: $("#tbsRequestNumber").val(),
            CreatedDate: $("#tbsCreatedDateStart").val(),
            CreatedDate2: $("#tbsCreatedDateEnd").val(),
        };

        var tbl = $("#tblSummary").DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "filter": false,
            "destroy": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/ReOpenPaymentDate/SummaryData",
                "type": "POST",
                "cache": false,
                data: params,

            },
            "columns": [
               {
                   orderable: false,
                   mRender: function (data, type, full) {
                       return "<i class='fa fa-eye btn btn-xs green detailSummary'></i>";
                   }
               },
               { data: "RowIndex" },
                { data: "RequestNumber" },
                { data: "ApprStatus" },
                {
                    data: "PaymentDateRevision", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                 {
                     data: "CreateDate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                { data: "Remarks" },
                { data: "ID" },

            ],
            "columnDefs": [{ "targets": [7], "visible": false }, { "targets": [0, 1], "class": "text-center" }],
            "scrollY": 400,
            "fnDrawCallback": function () {
                l.stop();
            },
        });
        $("#tblSummary").unbind();
        $("#tblSummary").on("click", ".detailSummary", function (e) {
            var table = $("#tblSummary").DataTable();
            var row = table.row($(this).parents('tr')).data();
            table.row($(this).parents('tr')).remove().draw(false);
            $("#SummaryHeader").hide();
            $("#SummaryDetail").show();
            Table.SummaryDetailTbl(row.ID, row.PaymentDateRevision);
            $("#formSummarySubmit").parsley().reset();
            if ($("#hdSummaryEdit").val() && (row.ApprStatus.toUpperCase() != "APPROVED" && row.ApprStatus.toUpperCase() != "REJECT"))
                $(".summarySubmit").show();
            else //if ($("#hdSummaryEdit").val() == false || row.ApprStatus.toUpperCase() == "APPROVED")
                $(".summarySubmit").hide();

            $("#hdTrxHeaderID").val(row.ID);
            $("#slStatuApproval").val('').trigger('change');
        });
    },

    SummaryDetailTbl: function (id, paymentDateRevisi) {
        Table.Init("#tblSummaryDetail");
        var params = { HeaderID: id, PaymentDate: $("#tbsPaymentDate").val() };

        var tbl = $("#tblSummaryDetail").DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "filter": false,
            "destroy": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/ReOpenPaymentDate/SummaryDataDetail",
                "type": "POST",
                "cache": false,
                "data": params
            },
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "columns": [
               { data: "RowIndex" },
                { data: "InvNo" },
                {
                    data: "PaymentDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                 {
                     orderable: false,
                     mRender: function (data, type, full) {
                         return Common.Format.ConvertJSONDateTime(paymentDateRevisi);
                     }
                 },
            ],
            "columnDefs": [{ "targets": [0], "class": "text-center" }],
            "scrollY": 390,

        });
    },
}


var Constants = {
    CompanyCode: {
        PKP: "PKP"
    }
}

var PKPPurpose = {
    Filter: {
        PKPOnly: function () {
            $('#slsCompanyID').val(Constants.CompanyCode.PKP).trigger('change');
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