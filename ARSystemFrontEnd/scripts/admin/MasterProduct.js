Data = {};

UploadedData = [];

jQuery(document).ready(function () {
    Data.RowSelected = [];
    Data.CurrentSelectedRow = [];
    Data.RowSelectedSubmitted = [];
    window.Parsley.addValidator('period', {
        validateString: function (value) {
            var startDateComponents = Helper.ReverseDateToSQLFormat($("#tbStartLeaseDate").val()).split("/");
            var endDateComponents = Helper.ReverseDateToSQLFormat($("#tbEndLeaseDate").val()).split("/");
            var startDate = new Date(parseInt(startDateComponents[2]), parseInt(startDateComponents[0]) - 1, parseInt(startDateComponents[1]));
            var endDate = new Date(parseInt(endDateComponents[2]), parseInt(endDateComponents[0]) - 1, parseInt(endDateComponents[1]));

            return endDate > startDate;
        },
        messages: {
            en: 'The End Lease Date must be greater than Start Lease Date'
        }
    });

    Control.BindProductTypeList();
    Form.Init();
    Table.Init();
    Table.Search();
    // Initialize Datepicker
    $("body").delegate(".datepicker", "focusin", function () {
        $(this).datepicker({
            format: "dd-M-yyyy"
        });
    });

    //panel Summary
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

    $("#btCreate").unbind().click(function () {
        Form.Create();
    });

    //panel transaction Header
    $("#formProduct").submit(function (e) {
        e.preventDefault();
        if ($("#formProduct").parsley().validate())
        {
            var relatedToSonumb = $("#chRelatedToSonumb").bootstrapSwitch("state");
            if (Data.CurrentSelectedRow.length <= 0 && relatedToSonumb) {
                Common.Alert.Warning("One or more SO Number have to be selected.");
            } else {
                if (Data.Mode == "Create") {
                    var fileName = Helper.GetFileName($("#divAgreementDoc .fileinput-filename").html());
                    if (fileName != "")
                        Product.Post();
                    else
                        Common.Alert.Warning("The Agreement Document must be uploaded.");
                }
                else if (Data.Mode == "Edit")
                    Product.Put();
            }
        }
    });

    $(".btCancel").unbind().click(function () {
        Form.Cancel();
    });

    $("#btBack").unbind().click(function () {
        $("#pnlSummary").fadeIn();
        $("#pnlDetailResult").hide();
    });

    // SO Number Modal Dialog
    $("#btBrowseSoNumber").unbind().click(function () {
        $('#mdlSoNumber').modal('show');
        if(Data.RowSelected.length <= 0)
            $("#btSelectSoNumber").hide();

        if (Data.Mode == "Create" && Data.RowSelectedSubmitted.length <= 0) {
            Data.RowSelected = [];
        }

        TableSoNumber.Reset();
        TableSoNumber.Init();
    });

    $("#btSearchSoNumber").unbind().click(function () {
        TableSoNumber.Init();
    });

    $("#btResetSoNumber").unbind().click(function () {
        TableSoNumber.Reset();
    });

    $("#fuAgreementDoc").fileinput();

    $("#btUpload").on("click", function () {
        UploadProduct.UploadFile();
    });

    $("#btSubmitProducts").on("click", function () {
        UploadProduct.Submit();
    });

    $("#btDownload").on("click", function () {
        UploadProduct.DownloadTemplate();
    });

    $('#tblSoNumber').on('change', 'tbody tr .checkboxes', function () {
        var id = $(this).parent().attr('id');
        if (this.checked) {
            if (Data.Mode == "Edit")
                Data.RowSelectedSubmitted.push(id + "");
            Data.RowSelected.push(id + "");
        } else {
            if (Data.Mode == "Edit")
                Data.RowSelectedSubmitted = Helper.RemoveElementFromArray(Data.RowSelectedSubmitted, id + "");
            Data.RowSelected = Helper.RemoveElementFromArray(Data.RowSelected, id + "");
        }

        if (Data.RowSelected.length > 0) {
            $("#btSelectSoNumber").show();
        } else {
            $("#btSelectSoNumber").hide();
        }
    });

    $("#divSoNumberGrid").hide();
    $("#btSelectSoNumber").on("click", function () {
        TableSoNumber.Select();
    });
    $("#divUploadField").hide();
    $("#divUploadButtons").hide();
});

