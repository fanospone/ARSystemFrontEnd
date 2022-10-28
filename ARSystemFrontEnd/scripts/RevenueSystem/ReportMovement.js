Data = {};

jQuery(document).ready(function () {
    Form.Init();
    $(".summary").fadeIn();
    $(".detail").hide();

    $("#btBackSummary").unbind().click(function (e) {
        $(".summary").fadeIn();
        $(".detail").hide();
    });
});

var Form = {
    Init: function () {
        
        Control.BindingSelectYear($('#fYear'));
        Control.BindingSelectMonth($('#fMonth'));
        Table.ReportSummary();
        Control.BindingSelectCompany();
        Control.BindingSelectRegional();
        Control.BindingSelectOperator();
        Control.BindingSelectProduct();
        $("#btSearch").unbind().click(function () {
            $(".summary").fadeIn();
            $(".detail").hide();

            Table.ReportSummary();
        });

        $("#btReset").unbind().click(function () {
            Control.Reset();
        });

        $(".btnSearchHeader").unbind().click(function () {
            var params = {
                fDesc: Data.Selected.Description,
                fYear: $('#fYear').val() == null ? "" : $('#fYear').val(),
                fMonth: $('#fMonth').val() == null ? "" : $('#fMonth').val(),
                fCompanyId: $('#fCompany').val() == null ? "" : $('#fCompany').val(),
                fRegionalName: $('#fRegional').val() == null ? "" : $('#fRegional').val(),
                fOperatorId: $('#fOperator').val() == null ? "" : $('#fOperator').val(),
                fProduct: $('#fProduct').val() == null ? "" : $('#fProduct').val(),
                fGroupBy: $('input[name=rbGroupBy]:checked').val(),
                fViewBy: $('input[name=rbViewBy]:checked').val(),
                fSoNumber: $("#SONumberSearch").val(),
                fSiteID: $("#SiteIDSearch").val(),
                fSiteName: $("#SiteNameSearch").val()
            };

            TableDetail.Detail(params);
        });


    }
}

