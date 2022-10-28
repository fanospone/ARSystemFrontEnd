Data = {};
SelectRow = {};
TempData = [];
SiteData = [];

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
var CurrentTab = 1;

jQuery(document).ready(function () {
    $("#slSONumber").select2({
        tags: true,
        multiple: true,
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
                $result.append(" <em>(new)</em>");
            }

            return $result;
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

    Data.RowSelectedRaw = [];
    Data.RowSelectedProcess = [];
    Data.RowSelectedDone = [];
    Data.RowSelectedHold = [];
    Data.RowSelectedUpload = [];
    Data.RowSelectedSite = [];
    Form.Init();
    TableProcess.Init();
    TableRaw.Init();
    TableHold.Init();
    TableDone.Init();
    TableUpload.Init();
    $('#tblSiteUpload').hide();
    $('#btDocumentUpload').hide();
    $("#slBatch").val("").trigger('change');
    $("#slTenantType").val("").trigger('change');
    $(".panelSearchResult").show();
    //TableRaw.Search();
    //oTable = $('#tblRaw').DataTable();   //pay attention to capital D, which is mandatory to retrieve "api" datatables' object, as @Lionel said
    //$('#myInputTextField').keyup(function () {
    //    //oTable.search($(this).val()).draw();
    //    oTable.columns(1).search(this.value);
    //    oTable.columns(2).search(this.value);
    //    //oTable.draw();
    //})

    $('#btUpdateAmountBulky').hide();
    $('.datepicker').datepicker({
        format: 'd-M-yyyy'
    });
    $('#btProcess').show();
    $('#btUpdate').hide();
    $('#btDone').hide();
    $('#btModify').hide();
    $('#btUpload').show();
    $('#btBack').hide();
    $('#error_omprice').hide();
    $('#btSubmitReconcile').hide();
    $('#btUploadDoc').hide();
    $('#btHoldUpdate').hide();
    $('#btDocumentUpload').hide();
    //panel Summary
    $("#formSearch").submit(function (e) {
        TableProcess.Search();
        e.preventDefault();
    });

    $("#btAddSite").unbind().click(function () {
        Form.AddSite();
        TableUpload.Search();
        if (SiteData.length < 1) {
            $('#formupload').hide();
            $('#btDeleteSite').hide();
        }
        else {
            $('#formupload').show();
            $('#btDeleteSite').show();
        }
    });

    $("#btDeleteSite").unbind().click(function () {
        Form.DeleteSite();
        TableUpload.Search();
    });

    $("#btSearch").unbind().click(function () {
        //debugger;
        //if ($("#tabReconcile").tabs('option', 'active') == 0)
        //    TableRaw.Search();
        //else
        //    TableProcess.Search();

        if (CurrentTab == 1)
            TableRaw.Search();
        else if (CurrentTab == 2)
            TableProcess.Search();
        else if (CurrentTab == 3)
            TableDone.Search();
        else
            TableHold.Search();
    });

    $("#btReset").unbind().click(function () {
        TableProcess.Reset();
    });

    $("#btSearchUpload").click(function (e) {
        e.preventDefault();
        if ($("#formSearchUpload").parsley().validate()) {
            TableUpload.Search();
            if (SiteData.length < 1) {
                $('#formupload').hide();
                $('#btDeleteSite').hide();
            }
            else {
                $('#formupload').show();
                $('#btDeleteSite').show();
            }
        }
    });

    $("#btResetUpload").unbind().click(function () {
        TableUpload.Reset();
    });

    $('#tblUpload').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');


        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            Data.RowSelectedUpload.push(parseInt(id));

        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row" + id).removeClass("active");
            Helper.RemoveElementFromArray(Data.RowSelectedUpload, parseInt(id));
        }
    });

    $('#tblRaw').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');


        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            Data.RowSelectedRaw.push(parseInt(id));
            //var OP = table.row(this).data(); //$(this).parents('tr');
            //console.log(OP);
            //Common.Alert.Warning(JSON.stringify(Data.RowSelectedRaw));
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row" + id).removeClass("active");
            Helper.RemoveElementFromArray(Data.RowSelectedRaw, parseInt(id));
        }
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

    $('#tblDone').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');

        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            Data.RowSelectedDone.push(parseInt(id));
            //Common.Alert.Warning(JSON.stringify(Data.RowSelectedRaw));
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row" + id).removeClass("active");
            Helper.RemoveElementFromArray(Data.RowSelectedDone, parseInt(id));
        }
    });

    $('#tblHold').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        var table = $('#tblHold').DataTable();

        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            Data.RowSelectedHold.push(parseInt(id));
            TempData.push(table.row(this).data());
            if (Form.CheckObjectEmpty(SelectRow)) {
                SelectRow = table.row(this).data();
            }
            //Common.Alert.Warning(JSON.stringify(Data.RowSelectedRaw));
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row" + id).removeClass("active");
            Helper.RemoveElementFromArray(Data.RowSelectedHold, parseInt(id));
            Helper.RemoveElementFromArray(TempData, table.row(this).data());
            if (table.row(this).data().Id == SelectRow.Id) {
                SelectRow = {};
            }
        }
    });

    $('#tblUpload').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        var table = $('#tblUpload').DataTable();

        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            TempData.push(table.row(this).data());
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row" + id).removeClass("active");
            Helper.RemoveElementFromArray(TempData, table.row(this).data());
        }
    });


    $('#tabReconcile').unbind().on('tabsactivate', function (event, ui) {
        var newIndex = ui.newTab.index();
        CurrentTab = 1;
        CurrentTab = newIndex + 1;
        $('.panelSearchFilter').show();
        $('#btDocumentUpload').hide();
        $('#btUpdateAmountBulky').hide();

        if (newIndex == 1) {
            Data.RowSelectedProcess = [];
            TableProcess.Search();
            $('#btProcess').hide();
            $('#btUpdate').hide();
            $('#btModify').show();
            //$('#btUploadDoc').show();
            $('#btBack').hide();
            $('#btSubmitReconcile').show();
            $('#btDone').show();
            $('#btHoldUpdate').hide();
            $('#btUpdateAmountBulky').show();
        }
        else if (newIndex == 2) {
            Data.RowSelectedDone = [];
            TableDone.Search();
            $('#btProcess').hide();
            $('#btUpdate').hide();
            $('#btModify').hide();
            //$('#btUploadDoc').show();
            $('#btBack').show();
            $('#btSubmitReconcile').hide();
            $('#btDone').hide();
            $('#btHoldUpdate').hide();
        }
        else if (newIndex == 3) {
            Data.RowSelectedHold = [];
            TableHold.Search();
            $('#btProcess').hide();
            $('#btUpdate').hide();
            $('#btModify').hide();
            //$('#btUploadDoc').show();
            $('#btBack').hide();
            $('#btSubmitReconcile').hide();
            $('#btDone').hide();
            $('#btHoldUpdate').show();
        }
        else if (newIndex == 4) {
            $('#btProcess').hide();
            $('#btUpdate').hide();
            $('#btDone').hide();
            $('#btModify').hide();
            $('#btUpload').hide();
            $('#btBack').hide();
            $('#error_omprice').hide();
            $('#btSubmitReconcile').hide();
            $('#btUploadDoc').hide();
            $('#btHoldUpdate').hide();
            $('#slUploadBatch').select2({});
            $('.panelSearchFilter').hide();
            $('#btDocumentUpload').show();
            if (SiteData.length < 1) {
                $('#formupload').hide();
                $('#btDeleteSite').hide();
            }
        }
        else {
            Data.RowSelectedRaw = [];
            TableRaw.Search();
            $('#btDone').hide();
            $('#btProcess').show();
            $('#btModify').hide();
            $('#btUploadDoc').hide();
            $('#btBack').hide();
            $('#btSubmitReconcile').hide();
            $('#btHoldUpdate').hide();
        }
    });

    $("#btUpdateAmountBulky").unbind().click(function () {
        $('#btNoUpdate').focus();
        if(Data.RowSelectedProcess.length > 0)
            $('#mdlBulkyAmount').modal('show');
        else
            Common.Alert.Warning("Please Select One or More Data")
    });

    $("#btHoldUpdate").unbind().click(function () {
        if (Data.RowSelectedHold.length == 0 || Data.RowSelectedHold.length > 1)
            Common.Alert.Warning("Please Select One Data")
        else {
            //console.log(TempData);
            //$('#tbExpiredDate').val(Common.Format.ConvertJSONDateTime(SelectRow.StartInvoiceDate));
            if (Form.CheckObjectEmpty(SelectRow)) {
                SelectRow = TempData[0];
                //var oTable = $('#tblHold').DataTable();
                //oTable.rows().every(function (rowIdx, tableLoop, rowLoop) {
                //    var node = this.node();
                //    //$(node).find('input').prop('checked');
                //    console.log($(node).find('input').prop('checked'));
                //});
            }
            $('#mdlToInput').modal('show');
        }
        
    });

    $("#btProcess").unbind().click(function () {
        if (Data.RowSelectedRaw.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else
            $('#mdlNextActivity').modal('show');

        Control.BindingNextStep();
    });

    $("#btDone").unbind().click(function () {
        if (Data.RowSelectedProcess.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else
            $('#mdlNextActivity').modal('show');

        Control.BindingNextStep();
    });

    $("#btDoneConfirm").unbind().click(function () {
        $('#mdlToDone').modal('hide');
        Process.SendToDone();
    });

    $("#btUploadDoc").unbind().click(function () {
        if (Data.RowSelectedProcess.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else
            Form.UploadPanel();
    });

    $("#btSubmitUpload").unbind().click(function () {
        Process.UploadDocument();
    });

    $("#btDismissUpload").unbind().click(function () {
        Form.BackToListPanel();
    });

    $("#btNoReject").unbind().click(function () {
        $("#slDept").val("").trigger('change');
        $("#slPIC").val("").trigger('change');
        $("#slPICA").val("").trigger('change');
        $("#tbRemarks").val("");
        $('#mdlReject').modal('hide');
    });

    $("#btUpdate").unbind().click(function () {

        if (Data.RowSelectedRaw.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else if (Data.RowSelectedRaw.length > 1)
            Common.Alert.Warning("Please Select One Data")
        else
            Form.UpdateOMPricePanel();
    });

    $("#btModify").unbind().click(function () {
        if (Data.RowSelectedProcess.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else
            $('#mdlReject').modal('show');
    });

    $("#btBack").unbind().click(function () {
        if (Data.RowSelectedDone.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else
            $('#mdlNextActivity').modal('show');

        Control.BindingNextStep();
    });

    $("#btSubmitReconcile").unbind().click(function () {
        if (Data.RowSelectedProcess.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else
            $('#mdlSubmitReconcile').modal('show');
    });

    $("#btConfirm").unbind().click(function () {
        Process.ProcessNextActivity();
    });

    $("#btVoid").unbind().click(function () {
        $('#mdlNextActivity').modal('show');
    });

    $("#btYesConfirm").unbind().click(function () {
        Process.SendToProcess();
    });

    $("#btOMPriceUpdate").unbind().click(function () {
        var str = Number($("#tbUpdateOMPrice").val().replace(/,/g, '')).toFixed(2);
        var re = /^[-+]?[0-9]+\.[0-9]+$/;
        var found = str.match(re);

        if (found && str != "0" && str != "0.00") {
            Process.UpdateOmPrice();
        }
        else {
            Common.Alert.Warning("Please fill not zero OM Price!");
        }
    });

    $("#btBackToList").unbind().click(function () {
        Form.BackToListPanel();
    });

    $("#btUpdateAmount").unbind().click(function () {
        $('#error_omprice').hide();
        var str = Number($("#tbDetailOMPrice").val().replace(',', '')).toFixed(2);
        var re = /^[-+]?[0-9]+\.[0-9]+$/;
        var found = str.match(re);

        if (found && str != "0" && str != "0.00") {
            Process.UpdateOmPrice();
        }
        else {
            $('#error_omprice').show();
            $('#mdlDetail').modal('toggle');
            //Common.Alert.Warning("Please fill not zero OM Price!");
        }
    });

    $("#btUpdateReconcile").unbind().click(function () {
        var str = Number($("#tbUpdateBaseLease").val().replace(/,/g, '')).toFixed(2);
        var re = /^[-+]?[0-9]+\.[0-9]+$/;
        var found = str.match(re);

        if ($("#slUpdateCurrency").val() == null || $("#slUpdateCurrency").val() == "") {
            Common.Alert.Warning("Please Select Invoice Currency!");
            return;
        }

        if ($("#slUpdateServiceCurrency").val() == null || $("#slUpdateServiceCurrency").val() == "") {
            Common.Alert.Warning("Please Select Service Currency!");
            return;
        }

        if (found && str != "0" && str != "0.00") {

            if (Data.UpdateCutOffHCPT) {
                swal({
                    title: "Are you sure to change the invoice period?",
                    text: "Site Name: " + $("#tbUpdateSiteName").val() + ", SO Number: " + $("#tbUpdateSONumber").val() +
                        ", Customer Invoice: " + $("#tbUpdateCustomerInvoice").val() + ", Term: " + $("#tbUpdateTerm").val() +
                        ", New Invoice Period: " + $("#tbUpdateStartDateInv").val() + " - " + $("#tbUpdateEndDateInv").val(),
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonClass: "btn-success",
                    confirmButtonText: "Yes"
                }, function () {
                    Process.UpdateReconcile();
                });
            } else {
                Process.UpdateReconcile();
            }
        }
        else {
            Common.Alert.Warning("Please fill not zero for Base Price!");
        }
    });

    $("#btUpload").unbind().click(function () {
        Process.Import();
    });

    $("#btYesConfirm2").unbind().click(function () {
        if ($("#tbExpiredDate").val() == null || $("#tbExpiredDate").val() == '') {
            $('#mdlToInput').modal('hide');
            Common.Alert.Warning("Please Select Active Date!");
        }
        else {
            var ExpDate = Date.parse(Form.ParseDateFormat($("#tbExpiredDate").val()));
            var InvDate = Date.parse(SelectRow.StartInvoiceDate);
            var EndDate = Date.parse(SelectRow.EndBapsDate);


            if (ExpDate > EndDate) {
                $('#mdlToInput').modal('hide');
                Common.Alert.Warning("Active Date Can`t be Greater Than Baps End Date ( " + Common.Format.ConvertJSONDateTime(SelectRow.EndBapsDate) + " )!"); return;
            }

            if (ExpDate >= InvDate) {
                Process.SendToInput();
            }
            else {
                $('#mdlToInput').modal('hide');
                Common.Alert.Warning("Invalid Active Date!");
            }

        }

    });

    $("#btYesReject").unbind().click(function (e) {
        e.preventDefault();
        var DeptReject = $("#slDept").val();
        var PICReject = $("#slPIC").val();
        var PICAReject = $("#slPICA").val();
        var RemarksReject = $("#tbRemarks").val();

        if ((DeptReject != null && DeptReject != "") && (PICReject != null && PICReject != "") && (PICAReject != null && PICAReject != "") && (RemarksReject != null && RemarksReject != "")) {
            Process.PICA();
        }
        else {
            return;
        }

    });

    $('#slDept').on('change', function () {

        $('#slPIC').html('<option></option>');
        $('#slPICA').html('<option></option>');

        if (this.value == 'mar') {
            $('#slPIC').append('<option>Reny Anggraeny</option>');
            $('#slPIC').append('<option>Fattah Mirza</option>');
            $('#slPIC').append('<option>Ahmad Junaedi</option>');
            $('#slPIC').append('<option>Egia Darminta</option>');
            $('#slPIC').append('<option>Rizky Dhaahirrani</option>');
            $('#slPIC').append('<option>Anggareni</option>');

            $('#slPICA').append('<option>CR Minus</option>');
            $('#slPICA').append('<option>Proses PO</option>');
            $('#slPICA').append('<option>LOI</option>');
            $('#slPICA').append('<option>SPK</option>');
            $('#slPICA').append('<option>Form Kesepakatan Relok</option>');
            $('#slPICA').append('<option>CAF</option>');
            $('#slPICA').append('<option>Issue Commercial</option>');
        }

        if (this.value == 'pdi') {
            $('#slPIC').append('<option>Erlangga</option>');
            $('#slPIC').append('<option>Dora Naro</option>');

            $('#slPICA').append('<option>Revisi MLA</option>');
        }

        if (this.value == 'ra') {
            $('#slPIC').append('<option>Iwan Setiawan/option>');
            $('#slPIC').append('<option>Subiyatmoko</option>');
            $('#slPIC').append('<option>Riny Novelia</option>');
            $('#slPIC').append('<option>Ninda Wibowo</option>');
            $('#slPIC').append('<option>Desi Patmasatiti</option>');
            $('#slPIC').append('<option>Defi Fazria</option>');
            $('#slPICA').append('<option>Revisi Dokumen</option>');
        }
    });

    Data.UpdateCutOffHCPT = false;
    $('#cbUpdateCutOffHCPT').on('switchChange.bootstrapSwitch', function (event, state) {
        if (event.target.checked) {
            Data.UpdateCutOffHCPT = true;
            Data.BackupEndDateInv = $('#tbUpdateEndDateInv').val();

            var dtStartDateInv = new Date($('#tbUpdateStartDateInv').val().replace(/-/g, '/'));
            var dtEndDateInv = new Date($('#tbUpdateEndDateInv').val().replace(/-/g, '/'));
            var dtOneYearAferStart = new Date(dtStartDateInv.setMonth(dtStartDateInv.getMonth() + 12));
            var dtEndDateInvNew = new Date(dtEndDateInv.getFullYear().toString() + '/09/30')

            if (dtOneYearAferStart < dtEndDateInvNew) {
                dtEndDateInvNew = new Date((dtEndDateInv.getFullYear() - 1).toString() + '/09/30')
            }

            $('#tbUpdateEndDateInv').val(Common.Format.ConvertJSONDateTime(dtEndDateInvNew));

        } else {
            Data.UpdateCutOffHCPT = false;

            if (Data.BackupEndDateInv != null) {
                $("#tbUpdateEndDateInv").val(Data.BackupEndDateInv);
            }
        }

        Control.BindProRateAmount();
    });
});

var Form = {
    Init: function () {
        $(".panelUpdate").hide();
        $(".panelUpload").hide();
        if (!$("#hdAllowProcess").val()) {
            //$("#btProcess").hide();
            //$("#btReject").hide();
            //$("#btReceive").hide();
        }

        $("#slRegional").select2({ placeholder: "Select Regionl", width: null });
        $("#slBatch").select2({ placeholder: "Select Batch", width: null });
        $("#slRenewalYear").select2({ placeholder: "Select Renewal Year", width: null });
        $("#slRenewalYearSeq").select2({ placeholder: "Select Renewal Year Seq", width: null });
        $("#slProvince").select2({ placeholder: "Select Province", width: null });
        $("#slReconcileType").select2({ placeholder: "Select ReconcileType Type", width: null });
        $("#slSearchCurrency").select2({ placeholder: "Select Currency", width: null });
        $("#slDueDatePerMonth").select2({ placeholder: "Select Due Date Per Month", width: null });

        Control.BindingSelectCompany();
        Control.BindingSelectOperator();
        Control.BindingSelectRegional();
        Control.BindingSelectProvince();
        Control.BindingSelectTenantType();

        TableProcess.Reset();
        $('#tabReconcile').tabs();
        $('#btProcess').show();
    },
    UpdateOMPricePanel: function () {
        //debugger;
        $('#tbUpdateID').val(Data.Selected.Id);
        $('#tbUpdateAddress').val(Data.Selected.Address);
        $('#tbUpdateSONumber').val(Data.Selected.SONumber);
        $('#tbUpdateSiteId').val(Data.Selected.SiteID);
        $('#tbUpdateSiteName').val(Data.Selected.SiteName);
        $('#tbUpdateSiteIDOpr').val(Data.Selected.CustomerSiteID);
        $('#tbUpdateSiteNameOpr').val(Data.Selected.CustomerSiteName);
        $('#tbUpdateRegionName').val(Data.Selected.RegionalName);
        $('#tbUpdateProvinceName').val(Data.Selected.ProvinceName);
        $('#tbUpdateResidenceName').val(Data.Selected.ResidenceName);
        $('#tbUpdateIpo').val(Data.Selected.PONumber);
        $('#tbUpdateMla').val(Data.Selected.MLANumber);
        $('#tbUpdateStartDateLease').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartBapsDate));
        $('#tbUpdateEndDateLease').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndBapsDate));
        $('#tbUpdateTerm').val(Data.Selected.Term);
        $('#tbUpdateBapsType').val(Data.Selected.BapsType);
        $('#tbUpdateCustomerInvoice').val(Data.Selected.CustomerInvoice);
        $('#tbUpdateCustomerAsset').val(Data.Selected.CustomerID);
        $('#tbUpdateCompanyInvoice').val(Data.Selected.CompanyInvoice);
        $('#tbUpdateCompanyInvoice').trigger("change");
        $('#tbUpdateCompanyAsset').val(Data.Selected.CompanyName);
        $('#tbUpdateStipSiro').val(Data.Selected.StipSiro);
        $('#tbUpdateInvoiceType').val(Data.Selected.InvoiceTypeName);
        $('#slUpdateCurrency').val(Data.Selected.BaseLeaseCurrency);
        $('#tbUpdateStartDateInv').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartInvoiceDate));
        $('#tbUpdateEndDateInv').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndInvoiceDate));
        $('#tbUpdateRFIDate').val(Common.Format.ConvertJSONDateTime(Data.Selected.RFIDate));
        $('#tbUpdateBaukDate').val(Common.Format.ConvertJSONDateTime(Data.Selected.BaufDate));
        $('#tbUpdateDropFODistance').val(Data.Selected.DropFODistance);

        $('#tbUpdateBaseLease').val(Common.Format.CommaSeparation(Data.Selected.BaseLeasePrice.toString()));
        $('#tbUpdateOMPrice').val(Common.Format.CommaSeparation(Data.Selected.ServicePrice.toString()));
        $('#tbUpdateTotalProRate').val(Common.Format.CommaSeparation(Data.Selected.TotalPaymentRupiah.toString()));

        $('#tbUpdateInflationAmount').val(Common.Format.CommaSeparation(Data.Selected.InflationAmount.toString()));
        $('#tbUpdateAdditionalAmount').val(Common.Format.CommaSeparation(Data.Selected.AdditionalAmount.toString()));
        $('#tbUpdateDeductionAmount').val(Common.Format.CommaSeparation(Data.Selected.DeductionAmount.toString()));
        $('#tbUpdatePenaltyAmount').val(Common.Format.CommaSeparation(Data.Selected.PenaltySlaAmount.toString()));
        $('#slUpdateServiceCurrency').val(Data.Selected.ServiceCurrency);
        $('#slUpdateInflationCurrency').val(Data.Selected.InflationCurrency);
        $('#slUpdateAdditionalCurrency').val(Data.Selected.AdditionalCurrency);
        $('#slUpdateDeductionCurrency').val(Data.Selected.DeductionCurrency);
        $('#slUpdatePenaltyCurrency').val(Data.Selected.PenaltySlaCurrency);
        $('#tbUpdateReconcileDate').val(Common.Format.ConvertJSONDateTime(Data.Selected.ReconcileDate));

        $("#cbUpdateCutOffHCPT").bootstrapSwitch("state", false);
        if (Data.Selected.CustomerID == 'HCPT' && new Date(Data.Selected.EndInvoiceDate).getDate() == 31 && (new Date(Data.Selected.EndInvoiceDate).getMonth()+1) == 3) {
            $("#divUpdateCutOffHCPT").show();
        } else {
            $("#divUpdateCutOffHCPT").hide();
        }

        $(".panelSearchZero").fadeOut();
        $(".panelSearchResult").fadeOut();
        $(".panelSearchFilter").fadeOut();
        $(".panelUpdate").fadeIn(1000);
    },
    BackToListPanel: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").show();
        $(".panelSearchFilter").show();
        $(".panelUpdate").hide();
        $(".panelUpload").hide();
        //TableRaw.Search();
    },
    DoneUploadDocument: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").show();
        $(".panelSearchFilter").show();
        $(".panelUpdate").hide();
        $(".panelUpload").hide();
        TableProcess.Search();
    },
    UploadPanel: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        $(".panelSearchFilter").hide();
        $(".panelUpdate").hide();
        $(".panelUpload").show();
    },
    DoneProcess: function () {
        $("#pnlSummary").fadeIn();
        $('#mdlToProcess').modal('toggle');
        TableRaw.Search();
    },
    DoneInput: function () {
        $("#pnlSummary").fadeIn();
        $('#mdlToInput').modal('toggle');
        TableProcess.Search();
    },
    DonePICA: function () {
        $("#pnlSummary").fadeIn();
        $('#mdlReject').modal('hide');
        $("#slDept").val("").trigger('change');
        $("#slPIC").val("").trigger('change');
        $("#slPICA").val("").trigger('change');
        $("#tbRemarks").val("");
        $('#mdlReject').modal('hide');
        TableProcess.Search();
    },
    DoneUpdateOMPrice: function () {
        Form.BackToListPanel();
        $('#mdlDetail').modal('hide');
        TableProcess.Search();
    },
    CheckDecimal: function (text) {
        var inputnumber = text.value.replace(',', '');
        text.value = Common.Format.FormatCurrency(inputnumber);
    },
    ReplaceDecimal: function (text) {
        var inputnumber = text.value.replace(',', '');
        text.value = Common.Format.FormatCurrency(inputnumber);
        Control.BindProRateAmount();
    },
    UpdateProRate: function () {

    },
    PrintData: function (divName) {
        $('#tbUpdateID').val(Data.Selected.Id);
        $('#tbUpdateAddress').val(Data.Selected.Address);
        $('#tbUpdateSONumber').val(Data.Selected.SONumber);
        $('#tbUpdateSiteId').val(Data.Selected.SiteID);
        $('#tbUpdateSiteName').val(Data.Selected.SiteName);
        $('#tbUpdateSiteIDOpr').val(Data.Selected.CustomerSiteID);
        $('#tbUpdateSiteNameOpr').val(Data.Selected.CustomerSiteName);
        $('#tbUpdateRegionName').val(Data.Selected.RegionalName);
        $('#tbUpdateProvinceName').val(Data.Selected.ProvinceName);
        $('#tbUpdateResidenceName').val(Data.Selected.ResidenceName);
        $('#tbUpdateIpo').val(Data.Selected.PONumber);
        $('#tbUpdateMla').val(Data.Selected.MLANumber);
        $('#tbUpdateStartDateLease').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartBapsDate));
        $('#tbUpdateEndDateLease').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndBapsDate));
        $('#tbUpdateTerm').val(Data.Selected.Term);
        $('#tbUpdateBapsType').val(Data.Selected.BapsType);
        $('#tbUpdateCustomerInvoice').val(Data.Selected.CustomerInvoice);
        $('#tbUpdateCustomerAsset').val(Data.Selected.CustomerID);
        $('#tbUpdateCompanyInvoice').val(Data.Selected.CompanyInvoiceName);
        $('#tbUpdateCompanyAsset').val(Data.Selected.CompanyInvoiceName);
        $('#tbUpdateStipSiro').val(Data.Selected.StipSiro);
        $('#tbUpdateInvoiceType').val(Data.Selected.InvoiceTypeName);
        $('#slUpdateCurrency').val(Data.Selected.BaseLeaseCurrency);
        $('#tbUpdateStartDateInv').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartInvoiceDate));
        $('#tbUpdateEndDateInv').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndInvoiceDate));
        $('#tbUpdateDropFODistance').val(Data.Selected.DropFODistance);

        $('#tbUpdateBaseLease').val(Common.Format.CommaSeparation(Data.Selected.BaseLeasePrice.toString()));
        $('#tbUpdateOMPrice').val(Common.Format.CommaSeparation(Data.Selected.ServicePrice.toString()));
        $('#tbUpdateTotalProRate').val(Common.Format.CommaSeparation(Data.Selected.TotalPaymentRupiah.toString()));

        $('#tbUpdateInflationAmount').val(Common.Format.CommaSeparation(Data.Selected.InflationAmount.toString()));
        $('#tbUpdateAdditionalAmount').val(Common.Format.CommaSeparation(Data.Selected.AdditionalAmount.toString()));
        $('#tbUpdateDeductionAmount').val(Common.Format.CommaSeparation(Data.Selected.DeductionAmount.toString()));
        $('#tbUpdatePenaltyAmount').val(Common.Format.CommaSeparation(Data.Selected.PenaltySlaAmount.toString()));
        $('#slUpdateServiceCurrency').val(Data.Selected.ServiceCurrency);
        $('#slUpdateInflationCurrency').val(Data.Selected.InflationCurrency);
        $('#slUpdateAdditionalCurrency').val(Data.Selected.AdditionalCurrency);
        $('#slUpdateDeductionCurrency').val(Data.Selected.DeductionCurrency);
        $('#slUpdatePenaltyCurrency').val(Data.Selected.PenaltySlaCurrency);
        $('#tbUpdateReconcileDate').val(Common.Format.ConvertJSONDateTime(Data.Selected.ReconcileDate));

        var printContents = document.getElementById(divName).innerHTML;
        var originalContents = document.body.innerHTML;

        document.body.innerHTML = printContents;

        window.print();

        document.body.innerHTML = originalContents;
    },
    CheckObjectEmpty: function (obj) {
        for (var key in obj) {
            if (obj.hasOwnProperty(key))
                return false;
        }
        return true;
    },
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
    UploadDocument: function () {
        if (SiteData.length < 1) {
            Common.Alert.Warning("Can`t Process Upload Document without detail data !"); return;
        }

        Data.RowSelectedUpload = [];
        
        var l = Ladda.create(document.querySelector("#btDocumentUpload"))
        var formData = new FormData();
        vparam = Control.GetParam();

        var CustomerID = vparam.CustomerID;
        var RegionID = vparam.Regional;
        var Batch = vparam.Batch;
        TotalTenant = 0;
        TotalAmount = 0;

        SiteData.forEach(function (entry) {
            TotalTenant = TotalTenant + 1;
            TotalAmount = TotalAmount + entry.TotalAmount;
            //Data.RowSelectedUpload.push(parseInt(entry.RegionID));
        });

        formData.append("Id", Data.RowSelectedSite.toString());
        formData.append("RegionID", RegionID);
        formData.append("CustomerID", CustomerID);
        formData.append("Batch", Batch);
        formData.append("TotalTenant", TotalTenant);
        formData.append("ReconYear", (new Date()).getFullYear());
        formData.append("TotalAmount", TotalAmount); 

        var fileInput = document.getElementById("postedFile1");
        if (document.getElementById("postedFile1").files.length != 0) {

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

                formData.append('File', fsFile);
            }

            errors = false;
        }
        else {
            Common.Alert.Warning("Please Select Document."); return;
        }

        $.ajax({
            url: '/api/ReconcileData/Upload',
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
            if (data !== "Exception") {
                if (data.length <= 0) {
                    $('.panelSearchResult').find('input:file').val('');
                    Common.Alert.Warning("The uploaded file is invalid or empty. Please use the available template, and fill the file correctly with valid product data.");
                } else {
                    Data.RowSelectedUpload = [];
                    Data.RowSelectedSite = [];
                    Form.DeleteSite();
                    TempData = [];
                    $('.panelSearchResult').find('input:file').val('');
                    Common.Alert.Success("Upload File Success!");
                    $('#file-upload-filename').text("");
                    TableUpload.Search();
                }
            } else {
                $('.panelSearchResult').find('input:file').val('');
                Common.Alert.Warning("The uploaded file is invalid. Please download the latest template by clicking the Download Template button.");
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        });
    },
    AddSite: function () {
        var l = Ladda.create(document.querySelector("#btAddSite"))
        var oTable = $('#tblUpload').DataTable();
        var oTableSite = $('#tblSiteUpload').DataTable();

        if (Data.RowSelectedUpload.length == 0) {
            Common.Alert.Warning("Please Select One or More Data")
        }
        else {
            TableSite.Search();

            oTable.rows('.active').every(function (rowIdx, tableLoop, rowLoop) {
                var checkBoxId = this.data().Id;
                $("#Row" + checkBoxId).removeClass('active');
                $("#" + checkBoxId).hide();
            });

            // Hide the checkboxes in the cloned table
            $.each(Data.RowSelectedUpload, function (index, item) {
                $(".Row" + item).removeClass('active');
                $("." + item).hide();
            });

            //insert Data.RowSelectedSite for rendering checkbox
            $.each(Data.RowSelectedUpload, function (index, item) {
                if (Data.RowSelectedSite.indexOf(parseInt(item)) == -1)
                    Data.RowSelectedSite.push(parseInt(item));
            });
        }
        
    },
    DeleteSite: function () {
        Data.RowSelectedUpload = [];
        Data.RowSelectedSite = [];
        var tblSiteData = $("#tblSiteUpload").DataTable();
        tblSiteData.clear().draw();
    },
    GetSelectedSiteData: function (ListID) {

        var AjaxData = [];
        var params = {
            Id: ListID
        };
        var l = Ladda.create(document.querySelector("#btAddSite"));
        $.ajax({
            url: "/api/ReconcileData/SitelistGrid",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                AjaxData = data;
                SiteData = data;
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            alert("fail");
            Common.Alert.Error(errorThrown)
            l.stop();
        });
        return AjaxData;

        //var AjaxData = [];
        //var params = {
        //    Id: ListID
        //}

        //var l = Ladda.create(document.querySelector("#btAddSite"));

        //$.ajax({
        //    url: "/api/ReconcileData/SitelistGrid",
        //    type: "POST",
        //    data: params,
        //    beforeSend: function (xhr) {
        //        l.start();
        //    }
        //}).done(function (data, textStatus, jqXHR) {
        //    if (Common.CheckError.List(data)) {
        //        l.stop();
        //        SiteData = data;
        //    }
        //    l.stop();
        //})
        //.fail(function (jqXHR, textStatus, errorThrown) {
        //    alert("fail");
        //    Common.Alert.Error(errorThrown)
        //    l.stop();
        //});
        
    },
    CancelBulkyAmount: function () {
        $('#mdlBulkyAmount').modal('hide');
        $('#tbBaseLease').val("");
        $('#tbService').val("");
        $('#tbInflation').val("");
        $('#tbAdditional').val("");
    },
    ProcessBulkyAmount: function () {
        
        var l = Ladda.create(document.querySelector("#btYesUpdate"))

        var params = {
            ID: Data.RowSelectedProcess,
            BaseLeasePrice: $("#tbBaseLease").val().replace(/,/g, ""), //== "" ? "0" : $("#tbBaseLease").val().replace(/,/g, ""),
            ServicePrice: $("#tbService").val().replace(/,/g, ""), //== "" ? "0" : $("#tbService").val().replace(/,/g, ""),
            InflationAmount: $("#tbInflation").val().replace(/,/g, ""), //== "" ? "0" : $("#tbInflation").val().replace(/,/g, ""),
            AdditionalAmount: $("#tbAdditional").val().replace(/,/g, "") //== "" ? "0" : $("#tbAdditional").val().replace(/,/g, "")
        }

        var formData = new FormData();

        formData.append("data", JSON.stringify(params));

        //$.ajax({
        //    url: "/api/ReconcileData/UpdateBulkyAmount",
        //    type: "POST",
        //    dataType: "json",
        //    contentType: "application/json",
        //    data: JSON.stringify(params),
        //    cache: false,
        //    beforeSend: function (xhr) {
        //        l.start();
        //    }
        //})
        $.ajax({
            url: '/api/ReconcileData/UpdateBulkyAmount',
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
            if (data != 0) {
                Form.CancelBulkyAmount();
                Data.RowSelectedProcess = [];
                TableProcess.Search();
                $('#mdlBulkyAmount').modal('hide');
                Common.Alert.Success("Update Success !");
            }
            else {
                Common.Alert.Warning("Something Went Wrong. Contact IT Help Desk");
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        });

        
    }
}