var Form = {
    Init: function () {
        $("#pnlProduct").hide();
        $("#formProduct").parsley();

        if (!$("#hdAllowAdd").val()) {
            $("#btCreate").hide();
        }

        Control.BindCompanyList();
        Control.BindCustomerTypeList();
        Control.BindCustomerList();
        Control.BindOperatorList();
        $("#chRelatedToSonumb").bootstrapSwitch("disabled", true);

        $("#slProductTypeID").unbind().on("change", function () {
            if (Data.Mode == "Create")
            {
                Data.RowSelected = [];
                Data.RowSelectedSubmitted = [];
            }
            $("#divSoNumberGrid").hide();
            var selectedProductTypeId = $(this).val();
            var components = selectedProductTypeId.split("_");
            var productTypeId = components[0];
            var relatedToSoNumber = (components[1] == "true");

            $("#chRelatedToSonumb").bootstrapSwitch("disabled", false);
            $("#chRelatedToSonumb").bootstrapSwitch("state", relatedToSoNumber);
            $("#chRelatedToSonumb").bootstrapSwitch("disabled", true);
            if (relatedToSoNumber) {
                //if(Data.RowSelected.length > 0)
                //    $("#divSoNumberGrid").show();
                $("#btBrowseSoNumber").removeAttr("disabled");
                $("#icSoNumber").show();
            }
            else {
                $("#divSoNumberGrid").hide();
                $("#btBrowseSoNumber").attr("disabled", "disabled");
                $("#icSoNumber").hide();
            }
        });

        $("#rbOperator").unbind().on("click", function () {
            $("#slCustomerID").val("").trigger("change");
            $("#slOperatorID").val("").trigger("change");
            $("#slCustomerTypeID").val("").trigger("change");
            $("#slOperatorID").attr("required", "required");
            $("#slCustomerID").removeAttr("required");
            $("#divCustomerName").hide();
            $("#divOperatorName").show();
            $("#divCustomerType").hide();
            $("#slCustomerTypeID").removeAttr("required");
        });

        $("#rbNonOperator").unbind().on("click", function () {
            $("#slCustomerID").val("").trigger("change");
            $("#slOperatorID").val("").trigger("change");
            $("#slCustomerTypeID").val("").trigger("change");
            $("#slCustomerID").attr("required", "required");
            $("#slOperatorID").removeAttr("required");
            $("#divCustomerName").show();
            $("#divOperatorName").hide();
            $("#divCustomerType").show();
            $("#slCustomerTypeID").attr("required", "required");
        });

        $("#slCustomerID").unbind().on("change", function () {
            var selectedCustomerId = $(this).val();
            if (selectedCustomerId != null && selectedCustomerId != undefined) {
                var components = selectedCustomerId.split("_");
                var customerTypeId = components[1];

                $("#slCustomerTypeID").val(customerTypeId).trigger("change");
            }
        });

        $("#formProduct").unbind().on('click', '.rbForm', function (e) {
            var radioButton = $(this);
            console.log(radioButton.val());
            if (radioButton.val() == 1) {
                $("#divUploadField").hide();
                $("#divUploadButtons").hide();

                $("#divProductType").show();
                $("#divRelatedToSoNumber").show();
                $("#divSoNumber").show();
                $("#divProductName").show();
                $("#divCompany").show();
                $("#divCustomerCategory").show();
                $("#divCustomerName").show();
                $("#divCustomerType").show();
                $("#divOperatorName").show();
                $("#divStartLeaseDate").show();
                $("#divEndLeaseDate").show();
                $("#divAgreementDoc").show();
                $("#divButtons").show();
            } else {
                $("#divUploadField").show();
                $("#divUploadButtons").show();

                $("#divProductType").hide();
                $("#divRelatedToSoNumber").hide();
                $("#divSoNumber").hide();
                $("#divProductName").hide();
                $("#divCompany").hide();
                $("#divCustomerCategory").hide();
                $("#divCustomerName").hide();
                $("#divCustomerType").hide();
                $("#divOperatorName").hide();
                $("#divStartLeaseDate").hide();
                $("#divEndLeaseDate").hide();
                $("#divAgreementDoc").hide();
                $("#divButtons").hide();
            }
        });

        Table.Reset();
    },
    Create: function () {
        Data.CurrentSelectedRow = [];
        Data.RowSelected = [];
        Data.RowSelectedSubmitted = [];
        Data.Mode = "Create";
        $("#divInputSelection").show();
        $("input[name=rbForm][value=1]").prop('checked', true);
        $("#pnlSummary").hide();
        $("#pnlProduct").fadeIn();
        $(".panelProduct").show();
        $("#panelProductTitle").text("Create Product");
        $("#formProduct").parsley().reset()

        // Show Operator Div and Dropdown
        $("#slOperatorID").val("").trigger("change");
        $("#divOperatorName").show();
        $("#slOperatorID").attr("required", "required");

        // Hide Customer Div and Dropdown
        $("#divCustomerName").hide();
        $("#divCustomerType").hide();
        $("#slCustomerID").val("").trigger("change");
        $("#slCustomerID").removeAttr("required");
        $("#slCustomerTypeID").removeAttr("required");

        $("#slProductTypeID").val("").trigger("change");
        $("#chRelatedToSonumb").bootstrapSwitch("state", false);

        $("#hfSoNumber").val("");
        $("#btBrowseSoNumber").attr("disabled", "disabled");
        $("#icSoNumber").hide();

        $("#slCompanyID").val("").trigger("change");
        $("#tbProductName").val("");
        $("#rbOperator").attr("checked", "checked");
        $("#tbStartLeaseDate").val("");
        $("#tbEndLeaseDate").val("");

        $(".fileinput").fileinput("clear");
        $("#fuAgreementDoc").attr("required", "required");
        $("#icAgreementDoc").show();

        $("#divUploadField").hide();
        $("#divUploadButtons").hide();
        $("#divDownloadAgreementDoc").hide();

        $("#divProductType").show();
        $("#divRelatedToSoNumber").show();
        $("#divSoNumber").show();
        $("#divProductName").show();
        $("#divCompany").show();
        $("#divCustomerCategory").show();
        $("#divStartLeaseDate").show();
        $("#divEndLeaseDate").show();
        $("#divAgreementDoc").show();
        $("#divButtons").show();
    },
    Edit: function () {
        $("#divInputSelection").hide();
        $("#divUploadField").hide();
        $("#divUploadButtons").hide();
        Data.Mode = "Edit";
        $("#pnlSummary").hide();
        $("#pnlProduct").fadeIn();
        $(".panelProduct").show();
        $("#panelProductTitle").text("Edit Product");
        $("#formProduct").parsley().reset();

        $("#divProductType").show();
        $("#divRelatedToSoNumber").show();
        $("#divSoNumber").show();
        $("#divProductName").show();
        $("#divCompany").show();
        $("#divCustomerCategory").show();
        $("#divStartLeaseDate").show();
        $("#divEndLeaseDate").show();
        $("#divAgreementDoc").show();
        $("#divButtons").show();

        // Show / Hide Download Button
        if (Data.Selected.AgreementDoc != null && Data.Selected.AgreementDoc != undefined && Data.Selected.AgreementDoc != "") {
            var iconDownload = "<i class='fa fa-download'></i> ";
            $("#btDownloadAgreementDoc").html(iconDownload);
            $("#btDownloadAgreementDoc").append(Data.Selected.AgreementDoc);
            $("#divDownloadAgreementDoc").show();

            $("#btDownloadAgreementDoc").unbind().on("click", function (e) {
                var docPath = $("#hfDocPath").val();
                var path = docPath + Data.Selected.FilePath;
                var fileName = Data.Selected.AgreementDoc;
                var contentType = Data.Selected.ContentType;
                window.location.href = "/Admin/Download?path=" + path + "&fileName=" + fileName + "&contentType=" + contentType;
            });
        }
        else
            $("#divDownloadAgreementDoc").hide();

        if (Data.Selected.ProjectInformations.length > 0) {
            Data.CurrentSelectedRow = Data.Selected.ProjectInformations;
            Data.RowSelected = [];
            Data.RowSelectedSubmitted = [];
            $.each(Data.CurrentSelectedRow, function (index, item) {
                Data.RowSelected.push(item.ProjectInformationID + "");
                Data.RowSelectedSubmitted.push(item.ProjectInformationID + "");
            });
        } else {
            Data.CurrentSelectedRow = [];
            Data.RowSelected = [];
        }

        $("#tbProductName").val(Data.Selected.ProductName);
        $("#slCompanyID").val(Data.Selected.CompanyID).trigger("change");
        $("#slProductTypeID").val(Data.Selected.ProductTypeId + "_" + Data.Selected.RelatedToSonumb).trigger("change");

        if (Data.Selected.ProjectInformations.length > 0) {
            $("#divSoNumberGrid").show();
            TableSelectedSoNumber.Init(Data.Selected.ProjectInformations);
        } else {
            TableSelectedSoNumber.Init([]);
            $("#divSoNumberGrid").hide();
        }

        $("#chRelatedToSonumb").bootstrapSwitch("state", Data.Selected.RelatedToSonumb);

        var isOperator = (Data.Selected.IsOperator) ? 1 : 0;
        $("input[name=rbIsOperator][value=" + isOperator + "]").prop('checked', true);

        if (!Data.Selected.IsOperator) {
            $("#slCustomerID").val(Data.Selected.CustomerID + "_" + Data.Selected.CustomerTypeId).trigger("change");
            $("#divCustomerName").show();
            $("#divCustomerType").show();
            $("#slCustomerID").attr("required", "required");
            $("#slCustomerTypeID").attr("required", "required");
            $("#divOperatorName").hide();
            $("#slOperatorID").removeAttr("required");
        } else {
            $("#slOperatorID").val(Data.Selected.OperatorID.trim() + "_" + Data.Selected.OperatorCode).trigger("change");
            $("#divCustomerName").hide();
            $("#divCustomerType").hide();
            $("#slCustomerID").removeAttr("required");
            $("#slCustomerTypeID").removeAttr("required");
            $("#divOperatorName").show();
            $("#slOperatorID").attr("required", "required");
        }
        $("#tbStartLeaseDate").val(Common.Format.ConvertJSONDateTime(Data.Selected.StartLeaseDate));
        $("#tbEndLeaseDate").val(Common.Format.ConvertJSONDateTime(Data.Selected.EndLeaseDate));

        $(".fileinput").fileinput("clear");
        $("#fuAgreementDoc").removeAttr("required");
        $("#icAgreementDoc").hide();
    },
    Cancel: function () {
        if (Data.Mode == "Create")
            Data.RowSelected = [];
        $("#pnlSummary").fadeIn();
        $("#pnlProduct").hide();
    },
    Done: function () {
        $("#pnlSummary").fadeIn();
        $("#pnlProduct").hide();
        //$(".panelSearchBegin").fadeIn();
        //$(".panelSearchZero").hide();
        $(".panelSearchResult").fadeIn();
        Table.Search();
    }
}

