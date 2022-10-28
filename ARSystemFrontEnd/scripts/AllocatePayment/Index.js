params = {};
Data = {};

var trxAllocatePaymentBankInID;
var trxAllocatePaymentBankOutID;
var amountBankOutExs;

jQuery(document).ready(function () {
    Control.Init();
    Control.SetParams();
    Table.Init();
    Table.Search();
});

var Control = {
    Init: function () {
        Control.BindingCompany();
        Control.BindingOperator();

        $("#slStatus").select2({ placeholder: "Select Status", width: null });
        $("#slType").select2({ placeholder: "Select Category", width: null });

        // Initialize Datepicker
        $("body").delegate(".date-picker", "focusin", function () {
            $(this).datepicker({
                autoclose: true,
                format: "dd-M-yyyy",
            })
        });

        //Action
        $("#btSearch").unbind().click(function () {
            Control.SetParams();
            Table.Search();
        });

        $("#btReset").unbind().click(function () {
            $("#slStatus").val("").trigger('change');
            $("#slCompany").val("").trigger('change');
            $("#slCustomer").val("").trigger('change');
            $("#tbStartPaid").val("");
            $("#tbEndPaid").val("");
        });

        $("#btBankIn").unbind().click(function () {
            Control.Reset();
            $('#mdlBankIn').modal('show');
            $('#lblmdlBankIn').text("Add Bank In");
            $('#btSaveBankIn').show();
            $('#btEditBankIn').hide();
        });

        $("#btSaveBankIn").unbind().click(function () {
            Process.SaveBankIn();
        });

        $("#btEditBankIn").unbind().click(function () {
            Process.EditBankIn();
        });

        $("#btAddBankOut").unbind().click(function () {
            Process.AddBankOut();
        });

        $("#btEditBankOut").unbind().click(function () {
            Process.EditBankOut();
        });

        $("#btCancel").unbind().click(function () {
            $('#mdlBankIn').modal('hide');
            Control.Reset();
            Table.Search();
        });

    },

    BindingCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $("#slCompany").html("<option></option>");
                $("#slsCompany").html("<option></option>");

                $.each(data, function (i, item) {
                    $("#slCompany").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                    $("#slsCompany").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })

                $("#slCompany").select2({ placeholder: "Select Company", width: null });
                $("#slsCompany").select2({ placeholder: "Select Company", width: null });
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        })
    },

    BindingOperator: function () {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $("#slCustomer").html("<option></option>");
                $("#slsCustomer").html("<option></option>");

                $.each(data, function (i, item) {
                    $("#slCustomer").append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                    $("#slsCustomer").append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                })

                $("#slCustomer").select2({ placeholder: "Select Customer", width: null });
                $("#slsCustomer").select2({ placeholder: "Select Customer", width: null });
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        })
    },

    SetParams: function () {
        params = {
            vStatus: $('#slStatus').val(),
            vCompany: $('#slCompany').val(),
            vOperator: $('#slCustomer').val(),
            vStartPaid: $('#tbStartPaid').val(),
            vEndPaid: $('#tbEndPaid').val()
        }
    },

    Reset: function () {
        $("#tbPaidDate").val("");
        $("#slsCompany").val("").trigger('change');
        $("#slsCustomer").val("").trigger('change');
        $("#tbAmount").val("0.00");
        $("#tbDescription").val("");

        $("#tbAmountBankOut").val("0.00");
        $("#tbDescriptionBankOut").val("");

        $('#btAdd').show();
        $('#btFormEdit').hide();
    },

    CalculateVariance: function () {
        var AmountBankOut = parseFloat($("#tbAmountBankOut").val().replace(/,/g, ""));

        $.ajax({
            url: "/api/AllocatePayment/getVariance",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            data: { vtrxAllocatePaymentBankInID: trxAllocatePaymentBankInID },
            cache: false,
        })
        .done(function (data, textStatus, jqXHR) {
            $("#tbVariance").val(Common.Format.CommaSeparation(data.data));
            if (data.data != 0) {
                $('#btAddBankOut').prop("disabled", false);
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
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
                $("#tbPaidDate").val(Common.Format.ConvertJSONDateTime(Data.Selected.PaidDate));
                $("#slsCompany").val(Data.Selected.CompanyID).trigger('change');
                $("#slsCustomer").val(Data.Selected.OperatorID).trigger('change');
                $("#tbAmount").val(Common.Format.CommaSeparation(Data.Selected.Amount.toString()));
                $("#tbDescription").val(Data.Selected.Description);

                trxAllocatePaymentBankInID = Data.Selected.trxAllocatePaymentBankInID;
            }
            
            $('#lblmdlBankIn').text("Edit Bank In");
            $('#mdlBankIn').modal('show');
            $('#btSaveBankIn').hide();
            $('#btEditBankIn').show();
        });

        $("#tblSummaryData tbody").on("click", "button.btDelete", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var data = table.row($(this).parents("tr")).data();

            swal({
                title: "Warning",
                text: "Are You Sure Delete This Bank IN ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, Continue!",
                closeOnConfirm: true
            }, function () {
                Table.DeleteBankIn($.trim(data.trxAllocatePaymentBankInID));
            });
        });

        $("#tblSummaryData tbody").on("click", "button.btBankOut", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                $("#tbPaidDateBankIn").val(Common.Format.ConvertJSONDateTime(Data.Selected.PaidDate));
                $("#tbCompany").val(Data.Selected.CompanyID);
                $("#tbOperator").val(Data.Selected.OperatorID);
                $("#tbAmountBankIn").val(Common.Format.CommaSeparation(Data.Selected.Amount.toString()));
                $("#tbDescriptionBankIn").val(Data.Selected.Description);
                $("#tbVariance").val(Common.Format.CommaSeparation(Data.Selected.Unsettle.toString()));

                variance = Data.Selected.Unsettle.toString();

                trxAllocatePaymentBankInID = Data.Selected.trxAllocatePaymentBankInID;

                if (Data.Selected.Status == "Done") {
                    $('#btAddBankOut').prop("disabled", true);
                } else {
                    $('#btAddBankOut').prop("disabled", false);
                }
            }

            Table.BankOut();
            $('#mdlBankOut').modal('show');
            $('#btAddBankOut').show();
            $('#btEditBankOut').hide();

        });

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },

    DeleteBankIn: function (ID) {
        $.ajax({
            type: "GET",
            url: "/api/AllocatePayment/deleteBankIn",
            data: { vtrxAllocatePaymentBankInID: ID },
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (data) {
                Common.Alert.Success("Bank In has been deleted!");
                Table.Search();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/AllocatePayment/grid",
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
                    "data": "id",
                    render: function (data, type, row, meta) {
                        return meta.row + meta.settings._iDisplayStart + 1;
                    }
                },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                            strReturn += "<button type='button' title='Edit' class='btn blue btn-xs btEdit'><i class='fa fa-edit'></i></button>";
                            strReturn += "<button type='button' title='Delete' class='btn red btn-xs btDelete'><i class='fa fa-trash'></i></button>";
                            strReturn += "<button type='button' class='btn yellow btn-xs btBankOut'>Bank Out</button>";
                        return strReturn;
                    }
                },
                {
                    data: "PaidDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "Type" },
                { data: "CompanyID" },
                { data: "OperatorID" },
                { data: "Amount", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Description" },
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "Unsettle", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Status" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").show();
                }
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            "columnDefs": [
                    { "targets": [0, 1, 2, 3, 4, 5, 7, 8, 9], "className": "dt-center" },
                    { "targets": [6], "className": "dt-left" }
            ],
            "order": []
        });
    },

    BankOut: function () {
        var tbBankOut = $("#tbBankOut").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/AllocatePayment/gridBankOut",
                "type": "POST",
                "datatype": "json",
                "data": { vtrxAllocatePaymentBankInID: trxAllocatePaymentBankInID },
                "async": false
            },
            buttons: [],
            "filter": false,
            "lengthMenu": [[5, 10], ['5', '10']],
            "destroy": true,
            "columns": [
                {
                    "data": "id",
                    render: function (data, type, row, meta) {
                        return meta.row + meta.settings._iDisplayStart + 1;
                    }
                },
                { data: "Amount", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Description" },
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<button type='button' title='Edit' class='btn blue btn-xs btEditBankOut'>Edit</button>";
                        strReturn += "<button type='button' title='Edit' class='btn red btn-xs btDeleteBankOut'>Delete</button>";
                        return strReturn;
                    }
                },
            ],
            "columnDefs": [
                    { "targets": [0, 1, 3, 4], "className": "dt-center" },
                    { "targets": [2], "className": "dt-left" }
            ],
            "dom": "<'row'<'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "fnDrawCallback": function () {
                App.unblockUI("#tbBankOut");
            }
        });

        $("#tbBankOut tbody").on("click", "button.btEditBankOut", function (e) {
            var table = $("#tbBankOut").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                $("#tbAmountBankOut").val(Common.Format.CommaSeparation(Data.Selected.Amount.toString()));
                $("#tbDescriptionBankOut").val(Data.Selected.Description);

                trxAllocatePaymentBankOutID = Data.Selected.trxAllocatePaymentBankOutID;
                amountBankOutExs = parseFloat(Data.Selected.Amount).toString();
            }

            $('#btAddBankOut').hide();
            $('#btEditBankOut').show();
        });

        $("#tbBankOut tbody").on("click", "button.btDeleteBankOut", function (e) {
            var table = $("#tbBankOut").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                var AmountBankOut = Data.Selected.Amount;
                variance = AmountBankOut + variance;

                $.ajax({
                    type: "GET",
                    url: "/api/AllocatePayment/deleteBankOut",
                    data: { vtrxAllocatePaymentBankOutID: data.trxAllocatePaymentBankOutID },
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        Common.Alert.Success("Bank Out has been deleted!");
                        Table.BankOut();
                        Table.Search();
                        Control.CalculateVariance();

                        $("#tbAmountBankOut").val("0.00");
                        $("#tbDescriptionBankOut").val("");
                        $('#btAddBankOut').show();
                        $('#btEditBankOut').edit();
                    },
                    error: function (errormessage) {
                        alert(errormessage.responseText);
                    }
                });
            }
        });
    },

    Export: function () {
        Control.SetParams();
        window.location.href = "/AllocatePayment/Summary/Export?" + $.param(params);
    }
}

