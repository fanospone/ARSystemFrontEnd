Data = {};

var fsCustomerID = "";
var fsCompanyId = "";
var fsProductID = "";
var fsGroupBy = "";
var fsSoNumb = "";
var fsSiteID = "";
var fsAction = "";
var countChekedProject = 0;
var ActivityID = 0;
var _Type = "";
var DocumentCheckList = [];
var DocumentCheckListPost = [];
var SONumber = "";
var SiteId = "";
var CompanyId = "";

jQuery(document).ready(function () {

    Form.Init();

    //Table.GridSonumbList();

    $("#btSearch").unbind().click(function () {
        Table.GridSonumbList();
    });

    $("#cbProject").unbind().click(function () {
        if ($("#cbProject").prop("checked")) {
            $('.cbCheckedProject').prop("checked", true);
        } else {
            $('.cbCheckedProject').prop("checked", false);
        }

    });

    $("#btnTrxSave").unbind().click(function () {

        if ($("#formTrxCheckDoc").parsley().validate()) {

            if ($('#slNextStep').val() == null || $('#slNextStep').val() == "") {
                Common.Alert.Warning("Please Select Next Activity!");
                return;
            }

            var ValidationSubmitTrx = Helper.ValidationSubmitTrx();
            if (ValidationSubmitTrx != "") {

                Common.Alert.Warning(ValidationSubmitTrx);
            }
            else {
                // $("#tblTrxCheckingDocument").DataTable()
                Form.SubmitTrx();
            }
        }
    });


    $("#btnSave").unbind().click(function () {
        if ($('#lblSIRO').text() == "0") {
            tabler = $('#tblTrxCheckingDocument').DataTable();
            var datachecklist = tabler.rows().data();

            if (datachecklist.length < 1) {
                Common.Alert.Warning("Document Project Not Available, Please Contact Project Team !");
                return;
            }
        }

        if ($("#formChecklist").parsley().validate()) {

            var condition = true;
            /* start remark by  enhancement checking document  */
            //jQuery("#tblTrxCheckingDocument .cbCheckedBaps").each(function () {
            //    checked = $(this).prop("checked");
            //    if (!checked) {
            //        condition = false;
            //    }
            //});
            /* end  remark by enhancement checking document  */

            if (condition) {
                if ($('#lblCustomer').text() == "SMART8" || $('#lblCustomer').text() == "SMART" || $('#lblCustomer').text() == "HCPT" || $('#lblCustomer').text() == "M8") {
                    $('.pnlReviewHardcopy').show();
                    var SONumber = $('#lblSONumber').text();
                    Control.BindingHardCopyBAUK(SONumber);
                } else {
                    $('.pnlReviewHardcopy').hide();
                }
                Control.BindingNextStep();

                if (!Form.CheckDocMandatoryValid()) {
                    $('#slNextStep').val('0').trigger('change');
                    $('#slNextStep').prop('disabled', true);
                } else {
                    $('#slNextStep').val('').trigger('change');
                    $('#slNextStep').prop('disabled', false);
                }
                Form.FillRemarkApproval();
                $("#mdlTrxCheckingDocSave").modal('show');
            }
            else {
                Common.Alert.Warning("Please Check All Mandatory Document Before Go To Next Process!");
            }
        }
        // Form.SubmitTrx();
    });


    $("#cbBaps").unbind().click(function () {
        if ($("#cbBaps").prop("checked")) {
            $('.cbCheckedBaps').prop("checked", true);
        } else {
            $('.cbCheckedBaps').prop("checked", false);
        }

    });


    $("#btnCancel").unbind().click(function () {

        $(".panelSearchResult").fadeIn(500);
        $(".filter").fadeIn(500);
        $(".panelTrxCheckDoc").fadeOut(500);
        $(".HeaderData").fadeOut(500);
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
    });

    $('input[name=rbAction]').on('change', function () {
        _Type = $(this).val();
    });

    $("#slNextStep").on('change', function () {
        if ($('#slNextStep').val() == 14) //Approve To BAPS Validation
        {
            $("#rbNotComplete").prop('disabled', true);
			
			$("#rbComplete").prop('disabled', false);
            $("#rbCheckNext").prop('disabled', false);
        } else {
            $("#rbNotComplete").prop('disabled', false);
            $("#rbNotComplete").prop('checked', true);

            $("#rbComplete").prop('disabled', true);
            $("#rbCheckNext").prop('disabled', true);
        }
    });
    if ($('#DefaultFilter').val()) {
        $('#btSearch').trigger("click");
    }
});

