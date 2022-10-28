var fsStatus = "";
var fsListDetailID = [];
var fsCustomerid;
var fsyear;
var fsUserRole = $("#hdUserRole").val();

$(document).ready(function () {
    $('#tabStartBaps').tabs();
    BindData.BindingSelectOperator("#slSearchCustomerID");
    BindData.BindYear("#sslYear");
    BindData.BindingApprovalStatus("#slApprovalStatusID");
    var date = new Date();
    //$("#month12").mask("999,999,999,999,999999");

        
    $("#sslYear").val(date.getFullYear()).trigger('change');
    $("#slSearchCustomerID").val("XL").trigger('change');
    fsCustomerid = $("#slSearchCustomerID").val();
    fsyear = $("#sslYear").val();

    Table.LoadData();
    Table.LoadDataReady();

    $('#tabStartBaps').unbind().on('tabsactivate', function (event, ui) {
        var newIndex = ui.newTab.index();
        if (newIndex == 0) {
            //Table.LoadData();
        } else {
            
            if ($("#tblRequestAmountTarget").DataTable().data().length > 0)
                $("#btnUpdateRequest").hide();

        }
    });
    Control.Init();
    Control.Buttons();

    if (fsUserRole == "DEPT HEAD") {
        $(".btnUpdateRequest").hide();
        $("#btnAddRequest").hide();
    }
    else {
        $(".btnUpdateRequest").show();
        $("#btnAddRequest").show();
    }

    //Table.LoadData();
});

function showInfo() {
    $("#searchRes").hide();
    $("#filter").hide();
    $("#customerInfo").show();
    $("#monthInfo").show();
    $("#approvalInfo").show();
    $("#btnInfo").show();
    $("#submitData").show();
}

function getDataDetail(id) {
    $("#searchRes").hide();
    $("#filter").hide();
    $("#customerInfo").show();
    $("#monthInfo").show();
    $("#approvalInfo").show();
    $("#btnInfo").show();
    $("#hdTrxAmountTargetID").val(id);

    //  $("#slSearchCustomerID2").val($("#slSearchCustomerID").val()).trigger('change');
    $("#slSearchCustomerID2").val($("#slSearchCustomerID").val()).trigger('change');
    $("#sslYear2").val($("#sslYear").val()).trigger("change");

    //$("#slSearchCustomerID2").text($("#slSearchCustomerID").html());
    //$("#sslYear2").html($("#sslYear").text());

    $("#slSearchCustomerID2").attr('disabled', true);
    $("#sslYear2").attr('disabled', true);

    var param = { ID: id };
    
    $.ajax({
        url: "/api/AmountTarget/GetListDetail",
        type: "POST",
        datatype: "json",
        data: param,
        success: function (data) {
            for (i = 0; i < data.data.length; i++) {
                $("#month" + data.data[i].Month).val(data.data[i].AmountTarget);
                $("#month" + data.data[i].Month).attr('disabled', true);
                fsListDetailID.push(data.data[i].ID);
            }
        }
    });
    var abc = $("#hdUserRole").val();
    if (abc == "DEPT HEAD") {
        $("#approvalInfo").show();
        $("#submitData").show();

    } else {
        $("#approvalInfo").hide();
        $("#submitData").hide();
    }

}

function hideInfo() {
    $("#searchRes").show();
    $("#filter").show();
    $("#customerInfo").hide();
    $("#monthInfo").hide();
    $("#approvalInfo").hide();
    $("#btnInfo").hide();
}

