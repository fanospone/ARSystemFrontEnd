$(function () {
    Control.Init();
    Control.InitCreateEditInflasi();

});

var Control = {
    Init: function () {

        $('#addNewInflasi').on('click', function () {
            Temp.IDIR = 0;
            var htmlscript = $('script#formCreateEditInflasi').html();
            $('#createEditInflasi').html(htmlscript);

            $('#mdlCreateEditInflasi').modal('show');
        });


        Table.InflationRate.Search();
        Widget.Init();
        $('.btnSearchIR').on('click', function () {
            Table.InflationRate.Search();
        });
        $('.btnSearchSI').on('click', function () {
            Table.SonumbInflasi.Search();
        });
        $('.btnSearchAddSonumb').on('click', function () {
            Table.gridSonumbList.Search();
        });
        Table.SonumbInflasi.Search();
    },
    InitCreateEditInflasi: function () {
        Widget.Init();
        $('#btnAddSonumbInflasi').on('click', function () {
            $('#mdlAddSonumb').modal('show');
            $('#btnCancelAddSI').on('click', function () {
                location.reload();
            });
            $('#btnDownloadTemplate').on('click', function () {
                var FilePath = "";
                var ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var OriginalFileName = "TemplateUploadSonumbInflation.xlsx";
                window.location.href = Url.Download.gridIR + "?FilePath=" + FilePath + "&ContentType=" + ContentType + "&OriginalFileName=" + OriginalFileName;
            });
            Table.gridSonumbList.Search();
            Func.SeparationNumber("#txtRentalAmountModal");
            Func.SeparationNumber("#txtServiceAmountModal");
            Func.SeparationNumber("#txtInflasiAmountModal");
            $("#tblModalAddSonumb").on('change', 'tbody tr .checkboxes', function () {
                var table = $("#tblModalAddSonumb").DataTable();
                var Row = table.row($(this).parents('tr')).data();

                if (this.checked) {
                    Temp.SonumbList.push(Row.SoNumber);
                }
                else {
                    Temp.SonumbList.shift(Row.SoNumber);
                }
            });
        });
        $("#fileAttach").on("change", function () {
            var fileName = $(this).val().split("\\").pop();
            $("#txtAttachment").text(fileName);
        });
        $("#fileAttachmentSonumb").on("change", function () {
            var fileName = $(this).val().split("\\").pop();
            $("#txtAttachmentSonumb").text(fileName);
        });
        var start_year = new Date().getFullYear();
        for (var i = start_year; i > start_year - 5; i--) {
            $('#slYear').append('<option value="' + i + '">' + i + '</option>');
        }
        for (var i = start_year + 1; i <= start_year + 10; i++) {
            $('#slYear').append('<option value="' + i + '">' + i + '</option>');
        }
        var options = $('#slYear option');
        var arr = options.map(function (_, o) { return { t: $(o).text(), v: o.value }; }).get();
        arr.sort(function (o1, o2) { return o1.t > o2.t ? 1 : o1.t < o2.t ? -1 : 0; });
        options.each(function (i, o) {
            o.value = arr[i].v;
            $(o).text(arr[i].t);
        });
        $('#slYear').val(new Date().getFullYear()).trigger('change');
    }

}

