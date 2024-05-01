function DeleteProvider(phyid) {
    $.ajax({
        url: '/AdminPartials/DeleteProvider',
        method: 'POST',
        data: { phyid: phyid },
        success: function (response) {
            if (response) {
                toastr.success("Provider Deleted");
            }
            else {
                toastr.error("Some thing Went Wrong");
            }
            $("#Provider-drop-down").click();
        }
    })
}
//asp form was handled in adminscript
$("#ProviderDocuments").on("submit", function (e) {
    e.preventDefault();
    var formdata = new FormData();
    formdata.append("file1", $("#actual-btn1")[0].files[0]);
    formdata.append("file2", $("#actual-btn2")[0].files[0]);
    formdata.append("file3", $("#actual-btn3")[0].files[0]);
    formdata.append("file4", $("#actual-btn4")[0].files[0]);
    formdata.append("file5", $("#actual-btn5")[0].files[0]);

    if ($("#actual-btn1").val() == "" && $("#actual-btn2").val() == "" && $("#actual-btn3").val() == "" && $("#actual-btn4").val() == "" && $("#actual-btn5").val() == "") {
        toastr.error("Please Select Any File")
        return false;
    }
    $.ajax({
        url: '/AdminPartials/ProviderDocumentsUpload',
        data: formdata,
        method: 'POST',
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.status) {
                toastr.success("file Uploaded");
            }
            else {
                toastr.error("Some Thing Went Wrong");
            }
            EditPhysicianProfile(response.phyid);
        }
    });
});
function ProviderBack() {
    if (localStorage.getItem("providerfrom") == "user") {
        $("#User-Access").click();
    }
    else if (localStorage.getItem("providerfrom") == "Provider") {
        $("#Provider-drop-down").click();
    }

}
var controller = "AdminPartials";

if (localStorage.getItem("CurrentAccount") == "Provider") {
    controller = "ProviderAccount";
}
function agreementdocBtn(e, phyid, filetype) {
    //show model with image
    e = e || window.event;
    e.preventDefault();
    $.ajax({
        url: '/' + controller + '/showProviderDocuments',
        method: 'POST',
        data: {
            phyid: phyid,
            fileType: filetype
        },
        success: function (response) {
            if (response != "") {
                showDocs(response);
                toastr.success("fetched Successfully");
            }
            else {
                toastr.error("file not found!");
            }
        }
    });
    return false;
}
function showDocs(filename) {
    $("#loadImageHere").empty();
    $("#loadImageHere").append('<img src="/uploads/ProviderDocument/' + filename + '" alt="Document" style="height: 500px; width: 360px;">');
    $("#ProvideDocument").modal("show");
}

$("#actual-btn1").on("change", function () {
    $("#ICAcheckbox").prop("checked", true);
})
///on uncheck remove document 
$("#ICAcheckbox").on("change", function () {
    if ($(this).is(":checked")) {
        if ($("#actual-btn1").val() == '') {
            toastr.error("please select file first")
            $("#ICAcheckbox").prop("checked", false);
        }
    }
    else {
        if ($("#actual-btn1").val() != '') {
            $("#actual-btn1").val('');
            toastr.success("file removed form select")
        }
    }
})
$("#actual-btn2").on("change", function () {
    $("#Background-checkbox").prop("checked", true);
})
$("#Background-checkbox").on("change", function () {
    if ($(this).is(":checked")) {
        if ($("#actual-btn2").val() == '') {
            toastr.error("please select file first")
            $("#Background-checkbox").prop("checked", false);
        }
    }
    else {
        if ($("#actual-btn2").val() != '') {
            $("#actual-btn2").val('');
            toastr.success("file removed form select")
        }
    }
})
$("#actual-btn3").on("change", function () {
    $("#HIPAACompliancecheckbox").prop("checked", true);
})
$("#HIPAACompliancecheckbox").on("change", function () {
    if ($(this).is(":checked")) {
        if ($("#actual-btn3").val() == '') {
            toastr.error("please select file first")
            $("#HIPAACompliancecheckbox").prop("checked", false);
        }
    }
    else {
        if ($("#actual-btn3").val() != '') {
            $("#actual-btn3").val('');
            toastr.success("file removed form select")
        }
    }
})
$("#actual-btn4").on("change", function () {
    $("#NDAcheckbox").prop("checked", true);
})
$("#NDAcheckbox").on("change", function () {
    if ($(this).is(":checked")) {
        if ($("#actual-btn4").val() == '') {
            toastr.error("please select file first")
            $("#NDAcheckbox").prop("checked", false);
        }
    }
    else {
        if ($("#actual-btn4").val() != '') {
            $("#actual-btn4").val('');
            toastr.success("file removed form select")
        }
    }
})
$("#actual-btn5").on("change", function () {
    $("#LicenseDocumentcheckbox").prop("checked", true);
})
$("#LicenseDocumentcheckbox").on("change", function () {
    if ($(this).is(":checked")) {
        if ($("#actual-btn5").val() == '') {
            toastr.error("please select file first")
            $("#LicenseDocumentcheckbox").prop("checked", false);
        }
    }
    else {
        if ($("#actual-btn5").val() != '') {
            $("#actual-btn5").val('');
            toastr.success("file removed form select")
        }
    }
})

