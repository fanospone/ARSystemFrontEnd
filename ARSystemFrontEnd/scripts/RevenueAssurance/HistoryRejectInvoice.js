
jQuery(document).ready(function () {
    Control.Init();
    Control.SetParams();
    Table.Init();
    Table.Search();
});

var Control = {
    Init: function () {
        Control.BindingSelectPowerType();
        Control.BindingSelectDepartment();
        Control.BindingSelectYear($('#slYear'));
        Control.BindingSelectMonth($('#slMonth'));
        $("#slSONumberMultiple").select2({
            tags: true,
            multiple: true,
            width: '100%',
            createTag: function (params) {
                return {
                    id: params.term,
                    text: params.term,
                    newOption: true
                }
            },
            templateResult: function (data) {
                var $result = $("<span></span>");
                $result.text(data.text);
                if (data.newOption) {
                    $result.append(" <em>(add)</em>");
                }
                return $result;
            }
        });

        $("#slReconcileType").select2({ placeholder: "Select Reconcile Type", width: null });

        //Action
        $("#btSearch").unbind().click(function () {
            Control.SetParams();
            Table.Search();
        });

        $("#btReset").unbind().click(function () {
            Control.Reset();
        });
    },

    BindingSelectPowerType: function () {
        $.ajax({
            url: "/api/mstDataSource/PowerType",
            type: "GET"
        })
       .done(function (data, textStatus, jqXHR) {
           $('#slPowerType').html("<option></option>")
           if (Common.CheckError.List(data)) {
               $.each(data, function (i, item) {
                   $('#slPowerType').append("<option value='" + parseInt($.trim(item.KodeType)) + "'>" + item.PowerType + "</option>");
               })
           }
           $('#slPowerType').select2({ placeholder: "Select Power Type", width: null });
       })
       .fail(function (jqXHR, textStatus, errorThrown) {
           Common.Alert.Error(errorThrown);
       });
    },

    BindingSelectDepartment: function () {
        $.ajax({
            url: "/api/mstDataSource/DepartmentHumanCapital",
            type: "GET"
        })
        .done(function (data, textstatus, jqxhr) {
            $("#slDepartmentName").html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slDepartmentName").append("<option value='" + item.DepartmentCode + "'>" + item.DepartmentName + "</option>");
                });
            }
            $("#slDepartmentName").select2({ placeholder: "Select Department", width: null });
        })
        .fail(function (jqxhr, textstatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectYear: function (elements) {
        var dt = new Date();
        var currentyear = dt.getFullYear();
        elements.html("");
        elements.append("<option>All</option>");
        for (var i = -10; i <= 10; i++) {
            if (currentyear == (currentyear + i))
                elements.append("<option value='" + (currentyear + i) + "' selected >" + (currentyear + i) + "</option>");
            else
                elements.append("<option value='" + (currentyear + i) + "'>" + (currentyear + i) + "</option>");

        }

        elements.select2({ placeholder: "Select Year", width: null });
    },

    BindingSelectMonth: function (elements) {
        var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        elements.html("");
        elements.append("<option>All</option>");
        var dt = new Date();
        var currentmonth = dt.getMonth();
        for (var i = 0; i < monthNames.length; i++) {
            if (currentmonth == (i))
                elements.append("<option value='" + (i + 1) + "' selected>" + monthNames[i] + "</option>");
            else
                elements.append("<option value='" + (i + 1) + "'>" + monthNames[i] + "</option>");
        }
        elements.select2({ placeholder: "Select Month", width: null });
    },

    SetParams: function () {
        params = {
            vReconcileType: $('#slReconcileType').val(),
            vPowerType: $('#slPowerType').val(),
            vDepartmentCode: $('#slDepartmentName').val(),
            vSONumber: $('#slSONumberMultiple').val(),
            vYear: $('#slYear').val(),
            vMonth: $('#slMonth').val()
        }
    },

    Reset: function () {
        $("#slReconcileType").val("").trigger('change');
        $("#slPowerType").val("").trigger('change');
        $("#slDepartmentName").val("").trigger('change');
        $("#slSONumberMultiple").val("");
        Control.BindingSelectYear($('#slYear'));
        Control.BindingSelectMonth($('#slMonth'));
    }
}

var Table = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblSummary = $('#tblSummary').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblSummary").DataTable().columns.adjust().draw();
        });
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var tblSummary = $("#tblSummary").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/HistoryRejectInvoice/grid",
                "type": "POST",
                "datatype": "json",
                "data": params
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
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
                { data: "Source" },
                { data: "SONumber" },
                { data: "SiteIdOld" },
                { data: "SiteName" },
                { data: "ReconcileType" },
                { data: "PowerType" },
                { data: "DepartmentName" },
                { data: "RejectYear" },
                { data: "RejectMonth" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CustomerInvoice" },
                { data: "CompanyInvoice" },
                { data: "InvNo" },
                {
                    data: "StartDateInvoice", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndDateInvoice", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "AmountRental", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "AmountService", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "BaseLeaseCurrency" },
                { data: "ServiceCurrency" },
                { data: "AmountIDR" },
                { data: "AmountUSD" },
                { data: "Product" },
                { data: "BapsType" },
                { data: "StipSiro" },
                { data: "PicaType" },
                { data: "PicaMajor" },
                { data: "PicaDetail" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummary.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").show();
                }
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            "columnDefs": [
                    { "targets": [0, 1, 2, 4, 5, 7, 8, 9, 11, 12, 13, 14, 15, 18, 19, 22, 23, 24, 25], "className": "dt-center" },
                    { "targets": [16, 17, 20, 21, 26], "className": "dt-right" },
                    { "targets": [3, 6, 10, 13, ], "className": "dt-left" }
            ],
            "order": []
        });
    },

    Export: function () {
        Control.SetParams();
        window.location.href = "/RevenueAssurance/HistoryReject/Export?" + $.param(params);
    }
}