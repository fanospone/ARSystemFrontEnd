var TempDataId = [];
var TempDataIdAdded = [];
jQuery(document).ready(function () {
    Form.Init();
    $("#btAddSite").unbind().click(function () {
        Modal.Reset();
        Modal.Init();
    });
    var tblSite = $('#tblSite').dataTable({
        "filter": false,
        "destroy": true,
        "data": []
    });

    //auto calculate total amount
    $("#ibulkStartLeasePeriod, #ibulkStartEffectivePeriod, #ibulkEndEffectivePeriod, #ibulkBaseLeasePrice, #ibulkServicePrice").change(function () {
        Helper.GetTotalAmount();
    });
    //auto calculate total amount
    $("#ibulkCustomerID").on('select2:select', function () {
        Helper.GetTotalAmount();
    });

    $("#btnClearBaps").unbind().click(function () {
        TempDataIdAdded = [];
        TempDataId = [];
        $("#formBapsInformation input, #formBapsInformation textarea").val("");
        $("#formProjectInformation input").val("");

        $("#ibulkStartLeasePeriod").trigger("changeDate");
        $("#ibulkEndLeasePeriod").trigger("changeDate");
        $("#ibulkStartEffectivePeriod").trigger("changeDate");
        $("#ibulkEndEffectivePeriod").trigger("changeDate");
        Form.Clear();
    });
   
});

