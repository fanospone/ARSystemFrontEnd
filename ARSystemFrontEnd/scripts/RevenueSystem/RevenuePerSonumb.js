Data = {};

var fsAccount = "";
var fsPeriode = "";
var fsCompanyId = "";
var fsRegional = "";
var fsOperatorId = "";
var fsProductId = "";

jQuery(document).ready(function () {
    var dt = new Date();
    $('#slSearchYear').val(dt.getFullYear()).trigger('change');
    $('#slSearchYearTo').val(dt.getFullYear()).trigger('change');

    $('#slSearchYear').on('change', function () { Control.YearPeriodFilterChange("#slSearchYear") });
    $('#slSearchYearTo').on('change', function () { Control.YearPeriodFilterChange("#slSearchYearTo") });



    Control.BindingSelectCompany($('#slSearchCompanyName'));
    Control.BindingSelectRegional($('#slSearchRegional'));
    Control.BindingSelectOperator($('#slSearchOperator'));
    Control.BindingSelectProduct($('#slSearchProduct'));

    $("#btSearch").unbind().click(function () {
        Table.Search();
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
    });

    $('input.number').keyup(function (event) {

        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return Helper.FormatNumberForm(value);
        });
    });

    Table.Init();
});

var Control = {
    BindingSelectCompany: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                elements.html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        elements.append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                    })
                }

                elements.select2({ placeholder: "Select Company Name", width: null });

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

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        elements.append("<option value='" + item.Regional + "'>" + item.Regional + "</option>");
                    })
                }

                elements.select2({ placeholder: "Select Regional", width: null });

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectOperator: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                elements.html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        elements.append("<option value='" + $.trim(item.OperatorId) + "'>" + item.OperatorId + "</option>");
                    })
                }

                elements.select2({ placeholder: "Select Operator", width: null });

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
                        //elements.append("<option value='" + item.Value.trim() + "'>" + item.Text + "</option>");
                        elements.append("<option value='" + item.Text + "'>" + item.Text + "</option>");
                    })
                }

                elements.select2({ placeholder: "Select Product", width: null });

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    YearPeriodFilterChange: function(ele){
        if (ele == '#slSearchYearTo') {
            if ((parseInt($(ele).val()) || 0) < (parseInt($('#slSearchYear').val()) || 0)) {
                Common.Alert.Warning("Cannot set End Year less than Start Year.")
                $('#slSearchYearTo').val($('#slSearchYear').val())
            }
        } else {
            if ((parseInt($(ele).val()) || 0) > (parseInt($('#slSearchYearTo').val()) || 0)) {
                Common.Alert.Warning("Cannot set Start Year bigger than End Year.")
                $('#slSearchYear').val($('#slSearchYearTo').val())
            }
        }
    }
}

