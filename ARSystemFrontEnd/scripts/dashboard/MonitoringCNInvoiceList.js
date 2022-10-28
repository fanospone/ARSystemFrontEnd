var PKP = "PKP";
$(function () {
    List.Init();
    
    $(".detail").hide();
});


var List = {
    Init: function () {
        _tblL.MonitoringCN.Init();
        //_tblL.MonitoringCNDetail.Init();
        List.Control.Events();
        _tblL.MonitoringCN.LoadCompany();
        
        _tblL.MonitoringCN.LoadCustomer();
    },
    Control: {
        ID: {
            frmFilter: "#frmFilter",
            fCustomer: "#fCustomer",
            fCompany: "#fCompany",
            //fStatus: "#fStatus",
            //fPeriode: "#fPeriode",
            btnSearch: "#btnSearch",
            btnReset: "#btnRst",

            tblList: "#tblvwMonitoringCNInvoice",
            tblDetail: "#tblvwMonitoringCNInvoiceDetail",

        },
        Class: {
            inputSearch: ".inputSearch",
            btnSearch: ".btnSearch"
        },
        Events: function () {

            $(_classL.inputSearch).keypress(function (e) {
                var keycode = (e.keyCode ? e.keyCode : e.which);
                if (keycode == '13') {
                    $(_idL.btnSearch).trigger('click');
                }
            });

            $(_idL.btnSearch + ', ' + _classL.btnSearch).on('click', function () {
                _tblL.MonitoringCN.Search();
            });

            $(_idL.btnReset).on('click', function () {
                _tblL.MonitoringCN.Reset();
            });
            
            $('#fExcludePKP').on("switchChange.bootstrapSwitch", function (event, state) {
                if ($("#fExcludePKP").bootstrapSwitch("state") == false) {
                    PKP = "";
                    _tblL.MonitoringCN.LoadCompany();
                }
                else {
                    PKP = "PKP";
                    $("#fCompany option[value='PKP']").remove();
                }
                //alert(PKP);
                _tblL.MonitoringCN.Search();
            });


            $(_idL.tblList + " tbody").on("click", ".btDetail", function (e) {
                
                $("#filterPnlMonitoringCNInvoice").hide();
                //var cid = '';
                var params = {
                    CustomerID: $(this).attr('CustomerID'),
                    //CompanyID: $(this).attr('CompanyID'),
                    CompanyID: $('#fCompany').val(),
                    vPKP: PKP,
                    Range: $(this).attr('range')
                };
                //alert(params.CustomerID + " " + params.CompanyID);
                _tblL.MonitoringCNDetail.Search(params);
                $(".summary").hide();
                $(".detail").fadeIn();
                e.preventDefault();
            });
            

            $("#btBackSummary").on('click', function () {
                $("#filterPnlMonitoringCNInvoice").fadeIn();
                $(".summary").fadeIn();
                $(".detail").hide();
            });
        }
    },
    Table: {
        MonitoringCN: {
            Init: function () {
                var tblSummaryData = $(_idL.tblList).DataTable({
                    "filter": false,
                    "destroy": true,
                    "data": []
                });

                _tblL.MonitoringCN.Search();
                _tblL.MonitoringCN.Reset();
                
            },
            Search: function () {
                var l = Ladda.create(document.querySelector(_idL.btnSearch));
                l.start();

                _tempL.SetDataFilter();

                var tblSummaryData = $(_idL.tblList).dataTable({
                    proccessing: true,
                    serverSide: true,
                    paging: true,
                    filter: false,
                    lengthMenu: [[10, 25, 50], ['10', '25', '50']],
                    destroy: true,
                    language: {
                        "emptyTable": "No data available in table",
                    },
                    ajax: {
                        "url": _urlL.GetMonitoringCNInvoiceList,
                        "type": "POST",
                        "datatype": "json",
                        "data": _tempL.DataFilter
                    },
                    buttons: [
                        { extend: 'copy', className: 'btn red btn-outline summary', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy summary" title="Copy"></i>', titleAttr: 'Copy' },
                        {
                            text: '<i class="fa fa-file-excel-o summary" data-style="zoom-in"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline summary', action: function (e, dt, node, config) {
                                var l = Ladda.create(document.querySelector(".yellow"));
                                l.start();
                                List.Table.MonitoringCN.Export()
                                l.stop();
                            }
                        }
                        , { extend: 'colvis', className: 'btn dark btn-outline summary', text: '<i class="fa fa-columns summary"></i>', titleAttr: 'Show / Hide Columns' }
                    ],
                    "columns": [
                        { data: 'CustomerName', className: "dt-center", orderable: false },
                        {
                            data: "OD_13", className: "dt-center", orderable: false, render: function (val, type, full) {
                                if (val == "0") {
                                    return val;
                                } else return Helper.RenderLink(val, full, "13");
                                
                            }
                        },
                        {
                            data: "OD_46", className: "dt-center", orderable: false, render: function (val, type, full) {
                                if (val == "0") {
                                    return val;
                                } else return Helper.RenderLink(val, full, "46");
                            }
                        },
                        {
                            data: "OD_79", className: "dt-center", orderable: false, render: function (val, type, full) {
                                if (val == "0") {
                                    return val;
                                } else return Helper.RenderLink(val, full, "79");
                            }
                        },
                        {
                            data: "OD_9s", className: "dt-center", orderable: false, render: function (val, type, full) {
                                if (val == "0") {
                                    return val;
                                } else return Helper.RenderLink(val, full, "9s");
                            }
                        },
                        {
                            data: "GrandTotal", className: "dt-center", orderable: false, render: function (val, type, full) {
                                if (val == "0") {
                                    return val;
                                } else return Helper.RenderLink(val, full, "total");
                            }
                        },
                    ],
                    "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                    "fnPreDrawCallback": function () {
                        App.blockUI({ target: _idL.tblList, animate: !0 });
                    },
                    "fnDrawCallback": function () {
                        if (Common.CheckError.List(tblSummaryData.data())) {
                        }
                        l.stop();
                        App.unblockUI(_idL.tblList);
                    },
                    "order": []
                });
                
            },
            Reset: function () {
                $("#fCompany").val($('#fCompany option').eq(0).val()).trigger("change");
                $("#fCustomer").val($('#fCustomer option').eq(0).val()).trigger("change");
                $("#fExcludePKP").bootstrapSwitch('state', true);
                _tblL.MonitoringCN.Search();
            },
            Export: function () {
                _tempL.SetDataFilter();
                var f = _tempL.DataFilter;
                window.location.href = "/Dashboard/List/ExportMonitoringCN?CustomerID=" + f.CustomerID + "&CompanyID=" + f.CompanyID + "&vPKP=" + f.vPKP
            },
            LoadCustomer: function () {
                $.ajax({
                    url: "/api/Dashboard/Operator",
                    type: "GET"
                })
                .done(function (data, textStatus, jqXHR) {
                    $("#fCustomer").html("<option></option>")

                    if (Common.CheckError.List(data)) {
                        $.each(data, function (i, item) {
                            $("#fCustomer").append("<option value='" + item.OperatorId + "'>" + item.Operator + "</option>");
                        })
                    }

                    $("#fCustomer").select2({ placeholder: "Select Customer Name", width: null });
                    if (PKP == "PKP") {
                        $("#fCompany option[value='PKP']").remove();
                    }
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    Common.Alert.Error(errorThrown);
                });
            },
            LoadCompany: function () {
                $.ajax({
                    url: "/api/MstDataSource/Company",
                    type: "GET"
                })
                    .done(function (data, textStatus, jqXHR) {
                        $("#fCompany").html("<option></option>")

                        if (Common.CheckError.List(data)) {
                            $.each(data, function (i, item) {
                                $("#fCompany").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                            })
                        }

                        $("#fCompany").select2({ placeholder: "Select Company Name", width: null });
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        Common.Alert.Error(errorThrown);
                    });
            }
        },
        MonitoringCNDetail: {
            Init: function () {
                var tblDetailData = $(_idL.tblDetail).DataTable({
                    "filter": false,
                    "destroy": true,
                    "data": []
                });

                _tblL.MonitoringCNDetail.Search();
                //_tblL.MonitoringCNDetail.Reset();
            },
            Search: function (params) {
                var l = Ladda.create(document.querySelector(_idL.btnSearch));
                l.start();

                _tempD.SetDataFilter(params);

                var tblDetailData = $(_idL.tblDetail).dataTable({
                    proccessing: true,
                    serverSide: true,
                    paging: true,
                    filter: false,
                    lengthMenu: [[10, 25, 50], ['10', '25', '50']],
                    destroy: true,
                    language: {
                        "emptyTable": "No data available in table",
                    },
                    ajax: {
                        "url": _urlL.GetMonitoringCNInvoiceListDetail,
                        "type": "POST",
                        "datatype": "json",
                        "data": _tempD.DataFilter
                    },
                    buttons: [
                        { extend: 'copy', className: 'btn red btn-outline detail', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy detail" title="Copy"></i>', titleAttr: 'Copy' },
                        {
                            text: '<i class="fa fa-file-excel-o detail" data-style="zoom-in"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline detail', action: function (e, dt, node, config) {
                                var l = Ladda.create(document.querySelector(".yellow"));
                                l.start();
                                List.Table.MonitoringCNDetail.Export()
                                l.stop();
                            }
                        }
                        , { extend: 'colvis', className: 'btn dark btn-outline detail', text: '<i class="fa fa-columns detail"></i>', titleAttr: 'Show / Hide Columns' }
                    ],
                    "columns": [
                        { data: 'InvNumber', className: "dt-center", orderable: false },
                        { data: 'NoFaktur', className: "dt-center", orderable: false },
                        { data: 'CompanyInvoice', className: "dt-center", orderable: false },
                        { data: 'CustomerID', className: "dt-center", orderable: false },
                        { data: 'Subject', className: "dt-center", orderable: false },
                        //{ data: 'AmountInvoice', className: "dt-center", orderable: false }
                        {
                            data: "AmountInvoice", className: "text-right", orderable: false, render: function (val, type, full) {
                                return Helper.ThousandFormatter(val);
                            }
                        }
                    ],
                    "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                    "fnPreDrawCallback": function () {
                        App.blockUI({ target: _idL.tblDetail, animate: !0 });
                    },
                    "fnDrawCallback": function () {
                        if (Common.CheckError.List(tblDetailData.data())) {
                        }
                        l.stop();
                        App.unblockUI(_idL.tblDetail);
                    },
                    "order": []
                });
                
            },
            //Reset: function () {
            //    $("#fCompany").val($('#fCompany option').eq(0).val()).trigger("change");
            //    $("#fCustomer").val($('#fCustomer option').eq(0).val()).trigger("change");
            //    $("#fExcludePKP").bootstrapSwitch('state', true);
            //    _tblL.MonitoringCN.Search();
            //},
            Export: function () {
                //_tempL.SetDataFilter();
                var f = _tempD.DataFilter;
                window.location.href = "/Dashboard/List/ExportMonitoringCNDetail?CustomerID=" + f.CustomerID + "&CompanyID=" + f.CompanyID + "&vPKP=" + f.vPKP + "&Range=" + f.Range;
            },
        }
    },
    Url: {
        APIs: {
            GetMonitoringCNInvoiceList: "/api/Dashboard/MonitoringCNInvoiceList",
            GetMonitoringCNInvoiceListDetail: "/api/Dashboard/MonitoringCNInvoiceListDetail"
        }
    },
    TempData: {
        DataFilter: {
            CustomerID: null,
            CompanyID: null,
            vPKP: null,
            //Status: null,
            //Periode: null
            
        },
        SetDataFilter: function () {
            _tempL.DataFilter.CustomerID = $('#fCustomer').val(),
            _tempL.DataFilter.CompanyID = $('#fCompany').val(),
            _tempL.DataFilter.vPKP = PKP
            //_tempL.DataFilter.Status = $('#fStatus').val(),
            //_tempL.DataFilter.Periode = $('#fPeriode').val(),
        },
        CheckedData: []
    },
    TempDetailData: {
        DataFilter: {
            CustomerID: null,
            CompanyID: null,
            vPKP: null,
            Range: null
            //Status: null,
            //Periode: null

        },
        SetDataFilter: function (param) {

            _tempD.DataFilter.CustomerID = param.CustomerID,
            _tempD.DataFilter.CompanyID = param.CompanyID,
            _tempD.DataFilter.vPKP = PKP,
            _tempD.DataFilter.Range = param.Range
            //_tempL.DataFilter.Status = $('#fStatus').val(),
            //_tempL.DataFilter.Periode = $('#fPeriode').val(),
        },
        CheckedData: []
    }
}

var Helper = {
    RenderLink: function (val, full, range) {
        var CustomerID = full.CustomerID;
        var CompanyID = full.CompanyID;
        //alert(CustomerID + " " + CompanyID);
        return "<a class='btDetail' CustomerID='" + CustomerID + "' CompanyID='" + CompanyID + "' vPKP='" + PKP + "' range='" + range + "'>" + val + "</a>";
        //if (full.Operator == "TOTAL" && table.data().count() == 1) {
        //    return val;
        //} else {
        //    var CustomerID = $("#fCustomer").val();
        //    var CompanyID = $("#fCompany").val();
        //    var PKP = PKP;
        //    var OperatorId = full.Operator;

        //    if (OperatorId == "TOTAL")
        //        OperatorId = "All";

        //    return "<a class='btDetail' CustomerID='" + CustomerID + "' CompanyID='" + CompanyID + "' vPKP='" + PKP;
        //}
    },
    ThousandFormatter: function (nStr) {
        nStr += '';
        var x = nStr.split('.');
        var x1 = x[0];
        var x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return x1 + x2;
    }

}
var _urlL = List.Url.APIs, _idL = List.Control.ID, _classL = List.Control.Class, _tempL = List.TempData, _tempD = List.TempDetailData, _tblL = List.Table;