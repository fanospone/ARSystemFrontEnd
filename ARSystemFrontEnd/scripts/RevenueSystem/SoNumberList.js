$(function () {
    Control.Init();
});

var Control = {
    Init: function () {
        Control.BindingSelectAccount();
        Control.BindingSelectYear();
        Control.BindingSelectCompany();
        Control.BindingSelectRegional();
        Control.BindingSelectOperator();
        Control.BindingSelectProduct();
        Control.BindingQueryParameters();

        $("#tblSonumbList").DataTable();//fixing Issue [object Object]

        $("#btSearch").unbind().click(function () {
            Tables.ReportSummary();
        });
        $("#btReset").unbind().click(function () {
            Control.Reset();
        });

    },
    BindingQueryParameters: function () {
        if (Common.Helper.getUrlParameter("strAccount") !== undefined && Common.Helper.getUrlParameter("strAccount") !== "") {
            $('#fAccount').val(Common.Helper.getUrlParameter("strAccount")).trigger('change');
        }
        if (Common.Helper.getUrlParameter("intYear") !== undefined && Common.Helper.getUrlParameter("intYear") !== "") {
            $('#fYear').val(Common.Helper.getUrlParameter("intYear")).trigger('change');
        }
    },
    BindingSelectAccount: function () {
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

                if (Common.Helper.getUrlParameter("strCompanyId") !== undefined && Common.Helper.getUrlParameter("strCompanyId") !== "") {
                    $('#fCompany').val(Common.Helper.getUrlParameter("strCompanyId")).trigger('change');
                }

                Control.AjaxDoneCount += 1;
                if (Control.AjaxDoneCount==4) {//mastiin ajax company,regional,operator & product done.
                    $("#btSearch").trigger('click');
                    Control.AjaxDoneCount = 0;
                }
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

                if (Common.Helper.getUrlParameter("strRegionName") !== undefined && Common.Helper.getUrlParameter("strRegionName") !== "") {
                    $('#fRegional').val(Common.Helper.getUrlParameter("strRegionName")).trigger('change');
                }

                Control.AjaxDoneCount += 1;
                if (Control.AjaxDoneCount == 4) {//mastiin ajax company,regional,operator & product done.
                    $("#btSearch").trigger('click');
                    Control.AjaxDoneCount = 0;
                }
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

                if (Common.Helper.getUrlParameter("strOperatorId") !== undefined && Common.Helper.getUrlParameter("strOperatorId") !== "") {
                    $('#fOperator').val(Common.Helper.getUrlParameter("strOperatorId")).trigger('change');
                }

                Control.AjaxDoneCount += 1;
                if (Control.AjaxDoneCount == 4) {//mastiin ajax company,regional,operator & product done.
                    $("#btSearch").trigger('click');
                    Control.AjaxDoneCount = 0;
                }
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

                if (Common.Helper.getUrlParameter("strProduct") !== undefined && Common.Helper.getUrlParameter("strProduct") !== "") {
                    $('#fProduct').val(Common.Helper.getUrlParameter("strProduct")).trigger('change');
                }

                Control.AjaxDoneCount += 1;
                if (Control.AjaxDoneCount == 4) {//mastiin ajax company,regional,operator & product done.
                    $("#btSearch").trigger('click');
                    Control.AjaxDoneCount = 0;
                }
            }
            $(id).select2({ placeholder: "Select Product", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    Reset: function () {
        $('#fYear').val(null).trigger('change');
        $('#fCompany').val(null).trigger('change');
        $('#fRegional').val(null).trigger('change');
        $('#fOperator').val(null).trigger('change');
        $('#fProduct').val(null).trigger('change');
    },
    AjaxDoneCount: 0
}


var Tables = {
    ReportSummary: function () {
        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();
        var params = {
            fAccount     : $('#fAccount').val(),
            fYear        : $('#fYear').val(),
            fCompanyId   : $('#fCompany').val(),
            fRegionalName: $('#fRegional').val(),
            fOperatorId  : $('#fOperator').val(),
            fProduct     : $('#fProduct').val(),
            fMonth       : Common.Helper.getUrlParameter("intMonth"),
            fGroupBy     : Common.Helper.getUrlParameter("strGroupBy"),
            fGroupValue  : Common.Helper.getUrlParameter("strDesc"),
        }

        var tblList = $("#tblSonumbList").DataTable({
            "deferRender": true,
            "proccessing": true,
            //"serverSide": true,
            "language": {
                "emptyTable": "No data available"
            },
            "ajax": {
                "url": "/api/RevenueSystem/GetSoNumberList",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            "filter": false,
            "lengthMenu": [[10, 25, 30, 50, 100], ['10', '25', '50', '100']],
            "destroy": true,
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
            "columns": [
                {
                    data: null
                },
                {
                    data: "SoNumber",
                    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html("<a class='rowSoNumber' data-sonumber='" + oData.SoNumber + "' data-siteid='" + oData.SiteID +
                            "' data-sitename='" + oData.SiteName + "' data-customerid='" + oData.CustomerID + "' data-productname='" + oData.ProductName +
                            "' data-regional='"+ oData.RegionName +"' data-status='"+ oData.Status +"' data-stipsiro='"+ oData.StipSiro +"' href='#'>" + oData.SoNumber + "</a>");
                    }
                },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerID" },
                { data: "RegionName" },
                { data: "ProductName" },
                { data: "Status" },
                { data: "StipSiro" },
                { data: "StipID" },
                { data: "StipCategory" },
                {
                    data: "TotalAccrued",
                    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html(Common.Format.CommaSeparation(oData.TotalAccrued));
                    }
                },
                {
                    data: "TotalUnearned",
                    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html(Common.Format.CommaSeparation(oData.TotalUnearned));
                    }
                },
                {
                    data: "AmountRevenue",
                    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html(Common.Format.CommaSeparation(oData.AmountRevenue));
                    }
                }
            ],
            "columnDefs": [
                { "targets": [0], "orderable": false },
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function (result) {
                if (result.json != undefined && result.json.data != undefined) {
                    if (Common.CheckError.List(this.fnGetData())) {
                        $(".panelSearchResult").fadeIn(1000);
                    }
                    l.stop(); App.unblockUI('.panelSearchResult');

                    //Link SoNumber klik
                    $('.rowSoNumber').click(function (e) {

                        var sonumb       = this.getAttribute('data-sonumber');
                        var siteID       = this.getAttribute('data-siteid');
                        var siteName     = this.getAttribute('data-sitename');
                        var customerID   = this.getAttribute('data-customerid');
                        var regional     = this.getAttribute('data-regional');
                        var productName  = this.getAttribute('data-productname');
                        var status       = this.getAttribute('data-status');
                        var stipSiro       = this.getAttribute('data-stipsiro');
                        var strUrl = "/RevenueSystem/SoNumberDetail?strSoNumber=" + sonumb + "&strSiteID=" + siteID + "&strSiteName=" + siteName + "&strCustomerID=" + customerID +
                            "&strRegional=" + regional + "&strProductName=" + productName + "&strStatus=" + status + "&intStipSiro="+ stipSiro;
                        window.open(strUrl, "_blank");
                    })
                }
            },
            "order": [1, "asc"],
            "scrollCollapse": true,
        });//end of DataTables

        //RowNumber No.
        tblList.on('draw.dt', function () {
            var PageInfo = $('#tblSonumbList').DataTable().page.info();
            tblList.column(0, { page: 'current' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1 + PageInfo.start;
            });
        });
    },

    Export: function () {
        var fAccount = $('#fAccount').val();
        var fYear = $('#fYear').val();
        var fCompanyId = $('#fCompany').val();
        var fRegionalName = $('#fRegional').val();
        var fOperatorId = $('#fOperator').val();
        var fProduct = $('#fProduct').val();
       
        var qGroubBy = Common.Helper.getUrlParameter("strGroupBy");
        var qMonth = Common.Helper.getUrlParameter("intMonth");
        var qDesc = Common.Helper.getUrlParameter("strDesc");
        window.location.href = "/RevenueSystem/SoNumberList/Export?strGroupBy=" + qGroubBy + "&strAccount=" + fAccount + "&intYear=" + fYear + "&strCompanyId=" + fCompanyId + "&strRegionName=" +
            fRegionalName + "&strOperatorId=" + fOperatorId + "&strProduct=" + fProduct + "&strDesc=" + qDesc + "&intMonth=" + qMonth;
    },
}