var Table = {
    Init: function () {
        $(".panelSearchZero").hide();
        $('#tblvwARRevSysPerSonumb').dataTable();
        $('#tblvwARRevSysBalancePerSonumb').dataTable();

        $(".btnSearch").unbind().click(function () {
            Table.Search();
        });

        $('#tblvwARRevSysPerSonumb input, #tblvwARRevSysBalancePerSonumb input').keypress(function (e) {
            if (e.which == 13) { Table.Search(); }
        });

        $('#tblvwARRevSysPerSonumb select, #tblvwARRevSysBalancePerSonumb select').change(function () {
            Table.Search();
        });

    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsAccount = $("#slSearchAccount").val();
        fsPeriode = $("#slSearchYear").val() == null ? "" : $("#slSearchYear").val();
        fsPeriodeTo = $("#slSearchYearTo").val() == null ? "" : $("#slSearchYearTo").val();
        fsCompany = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsRegional = $("#slSearchRegional").val() == null ? "" : $("#slSearchRegional").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsProduct = $("#slSearchProduct").val() == null ? "" : $("#slSearchProduct").val();
        var SoNumber = $("#schSoNumber").val().replace(/'/g, "''");

        params = {
            strAccount: fsAccount,
            strPeriode: fsPeriode,
            strPeriodeTo: fsPeriodeTo,
            strCompany: fsCompany,
            strRegional: fsRegional,
            strOperator: fsOperator,
            strProduct: fsProduct,
            schSoNumber: SoNumber
        };

        //load grid
        if (fsAccount == "REVENUE") {
            //hide table balance and show table revenue
            $("#sectionBalancePerSonumb").hide();
            $("#sectionRevenuePerSonumb").show();

            var tblList = $("#tblvwARRevSysPerSonumb").DataTable({
                "deferRender": true,
                "proccessing": true,
                "serverSide": true,
                "bSortCellsTop": true,
                "language": {
                    "emptyTable": "No data available in table",
                },
                "ajax": {
                    "url": "/Api/RevenueSystem/grid",
                    "type": "POST",
                    "datatype": "json",
                    "data": params,
                },
                buttons: [
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
                "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
                "destroy": true,
                "columns": [
                    { data: null },
                    {
                        data: 'SoNumber',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<a class='rowSoNumber' data-sonumber='" + oData.SoNumber + "' data-siteid='" + oData.SiteID +
                                "' data-sitename='" + oData.SiteName + "' data-customerid='" + oData.CustomerID + "' data-productname='" + oData.ProductName +
                                "' data-regional='" + oData.RegionName + "' data-status='" + oData.Status + "' data-stipsiro='" + oData.StipSiro + "' href='#'>" + oData.SoNumber + "</a>");
                        }
                    },
                    { data: 'Periode' },
                    { data: 'SiteName' },
                    { data: 'Operator' },
                    { data: 'RegionName' },
                    {
                        data: "Jan_RevenueNormal", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Jan_Inflasi", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Jan_Overblast", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Jan_NetRevenue", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Jan_PPN", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Feb_RevenueNormal", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Feb_Inflasi", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Feb_Overblast", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Feb_NetRevenue", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Feb_PPN", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },

                    {
                        data: "Mar_RevenueNormal", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Mar_Inflasi", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Mar_Overblast", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Mar_NetRevenue", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Mar_PPN", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Apr_RevenueNormal", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Apr_Inflasi", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Apr_Overblast", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Apr_NetRevenue", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Apr_PPN", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "May_RevenueNormal", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "May_Inflasi", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "May_Overblast", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "May_NetRevenue", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "May_PPN", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Jun_RevenueNormal", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Jun_Inflasi", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Jun_Overblast", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Jun_NetRevenue", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Jun_PPN", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Jul_RevenueNormal", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Jul_Inflasi", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Jul_Overblast", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Jul_NetRevenue", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Jul_PPN", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Aug_RevenueNormal", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Aug_Inflasi", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Aug_Overblast", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Aug_NetRevenue", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Aug_PPN", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Sep_RevenueNormal", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Sep_Inflasi", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Sep_Overblast", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Sep_NetRevenue", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Sep_PPN", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Oct_RevenueNormal", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Oct_Inflasi", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Oct_Overblast", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Oct_NetRevenue", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Oct_PPN", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Nov_RevenueNormal", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Nov_Inflasi", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Nov_Overblast", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Nov_NetRevenue", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Nov_PPN", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Dec_RevenueNormal", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Dec_Inflasi", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Dec_Overblast", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Dec_NetRevenue", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }, {
                        data: "Dec_PPN", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "TotalBalance", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }

                ],
                "columnDefs": [{ "targets": [0, 1], "orderable": false }],
                "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                "fnPreDrawCallback": function () {
                    App.blockUI({ target: ".panelSearchResult", boxed: true });
                },
                "fnDrawCallback": function () {
                    if (Common.CheckError.List(tblList.data())) {
                        $(".panelSearchBegin").hide();
                        $(".panelSearchResult").show();
                    }

                    //Link SoNumber klik
                    $('.rowSoNumber').click(function (e) {

                        var sonumb = this.getAttribute('data-sonumber');
                        var siteID = this.getAttribute('data-siteid');
                        var siteName = this.getAttribute('data-sitename');
                        var customerID = this.getAttribute('data-customerid');
                        var regional = this.getAttribute('data-regional');
                        var productName = this.getAttribute('data-productname');
                        var status = this.getAttribute('data-status');
                        var stipSiro = this.getAttribute('data-stipsiro');
                        var strUrl = "/RevenueSystem/SoNumberDetail?strSoNumber=" + sonumb + "&strSiteID=" + siteID + "&strSiteName=" + siteName + "&strCustomerID=" + customerID +
                            "&strRegional=" + regional + "&strProductName=" + productName + "&strStatus=" + status + "&intStipSiro=" + stipSiro;
                        window.open(strUrl, "_blank");
                    })

                    l.stop();
                    App.unblockUI('.panelSearchResult');
                },
                "columnDefs": [
                    {
                        "targets": [6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                          21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37,
                          38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54,
                          55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66], "className": "text-right"
                    },
                ],
                "order": false
            });
            //RowNumber No.
            tblList.on('draw.dt', function () {
                var PageInfo = $('#tblvwARRevSysPerSonumb').DataTable().page.info();
                tblList.column(0, { page: 'current' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1 + PageInfo.start;
                });
            });

        }else{
            //hide table balance and show table revenue
            $("#sectionBalancePerSonumb").show();
            $("#sectionRevenuePerSonumb").hide();

            //Table for filter Balance Accrued and Balance Unearned
            var tblListBalance = $("#tblvwARRevSysBalancePerSonumb").DataTable({
                "deferRender": true,
                "proccessing": true,
                "serverSide": true,
                "bSortCellsTop": true,
                "language": {
                    "emptyTable": "No data available in table",
                },
                "ajax": {
                    "url": "/Api/RevenueSystem/grid",
                    "type": "POST",
                    "datatype": "json",
                    "data": params,
                },
                buttons: [
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
                "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
                "destroy": true,
                "columns": [
                    { data: null },
                    {
                        data: 'SoNumber',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<a class='rowSoNumber' data-sonumber='" + oData.SoNumber + "' data-siteid='" + oData.SiteID +
                                "' data-sitename='" + oData.SiteName + "' data-customerid='" + oData.CustomerID + "' data-productname='" + oData.ProductName +
                                "' data-regional='" + oData.RegionName + "' data-status='" + oData.Status + "' data-stipsiro='" + oData.StipSiro + "' href='#'>" + oData.SoNumber + "</a>");
                        }
                    },
                    { data: 'SiteName' },
                    { data: 'Operator' },
                    { data: 'RegionName' },
                    {
                        data: "Jan", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Feb", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Mar", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Apr", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "May", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Jun", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Jul", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Aug", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Sep", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Oct", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Nov", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "Dec", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    },
                    {
                        data: "TotalBalance", render: function (val, type, full) {
                            val = val == null ? 0 : val;
                            return Common.Format.CommaSeparation(val);
                        },
                    }

                ],
                "columnDefs": [{ "targets": [0, 1], "orderable": false }, {
                    "targets": [6, 7, 8, 9, 10, 11, 12, 13, 14,15,16,17], "className": "text-right"
                }],
                "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                "fnPreDrawCallback": function () {
                    App.blockUI({ target: ".panelSearchResult", boxed: true });
                },
                "fnDrawCallback": function () {
                    if (Common.CheckError.List(tblListBalance.data())) {
                        $(".panelSearchBegin").hide();
                        $(".panelSearchResult").show();
                    }

                    //Link SoNumber klik
                    $('.rowSoNumber').click(function (e) {

                        var sonumb = this.getAttribute('data-sonumber');
                        var siteID = this.getAttribute('data-siteid');
                        var siteName = this.getAttribute('data-sitename');
                        var customerID = this.getAttribute('data-customerid');
                        var regional = this.getAttribute('data-regional');
                        var productName = this.getAttribute('data-productname');
                        var status = this.getAttribute('data-status');
                        var stipSiro = this.getAttribute('data-stipsiro');
                        var strUrl = "/RevenueSystem/SoNumberDetail?strSoNumber=" + sonumb + "&strSiteID=" + siteID + "&strSiteName=" + siteName + "&strCustomerID=" + customerID +
                            "&strRegional=" + regional + "&strProductName=" + productName + "&strStatus=" + status + "&intStipSiro=" + stipSiro;
                        window.open(strUrl, "_blank");
                    })

                    l.stop();
                    App.unblockUI('.panelSearchResult');
                },
                "order": false
            });



            //RowNumber No.
            tblListBalance.on('draw.dt', function () {
                var PageInfo = $('#tblvwARRevSysBalancePerSonumb').DataTable().page.info();
                tblListBalance.column(0, { page: 'current' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1 + PageInfo.start;
                });
            });
        }

    },
    Reset: function () {
        fsAccount = "";
        fsPeriode = "";
        fsCompanyId = "";
        fsRegional = "";
        fsOperatorId = "";
        fsProductId = "";

        $("#slSearchAccount").val("REVENUE").trigger('change');
        $("#slSearchYear").val(" ").trigger('change');
        $("#slSearchYearTo").val(" ").trigger('change');
        $("#slSearchCompanyName").val("").trigger('change');
        $("#slSearchRegional").val("").trigger('change');
        $("#slSearchOperator").val("").trigger('change');
        $("#slSearchProduct").val("").trigger('change');


    },
    Export: function () {

        var fAccount = $("#slSearchAccount").val();
        var fYear = $("#slSearchYear").val();
        var fYear = $("#slSearchYearTo").val();
        var fCompany = $("#slSearchCompanyName").val();
        var fRegionalName = $("#slSearchRegional").val();
        var fOperator = $("#slSearchOperator").val();
        var fProduct = $("#slSearchProduct").val();
        var schSoNumber = $("#schSoNumber").val();
        window.location.href = "/RevenueSystem/RevenuePerSonumb/Export?strAccount=" + fAccount + "&strPeriode=" + fYear + "&strCompany=" + fCompany
            + "&strRegionName=" + fRegionalName + "&strOperator=" + fOperator + "&strProduct=" + fProduct + "&schSoNumber=" + schSoNumber;
    }
}

var Helper = {
    FormatNumberGrid: function FormatNumberGrid(angka) {
        var FormatNumberGrid = $.fn.dataTable.render.number(',', '.', 2, '').display;
        return FormatNumberGrid(angka);
    },
    FormatNumberForm: function (value) {
        // format number
        return value
            .replace(/\D/g, "")
            .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
        ;
    }
}