var Control = {

    BindingSelectCompany: function (ele) {
        $.ajax({
            url: "/api/MstDataSource/TBGSys_Company",
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

    BindingSelectOperator: function (ele) {
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

    BindingSelectCurrency: function (ele) {
        $(ele).html("<option></option>")
        $(ele).append("<option value='IDR'>IDR</option>");
        $(ele).append("<option value='USD'>USD</option>");
        $(ele).val('IDR').trigger('change');
        $(ele).select2({ placeholder: "Select", width: null });
    },

    BindingDatePicker: function () {
        $(".datepicker").datepicker({
            format: "dd M yyyy",
            clearBtn: true
        });
    },

    BindingSelectProductType: function (ele) {
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
    BindingSelectMonthYear: function(ele){
        $(ele).datepicker({
            format: "M yyyy",
            startView: "months",
            minViewMode: "months",
            clearBtn: true
        });
    }
}

var Form = {
    Init: function () {
        Control.BindingSelectCompany("#ibulkCompanyID");
        Control.BindingSelectOperator("#ibulkCustomerID");
        $("#ibulkCustomerID").val('TELKOM').trigger('change').prop('disabled', true)
        Control.BindingSelectCurrency("#ibulkBaseLeaseCurrency, #ibulkSeriveCurrency");
        Control.BindingDatePicker();
        $('#ibulkStartLeasePeriod').datepicker().on("changeDate", function () {
            var startVal = $('#ibulkStartLeasePeriod').val();
            $("#ibulkStartEffectivePeriod").val(startVal).trigger("chageDate");
            $('#ibulkEndLeasePeriod').data('datepicker').setStartDate(startVal);
        });
        $('#ibulkEndLeasePeriod').datepicker().on("changeDate", function () {
            var endVal = $('#ibulkEndLeasePeriod').val();
            $('#ibulkStartLeasePeriod').data('datepicker').setEndDate(endVal);
        });

        $('#ibulkStartEffectivePeriod').datepicker().on("changeDate", function () {
            var startVal = $('#ibulkStartEffectivePeriod').val();
            $('#ibulkEndEffectivePeriod').data('datepicker').setStartDate(startVal);
        });
        $('#ibulkEndEffectivePeriod').datepicker().on("changeDate", function () {
            var endVal = $('#ibulkEndEffectivePeriod').val();
            $('#ibulkStartEffectivePeriod').data('datepicker').setEndDate(endVal);
        });

        $('#ibulkCustomerID').val("TELKOM");
        $('#ibulkCustomerID').prop("disabled", true);

        $("#btnSubmitBaps").on('click', function (e) {
            e.preventDefault();
            Form.Submit();
        });
    },
    Submit: function () {
        var l = Ladda.create(document.querySelector("#btnSubmitBaps"))
        l.start();
        App.blockUI({ target: "#pnlSummary", boxed: true });
        $("#formBapsInformation input, #formBapsInformation textarea, #formProjectInformation select").prop('required', true)
        var result = "";
        var formBapsValid = $("#formBapsInformation").parsley().validate();
        var formProjValid = $("#formProjectInformation").parsley().validate();
        if (!(formBapsValid && formProjValid)) {
            $("#formBapsInformation input, #formBapsInformation textarea, #formProjectInformation input").prop('required', false)
            if ($('#ibulkStartLeasePeriod').val() == "")
                result += " Start Lease Period,";
            if ($('#ibulkEndLeasePeriod').val() == "")
                result += " End Lease Period,";
            if ($('#ibulkCustomerID').val() == "")
                result += " Customer ID,";
            if ($('#ibulkCompanyID').val() == "")
                result += " Company ID,";
            if ($('#ibulkStartEffectivePeriod').val() == "")
                result += " Start Effective Period,";
            if ($('#ibulkEndEffectivePeriod').val() == "")
                result += " End Effective Period,";
            if ($('#ibulkRemarks').val() == "")
                result += " Remarks,";
            if ($('#ibulkBaseLeaseCurrency').val() == "")
                result += " Base Lease Currenct,";
            if ($('#ibulkSeriveCurrency').val() == "")
                result += " Service Currenct,";
            if ($('#ibulkBaseLeasePrice').val() == "")
                result += " Base Lease Type,";
            if ($('#ibulkServicePrice').val() == "")
                result += " Base Service Price,";
            if ($('#ibulkTotalAmount').val() == "")
                result += " Total Amount";
            l.stop();
            Common.Alert.Warning(result + " Is Mandatory");
            App.unblockUI("#pnlSummary");
            return false;
        }
        if (result == "") {
            //dilepas lagi untuk button filter
            $("#formBapsInformation input, #formBapsInformation textarea, #formProjectInformation select").prop('required', false)
                if (TempDataIdAdded.length > 0) {
                    var params = {
                        StartBapsDate: $('#ibulkStartLeasePeriod').val(),
                        EndBapsDate: $('#ibulkEndLeasePeriod').val(),
                        CustomerID: $('#ibulkCustomerID').val(),
                        CompanyInvoiceId: $('#ibulkCompanyID').val(),
                        StartEffectiveDate: $('#ibulkStartEffectivePeriod').val(),
                        EndEffectiveDate: $('#ibulkEndEffectivePeriod').val(),
                        Remark: $('#ibulkRemarks').val(),
                        BaseLeaseCurrency: $('#ibulkBaseLeaseCurrency').val(),
                        ServiceCurrency: $('#ibulkSeriveCurrency').val(),
                        BaseLeasePrice: $('#ibulkBaseLeasePrice').val(),
                        ServicePrice: $('#ibulkServicePrice').val(),
                        TotalAmount: $('#ibulkTotalAmount').val(),
                        ListMstBaps: TempDataIdAdded,
                    }

                    App.blockUI({
                        target: ".modal-content", boxed: true
                    });

                    $.ajax({
                        url: "/api/BAPSValidation/submitBulkyBapsValidation",
                        type: "post",
                        dataType: "json",
                        contentType: "application/json",
                        data: JSON.stringify(params),
                        cache: false,
                        beforeSend: function (xhr) {
                        }
                    }).done(function (data, textStatus, jqXHR) {
                        if (Common.CheckError.List(data)) {
                            if (data.ErrorType != 0) {
                                Common.Alert.Warning(data.ErrorMessage);
                            } else {
                                Common.Alert.Success("Data has been saved!")
                                $("#ibulkStartLeasePeriod").val("").trigger('changeDate');
                                $("#ibulkEndLeasePeriod").val("").trigger('changeDate');
                                $("#ibulkCompanyID").val("").trigger('change');
                                $("#ibulkStartEffectivePeriod").val("").trigger('changeDate');
                                $("#ibulkEndEffectivePeriod").val("").trigger('changeDate');
                                $("#ibulkRemarks").val("").trigger('change');
                                $("#ibulkBaseLeasePrice").val("");
                                $("#ibulkServicePrice").val("");
                                
                                TempDataIdAdded = [];
                                TempDataId = [];
                                var tblSite = $('#tblSite').dataTable({
                                    "filter": false,
                                    "destroy": true,
                                    "data": []
                                });
                                $("#ibulkTotalAmount").val("");
                                setTimeout(function () {
                                    $("#ibulkTotalAmount").val("");
                                }, 1000);
                            }
                            App.unblockUI({
                                target: ".modal-content"
                            });
                            l.stop();
                            App.unblockUI("#pnlSummary");
                        }
                        else {
                            Common.Alert.Error(data.ErrorMessage);
                            l.stop();
                            App.unblockUI("#pnlSummary");
                        }
                    })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            Common.Alert.Error(errorThrown)
                            App.unblockUI({
                                target: ".modal-content"
                            });
                            l.stop();
                            App.unblockUI("#pnlSummary");
                        })
                } else {
                    // Common.Alert.Error("Please Input Year Target, Month Target, Reconcile Type and Type Power !");
                    Common.Alert.Warning("Site cannot be empty");
                    l.stop();
                    App.unblockUI("#pnlSummary");
                }
            }

    },
    Clear: function () {
        $("#tblSite").DataTable({
            "serverSide": false,
            "filter": true,
            "destroy": true,
            "async": false,
            "data": TempDataIdAdded,
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
    }
}

var Modal = {
    Init: function () { //Modal.Init
        var isValid = '';
        //$("#formBapsInformation #ibulkCustomerID, #formBapsInformation #ibulkCompanyID").prop('required', true);
        //if (!($("#formBapsInformation").parsley().validate())) {
        //    $("#formBapsInformation #ibulkCustomerID, #formBapsInformation #ibulkCompanyID").prop('required', false);
        //    if ($("#ibulkCustomerID").val() == '') {
        //        isValid = 'Customer must be selected.\n';
        //    }
        //    if ($("#ibulkCompanyID").val() == '') {
        //        isValid += 'Company must be selected.\n';
        //    }
        //    if (isValid != '') {
        //        Common.Alert.Warning("Please select mandatory data first.\n" + isValid);
        //        return false;
        //    }
        //    return false
        //}
        //$("#formBapsInformation #ibulkCustomerID, #formBapsInformation #ibulkCompanyID").prop('required', false);

        TempDataId = (TempDataIdAdded || []).concat();//using prevent being same object
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
        var compValue = $("#ibulkCompanyID").select2('data')[0];
        var opValue = $("#ibulkCustomerID").select2('data')[0];
        $("#slmdlCustomer").html((compValue != null ? "<option value='" + opValue.id + "' selected>" + opValue.text + "</option>" : "<option></option>"))
        //$("#slmdlCopmany").html((compValue != null ? "<option value='" + compValue.id + "' selected>" + compValue.text + "</option>" : "<option></option>"))
        Control.BindingSelectCompany("#slmdlCopmany");
        Control.BindingSelectProductType("#slmdlProductType");
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
        $("#btnSearch").unbind().click(function (e) {
            e.preventDefault();
            Modal.Search();
        });
        $("#btReset").unbind().click(function (e) {
            Modal.Init();
        });
        $(".btnSearchGridBaps").unbind().click(function () {
            Modal.Search();
        });
        //btn add site
        $("#btSaveAddSite").unbind().click(function (e) {
            Modal.SaveAddSite();
        });
        $("#btCloseAddSite").unbind().click(function (e) {
            $('#mdlAddSite').modal('hide');
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
                    DataRows.BAPSNumber = Row.BAPSNumber
                    DataRows.BapsDate = Row.BapsDate
                    DataRows.CompanyID = Row.CompanyID
                    DataRows.CustomerID = Row.CustomerID
                    DataRows.CustomerSiteID = Row.CustomerSiteID
                    DataRows.ID = Row.ID
                    DataRows.InstallationAmount = Row.InstallationAmount
                    DataRows.InvoiceType = Row.InvoiceType
                    DataRows.Label = Row.Label
                    DataRows.LeasePriod = Row.LeasePriod
                    DataRows.OmPrice = Row.OmPrice
                    DataRows.PowerTypeID = Row.PowerTypeID
                    DataRows.PriceAmount = Row.PriceAmount
                    DataRows.ProductID = Row.ProductID
                    DataRows.StipSiro = Row.SIRO
                    DataRows.STIPNumber = Row.STIPNumber
                    DataRows.SiteID = Row.SiteID
                    DataRows.SiteName = Row.SiteName
                    DataRows.SoNumber = Row.SoNumber
                    DataRows.StipID = Row.StipID
                    DataRows.TowerTypeID = Row.TowerTypeID
                    DataRows.CustomerSiteName = Row.CustomerSiteName
                    
                    TempDataId.push(DataRows);

                }
            } else {
                var index = TempDataId.findIndex(function (data) {
                    return data.SoNumber == Row.SoNumber;
                });
                TempDataId.splice(index, 1);
                if ($("#tblSearchBaps_chkAll").is(":checked")) {
                    $("#tblSearchBaps_chkAll").prop("checked",false)
                }
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
                "url": "/api/BAPSValidation/gridSonumbList",
                "type": "POST",
                "datatype": "json",
                "data": Helper.ModalFilterGetParam(),
            },
            "filter": false,
            "destroy": true,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "columns": [{
                orderable: false,
                mRender: function (data, type, full) {
                    var strReturn = "";
                    if (Helper.IsGridDataExistsInTempData(full.SoNumber, TempDataId)) {
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

                                $("." + id).removeClass("active");
                                $("." + id).prop("checked", false);
                                //$("." + id).trigger("change");
                            }
                        }
                    });
                    if (checked){
                        TempDataId = [];
                        Helper.SelectAllSeachSiteIDs();
                    }
                   else
                        TempDataId = (TempDataIdAdded || []).concat();
                });
            },
        });
    },
    Reset: function () { //Modal.Reset
        var srt = $("script#mdlAddSiteHtml").html();
        $("#mdlAddSiteContainer").html(srt);
        TempDataId = (TempDataIdAdded || []).concat();
    },

    SaveAddSite: function () {
        if (TempDataId.length > 0) 
            {
            TempDataIdAdded = (TempDataId || []).concat(); //using splice prevent being same object
            $("#tblSite").DataTable({
                "serverSide": false,
                "filter": true,
                "destroy": true,
                "async": false,
                "data": TempDataIdAdded,
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
                Helper.RemoveDataInTempData(row.SoNumber, TempDataIdAdded);
            });
            $('#mdlAddSite').modal('hide');

        }
        else {
            Common.Alert.Warning("Please tick the checkbox to select data.");
        }

    }
}

