Data = {};

//filter search 
var fsRenewalYear = "";
var fsCompanyId = "";
var fsOperator = "";
var fsReconcileType = "";
var fsRenewalYearSeq = "";
var fsDueDatePerMonth = "";
var fsCurrency = "";
var fsRegional = "";
var fsProvince = "";
var PurchaseOrderID = "";
var CurrentTab = 7;
var DocPath = "";
var SiteData = [];
Data.RowSelectedData = [];
Data.RowDataSelected = [];
var CurrentCompanyID = "";
var CurrentCustomerID = "";
var CurrentBapsType = "";
var CurrentCurrency = "";
var FilterCustomerTab = 0;

jQuery(document).ready(function () {
    $('.datepicker').datepicker({
        format: 'd-M-yyyy',
        autoclose: true
    });
    $('.select2').select2({});

    $('#fmBill').hide();
    DocPath = $('#DocPath').val();
    Data.SiteRow = [];
    Data.RowSelectedSite = [];
    Data.RowSelectedRaw = [];
    Data.RowSelectedProcess = [];
    Form.Init();
    //TableProcess.Init();
    TableRaw.Init();
    TableRaw.Search();
    TableSiteInfo.Init();
    TableProcessDetail.Init();
    TableProcess.Init();

    Data.RowSelectedData = [];
    Data.RowDataSelected = [];
    SiteData = [];

    //panel Summary
    $("#formSearch").submit(function (e) {
        TableProcess.Search();
        e.preventDefault();
    });
    
    $('#tabTrx').unbind().on('tabsactivate', function (event, ui) {
        var newIndex = ui.newTab.index();
        //CurrentTab = 7;
        //CurrentTab = newIndex + CurrentTab;
        FilterCustomerTab = newIndex;

        if (newIndex == 1) {
            TableData.Init();
            Data.RowDataSelected = [];
            Data.RowSelectedData = [];
            $('#tblDataSelected').hide();
            $('#DetailNonTSEL').hide();
            $('#file-upload-filenames').text("");
            $('#PoAmountVal').val("");
            $('#PoTenantVal').val("");
            $('#PoNumberVal').val("");
            $('#PoDateVal').val("");
            $('#PoReceiveDateVal').val("");
            $('#tbStartPeriod').val("");
            $('#tbEndPeriod').val("");
            $('#tbRemarksVal').val("");
            CurrentBapsType = "";
            CurrentCompanyID = "";
            CurrentCurrency = "";
            CurrentCustomerID = "";
            $('#PoTypeVal').removeAttr("disabled");
            $('#PoCurrencyVal').removeAttr("disabled");

            $('.panelFilters').load();
            $('#DetailNonTSEL').hide();
            Control.BindingSelectOperators($('#slSearchCustomer'));
            Control.BindingSelectCompany($('#slSearchCompany'));
            Control.BindingSelectStip($('#slSearchStip'));
            Control.BindingSelectProduct($('#slSearchProductType'));
            Control.BindingSelectBapsType($('#slSearchBapsType'));
            Control.BindingSelectPowerType($('#slSearchPowerType'));
            CurrentTab = 7;
            $("#btProcess").hide();
        }
        else if (newIndex == 2) {
            $('.panelFilter').show();
            $('.panelFilter').load();
            Control.BindingSelectCompany($('#slSearchCompanyDone'));
            Control.BindingSelectOperators($('#slSearchCustomerDone'));
            CurrentTab = 8;
            $("#btProcess").show();
        }
        else {
            CurrentTab = 7;
        }
        
    });

    $('#tabPO').unbind().on('tabsactivate', function (event, ui) {
        var newIndex = ui.newTab.index();
        CurrentTab = 7;
        CurrentTab = newIndex + CurrentTab;

        if (newIndex == 1) {
            Data.RowSelectedProcess = [];
            TableProcess.Search();
        }
        else {
            Data.RowSelectedRaw = [];
            TableRaw.Search();
        }
    });

    $("#btSearch").unbind().click(function () {
        if (CurrentTab == 7)
            TableRaw.Search();
        else
            TableProcess.Search();
    });

    $("#btSearchDone").unbind().click(function () {
        TableProcess.Search();
    });

    $("#btReset").unbind().click(function () {
        $("#slSearchCompanyName").val(null).trigger("change");
        $("#slGroupBy").val(null).trigger("change");
    });

    $("#btResetDone").unbind().click(function () {
        $("#slSearchCompanyDone").val(null).trigger("change");
        $("#slSearchCustomerDone").val(null).trigger("change");
        $("#slSearchTypeDone").val(null).trigger("change");
        $("#slSearchCurrencyDone").val(null).trigger("change");
    });

    $("#btAddPO").unbind().click(function () {
        $('#slSearchOperator2').val("").trigger("change");
        $('.panelDetailPO').fadeIn();
        $(".panelSearchZero").fadeOut();
        $(".panelSearchBegin").fadeOut();
        $(".panelFilter").fadeOut();
        $(".panelSearchResult").fadeOut();
        $("#btSaveData").fadeIn();
        $("#btUpdateData").fadeOut();
        $("#btBoqSelect").fadeOut();
        $("#btProcess").fadeOut();
    });

    $("#btBackToList").unbind().click(function () {
        Form.ClearInputDetail();
        Form.BackToList();
        $("#btProcess").fadeIn();
        //Form.GetCountRows();
    });

    $("#btSearchData").unbind().click(function () {
        Form.GetDetailPO();
        Form.GetCountRows();
        //$('#tblProcessDetail tr').length;
        
        //if (Operator == 'TSEL')
        //    $('#TotalTenant').text($('#tblProcessDetail tr').length);
        //Data.POAmount = [];
        //var POAmount = 0;
        //var oTable = $('#tblProcessDetail').DataTable();
        //var rowcollection = oTable.$(".checkboxes:checked", { "page": "all" });
        //rowcollection.each(function (index, elem) {
        //    POAmount = parseFloat($(elem).attr('amountRupiah'));
        //    Data.POAmount.push(POAmount);
        //    //Do something with 'checkbox_value'
        //});
        //$('#POAmount').text(Common.Format.CommaSeparation(POAmount));
    });

    $("#btProcess").unbind().click(function () {
        var count = 0;
        var customerselected = "";

        if (CurrentTab == 7){
            count = Data.RowSelectedRaw.length; 
            customerselected = $('#slSearchOperator').val();
        }
        else {
            customerselected = $('#slSearchCustomerDone').val();
            count = Data.RowSelectedProcess.length;
        }
            

        if (count == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else {
            $('#mdlNextActivity').modal('show');

            Control.BindingNextStep(customerselected);
        }
            
    });

    $("#btConfirm").unbind().click(function () {
        var Activity = $("#slNextActivity").val();
        var data = [];
        if (CurrentTab == 7)
            data = Data.RowSelectedRaw;
        else
            data = Data.RowSelectedProcess;

        if (data.length < 1) {
            Common.Alert.Warning("Please Select One or More Data")
        }
        else {
            Form.UpdateNextActivity(data.toString(), Activity);
        }
    });

    $('#tblData').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');

        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            Data.RowSelectedData.push(parseInt(id));

        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row" + id).removeClass("active");
            Helper.RemoveElementFromArray(Data.RowSelectedData, parseInt(id));
        }
    });

    $('#tblRaw').find('.group-checkable').change(function () {
        var set = jQuery(this).attr("data-set");
        var checked = jQuery(this).is(":checked");
        jQuery(set).each(function () {
            if (checked) {
                $(this).prop("checked", true);
                $(this).parents('tr').addClass('active');
                $(this).trigger("change");
            } else {
                $(this).prop("checked", false);
                $(this).parents('tr').removeClass("active");
                $(this).trigger("change");
            }
        });
        if (checked)
            Data.RowSelectedRaw = Helper.GetListId(CurrentTab);
        else
            Data.RowSelectedRaw = [];
    });

    $('#tblRaw').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');

        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            Data.RowSelectedRaw.push(parseInt(id));
            //Common.Alert.Warning(JSON.stringify(Data.RowSelectedRaw));
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row" + id).removeClass("active");
            Helper.RemoveElementFromArray(Data.RowSelectedRaw, parseInt(id));
        }
    });

    $('#tblProcess').find('.group-checkable').change(function () {
        var set = jQuery(this).attr("data-set");
        var checked = jQuery(this).is(":checked");
        jQuery(set).each(function () {
            if (checked) {
                $(this).prop("checked", true);
                $(this).parents('tr').addClass('active');
                $(this).trigger("change");
            } else {
                $(this).prop("checked", false);
                $(this).parents('tr').removeClass("active");
                $(this).trigger("change");
            }
        });
        if (checked)
            Data.RowSelectedProcess = Helper.GetListId(CurrentTab);
        else
            Data.RowSelectedProcess = [];
    });

    $('#tblProcess').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            Data.RowSelectedProcess.push(parseInt(id));
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row" + id).removeClass("active");
            Helper.RemoveElementFromArray(Data.RowSelectedProcess, parseInt(id));
        }
    });

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

    $('#postedFile2').on('change', function () {
        var input = document.getElementById('postedFile2');
        var infoArea = document.getElementById('file-upload-filenames');
        // the change event gives us the input it occurred in 
        //var input = event.srcElement;

        // the input has an array of files in the `files` property, each one has a name that you can use. We're just using the name here.
        var fileName = input.files[0].name;

        // use fileName however fits your app best, i.e. add it into a div
        infoArea.textContent = 'File name: ' + fileName;
    });

    $("#btnDetailSave").unbind().click(function () {
        var l = Ladda.create(document.querySelector("#btnDetailSave"));
        l.start();
        TableData.ValidatePO();
        l.stop();
    });


    $('#btBoqSelect').on('click', function () {
        var BoqList = Data.Selected.mstRABoqID.split(',');
        var $exampleMulti = $("#slBOQ").select2();
        $exampleMulti.val(BoqList).trigger("change");
    });


    $("#btSaveData").click(function (e) {
        e.preventDefault();
        if ($("#formDetailData").parsley().validate()) {
            Form.SavePO();
        }
    });

    $("#btSearchNon").unbind().click(function () {
        if ($("#formSearchs").parsley().validate()) {
            TableData.Search();
        }
    });
    
    $("#tblDataSelected tbody").unbind().on("click", "button.btDeleteSite", function (e) {
        var table = $("#tblDataSelected").DataTable();
        var buttonId = $(this).attr("id");
        var idComponents = buttonId.split('btDeleteSite');
        var id = idComponents[1];
        //Row with ID in btDeleteSite back to tblSummary with normal state checkbox
        $("#Row" + id).removeClass('active');
        $("#" + id + " input[type=checkbox]").removeAttr("checked");
        $("#" + id).show();

        /* Uncheck the checkbox in cloned table */
        $('.Row' + id).removeClass('active');
        $('.' + id + ' input[type=checkbox]').removeAttr("checked");
        $('.' + id).show();

        table.row($(this).parents('tr')).remove().draw();
        //Delete id in Array for rendering when change page
        Helper.RemoveElementFromArray(Data.RowDataSelected, parseInt(id));
        if (Data.RowDataSelected.length == 0) {
            $('#DetailNonTSEL').hide();
            CurrentBapsType = "";
            CurrentCompanyID = "";
            CurrentCurrency = "";
            CurrentCustomerID = "";
            $('#PoTypeVal').removeAttr("disabled");
            $('#PoCurrencyVal').removeAttr("disabled");
            //TableUpload.Search();
        }
    });

    $('#slSearchBapsType').on('change', function () {
        Form.InflationInput();
    });

    $("#btEdit").unbind().click(function () {
        Form.UpdatePODone();
    });
});

