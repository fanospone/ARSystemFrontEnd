Data = {};
Data.RowSelected = {};
var fsCustomerID = "";
var fsCompanyID = "";
var fsStartBapsDate = "";
var fsEndBapsDate = "";
var fsBaseLease = "";
var fsInflation = "";
var fsServicePrice = "";
var fsPOType = "";
var fsBaseLeaseCurr = "";
var fsServiceCurr = "";


jQuery(document).ready(function () {
    Helper.Init();
    Table.Init("#tbllReconcileReject");
    Table.Grid();

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

    Grid: function () {

        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
        var idTB = "#tbllReconcileReject";
        var params = {
            strReconcileID: $("#sCustomerID").val(),
            strBAPSTypeID: $("#sBAPSTypeID").val(),
            strProductID: $("#sProductID").val(),
            strYear: $("#sYear").val(),
            strSTIPID: $("#sSTIPID").val(),
            strSONumber: $("#sSONumber").val(),
            strSiteID: $("#sSiteID").val(),
            strInvoiceTypeID: $("#sQuarterly").val(),
            strPowerTypeID: $("#sPowerTypeID").val(),
            strCompanyID: $("#sCompanyID").val(),
            strCustomerID: $("#sCustomerID").val(),
        };
        var tblList = $(idTB).DataTable({
            "deferRender": true,
            "serverSide": true,
            "proccessing": true,
            "async":false,
            "language": {
                "emptyTable": "No data available in table",
                "processing": "<i class='fa fa-spinner'></i><span class='sr-only'>Loading...</span>}"
            },
            "ajax": {
                "url": "/api/ReconcileReject/grid",
                "type": "POST",
                "data": params,
                "cache": false,
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
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "destroy": true,
            "columns": [
                { data: "RowIndex" }, //0
                {
                    orderable: false,
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<i class='fa fa-edit btn btn-xs green link-EditData' title='Edit Data'></i>&nbsp;&nbsp;";
                        return strReturn;
                    } // 1
                },
                { data: "SoNumber" }, //2
                { data: "CustomerInvID" },//3
                { data: "CompanyInvoiceId" },//4
                { data: "SiteID" },//5
                { data: "SiteName" }, //6
                { data: "CustomerSiteID" },//7
                { data: "CustomerSiteName" },//8
                { data: "Term" },//9
                {
                    data: "StartInvoiceDate", render: function (data) {
                        return Common.Format.ConvertJSONDateTime(data);
                    }
                },//10
                  {
                      data: "EndInvoiceDate", render: function (data) {
                          return Common.Format.ConvertJSONDateTime(data);
                      }
                  }, //11
                  {
                      data: "BaseLeasePrice", render: function (data) {
                          return Common.Format.CommaSeparation(data);
                      }
                  }, //12
                  {
                      data: "ServicePrice", render: function (data) {
                          return Common.Format.CommaSeparation(data);
                      }
                  }, //13
                  {
                      data: "AmountIDR", render: function (data) {
                          return Common.Format.CommaSeparation(data);
                      }
                  }, //14
                   {
                       data: "AmountUSD", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                       }
                   }, //15
                  { data: "ActivityName" }, //16
                { data: "ReconcileID" }, //17
                { data: "CompanyInvoiceId" }, //18
                { data: "AdditionalAmount" }, //19
                { data: "InflationAmount" }, //20
                { data: "DeductionAmount" }, //21
                { data: "StartBapsDate" }, //22
                { data: "EndBapsDate" }, //23
                { data: "BAPSNumber" }, //24
                { data: "BapsType" }, //25
                { data: "InvoiceType" }, //26
                { data: "PowerType" }, //27
                { data: "ActivityLabel" }, //28
                { data: "PoType" }, //29
                { data: "PODtlID" }, //30
                { data: "BaseLeaseCurrency" }, //31
                { data: "ServiceCurrency" } //32

            ],
            "columnDefs": [{ "targets": [0], "orderable": false }, { "targets": [1], "class": "text-center" }, { "targets": [17, 18, 19, 20, 21, 22, 23,24, 25, 26, 27, 28, 30, 31, 32], "visible": false }],
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
                    for (var i = 0 ; i < Data.RowSelected.length; i++) {
                        item = Data.RowSelected[i];
                        if (!Helper.IsElementExistsInArray(parseInt(item), Data.RowSelectedSite))
                            $("#Row" + item).addClass("active");
                    }
                }
            },
            "order": [1, "asc"],
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
        $(idTB + " tbody").unbind();
        $(idTB + " tbody").on("click", ".link-EditData", function (e) {
            var table = $(idTB).DataTable();
            var row = table.row($(this).parents('tr')).data();
            $("#iTrxID").val(row.ReconcileID);
            $("#iBaseLease").val(Common.Format.CommaSeparation(row.BaseLeasePrice));
            $("#iServicePrice").val(Common.Format.CommaSeparation(row.ServicePrice));
            $("#iAddAmount").val(Common.Format.CommaSeparation(row.AdditionalAmount));
            $("#iInflasiAmount").val(Common.Format.CommaSeparation(row.InflationAmount));
            $("#iDeductAmount").val(Common.Format.CommaSeparation(row.DeductionAmount));
            $("#iCompanyInvoice").val($.trim(row.CompanyInvoiceId)).trigger('change');
            $("#lbSisteID").text(row.SiteID);
            $("#lbSisteName").text(row.SiteName);
            $("#lbStartPeriod").text(Common.Format.ConvertJSONDateTime(row.StartBapsDate));
            $("#lbEndPeriod").text(Common.Format.ConvertJSONDateTime(row.EndBapsDate));
            $("#lbSONumber").text(row.SoNumber);
            $("#lbBAPSNumber").text(row.BAPSNumber);
            $("#lbBapsType").text(row.BapsType);
            $("#lbInvoiceType").text(row.InvoiceType);
            $("#lbPowerType").text(row.PowerType);
            $("#lbTerm").text(row.Term);
            $("#lbStartinvDate").text(Common.Format.ConvertJSONDateTime(row.StartInvoiceDate));
            $("#lbEndInvDate").text(Common.Format.ConvertJSONDateTime(row.EndInvoiceDate));
            $("#iSiteNameCust").val(row.CustomerSiteName);
            $("#iSiteIDCust").val(row.CustomerSiteID);
            $("#iPOType").val(row.PoType);
            $("#iPODtlID").val(row.PODtlID);
            $("#iInvoiceAmountIDR").val(Common.Format.CommaSeparation(row.AmountIDR));
            $("#iInvoiceAmountUSD").val(Common.Format.CommaSeparation(row.AmountUSD));

            fsStartBapsDate = row.StartInvoiceDate;
            fsEndBapsDate = row.EndInvoiceDate;
            fsPOType = row.PoType;
            fsCustomerID = row.CustomerInvID;
            fsCompanyID = row.CompanyInvoiceId;
            fsBaseLeaseCurr = row.BaseLeaseCurrency;
            fsServiceCurr = row.ServiceCurrency
            $("#spBaseLeaseCurr").text(row.BaseLeaseCurrency);
            $("#spServiceCurr").text(row.ServiceCurrency);

            Helper.ProrateAmount(fsCustomerID, fsStartBapsDate, fsEndBapsDate);

            if (row.PoType.toUpperCase() == "BASELEASE") {
                $("#iBaseLease").attr("readonly", false);
                $("#iServicePrice").attr("readonly", "readonly");
                $("#iInflasiAmount").attr("readonly", "readonly");
            } else if (row.PoType.toUpperCase() == "SERVICE") {
                $("#iBaseLease").attr("readonly", "readonly");
                $("#iServicePrice").attr("readonly", false);
                $("#iInflasiAmount").attr("readonly", "readonly");
            } else if (row.PoType.toUpperCase() == "INFLATION") {
                $("#iBaseLease").attr("readonly", "readonly");
                $("#iServicePrice").attr("readonly", "readonly");
                $("#iInflasiAmount").attr("readonly", false);
            }

            if (row.ActivityLabel != "BAPS_RETURN")
                $("#btnSaveTrx").attr("disabled", "disabled");
            else
                $("#btnSaveTrx").attr("disabled", false);
            $("#mdlDetaiReconcileRejectNonTSEL").modal("show");
        });
    },


    Export: function () {
        var strBAPSTypeID = $("#sBAPSTypeID").val() == "" ? 0 : $("#sBAPSTypeID").val();
        var strProductID = $("#sProductID").val() == "" ? 0 : $("#sProductID").val();
        var strYear = $("#sYear").val() == "" ? 0 : $("#sYear").val();
        var strSTIPID = $("#sSTIPID").val() == "" ? 0 : $("#sSTIPID").val();
        var strSONumber = $("#sSONumber").val();
        var strSiteID = $("#sSiteID").val();
        var strInvoiceTypeID = $("#sQuarterly").val();
        var strPowerTypeID = $("#sPowerTypeID").val() == "" ? 0 : $("#sPowerTypeID").val();
        var strCompanyID = $("#sCompanyID").val();
        var strCustomerID = $("#sCustomerID").val();
        window.location.href = "/RevenueAssurance/ReconcileRejectNonTSEL/Export?strBAPSTypeID=" + strBAPSTypeID + "&strProductID=" + strProductID
           + "&strYear=" + strYear + "&strSTIPID=" + strSTIPID + "&strSONumber=" + strSONumber + "&strSiteID=" + strSiteID
           + "&strInvoiceTypeID=" + strInvoiceTypeID + "&strPowerTypeID=" + strPowerTypeID + "&strCompanyID=" + strCompanyID + "&strCustomerID=" + strCustomerID;
    },
}