var TableSite = {
    Search: function () {
        Form.GetSelectedSiteData(Data.RowSelectedUpload.toString());

        //ajaxData = SiteData;

        if (SiteData.length > 0) {
            $('#tblSiteUpload').show();
            $('#btDocumentUpload').show();
        }
        else {
            $('#tblSiteUpload').hide();
            $('#btDocumentUpload').hide();
        }

        var tblSiteData = $("#tblSiteUpload").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "data": SiteData,
            "filter": false,
            "destroy": true,

            "columns": [
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<button type='button' title='Delete' class='btn btn-xs red btDeleteSite' id='btDeleteSite" + full.Id + "'><i class='fa fa-trash'></i></button>";
                        return strReturn;
                    }
                },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CompanyInvoiceName" },
                { data: "RegionName" },
                { data: "CustomerRegionName" },
                { data: "Term" },
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

                { data: "BaseLeaseCurrency" },
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

                { data: "ServiceCurrency" },
                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

                { data: "DeductionCurrency" },
                { data: "DeductionAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

                { data: "TotalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
            ],
            "buttons": [],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "footerCallback": function (row, data, start, end, display) {

            },

        });

        $("#tblSiteUpload tbody").unbind().on("click", "button.btDeleteSite", function (e) {
            var table = $("#tblSiteUpload").DataTable();
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
            Helper.RemoveElementFromArray(Data.RowSelectedUpload, parseInt(id));
            Helper.RemoveElementFromArray(Data.RowSelectedSite, parseInt(id));
            if (Data.RowSelectedSite.length == 0) {
                $('#tblSiteUpload').hide();
                $('#btDocumentUpload').hide();
                //TableUpload.Search();
            }
        });

        $(window).resize(function () {
            $("#tblSiteData").DataTable().columns.adjust().draw();
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
            "data": []
        });

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

        $("#tblProcess tbody").on("click", "button.btEdit", function (e) {
            e.preventDefault();
            var table = $("#tblProcess").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                Form.UpdateOMPricePanel();
            }
        });

        $(window).resize(function () {
            $("#tblProcess").DataTable().columns.adjust().draw();
        });
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val().trim();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val().trim();
        fsRenewalYear = $("#slRenewalYear").val() == null ? "" : $("#slRenewalYear").val().trim();
        fsRenewalYearSeq = $("#slRenewalYearSeq").val() == null ? "" : $("#slRenewalYearSeq").val().trim();
        fsRegional = $("#slRegional").val() == null ? "" : $("#slRegional").val().trim();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val().trim();
        fsDueDatePerMonth = $("#slDueDatePerMonth").val() == null ? "" : $("#slDueDatePerMonth").val().trim();
        fsReconcileType = $("#slReconcileType").val() == null ? "" : $("#slReconcileType").val().trim();
        fsProvince = $("#slProvince").val() == null ? "" : $("#slProvince").val().trim();
        var TenantType = $("#slTenantType").val() == null ? "" : $("#slTenantType").val().trim();

        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strRenewalYear: fsRenewalYear,
            strRenewalYearSeq: fsRenewalYearSeq,
            strRegional: fsRegional,
            strCurrency: fsCurrency,
            strDueDatePerMonth: fsDueDatePerMonth,
            Batch: $("#slBatch").val() == null ? "" : $("#slBatch").val(),
            strSONumber: $("#slSONumber").val() == null ? "" : $("#slSONumber").val(),
            strReconcileType: fsReconcileType,
            strProvince: fsProvince,
            TenantType: TenantType,
            isRaw: 1
        };
        var tblProcess = $("#tblProcess").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/ReconcileData/grid",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    extend: 'pdf',
                    orientation: 'landscape',
                    pageSize: 'LEGAL',
                    className: 'btn blue btn-outline',
                    exportOptions: { columns: [2, 3, 5, 4, 15, 21, 14, 23, 24, 22, 25, 26, 27, 28] },
                    text: '<i class="fa fa-file-pdf-o" title="PDF"></i>',
                    titleAttr: 'Export To PDF',
                    customize: function (doc) {
                        //Remove the title created by datatTables
                        doc.content.splice(0, 1);
                        //Create a date string that we use in the footer. Format is dd-mm-yyyy
                        var now = new Date();
                        var jsDate = now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();
                        // Logo converted to base64
                        // var logo = getBase64FromImageUrl('https://datatables.net/media/images/logo.png');
                        // The above call should work, but not when called from codepen.io
                        // So we use a online converter and paste the string in.
                        // Done on http://codebeautify.org/image-to-base64-converter
                        // It's a LONG string scroll down to see the rest of the code !!!                        
                        var logo = 'data:image/jpg;base64,/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAABQAAD/4QMpaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjAtYzA2MCA2MS4xMzQ3NzcsIDIwMTAvMDIvMTItMTc6MzI6MDAgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCBDUzUgV2luZG93cyIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDozQTFDNjMzNEQxQ0UxMUUxODVFQTlEQTgzMUI2Njg4MyIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDozQTFDNjMzNUQxQ0UxMUUxODVFQTlEQTgzMUI2Njg4MyI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOjNBMUM2MzMyRDFDRTExRTE4NUVBOURBODMxQjY2ODgzIiBzdFJlZjpkb2N1bWVudElEPSJ4bXAuZGlkOjNBMUM2MzMzRDFDRTExRTE4NUVBOURBODMxQjY2ODgzIi8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+/+4ADkFkb2JlAGTAAAAAAf/bAIQAAgICAgICAgICAgMCAgIDBAMCAgMEBQQEBAQEBQYFBQUFBQUGBgcHCAcHBgkJCgoJCQwMDAwMDAwMDAwMDAwMDAEDAwMFBAUJBgYJDQsJCw0PDg4ODg8PDAwMDAwPDwwMDAwMDA8MDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwM/8AAEQgARwDRAwERAAIRAQMRAf/EAKEAAQABBAMBAQAAAAAAAAAAAAAGBQcICQEDBAIKAQEAAwACAwAAAAAAAAAAAAAABQYHAwQBAggQAAAFAwMEAQMCAwcFAAAAAAECAwQFAAYHERITIVFhCDFBIhQyCUIzFXGBUmJyoiSRsSNzFhEAAgECAwQHBwMFAQAAAAAAAAECEQMhBAUxcRIGQVFhgZEiB6GxwdEyQhNSchThYoIjFTP/2gAMAwEAAhEDEQA/ANy/F/l/7UB88XigohxeKCiHF4oKIcXigohxeKCiHF4oKIcXigohxeKCiHF4oKIcXigohxeKCiHF4oKIcXigHF4oBxeKAcXigHF4oBxeKAcXigHF4oBxeKAcXigHF4oBxeKAcXigHF4oD3bA/wANAOMO1AOMO1AW3yfk+0sSW8FxXY5UTRWV/Hj2TcvIu5W0E2xMuoB0ANRER0AKmNE0PM6vf/DYWKVW3siutkVq+sZfS7P5bzw2JLa31Ig+LPY/GWVlf6fFyB4SfEQBOBlRIisrr0DhMBhIp/YUdfFSmucmahpK45x47f6o4pb+le4j9H5ryWpvhhLhn+mWDe7oZkBxh2qpllNXnvzG3xjO47A9gcfXBJQ6zZwlDXAg3crA3MokJnDQVUSn2GIoUqhDlENo6Br1GtZ9OrmW1Cze03MwjJNOUapVxwlR7arBplY16NyxKGYttrofwNgmLb7i8pY9tK/4guxnc0em6FDXUUVupF0R8pqlMX+6s31bTp6dm7mWnthJreuh96xLBlb6v2o3FsaJsZdmV0mxM5SK9VTMqkzFQoKmTKIAY5Sa7hABEAEdK6KtyceKjosK9HicrnFS4aqvV0np4w7V6HsOMO1AOMO1AOMO1AOMO1AOMO1AOMO1AOMO1AOMO1AOMO1AOMO1AOMO1AOMO1AOMO1AOMO1AOMO1AOMO1AOMO1Ae7iGgHGPagHGPagIreNiWrf8OpA3fCNpyLUNvBBcB3JnABADpHKIHIYAH9RRAa72n6nmdPuq7l5uEuz3NbGuxnTz2n2M9b/HfgpR7fh1Pca5Ms+j8/BGdXBiaRUm2SIisW2XJgJII6ddG6waFV0+gDtN/qGtg0H1Ks36Ws/Hgk8ONfQ/3LbH2rcZZrXp/ds1u5KXEtvC/qW59Pse8jeNPbTJGLJD/wCRyrFvbijWBgQVTelFCXZAT7dAOoAcoBp8Kdex67es8hZHVYfyMjKMJPHDG3Lw2f4+B1tJ51zmmz/BnIucVhjhOPjt7/Ezfukcde0+G7yta25ppMIz0YdNAhh2uGD8ocrRRZE33pimsUpuodQ8DWX2LOd5a1G3dvQcXGXdKOyVHsdVU06znsprGWkrM1JNd8X0VW1YmM/7erHLNmxN+YvyJZc1b8VAuyv4B9ItzoolWWEU3TZJQwbT6iUqgbREP1D9asvqRPIZu5azeWuxlKSpJJ1eH0ya6Oo6+gK9ajK1ci0liq+1HUhcMvln3UZJxzpwyh8crOWRRRExdUIzeDkFBLp9q7gRKOvyUQCpWWTt6Tyo3NJzvpPHrnThp+2OO+pTI5q5qfMqUW1GzVYdUK8Vf3Sw3UNk/GPascNVHGPagLHexry5IHD16XTatzPLXmrUj1ZVo7ZkbqcwoFEeFUrhJUNhteu3QfNAWubGv27bttTDxcoTMKzZ2Ohed0Xe1KzTnpRSQdnQSbIn/HFFBJDT7jES3D9pR7iBGpy/sjWInli0VbudXSphde2bqSudVJAHzqBfOQF/HSAJplTMdNuRQ3IUpTGLoPQaAjBs5ZAlpu54JlPGRJlW5ItLBDxAqQiEOhKnj5RZIeMdwcbcyv3a/aYDdNaAn9nXXkGczbfLB2/vl5a9t3e4iWQRzWKG20G6TJBYEny6pAd7gMruNxj8CXr80BDfVvKeS8hX45Yy09NzkVGxL5xeSMy2YNmaC6z0SRSkSduUq6pFEk1QPvDTp866BQGwHjHtQDjHtQDjHtQDjHtQDjHtQDjHtQDjHtQDjHtQDjHtQDjHtQFQ4QoBwhQDhCgHCFAOEKAtfkjC+PsqsDs7vgUnTkCbWsyiAJPUNB1DjXL92gD/AAjqXxU3o/MOd0qfFl5tLpi8YvevjtIjVdDympQ4b8E30S2SW5lnMG+qbDCt6Tl1t7tczqL1kZhFMFEAQFJJQ5DnMuYpxBQ2pAAuhSgHWrBzNzvPWsrCw7Sg0+KTrWrSaXDhgsccWQfL3KENIzM7yuOSaolSlFtx63h2GWfCFUQuZa6et60sdp3vleGsgH90FiVl5YkUmQj2QTbhzmTKBjFIJzCUBER6joGutTFnN5nPK1krl6lviXDxPyxrhXrovYR38LL5a5PMwtr8jWLW2XSa64f9xW/79uqPhca+v7ubaKuEiPWyazh+94BOBTnAGyJCEHT/ABagH1GtGv8AprlcnYlPNZtRdHTBRjXo+ptvuIeHMFy7NK3ar7X7DbEmnvIQ4kMmJygYSG0AxdQ10HQR6hWRNULSRy8rNh77tads6fIspC3G0UZSREFBSUFJQNDAU4dSj5rwCF3nhO07zQt4yjmVt2ctRt+HAXZBvDspNBsJQIdAVyfzEz7QExDgIahu+aA+IPBdhwdnXZZhGzySa34g4QvSbkHSjmTkhdJCgc67s47xMUhtpNNAKH6QCgOhlgHHMc4xY5aRrhJbDiCzayT/AJBx403CXEpza/zREOuo/XrQHDHBNqxd9SN/x01crGSl5M8tKwyMssSKculEypGMqyD/AMZtSFKGg9g7UB67PwfYtiyFvSluN3jR9bcW9hmzg7k6hl2L11+YZFzr/NBNYRMnr1JqIB0oC7XCFAOEKAcIUA4QoBwhQDhCgHCFAOEKAcIUA4QoD2bKA42eKAp8rJxcFHO5eakG0RFMExVfSTxYiCCKYfJlFFBKUoeRGuS1ZndmoQTlJ7ElVvuPWUlFVbojEW8ffT1mtATJkvZS6nJR2mbQLRV1oP8A7Tgkl/eBxq4ZL0/1jM4/i4F/e0vZi/YRV7XMrb+6u7ExbvT90+Ab8yOP8YO5I5TaJP5x2VsmYO/CgChv99WvI+k92VHmL6XZFV9rp7iNvczRX/nBveYrXb+437GXEmu3iHsLZyCgiKakYxBRwQO3K6MsH+2rbk/TXSbDTmpXH/dLDwjT3kZd5hzU9lI7l8zMP9vbOOb8kXBd8BkdxL3VbP8ATglIi7X7cQTbuSrETM2K4AhSmBUqgmAuo6bB06VTPUXQdNyNq3cyyjCfFwuCe1UrxU7KUr2ktoOdzF6Uo3KtUqn8Daps8VkxZj5OUoEOJw1IACJg+en16V5W0Mxmw97AYYvKQkLWtlBtZM4i7WIEA4QRZA7EhzFFRAyWiahh01Euu7r8DVv17lTU8lCN67W5BpeZNy4ex1xW/YVjSOaMhnpu1B8E035XRV3dD3bTJvZ4qnlnMWval9cLS38dx1uuZ9Je4byZxzxjbL0jCTeIHbuDmQQcKCBCCIkAdTDpoFAY3S16ZFuuwcAQxZu75m7HFxXFA33D26+RhZxRxFILHBo4cqhwio2KQgqH02qaCYum6gOvNVwX7btyY4tuMmL+gpVex2CMKh/Wm2jK4X0qdozc3G5EhkVkROJSKGANB+PqFAXnust1WpnC07pyLdlwpWbKhDRUC7t1/wAUNHTSiZknDKZjhKbek/VMAorj+kdCdOg0Bb/LHsMFuewBHbTISDC1sZPoi3J/H+8RPMKzJzBJOilAdB/AIqjpr8GKcP7QJVkHM85Znssm+dyyrbEFpxMVAXyiY4i1Se3Em9eMn2gfaG0zZJMxx+CnDvQFJwtnC64Sz8+XnlVy4XVh0W1925CuVBEW0TNtjqxzEvQNmoplLp9BNQFd9RsrPLifXZY1zZFa5Cnjt4+646SRVE/CSTQD8+NDXTT8JymYClD+AwD4ADOLZ4oBs8UA2eKAbPFANnigGzxQDZ4oBs8UBUOLxQHHEXtQGLXuTiS4cw4Duy1bSBRa5GqjaWio1M+z807I+87YdRABE5BNtAR/WBatXJerWtM1O3evfQ6xb/Txfd3Pb2VI3VsrLMZeUI7du+nQaF7Q9SPYq9ZH+nRGKZtqJVRSXeyiP9PbJGAdB3rOdhf+mtb9nOb9KysOKd+L7Ivib7kUi1peZuOig+/Azox3+1XPOTtneUsjNItuIFO4hreRM5XHX9SYuXAETIIdwIcKoeo+q9uNY5Wy2+ubot9Fj7UTWX5Zk8bs6di+Znxjz0j9bsbnbOo3Hre4JZqH2TFxKGk1RMHwfiV/45TB9BIkWqBqPPGrZ6qlecYvoh5V4rzeLZOZfR8rZxUavreP9DIufkoayrYmrgeJpsoe249d86IkUpClRbJicSlKGga6F0AKrmVy9zOX4Wo4ynJJb26HbzN+GWsyuywjFNvclUwX9b/bW8ssZGVsi47WZnaSZXTyNko3emdigiG4pHBTmMChdNC7w2jqIdB16aTzbyLldKyX8mzcdY0TUvub/TTZ10xwKByxzlmNSzf4LttUdWnH7Uuvr34Hq9l/cOb9fc049sZ1Z7VxYk81bPriuZwdTnFBdwo3WBoBRKQotwIBzbt27XTQvzUbyzyZb1nT719XH+WLajFbKpJri6fNsVKFr1HVpZS/GDj5Xtfy3FyctepGM8sIqXNbYls27H5Su21wxpf+M6Mcu4p124CUo79QETk2m+o7viuDQuec9pbVm7/stLDhltj+2XZ1Oq3EVrXJuT1Gty3/AK7rx4o7HvXxVHvLhYBtTKdn2k/tzK02S45CLkTo2/MFW5xWjwITjExzFBT9W7QFPuAPGlRXNGc0/N5iN7JQ4IyjWUaUpOuOGzwwJHl3K57K2HazkuNxl5XWtY+/xxJ1kDGFm5PiWkLecarIsWDxOQZA3duWSqTlIDARQizVRJQBADD8GqtFgIO/9acNyNs27aKtqHbwdqunD6ESZyL9quk5dgIOFjOkXBF1DqajuMc4iNAVtzgvGT1ieOfW8Z+1UtoLRU/KeO1jmiQW/IKiKh1ROJiq/eCgjyAOggbpQHge+vmLpC7Wt6PYV05mkFmLpwRSReGaO3cYQCMnTxoK3CusiABtOoUR16jqPWgJGjiPHqFuXLaYW03Vg7wXkHVytljKKndrShhM7UUWOYVNTib5A329NumgUBTJHBuMZeGuiAlLaLIRl5JRqNyIruHBzOSRCaSTEBUFTeUUiok0EogIiGo6iI0B6pXDOOZp68fyFuEVWkE4dF+mRddNFZKBWFxHJnSIcpBIioOu3TQ3QDagFAVh9jazJC7ravpxCJhdNoovG8DKInURFJJ8nxLkOmmYpFAMX4A5TbR6l0GgJrxF7UA4i9qAcRe1AOIvagHEXtQDiL2oBxF7UA4i9qAqGwaAcdAOOgHHQDjoBx0BgH795JC3MfRWPY95xSl7OQWk0iD939NaDuEDdgUW2h52mCtN9MtI/kZyWakvLaVF++XyVfFGe+oWqfgykctF+a48f2r5ungy3/7dthqFQvjIrpAATXMlBxCxigIjs0XciU30+UwGpP1U1JN2cpF7Kzfuj8SO9N8g0ruZa20ive/gTb9xrC5Mi4QWvSNYivc+LVDSbdRMobzxqu0j9IR+RKUoFV0DrqTyNQHpxrTyOoqzJ0he8v8Al9r+HeXbX8p+axxpeaOPd0/MrX7fOZT5XwWwhZZ8Dq6sbKFg5TeYRVO0AurFY+7qO5MBJrr1Eg1w+oWi/wDP1JzgqW7vmXVX7l447mjk0POfny6TeMcPkZ2cdUQmTHL2ZnpeDs62GLG4XFmRN23bEwN2Xq0MCa8ZGvFRBVRNUf5RlTFKlyfw79aAt5bLTHGJs22/ZVvQuSGEjdqDlnHunEgeRtuVFJsDtV2b8p8srubkAQFQqZQAREB16UBjJ69XZkMkzkOdhyy6sta9oXVNLR0tMOpBK7FxfLliVGEeoAptytFG50VBTNuETAAh1oC4ElbcFB+uLH2Pi8nzjjLoQrO4SXkpMOVUnssvxipFHjhV/HMkY5hbcJUwEv8AqAaAkt7XfOEgfcx2/l3cHJsbStd9GsPyTpmj1XkIIm/G+77BMuBg1J8mDvQEKlILKl9ew1wxNmOJUru24iwXxbmPcKzBnAEVR5XxlIsNxX4vE0jk2CXQo9RHrQGzvjoBx0A46AcdAOOgHHQDjoBx0A46AcdAdtAKAUAoBQHBjAUBMYQKUoamMPQAAPqNAfn59nckhlLMl0TbNUysNHKhEQAajoLZmIk3lARHTkPuP0719Pcn6T/zdNt25fVJcUt8sfYqI+d+atT/AOhqE5r6V5Y7o/N4m5f1wsc2PcLWHbzhuVtImjyyEuQA0N+S9EVzgf8AzFA4EH/TWB816j/P1O9dTrHiot0cPbSvebXy1kP4Wn2rTVHSr3yx/oXmfMmskyeRz5Arlk/QUbPGxw1KokqUSHIYPqAlEQGq/CbhJSi6NOqJxpNUZrU9QPUrJWA87ZWuJ+/RZ4zcoOIu2WhFiqKSqSjhNw0cHSII8f46YGJ9/wB24xgD7eo6XzfzblNX02xbim7yalJ0+h0akq9PE8cOor+laXdyuYnJvybF29XgbNazIsJS5uEh7kin0HcEW1mYaTSFGQjHiRVkFkzfJTkOAgNAQCxsJYrxu/XlLLs1nDSbhAGppHes5XK3AdeFNRyoqZNPX+EggXxQFZhsZWFbzmDeQlrsot3bZH6UG4blMQ7dOTV53hCiA9SqqfeYo6hu6h1oCKtvXvCzO6S3m2x3FJXAm8NIpOAKoKCbwwdXJGgnFuVX67ypgbXrrrQFSu7CmK78uGPuu7rJYTlwRhEk20ivyAJiIKcqRViEOUixSH+4oKFMAD8UBMWNqW5GXBN3UwiG7S4bjRaN5yWTAQVcpMSmK2IcddNEwOYA6fWgJDQCgFAKAUAoBQCgFAKAUA60A60A60A60BTZlqk+iJVk4ciyQeM10F3hTAQUiKJmKZQDD0DaA661y2JuFyMkqtNOnXjsOO9BThKLdE01XqNNFq+s1jR9+Qb2a9icWSdoMZVFw8aI3Ch+e5bpKgfh4jACYGUANo/f016a1vmd5uzVzKTjbyOZjdcWk3bfCm1trtw2rAxTJ8r5aGahKecsO2pJtca4mk9lNmO83UB8Bppp9NK+fjbznrQDrQDrQDrQDrQDrQDrQDrQDrQDrQDrQDrQDrQDrQDrQDrQDrQDrQDrQH//2Q==';
                        // A documentation reference can be found at
                        // https://github.com/bpampuch/pdfmake#getting-started
                        // Set page margins [left,top,right,bottom] or [horizontal,vertical]
                        // or one number for equal spread
                        // It's important to create enough space at the top for a header !!!
                        doc.pageMargins = [20, 60, 20, 30];
                        // Set the font size fot the entire document
                        doc.defaultStyle.fontSize = 7;
                        // Set the fontsize for the table header
                        doc.styles.tableHeader.fontSize = 7;
                        // Create a header object with 3 columns
                        // Left side: Logo
                        // Middle: brandname
                        // Right side: A document title
                        doc['header'] = (function () {
                            return {
                                columns: [
									//{
									//    image: logo,
									//    width: 200
									//},
									{
									    alignment: 'left',
									    italics: true,
									    text: 'RA System',
									    fontSize: 18,
									    margin: [10, 0]
									},
									{
									    alignment: 'right',
									    fontSize: 14,
									    text: 'List Reconcile'
									}
                                ],
                                margin: 20
                            }
                        });
                        // Create a footer object with 2 columns
                        // Left side: report creation date
                        // Right side: current page and total pages
                        doc['footer'] = (function (page, pages) {
                            return {
                                columns: [
									{
									    alignment: 'left',
									    text: ['Created on: ', { text: jsDate.toString() }]
									},
									{
									    alignment: 'right',
									    text: ['page ', { text: page.toString() }, ' of ', { text: pages.toString() }]
									}
                                ],
                                margin: 20
                            }
                        });
                        // Change dataTable layout (Table styling)
                        // To use predefined layouts uncomment the line below and comment the custom lines below
                        // doc.content[0].layout = 'lightHorizontalLines'; // noBorders , headerLineOnly
                        var objLayout = {};
                        objLayout['hLineWidth'] = function (i) { return .5; };
                        objLayout['vLineWidth'] = function (i) { return .5; };
                        objLayout['hLineColor'] = function (i) { return '#aaa'; };
                        objLayout['vLineColor'] = function (i) { return '#aaa'; };
                        objLayout['paddingLeft'] = function (i) { return 4; };
                        objLayout['paddingRight'] = function (i) { return 4; };
                        doc.content[0].layout = objLayout;
                    }
                },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        TableProcess.Export();
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            //buttons: ['copy','csv','excel','pdf','print'],
            "filter": false,
            "lengthMenu": [[25, 50, 100, 200, 7000], ['25', '50', '100', '200', '7000']],
            "destroy": true,
            "columns": [

                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Helper.IsElementExistsInArray(full.Id, Data.RowSelectedProcess)) {
                                $("#Row" + full.Id).addClass("active");
                                strReturn += "<label id='" + full.Id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                            }
                            else {
                                strReturn += "<label id='" + full.Id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                },
                {
                    mRender: function (data, type, full) {
                        var strReturn = "<button type='button' title='Edit' class='btn btn-xs green btEdit'><i class='fa fa-edit'></i></button>";
                        return strReturn;
                    }
                },
                { data: "RowIndex" },
                {
                    data: "SONumber", mRender: function (data, type, full) {
                        return "<a class='btDetail2'>" + data + "</a>";
                    }
                },

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
                {
                    data: "RFIDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "BaufDate", render: function (data) {
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
                { data: "Currency" },
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ServiceCurrency" },
                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InflationAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AdditionalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "DeductionAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "TotalPaymentRupiah", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

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
    },

    Reset: function () {
        $("#slSearchCompanyName").val("").trigger('change');
        $("#slSearchOperator").val("").trigger('change');
        $("#slRenewalYear").val("").trigger('change');
        $("#slRenewalYearSeq").val("").trigger('change');
        $("#slReconcileType").val("").trigger('change');
        $("#slSearchCurrency").val("").trigger('change');
        $("#slRegional").val("").trigger('change');
        $("#slProvince").val("").trigger('change');
        $("#slDueDatePerMonth").val("").trigger('change');
        $("#slBatch").val("").trigger('change');

        fsCompanyId = "";
        fsOperator = "";
        fsRenewalYear = "";
        fsRenewalYearSeq = "";
        fsReconcileType = "";
        fsCurrency = "";
        fsRegional = "";
        fsProvince = "";
        fsDueDatePerMonth = "";
    },
    ShowDetail: function () {
        $('#tbDetailAddress').val(Data.Selected.Address);
        $('#tbDetailID').val(Data.Selected.Id);
        $('#tbDetailSONumber').val(Data.Selected.SONumber);
        $('#tbDetailSiteId').val(Data.Selected.SiteID);
        $('#tbDetailSiteName').val(Data.Selected.SiteName);
        $('#tbDetailSiteIDOpr').val(Data.Selected.CustomerSiteID);
        $('#tbDetailSiteNameOpr').val(Data.Selected.CustomerSiteName);
        $('#tbDetailRegionName').val(Data.Selected.RegionalName);
        $('#tbDetailProvinceName').val(Data.Selected.ProvinceName);
        $('#tbDetailResidenceName').val(Data.Selected.ResidenceName);
        $('#tbDetailIpo').val(Data.Selected.PONumber);
        $('#tbDetailMla').val(Data.Selected.MLANumber);
        $('#tbDetailStartDateLease').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartBapsDate));
        $('#tbDetailEndDateLease').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndBapsDate));
        $('#tbDetailTerm').val(Data.Selected.Term);
        $('#tbDetailBapsType').val(Data.Selected.BapsType);
        $('#tbDetailCustomerInvoice').val(Data.Selected.CustomerInvoice);
        $('#tbDetailCustomerAsset').val(Data.Selected.CustomerID);
        $('#tbDetailCompanyInvoice').val(Data.Selected.CompanyInvoiceName);
        $('#tbDetailCompanyAsset').val(Data.Selected.CompanyInvoiceName);
        $('#tbDetailStipSiro').val(Data.Selected.StipSiro);
        $('#tbDetailInvoiceType').val(Data.Selected.InvoiceTypeName);
        $('#tbDetailCurrency').val(Data.Selected.Currency);
        $('#tbDetailStartDateInv').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartInvoiceDate));
        $('#tbDetailEndDateInv').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndInvoiceDate));
        $('#tbDetailBaseLease').val(Common.Format.CommaSeparation(Data.Selected.BaseLeasePrice.toString()));
        $('#tbDetailOMPrice').val(Common.Format.CommaSeparation(Data.Selected.ServicePrice.toString()));
        $('#tbDetailDeductionAmount').val(Common.Format.CommaSeparation(Data.Selected.DeductionAmount.toString()));
        $('#tbDetailTotalProRate').val(Common.Format.CommaSeparation(Data.Selected.TotalPaymentRupiah.toString()));
        $('#tbDetailInflation').val(Common.Format.CommaSeparation(Data.Selected.InflationAmount.toString()));
        $('#tbDetailAdditional').val(Common.Format.CommaSeparation(Data.Selected.AdditionalAmount.toString()));
        $('#tbDetailDistance').val(Data.Selected.DropFODistance);

        $('#tbDetailOMPrice').attr("disabled", "disabled");
        //$("#tbDetailOMPrice").removeAttr("disabled");
        $("#btUpdateAmount").hide();
        $("#btDismiss").show();
    },
    Export: function () {
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsRenewalYear = $("#slRenewalYear").val() == null ? "" : $("#slRenewalYear").val();
        fsRenewalYearSeq = $("#slRenewalYearSeq").val() == null ? "" : $("#slRenewalYearSeq").val();
        fsRegional = $("#slRegional").val() == null ? "" : $("#slRegional").val();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
        fsDueDatePerMonth = $("#slDueDatePerMonth").val() == null ? "" : $("#slDueDatePerMonth").val();
        fsReconcileType = $("#slReconcileType").val() == null ? "" : $("#slReconcileType").val();
        fsProvince = $("#slProvince").val() == null ? "" : $("#slProvince").val();
        var lengthdata = $('#tblProcess tr').length;
        var Batch = $("#slBatch").val() == null ? "" : $("#slBatch").val();
        var TenantType = $("#slTenantType").val() == null ? "" : $("#slTenantType").val();
        var SONumber = $("#slSONumber").val() == null ? "" : $("#slSONumber").val();
        var SoNumberFilter = "";

        if (SONumber != null && SONumber != "") {
            SoNumberFilter = "0";
            for (var i = 0; i < SONumber.length; i++) {
                SoNumberFilter += ("," + SONumber[i].toString());
            }
        }

        window.location.href = "/RevenueAssurance/Input/Export?strCompanyId=" + fsCompanyId + "&strOperator=" + fsOperator
            + "&strRenewalYear=" + fsRenewalYear + "&strReconcileType=" + fsReconcileType + "&strRenewalYearSeq=" + fsRenewalYearSeq + "&strCurrency=" + fsCurrency
        + "&strDueDatePerMonth=" + fsDueDatePerMonth + "&strRegional=" + fsRegional + "&Batch=" + Batch + "&TenantType=" + TenantType + "&SONumber=" + SoNumberFilter + "&strProvince=" + fsProvince + "&length=" + lengthdata + "&isRaw=1";
    }
}

