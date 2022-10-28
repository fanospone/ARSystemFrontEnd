var bitAmountModeFrc = false;

jQuery(document).ready(function () {
    TblForecast.Init();
});

var TblForecast = {
    Init: function () {
        TblForecast.Search();

        $("#btnFrcExport").unbind().click(function () {
            TblForecast.Export();
        })

        $('#chkFrcAmount').on('switchChange.bootstrapSwitch', function (event, state) {
            if (state) {
                bitAmountModeFrc = false;
                TblForecast.Search();

            }
            else {
                bitAmountModeFrc = true;
                TblForecast.Search();
            }
        });

        $("#tblForecast tbody").on("click", ".btDetail", function (e) {
            var table = $("#tblForecast").DataTable();
            var rowdata = table.row($(this).parents("tr")).data();
            DataSelected = rowdata;
            DataSelected.SelectedData = $(this).data('selection');
            $("#panelDetail").fadeIn();
            $("#panelHeader").fadeOut();
            ForecastDetail.Init();
        });

        $("#tblForecast tfoot").on("click", ".btDetail", function (e) {
            DataSelected.GroupSumID = 0;
            DataSelected.SelectedData = $(this).data('selection');
            $("#panelDetail").fadeIn();
            $("#panelHeader").fadeOut();
            ForecastDetail.Init();
        });
    },
    
    Search: function () {
        App.blockUI({ target: "#tblForecast", animate: !0 });
        var tblForecast = $("#tblForecast").DataTable({
            "orderCellsTop": false,
            "ordering": false,
            "proccessing": true,
            "serverSide": true,
            "info": false,
            "paging": false,
            "footer": true,
            "language": {
                "emptyTable": "No data available in table",
            },

            "ajax": {
                "url": "/api/dashboardBAUK/forecast",
                "type": "POST",
                "data": {
                    CompanyIDs: $("#ddlBAUKCompany").val(),
                    CustomerIDs: $("#ddlBAUKCustomer").val(),
                    STIPIDs: $("#ddlBAUKCategory").val(),
                    ProductIDs: $("#ddlBAUKProduct").val(),
                    GroupBy: $("#ddlBAUKGrouping").val(),
                    Month: $("#ddlBAUKMonth").val(),
                    Year: $("#ddlBAUKYear").val(),
                    AmountMode: bitAmountModeFrc,
                },
            },

            "buttons": [{
                footer: true
            }],
            "filter": false,
            "destroy": true,
            "columns": [
                {
                    data: "GroupSum",
                    mRender: function (data, type, full) {
                        return full.GroupSum;
                    }
                },
                {
                    mRender: function (data, type, full) {
                        if (bitAmountModeFrc)
                            return "<a data-selection='All' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(full.AmountTotOutstanding) + "</a>";
                        else
                            return "<a data-selection='All' class='btDetail'>" + full.TotOutstanding + "</a>";
                    }
                },
                {
                    mRender: function (data, type, full) {
                        if (bitAmountModeFrc)
                            return "<a data-selection='W1' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(full.AmountTotW1) + "</a>";
                        else
                            return "<a data-selection='W1' class='btDetail'>" + full.TotW1 + "</a>";
                    }
                },
                {
                    mRender: function (data, type, full) {
                        if (bitAmountModeFrc)
                            return "<a data-selection='W2' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(full.AmountTotW2) + "</a>";
                        else
                            return "<a data-selection='W2' class='btDetail'>" + full.TotW2 + "</a>";
                    }
                },
                {
                    mRender: function (data, type, full) {
                        if (bitAmountModeFrc)
                            return "<a data-selection='W3' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(full.AmountTotW3) + "</a>";
                        else
                            return "<a data-selection='W3' class='btDetail'>" + full.TotW3 + "</a>";
                    }
                },
                {
                    mRender: function (data, type, full) {
                        if (bitAmountModeFrc)
                            return "<a data-selection='W4' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(full.AmountTotW4) + "</a>";
                        else
                            return "<a data-selection='W4' class='btDetail'>" + full.TotW4 + "</a>";
                    }
                },
                {
                    mRender: function (data, type, full) {
                        if (bitAmountModeFrc)
                            return "<a data-selection='W5' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(full.AmountTotW5) + "</a>";
                        else
                            return "<a data-selection='W5' class='btDetail'>" + full.TotW5 + "</a>";
                    }
                },
                {
                    mRender: function (data, type, full) {
                        if (bitAmountModeFrc)
                            return "<a data-selection='M1' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(full.AmountTotM1) + "</a>";
                        else
                            return "<a data-selection='M1' class='btDetail'>" + full.TotM1 + "</a>";
                    }
                },
                {
                    mRender: function (data, type, full) {
                        if (bitAmountModeFrc)
                            return "<a data-selection='M2' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(full.AmountTotM2) + "</a>";
                        else
                            return "<a data-selection='M2' class='btDetail'>" + full.TotM2 + "</a>";
                    }
                },
                {
                    mRender: function (data, type, full) {
                        if (bitAmountModeFrc)
                            return "<a data-selection='M3' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(full.AmountTotM3) + "</a>";
                        else
                            return "<a data-selection='M3' class='btDetail'>" + full.TotM3 + "</a>";
                    }
                },
                {
                    mRender: function (data, type, full) {
                        if (bitAmountModeFrc)
                            return "<a data-selection='M4' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(full.AmountTotM4) + "</a>";
                        else
                            return "<a data-selection='M4' class='btDetail'>" + full.TotM4 + "</a>";
                    }
                },
                {
                    mRender: function (data, type, full) {
                        if (bitAmountModeFrc)
                            return "<a data-selection='M5' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(full.AmountTotM5) + "</a>";
                        else
                            return "<a data-selection='M5' class='btDetail'>" + full.TotM5 + "</a>";
                    }
                },
                {
                    mRender: function (data, type, full) {
                        if (bitAmountModeFrc)
                            return "<a data-selection='NA' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(full.AmountTotNA) + "</a>";
                        else
                            return "<a data-selection='NA' class='btDetail'>" + full.TotNA + "</a>";
                    }
                },
            ],
            "columnDefs": [
                { "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12], "className": "dt-center" }
            ],
            "fnDrawCallback": function () {
                console.log(tblForecast.data);
                Common.CheckError.List(tblForecast.data());
                App.unblockUI("#tblForecast");
            },
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;

                intOut = 0;
                intW1 = 0;
                intW2 = 0;
                intW3 = 0;
                intW4 = 0;
                intW5 = 0;
                intM1 = 0;
                intM2 = 0;
                intM3 = 0;
                intM4 = 0;
                intM5 = 0;
                intNA = 0;

                var oTable = $('#tblForecast').DataTable();
                oTable.rows('').every(function (rowIdx, tableLoop, rowLoop) {
                    intOut += bitAmountModeFrc ? this.data().AmountTotOutstanding : this.data().TotOutstanding;
                    intW1 += bitAmountModeFrc ? this.data().AmountTotW1 : this.data().TotW1;
                    intW2 += bitAmountModeFrc ? this.data().AmountTotW2 : this.data().TotW2;
                    intW3 += bitAmountModeFrc ? this.data().AmountTotW3 : this.data().TotW3;
                    intW4 += bitAmountModeFrc ? this.data().AmountTotW4 : this.data().TotW4;
                    intW5 += bitAmountModeFrc ? this.data().AmountTotW5 : this.data().TotW5;
                    intM1 += bitAmountModeFrc ? this.data().AmountTotM1 : this.data().TotM1;
                    intM2 += bitAmountModeFrc ? this.data().AmountTotM2 : this.data().TotM2;
                    intM3 += bitAmountModeFrc ? this.data().AmountTotM3 : this.data().TotM3;
                    intM4 += bitAmountModeFrc ? this.data().AmountTotM4 : this.data().TotM4;
                    intM5 += bitAmountModeFrc ? this.data().AmountTotM5 : this.data().TotM5;
                    intNA += bitAmountModeFrc ? this.data().AmountTotNA : this.data().TotNA;
                });

                if (bitAmountModeFrc) {
                    $(api.column(1).footer()).html("<a data-selection='All' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(intOut) + "</a>");
                    $(api.column(2).footer()).html("<a data-selection='W1' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(intW1) + "</a>");
                    $(api.column(3).footer()).html("<a data-selection='W2' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(intW2) + "</a>");
                    $(api.column(4).footer()).html("<a data-selection='W3' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(intW3) + "</a>");
                    $(api.column(5).footer()).html("<a data-selection='W4' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(intW4) + "</a>");
                    $(api.column(6).footer()).html("<a data-selection='W5' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(intW5) + "</a>");
                    $(api.column(7).footer()).html("<a data-selection='M1' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(intM1) + "</a>");
                    $(api.column(8).footer()).html("<a data-selection='M2' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(intM2) + "</a>");
                    $(api.column(9).footer()).html("<a data-selection='M3' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(intM3) + "</a>");
                    $(api.column(10).footer()).html("<a data-selection='M4' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(intM4) + "</a>");
                    $(api.column(11).footer()).html("<a data-selection='M5' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(intM5) + "</a>");
                    $(api.column(12).footer()).html("<a data-selection='NA' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(intNA) + "</a>");
                }
                else {
                    $(api.column(1).footer()).html("<a data-selection='All' class='btDetail'>" + intOut + "</a>");
                    $(api.column(2).footer()).html("<a data-selection='W1' class='btDetail'>" + intW1 + "</a>");
                    $(api.column(3).footer()).html("<a data-selection='W2' class='btDetail'>" + intW2 + "</a>");
                    $(api.column(4).footer()).html("<a data-selection='W3' class='btDetail'>" + intW3 + "</a>");
                    $(api.column(5).footer()).html("<a data-selection='W4' class='btDetail'>" + intW4 + "</a>");
                    $(api.column(6).footer()).html("<a data-selection='W5' class='btDetail'>" + intW5 + "</a>");
                    $(api.column(7).footer()).html("<a data-selection='M1' class='btDetail'>" + intM1 + "</a>");
                    $(api.column(8).footer()).html("<a data-selection='M2' class='btDetail'>" + intM2 + "</a>");
                    $(api.column(9).footer()).html("<a data-selection='M3' class='btDetail'>" + intM3 + "</a>");
                    $(api.column(10).footer()).html("<a data-selection='M4' class='btDetail'>" + intM4 + "</a>");
                    $(api.column(11).footer()).html("<a data-selection='M5' class='btDetail'>" + intM5 + "</a>");
                    $(api.column(12).footer()).html("<a data-selection='NA' class='btDetail'>" + intNA + "</a>");
                }
            },

        });
    },

    Export: function () {
        var arr = $("#ddlBAUKMonth").val();
        var strCompany = $("#ddlBAUKCompany").val() == null ? "" : $("#ddlBAUKCompany").val().join("_");
        var strCustomer = $("#ddlBAUKCustomer").val() == null ? "" : $("#ddlBAUKCustomer").val().join("_");
        var strSTIP = $("#ddlBAUKCategory").val() == null ? "" : $("#ddlBAUKCategory").val().join("_");
        var strProduct = $("#ddlBAUKProduct").val() == null ? "" : $("#ddlBAUKProduct").val().join("_");
        var intMonth = arr[0];
        var intYear = $("#ddlBAUKYear").val();
        var strGroupBy = $("#ddlBAUKGrouping").val();
        var bitAmount = bitAmountModeFrc;

        window.location.href = "/Dashboard/BAUK/Forecast/Export?intMonth=" + intMonth + "&intYear=" + intYear + "&strGroupBy=" + strGroupBy
            + "&strCompany=" + strCompany + "&strCustomer=" + strCustomer + "&strSTIP=" + strSTIP + "&strProduct=" + strProduct + "&bitAmount=" + bitAmount;
    },
}

var ForecastDetail = {
    Init: function () {
        this.Load();
        $(".btnSearch").unbind().click(function () {
            var id = $(this).data("id");
            ForecastDetail.Search($("#searchInput-" + id));
        });

        $('#tblDetail input.tbxSearch').keypress(function (e) {
            if (e.which == 13)
                ForecastDetail.Search(this);
        });
    },
    Load: function () {
        App.blockUI({ target: "#tblDetail", animate: !0 });
        var tblDetail = $("#tblDetail").DataTable({
            "orderCellsTop": true,
            "filter": true,
            "destroy": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "buttons": [
                { text: 'Excel', className: 'btn green btn-outline', action: function (e, dt, node, config) { ForecastDetail.Export() } },
            ],
            "order": [[0, 'asc']],
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "ajax": {
                "url": "/api/dashboardBAUK/forecast/detail",
                "type": "POST",
                "datatype": "json",
                "data": {
                    CompanyIDs: $("#ddlBAUKCompany").val(),
                    CustomerIDs: $("#ddlBAUKGrouping").val() == "Customer" ? DataSelected.GroupSumID : $("#ddlBAUKCustomer").val(),
                    STIPIDs: $("#ddlBAUKGrouping").val() == "STIP" ? DataSelected.GroupSumID : $("#ddlBAUKCategory").val(),
                    ProductIDs: $("#ddlBAUKProduct").val(),
                    Month: $("#ddlBAUKMonth").val(),
                    Year: $("#ddlBAUKYear").val(),
                    SelectedData: DataSelected.SelectedData,
                },
            },
            "columns": [
                {
                    data: "SONumber"
                },
                {
                    data: "CustomerName"
                },
                {
                    data: "SiteID"
                },
                {
                    data: "SiteName"
                },
                {
                    data: "CustomerSiteID"
                },
                {
                    data: "CustomerSiteName"
                },
                {
                    data: "Company"
                },
                {
                    data: "STIPCode"
                },
                {
                    data: "Product"
                },
                {
                    data: "RegionName"
                },
                {
                    data: "ProvinceName"
                },
                {
                    data: "ResidenceName"
                },
                {
                    data: "STIPAmount",
                    render: function (data, type, row, meta) {
                        return "Rp " + Common.Format.CommaSeparation(row.STIPAmount);
                    }
                },
                {
                    data: "LeadTime",
                },
                {
                    data: "BAUKFirstSubmitDate",
                    render: function (data, type, row, meta) {
                        var date = new Date(Date.parse(data));
                        return (data != null) ? moment(date).format('DD MMM YYYY') : "-";
                    }
                },
                {
                    data: "BAUKLastSubmitDate",
                    render: function (data, type, row, meta) {
                        var date = new Date(Date.parse(data));
                        return (data != null) ? moment(date).format('DD MMM YYYY') : "-";
                    }
                },
                {
                    data: "BAUKApprovalDate",
                    render: function (data, type, row, meta) {
                        var date = new Date(Date.parse(data));
                        return (data != null) ? moment(date).format('DD MMM YYYY') : "-";
                    }
                },
                {
                    data: "BAUKForecastDate",
                    render: function (data, type, row, meta) {
                        var date = new Date(Date.parse(data));
                        return (data != null) ? moment(date).format('DD MMM YYYY') : "-";
                    }
                },
                {
                    data: "BAUKStatus"
                },
                {
                    data: "RFIDoneDate",
                    render: function (data, type, row, meta) {
                        var date = new Date(Date.parse(data));
                        return (data != null) ? moment(date).format('DD MMM YYYY') : "-";
                    }
                }
            ],
            "columnDefs": [
                { "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19], "className": "dt-center" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                App.unblockUI("#tblDetail");
                $("#tblDetail_filter").hide();
            }

        });
    },
    Summary: function () {
        return $.ajax({
            url: "/api/dashboardBAUK/forecast/detail",
            type: "POST",
            dataType: "json",
            data: {
            },
            cache: false,
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    Search: function (elem) {
        var index = $(elem).data("index");
        var query = $(elem).val();
        var table = $("#tblDetail").DataTable();
        console.log(query);
        table.columns(index).search(query).draw();
    },
    Export: function () {
        var arr = $("#ddlBAUKMonth").val();

        var strCompany = $("#ddlBAUKCompany").val() == null ? "" : $("#ddlBAUKCompany").val().join("','");
        var strCustomer = $("#ddlBAUKCustomer").val() == null ? "" : $("#ddlBAUKCustomer").val().join("','");
        var strSTIP = $("#ddlBAUKCategory").val() == null ? "" : $("#ddlBAUKCategory").val().join(",");
        var strProduct = $("#ddlBAUKProduct").val() == null ? "" : $("#ddlBAUKProduct").val().join(",");
        var strMonth = $("#ddlBAUKMonth").val() == null ? "" : $("#ddlBAUKMonth").val().join(",");
        var intMonth = arr[0];
        var intYear = $("#ddlBAUKYear").val();

        var Params = {
            CompanyIDs: "'" + strCompany + "'",
            CustomerIDs: $("#ddlBAUKGrouping").val() == "Customer" ? "'" + DataSelected.GroupSumID + "'" : "'" + strCustomer + "'",
            STIPIDs: $("#ddlBAUKGrouping").val() == "STIP" ? DataSelected.GroupSumID : strSTIP,
            ProductIDs: strProduct,
            Months: strMonth,
            Month: intMonth,
            Year: $("#ddlBAUKYear").val(),
            SelectedData: DataSelected.SelectedData,
            TabMode: "Forecast",
        }
        Cookies.set("ExportParams", JSON.stringify(Params), { expires: 1 });

        window.location.href = "/Dashboard/BAUK/Detail/Export";
    },
}