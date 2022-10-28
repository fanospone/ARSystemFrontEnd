Data = {};
ProcessData = [];
DoneData = [];

var fsUserID = "";
var fsCompanyId = "";
var fsCustomerID = "";
var fsBAPS = "";
var fsPO = "";
var fsSONumber = "";
var fsTransactionID = "";
var CurrentTab = 0;
var fsYear = "";
var fsQuartal = "";
var fsBapsType = "";

var fsID = "";
var fsTerm = "";
var fsResult = "";
var fsStartPeriodDate = "";
var fsEndPeriodDate = "";
var fsStartDate = "";
var fsEndDate = "";
var fsTotalPrice = "";
var fsCounter = "";
var fsBaseLeasePrice = "";
var fsServicePrice = "";
var fsState = "";
var basePath = "";

jQuery(document).ready(function () {
    $("#slSearchBAPS").select2({
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

    $("#slSearchPO").select2({
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

    Form.Init();
    Table.Init();
    //Table.SortBySiteName();
    GridView.Init();
    //Table.Reset();
    Data.RowSelectedRaw = [];
    Data.RowSelectedProcess = [];

    var renderType = $('#renderType').val();
    var operatorType = $('#renderOperator').val();
    var renderDate = $('#renderDate').val();

    console.log(renderType)
    console.log(operatorType)
    console.log(renderDate)

    if (
       (renderType == '' || renderType == undefined || renderType == null) &&
       (operatorType == '' || operatorType == undefined || operatorType == null) &&
       (renderDate == '' || renderDate == undefined || renderDate == null)) {
        TreeView.Search();
    }
    else {
        $('#tbStartPeriod').val(renderDate),
        $('#tbEndPeriod').val(renderDate),
        $('#slSearchCustomer').val(operatorType).change();

        $('#gridViewTab').click();
        GridView.Search();
    }

    $("#formSearch").submit(function (e) {

        e.preventDefault();

        if (CurrentTab == 0) {
            if ($('#treeView').hasClass('active'))
                TreeView.Search();
            if ($('#gridView').hasClass('active'))
                GridView.Search();
        }
        else {
            Table.Search();
        }
    });

    $("#btSearch").click(function (e) {
        e.preventDefault();
        if ($("#formSearch").parsley().validate()) {

            var params = Control.GetParam();
            //if (params.CustomerID == 'TSEL' && CurrentTab == 1 && (params.BAPS == null || params.BAPS == "")) {
            //    Common.Alert.Warning("Please choose BAPS !"); return;
            //}


            if (CurrentTab == 0) {
                if (params.HOStartDate == "" || params.HOEndDate == "") {
                    Common.Alert.Warning("Handover Date is Required!");
                    return;
                }
                else {
                    if ($('#treeView').hasClass('active'))
                        TreeView.Search();
                    if ($('#gridView').hasClass('active'))
                        GridView.Search();
                }
            }
            else {
                //if (params.CompanyID == "") {
                //    Common.Alert.Warning("Company is Required!");
                //    return;
                //}
                //else {
                //    Table.Search();
                //}

                Table.Search();

            }
        }
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
    });

    $('#tabRTI').unbind().on('tabsactivate', function (event, ui) {
        var newIndex = ui.newTab.index();
        CurrentTab = 0;
        var vparams = Control.GetParam();
        $('#btSave').show();

        if (newIndex == 0) {
            //if (vparams.CustomerID != null && vparams.CompanyID != null && vparams.BAPS != null && vparams.CustomerID != '' && vparams.CompanyID != '' && vparams.BAPS != '') {
            if (vparams.BAPS != null && vparams.HOStartDate != null && vparams.HOEndDate != null && vparams.BAPS != '' && vparams.HOStartDate != '' && vparams.HOEndDate != '') {
                if ($('#treeView').hasClass('active'))
                    TreeView.Search();
                if ($('#gridView').hasClass('active'))
                    GridView.Search();
            }
        }
        else {
            CurrentTab = CurrentTab + 1;
            $('#pnlUpload').hide();
            $('#btSave').hide();
            //if (vparams.CustomerID != null && vparams.CompanyID != null && vparams.BAPS != null && vparams.CustomerID != '' && vparams.CompanyID != '' && vparams.BAPS != '')
            if (vparams.BAPS != null && vparams.HOStartDate != null && vparams.HOEndDate != null && vparams.BAPS != '' && vparams.HOStartDate != '' && vparams.HOEndDate != '') {
                Table.Search();
            }
        }
    });

    $('#tblRaw').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            Data.RowSelectedRaw.push(parseInt(id));
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row" + id).removeClass("active");
            Helper.RemoveElementFromArray(Data.RowSelectedRaw, parseInt(id));
        }
    });
});

var Form = {
    Init: function () {
        Control.BindingSelectOperators($('#slSearchCustomer'));
        Control.BindingSelectCompany($('#slSearchCompany'));
        Control.BindingSelectBapsType($('#slSearchBapsType'));
        Control.BindingSelectPowerType($('#slSearchPowerType'));

        $("#slSearchCompany").val("").trigger("change");
        $("#slSearchCustomer").val("").trigger("change");
        //$("#slRenewalYear").select2({ placeholder: "Select Renewal Year", width: null });
        //$("#slQuartal").select2({ placeholder: "Select Quartal", width: null });
        //$("#slRenewalYear").val("").trigger("change");
        //$("#slQuartal").val("").trigger("change");

    
        $(".panelSearchZero").hide();
        $('#tabRTI').tabs();
    },
    View: function () {
        var params = Control.GetParam();
        if (params.CustomerID == 'TSEL') {
            $('#slSearchPO').hide();
            if (params.CompanyID != null && params.CompanyID != '')
                Control.BindingSelectBAPS($("#slSearchBAPS"));
        }
    },
    CheckingPeriod: function () {
        var l = Ladda.create(document.querySelector("#btSaveSplitData"))
        l.start();

        fsStartDate = $("#tbStartInvoiceDate").val() == "" ? 0 : $("#tbStartInvoiceDate").val();

        var params = {
            trxReconcileID: fsID,
            StartInvoiceDate: fsStartDate,
            Term: fsTerm
        };

        $.ajax({
            url: "/api/RTI/CheckingPeriod",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            async: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (data) {
                fsResult = data;
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
    },
}

var Table = {
    Init: function () {
        var tblRaw = $('#tblRaw').DataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblRaw").DataTable().columns.adjust().draw();
        });
    },
    SortBySiteName:function(){

        $('#tblRaw thead').on('click', 'tr th:eq(5)', function () {
            const isOrder = true;
            Table.SearchWithSorting(isOrder);
        });

    },

    SearchWithSorting: function (isOrder) {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        var i = 0;
        $('#pnlUpload').hide();

        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsCustomerId = $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val();
        fsBAPSNumber = $("#slSearchBAPS").val() == null ? "" : $("#slSearchBAPS").val();
        //fsSONumber = $("#slSONumber").val() == null ? "" : $("#slSONumber").val();
        //fsYear = $("#slRenewalYear").val() == null ? "" : $("#slRenewalYear").val();
        //fsQuartal = $("#slQuartal").val() == null ? "" : $("#slQuartal").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsPONumber = $("#slSearchPO").val() == null ? "" : $("#slSearchPO").val();

        var params = {
            CompanyId: fsCompanyId,
            CustomerId: fsCustomerId,
            strBAPSNumber: fsBAPSNumber,
            strSONumber: fsSONumber,
            Year: fsYear,
            Quartal: fsQuartal,
            BapsType: fsBapsType,
            strPONumber: fsPONumber,
            IsOrder: true
        };

        var tblRaw = $("#tblRaw").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/handoverDocRTI/gridReconcile",
                "type": "POST",
                "datatype": "json",
                //"data": Control.GetParam(),
                "data": params
            },
            buttons: [
                 {
                     text: '<i class="fa fa-file-zip-o"></i> Download Doc', titleAttr: 'Compressed All', className: 'btn red btn-outline',
                     action: function (e, dt, node, config) {
                         Table.DownloadAllArchive();
                     }
                 },
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.Export();
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

                     if (full.FilePath == null)
                         //return "<a class='btUpload'><i class='fa fa-upload'></i></a>";
                         return "";
                     else
                         var FileNames = full.FilePath.split('\\');
                     var Name = FileNames[3].toString();
                     var basePath = $('#baseRoute').val();
                     //return "<a class='files' target='_blank' href='/RevenueAssurance/Download?FilePath=" + full.FilePath + "&PONumber=0&FileName=" + Name + "&ContentType=" + full.ContentType + "'><i class='fa fa-download'></i></a>";
                     return `<a class='files' href='${basePath}${full.FilePath}' download='${Name}' target="_blank"><i class='fa fa-download'></i></a>`;
                 }
             },
             { data: "SONumber" },
             { data: "SiteID" },
             { data: "SiteName" },

             { data: "CustomerSiteID" },
             { data: "CustomerSiteName" },
             { data: "RegionName" },
             { data: "ProvinceName" },

             { data: "ResidenceName" },
             { data: "PONumber" },
             { data: "BAPSNumber" },
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
             { data: "BaseLeaseCurrency" },
             { data: "ServiceCurrency" },
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

             {
                 data: "BAPSConfirmDate", render: function (data) {
                     return Common.Format.ConvertJSONDateTime(data);
                 }
             }, {
                 data: "CreateInvoiceDate", render: function (data) {
                     return Common.Format.ConvertJSONDateTime(data);
                 }
             }, {
                 data: "PostingInvoiceDate", render: function (data) {
                     return Common.Format.ConvertJSONDateTime(data);
                 }
             },
             { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
             { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
             { data: "DeductionAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
             { data: "InflationAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
             { data: "TotalPaymentRupiah", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

            ],
            "columnDefs": [{ "targets": [0], "orderable": true }],
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

            }
        });

        $(window).resize(function () {
            $("#tblRaw").DataTable().columns.adjust().draw();
        });
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        var i = 0;
        $('#pnlUpload').hide();

        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsCustomerId = $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val();
        fsBAPSNumber = $("#slSearchBAPS").val() == null ? "" : $("#slSearchBAPS").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsPONumber = $("#slSearchPO").val() == null ? "" : $("#slSearchPO").val();

        var params = {
            CompanyId: fsCompanyId,
            CustomerId: fsCustomerId,
            strBAPSNumber: fsBAPSNumber,
            strSONumber: fsSONumber,
            Year: fsYear,
            Quartal: fsQuartal,
            BapsType: fsBapsType,
            strPONumber: fsPONumber,
            strStartDate: $('#tbStartPeriod').val(),
            strEndDate: $('#tbEndPeriod').val(),
        };

        var tblRaw = $("#tblRaw").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/handoverDocRTI/gridReconcile",
                "type": "POST",
                "datatype": "json",
                //"data": Control.GetParam(),
                "data": params
            },
            buttons: [
                {
                    text: '<i class="fa fa-file-zip-o"></i> Download Doc', titleAttr: 'Compressed All', className: 'btn blue btn-outline',
                    action: function (e, dt, node, config) {
                        Table.DownloadAllArchive();
                    }
                },
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.Export();
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

                        if (full.FilePath == null)
                            //return "<a class='btUpload'><i class='fa fa-upload'></i></a>";
                            return "";
                        else
                            var FileNames = full.FilePath.split('\\');
                        var Name = FileNames[3].toString();
                        var basePath = $('#baseRoute').val();
                        //return "<a class='files' target='_blank' href='/RevenueAssurance/Download?FilePath=" + full.FilePath + "&PONumber=0&FileName=" + Name + "&ContentType=" + full.ContentType + "'><i class='fa fa-download'></i></a>";
                        return `<a class='files' href='${basePath}${full.FilePath}' download='${Name}' target="_blank"><i class='fa fa-download'></i></a>`;
                    }
                },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },

                { data: "CustomerSiteID" },
                { data: "CustomerSiteName"},
                { data: "RegionName" },
                { data: "ProvinceName" },

                { data: "ResidenceName" },
                { data: "PONumber" },
                { data: "BAPSNumber" },
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
                { data: "BaseLeaseCurrency" },
                { data: "ServiceCurrency" },
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

                {
                    data: "BAPSConfirmDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                }, {
                        data: "CreateInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                }, {
                     data: "PostingInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "DeductionAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InflationAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "TotalPaymentRupiah", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

            ],
            "columnDefs": [{ "targets": [0], "orderable": true }],
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

            }
        });

        $(window).resize(function () {
            $("#tblRaw").DataTable().columns.adjust().draw();
        });
    },

    Reset: function () {
        $("#slSearchCompany").val("").trigger("change");
        $("#slSearchCustomer").val("").trigger("change");
        //$("#slRenewalYear").val("").trigger("change");
        //$("#slQuartal").val("").trigger("change");
        $("#slSearchBapsType").val("TOWER").trigger("change");
        $("#slSearchPowerType").val("").trigger("change");
        //$("#slSONumber").val("").trigger("change");
        $("#slSearchBAPS").val("").trigger("change");
        $("#slSearchPO").val("").trigger("change");

        $('#tbStartPeriod').val($('#defaultStartDate').val());
        $('#tbEndPeriod').val($('#defaultEndDate').val());


        var tblRaw = $("#tblRaw").DataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

    },

    Export: function () {
        var params = Control.GetParam();
        fsCompanyId = params.CompanyID;
        fsCustomerID = params.CustomerID;
        fsBAPS = params.BAPS;
        fsPO = params.PO;
        fsYear = '';
        fsQuartal = '';
        fsBapsType = params.BapsType;
        fsPowerType = params.PowerType;
        var SoNumberFilter = '';

        window.location.href = "/InvoiceTransaction/Input/ExportRTIDone?strCompanyId=" + fsCompanyId
            + "&strOperator=" + fsCustomerID
            + "&strBAPS=" + fsBAPS
            + "&strPO=" + fsPO
            + "&strYear=" + fsYear
            + "&strQuartal=" + fsQuartal
            + "&strBapsType=" + fsBapsType
            + "&strPowerType=" + fsPowerType
            + "&SONumber=" + SoNumberFilter
            + "&strStartDate=" + $('#tbStartPeriod').val()
            + "&strEndDate=" + $('#tbEndPeriod').val()
            + "&isRaw=1";
    },

    DownloadAllArchive: function () {
        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsCustomerId = $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val();
        fsBAPSNumber = $("#slSearchBAPS").val() == null ? "" : $("#slSearchBAPS").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsPONumber = $("#slSearchPO").val() == null ? "" : $("#slSearchPO").val();

        var params = {
            CompanyId: fsCompanyId,
            CustomerId: fsCustomerId,
            strBAPSNumber: fsBAPSNumber,
            strSONumber: fsSONumber,
            Year: fsYear,
            Quartal: fsQuartal,
            BapsType: fsBapsType,
            strPONumber: fsPONumber,
            strStartDate: $('#tbStartPeriod').val(),
            strEndDate: $('#tbEndPeriod').val(),
        };

        if ($("#slSearchBapsType").val() == '' || $("#slSearchBapsType").val() == null ||
            $('#tbStartPeriod').val() == '' || $('#tbStartPeriod').val() == null ||
            $('#tbEndPeriod').val() == '' || $('#tbEndPeriod').val() == null) {
            Common.Alert.Warning('Please, fill all required fields!');
        }
        
        $('#onloader').show();
        $.ajax({
            url: "/api/handoverDocRTI/getArchiveRTIByRange",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
        })
       .done(function (data, textStatus, jqXHR) {
           $('#onloader').hide();
           if (data != 0 || data != null) {
               if (data.zipPath != '') {
                   let hostname = location.hostname;
                   $('#downloadAllZip').attr("href", data.zipPath);
                   const link = document.getElementById('downloadAllZip');
                   window.location.href = link.href;
               }
               else {
                   console.log(data.innerMessage);
                   Common.Alert.Warning(data.message);
               }
           }
           else {
               Common.Alert.Warning("Something Wrong, Please Contact System Support!");
           }
       })
       .fail(function (jqXHR, textStatus, errorThrown) {
           $('#onloader').hide();
           Common.Alert.Error("Internal server error");
       });
    }
}