function fillTarget() {
    var monthsAmount = [];
    for (i = 1; i <= 12; i++) {
        if ($("#month" + i).val()) {
            monthsAmount.push($("#month" + i).val());
        }
        else {
            monthsAmount.push(0);
        }
    }
    var operator = $('#slSearchCustomerID2').val();
    var yearfill = $('#sslYear2').val();

    //if (fsStatus == "update") {
    //    approvalStatus = $("#slApprovalStatusID").val();
    //}

    if (fsUserRole == "DEPT HEAD")
        var approvalStatus = $("#slApprovalStatusID").val();
    else
        approvalStatus = 0;


    var params = {
        ID: $("#hdTrxAmountTargetID").val(),
        CustomerID: operator,
        Year: yearfill,
        ApprovalStatusID: approvalStatus
    }
    var paramsdetail = [];
    //var approvalStatus = 0;



    for (i = 1; i <= 12; i++) {
        var dataMonth = {
            ID: fsListDetailID[i - 1],
            Month: i,
            AmountTarget: monthsAmount[i - 1],
        }
        paramsdetail.push(dataMonth);
    }


    $.ajax({
        url: "/api/AmountTarget/Submit",
        type: "POST",
        datatype: "json",
        contentType: "application/json",
        data: JSON.stringify({ post: params, postDetail: paramsdetail }),
        cache: false
    }).done(function (data, textStatus, jqXHR) {
        fsListDetailID = [];
        //location.reload();

    }).fail(function (jqXHR, textStatus, errorThrown) {
        Common.Alert.Error(errorThrown)
    });
}


var Table = {
    Init: function (idTable) {
        var tblSummary = $(idTable).dataTable({
            "filter": false,
            "destroy": true,
            "data": [],
            "proccessing": true,
            "language": {
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
        });
        $(window).resize(function () {
            $(idTable).DataTable().columns.adjust().draw();
        });
    },

    LoadData: function () {

        var idTb = "#tblRequestAmountTarget";
        Table.Init(idTb);
        var params = {
            Year: fsyear,
            CustomerID: fsCustomerid
        };
        var tblList = $(idTb).DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/AmountTarget/GetListHeader",
                "type": "POST",
                "datatype": "json",
                "data": params,
                "cache": false
            },
            buttons: [
                 { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.Export(month);
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50, 200, 99999], ['10', '25', '50', '200', 'Visible']],
            "destroy": true,
            "columns": [

                //{ data: "RowIndex" },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";

                        strReturn += "<i class='fa fa-edit btn btn-xs green link-TrxDataValidationBulky' data-toggle='modal'  title='' ></i> &nbsp;&nbsp;";

                        return strReturn;
                    }
                },
                { data: "StatusName" },
                { data: "CustomerID" },
                { data: "Year" },
                { data: "CreatedBy" },
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "ID" },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [1,2,3,4], "className": "text-center" }, { "targets": [6], "visible": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                if (tblList.data().length >= 1) {
                    $("#btnAddRequest").hide();
                    $("#btnUpdateRequest").hide();
                }
            }
        });
        $(idTb + " tbody").unbind();
        $(idTb + " tbody").on("click", ".link-TrxDataValidationBulky", function (e) {
            var table = $("#tblRequestAmountTarget").DataTable();
            var row = table.row($(this).parents('tr')).data();
            fsCustomerid = row.CustomerID;
            fsyear = row.Year;
            Form.GetDetailRequest(row.ID, row.StatusName);

        });
    },

    LoadDataReady: function () {
      
        var idTb = "#tblBapsData";
        Table.Init(idTb);

        var params = {
            Year: fsyear,
            CustomerID: fsCustomerid
        };

        var tblList = $(idTb).DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/AmountTarget/GetAmountDetailReady",
                "type": "POST",
                "datatype": "json",
                "data": params,
                "cache": false
            },
            buttons: [
                 { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.Export(month);
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[12], ['12']],
            "destroy": true,
            "columns": [
                { data: "CustomerID" },
                { data: "Year" },
                { data: "Month" },
                {
                    data: "AmountTarget", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                { data: "CreatedBy" },
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [0, 1, 2, 4, 5], "className": "text-center" }, { "targets": [3], "className": "text-right" }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {

                $("#btnAddRequest").hide();
                if (tblList.data().length > 0 && fsUserRole != "DEPT HEAD") {
                    $("#btnAddRequest").hide();
                    $("#btnUpdateRequest").show();
                    $("#btnUpdateRequest").unbind().click(function () {
                        fsStatus = "update";
                        
                        var thisYear = new Date().getFullYear();
                        var thisMonth = new Date().getMonth();

                        Control.Request();

                        if (fsyear > thisYear){
                            for (i = 0; i < tblList.data().length; i++) {
                                var j = i + 1;
                                $("#month" + j).val(tblList.data()[i].AmountTarget);
                                $("#month" + j).removeAttr('disabled');
                                fsListDetailID.push(tblList.data()[i].ID);
                            }
                        }
                        else if (fsyear == thisYear){
                            for (i = thisMonth+1; i < tblList.data().length; i++) {
                                var j = i + 1;
                                $("#month" + j).val(tblList.data()[i].AmountTarget);
                                $("#month" + j).removeAttr('disabled');
                                fsListDetailID.push(tblList.data()[i].ID);
                                
                            }
                            for (k=0; k<=thisMonth; k++){
                                var j = k + 1;
                                $("#month" + j).val(tblList.data()[k].AmountTarget);
                                $("#month" + j).attr('disabled', true);
                                fsListDetailID.push(tblList.data()[k].ID);
                            }
                        }
                        else{
                            for (i = 0; i < tblList.data().length; i++) {
                                var j = i + 1;
                                $("#month" + j).val(tblList.data()[i].AmountTarget);
                                $("#month" + j).attr('disabled', true);
                                fsListDetailID.push(tblList.data()[i].ID);
                            }
                        }
                        
                        Helper.CalculatePercentage(1);


                        $("#approvalInfo").hide();
                    })

                } else {
                    if (fsUserRole != "DEPT HEAD"){
                        $("#btnAddRequest").show();
                    }
                    $("#btnUpdateRequest").hide();
                    
                }
                

                if ($("#tblRequestAmountTarget").DataTable().data().length > 0)
                    $("#btnUpdateRequest").hide();

            },
        });
       
    },

    Export: function (month) {
        var fsyear = $("#sslYear").val();
        fsCustomerid = $("#slSearchCustomerID").val();

        window.location.href = "/DashboardRA/FulfilmentRTI/Export?CustomerID=" + fsCustomerid + "&Month=" + month + "&Year=" + fsyear + "";
    },

}

