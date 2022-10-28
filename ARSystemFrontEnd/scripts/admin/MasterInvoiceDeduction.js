Data = {};

/* Helper Functions */

var fsOperatorId = "";
var fsCompanyId = "";

jQuery(document).ready(function () {
    window.Parsley.addValidator('period', {
        validateString: function (value) {
            var startDateComponents = Helper.ReverseDateToSQLFormat($("#tbStartPeriod").val()).split("/");
            var endDateComponents = Helper.ReverseDateToSQLFormat($("#tbEndPeriod").val()).split("/");
            var startDate = new Date(parseInt(startDateComponents[2]), parseInt(startDateComponents[0]) - 1, parseInt(startDateComponents[1]));
            var endDate = new Date(parseInt(endDateComponents[2]), parseInt(endDateComponents[0]) - 1, parseInt(endDateComponents[1]));

            return endDate > startDate;
        },
        messages: {
            en: 'The End Date Period must be greater than Start Date Period'
        }
    });

    Helper.OpenTab("#tabSKB");
    Data.DeductionType = "SKB";
    Form.Init();
    Table.Init();
    TableWAPU.Init();
    Table.Search();

    //panel Summary
    $("#formSearch").submit(function (e) {
        if ($("#tabDeductionType").tabs('option', 'active') == 0)
            Table.Search();
        else
            TableWAPU.Search();
        e.preventDefault();
    });

    $("#btSearch").unbind().click(function () {
        if ($("#tabDeductionType").tabs('option', 'active') == 0)
            Table.Search();
        else
            TableWAPU.Search();
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
    });

    $("#btCreateSKB").unbind().click(function () {
        Data.DeductionType = "SKB";
        Form.Init();
        Form.Create("");
    });

    $("#btCreateWAPU").unbind().click(function () {
        Data.DeductionType = "WAPU";
        Form.Init();
        Form.Create();
    });

    //panel transaction Header
    $("#formTransaction").submit(function (e) {
        e.preventDefault();
        if (Data.Mode == "Create") {
            if (Data.DeductionType == "SKB") {
                InvoiceDeduction.PostSKB();
            } else {
                InvoiceDeduction.PostWAPU();
            }
        } else if (Data.Mode == "Edit") {
            if (Data.DeductionType == "SKB") {
                InvoiceDeduction.PutSKB();
            } else {
                InvoiceDeduction.PutWAPU();
            }
        }
    });

    $(".btCancel").unbind().click(function () {
        Form.Cancel();
        Table.Reset();
    });

    $("#btBack").unbind().click(function () {
        $("#pnlSummary").fadeIn();
        $("#pnlDetailResult").hide();
    });

    Control.BindingSelectCompany();
    Control.BindingSelectOperator();
    Control.BindingSelectDeductionType();

    $(".fileinput").fileinput();

    Helper.InitCurrencyInput("#tbAmountWAPU");

    $("#tabDeductionType").tabs({
        activate: function (event, ui) {
            var newIndex = ui.newTab.index();
            if (newIndex == 0) {
                Helper.OpenTab("tabSKB");
                Data.DeductionType = "SKB";
                Form.Init();
                Table.Search();
            } else {
                Helper.OpenTab("tabWAPU");
                Data.DeductionType = "WAPU";
                Form.Init();
                TableWAPU.Search();
            }
        }
    });

    $("#btDownload").unbind().on("click", function (e) {
        Helper.Download(Data.Selected.FilePath, Data.Selected.UploadBA, Data.Selected.ContentType);
    });

    $("#slCurrency").select2({ placeholder: "Select Currency", width: null });
});

