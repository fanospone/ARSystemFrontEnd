Data = {};
ProcessData = [];
DoneData = [];

var fsUserID = "";
var fsCompanyId = "";
var fsCustomerID = "";
var fsBAPS = "";
var fsPO = "";
var fsSONumber = "";
var fsTransactionID = "";
var CurrentTab = 0;
var fsYear = "";
var fsQuartal = "";
var fsBapsType = "";

var fsID = "";
var fsTerm = "";
var fsResult = "";
var fsStartPeriodDate = "";
var fsEndPeriodDate = "";
var fsStartDate = "";
var fsEndDate = "";
var fsTotalPrice = "";
var fsCounter = "";
var fsBaseLeasePrice = "";
var fsServicePrice = "";
var fsState = "";

$("#btSaveSplitData").unbind().click(function () {
    fsState = "Create";
    var Validation = Helper.Validation();
    if (Validation != "") {
        $("#lblError").html(Validation);
    }
    else {
        Form.SaveRTIPartition();
    }

});

$("#btEditSplitData").unbind().click(function () {
    fsState = "Edit";
    var Validation = Helper.Validation();
    if (Validation != "") {
        $("#lblError").html(Validation);
    }
    else {
        Form.SaveRTIPartition();
    }
});

$("#btDeleteSplitData").unbind().click(function () {
    $('#mdlToDelete').modal('toggle');
});

$("#btCancelData").unbind().click(function () {
    Form.ResetSplit();
});

$("#btYesConfirmDelete").unbind().click(function () {
    Form.DeleteRTIPartition();
});



