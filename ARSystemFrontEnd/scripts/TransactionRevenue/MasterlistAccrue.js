Data = {};

jQuery(document).ready(function () {
    Control.LoadCompany();
    Control.LoadOperatorRevSys();
    Form.Init();

    $("#btSearch").unbind().click(function () {
        Form.bind();
    });

    $("#btReset").unbind().click(function () {
        Form.reset();
        Form.bind();
    });


    $('#tblData').on('click', '.btSonumb', function (e) {

        var table = $('#tblData').DataTable();
        var tr = $(this).closest('tr');
        var data = table.row(tr).data();
        document.getElementById('inHiddenSonumb').value = data.Sonumb.toString().trim();

        var $radios = $('input:radio[name=rbAction]');
        if ($radios.is(':checked') === false) {
            $radios.filter('[value=opHold]').prop('checked', true);
        }

        document.getElementById('sldateFrom').value = "";
        document.getElementById('sldateEnd').value = "";
        document.getElementById('txRemaks').value = "";
        document.getElementById("sldateEnd").style.visibility = "visible";
        document.getElementById("fileToUpload1").value = "";
        $('#mdlDetail').modal('toggle');

    });


    $('#mdlDetail input[name=rbAction]').on('change', function () {
        var statusAction = $('input[name=rbAction]:checked', '#mdlDetail').val().toString().trim();

        if (statusAction == "Hold") {
            document.getElementById('sldateFrom').value = "";
            document.getElementById('sldateEnd').value = "";
            document.getElementById("sldateEnd").style.visibility = "visible";
        }
        else {
            document.getElementById("sldateEnd").style.visibility = "hidden";
        }
    });


    $("#btSubmit").unbind().on("click", function (e) {
        Form.SaveModal()
        Form.bind();
    });


    $("#btCancel").unbind().click(function () {
        Form.clearModal();
    });


});


