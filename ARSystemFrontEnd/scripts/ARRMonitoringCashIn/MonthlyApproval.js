
var _Year = new Date().getFullYear().toString();
var _Month = new Date().getMonth().toString();

var _Params = {};
var Data = {};


jQuery(document).ready(function () {
    Form.Init();
    //$('.pnlDetail').fadeIn(1000);
    //Table.Summary();
    $(document).keypress(function (e) {
        if (e.which == 13) {
            Table.Summary();
        }
    });

    $("#btSearch").unbind().click(function () {
        $("#btSearch").unbind().click(function () {
            if ($("#slSearchYear").val() != '' && $("#slSearchYear").val() != null && $("#slSearchMonth").val() != '' && $("#slSearchMonth").val() != null) {

                $('.pnlDetail').fadeIn(1000);
                Table.Summary();

            }
            else {
                Common.Alert.Warning("Please Select Periode And Month !");
                return false;
            }


        });

    });
    $("#btSearchDetail").unbind().click(function () {
        $('.panelSearchResultWeek').fadeOut();
        $('.pnlDetail').fadeOut();
        $('.panelSearchResultMonth').fadeOut();
        $('.panelSearchChart').fadeOut();
        $('#pnlDetail').fadeIn(1000);

        Table.Details(_ParamExport.Type, _ParamExport.RFIDateMonth, _ParamExport.RFIDateWeek, _ParamExport.CustomerID, _ParamExport.Step);

    });
    $("#btResetDetail").unbind().click(function () {
        $("#slSearchDetailSonum").val('');
        $("#slSearchDetailSiteID").val('');
        $("#slSearchDetailSiteName").val('');
        Table.Details(_ParamExport.Type, _ParamExport.RFIDateMonth, _ParamExport.RFIDateWeek, _ParamExport.CustomerID, _ParamExport.Step);
    });

});


var Form = {
    Init: function () {
        //$('.panelSearchResult').hide();
        //$('.panelSearchResultMonth').hide();
        Control.BindingSelectYear($('#slSearchYear'));
        Control.BindingSelectMonth($('#slSearchMonth'));

    },
    Reset: function () {
        $("#slSearchYear").val(null).trigger("change");
        $("#slSearchMonth").val(null).trigger("change");
    },
    EditRemark: function () {

        $.ajax({
            url: "/api/CashInRemarkMonthly/EditRemark",
            type: "POST",
            data: {
                Periode: $("#slSearchYear").val(),
                Month: $("#slSearchMonth").val(),
                OperatorID: Data.Selected.OperatorID,
                Remarks: $("#MdTxtRemark").val()
            },
            beforeSend: function (xhr) {
                App.blockUI({ target: "#panelDetail", animate: !0 });
            }
        }).done(function (data, textStatus, jqXHR) {
            $('#MdAddRemark').modal('hide');
            //App.unblockUI("#panelDetail");
            //Form.Back();
            Table.Summary();
            Common.Alert.Success("Success to Saved data !");
        })
            .fail(function (jqXHR, textStatus, errorThrown) {
                App.unblockUI("#panelDetail");
                Common.Alert.Error(errorThrown)
                return;
            })
    },
    Submit: function () {
        var table = $("#tblSummaryMonthlys").DataTable();
        if (table.data().count() > 0) {
            $.ajax({
                url: "/api/CashInRemarkMonthly/submit",
                type: "GET",
                data: {
                    Type: "MONTHLY",
                    Year: $("#slSearchYear").val(),
                    Month: $("#slSearchMonth").val()
                },
            })
                .done(function (data, textstatus, jqxhr) {

                    if (Common.CheckError.List(data)) {
                        var IsError = 0;
                        var message = '';
                        $.each(data, function (i, item) {
                            IsError = item.IsError;
                            message = item.message;
                        })
                    }
                    if (IsError == 1) {
                        Common.Alert.Error(message);
                    }
                    else {
                        Common.Alert.Success("Success to Submit data !");
                        Table.Summary();
                    }

                })
                .fail(function (jqxhr, textstatus, errorThrown) {
                    Common.Alert.Error(errorThrown);
                });
            //$("#btSubmit").prop('disabled', false);
            //$(':input[type="submit"]').prop('disabled', false);
        }
        else {
            Common.Alert.Warning("Data Is Nothing!");
            //$("#btSubmit").prop('disabled', true);
        }



    }
}

