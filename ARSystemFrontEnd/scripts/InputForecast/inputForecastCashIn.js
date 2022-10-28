$(function () {
    Control.BindSelectYearAll("#fyear");
    Control.BindSelectQuarter($("#fquarter"));
    //SEARCH CLICK 
    $("#btSearch").unbind().click(function () {
        trxARInputForecastVsActual.Search()
        trxARInputForecastVsForecast.Search()
    });
    //RESET
    $("#btReset").unbind().click(function () {
        Control.BindSelectYearAll("#fyear");
        Control.BindSelectQuarter($("#fquarter"));

        trxARInputForecastVsActual.Init();
        trxARInputForecastVsForecast.Init();
    });
});

var Control = {
    BindSelectYearAll: function (elements) {
        var dt = new Date();
        var currentyear = dt.getFullYear();
        $(elements).html("");
        for (var i = -10; i <= 10; i++) {
            if (currentyear + i == settingVariable.yearOfInputPica) {
                $(elements).append("<option selected value='" + (currentyear + i) + "'>" + (currentyear + i) + "</option>");
            }else{
                $(elements).append("<option value='" + (currentyear + i) + "'>" + (currentyear + i) + "</option>");

            }
        }
        $(elements).select2({ placeholder: "Select Year", width: null });
    },
    BindSelectQuarter: function (elements) {
        var dt = new Date();
        var currentQuarter = Math.floor((dt.getMonth() + 3) / 3);;
        elements.html("");
        var dt = new Date();
        var currentmonth = dt.getMonth();
        for (var i = 1; i <= 4; i++) {
            if (i == settingVariable.quarterOfInputPica) {
                elements.append("<option selected value='" + i + "'>" + i + "</option>");
            } else {
                elements.append("<option value='" + i + "'>" + i + "</option>");
            }
        }
        elements.select2({ placeholder: "Select Quarter", width: null });
    },
    

}
