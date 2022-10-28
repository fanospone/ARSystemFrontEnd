Data = {};

//Filter Search Parameter (For Export Excel Only)
var fsCompanyId;
var fsEndDate;
var fsOperator;
var fsInvoiceType;
var fsAmountTyoe;

PKP = "";

jQuery(document).ready(function () {
    Control.LoadCompany();
    Control.LoadPeriod();
    Control.LoadOperator();
    Control.LoadInvoiceType();
    Control.LoadAmountType();
    PKP = "PKP";
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

    $("#btBackSummary").unbind().click(function (e) {
        $(".summary").fadeIn();
        $(".detail").hide();
    });

    $('#chExcludePKP').on("switchChange.bootstrapSwitch", function (event, state) {
        if ($("#chExcludePKP").bootstrapSwitch("state") == false)
            PKP = "";
        else
            PKP = "PKP";
        Table.Search();
    });
});


var Control = {
    LoadCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slCompany").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slCompany").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
            }

            $("#slCompany").select2({ placeholder: "Select Company Name", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    LoadOperator: function () {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slOperator").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slOperator").append("<option value='" + item.OperatorId + "'>" + item.Operator + "</option>");
                })
            }

            $("#slOperator").select2({ placeholder: "Select Operator", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    LoadPeriod: function () {
        var date = new Date();
        var today = new Date(date.getFullYear(), date.getMonth(), date.getDate());
        //console.log(today);
        $("#txtPeriod").datepicker({
            endDate: '+0d',
            format: "dd-M-yyyy"
        });
        $("#txtPeriod").datepicker("setDate", today);
    },
    LoadInvoiceType: function () {
        var data = [
            {
                mstInvoiceTypeId: 'All',
                Description: 'ALL'
            },
            {
                mstInvoiceTypeId: 'Tower',
                Description: 'TOWER'
            },
            {
                mstInvoiceTypeId: 'Power',
                Description: 'POWER'
            }
        ]
        $.each(data, function (i, item) {
            $("#slInvoiceType").append("<option value='" + item.mstInvoiceTypeId + "'>" + item.Description + "</option>");
        });
        $("#slInvoiceType").select2({width: null});
    },
        LoadAmountType: function () {
            $.ajax({
                    url: "/api/Dashboard/GetAmountType",
                type: "POST"
        })
            .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null) {
                        $("#slAmountType").append("<option value='" + item.ValueId + "'>" +item.ValueDesc + "</option>");
                }
            });
            }
            $("#slAmountType").select2({
                width: null
            });
});
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
       
        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var columns = [];

        columns = [
            {
                data: "Operator", render: function (val, type, full) {
                    return "<b>" + val + "</b>";
                }
            },
            {
                data: "CountNoReceipt", render: function (val, type, full) {
                    return "<center>" + Helper.RenderLink(val, full,"0") + "</center>";
                }
            },
            {
                data: "AmountNoReceipt", className: "text-right", render: function (val, type, full) {
                    return Helper.ThousandFormatter(val, full);
                }
            },
            {
                data: "CountCurrent", className: "text-right", render: function (val, type, full) {
                    return "<center>" + Helper.RenderLink(val, full,"1") + "</center>";
                }
            },
            {
                data: "AmountCurrent", className: "text-right", render: function (val, type, full) {
                    return Helper.ThousandFormatter(val, full);
                }
            },
            {
                data: "CountAging1_30", className: "text-right", render: function (val, type, full) {
                    return "<center>" + Helper.RenderLink(val, full, "2") + "</center>";
                }
            },
            {
                data: "AmountAging1_30", className: "text-right", render: function (val, type, full) {
                    return Helper.ThousandFormatter(val, full);
                }
            },
            //{
            //    data: "CountAging16_30", className: "text-right", render: function (val, type, full) {
            //        return "<center>" + Helper.RenderLink(val, full,"7") + "</center>";
            //    }
            //},
            //{
            //    data: "AmountAging16_30", className: "text-right", render: function (val, type, full) {
            //        return Helper.ThousandFormatter(val, full);
            //    }
            //},
            {
                data: "CountAging31_60", className: "text-right", render: function (val, type, full) {
                    return "<center>" + Helper.RenderLink(val, full,"3") + "</center>";
                }
            },
            {
                data: "AmountAging31_60", className: "text-right", render: function (val, type, full) {
                    return Helper.ThousandFormatter(val, full);
                }
            },
            {
                data: "CountAging61_90", className: "text-right", render: function (val, type, full) {
                    return "<center>" + Helper.RenderLink(val, full,"4") + "</center>";
                }
            },
            {
                data: "AmountAging61_90", className: "text-right", render: function (val, type, full) {
                    return Helper.ThousandFormatter(val, full);
                }
            },
            {
                data: "CountAgingOver90", className: "text-right", render: function (val, type, full) {
                    return "<center>" + Helper.RenderLink(val, full,"5") + "</center>";
                }
            },
            {
                data: "AmountAgingOver90", className: "text-right", render: function (val, type, full) {
                    return Helper.ThousandFormatter(val, full);
                }
            },
            {
                data: "CountTotal", className: "text-right", render: function (val, type, full) {
                    return "<center>" + Helper.RenderLink(val, full,"All") + "</center>";
                }
            },
            {
                data: "AmountTotal", className: "text-right", render: function (val, type, full) {
                    return Helper.ThousandFormatter(val, full);
                }
            },
            {
                data: "PercentageOverDue", className: "text-right", render: function (val, type, full) {
                    return Helper.PercentageFormatter(val, full);
                }
            },
            {
                data: "InvoiceOverDue", className: "text-right", render: function (val, type, full) {
                    return Helper.ThousandFormatter(val, full);
                }
            }
        ]
        ///
        selector = "#tblSummaryData";
        Table.Draw(selector, columns);
        $(".summary").fadeIn();
        $(".detail").hide();

        $(selector + " tbody").unbind().on("click", "a.btDetail", function (e) {
            
            var params = {
                CompanyId: $(this).attr('companyId'),
                EndDate: $.datepicker.formatDate('yy/mm/dd', new Date($("#txtPeriod").val())),// $(this).attr('enddate'),
                OperatorId: $(this).attr('operatorId'),
                //InvoiceType: $("#slInvoiceType").val(),
                InvoiceType: $('input[name=rbSummary]:checked').val(),
                //AmountType: $(this).attr('amountType'), remark by mtr
                AmountType: $("#slAmountType").val(), //edit by mtr
                Status: $(this).attr('status'),
                vPKP: PKP
            };
            if ($("#slOperator").val() != null && $("#slOperator").val() != "")
                params.OperatorId = $("#slOperator").val();
            //console.log(params.EndDate);

            TableDetail.Search(params);
            $(".summary").hide();
            $(".detail").fadeIn();
            e.preventDefault();
        });

        fsCompanyId = $("#slCompany").val();
        fsEndDate = $.datepicker.formatDate('yy/mm/dd', new Date($("#txtPeriod").val()));
        fsOperatorId =  $("#slOperator").val();
        fsInvoiceType = $('input[name=rbSummary]:checked').val();
        fsAmountType = $("#slAmountType").val();
    },
    Reset: function () {
        $("#slCompany").val($('#slCompany option').eq(0).val()).trigger("change");
        Control.LoadPeriod();
        $("#slOperator").val($('#slOperator option').eq(0).val()).trigger("change");
        //$("#slInvoiceType").val($('#slInvoiceType option').eq(0).val()).trigger("change");           
        $("#rbSearchRent").prop("checked", true);        
        $("#slAmountType").val($('#slAmountType option').eq(0).val()).trigger("change");
        Table.Search();
    },
    Export: function () {
        if (fsCompanyId == null)
            fsCompanyId = $("#slCompany").val();
        if (fsEndDate == null)
            fsEndDate = $.datepicker.formatDate('yy/mm/dd', new Date($("#txtPeriod").val())) //_year + "/" + _month + "/" + _date;
        if (fsOperatorId ==  null)
            fsOperatorId = $("#slOperator").val();
        if (fsInvoiceType == null)
            InvoiceType= $('input[name=rbSummary]:checked').val();
        if (fsAmountType == null)
            fsAmountType = $("#slAmountType").val();

        var params = {
            CompanyId: fsCompanyId,
            EndDate: fsEndDate,
            OperatorId: fsOperatorId,
            InvoiceType: fsInvoiceType,
            AmountType: fsAmountType,
            vPKP: PKP
        };

        window.location.href = "/Dashboard/ARMonitoringAging/Summary/Export?" + $.param(params);
    },
    Draw: function (selector, columns) {

        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var params = {
            CompanyId: $("#slCompany").val(),
            EndDate: $("#txtPeriod").val(),
            OperatorId: $("#slOperator").val(),
            //InvoiceType: $("#slInvoiceType").val(),
            InvoiceType: $('input[name=rbSummary]:checked').val(),
            AmountType: $("#slAmountType").val(),
            vPKP: PKP
        };

        var tblSummaryData = $(selector).DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
                       
            "ajax": {
                "url": "/api/Dashboard/ARMonitoringAging/Summary",
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
            "lengthMenu": [[-1], ['All']],
            "destroy": true,
            "columns": columns,
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fixedColumns": {
                leftColumns: 1
            },
            
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                }
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            "fnRowCallback": function( nRow, aData, iDisplayIndex, iDisplayIndexFull ) {
                if ( aData.Operator == "TOTAL" )
                {
                    $('td', nRow).css('background-color', '#e8e8e8');
                    //$('td', nRow).css('border', '3px black'); //border: "3px red solid"
                    $('td', nRow).css('font-size', '200%');
                    $('td', nRow).css('font-weight', 'bold');
                    $('tr', nRow).css('color', 'black');
                    $('center', nRow).css('color', 'black');
                    //$('td', nRow).css('color', 'white');
                }               
            },
            'ordering': false,
            'order': []
        });
    }
}

