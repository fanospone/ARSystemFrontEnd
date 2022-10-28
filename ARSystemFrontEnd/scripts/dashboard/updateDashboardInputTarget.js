var isDataUpdated = false;
var filterValueYearAndDept = {
    year: $('#year').val(),
    dept: $('#departmentCode').val()
};
jQuery(document).ready(function () {
    Control.BindSelectYearAll();
    Control.BindingSelectDepartment();
    

    $('#formDashboardInputTarget').submit(function (e) {
        e.preventDefault();
        trxDashboardInputTargetDetail.Submit();
    });
    $('#formDashboardInputTarget .input-group input').on('change', function () {
        isDataUpdated = true;
    })

    $('#year, #departmentCode').on('change', function () {
        //load data value
        var year = $('#year').val();
        var dept = $('#departmentCode').val();
        //bind latest filter value to object

        if (year > 0 && dept.length > 0) {
            //prompt load data
            if (isDataUpdated) {
                swal({
                    title: "Warning",
                    text: "Your following action will reload data in this page, your changed data will be lost. Are You Sure?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    showConfirmButton: true,
                    confirmButtonText: 'Yes, Reload',
                    cancelButtonText: 'Cancel',
                    dangerMode: true,
                }, function (isConfirm) {
                    if (isConfirm) {
                        //reload data from database
                        trxDashboardInputTargetDetail.LoadData()
                        filterValueYearAndDept = {
                            year: year,
                            dept: dept
                        }
                        isDataUpdated = false;
                    } else {
                        $('#year').val(filterValueYearAndDept.year).trigger('change');
                        $('#departmentCode').val(filterValueYearAndDept.dept).trigger('change');
                    }
                });
            } else {
                trxDashboardInputTargetDetail.LoadData()
                filterValueYearAndDept = {
                    year: year,
                    dept: dept
                }
            }
        }
        
    });


    $('.btResetTarget').click(function (e) {
        e.preventDefault();
        $('#formDashboardInputTarget input').val('');
    });
    
});
var trxDashboardInputTargetDetail = {
    Init: function () {
        var tblSummaryData = $('#tbltrxdashboardDetail').dataTable({
            "filter": false,
            "destroy": true,
            "data": [],
            "ordering": false
        });
    },
    Submit: function()
    {
        var url = "/api/ApiDashboardInputTarget/SaveInputTargetPercentage";
            var l = Ladda.create(document.querySelector("#btSubmitTarget"));
            l.start();
            var data = $("#formDashboardInputTarget").serializeObject();
            $.ajax({
                url: url,
                type: "post",
                dataType: "json",
                data: data,
                cache: false,
                beforeSend: function (xhr) {
                }
            }).done(function (data, textStatus, jqXHR) {
                if (data.ErrorMessage != null) {
                    Common.Alert.Error(data.ErrorMessage);
                } else {
                    Common.Alert.Success("Data has been saved!")
                    //set isDataUpdated to false, to prevent prompt confirmation
                    isDataUpdated = false;
                }
                l.stop()
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l.stop()
            })
    },



    Reset: function () {
        Filter.ddlfYear('#fyear');
        Filter.ddlfQuarter('#fquarter');
    },

    LoadData: function () {
        var year = $('#year').val();
        var dept = $('#departmentCode').val();
        var url = "/api/ApiDashboardInputTarget/FindInputTargetPercentage";
        App.blockUI({
            target: ".panelSearchResult", boxed: true
        });
        $.ajax({
            url: url,
            type: "post",
            dataType: "json",
            data: { year: year, DepartmentCode: dept },
            cache: false,
            beforeSend: function (xhr) {
            }
        }).done(function (data, textStatus, jqXHR) {
            if (data != {}) {
                $('#jan_optimist').val(data.Jan_Optimist);
                $('#jan_mostLikely').val(data.Jan_MostLikely);
                $('#jan_pessimist').val(data.Jan_Pessimist);

                $('#feb_optimist').val(data.Feb_Optimist);
                $('#feb_mostLikely').val(data.Feb_MostLikely);
                $('#feb_pessimist').val(data.Feb_Pessimist);

                $('#mar_optimist').val(data.Mar_Optimist);
                $('#mar_mostLikely').val(data.Mar_MostLikely);
                $('#mar_pessimist').val(data.Mar_Pessimist);

                $('#apr_optimist').val(data.Apr_Optimist);
                $('#apr_mostLikely').val(data.Apr_MostLikely);
                $('#apr_pessimist').val(data.Apr_Pessimist);

                $('#may_optimist').val(data.May_Optimist);
                $('#may_mostLikely').val(data.May_MostLikely);
                $('#may_pessimist').val(data.May_Pessimist);

                $('#jun_optimist').val(data.Jun_Optimist);
                $('#jun_mostLikely').val(data.Jun_MostLikely);
                $('#jun_pessimist').val(data.Jun_Pessimist);

                $('#jul_optimist').val(data.Jul_Optimist);
                $('#jul_mostLikely').val(data.Jul_MostLikely);
                $('#jul_pessimist').val(data.Jul_Pessimist);

                $('#aug_optimist').val(data.Aug_Optimist);
                $('#aug_mostLikely').val(data.Aug_MostLikely);
                $('#aug_pessimist').val(data.Aug_Pessimist);

                $('#sep_optimist').val(data.Sep_Optimist);
                $('#sep_mostLikely').val(data.Sep_MostLikely);
                $('#sep_pessimist').val(data.Sep_Pessimist);

                $('#oct_optimist').val(data.Oct_Optimist);
                $('#oct_mostLikely').val(data.Oct_MostLikely);
                $('#oct_pessimist').val(data.Oct_Pessimist);

                $('#nov_optimist').val(data.Nov_Optimist);
                $('#nov_mostLikely').val(data.Nov_MostLikely);
                $('#nov_pessimist').val(data.Nov_Pessimist);

                $('#dec_optimist').val(data.Dec_Optimist);
                $('#dec_mostLikely').val(data.Dec_MostLikely);
                $('#dec_pessimist').val(data.Dec_Pessimist);

            }

            if (data.ErrorMessage != null) {
                Common.Alert.Error(data.ErrorMessage);
            } 
            App.unblockUI('.panelSearchResult');

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            App.unblockUI('.panelSearchResult');

        })


    }
}

