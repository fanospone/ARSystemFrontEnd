Data = {};

jQuery(document).ready(function () {
    Control.GetCurrentUserRole();
    FormHdr.Init();

    if (Data.Role == "AR COLLECTION" || Data.Role == "AR MONITORING") {
        TableHdrCollection.Init();
        TableHdrCollection.Search();
        $("#tblSummaryDataCollection").show();
        $("#tblSummaryData").hide();
    }
    else {
        TableHdr.Init();
        TableHdr.Search();
        $("#tblSummaryDataCollection").hide();
        $("#tblSummaryData").show();
    }

    FormDtl.Init();
    TableDtl.Init();

    //panel Summary
    //$("#formSearch").submit(function (e) {
    //    TableHdr.Search();
    //    e.preventDefault();
    //});

    $("#btSearch").unbind().click(function () {
        if (Data.Role == "AR COLLECTION" || Data.Role == "AR MONITORING") {
            TableHdrCollection.Search();
        }
        else {
            TableHdr.Search();
        }
    });

    $("#btReset").unbind().click(function () {
        TableHdr.Reset();
    });

    $("#btCreate").unbind().click(function () {
        FormHdr.Create();
        TableHdr.Reset();
    });

    $("#btCreateDtl").unbind().click(function () {
        FormDtl.Create();
    });

    //panel transaction Header
    $("#formTransactionHdr").submit(function (e) {
        if (Data.Mode == "Create")
            RejectHeader.Post();
        else if (Data.Mode == "Edit")
            RejectHeader.Put();
        e.preventDefault();
    });

    $(".btCancelHdr").unbind().click(function () {
        FormHdr.Cancel();
        TableHdr.Reset();
    });

    $("#btBack").unbind().click(function () {
        $("#pnlSummary").fadeIn();
        $("#pnlDetailResult").hide();
        $("#pnlTransactionDtl").hide();
        $("#formTransactionDtl").parsley();
    });

    //panel transaction Detail
    $("#formTransactionDtl").submit(function (e) {
        if (Data.Mode == "Create")
            RejectDetail.Post();
        else if (Data.Mode == "Edit")
            RejectDetail.Put();
        e.preventDefault();
    });

    $(".btCancelDtl").unbind().click(function () {
        FormDtl.Cancel();
    });
});

var FormHdr = {
    Init: function () {
        $("#pnlTransactionHdr").hide();
        $("#formTransactionHdr").parsley();

        if (Data.Role == "NON AR")
            $("#rbARData").attr('checked', 'checked');

        if (!$("#hdAllowAdd").val()) {
            $("#btCreate").hide();
        }

        TableHdr.Reset();
    },
    Create: function () {
        Data.Mode = "Create";
        $("#pnlSummary").hide();
        $("#pnlTransactionHdr").fadeIn();
        $(".panelTransactionHdr").show();
        $("#panelTransactionTitleHdr").text("Create Header");
        $("#formTransactionHdr").parsley().reset()

        $("#chStatus").bootstrapSwitch("state", true);
        $("#tbDescriptionHdr").val("");
        if (Data.Role == "AR COLLECTION" || Data.Role == "AR MONITORING")
            $(".divARData").hide();
        else {
            $(".divARData").show();
            $("#tbEmailRecipient").val("");
            $("#tbEmailCC").val("");
        }

    },
    Edit: function () {
        Data.Mode = "Edit";
        $("#pnlSummary").hide();
        $("#pnlTransactionHdr").fadeIn();
        $(".panelTransactionHdr").show();
        $("#panelTransactionTitleHdr").text("Edit Header");
        $("#formTransactionHdr").parsley().reset()


        $("#chStatus").bootstrapSwitch("state", Data.Selected.isActive);
        $("#tbDescriptionHdr").val(Data.Selected.Description);
        $("input[name=rdUserGroup][value=" + Data.Selected.mstUserGroupId + "]").attr("checked", "checked");
        if (Data.Role == "AR COLLECTION" || Data.Role == "AR MONITORING")
            $(".divARData").hide();
        else
        {
            $(".divARData").show();
            $("#tbEmailRecipient").val(Data.Selected.Recipient);
            $("#tbEmailCC").val(Data.Selected.CC);
        }
    },
    Cancel: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransactionHdr").hide();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlTransactionHdr").hide();
        $(".panelSearchBegin").fadeIn();
        $(".panelSearchZero").hide();

        //TableHdr.Reset();
        if (Data.Role == "AR COLLECTION" || Data.Role == "AR MONITORING") {
            TableHdrCollection.Search();
        }
        else {
            TableHdr.Search();
        }
        $(".panelSearchResult").fadeIn();
    }
}