var TableRaw = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblRaw = $('#tblRaw').DataTable({
            "filter": false,
            "destroy": true,
            "searchable": true,
            "data": []
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

        $("#tblRaw tbody").on("click", "button.btEdit", function (e) {
            e.preventDefault();
            var table = $("#tblRaw").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                Form.UpdateOMPricePanel();
            }
        });


        $(window).resize(function () {
            $("#tblRaw").DataTable().columns.adjust().draw();
        });
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val().trim();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val().trim();
        fsRenewalYear = $("#slRenewalYear").val() == null ? "" : $("#slRenewalYear").val().trim();
        fsRenewalYearSeq = $("#slRenewalYearSeq").val() == null ? "" : $("#slRenewalYearSeq").val().trim();
        fsRegional = $("#slRegional").val() == null ? "" : $("#slRegional").val().trim();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val().trim();
        fsDueDatePerMonth = $("#slDueDatePerMonth").val() == null ? "" : $("#slDueDatePerMonth").val().trim();
        fsReconcileType = $("#slReconcileType").val() == null ? "" : $("#slReconcileType").val().trim();
        fsProvince = $("#slProvince").val() == null ? "" : $("#slProvince").val().trim();
        var TenantType = $("#slTenantType").val() == null ? "" : $("#slTenantType").val().trim();

        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strRenewalYear: fsRenewalYear,
            strRenewalYearSeq: fsRenewalYearSeq,
            strRegional: fsRegional,
            strCurrency: fsCurrency,
            strDueDatePerMonth: fsDueDatePerMonth,
            Batch: $("#slBatch").val() == null ? "" : $("#slBatch").val(),
            strSONumber: $("#slSONumber").val() == null ? "" : $("#slSONumber").val(),
            strReconcileType: fsReconcileType,
            strProvince: fsProvince,
            TenantType: TenantType,
            isRaw: 0
        };


        var tblRaw = $("#tblRaw").DataTable({
            "columnDefs": [
                { "searchable": true, "targets": 0 }
            ],
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/ReconcileData/grid",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [

                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    extend: 'pdf',
                    orientation: 'landscape',
                    pageSize: 'LEGAL',
                    className: 'btn blue btn-outline',
                    exportOptions: { columns: [1, 2, 4, 3, 14, 20, 13, 22, 23, 21, 24, 25, 26, 27] },
                    text: '<i class="fa fa-file-pdf-o" title="PDF"></i>',
                    titleAttr: 'Export To PDF',
                    customize: function (doc) {
                        //Remove the title created by datatTables
                        doc.content.splice(0, 1);
                        //Create a date string that we use in the footer. Format is dd-mm-yyyy
                        var now = new Date();
                        var jsDate = now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();
                        // Logo converted to base64
                        // var logo = getBase64FromImageUrl('https://datatables.net/media/images/logo.png');
                        // The above call should work, but not when called from codepen.io
                        // So we use a online converter and paste the string in.
                        // Done on http://codebeautify.org/image-to-base64-converter
                        // It's a LONG string scroll down to see the rest of the code !!!                        
                        var logo = 'data:image/jpg;base64,/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAABQAAD/4QMpaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjAtYzA2MCA2MS4xMzQ3NzcsIDIwMTAvMDIvMTItMTc6MzI6MDAgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCBDUzUgV2luZG93cyIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDozQTFDNjMzNEQxQ0UxMUUxODVFQTlEQTgzMUI2Njg4MyIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDozQTFDNjMzNUQxQ0UxMUUxODVFQTlEQTgzMUI2Njg4MyI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOjNBMUM2MzMyRDFDRTExRTE4NUVBOURBODMxQjY2ODgzIiBzdFJlZjpkb2N1bWVudElEPSJ4bXAuZGlkOjNBMUM2MzMzRDFDRTExRTE4NUVBOURBODMxQjY2ODgzIi8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+/+4ADkFkb2JlAGTAAAAAAf/bAIQAAgICAgICAgICAgMCAgIDBAMCAgME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';
                        // A documentation reference can be found at
                        // https://github.com/bpampuch/pdfmake#getting-started
                        // Set page margins [left,top,right,bottom] or [horizontal,vertical]
                        // or one number for equal spread
                        // It's important to create enough space at the top for a header !!!
                        doc.pageMargins = [20, 60, 20, 30];
                        // Set the font size fot the entire document
                        doc.defaultStyle.fontSize = 7;
                        // Set the fontsize for the table header
                        doc.styles.tableHeader.fontSize = 7;
                        // Create a header object with 3 columns
                        // Left side: Logo
                        // Middle: brandname
                        // Right side: A document title
                        doc['header'] = (function () {
                            return {
                                columns: [
									{
									    image: logo,
									    width: 24
									},
									{
									    alignment: 'left',
									    italics: true,
									    text: 'RA System',
									    fontSize: 18,
									    margin: [10, 0]
									},
									{
									    alignment: 'right',
									    fontSize: 14,
									    text: 'List Reconcile'
									}
                                ],
                                margin: 20
                            }
                        });
                        // Create a footer object with 2 columns
                        // Left side: report creation date
                        // Right side: current page and total pages
                        doc['footer'] = (function (page, pages) {
                            return {
                                columns: [
									{
									    alignment: 'left',
									    text: ['Created on: ', { text: jsDate.toString() }]
									},
									{
									    alignment: 'right',
									    text: ['page ', { text: page.toString() }, ' of ', { text: pages.toString() }]
									}
                                ],
                                margin: 20
                            }
                        });
                        // Change dataTable layout (Table styling)
                        // To use predefined layouts uncomment the line below and comment the custom lines below
                        // doc.content[0].layout = 'lightHorizontalLines'; // noBorders , headerLineOnly
                        var objLayout = {};
                        objLayout['hLineWidth'] = function (i) { return .5; };
                        objLayout['vLineWidth'] = function (i) { return .5; };
                        objLayout['hLineColor'] = function (i) { return '#aaa'; };
                        objLayout['vLineColor'] = function (i) { return '#aaa'; };
                        objLayout['paddingLeft'] = function (i) { return 4; };
                        objLayout['paddingRight'] = function (i) { return 4; };
                        doc.content[0].layout = objLayout;
                    }
                },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        TableRaw.Export();
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[25, 50, 100, 200, 7000], ['25', '50', '100', '200', '7000']],
            "destroy": true,
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (full.StatusRecon <= 3) {
                                if (Helper.IsElementExistsInArray(full.Id, Data.RowSelectedRaw)) {
                                    $("#Row" + full.Id).addClass("active");
                                    strReturn += "<label id='" + full.Id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.Id + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                }
                                else {
                                    strReturn += "<label id='" + full.Id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.Id + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                                }
                            }
                            else {
                                strReturn += "<label id='" + full.Id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.Id + "'><input type='checkbox' class='checkboxes' disable/><span></span></label>";
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
                { data: "RowIndex" },
                {
                    data: "SONumber", mRender: function (data, type, full) {
                        return "<a class='btDetail'>" + data + "</a>";
                    }
                },


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
                {
                    data: "RFIDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "BaufDate", render: function (data) {
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
                { data: "Currency" },
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ServiceCurrency" },
                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InflationAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AdditionalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "DeductionAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "TotalPaymentRupiah", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                //{ data: "ISNULL(fnCalculateTotalHargaReconcile(invoiceStartDate, invoiceEndDate, basePrice, omPrice, 0, SoNumber, SiteID), 0)", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '')},

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
        //debugger;
        $('#tbDetailAddress').val(Data.Selected.Address);
        $('#tbDetailID').val(Data.Selected.Id);
        $('#tbDetailSONumber').val(Data.Selected.SONumber);
        $('#tbDetailSiteId').val(Data.Selected.SiteID);
        $('#tbDetailSiteName').val(Data.Selected.SiteName);
        $('#tbDetailSiteIDOpr').val(Data.Selected.CustomerSiteID);
        $('#tbDetailSiteNameOpr').val(Data.Selected.CustomerSiteName);
        $('#tbDetailRegionName').val(Data.Selected.RegionalName);
        $('#tbDetailProvinceName').val(Data.Selected.ProvinceName);
        $('#tbDetailResidenceName').val(Data.Selected.ResidenceName);
        $('#tbDetailIpo').val(Data.Selected.PONumber);
        $('#tbDetailMla').val(Data.Selected.MLANumber);
        $('#tbDetailStartDateLease').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartBapsDate));
        $('#tbDetailEndDateLease').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndBapsDate));
        $('#tbDetailTerm').val(Data.Selected.Term);
        $('#tbDetailBapsType').val(Data.Selected.BapsType);
        $('#tbDetailCustomerInvoice').val(Data.Selected.CustomerInvoice);
        $('#tbDetailCustomerAsset').val(Data.Selected.CustomerID);
        $('#tbDetailCompanyInvoice').val(Data.Selected.CompanyInvoiceName);
        $('#tbDetailCompanyAsset').val(Data.Selected.CompanyInvoiceName);
        $('#tbDetailStipSiro').val(Data.Selected.StipSiro);
        $('#tbDetailInvoiceType').val(Data.Selected.InvoiceTypeName);
        $('#tbDetailCurrency').val(Data.Selected.Currency);
        $('#tbDetailStartDateInv').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartInvoiceDate));
        $('#tbDetailEndDateInv').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndInvoiceDate));
        $('#tbDetailBaseLease').val(Common.Format.CommaSeparation(Data.Selected.BaseLeasePrice.toString()));
        $('#tbDetailOMPrice').val(Common.Format.CommaSeparation(Data.Selected.ServicePrice.toString()));
        $('#tbDetailDeductionAmount').val(Common.Format.CommaSeparation(Data.Selected.DeductionAmount.toString()));
        $('#tbDetailTotalProRate').val(Common.Format.CommaSeparation(Data.Selected.TotalPaymentRupiah.toString()));
        $('#tbDetailInflation').val(Common.Format.CommaSeparation(Data.Selected.InflationAmount.toString()));
        $('#tbDetailAdditional').val(Common.Format.CommaSeparation(Data.Selected.AdditionalAmount.toString()));
        $('#tbDetailDistance').val(Data.Selected.DropFODistance);

        $('#tbDetailOMPrice').attr("disabled", "disabled");
        //$("#tbDetailOMPrice").removeAttr("disabled");
        $("#btUpdateAmount").hide();
        $("#btDismiss").show();
    },
    Export: function () {
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsRenewalYear = $("#slRenewalYear").val() == null ? "" : $("#slRenewalYear").val();
        fsRenewalYearSeq = $("#slRenewalYearSeq").val() == null ? "" : $("#slRenewalYearSeq").val();
        fsRegional = $("#slRegional").val() == null ? "" : $("#slRegional").val();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
        fsDueDatePerMonth = $("#slDueDatePerMonth").val() == null ? "" : $("#slDueDatePerMonth").val();
        fsReconcileType = $("#slReconcileType").val() == null ? "" : $("#slReconcileType").val();
        fsProvince = $("#slProvince").val() == null ? "" : $("#slProvince").val();
        var Batch = $("#slBatch").val() == null ? "" : $("#slBatch").val();
        var lengthdata = $('#tblRaw tr').length;
        var TenantType = $("#slTenantType").val() == null ? "" : $("#slTenantType").val();
        var SONumber = $("#slSONumber").val() == null ? "" : $("#slSONumber").val();
        var SoNumberFilter = "";

        if (SONumber != null && SONumber != "") {
            SoNumberFilter = "0";
            for (var i = 0; i < SONumber.length; i++) {
                SoNumberFilter += ("," + SONumber[i].toString());
            }
        }

        window.location.href = "/RevenueAssurance/Input/Export?strCompanyId=" + fsCompanyId + "&strOperator=" + fsOperator
            + "&strRenewalYear=" + fsRenewalYear + "&strReconcileType=" + fsReconcileType + "&strRenewalYearSeq=" + fsRenewalYearSeq + "&strCurrency=" + fsCurrency
        + "&strDueDatePerMonth=" + fsDueDatePerMonth + "&strRegional=" + fsRegional + "&Batch=" + Batch + "&TenantType=" + TenantType + "&SONumber=" + SoNumberFilter + "&strProvince=" + fsProvince + "&length=" + lengthdata + "&isRaw=0";
    }
}

