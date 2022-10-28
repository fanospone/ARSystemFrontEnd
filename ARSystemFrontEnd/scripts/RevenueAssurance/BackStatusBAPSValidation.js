var Data = {};
Data.RowSelected = [];
Data.TotalSearch = 0;

var IsAllowProccess = false;

jQuery(document).ready(function () {
    IsAllowProccess = $("#hdAllowProcess").val();
    $('.panelSearchResult').hide();

    Form.Init();

    $("#btSearch").unbind().click(function () {
        Table.Search();

        $.ajax({
            "url": "/api/BackStatusBAPSValidation/count",
            "type": "POST",
            "datatype": "json",
            "data": Param.Get(),
            "beforeSend": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "success": function (response) {
                Data.TotalSearch = response.data;
            },
            "error": function (XMLHttpRequest, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
                Data.TotalSearch = 0;
            },
            "complete": function (data) {
                App.unblockUI(".panelSearchResult");
            }
        });
    });

    if (IsAllowProccess) {
        $("#btSubmitYes").unbind().click(function () {
            Table.Submit();
        });

        $("#btSubmit").unbind().click(function () {
            $('#txtCountSONumber').html(Data.RowSelected.length);
            $('#txtRemark').val('');
        });
    } else {
        $("#chkSelectAll").hide();
        $("#btSubmit").hide();
    }
});

var Form = {
    Init: function () {
        Data = {};
        Data.RowSelected = [];
        $('#slSearchStipSiro').select2({ placeholder: "Select Stip Siro", width: null });
        $("#slSearchStipSiro").val(null).trigger("change");
        $('#slSearchBapsType').select2({ placeholder: "Select Baps Type", width: null });
        $("#slSearchBapsType").val(null).trigger("change");
        Control.BindingSelectStipSiro();
        Control.BindingSelectBapsType();
        Table.Init();
    },
    ResetFilter: function () {
        $("#slSearchStipSiro").val(null).trigger("change");
        $("#slSearchBapsType").val(null).trigger("change");
        $("#SearchCustomerSite").val("");
        $("#SearchSONumber").val("");
    }
}

