$(function () {
    Control.Init();
})

var Tables = {
    ReportSummary: function () {
        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();
        var params = {
            fAccount      : $('#fAccount').val(),
            fYear         : $('#fYear').val(),
            fCompanyId    : $('#fCompany').val(),
            fRegionalName : $('#fRegional').val(),
            fOperatorId   : $('#fOperator').val(),
            fProduct      : $('#fProduct').val(),
            fGroupBy      : $('input[name=rbGroupBy]:checked').val(),
            fViewBy       : $('input[name=rbViewBy]:checked').val()
        }
        var tblList = $("#tblReportSummary").DataTable({
            "deferRender": true,
            "proccessing": true,
            //"serverSide": true,
            "language": {
                "emptyTable": "No data available"
            },
            "ajax": {
                "url": "/api/RevenueSystem/GetSummary",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
               {
                   text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                       var l = Ladda.create(document.querySelector(".yellow"));
                       l.start();
                       Tables.Export()
                       l.stop();
                   }
               },
               { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 30, 50, 100], ['10', '25', '50', '100']],
            "destroy": true,
            "columns": [
                {data:null},
                { data: "Description" },
                {
                    data: "Jan",
                    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html("<a class='rowSummary' data-desc='"+oData.Description+"' data-month='1'>" + Common.Format.CommaSeparation(oData.Jan) + "</a>");
                    }
                },
                {
                    data: "Feb",
                    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html("<a class='rowSummary' data-desc='" + oData.Description + "' data-month='2'>" + Common.Format.CommaSeparation(oData.Feb) + "</a>");
                    }
                },
                {
                    data: "Mar",
                    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html("<a class='rowSummary' data-desc='" + oData.Description + "' data-month='3'>" + Common.Format.CommaSeparation(oData.Mar) + "</a>");
                    }
                },
                {
                    data: "Apr",
                    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html("<a class='rowSummary' data-desc='" + oData.Description + "' data-month='4'>" + Common.Format.CommaSeparation(oData.Apr) + "</a>");
                    }
                },
                {
                    data: "May",
                    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html("<a class='rowSummary' data-desc='" + oData.Description + "' data-month='5'>" + Common.Format.CommaSeparation(oData.May) + "</a>");
                    }
                },
                {
                    data: "Jun",
                    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html("<a class='rowSummary' data-desc='" + oData.Description + "' data-month='6'>" + Common.Format.CommaSeparation(oData.Jun) + "</a>");
                    }
                },
                {
                    data: "Jul",
                    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html("<a class='rowSummary' data-desc='" + oData.Description + "' data-month='7'>" + Common.Format.CommaSeparation(oData.Jul) + "</a>");
                    }
                },
                {
                    data: "Aug",
                    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html("<a class='rowSummary' data-desc='" + oData.Description + "' data-month='8'>" + Common.Format.CommaSeparation(oData.Aug) + "</a>");
                    }
                },
                {
                    data: "Sep",
                    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html("<a class='rowSummary' data-desc='" + oData.Description + "' data-month='9'>" + Common.Format.CommaSeparation(oData.Sep) + "</a>");
                    }
                },
                {
                    data: "Oct",
                    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html("<a class='rowSummary' data-desc='" + oData.Description + "' data-month='10'>" + Common.Format.CommaSeparation(oData.Oct) + "</a>");
                    }
                },
                {
                    data: "Nov",
                    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html("<a class='rowSummary' data-desc='" + oData.Description + "' data-month='11'>" + Common.Format.CommaSeparation(oData.Nov) + "</a>");
                    }
                },
                {
                    data: "Dec",
                    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html("<a class='rowSummary' data-desc='" + oData.Description + "' data-month='12'>" + Common.Format.CommaSeparation(oData.Dec) + "</a>");
                    }
                },
                {
                    data: "Total",
                    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html("<a class='rowSummary' data-desc='" + oData.Description + "'>" + Common.Format.CommaSeparation(oData.Total) + "</a>");
                    }
                },
                {
                    data: "TotalInPercent",
                    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html("<a class='rowSummary' data-desc='" + oData.Description + "'>" + Common.Format.CommaSeparation(oData.TotalInPercent) + "%</a>");
                    }
                },
            ],
            "columnDefs": [
                { "targets": [0], "orderable": false },
                { "targets": [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14], "orderable": false, "className": "dt-right" },
                { "targets": [15], "orderable": false, "className": "dt-center" },//total inpercent
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            'scrollX': true,
            'scrollCollapse': true,
            'fixedColumns': {
                leftColumns: 2
            },
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function (result) {
                if (result.json != undefined && result.json.data != undefined) {
                    if (Common.CheckError.List(this.fnGetData())) {
                        $(".panelSearchResult").fadeIn(1000);
                    }
                    l.stop(); App.unblockUI('.panelSearchResult');
                    
                    //link rows
                    $('.rowSummary').click(function (e) {
                        var fAccount = $('#fAccount').val().trim();
                        var fYear = $('#fYear').val().trim();
                        var fCompanyId = $('#fCompany').val().trim();
                        var fRegionalName = $('#fRegional').val().trim();
                        var fOperatorId = $('#fOperator').val().trim();
                        var fProduct = $('#fProduct').val().trim();
                        var fGroupBy = $('input[name=rbGroupBy]:checked').val();
                        //var fViewBy       = $('input[name=rbViewBy]:checked').val();
                        var desc = this.getAttribute('data-desc');
                        var month = this.getAttribute('data-month');
                        var strUrl          = "/RevenueSystem/SoNumberList?strGroupBy=" + fGroupBy + "&strAccount=" + fAccount + "&intYear=" + fYear + "&strCompanyId=" + fCompanyId
                                            + "&strRegionName=" + fRegionalName + "&strOperatorId=" + fOperatorId + "&strProduct=" + fProduct + "&strDesc=" + desc + "&intMonth=" + month;
                        window.open(strUrl, "_blank");
                    });

                    //Link total
                    $('.rowSum').click(function (e) {

                        var fAccount = $('#fAccount').val().trim();
                        var fYear = $('#fYear').val().trim();
                        var fCompanyId = $('#fCompany').val().trim();
                        var fRegionalName = $('#fRegional').val().trim();
                        var fOperatorId = $('#fOperator').val().trim();
                        var fProduct = $('#fProduct').val().trim();
                        var fGroupBy = $('input[name=rbGroupBy]:checked').val();
                        var month = this.getAttribute('data-month');
                        var strUrl = "/RevenueSystem/SoNumberList?strGroupBy=" + fGroupBy + "&strAccount=" + fAccount + "&intYear=" + fYear + "&strCompanyId=" + fCompanyId
                                            + "&strRegionName=" + fRegionalName + "&strOperatorId=" + fOperatorId + "&strProduct=" + fProduct + "&intMonth=" + month;
                        window.open(strUrl, "_blank");
                    })
                }
            },
            "order": [1, "asc"],
            //"scrollY": 300,
            //"scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            //"fixedColumns": {
            //    leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            //},
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api();

                // converting to interger to find total
                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                        i : 0;
                };

                // computing column Total of the complete result 
                var JanTot = api.column(2).data().reduce(function (a, b) {
                                return intVal(a) + intVal(b);
                }, 0);
                var FebTot = api.column(3).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var MarTot = api.column(4).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var AprTot = api.column(5).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var MayTot = api.column(6).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var JunTot = api.column(7).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var JulTot = api.column(8).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var AugTot = api.column(9).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var SepTot = api.column(10).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var OctTot = api.column(11).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var NovTot = api.column(12).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var DecTot = api.column(13).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var GraTot = api.column(14).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                
                var GraTotInPercent = api.column(15).data().reduce(function (a, b) {
                    return intVal(Number(a)) + Number(Number(b));
                }, 0);

                // Update footer by showing the total with the reference of the column index 
                $(api.column(1).footer()).html('Total');
                $(api.column(2).footer()).html('<a class="rowSum" data-month="1">' + Common.Format.CommaSeparation(JanTot) + '</a>');
                $(api.column(3).footer()).html('<a class="rowSum" data-month="2">' + Common.Format.CommaSeparation(FebTot) + '</a>');
                $(api.column(4).footer()).html('<a class="rowSum" data-month="3">' + Common.Format.CommaSeparation(MarTot) + '</a>');
                $(api.column(5).footer()).html('<a class="rowSum" data-month="4">' + Common.Format.CommaSeparation(AprTot) + '</a>');
                $(api.column(6).footer()).html('<a class="rowSum" data-month="5">' + Common.Format.CommaSeparation(MayTot) + '</a>');
                $(api.column(7).footer()).html('<a class="rowSum" data-month="6">' + Common.Format.CommaSeparation(JunTot) + '</a>');
                $(api.column(8).footer()).html('<a class="rowSum" data-month="7">' + Common.Format.CommaSeparation(JulTot) + '</a>');
                $(api.column(9).footer()).html('<a class="rowSum" data-month="8">' + Common.Format.CommaSeparation(AugTot) + '</a>');
                $(api.column(10).footer()).html('<a class="rowSum" data-month="9">' + Common.Format.CommaSeparation(SepTot) + '</a>');
                $(api.column(11).footer()).html('<a class="rowSum" data-month="10">' + Common.Format.CommaSeparation(OctTot) + '</a>');
                $(api.column(12).footer()).html('<a class="rowSum" data-month="11">' + Common.Format.CommaSeparation(NovTot) + '</a>');
                $(api.column(13).footer()).html('<a class="rowSum" data-month="12">' + Common.Format.CommaSeparation(DecTot) + '</a>');
                $(api.column(14).footer()).html('<a class="rowSum">' + Common.Format.CommaSeparation(GraTot) + '</a>');
                $(api.column(15).footer()).html(Common.Format.CommaSeparation(GraTotInPercent) + " %");
            },
        });//end of DataTables

        $("#tblReportSummary tbody").unbind();

        //RowNumber No.
        tblList.on('draw.dt', function () {
            var PageInfo = $('#tblReportSummary').DataTable().page.info();
            tblList.column(0, { page: 'current' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1 + PageInfo.start;
            });
        });
    },

    Export: function () {
        var fAccount      = $('#fAccount').val();
        var fYear         = $('#fYear').val();
        var fCompanyId    = $('#fCompany').val();
        var fRegionalName = $('#fRegional').val();
        var fOperatorId   = $('#fOperator').val();
        var fProduct      = $('#fProduct').val();
        var fGroupBy      = $('input[name=rbGroupBy]:checked').val();
        var fViewBy       = $('input[name=rbViewBy]:checked').val();
        window.location.href = "/RevenueSystem/Summary/Export?strViewBy=" + fViewBy + "&strGroupBy="+fGroupBy+"&strAccount=" + fAccount + "&intYear=" + fYear + "&strCompanyId=" + fCompanyId
            + "&strRegionName=" + fRegionalName + "&strOperatorId=" + fOperatorId + "&strProduct="+fProduct;
    },
    HoverOverrideByCommonJs: function () {
        $(document).on({
            mouseenter: function () {
                trIndex = $(this).index() + 1;
                $("table.dataTable").each(function (index) {
                    $(this).find("tr:eq(" + trIndex + ")").removeClass("hover")
                });
            },
            mouseleave: function () {
                trIndex = $(this).index() + 1;
                $("table.dataTable").each(function (index) {
                    $(this).find("tr:eq(" + trIndex + ")").removeClass("hover")
                });
            }
        }, ".dataTables_wrapper tr");
    }
}


