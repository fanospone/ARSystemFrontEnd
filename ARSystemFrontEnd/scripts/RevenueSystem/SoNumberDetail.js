$(function () {
    Control.Init();
});

var Control = {
    Init: function () {
        Control.BindingQueryParameters();

        $(".btBackRevenueDetail").unbind().click(function () {
            window.close();
        });

        $('#btnSaveAdjustment').click(function () {
            Adjustment.Save();
        })
        $('#btnSeeInvoiceInformation').attr('href', '/ISPInvoiceInformation/InvoiceInformationDetail/' + Common.Helper.getUrlParameter("strSoNumber"));
        $('#btnSeeInvoiceInformation').attr('target', '_blank');
        
    },
    BindingQueryParameters: function () {
        $("#tbSonumb").val(Common.Helper.getUrlParameter("strSoNumber"));
        $("#tbSiteId").val(Common.Helper.getUrlParameter("strSiteID"));
        $("#tbSiteName").val(Common.Helper.getUrlParameter("strSiteName"));
        $("#tbCustomerId").val(Common.Helper.getUrlParameter("strCustomerID"));
        $("#tbRegional").val(Common.Helper.getUrlParameter("strRegional"));
        $("#tbTenantType").val(Common.Helper.getUrlParameter("strProductName"));
        $("#tbStatus").val(Common.Helper.getUrlParameter("strStatus"));
        Tables.SoNumberDetail(Common.Helper.getUrlParameter("strSoNumber"), Common.Helper.getUrlParameter("intStipSiro"));
    }
}

var Tables = {
    SoNumberDetail: function (soNumber, stipSiro) {
        var tblRevenueDetail = $("#tblRevenueDetail").DataTable({
            "processing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/RevenueSystem/GetSoNumberDetail",
                "type": "POST",
                "datatype": "json",
                "data": { fSoNumber: soNumber, fStipSiro: stipSiro },
            },
            "filter": false,
            //"lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            buttons: [],
            "columns": [
                { data: 'DataPeriod' },
                { data: 'DataMonth' },
                { data: 'DataYear' },
                {
                    data: 'StartSLDDate', render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: 'EndSLDate', render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: 'StartBapsDate', render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: 'EndBapsDate', render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: 'StatusInvoice' },
                {
                    data: 'StartInvoiceDate', render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: 'EndInvoiceDate', render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: 'InvoiceNumber' },
                {
                    data: 'TotalAmountInvoice', render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: 'AmountPerMonth', render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: 'AmountPerPeriod', render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: 'BalanceAccrue', render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: 'BalanceUnearned', render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: 'TotalAdjustment', render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    mRender: function (nTd, sData, oData, iRow, iCol) {
                        return "<a class='btViewAdjustment' data-sonumber='" + oData.SONumber + "' data-datamonth='" + oData.DataMonth + "' data-datayear='" + oData.DataYear + "'>View Adjustment</a>";
                    }
                }
            ],
            //"dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [[0, "asc"]],
            "columnDefs": [
               {
                   "targets": [11,12, 13, 14, 15, 16, 17], "className": "text-right"
               },
            ],
            "paging": false,
            scrollX: true,
            scrollY: "750px",
            scrollCollapse: true,
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api();
                fsTotalAccrued = 0;
                fsTotalUnearned = 0;
                var oTableDetail = $('#tblRevenueDetail').DataTable();
                oTableDetail.rows('').every(function (rowIdx, tableLoop, rowLoop) {
                    fsTotalAccrued += this.data().TotalAccrued;
                    fsTotalUnearned += this.data().TotalUnearned;
                    $(api.column(14).footer()).html(Common.Format.CommaSeparation(fsTotalAccrued));
                    $(api.column(15).footer()).html(Common.Format.CommaSeparation(fsTotalUnearned));
                });
                $("#tbAccrued").val(Common.Format.CommaSeparation(fsTotalAccrued));
                $("#tbUnearned").val(Common.Format.CommaSeparation(fsTotalUnearned));

            },
            "fnDrawCallback": function (result) {
                $('.btViewAdjustment').click(function (e) {
                    var params = {
                        soNumber: this.getAttribute('data-sonumber'),
                        monthPeriod: this.getAttribute('data-datamonth'),
                        yearPeriod: this.getAttribute('data-datayear')
                    };
                    Adjustment.DataObjectListAdjustment(params);
                    $('#mdlViewAdjustment').modal('show');
                })
            }
        });

    },
}

var Adjustment = {
    DataObjectListAdjustment: function (params) {
        var result = [];
        $.ajax({
            url: "/api/RevenueSystem/Adjustment",
            type: "GET",
            cache: false,
            async: false,
            data: params
        }).done(function (data, textStatus, jqXHR) {
            result = data;
            Adjustment.Search(result);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
        return result;
    },
    Save: function () {
        var l = Ladda.create(document.querySelector("#btnSaveAdjustment"))
        $.ajax({
            url: "/api/RevenueSystem/SaveAdjustment",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify({
                id: $('#hdId').val(), amount: $('#txtAmount').val()
            }),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            //if (Common.CheckError.Object(data)) {
            //    Common.Alert.Info("Data has been saved.")
            //}
            var params = {
                soNumber: data.SoNumber,
                monthPeriod: data.MonthPeriod,
                yearPeriod: data.YearPeriod
            };
            Adjustment.DataObjectListAdjustment(params);
            l.stop();
            $('#mdlEditAdjustment').modal('hide');
            $('#tblRevenueDetail').DataTable().ajax.reload(null, false);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        })
    },

    Search: function (data) {
        $("#tblAdjustment").dataTable({
            "serverSide": false,
            "filter": false,
            "async": false,
            "data": data,
            "language": {
                "emptyTable": "No data available in table",
            },
            buttons: [],
            "lengthMenu": false,
            "Info": false,
            "bLengthChange": false,
            "paging": false,
            "destroy": true,
            "columns": [
                { data: 'RemarksAdjustment' },
                {
                    data: 'Amount', render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    orderable: false,
                    mRender: function (nTd, sData, oData, iRow, iCol) {
                        return "<i class='fa fa-edit btn btn-xs green link-adjustmentEdit' data-remarkadj='" + oData.RemarksAdjustment + "' data-amount='" + oData.Amount + "' data-id='" + oData.Id + "'></i>";
                    }
                }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "columnDefs": [
                { "targets": [0], "orderable": false, "className": "dt-left" },
                { "targets": [1], "orderable": false, "className": "dt-right" },
            ],
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api();
                // converting to interger to find total
                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                            i : 0;
                };
                // computing column Total of the complete result 
                var total = api.column(1).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                // Update footer by showing the total with the reference of the column index 
                $(api.column(0).footer()).html('Total');
                $(api.column(1).footer()).html(Common.Format.CommaSeparation(total));

            },
            "fnDrawCallback": function (result) {
                $('.link-adjustmentEdit').click(function (e) {
                    $('#txtRemarkAdjustment').val(this.getAttribute('data-remarkadj'));
                    $('#txtAmount').val(this.getAttribute('data-amount'));
                    $('#hdId').val(this.getAttribute('data-id'));
                    $('#mdlEditAdjustment').modal('show');
                })
            }
        });

    }
}