var TreeView = {
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        var i = 0;
        $('#pnlUpload').hide();
        console.log($('#slSearchBapsType').val());

        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsCustomerId = $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val();
        fsBAPSNumber = $("#slSearchBAPS").val() == null ? "" : $("#slSearchBAPS").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsPONumber = $("#slSearchPO").val() == null ? "" : $("#slSearchPO").val();

        var params = {
            strCompanyId: fsCompanyId,
            strCustomerID: fsCustomerId,
            strBAPSNumber: fsBAPSNumber,
            strBAPSType: $("#slSearchBapsType").val(),
            strStartDate: $('#tbStartPeriod').val(),
            strEndDate: $('#tbEndPeriod').val(),

        };

        var monthList = ['January', 'Februari', 'Maret', 'April', 'Mei', 'Juni', 'Juli', 'Agustus', 'September', 'Oktober', 'November', 'Desember'];

        $.ajax({
            url: "/api/handoverDocRTI/getBapsDoneTree",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
        })
        .done(function (data, textStatus, jqXHR) {
            l.stop();
            if (data != 0) {
                var year = '';
                var month = '';
                var day = '';
                var treeHtml = '';
                $("#jstree").jstree("destroy")
                $("#jstree").jstree({
                    "core": {
                        "themes": {
                            "responsive": false
                        },
                        'data': data.treeViewData
                    },
                    "types": {
                        "default": {
                            "icon": "fa fa-folder text-warning"
                        },
                        "file": {
                            "icon": "fa fa-file  text-warning"
                        }
                    },
                    "plugins": ["types"],
                });

      
                // 8 interact with the tree - either way is OK
                $('button').on('click', function () {
                    $('#jstree').jstree(true).select_node('child_node_1');
                    $('#jstree').jstree('select_node', 'child_node_1');
                    $.jstree.reference('#jstree').select_node('child_node_1');
                });

                // 7 bind to events triggered on the tree
                $('#jstree').on("changed.jstree", function (e, data) {
                    if (data.node.data.length != 0) {
                        $('#onloader').show();
                        var params = {
                            DoneDate: data.node.data[0].DoneDate,
                            CompanyID: data.node.data[0].CompanyID,
                            CustomerID: data.node.data[0].CustomerID,
                            SoNumber: data.node.data[0].SoNumber,
                            TowerTypeID: $("#slSearchBapsType").val(),
                        };

                        $.ajax({
                            url: "/api/handoverDocRTI/getArchive",
                            type: "POST",
                            dataType: "json",
                            contentType: "application/json",
                            data: JSON.stringify(params),
                            cache: false,
                        })
                       .done(function (data, textStatus, jqXHR) {
                           $('#onloader').hide();
                           if (data != 0 || data != null) {
                               if (data.zipPath != '') {
                                   let hostname = location.hostname;
                                   $('#downloadZip').attr("href", data.zipPath);
                                   const link = document.getElementById('downloadZip');
                                   //window.location = link.href;
                                   window.location.href = link.href;
                                   console.log(data.zipPath);
                               }
                               else {
                                   console.log(data.innerMessage);
                                   Common.Alert.Warning(data.message);
                               }
                           }
                           else {
                               Common.Alert.Warning("Something Wrong, Please Contact System Support!");
                           }
                           l.stop();

                       })
                       .fail(function (jqXHR, textStatus, errorThrown) {
                           $('#onloader').hide();
                           Common.Alert.Error("Internal server error");
                           l.stop();

                       });

                    }
                });
                if (data.treeViewData.length === 0) 
                    $("#treeNotFound").show();
                else 
                    $("#treeNotFound").hide();

                l.stop();
                $(".panelSearchBegin").hide();
          
            }
            else {
                Common.Alert.Warning("Something Wrong, Please Contact System Support!");
                $("#treeNotFound").show();
                l.stop();
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            $("#treeNotFound").show();
            l.stop();
        });
    }
}

