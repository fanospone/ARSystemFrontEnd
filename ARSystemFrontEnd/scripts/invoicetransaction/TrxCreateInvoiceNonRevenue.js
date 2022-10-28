params = {};

Data = {};
Data.RowSelected = [];
Data.RowSelectedPeriod = [];
Data.RowSummaryId = [];
Data.RowSiteId = [];
//Data.Mode = "";

DataSiteSelected = [];
DataSiteSummary = [];
SiteTemp = [];

var trxInvoiceNonRevenueID;
var _isPPHFinal;
var _totalPPH = 0;

jQuery(document).ready(function () {
    Control.Init();
    Table.Init();
    Table.Search();
    Table.History();
    // Initialize Datepicker
    $("body").delegate(".datepicker", "focusin", function () {
        $(this).datepicker({
            format: "dd-M-yyyy",
            autoclose: true,
            todayHighlight: true
        });
    });

    $('#chPPN').on("switchChange.bootstrapSwitch", function (event, state) {
        Process.Calculate();
    });

    $('#chPPH').on("switchChange.bootstrapSwitch", function (event, state) {
        Process.Calculate();
    });

    $('#chRounding').on("switchChange.bootstrapSwitch", function (event, state) {
        Process.Calculate();
    });

    $('input[type=radio][name=rdPPHType]').change(function () {
        Process.Calculate();
    });

    $("#slSONumber").select2({
        tags: true,
        multiple: true,
        width: 'auto',
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
                $result.append(" <em>(new)</em>");
            }

            return $result;
        }
    });
});

