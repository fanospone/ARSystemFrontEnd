Data = {};

//filter search 
var fsYear = "";
var fsResidence = "";

//filter search 
var fsRenewalYear = "";
var fsOperator = "";
var fsReconcileType = "";
var fsRenewalYearSeq = "";
var fsDueDatePerMonth = "";
var fsCurrency = "";
var fsRegional = "";
var fsProvince = "";

jQuery(document).ready(function () {
    Data.RowSelectedRaw = [];
    Data.RowSelectedProcess = [];
    Form.Init();
    TableRaw.Init();
    TableRaw.Reset();
    TableRaw.Search();
    

    $('#btProcess').hide();
    $('#btModify').hide();
    //$('#btBack').hide();
    
    //panel Summary
    $("#formSearch").submit(function (e) {
        TableRaw.Search();
        e.preventDefault();
    });

    $("#btSearch").unbind().click(function () {
        TableRaw.Search();
    });

    $("#btBack").unbind().click(function () {
        if (Data.RowSelectedRaw.length == 0)
            Common.Alert.Warning("Please Select One or More Data")
        else
            $('#mdlToInput').modal('show');
    });

    $("#btYesConfirm2").unbind().click(function () {
        Process.SendToInput();
    });

    $("#btReset").unbind().click(function () {
        TableRaw.Reset();
    });

    $('#tblRaw').find('.group-checkable').change(function () {
        var set = jQuery(this).attr("data-set");
        var checked = jQuery(this).is(":checked");
        jQuery(set).each(function () {
            if (checked) {
                $(this).prop("checked", true);
                $(this).parents('tr').addClass('active');
                $(this).trigger("change");
            } else {
                $(this).prop("checked", false);
                $(this).parents('tr').removeClass("active");
                $(this).trigger("change");
            }
        });
        if (checked)
            Data.RowSelectedRaw = Helper.GetListId(0);
        else
            Data.RowSelectedRaw = [];
    });

    $('#tblRaw').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');

        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            Data.RowSelectedRaw.push(parseInt(id));
            //Common.Alert.Warning(JSON.stringify(Data.RowSelectedRaw));
        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row" + id).removeClass("active");
            Helper.RemoveElementFromArray(Data.RowSelectedRaw, parseInt(id));
        }
    });
});

var Form = {
    Init: function () {

        $("#slRegional").select2({ placeholder: "Select Regionl", width: null });
        $("#slRenewalYear").select2({ placeholder: "Select Renewal Year", width: null });
        $("#slProvince").select2({ placeholder: "Select Province", width: null });
        $("#slResidence").select2({ placeholder: "Select Residence Type", width: null });
        $("#slSearchCurrency").select2({ placeholder: "Select Currency", width: null });

        Control.BindingSelectCompany();
        Control.BindingSelectOperator();
        Control.BindingSelectRegional();
        Control.BindingSelectProvince();
        Control.BindingSelectResidence();

        
        $('#tabReconcile').tabs();
        //var CurrentDate = new Date();
        //var CurrentMonth = CurrentDate.getMonth() + 1;
        //$("#slDueDatePerMonth").val(CurrentMonth);
    }
    
}


