
$(document).ready(function () {

    //let SendAgrMobile = document.querySelector("#Send_Agr_PhoneNumber");
    //window.intlTelInput(SendAgrMobile, {
    //    utilsScript: "https://cdn.jsdelivr.net/npm/intl-tel-input@19.5.6/build/js/utils.js",
    //    //preferredCountries: ["in"],
    //    separateDialCode: true,
    //});

    $("#ViewCaseForm :input,#closeCaseForm :input").attr("disabled", true);
    $("#ViewCaseForm :button, #closeCaseForm :button").removeAttr("disabled", true);
    $("#Submitbtn,#CloseCaseSubmitbtn").hide();
    $(document).on('click', '#Editbtn , #CloseCaseEditbtn', function (event) {
        event.preventDefault();
        $("#Submitbtn,#CloseCaseSubmitbtn").show();
        $("#PatientMobile , #PatientEmail ").removeAttr("disabled", true);
        $("#Editbtn").hide();
    });

    $('#viewNotes').click(function (event) {
        event.preventDefault();
    });
    $('#Cancelbtn').click(function (event) {
        event.preventDefault();
        $("#ViewCaseForm :input").attr("disabled", true);
        $("#ViewCaseForm :button").removeAttr("disabled", true);
        $("#Editbtn").show();
        $("#Submitbtn").hide();
    });
});


$(document).ready(function () {
    const  input = document.querySelector(".Patientmobile");
    const input1script= window.intlTelInput(input, {
        utilsScript: "https://cdn.jsdelivr.net/npm/intl-tel-input@19.5.6/build/js/utils.js",
        separateDialCode: true,
    });

    $(".Patientmobile").on("keyup", function () {
        if (input1script.isValidNumber() == false) {
            $("#mobilePatientValidation").html("Phonenumber is not valid");
        }
        else {
            $("#mobilePatientValidation").html("");
        }
    });

    $('.myform,#closeCaseForm').on('submit', function (e) {
        e.preventDefault();
        $('.myform').validate();
        $("#closeCaseForm").validate()
        var formData = $(this).serializeArray();
        formData.push({ name: "PatientCountryCode", value: $(".iti__selected-dial-code").text() });

        if (input1script.isValidNumber() == false) {
            e.preventDefault();
            return false;
        }
        if ($(this).attr("id") == "closeCaseForm") {
            if ($("#closeCaseForm").valid()) {
                $.post('/ProviderAccount/EditUserData', formData, function (response) {
                    //show tostr
                    if (response) {
                        $(".Provider-data-container").load('/ProviderAccount/CloseCase?reqid=' + response);
                        toastr.success("Patient Data Changed!");
                    }
                    else {
                        toastr.error("Error Occured!");
                    }
                });
            }
        }
        else {
            if ($(".myform").valid()) {
                $.post('/ProviderAccount/ViewNewCasespost', formData, function (response) {
                    if (response.status) {
                        $(".Provider-data-container").load('/ProviderAccount/ViewNewCases?requestid=' + response.reqid, function () {
                        });
                        toastr.success("Patient Data Changed!");
                    }
                    else {
                        toastr.error("Error Occured!");
                    }
                });

            }
        }
        if ($(".myform ,#closeCaseForm ").valid()) {

            $("#ViewCaseForm :input , #closeCaseForm :input").attr("disabled", true);
            $("#ViewCaseForm :button,#closeCaseForm :button").removeAttr("disabled", true);
            $("#Submitbtn,#CloseCaseSubmitbtn").hide();
            $("#Editbtn , #CloseCaseEditbtn").show();
        }
    });

});