var Form = {
    Init: function () {

        if (!$("#hdAllowProcess").val()) {
            //$("#btProcess").hide();
            //$("#btReject").hide();
            //$("#btReceive").hide();
        }
        $('.panelDetailPO').hide();
        $(".panelSearchZero").hide();
        //Control.BindingSelectOperators($('#slSearchOperator'));
        //Control.BindingSelectOperators($('#slSearchOperator2'));
        Control.BindingSelectCompany($("#slSearchCompanyName"));
        Control.BindingSelectCompany($("#slSearchCompanyName2"));
        Control.BindingSelectRegional();
        $("#tabTrx").tabs();
        $("#tabPO").tabs();
        TableData.Init();
        $('#slSearchOperator2').select2({ placeholder: "Select Customer", width: null });
        
        //$(".panelSearchResult").hide();
    },
    GetBOQ: function () {
        if ($("#slSearchOperator2").val() != null && $("#slSearchOperator2").val() != "" && $("#slSearchOperator2").val().trim() == 'TSEL') {
            Control.BindingSelectGetBOQList();
            $('#BOQList').attr('style', 'display:block');
            $("#slRegional").val("").trigger('change');
            $('#slRegional').attr('disabled', 'disabled');
        }
        else {
            $('#BOQList').attr('style', 'display:none');
            $('#slRegional').prop('disabled', false);
        }

        //if (Operator != 'TSEL') {
        //    $("#RegionalRow").show();
        //}
        //else {
        //    $("#RegionalRow").hide();
        //}
    },
    DoneInput: function () {
        $("#pnlSummary").fadeIn();
        $('#mdlToInput').modal('toggle');
        TableProcess.Search();
    },
    BackToList: function () {
        $('.panelDetailPO').hide();
        $(".panelSearchZero").hide();
        $(".panelSearchBegin").show();
        $(".panelFilter").show();
        $(".panelSearchResult").show();
    },
    GetDetailPO: function () {
        var tblProcessDetail = $('#tblProcessDetail').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $('#POAmount').text('0');
        $('#TotalTenant').text('0');

        var BoqID = $('#slBOQ').val() == null ? "" : $('#slBOQ').val();
        var CompanyId = $('#slSearchCompanyName2').val().trim();
        var Operator = $('#slSearchOperator2').val().trim();
        var Regional = $('#slRegional').val().trim();

        if (CompanyId == "") {
            Common.Alert.Warning("Please Select Company")
            return;
        }

        if (Operator == "TSEL") {
            if (BoqID == "") {
                Common.Alert.Warning("Please Select One or More Boq")
                return;
            }
        }

        if (Operator == "") {
            Common.Alert.Warning("Please Select Customer")
            return;
        }

        TableProcessDetail.SearchProcessDetail();
    },
    GetCountRows: function () {
        $('#TotalTenant').text(Data.SiteRow.length);
    },
    DoneSaveDetail: function(){
        $('.panelDetailPO').hide();
        $(".panelSearchZero").hide();
        $(".panelSearchBegin").show();
        $(".panelFilter").show();
        $(".panelSearchResult").show();
        Form.ClearInputDetail();
        TableRaw.Search();
        $("#btProcess").fadeIn();
    },
    ClearInputDetail: function(){
        $('.panelDetailPO').find('input:text').val('');
        $('.panelDetailPO').find('select').val('');
        $('.panelDetailPO').find('textarea').val('');
        $('.panelDetailPO').find('input:file').val('');
        var tblProcessDetail = $('#tblProcessDetail').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });
        $("#slSearchCompanyName2").val("").trigger('change');
        $("#slSearchOperator2").val("").trigger('change');
        $("#slRegional").val("").trigger('change');
        $("#slBOQ").val("");
        $("#POAmount").text('');
        $("#TotalTenant").text('');
    },
    RefreshBOQSelect: function(){
        var BoqList = Data.Selected.mstRABoqID.split(',');
        var boqselect = $("#slBOQ").select2();
        //boqselect.val(BoqList).trigger({ type: 'select2:select', params: { data: BoqList } });
        $('#slBOQ').select2('destroy');
        $('#slBOQ').val(null).trigger('change');
        //$('#slBOQ').select2('data', null);
        var CompanyId = $('#slSearchCompanyName2').val().trim();
        var Operator = $('#slSearchOperator2').val().trim();
        var Regional = $('#slRegional').val().trim();

        var params = { strCompanyId: CompanyId, strOperator: Operator, strRegional: Regional }
        //$("#slBOQ").select2({}).select2('val', BoqList);

        // Fetch the preselected item, and add to the control
        var studentSelect = $('#slBOQ');
        $.ajax({
            url: "/api/POInput/GetBOQList",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
        }).then(function (data) {
            // create the option and append to Select2
            $.each(data.data, function (i, item) {
                var option = new Option(item.BOQNumber, item.ID.toString(), true, true);

                if (Helper.IsElementExistsInArray(item.ID.toString(), BoqList)) {
                    studentSelect.append(option).trigger('change');
                    studentSelect.trigger({
                        type: 'select2:select',
                        params: {
                            data: item
                        }
                    });
                }
            })
            // manually trigger the `select2:select` event
        });
        $("#slBOQ").select2();
    },
    SavePO: function () {
        //e.preventDefault();
        var oTable = $('#tblProcessDetail').DataTable();
        var rowcollection = oTable.$(".checkboxes:checked", { "page": "all" });
        if (rowcollection.length < 1) {
            //return false;
            Common.Alert.Warning("Can`t Create PO without detail data !"); return;
        }
        Data.SiteRow = [];
        var l = Ladda.create(document.querySelector("#btSaveData"))
        var formData = new FormData();

        var BoqID = $('#slBOQ').val() == null ? "" : $('#slBOQ').val();
        formData.append("mstRABoqID", BoqID);

        var CompanyId = $('#slSearchCompanyName2').val().trim();
        formData.append("CompanyId", CompanyId);

        var Operator = $('#slSearchOperator2').val().trim();
        formData.append("CustomerID", Operator);

        var Regional = $('#slRegional').val().trim();
        formData.append("Regional", Regional);

        var Currency = $('#slCurrency').val().trim();
        formData.append("Currency", Currency);

        var POType = $('#slPOType').val().trim();
        formData.append("POType", POType);

        var Remarks = $('#Remarks').val();
        formData.append("Remarks", Remarks);

        var PONumber = $('#PONumber').val().trim();
        formData.append("PONumber", PONumber);

        var PODate = $('#PODate').val().trim();
        formData.append("PODate", PODate);

        var POReceiveDate = $('#POReceived').val().trim();
        formData.append("POReceiveDate", POReceiveDate);

        var StartPeriod = $('#POPeriodeStart').val().trim();
        formData.append("StartPeriod", StartPeriod);

        var EndPeriod = $('#POPeriodeEnd').val().trim();
        formData.append("EndPeriod", EndPeriod);

        var POAmount = $('#POAmount').text().replace(/,/g, "");
        formData.append("POAmount", POAmount);

        var TotalTenant = $('#TotalTenant').text().trim();
        formData.append("TotalTenant", TotalTenant);

        var mstRAActivityID = 7;
        formData.append("mstRAActivityID", mstRAActivityID);

        //var oTable = $('#tblProcessDetail').DataTable();
        //var rowcollection = oTable.$(".checkboxes:checked", { "page": "all" });

        rowcollection.each(function (index, elem) {
            var checkbox_value = $(elem).val();
            
            if (!Helper.IsElementExistsInArray(checkbox_value, Data.SiteRow)) {
                Data.SiteRow.push(parseInt(checkbox_value));
            }
            //Do something with 'checkbox_value'
        });

        var ListID = Data.SiteRow.toString();
        var lengthid = ListID.split(',');

        if (lengthid.length == 1)
            ListID = ListID + ',0';

        formData.append("ListID", ListID);

        var fileInput = document.getElementById("postedFile1");
        if (document.getElementById("postedFile1").files.length != 0) {

            fsFileName = fileInput.files[0].name;
            formData.append("FileName", fsFileName);

            fsFile = fileInput.files[0];

            fsExtension = fsFileName.split('.').pop().toUpperCase();

            if ((fsFile.size/1024) > 2048) {
                Common.Alert.Warning("Upload File Can`t bigger then 2048 bytes (2mb)."); return;
            }

            if (fsExtension != "XLS" && fsExtension != "XLSX" && fsExtension != "PDF") {
                Common.Alert.Warning("Please upload an Excel or PDF File."); return;
            }
            else {

                formData.append('FilePO', fsFile);
            }

            errors = false;
        }
        else {
            Common.Alert.Warning("Please Select Document."); return;
        }

        $.ajax({
            url: '/api/POInput/SaveData',
            type: 'POST',
            data: formData,
            async: false,
            beforeSend: function (xhr) {
                l.start();
            },
            cache: false,
            contentType: false,
            processData: false
        }).done(function (data, textStatus, jqXHR) {
            if (data != "0") {
                if (data !== "Exception") {
                    if (data.length <= 0) {
                        $('.panelDetailPO').find('input:file').val('');
                        Common.Alert.Warning("The uploaded file is invalid or empty. Please use the available template, and fill the file correctly with valid product data.");
                    } else {
                        $('.panelDetailPO').find('input:file').val('');
                        Common.Alert.Success("Upload File Success!");
                        $('#file-upload-filename').text("");
                        $('#POAmount').text("");
                        $('#TotalTenant').text("");
                        Form.DoneSaveDetail();
                    }
                } else {
                    $('.panelDetailPO').find('input:file').val('');
                    Common.Alert.Warning("The uploaded file is invalid. Please download the latest template by clicking the Download Template button.");
                }
            } else {
                Common.Alert.Warning("Failed to save PO, Please Contact System Administrator !");
            }
            
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        });
    },
    UpdateNextActivity: function (ID,Activity) {
        var params = { trxRAPurchaseOrderID: ID, Activity: Activity }

        $.ajax({
            url: "/api/POInput/UpdateNextActivity",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
        })
        .done(function (data, textStatus, jqXHR) {
            $('#mdlNextActivity').modal('hide');
            if (data != 0)
            {
                Common.Alert.Success("Success To Process Data!");
                if (CurrentTab == 7)
                    TableRaw.Search();
                else
                    TableProcess.Search();

                Data.RowSelectedRaw = [];
                Data.RowSelectedProcess = [];
            }
            else
            {
                Common.Alert.Warning("Something Wrong, Please Contact System Support!");
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    SplitPO: function (e) {
        var SplitValue = e.value;
        var BillType = "";

        if (SplitValue == 1) {
            $('#fmBill').show();
            BillType = $('#slBillType').val();
        }
        else {
            $('#fmBill').hide();
            $('#slBillType').val(" ");
        }
        table = $('#tblDataSelected').DataTable();
        var Total = 0;
        var Tenant = 0;
        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            Tenant += 1;
            var data = this.data();
            if (SplitValue == 1) {
                if (BillType == "BaseLease") {
                    Total += parseFloat(data.BaseLeasePrice);
                }
                else if (BillType == "Service") {
                    Total += parseFloat(data.ServicePrice);
                }
                else if (BillType == "Inflation") {
                    JmlBln = Form.monthDiff(data.StartInvoiceDate, data.EndInvoiceDate);
                    Total += parseFloat(data.InflationAmount * JmlBln);
                }
            }
            else {
                if ($('#PoCurrencyVal').val() == "IDR")
                    Total += parseFloat(data.AmountIDR);
                else
                    Total += parseFloat(data.AmountUSD);
            }
        });

        $('#PoAmountVal').val(Common.Format.CommaSeparation(Total));
        $("#PoTenantVal").val(Tenant);
    },
    monthDiff: function(d1, d2) {
        var months;
        start = new Date(d1);
        end = new Date(d2);
        months = end.getMonth() - start.getMonth() + 1;
        //months = (end.getFullYear() - start.getFullYear()) * 12;
        //months -= start.getMonth() + 1;
        //months += end.getMonth();
        return months <= 0 ? 0 : months;
    },
    CalculateAmountPO: function () {
        SplitValue = $('#slSplitPO').val();
        BillType = $('#slBillType').val();
        table = $('#tblDataSelected').DataTable();
        var Total = 0;
        var Tenant = 0;
        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            Tenant += 1;
            var data = this.data();
            if (SplitValue == 1) {
                if (BillType == "BaseLease") {
                    Total += parseFloat(data.BaseLeasePrice);
                }
                else if (BillType == "Service") {
                    Total += parseFloat(data.ServicePrice);
                }
                else if (BillType == "Inflation") {
                    JmlBln = Form.monthDiff(data.StartInvoiceDate, data.EndInvoiceDate);
                    Total += parseFloat(data.InflationAmount * JmlBln);
                    //Total += parseFloat(data.InflationAmount);
                }
            }
            else {
                if ($('#PoCurrencyVal').val() == "IDR")
                    Total += parseFloat(data.AmountIDR);
                else
                    Total += parseFloat(data.AmountUSD);
            }
        });

        $('#PoAmountVal').val(Common.Format.CommaSeparation(Total));
        $("#PoTenantVal").val(Tenant);
    },
    InflationInput: function () {
        var typebaps = $('#slSearchBapsType').val();
        if (typebaps == "INF") {
            $('#slSearchCurrency').attr("disabled", "disabled");
            $('#PoCurrencyVal').attr("disabled", "disabled");
            $('#slSplitPO').attr("disabled", "disabled");
            $('#slBillType').attr("disabled", "disabled");
            $('#PoCurrencyVal').val("IDR");
            $('#slSearchCurrency').val("IDR");
            $('#slSplitPO').val("1");
            $('#slBillType').val("Inflation");
            $('#fmBill').show();
        } else {
            $('#slSearchCurrency').removeAttr("disabled");
            $('#PoCurrencyVal').removeAttr("disabled");
            $('#slSplitPO').removeAttr("disabled");
            $('#slBillType').removeAttr("disabled");
            $('#fmBill').hide();
        }
    },
    UpdatePODone: function () {       
        let updateDocument = $("#fileDocument")[0].files[0];
        if (updateDocument != undefined) {
            let extension = updateDocument.name.split('.').pop().toUpperCase();
            if ((updateDocument.size / 1024) > 2048) {
                //$(".sweet-alert").css("z-index", "100000");
                Common.Alert.Warning("Upload File Can`t bigger then 2048 bytes (2mb).");
                $('#mdlDetail').modal('hide');
                return;
            } else if (extension != "XLS" && extension != "XLSX" && extension != "PDF") {
                //$(".sweet-alert").css("z-index", "100000");
                Common.Alert.Warning("Please upload an Excel or PDF File.");
                $('#mdlDetail').modal('hide');
                return;
            }
        }
        //$(".sweet-alert").css("z-index", "1000");

        let l = Ladda.create(document.querySelector('#btEdit'));
        l.start();

        let updateData = new FormData();
        updateData.append("ID", Data.Selected.ID)
        updateData.append("PONumber", $("#tbDetailPONumber").val());
        updateData.append("UploadDocument", updateDocument);

        $.ajax({
            url: "/api/POInput/UpdatePODone",
            type: "POST", 
            contentType: false,  
            processData: false,
            data: updateData,
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {

                    Common.Alert.Success("Data successfully updated!");
                    $('#mdlDetail').modal('hide');
                    TableProcess.Search();
                } else {
                    Common.Alert.Warning(data.ErrorMessage);
                }
            } else if (data.GenericError != undefined) {
                Common.Alert.Error(data.GenericError)
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        });
    }
}

