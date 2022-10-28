var HeaderID;
var AppHeaderID;
var RowSelected = {};
var TempSonumbList = [];
var TempRequestHeaderList = [];
var TempRequestDetail = [];
var TempRequestReHold = [];
var dataType = "new";
var CreteType = "create";
var ActLabel = "";
var PrevActLabel = "";
var requestDetail = [];
var userLoginRole = "";
var userLoginID = "";
var PrevActivityID;
var PrevAppHeaderID;
var RequestTypeID;
var RequestType;
var ActivityID;
var RequestNumber;
var paramHeader = {};
var paramSoNumbList = {};
var prevStartEffectiveDate;
var isReHold;
var paramDraftList = {};
var SubmissionType = [];
var SoNumber = "";
var Session = "";
jQuery(document).ready(function () {
    userLoginRole = $("#userLoginRoleLabel").val();
    userLoginID = $("#userLogin").val();
    $("#pnlHeader").hide();
    $("#pnlDetail").hide();
    $("#pnlHeader").css('visibility', 'visible');
    $("#pnlHeader").fadeIn(2000);
    Buttons.Init();
    Control.SetParamHeader();
    Table.LoadHeader();
    BindData.BindingDatePicker();
    BindData.BindCategoryCase();
    BindData.BindAccrueType();
    BindData.BindDetailCase(null);
    BindData.BindCompany();
    BindData.BindCustomer();
    BindData.BindProduct();
    BindData.BindRegion();
    BindData.BindAccrueActivity();

    if (userLoginRole == 'DEPT_HEAD')
        $(".StatusSearch").show();
    else
        $(".StatusSearch").hide();
});


var Buttons = {
    Init: function () {

        if (userLoginRole == "DEPT_HEAD") {
            $("#btnAddRequest").show();
        } else {
            $("#btnAddRequest").hide();
        }

        if (userLoginRole == "APPR_DIV_ACC" || userLoginRole == "APPR_DIV_ASET") {
            $("#btnReceiveDoc").show();
        } else {
            $("#btnReceiveDoc").hide();
        }


        /*filter data header*/
        /*start*/
        $("#btSearchRequest").unbind().click(function () {
            Control.SetParamHeader();
            Table.LoadHeader();
        });
        $("#btResetRequest").unbind().click(function () {
            $("#slsActivityID").val('').trigger('change');
            $("#slsRequestTypeID").val('').trigger('change');
            $("#sRequestNumber").val('');
            $("#sInitiator").val('');
            $("#sActivityOwner").val('');
            $("#sCreatedDate").val('');
            $("#sStartEffectiveDate").val('');
            $("#sEndEffectiveDate").val('');
        });
        /*end*/
        /*==================================================================*/
        /*filter data detail*/
        /*start*/
        $("#btSearchRequestDetail").unbind().click(function () {
            Control.SetParamSonumbList();
            Table.DetailTable();
            Table.LoadSonumbList();
            //TempSonumbList = [];
            //TempRequestDetail = [];
        });
        $("#btResetSearchRequestDetail").unbind().click(function () {
            Control.ClearSearchDetail();
        });
        /*end*/
        /*==================================================================*/
        /*transaction detail*/
        /*start*/
        $("#btnTrxSaveDetail").unbind().click(function () {
            if (Form.ValidationTrxDetail()) {
                Form.UpdateTrxDetail();
                $(".tblRequestDetails").show();
            }
        });
        $("#btnTrxResetDetail").unbind().click(function () {
            Control.ClearFormDetail();
        });
        $("#btnTrxCancelDetail").unbind().click(function () {
            Control.ClearFormDetail();
        });
        /*end*/
        /*==================================================================*/
        $("#btnAddSonumb").unbind().click(function () {
            Control.ClearFormDetail();
            $(".fileUpload").hide();
            if (TempSonumbList.length >= 1) {
                if ($("#iRequestTypeID").val() != null && $("#iRequestTypeID").val() != "") {
                    if ($("#iRequestTypeID option:selected").html() == "STOP") {
                        $(".EffectiveDate").show();
                    } else {
                        $(".EffectiveDate").hide();
                    }

                    CreteType = "create";
                    $("#iCategoryCaseID").prop('disabled', false);
                    $("#iDetailCaseID").prop('disabled', false);
                    $("#mdlRequestDetail").modal('show');

                    $("#sRFIDate").css('visibility', 'hidden');
                } else {
                    Common.Alert.Warning("Please select type submission!");
                }

            } else {
                $("#sRFIDate").css('visibility', 'visible');
                Common.Alert.Warning("Please select sonumb!");
            }
        });
        $("#btnAddRequest").unbind().click(function () {
            ////Check Draft
            Form.CekDraft();
        });
        /*==================================================================*/
        /*transaction header*/
        /*start*/
        $("#btResetRequestDetail").unbind().click(function () {
            if (dataType != "") {
                $("#EndEffective").val('');
                $("#StartEffective").val('');
            }
            $("#iRemarksHeader").val('');
            $("#iStatusActID").val('').trigger('change');

        });
        $("#btCancelRequestDetail").unbind().click(function () {
            Control.ClearFormRequest();
        });
        $("#btSaveRequestDetail").unbind().click(function () {
            var date1 = new Date($("#StartEffective").val());
            var date2 = new Date($("#EndEffective").val());

            if (dataType == "new") {
                if (TempRequestDetail.length > 0) {

                    var submissionType = $("#iRequestTypeID option:selected").html();

                    if (submissionType == "") {
                        Common.Alert.Warning("Pelase select type submission!");
                    } else if ((submissionType == "HOLD") && ($("#StartEffective").val() == "" || $("#EndEffective").val() == "")) {
                        Common.Alert.Warning("Date must be fill!");
                    } else if (Control.DiffMonths(date1, date2) > 6 && submissionType == "HOLD") {
                        Common.Alert.Warning("Date lest then 6 Months");
                    }
                    else if ($("#iRemarksHeader").val() == "") {
                        Common.Alert.Warning("Pelase insert remarks!");
                    }
                    else if (date1 > date2) {
                        Common.Alert.Warning("End must be more than Start");
                    } else {
                        Form.Submit();
                    }
                } else {
                    Common.Alert.Warning("Please select sonumb!");
                }
            } else if (dataType == "rehold") {

                if ($("#StartEffective").val() == "" || $("#EndEffective").val() == "") {
                    Common.Alert.Warning("Date must be fill!");
                } else if ($("#iRemarksHeader").val() == "") {
                    Common.Alert.Warning("Pelase insert remarks!");
                } else if (Control.DiffMonths(date1, date2) > 6) {
                    Common.Alert.Warning("Date lest then 6 Months");
                } else if (date1 > date2) {
                    Common.Alert.Warning("End must be more than Start");
                } else if (date1 <= (new Date(prevStartEffectiveDate))) {
                    Common.Alert.Warning("Start Date must be more than End date on request number : " + RequestNumber);
                }
                else if (TempRequestReHold.length > 0) {
                    for (var i = 0; i < TempRequestReHold.length; i++) {
                        if ($("#remarks_" + TempRequestReHold[i].ID + "").val() == "")
                            return Common.Alert.Warning("Pelase insert remarks on SO Number : " + TempRequestReHold[i].SONumber + "");
                    }
                    Form.SubmitReHoldAccrue();
                } else {
                    Form.SubmitReHoldAccrue();
                }

            }
            else {
                if (userLoginRole !== "DEPT_HEAD" && userLoginRole != "RECEIVE_DHAS" && userLoginRole != "APPR_DIV_ASET" && $("#iStatusActID").val() == "") {
                    Common.Alert.Warning("Pelase select status!");
                }

                else if ($("#iRemarksHeader").val() == "") {
                    Common.Alert.Warning("Pelase insert remarks!");
                }

                else {
                    Form.Update();
                }

            }
        });
        $("#btSaveReqDetailDraft").unbind().click(function () {
            var date1 = new Date($("#StartEffective").val());
            var date2 = new Date($("#EndEffective").val());

            if (dataType == "new") {
                if (TempRequestDetail.length > 0) {

                    var submissionType = $("#iRequestTypeID option:selected").html();

                    if (submissionType == "") {
                        Common.Alert.Warning("Pelase select type submission!");
                        //} else if ((submissionType == "HOLD") && ($("#StartEffective").val() == "" || $("#EndEffective").val() == "")) {
                        //    Common.Alert.Warning("Date must be fill!");
                    } else if (Control.DiffMonths(date1, date2) > 6 && submissionType == "HOLD") {
                        Common.Alert.Warning("Date lest then 6 Months");
                    }
                        //else if ($("#iRemarksHeader").val() == "") {
                        //    Common.Alert.Warning("Pelase insert remarks!");
                        //}
                    else if (date1 > date2) {
                        Common.Alert.Warning("End must be more than Start");
                    } else {
                        Form.Draft();
                    }
                } else {
                    Common.Alert.Warning("Please select sonumb!");
                }
            } else if (dataType == "rehold") {

                if ($("#StartEffective").val() == "" || $("#EndEffective").val() == "") {
                    Common.Alert.Warning("Date must be fill!");
                } else if ($("#iRemarksHeader").val() == "") {
                    Common.Alert.Warning("Pelase insert remarks!");
                } else if (Control.DiffMonths(date1, date2) > 6) {
                    Common.Alert.Warning("Date lest then 6 Months");
                } else if (date1 > date2) {
                    Common.Alert.Warning("End must be more than Start");
                } else if (date1 <= (new Date(prevStartEffectiveDate))) {
                    Common.Alert.Warning("Start Date must be more than End date on request number : " + RequestNumber);
                }
                else if (TempRequestReHold.length > 0) {
                    for (var i = 0; i < TempRequestReHold.length; i++) {
                        if ($("#remarks_" + TempRequestReHold[i].ID + "").val() == "")
                            return Common.Alert.Warning("Pelase insert remarks on SO Number : " + TempRequestReHold[i].SONumber + "");
                    }
                    Form.SubmitReHoldAccrueDraft();
                } else {
                    Form.SubmitReHoldAccrueDraft();
                }

            }
            else {
                if (userLoginRole !== "DEPT_HEAD" && userLoginRole != "RECEIVE_DHAS" && userLoginRole != "APPR_DIV_ASET" && $("#iStatusActID").val() == "") {
                    Common.Alert.Warning("Pelase select status!");
                }
                else if ($("#iRemarksHeader").val() == "") {
                    Common.Alert.Warning("Pelase insert remarks!");
                }
                else {
                    Form.Update();
                }
            }
        });
        $("#btClearReqDetailDraft").unbind().click(function () {
            Form.ClearDraft();
            location.reload(true);
        });
        $("#btSaveEdit").unbind().click(function () {
            var date1 = new Date($("#StartEffective").val());
            var date2 = new Date($("#EndEffective").val());

            if (dataType == "new") {
                if (TempRequestDetail.length > 0) {

                    var submissionType = $("#iRequestTypeID option:selected").html();

                    if (submissionType == "") {
                        Common.Alert.Warning("Pelase select type submission!");
                    } else if ((submissionType == "HOLD") && ($("#StartEffective").val() == "" || $("#EndEffective").val() == "")) {
                        Common.Alert.Warning("Date must be fill!");
                    } else if (Control.DiffMonths(date1, date2) > 6 && submissionType == "HOLD") {
                        Common.Alert.Warning("Date lest then 6 Months");
                    }
                    else if ($("#iRemarksHeader").val() == "") {
                        Common.Alert.Warning("Pelase insert remarks!");
                    }
                    else if (date1 > date2) {
                        Common.Alert.Warning("End must be more than Start");
                    } else {
                        Form.SubmitEdit();
                    }
                } else {
                    Common.Alert.Warning("Please select sonumb!");
                }
            } else if (dataType == "rehold") {

                if ($("#StartEffective").val() == "" || $("#EndEffective").val() == "") {
                    Common.Alert.Warning("Date must be fill!");
                } else if ($("#iRemarksHeader").val() == "") {
                    Common.Alert.Warning("Pelase insert remarks!");
                } else if (Control.DiffMonths(date1, date2) > 6) {
                    Common.Alert.Warning("Date lest then 6 Months");
                } else if (date1 > date2) {
                    Common.Alert.Warning("End must be more than Start");
                } else if (date1 <= (new Date(prevStartEffectiveDate))) {
                    Common.Alert.Warning("Start Date must be more than End date on request number : " + RequestNumber);
                }
                else if (TempRequestReHold.length > 0) {
                    for (var i = 0; i < TempRequestReHold.length; i++) {
                        if ($("#remarks_" + TempRequestReHold[i].ID + "").val() == "")
                            return Common.Alert.Warning("Pelase insert remarks on SO Number : " + TempRequestReHold[i].SONumber + "");
                    }
                    Form.SubmitReHoldAccrue();
                } else {
                    Form.SubmitReHoldAccrue();
                }

            }
            else {
                if (userLoginRole !== "DEPT_HEAD" && userLoginRole != "RECEIVE_DHAS" && userLoginRole != "APPR_DIV_ASET" && $("#iStatusActID").val() == "") {
                    Common.Alert.Warning("Pelase select status!");
                }

                else if ($("#iRemarksHeader").val() == "") {
                    Common.Alert.Warning("Pelase insert remarks!");
                }

                else {
                    Form.Update();
                }
            }
        });

        $(".btnSearchHeader").unbind().click(function () {
            Control.SetParamHeader();
            Table.LoadHeader();
        });
        $(".btnSearchHeaderReset").unbind().click(function () {
            $("#sRequestNumber").val('');
            $("#slsActivityID").val('').trigger('change');
            $("#slsRequestTypeID").val('').trigger('change');
            $("#sInitiator").val('');
            $("#sActivityOwner").val('');
            $("#sCreatedDate").val('');
            $("#sStartEffectiveDate").val('');
            $("#sEndEffectiveDate").val('');


        });
        $(".btnSearchSoNumbReset").unbind().click(function () {
            $("#sSONumber").val('');
            $("#slsCompanyID").val('').trigger('change');
            $("#slsCustomerID").val('').trigger('change');
            $("#slsRegionID").val('').trigger('change');
            $("#slsProductID").val('').trigger('change');
            $("#sSiteName").val('');
            $("#sSiteID").val('');
            $("#sRFIDate").val('');

        });
        $(".tblSonumbList").on('click', '.btnSearchSoNumb', function () {
        //$(".tblSonumbList table").on('click', '.btnSearchSoNumb', function () {
            //$(".btnSearchSoNumb").unbind().click(function () {

            Control.SetParamSonumbList();
            Table.LoadSonumbList();
        });

        /*end*/
        /*==================================================================*/
        /*form*/
        /*start*/
        $("#tblSonumbList").on('change', 'tbody tr .checkboxes', function () {
            var id = $(this).parent().attr('id');
            var table = $("#tblSonumbList").DataTable();

            var DataRows = {};
            var Row = table.row($(this).parents('tr')).data();

            if (this.checked) {
                DataRows.ViewIdx = Row.ViewIdx;
                DataRows.SONumber = Row.SONumber;
                DataRows.RFIDone = Row.RFIDone;
                DataRows.SiteID = Row.SiteID;
                DataRows.SiteName = Row.SiteName;
                DataRows.Customer = Row.CustomerID;
                DataRows.Company = Row.CompanyID;
                DataRows.Product = Row.Product;
                DataRows.IsHold = 1;
                RowSelected = DataRows;
                TempSonumbList.push(RowSelected);
            } else {
                var index2 = TempSonumbList.findIndex(function (data) {
                    return data.ViewIdx == Row.ViewIdx;
                });

                Control.DeleteDateDetailTemp(Row.ViewIdx);
                TempSonumbList.splice(index2, 1);
            }
        });
        $('#iFileUpload').on('change', function () {
            var input = document.getElementById('iFileUpload');
            var infoArea = document.getElementById('file-upload-filename');
            var fileName = input.files[0].name;
            infoArea.textContent = fileName;
            $("#spanFile").text("");
        });
        $('#iFileUploadDoc').on('change', function () {
            var input = document.getElementById('iFileUploadDoc');
            var infoArea = document.getElementById('file-upload-filenameDoc');
            var fileName = input.files[0].name;
            infoArea.textContent = fileName;
            $("#spanFileDoc").text("");
        });
        $("#iCategoryCaseID").unbind().change(function () {
            var id = $("#iCategoryCaseID").val();
            if (id != null && id != "") {
                BindData.BindDetailCase(id);
                $("#spanCategory").text('');
            }

        });
        $("#iDetailCaseID").unbind().change(function () {
            $("#spanCaseDetail").text('');
        });
        $("#iRemarks").on('focus', function () {
            $("#spanRemarks").text('');
        });
        $("#iRequestTypeID").unbind().change(function () {
            var submissionType = $("#iRequestTypeID option:selected").html()
            if (submissionType == "STOP") {
                $("#EndEffective").prop("disabled", true);
                $("#EndEffective").val("");
            } else {
                $("#EndEffective").prop("disabled", false);
            }
        });
        /*end*/

        /*button for upload document*/
        $("#btnUploadDoc").unbind().click(function () {

            Form.UploadDocRequest();
        });

        $("#btnCancelUploadDoc").unbind().click(function () {
            document.getElementById("iFileUploadDoc").value = "";
            document.getElementById("file-upload-filenameDoc").innerText = "";
        });
        /*button for recieve document*/
        $("#tblRequestHeader").on('change', 'tbody tr .checkboxes', function () {
            var id = $(this).parent().attr('id');
            var table = $("#tblRequestHeader").DataTable();

            var DataRows = {};
            var Row = table.row($(this).parents('tr')).data();
            RowSelected = {};
            if (this.checked) {
                DataRows.HeaderID = Row.ID;
                DataRows.AppHeaderID = Row.AppHeaderID;
                DataRows.NextFlag = Row.AccrueType;
                RowSelected = DataRows;
                TempRequestHeaderList.push(RowSelected);
                RowSelected = {};
            } else {
                var index = TempRequestHeaderList.findIndex(function (data) {
                    return data.ID == Row.ID;
                });
                TempRequestHeaderList.splice(index, 1);
            }
        });
        $("#btnReceiveDoc").unbind().click(function () {
            if (TempRequestHeaderList.length > 0) {
                var accrueType = TempRequestHeaderList[0].AccrueType;
                for (var i = 1; i < TempRequestHeaderList.length; i++) {
                    if (TempRequestHeaderList[i].AccrueType != accrueType) {
                        return Common.Alert.Warning("Submission type not same");
                    }
                }
                Form.ReceiveDoc();
            }
            else {
                return Common.Alert.Warning("Please select request number!");
            }

        });

        /*button for recieve document*/
        $("#tblRequestDetail").on('change', 'tbody tr .checkboxes', function () {
            var id = $(this).parent().attr('id');
            var table = $("#tblRequestDetail").DataTable();
            var DataRows = {};
            var Row = table.row($(this).parents('tr')).data();
            if (this.checked) {
                DataRows.ID = Row.ID;
                DataRows.SONumber = Row.SONumber;
                DataRows.Remarks = "";
                TempRequestReHold.push(DataRows);
                DataRows = {};
            } else {
                var index = TempRequestReHold.findIndex(function (data) {
                    return data.ID == Row.ID;
                });
                TempRequestReHold.splice(index, 1);
            }

            if (TempRequestReHold.length > 0) {
                $(".reHoldAccruePnl").show();
                Table.ReHoldAcrrueTable();
            } else {
                $(".reHoldAccruePnl").hide();
            }

        });

        $("#iRequestTypeID").unbind().change(function () {
            if ($("#iRequestTypeID option:selected").html() == "STOP") {
                $(".StartEffectiveDate").hide();
            } else {
                $(".StartEffectiveDate").show();
            }
        });
    }
}

