var Data = {};
var fsSONumber = "";
var fsSiteID = "";
var fsSiteName = "";
var fsSiteCustID = "";
var fsSiteCustName = "";
var desc = "";
var descID = "";
var month = "";
var path = "";

jQuery(document).ready(function () {

    Control.Init();
    path = $('#path').val();

    $(".btnSearchDetail").unbind().click(function () {
        Table.Detail(descID, desc, month);
    });
    $('#btSearch').unbind().click(function () {
        Table.Init('#tblSummaryHeader');
        Table.Header();
    });


    $('#btReset').unbind().click(function () {
        $('#sslCustomerID').val('').trigger('change');
        $('#sslCompanyID').val('').trigger('change');
        $('#sslStipID').val('').trigger('change');
        $('#sslGroupBy').val('customer').trigger('change');
        $('#sslRegionID').val('').trigger('change');
        $('#sslProvinceID').val('').trigger('change');
        $('#sslProductID').val('').trigger('change');
        $('#sslPowerTypeID').val('').trigger('change');
        $('#sslBapsType').val('NEW').trigger('change');

        var d = new Date();
        var n = d.getFullYear();
        $('#sslYear').val(n).trigger('change');
    });

    $('#btnBack').unbind().click(function () {
        $('#SummaryDetail').hide();
        $('.filter').show();
        $('#SummaryHeader').show();
        $("#schSONumber").val('');
        $("#schSiteID").val('');
        $("#schSiteName").val('');
        $("#schSiteCustID").val('');
        $("#schSiteCustName").val('');
    });
});

