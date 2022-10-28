var Data = {};
Data.RowSelectedRaw = [];

jQuery(document).ready(function () {
    Form.Init();

    $('.datepicker').datepicker({
        format: 'd-M-yyyy'
    });
    $('.pnlDetail').hide();

    $("#btSearch").unbind().click(function () {
        Table.Search();
    });

    $("#btnSave").unbind().click(function () {
        if ($("#formDetailData").parsley().validate()) {
            Form.SubmitData();
        }
    });
});

var Form = {
    Init: function () {
        $('#slSearchCustomer').select2({ placeholder: "Select Customer", width: null });
        $("#slSearchCustomer").val(null).trigger("change");
        Control.BindingSelectCompany();
        Control.BindingSelectStip();
        Control.BindingSelectTenant();
        Table.Init();
    },
    ResetFilter: function () {
        $("#slSearchCompany").val(null).trigger("change");
        $("#slSearchCustomer").val(null).trigger("change");
        $("#slSearchStip").val(null).trigger("change");
        $("#slSearchTenant").val(null).trigger("change");
        $("#SearchSONumber").val("");
    },
    DetailShow: function () {
        $('.pnlDetail').fadeIn(1000);
        $('.filter').fadeOut(1000);
        $('.panelSearchResult').fadeOut(1000);
        $('#StartDateNew').val('');
        $('#EndDateNew').val('');
        $('#Remarks').val('');
    },
    HideDetail: function () {
        $('.pnlDetail').fadeOut(1000);
        $('.filter').fadeIn(1000);
        $('.panelSearchResult').fadeIn(1000);
    },
    SubmitData: function () {
        var l = Ladda.create(document.querySelector("#btnSave"))
        l.start();

        var params = {
            SONumberLive: $('#SONumberNew').val(),
            SONumber: $('#SONumberOld').val(),
            CustomerSiteName: $('#CustomerSiteNameNew').val(),
            Customer: $('#CustomerNew').val(),
            Company: $('#CompanyNew').val(),
            StartBapsDate: $('#StartDateNew').val(),
            EndBapsDate: $('#EndDateNew').val(),
            Status: $('#slAction').val(),
            Remarks: $('#Remarks').val(),
            CustomerSiteID: $('#CustomerSiteIDNew').val(),
            StipSiro: $('#StipSiroVal').val(),
        }

        if ($('#ApprovalAccess').val()) {
            params.Status = "Done";
        }

        $.ajax({
            url: "/api/RelocBAPS/submit",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            if (data != null) {
                if (Common.CheckError.Object(data)) {
                    if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                        l.stop();
                        Common.Alert.Success("Success To Submit Data");
                        Form.HideDetail();
                        Table.Search();
                    }
                }
                else {
                    l.stop();
                    Common.Alert.Warning(data.ErrorMessage);
                }
            }
            else {
                l.stop();
                Common.Alert.Error(errorThrown)
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            l.stop();
            Common.Alert.Error(errorThrown)
        });

        
    },
    Startvalidate: function () {
        var params = {
            SONumberLive: $('#SONumberNew').val(),
            SONumber: $('#SONumberOld').val(),
            CustomerSiteName: $('#CustomerSiteNameNew').val(),
            Customer: $('#CustomerNew').val(),
            Company: $('#CompanyNew').val(),
            StartBapsDate: $('#StartDateNew').val(),
            EndBapsDate: "1900-01-01",
            Status: $('#slAction').val(),
            Remarks: $('#Remarks').val()
        }

        if (params.StartBapsDate != "") {
            $.ajax({
                url: "/api/RelocBAPS/datevalidate",
                type: "POST",
                datatype: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                if ((data != null || data != "") && data.length > 3 ) {
                    $('#StartDateNew').val("");
                    Common.Alert.Warning(data)
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
            });
        }
        
    },
    Endvalidate: function () {
        var params = {
            SONumberLive: $('#SONumberNew').val(),
            SONumber: $('#SONumberOld').val(),
            CustomerSiteName: $('#CustomerSiteNameNew').val(),
            Customer: $('#CustomerNew').val(),
            Company: $('#CompanyNew').val(),
            StartBapsDate: $('#StartDateNew').val(),
            EndBapsDate: $('#EndDateNew').val(),
            Status: $('#slAction').val(),
            Remarks: $('#Remarks').val()
        }

        if (params.StartBapsDate != "") {
            $.ajax({
                url: "/api/RelocBAPS/datevalidate",
                type: "POST",
                datatype: "json",
                contentType: "application/json",
                data: JSON.stringify(params),
                cache: false
            }).done(function (data, textStatus, jqXHR) {
                if ((data != null || data != "") && data.length > 3) {
                    $('#EndDateNew').val("");
                    Common.Alert.Warning(data)
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
            });
        }
    }
}

var Control = {
    BindingSelectCompany: function () {
        var id = "#slSearchCompany"
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Company Name", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectStip: function () {
        var id = "#slSearchStip"
        $.ajax({
            url: "/api/MstDataSource/TBGSys_Stip",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select STIP", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectTenant: function () {
        var id = "#slSearchTenant"
        $.ajax({
            url: "/api/MstDataSource/TenantType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Tenant Type", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
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
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsCustomerId = $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val();
        fsStip = $("#slSearchStip").val() == null ? "" : $("#slSearchStip").val();
        fsTenant = $("#slSearchTenant").val() == null ? "" : $("#slSearchTenant").val();
        fsSONumber = $("#SearchSONumber").val() == null ? "" : $("#SearchSONumber").val();

        var params = {
            Company: fsCompanyId,
            Customer: fsCustomerId,
            Stip: fsStip,
            Tenant: fsTenant,
            SONumber: fsSONumber
        };

        var tblRaw = $("#tblRaw").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "filter": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/RelocBAPS/grid",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },

            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    extend: 'excelHtml5',
                    filename: 'Site Detail PO',
                    text: '<i class="fa fa-file-excel-o"></i>',
                    titleAttr: 'Export to Excel',
                    className: 'btn yellow btn-outline',
                    exportOptions: {
                        columns: ':not(.notexport)',
                        format: {
                            body: function (data, row, column, node) {
                                return (column <= 4) ? "\0" + data : data;
                            }
                        }
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "columns": [
                { data: "RowIndex" },
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "Done";
                        
                        if (full.Status != "Done") {
                            if ($("#hdAllowProcess").val() && full.Status == "Approve") {
                                strReturn = "<a class='btDetail notexport'> Waiting Approval </a>"; //"<button type='button' title='Approve' class='btn btn-xs green btApprove' id='btApprove" + full.ID + "'><i class='fa fa-check'></i></button>";
                            }
                            else if (!$("#hdAllowProcess").val() && full.Status == "Approve") {
                                strReturn = "Need Approval";
                            }
                            else {
                                strReturn = "<a class='btDetail notexport'> Ready To Process </a>"; //"<button type='button' title='Process' class='btn btn-xs blue btProcess' id='btProcess" + full.ID + "'><i class='fa fa-edit'></i></button>";
                            }
                        }
                        return strReturn;
                    }
                },
                
                { data: "SONumber" },
                { data: "SONumberLive" },
                { data: "StipSiro" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "Customer" },
                { data: "Company" },
                { data: "Stip" },
                { data: "TenantType" },
                

            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "lengthMenu": [[25, 50, 100, 200], ['25', '50', '100', '200']],
            "scrollY": 300,
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "destroy": true,
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

        });

        $("#tblRaw tbody").on("click", "a.btDetail", function (e) {
            e.preventDefault();
            var table = $("#tblRaw").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                $('#formDetailData').parsley().reset();
                Form.DetailShow();
                $('#SONumberOld').val(data.SONumber);
                $('#SiteNameOld').val(data.SiteName);
                $('#SiteIDOld').val(data.SiteID);
                $('#CustomerSiteIDOld').val(data.CustomerSiteID);
                $('#CustomerSiteNameOld').val(data.CustomerSiteName);
                $('#CustomerOld').val(data.Customer);
                $('#CompanyOld').val(data.Company);
                $('#StartDateOld').val(Common.Format.ConvertJSONDateTime(data.StartBapsDate));
                $('#EndDateOld').val(Common.Format.ConvertJSONDateTime(data.EndBapsDate));
                
                $('#SONumberNew').val(data.SONumberLive);
                $('#SiteNameNew').val(data.SiteName);
                $('#SiteIDNew').val(data.SiteID);
                $('#CustomerSiteIDNew').val(data.CustomerSiteID);
                $('#CustomerSiteNameNew').val(data.CustomerSiteName);
                $('#CustomerNew').val(data.Customer);
                $('#CompanyNew').val(data.Company);
                $('#StipSiroVal').val(data.StipSiro);

                Table.GridChecdocList(data.SONumberLive, data.SiteIDLive, data.Customer);
                //$('#StartDateNew').val(Common.Format.ConvertJSONDateTime(data.StartBapsDate));
                $('#EndDateNew').val(Common.Format.ConvertJSONDateTime(data.EndBapsDate));
            }
        });

        $(window).resize(function () {
            $("#tblRaw").DataTable().columns.adjust().draw();
        });
    },

    GridChecdocList: function (SoNumber, SiteID, CustomerID) {
        Data.countRow = 0;
        Data.projectChecked = 0;
        Data.bapsChecked = 0;

        var params = {
            strSoNumber: SoNumber,
            strSiteId: SiteID,
            strCustomerId: CustomerID
        };

        var tbl = $("#tblTrxCheckingDocument").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "paging": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/CheckingDoc/getCheckDocList",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            "filter": false,
            "destroy": true,
            "columns": [
                { data: "RowIndex" }
                , {
                    data: "DocName",
                    mRender: function (data, type, full) {
                        return "<a href='#' class='btn-Download' id='btDownload_" + full.RowIndex + "'>" + full.DocName + "</a>"
                    }
                }
                , { data: "FileName" }
                , {
                    data: "CheckedProject",
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        if (full.CheckedProject == true) {
                            strReturn = "<input  disabled='disabled' type='checkbox' class='cbCheckedProject' id='cbProject_" + full.IDTrx + "' checked  />";
                        } else {
                            strReturn = "<input  disabled='disabled' type='checkbox' class='cbCheckedProject' id='cbProject_" + full.IDTrx + "'  />";
                        }
                        return strReturn;
                    }
                }
                , {
                    data: "CheckedBaps",
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        var isMandatory = "";

                        if (full.Mandatory) {
                            isMandatory = " required";
                        }

                        if (checked = full.CheckedBaps == true) {
                            strReturn = "<input " + isMandatory + "   type='checkbox' class='cbCheckedBaps' id='" + full.IDTrx + "' checked />";
                        } else {
                            strReturn = "<input " + isMandatory + "   type='checkbox' class='cbCheckedBaps' id='" + full.IDTrx + "' />";
                        }

                        if (isMandatory != "") {
                            strReturn = strReturn + "<label style='color:red'>*</label>"
                        }
                        return strReturn;
                    },
                }
                , { data: "Remarks" }
                , { data: "IDTrx" }
                , { data: "LinkFile" }

            ],
            "columnDefs": [
                            { "targets": [3], "className": "text-center" },
                            { "targets": [4], "className": "text-center" },
                            { "targets": [6], "visible": false },
                            { "targets": [7], "visible": false }
            ],
            "fnDrawCallback": function () {

                Data.countRow = tbl.data().length;
                var chkAllProj = $("#tblTrxCheckingDocument").find(".cbCheckedProject");
                var totalProj = chkAllProj.length;
                if (Data.countRow == totalProj) {
                    $('#cbProject').prop("checked", true);
                }
                var chkAllBaps = $("#tblTrxCheckingDocument").find(".cbCheckedBaps:checked");
                var totalBaps = chkAllBaps.length;
                if (Data.countRow == totalBaps) {
                    $('#cbBaps').prop("checked", true);
                }
            }
        });
        $("#tblTrxCheckingDocument tbody").unbind();
        $("#tblTrxCheckingDocument tbody").on("click", ".btn-Download", function (e) {
            var table = $("#tblTrxCheckingDocument").DataTable();
            var row = table.row($(this).parents('tr')).data();
            var IsLegacy = row.IsLegacy;

            //Helper.Download(row.LinkFile,row.FileName, "application/pdf");
            Helper.DownloadDoc(row.LinkFile, row.FileName, IsLegacy, "application/pdf");
        });
    },
}

var Helper = {
    DownloadDoc: function (filePath, Document, IsLegacy, contentType) {
        var path = filePath;
        window.location.href = "/RevenueAssurance/DownloadFileProject?FilePath=" + path + "&FileName=" + Document + "&ContentType=" + contentType + "&IsLegacy=" + IsLegacy;
    },
}