var Control = {
    ClearFormDetail: function () {
        $("#iCategoryCaseID").val('').trigger('change');
        $("#iDetailCaseID").val('').trigger('change');
        $("#iRemarks").val('');
        $("#iRemarksHeader").val('');
        $("#iEffectiveDate").val('');
        document.getElementById('iFileUpload').value = "";
        document.getElementById("file-upload-filename").innerText = "";
        $("#spanCategory").text("");
        $("#spanCaseDetail").text("");
        $("#spanRemarks").text("");
        $("#spanFile").text("");
        $("#spanEffectiveDate").text("");
        $("#sRFIDate").css('visibility', 'visible');
    },
    ClearFormRequest: function () {
        $("#pnlDetail").hide();
        $("#pnlHeader").fadeIn(2000);
        $("#EndEffective").val('');
        $("#iRemarks").val('');
        $("#StartEffective").val('');
        $("#iRemarksHeader").val('');
        $("#iRequestTypeID").val('').trigger('change');
        RowSelected = {};
        TempSonumbList = [];
        TempRequestDetail = [];
        TempRequestReHold = [];
        dataType = "";
        ActLabel = "";
        location.reload(true);
        Control.ClearSearchDetail();
        Table.LoadHeader();
    },
    ClearSearchDetail: function () {
        $("#slsCustomerID").val('').trigger('change');
        $("#slsCompanyID").val('').trigger('change');
        $("#slsProductID").val('').trigger('change');
        $("#slsRegionID").val('').trigger('change');
        $("#sSONumber").val('');
        $("#sSiteID").val('');
        $("#sSiteName").val('');
        $("#sRFIDate").val('');
    },
    SelectData: function () {
        if (TempRequestDetail.length > 0) {
            $.each(TempRequestDetail, function (i, item) {
                $(".cb_" + item.ViewIdx).prop('checked', true);
            })
        }
        if (TempSonumbList.length > 0) {
            $.each(TempSonumbList, function (i, item) {
                $(".cb_" + item.ViewIdx).prop('checked', true);
            })
        }
    },
    DeleteRequestDetail: function (ViewIdx) {
        var id = ".cb_" + ViewIdx;
        $(id).prop('checked', false);
        Control.DeleteDateDetailTemp(ViewIdx)

        //var index = TempRequestDetail.findIndex(function (data) {
        //    return data.SONumber == soNumber
        //});
        //TempRequestDetail.splice(index, 1);
        //if (TempRequestDetail.length == 0) {
        //    $("#iRequestTypeID").prop("disabled", false);
        //}
        //Table.LoadDetail();
    },
    DeleteRequestDetailDraft: function (ViewIdx) {
        var id = ".cb_" + ViewIdx;
        $(id).prop('checked', false);
        Control.DeleteDateDetailTempDraft(ViewIdx)

    },
    DeleteDateDetailTemp: function (ViewIdx) {
        var index = TempRequestDetail.findIndex(function (data) {
            return data.ViewIdx == ViewIdx
        });
        TempRequestDetail.splice(index, 1);
        if (TempRequestDetail.length == 0) {
            $("#iRequestTypeID").prop("disabled", false);
        }
        Table.LoadDetail();
    },
    DeleteDateDetailTempDraft: function (ViewIdx) {
        var index = TempRequestDetail.findIndex(function (data) {
            return data.ViewIdx == ViewIdx
        });
        TempRequestDetail.splice(index, 1);
        if (TempRequestDetail.length == 0) {
            $("#iRequestTypeID").prop("disabled", false);
        }
        Table.LoadDetail();
    },
    DiffMonths: function (dt2, dt1) {
        var diff = (dt2.getTime() - dt1.getTime()) / 1000;
        diff /= (60 * 60 * 24 * 7 * 4);
        return Math.abs(Math.round(diff));
    },
    SetParamHeader: function () {

        var RequestNumber = $("#sRequestNumber").val();
        var ActivityID = $("#slsActivityID").val();
        var RequestTypeID = $("#slsRequestTypeID").val();
        var Initiator = userLoginID;
        var InitiatorName = $("#sInitiator").val();
        var UserRole = userLoginRole;
        var ActivityOwner = userLoginID;
        var ActivityOwnerName = $("#sActivityOwner").val();
        var CreatedDate = $("#sCreatedDate").val() == "" ? null : $("#sCreatedDate").val();
        var StartEffectiveDate = $("#sStartEffectiveDate").val() == "" ? null : $("#sStartEffectiveDate").val();
        var EndEffectiveDate = $("#sEndEffectiveDate").val() == "" ? null : $("#sEndEffectiveDate").val();

        paramHeader = {
            RequestNumber: RequestNumber,
            ActivityID: ActivityID,
            RequestTypeID: RequestTypeID,
            InitiatorName: InitiatorName,
            ActivityOwnerName: ActivityOwnerName,
            CreatedDate: CreatedDate,
            StartEffectiveDate: StartEffectiveDate,
            EndEffectiveDate: EndEffectiveDate,
            UserRole: UserRole,
            Initiator: Initiator,
            ActivityOwner: ActivityOwner,
        };
    },
    SetParamSonumbList: function () {
        var SONumber = $("#sSONumber").val();
        var SiteName = $("#sSiteName").val();
        var CompanyID = $("#slsCompanyID").val();
        var SiteID = $("#sSiteID").val();
        var CustomerID = $("#slsCustomerID").val()
        var RFIDone = $("#sRFIDate").val();
        var RegionID = $("#slsRegionID").val();
        var ProductID = $("#slsProductID").val();
        paramSoNumbList = { SONumber: SONumber, SiteID: SiteID, SiteName: SiteName, CustomerID: CustomerID, CompanyID: CompanyID, ProductID: ProductID, RegionID: RegionID, RFIDone: RFIDone };
    },
    ClearFormUploadDoc: function () {
        $("#spanFileDoc").text("");
        $("#uploadHeaderID").val('');
        $("#uploadAppHeaderID").val('');
        document.getElementById('iFileUploadDoc').value = "";
        document.getElementById("file-upload-filenameDoc").innerText = "";
    }
}
/* Bind data master */
var BindData = {
    BindCompany: function () {
        var selectId = "#slsCompanyID";
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                $(selectId).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(selectId).append("<option value='" + $.trim(item.CompanyId) + "'>" + item.Company + "</option>");
                    })
                }
                $(selectId).select2({ placeholder: "Select Category", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindCustomer: function () {
        var selectId = "#slsCustomerID";
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                $(selectId).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(selectId).append("<option value='" + $.trim(item.OperatorId) + "'>" + item.Operator + "</option>");
                    })
                }
                $(selectId).select2({ placeholder: "Select Category", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindProduct: function () {
        var selectId = "#slsProductID";
        $.ajax({
            url: "/api/MstDataSource/TenantType",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                $(selectId).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(selectId).append("<option value='" + $.trim(item.Value) + "'>" + item.Text + "</option>");
                    })
                }
                $(selectId).select2({ placeholder: "Select Tenant", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindRegion: function () {
        var selectId = "#slsRegionID";
        $.ajax({
            url: "/api/MstDataSource/Region",
            type: "GET",
            async: false
        })
            .done(function (data, textStatus, jqXHR) {
                $(selectId).html("<option></option>")
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(selectId).append("<option value='" + $.trim(item.RegionID) + "'>" + item.RegionName + "</option>");
                    })
                }
                $(selectId).select2({ placeholder: "Select Category", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingDatePicker: function () {
        var d = new Date();
        var dateNow = new Date(d.getDate(), d.getMonth(), d.getFullYear());
        $(".datepicker").datepicker({
            format: "dd M yyyy",
            autoclose: true
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
                        $(selectId).append("<option value='" + $.trim(item.ID) + "'>" + item.AccrueType + "</option>");
                        $(selectId2).append("<option value='" + $.trim(item.ID) + "'>" + item.AccrueType + "</option>");
                    })
                }
                $(selectId).select2({ placeholder: "Select Type Submission", width: null });
                $(selectId2).select2({ placeholder: "Select Type Submission", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindAccrueActivity: function () {
        var selectId = "#slsActivityID";
        $.ajax({
            url: "/api/StopAccrue/accrueActivity",
            type: "GET",
            async: false,

        })
            .done(function (data, textStatus, jqXHR) {
                $(selectId).html("<option></option>");
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(selectId).append("<option value='" + $.trim(item.ActivityID) + "'>" + item.Activity + "</option>");
                    })
                }
                $(selectId).select2({ placeholder: "Select Activity", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindNextFlag: function (activityLabel) {
        var selectId = "#iStatusActID";
        $.ajax({
            url: "/api/StopAccrue/nextFlag",
            type: "GET",
            async: false,
            data: { RoleLabel: userLoginRole, ActivityLabel: activityLabel },
        })
            .done(function (data, textStatus, jqXHR) {
                $(selectId).html("<option></option>");
                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        $(selectId).append("<option value='" + $.trim(item.StatusApproval) + "'>" + item.StatusApproval + "</option>");
                    })
                }
                $(selectId).select2({ placeholder: "Select Status", width: null });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },

}

/* Form  */
var Form = {
    /* Submit begin */
    Submit: function () {
        var postData = [];
        var arrayData = {};
        $.each(TempRequestDetail, function (i, item) {
            arrayData = {};
            if ($("#iRequestTypeID option:selected").html() == "STOP") {
                arrayData.EffectiveDate = item.EffectiveDate;
            } else {
                arrayData.EffectiveDate = $("#StartEffective").val();
            }
            arrayData.DetailID = item.ID;
            arrayData.CategoryCaseID = item.CaseCategoryID;
            arrayData.DetailCaseID = item.CaseDetailID;
            arrayData.SONumber = item.SONumber;
            arrayData.Remarks = item.Remarks;
            arrayData.FileName = item.FileName;
            arrayData.EffectiveDate = item.EffectiveDate;
            postData.push(arrayData);
        });
        var params = {
            StopAccrueRequest: postData, 
            RequestTypeID: $("#iRequestTypeID").val(),
            StartEffectiveDate: $("#StartEffective").val(),
            EnndEffectiveDate: $("#iRequestTypeID option:selected").html() == "STOP" ? $("#StartEffective").val() : $("#EndEffective").val(),
            HeaderID: HeaderID,
            Remarks: $("#iRemarksHeader").val(), 
            RequestType: $("#iRequestTypeID option:selected").html()

        };
        var l = Ladda.create(document.querySelector("#btSaveRequestDetail"));
        $.ajax({ 
            url: "/api/StopAccrue/submitRequest",  
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            },
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Data Success Saved!");
                Table.LoadHeader();
                Control.ClearFormRequest();
            } else {
                Common.Alert.Warning(data.ErrorMessage);
            }
            l.stop();

        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        });

    },
    /* Submit edit */
    SubmitEdit: function ()
    {
        var postData = [];
        var arrayData = {};
        $.each(TempRequestDetail, function (i, item) {
            arrayData = {};
            if ($("#iRequestTypeID option:selected").html() == "STOP") {
                arrayData.EffectiveDate = item.EffectiveDate;
            } else {
                arrayData.EffectiveDate = $("#StartEffective").val();
            }
            arrayData.RequestNumber = RequestNumber
            arrayData.DetailID = item.ID;
            arrayData.CategoryCaseID = item.CaseCategoryID;
            arrayData.DetailCaseID = item.CaseDetailID;
            arrayData.SONumber = item.SONumber;
            arrayData.Remarks = item.Remarks;
            arrayData.FileName = item.FileName;
            arrayData.EffectiveDate = item.EffectiveDate;
            postData.push(arrayData);
        });
        var params = {
            StopAccrueRequest: postData,
            RequestTypeID: $("#iRequestTypeID").val(),
            StartEffectiveDate: $("#StartEffective").val(),
            EnndEffectiveDate: $("#iRequestTypeID option:selected").html() == "STOP" ? $("#StartEffective").val() : $("#EndEffective").val(),
            HeaderID: HeaderID,
            RequestNumber : RequestNumber,
            Remarks: $("#iRemarksHeader").val(),
            RequestType: $("#iRequestTypeID option:selected").html()
        };
        var l = Ladda.create(document.querySelector("#btSaveEdit"));
        $.ajax({
            url: "/api/StopAccrue/submitEdit",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            },
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Data Success Saved!");
                Table.LoadHeader();
                Control.ClearFormRequest();
            } else {
                Common.Alert.Warning(data.ErrorMessage);
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        });
    },
    /*Save Draft */
    Draft: function () {
        var postData = [];
        var arrayData = {};
        $.each(TempRequestDetail, function (i, item) {
            arrayData = {};
            if ($("#iRequestTypeID option:selected").html() == "STOP") {
                arrayData.EffectiveDate = item.EffectiveDate;
            } else {
                arrayData.EffectiveDate = $("#StartEffective").val();
            }
            arrayData.DetailID = item.ID;
            arrayData.CategoryCaseID = item.CaseCategoryID;
            arrayData.DetailCaseID = item.CaseDetailID;
            arrayData.SONumber = item.SONumber;
            arrayData.Remarks = item.Remarks;
            arrayData.FileName = item.FileName;
            arrayData.EffectiveDate = item.EffectiveDate;
            postData.push(arrayData);
        });
        var params = {
            StopAccrueRequest: postData,
            RequestTypeID: $("#iRequestTypeID").val(),
            StartEffectiveDate: $("#StartEffective").val(),
            EnndEffectiveDate: $("#iRequestTypeID option:selected").html() == "STOP" ? $("#StartEffective").val() : $("#EndEffective").val(),
            HeaderID: HeaderID,
            Remarks: $("#iRemarksHeader").val(),
            RequestType: $("#iRequestTypeID option:selected").html()

        };
        var d = Ladda.create(document.querySelector("#btSaveReqDetailDraft"));
        $.ajax({
            url: "/api/StopAccrue/submitRequestDraft",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                d.start();
            },
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Data Success Saved!");
                //Table.LoadHeader();
                //Control.ClearFormRequest();
            } else {
                Common.Alert.Warning(data.ErrorMessage);
            }
            d.stop();

        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            d.stop();
        });
    },
    /*Clear Draft */
        ClearDraft: function () {
        var postData = [];
        var arrayData = {};
        var params = {
            StopAccrueRequest: postData,
            RequestTypeID: $("#iRequestTypeID").val(),
            StartEffectiveDate: $("#StartEffective").val(),
            EnndEffectiveDate: $("#iRequestTypeID option:selected").html() == "STOP" ? $("#StartEffective").val() : $("#EndEffective").val(),
            HeaderID: HeaderID,
            Remarks: $("#iRemarksHeader").val(),
            RequestType: $("#iRequestTypeID option:selected").html()
        };
        var d = Ladda.create(document.querySelector("#btClearReqDetailDraft"));
        $.ajax({
            url: "/api/StopAccrue/clearDraft",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                d.start();
            },
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Data Cleared!");
            } else {
                Common.Alert.Warning(data.ErrorMessage);
            }
            d.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            d.stop();
        });
    },
    /*Update worklow */
    Update: function () {

        var l = Ladda.create(document.querySelector("#btSaveRequestDetail"));
        l.start();
        var params = {
            HeaderID: HeaderID,
            Remarks: $("#iRemarksHeader").val(),
            NextFlag: $("#iStatusActID").val(),
            AppHeaderID: AppHeaderID,
            RequestType: RequestType
        };

        $.ajax({
            url: "/api/StopAccrue/updateRequest",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Data Success Saved!");
                Table.LoadHeader();
                Control.ClearFormRequest();
            } else {
                Common.Alert.Warning(data.ErrorMessage);
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        });

    },
    /* Update data detail transaction */
    UpdateTrxDetail: function () {
        var l = Ladda.create(document.querySelector("#btnTrxSaveDetail"));
        l.start();
        var fileInput = document.getElementById("iFileUpload");
        var formData = new FormData();
        formData.append('File', fileInput.files[0]);

        var fileName = fileInput.files[0].name;
        fileName = fileName.replace('&', 'dan');
        var fileExt = fileName.split('.').pop().toUpperCase();
        var fileSize = fileInput.files[0];
        if (fileExt != "PDF" && fileExt != "ZIP" && fileExt != "RAR") {
            $("#spanFile").text("Please upload an PDF or ZIP or RAR File.!");
            l.stop();

        } else if ((fileSize.size / 1024) > 2048) {
            $("#spanFile").text("Upload File Can`t bigger then 2048 bytes (2mb)..! ");
            l.stop();
        } else if (fileName.length > 100) {
            $("#spanFile").text("File name to long, max 100 char ");
            l.stop();
        }
        else {
            $.ajax({
                url: "/StopAccrue/UploadFile",
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response == "") {
                        fileInput = document.getElementById("iFileUpload");
                        if (CreteType == "create") {
                            $("#iRequestTypeID").prop("disabled", true);
                            $.each(TempSonumbList, function (i, item) {
                                RowSelected = {};
                                RowSelected.ID = 0;
                                RowSelected.SONumber = item.SONumber;
                                RowSelected.RFIDone = item.RFIDone;
                                RowSelected.SiteID = item.SiteID;
                                RowSelected.SiteName = item.SiteName;
                                RowSelected.Customer = item.Customer;
                                RowSelected.Company = item.Company;
                                RowSelected.Product = item.Product;
                                RowSelected.CaseCategoryID = $("#iCategoryCaseID").val();
                                RowSelected.CaseDetailID = $("#iDetailCaseID").val();
                                RowSelected.CategoryCase = $("#iCategoryCaseID option:selected").html()
                                RowSelected.DetailCase = $("#iDetailCaseID option:selected").html()
                                RowSelected.Remarks = $("#iRemarks").val();
                                RowSelected.EffectiveDate = $("#iEffectiveDate").val();
                                RowSelected.FileName = fileName;
                                RowSelected.ViewIdx = item.ViewIdx;
                                RowSelected.IsHold = item.IsHold;
                                TempRequestDetail.push(RowSelected);
                            });
                            TempSonumbList = [];
                        } else if (CreteType == "edit") {
                            if ($("#iTrxDetail").val() == "0") {
                                for (var i = 0; i < TempRequestDetail.length; i++) {
                                    if ($("#iSONumber").val() == TempRequestDetail[i].SONumber) {
                                        var fileInput = document.getElementById("iFileUpload");
                                        TempRequestDetail[i].CaseCategoryID = $("#iCategoryCaseID").val();
                                        TempRequestDetail[i].CaseDetailID = $("#iDetailCaseID").val();
                                        TempRequestDetail[i].CategoryCase = $("#iCategoryCaseID option:selected").html();
                                        TempRequestDetail[i].DetailCase = $("#iDetailCaseID option:selected").html();
                                        TempRequestDetail[i].Remarks = $("#iRemarks").val();
                                        TempRequestDetail[i].FileName = fileName;
                                        TempRequestDetail[i].EffectiveDate = $("#iEffectiveDate").val();
                                        break;
                                    }
                                }
                            }
                        } else if (CreteType == "revise") {
                            Form.ReviseDetail();
                        }
                        $("#mdlRequestDetail").modal('hide');
                        Table.LoadDetail();
                        Control.ClearFormDetail();
                    } else {
                        $("#spanFile").text(response);
                    }
                    l.stop();
                },
                error: function (error) { l.stop(); }
            });
        }

    },
    /*Validation before submit transaction detail */
    ValidationTrxDetail: function () {
        var result = true;
        if (($("#iEffectiveDate").val() == "" || $("#iEffectiveDate").val() == null) && $("#iRequestTypeID option:selected").html() == "STOP") {
            $("#spanEffectiveDate").text("Please effective date");
            result = false;
        }
        if ($("#iCategoryCaseID").val() == "" || $("#iCategoryCaseID").val() == null) {
            $("#spanCategory").text("Please select category");
            result = false;
        }
        if ($("#iDetailCaseID").val() == "" || $("#iDetailCaseID").val() == null) {
            $("#spanCaseDetail").text("Please select Case");
            result = false;
        }
        if ($("#iRemarks").val() == "" || $("#iRemarks").val() == null) {
            $("#spanRemarks").text("Please input remarks");
            result = false;
        }
        var infoArea = document.getElementById('file-upload-filename');
        if (infoArea.textContent == "" || infoArea.textContent == null) {
            $("#spanFile").text("Please select file");
            result = false;
        }
        return result;
    },
    /* for submit revise */
    ReviseDetail: function () {
        var fileInput = document.getElementById("iFileUpload");
        var DetailID = $("#iTrxDetail").val();
        var Remarks = $("#iRemarks").val();
        var FileName = fileInput.files[0].name;

        var arrayData = {};
        arrayData.DetailID = DetailID;
        arrayData.Remarks = Remarks;
        arrayData.FileName = FileName.replace("&", "dan");

        var params = { StopAccrueRequestDetail: arrayData };
        $.ajax({
            url: "/api/StopAccrue/reviseDetail",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Data Success Saved!");
                Table.LoadDetail();
                Control.ClearFormDetail();
            } else {
                Common.Alert.Warning(data.ErrorMessage);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    /*Upload document after print document */
    UploadDocRequest: function () {
        var l = Ladda.create(document.querySelector("#btnUploadDoc"));
        l.start();
        var infoArea = document.getElementById('file-upload-filenameDoc');

        if (infoArea.textContent == "" || infoArea.textContent == null) {
            Common.Alert.Warning("Please select file");
            l.stop();
        } else {

            var formData = new FormData(); //FormData object  
            var fileInput = document.getElementById("iFileUploadDoc");
            if (fileInput.files[0] != undefined && fileInput.files[0] != null) {
                var fileName = fileInput.files[0].name;
                var extension = fileName.split('.').pop().toUpperCase();
                if (extension != "PDF") {
                    Common.Alert.Warning("Please upload an pdf File.");
                    l.stop();
                }
                else {

                    formData.append("File", fileInput.files[0]);
                    formData.append('HeaderID', $("#uploadHeaderID").val());
                    formData.append('AppHeaderID', $("#uploadAppHeaderID").val());


                    $.ajax({
                        url: '/api/StopAccrue/UploadDocRequest',
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
                        $("#mdlUploadDoc").modal('hide');
                        if (data != "") {
                            Common.Alert.Warning(data);
                        } else {
                            document.getElementById("iFileUploadDoc").value = "";
                            document.getElementById("file-upload-filenameDoc").innerText = "";
                            Common.Alert.Success("Upload data success!");
                            Table.LoadHeader();
                        }
                        l.stop();
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        Common.Alert.Error(errorThrown)
                        l.stop();
                    });
                }
            } else {
                Common.Alert.Warning("Please upload an pdf File.");
                l.stop();

            }
        }

    },
    /* for update workflow after receive document */
    ReceiveDoc: function () {

        // var params = { TempRequestHeaderList};
        var l = Ladda.create(document.querySelector("#btnReceiveDoc"));
        $.ajax({
            url: "/api/StopAccrue/receiveDoc",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(TempRequestHeaderList),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            },
        }).done(function (data, textStatus, jqXHR) {
            if (data == "") {
                Common.Alert.Success("Data Success Saved!");
                Table.LoadHeader();
            } else {
                Common.Alert.Warning("Receive data failed!");
            }
            l.stop();

        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        });
    },

    SubmitReHoldAccrue: function () {
        var dataReholdAccrue = [];
        for (var i = 0; i < TempRequestReHold.length; i++) {
            DataRows = {};
            DataRows.SONumber = TempRequestReHold[i].SONumber;
            DataRows.Remarks = $("#remarks_" + TempRequestReHold[i].ID + "").val();
            dataReholdAccrue.push(DataRows);
        }
        var params = {
            stopAccrueDetail: requestDetail,
            reHoldAccrue: dataReholdAccrue,
            RequestTypeID: RequestTypeID,
            Remarks: $("#iRemarksHeader").val(),
            RequestType: RequestType,
            PrevAppHeaderID: AppHeaderID,
            RequestNumber: RequestNumber,
            StartEffectiveDate: $("#StartEffective").val(),
            EndEffectiveDate: $("#EndEffective").val()
        };
        var l = Ladda.create(document.querySelector("#btSaveRequestDetail"));
        $.ajax({
            url: "/api/StopAccrue/submitReHoldRequest",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            },
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Data Success Saved!");
                Table.LoadHeader();
                Control.ClearFormRequest();
            } else {
                Common.Alert.Warning(data.ErrorMessage);
            }
            l.stop();

        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        });
    },

    //Form Check Draft
    CekDraft: function (param) {
        $(".reHoldAccruePnl").hide();
        var id = "#tblRequestDetail";
        var Initiator = userLoginID;
        var params = { Initiator: Initiator };
        var l = Ladda.create(document.querySelector("#btnAddRequest"));
        l.start();
        $.ajax({
            url: "/api/StopAccrue/CheckDraft",
            type: "GET",
            dataType: "json",
            async: false,
            data: params,
        }).done(function (data) {
            if (data.length > 0) {
                dataType = "new";
                HeaderID = "";
                requestDetail = [];
                $(".timeline").html('');
                $("#iRemarksHeader").val('');
                $("#iHeaderID").val('');
                $("#iStatusActID").val('').trigger('change');
                $("#fileUploaded").html("");
                $("#pnlHeader").hide();
                $("#spanFormNumber").text("");
                $(".historyActivity").hide();
                $(".approvalFrom").hide();
                $(".reHoldAccruePnl").hide();
                $(".tblRequestDetails").show();
                $("#btSaveRequestDetail").show();
                $("#btSaveEdit").hide();
                $("#btResetRequestDetail").show();
                $("#btnTrxResetDetail").show();
                $("#btnTrxSaveDetail").show();
                $(".approvalRemarks").show();
                $(".tblSonumbList").show();
                $(".filterDetail").show();
                $(".uploadFile").show();
                $("#EndEffective").show();
                $("#iRequestTypeID").prop('disabled', false);
                $("#StartEffective").prop('disabled', false);
                $("#EndEffective").prop('disabled', false);
                $("#iRemarksHeader").prop('disabled', false);
                $("#iRemarks").prop('disabled', false);
                $("#pnlDetail").fadeIn(2000);
                $("#pnlDetail").css('visibility', 'visible');
                Table.LoadSonumbList();

                var tblReqDetail = $("#tblRequestDetail").DataTable({
                    "proccessing": true,
                    "serverSide": false,
                    "bSortCellsTop": true,
                    "language": {
                        "emptyTable": "No data available in table",
                    },

                    //"serverside": false,
                    //"filter": false,
                    //"destroy": true,
                    //"async": false,
                    "data": data,
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
                        { data: "RowIndex" },//1
                        {
                            orderable: false,
                            mRender: function (a, b, c) {
                                var strReturn = ""
                                if (ActLabel != "")
                                    strReturn += "<i class='fa fa-eye btn btn-xs yellow editRow' ></i>";
                                else if (ActLabel == "")
                                    strReturn += "<i class='fa fa-trash btn btn-xs red deleteRow'></i>&nbsp;&nbsp;&nbsp;<i class='fa fa-pencil btn btn-xs yellow editRow'></i>";
                                return strReturn;
                            }
                        },//2
                        { data: "SONumber" },//3
                        {
                            mRender: function (a, b, c) {
                                if (c.IsHold) {
                                    if (dataType == "rehold")
                                        return "&nbsp;&nbsp;<label class='mt-checkbox mt-checkbox-single mt-checkbox-outline  lb_" + c.ID + "' disabled  style='visibility:hidden'><input type='checkbox' class='checkboxes cb_" + c.ID + "'  style='visibility:hidden'/><span></span></label>";
                                    else
                                        return "<i class='fa fa-remove'  style='visibility:hidden'></i>";
                                }
                                else {
                                    return "<i class='fa fa-check'  style='visibility:hidden'></i>";
                                }
                            },
                        },//4
                        { data: "SiteID" },//5
                        { data: "SiteName" },//6
                        { data: "Company" },//7
                        { data: "Customer" },//8
                        { data: "Product" },//9
                        {
                            data: "RFIDone", render: function (data) {
                                return Common.Format.ConvertJSONDateTime(data);
                            }
                        },//10
                        {
                            data: "CategoryCase", render: function (data) {
                                if (data == '1')
                                {return 'INTERNAL' }
                                else
                                {return 'EXTERNAL' }
                            }
                        
                        },//11
                        { data: "DetailCase" },//12
                        {
                            data: "EffectiveDate", render: function (data) {
                                return Common.Format.ConvertJSONDateTime(data);
                            }
                        },//13
                        {
                            data: "RevenueAmount", render: function (data) {
                                if (data == null)
                                    return "";
                                else
                                    return Common.Format.CommaSeparation(data);
                            }
                        },//14
                        {
                            data: "CapexAmount", render: function (data) {
                                if (data == null)
                                    return "";
                                else
                                    return Common.Format.CommaSeparation(data);
                            }
                        },//15
                        { data: "FileName" },//16
                        { data: "ID" },//17
                        { data: "CaseCategoryID" },//18
                        { data: "CaseDetailID" },//19
                        { data: "Remarks" },//20
                        { data: "ViewIdx" },//21
                        //{ data: "SubmissionType" },//22

                    ],
                    "columnDefs": [{ "targets": [4, 16, 17, 18, 19, 20], "visible": false }, { "targets": [1, 2, 3, 5, 6, 7, 8, 9, 10, 11, 12], "class": "text-center" }, { "targets": [13, 14, 15], "class": "text-right" }],
                    "scrollY": 300, /* Enable vertical scroll to allow fixed columns */
                    "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
                    "scrollCollapse": true,
                    "fixedColumns": {
                        leftColumns: 3 /* Set the 2 most left columns as fixed columns */
                    },
                    "fnDrawCallback": function () {
                        if (dataType == "new") {
                            $(".btnExportDetail").hide();
                        } else {
                            $(".btnExportDetail").show();
                        }
                    },
                });

                $("#tblRequestDetail tbody").unbind();

                $("#tblRequestDetail tbody").on("click", ".deleteRow", function (e) {
                    var table = $("#tblRequestDetail").DataTable();
                    var row = table.row($(this).parents('tr')).data();
                    table.row($(this).parents('tr')).remove().draw(false);
                    Control.DeleteRequestDetailDraft(row.ViewIdx);

                });
                $("#tblRequestDetail tbody").on("click", ".editRow", function (e) {
                    var table = $(id).DataTable();
                    var row = table.row($(this).parents('tr')).data();
                    $("#iCategoryCaseID").val(RowSelected.CaseCategoryID).trigger('change');
                    $("#iDetailCaseID").val(RowSelected.CaseDetailID).trigger('change');
                    $("#iRemarks").val(RowSelected.Remarks);
                    $("#iTrxDetail").val(RowSelected.ID);
                    $("#iEffectiveDate").val(Common.Format.ConvertJSONDateTime(RowSelected.EffectiveDate));
                    $("#iSONumber").val(RowSelected.SONumber);
                    $(".fileUpload").show();
                    $("#sRFIDate").css('visibility', 'hidden');
                    if ((ActLabel == "SA_FEED_DIV_ACC" || ActLabel == "SA_FEED_DEPT_ACC" || ActLabel == "")) {
                        $("#btnTrxSaveDetail").show();
                        $(".uploadFile").show();
                    } else if (ActLabel != "SA_FEED_DIV_ACC") {
                        $("#iRemarks").prop("disabled", true);
                        $(".uploadFile").hide();
                        $("#btnTrxSaveDetail").hide();
                    }
                    if (HeaderID == null || HeaderID == "") {
                        CreteType = "edit";
                    } else {
                        $("#btnTrxResetDetail").hide();
                        CreteType = "revise";
                    }
                    var fileName = RowSelected.FileName.split(',');
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
                if (dataType == "new" || isReHold == false)
                    Colums.column(3).visible(false);
                else
                    Colums.column(3).visible(true);

                if (dataType == "new" && $("#iRequestTypeID option:selected").html() == "HOLD")
                    Colums.column(12).visible(false);
                else
                    Colums.column(12).visible(true);

                var id = $(this).parent().attr('id');
                //var table = $("#tblSonumbList").DataTable();// AYR

                requestDetail = [];

                //$(".timeline").html('');
                //if (dataType == "new") {
                var table = $("#tblSonumbList").DataTable();

                //var DataRows = {};
                var Row = table.row($(this).parents('tr')).data();
                    for (var i = 0; i < data.length; i++) {
                        RowSelected = {};
                        RowSelected.RowIndex = i + 1;
                        RowSelected.ID = data[i].ID;
                        RowSelected.SONumber = data[i].SONumber; //DataRows.SONumber = ;
                        RowSelected.RFIDone = data[i].RFIDone; //DataRows.RFIDone =
                        RowSelected.SiteID = data[i].SiteID; //DataRows.SiteID =
                        RowSelected.SiteName = data[i].SiteName;  //DataRows.SiteName =
                        RowSelected.Customer = data[i].Customer; //DataRows.Customer =
                        RowSelected.Company = data[i].Company; //DataRows.Company =
                        RowSelected.Product = data[i].Product; //DataRows.Product =
                        RowSelected.ViewIdx = data[i].ViewIdx + 1; //DataRows.ViewIdx =
                        RowSelected.CaseCategoryID = data[i].CaseCategoryID;
                        RowSelected.CaseDetailID = data[i].CaseDetailID;
                        RowSelected.CategoryCase = data[i].CategoryCase;
                        RowSelected.DetailCase = data[i].DetailCase;
                        RowSelected.Remarks = data[i].Remarks;
                        RowSelected.EffectiveDate = data[i].EffectiveDate;
                        RowSelected.FileName = data[i].FileName;
                        RowSelected.IsHold = data[i].IsHold;
                        requestDetail.push(RowSelected);
                    }
                    TempRequestDetail = requestDetail;
                    SubmissionTypeVal = data[0].RequestTypeID;
                    $("#iRequestTypeID").val(SubmissionTypeVal).trigger('change');
                    $("#iRequestTypeID").prop('disabled', true);
                    $("#iCategoryCaseID").val(data[0].CaseCategoryID).trigger('change');
                    $("#iRemarksHeader").val(data[0].Remarks);
            } else {
                dataType = "new";
                HeaderID = "";
                requestDetail = [];
                $(".timeline").html('');
                $("#iRemarksHeader").val('');
                $("#iHeaderID").val('');
                $("#iStatusActID").val('').trigger('change');
                $("#fileUploaded").html("");
                $("#pnlHeader").hide();
                $("#spanFormNumber").text("");
                $(".historyActivity").hide();
                $(".approvalFrom").hide();
                $(".reHoldAccruePnl").hide();
                $(".tblRequestDetails").hide();
                $("#btSaveRequestDetail").show();
                $("#btResetRequestDetail").show();
                $("#btSaveEdit").hide();
                $("#btnTrxResetDetail").show();
                $("#btnTrxSaveDetail").show();
                $(".approvalRemarks").show();
                $(".tblSonumbList").show();
                $(".filterDetail").show();
                $(".uploadFile").show();
                $("#EndEffective").show();
                $("#iRequestTypeID").prop('disabled', false);
                $("#StartEffective").prop('disabled', false);
                $("#EndEffective").prop('disabled', false);
                $("#iRemarksHeader").prop('disabled', false);
                $("#iRemarks").prop('disabled', false);
                $("#pnlDetail").fadeIn(2000);
                $("#pnlDetail").css('visibility', 'visible');
                Table.LoadSonumbList();
                Table.Init("#tblRequestDetail");
            }
            l.stop();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            l.stop();
        });
    },
}

