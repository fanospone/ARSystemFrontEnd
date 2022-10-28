Data = {};

/* Helper Functions */

jQuery(document).ready(function () {
    window.Parsley.addValidator('period', {
        validateString: function (value) {
            var startDateComponents = Helper.ReverseDateToSQLFormat($("#tbInvoiceStartDate").val()).split("/");
            var endDateComponents = Helper.ReverseDateToSQLFormat($("#tbInvoiceEndDate").val()).split("/");
            var startDate = new Date(parseInt(startDateComponents[2]), parseInt(startDateComponents[0]) - 1, parseInt(startDateComponents[1]));
            var endDate = new Date(parseInt(endDateComponents[2]), parseInt(endDateComponents[0]) - 1, parseInt(endDateComponents[1]));

            return endDate > startDate;
        },
        messages: {
            en: 'The End Invoice Date must be greater than Start Invoice Date'
        }
    });

    Form.Init();
    Table.Init();
    Table.Search();

    // Initialize Datepicker
    $("body").delegate(".datepicker", "focusin", function () {
        $(this).datepicker({
            format: "dd-M-yyyy"
        });
    });

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
    });

    $("#btSubmit").unbind().on("click", function (e) {
        if (Tabs.ProductData.IsValid() && Tabs.ChargeConfiguraton.IsValid() && Tabs.BillingParameters.IsValid()) {
            $("#formTransaction").trigger("submit");
        }
        e.preventDefault();
    });

    // Invoice Form
    $("#formTransaction").submit(function (e) {
        if (Data.Mode == "Create")
            InvoiceOtherProductDetail.Post();
        else if (Data.Mode == "Edit")
            InvoiceOtherProductDetail.Put();
        e.preventDefault();
    });

    // Currency Change Event
    $("#slCurrency").on("change", function () {
        var selectedCurrency = $("#slCurrency").val();
        $("#lbPenalty").val(selectedCurrency);
        $("#lbInvoiceAmount").val(selectedCurrency);
        $("#lbInvoiceTax").val(selectedCurrency);
        $("#lbDiscount").val(selectedCurrency);
        $("#lbPowerAmount").val(selectedCurrency);
        $("#lbTotal").val(selectedCurrency);
    });

    $("#slInvoiceTypeID").on("change", function () {
        Helper.CalculateAmount();
    });

    $(".btCancel").unbind().click(function () {
        Form.Cancel();
    });

    $("#btBack").unbind().click(function () {
        $("#pnlSummary").fadeIn();
        $("#pnlDetailResult").hide();
    });

    $(".fileinput").fileinput();
});