var TableDetail = {
    Search: function (params) {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        $.ajax({
            url: "/api/Dashboard/ARMonitoringAging/Detail",
            type: "POST",
            datatype: "json",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            var tblSummaryData = $("#tblDetailOperator").DataTable({
                "proccessing": false,
                "serverSide": false,
                "sScrollX": "100%",
                "bScrollCollapse": true,
                "colReorder": true,
                "language": {
                    "emptyTable": "No data available in table",
                },
                "data": data.data,
                buttons: [
                    { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                    {
                        text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                            var l = Ladda.create(document.querySelector(".yellow"));
                            l.start();
                            TableDetail.Export(params);
                            l.stop();
                        }
                    },
                    { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
                ],
                "fixedColumns": {
                    leftColumns: 2 /* Set the 2 most left columns as fixed columns */
                },
                "filter": false,
                "lengthMenu": [[10, 25, 50, -1], ['10', '25', '50', 'All']],
                "destroy": true,
                "columns": [
                    { data: "Company" },
                    { data: "Operator" },
                    { data: "InvoiceNo" },
                    { data: "InvoiceDate" },
                    { data: "ReceiptDate" },
                    { data: "DueDate" },
                    { data: "InvoiceType" },
                    {
                        data: "OverDue", className: "text-right", render: function (val, type, full) {
                            return Common.Format.CommaSeparationOnly(val);
                        }
                    },
                    {
                        data: "DPP", className: "text-right", render: function (val, type, full) {
                            return Common.Format.CommaSeparationOnly(val);
                        }
                    },
                    {
                        data: "VAT", className: "text-right", render: function (val, type, full) {
                            return Common.Format.CommaSeparationOnly(val);
                        }
                    },
                    { data: "WAPU" },
                    { data: "SKB" },
                    {
                        data: "Penalty", className: "text-right", render: function (val, type, full) {
                            return Common.Format.CommaSeparationOnly(val);
                        }
                    },
                    {
                        data: "OutstandingInvoiceGross", className: "text-right", render: function (val, type, full) {
                            return Common.Format.CommaSeparationOnly(val);
                        }
                    },
                    { data: "PICInternal" },
                    { data: "PICExternal" },
                    { data: "Subject", width: '250px !important' }
                ],
                "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                "fnDrawCallback": function () {
                    l.stop();
                },
                'aoColumnDefs': [{
                    'bSortable': false,
                    'aTargets': []
                }],
                'order': []
            });
        })
        .fail(function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            //console.log(err.Message);
        });
    },
    Export: function (params) {
        window.location.href = "/Dashboard/ARMonitoringAging/Detail/Export?" + $.param(params);
    }
}