var Form = {
    Init: function () {
        $("#pnlTransaction").hide();
        $("#formTransaction").parsley();

        if (!$("#hdAllowAdd").val()) {
            $("#btCreate").hide();
        }

        if (Data.DeductionType == "SKB") {
            $("#divOperator").hide();
            $("#slOperator").removeAttr("required");
            $("#divAmountWAPU").hide();
            $("#tbAmountWAPU").removeAttr("required");

            $("#slCompany").attr("required", "required");
            $("#divCompany").show();
            $("#tbStartPeriod").attr("required", "required");
            $("#divStartPeriod").show();
            $("#tbEndPeriod").attr("required", "required");
            $("#divEndPeriod").show();
            $("#fuUploadBA").attr("required", "required");
            $("#divUploadBA").show();
        } else {
            $("#divOperator").show();
            $("#slOperator").attr("required", "required");
            $("#divAmountWAPU").show();
            $("#tbAmountWAPU").attr("required", "required");

            $("#slCompany").removeAttr("required");
            $("#divCompany").hide();
            $("#tbStartPeriod").removeAttr("required");
            $("#divStartPeriod").hide();
            $("#tbEndPeriod").removeAttr("required");
            $("#divEndPeriod").hide();
            $("#fuUploadBA").removeAttr("required");
            $("#divUploadBA").hide();
        }

        Table.Reset();
    },
    Create: function () {
        Data.Mode = "Create";
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Add New Invoice Deduction");
        $("#formTransaction").parsley().reset();

        $("#tbDeductionType").val(Data.DeductionType);
        $("#slCompany").val("").trigger("change");
        $("#tbStartPeriod").val("");
        $("#tbEndPeriod").val("");
        $("#slOperator").val("").trigger("change");
        $("#tbAmountWAPU").val("0.00");
        $("#chIsActive").bootstrapSwitch("state", true);
        $("#lblUploadBA").html('Upload BA<i class="font-red">*</i>');
        $(".fileinput").fileinput("clear");

        if(Data.DeductionType == "SKB")
            $("#fuUploadBA").attr("required", "required");
        else
            $("#fuUploadBA").removeAttr("required");

        $("#divAttachment").hide();
    },
    Edit: function () {
        Data.Mode = "Edit";
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Edit Invoice Deduction");
        $("#formTransaction").parsley().reset();

        $("#tbDeductionType").val(Data.DeductionType);
        $("#slCompany").val(Data.Selected.CompanyId).trigger("change");
        $("#tbStartPeriod").val(Common.Format.ConvertJSONDateTime(Data.Selected.StartPeriod));
        $("#tbEndPeriod").val(Common.Format.ConvertJSONDateTime(Data.Selected.EndPeriod));
        $("#slOperator").val(Data.Selected.OperatorId).trigger("change");
        $("#tbAmountWAPU").val(Common.Format.CommaSeparation(Data.Selected.AmountWAPU));
        $("#chIsActive").bootstrapSwitch("state", Data.Selected.RelatedToSonumb);
        $("#lblUploadBA").html('Upload BA');
        $(".fileinput").fileinput("clear");
        $("#fuUploadBA").removeAttr("required");

        if (Data.DeductionType == "SKB") {
            if (Data.Selected.UploadBA != null && Data.Selected.UploadBA != undefined) {
                $("#btDownload").html('<i class="fa fa-download"></i>&nbsp;' + Data.Selected.UploadBA);
                $("#divAttachment").show();
            } else {
                $("#divAttachment").hide();
            }
        } else {
            $("#divAttachment").hide();
        }
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

        $("#tbDeductionType").val("");
        $("#slCompany").val("").trigger("change");
        $("#tbStartPeriod").val("");
        $("#tbEndPeriod").val("");
        $("#slOperator").val("").trigger("change");
        $("#tbAmountWAPU").val("0.00");
        $(".fileinput").fileinput("clear");

        if (Data.DeductionType == "SKB")
            Table.Search();
        else
            TableWAPU.Search();
    }
}