var FormDtl = {
    Init: function () {
        $("#pnlDetailResult").hide();
        $("#pnlTransactionDtl").hide();
        $("#formTransactionDtl").parsley();

        if (!$("#hdAllowAdd").val()) {
            $("#btCreateDtl").hide();
        }
    },
    Create: function () {
        Data.Mode = "Create";
        $("#pnlDetailResult").hide();
        $("#pnlTransactionDtl").fadeIn();
        $(".panelTransactionDtl").show();
        $("#panelTransactionTitleDtl").text("Create Detail");
        $("#formTransactionDtl").parsley().reset()

        $("#chStatusDtl").bootstrapSwitch("state", true);
        $("#tbDescriptionDtl").val("");

    },
    Edit: function () {
        Data.Mode = "Edit";
        $("#pnlDetailResult").hide();
        $("#pnlTransactionDtl").fadeIn();
        $(".panelTransactionDtl").show();
        $("#panelTransactionTitleDtl").text("Edit Detail");
        $("#formTransactionDtl").parsley().reset()


        $("#chStatusDtl").bootstrapSwitch("state", Data.Selected.IsActive);
        $("#tbDescriptionDtl").val(Data.Selected.Description);
    },
    Cancel: function () {
        $("#pnlDetailResult").fadeIn();
        $("#pnlTransactionDtl").hide();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $(".panelSearchResult").hide();

        $("#pnlTransactionDtl").hide();
        $("#pnlDetailResult").hide();

        TableHdr.Reset();
        TableHdr.Search();
    }
}

var TableHdr = {
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
                FormHdr.Edit();
                TableHdr.Reset();
            }
        });

        $("#tblSummaryData tbody").on("click", "a.btDetail", function (e) {
            e.preventDefault();
            var table = $("#tblSummaryData").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                $("#pnlDetailResult").show();

                TableDtl.Generate();
            }
        });

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var params = {
            strHdr: $("#tbSearchHdrDescription").val(),
            intIsActive: $("input[type=radio][name=rdStatus]:checked").val(),
            mstUserGroupId: $("input[type=radio][name=rdSearchUserGroup]:checked").val()
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/RejectHeaderDetail/grid",
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
                        TableHdr.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': [0]
            }],
            "columns": [
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        if ($("#hdAllowEdit").val())
                            strReturn += "<button type='button' title='Edit' class='btn btn-xs green btEdit'><i class='fa fa-edit'></i></button>";
                        return strReturn;
                    },
                    width: "10%"
                },
                {
                    data: "Description",
                    render: function (val, type, full) {
                        return "<a href='' class='btDetail'>" + val + "</a>";
                    }
                },
                {
                    data:"Recipient"
                },
                {
                    data: "CC"
                },
                {
                    data: "IsActive", mRender: function (data, type, full) {
                        return full.isActive ? "Yes" : "No";
                    }
                },
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            "order": []
        });
    },
    Reset: function () {
        $("#tbSearchHdrDescription").val("");
        $("#rbSearchStatusAll").prop("checked", true);
        $("#rbSearchAll").prop("checked", true);
    },
    Export: function () {

        var strHdr = $("#tbSearchHdrDescription").val();
        var intIsActive = $("input[type=radio][name=rdStatus]:checked").val();
        var mstUserGroupId = $("input[type=radio][name=rdSearchUserGroup]:checked").val() == null ? 0 : $("input[type=radio][name=rdSearchUserGroup]:checked").val()

        window.location.href = "/Admin/MasterRejectHdr/Export?strHdr=" + strHdr + "&intIsActive=" + intIsActive + "&mstUserGroupId=" + mstUserGroupId;
    }
}

var TableHdrCollection = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();
        var tblSummaryDataCollection = $('#tblSummaryDataCollection').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $("#tblSummaryDataCollection tbody").on("click", "button.btEditCollection", function (e) {
            var table = $("#tblSummaryDataCollection").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                FormHdr.Edit();
                TableHdr.Reset();
            }
        });

        $("#tblSummaryDataCollection tbody").on("click", "a.btDetailCollection", function (e) {
            e.preventDefault();
            var table = $("#tblSummaryDataCollection").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                $("#pnlDetailResult").show();

                TableDtl.Generate();
            }
        });

        $(window).resize(function () {
            $("#tblSummaryData").DataTable().columns.adjust().draw();
        });
    },
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var params = {
            strHdr: $("#tbSearchHdrDescription").val(),
            intIsActive: $("input[type=radio][name=rdStatus]:checked").val(),
            mstUserGroupId: $("input[type=radio][name=rdSearchUserGroup]:checked").val()
        };
        var tblSummaryDataCollection = $("#tblSummaryDataCollection").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/RejectHeaderDetail/grid",
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
                        TableHdrCollection.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': [0]
            }],
            "columns": [
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        if ($("#hdAllowEdit").val())
                            strReturn += "<button type='button' title='Edit' class='btn btn-xs green btEditCollection'><i class='fa fa-edit'></i></button>";
                        return strReturn;
                    },
                    width: "10%"
                },
                {
                    data: "Description",
                    render: function (val, type, full) {
                        return "<a href='' class='btDetailCollection'>" + val + "</a>";
                    }
                },
                {
                    data: "IsActive", mRender: function (data, type, full) {
                        return full.isActive ? "Yes" : "No";
                    }
                },
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryDataCollection.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            "order": []
        });
    },
    Reset: function () {
        $("#tbSearchHdrDescription").val("");
        $("#rbSearchStatusAll").prop("checked", true);
        $("#rbSearchAll").prop("checked", true);
    },
    Export: function () {

        var strHdr = $("#tbSearchHdrDescription").val();
        var intIsActive = $("input[type=radio][name=rdStatus]:checked").val();
        var mstUserGroupId = $("input[type=radio][name=rdSearchUserGroup]:checked").val() == null ? 0 : $("input[type=radio][name=rdSearchUserGroup]:checked").val()

        window.location.href = "/Admin/MasterRejectHdr/Export?strHdr=" + strHdr + "&intIsActive=" + intIsActive + "&mstUserGroupId=" + mstUserGroupId;
    }
}