var Control = {
    Init: function () {
        $('#SummaryHeader').hide();
        $('#SummaryDetail').hide();
        Control.BindYear();
        Control.BindGroupBy();
        Control.BindBAPSType();
        Control.BindCustomer();
        Control.BindCompany();
        Control.BindSTIPCategory();
        Control.BindProduct();
        Control.BindPowerType();
        Control.BindProvince();
        Control.BindRegion();
        Data.RowSelected = [];

    },

    BindYear: function () {
        var start_year = new Date().getFullYear();
        var id = "#sslYear";
        var yearNow = new Date();


        for (var i = start_year - 10; i < start_year; i++) {
            $(id).append('<option value="' + i + '">  ' + i + '  </option>');
        }

        for (var i = start_year; i < start_year + 20; i++) {
            $(id).append('<option value="' + i + '">  ' + i + '  </option>');
        }

        $(id).select2({ placeholder: "Select Year", width: null });
        $(id).val(yearNow.getFullYear()).trigger('change');
    },
    BindGroupBy: function () {
        var id = "#sslGroupBy";

        $(id).append('<option value="customer"> Customer </option>');
        $(id).append('<option value="company"> Company </option>');
        $(id).append('<option value="region"> Region </option>');
        $(id).append('<option value="province"> Province </option>');
        $(id).select2({ placeholder: "Select Group", width: null });
        $(id).val('customer').trigger('change');
    },
    BindBAPSType: function () {
        var id = "#sslBapsType";

        $(id).append('<option value="NEW"> BAPS New </option>');
        $(id).append('<option value="SIRO"> BAPS SIRO </option>');
        $(id).append('<option value="POWER"> BAPS Power </option>');
        $(id).select2({ placeholder: "Select BAPS", width: null });
        $(id).val('NEW').trigger('change');
    },
    BindCustomer: function () {
        var id = "#sslCustomerID";
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + $.trim(item.OperatorId) + "'>" + item.Operator + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Customer", width: null });
            }).fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });

    },
    BindCompany: function () {
        var id = "#sslCompanyID";
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>");

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.CompanyId) + "'>" + item.Company + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Company Name", width: null });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindSTIPCategory: function () {
        var id = "#sslStipID";
        $.ajax({
            url: "/api/MstDataSource/TBGSys_Stip",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.Value) + "'>" + item.Text + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select STIP", width: null });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindProduct: function () {
        var id = "#sslProductID";
        $.ajax({
            url: "/api/MstDataSource/TenantType",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.Value) + "'>" + item.Text + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Tenant", width: null });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindPowerType: function () {
        var id = "#sslPowerTypeID";
        $.ajax({
            url: "/api/MstDataSource/PowerType",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.KodeType) + "'>" + item.PowerType + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Power", width: null });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindProvince: function () {
        var id = "#sslProvinceID";
        $.ajax({
            url: "/api/MstDataSource/Province",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.ProvinceID) + "'>" + item.ProvinceName + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Province", width: null });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindRegion: function () {
        var id = "#sslRegionID";
        $.ajax({
            url: "/api/MstDataSource/Region",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.RegionID) + "'>" + item.RegionName + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Region", width: null });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
}

var Table = {
    Init: function (id) {
        if (id == "#tblSummaryHeader") {
            $("#tblSummaryHeader thead tr").remove();

            if ($('input[name=rdDataType]:checked').val() == "1")
                var col = "10";
            else
                var col = "100";

            var append = '<tr>' +
                '<th class="datatable-col-10">No</th>' +
                '<th class="datatable-col-200">Description</th>' +
                '<th class="datatable-col-' + col + '">Jan</th>' +
                '<th class="datatable-col-' + col + '">Feb</th>' +
                '<th class="datatable-col-' + col + '">Mar</th>' +
                '<th class="datatable-col-' + col + '">Apr</th>' +
                '<th class="datatable-col-' + col + '">May</th>' +
                '<th class="datatable-col-' + col + '">Jun</th>' +
                '<th class="datatable-col-' + col + '">Jul</th>' +
                '<th class="datatable-col-' + col + '">Aug</th>' +
                '<th class="datatable-col-' + col + '">Sep</th>' +
                '<th class="datatable-col-' + col + '">Oct</th>' +
                '<th class="datatable-col-' + col + '">Nov</th>' +
                '<th class="datatable-col-' + col + '">Dec</th>' +
                '<th class="datatable-col-' + col + '">Total</th>' +
                '<th></th>' +
                '</tr>'
            $("#tblSummaryHeader thead").append(append);
        }
        var tbl = $(id).dataTable({
            "filter": false,
            "destroy": true,
            "data": [],
            "proccessing": true,
            "language": {
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
        });
        $(window).resize(function () {
            $(id).DataTable().columns.adjust().draw();
        });
    },

    Header: function () {
        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();


        var params = {
            CustomerID: $("#sslCustomerID").val() == null ? "" : $("#sslCustomerID").val(),
            CompanyID: $("#sslCompanyID").val() == null ? "" : $("#sslCompanyID").val(),
            STIPID: $("#sslStipID").val() == null ? "" : $("#sslStipID").val(),
            Year: $("#sslYear").val() == null ? "" : $("#sslYear").val(),
            RegionID: $("#sslRegionID").val() == null ? "" : $("#sslRegionID").val(),
            ProvinceID: $("#sslProvinceID").val() == null ? "" : $("#sslProvinceID").val(),
            ProductID: $("#sslProductID").val() == null ? "" : $("#sslProductID").val(),
            PowerTypeID: $("#sslPowerTypeID").val() == null ? "" : $("#sslPowerTypeID").val(),
            BapsType: $("#sslBapsType").val() == null ? "" : $("#sslBapsType").val(),
            GroupBy: $("#sslGroupBy").val() == null ? "" : $("#sslGroupBy").val(),
            DataType: $('input[name=rdDataType]:checked').val(),
        };
        var tblList = $("#tblSummaryHeader").DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "paging": false,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/MonitoringBapDone/summaryHeader",
                "type": "POST",
                "data": params,
                "cache": false,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.ExportHeader();
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "destroy": true,
            "bInfo": false,
            "columns": [
                { data: "RowIndex" },
                {
                    data: "Descrip",
                    render: function (val, type, full) {
                        return Helper.RenderText(full);
                    }
                },
                {
                    data: "Jan",
                    render: function (val, type, full) {
                        return Helper.RenderLink(val, full, "1");
                    }
                },
                {
                    data: "Feb",
                    render: function (val, type, full) {
                        return Helper.RenderLink(val, full, "2");
                    }
                },
                {
                    data: "Mar",
                    render: function (val, type, full) {
                        return Helper.RenderLink(val, full, "3");
                    }
                },
                {
                    data: "Apr",
                    render: function (val, type, full) {
                        return Helper.RenderLink(val, full, "4");
                    }
                },
                {
                    data: "Mei",
                    render: function (val, type, full) {
                        return Helper.RenderLink(val, full, "5");
                    }
                },
                {
                    data: "Jun",
                    render: function (val, type, full) {
                        return Helper.RenderLink(val, full, "6");
                    }
                },
                {
                    data: "Jul",
                    render: function (val, type, full) {
                        return Helper.RenderLink(val, full, "7");
                    }
                },
                {
                    data: "Agu",
                    render: function (val, type, full) {
                        return Helper.RenderLink(val, full, "8");
                    }
                },
                {
                    data: "Sep",
                    render: function (val, type, full) {
                        return Helper.RenderLink(val, full, "9");
                    }
                },
                {
                    data: "Okt",
                    render: function (val, type, full) {
                        return Helper.RenderLink(val, full, "10");
                    }
                },
                {
                    data: "Nov",
                    render: function (val, type, full) {
                        return Helper.RenderLink(val, full, "11");
                    }
                },
                {
                    data: "Des",
                    render: function (val, type, full) {
                        return Helper.RenderLink(val, full, "12");
                    }
                },
                {
                    data: "TotalSite",
                    render: function (val, type, full) {
                        return Helper.RenderLink(val, full, "13");
                    }
                },
                { data: "DescripID" }

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [15], "visible": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblList.data())) {
                    $(".panelSearchResult").fadeIn(1000);
                }
                l.stop(); App.unblockUI('.panelSearchResult');
                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        if (!Helper.IsElementExistsInArray(parseInt(item), Data.RowSelectedSite))
                            $("#Row" + item).addClass("active");
                    }
                }
            },
            "order": [1, "asc"],
            //"scrollY": 300,
            //"scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            //"scrollCollapse": true,
            //"fixedColumns": {
            //    leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            //},
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
        });

        $("#tblSummaryHeader tbody").unbind().on("click", "a.btDetail", function (e) {
            $('.filter').hide();
            $('#SummaryHeader').hide();
            desc = $(this).attr('desc');
            descID = $(this).attr('descID');
            month = $(this).attr('month');

            Table.Detail(descID, desc, month);
            e.preventDefault();
            $('#SummaryDetail').show();
        });
        $('#SummaryHeader').show();
        $('#SummaryDetail').hide();
    },

    Detail: function (descID, desc, month) {
        Table.Init('#tblSummaryDetail');
        fsSONumber = $("#schSONumber").val();
        fsSiteID = $("#schSiteID").val();
        fsSiteName = $("#schSiteName").val();
        fsSiteCustID = $("#schSiteCustID").val();
        fsSiteCustName = $("#schSiteCustName").val();

        var params = {
            CustomerID: $("#sslCustomerID").val() == null ? "" : $("#sslCustomerID").val(),
            CompanyID: $("#sslCompanyID").val() == null ? "" : $("#sslCompanyID").val(),
            STIPID: $("#sslStipID").val() == null ? "" : $("#sslStipID").val(),
            Year: $("#sslYear").val() == null ? "" : $("#sslYear").val(),
            RegionID: $("#sslRegionID").val() == null ? "" : $("#sslRegionID").val(),
            ProvinceID: $("#sslProvinceID").val() == null ? "" : $("#sslProvinceID").val(),
            ProductID: $("#sslProductID").val() == null ? "" : $("#sslProductID").val(),
            PowerTypeID: $("#sslPowerTypeID").val() == null ? "" : $("#sslPowerTypeID").val(),
            BapsType: $("#sslBapsType").val() == null ? "" : $("#sslBapsType").val(),
            GroupBy: $("#sslGroupBy").val() == null ? "" : $("#sslGroupBy").val(),
            Desc: desc,
            DescID: descID,
            Month: month,
            schSONumber: fsSONumber,
            schSiteID: fsSiteID,
            schSiteName: fsSiteName,
            schSiteCustID: fsSiteCustID,
            schSiteCustName: fsSiteCustName
        };

        var tblList = $("#tblSummaryDetail").DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "bSortCellsTop": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/MonitoringBapDone/summaryDetail",
                "type": "POST",
                "data": params,
                "cache": false,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.ExportDetail(descID, month);
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                { data: "RowIndex" }, //0
                { data: "SoNumber" }, //1
                { data: "SiteID" }, //2
                { data: "SiteName" }, //3
                { data: "CustomerSiteID" }, //4
                { data: "CustomerSiteName" }, //5
                { data: "TenantType" }, //6
                { data: "StipSiro" }, //7
                { data: "StipCategory" }, //8
                { data: "Company" }, //9
                { data: "CustomerId" }, //10
                { data: "RegionName" }, //11
                { data: "ProvinceName" }, //12
                { data: "ResidenceName" }, //13
                {
                    data: "RFIDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                }, // 14
                {
                    data: "BAUKDone", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                }, //15
                {
                    data: "PODone", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                }, //16
                { data: "PONumber" }, //13
                {
                    data: "BapsDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                }, //17
                {
                    data: "BapsDoneDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                }, //18
                {
                    data: "BapsAmount", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                }, //
                {
                    data: "BAPSNumber"
                }, //19BAPSNumber

                //Added by ASE
                {
                    data: "StartLeasedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndLeasedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                
                {
                    data: "StartInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "BAPSReceiveDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "BAPSConfirmDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                
                
                {
                    data: "CreateInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },//26
                {
                    data: "PostingInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },//27
                {
                    data: "InvoiceNumber"
                },//28
                {
                    render: function (data, type, full) {
                        if (full.BAPSDocumentFileName != null && full.BAPSDocumentFileName != '')
                            return '<button type="button" class="btn btn-xs btn-primary btn-downloadBAPS" id="btDownload_' + full.SoNumber + '"><i class="fa fa-download"></i></button>';
                        else
                            return '';
                    }
                },//29
                {
                    render: function (data, type, full) {
                        //if (full.BAUKDocument != null && full.BAUKDocument != '')
                        return '<button type="button" class="btn btn-xs btn-primary btn-downloadBAUK text-center" id="btDownload_' + full.SoNumber + '"><i class="fa fa-download"></i></button>';
                        //else
                        //    return '';
                    }
                },//30
                { data: "CustomerId" },//32
                { data: "BAPSDocumentFilePath" },//33
                { data: "BAPSDocumentFileName" },//34
                { data: "BAPSDocumentContentType" },//35
            ],
            "columnDefs": [{ "targets": [0], "orderable": false },
                { "targets": [31], "className": "text-center" },
                { "targets": [32], "className": "text-center" },
            { "targets": [33], "visible": false },
            { "targets": [34], "visible": false },
            { "targets": [35], "visible": false },
            { "targets": [36], "visible": false }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [1, "asc"],
            "scrollY": 300,
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            //"fixedColumns": {
            //    leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            //},
        });

        $("#tblSummaryDetail tbody").on("click", "button.btn-downloadBAUK", function (e) {
            var table = $("#tblSummaryDetail").DataTable();
            var row = table.row($(this).parents('tr')).data();
            Table.GridDocumentBAUK(row.SoNumber, row.SiteID, row.CustomerId);
            //Helper.Download(row.FilePathConfirm, row.FileNameConfirm, row.ContentTypeConfirm);
        });

        $("#tblSummaryDetail tbody").on("click", "button.btn-downloadBAPS", function (e) {
            var table = $("#tblSummaryDetail").DataTable();
            var row = table.row($(this).parents('tr')).data();

            var docPath = path + row.BAPSDocumentFilePath;
            window.location.href = "/Admin/Download?path=" + docPath + "&fileName=" + row.BAPSDocumentFileName + "&contentType=" + row.BAPSDocumentContentType;
            //window.location.href = docPath;
            //window.open(
            //    docPath,
            //    '_blank' // <- This is what makes it open in a new window.
            //);

            //Helper.Download(row.FilePathConfirm, row.FileNameConfirm, row.ContentTypeConfirm);
        });
    },

    GridDocumentBAUK: function (SoNumber, SiteID, CustomerID) {

        var params = {
            strSoNumber: SoNumber,
            strSiteId: SiteID,
            strCustomerId: CustomerID
        };
        var tbl = $("#tblDocument").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "paging": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/CheckingDoc/getCheckDocList",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            "filter": false,
            "destroy": true,

            "columns": [
                { data: "RowIndex" }
                , {
                    data: "DocName",
                    mRender: function (data, type, full) {
                        return "<a href='#' class='btn-Download' id='btDownload_" + full.RowIndex + "'>" + full.DocName + "</a>"
                    }
                }
                , { data: "FileName" }
                //, { data: "LinkFile" }
            ],
            "columnDefs": [
                { "targets": [0], "className": "text-center" },
                //{ "targets": [3], "visible": false }
            ],
            "fnDrawCallback": function () {

            }
        });
        $("#tblDocument tbody").unbind();
        $("#tblDocument tbody").on("click", ".btn-Download", function (e) {
            var table = $("#tblDocument").DataTable();
            var row = table.row($(this).parents('tr')).data();
            var IsLegacy = row.IsLegacy;

            Helper.DownloadDoc(row.LinkFile, row.FileName, IsLegacy, "application/pdf");
        });
        $('#mdlDocument').modal('show');
    },
    ExportHeader: function () {
        var CustomerID = $("#sslCustomerID").val() == null ? "" : $("#sslCustomerID").val();
        var CompanyID = $("#sslCompanyID").val() == null ? "" : $("#sslCompanyID").val();
        var STIPID = $("#sslStipID").val() == null ? "" : $("#sslStipID").val();
        var Year = $("#sslYear").val() == null ? "" : $("#sslYear").val();
        var RegionID = $("#sslRegionID").val() == null ? "" : $("#sslRegionID").val();
        var ProvinceID = $("#sslProvinceID").val() == null ? "" : $("#sslProvinceID").val();
        var ProductID = $("#sslProductID").val() == null ? "" : $("#sslProductID").val();
        var PowerTypeID = $("#sslPowerTypeID").val() == null ? "" : $("#sslPowerTypeID").val();
        var BapsType = $("#sslBapsType").val() == null ? "" : $("#sslBapsType").val();
        var GroupBy = $("#sslGroupBy").val() == null ? "" : $("#sslGroupBy").val();

        window.location.href = "/DashboardRA/MonitoringBapsDoneHeader/Export?groupBy=" + GroupBy + "&bapsType=" + BapsType + "&customerID=" + CustomerID +
            "&companyID=" + CompanyID + "&stipID=" + STIPID + "&regionID=" + RegionID + "&provinceID=" + ProvinceID + "&productID=" + ProductID +
            "&powerTypeID=" + PowerTypeID + "&year=" + Year + "";
    },
    ExportDetail: function (descID, month) {
        var CustomerID = $("#sslCustomerID").val() == null ? "" : $("#sslCustomerID").val();
        var CompanyID = $("#sslCompanyID").val() == null ? "" : $("#sslCompanyID").val();
        var STIPID = $("#sslStipID").val() == null ? "" : $("#sslStipID").val();
        var Year = $("#sslYear").val() == null ? "" : $("#sslYear").val();
        var RegionID = $("#sslRegionID").val() == null ? "" : $("#sslRegionID").val();
        var ProvinceID = $("#sslProvinceID").val() == null ? "" : $("#sslProvinceID").val();
        var ProductID = $("#sslProductID").val() == null ? "" : $("#sslProductID").val();
        var PowerTypeID = $("#sslPowerTypeID").val() == null ? "" : $("#sslPowerTypeID").val();
        var BapsType = $("#sslBapsType").val() == null ? "" : $("#sslBapsType").val();
        var GroupBy = $("#sslGroupBy").val() == null ? "" : $("#sslGroupBy").val();
        var Month = month;
        var DescID = descID;
        var schSONumber = fsSONumber;
        var schSiteID = fsSiteID;
        var schSiteName = fsSiteName;
        var schSiteCustID = fsSiteCustID;
        var schSiteCustName = fsSiteCustName;

        window.location.href = "/DashboardRA/MonitoringBapsDoneDetail/Export?groupBy=" + GroupBy + "&bapsType=" + BapsType + "&customerID=" + CustomerID +
            "&companyID=" + CompanyID + "&stipID=" + STIPID + "&regionID=" + RegionID + "&provinceID=" + ProvinceID + "&productID=" + ProductID +
            "&powerTypeID=" + PowerTypeID + "&year=" + Year + "&month=" + Month + "&descID=" + DescID + "&schSONumber=" + schSONumber + "&schSiteID=" + schSiteID
            + "&schSiteName=" + schSiteName + "&schSiteCustID=" + schSiteCustID + "&schSiteCustName=" + schSiteCustName + "";
    },

}

var Helper = {
    RenderLink: function (val, full, month) {
        if ($('input[name=rdDataType]:checked').val() == '1') {
            if (full.RowIndex == "" || month == "13")
                return "<center><a style='font-weight:bold' class='btDetail' desc='" + full.Descrip + "' descID='" + full.DescripID + "' month='" + month + "'>" + val + "</a></center>";
            else
                return "<center><a class='btDetail' desc='" + full.Descrip + "' descID='" + full.DescripID + "' month='" + month + "'>" + val + "</a></center>";
        }

        else {
            if (full.RowIndex == "" || month == "13")
                return "<div class='text-right'><a style='font-weight:bold; text-align:right' class='btDetail' desc='" + full.Descrip + "' descID='" + full.DescripID + "' month='" + month + "'>" + Common.Format.CommaSeparation(val) + "</a></div>";
            else
                return "<div class='text-right'><a class='btDetail' desc='" + full.Descrip + "' descID='" + full.DescripID + "' month='" + month + "'>" + Common.Format.CommaSeparation(val) + "</a></div>";
        }

    },

    RenderText: function (full) {
        if (full.RowIndex == "") {
            return "<b>" + full.Descrip + "</b>";
        } else {
            return full.Descrip;
        }
    },
    DownloadDoc: function (filePath, Document, IsLegacy, contentType) {
        var path = filePath;
        window.location.href = "/RevenueAssurance/DownloadFileProject?FilePath=" + path + "&FileName=" + Document + "&ContentType=" + contentType + "&IsLegacy=" + IsLegacy;
    },
}