var Table = {
    Init: function (idTable) {
        var tblSummary = $(idTable).dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });
        $(window).resize(function () {
            $(idTable).DataTable().columns.adjust().draw();
        });
    },

    Reset: function () {
        $("#slSearchCustomerID").val("").trigger('change');
        $("#slSearchSiteID").val("").trigger('change');
        $("#slSearchSoNumber").val("").trigger('change');
        $("#slSearchCompanyID").val("").trigger('change');
        $("#slSearchProductID").val("").trigger('change');

        fsOperator = "";
        fsCompanyId = "";
        fsProductType = "";
    },

    GridChecdocList: function (SoNumber, SiteID, CustomerID) {
        SONumber = SoNumber;
        Data.countRow = 0;
        Data.projectChecked = 0;
        Data.bapsChecked = 0;
        $(".panelSearchResult").fadeOut();
        $(".filter").fadeOut();
        $('#cbBaps').prop("checked", false);
        $("#tbSiteId").val(SiteID);
        $("#tbSoNumber").val(SoNumber);
        $("#tbCustomerID").val(CustomerID);
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
                /* start remark by  enhancement checking document  */
                //, {
                //    data: "CheckedBaps",
                //    orderable: false,
                //    mRender: function (data, type, full) {
                //        var strReturn = "";
                //        var isMandatory = "";

                //        if (full.Mandatory) {
                //            isMandatory = " required";
                //        }

                //        if (checked = full.CheckedBaps == true) {
                //            strReturn = "<input " + isMandatory + "   type='checkbox' class='cbCheckedBaps' id='" + full.IDTrx + "' checked />";
                //        } else {
                //            strReturn = "<input " + isMandatory + "   type='checkbox' class='cbCheckedBaps' id='" + full.IDTrx + "' />";
                //        }

                //        if (isMandatory != "") {
                //            strReturn = strReturn + "<label style='color:red'>*</label>"
                //        }
                //        return strReturn;
                //    },
                //}
                /* end remark by  enhancement checking document  */
                , {
                    data: 'IDTrx',
                    orderable: false,
                    mRender: function (data) {
                        return "<input   type='radio' class='radioCheck'  name ='radio" + data + "' id='radio" + data + "' value='1'  />";
                    }
                },
                {
                    data: 'IDTrx',
                    orderable: false,
                    mRender: function (data) {
                        return "<input   type='radio' class='radioCheck'  name ='radio" + data + "' id='radio" + data + "' value='2'  />";
                    }
                },
                {
                    data: 'IDTrx',
                    orderable: false,
                    mRender: function (data) {
                        return "<input   type='radio' class='radioCheck'  name ='radio" + data + "' id='radio" + data + "' value='3'  />";
                    }
                },
                {
                    data: 'IDTrx',
                    orderable: false,
                    mRender: function (data) {
                        return "<input   type='radio' class='radioCheck'  name ='radio" + data + "' id='radio" + data + "' value='4'  />";

                    }
                },
                {
                    data: 'IDTrx',
                    orderable: false,
                    mRender: function (data) {
                        return "<input   type='text' class='form-control' id='tb" + data + "' disabled  />";
                    }
                }
                , { data: "Remarks" }
                , { data: "IDTrx" }
                , { data: "LinkFile" }

            ],
            "columnDefs": [
                { "targets": [3, 4, 5, 6, 7], "className": "text-center" },
                { "targets": [10], "visible": false },
                { "targets": [11], "visible": false }
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
        $("#tblTrxCheckingDocument tbody").on("click", ".radioCheck", function (e) {
            var table = $("#tblTrxCheckingDocument").DataTable();
            var row = table.row($(this).parents('tr')).data();

            var docCheckVal = $("input[name='radio" + row.IDTrx + "']:checked").val();
            if (docCheckVal == 4) {
                $('#tb' + row.IDTrx).prop('disabled', false);
            } else {
                $('#tb' + row.IDTrx).prop('disabled', true);
                $('#tb' + row.IDTrx).val('');
            }

            var dataRow = {};
            dataRow.IDTrx = row.IDTrx;
            dataRow.DocCheckValue = docCheckVal;
            dataRow.Mandatory = row.Mandatory;
            dataRow.DocName = row.DocName;
            DocumentCheckList.push(dataRow);

            // console.log(DocumentCheckList);

            var index = DocumentCheckList.findIndex(function (data) {
                return data.IDTrx == row.IDTrx && data.DocCheckValue != docCheckVal;
            });

            if (index != -1) {
                DocumentCheckList.splice(index, 1);
            }


        });
        $(".panelTrxCheckDoc").fadeIn(500);
        $(".HeaderData").fadeIn(500);
        Form.ClearApproval();
    },

    GridSonumbList: function () {
        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();
        fsCustomerId = $("#slSearchCustomerID").val() == null ? "" : $("#slSearchCustomerID").val();
        fsCompanyId = $("#slSearchCompanyID").val() == null ? "" : $("#slSearchCompanyID").val();
        fsProductID = $("#slSearchProductID").val() == null ? "" : $("#slSearchProductID").val();
        fsSiteID = $("#slSearchSiteID").val() == null ? "" : $("#slSearchSiteID").val();
        fsSoNumb = $("#slSearchSoNumber").val() == null ? "" : $("#slSearchSoNumber").val();
        //fsGroupBy = $("#slGroupBy").val() == null ? "" : $("#slGroupBy").val();

        var params = {
            strCustomerId: fsCustomerId,
            strCompanyId: fsCompanyId,
            strProductId: fsProductID,
            // strGroupBy: fsGroupBy,
            strSoNumber: fsSoNumb,
            strSiteID: fsSiteID,
            mstRAActivityID: $('#slSearchStatus').val()
        };
        var tblList = $("#tblCheckingDocumentList").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/CheckingDoc/getSonumbList",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
                {
                    text: '<i class="fa fa-file-excel-o"></i> Export Summary', titleAttr: 'Export Summary to Excel', className: 'btn green', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".green"));
                        l.start();
                        Table.ExportSummary()
                        l.stop();
                    }
                },
                {
                    text: '<i class="fa fa-file-excel-o"></i> Export Summary Per Item', titleAttr: 'Export Summary to Excel', className: 'btn green', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".green"));
                        l.start();
                        Table.ExportSummaryPerItem()
                        l.stop();
                    }
                },
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[25, 50, 100, 200], ['25', '50', '100', '200']],
            "destroy": true,
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        if (full.ActivityID.toString() == "13") {
                            strReturn += "<i class='fa fa-edit btn btn-xs green link-TrxCheckDoc' data-toggle='modal' data-target='#mdlCheckingDoc'></i>"
                        }
                        return strReturn;
                    }
                },
                { data: "SoNumber" },
                { data: "CustomerID" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "CompanyID" },
                { data: "SIRO" },
                { data: "STIPNumber" },
                { data: "StipCode" },
                { data: "Product" },
                { data: "MLANumber" },
                {
                    data: "MLADate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "BaukNumber" },
                {
                    data: "BaukDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblList.data())) {
                    $(".panelSearchResult").fadeIn(1000);
                }
                l.stop(); App.unblockUI('.panelSearchResult');
                if (Data.RowSelected.length > 0) {
                    var item;
                    for (var i = 0; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        if (!Helper.IsElementExistsInArray(parseInt(item), Data.RowSelectedSite))
                            $("#Row" + item).addClass("active");
                    }
                }
            },
            "order": false,
            "scrollY": 300,
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
        });

        $("#tblCheckingDocumentList tbody").unbind();
        $("#tblCheckingDocumentList tbody").on("click", ".link-TrxCheckDoc", function (e) {
            var table = $("#tblCheckingDocumentList").DataTable();
            var row = table.row($(this).parents('tr')).data();
            $("#lblSONumber").text(row.SoNumber);
            $("#lblSiteID").text(row.SiteID);
            $("#lblCustomer").text(row.CustomerID);
            $("#lblCustomerSiteID").text(row.CustomerSiteID);
            $("#lblSIRO").text(row.SIRO);
            $("#lblSTIPNumber").text(row.STIPNumber);
            $("#lblCompany").text(row.CompanyID);
            $("#lblSiteName").text(row.SiteName);
            $("#lblCustomerSiteName").text(row.CustomerSiteName);
            $("#lblProduct").text(row.Product);
            $("#lblMLANumber").text(row.MLANumber);
            $("#lblMLADate").text(Common.Format.ConvertJSONDateTime(row.MLADate));
            $("#lblBaukNumber").text(row.BaukNumber);
            $("#lblBaukDate").text(Common.Format.ConvertJSONDateTime(row.BaukDate));
            ActivityID = row.ActivityID;
            Table.GridChecdocList(row.SoNumber, row.SiteID, row.CustomerID);
            DocumentCheckList = [];
            DocumentCheckListPost = [];
            SiteId = row.SiteID;
            CompanyId = row.CompanyID;
            Table.GridDocumentSupport();
        });
    },

    GridDocumentSupport: function () {
        var params = { companyId: CompanyId, siteId: SiteId };
        var idTbl = "#AddDocumentSupport";
        Table.Init(idTbl);
        $(idTbl).DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "paging": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/CheckingDoc/getDocumentSupport",
                "type": "GET",
                "data": params,
            },
            "filter": false,
            "destroy": true,
            "columns": [
                { data: "No" },
                {
                    data: "DocumentName",
                    mRender: function (data) {
                        return "<a href='#' class='btn-Download'>" + data + "</a>"
                    }
                },
            ],
            "columnDefs": [{ "targets": [0], "className": "text-center" }],

        });
       $(idTbl+" tbody").unbind();
        $(idTbl + " tbody").on("click", ".btn-Download", function (e) {
            var table = $(idTbl).DataTable();
            var row = table.row($(this).parents('tr')).data();
            Helper.DownloadDocSupport(row.DocumentName);
        });
    },
    Export: function () {
        fsCustomerId = $("#slSearchCustomerID").val() == null ? "" : $("#slSearchCustomerID").val();
        fsCompanyId = $("#slSearchCompanyID").val() == null ? "" : $("#slSearchCompanyID").val();
        fsProductID = $("#slSearchProductID").val() == null ? "" : $("#slSearchProductID").val();
        fsSiteID = $("#slSearchSiteID").val() == null ? "" : $("#slSearchSiteID").val();
        fsSoNumb = $("#slSearchSoNumber").val() == null ? "" : $("#slSearchSoNumber").val();
        mstRAActivityID = $('#slSearchStatus').val();

        window.location.href = "/RevenueAssurance/CheckingDocument/Export?strCustomerID=" + fsCustomerId + "&strCompanyId=" + fsCompanyId + "&strProductId=" + fsProductID
            + "&strSoNumber=" + fsSoNumb + "&strSiteID=" + fsSiteID + "&strTenantType=" + "" + "&mstRAActivityID=" + mstRAActivityID;
    },
    ExportSummary: function () {
        fsCustomerId = $("#slSearchCustomerID").val() == null ? "" : $("#slSearchCustomerID").val();
        fsCompanyId = $("#slSearchCompanyID").val() == null ? "" : $("#slSearchCompanyID").val();
        fsProductID = $("#slSearchProductID").val() == null ? "" : $("#slSearchProductID").val();
        fsSiteID = $("#slSearchSiteID").val() == null ? "" : $("#slSearchSiteID").val();
        fsSoNumb = $("#slSearchSoNumber").val() == null ? "" : $("#slSearchSoNumber").val();
        mstRAActivityID = $('#slSearchStatus').val();

        window.location.href = "/RevenueAssurance/SummaryDocumentCheck/Export?strCustomerID=" + fsCustomerId + "&strCompanyId=" + fsCompanyId + "&strProductId=" + fsProductID
            + "&strSoNumber=" + fsSoNumb + "&strSiteID=" + fsSiteID + "&strTenantType=" + "" + "&mstRAActivityID=" + mstRAActivityID;
    },
    ExportSummaryPerItem: function () {
        fsCustomerId = $("#slSearchCustomerID").val() == null ? "" : $("#slSearchCustomerID").val();
        fsCompanyId = $("#slSearchCompanyID").val() == null ? "" : $("#slSearchCompanyID").val();
        fsProductID = $("#slSearchProductID").val() == null ? "" : $("#slSearchProductID").val();
        fsSiteID = $("#slSearchSiteID").val() == null ? "" : $("#slSearchSiteID").val();
        fsSoNumb = $("#slSearchSoNumber").val() == null ? "" : $("#slSearchSoNumber").val();
        mstRAActivityID = $('#slSearchStatus').val();

        window.location.href = "/RevenueAssurance/SummaryDocumentCheckPerDoc/Export?strCustomerID=" + fsCustomerId + "&strCompanyId=" + fsCompanyId + "&strProductId=" + fsProductID
            + "&strSoNumber=" + fsSoNumb + "&strSiteID=" + fsSiteID + "&strTenantType=" + "" + "&mstRAActivityID=" + mstRAActivityID;
    }
}