var TableDone = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblDone = $('#tblDone').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $("#tblDone tbody").on("click", "a.btDetailDone", function (e) {
            e.preventDefault();
            var table = $("#tblDone").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                $('#mdlDetail').modal('toggle');
                TableDone.ShowDetail();
            }
        });

        $(window).resize(function () {
            $("#tblDone").DataTable().columns.adjust().draw();
        });
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val().trim();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val().trim();
        fsRenewalYear = $("#slRenewalYear").val() == null ? "" : $("#slRenewalYear").val().trim();
        fsRenewalYearSeq = $("#slRenewalYearSeq").val() == null ? "" : $("#slRenewalYearSeq").val().trim();
        fsRegional = $("#slRegional").val() == null ? "" : $("#slRegional").val().trim();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val().trim();
        fsDueDatePerMonth = $("#slDueDatePerMonth").val() == null ? "" : $("#slDueDatePerMonth").val().trim();
        fsReconcileType = $("#slReconcileType").val() == null ? "" : $("#slReconcileType").val().trim();
        fsProvince = $("#slProvince").val() == null ? "" : $("#slProvince").val().trim();
        var TenantType = $("#slTenantType").val() == null ? "" : $("#slTenantType").val().trim();

        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strRenewalYear: fsRenewalYear,
            strRenewalYearSeq: fsRenewalYearSeq,
            strRegional: fsRegional,
            strCurrency: fsCurrency,
            strDueDatePerMonth: fsDueDatePerMonth,
            Batch: $("#slBatch").val() == null ? "" : $("#slBatch").val(),
            strSONumber: $("#slSONumber").val() == null ? "" : $("#slSONumber").val(),
            strReconcileType: fsReconcileType,
            strProvince: fsProvince,
            TenantType: TenantType,
            isRaw: 2
        };
        var tblDone = $("#tblDone").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/ReconcileData/grid",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    extend: 'pdf',
                    orientation: 'landscape',
                    pageSize: 'LEGAL',
                    className: 'btn blue btn-outline',
                    exportOptions: { columns: [2, 3, 5, 4, 15, 21, 14, 23, 24, 22, 25, 26, 27] },
                    text: '<i class="fa fa-file-pdf-o" title="PDF"></i>',
                    titleAttr: 'Export To PDF',
                    customize: function (doc) {
                        //Remove the title created by datatTables
                        doc.content.splice(0, 1);
                        //Create a date string that we use in the footer. Format is dd-mm-yyyy
                        var now = new Date();
                        var jsDate = now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();
                        // Logo converted to base64
                        // var logo = getBase64FromImageUrl('https://datatables.net/media/images/logo.png');
                        // The above call should work, but not when called from codepen.io
                        // So we use a online converter and paste the string in.
                        // Done on http://codebeautify.org/image-to-base64-converter
                        // It's a LONG string scroll down to see the rest of the code !!!                        
                        var logo = 'data:image/jpg;base64,/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAABQAAD/4QMpaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjAtYzA2MCA2MS4xMzQ3NzcsIDIwMTAvMDIvMTItMTc6MzI6MDAgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCBDUzUgV2luZG93cyIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDozQTFDNjMzNEQxQ0UxMUUxODVFQTlEQTgzMUI2Njg4MyIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDozQTFDNjMzNUQxQ0UxMUUxODVFQTlEQTgzMUI2Njg4MyI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOjNBMUM2MzMyRDFDRTExRTE4NUVBOURBODMxQjY2ODgzIiBzdFJlZjpkb2N1bWVudElEPSJ4bXAuZGlkOjNBMUM2MzMzRDFDRTExRTE4NUVBOURBODMxQjY2ODgzIi8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+/+4ADkFkb2JlAGTAAAAAAf/bAIQAAgICAgICAgICAgMCAgIDBAMCAgME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';
                        // A documentation reference can be found at
                        // https://github.com/bpampuch/pdfmake#getting-started
                        // Set page margins [left,top,right,bottom] or [horizontal,vertical]
                        // or one number for equal spread
                        // It's important to create enough space at the top for a header !!!
                        doc.pageMargins = [20, 60, 20, 30];
                        // Set the font size fot the entire document
                        doc.defaultStyle.fontSize = 7;
                        // Set the fontsize for the table header
                        doc.styles.tableHeader.fontSize = 7;
                        // Create a header object with 3 columns
                        // Left side: Logo
                        // Middle: brandname
                        // Right side: A document title
                        doc['header'] = (function () {
                            return {
                                columns: [
									//{
									//    image: logo,
									//    width: 200
									//},
									{
									    alignment: 'left',
									    italics: true,
									    text: 'RA System',
									    fontSize: 18,
									    margin: [10, 0]
									},
									{
									    alignment: 'right',
									    fontSize: 14,
									    text: 'List Reconcile'
									}
                                ],
                                margin: 20
                            }
                        });
                        // Create a footer object with 2 columns
                        // Left side: report creation date
                        // Right side: current page and total pages
                        doc['footer'] = (function (page, pages) {
                            return {
                                columns: [
									{
									    alignment: 'left',
									    text: ['Created on: ', { text: jsDate.toString() }]
									},
									{
									    alignment: 'right',
									    text: ['page ', { text: page.toString() }, ' of ', { text: pages.toString() }]
									}
                                ],
                                margin: 20
                            }
                        });
                        // Change dataTable layout (Table styling)
                        // To use predefined layouts uncomment the line below and comment the custom lines below
                        // doc.content[0].layout = 'lightHorizontalLines'; // noBorders , headerLineOnly
                        var objLayout = {};
                        objLayout['hLineWidth'] = function (i) { return .5; };
                        objLayout['vLineWidth'] = function (i) { return .5; };
                        objLayout['hLineColor'] = function (i) { return '#aaa'; };
                        objLayout['vLineColor'] = function (i) { return '#aaa'; };
                        objLayout['paddingLeft'] = function (i) { return 4; };
                        objLayout['paddingRight'] = function (i) { return 4; };
                        doc.content[0].layout = objLayout;
                    }
                },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        TableDone.Export();
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            //buttons: ['copy','csv','excel','pdf','print'],
            "filter": false,
            "lengthMenu": [[25, 50, 100, 200, 7000], ['25', '50', '100', '200', '7000']],
            "destroy": true,
            "columns": [

                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Helper.IsElementExistsInArray(full.Id, Data.RowSelectedDone)) {
                                $("#Row" + full.Id).addClass("active");
                                strReturn += "<label id='" + full.Id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                            }
                            else {
                                strReturn += "<label id='" + full.Id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (full.FilePath != null) {
                                var FileNames = full.FilePath.split('\\');
                                var Name = FileNames[3].toString();
                                strReturn = "<a class='files' target='_blank' href='/RevenueAssurance/Download?FilePath=" + full.FilePath + "&PONumber=0&FileName=" + Name + "&ContentType=" + full.ContenType + "'><i class='fa fa-download'></i></a>";
                            }
                            
                            return strReturn;
                        }
                    }
                },
                { data: "RowIndex" },
                {
                    data: "SONumber", mRender: function (data, type, full) {
                        return "<a class='btDetailDone'>" + data + "</a>";
                    }
                },

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
                {
                    data: "RFIDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "BaufDate", render: function (data) {
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
                { data: "Currency" },
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ServiceCurrency" },
                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InflationAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AdditionalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "DeductionAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "TotalPaymentRupiah", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblDone.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelectedDone.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelectedDone.length; i++) {
                        item = Data.RowSelectedDone[i];
                        $("#Row" + item).addClass("active");
                    }
                }
            },
            "order": [],
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
                    '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tblDone .checkboxes" />' +
                    '<span></span> ' +
                    '</label>';
                var th = $("th.select-all-done").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-done").unbind().on("change", ".group-checkable", function (e) {
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
                        Data.RowSelectedDone = Helper.GetListId(2);
                    else
                        Data.RowSelectedDone = [];
                });
            }
        });
    },

    Reset: function () {
        $("#slSearchCompanyName").val("").trigger('change');
        $("#slSearchOperator").val("").trigger('change');
        $("#slRenewalYear").val("").trigger('change');
        $("#slRenewalYearSeq").val("").trigger('change');
        $("#slReconcileType").val("").trigger('change');
        $("#slSearchCurrency").val("").trigger('change');
        $("#slRegional").val("").trigger('change');
        $("#slProvince").val("").trigger('change');
        $("#slDueDatePerMonth").val("").trigger('change');
        $("#slBatch").val("").trigger('change');

        fsCompanyId = "";
        fsOperator = "";
        fsRenewalYear = "";
        fsRenewalYearSeq = "";
        fsReconcileType = "";
        fsCurrency = "";
        fsRegional = "";
        fsProvince = "";
        fsDueDatePerMonth = "";
    },
    ShowDetail: function () {
        $('#tbDetailAddress').val(Data.Selected.Address);
        $('#tbDetailID').val(Data.Selected.Id);
        $('#tbDetailSONumber').val(Data.Selected.SONumber);
        $('#tbDetailSiteId').val(Data.Selected.SiteID);
        $('#tbDetailSiteName').val(Data.Selected.SiteName);
        $('#tbDetailSiteIDOpr').val(Data.Selected.CustomerSiteID);
        $('#tbDetailSiteNameOpr').val(Data.Selected.CustomerSiteName);
        $('#tbDetailRegionName').val(Data.Selected.RegionalName);
        $('#tbDetailProvinceName').val(Data.Selected.ProvinceName);
        $('#tbDetailResidenceName').val(Data.Selected.ResidenceName);
        $('#tbDetailIpo').val(Data.Selected.PONumber);
        $('#tbDetailMla').val(Data.Selected.MLANumber);
        $('#tbDetailStartDateLease').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartBapsDate));
        $('#tbDetailEndDateLease').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndBapsDate));
        $('#tbDetailTerm').val(Data.Selected.Term);
        $('#tbDetailBapsType').val(Data.Selected.BapsType);
        $('#tbDetailCustomerInvoice').val(Data.Selected.CustomerInvoice);
        $('#tbDetailCustomerAsset').val(Data.Selected.CustomerID);
        $('#tbDetailCompanyInvoice').val(Data.Selected.CompanyInvoiceName);
        $('#tbDetailCompanyAsset').val(Data.Selected.CompanyInvoiceName);
        $('#tbDetailStipSiro').val(Data.Selected.StipSiro);
        $('#tbDetailInvoiceType').val(Data.Selected.InvoiceTypeName);
        $('#tbDetailCurrency').val(Data.Selected.Currency);
        $('#tbDetailStartDateInv').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartInvoiceDate));
        $('#tbDetailEndDateInv').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndInvoiceDate));
        $('#tbDetailBaseLease').val(Common.Format.CommaSeparation(Data.Selected.BaseLeasePrice.toString()));
        $('#tbDetailOMPrice').val(Common.Format.CommaSeparation(Data.Selected.ServicePrice.toString()));
        $('#tbDetailDeductionAmount').val(Common.Format.CommaSeparation(Data.Selected.DeductionAmount.toString()));
        $('#tbDetailTotalProRate').val(Common.Format.CommaSeparation(Data.Selected.TotalPaymentRupiah.toString()));
        $('#tbDetailInflation').val(Common.Format.CommaSeparation(Data.Selected.InflationAmount.toString()));
        $('#tbDetailAdditional').val(Common.Format.CommaSeparation(Data.Selected.AdditionalAmount.toString()));
        $('#tbDetailDistance').val(Data.Selected.DropFODistance);

        $('#tbDetailOMPrice').attr("disabled", "disabled");
        $("#btUpdateAmount").hide();
        //$("#btDismiss").hide();
        //$('#tbDetailTotalProRate').val(Common.Format.CommaSeparation(Data.Selected.amountRupiah.toString()));
    },
    Export: function () {
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsRenewalYear = $("#slRenewalYear").val() == null ? "" : $("#slRenewalYear").val();
        fsRenewalYearSeq = $("#slRenewalYearSeq").val() == null ? "" : $("#slRenewalYearSeq").val();
        fsRegional = $("#slRegional").val() == null ? "" : $("#slRegional").val();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
        fsDueDatePerMonth = $("#slDueDatePerMonth").val() == null ? "" : $("#slDueDatePerMonth").val();
        fsReconcileType = $("#slReconcileType").val() == null ? "" : $("#slReconcileType").val();
        fsProvince = $("#slProvince").val() == null ? "" : $("#slProvince").val();
        var lengthdata = $('#tblDone tr').length;
        var Batch = $("#slBatch").val() == null ? "" : $("#slBatch").val();
        var TenantType = $("#slTenantType").val() == null ? "" : $("#slTenantType").val();
        var SONumber = $("#slSONumber").val() == null ? "" : $("#slSONumber").val();
        var SoNumberFilter = "";

        if (SONumber != null && SONumber != "") {
            SoNumberFilter = "0";
            for (var i = 0; i < SONumber.length; i++) {
                SoNumberFilter += ("," + SONumber[i].toString());
            }
        }

        window.location.href = "/RevenueAssurance/Input/Export?strCompanyId=" + fsCompanyId + "&strOperator=" + fsOperator
            + "&strRenewalYear=" + fsRenewalYear + "&strReconcileType=" + fsReconcileType + "&strRenewalYearSeq=" + fsRenewalYearSeq + "&strCurrency=" + fsCurrency
        + "&strDueDatePerMonth=" + fsDueDatePerMonth + "&strRegional=" + fsRegional + "&Batch=" + Batch + "&TenantType=" + TenantType + "&SONumber=" + SoNumberFilter + "&strProvince=" + fsProvince + "&length=" + lengthdata + "&isRaw=2";
    }
}

