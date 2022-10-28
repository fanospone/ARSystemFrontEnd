Data = {};

var fsUserID = "";
var fsCompanyId = "";
var fsOperator = "";
var fsTransactionID = "";

jQuery(document).ready(function () {
    $('.datepicker').datepicker({
        format: 'd-M-yyyy',
        closeAfterSelect: true
    });

    Form.Init();
    Table.Init();
    Table.Reset();

    $("#slSearchSONumber").select2({
        tags: true,
        multiple: true,
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
    $("#slSearchActivity").val("").trigger("change");
    //$('.select2').select2({});

    $("#formSearch").submit(function (e) {
        Table.Search();
        e.preventDefault();
    });

    $("#btSearch").unbind().click(function () {
        Table.Search();
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
    });

});

var Form = {
    Init: function () {
        Control.BindingSelectOperators($('#slSearchCustomer'));
        Control.BindingSelectCompany($('#slSearchCompany'));
        Control.BindingSelectActivity($('#slSearchActivity'));
        $("#slSearchCompany").val("").trigger("change");
        $("#slSearchCustomer").val("").trigger("change");
        $("#slSearchActivity").val("").trigger("change");
        $("#slSearchSONumber").val("").trigger("change");
        $(".panelSearchZero").hide();
    }
}

var Table = {
    Init: function () {
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

        var params = {
            strCompanyID: $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val(),
            strCustomerID: $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val(),
            strActivity: $("#slSearchActivity").val() == null ? "" : $("#slSearchActivity").val(),
            strSONumber: $("#slSearchSONumber").val() == null ? "" : $("#slSearchSONumber").val().toString()
        };

        var tblSiteInfo = $("#tblRaw").DataTable({
            "scrollY": 500,
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 2 /* Set the 2 most left columns as fixed columns */
            },
            "scroller": true,
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/TimeLineActivity/GetList",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            "lengthMenu": [[-1], ['All']],
            "columns": [
                {
                    orderable: false,
                    data: "TransactionID", mRender: function (data, type, full) {
                        return "<a class='btDetail'><i class='fa fa-edit'></i></a>";
                    }
                },
                { data: "SoNumber" },
                { data: "Company" },
                { data: "CustomerId" },

                { data: "ActivityName" },
                {
                    data: "LogDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "UserID" },
            ],
            "buttons": [],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "destroy": true,
            "fnDrawCallback": function (oSettings, json) {
                
            },
            "footerCallback": function (row, data, start, end, display) {
                
            },
        });

        $(window).resize(function () {
            $("#tblSiteInfo").DataTable().columns.adjust().draw();
        });
    },

    Reset: function () {
        $("#slSearchCompany").val("").trigger("change");
        $("#slSearchCustomer").val("").trigger("change");
        $("#slSearchActivity").val("").trigger("change");
        $("#slSearchSONumber").val("").trigger("change");
    }
}

var Control = {
    BindingSelectCompany: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/Company",
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

    BindingSelectOperators: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item.OperatorId.trim() == 'TSEL' || item.OperatorId.trim() == 'ISAT' || item.OperatorId.trim() == 'XL' || item.OperatorId.trim() == 'HCPT')
                        elements.append("<option value='" + item.OperatorId.trim() + "'>" + item.OperatorId + "</option>");
                    //elements.append("<option value='" + item.OperatorId + "'>" + item.OperatorId + "</option>");
                })
            }

            elements.select2({ placeholder: "Select Operator", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectActivity: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/ListActivity",
            type: "GET",
            data: {}
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("");
            //$("#slNextActivity").append("<option value='0'>Reject PO</option>");

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    elements.append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                })
            }

            $("#slSearchActivity").val("").trigger("change");
            elements.select2({ placeholder: "Select Activity", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
}