var Form = {

    Init: function () {

        Control.BindingSelectCompany();
        Control.BindingSelectOperator();
        Control.BindingSelectProductType();
        Control.BindingSelectGroupBy();
        Control.BindingSelectRegional();
        Control.BindingNextStep();
        $(".BapsType").hide();
        $(".fXLBulky").hide();
        $(".fProductType").hide();
        $(".fGroupBy").hide();
        $(".panelTrxCheckDoc").hide();
        $(".panelSearchResult").hide();
        $(".panelTrxCheckDoc").hide();
        $(".HeaderData").fadeOut(500);
        //untuk validasi checkbox
        Data.RowSelected = [];
        //untuk validasi table site
        Data.RowSelectedSite = [];
    },

    ClearRowSelected: function () {
        Data.RowSelected = [];
        Data.RowSelectedSite = [];

    },

    SubmitTrx: function () {
        fsSiteID = $("#tbSiteId").val();
        fsSoNumb = $("#tbSoNumber").val();
        fsCustomerID = $("#tbCustomerID").val();
        var nextStepID;
        var checked = false;
        var id = "";
        var fsDocID = "";
        var fsBapsCheck = "";
        jQuery("#tblTrxCheckingDocument .cbCheckedBaps").each(function () {
            checked = $(this).prop("checked");
            id = $(this).prop("id");
            fsDocID += id + "|";
            if (checked == true) {
                fsBapsCheck += "true|";
            } else {
                fsBapsCheck += "false|";
            }
        });
        var l = Ladda.create(document.querySelector("#btnTrxSave"));
        var params = {
            strSiteID: fsSiteID,
            strSoNumber: fsSoNumb,
            strCustomerID: fsCustomerID,
            strDocID: fsDocID,
            strBapsChecked: fsBapsCheck,
            mstRAActivityID: $('#slNextStep').val(),
            Remarks: $('#tbRemark').val(),
            vReceiveDate: $('#lblReceiveDate').text(),
            vPICReceive: $('#lblPICReceive').text(),
            vAction: _Type,
            vRemarks: $('#tbRemarks').val(),
            vSiteName: $('#lblSiteName').text(),
            DocumentCheck: DocumentCheckListPost
        };
        $.ajax({
            url: "/api/CheckingDoc/submitTrxCheckDoc",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        })
            .done(function (data, textStatus, jqXHR) {
                if (Common.CheckError.Object(data)) {
                    if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                        Table.Init("#tblCheckingDocumentList");
                        Table.GridSonumbList();
                        Common.Alert.Success("Data Success Confirmed!");
                        Form.DoneConfirm();
                        ActivityID = 0;
                    } else {
                        Common.Alert.Warning(data.ErrorMessage);
                    }
                }
                l.stop();
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l.stop();
            });
    },

    Cancel: function () {

        Data.RowSelected = [];

    },

    ClearApproval: function () {
        $("#slAction").val("00").trigger('change');
        $("#tbUserId").val("");
        $("#tbPassword").val("");
        $("#tbRemark").val("");
    },

    DoneConfirm: function () {
        $("#mdlTrxCheckingDocSave").modal('hide');
        $(".panelTrxCheckDoc").fadeOut();
        $(".panelSearchResult").fadeIn(1500);
        $(".filter").fadeIn(1500);
        $(".HeaderData").fadeOut();
        Table.GridSonumbList();
    },

    /** start add by enhancement checking document  */
    CheckDocMandatoryValid: function () {

        for (var i = 0; i < DocumentCheckList.length; i++) {
            if (DocumentCheckList[i].Mandatory) {
                return false;
            }
        }

        return true;
    },

    FillRemarkApproval: function () {
        var textRemarks = '';
        var docCheckName = [{ 'key': '1', 'value': 'Improper Content ' }, { 'key': '2', 'value': 'Uncompleted Doc ' }, { 'key': '3', 'value': 'Wrong Upload' }, { 'key': '4', 'value': 'Other' }];

        for (var i = 0; i < DocumentCheckList.length; i++) {
            var doc = '';
            for (var j = 0; j < docCheckName.length; j++) {
                if (DocumentCheckList[i].DocCheckValue == docCheckName[j].key) {
                    doc = docCheckName[j].value;
                }

            }
            if (DocumentCheckList[i].DocCheckValue == 4) {
                doc = doc + ':' + $('#tb' + DocumentCheckList[i].IDTrx).val();
            }
            var remark = DocumentCheckList[i].DocName + ' - ' + (DocumentCheckList[i].Mandatory == true ? 'Mandatory' : 'Non Mandatory') + ' - ' + doc;

            textRemarks += (i + 1) + '. ' + remark + '\n';

            var dataDoc = {};
            dataDoc.DocumentId = DocumentCheckList[i].IDTrx;
            dataDoc.SoNumber = SONumber;
            dataDoc.CheckListType = DocumentCheckList[i].DocCheckValue;
            dataDoc.CheckListName = doc;
            dataDoc.Remark = remark;
            DocumentCheckListPost.push(dataDoc);
        }

        $('#tbRemark').val(textRemarks);
    }

    /** end add by enhancement checking document  */

}

