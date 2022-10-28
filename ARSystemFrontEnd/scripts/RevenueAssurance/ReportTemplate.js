var CustomerID = "";
var StipCategory = "";
var ProductType = "";
var PrintType = "";
var HtmlString = "";
var LogoRight = "";
var LogoLeft = "";
var DataSelected = {};
var ImageSave = {};
var LogData = {};

jQuery(document).ready(function () {
    Control.BindingSelectProductType();
    Control.BindingSelectCustomer();
    Table.Init();
    Table.Search();
    $('.panelDetailPO').hide();

    $("#btBackToList").unbind().click(function () {
        $('.panelDetailPO').fadeOut();
        $('.panelSearchResult').fadeIn(500);
    });

    $("#btSearchData").unbind().click(function () {
        
        Form.ViewReport();
    });

    $('#btViewHtml').unbind().click(function () {
        $('.ReportView').html("");
        var report = $('.textarea-editor').summernote('code');
        $('.ReportView').append(report);
        //LogoRight = $('#CurrentLogoRight').attr('src');
        //LogoLeft = $('#CurrentLogoLeft').attr('src');
        $('#ViewLogoLeft').attr('src', $('#CurrentLogoLeft').attr('src')).height('10%').width('10%');
        $('#ViewLogoRight').attr('src', $('#CurrentLogoRight').attr('src')).height('10%').width('10%');
        $('#mdlView').modal('show');
    });

    $("#btSaveData").unbind().click(function () {
        Form.SaveReport();
    });

    $("#btAdd").unbind().click(function () {
        $('#btSearchData').hide();
        $('.panelDetailPO').fadeIn(500);
        $('.panelSearchResult').hide();
        Form.ClearInput();
    });

    $('.textarea-editor').summernote({
        height: 300, // set editor height  
        minHeight: null, // set minimum height of editor  
        maxHeight: null, // set maximum height of editor  
        focus: true // set focus to editable area after initializing summernote  
    });

    $(".textarea-editor").summernote({
        onpaste: function (e) {
            var bufferText = ((e.originalEvent || e).clipboardData || window.clipboardData).getData('Text');
            e.preventDefault();
            setTimeout(function () {
                document.execCommand('insertText', false, bufferText);
                $(this).parent().siblings('.summernote').destroy();
            }, 10);
        }
    });
});

var Table = {
    Init: function () {
        var tblRaw = $('#tblRaw').DataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });

        $("#tblRaw tbody").on("click", "a.btDetail", function (e) {
            e.preventDefault();
            var table = $("#tblRaw").DataTable();
            var data = table.row($(this).parents("tr")).data();
            if (data != null) {
                DataSelected = data;
                $('.panelDetailPO').fadeIn(500);
                $('.panelSearchResult').hide();
                var pathright = data.LogoPathRight.replace("~","..\\..\\..");
                var pathleft = data.LogoPathLeft.replace("~", "..\\..\\..");
                $("#CurrentLogoLeft").attr("src", pathleft);
                $("#CurrentLogoRight").attr("src", pathright);
                $('.textarea-editor').summernote("code", data.HtmlString);
                
                LogoLeft = data.LogoPathLeft;
                LogoRight = data.LogoPathRight;
                $('#CustomerID').val(data.CustomerID).trigger("change");
                $('#StipCategory').val(data.StipCategory);
                if (data.ProductType == "ALL") {
                    $('#ProductType').val(data.ProductType).trigger("change");
                }
                else {
                    $('#ProductType').val(data.ProductID).trigger("change");
                }
                
                $('#PrintType').val(data.PrintType).trigger('change');

                $('#MarginTop').val(data.MarginTop);
                $('#MarginBottom').val(data.MarginBottom);
                $('#MarginLeft').val(data.MarginLeft);
                $('#MarginRight').val(data.MarginRight);
                $('#IsPotrait').val(data.IsPotrait);
                $('#UseQR').val(data.UseQR);

                $('#btSearchData').show();

                LogData.PK = data.ID;
                LogData.Module = "Custom Report";
                LogData.ItemBefore = JSON.stringify(DataSelected);
            }
        });

        $(window).resize(function () {
            $("#tblRaw").DataTable().columns.adjust().draw();
        });
    },

    Search: function () {


        var tblSiteInfo = $("#tblRaw").DataTable({
            "deferRender": true,
            "proccessing": true,
            "serverSide": false,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/api/CustomReport/GetReport",
                "type": "POST",
                "datatype": "json",
            },
            "lengthMenu": [[-1], ['All']],
            "columns": [
                {
                    data: "ID", mRender: function (data, type, full) {
                        return "<a class='btDetail btn btn-sm btn-info'> <i class='fa fa-edit'></i></a>";
                    }
                },
                { data: "CustomerID" },
                { data: "StipCategory" },
                { data: "ProductType" },
                { data: "PrintType" }
            ],
            "buttons": [],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "order": [],
            "destroy": true,
            "fnDrawCallback": function (oSettings, json) {
                
            },
            "footerCallback": function (row, data, start, end, display) {
                
            },
        });

        $(window).resize(function () {
            $("#tblRaw").DataTable().columns.adjust().draw();
        });
    }
}

