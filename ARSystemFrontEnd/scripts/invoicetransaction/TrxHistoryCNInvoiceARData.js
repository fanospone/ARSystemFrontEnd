Data = {};

//filter search 
var fsStartPeriod = "";
var fsEndPeriod = "";
var fsCompanyId = "";
var fsOperator = "";
var fsCNStatus = "";
var fsInvoiceStatus = "";
var fsInvNo = "";
var fsInvoiceTypeId = "";
var fsType = "";


jQuery(document).ready(function () {
    
    Form.Init();
    Table.Search();
    Control.ReplacePrompt();
    $("#needReplace").bootstrapSwitch('state', true);
    //panel Summary
    $('#needReplace').on("switchChange.bootstrapSwitch", function (event, state) {
        var needReplace = $("#needReplace").bootstrapSwitch("state");
        if ($("#needReplace").bootstrapSwitch("state") == false) {
            $("#addSection").hide();
        }
        else {
            $("#addSection").fadeIn();
        }
    });

    $("#formSearch").submit(function (e) {
        Table.Search();
        e.preventDefault();
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
    });
    
    $("#btnEditSave").off('click').on('click', function () {
        if ($("#formTransactionEdit").parsley().validate()) {
            var cnInvoice = $("#cnInvNo").val();
            var invoiceReplacement = [];
            
            var needReplace = $("#needReplace").bootstrapSwitch("state");
            if (needReplace == true) {
                $('.rpInvNo').each(function (i, rpInvNo) {
                    var inv = $(this).val();
                    var fak = $(this).closest("tr").find('td:eq(1) > input').val();
                    invoiceReplacement.push({ CNInvoiceNo: cnInvoice, InvoiceNo: inv, FakturNo: fak, isReplaceable: needReplace });
                });
            } else {
                $(".appendedRow").remove();
                invoiceReplacement.push({ CNInvoiceNo: cnInvoice, InvoiceNo:null, FakturNo:null, isReplaceable: needReplace });
            }

            $("#modalEditPeriode").hide();
            var l = Ladda.create(document.querySelector("#btnEditSave"))
            //validation
            $.ajax({
                url: "/api/HistoryCNInvoiceARData/ValidateReplacement",
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(invoiceReplacement),
                cache: false,
                beforeSend: function (xhr) {
                    l.start();
                }
            }).done(function (data, textStatus, jqXHR) {
            
                if (!data) {
                    $.ajax({
                        url: "/api/HistoryCNInvoiceARData/ManageReplacement",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json",
                        data: JSON.stringify(invoiceReplacement),
                        cache: false,
                        beforeSend: function (xhr) {
                            l.start();
                        }
                    }).done(function (data, textStatus, jqXHR) {
                        if (textStatus == 'success') {
                            $("#btnEditClose").trigger('click');
                            Common.Alert.Success("Invoice Replacement has been updated.")
                            $('.rpFakturNo').each(function () {
                                $(this).val('');
                            });
                            Table.Search();
                        } else {
                            Common.Alert.Warning(data.ErrorMessage);
                        }
                        l.stop();
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        Common.Alert.Error(errorThrown);
                        l.stop();
                    })
                }
                else {
                    $("#btnEditClose").trigger('click');
                    Common.Alert.Warning("Duplicate or Empty Replacement.");
                }
                l.stop();
            }).fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
                l.stop();
            })

            
        }
    });
    
});

var Form = {
    Init: function () {
        Control.BindingSelectCompany();
        PKPPurpose.Set.UserCompanyCode();
        Control.BindingSelectOperator();
        $("#slSearchCNStatus").select2({ placeholder: "Select CN Status", width: null });
        $("#slSearchInvoiceStatus").select2({ placeholder: "Select Invoice Status", width: null });
        Control.BindingSelectInvoiceType();
        

        Table.Reset();
        // Initialize Datepicker
        $("body").delegate(".date-picker", "focusin", function () {
            $(this).datepicker({
                format: "dd-M-yyyy"
            });
        });
        Table.Init();
        $(".panelSearchResult").hide();

    },
    Done: function () {
        $("#pnlSummary").fadeIn();
    }
}