var Control = {
    BindingSelectStipSiro: function () {
        var id = "#slSearchStipSiro"
        $.ajax({
            url: "/api/BackStatusBAPSValidation/GetListStipSiro",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + item + "'>" + item + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Stip Siro", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectBapsType: function () {
        var id = "#slSearchBapsType"
        $.ajax({
            url: "/api/MstDataSource/BapsType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + item.BapsType + "'>" + item.BapsType + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Baps Type", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    }
}

var Param = {
    Get: function () {
        var fsStipSiro          = $("#slSearchStipSiro").val() == null || $("#slSearchStipSiro").val() == ""? -1 : $("#slSearchStipSiro").val();
        var fsBapsType          = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        var fsCustomerSiteName  = $("#SearchCustomerSite").val() == null ? "" : $("#SearchCustomerSite").val();
        var fsSONumber          = $("#SearchSONumber").val() == null ? "" : $("#SearchSONumber").val();

        var params = {
            StipSiro        : fsStipSiro,
            CustomerSiteName: fsCustomerSiteName,
            BapsType        : fsBapsType,
            SoNumber        : fsSONumber
        };

        return params;
    }
}

var Table = {
    Init: function () {
        Data = {};
        Data.RowSelected = [];

        $('#chkSelectAll').prop("checked", false);

        var tblRaw = $('#tblRaw').DataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblRaw").DataTable().columns.adjust().draw();
        });
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        Data = {};
        Data.RowSelected = [];

        $('#chkSelectAll').prop("checked", false);
        $("#btSubmit").prop("disabled", true);

        var tblRaw = $("#tblRaw").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "filter": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/BackStatusBAPSValidation/grid",
                "type": "POST",
                "datatype": "json",
                "data": Param.Get(),
            },
            "buttons": [],
            "columns": [
                { data: 'RowIndex' },
                {
                    data: null, render: function (data) {
                        if (!IsAllowProccess)
                            return "";

                        return "<input type='checkbox'>";
                    }
                },
                { data: 'SoNumber' },
                { data: 'BAPSNumber' },
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
                { data: 'StipSiro' },
                { data: 'BapsType' },
                { data: 'CustomerSiteName' },
                {
                    data: 'CheckListDoc', render: function (data) {
                        return data ? 1 : 0;
                    }
                },
                {
                    data: 'BapsValidation', render: function (data) {
                        return data ? 1 : 0;
                    }
                },
                {
                    data: 'BapsPrint', render: function (data) {
                        return data ? 1 : 0;
                    }
                },
                {
                    data: 'BapsInput', render: function (data) {
                        return data ? 1 : 0;
                    }
                },
                { data: 'ActivityName' },

            ],
            "columnDefs": [
                { "targets": [0], "className": "text-center" },
                { "targets": [1], "className": "text-center", "orderable": false }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "lengthMenu": [[25, 50, 100, 200], ['25', '50', '100', '200']],
            "scrollY": 300,
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "destroy": true,
            //"fixedColumns": {
            //    leftColumns: 3
            //},
            "fnCreatedRow": function (row, data, index) {
                $(row).attr('id', 'Row_' + data.SoNumber + '_' + data.StipSiro + '_' + data.BapsType);
            },
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblRaw.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }

                if (Data.RowSelected.length > 0) {
                    for (var i = 0; i < Data.RowSelected.length; i++) {
                        let item = Data.RowSelected[i];
                        //$("#Row_" + item).addClass("selected");
                        $("#Row_" + item.SoNumber + "_" + item.StipSiro + "_" + item.BapsType).find("input[type=checkbox]").attr("checked", true);
                    }
                }

                l.stop(); App.unblockUI('.panelSearchResult');
            }
        });

        $(window).resize(function () {
            $("#tblRaw").DataTable().columns.adjust().draw();
        });

        if (IsAllowProccess) {
            // Handle click on checkbox
            $('#tblRaw tbody').unbind('click').on('click', 'input[type="checkbox"]', function (e) {
                var row = $(this).parents('tr');

                var data = tblRaw.row(row).data();
                var sonumb = data.SoNumber;
                var stipSiro = data.StipSiro;
                var bapsType = data.BapsType;

                var dataList = {
                    SoNumber    : sonumb,
                    StipSiro    : stipSiro,
                    BapsType    : bapsType
                }

                var index = Data.RowSelected.findIndex(item => item.SoNumber === dataList.SoNumber && item.StipSiro === dataList.StipSiro && item.BapsType === dataList.BapsType);

                if (this.checked && index === -1)
                    Data.RowSelected.push(dataList);
                else if (!this.checked && index !== -1)
                    Data.RowSelected.splice(index, 1);

                if (this.checked) {
                    //$row.addClass('selected');

                    if (Data.RowSelected.length === Data.TotalSearch) 
                        $('#chkSelectAll').prop("checked", true);

                } else {
                    $('#chkSelectAll').prop("checked", false);
                    //$row.removeClass('selected');
                }

                if (Data.RowSelected.length > 0)
                    $("#btSubmit").prop("disabled", false);
                else
                    $("#btSubmit").prop("disabled", true);

                // Prevent click event from propagating to parent
                e.stopPropagation();
            });

            // Handle click on table cells with checkboxes
            //$('#tblRaw').unbind('click').on('click', 'tbody td, thead th:first-child', function (e) {
            //    $(this).parent().find('input[type="checkbox"]').trigger('click');
            //});

            // Handle click on "Select all" control
            $('thead input[name="select_all"]', tblRaw.table().container()).unbind('click').on('click', function (e) {
                if (this.checked) {
                    $.ajax({
                        "url": "/api/BackStatusBAPSValidation/list",
                        "type": "POST",
                        "datatype": "json",
                        "data": Param.Get(),
                        "beforeSend": function () {
                            App.blockUI({ target: ".panelSearchResult", boxed: true });
                        },
                        "success": function (response) {
                            Data.RowSelected = [];

                            if (response.data.length > 0)
                                $('#tblRaw tbody input[type="checkbox"]:not(:checked)').trigger('click');

                            $.each(response.data, function (itemIndex, item) {
                                var dataList = {
                                    SoNumber: item.SoNumber,
                                    StipSiro: item.StipSiro,
                                    BapsType: item.BapsType
                                }
                                var index = Data.RowSelected.findIndex(fItem => fItem.SoNumber === dataList.SoNumber && fItem.StipSiro === dataList.StipSiro && fItem.BapsType === dataList.BapsType);
                                if (index === -1)
                                    Data.RowSelected.push(dataList);
                            });

                        },
                        "error": function (XMLHttpRequest, textStatus, errorThrown) {
                            Common.Alert.Error(errorThrown);
                        },
                        "complete": function (data) {
                            App.unblockUI(".panelSearchResult");
                        }
                    });
                } else {
                    $('#tblRaw tbody input[type="checkbox"]:checked').trigger('click');
                    Data.RowSelected = [];
                    $("#btSubmit").prop("disabled", true);
                }

                // Prevent click event from propagating to parent
                e.stopPropagation();
            });
        }
    },

    Submit: function () {
        $('#submitModal').modal('hide');

        if (!IsAllowProccess) {
            Common.Alert.Error("You are not allowed to process this action");
            return;
        }

        var remark = $.trim($("#txtRemark").val());
        if (remark === "" || remark === null || remark === undefined) {
            Common.Alert.Info("Remark must be filled");
            return;
        }

        var submitData = { dataList: Data.RowSelected, remark: remark };

        $.ajax({
            "url": "/api/BackStatusBAPSValidation/submit",
            "type": "POST",
            "dataType": "json",
            "contentType": "application/json",
            "data": JSON.stringify(submitData),
            "cache": false,
            "beforeSend": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "success": function (response) {
                $('#submitModal').modal('hide');

                if (response.data === false) {
                    Commom.Alert.Error("Failed back to BAPS status to validation");
                    return;
                }

                Common.Alert.Success("Success back to BAPS status to validation");
                Table.Search();
            },
            "error": function (XMLHttpRequest, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            },
            "complete": function (data) {
                App.unblockUI(".panelSearchResult");
            }
        });
    }
}

function SearchArray(srcArr, findArr) {
    var i, j, current;
    for (i = 0; i < srcArr.length; ++i) {
        if (findArr.length === srcArr[i].length) {
            current = srcArr[i];
            for (j = 0; j < findArr.length && findArr[j] === current[j]; ++j);
            if (j === findArr.length)
                return i;
        }
    }
    return -1;
}