var TableDtl = {
    Init: function () {
        var tblDetailData = $("#tblDetailData").DataTable({
            "language": {
                "emptyTable": "No data available in table",
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        TableDtl.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50, -1], ['10', '25', '50', 'All']],
            "destroy": true,
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': [0]
            }],
            "columns": [
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        if ($("#hdAllowEdit").val())
                            strReturn += "<button type='button' title='Edit' class='btn btn-xs green btEditDtl'><i class='fa fa-edit'></i></button>";
                        return strReturn;
                    }
                },
                { data: "Description" },
                {
                    data: "IsActive", mRender: function (data, type, full) {
                        return full.IsActive ? "Yes" : "No";
                    }
                },
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": []
        });

        $("#tblDetailData tbody").on("click", "button.btEditDtl", function (e) {
            var table = $("#tblDetailData").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                FormDtl.Edit();
            }
        });

        $(window).resize(function () {
            $("#tblDetailData").DataTable().columns.adjust().draw();
        });
    },
    Generate: function () {
        $("#pnlSummary").hide();
        $("#lblHeaderDescription").html(Data.Selected.Description);

        var tblDetailData = $("#tblDetailData").DataTable();
        tblDetailData.clear().rows.add(Data.Selected.DetailList).draw();
    },
    Export: function () {

        var strHdrId = Data.Selected.mstPICATypeID;
        window.location.href = "/Admin/MasterRejectDtl/Export?strHdrId=" + strHdrId;
    }
}

var RejectHeader = {
    Post: function () {
        var params = {

            Description: $("#tbDescriptionHdr").val(),
            IsActive: $("#chStatus").bootstrapSwitch("state"),
            mstUserGroupId: $("input[type=radio][name=rdUserGroup]:checked").val() == null ? 0 : $("input[type=radio][name=rdUserGroup]:checked").val(),
            Recipient: $("#tbEmailRecipient").val(),
            CC: $("#tbEmailCC").val()
        }

        var l = Ladda.create(document.querySelector("#btSubmitHdr"))
        $.ajax({
            url: "/api/RejectHeaderDetail/Hdr",
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
                Common.Alert.Success("Master Reject Header has been created!")
                FormHdr.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    },

    Put: function () {
        var params = {
            Description: $("#tbDescriptionHdr").val(),
            IsActive: $("#chStatus").bootstrapSwitch("state"),
            mstUserGroupId: $("input[type=radio][name=rdUserGroup]:checked").val() == null ? 0 : $("input[type=radio][name=rdUserGroup]:checked").val(),
            Recipient: $("#tbEmailRecipient").val(),
            CC: $("#tbEmailCC").val()
        }

        var l = Ladda.create(document.querySelector("#btSubmitHdr"))
        $.ajax({
            url: "/api/RejectHeaderDetail/Hdr/" + Data.Selected.mstPICATypeID,
            type: "PUT",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Master Reject Header has been edited!")
                FormHdr.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    }
}

var RejectDetail = {
    Post: function () {
        var params = {
            mstPICATypeID: Data.Selected.mstPICATypeID,
            Description: $("#tbDescriptionDtl").val(),
            IsActive: $("#chStatusDtl").bootstrapSwitch("state")
        }

        var l = Ladda.create(document.querySelector("#btSubmitDtl"))
        $.ajax({
            url: "/api/RejectHeaderDetail/Dtl",
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
                Common.Alert.Success("Master Reject Detail has been created!")
                FormDtl.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    },

    Put: function () {
        var params = {
            Description: $("#tbDescriptionDtl").val(),
            IsActive: $("#chStatusDtl").bootstrapSwitch("state"),
            mstPICATypeID: Data.Selected.mstPICATypeID
        }

        var l = Ladda.create(document.querySelector("#btSubmitHdr"))
        $.ajax({
            url: "/api/RejectHeaderDetail/Dtl/" + Data.Selected.mstPICADetailID,
            type: "PUT",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Master Reject Detail has been edited!")
                FormDtl.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    }
}


var Control = {
    GetCurrentUserRole: function () {
        Role = "";
        $.ajax({
            url: "/api/user/GetPosition",
            type: "GET",
            async: false

        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                Data.Role = data.Result;
                if (data.Result == "NON AR") {
                    $('.divUserGroup').show();
                }
                else {
                    $('.divUserGroup').hide();
                }
            }
        });
    }
}