var Table = {
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
                Form.Edit();
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
            ProductTypeId: $("#slSearchProductType").val(),
            ProductName: $("#tbSearchProductName").val(),
            IsOperator: $('input[name=rbCustomerCategory]:checked').val(),
            ProductId: "0"
        };
        var tblSummaryData = $("#tblSummaryData").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/Product/grid",
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
                        Table.Export()
                        l.stop();
                    }
                },
                { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "filter": false,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        //strReturn += "<button type='button' title='Detail' class='btn green btDetail'><i class='fa fa-search-plus'></i></button>";
                        if ($("#hdAllowEdit").val())
                            strReturn += "<button type='button' title='Edit' class='btn btn-xs green btEdit'><i class='fa fa-edit'></i></button>";
                        return strReturn;
                    }
                },
                { data: "ProductType" },
                { data: "ProductName" },
                {
                    mRender: function (data, type, full) {
                        return full.RelatedToSonumb ? "Yes" : "No";
                    }
                },
                { data: "CustomerCategory" },
                { data: "CustomerType" },
                { data: "CustomerName" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop();
            },
            "order": [[1, "asc"]]
        });
    },
    Reset: function () {
        $("#tbSearchProductName").val("");
        $("#slSearchProductType").val("").trigger('change');
        $("#rbSearchAll").prop("checked", true);
    },
    Export: function () {
        var productTypeId = $("#slSearchProductType").val();
        var productName = $("#tbSearchProductName").val();
        var isOperator = $('input[name=rbCustomerCategory]:checked').val();

        if (productTypeId == null || productTypeId == "" || productTypeId == undefined)
            productTypeId = 0;

        window.location.href = "/Admin/Product/Export?productTypeId=" + productTypeId + "&productName=" + productName + "&isOperator=" + isOperator;
    }
}

