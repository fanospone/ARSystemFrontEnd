params = {};
var _type;
PKP = "";
jQuery(document).ready(function () {
    Control.Init();
});

var Control = {
    Init: function () {
        Control.BindingSelectCustomer('slCustomer');
        Control.BindingSelectCompany('slCompany');
        Control.BindingBeginYear('tbBapsReceiveYearStart');
        Control.BindingYear('tbBapsReceiveYearEnd');
        Control.BindingBeginYear('tbInvoiceYearStart');
        Control.BindingYear('tbInvoiceYearEnd');
        $("#slAgingCategory").select2();
        $("#slInvoiceCategory").select2();
        Bind.Summary();

        PKP = Constants.CompanyCode.PKP;
        PKPPurpose.Set.UserCompanyCode();
        if (PKPPurpose.Temp.UserCompanyCode == Constants.CompanyCode.PKP) {
            PKPPurpose.Filter.PKPOnly();
            $('#pnlExcludePKP').hide();
        }

        $("#btSearch").unbind().click(function () {
            //e.preventDefault();
            Bind.Summary();
        });

        $("#btBack").unbind().click(function () {
            Bind.Summary();
        });
        
        $("#divCreateInvoice").unbind().click(function () {
            Table.Init();
            Table.Header('CreateInvoice');
            Table.Detail('CreateInvoice');
        });

        $("#divPosting").unbind().click(function () {
            Table.Init();
            Table.Header('Posting');
            Table.Detail('Posting');
        });

        $("#divSubmitDocInvoice").unbind().click(function () {
            Table.Init();
            Table.Header('SubmitInvoice');
            Table.Detail('SubmitInvoice');
        });

        $("#divReceiptARR").unbind().click(function () {
            Table.Init();
            Table.Header('ReceiptARR');
            Table.Detail('ReceiptARR');
        });

        $("#divSubmitOperator").unbind().click(function () {
            Table.Init();
            Table.Header('SubmitOperator');
            Table.Detail('SubmitOperator');
        });

        $("#divLT1").unbind().click(function () {
            Table.Init();
            Table.Header('LT1');
            Table.Detail('LT1');
        });

        $("#divLT2").unbind().click(function () {
            Table.Init();
            Table.Header('LT2');
            Table.Detail('LT2');
        });

        $("#divLT3").unbind().click(function () {
            Table.Init();
            Table.Header('LT3');
            Table.Detail('LT3');
        });

        $("#divMatchingAR").unbind().click(function () {
            Table.Init();
            Table.Header('MatchingAR');
            Table.Detail('MatchingAR');
        });

        $("#btReset").unbind().click(function () {
            $('#slCustomer').val(null).trigger('change');
            $('#slCompany').val(null).trigger('change');
            $('#slAgingCategory').val(null).trigger('change');
            $('#slInvoiceCategory').val(null).trigger('change');
            Control.BindingYear('tbBapsReceiveYearStart');
            Control.BindingYear('tbBapsReceiveYearEnd');
            Control.BindingYear('tbInvoiceYearStart');
            Control.BindingYear('tbInvoiceYearEnd');
        });

        $("#btExportAll").unbind().click(function () {
            Table.ExportAll();
        });

        $('#chExcludePKP').on("switchChange.bootstrapSwitch", function (event, state) {
            if ($("#chExcludePKP").bootstrapSwitch("state") == false)
                PKP = "";
            else
                PKP = Constants.CompanyCode.PKP;

            Bind.Summary();
        });
    },

    BindingSelectCustomer: function (element) {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET"
        }).done(function (data, textStatus, jqXHR) {
            $('#' + element).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $('#' + element).append("<option value='" + item.OperatorId + "'>" + item.Operator + "</option>");
                });
            }
            $('#' + element).select2({ placeholder: "Select Customer", width: null });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectCompany: function (element) {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $('#' + element).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $('#' + element).append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                });
            }
            $('#' + element).select2({ placeholder: "Select Company", width: null });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingYear: function (element) {
        var date = new Date();
        var today = new Date(date.getFullYear(), date.getMonth(), date.getDate());
        $('#' + element).datepicker({
            format: "dd-M-yyyy",
            endDate: '+0d',
            orientation: "bottom"
        });
        $('#' + element).datepicker("setDate", today);
    },

    BindingBeginYear: function (element) {
        var date = new Date();
        var begin = new Date(date.getFullYear(), "01" - 1, "01");
        $('#' + element).datepicker({
            format: "dd-M-yyyy",
            orientation: "bottom"
        });
        $('#' + element).datepicker("setDate", begin);
    },

    SetParams: function () {
        params = {
            vOperator: $("#slCustomer").val(),
            vCompany: $("#slCompany").val(),
            vBAPSReceiveStart: $("#tbBapsReceiveYearStart").val(),
            vBAPSReceiveEnd: $("#tbBapsReceiveYearEnd").val(),
            vAgingCategory: $("#slAgingCategory").val(),
            vInvoiceCategory: $("#slInvoiceCategory").val(),
            vInvoiceDateStart: $("#tbInvoiceYearStart").val(),
            vInvoiceDateEnd: $("#tbInvoiceYearEnd").val(),
            vPKP: PKP
        }
    }
}

