var DataSelected = {};
var MilestoneStatus = "";

jQuery(document).ready(function () {
    Form.Init();

    $("#btSearch").unbind().click(function () {
        Table.Search();
    });

    $("#btBackToList").unbind().click(function () {
        $('.panelDetailTemplate').fadeOut(500);
        $('.panelSearchResult').fadeIn(500);
        Table.Search();
        $('#slProcess').val("").trigger("change");
        $('#slCustomer').val("").trigger("change");
        $('#slEmailTemplate').val("").trigger("change");
    });

    $(".btCancel").unbind().click(function () {
        Form.Cancel();

    });

    $("#slTypeProcess").change(function () {
        Form.BindingSelectTypePICA();

    });

    $("#slTypePICA").change(function () {
        Form.BindingSelectCategoryPICA();

    });

    $("#btSavePICA").unbind().click(function () {
        if ($("#formUpdatePICA").parsley().validate()) {
            Action.SavePICA();
        }

    });

    $("#btCancelPICA").unbind().click(function () {
        Form.ResetPICA();
        $("#formUpdatePICA").parsley().destroy();
    });

    $("#btDownloadPica").unbind().click(function () {
        Table.Export();
    });
});


var Table = {
    Init: function () {

        var tblDetail = $('#tblDetail').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $("#tblDashboard tbody").on("click", "a.btDetail1", function (e) {
            e.preventDefault();
            var table = $("#tblDashboard").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                var params = {
                    CustomerID: $('#slSearchCustomer').val(),
                    CompanyID: $('#slSearchCompany').val(),
                    StipCategory: $('#slSearchStip').val(),
                    Year: $('#slSearchYear').val(),
                    TenantType: $('#slSearchTenant').val(),
                    LeadTime: "1_5",
                    StatusID: data.StatusID,
                    ID: data.ID
                }
                Table.ShowDetail();
                Table.SearchDetail(params);
            }
        });

        $("#tblDashboard tbody").on("click", "a.btDetail2", function (e) {
            e.preventDefault();
            var table = $("#tblDashboard").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                var params = {
                    CustomerID: $('#slSearchCustomer').val(),
                    CompanyID: $('#slSearchCompany').val(),
                    StipCategory: $('#slSearchStip').val(),
                    Year: $('#slSearchYear').val(),
                    TenantType: $('#slSearchTenant').val(),
                    LeadTime: "6_10",
                    StatusID: data.StatusID,
                    ID: data.ID
                }

                Table.ShowDetail();
                Table.SearchDetail(params);
            }
        });

        $("#tblDashboard tbody").on("click", "a.btDetail3", function (e) {
            e.preventDefault();
            var table = $("#tblDashboard").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                var params = {
                    CustomerID: $('#slSearchCustomer').val(),
                    CompanyID: $('#slSearchCompany').val(),
                    StipCategory: $('#slSearchStip').val(),
                    Year: $('#slSearchYear').val(),
                    TenantType: $('#slSearchTenant').val(),
                    LeadTime: "11_15",
                    StatusID: data.StatusID,
                    ID: data.ID
                }
                Table.ShowDetail();
                Table.SearchDetail(params);
            }
        });

        $("#tblDashboard tbody").on("click", "a.btDetail4", function (e) {
            e.preventDefault();
            var table = $("#tblDashboard").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                var params = {
                    CustomerID: $('#slSearchCustomer').val(),
                    CompanyID: $('#slSearchCompany').val(),
                    StipCategory: $('#slSearchStip').val(),
                    Year: $('#slSearchYear').val(),
                    TenantType: $('#slSearchTenant').val(),
                    LeadTime: "16_20",
                    StatusID: data.StatusID,
                    ID: data.ID
                }
                Table.ShowDetail();
                Table.SearchDetail(params);
            }
        });

        $("#tblDashboard tbody").on("click", "a.btDetail5", function (e) {
            e.preventDefault();
            var table = $("#tblDashboard").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                var params = {
                    CustomerID: $('#slSearchCustomer').val(),
                    CompanyID: $('#slSearchCompany').val(),
                    StipCategory: $('#slSearchStip').val(),
                    Year: $('#slSearchYear').val(),
                    TenantType: $('#slSearchTenant').val(),
                    LeadTime: ">21",
                    StatusID: data.StatusID,
                    ID: data.ID
                }
                Table.ShowDetail();
                Table.SearchDetail(params);
            }
        });

        $("#tblDetail tbody").on("click", "a.btUpdatePICA", function (e) {
            e.preventDefault();
            var table = $("#tblDetail").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                DataSelected.Selected = data;
                $('#mdlPICA').modal('toggle');
                Table.ShowPICA();
            }
        });

        $("#tblDashboard tbody").on("click", "a.btTotalSite", function (e) {
            e.preventDefault();
            var table = $("#tblDashboard").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                var params = {
                    CustomerID: $('#slSearchCustomer').val(),
                    CompanyID: $('#slSearchCompany').val(),
                    StipCategory: $('#slSearchStip').val(),
                    Year: $('#slSearchYear').val(),
                    TenantType: $('#slSearchTenant').val(),
                    StatusID: data.StatusID,
                    ID: data.ID
                }
                Table.ShowDetail();
                Table.SearchTotalSite(params);
            }
        });

        $("#tblDashboard tbody").on("click", "a.btParentData", function (e) {
            e.preventDefault();
            var table = $("#tblDashboard").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                var params = {
                    CustomerID: $('#slSearchCustomer').val(),
                    CompanyID: $('#slSearchCompany').val(),
                    StipCategory: $('#slSearchStip').val(),
                    Year: $('#slSearchYear').val(),
                    TenantType: $('#slSearchTenant').val(),
                    StatusID: data.StatusID,
                    ID: data.ID
                }
                Table.ShowDetail();
                Table.SearchParentData(params);
            }
        });

        $(window).resize(function () {
            $("#tblDashboard").DataTable().columns.adjust().draw();
        });
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        var params = {
            CustomerID: $('#slSearchCustomer').val(),
            CompanyID: $('#slSearchCompany').val(),
            StipCategory: $('#slSearchStip').val(),
            Year: $('#slSearchYear').val(),
            TenantType: $('#slSearchTenant').val()
        }

        var tblSiteInfo = $("#tblDashboard").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/DashboardXL/GetList",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            "lengthMenu": [[-1], ['All']],
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        return full.ID == 0 ? "" : full.ID;
                    }
                },
                { data: "Status" },

                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        return full.OneToFive == 0 ? full.OneToFive : "<a class='btDetail1'>" + full.OneToFive + "</a>";

                    }
                },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        return full.SixToTen == 0 ? full.SixToTen : "<a class='btDetail2'>" + full.SixToTen + "</a>";

                    }
                },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        return full.ElevenToFiveteen == 0 ? full.ElevenToFiveteen : "<a class='btDetail3'>" + full.ElevenToFiveteen + "</a>";

                    }
                },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        return full.SixteenToTwenty == 0 ? full.SixteenToTwenty : "<a class='btDetail4'>" + full.SixteenToTwenty + "</a>";
                    }
                },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        return full.TwentyOneUp == 0 ? full.TwentyOneUp : "<a class='btDetail5'>" + full.TwentyOneUp + "</a>";
                    }
                },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        if (full.ID == 0) {
                            return full.TotalSite == 0 ? full.TotalSite : "<a class='btTotalSite'>" + full.TotalSite + "</a>";
                        } else {
                            return full.TotalSite == 0 ? full.TotalSite : "<a class='btParentData'>" + full.TotalSite + "</a>";
                        }

                    }
                },
            ],
            "buttons": [
                {
                    text: '<i class="fa fa-download">&nbsp;Download PICA</i>', titleAttr: 'Download PICA', className: 'btn blue btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".blue"));
                        l.start();
                        Table.Export();
                        l.stop();
                    }
                }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "destroy": true,
            "fnDrawCallback": function (oSettings, json) {

            },
            "footerCallback": function (row, data, start, end, display) {

            },

        });
        l.stop();

        $(window).resize(function () {
            $("#tblDashboard").DataTable().columns.adjust().draw();
        });

    },

    ShowDetail: function () {

        $('.Detail').show();

        $("#pnlSummary").hide();
        $("#pnlDetail").fadeIn();
        $(".panelDetail").show();
        $("#formMaster").parsley().reset()

    },

    SearchDetail: function (Parameter) {
        var tblDashboardDetail = $("#tblDetail").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/DashboardXL/GetListDetail",
                "type": "POST",
                "datatype": "json",
                "data": Parameter,
            },
            "lengthMenu": [[50, 100], ['50', '100']],
            "columns": [
                 {
                     orderable: false,
                     mRender: function (data, type, full) {
                         return "<a class='btUpdatePICA'>" + "Update" + "</a>";
                     }
                 },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CustomerID" },
                { data: "CompanyID" },
                {
                    data: "bauk_done", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "Status" },
            ],
            buttons: [
               { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    extend: 'excelHtml5',
                    filename: 'Detail Sonumb',
                    text: '<i class="fa fa-file-excel-o"></i>',
                    titleAttr: 'Export to Excel',
                    className: 'btn yellow btn-outline',
                    exportOptions: {
                        columns: 'th:not(:first-child)',
                        format: {
                            body: function (data, row, column, node) {
                                return (column <= 4) ? "\0" + data : data;
                            }
                        }
                    }
                },
               { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "scrollY": 500,
            "destroy": true,
            "fnDrawCallback": function (oSettings, json) {

            },
            "footerCallback": function (row, data, start, end, display) {

            },
        });

        $(window).resize(function () {
            $("#tblDetail").DataTable().columns.adjust().draw();
        });

    },

    SearchTotalSite: function (Parameter) {
        var tblDashboardDetail = $("#tblDetail").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/DashboardXL/GetListDetail",
                "type": "POST",
                "datatype": "json",
                "data": Parameter,
            },
            "lengthMenu": [[50, 100], ['50', '100']],
            "columns": [
                 {
                     orderable: false,
                     mRender: function (data, type, full) {
                         return "<a class='btUpdatePICA'>" + "Update" + "</a>";
                     }
                 },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CustomerID" },
                { data: "CompanyID" },
                {
                    data: "bauk_done", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "Status" },
            ],
            buttons: [
               { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    extend: 'excelHtml5',
                    filename: 'Detail Sonumb',
                    text: '<i class="fa fa-file-excel-o"></i>',
                    titleAttr: 'Export to Excel',
                    className: 'btn yellow btn-outline',
                    exportOptions: {
                        columns: 'th:not(:first-child)',
                        format: {
                            body: function (data, row, column, node) {
                                return (column <= 4) ? "\0" + data : data;
                            }
                        }
                    }
                },
               { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "scrollY": 500,
            "destroy": true,
            "fnDrawCallback": function (oSettings, json) {

            },
            "footerCallback": function (row, data, start, end, display) {

            },
        });

        $(window).resize(function () {
            $("#tblDetail").DataTable().columns.adjust().draw();
        });

    },

    SearchParentData: function (Parameter) {
        var tblDashboardDetail = $("#tblDetail").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/DashboardXL/GetListDetailParent",
                "type": "POST",
                "datatype": "json",
                "data": Parameter,
            },
            "lengthMenu": [[50, 100], ['50', '100']],
            "columns": [
                 {
                     orderable: false,
                     mRender: function (data, type, full) {
                         return "<a class='btUpdatePICA'>" + "Update" + "</a>";
                     }
                 },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CustomerID" },
                { data: "CompanyID" },
                {
                    data: "bauk_done", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "Status" },
            ],
            buttons: [
               { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    extend: 'excelHtml5',
                    filename: 'Detail Sonumb',
                    text: '<i class="fa fa-file-excel-o"></i>',
                    titleAttr: 'Export to Excel',
                    className: 'btn yellow btn-outline',
                    exportOptions: {
                        columns: 'th:not(:first-child)',
                        format: {
                            body: function (data, row, column, node) {
                                return (column <= 4) ? "\0" + data : data;
                            }
                        }
                    }
                },
               { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "scrollY": 500,
            "destroy": true,
            "fnDrawCallback": function (oSettings, json) {

            },
            "footerCallback": function (row, data, start, end, display) {

            },
        });

        $(window).resize(function () {
            $("#tblDetail").DataTable().columns.adjust().draw();
        });

    },

    ShowPICA: function () {
        $('#tbSonumb').val(DataSelected.Selected.SONumber);
        $('#tbSiteID').val(DataSelected.Selected.SiteID);
        $('#tbSiteName').val(DataSelected.Selected.SiteName);
        $('#tbSiteIDOpr').val(DataSelected.Selected.CustomerSiteID);
        $('#tbSiteNameOpr').val(DataSelected.Selected.CustomerSiteName);
    },

    Export: function () {
        window.location.href = "/DashboardRA/ExportToExcelPICA";
    }

}

