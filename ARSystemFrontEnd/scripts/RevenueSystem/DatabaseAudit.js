Data = {};

var fsYear = "";
var fsRegional = "";
var fsCompany = "";
var fsOperator = "";

jQuery(document).ready(function () {
    Control.Init();
});

var Control = {
    Init: function () {
        Table.Search();

        Control.BindingSelectYear($("#fyear"));
        Control.BindingSelectRegional($('#fRegional'));
        Control.BindingSelectCompany($('#fCompany'));
        Control.BindingSelectOperator($('#fOperator'));

        $("#btSearch").unbind().click(function () {
            Table.Search();
        });
        $("#btReset").unbind().click(function () {
            Control.Reset();
        });
        $(".btnSearchHistory").unbind().click(function () {
            Table.Search();
        });
        
    },
    BindingSelectYear: function () {
        var dt = new Date();
        var past = dt.getFullYear() - 5;
        var ydata = [];
        for (var i = 0; i <= 20; i++) {
            ydata.push(past);
            past++;
        }
        $('#fYear').select2({
            data: ydata,
            placeholder: "Select Year"
        });
        $('#fYear').val(dt.getFullYear()).trigger('change')
    },
    BindingSelectRegional: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/Regional",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                elements.html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        elements.append("<option value='" + item.Regional + "'>" + item.Regional + "</option>");
                    })
                }

                elements.select2({ placeholder: "Select Regional", width: null });

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    BindingSelectCompany: function (elements) {
    $.ajax({
        url: "/api/MstDataSource/Company",
        type: "GET"
    })
        .done(function (data, textStatus, jqXHR) {
            elements.html("<option></option>")

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    elements.append("<option value='" + item.CompanyId + "'>" + item.Company + "</option>");
                })
            }

            elements.select2({ placeholder: "Select Company Name", width: null });

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindingSelectOperator: function (elements) {
        $.ajax({
            url: "/api/MstDataSource/Operator",
            type: "GET"
        })
            .done(function (data, textStatus, jqXHR) {
                elements.html("<option></option>")

                if (Common.CheckError.List(data)) {
                    $.each(data, function (i, item) {
                        elements.append("<option value='" + $.trim(item.OperatorId) + "'>" + item.OperatorId + "</option>");
                    })
                }

                elements.select2({ placeholder: "Select Operator", width: null });

            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
            });
    },
    Reset: function () {
        //$('#fYear').val(null).trigger('change');
        $('#fCompany').val(null).trigger('change');
        $('#fRegional').val(null).trigger('change');
        $('#fOperator').val(null).trigger('change');
        $('#schSONumber').val('');
        $('#schSiteName').val('');
    }
}