var Control = {
    Init: function () {
        Control.BindingSelectAccount();
        Control.BindingSelectYear();
        Control.BindingSelectCompany();
        Control.BindingSelectRegional();
        Control.BindingSelectOperator();
        Control.BindingSelectProduct();
        $("#btSearch").unbind().click(function () {
            Tables.ReportSummary();
        });
        $("#btReset").unbind().click(function () {
            Control.Reset();
        });

        //init grid no data
        $("#tblReportSummary").DataTable();

        Tables.HoverOverrideByCommonJs();
    },
    BindingSelectAccount : function(){
        var id = "#fAccount"
        $(id).append("<option value='REVENUE'>REVENUE</option>");
        $(id).append("<option value='BALANCE_ACCRUED'>BALANCE ACCRUED</option>");
        $(id).append("<option value='BALANCE_UNEARNED'>BALANCE UNEARNED</option>");
        $(id).select2();
    },
    BindingSelectYear: function () {
        var dt = new Date();
        var past = dt.getFullYear() - 5;
        var ydata = [''];
        for (var i = 0; i <= 10; i++) {
            ydata.push(past);
            past++;
        }
        $('#fYear').select2({
            data: ydata,
            placeholder: "Select Year"
        });

        $('#fYear').val(dt.getFullYear()).trigger('change');
    },
    BindingSelectCompany: function () {
        var id = "#fCompany"
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + item.CompanyId.trim() + "'>" + item.Company + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Company Name", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectRegional: function () {
        var id = "#fRegional"
        $.ajax({
            url: "/api/MstDataSource/Regional",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + item.RegionalName.trim() + "'>" + item.RegionalName + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Regional Name", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectOperator: function () {
        var id = "#fOperator"
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + item.OperatorId.trim() + "'>" + item.OperatorId + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Operator Name", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectProduct: function () {
        var id = "#fProduct"
        $.ajax({
            url: "/api/MstDataSource/TenantType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + item.Text.trim() + "'>" + item.Text + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Product", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    Reset: function () {
        var dt = new Date();
        $('#fYear').val(dt.getFullYear()).trigger('change');
        $('#fCompany').val(null).trigger('change');
        $('#fRegional').val(null).trigger('change');
        $('#fOperator').val(null).trigger('change');
        $('#fProduct').val(null).trigger('change');
    }
}
