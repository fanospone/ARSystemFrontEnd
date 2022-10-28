Data = {};
Data.RowSelectedDocumentChecklist = [];

var Duration, StipSiroDtl, activityID;
var startTarget, endTarget, _idCategory;
jQuery(document).ready(function () {
    DropDown.Init();
    Table.Init();
    $("#pnlSummary").hide();
    $("#panelDetail").hide();
    $("#pnlSummaryDetail").hide();
    $("#myModal").hide();
    $("#modalHistoryPICA").hide();
    $("#ddlTypeBaps").attr('disabled', true);

    $(".btSearch").unbind().click(function () {
        $("#pnlSummary").fadeIn();
        Table.GetListHeader();
    });

    $(".btnReset").unbind().click(function () {
        Table.Reset();
    });

    $(".btBack").unbind().click(function () {
        $("#pnlSummary").fadeIn();
        $("#panelTransaction").fadeIn();
        $("#panelDetail").hide();
        $("#pnlSummaryDetail").hide();
    });

    $(".btnSearchSummary").unbind().click(function () {
        Table.GetListHeader();
    });

    $('#tblSummary input').keypress(function (e) {
        if (e.which == 13) {
            Table.GetListHeader();
        }
    });

    $(window).resize(function () {
        $("#tblSummary").DataTable().columns.adjust().draw();
    });

});