jQuery(document).ready(function () {
    $("#slSearchBAPS").select2({
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

    $("#slSONumber").select2({
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

    $("#slSearchPO").select2({
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

    $('.PanelUpload').hide();

    $('#pnlUpload').hide();

    Form.Init();
    Table.Init();
    TableDone.Init();
    Table.Reset();
    Data.RowSelectedRaw = [];
    Data.RowSelectedProcess = [];

    $("#formSearch").submit(function (e) {

        e.preventDefault();

        if (CurrentTab == 0) {
            Table.Search();
        }
        else {
            TableDone.Search();
        }
    });

    $("#btSearch").click(function (e) {
        e.preventDefault();
        if ($("#formSearch").parsley().validate()) {

            var params = Control.GetParam();
            if (params.CustomerID == 'TSEL' && CurrentTab == 0 && (params.BAPS == null || params.BAPS == "")) {
                Common.Alert.Warning("Please choose BAPS !"); return;
            }

            if (CurrentTab == 0) {
                Table.Search();
            }
            else {
                TableDone.Search();
            }
        }
    });

    $("#btReset").unbind().click(function () {
        Table.Reset();
    });

    $('#tabRTI').unbind().on('tabsactivate', function (event, ui) {
        var newIndex = ui.newTab.index();
        CurrentTab = 0;
        var vparams = Control.GetParam();
        $('#btSave').show();

        if (newIndex == 0) {
            if (vparams.CustomerID != null && vparams.CompanyID != null && vparams.BAPS != null && vparams.CustomerID != '' && vparams.CompanyID != '' && vparams.BAPS != '') {
                Table.Search();
            }
        }
        else {
            CurrentTab = CurrentTab + 1;
            $('#pnlUpload').hide();
            $('#btSave').hide();
            if (vparams.CustomerID != null && vparams.CompanyID != null && vparams.BAPS != null && vparams.CustomerID != '' && vparams.CompanyID != '' && vparams.BAPS != '')
                TableDone.Search();
        }
    });

    $('#postedFile1').on('change', function () {
        var input = document.getElementById('postedFile1');
        var infoArea = document.getElementById('file-upload-filename');
        // the change event gives us the input it occurred in 
        //var input = event.srcElement;

        // the input has an array of files in the `files` property, each one has a name that you can use. We're just using the name here.
        var fileName = input.files[0].name;

        // use fileName however fits your app best, i.e. add it into a div
        infoArea.textContent = 'File name: ' + fileName;
    });

    $('#tblRaw').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');


        if (this.checked) {
            $(this).parents('tr').addClass("active");
            $(".Row" + id).addClass("active");
            Data.RowSelectedRaw.push(parseInt(id));
            //var OP = table.row(this).data(); //$(this).parents('tr');
            //console.log(OP);
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
        Control.BindingSelectOperators($('#slSearchCustomer'));
        Control.BindingSelectCompany($('#slSearchCompany'));
        Control.BindingSelectBapsType($('#slSearchBapsType'));
        Control.BindingSelectPowerType($('#slSearchPowerType'));
        //$("#slSearchBAPS").val("").trigger("change");
        //$("#slSearchBAPS").select2({ placeholder: "Select BAPS", width: null });
        //Control.BindingSelectBAPS($('#slSearchBAPS'));
        //Control.BindingSelectPO($('#slSearchPO'));
        //Control.BindingSelectArea($('#slSearchArea'));
        //Control.BindingSelectRegional($('#slSearchRegional'));
        $("#slSearchCompany").val("").trigger("change");
        $("#slSearchCustomer").val("").trigger("change");
        $("#slRenewalYear").select2({ placeholder: "Select Renewal Year", width: null });
        $("#slQuartal").select2({ placeholder: "Select Quartal", width: null });
        $("#slRenewalYear").val("").trigger("change");
        $("#slQuartal").val("").trigger("change");

        //$("#slSearchArea").val("").trigger("change");
        //$("#slSearchRegional").val("").trigger("change");
        //$("#slSearchYear").val("").trigger("change");
        //$("#slSearchType").val("").trigger("change");
        //$("#slSearchCurrency").val("").trigger("change");
        $(".panelSearchZero").hide();
        $('#tabRTI').tabs();
        //$("#slSearchPO").val("").trigger("change");
        //$("#slSearchPO").select2({ placeholder: "Select PO", width: null });

        //$('#POFilter').hide();

        $('#btEditSplitData').hide();

    },
    UploadRTI: function () {
        //var oTable = $('#tblRaw').DataTable();
        //var rowcollection = oTable.$(".checkboxes:checked", { "page": "all" });
        if (Data.RowSelectedRaw.length < 1) {
            Common.Alert.Warning("Can`t Process RTI without detail data !"); return;
        }

        Data.SiteRow = Data.RowSelectedRaw;               

        //if (Control.ValidationSplitAmount() == false) {
        //    Common.Alert.Warning("Can`t Process BaseLease without Service !"); return;
        //}

        var l = Ladda.create(document.querySelector("#btSave"))
        var formData = new FormData();

        var CompanyID = ($("#slSearchCompany").val() == null || $("#slSearchCompany").val() == "") ? "" : $("#slSearchCompany").val();
        var CustomerID = ($("#slSearchCustomer").val() == null || $("#slSearchCustomer").val() == "") ? "" : $("#slSearchCustomer").val();
        var BapsType = ($("#slSearchBapsType").val() == null || $("#slSearchBapsType").val() == "") ? "" : $("#slSearchBapsType").val();
        var PONumber = $("#slSearchPO").val() == null ? "" : $("#slSearchPO").val();

        //var Area = ($("#slSearchArea").val() == null || $("#slSearchArea").val() == "") ? "" : $("#slSearchArea").val();
        //var Regional = Data.Selected.RegionID;
        //var Year = Data.Selected.Year;
        //var Type = ($("#slSearchType").val() == null || $("#slSearchType").val() == "") ? "" : $("#slSearchType").val();
        //var Currency = ($("#slSearchCurrency").val() == null || $("#slSearchCurrency").val() == "") ? "" : $("#slSearchCurrency").val().toString();

        formData.append("CompanyID", CompanyID);
        formData.append("CustomerID", CustomerID);
        formData.append("Area", "");
        formData.append("Regional", "");
        formData.append("Year", (new Date()).getFullYear());
        formData.append("Type", BapsType);
        formData.append("Currency", "IDR");
        formData.append("mstRAActivityID", 12);

        formData.append("TotalBoq", 0);
        formData.append("TotalPO", 0);

        //rowcollection.each(function (index, elem) {
        //    var checkbox_value = elem.id;

        //    if (!Helper.IsElementExistsInArray(checkbox_value, Data.SiteRow)) {
        //        Data.SiteRow.push(parseInt(checkbox_value));
        //    }
        //    //Do something with 'checkbox_value'
        //});

        var ListID = Data.SiteRow.toString();
        var lengthid = ListID.split(',');

        if (lengthid.length == 1)
            ListID = ListID + ',0';     

        var lengthpo = PONumber.length;

        var strPONumber = "";

        for (var i = 0; i < lengthpo; i++)
        {
            if (strPONumber == "") {
                strPONumber += "'" + PONumber[i] + "'";
            }else
            {
                strPONumber += ",'" + PONumber[i] + "'";
            }
        }

        formData.append("ListID", ListID);
        formData.append("TotalSite", lengthid.length);
        formData.append("PONumber", strPONumber);

        var fileInput = document.getElementById("postedFile1");
        if (document.getElementById("postedFile1").files.length != 0) {

            fsFileName = fileInput.files[0].name;
            formData.append("FileName", fsFileName);

            fsFile = fileInput.files[0];

            fsExtension = fsFileName.split('.').pop().toUpperCase();

            if ((fsFile.size / 1024) > 2048) {
                Common.Alert.Warning("Upload File Can`t bigger then 2048 bytes (2mb)."); return;
            }

            if (fsExtension != "XLS" && fsExtension != "XLSX" && fsExtension != "PDF") {
                Common.Alert.Warning("Please upload an Excel or PDF File."); return;
            }
            else {

                formData.append('File', fsFile);
            }

            errors = false;
        }
        else if (BapsType != "INF")
        {
            Common.Alert.Warning("Please Select Document."); return;
        }

        $.ajax({
            url: '/api/RTI/Upload',
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
            if (data !== "Exception") {
                if (data.length <= 0) {
                    $('.panelSearchResult').find('input:file').val('');
                    Common.Alert.Warning("The uploaded file is invalid or empty. Please use the available template, and fill the file correctly with valid product data.");
                } else {
                    Data.RowSelectedRaw = [];
                    $('#mdlDetailTotalSite').modal('hide');
                    $('.panelSearchResult').find('input:file').val('');
                    Common.Alert.Success("Upload File Success!");
                    $('#file-upload-filename').text("");
                    Table.Search();
                }
            } else {
                $('.panelSearchResult').find('input:file').val('');
                Common.Alert.Warning("The uploaded file is invalid. Please download the latest template by clicking the Download Template button.");
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        });
    },
    //SearchData: function () {
    //    //var params = {
    //    //    CompanyID: $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val(),
    //    //    CustomerID: $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val(),
    //    //    Area: $("#slSearchArea").val() == null ? "" : $("#slSearchArea").val(),
    //    //    Regional: $("#slSearchRegional").val() == null ? "" : $("#slSearchRegional").val().toString(),
    //    //    Year: $("#slSearchYear").val() == null ? "" : $("#slSearchYear").val(),
    //    //    Type: $("#slSearchType").val() == null ? "" : $("#slSearchType").val(),
    //    //    Currency: $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val().toString()
    //    //};

    //    var params = {
    //        CompanyID: $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val(),
    //        CustomerID: $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val(),
    //        Area: $("#slSearchArea").val() == null ? "" : $("#slSearchArea").val(),
    //        Regional: $("#slSearchRegional").val() == null ? "" : $("#slSearchRegional").val().toString(),
    //        Year: $("#slSearchYear").val() == null ? "" : $("#slSearchYear").val(),
    //        Type: $("#slSearchType").val() == null ? "" : $("#slSearchType").val(),
    //        Currency: $("#slSearchCurrency").val() == null ? "" : $("#slSearchCurrency").val().toString()
    //    };

    //    if (params.Year == null || params.Year == "") {
    //        Common.Alert.Warning("Please Select Year !"); return;
    //    }

    //    if (params.CompanyID == null || params.CompanyID == "") {
    //        Common.Alert.Warning("Please Select Company !"); return;
    //    }

    //    if (params.CustomerID == null || params.CustomerID == "") {
    //        Common.Alert.Warning("Please Select Customer !"); return;
    //    }

    //    if (params.Type == null || params.Type == "") {
    //        Common.Alert.Warning("Please Select Type !"); return;
    //    }

    //    //if (params.Regional == null || params.Regional == "") {
    //    //    Common.Alert.Warning("Please Select Regional !"); return;
    //    //}

    //    //Table.Search();

    //    var l = Ladda.create(document.querySelector("#btSearch"))

    //    $.ajax({
    //        "url": "/api/RTI/GetList",
    //        "type": "POST",
    //        "datatype": "json",
    //        "data": params,
    //    }).done(function (data, textStatus, jqXHR) {
    //        if (data !== "Exception") {
    //            //Table.Search(data.data);
    //            ProcessData = data.data;
    //            DoneData = data.done;
    //            if (CurrentTab == 0) {
    //                Table.Search(ProcessData);
    //            }
    //            else {
    //                TableDone.Search(DoneData);
    //            }
    //            //TableDone.Search(data.done);
    //        } else {
    //            Common.Alert.Warning("Error on system.");
    //        }
    //        l.stop();
    //    }).fail(function (jqXHR, textStatus, errorThrown) {
    //        Common.Alert.Error(errorThrown)
    //        l.stop()
    //    });
    //},
    View: function () {
        var params = Control.GetParam();
        if (params.CustomerID == 'TSEL') {
            $('#slSearchPO').hide();
            if (params.CompanyID != null && params.CompanyID != '')
                Control.BindingSelectBAPS($("#slSearchBAPS"));
        }
    },
    CheckingPeriod: function () {
        var l = Ladda.create(document.querySelector("#btSaveSplitData"))
        l.start();

        fsStartDate = $("#tbStartInvoiceDate").val() == "" ? 0 : $("#tbStartInvoiceDate").val();

        var params = {
            trxReconcileID: fsID,
            StartInvoiceDate: fsStartDate,
            Term: fsTerm
        };

        $.ajax({
            url: "/api/RTI/CheckingPeriod",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            async: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (data) {
                fsResult = data;
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
    },

    SaveRTIPartition: function () {

        var l = Ladda.create(document.querySelector("#btSaveSplitData"))

        fsStartDate = $("#tbStartInvoiceDate").val() == "" ? 0 : $("#tbStartInvoiceDate").val();
        fsEndDate = $("#tbEndInvoiceDate").val() == "" ? 0 : $("#tbEndInvoiceDate").val();
        fsTotalPrice = $("#txtTotalPrice").val() == "" ? 0 : $("#txtTotalPrice").val();

        var params = {
            State: fsState,
            trxReconcileID: fsID,
            Term: fsTerm,
            EndInvoiceDate: fsEndDate,
            StartPeriodInvoiceDate: fsStartPeriodDate,
            EndPeriodInvoiceDate: fsEndPeriodDate,
            CustomerID: fsCustomerID,
            BaseLeasePrice: fsBaseLeasePrice,
            ServicePrice: fsServicePrice,
            DropFODistance: Data.Selected.DropFODistance,
            ProductID: Data.Selected.ProductID
        };

        $.ajax({
            url: "/api/RTI/SaveRTIPartition",
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
                //Common.Alert.Successhtml("Data Success Save With BAPS Number :<b> " + data.BAPSNumber + "</b>")
            }
            l.stop();

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Form.ResetSplit();
            Table.Split(fsID);
        })
    },
    UpdateRTIPartition: function () {

        var l = Ladda.create(document.querySelector("#btEditSplitData"))

        fsStartDate = $("#tbStartInvoiceDate").val() == "" ? 0 : $("#tbStartInvoiceDate").val();
        fsEndDate = $("#tbEndInvoiceDate").val() == "" ? 0 : $("#tbEndInvoiceDate").val();
        fsTotalPrice = $("#txtTotalPrice").val() == "" ? 0 : $("#txtTotalPrice").val();

        var params = {
            ID: fsSplitID,
            StartInvoiceDate: fsStartDate,
            EndInvoiceDate: fsEndDate,
            AmountIDR: fsTotalPrice
        };

        $.ajax({
            url: "/api/RTI/UpdateRTIPartition",
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
                //Common.Alert.Successhtml("Data Success Save With BAPS Number :<b> " + data.BAPSNumber + "</b>")
            }
            l.stop();

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop();
        })
        .always(function (jqXHR, textStatus) {
            Form.ResetSplit();
            Table.Split(fsID);
        })
    },

    DeleteRTIPartition: function () {
        var l = Ladda.create(document.querySelector("#btYesConfirmDelete"))

        var params = {
            trxReconcileID: fsID,
            Term: fsTerm
        };

        $.ajax({
            url: "/api/RTI/DeleteRTIPartition",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            async: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (data) {
                Common.Alert.Success("Data Success Delete!")
                $('#mdlToDelete').modal('hide');
                $('#mdlSplit').modal('hide');
            }
            l.stop();

        })
         .fail(function (jqXHR, textStatus, errorThrown) {
             Common.Alert.Error(errorThrown)
             l.stop();
         })

    },
    ResetSplit: function () {
        //$('#btSaveSplitData').show();
        //$('#btEditSplitData').hide();

        //$('#tbStartInvoiceDate').val("");
        $('#tbEndInvoiceDate').val("");
        $('#txtTotalPrice').val("");
        $('#lblError').text("");
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

        var tblSplit = $('#tblSplit').DataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblSplit").DataTable().columns.adjust().draw();
        });
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        var i = 0;
        $('#pnlUpload').hide();
        
        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsCustomerId = $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val();
        fsBAPSNumber = $("#slSearchBAPS").val() == null ? "" : $("#slSearchBAPS").val();
        fsSONumber = $("#slSONumber").val() == null ? "" : $("#slSONumber").val();
        fsYear = $("#slRenewalYear").val() == null ? "" : $("#slRenewalYear").val();
        fsQuartal = $("#slQuartal").val() == null ? "" : $("#slQuartal").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsPONumber = $("#slSearchPO").val() == null ? "" : $("#slSearchPO").val();

        var params = {
            CompanyId: fsCompanyId,
            CustomerId: fsCustomerId,
            strBAPSNumber: fsBAPSNumber,
            strSONumber: fsSONumber,
            Year: fsYear,
            Quartal: fsQuartal,
            BapsType: fsBapsType,
            strPONumber: fsPONumber
        };

        var tblRaw = $("#tblRaw").DataTable({
            "columnDefs": [
                { "searchable": true, "targets": 0 }
            ],
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/RTI/grid",
                "type": "POST",
                "datatype": "json",
                //"data": Control.GetParam(),
                "data":params,
            },
            buttons: [

                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.Export();
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[25, 50, 100, 200, 7000], ['25', '50', '100', '200', '7000']],
            "destroy": true,
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        {
                            if (full.StatusRecon <= 3) {
                                if (Helper.IsElementExistsInArray(full.Id, Data.RowSelectedRaw)) {
                                    $("#Row" + full.Id).addClass("active");
                                    strReturn += "<label id='" + full.Id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.Id + "'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                                }
                                else {
                                    strReturn += "<label id='" + full.Id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.Id + "'><input type='checkbox' class='checkboxes' /><span></span></label>";
                                }
                            }
                            else {
                                strReturn += "<label id='" + full.Id + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline " + full.Id + "'><input type='checkbox' class='checkboxes' disable/><span></span></label>";
                            }

                            return strReturn;
                        }
                    }
                },
                //{
                //    mRender: function (data, type, full) {
                //        var strReturn = "<button type='button' title='Edit' class='btn btn-xs green btEdit'><i class='fa fa-edit'></i></button>";
                //        return strReturn;
                //    }
                //},
                //{
                //    data: "SONumber", mRender: function (data, type, full) {
                //        return "<a class='btDetail'>" + data + "</a>";
                //    }
                //},
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        if (full.CustomerInvoice == "XL") {
                            var strReturn = "<button type='button' title='Edit' class='btn btn-xs green btSplit' id='btSplit" + full.ID + "'><i class='fa fa-edit'></i></button>";
                        }
                        return strReturn;
                    }
                },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },

                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "RegionName" },
                { data: "ProvinceName" },

                { data: "ResidenceName" },
                { data: "PONumber" },
                { data: "BAPSNumber" },
                { data: "MLANumber" },
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
                //{
                //    data: "RFIDate", render: function (data) {
                //        return Common.Format.ConvertJSONDateTime(data);
                //    }
                //},
                //{
                //    data: "BaufDate", render: function (data) {
                //        return Common.Format.ConvertJSONDateTime(data);
                //    }
                //},
                { data: "Term" },
                { data: "BapsType" },
                { data: "CustomerInvoice" },
                { data: "CustomerID" },
                { data: "CompanyInvoice" },
                { data: "Company" },
                { data: "StipSiro" },
                { data: "InvoiceTypeName" },
                { data: "BaseLeaseCurrency" },
                { data: "ServiceCurrency" }, //fix type from SecrviceCurrency
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
                { data: "DeductionAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InflationAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "TotalPaymentRupiah", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
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
                leftColumns: 3 /* Set the 2 most left columns as fixed columns */
            },
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
                i += 1;
            },
            "initComplete": function () {
                if (i > 0 && fsBapsType != "INF") {
                    $('#pnlUpload').show();
                }
                /* Add Select All Checkbox to the first TH of the grid, to prevent the Cloned Grid messed up the control */
                var checkbox = '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">' +
                    '<input type="checkbox" name="SelectAll" class="group-checkable" data-set="#tblRaw .checkboxes" />' +
                    '<span></span> ' +
                    '</label>';
                var th = $("th.select-all-raw").html(checkbox);

                /* Bind Event to Select All Checkbox in the Cloned Table */
                $("th.select-all-raw").unbind().on("change", ".group-checkable", function (e) {
                    Data.RowSelectedRaw = [];
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

        $("#tblRaw tbody").on("click", "button.btSplit", function (e) {
            e.preventDefault();
            Form.ResetSplit();
            var table = $("#tblRaw").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                fsID = data.Id;
                fsTerm = data.Term;
                fsStartPeriodDate = data.StartInvoiceDate;
                fsEndPeriodDate = data.EndInvoiceDate;
                fsCustomerID = data.CustomerID;
                fsBaseLeasePrice = data.BaseLeasePrice;
                fsServicePrice = data.ServicePrice;

                $('#tbStartInvoiceDate').val(Common.Format.ConvertJSONDateTime(data.StartInvoiceDate));
                $('#txtSoNumber').val(data.SONumber);
                $('#txtTerm').val(data.Term);
                $('#txtBaseLeasePrice').val(Helper.numformat(data.BaseLeasePrice));
                $('#txtDeductionPrice').val(Helper.numformat(data.DeductionAmount));
                $('#tbStartDate').val(Common.Format.ConvertJSONDateTime(data.StartInvoiceDate));
                $('#tbEndDate').val(Common.Format.ConvertJSONDateTime(data.EndInvoiceDate));
                $('#txtStipSiro').val(data.StipSiro);
                $('#txtCustomerID').val(data.CustomerID);
                $('#txtServicePrice').val(Helper.numformat(data.ServicePrice));
                $('#mdlSplit').modal('toggle');

                Table.Split(fsID);

            }
        });

        $(window).resize(function () {
            $("#tblRaw").DataTable().columns.adjust().draw();
        });
    },

    Split: function (fsID) {

        var params = {
            trxReconcileID: fsID
        };

        var tblSplit = $("#tblSplit").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/RTI/GetListReconcilePartition",
                "type": "POST",
                "datatype": "json",
                "data": params
            },
            buttons: [],
            "filter": false,
            "bLengthChange": false,
            //"lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            "columns": [
                        {
                            mRender: function (data, type, full) {
                                var strReturn = "";

                                if (Common.Format.ConvertJSONDateTime(full.StartInvoiceDate) == $("#tbStartDate").val()) {

                                    strReturn = "<button type='button' title='Edit' class='btn btn-xs green btEditSplit' id='btEditSplit" + full.ID + "'><i class='fa fa-edit'></i></button>";
                                }
                                return strReturn;
                            }
                        },
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
                        { data: "AmountIDR", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                        //{ data: "AmountUSD", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "fnDrawCallback": function () {

                var table = $('#tblSplit').DataTable();
                fsCounter = table.data().count();

                if (fsCounter != 0) {
                    $('#btSaveSplitData').hide();
                    $('#btEditSplitData').show();
                } else
                {
                    $('#btSaveSplitData').show();
                    $('#btEditSplitData').hide();
                }


                if (Common.CheckError.List(tblSplit.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();

                }
            },
            "order": [],

            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },

        });

        $("#tblSplit tbody").on("click", "button.btEditSplit", function (e) {
            e.preventDefault();
            //Form.ResetSplit();

            //$('#btSaveSplitData').hide();
            $('#btEditSplitData').show();

            var table = $("#tblSplit").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                Data.Selected = data;
                fsSplitID = data.ID;

                $('#tbStartInvoiceDate').val(Common.Format.ConvertJSONDateTime(data.StartInvoiceDate));
                $('#tbEndInvoiceDate').val(Common.Format.ConvertJSONDateTime(data.EndInvoiceDate));
                $('#txtTotalPrice').val(Helper.numformat(data.AmountIDR));

            }
        });
    },

    Reset: function () {
        $("#slSearchCompany").val("").trigger("change");
        $("#slSearchCustomer").val("").trigger("change");
        $("#slRenewalYear").val("").trigger("change");
        $("#slQuartal").val("").trigger("change");
        $("#slSearchBapsType").val("").trigger("change");
        $("#slSearchPowerType").val("").trigger("change");
        $("#slSONumber").val("").trigger("change");
        $("#slSearchBAPS").val("").trigger("change");
        $("#slSearchPO").val("").trigger("change");

        var tblRaw = $("#tblRaw").DataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

    },

    Export: function () {

        var params = Control.GetParam();
        fsCompanyId = params.CompanyID;
        fsCustomerID = params.CustomerID;
        fsBAPS = params.BAPS;
        fsPO = params.PO;
        var SoNumberFilter = params.SONumber
        fsYear = params.Year;
        fsQuartal = params.Quartal;
        fsBapsType = params.BapsType

        window.location.href = "/RevenueAssurance/Input/ExportRTI?strCompanyId=" + fsCompanyId
            + "&strOperator=" + fsCustomerID
            + "&strBAPS=" + fsBAPS
            + "&SONumber=" + SoNumberFilter
            + "&strYear=" + fsYear
            + "&strQuartal=" + fsQuartal
            + "&strBapsType=" + fsBapsType
        ;
    },
}