var Table = {
    ReportSummary: function () {
        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();
        var params = {
            fYear: $('#fYear').val(),
            fMonth: $('#fMonth').val(),
            fCompanyId: $('#fCompany').val(),
            fRegionalName: $('#fRegional').val(),
            fOperatorId: $('#fOperator').val(),
            fProduct: $('#fProduct').val(),
            fGroupBy: $('input[name=rbGroupBy]:checked').val(),
            fViewBy: $('input[name=rbViewBy]:checked').val()
        }
        var tblList = $("#tblReportSummary").DataTable({
            "deferRender": true,
            "proccessing": true,
            //"serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/RevenueSystem/GetSummaryMovement",
                "type": "POST",
                "datatype": "json",
                "data": params,
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
            "order": [],
            "columns": [
                   { data: null },
                   {
                       data: "Description", mRender: function (data, type, full) {
                           return "<a class='btDetail'>" + data + "</a>";
                       }

                   },
                   {
                       data: "Previous", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "Current", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "Movement", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "Percentage", className: "text-right", render: function (data) {
                           return data + ' %';
                       }
                   },
                   {
                       data: "prevAccrueOverquotaISAT", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "prevCancelDiscount", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "prevDiffMLnv", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "prevDiskon", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "prevHoldAccrueNew", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "prevHoldAccrueRenewal", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "prevInflasi", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "prevKurs", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "prevNewPriceNew", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "prevNewPriceRenewal", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "prevNewSLD", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "prevOverBlast", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "prevReAktifHoldAccrue", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "prevRelokasi", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "prevReStopAccrue", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "prevSharingRevenue", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "prevSLDBAPSInvoice", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "prevStopAccrue", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "prevTemporaryFree", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "AccrueOverquotaISAT", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "CancelDiscount", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                   {
                       data: "DiffMLnv", className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   },
                    {
                        data: "Diskon", className: "text-right", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    },
                    {
                        data: "HoldAccrueNew", className: "text-right", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    },
                    {
                        data: "HoldAccrueRenewal", className: "text-right", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    },
                    {
                        data: "Inflasi", className: "text-right", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    },
                    {
                        data: "Kurs", className: "text-right", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    },
                    {
                        data: "NewPriceNew", className: "text-right", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    },
                    {
                        data: "NewPriceRenewal", className: "text-right", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    },
                    {
                        data: "NewSLD", className: "text-right", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    },
                    {
                        data: "OverBlast", className: "text-right", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    },
                    {
                        data: "ReAktifHoldAccrue", className: "text-right", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    },
                    {
                        data: "Relokasi", className: "text-right", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    },
                    {
                        data: "ReStopAccrue", className: "text-right", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    },
                    {
                        data: "SharingRevenue", className: "text-right", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    },
                    {
                        data: "SLDBAPSInvoice", className: "text-right", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    },
                    {
                        data: "StopAccrue", className: "text-right", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    },
                    {
                        data: "TemporaryFree", className: "text-right", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    },
                    {
                        data: "Total", className: "text-right", render: function (data) {
                            return Common.Format.CommaSeparation(data);
                        }
                    },
                    {
                        data: "GroupID", render: function (val, type, full) {
                            return "<b>" + val + "</b>";
                        }
                    },

            ],
            "columnDefs": [
                { "targets": [0], "orderable": false },
                { "targets": [45], "visible": false },
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fixedColumns": {
                leftColumns: 1
            },
            "fnDrawCallback": function (result) {
                if (result.json != undefined && result.json.data != undefined) {
                    if (Common.CheckError.List(this.fnGetData())) {
                        $(".panelSearchResult").fadeIn(1000);
                    }
                    l.stop(); App.unblockUI('.panelSearchResult');
                }
            },
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;
                var colNumber = [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
                                    25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44];

                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                        i : 0;
                };

                var numformat = $.fn.dataTable.render.number(',', '.', 2, '').display;

                for (i = 0; i < colNumber.length; i++) {

                    var colNo = colNumber[i];

                    TotalAmount = api
                            .column(colNo, { page: 'all' })
                            .data()
                            .reduce(function (a, b) {
                                return intVal(a) + intVal(b);
                            }, 0);
                    full = {};
                    full.GroupID = "Total";
                    $(api.column(colNo).footer()).html(Common.Format.CommaSeparation(TotalAmount));
                }

            }
        });

        //RowNumber No.
        tblList.on('draw.dt', function () {
            var PageInfo = $('#tblReportSummary').DataTable().page.info();
            tblList.column(0, { page: 'current' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1 + PageInfo.start;
            });
        });

        $("#tblReportSummary tbody").on("click", "a.btDetail", function (e) {
            var table = $("#tblReportSummary").DataTable();
            Data.Selected = {};
            Data.Selected = table.row($(this).parents('tr')).data();

            var params = {
                fDesc: Data.Selected.GroupID,
                fYear: $('#fYear').val() == null ? "" : $('#fYear').val(),
                fMonth: $('#fMonth').val() == null ? "" : $('#fMonth').val(),
                fCompanyId: $('#fCompany').val() == null ? "" : $('#fCompany').val(),
                fRegionalName: $('#fRegional').val() == null ? "" : $('#fRegional').val(),
                fOperatorId: $('#fOperator').val() == null ? "" : $('#fOperator').val(),
                fProduct: $('#fProduct').val() == null ? "" : $('#fProduct').val(),
                fGroupBy: $('input[name=rbGroupBy]:checked').val(),
                fViewBy: $('input[name=rbViewBy]:checked').val(),
                fSoNumber: $("#SONumberSearch").val(),
                fSiteID: $("#SiteIDSearch").val(),
                fSiteName: $("#SiteNameSearch").val()
            };


            TableDetail.Detail(params);
            $(".summary").hide();
            $(".detail").fadeIn();
        });
    },

    Export: function () {
        var fYear = $('#fYear').val();
        var fMonth = $('#fMonth').val();
        var fCompanyId = $('#fCompany').val();
        var fRegionalName = $('#fRegional').val();
        var fOperatorId = $('#fOperator').val();
        var fProduct = $('#fProduct').val();
        var fGroupBy = $('input[name=rbGroupBy]:checked').val();
        var fViewBy = $('input[name=rbViewBy]:checked').val();
        window.location.href = "/RevenueSystem/SummaryMovement/Export?strViewBy=" + fViewBy + "&strGroupBy=" + fGroupBy + "&intYear=" + fYear + "&intMonth=" + fMonth + "&strCompanyId=" + fCompanyId
            + "&strRegionName=" + fRegionalName + "&strOperatorId=" + fOperatorId + "&strProduct=" + fProduct;
    }
}