var Control = {
    Init: function () {
        $("#pnlTransaction").hide();
        $("#formTransaction").parsley();

        Control.BindingCompany('slScrCompany');
        Control.BindingCompany('slCompany');
        Control.BindingCompany('slCompanySite');
        Control.BindingOperator('slScrCustomer', '');
        Control.BindingOperator('slCustomer', 'slCustomerSite');
        Control.GetCurrentUserRole();
        Control.BindingCategoryInvoice(Control.ID.slCategory);

        Control.SetParams();
        Table.SummarySite("#tbSummarySite");
        Table.Site("#tbSite");

        $('#' + Control.ID.tbInvNo)
            .keypress(function (e) {
                var keycode = (e.keyCode ? e.keyCode : e.which);
                if (keycode == '13') {
                    e.preventDefault();
                    $("#btSearch").trigger('click');
                }
            });

        $("#btSearch").unbind().click(function () {
            Table.Search();
            Table.History();
        });

        $("#btReset").unbind().click(function () {
            Table.Reset();
        });

        $("#btCreate").unbind().click(function () {
            Control.Form();
        });

        $("#slCompanySite").on('change', function () {
            Control.SetParams();
            Table.Site("#tbSite");

            var chkAll = $(".group-checkable");
            //By default set to Unchecked.
            chkAll.prop("checked", false);

            var set = $(".checkboxes");
            jQuery(set).each(function () {
                label = $(this).parent();
                var id = label.attr("id");
                if (label.attr("style") != "display:none") {
                    $(this).prop("checked", false);
                    $(this).parents('tr').removeClass("active");
                    $(this).trigger("change");

                    $(".Row" + id).removeClass("active");
                    $("." + id).prop("checked", false);
                    $("." + id).trigger("change");
                }
            });

            DataSiteSelected = [];
            Data.Mode = "slCompany";
            Table.SiteCheckList("#tbSiteCheckList");
        });

        $('#' + Control.ID.slCompH).on('change', function () {
            var slcomp = $('#' + Control.ID.slCompPop).val(this.value).trigger('change').prop('disabled', true);
        });

        $("#slCustomer").on('change', function () {
            //Control.SetParams();
            //Table.Site("#tbSite");
            //Data.RowSelected = [];
            //Table.SiteCheckList("#tbSiteCheckList");
            var slCustomer = $("#slCustomer").val();
            $("#slCustomerSite").val(slCustomer);

            Control.SetParams();
            Table.Site("#tbSite");
        });

        $("#" + Control.ID.slCategory).on('change', function () {
            var slcat = $('#' + Control.ID.slCustomer);
            slcat.prop('disabled', false);
            //updated 25/04/2022 AMA
            //if (this.value != 4) {
            //slcat.val('TBG').trigger('change').prop('disabled', true);
            //}
            Data.RowSummaryId = [];
            Control.ClearRowSelected();
            Process.SaveSite();
        });

        $("#" + Control.ID.slCompH).on('change', function () {
            Data.RowSummaryId = [];
            Control.ClearRowSelected();
            Process.SaveSite();
        });

        $("#" + Control.ID.slCustomer).on('change', function () {
            Data.RowSummaryId = [];
            Control.ClearRowSelected();
            Process.SaveSite();
        });

        $("#btAddSite").unbind().click(function () {
            //Data.RowSelected = [];
            if ($('#' + Control.ID.slCategory).val() == '') {
                Common.Alert.Warning("Choose Category is required");
                return;
            }
            if ($('#' + Control.ID.slCompH).val() == '') {
                Common.Alert.Warning("Choose Company is required");
                return;
            }
            DataSiteSelected = [];
            Table.SiteCheckList("#tbSiteCheckList");
            Table.Site("#tbSite");
            $('#mdlAddSite').modal('show');
        });

        $("#btAddPeriod").unbind().click(function () {
            if (Data.RowSelected.length > 0) {
                $('#tbStartPeriod').val('');
                $('#tbEndPeriod').val('');
                var catInv = $('#' + Control.ID.slCategory).val();
                $('#tbAmountSite').val(catInv != 4 ? $('#' + Control.ID.tbDPP).val() : "0.00");
                $('#mdlAddPeriod').modal('show');
            }
            else {
                Common.Alert.Warning("Please CheckList Data Site!");
            }
        });

        $("#btSavePeriod").unbind().on("click", function (e) {
            Process.SavePeriod();
        });

        $("#btSaveSite").unbind().on("click", function (e) {
            if (DataSiteSelected.length > 0) {
                Process.SaveSite();
            } else {
                Common.Alert.WarningThenRunFunction("Data Site By Period&Amount Must Be CheckList!", function () {
                    $('#mdlAddSite').modal('show');
                });
            }
        });

        $("#btCancel").unbind().on("click", function (e) {
            Control.ClearRowSelected();
            Control.Reset();
            //Table.SummarySite("#tbSummarySite");
        });

        $("#btCreateInvoice").unbind().click(function () {
            var validate = Process.ValidateCreateInvoice();
            if (validate == '')
                Process.CreateInvoice();
            else
                Common.Alert.Warning(validate);
        });

        $("#btEditInvoice").unbind().click(function () {
            var validate = Process.ValidateCreateInvoice();
            if (validate == '')
                Process.EditInvoice();
            else
                Common.Alert.Warning(validate);
        });

        $('#tbSite').on('change', 'tbody tr .checkboxes', function () {
            var id = $(this).parent().attr('id');
            if (this.checked) {
                $(this).parents('tr').addClass("active");
                $(".Row" + id).addClass("active");
                Data.RowSelected.push(parseInt(id));
            } else {
                $(this).parents('tr').removeClass("active");
                $(".Row" + id).removeClass("active");
                Helper.RemoveElementFromArray(Data.RowSelected, parseInt(id));
            }
        });

        $('#tbSiteCheckList').on('change', 'tbody tr .checkboxes1', function () {
            var id = $(this).parent().attr('id');
            var table = $("#tbSiteCheckList").DataTable();

            var DataRows = {};
            var Row = table.row($(this).parents('tr')).data();

            if (this.checked) {
                DataRows.RowNumber = Row.RowNumber;
                DataRows.SoNumber = Row.SoNumber;
                DataRows.SiteID = Row.SiteID;
                DataRows.SiteName = Row.SiteName;
                DataRows.SiteIDCustomer = Row.SiteIDCustomer;
                DataRows.SiteNameCustomer = Row.SiteNameCustomer;
                DataRows.CompanyID = Row.CompanyID;
                DataRows.OperatorID = Row.OperatorID;
                DataRows.StartPeriod = Row.StartPeriod != "" ? Row.StartPeriod : $("#tbStartPeriod").val();
                DataRows.EndPeriod = Row.EndPeriod != "" ? Row.EndPeriod : $("#tbEndPeriod").val();
                DataRows.Amount = Row.Amount != "" ? Row.Amount : $("#tbAmountSite").val();
                Data.RowSelectedPeriod.push(DataRows);
            } else {
                var index = Data.RowSelectedPeriod.findIndex(function (data) {
                    return data.RowNumber == Row.RowNumber;
                });
                Data.RowSelectedPeriod.splice(index, 1);
            }
        });

        $("#tbSummarySite tbody").unbind().on("click", "button.btDeleteEXS", function (e) {
            var table = $("#tbSummarySite").DataTable();
            var buttonId = $(this).attr("id");
            var idComponents = buttonId.split('btDeleteEXS');
            var id = idComponents[1];

            var index = DataSiteSummary.findIndex(function (data) {
                return data.RowNumber == id;
            });
            DataSiteSummary.splice(index, 1);

            for (var i = 0; i < DataSiteSummary.length; i++) {
                Data.RowSummaryId.push(DataSiteSummary[i].RowNumber);
            }

        });

        $("#btSrcSONumber").unbind().click(function () {
            Control.SetParams();
            Table.Site("#tbSite");
        });

        Helper.InitCurrencyInput("#tbAmount");
        Helper.InitCurrencyInput("#tbDiscount");
        Helper.InitCurrencyInput("#tbPenalty");

        Helper.InitCurrencyInput("#tbAmountSite");

        $('#rbPPH').each(function () { this.checked = true; });
        $("#tbPPF").hide();
    },

    BindingCategoryInvoice: function (element) {
        $.ajax({
            url: "/api/CreateInvoiceNonRevenue/CategoryInvoiceList",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $("#" + element).html("<option></option>");
                $.each(data, function (i, item) {
                    $("#" + element).append("<option value='" + item.ID + "'>" + item.CategoryName + "</option>");
                })
                $("#" + element).select2({ placeholder: "Select Category", width: null });
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        })
    },

    BindingCompany: function (element) {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $("#" + element).html("<option></option>");
                $.each(data, function (i, item) {
                    $("#" + element).append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
                $("#" + element).select2({ placeholder: "Select Company", width: null });
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        })
    },

    BindingOperator: function (element, element2) {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                //Updated 25/04/2022 user need to select customer invoice
                //if (element2 == '')
                    $("#" + element).html("<option></option>");

                $.each(data, function (i, item) {
                    $("#" + element).append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                    $("#" + element2).append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                })
                $("#" + element).select2({ placeholder: "Select Customer", width: null });
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        })
    },

    GetCurrentUserRole: function () {
        $.ajax({
            url: "/api/user/GetPosition",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                Data.Role = data.Result;
            }
        });
    },

    SetParams: function () {
        params = {
            vCompany: $('#slCompanySite').val(),
            vOperator: $('#slCustomerSite').val(),
            strSONumber: $("#slSONumber").val() == null ? "" : $("#slSONumber").val(),
            CategoryInvoiceID: $('#' + Control.ID.slCategory).val()
        }
    },

    ClearRowSelected: function () {
        Data.RowSelected = [];
        Data.RowSelectedPeriod = [];
        Data.RowSelectedId = [];
        DataSiteSelected = [];
        DataSiteSummary = [];
        SiteTemp = [];
    },

    Form: function () {
        $("#pnlSummary").hide();
        $("#pnlTransaction").fadeIn();
        $(".panelTransaction").show();

        $("#panelTransactionTitle").text("Create Invoice Non Revenue");

        $("#btCreateInvoice").show();
        $("#btEditInvoice").hide();
        $(".divDesc").hide();

        Table.SummarySite("#tbSummarySite");
        $('#' + Control.ID.slCategory).val(null).trigger('change');
    },

    Reset: function () {
        $("#pnlSummary").show();
        $("#pnlTransaction").fadeOut();
        $(".panelTransaction").hide();

        $("#tbAmount").val('00.0');
        $("#tbDiscount").val('00.0');
        $("#tbDPP").val('00.0');
        $("#tbPPN").val('00.0');
        $("#tbPPH").val('00.0');
        $("#tbPenalty").val('00.0');
        $("#tbTotalInvoice").val('00.0');
        $("#tbTerbilang").val('');
        $("#slCompany").val(null).trigger('change');
        Control.BindingOperator('slCustomer', 'slCustomerSite');
    },

    ID: {
        tbInvNo: "tbInvNo",
        slCategory: "slCategory",
        slCompH: "slCompany",
        slCompPop: "slCompanySite",
        slCustomer: "slCustomer",
        tbDPP: "tbDPP"
    }
}