var Helper = {
    SelectAllSeachSiteIDs: function () {
        var AjaxData = [];
        $.ajax({
            url: "/api/BAPSValidation/gridSonumbListIds",
            type: "POST",
            dataType: "json",
            async: false,
            data: Helper.ModalFilterGetParam(),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            AjaxData = data;
            if (data != null) {
                //model return to much properties
                for (var i = 0; i < data.length; i++) {
                    TempDataId.push({
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
                        StipSiro: data[i].SIRO,
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
    GetTotalAmount: function () {
        var params = {
            CustomerID: $("#ibulkCustomerID").val(),
            StartInvoiceDate: $("#ibulkStartLeasePeriod").val(),
            EndInvoiceDate: $("#ibulkEndEffectivePeriod").val(),
            InvoiceAmount: $("#ibulkBaseLeasePrice").val(),
            ServiceAmount: $("#ibulkServicePrice").val(),
            DropFODistance: null, //checked its save to be null, fixed for customer TELKOM 
            ProductID: null
        }
        if (params.startInvoiceDate == '' || params.EndInvoiceDate == '' || params.InvoiceAmount == '' || params.ServiceAmount == '') {
            $("#ibulkTotalAmount").val(0);
        }
        $.ajax({
            url: "/api/ReconcileData/GetProRateAmount",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
        })
        .done(function (data, textStatus, jqXHR) {
            var result = (parseFloat(data.data)).toString();
            $("#ibulkTotalAmount").val(Common.Format.CommaSeparation(result));
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });

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