var TableProcess = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblProcess = $('#tblProcess').dataTable({
            "filter": false,
            "destroy": true,
            "orderCellsTop": true,
            "data": []
        });

        //$('#tblProcess thead tr:eq(1) th').each(function () {
        //    var title = $('#tblProcess thead tr:eq(0) th').eq($(this).index()).text();
        //    $(this).html('<input type="text" placeholder="Search ' + title + '" />');
        //});

        //tblProcess.columns().every(function (index) {
        //    $('#tblProcess thead tr:eq(1) th:eq(' + index + ') input').on('keyup change', function () {
        //        tblProcess.column($(this).parent().index() + ':visible')
        //            .search(this.value)
        //            .draw();
        //    });
        //});

        $("#tblProcess tbody").on("click", "a.btDetail2", function (e) {
            e.preventDefault();
            var table = $("#tblProcess").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                $('#mdlDetail').modal('toggle');
                TableProcess.ShowDetail();
            }
        });

        $(window).resize(function () {
            $("#tblProcess").DataTable().columns.adjust().draw();
        });
    },

    Search: function () {
        if ($("#formSearchDone").parsley().validate()) {
            var l = Ladda.create(document.querySelector("#btSearch"))
            l.start();
            fsCompanyIds = $("#slSearchCompanyDone").val() == null ? "" : $("#slSearchCompanyDone").val();
            fsOperators = $("#slSearchCustomerDone").val() == null ? "" : $("#slSearchCustomerDone").val();
            var fsGroupBy = $("#slSearchTypeDone").val() == null ? "" : $("#slSearchTypeDone").val();
            var fsCurrency = $("#slSearchCurrencyDone").val() == null ? "" : $("#slSearchCurrencyDone").val();

            var params = {
                strCompanyId: fsCompanyIds,
                strOperator: fsOperators,
                strGroupBy: fsGroupBy,
                isRaw: CurrentTab,
                strCurrency: fsCurrency
            };
            var tblProcess = $("#tblProcess").DataTable({
                "deferRender": true,
                "proccessing": true,
                "serverSide": true,
                "language": {
                    "emptyTable": "No data available in table"
                },
                "ajax": {
                    "url": "/api/POInput/grid",
                    "type": "POST",
                    "datatype": "json",
                    "data": params,
                },
                buttons: [
                    { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                    { extend: 'excel', className: 'btn yellow btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="a fa-file-excel-o" title="Export Excel"></i>', titleAttr: 'Export Excel' },
                    { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
                ],
                "filter": false,
                "lengthMenu": [[5, 10, 25, 50, 100, 200], ['5', '10', '25', '50', '100', '200']],
                "destroy": true,
                "columns": [
                    {
                        orderable: false,
                        mRender: function (data, type, full) {
                            var strReturn = "";
                            {
                                if (Helper.IsElementExistsInArray(full.ID, Data.RowSelectedRaw)) {
                                    $("#Row" + full.ID).addClass("active");
                                    strReturn += "<label id='" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.ID + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                }
                                else {
                                    strReturn += "<label id='" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.ID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                                }

                                return strReturn;
                            }
                        }
                    },
                    {
                        orderable: false,
                        mRender: function (data, type, full) {
                            if (full.FilePath != null && full.FilePath != "") {
                                var FileNames = full.FilePath.split('\\');
                                var Name = FileNames[3].toString();
                                return "<a class='files' target='_blank' href='/RevenueAssurance/Download?FilePath=" + full.FilePath + "&FileName=" + Name + "&PONumber=" + full.PONumber + "&ContentType=" + full.ContentType + "'><i class='fa fa-download'></i></a>";
                            }
                            else
                                return "";
                        }
                    },
                    //{
                    //    data: "FilePath", mRender: function (data, type, full) {
                    //        return "<a class='files' target='_blank' href='" + data + "'><i class='fa fa-download'></i></a>";
                    //    }
                    //},
                    {
                        data: "PONumber", mRender: function (data, type, full) {
                            return "<a class='btDetail2'>" + data + "</a>";
                        }
                    },
                    //{ data: "PONumber" },
                    { data: "CompanyID" },
                    { data: "CustomerID" },
                    { data: "Regional" },
                    {
                        data: "PODate", render: function (data) {
                            return Common.Format.ConvertJSONDateTime(data);
                        }
                    },
                    {
                        data: "POReceiveDate", render: function (data) {
                            return Common.Format.ConvertJSONDateTime(data);
                        }
                    },
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
                    { data: "POType" },
                    { data: "TotalTenant" },
                    { data: "Currency" },
                    { data: "POAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { data: "Remarks" },
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

                    if (Data.RowSelectedProcess.length > 0) {
                        var item;
                        for (var i = 0 ; i < Data.RowSelectedProcess.length; i++) {
                            item = Data.RowSelectedProcess[i];
                            $("#Row" + item).addClass("active");
                        }
                    }
                },
                "order": [],
                "orderCellsTop": true,
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
                "initComplete": function () {
                    /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                    var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                        '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tblProcess .checkboxes" />' +
                        '<span></span> ' +
                        '</label>';
                    var th = $("th.select-all-process").html(checkbox);

                    /* Bind Event to Select All Checkbox in the Cloned Table */
                    $("th.select-all-process").unbind().on("change", ".group-checkable", function (e) {
                        var set = $(".panelSearchResult .DTFC_LeftBodyWrapper .DTFC_Cloned .checkboxes");
                        var checked = jQuery(this).is(":checked");
                        jQuery(set).each(function () {
                            label = $(this).parent();
                            var id = label.attr("id");
                            if (label.attr("style") != "display:none") {
                                /* Replace the following code with the code to select all checkboxes in all pages */
                                if (checked) {
                                    $(this).prop("checked", true);
                                    $(this).parents('tr').addClass('active');
                                    $(this).trigger("change");

                                    $(".Row" + id).addClass("active");
                                    $("." + id).prop("checked", true);
                                    $("." + id).trigger("change");
                                } else {
                                    $(this).prop("checked", false);
                                    $(this).parents('tr').removeClass("active");
                                    $(this).trigger("change");

                                    $(".Row" + id).removeClass("active");
                                    $("." + id).prop("checked", false);
                                    $("." + id).trigger("change");
                                }
                            }
                        });
                        if (checked)
                            Data.RowSelectedProcess = Helper.GetListId(1);
                        else
                            Data.RowSelectedProcess = [];
                    });
                }
            });
        }
        
    },

    Reset: function () {
        $("#slSearchCompanyDone").val(null).trigger("change");
        $("#slSearchCustomerDone").val(null).trigger("change");
        $("#slSearchTypeDone").val(null).trigger("change");
        $("#slSearchCurrencyDone").val(null).trigger("change");
    },
    ShowDetail: function () {
        $('#tbDetailID').val(Data.Selected.ID);
        $('#tbDetailPONumber').val(Data.Selected.PONumber);
        $('#tbDetailPOType').val(Data.Selected.POType);
        $('#tbDetailCurrency').val(Data.Selected.Currency);
        $('#tbDetailStartPeriod').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartPeriod));
        $('#tbDetailEndPeriod').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndPeriod));
        $('#tbDetailPODate').val(Common.Format.ConvertJSONDateTime(Data.Selected.PODate));
        $('#tbDetailPOReceiveDate').val(Common.Format.ConvertJSONDateTime(Data.Selected.POReceiveDate));
        $('#tbDetailCompany').val(Data.Selected.CompanyID);
        $('#tbDetailCustomer').val(Data.Selected.CustomerID);
        $('#tbDetailRegional').val(Data.Selected.Regional);
        $('#tbDetailRemarks').val(Data.Selected.Remarks);
        $('#tbDetailTotalTenant').text(Data.Selected.TotalTenant);

        $('#rowdtlBOQ').hide();
        if (Data.Selected.CustomerID.trim() == "TSEL") {
            Control.BindingBOQDetail(Data.Selected.mstRABoqID);
            $('#rowdtlBOQ').show();

            $("#tbDetailPONumber").attr("readonly", false);
            $("#formFileDocuemnt").show();
            $("#fileDocument").val("")
            $("#btEdit").show();
        }

        TableSiteInfo.Search(Data.Selected.ID);
    },
    Export: function () {}
}