var Table = {
    InflationRate: {
        Init: function () {
            $('#tblSummary').dataTable({
                columns: Table.InflationRate.Columns,
            });
        },
        Columns: [
            {
                data: null, fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    //console.log(oData);
                    var _link = "";
                    _link += "<button type='button' title='Edit' class='btn btn-xs blue EditInflationRate' data-ID='" + oData.ID + "' data-Year='" + oData.Year + "' data-Percentage='" + oData.Percentage + "' data-FileName='" + oData.FileName + "'><i class='fa fa-edit'></i>&nbsp;Edit</button>";
                    $(nTd).html(_link);
                },
                className: "dt-center",
                
                    width: "20%"
            },
            {
                title: "Year",
                data: "Year",
                className: "dt-center",
                width: "30%"
            },
            {
                data: "Percentage",
                className: "dt-center",
                width: "15%"
            },
            {
                data: "FilePath", mRender: function (data, type, full) {
                    return full.FilePath == null ? "" : "<center><button type='button' href='' class='btDownloadInflationRate btn btn-xs blue' data-FileName='" + full.FileName + "' data-FilePath='" + full.FilePath + "' data-ContentType='" + full.ContentType + "' onclick='Widget.Table.DownloadAttachmentIR(this);'><i class='fa fa-download'></i>&nbsp;Download</button></center>";
                },
                width: "35%"
            }
        ],
        Search: function () {
            var params = {
                Year: $("#tbxSearchYearIR").val() != "undefined" ? $("#tbxSearchYearIR").val().replace(/'/g, "''") : null
            }
            var tblSummary = $("#tblSummary").DataTable({
                "orderCellsTop": true,
                "proccessing": true,
                "serverSide": false,
                "language": {
                    "emptyTable": "No data available in table",
                },
                "ajax": {
                    "url": Url.API.gridIR,
                    "type": "POST",
                    "datatype": "json",
                    "data": params,
                },
                buttons: [
                       {
                           extend: 'copy', className: 'btn red btn-outline', exportOptions: {
                               columns: ':visible'
                           }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy'
                       },
                               {
                                   text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                                       Table.InflationRate.Export();
                                   }
                               },
                       { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
                ],
                "filter": false,
                "order": [[1, 'asc']],
                "lengthMenu": [[15, 30, 50, 100], ['15', '30', '50', '100']],
                "destroy": true,
                "columns": Table.InflationRate.Columns,
                dom: "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                fnPreDrawCallback: function () {
                    App.blockUI({ target: "#panelSummary", animate: !0 });
                },
                fnDrawCallback: function (result) {//method ini di hit sebanyak jumlah row
                    if (result.json != undefined && result.json.data != undefined) {// mastiin data udah ke load semua.
                        App.unblockUI('#panelSummary');

                        $('.EditInflationRate').click(function () {
                            //console.log(this);
                            var htmlscript = $('script#formCreateEditInflasi').html();
                            $('#createEditInflasi').html(htmlscript);

                            Control.InitCreateEditInflasi();
                            var FileName = this.getAttribute('data-FileName') == "null" ? "" : this.getAttribute('data-FileName');
                            Temp.IDIR = this.getAttribute('data-ID');
                            $("#txtPrecentage").val(this.getAttribute('data-Percentage'));
                            $("#txtAttachment").text(FileName);
                            $('#slYear').val(this.getAttribute('data-Year')).trigger('change');
                            $("#slYear").prop('disabled', true);
                            $('#mdlCreateEditInflasi').modal('show');
                            $('#mdlCreateEditInflasi .modal-title span').html('Edit Inflasi');
                        });
                    }
                }
            });
        },
        Export: function () {
            var year = $("#tbxSearchYearIR").val() == '' ? 0 : $("#tbxSearchYearIR").val();
            window.location.href = Url.Export.gridIR + "?periodYear=" + year;
        }
    },
    SonumbInflasi: {
        Init: function () {
            $('#tblSonumbInflasi').dataTable({
                columns: Table.SonumbInflasi.Columns,
            });
        },
        Columns: [
            {
                data: null, fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    //console.log(oData);
                    var _link = "";
                    _link += "<button type='button' title='Edit' class='btn btn-xs blue EditSonumbInfaltion' data-ID='" + oData.ID + "' data-Sonumb='" + oData.Sonumb + "' data-AmountRental='" + oData.AmountRental + "' data-AmountService='" + oData.AmountService + "' data-AmountInflation='" + oData.AmountInflation + "' data-InflationRate='" + oData.InflationRate + "'><i class='fa fa-edit'></i>&nbsp;Edit</button>";
                    _link += "<button type='button' title='Delete' class='btn btn-xs blue DeleteSonumbInfaltion' data-ID='" + oData.ID + "'><i class='fa fa-trash'></i>&nbsp;Delete</button>";
                    $(nTd).html(_link);
                },
                className: "dt-center",
            },
            { data: "Sonumb" },
            { data: "SiteID" },
            { data: "SiteName" },
            { data: "OperatorID" },
            { data: "SiteNameOpr" },
            { data: "CustomerInvoice" },
            { data: "CompanyInvoice" },
            { data: "RegionalName" },
            //{
            //    data: "AmountRental", className: 'dt-right',
            //    render: function (data) {
            //        return 'Rp.' + Number(data).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
            //    }
            //},
            //{
            //    data: "AmountService", className: 'dt-right',
            //    render: function (data) {
            //        return 'Rp.' + Number(data).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
            //    }
            //},
            {
                data: "AmountInflation", className: 'dt-right',
                render: function (data) {
                    return 'Rp.' + Number(data).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                }
            },
            //{
            //    data: "InflationRate", className: 'dt-right',
            //    render: function (data) {
            //        return 'Rp.' + Number(data).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
            //    }
            //},
            { data: "Status" }
        ],
        Search: function () {
            var params = {
                Sonumb: $("#tbxSearchSonumbSI").val() != "undefined" ? $("#tbxSearchSonumbSI").val().replace(/'/g, "''") : null,
                SiteID: $("#tbxSearchSiteIDSI").val() != "undefined" ? $("#tbxSearchSiteIDSI").val().replace(/'/g, "''") : null,
                SiteName: $("#tbxSearchSiteNameSI").val() != "undefined" ? $("#tbxSearchSiteNameSI").val().replace(/'/g, "''") : null,
                SiteIDOpr: $("#tbxSearchSiteIDOprSI").val() != "undefined" ? $("#tbxSearchSiteIDOprSI").val().replace(/'/g, "''") : null,
                SiteNameOpr: $("#tbxSearchSiteNameOprSI").val() != "undefined" ? $("#tbxSearchSiteNameOprSI").val().replace(/'/g, "''") : null,
                Customer: $("#tbxSearchCustInvSI").val() != "undefined" ? $("#tbxSearchCustInvSI").val().replace(/'/g, "''") : null,
                Company: $("#tbxSearchCompInvSI").val() != "undefined" ? $("#tbxSearchCompInvSI").val().replace(/'/g, "''") : null,
                Regional: $("#tbxSearchRegSI").val() != "undefined" ? $("#tbxSearchRegSI").val().replace(/'/g, "''") : null
            }
            var tblSummary = $("#tblSonumbInflasi").DataTable({
                "orderCellsTop": true,
                "proccessing": true,
                "serverSide": false,
                "language": {
                    "emptyTable": "No data available in table",
                },
                "ajax": {
                    "url": Url.API.gridSonumbInflasiList,
                    "type": "POST",
                    "datatype": "json",
                    "data": params,
                },
                buttons: [
                       {
                           extend: 'copy', className: 'btn red btn-outline', exportOptions: {
                               columns: ':visible'
                           }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy'
                       },
                               {
                                   text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                                       Table.SonumbInflasi.Export();
                                   }
                               },
                       { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }
                ],
                "filter": false,
                "order": [[1, 'asc']],
                "lengthMenu": [[15, 30, 50, 100], ['15', '30', '50', '100']],
                "destroy": true,
                "columns": Table.SonumbInflasi.Columns,
                dom: "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                fnPreDrawCallback: function () {
                    App.blockUI({ target: "#panelSummary", animate: !0 });
                },
                fnDrawCallback: function (result) {//method ini di hit sebanyak jumlah row
                    if (result.json != undefined && result.json.data != undefined) {// mastiin data udah ke load semua.
                        App.unblockUI('#panelSummary');

                        $('.EditSonumbInfaltion').click(function () {
                            $('#mdlEditSonumbInflasi').modal('show');
                            var ID = parseInt(this.getAttribute('data-ID'));
                            //console.log(parseInt(this.getAttribute('data-ID')));
                            Func.SeparationNumber("#txtRentalAmountModalEdit");
                            Func.SeparationNumber("#txtServiceAmountModalEdit");
                            Func.SeparationNumber("#txtInflasiAmountModalEdit");
                            $("#txtSonumbModalEdit").val(this.getAttribute('data-Sonumb'));
                            $("#txtRentalAmountModalEdit").val(Number(this.getAttribute('data-AmountRental')).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
                            $("#txtServiceAmountModalEdit").val(Number(this.getAttribute('data-AmountService')).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
                            $("#txtInflasiAmountModalEdit").val(Number(this.getAttribute('data-AmountInflation')).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
                            $("#txtInflationRateModalEdit").val(this.getAttribute('data-InflationRate'));
                            $('#btnEditMdlSonumbInflasi').on('click', function () {
                                var params = {
                                    ID: ID,
                                    AmountRental: parseFloat($("#txtRentalAmountModalEdit").val().replace(/\,/g, '')),
                                    AmountService: parseFloat($("#txtServiceAmountModalEdit").val().replace(/\,/g, '')),
                                    AmountInflation: parseFloat($("#txtInflasiAmountModalEdit").val().replace(/\,/g, '')),
                                    InflationRate: parseFloat($("#txtInflationRateModalEdit").val().replace(/\,/g, ''))
                                }
                                formData = new FormData();
                                formData.append("datainput", JSON.stringify(params));
                                if (Widget.ValidateEditSI(formData)) {
                                    if (confirm('Are you sure you want to edit?')) {
                                        var n = Ladda.create(document.querySelector("#btnEditMdlSonumbInflasi"));
                                        $.ajax({
                                            url: Url.API.SubmitSI,
                                            type: "POST",
                                            contentType: false, // Not to set any content header  
                                            processData: false, // Not to process data 
                                            data: formData,
                                            cache: !1,
                                            beforeSend: function () {
                                                n.start()
                                            }
                                        }).done(function (t) {
                                            var delayInMilliseconds = 500; //0,5 second
                                            console.log(t);
                                            setTimeout(function () {
                                                Common.CheckError.Object(t) && Common.Alert.Success("Sonumb Inflasi has been Edited");
                                            }, delayInMilliseconds);

                                            $('#mdlEditSonumbInflasi').modal('hide');
                                            Table.SonumbInflasi.Search();
                                            location.reload();

                                            n.stop();
                                        }).fail(function (t, i, r) {
                                            console.log(r);
                                            Common.Alert.Error(r);
                                            n.stop();
                                        });
                                    } else {

                                    }
                                }
                            });
                        });
                        $('.DeleteSonumbInfaltion').click(function () {
                            var params = {
                                ID: this.getAttribute('data-ID'),
                                UpdatedBy: "Delete"
                            }
                            formData = new FormData();
                            formData.append("datainput", JSON.stringify(params));
                            if (confirm('Are you sure you want to delete?')) {
                                $.ajax({
                                    url: Url.API.SubmitSI,
                                    type: "POST",
                                    contentType: false, // Not to set any content header  
                                    processData: false, // Not to process data 
                                    data: formData,
                                    cache: !1,
                                    beforeSend: function () {
                                        //n.start()
                                    }
                                }).done(function (t) {
                                    var delayInMilliseconds = 500; //0,5 second

                                    setTimeout(function () {
                                        Common.CheckError.Object(t) && Common.Alert.Success("Sonumb Inflasi has been Deleted.");
                                    }, delayInMilliseconds);

                                    Table.SonumbInflasi.Search();

                                    //n.stop()
                                }).fail(function (t, i, r) {
                                    Common.Alert.Error(r);
                                    //n.stop()
                                });
                            }
                            else {

                            }
                        });
                    }
                }
            });
        },
        Export: function () {
            var Sonumb = $("#tbxSearchSonumbSI").val() != "undefined" ? $("#tbxSearchSonumbSI").val().replace(/'/g, "''") : null,
            SiteID = $("#tbxSearchSiteIDSI").val() != "undefined" ? $("#tbxSearchSiteIDSI").val().replace(/'/g, "''") : null,
            SiteName = $("#tbxSearchSiteNameSI").val() != "undefined" ? $("#tbxSearchSiteNameSI").val().replace(/'/g, "''") : null,
            SiteIDOpr = $("#tbxSearchSiteIDOprSI").val() != "undefined" ? $("#tbxSearchSiteIDOprSI").val().replace(/'/g, "''") : null,
            SiteNameOpr = $("#tbxSearchSiteNameOprSI").val() != "undefined" ? $("#tbxSearchSiteNameOprSI").val().replace(/'/g, "''") : null,
            Customer = $("#tbxSearchCustInvSI").val() != "undefined" ? $("#tbxSearchCustInvSI").val().replace(/'/g, "''") : null,
            Company = $("#tbxSearchCompInvSI").val() != "undefined" ? $("#tbxSearchCompInvSI").val().replace(/'/g, "''") : null,
            Regional = $("#tbxSearchRegSI").val() != "undefined" ? $("#tbxSearchRegSI").val().replace(/'/g, "''") : null;

            window.location.href = Url.Export.gridSI + "?Sonumb=" + Sonumb + "&SiteID=" + SiteID + "&SiteName=" + SiteName + "&SiteIDOpr=" + SiteIDOpr + "&SiteNameOpr=" + SiteNameOpr + "&CustInv=" + Customer + "&ComInv=" + Company + "&Reg=" + Regional;
        }
    },
    gridSonumbList: {
        Init: function () {
            $('#tblModalAddSonumb').dataTable({
                columns: Table.gridSonumbList.Columns,
            });
        },
        Columns: [
            {
                data: null, fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    //console.log(oData);
                    var _link = "";
                    _link += "<label id='" + oData.SoNumber + "' class='mt-checkbox mt-checkbox-single mt-checkbox-outline'><input type='checkbox' class='checkboxes' /><span></span></label>";
                    $(nTd).html(_link);
                },
                className: "dt-center",
            },
            { data: "SoNumber" },
            { data: "SiteID" },
            { data: "SiteName" },
            { data: "OperatorID" },
            { data: "CompanyID" },
            { data: "RegionalName" }
        ],
        Search: function () {
            var params = {
                Sonumb: $("#tbxSearchSonumbMdl").val() != "undefined" ? $("#tbxSearchSonumbMdl").val().replace(/'/g, "''") : null,
                SiteID: $("#tbxSearchSiteIDMdl").val() != "undefined" ? $("#tbxSearchSiteIDMdl").val().replace(/'/g, "''") : null,
                SiteName: $("#tbxSearchSiteNameMdl").val() != "undefined" ? $("#tbxSearchSiteNameMdl").val().replace(/'/g, "''") : null,
                Customer: $("#tbxSearchOperatorIDMdl").val() != "undefined" ? $("#tbxSearchOperatorIDMdl").val().replace(/'/g, "''") : null,
                Company: $("#tbxSearchCompanyIDMdl").val() != "undefined" ? $("#tbxSearchCompanyIDMdl").val().replace(/'/g, "''") : null,
                Regional: $("#tbxSearchRegionalMdl").val() != "undefined" ? $("#tbxSearchRegionalMdl").val().replace(/'/g, "''") : null
            }
            var tblSummary = $("#tblModalAddSonumb").DataTable({
                "orderCellsTop": true,
                "proccessing": true,
                "serverSide": false,
                "language": {
                    "emptyTable": "No data available in table",
                },
                "ajax": {
                    "url": Url.API.gridSonumbList,
                    "type": "POST",
                    "datatype": "json",
                    "data": params,
                },
                buttons: [],
                "filter": false,
                "order": [[1, 'asc']],
                "lengthMenu": [[15, 30, 50, 100], ['15', '30', '50', '100']],
                "destroy": true,
                "columns": Table.gridSonumbList.Columns,
                dom: "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
                fnPreDrawCallback: function () {
                    App.blockUI({ target: "#mdlAddSonumb", animate: !0, baseZ: 2000 });
                },
                fnDrawCallback: function (result) {//method ini di hit sebanyak jumlah row
                    if (result.json != undefined && result.json.data != undefined) {// mastiin data udah ke load semua.
                        App.unblockUI('#mdlAddSonumb');

                        //$('.EditInflationRate').click(function () {
                        //    //console.log(this);
                        //    var FileName = this.getAttribute('data-FileName') == "null" ? "" : this.getAttribute('data-FileName');
                        //    Temp.IDIR = this.getAttribute('data-ID');
                        //    $("#txtPrecentage").val(this.getAttribute('data-Percentage'));
                        //    $("#txtAttachment").text(FileName);
                        //    $('#slYear').val(this.getAttribute('data-Year')).trigger('change');

                        //});
                    }
                }
            });
        },
        Export: function () {
            var year = $("#tbxSearchYearIR").val() == '' ? 0 : $("#tbxSearchYearIR").val();
            window.location.href = Url.Export.gridIR + "?periodYear=" + year;
        }
    }
}

var Widget = {
    Init: function(){
        Widget.Button.btnSaveIR();
        Widget.Button.btnResetIR();
        Widget.Button.btnSaveSI();
    },
    Table: {
        DownloadAttachmentIR: function (params) {
            var FilePath = params.getAttribute('data-FilePath');
            var ContentType = params.getAttribute('data-ContentType');
            var OriginalFileName = params.getAttribute('data-FileName');
            window.location.href = Url.Download.gridIR + "?FilePath=" + FilePath + "&ContentType=" + ContentType + "&OriginalFileName=" + OriginalFileName;
        }
    },
    Button: {
        btnSaveIR: function () {
            $('#btnSaveIR').unbind().on('click', function () {
                var params = {
                    Year: $("#slYear").val(),
                    Percentage: $("#txtPrecentage").val(),
                    ID: Temp.IDIR
                }
                formData = new FormData();
                formData.append("datainput", JSON.stringify(params));
                files = $("#fileAttach").get(0).files;
                if (files.length != 0) {
                    formData.append("attachment", files[0]);
                }
                if (Widget.ValidateSubmitIR(formData)) {
                    var confirmS = Temp.IDIR == 0 ? "Save" : "Edit";
                    if (confirm('Are you sure you want to ' + confirmS + '?')) {
                        var n = Ladda.create(document.querySelector("#btnSaveIR"));
                        $.ajax({
                            url: Url.API.SubmitIR,
                            type: "POST",
                            contentType: false, // Not to set any content header  
                            processData: false, // Not to process data 
                            data: formData,
                            cache: !1,
                            beforeSend: function () {
                                n.start()
                            }
                        }).done(function (t) {
                            var delayInMilliseconds = 500; //0,5 second

                            setTimeout(function () {
                                Common.CheckError.Object(t) && Common.Alert.Success("Inflation Rate has been " + confirmS + (confirmS == "Save" ? "d" : "ed."));
                            }, delayInMilliseconds);

                            Table.InflationRate.Search();
                            Temp.ClearIR();
                            $("#slYear").prop('disabled', false);
                            $('#mdlCreateEditInflasi').modal('hide');

                            n.stop()
                        }).fail(function (t, i, r) {
                            Common.Alert.Error(r);
                            n.stop()
                        });
                    } else {

                    }
                }
            });
        },
        btnResetIR: function () {
            $('#btnResetIR').unbind().on('click', function () {
                Temp.ClearIR();
            });
        },
        btnSaveSI: function () {
            $('#btnSaveModal').unbind().on('click', function () {
                var params = {
                    ID: 0,
                    SonumbList: Temp.SonumbList,
                    AmountRental: parseFloat($("#txtRentalAmountModal").val().replace(/\,/g, '')),
                    AmountService: parseFloat($("#txtServiceAmountModal").val().replace(/\,/g, '')),
                    AmountInflation: parseFloat($("#txtInflasiAmountModal").val().replace(/\,/g, '')),
                    InflationRate: parseFloat($("#txtInflationRateModal").val().replace(/\,/g, '')),
                    SonumbListMst: $("#tblSonumbInflasi").DataTable().data().toArray()
                }
                formData = new FormData();
                formData.append("datainput", JSON.stringify(params));
                files = $("#fileAttachmentSonumb").get(0).files;
                if (files.length != 0) {
                    formData.append("attachment", files[0]);
                }
                if (Widget.ValidateSubmitSI(formData)) {
                    if (confirm('Are you sure you want to save?')) {
                        var n = Ladda.create(document.querySelector("#btnSaveModal"));
                        $.ajax({
                            url: Url.API.SubmitSI,
                            type: "POST",
                            contentType: false, // Not to set any content header  
                            processData: false, // Not to process data 
                            data: formData,
                            cache: !1,
                            beforeSend: function () {
                                n.start()
                            }
                        }).done(function (t) {
                            var delayInMilliseconds = 500; //0,5 second

                            setTimeout(function () {
                                Common.CheckError.Object(t) && Common.Alert.Success("Sonumb Inflasi has been Saved");
                            }, delayInMilliseconds);

                            $('#mdlAddSonumb').modal('hide');
                            Table.SonumbInflasi.Search();
                            Table.gridSonumbList.Search();
                            Temp.SonumbList = [];
                            Temp.ClearSI();
                            location.reload();
                            //console.log(t);
                            //Temp.ClearIR();

                            n.stop()
                        }).fail(function (t, i, r) {
                            Common.Alert.Error(r);
                            Temp.ClearSI();
                            n.stop();
                        });
                    } else {

                    }
                }
            });
        }
    },
    ValidateSubmitIR: function (formData) {
        var n = 0, s = "", p = JSON.parse(formData.get("datainput"));
        var table = $("#tblSummary").DataTable().data().toArray();
        var listyear = [];
        for (var i = 0; i < table.length; i++) {
            listyear.push(table[i].Year);
        }
        var file = formData.get("attachment");
        if (file != null) {
            if (Func.FileSizeToMB(file.size) > 2) {
                n += 1; s += "\n - Attachment file Can`t bigger then 2MB";
            }
        }
        if (Temp.IDIR == 0 && listyear.includes(parseInt(p.Year.trim()))) {
            n += 1; s += "\n - Year already exist.";
        }
        if (p.Year.trim() == "" || p.Year.trim() == null || p.Year.trim() == 0) {
            n += 1; s += "\n - Year can't null.";
        }
        if (p.Percentage.trim() == "" || p.Percentage.trim() == null || p.Percentage.trim() == 0) {
            n += 1; s += "\n - Percentage can't null.";
        }
        if (n > 0) {
            Common.Alert.Warning(s);
            return !1
        }
        return !0;
    },
    ValidateSubmitSI: function (formData) {
        var n = 0, s = "", p = JSON.parse(formData.get("datainput"));
        var file = formData.get("attachment");
        //console.log(file.name.split('.').pop());
        if (file != null) {
            if (Func.FileSizeToMB(file.size) > 2) {
                n += 1; s += "\n - Attachment file Can`t bigger then 2MB";
            }
            if (file.name.split('.').pop() != "xlsx" && file.name.split('.').pop() != "xls") {
                n += 1; s += "\n - Attachment file only file Excel.";
            }
        }
        if (Temp.SonumbList.length == 0 && file == null) {
            n += 1; s += "\n - Select at least one Sonumb.";
        }
        if (Temp.SonumbList.length == 0 && (p.AmountRental == "" || p.AmountRental == null || p.AmountRental == 0) && file == null) {
            n += 1; s += "\n - Rental Amount can't null.";
        }
        if (Temp.SonumbList.length == 0 && (p.AmountService == "" || p.AmountService == null || p.AmountService == 0) && file == null) {
            n += 1; s += "\n - Service Amount can't null.";
        }
        if (Temp.SonumbList.length == 0 && (p.AmountInflation == "" || p.AmountInflation == null || p.AmountInflation == 0) && file == null) {
            n += 1; s += "\n - Inflation Amount can't null.";
        }
        if (Temp.SonumbList.length == 0 && (p.InflationRate == "" || p.InflationRate == null || p.InflationRate == 0) && file == null) {
            n += 1; s += "\n - Inflation Rate can't null.";
        }
        if (n > 0) {
            Common.Alert.Warning(s);
            return !1
        }
        return !0;
    },
    ValidateEditSI: function (formData) {
        var n = 0, s = "", p = JSON.parse(formData.get("datainput"));

        if (p.AmountRental == "" || p.AmountRental == null || p.AmountRental == 0) {
            n += 1; s += "\n - Rental Amount can't null.";
        }
        if (p.AmountService == "" || p.AmountService == null || p.AmountService == 0) {
            n += 1; s += "\n - Service Amount can't null.";
        }
        if (p.AmountInflation == "" || p.AmountInflation == null || p.AmountInflation == 0) {
            n += 1; s += "\n - Inflation Amount can't null.";
        }
        if (p.InflationRate == "" || p.InflationRate == null || p.InflationRate == 0) {
            n += 1; s += "\n - Inflation Rate can't null.";
        }
        if (n > 0) {
            Common.Alert.Warning(s);
            return !1
        }
        return !0;
    }
}

var Func = {
    FileSizeToMB: function (fileSize) {
        return fileSize / (1024 * 1024);
    },
    SeparationNumber: function (selector) {
        $(selector).on('keyup', function (evt) {
            if (evt.which != 110 && evt.which != 190) {
                var n = parseFloat($(this).val().replace(/\,/g, ''), 10);
                var value = n.toLocaleString();
                if (value == "NaN") {
                    $(this).val('');
                } else {
                    $(this).val(n.toLocaleString());
                }

            }
        });
    },
}

var Url = {
    API: {
        gridIR: "/api/Inflasi/InflationRateGridList",
        gridSonumbInflasiList: "/api/Inflasi/SonumbInflasiGridList",
        gridSonumbList: "/api/Inflasi/SonumbGridList",
        SubmitIR: "/api/Inflasi/SubmitIR",
        SubmitSI: "/api/Inflasi/SubmitSI"
    },
    Export: {
        gridIR: "/Inflasi/GridIR/Export",
        gridSI: "/Inflasi/GridSI/Export"
    },
    Download: {
        gridIR: "/Inflasi/DownloadAttachmentIR"
    }
}

var Temp = {
    IDIR: 0,
    SonumbList: [],
    ClearIR: function () {
        $("#txtPrecentage").val('');
        $("#txtAttachment").text('');
        $('#slYear').val(new Date().getFullYear()).trigger('change');
        Temp.IDIR = 0;
        $("#slYear").prop('disabled', false);
    },
    ClearSI: function () {
        $("#txtRentalAmountModal").val('');
        $("#txtServiceAmountModal").val('');
        $('#txtInflasiAmountModal').val('');
        $("#txtInflationRateModal").val('');
        $("#txtAttachmentSonumb").text('');
    }
}