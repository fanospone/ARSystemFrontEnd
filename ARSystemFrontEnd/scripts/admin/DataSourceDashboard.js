Data = {};

var fsFiltering = "";
var fsShowColumn = "";

jQuery(document).ready(function () {
    Form.Init();
    
    Table.LoadData();
});

var Form = {
    Init: function () {

        Control.BindSchemaDb();
        $("#panelTransaction").hide();
        $("#formSaveDataSource").parsley();

        $("#btnAddNewDataSource").unbind().click(function () {
            fsFiltering = "";
            fsShowColumn = "";
            Form.Done();
            Control.BindRole2("0");
            Control.BindFilter(null, null);
            $("#tbViewName").prop("readonly", false);
            $("#tbDBName").prop("readonly", false);

            $("#panelSummary").fadeOut(300);
            $("#panelTransaction").fadeIn(500);
            $("#formSaveDataSource").parsley().reset();
        });

        $("#btCancel").unbind().click(function () {
            Form.Done();
            $("#panelTransaction").fadeOut(300);
            $("#panelSummary").fadeIn(500);

            $("#formSaveDataSource").parsley().reset();
        });

        $("#btSave").unbind().click(function (e) {
            e.preventDefault();
            /* Your Input must be in <form> </form>  */
            reqValue = $("#formSaveDataSource").parsley().validate();
            if ($("#formSaveDataSource").parsley().validate()) {
                Form.Save();
                // Add Your function In Here
            }
        })

        $("#btSearch").unbind().click(function () {
            Table.LoadData();
        });

        $("#btReset").unbind().click(function () {
            $("#sViewName").val("");
            $("#sDBName").val("");
            $("#sDataSourceName").val("");
            $("#sRoleID").val("").trigger("change");
        });


        Control.CustomeSelect("slRoleID2");
        Control.CustomeSelect("slParamFilter");
        Control.CustomeSelect("slColumn");


        $("#formSaveDataSource").on("click", "#btSelectslRoleID2", function () {
            Control.SelectAll("#slRoleID2");
        });

        $("#formSaveDataSource").on("click", "#btDeselectslRoleID2", function () {
            Control.DeselectAll("#slRoleID2");
        });

        $("#formSaveDataSource").on("click", "#btSelectslParamFilter", function () {
            Control.SelectAll("#slParamFilter");
        });

        $("#formSaveDataSource").on("click", "#btDeselectslParamFilter", function () {
            Control.DeselectAll("#slParamFilter");
        });


        $("#formSaveDataSource").on("click", "#btSelectslColumn", function () {
            Control.SelectAll("#slColumn");
        });

        $("#formSaveDataSource").on("click", "#btDeselectslColumn", function () {
            Control.DeselectAll("#slColumn");
        });


        $("#tbViewName").unbind().blur(function () {
            Control.BindFilter($('#tbDBName').val(), $('#tbViewName').val());
        });
        $("#tbDBName").unbind().blur(function () {
            Control.BindFilter($('#tbDBName').val(), $('#tbViewName').val());
        });
    },

    Done: function () {
        $("#tbID").val('');
        $("#tbViewName").val('');
        $("#tbDataSourceName").val('');
        $("#tbDBName").val('');
        $("#slParamFilter").val('').trigger('change');
        $("#slRoleID").val('').trigger('change');

    },

    Save: function () {
        var fsRoleID = [];
        var fsParamFilter = [];
        var fsShowColumn = [];
        fsRoleID = $("#slRoleID2").val() != null ? $("#slRoleID2").val() : fsRoleID;
        fsParamFilter = $("#slParamFilter").val() != null ? $("#slParamFilter").val() : fsParamFilter;
        fsShowColumn = $("#slColumn").val() != null ? $("#slColumn").val() : fsShowColumn;
        //$("#slColumn option:not(:selected)").each(function () {
        //    fsShowColumn.push($(this).val());
        //});

        var schema = $("#tbSchema").val();
        var params = {
            ID: $("#tbID").val(),
            ViewName: $("#tbViewName").val(),
            Schema: schema,
            DataSourceName: $("#tbDataSourceName").val(),
            DatabaseName: $("#tbDBName").val(),
            RoleID: fsRoleID,//$("#slRoleID2").val(),
            ParamFilter: fsParamFilter,// $("#slParamFilter").val()
            ShowColumn: fsShowColumn
        }

        var l = Ladda.create(document.querySelector("#btSave"))
        $.ajax({
            url: "/api/Dashboard/SaveDataSourceDashboard",
            type: "post",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false,
            beforeSend: function (xhr) {
                l.start();
            }
        }).done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.Object(data)) {
                Form.Done();
                Common.Alert.Success("Data has been saved!")
                Table.LoadData();
                $("#panelTransaction").fadeOut(300);
                $("#panelSummary").fadeIn(500);
            }
            l.stop()
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown)
            l.stop()
        })

    },

}

