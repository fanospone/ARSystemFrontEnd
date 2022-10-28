var bitAmountModeAct = false;

jQuery(document).ready(function () {
    TblActivity.Init();
});

var TblActivity = {
    Init: function () {
        TblActivity.Search();

        $("#btnActExport").unbind().click(function () {
            TblActivity.Export();
        })

        $('#chkActAmount').on('switchChange.bootstrapSwitch', function (event, state) {
            if (state) {
                bitAmountModeAct = false;
                TblActivity.Search();
            }
            else {
                bitAmountModeAct = true;
                TblActivity.Search();
            }
        });

        $("#tblActivity tbody").on("click", ".btDetail", function (e) {
            var table = $("#tblActivity").DataTable();
            var rowdata = table.row($(this).parents("tr")).data();
            DataSelected = rowdata;
            DataSelected.SelectedData = $(this).data('selection');
            $("#panelDetail").fadeIn();
            $("#panelHeader").fadeOut();
            ActivityDetail.Init();
        });

        $("#tblActivity tfoot").on("click", ".btDetail", function (e) {
            DataSelected.GroupSumID = 0;
            DataSelected.SelectedData = $(this).data('selection');
            $("#panelDetail").fadeIn();
            $("#panelHeader").fadeOut();
            ActivityDetail.Init();
        });
    },
    
    Search: function () {
        App.blockUI({ target: "#tblActivity", animate: !0 });
        var tblActivity = $("#tblActivity").DataTable({
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
                "url": "/api/dashboardBAUK/activity",
                "type": "POST",
                "data": {
                    CompanyIDs: $("#ddlBAUKCompany").val(),
                    CustomerIDs: $("#ddlBAUKCustomer").val(),
                    STIPIDs: $("#ddlBAUKCategory").val(),
                    ProductIDs: $("#ddlBAUKProduct").val(),
                    GroupBy: $("#ddlBAUKGrouping").val(),
                    Month: $("#ddlBAUKMonth").val(),
                    Months: $("#ddlBAUKMonth").val(),
                    Year: $("#ddlBAUKYear").val(),
                    AmountMode: bitAmountModeAct,
                },
            },

            "buttons": [{
                footer: true
            }],
            "filter": false,
            "destroy": true,
            "columns": [
                {
                    mRender: function (data, type, full) {
                        return full.GroupSum;
                    }
                },
                {
                    mRender: function (data, type, full) {
                        if (bitAmountModeAct)
                            return "<a data-selection='RFI' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(full.AmountRFIDone) + "</a>";
                        else
                            return "<a data-selection='RFI' class='btDetail'>" + full.RFIDone + "</a>";
                    }
                },
                {
                    mRender: function (data, type, full) {
                        if (bitAmountModeAct)
                            return "<a data-selection='Submit' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(full.AmountBAUKSubmitted) + "</a>";
                        else
                            return "<a data-selection='Submit' class='btDetail'>" + full.BAUKSubmitted + "</a>";
                    }
                },
                {
                    mRender: function (data, type, full) {
                        if (bitAmountModeAct)
                            return "<a data-selection='Approve' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(full.AmountBAUKApproved) + "</a>";
                        else
                            return "<a data-selection='Approve' class='btDetail'>" + full.BAUKApproved + "</a>";
                    }
                },
                {
                    mRender: function (data, type, full) {
                        if (bitAmountModeAct)
                            return "<a data-selection='SuRejectbmit' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(full.AmountBAUKRejected) + "</a>";
                        else
                            return "<a data-selection='Reject' class='btDetail'>" + full.BAUKRejected + "</a>";
                    }
                },
                {
                    mRender: function (data, type, full) {
                        if (bitAmountModeAct)
                            return "<a data-selection='Total' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(full.AmountTotal) + "</a>";
                        else
                            return "<a data-selection='Total' class='btDetail'>" + full.Total + "</a>";
                    }
                },
                {
                    data: "PercentTotal",
                    mRender: function (data, type, full) {
                        return full.PercentTotal.toFixed(2) + " %";
                    }
                },
            ],
            "columnDefs": [
                { "targets": [0, 1, 2, 3, 4, 5, 6], "className": "dt-center" }
            ],
            "fnDrawCallback": function () {
                Common.CheckError.List(tblActivity.data());
                App.unblockUI("#tblActivity");
            },
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;


                intRFI = 0;
                intSubmit = 0;
                intApproved = 0;
                intReject = 0;
                intTotal = 0;
                intPercent = 0;
                intAmRFI = 0.00;
                intAmSubmit = 0.00;
                intAmApproved = 0.00;
                intAmReject = 0.00;
                intAmTotal = 0.00;

                var oTable = $('#tblActivity').DataTable();
                oTable.rows('').every(function (rowIdx, tableLoop, rowLoop) {
                    intRFI += this.data().RFIDone;
                    intSubmit += this.data().BAUKSubmitted;
                    intApproved += this.data().BAUKApproved;
                    intReject += this.data().BAUKRejected;
                    intTotal += this.data().Total;
                    intAmRFI += this.data().AmountRFIDone;
                    intAmSubmit += this.data().AmountBAUKSubmitted;
                    intAmApproved += this.data().AmountBAUKApproved;
                    intAmReject += this.data().AmountBAUKRejected;
                    intAmTotal += this.data().AmountTotal;
                    intPercent += this.data().PercentTotal;
                });

                if (bitAmountModeAct) {
                    prcSubmit = intAmTotal == 0 ? 0 : (intAmSubmit / intAmTotal) * 100;
                    prcApproved = intAmTotal == 0 ? 0 : (intAmApproved / intAmTotal) * 100;
                    prcReject = intAmTotal == 0 ? 0 : (intAmReject / intAmTotal) * 100;

                    $("#totRFI").html("<a data-selection='RFI' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(intAmRFI) + "</a>");
                    $("#totSubmitted").html("<a data-selection='Submit' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(intAmSubmit) + "</a>");
                    $("#totApproved").html("<a data-selection='Approve' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(intAmApproved) + "</a>");
                    $("#totRejected").html("<a data-selection='Reject' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(intAmReject) + "</a>");
                    $("#totAllBAUK").html("<a data-selection='Total' class='btDetail'>" + "Rp " + Common.Format.CommaSeparation(intAmTotal) + "</a>");
                    $("#totPrcAct").html(intPercent + " %");
                    $("#prcSubmitted").html(prcSubmit.toFixed(2) + " %");
                    $("#prcApproved").html(prcApproved.toFixed(2) + " %");
                    $("#prcRejected").html(prcReject.toFixed(2) + " %");
                }
                else {
                    prcSubmit = intTotal == 0 ? 0 : (intSubmit / intTotal) * 100;
                    prcApproved = intTotal == 0 ? 0 : (intApproved / intTotal) * 100;
                    prcReject = intTotal == 0 ? 0 : (intReject / intTotal) * 100;

                    $("#totRFI").html("<a data-selection='RFI' class='btDetail'>" + intRFI + "</a>");
                    $("#totSubmitted").html("<a data-selection='Submit' class='btDetail'>" + intSubmit + "</a>");
                    $("#totApproved").html("<a data-selection='Approve' class='btDetail'>" + intApproved + "</a>");
                    $("#totRejected").html("<a data-selection='Reject' class='btDetail'>" + intReject + "</a>");
                    $("#totAllBAUK").html("<a data-selection='Total' class='btDetail'>" + intTotal.toFixed(0) + "</a>");
                    $("#totPrcAct").html(intPercent.toFixed(2) + " %");
                    $("#prcSubmitted").html(prcSubmit.toFixed(2) + " %");
                    $("#prcApproved").html(prcApproved.toFixed(2) + " %");
                    $("#prcRejected").html(prcReject.toFixed(2) + " %");
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
        var strMonth = $("#ddlBAUKMonth").val() == null ? "" : $("#ddlBAUKMonth").val().join("_");
        var intMonth = arr[arr.length - 1];
        var intYear = $("#ddlBAUKYear").val();
        var strGroupBy = $("#ddlBAUKGrouping").val();
        var bitAmount = bitAmountModeAct;

        window.location.href = "/Dashboard/BAUK/Activity/Export?intMonth=" + intMonth + "&intYear=" + intYear + "&strGroupBy=" + strGroupBy
            + "&strCompany=" + strCompany + "&strCustomer=" + strCustomer + "&strSTIP=" + strSTIP + "&strProduct=" + strProduct + "&strMonth=" + strMonth + "&bitAmount=" + bitAmount;
    },
}

var ActivityDetail = {
    Init: function () {
        this.Load();
        $(".btnSearch").unbind().click(function () {
            var id = $(this).data("id");
            ActivityDetail.Search($("#searchInput-" + id));
        });

        $('#tblDetail input.tbxSearch').keypress(function (e) {
            if (e.which == 13)
                ActivityDetail.Search(this);
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
                { text: 'Excel', className: 'btn green btn-outline', action: function (e, dt, node, config) { ActivityDetail.Export() } },
            ],
            "order": [[0, 'asc']],
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "ajax": {
                "url": "/api/dashboardBAUK/activity/detail",
                "type": "POST",
                "datatype": "json",
                "data": {
                    CompanyIDs: $("#ddlBAUKCompany").val(),
                    CustomerIDs: $("#ddlBAUKGrouping").val() == "Customer" ? DataSelected.GroupSumID : $("#ddlBAUKCustomer").val(),
                    STIPIDs: $("#ddlBAUKGrouping").val() == "STIP" ? DataSelected.GroupSumID : $("#ddlBAUKCategory").val(),
                    ProductIDs: $("#ddlBAUKProduct").val(),
                    Month: $("#ddlBAUKMonth").val(),
                    Months: $("#ddlBAUKMonth").val(),
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
            url: "/api/dashboardBAUK/activity/detail",
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
        var intMonth = arr[arr.length - 1];
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
            TabMode: "Activity",
        }
        Cookies.set("ExportParams", JSON.stringify(Params), { expires: 1 });

        window.location.href = "/Dashboard/BAUK/Detail/Export";
    },
}