var Bind = {
    Summary: function () {
        $('.panelSearchResult').fadeIn(1000);
        $('#pnlDetail').fadeOut();

        $('[name=rbGroup]').prop("disabled", false);

        Control.SetParams();

        if ($('input[name=rbGroup]:checked').val() == 1)
            Bind.SummaryOutStanding();
        else
            Bind.SummaryProduction();
    },

    SummaryOutStanding: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        $.ajax({
            url: "/api/DashboardInvoiceProduction/getSummaryOutStanding",
            type: "POST",
            data: params,
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            var _createInvoice = data['CreateInvoice'];
            $('#CreateInvoice').html(_createInvoice + " D");
            if (_createInvoice > 1)    
                $('#CreateInvoice').css({ 'color': 'red', 'font-weight': 'bold' });
            else if (_createInvoice < 1)
                $('#CreateInvoice').css({ 'color': 'green', 'font-weight': 'bold' });
            else 
                $('#CreateInvoice').css({ 'color': 'yellow', 'font-weight': 'bold' });
            
            var _posting = data['Posting'];
            $('#Posting').html(_posting + " D");
            if (_posting > 1)
                $('#Posting').css({ 'color': 'red', 'font-weight': 'bold' });
            else if (_posting < 1)
                $('#Posting').css({ 'color': 'green', 'font-weight': 'bold' });
            else
                $('#Posting').css({ 'color': 'yellow', 'font-weight': 'bold' });

            var _submitDocInvoice = data['SubmitDocInvoice'];
            $('#SubmitDocInvoice').html(_submitDocInvoice + " D");
            if (_submitDocInvoice > 2)
                $('#SubmitDocInvoice').css({ 'color': 'red', 'font-weight': 'bold' });
            else if (_submitDocInvoice < 2)
                $('#SubmitDocInvoice').css({ 'color': 'green', 'font-weight': 'bold' });
            else
                $('#SubmitDocInvoice').css({ 'color': 'yellow', 'font-weight': 'bold' });

            var _receiptARR = data['ReceiptARR'];
            $('#ReceiptARR').html(_receiptARR + " D");
            if (_receiptARR > 1)
                $('#ReceiptARR').css({ 'color': 'red', 'font-weight': 'bold' });
            else if (_receiptARR < 1)
                $('#ReceiptARR').css({ 'color': 'green', 'font-weight': 'bold' });
            else
                $('#ReceiptARR').css({ 'color': 'yellow', 'font-weight': 'bold' });

            var _submitOperator = data['SubmitOperator'];
            $('#SubmitOperator').html(_submitOperator + " D");
            if (_submitOperator > 1)
                $('#SubmitOperator').css({ 'color': 'red', 'font-weight': 'bold' });
            else if (_submitOperator < 1)
                $('#SubmitOperator').css({ 'color': 'green', 'font-weight': 'bold' });
            else
                $('#SubmitOperator').css({ 'color': 'yellow', 'font-weight': 'bold' });

            var _lt1 = data['LT1'];
            $('#LT1').html(_lt1 + " D");
            if (_lt1 > 36)
                $('#LT1').css({ 'color': 'red', 'font-weight': 'bold' });
            else if (_lt1 < 36)
                $('#LT1').css({ 'color': 'green', 'font-weight': 'bold' });
            else
                $('#LT1').css({ 'color': 'yellow', 'font-weight': 'bold' });

            var _lt2 = data['LT2'];
            $('#LT2').html(_lt2 + " D");
            if (_lt2 > 32)
                $('#LT2').css({ 'color': 'red', 'font-weight': 'bold' });
            else if (_lt2 < 32)
                $('#LT2').css({ 'color': 'green', 'font-weight': 'bold' });
            else
                $('#LT2').css({ 'color': 'yellow', 'font-weight': 'bold' });

            var _lt3 = data['LT3'];
            $('#LT3').html(_lt3 + " D");
            if (_lt3 > 30)
                $('#LT3').css({ 'color': 'red', 'font-weight': 'bold' });
            else if (_lt3 < 30)
                $('#LT3').css({ 'color': 'green', 'font-weight': 'bold' });
            else
                $('#LT3').css({ 'color': 'yellow', 'font-weight': 'bold' });

            var _matchingAR = data['MatchingAR'];
            $('#MatchingAR').html(_matchingAR + " D");
            if (_matchingAR > 2)
                $('#MatchingAR').css({ 'color': 'red', 'font-weight': 'bold' });
            else if (_matchingAR < 2)
                $('#MatchingAR').css({ 'color': 'green', 'font-weight': 'bold' });
            else
                $('#MatchingAR').css({ 'color': 'yellow', 'font-weight': 'bold' });
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        });
    },

    SummaryProduction: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        $.ajax({
            url: "/api/DashboardInvoiceProduction/getSummaryProduction",
            type: "POST",
            data: params
        }).done(function (data, textStatus, jqXHR) {
            var _createInvoice = data['CreateInvoice'];
            $('#CreateInvoice').html(_createInvoice + " D");
            if (_createInvoice > 1)
                $('#CreateInvoice').css({ 'color': 'red', 'font-weight': 'bold' });
            else if (_createInvoice < 1)
                $('#CreateInvoice').css({ 'color': 'green', 'font-weight': 'bold' });
            else
                $('#CreateInvoice').css({ 'color': 'yellow', 'font-weight': 'bold' });

            var _posting = data['Posting'];
            $('#Posting').html(_posting + " D");
            if (_posting > 1)
                $('#Posting').css({ 'color': 'red', 'font-weight': 'bold' });
            else if (_posting < 1)
                $('#Posting').css({ 'color': 'green', 'font-weight': 'bold' });
            else
                $('#Posting').css({ 'color': 'yellow', 'font-weight': 'bold' });

            var _submitDocInvoice = data['SubmitDocInvoice'];
            $('#SubmitDocInvoice').html(_submitDocInvoice + " D");
            if (_submitDocInvoice > 2)
                $('#SubmitDocInvoice').css({ 'color': 'red', 'font-weight': 'bold' });
            else if (_submitDocInvoice < 2)
                $('#SubmitDocInvoice').css({ 'color': 'green', 'font-weight': 'bold' });
            else
                $('#SubmitDocInvoice').css({ 'color': 'yellow', 'font-weight': 'bold' });

            var _receiptARR = data['ReceiptARR'];
            $('#ReceiptARR').html(_receiptARR + " D");
            if (_receiptARR > 1)
                $('#ReceiptARR').css({ 'color': 'red', 'font-weight': 'bold' });
            else if (_receiptARR < 1)
                $('#ReceiptARR').css({ 'color': 'green', 'font-weight': 'bold' });
            else
                $('#ReceiptARR').css({ 'color': 'yellow', 'font-weight': 'bold' });

            var _submitOperator = data['SubmitOperator'];
            $('#SubmitOperator').html(_submitOperator + " D");
            if (_submitOperator > 1)
                $('#SubmitOperator').css({ 'color': 'red', 'font-weight': 'bold' });
            else if (_submitOperator < 1)
                $('#SubmitOperator').css({ 'color': 'green', 'font-weight': 'bold' });
            else
                $('#SubmitOperator').css({ 'color': 'yellow', 'font-weight': 'bold' });

            var _lt1 = data['LT1'];
            $('#LT1').html(_lt1 + " D");
            if (_lt1 > 36)
                $('#LT1').css({ 'color': 'red', 'font-weight': 'bold' });
            else if (_lt1 < 36)
                $('#LT1').css({ 'color': 'green', 'font-weight': 'bold' });
            else
                $('#LT1').css({ 'color': 'yellow', 'font-weight': 'bold' });

            var _lt2 = data['LT2'];
            $('#LT2').html(_lt2 + " D");
            if (_lt2 > 32)
                $('#LT2').css({ 'color': 'red', 'font-weight': 'bold' });
            else if (_lt2 < 32)
                $('#LT2').css({ 'color': 'green', 'font-weight': 'bold' });
            else
                $('#LT2').css({ 'color': 'yellow', 'font-weight': 'bold' });

            var _lt3 = data['LT3'];
            $('#LT3').html(_lt3 + " D");
            if (_lt3 > 30)
                $('#LT3').css({ 'color': 'red', 'font-weight': 'bold' });
            else if (_lt3 < 30)
                $('#LT3').css({ 'color': 'green', 'font-weight': 'bold' });
            else
                $('#LT3').css({ 'color': 'yellow', 'font-weight': 'bold' });

            var _matchingAR = data['MatchingAR'];
            $('#MatchingAR').html(_matchingAR + " D");
            if (_matchingAR > 2)
                $('#MatchingAR').css({ 'color': 'red', 'font-weight': 'bold' });
            else if (_matchingAR < 2)
                $('#MatchingAR').css({ 'color': 'green', 'font-weight': 'bold' });
            else
                $('#MatchingAR').css({ 'color': 'yellow', 'font-weight': 'bold' });
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        });
    }
}