var Control = {

    BindingSelectCompany: function () {
        var id = "#slSearchCompanyID"
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

    BindingSelectOperator: function () {
        var id = "#slSearchCustomerID";
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.OperatorId + "'>" + item.Operator + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Operator", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingSelectProductType: function () {
        var id = "#slSearchProductID";
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
                $(id).select2({ placeholder: "Select Product", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });

    },

    BindingSelectRegional: function () {
        var id = "#slSearchRegionalID";
        $.ajax({
            url: "/api/MstDataSource/Regional",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.RegionalId + "'>" + item.Regional + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Regional", width: null });

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingSelectGroupBy: function () {
        $("#slGroupBy").html("<option></option>")
        $("#slGroupBy").append("<option value='NEW' selected>New</option>");
        $("#slGroupBy").append("<option value='REPEAT'>Repeat Order</option>");
        $("#slGroupBy").select2({ placeholder: "Select Group", width: null });
    },

    BindingNextStep: function () {

        var id = "#slNextStep";
        $.ajax({
            url: "/api/MstDataSource/NextActivity",
            type: "GET",
            async: false,
            data: { CurrentActivity: ActivityID, CustomerID: $("#lblCustomer").text() }
        })
            .done(function (data, textStatus, jqXHR) {
                $(id).html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(id).append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                    })
                }
                $(id).select2({ placeholder: "Select Next Step", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

    BindingHardCopyBAUK: function (SONumber) {
        $.ajax({
            url: "/api/ReceiveDoc/GetReceiveBySonumb",
            type: "POST",
            data: { vSONumber: SONumber }
        })
            .done(function (data, textStatus, jqXHR) {
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $('#lblReceiveDate').text(item.ReceiveDate);
                        $('#lblPICReceive').text(item.PICReceive);
                        if (item.StatusDoc == "Not Complete") {
                            $("#rbNotComplete").prop('checked', true);
                        } else {
                            $("#rbCheckNext").prop('checked', true);
                        }
                        $('#tbRemarks').text(item.Remarks);
                    })
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    
}

var Helper = {

    ValidationSearch: function () {

        var msg = '';

        if ($("#slSearchCompanyName").val() == "") {
            msg = msg + "Operator must be choosed! \n"
        }
        if ($("#slSearchCompanyName").val() == "") {
            msg = msg + "Company must be choosed! \n"
        }
        if ($("#slSearchYear").val() == "") {
            msg = msg + "Year must be choosed! \n"
        }
        return msg;
    },

    ValidationSubmitTrx: function () {

        var msg = '';

        if ($("#slAction").val() == "00") {
            msg = msg + "Action must be choosed! \n"
        }
        if ($("#tbUserId").val() == "") {
            msg = msg + "User ID must be fill! \n"
        }
        if ($("#tbPassword").val() == "") {
            msg = msg + "Password must be fill! \n"
        }
        if ($("#tbRemark").val() == "") {
            msg = msg + "Remark must be fill! \n"
        }
        return msg;
    },

    ValidationCheckDoc: function () {
        var msg = '';

        if (!($("#cbBaps").prop("checked")) || !($("#cbProject").prop("checked"))) {
            msg = msg + " Check  Project And Check  BAPS must be choosed! \n"

        }
        return msg;
    },

    IsElementExistsInArray: function (value, arr) {
        var isExist = false;
        for (var i = 0; i < arr.length; i++) {
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

    Download: function (filePath, Document, contentType) {
        var path = filePath;
        window.location.href = "/RevenueAssurance/DownloadFile?FilePath=" + path + "&FileName=" + Document + "&ContentType=" + contentType;
    },

    DownloadDoc: function (filePath, Document, IsLegacy, contentType) {
        var path = filePath;
        window.location.href = "/RevenueAssurance/DownloadFileProject?FilePath=" + path + "&FileName=" + Document + "&ContentType=" + contentType + "&IsLegacy=" + IsLegacy;
    },


    DownloadDocSupport: function (fileName) {
        window.location.href = "/RevenueAssurance/DownloadDocumentSuppoert?fileName=" + fileName + "&companyId=" + CompanyId + "&siteId=" + SiteId;

    },


}