var GridView = {
    Init: function () {
        var tblBapsDone = $('#tblBapsDone').DataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblBapsDone").DataTable().columns.adjust().draw();
        });
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        //fsSoNumb = $("#slDSearchSoNumber").val() == null ? "" : $("#slDSearchSoNumber").val();
        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsCustomerId = $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val();
        fsBAPSNumber = $("#slSearchBAPS").val() == null ? "" : $("#slSearchBAPS").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsPONumber = $("#slSearchPO").val() == null ? "" : $("#slSearchPO").val();

        var params = {
            strCompanyId: fsCompanyId,
            strCustomerID: fsCustomerId,
            strBAPSType: $("#slSearchBapsType").val(),
            strStartDate: $('#tbStartPeriod').val(),
            strEndDate: $('#tbEndPeriod').val(),
        };

        GridView.Init();
        var tblBapsDone = $("#tblBapsDone").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/handoverDocRTI/getBapsDoneGrid",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
                 {
                     text: '<i class="fa fa-file-zip-o"></i> Download Doc', titleAttr: 'Compressed All', className: 'btn blue btn-outline',
                     action: function (e, dt, node, config) {
                         GridView.DownloadAllArchive();
                     }
                 },
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                 {
                     text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                         var l = Ladda.create(document.querySelector(".yellow"));
                         l.start();
                         GridView.Export();
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

                        if (full.FilePath == null)
                            //return "<a class='btUpload'><i class='fa fa-upload'></i></a>";
                            return "";
                        else
                            var FileNames = full.FilePath.split('\\');

                        var Name = FileNames[3].toString();
                        var basePath = $('#baseRoute').val();
                        return `<a class='files' href='${basePath}${full.FilePath}' download='${Name}' target="_blank"><i class='fa fa-download'></i></a>`;
                    }
                },
                {
                    data: "DoneDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                 {
                     data: "BAPSConfirmDate", render: function (data) {
                         return Common.Format.ConvertJSONDateTime(data);
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
                 }
                   

            ],
            "columnDefs": [{ "targets": [0], "orderable": true }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblBapsDone.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                //if (Data.RowSelected.length > 0) {
                //    var item;
                //    for (var i = 0 ; i < Data.RowSelected.length; i++) {
                //        item = Data.RowSelected[i];
                //        $("#Row" + item).addClass("active");
                //    }
                //}
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
    },

    Export: function () {
        mstRAActivityID = 20;
        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsCustomerId = $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val();
        fsBAPSNumber = $("#slSearchBAPS").val() == null ? "" : $("#slSearchBAPS").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();

        var fsProductID = "";

        window.location.href = "/InvoiceTransaction/Input/ExportBapsDone?strCustomerID=" + fsCustomerId
            + "&strCompanyId=" + fsCompanyId + "&strProductId=" + fsProductID
            + "&strStartDate=" + $('#tbStartPeriod').val() + "&strEndDate=" + $('#tbEndPeriod').val()
            + "&strTowerType=" + fsBapsType + "&mstRAActivityID=" + mstRAActivityID;
    },

    DownloadAllArchive: function () {
        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsCustomerId = $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val();
        fsBAPSNumber = $("#slSearchBAPS").val() == null ? "" : $("#slSearchBAPS").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsPONumber = $("#slSearchPO").val() == null ? "" : $("#slSearchPO").val();

        if ($("#slSearchBapsType").val() == '' || $("#slSearchBapsType").val() == null ||
            $('#tbStartPeriod').val() == '' || $('#tbStartPeriod').val() == null ||
            $('#tbEndPeriod').val() == '' || $('#tbEndPeriod').val() == null) {
            Common.Alert.Warning('Please, fill all required fields!');
        }

        var params = {
            CompanyID: fsCompanyId,
            CustomerID: fsCustomerId,
            TowerTypeID: fsBapsType,
            StartDate: $('#tbStartPeriod').val(),
            EndDate: $('#tbEndPeriod').val(),
        };

        $('#onloader').show();
        $.ajax({
            url: "/api/handoverDocRTI/getArchiveBAPSByRange",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
        })
       .done(function (data, textStatus, jqXHR) {
           $('#onloader').hide();
           if (data != 0 || data != null) {
               if (data.zipPath != '') {
                   let hostname = location.hostname;
                   $('#downloadAllZip').attr("href", data.zipPath);
                   const link = document.getElementById('downloadAllZip');
                   window.location.href = link.href;
               }
               else {
                   console.log(data.innerMessage);
                   Common.Alert.Warning(data.message);
               }
           }
           else {
               Common.Alert.Warning("Something Wrong, Please Contact System Support!");
           }
       })
       .fail(function (jqXHR, textStatus, errorThrown) {
           $('#onloader').hide();
           Common.Alert.Error("Internal server error");
       });
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

    BindingSelectOperators: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item.OperatorId.trim() == 'TSEL' || item.OperatorId.trim() == 'ISAT' || item.OperatorId.trim() == 'XL' || item.OperatorId.trim() == 'MITEL' || item.OperatorId.trim() == 'PAB' || item.OperatorId.trim() == 'TELKOM' || item.OperatorId.trim() == 'HCPT')
                        elements.append("<option value='" + item.OperatorId.trim() + "'>" + item.OperatorId + "</option>");
                    //elements.append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                })
            }

            elements.select2({ placeholder: "Select Operator", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectArea: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/AreaList",
            type: "GET",
            data: {}
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("");

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    elements.append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                })
            }

            $("#slSearchArea").val("").trigger("change");
            elements.select2({ placeholder: "Select Area", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectRegional: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/Regional",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("<option></option>")
            elements.append("<option value=' '>No Filter</option>");

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    elements.append("<option value='" + item.RegionalId + "'>" + item.RegionalName + "</option>");
                })
            }

            elements.select2({ placeholder: "Select Regional", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectPO: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/ListPO",
            type: "GET",
            data: {}
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("");

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    elements.append("<option value='" + item.Text + "'>" + item.Text + "</option>");
                })
            }

            $("#slSearchPO").val("").trigger("change");
            //elements.select2({ placeholder: "Select PO", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectBAPS: function (elements) {
        var vparam = this.GetParam();
        var params = "";

        if (vparam.CustomerID == 'TSEL') {
            params = " AND CustomerID = '" + vparam.CustomerID + "' ";

            if (vparam.CompanyID != null) {
                params = params + " AND CompanyID = '" + vparam.CompanyID + "' ";
            }
        }
        else {
            params = " AND CustomerID = '" + vparam.CustomerID + "' ";
        }

        $.ajax({
            url: "/api/MstDataSource/ListBAPS",
            type: "GET",
            data: { param: params }
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("");

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    elements.append("<option value='" + item.Text + "'>" + item.Text + "</option>");
                })
            }

            $("#slSearchBAPS").val("").trigger("change");
            //elements.select2({ placeholder: "Select BAPS", width: null });

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

    GetParam: function () {
        var SONumber = $("#slSONumber").val() == null ? "" : $("#slSONumber").val();
        var SoNumberFilter = "";

        if (SONumber != null && SONumber != "") {
            SoNumberFilter = "0";
            for (var i = 0; i < SONumber.length; i++) {
                SoNumberFilter += ("," + SONumber[i].toString());
            }
        }

        var params = {
            CompanyID: $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val(),
            CustomerID: $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val(),
            BAPS: $("#slSearchBAPS").val() == null ? "" : $("#slSearchBAPS").val().toString(),
            PO: $("#slSearchPO").val() == null ? "" : $("#slSearchPO").val().toString(),
            SONumber: SoNumberFilter,
            HOStartDate: $("#tbStartPeriod").val() == null ? "" : $("#tbStartPeriod").val(),
            HOEndDate: $("#tbEndPeriod").val() == null ? "" : $("#tbEndPeriod").val(),
            BapsType: $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val(),
            PowerType: $("#slSearchPowerType").val() == null ? "" : $("#slSearchPowerType").val(),
            isRaw: CurrentTab
        };

        return params;
    },

    BindProRateAmount: function () {
        //var CustomerID = $('#txtCustomerID').val();
        var CustomerID = fsCustomerID;
        var InvoiceStartDate = $('#tbStartInvoiceDate').val();
        var InvoiceEndDate = $('#tbEndInvoiceDate').val();
        var InvoiceAmount = $('#txtBaseLeasePrice').val().replace(/,/g, "");
        var ServiceAmount = $('#txtServicePrice').val().replace(/,/g, "");
        var DeductionAmount = "0";

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
            $("#txtTotalPrice").val(Common.Format.CommaSeparation(result));
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
}

