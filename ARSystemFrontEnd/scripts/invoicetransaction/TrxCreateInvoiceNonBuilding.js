Data = {};

var fsCompanyName = "";
var fsInvoiceTypeId = "";
var fsInvNo = "";
var fsEditPrice = "0";

/* Helper Functions */

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
            en: 'The End Period Date must be greater than Start Period Date'
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
        e.preventDefault();
        if ($("#formTransaction").parsley().validate()) {
            if (Data.Mode == "Create")
                InvoiceBuilding.Post();
        }
    });

    $(".btCancel").unbind().click(function () {
        Form.Cancel();
    });

    $("#btBack").unbind().click(function () {
        $("#pnlSummary").fadeIn();
        $("#pnlDetailResult").hide();
    });

    $('input[type=radio][name=rdCustomerType]').change(function () {
        Control.LoadCompanyByType($(this).val());
        Form.Clear();
    });

    //$("#slCompanyInformationID").unbind().on("change", function () {
    //    //var companyId = $(this).val();
    //   // Control.LoadCompanyByID(companyId);
    //});
    //* added by MTR *//
    $("#slCategoryBuildingID").unbind().on("change", function () {
        var categBuildId = $("#slCategoryBuildingID").val();
        if (categBuildId != "") {
            Helper.CalculateAmount();
        }
    });
    $("#btnEditTrx").unbind().click(function () {
        fsEditPrice = fsEditPrice == "0" ? "1" : "0";
        if (fsEditPrice=="0")
            $("#tbTotalPrice").prop("readonly", true);
        else {
            $("#tbTotalPrice").prop("readonly", false);
            $("#btnEditTrx").prop("disabled", "disabled");
        }
            
    });
});

var Form = {
    Init: function () {
        $("#pnlTransaction").hide();
        $("#formTransaction").parsley();

        if (!$("#hdAllowProcess").val()) {
            $("#btCreate").hide();
        }

        Control.BindingSelectInvoiceType();
        Control.LoadCompanyByType("1");
        Control.BindingSelectCompany();
        Control.BindingSelectCategoryBuilding();
        Control.BindingSelectInvoiceType();

        Helper.InitCurrencyInput("#tbDiscount");
        Helper.InitCurrencyInput("#tbPenalty");
        Helper.InitCurrencyInput("#tbTotalPrice");

        $("#tbStartPeriod").unbind().on("blur", function () {
            Control.BindCalculateAmount();
        });
        $("#tbEndPeriod").unbind().on("blur", function () {
            Control.BindCalculateAmount();
        });

        // Helper.InitCurrencyInput("#tbPPN");
        // Helper.InitCurrencyInput("#tbPPH");

        Table.Reset();
    },
    Create: function () {
        Form.Clear();
        Data.Mode = "Create";
        var emptyString = "";
        var zero = "0.00";

        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Create Building Invoice");
        $("#formTransaction").parsley().reset()

        $("#slCompanyInformationID").val("").trigger("change");
        $("#slCategoryBuildingID").val("").trigger("change");

        // Reset Total
        $("#tbStartPeriod").val(emptyString);
        $("#tbEndPeriod").val(emptyString);
        $("#slCompanyTBG").val(emptyString).trigger("change");
        $("#tbDiscount").val(zero);
        $("#tbPenalty").val(zero);
        $("#tbTotalAmount").val(zero);
        fsEditPrice = "0";
        $("#btnEditTrx").prop("disabled", "");
        $("#tbTotalPrice").prop("readonly", true);
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
    },
    Clear: function () {
        var emptyString = "";
        var zero = "0.00";
        //$("#tbArea").val(zero);
        $("#tbTermPeriod").val(emptyString);
        $("#tbTotalPrice").val(zero);
        //$("#tbPricePerArea").val(zero);
        //$("#tbPricePerMonth").val(zero);
        //$("#tbDPP").val(zero);
        $("#tbPPN").val(zero);
        $("#tbPPH").val(zero);
        $("#tbTotalAmount").val(zero);
        $("#tbDPP").val(zero);
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

        fsCompanyName = $("#tbSearchCompanyName").val(),
        fsInvoiceTypeId = $("#slSearchTerm").val() == null ? "" : $("#slSearchTerm").val(),
        fsInvNo = $("#tbInvNo").val()

        var params = {
            companyName: fsCompanyName,
            invoiceTypeId: fsInvoiceTypeId,
            invNo: fsInvNo,
            InvoiceCategory: "Non Building"
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/CreateInvoiceBuilding/grid",
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
                { data: "InvTemp" },
                { data: "CompanyName" },
                { data: "CategoryBuildingName" },
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
                { data: "InvoiceType" },
                {
                    data: "InvTotalAmount", className: "text-right", render: function (data, type, full) {
                        return Helper.FormatCurrency(data);
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
                    data: "InvTotalAPPH", className: "text-right", render: function (data) {
                        return Helper.FormatCurrency(data);
                    }
                },
                {
                    data: "InvTotalPenalty", className: "text-right", render: function (data) {
                        return Helper.FormatCurrency(data);
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
                    $(".panelSearchResult").show();
                }
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': []
            }],
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 1 /* Set the 2 most left columns as fixed columns */
            },
            "order": []
        });
    },
    Reset: function () {
        fsCompanyName = "";
        fsInvoiceTypeId = "";
        fsInvNo = "";
        $("#tbSearchCompanyName").val("");
        $("#slSearchTerm").val("").trigger("change");
        $("#tbInvNo").val("");
    },
    Export: function () {
        window.location.href = "/InvoiceTransaction/TrxCreateInvoiceBuilding/Export?companyName=" + fsCompanyName + "&invoiceTypeId=" + fsInvoiceTypeId + "&invNo=" + fsInvNo;
    }
}