var TableDetail = {
    Detail: function (params) {
        //var l = Ladda.create(document.querySelector("#btSearch"))
        //l.start();

        $.ajax({
            url: "/api/RevenueSystem/GetMovementDetail",
            type: "POST",
            datatype: "json",
            data: params
        })
        .done(function (data, textStatus, jqXHR) {
            var tblSummaryData = $("#tblDetail").DataTable({
                "proccessing": true,
                "serverSide": false,
                "bSortCellsTop": true,
                //"sScrollX": "100%",
                //"colReorder": true,
                "language": {
                    "emptyTable": "No data available in table",
                },
                "data": data.data,
                buttons: [
                    { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                    {
                        text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                            var l = Ladda.create(document.querySelector(".yellow"));
                            l.start();
                            TableDetail.Export(params);
                            l.stop();
                        }
                    },
                    { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
                ],
                //"fixedColumns": {
                //    leftColumns: 2 /* Set the 2 most left columns as fixed columns */
                //},
                "filter": false,
                "lengthMenu": [[10, 25, 50, -1], ['10', '25', '50', 'All']],
                "destroy": true,
                "columns": [
                    { data: "SoNumber" },
                    { data: "SiteID" },
                    { data: "SiteName" },
                    { data: "RegionName" },
                    { data: "CustomerID" },
                    { data: "CompanyID" },
                    {
                        data: "RFIDate", render: function (data) {
                            return Common.Format.ConvertJSONDateTime(data);
                        }
                    },
                    {
                        data: "SLDDate", render: function (data) {
                            return Common.Format.ConvertJSONDateTime(data);
                        }
                    },
                    {
                        data: "StartBapsDate", render: function (data) {
                            return Common.Format.ConvertJSONDateTime(data);
                        }
                    },
                    { data: "PreviousMonth" },
                    { data: "CurrentMonth" },
                    { data: "Movement" }
                ],
                "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                //"fnDrawCallback": function () {
                //    l.stop();
                //},
                'scrollCollapse': true,
                'aoColumnDefs': [{
                    'bSortable': false,
                    'aTargets': []
                }],
                'order': []
            });
        })
        .fail(function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            //console.log(err.Message);
        });
    },

    Export: function () {
        var fDesc = Data.Selected.GroupID;
        var fYear = $('#fYear').val();
        var fMonth = $('#fMonth').val();
        var fCompanyId = $('#fCompany').val();
        var fRegionalName = $('#fRegional').val();
        var fOperatorId = $('#fOperator').val();
        var fProduct = $('#fProduct').val();
        var fGroupBy = $('input[name=rbGroupBy]:checked').val();
        var fViewBy = $('input[name=rbViewBy]:checked').val();
        var fSoNumber = $("#SONumberSearch").val();
        var fSiteID = $("#SiteIDSearch").val();
        var fSiteName = $("#SiteNameSearch").val();
        window.location.href = "/RevenueSystem/SummaryMovementDetail/Export?strViewBy=" + fViewBy + "&strGroupBy=" + fGroupBy
            + "&intYear=" + fYear + "&intMonth=" + fMonth + "&strCompanyId=" + fCompanyId + "&strRegionName=" + fRegionalName
            + "&strOperatorId=" + fOperatorId + "&strProduct=" + fProduct + "&strDesc=" + fDesc + "&strSoNumber=" + fSoNumber
            + "&strSiteID=" + fSiteID + "&strSiteName=" + fSiteName;
    }
}

