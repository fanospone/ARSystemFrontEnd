Data = {};
var TempDataBapsBulky = [];
var TempDataBapsBulkyAdded = [];
var fsCustomerId = "";
var fsCompanyId = "";
var fsProductType = "";
var fsGroupBy = "";
var fsBapsType = "";
var fsBapsTypeId = "";
var fsSoNumb = "";
var fsAction = "";
var fsUserId = "";
var fsPassword = "";
var fsRemark = "";
var fsCustomerID = "";
var fsSoNumber = "";
var fsSiteID = "";
var fsSiteIDCust = "";
var fsSiteNameCust
var fsIDTransaction = "";
var fsApprIDCompany = "";
var fsApprIDCustomer1 = "";
var fsApprIDCustomer2 = "";
var fsApprIDCustomer3 = "";
var fsStipCategory = "";
var NextActivity = "";
var StartEffectiveDate = "";
var EndEffectiveDate = "";
var BaseLease = "";
var ServicePrice = "";
var CustomerInvoice = "";
var path = "";
DataSelected = {};
activitys = "";
currenttab = 0;
SplitData = [];

jQuery(document).ready(function () {
    
    path = $('#path').val();
    Form.Init();
    $("#btAddSite").unbind().click(function () {
        ModalInputBulky.Reset();
        ModalInputBulky.Init();
    });
    if ($('#TaskTodoType').val() == 'apr') {
        //Table.Init();
        //Table.GridBapsInput();
        $('.nav-tabs a[href="#tabBapsValidation"]').trigger('click');
        Control.TabActive(0);
        Table.GridBapsInput();
        $(".panelPicaInput").fadeOut(100);
        $(".panelDataValidationPrint").fadeOut(100);
        $(".panelDataValidationBulkyPrint").fadeOut(100);
        $(".panelBapsValidation").fadeIn(1500);
    } else {
        $('.nav-tabs a[href="#tabBapsDone"]').trigger('click');
        Control.TabActive(1);
        Table.GridBapsDone();
        $(".panelPicaInput").fadeOut(100);
        $(".panelDataValidationPrint").fadeOut(100);
        $(".panelDataValidationBulkyPrint").fadeOut(100);
        $(".panelBapsValidation").fadeIn(1500);
    }
    Table.Init($("#tblSite"))
    Table.Init($("#tblBapsDone"))
    
    //$('#pwr').hide();
    // ==================== Buttton Seacrh =============================//
    $(".btSearch").unbind().click(function () {
        if ($("#tabStartBaps").tabs('option', 'active') == 0) {
            Table.GridBapsInput();
        }
        else {
            Table.GridBapsDone();
        }

    });
    // ==================== End Buttton Seacrh =============================//
    $(".btReset").unbind().click(function () {
        if ($("#tabStartBaps").tabs('option', 'active') == 0) {
            $("#slISearchCustomerID").val("").trigger('change');
            $("#slISearchSiteID").val("").trigger('change');
            $("#slISearchSoNumber").val("").trigger('change');
            $("#slISearchCompanyID").val("").trigger('change');
            $("#slISearchProductID").val("").trigger('change');
            $("#slIBulkyNumber").val("").trigger('change');
            Table.GridBapsInput();
        }
        else {
            $("#slDSearchCustomerID").val("").trigger('change');
            $("#slDSearchSiteID").val("").trigger('change');
            $("#slDSearchSoNumber").val("").trigger('change');
            $("#slDSearchCompanyID").val("").trigger('change');
            $("#slDSearchProductID").val("").trigger('change');
            $("#slBulkyNumber").val("").trigger('change');
            Table.GridBapsDone();
        }

    });

    $("#btnSaveValidation").unbind().click(function () {
        var oprID = $("#OperatorIdHD").val();
        Form.SubmitValidation(oprID);
    });

    $("#btnCancelValidation").unbind().click(function () {
        $(".panelValidationForm").fadeOut();
        $(".panelTab").fadeIn(1000);
        $(".filter").fadeIn();
        $(".panelDataValidationPrint").fadeIn(1000);
    });

    $("#BapsType").change(function () {
        WhereClause = " sonumb = '" + DataSelected.SoNumber + "' and site_id_old = '" + DataSelected.SiteID + "' and stip_siro = " + DataSelected.SIRO + " and status_renewal = 1 and po_type = '" + $(this).find("option:selected").text() + "' "

        if ($(this).find("option:selected").text() == "POWER")
            $('#pwr').show();
        else
            $('#pwr').hide();


        $.ajax({
            url: "/api/MstDataSource/PONewBaps",
            type: "GET",
            data: { WhereClause: WhereClause }
        })
        .done(function (data, textStatus, jqXHR) {
            if (data.PONumber != null) {
                $('#PoNumberVal').val(data.PONumber);
                $('#PoDateVal').val(Common.Format.ConvertJSONDateTime(data.PoDate));
                var amount = Common.Format.CommaSeparation(data.PoAmount);
                $('#TotalPoAmountVal').val(Common.Format.CommaSeparation(data.PoAmount));
                $('#InitialPoAmountVal').val(Common.Format.CommaSeparation(data.PoAmount));
            }
            //console.log(data);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    });

    $('#tabStartBaps').unbind().on('tabsactivate', function (event, ui) {
        var newIndex = ui.newTab.index();
        currenttab = newIndex;
        if (newIndex == 0) {
            Control.TabActive(newIndex)
            //Table.GridBapsInput();
            $(".panelPicaInput").fadeOut(100);
            $(".panelDataValidationPrint").fadeOut(100);
            $(".panelDataValidationBulkyPrint").fadeOut(100);
            $(".panelBapsValidation").fadeIn(1500);

        }
        else if (newIndex == 1) {
            //Table.GridBapsDone();
            $(".panelPicaInput").fadeOut(100);
            $(".panelDataValidationPrint").fadeOut(100);
            $(".panelDataValidationBulkyPrint").fadeOut(100);
            $(".panelBapsValidation").fadeIn(1500);
        }
        else if (newIndex == 2) {
            //Table.GridBapsDone();
            $(".panelPicaInput").fadeOut(100);
            $(".panelDataValidationPrint").fadeOut(100);
            $(".panelDataValidationBulkyPrint").fadeOut(100);
            $(".panelBapsValidation").fadeIn(1500);
        }

    });

    // ==================== End Buttton Action =============================//
    $("#btnProcessData").unbind().click(function () {
        if (document.getElementById("SplitBill").checked) {
            var Row1 = {
                StartDate: $('#StartSplit').val(),
                EndDate: $('#EndSplit').val(),
                InvoiceAmount: $('#AmountSplit').val().replace(/,/g, ""),
                SONumber: DataSelected.SoNumber,
                StipSiro: DataSelected.SIRO,
                SplitRow: 1
            }

            var Row2 = {
                StartDate: $('#StartSplit2').val(),
                EndDate: $('#EndSplit2').val(),
                InvoiceAmount: $('#AmountSplit2').val().replace(/,/g, ""),
                SONumber: DataSelected.SoNumber,
                StipSiro: DataSelected.SIRO,
                SplitRow: 2
            }

            if (Row1.StartDate == "" || Row1.EndDate == "" || Row1.InvoiceAmount == "") {
                $('#mdlConfirmNextProcess').modal('hide');
                Common.Alert.Warning("Please Check Split Payment, Row 1 !");
                return;
            }

            if (Row2.StartDate == "" || Row2.EndDate == "" || Row2.InvoiceAmount == "") {
                $('#mdlConfirmNextProcess').modal('hide');
                Common.Alert.Warning("Please Check Split Payment, Row 2 !");
                return;
            }

        }

        if ($("#formUploadData").parsley().validate()) {
            Control.BindingNextStep();
            $('#mdlConfirmNextProcess').modal('show');
        }
    });
    
    $("#btnOK").unbind().click(function () {
        var l = Ladda.create(document.querySelector("#btnOK"))
        l.start();
        NextActivity = $("#slNextStep").val();
        if (NextActivity != null && NextActivity != "") {
            //Form.BapsValidationSubmit();
            Form.ApproveNextStep(); l.stop();
        } else {
            l.stop();
            $('#mdlConfirmNextProcess').modal('hide');
            Common.Alert.Warning("Please Select Next Activity");
        }
        
    });
    $("#btnSubmitBaps").unbind().click(function () {
        Form.SumitBapsBulky();
    });
    $("#btConfirm").unbind().click(function () {
        Form.RejectData();
    });

    $("#btnBapsValidationSave").unbind().click(function () {
        if ($('#BapsType').val() == null || $('#BapsType').val() == "") {
            Common.Alert.Warning("Please Select BAPS Type");
            return;
        }

        if ($("#formDetailData").parsley().validate()) {
            Form.BapsValidationSubmit();
        }
    });

    $("#btnBapsValidationCancel").unbind().click(function () {
        $("#partialBapsValidationForm").hide();
        Control.ButtonCancelForm();
    });

    $("#panelHeaderInfo *").attr("disabled", "disabled").off('click');
    $(".ProjectInfo *").attr("disabled", "disabled").off('click');
    $(".BapsInfo *").attr("disabled", "disabled").off('click');

    $('#postedFile1').on('change', function () {
        var input = document.getElementById('postedFile1');
        var infoArea = document.getElementById('file-upload-filename');
        // the change event gives us the input it occurred in 
        //var input = event.srcElement;

        // the input has an array of files in the `files` property, each one has a name that you can use. We're just using the name here.
        var fileName = input.files[0].name;

        // use fileName however fits your app best, i.e. add it into a div
        infoArea.textContent = 'File name: ' + fileName;
    });

    $('#btnBapsValidationSave').hide();
    $('#btnRejectData').hide();

    $("#btnRejectData").unbind().click(function () {
        $('#mdlConfirmation').modal('show');
    });

    $('#BapsDateRow').show();
    $('#BapsDateBlank').show();
    $('#BapsDate').removeAttr("disabled");
    $('#SplitBillRow').hide();

    $(".DocumentBaukList").hide();
    $('#btnViewDocumentBauk').unbind().click(function () {
        $(".DocumentBaukList").show();
    });
    
    $('#chkProrateFormulation').on('switchChange.bootstrapSwitch', function (event, state) {
        if (event.target.checked) {
            DataSelected.ProrateFormulation = 1;
        } else {
            DataSelected.ProrateFormulation = 0;
        }
        Control.BindProRateAmount()
    });

    $('#chkInstallationFee').on('switchChange.bootstrapSwitch', function (event, state) {
        if (event.target.checked) {
            DataSelected.InstallationFee = 1;
            $("#BapsDateBlank").hide();
            $("#divInstallationAmount").fadeIn();
        } else {
            DataSelected.InstallationFee = 0;
            $("#divInstallationAmount").hide();
            $("#BapsDateBlank").fadeIn();
            $("#tbxInstallationAmount").val("0");
        }
        Control.BindProRateAmount()
    

 
    });
    $("#btnClearBaps").unbind().on('click', function () {
        TempDataBapsBulkyAdded = [];
        Table.Init($("#tblSite"));
    });

});
var Control = {
    btnSearchBapsSiteDatePicker: function () {
        $(".datepicker").datepicker({
            format: "dd M yyyy"
        });
    },

    BindingSelectProductTypeByParam: function (ele) {
        $.ajax({
            url: "/api/MstDataSource/TenantType",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $(ele).html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(ele).append("<option value='" + $.trim(item.Value) + "'>" + item.Text + "</option>");
                    })
                }
                $(ele).select2({ placeholder: "Select Product", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingSelectStip: function (ele) {
        $.ajax({
            url: "/api/MstDataSource/TBGSys_Stip",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(ele).html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(ele).append("<option value='" + item.Value.trim() + "'>" + item.Text + "</option>");
                })
            }

            $(ele).select2({ placeholder: "Select STIP", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    
    BindingSelectSiro: function (ele) {
        $(ele).html("<option></option>")
        $(ele).append("<option value='0'>0</option>");
        $(ele).append("<option value='1'>1</option>");
        $(ele).append("<option value='2'>2</option>");
        $(ele).append("<option value='3'>3</option>");
        $(ele).append("<option value='4'>4</option>");
        $(ele).append("<option value='5'>5</option>");
        $(ele).select2({ placeholder: "Select Siro", width: null });
    }, 
    
    BindingSelectCompanyByParam: function (ele) {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                $(ele).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(ele).append("<option value='" + $.trim(item.CompanyId) + "'>" + item.Company + "</option>");
                    });
                }
                $(ele).select2({ placeholder: "Select Company Name", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingSelectOperatorByParam: function (ele) {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                $(ele).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(ele).append("<option value='" + $.trim(item.OperatorId) + "'>" + item.Operator + "</option>");
                    })
                }
                $(ele).select2({ placeholder: "Select Operator", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    
    BindingSelectCompany: function () {
        var idBapsInput = "#slISearchCompanyID";
        var idBapsDone = "#slDSearchCompanyID";
        var id2 = "#CompanyInvoice";
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $(idBapsInput).html("<option></option>")
            $(idBapsDone).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(idBapsInput).append("<option value='" + $.trim(item.CompanyId) + "'>" + item.Company + "</option>");
                    $(idBapsDone).append("<option value='" + $.trim(item.CompanyId) + "'>" + item.Company + "</option>");
                    $(id2).append("<option value='" + $.trim(item.CompanyId) + "'>" + item.Company + "</option>");
                })
            }
            $(idBapsInput).select2({ placeholder: "Select Company Name", width: null });
            $(idBapsDone).select2({ placeholder: "Select Company Name", width: null });
            $(id2).select2({ placeholder: "Select Company Name", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectOperator: function () {
        var idBapsInput = "#slISearchCustomerID";
        var idBapsDone = "#slDSearchCustomerID";
        var id2 = "#CustomerInvoice";
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $(idBapsInput).html("<option></option>")
            $(idBapsDone).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(idBapsInput).append("<option value='" + $.trim(item.OperatorId) + "'>" + item.Operator + "</option>");
                    $(idBapsDone).append("<option value='" + $.trim(item.OperatorId) + "'>" + item.Operator + "</option>");
                    $(id2).append("<option value='" + $.trim(item.OperatorId) + "'>" + item.Operator + "</option>");
                })
            }
            $(idBapsInput).select2({ placeholder: "Select Operator", width: null });
            $(idBapsDone).select2({ placeholder: "Select Operator", width: null });
            $(id2).select2({ placeholder: "Select Operator", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectProductType: function () {
        var idBapsInput = "#slISearchProductID";
        var idBapsDone = "#slDSearchProductID";
        $.ajax({
            url: "/api/MstDataSource/TenantType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(idBapsInput).html("<option></option>")
            $(idBapsDone).html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(idBapsInput).append("<option value='" + $.trim(item.Value) + "'>" + item.Text + "</option>");
                    $(idBapsDone).append("<option value='" + $.trim(item.Value) + "'>" + item.Text + "</option>");
                })
            }
            $(idBapsInput).select2({ placeholder: "Select Product", width: null });
            $(idBapsDone).select2({ placeholder: "Select Product", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectGroupBy: function () {
        $("#slGroupBy").html("<option></option>")
        $("#slGroupBy").append("<option value='NEW' selected>New</option>");
        $("#slGroupBy").append("<option value='REPEAT'>Repeat Order</option>");
        $("#slGroupBy").select2({ placeholder: "Select Group", width: null });
    },

    BindingSelectInvoiceType: function () {
        //$("#InvoiceType").html("<option></option>")
        //$("#InvoiceType").append("<option value='TRIWULAN'>TRIWULAN</option>");
        //$("#InvoiceType").append("<option value='MONTHLY'>MONTHLY</option>");
        //$("#InvoiceType").append("<option value='YEARLY'>YEARLY</option>");
        //$("#InvoiceType").select2({ placeholder: "Select Invoice Type", width: null });
        //$("#InvoiceType").html("<option></option>")
        //$("#InvoiceType").append("<option value='TRIWULAN'>TRIWULAN</option>");
        //$("#InvoiceType").append("<option value='MONTHLY'>MONTHLY</option>");
        //$("#InvoiceType").append("<option value='YEARLY'>YEARLY</option>");
        //$("#InvoiceType").select2({ placeholder: "Select Invoice Type", width: null });

        var id = "#InvoiceType";
        $.ajax({
            url: "/api/mstDataSource/InvoiceType",
            type: "GET",
            async: false
        })
       .done(function (data, textStatus, jqXHR) {
           $(id).html("<option></option>")
           if (Common.CheckError.List(data)) {
               $.each(data, function (i, item) {
                   $(id).append("<option value='" + parseInt($.trim(item.mstInvoiceTypeId)) + "'>" + item.Description + "</option>");
               })
           }
           $(id).select2({ placeholder: "Select Invoice Type", width: null });
       })
       .fail(function (jqXHR, textStatus, errorThrown) {
           Common.Alert.Error(errorThrown);
       });
    },

    BindingSelectCurrency: function (FieldId) {
        FieldId = FieldId;
        $(FieldId).html("<option></option>")
        $(FieldId).append("<option value='IDR'>IDR</option>");
        $(FieldId).append("<option value='USD'>USD</option>");
        $(FieldId).val('IDR').trigger('change');
        $(FieldId).select2({ placeholder: "Select", width: null });
    },

    BindingSelectRegional: function () {
        $.ajax({
            url: "/api/MstDataSource/Regional",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#OprCoresRegion").html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#OprCoresRegion").append("<option value='" + $.trim(item.RegionalId) + "'>" + item.Regional + "</option>");
                })
            }
            $("#OprCoresRegion").select2({ placeholder: "Select Regional", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    
    BindingSelectMonthYear: function(ele){
        $(ele).datepicker({
            format: "M yyyy",
            startView: "months",
            minViewMode: "months",
            clearBtn: true
        });
    },

    BindingSelectBapsType: function () {
        var id = "#BapsType";
        $.ajax({
            url: "/api/mstDataSource/BapsType",
            type: "GET",
            async: false
        })
       .done(function (data, textStatus, jqXHR) {
           $(id).html("<option></option>")
           if (Common.CheckError.List(data)) {
               $.each(data, function (i, item) {
                   $(id).append("<option value='" + $.trim(item.mstBapsTypeId) + "'>" + item.BapsType + "</option>");
               })
               //$(id).val(5).trigger('change');
           }
           $(id).select2({ placeholder: "Select Baps Type", width: null });
       })
       .fail(function (jqXHR, textStatus, errorThrown) {
           Common.Alert.Error(errorThrown);
       });
    },

    BindingSelectPowerType: function () {
        var id = "#PowerType";
        $.ajax({
            url: "/api/mstDataSource/PowerType",
            type: "GET",
            async: false
        })
       .done(function (data, textStatus, jqXHR) {
           $(id).html("<option></option>")
           if (Common.CheckError.List(data)) {
               $.each(data, function (i, item) {
                   $(id).append("<option value='" + parseInt($.trim(item.KodeType)) + "'>" + item.PowerType + "</option>");
               })
           }
           $(id).select2({ placeholder: "Select Power Type", width: null });
       })
       .fail(function (jqXHR, textStatus, errorThrown) {
           Common.Alert.Error(errorThrown);
       });
    },

    BindingDatePicker: function () {
        $(".datepicker").datepicker({
            format: "dd M yyyy"
        });
    },

    BindProRateAmount: function () {
        var CustomerID = CustomerInvoice;
        var InvoiceStartDate = StartEffectiveDate;
        var InvoiceEndDate = EndEffectiveDate;
        var InvoiceAmount = BaseLease;
        var ServiceAmount = ServicePrice;
        var DeductionAmount = "0";
        var InstallationAmount = $('#tbxInstallationAmount').val().replace(/,/g, "");
        var DropFODistance = $('#tbxBapsValidateDropFODistance').val().replace(/,/g, "");
        var StipCategorySelected = $('#StipCategory').text().trim();

        if (StipCategorySelected == "STIP 3") {
            var result = (parseFloat(InvoiceAmount) + parseFloat(ServiceAmount)).toString();
            $("#TotalAmount").val(Common.Format.CommaSeparation(result));
        }
        else if (StipCategorySelected == "STIP 10" && !DataSelected.ProrateFormulation) {
            var result = (parseFloat(InvoiceAmount) + parseFloat(ServiceAmount) + parseFloat(InstallationAmount)).toString();
            $("#TotalAmount").val(Common.Format.CommaSeparation(result));
        }
        else {
            var params = { CustomerID: CustomerID, StartInvoiceDate: InvoiceStartDate, EndInvoiceDate: InvoiceEndDate, InvoiceAmount: InvoiceAmount, ServiceAmount: ServiceAmount, DropFODistance: DropFODistance, ProductID: DataSelected.ProductID }

            $.ajax({
                url: "/api/ReconcileData/GetProRateAmount",
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false,
            })
            .done(function (data, textStatus, jqXHR) {
                var result = (parseFloat(data.data) - parseFloat(DeductionAmount)).toString();

                if (StipCategorySelected == "STIP 10" && DataSelected.ProrateFormulation) {
                    result = (parseFloat(data.data) - parseFloat(DeductionAmount) + parseFloat(InstallationAmount)).toString();
                }

                $("#TotalAmount").val(Common.Format.CommaSeparation(result));
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
        }
    },

    ValidateSplit: function (element, typeSplit) {
        if (document.getElementById("SplitBill").checked) {
            var InvoiceStartDate = element.value;
            var InvoiceEndDate = (typeSplit == 1) ? $('#EndSplit').val() : $('#EndSplit2').val();

            if (Date.parse(InvoiceStartDate) < Date.parse($('#StartEffectiveDateVal').val())) {
                Common.Alert.Warning("Invalid Range Date !");
                return;
            }

            if (typeSplit != 1) {
                if (Date.parse(InvoiceStartDate) <= Date.parse($('#EndSplit').val())) {
                    Common.Alert.Warning("Start Date Row 2 Must be Bigger than End Date Row 1 !");
                    return;
                }
            }

            if (Date.parse(InvoiceStartDate) > Date.parse($('#EndEffectiveDateVal').val())) {
                Common.Alert.Warning("Start Date Can`t Bigger Than End EffDate !");
                return;
            }

            if (InvoiceEndDate) {
                if (InvoiceEndDate != "" && Date.parse(InvoiceStartDate) > Date.parse(InvoiceEndDate)) {
                    Common.Alert.Warning("Start Date Can`t bigger than End Date !");
                    return;
                }

                if (InvoiceEndDate != "" && Date.parse(InvoiceStartDate) < Date.parse(InvoiceEndDate)) {
                    Control.BindSplitAmount(typeSplit);
                }
            }
        }
    },

    BindSplitAmount: function (typeSplit) {
        if (document.getElementById("SplitBill").checked) {

            if (typeSplit != 1) {
                if ($("#AmountSplit").val() == null || $("#AmountSplit").val() == "" || $("#AmountSplit").val() == "0" || $("#AmountSplit").val() == "0.00") {
                    Common.Alert.Warning("Please Complete First Row Data of Split !");
                    return;
                }
            }

            var CustomerID = CustomerInvoice;
            var InvoiceStartDate = (typeSplit == 1) ? $('#StartSplit').val() : $('#StartSplit2').val();
            var InvoiceEndDate = (typeSplit == 1) ? $('#EndSplit').val() : $('#EndSplit2').val();
            var StartEffectiveDate = $('#StartEffectiveDateVal').val();
            var EndEffectiveDate = $('#EndEffectiveDateVal').val();
            var InvoiceAmount = $('#BaseLeasePriceVal').val().replace(/,/g, "");
            var ServiceAmount = $('#ServicePriceVal').val().replace(/,/g, "");
            var DeductionAmount = "0";

            if (Date.parse(InvoiceStartDate) > Date.parse(InvoiceEndDate)) {
                Common.Alert.Warning("Start Date Can`t bigger than End Date !");
                return;
            }

            if (typeSplit != 1) {
                if (Date.parse(InvoiceStartDate) <= Date.parse($('#EndSplit').val())) {
                    Common.Alert.Warning("Start Date Row 2 Must be Bigger than End Date Row 1 !");
                    return;
                }

                if (Date.parse(InvoiceEndDate) <= Date.parse($('#EndSplit').val())) {
                    Common.Alert.Warning("End Date Row 2 Must be Bigger than End Date Row 1 !");
                    return;
                }
            }

            if (Date.parse(InvoiceStartDate) > EndEffectiveDate) {
                Common.Alert.Warning("Invalid Range Date !");
                return;
            }

            if (Date.parse(InvoiceStartDate) < StartEffectiveDate) {
                Common.Alert.Warning("Invalid Range Date !");
                return;
            }

            if (Date.parse(InvoiceEndDate) > EndEffectiveDate) {
                Common.Alert.Warning("Invalid Range Date !");
                return;
            }

            var params = { CustomerID: CustomerID, StartInvoiceDate: StartEffectiveDate, EndInvoiceDate: EndEffectiveDate, StartSplitDate: InvoiceStartDate, EndSplitDate: InvoiceEndDate, InvoiceAmount: InvoiceAmount, ServiceAmount: ServiceAmount }

            $.ajax({
                url: "/api/ReconcileData/GetProRateAmountSplit",
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false,
            })
            .done(function (data, textStatus, jqXHR) {
                var result = (parseFloat(data.data) - parseFloat(DeductionAmount)).toString();
                if (typeSplit == 1) {
                    $("#AmountSplit").val(Common.Format.CommaSeparation(result));
                }
                else {
                    $("#AmountSplit2").val(Common.Format.CommaSeparation(result));
                }

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
        }
        
    },

    AlertWarning: function (sa_message) {
        swal({
            title: "Warning",
            text: sa_message,
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, Continue!",
            closeOnConfirm: false
        }, function () {

            // remove these events;
            window.onkeydown = null;
            window.onfocus = null;
        });
    },

    DateDiff: function (year1, year2, idResult) {
        year1 = new Date(year1);
        year2 = new Date(year2);
        var millisBetween = year1.getFullYear() - year2.getFullYear();
        $("#" + idResult).val(millisBetween);
    },

    ButtonCancelForm: function () {
        //Control.BindingSelectBulkNumber();
        $("#panelHeaderInfo").hide();
        $(".panelValidationForm").hide();
        $(".FinalInfo").hide();
        $(".panelTab").fadeIn(2000);
        $(".filter").fadeIn(2000);
        $('#BapsType').val(null).trigger('change');
        $('#PowerType').val(null).trigger('change');
        $('#CompanyInvoice').val(null).trigger('change');
        $('#CustomerInvoice').val(null).trigger('change');
        $('#TotalAmount').val('');
        $('#RemarksVal').val('');
        $('#PoNumberVal').val('');
        $('#InitialPoAmountVal').val('');
        $('#TotalPoAmountVal').val('');
        $('#PoDateVal').val('');
        $('#MLAnumberVal').val('');
        $('#MLAdateVal').val('');
        $('#BaukNumberVal').val('');
        $('#BaukDateVal').val('');
        $('#StartLeasePeriodVal').val('');
        $('#EndLeasePeriodVal').val('');
        $('#EndEffectiveDateVal').val('');
        $('#BaseLeasePriceVal').val('');
        $('#ServicePriceVal').val('');
        $("#fileready").removeAttr("href");
        $('#BapsNo').val('');
        $('#file-upload-filename').text('');
        $('#btnProcessData').show();
        $('#BapsNo').attr('disabled', false);
        $('#postedFile1').attr('disabled', false);
        $('#SplitBillRow').hide();

        $('#StartSplit').val("");
        $('#EndSplit').val("");
        $('#AmountSplit').val("");
        $('#StartSplit2').val("");
        $('#EndSplit2').val("");
        $('#AmountSplit2').val("");
        $(".DocumentBaukList").hide();

        $("#chkProrateFormulation").bootstrapSwitch('disabled', false);
        $("#chkInstallationFee").bootstrapSwitch('disabled', false);
        $('#divProrateFormulation').hide();
        $('#divInstallationFee').hide();
        $('#divBapsValidateDropFODistance').hide();
        $("#BapsDateBlank").fadeIn();
        $("#chkProrateFormulation").bootstrapSwitch("state", false);
        $("#chkInstallationFee").bootstrapSwitch("state", false);
    },

    AlertDeleteRowTable: function (rowSelected) {
        swal({
            title: "Warning",
            text: "Are You Sure Delete This Row ?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, Continue!",
            closeOnConfirm: true
        }, function () {

            window.onkeydown = null;
            window.onfocus = null;
            $(rowSelected).closest('tr').remove();
        });
    },

    TabActive: function (index) {
        if (index == 2) {
            $(".fGroupBy").hide();
            $(".fProductType").hide();
            $(".fBapsType").hide();
            $(".fSoNumber").hide();
            $(".fsSiteID").hide();
            $(".fCustomer").hide();
            $(".fBuklyNumber").show();
        } else {
            $(".fBuklyNumber").hide();
            $(".fGroupBy").show();
            $(".fProductType").show();
            $(".fBapsType").show();
            $(".fSoNumber").show();
            $(".fsSiteID").show();
            $(".fCustomer").show();
        }
    },

    ApprValidate: function (id) {
        var value = $(id).val();
        if (value == "") {
            Common.Alert.Warning("Approval is missing !.");
            return false;
        } else {
            return true
        }
    },

    ReplaceDecimal: function (text) {
        var inputnumber = text.value.replace(',', '');
        text.value = Common.Format.FormatCurrency(inputnumber);
        Control.BindProRateAmount();
    },

    CommaSeparationOnly: function (text) {
        var inputnumber = text.value.replace(',', '');
        text.value = Common.Format.CommaSeparationOnly(inputnumber)
        Control.BindProRateAmount();
    },

    BindingNextStep: function () {

        var id = "#slNextStep";
        $.ajax({
            url: "/api/MstDataSource/NextActivity",
            type: "GET",
            data: { CurrentActivity: 19 }
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Next Step", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSIROSoNumber: function (SoNumber, DataS) {
        var id = "#StipSiro";
        $.ajax({
            url: "/api/MstDataSource/GetSiroSoNumber",
            type: "GET",
            data: { SoNumber: SoNumber }
        })
        .done(function (data, textStatus, jqXHR) {

            $(id).html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (DataS != null && DataS == item.Value.toString())
                        $(id).append("<option value='" + item.Value + "' selected>" + item.Text + "</option>");
                    else
                        $(id).append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                })
            }
            //$(id).select2({ placeholder: "Select SIRO", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    InvoiceTypeAddMonth: function () {
        tanggal = $('#StartLeasePeriodVal').val();
        DateStart = new Date(tanggal);
        var day = DateStart.getDate();
        var month = DateStart.getMonth();
        var year = DateStart.getFullYear();
        someDate = new Date();
        numberOfDaysToAdd = -1;

        Invoice = $('#InvoiceType').val();
        if (Invoice == 1) {
            month = month + 1;
            someDate = new Date(year, month, day);
            someDate.setDate(someDate.getDate() + numberOfDaysToAdd);
        }
        else if (Invoice == 2) {
            month = month + 3;
            someDate = new Date(year, month, day);
            someDate.setDate(someDate.getDate() + numberOfDaysToAdd);
        }
        else {
            month = month + 12;
            someDate = new Date(year, month, day);
            someDate.setDate(someDate.getDate() + numberOfDaysToAdd);
        }

        result = someDate.toDateString(); //(month + 1).toString() + "/" + day + "/" + year;
        $('#EndEffectiveDateVal').val(Common.Format.ConvertJSONDateTime(result));
        //DateStart = DateStart.setMonth(DateStart.getMonth() + 3);
        //$('#StartLeasePeriodVal').val(DateStart.toString("dd/MM/yyyy"));
    },

    BindingMaxSizeUpload: function () {

        var id = "#MaxSizeUpload";
        $.ajax({
            url: "/api/MstDataSource/GetSizeUpload",
            type: "GET",
            data: { Module: "BAPS_SUBMIT" }
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).val(data);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
}

var Form = {

    Init: function () {
        Control.BindingSelectCompany();
        Control.BindingSelectOperator();
        Control.BindingSelectProductType();
        Control.BindingSelectGroupBy();
        Control.BindingSelectBapsType();
        Control.BindingDatePicker();
        Control.BindingSelectPowerType();
        Control.BindingSelectInvoiceType();
        //untuk validasi checkbox
        Data.RowSelected = [];
        Data.RowFormValidation = [];
        Data.RowSelectedSite = [];
        $(".FinalInfo").hide();
        $(".panelBapsDone").hide();
        $(".panelValidationForm").hide();
        $("#panelHeaderInfo").hide();
        $('#tabStartBaps').tabs();
        /// Forms ///
        $("#partialBapsValidationForm").hide();
        Control.BindingMaxSizeUpload();
    },

    SetHeaderInfo: function (SiteID, SoNumber, CustomerID, CompanyID, CompanyName, CustomerSiteID, CustomerSiteName, BapsType) {
        $("#SiteIdTbg").text($.trim(SiteID));
        $("#SoNumber").text($.trim(SoNumber));
        $("#CustomerID").text($.trim(CustomerID));
        $("#CompanyID").text($.trim(CompanyID));
        $("#CompanyName").text($.trim(CompanyName));
        $("#CustomerSiteID").val($.trim(CustomerSiteID));
        //$("#BapsType").val($.trim(BapsType));
        $("#CustomerSiteName").val($.trim(CustomerSiteName));
    },

    BapsValidationForm: function (SoNumber, CustomerID, SIRO, CompanyID, MLANumber, MLADate, BaukNumber, BaukDate, POAmount, PoDate, CustomerInvoice) {

        Control.BindingSelectCurrency('#ServiceCryVal');
        Control.BindingSelectCurrency('#BaseLeaseCryVal');
        $(".panelTab").fadeOut();
        $(".filter").fadeOut();
        $(".FinalInfo").fadeIn(1800);
        $(".panelValidationForm").fadeIn(1800);
        $("#partialBapsValidationForm").fadeIn(1800);
        $("#panelHeaderInfo").fadeIn(2000);
        $(".partialBapsValidationForm").fadeIn(2000);
        // =========================================================================================//
        $("#SoNumberVal").val($.trim(SoNumber));
        $("#CustomerIDVal").val($.trim(CustomerID));
        $("#SiroVal").val($.trim(SIRO));
        $("#CompanyInvoiceIDVal").val($.trim(CompanyID));
        $("#BapsTypeIDVal").val($("#slBapsTypeID").val());
        $("#MLAnumberVal").val(MLANumber);
        $("#MLAdateVal").val(Common.Format.ConvertJSONDateTime(MLADate));
        $("#BaukNumberVal").val($.trim(BaukNumber));
        $("#BaukDateVal").val(Common.Format.ConvertJSONDateTime(BaukDate));
        $("#InitialPoAmountVal").val($.trim(POAmount));
        $("#PoDateVal").val(Common.Format.ConvertJSONDateTime(PoDate));
        $("#TotalPoAmountVal").val($.trim(POAmount));
        // =========================================================================================//

        // =========================================================================================//
        var params = { strSoNumber: $.trim(SoNumber), strCustomerId: CustomerInvoice, strStipSiro: $.trim(SIRO) };
        $.ajax({
            url: "/api/BAPSValidation/getBapsValidation",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (data != null) {
                if (Common.CheckError.Object(data)) {
                    if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                        $("#IDTrxBapsVal").val($.trim(data.ID));
                        $("#PoNumberVal").val($.trim(data.InitialPONumber));
                        $("#StartLeasePeriodVal").val(Common.Format.ConvertJSONDateTime(data.StartBapsDate));
                        $("#EndLeasePeriodVal").val(Common.Format.ConvertJSONDateTime(data.EndBapsDate));
                        $("#InvoiceTypeIDVal").val(data.CompanyInvoiceId);
                        $("#StartEffectiveDateVal").val(Common.Format.ConvertJSONDateTime(data.StartEffectiveDate));
                        $("#EndEffectiveDateVal").val(Common.Format.ConvertJSONDateTime(data.EndEffectiveDate));
                        $("#BaseLeasePriceVal").val($.trim(Common.Format.CommaSeparation(data.BaseLeasePrice)));
                        $("#ServicePriceVal").val($.trim(Common.Format.CommaSeparation(data.ServicePrice)));
                        $("#RemarksVal").val($.trim(data.Remark));

                        StartEffectiveDate = Common.Format.ConvertJSONDateTime(data.StartBapsDate);
                        EndEffectiveDate = Common.Format.ConvertJSONDateTime(data.EndEffectiveDate);
                        BaseLease = $.trim(Common.Format.CommaSeparation(data.BaseLeasePrice));
                        ServicePrice = $.trim(Common.Format.CommaSeparation(data.ServicePrice));

                        Control.BindProRateAmount();
                    } else {
                        Common.Alert.Warning(data.ErrorMessage);
                    }
                }
            } else {
                $("#IDTrxBapsVal").val('');
                $("#StartLeasePeriodVal").val('');
                $("#EndLeasePeriodVal").val('');
                $("#InvoiceTypeIDVal").val('');
                $("#StartEffectiveDateVal").val('');
                $("#EndEffectiveDateVal").val('');
                $("#BaseLeasePriceVal").val('');
                $("#ServicePriceVal").val('');
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
    },

    BapsValidationSubmit: function () {
        var data = new Object();
        data.ID = $.trim($("#IDTrxBapsVal").val());
        data.SoNumber = $.trim($("#SoNumberVal").val());
        data.InitialPONumber = $.trim($("#PoNumberVal").val());
        data.InitialPODate = $.trim($("#PoDateVal").val());
        data.StipSiro = $.trim($("#SiroVal").val());
        data.mstBapsTypeID = $.trim($("#BapsType").val());
        data.PowerTypeID = $.trim($("#PowerType").val());
        data.mstCustomerInvoiceID = $.trim($("#InvoiceTypeIDVal").val());
        data.CompanyInvoiceId = $.trim($("#CompanyInvoiceIDVal").val());
        data.CustomerId = $.trim($("#CustomerIDVal").val());
        data.StartEffectiveDate = $.trim($("#StartEffectiveDateVal").val());
        data.EndEffectiveDate = $.trim($("#EndEffectiveDateVal").val());
        data.BaseLeasePrice = $.trim($("#BaseLeasePriceVal").val());
        data.ServicePrice = $.trim($("#ServicePriceVal").val());
        data.BaseLeaseCurrency = $.trim($("#BaseLeaseCryVal").val());
        data.ServiceCurrency = $.trim($("#ServiceCryVal").val());
        data.StartBapsDate = $.trim($("#StartLeasePeriodVal").val());
        data.EndBapsDate = $.trim($("#EndLeasePeriodVal").val());
        data.Remark = $.trim($("#RemarksVal").val());

        data.ProrateFormulation = DataSelected.ProrateFormulation;
        data.InstallationAmount = parseFloat($('#tbxInstallationAmount').val().replace(/,/g, ""));

        var params = {
            bapsValidation: data
        };
        var l = Ladda.create(document.querySelector("#btnBapsValidationSave"));
        $.ajax({
            url: "/api/BAPSValidation/submitBapsValidation",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    Common.Alert.Success("Data Success Saved!");
                    Control.ButtonCancelForm();
                    Table.GridBapsInput();
                } else {
                    Common.Alert.Warning(data.ErrorMessage);
                }
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        });

    },

    ClearRowSelected: function () {
        Data.RowSelected = [];
        Data.RowSelectedSite = [];
    },

    GetForm: function (SoNumber, CustomerID, SiteID, CompanyID, CompanyName, CustomerSiteID, BapsType, CustomerSiteName, ProductID, StipCategory, StipSiro) {
        var params = { strSoNumber: SoNumber, strSiteID: SiteID, strCustomerId: CustomerID, strProductId: ProductID, strStipCategory: StipCategory, strStipSiro: StipSiro };
        $.ajax({
            url: "/api/BAPSValidation/getFormValidationPrint",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (data != '') {
                Form.SetHeaderInfo(SiteID, SoNumber, CustomerID, CompanyID, CompanyName, CustomerSiteID, CustomerSiteName, BapsType)
                $(".panelTab").fadeOut();
                $(".filter").fadeOut();
                $(".panelValidationForm").fadeIn(1800);
                $("#panelHeaderInfo").fadeIn(2000);
                jQuery.globalEval(data);
            } else {
                Common.Alert.Warning("Form Not Found !");
            }
        });

    },

    ApproveNextStep: function () {
        var MaxSizeUpload = $('#MaxSizeUpload').val();

        var formData = new FormData();
        formData.append("mstBapsID", DataSelected.ID);
        formData.append("SoNumber", DataSelected.SoNumber);
        formData.append("SIRO", DataSelected.SIRO);
        formData.append("BapsType", DataSelected.PoType);

        if (DataSelected.PowerTypeID)
            formData.append("PowerType", DataSelected.PowerTypeID);
        else
            formData.append("PowerType", "");

        formData.append("BAPSNumber", $('#BapsNo').val());
        formData.append("mstRAActivityID", $('#slNextStep').val()); 
        formData.append("trxBapsRejectId", DataSelected.trxBapsRejectId);
        formData.append("BAPSDate", $('#BapsDate').val());

        if (document.getElementById("SplitBill").checked) {
            var Row1 = {
                StartDate: $('#StartSplit').val(),
                EndDate: $('#EndSplit').val(),
                InvoiceAmount: $('#AmountSplit').val().replace(/,/g, ""),
                SONumber: DataSelected.SoNumber,
                StipSiro: DataSelected.SIRO,
                SplitRow: 1
            }

            var Row2 = {
                StartDate: $('#StartSplit2').val(),
                EndDate: $('#EndSplit2').val(),
                InvoiceAmount: $('#AmountSplit2').val().replace(/,/g, ""),
                SONumber: DataSelected.SoNumber,
                StipSiro: DataSelected.SIRO,
                SplitRow: 2
            }

            formData.append("SplitStatus", "1");
            var SplitData = [];
            SplitData.push(Row1);
            SplitData.push(Row2);
            formData.append("SplitData", JSON.stringify(SplitData));
        }
        formData.append("SplitStatus", "");
        formData.append("SplitData", "");

        var fileInput = document.getElementById("postedFile1");
        if (DataSelected.trxBapsRejectId > 0 && fileInput.files.length == 0) {
            formData.append("FileName", "");
            formData.append('File', null);
        }
        else {
            if (document.getElementById("postedFile1").files.length != 0) {

                fsFileName = fileInput.files[0].name;
                formData.append("FileName", fsFileName);

                fsFile = fileInput.files[0];

                fsExtension = fsFileName.split('.').pop().toUpperCase();

                if ((fsFile.size / 1024) > MaxSizeUpload) {
                    Common.Alert.Warning("Upload File Can`t bigger then " + MaxSizeUpload.toString() + " bytes (" + (MaxSizeUpload/1024).toString() + "mb)."); return;
                }

                if (fsExtension != "XLS" && fsExtension != "XLSX" && fsExtension != "PDF") {
                    $('#mdlConfirmNextProcess').modal('hide');
                    Common.Alert.Warning("Please upload an Excel or PDF File."); return;
                }
                else {

                    formData.append('File', fsFile);
                }

                errors = false;
            }
            else {
                $('#mdlConfirmNextProcess').modal('hide');
                Common.Alert.Warning("Please Select Document."); return;
            }
        }
        

        $.ajax({
            url: '/api/NewBapsInput/Upload',
            type: 'POST',
            data: formData,
            async: false,
            cache: false,
            contentType: false,
            processData: false
        })
        .done(function (data, textStatus, jqXHR) {
            if ((data != 0 && data != null)) {
                $('#mdlConfirmNextProcess').modal('hide');
                Common.Alert.Success("Success To Process Data");
                Control.ButtonCancelForm();
                Table.GridBapsInput();
            } else {
                $('#mdlConfirmNextProcess').modal('hide');
                Common.Alert.Warning("Fail To Process Data ! Please Contact Helpdesk");
            }

        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            $('#mdlConfirmNextProcess').modal('hide');
        });
    },

    RejectData: function () {
        var params = {
            ID: DataSelected.ID,
            SoNumber: DataSelected.SoNumber,
            StipSiro: DataSelected.SIRO,
            mstBapsTypeID: DataSelected.PoType
        };

        $.ajax({
            url: '/api/NewBapsInput/Reject',
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
        })
        .done(function (data, textStatus, jqXHR) {
            if ((data != 0 && data != null)) {
                $('#mdlConfirmation').modal('hide');
                Common.Alert.Success("Success To Reject Data");
                Control.ButtonCancelForm();
                Table.GridBapsInput();
            } else {
                Common.Alert.Warning("Fail To Process Data ! Please Contact Helpdesk");
            }

        }).fail(function (jqXHR, textStatus, errorThrown) {
            $('#mdlConfirmation').modal('hide');
            Common.Alert.Error(errorThrown)
        });
    },

    EffDate: function (dt) {
        $('#StartEffectiveDateVal').val(Common.Format.ConvertJSONDateTime(dt.value));
    },

    SplitBill: function () {
        if (document.getElementById("SplitBill").checked) {
            $('#SplitBillRow').show();
        }
        else {
            $('#SplitBillRow').hide();

            $('#StartSplit').val("");
            $('#EndSplit').val("");
            $('#AmountSplit').val("");
            $('#StartSplit2').val("");
            $('#EndSplit2').val("");
            $('#AmountSplit2').val("");
        }
    },

    SumitBapsBulky: function () {
        App.blockUI({ target: "#pnlSummary", boxed: true });
        var l = Ladda.create(document.querySelector("#btnSubmitBaps"))
        l.start();

        //formSearchBapsUpload
        var formBapsValid = $("#formSearchBapsUpload").parsley().validate();
        var formData = new FormData();
        var isValid = "";
        if ($('#BapsNumberBulky').val() <= 0) {
            isValid += "Please input BAPS Number.\n";
        }
        if ($('#BapsDateBulky').val() <= 0) {
            isValid += "Please select BAPS Date.\n";
        }
        if (TempDataBapsBulkyAdded.length <= 0) {
            isValid += "Please upload BAPS Document.\n";
        }
        if (isValid != "" || formBapsValid == false) {
            Common.Alert.Warning("Cannot Submit Baps\n" + isValid);
            App.unblockUI("#pnlSummary");
            l.stop();
            return false;
        }
        var MaxSizeUpload = $('#MaxSizeUpload').val();
        formData.append("BAPSNumberBulky", $('#BapsNumberBulky').val());
        formData.append("BAPSDateBulky", $('#BapsDateBulky').val());
        formData.append("vmBapsData", JSON.stringify(TempDataBapsBulkyAdded));
        var fileInput = document.getElementById("postFileInputBulky");
        if (fileInput.files.length != 0) {
            fsFileName = fileInput.files[0].name;
            formData.append("FileName", fsFileName);

            fsFile = fileInput.files[0];

            fsExtension = fsFileName.split('.').pop().toUpperCase();

            if ((fsFile.size / 1024) > MaxSizeUpload) {
                Common.Alert.Warning("Upload File Can`t bigger then " + MaxSizeUpload.toString() + " bytes (" + (MaxSizeUpload / 1024).toString() + "mb).");
                App.unblockUI("#pnlSummary");
                l.stop();
                return;
            }

            if (fsExtension != "XLS" && fsExtension != "XLSX" && fsExtension != "PDF") {
                //$('#mdlConfirmNextProcess').modal('hide');
                Common.Alert.Warning("Please upload an Excel or PDF File.");
                App.unblockUI("#pnlSummary");
                l.stop();
                return;
            }
            else {
                formData.append('File', fsFile);
            }

            errors = false;
        }
        else {
            //$('#mdlConfirmNextProcess').modal('hide');
            Common.Alert.Warning("Please Select Document.");
            App.unblockUI("#pnlSummary");
            l.stop();
            return;
        }
        
        //submit-----
        $.ajax({
            url: '/api/NewBapsInput/UploadBapsBulky',
            type: 'POST',
            data: formData,
            async: false,
            cache: false,
            contentType: false,
            processData: false
        })
        .done(function (data, textStatus, jqXHR) {
            if ((data != 0 && data != null)) {
                $('#mdlConfirmNextProcess').modal('hide');
                Common.Alert.Success("Success To Process Data");
                TempDataBapsBulkyAdded = [];
                TempDataBapsBulky = [];
                $('#BapsNumberBulky').val('').trigger('change');
                $('#BapsDateBulky').val('').trigger('change');
                $('#postFileInputBulky').val('').trigger('change');

                Table.Init($("#tblSite"));
            } else {
                Common.Alert.Warning("Fail To Process Data ! Please Contact Helpdesk");
            }
            App.unblockUI("#pnlSummary");
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            App.unblockUI("#pnlSummary");
            l.stop();
        });
    }
}

var Table = {

    Init: function (id) {
        //$(".panelSearchZero").hide();
        //$(".panelSearchResult").hide();
         $(id).dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });


        $(window).resize(function () {
            $(id).DataTable().columns.adjust().draw();
        });
    },


    GridBapsInput: function () {
        var l = Ladda.create(document.querySelector("#btSearchBapsInput"))
        l.start();

        fsCustomerId = $("#slISearchCustomerID").val() == null ? "" : $("#slISearchCustomerID").val();
        fsCompanyId = $("#slISearchCompanyID").val() == null ? "" : $("#slISearchCompanyID").val();
        fsProductID = $("#slISearchProductID").val() == null ? "" : $("#slISearchProductID").val();
        fsSiteID = $("#slISearchSiteID").val() == null ? "" : $("#slISearchSiteID").val();
        fsSoNumb = $("#slISearchSoNumber").val() == null ? "" : $("#slISearchSoNumber").val();

        var params = {
            strCustomerId: fsCustomerId,
            strCompanyId: fsCompanyId,
            strProductId: fsProductID,
            strSoNumber: fsSoNumb,
            strSiteID: fsSiteID,
            mstRAActivityID: '19'
        };
        Table.Init('#tblBapsInput');
        var tblProcess = $("#tblBapsInput").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/NewBapsInput/getSonumbList",
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
            "lengthMenu": [[25, 50, 100, 200], ['25', '50', '100', '200']],
            "destroy": true,
            "columns": [
                { data: "RowIndex" },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<i class='fa fa-edit btn btn-xs green link-TrxBaspValidation'></i>"
                        return strReturn;
                    }
                },
                { data: "SoNumber" },
                { data: "TowerTypeID" },
                { data: "CustomerID" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CompanyID" },
                { data: "CompanyName" },
                { data: "SIRO" },
                { data: "STIPNumber" },
                { data: "StipCode" },
                { data: "Product" },
                 {
                     data: "MLADate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                 { data: "MLANumber" },
                 { data: "BaukNumber" },
                 {
                     data: "BaukDate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                 { data: "PoAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                 {
                     data: "PoDate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblProcess.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        $("#Row" + item).addClass("active");
                    }
                }
            },
            "order": [],
            "scrollY": 300, /* Enable vertical scroll to allow fixed columns */
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
        });
        $("#tblBapsInput tbody").unbind();
        $("#tblBapsInput tbody").on("click", ".link-TrxBaspValidation", function (e) {
            $('#SplitBill').prop('checked', false);
            $('#formDetailData').parsley().reset();
            $('#formUploadData').parsley().reset();
            var table = $("#tblBapsInput").DataTable();
            var row = table.row($(this).parents('tr')).data();
            DataSelected = row;
            CustomerInvoice = row.CustomerInvoice;
            Form.SetHeaderInfo(row.SiteID, row.SoNumber, row.CustomerID, row.CompanyID, row.CompanyName, row.CustomerSiteID, row.CustomerSiteName, fsBapsType)
            var customerInv = row.CustomerInvoice == null ? row.CustomerInvoice : row.CustomerInvoice.trim();
            
            $('#CustomerInvoice').val(customerInv).trigger('change');
            $('#SiteName').text(row.SiteName);
            $('#BapsType').val(row.PoType).trigger('change');
            $('#PowerType').val(row.PowerTypeID).trigger('change');
            $('#InvoiceType').val(row.InvoiceType).trigger('change');
            $('#BapsDate').val(Common.Format.ConvertJSONDateTime(row.BapsDate));
            $('#BapsDate').removeAttr("disabled");
            Form.BapsValidationForm(row.SoNumber, row.CustomerID, row.SIRO, row.CompanyID, row.MLANumber, row.MLADate, row.BaukNumber, row.BaukDate, row.PoAmount, row.PoDate, customerInv);
            if (row.FilePath != null && row.FilePath != "") {
                $("#fileready").show();
                $("#fileready").attr("href", path + row.FilePath);
                $('#BapsNo').val(row.BAPSNumber);
                var FileNames = row.FilePath.split('\\');
                var Names = FileNames[FileNames.length - 1];
                $('#file-upload-filename').text(Names);
                $('#postedFile1').removeAttr('required');
            }
            else {
                $("#fileready").hide();
            }

            if (row.ActivityID > 19) {
                $('#btnProcessData').hide();
                $('#BapsNo').attr('disabled', 'disabled');
                $('#postedFile1').attr('disabled', 'disabled');
            }
            else {
                $('#btnProcessData').show();
            }

            Control.BindingSIROSoNumber("SoNumber = '" + row.SoNumber + "'", row.SIRO);
            $('#StipCategory').text(row.StipCode);
            
            $('#CompanyInvoice').val(row.CompanyInvoice.trim()).trigger('change');
            $('#SplitBill').removeAttr("disabled");
            if (row.SplitStartDate) {
                document.getElementById("SplitBill").checked = true;
                $('#SplitBillRow').show();
                $('#SplitBillRow').removeAttr("disabled");
                $('#StartSplit').val(Common.Format.ConvertJSONDateTime(row.SplitStartDate));
                $('#EndSplit').val(Common.Format.ConvertJSONDateTime(row.SplitEndDate));
                $('#AmountSplit').val(Common.Format.CommaSeparation(row.SplitAmount));
                $('#StartSplit2').val(Common.Format.ConvertJSONDateTime(row.SplitStartDate2));
                $('#EndSplit2').val(Common.Format.ConvertJSONDateTime(row.SplitEndDate2));
                $('#AmountSplit2').val(Common.Format.CommaSeparation(row.SplitAmount2));
            }
            $(".DocumentBaukList").hide();

            $("#tbxInstallationAmount").val("0");
            
            if (row.StipID == 10) {
                $('#divProrateFormulation').show();
                $('#divInstallationFee').show();

                if (row.ProrateFormulation == 1) {
                    $("#chkProrateFormulation").bootstrapSwitch("state", true);
                } else {
                    $("#chkProrateFormulation").bootstrapSwitch("state", false);
                }

                if (row.InstallationAmount > 0) {
                    $('#tbxInstallationAmount').val(Common.Format.CommaSeparation(row.InstallationAmount))
                    $("#chkInstallationFee").bootstrapSwitch("state", true);
                } else {
                    $("#chkInstallationFee").bootstrapSwitch("state", false);
                }
            } else if (row.StipID == 1 && row.ProductID == 45) {
                $('#tbxBapsValidateDropFODistance').val(Common.Format.CommaSeparationOnly(row.DropFODistance))
                $('#divBapsValidateDropFODistance').show();
                $("#BapsDateBlank").hide();
            }

            $("#chkProrateFormulation").bootstrapSwitch('disabled', true);
            $("#chkInstallationFee").bootstrapSwitch('disabled', true);

            Table.GridDocumentSupport(row.CompanyID, row.SiteID);
            Table.GridChecdocList(row.SoNumber, row.SiteID, row.CustomerID);
        });
    },

    GridBapsDone: function () {
        var l = Ladda.create(document.querySelector("#btSearchBapsInput"))
        l.start();

        fsCustomerId = $("#slDSearchCustomerID").val() == null ? "" : $("#slDSearchCustomerID").val();
        fsCompanyId = $("#slDSearchCompanyID").val() == null ? "" : $("#slDSearchCompanyID").val();
        fsProductID = $("#slDSearchProductID").val() == null ? "" : $("#slDSearchProductID").val();
        fsSiteID = $("#slDSearchSiteID").val() == null ? "" : $("#slDSearchSiteID").val();
        fsSoNumb = $("#slDSearchSoNumber").val() == null ? "" : $("#slDSearchSoNumber").val();

        var params = {
            strCustomerId: fsCustomerId,
            strCompanyId: fsCompanyId,
            strProductId: fsProductID,
            strSoNumber: fsSoNumb,
            strSiteID: fsSiteID,
            mstRAActivityID: '20'
        };
        Table.Init('#tblBapsDone');
        var tblProcess = $("#tblBapsDone").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/NewBapsInput/getBapsDone",
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
            "lengthMenu": [[25, 50, 100, 200], ['25', '50', '100', '200']],
            "destroy": true,
            "columns": [
                { data: "RowIndex" },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<i class='fa fa-edit btn btn-xs green link-TrxBapsDone'></i>"
                        return strReturn;
                       
                    }
                },
                { data: "SoNumber" },
                { data: "TowerTypeID" },
                { data: "CustomerID" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CompanyID" },
                { data: "CompanyName" },
                { data: "SIRO" },
                { data: "STIPNumber" },
                { data: "StipCode" },
                { data: "Product" },
                 {
                     data: "MLADate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                 { data: "MLANumber" },
                 { data: "BaukNumber" },
                 {
                     data: "BaukDate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },
                 { data: "PoAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                 {
                     data: "PoDate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
                     }
                 },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblProcess.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        $("#Row" + item).addClass("active");
                    }
                }
            },
            "order": [],
            "scrollY": 300, /* Enable vertical scroll to allow fixed columns */
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
        });
        $("#tblBapsDone tbody").unbind();
        $("#tblBapsDone tbody").on("click", ".link-TrxBapsDone", function (e) {
            $('#formDetailData').parsley().reset();
            $('#formUploadData').parsley().reset();
            var table = $("#tblBapsDone").DataTable();
            var row = table.row($(this).parents('tr')).data();
            DataSelected = row;
            CustomerInvoice = row.CustomerInvoice;
            Form.SetHeaderInfo(row.SiteID, row.SoNumber, row.CustomerID, row.CompanyID, row.CompanyName, row.CustomerSiteID, row.CustomerSiteName, fsBapsType)

            $('#SiteName').text(row.SiteName);
            $('#BapsType').val(row.PoType).trigger('change');
            $('#PowerType').val(row.PowerTypeID).trigger('change');

            $('#CustomerInvoice').val(row.CustomerInvoice).trigger('change');
            $('#CompanyInvoice').val(row.CompanyInvoice).trigger('change');
            Form.BapsValidationForm(row.SoNumber, row.CustomerID, row.SIRO, row.CompanyID, row.MLANumber, row.MLADate, row.BaukNumber, row.BaukDate, row.PoAmount, row.PoDate, row.CustomerInvoice.trim());

            //return "<a href='" +  + "' target='_blank'> Download </a>";
            $('#InvoiceType').val(row.InvoiceType).trigger('change');
            $('#BapsDate').val(Common.Format.ConvertJSONDateTime(row.BapsDate));
            $('#BapsDate').attr('disabled', 'disabled');

            if (row.FilePath != null && row.FilePath != "") {
                $("#fileready").show();
                $("#fileready").attr("href", path + row.FilePath);
                $('#BapsNo').val(row.BAPSNumber);
                var FileNames = row.FilePath.split('\\');
                var Names = FileNames[FileNames.length - 1];
                $('#file-upload-filename').text(Names);
            }
            else {
                $("#fileready").hide();
            }

            $('#btnProcessData').hide();
            $('#BapsNo').attr('disabled', 'disabled');
            $('#postedFile1').attr('disabled', 'disabled');
            
            Control.BindingSIROSoNumber("SoNumber = '" + row.SoNumber + "'", row.SIRO);
            $('#StipCategory').text(row.StipCode);

            if (row.SplitStartDate) {
                document.getElementById("SplitBill").checked = true;
                $('#SplitBillRow').show();
                $('#SplitBillRow *').attr('disabled', 'disabled').off('click');
                $('#SplitBill').attr('disabled', 'disabled').off('click');

                $('#StartSplit').val(Common.Format.ConvertJSONDateTime(row.SplitStartDate));
                $('#EndSplit').val(Common.Format.ConvertJSONDateTime(row.SplitEndDate));
                $('#AmountSplit').val(Common.Format.CommaSeparation(row.SplitAmount));
                $('#StartSplit2').val(Common.Format.ConvertJSONDateTime(row.SplitStartDate2));
                $('#EndSplit2').val(Common.Format.ConvertJSONDateTime(row.SplitEndDate2));
                $('#AmountSplit2').val(Common.Format.CommaSeparation(row.SplitAmount2));
            }

            $(".DocumentBaukList").hide();

            $("#tbxInstallationAmount").val("0");

            if (row.StipID == 10) {
                $('#divProrateFormulation').show();
                $('#divInstallationFee').show();

                if (row.ProrateFormulation == 1) {
                    $("#chkProrateFormulation").bootstrapSwitch("state", true);
                } else {
                    $("#chkProrateFormulation").bootstrapSwitch("state", false);
                }

                if (row.InstallationAmount > 0) {
                    $('#tbxInstallationAmount').val(Common.Format.CommaSeparation(row.InstallationAmount))
                    $("#chkInstallationFee").bootstrapSwitch("state", true);
                } else {
                    $("#chkInstallationFee").bootstrapSwitch("state", false);
                }
            } else if (row.StipID == 1 && row.ProductID == 45) {
                $('#tbxBapsValidateDropFODistance').val(Common.Format.CommaSeparationOnly(row.DropFODistance))
                $('#divBapsValidateDropFODistance').show();
                $("#BapsDateBlank").hide();
            }

            $("#chkProrateFormulation").bootstrapSwitch('disabled', true);
            $("#chkInstallationFee").bootstrapSwitch('disabled', true);

            Table.GridDocumentSupport(row.CompanyID, row.SiteID);
            Table.GridChecdocList(row.SoNumber, row.SiteID, row.CustomerID);
        });
    },
    Export: function () {
        fsCustomerId = $("#slDSearchCustomerID").val() == null ? "" : $("#slDSearchCustomerID").val();
        fsCompanyId = $("#slDSearchCompanyID").val() == null ? "" : $("#slDSearchCompanyID").val();
        fsProductID = $("#slDSearchProductID").val() == null ? "" : $("#slDSearchProductID").val();
        fsSiteID = $("#slDSearchSiteID").val() == null ? "" : $("#slDSearchSiteID").val();
        fsSoNumb = $("#slDSearchSoNumber").val() == null ? "" : $("#slDSearchSoNumber").val();
        mstRAActivityID = 0;//$('#slSearchStatus').val();

        if (currenttab == 0)
            mstRAActivityID = 19;
        else
            mstRAActivityID = 20;

        window.location.href = "/RevenueAssurance/NewBaps/Export?strCustomerID=" + fsCustomerId + "&strCompanyId=" + fsCompanyId + "&strProductId=" + fsProductID
            + "&strSoNumber=" + fsSoNumb + "&strSiteID=" + fsSiteID + "&strTenantType=" + "" + "&mstRAActivityID=" + mstRAActivityID;
    },
    GridDocumentSupport: function (CompanyID, SiteID) {
        var params = { companyId: CompanyID, siteId: SiteID };
        var idTbl = "#AddDocumentSupport";
        Table.Init(idTbl);
        $(idTbl).DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "paging": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/CheckingDoc/getDocumentSupport",
                "type": "GET",
                "data": params,
            },
            "filter": false,
            "destroy": true,
            "columns": [
                { data: "No" },
                {
                    data: "DocumentName",
                    mRender: function (data) {
                        return "<a href='#' class='btn-Download'>" + data + "</a>"
                    }
                },
            ],
            "columnDefs": [[{ "targets": [0], "className": "text-center" }]],

        });
        $(idTbl + " tbody").unbind();
        $(idTbl + " tbody").on("click", ".btn-Download", function (e) {
            var table = $(idTbl).DataTable();
            var row = table.row($(this).parents('tr')).data();
            Helper.DownloadDocSupport(row.DocumentName, CompanyID, SiteID);
        });
    },

    GridChecdocList: function (SoNumber, SiteID, CustomerID) {
        var params = {
            strSoNumber: SoNumber,
            strSiteId: SiteID,
            strCustomerId: CustomerID
        };
        var idTbl = "#DocumentList";
        Table.Init(idTbl);
        $(idTbl).DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "paging": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/CheckingDoc/getCheckDocList",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            "filter": false,
            "destroy": true,
            "columns": [
                { data: "RowIndex" }
                , {
                    data: "DocName",
                    mRender: function (data, type, full) {
                        return "<a href='#' class='btn-Download' id='btDownload_" + full.RowIndex + "'>" + full.DocName + "</a>"
                    }
                }
            ],
            "columnDefs": [[{ "targets": [0], "className": "text-center" }]],

        });
        $(idTbl + " tbody").unbind();
        $(idTbl + " tbody").on("click", ".btn-Download", function (e) {
            var table = $(idTbl).DataTable();
            var row = table.row($(this).parents('tr')).data();
            Helper.DownloadBAUKDocument(row.FileName, row.LinkFile, row.IsLegacy);
        });
    },
}

var Helper = {
    ParseDateFormat: function (str) {
        var arr = str.split('-');
        var day = arr[0].toString();
        var month = "01";
        switch (arr[1].toString()) {
            case "Jan":
                month = "01";
                break;
            case "Feb":
                month = "02";
                break;
            case "Mar":
                month = "03";
                break;
            case "Apr":
                month = "04";
                break;
            case "May":
                month = "05";
                break
            case "Jun":
                month = "06";
                break;
            case "Jul":
                month = "07";
                break;
            case "Aug":
                month = "08";
                break;
            case "Sep":
                month = "09";
                break;
            case "Oct":
                month = "10";
                break;
            case "Nov":
                month = "11";
                break;
            case "Dec":
                month = "12";
                break;
        }
        var year = arr[2].toString();

        return (year + '-' + month + '-' + day);
    },
    DownloadBAUKDocument: function (fileName, path, IsLegacy) {
        var contentType = "application/pdf";
        window.location.href = "/RevenueAssurance/DownloadFileProject?FilePath=" + path + "&FileName=" + fileName + "&ContentType=" + contentType + "&IsLegacy=" + IsLegacy;
    },
    DownloadDocSupport: function (fileName, CompanyId, SiteId) {
        window.location.href = "/RevenueAssurance/DownloadDocumentSuppoert?fileName=" + fileName + "&companyId=" + CompanyId + "&siteId=" + SiteId;

    },
}

var ModalInputBulky = {
    Init: function () { //Modal.Init
        TempDataBapsBulky = (TempDataBapsBulkyAdded || []).concat();

        var tblSearchBaps = $('#tblSearchBaps').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });
        $("#tblSearchBaps tbody").on("click", "a.btdetail", function (e) {
            e.preventdefault();
            var table = $('#tblSearchBaps').DataTable();
            var data = table.row($(this).parents("tr")).data();

        });

        $(window).resize(function () {
            $("#tblSearchBaps").DataTable().columns.adjust().draw();
        });

        //bind customer dan copmany but value fixed from main page
        Control.BindingSelectCompanyByParam("#slmdlCopmany");
        Control.BindingSelectOperatorByParam("#slmdlCustomer");
        $("#slmdlCustomer").val('TELKOM').trigger('change').prop('disabled', true)

        Control.BindingSelectProductTypeByParam("#slmdlProductType");
        Control.BindingSelectStip("#slmdlStipCategory");
        Control.BindingSelectSiro("#slmdlSiro");
        Control.BindingSelectMonthYear("#slmdlBaukDoneStart, #slmdlBaukDoneEnd");
        $('#slmdlBaukDoneStart').datepicker().on("changeDate", function () {
            var startVal = $('#slmdlBaukDoneStart').val();
            $('#slmdlBaukDoneEnd').data('datepicker').setStartDate(startVal);
        });
        $('#slmdlBaukDoneEnd').datepicker().on("changeDate", function () {
            var endVal = $('#slmdlBaukDoneEnd').val();
            $('#slmdlBaukDoneStart').data('datepicker').setEndDate(endVal);
        });

        $("#btnSearchBapsSite").unbind().click(function (e) {
            e.preventDefault();
            ModalInputBulky.Search();
        });
        $("#btResetBapsSite").unbind().click(function (e) {
            ModalInputBulky.Init();
        });

        //btn add site
        $("#btSaveAddSite").unbind().click(function (e) {
            ModalInputBulky.SaveAddSite();
        });
        $("#btCloseAddSite").unbind().click(function (e) {
            $('#mdlAddSite').modal('hide');
        });
        $(".btnSearchGridBaps").unbind().click(function () {
            ModalInputBulky.Search();
        });
        $("#slmdlSONumberMultiple").select2({
            tags: true,
            multiple: true,
            width: '100%',
            createTag: function (params) {
                return {
                    id: params.term,
                    text: params.term,
                    newOption: true
                }
            },
            templateResult: function (data) {
                var $result = $("<span></span>");

                $result.text(data.text);

                if (data.newOption) {
                    $result.append(" <em>(add)</em>");
                }

                return $result;
            }
        });

        //EVENT CHECKBOX CLICKED
        $("#tblSearchBaps").on('change', 'tbody tr .checkboxes', function () {
            var SoNumber = $(this).parent().attr('id');
            var table = $("#tblSearchBaps").DataTable();
            var idComponents = SoNumber
            var id = idComponents[1];
            var DataRows = {};
            var Row = table.row($(this).parents('tr')).data();

            if (this.checked) {
                if (!(Row.SoNumber == '' || Row.SoNumber == null)) {
                    DataRows.ActivityID = Row.ActivityID
                    //DataRows.BAPSNumber = Row.BAPSNumber
                    //DataRows.BapsDate = Row.BapsDate
                    DataRows.CompanyID = Row.CompanyID
                    DataRows.CustomerID = Row.CustomerID
                    DataRows.CustomerSiteID = Row.CustomerSiteID
                    DataRows.ID = Row.ID
                    DataRows.SIRO = Row.SIRO
                    DataRows.SiteID = Row.SiteID
                    DataRows.SiteName = Row.SiteName
                    DataRows.SoNumber = Row.SoNumber
                    DataRows.StipID = Row.StipID
                    DataRows.TowerTypeID = Row.TowerTypeID
                    DataRows.CustomerSiteName = Row.CustomerSiteName

                    TempDataBapsBulky.push(DataRows);
                }
            } else {
                var index = TempDataBapsBulky.findIndex(function (data) {
                    return data.SoNumber == Row.SoNumber;
                });
                TempDataBapsBulky.splice(index, 1);
            }

        });

        //open modal dialig
        $("#mdlAddSite").modal({ backdrop: 'static', keyboard: false }).show();
    },
    Search: function () {
        $("#tblSearchBaps").DataTable({
            "deferRender": true,
            "processing": true,
            "serverSide": true,
            "bSortCellsTop": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/NewBapsInput/getSonumbList",
                "type": "POST",
                "datatype": "json",
                "data":  ModalHelper.ModalFilterGetParam(),
            },
            "filter": false,
            "destroy": true,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "columns": [{
                orderable: false,
                mRender: function (data, type, full) {
                    var strReturn = "";
                    if (ModalHelper.IsGridDataExistsInTempData(full.SoNumber, TempDataBapsBulky)) {
                        strReturn += "<label id='" + full.SoNumber + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.SoNumber + "'><input type='checkbox' class='checkboxes' checked='true' /><span></span></label>";

                    }
                    else {
                        strReturn += "<label id='" + full.SoNumber + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.SoNumber + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                    }
                    return strReturn;
                }
            },
            { data: "SoNumber" },
            { data: "CustomerID" },
            { data: "SiteID" },
            { data: "SiteName" },
            { data: "CustomerSiteID" },
            { data: "CustomerSiteName" },
            
            { data: "CompanyID" },
            { data: "CompanyName" },
            { data: "SIRO" },
            { data: "STIPNumber" },
            { data: "StipCode" },
            { data: "Product" },
            {
                data: "MLADate", render: function (data, type, full) {
                    if (data != null) {
                        return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                    } else {
                        return "";
                    }
                }
            },
            { data: "MLANumber" },
            { data: "BaukNumber" },
            {
                data: "BaukDate", render: function (data, type, full) {
                    if (data != null) {
                        return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                    } else {
                        return "";
                    }
                }
            },
            { data: "PONumber" },
            {
                data: "PoDate", render: function (data, type, full) {
                    if (data != null) {
                        return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                    } else {
                        return "";
                    }
                }
            },
            {
                data: "POInputDate", render: function (data, type, full) {
                    if (data != null) {
                        return moment(data, 'YYYY-MM-DD').format('DD MMM YYYY');
                    } else {
                        return "";
                    }
                }
            }

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                    '<input type="checkbox" name="SelectAll" id="tblSearchBaps_chkAll" class="group-checkable" data-set="#tblSearchBaps .checkboxes" />' +
                    '<span></span> ' +
                    '</label>';
                var th = $("#tblSearchBaps th.select-all-raw").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-raw").unbind().on("change", ".group-checkable", function (e) {
                    var set = $("#tblSearchBaps .checkboxes");
                    var checked = jQuery(this).is(":checked");
                    jQuery(set).each(function () {
                        label = $(this).parent();
                        var id = label.attr("id");
                        if (label.attr("style") != "display:none") {
                            /* Replace the following code with the code to select all checkboxes in all pages */
                            if (checked) {
                                $(this).prop("checked", true);
                                $(this).parents('tr').addClass('active');
                                //$(this).trigger("change");

                                $("." + id).addClass("active");
                                $("." + id).prop("checked", true);
                                //$("." + id).trigger("change");
                            } else {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");
                                //$(this).trigger("change");

                                $(".Row" + id).removeClass("active");
                                $("." + id).prop("checked", false);
                                //$("." + id).trigger("change");
                            }
                        }
                    });
                    if (checked) {
                        TempDataBapsBulky = [];
                        ModalHelper.SelectAllSeachSiteIDs();
                    }
                    else
                        TempDataBapsBulky = (TempDataBapsBulkyAdded || []).concat();
                });
            },
            

        });
    },
    Reset: function () { //Modal.Reset
        var srt = $("script#mdlAddSiteHtml").html();
        $("#mdlAddSiteContainer").html(srt);
        TempDataBapsBulky = (TempDataBapsBulkyAdded || []).concat();

    },

    SaveAddSite: function () {
        if (TempDataBapsBulky.length > 0) {
            TempDataBapsBulkyAdded = (TempDataBapsBulky || []).concat(); //using splice prevent being same object

            $("#tblSite").DataTable({
                "serverSide": false,
                "filter": true,
                "destroy": true,
                "async": false,
                "data": TempDataBapsBulkyAdded,
                "columns": [{
                    orderable: false,
                    mRender: function (data, type, full) {
                        return "<i class='fa fa-remove btn bn-xs red deleteRow'></i>";
                    }
                },
                { data: "SoNumber" },
            { data: "CustomerID" },
            { data: "SiteID" },
            { data: "SiteName" },
            { data: "CustomerSiteID" },
            { data: "CustomerSiteName" }

                ],
                "columnDefs": [{ "targets": [0], "orderable": false }],
            });
            $("#tblSite").unbind();
            $("#tblSite").on("click", ".deleteRow", function (e) {
                var table = $("#tblSite").DataTable();
                var row = table.row($(this).parents('tr')).data();
                table.row($(this).parents('tr')).remove().draw(false);
                ModalHelper.RemoveDataInTempData(row.SoNumber, TempDataBapsBulkyAdded);
            });
            $('#mdlAddSite').modal('hide');
        }
        else {
            Common.Alert.Warning("Please tick the checkbox to select data.");
        }
    }

}

var ModalHelper = {
    SelectAllSeachSiteIDs: function () {
        //for CheckAll Pages
        var params = {
            strCompanyId: $("#slmdlCopmany").val(),
            strCustomerID: $("#slmdlCustomer").val(),
            strProductID: $("#slmdlProductType").val(),
            strStipID: $("#slmdlStipCategory").val(),
            strSiroID: $("#slmdlSiro").val(),
            //hardcoded
            strTenantTypeID: "validation",
            strAction: '14',
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/NewBapsInput/gridSonumbListIds",
            type: "POST",
            dataType: "json",
            async: false,
            data: ModalHelper.ModalFilterGetParam(),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            AjaxData = data;
            if (data != null) {
                //model return to much properties
                for (var i = 0; i < data.length; i++) {
                    TempDataBapsBulky.push({
                        SoNumber: data[i].SoNumber,
                        ActivityID: data[i].ActivityID,
                        BapsDate: data[i].BapsDate,
                        CompanyID: data[i].CompanyID,
                        CustomerID: data[i].CustomerID,
                        CustomerSiteID: data[i].CustomerSiteID,
                        CustomerSiteName: data[i].CustomerSiteName,
                        ID: data[i].ID,
                        InstallationAmount: data[i].InstallationAmount,
                        InvoiceType: data[i].InvoiceType,
                        LeasePriod: data[i].LeasePriod,
                        OmPrice: data[i].OmPrice,
                        PowerTypeID: data[i].PowerTypeID,
                        PriceAmount: data[i].PriceAmount,
                        ProductID: data[i].ProductID,
                        SIRO: data[i].SIRO,
                        STIPNumber: data[i].STIPNumber,
                        SiteID: data[i].SiteID,
                        SiteName: data[i].SiteName,
                        SoNumber: data[i].SoNumber,
                        StipID: data[i].StipID,
                        TowerTypeID: data[i].TowerTypeID,
                    });
                }
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
        //Common.Alert.Error(AjaxData);
        return AjaxData;
    },

    IsGridDataExistsInTempData: function (soNumber, tempData) {
        for (var i = 0; i < tempData.length; i++) {
            var bapsId = tempData[i].SoNumber;
            if (bapsId == soNumber)
                return true;
        }
        return false;
    },
    RemoveDataInTempData: function (soNumber, tempData) {
        for (var i = 0; i < tempData.length; i++) {
            var bapsId = tempData[i].SoNumber;
            if (bapsId == soNumber)
                tempData.splice(i, 1);
        }
    },
    ModalFilterGetParam: function () {
        var params = {
            strCompanyId: $("#slmdlCopmany").val(),
            strCustomerID: $("#slmdlCustomer").val(),
            strProductID: $("#slmdlProductType").val(),
            strStipID: $("#slmdlStipCategory").val(),
            strSiroID: $("#slmdlSiro").val(),
            //hardcoded
            strTenantTypeID: "validation-bulky",
            strAction: '14',
            mstRAActivityID: '19',
            strSoNumber: $("#schMdlSONumber").val(),
            strSiteName: $("#schMdlSiteName").val(),
            strSONumberMultiple: $("#slmdlSONumberMultiple").val()
        };
        var sDate = $("#slmdlBaukDoneStart").val() != '' ? moment('1 ' + $("#slmdlBaukDoneStart").val(), 'D MMM YYYY').format('YYYY/MM/DD') : '';
        var _eDate = '';
        if ($("#slmdlBaukDoneEnd").val() != '') {
            var eDate = moment('1 ' + $("#slmdlBaukDoneEnd").val(), 'D MMM YYYY').format('YYYY/MM/DD');
            var dayInMonth = moment(eDate, "YYYY/MM/DD").daysInMonth()
            _eDate = moment(eDate, 'YYYY/MM/DD').set({ 'D': dayInMonth }).format('YYYY/MM/DD');
        }
        params.strStartBaukDoneDate = sDate;
        params.strEndBaukDoneDate = _eDate;

        return params;
    }

}