var Product = {
    Post: function () {
        var operatorCode = null;
        var operatorID = null;
        var customerID = null;
        var isOperator = $("input[name=rbIsOperator]:checked").val() == "1";

        if (isOperator) {
            operatorCode = Helper.GetOperatorCode($("#slOperatorID").val());
            operatorID = Helper.GetOperatorId($("#slOperatorID").val());
        } else {
            customerID = Helper.GetCustomerId($("#slCustomerID").val());
        }

        var projectInformationID = null;
        var soNumber = null;
        var relatedToSoNumber = $("#chRelatedToSonumb").bootstrapSwitch("state");

        var projectInformations = Data.CurrentSelectedRow;
        var fileInput = document.getElementById("fuAgreementDoc");
        var file = fileInput.files[0];
        var formData = new FormData();
        var projectInformationIDs = "";
        if (!relatedToSoNumber) {
            projectInformations = [];
        } else {
            $.each(projectInformations, function (index, item) {
                projectInformationIDs += item.ProjectInformationID + ",";
            });
            formData.append("ProjectInformations", projectInformationIDs.substr(0, projectInformationIDs.length - 1));
        }

        formData.append("ProductName", $("#tbProductName").val());
        formData.append("ProductTypeId", Helper.GetProductTypeId($("#slProductTypeID").val()));
        formData.append("CompanyID", $("#slCompanyID").val());
        formData.append("IsOperator", isOperator);
        formData.append("CustomerID", customerID);
        formData.append("OperatorID", operatorID);
        formData.append("OperatorCode", operatorCode);
        formData.append("StartLeaseDate", Helper.ReverseDateToSQLFormat($("#tbStartLeaseDate").val()));
        formData.append("EndLeaseDate", Helper.ReverseDateToSQLFormat($("#tbEndLeaseDate").val()));
        formData.append("File", file);

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/Product",
            type: "POST",
            dataType: "json",
            contentType: false,
            data: formData,
            processData: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Master Product has been created!");
                Table.Reset();
                Form.Done();
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })
    },
    Put: function () {
        var operatorCode = null;
        var operatorID = null;
        var customerID = null;
        var isOperator = $("input[name=rbIsOperator]:checked").val() == "1";

        if (isOperator) {
            operatorCode = Helper.GetOperatorCode($("#slOperatorID").val());
            operatorID = Helper.GetOperatorId($("#slOperatorID").val());
        } else {
            customerID = Helper.GetCustomerId($("#slCustomerID").val());
        }

        var projectInformationID = null;
        var soNumber = null;
        var projectInformations = Data.CurrentSelectedRow;
        var temp = null;

        var relatedToSoNumber = $("#chRelatedToSonumb").bootstrapSwitch("state");

        var fileInput = document.getElementById("fuAgreementDoc");
        var file = null;
        if (fileInput.files != undefined) {
            file = fileInput.files[0];
        }

        var formData = new FormData();
        var projectInformationIDs = "";
        if (!relatedToSoNumber) {
            projectInformations = [];
        } else {
            $.each(projectInformations, function (index, item) {
                projectInformationIDs += item.ProjectInformationID + ",";
            });
            formData.append("ProjectInformations", projectInformationIDs.substr(0, projectInformationIDs.length - 1));
        }
        formData.append("ProductName", $("#tbProductName").val());
        formData.append("ProductTypeId", Helper.GetProductTypeId($("#slProductTypeID").val()));
        formData.append("CompanyID", $("#slCompanyID").val());
        formData.append("IsOperator", isOperator);
        formData.append("CustomerID", customerID);
        formData.append("OperatorID", operatorID);
        formData.append("OperatorCode", operatorCode);
        formData.append("StartLeaseDate", Helper.ReverseDateToSQLFormat($("#tbStartLeaseDate").val()));
        formData.append("EndLeaseDate", Helper.ReverseDateToSQLFormat($("#tbEndLeaseDate").val()));

        if(file != undefined && file != null)
            formData.append("File", file);

        var l = Ladda.create(document.querySelector("#btSubmit"))
        $.ajax({
            url: "/api/Product/" + Data.Selected.ProductId,
            type: "PUT",
            dataType: "json",
            contentType: false,
            data: formData,
            processData: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Common.Alert.Success("Master Product has been updated!");
                Table.Reset();
                Form.Done();
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
    BindProductTypeList: function () {
        $.ajax({
            url: "/api/MstProductType?isActive=1",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slSearchProductType").html("<option></option>");
            $("#slProductTypeID").html("<option></option>");
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null) {
                        $("#slSearchProductType").append("<option value='" + item.ProductTypeId + "'>" + item.ProductType + "</option>");
                        $("#slProductTypeID").append("<option value='" + item.ProductTypeId + "_" + item.RelatedToSonumb + "'>" + item.ProductType + "</option>");
                    }
                });
            }
            $("#slSearchProductType").select2({ placeholder: "Select Product Type", width: null });
            $("#slProductTypeID").select2({ placeholder: "Select Product Type", width: null });
        });
    },
    BindOperatorList: function () {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slOperatorID").html("<option></option>");
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null)
                        $("#slOperatorID").append("<option value='"+ item.OperatorId.trim() +"_"+ item.OperatorCode +"'>"+ item.OperatorCode +" - "+ item.Operator +"</option>");
                })
            }
            $("#slOperatorID").select2({ placeholder: "Select Operator", width: null });
        });
    },
    BindCustomerList: function () {
        $.ajax({
            url: "/api/MstDataSource/Customer",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slCustomerID").html("<option></option>");
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null)
                        $("#slCustomerID").append("<option value='" + item.CustomerID + "_"+ item.CustomerTypeID +"'>" + item.CustomerName + "</option>");
                })
            }
            $("#slCustomerID").select2({ placeholder: "Select Customer", width: null });
        });
    },
    BindCompanyList: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slCompanyID").html("<option></option>");
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null)
                        $("#slCompanyID").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
            }
            $("#slCompanyID").select2({ placeholder: "Select Company", width: null });
        });
    },
    BindCustomerTypeList: function () {
        $.ajax({
            url: "/api/MstDataSource/CustomerType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slCustomerTypeID").html("<option></option>");
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null)
                        $("#slCustomerTypeID").append("<option value='" + item.CustomerTypeId + "'>" + item.CustomerType + "</option>");
                })
            }
            $("#slCustomerTypeID").select2({ placeholder: "Select Customer Type", width: null });
        });
    }
}

