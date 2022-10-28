Data = {};
Data.RowSelectedData = [];
Data.RowDataSelected = [];

jQuery(document).ready(function () {
    $('.select2').select2({});

    Form.Init();
    $('#slSearchBapsType').val("INF").trigger("change");
    $('#slSearchBapsType').attr("disabled", "disabled");
    $('#slSearchCurrency').attr("disabled", "disabled");

    $("#btSearch").unbind().click(function () {
        if ($("#formSearchs").parsley().validate()) {
            if ($('#Status').val() == "Update")
                TableData.Search();
            else
                TableBaps.Search();
        }

        $('.pnlAdd').hide();
    });

    $("#btUpdateInflation").unbind().click(function () {

        if (Data.RowSelectedData.length > 0) {
            $('#mdlToProcess').modal('show');
        } else {
            Common.Alert.Warning("Please Select One or More Data")
        }
        
    });

    $("#btYesConfirm").unbind().click(function () {
        var l = Ladda.create(document.querySelector("#btYesConfirm"));
        if ($("#UpdateInflationData").parsley().validate()) {
            l.start();
            var params = {
                ListID: Data.RowSelectedData.toString(),
                Percentage: $('#InflationPercentage').val(),
                Year: $("#slSearchYear").val()
            }

            $.ajax({
                url: "/api/UpdateInflation/UpdateData",
                type: "POST",
                dataType: "json",
                async: false,
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                if (Common.CheckError.Object(data)) {
                    l.stop();
                    $('#mdlToProcess').modal('hide');
                    Common.Alert.Success("Success to Update Data");
                    Data.RowSelectedData = [];
                    TableData.Search();
                } else {
                    l.stop();
                    $('#mdlToProcess').modal('hide');
                    Common.Alert.Error((data.errorMessage == null || data.errorMessage == "") ? "Something Wrong, Please Contact Helpdesk !" : data.errorMessage)
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                l.stop();
                $('#mdlToProcess').modal('hide');
                Common.Alert.Error(errorThrown)
            });
        }
    });

    $("#btNoConfirm").unbind().click(function () {
        $('#mdlToProcess').modal('hide');
    });

    $("#btSave").unbind().click(function () {
        var l = Ladda.create(document.querySelector("#btSave"))
        l.start();
        Form.Save();
        l.stop();
    });

    $('#tblData').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');

        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            Data.RowSelectedData.push(parseInt(id));

        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row" + id).removeClass("active");
            Helper.RemoveElementFromArray(Data.RowSelectedData, parseInt(id));
        }
    });

    $('#tblBaps').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');

        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            Data.RowDataSelected.push(parseInt(id));

        } else {
            $(this).parents('tr').removeClass("active");
            $(".Row" + id).removeClass("active");
            Helper.RemoveElementFromArray(Data.RowDataSelected, parseInt(id));
        }
    });
});