var Form = {
    Init: function () {
        $("#pnlTransaction").hide();
        $("#formTransaction").parsley();

        if (!$("#hdAllowProcess").val()) {
            $("#btCreate").hide();
        }

        Control.LoadChargeEntity();
        Control.LoadChargeFrequency();
        Control.LoadPowerType();
        Control.LoadProductType();
        $("#slCurrency").select2();
        $("#slProduct").select2({placeholder: "Select Product", width: null});

        $("#slProductType").unbind().on("change", function () {
            var productTypeId = $(this).val();
            $("#divSoNumber").hide();
            Tabs.ProductData.Reset();
            Control.LoadProductByType(productTypeId);
        });

        $("#slProduct").unbind().on("change", function () {
            var productId = $(this).val();
            Control.LoadProductById(productId);
        });

        // Bind Prev and Next Button Events
        $("#btNextToChargeConfiguration").unbind().on("click", function () {
            Helper.OpenTab("tabChargeConfiguration");
        });

        $("#btBackToProductData").unbind().on("click", function () {
            Helper.OpenTab("tabProductData");
        });

        $("#btNextToBillingParameters").unbind().on("click", function () {
            Helper.OpenTab("tabBillingParameters");
        }); 

        $("#btBackToChargeConfiguration").unbind().on("click", function () {
            Helper.OpenTab("tabChargeConfiguration");
        });

        $("#btNextToEnergyConsumption").unbind().on("click", function () {
            Helper.OpenTab("tabEnergyConsumption");
        });

        $("#btBackToBillingParameters").unbind().on("click", function () {
            Helper.OpenTab("tabBillingParameters");
        });

        // Define form Input Mask
        Helper.InitCurrencyInput("#tbInvoiceAmount");
        Helper.InitCurrencyInput("#tbInvoiceTax");
        Helper.InitCurrencyInput("#tbInvoicePenalty");
        Helper.InitCurrencyInput("#tbTotal");
        Helper.InitCurrencyInput("#tbDiscount");
        Helper.InitCurrencyInput("#tbPowerAmount");

        Table.Reset();
    },
    Create: function () {
        Helper.OpenTab("tabProductData");

        Data.Mode = "Create";
        var emptyString = "";
        var zero = "0.00";

        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Create Other Product Invoice");
        $("#formTransaction").parsley().reset()

        // Reset Control on Product Data Tab
        $("#slProductType").val(emptyString).trigger("change");
        $("#slProduct").empty();
        $("#slProduct").select2({ placeholder: "Select Product", width: null });
        $("#tbSoNumber").val(emptyString);
        $("#tbProductName").val(emptyString);
        $("#tbSiteName").val(emptyString);
        $("#tbCompanyName").val(emptyString);
        $("#tbCustomerName").val(emptyString);

        // Reset Control on Charge Configuration Tab
        $("#slInvoiceTypeID").val(emptyString).trigger("change");
        $("#slChargeEntityID").val(emptyString).trigger("change");

        // Reset Control on Billing Parameter Tab
        $("#tbInvoiceStartDate").val(emptyString);
        $("#tbInvoiceEndDate").val(emptyString);
        $("#slCurrency").val("IDR").trigger("change");
        $("#tbInflation").val(emptyString);
        $(".fileinput").fileinput("clear");
        $("#tbInvoiceAmount").val(zero);
        $("#tbInvoiceTax").val(zero);
        $("#tbDiscount").val(zero);
        $("#tbInvoicePenalty").val(zero);

        // Reset Control on Energy Consumption Tab
        $("#slPowerTypeID").val(emptyString).trigger("change");
        $("#tbPowerAmount").val(zero);

        // Reset Total
        $("#tbTotal").val(zero);
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

var Tabs = {
    ProductData : {
        Reset: function() {
            $("#tbSoNumber").val("");
            $("#tbSiteName").val("");
            $("#tbCompanyTBG").val("");
            $("#tbCustomerName").val("");
        },
        IsValid: function () {
            var isProductTypeValid = $("#slProductType").parsley().validate();
            var isProductValid = $("#slProduct").parsley().validate();

            if (isProductTypeValid == true && isProductValid == true) {
                return true;
            } else {
                $("#btBackToProductData").trigger("click");
                return false;
            }
        }
    },
    ChargeConfiguraton: {
        Reset: function () {
            $("#slInvoiceTypeID").val("").trigger("change");
            $("#slChargeEntityID").val("").trigger("change");
        },
        IsValid: function () {
            var isInvoiceTypeValid = $("#slInvoiceTypeID").parsley().validate();
            var isChargeEntityValid = $("#slChargeEntityID").parsley().validate();

            if (isInvoiceTypeValid == true && isChargeEntityValid == true) {
                return true;
            } else {
                $("#btBackToChargeConfiguration").trigger("click");
                return false;
            }
        }
    },
    BillingParameters: {
        Reset: function () {
        },
        IsValid: function () {
            var isStartLeaseDateValid = $("#tbInvoiceStartDate").parsley().validate();
            var isEndLeaseDateValid = $("#tbInvoiceEndDate").parsley().validate();

            if (isStartLeaseDateValid == true && isEndLeaseDateValid == true) {
                return true;
            } else {
                $("#btBackToBillingParameters").trigger("click");
                return false;
            }
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
            invoiceNumber: $("#tbSearchInvoiceNumber").val()
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/CreateInvoiceOtherProduct/grid",
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
                { data: "InvNo" },
                { data: "ProductType" },
                { data: "ProductName" },
                { data: "CustomerName" },
                {
                    data: "InvoiceStartDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "InvoiceEndDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "Currency" },
                {
                    data: "InvTotalAmount", render: function (data) {
                        return Common.Format.CommaSeparation(data + "");
                    }
                }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").show();
                }
                l.stop();
            },
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': []
            }],
            "order": [[0, "asc"]]
        });
    },
    Reset: function () {
        $("#tbSearchInvoiceNumber").val("");
    },
    Export: function () {
        var invoiceNumber = $("#tbSearchInvoiceNumber").val();

        window.location.href = "/InvoiceTransaction/TrxCreateInvoiceOtherProduct/Export?invoiceNumber=" + invoiceNumber;
    }
}