var Table = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblSummaryData = $('#tblSummaryDataSKB').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $("#tblSummaryDataSKB tbody").on("click", "button.btEdit", function (e) {
            var table = $("#tblSummaryDataSKB").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                Form.Edit();
            }
        });

        $(window).resize(function () {
            $("#tblSummaryDataSKB").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsOperatorId = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();

        var params = {
            operatorId: fsOperatorId,
            companyId: fsCompanyId,
            mstDeductionTypeId: DeductionType.SKB
        };

        var tblSummaryData = $("#tblSummaryDataSKB").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/MstInvoiceDeduction/gridSKB",
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
                        //strReturn += "<button type='button' title='Detail' class='btn green btDetail'><i class='fa fa-search-plus'></i></button>";
                        if ($("#hdAllowEdit").val())
                            strReturn += "<button type='button' title='Edit' class='btn btn-xs green btEdit'><i class='fa fa-edit'></i></button>";
                        return strReturn;
                    }
                },
                { data: "CompanyId" },
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
                {
                    render: function (data, type, full) {
                        if (full.UploadBA != null && full.UploadBA != '')
                            return '<button type="button" class="btn btn-xs btn-primary btn-download" id="btDownload_' + full.mstInvoiceDeductionId + '"><i class="fa fa-download"></i></button>';
                        return '';
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
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': [0, 4]
            }],
            "order": []
        });

        $("#tblSummaryDataSKB tbody").on("click", "button.btn-download", function (e) {
            var table = $("#tblSummaryDataSKB").DataTable();
            var row = table.row($(this).parents('tr')).data();
            Helper.Download(row.FilePath, row.UploadBA, row.ContentType);
        });
    },
    Reset: function () {
        fsCompanyId = "";
        fsOperatorId = "";
        $("#slSearchOperator").val("").trigger("change");
        $("#slSearchCompany").val("").trigger("change");
    },
    Export: function () {
        window.location.href = "/Admin/InvoiceDeductionSKB/Export?companyId=" + fsCompanyId + "&operatorId=" + fsOperatorId + "&deductionTypeId=" + DeductionType.SKB;
    }
}

var TableWAPU = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblSummaryData = $('#tblSummaryDataWAPU').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $("#tblSummaryDataWAPU tbody").on("click", "button.btEdit", function (e) {
            var table = $("#tblSummaryDataWAPU").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                Form.Edit();
            }
        });

        $(window).resize(function () {
            $("#tblSummaryDataWAPU").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsOperatorId = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();

        var params = {
            operatorId: fsOperatorId,
            companyId: fsCompanyId,
            mstDeductionTypeId: DeductionType.WAPU
        };

        var tblSummaryData = $("#tblSummaryDataWAPU").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/MstInvoiceDeduction/gridSKB",
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
                        TableWAPU.Export()
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
                        //strReturn += "<button type='button' title='Detail' class='btn green btDetail'><i class='fa fa-search-plus'></i></button>";
                        if ($("#hdAllowEdit").val())
                            strReturn += "<button type='button' title='Edit' class='btn btn-xs green btEdit'><i class='fa fa-edit'></i></button>";
                        return strReturn;
                    }
                },
                { data: "OperatorId" },
                {
                    data: "AmountWAPU", className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
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
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': [0]
            }],
            "order": []
        });
    },
    Reset: function () {
        fsCompanyId = "";
        fsOperatorId = "";
        $("#slSearchOperator").val("").trigger("change");
        $("#slSearchCompany").val("").trigger("change");
    },
    Export: function () {
        window.location.href = "/Admin/InvoiceDeductionWAPU/Export?companyId=" + fsCompanyId + "&operatorId=" + fsOperatorId + "&deductionTypeId=" + DeductionType.WAPU;
    }
}