var TableRaw = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();

        var tblRaw = $('#tblRaw').DataTable({
            "filter": false,
            "destroy": true,
            "orderCellsTop": true,
            "data": []
        });

        

        $("#tblRaw tbody").on("click", "button.btEdit", function (e) {
            e.preventDefault();
            var table = $("#tblRaw").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                $('.panelDetailPO').show();
                $(".panelSearchZero").hide();
                $(".panelSearchBegin").hide();
                $(".panelFilter").hide();
                $(".panelSearchResult").hide();
                $("#btSaveData").hide();
                $("#btUpdateData").show();
                $("#btBoqSelect").show();
                //$('#mdlDetail').modal('toggle');
                TableRaw.EditDetail();
                //$('#btBoqSelect').on('click', function () {
                //    var BoqList = Data.Selected.mstRABoqID.split(',');
                //    var $exampleMulti = $("#slBOQ").select2();
                //    $exampleMulti.val(BoqList).trigger("change");
                //});
            }
        });

        $("#tblRaw tbody").on("click", "a.btDetail", function (e) {
            e.preventDefault();
            var table = $("#tblRaw").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                $('#mdlDetail').modal('toggle');
                TableRaw.ShowDetail();
            }
        });

        

        $(window).resize(function () {
            $("#tblRaw").DataTable().columns.adjust().draw();
        });
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        var fsGroupBy = $("#slGroupBy").val() == null ? "" : $("#slGroupBy").val();

        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strGroupBy: fsGroupBy,
            isRaw: CurrentTab
        };
        var tblRaw = $("#tblRaw").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/POInput/grid",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                { extend: 'excel', className: 'btn yellow btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="a fa-file-excel-o" title="Export Excel"></i>', titleAttr: 'Export Excel' },
                //{
                //    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                //        var l = Ladda.create(document.querySelector(".yellow"));
                //        l.start();
                //        TableRaw.Export()
                //        l.stop();
                //    }
                //},
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50,100,200], ['5', '10', '25', '50','100','200']],
            "destroy": true,
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Helper.IsElementExistsInArray(full.ID, Data.RowSelectedRaw)) {
                                $("#Row" + full.ID).addClass("active");
                                strReturn += "<label id='" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.ID + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                            }
                            else {
                                strReturn += "<label id='" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.ID + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                },
                //{
                //    mRender: function (data, type, full) {
                //        var strReturn = "<button type='button' title='Edit' class='btn btn-xs green btEdit'><i class='fa fa-edit'></i></button>";
                //        return strReturn;
                //    }
                //},
                //{ data: "PONumber" },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        return "<a class='files' target='_blank' href='/RevenueAssurance/Download?FilePath=" + full.FilePath + "&FileName=" + full.FileName + "&PONumber=" + full.PONumber + "&ContentType=" + full.ContentType + "'><i class='fa fa-download'></i></a>";
                    }
                },
                {
                    data: "PONumber", mRender: function (data, type, full) {
                        return "<a class='btDetail'>" + data + "</a>";
                    }
                },
                { data: "CompanyID" },
                { data: "CustomerID" },
                { data: "Regional" },
                {
                    data: "PODate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "POReceiveDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
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
                { data: "POType" },
                { data: "TotalTenant" },
                { data: "Currency" },
                { data: "POAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Remarks" },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblRaw.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelectedRaw.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelectedRaw.length; i++) {
                        item = Data.RowSelectedRaw[i];
                        $("#Row" + item).addClass("active");
                    }
                }
            },
            /*fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (aData.IsLossPPN == 1) {
                    if (aData.AmountPPN != aData.AmountLossPPN) {
                        $('td', nRow).css('background-color', '#FF9999');
                    }
                }
                l.stop();
            },*/
            "order": [],
            "orderCellsTop": true,
            "scrollY": 300, /* Enable vertical scroll to allow fixed columns */
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 3 /* Set the 2 most left columns as fixed columns */
            },
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                    '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tblRaw .checkboxes" />' +
                    '<span></span> ' +
                    '</label>';
                var th = $("th.select-all-raw").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-raw").unbind().on("change", ".group-checkable", function (e) {
                    var set = $(".panelSearchResult .DTFC_LeftBodyWrapper .DTFC_Cloned .checkboxes");
                    var checked = jQuery(this).is(":checked");
                    jQuery(set).each(function () {
                        label = $(this).parent();
                        var id = label.attr("id");
                        if (label.attr("style") != "display:none") {
                            /* Replace the following code with the code to select all checkboxes in all pages */
                            if (checked) {
                                $(this).prop("checked", true);
                                $(this).parents('tr').addClass('active');
                                $(this).trigger("change");

                                $(".Row" + id).addClass("active");
                                $("." + id).prop("checked", true);
                                $("." + id).trigger("change");
                            } else {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");
                                $(this).trigger("change");

                                $(".Row" + id).removeClass("active");
                                $("." + id).prop("checked", false);
                                $("." + id).trigger("change");
                            }
                        }
                    });
                    if (checked)
                        Data.RowSelectedRaw = Helper.GetListId(0);
                    else
                        Data.RowSelectedRaw = [];
                });
            }
        });
    },
    ShowDetail: function () {
        $('#tbDetailID').val(Data.Selected.ID);
        $('#tbDetailPONumber').val(Data.Selected.PONumber);
        $('#tbDetailPOType').val(Data.Selected.POType);
        $('#tbDetailCurrency').val(Data.Selected.Currency);
        $('#tbDetailStartPeriod').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartPeriod));
        $('#tbDetailEndPeriod').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndPeriod));
        $('#tbDetailPODate').val(Common.Format.ConvertJSONDateTime(Data.Selected.PODate));
        $('#tbDetailPOReceiveDate').val(Common.Format.ConvertJSONDateTime(Data.Selected.POReceiveDate));
        $('#tbDetailCompany').val(Data.Selected.CompanyID);
        $('#tbDetailCustomer').val(Data.Selected.CustomerID);
        $('#tbDetailRegional').val(Data.Selected.Regional);
        $('#tbDetailRemarks').val(Data.Selected.Remarks);
        $('#tbDetailTotalTenant').text(Data.Selected.TotalTenant);
        //$('#tbDetailPOAmount').val(Data.Selected.Remarks);

        $('#rowdtlBOQ').hide();
        if (Data.Selected.CustomerID.trim() == "TSEL") {
            Control.BindingBOQDetail(Data.Selected.mstRABoqID);
            $('#rowdtlBOQ').show();
        }

        TableSiteInfo.Search(Data.Selected.ID);
    },
    Export: function () { },
    Import: function () { $('#mdlBulky').modal('show'); },
    EditDetail: function () {
        $('#slSearchCompanyName2').val(Data.Selected.CompanyID);
        $('#slSearchOperator2').val(Data.Selected.CustomerID);
        $('#slRegional').val(Data.Selected.Regional);
        $('#slCurrency').val(Data.Selected.Currency);
        $('#slPOType').val(Data.Selected.POType);
        $('#Remarks').val(Data.Selected.Remarks);
        $('#PONumber').val(Data.Selected.PONumber);
        $('#PODate').val(Common.Format.ConvertJSONDateTime(Data.Selected.PODate));
        $('#POReceived').val(Common.Format.ConvertJSONDateTime(Data.Selected.POReceiveDate));
        $('#POPeriodeStart').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartPeriod));
        $('#POPeriodeEnd').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndPeriod));
        $('#POAmount').text(Common.Format.CommaSeparation(Data.Selected.POAmount));
        $('#TotalTenant').text(Data.Selected.TotalTenant);
        $('#slSearchCompanyName2').trigger('change');
        $('#slSearchOperator2').trigger('change');
        $('#slRegional').trigger('change');
        PurchaseOrderID = Data.Selected.ID.toString();

        if (Data.Selected.CustomerID == "TSEL") {
            $('#slRegional').attr('disabled', 'disabled');
            $('#BOQList').attr('style', 'display:block');
            //var BoqList = Data.Selected.mstRABoqID.split(',');
            //var $exampleMulti = $("#slBOQ").select2();
            //$exampleMulti.val(BoqList).trigger("change");
            //$("#slBOQ").select2({ multiple: true });
            //$('#slBOQ').select2('destroy');
            //$('#slBOQ').select2('destroy').select2();
            //$("#slBOQ").html('');
            //$("#slBOQ").select2({ multiple: true });
            //$("#slBOQ").val(BoqList).trigger("change");
            //Control.BindingSelectGetBOQEditList(BoqList);
            //Control.BindingSelectGetBOQList();
            //$("#slBOQ").select2({}).select2('val', BoqList);
            //if (BoqList.length > 1) {
            //    for (var i = 0; i < BoqList.length; i++) {
            //        $("#slBOQ").append("<option value='" + BoqList[i] + "' selected>" + BoqList[i] + "</option>");
            //    }
            //    //$('#slBOQ').val(BoqList);
            //    //$('#slBOQ').trigger('change');
            //}
            //else {
            //    $("#slBOQ").append("<option value='" + Data.Selected.mstRABoqID + "' selected>" + Data.Selected.mstRABoqID + "</option>");
            //    //$('#slBOQ').val(BoqList);
            //    //$('#slBOQ').trigger('change');
            //}
            //$('#slBOQ').trigger('change');
            //$("#slBOQ").select2({ placeholder: "Select BOQ", width: null });
            //$("#slSearchCompanyName2").select2({ placeholder: "Select Company", width: null });
            //$("#slSearchOperator2").select2({ placeholder: "Select Customer", width: null });
            //$("#slRegional").select2({ placeholder: "Select Regional", width: null });
            
            //Form.GetDetailPO();
            //$("#btBoqSelect").click();
           //Form.RefreshBOQSelect();
            //var btBoqSelect = document.getElementById("btBoqSelect");
            
        }
        //$("#slBOQ").select2().select2('val', ['14', '15'])
        //$("#slBOQ").select2().val(['14', '15']).trigger('change')

        //TableProcessDetail.SearchProcessDetail();
        //btBoqSelect.click();
    }
}