var Form = {

    Init: function () {

    },

    Update: function () {

        Data.ID = $("#iTrxID").val();
        Data.CustomerSiteID = $("#iSiteIDCust").val();
        Data.CustomerSiteName = $("#iSiteNameCust").val();
        Data.CompanyInvoice = $("#iCompanyInvoice").val();
        Data.BaseLeasePrice = $("#iBaseLease").val();
        Data.ServicePrice = $("#iServicePrice").val();
        Data.AmountIDR = $("#iInvoiceAmountIDR").val();
        Data.AmountUSD = $("#iInvoiceAmountUSD").val();
        Data.AdditionalAmount = $("#iAddAmount").val();
        Data.DeductionAmount = $("#iDeductAmount").val();
        Data.InflationAmount = $("#iInflasiAmount").val();

        var params = { trxReconcile: Data, strCustomerID: fsCustomerID, strCompanyID: fsCompanyID, strPOType: $("#iPOType").val(), strPODtlID: $("#iPODtlID").val() };
        var l = Ladda.create(document.querySelector("#btnSaveTrx"))
        l.start();
        $.ajax({
            url: "/api/ReconcileReject/update",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            async: false,
            cache: false,
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                if ((data.ErrorMessage == "" || data.ErrorMessage == null)) {
                    l.stop();
                    Common.Alert.Success("Data Success Saved!");
                    Table.Init("#tbllReconcileReject");
                    Table.Grid();
                } else {
                    Common.Alert.Warning(data.ErrorMessage);
                }
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

}

var Control = {
    BindYear: function () {
        var start_year = new Date().getFullYear();
        var id = "#sYear";
        $(id).html("<option value='0'> All </option>")

        for (var i = start_year - 10; i < start_year ; i++) {
            $(id).append('<option value="' + i + '">  ' + i + '  </option>');
        }

        for (var i = start_year ; i < start_year + 20 ; i++) {
            $(id).append('<option value="' + i + '">  ' + i + '  </option>');
        }

        $(id).select2({ placeholder: "Select Year", width: null });
    },
    BindCustomer: function () {
        var id = "#sCustomerID";
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.OperatorId) + "'>" + item.Operator + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Customer", width: null });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });

    },
    BindCompany: function () {
        var id = "#sCompanyID";
        var id2 = "#iCompanyInvoice";
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>");
            $(id2).html("<option></option>");
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.CompanyId) + "'>" + item.Company + "</option>");
                    $(id2).append("<option value='" + $.trim(item.CompanyId) + "'>" + item.Company + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Company Name", width: null });
            $(id2).select2({ placeholder: "Select Company Name", width: null });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindSTIPCategory: function () {
        var id = "#sSTIPID";
        $.ajax({
            url: "/api/MstDataSource/TBGSys_Stip",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.Value) + "'>" + item.Text + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select STIP", width: null });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindQuarterly: function () {
        var id = "#sQuarterly";
        $.ajax({
            url: "/api/MstDataSource/InvoiceType",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.mstInvoiceTypeId) + "'>" + item.Description + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Quarter", width: null });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindProduct: function () {
        var id = "#sProductID";
        $.ajax({
            url: "/api/MstDataSource/TenantType",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.Value) + "'>" + item.Text + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Product", width: null });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindBAPSType: function () {
        var id = "#sBAPSTypeID";
        $.ajax({
            url: "/api/MstDataSource/BapsType",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.mstBapsTypeId) + "'>" + item.BapsType + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select BAPS Type", width: null });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindPowerType: function () {
        var id = "#sPowerTypeID";
        $.ajax({
            url: "/api/MstDataSource/PowerType",
            type: "GET",
            async: false
        }).done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.KodeType) + "'>" + item.PowerType + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select BAPS Type", width: null });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
}

var Helper = {
    Init: function () {
        Control.BindYear();
        Control.BindCustomer();
        Control.BindCompany();
        Control.BindSTIPCategory();
        Control.BindQuarterly();
        Control.BindProduct();
        Control.BindBAPSType();
        Control.BindPowerType();
        //Helper.FormatCurrency("#iServicePrice");
        //Helper.FormatCurrency("#iBaseLease");
        Helper.FormatCurrency("#iAddAmount");
        Helper.FormatCurrency("#iInflasiAmount");
        Helper.FormatCurrency("#iDeductAmount");

        $("#btSearch").unbind().click(function () {
            Table.Init("#tbllReconcileReject");
            Table.Grid();
        });

        $("#btnSaveTrx").unbind().click(function () {
            Form.Update();
        });

        $("#iServicePrice").unbind().on("blur", function () {


            $("#iServicePrice").val(Common.Format.CommaSeparation($("#iServicePrice").val()));
            Helper.ProrateAmount(fsCustomerID, fsStartBapsDate, fsEndBapsDate);
        });
        $("#iBaseLease").unbind().on("blur", function () {

            $("#iBaseLease").val(Common.Format.CommaSeparation($("#iBaseLease").val()));
            Helper.ProrateAmount(fsCustomerID, fsStartBapsDate, fsEndBapsDate);
        });

        $("#btReset").unbind().click(function () {
            $("#sCustomerID").val("").trigger("change");
            $("#sCompanyID").val("").trigger("change");
            $("#sSTIPID").val("").trigger("change");
            $("#sSONumber").val("").trigger("change");
            $("#sQuarterly").val("").trigger("change");
            $("#sYear").val("0").trigger("change");
            $("#sSiteID").val("").trigger("change");
            $("#sProductID").val("").trigger("change");
            $("#sBAPSTypeID").val("").trigger("change");
            $("#sPowerTypeID").val("").trigger("change");
        });
    },

    FormatCurrency: function (id) {
        $(id).unbind().on("blur", function () {
            var value = $(id).val();
            if (value != "") {
                $(id).val(Common.Format.CommaSeparation(value));
            } else {
                $(id).val("0.00");
            }
        });
    },

    ProrateAmount: function (CustomerId, StartDate, EndDate) {


        //fsBaseLease = $("#iBaseLease").val().replace(".00", "");
        //fsBaseLease = fsBaseLease.replace(",", "");
        //fsServicePrice = fsServicePrice.replace(",", "");
        fsInflation = $("#iInflasiAmount").val();
        fsBaseLease = $("#iBaseLease").val();
        fsServicePrice = $("#iServicePrice").val();

        var curr = "";
        if (fsPOType.toUpperCase() == "BASELEASE") {
            curr = fsBaseLeaseCurr;
        } else {
            curr = fsServiceCurr;
        }
        fsPOType = fsPOType == "" ? "0" : fsPOType;


        var params = { strCustomerID: CustomerId, strStartDate: StartDate, strEndDate: EndDate, strBaseLease: fsBaseLease, strServicePrice: fsServicePrice, strBaseLeaseCurr: fsBaseLeaseCurr, strServiceCurr: fsServiceCurr, strPOType: fsPOType };

        $.ajax({
            url: "/api/ReconcileReject/prorate",
            type: "POST",
            dataType: "json",
            data: params,
            async: false,
        })
       .done(function (data, textStatus, jqXHR) {

           if (data != "") {
               if (curr == "USD") {
                   $("#iInvoiceAmountUSD").val(Common.Format.CommaSeparation(data));
               }

               else {
                   $("#iInvoiceAmountIDR").val(Common.Format.CommaSeparation(data));
               }


           }

       }).fail(function (jqXHR, textStatus, errorThrown) {
           Common.Alert.Error(errorThrown);
       });
    }
}