var TableHold = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblHold = $('#tblHold').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $("#tblHold tbody").on("click", "a.btDetailHold", function (e) {
            e.preventDefault();
            var table = $("#tblHold").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                $('#mdlDetail').modal('toggle');
                TableHold.ShowDetail();
            }
        });

        $(window).resize(function () {
            $("#tblHold").DataTable().columns.adjust().draw();
        });
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val().trim();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val().trim();
        fsRenewalYear = $("#slRenewalYear").val() == null ? "" : $("#slRenewalYear").val().trim();
        fsRenewalYearSeq = $("#slRenewalYearSeq").val() == null ? "" : $("#slRenewalYearSeq").val().trim();
        fsRegional = $("#slRegional").val() == null ? "" : $("#slRegional").val().trim();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val().trim();
        fsDueDatePerMonth = $("#slDueDatePerMonth").val() == null ? "" : $("#slDueDatePerMonth").val().trim();
        fsReconcileType = $("#slReconcileType").val() == null ? "" : $("#slReconcileType").val().trim();
        fsProvince = $("#slProvince").val() == null ? "" : $("#slProvince").val().trim();
        var TenantType = $("#slTenantType").val() == null ? "" : $("#slTenantType").val().trim();

        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strRenewalYear: fsRenewalYear,
            strRenewalYearSeq: fsRenewalYearSeq,
            strRegional: fsRegional,
            strCurrency: fsCurrency,
            strDueDatePerMonth: fsDueDatePerMonth,
            Batch: $("#slBatch").val() == null ? "" : $("#slBatch").val(),
            strSONumber: $("#slSONumber").val() == null ? "" : $("#slSONumber").val(),
            strReconcileType: fsReconcileType,
            strProvince: fsProvince,
            TenantType: TenantType,
            isRaw: 3
        };
        var tblHold = $("#tblHold").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/ReconcileData/grid",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    extend: 'pdf',
                    orientation: 'landscape',
                    pageSize: 'LEGAL',
                    className: 'btn blue btn-outline',
                    exportOptions: { columns: [2, 3, 5, 4, 15, 21, 14, 23, 24, 22, 25, 26, 27] },
                    text: '<i class="fa fa-file-pdf-o" title="PDF"></i>',
                    titleAttr: 'Export To PDF',
                    customize: function (doc) {
                        //Remove the title created by datatTables
                        doc.content.splice(0, 1);
                        //Create a date string that we use in the footer. Format is dd-mm-yyyy
                        var now = new Date();
                        var jsDate = now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();
                        // Logo converted to base64
                        // var logo = getBase64FromImageUrl('https://datatables.net/media/images/logo.png');
                        // The above call should work, but not when called from codepen.io
                        // So we use a online converter and paste the string in.
                        // Done on http://codebeautify.org/image-to-base64-converter
                        // It's a LONG string scroll down to see the rest of the code !!!                        
                        var logo = 'data:image/jpg;base64,/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAABQAAD/4QMpaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjAtYzA2MCA2MS4xMzQ3NzcsIDIwMTAvMDIvMTItMTc6MzI6MDAgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCBDUzUgV2luZG93cyIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDozQTFDNjMzNEQxQ0UxMUUxODVFQTlEQTgzMUI2Njg4MyIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDozQTFDNjMzNUQxQ0UxMUUxODVFQTlEQTgzMUI2Njg4MyI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOjNBMUM2MzMyRDFDRTExRTE4NUVBOURBODMxQjY2ODgzIiBzdFJlZjpkb2N1bWVudElEPSJ4bXAuZGlkOjNBMUM2MzMzRDFDRTExRTE4NUVBOURBODMxQjY2ODgzIi8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+/+4ADkFkb2JlAGTAAAAAAf/bAIQAAgICAgICAgICAgMCAgIDBAMCAgME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';
                        // A documentation reference can be found at
                        // https://github.com/bpampuch/pdfmake#getting-started
                        // Set page margins [left,top,right,bottom] or [horizontal,vertical]
                        // or one number for equal spread
                        // It's important to create enough space at the top for a header !!!
                        doc.pageMargins = [20, 60, 20, 30];
                        // Set the font size fot the entire document
                        doc.defaultStyle.fontSize = 7;
                        // Set the fontsize for the table header
                        doc.styles.tableHeader.fontSize = 7;
                        // Create a header object with 3 columns
                        // Left side: Logo
                        // Middle: brandname
                        // Right side: A document title
                        doc['header'] = (function () {
                            return {
                                columns: [
									//{
									//    image: logo,
									//    width: 200
									//},
									{
									    alignment: 'left',
									    italics: true,
									    text: 'RA System',
									    fontSize: 18,
									    margin: [10, 0]
									},
									{
									    alignment: 'right',
									    fontSize: 14,
									    text: 'List Reconcile'
									}
                                ],
                                margin: 20
                            }
                        });
                        // Create a footer object with 2 columns
                        // Left side: report creation date
                        // Right side: current page and total pages
                        doc['footer'] = (function (page, pages) {
                            return {
                                columns: [
									{
									    alignment: 'left',
									    text: ['Created on: ', { text: jsDate.toString() }]
									},
									{
									    alignment: 'right',
									    text: ['page ', { text: page.toString() }, ' of ', { text: pages.toString() }]
									}
                                ],
                                margin: 20
                            }
                        });
                        // Change dataTable layout (Table styling)
                        // To use predefined layouts uncomment the line below and comment the custom lines below
                        // doc.content[0].layout = 'lightHorizontalLines'; // noBorders , headerLineOnly
                        var objLayout = {};
                        objLayout['hLineWidth'] = function (i) { return .5; };
                        objLayout['vLineWidth'] = function (i) { return .5; };
                        objLayout['hLineColor'] = function (i) { return '#aaa'; };
                        objLayout['vLineColor'] = function (i) { return '#aaa'; };
                        objLayout['paddingLeft'] = function (i) { return 4; };
                        objLayout['paddingRight'] = function (i) { return 4; };
                        doc.content[0].layout = objLayout;
                    }
                },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        TableHold.Export();
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            //buttons: ['copy','csv','excel','pdf','print'],
            "filter": false,
            "lengthMenu": [[25, 50, 100, 200, 7000], ['25', '50', '100', '200', '7000']],
            "destroy": true,
            "columns": [

                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Helper.IsElementExistsInArray(full.Id, Data.RowSelectedHold)) {
                                $("#Row" + full.Id).addClass("active");
                                strReturn += "<label id='" + full.Id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.Id + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                            }
                            else {
                                strReturn += "<label id='" + full.Id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.Id + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                },
                { data: "RowIndex" },
                {
                    data: "SONumber", mRender: function (data, type, full) {
                        return "<a class='btDetailHold'>" + data + "</a>";
                    }
                },

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
                {
                    data: "RFIDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "BaufDate", render: function (data) {
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
                { data: "Currency" },
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ServiceCurrency" },
                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InflationAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AdditionalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "DeductionAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "TotalPaymentRupiah", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblHold.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelectedHold.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelectedHold.length; i++) {
                        item = Data.RowSelectedHold[i];
                        $("#Row" + item).addClass("active");
                    }
                }
            },
            "order": [],
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
                //var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                //    '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tblHold .checkboxes" />' +
                //    '<span></span> ' +
                //    '</label>';
                //var th = $("th.select-all-hold").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                //$("th.select-all-hold").unbind().on("change", ".group-checkable", function (e) {
                //    var set = $(".panelSearchResult .DTFC_LeftBodyWrapper .DTFC_Cloned .checkboxes");
                //    var checked = jQuery(this).is(":checked");
                //    jQuery(set).each(function () {
                //        label = $(this).parent();
                //        var id = label.attr("id");
                //        if (label.attr("style") != "display:none") {
                //            /* Replace the following code with the code to select all checkboxes in all pages */
                //            if (checked) {
                //                $(this).prop("checked", true);
                //                $(this).parents('tr').addClass('active');
                //                $(this).trigger("change");

                //                $(".Row" + id).addClass("active");
                //                $("." + id).prop("checked", true);
                //                $("." + id).trigger("change");
                //            } else {
                //                $(this).prop("checked", false);
                //                $(this).parents('tr').removeClass("active");
                //                $(this).trigger("change");

                //                $(".Row" + id).removeClass("active");
                //                $("." + id).prop("checked", false);
                //                $("." + id).trigger("change");
                //            }
                //        }
                //    });
                //    if (checked)
                //        Data.RowSelectedHold = Helper.GetListId(3);
                //    else
                //        Data.RowSelectedHold = [];
                //});
            }
        });
    },

    Reset: function () {
        $("#slSearchCompanyName").val("").trigger('change');
        $("#slSearchOperator").val("").trigger('change');
        $("#slRenewalYear").val("").trigger('change');
        $("#slRenewalYearSeq").val("").trigger('change');
        $("#slReconcileType").val("").trigger('change');
        $("#slSearchCurrency").val("").trigger('change');
        $("#slRegional").val("").trigger('change');
        $("#slProvince").val("").trigger('change');
        $("#slDueDatePerMonth").val("").trigger('change');
        $("#slBatch").val("").trigger('change');

        fsCompanyId = "";
        fsOperator = "";
        fsRenewalYear = "";
        fsRenewalYearSeq = "";
        fsReconcileType = "";
        fsCurrency = "";
        fsRegional = "";
        fsProvince = "";
        fsDueDatePerMonth = "";
    },
    ShowDetail: function () {
        $('#tbDetailAddress').val(Data.Selected.Address);
        $('#tbDetailID').val(Data.Selected.Id);
        $('#tbDetailSONumber').val(Data.Selected.SONumber);
        $('#tbDetailSiteId').val(Data.Selected.SiteID);
        $('#tbDetailSiteName').val(Data.Selected.SiteName);
        $('#tbDetailSiteIDOpr').val(Data.Selected.CustomerSiteID);
        $('#tbDetailSiteNameOpr').val(Data.Selected.CustomerSiteName);
        $('#tbDetailRegionName').val(Data.Selected.RegionalName);
        $('#tbDetailProvinceName').val(Data.Selected.ProvinceName);
        $('#tbDetailResidenceName').val(Data.Selected.ResidenceName);
        $('#tbDetailIpo').val(Data.Selected.PONumber);
        $('#tbDetailMla').val(Data.Selected.MLANumber);
        $('#tbDetailStartDateLease').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartBapsDate));
        $('#tbDetailEndDateLease').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndBapsDate));
        $('#tbDetailTerm').val(Data.Selected.Term);
        $('#tbDetailBapsType').val(Data.Selected.BapsType);
        $('#tbDetailCustomerInvoice').val(Data.Selected.CustomerInvoice);
        $('#tbDetailCustomerAsset').val(Data.Selected.CustomerID);
        $('#tbDetailCompanyInvoice').val(Data.Selected.CompanyInvoiceName);
        $('#tbDetailCompanyAsset').val(Data.Selected.CompanyInvoiceName);
        $('#tbDetailStipSiro').val(Data.Selected.StipSiro);
        $('#tbDetailInvoiceType').val(Data.Selected.InvoiceTypeName);
        $('#tbDetailCurrency').val(Data.Selected.Currency);
        $('#tbDetailStartDateInv').val(Common.Format.ConvertJSONDateTime(Data.Selected.StartInvoiceDate));
        $('#tbDetailEndDateInv').val(Common.Format.ConvertJSONDateTime(Data.Selected.EndInvoiceDate));
        $('#tbDetailBaseLease').val(Common.Format.CommaSeparation(Data.Selected.BaseLeasePrice.toString()));
        $('#tbDetailOMPrice').val(Common.Format.CommaSeparation(Data.Selected.ServicePrice.toString()));
        $('#tbDetailDeductionAmount').val(Common.Format.CommaSeparation(Data.Selected.DeductionAmount.toString()));
        $('#tbDetailTotalProRate').val(Common.Format.CommaSeparation(Data.Selected.TotalPaymentRupiah.toString()));
        $('#tbDetailInflation').val(Common.Format.CommaSeparation(Data.Selected.InflationAmount.toString()));
        $('#tbDetailAdditional').val(Common.Format.CommaSeparation(Data.Selected.AdditionalAmount.toString()));
        $('#tbDetailDistance').val(Data.Selected.DropFODistance);

        $('#tbDetailOMPrice').attr("disabled", "disabled");
        $("#btUpdateAmount").hide();
        //$("#btDismiss").hide();
        //$('#tbDetailTotalProRate').val(Common.Format.CommaSeparation(Data.Selected.amountRupiah.toString()));
    },
    Export: function () {
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsRenewalYear = $("#slRenewalYear").val() == null ? "" : $("#slRenewalYear").val();
        fsRenewalYearSeq = $("#slRenewalYearSeq").val() == null ? "" : $("#slRenewalYearSeq").val();
        fsRegional = $("#slRegional").val() == null ? "" : $("#slRegional").val();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
        fsDueDatePerMonth = $("#slDueDatePerMonth").val() == null ? "" : $("#slDueDatePerMonth").val();
        fsReconcileType = $("#slReconcileType").val() == null ? "" : $("#slReconcileType").val();
        fsProvince = $("#slProvince").val() == null ? "" : $("#slProvince").val();
        var lengthdata = $('#tblHold tr').length;
        var Batch = $("#slBatch").val() == null ? "" : $("#slBatch").val();
        var TenantType = $("#slTenantType").val() == null ? "" : $("#slTenantType").val();
        var SONumber = $("#slSONumber").val() == null ? "" : $("#slSONumber").val();
        var SoNumberFilter = "";

        if (SONumber != null && SONumber != "") {
            SoNumberFilter = "0";
            for (var i = 0; i < SONumber.length; i++) {
                SoNumberFilter += ("," + SONumber[i].toString());
            }
        }

        window.location.href = "/RevenueAssurance/Input/Export?strCompanyId=" + fsCompanyId + "&strOperator=" + fsOperator
            + "&strRenewalYear=" + fsRenewalYear + "&strReconcileType=" + fsReconcileType + "&strRenewalYearSeq=" + fsRenewalYearSeq + "&strCurrency=" + fsCurrency
        + "&strDueDatePerMonth=" + fsDueDatePerMonth + "&strRegional=" + fsRegional + "&Batch=" + Batch + "&TenantType=" + TenantType + "&SONumber=" + SoNumberFilter + "&strProvince=" + fsProvince + "&length=" + lengthdata + "&isRaw=3";
    }
}

