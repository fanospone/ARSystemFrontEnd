
var tblReportRevenueDesc = '#tblReportRevenueDesc';
var paramSearch = {};

var slsCompanyId = '#slsCompanyId';
var slsRegionId = '#slsRegionId';
var slsCustomerId = '#slsCustomerId';
var slsDepartment = '#slsDepartment';
var slsRegionId = '#slsRegionId';
var slsCustomerId = '#slsCustomerId';
var slsDepartment = '#slsDepartment';
var sStartRFIPeriod = '#sStartRFIPeriod';
var sEndRFIPeriod = '#sEndRFIPeriod';
var sStartSLDPeriod = '#sStartSLDPeriod';
var sEndSLDPeriod = '#sEndSLDPeriod';
var sSoNumber = '#sSoNumber';
var sSiteId = '#sSiteId';
var sSiteName = '#sSiteName';



jQuery(document).ready(function () {
    Control.Buttons();
    Control.DatePicker();
    BindData.Table.Init();
    BindData.Table.Load();
    BindData.SetParam();
    BindData.DDL.Customer();
    BindData.DDL.Company();
    BindData.DDL.Regional();
    BindData.DDL.Department();


});

var Control = {
    Buttons: function () {
        $('.btSearch').unbind().click(function () {
            
            if (Control.ValidateSearch()) {
                BindData.SetParam();
                BindData.Table.Load();
            }

        });

        $('#btReset').unbind().click(function () {
            $(sStartRFIPeriod).val('');
            $(sEndRFIPeriod).val('');
            $(sStartSLDPeriod).val('');
            $(sEndSLDPeriod).val('');
            $(slsDepartment).val('').trigger('change');
            $(slsCustomerId).val('').trigger('change');
            $(slsCompanyId).val('').trigger('change');
            $(slsRegionId).val('').trigger('change');
        });

    },
    Event: function () {
        $(sEndRFIPeriod).unbind().change(function () {
            $(sEndRFIPeriod).datepicker({
                format: "dd-M-yyyy",
                autoclose: true,
                minDate: new Date(2014, 10, 30),

            });
        })
    },
    DatePicker: function () {

        $('.datepicker').datepicker({
            format: "dd M yyyy",
            autoclose: true
        });

    },

    ValidateSearch: function () {

        var result = true;
        var startRFI = $(sStartRFIPeriod).val();
        var endtRFI = $(sEndRFIPeriod).val();
        var startSLD = $(sStartSLDPeriod).val();
        var endtSLD = $(sEndSLDPeriod).val();

        var compareDates = function (start, end) {
            start = new Date(start);
            end = new Date(end);

            if (start > end)
                return false;
            else if (start < end)
                return true;
        }

        if (startRFI != "" && endtRFI != "") {
            if (compareDates(startRFI, endtRFI) == false) {
                Common.Alert.Warning('End  Period RFI  must be large then Start!');
                result = false;
            }
        }

        if (startSLD != "" && endtSLD != "") {
            if (compareDates(startSLD, endtSLD) == false) {
                Common.Alert.Warning('End  Period SLD must be large then Start!');
                result = false;
            }
        }

        return result;
    }
}