var TableSiteInfo = {
    Init: function () {
        var tblRaw = $('#tblSiteInfo').DataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblSiteInfo").DataTable().columns.adjust().draw();
        });
    },

    Search: function (PurchaseOrderID) {

        var params = { trxRAPurchaseOrderID: PurchaseOrderID }

        var tblSiteInfo = $("#tblSiteInfo").DataTable({
            "scrollY": 300,
            searching: false,
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
            "scroller": true,
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/POInput/GetDetail",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            "lengthMenu": [[-1], ['All']],
            "columns": [
                { data: "BOQNumber" },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },

                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "RegionalName" },
                { data: "ProvinceName" },

                { data: "ResidenceName" },
                { data: "PONumber" },
                { data: "MLANumber" },
                {
                    data: "StartBapsDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndBapsDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "Term" },
                { data: "BapsType" },
                { data: "CustomerInvoice" },
                { data: "CustomerID" },
                { data: "CompanyInvoice" },
                { data: "Company" },
                { data: "StipSiro" },
                { data: "InvoiceTypeName" },
                { data: "Currency" },
                {
                    data: "StartInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "TotalPaymentRupiah", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
            ],
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    extend: 'excelHtml5',
                    filename: 'Site Detail PO',
                    text: '<i class="fa fa-file-excel-o"></i>',
                    titleAttr: 'Export to Excel',
                    className: 'btn yellow btn-outline',
                    exportOptions: {
                        columns: 'th:not(:first-child)',
                        format: {
                            body: function (data, row, column, node) {
                                return (column <= 4) ? "\0" + data : data;
                            }
                        }
                    },
                    rows: ':visible'
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "destroy": true,
            "fnDrawCallback": function (oSettings, json) {
                if (Data.Selected.CustomerID.trim() == 'TSEL') {
                    //console.log(this.fnGetData());
                    //$.each(this.fnGetData(), function (key, value) {
                    //    Data.SiteRow.push(0);
                    //});
                    //var Data = this.fnGetData();
                    //for (var i = 0; i < this.fnGetData().length; i++) {
                    //    //Data.SiteRow.push();
                    //    Helper.InsertsElementInArray(Data[i]["Id"]);
                    //}
                    $('#TotalTenant').text(this.fnGetData().length);
                }
            },
            "footerCallback": function (row, data, start, end, display) {
                if (Data.Selected.CustomerID.trim() == 'TSEL') {
                    var api = this.api(), data;
                    var colNumber = [26];


                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };

                    var numformat = $.fn.dataTable.render.number(',', '.', 2, '').display;

                    for (i = 0; i < colNumber.length; i++) {

                        var colNo = colNumber[i];

                        TotalAmount = api
                                .column(colNo, { page: 'all' })
                                .data()
                                .reduce(function (a, b) {
                                    return intVal(a) + intVal(b);
                                }, 0);
                        $(api.column(colNo).footer()).html(numformat(TotalAmount));

                        $("#tbDetailPOAmounts").text(numformat(TotalAmount));
                    }
                }
            },
        });

        $(window).resize(function () {
            $("#tblSiteInfo").DataTable().columns.adjust().draw();
        });
    },
}