var Table = {
    Init: function () {
        //Table Summary
        $(".panelSearchResult").hide();

        var tblDataSourceDashboard = $('#tblDataSourceDashboard').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });
        $(window).resize(function () {
            $("#tblDataSourceDashboard").DataTable().columns.adjust().draw();
        });

    },

    LoadData: function () {
        Table.Init();

        var l = Ladda.create(document.querySelector("#btSearch"));
        l.start();
        var params = {
            ViewName: $("#sViewName").val(),
            DataSourceName: $("#sDataSourceName").val(),
            DatabaseName: $("#sDBName").val(),
            RoleID: $("#sRoleID").val()
        };

        var tblDataSourceDashboard = $("#tblDataSourceDashboard").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/Dashboard/GetDataSourceDashboard",
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
            "order": [[1, 'asc']],
            "lengthMenu": [[10, 25, 50], ['10', '25', '50']],
            "destroy": true,
            "columns": [
                { data: "RowIndex" },
                {
                    mRender: function (data, type, full) {
                        var strReturn = "";
                        strReturn += "<i class='fa fa-edit btn btn-xs green link-btEdit'></i>";
                        return strReturn;
                    }
                },
                { data: "DataSourceName" },
                { data: "DBContext" },
                { data: "ViewName" },
                { data: "Role" },
                { data: "ID" },
                { data: "RoleID" },
                { data: "DBSchema" },
                { data: "ParamFilter" },
                { data: "ShowColumn" }
            ],
            "columnDefs": [
                { "targets": [0], "orderable": false },
                { "targets": [1], "class": "text-center" },
                { "targets": [5, 6, 7, 8, 9,10], "visible": false },
            ],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblDataSourceDashboard.data())) {
                    $(".panelSearchResult").fadeIn();
                }
                l.stop();
            }
        });

        $("#tblDataSourceDashboard tbody").unbind();
        $("#tblDataSourceDashboard tbody").on("click", ".link-btEdit", function (e) {
            var table = $("#tblDataSourceDashboard").DataTable();
            var row = table.row($(this).parents('tr')).data();
            fsFiltering = row.ParamFilter;
            fsShowColumn = row.ShowColumn;
            Control.BindRole2("" + row.RoleID + "");
            Control.BindFilter(row.DBContext, row.ViewName);
            $("#tbID").val(row.ID);
            $("#tbViewName").val(row.ViewName);
            $("#tbDataSourceName").val(row.DataSourceName);
            $("#tbDBName").val(row.DBContext);
            $("#tbSchema").val(row.DBSchema).trigger('change');
            $("#panelSummary").fadeOut(300);
            $("#panelTransaction").fadeIn(500);
            $("#tbViewName").prop("readonly", true);
            $("#tbDBName").prop("readonly", true);
        });
    },

    Export: function () {

        var strCategoryBuilding = $("#sCategoryBuilding").val()
        window.location.href = "/Admin/CategoryBuilding/Export?strCategoryBuilding=" + strCategoryBuilding;
    },
}