var TableDone = {
    Init: function () {
        var tblRaw = $('#tblDone').DataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $(window).resize(function () {
            $("#tblDone").DataTable().columns.adjust().draw();
        });
    },

    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        
        fsCompanyId = $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val();
        fsCustomerId = $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val();
        fsBAPSNumber = $("#slSearchBAPS").val() == null ? "" : $("#slSearchBAPS").val();
        fsSONumber = $("#slSONumber").val() == null ? "" : $("#slSONumber").val();
        fsYear = $("#slRenewalYear").val() == null ? "" : $("#slRenewalYear").val();
        fsQuartal = $("#slQuartal").val() == null ? "" : $("#slQuartal").val();
        fsBapsType = $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val();
        fsPONumber = $("#slSearchPO").val() == null ? "" : $("#slSearchPO").val();

        var params = {
            CompanyId: fsCompanyId,
            CustomerId: fsCustomerId,
            strBAPSNumber: fsBAPSNumber,
            strSONumber: fsSONumber,
            Year: fsYear,
            Quartal: fsQuartal,
            BapsType: fsBapsType,
            strPONumber: fsPONumber
        };

        var tblDone = $("#tblDone").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/RTI/gridDone",
                "type": "POST",
                "datatype": "json",
                //"data": Control.GetParam(),
                "data": params
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        TableDone.Export();
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[25, 50, 100, 200, 7000], ['25', '50', '100', '200', '7000']],
            "destroy": true,
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {

                        if (full.FilePath == null)
                            //return "<a class='btUpload'><i class='fa fa-upload'></i></a>";
                            return "";
                        else
                            var FileNames = full.FilePath.split('\\');
                            var Name = FileNames[3].toString();

                            return "<a class='files' target='_blank' href='/RevenueAssurance/Download?FilePath=" + full.FilePath + "&PONumber=0&FileName=" + Name + "&ContentType=" + full.ContentType + "'><i class='fa fa-download'></i></a>";
                    }
                },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },

                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "RegionName" },
                { data: "ProvinceName" },

                { data: "ResidenceName" },
                { data: "PONumber" },
                { data: "BAPSNumber" },
                { data: "MLANumber" },
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
                //{
                //    data: "RFIDate", render: function (data) {
                //        return Common.Format.ConvertJSONDateTime(data);
                //    }
                //},
                //{
                //    data: "BaufDate", render: function (data) {
                //        return Common.Format.ConvertJSONDateTime(data);
                //    }
                //},
                { data: "Term" },
                { data: "BapsType" },
                { data: "CustomerInvoice" },
                { data: "CustomerID" },
                { data: "CompanyInvoice" },
                { data: "Company" },
                { data: "StipSiro" },
                { data: "InvoiceTypeName" },
                { data: "BaseLeaseCurrency" },
                { data: "ServiceCurrency" },
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

                                {
                                    data: "BAPSConfirmDate", render: function (data) {
                                        return Common.Format.ConvertJSONDateTime(data);
                                    }
                                }, {
                                    data: "PostingInvoiceDate", render: function (data) {
                                        return Common.Format.ConvertJSONDateTime(data);
                                    }
                                }, {
                                    data: "CreateInvoiceDate", render: function (data) {
                                        return Common.Format.ConvertJSONDateTime(data);
                                    }
                                },
                { data: "BapsType" },
                { data: "InvoiceTypeName" },

                { data: "BaseLeasePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "ServicePrice", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "DeductionAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "InflationAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "TotalPaymentRupiah", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblDone.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');

            },
            "order": [],
            "scrollY": 300, /* Enable vertical scroll to allow fixed columns */
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 3 /* Set the 2 most left columns as fixed columns */
            },
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
            "initComplete": function () {

            }
        });

        $(window).resize(function () {
            $("#tblDone").DataTable().columns.adjust().draw();
        });
    },

    Reset: function () {
        $("#slSearchCompany").val("").trigger("change");
        $("#slSearchCustomer").val("").trigger("change");
        $("#slSearchBAPS").val("").trigger("change");
        $("#slSearchPO").val("").trigger("change");
        var tblRaw = $('#tblDone').DataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

    },

    Export: function () {
        var params = Control.GetParam();
        fsCompanyId = params.CompanyID;
        fsCustomerID = params.CustomerID;
        fsBAPS = params.BAPS;
        fsPO = params.PO;
        fsYear = params.Year;
        fsQuartal = params.Quartal;
        fsBapsType = params.BapsType;
        fsPowerType = params.PowerType;
        var SoNumberFilter = params.SONumber;

        window.location.href = "/RevenueAssurance/Input/ExportRTIDone?strCompanyId=" + fsCompanyId
            + "&strOperator=" + fsCustomerID
            + "&strBAPS=" + fsBAPS
            + "&strPO=" + fsPO
            + "&strYear=" + fsYear
            + "&strQuartal=" + fsQuartal
            + "&strBapsType=" + fsBapsType
            + "&strPowerType=" + fsPowerType
            + "&SONumber=" + SoNumberFilter
            + "&isRaw=1";
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

    BindingSelectOperators: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item.OperatorId.trim() == 'TSEL' || item.OperatorId.trim() == 'ISAT' || item.OperatorId.trim() == 'XL' || item.OperatorId.trim() == 'MITEL' || item.OperatorId.trim() == 'PAB' || item.OperatorId.trim() == 'TELKOM' || item.OperatorId.trim() == 'HCPT')
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

    BindingSelectArea: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/AreaList",
            type: "GET",
            data: {}
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("");

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    elements.append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                })
            }

            $("#slSearchArea").val("").trigger("change");
            elements.select2({ placeholder: "Select Area", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectRegional: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/Regional",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("<option></option>")
            elements.append("<option value=' '>No Filter</option>");

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    elements.append("<option value='" + item.RegionalId + "'>" + item.RegionalName + "</option>");
                })
            }

            elements.select2({ placeholder: "Select Regional", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectPO: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/ListPO",
            type: "GET",
            data: {}
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("");

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    elements.append("<option value='" + item.Text + "'>" + item.Text + "</option>");
                })
            }

            $("#slSearchPO").val("").trigger("change");
            //elements.select2({ placeholder: "Select PO", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectBAPS: function (elements) {
        var vparam = this.GetParam();
        var params = "";

        if (vparam.CustomerID == 'TSEL') {
            params = " AND CustomerID = '" + vparam.CustomerID + "' ";

            if (vparam.CompanyID != null) {
                params = params + " AND CompanyID = '" + vparam.CompanyID + "' ";
            }
        }
        else {
            params = " AND CustomerID = '" + vparam.CustomerID + "' ";
        }

        $.ajax({
            url: "/api/MstDataSource/ListBAPS",
            type: "GET",
            data: { param: params }
        })
        .done(function (data, textStatus, jqXHR) {
            elements.html("");

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    elements.append("<option value='" + item.Text + "'>" + item.Text + "</option>");
                })
            }

            $("#slSearchBAPS").val("").trigger("change");
            //elements.select2({ placeholder: "Select BAPS", width: null });

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

    GetParam: function () {
        var SONumber = $("#slSONumber").val() == null ? "" : $("#slSONumber").val();
        var SoNumberFilter = "";

        if (SONumber != null && SONumber != "") {
            SoNumberFilter = "0";
            for (var i = 0; i < SONumber.length; i++) {
                SoNumberFilter += ("," + SONumber[i].toString());
            }
        }

        var params = {
            CompanyID: $("#slSearchCompany").val() == null ? "" : $("#slSearchCompany").val(),
            CustomerID: $("#slSearchCustomer").val() == null ? "" : $("#slSearchCustomer").val(),
            BAPS: $("#slSearchBAPS").val() == null ? "" : $("#slSearchBAPS").val().toString(),
            PO: $("#slSearchPO").val() == null ? "" : $("#slSearchPO").val().toString(),
            SONumber: SoNumberFilter,
            Year : $("#slRenewalYear").val() == null ? "" : $("#slRenewalYear").val(),
            Quartal : $("#slQuartal").val() == null ? "" : $("#slQuartal").val(),
            BapsType: $("#slSearchBapsType").val() == null ? "" : $("#slSearchBapsType").val(),
            PowerType: $("#slSearchPowerType").val() == null ? "" : $("#slSearchPowerType").val(),
            isRaw: CurrentTab
        };

        return params;
    },

    BindProRateAmount: function () {
        //var CustomerID = $('#txtCustomerID').val();
        var CustomerID = fsCustomerID;
        var InvoiceStartDate = $('#tbStartInvoiceDate').val();
        var InvoiceEndDate = $('#tbEndInvoiceDate').val();
        var InvoiceAmount = $('#txtBaseLeasePrice').val().replace(/,/g, "");
        var ServiceAmount = $('#txtServicePrice').val().replace(/,/g, "");
        var DeductionAmount = "0";

        var params = { CustomerID: CustomerID, StartInvoiceDate: InvoiceStartDate, EndInvoiceDate: InvoiceEndDate, InvoiceAmount: InvoiceAmount, ServiceAmount: ServiceAmount, DropFODistance: Data.Selected.DropFODistance, ProductID: Data.Selected.ProductID }

        $.ajax({
            url: "/api/ReconcileData/GetProRateAmount",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
        })
        .done(function (data, textStatus, jqXHR) {
            var result = (parseFloat(data.data) - parseFloat(DeductionAmount)).toString();
            $("#txtTotalPrice").val(Common.Format.CommaSeparation(result));
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

//    ValidationSplitAmount: function () {
//        var result;
//    var l = Ladda.create(document.querySelector("#btSave"))
//    l.start();
    
//    var ListID = Data.SiteRow.toString();
//    var lengthid = ListID.split(',');

//    if (lengthid.length == 1)
//        ListID = ListID + ',0';

//    var params = {
//        trxReconcileID: ListID
//    };

//    $.ajax({
//        url: "/api/RTI/ValidationSplitAmount",
//        type: "POST",
//        dataType: "json",
//        contentType: "application/json",
//        data: JSON.stringify(params),
//        cache: false,
//        async: false,
//        beforeSend: function (xhr) {
//            l.start();
//        }
//    }).done(function (data, textStatus, jqXHR) {
//        result = data;
//        l.stop();
//    }).fail(function (jqXHR, textStatus, errorThrown) {
//        Common.Alert.Error(errorThrown)
//        l.stop();
//    })
//    return result;
//},
}

var Helper = {
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

        //var params = {
        //    strCompanyId: fsCompanyId,
        //    strOperator: fsCustomerID,
        //    strBAPS: fsBAPS,
        //    strPO: fsPO,
        //    strSONumber: fsSONumber
        //};

        var params = {
            CompanyId: fsCompanyId,
            CustomerId: fsCustomerId,
            strBAPSNumber: fsBAPSNumber,
            strSONumber: fsSONumber,
            strPONumber: fsPONumber,
            Year: fsYear,
            Quartal: fsQuartal,
            BapsType: fsBapsType
        }

        var AjaxData = [];
        $.ajax({
            url: "/api/RTI/GetListId",
            type: "POST",
            dataType: "json",
            async: false,
            contentType: "application/json",
            data: JSON.stringify(params),
            //data: params,
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
    numformat: function numformat(angka) {
        var numformat = $.fn.dataTable.render.number(',', '.', 2, '').display;
        return numformat(angka)
    },
    Validation: function () {

        fsStartDate = $("#tbStartInvoiceDate").val() == "" ? 0 : $("#tbStartInvoiceDate").val();
        fsEndDate = $("#tbEndInvoiceDate").val() == "" ? 0 : $("#tbEndInvoiceDate").val();

        fsStartDatePeriod = $("#tbStartDate").val() == null ? "" : $("#tbStartDate").val();
        fsEndDatePeriod = $("#tbEndDate").val() == null ? "" : $("#tbEndDate").val();

        var startDateComponents = Helper.ReverseDateToSQLFormat($("#tbStartInvoiceDate").val()).split("/");
        var endDateComponents = Helper.ReverseDateToSQLFormat($("#tbEndInvoiceDate").val()).split("/");
        var startDate = new Date(parseInt(startDateComponents[2]), parseInt(startDateComponents[0]) - 1, parseInt(startDateComponents[1]));
        var endDate = new Date(parseInt(endDateComponents[2]), parseInt(endDateComponents[0]) - 1, parseInt(endDateComponents[1]));

        var startDatePeriodComponents = Helper.ReverseDateToSQLFormat($("#tbStartDate").val()).split("/");
        var endDatePeriodComponents = Helper.ReverseDateToSQLFormat($("#tbEndDate").val()).split("/");
        var startDatePeriod = new Date(parseInt(startDateComponents[2]), parseInt(startDateComponents[0]) - 1, parseInt(startDateComponents[1]));
        var endDatePeriod = new Date(parseInt(endDateComponents[2]), parseInt(endDateComponents[0]) - 1, parseInt(endDateComponents[1]));

        var msg = '';

        if (fsStartDate == "" || fsEndDate == "") {
            msg = msg + "Data must be completed! <br/>";
        }
        else {

            //if (fsCounter > 0) {
            //    Form.CheckingPeriod();

            //    if (fsResult == false) {
            //        msg = msg + " Period Date must be greater than existing period ! <br/>";
            //    }
            //    else {
            //        if (endDate < startDate || startDate.toDateString() == endDate.toDateString()) {
            //            msg = msg + "End period must be greater than start period !  <br/>";
            //        }
            //    }

            //}
            //else {

                if (fsStartDate < fsStartDatePeriod) {
                    msg = msg + " Start Date must same or be greater than Period Start Date ! <br/>";
                }
                else if (endDate > endDatePeriod) {
                    msg = msg + " End Date cannot be greater than Period End Date ! <br/>";
                }
            //}


        }

        fsResult = false;

        return msg;
    },
    ReverseDateToSQLFormat: function (dateValue) {
        var dateComponents = dateValue.split("-");
        var dateValue = dateComponents[0];
        var monthValue = dateComponents[1];
        var yearValue = dateComponents[2];
        var allMonths = "JanFebMarAprMayJunJulAugSepOctNovDec";

        var monthNumberValue = allMonths.indexOf(monthValue) / 3 + 1;

        return monthNumberValue + "/" + dateValue + "/" + yearValue;
    }

}