var TableData = {
    Init: function () {
        var tblData = $('#tblData').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

    },

    Param: function () {
        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsCustomer = $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val();
        fsStip = $("#slSearchStip").val() == null ? "" : $("#slSearchStip").val();
        fsQuarterly = $("#slSearchQuarterly").val() == null ? "" : $("#slSearchQuarterly").val();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
        fsSiteName = $("#tbSearchSiteName").val();
        fsProductType = $("#slSearchProductType").val() == null ? "" : $("#slSearchProductType").val();
        fsSONumber = $("#tbSearchSONumber").val();
        fsSiteID = $("#tbSearchSiteID").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsPowerType = $("#slSearchPowerType").val() == null ? "" : $("#slSearchPowerType").val();
        fsYear = $("#slSearchYear").val() == null ? "" : $("#slSearchYear").val();

        var params = {
            strCompanyId: fsCompanyId,
            strCustomerId: fsCustomer,
            strStipID: fsStip,
            strCurrency: fsCurrency,
            strSONumber: fsSONumber,
            strQuarterly: fsQuarterly,
            strYear: fsYear,
            strProduct: fsProductType,
            strBapsType: fsBapsType,
            strPowerType: fsPowerType,
            strSiteID: fsSiteID,
            strSiteName: fsSiteName
        };

        return params;
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        params = TableData.Param();

        var tblData = $("#tblData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/UpdateInflation/gridData",
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
                        TableData.Export();
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            //buttons: ['copy','csv','excel','pdf','print'],
            "filter": false,
            "lengthMenu": [[25, 50, 100, 1000], ['25', '50', '100', '1000']],
            "destroy": true,
            "columns": [

                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Helper.IsElementExistsInArray(full.id, Data.RowSelectedData)) {
                                $("#Row" + full.id).addClass("active");
                                strReturn += "<label id='" + full.id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                            }
                            else {
                                strReturn += "<label id='" + full.id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                },

                { data: "SONumber" },
                { data: "CompanyInvoice" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "Term" },
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

                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InflationAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

                { data: "AdditionalAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

                { data: "AmountIDR", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".pnlSearch", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".pnlSearch").fadeIn();
                }
                l.stop(); App.unblockUI('.pnlSearch');

                if (Data.RowSelectedData.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowSelectedData.length; i++) {
                        item = Data.RowSelectedData[i];
                        $("#Row" + item).addClass("active");
                    }
                }
            },
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
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                    '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tblData .checkboxes" />' +
                    '<span></span> ' +
                    '</label>';
                var th = $("th.select-all-data").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-data").unbind().on("change", ".group-checkable", function (e) {
                    Data.RowSelectedData = [];
                    var set = $(".pnlSearch .DTFC_LeftBodyWrapper .DTFC_Cloned .checkboxes");
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
                                //Data.RowSelectedData.push(parseInt(id));
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
                        Data.RowSelectedData = TableData.GetListId();
                    else
                        Data.RowSelectedData = [];
                });
            }
        });

        
    },

    GetListId: function () {

        var params = TableData.Param();

        var AjaxData = [];
        $.ajax({
            url: "/api/UpdateInflation/GetDataListId",
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
    },

    Export: function () {
        if ($("#formSearchs").parsley().validate()) {
            fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
            fsCustomer = $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val();
            fsStip = $("#slSearchStip").val() == null ? "" : $("#slSearchStip").val();
            fsQuarterly = $("#slSearchQuarterly").val() == null ? "" : $("#slSearchQuarterly").val();
            fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
            fsSiteName = $("#tbSearchSiteName").val();
            fsProductType = $("#slSearchProductType").val() == null ? "" : $("#slSearchProductType").val();
            fsSONumber = $("#tbSearchSONumber").val();
            fsSiteID = $("#tbSearchSiteID").val();
            fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
            fsPowerType = $("#slSearchPowerType").val() == null ? "" : $("#slSearchPowerType").val();
            fsYear = $("#slSearchYear").val() == null ? "" : $("#slSearchYear").val();


            window.location.href = "/RevenueAssurance/POExport?strCustomerId=" + fsCustomer + "&strCompanyId=" + fsCompanyId + "&strStipID=" + fsStip + "&strCurrency=" + fsCurrency + "&strQuarterly=" + fsQuarterly
                + "&strSONumber=" + fsSONumber + "&strSiteID=" + fsSiteID + "&strYear=" + fsYear + "&strProduct=" + fsProductType + "&strBapsType=" + fsBapsType + "&strPowerType=" + fsPowerType + "&strSiteName=" + fsSiteName;
        }

    }
}

var TableBaps = {
    Init: function () {
        var tblData = $('#tblBaps').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

    },

    Param: function () {
        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsCustomer = $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val();
        fsStip = $("#slSearchStip").val() == null ? "" : $("#slSearchStip").val();
        fsQuarterly = $("#slSearchQuarterly").val() == null ? "" : $("#slSearchQuarterly").val();
        fsCurrency = $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val();
        fsSiteName = $("#tbSearchSiteName").val();
        fsProductType = $("#slSearchProductType").val() == null ? "" : $("#slSearchProductType").val();
        fsSONumber = $("#tbSearchSONumber").val();
        fsSiteID = $("#tbSearchSiteID").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsPowerType = $("#slSearchPowerType").val() == null ? "" : $("#slSearchPowerType").val();
        fsYear = $("#slSearchYear").val() == null ? "" : $("#slSearchYear").val();

        var params = {
            strCompanyId: fsCompanyId,
            strCustomerId: fsCustomer,
            strStipID: fsStip,
            strCurrency: fsCurrency,
            strSONumber: fsSONumber,
            strQuarterly: fsQuarterly,
            strYear: fsYear,
            strProduct: fsProductType,
            strBapsType: fsBapsType,
            strPowerType: fsPowerType,
            strSiteID: fsSiteID,
            strSiteName: fsSiteName
        };

        return params;
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        params = TableData.Param();

        var tblData = $("#tblBaps").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/UpdateInflation/gridBaps",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    extend: 'excelHtml5',
                    filename: 'Site Detail',
                    text: '<i class="fa fa-file-excel-o"></i>',
                    titleAttr: 'Export to Excel',
                    className: 'btn yellow btn-outline',
                    exportOptions: {
                        columns: 'th:not(:first-child)',
                        format: {
                            body: function (data, row, column, node) {
                                return (column <= 7) ? "\0" + data : data;
                            }
                        }
                    },
                    rows: ':visible'
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            //buttons: ['copy','csv','excel','pdf','print'],
            "filter": false,
            "lengthMenu": [[25, 50, 100, 1000], ['25', '50', '100', '1000']],
            "destroy": true,
            "columns": [

                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (Helper.IsElementExistsInArray(full.ID, Data.RowDataSelected)) {
                                $("#Row" + full.ID).addClass("active");
                                strReturn += "<label id='" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                            }
                            else {
                                strReturn += "<label id='" + full.ID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                },

                { data: "SoNumber" },
                { data: "StipSiro" },
                { data: "CustomerId" },
                { data: "CompanyInvoiceId" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                
                { data: "BapsType" },
                { data: "TERM" },
                {
                    data: "StartBapsDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndBapsDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },

                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Product" },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".pnlAdd", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".pnlAdd").fadeIn();
                }
                l.stop(); App.unblockUI('.pnlAdd');

                if (Data.RowDataSelected.length > 0) {
                    var item;
                    for (var i = 0 ; i < Data.RowDataSelected.length; i++) {
                        item = Data.RowDataSelected[i];
                        $("#Row" + item).addClass("active");
                    }
                }
            },
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
                
            }
        });
    },
}

