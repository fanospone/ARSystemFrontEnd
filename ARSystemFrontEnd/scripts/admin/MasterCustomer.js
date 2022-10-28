Data = {};

jQuery(document).ready(function () {
    window.Parsley.addValidator('phonenumber', {
        validateString: function (value) {
            return Validator.ValidatePhoneNumber(value);
        },
        messages: {
            en: 'The Phone Number is invalid'
        }
    });

    Control.BindSearchCustomerTypeRadio();
    Control.BindCustomerTypeRadio();
    Control.BindLegalProvinceSelect();
    Control.BindBillingProvinceSelect();

    Form.Init();
    Table.Init();
    Table.Search();

    // Initialize Datepicker
    $("body").delegate(".datepicker", "focusin", function () {
        $(this).datepicker({
            format: 'dd-M-yy'
        });
    });

    var im = new Inputmask("99.999.999.9-999.999");
    im.mask($("#tbNPWP"));

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

    $("#btSubmit").on('click', function () {
        $("#form").submit();
    });

    $("#form").on('submit', function (e) {
        if ($("#form").parsley().validate()) {
            if (Data.Mode == "Create") {
                Customer.Post();
            } else if (Data.Mode == "Edit") {
                Customer.Put();
            }
        }
        e.preventDefault();
    });

    $(".tbAmount").unbind().on("blur", function () {
        Helper.Calculate();
    });

    $("#slTerm").change(function () {
        Helper.Calculate();
    });

    $(".btCancel").unbind().click(function () {
        Form.Cancel();
        Table.Reset();
    });

    $("#btBack").unbind().click(function () {
        $("#pnlSummary").fadeIn();
        $("#pnlDetailResult").hide();
    });
});

/**
 * Customer
 */