var InvoiceOtherProductDetail = {
    Post: function () {
        var invTotalAPPN = parseFloat($("#tbInvoiceTax").val().replace(/,/g, ""));
        var invTotalPenalty = parseFloat($("#tbInvoicePenalty").val().replace(/,/g, ""));
        var invSumADPP = parseFloat($("#tbInvoiceAmount").val().replace(/,/g, ""));
        var invAmount = parseFloat($("#tbTotal").val().replace(/,/g, ""));
        var discount = parseFloat($("#tbDiscount").val().replace(/,/g, ""));
        var powerAmount = parseFloat($("#tbPowerAmount").val().replace(/,/g, ""));

        if (invTotalAPPN == null || invTotalAPPN == undefined || invTotalAPPN == "")
            invTotalAPPN = 0;

        if (invTotalPenalty == null || invTotalPenalty == undefined || invTotalPenalty == "")
            invTotalPenalty = 0;

        if (invSumADPP == null || invSumADPP == undefined || invSumADPP == "")
            invSumADPP = 0;

        if (invAmount == null || invAmount == undefined || invAmount == "")
            invAmount = 0;

        if (discount == null || discount == undefined || discount == "")
            discount = 0;

        if (powerAmount == null || powerAmount == undefined || powerAmount == "")
            powerAmount = 0;

        var formData = new FormData();
        formData.append("InvTotalAPPN", invTotalAPPN);
        formData.append("InvTotalPenalty", invTotalPenalty);
        formData.append("invSumADPP", invSumADPP);
        formData.append("InvTotalAmount", invAmount);
        formData.append("CompanyID", $("#hfCompanyID").val());
        formData.append("OperatorID", $("#hfOperatorID").val());
        formData.append("mstInvoiceCategoryId", 3);
        formData.append("ChargeEntityID", $("#slChargeEntityID").val());
        formData.append("Currency", $("#slCurrency").val());
        formData.append("Discount", discount);
        formData.append("Inflation", $("#tbInflation").val());
        formData.append("InvoiceEndDate", $("#tbInvoiceEndDate").val());
        formData.append("InvoiceStartDate", $("#tbInvoiceStartDate").val());
        formData.append("mstInvoiceTypeId", $("#slInvoiceTypeID").val());
        formData.append("PowerAmount", powerAmount);
        formData.append("PowerTypeID", $("#slPowerTypeID").val());
        formData.append("ProductID", $("#hfProductID").val());
        formData.append("ReconcileDocument", Helper.GetFileName($("#fuReconcileDocument").val()));

        var fileInput = document.getElementById("fuReconcileDocument");
        var file = null;
        if (fileInput.files != undefined) {
            file = fileInput.files[0];
        }

        if(file != null && file != undefined)
            formData.append("File", file);

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/CreateInvoiceOtherProduct",
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
                Common.Alert.Success("Invoice "+ data.InvTemp +" has been created!");
                //Table.Reset();
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
    LoadProductType: function () {
        $.ajax({
            url: "/api/MstProductType?isActive=1",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slProductType").html("<option></option>");
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null)
                        $("#slProductType").append("<option value='" + item.ProductTypeId + "'>" + item.ProductType + "</option>");
                })
            }
            $("#slProductType").select2({ placeholder: "Select Product Type", width: null });
        });
    },
    LoadProductByType: function (productTypeId) {
        if (productTypeId != "" && productTypeId != undefined) {
            $.ajax({
                url: "/api/Product?productTypeId=" + productTypeId,
                type: "GET"
            })
            .done(function (data, textStatus, jqXHR) {
                $("#slProduct").html("<option></option>");
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        if (item != null)
                            $("#slProduct").append("<option value='" + item.ProductId + "'>" + item.ProductName + "</option>");
                    })
                }
                $("#slProduct").select2({ placeholder: "Select Product", width: null });
            });
        }
    },
    LoadProductById: function (productId) {
        if (productId != undefined && productId != "") {
            $.ajax({
                url: "/api/Product?productId=" + productId,
                type: "GET"
            })
            .done(function (data, textStatus, jqXHR) {
                var product = data[0];
                if (product != null && product != undefined) {
                    $("#tbSoNumber").val(product.SoNumber);
                    $("#tbSiteName").val(product.SiteName);
                    $("#tbCompanyTBG").val(product.Company);
                    $("#tbCustomerName").val(product.CustomerName);
                    $("#hfProductID").val(product.ProductId);
                    $("#hfCompanyID").val(product.CompanyID);
                    $("#hfOperatorID").val(product.OperatorID);

                    if (product.RelatedToSonumb) {
                        TableSoNumber.Init(product.ProjectInformations);
                        $("#divSoNumber").show();
                    } else {
                        TableSoNumber.Init([]);
                        $("#divSoNumber").hide();
                    }
                }
            });
        }
    },
    LoadChargeFrequency: function () {
        $.ajax({
            url: "/api/MstDataSource/InvoiceType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slInvoiceTypeID").html("<option></option>");
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null)
                        if (item.Description != "-") {
                            $("#slInvoiceTypeID").append("<option value='" + item.mstInvoiceTypeId + "'>" + item.Description + "</option>");
                        }
                })
            }
            $("#slInvoiceTypeID").select2({ placeholder: "Select Frequency", width: null });
        });
    },
    LoadChargeEntity: function () {
        $.ajax({
            url: "/api/MstDataSource/ChargeEntity",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slChargeEntityID").html("<option></option>");
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null)
                        $("#slChargeEntityID").append("<option value='" + item.ChargeEntityID + "'>" + item.ChargeEntity + "</option>");
                })
            }
            $("#slChargeEntityID").select2({ placeholder: "Select Charge Entity", width: null });
        });
    },
    LoadPowerType: function () {
        $.ajax({
            url: "/api/MstDataSource/PowerType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slPowerTypeID").html("<option></option>");
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null)
                        if(item.PowerType != "-")
                            $("#slPowerTypeID").append("<option value='" + item.KodeType + "'>" + item.BapsType + " - " + item.PowerType + "</option>");
                })
            }
            $("#slPowerTypeID").select2({ placeholder: "Select Power Type", width: null });
        });
    }
}