var TableProcessDetail = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblProcess = $('#tblProcessDetail').dataTable({
            "filter": false,
            "destroy": true,
            //"lengthMenu": [[-1], ['All']],
            "data": []
        });

        $(window).resize(function () {
            $("#tblProcessDetail").DataTable().columns.adjust().draw();
        });
    },
    SearchProcessDetail: function () {
        var l = Ladda.create(document.querySelector("#btSearchData"))
        //Get Data that in RowSelected
        //Data.POAmount = [];
        Data.SiteRow = [];
        var BoqID = $('#slBOQ').val() == null ? "" : $('#slBOQ').val();
        var CompanyId = $('#slSearchCompanyName2').val().trim();
        var Operator = $('#slSearchOperator2').val().trim();
        var Regional = $('#slRegional').val().trim();
        //var POAmount = 0;

        var params = { strCompanyId: CompanyId, strOperator: Operator, strRegional: Regional, strBoqID: BoqID.toString(), trxRAPurchaseOrderID: PurchaseOrderID }

        var tblProcessDetail = $("#tblProcessDetail").DataTable({
            "scrollY": 500,
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
            "scroller": true,
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/POInput/GetDetail",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            "lengthMenu": [[-1], ['All']],
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Operator == 'TSEL') {
                                
                                
                                $("#Row" + full.Id).addClass("active");
                                strReturn += "<label id='" + full.Id + "' name='" + full.TotalPaymentRupiah + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.Id + "' ><input value='" + full.Id + "' type='checkbox' checked class='checkboxes' disabled /><span></span></label>";
                                //POAmount = POAmount + parseFloat(full.amountRupiah);
                                //Data.POAmount.push(parseFloat(full.amountRupiah));
                            }
                            else {
                                if (Helper.IsElementExistsInArray(full.Id, Data.SiteRow)) {
                                    $("#Row" + full.Id).addClass("active");
                                    strReturn += "<label id='" + full.Id + "' name='" + full.TotalPaymentRupiah + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.Id + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                    //POAmount = POAmount + parseFloat(full.amountRupiah);
                                    //Data.POAmount.push(parseFloat(full.amountRupiah));
                                }
                                else {
                                    strReturn += "<label id='" + full.Id + "' name='" + full.TotalPaymentRupiah + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.Id + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                                }
                            }

                            return strReturn;
                        }
                    }
                },
                { data: "BOQNumber" },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },

                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "RegionalName" },
                { data: "ProvinceName" },

                { data: "ResidenceName" },
                { data: "PONumber" },
                { data: "MLANumber" },
                {
                    data: "StartBapsDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndBapsDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "Term" },
                { data: "BapsType" },
                { data: "CustomerInvoice" },
                { data: "CustomerID" },
                { data: "CompanyInvoice" },
                { data: "Company" },
                { data: "StipSiro" },
                { data: "InvoiceTypeName" },
                { data: "Currency" },
                {
                    data: "StartInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "TotalPaymentRupiah", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
            ],
            "buttons": [],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "destroy": true,
            "fnDrawCallback": function (oSettings, json) {
                if (Operator == 'TSEL') {
                    //console.log(this.fnGetData());
                    //$.each(this.fnGetData(), function (key, value) {
                    //    Data.SiteRow.push(0);
                    //});
                    //var Data = this.fnGetData();
                    //for (var i = 0; i < this.fnGetData().length; i++) {
                    //    //Data.SiteRow.push();
                    //    Helper.InsertsElementInArray(Data[i]["Id"]);
                    //}
                    $('#TotalTenant').text(this.fnGetData().length);
                }
            },
            "footerCallback": function (row, data, start, end, display) {
                if (Operator == 'TSEL') {
                    var api = this.api(), data;
                    var colNumber = [27];


                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };

                    var numformat = $.fn.dataTable.render.number(',', '.', 2, '').display;

                    for (i = 0; i < colNumber.length; i++) {

                        var colNo = colNumber[i];

                        TotalAmount = api
                                .column(colNo, { page: 'all' })
                                .data()
                                .reduce(function (a, b) {
                                    return intVal(a) + intVal(b);
                                }, 0);
                        $(api.column(colNo).footer()).html(numformat(TotalAmount));

                        $("#POAmount").text(numformat(TotalAmount));
                    }
                }
            },
        });

        $(window).resize(function () {
            $("#tblProcessDetail").DataTable().columns.adjust().draw();
        });

        //var set = jQuery(this).attr("data-set");
        //var checked = jQuery(this).is(":checked");
        //jQuery(set).each(function () {
        //    if (checked) {
        //        POAmount = parseFloat($(this).parent().attr('amountRupiah'));
        //        Data.POAmount.push(POAmount);
        //    }
        //});

        //console.log(Data.POAmount);

        //$('#POAmount').text(Common.Format.CommaSeparation(POAmount));
    },
}