function PreviewImage() {
    var reader = new FileReader();
    reader.readAsDataURL(document.getElementById("ProviderPhoto").files[0]);
    reader.onload = function (e) {
        document.getElementById("loadphotohere").src = e.target.result;
    };
    $("#loadphotohere").prop("hidden", false);
};


$("#CreatePhysicianForm").on("submit", function (e) {

    $("#CreatePhysicianForm").validate();
    if ($("#CreatePhysicianForm").valid() != true) {
        return false;
    }
    e.preventDefault();
    var formData = new FormData($("#CreatePhysicianForm")[0]);
    var ids = [];
    //datas.append("AdminData", $("#AdminAspForm")[0]);
    $('input[type=checkbox]:checked').each(function () {
        formData.append("RegionList", $(this).attr("id"))
    });
    formData.append("CountryCode", $(".iti__selected-dial-code").text());
    //var serialdata = $(this).serialize();
    //formData.append("physician", serialdata);
    //formData.append("aspnetuser", serialdata);

    //formData.append("file1", $("#actual-btn1")[0].files[0]);
    //formData.append("file2", $("#actual-btn2")[0].files[0]);
    //formData.append("file3", $("#actual-btn3")[0].files[0]);
    //formData.append("file4", $("#actual-btn4")[0].files[0]);
    //formData.append("file5", $("#actual-btn5")[0].files[0]);

    $.ajax({
        url: '/AdminPartials/CreatePhysician',
        method: 'POST',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,

        success: function (response) {
            if (response) {
                toastr.success("Provider Created");
                //add navigation for both pages
                $("#Provider-drop-down").click();
            }
            else {
                toastr.error("something went wrong")
            }
        }
    });

});
var controller = "AdminPartials";
if (localStorage.getItem("CurrentAccount") == "Provider") {
    controller = "ProviderAccount";
}
// ResetProviderPass  aspID   ProviderPassword 
$("#ProviderPassword").on("keyup", function () {
    var regularExpression = /^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,16}$/;
    var status = regularExpression.test($("#ProviderPassword").val());
    if (!status) {
        $(".AdminpasswordValidation").html("Password must contains one upper letter , one lower case, minimum  length , one unique icon and one number ")
    }
    else {
        $(".AdminpasswordValidation").html("");
    }

});
$("#ResetProviderPass").click(function (e) {
    e.preventDefault();
    if ($("#ResetProviderPass").text() == "Save Changes") {
        //reset Password
        var regularExpression = /^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,16}$/;
        var status = regularExpression.test($("#ProviderPassword").val());
        if (status) {
            $.ajax({
                url: '/' + controller + '/ResetProviderPass',
                method: 'POST',
                data: {
                    password: $("#ProviderPassword").val(),
                    phyId: $("#aspID").val(),
                },
                success: function (response) {
                    if (response) {
                        toastr.success("password changed");
                    }
                    else {
                        toastr.error("Something Went Wrong!")
                    }
                    $("#ResetProviderPass").text("Reset Password");
                    $("#ProviderPassword").val("");
                    $("#ProviderPassword").attr("disabled", true);
                }
            });
        }
    }
    else {
        $("#ProviderPassword").removeAttr("disabled", true);
        $("#ResetProviderPass").html("Save Changes");
    }
});