var InvoiceBuilding = {
    Post: function () {
        var invTotalAPPN = parseFloat($("#tbPPN").val().replace(/,/g, ""));
        var invTotalPenalty = parseFloat($("#tbPenalty").val().replace(/,/g, ""));
        var invSumADPP;// = parseFloat($("#tbDPP").val().replace(/,/g, ""));
        var invAmount = parseFloat($("#tbTotalAmount").val().replace(/,/g, ""));
        var discount = parseFloat($("#tbDiscount").val().replace(/,/g, ""));
        var invTotalAPPH = parseFloat($("#tbPPH").val().replace(/,/g, ""));
        var monthlyPrice = parseFloat($("#tbTotalPrice").val().replace(/,/g, ""));
        if (invTotalAPPN == null || invTotalAPPN == undefined || invTotalAPPN == "")
            invTotalAPPN = 0;

        if (invTotalAPPH == null || invTotalAPPH == undefined || invTotalAPPH == "")
            invTotalAPPH = 0;

        if (invTotalPenalty == null || invTotalPenalty == undefined || invTotalPenalty == "")
            invTotalPenalty = 0;

        //if (invSumADPP == null || invSumADPP == undefined || invSumADPP == "")
        //    invSumADPP = 0;

        if (invAmount == null || invAmount == undefined || invAmount == "")
            invAmount = 0;

        if (discount == null || discount == undefined || discount == "")
            discount = 0;

        var params = {
            InvSumADPP: monthlyPrice - discount,
            InvTotalPenalty: invTotalPenalty,
            Discount: discount,
            InvTotalAPPN: invTotalAPPN,
            invTotalAPPH: invTotalAPPH,
            InvTotalAmount: invAmount,
            IsTower: false,
            Currency: "IDR",
            StartPeriod: $("#tbStartPeriod").val(),
            EndPeriod: $("#tbEndPeriod").val(),
            CustomerID: $("#slCompanyInformationID").val(),
            InvCompanyId: $("#slCompanyTBG").val(),
            CategoryBuildingID: $("#slCategoryBuildingID").val(),
            InvoiceCategoryName: "Non Building",
            InvoiceTypeId: $("#slTerm").val(),
            MonthlyPrice: monthlyPrice,
        }

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/CreateInvoiceBuilding",
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
                Common.Alert.Success("Invoice " + data.InvTemp + " has been created!");
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
    LoadCompanyByType(companyType) {
        var params = {
            isActive: 1
        }
        $.ajax({
            url: "/api/MstDataSource/CustomerByCustomerType?CustomerTypeID=" + companyType,
            type: "GET",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slCompanyInformationID").html("<option></option>");
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null) {
                        $("#slCompanyInformationID").append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                        // $("#divHiddenFields").append("<input type='hidden' id='hf_" + item.CustomerID + "' TermPeriod='" + item.TermPeriod + "' TotalPrice='" + item.TotalPrice + "' MonthlyPrice='" + item.MonthlyPrice + "' Area='" + item.Area + "' MeterPrice='" + item.MeterPrice + "' />");
                    }
                })
            }
            $("#slCompanyInformationID").select2({ placeholder: "Select Company", width: null });
        });
    },
    LoadCompanyByID: function (customerId) {
        if (customerId != "") {
            if ($("#slCategoryBuildingID").val() == "") {
                Common.Alert.Warning("Please select category building!");
            } else {
                $.ajax({
                    url: "/api/CustomerTenant?customerId=" + customerId,
                    type: "GET"
                })
           .done(function (data, textStatus, jqXHR) {
               if (Common.CheckError.List(data)) {
                   var item = data[0];
                   if (item != null) {
                       $("#tbTotalPrice").val(Helper.FormatCurrency(item.TotalPrice));
                       $("#tbPricePerArea").val(Helper.FormatCurrency(item.MeterPrice));
                       $("#tbPricePerMonth").val(Helper.FormatCurrency(item.MonthlyPrice));
                       Helper.CalculateAmount();
                   }
               }
           });
            }
        }
    },
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
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slCompanyTBG").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slCompanyTBG").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
            }

            $("#slCompanyTBG").select2({ placeholder: "Select Company", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectCategoryBuilding: function () {
        var param = { strBuildingType: '0' }
        $.ajax({
            url: "/api/MstDataSource/CategoryBuilding",
            type: "GET",
            data: param
        })
      .done(function (data, textStatus, jqXHR) {
          $("#slCategoryBuildingID").html("<option></option>")

          if (Common.CheckError.List(data)) {
              $.each(data, function (i, item) {
                  $("#slCategoryBuildingID").append("<option value='" + item.Value + "'>" + item.Text + "</option>");
              })
          }

          $("#slCategoryBuildingID").select2({ placeholder: "Select Category Building", width: null });

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
            //$("#slSearchTerm").html("<option></option>")
            $("#slTerm").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    //$("#slSearchTerm").append("<option value='" + item.mstInvoiceTypeId + "'>" + item.Description + "</option>");
                    $("#slTerm").append("<option value='" + item.mstInvoiceTypeId + "'>" + item.Description + "</option>");
                })
            }

            //$("#slSearchTerm").select2({ placeholder: "Select Term Invoice", width: null });
            $("#slTerm").select2({ placeholder: "Select Term Invoice", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindCalculateAmount: function () {
        var startDate = $("#tbStartPeriod").val();
        var endDate = $("#tbEndPeriod").val();
        var categoryBuilding = $("#slCategoryBuildingID").val();
        if (startDate != "" && endDate != "" && categoryBuilding != "")
            Helper.CalculateAmount();
    },
}

var Helper = {
    CalculateAmount: function () {
        var ppn = parseFloat($("#tbPPN").val().replace(/,/g, ""));
        var pph = parseFloat($("#tbPPH").val().replace(/,/g, ""));
        var dpp = parseFloat($("#tbDPP").val().replace(/,/g, ""));
        var discount = parseFloat($("#tbDiscount").val().replace(/,/g, ""));
        var penalty = parseFloat($("#tbPenalty").val().replace(/,/g, ""));
        var totalPrice = parseFloat($("#tbTotalPrice").val().replace(/,/g, ""));
        //* added by MTR *//
        var CategoryBuildingID = $("#slCategoryBuildingID").val();
        if (fsEditPrice == "1") {
            $.ajax({
                url: "/api/CategoryBuilding/GetDetail?ID=" + CategoryBuildingID,
                type: "GET"
            })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                dpp = totalPrice - discount;
                ppn = dpp * parseFloat(data.PPNValue);
                pph = dpp * parseFloat(data.PPHValue);
                var totalInvoice = dpp + ppn - penalty;
                $("#tbDPP").val(Helper.FormatCurrency(dpp));
                $("#tbPPN").val(Helper.FormatCurrency(ppn));
                $("#tbPPH").val(Helper.FormatCurrency(pph));
                $("#tbTotalAmount").val(Helper.FormatCurrency(totalInvoice));
                return totalInvoice;
            }
        });
        } else {
            var params = { CatagoryBuildingID: CategoryBuildingID, startDate: $("#tbStartPeriod").val(), endDate: $("#tbEndPeriod").val(), Area: "1" };

            $.ajax({
                //url: "/api/CategoryBuilding/GetDetail?ID=" + CategoryBuildingID,
                url: "/api/CreateInvoiceBuilding/GetProrateTotalPrice",
                type: "GET",
                data: params
            })
             .done(function (data, textStatus, jqXHR) {
                 if (Common.CheckError.Object(data)) {
                     dpp = data.TotalPrice - discount;
                     ppn = dpp * parseFloat(data.PPNValue);
                     pph = dpp * parseFloat(data.PPHValue);
                     var totalInvoice = dpp + ppn - penalty;
                     $("#tbTotalPrice").val(Common.Format.CommaSeparation(data.TotalPrice));
                     $("#tbDPP").val(Helper.FormatCurrency(dpp));
                     $("#tbPPN").val(Helper.FormatCurrency(ppn));
                     $("#tbPPH").val(Helper.FormatCurrency(pph));
                     $("#tbTotalAmount").val(Helper.FormatCurrency(totalInvoice));
                     return totalInvoice;
                 }
             });
        }
    },
    FormatCurrency: function (value) {
        if (isNaN(value))
            return "0.00";
        else
            return parseFloat(value, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
    },
    InitCurrencyInput: function (selector) {
        $(selector).unbind().on("blur", function () {
            var value = $(selector).val();
            if (value != "") {
                $(selector).val(Common.Format.CommaSeparation(value));

            } else {
                $(selector).val("0.00");
            }
            Helper.CalculateAmount();
        })
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