var TableUpload = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();

        var TableUpload = $('#tblUpload').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        var TableUpload = $('#tblSiteUpload').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        //$("#tblUpload tbody").on("click", "a.btDetailHold", function (e) {
        //    e.preventDefault();
        //    var table = $("#tblUpload").DataTable();
        //    var data = table.row($(this).parents("tr")).data();
        //    if (data != null) {
        //        Data.Selected = data;
        //        $('#mdlDetail').modal('toggle');
        //        TableHold.ShowDetail();
        //    }
        //});

        $(window).resize(function () {
            $("#tblUpload").DataTable().columns.adjust().draw();
        });
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        Data.RowSelectedUpload = [];
        var vparams = Control.GetParam();

        var params = {
            strOperator: vparams.CustomerID,
            strRegional: vparams.Regional,
            Batch: vparams.Batch
        };
        var tblUpload = $("#tblUpload").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/ReconcileData/gridupload",
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
                        TableUpload.Export();
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            //buttons: ['copy','csv','excel','pdf','print'],
            "filter": false,
            "lengthMenu": [[25,50,100,1000,-1], ['25','50','100','1000','All']],
            "destroy": true,
            "columns": [

                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (full.FilePath == null) {
                                //if (Helper.IsElementExistsInArray(full, Data.RowSelectedUpload)) {
                                //    $("#Row" + full.ReconYear + full.CustomerRegionID + full.RegionID).addClass("active");
                                //    strReturn += "<label id='" + full.ReconYear + full.CustomerRegionID + full.RegionID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.ReconYear + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                //}
                                //else {
                                //    strReturn += "<label id='" + full.ReconYear + full.CustomerRegionID + full.RegionID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.ReconYear + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                                //}
                                if (Helper.IsElementExistsInArray(parseInt(full.Id), Data.RowSelectedSite)) {
                                    strReturn += "<label style='display:none;' id='" + full.Id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.Id + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                } else {
                                    if (Helper.IsElementExistsInArray(full.Id, Data.RowSelectedUpload)) {
                                        $("#Row" + full.Id).addClass("active");
                                        strReturn += "<label id='" + full.Id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                    }
                                    else {
                                        strReturn += "<label id='" + full.Id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' class='checkboxes' /><span></span></label>";
                                    }
                                }
                                
                            }
                            else
                                strReturn = "<a class='files' target='_blank' href='/RevenueAssurance/Download?FilePath=" + full.FilePath + "&PONumber=0&FileName=" + full.FileName + "&ContentType=" + full.ContentType + "'><i class='fa fa-download'></i></a>";

                            

                            return strReturn;
                        }
                    }
                },

                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CompanyInvoiceName" },
                { data: "RegionName" },
                { data: "CustomerRegionName" },
                { data: "Term" },
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

                { data: "BaseLeaseCurrency" },
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

                { data: "ServiceCurrency" },
                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

                { data: "DeductionCurrency" },
                { data: "DeductionAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

                { data: "TotalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblUpload.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelectedUpload.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelectedUpload.length; i++) {
                        item = Data.RowSelectedUpload[i];
                        $("#Row" + item).addClass("active");
                    }
                }
            },
            "order": [],
            "scrollY": 300, /* Enable vertical scroll to allow fixed columns */
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 1 /* Set the 2 most left columns as fixed columns */
            },
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
            "initComplete": function () {
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                    '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tblUpload .checkboxes" />' +
                    '<span></span> ' +
                    '</label>';
                var th = $("th.select-all-upload").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-upload").unbind().on("change", ".group-checkable", function (e) {
                    Data.RowSelectedUpload = Data.RowSelectedSite;
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
                                //Data.RowSelectedUpload.push(parseInt(id));
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
                        Data.RowSelectedUpload = Helper.GetListIdUpload(1);
                    else
                        Data.RowSelectedUpload = [];
                });
            }
        });
    },

    Reset: function () {
        $("#slUploadOperator").val("").trigger('change');
        $("#slUploadRegional").val("").trigger('change');
        $("#slUploadBatch").val("").trigger('change');

    },
    
    Export: function () {
        if (TempData.length < 1) {
            var vparams = Control.GetParam();
            var RegionID = "";

            TempData.forEach(function (entry) {
                Data.RowSelectedUpload.push(parseInt(entry.RegionID));
            });

            window.location.href = "/RevenueAssurance/Upload/Export?strOperator=" + vparams.CustomerID + "&strRegional=" + vparams.Regional + "&Batch=" + vparams.Batch.toString() + "&RegionID=" + Data.RowSelectedUpload.toString();
        }
        else {
            Common.Alert.Warning("Please Select one or more data !");
        }
    }
}