var Helper = {
    GetProductTypeId: function (selectedProductTypeId) {
        var productTypeId = null;
        if (selectedProductTypeId != null) {
            var components = selectedProductTypeId.split("_");
            productTypeId = components[0];
        }
        return productTypeId;
    },
    GetCustomerId: function (selectedCustomerId) {
        var customerId = null;
        if (selectedCustomerId != null) {
            var components = selectedCustomerId.split("_");
            customerId = components[0];
        }
        return customerId;
    },
    GetOperatorCode: function (selectedOperatorId) {
        var operatorCode = null;
        if (selectedOperatorId != null) {
            var components = selectedOperatorId.split("_");
            operatorCode = components[1];
        }
        return operatorCode;
    },
    GetOperatorId: function (selectedOperatorId) {
        var operatorId = null;
        if (selectedOperatorId != null) {
            var components = selectedOperatorId.split("_");
            operatorId = components[0].trim();
        }
        return operatorId;
    },
    ReverseDateToSQLFormat: function (dateValue) {
        var dateComponents = dateValue.split("-");
        var dateValue = dateComponents[0];
        var monthValue = dateComponents[1];
        var yearValue = dateComponents[2];
        var allMonths = "JanFebMarAprMayJunJulAugSepOctNovDec";

        var monthNumberValue = allMonths.indexOf(monthValue) / 3 + 1;

        return monthNumberValue + "/" + dateValue + "/" + yearValue;
    },
    GetFileName: function (fullFileName) {
        var fileName = "";
        if(fullFileName != undefined)
            fileName = fullFileName.split(/(\\|\/)/g).pop();

        return fileName;
    },
    GetSoNumber: function (strSoNumber) {
        var components = strSoNumber.split(" ");
        return components[0];
    },
    ExtractDotNetJSONDateValue: function (value) {
        if (value === null) return "";

        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));

        return ((parseInt(dt.getMonth() + 1) < 10) ? "0" + (dt.getMonth() + 1) : dt.getMonth() + 1) + "/" + ((parseInt(dt.getDate()) < 10) ? "0" + dt.getDate() : dt.getDate()) + "/" + dt.getFullYear();
    },
    DoesElementExistInArray: function (value, arr) {
        var isExist = false;
        for (var i = 0 ; i < arr.length ; i++) {
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
}

/*
SO Number Picker
*/
var TableSoNumber = {
    Init: function () {
        var l = Ladda.create(document.querySelector("#btBrowseSoNumber"))
        l.start();

        // Initialize filter parameters
        var params = {
            SoNumber: $("#tbSearchSoNumberModal").val(),
            SiteID: $("#tbSearchSiteID").val(),
            SiteName: $("#tbSearchSiteName").val(),
            RegionalName: $("#tbSearchRegionalName").val()
        }

        // Initialize datatable
        var tblSummaryData = $("#tblSoNumber").DataTable({
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/api/ProjectInformation/grid",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        
                        if (Data.RowSelectedSubmitted.length > 0) {
                            if (Helper.DoesElementExistInArray(full.ProjectInformationID + "", Data.RowSelectedSubmitted)) {
                                strReturn += "<label id='" + full.ProjectInformationID + "' sonumber='" + full.SoNumber + "' sitename='" + full.SiteName + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                            } else {
                                strReturn += "<label id='" + full.ProjectInformationID + "' sonumber='" + full.SoNumber + "' sitename='" + full.SiteName + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }
                        } else {
                            if (Helper.DoesElementExistInArray(full.ProjectInformationID + "", Data.RowSelected)) {
                                strReturn += "<label id='" + full.ProjectInformationID + "' sonumber='" + full.SoNumber + "' sitename='" + full.SiteName + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' checked class='checkboxes' /><span></span></label>";
                            } else {
                                strReturn += "<label id='" + full.ProjectInformationID + "' sonumber='" + full.SoNumber + "' sitename='" + full.SiteName + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' class='checkboxes' /><span></span></label>";
                            }
                        }
                        return strReturn;
                    }
                },
                { data: "SoNumber" },
                { data: "SiteID" },
                { data: "SiteName" },
                { data: "RegionalName" }
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblSummaryData.data())) {
                    if (this.fnSettings().fnRecordsTotal() > 0) {
                        $(".panelSearchBegin").hide();
                        $(".panelSearchZero").hide();
                        $(".panelSearchResult").fadeIn();
                    }
                    else {
                        $(".panelSearchBegin").hide();
                        $(".panelSearchZero").fadeIn();
                        $(".panelSearchResult").hide();
                    }
                }
                l.stop();
            },
            "order": [[1, "asc"]]
        });
    },
    Reset: function () {
        $("#tbSearchSoNumberModal").val("");
        $("#tbSearchSiteID").val("");
        $("#tbSearchSiteName").val("");
        $("#tbSearchRegionalName").val("");
    },
    Select: function () {
        Data.RowSelectedSubmitted = [];
        $.each(Data.RowSelected, function (index, item) {
            Data.RowSelectedSubmitted.push(item);
        });
        var params = {
            listID: Data.RowSelectedSubmitted
        };
        var l = Ladda.create(document.querySelector("#btSelectSoNumber"));
        $.ajax({
            url: "/api/ProjectInformation/ListByID",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            $("#mdlSoNumber").modal("hide");
            TableSelectedSoNumber.Init(data);
            Data.CurrentSelectedRow = data;
            l.stop();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $("#mdlSoNumber").modal("hide");
            Common.Alert.Error(errorThrown)
            l.stop();
        });
    }
}