var Control = {
    BindingSelectYear: function (elements) {
        var dt = new Date();
        var currentyear = dt.getFullYear();

        elements.html("");

        for (var i = -5; i <= 5; i++) {
            if (currentyear == (currentyear + i))
                elements.append("<option value='" + (currentyear + i) + "' selected >" + (currentyear + i) + "</option>");
            else
                elements.append("<option value='" + (currentyear + i) + "'>" + (currentyear + i) + "</option>");

        }

        elements.select2({ placeholder: "Select Year", width: null });
    },
    BindingSelectMonth: function (elements) {
        var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        var dt = new Date();
        var currentMonth = dt.getMonth();

        elements.html("");

        for (var i = 0; i < monthNames.length; i++) {
            if (currentMonth == i)
                elements.append("<option value='" + (i + 1) + "' selected >" + monthNames[i] + "</option>");
            else
                elements.append("<option value='" + (i + 1) + "'>" + monthNames[i] + "</option>");
        }

        elements.select2({ placeholder: "Select Month", width: null });
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
            }
            $(id).select2({ placeholder: "Select Product", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    Reset: function () {
        //$('#fYear').val(null).trigger('change');
        //$('#fMonth').val(null).trigger('change');
        $('#fCompany').val(null).trigger('change');
        $('#fRegional').val(null).trigger('change');
        $('#fOperator').val(null).trigger('change');
        $('#fProduct').val(null).trigger('change');
    },
}

var Helper = {
    //RenderLink: function (val, full, status) {

    //    var desc = full.Description;
    //    var fYear = $('#fYear').val() == null ? "" : $('#fYear').val();
    //    var fMonth = $('#fMonth').val() == null ? "" : $('#fMonth').val();
    //    var fCompanyId = $('#fCompany').val() == null ? "" : $('#fCompany').val();
    //    var fRegionalName = $('#fRegional').val() == null ? "" : $('#fRegional').val();
    //    var fOperatorId = $('#fOperator').val() == null ? "" : $('#fOperator').val();
    //    var fProduct = $('#fProduct').val() == null ? "" : $('#fProduct').val();
    //    var fGroupBy = $('input[name=rbGroupBy]:checked').val();
    //    var fViewBy = $('input[name=rbViewBy]:checked').val();

    //    var groupId = full.GroupID;
    //    var customerId = $("#ddlCustomer").val() == null ? "" : $("#ddlCustomer").val();
    //    var leadPm = $("#ddlLeadPM").val() == null ? "" : $("#ddlLeadPM").val();
    //    var aRo = $("#ddlAro").val() == null ? "" : $("#ddlAro").val();
    //    var pmCme = $("#ddlPMCME").val() == null ? "" : $("#ddlPMCME").val();
    //    var regionId = $("#ddlRegion").val() == null ? "" : $("#ddlRegion").val();
    //    var vendorId = $("#ddlVendor").val() == null ? "" : $("#ddlVendor").val();
    //    var powerTypeId = $("#ddlPowerType").val() == null ? "" : $("#ddlPowerType").val();
    //    var groupBy = $('input[name=rdGroupBy]:checked').val();

    //    return "<a class='btDetail' desc='" + desc + "' fYear='" + fYear + "' fMonth='" + fMonth + "' fCompanyId='" + fCompanyId +
    //        "' fRegionalName='" + fRegionalName + "' fOperatorId='" + fOperatorId + "' fProduct='" + fProduct +
    //        "' fGroupBy='" + fGroupBy + "' fViewBy='" + fViewBy + "</a>";
    //}
}