var TableRaw = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblRaw = $('#tblRaw').DataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });
        $("#tblRaw tbody").on("click", "a.btDetail", function (e) {
            e.preventDefault();
            var table = $("#tblRaw").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                $('#mdlDetail').modal('toggle');
                TableRaw.ShowDetail();
            }
        });
        $(window).resize(function () {
            $("#tblRaw").DataTable().columns.adjust().draw();
        });
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        fsResidence = $("#slResidence").val() == null ? "" : $("#slResidence").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsRenewalYear = $("#slRenewalYear").val() == null ? "" : $("#slRenewalYear").val();
        fsRegional = $("#slRegional").val() == null ? "" : $("#slRegional").val();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
        fsProvince = $("#slProvince").val() == null ? "" : $("#slProvince").val();
        
        var params = {
            strResidence: fsResidence,
            strOperator: fsOperator,
            strYear: fsRenewalYear,
            strRenewalYear: fsRenewalYear,
            strRegional: fsRegional,
            strCurrency: fsCurrency,
            strProvince: fsProvince,
            isRaw: 0
        };

        var tblRaw = $("#tblRaw").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/ReconcileDone/grid",
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
                        TableRaw.Export();
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Helper.IsElementExistsInArray(full.Id, Data.RowSelectedRaw)) {
                                $("#Row" + full.Id).addClass("active");
                                strReturn += "<label id='" + full.ReconProcessedId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.ReconProcessedId + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                            }
                            else {
                                strReturn += "<label id='" + full.ReconProcessedId + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.ReconProcessedId + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                },
                {
                    data: "ReconProcessedId", mRender: function (data, type, full) {
                        return "<a class='btDetail'>" + data + "</a>";
                    }
                },
                { data: "Operator" },
                { data: "RegionId" },
                { data: "ProvinceName" },
                { data: "ResidenceName" },
                {
                    data: "ReconcileDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "Year" },
                { data: "TotalRenewalTenant" },
                { data: "Currency" },
                { data: "TotalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "BAReconcile" },

                { data: "BAOther" },
                { data: "FinalDocument" },
                //{ data: "ISNULL(fnCalculateTotalHargaReconcile(invoiceStartDate, invoiceEndDate, basePrice, omPrice, 0, SoNumber, SiteID), 0)", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '')},

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblRaw.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

                if (Data.RowSelectedRaw.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelectedRaw.length; i++) {
                        item = Data.RowSelectedRaw[i];
                        $("#Row" + item).addClass("active");
                    }
                }
            },
            /*fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (aData.IsLossPPN == 1) {
                    if (aData.AmountPPN != aData.AmountLossPPN) {
                        $('td', nRow).css('background-color', '#FF9999');
                    }
                }
                l.stop();
            },*/
            "order": [],
            "scrollY": 300, /* Enable vertical scroll to allow fixed columns */
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
            "initComplete": function () {
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                    '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tblRaw .checkboxes" />' +
                    '<span></span> ' +
                    '</label>';
                var th = $("th.select-all-raw").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-raw").unbind().on("change", ".group-checkable", function (e) {
                    var set = $(".panelSearchResult .DTFC_LeftBodyWrapper .DTFC_Cloned .checkboxes");
                    var checked = jQuery(this).is(":checked");
                    jQuery(set).each(function () {
                        label = $(this).parent();
                        var id = label.attr("id");
                        if (label.attr("style") != "display:none") {
                            /* Replace the following code with the code to select all checkboxes in all pages */
                            if (checked) {
                                $(this).prop("checked", true);
                                $(this).parents('tr').addClass('active');
                                $(this).trigger("change");

                                $(".Row" + id).addClass("active");
                                $("." + id).prop("checked", true);
                                $("." + id).trigger("change");
                            } else {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");
                                $(this).trigger("change");

                                $(".Row" + id).removeClass("active");
                                $("." + id).prop("checked", false);
                                $("." + id).trigger("change");
                            }
                        }
                    });
                    if (checked)
                        Data.RowSelectedRaw = Helper.GetListId(0);
                    else
                        Data.RowSelectedRaw = [];
                });
            }
        });

        
    },
    Reset: function () {
        $("#slResidence").val("").trigger('change');
        $("#slSearchOperator").val("").trigger('change');
        $("#slRenewalYear").val("").trigger('change');
        $("#slSearchCurrency").val("").trigger('change');
        $("#slRegional").val("").trigger('change');
        $("#slProvince").val("").trigger('change');

        fsResidence = "";
        fsOperator = "";
        fsRenewalYear = "";
        fsCurrency = "";
        fsRegional = "";
        fsProvince = "";
    },
    ShowDetail: function () {
        $('#tbReconProcessedID').val(Data.Selected.ReconProcessedId);
        $('#tbYear').val(Data.Selected.Year);
        $('#tbOperator').val(Data.Selected.Operator);
        $('#tbRegionName').val(Data.Selected.RegionId);
        $('#tbTotalAmount').val(Common.Format.CommaSeparation(Data.Selected.TotalAmount));
        $('#tbTotalRenewalTenant').val(Data.Selected.TotalRenewalTenant);
        $('#tbBAReconcile').val(Data.Selected.BAReconcile);
        $('#tbBAOther').val(Data.Selected.BAOther);
        $('#tbFinalDocument').val(Data.Selected.FinalDocument);
        $('#tbFinalDocumentCode').val(Data.Selected.FinalDocUploadCode);
        $('#tbBAOtherCode').val(Data.Selected.BAUploadCode);
        $('#tbReconcileDate').val(Common.Format.ConvertJSONDateTime(Data.Selected.ReconcileDate));
        $('#tbBAReconcileCode').val(Data.Selected.BAUploadCode);
        $('#tbStatus').val(Data.Selected.StatusRecon);
    },
    Export: function () {
        fsResidence = $("#slResidence").val() == null ? "" : $("#slResidence").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        fsRenewalYear = $("#slRenewalYear").val() == null ? "" : $("#slRenewalYear").val();
        fsRegional = $("#slRegional").val() == null ? "" : $("#slRegional").val();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
        fsProvince = $("#slProvince").val() == null ? "" : $("#slProvince").val();
        var lengthdata = $('#tblRaw tr').length;


        window.location.href = "/RevenueAssurance/Done/Export?strResidence=" + fsResidence + "&strOperator=" + fsOperator
        + "&strRenewalYear=" + fsRenewalYear + "&strYear=" + fsRenewalYear + "&strCurrency=" + fsCurrency
        + "&strRegional=" + fsRegional + "&strProvince=" + fsProvince + "&length=" + lengthdata + "&isRaw=0";
    }
}

