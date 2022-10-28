Data = {};

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

    $("#btCreate").unbind().click(function () {
        Form.Create();
        Table.Reset();
    });

    //panel transaction
    $("#formTransaction").submit(function (e) {
        if ($('#rbCompanyPT').is(':checked'))
            Data.CompanyType = $("#rbCompanyPT").val();
        else if ($('#rbCompanyCV').is(':checked'))
            Data.CompanyType = $("#rbCompanyCV").val();
        if (Data.Mode == "Create")
            CompanyInformation.Post();
        else if (Data.Mode == "Edit")
            CompanyInformation.Put();
        e.preventDefault();
    });


    $(".btCancel").unbind().click(function () {
        Form.Cancel();
        Table.Reset();
    });

    $(".tbAmount").unbind().on("blur", function () {
        Helper.Calculate();
    });

    $("#slTerm").change(function () {
        Helper.Calculate();
    });
});

var Form = {
    Init: function () {
        $("#pnlTransaction").hide();
        $("#formTransaction").parsley();

        if (!$("#hdAllowAdd").val())
            $("#btCreate").hide();

        Control.BindingSelectInvoiceType();
        Table.Reset();

    },
    Create: function () {
        Data.Mode = "Create";
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Create");
        $("#formTransaction").parsley().reset()

        $("#chStatus").bootstrapSwitch("state", true);
        $("#rbCompanyPT").prop("checked", true);
        $("#tbTenant").val("");
        $("#tbTenantId").val("");
        $("#tbCompanyName").val("");
        $("#tbNPWP").val("");
        $("#tbContractNo").val("");
        $('#slTerm').val("").trigger('change');
        $("#tbCompanyAddress").val("");
        $("#tbBillingAddress").val("");
        $("#tbArea").val("");
        $("#tbPricePerArea").val("");
        $("#tbPricePerMonth").val("0.00");
        $("#tbTotalPrice").val("0.00");

    },
    Edit: function () {
        Data.Mode = "Edit";
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Edit");
        $("#formTransaction").parsley().reset()

        
        $("#chStatus").bootstrapSwitch("state", Data.Selected.IsActive);
        if(Data.Selected.CompanyType == "PT")
            $("#rbCompanyPT").prop("checked", true);
        else
            $("#rbCompanyCV").prop("checked", true);
        $("#tbTenant").val(Data.Selected.Tenant);
        $("#tbTenantId").val(Data.Selected.TenantId);
        $("#tbCompanyName").val(Data.Selected.Company);
        $("#tbNPWP").val(Data.Selected.CompanyNPWP);
        $("#tbContractNo").val(Data.Selected.ContractNumber);
        $('#slTerm').val(Data.Selected.InvoiceTypeId).trigger('change');
        $("#tbCompanyAddress").val(Data.Selected.CompanyAddress);
        $("#tbBillingAddress").val(Data.Selected.BillingAddress);
        $("#tbArea").val(Data.Selected.Area);
        $("#tbPricePerArea").val(Data.Selected.MeterPrice);
        $("#tbPricePerMonth").val(Data.Selected.MonthlyPrice);
        $("#tbTotalPrice").val(Data.Selected.TotalPrice);
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

        $("#tblSummaryData tbody").on("click", "button.btEdit", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                Form.Edit();
                Table.Reset();
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
            strCompany: $("#tbSearchCompanyName").val(),
            strTerm: $("#slSearchTerm").val(),
            intIsActive: ($("#rbSearchStatusActive").is(":checked") ? 1 : ($("#rbSearchStatusInactive").is(":checked") ? 0 : -1))
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/CompanyInformation/grid",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>' },
                {
                    text: '<i class="fa fa-file-excel-o" title="Export to Excel"></i>', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>' }
               
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        if ($("#hdAllowEdit").val())
                            strReturn += "<button type='button' title='Edit' class='btn btn-xs green btEdit'><i class='fa fa-edit'></i></button>";
                        return strReturn;
                    }
                },
                {
                    mRender: function (data, type, full) {
                        return full.CompanyType + " " + full.Tenant;
                    }
                },
                { data: "Company" },
                { data: "TermPeriod" },
                {
                    mRender: function (data, type, full) {
                        return full.IsActive ? "Yes" : "No";
                    }
                },

            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop();
            }
        });
    },
    Reset: function () {
        $("#tbSearchCompanyName").val("");
        $('#slSearchTerm').val("").trigger('change');
        $("#rbSearchStatusAll").prop("checked", true);
    },
    Export: function () {

        var strCompany= $("#tbSearchCompanyName").val();
        var strTerm= $("#slSearchTerm").val();
        var intIsActive = ($("#rbSearchStatusActive").is(":checked") ? 1 : ($("#rbSearchStatusInactive").is(":checked") ? 0 : -1));

        window.location.href = "/Admin/CompanyInformation/Export?strCompany=" + strCompany + "&strTerm=" + strTerm
            + "&intIsActive=" + intIsActive;
    }
}

