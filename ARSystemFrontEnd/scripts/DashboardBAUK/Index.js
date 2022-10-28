var DataSelected = [];

jQuery(document).ready(function () {
    Filter.Init();
});

var Filter = {
    Init: function () {
        Filter.MonthList();
        Filter.YearList();
        Filter.MltSlsCompany.init();
        Filter.MltSlsCustomer.init();
        Filter.MltSlsSTIPCategory.init();
        Filter.MltSlsProduct.init();

        $("#panelHeader").show();
        $("#panelDetail").hide();

        $(".select2").select2({
            width: null,
            minimumResultsForSearch: -1
        });

        $("#btnBack").unbind().click(function () {
            $("#panelHeader").fadeIn();
            $("#panelDetail").fadeOut();
        })

        $("#btnBAUKSearch").unbind().click(function () {
            $("#txtActGroup").text($("#ddlBAUKGrouping").val());
            $("#txtFrcGroup").text($("#ddlBAUKGrouping").val());
            $("#txtAchGroup").text($("#ddlBAUKGrouping").val());
            $("#txtRejGroup").text($("#ddlBAUKGrouping").val());

            TblActivity.Search();
            TblForecast.Search();
            TblAchievement.Search();
            TblReject.Search();
        });

        $("#btnBAUKReset").unbind().click(function () {
            Filter.MonthList();
            Filter.YearList();
            Filter.MltSlsCompany.init();
            Filter.MltSlsCustomer.init();
            Filter.MltSlsSTIPCategory.init();
            Filter.MltSlsProduct.init();
            TblActivity.Search();
            TblForecast.Search();
            TblAchievement.Search();
            TblReject.Search();
        });

    },
    MonthList: function () {

        var id = "#ddlBAUKMonth"
        $(id).html("");
        var month = [];
        var arrayMonth = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
        var arrayName = ["Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember"];
        var currentMonth = new Date().getMonth() + 1;
        $(id).html('');
        //$(id).append('<option value=" ">Select All</option>');
        for (var i = 0; i < 12; i++) {
            var data = {};
            data.month = arrayMonth[i];
            data.monthName = arrayName[i];

            if (data.month == currentMonth)
                $(id).append('<option selected value="' + data.month + '">' + data.monthName + '</option>');
            else
                $(id).append('<option value="' + data.month + '">' + data.monthName + '</option>');
        }
        // init multiselect
        $('#ddlBAUKMonth').multiselect({
            disableIfEmpty: true,
            enableFiltering: true,
            includeSelectAllOption: true,
            allSelectedText: 'All Months',
            nSelectedText: 'Months',
            nonSelectedText: 'Select Month',
            buttonWidth: '100%',
            maxHeight: 200,
            buttonClass: 'mt-multiselect btn btn-default',
            enableCaseInsensitiveFiltering: true,
        });
        $(id).multiselect('rebuild');
    },
    YearList: function () {
        var id = "#ddlBAUKYear";
        var startYear = 2015;
        var currentyear = new Date().getFullYear();
        $(id).html('');
        //$(id).append('<option value=" ">Select All</option>');

        for (var i = currentyear; i >= startYear; i--) {
            if (i == currentyear)
                $(id).append('<option selected value="' + i + '">' + i + '</option>');
            else
                $(id).append('<option value="' + i + '">' + i + '</option>');
        }
    },
    MltSlsCompany: {
        init: function () {
            Filter.MltSlsCompany.refresh("#ddlBAUKCompany");
            $('#ddlBAUKCompany').multiselect({
                disableIfEmpty: true,
                enableFiltering: true,
                includeSelectAllOption: true,
                allSelectedText: 'All Company',
                nSelectedText: 'Company',
                nonSelectedText: 'Select Company',
                buttonWidth: '100%',
                maxHeight: 200,
                buttonClass: 'mt-multiselect btn btn-default',
                enableCaseInsensitiveFiltering: true,
            });
        },
        refresh: function (selector) {
            $(selector).html("");

            Filter.MltSlsCompany.getAjax().done(function (data, textStatus, jqXHR) {
                $.each(data, function (i, item) {
                    $(selector).append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                });
                $(selector).multiselect('rebuild');
            });
        },
        getAjax: function () {
            return $.ajax({
                url: "/api/MstDataSource/TBGSys_Company",
                type: "GET",
            })
        }
    },
    MltSlsCustomer: {
        init: function () {
            Filter.MltSlsCustomer.refresh("#ddlBAUKCustomer");
            // init multiselect
            $('#ddlBAUKCustomer').multiselect({
                disableIfEmpty: true,
                enableFiltering: true,
                includeSelectAllOption: true,
                allSelectedText: 'All Customer',
                nSelectedText: 'Customer',
                nonSelectedText: 'Select Customer',
                buttonWidth: '100%',
                maxHeight: 200,
                buttonClass: 'mt-multiselect btn btn-default',
                enableCaseInsensitiveFiltering: true,
            });
        },
        refresh: function (selector) {
            $(selector).html("");

            Filter.MltSlsCustomer.getAjax().done(function (data, textStatus, jqXHR) {
                $.each(data, function (i, item) {
                    $(selector).append("<option value='" + item.Operator + "'>" + item.PopularName + "</option>");
                });
                $(selector).multiselect('rebuild');
            });
        },
        getAjax: function () {
            return $.ajax({
                url: "/api/MstDataSource/TBGSys_Operator",
                type: "GET",
            })
        }
    },
    MltSlsSTIPCategory: {
        init: function () {
            Filter.MltSlsSTIPCategory.refresh("#ddlBAUKCategory");
            // init multiselect
            $('#ddlBAUKCategory').multiselect({
                disableIfEmpty: true,
                enableFiltering: true,
                includeSelectAllOption: true,
                allSelectedText: 'All STIP Category',
                nSelectedText: 'STIP Category',
                nonSelectedText: 'Select STIP Category',
                buttonWidth: '100%',
                maxHeight: 200,
                buttonClass: 'mt-multiselect btn btn-default',
                enableCaseInsensitiveFiltering: true,
            });
        },
        refresh: function (selector) {
            $(selector).html("");

            Filter.MltSlsSTIPCategory.getAjax().done(function (data, textStatus, jqXHR) {
                $.each(data, function (i, item) {
                    $(selector).append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                });
                $(selector).multiselect('rebuild');
            });
        },
        getAjax: function () {
            return $.ajax({
                url: "/api/MstDataSource/TBGSys_Stip",
                type: "GET",
            })
        }
    },
    MltSlsProduct: {
        init: function () {
            Filter.MltSlsProduct.refresh("#ddlBAUKProduct");
            // init multiselect
            $('#ddlBAUKProduct').multiselect({
                disableIfEmpty: true,
                enableFiltering: true,
                includeSelectAllOption: true,
                allSelectedText: 'All Product',
                nSelectedText: 'Product',
                nonSelectedText: 'Select Product',
                buttonWidth: '100%',
                maxHeight: 200,
                buttonClass: 'mt-multiselect btn btn-default',
                enableCaseInsensitiveFiltering: true,
            });
        },
        refresh: function (selector) {
            $(selector).html("");

            Filter.MltSlsProduct.getAjax().done(function (data, textStatus, jqXHR) {
                $.each(data, function (i, item) {
                    $(selector).append("<option value='" + item.ProductID + "'>" + item.Product + "</option>");
                });
                $(selector).multiselect('rebuild');
            });
        },
        getAjax: function () {
            return $.ajax({
                url: "/api/MstDataSource/TBGSys_Product",
                type: "GET",
            })
        }
    }
}