var Control = {

    BindSelectYearAll: function () {
        var dt = new Date();
        var currentyear = dt.getFullYear();
        var ids = "#year"
        $(ids).html("<option></option>")
        for (var i = -10; i <= 10; i++) {
            if (currentyear == (currentyear + i))
                $(ids).append("<option value='" + (currentyear + i) + "' selected >" + (currentyear + i) + "</option>");
            else
                $(ids).append("<option value='" + (currentyear + i) + "'>" + (currentyear + i) + "</option>");

        }

        $(ids).select2({ placeholder: "Select Year", width: null });
    },
    BindingSelectDepartment: function () {
        var ids = "#departmentCode";
        var data = [
            {
                id: "DE290",
                text: 'New BAPS Department'
            },
            {
                id: "DE000434",
                text: 'Recurring BAPS TSEL Department'
            },
            {
                id: "DE000435",
                text: 'Recurring BAPS Non TSEL Department'
            },
            {
                id: "DE000421",
                text: 'Recurring New Product & Others Revenue Department'
            }
        ];
        $(ids).html("<option></option>")
        for (var i = 0; i < data.length; i++) {
            $(ids).append("<option value='" + data[i].id + "'>" + data[i].text + "</option>");
        }

        $(ids).select2({ placeholder: "Select Department Code", width: null });
        //public static class RADepartmentCodeTabEnum
        //{
        //    public const string NewBaps = "DE290";
        //    public const string TSEL = "DE000434";
        //    public const string NonTSEL = "DE000435";
        //    public const string NewProduct = "DE000421";
        //    }
    }

  
}

jQuery.fn.extend({
    serializeObject: function () {
        var formdata = $(this).serializeArray();
        var data = {};
        $(formdata).each(function (index, obj) {
            if (obj.name == 'ActualM1' || obj.name == 'ActualM2' || obj.name == 'ActualM3' ||
                obj.name == 'FCFinanceM1' || obj.name == 'FCFinanceM2' || obj.name == 'FCFinanceM3' ||
                obj.name == 'FCRevenueAssuranceM1' || obj.name == 'FCRevenueAssuranceM2' || obj.name == 'FCRevenueAssuranceM3' ||
                obj.name == 'FCMarketingM1' || obj.name == 'FCMarketingM2' || obj.name == 'FCMarketingM3' ||
                obj.name == 'VarianceM1' || obj.name == 'VarianceM2' || obj.name == 'VarianceM3') {
                data[obj.name] = obj.value.replace(/,/g, "");
            } else {
                data[obj.name] = obj.value;
            }
        });
        return data;
    }
});


jQuery.fn.extend({
    serializeObject: function () {
        var formdata = $(this).serializeArray();
        var data = {};
        $(formdata).each(function (index, obj) {
            data[obj.name] = obj.value;
        });
        return data;
    }
});