var Process = {

    PICA: function () {
        var l = Ladda.create(document.querySelector("#btYesReject"))

        var params = {
            mstRAScheduleId: Data.RowSelectedProcess,
            Department: $("#slDept").val(),
            PIC: $("#slPIC").val(),
            PICA: $("#slPICA").val(),
            Remarks: $("#tbRemarks").val()
        }

        $.ajax({
            url: "/api/ReconcileData/PICA",
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
                Common.Alert.Success("Data Success to Reject!");
                Data.RowSelectedProcess = [];
                Data.RowSelectedRaw = [];
                Form.DonePICA();
            }
            else {
                Common.Alert.Warning("Something Went Wrong. Contact IT Help Desk");
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        })
    },

    SendToInput: function () {
        var l = Ladda.create(document.querySelector("#btYesConfirm2"))

        var params = {
            mstRAScheduleId: Data.RowSelectedHold,
            ExpiredDate: $("#tbExpiredDate").val(),
            IsActive: 0
        }

        $.ajax({
            url: "/api/ReconcileData/PICAUpdate",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            $('#mdlToInput').modal('hide');
            if (data == "1") {
                Common.Alert.Success("Data Success Send Back To Reconcile Input!");
                Data.RowSelectedHold = [];
                TableHold.Search();
                SelectRow = {};
            } else {
                Common.Alert.Warning("Something Went Wrong. Contact IT Help Desk");
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        })
    },

    SendToProcess: function () {
        var l = Ladda.create(document.querySelector("#btYesConfirm"))

        var params = {
            soNumb: Data.RowSelectedRaw
        }

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
                Common.Alert.Success("Data Success Send To Reconcile Process!");
                Data.RowSelectedRaw = [];
                Data.RowSelectedProcess = [];
            } else {
                Common.Alert.Warning("Something Went Wrong. Contact IT Help Desk");
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        }).always(function (jqXHR, textStatus) {
            Form.DoneProcess();
        })
    },

    SendToDone: function () {
        var l = Ladda.create(document.querySelector("#btDoneConfirm"))

        var params = {
            soNumb: Data.RowSelectedProcess
        }

        $.ajax({
            url: "/api/ReconcileData/doDone",
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
                Common.Alert.Success("Data Success Send To Reconcile Done!");
                Data.RowSelectedRaw = [];
                Data.RowSelectedProcess = [];
                TableProcess.Search();
            } else {
                Common.Alert.Warning("Something Went Wrong. Contact IT Help Desk");
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        })
    },

    Import: function () { $('#mdlBulky').modal('show'); },

    UpdateReconcile: function () {
        var l = Ladda.create(document.querySelector("#btUpdateReconcile"))

        var params = {
            ID: Data.Selected.Id, //Data.Selected.Id,
            ServicePrice: $('#tbUpdateOMPrice').val().replace(',', ''),
            BaseLeasePrice: $('#tbUpdateBaseLease').val().replace(',', ''),
            InflationAmount: $('#tbUpdateInflationAmount').val().replace(',', ''),
            AdditionalAmount: $('#tbUpdateAdditionalAmount').val().replace(',', ''),
            DeductionAmount: $('#tbUpdateDeductionAmount').val().replace(',', ''),
            PenaltySlaAmount: $('#tbUpdatePenaltyAmount').val().replace(',', ''),
            ServiceCurrency: $('#slUpdateServiceCurrency').val(),
            BaseLeaseCurrency: $('#slUpdateCurrency').val(),
            InflationCurrency: $('#slUpdateInflationCurrency').val(),
            AdditionalCurrency: $('#slUpdateAdditionalCurrency').val(),
            DeductionCurrency: $('#slUpdateDeductionCurrency').val(),
            PenaltySlaCurrency: $('#slUpdatePenaltyCurrency').val(),
            ReconcileDate: $('#tbUpdateReconcileDate').val(),
            CustomerMLANumber: $('#tbUpdateMla').val(),
            CustomerSiteID: $('#tbUpdateSiteIDOpr').val(),
            CustomerSiteName: $('#tbUpdateSiteNameOpr').val(),
            CompanyInvoice: $('#tbUpdateCompanyInvoice').val(),
            RFIDate: $('#tbUpdateRFIDate').val(),
            BaufDate: $('#tbUpdateBaukDate').val(),
            StartInvoiceDate: $('#tbUpdateStartDateInv').val(),
            EndInvoiceDate: $('#tbUpdateEndDateInv').val(),
            ChangeCutOffHCPT: Data.UpdateCutOffHCPT
        }

        $.ajax({
            "url": "/api/ReconcileData/UpdateReconcile",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (data != null) {
                Common.Alert.Success("Data Successfully Updated!");
                Data.RowSelectedProcess = [];
                Data.RowSelectedRaw = [];
                Form.DoneUpdateOMPrice();
            } else {
                Common.Alert.Warning("Something Went Wrong. Contact IT Help Desk");
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        })
    },

    UploadDocument: function () {
        var errors = true;
        var formData = new FormData();
        formData.append("Id", Data.RowSelectedProcess);

        var fileInput = document.getElementById("file");
        if (document.getElementById("file").files.length != 0) {

            fsFileName = fileInput.files[0].name;
            formData.append("BADocumentCode", fsFileName);

            fsFile = fileInput.files[0];

            fsExtension = fsFileName.split('.').pop().toUpperCase();

            if (fsExtension != "XLS" && fsExtension != "XLSX" && fsExtension != "DOC" && fsExtension != "DOCX" && fsExtension != "PDF") {
                Common.Alert.Warning("Please upload an Excel or PDF File."); return;
            }
            else {

                formData.append('BADocument', fsFile);
            }

            errors = false;
        }
        else {
            Common.Alert.Warning("Please Select BA Document."); return;
        }

        fileInput = document.getElementById("file2");
        console.log(document.getElementById("file2").files.length);
        if (document.getElementById("file2").files.length != 0) {

            fsFileName = fileInput.files[0].name;
            formData.append("BAOtherCode", fsFileName);

            fsFile = fileInput.files[0];

            fsExtension = fsFileName.split('.').pop().toUpperCase();

            if (fsExtension != "XLS" && fsExtension != "XLSX" && fsExtension != "DOC" && fsExtension != "DOCX" && fsExtension != "PDF") {
                Common.Alert.Warning("Please upload an Excel or PDF File."); return;
            }
            else {

                formData.append('BAOther', fsFile);
            }
        }
        else {
            formData.append("BAOtherCode", '');
        }


        if (!errors) {
            var l = Ladda.create(document.querySelector("#btSubmitUpload"));
            $.ajax({
                url: '/api/ReconcileData/UploadFile',
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
                if (data !== "Exception") {
                    if (data.length <= 0) {
                        $('.panelUpload').find('input:file').val('');
                        Common.Alert.Warning("The uploaded file is invalid or empty. Please use the available template, and fill the file correctly with valid product data.");
                    } else {
                        $('.panelUpload').find('input:file').val('');
                        Common.Alert.Success("Upload File Success!");
                        Form.DoneUploadDocument();
                    }
                } else {
                    $('.panelUpload').find('input:file').val('');
                    Common.Alert.Warning("The uploaded file is invalid. Please download the latest template by clicking the Download Template button.");
                }
                l.stop();
            }).fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l.stop()
            });
        }

        //console.log(UploadFile);
        //return UploadFile.filePath;
    },

    ProcessNextActivity: function () {
        var l = Ladda.create(document.querySelector("#btConfirm"))
        var NextActivity = $('#slNextActivity :selected').val();
        var url = "";

        if (CurrentTab == 1) {
            var params = {
                mstRAScheduleId: Data.RowSelectedRaw,
                NextActivity: NextActivity,
                ListID: Data.RowSelectedRaw.toString()
            }
            url = "/api/ReconcileData/ProcessNextActivity";
        }
        else if (CurrentTab == 2) {
            var params = {
                Id: Data.RowSelectedProcess.toString(),
                NextActivity: NextActivity
            }

            url = "/api/ReconcileData/UpdateNextActivity";
        }
        else if (CurrentTab == 3) {
            var params = {
                Id: Data.RowSelectedDone.toString(),
                NextActivity: NextActivity
            }

            url = "/api/ReconcileData/UpdateNextActivity";
        }

        $.ajax({
            url: url,
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (data != "0") {
                Common.Alert.Success("Process Data Success!");
                Data.RowSelectedRaw = [];
                Data.RowSelectedProcess = [];
                $('#mdlNextActivity').modal('hide');
                if (CurrentTab == 1) {
                    TableRaw.Search();
                }
                else if (CurrentTab == 2) {
                    TableProcess.Search();
                }
                else if (CurrentTab == 3) {
                    TableDone.Search();
                }

            } else {
                $('#mdlNextActivity').modal('hide');
                Common.Alert.Warning("Something Went Wrong. Contact IT Help Desk");
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        })
    }
}

