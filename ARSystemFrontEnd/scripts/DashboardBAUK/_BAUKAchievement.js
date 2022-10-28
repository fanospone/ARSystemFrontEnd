jQuery(document).ready(function () {
    TblAchievement.Init();
});

var TblAchievement = {
    Init: function () {
        TblAchievement.Search();

        $("#btnAchExport").unbind().click(function () {
            TblAchievement.Export();
        })

        $("#tblAchievement tbody").on("click", ".btDetail", function (e) {
            var table = $("#tblAchievement").DataTable();
            var rowdata = table.row($(this).parents("tr")).data();
            DataSelected = rowdata;
            DataSelected.Month = $(this).data('selection');
            $("#panelDetail").fadeIn();
            $("#panelHeader").fadeOut();
            AchievementDetail.Init();
        });

        $("#tblAchievement tfoot").on("click", ".btDetail", function (e) {
            DataSelected.GroupSumID = 0;
            DataSelected.Month = $(this).data('selection');
            $("#panelDetail").fadeIn();
            $("#panelHeader").fadeOut();
            AchievementDetail.Init();
        });
    },
    
    Search: function () {
        App.blockUI({ target: "#tblAchievement", animate: !0 });
        var tblAchievement = $("#tblAchievement").DataTable({
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
                "url": "/api/dashboardBAUK/achievement",
                "type": "POST",
                "data": {
                    CompanyIDs: $("#ddlBAUKCompany").val(),
                    CustomerIDs: $("#ddlBAUKCustomer").val(),
                    STIPIDs: $("#ddlBAUKCategory").val(),
                    ProductIDs: $("#ddlBAUKProduct").val(),
                    GroupBy: $("#ddlBAUKGrouping").val(),
                    Month: $("#ddlBAUKMonth").val(),
                    Year: $("#ddlBAUKYear").val()
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
                        return "<b>" + full.GroupSum + "</b>";
                    }
                },
                {
                    data: "TotM1",
                    mRender: function (data, type, full) {
                        return "<a data-selection='1' class='btDetail'>" + full.TotM1 + "</a>";
                    }
                },
                {
                    data: "TotLTM1",
                    mRender: function (data, type, full) {
                        if (full.TotM1 == 0)
                            return 0;
                        else
                            return (full.TotLTM1 / full.TotM1).toFixed(0);
                    }
                },
                {
                    data: "TotM2",
                    mRender: function (data, type, full) {
                        return "<a data-selection='2' class='btDetail'>" + full.TotM2 + "</a>";
                    }
                },
                {
                    data: "TotLTM2",
                    mRender: function (data, type, full) {
                        if (full.TotM2 == 0)
                            return 0;
                        else
                            return (full.TotLTM2 / full.TotM2).toFixed(0);
                    }
                },
                {
                    data: "TotM3",
                    mRender: function (data, type, full) {
                        return "<a data-selection='3' class='btDetail'>" + full.TotM3 + "</a>";
                    }
                },
                {
                    data: "TotLTM3",
                    mRender: function (data, type, full) {
                        if (full.TotM3 == 0)
                            return 0;
                        else
                            return (full.TotLTM3 / full.TotM3).toFixed(0);
                    }
                },
                {
                    data: "TotM4",
                    mRender: function (data, type, full) {
                        return "<a data-selection='4' class='btDetail'>" + full.TotM4 + "</a>";
                    }
                },
                {
                    data: "TotLTM4",
                    mRender: function (data, type, full) {
                        if (full.TotM4 == 0)
                            return 0;
                        else
                            return (full.TotLTM4 / full.TotM4).toFixed(0);
                    }
                },
                {
                    data: "TotM5",
                    mRender: function (data, type, full) {
                        return "<a data-selection='5' class='btDetail'>" + full.TotM5 + "</a>";
                    }
                },
                {
                    data: "TotLTM5",
                    mRender: function (data, type, full) {
                        if (full.TotM5 == 0)
                            return 0;
                        else
                            return (full.TotLTM5 / full.TotM5).toFixed(0);
                    }
                },
                {
                    data: "TotM6",
                    mRender: function (data, type, full) {
                        return "<a data-selection='6' class='btDetail'>" + full.TotM6 + "</a>";
                    }
                },
                {
                    data: "TotLTM6",
                    mRender: function (data, type, full) {
                        if (full.TotM6 == 0)
                            return 0;
                        else
                            return (full.TotLTM6 / full.TotM6).toFixed(0);
                    }
                },
                {
                    data: "TotM7",
                    mRender: function (data, type, full) {
                        return "<a data-selection='7' class='btDetail'>" + full.TotM7 + "</a>";
                    }
                },
                {
                    data: "TotLTM7",
                    mRender: function (data, type, full) {
                        if (full.TotM7 == 0)
                            return 0;
                        else
                            return (full.TotLTM7 / full.TotM7).toFixed(0);
                    }
                },
                {
                    data: "TotM8",
                    mRender: function (data, type, full) {
                        return "<a data-selection='8' class='btDetail'>" + full.TotM8 + "</a>";
                    }
                },
                {
                    data: "TotLTM8",
                    mRender: function (data, type, full) {
                        if (full.TotM8 == 0)
                            return 0;
                        else
                            return (full.TotLTM8 / full.TotM8).toFixed(0);
                    }
                },
                {
                    data: "TotM9",
                    mRender: function (data, type, full) {
                        return "<a data-selection='9' class='btDetail'>" + full.TotM9 + "</a>";
                    }
                },
                {
                    data: "TotLTM9",
                    mRender: function (data, type, full) {
                        if (full.TotM9 == 0)
                            return 0;
                        else
                            return (full.TotLTM9 / full.TotM9).toFixed(0);
                    }
                },
                {
                    data: "TotM10",
                    mRender: function (data, type, full) {
                        return "<a data-selection='10' class='btDetail'>" + full.TotM10 + "</a>";
                    }
                },
                {
                    data: "TotLTM10",
                    mRender: function (data, type, full) {
                        if (full.TotM10 == 0)
                            return 0;
                        else
                            return (full.TotLTM10 / full.TotM10).toFixed(0);
                    }
                },
                {
                    data: "TotM11",
                    mRender: function (data, type, full) {
                        return "<a data-selection='11' class='btDetail'>" + full.TotM11 + "</a>";
                    }
                },
                {
                    data: "TotLTM11",
                    mRender: function (data, type, full) {
                        if (full.TotM11 == 0)
                            return 0;
                        else
                            return (full.TotLTM11 / full.TotM11).toFixed(0);
                    }
                },
                {
                    data: "TotM12",
                    mRender: function (data, type, full) {
                        return "<a data-selection='12' class='btDetail'>" + full.TotM12 + "</a>";
                    }
                },
                {
                    data: "TotLTM12",
                    mRender: function (data, type, full) {
                        if (full.TotM12 == 0)
                            return 0;
                        else
                            return (full.TotLTM12 / full.TotM12).toFixed(0);
                    }
                },
            ],
            "columnDefs": [
                { "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24], "className": "dt-center" }
            ],
            "fnDrawCallback": function () {
                Common.CheckError.List(tblAchievement.data());
                App.unblockUI("#tblAchievement");
            },
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;

                intM1 = 0;
                intLTM1 = 0;
                intM2 = 0;
                intLTM2 = 0;
                intM3 = 0;
                intLTM3 = 0;
                intM4 = 0;
                intLTM4 = 0;
                intM5 = 0;
                intLTM5 = 0;
                intM6 = 0;
                intLTM6 = 0;
                intM7 = 0;
                intLTM7 = 0;
                intM8 = 0;
                intLTM8 = 0;
                intM9 = 0;
                intLTM9 = 0;
                intM10 = 0;
                intLTM10 = 0;
                intM11 = 0;
                intLTM11 = 0;
                intM12 = 0;
                intLTM12 = 0;

                var oTableReject = $('#tblAchievement').DataTable();
                oTableReject.rows('').every(function (rowIdx, tableLoop, rowLoop) {
                    intM1 += this.data().TotM1;
                    intLTM1 += this.data().TotLTM1;
                    intM2 += this.data().TotM2;
                    intLTM2 += this.data().TotLTM2;
                    intM3 += this.data().TotM3;
                    intLTM3 += this.data().TotLTM3;
                    intM4 += this.data().TotM4;
                    intLTM4 += this.data().TotLTM4;
                    intM5 += this.data().TotM5;
                    intLTM5 += this.data().TotLTM5;
                    intM6 += this.data().TotM6;
                    intLTM6 += this.data().TotLTM6;
                    intM7 += this.data().TotM7;
                    intLTM7 += this.data().TotLTM7;
                    intM8 += this.data().TotM8;
                    intLTM8 += this.data().TotLTM8;
                    intM9 += this.data().TotM9;
                    intLTM9 += this.data().TotLTM9;
                    intM10 += this.data().TotM10;
                    intLTM10 += this.data().TotLTM10;
                    intM11 += this.data().TotM11;
                    intLTM11 += this.data().TotLTM11;
                    intM12 += this.data().TotM12;
                    intLTM12 += this.data().TotLTM12;
                });

                $(api.column(1).footer()).html("<a data-selection='1' class='btDetail'>" + intM1);
                $(api.column(2).footer()).html(intM1 == 0 ? 0 : (intLTM1 / intM1).toFixed(0));
                $(api.column(3).footer()).html("<a data-selection='2' class='btDetail'>" + intM2);
                $(api.column(4).footer()).html(intM2 == 0 ? 0 : (intLTM2 / intM2).toFixed(0));
                $(api.column(5).footer()).html("<a data-selection='3' class='btDetail'>" + intM3);
                $(api.column(6).footer()).html(intM3 == 0 ? 0 : (intLTM3 / intM3).toFixed(0));
                $(api.column(7).footer()).html("<a data-selection='4' class='btDetail'>" + intM4);
                $(api.column(8).footer()).html(intM4 == 0 ? 0 : (intLTM4 / intM4).toFixed(0));
                $(api.column(9).footer()).html("<a data-selection='5' class='btDetail'>" + intM5);
                $(api.column(10).footer()).html(intM5 == 0 ? 0 : (intLTM5 / intM5).toFixed(0));
                $(api.column(11).footer()).html("<a data-selection='6' class='btDetail'>" + intM6);
                $(api.column(12).footer()).html(intM6 == 0 ? 0 : (intLTM6 / intM6).toFixed(0));
                $(api.column(13).footer()).html("<a data-selection='7' class='btDetail'>" + intM7);
                $(api.column(14).footer()).html(intM7 == 0 ? 0 : (intLTM7 / intM7).toFixed(0));
                $(api.column(15).footer()).html("<a data-selection='8' class='btDetail'>" + intM8);
                $(api.column(16).footer()).html(intM8 == 0 ? 0 : (intLTM8 / intM8).toFixed(0));
                $(api.column(17).footer()).html("<a data-selection='9' class='btDetail'>" + intM9);
                $(api.column(18).footer()).html(intM9 == 0 ? 0 : (intLTM9 / intM9).toFixed(0));
                $(api.column(19).footer()).html("<a data-selection='10' class='btDetail'>" + intM10);
                $(api.column(20).footer()).html(intM10 == 0 ? 0 : (intLTM10 / intM10).toFixed(0));
                $(api.column(21).footer()).html("<a data-selection='11' class='btDetail'>" + intM11);
                $(api.column(22).footer()).html(intM11 == 0 ? 0 : (intLTM11 / intM11).toFixed(0));
                $(api.column(23).footer()).html("<a data-selection='12' class='btDetail'>" + intM12);
                $(api.column(24).footer()).html(intM12 == 0 ? 0 : (intLTM12 / intM12).toFixed(0));
            },

        });
    },

    Export: function () {
        var strCompany = $("#ddlBAUKCompany").val() == null ? "" : $("#ddlBAUKCompany").val().join("_");
        var strCustomer = $("#ddlBAUKCustomer").val() == null ? "" : $("#ddlBAUKCustomer").val().join("_");
        var strSTIP = $("#ddlBAUKCategory").val() == null ? "" : $("#ddlBAUKCategory").val().join("_");
        var strProduct = $("#ddlBAUKProduct").val() == null ? "" : $("#ddlBAUKProduct").val().join("_");
        var intYear = $("#ddlBAUKYear").val();
        var strGroupBy = $("#ddlBAUKGrouping").val();

        window.location.href = "/Dashboard/BAUK/Achievement/Export?intYear=" + intYear + "&strGroupBy=" + strGroupBy
            + "&strCompany=" + strCompany + "&strCustomer=" + strCustomer + "&strSTIP=" + strSTIP + "&strProduct=" + strProduct;
    },
}

