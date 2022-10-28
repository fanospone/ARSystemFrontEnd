var requestDetail = [];
var AppHeaderID;
var HeaderID;
var ParamHeader = {};
var ParamDetail = {};
var isReHold;
var AccrueType;
var fsRequestNumber;
var fsUserRole;
var fsUserLogin;

jQuery(document).ready(function () {
    fsUserLogin = $("#userLogin").val();
    fsUserRole = $("#userLoginRoleLabel").val();
    //BindData.BindAccrueActivity();
    BindData.BindCategoryCase();
    BindData.BindDetailCase(null);
    BindData.BindingDatePicker();
    BindData.BindAccrueType();

    $("#pnlHeader").hide();
    $("#pnlDetail").hide();
    $("#pnlHeader").css('visibility', 'visible');
    $("#pnlHeader").fadeIn(2000);
 

    $("#btnDownloadTemplate").unbind().click(function () {
        Table.DownloadTemplate();
    });
    $("#btnSaveUpdate").unbind().click(function () {
        Form.Update();
    });
    $('#iFileUpload').on('change', function () {
        var input = document.getElementById('iFileUpload');
        var infoArea = document.getElementById('file-upload-filename');
        var fileName = input.files[0].name;
        infoArea.textContent = fileName;
        $("#spanFile").text("");
    });
    $("#btnCancelUpdate").click(function () {
        Control.ClearForm();
        $("#pnlHeader").fadeIn(2000);
        $("#pnlDetail").hide();

    });

    $(".btnSearchHeader").unbind().click(function () {
        Control.SetParamHeader();
        Table.LoadHeader();
    });
    Control.SetParamHeader();
    Table.Init("#tblRequestHeader");
    Table.LoadHeader();
});