var Table = {
    //Init: function () {
    //    $(".panelSearchZero").hide();
    //    $(".panelSearchResult").hide();
    //    Table.Search();
    //},
    Search: function () {
        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();
       
        //serch grid
        var SoNumber = $("#schSONumber").val();
        var SiteName = $("#schSiteName").val();
        // var filter
        var params = {
            fYear: $('#fYear').val(),
            fCompanyId: $('#fCompany').val(),
            fRegionalName: $('#fRegional').val(),
            fOperatorId: $('#fOperator').val(),
            schSONumber: SoNumber,
            schSiteName: SiteName
        }
        var no = 1;
        var tblRevenueSummary = $("#tblRevenueSummary").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "bSortCellsTop": true,
            "language": {
                "emptyTable": "No data available in table",
            },
            "ajax": {
                "url": "/Api/RevenueSystem/GetSummaryRevenue",
                "type": "POST",
                "datatype": "json",
                "data": params,
            },
            buttons: [
                { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy to Clipboard"></i>', titleAttr: 'Copy to Clipboard' },
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
                { data: null },
                { data: "SONumber", className: "text-left" },
                { data: "SiteName", className: "text-left" },
                { data: "CustomerID", className: "text-center" },
                { data: "RegionName", className: "text-left" },
                 //jan
                 {
                     data: 'JanSourceAccrued', className: "text-right", render: function (data) {
                           return Common.Format.CommaSeparation(data);
                      }
                 },
                 {
                     data: 'JanSourceAmrUnearned', className: "text-right", render: function (data) {
                          return Common.Format.CommaSeparation(data);
                      }
                 },
                 {
                     data: 'JanSourceBalance', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'JanBalanceAccrued', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'JanBalanceUnearned', className: "text-right", render: function (data) {
                          return Common.Format.CommaSeparation(data);
                      }
                  },         
                //feb
                 {
                    data: 'FebSourceAccrued', className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                 {
                     data: 'FebSourceAmrUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'FebSourceBalance', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'FebBalanceAccrued', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'JanBalanceUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                //mar
                 {
                    data: 'MarSourceAccrued', className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                 {
                     data: 'MarSourceAmrUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'MarSourceBalance', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'MarBalanceAccrued', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'MarBalanceUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                //apr
                 {
                    data: 'AprSourceAccrued', className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                 {
                     data: 'AprSourceAmrUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'AprSourceBalance', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'AprBalanceAccrued', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'AprBalanceUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                //may
                 {
                    data: 'MaySourceAccrued', className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                 {
                     data: 'MaySourceAmrUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'MaySourceBalance', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'MayBalanceAccrued', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'MayBalanceUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                //Jun
                 {
                    data: 'JunSourceAccrued', className: "text-right", render: function (data) {
                        return Common.Format.CommaSeparation(data);
                    }
                },
                 {
                     data: 'JunSourceAmrUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'JunSourceBalance', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'JunBalanceAccrued', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'JunBalanceUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                //Jul
                 {
                   data: 'JulSourceAccrued', className: "text-right", render: function (data) {
                       return Common.Format.CommaSeparation(data);
                   }
               },
                 {
                     data: 'JulSourceAmrUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'JulSourceBalance', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'JulBalanceAccrued', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'JulBalanceUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                //Agu
                 {
                   data: 'AugSourceAccrued', className: "text-right", render: function (data) {
                       return Common.Format.CommaSeparation(data);
                   }
               },
                 {
                     data: 'AugSourceAmrUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'AugSourceBalance', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'AugBalanceAccrued', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'AugBalanceUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                //Sep
                 {
                   data: 'SepSourceAccrued', className: "text-right", render: function (data) {
                       return Common.Format.CommaSeparation(data);
                   }
               },
                 {
                     data: 'SepSourceAmrUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'SepSourceBalance', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'SepBalanceAccrued', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'SepBalanceUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                //Oct
                 {
                  data: 'OctSourceAccrued', className: "text-right", render: function (data) {
                      return Common.Format.CommaSeparation(data);
                  }
              },
                 {
                     data: 'OctSourceAmrUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'OctSourceBalance', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'OctBalanceAccrued', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'OctBalanceUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                //Nov
                 {
                  data: 'NovSourceAccrued', className: "text-right", render: function (data) {
                      return Common.Format.CommaSeparation(data);
                  }
              },
                 {
                     data: 'NovSourceAmrUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'NovSourceBalance', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'NovBalanceAccrued', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'NovBalanceUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                //Dec
                 {
                  data: 'DecSourceAccrued', className: "text-right", render: function (data) {
                      return Common.Format.CommaSeparation(data);
                  }
              },
                 {
                     data: 'DecSourceAmrUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'DecSourceBalance', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'DecBalanceAccrued', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
                 {
                     data: 'DecBalanceUnearned', className: "text-right", render: function (data) {
                         return Common.Format.CommaSeparation(data);
                     }
                 },
            ],
            "columnDefs": [
                { "targets": [0], "width": "2%", "className": "dt-center" },
                { "targets": [1,2,3,4], "width": "5%", "className": "dt-center" },
                { "targets": [5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
                   31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60 ], "width": "40%", "className": "dt-center"}
               
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblRevenueSummary.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").show();
                }
                l.stop();
                App.unblockUI('.panelSearchResult');
            },
            'scrollCollapse': true, 
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api();

                // converting to interger to find total
                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                        i : 0;
                };
                //  computing column Total of the complete result 
                //jan
                var JanSourceAccruedTot = api.column(5).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var JanSourceAmrUnearnedTot = api.column(6).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var JanSourceBalanceTot = api.column(7).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var JanBalanceAccruedTot = api.column(8).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var JanBalanceUnearnedTot = api.column(9).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                //feb
                var FebSourceAccruedTot = api.column(10).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var FebSourceAmrUnearnedTot = api.column(11).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var FebSourceBalanceTot = api.column(12).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var FebBalanceAccruedTot = api.column(13).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var FebBalanceUnearnedTot = api.column(14).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                //mar
                var MarSourceAccruedTot = api.column(15).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var MarSourceAmrUnearnedTot = api.column(16).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var MarSourceBalanceTot = api.column(17).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var MarBalanceAccruedTot = api.column(18).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var MarBalanceUnearnedTot = api.column(19).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                //apr
                var AprSourceAccruedTot = api.column(20).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var AprSourceAmrUnearnedTot = api.column(21).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var AprSourceBalanceTot = api.column(22).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var AprBalanceAccruedTot = api.column(23).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var AprBalanceUnearnedTot = api.column(24).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                //may
                var MaySourceAccruedTot = api.column(25).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var MaySourceAmrUnearnedTot = api.column(26).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var MaySourceBalanceTot = api.column(27).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var MayBalanceAccruedTot = api.column(28).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var MayBalanceUnearnedTot = api.column(29).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                //jun
                var JunSourceAccruedTot = api.column(30).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var JunSourceAmrUnearnedTot = api.column(31).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var JunSourceBalanceTot = api.column(32).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var JunBalanceAccruedTot = api.column(33).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var JunBalanceUnearnedTot = api.column(34).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                //jun
                var JulSourceAccruedTot = api.column(35).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var JulSourceAmrUnearnedTot = api.column(36).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var JulSourceBalanceTot = api.column(37).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var JulBalanceAccruedTot = api.column(38).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var JulBalanceUnearnedTot = api.column(39).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                //aug
                var AugSourceAccruedTot = api.column(40).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var AugSourceAmrUnearnedTot = api.column(41).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var AugSourceBalanceTot = api.column(42).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var AugBalanceAccruedTot = api.column(43).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var AugBalanceUnearnedTot = api.column(44).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                //sep
                var SepSourceAccruedTot = api.column(45).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var SepSourceAmrUnearnedTot = api.column(46).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var SepSourceBalanceTot = api.column(47).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var SepBalanceAccruedTot = api.column(48).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var SepBalanceUnearnedTot = api.column(49).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                //oct
                var OctSourceAccruedTot = api.column(50).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var OctSourceAmrUnearnedTot = api.column(51).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var OctSourceBalanceTot = api.column(52).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var OctBalanceAccruedTot = api.column(53).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var OctBalanceUnearnedTot = api.column(54).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                //oct
                var NovSourceAccruedTot = api.column(55).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var NovSourceAmrUnearnedTot = api.column(56).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var NovSourceBalanceTot = api.column(57).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var NovBalanceAccruedTot = api.column(58).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var NovBalanceUnearnedTot = api.column(59).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                //Dec
                var DecSourceAccruedTot = api.column(60).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var DecSourceAmrUnearnedTot = api.column(61).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var DecSourceBalanceTot = api.column(62).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var DecBalanceAccruedTot = api.column(63).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                var DecBalanceUnearnedTot = api.column(64).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
                // Update footer by showing the total with the reference of the column index 
                $(api.column(1).footer()).html('Total');
                //jan
                $(api.column(5).footer()).html(Common.Format.CommaSeparation(JanSourceAccruedTot));
                $(api.column(6).footer()).html(Common.Format.CommaSeparation(JanSourceAmrUnearnedTot));
                $(api.column(7).footer()).html(Common.Format.CommaSeparation(JanSourceBalanceTot));
                $(api.column(8).footer()).html(Common.Format.CommaSeparation(JanBalanceAccruedTot));
                $(api.column(9).footer()).html(Common.Format.CommaSeparation(JanBalanceUnearnedTot));
                //feb
                $(api.column(10).footer()).html(Common.Format.CommaSeparation(FebSourceAccruedTot));
                $(api.column(11).footer()).html(Common.Format.CommaSeparation(FebSourceAmrUnearnedTot));
                $(api.column(12).footer()).html(Common.Format.CommaSeparation(FebSourceBalanceTot));
                $(api.column(13).footer()).html(Common.Format.CommaSeparation(FebBalanceAccruedTot));
                $(api.column(14).footer()).html(Common.Format.CommaSeparation(FebBalanceUnearnedTot));
                //mar
                $(api.column(15).footer()).html(Common.Format.CommaSeparation(MarSourceAccruedTot));
                $(api.column(16).footer()).html(Common.Format.CommaSeparation(MarSourceAmrUnearnedTot));
                $(api.column(17).footer()).html(Common.Format.CommaSeparation(MarSourceBalanceTot));
                $(api.column(18).footer()).html(Common.Format.CommaSeparation(MarBalanceAccruedTot));
                $(api.column(19).footer()).html(Common.Format.CommaSeparation(MarBalanceUnearnedTot));
                //apr
                $(api.column(20).footer()).html(Common.Format.CommaSeparation(AprSourceAccruedTot));
                $(api.column(21).footer()).html(Common.Format.CommaSeparation(AprSourceAmrUnearnedTot));
                $(api.column(22).footer()).html(Common.Format.CommaSeparation(AprSourceBalanceTot));
                $(api.column(23).footer()).html(Common.Format.CommaSeparation(AprBalanceAccruedTot));
                $(api.column(24).footer()).html(Common.Format.CommaSeparation(AprBalanceUnearnedTot));
                //may
                $(api.column(25).footer()).html(Common.Format.CommaSeparation(MaySourceAccruedTot));
                $(api.column(26).footer()).html(Common.Format.CommaSeparation(MaySourceAmrUnearnedTot));
                $(api.column(27).footer()).html(Common.Format.CommaSeparation(MaySourceBalanceTot));
                $(api.column(28).footer()).html(Common.Format.CommaSeparation(MayBalanceAccruedTot));
                $(api.column(29).footer()).html(Common.Format.CommaSeparation(MayBalanceUnearnedTot));
                //jun
                $(api.column(30).footer()).html(Common.Format.CommaSeparation(JunSourceAccruedTot));
                $(api.column(31).footer()).html(Common.Format.CommaSeparation(JunSourceAmrUnearnedTot));
                $(api.column(32).footer()).html(Common.Format.CommaSeparation(JunSourceBalanceTot));
                $(api.column(33).footer()).html(Common.Format.CommaSeparation(JunBalanceAccruedTot));
                $(api.column(34).footer()).html(Common.Format.CommaSeparation(JunBalanceUnearnedTot));
                //jul
                $(api.column(35).footer()).html(Common.Format.CommaSeparation(JulSourceAccruedTot));
                $(api.column(36).footer()).html(Common.Format.CommaSeparation(JulSourceAmrUnearnedTot));
                $(api.column(37).footer()).html(Common.Format.CommaSeparation(JulSourceBalanceTot));
                $(api.column(38).footer()).html(Common.Format.CommaSeparation(JulBalanceAccruedTot));
                $(api.column(39).footer()).html(Common.Format.CommaSeparation(JulBalanceUnearnedTot));
                //aug
                $(api.column(40).footer()).html(Common.Format.CommaSeparation(AugSourceAccruedTot));
                $(api.column(41).footer()).html(Common.Format.CommaSeparation(AugSourceAmrUnearnedTot));
                $(api.column(42).footer()).html(Common.Format.CommaSeparation(AugSourceBalanceTot));
                $(api.column(43).footer()).html(Common.Format.CommaSeparation(AugBalanceAccruedTot));
                $(api.column(44).footer()).html(Common.Format.CommaSeparation(AugBalanceUnearnedTot));
                //sep
                $(api.column(45).footer()).html(Common.Format.CommaSeparation(SepSourceAccruedTot));
                $(api.column(46).footer()).html(Common.Format.CommaSeparation(SepSourceAmrUnearnedTot));
                $(api.column(47).footer()).html(Common.Format.CommaSeparation(SepSourceBalanceTot));
                $(api.column(48).footer()).html(Common.Format.CommaSeparation(SepBalanceAccruedTot));
                $(api.column(49).footer()).html(Common.Format.CommaSeparation(SepBalanceUnearnedTot));
                //oct
                $(api.column(50).footer()).html(Common.Format.CommaSeparation(OctSourceAccruedTot));
                $(api.column(51).footer()).html(Common.Format.CommaSeparation(OctSourceAmrUnearnedTot));
                $(api.column(52).footer()).html(Common.Format.CommaSeparation(OctSourceBalanceTot));
                $(api.column(53).footer()).html(Common.Format.CommaSeparation(OctBalanceAccruedTot));
                $(api.column(54).footer()).html(Common.Format.CommaSeparation(OctBalanceUnearnedTot));
                //oct
                $(api.column(55).footer()).html(Common.Format.CommaSeparation(NovSourceAccruedTot));
                $(api.column(56).footer()).html(Common.Format.CommaSeparation(NovSourceAmrUnearnedTot));
                $(api.column(57).footer()).html(Common.Format.CommaSeparation(NovSourceBalanceTot));
                $(api.column(58).footer()).html(Common.Format.CommaSeparation(NovBalanceAccruedTot));
                $(api.column(59).footer()).html(Common.Format.CommaSeparation(NovBalanceUnearnedTot));
                //Dec
                $(api.column(60).footer()).html(Common.Format.CommaSeparation(DecSourceAccruedTot));
                $(api.column(61).footer()).html(Common.Format.CommaSeparation(DecSourceAmrUnearnedTot));
                $(api.column(62).footer()).html(Common.Format.CommaSeparation(DecSourceBalanceTot));
                $(api.column(63).footer()).html(Common.Format.CommaSeparation(DecBalanceAccruedTot));
                $(api.column(64).footer()).html(Common.Format.CommaSeparation(DecBalanceUnearnedTot));
            },
        });
     
        tblRevenueSummary.on('draw.dt', function () {
            var PageInfo = $('#tblRevenueSummary').DataTable().page.info();
            tblRevenueSummary.column(0, { page: 'current' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1 + PageInfo.start;
            });
        });
    },
    
    Export: function () {
     
        var fYear = $("#fYear").val();
        var RegionName = $("#fRegional").val();
        var CompanyID = $("#fCompany").val();
        var CustomerID = $("#fOperator").val();

       
        window.location.href = "/RevenueSystem/ListDashboardRevenue/Export?strYear=" + fYear + "&strRegionName=" + RegionName + "&strCompany=" + CompanyID + "&strCustomer=" + CustomerID;
    }
}