var Control = {
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/TBGSys_Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchCompanyName").html("<option></option>")
            $("#slSearchCompanyName").append("<option value=' '>No Filter</option>");
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchCompanyName").append("<option value='" + item.CompanyId.trim() + "'>" + item.Company.trim() + "</option>");
                })
            }

            $("#slSearchCompanyName").select2({ placeholder: "Select Company Name", width: null });
            Control.BindingSelectUpdateCompany();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectUpdateCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/TBGSys_Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#tbUpdateCompanyInvoice").html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#tbUpdateCompanyInvoice").append("<option value='" + item.CompanyId.trim() + "'>" + item.Company.trim() + "</option>");
                })
            }

            $("#tbUpdateCompanyInvoice").select2({ placeholder: "Select Company Name", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectTenantType: function () {
        $.ajax({
            url: "/api/MstDataSource/TenantType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slTenantType").html("<option></option>")
            $("#slTenantType").append("<option value=' '>No Filter</option>");
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slTenantType").append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                })
            }

            $("#slTenantType").select2({ placeholder: "Select Tenant Type", width: null });

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
            $("#slRegional").append("<option value=' '>No Filter</option>");

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slRegional").append("<option value='" + item.RegionalId + "'>" + item.RegionalName + "</option>");
                })
            }

            $("#slRegional").select2({ placeholder: "Select Regional", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectProvince: function () {
        $.ajax({
            url: "/api/MstDataSource/Province",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slProvince").html("<option></option>")
            $("#slProvince").append("<option value=' '>No Filter</option>");

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slProvince").append("<option value='" + item.ProvinceID + "'>" + item.ProvinceName + "</option>");
                })
            }

            $("#slProvince").select2({ placeholder: "Select Province", width: null });

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
            $("#slSearchOperator").append("<option value=' '>No Filter</option>")
            $("#slUploadOperator").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item.OperatorId.trim() == 'TSEL' || item.OperatorId.trim() == 'ISAT' || item.OperatorId.trim() == 'XL' || item.OperatorId.trim() == 'MITEL' || item.OperatorId.trim() == 'PAB' || item.OperatorId.trim() == 'TELKOM' || item.OperatorId.trim() == 'HCPT') {
                        $("#slSearchOperator").append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                    }

                    if (item.OperatorId.trim() == 'TSEL') {
                        $("#slUploadOperator").append("<option value='" + item.OperatorId.trim() + "'>" + item.OperatorId + "</option>");
                    }
                })
            }

            $("#slSearchOperator").select2({ placeholder: "Select Operator", width: null });
            $("#slUploadOperator").select2({ placeholder: "Select Operator", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindProRateAmount: function () {
        var CustomerID = $('#tbUpdateCustomerInvoice').val();
        var InvoiceStartDate = $('#tbUpdateStartDateInv').val();
        var InvoiceEndDate = $('#tbUpdateEndDateInv').val();
        var InvoiceAmount = $('#tbUpdateBaseLease').val().replace(/,/g, "");
        var ServiceAmount = $('#tbUpdateOMPrice').val().replace(/,/g, "");
        var DeductionAmount = $('#tbUpdateDeductionAmount').val().replace(/,/g, "");

        var params = { CustomerID: CustomerID, StartInvoiceDate: InvoiceStartDate, EndInvoiceDate: InvoiceEndDate, InvoiceAmount: InvoiceAmount, ServiceAmount: ServiceAmount, DropFODistance: Data.Selected.DropFODistance, ProductID: Data.Selected.ProductID }

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
            $("#tbUpdateTotalProRate").val(Common.Format.CommaSeparation(result));
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingNextStep: function () {

        $.ajax({
            url: "/api/MstDataSource/NextActivity",
            type: "GET",
            data: { CurrentActivity: CurrentTab }
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slNextActivity").html("")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item.Value < '4')
                        $("#slNextActivity").append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                })
            }

            //$("#slNextStep").select2({ placeholder: "Select Operator", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindCustomerRegion: function(CustomerID){
        $.ajax({
            url: "/api/MstDataSource/ListCustomerRegion",
            type: "GET",
            data: { CustomerID: CustomerID.value.trim() }
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slUploadRegional").html("")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slUploadRegional").append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                })
            }

            //$("#slNextStep").select2({ placeholder: "Select Operator", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    GetParam: function () {
        var params = {
            CustomerID: $("#slUploadOperator").val() == null ? "" : $("#slUploadOperator").val(),
            Regional: $("#slUploadRegional").val() == null ? "" : $("#slUploadRegional").val(),
            Batch: $("#slUploadBatch").val() == null ? "" : $("#slUploadBatch").val()
        }

        return params;
    },
}

var Helper = {
    Calculate: function () { },
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
    GetListIdUpload: function (isRaw) {
        //for CheckAll Pages
        var vparams = Control.GetParam();

        var params = {
            strOperator: vparams.CustomerID,
            strRegional: vparams.Regional,
            Batch: vparams.Batch
        };

        var AjaxData = [];
        $.ajax({
            url: "/api/ReconcileData/GetListIdUpload",
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
    GetListId: function (isRaw) {
        //for CheckAll Pages
        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strRenewalYear: fsRenewalYear,
            strRenewalYearSeq: fsRenewalYearSeq,
            strRegional: fsRegional,
            strCurrency: fsCurrency,
            strDueDatePerMonth: fsDueDatePerMonth,
            Batch: $("#slBatch").val() == null ? "" : $("#slBatch").val(),
            strSONumber: $("#slSONumber").val() == null ? "" : $("#slSONumber").val(),
            strReconcileType: fsReconcileType,
            strProvince: fsProvince,
            isRaw: isRaw,
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/ReconcileData/GetListId",
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