var Control = {
    ClearForm: function () {
        document.getElementById('iFileUpload').value = "";
        document.getElementById("file-upload-filename").innerText = "";
    },
    SetParamHeader: function () {
        var RequestNumber = $("#sRequestNumber").val();
        var RequestTypeID = $("#slsRequestTypeID").val();
        var InitiatorName = $("#sInitiator").val();
        var ActivityOwnerName = $("#sActivityOwner").val();
        var CreatedDate = $("#sCreatedDate").val() == "" ? null : $("#sCreatedDate").val();
        var StartEffectiveDate = $("#sStartEffectiveDate").val() == "" ? null : $("#sStartEffectiveDate").val();
        var EndEffectiveDate = $("#sEndEffectiveDate").val() == "" ? null : $("#sEndEffectiveDate").val();
        ParamHeader = { RequestNumber: RequestNumber, RequestTypeID: RequestTypeID, CreatedDate: CreatedDate, StartEffectiveDate: StartEffectiveDate, EndEffectiveDate: EndEffectiveDate, ActivityOwnerName: ActivityOwnerName, InitiatorName: InitiatorName, Initiator: fsUserLogin, UserRole: fsUserRole}
    },
}
var Table = {
    Init: function (id) {
        var tbl = $(id).dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $(id).DataTable().columns.adjust().draw();
        });
    },
    LoadHeader: function () {
        var id = "#tblRequestHeader";
        Table.Init(id);
        var l = Ladda.create(document.querySelector("#loadHeader"));

        $(".loadHeader").fadeIn(100);
        $(id + " tbody").hide();


         $(id).DataTable({
            "deferRender": false,
            "proccessing": false,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/StopAccrue/RequestHeaderUpdateAmount",
                "type": "POST",
                "datatype": "json",
                "data": ParamHeader,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.ExportHeader()
                        l.stop();
                    }
                },

            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "filter": false,
            "order": [[0, 'asc']],
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                { data: "RowIndex" },
                {
                    data: "RequestNumber", render: function (a, b, c) {
                        return "<a class='linkDetail' title='Detail Request'>" + c.RequestNumber + "</i>";
                    }
                },
                
                { data: "InitiatorName" },
                { data: "ActivityOwner" },
                {
                    data: "CreatedDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "StartEffectiveDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "EndEffectiveDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "AccrueType" },
                { data: "ActivityLabel" },
                { data: "ActivityID" },
                { data: "PrevActivityID" },
                { data: "PrevActivityLabel" },
                { data: "RequestTypeID" },
                { data: "ID" },
                { data: "Initiator" },
                { data: "AppHeaderID" },
                { data: "IsReHold" },
            ],
            "columnDefs": [{ "targets": [8, 9, 10, 11, 12, 13, 14, 15,16], "visible": false }, { "targets": [1,2,3,4,5,6,7], "class": "text-center" }],
            "fnDrawCallback": function () {
                l.stop();
                $(".loadHeader").fadeOut(100);
                $(id + " tbody").fadeIn(1500);
            },

        });
        $(id + " tbody").unbind().on("click", ".linkDetail", function (e) {
            var table = $(id).DataTable();
            var row = table.row($(this).parents('tr')).data();
            $("#pnlHeader").hide();
            $("#pnlDetail").fadeIn(2000);
            $("#pnlDetail").css('visibility', 'visible');
            $("#spanFormNumber").text("(" + row.RequestNumber + ")");
            fsRequestNumber = row.RequestNumber;
            AppHeaderID = row.AppHeaderID;
            HeaderID = row.ID;
            isReHold = row.IsReHold;
            AccrueType = row.AccrueType;
            Table.LoadDetail();
            Control.ClearForm();
        });
    },
    LoadDetail: function () {
        requestDetail = [];
        $(".timeline").html('');

        var params = { AppHeaderID: AppHeaderID, HeaderID: HeaderID };
        $.ajax({
            url: "/api/StopAccrue/RequestDetail",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
        }).done(function (data, textStatus, jqXHR) {
            requestDetail = data.vwStopAccrueDetail;
            TempRequestDetail = requestDetail;
            //$(".timeline").append(data.HtmlElements);
            Table.DetailTable();
            Table.DetailTable();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    DetailTable: function () {
        var id = "#tblRequestDetail";
        Table.Init(id);
        $("#tblRequestDetail").DataTable({
            "serverSide": false,
            "filter": true,
            "destroy": true,
            "async": false,
            "data": requestDetail,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },

                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline btnExportDetail', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.ExportDetail();
                        l.stop();
                    }
                },

            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "columns": [
                { data: "RowIndex" },
                {
                    data: "SONumber",
                    orderable: false,
                    mRender: function () {
                        return "<i class='fa fa-eye btn btn-xs yellow editRow' ></i>";
                    }
                },
                { data: "SONumber" },
                {
                    mRender: function (a, b, c) {
                        if (c.IsHold)
                            return "<i class='fa fa-remove'></i>";
                        else
                            return "<i class='fa fa-check'></i>";


                    }

                },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "Company" },
                { data: "Customer" },
                { data: "Product" },
                {
                    data: "RFIDone", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "CategoryCase" },
                { data: "DetailCase" },
                {
                    data: "EffectiveDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                {
                    data: "RevenueAmount", render: function (data) {
                        if (data == null)
                            return "";
                        else
                            return Common.Format.CommaSeparation(data);
                    }
                },
                {
                    data: "CapexAmount", render: function (data) {
                        if (data == null)
                            return "";
                        else
                            return Common.Format.CommaSeparation(data);
                    }
                },
                { data: "FileName" },
                { data: "ID" },
                { data: "CaseCategoryID" },
                { data: "CaseDetailID" },
                { data: "Remarks" },

            ],
            "columnDefs": [{ "targets": [16, 17, 18, 19], "visible": false }, { "targets": [1,2,3,4,5,6,7,8,9,10,11,12], "class": "text-center" }],
            "fnDrawCallback": function () {
            },
            //"scrollY": 300,
            //"scrollX": true,
            //"scrollCollapse": true,
            //"fixedColumns": {
            //    leftColumns: 3 /* Set the 2 most left columns as fixed columns */
            //},
        });
        $("#tblRequestDetail tbody").unbind();

        $("#tblRequestDetail tbody").on("click", ".editRow", function (e) {
            var table = $(id).DataTable();
            var row = table.row($(this).parents('tr')).data();
            BindData.BindDetailCase(row.CaseCategoryID);
            $("#iCategoryCaseID").val(row.CaseCategoryID).trigger('change');
            $("#iDetailCaseID").val(row.CaseDetailID).trigger('change');
            $("#iRemarks").val(row.Remarks);
            $("#iTrxDetail").val(row.ID);
            $("#iSONumber").val(row.SONumber);
            $("#iEffectiveDate").val(Common.Format.ConvertJSONDateTime(row.EffectiveDate));
            $(".fileUpload").show();

            var fileName = row.FileName.split(',');
            $("#fileUploaded").html("");
            $("#fileUploaded").append("<ul>");
            var no = 0;
            for (var i = 0; i < fileName.length; i++) {
                no = no + 1;
                $("#fileUploaded").append("<li>" + (no) + " <a href='/StopAccrue/downloadFile?fileName=" + fileName[i] + "'>" + fileName[i] + "</a></li>");
            }
            $("#fileUploaded").append("</ul>");

            $("#mdlRequestDetail").modal('show');
        });

        var Colums = $(id).DataTable();
        if (isReHold == false)
            Colums.column(3).visible(false);
        else
            Colums.column(3).visible(true);
             
    },
    ExportHeader: function () {

      
       // window.location.href = "/StopAccrue/exportRequestHeader?&InitiatorName=" + InitiatorName + "&RequestTypeID=" + RequestTypeID + "&ActivityOwnerName=" + ActivityOwnerName + "&InitiatorName=" + InitiatorName + "&CreatedDate=" + CreatedDate + "&StartEffectiveDate=" + CreatedDate;
        window.location.href = "/StopAccrue/exportRequestHeaderUpdateAmount?" + $.param(ParamHeader);
    },
    ExportDetail: function () {
        if (HeaderID != null && HeaderID != "") {
            window.location.href = "/StopAccrue/exportRequestDetail?&HeaderID=" + HeaderID + "&IsReHold=" + isReHold + "&RequestNumber=" + fsRequestNumber;
        }
    },
    DownloadTemplate: function () {
        window.location.href = "/StopAccrue/downloadTemplateUpdateAmount?&HeaderID=" + HeaderID + "&RequestNumber=" + fsRequestNumber;
    }
}