var InvoiceDeduction = {
    PostSKB: function () {
        var formData = new FormData();
        formData.append("InvoiceDeductionId", null);
        formData.append("DeductionTypeId", DeductionType.SKB);
        formData.append("CompanyId", $("#slCompany").val());
        formData.append("StartPeriod", Helper.ReverseDateToSQLFormat($("#tbStartPeriod").val()));
        formData.append("EndPeriod", Helper.ReverseDateToSQLFormat($("#tbEndPeriod").val()));
        formData.append("IsActive", $("#chIsActive").bootstrapSwitch("state"));

        var fileInput = document.getElementById("fuUploadBA");
        var file = null;
        if (fileInput.files != undefined) {
            file = fileInput.files[0];
        }

        if (file != null && file != undefined)
            formData.append("File", file);

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/MstInvoiceDeduction/SaveSKB",
            type: "POST",
            dataType: "json",
            contentType: false,
            data: formData,
            processData: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Invoice Deduction has been created!");
                Table.Reset();
                Form.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        });
    },
    PostWAPU: function () {
        var params = {
            mstDeductionTypeId: DeductionType.WAPU,
            OperatorId: $("#slOperator").val(),
            AmountWAPU: $("#tbAmountWAPU").val().replace("/g/,", ""),
            IsActive: $("#chIsActive").bootstrapSwitch("state")
        }

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/MstInvoiceDeduction/SaveWAPU",
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
                Common.Alert.Success("Invoice Deduction has been created!");
                Table.Reset();
                Form.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    },
    PutSKB: function () {
        var formData = new FormData();
        formData.append("InvoiceDeductionId", Data.Selected.mstInvoiceDeductionId);
        formData.append("DeductionTypeId", DeductionType.SKB);
        formData.append("CompanyId", $("#slCompany").val());
        formData.append("StartPeriod", Helper.ReverseDateToSQLFormat($("#tbStartPeriod").val()));
        formData.append("EndPeriod", Helper.ReverseDateToSQLFormat($("#tbEndPeriod").val()));
        formData.append("IsActive", $("#chIsActive").bootstrapSwitch("state"));

        var fileInput = document.getElementById("fuUploadBA");
        var file = null;
        if (fileInput.files != undefined) {
            file = fileInput.files[0];
        }

        if (file != null && file != undefined)
            formData.append("File", file);

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/MstInvoiceDeduction/UpdateSKB",
            type: "PUT",
            dataType: "json",
            contentType: false,
            data: formData,
            processData: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Invoice Deduction has been updated!");
                Table.Reset();
                Form.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        });
    },
    PutWAPU: function () {
        var params = {
            mstDeductionTypeId: DeductionType.WAPU,
            OperatorId: $("#slOperator").val(),
            AmountWAPU: $("#tbAmountWAPU").val().replace("/g/,", ""),
            IsActive: $("#chIsActive").bootstrapSwitch("state")
        }

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/MstInvoiceDeduction/UpdateWAPU/" + Data.Selected.mstInvoiceDeductionId,
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
                Common.Alert.Success("Master Invoice Deduction has been edited!");
                Table.Reset();
                Form.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    },
}

var Control = {
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchCompany").html("<option></option>");
            $("#slCompany").html("<option></option>");

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slCompany").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                    $("#slSearchCompany").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                });
            }

            $("#slSearchCompany").select2({ placeholder: "Select Company", width: null });
            $("#slCompany").select2({ placeholder: "Select Company", width: null });

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
            $("#slSearchOperator").html("<option></option>");
            $("#slOperator").html("<option></option>");

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slOperator").append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                    $("#slSearchOperator").append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                })
            }

            $("#slSearchOperator").select2({ placeholder: "Select Operator", width: null });
            $("#slOperator").select2({ placeholder: "Select Operator", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectDeductionType: function () {
        $.ajax({
            url: "/api/MstDataSource/DeductionType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchDeductionType").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchDeductionType").append("<option value='" + item.mstDeductionTypeId + "'>" + item.DeductionType + "</option>");
                })
            }

            $("#slSearchDeductionType").select2({ placeholder: "Select Deduction Type", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    }
}

var Helper = {
    OpenTab: function (tabName) {
        $(".nav-tabs a[href='#" + tabName + "']").tab("show");
    },
    ReverseDateToSQLFormat: function (dateValue) {
        var dateComponents = dateValue.split("-");
        var dateValue = dateComponents[0];
        var monthValue = dateComponents[1];
        var yearValue = dateComponents[2];
        var allMonths = "JanFebMarAprMayJunJulAugSepOctNovDec";

        var monthNumberValue = allMonths.indexOf(monthValue) / 3 + 1;

        return monthNumberValue + "/" + dateValue + "/" + yearValue;
    },
    InitCurrencyInput: function (selector) {
        $(selector).unbind().on("blur", function () {
            var value = $(selector).val();
            if (value != "") {
                $(selector).val(Helper.FormatCurrency(value));
            } else {
                $(selector).val("0.00");
            }
        })
    },
    Download: function (filePath, invReceiptFile, contentType) {
        var docPath = $("#hfDocPath").val();
        var path = docPath + filePath;
        window.location.href = "/Admin/Download?path=" + path + "&fileName=" + invReceiptFile + "&contentType=" + contentType;
    },
    FormatCurrency: function (value) {
        if (isNaN(value))
            return "0.00";
        else
            return Common.Format.CommaSeparation(value);
    }
}

var DeductionType = {
    SKB: 1,
    WAPU: 2
}