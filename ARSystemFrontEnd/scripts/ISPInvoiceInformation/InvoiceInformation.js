Data = {};

Data = {};
var AllDataId = [];

$(function () {
    Pages.Init();
})

var Control={
    ID: {
        slCompany: "slCompany",
        slCustomer: "slCustomer",
        slStipCode: "slStipCode",
        fSONumber: "fSONumber",
        fSiteID: "fSiteID",
        fSiteName: "fSiteName",
        fSiteIDOpr: "fSiteIDOpr",
        fSiteNameOpr: "fSiteNameOpr",
        btnSearch: "btnSearch",
        btReset: "btReset"
       
    },
    Class :{
        btnSearchFilter: "btnSearchFilter",
        txtTableSearch: "txtTableSearch"
    },
    Init:function() {
        Control.BindingSelectCompany();
        Control.BindingSelectCustomer();
        Control.BindingStipCategory();
        $('.' + Control.Class.btnSearchFilter + ', .' + Control.Class.txtTableSearch).keypress(function (e) {
            var keycode = (e.keycode ? e.keycode : e.which);
            if (keycode == '13') {
                $('#' + Control.ID.btnSearch).trigger('click');
                $("#" + Control.ID.fRequestNumber).val('');
            }
        });
        $('#' + Control.ID.btReset).on('click', function () {
            Control.Action.Reset();
        });
    },
    BindingSelectCompany: function () {
        $.ajax({
            url: "/api/MstDataSource/Company",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slCompany").html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slCompany").append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
            }

            $("#slCompany").select2({ placeholder: "Select Company", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectCustomer: function () {
        var id = "#slCustomer";
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
            $(id).select2({ placeholder: "Select Customer", width: null});
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingStipCategory: function () {
        $.ajax({
            url: "/api/MstDataSource/STIPCategory",
            type: "GET",
            async: false,
        })
        .done(function (data, textStatus, jqXHR) {
            $("#slStipCode").html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $("#slStipCode").append("<option value='" + item.STIPCode + "'>" + item.STIPDescription + "</option>");
                })
            }
            $("#slStipCode").select2({ placeholder: "STIP Code", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    Action: {
        Reset: function () {
            Control.Init();

            $("#" + Control.ID.fSONumber).val('');
            $("#" + Control.ID.fSiteID).val('');
            $("#" + Control.ID.fSiteName).val('');
            $("#" + Control.ID.fSiteIDOpr).val('');
            $("#" + Control.ID.fSiteNameOpr).val('');
        }
    }
}


var Pages = {
    Parameters: {
        slCompany: $("#" + Control.ID.slCompany).val(),
        slCustomer: $("#" + Control.ID.slCustomer).val(),
        slStipCode: $("#" + Control.ID.slStipCode).val(),
        SetValue: function () {
            Pages.Parameters.slCompany = $("#" + Control.ID.slCompany).val();
            Pages.Parameters.slStipCode = $("#" + Control.ID.slStipCode).val();
            Pages.Parameters.slCustomer = $("#" + Control.ID.slCustomer).val();
            Pages.Parameters.fSONumber = $("#" + Control.ID.fSONumber).val();
            Pages.Parameters.fSiteID = $("#" + Control.ID.fSiteID).val();
            Pages.Parameters.fSiteName = $("#" + Control.ID.fSiteName).val();
            Pages.Parameters.fSiteIDOpr = $("#" + Control.ID.fSiteIDOpr).val();
            Pages.Parameters.fSiteNameOpr = $("#" + Control.ID.fSiteNameOpr).val();
          
            
        }
    },
    Init: function () {
        Control.Init();
        Table.Search();
        $('#' + Control.ID.btnSearch + ', .' + Control.Class.btnSearchFilter).unbind().click(function () {
            Table.Search();
        });
    }
}

var Table = {
    Init: function () {
        var tblISPHeader = $('#tblISPHeader').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });
        Table.Search();
    },
    Search: function () {
        Pages.Parameters.SetValue();
        var _params = Pages.Parameters;
        var tblISPHeader = $("#tblISPHeader").DataTable({
            "deferRender": true,
            "processing": true,
            "serverSide": true,
            "bSortCellsTop": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": Url.APIs.GetDataInvoiceInformation,
                "type": "POST",
                "datatype": "json",
                "data": Func.Clone(_params)
            },
            "filter": false,
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            "buttons": [
               { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
               {
                   text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                       var l = Ladda.create(document.querySelector(".yellow"));
                       l.start();
                       Pages.Parameters.SetValue();
                       var par = Func.Clone(Pages.Parameters)
                       Table.Export(par, Url.Export.ISPInvoiceInformation);
                       l.stop();
                   }
               },
               { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
            ],
            "columns": [
                {
                    data: "SONumber", fnCreatedCell: function (ntd, sData, oData, iRow, iCol) {
                        var _link = "<a class='btn btn-xs yellow-crusta btView 'title='Invoice' href='/ISPInvoiceInformation/InvoiceInformationDetail/" + oData.SONumber + "'><i class='fa fa-eye'></i></a>"
                        $(ntd).html(_link);
                    },
                    className: "dt-center", width:'50px'
                },
                { data: "SONumber", className: "dt-center" },
                { data: "SiteID", className: "dt-center" },
                { data: "SiteName" },
                { data: "CustomerSiteID" },
                { data: "CustomerSiteName" },
                { data: "Company" },
                { data: "CustomerName" },
                { data: "STIPDescription" }
            ],
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "fnDrawCallback": function () {
                Common.CheckError.List(tblISPHeader.data());
                App.unblockUI("tblISPHeader");
            }
        })
    },
    Export: function (x, urlX) {
        window.location.href = urlX + '?slCompany=' + x.slCompany + '&slCustomer=' + x.slCustomer + '&slStipCode=' + x.slStipCode + '&fSONumber=' +
                                    x.fSONumber + '&fSiteID=' + x.fSiteID + '&fSiteName=' + x.fSiteName + '&fSiteIDOpr=' + x.fSiteIDOpr +
                                    '&fSiteNameOpr=' + x.fSiteNameOpr
    }
}

const Url = {
    APIs : {
        GetDataInvoiceInformation: "/api/ISPInvoiceInformation/InvoiceInformationGrid",
        GetTrxInvoiceInformationBySonumb: "api/ISPInvoiceInformation/TrxISPInvoiceInformationBySonumb/"
    },
    Export: {
        ISPInvoiceInformation: "ExporInvoiceInformationList"
    }

}

var Func = {
    Clone: function (arrParams) {
        var a = JSON.stringify(arrParams);
        return JSON.parse(a);
    }
}