var Table = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblSummaryData = $('#tblSummaryData').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $("#tblSummaryData tbody").on("click", "button.btEdit", function (e) {
            var table = $("#tblSummaryData").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                $("#tbAmount").val(Common.Format.CommaSeparation(Data.Selected.Amount.toString()));
                $("#tbDiscount").val(Common.Format.CommaSeparation(Data.Selected.Discount.toString()));
                $("#tbDPP").val(Common.Format.CommaSeparation(Data.Selected.DPP.toString()));
                $("#tbPPN").val(Common.Format.CommaSeparation(Data.Selected.TotalPPN.toString()));
                $("#tbPPH").val(Common.Format.CommaSeparation(Data.Selected.TotalPPH.toString()));
                $("#tbPenalty").val(Common.Format.CommaSeparation(Data.Selected.Penalty.toString()));
                $("#tbTotalInvoice").val(Common.Format.CommaSeparation(Data.Selected.InvoiceTotal.toString()));
                $("#tbTerbilang").val(Common.Format.Terbilang(Data.Selected.InvoiceTotal));
                $("#slCompany").val(Data.Selected.CompanyID).trigger("change");
                $("#slCustomer").val(Data.Selected.OperatorID).trigger("change");
                $("#tbDescription").val(Data.Selected.InvSubject);
                $("#" + Control.ID.slCategory).val(Data.Selected.mstCategoryInvoiceID).trigger('change');

                trxInvoiceNonRevenueID = Data.Selected.trxInvoiceNonRevenueID;
                Table.LoadDataSite(trxInvoiceNonRevenueID);

                $("#pnlSummary").hide();
                $("#pnlTransaction").fadeIn();
                $(".panelTransaction").show();
                $("#panelTransactionTitle").text("Edit Invoice Non Revenue");
                $(".divDesc").show();

                $("#btCreateInvoice").hide();
                $("#btEditInvoice").show();
            }
        });

        $("#tbSummarySite").DataTable().columns.adjust().draw();
        $("#tbSiteCheckList").DataTable().columns.adjust().draw();
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var params = {
            vCompany: $('#slScrCompany').val(),
            vOperator: $('#slScrCustomer').val(),
            vInvNo: $("#tbInvNo").val()
        }

        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/CreateInvoiceNonRevenue/grid",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    extend: 'excelHtml5',
                    filename: 'ONGoingInvoiceNonRevenue',
                    text: '<i class="fa fa-file-excel-o"></i>',
                    titleAttr: 'Export to Excel',
                    className: 'btn yellow btn-outline',
                    exportOptions: {
                        //format: {
                        //    body: function (data, row, column, node) {
                        //        return (column <= 4) ? "\0" + data : data;
                        //    }
                        //}
                    },
                    rows: ':visible'
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        if (Data.Role == "DEPT HEAD") {
                            strReturn += "<button type='button' title='Edit' class='btn blue btn-xs btEdit'>Edit</button>";
                        } else {
                            strReturn += "<button type='button' title='Edit' class='btn blue btn-xs btEdit' disabled>Edit</button>";
                        }

                        return strReturn;
                    }
                },
                { data: "InvNo" },
                {
                    data: "InvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "TermInvoice" },
                { data: "Company" },
                { data: "Company" },
                { data: "Customer" },
                { data: "Customer" },
                { data: "Amount", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Discount", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "TotalPPN", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Penalty", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Currency" },
                { data: "Status" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").show();
                }
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            "columnDefs": [
                    { "targets": [0, 2, 3, 9, 10, 11, 12, 13], "className": "dt-center" },
                    { "targets": [1, 4, 5, 6, 7, 8, 14], "className": "dt-left" }
            ],
            "order": []
        });
    },

    LoadDataSite: function (ID) {
        $.ajax({
            url: "/api/CreateInvoiceNonRevenue/gridSummarySite",
            type: "POST",
            data: { trxInvoiceNonRevenueID: ID },
            async: false,
            beforeSend: function (xhr) {
                App.blockUI({ target: "#content", animate: !0 });
            }
        }).done(function (data, textStatus, jqXHR) {
            App.unblockUI("#content");
            if (Common.CheckError.List(data)) {
                SiteTemp = [];
                $.each(data, function (index, item) {
                    SiteTemp.push(item);
                });

                for (var i = 0; i < SiteTemp.length; i++) {
                    var DataRows = {}
                    DataRows.RowNumber = SiteTemp[i].RowNumber;
                    DataRows.SONumber = SiteTemp[i].SONumber;
                    DataRows.SiteID = SiteTemp[i].SiteID;
                    DataRows.SiteName = SiteTemp[i].SiteName;
                    DataRows.SiteIDCustomer = SiteTemp[i].SiteIDCustomer;
                    DataRows.SiteNameCustomer = SiteTemp[i].SiteNameCustomer;
                    DataRows.CompanyID = SiteTemp[i].CompanyID;
                    DataRows.OperatorID = SiteTemp[i].OperatorID;
                    DataRows.StartPeriod = SiteTemp[i].StartPeriod;
                    DataRows.EndPeriod = SiteTemp[i].EndPeriod;
                    DataRows.Amount = SiteTemp[i].Amount
                    DataSiteSummary.push(DataRows);
                    Data.RowSummaryId.push(SiteTemp[i].RowNumber);
                }
                Table.SummarySite("#tbSummarySite");
            } else {
                Common.Alert.Error(data[0].ErrorMessage);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            App.unblockUI("#content");
        });
    },

    History: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var params = {
            vCompany: $('#slScrCompany').val(),
            vOperator: $('#slScrCustomer').val(),
            vInvNo: $("#tbInvNo").val()
        }

        var tblHistory = $("#tblHistory").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/CreateInvoiceNonRevenue/gridHistory",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    extend: 'excelHtml5',
                    filename: 'HistoryInvoiceNonRevenue',
                    text: '<i class="fa fa-file-excel-o"></i>',
                    titleAttr: 'Export to Excel',
                    className: 'btn yellow btn-outline',
                    exportOptions: {
                        //format: {
                        //    body: function (data, row, column, node) {
                        //        return (column <= 4) ? "\0" + data : data;
                        //    }
                        //}
                    },
                    rows: ':visible'
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            "columns": [
                { data: "InvNo" },
                {
                    data: "InvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "TermInvoice" },
                { data: "Company" },
                { data: "Company" },
                { data: "Customer" },
                { data: "Customer" },
                { data: "Amount", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Discount", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "TotalPPN", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Penalty", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Currency" },
                { data: "Status" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblHistory.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").show();
                }
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            "columnDefs": [
                    { "targets": [1, 2, 3, 8, 9, 10, 11, 12], "className": "dt-center" },
                    { "targets": [0, 4, 5, 6, 7, 13], "className": "dt-left" }
            ],
            "order": []
        });
    },

    SummarySite: function (_idTb) {
        var tbSummarySite = $(_idTb).DataTable({
            "deferRender": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table",
            },
            "data": DataSiteSummary,
            "filter": false,
            "lengthMenu": [[10], ['10']],
            "destroy": true,
            "paging": true,
            "columns": [
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<button type='button' title='Delete' class='btn btn-xs red btDelete' id='btDelete" + full.RowNumber + "'><i class='fa fa-trash'></i></button>";
                        return strReturn;
                    }
                },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "SiteIDCustomer" },
                { data: "SiteNameCustomer" },
                { data: "CompanyID" },
                { data: "OperatorID" },
                { data: "StartPeriod" },
                { data: "EndPeriod" },
                { data: "Amount", render: $.fn.dataTable.render.number(',', '.', 2, '') }
            ],
            "order": [],
            "columnDefs": [
                    { "targets": [0], "className": "dt-center" }
            ],
        });

        $("#tbSummarySite tbody").unbind().on("click", "button.btDelete", function (e) {
            var table = $("#tbSummarySite").DataTable();
            var buttonId = $(this).attr("id");
            var idComponents = buttonId.split('btDelete');
            var id = idComponents[1];

            var index = DataSiteSummary.findIndex(function (data) {
                return data.RowNumber == id;
            });
            DataSiteSummary.splice(index, 1);
            Data.RowSummaryId.splice(index, 1);

            //for (var i = 0; i < DataSiteSummary.length; i++) {
            //    Data.RowSummaryId.push(DataSiteSummary[i].RowNumber);
            //}
            Process.SaveSite();
        });
    },

    Site: function (_idTb) {
        App.blockUI({ target: _idTb, animate: !0 });

        let categoryInv = $("#" + Control.ID.slCategory).val();

        //Updated : 25/04/2022
        //no need filter by customer
        params.vOperator = '';
        var tbSite = $(_idTb).DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": categoryInv == 4 ? "/api/CreateInvoiceNonRevenue/gridSite" : Url.APIs.gridSiteINS,
                "type": "POST",
                "datatype": "json",
                "data": params,
                //"async": false
            },
            buttons: [],
            "filter": false,
            "lengthMenu": [[5, 10], ['5', '10']],
            "destroy": true,
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";

                        if (Helper.IsElementExistsInArray(parseInt(full.RowNumber), Data.RowSummaryId)) {
                            strReturn += "<label style='display:none' id='" + full.RowNumber + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.RowNumber + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                        }
                        else {
                            strReturn += "<label id='" + full.RowNumber + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.RowNumber + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                        }
                        return strReturn;
                    }
                },
                { data: "SoNumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "SiteIDCustomer" },
                { data: "SiteNameCustomer" },
                { data: "CompanyID" },
                { data: "OperatorID" }
            ],
            "columnDefs": [
                    { "targets": [4, 5, 6, 7], "className": "dt-center" },
                    { "targets": [1, 2, 3, 5], "className": "dt-left" }
            ],
            "dom": "<'row'<'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "fnDrawCallback": function () {
                App.unblockUI(_idTb);

                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        $("#Row" + item).addClass("active");
                    }
                }
            },
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                                    '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tbSite .checkboxes" />' +
                                    '<span></span> ' +
                                '</label>';
                var th = $("th.select-all-raw").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-raw").unbind().on("change", ".group-checkable", function (e) {
                    //Data.RowSelected = [];
                    var set = $(".checkboxes");
                    var checked = jQuery(this).is(":checked");
                    jQuery(set).each(function () {
                        label = $(this).parent();
                        var id = label.attr("id");
                        if (label.attr("style") != "display:none") {
                            if (checked) {
                                $(this).prop("checked", true);
                                $(this).parents('tr').addClass('active');
                                $(this).trigger("change");

                                $(".Row" + id).addClass("active");
                                $("." + id).prop("checked", true);
                                $("." + id).trigger("change");

                                //$("." + id).hide();
                            } else {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");
                                $(this).trigger("change");

                                $(".Row" + id).removeClass("active");
                                $("." + id).prop("checked", false);
                                $("." + id).trigger("change");

                                $("." + id).show()
                            }
                        }
                    });
                    if (checked) {
                        Data.RowSelected = Data.RowSelected.concat(Helper.GetListId());
                        Table.SiteCheckList("#tbSiteCheckList");
                    }
                    else {
                        $.each(Helper.GetListId(), function (index, item) {
                            Helper.RemoveElementFromArray(Data.RowSelected, parseInt(item));
                        });
                        Table.SiteCheckList("#tbSiteCheckList");
                    }
                });
            }
        });
    },

    SiteCheckList: function (_idTb) {
        var tbSiteCheckList = $(_idTb).DataTable({
            "deferRender": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table",
            },
            "data": DataSiteSelected,
            "filter": false,
            "lengthMenu": [[10], ['10']],
            "destroy": true,
            "columns": [
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<button type='button' title='Delete' class='btn btn-xs red btDeleteSite' id='btDeleteSite" + full.RowNumber + "'><i class='fa fa-trash'></i></button>";
                        return strReturn;
                    }
                },
                { data: "SoNumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "SiteIDCustomer" },
                { data: "SiteNameCustomer" },
                { data: "CompanyID" },
                { data: "OperatorID" },
                { data: "StartPeriod" },
                { data: "EndPeriod" },
                { data: "Amount", render: $.fn.dataTable.render.number(',', '.', 2, '') }
            ],
            "order": [],
            "fnDrawCallback": function () {
                App.unblockUI(_idTb);
            },
            "columnDefs": [
                    { "targets": [0], "className": "dt-center" }
            ],
            //"createdRow": function (row, data, index) {
            //    /* Add Unique CSS Class to row as identifier in the cloned table */
            //    var id = $(row).attr("id");
            //    $(row).addClass(id);
            //},
            //"initComplete": function () {
            //    /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
            //    var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
            //                        '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tbSiteCheckList .checkboxes1" />' +
            //                        '<span></span> ' +
            //                    '</label>';
            //    var th = $("th.select-all-raw").html(checkbox);
            //    /* Bind Event to Select All Checkbox in the Cloned Table */
            //    $("th.select-all-raw").unbind().on("change", ".group-checkable", function (e) {
            //        var set = $(".checkboxes1");
            //        var checked = jQuery(this).is(":checked");
            //        jQuery(set).each(function () {
            //            label = $(this).parent();
            //            var id = label.attr("id");
            //            if (label.attr("style") != "display:none") {
            //                if (checked) {
            //                    $(this).prop("checked", true);
            //                    $(this).parents('tr').addClass('active');
            //                    $(this).trigger("change");
            //                    $(".Row" + id).addClass("active");
            //                    $("." + id).prop("checked", true);
            //                    $("." + id).trigger("change");
            //                } else {
            //                    $(this).prop("checked", false);
            //                    $(this).parents('tr').removeClass("active");
            //                    $(this).trigger("change");
            //                    $(".Row" + id).removeClass("active");
            //                    $("." + id).prop("checked", false);
            //                    $("." + id).trigger("change");
            //                }
            //            }
            //        });
            //    });
            //}
        });

        $("#tbSiteCheckList tbody").unbind().on("click", "button.btDeleteSite", function (e) {
            var table = $("#tbSiteCheckList").DataTable();
            var buttonId = $(this).attr("id");
            var idComponents = buttonId.split('btDeleteSite');
            var id = idComponents[1];

            var index = DataSiteSelected.findIndex(function (data) {
                return data.RowNumber == id;
            });
            DataSiteSelected.splice(index, 1);

            //Row with ID in btDeleteSite back to tblSummary with normal state checkbox
            $("#Row" + id).removeClass('active');
            $("#" + id + " input[type=checkbox]").removeAttr("checked");
            $("#" + id).show();

            /* Uncheck the checkbox in cloned table */
            $('.Row' + id).removeClass('active');
            $('.' + id + ' input[type=checkbox]').removeAttr("checked");
            $('.' + id).show();

            table.row($(this).parents('tr')).remove().draw();

            Helper.RemoveElementFromArray(Data.RowSelected, parseInt(id));
        });
    },

    Reset: function () {
        $("#slScrCompany").val("").trigger('change');
        $("#slScrCustomer").val("").trigger('change');
    },
}