var Form = {

    ShowImage: function (input,target) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#'+ target +'')
                    .attr('src', e.target.result)
                    .width('10%')
                    .height('10%');
            };

            if (target.includes("Right")) {
                LogoRight = "Change";
            } else {
                LogoLeft = "Change";
            }

            reader.readAsDataURL(input.files[0]);
        }
    },

    ViewReport: function () {
        //var data = {ID : DataSelected.ID}
        //$.ajax({
        //    type: 'GET',
        //    dataType: 'json',
        //    cache: false,
        //    url: '/RANewBaps/ViewPrint',
        //    data: data,
        //    error: function (jqXHR, textStatus, errorThrown) {
        //        alert('error');
        //    }
        //});
        window.location.href = "/RevenueAssurance/ViewPrint?ID=" + DataSelected.ID;
    },

    SaveReport: function () {
        var error = true;
        var formData = new FormData();
        
        var fileInput = document.getElementById("LogoLeft");
        
        if (LogoLeft == "Change") {
            if (document.getElementById("LogoLeft").files.length != 0) {

                fsFileName = fileInput.files[0].name;
                formData.append("LogoLeft", fsFileName);

                fsFile = fileInput.files[0];

                fsExtension = fsFileName.split('.').pop().toUpperCase();

                if (fsExtension != "JPG" && fsExtension != "JPEG" && fsExtension != "PNG") {
                    Common.Alert.Warning("Please upload an JPG or PNG File."); return;
                }
                else {

                    formData.append('LogoLeft', fsFile);
                }

                errors = false;
            }
        }
        else {
            formData.append('LogoLeft', null);
        }
        
        if (LogoRight == "Change") {
            fileInput = document.getElementById("LogoRight");
            if (document.getElementById("LogoRight").files.length != 0) {

                fsFileName = fileInput.files[0].name;
                formData.append("LogoRight", fsFileName);

                fsFile = fileInput.files[0];

                fsExtension = fsFileName.split('.').pop().toUpperCase();

                if (fsExtension != "JPG" && fsExtension != "JPEG" && fsExtension != "PNG") {
                    Common.Alert.Warning("Please upload an JPG or PNG File."); return;
                }
                else {

                    formData.append('LogoRight', fsFile);
                }

                errors = false;
            }
        }
        else {
            formData.append('LogoRight', null);
        }
        
        if (LogoLeft == "Change" || LogoRight == "Change") {
            $.ajax({
                url: '/api/CustomReport/UploadFile',
                type: 'POST',
                data: formData,
                async: false,
                cache: false,
                contentType: false,
                processData: false
            }).done(function (data, textStatus, jqXHR) {
                if (data != null) {
                    error = false;
                    ImageSave = data;
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
            });
        } else {
            error = false;
        }
        

        if (!error) {
            if (LogoLeft == "Change")
                DataSelected.LogoPathLeft = ImageSave.LogoLeft;

            if (LogoRight == "Change")
                DataSelected.LogoPathRight = ImageSave.LogoRight;

            DataSelected.HtmlString = $('.textarea-editor').summernote("code");
            DataSelected.CustomerID = $('#CustomerID').val();
            DataSelected.StipCategory = $('#StipCategory').val();
            DataSelected.ProductType = $('#ProductType').val();
            DataSelected.PrintType = $('#PrintType').val();
            DataSelected.MarginBottom = $('#MarginBottom').val();
            DataSelected.MarginTop = $('#MarginTop').val();
            DataSelected.MarginLeft = $('#MarginLeft').val();
            DataSelected.MarginRight = $('#MarginRight').val();
            DataSelected.IsPotrait = $('#IsPotrait').val();
            DataSelected.UseQR = $('#UseQR').val();

            LogData.ItemAfter = JSON.stringify(DataSelected);

            var dataparam = {
                Report: DataSelected,
                Log: LogData
            }

            $.ajax({
                url: "/api/CustomReport/SaveReport",
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(dataparam),
                cache: false,
            }).done(function (data, textStatus, jqXHR) {
                if (data != null) {
                    Common.Alert.Success("Success To Save Data!")
                    $('.panelDetailPO').fadeOut();
                    $('.panelSearchResult').fadeIn(500);
                    Table.Search();
                    Form.ClearInput();
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
            });
        }
        
    },

    ClearInput: function(){
        $('#CustomerID').val("");
        $('#StipCategory').val("");
        $('#ProductType').val("");
        $('#PrintType').val("");
        $('#LogoRight').val(null);
        $('#LogoLeft').val(null);
        $('.textarea-editor').summernote("code", "");
        $("#CurrentLogoLeft").attr("src", "");
        $("#CurrentLogoRight").attr("src", "");
    }
}

var Control = {
    BindingSelectCustomer: function () {
        var id = "#CustomerID";
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
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },

    BindingSelectProductType: function () {
        var id = "#ProductType";
        $.ajax({
            url: "/api/MstDataSource/TenantType",
            type: "GET"
        })
        .done(function (data, textStatus, jqXHR) {
            $(id).html("<option></option>")
            $(id).append("<option value='ALL'>ALL</option>");

            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    $(id).append("<option value='" + $.trim(item.Value) + "'>" + item.Text + "</option>");
                })
            }
            $(id).select2({ placeholder: "Select Product", width: null });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
}