jQuery(document).ready(function () {
    TblReject.Init();
});

var TblReject = {
    Init: function () {
        TblReject.Search();

        $("#btnRejExport").unbind().click(function () {
            TblReject.Export();
        })

        $("#tblReject tbody").on("click", ".btDetail", function (e) {
            var table = $("#tblReject").DataTable();
            var rowdata = table.row($(this).parents("tr")).data();
            DataSelected = rowdata;
            DataSelected.SelectedData = $(this).data('selection');
            $("#modal-reject").modal('show');
            RejectDocDetail.Init();
        });
    },

    Search: function () {
        App.blockUI({ target: "#tblReject", animate: !0 });
        var tblReject = $("#tblReject").DataTable({
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
                "url": "/api/dashboardBAUK/rejectSummary",
                "type": "POST",
                "data": {
                    CompanyIDs: $("#ddlBAUKCompany").val(),
                    CustomerIDs: $("#ddlBAUKCustomer").val(),
                    STIPIDs: $("#ddlBAUKCategory").val(),
                    ProductIDs: $("#ddlBAUKProduct").val(),
                    GroupBy: $("#ddlBAUKGrouping").val(),
                    Months: $("#ddlBAUKMonth").val(),
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
                { data: "TotReject" },
                {
                    data: "TotImproper",
                    mRender: function (data, type, full) {
                        return "<a data-selection='1' class='btDetail'>" + full.TotImproper + "</a>";
                    }
                },
                {
                    data: "TotUncompleted",
                    mRender: function (data, type, full) {
                        return "<a data-selection='2' class='btDetail'>" + full.TotUncompleted + "</a>";
                    }
                },
                {
                    data: "TotWrong",
                    mRender: function (data, type, full) {
                        return "<a data-selection='3' class='btDetail'>" + full.TotWrong + "</a>";
                    }
                },
                {
                    data: "TotOther",
                    mRender: function (data, type, full) {
                        return "<a data-selection='4' class='btDetail'>" + full.TotOther + "</a>";
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
                Common.CheckError.List(tblReject.data());
                App.unblockUI("#tblReject");
            },
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;


                intReject = 0;
                intImproper = 0;
                intUncompleted = 0;
                intWrong = 0;
                intOther = 0;
                intPercent = 0;

                var oTableReject = $('#tblReject').DataTable();
                oTableReject.rows('').every(function (rowIdx, tableLoop, rowLoop) {
                    intReject += this.data().TotReject;
                    intImproper += this.data().TotImproper;
                    intUncompleted += this.data().TotUncompleted;
                    intWrong += this.data().TotWrong;
                    intOther += this.data().TotOther;
                    intPercent += this.data().PercentTotal;
                });

                totalBAUK = intImproper + intUncompleted + intWrong + intOther;
                prcImproper = totalBAUK == 0 ? 0 : (intImproper / totalBAUK) * 100;
                prcUncompleted = totalBAUK == 0 ? 0 : (intUncompleted / totalBAUK) * 100;
                prcWrong = totalBAUK == 0 ? 0 : (intWrong / totalBAUK) * 100;
                prcOther = totalBAUK == 0 ? 0 : (intOther / totalBAUK) * 100;

                $("#totRej").html(intReject);
                $("#totImp").html(intImproper);
                $("#totUnc").html(intUncompleted);
                $("#totWro").html(intWrong);
                $("#totOth").html(intOther);
                $("#totPrc").html(intPercent + " %");
                $("#prcImp").html(prcImproper.toFixed(2) + " %");
                $("#prcUnc").html(prcUncompleted.toFixed(2) + " %");
                $("#prcWro").html(prcWrong.toFixed(2) + " %");
                $("#prcOth").html(prcOther.toFixed(2) + " %");
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

        window.location.href = "/Dashboard/BAUK/Reject/Export?intMonth=" + intMonth + "&intYear=" + intYear + "&strGroupBy=" + strGroupBy
            + "&strMonth=" + strMonth + "&strCompany=" + strCompany + "&strCustomer=" + strCustomer + "&strSTIP=" + strSTIP + "&strProduct=" + strProduct;
    },
}

var RejectDocDetail = {
    Init: function () {
        this.Load();
        $("#btnCloseMdl").unbind().click(function () {
            $("#modal-reject").modal('hide');
        })
    },
    Load: function () {
        App.blockUI({ target: "#tblRejectDetail", animate: !0 });
        var tblRejectDetail = $("#tblRejectDetail").DataTable({
            "orderCellsTop": true,
            "filter": true,
            "destroy": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "buttons": [
            ],
            "order": [[0, 'asc']],
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "ajax": {
                "url": "/api/dashboardBAUK/rejectSummary/detail",
                "type": "POST",
                "datatype": "json",
                "data": {
                    CustomerID: $("#ddlBAUKGrouping").val() == "Customer" ? DataSelected.GroupSumID : "",
                    STIPID: $("#ddlBAUKGrouping").val() == "STIP" ? DataSelected.GroupSumID : "",
                    CheckType: DataSelected.SelectedData,
                    Months: $("#ddlBAUKMonth").val(),
                    Year: $("#ddlBAUKYear").val(),
                },
            },
            "columns": [
                {
                    data: "SoNumber"
                },
                {
                    data: "DocumentName"
                },
                {
                    data: "RejectReason"
                },
                {
                    data: "ActivityDate",
                    render: function (data, type, row, meta) {
                        var date = new Date(Date.parse(data));
                        return (data != null) ? moment(date).format('DD MMM YYYY') : "-";
                    }
                }
            ],
            "columnDefs": [
                { "targets": [0, 1, 2, 3], "className": "dt-center" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                App.unblockUI("#tblRejectDetail");
                $("#tblRejectDetail_filter").hide();
            }

        });
    },
}