var Process = {
    SavePeriod: function () {
        var validate = Process.Validate();

        if (validate == '') {
            DataSiteSelected = Helper.GetCheckListSite(Data.RowSelected);
            var SiteTemp = [];
            for (var i = 0; i < DataSiteSelected.length; i++) {
                var DataRows = {}
                DataRows.RowNumber = DataSiteSelected[i].RowNumber;
                DataRows.SoNumber = DataSiteSelected[i].SoNumber;
                DataRows.SiteID = DataSiteSelected[i].SiteID;
                DataRows.SiteName = DataSiteSelected[i].SiteName;
                DataRows.SiteIDCustomer = DataSiteSelected[i].SiteIDCustomer;
                DataRows.SiteNameCustomer = DataSiteSelected[i].SiteNameCustomer;
                DataRows.CompanyID = DataSiteSelected[i].CompanyID;
                DataRows.OperatorID = DataSiteSelected[i].OperatorID;
                DataRows.StartPeriod = $("#tbStartPeriod").val();
                DataRows.EndPeriod = $("#tbEndPeriod").val();
                DataRows.Amount = parseFloat($("#tbAmountSite").val().replace(/,/g, ""));
                SiteTemp.push(DataRows);
                Data.RowSiteId.push(DataSiteSelected[i].RowNumber);
            }
            DataSiteSelected = [];
            DataSiteSelected = SiteTemp;
            Table.SiteCheckList("#tbSiteCheckList");

            // Hide the checkboxes in the cloned table
            $.each(Data.RowSelected, function (index, item) {
                $(".Row" + item).removeClass('active');
                $("." + item).hide();
            });

            //Data.RowSelected = [];

        } else {
            Common.Alert.WarningThenRunFunction(validate, function () {
                $('#mdlAddPeriod').modal('show');
            });
            return;
        }

    },

    Validate: function () {
        if ($("#tbStartPeriod").val() == '')
            return "Start Period must be filled.";

        if ($("#tbEndPeriod").val() == '')
            return "End Period must be filled.";

        if ($("#tbAmountSite").val() == '0.00')
            return "Amount must be filled.";

        return '';
    },

    SaveSite: function () {
        //var SiteTemp = [];
        for (var i = 0; i < DataSiteSelected.length; i++) {
            var DataRows = {}
            DataRows.RowNumber = DataSiteSelected[i].RowNumber;
            DataRows.SONumber = DataSiteSelected[i].SoNumber;
            DataRows.SiteID = DataSiteSelected[i].SiteID;
            DataRows.SiteName = DataSiteSelected[i].SiteName;
            DataRows.SiteIDCustomer = DataSiteSelected[i].SiteIDCustomer;
            DataRows.SiteNameCustomer = DataSiteSelected[i].SiteNameCustomer;
            DataRows.CompanyID = DataSiteSelected[i].CompanyID;
            DataRows.OperatorID = DataSiteSelected[i].OperatorID;
            DataRows.StartPeriod = $("#tbStartPeriod").val();
            DataRows.EndPeriod = $("#tbEndPeriod").val();
            DataRows.Amount = parseFloat($("#tbAmountSite").val().replace(/,/g, ""));
            DataSiteSummary.push(DataRows);
            Data.RowSummaryId.push(DataSiteSelected[i].RowNumber);
        }
        Data.RowSelected = [];
        DataSiteSelected = [];
        $('#tbStartPeriod').val('');
        $('#tbEndPeriod').val('');
        $('#tbAmountSite').val('0.00');
        Table.SummarySite("#tbSummarySite");
    },

    Calculate: function () {
        var InvoiceAmount = parseFloat($("#tbAmount").val().replace(/,/g, ""));
        var DPPAmount = parseFloat($("#tbDPP").val().replace(/,/g, ""));
        var DiscountAmount = parseFloat($("#tbDiscount").val().replace(/,/g, ""));
        var PenaltyAmount = parseFloat($("#tbPenalty").val().replace(/,/g, ""));
        var TotalInvoice = 0;
        var PPNAmount = 0;
        var PPHAmount = 0;
        var PPFAmount = 0;
        Data.PPHValue = $("#hdPPHValue").val();
        Data.PPFValue = $("#hdPPFValue").val();
        Data.PPNValue = $("#hdPPNValue").val();

        DPPAmount = InvoiceAmount - DiscountAmount;
        PPNAmount = DPPAmount * PPNValue;

        if ($("#chPPN").bootstrapSwitch("state") == false) {
            PPNAmount = 0;
        }

        if ($("#chPPH").bootstrapSwitch("state") == false) {
            PPHAmount = 0;
            $("input[type=radio][name=rdPPHType]:checked").each(function () { this.checked = false; });
            $("input[type=radio][name=rdPPHType]:checked").prop("disabled", true);
        } else {
            if ($("input[type=radio][name=rdPPHType]:checked").val() == 0)//PPH
            {
                AmountPPH = DPPAmount * Data.PPHValue;
                PPHAmount = AmountPPH;
                $("#tbPPH").show();
                $("#tbPPF").hide();
                _isPPHFinal = 0;
                _totalPPH = Common.Format.CommaSeparation(PPHAmount.toString());
            }
            else //PPF
            {
                AmountPPF = DPPAmount * Data.PPFValue;
                PPFAmount = AmountPPF;
                $("#tbPPH").hide();
                $("#tbPPF").show();
                _isPPHFinal = 1;
                _totalPPH = Common.Format.CommaSeparation(PPFAmount.toString());
            }
        }

        if ($("#chRounding").bootstrapSwitch("state") == true) {
            PPNAmount = Math.floor(PPNAmount);
        }
        else {
            PPNAmount = Math.round(PPNAmount);
        }

        TotalInvoice = DPPAmount + PPNAmount - PPHAmount - PenaltyAmount - PPFAmount;

        $("#tbAmount").val(Common.Format.CommaSeparation(InvoiceAmount.toString()));
        $("#tbDPP").val(Common.Format.CommaSeparation(DPPAmount.toString()));
        $("#tbPPN").val(Common.Format.CommaSeparation(PPNAmount.toString()));
        $("#tbPPH").val(Common.Format.CommaSeparation(PPHAmount.toString()));
        $("#tbPPF").val(Common.Format.CommaSeparation(PPFAmount.toString()));
        $("#tbPenalty").val(Common.Format.CommaSeparation(PenaltyAmount.toString()));
        $("#tbDiscount").val(Common.Format.CommaSeparation(DiscountAmount.toString()));
        $("#tbTotalInvoice").val(Common.Format.CommaSeparation(TotalInvoice.toString()));

        if (TotalInvoice != 0) {
            if (TotalInvoice > 0)
                $("#tbTerbilang").val(Common.Format.Terbilang(TotalInvoice));
            else
                $("#tbTerbilang").val("");
        } else
            $("#tbTerbilang").val("");
    },

    CreateInvoice: function () {
        var l = Ladda.create(document.querySelector("#btCreateInvoice"));
        dataList = [];
        for (var i = 0; i < DataSiteSummary.length; i++) {
            var DataRows = {}
            DataRows.RowNumber = DataSiteSummary[i].RowNumber;
            DataRows.SONumber = DataSiteSummary[i].SONumber;
            DataRows.SiteID = DataSiteSummary[i].SiteID;
            DataRows.SiteName = DataSiteSummary[i].SiteName;
            DataRows.SiteIDCustomer = DataSiteSummary[i].SiteIDCustomer;
            DataRows.SiteNameCustomer = DataSiteSummary[i].SiteNameCustomer;
            DataRows.CompanyID = DataSiteSummary[i].CompanyID;
            DataRows.OperatorID = DataSiteSummary[i].OperatorID;
            DataRows.StartPeriod = DataSiteSummary[i].StartPeriod;
            DataRows.EndPeriod = DataSiteSummary[i].EndPeriod;
            DataRows.Amount = DataSiteSummary[i].Amount;
            dataList.push(DataRows);
        }
        if (DataSiteSummary.length == 0) {
            Common.Alert.Warning("Detail is empty, please Add Site");
            return;
        }
        var params = {
            vAmount: $("#tbAmount").val().replace(/,/g, ''),
            vDiscount: $("#tbDiscount").val().replace(/,/g, ''),
            vDPP: $("#tbDPP").val().replace(/,/g, ''),
            vTotalPPN: $("#tbPPN").val().replace(/,/g, ''),
            vTotalPPH: _totalPPH.replace(/,/g, ''),
            vPenalty: $("#tbPenalty").val().replace(/,/g, ''),
            vInvoiceTotal: $("#tbTotalInvoice").val().replace(/,/g, ''),
            IsPPN: $("#chPPN").bootstrapSwitch("state"),
            IsPPH: $("#chPPH").bootstrapSwitch("state"),
            vCompany: $("#slCompany").val(),
            vOperator: $("#slCustomer").val(),
            IsPPHFinal: _isPPHFinal, // 1 = PPH 10%, 0 = 2%
            mstCategoryInvoiceID: $("#" + Control.ID.slCategory).val(),
            ListInvoiceNonRevenueSite: dataList
        }

        $.ajax({
            url: "/api/CreateInvoiceNonRevenue/createInvoice",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Data Success Created With Invoice No :" + data.InvNo)
                Table.Search();
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Control.ClearRowSelected();
            Control.Reset();
            Table.SummarySite("#tbSummarySite");
        })
    },

    EditInvoice: function () {
        var l = Ladda.create(document.querySelector("#btEditInvoice"));
        dataList = [];
        for (var i = 0; i < DataSiteSummary.length; i++) {
            var DataRows = {}
            DataRows.RowNumber = DataSiteSummary[i].RowNumber;
            DataRows.SONumber = DataSiteSummary[i].SONumber;
            DataRows.SiteID = DataSiteSummary[i].SiteID;
            DataRows.SiteName = DataSiteSummary[i].SiteName;
            DataRows.SiteIDCustomer = DataSiteSummary[i].SiteIDCustomer;
            DataRows.SiteNameCustomer = DataSiteSummary[i].SiteNameCustomer;
            DataRows.CompanyID = DataSiteSummary[i].CompanyID;
            DataRows.OperatorID = DataSiteSummary[i].OperatorID;
            DataRows.StartPeriod = DataSiteSummary[i].StartPeriod;
            DataRows.EndPeriod = DataSiteSummary[i].EndPeriod;
            DataRows.Amount = DataSiteSummary[i].Amount;
            dataList.push(DataRows);
        }

        var params = {
            vtrxInvoiceNonRevenueID: trxInvoiceNonRevenueID,
            vAmount: $("#tbAmount").val().replace(/,/g, ''),
            vDiscount: $("#tbDiscount").val().replace(/,/g, ''),
            vDPP: $("#tbDPP").val().replace(/,/g, ''),
            vTotalPPN: $("#tbPPN").val().replace(/,/g, ''),
            vTotalPPH: $("#tbPPH").val().replace(/,/g, ''),
            vPenalty: $("#tbPenalty").val().replace(/,/g, ''),
            vInvoiceTotal: $("#tbTotalInvoice").val().replace(/,/g, ''),
            IsPPN: $("#chPPN").bootstrapSwitch("state"),
            IsPPH: $("#chPPH").bootstrapSwitch("state"),
            vCompany: $("#slCompany").val(),
            vOperator: $("#slCustomer").val(),
            vDescription: $("#tbDescription").val(),
            mstCategoryInvoiceID: $("#" + Control.ID.slCategory).val(),
            ListInvoiceNonRevenueSite: dataList
        }

        $.ajax({
            url: "/api/CreateInvoiceNonRevenue/updateInvoice",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Data Success Update With Invoice No :" + data.InvNo)
                Table.Search();
            }
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Control.ClearRowSelected();
            Control.Reset();
            //Table.SummarySite("#tbSummarySite");
        })
    },

    ValidateCreateInvoice: function () {
        var amount = $("#tbAmount").val();
        var company = $("#slCompany").val();

        if (amount == '0.00' || amount == '')
            return "Amount must be filled.";

        if (company == '')
            return "Company must be selected.";

        var sumAmt = $('#tbSummarySite').DataTable().rows().data().toArray().sum('Amount'),
            dppAmt = $('#tbDPP').val().replace(/,/g, "");
        if (sumAmt != dppAmt) {
            return "Summary Amount Site not equal with Amount DPP";
        }

        return '';
    }
}