var Table = {
    Summary: function () {

        //App.blockUI({ target: "#Main", animate: !0 });
        //App.blockUI({ target: ".portlet-body", animate: !0 });
        var params = {
            Periode: $("#slSearchYear").val(),
            Month: $("#slSearchMonth").val()
        }
        $('#thTitleOfWeek').html($("#slSearchMonth option:selected").text() + ' ' + $("#slSearchYear").val());
        var tblSummary = $("#tblSummaryMonthlys").DataTable({
            "orderCellsTop": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/CashInRemarkMonthly/gridapproval",
                "type": "POST",
                "datatype": "json",
                "data": params
            },
            buttons: [
                { text: 'Excel', className: 'btn green btn-outline', action: function (e, dt, node, config) { Table.ExportSummary('tblSummaryMonthlys', 'Monthly - ' + $("#slSearchMonth").val() + ' ', 'ARR Monitoring Cash In (Monthly - ' + $("#slSearchMonth").val() + ')'); } },
                { extend: 'colvis', className: 'btn dark btn-outline', text: 'Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 50, 100, 250, 500], ['10', '50', '100', '250', '500']],
            "destroy": true,
            "columns": [
                { data: "OperatorID" },
                {
                    'mRender': function (data, type, row, meta) {
                        var id = (row.TotalForecast != null) ? meta.settings.fnFormatNumber(row.TotalForecast) : null;
                        return id;
                    }
                },
                {
                    'mRender': function (data, type, row, meta) {
                        var id = (row.TotalActual != null) ? meta.settings.fnFormatNumber(row.TotalActual) : null;
                        return id;
                    }
                },
                {
                    'mRender': function (data, type, row, meta) {
                        var id = (row.TotalVariance != null) ? meta.settings.fnFormatNumber(row.TotalVariance) : null;
                        return id;
                    }
                },
                {
                    'mRender': function (data, type, row, meta) {
                        var id = (row.PercentageVariance != null) ? meta.settings.fnFormatNumber(row.PercentageVariance) : 0;
                        return id + " %";
                    }
                },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var val = full.Remarks;
                        var col = "";

                        if (val == "null" || val == null) {
                            val = "";
                            col = "1";
                        }
                        else {
                            val = val.substring(0, 30) + "...";
                            col = "10";
                        }
                        return val;

                    }
                }
                , {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "<div class='row'>";
                        {
                            strReturn += "<div class ='col-md-3 text-center' style='padding-right:5px;'><button style ='display:block;' type='button' title='Add Remark' class='btn green-jungle btn-xs btAddRemark'><i class=' fa fa-edit'></i> </button></div>";

                            strReturn += "</div>";
                            return strReturn;
                        }
                    }
                }
            ],
            "columnDefs": [{ "targets": [0], "orderable": true }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                Common.CheckError.List(tblSummary.data());
                //App.unblockUI("#divDataMaster");
                //App.unblockUI({ target: "#formSearch", animate: !0 });
                App.unblockUI("#Main");
            },
            "fnPreDrawCallback": function () {
                App.blockUI({ target: "#Main", boxed: true });
            },            
            "order": [],
            "scrollY": 500, /* Enable vertical scroll to allow fixed columns */
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
        });
        $("#tblSummaryMonthlys tbody").on("click", ".btAddRemark", function (e) {
            var table = $("#tblSummaryMonthlys").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                $("#MdLblCustomer").html(data.OperatorID);
                $("#MdLblForecast").html(data.TotalForecast);
                $("#MdLblActual").html(data.TotalActual);
                $("#MdLblVarianceTotal").html(data.TotalVariance);
                $("#MdLblVariancePercentage").html(data.PercentageVariance);
                $("#MdTxtRemark").html(data.Remarks);
                $('#MdAddRemark').modal('show');

            }
        });



    },

    ExportSummary: function (table, name, filename) {
        var uri = 'data:application/vnd.ms-excel;base64,'
            , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><meta charset="utf-8"/><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
            , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
            , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
        var ctx = {}
        var head = document.getElementById("tblSummaryHead")
        table = document.getElementById(table)
        ctx = { worksheet: name || 'Worksheet', table: head.innerHTML + table.innerHTML }
        document.getElementById("dlink").href = uri + base64(format(template, ctx));
        document.getElementById("dlink").download = filename;
        document.getElementById("dlink").click();
    },
    ExportDetail: function () {

        var Type = _ParamExport.Type;
        var STIPCategory = _ParamExport.STIPCategory;
        var RFIDateYear = _ParamExport.RFIDateYear;
        var RFIDateMonth = _ParamExport.RFIDateMonth;
        var week = _ParamExport.RFIDateWeek;
        var step = _ParamExport.Step;
        var CustomerID = _ParamExport.CustomerID;

        var SoNumber = $("#slSearchDetailSonum").val();
        var SiteID = $("#slSearchDetailSiteID").val();
        var SiteName = $("#slSearchDetailSiteName").val();

        window.location.href = "/DashboardRA/ExportDetailPotentialRFI?strType=" + Type + "&strSTIPCategory=" + STIPCategory + "&strRFIDateYear=" + RFIDateYear
            + "&RFIDateMonth=" + RFIDateMonth + "&CustomerID=" + CustomerID + "&Week=" + week + "&Step=" + step + "&SoNumber=" + SoNumber + "&SiteID=" + SiteID + "&SiteName=" + SiteName

    },
}