var Table = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        fsStartPeriod = $("#tbStartPeriod").val();
        fsEndPeriod = $("#tbEndPeriod").val();
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsCNStatus = $("#slSearchCNStatus").val() == null ? "" : $("#slSearchCNStatus").val();
        fsInvoiceStatus = $("#slSearchInvoiceStatus").val() == null ? "" : $("#slSearchInvoiceStatus").val();
        fsInvNo = $("#tbInvNo").val();
        fsInvoiceTypeId = $("#slSearchTermInvoice").val() == null ? "" : $("#slSearchTermInvoice").val();
        fsType = $("input[name=rdProccessType]:checked").val();
        var params = {
            strStartPeriod: fsStartPeriod,
            strEndPeriod: fsEndPeriod,
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            InvNo: fsInvNo,
            CNStatus: fsCNStatus,
            ReplacementStatus: fsInvoiceStatus,
            InvoiceTypeId: fsInvoiceTypeId,
            ProccessType :fsType
        };

        var tblSummaryData = $("#tblSummaryData").DataTable({
            order: [[1, 'asc']],
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/HistoryCNInvoiceARData/grid",
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
                        Table.Export();
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
                    className: "dt-center",
                    mRender: function (data, type, full) {
                        var strReturn = "<button type='button' title='Edit' class='btn btn-xs green-jungle btEdit'><i class='fa fa-pencil'></i></button>"
                        return strReturn;
                    }
                },
                { data: "InvNo" },
                { data: "InvParentNo" },                
                { data: "InvTemp" },
                { data: "InvSumADPP", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InvTotalAPPN", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                {
                    data: "InvPrintDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "RequestedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "RequestedBy" },
                {
                    data: "ApprovedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "ApprovedBy" },
                { data: "Description" },
                { data: "InvCompanyId" },
                { data: "InvOperatorID" },
                { data: "CNInfo" },
                { data: "CNFrom" },
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += full.PicaTypeRequestor + " - " + full.PicaDetailRequestor
                        return strReturn;
                    }
                },
                { data: "Remark" },
                { data: "ReplacementStatus" },
                {
                    data: 'ReplaceDate', orderable: false, className: "dt-center", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "ReplaceInvoice" }
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
                l.stop(); App.unblockUI('.panelSearchResult');
            },
            "orderBy": [],
            "fixedColumns": {
                "leftColumns": 1
            },
            "scrollX": true,
            "scrollCollapse": true
        });
        
        var tblSummaryData = $("#tblSummaryData").DataTable();
        if (fsType == 2) {
            tblSummaryData.columns([0,18,19,20]).visible(true);
        }
        if (fsType == 1) {
            tblSummaryData.columns([0,18,19,20]).visible(false);
        }
    },
    Reset: function () {
        fsStartPeriod = "";
        fsEndPeriod = "";
        fsCompanyId = "";
        fsOperator = "";
        fsCNStatus = "";
        fsInvoiceStatus = "";
        fsInvNo = "";
        fsInvoiceTypeId = "";
        fsType = "";

        $("#tbStartPeriod").val("");
        $("#tbEndPeriod").val("");
        $("#slSearchCompanyName").val("").trigger('change');
        $("#slSearchOperator").val("").trigger('change');
        $("#slSearchCNStatus").val("").trigger('change');
        $("#slSearchInvoiceStatus").val("").trigger('change');
        $("#slSearchTermInvoice").val("").trigger('change');
        $("#tbInvNo").val("");

        if (PKPPurpose.Temp.UserCompanyCode == Constants.CompanyCode.PKP) {
            PKPPurpose.Filter.PKPOnly();
        }
    },
    Export: function () {
        window.location.href = "/InvoiceTransaction/TrxHistoryCNInvoiceARDataReport/Export?strStartPeriod=" + fsStartPeriod + "&strEndPeriod=" + fsEndPeriod
           + "&strCompanyId=" + fsCompanyId + "&strOperator=" + fsOperator
           + "&strCNStatus=" + fsCNStatus
           + "&strRpStatus=" + fsInvoiceStatus
           + "&InvNo=" + fsInvNo + "&InvoiceTypeId=" + fsInvoiceTypeId + "&ProccessType=" + fsType;
    }
}
var rowNumber = 1;
var Control = {
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchCompanyName").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchCompanyName").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                });
            }

            $("#slSearchCompanyName").select2({ placeholder: "Select Company Name", width: null });

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
                });
            }

            $("#slSearchOperator").select2({ placeholder: "Select Operator", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectInvoiceCategory: function () {
        $.ajax({
            url: "/api/MstDataSource/InvoiceCategory",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchInvoiceCategory").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchInvoiceCategory").append("<option value='" + item.mstInvoiceCategoryId + "'>" + item.Description + "</option>");
                });
            }

            $("#slSearchInvoiceCategory").select2({ placeholder: "Select Invoice Category", width: null });

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
            $("#slSearchTermInvoice").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchTermInvoice").append("<option value='" + item.mstInvoiceTypeId + "'>" + item.Description + "</option>");
                })
            }

            $("#slSearchTermInvoice").select2({ placeholder: "Select Term Invoice", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectInvoiceHeader: function (operatorId, rpInvoice, rpFaktur, rowNumber) {
        $.ajax({
            url: "/api/HistoryCNInvoiceARData/InvHeader/"+operatorId,
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#rpInvNo" + rowNumber).html("<option></option>");
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#rpInvNo" + rowNumber).append("<option value='" + item.InvNo + "' faktur='" + item.TaxNo + "'>" + item.InvNo + "</option>");
                    //debugger;
                })
            }

            $("#rpInvNo" + rowNumber).select2({
                placeholder: "Select Replacement Invoice", width: null, dropdownParent: $('#formTransactionEdit')
            });
            if (rpInvoice != '') {
                $("#rpInvNo" + rowNumber).val(rpInvoice).trigger("change");
                $("#rpFakturNo" + rowNumber).val(rpFaktur);
            }

            $("#rpInvNo" + rowNumber).on("change", function () {
                var fakNo = $('option:selected', this).attr('faktur');
                $("#rpFakturNo" + rowNumber).val(fakNo);
            });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    GenerateRow: function (operatorId, rpInvoice, rpFaktur) {
            var rows = "<tr class='appendedRow'>"
                        + "<td><div ><select id='rpInvNo" + rowNumber + "' name='rpInvNo" + rowNumber + "' class='rpInvNo form-control select2'></select> </div></td>"
                        + "<td><input type='text' id='rpFakturNo" + rowNumber + "' name='rpFakturNo" + rowNumber + "' class='rpFakturNo form-control' disabled /></td>"
                        + "<td><a type='button' id='btnDeleteComponent" + rowNumber + "' class='btnDeleteComp m-portlet__nav-link btn m-btn m-btn--hover-danger m-btn--icon m-btn--icon-only m-btn--pill' title='Remove Row'><i title='Remove Row' class='fa fa-minus-circle'></i></a></td>"
                    + "</tr>";
            $("#tableAddComponent > tbody").append(rows);

            var rpInvNoID = "rpInvNo" + rowNumber;
            
        //Bind Dropdown Inv Replacement
            if (rpInvoice != '') {
                Control.BindingSelectInvoiceHeader(operatorId, rpInvoice, rpFaktur, rowNumber);
            } else {
                Control.BindingSelectInvoiceHeader(operatorId, '', '', rowNumber);
            }

            rowNumber = rowNumber + 1;

            $(".btnDeleteComp").off('click').on('click', function(e) {
                //debugger;
                $(this).closest("tr").remove();
            })
    },
    ReplacePrompt: function () {
        
        $("#tblSummaryData" + " tbody").off('click').on("click", ".btEdit", function (e) {
            rowNumber = 1;
            var tblSummaryData = $("#tblSummaryData").DataTable();
            var tr = $(this).closest('tr');
            var row = tblSummaryData.row(tr);
            var dt = row.data();

            $("#modalEditPeriode").show();
            $("#cnInvNo").val(dt.InvNo);
            $("#cnFakNo").val(dt.TaxNumber);
            

            $("#btnAppendTableRows").off('click').on('click', function (e, opId, invNo, fakNo) {
                e.preventDefault();
                if (invNo == '') {
                    Control.GenerateRow(dt.InvOperatorID, '', '');
                } else {
                    Control.GenerateRow(dt.InvOperatorID, invNo, fakNo);
                }
            });

            var CNInvoiceEncoded = dt.InvNo;
            var CNInvoiceNo = CNInvoiceEncoded.replace(/\//g, '-');
            $.ajax({
                url: "/api/HistoryCNInvoiceARData/MappedReplacement/" + CNInvoiceNo,
                type: "GET",
                dataType: "json"
            })
            .done(function (data, textStatus, jqXHR) {
                if (data.length == 0) {

                } else {
                    //first flag
                    var firstFlag = data[0].isReplaceable;
                    $("#needReplace").bootstrapSwitch('state', firstFlag);
                    $.each(data, function (key, value) {
                        if (value.isReplaceable == true) {
                            var existRplcID = value.InvoiceReplacementID;
                            var existInvNo = value.InvoiceNo;
                            var existFakturNo = value.FakturNo;
                            $('#btnAppendTableRows').trigger('click', [dt.InvOperatorID, existInvNo, existFakturNo]);
                        }
                    });
                }
                

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
            
            
            $("#btnEditClose").on('click', function () {
                $(".appendedRow").remove();
                $("#needReplace").bootstrapSwitch('state', true);
                rowNumber = 1;
                $(".rpFakturNo").val('');
                $("#modalEditPeriode").hide();
            });
        });
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
            $('#slSearchCompanyName').val(Constants.CompanyCode.PKP).trigger('change');
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