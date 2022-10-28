$(function () {
    $("#btUploadSiteTsel").unbind().click(function () {
        ModalUploadInflasi.Reset();
        $("#uplDepartmentType").val(settingVariable.deptCodeTsel);
        $("#mdlUploadInflasi .modal-title").html('Upload Inflasi Rec. TSEL');
        ModalUploadInflasi.Init();
    });
    $("#btUploadSiteNonTsel").unbind().click(function () {
        ModalUploadInflasi.Reset();
        $("#uplDepartmentType").val(settingVariable.deptCodeNonTsel);
        $("#mdlUploadInflasi .modal-title").html('Upload Inflasi Rec. Non TSEL');

        ModalUploadInflasi.Init();
    });
    $("#btUploadSiteNewBaps").unbind().click(function () {
        ModalUploadInflasi.Reset();
        $("#uplDepartmentType").val(settingVariable.deptCodeNewBaps);
        $("#mdlUploadInflasi .modal-title").html('Upload Inflasi New BAPS');

        ModalUploadInflasi.Init();
    });
    $("#btUploadSiteProd").unbind().click(function () {
        ModalUploadInflasi.Reset();
        $("#uplDepartmentType").val(settingVariable.deptCodeNewProduct);
        $("#mdlUploadInflasi .modal-title").html('Upload Inflasi Rec. New Product & Other');

        ModalUploadInflasi.Init();
    });


})
var ModalUploadInflasi = {
    Init: function () {
        $("#btCloseUpload").unbind().click(function (e) {
            e.preventDefault();
            $('#mdlUploadInflasi').modal('hide');
        });
        $("#btSubmitUpload").unbind().click(function (e) {
            e.preventDefault();
            ModalUploadInflasi.SubmitUpload();
        });
        $("#mdlUploadInflasi").modal({ backdrop: 'static', keyboard: false }).show();
    },
    Reset: function () { //Modal.Reset
        var srt = $("script#mdlUploadInflasiHtml").html();
        $("#mdlUploadInflasiContainer").html(srt);
    },
    SubmitUpload: function () {
        var l = Ladda.create(document.querySelector("#btSubmitUpload"))
        l.start();
        App.blockUI({
            target: "#mdlUploadInflasi .modal-content", boxed: true
        });
        var fileUpload = $("#formUploadInputTarget input[name='postFileInputBulky']").get(0);
        var files = fileUpload.files;
        if (files.length <= 0) {
            l.stop(); App.unblockUI('#mdlUploadInflasi .modal-content');
            Common.Alert.Warning("No file selected.")
        }
        var data = new FormData();
        data.append("FileUpload", files[0]);
        data.append("FileUploadName", files[0].name);
        data.append("DepartmentCode", $("#uplDepartmentType").val());
        var url = ($("#uplDepartmentType").val() == settingVariable.deptCodeNewBaps) ? "/api/ApiInputTarget/UploadExcelTargetNewBaps" : "/api/ApiInputTarget/UploadExcelTargetRecurring";
        $("#uploadTargetErrorLogs").html("");
        $("#uploadErrorLogsAlert").addClass('hidden');
        $.ajax({
            url: url,
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            l.stop(); App.unblockUI('#mdlUploadInflasi .modal-content');
            if (typeof data.Message != "undefined") {
                //Common.Alert.Error(data.Message);
                $("#uploadTargetErrorLogs").html(data.Message);
                $("#uploadErrorLogsAlert").removeClass('hidden');
            } else {
                Common.Alert.Success("Data has been saved!")
                $('#mdlUploadInflasi').modal('hide');

                TableHistory.SearchHistory();
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            if (textStatus == "error" && errorThrown == "") {
                Common.Alert.Warning("Cannot upload file, please try to reselect file again, it's may cause the file has been updated at external resource.")
                l.stop(); App.unblockUI('#mdlUploadInflasi .modal-content');
            } else {
                Common.Alert.Error(errorThrown)
                l.stop(); App.unblockUI('#mdlUploadInflasi .modal-content');
            }
        });
    }


}
