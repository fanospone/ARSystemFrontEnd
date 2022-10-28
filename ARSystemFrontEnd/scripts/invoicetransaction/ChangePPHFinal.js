Data = {};

jQuery(document).ready(function () {
    Data.RowSelected = [];

    Control.Init();

    Form.Init();
    Table.Init();
    Table.Search();

    $("#btSearch").unbind().click(function () {
        Table.Search();
    });

    $("#btSave").unbind().click(function () {
        if (Helper.SaveValidation()) {
            Table.SaveChange();
        } else {
            Common.Alert.Info("Please select at least one row and choose the pph type");
        }
    });

    $("#btReset").unbind().click(function () {
        Form.Reset();
    });

    $('#tblSummaryData').on('change', 'tbody tr .checkboxes', function () {
        var table = $("#tblSummaryData").DataTable();
        var data = table.row($(this).parents("tr")).data();

        var temp = new Object();
        if (this.checked) {
            if (!Helper.IsElementExistsInArray(Data.RowSelected, data.UniqueID)) {
                temp.UniqueID = data.UniqueID;
                temp.SONumber = data.SONumber;
                temp.BAPSNumber = data.BAPSNumber;
                temp.BAPSType = data.BAPSType;
                temp.BAPSPeriod = data.BAPSPeriod;
                temp.PONumber = data.PONumber;
                temp.STIPSiroID = data.STIPSiroID;
                temp.StartDateInvoice = data.StartDateInvoice;
                temp.EndDateInvoice = data.EndDateInvoice;
                Data.RowSelected.push(temp);
            }
        } else {
            Data.RowSelected = Helper.RemoveObjectByIdFromArray(Data.RowSelected, data.UniqueID);
        }
    });
});

var Form = {
    Init: function () {
        $("body").delegate(".date-picker", "focusin", function () {
            $(this).datepicker({
                format: "dd-M-yyyy"
            });
        });
    },
    Reset: function () {
        $("#tbInvoiceNumber").val("");
        $("#tbBAPSNumber").val("");
        $("#tbPONumber").val("");
        $("#tbStartInvoice").val("");
        $("#tbEndInvoice").val("");
        $("#tbSONumber").val("");
        $("#slSearchOperator").val("").trigger('change');
    }
}

var Table = {
    Init: function () {
        $(".panelSearchResult").hide();
        var tblSummaryData = $('#tblSummaryData').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var params = {
            strInvoiceNumber: $("#tbInvoiceNumber").val(),
            strSONumber: $("#tbSONumber").val(),
            strBAPSNumber: $("#tbBAPSNumber").val(),
            strPONumber: $("#tbPONumber").val(),
            strOperatorID: $("#slSearchOperator").val(),
            strStartDateInvoice: $("#tbStartInvoice").val(),
            strEndDateInvoice: $("#tbEndInvoice").val(),
        };

        var tblSummaryData = $("#tblSummaryData").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/BapsData/gridChangePPHFinal",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";

                        if (Helper.IsElementExistsInArray(Data.RowSelected, full.UniqueID)) {
                            strReturn += "<label id='" + full.UniqueID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                        }
                        else {
                            strReturn += "<label id='" + full.UniqueID + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' class='checkboxes' /><span></span></label>";
                        }

                        return strReturn;
                    }
                },
                {
                    data: "IsPPHFinal", render: function (data) {
                        return data == 1 ? "PPH Final" : "PPH 23";
                    }
                },
                { data: "SONumber" },
                { data: "InvoiceNumber" },
                { data: "BAPSNumber" },
                { data: "PONumber" },
                { data: "SiteIDOpr" },
                { data: "SiteName" },
                { data: "Type" },
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
                { data: "Company" }
            ],
            "columnDefs": [{ "targets": [0, 1], "orderable": false, "className": "text-center" }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    if (this.fnSettings().fnRecordsTotal() > 0) {
                        $(".panelSearchResult").fadeIn();
                    }
                }
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            "order": [],
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
                    '<input type="checkbox" name="cbSelectAll" class="group-checkable" data-set="#tblSummaryData .checkboxes" />' +
                    '<span></span> ' +
                    '</label>';
                var th = $("th.select-all").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all").unbind().on("change", ".group-checkable", function (e) {
                    var set = $(".panelSearchResult .DTFC_LeftBodyWrapper .DTFC_Cloned .checkboxes");
                    var checked = jQuery(this).is(":checked");
                    jQuery(set).each(function () {
                        label = $(this).parent();
                        if (label.attr("style") != "display:none") {
                            if (checked) {
                                $(this).prop("checked", true);
                                $(this).parents('tr').addClass('active');
                                $(this).trigger("change");
                            } else {
                                $(this).prop("checked", false);
                                $(this).parents('tr').removeClass("active");
                                $(this).trigger("change");
                            }
                        }
                    });
                });
            }
        });
    },
    SaveChange: function () {
        var pphSelected = $("#slPPHType").val();
        var l = Ladda.create(document.querySelector("#btSearch"))
        $.ajax({
            url: "/api/BapsData/doChangePPHFinal/" + pphSelected,
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(Data.RowSelected),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            }
        }).done(function (data, textStatus, jqXHR) {
            if (data == 1) {
                Common.Alert.Success("PPH Type Successfully Changed!");
            } else {
                Common.Alert.Error("PPH Type Failed to Change!");
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        }).always(function (jqXHR, textStatus) {
            l.stop();
            App.unblockUI('.panelSearchResult');
            Data.RowSelected = [];
            Table.Search();
        });
    }
}

var Control = {
    Init: function () {
        Control.BindingSelectOperator();
        Control.BindingSelectPPHType();
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
                    $("#slSearchOperator").append("<option value='" + $.trim(item.OperatorId) + "'>" + item.OperatorId + "</option>");
                })
            }

            $("#slSearchOperator").select2({ placeholder: "Select Operator", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectPPHType: function () {
        $("#slPPHType").html("<option></option>")
        $("#slPPHType").append("<option value='0'>PPH 23</option>");
        $("#slPPHType").append("<option value='1'>PPH Final</option>");
        $("#slPPHType").select2({ placeholder: "Select PPH Type", width: null });
    }
}

var Helper = {
    IsElementExistsInArray: function (data, id) {
        var isExist = false;
        for (var i = 0 ; i < data.length ; i++) {
            if (data[i].UniqueID == id) {
                isExist = true;
                break;
            }
        }
        return isExist;
    },
    RemoveObjectByIdFromArray: function (data, id) {
        var data = $.grep(data, function (e) {
            if (e.UniqueID == id) {
                return false;
            } else {
                return true;
            }
        });

        return data;
    },
    SaveValidation: function () {
        if (Data.RowSelected.length > 0 && $("#slPPHType").val() != "") {
            return true;
        } else {
            return false;
        }
    }
}