/* table manipulation */
var Table = {
    /* initialisation tables*/
    Init: function (id) {
        $(id).dataTable({
            "filter": false,
            "destroy": true,
            "data": [],
        });

        $(window).resize(function () {
            $(id).DataTable().columns.adjust().draw();
        });
    },

    LoadHeader: function () {
        var id = "#tblRequestHeader";
        Table.Init(id);
        var l = Ladda.create(document.querySelector("#loadHeader"));
        var l2 = Ladda.create(document.querySelector("#btSearchRequest"));
        l.start();
        l2.start();
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
                "url": "/api/StopAccrue/RequestHeader",
                "type": "POST",
                "datatype": "json",
                "data": paramHeader,
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
            "order": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    data: "RowIndex",
                },
                {
                    render: function (a, b, c) {
                        var strReturn = "";
                        //if (c.ActivityLabel == "SA_PRINT_DOC" && c.ActivityOwner == userLoginID)
                        //    strReturn += "<button  type='button' class='btn yellow mt-ladda-btn ladda-button printHeader' data-style='zoom-in'><i class='fa fa-print' title='Print'></i>&nbsp;Print</button>";

                        if (c.ActivityLabel == "SA_SUBMIT_DOC" && c.ActivityOwner == userLoginID) {
                            strReturn += "<button  type='button' class='btn btn-xs blue  printHeader' data-style='zoom-in'><i class='fa fa-print' title='Print'></i>&nbsp;Print</button>";
                            strReturn += "<button  type='button' class='btn  btn-xs green  uploadDoc' data-style='zoom-in'><i class='fa fa-upload' title='Upload'></i>&nbsp;Upload</button>";
                        }


                        else if ((c.ActivityLabel == "SA_DOC_RECEIVE_DHAC" || c.ActivityLabel == "SA_DOC_RECEIVE_DHAS") && c.ActivityOwner == userLoginID) {
                            strReturn += "<button  type='button ' class='btn  btn-xs green  downloadDoc' data-style='zoom-in'><i class='fa fa-download' title='Download'></i>&nbsp;Download</button>";
                            //  strReturn += "<button  type='button' class='btn btn-xs white' data-style='zoom-in'><label class='mt-checkbox mt-checkbox-single mt-checkbox-outline   lb_" + c.ID + "' disabled><input type='checkbox' class='checkboxes cb_" + c.ID + "'/><span></span></label>Receive</button>";
                        }


                        else if (c.IsReHoldReady && c.ActivityLabel == "SA_STOP")
                            strReturn += "<button  type='button' class='btn  btn-xs green  reholdAccrue' data-style='zoom-in'><i class='fa fa-undo' title='ReHold'></i>&nbsp;Rehold</button>";

                        //orderable: false,
                            //var strReturn = ""
                            else if (ActLabel == "")
                                strReturn += "<i class='fa fa-pencil btn btn-xs yellow editRow'></i>";

                        return strReturn;
                    }
                },
                {
                    data: "RequestNumber", render: function (a, b, c) {
                        return "<a class='linkDetail' title='Detail Request'>" + c.RequestNumber + "</a>";
                    }
                },

                { data: "InitiatorName" },
                { data: "ActivityOwnerName" },
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
                { data: "ActivityName" },
                {
                    data: "AccrueType", render: function (data) {
                        return data + " Acrrue";
                    }
                },
                { data: "ActivityLabel" },
                { data: "ActivityID" },
                { data: "PrevActivityID" },
                { data: "PrevActivityLabel" },
                { data: "RequestTypeID" },
                { data: "ID" },
                { data: "Initiator" },
                { data: "AppHeaderID" },
                { data: "ActivityOwner" },
                { data: "IsReHold" },
                { data: "FileName" },
            ],
            "columnDefs": [{ "targets": [10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20], "visible": false }, { "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], "class": "text-center" }],
            "fnDrawCallback": function () {
                l.stop();
                l2.stop();
                $(".loadHeader").fadeOut(100);
                $(id + " tbody").fadeIn(1500);
            },
        });

        $(id + " tbody").unbind();

        /*button for show up detail request*/
        $(id + " tbody").on("click", ".linkDetail", function (e) {
            var table = $(id).DataTable();
            var row = table.row($(this).parents('tr')).data();
            dataType = "";
            PrevActLabel = row.PrevActivityLabel;
            ActLabel = row.ActivityLabel;
            AppHeaderID = row.AppHeaderID;
            PrevActivityID = row.PrevActivityID;
            ActivityID = row.ActivityID;
            isReHold = row.IsReHold;
            RequestType = row.AccrueType
            //|| row.ActivityLabel.toUpperCase() == "SA_DOC_RECEIVE_DHAC" || row.ActivityLabel.toUpperCase() == "SA_DOC_RECEIVE_DHAS"
            if (((ActLabel != "SA_FEED_DIV_ACC" || ActLabel != "SA_FEED_DEPT_ACC") && userLoginID == row.Initiator) || ActLabel == "SA_STOP") {
                $("#btSaveRequestDetail").hide();
                $("#btResetRequestDetail").hide();
                $("#btSaveEdit").hide();
                $(".approvalFrom").hide();
                $(".approvalRemarks").hide();
            }

            if ((ActLabel == "SA_FEED_DIV_ACC" || ActLabel == "SA_FEED_DEPT_ACC") && row.ActivityOwner != "") {
                $("#btSaveRequestDetail").show();
                $("#btResetRequestDetail").hide();
                $("#btSaveEdit").hide();
                $(".approvalFrom").hide();
                $(".approvalRemarks").show();

            }
            else if ((ActLabel == "SA_FEED_DIV_ACC" || ActLabel == "SA_FEED_DEPT_ACC") && row.ActivityOwner == "") {
                $("#btSaveRequestDetail").hide();
                $("#btResetRequestDetail").hide();
                $("#btSaveEdit").hide();
                $(".approvalFrom").hide();
                $(".approvalRemarks").hide();
            }

            if (row.AccrueType.toUpperCase() == "STOP") {
                $(".StartEffectiveDate").hide();
            } else {
                $(".StartEffectiveDate").show();
            }

            $("#spnSubmit").text("Submit");
            if (ActLabel.toUpperCase() == "SA_DOC_RECEIVE_DHAS") {
                $(".approvalFrom").hide();
                $("#spnSubmit").text("Receive");
            }
            $("#spnSubmitDraft").text("Draft");
            if (ActLabel.toUpperCase() == "SA_DOC_RECEIVE_DHAS") {
                $(".approvalFrom").hide();
                $("#spnSubmitDraft").text("Receive");
            }
            $("#spnClearDraft").text("Clear");
            if (ActLabel.toUpperCase() == "SA_DOC_RECEIVE_DHAS") {
                $(".approvalFrom").hide();
                $("#spnClearDraft").text("Receive");
            }

            $("#btSaveReqDetailDraft").hide();
            $("#btClearReqDetailDraft").hide();
            $("#iRemarksHeader").val('');
            $(".historyActivity").show();
            $("#iRequestTypeID").val(row.RequestTypeID).trigger('change');
            $("#StartEffective").val(Common.Format.ConvertJSONDateTime(row.StartEffectiveDate));
            $("#EndEffective").val(Common.Format.ConvertJSONDateTime(row.EndEffectiveDate));
            $("#pnlHeader").hide();
            $("#pnlDetail").fadeIn(2000);
            $("#pnlDetail").css('visibility', 'visible');
            $(".tblSonumbList").hide();
            $(".filterDetail").hide();
            $("#iRequestTypeID").prop('disabled', true);
            $("#StartEffective").prop('disabled', true);
            $("#EndEffective").prop('disabled', true);
            $("#iCategoryCaseID").prop('disabled', true);
            $("#iDetailCaseID").prop('disabled', true);
            HeaderID = row.ID;
            $("#iHeaderID").val(HeaderID);
            $("#spanFormNumber").text("(" + row.RequestNumber + ")");
            RequestNumber = row.RequestNumber;
            BindData.BindNextFlag(row.ActivityLabel);
            Table.LoadDetail();


        });

        /* button for print header request*/
        $(id + " tbody").on("click", ".printHeader", function (e) {
            var table = $(id).DataTable();
            var row = table.row($(this).parents('tr')).data();
            var params = { HeaderID: row.ID, AppHeaderID: row.AppHeaderID, InitiatorName: row.InitiatorName, RequestNumber: row.RequestNumber, RequestType: row.AccrueType, IsReHold: row.IsReHold, StartEffectiveDate: row.StartEffectiveDate, EndEffectiveDate: row.EndEffectiveDate };
            // Table.PrintHeaderRequest(row.ID, row.AppHeaderID, row.InitiatorName, row.RequestNumber, row.AccrueType, row.IsReHold, row.StartEffectiveDate, row.EndEffectiveDate);
            Table.PrintHeaderRequest(params);
            ////  setTimeout(function () {
            //      Table.LoadHeader();
            //  }, 1000);

        });
        /*button for upload document after print document*/
        $(id + " tbody").on("click", ".uploadDoc", function (e) {
            var table = $(id).DataTable();
            var row = table.row($(this).parents('tr')).data();
            $("#lbSubmitDoc").text(row.RequestNumber);
            $("#uploadHeaderID").val(row.ID);
            $("#uploadAppHeaderID").val(row.AppHeaderID);
            $("#mdlUploadDoc").modal('show');
        });

        $(id + " tbody").on("click", ".downloadDoc", function (e) {
            var table = $(id).DataTable();
            var row = table.row($(this).parents('tr')).data();

            window.location.href = "/StopAccrue/downloadDocSubmit?fileName=" + row.FileName + "&activity=" + row.ActivityName + "&appHeaderID=" + row.AppHeaderID;

        });

        /*button for rehold accrue request*/
        $(id + " tbody").on("click", ".reholdAccrue", function (e) {
            var table = $(id).DataTable();
            var row = table.row($(this).parents('tr')).data();
            dataType = "rehold";
            HeaderID = row.ID;
            AppHeaderID = row.AppHeaderID;
            RequestTypeID = row.RequestTypeID;
            RequestType = row.AccrueType;
            PrevActLabel = row.PrevActivityLabel;
            ActLabel = row.ActivityLabel;
            PrevActivityID = row.PrevActivityID;
            ActivityID = row.ActivityID;
            RequestNumber = row.RequestNumber;
            isReHold = true;
            prevStartEffectiveDate = row.EndEffectiveDate;
            TempRequestReHold = [];
            $("#btSaveRequestDetail").show();
            $("#btResetRequestDetail").show();
            $(".approvalFrom").hide();
            $(".approvalRemarks").show();
            $("#pnlHeader").hide();
            $("#pnlDetail").fadeIn(2000);
            $("#pnlDetail").css('visibility', 'visible');
            $(".tblSonumbList").hide();
            $(".filterDetail").hide();
            $("#iRequestTypeID").prop('disabled', false);
            $("#StartEffective").prop('disabled', false);
            $("#EndEffective").prop('disabled', false);
            $("#iCategoryCaseID").prop('disabled', true);
            $("#iDetailCaseID").prop('disabled', true);
            $("#iHeaderID").val(HeaderID);
            $("#spanFormNumber").text("(" + row.RequestNumber + ")");
            BindData.BindNextFlag(row.ActivityID);
            Table.LoadDetail();
            Table.ReHoldAcrrueTable();
            $(".reHoldAccruePnl").hide();
        });

        /*button for edit accrue request Header*/
        $("#tblRequestHeader tbody").on("click", ".editRow", function (e) {
            {
                $(".reHoldAccruePnl").hide();
                var table = $("#tblRequestHeader").DataTable();
                var row = table.row($(this).parents('tr')).data();

                RequestNumber = row.RequestNumber
                var params = { RequestNumber: RequestNumber };

                var l = Ladda.create(document.querySelector("#btnAddRequest"));
                l.start();
                $.ajax({
                    url: "/api/StopAccrue/EditHeader",
                    type: "GET",
                    dataType: "json",
                    async: false,
                    data: params,
                }).done(function (data) {
                    if (data.length > 0) {
                        dataType = "new";
                        HeaderID = "";
                        requestDetail = [];
                        $(".timeline").html('');
                        $("#iRemarksHeader").val('');
                        $("#iHeaderID").val('');
                        $("#iStatusActID").val('').trigger('change');
                        $("#fileUploaded").html("");
                        $("#pnlHeader").hide();
                        $("#spanFormNumber").text("");
                        $(".historyActivity").hide();
                        $(".approvalFrom").hide();
                        $(".reHoldAccruePnl").hide();
                        $(".tblRequestDetails").show();
                        $("#btSaveRequestDetail").hide();
                        $("#btSaveEdit").show();
                        $("#btSaveReqDetailDraft").hide();
                        $("#btClearReqDetailDraft").hide();
                        $("#btResetRequestDetail").hide();
                        $("#btnTrxResetDetail").show();
                        $("#btnTrxSaveDetail").show();
                        $(".approvalRemarks").show();
                        $(".tblSonumbList").show();
                        $(".filterDetail").show();
                        $(".uploadFile").show();
                        $("#EndEffective").show();
                        $("#iRequestTypeID").prop('disabled', false);
                        $("#StartEffective").prop('disabled', false);
                        $("#EndEffective").prop('disabled', false);
                        $("#iRemarksHeader").prop('disabled', false);
                        $("#iRemarks").prop('disabled', false);
                        $("#pnlDetail").fadeIn(2000);
                        $("#pnlDetail").css('visibility', 'visible');
                        Table.LoadSonumbList();

                        var tblReqDetail = $("#tblRequestDetail").DataTable({
                            "proccessing": true,
                            "serverSide": false,
                            "bSortCellsTop": true,
                            "language": {
                                "emptyTable": "No data available in table",
                            },

                            //"serverside": false,
                            //"filter": false,
                            //"destroy": true,
                            //"async": false,
                            "data": data,
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
                                    orderable: false,
                                    mRender: function (a, b, c) {
                                        var strReturn = ""
                                        if (ActLabel != "")
                                            strReturn += "<i class='fa fa-eye btn btn-xs yellow editRow' ></i>";
                                        else if (ActLabel == "")
                                            strReturn += "<i class='fa fa-trash btn btn-xs red deleteRow'></i>&nbsp;&nbsp;&nbsp;<i class='fa fa-pencil btn btn-xs yellow editRow'></i>";
                                        return strReturn;
                                    }
                                },
                                { data: "SONumber" },
                                {
                                    mRender: function (a, b, c) {
                                        if (c.IsHold) {
                                            if (dataType == "rehold")
                                                return "&nbsp;&nbsp;<label class='mt-checkbox mt-checkbox-single mt-checkbox-outline  lb_" + c.ID + "' disabled><input type='checkbox' class='checkboxes cb_" + c.ID + "'/><span></span></label>";
                                            else
                                                return "<i class='fa fa-remove'></i>";
                                        }
                                        else {
                                            return "<i class='fa fa-check'></i>";
                                        }
                                    },
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
                                {
                                    data: "CategoryCase", render: function (data) {
                                        if (data == '1')
                                        { return 'INTERNAL' }
                                        else
                                        { return 'EXTERNAL' }
                                    }

                                },
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
                                { data: "ViewIdx" },
                                //{ data: "SubmissionType" },

                            ],
                            "columnDefs": [{ "targets": [4, 16, 17, 18, 19, 20], "visible": false }, { "targets": [1, 2, 3, 5, 6, 7, 8, 9, 10, 11, 12], "class": "text-center" }, { "targets": [13, 14, 15], "class": "text-right" }],
                            "fnDrawCallback": function () {
                                if (dataType == "new") {
                                    $(".btnExportDetail").hide();
                                } else {
                                    $(".btnExportDetail").show();
                                }
                            },
                        });

                        $("#tblRequestDetail tbody").unbind();

                        $("#tblRequestDetail tbody").on("click", ".deleteRow", function (e) {
                            var table = $("#tblRequestDetail").DataTable();
                            var row = table.row($(this).parents('tr')).data();
                            table.row($(this).parents('tr')).remove().draw(false);
                            Control.DeleteRequestDetail(row.ViewIdx);

                        });
                        $("#tblRequestDetail tbody").on("click", ".editRow", function (e) {
                            var table = $(id).DataTable();
                            var row = table.row($(this).parents('tr')).data();
                            $("#iCategoryCaseID").val(RowSelected.CaseCategoryID).trigger('change');
                            $("#iDetailCaseID").val(RowSelected.CaseDetailID).trigger('change');
                            $("#iRemarks").val(RowSelected.Remarks);
                            $("#iTrxDetail").val(RowSelected.ID);
                            $("#iEffectiveDate").val(Common.Format.ConvertJSONDateTime(RowSelected.EffectiveDate));
                            $("#iSONumber").val(RowSelected.SONumber);
                            $(".fileUpload").show();
                            $("#sRFIDate").css('visibility', 'hidden');
                            if ((ActLabel == "SA_FEED_DIV_ACC" || ActLabel == "SA_FEED_DEPT_ACC" || ActLabel == "")) {
                                $("#btnTrxSaveDetail").show();
                                $(".uploadFile").show();
                            } else if (ActLabel != "SA_FEED_DIV_ACC") {
                                $("#iRemarks").prop("disabled", true);
                                $(".uploadFile").hide();
                                $("#btnTrxSaveDetail").hide();
                            }
                            if (HeaderID == null || HeaderID == "") {
                                CreteType = "edit";
                            } else {
                                $("#btnTrxResetDetail").hide();
                                CreteType = "revise";
                            }
                            var fileName = RowSelected.FileName.split(',');
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
                        if (dataType == "new" || isReHold == false)
                            Colums.column(3).visible(false);
                        else
                            Colums.column(3).visible(true);

                        if (dataType == "new" && $("#iRequestTypeID option:selected").html() == "HOLD")
                            Colums.column(12).visible(false);
                        else
                            Colums.column(12).visible(true);

                        var id = $(this).parent().attr('id');
                        //var table = $("#tblSonumbList").DataTable();// AYR

                        requestDetail = [];

                        //$(".timeline").html('');
                        //if (dataType == "new") {
                        var table = $("#tblSonumbList").DataTable();

                        var DataRows = {};
                        var Row = table.row($(this).parents('tr')).data();
                        for (var i = 0; i < data.length; i++) {
                            RowSelected = {};
                            RowSelected.RowIndex = i + 1;
                            RowSelected.ID = data[i].ID;
                            RowSelected.SONumber = data[i].SONumber;
                            RowSelected.RFIDone = data[i].RFIDone;
                            RowSelected.SiteID = data[i].SiteID;
                            RowSelected.SiteName = data[i].SiteName;
                            RowSelected.Customer = data[i].Customer;
                            RowSelected.Company = data[i].Company;
                            RowSelected.Product = data[i].Product;
                            RowSelected.CaseCategoryID = data[i].CaseCategoryID;
                            RowSelected.CaseDetailID = data[i].CaseDetailID;
                            RowSelected.CategoryCase = data[i].CategoryCase;
                            RowSelected.DetailCase = data[i].DetailCase;
                            RowSelected.Remarks = data[i].Remarks;
                            RowSelected.EffectiveDate = data[i].EffectiveDate;
                            RowSelected.FileName = data[i].FileName;
                            RowSelected.ViewIdx = data[i].ViewIdx;
                            RowSelected.IsHold = data[i].IsHold;
                            requestDetail.push(RowSelected);

                        }
                        TempRequestDetail = requestDetail;
                        SubmissionTypeVal = data[0].RequestTypeID;
                        $("#iRequestTypeID").val(SubmissionTypeVal).trigger('change');
                        $("#iRequestTypeID").prop('disabled', true);
                        $("#iCategoryCaseID").val(data[0].CaseCategoryID).trigger('change');
                        $("#iRemarksHeader").val(data[0].Remarks);



                        var StartDate = data[0].StartEffectiveDate;
                        var EndDate = data[0].EndEffectiveDate;
                        //var Sdate = StartDate.datepicker({ dateFormat: 'dd M yyyy' });

                        if (StartDate != null)
                        {
                            var formattedDate = new Date(StartDate);
                            var d = formattedDate.getDate();
                            var m = formattedDate.getMonth() + 1;
                            var y = formattedDate.getFullYear();
                            var dateString = y + "-" + m + "-" + d;
                            $("#StartEffective").val(dateString);
                            //$("#StartEffective").prop("disabled", true);
                        }
                        if (EndDate != null)
                        {
                            var formattedDateEnd = new Date(EndDate);
                            var dEnd = formattedDateEnd.getDate();
                            var mEnd = formattedDateEnd.getMonth() + 1;
                            var yEnd = formattedDateEnd.getFullYear();
                            var dateStringEnd = yEnd + "-" + mEnd + "-" + dEnd;
                            $("#EndEffective").val(dateStringEnd);
                            //$("#EndEffective").prop("disabled", true)
                        }
                        
                    } else {
                        dataType = "new";
                        HeaderID = "";
                        requestDetail = [];
                        $(".timeline").html('');
                        $("#iRemarksHeader").val('');
                        $("#iHeaderID").val('');
                        $("#iStatusActID").val('').trigger('change');
                        $("#fileUploaded").html("");
                        $("#pnlHeader").hide();
                        $("#spanFormNumber").text("");
                        $(".historyActivity").hide();
                        $(".approvalFrom").hide();
                        $(".reHoldAccruePnl").hide();
                        $(".tblRequestDetails").hide();
                        $("#btSaveRequestDetail").show();
                        $("#btResetRequestDetail").show();
                        $("#btSaveEdit").hide();
                        $("#btSaveReqDetailDraft").show();
                        $("#btClearReqDetailDraft").show();
                        $("#btnTrxResetDetail").show();
                        $("#btnTrxSaveDetail").show();
                        $(".approvalRemarks").show();
                        $(".tblSonumbList").show();
                        $(".filterDetail").show();
                        $(".uploadFile").show();
                        $("#EndEffective").show();
                        $("#iRequestTypeID").prop('disabled', false);
                        $("#StartEffective").prop('disabled', false);
                        $("#EndEffective").prop('disabled', false);
                        $("#iRemarksHeader").prop('disabled', false);
                        $("#iRemarks").prop('disabled', false);
                        $("#pnlDetail").fadeIn(2000);
                        $("#pnlDetail").css('visibility', 'visible');
                        Table.LoadSonumbList();
                        Table.Init("#tblRequestDetail");
                    }
                    l.stop();
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    Common.Alert.Error(errorThrown);
                    l.stop();
                });
            }
        });
    },
    /* load detail request */
    LoadDetail: function () {
        requestDetail = [];
        $(".timeline").html('');
        if (dataType == "new") {
            for (var i = 0; i < TempRequestDetail.length; i++) {
                RowSelected = {};
                RowSelected.RowIndex = i + 1;
                RowSelected.ID = TempRequestDetail[i].ID;
                RowSelected.SONumber = TempRequestDetail[i].SONumber;
                RowSelected.RFIDone = TempRequestDetail[i].RFIDone;
                RowSelected.SiteID = TempRequestDetail[i].SiteID;
                RowSelected.SiteName = TempRequestDetail[i].SiteName;
                RowSelected.Customer = TempRequestDetail[i].Customer;
                RowSelected.Company = TempRequestDetail[i].Company;
                RowSelected.Product = TempRequestDetail[i].Product;
                RowSelected.CaseCategoryID = TempRequestDetail[i].CaseCategoryID;
                RowSelected.CaseDetailID = TempRequestDetail[i].CaseDetailID;
                RowSelected.CategoryCase = TempRequestDetail[i].CategoryCase;
                RowSelected.DetailCase = TempRequestDetail[i].DetailCase;
                RowSelected.Remarks = TempRequestDetail[i].Remarks;
                RowSelected.EffectiveDate = TempRequestDetail[i].EffectiveDate;
                RowSelected.FileName = TempRequestDetail[i].FileName;
                RowSelected.ViewIdx = TempRequestDetail[i].ViewIdx;
                RowSelected.IsHold = TempRequestDetail[i].IsHold;
                requestDetail.push(RowSelected);
            }
            TempRequestDetail = requestDetail;
            Table.DetailTable();
        }
        else if (HeaderID != null && HeaderID != "") {
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
                $(".timeline").append(data.HtmlElements);
                Table.DetailTable();
            }).fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
        }
    },
    /* table detail request */
    DetailTable: function () {
        $(".reHoldAccruePnl").hide();
        var id = "#tblRequestDetail";
        //Table.Init(id);
        if (dataType == "rehold")
            $(".reHoldAccruePnl").show();
        else
            $(".reHoldAccruePnl").hide();

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
                { data: "RowIndex" }, //#1
                {
                    orderable: false,
                    mRender: function (a, b, c) {
                        var strReturn = ""
                        if (ActLabel != "")
                            strReturn += "<i class='fa fa-eye btn btn-xs yellow editRow' ></i>";
                        else if (ActLabel == "")
                            strReturn += "<i class='fa fa-trash btn btn-xs red deleteRow'></i>&nbsp;&nbsp;&nbsp;<i class='fa fa-pencil btn btn-xs yellow editRow'></i>";



                        return strReturn;
                    }
                },  //#2
                { data: "SONumber" }, //#3
                {
                    mRender: function (a, b, c) {
                        if (c.IsHold) {
                            if (dataType == "rehold")
                                return "&nbsp;&nbsp;<label class='mt-checkbox mt-checkbox-single mt-checkbox-outline  lb_" + c.ID + "' disabled><input type='checkbox' class='checkboxes cb_" + c.ID + "'/><span></span></label>";
                            else
                                return "<i class='fa fa-remove'></i>";
                        }
                        else {
                            return "<i class='fa fa-check'></i>";
                        }
                    },
                }, //#4
                { data: "SiteID" }, //#5
                { data: "SiteName" }, //#6
                { data: "Company" }, //#7
                { data: "Customer" }, //#8
                { data: "Product" }, //#9
                {
                    data: "RFIDone", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                }, //#10
                { data: "CategoryCase" }, //#11
                { data: "DetailCase" }, //#12
                {
                    data: "EffectiveDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                }, //#13
                {
                    data: "RevenueAmount", render: function (data) {
                        if (data == null)
                            return "";
                        else
                            return Common.Format.CommaSeparation(data);
                    }
                }, //#14
                {
                    data: "CapexAmount", render: function (data) {
                        if (data == null)
                            return "";
                        else
                            return Common.Format.CommaSeparation(data);
                    }
                }, //#15
                { data: "FileName" }, //#16
                { data: "ID" }, //#17
                { data: "CaseCategoryID" }, //#18
                { data: "CaseDetailID" }, //#19
                { data: "Remarks" }, //#20
                { data: "ViewIdx" }, //#21
            ],
            "columnDefs": [{ "targets": [4, 17, 18, 19, 20], "visible": false }, { "targets": [1, 2, 3, 5, 6, 7, 8, 9, 10, 11, 12], "class": "text-center" }, { "targets": [13, 14], "class": "text-right" }],
            "fnDrawCallback": function () {
                if (dataType == "new") {
                    $(".btnExportDetail").hide();
                } else {
                    $(".btnExportDetail").show();
                }
            },
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': []
            }],
            "scrollY": 300, /* Enable vertical scroll to allow fixed columns */
            "scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            "scrollCollapse": true,
            "fixedColumns": {
                leftColumns: 3 /* Set the 2 most left columns as fixed columns */
            },
            "order": []
        });
        $("#tblRequestDetail tbody").unbind();
        $("#tblRequestDetail tbody").on("click", ".deleteRow", function (e) {
            var table = $(id).DataTable();
            var row = table.row($(this).parents('tr')).data();
            table.row($(this).parents('tr')).remove().draw(false);
            Control.DeleteRequestDetail(row.SoNumber);

        });
        $("#tblRequestDetail tbody").on("click", ".editRow", function (e) {
            var table = $(id).DataTable();
            var row = table.row($(this).parents('tr')).data();
            $("#iCategoryCaseID").val(row.CaseCategoryID).trigger('change');
            $("#iDetailCaseID").val(row.CaseDetailID).trigger('change');
            $("#iRemarks").val(row.Remarks);
            $("#iTrxDetail").val(row.ID);
            $("#iEffectiveDate").val(Common.Format.ConvertJSONDateTime(row.EffectiveDate));
            $("#iSONumber").val(row.SONumber);
            $(".fileUpload").show();
            $("#sRFIDate").css('visibility', 'hidden');
            if ((ActLabel == "SA_FEED_DIV_ACC" || ActLabel == "SA_FEED_DEPT_ACC" || ActLabel == "")) {
                $("#btnTrxSaveDetail").show();
                $(".uploadFile").show();
            } else if (ActLabel != "SA_FEED_DIV_ACC") {
                $("#iRemarks").prop("disabled", true);
                $(".uploadFile").hide();
                $("#btnTrxSaveDetail").hide();
            }
            if (HeaderID == null || HeaderID == "") {
                CreteType = "edit";
            } else {

                $("#btnTrxResetDetail").hide();
                CreteType = "revise";
            }
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
        if (dataType == "new" || isReHold == false)
            Colums.column(3).visible(false);
        else
            Colums.column(3).visible(true);


        if (dataType == "new" && $("#iRequestTypeID option:selected").html() == "HOLD")
            Colums.column(12).visible(false);
        else
            Colums.column(12).visible(true);

    },

    ReHoldAcrrueTable: function () {
        Table.Init("#tblReHoldAccruRequest");

        $("#tblReHoldAccruRequest").DataTable({
            "serverSide": false,
            "filter": true,
            "destroy": true,
            "async": false,
            "data": TempRequestReHold,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "columns": [
                { data: "SONumber" },
                {
                    data: "Remarks",
                    render: function (a, b, c) {
                        return "<input type='text' class='form-control' id='remarks_" + c.ID + "'>";
                    }
                }]
        });
    },
    /* load data so number list for request stop accrue */
    LoadSonumbList: function () {
        var id = "#tblSonumbList";
        //Table.Init(id);

        $(".loadSonumb").fadeIn(100);
        $(id + " tbody").hide();
        var l = Ladda.create(document.querySelector("#loadSonumb"));
        var l2 = Ladda.create(document.querySelector("#btSearchRequestDetail"));
        l.start();
        l2.start();
        var tblHeader = $(id).DataTable({
            "destroy": true,
            "deferRender": false,
            "proccessing": false,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/StopAccrue/SonumbList",
                "type": "POST",
                "datatype": "json",
                "data": paramSoNumbList,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
                {
                    text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                        var l = Ladda.create(document.querySelector(".yellow"));
                        l.start();
                        Table.ExportSonumbList();
                        l.stop();
                    }
                },
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "order": [[0, 'asc']],
            "columns": [
                { data: "RowIndex" },
                {
                    data: "RowIndex", render: function (i, e, data) {
                        return "<label class='mt-checkbox mt-checkbox-single mt-checkbox-outline lb_" + data.ViewIdx + "' disabled><input type='checkbox' class='checkboxes cb_" + data.ViewIdx + "'/><span></span></label>";
                    }
                },
                { data: "SONumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "RegionName" },
                {
                    data: "RFIDone", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },
                { data: "CustomerID" },
                { data: "CompanyID" },
                { data: "Product" },
                    
                { data: "RegionID" },
                { data: "ViewIdx" },
            ],
            "columnDefs": [{ "targets": [10, 11], "visible": false }, { "targets": [1, 2, 3, 4, 5, 6, 7, 8], "class": "text-center" }],
            "fnDrawCallback": function () {
                Control.SelectData();
                l.stop();
                l2.stop();
                $(".loadSonumb").fadeOut(100);
                $(id + " tbody").fadeIn(1000);
            },
            "order": [],
            //"scrollY": 300,
            //"scrollX": true, /* Enable horizontal scroll to allow fixed columns */
            //"scrollCollapse": true,
            //"fixedColumns": {
            //    leftColumns: 3  /* Set the 2 most left columns as fixed columns */
            //},
            "createdRow": function (row, data, index) {
                /* Add Unique CSS Class to row as identifier in the cloned table */
                var id = $(row).attr("id");
                $(row).addClass(id);
            },
        });
        $(id + " tbody").unbind();
    },
    /* export header request to excel */
    ExportHeader: function () {
        window.location.href = "/StopAccrue/exportRequestHeader?" + $.param(paramHeader);
    },
    /* export detail request to excel */
    ExportDetail: function () {
        if (HeaderID != null && HeaderID != "") {
            window.location.href = "/StopAccrue/exportRequestDetail?&HeaderID=" + HeaderID + "&IsReHold=" + isReHold + "&RequestNumber=" + RequestNumber;
        }
    },
    /* Eport so number ready to submit request to excel */
    ExportSonumbList: function () {
        window.location.href = "/StopAccrue/exportSonumbList?" + $.param(paramSoNumbList);
    },
    /* Printe header request*/
    //PrintHeaderRequest: function (id, appheaderID, initiator, requestNumber, accrueType, isRehold) {
    //    var params = { HeaderID: id, AppHeaderID: appheaderID, InitiatorName: initiator, RequestNumber: requestNumber, RequestType: accrueType, IsReHold:isReHold };
    //    window.location.href = "/StopAccrue/printHeaderRequest?" + $.param(params);

    //},
    PrintHeaderRequest: function (params) {

        window.location.href = "/StopAccrue/printHeaderRequest?" + $.param(params);

    },
}