var TableSelectedSoNumber = {
    Init: function (data) {
        var oTable = $("#tblSelectedSoNumber").DataTable({
            "data": data,
            "pageLength": 5,
            "lengthMenu": [[-1, 5, 10, 25, 50], ['All', '5', '10', '25', '50']],
            "columnDefs": [{
                'orderable': false,
                'targets': []
            }, {
                "searchable": true,
                "targets": [0, 1, 2]
            }],
            "destroy": true,
            "columns": [
                { data: "SoNumber" },
                { data: "SiteName" },
                { data: "RegionalName" },
            ]
        });
        $("#divSoNumberGrid").show();
    }
}

/*
Upload Excel Product
*/
var UploadProduct = {
    UploadFile: function () {
        var formData = new FormData(); //FormData object  
        var fileInput = document.getElementById("fuProduct");
        if (fileInput.files[0] != undefined && fileInput.files[0] != null)
        {
            var fileName = fileInput.files[0].name;
            var extension = fileName.split('.').pop().toUpperCase();
            if (extension != "XLS" && extension != "XLSX") {
                Common.Alert.Warning("Please upload an Excel File.");
            }
            else {
                for (i = 0; i < fileInput.files.length; i++) {
                    formData.append(fileInput.files[i].name, fileInput.files[i]);
                }

                var l = Ladda.create(document.querySelector("#btUpload"));
                $.ajax({
                    url: '/Admin/Product/ImportExcel',
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
                            $(".fileinput").fileinput("clear");
                            Common.Alert.Warning("The uploaded file is invalid or empty. Please use the available template, and fill the file correctly with valid product data.");
                        } else {
                            $(".fileinput").fileinput("clear");
                            UploadedData = data;
                            $('#mdlUploadedProduct').modal('show');
                            $("#mdlUploadedProduct tbody").html("");
                            $("#ulErrors").html("");
                            UploadProduct.DrawTable(data);
                        }
                    } else {
                        $(".fileinput").fileinput("clear");
                        Common.Alert.Warning("The uploaded file is invalid. Please download the latest template by clicking the Download Template button.");
                    }
                    l.stop();
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    Common.Alert.Error(errorThrown)
                    l.stop()
                });
            }
        } else {
            Common.Alert.Warning("Please upload an Excel File.");
        }
    },
    DownloadTemplate: function () {
        window.location.href = "/Admin/Product/DownloadExcel";
    },
    Submit: function () {
        var params = [];
        var param = {}

        $.each(UploadedData, function (index, item) {
            param = new Object();
            param = {
                ProductName: item.ProductName,
                ProductTypeID: item.ProductTypeId,
                CompanyID: item.CompanyID,
                IsOperator: item.IsOperator,
                CustomerID: item.CustomerID,
                OperatorID: item.OperatorID,
                OperatorCode: item.OperatorCode,
                SoNumber: item.SoNumber,
                ProjectInformationID: item.ProjectInformationID,
                StartLeaseDate: item.StartLeaseDate,
                EndLeaseDate: item.EndLeaseDate
            }
            params.push(param);
        });

        var l = Ladda.create(document.querySelector("#btSubmitProducts"));
        $.ajax({
            url: "/api/Product/BulkCreate",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            $('#mdlUploadedProduct').modal('hide');
            Common.Alert.Success("Products have been created successfully!");
            Table.Reset();
            Form.Done();
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $('#mdlUploadedProduct').modal('hide');
            Common.Alert.Error(errorThrown)
            l.stop()
        });
    },
    DrawTable: function (data) {
        var html = "";
        var errorHtml = "";
        var anyErrors = false;
        UploadProduct.Table.Init();
        $.each(data, function (index, item) {
            errorHtml = "";
            if (item.IsError) {
                $.each(item.ErrorMessages, function (idx, message) {
                    errorHtml += "<li>" + message + "</li>";
                });
                $("#ulErrors").append(errorHtml);
                anyErrors = true;
            }
        });
        //$("#tblUploadedProduct").append(html);

        if (anyErrors)
            $("#btSubmitProducts").hide();
        else
            $("#btSubmitProducts").show();
    },
    Table: {
        Init: function () {
            var oTable = $("#tblUploadedProduct").DataTable({
                "data": UploadedData,
                "pageLength": 10,
                "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
                "columnDefs": [{
                    'orderable': false,
                    'targets': [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                }, {
                    "searchable": true,
                    "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
                }],
                "destroy": true,
                "columns": [
                    { data: "ProductType" },
                    { data: "ProductName" },
                    {
                        data: "RelatedToSonumb", render: function (value) {
                            if (value)
                                return "Yes";
                            else
                                return "No";
                        }
                    },
                    { data: "SoNumber" },
                    { data: "Company" },
                    {
                        data: "IsOperator", render: function (value) {
                            if (value)
                                return "Yes";
                            else
                                return "No";
                        }
                    },
                    { data: "Operator" },
                    { data: "CustomerName" },
                    {
                        data: "StartLeaseDate", render: function (value) {
                            return Common.Format.ConvertJSONDateTime(Helper.ExtractDotNetJSONDateValue(value));
                        }
                    },
                    {
                        data: "EndLeaseDate", render: function (value) {
                            return Common.Format.ConvertJSONDateTime(Helper.ExtractDotNetJSONDateValue(value));
                        }
                    },
                    { data: "IsError", visible: false }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    if (aData.IsError) {
                        $('td', nRow).attr('style', 'background-color: #FF9999 !important;');
                    } else {
                        $('td', nRow).removeAttr('style');
                    }
                },
                "order": []
            });
        }
    }
}