var Process = {
    SaveBankIn: function () {
        var validate = Process.ValidateBankIn();

        if (validate == "") {
            var params = {
                PaidDate: $("#tbPaidDate").val(),
                TypeID: $("#slType").val(),
                CompanyID: $("#slsCompany").val(),
                OperatorID: $("#slsCustomer").val(),
                Amount: $("#tbAmount").val(),
                Description: $("#tbDescription").val()
            }

            $.ajax({
                url: "/api/AllocatePayment/createBankIn",
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                if (Common.CheckError.Object(data)) {
                    Common.Alert.Success("Success Save Data Bank IN.")
                    Table.Search();
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l.stop();
            })
            .always(function (jqXHR, textStatus) {
                Control.Reset();
            })
        } else {
            Common.Alert.Warning(validate);
        }
    },

    EditBankIn: function () {
        var params = {
            trxAllocatePaymentBankInID: trxAllocatePaymentBankInID,
            PaidDate: $("#tbPaidDate").val(),
            TypeID: $("#slType").val(),
            CompanyID: $("#slsCompany").val(),
            OperatorID: $("#slsCustomer").val(),
            Amount: $("#tbAmount").val(),
            Description: $("#tbDescription").val()
        }

        $.ajax({
            url: "/api/AllocatePayment/editBankIn",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Success Edit Data Bank IN.")
                Table.Search();
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Control.Reset();
        })
    },

    AddBankOut: function () {
        var mandatory = Process.ValidateBankOut();

        var validate = Process.Validate();

        if (mandatory == "") {
            if (validate != "")
                Common.Alert.Warning(validate);
            else {
                var params = {
                    trxAllocatePaymentBankInID: trxAllocatePaymentBankInID,
                    Amount: $("#tbAmountBankOut").val(),
                    Description: $("#tbDescriptionBankOut").val()
                }

                $.ajax({
                    url: "/api/AllocatePayment/addBankOut",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json",
                    data: JSON.stringify(params),
                    cache: false
                }).done(function (data, textStatus, jqXHR) {
                    if (Common.CheckError.Object(data)) {
                        Table.BankOut();
                        Table.Search();
                        Control.CalculateVariance();
                    }
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    Common.Alert.Error(errorThrown)
                    l.stop();
                })
                .always(function (jqXHR, textStatus) {
                    Control.Reset();
                })
            }
        } else {
            Common.Alert.Warning(mandatory);
        }

    },

    EditBankOut: function () {
        var validate = Process.Validate();

        if (validate != "")
        {
            Common.Alert.Warning(validate);
            Control.Reset();
        }
        else {
            var params = {
                trxAllocatePaymentBankOutID: trxAllocatePaymentBankOutID,
                Amount: $("#tbAmountBankOut").val(),
                Description: $("#tbDescriptionBankOut").val()
            }

            $.ajax({
                url: "/api/AllocatePayment/editBankOut",
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                if (Common.CheckError.Object(data)) {
                    Common.Alert.Success("Bank Out has been edit!");
                    Table.BankOut();
                    Table.Search();
                    Control.CalculateVariance();
                    amountBankOutExs = 0;
                    $('#btAddBankOut').show();
                    $('#btEditBankOut').hide();
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l.stop();
            })
            .always(function (jqXHR, textStatus) {
                Control.Reset();
            })
        }
    },

    Validate: function () {
        var Result = {};

        var params = {
            vtrxAllocatePaymentBankInID: trxAllocatePaymentBankInID,
            vAmount: $("#tbAmountBankOut").val(),
            vAmountBankOutExs: amountBankOutExs
        }

        $.ajax({
            url: "/api/AllocatePayment/validateVAR",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params)
        }).done(function (data, textStatus, jqXHR) {
            Result = data;        
        })        
        return Result;
    },

    ValidateBankIn: function () {
        if ($("#tbPaidDate").val() == "")
            return "Paid Date must be filled.";

        if ($("#slType").val() == "")
            return "Type must be selected.";

        if ($("#slsCompany").val() == "")
            return "Company must be selected.";

        if ($("#slsCustomer").val() == "")
            return "Customer must be selected.";

        if ($("#tbAmount").val() == '0.00')
            return "Amount must be filled.";

        if ($("#tbDescription").val() == "")
            return "Description must be filled.";

        return '';
    },

    ValidateBankOut: function () {
        if ($("#tbAmountBankOut").val() == '0.00')
            return "Amount must be filled.";

        if ($("#tbDescriptionBankOut").val() == "")
            return "Description must be filled.";

        return '';
    }
}