var TableData = {
    Init: function () {
        var tblData = $('#tblData').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        var tblDataSelected = $('#tblDataSelected').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearchNon"))
        l.start();
        params = TableData.Param();

        var tblData = $("#tblData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/POInput/gridData",
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
                        TableData.Export();
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            //buttons: ['copy','csv','excel','pdf','print'],
            "filter": false,
            "lengthMenu": [[25, 50, 100, 1000], ['25', '50', '100', '1000']],
            "destroy": true,
            "columns": [

                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Helper.IsElementExistsInArray(parseInt(full.id), Data.RowDataSelected)) {
                                strReturn += "<label style='display:none;' id='" + full.id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.id + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                            } else {
                                if (Helper.IsElementExistsInArray(full.id, Data.RowSelectedData)) {
                                    $("#Row" + full.id).addClass("active");
                                    strReturn += "<label id='" + full.id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                }
                                else {
                                    strReturn += "<label id='" + full.id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' class='checkboxes' /><span></span></label>";
                                }
                            }

                            return strReturn;
                        }
                    }
                },

                { data: "SONumber" },
                { data: "CompanyInvoice" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "Term" },
                {
                    data: "StartInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },

                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InflationAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

                { data: "AdditionalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

                { data: "AmountIDR", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".pnlSearchNon", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".pnlSearchNon").fadeIn();
                }
                l.stop(); App.unblockUI('.pnlSearchNon');

                if (Data.RowSelectedData.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelectedData.length; i++) {
                        item = Data.RowSelectedData[i];
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
            "initComplete": function () {
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                    '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tblData .checkboxes" />' +
                    '<span></span> ' +
                    '</label>';
                var th = $("th.select-all-data").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-data").unbind().on("change", ".group-checkable", function (e) {
                    Data.RowSelectedData = Data.RowSelectedSite;
                    var set = $(".pnlSearchNon .DTFC_LeftBodyWrapper .DTFC_Cloned .checkboxes");
                    var checked = jQuery(this).is(":checked");
                    jQuery(set).each(function () {
                        label = $(this).parent();
                        var id = label.attr("id");
                        if (label.attr("style") != "display:none") {
                            /* Replace the following code with the code to select all checkboxes in all pages */
                            if (checked) {
                                $(this).prop("checked", true);
                                $(this).parents('tr').addClass('active');
                                $(this).trigger("change");

                                $(".Row" + id).addClass("active");
                                $("." + id).prop("checked", true);
                                $("." + id).trigger("change");
                                //Data.RowSelectedData.push(parseInt(id));
                            } else {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");
                                $(this).trigger("change");

                                $(".Row" + id).removeClass("active");
                                $("." + id).prop("checked", false);
                                $("." + id).trigger("change");
                            }
                        }
                    });
                    if (checked)
                        Data.RowSelectedData = TableData.GetListId();
                    else
                        Data.RowSelectedData = [];
                });
            }
        });
    },

    Param: function () {
        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsCustomer = $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val();
        fsStip = $("#slSearchStip").val() == null ? "" : $("#slSearchStip").val();
        fsQuarterly = $("#slSearchQuarterly").val() == null ? "" : $("#slSearchQuarterly").val();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
        fsSiteName = $("#tbSearchSiteName").val();
        fsProductType = $("#slSearchProductType").val() == null ? "" : $("#slSearchProductType").val();
        fsSONumber = $("#tbSearchSONumber").val();
        fsSiteID = $("#tbSearchSiteID").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsPowerType = $("#slSearchPowerType").val() == null ? "" : $("#slSearchPowerType").val();
        fsYear = $("#slSearchYear").val() == null ? "" : $("#slSearchYear").val();

        var params = {
            strCompanyId: fsCompanyId,
            strCustomerId: fsCustomer,
            strStipID: fsStip,
            strCurrency: fsCurrency,
            strSONumber: fsSONumber,
            strQuarterly: fsQuarterly,
            strYear: fsYear,
            strProduct: fsProductType,
            strBapsType: fsBapsType,
            strPowerType: fsPowerType,
            strSiteID: fsSiteID,
            strSiteName: fsSiteName
        };

        return params;
    },

    ResetFilter: function(){
        $("#slSearchCompany").val(null).trigger("change");
        $("#slSearchCustomer").val(null).trigger("change");
        $("#slSearchStip").val(null).trigger("change");
        $("#slSearchQuarterly").val(null).trigger("change");
        $("#slSearchCurrency").val("IDR").trigger("change");
        $("#tbSearchSiteName").val("");
        $("#slSearchProductType").val(null).trigger("change");
        $("#tbSearchSONumber").val("");
        $("#tbSearchSiteID").val("");
        $("#slSearchBapsType").val(null).trigger("change");
        $("#slSearchPowerType").val(null).trigger("change");
        $("#slSearchYear").val(null).trigger("change");
    },

    GetListId: function () {
        
        var params = TableData.Param();

        var AjaxData = [];
        $.ajax({
            url: "/api/POInput/GetDataListId",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                AjaxData = data;
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
        //Common.Alert.Error(AjaxData);
        return AjaxData;
    },

    AddSite: function () {
        if ($("#formSearchs").parsley().validate()) {
            var l = Ladda.create(document.querySelector("#btAddSite"))
            var oTable = $('#tblData').DataTable();
            var oTableSite = $('#tblDataSelected').DataTable();

            if (CurrentCompanyID == "") {
                CurrentCompanyID = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
                CurrentCustomerID = $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val();
                CurrentCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
                CurrentBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();

                $('#PoTypeVal').val(CurrentBapsType);
                $('#PoCurrencyVal').val(CurrentCurrency);
                $('#PoTypeVal').attr("disabled", "disabled");
                //$('#PoCurrencyVal').attr("disabled", "disabled");
            }
            else {
                fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
                fsCustomer = $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val();
                fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
                fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();

                if (CurrentCompanyID != fsCompanyId) {
                    Common.Alert.Warning("Different Company is not Allowed !")
                    return;
                }

                if (CurrentCustomerID != fsCustomer) {
                    Common.Alert.Warning("Different Customer is not Allowed !")
                    return;
                }

                if (CurrentCurrency != fsCurrency) {
                    Common.Alert.Warning("Different Currency is not Allowed !")
                    return;
                }

                if (CurrentBapsType != fsBapsType) {
                    Common.Alert.Warning("Different BapsType is not Allowed !")
                    return;
                }
            }

            if (Data.RowSelectedData.length == 0) {
                Common.Alert.Warning("Please Select One or More Data")
                return;
            }
            else {
                //insert Data.RowSelectedSite for rendering checkbox
                $.each(Data.RowSelectedData, function (index, item) {
                    if (!Helper.IsElementExistsInArray(item, Data.RowDataSelected)) {
                        Data.RowDataSelected.push(parseInt(item));
                    }
                });

                var filterid = Data.RowDataSelected.toString();
                TableData.GetSelectedSiteData(filterid);

                //Hide the checkboxes in the cloned table
                $.each(Data.RowSelectedData, function (index, item) {
                    $(".Row" + item).removeClass('active');
                    $("." + item).hide();
                });

                Data.RowSelectedData = [];
                TableData.Search();
            }
            $('#slSearchBapsType').attr("disabled", "disabled");
        }
    },

    SelectedSearch: function (Result) {
        

        if (Result.length > 0) {
            $('#tblDataSelected').show();
            $('#DetailNonTSEL').show();
        }
        else {
            $('#tblDataSelected').hide();
            $('#DetailNonTSEL').hide();
        }

        var tblSiteData = $("#tblDataSelected").DataTable({
            "scrollY": 500,
            searching: false,
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
            "scroller": true,
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "data": Result,
            "filter": false,
            "destroy": true,

            "columns": [
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<button type='button' title='Delete' class='btn btn-xs red btDeleteSite' id='btDeleteSite" + full.id + "'><i class='fa fa-trash'></i></button>";
                        return strReturn;
                    }
                },
                { data: "SONumber" },
                { data: "CompanyInvoice" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "Term" },
                {
                    data: "StartInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },

                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InflationAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

                { data: "AdditionalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

                { data: "AmountIDR", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
            ],
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    extend: 'excelHtml5',
                    filename: 'Site Detail PO',
                    text: '<i class="fa fa-file-excel-o"></i>',
                    titleAttr: 'Export to Excel',
                    className: 'btn yellow btn-outline',
                    exportOptions: {
                        columns: 'th:not(:first-child)',
                        format: {
                            body: function (data, row, column, node) {
                                return (column <= 4) ? "\0" + data : data;
                            }
                        }
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "footerCallback": function (row, data, start, end, display) {
                //var api = this.api(), data;
                ////var colNumber = [23];


                //var intVal = function (i) {
                //    return typeof i === 'string' ?
                //        i.replace(/[\$,]/g, '') * 1 :
                //        typeof i === 'number' ?
                //        i : 0;
                //};

                //var numformat = $.fn.dataTable.render.number(',', '.', 2, '').display;

                //fsTotalTenant = 0;
                //fsTotalAmount = 0;
                //var oTableSite = $('#tblDataSelected').DataTable();
                ////var Percent = $('input[name=rbPercent]:checked').val();

                //oTableSite.rows('').every(function (rowIdx, tableLoop, rowLoop) {
                //    fsTotalTenant += 1;
                //    fsTotalAmount += this.data().AmountIDR;
                //});

                //$(api.column(11).footer()).html(numformat(fsTotalAmount));

                //$("#PoTenantVal").val(fsTotalTenant);
                //$("#PoAmountVal").val(numformat(fsTotalAmount));
            },
            "initComplete": function (settings, json) {
                Form.CalculateAmountPO();
            }
        });

        

        $(window).resize(function () {
            $("#tblDataSelected").DataTable().columns.adjust().draw();
        });
    },

    GetSelectedSiteData: function (ListID) {

        var AjaxData = [];
        var params = {
            strFilterID: ListID,
            strBapsType : $('#slSearchBapsType').val()
        };
        var l = Ladda.create(document.querySelector("#btAddSite"));
        $.ajax({
            url: "/api/POInput/gridData",
            type: "POST",
            dataType: "json",
            data: params,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                TableData.SelectedSearch(data.data);
                l.stop();
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        });
        
    },

    ResetTrx: function () {
        Data.RowDataSelected = [];
        Data.RowSelectedData = [];
        $('#tblDataSelected').hide();
        $('#DetailNonTSEL').hide();
        $('#file-upload-filenames').text("");
        $('#PoAmountVal').val("");
        $('#PoTenantVal').val("");
        $('#PoNumberVal').val("");
        $('#PoDateVal').val("");
        $('#PoReceiveDateVal').val("");
        $('#tbStartPeriod').val("");
        $('#tbEndPeriod').val("");
        $('#tbRemarksVal').val("");
        TableData.Search();
        CurrentBapsType = "";
        CurrentCompanyID = "";
        CurrentCurrency = "";
        CurrentCustomerID = "";
        $('#PoTypeVal').removeAttr("disabled");
        $('#PoCurrencyVal').removeAttr("disabled");
        $('#slSearchBapsType').removeAttr("disabled");
        Form.InflationInput();
    },

    ValidatePO: function () {
        var l = Ladda.create(document.querySelector("#btnDetailSave"))
        if ($("#formDetailDataNon").parsley().validate()) {

            SplitValue = $('#slSplitPO').val();
            BillType = $('#slBillType').val();
            PoDetail = [];
            POValidate = [];

            if (SplitValue != "1") {
                BillType = "";
            }

            tabler = $('#tblDataSelected').DataTable();

            tabler.rows().every(function (rowIdx, tableLoop, rowLoop) {
                var objDetail = {
                    Amount: 0,
                    IsSplit: false,
                    Type: "",
                    trxReconcileID: 0
                }

                var objValidate = {
                    ValueId: 0,
                    ValueDesc: ""
                }

                var data = this.data();
                if (SplitValue == 1) {
                    if (BillType == "BaseLease") {
                        objDetail.Amount = parseFloat(data.BaseLeasePrice);
                    }
                    else if (BillType == "Service") {
                        objDetail.Amount = parseFloat(data.ServicePrice);
                    }
                    else if (BillType == "Inflation") {
                        JmlBln = Form.monthDiff(data.StartInvoiceDate, data.EndInvoiceDate);
                        //Total += parseFloat(data.InflationAmount);
                        objDetail.Amount = parseFloat(data.InflationAmount * JmlBln);
                    }
                    objDetail.IsSplit = true;
                }
                else {
                    objDetail.Amount = parseFloat(data.AmountIDR);
                    objDetail.IsSplit = false;
                }
                objDetail.Type = BillType;
                objDetail.trxReconcileID = data.id;
                PoDetail.push(objDetail);
                objValidate.ValueId = data.id;
                objValidate.ValueDesc = BillType;
                POValidate.push(objValidate);
            });

            var parameters = {
                param: POValidate
            }

            $.ajax({
                url: "/api/POInput/CheckSplitPO",
                type: "POST",
                dataType: "json",
                data: parameters,
            }).done(function (data, textStatus, jqXHR) {
                if (data != null && data != "") {
                    Common.Alert.Warning(data)
                    return;
                }
                else {
                    TableData.SaveTrx();
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                return;
            }).always(function () {
                l.stop();
            });
        } else {
            l.stop();
        }
    },

    SaveTrx: function () {
        $('#btnDetailSave').attr("disabled", "disabled");
        if ($("#formDetailDataNon").parsley().validate()) {
            var l = Ladda.create(document.querySelector("#btnDetailSave"))
            SplitValue = $('#slSplitPO').val();
            BillType = $('#slBillType').val();
            PoDetail = [];

            if (SplitValue != "1") {
                BillType = "";
            }

            tabler = $('#tblDataSelected').DataTable();

            tabler.rows().every(function (rowIdx, tableLoop, rowLoop) {
                var objDetail = {
                    Amount: 0,
                    IsSplit: false,
                    Type: "",
                    trxReconcileID: 0
                }

                var objValidate = {
                    ValueId: 0,
                    ValueDesc: ""
                }

                var data = this.data();
                if (SplitValue == 1) {
                    if (BillType == "BaseLease") {
                        objDetail.Amount = parseFloat(data.BaseLeasePrice);
                    }
                    else if (BillType == "Service") {
                        objDetail.Amount = parseFloat(data.ServicePrice);
                    }
                    else if (BillType == "Inflation") {
                        JmlBln = Form.monthDiff(data.StartInvoiceDate, data.EndInvoiceDate);
                        //Total += parseFloat(data.InflationAmount);
                        objDetail.Amount = parseFloat(data.InflationAmount * JmlBln);
                        //objDetail.Amount = parseFloat(data.InflationAmount);
                    }
                    objDetail.IsSplit = true;
                }
                else {
                    objDetail.Amount = parseFloat(data.AmountIDR);
                    objDetail.IsSplit = false;
                }
                objDetail.Type = BillType;
                objDetail.trxReconcileID = data.id;
                PoDetail.push(objDetail);
            });

            var formData = new FormData();

            formData.append("mstRABoqID", "");

            var CompanyId = $('#slSearchCompany').val().trim();
            formData.append("CompanyId", CompanyId);

            var Operator = $('#slSearchCustomer').val().trim();
            formData.append("CustomerID", Operator);

            formData.append("Regional", "");

            var Currency = $('#PoCurrencyVal').val().trim();
            formData.append("Currency", Currency);

            var POType = $('#PoTypeVal').val().trim();
            formData.append("POType", POType);

            var Remarks = $('#tbRemarksVal').val();
            formData.append("Remarks", Remarks);

            var PONumber = $('#PoNumberVal').val().trim();
            formData.append("PONumber", PONumber);

            var PODate = $('#PoDateVal').val().trim();
            formData.append("PODate", PODate);

            var POReceiveDate = $('#PoReceiveDateVal').val().trim();
            formData.append("POReceiveDate", POReceiveDate);

            var StartPeriod = $('#tbStartPeriod').val().trim();
            formData.append("StartPeriod", StartPeriod);

            var EndPeriod = $('#tbEndPeriod').val().trim();
            formData.append("EndPeriod", EndPeriod);

            var POAmount = $('#PoAmountVal').val().replace(/,/g, "");
            formData.append("POAmount", POAmount);

            var TotalTenant = $('#PoTenantVal').val().trim();
            formData.append("TotalTenant", TotalTenant);

            var mstRAActivityID = 8;
            formData.append("mstRAActivityID", mstRAActivityID);

            formData.append("ListID", Data.RowDataSelected.toString());

            formData.append("PoDetail", JSON.stringify(PoDetail));

            var fileInput = document.getElementById("postedFile2");
            if (document.getElementById("postedFile2").files.length != 0) {

                fsFileName = fileInput.files[0].name;
                formData.append("FileName", fsFileName);

                fsFile = fileInput.files[0];

                fsExtension = fsFileName.split('.').pop().toUpperCase();

                if ((fsFile.size / 1024) > 2048) {
                    Common.Alert.Warning("Upload File Can`t bigger then 2048 bytes (2mb)."); return;
                }

                if (fsExtension != "XLS" && fsExtension != "XLSX" && fsExtension != "PDF") {
                    Common.Alert.Warning("Please upload an Excel or PDF File."); return;
                }
                else {

                    formData.append('FilePO', fsFile);
                }

                errors = false;
            }
            else {
                Common.Alert.Warning("Please Select Document."); return;
            }

            $.ajax({
                url: '/api/POInput/SaveData',
                type: 'POST',
                data: formData,
                async: false,
                crossDomain: true,
                cache: false,
                contentType: false,
                processData: false,
            }).done(function (data, textStatus, jqXHR) {
                if (data != "0") {
                    if (data !== "Exception") {
                        if (data.length <= 0) {
                            $('.panelDetailPONon').find('input:file').val('');
                            Common.Alert.Warning("The uploaded file is invalid or empty. Please use the available template, and fill the file correctly with valid product data.");
                        } else {
                            $('.panelDetailPONon').find('input:file').val('');
                            Common.Alert.Success("Success to create PO!");
                            $('#slSplitPO').val("0");
                            TableData.ResetTrx();
                        }
                    } else {
                        $('.panelDetailPONon').find('input:file').val('');
                        Common.Alert.Warning("The uploaded file is invalid. Please download the latest template by clicking the Download Template button.");
                    }
                } else {
                    Common.Alert.Warning("Failed to save PO, Please Contact System Administrator !");
                }
                
                l.stop();
            }).fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l.stop()
            });
        }
        $('#btnDetailSave').removeAttr("disabled");
    },

    Export: function () {
        if ($("#formSearchs").parsley().validate()) {
            fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
            fsCustomer = $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val();
            fsStip = $("#slSearchStip").val() == null ? "" : $("#slSearchStip").val();
            fsQuarterly = $("#slSearchQuarterly").val() == null ? "" : $("#slSearchQuarterly").val();
            fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
            fsSiteName = $("#tbSearchSiteName").val();
            fsProductType = $("#slSearchProductType").val() == null ? "" : $("#slSearchProductType").val();
            fsSONumber = $("#tbSearchSONumber").val();
            fsSiteID = $("#tbSearchSiteID").val();
            fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
            fsPowerType = $("#slSearchPowerType").val() == null ? "" : $("#slSearchPowerType").val();
            fsYear = $("#slSearchYear").val() == null ? "" : $("#slSearchYear").val();


            window.location.href = "/RevenueAssurance/POExport?strCustomerId=" + fsCustomer + "&strCompanyId=" + fsCompanyId + "&strStipID=" + fsStip + "&strCurrency=" + fsCurrency + "&strQuarterly=" + fsQuarterly
                + "&strSONumber=" + fsSONumber + "&strSiteID=" + fsSiteID + "&strYear=" + fsYear + "&strProduct=" + fsProductType + "&strBapsType=" + fsBapsType + "&strPowerType=" + fsPowerType + "&strSiteName=" + fsSiteName;
        }
        
    }
}

var Process = {
    
    PICA: function () {
        Common.Alert.Success('Data Alredy Send To Reconcile Process');
    },
    SendToInput: function () {
        Common.Alert.Success('Data Alredy Send Back To Reconcile Input');
    },
    
    SendToProcess: function () {
        var l = Ladda.create(document.querySelector("#btYesConfirm"))
        var indexTab = $("#tabReconcile").tabs('option', 'active');
        var params = {
            soNumb: Data.RowSelectedRaw
        }
        
        //console.log("Data: " + JSON.stringify(params));
        $.ajax({
            url: "/api/ReconcileData/doProcess",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (data == "1") {
                if (data == "1") {
                    Common.Alert.Success("Data Success Send To Reconcile Process!");
                    Data.RowSelectedRaw = [];
                    Data.RowSelectedProcess = [];
                } else {
                    Common.Alert.Warning("Something Went Wrong. Contact IT Help Desk");
                }
            
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Form.DoneProcess();
        })
    }

}

var Control = {
    BindingSelectCompany: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/TBGSys_Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    elements.append("<option value='" + item.CompanyId.trim() + "'>" + item.Company + "</option>");
                })
            }

            elements.select2({ placeholder: "Select Company Name", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectStip: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/TBGSys_Stip",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    elements.append("<option value='" + item.Value.trim() + "'>" + item.Text + "</option>");
                })
            }

            elements.select2({ placeholder: "Select STIP", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectProduct: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/TenantType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    elements.append("<option value='" + item.Value.trim() + "'>" + item.Text + "</option>");
                })
            }

            elements.select2({ placeholder: "Select Product", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectBapsType: function (elements) {
        $.ajax({
            url: "/api/mstDataSource/BapsType",
            type: "GET",
            async: false
        })
       .done(function (data, textStatus, jqXHR) {
           elements.html("<option></option>")
           if (Common.CheckError.List(data)) {
               $.each(data, function (i, item) {
                   elements.append("<option value='" + $.trim(item.BapsType) + "'>" + item.BapsType + "</option>");
               })
               //$(id).val(5).trigger('change');
           }
           elements.select2({ placeholder: "Select Baps Type", width: null });
       })
       .fail(function (jqXHR, textStatus, errorThrown) {
           Common.Alert.Error(errorThrown);
       });
    },
    BindingSelectPowerType: function (elements) {
        $.ajax({
            url: "/api/mstDataSource/PowerType",
            type: "GET",
            async: false
        })
       .done(function (data, textStatus, jqXHR) {
           elements.html("<option></option>")
           if (Common.CheckError.List(data)) {
               $.each(data, function (i, item) {
                   elements.append("<option value='" + parseInt($.trim(item.KodeType)) + "'>" + item.PowerType + "</option>");
               })
           }
           elements.select2({ placeholder: "Select Power Type", width: null });
       })
       .fail(function (jqXHR, textStatus, errorThrown) {
           Common.Alert.Error(errorThrown);
       });
    },
    BindingSelectRegional: function () {
        $.ajax({
            url: "/api/MstDataSource/Regional",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slRegional").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slRegional").append("<option value='" + item.RegionalId.trim() + "'>" + item.RegionalName + "</option>");
                })
            }

            $("#slRegional").select2({ placeholder: "Select Regional", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectGetBOQList: function () {
        var CompanyId = $('#slSearchCompanyName2').val().trim();
        var Operator = $('#slSearchOperator2').val().trim();
        var Regional = $('#slRegional').val().trim();

        var params = { strCompanyId: CompanyId, strOperator: Operator, strRegional: Regional }


        $.ajax({
            url: "/api/POInput/GetBOQList",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slBOQ").html("<option></option>")

            if (Common.CheckError.List(data)) {
                if (data.data.length > 0) {
                    $.each(data.data, function (i, item) {
                        $("#slBOQ").append("<option value='" + item.ID + "'>" + item.BOQNumber + "</option>");
                    })
                }
            }

            $("#slBOQ").select2({ placeholder: "Select BOQ", width: null });

            
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectGetBOQEditList: function (arr) {
        var CompanyId = $('#slSearchCompanyName2').val().trim();
        var Operator = $('#slSearchOperator2').val().trim();
        var Regional = $('#slRegional').val().trim();

        var params = { strCompanyId: CompanyId, strOperator: Operator, strRegional: Regional }


        $.ajax({
            url: "/api/POInput/GetBOQList",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slBOQ").html("<option></option>")

            if (Common.CheckError.List(data)) {
                if (data.data.length > 0) {
                    $.each(data.data, function (i, item) {
                        $("#slBOQ").append("<option value='" + item.ID.toString() + "'>" + item.BOQNumber + "</option>");
                    })
                }
            }

            $("#slBOQ").select2({ placeholder: "Select BOQ", width: null }).select2('val', arr);


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
            $("#slSearchOperator").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchOperator").append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                })
            }

            $("#slSearchOperator").select2({ placeholder: "Select Operator", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectOperators: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item.OperatorId.trim() == 'TSEL' || item.OperatorId.trim() == 'ISAT' || item.OperatorId.trim() == 'XL' || item.OperatorId.trim() == 'HCPT') {

                        if (FilterCustomerTab == 0 && item.OperatorId.trim() == 'TSEL') {
                            elements.append("<option value='" + item.OperatorId.trim() + "'>" + item.OperatorId + "</option>");
                        }
                        else if (FilterCustomerTab == 1 && item.OperatorId.trim() != 'TSEL') {
                            elements.append("<option value='" + item.OperatorId.trim() + "'>" + item.OperatorId + "</option>");
                        }
                        else if (FilterCustomerTab > 1 ){
                            elements.append("<option value='" + item.OperatorId.trim() + "'>" + item.OperatorId + "</option>");
                        }
                    }
                        
                    //elements.append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                })
            }

            elements.select2({ placeholder: "Select Operator", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingNextStep: function (Customer) {

        $.ajax({
            url: "/api/MstDataSource/NextActivity",
            type: "GET",
            data: { CurrentActivity: CurrentTab, CustomerID: Customer }
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slNextActivity").html("");
            $("#slNextActivity").append("<option value='0'>Cancel PO</option>");

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item.Value < '9')
                        $("#slNextActivity").append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                })
            }

            //$("#slNextStep").select2({ placeholder: "Select Operator", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingBOQDetail: function (ID) {
        var params = { trxRAPurchaseOrderID: ID }

        $.ajax({
            url: "/api/POInput/GetBOQDetail",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
        })
        .done(function (data, textStatus, jqXHR) {
            $("#tbDetailBOQ").val(data.data);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    }
}

var Helper = {
    Calculate: function () {},
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
    IsElementExistsInArray: function (value, arr) {
        var isExist = false;
        for (var i = 0 ; i < arr.length ; i++) {
            if (arr[i] == value) {
                isExist = true;
                break;
            }
        }
        return isExist;
    },
    InsertsElementInArray: function (value) {
        var arr = Data.RowSelectedSite;
        var isExist = false;
        for (var i = 0 ; i < arr.length ; i++) {
            if (arr[i] == value) {
                isExist = true;
                break;
            }
        }
        if (!isExist) {
            Data.SiteRow.push(value);
        }
    },
    GetListId: function (isRaw) {
        //for CheckAll Pages
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        var fsGroupBy = $("#slGroupBy").val() == null ? "" : $("#slGroupBy").val();

        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strGroupBy: fsGroupBy,
            isRaw: isRaw
        };

        var AjaxData = [];
        $.ajax({
            url: "/api/POInput/GetListId",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                AjaxData = data;
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
        //Common.Alert.Error(AjaxData);
        return AjaxData;
    }
}