var AchievementDetail = {
    Init: function () {
        this.Load();
        $(".btnSearch").unbind().click(function () {
            var id = $(this).data("id");
            AchievementDetail.Search($("#searchInput-" + id));
        });

        $('#tblDetail input.tbxSearch').keypress(function (e) {
            if (e.which == 13)
                AchievementDetail.Search(this);
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
                { text: 'Excel', className: 'btn green btn-outline', action: function (e, dt, node, config) { AchievementDetail.Export() } },
            ],
            "order": [[0, 'asc']],
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "ajax": {
                "url": "/api/dashboardBAUK/achievement/detail",
                "type": "POST",
                "datatype": "json",
                "data": {
                    CompanyIDs: $("#ddlBAUKCompany").val(),
                    CustomerIDs: $("#ddlBAUKGrouping").val() == "Customer" ? DataSelected.GroupSumID : $("#ddlBAUKCustomer").val(),
                    STIPIDs: $("#ddlBAUKGrouping").val() == "STIP" ? DataSelected.GroupSumID : $("#ddlBAUKCategory").val(),
                    ProductIDs: $("#ddlBAUKProduct").val(),
                    Month: DataSelected.Month,
                    Year: $("#ddlBAUKYear").val(),
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
        var intMonth = arr[arr.length - 1];
        var intYear = $("#ddlBAUKYear").val();

        var Params = {
            CompanyIDs: "'" + strCompany + "'",
            CustomerIDs: $("#ddlBAUKGrouping").val() == "Customer" ? "'" + DataSelected.GroupSumID + "'" : "'" + strCustomer + "'",
            STIPIDs: $("#ddlBAUKGrouping").val() == "STIP" ? DataSelected.GroupSumID : strSTIP,
            ProductIDs: strProduct,
            Months: strMonth,
            Month: DataSelected.Month,
            Year: $("#ddlBAUKYear").val(),
            SelectedData: DataSelected.SelectedData,
            TabMode: "Achievement",
        }
        Cookies.set("ExportParams", JSON.stringify(Params), { expires: 1 });

        window.location.href = "/Dashboard/BAUK/Detail/Export";
    },
}