var Helper = {
    RenderLink: function (val, full, status) {
        var table = $('#tblSummaryData').DataTable();

        if (full.Operator == "TOTAL" && table.data().count() == 1) {
            return val;
        } else {
            var d = new Date($("#txtPeriod").val());
            var _date = d.getDate();
            var _month = d.getMonth() + 1;
            var _year = d.getFullYear();

            var CompanyId = $("#slCompany").val();
            var EndDate = _year + "/" + _month + "/" + _date;
            var InvoiceType = $('input[name=rbSummary]:checked').val();                        
            var AmountType = $("#slAmountType").val();
            var OperatorId = full.Operator;

            if (OperatorId == "TOTAL")
                OperatorId = "All";

            return "<a class='btDetail' companyId='" + CompanyId + "' endDate='" + EndDate + "' operatorId='" + OperatorId + "' invoiceType='" + InvoiceType + "' amountType='" + AmountType + "' status='" + status + "'>" + val + "</a>";
        }
    },
    ThousandFormatter: function (value) {
            if (value == null)
                return "0";

            var divider = 1000000.00;
            var parts = (parseFloat(value).toFixed(2) / divider).toString().split(".");
            parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");

            if (parseFloat(value).toFixed(2) % 1 == 0)
                return parts[0];

            return parts.join(".");
    },
    PercentageFormatter: function (value) {
        return value.toFixed(2) + '%';
    }

}