var Form = {
    Init: function () {
        $("#pnlTransaction").hide();
        $("#form").parsley();

        if (!$("#hdAllowAdd").val()) {
            $("#btCreate").hide();
        }

        // Define Select2 on dropdown list
        $("#slLegalProvince").select2({ placeholder: "Select Province", width: null });
        $("#slLegalRegency").select2({ placeholder: "Select Region", width: null });
        $("#slBillingProvince").select2({ placeholder: "Select Province", width: null });
        $("#slBillingRegency").select2({ placeholder: "Select Region", width: null });

        //Customer Form
        $("#dvContactPerson").hide();

        // Hide / show the contat person field when the corporate customer type is selected
        $("#form").unbind().on('click', '.customer-type-options', function (e) {
            var radioButton = $(this);
            if (radioButton.val() != CustomerType.Personal) {
                $("#lbCustomerName").html("Company Name<i class='font-red'>*</i>");
                $("#tbContactPerson").attr('required', 'required');
                $("#dvContactPerson").show();
            } else {
                $("#lbCustomerName").html("Customer Name<i class='font-red'>*</i>");
                $("#tbContactPerson").removeAttr('required');
                $("#dvContactPerson").hide();
            }

            if (radioButton.val() == CustomerType.Building) {
                $("#dvCompanyType").show();
                $("#dvContractNumber").show();
                $("#dvTermPeriod").show();
                $("#dvArea").show();
                $("#dvPricePerArea").hide();
                $("#dvPricePerMonth").hide();
                $("#dvTotalPrice").hide();
                Helper.SetBuildingFieldsRequired(true);
            } else {
                $("#dvCompanyType").hide();
                $("#dvContractNumber").hide();
                $("#dvTermPeriod").hide();
                $("#dvArea").hide();
                $("#dvPricePerArea").hide();
                $("#dvPricePerMonth").hide();
                $("#dvTotalPrice").hide();
                Helper.SetBuildingFieldsRequired(false);
            }
        });

        // Change Legal Province Event
        $("#slLegalProvince").unbind().on('change', function (e) {
            var provinceId = $("#slLegalProvince").val();
            if (provinceId != 0 && provinceId != "")
                Control.BindLegalRegencySelect(provinceId);
        });

        // Change Billing Province Event
        $("#slBillingProvince").unbind().on('change', function (e) {
            var provinceId = $("#slBillingProvince").val();
            if (provinceId != 0 && provinceId != "")
                Control.BindBillingRegencySelect(provinceId);
        });

        // Copy Legal Address to Billing Address
        $("#chCopyFromLegalAddress").on('switchChange.bootstrapSwitch', function (event, state) {
            if (state) {
                $("#tbBillingAddress").val($("#tbLegalAddress").val());
                $("#slBillingProvince").val($("#slLegalProvince").val()).trigger('change');
                var regencyId = $("#slLegalRegency").val();
                Control.BindBillingRegencySelect($("#slBillingProvince").val(), regencyId);
            } else {
                $("#tbBillingAddress").val("");
                $("#slBillingProvince").val("").trigger('change');
                $("#slBillingRegency").empty();
                $("#slBillingRegency").select2({ placeholder: "Select Region", width: null });
            }
        });
        Control.BindingSelectInvoiceType();
        Table.Reset();
    },
    Create: function () {
        Data.Mode = "Create";
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Create Customer");
        $("#form").parsley().reset()

        // Form Fields
        $("#chStatus").bootstrapSwitch("state", true);
        $("#tbCustomerCode").removeAttr("readonly");
        $("#tbCustomerCode").val("");

        //$("input[name=rbCustomerType][value=" + CustomerType.Personal + "]").prop('checked', true);
        $("input[name=rbCustomerType][value=" + CustomerType.Corporate + "]").prop('checked', true);
        $("#lbCustomerName").html("Company Name<i class='font-red'>*</i>");
        $("#tbContactPerson").removeAttr('required');
        $("#dvContactPerson").hide();
        $("#dvCompanyType").hide();
        $("#dvContractNumber").hide();
        $("#dvTermPeriod").hide();
        $("#dvArea").hide();
        $("#dvPricePerArea").hide();
        $("#dvPricePerMonth").hide();
        $("#dvTotalPrice").hide();
        $("#tbCustomerName").val("");

        $("#tbLegalAddress").val("");
        $("#slLegalProvince").val("").trigger('change');
        $("#slLegalRegency").empty();
        $("#slLegalRegency").select2({ placeholder: "Select Region", width: null });

        $("#tbBillingAddress").val("");
        $("#slBillingProvince").val("").trigger('change');
        $("#slBillingRegency").empty();
        $("#slBillingRegency").select2({ placeholder: "Select Region", width: null });

        $("#chCopyFromLegalAddress").bootstrapSwitch("state", false);

        $("#tbPhoneNumber").val("");
        $("#tbEmail").val("");
        $("#tbNPWP").val("");
        $("#tbContactPerson").val("");

        $("#tbContractNo").val("");
        $("#slTerm").val("").trigger("change");
        $("#tbArea").val("");
        $("#tbPricePerArea").val("");
        $("#tbPricePerMonth").val("");
        $("#tbTotalPrice").val("");
        $("#rbCompanyPT").prop("checked", true);

        Helper.SetBuildingFieldsRequired(false);
    },
    Edit: function () {
        Data.Mode = "Edit";
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();
        $("#panelTransactionTitle").text("Edit Customer");
        $("#form").parsley().reset();

        $("#chStatus").bootstrapSwitch("state", Data.Selected.IsActive);
        if (Data.Selected.CustomerTypeID == CustomerType.Building) {
            $("#dvCompanyType").show();
            $("#dvContractNumber").show();
            $("#dvTermPeriod").show();
            $("#dvArea").show();
            $("#dvPricePerArea").hide();
            $("#dvPricePerMonth").hide();
            $("#dvTotalPrice").hide();
            Helper.SetBuildingFieldsRequired(true);
        } else {
            $("#dvCompanyType").hide();
            $("#dvContractNumber").hide();
            $("#dvTermPeriod").hide();
            $("#dvArea").hide();
            $("#dvPricePerArea").hide();
            $("#dvPricePerMonth").hide();
            $("#dvTotalPrice").hide();
            Helper.SetBuildingFieldsRequired(false);
        }

        if (Data.Selected.CompanyType == "PT")
            $("#rbCompanyPT").prop("checked", true);
        else
            $("#rbCompanyCV").prop("checked", true);

        $("input[name=rbCustomerType][value=" + Data.Selected.CustomerTypeID + "]").prop('checked', true);
        $("#tbCustomerName").val(Data.Selected.CustomerName);

        if (Data.Selected.BillingAddress == Data.Selected.LegalAddress
            && Data.Selected.BillingProvinceID == Data.Selected.LegalProvinceID
            && Data.Selected.BillingRegencyID == Data.Selected.LegalRegencyID
        ) {
            $("#chCopyFromLegalAddress").bootstrapSwitch("state", true);
        } else {
            $("#chCopyFromLegalAddress").bootstrapSwitch("state", false);
        }

        var dfd = $.Deferred();
        dfd.done(Control.SetLegalProvinceSelect(),
            Control.BindLegalRegencySelect(Data.Selected.LegalProvinceID, Data.Selected.LegalRegencyID),
            Control.SetBillingProvinceSelect(),
            Control.BindBillingRegencySelect(Data.Selected.BillingProvinceID, Data.Selected.BillingRegencyID));

        //$("#tbCustomerCode").attr("readonly", "readonly");
        $("#tbCustomerCode").val(Data.Selected.CustomerCode);
        $("#tbLegalAddress").val(Data.Selected.LegalAddress);
        $("#tbBillingAddress").val(Data.Selected.BillingAddress);
        $("#tbPhoneNumber").val(Data.Selected.PhoneNumber);
        $("#tbEmail").val(Data.Selected.Email);
        $("#tbNPWP").val(Data.Selected.NPWP);
        $("#tbContactPerson").val(Data.Selected.ContactPerson);
        $("#tbContractNo").val(Data.Selected.ContractNumber);
        $("#slTerm").val(Data.Selected.InvoiceTypeId).trigger("change");
        $("#tbArea").val(Helper.CommaSeparation3(Data.Selected.Area));
        $("#tbPricePerArea").val(Common.Format.CommaSeparation(Data.Selected.MeterPrice));
        $("#tbPricePerMonth").val(Common.Format.CommaSeparation(Data.Selected.MonthlyPrice));
        $("#tbTotalPrice").val(Common.Format.CommaSeparation(Data.Selected.TotalPrice));

        if (Data.Selected.CustomerTypeID != CustomerType.Personal) {
            $("#lbCustomerName").html("Company Name<i class='font-red'>*</i>");
            $("#tbContactPerson").attr('required', 'required');
            $("#dvContactPerson").show();
        } else {
            $("#lbCustomerName").html("Customer Name<i class='font-red'>*</i>");
            $("#tbContactPerson").removeAttr('required');
            $("#dvContactPerson").hide();
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

        $("#tblSummaryData tbody").unbind().on("click", "button.btEdit", function (e) {
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

        var customerTypeId = $("input:radio[name='rbSearchCustomerType']:checked").val();
        var params = {
            customerName: $("#tbSearchCustomerName").val(),
            customerTypeId: customerTypeId,
            intIsActive: ($("#rbSearchStatusActive").is(":checked") ? 1 : ($("#rbSearchStatusInactive").is(":checked") ? 0 : -1))
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/CustomerTenant/grid",
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
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        //strReturn += "<button type='button' title='Detail' class='btn green btDetail'><i class='fa fa-search-plus'></i></button>";
                        if ($("#hdAllowEdit").val())
                            strReturn += "<button type='button' title='Edit' class='btn btn-xs green btEdit'><i class='fa fa-mouse-pointer'></i></button>";
                        return strReturn;
                    }
                },
                { data: "CustomerName" },
                { data: "CustomerType" },
                { data: "CustomerCode" },
                { data: "LegalAddress" },
                { data: "LegalProvince" },
                { data: "LegalRegion" },
                { data: "BillingAddress" },
                { data: "BillingProvince" },
                { data: "BillingRegion" },
                { data: "PhoneNumber" },
                { data: "Email" },
                { data: "NPWP" },
                { data: "ContactPerson" },
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
            "order": [[1, "asc"]],
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
        });
    },
    Reset: function () {
        $("#tbSearchCustomerName").val("");
        $("input[name=rbSearchCustomerType][value=0]").prop('checked', true);
        $("#rbSearchStatusAll").prop("checked", true);
    },
    Export: function () {
        var customerName = $("#tbSearchCustomerName").val();
        var customerType = $("input[name='rbSearchCustomerType']:checked").val();
        var status = $("input[name='rdStatus']:checked").val();

        window.location.href = "/Admin/Customer/Export?customerName=" + customerName + "&customerType=" + customerType + "&status=" + status;
    }
}