var Process = {

    SendToInput: function () {
        var l = Ladda.create(document.querySelector("#btYesConfirm2"))

        var params = {
            soNumb: Data.RowSelectedRaw
        }

        $.ajax({
            url: "/api/ReconcileDone/BackToProcess",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (data == "1") {
                Common.Alert.Success("Data Success Send Back To Reconcile Input!");
                Data.RowSelectedProcess = [];
                Data.RowSelectedRaw = [];
                $('#mdlToInput').modal('hide');
                TableRaw.Search();
            } else {
                Common.Alert.Warning("Something Went Wrong. Contact IT Help Desk");
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        })
    }
}

var Control = {
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchCompanyName").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchCompanyName").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
            }

            $("#slSearchCompanyName").select2({ placeholder: "Select Company Name", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectOperator: function () {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchOperator").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slSearchOperator").append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                })
            }

            $("#slSearchOperator").select2({ placeholder: "Select Operator", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectRegional: function () {
        $.ajax({
            url: "/api/MstDataSource/Regional",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slRegional").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slRegional").append("<option value='" + item.RegionalId + "'>" + item.RegionalName + "</option>");
                })
            }

            $("#slRegional").select2({ placeholder: "Select Operator", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectProvince: function () {
        $.ajax({
            url: "/api/MstDataSource/Province",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slProvince").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slProvince").append("<option value='" + item.ProvinceID + "'>" + item.ProvinceName + "</option>");
                })
            }

            $("#slProvince").select2({ placeholder: "Select Province", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectResidence: function () {
        $.ajax({
            url: "/api/MstDataSource/Residence",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slResidence").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slResidence").append("<option value='" + item.ResidenceID + "'>" + item.ResidenceName + "</option>");
                })
            }

            $("#slResidence").select2({ placeholder: "Select Residence", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    }
}

var Helper = {
    Calculate: function () {},
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
    GetListId: function (isRaw) {
        //for CheckAll Pages
        var params = {
            strOperator: fsOperator,
            strYear: fsYear,
            strResidence: fsResidence,
            strRegional: fsRegional,
            strProvince: fsProvince,
           isRaw : isRaw,
        };
        var AjaxData = [];
        $.ajax({
            url: "/api/ReconcileDone/GetListId",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                AjaxData = data;
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
        //Common.Alert.Error(AjaxData);
        return AjaxData;
    }
}