var Form = {
    Init: function () {
        $('.select2').select2({});
        Table.Init();
        Form.BindingSelectCompany();
        Form.BindingSelectStip();
        Form.BindYear();
        Form.BindingSelectTenant();

        Form.BindingSelectTypeProcess();

        MilestoneStatus = "Init";

        $("#pnlDetail").hide();
        $("#Detail").parsley();
        $("#btDownloadPica").hide();
    },
    BindingSelectCompany: function () {
        var id = "#slSearchCompany"
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Company Name", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectStip: function () {
        var id = "#slSearchStip"
        $.ajax({
            url: "/api/MstDataSource/TBGSys_Stip",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select STIP", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectTenant: function () {
        var id = "#slSearchTenant"
        $.ajax({
            url: "/api/MstDataSource/TenantType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Tenant Type", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectTypeProcess: function () {
        var id = "#slTypeProcess"
        $.ajax({
            url: "/api/DashboardXL/GetTypeProcess",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + item.Process + "'>" + item.Process + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Type Process", width: null });
        })
       .fail(function (jqXHR, textStatus, errorThrown) {
           Common.Alert.Error(errorThrown);
       });
    },

    BindingSelectTypePICA: function () {
        var params = {
            Process: $('#slTypeProcess').val()
        }
        $.ajax({
            url: "/api/DashboardXL/GetTypePICA",
            "type": "POST",
            "datatype": "json",
            "data": params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slTypePICA").html("<option></option>")


            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slTypePICA").append("<option value='" + item.TypePICA + "'>" + item.TypePICA + "</option>");

                })
            }

            $("#slTypePICA").select2({ placeholder: "Select Type PICA", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectCategoryPICA: function () {
        var params = {
            Process: $('#slTypeProcess').val(),
            TypePICA: $('#slTypePICA').val()
        }
        $.ajax({
            url: "/api/DashboardXL/GetCategoryPICA",
            "type": "POST",
            "datatype": "json",
            "data": params
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slCategoryPICA").html("<option></option>")


            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slCategoryPICA").append("<option value='" + item.CategoryPICA + "'>" + item.CategoryPICA + "</option>");

                })
            }

            $("#slCategoryPICA").select2({ placeholder: "Select Category PICA", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindYear: function () {
        var start_year = new Date().getFullYear();
        var id = "#slSearchYear";
        var yearNow = new Date();


        for (var i = start_year - 10; i < start_year ; i++) {
            $(id).append('<option value="' + i + '">  ' + i + '  </option>');
        }

        for (var i = start_year ; i < start_year + 20 ; i++) {
            $(id).append('<option value="' + i + '">  ' + i + '  </option>');
        }

        $(id).select2({ placeholder: "Select Year", width: null });
        $(id).val(yearNow.getFullYear()).trigger('change');
    },


    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlDetail").hide();

    },
    ResetFilter: function () {
        $("#slSearchCompany").val(null).trigger("change");
        $("#slSearchStip").val(null).trigger("change");
        $("#slSearchTenant").val(null).trigger("change");
        $("#slSearchYear").val(null).trigger("change");
    },
    ResetPICA: function () {
        $("#slTypeProcess").val(null).trigger("change");
        $("#slTypePICA").val(null).trigger("change");
        $("#slCategoryPICA").val(null).trigger("change");
        $("#tbRemarks").val('');
    }
}

var Action = {
    SavePICA: function () {
        var params = {
            SONumber: $('#tbSonumb').val(),
            SiteID: $('#tbSiteID').val(),
            SiteName: $('#tbSiteName').val(),
            CustomerSiteID: $('#tbSiteIDOpr').val(),
            CustomerSiteName: $('#tbSiteNameOpr').val(),
            Process: $('#slTypeProcess').val(),
            TypePICA: $('#slTypePICA').val(),
            CategoryPICA: $('#slCategoryPICA').val(),
            Description: $('#tbRemarks').val(),
            SIRO: DataSelected.Selected.SIRO

        }
        $.ajax({
            url: "/api/DashboardXL/InsertDataPICA",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Success to Insert Data");
                $('#mdlPICA').modal('hide');
                Form.ResetPICA();
            } else {
                Common.Alert.Error("Something wrong, Please Contact System Administrator !")
            }
        })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
            });
    }
}