var Form = {
    Init: function(){
        Control.BindingSelectOperators($('#slSearchCustomer'));
        Control.BindingSelectCompany($('#slSearchCompany'));
        Control.BindingSelectStip($('#slSearchStip'));
        Control.BindingSelectProduct($('#slSearchProductType'));
        Control.BindingSelectBapsType($('#slSearchBapsType'));
        Control.BindingSelectPowerType($('#slSearchPowerType'));
        TableData.Init();
        TableBaps.Init();
        $('.pnlAdd').hide();
    },

    ResetFilter: function () {
        $("#slSearchCompany").val(null).trigger("change");
        $("#slSearchCustomer").val(null).trigger("change");
        $("#slSearchStip").val(null).trigger("change");
        $("#slSearchQuarterly").val(null).trigger("change");
        $("#slSearchCurrency").val("IDR").trigger("change");
        $("#tbSearchSiteName").val("");
        $("#slSearchProductType").val(null).trigger("change");
        $("#tbSearchSONumber").val("");
        $("#tbSearchSiteID").val("");
        $("#slSearchBapsType").val("INF").trigger("change");
        $("#slSearchPowerType").val(null).trigger("change");
        $("#slSearchYear").val(null).trigger("change");
    },

    Add: function () {
        $('#Status').val("Add");
        $('.pnlSearch').fadeOut(500);
        $('.pnlUpdate').fadeOut(500);
        $('.pnlAdd').fadeIn(500);
        $("#slSearchBapsType").removeAttr("disabled");
        $("#slSearchBapsType").val("TOWER").trigger("change");
    },

    Back: function () {
        $('#Status').val("Update");
        $('.pnlAdd').fadeOut(500);
        $('.pnlSearch').fadeIn(500);
        $('.pnlUpdate').fadeIn(500);
        $("#slSearchBapsType").val("INF").trigger("change");
        $("#slSearchBapsType").attr("disabled", "disabled");
    },

    Save: function () {
        if (Data.RowDataSelected.length < 1) {
            Common.Alert.Warning("Please select one or more data")
        }
        else {
            var params = {
                ListID: Data.RowDataSelected.toString()
            }

            $.ajax({
                url: "/api/UpdateInflation/InsertData",
                type: "POST",
                dataType: "json",
                async: false,
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                if (Common.CheckError.Object(data)) {
                    Common.Alert.Success("Success to Insert Data");
                    TableBaps.Search();
                } else {
                    Common.Alert.Error("Something wrong, Please Contact System Administrator !")
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
            });
        }
    }
}

var Control = {
    BindingSelectCompany: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/TBGSys_Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    elements.append("<option value='" + item.CompanyId.trim() + "'>" + item.Company + "</option>");
                })
            }

            elements.select2({ placeholder: "Select Company Name", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectStip: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/TBGSys_Stip",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    elements.append("<option value='" + item.Value.trim() + "'>" + item.Text + "</option>");
                })
            }

            elements.select2({ placeholder: "Select STIP", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectProduct: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/TenantType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    elements.append("<option value='" + item.Value.trim() + "'>" + item.Text + "</option>");
                })
            }

            elements.select2({ placeholder: "Select Product", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectBapsType: function (elements) {
        $.ajax({
            url: "/api/mstDataSource/BapsType",
            type: "GET",
            async: false
        })
       .done(function (data, textStatus, jqXHR) {
           elements.html("<option></option>")
           if (Common.CheckError.List(data)) {
               $.each(data, function (i, item) {
                   elements.append("<option value='" + $.trim(item.BapsType) + "'>" + item.BapsType + "</option>");
               })
               //$(id).val(5).trigger('change');
           }
           elements.select2({ placeholder: "Select Baps Type", width: null });
       })
       .fail(function (jqXHR, textStatus, errorThrown) {
           Common.Alert.Error(errorThrown);
       });
    },
    BindingSelectPowerType: function (elements) {
        $.ajax({
            url: "/api/mstDataSource/PowerType",
            type: "GET",
            async: false
        })
       .done(function (data, textStatus, jqXHR) {
           elements.html("<option></option>")
           if (Common.CheckError.List(data)) {
               $.each(data, function (i, item) {
                   elements.append("<option value='" + parseInt($.trim(item.KodeType)) + "'>" + item.PowerType + "</option>");
               })
           }
           elements.select2({ placeholder: "Select Power Type", width: null });
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
    BindingSelectOperators: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    elements.append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                })
            }

            elements.select2({ placeholder: "Select Customer", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
}

var Helper = {
    Calculate: function () { },
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
    InsertsElementInArray: function (value) {
        var arr = Data.RowSelectedSite;
        var isExist = false;
        for (var i = 0 ; i < arr.length ; i++) {
            if (arr[i] == value) {
                isExist = true;
                break;
            }
        }
        if (!isExist) {
            Data.SiteRow.push(value);
        }
    },
    GetListId: function (isRaw) {
        //for CheckAll Pages
        fsCompanyId = $("#slSearchCompanyName").val() == null ? "" : $("#slSearchCompanyName").val();
        fsOperator = $("#slSearchOperator").val() == null ? "" : $("#slSearchOperator").val();
        var fsGroupBy = $("#slGroupBy").val() == null ? "" : $("#slGroupBy").val();

        var params = {
            strCompanyId: fsCompanyId,
            strOperator: fsOperator,
            strGroupBy: fsGroupBy,
            isRaw: isRaw
        };

        var AjaxData = [];
        $.ajax({
            url: "/api/UpdateInflation/GetListId",
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