var Helper = {
    CalculatePercentage: function (month) {
        var total = 0;

        for (i = 1; i <= 12; i++) {
            if ($("#month" + i).val()) {
                total += parseInt($("#month" + i).val().replace(/[, ]+/g, " ").trim());
            }
        }

        for (j = 1; j <= 12; j++) {
            if ($("#month" + j).val()) {
                var amount = parseInt($("#month" + j).val());
                var percentage = (amount * 100 )/ total;
                $("#permonth" + j).val(parseFloat(percentage).toFixed(2));
            }
            else {
                $("#permonth" + j).val(0);
            }
        }

        //$("#month" + month).val(Common.Format.FormatCurrency($("#month" + month).val()));

        $("#total").val(total);

        if (total == 0) {
            $("#submitData").attr("disabled", true);
        }
        else {
            $("#submitData").removeAttr("disabled");
        }

    }
}

var BindData = {
    BindingSelectOperator: function (id) {

        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option value='XL' selected>EXCELCOMINDO</option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.OperatorId) + "'>" + item.Operator + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Operator", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindYear: function (id) {
        var start_year = new Date().getFullYear();
        var yearNow = new Date();


        for (var i = start_year - 10; i < start_year ; i++) {
            $(id).append('<option value="' + i + '">  ' + i + '  </option>');
        }

        for (var i = start_year ; i < start_year + 20 ; i++) {
            $(id).append('<option value="' + i + '">  ' + i + '  </option>');
        }

        $(id).select2({ placeholder: "Select Year", width: null });
        $(id).val(yearNow.getFullYear()).trigger('change');
    },
    BindingApprovalStatus: function (id) {

        $.ajax({
            url: "/api/AmountTarget/GetListApprovalStatus",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            // $(id).html("<option value='XL' selected>EXCELCOMINDO</option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.ID) + "'>" + item.StatusName + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Approval", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

}

var Control = {

    Init: function () {

        $("#customerInfo").hide();
        $("#monthInfo").hide();
        $("#approvalInfo").hide();
        $("#btnInfo").hide();
    },

    Buttons: function () {
        $("#btnAddRequest").unbind().click(function () {
            fsStatus = "add";
            Control.Request();
            Control.ClearForm();
            $("#approvalInfo").hide();

            $("#slSearchCustomerID2").val($("#slSearchCustomerID").val()).trigger('change');
            $("#sslYear2").val($("#sslYear").val()).trigger("change");
        })




        $("#btCancel").unbind().click(function () {
            fsStatus = "";
            Control.RequestCancel();

        });

        $("#btReset").unbind().click(function () {

        });

        $("#btSearch").unbind().click(function () {
            fsCustomerid = $("#slSearchCustomerID").val();
            fsyear = $("#sslYear").val();

            Table.LoadData();

            Table.LoadDataReady();

         
        });

        $("#submitData").unbind().click(function () {
            Form.SubmitRequest();
        });
    },

    Request: function () {
        $("#searchRes").hide();
        // $("#filter").hide();
        $('#slSearchCustomerID').removeAttr("disabled");
        $('#sslYear').removeAttr("disabled");
        $(".btnFilter").hide();
        $("#customerInfo").show();
        $("#monthInfo").show();
        $("#approvalInfo").show();
        $("#btnInfo").show();
        $("#submitData").show();
    },
    RequestCancel: function () {
        $('#slSearchCustomerID').removeAttr("disabled");
        $('#sslYear').removeAttr("disabled");
        $("#searchRes").show();
        $(".btnFilter").show();
        $("#customerInfo").hide();
        $("#monthInfo").hide();
        $("#approvalInfo").hide();
        $("#btnInfo").hide();
    },

    ClearForm: function () {
        for (i = 1; i <= 12; i++) {
            $("#month" + i).val(0);
            fsListDetailID = [];
            $("#month" + i).removeAttr('disabled');
        }
    }
}

var Form = {
    SubmitRequest: function () {
        var monthsAmount = [];
        for (i = 1; i <= 12; i++) {
            if ($("#month" + i).val()) {
                monthsAmount.push($("#month" + i).val());
            }
            else {
                monthsAmount.push(0);
            }
        }
        fsCustomerid = $('#slSearchCustomerID').val();
        fsyear = $('#sslYear').val();


        if (fsUserRole == "DEPT HEAD")
            var approvalStatus = $("#slApprovalStatusID").val();
        else
            approvalStatus = 0;


        var params = {
            ID: $("#hdTrxAmountTargetID").val(),
            CustomerID: fsCustomerid,
            Year: fsyear,
            ApprovalStatusID: approvalStatus,
        }
        var paramsdetail = [];

        for (i = 1; i <= 12; i++) {
            var dataMonth = {
                ID: fsListDetailID[i - 1],
                Month: i,
                AmountTarget: monthsAmount[i - 1],
            }
            paramsdetail.push(dataMonth);
        }

        $.ajax({
            url: "/api/AmountTarget/Submit",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify({ post: params, postDetail: paramsdetail }),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            fsListDetailID = [];
            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    Common.Alert.Success("Data Success Saved!");
                    Control.RequestCancel();
                    Table.LoadData();
                    Table.LoadDataReady();

                } else {
                    Common.Alert.Warning(data.ErrorMessage);
                }
            }

        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
        });
    },

    GetDetailRequest: function (id, status) {
        Control.Request();
        $("#hdTrxAmountTargetID").val(id);
        var abc = $("#hdUserRole").val();
        $("#slSearchCustomerID").val(fsCustomerid).trigger('change');
        $("#sslYear").val(fsyear).trigger("change");

        $("#slSearchCustomerID").attr('disabled', true);
        $("#sslYear").attr('disabled', true);

        var param = { ID: id };
        $.ajax({
            url: "/api/AmountTarget/GetListDetail",
            type: "POST",
            datatype: "json",
            data: param,
            success: function (data) {
               // var abc = $("#hdUserRole").val();

                for (i = 0; i < data.data.length; i++) {
                    $("#month" + data.data[i].Month).val(data.data[i].AmountTarget);

                    if (abc == "DEPT HEAD" || status.toUpperCase() != "REJECT") {
                        if (abc == "DEPT HEAD") {
                            $("#approvalInfo").show();
                            $("#submitData").show();
                        }
                        else {
                            $("#approvalInfo").hide();
                            $("#submitData").hide();
                        }
                        $("#month" + data.data[i].Month).attr('disabled', true);
                        
                    }
                    else {
                        $("#approvalInfo").hide();
                        $("#submitData").show();
                        $("#month" + data.data[i].Month).removeAttr('disabled');
                    }
                        
                    fsListDetailID.push(data.data[i].ID);
                }
            }
        });
    }
}