var DropDown = {
    Init: function () {
        DropDown.DdlCustomer();
        DropDown.DdlCompany();
        DropDown.DdlProduct();
        DropDown.DdlBapsType();
        DropDown.DdlStipCategory();
        DropDown.DdlActivityStatus();
        DropDown.BindingDatePicker();
    },

    DdlCustomer: function () {
        $.ajax({
            url: "/api/PICARASystem/GetDdlCustomer",
            type: "GET",
            async: false,
        })
        .done(function (data, textStatus, jqXHR) {
            $("#ddlCustomer").html("<option>ALL</option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {

                    $("#ddlCustomer").append("<option value='" + item.ID + "'>" + item.Value + "</option>");

                })
            }
            $("#ddlCustomer").select2({ placeholder: "Customer", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    DdlCompany: function () {
        $.ajax({
            url: "/api/PICARASystem/GetDdlCompany",
            type: "GET",
            async: false,
        })
        .done(function (data, textStatus, jqXHR) {
            $("#ddlCompany").html("<option>ALL</option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#ddlCompany").append("<option value='" + item.ID + "'>" + item.Value + "</option>");
                })
            }
            $("#ddlCompany").select2({ placeholder: "Company", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    DdlProduct: function () {
        $.ajax({
            url: "/api/PICARASystem/GetDdlProduct",
            type: "GET",
            async: false,
        })
        .done(function (data, textStatus, jqXHR) {
            $("#ddlProductName").html("<option>ALL</option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {

                    $("#ddlProductName").append("<option value='" + item.ID + "'>" + item.Value + "</option>");

                })
            }
            $("#ddlProductName").select2({ placeholder: "Prduct Name", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    DdlBapsType: function () {
        $.ajax({
            url: "/api/PICARASystem/GetDdlBapsType",
            type: "GET",
            async: false,
        })
        .done(function (data, textStatus, jqXHR) {
            $("#ddlTypeBaps").html("<option>TOWER</option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#ddlTypeBaps").append("<option value='" + item.Value + "'>" + item.Value + "</option>");
                })
            }
            $("#ddlTypeBaps").select2({ placeholder: "Type Baps", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    DdlStipCategory: function () {
        $.ajax({
            url: "/api/PICARASystem/GetDdlStipCategory",
            type: "GET",
            async: false,
        })
        .done(function (data, textStatus, jqXHR) {
            $("#ddlSTIPCategory").html("<option>ALL</option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#ddlSTIPCategory").append("<option value='" + item.Value + "'>" + item.Value + "</option>");
                })
            }
            $("#ddlSTIPCategory").select2({ placeholder: "STIP Category", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    DdlActivityStatus: function () {
        $.ajax({
            url: "/api/PICARASystem/GetDdlActivityStatus",
            type: "GET",
            async: false,
        })
        .done(function (data, textStatus, jqXHR) {
            $("#ddlActivityStatus").html("<option>ALL</option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#ddlActivityStatus").append("<option value='" + item.Value + "'>" + item.Value + "</option>");
                })
            }
            $("#ddlActivityStatus").select2({ placeholder: "Activity Status", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    DdlCategoryPICA: function () {
        $.ajax({
            url: "/api/PICARASystem/GetDdlCategoryPICA",
            type: "GET",
            async: false,
        })
        .done(function (data, textStatus, jqXHR) {
            $("#ddlCategoryPICA").html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#ddlCategoryPICA").append("<option value='" + item.Value + "'>" + item.Value + "</option>");
                })
            }
            $("#ddlCategoryPICA").select2({ placeholder: "Category PICA", width: null });
            $("#ddlPICA").select2({ placeholder: "PICA", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    DdlPICA: function (PICA) {
        $.ajax({
            url: "/api/PICARASystem/GetDdlPICA?PICA="+ PICA,
            type: "GET",
            async: false,
        })
        .done(function (data, textStatus, jqXHR) {
            $("#ddlPICA").html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#ddlPICA").append("<option value='" + item.ID + "'>" + item.Value + "</option>");
                })
            }
            $("#ddlPICA").select2({ placeholder: "PICA", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingDatePicker: function () {
        
        $('#drTargetPica').datepicker({
            format: "yyyy-mm-dd",
            autoclose: true,
            defaultDate: new Date()
        });

    },
}

$("#ddlCategoryPICA").on('change', function () {
    $('#ddlPICA').val($(this).val());
    var PICA = $(this).val();
    DropDown.DdlPICA(PICA);
});

var Table = {
    Init: function(){
        $(".btSave").unbind().click(function () {
            Data.InsertPICA(Duration, startTarget, endTarget, StipSiroDtl, activityID);
        });

        $(".btCancel").unbind().click(function () {
            $('#myModal').modal('hide');
            $("#drTargetPica").val("");
            $("#tbDetailPICA").val("");
            $("#ddlPICA").val("").trigger('change');
            $("#ddlCategoryPICA").val("").trigger('change');
        });
    },
    GetListHeader: function () {
        App.blockUI({ target: "#tblSummary", animate: !0 });

        var url_datatable = "/api/PICARASystem/GetListHeader";
        var formData = {
            CompanyId: $("#ddlCompany").val(),
            CustomerId: $("#ddlCustomer").val(),
            ProductID: $("#ddlProductName").val(),
            StipSiro: $("#ddlSTIPCategory").val(),
            ActivityName: $("#ddlActivityStatus").val(),
            BapsType: $("#ddlTypeBaps").val(),
            SONumber: $("#tbxSearchSonumb").val().replace(/'/g, "''")
        };

        console.log(formData);
        if (formData.CompanyId == "ALL") {
            formData.CompanyId = null
        }
        if (formData.CustomerId == "ALL") {
            formData.CustomerId = null
        }
        if (formData.ProductID == "ALL") {
            formData.ProductID = null
        }
        if (formData.StipSiro == "ALL") {
            formData.StipSiro = null
        }
        if (formData.CompanyId == "ALL") {
            formData.CompanyId = null
        }
        if (formData.ActivityName == "ALL") {
            formData.ActivityName = null
        }
        if (formData.BapsType == "ALL") {
            formData.BapsType = null
        }

        var params = {
            DashboardPICARASystem: formData
        };
        console.log(params);

        var tblSummaryData = $('#tblSummary').DataTable({
            "orderCellsTop": true,
            "paging": true,
            "destroy": true,
            "processing": true,
            "serverSide": true,
            //"cache": false,
            "ajax": {
                "url": url_datatable,
                "type": 'POST',
                "dataType": "json",
                "data": params

            },
            "language": {
                "emptyTable": "No data available in table",
            },
            buttons: [
                    { text: 'Export All PICA History', className: 'btn blue btn-outline', action: function (e, dt, node, config) { Table.HistoryPica(null, null) } },
                    { text: 'Excel', className: 'btn green btn-outline', action: function (e, dt, node, config) { Data.ExportHeader() } },
                    { extend: 'colvis', className: 'btn dark btn-outline', text: 'Columns' }
            ],
            "filter": false,
            //"async": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            
            "columns": [
                {
                    data: "CompanyInvoice",
                    render: function (x, y, data) {
                        console.log("Masuk gak sih", data);
                        return "<button  type='button ' class='btn  btn-xs green  btSelect' data-style='zoom-in'><i class='fa fa-edit' title='Select'></i>&nbsp;</button>";
                    }
                },
                {
                    data: "SONumber"
                },
                {
                    data: "SiteId"
                },
                 {
                     data: "SiteName"
                 },
                {
                    data: "SiteIdOpr"
                },
                {
                    data: "SiteNameOpr"
                },
                {
                    data: "CustomerId"
                },
                {
                    data: "CompanyId"
                },
                {
                    data: "StipSiro"
                },
                {
                    data: "BapsType"
                },
                {
                    data: "StartBapsDate",
                    mRender: function (data, type, full) {
                        console.log(full.StartBapsDate);
                        return full.StartBapsDate ? Common.Format.ConvertJSONDateTime(full.StartBapsDate) : "-";
                    }
                },
                {
                    data: "EndBapsDate",
                    mRender: function (data, type, full) {
                        return full.EndBapsDate ? Common.Format.ConvertJSONDateTime(full.EndBapsDate) : "-";
                    }

                },
                {
                    data: "StartDateInvoice",
                    mRender: function (data, type, full) {
                        console.log(full.StartBapsDate);
                        return full.StartDateInvoice ? Common.Format.ConvertJSONDateTime(full.StartDateInvoice) : "-";
                    }
                },
                {
                    data: "EndDateInvoice",
                    mRender: function (data, type, full) {
                        console.log(full.StartBapsDate);
                        return full.EndDateInvoice ? Common.Format.ConvertJSONDateTime(full.EndDateInvoice) : "-";
                    }
                },
                {
                    data: "AmountRental", render: $.fn.dataTable.render.number(',', '.', 2)
                },
                {
                    data: "AmountService", render: $.fn.dataTable.render.number(',', '.', 2)
                },
                {
                    data: "InvoiceAmount", render: $.fn.dataTable.render.number(',', '.', 2)
                },
                {
                    data: "ActivityName"
                }

            ],
            "columnDefs": [
                { "targets": [0], "width": "2%", "className": "dt-center" },
                { "targets": [1], "width": "20%", "className": "dt-center" },
                { "targets": [2,4], "width": "2%", "className": "dt-center" },
                { "targets": [3, 5, 6,7,8,9,10], "width": "10%", "className": "dt-center" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                App.unblockUI("#tblSummary");
            },
            "order": [[1, 'asc']]
        });

        $("#tblSummary tbody").on("click", ".btSelect", function (e) {
            var tblSummaryData = $("#tblSummary").DataTable();
            var tr = $(this).closest('tr');
            var row = tblSummaryData.row(tr);
            var dt = row.data();

            console.log("datatables: ", dt);
            $("#pnlSummary").hide();
            $("#panelTransaction").hide();
            $("#panelDetail").fadeIn();
            $("#pnlSummaryDetail").fadeIn();
            

            $("#tbSONumber").attr('disabled', true);
            $("#tbSiteId").attr('disabled', true);
            $("#tbCompanyInv").attr('disabled', true);
            $("#tbSiteName").attr('disabled', true);
            $("#drPeriodeBapsFrom").attr('disabled', true);
            $("#drPeriodeBapsTo").attr('disabled', true);
            $("#drPeriodeInvoiceFrom").attr('disabled', true);
            $("#drPeriodeInvoiceTo").attr('disabled', true);
            $("#tbSiteIdOpr").attr('disabled', true);
            $("#tbSiteNameOpr").attr('disabled', true);
            $("#tbStipSiro").attr('disabled', true);
            $("#tbCustomerInv").attr('disabled', true);

            $("#tbSONumber").val(dt.SONumber);
            $("#tbSiteId").val(dt.SiteId);
            $("#tbCompanyInv").val(dt.CompanyInvoice);
            $("#tbSiteName").val(dt.SiteName);
            $("#drPeriodeBapsFrom").val(Common.Format.ConvertJSONDateTime(dt.StartBapsDate));
            $("#drPeriodeBapsTo").val(Common.Format.ConvertJSONDateTime(dt.EndBapsDate));
            $("#drPeriodeInvoiceFrom").val(Common.Format.ConvertJSONDateTime(dt.StartDateInvoice));
            $("#drPeriodeInvoiceTo").val(Common.Format.ConvertJSONDateTime(dt.EndDateInvoice));
            $("#tbSiteIdOpr").val(dt.SiteIdOpr);
            $("#tbSiteNameOpr").val(dt.SiteNameOpr);
            $("#tbStipSiro").val(dt.StipSiro);
            $("#tbBapsType").val(dt.BapsType);
            $("#tbActivityID").val(dt.ActivityID);
            $("#tbCustomerInv").val(dt.CustomerInvoice);

            console.log(dt.StipSiro);
            Table.GetListDetail(dt.SONumber, dt.ActivityName, dt.StipSiro);
        });
    },

    GetListDetail: function (SoNumber, ActivityName, Stipsiro) {
        App.blockUI({ target: "#tblSummaryDetail", animate: !0 });

        var url_datatable = "/api/PICARASystem/GetListDetail";
        var formData = {
            CompanyId: $("#ddlCompany").val(),
            CustomerId: $("#ddlCustomer").val(),
            ProductID: $("#ddlProductName").val(),
            StipCode: $("#ddlSTIPCategory").val(),
            ActivityName: $("#ddlActivityStatus").val(),
            BapsType: $("#ddlTypeBaps").val(),
            SONumber: SoNumber,
            StipSiro: Stipsiro

        };
        if (formData.CompanyId == "ALL") {
            formData.CompanyId = null
        }
        if (formData.CustomerId == "ALL") {
            formData.CustomerId = null
        }
        if (formData.ProductID == "ALL") {
            formData.ProductID = null
        }
        if (formData.StipSiro == "ALL") {
            formData.StipSiro = null
        }
        if (formData.CompanyId == "ALL") {
            formData.CompanyId = null
        }
        if (formData.ActivityName == "ALL") {
            formData.ActivityName = null
        }
        if (formData.BapsType == "ALL") {
            formData.BapsType = null
        }

        var params = {
            DashboardPICARASystem: formData
        };

        var tblSummaryData = $('#tblSummaryDetail').DataTable({
            "orderCellsTop": true,
            "paging": false,
            "destroy": true,
            "processing": false,
            "serverSide": true,
            //"cache": false,
            "ajax": {
                "url": url_datatable,
                "type": 'POST',
                "dataType": "json",
                "data": params

            },
            "language": {
                "emptyTable": "No data available in table",
            },
            buttons: [
                    { text: 'View PICA', className: 'btn blue btn-outline', action: function (e, dt, node, config) { Table.HistoryPica(SoNumber, Stipsiro) } },
                    { text: 'Excel', className: 'btn green btn-outline', action: function (e, dt, node, config) { Data.ExportDetail(SoNumber, ActivityName) } },
                    { extend: 'colvis', className: 'btn dark btn-outline', text: 'Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10,20], [10,20]],
            "columns": [
                {
                    data: "RowIndex"
                },
                {
                    data: "ActivityName"
                },
                {
                    data: "Durasi"
                },
                {
                    data: "StartTarget",
                    render: function (date) {
                        return Common.Format.ConvertJSONDateTime(date);
                    }
                },
                {
                    data: "EndTarget",
                    render: function (date) {
                        return Common.Format.ConvertJSONDateTime(date);
                    }
                },
                {
                    data: "EndActual",
                    mRender: function (data, type, full) {
                        return full.EndActual ? Common.Format.ConvertJSONDateTime(full.EndActual) : "-";
                    }

                },
                {
                    data: "LTActual"
                },
                {
                    data: "ActivityID",
                    render: function (x, y, data) {
                        return "<button  type='button ' class='btn  btn-xs green  btPICA' data-style='zoom-in'>&nbsp; PICA</button>";
                    }
                }
            ],
            "columnDefs": [
                { "targets": [0], "width": "1%", "className": "dt-center" },
                { "targets": [1], "width": "20%", "className": "dt-center" },
                { "targets": [2], "width": "10%", "className": "dt-center" },
                { "targets": [3,4,5,6,7], "width": "10%", "className": "dt-center" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                App.unblockUI("#tblSummaryDetail");
            },
            "order": [[1, 'asc']]
        });

        $("#tblSummaryDetail tbody").on("click", ".btPICA", function (e) {
            
            var tblSummaryData = $("#tblSummaryDetail").DataTable();
            var tr = $(this).closest('tr');
            var row = tblSummaryData.row(tr);
            var dt = row.data();

            console.log("btPICA: ", dt);
            Duration = dt.Durasi;
            startTarget = dt.StartTarget;
            endTarget = dt.EndTarget;
            StipSiroDtl = dt.StipSiro;
            activityID = dt.ActivityID;

            DropDown.DdlCategoryPICA();
            $('#myModal').modal('show');
        });
    },
    
    HistoryPica: function (SoNumber, Stipsiro) {
       
        App.blockUI({ target: "#tblHistoryPICA", animate: !0 });
        var url_datatable = "/api/PICARASystem/GetHistoryPICA";

        var formData = {
            SONumber: SoNumber,
            StipSiro: Stipsiro
        };
        var params = {
            DashboardPICARASystem: formData
        };

        console.log("View PICA PAram : ", params);
        var tblSummaryData = $('#tblHistoryPICA').DataTable({
            "orderCellsTop": true,
            "paging": true,
            "destroy": true,
            "processing": false,
            "serverSide": true,
            //"cache": false,
            "ajax": {
                "url": url_datatable,
                "type": 'POST',
                "dataType": "json",
                "data": params

            },
            "language": {
                "emptyTable": "No data available in table",
            },
            buttons: [{ text: 'Excel', className: 'btn green btn-outline', action: function (e, dt, node, config) { Data.ExportHistoryPICA(SoNumber, Stipsiro) } },
                    
            ],
            "filter": false,
            "lengthMenu": [[10, 20], [10, 20]],

            "columns": [
                {
                    data: "RowIndex"
                },
                {
                    data: "SONumber"
                },
                {
                    data: "SiteId"
                },
                {
                    data: "SiteName"
                },
                {
                    data: "SiteIdOpr"
                },
                {
                    data: "SiteNameOpr"
                },
                {
                    data: "ActivityName"
                },
                {
                    data: "CategoryPICA"
                },
                {
                    data: "PICA"
                },
                {
                    data: "DetailPICA"
                },
                {
                    data: "StipSiro"
                },
                {
                    data: "TargetPICA",
                    render: function (date) {
                        return Common.Format.ConvertJSONDateTime(date);
                    }
                },
                {
                    data: "CreatedDate",
                    render: function (date) {
                        return Common.Format.ConvertJSONDateTime(date);
                    }
                },
                {
                    data: "CreatedBy"
                }
            ],
            "columnDefs": [
                { "targets": [0], "width": "1%", "className": "dt-center" },
                { "targets": [1], "width": "10%", "className": "dt-center" },
                { "targets": [2, 3, 4, 5, 6], "width": "3%", "className": "dt-center" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                App.unblockUI("#tblHistoryPICA");
            },
            "order": [[1, 'asc']]
        });
        $('#modalHistoryPICA').modal('show');
        //$("#modalHistoryPICA").fadeIn();
    },

    Reset: function () {
        $("#ddlCompany").val("").trigger('change');
        $("#ddlCustomer").val("").trigger('change');
        $("#ddlProductName").val("").trigger('change');
        $("#ddlSTIPCategory").val("").trigger('change');
        $("#ddlActivityStatus").val("").trigger('change');

        $("#pnlSummary").hide();
        $("#panelDetail").hide();
        $("#ddlTypeBaps").val("").trigger('change');

        $("#drTargetPica").val("");
        $("#tbDetailPICA").val("");
        $("#ddlPICA").val("").trigger('change');
        $("#ddlCategoryPICA").val("").trigger('change');
        $("tbxSearchSonumb").val("");
    },
}

var Data = {
    InsertPICA: function (Duration, startTarget, endTarget, Stipsiro, activityID) {
        var l = Ladda.create(document.querySelector(".btSave"));

        var formData = {
            SONumber: $('#tbSONumber').val(),
            SiteId: $('#tbSiteId').val(),
            SiteIdOpr: $('#tbSiteIdOpr').val(),
            SiteName: $('#tbSiteName').val(),
            SiteNameOpr: $('#tbSiteNameOpr').val(),
            BapsType: $('#tbBapsType').val(),
            ActivityID: activityID,
            Durasi: Duration,
            CategoryPICA: $('#ddlCategoryPICA').val(),
            PICA: $('#ddlPICA').val(),
            DetailPICA: $('#tbDetailPICA').val(),
            TargetPICA: $('#drTargetPica').val(),
            StartTarget: startTarget,
            EndTarget: endTarget,
            StipSiro: Stipsiro
        };

        var params = {
            DashboardPICARASystem: formData
        }
        console.log("Param Insert: ", params);
        $.ajax({
            type: 'POST',
            url: '/api/PICARASystem/InsertPICA',
            data: JSON.stringify(params),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (msg) {
                $('#myModal').modal('hide');
                Common.Alert.Success("Data has been created!");
                
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
                l.stop();
            }
        });
    },

    ExportHeader() {

        var formData = {
            CompanyId: $("#ddlCompany").val(),
            CustomerId: $("#ddlCustomer").val(),
            ProductID: $("#ddlProductName").val(),
            StipSiro: $("#ddlSTIPCategory").val(),
            ActivityName: $("#ddlActivityStatus").val(),
            BapsType: $("#ddlTypeBaps").val(),
            SONumber: $("#tbxSearchSonumb").val().replace(/'/g, "''")
        };

        if ($("#tbxSearchSonumb").val().replace(/'/g, "''") == '') {
            formData.SONumber = null;
        }
        
        if (formData.ProductID == 'ALL') {
            formData.ProductID = 0;
        }
        console.log("formData export : ", formData.ProductID);

        window.location.href = "/RevenueAssurance/ExportHeader?Company=" + formData.CompanyId +
            "&Customer=" + formData.CustomerId + "&ProductID=" + formData.ProductID + "&StipSiro=" + formData.StipSiro +
            "&ActivityName=" + formData.ActivityName + "&BapsType=" + formData.BapsType + "&SONumber=" + formData.SONumber;
    },

    ExportDetail(SoNumber, ActivityName) {
        var formData = {
            CompanyId: $("#ddlCompany").val(),
            CustomerId: $("#ddlCustomer").val(),
            ProductID: $("#ddlProductName").val(),
            StipSiro: $("#ddlSTIPCategory").val(),
            ActivityName: $("#ddlActivityStatus").val(),
            BapsType: $("#ddlTypeBaps").val(),
            SONumber: SoNumber
        };

        if (formData.ProductID == 'ALL') {
            formData.ProductID = 0;
        }

        window.location.href = "/RevenueAssurance/ExportDetail?Company=" + formData.CompanyId +
            "&Customer=" + formData.CustomerId + "&ProductID=" + formData.ProductID + "&StipSiro=" + formData.StipSiro +
            "&ActivityName=" + formData.ActivityName + "&BapsType=" + formData.BapsType + "&SONumber=" + formData.SONumber;
    },

    ExportHistoryPICA(SoNumber, Stipsiro) {
        var formData = {
            StipSiro: Stipsiro,
            SONumber: SoNumber
        };

        console.log("history export : ", formData);
        window.location.href = "/RevenueAssurance/ExportHistoryPICA?StipSiro=" + formData.StipSiro + "&SONumber=" + formData.SONumber;
        $('#modalHistoryPICA').modal('hide');
    }
}