var Control = {
    BindRole: function (id) {
        $.ajax({
            url: "/api/MstDataSource/ListRole",
            type: "GET",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.mstRoleID) + "'>" + item.Role + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Role", width: null });
            // $(id).multiSelect("refresh");
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    BindRole2: function (roleID) {
        var id = "#slRoleID2";
        $.ajax({
            url: "/api/MstDataSource/ListRole",
            type: "GET",
            async: false
        })
           .done(function (data, textStatus, jqXHR) {
               $(id).html("")
               if (Common.CheckError.List(data)) {

                   var a = roleID.split(",");
                   var role = [];
                   for (var i = 0; i < a.length; i++) {
                       role.push(a[i]);
                   }

                   $.each(data, function (i, item) {
                       if (role.includes("" + item.mstRoleID + "")) {
                           $(id).append("<option selected value='" + $.trim(item.mstRoleID) + "'>" + item.Role + "</option>");
                       } else {
                           $(id).append("<option value='" + $.trim(item.mstRoleID) + "'>" + item.Role + "</option>");
                       }

                   })
               }
               $(id).multiSelect("refresh");
           })
           .fail(function (jqXHR, textStatus, errorThrown) {
               Common.Alert.Error(errorThrown);
           });
    },

    BindSchemaDb: function () {
        var id = "#tbSchema";
        $.ajax({
            url: "/api/Dashboard/GetListSchemaDB",
            type: "GET",
            async: false
        })
           .done(function (data, textStatus, jqXHR) {
               $(id).html("");
               if (Common.CheckError.List(data)) {
                   data = JSON.parse(data);
                   $.each(data, function (i, item) {
                       $(id).append("<option value='" + $.trim(item.value) + "'>" + item.value + "</option>");
                   })
               }
               $(id).select2({ placeholder: "Select Schema", width: null });
           })
           .fail(function (jqXHR, textStatus, errorThrown) {
               Common.Alert.Error(errorThrown);
           });
    },


    BindFilter: function (dbName, vwName) {
        var id = "#slParamFilter";
        var id2 = "#slColumn";
        var result = dbName != null && dbName != "" ? true : false;
        result = vwName != null && vwName != "" ? true : false;

        if (result) {
            $.ajax({
                url: "/api/Dashboard/GetListColumns",
                type: "GET",
                async: false,
                data: { dbName: dbName, vwName: vwName },
            })
              .done(function (data, textStatus, jqXHR) {
                  $(id).html("");
                  $(id2).html("");
                  if (Common.CheckError.List(data)) {
                      if (data != "[]") {
                          $("#msgView").text('');
                          var columnFilter = [];
                          if (fsFiltering != "" && fsFiltering !=null) {
                             var Filtering = JSON.parse(fsFiltering);
                             $.each(Filtering, function (key, value) {
                                  columnFilter.push(key);
                              });
                          }

                          var column = [];
                          if (fsShowColumn != "" && fsShowColumn !=null) {
                              column = fsShowColumn.split(',');
                          }

                          data = JSON.parse(data);
                          $.each(data, function (i, item) {
                              if (columnFilter.includes("" + item.text + "")) {
                                  $(id).append("<option selected value='" + $.trim(item.text) + "'>" + item.text + "</option>");

                              } else {
                                  $(id).append("<option value='" + $.trim(item.text) + "'>" + item.text + "</option>");

                              }

                              if (column.includes("" + item.text + "")) {
                                  $(id2).append("<option selected value='" + $.trim(item.text) + "'>" + item.text + "</option>");
                              }
                              else {
                                  $(id2).append("<option  value='" + $.trim(item.text) + "'>" + item.text + "</option>");
                              }

                          });
                      } else {
                          $("#msgView").text("View is nothing!");
                          $("#tbViewName").val("");
                      }

                  }
                  $(id).multiSelect("refresh");
                  $(id2).multiSelect("refresh");
              })
              .fail(function (jqXHR, textStatus, errorThrown) {
                  Common.Alert.Error(errorThrown);
              });
        } else {
            $(id + " option").remove();
            $(id).multiSelect("refresh");
            $(id2 + " option").remove();
            $(id2).multiSelect("refresh");
        }
    },
    SelectAll: function (id) {
        $(id + " option").prop("selected", true)
        $(id).multiSelect("refresh");
    },

    DeselectAll: function (id) {
        $(id + " option:selected").prop("selected", false)
        $(id).multiSelect("refresh");
    },

    CustomeSelect: function (id) {
        $('#' + id).multiSelect({
            selectableHeader: "<input type='text' class='form-control' autocomplete='off' placeholder='Available Action'>",
            selectionHeader: "<input type='text' class='form-control' autocomplete='off' placeholder='Selected Action'>",
            afterInit: function (ms) {
                var that = this,
                    $selectableSearch = that.$selectableUl.prev(),
                    $selectionSearch = that.$selectionUl.prev(),
                    selectableSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selectable:not(.ms-selected)',
                    selectionSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selection.ms-selected';

                that.qs1 = $selectableSearch.quicksearch(selectableSearchString)
                .on('keydown', function (e) {
                    if (e.which === 40) {
                        that.$selectableUl.focus();
                        return false;
                    }
                });

                that.qs2 = $selectionSearch.quicksearch(selectionSearchString)
                .on('keydown', function (e) {
                    if (e.which == 40) {
                        that.$selectionUl.focus();
                        return false;
                    }
                });
            },
            afterSelect: function () {
                this.qs1.cache();
                this.qs2.cache();
            },
            afterDeselect: function () {
                this.qs1.cache();
                this.qs2.cache();
            },
            selectableFooter: "<button type='button' id='btSelect" + id + "' class='btn blue btn-block'>Select All</button>",
            selectionFooter: "<button type='button' id='btDeselect" + id + "' class='btn green btn-block'>Deselect All</button>"
        });
    },

}