var Customer = {
    Post: function () {
        var customerTypeId = $("#form input:radio[name='rbCustomerType']:checked").val();
        if (customerTypeId == CustomerType.Personal) {
            $("#tbContactPerson").val("");
        }

        var companyType = $("#form input:radio[name='rdStatus']:checked").val();

        if(customerTypeId != CustomerType.Building) {
            $("#tbContractNo").val("");
            $("#slTerm").val("").trigger("change");
            $("#tbArea").val("");
            $("#tbPricePerArea").val("");
            $("#tbPricePerMonth").val("");
            $("#tbTotalPrice").val("");
            companyType = null;
        }

        var params = {
            CustomerName: $("#tbCustomerName").val(),
            CustomerCode: $("#tbCustomerCode").val(),
            CustomerTypeID: customerTypeId,
            LegalAddress: $("#tbLegalAddress").val(),
            LegalProvinceID: $("#slLegalProvince").val(),
            LegalRegencyID: $("#slLegalRegency").val(),
            BillingAddress: $("#tbBillingAddress").val(),
            BillingProvinceID: $("#slBillingProvince").val(),
            BillingRegencyID: $("#slBillingRegency").val(),
            PhoneNumber: $("#tbPhoneNumber").val(),
            Email: $("#tbEmail").val(),
            NPWP: $("#tbNPWP").val(),
            ContactPerson: $("#tbContactPerson").val(),
            IsActive: $("#chStatus").bootstrapSwitch("state"),
            CompanyType: companyType,
            ContractNumber: $("#tbContractNo").val(),
            InvoiceTypeId: $("#slTerm").val(),
            TermPeriod: $("#slTerm option:selected").text(),
            Area: $("#tbArea").val(),
            MeterPrice: $("#tbPricePerArea").val(),
            MonthlyPrice: $("#tbPricePerMonth").val(),
            TotalPrice: $("#tbTotalPrice").val()
        }

        var l = Ladda.create(document.querySelector("#btSubmit"));
        $.ajax({
            url: "/api/CustomerTenant",
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
                Common.Alert.Success("Customer has been created!");
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

    Put: function () {
        var customerTypeId = $("#form input:radio[name='rbCustomerType']:checked").val();
        if (customerTypeId == CustomerType.Personal) {
            $("#tbContactPerson").val("");
        }

        var companyType = $("#form input:radio[name='rdStatus']:checked").val();

        if (customerTypeId != CustomerType.Building) {
            $("#tbContractNo").val("");
            $("#slTerm").val("").trigger("change");
            $("#tbArea").val("");
            $("#tbPricePerArea").val("");
            $("#tbPricePerMonth").val("");
            $("#tbTotalPrice").val("");
            companyType = null;
        }

        var params = {
            CustomerName: $("#tbCustomerName").val(),
            CustomerTypeID: customerTypeId,
            CustomerCode: $("#tbCustomerCode").val(),
            LegalAddress: $("#tbLegalAddress").val(),
            LegalProvinceID: $("#slLegalProvince").val(),
            LegalRegencyID: $("#slLegalRegency").val(),
            BillingAddress: $("#tbBillingAddress").val(),
            BillingProvinceID: $("#slBillingProvince").val(),
            BillingRegencyID: $("#slBillingRegency").val(),
            PhoneNumber: $("#tbPhoneNumber").val(),
            Email: $("#tbEmail").val(),
            NPWP: $("#tbNPWP").val(),
            ContactPerson: $("#tbContactPerson").val(),
            IsActive: $("#chStatus").bootstrapSwitch("state"),
            CompanyType: companyType,
            ContractNumber: $("#tbContractNo").val(),
            InvoiceTypeId: $("#slTerm").val(),
            TermPeriod: $("#slTerm option:selected").text(),
            Area: $("#tbArea").val(),
            MeterPrice: $("#tbPricePerArea").val(),
            MonthlyPrice: $("#tbPricePerMonth").val(),
            TotalPrice: $("#tbTotalPrice").val()
        }

        var l = Ladda.create(document.querySelector("#btSubmit"));
        $.ajax({
            url: "/api/CustomerTenant/" + Data.Selected.CustomerID,
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
                Common.Alert.Success("Customer has been edited!");
                Table.Reset();
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
    BindCustomerTypeRadio: function () {
        $.ajax({
            url: "/api/MstDataSource/CustomerType",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(".customer-types").append("<label class='mt-radio'><input type='radio' class='customer-type-options' name='rbCustomerType' value='" + item.CustomerTypeId + "' /> " + item.CustomerType + "<span></span></label>");
                    })
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindSearchCustomerTypeRadio: function () {
        $.ajax({
            url: "/api/MstDataSource/CustomerType",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(".customer-types-field")
                            .append("<label class='mt-radio'><input type='radio' class='customer-type-options' name='rbSearchCustomerType' value='" + item.CustomerTypeId + "' /> " + item.CustomerType + "<span></span></label>");
                    })
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindLegalProvinceSelect: function () {
        $.ajax({
            url: "/api/MstDataSource/Province",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $("#slLegalProvince").html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        if (item != null)
                            $("#slLegalProvince").append("<option value='" + item.ProvinceID + "'>" + item.ProvinceName + "</option>");
                    })
                }
                $("#slLegalProvince").select2({ placeholder: "Select Province", width: null });
            });
    },
    BindLegalRegencySelect: function (provinceId, selectedRegencyId) {
        $.ajax({
            url: "/api/MstDataSource/Regency?provinceId=" + provinceId,
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $("#slLegalRegency").html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        if (item != null)
                            $("#slLegalRegency").append("<option value='" + item.RegencyID + "'>" + item.RegencyName + "</option>");
                    })
                }
                $("#slLegalRegency").select2({ placeholder: "Select Region", width: null });

                if (selectedRegencyId != 0 && selectedRegencyId != "") {
                    $("#slLegalRegency").val(selectedRegencyId).trigger('change');
                }
            });
    },
    BindBillingProvinceSelect: function () {
        $.ajax({
            url: "/api/MstDataSource/Province",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $("#slBillingProvince").html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        if (item != null)
                            $("#slBillingProvince").append("<option value='" + item.ProvinceID + "'>" + item.ProvinceName + "</option>");
                    })
                }
                $("#slBillingProvince").select2({ placeholder: "Select Province", width: null });
            });
    },
    BindBillingRegencySelect: function (provinceId, selectedRegencyId) {
        $.ajax({
            url: "/api/MstDataSource/Regency?provinceId=" + provinceId,
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slBillingRegency").html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if(item != null)
                        $("#slBillingRegency").append("<option value='" + item.RegencyID + "'>" + item.RegencyName + "</option>");
                })
            }
            $("#slBillingRegency").select2({ placeholder: "Select Region", width: null });

            if (selectedRegencyId != 0 && selectedRegencyId != "" && selectedRegencyId != undefined) {
                $("#slBillingRegency").val(selectedRegencyId).trigger('change');
            }
        });
    },
    SetLegalProvinceSelect: function () {
        return $("#slLegalProvince").val(Data.Selected.LegalProvinceID).trigger('change');
    },
    SetBillingProvinceSelect: function () {
        return $("#slBillingProvince").val(Data.Selected.BillingProvinceID).trigger('change');
    },
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
};