var CompanyInformation = {
    Post: function () {
        var Area = parseFloat($("#tbArea").val().replace(/,/g, ""));
        var MeterPrice = parseFloat($("#tbPricePerArea").val().replace(/,/g, ""));
        var MonthlyPrice = parseFloat($("#tbPricePerMonth").val().replace(/,/g, ""));
        var TotalPrice = parseFloat($("#tbTotalPrice").val().replace(/,/g, ""));

        if (Area == null || Area == undefined || Area == "")
            Area = 0;

        if (MeterPrice == null || MeterPrice == undefined || MeterPrice == "")
            MeterPrice = 0;

        if (MonthlyPrice == null || MonthlyPrice == undefined || MonthlyPrice == "")
            MonthlyPrice = 0;

        if (TotalPrice == null || TotalPrice == undefined || TotalPrice == "")
            TotalPrice = 0;

        var params = {

                CompanyType: Data.CompanyType,
                Tenant: $("#tbTenant").val(),
                TenantId: $("#tbTenantId").val(),
                Company: $("#tbCompanyName").val(),
                CompanyNPWP: $("#tbNPWP").val(),
                ContractNumber: $("#tbContractNo").val(),
                InvoiceTypeId: $("#slTerm").val(),
                TermPeriod: $("#slTerm option:selected").text(),
                CompanyAddress: $("#tbCompanyAddress").val(),
                BillingAddress: $("#tbBillingAddress").val(),
                Area: Area,
                MeterPrice: MeterPrice,
                MonthlyPrice: MonthlyPrice,
                TotalPrice: TotalPrice,
                IsActive: $("#chStatus").bootstrapSwitch("state")
        }

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/CompanyInformation",
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
                Common.Alert.Success("Company Information has been created!")
                Form.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    },

    Put: function () {
        var Area = parseFloat($("#tbArea").val().replace(/,/g, ""));
        var MeterPrice = parseFloat($("#tbPricePerArea").val().replace(/,/g, ""));
        var MonthlyPrice = parseFloat($("#tbPricePerMonth").val().replace(/,/g, ""));
        var TotalPrice = parseFloat($("#tbTotalPrice").val().replace(/,/g, ""));

        if (Area == null || Area == undefined || Area == "")
            Area = 0;

        if (MeterPrice == null || MeterPrice == undefined || MeterPrice == "")
            MeterPrice = 0;

        if (MonthlyPrice == null || MonthlyPrice == undefined || MonthlyPrice == "")
            MonthlyPrice = 0;

        if (TotalPrice == null || TotalPrice == undefined || TotalPrice == "")
            TotalPrice = 0;
        var params = {
            CompanyType: Data.CompanyType,
            Tenant: $("#tbTenant").val(),
            TenantId: $("#tbTenantId").val(),
            Company: $("#tbCompanyName").val(),
            CompanyNPWP: $("#tbNPWP").val(),
            ContractNumber: $("#tbContractNo").val(),
            InvoiceTypeId: $("#slTerm").val(),
            TermPeriod: $("#slTerm option:selected").text(),
            CompanyAddress: $("#tbCompanyAddress").val(),
            BillingAddress: $("#tbBillingAddress").val(),
            Area: Area,
            MeterPrice: MeterPrice,
            MonthlyPrice: MonthlyPrice,
            TotalPrice: TotalPrice,
            IsActive: $("#chStatus").bootstrapSwitch("state")
        }

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/CompanyInformation/" + Data.Selected.mstCompanyInformationId,
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
                Common.Alert.Success("Company Information has been edited!")
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

var Control = {
    BindingSelectInvoiceType: function () {
        $.ajax({
            url: "/api/MstDataSource/InvoiceType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchTerm").html("<option></option>")
            $("#slTerm").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchTerm").append("<option value='" + item.mstInvoiceTypeId + "'>" + item.Description + "</option>");
                    $("#slTerm").append("<option value='" + item.mstInvoiceTypeId + "'>" + item.Description + "</option>");
                })
            }

            $("#slSearchTerm").select2({ placeholder: "Select Term Invoice", width: null });
            $("#slTerm").select2({ placeholder: "Select Term Invoice", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    }
}

var Helper = {
    Calculate: function ()
    {
        var multiply = 0;
        if ($('#slTerm').val() == "0001")
            multiply = 1;
        else if ($('#slTerm').val() == "0002")
            multiply = 3;
        else if ($('#slTerm').val() == "0004")
            multiply = 6;
        else if ($('#slTerm').val() == "0003")
            multiply = 12;
        var Area = parseFloat($("#tbArea").val().replace(/,/g, ""));
        var PricePerArea = parseFloat($("#tbPricePerArea").val().replace(/,/g, ""));
        var PricePerMonth = (Area * PricePerArea).toFixed(2);
        var TotalPrice = (PricePerMonth * multiply).toFixed(2);
        $("#tbArea").val(Common.Format.CommaSeparation($("#tbArea").val()));
        $("#tbPricePerArea").val(Common.Format.CommaSeparation($("#tbPricePerArea").val()));
        $("#tbPricePerMonth").val(Common.Format.CommaSeparation(PricePerMonth));
        $("#tbTotalPrice").val(Common.Format.CommaSeparation(TotalPrice));
    }
}