var Form = {
    Init: function () {
        $(".panelSearchZero").hide();
        $(".panelSearchResult").hide();

        var tblSummaryData = $('#tblData').dataTable({
            "filter": false,
            "destroy": true,
            "data": []
        });


        Form.bind();
    },
    validationData: function () {
        var messege = "";
        var rb = $('input[name=rbAction]:checked', '#mdlDetail').val();
        if ($('input[name=rbAction]:checked', '#mdlDetail').val() == "" || $('input[name=rbAction]:checked', '#mdlDetail').val() == undefined) {
            messege = "Please Check Radio Button";
        }
        else if ($("#sldateFrom").val().toString().trim() == "" || $("#sldateFrom").val().toString().trim() == undefined) {
            messege = "Please Insert Date From";
        }
        else {
            messege = "";
        }
        return messege;
    },
    SaveModal: function () {
        var sc = $('input[name=rbAction]:checked', '#mdlDetail').val();

        var dateE = "";
        var slEndDate;
        if (sc == "Hold") {
            slEndDate = $("#sldateEnd").val().toString().trim();
            dateE = new Date(slEndDate);
        }
        else {
            slEndDate = "";
            dateE = new Date("9999-12-01");
        }
        var dateS = new Date($("#sldateFrom").val().toString().trim());

        if (Form.validationData() == "") {
            if (sc == "Hold" && dateS > dateE) {
                alert("Date End must be greater from Date Start")
            }
            else {
                var formData = new FormData();
                formData.append("sonumb", $("#inHiddenSonumb").val().toString().trim());
                formData.append("action", $('input[name=rbAction]:checked', '#mdlDetail').val());
                formData.append("DateStart", $("#sldateFrom").val().toString().trim());
                formData.append("DateEnd", slEndDate);
                formData.append("Remaks", $("#txRemaks").val().toString().trim());

                var fileInput = document.getElementById("fileToUpload1");
                var file = null;
                if (fileInput.files != undefined) {
                    file = fileInput.files[0];
                }

                if (file != null && file != undefined) {
                    formData.append("Path", file);
                }


                //formData.append("Path", $("#taRemark").val());

                //var param = {
                //    sonumb: $("#inHiddenSonumb").val().toString().trim(),
                //    action: $('input[name=rbAction]:checked', '#mdlDetail').val(),
                //    DateStart: $("#sldateFrom").val().toString().trim(),
                //    DateEnd: slEndDate,
                //    Remaks: $("#txRemaks").val().toString().trim(),
                //    Path: $("#fileToUpload").val().toString().trim(),
                //}
                var l = Ladda.create(document.querySelector("#btSubmit"))
                $.ajax({
                    url: "/api/RevenueSystem/SaveRevSysHoldStopAccrue",
                    type: "POST",
                    dataType: "json",
                    data: formData,
                    contentType: false,
                    processData: false,
                    beforeSend: function (xhr) {
                        l.start();
                    }
                }).done(function (data, textStatus, jqXHR) {

                    if (data.success) {
                        //console.log(data);
                        Common.Alert.Success("Sonumb has been submitted");
                        //Table.Reset();
                        //Form.Done();
                    }
                    l.stop()
                })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown)
                l.stop()
            })

                Form.clearModal();
            }
        }
        else {
            alert(Form.validationData());
        }







    },
    clearModal: function () {
        $("#opHold").prop("checked", true);
        document.getElementById('sldateFrom').value = "";
        document.getElementById('sldateEnd').value = "";
        document.getElementById('txRemaks').value = "";
        document.getElementById("sldateEnd").style.visibility = "visible";
        document.getElementById("fileToUpload1").value = "";
        $('#mdlDetail').modal('hide');
    },
    bind: function () {
        var columns = [];
        columns = [
              { data: "Sonumb", render: function (val, type, full) { return helper.RenderLink(val) } },
              { data: "SiteName" },
              { data: "SiteID" },
              { data: "Operator" },
              { data: "Company" },
              { data: "RFI_Date" },
              { data: "SLD_Date" },
              { data: "Status_Tenant" },
              { data: "Is_Tagih" },
              { data: "date_start" },
              { data: "date_END" }
        ];

        var selector = "#tblData";
        Form.draw(selector, columns);
    },
    draw: function (selector, columns) {

        var l = Ladda.create(document.querySelector("#btSearch"))
        l.start();

        var params = {
            companyID: $("#slCompany").val().toString().trim(),
            operatorID: $("#slOperator").val().toString().trim(),
            sonumb: $("#inSonumb").val().toString().trim()
        }

        var tblData = $("#tblData").DataTable({
            dom: 'Bfrtip',
            "deferRender": true,
            "proccessing": true,
            "serverSide": true,
            "language": {
                "emptyTable": "No data available in table"
            },
            "ajax": {
                "url": "/Api/RevenueSystem/GetRevSysDataHoldStopAccrue",
                "type": "POST",
                "datatype": "json",
                "data": params
            },
            buttons: [
               { extend: 'copy', className: 'btn red btn-outline', exportOptions: { columns: ':visible' }, text: '<i class="fa fa-copy" title="Copy"></i>', titleAttr: 'Copy' },
               {
                   text: '<i class="fa fa-file-excel-o"></i>', titleAttr: 'Export to Excel', className: 'btn yellow btn-outline', action: function (e, dt, node, config) {
                       var l = Ladda.create(document.querySelector(".yellow"));
                       l.start();
                       Form.Export(params)
                       l.stop();
                   }
               },
               { extend: 'colvis', className: 'btn dark btn-outline', text: '<i class="fa fa-columns"></i>', titleAttr: 'Show / Hide Columns' }

            ],
            "filter": false,
            "lengthMenu": [[5, 10, 25, 50], ['5', '10', '25', '50']],
            "columns": columns,
            "columnDefs": [{ "targets": [0], "orderable": false }],
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "fnPreDrawCallback": function () {
                App.blockUI({ target: ".panelSearchResult", boxed: true });
            },
            "fnDrawCallback": function () {
                if (Common.CheckError.List(tblData.data())) {
                    $(".panelSearchBegin").hide();
                    $(".panelSearchResult").fadeIn();
                }
                l.stop(); App.unblockUI('.panelSearchResult');
            },
            "order": [],
            "destroy": true
        })





    },
    reset: function () {
        $("#slOperator").val($('#slOperator option:first-child').val()).trigger("change");
        $("#slCompany").val($('#slCompany option:first-child').val()).trigger("change");
        $("#inSonumb").val("");

    },
    Export: function (param) {
        window.location.href = "/RevenueSystem/MasterListAccrue/Export?companyID=" + param.companyID + "&operatorID=" + param.operatorID + "&sonumb=" + param.sonumb;
    }


};


var Control = {
    LoadCompany: function () {
        $.ajax({
            url: "/api/RevenueSystem/GetRevSysCompany",
            type: "POST",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null) {

                        $("#slCompany").append("<option value='" + item.ValueId + "'>" + item.ValueDesc + "</option>");
                    }
                });
            }
            $("#slCompany").select2({ width: null });
        });
    },
    LoadOperatorRevSys: function () {
        $.ajax({
            url: "/api/RevenueSystem/GetOperatorRevSys",
            type: "POST",
            async: false
        })
        .done(function (data, textStatus, jqXHR) {
            if (Common.CheckError.List(data)) {
                $.each(data, function (i, item) {
                    if (item != null) {
                        $("#slOperator").append("<option value='" + item.ValueId + "'>" + item.ValueDesc + "</option>");
                    }
                });
            }
            $("#slOperator").select2({ width: null });
        });
    }

};

var helper = {
    RenderLink: function (val) {
        return "<a class='btSonumb'>" + val + "</a>";

    },

}