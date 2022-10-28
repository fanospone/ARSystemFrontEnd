var _Year = new Date().getFullYear().toString();
var _Month = new Date().getMonth().toString();
var Data = {};


jQuery(document).ready(function () {
    Form.Init();
   
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
     

});


var Form = {
    Init: function () {
        
        Control.BindingSelectYear($('#slSearchYear'));
        Control.BindingSelectMonth($('#slSearchMonth'));
        
    },
    Reset: function () {
        location.reload();
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
            $("#MdTxtRemark").val('');
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

        swal({
            title: "Are you sure want to submit?",
            text: "",
            type: "warning",
            allowOutsideClick: true,
            showConfirmButton: true,
            showCancelButton: true,
            confirmButtonClass: "btn-warning",
            cancelButtonClass: "btn-default",
            closeOnConfirm: false,
            closeOnCancel: true,
        },
        function (isConfirm) {
            if (isConfirm) {
                App.blockUI({ target: "#Main", boxed: true });
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
                            else if (IsError == 3)
                            {
                                Common.Alert.Warning(message);
                            }
                            else {
                                Common.Alert.Success("Success to Submit data !");
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
                    Common.Alert.Warning("Data Is Nothing!");
                    App.unblockUI("#Main");
                     
                }

            }
        });
        

        
    }
}

var Table = {
    Summary: function () {
        
        var params = {
            Periode: $("#slSearchYear").val(),
            Month: $("#slSearchMonth").val()
        }
        $('#thTitle').html($("#slSearchMonth option:selected").text() + ' ' + $("#slSearchYear").val());
        var tblSummary = $("#tblSummaryMonthlys").DataTable({
            "orderCellsTop": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/CashInRemarkMonthly/grid",
                "type": "POST",
                "datatype": "json",
                "data": params
            },
            buttons: [
                { text: 'Excel', className: 'btn green btn-outline', action: function (e, dt, node, config) { Table.ExportSummary('tblSummaryMonthlys', 'Monthly - ' + $("#slSearchMonth").val() + ' ', 'ARR Monitoring Cash In (Monthly - ' + $("#slSearchMonth").val()+')'); } },
                { extend: 'colvis', className: 'btn dark btn-outline', text: 'Columns' }
            ],
            "filter": false,
            "lengthMenu": [[-1], ['All']],
            "destroy": true,
            "columns": [
                //{
                //    orderable: false,
                //    mRender: function (data, type, full) {
                //        var strReturn = "<div class='row'>";
                //        {
                //            strReturn += "<div class ='col-md-3 text-center' style='padding-right:5px;'><button style ='display:block;' type='button' title='Add Remark' class='btn green-jungle btn-xs btAddRemark'><i class=' fa fa-edit'></i> </button></div>";
                             
                //            strReturn += "</div>";
                //            return strReturn;
                //        }
                //    }
                //},  
                //{ data: "OperatorID" },
                //{
                //    'mRender': function (data, type, row, meta) {
                //        var id = meta.settings.fnFormatNumber(row.TotalForecast);
                //        return id;
                //    }
                //},
                //{
                //    'mRender': function (data, type, row, meta) {
                //        var id = meta.settings.fnFormatNumber(row.TotalActual);
                //        return id;
                //    }
                //},
                //{
                //    'mRender': function (data, type, row, meta) {
                //        var id = meta.settings.fnFormatNumber(row.TotalVariance );
                //        return id;
                //    }
                //},
                //{
                //    'mRender': function (data, type, row, meta) {
                //        var id = meta.settings.fnFormatNumber(row.PercentageVariance);
                //        return id +" %";
                //    }
                //},               
                //{ data: "Remarks" }
                { data: "OperatorID" },
                {
                    'mRender': function (data, type, row, meta) {
                        var id = row.TotalForecast != null ? meta.settings.fnFormatNumber(row.TotalForecast || 0) : null;
                        return id;
                    }
                },
                {
                    'mRender': function (data, type, row, meta) {
                        var id = row.TotalActual != null ? meta.settings.fnFormatNumber(row.TotalActual || 0) : null;
                        return id;
                    }
                },
                {
                    'mRender': function (data, type, row, meta) {
                        var id = row.TotalVariance != null ? meta.settings.fnFormatNumber(row.TotalVariance || 0) : null;
                        return id;
                    }
                },
                {
                    'mRender': function (data, type, row, meta) {
                        var id = row.PercentageVariance != null ? meta.settings.fnFormatNumber(row.PercentageVariance || 0) : 0;
                        return id + " %";
                    }
                },
                //{ data: "Remarks" }

                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var val = full.Remarks;
                        var col = "";
                        //if (full.TotalVariance != 0) {
                        //    if (val == "null" || val == null) {
                        //        val = "";
                        //        col = "1";
                        //    }
                        //    else {
                        //        val = val.substring(0, 30) + "...";
                        //        col = "10";
                        //    }

                        //    var strReturn = "<div class='row'>";
                        //    strReturn += " <div class ='col-md-" + col + "' > " + val + "</div><button style ='display:block;' type='button' title='Add Remark' class='btn green-jungle btn-xs btAddRemark'> <i class=' fa fa-edit'></i> </button>";

                        //    strReturn += "</div>";
                        //    return strReturn;
                        //}
                        //else
                        //    return "";
                        if (val == "null" || val == null) {
                            val = "";
                            col = "1";
                        }
                        else {
                            val = val.substring(0, 30) + "...";
                            col = "10";
                        }

                        var strReturn = "<div class='row'>";
                        strReturn += " <div class ='col-md-" + col + "' > " + val + "</div><button style ='display:block;' type='button' title='Add Remark' class='btn green-jungle btn-xs btAddRemark'> <i class=' fa fa-edit'></i> </button>";

                        strReturn += "</div>";
                        return strReturn;

                    }
                }
            ],
            "columnDefs": [{ "targets": [0], "orderable": true }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
             
            "fnDrawCallback": function () {
                Common.CheckError.List(tblSummary.data());
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
                if (data.Remarks != null && data.Remarks != 'null')
                    $("#MdTxtRemark").val(data.Remarks);
                else
                    $("#MdTxtRemark").val('');
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
        var head = document.getElementById("tblSummaryMonthlysHead")
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
        App.blockUI({ target: "#Main", boxed: true });
        $.ajax({
            url: "/api/CashInRemarkMonthly/filter",
            type: "GET",
            async: false,
            data: {
                Type: "FilterYears",
                Year: 0,
                Month: 0
            },
        })
            .done(function (data, textstatus, jqxhr) {
                $(id).empty();
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.val + "'>" + item.name + "</option>");
                    })
                }
             
                $(id).val(_Year);
                App.unblockUI("#Main");
            })
            .fail(function (jqxhr, textstatus, errorThrown) {
                App.unblockUI("#Main");
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectMonth: function (id) {
        App.blockUI({ target: "#Main", boxed: true });
        $.ajax({
            url: "/api/CashInRemarkMonthly/filter",
            type: "GET",
            async: false,
            data: {
                Type: "FilterMonths",
                Year: 0,  
                Month: 0
            },
        })
            .done(function (data, textstatus, jqxhr) {
                $(id).empty();
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        _Month = item.SetMonth;
                        $(id).append("<option value='" + item.val + "'>" + item.name + "</option>");
                    })
                }            
                $(id).val(_Month);
                App.unblockUI("#Main");
            })
            .fail(function (jqxhr, textstatus, errorThrown) {
                App.unblockUI("#Main");
                Common.Alert.Error(errorThrown);
            });
    }
    
}