var Helper = {
    IsElementExistsInArray: function (value, arr) {
        var isExist = false;
        for (var i = 0 ; i < arr.length ; i++) {
            if (arr[i] == value) {
                isExist = true;
                break;
            }
        }
        return isExist;
    },
    RemoveElementFromArray: function (arr) {
        var what, a = arguments, L = a.length, ax;
        while (L > 1 && arr.length) {
            what = a[--L];
            while ((ax = arr.indexOf(what)) !== -1) {
                arr.splice(ax, 1);
            }
        }
        return arr;
    },
    GetListId: function () {
        var ListID = [];
        $.ajax({
            url: "/api/CreateInvoiceNonRevenue/getListID",
            type: "POST",
            dataType: "json",
            async: false,
            data: params
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                ListID = data;
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
        return ListID;
    },
    GetCheckListSite: function (ListIDSelected) {
        var AjaxData = [];

        var params = {
            ListID: ListIDSelected
        }
        let categoryInv = $("#" + Control.ID.slCategory).val();
        $.ajax({
            url: categoryInv == 4 ? "/api/CreateInvoiceNonRevenue/gridSiteCheckList" : Url.APIs.gridSiteCheck,
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                AjaxData = data;
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            alert("fail");
            Common.Alert.Error(errorThrown)
        });
        return AjaxData;
    },
    GetExcludedID: function () {
        var oTableSite = $('#tbSummarySite').DataTable();
        var excludedIDs = [];
        oTableSite.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var id = this.data().RowNumber;
            excludedIDs.push(parseInt(id));
        });
        return excludedIDs;
    },
    InitCurrencyInput: function (selector) {
        $(selector).unbind().on("blur", function () {
            var value = $(selector).val();
            if (value != "") {
                $(selector).val(Common.Format.CommaSeparation(value));
            } else {
                $(selector).val("0.00");
            }
            Process.Calculate();
        })
    },
},

Url = {
    APIs: {
        gridSiteINS: "/api/CreateInvoiceNonRevenue/gridSiteInvoiceNonSonumb",
        gridSiteCheck: "/api/CreateInvoiceNonRevenue/gridSiteCheckInvoiceNonSonumbList"
    }
}