var CustomerType = {
    All: 0,
    Personal: 1,
    Corporate: 2,
    Building: 3
}


var Validator = {
    ValidatePhoneNumber: function (phoneNumber) {
        var pattern = /^[\+]?[(]?[0-9]{2,6}[)]?[-\s\.]?[0-9]{4,8}[-\s\.]?[0-9]{4,6}$/im;
        return phoneNumber.match(pattern) != null;
    }
}

var Helper = {
    Calculate: function () {
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
        var TotalPrice = PricePerMonth //(PricePerMonth * multiply).toFixed(2);
        $("#tbArea").val(Helper.CommaSeparation3($("#tbArea").val()));
        $("#tbPricePerArea").val(Common.Format.CommaSeparation($("#tbPricePerArea").val()));
        $("#tbPricePerMonth").val(Common.Format.CommaSeparation(PricePerMonth));
        $("#tbTotalPrice").val(Common.Format.CommaSeparation(TotalPrice));
    },

    CommaSeparation3: function (yourNumber) {
        //Seperates the components of the number
        // Set the parameter to string, to enable replace functionality (replace won't work on float / int)
        var temp = yourNumber + "";
        var value = parseFloat(temp.replace(/,/g, ""));
        if (value != "" && !isNaN(value)) {
            var n = parseFloat(value).toFixed(3).toString().split(".");
            //Comma-fies the first part
            n[0] = n[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            //Combines the two sections
            return n.join(".");
        } else {
            return "0.000";
        }
    },
    SetBuildingFieldsRequired: function (required) {
        if (required) {
            $("#tbContractNo").attr("required", "required");
            $("#slTerm").attr("required", "required");
            $("#tbArea").attr("required", "required");
            $("#tbPricePerArea").attr("required", "required");
            $("#tbPricePerMonth").attr("required", "required");
            $("#tbTotalPrice").attr("required", "required");
        } else {
            $("#tbContractNo").removeAttr("required");
            $("#slTerm").removeAttr("required");
            $("#tbArea").removeAttr("required");
            $("#tbPricePerArea").removeAttr("required");
            $("#tbPricePerMonth").removeAttr("required");
            $("#tbTotalPrice").removeAttr("required");
        }
    }
}