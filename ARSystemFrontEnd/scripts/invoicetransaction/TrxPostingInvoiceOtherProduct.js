Data = {};

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

    // Invoice Form
    $("#formTransaction").submit(function (e) {
        if ($(this).parsley().validate())
            Process.Posting();
        e.preventDefault();
    });

    $("#btCancelInvoiceTemp").on("click", function () {
        Process.CancelInvoiceTemp();
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

    $("#tbInvoiceDate").datepicker({
        format: "dd-M-yyyy"
    });
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
        Control.LoadSignatureUser();
        $("#slCurrency").select2();

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

        Table.Reset();
    },
    SelectedData: function () {
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Posting Other Product Invoice");
        $("#formTransaction").parsley().reset();

        $("#tbInvoiceDate").val("");
        $("#taDescription").val("");
        $("#slSignature").val("").trigger("change");

        // Tab Product Data
        $("#tbProductType").val(Data.Selected.ProductType);
        $("#tbProduct").val(Data.Selected.ProductName);

        Control.LoadProductById(Data.Selected.ProductID);
        $("#tbCompanyTBG").val(Data.Selected.Company);
        $("#tbCustomerName").val(Data.Selected.CustomerName);

        // Tab Charge Configuration
        $("#slInvoiceTypeID").val(Data.Selected.mstInvoiceTypeId).trigger("change");
        $("#slChargeEntityID").val(Data.Selected.ChargeEntityID).trigger("change");

        // Tab Billing Parameters
        $("#tbInvoiceStartDate").val(Common.Format.ConvertJSONDateTime(Data.Selected.InvoiceStartDate));
        $("#tbInvoiceEndDate").val(Common.Format.ConvertJSONDateTime(Data.Selected.InvoiceEndDate));
        $("#slCurrency").val(Data.Selected.Currency).trigger("change");
        $("#tbInflation").val(Data.Selected.Inflation);
        $("#tbInvoiceAmount").val(Common.Format.CommaSeparation(Data.Selected.InvSumADPP));
        $("#tbInvoiceTax").val(Common.Format.CommaSeparation(Data.Selected.InvTotalAPPN));
        $("#tbDiscount").val(Common.Format.CommaSeparation(Data.Selected.Discount));
        $("#tbInvoicePenalty").val(Common.Format.CommaSeparation(Data.Selected.InvTotalPenalty));

        // Tab Energy Consumption
        $("#slPowerTypeID").val(Data.Selected.PowerTypeID).trigger("change");
        $("#tbPowerAmount").val(Common.Format.CommaSeparation(Data.Selected.PowerAmount));

        // Total
        $("#tbTotal").val(Common.Format.CommaSeparation(Data.Selected.InvTotalAmount));

        Helper.OpenTab("tabProductData");

        // Show / Hide Download Button
        if (Data.Selected.ReconcileDocument != null && Data.Selected.ReconcileDocument != undefined && Data.Selected.ReconcileDocument != "") {
            var iconDownload = "<i class='fa fa-download'></i> ";
            $("#btDownloadReconcileDocument").html(iconDownload);
            $("#btDownloadReconcileDocument").append(Data.Selected.ReconcileDocument);
            $("#divDownloadReconcileDocument").show();

            $("#btDownloadReconcileDocument").unbind().on("click", function (e) {
                var docPath = $("#hfDocPath").val();
                var path = docPath + Data.Selected.FilePath;
                var fileName = Data.Selected.ReconcileDocument;
                var contentType = Data.Selected.ContentType;
                window.location.href = "/Admin/Download?path=" + path + "&fileName=" + fileName + "&contentType=" + contentType;
            });
        }
        else
            $("#divDownloadReconcileDocument").hide();
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
    ProductData: {
        Reset: function () {
            $("#tbSoNumber").val("");
            $("#tbSiteName").val("");
            $("#tbCompanyTBG").val("");
            $("#tbCustomerName").val("");
        },
        IsValid: function () {
            return true;
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
                "url": "/api/PostingInvoiceOtherProduct/grid",
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
                        strReturn += "<button type='button' title='Select' class='btn btn-xs blue btSelect'><i class='fa fa-mouse-pointer'></i></button>";

                        return strReturn;
                    }
                },
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
                    $(".panelSearchResult").fadeIn();
                }
                l.stop();
            },
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': []
            }],
            "order": [[1, "asc"]]
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
        $("#tbSearchInvoiceNumber").val("");
    },
    Export: function () {
        var invoiceNumber = $("#tbSearchInvoiceNumber").val();

        window.location.href = "/InvoiceTransaction/TrxCreateInvoiceOtherProduct/Export?invoiceNumber=" + invoiceNumber;
    }
}

var Control = {
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
                            if (item.PowerType != "-")
                                $("#slPowerTypeID").append("<option value='" + item.KodeType + "'>" + item.BapsType + " - " + item.PowerType + "</option>");
                    })
                }
                $("#slPowerTypeID").select2({ placeholder: "Select Power Type", width: null });
            });
    },
    LoadSignatureUser: function () {
        $.ajax({
            url: "/api/MstDataSource/SignatureUser",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $("#slSignature").html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $("#slSignature").append("<option value='" + item.UserID + "'>" + item.FullName + "</option>");
                    })
                }

                $("#slSignature").select2({ placeholder: "Select Signature", width: null });

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
        if (fullFileName != undefined)
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
            url: "/api/PostingInvoiceOtherProduct",
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
            invoiceDate: $("#tbInvoiceDate").val()
        }

        $.ajax({
            url: "/api/PostingInvoiceOtherProduct/Cancel",
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
                Common.Alert.Success("Invoice Rejected Successfully!")
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
};