var Control = {
    BindingSelectYear: function (id) {
        $.ajax({
            url: "/api/CashInRemarkMonthly/filter",
            type: "GET",
            data: {
                Type: "FilterYears",
                Year: 0,
                Month: 0
            },
        })
            .done(function (data, textstatus, jqxhr) {
                
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.val + "'>" + item.name + "</option>");
                    })
                }
                $(id).val(_Year);
            })
            .fail(function (jqxhr, textstatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectMonth: function (id) {
        $.ajax({
            url: "/api/CashInRemarkMonthly/filter",
            type: "GET",
            data: {
                Type: "FilterMonths",
                Year: 0, //new Date().getFullYear(),
                Month: 0
            },
        })
            .done(function (data, textstatus, jqxhr) {
                
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        _Month = item.SetMonth;
                        $(id).append("<option value='" + item.val + "'>" + item.name + "</option>");
                    })
                }
                $(id).val(_Month);
            })
            .fail(function (jqxhr, textstatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectApproval: function (id) {
        $.ajax({
            url: "/api/CashInRemarkMonthly/filter",
            type: "GET",
            data: {
                Type: "FilterAction",
                Year: 0, //new Date().getFullYear(),
                Month: 0
            },
        })
            .done(function (data, textstatus, jqxhr) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.val + "'>" + item.name + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Action", width: null });
            })
            .fail(function (jqxhr, textstatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    }

}

var Approval = {
    ShowModal: function () {
        $('#MdApproval').modal('show');
        $("#tbRemarkApproval").val('')
        Control.BindingSelectApproval($('#slApproval'));
    },
    
   
    Submit: function () {
        App.blockUI({ target: "#Main", animate: !0 });
        if ($("#slApproval").val() != '' && $("#tbRemarkApproval").val() != '') {
            $.ajax({
                url: "/api/CashInRemarkMonthly/approval",
                type: "GET",
                data: {                    
                    Type: "MONTHLY",
                    Year: $("#slSearchYear").val(),
                    Month: $("#slSearchMonth").val(),
                    Isapproval: $("#slApproval").val(),
                    Remark: $("#tbRemarkApproval").val()
                },
            })
                .done(function (data, textstatus, jqxhr) {

                    if (Common.CheckError.List(data)) {
                        var IsError = 0;
                        var message = '';
                        $.each(data, function (i, item) {
                            IsError = item.IsError;
                            message = item.message;
                        })
                    }
                    if (IsError == 1) {
                        Common.Alert.Error(message);
                    }
                    else {
                        Common.Alert.Success("Success to Submit data !");
                        $('#MdApproval').modal('hide');
                        Table.Summary();
                    }
                    App.unblockUI("#Main");
                })
                .fail(function (jqxhr, textstatus, errorThrown) {
                    Common.Alert.Error(errorThrown);
                    App.unblockUI("#Main");
                });       
        }
        else {
            Common.Alert.Warning("Please Select Action And Fill Remark!");
            //$("#btSubmit").prop('disabled', true);
        }



    }
}