var Table = {
    Init: function () {
        $('#pnlDetail').fadeIn(1000);
        $('.panelSearchResult').fadeOut();

        $('[name=rbGroup]').prop("disabled", true);

        var tbHeader = $('#tbHeader').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });
    },

    Header: function (vType) {
        App.blockUI({ target: ".portlet-Main", animate: !0 });
        
        var tbHeader = $("#tbHeader").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/DashboardInvoiceProduction/getHeader",
                "type": "POST",
                "datatype": "json",
                "data": params = {
                    vOperator: $("#slCustomer").val(),
                    vCompany: $("#slCompany").val(),
                    vBAPSReceiveStart: $("#tbBapsReceiveYearStart").val(),
                    vBAPSReceiveEnd: $("#tbBapsReceiveYearEnd").val(),
                    vAgingCategory: $("#slAgingCategory").val(),
                    vInvoiceCategory: $("#slInvoiceCategory").val(),
                    vInvoiceDateStart: $("#tbInvoiceYearStart").val(),
                    vInvoiceDateEnd: $("#tbInvoiceYearEnd").val(),
                    vPKP: PKP,
                    vType: vType,
                    vGroup: $('input[name=rbGroup]:checked').val()
                },
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.ExportHeader(vType);
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
                    "data": "InvoiceNumber",
                    render: function (data, type, row, meta) {
                        return meta.row + meta.settings._iDisplayStart + 1;
                    }
                },
                { data: "InvoiceNumber" },
                { data: "SubjectInvoice" },
                { data: "AmountInvoice", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Operator" },
                { data: "Company" },
                { data: "TypeInvoice" },
                {
                    data: "CreateInvoice", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "PostingInvoice", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "SubmitDocInvoice", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "ApproveDocInvoice", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "ReceiptDocInvoice", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "TypePayment" },
                {
                    data: "PaymentDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "DocumentPayment" },
                {
                    data: "DocumentPaymentIntegration", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
            ],
            "columnDefs": [
                { "targets": [0], "width": "10%", "className": "dt-center", "orderable": true },
                { "targets": [1, 2], "className": "dt-left" },
                { "targets": [3], "className": "dt-right" },
                { "targets": [4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15], "className": "dt-center" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".portlet-Main", boxed: true });
            },
            "fnDrawCallback": function () {
                App.unblockUI(".portlet-Main");
            },
            "order": []
        });
    },

    Detail: function (vType) {
        App.blockUI({ target: ".portlet-Main", animate: !0 });

        var tbDetail = $("#tbDetail").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/DashboardInvoiceProduction/getDetail",
                "type": "POST",
                "datatype": "json",
                "data": params = {
                    vOperator: $("#slCustomer").val(),
                    vCompany: $("#slCompany").val(),
                    vBAPSReceiveStart: $("#tbBapsReceiveYearStart").val(),
                    vBAPSReceiveEnd: $("#tbBapsReceiveYearEnd").val(),
                    vAgingCategory: $("#slAgingCategory").val(),
                    vInvoiceCategory: $("#slInvoiceCategory").val(),
                    vInvoiceDateStart: $("#tbInvoiceYearStart").val(),
                    vInvoiceDateEnd: $("#tbInvoiceYearEnd").val(),
                    vPKP: PKP,
                    vType: vType,
                    vGroup: $('input[name=rbGroup]:checked').val()
                },
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.ExportDetail(vType);
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
                    "data": "SONumber",
                    render: function (data, type, row, meta) {
                        return meta.row + meta.settings._iDisplayStart + 1;
                    }
                },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "SiteIDOpr" },
                { data: "SiteNameOpr" },
                { data: "Operator" },
                { data: "Company" },
                { data: "BapsType" },
                {
                    data: "StartPeriodInvoice", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndPeriodInvoice", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "AmountInvoice", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                {
                    data: "ReceiveDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "ConfirmDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "InvoiceNumber" },
                {
                    data: "CreateInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "PostingDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "PrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "SubmitChecklistDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "ApproveChecklistDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
            ],
            "columnDefs": [
                { "targets": [0], "width": "10%", "className": "dt-center", "orderable": true },
                { "targets": [1, 2, 4, 6, 7, 8, 9, 10, 11, 13, 14, 15, 16, 17], "className": "dt-center" },
                { "targets": [12], "className": "dt-left" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".portlet-Main", boxed: true });
            },
            "fnDrawCallback": function () {
                App.unblockUI(".portlet-Main");
            },
            "order": [],
        });
    },

    ExportHeader: function (vType) {
        var params = {
            vOperator: $("#slCustomer").val(),
            vCompany: $("#slCompany").val(),
            vBAPSReceiveStart: $("#tbBapsReceiveYearStart").val(),
            vBAPSReceiveEnd: $("#tbBapsReceiveYearEnd").val(),
            vAgingCategory: $("#slAgingCategory").val(),
            vInvoiceCategory: $("#slInvoiceCategory").val(),
            vInvoiceDateStart: $("#tbInvoiceYearStart").val(),
            vInvoiceDateEnd: $("#tbInvoiceYearEnd").val(),
            vPKP: PKP,
            vType: vType,
            vGroup: $('input[name=rbGroup]:checked').val()
        }

        window.location.href = "/Dashboard/InvoiceProduction/Header/Export?" + $.param(params);
    },

    ExportDetail: function (vType) {
        var params = {
            vOperator: $("#slCustomer").val(),
            vCompany: $("#slCompany").val(),
            vBAPSReceiveStart: $("#tbBapsReceiveYearStart").val(),
            vBAPSReceiveEnd: $("#tbBapsReceiveYearEnd").val(),
            vAgingCategory: $("#slAgingCategory").val(),
            vInvoiceCategory: $("#slInvoiceCategory").val(),
            vInvoiceDateStart: $("#tbInvoiceYearStart").val(),
            vInvoiceDateEnd: $("#tbInvoiceYearEnd").val(),
            vPKP: PKP,
            vType: vType,
            vGroup: $('input[name=rbGroup]:checked').val()
        }

        window.location.href = "/Dashboard/InvoiceProduction/Detail/Export?" + $.param(params);
    },

    ExportAll: function () {
        var params = {
            vOperator: $("#slCustomer").val(),
            vCompany: $("#slCompany").val(),
            vBAPSReceiveStart: $("#tbBapsReceiveYearStart").val(),
            vBAPSReceiveEnd: $("#tbBapsReceiveYearEnd").val(),
            vAgingCategory: $("#slAgingCategory").val(),
            vInvoiceCategory: $("#slInvoiceCategory").val(),
            vInvoiceDateStart: $("#tbInvoiceYearStart").val(),
            vInvoiceDateEnd: $("#tbInvoiceYearEnd").val(),
            vPKP: PKP,
            vGroup: $('input[name=rbGroup]:checked').val()
        }

        window.location.href = "/Dashboard/InvoiceProduction/All/Export?" + $.param(params);
    }
}

var Constants = {
    CompanyCode: {
        PKP: "PKP"
    }
}

var PKPPurpose = {
    Filter: {
        PKPOnly: function () {
            $('#slCompany').val(Constants.CompanyCode.PKP).trigger('change');
        },
    },
    Set: {
        UserCompanyCode: function () {
            PKPPurpose.Temp.UserCompanyCode = $('#hdUserCompanyCode').val();
        }
    },
    Temp: {
        UserCompanyCode: ""
    }
}