var Helper = {
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
    GetListId: function (isRaw) {
        var params = {
            CompanyId: fsCompanyId,
            CustomerId: fsCustomerId,
            strBAPSNumber: fsBAPSNumber,
            strSONumber: fsSONumber,
            strPONumber: fsPONumber,
            Year: fsYear,
            Quartal: fsQuartal,
            BapsType: fsBapsType
        }

        var AjaxData = [];
        $.ajax({
            url: "/api/RTI/GetListId",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params),
            //data: params,
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                AjaxData = data;
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
        return AjaxData;
    },
    numformat: function numformat(angka) {
        var numformat = $.fn.dataTable.render.number(',', '.', 2, '').display;
        return numformat(angka)
    },
    Validation: function () {

        fsStartDate = $("#tbStartInvoiceDate").val() == "" ? 0 : $("#tbStartInvoiceDate").val();
        fsEndDate = $("#tbEndInvoiceDate").val() == "" ? 0 : $("#tbEndInvoiceDate").val();

        fsStartDatePeriod = $("#tbStartDate").val() == null ? "" : $("#tbStartDate").val();
        fsEndDatePeriod = $("#tbEndDate").val() == null ? "" : $("#tbEndDate").val();

        var startDateComponents = Helper.ReverseDateToSQLFormat($("#tbStartInvoiceDate").val()).split("/");
        var endDateComponents = Helper.ReverseDateToSQLFormat($("#tbEndInvoiceDate").val()).split("/");
        var startDate = new Date(parseInt(startDateComponents[2]), parseInt(startDateComponents[0]) - 1, parseInt(startDateComponents[1]));
        var endDate = new Date(parseInt(endDateComponents[2]), parseInt(endDateComponents[0]) - 1, parseInt(endDateComponents[1]));

        var startDatePeriodComponents = Helper.ReverseDateToSQLFormat($("#tbStartDate").val()).split("/");
        var endDatePeriodComponents = Helper.ReverseDateToSQLFormat($("#tbEndDate").val()).split("/");
        var startDatePeriod = new Date(parseInt(startDateComponents[2]), parseInt(startDateComponents[0]) - 1, parseInt(startDateComponents[1]));
        var endDatePeriod = new Date(parseInt(endDateComponents[2]), parseInt(endDateComponents[0]) - 1, parseInt(endDateComponents[1]));

        var msg = '';

        if (fsStartDate == "" || fsEndDate == "") {
            msg = msg + "Data must be completed! <br/>";
        }
        else {
            if (fsStartDate < fsStartDatePeriod) {
                msg = msg + " Start Date must same or be greater than Period Start Date ! <br/>";
            }
            else if (endDate > endDatePeriod) {
                msg = msg + " End Date cannot be greater than Period End Date ! <br/>";
            }
        }

        fsResult = false;

        return msg;
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

$(document).ready(function () {

    $('#tabDones').click(function () {
        $('#handoverDate').show();
        //$("#slSearchBAPS").prop('disabled', true);
        //$("#slSearchPO").prop('disabled', true);
        $(".baps-rec-ctl").css("visibility", "hidden");
        $("#slSearchBapsType option[value='PWRUP']").attr("disabled", "disabled");
        $("#slSearchBapsType option[value='INF']").attr("disabled", "disabled");
        $("#slSearchBapsType option[value='ELECT']").attr("disabled", "disabled");
    });

    $('#treeViewTab').click(function () {
        TreeView.Search();
    });

    $('#gridViewTab').click(function () {
        GridView.Search();
    });


    $('#tabRaws').click(function () {
        //$('#handoverDate').hide();
        $('#handoverDate').show();
        //$('#tbStartPeriod').val('');
        //$('#tbEndPeriod').val('');

        $(".baps-rec-ctl").css("visibility", "visible");
        $("#slSearchBapsType option[value='PWRUP']").attr("disabled", false);
        $("#slSearchBapsType option[value='INF']").attr("disabled", false);
        $("#slSearchBapsType option[value='ELECT']").attr("disabled", false);
        Table.Search();

    });

    $('#slSearchBapsType').val('TOWER').change();
    $("#slSearchBapsType option[value='PWRUP']").attr("disabled", "disabled");
    $("#slSearchBapsType option[value='INF']").attr("disabled", "disabled");
    $("#slSearchBapsType option[value='ELECT']").attr("disabled", "disabled");



});