var BindData = {
    SetParam: function () {
        paramSearch = {
            RfiStartDate: $(sStartRFIPeriod).val(),
            RfiEndDate: $(sEndRFIPeriod).val(),
            StartSLDDate: $(sStartSLDPeriod).val(),
            EndSLDDate: $(sEndSLDPeriod).val(),
            DepartmentName: $(slsDepartment).val(),
            CustomerId: $(slsCustomerId).val(),
            CompanyId: $(slsCompanyId).val(),
            RegionName: $(slsRegionId + ' option:selected').text(),
            RegionId: $(slsRegionId).val(),
            SoNumber: $(sSoNumber).val(),
            SiteId: $(sSiteId).val(),
            SiteName: $(sSiteName).val(),
        };
    },

    GetData: function (_url) {
        var result = [];
        $.ajax({
            url: _url,
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                result = data;
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
        return result;
    },

    DDL: {
        Company: function () {
            var data = BindData.GetData('/api/MstDataSource/Company');
            $(slsCompanyId).html("<option></option>")
            $.each(data, function (i, item) {
                $(slsCompanyId).append("<option value='" + $.trim(item.CompanyId) + "'>" + item.Company + "</option>");
            })
            $(slsCompanyId).select2({ placeholder: "Select Company", width: null });

        },
        Customer: function () {
            var data = BindData.GetData('/api/MstDataSource/Operator');
            $(slsCustomerId).html("<option></option>")
            $.each(data, function (i, item) {
                $(slsCustomerId).append("<option value='" + $.trim(item.OperatorId) + "'>" + item.Operator + "</option>");
            })
            $(slsCustomerId).select2({ placeholder: "Select Customer", width: null });
        },
        Regional: function () {
            var data = BindData.GetData('/api/MstDataSource/Regional');
            $(slsRegionId).html("<option></option>")
            $.each(data, function (i, item) {
                $(slsRegionId).append("<option value='" + $.trim(item.RegionalId) + "'>" + item.RegionalName + "</option>");
            })
            $(slsRegionId).select2({ placeholder: "Select Region", width: null });
        },
        Department: function () {
            var data = BindData.GetData('/api/MstDataSource/Department');
            $(slsDepartment).html("<option></option>")
            $.each(data, function (i, item) {
                $(slsDepartment).append("<option value='" + $.trim(item.DepartmentCode) + "'>" + item.DepartmentName + "</option>");
            })
            $(slsDepartment).select2({ placeholder: "Select Department", width: null });
        }
    },

    Table: {
        Init: function () {
            $(tblReportRevenueDesc).dataTable({
                "filter": false,
                "destroy": true,
                "data": []
            });

        },
        Load: function () {

            var l = Ladda.create(document.querySelector(tblReportRevenueDesc));
            l.start();

            $(tblReportRevenueDesc).DataTable({
                "deferRender": false,
                "proccessing": false,
                "serverSide": true,
                "language": {
                    "emptyTable": "No data available in table"
                },
                "ajax": {
                    "url": "/Api/RevenueSystem/RevenueReportDesc",
                    "type": "POST",
                    "datatype": "json",
                    "data": paramSearch,
                    // "async": false
                },
                buttons: [
                    { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                    {
                        text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                            var l = Ladda.create(document.querySelector(".yellow"));
                            l.start();
                            BindData.Table.Export()
                            l.stop();
                        }
                    },

                ],
                "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                "filter": false,
                "order": false,
                "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
                "destroy": true,
                "columns": [
                    { data: "RowIndex" }, // 1
                    {
                        data: "SoNumber", render: function (data) {
                            return "<div class='linkDtl'><i class='fa fa-cursor'></i><a class='linkDtl' title='Detail'>" + data + "</a></div>";

                        }
                    }, //2

                    { data: "SiteId" }, //3
                    { data: "SiteName" }, //4
                    { data: "StipCategory" }, //5
                    { data: "RegionName" }, //6
                    { data: "UserNumber" }, //7
                    { data: "CustomerId" }, //8
                    { data: "SiteStatus" }, //9
                    { data: "ContractStatus" }, //10
                    { data: "DocumentProcess" }, //11
                    { data: "CompanyId" }, //12
                    {
                        data: "RfiDate", render: function (data) {
                            return Common.Format.ConvertJSONDateTime(data);
                        }
                    }, //13
                    {
                        data: "SLDDate", render: function (data) {
                            return Common.Format.ConvertJSONDateTime(data);
                        }
                    }, //14
                    {
                        data: "BapsDate", render: function (data) {
                            return Common.Format.ConvertJSONDateTime(data);
                        }
                    }, //15
                    {
                        data: "RentalStartDate", render: function (data) {
                            return Common.Format.ConvertJSONDateTime(data);
                        }
                    }, //16
                    {
                        data: "RentalEndDate", render: function (data) {
                            return Common.Format.ConvertJSONDateTime(data);
                        }
                    }, //17
                    { data: "TenantType" }, //18
                    {
                        data: "RFInvoice", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    }, //19
                    {
                        data: "MFInvoice", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    }, //20
                    {
                        data: "PriceBaseOnML", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    }, //21
                    { data: "PriceBaseOnInvoice" }, //22
                    { data: "LastInvoicePeriod" }, //23
                    { data: "ContractStartDate" }, //24
                    { data: "ContractEndDate" }, //25
                    {
                        data: "BalanceAccrued", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    }, //26
                    { data: "AgingDays" }, //27
                ],
                "columnDefs": [{ "targets": [0, 12, 13, 14, 15, 16, 23, 24], "class": "text-center" },
                { "targets": [18, 19, 20, 21, 22, 25, 26], "class": "text-right" }],
                "fnDrawCallback": function () {
                    l.stop();
                },
                "scrollY": false, /* Enable vertical scroll to allow fixed columns */
                "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
                "scrollCollapse": true,
                //"fixedColumns": {
                //    leftColumns: 4 /* Set the 2 most left columns as fixed columns */
                //},
            });


            $(tblReportRevenueDesc + ' tbody').unbind();
            $(tblReportRevenueDesc + ' tbody').on("click", ".linkDtl", function (e) {


                var table = $(tblReportRevenueDesc).DataTable();
                var row = table.row($(this).parents('tr')).data();

                var strUrl = "/RevenueSystem/SoNumberDetail?strSoNumber=" + row.SoNumber + "&strSiteID=" + row.SiteId + "&strSiteName=" + row.SiteName + "&strCustomerID=" + row.CustomerId + "&strRegional=" + row.RegionName + "&strProductName=" + row.TenantType + "&strStatus=" + row.SiteStatus + "&intStipSiro=" + row.StipSiro;

                window.open(strUrl, "_blank");
            });

        },
        Export: function () {
            window.location.href = "/RevenueSystem/RevenueDescExport?" + $.param(paramSearch);
        }
    },


}