var Helper = {
    OpenTab: function (tabName) {
        $(".nav-tabs a[href='#" + tabName + "']").tab("show");
    },
    CalculateAmount: function () {
        var invTotalPenalty = parseFloat($("#tbInvoicePenalty").val().replace(/,/g, ""));
        var invSumADPP = parseFloat($("#tbInvoiceAmount").val().replace(/,/g, ""));
        var discount = parseFloat($("#tbDiscount").val().replace(/,/g, ""));
        var powerAmount = parseFloat($("#tbPowerAmount").val().replace(/,/g, ""));

        var totalBeforeTax = invSumADPP + invTotalPenalty + powerAmount - discount;
        var invTotalAPPN = 0.1 * (totalBeforeTax);
        var totalAfterTax = totalBeforeTax + invTotalAPPN;
        $("#tbInvoiceTax").val(Common.Format.CommaSeparation(invTotalAPPN + ""));
        $("#tbTotal").val(Common.Format.CommaSeparation(totalAfterTax + ""));
        return totalAfterTax;
    },
    InitCurrencyInput: function (selector) {
        $(selector).unbind().on("blur", function () {
            var value = $(selector).val();
            $(selector).val(Common.Format.CommaSeparation(value + ""));
            Helper.CalculateAmount();
        })
    },
    GetFileName: function (fullFileName) {
        var fileName = "";
        if(fullFileName != undefined)
            fileName = fullFileName.split(/(\\|\/)/g).pop();

        return fileName;
    },
    ReverseDateToSQLFormat: function (dateValue) {
        var dateComponents = dateValue.split("-");
        var dateValue = dateComponents[0];
        var monthValue = dateComponents[1];
        var yearValue = dateComponents[2];
        var allMonths = "JanFebMarAprMayJunJulAugSepOctNovDec";

        var monthNumberValue = allMonths.indexOf(monthValue) / 3 + 1;

        return monthNumberValue + "/" + dateValue + "/" + yearValue;
    }
}

var TableSoNumber = {
    Init: function (data) {
        var oTable = $("#tblSoNumber").DataTable({
            "data": data,
            "pageLength": 5,
            "lengthMenu": [[-1, 5, 10, 25, 50], ['All', '5', '10', '25', '50']],
            "columnDefs": [{
                'orderable': false,
                'targets': []
            }, {
                "searchable": true,
                "targets": [0, 1, 2]
            }],
            "destroy": true,
            "columns": [
                { data: "SoNumber" },
                { data: "SiteName" },
                { data: "RegionalName" },
            ]
        });
        $("#divSoNumberGrid").show();
    }
}