var BindData = {
    BindAccrueType: function () {
        var selectId = "#iRequestTypeID";
        var selectId2 = "#slsRequestTypeID";
        $.ajax({
            url: "/api/StopAccrue/accrueType",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                $(selectId).html("<option></option>");
                $(selectId2).html("<option></option>");
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(selectId).append("<option value='" + $.trim(item.ID) + "'>" + item.AccrueType + " Accrue</option>");
                        $(selectId2).append("<option value='" + $.trim(item.ID) + "'>" + item.AccrueType + " Accrue</option>");
                    })
                }
                $(selectId).select2({ placeholder: "Select Type Submission", width: null });
                $(selectId2).select2({ placeholder: "Select Type Submission", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindCategoryCase: function () {
        var selectId = "#iCategoryCaseID";
        $.ajax({
            url: "/api/StopAccrue/categoryCase",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                $(selectId).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(selectId).append("<option value='" + $.trim(item.ID) + "'>" + item.CategoryCase + "</option>");
                    })
                }
                $(selectId).select2({ placeholder: "Select Category", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindDetailCase: function (categID) {
        var selectId = "#iDetailCaseID";
        if (categID == null) {
            $(selectId).select2({ placeholder: "Select Detail Case", width: null });
        } else {

            $.ajax({
                url: "/api/StopAccrue/detailCase",
                type: "GET",
                async: false,
                data: { ID: categID }
            })
                .done(function (data, textStatus, jqXHR) {
                    $(selectId).html("<option></option>")
                    if (Common.CheckError.List(data)) {
                        $.each(data, function (i, item) {
                            $(selectId).append("<option value='" + $.trim(item.ID) + "'>" + item.DetailCase + "</option>");
                        })
                    }
                    $(selectId).select2({ placeholder: "Select Detail Case", width: null });
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    Common.Alert.Error(errorThrown);
                });
        }

    },
    BindingDatePicker: function () {
        $(".datepicker").datepicker({
            format: "dd M yyyy"
        });
    },
}

var Form = {
    Update: function () {

        var l = Ladda.create(document.querySelector("#btnSaveUpdate"));
        l.start();
        var formData = new FormData(); //FormData object  
        var fileInput = document.getElementById("iFileUpload");
        if (fileInput.files[0] != undefined && fileInput.files[0] != null) {
            var fileName = fileInput.files[0].name;
            var extension = fileName.split('.').pop().toUpperCase();
            if (extension != "XLS" && extension != "XLSX") {
                Common.Alert.Warning("Please upload an Excel File.");
                l.stop();
            }
            else {
                
                formData.append("File", fileInput.files[0]);
                formData.append('HeaderID', HeaderID);
                
                $.ajax({
                    url: '/api/StopAccrue/updateAmountCapexRevenue',
                    type: 'POST',
                    data: formData,
                    async: false,
                    beforeSend: function (xhr) {
                        l.start();
                    },
                    cache: false,
                    contentType: false,
                    processData: false
                }).done(function (data, textStatus, jqXHR) {
                    if (data != "") {
                        Common.Alert.Warning(data);
                    } else {
                        Common.Alert.Success("Upload data success!");
                        Table.LoadHeader();
                        $("#pnlHeader").hide();
                        $("#pnlDetail").hide();
                        $("#pnlHeader").css('visibility', 'visible');
                        $("#pnlHeader").fadeIn(2000);
                    }
                    l.stop();
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    Common.Alert.Error(errorThrown)
                    l.stop()
                });
            }
        } else {
            Common.Alert.Warning("Please upload an Excel File.");
            l.stop();
        }
        
    },

}