var dateFormat = function () {
    var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
        timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
        timezoneClip = /[^-+\dA-Z]/g,
        pad = function (val, len) {
            val = String(val);
            len = len || 2;
            while (val.length < len) val = "0" + val;
            return val;
        };

    // Regexes and supporting functions are cached through closure
    return function (date, mask, utc) {
        var dF = dateFormat;

        // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
        if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
            mask = date;
            date = undefined;
        }

        // Passing date through Date applies Date.parse, if necessary
        date = date ? new Date(date) : new Date;
        if (isNaN(date)) throw SyntaxError("invalid date");

        mask = String(dF.masks[mask] || mask || dF.masks["default"]);

        // Allow setting the utc argument via the mask
        if (mask.slice(0, 4) == "UTC:") {
            mask = mask.slice(4);
            utc = true;
        }

        var _ = utc ? "getUTC" : "get",
            d = date[_ + "Date"](),
            D = date[_ + "Day"](),
            m = date[_ + "Month"](),
            y = date[_ + "FullYear"](),
            H = date[_ + "Hours"](),
            M = date[_ + "Minutes"](),
            s = date[_ + "Seconds"](),
            L = date[_ + "Milliseconds"](),
            o = utc ? 0 : date.getTimezoneOffset(),
            flags = {
                d: d,
                dd: pad(d),
                ddd: dF.i18n.dayNames[D],
                dddd: dF.i18n.dayNames[D + 7],
                m: m + 1,
                mm: pad(m + 1),
                mmm: dF.i18n.monthNames[m],
                mmmm: dF.i18n.monthNames[m + 12],
                yy: String(y).slice(2),
                yyyy: y,
                h: H % 12 || 12,
                hh: pad(H % 12 || 12),
                H: H,
                HH: pad(H),
                M: M,
                MM: pad(M),
                s: s,
                ss: pad(s),
                l: pad(L, 3),
                L: pad(L > 99 ? Math.round(L / 10) : L),
                t: H < 12 ? "a" : "p",
                tt: H < 12 ? "am" : "pm",
                T: H < 12 ? "A" : "P",
                TT: H < 12 ? "AM" : "PM",
                Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
            };

        return mask.replace(token, function ($0) {
            return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
        });
    };
}();

// Some common format strings
dateFormat.masks = {
    "default": "ddd mmm dd yyyy HH:MM:ss",
    shortDate: "m/d/yy",
    mediumDate: "mmm d, yyyy",
    longDate: "mmmm d, yyyy",
    fullDate: "dddd, mmmm d, yyyy",
    shortTime: "h:MM TT",
    mediumTime: "h:MM:ss TT",
    longTime: "h:MM:ss TT Z",
    isoDate: "yyyy-mm-dd",
    isoTime: "HH:MM:ss",
    isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
    isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
};

// Internationalization strings
dateFormat.i18n = {
    dayNames: [
        "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
        "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
    ],
    monthNames: [
        "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
        "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
    ]
};

// For convenience...
Date.prototype.format = function (mask, utc) {
    return dateFormat(this, mask, utc);
};